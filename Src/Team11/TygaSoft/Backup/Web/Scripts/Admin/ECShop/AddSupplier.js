

var AddSupplier = {
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

            var provinceCity = $("#lbHProvinceCity").text();
            var address = $.trim($("#txtAddress").val());
            if (address != "") {
                if (provinceCity == "") {
                    $.messager.alert('错误提示', "请选择省市区", 'error');
                }
            }

            $.ajax({
                url: "../../ScriptServices/AdminService.asmx/SaveSupplier",
                type: "post",
                data: '{model:{Id:"' + $("#hId").val() + '",SupplierName:"' + $.trim($("#txtName").val()) + '",Phone:"' + $.trim($("#txtPhone").val()) + '",ProvinceCityName:"' + provinceCity + '",Address:"' + address + '"}}',
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
                        if ($.trim($("#hId").val()) != "") {
                            setTimeout("window.location = \"ytt.html\";", 500);
                        }
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