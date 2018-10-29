

var AddIssue = {
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
            var startDate = $("#startDate").datetimebox('getValue');
            var endDate = $("#endDate").datetimebox('getValue');
            var start = new Date(startDate.replace("-", "/").replace("-", "/"));
            var end = new Date(endDate.replace("-", "/").replace("-", "/"));
            if (end < start) {
                $.messager.alert('错误提醒', '有效期开始时间不能大于结束时间', 'error');
                return false;
            }
            var questionCount = $.trim($("#txtCount").val());
            var isDisable = $('input[type="radio"]:checked').val();
            var count = 0;
            var dataQAB = "[";
            $('#cphMain_qbList input[textboxname="qbList"]').each(function () {
                var id = $(this).attr("id");
                var val = $("#" + id).numberbox('getValue');
                count = count + val * 1;
                dataQAB += '{QuestionBankId:"' + id + '",QuestionCount:' + val + '},';
            });
            var dataQAB = dataQAB.substr(0, dataQAB.length - 1) + "]";
            if (questionCount != count) {
                $.messager.alert('错误提醒', '题目数和题库题目数总和不等', 'error');
                return false;
            }

            $.ajax({
                url: "../../ScriptServices/AdminService.asmx/SaveIssue",
                type: "post",
                data: '{model:{Id:"' + id + '",Title:"' + name + '",StartDate:"' + startDate + '",EndDate:"' + endDate + '",QuestionCount:' + questionCount + ',IsDisable:"' + isDisable + '",listAQB:' + dataQAB + '}}',
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
                        setTimeout("window.location = \"tli.html\";", 500);
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