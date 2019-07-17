

var AddActivitySubject = {
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
            var type = $("#selectType").combobox('getValue');
            var title = $.trim($("#txtTitle").val());
            var content = $("#txtContent").val();
            var pictureId = $("#hImgSinglePictureId").val() == "" ? "00000000-0000-0000-0000-000000000000" : $("#hImgSinglePictureId").val();
            var startDate = $("#startDate").datetimebox('getValue');
            var endDate = $("#endDate").datetimebox('getValue');
            var start = new Date(startDate.replace("-", "/").replace("-", "/"));
            var end = new Date(endDate.replace("-", "/").replace("-", "/"));
            if (end < start) {
                $.messager.alert('错误提醒', '有效期开始时间不能大于结束时间', 'error');
                return false;
            }
            var sort = $.trim($("#txtSort").val());
            var maxVoteCount = $.trim($("#txtMaxVoteCount").val());
            var voteMultiple = $.trim($("#txtVoteMultiple").val());
            if (type == "0" && voteMultiple * 1 < 1) {
                $.messager.alert('错误提醒', '投票倍数不能小于1！', 'error');
                return false;
            }
            var maxSignUpCount = $.trim($("#txtMaxSignUpCount").val());
            var actualSignUpCount = $("#txtMaxVoteCount").val() == "" ? "0" : $.trim($("#txtActualSignUpCount").val());
            var updateSignUpCount = $("#txtUpdateSignUpCount").val() == "" ? "0" : $.trim($("#txtUpdateSignUpCount").val());
            var isDisable = $('input[type="radio"]:checked').val();

            var data = '{model:{Id:"' + id + '",Title:"' + title + '",ContentText:"' + content + '",StartDate:"' + startDate + '",EndDate:"' + endDate +
                '",ActivityType:' + type + ',MaxVoteCount:' + maxVoteCount + ',VoteMultiple:' + voteMultiple + ',MaxSignUpCount:' + maxSignUpCount + ',ActualSignUpCount:' + actualSignUpCount +
                ',UpdateSignUpCount:' + updateSignUpCount + ',Sort:' + sort + ',IsDisable:"' + isDisable + '",PictureId:"' + pictureId + '"}}'

            $.ajax({
                url: "../../ScriptServices/AdminService.asmx/SaveActivity",
                type: "post",
                data: data,
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