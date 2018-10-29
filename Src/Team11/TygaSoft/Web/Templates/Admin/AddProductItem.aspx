<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddProductItem.aspx.cs" Inherits="TygaSoft.Web.Templates.Admin.AddProductItem" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="dlgProductItemFm" runat="server">
        <div class="row">
            <span class="rl"><b class="cr">*</b>名称：</span>
            <div class="fl ml10">
                <input type="text" id="txtName" runat="server" clientidmode="Static" class="easyui-textbox txt" data-options="required:true" />
            </div>
        </div>
        <div class="row mt10">
            <span class="rl">图片：</span>
            <div class="fl ml10">
                <img id="imgProductItemPicture" runat="server" clientidmode="Static" src="../../Images/nopic.gif" alt="上传图片" width="227" height="143" onclick="AddProductItem.DlgSingle()" />
                <input type="hidden" id="hImgProductItemPictureId" runat="server" clientidmode="Static" />
            </div>
            <span class="clr"></span>
        </div>
        <div class="row mt10">
            <span class="rl">排序：</span>
            <div class="fl ml10">
                <input id="txtSort" runat="server" clientidmode="Static" class="easyui-textbox" data-options="validType:'int'" />
            </div>
            <div class="clr"></div>
        </div>
        <div class="row mt10">
            <span class="rl">有效时间：</span>
            <div class="fl ml10">
                <input id="txtEnableStartTime_ProductItem" runat="server" clientidmode="Static" class="easyui-datetimebox" style="width:160px;" />
            </div>
            <div class="fl mlr10"> 至 </div>
            <div class="fl ml10">
                <input id="txtEnableEndTime_ProductItem" runat="server" clientidmode="Static" class="easyui-datetimebox" style="width:160px;" />
            </div>
            <span class="clr"></span>
        </div>
        <div class="row mt10">
            <span class="rl">上下架：</span>
            <div class="fl ml10">
                <asp:RadioButtonList ID="rbtnIsEnableList_ProductItem" runat="server" ClientIDMode="Static" RepeatDirection="Horizontal" RepeatLayout="Flow"></asp:RadioButtonList>
            </div>
            <span class="clr"></span>
        </div>
        <div class="row mt10">
            <span class="rl">是否禁用：</span>
            <div class="fl ml10">
                <asp:RadioButtonList ID="rbtnList_ProductItem" runat="server" ClientIDMode="Static" RepeatDirection="Horizontal" RepeatLayout="Flow"></asp:RadioButtonList>
            </div>
            <span class="clr"></span>
        </div>

        <input type="hidden" id="hProductItemId" runat="server"/>
    </form>

    <div id="dlgSingleSelectProductItemPicture" class="easyui-dialog" title="选择图片（单选）" data-options="closed:true,modal:true,href:'/t/tpicture.html?dlgId=dlgSingleSelectPicture&funName=PictureProduct',width:810,height:$(window).height()*0.8,
        buttons: [{
            id:'btnSingleSelectProductItemPicture',text:'确定',iconCls:'icon-ok',
            handler:function(){
                DlgPictureSelect.SetSinglePicture('imgProductItemPicture');
                $('#dlgSingleSelectProductItemPicture').dialog('close');
            }
        },{
            id:'btnCancelSingleSelectProductItemPicture', text:'取消',iconCls:'icon-cancel',
            handler:function(){
                $('#dlgSingleSelectProductItemPicture').dialog('close');
            }
        }],
        toolbar:[{
            id:'dlgSingleSelectProductItemPictureToolbarUpload',text:'上传',iconCls:'icon-add',
		    handler:function(){
                DlgPictureSelect.DlgUpload();
            }
	    }]" style="padding:10px;"></div>

    <script type="text/javascript">

    </script>

</body>
</html>
