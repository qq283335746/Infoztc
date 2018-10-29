<%@ Page Title="新建题目" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="AddQuestionSubject.aspx.cs" Inherits="TygaSoft.Web.Admin.Question.AddQuestionSubject" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <div class="row mt10">
        <span class="rl"><b class="cr">*</b>题库名称：</span>
        <div class="fl">
            <select class="easyui-combobox" id="selectQB" runat="server" clientidmode="Static" style="width:200px">
            </select>
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10">
        <span class="rl"><b class="cr">*</b>题目类型：</span>
        <div class="fl">
            <select class="easyui-combobox" id="selectType" runat="server" clientidmode="Static" data-options="panelHeight:'auto'" style="width:200px">
                <option value="0">单选</option>
                <option value="1">多选</option>
            </select>
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10">
        <span class="rl"><b class="cr">*</b>题目内容：</span>
        <div class="fl">
            <input type="text" id="txtName" runat="server" clientidmode="Static" class="easyui-textbox mtxt"
                data-options="required:true,missingMessage:'必填项',multiline:true" style="height:60px" />
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
                onclick="AddQuestionSubject.OnSave()">提交</a>
        </div>
        <span class="clr"></span>
    </div>
    <input type="hidden" id="hId" runat="server" clientidmode="Static" />
    <input type="hidden" id="qbId" runat="server" clientidmode="Static" />
    <script type="text/javascript" src="../../Scripts/Admin/Question/AddQuestionSubject.js"></script>
    <script type="text/javascript">
        $(function () {
            try {
                AddQuestionSubject.Init();
            }
            catch (e) {
                $.messager.alert('错误提醒', e.name + ": " + e.message, 'error');
            }

        })

    </script>
</asp:Content>
