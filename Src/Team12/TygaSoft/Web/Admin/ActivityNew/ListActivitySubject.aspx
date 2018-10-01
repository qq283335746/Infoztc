<%@ Page Title="活动列表" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ListActivitySubject.aspx.cs" Inherits="TygaSoft.Web.Admin.ActivityNew.ListActivitySubject" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
<div id="toolbar" style="padding:5px;">
    标题：<input type="text" runat="server" id="txtTitle" maxlength="50" class="txt" />
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
            <th data-options="field:'f2',width:80">有效期开始时间</th>      
            <th data-options="field:'f3',width:80">有效期结束时间</th>
            <th data-options="field:'f4',width:50">排序</th>
            <th data-options="field:'f5',width:60">最大投票数</th>
            <th data-options="field:'f6',width:60">最大报名数</th>
            <th data-options="field:'f7',width:50">实际报名数</th>
            <th data-options="field:'f8',width:50">虚拟报名数</th>
            <th data-options="field:'f9',width:50">访问量</th>
            <th data-options="field:'f10',width:80">最后更新时间</th>
            <th data-options="field:'f11',width:50">选手列表</th>
            <th data-options="field:'f12',width:70">是否刮刮奖</th>
            <th data-options="field:'f13',width:50">中奖列表</th>
            <th data-options="field:'f14',width:40">推送</th>
        </tr>
    </thead>
    <tbody>
    <asp:Repeater ID="rpData" runat="server">
        <ItemTemplate>
            <tr>
                <td><%#Eval("Id")%></td>
                <td><%#Eval("Title")%></td>
                <td><%#((DateTime)Eval("StartDate")).ToString("yyyy-MM-dd")%></td>
                <td><%#((DateTime)Eval("EndDate")).ToString("yyyy-MM-dd")%></td>
                <td><%#Eval("Sort")%></td>
                <td><%#Eval("MaxVoteCount")%></td>
                <td><%#Eval("MaxSignUpCount")%></td>
                <td><%#Eval("SignUpCount")%></td>
                <td><%#Eval("VirtualSignUpCount")%></td>
                <td><%#Eval("ViewCount")%></td>
                <td><%#((DateTime)Eval("LastUpdatedDate")).ToString("yyyy-MM-dd HH:mm")%></td>
                <td><a href="gactivity.html?asId=<%#Eval("Id")%>" style="color:Blue">详情</a></td>
                <td><a href="javascript:void(0)" onclick="ListActivitySubject.BindPrizeHref('<%#Eval("Id")%>','<%#Eval("IsPrize")%>')" style="color:Blue"><%#Eval("IsPrize").ToString() == "True" ? "奖项列表" : "否"%></a></td>
                <td><a href="yaactivity.html?asId=<%#Eval("Id")%>" style="color:Blue"><%#Eval("IsPrize").ToString() == "True" ? "中奖记录" : ""%></a></td>
                <td><a href="gtactivity.html?Id=<%#Eval("Id")%>" style="color:Blue">编辑</a></td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
    </tbody>
</table>

<asp:Literal runat="server" ID="ltrMyData"></asp:Literal>
<script type="text/javascript" src="../../Scripts/Admin/ActivityNew/ListActivitySubject.js"></script>

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
</script>
</asp:Content>
