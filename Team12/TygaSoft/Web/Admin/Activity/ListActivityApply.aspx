<%@ Page Title="报名列表" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ListActivityApply.aspx.cs" 
Inherits="TygaSoft.Web.Admin.Activity.ListActivityApply" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
<div id="toolbar" style="padding:5px;">
    <asp:Button ID="exp" runat="server" OnClick="exp_Click" Text="导 出"/>
</div>

<table id="dgT" class="easyui-datagrid" title="报名列表" data-options="rownumbers:true,pagination:true,fit:true,fitColumns:true,toolbar:'#toolbar'">
    <thead>
        <tr>
            <th data-options="field:'f0',width:80">活动标题</th>
            <th data-options="field:'f1',width:40">报名姓名</th>
            <th data-options="field:'f2',width:20">报名昵称</th>
            <th data-options="field:'f3',width:40">性别</th>
            <th data-options="field:'f4',width:80">联系方式</th>      
            <th data-options="field:'f5',width:80">最后更新时间</th>
        </tr>
    </thead>
    <tbody>
    <asp:Repeater ID="rpData" runat="server">
        <ItemTemplate>
            <tr>
                <td><%#Eval("Title")%></td>
                <td><%#Eval("UserName")%></td>
                <td><%#Eval("Nickname")%></td>
                <td><%#Eval("Sex")%></td>
                <td><%#Eval("MobilePhone") %></td>
                <td><%#((DateTime)Eval("LastUpdatedDate")).ToString("yyyy-MM-dd HH:mm")%></td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
    </tbody>
</table>

<asp:Literal runat="server" ID="ltrMyData"></asp:Literal>
<input type="hidden" id="asId" runat="server" clientidmode="Static" />
<script type="text/javascript" src="../../Scripts/Admin/Activity/ListActivityApply.js"></script>

<script type="text/javascript">
    var sPageIndex = 0;
    var sPageSize = 0;
    var sTotalRecord = 0;
    var sQueryStr = "";

    $(function () {
        try {
            var myData = ListActivityApply.GetMyData("myDataForPage");
            $.map(myData, function (item) {
                sPageIndex = parseInt(item.PageIndex);
                sPageSize = parseInt(item.PageSize);
                sTotalRecord = parseInt(item.TotalRecord);
                sQueryStr = item.QueryStr.replace(/&amp;/g, '&');
            })

            ListActivityApply.Init();
        }
        catch (e) {
            $.messager.alert('错误提醒', e.name + ": " + e.message, 'error');
        }

    })
</script>
</asp:Content>