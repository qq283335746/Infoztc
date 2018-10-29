<%@ Page Title="话题评论列表" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ListTopicComment.aspx.cs" Inherits="TygaSoft.Web.Admin.Topic.ListTopicComment" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
<div id="toolbar" style="padding:5px;">
    内容：<input type="text" runat="server" id="txtContent" maxlength="50" class="txt" />
    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-search'" onclick="ListTopicComment.Search()">查 詢</a>
    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true" onclick="ListTopicComment.Add()">新 增</a>
    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-edit',plain:true" onclick="ListTopicComment.Edit()">编 辑</a>
    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-remove',plain:true" onclick="ListTopicComment.Del()">删 除</a>
</div>

<table id="dgT" class="easyui-datagrid" title="评论列表" data-options="rownumbers:true,pagination:true,fit:true,fitColumns:true,toolbar:'#toolbar'">
    <thead>
        <tr>
            <th data-options="field:'f0',checkbox:true"></th>
            <th data-options="field:'f1',width:250">话题内容</th>
            <th data-options="field:'f2',width:250">评论内容</th>
            <th data-options="field:'f3',width:100">评论人</th>      
            <th data-options="field:'f4',width:100">最后更新时间</th>
            <th data-options="field:'f5',width:60">操作</th>
            <th data-options="field:'f6'" hidden="true"></th>
        </tr>
    </thead>
    <tbody>
    <asp:Repeater ID="rpData" runat="server">
        <ItemTemplate>
            <tr>
                <td><%#Eval("Id")%></td>
                <td><%#Eval("TSContentText").ToString().Length > 50 ? Eval("TSContentText").ToString().Substring(0, 50) + "......" : Eval("TSContentText")%></td>
                <td><%#Eval("ContentText").ToString().Length > 50 ? Eval("ContentText").ToString().Substring(0, 50) + "......" : Eval("ContentText")%></td>
                <td><%#Eval("UserName")%></td>
                <td><%#((DateTime)Eval("LastUpdatedDate")).ToString("yyyy-MM-dd HH:mm")%></td>
                <td style="text-align:center"><a href="javascript:void(0)" style="color:Blue" onclick="ListTopicComment.UpdateIsTop('<%#Eval("Id")%>','<%#Eval("IsTop").Equals(false) ? "true" : "false"%>')"><%#Eval("IsTop").Equals(false) ? "置顶" : "取消置顶"%></a></td>
                <td><%#Eval("IsTop")%></td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
    </tbody>
</table>

<asp:Literal runat="server" ID="ltrMyData"></asp:Literal>
<input type="hidden" id="tsId" runat="server" clientidmode="Static" />
<script type="text/javascript" src="../../Scripts/Admin/Topic/ListTopicComment.js"></script>

<script type="text/javascript">
    var sPageIndex = 0;
    var sPageSize = 0;
    var sTotalRecord = 0;
    var sQueryStr = "";

    $(function () {
        try {
            var myData = ListTopicComment.GetMyData("myDataForPage");
            $.map(myData, function (item) {
                sPageIndex = parseInt(item.PageIndex);
                sPageSize = parseInt(item.PageSize);
                sTotalRecord = parseInt(item.TotalRecord);
                sQueryStr = item.QueryStr.replace(/&amp;/g, '&');
            })

            ListTopicComment.Init();
        }
        catch (e) {
            $.messager.alert('错误提醒', e.name + ": " + e.message, 'error');
        }

    })
</script>
</asp:Content>
