
var ListServiceVote = {
    LoadDg: function (id, serviceItemId) {
        var dgLink = $('#' + id + '');
        dgLink.datagrid('options').url = "/Handlers/Admin/Serve/HandlerService.ashx";
        dgLink.datagrid('load', {
            reqName: 'GetDatagridForServiceVote',
            serviceItemId: serviceItemId
        });
    },
    ReloadDg: function () {
        var serviceItemId = "";
        var node = $("#treeCt").tree('getSelected');
        if (node) {
            serviceItemId = node.id;
        }
        var dgVote = $('#dgVote');
        dgVote.datagrid('reload', {
            reqName: 'GetDatagridForServiceVote',
            serviceItemId: serviceItemId
        });
    },
    Search: function () {
        var serviceItemId = "";
        var node = $("#treeCt").tree('getSelected');
        if (node) {
            serviceItemId = node.id;
        }
        var dgVote = $('#dgVote');
        dgVote.datagrid('reload', {
            reqName: 'GetDatagridForServiceVote',
            serviceItemId: serviceItemId,
            keyword: $("#txtKeywordForVote").val()
        });
    },
    Add: function () {
        var node = $("#treeCt").tree('getSelected');
        if (!node) {
            $.messager.alert('错误提示', '请选择一个服务分类再进行此操作', 'error');
            return false;
        }

        $('#dlgServiceVote').dialog({
            title:"新建/编辑投票",
            href: '/a/tgg.html',
            closed: false,
            modal: true,
            width: $(window).width() * 0.8,
            height: $(window).height() * 0.8,
            iconCls: 'icon-save',
            buttons: [{
                id: 'btnSaveVote', text: '保存', iconCls: 'icon-save', handler: function () {
                    ListServiceVote.Save();
                }
            }, {
                id: 'btnCancelVote', text: '取消', iconCls: 'icon-cancel', handler: function () {
                    $('#dlgServiceVote').dialog('close');
                }
            }]
        });
    },
    Edit: function () {
        var node = $("#treeCt").tree('getSelected');
        if (!node) {
            $.messager.alert('错误提示', '请选择一个服务分类再进行此操作', 'error');
            return false;
        }

        var rows = $('#dgVote').datagrid("getSelections");
        if (rows && rows.length == 1) {
            $('#dlgServiceVote').dialog({
                href: '/a/tgg.html?Id=' + rows[0].Id + '',
                closed: false
            });
        }
        else {
            $.messager.alert('错误提醒', '请选择一行且仅一行进行编辑', 'error');
        }
    },
    Del: function () {
        try {
            var rows = $('#dgVote').datagrid("getSelections");
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
                        url: "/ScriptServices/AdminService.asmx/DelServiceVote",
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
                            ListServiceVote.ReloadDg();
                            ListService.LoadTree();
                        }
                    });
                }
            });
        }
        catch (e) {
            $.messager.alert('错误提醒', e.name + ": " + e.message, 'error');
        }
    },
    Save: function () {
        try {
            $.messager.progress({
                title: '请稍等',
                msg: '正在执行...'
            });
            $('#dlgFmServiceVote').form('submit', {
                url: '../../Handlers/Admin/Serve/HandlerService.ashx',
                onSubmit: function (param) {

                    var isValid = $(this).form('validate');
                    if (!isValid) {
                        $.messager.progress('close');
                    }

                    param.reqName = "SaveServiceVote";
                    param.content = editor_Vote.html().replace(/</g, "&lt;");

                    return isValid;
                },
                success: function (data) {
                    $.messager.progress('close');
                    var jsonData = eval('(' + data + ')');
                    if (!jsonData.success) {
                        $.messager.alert('错误提示', jsonData.message, 'error');
                        return false;
                    }
                    jeasyuiFun.show("温馨提醒", jsonData.message);
                    setTimeout(function () {
                        ListServiceVote.ReloadDg();
                        if ($("#hServiceVoteId").val() == "") {
                            ListService.LoadTree();
                        }
                        $('#dlgServiceVote').dialog('close');
                    }, 500);
                }
            });
        }
        catch (e) {
            $.messager.progress('close');
            $.messager.alert('错误提醒', e.name + ": " + e.message, 'error');
        }
    }
}