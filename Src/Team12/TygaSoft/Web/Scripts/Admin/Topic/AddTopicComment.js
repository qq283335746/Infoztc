

var AddTopicComment = {
    Init: function () {
        this.SelectUser();
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
            var tsId = $("#tsId").val();
            if (tsId == "") {
                $.messager.alert('错误提醒', '请先选择话题', 'error');
                return false;
            }
            var content = $("#txtContent").val();
            var userId = $("#selectUser").combobox('getValue');
            var isTop = $("#isTop").val();
            var isDisable = $('input[type="radio"]:checked').val();

            $.ajax({
                url: "../../ScriptServices/AdminService.asmx/SaveTopicComment",
                type: "post",
                data: '{model:{Id:"' + id + '",TopicSubjectId:"' + tsId + '",UserId:"' + userId + '",ContentText:"' + content + '",IsTop:"' + isTop + '",IsDisable:"' + isDisable + '"}}',
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
                        setTimeout("window.location = \"gtopic.html?tsId=" + $("#tsId").val() + "\";", 500);
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
    },
    SelectUser: function () {
        $.ajax({
            url: "../../ScriptServices/AdminService.asmx/GetTopicUserList",
            type: "post",
            contentType: "application/json; charset=utf-8",
            data: '{}',
            beforeSend: function () {
            },
            complete: function () {
            },
            success: function (data) {
                var jsonData = (new Function("", "return " + data.d))();
                $("#selectUser").combobox({
                    data: jsonData,
                    valueField: "UserId",
                    textField: "Username",
                    panelHeight: 'auto',
                    editable: false,
                    onLoadSuccess: function () {
                        if ($("#UserId").val() == "") {
                            var data = $('#selectUser').combobox('getData');
                            if (data.length > 0) {
                                $("#selectUser").combobox('select', data[0].UserId);
                            }
                        }
                        else {
                            $("#selectUser").combobox('select', $("#UserId").val());
                        }
                    }
                });
            }
        });
    }
} 