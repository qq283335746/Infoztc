
var MenusFun = {
    Init: function () {
        MenusFun.SelectCurrent();
        MenusFun.Hover();
    },
    Hover: function () {
        $(".nav a").hover(function () {
            $(this).addClass("hover").siblings().removeClass("hover");
        }, function () {
            $(this).removeClass("hover")
        })
    },
    SelectCurrent: function () {
        var currMenu = $("#SitePaths>span:last").text();
        $(".nav a").filter(":contains('" + currMenu + "')").addClass("curr").siblings().removeClass("curr");
    }
};

var UserMenus = {
    Init: function () {
        UserMenus.TreeLoad();
    },
    TreeLoad: function () {
        var t = $("#eastTree");
        $.ajax({
            url: "/ScriptServices/UsersService.asmx/GetTreeJsonForMenu",
            type: "post",
            data: "{}",
            contentType: "application/json; charset=utf-8",
            success: function (json) {
                var jsonData = (new Function("", "return " + json.d))();
                t.tree({
                    data: jsonData,
                    formatter: function (node) {
                        if (node.id.length > 0) {
                            return "<a href=\"" + node.id + "\">" + node.text + "</a>";
                        }
                        return node.text;
                    },
                    animate: true
                })
                UserMenus.SelectCurrent();
                //t.children().children("div:first").hide();
            }
        });
    },
    SelectCurrent: function () {
        var currMenu = $("#SitePaths>span:last").text();
        $("#westTree").find("a").each(function () {
            if ($(this).text() == currMenu) {
                $(this).parent().parent().addClass("bg_curr");
            }
        })
    }
};

var AdminMenus = {

    Init: function () {
        AdminMenus.TreeLoad();
    },
    TreeLoad: function () {
        var t = $("#eastTree");
        $.ajax({
            url: "/ScriptServices/AdminService.asmx/GetTreeJsonForMenu",
            type: "post",
            data: "{}",
            contentType: "application/json; charset=utf-8",
            success: function (json) {
                var jsonData = (new Function("", "return " + json.d))();
                t.tree({
                    data: jsonData,
                    formatter: function (node) {
                        if (node.id.length > 0) {
                            return "<a href=\"" + node.id + "\">" + node.text + "</a>";
                        }
                        return node.text;
                    },
                    animate: true
                })
                AdminMenus.SelectCurrent();
                //t.children().children("div:first").hide();
            }
        });
    },
    SelectCurrent: function () {
        var currMenu = $("#SitePaths>span:last").text();
        $("#westTree").find("a").each(function () {
            if ($(this).text() == currMenu) {
                $(this).parent().parent().addClass("bg_curr");
            }
        })
    } 
};