<%@ Page Title="新建答案选项" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="AddAnswerOption.aspx.cs" Inherits="TygaSoft.Web.Admin.Question.AddAnswerOption" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <div class="row mt10">
        <span class="rl"><b class="cr">*</b>选项内容：</span>
        <div class="fl">
            <input type="text" id="txtName" runat="server" clientidmode="Static" class="easyui-textbox mtxt"
                data-options="required:true,missingMessage:'必填项',multiline:true" style="height: 60px" />
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10">
        <span class="rl"><b class="cr">*</b>正确答案：</span>
        <div class="fl">
            <select class="easyui-combobox" id="selectIsTrue" runat="server" clientidmode="Static"
                data-options="panelHeight:'auto'" style="width: 200px">
                <option value="False">否</option>
                <option value="True">是</option>
            </select>
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10">
        <span class="rl">是否禁用：</span>
        <div class="fl">
            <input type="radio" id="rdFalse" runat="server" name="rdIsDisable" value="False" checked="true" /><label>否</label>
            <input type="radio" id="rdTrue" runat="server" name="rdIsDisable" value="True" /><label>是</label>
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10">
        <span class="rl">&nbsp;</span>
        <div class="fl">
            <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-save'"
                onclick="AddAnswerOption.OnSave()">提交</a>
        </div>
        <span class="clr"></span>
    </div>
    <input type="hidden" id="hId" runat="server" clientidmode="Static" />
    <input type="hidden" id="qsId" runat="server" clientidmode="Static" />
    <input type="hidden" id="sort" value="0" runat="server" clientidmode="Static" />
    <script type="text/javascript" src="../../Scripts/Admin/Question/AddAnswerOption.js"></script>
    <script type="text/javascript">
        $(function () {
            try {
                AddAnswerOption.Init();
            }
            catch (e) {
                $.messager.alert('错误提醒', e.name + ": " + e.message, 'error');
            }

        })

    </script>
</asp:Content>
