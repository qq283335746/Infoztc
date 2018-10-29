
var CategoryPicture = {
    IsSelectSingle: true,
    Init: function () {
        var parmsData = { reqName: "GetJsonForDatagrid", page: 1, rows: 10 };
        this.BindPicture(parmsData);
    },
    BindPicture: function (parms) {
        $.get("../../Handlers/Admin/ECShop/HandlerCategoryPicture.ashx", parms, function (data) {
            var jsonData = eval("(" + data + ")");
            var ppWrap = $("#ppCategoryPictureWrap");
            ppWrap.children("div").filter(":not(:first)").remove();
            var row = ppWrap.children("div").eq(0);
            $.map(jsonData.rows, function (item) {
                var newRow = row.clone(true);
                newRow.appendTo(ppWrap);
                newRow.find("img").attr("src", item.MPicture);
                newRow.find("input[type=hidden]").val(item.Id);
                newRow.show();
            })

            if (CategoryPicture.IsSelectSingle) {
                CategoryPicture.PictureSingleClick();
            }
            else {
                CategoryPicture.PictureMultiClick();
            }

            if (jsonData.total > 10) {
                $('#ppCategoryPicture').pagination({
                    total: jsonData.total,
                    pageSize: 10,
                    onSelectPage: function (pageNumber, pageSize) {
                        var parmsData = { reqName: "GetJsonForDatagrid", page: pageNumber, rows: pageSize };
                        CategoryPicture.BindPicture(parmsData);
                    }
                });
            }
        })
    },
    PictureSingleClick: function () {
        $("#ppCategoryPictureWrap").children().bind("click", function () {
            $(this).addClass("curr").siblings().removeClass("curr");
        })
    },
    PictureMultiClick: function () {
        $("#ppCategoryPictureWrap").children().bind("click", function () {
            $(this).addClass("curr");
            var firstCol = $("#imgProductOther").children().eq(0);
            var newCol = firstCol.clone(true);
            newCol.appendTo($("#imgProductOther"));
            newCol.find("img").attr("src", $(this).find("img").attr("src"));
            newCol.find("input[type=hidden]").val($(this).find("input[type=hidden]").val());
            newCol.show();
        })
    },
    DlgSingle: function () {
        this.IsSelectSingle = true;
        var w = $(window).width() * 0.9;
        var h = $(window).height() * 0.9;
        $("#dlgCategoryPicture").dialog({
            title: '分类品牌相册',
            width: w,
            height: h,
            border: false,
            closed: false,
            href: '/Templates/DlgCategoryPicture.htm',
            modal: true,
            buttons: [{
                text: '确定',
                iconCls: 'icon-ok',
                handler: function () {
                    var selectPic = $("#ppCategoryPictureWrap").children().filter("[class*=curr]");
                    var picUrl = $.trim(selectPic.find("img").attr("src"));
                    if (picUrl == "") {
                        $.messager.alert('错误提醒', "请选择图片", 'error');
                        return false;
                    }
                    $("#imgCategoryPicture").attr("src", picUrl);
                    $("#imgCategoryPicture").parent().find("input[type=hidden]").val(selectPic.find("input[type=hidden]").val());

                    $("#dlgCategoryPicture").dialog('close');
                }
            }, {
                text: '取 消',
                iconCls: 'icon-cancel',
                handler: function () {
                    $("#dlgCategoryPicture").dialog('close');
                }
            }],
            toolbar: [{
                text: '上 传',
                iconCls: 'icon-add',
                handler: function () {
                    CategoryPicture.DlgUpload();
                }
            }]
        })
    },
    DlgMulti: function () {
        CategoryPicture.IsSelectSingle = false;
        var w = $(window).width() * 0.9;
        var h = $(window).height() * 0.9;
        $("#dlgCategoryPicture").dialog({
            title: '分类品牌相册',
            width: w,
            height: h,
            closed: false,
            href: '/Templates/DlgCategoryPictures.htm',
            modal: true,
            buttons: [{
                text: '确定',
                iconCls: 'icon-ok',
                handler: function () {
                    $("#dlgCategoryPicture").dialog('close');
                }
            }],
            toolbar: [{
                text: '上 传',
                iconCls: 'icon-add',
                handler: function () {
                    CategoryPicture.DlgUpload()
                }
            }]
        })
    },
    DlgUpload: function () {
        var h = $(window).height() * 0.9;
        $("#dlgCategoryPictureUpload").dialog({
            title: '上传',
            width: 600,
            height: h,
            closed: false,
            href: '/Templates/UploadPicture.htm',
            modal: true,
            buttons: [{
                text: '上 传',
                iconCls: 'icon-ok',
                handler: function () {
                    CategoryPicture.OnUpload();
                }
            }, {
                text: '取 消',
                iconCls: 'icon-cancel',
                handler: function () {
                    $("#dlgCategoryPictureUpload").dialog('close');
                }
            }],
            toolbar: [{
                id: 'btnAddCategoryFile',
                text: '添 加',
                iconCls: 'icon-add',
                handler: function () {
                    $("#dlgUploadFm").find("input").each(function () {
                        $(this).attr("id", "categoryPicture_" + $(this).attr("id") + "");
                    })
                    $("#dlgUploadFm").attr("id", "dlgUploadFm_CategoryPicture");
                    var currDlgFm = $("#dlgUploadFm_CategoryPicture");

                    var rowLen = currDlgFm.children("div").length;
                    rowLen++;
                    var newRow = $("<div class=\"mb10\"><input type=\"text\" id=\"categoryPicture_file" + rowLen + "\" name=\"file" + rowLen + "\" style=\"width:500px;\" /><a href=\"#\" onclick=\"$(this).parent().remove();return false;\" style=\"margin-left:10px;\">删 除</a></div>");
                    currDlgFm.append(newRow);
                    newRow.find("#categoryPicture_file" + rowLen + "").filebox({
                        buttonText: '选择文件',
                        prompt: '选择图片'
                    })
                    newRow.find("a:last").linkbutton({
                        iconCls: 'icon-remove',
                        plain: true
                    });
                }
            }]
        })
    },
    OnUpload: function () {
        try {
            $.messager.progress({
                title: '请稍等',
                msg: '正在执行...'
            });
            $('#dlgUploadFm_CategoryPicture').form('submit', {
                url: '../../Handlers/Admin/ECShop/HandlerCategoryPicture.ashx',
                onSubmit: function (param) {
                    var hasNotFile = true;
                    $('#dlgUploadFm_CategoryPicture').find("[class*=filebox-f]").each(function () {
                        if ($.trim($(this).filebox('getValue')) == "") {
                            hasNotFile = false;
                        }
                    })
                    if (!hasNotFile) {
                        $.messager.alert('错误提示', "包含一个或多个未选择文件，无法上传，请检查！", 'error');
                        return false;
                    }
                    param.reqName = "OnUpload";
                    return true;
                },
                success: function (data) {
                    $.messager.progress('close');
                    var data = eval('(' + data + ')');
                    if (!data.success) {
                        $.messager.alert('错误提示', data.message, 'error');
                        return false;
                    }
                    jeasyuiFun.show("温馨提醒", data.message);
                    $("#dlgCategoryPictureUpload").dialog('close');

                    CategoryPicture.Init();
                }
            });
        }
        catch (e) {
            $.messager.progress('close');
            $.messager.alert('错误提醒', e.name + ": " + e.message, 'error');
        }
    }
}