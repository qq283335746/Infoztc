<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddAdItemLinkProduct.aspx.cs" Inherits="TygaSoft.Web.Templates.Admin.AboutSite.AddAdItemLinkProduct" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
      <link href="../../Scripts/plugins/kindeditor/themes/default/default.css" rel="stylesheet" type="text/css" />
      <link href="../../Scripts/plugins/kindeditor/plugins/code/prettify.css" rel="stylesheet" type="text/css" /> 
      <script src="../../Scripts/JeasyuiExtend.js" type="text/javascript"></script>
      <script type="text/javascript" src="../../Scripts/plugins/kindeditor/kindeditor.js"></script>
      <script type="text/javascript" src="../../Scripts/plugins/kindeditor/lang/zh_CN.js"></script>
      <script type="text/javascript" src="../../Scripts/plugins/kindeditor/plugins/code/prettify.js"></script>
</head>
<body>
    <form id="dlgFmAdItemLinkProduct" runat="server">
    <div>
        <table id="dgProduct" class="easyui-datagrid" title="商品列表" data-options="rownumbers:true,pagination:true,fit:true,fitColumns:true">
            <thead>
                <tr>
                    <th data-options="field:'f0',checkbox:true"></th>
                    <th data-options="field:'f1',width:60">商品图片</th>
                    <th data-options="field:'f1',width:60">商品名称</th>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater ID="rpData" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td>1</td>
                            <td>
                                <img src="../../../Images/nopic.gif" id="imgPictureProduct" runat="server" clientidmode="Static" alt="" width="110" height="110" />
                            </td>
                            <td>士大夫士大夫</td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
    </div>
    </form>

    <script type="text/javascript" src="../../../Scripts/Admin/AboutSite/AddAdItemLink.js"></script>
    <script type="text/javascript">
        $(function () {
            AddAdItemLink.Init();
        })
    </script>
</body>
</html>
