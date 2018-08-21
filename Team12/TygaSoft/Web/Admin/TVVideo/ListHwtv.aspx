<%@ Page Title="电视台视频列表" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ListHwtv.aspx.cs" Inherits="TygaSoft.Web.Admin.TVVideo.ListHwtv" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">

    <div id="toolbar" style="padding:5px;">
    名称：<input type="text" runat="server" id="txtName" maxlength="50" class="txt" />
    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-search'" onclick="ListHwtv.Search()">查 詢</a>
    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true" onclick="ListHwtv.Add()">新 增</a>
    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-edit',plain:true" onclick="ListHwtv.Edit()">编 辑</a>
    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-remove',plain:true" onclick="ListHwtv.Del()">删 除</a>
</div>

<table id="dgT" class="easyui-datagrid" title="电视频道" data-options="rownumbers:true,pagination:true,fit:true,fitColumns:true,toolbar:'#toolbar'">
    <thead>
        <tr>
            <th data-options="field:'f0',checkbox:true"></th>
            <th data-options="field:'f1',width:150">电视台名称</th>
            <th data-options="field:'f2',width:250">转跳地址</th>
            <th data-options="field:'f3',width:50">排序</th>
            <th data-options="field:'f4',width:50">是否禁用</th>
            <th data-options="field:'f5',width:120">最后更新时间</th>
            <th data-options="field:'f6',width:50">操作</th>
        </tr>
    </thead>
    <tbody>
    <asp:Repeater ID="rpData" runat="server">
        <ItemTemplate>
            <tr>
                <td><%#Eval("Id")%></td>
                <td><%#Eval("HWTVName")%></td>
                <td><%#Eval("ProgramAddress")%></td>
                <td><%#Eval("Sort")%></td>
                <td><%#Eval("IsDisable").ToString()=="1"?"是":"否" %></td>
                <td><%#((DateTime)Eval("LastUpdatedDate")).ToString("yyyy-MM-dd HH:mm")%></td>
                <td><a href="gtv.html?qsId=<%#Eval("Id")%>" style="color:Blue">
                                        <span><%#Eval("IsTurnTo").ToString()=="False"?"查看二级<br/>节目表":"" %></span></a></td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
    </tbody>
</table>

<asp:Literal runat="server" ID="ltrMyData"></asp:Literal>

<script type="text/javascript" src="../../Scripts/Admin/TVVideo/ListHwtv.js"></script>

<script type="text/javascript">
    var sPageIndex = 0;
    var sPageSize = 0;
    var sTotalRecord = 0;
    var sQueryStr = "";

    $(function () {
        try {
            var myData = ListHwtv.GetMyData("myDataForPage");
            $.map(myData, function (item) {
                sPageIndex = parseInt(item.PageIndex);
                sPageSize = parseInt(item.PageSize);
                sTotalRecord = parseInt(item.TotalRecord);
                sQueryStr = item.QueryStr.replace(/&amp;/g, '&');
            })

            ListHwtv.Init();
        }
        catch (e) {
            $.messager.alert('错误提醒', e.name + ": " + e.message, 'error');
        }

    })
</script>

</asp:Content>
