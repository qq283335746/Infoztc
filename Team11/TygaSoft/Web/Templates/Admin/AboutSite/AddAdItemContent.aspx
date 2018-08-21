<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddAdItemContent.aspx.cs" Inherits="TygaSoft.Web.Templates.Admin.AboutSite.AddAdItemContent" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <link href="../../Scripts/plugins/kindeditor/themes/default/default.css" rel="stylesheet" type="text/css" />
    <link href="../../Scripts/plugins/kindeditor/plugins/code/prettify.css" rel="stylesheet" type="text/css" /> 
    <script type="text/javascript" src="../../Scripts/plugins/kindeditor/kindeditor.js"></script>
    <script type="text/javascript" src="../../Scripts/plugins/kindeditor/lang/zh_CN.js"></script>
    <script type="text/javascript" src="../../Scripts/plugins/kindeditor/plugins/code/prettify.js"></script>
    <form id="dlgFmAdItemContent" runat="server">
    <div>
        <div class="row">
            <span class="rl"><b class="cr">*</b> 简介：</span>
            <div class="fl">
                <textarea id="txtaDescr" runat="server" clientidmode="Static" rows="3" cols="80" class="txta" style="width:720px;"></textarea>
            </div>
        </div>
        <div class="row mt10">
            <span class="rl"><b class="cr">*</b> 内容：</span>
            <div class="fl">
                <textarea id="txtaContent" runat="server" clientidmode="Static" cols="100" rows="8" style="width:800px;height:800px;"></textarea>
            </div>
        </div>
    </div>
    </form>

    <script type="text/javascript" src="../../../Scripts/Admin/AboutSite/AddItemContent.js"></script>
    <script type="text/javascript">
        var h = $(window).height() * 0.8;
        var editor_content;
        editor_content = KindEditor.create('textarea[name=txtaContent]', {
            cssPath: '/Scripts/plugins/kindeditor/plugins/code/prettify.css',
            width: '730',
            height: h,
            uploadJson: '/h/ty.html',
            fileManagerJson: '/h/ty.html',
            allowFileManager: true,
            afterCreate: function () {
                var self = this;
                KindEditor.ctrl(document, 13, function () {
                });
                KindEditor.ctrl(self.edit.doc, 13, function () {
                });
            }
        });
    </script>
</body>
</html>
