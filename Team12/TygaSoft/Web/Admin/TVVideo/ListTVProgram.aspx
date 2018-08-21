<%@ Page Title="电视台视频二级列表" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ListTVProgram.aspx.cs" Inherits="TygaSoft.Web.Admin.TVVideo.ListTVProgram" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">

    <div id="toolbar" style="padding:5px;">
    名称：<input type="text" runat="server" id="txtName" maxlength="50" class="txt" />
    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-search'" onclick="ListTVProgram.Search()">查 詢</a>
    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true" onclick="ListTVProgram.Add()">新 增</a>
    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-edit',plain:true" onclick="ListTVProgram.Edit()">编 辑</a>
    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-remove',plain:true" onclick="ListTVProgram.Del()">删 除</a>
</div>

<table id="dgT" class="easyui-datagrid" title="电视频道二级列表" data-options="rownumbers:true,pagination:true,fit:true,fitColumns:true,toolbar:'#toolbar'">
    <thead>
        <tr>
            <th data-options="field:'f0',checkbox:true"></th>
            <th data-options="field:'f1',width:150">电视台名称</th>
            <th data-options="field:'f2',width:250">节目名称</th>
            <th data-options="field:'f3',width:250">节目链接</th>
            <th data-options="field:'f4',width:250">视频ID</th>
            <th data-options="field:'f5',width:250">点击次数</th>
            <th data-options="field:'f6',width:50">排序</th>
            <th data-options="field:'f7',width:80">是否禁用</th>
            <th data-options="field:'f8',width:120">最后更新时间</th>
        </tr>
    </thead>
    <tbody>
    <asp:Repeater ID="rpData" runat="server">
        <ItemTemplate>
            <tr>
                <td><%#Eval("Id")%></td>
                <td><%#Eval("HWTVName")%></td>
                <td><%#Eval("ProgramName")%></td>
                <td><%#Eval("ProgramAddress")%></td>
                <td><%#Eval("TVScID")%></td>
                <td><%#Eval("Time")%></td>
                <td><%#Eval("Sort")%></td>
                <td><%#Eval("IsDisable").ToString()=="1"?"是":"否" %></td>
                <td><%#((DateTime)Eval("LastUpdatedDate")).ToString("yyyy-MM-dd HH:mm")%></td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
    </tbody>
</table>

<asp:Literal runat="server" ID="ltrMyData"></asp:Literal>
<input type="hidden" id="hId" runat="server" clientidmode="Static" />
<script type="text/javascript" src="../../Scripts/Admin/TVVideo/ListTVProgram.js"></script>

<script type="text/javascript">
    var sPageIndex = 0;
    var sPageSize = 0;
    var sTotalRecord = 0;
    var sQueryStr = "";

    $(function () {
        try {
            var myData = ListTVProgram.GetMyData("myDataForPage");
            $.map(myData, function (item) {
                sPageIndex = parseInt(item.PageIndex);
                sPageSize = parseInt(item.PageSize);
                sTotalRecord = parseInt(item.TotalRecord);
                sQueryStr = item.QueryStr.replace(/&amp;/g, '&');
            })

            ListTVProgram.Init();
        }
        catch (e) {
            $.messager.alert('错误提醒', e.name + ": " + e.message, 'error');
        }

    })
</script>

</asp:Content>
