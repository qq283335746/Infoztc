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

    <div id="tabProduct" class="easyui-tabs" data-options="fit:true">
        <div title="商品" style="padding:20px;">
            <div class="fl">
                <div class="row">
                    <span class="rl"><b class="cr">*</b>名称：</span>
                    <div class="fl">
                        <input type="text" id="txtName" runat="server" clientidmode="Static" class="easyui-textbox" data-options="required:true" style="width:500px;" />
                    </div>
                    <span class="clr"></span>
                </div>
                <div class="row mt10">
                    <span class="rl">排序：</span>
                    <div class="fl">
                        <input id="txtSort" runat="server" clientidmode="Static" class="easyui-textbox" data-options="validType:'int'" />
                    </div>
                    <div class="clr"></div>
                </div>
                <div class="row mt10">
                    <span class="rl">上下架时间：</span>
                    <div class="fl">
                        <input id="txtEnableStartTime" runat="server" clientidmode="Static" class="easyui-datetimebox" />
                    </div>
                    <div class="fl mlr10"> 至 </div>
                    <div class="fl">
                        <input id="txtEnableEndTime" runat="server" clientidmode="Static" class="easyui-datetimebox" />
                    </div>
                    <span class="clr"></span>
                </div>
                <div class="row mt10">
                    <span class="rl">上下架：</span>
                    <div class="fl">
                        <input type="radio" name="rdIsEnable" value="false" checked="checked" /><label>上架</label>
                        <input type="radio" name="rdIsEnable" value="true" /><label>下架</label>
                    </div>
                    <span class="clr"></span>
                </div>
                <div class="row mt10">
                    <span class="rl">是否禁用：</span>
                    <div class="fl">
                        <input type="radio" name="rdIsDisable" value="false" checked="checked" /><label>否</label>
                        <input type="radio" name="rdIsDisable" value="true" /><label>是</label>
                    </div>
                    <span class="clr"></span>
                </div>
                <div class="row mt10">
                    <span class="rl">所属分类：</span>
                    <div class="fl">
                        <select id="cbtCategory" class="easyui-combotree" style="width:200px;"
                            data-options="url:'/h/a.html?reqName=GetCategoryTreeJson',method:'get',onLoadSuccess:AddProduct.OnCategoryTreeLoadSuccess">
                        </select>
                    </div>
                    <div class="clr"></div>
                </div>
                <div class="row mt10">
                    <span class="rl">所属品牌：</span>
                    <div class="fl">
                        <select id="cbtBrand" class="easyui-combotree" style="width:200px;"
                            data-options="url:'/h/g.html?reqName=GetBrandTreeJson',method:'get',onLoadSuccess:AddProduct.OnBrandTreeLoadSuccess">
                        </select>
                    </div>
                    <div class="clr"></div>
                </div>
                <div class="row mt10">
                    <span class="rl">展示区：</span>
                    <div class="fl">
                        <select id="cbtMenu" class="easyui-combotree" style="width:200px;"
                            data-options="url:'/h/tt.html?reqName=GetTreeJsonByParentCode&parentCode=CustomMenu',method:'get',onLoadSuccess:AddProduct.OnMenuTreeLoadSuccess">
                        </select>
                    </div>
                    <div class="clr"></div>
                </div>
            </div>
            <div class="fr">
                <div class="easyui-panel" title="图片" style="padding:10px;width:250px; height:218px;">
                    <img id="imgProductPicture" runat="server" clientidmode="Static" src="../../Images/nopic.gif" alt="上传图片" width="227" height="143" onclick="AddProduct.OnPictureClick()" />
                    <input type="hidden" id="hImgProductPictureId" runat="server" clientidmode="Static" />
                </div>
            </div>
            <span class="clr"></span>
            <div class="row mt10">
                <span class="fl rl">&nbsp;</span>
                <div class="fl">
                <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-save'" onclick="AddProduct.OnSave()">保存</a>
                </div>
                <span class="clr"></span>
            </div>

            <asp:Literal runat="server" ID="ltrMyData"></asp:Literal>

            <div id="dlgProductPicture"></div>
            <div id="dlgSizePicture"></div>
            <div id="dlgUploadProductPicture" style="padding:10px;"></div>
            <div id="dlgUploadSizePicture" style="padding:10px;"></div>
        </div>
        <div title="商品项" style="overflow:auto;padding:20px;">

            <div id="dgProductItemToolbar" style="padding:5px;">
                <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true" onclick="AddProductItem.Add()">新 增</a>
                <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-edit',plain:true" onclick="AddProductItem.Edit()">编 辑</a>
                <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-remove',plain:true" onclick="AddProductItem.Del()">删 除</a>
            </div>
            <table id="dgProductItem" class="easyui-datagrid" title="已添加列表"
                    data-options="rownumbers:true,pagination:true,fit:true,fitColumns:true,toolbar:'#dgProductItemToolbar',url:'/h/t.html',queryParams:{reqName:'GetProductItemJsonForDatagrid', productId:$('#hId').val()}">
                <thead>
                    <tr>
                        <th data-options="field:'ProductId',hidden:true">名称</th>
                        <th data-options="field:'Id',checkbox:true"></th>
                        <th data-options="field:'Named',width:200">名称</th>
                        <th data-options="field:'SPicture',width:100,formatter:AddProduct.FImageUrl">图片</th>
                        <th data-options="field:'Sort',width:50">排序</th>
                        <th data-options="field:'EnableStartTime',width:100">上下架开始时间</th>
                        <th data-options="field:'EnableEndTime',width:100">上下架结束时间</th>
                        <th data-options="field:'IsEnable',width:100,formatter:AddProduct.FBool">是否上架</th>
                        <th data-options="field:'IsDisable',width:100,formatter:AddProduct.FBool">是否禁用</th>
                    </tr>
                </thead>
            </table>

            <div id="dlgProductItem" style="padding:10px;"></div>

        </div>
        <div title="商品详情" style="overflow:auto;padding:20px;">

            <div id="dgProductDetailToolbar" style="padding:5px;">
                <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true" onclick="AddProductDetail.Add()">新 增</a>
                <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-edit',plain:true" onclick="AddProductDetail.Edit()">编 辑</a>
                <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-remove',plain:true" onclick="AddProductDetail.Del()">删 除</a>
            </div>
            <table id="dgProductDetail" class="easyui-datagrid" title="已添加列表"
                    data-options="rownumbers:true,pagination:true,fit:true,fitColumns:true,toolbar:'#dgProductDetailToolbar',url:'/h/t.html',queryParams:{reqName:'GetProductDetailJsonForDatagrid', productId:$('#hId').val()}">
                <thead>
                    <tr>
                        <th data-options="field:'ProductId',hidden:true"></th>
                        <th data-options="field:'ProductItemId',checkbox:true"></th>
                        <th data-options="field:'ProductItemName',width:100">所属商品项</th>
                        <th data-options="field:'OriginalPrice',width:100">原价</th>
                        <th data-options="field:'ProductPrice',width:100">现价</th>
                        <th data-options="field:'Discount',width:100">折扣率</th>
                        <th data-options="field:'DiscountDescr',width:100">折扣说明</th>
                    </tr>
                </thead>
            </table>

            <div id="dlgProductDetail" style="padding:10px;"></div>

        </div>
        <div title="自定义属性" style="overflow:auto;padding:20px;">

            <div id="dgProductAttrToolbar" style="padding:5px;">
                <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true" onclick="AddProductAttr.Add()">新 增</a>
                <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-edit',plain:true" onclick="AddProductAttr.Edit()">编 辑</a>
                <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-remove',plain:true" onclick="AddProductAttr.Del()">删 除</a>
            </div>
            <table id="dgProductAttr" class="easyui-datagrid" title="已添加列表"
                    data-options="rownumbers:true,pagination:true,fit:true,fitColumns:true,toolbar:'#dgProductAttrToolbar',url:'/h/t.html',queryParams:{reqName:'GetProductAttrJsonForDatagrid', productId:$('#hId').val()}">
                <thead>
                    <tr>
                        <th data-options="field:'ProductItemId',checkbox:true"></th>
                        <th data-options="field:'ProductId',hidden:true"></th>
                        <th data-options="field:'ProductItemName',width:300">商品项</th>
                        <th data-options="field:'AttrValue',width:800">值</th>
                    </tr>
                </thead>
            </table>

            <div id="dlgProductAttr" style="padding:10px;"></div>

        </div>
        <div title="尺码表" style="overflow:auto;padding:20px;">

            <div id="dgProductSizePictureToolbar" style="padding:5px;">
                <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true" onclick="AddProductSizePicture.Add()">新 增</a>
                <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-edit',plain:true" onclick="AddProductSizePicture.Edit()">编 辑</a>
                <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-remove',plain:true" onclick="AddProductSizePicture.Del()">删 除</a>
            </div>
            <table id="dgProductSizePicture" class="easyui-datagrid" title="已添加列表"
                    data-options="rownumbers:true,pagination:true,fit:true,fitColumns:true,toolbar:'#dgProductSizePictureToolbar',url:'/h/t.html',queryParams:{reqName:'GetProductSizePictureJsonForDatagrid', productId:$('#hId').val()}">
                <thead>
                    <tr>
                        <th data-options="field:'ProductId',hidden:true"></th>
                        <th data-options="field:'ProductItemId',hidden:true"></th>
                        <th data-options="field:'Named',width:100">名称</th>
                        <th data-options="field:'SPicture',width:100,formatter:AddProduct.FImageUrl">图片</th>
                    </tr>
                </thead>
            </table>

            <div id="dlgProductSizePicture" style="padding:10px;"></div>

        </div>
        <div title="尺码" style="overflow:auto;padding:20px;">
            <div id="dgProductSizeToolbar" style="padding:5px;">
                <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true" onclick="AddProductSize.Add()">新 增</a>
                <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-edit',plain:true" onclick="AddProductSize.Edit()">编 辑</a>
                <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-remove',plain:true" onclick="AddProductSize.Del()">删 除</a>
            </div>
            <table id="dgProductSize" class="easyui-datagrid" title="已添加列表"
                    data-options="rownumbers:true,pagination:true,fit:true,fitColumns:true,toolbar:'#dgProductSizeToolbar',url:'/h/t.html',queryParams:{reqName:'GetProductSizeJsonForDatagrid', productId:$('#hId').val()}">
                <thead>
                    <tr>
                        <th data-options="field:'ProductItemId',checkbox:true"></th>
                        <th data-options="field:'ProductId',hidden:true"></th>
                        <th data-options="field:'ProductItemName',width:300">商品项</th>
                        <th data-options="field:'SizeAppend',width:800">尺码</th>
                    </tr>
                </thead>
            </table>

            <div id="dlgProductSize" style="padding:10px;"></div>
        </div>
        <div title="商品其它图片" style="overflow:auto;padding:20px;">

            <div id="dgProductImageToolbar" style="padding:5px;">
                <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true" onclick="AddProductImage.Add()">新 增</a>
                <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-edit',plain:true" onclick="AddProductImage.Edit()">编 辑</a>
                <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-remove',plain:true" onclick="AddProductImage.Del()">删 除</a>
            </div>
            <table id="dgProductImage" class="easyui-datagrid" title="已添加列表"
                    data-options="rownumbers:true,pagination:true,fit:true,fitColumns:true,toolbar:'#dgProductImageToolbar',url:'/h/t.html',queryParams:{reqName:'GetProductImageJsonForDatagrid', productId:$('#hId').val()}">
                <thead>
                    <tr>
                        <th data-options="field:'ProductItemId',checkbox:true"></th>
                        <th data-options="field:'ProductId',hidden:true"></th>
                        <th data-options="field:'ProductItemName',width:300">商品项</th>
                        <th data-options="field:'PictureAppend',width:800,formatter:AddProduct.FImageAppendUrl">图片</th>
                    </tr>
                </thead>
            </table>

            <div id="dlgProductImage" style="padding:10px;"></div>

        </div>
        <div title="库存" style="overflow:auto;padding:20px;">
            <div id="dgProductStockToolbar" style="padding:5px;">
                <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true" onclick="AddProductStock.Add()">新 增</a>
                <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-edit',plain:true" onclick="AddProductStock.Edit()">编 辑</a>
                <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-remove',plain:true" onclick="AddProductStock.Del()">删 除</a>
            </div>
            <table id="dgProductStock" class="easyui-datagrid" title="已添加列表"
                    data-options="rownumbers:true,pagination:true,fit:true,fitColumns:true,toolbar:'#dgProductStockToolbar',url:'/h/t.html',queryParams:{reqName:'GetProductStockJsonForDatagrid', productId:$('#hId').val()}">
                <thead>
                    <tr>
                        <th data-options="field:'Id',checkbox:true"></th>
                        <th data-options="field:'ProductId',hidden:true"></th>
                        <th data-options="field:'ProductItemId',hidden:true"></th>
                        <th data-options="field:'ProductItemName',width:100">商品项</th>
                        <th data-options="field:'ProductSize',width:100">尺码</th>
                        <th data-options="field:'StockNum',width:100">库存</th>
                    </tr>
                </thead>
            </table>

            <div id="dlgProductStock" style="padding:10px;"></div>
        </div>
    </div>

    <div id="dlgUploadPicture" style="padding:10px;"></div>
    <div id="dlgSingleSelectProductPicture" class="easyui-dialog" title="选择图片（单选）" data-options="closed:true,modal:true,href:'/t/tpicture.html?dlgId=dlgSingleSelectProductPicture&funName=PictureProduct',width:810,height:$(window).height()*0.8,
        buttons: [{
            id:'btnSingleSelectProductPicture',text:'确定',iconCls:'icon-ok',
            handler:function(){
                AddProduct.SetSinglePicture('imgProductPicture');
                $('#dlgSingleSelectProductPicture').dialog('close');
            }
        },{
            id:'btnCancelSingleSelectProductPicture', text:'取消',iconCls:'icon-cancel',
            handler:function(){
                $('#dlgSingleSelectProductPicture').dialog('close');
            }
        }],
        toolbar:[{
            id:'dlgSingleSelectProductPictureToolbarUpload',text:'上传',iconCls:'icon-add',
		    handler:function(){
                DlgPictureSelect.DlgUpload();
            }
	    }]" style="padding:10px;"></div>

    <input type="hidden" id="hDlgOpenId" />
    <input type="hidden" id="hIsMutil" />

    <input type="hidden" id="hId" runat="server" clientidmode="Static" name="hId" />

    <script type="text/javascript" src="../../Scripts/Admin/DlgPictureSelect.js"></script>
    <script type="text/javascript" src="../../Scripts/Admin/ECShop/AddProduct.js"></script>
    <script type="text/javascript" src="../../Scripts/Admin/ECShop/ProductPicture.js"></script>
    <script type="text/javascript" src="../../Scripts/Admin/ECShop/SizePicture.js"></script>
    <script type="text/javascript" src="../../Scripts/Admin/ECShop/AddProductItem.js"></script>
    <script type="text/javascript" src="../../Scripts/Admin/ECShop/AddProductDetail.js"></script>
    <script type="text/javascript" src="../../Scripts/Admin/ECShop/AddProductAttr.js"></script>
    <script type="text/javascript" src="../../Scripts/Admin/ECShop/AddProductSizePicture.js"></script>
    <script type="text/javascript" src="../../Scripts/Admin/ECShop/AddProductSize.js"></script>
    <script type="text/javascript" src="../../Scripts/Admin/ECShop/AddProductImage.js"></script>
    <script type="text/javascript" src="../../Scripts/Admin/ECShop/AddProductStock.js"></script>

<script type="text/javascript">

    $(function () {
        try {
            AddProduct.Init();
            //ProductPicture.CreateDlgProductPicture();
            //SizePicture.CreateDlgSizePicture();
        }
        catch (e) {
            $.messager.alert('错误提醒', e.name + ": " + e.message, 'error');
        }

    })

</script>

</asp:Content>
