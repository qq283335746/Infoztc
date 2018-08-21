<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DlgSelectPicture.aspx.cs" Inherits="TygaSoft.Web.Templates.Admin.DlgSelectPicture" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="dlgSelectPictureFm" runat="server">
        <div class="easyui-layout" data-options="border:false, width:$(window).width()*0.7,height:$(window).height()*0.7">
            <div id="PictureBox" data-options="region:'center',title:'',border:false">
                <asp:Repeater ID="rpData" runat="server">
                    <ItemTemplate>
                        <div class="row_col w150">
                            <img src='<%#Eval("MPicture")%>' alt="图片" width="150px" height="150px" code='<%#Eval("Id")%>' />
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
            <div data-options="region:'south',title:'',border:false," style="height:50px;">
                <div id="easyPager"></div>
            </div>
        </div>
        <asp:Literal runat="server" ID="ltrMyData"></asp:Literal>
        <input id="hPictureSelectSingle" value="true" />
    </form>

    <script type="text/javascript">
        var DlgSelectPicture = {
            Init: function () {
                this.InitPager();
                this.InitPicSelect();
            },
            InitPager: function () {
                $("#myDataAppend>div").each(function () {
                    if ($(this).attr("code") == "myDataForPage") {
                        var jsonData = eval("(" + $(this).html() + ")");
                        $.map(jsonData, function (item) {
                            $('#easyPager').pagination({
                                total: item.TotalRecord,
                                pageSize: item.PageSize
                            });
                        })
                    }
                })
            },
            InitPicSelect: function () {
                if ($("#hPictureSelectSingle").val() == "true") {
                    $("#PictureBox").children().bind("click", function () {
                        $(this).addClass("curr").siblings().removeClass("curr");
                    })
                }
                else {
                    $("#PictureBox").children().bind("click", function () {
                        $(this).addClass("curr");
                    })
                }
            },
            GetPicSelect: function () {
                var data = new Array();
                $("#PictureBox>div").each(function () {
                    if ($(this).hasClass("curr")) {
                        var img = $(this).find("img");
                        data.push(img.attr("code") + "," + img.attr("src"));
                    }
                })
                return data;
            },
            OnPageChange: function () {
                $("#dlgSelectContentPicture").dialog('refresh', '/a/tyy.html');
            }
        }

        $(function () {
            DlgSelectPicture.Init();
        })
    </script>
</body>
</html>
