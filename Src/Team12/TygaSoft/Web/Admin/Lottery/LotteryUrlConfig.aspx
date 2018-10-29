<%@ Page Title="彩票Url配置" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="LotteryUrlConfig.aspx.cs" Inherits="TygaSoft.Web.Admin.Lottery.LotteryUrlConfig" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

     <link href="../../Styles/Team.css" rel="stylesheet" type="text/css" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <div class="row mt10">
        <span class="rl"><b class="cr">*</b>彩票神码URL：</span>
        <div class="fl">
            <input type="text" id="txtCPshenma" runat="server" clientidmode="Static" class="easyui-textbox mtxt"
                data-options="required:true,missingMessage:'必填项'" style="width: 400px" />
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10">
        <span class="rl"><b class="cr">*</b>彩票视频URL：</span>
        <div class="fl">
            <input type="text" id="txtCPshiping" runat="server" clientidmode="Static" class="easyui-textbox mtxt"
                data-options="required:true,missingMessage:'必填项'" style="width: 400px" />
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10">
        <span class="rl"><b class="cr">*</b>彩票活动URL：</span>
        <div class="fl">
            <input type="text" id="txtCPhuodong" runat="server" clientidmode="Static" class="easyui-textbox mtxt"
                data-options="required:true,missingMessage:'必填项'" style="width: 400px" />
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10">
        <span class="rl">&nbsp;</span>
        <div class="fl">
            <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-save'"
                onclick="LotteryUrlConfig.OnSave()">提交</a>
        </div>
        <span class="clr"></span>
    </div>
    <script type="text/javascript" src="../../Scripts/Admin/Lottery/LotteryUrlConfig.js"></script>
    <script type="text/javascript">
        $(function () {
            try {
                LotteryUrlConfig.Init();
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
