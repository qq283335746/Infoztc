
var ListServiceLink = {
    LoadDg: function (id, serviceItemId) {
        var dgLink = $('#' + id + '');
        dgLink.datagrid('options').url = "/Handlers/Admin/Serve/HandlerService.ashx";
        dgLink.datagrid('load', {
            reqName: 'GetDatagridForServiceLink',
            serviceItemId: serviceItemId
        });
    },
    ReloadDg: function () {
        var serviceItemId = "";
        var node = $("#treeCt").tree('getSelected');
        if (node) {
            serviceItemId = node.id;
        }
        var dgLink = $('#dgLink');
        dgLink.datagrid('reload', {
            reqName: 'GetDatagridForServiceLink',
            serviceItemId: serviceItemId
        });
    },
    Search: function () {
        var serviceItemId = "";
        var node = $("#treeCt").tree('getSelected');
        if (node) {
            serviceItemId = node.id;
        }
        var dgLink = $('#dgLink');
        dgLink.datagrid('reload', {
            reqName: 'GetDatagridForServiceLink',
            serviceItemId: serviceItemId,
            keyword: $("#txtKeywordForLink").val()
        });
    },
    Add: function () {
        var node = $("#treeCt").tree('getSelected');
        if (!node) {
            $.messager.alert('错误提示', '请选择一个服务分类再进行此操作', 'error');
            return false;
        }

        $('#dlgServiceLink').dialog({
            title: "新建/编辑链接",
            href: '/a/taa.html',
            closed: false,
            modal:true,
            width:790,
            height: 450,
            iconCls:'icon-save',
            buttons: [{
                id: 'btnSaveLink', text: '保存', iconCls: 'icon-save', handler: function () {
                    ListServiceLink.Save();
                }
            }, {
                id: 'btnCancelLink', text: '取消', iconCls: 'icon-cancel', handler: function () {
                    $('#dlgServiceLink').dialog('close');
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

        var rows = $('#dgLink').datagrid("getSelections");
        if (rows && rows.length == 1) {
            $('#dlgServiceLink').dialog({
                href: '/a/taa.html?Id=' + rows[0].Id + '',
                closed: false,
                width: $(window).width() * 0.8,
                height: $(window).height() * 0.8,
                iconCls: 'icon-save',
                buttons: [{
                    id: 'btnSaveLink', text: '保存', iconCls: 'icon-save', handler: function () {
                        ListServiceLink.Save();
                    }
                }, {
                    id: 'btnCancelLink', text: '取消', iconCls: 'icon-cancel', handler: function () {
                        $('#dlgServiceLink').dialog('close');
                    }
                }]
            });
        }
        else {
            $.messager.alert('错误提醒', '请选择一行且仅一行进行编辑', 'error');
        }
    },
    Del: function () {
        try {
            var rows = $('#dgLink').datagrid("getSelections");
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
                        url: "/ScriptServices/AdminService.asmx/DelServiceLink",
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
                            ListServiceLink.ReloadDg();
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
            $('#dlgFmServiceLink').form('submit', {
                url: '/h/y.html',
                onSubmit: function (param) {

                    var isValid = $(this).form('validate');
                    if (!isValid) {
                        $.messager.progress('close');
                    }

                    param.reqName = "SaveServiceLink";
                    param.isDisable = $("#dlgFmServiceLink").find("input[name=rbtnList_ServiceLink]:checked").val();

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
                        ListServiceLink.ReloadDg();
                        if ($("#hServiceLinkId").val() == "") {
                            ListService.LoadTree();
                        }
                        $('#dlgServiceLink').dialog('close');
                    }, 500);
                }
            });
        }
        catch (e) {
            $.messager.progress('close');
            $.messager.alert('错误提醒', e.name + ": " + e.message, 'error');
        }
    },
    OnPictureClick: function () {
        DlgPictureSelect.DlgOpenId = "dlgSingleSelectServiceLinkPicture";
        DlgPictureSelect.DlgSingle('PictureServiceLink');
    },
    SetSinglePicture: function (imgEleId) {
        var data = dlgSingleSelectServiceLinkPicture.GetPicSelect();
        if (data.length > 0) {
            var arr = data[0].split(",");
            $("#" + imgEleId + "").attr("src", arr[1])
            $("#" + imgEleId + "").parent().find("input[type=hidden]").val(arr[0]);
            $("#dlgSingleSelectServiceLinkPicture").dialog('close');
        }
    }
}