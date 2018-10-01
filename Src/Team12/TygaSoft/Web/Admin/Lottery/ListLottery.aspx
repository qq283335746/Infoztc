<%@ Page Title="彩票列表" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ListLottery.aspx.cs" Inherits="TygaSoft.Web.Admin.Lottery.ListLottery" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
<div id="toolbar" style="padding:5px;">
    期号：<input type="text" runat="server" id="txtQS" maxlength="50" class="txt" />
    海南期号：<input type="text" runat="server" id="txtHNQS" maxlength="50" class="txt" />
    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-search'" onclick="ListLottery.Search()">查 詢</a>
    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true" onclick="ListLottery.Add()">新 增</a>
    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-edit',plain:true" onclick="ListLottery.Edit()">编 辑</a>
    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-remove',plain:true" onclick="ListLottery.Del()">删 除</a>
</div>

<table id="dgT" class="easyui-datagrid" title="题库列表" data-options="rownumbers:true,pagination:true,fit:true,fitColumns:true,toolbar:'#toolbar'">
    <thead>
        <tr>
            <th data-options="field:'f0',checkbox:true"></th>
            <th data-options="field:'f1',width:60">期号</th>
            <th data-options="field:'f2',width:60">海南期号</th>
            <th data-options="field:'f3',width:80">开奖时间</th>
            <th data-options="field:'f4',width:60">开奖号码</th>       
            <th data-options="field:'f5',width:60">兑奖截止时间</th>
            <th data-options="field:'f6',width:60">本期销量</th>
            <th data-options="field:'f7',width:60">奖池滚存</th>  
            <th data-options="field:'f8',width:60">操作人</th> 
            <th data-options="field:'f9',width:60">最后更新时间</th>  
        </tr>
    </thead>
    <tbody>
    <asp:Repeater ID="rpData" runat="server">
        <ItemTemplate>
            <tr>
                <td><%#Eval("Id")%></td>
                <td><%#Eval("QS")%></td>
                <td><%#Eval("HNQS")%></td>
                <td><%#((DateTime)Eval("LotteryTime")).ToString("yyyy-MM-dd HH:mm")%></td>
                <td><%#Eval("LotteryNo")%></td>
                <td><%#((DateTime)Eval("ExpiryClosingDate")).ToString("yyyy-MM-dd").Replace("1754-01-01", "")%></td>
                <td><%#Eval("SalesVolume").ToString() == "0" ? "" : Eval("SalesVolume")%></td>
                <td><%#Eval("Progressive").ToString() == "0" ? "" : Eval("Progressive")%></td>
                <td><%#Eval("UserId").Equals(Guid.Empty) ? "auto" : Eval("UserName")%></td>
                <td><%#((DateTime)Eval("LastUpdatedDate")).ToString("yyyy-MM-dd HH:mm")%></td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
    </tbody>
</table>

<asp:Literal runat="server" ID="ltrMyData"></asp:Literal>
<input type="hidden" id="qbSelectId" runat="server" clientidmode="Static" />
<script type="text/javascript" src="../../Scripts/Admin/Lottery/ListLottery.js"></script>

<script type="text/javascript">
    var sPageIndex = 0;
    var sPageSize = 0;
    var sTotalRecord = 0;
    var sQueryStr = "";

    $(function () {
        try {
            var myData = ListLottery.GetMyData("myDataForPage");
            $.map(myData, function (item) {
                sPageIndex = parseInt(item.PageIndex);
                sPageSize = parseInt(item.PageSize);
                sTotalRecord = parseInt(item.TotalRecord);
                sQueryStr = item.QueryStr.replace(/&amp;/g, '&');
            })

            ListLottery.Init();
        }
        catch (e) {
            $.messager.alert('错误提醒', e.name + ": " + e.message, 'error');
        }

    })
</script>
</asp:Content>