<%@ Page Title="新建选手" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="AddActivityPlayer.aspx.cs" Inherits="TygaSoft.Web.Admin.ActivityNew.AddActivityPlayer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <div class="row mt10">
        <span class="rl"><b class="cr">*</b>姓名：</span>
        <div class="fl">
            <input type="text" id="txtName" runat="server" clientidmode="Static" class="easyui-textbox mtxt" data-options="required:true,missingMessage:'必填项'"
                style="width: 200px" />
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10">
        <span class="rl"><b class="cr">*</b>年龄：</span>
        <div class="fl">
            <input type="text" id="txtAge" runat="server" clientidmode="Static" class="easyui-numberbox mtxt" data-options="required:true,missingMessage:'必填项'"
                style="width: 200px" />
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10">
        <span class="rl"><b class="cr">*</b>职业：</span>
        <div class="fl">
            <input type="text" id="txtOccupation" runat="server" clientidmode="Static" class="easyui-textbox mtxt" data-options="required:true,missingMessage:'必填项'"
                style="width: 200px" />
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10">
        <span class="rl"><b class="cr">*</b>电话：</span>
        <div class="fl">
            <input type="text" id="txtPhone" runat="server" clientidmode="Static" class="easyui-textbox mtxt" data-options="required:true,missingMessage:'必填项'"
                style="width: 200px" />
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10">
        <span class="rl"><b class="cr">*</b>所在地：</span>
        <div class="fl">
            <input type="text" id="txtLocation" runat="server" clientidmode="Static" class="easyui-textbox mtxt" data-options="required:true,missingMessage:'必填项'"
                style="width: 200px" />
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10">
        <span class="rl"><b class="cr">*</b>专业：</span>
        <div class="fl">
            <input type="text" id="txtProfessional" runat="server" clientidmode="Static" class="easyui-textbox mtxt" data-options="required:true,missingMessage:'必填项'"
                style="width: 200px" />
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10">
        <span class="fl rl"><b class="cr">*</b>头像：</span>
        <div class="fl ml10">
            <img id="imgSinglePicture" runat="server" clientidmode="Static" src="../../Images/nopic.gif"
                alt="图片" width="110" height="110" onclick="AddActivityPlayer.DlgSingle()" /><br />
            <input id="hImgSinglePictureId" runat="server" clientidmode="Static" type="hidden" />
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10">
        <span class="fl rl"><b class="cr">*</b>图片：</span>
        <div class="fl ml10">
            <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true" onclick="AddActivityPlayer.DlgSingleForMutil()">选 择</a>
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
        <span class="rl">个人详情：</span>
        <div class="fl">
            <textarea id="txtDescr" runat="server" clientidmode="Static" cols="100" rows="15"></textarea>
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
        <span class="rl">虚拟投票数：</span>
        <div class="fl">
            <input type="text" id="txtUpdateVoteCount" value="0" runat="server" clientidmode="Static"
                class="easyui-numberbox mtxt" style="width: 60px" />
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10">
        <span class="rl">是否禁用：</span>
        <div class="fl">
            <input type="radio" id="rdFalse" runat="server" name="rdIsDisable" value="false" checked="true" /><label>否</label>
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

    <div id="dlgUploadPicture" style="padding: 10px;"></div>
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
	}]" style="padding: 10px;"></div>

    <div id="dlgMutilSelectPicture" class="easyui-dialog" title="选择图片（多选）" data-options="closed:true,modal:true,href:'/t/yt.html?dlgId=dlgMutilSelectPicture&funName=ActivityPlayerPhotoPicture&isMutil=false',width:810,height:$(window).height()*0.8,
    buttons: [{
        id:'btnMutilSelectPicture',text:'确定',iconCls:'icon-ok',
        handler:function(){
            AddActivityPlayer.SetMutilPicture();
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
            DlgPictureSelect.DlgUpload();
        }
	}]" style="padding:10px;"></div>

    <script type="text/javascript" src="../../Scripts/Admin/DlgSelectPicture.js"></script>
    <script type="text/javascript" src="../../Scripts/Admin/ActivityNew/AddActivityPlayer.js"></script>
    <script type="text/javascript">
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
