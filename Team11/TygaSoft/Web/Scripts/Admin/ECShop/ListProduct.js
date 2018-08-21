
var ListProduct = {
    Init: function () {
        this.Grid(sPageIndex, sPageSize);
        this.SetForm();
    },
    GetMyData: function (clientId) {
        return eval("(" + $("#" + clientId + "").html() + ")");
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
    Search: function () {
        window.location = "?name=" + $.trim($("[id$=txtName]").val()) + "&menu=" + $('#cbbMenu').combobox('getValue') + "";
    },
    SetForm: function () {
        var myDataJson = this.GetMyData("myDataForSearch");
        if (myDataJson) {
            $.map(myDataJson, function (item) {
                $("#txtName").val(item.name);
                $("#cbbMenu").combobox('setValue', item.menu);
            })
        }
    },
    Add: function () {
        window.location = "ay.html";
    },
    Edit: function () {
        var cbl = $('#dgT').datagrid("getSelections");
        if (cbl && cbl.length == 1) {
            window.location = "ay.html?Id=" + cbl[0].f0 + "";
        }
        else {
            $.messager.alert('错误提醒', '请选择一行且仅一行进行编辑', 'error');
        }
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
                        url: "/ScriptServices/AdminService.asmx/DelProduct",
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
                            window.location.reload();
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