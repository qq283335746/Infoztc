

var LotteryUrlConfig = {
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

            var itemAppend = "CPshenma*|*" + $.trim($("#txtCPshenma").val()) + "*,*CPshiping*|*" + $.trim($("#txtCPshiping").val()) + "*,*CPhuodong*|*" + $.trim($("#txtCPhuodong").val());

            $.ajax({
                url: "../../ScriptServices/AdminService.asmx/SaveLotteryInitItems",
                type: "post",
                data: '{itemAppend:"' + itemAppend + '"}',
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
    }
} 