
var AddProduct = {
    Init: function () {
        this.SetForm();
    },
    GetMyData: function (clientId) {
        var myData = $("#" + clientId + "").html();
        return eval("(" + myData + ")");
    },
    FImageUrl: function (value, row, index) {
        if (value != undefined && value != "") {
            return "<img src=\"" + value + "\" alt=\"图片\" width=\"50\" height=\"50\" />";
        }
    },
    FImageAppendUrl: function (value, row, index) {
        if (value != undefined && value != "") {
            var arr = value.split(",");
            var pictureAppend = "";
            for (var i = 0; i < arr.length; i++) {
                pictureAppend += "<img src=\"" + arr[i] + "\" alt=\"图片\" width=\"50\" height=\"50\" class=\"mr10\" />";
            }
            return pictureAppend;
        }
    },
    FBool: function (value, row, index) {
        if (value != undefined && value != "true") {
            return "否";
        }
        return "是";
    },
    SetForm: function () {
        if ($("#hId").val() != "") {
            var myDataJson = AddProduct.GetMyData("myDataForModel");
            $.map(myDataJson, function (item) {

                if (item.CategoryId != "") {
                    var t = $("#cbtCategory").combotree('tree');
                    var currNode = t.tree('find', item.CategoryId);
                    if (currNode) {
                        t.tree('select', currNode.target);
                        $("#cbtCategory").combotree('setValue', item.CategoryId);
                    }
                }
                if (item.BrandId != "") {
                    var t = $("#cbtBrand").combotree('tree');
                    var currNode = t.tree('find', item.BrandId);
                    if (currNode) {
                        t.tree('select', currNode.target);
                        $("#cbtBrand").combotree('setValue', item.BrandId);
                    }
                }
                if (item.MenuId != "") {
                    $("#cbbMenu").combobox('setValue', item.MenuId);
                }

                $("input[name=rdIsEnable]").each(function () {
                    if ($(this).val() == item.IsEnable) {
                        $(this).attr("checked", "checked").siblings().removeAttr("checked");
                    }
                })
                $("input[name=rdIsDisable]").each(function () {
                    if ($(this).val() == item.IsDisable) {
                        $(this).attr("checked", "checked").siblings().removeAttr("checked");
                    }
                })
            })
        }
    },
    OnCategoryTreeLoadSuccess: function () {
        if ($("#hId").val() != "") {
            var myDataJson = AddProduct.GetMyData("myDataForModel");
            $.map(myDataJson, function (item) {
                if (item.CategoryId != "") {
                    var t = $("#cbtCategory").combotree('tree');
                    var currNode = t.tree('find', item.CategoryId);
                    if (currNode) {
                        t.tree('select', currNode.target);
                        $("#cbtCategory").combotree('setValue', item.CategoryId);
                    }
                }
            })
        }
    },
    OnBrandTreeLoadSuccess: function () {
        if ($("#hId").val() != "") {
            var myDataJson = AddProduct.GetMyData("myDataForModel");
            $.map(myDataJson, function (item) {
                if (item.BrandId != "") {
                    var t = $("#cbtBrand").combotree('tree');
                    var currNode = t.tree('find', item.BrandId);
                    if (currNode) {
                        t.tree('select', currNode.target);
                        $("#cbtBrand").combotree('setValue', item.BrandId);
                    }
                }
            })
        }
    },
    OnMenuTreeLoadSuccess: function () {
        if ($("#hId").val() != "") {
            var myDataJson = AddProduct.GetMyData("myDataForModel");
            $.map(myDataJson, function (item) {
                if (item.MenuId != "") {
                    var t = $("#cbtMenu").combotree('tree');
                    var currNode = t.tree('find', item.MenuId);
                    if (currNode) {
                        t.tree('select', currNode.target);
                        $("#cbtMenu").combotree('setValue', item.MenuId);
                    }
                }
            })
        }
    },
    OnSave: function () {
        try {
            $.messager.progress({
                title: '请稍等',
                msg: '正在执行...'
            });
            $('#form1').form('submit', {
                url: '/h/t.html',
                onSubmit: function (param) {
                    var isValid = $(this).form('validate');
                    if (!isValid) {
                        $.messager.progress('close');
                    }
                    param.reqName = "SaveProduct";

                    var categoryId = "";
                    var t = $("#cbtCategory").combotree('tree');
                    var node = t.tree("getSelected");
                    if (node) {
                        categoryId = node.id;
                    }
                    param.categoryId = categoryId;
                    var brandId = "";
                    var tBrand = $("#cbtBrand").combotree('tree');
                    var nodeBrand = tBrand.tree("getSelected");
                    if (nodeBrand) {
                        brandId = nodeBrand.id;
                    }
                    param.brandId = brandId;
                    param.menuId = $('#cbtMenu').combobox('getValue');
                    param.isEnable = $("input[name=rdIsEnable]:checked").val();
                    param.isDisable = $("input[name=rdIsDisable]:checked").val();

                    if ($("#hId").val() != "") {
                        $("#txtContent").text("");
                    }

                    return isValid;
                },
                success: function (data) {
                    $.messager.progress('close');
                    var data = eval('(' + data + ')');
                    if (!data.success) {
                        $.messager.alert('错误提示', data.message, 'error');
                        return false;
                    }
                    var productId = data.message;
                    jeasyuiFun.show("温馨提醒", "保存成功");
                    $("#hId").val(productId);
                    //                    setTimeout(function () {
                    //                        window.location = "ay.html?Id=" + productId + "";
                    //                    }, 500);
                }
            });
        }
        catch (e) {
            $.messager.progress('close');
            $.messager.alert('错误提醒', e.name + ": " + e.message, 'error');
        }
    },
    OnPictureClick: function () {
        DlgPictureSelect.DlgOpenId = "dlgSingleSelectProductPicture";
        DlgPictureSelect.DlgSingle('PictureProduct');
    },
    SetSinglePicture: function (imgEleId) {
        var data = dlgSingleSelectProductPicture.GetPicSelect();
        if (data.length > 0) {
            var arr = data[0].split(",");
            $("#" + imgEleId + "").attr("src", arr[1])
            $("#" + imgEleId + "").parent().find("input[type=hidden]").val(arr[0]);
            $("#dlgSingleSelectProductPicture").dialog('close');
        }
    }
} 