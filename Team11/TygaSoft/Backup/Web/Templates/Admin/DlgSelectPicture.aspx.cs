using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using TygaSoft.Model;
using TygaSoft.BLL;
using TygaSoft.WebHelper;
using TygaSoft.DBUtility;

namespace TygaSoft.Web.Templates.Admin
{
    public partial class DlgSelectPicture : System.Web.UI.Page
    {
        int pageIndex = 1;
        int pageSize = 20;
        string sqlWhere;
        ParamsHelper parms;
        StringBuilder myDataAppend;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!string.IsNullOrWhiteSpace(Request.QueryString["pageIndex"]))
                {
                    int.TryParse(Request.QueryString["pageIndex"], out pageIndex);
                    if (pageIndex < 1) pageIndex = 1;
                }
                if (!string.IsNullOrWhiteSpace(Request.QueryString["pageSize"]))
                {
                    int.TryParse(Request.QueryString["pageSize"], out pageSize);
                    if (pageSize < 20) pageSize = 20;
                }

                myDataAppend = new StringBuilder(300);

                Bind();

                ltrMyData.Text = "<div id=\"myDataAppend\" style=\"display:none;\">" + myDataAppend.ToString() + "</div>";
            }
        }

        private void Bind()
        {
            int totalRecords = 0;

            ContentPicture cpBll = new ContentPicture();
            rpData.DataSource = cpBll.GetList(pageIndex, pageSize, out totalRecords, sqlWhere, parms == null ? null : parms.ToArray());
            rpData.DataBind();

            myDataAppend.Append("<div code=\"myDataForPage\">[{\"PageIndex\":\"" + pageIndex + "\",\"PageSize\":\"" + pageSize + "\",\"TotalRecord\":\"" + totalRecords + "\"}]</div>");
        }
    }
}