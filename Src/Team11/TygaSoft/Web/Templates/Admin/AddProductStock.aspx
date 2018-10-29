<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddProductStock.aspx.cs" Inherits="TygaSoft.Web.Templates.Admin.AddProductStock" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>商品库存</title>
</head>
<body>
    <form id="dlgProductStockFm" runat="server">
        <div class="row mt10">
            <span class="rl">商品项：</span>
            <div class="fl">
                <input id="cbbProductItem" class="easyui-combobox" data-options="required:true,validType:'select', valueField:'id',textField:'text',url:'/h/t.html',queryParams:{reqName:'GetProductItemJsonForCombobox',productId:$('#hId').val()},onLoadSuccess:AddProductStock.OnProductItemLoadSuccess,onSelect:AddProductStock.OnCbbProductItemSelect,width:280" />
            </div>
            <div class="clr"></div>
        </div>
        <div class="row mt10">
            <span class="rl">尺码：</span>
            <div class="fl">
                <input id="cbbProductSize" class="easyui-combobox" data-options="valueField:'id',textField:'text',method:'get',onLoadSuccess:AddProductStock.OnProductSizeLoadSuccess,width:280" />
            </div>
            <div class="clr"></div>
        </div>
        <div class="row mt10">
            <span class="rl">库存：</span>
            <div class="fl">
                <input type="text" id="txtName" runat="server" clientidmode="Static" maxlength="9" class="easyui-validatebox" data-options="required:true,validType:'int'" />
            </div>
            <div class="clr"></div>
        </div>

        <input type="hidden" id="hProductStockId" runat="server" clientidmode="Static" />

        <asp:Literal ID="ltrMyData" runat="server"></asp:Literal>
    </form>
</body>
</html>
