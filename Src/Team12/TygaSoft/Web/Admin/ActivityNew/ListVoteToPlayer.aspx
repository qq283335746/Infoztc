<%@ Page Title="投票统计"  Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ListVoteToPlayer.aspx.cs" Inherits="TygaSoft.Web.Admin.ActivityNew.ListVoteToPlayer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
<div id="toolbar" style="padding:5px;">
    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-search'" onclick="ListVoteToPlayer.Search()">查 詢</a>
</div>

<table id="dgT" class="easyui-datagrid" title="投票列表" data-options="rownumbers:true,pagination:true,fit:true,fitColumns:true,toolbar:'#toolbar'">
    <thead>
        <tr>
            <th data-options="field:'f0',checkbox:true"></th>
            <th data-options="field:'f1',width:100">选手姓名</th>     
            <th data-options="field:'f2',width:100">投票用户名</th>
            <th data-options="field:'f3',width:100">投票用户昵称</th>
            <th data-options="field:'f4',width:100">票数</th>
            <th data-options="field:'f5',width:100">最后更新时间</th>
        </tr>
    </thead>
    <tbody>
    <asp:Repeater ID="rpData" runat="server">
        <ItemTemplate>
            <tr>
                <td><%#Eval("Id")%></td>
                <td><%#Eval("Named")%></td>
                <td><%#Eval("UserFlag")%></td>
                <td><%#Eval("Nickname")%></td>
                <td><%#Eval("TotalVoteCount")%></td>
                <td><%#((DateTime)Eval("LastUpdatedDate")).ToString("yyyy-MM-dd HH:mm")%></td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
    </tbody>
</table>

<asp:Literal runat="server" ID="ltrMyData"></asp:Literal>
<input type="hidden" id="apId" runat="server" clientidmode="Static" />
<script type="text/javascript" src="../../Scripts/Admin/ActivityNew/ListVoteToPlayer.js"></script>

<script type="text/javascript">
    var sPageIndex = 0;
    var sPageSize = 0;
    var sTotalRecord = 0;
    var sQueryStr = "";

    $(function () {
        try {
            var myData = ListVoteToPlayer.GetMyData("myDataForPage");
            $.map(myData, function (item) {
                sPageIndex = parseInt(item.PageIndex);
                sPageSize = parseInt(item.PageSize);
                sTotalRecord = parseInt(item.TotalRecord);
                sQueryStr = item.QueryStr.replace(/&amp;/g, '&');
            })

            ListVoteToPlayer.Init();
        }
        catch (e) {
            $.messager.alert('错误提醒', e.name + ": " + e.message, 'error');
        }

    })
</script>
</asp:Content>