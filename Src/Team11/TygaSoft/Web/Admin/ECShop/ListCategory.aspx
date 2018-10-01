<%@ Page Title="分类管理" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ListCategory.aspx.cs" Inherits="TygaSoft.Web.Admin.Category.ListCategory" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">

<div class="easyui-panel" title="分类树" data-options="fit:true">
    <div class="mtb5">
       <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true" onclick="ListCategory.Add()">新建</a>
       <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-edit',plain:true" onclick="ListCategory.Edit()">编辑</a>
       <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-remove',plain:true" onclick="ListCategory.Del()">删除</a>
    </div>
   <ul id="treeCt" class="easyui-tree"></ul>
   <div id="mmTree" class="easyui-menu" style="width:120px;">
       <div onclick="ListCategory.Add()" data-options="iconCls:'icon-add'">添加</div>
       <div onclick="ListCategory.Edit()" data-options="iconCls:'icon-edit'">编辑</div>
       <div onclick="ListCategory.Del()" data-options="iconCls:'icon-remove'">删除</div>
   </div> 
</div>

 <input type="hidden" id="hCurrExpandNode" value="" />

 <div id="dlgCategory" style="width:720px;height:390px;padding:10px;"></div>

<script type="text/javascript" src="../../Scripts/Admin/DlgPictureSelect.js"></script>
<script type="text/javascript" src="../../Scripts/Admin/ECShop/ListCategory.js"></script>
<script type="text/javascript">
    $(function () {
        try {
            ListCategory.Init();
        }
        catch (e) {
            $.messager.alert('错误提醒', e.name + ": " + e.message, 'error');
        }
    })

    
</script>

</asp:Content>
