<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="HttpTest.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h3>Http请求</h3>
        <p>Method：<asp:RadioButton ID="rdPost" runat="server" Text="POST" GroupName="method" Checked="true" /><asp:RadioButton ID="rdGet" runat="server" Text="GET" GroupName="method" /></p>
        <p>请求地址：<asp:TextBox ID="txtUrl" runat="server" Width="500" Text="http://json.1yyg.com/android/index"></asp:TextBox></p>
        <p>头部信息：<asp:TextBox ID="txtHeader" runat="server" Width="500"></asp:TextBox></p>
        <p>发送数据：<asp:TextBox ID="txtData" runat="server" Width="500" Height="200" Text="action=serviceTimSpan&time=2017-08-01 01:01:00"></asp:TextBox></p>
        <%--<p>附带文件：<asp:TextBox ID="txtFile1" runat="server" Width="200"></asp:TextBox></p>--%>
        <p style="padding-left:500px">
            <asp:Button ID="btnSubmit" runat="server" Text="请求" OnClick="btnSubmit_Click" /></p>
        <p>返回数据：<asp:TextBox ID="txtResult" runat="server" Width="500" Height="200" Text=""></asp:TextBox></p>
    </div>
    </form>
</body>
</html>
