

var AddHwtv = {
    Init: function () {
        AddHwtv.SelectType();
    },
    IsValid: function () {
        var img = $.trim($('#hdimg').val());
        if (img == '')
        {
            alert("请选择图标！");
            return false;
        }
        AddHwtv.OnSave();
    },
    GetMyData: function (clientId) {
        var myData = $("#" + clientId + "").html();
        return eval("(" + myData + ")");
    },
    SelectType : function(){
        $('#selectType').combobox({
            editable: false,
            panelWidth: 50,
            width: 50,
            panelHeight: 50,
            onSelect: function (record) {
                var obj = $('#selectType').combobox('getValue');
                if (obj == 'True') {
                    $("#divProgramURL").show();
                }
                else {
                    $("#divProgramURL").hide();
                }
            },
            onLoadSuccess: function () {
                if ($("#hTurnTo").val() != "") {
                    $("#selectType").combobox('select', $("#hTurnTo").val());
                }
                else {
                    $("#selectType").combobox('select', 'False');
                }
            }
        });

        var objval = $('#selectType').combobox('getValue');
        if (objval == "True") {
            $("#divProgramURL").show();
        }
    },
    OnSave: function () {
        try {
            var isValid = $('#form1').form('validate');
            if (!isValid) return false;

            var id = $("#hId").val() == "" ? "00000000-0000-0000-0000-000000000000" : $("#hId").val();
            var name = $.trim($("#txtName").val());
            var ProgramURL = $.trim($("#txtProgramURL").val());
            var isTurnTo = $("#selectType").combobox("getValue");
            var isDisable = $('input[type="radio"]:checked').val();
            var Vsort = $.trim($('#txtSort').val());
            var address = $.trim($('#txtProgramURL').val());
            var img = $.trim($('#hdimg').val());
            $.ajax({
                url: "../../ScriptServices/AdminService.asmx/SaveHWTV",
                type: "post",
                data: '{model:{Id:"' + id + '",HWTVName:"' + name + '",IsDisable:"' + isDisable + '",IsTurnTo:"' + isTurnTo + '",Sort:"' + Vsort + '",ProgramAddress:"' + address + '",HWTVIcon:"' + img + '"}}',
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
                        setTimeout("window.location = \"ttv.html\";", 500);
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