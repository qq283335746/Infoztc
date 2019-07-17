<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DlgErnieItem.aspx.cs" Inherits="TygaSoft.Web.Templates.Admin.Lottery.DlgErnieItem" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>参数设定</title>
    
</head>
<body>
    <form id="dlgErnieItemFm" runat="server">
    <div>
        <div class="row">
            <span class="rl"><span class="cr">*</span>类型：</span>
            <div class="fl">
                <asp:DropDownList ID="ddlNumType" runat="server" class="easyui-validatebox" data-options="required:true,validType:'select'"></asp:DropDownList>
            </div>
        </div>
        <div class="row mt10">
            <span class="rl"><span class="cr">*</span>数值：</span>
            <div class="fl">
                <asp:CheckBoxList ID="cbListNum" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal" CssClass="cb_a"></asp:CheckBoxList>
            </div>
        </div>
        <div class="row mt10">
            <span class="rl"><span class="cr">*</span>现率：</span>
            <div class="fl">
                <input type="text" id="txtAppearRatio" runat="server" class="easyui-validatebox" data-options="required:true,validType:'float'" style="width:60px;" />
                (正确格式：浮点数)
            </div>
        </div>

        <input type="hidden" id="hErnieItemId" runat="server" />

    </div>
    </form>
</body>
</html>
