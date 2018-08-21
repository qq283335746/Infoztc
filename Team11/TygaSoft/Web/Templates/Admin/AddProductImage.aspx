<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddProductImage.aspx.cs" Inherits="TygaSoft.Web.Templates.Admin.AddProductImage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>商品其他图片</title>
</head>
<body>
    <form id="dlgProductImageFm" runat="server">

        <div class="row">
            <span class="rl" style="width:80px;">商品项：</span>
            <div class="fl">
                <asp:DropDownList ID="ddlProductItem_ProductImage" runat="server" CssClass="easyui-validatebox" data-options="required:true,validType:'select'"></asp:DropDownList>
            </div>
        </div>
        <div class="row mt10">
            <span class="rl" style="width:80px;">&nbsp;</span>
            <div class="fl" style="width:540px;">
                <div class="easyui-panel" title="" data-options="height: $(window).height() * 0.74" style="padding:10px; width:100%;">

                    <div class="mb10">
                        <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true" onclick="AddProductImage.AddInput()">添加项</a>
                    </div>

                    <table id="dynamicImageT" class="dynamicImageT">
                        <tbody>
                            <asp:Repeater ID="rpData" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <img src='<%#Eval("Value") %>' alt="上传图片" width="100" height="100" onclick="AddProductImage.OnPictureClick(this)" />
                                            <input type="hidden" value='<%#Eval("Key") %>' />
                                        </td>
                                        <td class="pdl10">
                                            <a href="javascript:void(0)" onclick="AddProductImage.UpInput(this)" class="mr10">上移</a>
                                            <a href="javascript:void(0)" onclick="AddProductImage.DownInput(this)" class="mr10">下移</a>
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

    <div id="dlgSingleSelectProductImagePicture" class="easyui-dialog" title="选择图片" data-options="closed:true,modal:true,href:'/t/tpicture.html?dlgId=dlgSingleSelectProductImagePicture&funName=PictureProduct',width:810,height:$(window).height()*0.8,
        buttons: [{
        id:'btnSingleSelectProductImagePicture',text:'确定',iconCls:'icon-ok',
            handler:function(){
                AddProductImage.SetSinglePicture();
                $('#dlgSingleSelectProductImagePicture').dialog('close');
            }
        },{
        id:'btnCancelSingleSelectProductImagePicture', text:'取消',iconCls:'icon-cancel',
            handler:function(){
	            $('#dlgSingleSelectProductImagePicture').dialog('close');
            }
        }],
        toolbar:[{
                    id:'dlgSingleSelectProductImagePictureToolbarUpload',text:'上传',iconCls:'icon-add',
			        handler:function(){
                        DlgPictureSelect.DlgUpload();
                    }
		        }]
    " style="padding:10px;"></div>

    <script type="text/javascript">
        $(function () {
            AddProductImage.Init();
        })
    </script>

</body>
</html>
