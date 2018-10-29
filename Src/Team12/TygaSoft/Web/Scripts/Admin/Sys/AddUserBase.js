
var AddUserBase = {
    Init: function () {

    },
    GetMyData: function (clientId) {
        var myData = $("#" + clientId + "").html();
        return eval("(" + myData + ")");
    },
    OnSave: function () {
        try {
            var isValid = $('#form1').form('validate');
            if (!isValid) return false;

            var headPicture = $("#imgSinglePicture").attr("src");
            var sex = $("[id*=rbtnSex]:checked").val();
            var sTotalGold = $.trim($("#txtTotalGold").val());
            var sTotalSilver = $.trim($("#txtTotalSilver").val());
            var sTotalIntegral = $.trim($("#txtTotalIntegral").val());
            var sSilverLevel = $.trim($("#txtSilverLevel").val());
            if (sSilverLevel == "") sSilverLevel = 0
            var sColorLevel = $.trim($("#txtColorLevel").val());
            if (sColorLevel == "") sColorLevel = 0;
            var sIntegralLevel = $.trim($("#txtIntegralLevel").val());
            if (sIntegralLevel == "") sIntegralLevel = 0

            $.ajax({
                url: "../../ScriptServices/AdminService.asmx/SaveUserBase",
                type: "post",
                data: '{model:{UserId:"00000000-0000-0000-0000-000000000000",Username:"' + $.trim($("#hName").val()) + '",Nickname:"' + $.trim($("#txtNickname").val()) + '",HeadPicture:"' + headPicture + '",Sex:"' + sex + '",MobilePhone:"' + $.trim($("#txtMobilePhone").val()) + '",TotalGold:' + sTotalGold + ',TotalSilver:' + sTotalSilver + ',TotalIntegral:' + sTotalIntegral + ',SilverLevel:' + sSilverLevel + ',ColorLevel:"' + $.trim($("#txtColorLevel").val()) + '",IntegralLevel:' + sIntegralLevel + '}}',
                contentType: "application/json; charset=utf-8",
                beforeSend: function () {
                    $("#dlgWaiting").dialog('open');
                },
                complete: function () {
                    $("#dlgWaiting").dialog('close');
                },
                success: function (data) {
                    var msg = data.d;
                    if (msg == "1") {
                        jeasyuiFun.show("温馨提示", "保存成功！");
                    }
                    else {
                        $.messager.alert('系统提示', msg, 'info');
                    }
                }
            });
        }
        catch (e) {
            $.messager.alert('错误提醒', e.name + ": " + e.message, 'error');
        }
    },
    DlgUpload: function () {
        var dlgParentId = DlgSelectPicture.DlgOpenId;
        var isMutil = DlgSelectPicture.IsMutil;
        var h = $(window).height() * 0.9;
        $("#dlgUploadPicture").dialog({
            title: '上传文件',
            width: 606,
            height: h,
            closed: false,
            href: '/t/yy.html?dlgId=dlgUploadPicture&funName=UserHeadPicture&isMutil=' + isMutil + '&dlgParentId=' + dlgParentId + '&submitUrl=/h/tg.html',
            modal: true,
            buttons: [{
                id: 'btnOnUploadPicture', text: '上 传', iconCls: 'icon-ok',
                handler: function () {
                    dlgUploadPicture.OnUpload();
                }
            }, {
                id: 'btnCancelUploadPicture', text: '取 消', iconCls: 'icon-cancel',
                handler: function () {
                    $("#dlgUploadPicture").dialog('close');
                }
            }],
            toolbar: [{
                id: 'btnAddTextbox', text: '添 加', iconCls: 'icon-add',
                handler: function () {
                    dlgUploadPicture.OnToolbarAdd();
                }
            }]
        })
    }
}