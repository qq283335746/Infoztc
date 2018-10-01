<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddProductSizePicture.aspx.cs" Inherits="TygaSoft.Web.Templates.Admin.AddProductSizePicture" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>尺码表</title>
</head>
<body>
    <form id="dlgProductSizePictureFm" runat="server">
        <div class="row mt10">
            <span class="rl">商品项：</span>
            <div class="fl">
                <asp:DropDownList ID="ddlProductItem_ProductSizePicture" runat="server" CssClass="easyui-validatebox" data-options="required:true,validType:'select'"></asp:DropDownList>
            </div>
            <div class="clr"></div>
        </div>
        <div class="row mt10">
            <span class="rl">尺码表名称：</span>
            <div class="fl">
                <input type="text" id="txtName" runat="server" clientidmode="Static" class="easyui-validatebox" data-options="required:true" />
            </div>
            <div class="clr"></div>
        </div>
        <div class="row mt10">
            <span class="rl">尺码表图片：</span>
            <div class="fl">
                <img id="imgProductSizePicture" runat="server" clientidmode="Static" src="../../Images/nopic.gif" alt="上传图片" width="227" height="143" onclick="AddProductSizePicture.OnPictureClick()" />
                <input type="hidden" id="hImgProductSizePictureId" runat="server" clientidmode="Static" />
            </div>
            <div class="clr"></div>
        </div>

        <input type="hidden" id="hAction_ProductSizePicture" runat="server" clientidmode="Static" value="add" />
    </form>

    <div id="dlgSingleSelectProductSizePicture" class="easyui-dialog" title="选择图片（单选）" data-options="closed:true,modal:true,href:'/t/tpicture.html?dlgId=dlgSingleSelectProductSizePicture&funName=PictureProductSize',width:810,height:$(window).height()*0.8,
        buttons: [{
            id:'btnSingleSelectProductSizePicture',text:'确定',iconCls:'icon-ok',
            handler:function(){
                AddProductSizePicture.SetSinglePicture('imgProductSizePicture');
                $('#dlgSingleSelectProductSizePicture').dialog('close');
            }
        },{
            id:'btnCancelSingleSelectProductSizePicture', text:'取消',iconCls:'icon-cancel',
            handler:function(){
                $('#dlgSingleSelectProductSizePicture').dialog('close');
            }
        }],
        toolbar:[{
            id:'dlgSingleSelectProductSizePictureToolbarUpload',text:'上传',iconCls:'icon-add',
		    handler:function(){
                DlgPictureSelect.DlgUpload();
            }
	    }]" style="padding:10px;"></div>

</body>
</html>
