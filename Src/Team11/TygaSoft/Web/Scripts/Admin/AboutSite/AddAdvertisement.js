
var AddAdvertisement = {
    OnTabSelect: function (title, index) {
        var currIndex = $("#lbTabSelectIndex").text();
        if (currIndex != index) {
            var adId = $.trim($("#hAdId").val());
            if (adId == "") {
                $.messager.alert('错误提示', '请先完成基本信息', 'error');
                return false;
            }

            switch (index) {
                case 0:
                    window.location = "/a/tad.html?adId=" + adId + "";
                    break;
                case 1:
                    window.location = "/a/yad.html?adId=" + adId + "";
                    break;
                default:
                    break;
            }
        }
    }
}