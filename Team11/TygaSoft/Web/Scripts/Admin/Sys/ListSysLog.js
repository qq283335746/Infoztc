
var ListSysLog = {
    Init: function () {
        this.Grid(sPageIndex, sPageSize);
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
        window.location = "?keyword=" + $.trim($("[id$=txtKeyword]").val()) + "";
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
                        url: "/ScriptServices/AdminService.asmx/DelSysLog",
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
                            ListSysLog.ReloadGrid();
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