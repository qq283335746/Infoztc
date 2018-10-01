
var ListServiceContent = {
    LoadDg: function (id, serviceItemId) {
        var dgContent = $('#' + id + '');
        dgContent.datagrid('options').url = "/Handlers/Admin/Serve/HandlerService.ashx";
        dgContent.datagrid('load', {
            reqName: 'GetDatagridForServiceContent',
            serviceItemId: serviceItemId
        });
    },
    ReloadDg: function () {
        var serviceItemId = "";
        var node = $("#treeCt").tree('getSelected');
        if (node) {
            serviceItemId = node.id;
        }
        var dgContent = $('#dgContent');
        dgContent.datagrid('reload', {
            reqName: 'GetDatagridForServiceContent',
            serviceItemId: serviceItemId
        });
    },
    Search:function(){
        var serviceItemId = "";
        var node = $("#treeCt").tree('getSelected');
        if (node) {
            serviceItemId = node.id;
        }
        var dgContent = $('#dgContent');
        dgContent.datagrid('reload', {
            reqName: 'GetDatagridForServiceContent',
            serviceItemId: serviceItemId,
            keyword: $("#txtKeywordForContent").val()
        });
    },
    Add: function () {
        var node = $("#treeCt").tree('getSelected');
        if (!node) {
            $.messager.alert('错误提示', '请选择一个服务分类再进行此操作', 'error');
            return false;
        }

        $('#dlgServiceContent').dialog({
            title:"新建/编辑图文",
            href: '/a/tga.html',
            closed: false,
            modal:true,
            width:$(window).width()*0.8,
            height: $(window).height() * 0.8,
            iconCls: 'icon-save',
            buttons: [{
                id: 'btnSaveContent', text: '保存', iconCls: 'icon-save', handler: function () {
                    ListServiceContent.Save();
                }
            }, {
                id: 'btnCancelContent', text: '取消', iconCls: 'icon-cancel', handler: function () {
                    $('#dlgServiceContent').dialog('close');
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

        var rows = $('#dgContent').datagrid("getSelections");
        if (rows && rows.length == 1) {
            $('#dlgServiceContent').dialog({
                href: '/a/tga.html?Id=' + rows[0].Id + '',
                closed: false
            });
        }
        else {
            $.messager.alert('错误提醒', '请选择一行且仅一行进行编辑', 'error');
        }
    },
    Del: function () {
        try {
            var rows = $('#dgContent').datagrid("getSelections");
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
                        url: "/ScriptServices/AdminService.asmx/DelServiceContent",
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
                            ListServiceContent.ReloadDg();
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
            $('#dlgFmServiceContent').form('submit', {
                url: '../../Handlers/Admin/Serve/HandlerService.ashx',
                onSubmit: function (param) {

                    var isValid = $(this).form('validate');
                    if (!isValid) {
                        $.messager.progress('close');
                    }

                    param.reqName = "SaveServiceContent";
                    param.content = editor_content.html().replace(/</g, "&lt;");

                    if ($("#hServiceContentId").val() != "") {
                        $("#txtaContent_ServiceContent").text("");
                    }

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
                        ListServiceContent.ReloadDg();
                        if ($("#hServiceContentId").val() == "") {
                            ListService.LoadTree();
                        }
                        
                        $('#dlgServiceContent').dialog('close');
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