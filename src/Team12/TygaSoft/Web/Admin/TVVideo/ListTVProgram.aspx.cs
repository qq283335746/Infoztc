using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TygaSoft.DBUtility;
using TygaSoft.SqlServerDAL;
using TygaSoft.WebHelper;

namespace TygaSoft.Web.Admin.TVVideo
{
    public partial class ListTVProgram : System.Web.UI.Page
    {
        Guid qsId;
        StringBuilder myDataAppend = new StringBuilder(300);
        int pageIndex = WebCommon.PageIndex;
        int pageSize = WebCommon.PageSize10;
        string queryStr;
        ParamsHelper parms;
        string sqlWhere;
        string name;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!string.IsNullOrWhiteSpace(Request.QueryString["qsId"]))
                {
                    Guid.TryParse(Request.QueryString["qsId"], out qsId);
                    hId.Value = qsId.ToString();
                }
                NameValueCollection nvc = Request.QueryString;
                int index = 0;
                foreach (string item in nvc.AllKeys)
                {
                    GetParms(item, nvc);

                    if (item != "pageIndex" && item != "pageSize")
                    {
                        index++;
                        if (index > 1) queryStr += "&";
                        queryStr += string.Format("{0}={1}", item, Server.HtmlEncode(nvc[item]));
                    }
                }

                Bind();
            }

            ltrMyData.Text = "<div id=\"myDataAppend\" style=\"display:none;\">" + myDataAppend.ToString() + "</div>";
        }

        private void Bind()
        {
            //查询条件
            GetSearchItem();

            int totalRecords = 0;
            TVProgram bll = new TVProgram();

            rpData.DataSource = bll.GetTable(pageIndex, pageSize, out totalRecords, sqlWhere, parms == null ? null : parms.ToArray());
            rpData.DataBind();

            myDataAppend.Append("<div id=\"myDataForPage\" style=\"display:none;\">[{\"PageIndex\":\"" + pageIndex + "\",\"PageSize\":\"" + pageSize + "\",\"TotalRecord\":\"" + totalRecords + "\",\"QueryStr\":\"" + queryStr + "\"}]</div>");
        }

        private void GetSearchItem()
        {
            if (parms == null) parms = new ParamsHelper();
            SqlParameter parm = new SqlParameter("@hwtvid", SqlDbType.UniqueIdentifier, 256);
            parm.Value = qsId;
            parms.Add(parm);
            sqlWhere += " and t.HWTVId=@hwtvid ";

            if (!string.IsNullOrWhiteSpace(name))
            {
                sqlWhere += "and t.ProgramName like @Named ";
                parm = new SqlParameter("@Named", SqlDbType.NVarChar, 256);
                parm.Value = "%" + name + "%";
                parms.Add(parm);
            }
        }

        private void GetParms(string key, NameValueCollection nvc)
        {
            switch (key)
            {
                case "pageIndex":
                    Int32.TryParse(nvc[key], out pageIndex);
                    break;
                case "pageSize":
                    Int32.TryParse(nvc[key], out pageSize);
                    break;
                case "name":
                    txtName.Value = nvc[key];
                    name = nvc[key];
                    break;
                default:
                    break;
            }
        }
    }
}