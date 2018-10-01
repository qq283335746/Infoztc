<%@ Page Title="个人基本信息设置" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AddUserBase.aspx.cs" Inherits="TygaSoft.Web.Admin.Sys.AddUserBase" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
 
    <div class="row mt10">
        <span class="rl"><b class="cr">*</b>昵称：</span>
        <div class="fl">
            <input type="text" id="txtNickname" runat="server" clientidmode="Static" class="easyui-textbox mtxt" data-options="required:true,missingMessage:'必填项'" />
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10">
        <span class="fl rl">头像：</span>
        <div class="fl ml10">
            <img id="imgSinglePicture" runat="server" clientidmode="Static" src="../../Images/nopic.gif" alt="图片" width="100" height="100" onclick="DlgSelectPicture.DlgSingle('UserHeadPicture')" /><br />
            <input type="hidden" id="hPictureId" runat="server" />
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10">
        <span class="rl"><b class="cr">*</b>性别：</span>
        <div class="fl">
            <asp:RadioButtonList ID="rbtnSex" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal"></asp:RadioButtonList>
        </div>
        <div class="clr"></div>
    </div>
    <div class="row mt10">
        <span class="rl"><b class="cr">*</b>手机号：</span>
        <div class="fl">
            <input type="text" id="txtMobilePhone" runat="server" clientidmode="Static" class="easyui-textbox mtxt" data-options="required:true,missingMessage:'必填项',validType:'phone'" />
        </div>
        <div class="clr"></div>
    </div>
    <div class="row mt10">
        <span class="rl">金币数：</span>
        <div class="fl">
            <input type="text" id="txtTotalGold" runat="server" clientidmode="Static" class="easyui-textbox mtxt" data-options="prompt:'请输入整数',validType:'int'" />
        </div>
        <div class="clr"></div>
    </div>
    <div class="row mt10">
        <span class="rl">元宝数：</span>
        <div class="fl">
            <input type="text" id="txtTotalSilver" runat="server" clientidmode="Static" class="easyui-textbox mtxt" data-options="prompt:'请输入整数',validType:'int'" />
        </div>
        <div class="clr"></div>
    </div>
    <div class="row mt10">
        <span class="rl">积分数：</span>
        <div class="fl">
            <input type="text" id="txtTotalIntegral" runat="server" clientidmode="Static" class="easyui-textbox mtxt" data-options="prompt:'请输入整数',validType:'int'" />
        </div>
        <div class="clr"></div>
    </div>
    <div class="row mt10">
        <span class="rl">VIP等级：</span>
        <div class="fl">
            <input type="text" id="txtSilverLevel" runat="server" clientidmode="Static" class="easyui-textbox mtxt" />
        </div>
        <div class="clr"></div>
    </div>
     <div class="row mt10">
        <span class="rl">积分等级：</span>
        <div class="fl">
            <input type="text" id="txtIntegralLevel" runat="server" clientidmode="Static" class="easyui-textbox mtxt" />
        </div>
        <div class="clr"></div>
    </div>
    <div class="row mt10">
        <span class="rl">颜色显示：</span>
        <div class="fl">
            <input type="text" id="txtColorLevel" runat="server" clientidmode="Static" class="easyui-textbox mtxt" data-options="prompt:'请输入整数',validType:'int'" />
        </div>
        <div class="clr"></div>
    </div>
    <div class="row mt10">
        <span class="rl">&nbsp;</span>
        <div class="fl">
        <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-save'" onclick="AddUserBase.OnSave()">提交</a>
        </div>
        <span class="clr"></span>
    </div>

    <input type="hidden" id="hName" runat="server" clientidmode="Static" />

    <script type="text/javascript" src="../../Scripts/Admin/DlgSelectPicture.js"></script>
    <script type="text/javascript" src="../../Scripts/Admin/Sys/AddUserBase.js"></script>

    <div id="dlgUploadPicture" style="padding:10px;"></div>
    <div id="dlgSingleSelectPicture" class="easyui-dialog" title="选择头像" data-options="closed:true,modal:true,href:'/t/yt.html?dlgId=dlgSingleSelectPicture&funName=UserHeadPicture',width:810,height:$(window).height()*0.8,
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

    <script type="text/javascript">
        $(function () {
            try {
                AddUserBase.Init();
            }
            catch (e) {
                $.messager.alert('错误提醒', e.name + ": " + e.message, 'error');
            }

        })

    </script>


</asp:Content>
