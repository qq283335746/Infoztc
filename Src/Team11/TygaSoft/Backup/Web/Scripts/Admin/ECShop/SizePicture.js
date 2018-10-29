
var SizePicture = {
    IsSelectSingle: true,
    Init: function () {
        var parmsData = { reqName: "GetJsonForDatagrid", page: 1, rows: 10 };
        SizePicture.BindPicture(parmsData);
    },
    BindPicture: function (parms) {
        $.get("../../Handlers/Admin/ECShop/HandlerSizePicture.ashx", parms, function (data) {
            var jsonData = eval("(" + data + ")");
            var sizePictureBox = $("#sizePictureBox");
            sizePictureBox.children("div").filter(":not(:first)").remove();
            var row = sizePictureBox.children("div").eq(0);
            $.map(jsonData.rows, function (item) {
                var newRow = row.clone(true);
                newRow.appendTo(sizePictureBox);
                newRow.find("img").attr("src", item.MPicture);
                newRow.find("input[type=hidden]").val(item.Id);
                newRow.show();
            })

            if (SizePicture.IsSelectSingle) {
                SizePicture.PictureSingleClick();
            }
            else {
                SizePicture.PictureMultiClick();
            }

            if (jsonData.total > 10) {
                $('#sizePicturePager').pagination({
                    total: jsonData.total,
                    pageSize: 10,
                    onSelectPage: function (pageNumber, pageSize) {
                        var parmsData = { reqName: "GetJsonForDatagrid", page: pageNumber, rows: pageSize };
                        SizePicture.BindPicture(parmsData);
                    }
                });
            }
        })
    },
    PictureSingleClick: function () {
        $("#sizePictureBox").children().bind("click", function () {
            $(this).addClass("curr").siblings().removeClass("curr");
        })
    },
    CreateDlgSizePicture: function () {
        var w = $(window).width() * 0.9;
        var h = $(window).height() * 0.9;
        $("#dlgSizePicture").dialog({
            title: '尺码表图片相册',
            width: w,
            height: h,
            closed: true,
            modal: true,
            buttons: [{
                id: 'btnSelectSizePicture',
                text: '确定',
                iconCls: 'icon-ok',
                handler: function () {
                    var selectPic = $("#sizePictureBox").children().filter("[class*=curr]");
                    var picUrl = $.trim(selectPic.find("img").attr("src"));
                    if (picUrl == "") {
                        $.messager.alert('错误提醒', "请选择一张图片作为尺码表", 'error');
                        return false;
                    }
                    $("#imgSize").attr("src", picUrl);
                    $("#imgSize").parent().find("input[type=hidden]").val(selectPic.find("input[type=hidden]").val());

                    $("#dlgSizePicture").dialog('close');
                }
            }, {
                id: 'btnCancelSelectSizePicture',
                text: '取 消',
                iconCls: 'icon-cancel',
                handler: function () {
                    $("#dlgSizePicture").dialog('close');
                }
            }],
            toolbar: [{
                id: 'btnDlgOpenSizePicture',
                text: '上 传',
                iconCls: 'icon-add',
                handler: function () {
                    SizePicture.DlgUpload()
                }
            }]
        })
    },
    DlgSingle: function () {
        SizePicture.IsSelectSingle = true;
        $("#dlgSizePicture").dialog('refresh', '/Templates/DlgSizePicture.htm');
        $("#dlgSizePicture").dialog('open');
        SizePicture.Init();
    },
    DlgMulti: function () {
        SizePicture.IsSelectSingle = false;
        $("#dlgSizePicture").dialog('refresh', '/Templates/DlgSizePicture.htm');
        $("#dlgSizePicture").dialog('open');
        SizePicture.Init();

    },
    CreateDlgUploadPicture: function () {
        var h = $(window).height() * 0.9;
        $("#dlgUploadSizePicture").dialog({
            title: '上传尺码文件',
            width: 700,
            height: h,
            closed: true,
            modal: true,
            href: '/Templates/UploadPicture.htm',
            buttons: [{
                id: 'btnUploadSizePicture',
                text: '上 传',
                iconCls: 'icon-ok',
                handler: function () {
                    SizePicture.OnUpload();
                }
            }, {
                id: 'btnCancelUploadSizePicture',
                text: '取 消',
                iconCls: 'icon-cancel',
                handler: function () {
                    $("#dlgUploadSizePicture").dialog('close');
                }
            }],
            toolbar: [{
                id: 'btnAddSizeFile',
                text: '添 加',
                iconCls: 'icon-add',
                handler: function () {
                    $("#dlgUploadFm").find("input").each(function () {
                        $(this).attr("id", "sizePicture_" + $(this).attr("id") + "");
                    })
                    $("#dlgUploadFm").attr("id", "dlgUploadFm_SizePicture");
                    var currDlgFm = $("#dlgUploadFm_SizePicture");

                    var rowLen = currDlgFm.children("div").length;
                    rowLen++;
                    var newRow = $("<div class=\"mb10\"><input type=\"text\" id=\"sizePicture_file" + rowLen + "\" name=\"sizePicture_file" + rowLen + "\" style=\"width:500px;\" /><a href=\"#\" onclick=\"$(this).parent().remove();return false;\" style=\"margin-left:10px;\">删 除</a></div>");
                    currDlgFm.append(newRow);
                    newRow.find("#sizePicture_file" + rowLen + "").filebox({
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
    DlgUpload: function () {
        //$("#dlgUploadSizePicture").dialog('refresh', '/Templates/UploadPicture.htm');
        $("#dlgUploadSizePicture").dialog('open');

    },
    OnUpload: function () {
        try {
            $.messager.progress({
                title: '请稍等',
                msg: '正在执行...'
            });
            $('#dlgUploadFm_SizePicture').form('submit', {
                url: '../../Handlers/Admin/ECShop/HandlerSizePicture.ashx',
                onSubmit: function (param) {
                    var hasNotFile = true;
                    $('#dlgUploadFm_SizePicture').find("[class*=filebox-f]").each(function () {
                        if ($.trim($(this).filebox('getValue')) == "") {
                            hasNotFile = false;
                        }
                    })
                    if (!hasNotFile) {
                        $.messager.alert('错误提示', "包含一个或多个未选择文件，无法上传，请检查！", 'error');
                        return false;
                    }
                    param.reqName = "OnUpload";

                    return hasNotFile;
                },
                success: function (data) {
                    $.messager.progress('close');
                    var data = eval('(' + data + ')');
                    if (!data.success) {
                        $.messager.alert('错误提示', data.message, 'error');
                        return false;
                    }
                    jeasyuiFun.show("温馨提醒", data.message);
                    $("#dlgUploadSizePicture").dialog('close');

                    SizePicture.Init();
                }
            });
        }
        catch (e) {
            $.messager.progress('close');
            $.messager.alert('错误提醒', e.name + ": " + e.message, 'error');
        }
    },
    FormatPicture: function (value, rowData, rowIndex) {
        if (value == undefined) return "";
        return "<img src=\"" + value + "\" alt=\"\" />";
    },
    AddInput: function () {
        var firstCol = $("#sizeBox").children().eq(0);
        var cloneCol = firstCol.clone(true);
        cloneCol.appendTo($("#sizeBox"));
        cloneCol.show();
    }
}