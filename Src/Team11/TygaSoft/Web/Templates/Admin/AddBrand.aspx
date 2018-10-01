<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddBrand.aspx.cs" Inherits="TygaSoft.Web.Templates.Admin.AddBrand" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="dlgFm" runat="server">
        <div class="fl">
            <div class="row">
                <span class="fl rl">所属品牌：</span>
                <div class="fl ml10">
                    <span id="lbParent"></span>
                    <input type="hidden" id="hParentId" runat="server" />
                </div>
                <span class="clr"></span>
            </div>
            <div class="row mt10">
                <span class="fl rl"><b class="cr">*</b>名称：</span>
                <div class="fl ml10">
                    <input type="text" id="txtName" runat="server" tabindex="1" maxlength="256" class="easyui-validatebox txt" data-options="required:true" />
                </div>
                <span class="clr"></span>
            </div>
            <div class="row mt10">
                <span class="fl rl"><b class="cr">*</b>代号：</span>
                <div class="fl ml10">
                    <input type="text" id="txtCode" runat="server" tabindex="2" maxlength="256" class="easyui-validatebox txt" data-options="required:true" />
                </div>
                <span class="clr"></span>
            </div>
            <div class="row mt10">
                <span class="fl rl">所属分类：</span>
                <div class="fl ml10">
                    <ul id="cbtCategory" class="easyui-combotree" data-options="url:'../Handlers/Admin/ECShop/HandlerCategory.ashx?reqName=GetCategoryTreeJson',method:'Get',onLoadSuccess:ListBrand.LoadSuccessForCategory" style="width:300px;"></ul>
                </div>
                <span class="clr"></span>
            </div>
            <div class="row mt10">
                <span class="fl rl">排序：</span>
                <div class="fl ml10">
                    <input type="text" id="txtSort" runat="server" tabindex="4" maxlength="9" class="easyui-validatebox txt" data-options="validType:'number'" />
                </div>
                <span class="clr"></span>
            </div>
            <div class="row mt10">
                <span class="fl rl">备注：</span>
                <div class="fl ml10">
                    <textarea id="txtRemark" runat="server" cols="90" rows="2" tabindex="5" class="txtarea" style="width:293px;"></textarea>
                </div>
                <span class="clr"></span>
            </div>
        </div>
        <div class="fl ml10" style="margin-top:30px;">
            <img id="imgBrandPicture" runat="server" src="../../Images/nopic.gif" alt="上传图片" width="200" height="210" onclick="DlgPictureSelect.DlgSingle('PictureBrand')" /><br />
            <input type="hidden" id="hBrandPictureId" runat="server" />
        </div>
        <span class="clr"></span>
        
        <input type="hidden"  id="hCategoryId" runat="server" value=""/>
        <input type="hidden" id="hId" runat="server" value="" />
    </form>

    <div id="dlgUploadPicture" style="padding:10px;"></div>
    <div id="dlgSingleSelectPicture" class="easyui-dialog" title="选择图片（单选）" data-options="closed:true,modal:true,href:'/t/tpicture.html?dlgId=dlgSingleSelectPicture&funName=PictureBrand',width:810,height:$(window).height()*0.8,
    buttons: [{
        id:'btnSingleSelectPicture',text:'确定',iconCls:'icon-ok',
        handler:function(){
            DlgPictureSelect.SetSinglePicture('imgBrandPicture');
            $('#dlgSingleSelectPicture').dialog('close');
        }
    },{
        id:'btnCancelSingleSelectPicture', text:'取消',iconCls:'icon-cancel',
        handler:function(){
            $('#dlgSingleSelectPicture').dialog('close');
        }
    }],
    toolbar:[{
        id:'dlgSingleSelectPictureToolbarUpload',text:'上传',iconCls:'icon-add',
		handler:function(){
            DlgPictureSelect.DlgUpload();
        }
	}]" style="padding:10px;"></div>

    <script type="text/javascript">
        $(function () {
            var node = $("#treeCt").tree('find', $("#hParentId").val());
            if (node) {
                $("#lbParent").text(node.text);
            }
        })
    </script>

</body>
</html>
