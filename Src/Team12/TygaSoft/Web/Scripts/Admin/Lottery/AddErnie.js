
var AddErnie = {
    Save: function () {
        try {

            var isValid = $('#form1').form('validate');
            if (!isValid) return false;

            $.messager.progress({
                title: '请稍等',
                msg: '正在执行...'
            });

            var postData = { reqName: 'SaveErnie', Id: $.trim($("#hErnieId").val()), startTime: $('#txtStartTime').datetimebox('getValue')
                , endTime: $('#txtEndTime').datetimebox('getValue'), isDisable: $("input[name=rdIsDisable]:checked").val()
                , userBetMaxCount: $.trim($("#txtUserBetMaxCount").val())
            }

            $.post("/h/ta.html", postData, function (data) {
                $.messager.progress('close');
                if (!data.success) {
                    $.messager.alert('错误提示', data.message, 'error');
                    return false;
                }
                jeasyuiFun.show("温馨提示", "保存成功！");
                $("#hErnieId").val(data.message);

            }, "json")
        }
        catch (e) {
            $.messager.progress('close');
            $.messager.alert('错误提醒', e.name + ": " + e.message, 'error');
        }
    }
}