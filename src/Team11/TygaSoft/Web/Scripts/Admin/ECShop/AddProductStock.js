
var AddProductStock = {
    GetMyData: function (clientId) {
        var myData = $("#" + clientId + "").html();
        return eval("(" + myData + ")");
    },
    OnProductItemLoadSuccess: function () {
        if ($("#hProductStockId").val() != "") {
            var myDataJson = AddProduct.GetMyData("myDataForModel_ProductStock");
            $.map(myDataJson, function (item) {
                if (item.ProductItemId != "") {
                    $("#cbbProductItem").combobox('select', item.ProductItemId);
                }
            })
        }
        else {
            $("#cbbProductItem").combobox('select', -1);
        }
    },
    OnProductSizeLoadSuccess: function () {
        if ($("#hProductStockId").val() != "") {
            var myDataJson = AddProduct.GetMyData("myDataForModel_ProductStock");
            $.map(myDataJson, function (item) {
                if (item.ProductSize != "") {
                    $("#cbbProductSize").combobox('select', item.ProductSize);
                }
            })
        }
        else {
            $("#cbbProductSize").combobox('select', -1);
        }
    },
    OnCbbProductItemSelect: function (record) {
        if (record.id != -1) {
            var productId = $("#hId").val();
            var productItemId = record.id;
            $("#cbbProductSize").combobox('reload', "/h/t.html?reqName=GetProductSizeJsonForCombobox&productId=" + productId + "&productItemId=" + productItemId + "");
        }
    },
    Add: function () {
        if ($.trim($("#hId").val()) == "") {
            $.messager.alert('错误提示', "请先完成商品基本信息", 'error');
            return false;
        }

        $('#dlgProductStock').dialog({
            title: "新建商品库存",
            closed: false,
            modal: true,
            width: 500,
            height: 300,
            href: '/t/tg.html?productId=' + $("#hId").val() + '',
            buttons: [{
                id: 'btnSaveProductStock', text: '保存', iconCls: 'icon-save',
                handler: function () {
                    AddProductStock.Save();
                }
            }, {
                id: 'btnCancelSaveProductStock', text: '取消', iconCls: 'icon-cancel',
                handler: function () {
                    $('#dlgProductStock').dialog('close');
                }
            }]
        });
    },
    Edit: function () {
        if ($.trim($("#hId").val()) == "") {
            $.messager.alert('错误提示', "请先完成商品基本信息", 'error');
            return false;
        }
        var rows = $('#dgProductStock').datagrid("getSelections");
        if (!(rows && rows.length == 1)) {
            $.messager.alert('错误提醒', '请选择一行且仅一行进行编辑', 'error');
            return false;
        }

        $('#dlgProductStock').dialog({
            title: "编辑商品库存",
            closed: false,
            modal: true,
            width: 500,
            height: 300,
            href: '/t/tg.html?Id=' + rows[0].Id + '&productId=' + rows[0].ProductId + '&productItemId=' + rows[0].ProductItemId + '',
            buttons: [{
                id: 'btnSaveProductStock', text: '保存', iconCls: 'icon-save',
                handler: function () {
                    AddProductStock.Save();
                }
            }, {
                id: 'btnCancelSaveProductStock', text: '取消', iconCls: 'icon-cancel',
                handler: function () {
                    $('#dlgProductStock').dialog('close');
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
            $('#dlgProductStockFm').form('submit', {
                url: '/h/t.html',
                onSubmit: function (param) {
                    var isValid = $(this).form('validate');
                    if (!isValid) {
                        $.messager.progress('close');
                    }
                    param.reqName = "SaveProductStock";
                    param.productId = $("#hId").val();
                    param.productItemId = $("#cbbProductItem").combobox("getValue");
                    param.productSize = $("#cbbProductSize").combobox("getValue");

                    return isValid;
                },
                success: function (data) {
                    $.messager.progress('close');
                    var data = eval('(' + data + ')');
                    if (!data.success) {
                        $.messager.alert('错误提示', data.message, 'error');
                        return false;
                    }
                    $('#dlgProductStock').dialog('close');
                    jeasyuiFun.show("温馨提醒", data.message);
                    setTimeout(function () {
                        $("#dgProductStock").datagrid('load', { reqName: 'GetProductStockJsonForDatagrid', productId: $('#hId').val() })
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
            var rows = $('#dgProductStock').datagrid("getSelections");
            if (!rows || rows.length == 0) {
                $.messager.alert('错误提醒', '请至少选择一行再进行操作', 'error');
                return false;
            }
            var itemAppend = "";
            for (var i = 0; i < rows.length; i++) {
                if (i > 0) itemAppend += ",";
                itemAppend += rows[i].ProductId + "|" + rows[i].ProductItemId + "|" + rows[i].ProductSizeId;
            }
            $.messager.confirm('温馨提醒', '删除后数据将不可恢复，确定要删除吗？', function (r) {
                if (r) {
                    $.ajax({
                        url: "/ScriptServices/AdminService.asmx/DelProductStock",
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
                            $("#dgProductStock").datagrid('load', { reqName: 'GetProductStockJsonForDatagrid', productId: $('#hId').val() })
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