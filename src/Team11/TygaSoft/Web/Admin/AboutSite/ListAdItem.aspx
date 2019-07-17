<%@ Page Title="广告项列表" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ListAdItem.aspx.cs" Inherits="TygaSoft.Web.Admin.AboutSite.ListAdItem" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<script type="text/javascript" src="../../Scripts/JeasyuiExtend.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">

    <div id="advertisementTab" class="easyui-tabs" data-options="fit:true,onSelect:AddAdvertisement.OnTabSelect">

	    <div title="基本信息" style="padding:10px"></div>
	    <div title="其它信息" data-options="selected:true" style="padding:10px">
            <div id="toolbar" style="padding:5px;">
                关键字：<input type="text" runat="server" id="txtKeyword" maxlength="50" class="txt" />
                <a class="easyui-linkbutton" data-options="iconCls:'icon-search'" onclick="ListAdItem.Search()">查 詢</a>
                <a class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true" onclick="ListAdItem.Add()">新 增</a>
                <a class="easyui-linkbutton" data-options="iconCls:'icon-edit',plain:true" onclick="ListAdItem.Edit()">编 辑</a>
                <a class="easyui-linkbutton" data-options="iconCls:'icon-remove',plain:true" onclick="ListAdItem.Del()">删 除</a>
            </div>

            <table id="dgT" class="easyui-datagrid" title="广告其它信息列表" data-options="rownumbers:true,pagination:true,fit:true,fitColumns:true,toolbar:'#toolbar'">
                <thead>
                    <tr>
                        <th data-options="field:'f0',checkbox:true"></th>
                        <th data-options="field:'f1',width:60">图片</th>
                        <th data-options="field:'f2',width:100">广告作用类型</th>
                        <th data-options="field:'f3',width:100">广告作用类型代码</th>
                        <th data-options="field:'f4',width:100">广告作用类型名称</th>
                        <th data-options="field:'f5',width:60">排序</th>
                        <th data-options="field:'f6',width:80">是否显示</th>
                        <th data-options="field:'f7',width:100">链接</th>
                        <th data-options="field:'f8',width:100">图文</th>
                    </tr>
                </thead>
                <tbody>
                <asp:Repeater ID="rpData" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td><%#Eval("Id")%></td>
                            <td><img src='<%#string.Format("{0}{1}/PC/{1}_2{2}",Eval("FileDirectory"),Eval("RandomFolder"),Eval("FileExtension")) %>' alt="" width="50" height="50" /> </td>
                            <td><%#Eval("ActionTypeName")%></td>
                            <td><%#Eval("TypeCode")%></td>
                            <td><%#Eval("TypeName")%></td>
                            <td><%#Eval("Sort")%></td>
                            <td><%# bool.Parse(Eval("IsDisable").ToString()) ? "不显示" : "显示"%></td>
                            <td>
                                <a href="javascript:void(0)" code='<%#Eval("Id")%>' onclick='ListAdItem.ShowAdItemLink(this)'>查看</a>
                            </td>
                            <td>
                                <a href="javascript:void(0)" code='<%#Eval("Id")%>' onclick='ListAdItem.ShowAdItemContent(this)'>查看</a>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
                </tbody>
            </table>
        </div>
    </div>

    <span id="lbTabSelectIndex" style="display:none;">1</span>
    <input type="hidden" id="hAdId" runat="server" clientidmode="Static" />

    <asp:Literal runat="server" ID="ltrMyData"></asp:Literal>

    <div id="dlgAddAdItem" style="padding:10px;"></div>
    <div id="dlgAdItemDetail" style="padding:10px;"></div>

    <script type="text/javascript" src="../../Scripts/Admin/AboutSite/AddAdvertisement.js"></script>
    <script type="text/javascript" src="../../Scripts/Admin/AboutSite/ListAdItem.js"></script>

    <script type="text/javascript">
        var sPageIndex = 0;
        var sPageSize = 0;
        var sTotalRecord = 0;
        var sQueryStr = "";

        $(function () {
            try {
                var myData = ListAdItem.GetMyData("myDataForPage");
                $.map(myData, function (item) {
                    sPageIndex = parseInt(item.PageIndex);
                    sPageSize = parseInt(item.PageSize);
                    sTotalRecord = parseInt(item.TotalRecord);
                    sQueryStr = item.QueryStr.replace(/&amp;/g, '&');
                })

                ListAdItem.Init();
            }
            catch (e) {
                $.messager.alert('错误提醒', e.name + ": " + e.message, 'error');
            }

        })
    </script>

</asp:Content>
