<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddServiceLink.aspx.cs" Inherits="TygaSoft.Web.Templates.Admin.AddServiceLink" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="dlgFmServiceLink" runat="server">
    <div>
        <div class="row">
            <span class="fl rl">所属服务分类：</span>
            <div class="fl ml10">
                <span></span>
                <input type="hidden" name="ServiceItemId" />
            </div>
            <span class="clr"></span>
        </div>
        <div class="row mt10">
            <span class="fl rl"><b class="cr">*</b>名称：</span>
            <div class="fl ml10">
                <input type="text" id="txtNamed_ServiceLink" runat="server" clientidmode="Static" tabindex="1" maxlength="256" class="easyui-validatebox ltxt" data-options="required:true" />
            </div>
            <span class="clr"></span>
        </div>
        <div class="row mt10">
            <span class="fl rl">图片：</span>
            <div class="fl ml10">
                <img id="imgPicture_ServiceLink" runat="server" clientidmode="Static" src="../../Images/nopic.gif" alt="图片" width="150" height="140" onclick="ListServiceLink.OnPictureClick()" />
                <input type="hidden" id="hPictureId_ServiceLink" runat="server" clientidmode="Static" />
            </div>
            <span class="clr"></span>
        </div>
        <div class="row mt10">
            <span class="fl rl">排序：</span>
            <div class="fl ml10">
                <input type="text" name="Sort" tabindex="4" maxlength="9" class="easyui-validatebox stxt" data-options="validType:'int'" />
            </div>
            <span class="clr"></span>
        </div>
        <div class="row mt10">
            <span class="rl">有效时间：</span>
            <div class="fl ml10">
                <input id="txtEnableStartTime_ServiceLink" runat="server" clientidmode="Static" class="easyui-datetimebox" style="width:160px;" />
            </div>
            <div class="fl mlr10"> 至 </div>
            <div class="fl">
                <input id="txtEnableEndTime_ServiceLink" runat="server" clientidmode="Static" class="easyui-datetimebox" style="width:160px;" />
            </div>
            <span class="clr"></span>
        </div>
        <div class="row mt10">
            <span class="rl">是否禁用：</span>
            <div class="fl ml10">
                <asp:RadioButtonList ID="rbtnList_ServiceLink" runat="server" ClientIDMode="Static" RepeatDirection="Horizontal" RepeatLayout="Flow"></asp:RadioButtonList>
            </div>
            <span class="clr"></span>
        </div>
        <div class="row mt10">
            <span class="fl rl">Url：</span>
            <div class="fl ml10">
                <input type="text" id="txtUrl_ServiceLink" runat="server" clientidmode="Static" tabindex="1" maxlength="256" class="ltxt" />
            </div>
            <span class="clr"></span>
        </div>

        <input type="hidden" id="hServiceLinkId" name="hServiceLinkId" runat="server" clientidmode="Static" />
    </div>

    <asp:Literal ID="ltrMyData" runat="server"></asp:Literal>

    </form>

    <div id="dlgSingleSelectServiceLinkPicture" class="easyui-dialog" title="选择图片（单选）" data-options="closed:true,modal:true,href:'/t/tpicture.html?dlgId=dlgSingleSelectServiceLinkPicture&funName=PictureServiceLink',width:810,height:$(window).height()*0.8,
        buttons: [{
            id:'btnSingleSelectServiceLinkPicture',text:'确定',iconCls:'icon-ok',
            handler:function(){
                ListServiceLink.SetSinglePicture('imgPicture_ServiceLink');
                $('#dlgSingleSelectServiceLinkPicture').dialog('close');
            }
        },{
            id:'btnCancelSingleSelectServiceLinkPicture', text:'取消',iconCls:'icon-cancel',
            handler:function(){
                $('#dlgSingleSelectServiceLinkPicture').dialog('close');
            }
        }],
        toolbar:[{
            id:'dlgSingleSelectServiceLinkPictureToolbarUpload',text:'上传',iconCls:'icon-add',
		    handler:function(){
                DlgPictureSelect.DlgUpload();
            }
	    }]" style="padding:10px;"></div>

    <script type="text/javascript">

        var ServiceLink = {
            Init: function () {
                this.InitModel();
                this.InitParent();
            },
            InitParent: function () {
                var node = $("#treeCt").tree('getSelected');
                if (node) {
                    if ($("#hServiceLinkId").val() == "") {
                        var hParentId = $("#dlgFmServiceLink").find("[name=ServiceItemId]");
                        hParentId.val(node.id);
                        hParentId.prev().text(node.text);
                    }
                }
            },
            InitModel: function () {
                if ($.trim($("#hServiceLinkId").val()) != "") {
                    var fmId = $("#dlgFmServiceLink");
                    fmId.find("[id=myDataAppend_ServiceLink]").children().each(function () {
                        if ($(this).attr("code") == "myDataForModel") {
                            var jsonModel = eval("(" + $(this).html() + ")");
                            $.map(jsonModel, function (item) {
                                fmId.find("[name=ServiceItemId]").val(item.ServiceItemId);
                                fmId.find("[name=ServiceItemId]").prev().text(item.ServiceItemName);
                                fmId.find("[name=Sort]").val(item.Sort);

                            })
                        }
                    })
                }
            }
        }

        $(function () {
            ServiceLink.Init();
        })
    </script>
</body>
</html>
