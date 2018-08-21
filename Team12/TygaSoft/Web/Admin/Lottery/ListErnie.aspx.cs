using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Specialized;
using System.Text;
using TygaSoft.Model;
using TygaSoft.BLL;
using TygaSoft.WebHelper;
using TygaSoft.DBUtility;

namespace TygaSoft.Web.Admin.Lottery
{
    public partial class ListErnie : System.Web.UI.Page
    {
        StringBuilder myDataAppend = new StringBuilder(300);
        int pageIndex = WebCommon.PageIndex;
        int pageSize = WebCommon.PageSize10;
        string queryStr;
        ParamsHelper parms;
        string sqlWhere;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
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
            Ernie bll = new Ernie();

            rpData.DataSource = bll.GetList(pageIndex, pageSize, out totalRecords, sqlWhere, parms == null ? null : parms.ToArray());
            rpData.DataBind();

            myDataAppend.Append("<div id=\"myDataForPage\" style=\"display:none;\">[{\"PageIndex\":\"" + pageIndex + "\",\"PageSize\":\"" + pageSize + "\",\"TotalRecord\":\"" + totalRecords + "\",\"QueryStr\":\"" + queryStr + "\"}]</div>");
        }

        private void GetSearchItem()
        {
            string sStartTime = Request.QueryString["startTime"];
            string sEndTime = Request.QueryString["endTime"];

            DateTime startTime = DateTime.MinValue;
            if (!string.IsNullOrWhiteSpace(sStartTime))
            {
                DateTime.TryParse(sStartTime, out startTime);
            }
            DateTime endTime = DateTime.MinValue;
            if (!string.IsNullOrWhiteSpace(sEndTime))
            {
                DateTime.TryParse(sEndTime, out endTime);
            }

            if (startTime != DateTime.MinValue && endTime != DateTime.MinValue)
            {
                if (parms == null) parms = new ParamsHelper();

                sqlWhere += "and LastUpdatedDate between @StartTime and @EndTime ";
                SqlParameter parm = new SqlParameter("@StartTime", SqlDbType.DateTime);
                parm.Value = startTime;
                parms.Add(parm);
                parm = new SqlParameter("@EndTime", SqlDbType.DateTime);
                parm.Value = endTime;
                parms.Add(parm);
            }
            else
            {
                if (startTime != DateTime.MinValue)
                {
                    if (parms == null) parms = new ParamsHelper();

                    sqlWhere += "and LastUpdatedDate >= @StartTime ";
                    SqlParameter parm = new SqlParameter("@StartTime", SqlDbType.DateTime);
                    parm.Value = startTime;
                    parms.Add(parm);
                }
                if (endTime != DateTime.MinValue)
                {
                    if (parms == null) parms = new ParamsHelper();

                    sqlWhere += "and LastUpdatedDate <= @EndTime ";
                    SqlParameter parm = new SqlParameter("@EndTime", SqlDbType.DateTime);
                    parm.Value = endTime;
                    parms.Add(parm);
                }
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
                default:
                    break;
            }
        }
    }
}