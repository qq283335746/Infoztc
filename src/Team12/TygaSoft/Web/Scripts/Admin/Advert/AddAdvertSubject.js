

var AddAdvertSubject = {
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
            var title = $.trim($("#txtTitle").val());
            if ($("#hImgSinglePictureId").val() == "") {
                $.messager.alert('错误提醒', '广告图片不能为空', 'error');
                return false;
            }
            var pictureId = $("#hImgSinglePictureId").val();
            var playTime = $.trim($("#txtPlayTime").val()); ;
            var isDisable = $('input[type="radio"]:checked').val();

            $.ajax({
                url: "../../ScriptServices/AdminService.asmx/SaveAdvert",
                type: "post",
                data: '{model:{Id:"' + id + '",Title:"' + title + '",PlayTime:' + playTime + ',IsDisable:"' + isDisable + '",PictureId:"' + pictureId + '"}}',
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
                        setTimeout("window.location = \"tadvert.html\"", 500);
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