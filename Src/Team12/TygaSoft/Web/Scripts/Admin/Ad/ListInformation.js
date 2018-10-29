
var ListInformation = {
    Init: function () {
        this.Grid(sPageIndex, sPageSize);
        this.BindInforAdList();
    },
    GetMyData: function (clientId) {
        var myData = $("#" + clientId + "").html();
        return eval("(" + myData + ")");
    },
    Grid: function (pageIndex, pageSize) {
        var pager = $('#dgT').datagrid('getPager');
        $(pager).pagination({
            total: sTotalRecord,
            pageNumber: sPageIndex,
            pageSize: sPageSize,
            onSelectPage: function (pageNumber, pageSize) {
                if (sQueryStr.length == 0) {
                    window.location = "?pageIndex=" + pageNumber + "&pageSize=" + pageSize + "";
                }
                else {
                    window.location = "?" + sQueryStr + "&pageIndex=" + pageNumber + "&pageSize=" + pageSize + "";
                }
            }
        });
    },
    ReloadGrid: function () {
        var reloadUrl = "";
        if (sQueryStr.length == 0) {
            reloadUrl = "?pageIndex=1&pageSize=" + sPageSize + "";
        }
        else {
            reloadUrl = "?" + sQueryStr + "&pageIndex=1&pageSize=" + sPageSize + "";
        }
        window.location = reloadUrl;
    },
    Search: function () {
        window.location = "?title=" + $.trim($("[id$=txtTitle]").val()) + "";
    },
    Add: function () {
        window.location = "yinfor.html";
    },
    Edit: function () {
        var rows = $('#dgT').datagrid("getSelections");
        if (!(rows && rows.length == 1)) {
            $.messager.alert('错误提醒', '请选择一行且仅一行进行编辑', 'error');
            return false;
        }
        window.location = "yinfor.html?Id=" + rows[0].f0;
    },
    Del: function () {
        try {
            var rows = $('#dgT').datagrid("getSelections");
            if (!rows || rows.length == 0) {
                $.messager.alert('错误提醒', '请至少选择一行再进行操作', 'error');
                return false;
            }
            var itemAppend = "";
            for (var i = 0; i < rows.length; i++) {
                if (i > 0) itemAppend += ",";
                itemAppend += rows[i].f0;
            }
            $.messager.confirm('温馨提醒', '确定要删除吗？', function (r) {
                if (r) {
                    $.ajax({
                        url: "/ScriptServices/AdminService.asmx/DelInformation",
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
                            ListInformation.ReloadGrid();
                        }
                    });
                }
            });
        }
        catch (e) {
            $.messager.alert('错误提醒', e.name + ": " + e.message, 'error');
        }
    },
    OpenInforAdWin: function (id) {
        $("#RelInforAd").dialog("open");
        $("#InformationId").val(id);
        this.BindSelectedInforAdList(id);
    },
    BindSelectedInforAdList: function (id) {
        $("#selectedList").html("");
        $.post("/h/tinforad.html", { "reqName": "GetSelectedInforAdList", "id": id }, function (data) {
            var dataDsObj = eval("(" + data + ")"); //转换为json对象
            var dataObj = dataDsObj.Table;
            var outHtml = "";
            for (var i = 0; i < dataObj.length; i++) {
                outHtml += "<option value=\"" + dataObj[i].InformationAdId + "\">" + dataObj[i].Title + "</option>";
            }
            $("#selectedList").html(outHtml);
        });
    },
    BindInforAdList: function () {
        $.post("/h/tinforad.html", { "reqName": "GetInforAdList" }, function (data) {
            var dataObj = eval("(" + data + ")"); //转换为json对象
            var outHtml = "";
            for (var i = 0; i < dataObj.length; i++) {
                outHtml += "<option value=\"" + dataObj[i].Id + "\">" + dataObj[i].Title + "</option>";
            }
            $("#selectList").html(outHtml);
        });
    },
    AddRelInforAd: function () {
        var outHtml = "";
        $("#selectList option:selected").each(function () {
            if ($("#selectedList option[value='" + $(this).val() + "']").length == 0) {
                outHtml += "<option value=\"" + $(this).val() + "\">" + $(this).text() + "</option>";
            }
        });
        $("#selectedList").append(outHtml);
    },
    DelRelInfoAd: function () {
        var selOpt = $("#selectedList option:selected");
        selOpt.remove();
    },
    SubmitRelInforAd: function () {
        var jsonData = "{\"reqName\":\"SubmitRelInforAd\",\"InformationId\":\"" + $("#InformationId").val() + "\",\"data\":\"";
        $("#selectedList option").each(function () {
            jsonData += $(this).val() + ",";
        });
        if (jsonData.substring(jsonData.length - 1, jsonData.length)) {
            jsonData = jsonData + "\"}";
        }
        else {
            jsonData = jsonData.substring(0, jsonData.length - 1) + "\"}";
        }
        var json = eval("(" + jsonData + ")");

        $("#dlgWaiting").dialog('open');
        $.post("/h/tinforad.html", json, function (data) {
            data = eval("(" + data + ")");
            $("#dlgWaiting").dialog('close');
            $.messager.alert('提示', data.message, 'info', function () {
                if (true == data.success) {
                    $("#RelInforAd").dialog("close");
                }
            });
        });
    },
    MoveUp: function () {
        var $select = $('#selectedList');
        var $selectOption = $select.find(':selected');
        if ($selectOption.length != 1) {
            $.messager.alert('提示', "请选择一项", 'info');
            return;
        }
        var index = $selectOption.index();
        if (index == 0) {
            return;
        }
        $selectOption.insertBefore($selectOption.prev());
    },
    MoveDown: function () {
        var $select = $('#selectedList');
        var $selectOption = $select.find(':selected');
        if ($selectOption.length != 1) {
            $.messager.alert('提示', "请选择一项", 'info');
            return;
        }
        var $option = $select.find('option');
        var index = $selectOption.index();
        if (index == $option.length - 1) {
            return;
        }
        $selectOption.insertAfter($selectOption.next());
    }
}
