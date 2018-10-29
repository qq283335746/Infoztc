
var AddProductSize = {
    Init: function () {
        $("#dynamicSizeT>tbody>tr").eq(0).find("a[code=del]").hide();
    },
    Add: function () {
        if ($.trim($("#hId").val()) == "") {
            $.messager.alert('错误提示', "请先完成商品基本信息", 'error');
            return false;
        }

        $('#dlgProductSize').dialog({
            title: "扩展选项录入信息",
            closed: false,
            modal: true,
            width: 660,
            height: 300,
            href: '/t/ty.html?action=add&productId=' + $("#hId").val() + '',
            buttons: [{
                id: 'btnSaveProductSize', text: '保存', iconCls: 'icon-save',
                handler: function () {
                    AddProductSize.Save();
                }
            }, {
                id: 'btnCancelSaveProductSize', text: '取消', iconCls: 'icon-cancel',
                handler: function () {
                    $("#dlgProductSize").dialog('close');
                }
            }]
        });
    },
    Edit: function () {
        if ($.trim($("#hId").val()) == "") {
            $.messager.alert('错误提示', "请先完成商品基本信息", 'error');
            return false;
        }
        var rows = $('#dgProductSize').datagrid("getSelections");
        if (!(rows && rows.length == 1)) {
            $.messager.alert('错误提醒', '请选择一行且仅一行进行编辑', 'error');
            return false;
        }

        $('#dlgProductSize').dialog({
            title: "扩展选项录入信息",
            closed: false,
            modal: true,
            width: 660,
            height: 300,
            href: '/t/ty.html?action=edit&productId=' + rows[0].ProductId + '&productItemId=' + rows[0].ProductItemId + '',
            buttons: [{
                id: 'btnSaveProductSize', text: '保存', iconCls: 'icon-save',
                handler: function () {
                    AddProductSize.Save();
                }
            }, {
                id: 'btnCancelSaveProductSize', text: '取消', iconCls: 'icon-cancel',
                handler: function () {
                    $('#dlgProductSize').dialog('close');
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
            $('#dlgProductSizeFm').form('submit', {
                url: '/h/t.html',
                onSubmit: function (param) {
                    var isValid = $(this).form('validate');
                    if (!isValid) {
                        $.messager.progress('close');
                    }
                    param.reqName = "SaveProductSize";
                    param.productId = $("#hId").val();
                    param.productItemId = $("#ddlProductItem_ProductSize").val();
                    var xml = "";
                    $("#dynamicSizeT>tbody>tr").each(function () {
                        var tds = $(this).find("td");

                        xml += "<Data><Name>" + $.trim(tds.eq(0).find("[type=text]").val()) + "</Name></Data>";
                    })
                    if (xml != "") xml = "<Datas>" + xml + "</Datas>";
                    param.sizeAppend = xml.replace(/</g, "&lt;");

                    return isValid;
                },
                success: function (data) {
                    $.messager.progress('close');
                    var data = eval('(' + data + ')');
                    if (!data.success) {
                        $.messager.alert('错误提示', data.message, 'error');
                        return false;
                    }
                    $('#dlgProductSize').dialog('close');
                    jeasyuiFun.show("温馨提醒", data.message);
                    setTimeout(function () {
                        $("#dgProductSize").datagrid('load', { reqName: 'GetProductSizeJsonForDatagrid', productId: $('#hId').val() })
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
            var rows = $('#dgProductSize').datagrid("getSelections");
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
                        url: "/ScriptServices/AdminService.asmx/DelProductSize",
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
                            $("#dgProductSize").datagrid('load', { reqName: 'GetProductSizeJsonForDatagrid', productId: $('#hId').val() })
                        }
                    });
                }
            });
        }
        catch (e) {
            $.messager.alert('错误提醒', e.name + ": " + e.message, 'error');
        }
    },
    AddInput: function () {
        var s = "<tr class=\"pdt10\">";
        s += "<td>尺码：<input type=\"text\" class=\"txt\" /></td>";
        s += " <td class=\"pdl10\">";
        s += "<a href=\"javascript:void(0)\" onclick=\"AddProductSize.UpInput(this)\" class=\"mr10\">上移</a>";
        s += "<a href=\"javascript:void(0)\" onclick=\"AddProductSize.DownInput(this)\" class=\"mr10\">下移</a>";
        s += "<a code=\"del\" href=\"javascript:void(0)\" onclick=\"$(this).parent().parent().remove()\" class=\"mr10\">删除</a>";
        s += "</td>";
        s += " </tr>";

        $("#dynamicSizeT").append(s);
    },
    UpInput: function (t) {
        var $_curr = $(t).parent().parent();
        var $_prev = $_curr.prev();
        $_curr.after($_prev);
    },
    DownInput: function (t) {
        var $_curr = $(t).parent().parent();
        var $_next = $_curr.next();
        $_curr.before($_next);
    }
}