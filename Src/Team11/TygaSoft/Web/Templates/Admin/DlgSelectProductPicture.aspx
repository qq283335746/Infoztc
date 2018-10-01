<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DlgSelectProductPicture.aspx.cs" Inherits="TygaSoft.Web.Templates.Admin.DlgSelectProductPicture" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="dlgPictureFm" runat="server">
        <div class="easyui-layout" data-options="border:false, width:760,height:$(window).height()*0.9">
            <div id="pictureBox" data-options="region:'center',title:'',border:false,fit:true">
                <asp:Repeater ID="rpData" runat="server">
                    <ItemTemplate>
                        <div class="row_col w120">
                            <img src='<%#Eval("MPicture")%>' alt="图片" width="120" height="100" code='<%#Eval("Id")%>' />
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
            <div data-options="region:'south',title:'',border:false," style="height:50px;">
                <div id="easyPager"></div>
            </div>
        </div>
        <asp:Literal runat="server" ID="ltrMyData"></asp:Literal>
    </form>
</body>
</html>
