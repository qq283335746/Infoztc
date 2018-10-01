<%@ Page Title="服务管理" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ListService.aspx.cs" Inherits="TygaSoft.Web.Admin.Serve.ListService" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

  <link href="../../Scripts/plugins/kindeditor/themes/default/default.css" rel="stylesheet" type="text/css" />
  <script type="text/javascript" src="../../Scripts/JeasyuiExtend.js"></script>
  <script type="text/javascript" src="../../Scripts/plugins/kindeditor/kindeditor.js"></script>
  <script type="text/javascript" src="../../Scripts/plugins/kindeditor/lang/zh_CN.js"></script>
  <script type="text/javascript" src="../../Scripts/plugins/kindeditor/plugins/code/prettify.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">

<div class="easyui-layout" data-options="fit:true">
    <div data-options="region:'west',title:'服务分类',split:true" style="width:500px;">
        <div class="mtb5">
           <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true" onclick="ListService.Add()">新建</a>
           <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-edit',plain:true" onclick="ListService.Edit()">编辑</a>
           <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-remove',plain:true" onclick="ListService.Del()">删除</a>
        </div>
       <ul id="treeCt" class="easyui-tree" style="padding-left: 5px; margin-top:10px;"></ul>
       <div id="mmTree" class="easyui-menu" style="width:120px;">
           <div onclick="ListService.Add()" data-options="iconCls:'icon-add'">添加</div>
           <div onclick="ListService.Edit()" data-options="iconCls:'icon-edit'">编辑</div>
           <div onclick="ListService.Del()" data-options="iconCls:'icon-remove'">删除</div>
       </div> 
       <input type="hidden" id="hCurrExpandNode" value="" />
    </div>
    <div data-options="region:'center',title:'投票/链接/图文'" style="padding:5px;">
        <div id="tabServiceDetail" class="easyui-tabs" data-options="fit:true">
            <div title="投票" style="padding:10px;">
		        <table id="dgVote" class="easyui-datagrid" title="" data-options="data:[],rownumbers:true,pagination:true,fit:true,fitColumns:true,toolbar:'#toolbarVote'">
                    <thead>
                        <tr>
                            <th data-options="field:'Id',checkbox:true"></th>
                            <th data-options="field:'SPicture',width:60,formatter:ListService.FPicture">头像</th>
                            <th data-options="field:'Named',width:120">姓名</th>
                            <th data-options="field:'Descr',width:200">简介</th>
                            <th data-options="field:'Sort',width:200">排序</th>
                            <th data-options="field:'ServiceItemName',width:100">服务分类</th>
                        </tr>
                    </thead>
                </table>
                <div id="toolbarVote" style="padding:5px;">
                    关键字：<input type="text" id="txtKeywordForVote" name="keyword" maxlength="50" class="txt" />
                    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-search'" onclick="ListServiceVote.Search()">查 詢</a>
                    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true" onclick="ListServiceVote.Add()">新增</a>
                    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-edit',plain:true" onclick="ListServiceVote.Edit()">编辑</a>
                    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-remove',plain:true" onclick="ListServiceVote.Del()">删除</a>
                </div>
            </div>
            <div title="链接" style="padding:10px;">
		        <table id="dgLink" class="easyui-datagrid" title="" data-options="data:[],rownumbers:true,pagination:true,fit:true,fitColumns:true,toolbar:'#toolbarLink'">
                    <thead>
                        <tr>
                            <th data-options="field:'Id',checkbox:true"></th>
                            <th data-options="field:'SPicture',width:60,formatter:ListService.FPicture">图片</th>
                            <th data-options="field:'Named',width:120">名称</th>
                            <th data-options="field:'Url',width:200">Url</th>
                            <th data-options="field:'Sort',width:70">排序</th>
                            <th data-options="field:'ServiceItemName',width:100">服务分类</th>
                        </tr>
                    </thead>
                </table>
                <div id="toolbarLink" style="padding:5px;">
                    关键字：<input type="text" id="txtKeywordForLink" maxlength="50" class="txt" />
                    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-search'" onclick="ListServiceLink.Search()">查 詢</a>
                    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true" onclick="ListServiceLink.Add()">新增</a>
                    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-edit',plain:true" onclick="ListServiceLink.Edit()">编辑</a>
                    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-remove',plain:true" onclick="ListServiceLink.Del()">删除</a>
                </div>
            </div>
            <div title="图文" style="padding:10px;">
		        <table id="dgContent" class="easyui-datagrid" title="" data-options="data:[],rownumbers:true,pagination:true,fit:true,fitColumns:true,toolbar:'#toolbarContent'">
                    <thead>
                        <tr>
                            <th data-options="field:'Id',checkbox:true"></th>
                            <th data-options="field:'SPicture',width:60,formatter:ListService.FPicture">图片</th>
                            <th data-options="field:'Named',width:120">名称</th>
                            <th data-options="field:'Descr',width:200">简介</th>
                            <th data-options="field:'Sort',width:70">排序</th>
                            <th data-options="field:'ServiceItemName',width:100">服务分类</th>
                        </tr>
                    </thead>
                </table>
                <div id="toolbarContent" style="padding:5px;">
                    关键字：<input type="text" id="txtKeywordForContent" maxlength="50" class="txt" />
                    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-search'" onclick="ListServiceContent.Search()">查 詢</a>
                    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true" onclick="ListServiceContent.Add()">新增</a>
                    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-edit',plain:true" onclick="ListServiceContent.Edit()">编辑</a>
                    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-remove',plain:true" onclick="ListServiceContent.Del()">删除</a>
                </div>
            </div>
        </div>
        
    </div>
</div>

<div id="dlgServiceItem" class="easyui-dialog" title="新建/编辑服务分类" data-options="iconCls:'icon-save',closed:true,modal:true,
href:'/Templates/AddService.htm',buttons: [{
	    text:'确定',iconCls:'icon-ok',handler:function(){
		    ListService.Save();
	    }
    },{
	    text:'取消',iconCls:'icon-cancel',handler:function(){
		    $('#dlgServiceItem').dialog('close');
	    }
    }]" style="width:490px;height:390px;padding:10px">

</div>

<div id="dlgServiceVote" style="padding:10px"></div>
<div id="dlgServiceContent" style="padding:10px"></div>
<div id="dlgServiceLink" style="padding:10px"></div>

<script type="text/javascript" src="../../Scripts/Admin/Serve/ServicePicture.js"></script>
<script type="text/javascript" src="../../Scripts/Admin/Serve/ListServiceVote.js"></script>
<script type="text/javascript" src="../../Scripts/Admin/Serve/ListServiceContent.js"></script>
<script type="text/javascript" src="../../Scripts/Admin/Serve/ListServiceLink.js"></script>
<script type="text/javascript" src="../../Scripts/Admin/Serve/ListService.js"></script>

<script type="text/javascript">
    $(function () {
        try {
            $("#dlgServiceItem").dialog('refresh', '../../Templates/AddService.htm');
            ListService.Init();
        }
        catch (e) {
            $.messager.alert('错误提醒', e.name + ": " + e.message, 'error');
        }

    })

    
</script>

</asp:Content>
