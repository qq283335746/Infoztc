
var ListActivitySubject = {
    Init: function () {
        this.Grid(sPageIndex, sPageSize);
        this.SelectType();
    },
    GetMyData: function (clientId) {
        var myData = $("#" + clientId + "").html();
        return eval("(" + myData + ")");
    },
    Grid: function (pageIndex, pageSize) {
        var pager = $('#dgT').datagrid('getPager');
        $('#dgT').datagrid({
            onLoadSuccess: function (data) {
                if ($("#hType").val() == "") {
                    $('#dgT').datagrid('hideColumn', 'f7');
                    $('#dgT').datagrid('hideColumn', 'f8');
                    $('#dgT').datagrid('hideColumn', 'f9');
                    $('#dgT').datagrid('hideColumn', 'f10');
                    $('#dgT').datagrid('hideColumn', 'f11');
                }
                else if ($("#hType").val() == "0") { 
                    $('#dgT').datagrid('showColumn', 'f7');
                    $('#dgT').datagrid('showColumn', 'f8');
                    $('#dgT').datagrid('hideColumn', 'f9');
                    $('#dgT').datagrid('hideColumn', 'f10');
                    $('#dgT').datagrid('hideColumn', 'f11');
                }
                else
                {
                    $('#dgT').datagrid('hideColumn', 'f7');
                    $('#dgT').datagrid('hideColumn', 'f8');
                    $('#dgT').datagrid('showColumn', 'f9');
                    $('#dgT').datagrid('showColumn', 'f10');
                    $('#dgT').datagrid('showColumn', 'f11');
                }
            }
        });
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
    SelectType: function () {
        var jsonData = '[{Id:"",Name:"请选择"},{Id:"0",Name:"投票"},{Id:"1",Name:"报名"}]';
        jsonData = eval(jsonData);
        $("#selectType").combobox({
            data: jsonData,
            valueField: "Id",
            textField: "Name",
            panelHeight: 'auto',      
            onLoadSuccess: function () {
                $("#selectType").combobox('select', $("#hType").val());
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
        window.location = "?title=" + $.trim($("[id$=txtTitle]").val()) + "&content=" + $.trim($("[id$=txtContent]").val()) + "&type=" + $("#selectType").combobox("getValue") + "";
    },
    Add: function () {
        window.location = "yactivity.html";
    },
    Edit: function () {
        var rows = $('#dgT').datagrid("getSelections");
        if (!(rows && rows.length == 1)) {
            $.messager.alert('错误提醒', '请选择一行且仅一行进行编辑', 'error');
            return false;
        }
        window.location = "yactivity.html?Id=" + rows[0].f0;
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
                        url: "/ScriptServices/AdminService.asmx/DelActivity",
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
                            ListActivitySubject.ReloadGrid();
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