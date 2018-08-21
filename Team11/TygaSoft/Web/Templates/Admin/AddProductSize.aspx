<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddProductSize.aspx.cs" Inherits="TygaSoft.Web.Templates.Admin.AddProductSize" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>尺码</title>
</head>
<body>
    <form id="dlgProductSizeFm" runat="server">
        
        <div class="row">
            <span class="rl">商品项：</span>
            <div class="fl">
                <asp:DropDownList ID="ddlProductItem_ProductSize" runat="server" CssClass="easyui-validatebox" data-options="required:true,validType:'select'"></asp:DropDownList>
            </div>
        </div>

        <div class="row mt10">
            <span class="rl">&nbsp;</span>
            <div class="fl" style="width:460px;">
                <div class="easyui-panel" title="" data-options="height: $(window).height() * 0.74" style="padding:10px; width:100%;">

                    <div class="mb10">
                        <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true" onclick="AddProductSize.AddInput()">添加输入项</a>
                    </div>

                    <table id="dynamicSizeT" class="dynamicT">
                        <tbody>
                            <asp:Repeater ID="rpData" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            尺码：<input type="text" class="txt" value='<%#Container.DataItem %>' />
                                        </td>
                                        <td class="pdl10">
                                            <a href="javascript:void(0)" onclick="AddProductSize.UpInput(this)" class="mr10">上移</a>
                                            <a href="javascript:void(0)" onclick="AddProductSize.DownInput(this)" class="mr10">下移</a>
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
    </form>

    <script type="text/javascript">
        $(function () {
            AddProductSize.Init();
        })
    </script>
</body>
</html>
