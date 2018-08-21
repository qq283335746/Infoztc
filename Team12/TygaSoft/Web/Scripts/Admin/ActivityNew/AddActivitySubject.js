

var AddActivitySubject = {
    Init: function () {
        this.BindPrizeClick();
    },
    GetMyData: function (clientId) {
        var myData = $("#" + clientId + "").html();
        return eval("(" + myData + ")");
    },
    BindPrizeClick: function () {
        $("input[name='ctl00$cphMain$rdIsPrize']").click(function () {
            if ($(this).attr("id") == "rdPrizeFalse") {
                $("#divPrize").hide();
            }
            else {
                $("#divPrize").show();
            }
        });
    },
    OnSave: function () {
        try {
            $.messager.progress({
                title: '请稍等',
                msg: '正在执行...'
            });
            $('#form1').form('submit', {
                url: '/h/tactivity.html',
                onSubmit: function (param) {
                    var isValid = $(this).form('validate');
                    if (!isValid) {
                        $.messager.progress('close');
                    }

                    param.reqName = "SaveActivitySubject";
                    param.pictureId = $("#hImgSinglePictureId").val() == "" ? "00000000-0000-0000-0000-000000000000" : $("#hImgSinglePictureId").val();
                    param.content = editor_content.html().replace(/</g, "&lt;");
                    param.signUpRule = editor_signUpRule.html().replace(/</g, "&lt;");
                    param.prizeRule = editor_PrizeRule.html().replace(/</g, "&lt;");
                    param.hideAttr = "";
                    $('input[type="checkbox"]:checked').each(function () {
                        param.hideAttr += $(this).attr("id") + ",";
                    });

                    param.isPrize = $('input[name="ctl00$cphMain$rdIsPrize"]:checked').val();
                    param.isDisable = $('input[name="ctl00$cphMain$rdIsDisable"]:checked').val();
                    param.isPush = $('input[name="ctl00$cphMain$rdIsPush"]:checked').val();

                    if ($("#hId").val() != "") {
                        $("#txtContent").text("");
                        $("#txtSignUpRule").text("");
                        $("#txtPrizeRule").text("");
                    }

                    return isValid;
                },
                success: function (data) {
                    $.messager.progress('close');
                    var jsonData = eval('(' + data + ')');
                    if (!jsonData.success) {
                        $.messager.alert('错误提示', jsonData.message, 'error');
                        return false;
                    }
                    jeasyuiFun.show("温馨提醒", jsonData.message);
                    setTimeout("window.location = \"tactivity.html\";", 500);
                }
            });
        }
        catch (e) {
            $.messager.alert('错误提醒', e.name + ": " + e.message, 'error');
        }
    }
} 