

var AddProductDetail = {
    Add: function () {
        if ($.trim($("#hId").val()) == "") {
            $.messager.alert('错误提示', "请先完成商品基本信息", 'error');
            return false;
        }
        var w = 1200;
        if ($(window).width() * 0.9 < w) w = $(window).width() * 0.9;

        $('#dlgProductDetail').dialog({
            title: "商品详情信息",
            closed: false,
            modal: true,
            width: w,
            height: $(window).height() * 0.9,
            href: '/t/a.html?action=add&productId=' + $("#hId").val() + '',
            buttons: [{
                id: 'btnSaveProductDetail', text: '保存', iconCls: 'icon-save',
                handler: function () {
                    AddProductDetail.Save();
                }
            }, {
                id: 'btnCancelSaveProductDetail', text: '取消', iconCls: 'icon-cancel',
                handler: function () {
                    $('#dlgProductDetail').dialog('close');
                }
            }]
        });
    },
    Edit: function () {
        if ($.trim($("#hId").val()) == "") {
            $.messager.alert('错误提示', "请先完成商品基本信息", 'error');
            return false;
        }
        var rows = $('#dgProductDetail').datagrid("getSelections");
        if (!(rows && rows.length == 1)) {
            $.messager.alert('错误提醒', '请选择一行且仅一行进行编辑', 'error');
            return false;
        }
        var w = 1200;
        if ($(window).width() * 0.9 < w) w = $(window).width() * 0.9;

        $('#dlgProductDetail').dialog({
            title: "扩展选项录入信息",
            closed: false,
            modal: true,
            width: w,
            height: $(window).height() * 0.9,
            href: '/t/a.html?action=edit&productId=' + rows[0].ProductId + '&productItemId=' + rows[0].ProductItemId + '',
            buttons: [{
                id: 'btnSaveProductDetail', text: '保存', iconCls: 'icon-save',
                handler: function () {
                    AddProductDetail.Save();
                }
            }, {
                id: 'btnCancelSaveProductDetail', text: '取消', iconCls: 'icon-cancel',
                handler: function () {
                    $('#dlgProductDetail').dialog('close');
                }
            }]
        });
    },
    Save: function () {
        try {
            $.messager.progress({
                title: '请稍等',
                msg: '正在执行...'
            });
            $('#dlgProductDetailFm').form('submit', {
                url: '/h/t.html',
                onSubmit: function (param) {
                    var isValid = $(this).form('validate');
                    if (!isValid) {
                        $.messager.progress('close');
                    }
                    param.reqName = "SaveProductDetail";
                    param.productId = $("#hId").val();
                    param.productItemId = $("#ddlProductItem").val();
                    param.content = editor_content.html().replace(/</g, "&lt;");
                    if ($.trim($("#hAction").val()) == "edit") {
                        $("#txtaContent").val("");
                    }

                    return isValid;
                },
                success: function (data) {
                    $.messager.progress('close');
                    var data = eval('(' + data + ')');
                    if (!data.success) {
                        $.messager.alert('错误提示', data.message, 'error');
                        return false;
                    }
                    $('#dlgProductDetail').dialog('close');
                    jeasyuiFun.show("温馨提醒", data.message);
                    setTimeout(function () {
                        $("#dgProductDetail").datagrid('load', { reqName: 'GetProductDetailJsonForDatagrid', productId: $('#hId').val() })
                    }, 500);
                }
            });
        }
        catch (e) {
            $.messager.progress('close');
            $.messager.alert('错误提醒', e.name + ": " + e.message, 'error');
        }
    },
    Del: function () {
        try {
            var rows = $('#dgProductDetail').datagrid("getSelections");
            if (!rows || rows.length == 0) {
                $.messager.alert('错误提醒', '请至少选择一行再进行操作', 'error');
                return false;
            }
            var itemAppend = "";
            for (var i = 0; i < rows.length; i++) {
                if (i > 0) itemAppend += ",";
                itemAppend += rows[i].ProductId + "|" + rows[i].ProductItemId;
            }
            $.messager.confirm('温馨提醒', '删除后数据将不可恢复，确定要删除吗？', function (r) {
                if (r) {
                    $.ajax({
                        url: "/ScriptServices/AdminService.asmx/DelProductDetail",
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
                            jeasyuiFun.show("温馨提醒", "操作成功");
                            $("#dgProductDetail").datagrid('load', { reqName: 'GetProductDetailJsonForDatagrid', productId: $('#hId').val() })
                        }
                    });
                }
            });
        }
        catch (e) {
            $.messager.alert('错误提醒', e.name + ": " + e.message, 'error');
        }
    }
}