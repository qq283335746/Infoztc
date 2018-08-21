
var ListQuestionSubject = {
    Init: function () {
        this.Grid(sPageIndex, sPageSize);
        this.SelectQB();
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
    SelectQB: function () {
        $.ajax({
            url: "../../ScriptServices/AdminService.asmx/GetQuestionBankList",
            type: "post",
            contentType: "application/json; charset=utf-8",
            data: '{}',
            beforeSend: function () {
            },
            complete: function () {
            },
            success: function (data) {
                var jsonData = (new Function("", "return " + data.d))();
                $("#selectQB").combobox({
                    data: jsonData,
                    valueField: "Id",
                    textField: "Named",
                    panelHeight: 'auto',
                    onLoadSuccess: function () {
                        $("#selectQB").combobox('select', $("#qbSelectId").val());
                    }
                });
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
        window.location = "?questionBankId=" + $("#selectQB").combobox("getValue") + "&name=" + $.trim($("[id$=txtName]").val()) + "";
    },
    Add: function () {
        window.location = "gtt.html";
    },
    Edit: function () {
        var rows = $('#dgT').datagrid("getSelections");
        if (!(rows && rows.length == 1)) {
            $.messager.alert('错误提醒', '请选择一行且仅一行进行编辑', 'error');
            return false;
        }
        window.location = "gtt.html?Id=" + rows[0].f0;
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
                        url: "/ScriptServices/AdminService.asmx/DelQuestionSubject",
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
                            ListQuestionSubject.ReloadGrid();
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