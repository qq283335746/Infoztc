<%@ Page Title="选手列表" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ListActivityPlayer.aspx.cs" 
Inherits="TygaSoft.Web.Admin.Activity.ListActivityPlayer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
<div id="toolbar" style="padding:5px;">
    姓名：<input type="text" runat="server" id="txtName" maxlength="50" class="txt" />
    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-search'" onclick="ListActivityPlayer.Search()">查 詢</a>
    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true" onclick="ListActivityPlayer.Add()">新 增</a>
    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-edit',plain:true" onclick="ListActivityPlayer.Edit()">编 辑</a>
    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-remove',plain:true" onclick="ListActivityPlayer.Del()">删 除</a>
</div>

<table id="dgT" class="easyui-datagrid" title="选手列表" data-options="rownumbers:true,pagination:true,fit:true,fitColumns:true,toolbar:'#toolbar'">
    <thead>
        <tr>
            <th data-options="field:'f0',checkbox:true"></th>
            <th data-options="field:'f1',width:100">活动标题</th>
            <th data-options="field:'f2',width:100">选手姓名</th>
            <th data-options="field:'f3',width:100">头像</th>      
            <th data-options="field:'f4',width:80">实际报名数</th>
            <th data-options="field:'f5',width:80">修改报名数</th>
            <th data-options="field:'f6',width:80">最后更新时间</th>
            <th data-options="field:'f7',width:50">投票统计</th>
        </tr>
    </thead>
    <tbody>
    <asp:Repeater ID="rpData" runat="server">
        <ItemTemplate>
            <tr>
                <td><%#Eval("Id")%></td>
                <td><%#Eval("Title")%></td>
                <td><%#Eval("Named")%></td>
                <td><img src='<%#string.Format("{0}{1}/PC/{1}_1{2}",Eval("FileDirectory"),Eval("RandomFolder"),Eval("FileExtension")) %>' alt="" width="80px" height="50px" /> </td>
                <td><%#Eval("ActualVoteCount")%></td>
                <td><%#Eval("UpdateVoteCount")%></td>
                <td><%#((DateTime)Eval("LastUpdatedDate")).ToString("yyyy-MM-dd HH:mm")%></td>
                <td><a href="ttactivity.html?apId=<%#Eval("Id")%>" style="color:Blue">详情</a></td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
    </tbody>
</table>

<asp:Literal runat="server" ID="ltrMyData"></asp:Literal>
<input type="hidden" id="asId" runat="server" clientidmode="Static" />
<script type="text/javascript" src="../../Scripts/Admin/Activity/ListActivityPlayer.js"></script>

<script type="text/javascript">
    var sPageIndex = 0;
    var sPageSize = 0;
    var sTotalRecord = 0;
    var sQueryStr = "";

    $(function () {
        try {
            var myData = ListActivityPlayer.GetMyData("myDataForPage");
            $.map(myData, function (item) {
                sPageIndex = parseInt(item.PageIndex);
                sPageSize = parseInt(item.PageSize);
                sTotalRecord = parseInt(item.TotalRecord);
                sQueryStr = item.QueryStr.replace(/&amp;/g, '&');
            })

            ListActivityPlayer.Init();
        }
        catch (e) {
            $.messager.alert('错误提醒', e.name + ": " + e.message, 'error');
        }

    })
</script>
</asp:Content>