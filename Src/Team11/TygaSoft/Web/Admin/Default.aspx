<%@ Page Title="后台管理首页" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TygaSoft.Web.Admin.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">

我的桌面

<div style="display:none;">
<table style="width:270px;">
    <tr><td rowspan="4" style="width:120px;"></td><td>删除</td><td>禁用</td></tr>
    <tr><td></td><td></td></tr>
    <tr><td></td><td></td></tr>
    <tr><td></td><td></td></tr>
    <tr><td></td><td></td><td></td></tr>
</table>
</div>

<script type="text/javascript">
    $(function () {
        //$.messager.progress();
//        $.messager.progress({
//            title: '请稍等',
//            msg: '正在执行...'
//        });
//        setTimeout(function () {
//            $.messager.progress('close');
//        },3000)
        //$.messager.progress('close');
    })
</script>

</asp:Content>
