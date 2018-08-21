
var ServicePicture = {
    IsSelectSingle: true,
    Init: function () {
        var parmsData = { reqName: "GetJsonForDatagrid", page: 1, rows: 10 };
        this.BindPicture(parmsData);
    },
    BindPicture: function (parms) {
        $.get("../../Handlers/Admin/Serve/HandlerServicePicture.ashx", parms, function (data) {
            var jsonData = eval("(" + data + ")");
            var servicePictureBox = $("#servicePictureBox");
            servicePictureBox.children("div").filter(":not(:first)").remove();
            var row = servicePictureBox.children("div").eq(0);
            $.map(jsonData.rows, function (item) {
                var newRow = row.clone(true);
                newRow.appendTo(servicePictureBox);
                newRow.find("img").attr("src", item.MPicture);
                newRow.find("input[type=hidden]").val(item.Id);
                newRow.show();
            })

            if (ServicePicture.IsSelectSingle) {
                ServicePicture.PictureSingleClick();
            }
            else {
                ServicePicture.PictureMultiClick();
            }

            if (jsonData.total > 10) {
                $('#servicePicturePager').pagination({
                    total: jsonData.total,
                    pageSize: 10,
                    onSelectPage: function (pageNumber, pageSize) {
                        var parmsData = { reqName: "GetJsonForDatagrid", page: pageNumber, rows: pageSize };
                        ServicePicture.BindPicture(parmsData);
                    }
                });
            }
        })
    },
    PictureSingleClick: function () {
        $("#servicePictureBox").children().bind("click", function () {
            $(this).addClass("curr").siblings().removeClass("curr");
            var pictureId = $("#" + ServicePicture.PictureId + "");
            pictureId.attr("src", $(this).find("img").attr("src"));
            pictureId.parent().find("input[type=hidden]").val($(this).find("input[type=hidden]").val());
        })
    },
    PictureMultiClick: function () {
        $("#servicePictureBox").children().bind("click", function () {
            $(this).addClass("curr");
            var pictureId = $("#PictureId");
            var firstCol = $("#imgProductOther").children().eq(0);
            var newCol = firstCol.clone(true);
            newCol.appendTo($("#imgProductOther"));
            newCol.find("img").attr("src", $(this).find("img").attr("src"));
            newCol.show();
        })
    },
    PictureId: "",
    DlgSingle: function (id) {
        this.PictureId = id;
        this.IsSelectSingle = true;
        $("#dlgServicePicture").dialog('open');
    },
    DlgMulti: function () {
        this.PictureId = id;
        ServicePicture.IsSelectSingle = false;
        var w = $(window).width() * 0.9;
        var h = $(window).height() * 0.9;
        $("#dlgServicePicture").dialog({
            title: '服务相册',
            width: w,
            height: h,
            closed: false,
            href: '/Templates/DlgServicePictures.htm',
            modal: true,
            buttons: [{
                id: 'btnSelectMultiServicePicture',
                text: '确定',
                iconCls: 'icon-ok',
                handler: function () {
                    $("#dlgServicePicture").dialog('close');
                }
            }],
            toolbar: [{
                id: 'btnDlgUploadServicePicture',
                text: '上 传',
                iconCls: 'icon-add',
                handler: function () {
                    ServicePicture.DlgUpload()
                }
            }]
        })
    },
    DlgUpload: function () {
        var h = $(window).height() * 0.9;
        $("#dlgUploadServicePicture").dialog({
            title: '上传文件',
            width: 700,
            height: h,
            closed: false,
            href: '/Templates/UploadPicture.htm',
            modal: true,
            buttons: [{
                id: 'btnUploadServicePicture',text: '上 传',iconCls: 'icon-ok',handler: function () {
                    ServicePicture.OnUpload();
                }
            }, {
                id: 'btnCancelUploadServicePicture',text: '取 消',iconCls: 'icon-cancel',handler: function () {
                    $("#dlgUploadServicePicture").dialog('close');
                }
            }],
            toolbar: [{
                id: 'btnAddServiceFile', text: '添 加', iconCls: 'icon-add', handler: function () {
                    var rowLen = $("#dlgUploadFm").children("div").length;
                    rowLen++;
                    var newRow = $("<div class=\"mb10\"><input type=\"text\" id=\"file" + rowLen + "\" name=\"file" + rowLen + "\" style=\"width:500px;\" /><a href=\"#\" onclick=\"$(this).parent().remove();return false;\" style=\"margin-left:10px;\">删 除</a></div>");
                    $("#dlgUploadFm").append(newRow);
                    newRow.find("#file" + rowLen + "").filebox({
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
            $('#dlgUploadFm').form('submit', {
                url: '../../Handlers/Admin/Serve/HandlerServicePicture.ashx',
                onSubmit: function (param) {
                    var hasNotFile = true;
                    $('#dlgUploadFm').find("[class*=filebox-f]").each(function () {
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
                    var jsonData = eval('(' + data + ')');
                    if (!jsonData.success) {
                        $.messager.alert('错误提示', jsonData.message, 'error');
                        return false;
                    }
                    jeasyuiFun.show("温馨提醒", jsonData.message);
                    $("#dlgUploadServicePicture").dialog('close');

                    ServicePicture.Init();
                }
            });
        }
        catch (e) {
            $.messager.progress('close');
            $.messager.alert('错误提醒', e.name + ": " + e.message, 'error');
        }
    }
}