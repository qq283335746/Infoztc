
var AddAdItemContent = {
    Init: function () {

    },
    OnSave: function () {
        try {
            $.messager.progress({ title: '请稍等', msg: '正在执行...' });
            $('#dlgFmAdItemContent').form('submit', {
                url: '/h/taboutsite.html',
                onSubmit: function (param) {

                    var isValid = $(this).form('validate');
                    if (!isValid) {
                        $.messager.progress('close');
                    }

                    param.reqName = "SaveAdItemContent";
                    param.adId = $("#hAdId").val();
                    param.adItemId = $("#hAdItemId").val();
                    param.content = editor_content.html().replace(/</g, "&lt;");
                    $("#txtaContent").text("");

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
                    setTimeout(function () {
                        window.location = "yad.html?adId=" + $("#hAdId").val();
                    }, 500)
                }
            });
        }
        catch (e) {
            $.messager.progress('close');
            $.messager.alert('错误提醒', e.name + ": " + e.message, 'error');
        }
    }
}