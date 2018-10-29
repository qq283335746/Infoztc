﻿<%@ Page Title="广告列表" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ListInformationAd.aspx.cs" Inherits="TygaSoft.Web.Admin.Ad.ListInformationAd" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">

<div id="toolbar" style="padding:5px;">
    标题：<input type="text" runat="server" id="txtTitle" maxlength="50" class="txt" />
    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-search'" onclick="ListInformationAd.Search()">查 詢</a>
    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true" onclick="ListInformationAd.Add()">新 增</a>
    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-edit',plain:true" onclick="ListInformationAd.Edit()">编 辑</a>
    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-remove',plain:true" onclick="ListInformationAd.Del()">删 除</a>
</div>


<table id="dgT" class="easyui-datagrid" title="资讯列表" data-options="rownumbers:true,pagination:true,fit:true,fitColumns:true,toolbar:'#toolbar'">
    <thead>
        <tr>
            <th data-options="field:'f0',checkbox:true"></th>
            <th data-options="field:'f1',width:100">标题</th>
            <th data-options="field:'f2',width:100">描述说明</th>   
            <th data-options="field:'f3',width:50">类型</th>
            <th data-options="field:'f4',width:50">有效期开始时间</th>
            <th data-options="field:'f5',width:50">有效期结束时间</th>
            <th data-options="field:'f6',width:50">最后更新时间</th>
        </tr>
    </thead>
    <tbody>
    <asp:Repeater ID="rpData" runat="server">
        <ItemTemplate>
            <tr>
                <td><%#Eval("Id")%></td>
                <td><%#Eval("Title")%></td>
                <td><%#Eval("Descr")%></td>
                <td><%#Eval("ViewType").ToString() == "1" ? "页面内容" : "外部链接"%></td>
                <td><%#((DateTime)Eval("StartDate")).ToString("yyyy-MM-dd")%></td>
                <td><%#((DateTime)Eval("EndDate")).ToString("yyyy-MM-dd")%></td>
                <td><%#((DateTime)Eval("LastUpdatedDate")).ToString("yyyy-MM-dd HH:mm")%></td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
    </tbody>
</table>

<asp:Literal runat="server" ID="ltrMyData"></asp:Literal>
<script type="text/javascript" src="../../Scripts/Admin/Ad/ListInformationAd.js"></script>

<script type="text/javascript">
    var sPageIndex = 0;
    var sPageSize = 0;
    var sTotalRecord = 0;
    var sQueryStr = "";

    $(function () {
        try {
            var myData = ListInformationAd.GetMyData("myDataForPage");
            $.map(myData, function (item) {
                sPageIndex = parseInt(item.PageIndex);
                sPageSize = parseInt(item.PageSize);
                sTotalRecord = parseInt(item.TotalRecord);
                sQueryStr = item.QueryStr.replace(/&amp;/g, '&');
            })

            ListInformationAd.Init();
        }
        catch (e) {
            $.messager.alert('错误提醒', e.name + ": " + e.message, 'error');
        }

    })
</script>

</asp:Content>
