
var AddProductAttr = {
    Init:function(){
        $("#dynamicAttrT>tbody>tr").eq(0).find("a[code=del]").hide();
    },
    Add: function () {
        if ($.trim($("#hId").val()) == "") {
            $.messager.alert('错误提示', "请先完成商品基本信息", 'error');
            return false;
        }
        //var h = 547;
        //if ($(window).height() * 0.9 < h) h = $(window).height() * 0.9;

        $('#dlgProductAttr').dialog({
            title: "自定义属性",
            closed: false,
            modal: true,
            width: 820,
            height: $(window).height() * 0.9,
            href: '/t/g.html?action=add&productId=' + $("#hId").val() + '',
            buttons: [{
                id: 'btnSaveProductAttr', text: '保存', iconCls: 'icon-save',
                handler: function () {
                    AddProductAttr.Save();
                }
            }, {
                id: 'btnCancelSaveProductAttr', text: '取消', iconCls: 'icon-cancel',
                handler: function () {
                    $("#dlgProductAttr").dialog('close');
                }
            }]
        });
    },
    Edit: function () {
        if ($.trim($("#hId").val()) == "") {
            $.messager.alert('错误提示', "请先完成商品基本信息", 'error');
            return false;
        }
        var rows = $('#dgProductAttr').datagrid("getSelections");
        if (!(rows && rows.length == 1)) {
            $.messager.alert('错误提醒', '请选择一行且仅一行进行编辑', 'error');
            return false;
        }
        //var h = 547;
        //if ($(window).height() * 0.9 < h) h = $(window).height() * 0.9;
        $('#dlgProductAttr').dialog({
            title: "自定义属性",
            closed: false,
            modal: true,
            width: 820,
            height: $(window).height() * 0.9,
            href: '/t/g.html?action=edit&productId=' + rows[0].ProductId + '&productItemId=' + rows[0].ProductItemId + '&Id=' + rows[0].Id + '',
            buttons: [{
                id: 'btnSaveProductAttr', text: '保存', iconCls: 'icon-save',
                handler: function () {
                    AddProductAttr.Save();
                }
            }, {
                id: 'btnCancelSaveProductAttr', text: '取消', iconCls: 'icon-cancel',
                handler: function () {
                    $('#dlgProductAttr').dialog('close');
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
            $('#dlgProductAttrFm').form('submit', {
                url: '/h/t.html',
                onSubmit: function (param) {
                    var isValid = $(this).form('validate');
                    if (!isValid) {
                        $.messager.progress('close');
                    }
                    param.reqName = "SaveProductAttr";
                    param.productId = $("#hId").val();
                    param.productItemId = $("#ddlProductItem").val();
                    var xml = "";
                    $("#dynamicAttrT>tbody>tr").each(function () {
                        var tds = $(this).find("td");

                        xml += "<Attr><Name>" + $.trim(tds.eq(0).find("[type=text]").val()) + "</Name><Value>" + $.trim(tds.eq(1).find("[type=text]").val()) + "</Value></Attr>";
                    })
                    if (xml != "") xml = "<Attrs>" + xml + "</Attrs>";
                    param.attrValue = xml.replace(/</g, "&lt;");

                    return isValid;
                },
                success: function (data) {
                    $.messager.progress('close');
                    var data = eval('(' + data + ')');
                    if (!data.success) {
                        $.messager.alert('错误提示', data.message, 'error');
                        return false;
                    }
                    $('#dlgProductAttr').dialog('close');
                    jeasyuiFun.show("温馨提醒", data.message);
                    setTimeout(function () {
                        $("#dgProductAttr").datagrid('load', { reqName: 'GetProductAttrJsonForDatagrid', productId: $('#hId').val() })
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
            var rows = $('#dgProductAttr').datagrid("getSelections");
            if (!rows || rows.length == 0) {
                $.messager.alert('错误提醒', '请至少选择一行再进行操作', 'error');
                return false;
            }
            var itemAppend = "";
            for (var i = 0; i < rows.length; i++) {
                if (i > 0) itemAppend += ",";
                itemAppend += rows[i].ProductItemId;
            }
            $.messager.confirm('温馨提醒', '确定要删除吗？', function (r) {
                if (r) {
                    $.ajax({
                        url: "/ScriptServices/AdminService.asmx/DelProductAttr",
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
                            $("#dgProductAttr").datagrid('load', { reqName: 'GetProductAttrJsonForDatagrid', productId: $('#hId').val() })
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
        s += "<td><input type=\"text\" name=\"attrItem\" class=\"txt\" /></td><td class=\"pdl10\"><input type=\"text\" name=\"attrItem\" class=\"txt\" /></td>";
        s += " <td class=\"pdl10\">";
        s += "<a href=\"javascript:void(0)\" onclick=\"AddProductAttr.UpInput(this)\" class=\"mr10\">上移</a>";
        s += "<a href=\"javascript:void(0)\" onclick=\"AddProductAttr.DownInput(this)\" class=\"mr10\">下移</a>";
        s += "<a href=\"javascript:void(0)\" onclick=\"$(this).parent().parent().remove()\" class=\"mr10\">删除</a>";
        s += "</td>";
        s += " </tr>";

        $("#dynamicAttrT").append(s);
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
    DlgToTeamplate: function () {
        var rows = $("#dynamicAttrT>tbody>tr");
        var rowIndex = 0;
        var index = 0;
        var jsonAppend = "";

        var isRight = true;
        rows.each(function () {
            rowIndex++;
            var inputArr = $(this).find("[name=attrItem]");
            var itemArr = new Array();
            inputArr.each(function () {
                itemArr.push($.trim($(this).val()));
            })
            var name = $.trim(itemArr[0]);
            var value = $.trim(itemArr[1]);
            if (name != "" && value != "") {
                index++;
                if (index > 1) jsonAppend += ",";
                jsonAppend += "{\"Id\":\"" + index + "\",\"KeyName\":\"" + name + "\",\"KeyValue\":\"" + value + "\",\"Sort\":\"" + index + "\"}";
            }
            else {
                isRight = false;
            }
        })
        if (!isRight) {
            $.messager.alert('错误提示', "自定义属性的第" + rowIndex + "行输入不完整，请正确输入", 'error');
            return false;
        }

        if (jsonAppend == "") {
            return false;
        }

        jsonAppend = "[" + jsonAppend + "]";
        $("#hTValue").val(jsonAppend);

        $("#dlgSaveProductAttrTemplate").dialog('open');
    },
    SaveToTeamplate: function () {

        try {
            var sName = $.trim($("#txtTName").val());
            if (sName == "") {
                $.messager.alert('错误提示', "模板名称不能为空字符串，请检查", 'error');
                return false;
            }
            $.ajax({
                url: "/h/t.html",
                type: "post",
                data: { reqName: 'SaveProductAttrTemplate', name: sName, value: $("#hTValue").val() },
                contentType: "application/x-www-form-urlencoded; charset=utf-8",
                beforeSend: function () {
                    $("#dlgWaiting").dialog('open');
                },
                complete: function () {
                    $("#dlgWaiting").dialog('close');
                },
                success: function (data) {
                    var json = eval("(" + data + ")");
                    if (json.success) {
                        jeasyuiFun.show("温馨提示", "保存成功！");
                        $('#dlgSaveProductAttrTemplate').dialog('close');
                    }
                    else {
                        $.messager.alert('系统提示', json.message, 'info');
                    }
                }
            });
        }
        catch (e) {
            $.messager.alert('错误提醒', e.name + ": " + e.message, 'error');
        }
    },
    DlgSelectProductAttrTeamplate: function () {
        var dg = $("#dgProductAttrTemplate");
        dg.datagrid('options').url = "/h/t.html";
        dg.datagrid('load', {
            reqName: 'GetProductAttrTemplateForDatagrid',
            keyword: $("#dlgProductAttrFm").find("#txtKeyword_ProductAttrTemplate").val()
        });
        $("#dlgProductAttrSelectTemplate").dialog('open');
    },
    SetProductAttrTeamplate: function () {
        var dg = $("#dgProductAttrTemplate");
        var rows = dg.datagrid("getSelections");
        if (!(rows && rows.length == 1)) {
            $.messager.alert('错误提醒', '请选择一行且仅一行再进行操作', 'error');
            return false;
        }

        var dynamicT = $("#dynamicAttrT");
        dynamicT.find("tbody>tr").remove();

        $.map(rows[0].TValue, function (item) {
            var s = "<tr class=\"pdt10\">";
            s += "<td><input type=\"text\" name=\"attrItem\" class=\"txt\" value=\"" + item.KeyName + "\" /></td><td class=\"pdl10\"><input type=\"text\" name=\"attrItem\" class=\"txt\" value=\"" + item.KeyValue + "\" /></td>";
            s += " <td class=\"pdl10\">";
            s += "<a href=\"javascript:void(0)\" onclick=\"AddProductAttr.UpInput(this)\" class=\"mr10\">上移</a>";
            s += "<a href=\"javascript:void(0)\" onclick=\"AddProductAttr.DownInput(this)\" class=\"mr10\">下移</a>";
            s += "<a href=\"javascript:void(0)\" onclick=\"$(this).parent().parent().remove()\" class=\"mr10\">删除</a>";
            s += "</td>";
            s += " </tr>";

            dynamicT.append(s);
        })

        $('#dlgProductAttrSelectTemplate').dialog('close');
    },
    SelectFromTeamplate: function () {

    }
}