var sort;
var ListAnswerOption = {
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
        window.location = "?qsId=" + $("#qsId").val() + "";
    },
    Add: function () {
        window.location = "gtg.html?qsId=" + $("#qsId").val();
    },
    Edit: function () {
        var rows = $('#dgT').datagrid("getSelections");
        if (!(rows && rows.length == 1)) {
            $.messager.alert('错误提醒', '请选择一行且仅一行进行编辑', 'error');
            return false;
        }
        window.location = "gtg.html?Id=" + rows[0].f0 + "&qsId=" + $("#qsId").val();
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
                        url: "/ScriptServices/AdminService.asmx/DelAnswerOption",
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
                            ListAnswerOption.ReloadGrid();
                        }
                    });
                }
            });
        }
        catch (e) {
            $.messager.alert('错误提醒', e.name + ": " + e.message, 'error');
        }
    },
    Sort: function (sortType) {
        try {
            var isValid = $('#form1').form('validate');
            if (!isValid) return false;

            var rows = $('#dgT').datagrid("getSelections");
            var rowsAll = $('#dgT').datagrid("getRows")
            if (!(rows && rows.length == 1)) {
                $.messager.alert('错误提醒', '请选择一行且仅一行进行编辑', 'error');
                return false;
            }
            if (sortType == 0 && rows[0].f2 == 1) {
                $.messager.alert('错误提醒', '当前已是第一行', 'error');
                return false;
            }
            if (sortType == 1 && rows[0].f2 == rowsAll.length) {
                $.messager.alert('错误提醒', '当前已是最后一行', 'error');
                return false;
            }

            var id = rows[0].f0;
            var qsId = $("#qsId").val();
            var name = rows[0].f4;
            var isTrue = rows[0].f6;
            sort = sortType == 0 ? rows[0].f2 * 1 - 1 : rows[0].f2 * 1 + 1;

            $.ajax({
                url: "../../ScriptServices/AdminService.asmx/SaveAnswerOption",
                type: "post",
                data: '{model:{Id:"' + id + '",QuestionSubjectId:"' + qsId + '",OptionContent:"' + name + '",Sort:' + sort + ',IsTrue:"' + isTrue + '"}}',
                contentType: "application/json; charset=utf-8",
                beforeSend: function () {
                    $("#dlgWaiting").dialog('open');
                },
                complete: function () {
                    $("#dlgWaiting").dialog('close');
                },
                success: function (data) {
                    var msg = data.d;
                    if (msg == "1") {
                        var row = $('#dgT').datagrid("getData").rows[sort-1];

                        var id = row.f0;
                        var qsId = $("#qsId").val();
                        var name = row.f4;
                        var isTrue = row.f6;
                        var sort1 = sortType == 0 ? row.f2 * 1 + 1 : row.f2 * 1 - 1;

                        $.ajax({
                            url: "../../ScriptServices/AdminService.asmx/SaveAnswerOption",
                            type: "post",
                            data: '{model:{Id:"' + id + '",QuestionSubjectId:"' + qsId + '",OptionContent:"' + name + '",Sort:' + sort1 + ',IsTrue:"' + isTrue + '"}}',
                            contentType: "application/json; charset=utf-8",
                            beforeSend: function () {
                                $("#dlgWaiting").dialog('open');
                            },
                            complete: function () {
                                $("#dlgWaiting").dialog('close');
                            },
                            success: function (data) {
                                var msg = data.d;
                                if (msg == "1") {
                                    ListAnswerOption.ReloadGrid();
                                }
                                else {
                                    $.messager.alert('系统提示', msg, 'info');
                                }
                            }
                        });
                    }
                    else {
                        $.messager.alert('系统提示', msg, 'info');
                    }
                }
            });
        }
        catch (e) {
            $.messager.alert('错误提醒', e.name + ": " + e.message, 'error');
        }
    }
} 