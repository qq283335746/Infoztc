<%@ Page Title="新增内容" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AddContentDetail.aspx.cs" Inherits="TygaSoft.Web.Admin.AboutSite.AddContentDetail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="../../Scripts/plugins/kindeditor/themes/default/default.css" rel="stylesheet" type="text/css" />
    <link href="../../Scripts/plugins/kindeditor/plugins/code/prettify.css" rel="stylesheet" type="text/css" /> 
    <script type="text/javascript" src="../../Scripts/JeasyuiExtend.js"></script>
    <script type="text/javascript" src="../../Scripts/plugins/kindeditor/kindeditor.js"></script>
    <script type="text/javascript" src="../../Scripts/plugins/kindeditor/lang/zh_CN.js"></script>
    <script type="text/javascript" src="../../Scripts/plugins/kindeditor/plugins/code/prettify.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">

    <div class="easyui-panel" style="padding:10px;">
        <div class="row mt10">
            <span class="rl"><b class="cr">*</b>标题：</span>
            <div class="fl" style="width:688px;">
                <input type="text" id="txtTitle" runat="server" clientidmode="Static" class="easyui-textbox" data-options="required:true,missingMessage:'必填项'" style="width:100%;" />
            </div>
            <span class="clr"></span>
        </div>
        <div class="row mt10">
            <span class="rl"><b class="cr">*</b>所属类型：</span>
            <div class="fl">
                <input id="txtParent" runat="server" clientidmode="Static" class="easyui-combotree" style="width:200px;" />
            </div>
            <div class="clr"></div>
        </div>
        <div class="row mt10" style="display:none;">
            <span class="rl">banner图片：</span>
            <div class="fl">
                <img id="imgPicture" runat="server" clientidmode="Static" src="../../Images/nopic.gif" alt="上传图片" width="688" height="200" onclick="DlgPictureSelect.DlgSingle('PictureContent')" /><br />
                <input type="hidden" id="hPictureId" runat="server" clientidmode="Static" />
            </div>
            <div class="clr"></div>
        </div>
        <div class="row mt10">
            <span class="rl">简介：</span>
            <div class="fl" style="width:680px;">
                <textarea id="txtaDescr" runat="server" clientidmode="Static" rows="3" cols="80" class="txta" style="width:100%;"></textarea>
            </div>
            <div class="clr"></div>
        </div>
        <div class="row mt10">
            <span class="rl"><b class="cr">*</b>内容：</span>
            <div class="fl">
                <textarea id="txtContent" runat="server" clientidmode="Static" cols="100" rows="8" style="width:688px;height:800px;"></textarea>
            </div>
            <div class="clr"></div>
        </div>
        <div class="row mt10" style="display:none;">
            <span class="rl">访问量设置：</span>
            <div class="fl">
                <input id="txtVirtualViewCount" runat="server" clientidmode="Static" class="easyui-validatebox" data-options="validType:'int'" />
            </div>
            <div class="clr"></div>
        </div>
        <div class="row mt10">
            <span class="rl">排序：</span>
            <div class="fl">
                <input id="txtSort" runat="server" clientidmode="Static" class="easyui-validatebox" data-options="validType:'int'" />
            </div>
            <div class="clr"></div>
        </div>
        <div class="row mt10">
            <span class="rl">是否禁用：</span>
                <div class="fl ml10">
                    <asp:RadioButtonList ID="rbtnlIsDisable" runat="server" ClientIDMode="Static" RepeatDirection="Horizontal" RepeatLayout="Flow"></asp:RadioButtonList>
                </div>
                <span class="clr"></span>
            </div>
        <div class="row mt10">
            <span class="rl">&nbsp;</span>
            <div class="fl">
            <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-save'" onclick="AddContentDetail.OnSave()">提交</a>
            </div>
            <span class="clr"></span>
        </div>
    </div>

    <input type="hidden" id="hId" runat="server" clientidmode="Static" name="hId" />

    <div id="dlgUploadPicture" style="padding:10px;"></div>
    <div id="dlgSingleSelectPicture" class="easyui-dialog" title="选择图片（单选）" data-options="closed:true,modal:true,href:'/t/tpicture.html?dlgId=dlgSingleSelectPicture&funName=PictureContent',width:810,height:$(window).height()*0.8,
    buttons: [{
        id:'btnSingleSelectPicture',text:'确定',iconCls:'icon-ok',
        handler:function(){
            DlgPictureSelect.SetSinglePicture('imgPicture');
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

    <script type="text/javascript" src="../../Scripts/Admin/DlgPictureSelect.js"></script>
    <script type="text/javascript" src="../../Scripts/Admin/AboutSite/AddContentDetail.js"></script>
    <script type="text/javascript">
        var editor_content;
        KindEditor.ready(function (K) {
            editor_content = K.create('#txtContent', {
                uploadJson: '/h/ty.html',
                fileManagerJson: '/h/ty.html',
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
                AddContentDetail.Init();
            }
            catch (e) {
                $.messager.alert('错误提醒', e.name + ": " + e.message, 'error');
            }
        
        })

    </script>

</asp:Content>
