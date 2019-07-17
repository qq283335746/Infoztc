
var ListServiceItem = {
    Add: function () {
        var node = $("#treeCt").tree('getSelected');
        if (!node) {
            $.messager.alert('错误提示', '请选择一个服务分类再进行此操作', 'error');
            return false;
        }

        $('#dlgServiceItem').dialog({
            title: "新建服务分类",
            closed: false,
            modal: true,
            width: 550,
            height: 450,
            iconCls: 'icon-save',
            href: '/t/y.html?action=add&Id=' + node.id + '',
            buttons: [{
                id: 'btnSaveServiceItem', text: '保存', iconCls: 'icon-save', handler: function () {
                    ListServiceItem.Save();
                }
            }, {
                id: 'btnCancelSaveServiceItem', text: '取消', iconCls: 'icon-cancel', handler: function () {
                    $('#dlgServiceItem').dialog('close');
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

        $('#dlgServiceItem').dialog({
            title: "编辑服务分类",
            closed: false,
            modal: true,
            width: 550,
            height: 450,
            iconCls: 'icon-save',
            href: '/t/y.html?action=edit&Id=' + node.id + '',
            buttons: [{
                id: 'btnSaveServiceItem', text: '保存', iconCls: 'icon-save', handler: function () {
                    ListServiceItem.Save();
                }
            }, {
                id: 'btnCancelSaveServiceItem', text: '取消', iconCls: 'icon-cancel', handler: function () {
                    $('#dlgServiceItem').dialog('close');
                }
            }]
        });
    },
    Del: function () {
        var t = $("#treeCt");
        var node = t.tree('getSelected');

        if (!node) {
            $.messager.alert('错误提示', "请选中一个节点再进行操作", 'error');
            return false;
        }

        try {
            var childNodes = t.tree('getChildren', node.target);
            if (childNodes && childNodes.length > 0) {
                $.messager.alert('错误提示', "请先删除子节点再删除此节点", 'error');
                return false;
            }
        }
        catch (e) {
        }

        var childNodes = t.tree('getChildren', node.target);
        if (childNodes && childNodes.length > 0) {
            $.messager.alert('错误提示', "请先删除子节点再删除此节点", 'error');
            return false;
        }

        if (node) {
            $.messager.confirm('温馨提醒', '警告：属于该节点的投票/链接/图文等数据将一并删除，确定要删除吗?', function (r) {
                if (r) {
                    var parentNode = t.tree('getParent', node.target);
                    if (parentNode) {
                        $("#hCurrExpandNode").val(parentNode.id);
                    }
                    $.ajax({
                        type: "POST",
                        url: "/ScriptServices/AdminService.asmx/DelServiceItem",
                        contentType: "application/json; charset=utf-8",
                        data: "{id:'" + node.id + "'}",
                        beforeSend: function () {
                            $("#dlgWaiting").dialog('open');
                        },
                        complete: function () {
                            $("#dlgWaiting").dialog('close');
                        },
                        success: function (data) {
                            var msg = data.d;
                            if (msg == "1") {
                                jeasyuiFun.show("温馨提醒", "保存成功！");
                                ListService.LoadTree();
                                $('#dlg').dialog('close');
                            }
                            else {
                                $.messager.alert('系统提示', msg, 'info');
                            }
                        }
                    })
                }
            });
        }
    },
    Save: function () {
        var isValid = $('#dlgFm').form('validate');
        if (!isValid) return false;

        var pictureId = $("#imgServicePicture").parent().find("input[type=hidden]").val();
        if (pictureId == "") pictureId = "00000000-0000-0000-0000-000000000000";
        var sSort = $.trim($("#txtSort").val());
        if (sSort.length == 0) sSort = 0;
        var startTime = $('#txtEnableStartTime').datetimebox('getValue');
        var endTime = $('#txtEnableEndTime').datetimebox('getValue');
        var isDisable = $("input[name=rbtnList]:checked").val();

        $.ajax({
            url: "/ScriptServices/AdminService.asmx/SaveServiceItem",
            type: "post",
            data: '{model:{Id:"' + $("#hId").val() + '",Named:"' + $("#txtName").val() + '",ParentId:"' + $("#hParentId").val() + '",PictureId:"' + pictureId + '",Sort:' + sSort + ',EnableStartTime:"' + startTime + '",EnableEndTime:"' + endTime + '",IsDisable:"' + isDisable + '"}}',
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
                    jeasyuiFun.show("温馨提示", "保存成功！");
                    ListService.LoadTree();
                    $('#dlgServiceItem').dialog('close');
                }
                else {
                    $.messager.alert('系统提示', msg, 'info');
                }
            }
        });
    },
    OnPictureClick: function () {
        DlgPictureSelect.DlgOpenId = "dlgSingleSelectServiceItemPicture";
        DlgPictureSelect.DlgSingle('PictureServiceItem');
    },
    SetSinglePicture: function (imgEleId) {
        var data = dlgSingleSelectServiceItemPicture.GetPicSelect();
        if (data.length > 0) {
            var arr = data[0].split(",");
            $("#" + imgEleId + "").attr("src", arr[1])
            $("#" + imgEleId + "").parent().find("input[type=hidden]").val(arr[0]);
            $("#dlgSingleSelectServiceItemPicture").dialog('close');
        }
    }
} 