<%@ Page Title="新建推送" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="AddActivityPush.aspx.cs" Inherits="TygaSoft.Web.Admin.ActivityNew.AddActivityPush" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
        <span class="rl">内容：</span>
        <div class="fl">
            <textarea id="txtConetent" runat="server" clientidmode="Static" cols="100" rows="10"></textarea>
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10">
        <span class="rl">&nbsp;</span>
        <div class="fl">
            <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-save'"
                onclick="AddActivityPush.OnSave()">提交</a>
        </div>
        <span class="clr"></span>
    </div>

    <input type="hidden" id="hId" runat="server" clientidmode="Static" />

    <script type="text/javascript" src="../../Scripts/Admin/ActivityNew/AddActivityPush.js"></script>
    <script type="text/javascript">
        $(function () {
            try {
                AddActivityPush.Init();
            }
            catch (e) {
                $.messager.alert('错误提醒', e.name + ": " + e.message, 'error');
            }

        })
    </script>
</asp:Content>
