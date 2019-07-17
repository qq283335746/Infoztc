
var AddProductImage = {
    Init: function () {
        $("#dynamicImageT>tbody>tr").eq(0).find("a[code=del]").hide();
    },
    Add: function () {
        if ($.trim($("#hId").val()) == "") {
            $.messager.alert('错误提示', "请先完成商品基本信息", 'error');
            return false;
        }

        $('#dlgProductImage').dialog({
            title: "商品其它图片",
            closed: false,
            modal: true,
            width: 780,
            height: $(window).height() * 0.8,
            href: '/t/ta.html?action=add&productId=' + $("#hId").val() + '',
            buttons: [{
                id: 'btnSaveProductImage', text: '保存', iconCls: 'icon-save',
                handler: function () {
                    AddProductImage.Save();
                }
            }, {
                id: 'btnCancelSaveProductImage', text: '取消', iconCls: 'icon-cancel',
                handler: function () {
                    $("#dlgProductImage").dialog('close');
                }
            }]
        });
    },
    Edit: function () {
        if ($.trim($("#hId").val()) == "") {
            $.messager.alert('错误提示', "请先完成商品基本信息", 'error');
            return false;
        }
        var rows = $('#dgProductImage').datagrid("getSelections");
        if (!(rows && rows.length == 1)) {
            $.messager.alert('错误提醒', '请选择一行且仅一行进行编辑', 'error');
            return false;
        }

        $('#dlgProductImage').dialog({
            title: "扩展选项录入信息",
            closed: false,
            modal: true,
            width: 780,
            height: $(window).height() * 0.8,
            href: '/t/ta.html?action=edit&productId=' + rows[0].ProductId + '&productItemId=' + rows[0].ProductItemId + '&Id=' + rows[0].Id + '',
            buttons: [{
                id: 'btnSaveProductImage', text: '保存', iconCls: 'icon-save',
                handler: function () {
                    AddProductImage.Save();
                }
            }, {
                id: 'btnCancelSaveProductImage', text: '取消', iconCls: 'icon-cancel',
                handler: function () {
                    $('#dlgProductImage').dialog('close');
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
            $('#dlgProductImageFm').form('submit', {
                url: '/h/t.html',
                onSubmit: function (param) {
                    var isValid = $(this).form('validate');
                    if (!isValid) {
                        $.messager.progress('close');
                    }
                    param.reqName = "SaveProductImage";
                    param.productId = $("#hId").val();
                    param.productItemId = $("#ddlProductItem_ProductImage").val();

                    var xml = "";
                    var index = 0;
                    $("#dynamicImageT>tbody>tr").each(function () {
                        var pictureId = $(this).find("img").parent().find("input[type=hidden]").val();
                        if (pictureId != "") {
                            xml += "<Add PictureId=\"" + pictureId + "\" Sort=\"" + index + "\"></Add>";
                            index++;
                        }
                    })

                    if (xml == "") {
                        $.messager.alert('错误提示', "请选择图片", 'error');
                        return false;
                    }
                    xml = "<Imgs>" + xml + "</Imgs>";
                    param.pictureAppend = xml.replace(/</g, "&lt;");

                    return isValid;
                },
                success: function (data) {
                    $.messager.progress('close');
                    var data = eval('(' + data + ')');
                    if (!data.success) {
                        $.messager.alert('错误提示', data.message, 'error');
                        return false;
                    }
                    $('#dlgProductImage').dialog('close');
                    jeasyuiFun.show("温馨提醒", data.message);
                    setTimeout(function () {
                        $("#dgProductImage").datagrid('load', { reqName: 'GetProductImageJsonForDatagrid', productId: $('#hId').val() })
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
            var rows = $('#dgProductImage').datagrid("getSelections");
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
                        url: "/ScriptServices/AdminService.asmx/DelProductImage",
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
                            $("#dgProductImage").datagrid('load', { reqName: 'GetProductImageJsonForDatagrid', productId: $('#hId').val() })
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
        s += "<td><img src='../../Images/nopic.gif' alt=\"上传图片\" width=\"100\" height=\"100\" onclick=\"AddProductImage.OnPictureClick(this)\" /><input type=\"hidden\" value='' /></td>";
        s += " <td class=\"pdl10\">";
        s += "<a href=\"javascript:void(0)\" onclick=\"AddProductAttr.UpInput(this)\" class=\"mr10\">上移</a>";
        s += "<a href=\"javascript:void(0)\" onclick=\"AddProductAttr.DownInput(this)\" class=\"mr10\">下移</a>";
        s += "<a code=\"del\" href=\"javascript:void(0)\" onclick=\"$(this).parent().parent().remove()\" class=\"mr10\">删除</a>";
        s += "</td>";
        s += " </tr>";

        $("#dynamicImageT").append(s);
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
    },
    CurrRow: 0,
    OnPictureClick: function (t) {
        this.CurrRow = $(t).parent().parent().index();
        DlgPictureSelect.DlgOpenId = "dlgSingleSelectProductImagePicture";
        DlgPictureSelect.DlgSingle('PictureProduct');
    },
    SetSinglePicture: function () {
        var data = dlgSingleSelectProductImagePicture.GetPicSelect();
        if (data.length > 0) {
            var arr = data[0].split(",");

            var img = $("#dynamicImageT>tbody>tr").eq(AddProductImage.CurrRow).find("img");
            img.attr("src", arr[1]);
            img.parent().find("input[type=hidden]").val(arr[0]);

            $("#dlgSingleSelectProductImagePicture").dialog('close');
        }
    }
}