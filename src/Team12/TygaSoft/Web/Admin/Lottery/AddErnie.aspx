<%@ Page Title="新建摇奖设定" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AddErnie.aspx.cs" Inherits="TygaSoft.Web.Admin.Lottery.AddErnie" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<script type="text/javascript" src="../../../Scripts/JeasyuiExtend.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">

<div id="tabErnie" class="easyui-tabs" data-options="fit:true">
    <div title="基本信息" style="padding:20px;">
		<div class="row">
          <span class="rl"><span class="cr">*</span> 开始时间：</span>
          <div class="fl">
              <input id="txtStartTime" runat="server" clientidmode="Static" class="easyui-datetimebox" data-options="required:true" />
          </div>
      </div>
      <div class="row mt10">
          <span class="rl"><span class="cr">*</span>结束时间：</span>
          <div class="fl">
              <input id="txtEndTime" runat="server" clientidmode="Static" class="easyui-datetimebox" data-options="required:true" />
          </div>
      </div>
      <div class="row mt10">
          <span class="rl"><span class="cr">*</span>最大摇奖次数：</span>
          <div class="fl">
              <input id="txtUserBetMaxCount" runat="server" clientidmode="Static" class="easyui-validatebox" data-options="required:true,validType:'int'" />
              <span>次/人</span>
          </div>
      </div>
       <div class="row mt10">
            <span class="rl">是否禁用：</span>
            <div class="fl ml10">
                <asp:RadioButtonList ID="rdIsDisable" runat="server" ClientIDMode="Static" RepeatDirection="Horizontal" RepeatLayout="Flow"></asp:RadioButtonList>
            </div>
            <span class="clr"></span>
        </div>
        <div class="row mt10">
            <span class="fl rl">&nbsp;</span>
            <div class="fl">
                <a id="abtnSaveErnie" runat="server" href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-save'" onclick="AddErnie.Save()">保存</a>
                <span id="lbMsg" runat="server" class="cr"></span>
            </div>
            <span class="clr"></span>
        </div>
    </div>
    <div title="参数设定" style="padding:20px;">
		<div id="dgErnieItemToolbar" style="padding:5px;">
            <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true" onclick="ListErnieItem.Add()">新 增</a>
            <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-edit',plain:true" onclick="ListErnieItem.Edit()">编 辑</a>
            <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-remove',plain:true" onclick="ListErnieItem.Del()">删 除</a>
            <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true" onclick="ListErnieItem.Copy()">复制最近一次数据</a>
        </div>
        <table id="dgErnieItem" class="easyui-datagrid" title="已添加项列表"
                data-options="rownumbers:true,pagination:true,fit:true,fitColumns:true,toolbar:'#dgErnieItemToolbar',url:'/h/ta.html',method:'get',
                queryParams:{reqName:'GetErnieItemForDatagrid', ernieId:$('#hErnieId').val()}">
            <thead>
                <tr>
                    <th data-options="field:'Id',checkbox:true"></th>
                    <th data-options="field:'ErnieId',hidden:true"></th>
                    <th data-options="field:'NumType',width:100">奖励类型</th>
                    <th data-options="field:'Num',width:100">数值</th>
                    <th data-options="field:'AppearRatio',width:100">现率</th>
                </tr>
            </thead>
        </table>

        <div id="dlgErnieItem" data-options="closed: false, modal: true, width: 580,height: 300,iconCls: 'icon-save'" style="padding:10px;"></div>
    </div>
</div>

<input type="hidden" id="hErnieId" runat="server" clientidmode="Static" />

<script type="text/javascript" src="../../Scripts/Admin/Lottery/AddErnie.js"></script>
<script type="text/javascript" src="../../Scripts/Admin/Lottery/ListErnieItem.js"></script>

</asp:Content>
