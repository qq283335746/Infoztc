<%@ Page Title="新建彩票" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="AddLottery.aspx.cs" Inherits="TygaSoft.Web.Admin.Lottery.AddLottery" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <div class="row mt10">
        <span class="rl"><b class="cr">*</b>期号：</span>
        <div class="fl">
            <input type="text" id="txtQS" runat="server" clientidmode="Static" class="easyui-numberbox"
                data-options="required:true,missingMessage:'必填项'" style="width: 200px" />
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10">
        <span class="rl"><b class="cr">*</b>海南期号：</span>
        <div class="fl">
            <input type="text" id="txtHNQS" runat="server" clientidmode="Static" class="easyui-numberbox"
                data-options="required:true,missingMessage:'必填项'" style="width: 200px" />
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10">
        <span class="rl"><b class="cr">*</b>开奖时间：</span>
        <div class="fl">
           <input class="easyui-datetimebox" id="txtLTime" runat="server" clientidmode="Static" data-options="required:true,missingMessage:'必填项',showSeconds: false" style="width:200px"/>
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10">
        <span class="rl"><b class="cr">*</b>开奖号码：</span>
        <div class="fl">
            <input type="text" id="txtLNo" runat="server" maxlength="7" clientidmode="Static" class="easyui-validatebox textbox"
                data-options="required:true,missingMessage:'必填项',validType:'length[7,7]'" style="width: 195px;" />
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10">
        <span class="rl"><b class="cr"></b>兑奖截止日期：</span>
        <div class="fl">
           <input class="easyui-datebox mtxt" id="txtECDate" runat="server" clientidmode="Static" style="width:200px"/>
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10">
        <span class="rl"><b class="cr"></b>本期销量：</span>
        <div class="fl">
            <input type="text" id="txtSV" runat="server" clientidmode="Static" class="easyui-numberbox mtxt" style="width: 200px" />
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10">
        <span class="rl"><b class="cr"></b>奖池滚存：</span>
        <div class="fl">
            <input type="text" id="txtPro" runat="server" clientidmode="Static" class="easyui-numberbox mtxt" style="width: 200px" />
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10">
        <span class="rl"><b class="cr"></b>详情：</span>
        <div class="fl">
            <textarea id="txtContentText" runat="server" style="overflow:auto;height: 100px; width:600px; border:1px solid #95B8E7;"></textarea>  
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10">
        <span class="rl">是否推送：</span>
        <div class="fl">
            <input type="radio" id="rdPushFalse" runat="server" name="rdIsPush" value="false" clientidmode="Static" checked="true" /><label>否</label>
            <input type="radio" id="rdPushTrue" runat="server" name="rdIsPush" value="true" clientidmode="Static" /><label>是</label>
        </div>
        <span class="clr"></span>
    </div>
    <div class="row mt10">
        <span class="rl">&nbsp;</span>
        <div class="fl">
            <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-save'"
                onclick="AddLottery.OnSave()">提交</a>
        </div>
        <span class="clr"></span>
    </div>
    <input type="hidden" id="hId" runat="server" clientidmode="Static" />
    <script type="text/javascript" src="../../Scripts/Admin/Lottery/AddLottery.js"></script>
    <script type="text/javascript">
        $(function () {
            try {
                AddLottery.Init();
            }
            catch (e) {
                $.messager.alert('错误提醒', e.name + ": " + e.message, 'error');
            }

        })

    </script>
    <style scoped="scoped">
        .textbox{
            height:20px;
            margin:0;
            padding:0 2px;
            box-sizing:content-box;
        }
    </style>
</asp:Content>
