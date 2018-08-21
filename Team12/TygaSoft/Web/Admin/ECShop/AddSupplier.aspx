<%@ Page Title="新建供应商" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AddSupplier.aspx.cs" Inherits="TygaSoft.Web.Admin.ECShop.AddSupplier" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<link href="../../Styles/ProvinceCity.css" rel="stylesheet" type="text/css" />
<script type="text/javascript" src="../../Scripts/JeasyuiExtend.js"></script>
<script type="text/javascript" src="../../Scripts/DlgProvinceCity.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">

<div class="row mt10">
    <span class="rl"><b class="cr">*</b>供应商名称：</span>
    <div class="fl">
        <input type="text" id="txtName" runat="server" clientidmode="Static" class="easyui-textbox mtxt" data-options="required:true,missingMessage:'必填项'" />
    </div>
    <span class="clr"></span>
</div>
<div class="row mt10">
    <span class="rl">联系方式：</span>
    <div class="fl">
        <input type="text" id="txtPhone" runat="server" clientidmode="Static" class="easyui-textbox mtxt" data-options="validType:'phone'" />
    </div>
    <span class="clr"></span>
</div>
<div class="row mt10">
    <span class="rl">省市区：</span>
    <div class="fl">
        <a href="javascript:void(0)" id="lbtnSelect" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true" onclick="ProvinceCity.OnOpenDlg()">选 择</a>
        <span id="lbProvinceCity" runat="server" clientidmode="Static" class="ml10"></span>
        <span id="lbHProvinceCity" runat="server" clientidmode="Static" style="display:none;"></span>
    </div>
    <div class="clr"></div>
</div>
<div class="row mt10">
    <span class="rl">详细地址：</span>
    <div class="fl">
        <input type="text" id="txtAddress" runat="server" clientidmode="Static" class="easyui-textbox mtxt" data-options="prompt:'省市区以外的地址'" />
    </div>
    <div class="clr"></div>
</div>
    
<div class="row mt10">
    <span class="rl">&nbsp;</span>
    <div class="fl">
    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-save'" onclick="AddSupplier.OnSave()">提交</a>
    </div>
    <span class="clr"></span>
</div>

<input type="hidden" id="hId" runat="server" clientidmode="Static" />

<div id="dlgProvinceCity" class="easyui-dialog" title="省市区" data-options="closed:true,modal:true,
buttons: [{
        id: 'btnOk',text:'确定',iconCls:'icon-ok',handler:function(){
		    ProvinceCity.OnOk();
	    }
    },{
        id: 'btnCancel',text:'取消',iconCls:'icon-cancel',handler:function(){
		    $('#dlgProvinceCity').dialog('close');
	    }
    }]" style="width:700px;height:400px;">

    <div id="tabProvinceCity" class="easyui-tabs" data-options="border: false,fit: true,tabWidth: 150"></div>
</div>

<script type="text/javascript" src="../../Scripts/Admin/ECShop/AddSupplier.js"></script>

<script type="text/javascript">
    $(function () {
        try {
            AddSupplier.Init();
            ProvinceCity.Init();
        }
        catch (e) {
            $.messager.alert('错误提醒', e.name + ": " + e.message, 'error');
        }

    })

</script>

</asp:Content>
