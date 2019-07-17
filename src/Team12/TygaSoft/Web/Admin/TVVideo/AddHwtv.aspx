<%@ Page Title="新建电视台视频" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AddHwtv.aspx.cs" Inherits="TygaSoft.Web.Admin.TVVideo.AddHwtv" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">

    <div class="row mt10">
        <span class="rl"><b class="cr">*</b>电视台名称：</span>
        <div class="fl">
            <input type="text" id="txtName" runat="server" clientidmode="Static" class="easyui-textbox mtxt"
                data-options="required:true,missingMessage:'必填项'" style="width: 300px" />
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10">
        <span class="rl"><b class="cr">*</b>电视台图标：</span>
        <div class="fl">
            <img id="imgSinglePicture" src="../../Images/nopic.gif" alt="图片" width="120" height="120" onclick="DlgSelectPicture.DlgSingle('CommunionPicture')" runat="server" clientidmode="Static"/><br />
            <input type="hidden" id="hdimg" runat="server" clientidmode="Static"/>
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10">
        <span class="rl"><b class="cr">*</b>是否外部链接：</span>
        <div class="fl">
             <input id="selectType" class="easyui-combobox" data-options="valueField: 'value', textField: 'label',
                 data: [{label: '是',value: 'True'},{label: '否', value: 'False'  }]" />
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10" id="divProgramURL" style="display:none">
        <span class="rl">外部地址：</span>
        <div class="fl">
            <input type="text" id="txtProgramURL" runat="server" clientidmode="Static" class="easyui-textbox mtxt"
                 style="width: 300px" />
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
        <span class="rl"><b class="cr">*</b>排序：</span>
        <div class="fl">
            <input type="text" id="txtSort" runat="server" clientidmode="Static" class="easyui-textbox mtxt"
                 style="width: 30px" data-options="required:true,missingMessage:'必填项'"/>
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10">
        <span class="rl">&nbsp;</span>
        <div class="fl">
            <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-save'"
                onclick="AddHwtv.IsValid();">提交</a>
        </div>
        <span class="clr"></span>
    </div>

    <input type="hidden" id="hId" runat="server" clientidmode="Static" />
    <input type="hidden" id="hTurnTo" runat="server" clientidmode="Static" />

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

    <script type="text/javascript" src="../../Scripts/Admin/DlgSelectPicture.js"></script>
    <script type="text/javascript" src="../../Scripts/Admin/TVVideo/AddHwtv.js"></script>
    <script type="text/javascript">
        $(function () {
            try {
                AddHwtv.Init();
            }
            catch (e) {
                $.messager.alert('错误提醒', e.name + ": " + e.message, 'error');
            }
            

        });
    </script>

</asp:Content>
