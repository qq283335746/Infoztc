<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddServiceVote.aspx.cs" Inherits="TygaSoft.Web.Templates.Admin.AddServiceVote" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="dlgFmServiceVote" runat="server">
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
            <span class="fl rl"><b class="cr">*</b>姓名：</span>
            <div class="fl ml10">
                <input type="text" id="txtNamed_ServiceVote" runat="server" clientidmode="Static" tabindex="1" maxlength="256" class="easyui-validatebox txt" data-options="required:true" />
            </div>
            <span class="clr"></span>
        </div>
        <div class="row mt10">
            <span class="fl rl"><b class="cr">*</b>头像：</span>
            <div class="fl ml10">
                <img id="imgHeadPicture_ServiceVote" runat="server" clientidmode="Static" src="../../Images/nopic.gif" alt="头像" width="150" height="140" onclick="ServicePicture.DlgSingle('imgHeadPicture_ServiceVote')" />
                <input type="hidden" id="hHeadPictureId_ServiceVote" runat="server" clientidmode="Static" /><br />
                <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true" onclick="ServicePicture.DlgSingle('imgHeadPicture_ServiceVote')">选 择</a>
            </div>
            <span class="clr"></span>
        </div>
        <div class="row mt10">
            <span class="fl rl">排序：</span>
            <div class="fl ml10">
                <input type="text" name="Sort" tabindex="4" maxlength="9" class="easyui-validatebox txt" data-options="validType:'int'" />
            </div>
            <span class="clr"></span>
        </div>
        <div class="row mt10">
            <span class="fl rl">简介：</span>
            <div class="fl ml10">
                <textarea id="txtaDescr_ServiceVote" runat="server" clientidmode="Static" rows="3" cols="100" style="width:100%; height:70px;"></textarea>
            </div>
            <span class="clr"></span>
        </div>
        <div class="row mt10">
            <span class="fl rl">内容：</span>
            <div class="fl ml10">
                <textarea id="txtaContent_ServiceVote" name="txtaContent_ServiceVote" runat="server" clientidmode="Static" cols="100" rows="8"></textarea>
            </div>
            <span class="clr"></span>
        </div>

        <input type="hidden" id="hServiceVoteId" runat="server" clientidmode="Static" />
    </div>

    <asp:Literal ID="ltrMyData" runat="server"></asp:Literal>

    </form>

    <script type="text/javascript">
        var editor_Vote;
        editor_Vote = KindEditor.create('textarea[name=txtaContent_ServiceVote]', {
            cssPath: '/Scripts/plugins/kindeditor/plugins/code/prettify.css',
            width: '1000px',
            height:'800px',
            uploadJson: '../../Handlers/Admin/HandlerKindeditor.ashx',
            fileManagerJson: '../../Handlers/Admin/HandlerKindeditor.ashx',
            allowFileManager: true,
            afterCreate: function () {
                var self = this;
                KindEditor.ctrl(document, 13, function () {
                });
                KindEditor.ctrl(self.edit.doc, 13, function () {
                });
            }
        });
        prettyPrint();

        var ServiceVote = {
            Init: function () {
                this.InitModel();
                this.InitParent();
            },
            InitParent: function () {
                var node = $("#treeCt").tree('getSelected');
                if (node) {
                    if ($("#hServiceVoteId").val() == "") {
                        var hParentId = $("#dlgFmServiceVote").find("[name=ServiceItemId]");
                        hParentId.val(node.id);
                        hParentId.prev().text(node.text);
                    }
                }
            },
            InitModel: function () {
                if ($.trim($("#hServiceVoteId").val()) != "") {
                    var fmId = $("#dlgFmServiceVote");
                    fmId.find("[id=myDataAppend_ServiceVote]").children().each(function () {
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
            ServiceVote.Init();
        })
    </script>

</body>
</html>
