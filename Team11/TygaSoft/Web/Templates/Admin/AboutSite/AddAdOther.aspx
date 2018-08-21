<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddAdOther.aspx.cs" Inherits="TygaSoft.Web.Templates.Admin.AboutSite.AddAdOther" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="dlgAdItemFm" runat="server">
    <div class="row">
        <span class="rl"><b class="cr">*</b> 图片：</span>
        <div class="fl">
            <img src="../../../Images/nopic.gif" id="imgPicture" runat="server" clientidmode="Static" onclick="DlgPictureSelect.DlgSingle('AdvertisementPicture')" alt="" width="110" height="110" />
            <input type="hidden" id="hImgPictureId" runat="server" clientidmode="Static" value="" />
        </div>
    </div>
    <div class="row mt10">
        <span class="rl"><b class="cr">*</b>作用类型：</span>
        <div class="fl">
            <asp:DropDownList ID="ddlActionType" runat="server" CssClass="easyui-validatebox" data-options="required:true,validType:'select'"></asp:DropDownList>
        </div>
    </div>
    <div class="row mt10">
        <span class="rl">排序：</span>
        <div class="fl">
            <input type="text" id="txtSort" runat="server" clientidmode="Static" class="easyui-validatebox txt" data-options="validType:'int'" style="width:108px;" />
        </div>
    </div>
    <div class="row mt10">
        <span class="rl">&nbsp;</span>
        <div class="fl">
            <input type="checkbox" id="cbIsDisable" runat="server" clientidmode="Static" /><label for="cbIsDisable">不显示</label>
        </div>
    </div>

    <input type="hidden" id="hAdItemId" runat="server" clientidmode="Static" />

    <asp:Literal ID="ltrMyOldData" runat="server"></asp:Literal>

    </form>

    <div id="dlgSingleSelectPicture" class="easyui-dialog" title="选择图片（单选）" data-options="closed:true,modal:true,href:'/t/tpicture.html?dlgId=dlgSingleSelectPicture&funName=AdvertisementPicture',width:810,height:$(window).height()*0.8,
    buttons: [{
        id:'btnSingleSelectPicture',text:'确定',iconCls:'icon-ok',
        handler:function(){
            DlgPictureSelect.SetSinglePicture('imgPicture');
            $('#dlgSingleSelectPicture').dialog('close');
        }
    },{
        id:'btnCancelSingleSelectPicture', text:'取消',iconCls:'icon-cancel',
        handler:function(){
            $('#dlgSingleSelectPicture').dialog('close');
        }
    }],
    toolbar:[{
        id:'dlgSingleSelectPictureToolbarUpload',text:'上传',iconCls:'icon-add',
		handler:function(){
            DlgPictureSelect.DlgUpload();
        }
	}]" style="padding:10px;"></div>

    <script type="text/javascript" src="../../../Scripts/Admin/DlgPictureSelect.js"></script>

</body>
</html>
