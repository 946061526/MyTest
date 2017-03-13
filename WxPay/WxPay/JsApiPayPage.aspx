<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="JsApiPayPage.aspx.cs" Inherits="WxPay.JsApiPayPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="content-type" content="text/html;charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <script src="/assets/js/jquery-1.11.3.min.js"></script>
    <script src="http://res.wx.qq.com/open/js/jweixin-1.0.0.js"></script>
    <title>微信支付-JSAPI支付</title>
</head>

<script type="text/javascript">
    //调用微信JS api 支付
    function jsApiCall()
    {
                   
        WeixinJSBridge.invoke(
        'getBrandWCPayRequest',                  
       <%=wxJsApiParam%>,//josn串 
        //{
        //    "appId": "wxc439ae3d56500a65", //公众号名称，由商户传入  
        //    "nonceStr": "c1aa8e15b8f649f39d35fe22360837b2",//随机串  
        //    "package": "prepay_id=wx2015091815301185a44fffa20203788917",//商品包信息  
        //    "paySign": "EB50592BCCC79C089F944D3777FD8A3C",//微信签名  
        //    "signType": "MD5",//微信签名方式 
        //    "timeStamp": "1442561411"//时间戳，自 1970 年以来的秒数  
        //},
        function (res)
        {
            WeixinJSBridge.log(res.err_msg);
            //alert(res.err_code + res.err_desc + res.err_msg);
            if(res.err_msg == "get_brand_wcpay_request:ok" ) {  
                alert("pay is ok!");
                document.getElementById("payFail").style.display="none";  
                document.getElementById("paySuccess").style.display="block";  
            }  
            else{  
                document.getElementById("payFail").style.display="block";  
            }  
        }
        );


    }

    function callpay()
    {
        if (typeof WeixinJSBridge == "undefined")
        {
            if (document.addEventListener)
            {
                document.addEventListener('WeixinJSBridgeReady', jsApiCall, false);
            }
            else if (document.attachEvent)
            {
                document.attachEvent('WeixinJSBridgeReady', jsApiCall);
                document.attachEvent('onWeixinJSBridgeReady', jsApiCall);
            }
        }
        else
        {
            jsApiCall();
        }
    }
               
</script>

<body>
    <form runat="server">
        <br />
        <div align="center">
            <br />
            <br />
            <br />
            <asp:Button ID="submit" runat="server" Text="立即支付" OnClientClick="callpay()" Style="width: 210px; height: 50px; border-radius: 15px; background-color: #00CD00; border: 0px #FE6714 solid; cursor: pointer; color: white; font-size: 16px;" />
        </div>
        <dl class="item_desc" id="dlPayInfo">
            <dt class="item_title"><span class="txt">支付信息</span></dt>
            <dd class="item_panel pl1">
                <%--<span class="title"><%=this.OrderDetail.ProductNames %></span>--%>  
            </dd>
            <dd class="item_panel pl1">
                <%--<span class="title">订单号：</span><span class="selected com_c4"><%=this.OrderID %></span>--%>  
            </dd>
            <dd class="item_select item_last">
                <div class="title">
                    <%--<strong>应付金额： </strong><span class="com_c4"><%=OrderDetail.TotalOrderAmount + OrderDetail.TotalLogisticsFee + OrderDetail.OrderAmountRevise + OrderDetail.LogisticsFeeRevise %> 元</span>--%>
                </div>
            </dd>
        </dl>
        <br />
        <dd class="item_select" id="paySuccess" style="display: none">
            <div class="title" style="padding-left: 3rem">
                <img src="/images/onSuccess.gif" title="支付成功" />
                <span class="selected"><b class="big">支付成功!</b><br />
                    系统正在处理收款，订单状态可能会有1-5分钟的滞后，感谢您的耐心等待。<br />
                    <a href="/member/order/info1/">查看订单</a><br />
                    <br />
                    您还可以：<a href="/">继续购物</a><br />
                    <br />
                    <br />
                    <br />
                </span>
            </div>
        </dd>
        <dd class="item_select" id="payFail" style="display: none">
            <div class="title item" style="padding-left: 2.0rem">
                <span class="selected"><b class="big">支付失败!</b></span>
                <div class="btns" style="margin-top: 10px"><a class="com_btn_7" style="padding: 1px; cursor: pointer;" onclick="callpay()">重新支付</a></div>
            </div>
            <div class="title item" style="padding-left: 2.0rem">
                <span class="selected">您可以查看微信交易记录。如果没有扣费，可以稍后尝试重新支付。如果已经扣费，请拨打客服电话联系我们  
                <br />
                    <br />
                    <%--如果多次重新支付还是遇到了问题，稍后您可以在登陆状态下，访问个人中心-><a href="/member/order/">我的订单</a>，为编号为<%=this.OrderID %>的订单付款，有多种支付方式供您选择--%>    
                ...  
                </span>
            </div>
        </dd>
    </form>
</body>
</html>
