

var AddActivityPrize = {
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
            var asId = $("#asId").val();
            if (asId == "") {
                $.messager.alert('错误提醒', '请先选择活动', 'error');
                return false;
            }
            var prizeName = $.trim($("#txtPrizeName").val());
            var prizeCount = $.trim($("#txtPrizeCount").val());
            if ($("#hImgSinglePictureId").val() == "") {
                $.messager.alert('错误提醒', '奖项图片不能为空', 'error');
                return false;
            }
            var pictureId = $("#hImgSinglePictureId").val();
            var prizeContent = $("#txtPrizeContent").val();
            if (prizeContent == "") {
                $.messager.alert('错误提醒', '奖品内容不能为空', 'error');
                return false;
            }
            var winningTimes = $.trim($("#txtWinningTimes").val());
            var sort = $.trim($("#txtSort").val());
            var businessName = $("#txtBusinessName").val();
            var businessPhone = $("#txtBusinessPhone").val();
            var businessAddress = $("#txtBusinessAddress").val();
            var isDisable = $('input[type="radio"]:checked').val();

            $.ajax({
                url: "../../ScriptServices/AdminService.asmx/SaveActivityPrize",
                type: "post",
                data: '{model:{Id:"' + id + '",ActivityId:"' + asId + '",PrizeName:"' + prizeName + '",PrizeCount:' + prizeCount + ',PrizeContent:"' + prizeContent + '",WinningTimes:' + winningTimes + ',Sort:' + sort + ',BusinessName:"' + businessName + '",BusinessPhone:"' + businessPhone + '",BusinessAddress:"' + businessAddress + '",IsDisable:"' + isDisable + '",PictureId:"' + pictureId + '"}}',
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
                        setTimeout("window.location = \"yyactivity.html?asId=" + $("#asId").val() + "\";", 500);
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