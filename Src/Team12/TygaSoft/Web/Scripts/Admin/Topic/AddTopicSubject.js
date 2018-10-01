

var AddTopicSubject = {
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
            var content = $("#txtContent").val();
            var pictureId = $("#hImgSinglePictureId").val() == "" ? "00000000-0000-0000-0000-000000000000" : $("#hImgSinglePictureId").val();
            //var userId = $("#selectUser").combobox('getValue');
            var isTop = $("#isTop").val();
            var isDisable = $('input[name="ctl00$cphMain$rdIsDisable"]:checked').val();
            var isPush = $('input[name="ctl00$cphMain$rdIsPush"]:checked').val();

            $.ajax({
                url: "../../ScriptServices/AdminService.asmx/SaveTopic",
                type: "post",
                data: '{model:{Id:"' + id + '",Title:"' + title + '",ContentText:"' + content + '",IsTop:"' + isTop + '",IsDisable:"' + isDisable + '",PictureId:"' + pictureId + '",IsPush:"' + isPush + '"}}',
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
                        setTimeout("window.location = \"ttopic.html\";", 500);
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