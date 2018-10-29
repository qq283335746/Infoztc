
var tAnimate;
var pAnimate;
var isBetAnimate = false;
var speed = 1;
$(function () {
    PlayErnie.Init();
    $("#stick").click(function () {
        if (!PlayErnie.IsUserCanBet()) return false;
        if (isBetAnimate) return false;
        isBetAnimate = true;
        if (PlayErnie.IsCanBet()) {
            try {
                $("#stick").hide().next().show();
                var priceLi = $("#priceBox li");
                pAnimate = setInterval(function () {
                    PlayErnie.RndAnimate();
                }, speed)
                $("#lbRemainTime").prev().text("抽奖中...");
                $.get("/h/ta.html", { reqName: 'GetBetResult' }, function (data) {
                    setTimeout(function () {
                        clearInterval(pAnimate);
                        $("#winPrice").show();
                        var liArr = $("#winPrice li");
                        if (!data.success) {
                            liArr.eq(0).find("span").text("获得金币0个");
                            liArr.eq(1).find("span").text("获得元宝0个");
                        }
                        else {
                            priceLi.eq(0).find("b").text(data.gold);
                            priceLi.eq(1).find("b").text(data.silver);
                            priceLi.eq(2).find("b").text(data.times);
                            var totalGold = parseInt(data.gold) * parseInt(data.times);
                            var totalSilver = parseInt(data.silver) * parseInt(data.times);
                            liArr.eq(0).find("span").text("获得金币" + totalGold + "个");
                            liArr.eq(1).find("span").text("获得元宝" + totalSilver + "个");
                        }

                        if ($.trim(data.message) != "") {
                            $("#lbRemainTime").prev().text(data.message);
                        }
                        else {
                            $("#lbRemainTime").prev().text("开始抽奖");
                        }
                        $("#stick").show().next().hide();
                        speed = 1;
                        isBetAnimate = false;
                        
                        if (data.remainTimes != null && $.trim(data.remainTimes) != "") {
                            $("#myDataAppend").attr("remainTimes", data.remainTimes);
                        }

                    }, 1000)
                }, "json")

            }
            catch (e) {
                //clearInterval(tAnimate);
                clearInterval(pAnimate);
                $("#stick").show().next().hide();
                speed = 1;
                isBetAnimate = false;
            }
        }

    });

    //clearInterval(tAnimate);					  

})

