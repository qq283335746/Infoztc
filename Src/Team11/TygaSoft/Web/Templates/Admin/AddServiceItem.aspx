<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddServiceItem.aspx.cs" Inherits="TygaSoft.Web.Templates.Admin.AddServiceItem" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="dlgFm" runat="server">
        <div class="row">
            <span class="fl rl">所属服务分类：</span>
            <div class="fl ml10">
                <span id="lbParent" runat="server" clientidmode="Static"></span>
                <input type="hidden" id="hParentId" runat="server" clientidmode="Static" />
            </div>
            <span class="clr"></span>
        </div>
        <div class="row mt10">
            <span class="fl rl"><b class="cr">*</b>名称：</span>
            <div class="fl ml10">
                <input type="text" id="txtName" runat="server" clientidmode="Static" tabindex="1" maxlength="256" class="easyui-validatebox txt" data-options="required:true" />
            </div>
            <span class="clr"></span>
        </div>
        <div class="row mt10">
            <span class="fl rl"><b class="cr">*</b>图片：</span>
            <div class="fl ml10">
                <img id="imgServicePicture" runat="server" clientidmode="Static" src="../../Images/nopic.gif" alt="上传图片" width="227" height="143" onclick="ListServiceItem.OnPictureClick()" />
                <input type="hidden" id="hImgServicePictureId" runat="server" clientidmode="Static" />
            </div>
            <span class="clr"></span>
        </div>
        <div class="row mt10">
            <span class="fl rl">排序：</span>
            <div class="fl ml10">
                <input type="text" id="txtSort" runat="server" clientidmode="Static" tabindex="4" maxlength="9" class="easyui-validatebox txt" data-options="validType:'int'" />
            </div>
            <span class="clr"></span>
        </div>
        <div class="row mt10">
            <span class="rl">有效时间：</span>
            <div class="fl ml10">
                <input id="txtEnableStartTime" runat="server" clientidmode="Static" class="easyui-datetimebox" style="width:160px;" />
            </div>
            <div class="fl mlr10"> 至 </div>
            <div class="fl">
                <input id="txtEnableEndTime" runat="server" clientidmode="Static" class="easyui-datetimebox" style="width:160px;" />
            </div>
            <span class="clr"></span>
        </div>
        <div class="row mt10">
            <span class="rl">是否禁用：</span>
            <div class="fl ml10">
                <asp:RadioButtonList ID="rbtnList" runat="server" ClientIDMode="Static" RepeatDirection="Horizontal" RepeatLayout="Flow"></asp:RadioButtonList>
            </div>
            <span class="clr"></span>
        </div>
        <input type="hidden" id="hId" runat="server" ClientIDMode="Static" value="" />
    </form>

    <div id="dlgSingleSelectServiceItemPicture" class="easyui-dialog" title="选择图片（单选）" data-options="closed:true,modal:true,href:'/t/tpicture.html?dlgId=dlgSingleSelectServiceItemPicture&funName=PictureServiceItem',width:810,height:$(window).height()*0.8,
        buttons: [{
            id:'btnSingleSelectServiceItemPicture',text:'确定',iconCls:'icon-ok',
            handler:function(){
                ListServiceItem.SetSinglePicture('imgServicePicture');
                $('#dlgSingleSelectServiceItemPicture').dialog('close');
            }
        },{
            id:'btnCancelSingleSelectServiceItemPicture', text:'取消',iconCls:'icon-cancel',
            handler:function(){
                $('#dlgSingleSelectServiceItemPicture').dialog('close');
            }
        }],
        toolbar:[{
            id:'dlgSingleSelectServiceItemPictureToolbarUpload',text:'上传',iconCls:'icon-add',
		    handler:function(){
                DlgPictureSelect.DlgUpload();
            }
	    }]" style="padding:10px;"></div>

    <script type="text/javascript">
        var dlgFun = {
            Init: function () {
                $("#dlgFm").removeAttr("action");
            }
        }

        $(function () {
            dlgFun.Init();
        })
    </script>
</body>
</html>
