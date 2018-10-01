<%@ Page Title="订单列表" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ListOrder.aspx.cs" Inherits="TygaSoft.Web.Admin.ECShop.ListOrder" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">


<div id="toolbar" style="padding:5px;">
    关键字：<input type="text" runat="server" id="txtName" maxlength="50" class="txt" />
    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-search'" onclick="ListCart.Search()">查 詢</a>
    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-remove',plain:true" onclick="ListCart.Del()">删 除</a>
</div>

<table id="dgT" class="easyui-datagrid" title="订单列表" data-options="rownumbers:true,pagination:true,fit:true,fitColumns:true,toolbar:'#toolbar'">
    <thead>
        <tr>
            <th data-options="field:'f0',checkbox:true"></th>
            <th data-options="field:'f1',width:100">用户</th>
            <th data-options="field:'f2',width:100">订单编号</th>
            <th data-options="field:'f3',width:120">下单日期</th>
            <th data-options="field:'f4',width:100">收货人</th>
            <th data-options="field:'f5',width:80">金额</th>
            <th data-options="field:'f6',width:100">状态</th>
           
        </tr>
    </thead>
    <tbody>
    <asp:Repeater ID="rpData" runat="server">
        <ItemTemplate>
            <tr>
                <td><%#Eval("Id")%></td>
                <td><%#Eval("UserName")%></td>
                <td><%#Eval("OrderNum")%></td>
                <td><%#((DateTime)Eval("CreateDate")).ToString("yyyy-MM-dd HH:mm")%></td>
                <td><%#Eval("Receiver")%></td>
                <td><%#Eval("TotalPrice")%></td>
                <td><%#Eval("Status")%></td>
                
            </tr>
        </ItemTemplate>
    </asp:Repeater>
    </tbody>
</table>

<asp:Literal runat="server" ID="ltrMyData"></asp:Literal>

<script type="text/javascript" src="../../Scripts/Admin/ECShop/ListCart.js"></script>

<script type="text/javascript">
    var sPageIndex = 0;
    var sPageSize = 0;
    var sTotalRecord = 0;
    var sQueryStr = "";

    $(function () {
        try {
            var myData = ListCart.GetMyData("myDataForPage");
            $.map(myData, function (item) {
                sPageIndex = parseInt(item.PageIndex);
                sPageSize = parseInt(item.PageSize);
                sTotalRecord = parseInt(item.TotalRecord);
                sQueryStr = item.QueryStr.replace(/&amp;/g, '&');
            })

            ListCart.Init();
        }
        catch (e) {
            $.messager.alert('错误提醒', e.name + ": " + e.message, 'error');
        }

    })
</script>

</asp:Content>
