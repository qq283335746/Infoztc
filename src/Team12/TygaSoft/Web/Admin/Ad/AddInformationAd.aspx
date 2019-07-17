<%@ Page Title="新建/编辑广告" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AddInformationAd.aspx.cs" Inherits="TygaSoft.Web.Admin.Ad.AddInformationAd" %>
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
            <input type="radio" id="rdViewType0" runat="server" name="rdViewType" value="0"
                clientidmode="Static" checked="true" /><label>外部链接</label>
            <input type="radio" id="rdViewType1" runat="server" name="rdViewType" value="1" clientidmode="Static" /><label>页面内容</label>
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10">
        <span class="rl"><b class="cr">*</b>广告描述：</span>
        <div class="fl">
            <textarea id="txtDescr" runat="server" clientidmode="Static" cols="120" rows="5"></textarea>
        </div>
        <span class="clr"></span>
    </div>
    <div id="divContent" style="display:none;" class="row mt10" runat="server" clientidmode="Static" >
        <span class="rl"><b class="cr">*</b>内容：</span>
        <div class="fl">
            <textarea id="txtContent" runat="server" clientidmode="Static" style="width:859px;height:220px;"></textarea>
        </div>
        <span class="clr"></span>
    </div>
    <div id="divUrl" class="row mt10" runat="server" clientidmode="Static" >
        <span class="rl">地址：</span>
        <div class="fl">
            <input type="text" id="txtUrl" value="0" runat="server" clientidmode="Static" class="easyui-textbox mtxt"
                style="width: 500px" />
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10">
        <span class="fl rl">图片：</span>
        <div class="fl ml10">
            <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true" onclick="AddInformationAd.DlgSingleForMutil()">选 择</a>
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
        <span class="rl"><b class="cr">*</b>有效期：</span>
        <div class="fl">
            <input class="easyui-datebox mtxt" id="startDate" runat="server" clientidmode="Static"
                data-options="required:true,missingMessage:'必填项'" style="width: 150px" />-
            <input class="easyui-datebox mtxt" id="endDate" runat="server" clientidmode="Static"
                data-options="required:true,missingMessage:'必填项'" style="width: 150px" />
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10">
        <span class="rl">&nbsp;</span>
        <div class="fl">
            <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-save'"
                onclick="AddInformationAd.OnSave()">提交</a>
        </div>
        <span class="clr"></span>
    </div>
    <input type="hidden" id="hId" runat="server" clientidmode="Static" />
    
    
    <div id="dlgUploadPicture" style="padding: 10px;"></div>
    <div id="dlgMutilSelectPicture" class="easyui-dialog" title="选择图片（多选）" data-options="closed:true,modal:true,href:'/t/yt.html?dlgId=dlgMutilSelectPicture&funName=InformationPicture&isMutil=true',width:810,height:$(window).height()*0.8,
    buttons: [{
        id:'btnMutilSelectPicture',text:'确定',iconCls:'icon-ok',
        handler:function(){
            AddInformationAd.SetMutilPicture();
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
    <script type="text/javascript" src="../../Scripts/Admin/Ad/AddInformationAd.js"></script>
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
                AddInformationAd.Init();
            }
            catch (e) {
                $.messager.alert('错误提醒', e.name + ": " + e.message, 'error');
            }
        });
    </script>

</asp:Content>
