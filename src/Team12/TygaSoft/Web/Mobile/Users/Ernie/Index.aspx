<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="TygaSoft.Web.Mobile.Users.Ernie.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1, user-scalable=no, minimal-ui" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <meta name="format-detection"content="telephone=no, email=no" />
    <title>摇奖</title>

    <link rel="stylesheet"  type="text/css" href="../../../Styles/Ernie/css/base.css" />
    <link rel="stylesheet"  type="text/css" href="../../../Styles/Ernie/css/all.css" />
    <script src="../../../Scripts/Admin/Ernie/zepto.min.js"></script>
</head>
<body>
    <div class="wrap">
        <form id="form1" runat="server">
            <div id="winPrice" class="win_box">
            <ul>
	            <li><i class="gold"></i><span>获得金币0个</span></li>
	            <li><i class="gold_ingots"></i><span>获得元宝0个</span></li>
	        </ul>
            </div>
            <div class="prize_box">
            <div class="prize_t"></div>
	        <div></div>
	        <div class="prize_c">
	            <div class="stick_up" id="stick"></div>
                <div class="stick_up down" style="display:none;"></div>
	            <div id="priceBox" class="prize_con">
	            <ul>
		            <li><i class="gold"></i><b>0</b></li>
		            <li><i class="gold_ingots"></i><b>0</b></li>
		            <li><i class="multiple"></i><b>0</b></li>
		        </ul>
	            </div>
	            <div class="start_time"><span>开始倒计时</span><span id="lbRemainTime" class="time_panel">00:00:00</span></div>
	        </div>
	        <div></div>
	        <div class="prize_b">
	            <div class="notice">
	            <h2>抽奖公告</h2>
		        <marquee direction="up" height="30" scrollamount="1">
                    <p>1这里放公告内容这里放公告内容这里放公告内容这里容这里放公告内容这里放公告内容这里放公告内容这里容</p>
                    <p>2这里放公告内容这里放公告内容这里放公告内容这里容这里放公告内容这里放公告内容这里放公告内容这里容</p>
                    <p>3这里放公告内容这里放公告内容这里放公告内容这里容这里放公告内容这里放公告内容这里放公告内容这里容</p>
                    <p>4这里放公告内容这里放公告内容这里放公告内容这里容这里放公告内容这里放公告内容这里放公告内容这里容</p>
                    <p>5这里放公告内容这里放公告内容这里放公告内容这里容这里放公告内容这里放公告内容这里放公告内容这里容</p>
                    <p>6这里放公告内容这里放公告内容这里放公告内容这里容这里放公告内容这里放公告内容这里放公告内容这里容</p>
                    <p>7这里放公告内容这里放公告内容这里放公告内容这里容这里放公告内容这里放公告内容这里放公告内容这里容</p>

		        </marquee>
	            </div>
	        </div>
            </div>

            <input type="hidden" id="hTotalMls" runat="server" clientidmode="Static" value="0" />
            
            <asp:Literal ID="ltrMyData" runat="server"></asp:Literal>

        </form>
    </div>

    <script src="../../../Scripts/Admin/Ernie/PlayErnie.js"></script>
    <script src="../../../Scripts/Admin/Ernie/all.js"></script>

</body>
</html>
