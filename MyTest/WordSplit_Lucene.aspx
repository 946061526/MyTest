<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WordSplit_Lucene.aspx.cs" Inherits="MyTest.WordSplit_Lucene" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        
    <div>
        <asp:TextBox ID="TextBox1" runat="server" TextMode="MultiLine" Height="158px" Width="382px" Text="1、颠覆传统学习模式：通过互联网平台进行在线授课、学习，将颠覆传统线下授课、学习方式。 "></asp:TextBox>
        <br />
    <asp:Button ID="Button1" runat="server" Text="分词" OnClick="Button1_Click" />
        <br /><br />
    </div>
        <asp:TextBox ID="TextBox2" runat="server" TextMode="MultiLine" Height="158px" Width="382px"></asp:TextBox>
    </form>
</body>
</html>
