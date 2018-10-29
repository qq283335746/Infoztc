<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DlgUploadPicture.aspx.cs" Inherits="TygaSoft.Web.Templates.Admin.DlgUploadPicture" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="dlgUploadFm" runat="server" enctype="multipart/form-data">
    <div class="mb10">
        <input id="file1" data-options="prompt:'选择图片',buttonText: '选择文件'" style="width:500px;" />
	</div>
    <asp:Literal runat="server" ID="ltrMyData"></asp:Literal>
    </form>
   
</body>
</html>
