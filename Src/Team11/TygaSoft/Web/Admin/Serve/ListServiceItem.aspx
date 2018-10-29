<%@ Page Title="服务项目" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ListServiceItem.aspx.cs" Inherits="TygaSoft.Web.Admin.Serve.ListServiceItem" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">

<div id="toolbar" style="padding:5px;">
    名称：<input type="text" runat="server" id="txtName" maxlength="50" class="txt" />
    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-search'" onclick="ListServiceItem.Search()">查 詢</a>
    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true" onclick="ListServiceItem.Add()">新 增</a>
    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-edit',plain:true" onclick="ListServiceItem.Edit()">编 辑</a>
    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-remove',plain:true" onclick="ListServiceItem.Del()">删 除</a>
     <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true" onclick="ListServiceItem.AddChild()">下级列表</a>
</div>

<table id="dgT" class="easyui-datagrid" title="服务列表" data-options="rownumbers:true,pagination:true,fit:true,fitColumns:true,toolbar:'#toolbar'">
    <thead>
        <tr>
            <th data-options="field:'f0',checkbox:true"></th>
            <th data-options="field:'f1',hidden:true"></th>
            <th data-options="field:'f2',hidden:true"></th>
            <th data-options="field:'f3',width:100">图片</th>
            <th data-options="field:'f4',width:200">名称</th>
            <th data-options="field:'f5',width:200">链接地址</th>
            <th data-options="field:'f6',width:100">类别</th>
            <th data-options="field:'f7',width:120">最后更新时间</th>
            <th data-options="field:'f8',hidden:true"></th>
            <th data-options="field:'f9',hidden:true"></th>
        </tr>
    </thead>
    <tbody>
    <asp:Repeater ID="rpData" runat="server">
        <ItemTemplate>
            <tr>
                <td><%#Eval("Id")%></td>
                <td><%#Eval("ServicePictureId")%></td>
                <td><%#Eval("SysEnumId")%></td>
                <td><img src='../..<%#Eval("PictureUrl")%>' alt="" width="50px" height="50px" /> </td>
                <td><%#Eval("Named")%></td>
                <td><%#Eval("Url")%></td>
                <td><%#Eval("ServeType")%></td>
                <td><%#((DateTime)Eval("LastUpdatedDate")).ToString("yyyy-MM-dd HH:mm")%></td>  
                <td><%#Eval("PictureUrl")%></td>
                <td><%#Eval("EnumCode")%></td>               
            </tr>
        </ItemTemplate>
    </asp:Repeater>
    </tbody>
</table>

<div id="dlg"></div>
<div id="dlgAdd" style="padding:10px;"></div>

<asp:Literal runat="server" ID="ltrMyData"></asp:Literal>

<script type="text/javascript" src="../../Scripts/Admin/Serve/ListServiceItem.js"></script>

<script type="text/javascript">
    var sPageIndex = 0;
    var sPageSize = 0;
    var sTotalRecord = 0;
    var sQueryStr = "";

    $(function () {
        try {
            var myData = ListServiceItem.GetMyData("myDataForPage");
            $.map(myData, function (item) {
                sPageIndex = parseInt(item.PageIndex);
                sPageSize = parseInt(item.PageSize);
                sTotalRecord = parseInt(item.TotalRecord);
                sQueryStr = item.QueryStr.replace(/&amp;/g, '&');
            })

            ListServiceItem.Init();
        }
        catch (e) {
            $.messager.alert('错误提醒', e.name + ": " + e.message, 'error');
        }

    })
</script>

</asp:Content>
