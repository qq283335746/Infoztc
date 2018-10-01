<%@ Page Title="发布" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AddIssue.aspx.cs" Inherits="TygaSoft.Web.Admin.Question.AddIssue" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <div class="row mt10">
        <span class="rl"><b class="cr">*</b>标题：</span>
        <div class="fl">
            <input type="text" id="txtName" runat="server" clientidmode="Static" class="easyui-textbox mtxt"
                data-options="required:true,missingMessage:'必填项'" style="width:400px" />
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10">
        <span class="rl"><b class="cr">*</b>有效期：</span>
        <div class="fl">
            <input class="easyui-datetimebox mtxt" id="startDate" runat="server" clientidmode="Static" data-options="required:true,missingMessage:'必填项',showSeconds: false" style="width:150px"/>-
            <input class="easyui-datetimebox mtxt" id="endDate" runat="server" clientidmode="Static" data-options="required:true,missingMessage:'必填项',showSeconds: false" style="width:150px"/>
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10">
        <span class="rl"><b class="cr">*</b>题目数：</span>
        <div class="fl">
            <input type="text" id="txtCount" value="10" runat="server" clientidmode="Static" class="easyui-numberbox mtxt"
                data-options="required:true,missingMessage:'必填项'" style="width:60px" />&nbsp;道
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10">
        <span class="rl"><b class="cr">*</b>题库组合：</span>
        <div class="fl" id="qbList" runat="server">
            
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
                onclick="AddIssue.OnSave()">提交</a>
        </div>
        <span class="clr"></span>
    </div>
    <input type="hidden" id="hId" runat="server" clientidmode="Static" />
    <script type="text/javascript" src="../../Scripts/Admin/Question/AddIssue.js"></script>
    <script type="text/javascript">
        $(function () {
            try {
                AddIssue.Init();
            }
            catch (e) {
                $.messager.alert('错误提醒', e.name + ": " + e.message, 'error');
            }

        })

    </script>
</asp:Content>