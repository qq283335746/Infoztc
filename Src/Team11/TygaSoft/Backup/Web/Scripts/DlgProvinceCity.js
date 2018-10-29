
var ProvinceCity = {
    Init: function () {
        this.GetEditData();
    },
    GetEditData: function () {
        if ($("#hId").val() != "") {
            var hProvinceCity = $.trim($("#lbHProvinceCity").text());
            if (hProvinceCity != "") {
                var arr = hProvinceCity.split("#");
                var arrLen = arr.length;
                if (arrLen > 0) {
                    ProvinceCity.Province = arr[0];
                }
                if (arrLen > 1) {
                    ProvinceCity.City = arr[1];
                }
                if (arrLen > 2) {
                    ProvinceCity.Country = arr[2];
                }
            }
        }
    },
    Province: "",
    City: "",
    Country: "",
    OnOpenDlg: function () {
        $("#dlgProvinceCity").dialog('open');
        ProvinceCity.InitTab();
    },
    InitTab: function () {
        var tabProvinceCity = $('#tabProvinceCity');

        if (!tabProvinceCity.tabs('exists', '省')) {
            tabProvinceCity.tabs('add', {
                title: '省',
                style: { paddingTop: 10 },
                content: '<div id="tabContent" class="pdlr10"></div>'
            });

            ProvinceCity.CreateProvince();
        }
        if (!tabProvinceCity.tabs('exists', '市')) {
            tabProvinceCity.tabs('add', {
                title: '市',
                selected: false,
                style: { paddingTop: 10 },
                content: '<div id="tabContent1" class="pdlr10"></div>'
            });
        }
        if (!tabProvinceCity.tabs('exists', '区')) {
            tabProvinceCity.tabs('add', {
                title: '区',
                selected: false,
                style: { paddingTop: 10 },
                content: '<div id="tabContent2" class="pdlr10"></div>'
            });
        }
    },
    CreateProvince: function () {
        $.get("/Handlers/HandlerProvinceCiy.ashx?reqName=GetJsonForProvince", function (data) {
            var selectId = "";
            var content = "<ul class=\"tab-region\">";
            var jsonData = eval("(" + data + ")");
            $.map(jsonData.rows, function (item) {
                if (ProvinceCity.Province != "" && item.Name == ProvinceCity.Province) {
                    selectId = item.Id;
                    content += "<li class=\"select\"><a href=\"#\" code=\"" + item.Id + "\" onclick=\"ProvinceCity.OnSelect(this);return false;\">" + item.Name + "</a></li>";
                }
                else {
                    content += "<li><a href=\"#\" code=\"" + item.Id + "\" onclick=\"ProvinceCity.OnSelect(this);return false;\">" + item.Name + "</a></li>";
                }
            })
            content += "</ul>";
            content += "<span class=\"clr\"></span>";
            $("#tabContent").html(content);

            if (selectId != "") {
                var nextIndex = 1;
                ProvinceCity.CreateChild(selectId, nextIndex);
                $('#tabProvinceCity').tabs('select', nextIndex);
            }
        })

    },
    CreateChild: function (parentId, tabIndex) {
        $.get("/Handlers/HandlerProvinceCiy.ashx?reqName=GetJsonByParentId&parentId=" + parentId + "", function (data) {
            var selectId = "";
            var content = "<ul class=\"tab-region\">";
            var jsonData = eval("(" + data + ")");
            $.map(jsonData.rows, function (item) {
                if (item.Name == ProvinceCity.City || item.Name == ProvinceCity.Country) {
                    selectId = item.Id;
                    content += "<li class=\"select\"><a href=\"#\" code=\"" + item.Id + "\" onclick=\"ProvinceCity.OnSelect(this);return false;\">" + item.Name + "</a></li>";
                }
                else {
                    content += "<li><a href=\"#\" code=\"" + item.Id + "\" onclick=\"ProvinceCity.OnSelect(this);return false;\">" + item.Name + "</a></li>";
                }
            })
            content += "</ul>";
            content += "<span class=\"clr\"></span>";

            $("#tabContent" + tabIndex + "").html(content);

            if (selectId != "") {
                console.log("selectId--"+selectId);
                var nextIndex = tabIndex + 1;
                ProvinceCity.CreateChild(selectId, nextIndex);
                $('#tabProvinceCity').tabs('select', nextIndex);
            }
        })
    },
    OnSelect: function (t) {
        var currAbtn = $(t);
        var tabProvinceCity = $("#tabProvinceCity");

        currAbtn.parent().addClass("select").siblings().removeClass("select");
        var parentId = currAbtn.attr("code");

        var tabsLen = tabProvinceCity.tabs('tabs').length;
        var tab = tabProvinceCity.tabs('getSelected');
        var selectIndex = tabProvinceCity.tabs('getTabIndex', tab);
        if (selectIndex < (tabsLen - 1)) {
            var nextIndex = selectIndex + 1;
            ProvinceCity.CreateChild(parentId, nextIndex);
            tabProvinceCity.tabs('select', nextIndex);
        }
        return false;
    },
    OnOk: function () {
        var s = "";
        var hs = "";
        var tabsLen = $('#tabProvinceCity').tabs('tabs').length;
        var index = 0;
        for (var i = 0; i < tabsLen; i++) {
            var tab = $('#tabProvinceCity').tabs('getTab', i);
            tab.find("li").each(function () {
                if ($(this).hasClass("select")) {
                    if (index > 0) {
                        s += " ";
                        hs += "#";
                    }

                    s += $(this).find("a").text() + " ";
                    hs += $(this).find("a").text() + "#";
                }
            })
        }
        $("#lbProvinceCity").text(s);
        $("#lbHProvinceCity").text(hs);
        if (hs == "") {
            $.messager.alert('错误提示', "请选择省市区", 'error');
            return false;
        }
        $("#lbtnSelect").find(".l-btn-text").text("修改");
        $("#dlgProvinceCity").dialog('close');
    }
}