using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WxPay
{
    public partial class JsApiPayPage : System.Web.UI.Page
    {
        public static string wxJsApiParam { get; set; } //H5调起JS API参数
        //微信APPID
        private string APPID = "wxc439ae3d56500a65";
        //密钥
        private string appsecret = "6ccc883a3a3b12ef1d8a5d5d24f58524";
        protected void Page_Load(object sender, EventArgs e)
        {
            Log.Info(this.GetType().ToString(), "（JsApiPayPage.aspx）page load");
            if (!IsPostBack)
            {
                string openid = Request.QueryString["openid"];
                string total_fee = Request.QueryString["total_fee"];

                // Log.Info(this.GetType().ToString(), "openid--" + openid);
                // Log.Info(this.GetType().ToString(), "total_fee--" + total_fee);
                //检测是否给当前页面传递了相关参数
                if (string.IsNullOrEmpty(openid) || string.IsNullOrEmpty(total_fee))
                {
                    Response.Write("<span style='color:#FF0000;font-size:20px'>" + "页面传参出错,请返回重试" + "</span>");
                    //Log.Error(this.GetType().ToString(), "This page have not get params, cannot be inited, exit...");
                    //submit.Visible = false;
                    return;
                }

                //若传递了相关参数，则调统一下单接口，获得后续相关接口的入口参数
                JsApiPay jsApiPay = new JsApiPay(this);
                jsApiPay.total_fee = Convert.ToInt32(total_fee) * 100;//单位是分，不能有小数                 
                jsApiPay.openid = openid;
                jsApiPay.total_fee = int.Parse(total_fee);

                //JSAPI支付预处理
                try
                {
                    WxPayData unifiedOrderResult = jsApiPay.GetUnifiedOrderResult();
                    wxJsApiParam = jsApiPay.GetJsApiParameters();//获取H5调起JS API参数                    
                    Log.Debug(this.GetType().ToString(), "wxJsApiParam : " + wxJsApiParam);
                    //在页面上显示订单信息
                    // Response.Write("<span style='color:#00CD00;font-size:20px'>订单详情：</span><br/>");
                    // Response.Write(unifiedOrderResult.ToPrintStr());

                }
                catch (Exception ex)
                {
                    Response.Write("<span style='color:#FF0000;font-size:20px'>" + "下单失败，请返回重试" + "</span>");
                    //submit.Visible = false;
                }
            }
        }
    }
}