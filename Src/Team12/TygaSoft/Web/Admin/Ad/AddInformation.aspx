<%@ Page Title="新建/编辑资讯" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AddInformation.aspx.cs" Inherits="TygaSoft.Web.Admin.Ad.AddInformation" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../Scripts/plugins/kindeditor/themes/default/default.css" rel="stylesheet"
        type="text/css" />
    <link href="../../Scripts/plugins/kindeditor/plugins/code/prettify.css" rel="stylesheet"
        type="text/css" />
    <script src="../../Scripts/JeasyuiExtend.js" type="text/javascript"></script>
    <script type="text/javascript" src="../../Scripts/plugins/kindeditor/kindeditor.js"></script>
    <script type="text/javascript" src="../../Scripts/plugins/kindeditor/lang/zh_CN.js"></script>
    <script type="text/javascript" src="../../Scripts/plugins/kindeditor/plugins/code/prettify.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
<div class="row mt10">
        <span class="rl"><b class="cr">*</b>标题：</span>
        <div class="fl">
            <input type="text" id="txtTitle" runat="server" clientidmode="Static" class="easyui-textbox mtxt"
                data-options="required:true,missingMessage:'必填项'" style="width: 400px" />
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10">
        <span class="rl">显示类型：</span>
        <div class="fl">
            <input type="radio" id="rdViewType0" runat="server" name="rdViewType" value="0" clientidmode="Static" checked="true" /><label>图文</label>
            <input type="radio" id="rdViewType1" runat="server" name="rdViewType" value="1" clientidmode="Static" /><label>横幅</label>
            <input type="radio" id="rdViewType2" runat="server" name="rdViewType" value="2" clientidmode="Static" /><label>图片</label>
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10">
        <span class="rl"><b class="cr">*</b>资讯描述：</span>
        <div class="fl">
            <textarea id="txtSummary" runat="server" clientidmode="Static" cols="120" rows="5"></textarea>
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10">
        <span class="rl"><b class="cr">*</b>内容：</span>
        <div class="fl">
            <textarea id="txtContent" runat="server" clientidmode="Static" cols="120" rows="25"></textarea>
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10">
        <span class="rl">备注：</span>
        <div class="fl">
            <textarea id="txtRemark" runat="server" clientidmode="Static" cols="120" rows="3"></textarea>
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10">
        <span class="rl">来源：</span>
        <div class="fl">
            <input type="text" id="txtSource" runat="server" clientidmode="Static" class="easyui-textbox mtxt"
                style="width: 400px" />
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10">
        <span class="fl rl">图片：</span>
        <div class="fl ml10">
            <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true" onclick="AddInformation.DlgSingleForMutil()">选 择</a>
        </div>
        <div class="fl" style="width:400px;">
            <div id="imgContentPicture"  runat="server" clientidmode="Static">
                <div class="row_col w110 mb10" style="display:none; width:200px;">
                    <table style="width:100%;">
                        <tr>
                            <td style="width:130px; vertical-align:top;">
                                <img src="" alt="" width="110px" height="110px" />
                                <input type="hidden" name="PicId" />
                            </td>
                            <td style="width:70px;">
                                <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-remove',plain:true" onclick="$(this).parents('.row_col').remove()">删 除</a>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10">
        <span class="rl">访问量：</span>
        <div class="fl">
            <input type="text" id="txtViewCount" value="0" runat="server" clientidmode="Static"
                class="easyui-numberbox mtxt" style="width: 60px" />
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10">
        <span class="rl"><b class="cr">*</b>排序：</span>
        <div class="fl">
            <input type="text" id="txtSort" value="0" runat="server" clientidmode="Static" class="easyui-numberbox mtxt"
                data-options="required:true,missingMessage:'必填项'" style="width: 60px" />
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10">
        <span class="rl">是否禁用：</span>
        <div class="fl">
            <input type="radio" id="rdFalse" runat="server" name="rdIsDisable" value="false"
                clientidmode="Static" checked="true" /><label>否</label>
            <input type="radio" id="rdTrue" runat="server" name="rdIsDisable" value="true" clientidmode="Static" /><label>是</label>
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
                onclick="AddInformation.OnSave()">提交</a>
        </div>
        <span class="clr"></span>
    </div>
    <input type="hidden" id="hId" runat="server" clientidmode="Static" />
    
    <div id="dlgUploadPicture" style="padding: 10px;"></div>
    <div id="dlgMutilSelectPicture" class="easyui-dialog" title="选择图片（多选）" data-options="closed:true,modal:true,href:'/t/yt.html?dlgId=dlgMutilSelectPicture&funName=InformationPicture&isMutil=true',width:810,height:$(window).height()*0.8,
    buttons: [{
        id:'btnMutilSelectPicture',text:'确定',iconCls:'icon-ok',
        handler:function(){
            AddInformation.SetMutilPicture();
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
    <script type="text/javascript" src="../../Scripts/Admin/Ad/AddInformation.js"></script>
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
                AddInformation.Init();
            }
            catch (e) {
                $.messager.alert('错误提醒', e.name + ": " + e.message, 'error');
            }
        });
    </script>
</asp:Content>
