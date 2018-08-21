

var AddPushMsg = {
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
                url: '/h/tpush.html',
                onSubmit: function (param) {
                    var isValid = $(this).form('validate');
                    if (!isValid) {
                        $.messager.progress('close');
                    }

                    var error = "";

                    param.reqName = "SavePushMsg";

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
                    setTimeout("window.location = \"tpush.html?asId=" + $("#asId").val() + "\";", 500);
                }
            });
        }
        catch (e) {
            $.messager.alert('错误提醒', e.name + ": " + e.message, 'error');
        }
    },
    OpenInforAdWin: function (id) {
        $("#RelInforAd").dialog("open");
        this.BindUserList();
    },
    AddPushUser: function () {
        var outHtml = "";
        $("#selectList option:selected").each(function () {
            if ($("#selectedList option[value='" + $(this).val() + "']").length == 0) {
                outHtml += "<option value=\"" + $(this).val() + "\">" + $(this).text() + "</option>";
            }
        });
        $("#selectedList").append(outHtml);
    },
    DelPushUser: function () {
        var selOpt = $("#selectedList option:selected");
        selOpt.remove();
    },
    BindUserList: function () {
        gPageNumber = 1;
        $("#userName").val("");
        var pageStr = "?pageIndex=" + gPageNumber + "&pageSize=" + gPageSize + "";
        this.GetUserList(pageStr);
    },
    GetUserList: function (queryStr) {
        $.post("/h/tpush.html" + queryStr, { "reqName": "GetUserList" }, function (data) {
            var dataDsObj = eval("(" + data + ")"); //转换为json对象
            var dataObj = dataDsObj.Table;
            var outHtml = "";
            for (var i = 0; i < dataObj.length; i++) {
                outHtml += "<option value=\"" + dataObj[i].UserId + "\">" + dataObj[i].UserName + "</option>";
            }
            $("#selectList").html(outHtml);

            gTotalSize = dataDsObj.total;
            $("#page").pagination({
                total: gTotalSize,
                pageNumber: gPageNumber,
                pageSize: gPageSize,
                showRefresh: false,
                onSelectPage: function (pageNumber, pageSize) {
                    gPageNumber = pageNumber;
                    gPageSize = pageSize;
                    var userName = $("#userName").val();
                    if (userName.length == 0) {
                        var pageStr = "?pageIndex=" + pageNumber + "&pageSize=" + pageSize + "";
                    }
                    else {
                        var pageStr = "?userName=" + userName + "&pageIndex=" + pageNumber + "&pageSize=" + pageSize + "";
                    }
                    AddPushMsg.GetUserList(pageStr);
                }
            });
        });
    },
    GetSelectedUser: function () {
        var jsonData = "";
        $("#selectedList option").each(function () {
            jsonData += $(this).text() + ",";
        });
        if (jsonData.length > 0) {
            $("#ckAll").prop("checked", false);
            jsonData = jsonData.substring(0, jsonData.length - 1);
        } else {
            $("#ckAll").prop("checked", true);
        }

        $("#txtSendRange").text(jsonData);

    },
    SearchUserList: function () {
        gPageNumber = 1;
        var userName = $("#userName").val();
        queryStr = "?userName=" + userName + "&pageIndex=" + gPageNumber + "&pageSize=" + gPageSize + "";
        this.GetUserList(queryStr);
    }
} 