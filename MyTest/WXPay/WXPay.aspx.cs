using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace MyTest.WXPay
{
    /// <summary>
    /// 根据Code获取OpenID等信息
    /// </summary>
    public class ModelOpenID
    {
        public string access_token { get; set; }
        public string expires_in { get; set; }
        public string refresh_token { get; set; }
        public string openid { get; set; }
        public string scope { get; set; }
    }

    public partial class WXPay : System.Web.UI.Page
    {
        public static string tenpay = "1";  //人民币
        public static string partnerid = "111111111111"; //PartnerID
        public static string partnerkey = "111111111111111111111111111111111111"; //PartnerKey
        public static string mchid = "1255341401"; //mchid
        public static string appId = "wxc439ae3d56500a65"; //appid
        public static string appsecret = "6ccc883a3a3b12ef1d8a5d5d24f58524"; //appsecret
        public static string appkey = "Lxuey130Wj668685Tmqceduwj668685M"; //paysignkey(非appkey 在微信商户平台设置 (md5)111111111111) 
        public static string timeStamp = ""; //时间戳 
        public static string nonceStr = ""; //随机字符串 

        public static string notify_url = "http:/111111111111.aspx"; //支付完成后的回调处理页面,*替换成notify_url.asp所在路径

        public static string code = "";     //微信端传来的code
        public static string prepayId = "";     //预支付ID
        public static string sign = "";     //为了获取预支付ID的签名
        public static string paySign = "";  //进行支付需要的签名
        public static string package = "";  //进行支付需要的包

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["code"] != null)
            {
                code = Request.QueryString["code"];
                string url =
                    string.Format(
                        "https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code",
                        appId, appsecret, code);
                string returnStr = HttpUtil.Send("", url);

                var obj = JsonConvert.DeserializeObject<ModelOpenID>(returnStr);

                url = string.Format(
                    "https://api.weixin.qq.com/sns/oauth2/refresh_token?appid={0}&grant_type=refresh_token&refresh_token={1}",
                    appId, obj.refresh_token);
                returnStr = HttpUtil.Send("", url);
                obj = JsonConvert.DeserializeObject<ModelOpenID>(returnStr);

                WriteFile(Server.MapPath("") + "\\Log.txt", obj.access_token);
                WriteFile(Server.MapPath("") + "\\Log.txt", obj.openid);

                url = string.Format(
                    "https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}",
                    obj.access_token, obj.openid);
                returnStr = HttpUtil.Send("", url);
                WriteFile(Server.MapPath("") + "\\Log.txt", returnStr);

                ///////////////////////////////////////////////////////////////////////////////////////////////

                string sp_billno = Request["order_no"];
                //当前时间 yyyyMMdd
                string date = DateTime.Now.ToString("yyyyMMdd");

                if (null == sp_billno)
                {
                    //生成订单10位序列号，此处用时间和随机数生成，商户根据自己调整，保证唯一
                    sp_billno = DateTime.Now.ToString("HHmmss") + TenpayUtil.BuildRandomStr(4);
                }
                else
                {
                    sp_billno = Request["order_no"];
                }

                sp_billno = partnerid + sp_billno;


                //创建支付应答对象
                var packageReqHandler = new RequestHandler(Context);
                //初始化
                packageReqHandler.init();

                timeStamp = TenpayUtil.getTimestamp();
                nonceStr = TenpayUtil.getNoncestr();


                //设置package订单参数

                packageReqHandler.setParameter("body", "test"); //商品信息 127字符
                packageReqHandler.setParameter("appid", appId);
                packageReqHandler.setParameter("mch_id", mchid);
                packageReqHandler.setParameter("nonce_str", nonceStr.ToLower());
                packageReqHandler.setParameter("notify_url", notify_url);
                packageReqHandler.setParameter("openid", obj.openid);
                packageReqHandler.setParameter("out_trade_no", sp_billno); //商家订单号
                packageReqHandler.setParameter("spbill_create_ip", Page.Request.UserHostAddress); //用户的公网ip，不是商户服务器IP
                packageReqHandler.setParameter("total_fee", "1"); //商品金额,以分为单位(money * 100).ToString()
                packageReqHandler.setParameter("trade_type", "JSAPI");

                //获取package包
                sign = packageReqHandler.CreateMd5Sign("key", appkey);
                WriteFile(Server.MapPath("") + "\\Log.txt", sign);
                packageReqHandler.setParameter("sign", sign);

                string data = packageReqHandler.parseXML();

                WriteFile(Server.MapPath("") + "\\Log.txt", data);

                string prepayXml = HttpUtil.Send(data, "https://api.mch.weixin.qq.com/pay/unifiedorder");

                WriteFile(Server.MapPath("") + "\\Log.txt", prepayXml);

                //获取预支付ID
                var xdoc = new XmlDocument();
                xdoc.LoadXml(prepayXml);
                XmlNode xn = xdoc.SelectSingleNode("xml");
                XmlNodeList xnl = xn.ChildNodes;
                if (xnl.Count > 7)
                {
                    prepayId = xnl[7].InnerText;
                    package = string.Format("prepay_id={0}", prepayId);
                    WriteFile(Server.MapPath("") + "\\Log.txt", package);
                }

                //设置支付参数
                var paySignReqHandler = new RequestHandler(Context);
                paySignReqHandler.setParameter("appId", appId);
                paySignReqHandler.setParameter("timeStamp", timeStamp);
                paySignReqHandler.setParameter("nonceStr", nonceStr);
                paySignReqHandler.setParameter("package", package);
                paySignReqHandler.setParameter("signType", "MD5");
                paySign = paySignReqHandler.CreateMd5Sign("key", appkey);


                WriteFile(Server.MapPath("") + "\\Log.txt", paySign);
            }
        }

        public static void WriteFile(string pathWrite, string content)
        {
            if (File.Exists(pathWrite))
            {
                //File.Delete(pathWrite);
            }
            File.AppendAllText(pathWrite, content + "\r\n----------------------------------------\r\n",
                Encoding.GetEncoding("utf-8"));
        }
    }
}