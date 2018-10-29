
var AddProduct = {
    Init: function () {
        this.SetForm();
    },
    GetMyData: function (clientId) {
        var myData = $("#" + clientId + "").html();
        return eval("(" + myData + ")");
    },
    SetForm: function () {
        if ($("#hId").val() != "") {
            var myDataJson = AddProduct.GetMyData("myDataForModel");
            $.map(myDataJson, function (item) {
                if ($.trim(item.ImageUrl) != "") {
                    $("#imgProduct").attr("src", item.ImageUrl);
                    $("#imgProduct").next().val(item.ProductPictureId);
                }
                if ($.trim(item.OtherPicture) != "") {
                    //var otherPictureJson = eval("(" + item.OtherPicture + ")");
                    $.map(item.OtherPicture, function (opItem) {
                        var firstCol = $("#imgProductOther").children().eq(0);
                        var newCol = firstCol.clone(true);
                        newCol.appendTo($("#imgProductOther"));
                        newCol.find("img").attr("src", opItem.MPicture);
                        newCol.find("input[type=hidden]").val(opItem.Id);
                        newCol.show();
                    })
                }
                if ($.trim(item.SizeItem) != "") {
                    //var sizeItemJson = eval("(" + item.SizeItem + ")");
                    $.map(item.SizeItem, function (siItem) {
                        var firstCol = $("#sizeWrap").children().eq(0);
                        var newCol = firstCol.clone(true);
                        newCol.appendTo($("#sizeWrap"));
                        newCol.find("input[type=text]").val(siItem.SizeName);
                        newCol.show();
                    })
                }
                if ($.trim(item.SizePicture) != "") {
                    $("#imgSize").attr("src", item.SizePicture);
                }
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
                if (item.CustomMenuId != "") {
                    $("#cbbMenu").combobox('setValue', item.CustomMenuId);
                }
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
    OnSave: function () {
        try {
            $.messager.progress({
                title: '请稍等',
                msg: '正在执行...'
            });
            $('#form1').form('submit', {
                url: '../../Handlers/Admin/ECShop/HandlerProduct.ashx',
                onSubmit: function (param) {
                    var isValid = $(this).form('validate');
                    if (!isValid) {
                        $.messager.progress('close'); 
                    }
                    param.reqName = "SaveProduct";
                    param.txtContent = editor_content.html().replace(/</g, "&lt;");
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
                    param.customMenuId = $('#cbbMenu').combobox('getValue');

                    var otherPicJson = "";
                    var otherPicIndex = 0;
                    $("#imgProductOther").find("input[type=hidden]").each(function () {
                        if ($.trim($(this).val()) != "") {
                            otherPicIndex++;
                            if (otherPicIndex > 1) otherPicJson += ",";
                            otherPicJson += $(this).val();
                        }
                    })
                    param.otherPic = otherPicJson;

                    var sizeItemJson = "[";
                    var sizeItemIndex = 0;
                    $("#sizeWrap").find("input[type=text]").each(function () {
                        if ($.trim($(this).val()) != "") {
                            sizeItemIndex++;
                            if (sizeItemIndex > 1) sizeItemJson += ",";
                            sizeItemJson += "{\"SizeName\":\"" + $.trim($(this).val()) + "\"}";
                        }
                    })
                    sizeItemJson += "]";
                    param.sizeItem = sizeItemJson;

                    param.imageUrl = $.trim($("#imgProduct").attr("src"));
                    param.productPictureId = $("#imgProduct").next().val();
                    param.sizePicture = $.trim($("#imgSize").attr("src"));

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
                    jeasyuiFun.show("温馨提醒", data.message);
                    setTimeout(5000, window.location = "at.html");
                }
            });
        }
        catch (e) {
            $.messager.progress('close');
            $.messager.alert('错误提醒', e.name + ": " + e.message, 'error');
        }
    }
} 