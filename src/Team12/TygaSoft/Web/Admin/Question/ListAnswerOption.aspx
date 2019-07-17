<%@ Page Title="答案选项列表" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ListAnswerOption.aspx.cs" Inherits="TygaSoft.Web.Admin.Question.ListAnswerOption" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
<div id="toolbar" style="padding:5px;">
    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true" onclick="ListAnswerOption.Add()">新 增</a>
    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-edit',plain:true" onclick="ListAnswerOption.Edit()">编 辑</a>
    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-remove',plain:true" onclick="ListAnswerOption.Del()">删 除</a>
    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-undo',plain:true" onclick="ListAnswerOption.Sort(0)">升 序</a>
    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-redo',plain:true" onclick="ListAnswerOption.Sort(1)">降 序</a>
</div>

<table id="dgT" class="easyui-datagrid" title="答案选项列表" data-options="rownumbers:true,pagination:true,fit:true,fitColumns:true,toolbar:'#toolbar'">
    <thead>
        <tr>
            <th data-options="field:'f0',checkbox:true"></th>
            <th data-options="field:'f1'" hidden="true"></th>
            <th data-options="field:'f2'" hidden="true">序号</th>
            <th data-options="field:'f3',width:200">题目</th>
            <th data-options="field:'f4',width:200">选项内容</th>
            <th data-options="field:'f5',width:50">正确答案</th>
            <th data-options="field:'f6'" hidden="true"></th>
            <th data-options="field:'f7',width:120">最后更新时间</th>     
        </tr>
    </thead>
    <tbody>
    <asp:Repeater ID="rpData" runat="server">
        <ItemTemplate>
            <tr>
                <td><%#Eval("Id")%></td>
                <td><%#Eval("Sort")%></td>
                <td><%#Eval("RowNumber")%></td>
                <td><%#Eval("QuestionContent")%></td>
                <td><%#Eval("OptionContent")%></td>
                <td><%#Eval("IsTrueContent")%></td>
                <td><%#Eval("IsTrue")%></td>
                <td><%#((DateTime)Eval("LastUpdatedDate")).ToString("yyyy-MM-dd HH:mm")%></td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
    </tbody>
</table>

<asp:Literal runat="server" ID="ltrMyData"></asp:Literal>
<input type="hidden" id="qsId" runat="server" clientidmode="Static" />
<script type="text/javascript" src="../../Scripts/Admin/Question/ListAnswerOption.js"></script>

<script type="text/javascript">
    var sPageIndex = 0;
    var sPageSize = 0;
    var sTotalRecord = 0;
    var sQueryStr = "";

    $(function () {
        try {
            var myData = ListAnswerOption.GetMyData("myDataForPage");
            $.map(myData, function (item) {
                sPageIndex = parseInt(item.PageIndex);
                sPageSize = parseInt(item.PageSize);
                sTotalRecord = parseInt(item.TotalRecord);
                sQueryStr = item.QueryStr.replace(/&amp;/g, '&');
            })

            ListAnswerOption.Init();
        }
        catch (e) {
            $.messager.alert('错误提醒', e.name + ": " + e.message, 'error');
        }

    })
</script>
</asp:Content>
