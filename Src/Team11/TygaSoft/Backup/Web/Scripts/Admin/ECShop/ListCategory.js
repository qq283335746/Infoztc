
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
    Add: function () {
        this.Url = "/ScriptServices/AdminService.asmx/SaveCategory";
        var t = $("#treeCt");
        var node = t.tree('getSelected');
        if (!node) {
            $.messager.alert('错误提示', '请选中一个节点再进行操作', 'error');
            return false;
        }

        $("#dlg").dialog('open');
        dlgFun.Add(node);
    },
    Edit: function () {
        this.Url = "/ScriptServices/AdminService.asmx/SaveCategory";
        var t = $("#treeCt");
        var node = t.tree('getSelected');
        if (!node) {
            $.messager.alert('错误提示', '请选中一个节点再进行操作', 'error');
            return false;
        }
        $("#dlg").dialog('open');
        dlgFun.Edit(node, t);
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

        var pictureId = $("#imgCategoryPicture").parent().find("input[type=hidden]").val();
        if (pictureId == "") pictureId = "00000000-0000-0000-0000-000000000000";
        var sSort = $.trim($("#txtSort").val());
        if (sSort.length == 0) sSort = 0;

        $.ajax({
            url: ListCategory.Url,
            type: "post",
            data: '{model:{Id:"' + $("#hId").val() + '",CategoryName:"' + $("#txtName").val() + '",CategoryCode:"' + $("#txtCode").val() + '",ParentId:"' + $("#hParentId").val() + '",CategoryPictureId:"' + pictureId + '",Sort:' + sSort + ',Remark:"' + $("#txtRemark").val() + '"}}',
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
                    ListCategory.Load();
                    $('#dlg').dialog('close');
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