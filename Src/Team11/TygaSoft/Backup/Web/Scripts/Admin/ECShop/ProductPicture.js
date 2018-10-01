
var ProductPicture = {
    IsSelectSingle: true,
    Init: function () {
        var parmsData = { reqName: "GetJsonForDatagrid", page: 1, rows: 10 };
        ProductPicture.BindPicture(parmsData);
    },
    BindPicture: function (parms) {
        $.get("../../Handlers/Admin/ECShop/HandlerProductPicture.ashx", parms, function (data) {
            var jsonData = eval("(" + data + ")");
            var productPictureBox = $("#productPictureBox");
            productPictureBox.children("div").filter(":not(:first)").remove();
            var row = productPictureBox.children("div").eq(0);
            $.map(jsonData.rows, function (item) {
                var newRow = row.clone(true);
                newRow.appendTo(productPictureBox);
                newRow.find("img").attr("src", item.MPicture);
                newRow.find("input[type=hidden]").val(item.Id);
                newRow.show();
            })

            if (ProductPicture.IsSelectSingle) {
                ProductPicture.PictureSingleClick();
            }
            else {
                ProductPicture.PictureMultiClick();
            }

            if (jsonData.total > 10) {
                $('#productPicturePager').pagination({
                    total: jsonData.total,
                    pageSize: 10,
                    onSelectPage: function (pageNumber, pageSize) {
                        var parmsData = { reqName: "GetJsonForDatagrid", page: pageNumber, rows: pageSize };
                        ProductPicture.BindPicture(parmsData);
                    }
                });
            }
        })
    },
    PictureSingleClick: function () {
        $("#productPictureBox").children().bind("click", function () {
            $(this).addClass("curr").siblings().removeClass("curr");
        })
    },
    PictureMultiClick: function () {
        $("#productPictureBox").children().bind("click", function () {
            $(this).addClass("curr");
            var firstCol = $("#imgProductOther").children().eq(0);
            var newCol = firstCol.clone(true);
            newCol.appendTo($("#imgProductOther"));
            newCol.find("img").attr("src", $(this).find("img").attr("src"));
            newCol.find("input[type=hidden]").val($(this).find("input[type=hidden]").val());
            newCol.show();
        })
    },
    CreateDlgProductPicture: function () {
        var w = $(window).width() * 0.9;
        var h = $(window).height() * 0.9;
        $("#dlgProductPicture").dialog({
            title: '商品图片相册',
            width: w,
            height: h,
            closed: true,
            modal: true,
            buttons: [{
                id: 'btnSelectProductPicture',
                text: '确定',
                iconCls: 'icon-ok',
                handler: function () {
                    var selectPic = $("#productPictureBox").children().filter("[class*=curr]");
                    var picUrl = $.trim(selectPic.find("img").attr("src"));
                    if (picUrl == "") {
                        $.messager.alert('错误提醒', "请选择一张图片作为商品主图片", 'error');
                        return false;
                    }
                    $("#imgProduct").attr("src", picUrl);
                    $("#imgProduct").parent().find("input[type=hidden]").val(selectPic.find("input[type=hidden]").val());
                    $("#dlgProductPicture").dialog('close');
                }
            }, {
                id: 'btnCancelSelectSizePicture',
                text: '取 消',
                iconCls: 'icon-cancel',
                handler: function () {
                    $("#dlgProductPicture").dialog('close');
                }
            }],
            toolbar: [{
                text: '上 传',
                iconCls: 'icon-add',
                handler: function () {
                    ProductPicture.DlgUpload();
                }
            }]
        })
    },
    DlgSingle: function () {
        ProductPicture.IsSelectSingle = true;
        $("#dlgProductPicture").dialog('refresh', '/Templates/DlgProductPicture.htm');
        $("#dlgProductPicture").dialog('open');
        ProductPicture.Init();
    },
    DlgMulti: function () {
        ProductPicture.IsSelectSingle = false;
        $("#dlgProductPicture").dialog('refresh', '/Templates/DlgProductPicture.htm');
        $("#dlgProductPicture").dialog('open');
        ProductPicture.Init();

    },
    CreateDlgUploadPicture: function () {
        var h = $(window).height() * 0.9;
        $("#dlgUploadProductPicture").dialog({
            title: '上传商品文件',
            width: 700,
            height: h,
            closed: true,
            modal: true,
            buttons: [{
                id: 'btnUploadProductPicture',
                text: '上 传',
                iconCls: 'icon-ok',
                handler: function () {
                    ProductPicture.OnUpload();
                }
            }, {
                id: 'btnCancelProductPicture',
                text: '取 消',
                iconCls: 'icon-cancel',
                handler: function () {
                    $("#dlgUploadProductPicture").dialog('close');
                }
            }],
            toolbar: [{
                id: 'btnAddProductFile',
                text: '添 加',
                iconCls: 'icon-add',
                handler: function () {
                    $("#dlgUploadFm").find("input").each(function () {
                        $(this).attr("id", "productPicture_" + $(this).attr("id") + "");
                    })
                    $("#dlgUploadFm").attr("id", "dlgUploadFm_ProductPicture");
                    var currDlgFm = $("#dlgUploadFm_ProductPicture");

                    var rowLen = currDlgFm.children("div").length;
                    rowLen++;
                    var newRow = $("<div class=\"mb10\"><input type=\"text\" id=\"productPicture_file" + rowLen + "\" name=\"productPicture_file" + rowLen + "\" style=\"width:500px;\" /><a href=\"#\" onclick=\"$(this).parent().remove();return false;\" style=\"margin-left:10px;\">删 除</a></div>");
                    currDlgFm.append(newRow);
                    newRow.find("#productPicture_file" + rowLen + "").filebox({
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
        $("#dlgUploadProductPicture").dialog('refresh', '/Templates/UploadPicture.htm');
        $("#dlgUploadProductPicture").dialog('open');
    },
    OnUpload: function () {
        try {
            $.messager.progress({
                title: '请稍等',
                msg: '正在执行...'
            });
            $('[id*=dlgUploadFm]').form('submit', {
                url: '../../Handlers/Admin/ECShop/HandlerProductPicture.ashx',
                onSubmit: function (param) {
                    var hasNotFile = true;
                    $('[id*=dlgUploadFm]').find("[class*=filebox-f]").each(function () {
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
                    $("#dlgUploadProductPicture").dialog('close');
                    ProductPicture.Init();
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
    }
}