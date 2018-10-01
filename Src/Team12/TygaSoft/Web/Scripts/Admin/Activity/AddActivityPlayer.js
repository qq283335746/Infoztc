

var AddActivityPlayer = {
    Init: function () {

    },
    GetMyData: function (clientId) {
        var myData = $("#" + clientId + "").html();
        return eval("(" + myData + ")");
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

                    param.reqName = "SaveActivityPlayer";
                    param.pictureId = $("#hImgSinglePictureId").val() == "" ? "00000000-0000-0000-0000-000000000000" : $("#hImgSinglePictureId").val();
                    param.detailInfo = editor_content.html().replace(/</g, "&lt;");
                    param.isDisable = $('input[type="radio"]:checked').val();           

                    if ($("#hId").val() != "") {
                        $("#txtContent").text("");
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
                    setTimeout("window.location = \"gactivity.html?asId=" + $("#asId").val() + "\";", 500);
                }
            });
        }
        catch (e) {
            $.messager.progress('close');
            $.messager.alert('错误提醒', e.name + ": " + e.message, 'error');
        }
    }
} 