<%@ Page Title="品牌管理" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ListBrand.aspx.cs" Inherits="TygaSoft.Web.Admin.ECShop.ListBrand" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">

    <div class="easyui-panel" title="品牌树" data-options="fit:true">
        <div class="mtb5">
           <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true" onclick="ListBrand.Add()">新建</a>
           <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-edit',plain:true" onclick="ListBrand.Edit()">编辑</a>
           <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-remove',plain:true" onclick="ListBrand.Del()">删除</a>
        </div>
       <ul id="treeCt" class="easyui-tree"></ul>
       <div id="mmTree" class="easyui-menu" style="width:120px;">
           <div onclick="ListBrand.Add()" data-options="iconCls:'icon-add'">添加</div>
           <div onclick="ListBrand.Edit()" data-options="iconCls:'icon-edit'">编辑</div>
           <div onclick="ListBrand.Del()" data-options="iconCls:'icon-remove'">删除</div>
       </div> 
    </div>

    <input type="hidden" id="hCurrExpandNode" value="" />

    <div id="dlgBrand" style="width:720px;height:390px;padding:10px;"></div>

    <script type="text/javascript" src="../../Scripts/Admin/DlgPictureSelect.js"></script>
    <script type="text/javascript" src="../../Scripts/Admin/ECShop/ListBrand.js"></script>
    <script type="text/javascript">
        $(function () {
            try {
                ListBrand.Init();
            }
            catch (e) {
                $.messager.alert('错误提醒', e.name + ": " + e.message, 'error');
            }
        
        })

    
    </script>


</asp:Content>
