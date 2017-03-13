<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="test.aspx.cs" Inherits="MyTest.VcCode.test" %>
<%@ Register Src="~/VcCode/VcCodeHtml.ascx" TagPrefix="ascx" TagName="VcCodeHtml" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="VcCode.css" rel="stylesheet" />
    <script src="../js/jquery190.js"></script>
    <script src="../js/jquery-ui.source.js"></script>
    <script src="../js/jquery.ui.touch-punch.source.js"></script>
    <script src="../js/pageDialog.js"></script>
    <script src="../js/BottomFun.js"></script>
    <script src="vcCodeUtil.js"></script>
    <script>
        var showVcCode = function () {
            var that = this;
            //在这个时机需要验证码弹框
            var _Dialog = null;
            var _VcDialogHtml = $("#vcCodeHtmlSectionTmpl").html();
            _Dialog = new $.PageDialog(_VcDialogHtml, {
                W: 300,
                H: 300,
                autoClose: false,
                ready: function () {
                    var dialogReadyFunOption = {
                        $dialogObj: $("#pageDialog"),
                        key: 'test',
                        canvasClickTooManyCallback: function () {
                            _Dialog.cancel();
                            $.PageDialog.fail1(vcCodeUtil.reqVcCodeToomany);
                            return false;
                        },
                        canvasClickRightCallback: function (auth) {
                            _Dialog.cancel();
                            //that.sendMsgOrEmail(auth);
                            alert('sendmsg')
                        }
                    };
                    vcCodeUtil.dialogReadyFun(dialogReadyFunOption);
                },
                isTop: false
            });

            $("div.vc-close-btn").click(function () {
                _Dialog.cancel();
            });
        }
        
    </script>

</head>
<body>
    <ascx:VcCodeHtml runat="server" ID="VcCodeHtml" />
    <form id="form1" runat="server">
    <div>
        <input type="button" value="验证码" onclick="showVcCode();" />
    </div>
    </form>
</body>
</html>
