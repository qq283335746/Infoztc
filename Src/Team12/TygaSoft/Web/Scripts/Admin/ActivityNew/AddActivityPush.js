

var AddActivityPush = {
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

            var id = $("#hId").val();
            if (id == "") {
                $.messager.alert('系统提示', "请选择活动进行推送", 'info');
                return false;
            }
            var title = $.trim($("#txtTitle").val());
            var content = $.trim($("#txtConetent").val());

            $.ajax({
                url: "../../ScriptServices/AdminService.asmx/SaveActivityPush",
                type: "post",
                data: '{model:{ActivityId:"' + id + '",Title:"' + title + '",Content:"' + content + '"}}',
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
                        jeasyuiFun.show("温馨提示", "推送成功！");
                        setTimeout("window.location = \"tactivity.html\";", 500);
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