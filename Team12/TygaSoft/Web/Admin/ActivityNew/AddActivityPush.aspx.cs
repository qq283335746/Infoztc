using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using TygaSoft.Model;
using TygaSoft.BLL;
using TygaSoft.WebHelper;

namespace TygaSoft.Web.Admin.ActivityNew
{
    public partial class AddActivityPush : System.Web.UI.Page
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
                ActivitySubjectNew bll = new ActivitySubjectNew();
                DataSet ds = bll.GetModelOW(Id);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    txtTitle.Value = dt.Rows[0]["Title"].ToString();
                    hId.Value = Id.ToString();
                }
            }
        }
    }
}