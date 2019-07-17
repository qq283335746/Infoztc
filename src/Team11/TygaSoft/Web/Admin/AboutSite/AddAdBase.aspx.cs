using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using TygaSoft.Model;
using TygaSoft.BLL;
using TygaSoft.SysHelper;

namespace TygaSoft.Web.Admin.AboutSite
{
    public partial class AddAdBase : System.Web.UI.Page
    {
        Guid Id;
        StringBuilder myDataAppend;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!string.IsNullOrWhiteSpace(Request.QueryString["adId"]))
                {
                    Guid.TryParse(Request.QueryString["adId"], out Id);
                }

                Bind();

                ltrMyData.Text = "<div id=\"myDataAppend\" style=\"display:none;\">" + (myDataAppend == null ? "" : myDataAppend.ToString()) + "</div>";
            }
        }

        private void Bind()
        {
            if (!Id.Equals(Guid.Empty))
            {
                Page.Title = "编辑广告";

                hAdId.Value = Id.ToString();

                AdBase bll = new AdBase();

                var model = bll.GetModelByJoin(Id);
                if (model != null)
                {
                    hAdId.Value = Id.ToString();
                    txtTitle.Value = model.Title;
                    txtTimeout.Value = model.Timeout.ToString();
                    txtSort.Value = model.Sort.ToString();
                    txtStartTime.Value = model.StartTime.ToString("yyyy-MM-dd HH:mm:ss");
                    txtEndTime.Value = model.EndTime.ToString("yyyy-MM-dd HH:mm:ss");
                    txtVirtualViewCount.Value = model.VirtualViewCount.ToString();

                    myDataAppend = new StringBuilder(1000);
                    myDataAppend.Append("<div id=\"myDataForModel\">{\"SiteFunId\":\"" + model.SiteFunId + "\",\"LayoutPositionId\":\"" + model.LayoutPositionId + "\",\"IsDisable\":\"" + model.IsDisable + "\"}</div>");
                }
            }
        }
    }
}