<%@ Page Title="活动列表" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ListActivitySubject.aspx.cs" Inherits="TygaSoft.Web.Admin.Activity.ListActivitySubject" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
<div id="toolbar" style="padding:5px;">
    标题：<input type="text" runat="server" id="txtTitle" maxlength="50" class="txt" />
    内容：<input type="text" runat="server" id="txtContent" maxlength="50" class="txt" />
    类型：<select class="easyui-combobox" id="selectType" runat="server" clientidmode="Static" style="width:100px"></select>
    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-search'" onclick="ListActivitySubject.Search()">查 詢</a>
    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true" onclick="ListActivitySubject.Add()">新 增</a>
    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-edit',plain:true" onclick="ListActivitySubject.Edit()">编 辑</a>
    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-remove',plain:true" onclick="ListActivitySubject.Del()">删 除</a>
</div>

<table id="dgT" class="easyui-datagrid" title="活动列表" data-options="rownumbers:true,pagination:true,fit:true,fitColumns:true,toolbar:'#toolbar'">
    <thead>
        <tr>
            <th data-options="field:'f0',checkbox:true"></th>
            <th data-options="field:'f1',width:100">标题</th>
            <th data-options="field:'f2',width:150">内容</th>
            <th data-options="field:'f3',width:80">有效期开始时间</th>      
            <th data-options="field:'f4',width:80">有效期结束时间</th>
            <th data-options="field:'f5',width:50">类型</th>
            <th data-options="field:'f6',width:50">排序</th>
            <th data-options="field:'f7',width:60">最大投票数</th>
            <th data-options="field:'f8',width:50">投票倍数</th>
            <th data-options="field:'f9',width:60">最大报名数</th>
            <th data-options="field:'f10',width:50">实际报名数</th>
            <th data-options="field:'f11',width:50">修改报名数</th>
            <th data-options="field:'f12',width:80">最后更新时间</th>
            <th data-options="field:'f13',width:50">详情</th>
        </tr>
    </thead>
    <tbody>
    <asp:Repeater ID="rpData" runat="server">
        <ItemTemplate>
            <tr>
                <td><%#Eval("Id")%></td>
                <td><%#Eval("Title")%></td>
                <td><%#Eval("ContentText").ToString().Length > 50 ? Eval("ContentText").ToString().Substring(0, 50) + "......" : Eval("ContentText")%></td>
                <td><%#((DateTime)Eval("StartDate")).ToString("yyyy-MM-dd HH:mm")%></td>
                <td><%#((DateTime)Eval("EndDate")).ToString("yyyy-MM-dd HH:mm")%></td>
                <td><%#Eval("ActivityType").ToString() == "0" ? "投票" : "报名"%></td>
                <td><%#Eval("Sort")%></td>
                <td><%#Eval("ActivityType").ToString() == "0" ? Eval("MaxVoteCount"): ""%></td>
                <td><%#Eval("ActivityType").ToString() == "0" ? Eval("VoteMultiple"): ""%></td>
                <td><%#Eval("ActivityType").ToString() == "0" ? "" : Eval("MaxSignUpCount")%></td>
                <td><%#Eval("ActivityType").ToString() == "0" ? "" : Eval("ActualSignUpCount")%></td>
                <td><%#Eval("ActivityType").ToString() == "0" ? "" : Eval("UpdateSignUpCount")%></td>
                <td><%#((DateTime)Eval("LastUpdatedDate")).ToString("yyyy-MM-dd HH:mm")%></td>
                <td><a href="javascript:void(0)" onclick="BindHref('<%#Eval("Id")%>','<%#Eval("ActivityType")%>')" style="color:Blue"><%#Eval("ActivityType").ToString() == "0" ? "选手列表" : "报名统计"%></a></td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
    </tbody>
</table>

<asp:Literal runat="server" ID="ltrMyData"></asp:Literal>
<input type="hidden" id="hType" runat="server" clientidmode="Static" />
<script type="text/javascript" src="../../Scripts/Admin/Activity/ListActivitySubject.js"></script>

<script type="text/javascript">
    var sPageIndex = 0;
    var sPageSize = 0;
    var sTotalRecord = 0;
    var sQueryStr = "";

    $(function () {
        try {
            var myData = ListActivitySubject.GetMyData("myDataForPage");
            $.map(myData, function (item) {
                sPageIndex = parseInt(item.PageIndex);
                sPageSize = parseInt(item.PageSize);
                sTotalRecord = parseInt(item.TotalRecord);
                sQueryStr = item.QueryStr.replace(/&amp;/g, '&');
            })

            ListActivitySubject.Init();
        }
        catch (e) {
            $.messager.alert('错误提醒', e.name + ": " + e.message, 'error');
        }

    })

    function BindHref(id, type) {
        if (type == "0") {
            window.location = "gactivity.html?asId=" + id;
        }
        else {
            window.location = "tyactivity.html?asId=" + id;
        }
    }
</script>
</asp:Content>
