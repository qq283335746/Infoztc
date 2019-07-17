using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TygaSoft.Model;
using TygaSoft.BLL;
using TygaSoft.WebHelper;

namespace TygaSoft.Web.Admin.Lottery
{
    public partial class AddLottery : System.Web.UI.Page
    {
        Guid Id;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request.QueryString["Id"] != null)
                {
                    Guid.TryParse(Request.QueryString["Id"], out Id);
                }

                Bind();
            }
        }

        private void Bind()
        {
            if (!Id.Equals(Guid.Empty))
            {
                Page.Title = "编辑七星彩";

                QXCLotteryNumber bll = new QXCLotteryNumber();
                var model = bll.GetModel(Id);
                if (model != null)
                {
                    txtQS.Value = model.QS;
                    txtHNQS.Value = model.HNQS;
                    txtLTime.Value = model.LotteryTime.ToString("yyyy-MM-dd HH:mm");
                    txtLNo.Value = model.LotteryNo;
                    txtECDate.Value = model.ExpiryClosingDate.ToString("yyyy-MM-dd");
                    txtSV.Value = model.SalesVolume.ToString();
                    txtPro.Value = model.Progressive.ToString();
                    txtContentText.InnerHtml = model.ContentText;
                    hId.Value = Id.ToString();
                }
            }
        }
    }
}