<%@ Page Title="广告基本信息" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AddAdBase.aspx.cs" Inherits="TygaSoft.Web.Admin.AboutSite.AddAdBase" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">

<div id="advertisementTab" class="easyui-tabs" data-options="fit:true,onSelect:AddAdvertisement.OnTabSelect">

	<div title="基本信息" style="padding:10px">
        <div class="row mt10">
            <span class="rl"><b class="cr">*</b>标题：</span>
            <div class="fl" style="width:800px;">
                <input type="text" id="txtTitle" runat="server" clientidmode="Static" class="easyui-validatebox ltxt" data-options="required:true,missingMessage:'必填项'" />
            </div>
            <span class="clr"></span>
        </div>
        <div class="row mt10">
            <span class="rl"><b class="cr">*</b>广告区：</span>
            <div class="fl">
                <input id="cbbSiteFun" class="easyui-combobox" data-options="required:true,missingMessage:'必选项',valueField:'id',textField:'text',url: '/h/tt.html?reqName=GetJsonForCombobox&enumCode=AdvertisementFun',method:'GET',onLoadSuccess:AddAdBase.OnCbbSiteFunLoadSuccess" />
            </div>
            <div class="clr"></div>
        </div>
        <div class="row mt10">
            <span class="rl"><b class="cr">*</b>布局位置：</span>
            <div class="fl">
                <input id="cbbLayoutPosition" class="easyui-combobox" data-options="required:true,missingMessage:'必选项',valueField:'id',textField:'text',url: '/h/tt.html?reqName=GetJsonForCombobox&enumCode=LayoutPosition',method:'GET',onLoadSuccess:AddAdBase.OnCbbLayoutPositionSuccess" />
            </div>
            <div class="clr"></div>
        </div>
        <div class="row mt10">
            <span class="rl">上下架：</span>
            <div class="fl">
                <select id="cbbIsDisable" class="easyui-combobox" data-options="onLoadSuccess:AddAdBase.OnCbbIsDisableSuccess" style="width:173px;">
                    <option value="False">上架</option>
                    <option value="True">下架</option>
                </select>
            </div>
            <span class="clr"></span>
        </div>
        <div class="row mt10">
            <span class="rl">间隔时间：</span>
            <div class="fl">
                <input type="text" id="txtTimeout" runat="server" clientidmode="Static" class="easyui-validatebox" data-options="validType:'float'" />
                （单位：秒）
            </div>
            <span class="clr"></span>
        </div>
        <div class="row mt10">
            <span class="rl">排序：</span>
            <div class="fl">
                <input type="text" id="txtSort" runat="server" clientidmode="Static" class="easyui-validatebox" data-options="validType:'int'" />
            </div>
            <span class="clr"></span>
        </div>
        <div class="row mt10">
            <span class="rl">开始时间：</span>
            <div class="fl">
                <input id="txtStartTime" runat="server" clientidmode="Static" class="easyui-datetimebox" />
            </div>
            <span class="clr"></span>
        </div>
        <div class="row mt10">
            <span class="rl">结束时间：</span>
            <div class="fl">
                <input type="text" id="txtEndTime" runat="server" clientidmode="Static" class="easyui-datetimebox" />
            </div>
            <span class="clr"></span>
        </div>
        <div class="row mt10">
            <span class="rl">访问量设置：</span>
            <div class="fl">
                <input type="text" id="txtVirtualViewCount" runat="server" clientidmode="Static" class="easyui-validatebox" data-options="validType:'int'" />
            </div>
            <span class="clr"></span>
        </div>

        <div class="row mt10">
            <span class="rl">&nbsp;</span>
            <div class="fl">
                <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-save'" onclick="AddAdBase.OnSave()">保 存</a>
            </div>
            <span class="clr"></span>
        </div>

	</div>
	<div title="其它信息" style="padding:10px"></div>
</div>

<span id="lbTabSelectIndex" style="display:none;">0</span>
<input type="hidden" id="hAdId" runat="server" clientidmode="Static" />

<asp:Literal runat="server" ID="ltrMyData"></asp:Literal>

<script type="text/javascript" src="../../Scripts/Admin/AboutSite/AddAdvertisement.js"></script>
<script type="text/javascript" src="../../Scripts/Admin/AboutSite/AddAdBase.js"></script>

<script type="text/javascript">
    $(function () {

    })
</script>

</asp:Content>
