
var PlayErnie = {
    Init: function () {
        this.ShowRun();
        this.LoadPage();
        setTimeout(PlayErnie.ReqServerData, 60000);
    },
    LoadPage: function () {
        var myDataDiv = $("#myDataAppend");
        //开奖状态 1-正在开奖 0-非正在开奖 100-无效
        var status = parseInt(myDataDiv.attr("status"));
        switch (status) {
            case 1:
                $("#hTotalMls").val(0);
                $("#lbRemainTime").hide().prev().text("开始抽奖");
                break;
            case 0:
                $("#lbRemainTime").show().prev().text("开始倒计时");
                break;
            case 100:
                $("#lbRemainTime").hide().prev().text("抽奖结束");
                break;
            default:
                break;
        }
    },
    GetMyData: function (clientId) {
        var myData = $("#" + clientId + "").html();
        return eval("(" + myData + ")");
    },
    ShowRun: function () {
        var totalMls = 0;
        totalMls = $("#hTotalMls").val();
        totalMls = totalMls - 1000;
        if (totalMls < 0) totalMls = 0;
        $("#hTotalMls").val(totalMls);

        $("#lbRemainTime").text(PlayErnie.CreateTimeByTotalMls(totalMls));

        if (totalMls > 0) {
            setTimeout(arguments.callee, 1000);
        }
        else {
            var myDataDiv = $("#myDataAppend");
            if (parseInt(myDataDiv.attr("totalmls")) > 0) {
                myDataDiv.attr("status", "1");
                $("#lbRemainTime").hide().prev().text("开始抽奖");
            }
        }
    },
    CreateTimeByTotalMls: function (totalMls) {
        if (totalMls <= 0) {
            return "00:00:00";
        }
        var hour = Math.floor((totalMls / 3600000));
        totalMls = totalMls - 3600000 * hour;
        var minute = Math.floor((totalMls / 60000));
        totalMls = totalMls - minute * 60000;
        var second = Math.floor(totalMls / 1000);
        if (hour < 10) hour = "0" + hour;
        if (minute < 10) minute = "0" + minute;
        if (second < 10) second = "0" + second;

        return hour + ":" + minute + ":" + second + "";
    },
    ReqServerData: function () {
        try {
            $.get("/h/ta.html", { reqName: 'GetLatestForBet' }, function (data) {
                if (data.success) {
                    //开奖状态 1-正在开奖 0-非正在开奖 100-无效
                    var status = parseInt(data.status);

                    var myDataDiv = $("#myDataAppend");
                    myDataDiv.attr("status", data.status);
                    var isChange = false;
                    if (myDataDiv.attr("startTime") != data.startTime) {
                        isChange = true;
                        myDataDiv.attr("startTime", data.startTime);
                    }
                    if (myDataDiv.attr("endTime") != data.endTime) {
                        isChange = true;
                        myDataDiv.attr("endTime", data.endTime);
                    }
                    if (isChange) {
                        $("#hTotalMls").val(data.totalMls);
                        myDataDiv.removeAttr("remainTimes");
                    }

                    switch (status) {
                        case 1:
                            $("#hTotalMls").val(0);
                            $("#lbRemainTime").hide().prev().text("开始抽奖");
                            break;
                        case 0:
                            $("#lbRemainTime").show().prev().text("开始倒计时");
                            break;
                        case 100:
                            $("#lbRemainTime").hide().prev().text("抽奖结束");
                            break;
                        default:
                            break;
                    }
                }
            }, "json")
        }
        catch (e) {

        }
        setTimeout(arguments.callee, 60000);
    },
    IsCanBet: function () {
        var myDataDiv = $("#myDataAppend");
        if (myDataDiv.attr("status") == "1") {
            return true;
        }
        return false;
    },
    IsUserCanBet: function () {
        
        var myDataDiv = $("#myDataAppend");
        if (myDataDiv.attr("remainTimes") != null && $.trim(myDataDiv.attr("remainTimes")) != "") {
            var remainTimes = 0;
            remainTimes = parseInt(myDataDiv.attr("remainTimes"));
            if (remainTimes < 1) {
                $("#lbRemainTime").prev().text("摇奖机会已用完");
                return false;
            }
        }
        return true;
    },
    RndAnimate: function () {
        var priceLi = $("#priceBox li");
        var rndGold = Math.round(Math.random() * 10);
        var rndSilver = parseInt(Math.random() * 5 + 1);
        var rndTimes = parseInt(Math.random() * 3 + 1);
        priceLi.eq(0).find("b").text(rndGold);
        priceLi.eq(1).find("b").text(rndSilver);
        priceLi.eq(2).find("b").text(rndTimes);
    }
}