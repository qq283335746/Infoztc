<%@ Page Title="新建话题评论" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="AddTopicComment.aspx.cs" Inherits="TygaSoft.Web.Admin.Topic.AddTopicComment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <div class="row mt10">
        <span class="rl"><b class="cr">*</b>内容：</span>
        <div class="fl">
            <textarea id="txtContent" runat="server" clientidmode="Static" style="overflow: auto; height: 150px; width: 600px;
                border: 1px solid #95B8E7;"></textarea>
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10">
        <span class="rl"><b class="cr">*</b>评论人：</span>
        <div class="fl">
            <select class="easyui-combobox" id="selectUser" runat="server" clientidmode="Static"
                data-options="required:true,missingMessage:'必填项'" style="width: 200px">
            </select>
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10">
        <span class="rl">是否禁用：</span>
        <div class="fl">
            <input type="radio" id="rdFalse" runat="server" clientidmode="Static" name="rdIsDisable" value="False"
                checked="true" /><label>否</label>
            <input type="radio" id="rdTrue" runat="server" clientidmode="Static" name="rdIsDisable" value="True" /><label>是</label>
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10">
        <span class="rl">&nbsp;</span>
        <div class="fl">
            <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-save'"
                onclick="AddTopicComment.OnSave()">提交</a>
        </div>
        <span class="clr"></span>
    </div>
    <input type="hidden" id="hId" runat="server" clientidmode="Static" />
    <input type="hidden" id="tsId" runat="server" clientidmode="Static" />
    <input type="hidden" id="isTop" value="False" runat="server" clientidmode="Static" />
    <input type="hidden" id="UserId" runat="server" clientidmode="Static" />
    <script type="text/javascript" src="../../Scripts/Admin/Topic/AddTopicComment.js"></script>
    <script type="text/javascript">
        $(function () {
            try {
                AddTopicComment.Init();
            }
            catch (e) {
                $.messager.alert('错误提醒', e.name + ": " + e.message, 'error');
            }

        })

    </script>
    <style scoped="scoped">
        .textbox
        {
            height: 20px;
            margin: 0;
            padding: 0 2px;
            box-sizing: content-box;
        }
    </style>
</asp:Content>
