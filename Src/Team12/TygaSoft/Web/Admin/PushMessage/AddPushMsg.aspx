<%@ Page Title="新建推送" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AddPushMsg.aspx.cs" Inherits="TygaSoft.Web.Admin.PushMessage.AddPushMsg" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../../Scripts/JeasyuiExtend.js" type="text/javascript"></script>
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
        <span class="rl"><b class="cr">*</b>内容：</span>
        <div class="fl">
            <textarea id="txtPushContent" runat="server" clientidmode="Static" cols="120" rows="5"></textarea>
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10">
        <span class="rl"><b class="cr">*</b>发送范围：</span>
        <div class="fl">
            <input type="checkbox" id="ckAll" runat="server" name="rdAll" 
                clientidmode="Static" checked="true" /><label>全部</label>
            <a href="javascript:void(0);" onclick="AddPushMsg.OpenInforAdWin('<%#Eval("Id")%>');$(this).parent().click();return false;" style="color:Blue">选择用户</a>
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10">
        <span class="rl">&nbsp;</span>
        <div class="fl">
            <textarea id="txtSendRange" runat="server" clientidmode="Static" cols="120" rows="3"></textarea>
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10">
        <span class="rl">&nbsp;</span>
        <div class="fl">
            <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-save'"
                onclick="AddPushMsg.OnSave()">提交</a>
        </div>
        <span class="clr"></span>
    </div>
    <input type="hidden" id="hId" runat="server" clientidmode="Static" />

    
<%--用户选择--%>
    <div id="RelInforAd" class="easyui-dialog" title="用户选择" style="width: 550px;
        height: 450px; overflow: auto;" modal="true" closed="true" buttons="#RelClass-buttons">
        <div style="margin-left: 10px; margin-top: 5px;"><span class="fl">用户名：</span>
        <div class="fl">
            <input type="text" id="userName" runat="server" clientidmode="Static" style="width: 116px" />
        </div>
        <div class="fl" style="margin-left:5px;">
            <button onclick="AddPushMsg.SearchUserList('')">查询</button>
        </div>
        <span class="clr"></span>
        </div>
        <div style=" float: left; margin:10px 0 0 10px">
            <div style="width: 220px; height: 335px; padding: 1px;" title="用户列表" class="easyui-panel">
                <select id="selectList" multiple="multiple" style="width: 100%; height: 100%;">
                </select>
            </div>
        </div>
        <div style=" float:left;width: 50px; height: 335px;margin:10px 0 0 0">
        <a id="btnAddClass" href="javascript:void(0);" class="easyui-linkbutton" style="margin:150px 0 0 18px" onclick="AddPushMsg.AddPushUser()">>>></a>
        <a id="btnDelClass" href="javascript:void(0);" class="easyui-linkbutton" style="margin:50px 0 0 18px" onclick="AddPushMsg.DelPushUser()"><<<</a>
        </div>
        <div style="float: right; margin: 10px 10px 0 0">
            <div style="width: 220px; height: 335px; padding: 1px;" title="已选用户" class="easyui-panel">
                <select id="selectedList" multiple="multiple" style="width: 100%; height: 100%;">
                </select>
            </div>
        </div>
    </div>
     <div id="RelClass-buttons">
     
        <a id="page">11</a>
        <a id="btn_Submit" href="javascript:void(0);" class="easyui-linkbutton" onclick="AddPushMsg.GetSelectedUser();$('#RelInforAd').dialog('close');return false;">确定</a> <a href="javascript:void(0);"
            class="easyui-linkbutton" onclick="$('#RelInforAd').dialog('close');return false;">
            取消</a>
    </div>
    <%--用户选择 end--%>


<script type="text/javascript" src="../../Scripts/Admin/PushMessage/AddPushMsg.js"></script>
<script type="text/javascript">
    var gPageNumber = 1;
    var gPageSize = 20;
    var gTotalSize = 0;
    $(function () {
        try {
            AddPushMsg.Init();
        }
        catch (e) {
            $.messager.alert('错误提醒', e.name + ": " + e.message, 'error');
        }
    });
    </script>
</asp:Content>
