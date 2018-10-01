<%@ Page Title="新建商品" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AddProduct.aspx.cs" Inherits="TygaSoft.Web.Admin.ECShop.AddProduct" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

  <link href="../../Scripts/plugins/kindeditor/themes/default/default.css" rel="stylesheet" type="text/css" />
  <link href="../../Scripts/plugins/kindeditor/plugins/code/prettify.css" rel="stylesheet" type="text/css" /> 
  <script type="text/javascript" src="../../Scripts/JeasyuiExtend.js"></script>
  <script type="text/javascript" src="../../Scripts/plugins/kindeditor/kindeditor.js"></script>
  <script type="text/javascript" src="../../Scripts/plugins/kindeditor/lang/zh_CN.js"></script>
  <script type="text/javascript" src="../../Scripts/plugins/kindeditor/plugins/code/prettify.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">

<div class="easyui-panel" title="基本信息" style="padding:10px;">
    <div class="fl">
        <div class="row">
            <span class="rl"><b class="cr">*</b>商品名称：</span>
            <div class="fl">
                <input type="text" id="txtProductName" runat="server" clientidmode="Static" class="easyui-textbox" data-options="required:true" style="width:500px;" />
            </div>
            <span class="clr"></span>
        </div>
        <div class="row mt10">
            <span class="rl">副标题：</span>
            <div class="fl">
                <input id="txtSubTitle" runat="server" clientidmode="Static" class="easyui-textbox" style="width:500px;" />
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
            <span class="rl">库存：</span>
            <div class="fl">
                <input id="txtStockNum" runat="server" clientidmode="Static" class="easyui-textbox" data-options="validType:'int'" />
            </div>
            <div class="clr"></div>
        </div>
        <div class="row mt10">
            <span class="rl">所属分类：</span>
            <div class="fl">
                <select id="cbtCategory" class="easyui-combotree" style="width:200px;"
                    data-options="url:'../../Handlers/Admin/ECShop/HandlerCategory.ashx?reqName=GetCategoryTreeJson',method:'get',onLoadSuccess:AddProduct.OnCategoryTreeLoadSuccess">
                </select>
            </div>
            <div class="clr"></div>
        </div>
        <div class="row mt10">
            <span class="rl">所属品牌：</span>
            <div class="fl">
                <select id="cbtBrand" class="easyui-combotree" style="width:200px;"
                    data-options="url:'../../Handlers/Admin/ECShop/HandlerBrand.ashx?reqName=GetBrandTreeJson',method:'get',onLoadSuccess:AddProduct.OnBrandTreeLoadSuccess">
                </select>
            </div>
            <div class="clr"></div>
        </div>
        <div class="row mt10">
            <span class="rl">展示区：</span>
            <div class="fl">
                <select id="cbbMenu" class="easyui-combobox" style="width:200px;"
                    data-options="url:'../../Handlers/Admin/HandlerSysEnum.ashx?reqName=GetJsonForCombobox&enumCode=CustomMenu',method:'get',valueField:'id',textField:'text',required:true">
                </select>
            </div>
            <div class="clr"></div>
        </div>
    </div>
    <div class="fr">
        <div class="easyui-panel" title="商品主图片" style="padding:10px;width:250px; height:218px;">
            <img id="imgProduct" src="../../Images/nopic.gif" alt="上传图片" width="227px" height="143px" onclick="ProductPicture.DlgSingle()" />
            <input type="hidden" />
             <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true" onclick="ProductPicture.DlgSingle()">选 择</a>
        </div>
    </div>
    <span class="clr"></span>
</div>

<div class="mt10"></div>
<div class="easyui-panel" title="商品其他图片" style="padding:10px;">
    <div class="fl mr10">
        <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true" onclick="ProductPicture.DlgMulti()">选 择</a>
    </div>
    <div id="imgProductOther" class="fl">
        <div class="row_col w110" style="display:none;">
            <img src="" alt="" width="110px" height="110px" />
            <input type="hidden" />
            <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-remove',plain:true" onclick="$(this).parent().remove()">删 除</a>
        </div>
    </div>
    <span class="clr"></span>
</div>

<div class="mt10"></div>
<div class="easyui-panel" title="尺码" style="padding:10px;">
    <div class="fl mr10">
        <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true" onclick="SizePicture.AddInput()">添 加</a>
    </div>
    <div id="sizeBox" class="fl" style="width:55%;">
        <div class="row_col mb10" style="display:none;">
            <input type="text" class="w80" /><br />
            <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-remove',plain:true" onclick="$(this).parent().remove()">删 除</a>
        </div>
    </div>
    <div class="fr">
        <div class="easyui-panel" title="尺码表" style="padding:10px;width:400px;">
            <img id="imgSize" src="../../Images/nopic.gif" alt="上传图片" width="376px" height="120px" onclick="SizePicture.DlgSingle()" />
             <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true" onclick="SizePicture.DlgSingle()">选 择</a>
        </div>
    </div>
    <span class="clr"></span>
</div>

<div class="mt10"></div>
<div class="easyui-panel" title="商品其他信息" style="padding:10px;">
    <div class="row mt10">
        <span class="fl rl"><b class="cr">*</b>商品描述：</span>
        <div class="fl">
            <textarea id="txtContent" runat="server" clientidmode="Static" cols="100" rows="8" style="width:800px;height:800px;"></textarea>
        </div>
        <div class="clr"></div>
    </div>
    <div class="row mt10">
        <span class="fl rl">&nbsp;</span>
        <div class="fl">
        <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-save'" onclick="AddProduct.OnSave()">提交</a>
        </div>
        <span class="clr"></span>
    </div>
</div>

<asp:Literal runat="server" ID="ltrMyData"></asp:Literal>

<div id="dlgProductPicture"></div>
<div id="dlgSizePicture"></div>

<input type="hidden" id="hId" runat="server" clientidmode="Static" name="hId" />
<script type="text/javascript" src="../../Scripts/Admin/ECShop/AddProduct.js"></script>
<script type="text/javascript" src="../../Scripts/Admin/ECShop/ProductPicture.js"></script>
<script type="text/javascript" src="../../Scripts/Admin/ECShop/SizePicture.js"></script>

<script type="text/javascript">
    var editor_content;
    KindEditor.ready(function (K) {
        editor_content = K.create('#txtContent', {
            uploadJson: '../../Handlers/Admin/HandlerKindeditor.ashx',
            fileManagerJson: '../../Handlers/Admin/HandlerKindeditor.ashx',
            allowFileManager: true,
            afterCreate: function () {
                var self = this;
                K.ctrl(document, 13, function () {
                });
                K.ctrl(self.edit.doc, 13, function () {
                });
            }
        });
        prettyPrint();

    });

    $(function () {
        try {
            AddProduct.Init();
            ProductPicture.CreateDlgProductPicture();
            SizePicture.CreateDlgSizePicture();
        }
        catch (e) {
            $.messager.alert('错误提醒', e.name + ": " + e.message, 'error');
        }

    })

</script>

</asp:Content>
