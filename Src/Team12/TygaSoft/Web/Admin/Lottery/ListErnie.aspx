<%@ Page Title="摇奖设定列表" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ListErnie.aspx.cs" Inherits="TygaSoft.Web.Admin.Lottery.ListErnie" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">


<div id="toolbar" style="padding:5px;">
    开始时间：<input type="text" runat="server" id="txtStartTime" class="easyui-datebox txt" maxlength="50" />
    结束时间：<input type="text" runat="server" id="txtEndTime" class="easyui-datebox txt" maxlength="50" />
    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-search'" onclick="ListErnie.Search()">查 詢</a>
    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true" onclick="ListErnie.Add()">新 增</a>
    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-edit',plain:true" onclick="ListErnie.Edit()">编 辑</a>
    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-remove',plain:true" onclick="ListErnie.Del()">删 除</a>
</div>

<table id="dgT" class="easyui-datagrid" title="摇奖设定列表" data-options="rownumbers:true,pagination:true,fit:true,fitColumns:true,toolbar:'#toolbar'">
    <thead>
        <tr>
            <th data-options="field:'f0',checkbox:true"></th>
            <th data-options="field:'f1',width:120">开始时间</th>
            <th data-options="field:'f2',width:120">结束时间</th>
            <th data-options="field:'f3',width:120">最大摇奖次数</th>
            <th data-options="field:'f4',width:80">是否结束</th>
            <th data-options="field:'f5',width:80">是否禁用</th>
            <th data-options="field:'f6',width:120">最后更新时间</th>
           
        </tr>
    </thead>
    <tbody>
    <asp:Repeater ID="rpData" runat="server">
        <ItemTemplate>
            <tr>
                <td><%#Eval("Id")%></td>
                <td><%#((DateTime)Eval("StartTime")).ToString("yyyy-MM-dd HH:mm")%></td>
                <td><%#((DateTime)Eval("EndTime")).ToString("yyyy-MM-dd HH:mm")%></td>
                <td><%#Eval("UserBetMaxCount")%></td>
                <td><%#Eval("IsOver").ToString() == "1" ? "是" : "否"%></td>
                <td><%#Eval("IsDisable").ToString() == "1" ? "是" : "否"%></td>
                <td><%#((DateTime)Eval("LastUpdatedDate")).ToString("yyyy-MM-dd HH:mm")%></td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
    </tbody>
</table>

<asp:Literal runat="server" ID="ltrMyData"></asp:Literal>

<script type="text/javascript" src="../../Scripts/Admin/Lottery/ListErnie.js"></script>

<script type="text/javascript">
    var sPageIndex = 0;
    var sPageSize = 0;
    var sTotalRecord = 0;
    var sQueryStr = "";

    $(function () {
        try {
            var myData = ListErnie.GetMyData("myDataForPage");
            $.map(myData, function (item) {
                sPageIndex = parseInt(item.PageIndex);
                sPageSize = parseInt(item.PageSize);
                sTotalRecord = parseInt(item.TotalRecord);
                sQueryStr = item.QueryStr.replace(/&amp;/g, '&');
            })

            ListErnie.Init();
        }
        catch (e) {
            $.messager.alert('错误提醒', e.name + ": " + e.message, 'error');
        }

    })
</script>


</asp:Content>
