<%@ Page Title="新建电视台视频二级" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AddTVProgram.aspx.cs" Inherits="TygaSoft.Web.Admin.TVVideo.AddTVProgram" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">

    <div class="row mt10">
        <span class="rl"><b class="cr">*</b>节目名称：</span>
        <div class="fl">
            <input type="text" id="txtName" runat="server" clientidmode="Static" class="easyui-textbox mtxt"
                data-options="required:true,missingMessage:'必填项'" style="width: 300px" />
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10" >
        <span class="rl"><b class="cr">*</b>节目链接：</span>
        <div class="fl">
            <input type="text" id="txtProgramURL" runat="server" clientidmode="Static" class="easyui-textbox mtxt"
                 data-options="required:true,missingMessage:'必填项'" style="width: 300px" />
        </div>
        <span class="clr"></span>
    </div>
     <div class="row mt10" >
        <span class="rl"><b class="cr">*</b>视频ID：</span>
        <div class="fl">
            <input type="text" id="txtTVScID" runat="server" clientidmode="Static" class="easyui-textbox mtxt"
                 data-options="required:true,missingMessage:'必填项'" style="width: 300px" />
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10">
        <span class="rl"><b class="cr">*</b>是否禁用：</span>
        <div class="fl">
            <input type="radio" id="rdFalse" runat="server" name="rdIsDisable" value="false"
                checked="true" /><label>否</label>
            <input type="radio" id="rdTrue" runat="server" name="rdIsDisable" value="true" /><label>是</label>
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10">
        <span class="rl">排序：</span>
        <div class="fl">
            <input type="text" id="txtSort" runat="server" clientidmode="Static" class="easyui-textbox mtxt"
                 style="width: 30px" />
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10">
        <span class="rl">&nbsp;</span>
        <div class="fl">
            <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-save'"
                onclick="AddTVProgram.OnSave()">提交</a>
        </div>
        <span class="clr"></span>
    </div>
    <input type="hidden" id="hId" runat="server" clientidmode="Static" />
    <input type="hidden" id="hHWid" runat="server" clientidmode="Static" />
    <script type="text/javascript" src="../../Scripts/Admin/TVVideo/AddTVProgram.js"></script>
    <script type="text/javascript">
        $(function () {
            try {
                AddTVProgram.Init();
            }
            catch (e) {
                $.messager.alert('错误提醒', e.name + ": " + e.message, 'error');
            }
            

        });
    </script>

</asp:Content>
