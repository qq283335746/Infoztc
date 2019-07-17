<%@ Page Title="商品列表" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ListProduct.aspx.cs" Inherits="TygaSoft.Web.Admin.ECShop.ListProduct" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">

<div id="toolbar" style="padding:5px;">
    商品名称：<input type="text" id="txtName" maxlength="50" class="txt" />
    <select id="cbbMenu" class="easyui-combobox" style="width:130px;"
        data-options="url:'../../Handlers/Admin/HandlerSysEnum.ashx?reqName=GetJsonForCombobox&enumCode=CustomMenu',method:'get',valueField:'id',textField:'text'">
    </select>
    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-search'" onclick="ListProduct.Search()">查 詢</a>
    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true" onclick="ListProduct.Add()">新 增</a>
    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-edit',plain:true" onclick="ListProduct.Edit()">编 辑</a>
    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-remove',plain:true" onclick="ListProduct.Del()">删 除</a>
</div>

<table id="dgT" class="easyui-datagrid" title="商品列表" data-options="rownumbers:true,pagination:true,fit:true,fitColumns:true,toolbar:'#toolbar'">
    <thead>
        <tr>
            <th data-options="field:'f0',checkbox:true"></th>
            <th data-options="field:'f1'">商品</th>
            <th data-options="field:'f2'">商品名称</th>
            <th data-options="field:'f3'">排序</th>
            <th data-options="field:'f4'">上下架时间</th>
            <th data-options="field:'f5'">是否上架</th>
            <th data-options="field:'f6'">是否禁用</th>
            <th data-options="field:'f7'">所属分类</th>
            <th data-options="field:'f8'">所属品牌</th>
            <th data-options="field:'f9'">展现区</th>
        </tr>
    </thead>
    <tbody>
    <asp:Repeater ID="rpData" runat="server">
        <ItemTemplate>
            <tr>
                <td><%#Eval("ProductId")%></td>
                <td>
                    <img src='<%#string.Format("{0}{1}/PC/{1}_2{2}",Eval("FileDirectory"),Eval("RandomFolder"),Eval("FileExtension")) %>' alt="" width="50" height="50" /> 
                </td>
                <td><%#Eval("ProductName")%></td>
                <td><%#Eval("ProductSort")%></td>
                <td><%#Eval("ProductEnableStartTime")%><span class="mlr10">至</span><%#Eval("ProductEnableEndTime")%></td>
                <td><%#Eval("ProductIsEnable")%></td>
                <td><%#Eval("ProductIsDisable")%></td>
                <td><%#Eval("CategoryName")%></td>
                <td><%#Eval("BrandName")%></td>
                <td><%#Eval("MenuName")%></td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
    </tbody>
</table>

<asp:Literal runat="server" ID="ltrMyData"></asp:Literal>

<script type="text/javascript" src="../../Scripts/Admin/ECShop/ListProduct.js"></script>

<script type="text/javascript">
    var sPageIndex = 0;
    var sPageSize = 0;
    var sTotalRecord = 0;
    var sQueryStr = "";

    $(function () {
        try {
            var myData = ListProduct.GetMyData("myDataForPage");
            $.map(myData, function (item) {
                sPageIndex = parseInt(item.PageIndex);
                sPageSize = parseInt(item.PageSize);
                sTotalRecord = parseInt(item.TotalRecord);
                sQueryStr = item.QueryStr.replace(/&amp;/g, '&');
            })

            ListProduct.Init();
        }
        catch (e) {
            $.messager.alert('错误提醒', e.name + ": " + e.message, 'error');
        }

    })
</script>

</asp:Content>
