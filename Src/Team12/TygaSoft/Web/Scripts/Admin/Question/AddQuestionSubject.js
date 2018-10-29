

var AddQuestionSubject = {
    Init: function () {
        this.SelectQB();
    },
    GetMyData: function (clientId) {
        var myData = $("#" + clientId + "").html();
        return eval("(" + myData + ")");
    },
    SelectQB: function () {
        $.ajax({
            url: "../../ScriptServices/AdminService.asmx/GetQuestionBankList",
            type: "post",
            contentType: "application/json; charset=utf-8",
            data: '{}',
            beforeSend: function () {
            },
            complete: function () {
            },
            success: function (data) {
                var jsonData = (new Function("", "return " + data.d))();
                $("#selectQB").combobox({
                    data: jsonData,
                    valueField: "Id",
                    textField: "Named",
                    panelHeight: 'auto',
                    editable: false,
                    onLoadSuccess: function () {
                        if ($("#qbId").val() == "") {
                            var data = $('#selectQB').combobox('getData');
                            if (data.length > 0) {
                                $("#selectQB").combobox('select', data[0].Id);
                            }
                        }
                        else {
                            $("#selectQB").combobox('select', $("#qbId").val());
                        }
                    }
                });
            }
        });
    },
    OnSave: function () {
        try {
            var isValid = $('#form1').form('validate');
            if (!isValid) return false;

            var id = $("#hId").val() == "" ? "00000000-0000-0000-0000-000000000000" : $("#hId").val();
            var qbId = $("#selectQB").combobox("getValue");
            var name = $.trim($("#txtName").val());
            var qType = $("#selectType").combobox("getValue");
            var isDisable = $('input[type="radio"]:checked').val();

            $.ajax({
                url: "../../ScriptServices/AdminService.asmx/SaveQuestionSubject",
                type: "post",
                data: '{model:{Id:"' + id + '",QuestionBankId:"' + qbId + '",QuestionContent:"' + name + '",QuestionType:' + qType + ',IsDisable:"' + isDisable + '"}}',
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
                        setTimeout("window.location = \"yaa.html\";", 500);
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