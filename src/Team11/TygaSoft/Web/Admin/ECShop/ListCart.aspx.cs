using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Specialized;
using System.Web.Security;
using TygaSoft.Model;
using TygaSoft.BLL;
using TygaSoft.WebHelper;
using TygaSoft.DBUtility;

namespace TygaSoft.Web.Admin.ECShop
{
    public partial class ListCart : System.Web.UI.Page
    {
        string myDataAppend;
        int pageIndex = WebCommon.PageIndex;
        int pageSize = WebCommon.PageSize10;
        StringBuilder queryStr;
        ParamsHelper parms;
        StringBuilder sqlWhere;
        string name;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (queryStr == null) queryStr = new StringBuilder(300);
                NameValueCollection nvc = Request.QueryString;
                int index = 0;
                foreach (string item in nvc.AllKeys)
                {
                    GetParms(item, nvc);

                    if (item != "pageIndex" && item != "pageSize")
                    {
                        index++;
                        if (index > 1) queryStr.Append("&");
                        queryStr.AppendFormat("{0}={1}", item, Server.HtmlEncode(nvc[item]));
                    }
                }

                Bind();
            }

            ltrMyData.Text = "<div id=\"myDataAppend\" style=\"display:none;\">" + myDataAppend + "</div>";
        }

        private void Bind()
        {
            //查询条件
            GetSearchItem();

            int totalRecords = 0;
            Cart bll = new Cart();

            rpData.DataSource = bll.GetListByJoin(pageIndex, pageSize, out totalRecords, sqlWhere == null ? "" : sqlWhere.ToString(), parms == null ? null : parms.ToArray());
            rpData.DataBind();

            myDataAppend += "<div id=\"myDataForPage\" style=\"display:none;\">[{\"PageIndex\":\"" + pageIndex + "\",\"PageSize\":\"" + pageSize + "\",\"TotalRecord\":\"" + totalRecords + "\",\"QueryStr\":\"" + queryStr + "\"}]</div>";
        }

        private void GetSearchItem()
        {
            if (parms == null) parms = new ParamsHelper();
            if (sqlWhere == null) sqlWhere = new StringBuilder(300);
            sqlWhere.Append("and AppName = @AppName ");
            parms.Add(new SqlParameter("@AppName", Membership.ApplicationName));

            if (!string.IsNullOrWhiteSpace(name))
            {
                sqlWhere.Append("and (u.UserName like @UserName or p.ProductName like @ProductName) ");
                SqlParameter parm = new SqlParameter("@UserName", SqlDbType.NVarChar, 256);
                parm.Value = "%" + name + "%";
                parms.Add(parm);
                parm = new SqlParameter("@ProductName", SqlDbType.NVarChar, 50);
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
                case "keyword":
                    txtName.Value = nvc[key];
                    name = nvc[key];
                    break;
                default:
                    break;
            }
        }
    }
}