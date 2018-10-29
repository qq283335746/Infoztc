<%@ Page Title="话题列表" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ListTopicSubject.aspx.cs" Inherits="TygaSoft.Web.Admin.Topic.ListTopicSubject" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
<div id="toolbar" style="padding:5px;">
    标题：<input type="text" runat="server" id="txtTitle" maxlength="50" class="txt" />
    内容：<input type="text" runat="server" id="txtContent" maxlength="50" class="txt" />
    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-search'" onclick="ListTopicSubject.Search()">查 詢</a>
    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true" onclick="ListTopicSubject.Add()">新 增</a>
    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-edit',plain:true" onclick="ListTopicSubject.Edit()">编 辑</a>
    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-remove',plain:true" onclick="ListTopicSubject.Del()">删 除</a>
</div>

<table id="dgT" class="easyui-datagrid" title="题库列表" data-options="rownumbers:true,pagination:true,fit:true,fitColumns:true,toolbar:'#toolbar'">
    <thead>
        <tr>
            <th data-options="field:'f0',checkbox:true"></th>
            <th data-options="field:'f1',width:150">标题</th>
            <th data-options="field:'f2',width:250">内容</th>
            <th data-options="field:'f3',width:80">发起者</th>      
            <th data-options="field:'f4',width:80">最后更新时间</th>
            <th data-options="field:'f5',width:50">操作</th>
            <th data-options="field:'f6',width:30">评论</th>
            <th data-options="field:'f7'" hidden="true"></th>
        </tr>
    </thead>
    <tbody>
    <asp:Repeater ID="rpData" runat="server">
        <ItemTemplate>
            <tr>
                <td><%#Eval("Id")%></td>
                <td><%#Eval("Title")%></td>
                <td><%#Eval("ContentText").ToString().Length > 50 ? Eval("ContentText").ToString().Substring(0, 50) + "......" : Eval("ContentText")%></td>
                <td><%#Eval("UserName")%></td>
                <td><%#((DateTime)Eval("LastUpdatedDate")).ToString("yyyy-MM-dd HH:mm")%></td>
                <td><a href="javascript:void(0)" style="color:Blue" onclick="ListTopicSubject.UpdateIsTop('<%#Eval("Id")%>','<%#Eval("IsTop").Equals(false) ? "true" : "false"%>')"><%#Eval("IsTop").Equals(false) ? "置顶" : "取消置顶"%></a></td>
                <td><a href="gtopic.html?tsId=<%#Eval("Id")%>" style="color:Blue">详情</a></td>
                <td><%#Eval("IsTop")%></td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
    </tbody>
</table>

<asp:Literal runat="server" ID="ltrMyData"></asp:Literal>
<script type="text/javascript" src="../../Scripts/Admin/Topic/ListTopicSubject.js"></script>

<script type="text/javascript">
    var sPageIndex = 0;
    var sPageSize = 0;
    var sTotalRecord = 0;
    var sQueryStr = "";

    $(function () {
        try {
            var myData = ListTopicSubject.GetMyData("myDataForPage");
            $.map(myData, function (item) {
                sPageIndex = parseInt(item.PageIndex);
                sPageSize = parseInt(item.PageSize);
                sTotalRecord = parseInt(item.TotalRecord);
                sQueryStr = item.QueryStr.replace(/&amp;/g, '&');
            })

            ListTopicSubject.Init();
        }
        catch (e) {
            $.messager.alert('错误提醒', e.name + ": " + e.message, 'error');
        }

    })
</script>
</asp:Content>
