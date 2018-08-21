

var AddQuestionBank = {
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

            var id = $("#hId").val() == "" ? "00000000-0000-0000-0000-000000000000" : $("#hId").val();
            var name = $.trim($("#txtName").val());
            var isDisable = $('input[type="radio"]:checked').val();

            $.ajax({
                url: "../../ScriptServices/AdminService.asmx/SaveQuestionBank",
                type: "post",
                data: '{model:{Id:"' + id + '",Named:"' + name + '",IsDisable:"' + isDisable + '"}}',
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
                        //if ($.trim($("#hId").val()) != "") {
                            setTimeout("window.location = \"yay.html\";", 500);
                        //}
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