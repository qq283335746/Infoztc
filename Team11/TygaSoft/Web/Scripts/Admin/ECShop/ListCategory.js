
var ListCategory = {
    Url: "",
    Init: function () {
        this.Load();
    },
    Load: function () {
        var t = $("#treeCt");

        $.ajax({
            url: "/ScriptServices/AdminService.asmx/GetJsonForCategory",
            type: "post",
            data: "{}",
            contentType: "application/json; charset=utf-8",
            beforeSend: function () {
                $("#dlgWaiting").dialog('open');
            },
            complete: function () {
                $("#dlgWaiting").dialog('close');
            },
            success: function (json) {

                var jsonData = (new Function("", "return " + json.d))();
                t.tree({
                    data: jsonData,
                    animate: true,
                    onContextMenu: function (e, node) {
                        e.preventDefault();
                        $(this).tree('select', node.target);
                        $('#mmTree').menu('show', {
                            left: e.pageX,
                            top: e.pageY
                        });
                    }
                })
                ListCategory.OnCurrExpand(t);
            }
        });
    },
    LoadSuccessForCategory: function (node, data) {
        var t = $('#cbtCategory').combotree('tree');
        var node = t.tree('find', $("#hCategoryId").val());
        if (node) {
            t.tree('select', node.target);
            $('#cbtCategory').combotree('setValue', node.id);
        }
    },
    Add: function () {
        this.Url = "/ScriptServices/AdminService.asmx/SaveCategory";
        var t = $("#treeCt");
        var node = t.tree('getSelected');
        if (!node) {
            $.messager.alert('错误提示', '请选中一个节点再进行操作', 'error');
            return false;
        }

        $('#dlgCategory').dialog({
            title: "新建分类",
            href: '/t/tCategory.html?action=add&Id=' + node.id + '',
            closed: false,
            modal: true,
            iconCls: 'icon-save',
            buttons: [{
                id: 'btnSaveCategory', text: '保存', iconCls: 'icon-save', handler: function () {
                    ListCategory.Save();
                }
            }, {
                id: 'btnCancelSaveCategory', text: '取消', iconCls: 'icon-cancel', handler: function () {
                    $('#dlgCategory').dialog('close');
                }
            }]
        });
    },
    Edit: function () {
        this.Url = "/ScriptServices/AdminService.asmx/SaveCategory";
        var t = $("#treeCt");
        var node = t.tree('getSelected');
        if (!node) {
            $.messager.alert('错误提示', '请选中一个节点再进行操作', 'error');
            return false;
        }

        $("#hCurrExpandNode").val(node.id);
        $('#dlgCategory').dialog({
            title: "编辑分类",
            href: '/t/tCategory.html?action=edit&Id=' + node.id + '',
            closed: false,
            modal: true,
            iconCls: 'icon-save',
            buttons: [{
                id: 'btnSaveCategory', text: '保存', iconCls: 'icon-save', handler: function () {
                    ListCategory.Save();
                }
            }, {
                id: 'btnCancelSaveCategory', text: '取消', iconCls: 'icon-cancel', handler: function () {
                    $('#dlgCategory').dialog('close');
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
            $.messager.confirm('温馨提醒', '确定要删除吗?', function (r) {
                if (r) {
                    var parentNode = t.tree('getParent', node.target);
                    if (parentNode) {
                        $("#hCurrExpandNode").val(parentNode.id);
                    }
                    $.ajax({
                        type: "POST",
                        url: "/ScriptServices/AdminService.asmx/DelCategory",
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
                                ListCategory.Load();
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
        
        var sSort = $.trim($("#txtSort").val());
        if (sSort.length == 0) sSort = 0;
        var pictureId = $("#imgCategoryPicture").parent().find("input[type=hidden]").val();
        if (pictureId == "") pictureId = "00000000-0000-0000-0000-000000000000";

        $.ajax({
            url: ListCategory.Url,
            type: "post",
            data: '{model:{Id:"' + $("#hId").val() + '",CategoryName:"' + $("#txtName").val() + '",CategoryCode:"' + $("#txtCode").val() + '",ParentId:"' + $("#hParentId").val() + '",PictureId:"' + pictureId + '",Sort:' + sSort + ',Remark:"' + $("#txtRemark").val() + '"}}',
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
                    setTimeout(function () {
                        ListCategory.Load();
                    }, 500)
                    $('#dlgCategory').dialog('close');
                }
                else {
                    $.messager.alert('系统提示', msg, 'info');
                }
            }
        });
    },
    OnCurrExpand: function (t) {
        var root = t.tree('getRoot');
        if (root) {
            var childNodes = t.tree('getChildren', root.target);
            if (childNodes && childNodes != undefined && (childNodes.length > 0)) {
                var cnLen = childNodes.length;
                for (var i = 0; i < cnLen; i++) {
                    t.tree('collapse', childNodes[i].target);
                }
            }
        }
        var currNode = t.tree('find', $("#hCurrExpandNode").val());
        if (currNode) {
            ListCategory.OnExpand(t, currNode);
        }
    },
    OnExpand: function (t, node) {
        if (node) {
            t.tree('expand', node.target);
            var pNode = t.tree('getParent', node.target);
            if (pNode) {
                ListCategory.OnExpand(t, pNode);
            }
        }
    }
}