
var ListErnieItem = {
    Add: function () {
        if ($.trim($("#hErnieId").val()) == "") {
            $.messager.alert('错误提示', "请先完成基本信息再执行此操作！", 'error');
            return false;
        }

        $('#dlgErnieItem').dialog({
            title: "新建参数项",
            href: '/t/ya.html?action=add',
            buttons: [{
                id: 'btnSaveErnieItem', text: '保存', iconCls: 'icon-save', handler: function () {
                    ListErnieItem.Save();
                }
            }, {
                id: 'btnCancelSaveErnieItem', text: '取消', iconCls: 'icon-cancel', handler: function () {
                    $('#dlgErnieItem').dialog('close');
                }
            }]
        });
    },
    Edit: function () {
        if ($.trim($("#hErnieId").val()) == "") {
            $.messager.alert('错误提示', "请先完成基本信息再执行此操作！", 'error');
            return false;
        }
        var rows = $('#dgErnieItem').datagrid("getSelections");
        if (!(rows && rows.length == 1)) {
            $.messager.alert('错误提醒', '请选择一行且仅一行进行编辑', 'error');
            return false;
        }
        $('#dlgErnieItem').dialog({
            title: "新建参数项",
            href: '/t/ya.html?action=add&Id=' + rows[0].Id + '',
            buttons: [{
                id: 'btnSaveErnieItem', text: '保存', iconCls: 'icon-save', handler: function () {
                    ListErnieItem.Save();
                }
            }, {
                id: 'btnCancelSaveErnieItem', text: '取消', iconCls: 'icon-cancel', handler: function () {
                    $('#dlgErnieItem').dialog('close');
                }
            }]
        });
    },
    Save: function () {
        try {

            var isValid = $('#dlgErnieItemFm').form('validate');
            if (!isValid) return false;

            $.messager.progress({
                title: '请稍等',
                msg: '正在执行...'
            });

            var numAppend = "";
            $("#cbListNum").find("[type=checkbox]").filter(":checked").each(function () {
                numAppend += $(this).val() + ",";
            })

            var postData = { reqName: 'SaveErnieItem', Id: $.trim($("#hErnieItemId").val()), ErnieId: $.trim($("#hErnieId").val()), NumType: $("#ddlNumType").val(), Num: numAppend,
                AppearRatio: $.trim($("#txtAppearRatio").val())
            }

            $.post("/h/ta.html", postData, function (data) {
                $.messager.progress('close');
                if (!data.success) {
                    $.messager.alert('错误提示', data.message, 'error');
                    return false;
                }
                $('#dlgErnieItem').dialog('close');
                jeasyuiFun.show("温馨提示", "保存成功！");
                setTimeout(function () {
                    $("#dgErnieItem").datagrid('load', { reqName: 'GetErnieItemForDatagrid', ernieId: $('#hErnieId').val() })
                }, 100);

            }, "json")
        }
        catch (e) {
            $.messager.progress('close');
            $.messager.alert('错误提醒', e.name + ": " + e.message, 'error');
        }
    },
    Del: function () {
        try {
            var rows = $('#dgErnieItem').datagrid("getSelections");
            if (!rows || rows.length == 0) {
                $.messager.alert('错误提醒', '请至少选择一行再进行操作', 'error');
                return false;
            }
            var itemAppend = "";
            for (var i = 0; i < rows.length; i++) {
                if (i > 0) itemAppend += ",";
                itemAppend += rows[i].Id;
            }
            $.messager.confirm('温馨提醒', '确定要删除吗？', function (r) {
                if (r) {
                    $.ajax({
                        url: "/ScriptServices/AdminService.asmx/DelErnieItem",
                        type: "post",
                        contentType: "application/json; charset=utf-8",
                        data: '{itemAppend:"' + itemAppend + '"}',
                        beforeSend: function () {
                            $("#dlgWaiting").dialog('open');
                        },
                        complete: function () {
                            $("#dlgWaiting").dialog('close');
                        },
                        success: function (data) {
                            var msg = data.d;
                            if (msg != "1") {
                                $.messager.alert('系统提示', msg, 'info');
                                return false;
                            }
                            setTimeout(function () {
                                $("#dgErnieItem").datagrid('load', { reqName: 'GetErnieItemForDatagrid', ernieId: $('#hErnieId').val() })
                            }, 100);
                        }
                    });
                }
            });
        }
        catch (e) {
            $.messager.alert('错误提醒', e.name + ": " + e.message, 'error');
        }
    },
    Copy: function () {
        var ernieId = $.trim($("#hErnieId").val());
        if (ernieId == "") {
            $.messager.alert('错误提示', "请先完成基本信息再执行此操作！", 'error');
            return false;
        }
        $.messager.confirm('温馨提醒', '此操作将会复制上一期的参数设定数据，并移除当前期的参数，确定要复制吗？', function (r) {
            if (r) {
                $.ajax({
                    url: "/ScriptServices/AdminService.asmx/CopyErnieItemByLasted",
                    type: "post",
                    contentType: "application/json; charset=utf-8",
                    data: '{ernieId:"' + ernieId + '"}',
                    beforeSend: function () {
                        $("#dlgWaiting").dialog('open');
                    },
                    complete: function () {
                        $("#dlgWaiting").dialog('close');
                    },
                    success: function (data) {
                        var msg = data.d;
                        if (msg != "1") {
                            $.messager.alert('系统提示', msg, 'info');
                            return false;
                        }
                        setTimeout(function () {
                            $("#dgErnieItem").datagrid('load', { reqName: 'GetErnieItemForDatagrid', ernieId: ernieId })
                        }, 100);
                    }
                });
            }
        });
    }
}