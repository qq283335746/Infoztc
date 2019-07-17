
var AddProductItem = {
    Add: function () {
        if ($.trim($("#hId").val()) == "") {
            $.messager.alert('错误提示', "请先完成商品基本信息", 'error');
            return false;
        }
        var h = 547;
        if ($(window).height() * 0.9 < h) h = $(window).height() * 0.9;

        $('#dlgProductItem').dialog({
            title: "扩展选项录入信息",
            closed: false,
            modal: true,
            width: 650,
            height: h,
            href: '/t/t.html',
            buttons: [{
                id: 'btnSaveProductItem', text: '保存', iconCls: 'icon-save',
                handler: function () {
                    AddProductItem.Save();
                }
            }, {
                id: 'btnCancelSaveProductItem', text: '取消', iconCls: 'icon-cancel',
                handler: function () {
                    $('#dlgProductItem').dialog('close');
                }
            }]
        });
    },
    Edit: function () {
        if ($.trim($("#hId").val()) == "") {
            $.messager.alert('错误提示', "请先完成商品基本信息", 'error');
            return false;
        }
        var rows = $('#dgProductItem').datagrid("getSelections");
        if (!(rows && rows.length == 1)) {
            $.messager.alert('错误提醒', '请选择一行且仅一行进行编辑', 'error');
            return false;
        }
        var h = 547;
        if ($(window).height() * 0.9 < h) h = $(window).height() * 0.9;
        $('#dlgProductItem').dialog({
            title: "扩展选项录入信息",
            closed: false,
            modal: true,
            width: 650,
            height: h,
            href: '/t/t.html?Id=' + rows[0].Id + '',
            buttons: [{
                id: 'btnSaveProductItem', text: '保存', iconCls: 'icon-save',
                handler: function () {
                    AddProductItem.Save();
                }
            }, {
                id: 'btnCancelSaveProductItem', text: '取消', iconCls: 'icon-cancel',
                handler: function () {
                    $('#dlgProductItem').dialog('close');
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
            $('#dlgProductItemFm').form('submit', {
                url: '/h/t.html',
                onSubmit: function (param) {
                    var isValid = $(this).form('validate');
                    if (!isValid) {
                        $.messager.progress('close');
                    }
                    param.reqName = "SaveProductItem";
                    param.productId = $("#hId").val();
                    param.isEnable = $("input[name=rbtnIsEnableList_ProductItem]:checked").val();
                    param.isDisable = $("input[name=rbtnList_ProductItem]:checked").val();

                    return isValid;
                },
                success: function (data) {
                    $.messager.progress('close');
                    var data = eval('(' + data + ')');
                    if (!data.success) {
                        $.messager.alert('错误提示', data.message, 'error');
                        return false;
                    }
                    $('#dlgProductItem').dialog('close');
                    jeasyuiFun.show("温馨提醒", data.message);
                    setTimeout(function () {
                        $("#dgProductItem").datagrid('load', { reqName: 'GetProductItemJsonForDatagrid', productId: $('#hId').val() })
                    }, 500);
                }
            });
        }
        catch (e) {
            $.messager.progress('close');
            $.messager.alert('错误提醒', e.name + ": " + e.message, 'error');
        }
    },
    Del:function(){
        try {
            var rows = $('#dgProductItem').datagrid("getSelections");
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
                        url: "/ScriptServices/AdminService.asmx/DelProductItem",
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
                            $("#dgProductItem").datagrid('load', { reqName: 'GetProductItemJsonForDatagrid', productId: $('#hId').val() })
                        }
                    });
                }
            });
        }
        catch (e) {
            $.messager.alert('错误提醒', e.name + ": " + e.message, 'error');
        }
    },
    DlgSingle: function () {
        $("#hDlgOpenId").val("dlgSingleSelectProductItemPicture");
        $("#hIsMutil").val("false");
        $('#dlgSingleSelectProductItemPicture').dialog('open');
    },
    SetSingleProductPic: function (imgEleId) {
        var data = dlgSingleSelectProductItemPicture.GetPicSelect();
        if (data.length > 0) {
            var arr = data[0].split(",");
            var imgPicture = $("#" + imgEleId + "");
            imgPicture.attr("src", arr[1])
            imgPicture.parent().find("input[type=hidden]").val(arr[0]);
            $("#dlgSingleSelectProductItemPicture").dialog('close');
        }
    },
    DlgUpload: function () {
        var dlgParentId = $("#hDlgOpenId").val();
        var isMutil = $("#hIsMutil").val();
        var h = $(window).height() * 0.9;
        $("#dlgUploadProductPicture").dialog({
            title: '上传文件',
            width: 606,
            height: h,
            closed: false,
            href: '/a/yyg.html?dlgId=dlgUploadProductPicture&funName=ProductPicture&isMutil=' + isMutil + '&dlgParentId=' + dlgParentId + '&submitUrl=/a/ygt.html',
            modal: true,
            buttons: [{
                id: 'btnOnUploadProductPicture', text: '上 传', iconCls: 'icon-ok',
                handler: function () {
                    dlgUploadProductPicture.OnUpload();
                }
            }, {
                id: 'btnCancelUploadProductPicture', text: '取 消', iconCls: 'icon-cancel',
                handler: function () {
                    $("#dlgUploadProductPicture").dialog('close');
                }
            }],
            toolbar: [{
                id: 'btnAddTextbox', text: '添 加', iconCls: 'icon-add',
                handler: function () {
                    dlgUploadProductPicture.OnToolbarAdd();
                }
            }]
        })
    }
}