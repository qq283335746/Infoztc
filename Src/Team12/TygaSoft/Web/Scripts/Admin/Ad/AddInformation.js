

var AddInformation = {
    Init: function () {
    },
    GetMyData: function (clientId) {
        var myData = $("#" + clientId + "").html();
        return eval("(" + myData + ")");
    },
    DlgSingleForMutil: function () {
        DlgSelectPicture.DlgOpenId = "dlgMutilSelectPicture";
        DlgSelectPicture.DlgSingle('InformationPicture');
    },
    SetMutilPicture: function () {
        if ($('#imgContentPicture>div:not(:first)').length == 3) {
            $.messager.progress('close');
            $.messager.alert('错误提示', '图片必须小于等于3张！', 'error');
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
    },
    OnSave: function () {
        try {
            $.messager.progress({
                title: '请稍等',
                msg: '正在执行...'
            });
            $('#form1').form('submit', {
                url: '/h/tinfor.html',
                onSubmit: function (param) {
                    var isValid = $(this).form('validate');
                    if (!isValid) {
                        $.messager.progress('close');
                    }

                    var error = "";
                    var pictureId = "";

                    param.reqName = "SaveInformation";
                    param.content = editor_content.html().replace(/</g, "&lt;");

                    param.rdViewType = $('input[name="ctl00$cphMain$rdViewType"]:checked').val();
                    param.isDisable = $('input[name="ctl00$cphMain$rdIsDisable"]:checked').val();
                    param.isPush = $('input[name="ctl00$cphMain$rdIsPush"]:checked').val();

                    $("#imgContentPicture>div:not(:first)").each(function () {
                        var sContentPictureId = $.trim($(this).find("input[name=PicId]").val());
                        if (sContentPictureId == "") {
                            error = "包含有未选择图片的框，请检查";
                            return;
                        }
                        pictureId += sContentPictureId + ",";
                    });

                    param.pictureId = pictureId;

                    if ($("#hId").val() != "") {
                        $("#txtContent").text("");
                    }

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
                    setTimeout("window.location = \"tinfor.html?asId=" + $("#asId").val() + "\";", 500);
                }
            });
        }
        catch (e) {
            $.messager.alert('错误提醒', e.name + ": " + e.message, 'error');
        }
    }
} 