
var AddAdBase = {
    Init: function () {

    },
    OnCbbSiteFunLoadSuccess: function () {
        if ($.trim($("#hAdId").val()) != "") {
            var myJsonData = eval("(" + $.trim($("#myDataForModel").html()) + ")");
            $("#cbbSiteFun").combobox('select', myJsonData.SiteFunId);
        }
    },
    OnCbbLayoutPositionSuccess: function () {
        if ($.trim($("#hAdId").val()) != "") {
            var myJsonData = eval("(" + $.trim($("#myDataForModel").html()) + ")");
            $("#cbbLayoutPosition").combobox('select', myJsonData.LayoutPositionId);
        }
    },
    OnCbbIsDisableSuccess: function () {
        if ($.trim($("#hAdId").val()) != "") {
            var myJsonData = eval("(" + $.trim($("#myDataForModel").html()) + ")");
            $("#cbbIsDisable").combobox('select', myJsonData.IsDisable);
        }
    },
    OnSave: function () {
        try {
            $.messager.progress({title: '请稍等', msg: '正在执行...'});
            $('#form1').form('submit', {
                url: '/h/taboutsite.html',
                onSubmit: function (param) {

                    var isValid = $(this).form('validate');
                    if (!isValid) {
                        $.messager.progress('close');
                    }

                    param.reqName = "SaveAdBase";
                    param.siteFunId = $("#cbbSiteFun").combobox('getValue');
                    param.layoutPositionId = $("#cbbLayoutPosition").combobox('getValue');
                    param.isDisable = $("#cbbIsDisable").combobox('getValue');

                    return isValid;
                },
                success: function (data) {
                    $.messager.progress('close');
                    var jsonData = eval('(' + data + ')');
                    if (!jsonData.success) {
                        $.messager.alert('错误提示', jsonData.message, 'error');
                        return false;
                    }
                    $("#hAdId").val(jsonData.data);
                    jeasyuiFun.show("温馨提醒", jsonData.message);
                }
            });
        }
        catch (e) {
            $.messager.progress('close');
            $.messager.alert('错误提醒', e.name + ": " + e.message, 'error');
        }
    }
}