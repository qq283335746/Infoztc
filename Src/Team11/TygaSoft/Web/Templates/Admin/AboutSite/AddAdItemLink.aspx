<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddAdItemLink.aspx.cs" Inherits="TygaSoft.Web.Templates.Admin.AboutSite.AddAdItemLink" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="dlgFmAdItemLink" runat="server">
    <div>
        <div class="row">
            <span class="rl"><b class="cr">*</b> 链接地址：</span>
            <div class="fl">
                <input type="text" id="txtUrl" runat="server" clientidmode="Static" class="easyui-validatebox ltxt" data-options="required:true" />
            </div>
        </div>
    </div>
    </form>

    <script type="text/javascript" src="../../../Scripts/Admin/AboutSite/AddAdItemLink.js"></script>
    <script type="text/javascript">
        $(function () {
            AddAdItemLink.Init();
        })
    </script>

</body>
</html>
