<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="PictureTest.aspx.cs" Inherits="TygaSoft.Web.Admin.Topic.PictureTest" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">

    <div class="row">
        <span class="fl rl">单选图片：</span>
        <div class="fl ml10">
            <img id="imgSinglePicture" src="../../Images/nopic.gif" alt="图片" width="120px" height="120px" onclick="DlgSelectPicture.DlgSingle('CommunionPicture')" /><br />
            <input type="hidden" />
        </div>
        <span class="clr"></span>
    </div>
    <div class="row">
        <span class="fl rl">多选图片：</span>
        <div class="fl ml10">
            <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true" onclick="DlgSelectPicture.DlgMutil('CommunionPicture')">选 择</a>
        </div>
        <div id="mutilPictureBox" class="fl">
            <div class="fl ml10" style="display:none;">
                <img src="../../Images/nopic.gif" alt="图片" width="120px" height="120px" alt="" /><br />
                <input type="hidden" />
            </div>
        </div>
        <span class="clr"></span>
    </div>

    <div id="dlgUploadPicture" style="padding:10px;"></div>

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
	}]" style="padding:10px;"></div>

    <div id="dlgMutilSelectPicture" class="easyui-dialog" title="选择图片（多选）" data-options="closed:true,modal:true,href:'/t/yt.html?dlgId=dlgMutilSelectPicture&funName=CommunionPicture&isMutil=true',width:810,height:$(window).height()*0.8,
    buttons: [{
        id:'btnMutilSelectPicture',text:'确定',iconCls:'icon-ok',
        handler:function(){
            DlgSelectPicture.SetMutilPicture('mutilPictureBox');
            $('#dlgMutilSelectPicture').dialog('close');
        }
    },{
        id:'btnCancelMutilSelectPicture', text:'取消',iconCls:'icon-cancel',
        handler:function(){
            $('#dlgMutilSelectPicture').dialog('close');
        }
    }],
    toolbar:[{
        id:'dlgMutilSelectPictureToolbarUpload',text:'上传',iconCls:'icon-add',
		handler:function(){
            DlgSelectPicture.DlgUpload();
        }
	}]" style="padding:10px;"></div>

    <script type="text/javascript" src="../../Scripts/Admin/DlgSelectPicture.js"></script>

</asp:Content>
