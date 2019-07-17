

var AddActivityPlayer = {
    Init: function () {

    },
    GetMyData: function (clientId) {
        var myData = $("#" + clientId + "").html();
        return eval("(" + myData + ")");
    },
    OnSave: function () {
        try {
            $.messager.progress({
                title: '请稍等',
                msg: '正在执行...'
            });
            $('#form1').form('submit', {
                url: '/h/tactivity.html',
                onSubmit: function (param) {

                    var isValid = $(this).form('validate');
                    if (!isValid) {
                        $.messager.progress('close');
                    }
                    var error = "";
                    var pictureId = "";

                    param.reqName = "SaveActivityPlayerNew";

                    if ($("#hImgSinglePictureId").val() == "") {
                        $.messager.progress('close');
                        $.messager.alert('错误提示', "头像不能为空！", 'error');
                        return false;
                    }

                    pictureId += $("#hImgSinglePictureId").val() + ",";
                    param.isDisable = $('input[type="radio"]:checked').val();

                    if ($("#imgContentPicture>div:not(:first)").length < 2) {
                        $.messager.progress('close');
                        $.messager.alert('错误提示', "图片必须大于等于2张！", 'error');
                        return false;
                    }

                    $("#imgContentPicture>div:not(:first)").each(function () {
                        var sContentPictureId = $.trim($(this).find("input[name=PicId]").val());
                        if ($("#hImgSinglePictureId").val() == sContentPictureId) {
                            error = "图片中有和头像是同一张图片，请检查！";
                            return;
                        }
                        if (sContentPictureId == "") {
                            error = "包含有未选择图片的框，请检查";
                            return;
                        }
                        pictureId += sContentPictureId + ",";
                    });

                    param.pictureId = pictureId;
                    if (error != "") {
                        $.messager.progress('close');
                        $.messager.alert('错误提示', error, 'error');
                        return false;
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
                    setTimeout("window.location = \"gactivity.html?asId=" + $("#asId").val() + "\";", 500);
                }
            });
        }
        catch (e) {
            $.messager.progress('close');
            $.messager.alert('错误提醒', e.name + ": " + e.message, 'error');
        }
    },
    DlgSingle: function () {
        DlgSelectPicture.DlgOpenId = "";
        DlgSelectPicture.DlgSingle('ActivityPlayerPhotoPicture');
    },
    DlgSingleForMutil: function () {
        DlgSelectPicture.DlgOpenId = "dlgMutilSelectPicture";
        DlgSelectPicture.DlgSingle('ActivityPlayerPhotoPicture');
    },
    SetMutilPicture: function () {
        if ($('#imgContentPicture>div:not(:first)').length == 6) {
            $.messager.progress('close');
            $.messager.alert('错误提示', '图片必须小于等于6张！', 'error');
            return false;
        }
        var data = dlgMutilSelectPicture.GetPicSelect();
        if (data.length > 0) {
            var imgEle = $("#imgContentPicture");
            var firstCol = imgEle.children().eq(0);
            for (var i = 0; i < data.length; i++) {
                var arr = data[i].split(",");
                var hasExist = false;
                imgEle.find("input[type=hidden]").each(function () {
                    if ($(this).val() == arr[0]) {
                        hasExist = true;
                        return false;
                    }
                })
                if (!hasExist) {
                    var newCol = firstCol.clone(true);
                    newCol.appendTo(imgEle);
                    newCol.find("img").attr("src", arr[1]);
                    newCol.find("input[type=hidden]").val(arr[0]);
                    newCol.show();
                }
            }
        }
    }
} 