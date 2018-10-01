<%@ Page Title="新建选手" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="AddActivityPlayer.aspx.cs" Inherits="TygaSoft.Web.Admin.Activity.AddActivityPlayer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

  <link href="../../Scripts/plugins/kindeditor/themes/default/default.css" rel="stylesheet" type="text/css" />
  <link href="../../Scripts/plugins/kindeditor/plugins/code/prettify.css" rel="stylesheet" type="text/css" /> 
  <script src="../../Scripts/JeasyuiExtend.js" type="text/javascript"></script>
  <script type="text/javascript" src="../../Scripts/plugins/kindeditor/kindeditor.js"></script>
  <script type="text/javascript" src="../../Scripts/plugins/kindeditor/lang/zh_CN.js"></script>
  <script type="text/javascript" src="../../Scripts/plugins/kindeditor/plugins/code/prettify.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <div class="row mt10">
        <span class="rl"><b class="cr">*</b>姓名：</span>
        <div class="fl">
            <input type="text" id="txtName" runat="server" clientidmode="Static" class="easyui-textbox mtxt"
                style="width: 200px" />
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10">
        <span class="fl rl">头像：</span>
        <div class="fl ml10">
            <img id="imgSinglePicture" runat="server" clientidmode="Static" src="../../Images/nopic.gif"
                alt="图片" width="120" height="120" onclick="DlgSelectPicture.DlgSingle('ActivityPlayerPhotoPicture')" /><br />
            <input id="hImgSinglePictureId" runat="server" clientidmode="Static" type="hidden" />
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10">
        <span class="rl"><b class="cr">*</b>个人详情：</span>
        <div class="fl">
            <textarea id="txtContent" runat="server" clientidmode="Static" cols="100" rows="8" style="width:800px;height:400px;"></textarea>
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10">
        <span class="rl">实际投票数：</span>
        <div class="fl">
            <input type="text" id="txtActualVoteCount" value="0" runat="server" clientidmode="Static" readonly="readonly"
                class="easyui-numberbox mtxt" style="width: 60px" />
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10">
        <span class="rl">修改投票数：</span>
        <div class="fl">
            <input type="text" id="txtUpdateVoteCount" value="0" runat="server" clientidmode="Static"
                class="easyui-numberbox mtxt" style="width: 60px" />
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10">
        <span class="rl">是否禁用：</span>
        <div class="fl">
            <input type="radio" id="rdFalse" runat="server" name="rdIsDisable" value="false"
                checked="true" /><label>否</label>
            <input type="radio" id="rdTrue" runat="server" name="rdIsDisable" value="true" /><label>是</label>
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10">
        <span class="rl">&nbsp;</span>
        <div class="fl">
            <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-save'"
                onclick="AddActivityPlayer.OnSave()">提交</a>
        </div>
        <span class="clr"></span>
    </div>

    <input type="hidden" id="hId" runat="server" clientidmode="Static" />
    <input type="hidden" id="asId" runat="server" clientidmode="Static" />

    <div id="dlgUploadPicture" style="padding: 10px;">
    </div>
    <div id="dlgSingleSelectPicture" class="easyui-dialog" title="选择图片（单选）" data-options="closed:true,modal:true,href:'/t/yt.html?dlgId=dlgSingleSelectPicture&funName=ActivityPlayerPhotoPicture',width:810,height:$(window).height()*0.8,
    buttons: [{
    id:'btnSelectPicture',text:'确定',iconCls:'icon-ok',
        handler:function(){
            DlgSelectPicture.SetSinglePicture('imgSinglePicture');
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
            DlgSelectPicture.DlgUpload();
        }
	}]" style="padding: 10px;">
    </div>

    <script type="text/javascript" src="../../Scripts/Admin/DlgSelectPicture.js"></script>
    <script type="text/javascript" src="../../Scripts/Admin/Activity/AddActivityPlayer.js"></script>
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
                AddActivityPlayer.Init();
            }
            catch (e) {
                $.messager.alert('错误提醒', e.name + ": " + e.message, 'error');
            }

        })
    </script>
</asp:Content>
