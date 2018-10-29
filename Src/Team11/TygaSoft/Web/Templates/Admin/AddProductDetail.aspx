<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddProductDetail.aspx.cs" Inherits="TygaSoft.Web.Templates.Admin.AddProductDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>商品详情</title>
</head>
<body>
    <form id="dlgProductDetailFm" runat="server">
        <div class="row mt10">
            <span class="rl">商品项：</span>
            <div class="fl">
                <asp:DropDownList ID="ddlProductItem" runat="server" CssClass="easyui-validatebox" data-options="required:true,validType:'select'"></asp:DropDownList>
            </div>
            <div class="clr"></div>
        </div>
        <div class="row mt10">
            <span class="rl">原价：</span>
            <div class="fl">
                <input id="txtOriginalPrice" runat="server" clientidmode="Static" class="easyui-textbox" data-options="validType:'price'" />
            </div>
            <div class="clr"></div>
        </div>
        <div class="row mt10">
            <span class="rl">现价：</span>
            <div class="fl">
                <input id="txtProductPrice" runat="server" clientidmode="Static" class="easyui-textbox" data-options="validType:'price'" />
            </div>
            <div class="clr"></div>
        </div>
        <div class="row mt10">
            <span class="rl">折扣率：</span>
            <div class="fl">
                <input id="txtDiscount" runat="server" clientidmode="Static" class="easyui-textbox" data-options="validType:'float'" />
            </div>
            <div class="clr"></div>
        </div>
        <div class="row mt10">
            <span class="rl">折扣说明：</span>
            <div class="fl">
                <input id="txtDiscountDescri" runat="server" clientidmode="Static" class="easyui-textbox" />
            </div>
            <div class="clr"></div>
        </div>
        <div class="row mt10">
            <span class="fl rl"><b class="cr">*</b>商品描述：</span>
            <div class="fl">
                <textarea id="txtaContent" name="txtaContent" runat="server" clientidmode="Static" cols="100" rows="8" style="width:800px;height:800px;"></textarea>
            </div>
            <div class="clr"></div>
        </div>
        <input type="hidden" id="hAction_ProductDetail" runat="server" clientidmode="Static" value="add" />
    </form>

    <script type="text/javascript">
        
        var editor_content;
        editor_content = KindEditor.create('textarea[name=txtaContent]', {
            cssPath: '/Scripts/plugins/kindeditor/plugins/code/prettify.css',
            width: '1000px',
            height: '800px',
            uploadJson: '/h/ty.html',
            fileManagerJson: '/h/ty.html',
            allowFileManager: true,
            afterCreate: function () {
                var self = this;
                KindEditor.ctrl(document, 13, function () {
                });
                KindEditor.ctrl(self.edit.doc, 13, function () {
                });
            }
        });
    </script>
</body>
</html>
