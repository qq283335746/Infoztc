<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ListActivityPrize.aspx.cs" Inherits="TygaSoft.Web.Admin.ActivityNew.ListActivityPrize" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
<div id="toolbar" style="padding:5px;">
    奖项名称：<input type="text" runat="server" id="txtName" maxlength="50" class="txt" />
    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-search'" onclick="ListActivityPrize.Search()">查 詢</a>
    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true" onclick="ListActivityPrize.Add()">新 增</a>
    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-edit',plain:true" onclick="ListActivityPrize.Edit()">编 辑</a>
    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-remove',plain:true" onclick="ListActivityPrize.Del()">删 除</a>
</div>

<table id="dgT" class="easyui-datagrid" title="奖项列表" data-options="rownumbers:true,pagination:true,fit:true,fitColumns:true,toolbar:'#toolbar'">
    <thead>
        <tr>
            <th data-options="field:'f0',checkbox:true"></th>
            <th data-options="field:'f1',width:100">奖项名称（数量）</th>
            <th data-options="field:'f2',width:100">奖品内容</th>      
            <th data-options="field:'f3',width:100">商家名称</th>
            <th data-options="field:'f4',width:50">预定中奖次数</th>
            <th data-options="field:'f5',width:50">排序</th>
            <th data-options="field:'f6',width:80">最后更新时间</th>
        </tr>
    </thead>
    <tbody>
    <asp:Repeater ID="rpData" runat="server">
        <ItemTemplate>
            <tr>
                <td><%#Eval("Id")%></td>
                <td><%#Eval("PrizeName")%>（<%#Eval("PrizeCount")%>）</td>
                <td><%#Eval("PrizeContent")%></td>
                <td><%#Eval("BusinessName")%></td>
                <td><%#Eval("WinningTimes")%></td>
                <td><%#Eval("Sort")%></td>
                <td><%#((DateTime)Eval("LastUpdatedDate")).ToString("yyyy-MM-dd HH:mm")%></td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
    </tbody>
</table>

<asp:Literal runat="server" ID="ltrMyData"></asp:Literal>
<input type="hidden" id="asId" runat="server" clientidmode="Static" />
<script type="text/javascript" src="../../Scripts/Admin/ActivityNew/ListActivityPrize.js"></script>

<script type="text/javascript">
    var sPageIndex = 0;
    var sPageSize = 0;
    var sTotalRecord = 0;
    var sQueryStr = "";

    $(function () {
        try {
            var myData = ListActivityPrize.GetMyData("myDataForPage");
            $.map(myData, function (item) {
                sPageIndex = parseInt(item.PageIndex);
                sPageSize = parseInt(item.PageSize);
                sTotalRecord = parseInt(item.TotalRecord);
                sQueryStr = item.QueryStr.replace(/&amp;/g, '&');
            })

            ListActivityPrize.Init();
        }
        catch (e) {
            $.messager.alert('错误提醒', e.name + ": " + e.message, 'error');
        }

    })
</script>
</asp:Content>
