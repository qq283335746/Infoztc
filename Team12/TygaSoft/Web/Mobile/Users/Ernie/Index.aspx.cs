using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TygaSoft.BLL;
using TygaSoft.WebHelper;

namespace TygaSoft.Web.Mobile.Users.Ernie
{
    public partial class Index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Bind();
            }
        }

        private void Bind()
        {
            var list = ErnieDataProxy.GetLatest();
            if (list == null || list.Count == 0)
            {
                ltrMyData.Text = "<div id=\"myDataAppend\" style=\"display:none;\" startTime=\"\" endTime=\"\" totalMls=\"0\" status=\"100\"></div>";
                return;
            }

            var model = list[0];

            if (!((DateTime.Now >= model.StartTime) && (DateTime.Now < model.EndTime)))
            {
                if (DateTime.Now < model.StartTime)
                {
                    hTotalMls.Value = "" + (model.StartTime - DateTime.Now).TotalMilliseconds + "";
                    ltrMyData.Text = "<div id=\"myDataAppend\" style=\"display:none;\" startTime=\"" + model.StartTime.ToString("yyyy-MM-dd HH:mm") + "\" endTime=\"" + model.EndTime.ToString("yyyy-MM-dd HH:mm") + "\" totalMls=\"" + (model.StartTime - DateTime.Now).TotalMilliseconds + "\" status=\"0\"></div>";
                    return;
                }
                if (DateTime.Now > model.EndTime)
                {
                    ltrMyData.Text = "<div id=\"myDataAppend\" style=\"display:none;\" startTime=\"" + model.StartTime.ToString("yyyy-MM-dd HH:mm") + "\" endTime=\"" + model.EndTime.ToString("yyyy-MM-dd HH:mm") + "\" totalMls=\"0\" status=\"100\"></div>";
                    return;
                }
            }

            UserErnie ueBll = new UserErnie();
            var totalBetCount = ueBll.GetTotalBetCount(WebCommon.GetUserId(), model.ErnieId);
            int remainTimes = model.UserBetMaxCount - totalBetCount;
            if (remainTimes < 0) remainTimes = 0;

            ltrMyData.Text = "<div id=\"myDataAppend\" style=\"display:none;\" startTime=\"" + model.StartTime.ToString("yyyy-MM-dd HH:mm") + "\" endTime=\"" + model.EndTime.ToString("yyyy-MM-dd HH:mm") + "\" totalMls=\"0\" status=\"1\" remainTimes=\"" + remainTimes + "\"></div>";
        }
    }
}