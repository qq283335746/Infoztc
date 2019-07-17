

var AddLottery = {
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
            var qs = $.trim($("#txtQS").val());
            var hnqs = $.trim($("#txtHNQS").val());
            var lTime = $("#txtLTime").datetimebox('getValue');
            var lNo = $.trim($("#txtLNo").val());
            var ecDate = $("#txtECDate").datebox('getValue');
            var sv = $.trim($("#txtSV").val()) == "" ? "0" : $.trim($("#txtSV").val());
            var pro = $.trim($("#txtPro").val()) == "" ? "0" : $.trim($("#txtPro").val());
            var content = $("#cphMain_txtContentText").val();
            var isPush = $('input[name="ctl00$cphMain$rdIsPush"]:checked').val();

            $.ajax({
                url: "../../ScriptServices/AdminService.asmx/SaveLottery",
                type: "post",
                data: '{model:{Id:"' + id + '",QS:"' + qs + '",HNQS:"' + hnqs + '",LotteryTime:"' + lTime + '",LotteryNo:"' + lNo + '",ExpiryClosingDate:"' + ecDate +
                    '",SalesVolume:' + sv + ',Progressive:' + pro + ',ContentText:"' + content + '",IsPush:"' + isPush + '"}}',
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
                        setTimeout("window.location = \"tlottery.html\";", 500);
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