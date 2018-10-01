
var ListService = {
    Url: "",
    Init: function () {
        this.LoadTree();
    },
    LoadTree: function () {
        var t = $("#treeCt");

        $.ajax({
            url: "/ScriptServices/AdminService.asmx/GetTreeJsonForServiceItem",
            type: "post",
            data: "{}",
            contentType: "application/json; charset=utf-8",
            beforeSend: function () {
                $("#dlgWaiting").dialog('open');
            },
            complete: function () {
                $("#dlgWaiting").dialog('close');
            },
            success: function (json) {
                var jsonData = (new Function("", "return " + json.d))();
                t.tree({
                    data: jsonData,
                    animate: true,
                    onContextMenu: function (e, node) {
                        e.preventDefault();
                        $(this).tree('select', node.target);
                        $('#mmTree').menu('show', {
                            left: e.pageX,
                            top: e.pageY
                        });
                    },
                    onSelect: ListService.OnTreeSelect
                })
                ListService.OnCurrExpand(t);
            }
        });
    },
    OnTreeSelect: function (node) {
        if (node) {
            $("#hCurrExpandNode").val(node.id);
            if (node.attributes.HasVote == "True") {
                ListServiceVote.LoadDg("dgVote", node.id);
            }
            else {
                $("#dgVote").datagrid('loadData', []);
            }
            if (node.attributes.HasContent == "True") {
                ListServiceContent.LoadDg("dgContent", node.id);
            }
            else {
                $("#dgContent").datagrid('loadData', []);
            }
            if (node.attributes.HasLink == "True") {
                ListServiceLink.LoadDg("dgLink", node.id);
            }
            else {
                $("#dgLink").datagrid('loadData', []);
            }
        }
    },
    FPicture: function (value, row, index) {
        if (value == undefined || value == "") return "";
        return "<img src=\"" + value + "\" alt=\"图片\" />";
    },
    OnCurrExpand: function (t) {
        var root = t.tree('getRoot');
        if (root) {
            var childNodes = t.tree('getChildren', root.target);
            if (childNodes && childNodes != undefined && (childNodes.length > 0)) {
                var cnLen = childNodes.length;
                for (var i = 0; i < cnLen; i++) {
                    t.tree('collapse', childNodes[i].target);
                }
            }
        }
        var currNode = t.tree('find', $("#hCurrExpandNode").val());
        if (currNode) {
            t.tree('select', currNode.target);
            ListService.OnExpand(t, currNode);
        }
    },
    OnExpand: function (t, node) {
        if (node) {
            t.tree('expand', node.target);
            var pNode = t.tree('getParent', node.target);
            if (pNode) {
                ListService.OnExpand(t, pNode);
            }
        }
    }
}