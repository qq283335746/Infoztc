<%@ Page Title="新建话题" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="AddTopicSubject.aspx.cs" Inherits="TygaSoft.Web.Admin.Topic.AddTopicSubject" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <div class="row mt10">
        <span class="rl"><b class="cr"></b>标题：</span>
        <div class="fl">
            <input type="text" id="txtTitle" runat="server" clientidmode="Static" class="easyui-textbox mtxt"
                style="width: 400px" />
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10">
        <span class="rl"><b class="cr">*</b>内容：</span>
        <div class="fl">
            <textarea id="txtContent" runat="server" clientidmode="Static" rows="100" cols="100" style="overflow: auto;
                height: 150px; width: 600px; border: 1px solid #95B8E7;"></textarea>
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10">
        <span class="fl rl">图片：</span>
        <div class="fl ml10">
            <img id="imgSinglePicture" runat="server" clientidmode="Static" src="../../Images/nopic.gif"
                alt="图片" width="120" height="120" onclick="DlgSelectPicture.DlgSingle('CommunionPicture')" /><br />
            <input id="hImgSinglePictureId" runat="server" clientidmode="Static" type="hidden" />
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10">
        <span class="rl">是否禁用：</span>
        <div class="fl">
            <input type="radio" id="rdFalse" runat="server" clientidmode="Static" name="rdIsDisable"
                value="False" checked="True" /><label>否</label>
            <input type="radio" id="rdTrue" runat="server" clientidmode="Static" name="rdIsDisable"
                value="True" /><label>是</label>
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10">
        <span class="rl">是否推送：</span>
        <div class="fl">
            <input type="radio" id="rdPushFalse" runat="server" name="rdIsPush" value="false" clientidmode="Static" checked="true" /><label>否</label>
            <input type="radio" id="rdPushTrue" runat="server" name="rdIsPush" value="true" clientidmode="Static" /><label>是</label>
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10">
        <span class="rl">&nbsp;</span>
        <div class="fl">
            <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-save'"
                onclick="AddTopicSubject.OnSave()">提交</a>
        </div>
        <span class="clr"></span>
    </div>

    <input type="hidden" id="hId" runat="server" clientidmode="Static" />
    <input type="hidden" id="isTop" value="False" runat="server" clientidmode="Static" />
    <input type="hidden" id="UserId" runat="server" clientidmode="Static" />

    <div id="dlgUploadPicture" style="padding: 10px;">
    </div>
    <div id="dlgSingleSelectPicture" class="easyui-dialog" title="选择图片（单选）" data-options="closed:true,modal:true,href:'/t/yt.html?dlgId=dlgSingleSelectPicture&funName=CommunionPicture',width:810,height:$(window).height()*0.8,
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
    <script type="text/javascript" src="../../Scripts/Admin/Topic/AddTopicSubject.js"></script>
    <script type="text/javascript">
        $(function () {
            try {
                AddTopicSubject.Init();
            }
            catch (e) {
                $.messager.alert('错误提醒', e.name + ": " + e.message, 'error');
            }

        })

    </script>
    <style scoped="scoped">
        .textbox
        {
            height: 20px;
            margin: 0;
            padding: 0 2px;
            box-sizing: content-box;
        }
    </style>
</asp:Content>
