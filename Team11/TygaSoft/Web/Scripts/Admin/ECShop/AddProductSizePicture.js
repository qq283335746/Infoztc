
var AddProductSizePicture = {
    
    Add: function () {
        if ($.trim($("#hId").val()) == "") {
            $.messager.alert('错误提示', "请先完成商品基本信息", 'error');
            return false;
        }

        $('#dlgProductSizePicture').dialog({
            title: "新增尺码信息",
            closed: false,
            modal: true,
            width: 500,
            height: 400,
            href: '/t/tt.html?action=add&productId=' + $("#hId").val() + '',
            buttons: [{
                id: 'btnSaveProductSizePicture', text: '保存', iconCls: 'icon-save',
                handler: function () {
                    AddProductSizePicture.Save();
                }
            }, {
                id: 'btnCancelSaveProductSizePicture', text: '取消', iconCls: 'icon-cancel',
                handler: function () {
                    $('#dlgProductSizePicture').dialog('close');
                }
            }]
        });
    },
    Edit: function () {
        if ($.trim($("#hId").val()) == "") {
            $.messager.alert('错误提示', "请先完成商品基本信息", 'error');
            return false;
        }
        var rows = $('#dgProductSizePicture').datagrid("getSelections");
        if (!(rows && rows.length == 1)) {
            $.messager.alert('错误提醒', '请选择一行且仅一行进行编辑', 'error');
            return false;
        }

        $('#dlgProductSizePicture').dialog({
            title: "编辑尺码信息",
            closed: false,
            modal: true,
            width: 500,
            height: 400,
            href: '/t/tt.html?action=edit&productId=' + rows[0].ProductId + '&productItemId=' + rows[0].ProductItemId + '',
            buttons: [{
                id: 'btnSaveProductSizePicture', text: '保存', iconCls: 'icon-save',
                handler: function () {
                    AddProductSizePicture.Save();
                }
            }, {
                id: 'btnCancelSaveProductSizePicture', text: '取消', iconCls: 'icon-cancel',
                handler: function () {
                    $('#dlgProductSizePicture').dialog('close');
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
            $('#dlgProductSizePictureFm').form('submit', {
                url: '/h/t.html',
                onSubmit: function (param) {
                    var isValid = $(this).form('validate');
                    if (!isValid) {
                        $.messager.progress('close');
                    }
                    param.reqName = "SaveProductSizePicture";
                    param.productId = $("#hId").val();
                    param.productItemId = $("#ddlProductItem_ProductSizePicture").val();

                    return isValid;
                },
                success: function (data) {
                    $.messager.progress('close');
                    var data = eval('(' + data + ')');
                    if (!data.success) {
                        $.messager.alert('错误提示', data.message, 'error');
                        return false;
                    }
                    $('#dlgProductSizePicture').dialog('close');
                    jeasyuiFun.show("温馨提醒", data.message);
                    setTimeout(function () {
                        $("#dgProductSizePicture").datagrid('load', { reqName: 'GetProductSizePictureJsonForDatagrid', productId: $('#hId').val() })
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
            var rows = $('#dgProductSizePicture').datagrid("getSelections");
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
                        url: "/ScriptServices/AdminService.asmx/DelProductSizePicture",
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
                            $("#dgProductSizePicture").datagrid('load', { reqName: 'GetProductSizePictureJsonForDatagrid', productId: $('#hId').val() })
                        }
                    });
                }
            });
        }
        catch (e) {
            $.messager.alert('错误提醒', e.name + ": " + e.message, 'error');
        }
    },
    OnPictureClick: function () {
        DlgPictureSelect.DlgOpenId = "dlgSingleSelectProductSizePicture";
        DlgPictureSelect.DlgSingle('PictureProductSize');
    },
    SetSinglePicture: function (imgEleId) {
        var data = dlgSingleSelectProductSizePicture.GetPicSelect();
        if (data.length > 0) {
            var arr = data[0].split(",");
            $("#" + imgEleId + "").attr("src", arr[1])
            $("#" + imgEleId + "").parent().find("input[type=hidden]").val(arr[0]);
            $("#dlgSingleSelectProductSizePicture").dialog('close');
        }
    },
    SetMutilPicture: function (imgEleId) {
        var data = dlgMutilSelectProductSizePicture.GetPicSelect();
        if (data.length > 0) {
            var imgEle = $("#" + imgEleId + "");
            var firstCol = imgEle.children().eq(0);
            for (var i = 0; i < data.length; i++) {
                var arr = data[i].split(",");
                var hasExist = false;
                imgEle.find("input[type=hidden]").each(function () {
                    if ($(this).val() == arr[0]) {
                        hasExist = true;
                        return false;
                    }
                })
                if (!hasExist) {
                    var newCol = firstCol.clone(true);
                    newCol.appendTo(imgEle);
                    newCol.find("img").attr("src", arr[1]);
                    newCol.find("input[type=hidden]").val(arr[0]);
                    newCol.show();
                }
            }
        }
    }
}