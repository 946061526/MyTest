<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Test1.aspx.cs" Inherits="MyTest.Test1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script src="js/jquery190.js"></script>

</head>
<body>    

    <form id="form1" runat="server">
    <div>auth:<input type="text" id="auth" runat="server" />
        <br />
        <asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click" />

    </div>


        <input type="hidden" id="hidIsBuyNext" runat="server" name="isBuyNext" value="0" />
    </form>
</body>
</html>
