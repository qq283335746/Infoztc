<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ListWinningRecord.aspx.cs" Inherits="TygaSoft.Web.Admin.ActivityNew.ListWinningRecord" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
<div id="toolbar" style="padding:5px;">
    手机号码：<input type="text" runat="server" id="txtNum" maxlength="50" class="txt" />
    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-search'" onclick="ListWinningRecord.Search()">查 詢</a>
</div>

<table id="dgT" class="easyui-datagrid" title="中奖记录列表" data-options="rownumbers:true,pagination:true,fit:true,fitColumns:true,toolbar:'#toolbar'">
    <thead>
        <tr>
            <th data-options="field:'f0',checkbox:true"></th>
            <th data-options="field:'f1',width:60">用户名</th>
            <th data-options="field:'f2',width:60">用户昵称</th>      
            <th data-options="field:'f3',width:60">手机号码</th>
            <th data-options="field:'f4',width:100">奖项名称（数量）</th>
            <th data-options="field:'f5',width:100">奖品内容</th>
            <th data-options="field:'f6',width:70">中奖时间</th>
            <th data-options="field:'f7',width:40">领取状态</th>
            <th data-options="field:'f8',width:40">操作</th>
        </tr>
    </thead>
    <tbody>
    <asp:Repeater ID="rpData" runat="server">
        <ItemTemplate>
            <tr>
                <td><%#Eval("Id")%></td>
                <td><%#Eval("UserFlag")%></td>
                <td><%#Eval("Nickname")%></td>
                <td><%#Eval("MobilePhone")%></td>
                <td><%#Eval("PrizeName")%>（<%#Eval("PrizeCount")%>）</td>
                <td><%#Eval("PrizeContent")%></td>
                <td><%#((DateTime)Eval("LastUpdatedDate")).ToString("yyyy-MM-dd HH:mm")%></td>
                <td><%#Eval("Status").ToString() == "0" ? "未领取" : "已领取"%></td>
                <td><a href="javascript:void(0)" onclick="ListWinningRecord.UpdateStatus('<%#Eval("Id")%>','<%#Eval("Status")%>')" style="color:Blue"><%#Eval("Status").ToString() == "0" ? "领取" : "取消领取"%></a></td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
    </tbody>
</table>

<asp:Literal runat="server" ID="ltrMyData"></asp:Literal>
<input type="hidden" id="asId" runat="server" clientidmode="Static" />
<script type="text/javascript" src="../../Scripts/Admin/ActivityNew/ListWinningRecord.js"></script>

<script type="text/javascript">
    var sPageIndex = 0;
    var sPageSize = 0;
    var sTotalRecord = 0;
    var sQueryStr = "";

    $(function () {
        try {
            var myData = ListWinningRecord.GetMyData("myDataForPage");
            $.map(myData, function (item) {
                sPageIndex = parseInt(item.PageIndex);
                sPageSize = parseInt(item.PageSize);
                sTotalRecord = parseInt(item.TotalRecord);
                sQueryStr = item.QueryStr.replace(/&amp;/g, '&');
            })

            ListWinningRecord.Init();
        }
        catch (e) {
            $.messager.alert('错误提醒', e.name + ": " + e.message, 'error');
        }

    })
</script>
</asp:Content>
