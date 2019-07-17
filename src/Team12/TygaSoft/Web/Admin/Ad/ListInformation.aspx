<%@ Page Title="资讯列表" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ListInformation.aspx.cs" Inherits="TygaSoft.Web.Admin.Ad.ListInformation" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/Styles/Team.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">

<div id="toolbar" style="padding:5px;">
    标题：<input type="text" runat="server" id="txtTitle" maxlength="50" class="txt" />
    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-search'" onclick="ListInformation.Search()">查 詢</a>
    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true" onclick="ListInformation.Add()">新 增</a>
    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-edit',plain:true" onclick="ListInformation.Edit()">编 辑</a>
    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-remove',plain:true" onclick="ListInformation.Del()">删 除</a>
</div>


<table id="dgT" class="easyui-datagrid" title="资讯列表" data-options="rownumbers:true,pagination:true,fit:true,fitColumns:true,toolbar:'#toolbar'">
    <thead>
        <tr>
            <th data-options="field:'f0',checkbox:true"></th>
            <th data-options="field:'f1',width:100">标题</th>
            <th data-options="field:'f2',width:100">描述说明</th>
            <th data-options="field:'f3',width:50">排序</th>
            <th data-options="field:'f4',width:50">访问量</th>
            <th data-options="field:'f5',width:50">显示类型</th>
            <th data-options="field:'f6',width:50">是否禁用</th>
            <th data-options="field:'f7',width:80">最后更新时间</th>
            <th data-options="field:'f8',width:80">操作</th>
        </tr>
    </thead>
    <tbody>
    <asp:Repeater ID="rpData" runat="server">
        <ItemTemplate>
            <tr>
                <td><%#Eval("Id")%></td>
                <td><%#Eval("Title")%></td>
                <td><%#Eval("Summary")%></td>
                <td><%#Eval("Sort")%></td>
                <td><%#Eval("ViewCount")%></td>
                <td><%#Eval("ViewType").ToString() == "0" ? "图文" : (Eval("ViewType").ToString() == "1" ? "横幅" : "图片")%></td>
                <td><%#Eval("IsDisable").ToString() == "True" ? "是" : "否"%></td>
                <td><%#((DateTime)Eval("LastUpdatedDate")).ToString("yyyy-MM-dd HH:mm")%></td>
                <td><a href="javascript:void(0);" onclick="ListInformation.OpenInforAdWin('<%#Eval("Id")%>');$(this).parent().click();return false;" style="color:Blue">关联广告</a></td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
    </tbody>
</table>

<%--关联广告--%>
    <div id="RelInforAd" class="easyui-dialog" title="关联广告" style="width: 550px;
        height: 450px; overflow: auto;" modal="true" closed="true" buttons="#RelClass-buttons">
        <input type="hidden"  id="InformationId" />
        <div style=" float: left; margin:10px 0 0 10px">
            <div style="width: 220px; height: 355px; padding: 1px;" title="广告列表" class="easyui-panel">
                <select id="selectList" multiple="multiple" style="width: 100%; height: 100%;">
                </select>
            </div>
        </div>
        <div style=" float:left;width: 50px; height: 355px;margin:10px 0 0 0">
        <a id="btnAddClass" href="javascript:void(0);" class="easyui-linkbutton" style="margin:150px 0 0 18px" onclick="ListInformation.AddRelInforAd()">>>></a>
        <a id="btnDelClass" href="javascript:void(0);" class="easyui-linkbutton" style="margin:50px 0 0 18px" onclick="ListInformation.DelRelInfoAd()"><<<</a>
        </div>
        <div style="float: right; margin: 10px 10px 0 0">
            <div style="width: 220px; height: 355px; padding: 1px;" title="已选列表" class="easyui-panel" data-options="tools:'#pntool'">
                <select id="selectedList" multiple="multiple" style="width: 100%; height: 100%;">
                </select>
            </div>
        </div>
    </div>
    <div id="pntool">
	<a href="#" onclick="ListInformation.MoveUp()" class="arrow_up" title="向上移动"></a>
	<a href="#" onclick="ListInformation.MoveDown()" class="arrow_down" title="向下移动"></a>
</div>
     <div id="RelClass-buttons">
        <a id="btn_Submit" href="javascript:void(0);" class="easyui-linkbutton" onclick="ListInformation.SubmitRelInforAd()">提交</a> <a href="javascript:void(0);"
            class="easyui-linkbutton" onclick="$('#RelInforAd').dialog('close');return false;">
            取消</a>
    </div>
    <%--关联广告 end--%>

<asp:Literal runat="server" ID="ltrMyData"></asp:Literal>
<script type="text/javascript" src="../../Scripts/Admin/Ad/ListInformation.js"></script>

<script type="text/javascript">
    var sPageIndex = 0;
    var sPageSize = 0;
    var sTotalRecord = 0;
    var sQueryStr = "";

    $(function () {
        try {
            var myData = ListInformation.GetMyData("myDataForPage");
            $.map(myData, function (item) {
                sPageIndex = parseInt(item.PageIndex);
                sPageSize = parseInt(item.PageSize);
                sTotalRecord = parseInt(item.TotalRecord);
                sQueryStr = item.QueryStr.replace(/&amp;/g, '&');
            })

            ListInformation.Init();
        }
        catch (e) {
            $.messager.alert('错误提醒', e.name + ": " + e.message, 'error');
        }

    });
</script>

</asp:Content>
