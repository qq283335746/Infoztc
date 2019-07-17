var ListAdItem = {
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
        window.location = "?name=" + $.trim($("[id$=txtName]").val()) + "";
    },
    Add: function () {
        if ($("#hAdId").val() == "") {
            $.messager.alert('错误提醒', '请先完成基本信息', 'error');
            return false;
        }

        $("#dlgAddAdItem").dialog({
            title: "新建广告项信息",
            closed: false,
            modal: true,
            width: 500,
            height: 300,
            href: '/t/tad.html?adId=' + $("#hAdId").val() + '',
            buttons: [{
                id: 'btnSaveAdItem', text: '下一步', iconCls: 'icon-save',
                handler: function () {
                    ListAdItem.SaveAdItem();
                }
            }, {
                id: 'btnCancelSaveAdItem', text: '取消', iconCls: 'icon-cancel',
                handler: function () {
                    $("#dlgAddAdItem").dialog('close');
                }
            }]
        })
    },
    Edit: function () {
        var rows = $('#dgT').datagrid("getSelections");
        if (!(rows && rows.length == 1)) {
            $.messager.alert('错误提醒', '请选择一行且仅一行进行编辑', 'error');
            return false;
        }
        $("#dlgAddAdItem").dialog({
            title: "编辑广告项信息",
            closed: false,
            modal: true,
            width: 500,
            height: 300,
            href: '/t/tad.html?adId=' + $("#hAdId").val() + '&adItemId=' + rows[0].f0 + '',
            buttons: [{
                id: 'btnSaveAdItem', text: '下一步', iconCls: 'icon-save',
                handler: function () {
                    ListAdItem.SaveAdItem();
                }
            }, {
                id: 'btnCancelSaveAdItem', text: '取消', iconCls: 'icon-cancel',
                handler: function () {
                    $("#dlgAddAdItem").dialog('close');
                }
            }]
        })
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
                        url: "/ScriptServices/AdminService.asmx/DelAdItem",
                        type: "post",
                        contentType: "application/json; charset=utf-8",
                        data: '{itemAppend:"' + itemAppend + '"}',
                        beforeSend: function () {
                            $.messager.progress({ title: '请稍等', msg: '正在执行...' });
                        },
                        complete: function () {
                            $.messager.progress('close');
                        },
                        success: function (data) {
                            var msg = data.d;
                            if (msg != "1") {
                                $.messager.alert('系统提示', msg, 'info');
                                return false;
                            }
                            ListAdItem.ReloadGrid();
                        }
                    });
                }
            });
        }
        catch (e) {
            $.messager.alert('错误提醒', e.name + ": " + e.message, 'error');
        }
    },
    ShowAdItemLink: function (t) {
        alert($(t).attr("code"));
    },
    ShowAdItemContent: function (t) {
        alert($(t).attr("code"));
    },
    SaveAdItem: function () {
        try {

            if ($.trim($("#hAdItemId").val()) != "") {
                var myOldData = ListAdItem.GetMyData("myOldData");
                var isSameAll = true;
                if (myOldData.PictureId != $("#hImgPictureId").val()) isSameAll = false;
                if (myOldData.ActionTypeId != $("#ddlActionType").val()) isSameAll = false;
                if (myOldData.Sort != $("#txtSort").val()) isSameAll = false;
                if ($("#cbIsDisable")[0].checked) {
                    if (myOldData.IsDisable == "False") isSameAll = false;
                }
                else {
                    if (myOldData.IsDisable == "True") isSameAll = false;
                }
                if (isSameAll) {
                    ListAdItem.DlgAdItemDetail();
                    return false;
                }
            }

            $.messager.progress({ title: '请稍等', msg: '正在执行...' });
            $('#dlgAdItemFm').form('submit', {
                url: '/h/taboutsite.html',
                onSubmit: function (param) {
                    var isValid = $(this).form('validate');
                    if (!isValid) {
                        $.messager.progress('close');
                    }
                    param.reqName = "SaveAdItem";
                    param.adId = $("#hAdId").val();
                    param.isDisable = $("#cbIsDisable")[0].checked;

                    return isValid;
                },
                success: function (result) {
                    $.messager.progress('close');
                    var data = eval('(' + result + ')');
                    if (!data.success) {
                        $.messager.alert('错误提示', data.message, 'error');
                        return false;
                    }

                    if ($.trim($("#hAdItemId").val()) == "") {
                        $("#hAdItemId").val(data.data);
                    }

                    ListAdItem.DlgAdItemDetail();
                }
            });
        }
        catch (e) {
            $.messager.progress('close');
            $.messager.alert('错误提醒', e.name + ": " + e.message, 'error');
        }
    },
    DlgAdItemDetail: function () {
        var adItemId = $("#hAdItemId").val();
        if (adItemId == "") {
            $.messager.alert('错误提醒', "请先完成上一步的操作再进行此操作！", 'error');
            return false;
        }
        var sHref = "";
        var sTitle = "";
        var w = 800;
        var h = 300;
        var actionType = $.trim($("#ddlActionType>option:selected").text());
        if (actionType.indexOf("图文") > -1) {
            sHref = "/t/yad.html?adItemId=" + $("#hAdItemId").val() + "";
            sTitle = "新建/编辑图文信息";
            w = 1000;
            h = $(window).height() * 0.8;
        }
        else if (actionType.indexOf("跳转至商品") > -1) {
            sHref = "/t/aad.html?adItemId=" + $("#hAdItemId").val() + "";
            sTitle = "新建/编辑链接信息";
            h = $(window).height() * 0.8;
        }
        else {
            sHref = "/t/gad.html?adItemId=" + $("#hAdItemId").val() + "";
            sTitle = "新建/编辑链接信息";
        }

        $("#dlgAdItemDetail").dialog({
            title: sTitle,
            closed: false,
            modal: true,
            width: w,
            height: h,
            href: sHref,
            buttons: [{
                id: 'btnSaveAdItemDetail', text: '保 存', iconCls: 'icon-save',
                handler: function () {
                    if (actionType.indexOf("图文") > -1) {
                        ListAdItem.OnSaveItemDetail = AddAdItemContent.OnSave();
                    }
                    else if (actionType.indexOf("跳转至商品") > -1) {
                        ListAdItem.OnSaveItemDetail = AddAdItemLinkProduct.OnSave();
                    }
                    else {
                        ListAdItem.OnSaveItemDetail = AddAdItemLink.OnSave();
                    }
                }
            }, {
                id: 'btnCancelSaveAdItemDetail', text: '取消', iconCls: 'icon-cancel',
                handler: function () {
                    $("#dlgAdItemDetail").dialog('close');
                }
            }]
        })
    }
}