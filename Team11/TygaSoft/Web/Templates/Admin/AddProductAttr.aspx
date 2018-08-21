<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddProductAttr.aspx.cs" Inherits="TygaSoft.Web.Templates.Admin.AddProductAttr" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="dlgProductAttrFm" runat="server">
        
        <div class="row">
            <span class="rl" style="width:80px;">商品项：</span>
            <div class="fl">
                <asp:DropDownList ID="ddlProductItem" runat="server" CssClass="easyui-validatebox" data-options="required:true,validType:'select'"></asp:DropDownList>
            </div>
        </div>
        <div class="row mt10">
            <span class="rl" style="width:80px;">&nbsp;</span>
            <div class="fl" style="width:650px;">
                <div class="easyui-panel" title="" data-options="height: $(window).height() * 0.74" style="padding:10px; width:100%;">

                    <div class="mb10">
                        <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true" onclick="AddProductAttr.AddInput()">添加输入项</a>
                        <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-save',plain:true" onclick="AddProductAttr.DlgToTeamplate()">保存为模板</a>
                        <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true" onclick="AddProductAttr.DlgSelectProductAttrTeamplate()">选择模板</a>
                    </div>

                    <table id="dynamicAttrT" class="dynamicImageT">
                        <tbody>
                            <asp:Repeater ID="rpData" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <input type="text" name="attrItem" class="txt" value='<%#Eval("Key") %>' />
                                        </td>
                                        <td class="pdl10">
                                            <input type="text" name="attrItem" class="txt" value='<%#Eval("Value") %>' />
                                        </td>
                                        <td class="pdl10">
                                            <a href="javascript:void(0)" onclick="AddProductAttr.UpInput(this)" class="mr10">上移</a>
                                            <a href="javascript:void(0)" onclick="AddProductAttr.DownInput(this)" class="mr10">下移</a>
                                            <a code="del" href="javascript:void(0)" onclick="$(this).parent().parent().remove()" class="mr10">删除</a>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>
                
            </div>
            <span class="clr"></span>
        </div>

        <asp:Literal ID="ltrData" runat="server"></asp:Literal>

        <div id="dlgSaveProductAttrTemplate" class="easyui-dialog" title="保存模板" data-options="closed:true,modal:true,width:500,height:150,
            buttons: [{
                id:'btnSaveProductAttrTemplate',text:'保存',iconCls:'icon-ok',
                handler:function(){
                    AddProductAttr.SaveToTeamplate();
                }
            },{
                id:'btnCancelSaveProductAttrTemplate', text:'取消',iconCls:'icon-cancel',
                handler:function(){
                    $('#dlgSaveProductAttrTemplate').dialog('close');
                }
            }]
        " style="padding:10px;">
                <div class="row">
                    <span class="rl">模板名称：</span>
                    <div class="fl">
                        <input type="text" id="txtTName" class="txt" />
                    </div>
                </div>
                <input type="hidden" id="hTValue" />
        </div>

        <div id="dlgProductAttrSelectTemplate" class="easyui-dialog" title="选择模板" data-options="closed:true,modal:true,width:800,height:500,
            buttons: [{
                id:'btnSelectProductAttrTemplate',text:'确定',iconCls:'icon-ok',
                handler:function(){
                    AddProductAttr.SetProductAttrTeamplate();
                }
            },{
                id:'btnCancelSelectProductAttrTemplate', text:'取消',iconCls:'icon-cancel',
                handler:function(){
                    $('#dlgProductAttrSelectTemplate').dialog('close');
                }
            }]
        " style="padding:10px;">
              <table id="dgProductAttrTemplate" class="easyui-datagrid" data-options="fit:true,fitColumns:true,singleSelect:true,pagination:true,toolbar:'#dgProductAttrTemplateToolbar'">
                <thead>
		            <tr>
			            <th data-options="field:'Id',checkbox:true"></th>
                        <th data-options="field:'TValue',hidden:true"></th>
			            <th data-options="field:'TName',width:300">模板名称</th>
		            </tr>
                </thead>
            </table>
            <div id="dgProductAttrTemplateToolbar" style="padding:5px;">
                关键字：<input type="text" id="txtKeyword_ProductAttrTemplate" class="txt" />
            </div>
        </div>
    </form>

    <script type="text/javascript">
        $(function () {
            AddProductAttr.Init();
        })
    </script>
</body>
</html>
