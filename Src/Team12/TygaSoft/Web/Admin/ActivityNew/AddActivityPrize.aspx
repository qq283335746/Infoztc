<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="AddActivityPrize.aspx.cs" Inherits="TygaSoft.Web.Admin.ActivityNew.AddActivityPrize" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <div class="row mt10">
        <span class="rl"><b class="cr">*</b>奖项名称：</span>
        <div class="fl">
            <input type="text" id="txtPrizeName" runat="server" clientidmode="Static" class="easyui-textbox mtxt"
                data-options="required:true,missingMessage:'必填项'" style="width: 200px" />
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10">
        <span class="rl"><b class="cr">*</b>奖项数量：</span>
        <div class="fl">
            <input type="text" id="txtPrizeCount" value="0" runat="server" clientidmode="Static"
                data-options="required:true,missingMessage:'必填项'" class="easyui-numberbox mtxt"
                style="width: 60px" />
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10">
        <span class="fl rl"><b class="cr">*</b>奖项图片：</span>
        <div class="fl ml10">
            <img id="imgSinglePicture" runat="server" clientidmode="Static" src="../../Images/nopic.gif"
                alt="图片" width="120" height="120" onclick="DlgPictureSelect.DlgSingle('PictureScratchLotto')" /><br />
            <input id="hImgSinglePictureId" runat="server" clientidmode="Static" type="hidden" />
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10">
        <span class="rl"><b class="cr">*</b>奖品内容：</span>
        <div class="fl">
            <textarea id="txtPrizeContent" runat="server" clientidmode="Static" rows="100" cols="100"
                style="overflow: auto; height: 80px; width: 400px; border: 1px solid #95B8E7;"></textarea>
        </div>
        <span class="clr"></span>
    </div>
        <div class="row mt10">
        <span class="rl"><b class="cr">*</b>预定中奖次数：</span>
        <div class="fl">
            <input type="text" id="txtWinningTimes" value="0" runat="server" clientidmode="Static"
                data-options="required:true,missingMessage:'必填项'" class="easyui-numberbox mtxt"
                style="width: 60px" />/天 
            <span id="spanZS" runat="server" clientidmode="Static" class="cr"></span>
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10">
        <span class="rl"><b class="cr">*</b>排序：</span>
        <div class="fl">
            <input type="text" id="txtSort" value="0" runat="server" clientidmode="Static"
                data-options="required:true,missingMessage:'必填项'" class="easyui-numberbox mtxt"
                style="width: 60px" />
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10">
        <span class="rl">商家名称：</span>
        <div class="fl">
            <input type="text" id="txtBusinessName" runat="server" clientidmode="Static" class="easyui-textbox mtxt"
                style="width: 200px" />
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10">
        <span class="rl">商家电话：</span>
        <div class="fl">
            <input type="text" id="txtBusinessPhone" runat="server" clientidmode="Static" class="easyui-textbox mtxt"
                style="width: 200px" />
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10">
        <span class="rl">商家地址：</span>
        <div class="fl">
            <input type="text" id="txtBusinessAddress" runat="server" clientidmode="Static" class="easyui-textbox mtxt"
                style="width: 400px" />
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10">
        <span class="rl">是否禁用：</span>
        <div class="fl">
            <input type="radio" id="rdFalse" runat="server" clientidmode="Static" name="rdIsDisable"
                value="False" checked="True" /><label>否</label>
            <input type="radio" id="rdTrue" runat="server" clientidmode="Static" name="rdIsDisable"
                value="True" /><label>是</label>
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10">
        <span class="rl">&nbsp;</span>
        <div class="fl">
            <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-save'"
                onclick="AddActivityPrize.OnSave()">提交</a>
        </div>
        <span class="clr"></span>
    </div>
    <input type="hidden" id="hId" runat="server" clientidmode="Static" />
    <input type="hidden" id="asId" runat="server" clientidmode="Static" />
    <div id="dlgUploadPicture" style="padding: 10px;">
    </div>
    <div id="dlgSingleSelectPicture" class="easyui-dialog" title="选择图片（单选）" data-options="closed:true,modal:true,href:'/t/yt.html?dlgId=dlgSingleSelectPicture&funName=PictureScratchLotto',width:810,height:$(window).height()*0.8,
    buttons: [{
    id:'btnSelectPicture',text:'确定',iconCls:'icon-ok',
        handler:function(){
            DlgPictureSelect.SetSinglePicture('imgSinglePicture');
            $('#dlgSingleSelectPicture').dialog('close');
        }
    },{
    id:'btnCancelSelectPicture', text:'取消',iconCls:'icon-cancel',
        handler:function(){
	        $('#dlgSingleSelectPicture').dialog('close');
        }
    }],
    toolbar:[{
        id:'dlgSingleSelectPictureToolbarUpload',text:'上传',iconCls:'icon-add',
		handler:function(){
            DlgPictureSelect.DlgUpload();
        }
	}]" style="padding: 10px;">
    </div>
    <script type="text/javascript" src="../../Scripts/Admin/DlgPictureSelect.js"></script>
    <script type="text/javascript" src="../../Scripts/Admin/ActivityNew/AddActivityPrize.js"></script>
    <script type="text/javascript">
        $(function () {
            try {
                AddActivityPrize.Init();
            }
            catch (e) {
                $.messager.alert('错误提醒', e.name + ": " + e.message, 'error');
            }

        })

    </script>
    <style scoped="scoped">
        .textbox
        {
            height: 20px;
            margin: 0;
            padding: 0 2px;
            box-sizing: content-box;
        }
    </style>
</asp:Content>
