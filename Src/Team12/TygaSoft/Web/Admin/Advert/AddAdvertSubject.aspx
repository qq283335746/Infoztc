<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="AddAdvertSubject.aspx.cs" Inherits="TygaSoft.Web.Admin.Advert.AddAdvertSubject" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="../../Scripts/JeasyuiExtend.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <div class="row mt10">
        <span class="rl">标题：</span>
        <div class="fl">
            <input type="text" id="txtTitle" runat="server" clientidmode="Static" class="easyui-textbox mtxt" style="width: 400px" />
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10">
        <span class="fl rl">图片：</span>
        <div class="fl ml10">
            <img id="imgSinglePicture" runat="server" clientidmode="Static" src="../../Images/nopic.gif"
                alt="图片" width="120" height="120" onclick="DlgPictureSelect.DlgSingle('PictureAdStartup')" /><br />
            <input id="hImgSinglePictureId" runat="server" clientidmode="Static" type="hidden" />
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10">
        <span class="rl"><b class="cr">*</b>播放时长(秒)：</span>
        <div class="fl">
            <input type="text" id="txtPlayTime" runat="server" clientidmode="Static" class="easyui-numberbox mtxt" data-options="required:true,missingMessage:'必填项'"
                style="width: 200px" />
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10">
        <span class="rl">是否有效：</span>
        <div class="fl">
            <input type="radio" id="rdFalse" runat="server" name="rdIsDisable" value="false"
                clientidmode="Static" checked="true" /><label>是</label>
            <input type="radio" id="rdTrue" runat="server" name="rdIsDisable" value="true" clientidmode="Static" /><label>否</label>
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10">
        <span class="rl">&nbsp;</span>
        <div class="fl">
            <a class="easyui-linkbutton" data-options="iconCls:'icon-save'" onclick="AddAdvertSubject.OnSave()">提交</a>
        </div>
        <span class="clr"></span>
    </div>
    <input type="hidden" id="hId" runat="server" clientidmode="Static" />
    <div id="dlgUploadPicture" style="padding: 10px;">
    </div>
    <div id="dlgSingleSelectPicture" class="easyui-dialog" title="选择图片（单选）" data-options="closed:true,modal:true,href:'/t/tpicture.html?dlgId=dlgSingleSelectPicture&funName=PictureAdStartup',width:810,height:$(window).height()*0.8,
    buttons: [{
    id:'btnSelectPicture',text:'确定',iconCls:'icon-ok',
        handler:function(){
            DlgPictureSelect.SetSinglePicture('imgSinglePicture');
            $('#dlgSingleSelectPicture').dialog('close');
        }
    },{
    id:'btnCancelSelectPicture', text:'取消',iconCls:'icon-cancel',
        handler:function(){
	        $('#dlgSingleSelectPicture').dialog('close');
        }
    }],
    toolbar:[{
        id:'dlgSingleSelectPictureToolbarUpload',text:'上传',iconCls:'icon-add',
		handler:function(){
            DlgPictureSelect.DlgUpload();
        }
	}]" style="padding: 10px;">
    </div>
    <script type="text/javascript" src="../../Scripts/Admin/DlgPictureSelect.js"></script>
    <script type="text/javascript" src="../../Scripts/Admin/Advert/AddAdvertSubject.js"></script>
    <script type="text/javascript">
        $(function () {
            try {
                AddAdvertSubject.Init();
            }
            catch (e) {
                $.messager.alert('错误提醒', e.name + ": " + e.message, 'error');
            }
        });
    </script>
</asp:Content>
