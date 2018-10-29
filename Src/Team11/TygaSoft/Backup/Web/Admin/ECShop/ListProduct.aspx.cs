using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Specialized;
using TygaSoft.Model;
using TygaSoft.BLL;
using TygaSoft.WebHelper;
using TygaSoft.DBUtility;

namespace TygaSoft.Web.Admin.ECShop
{
    public partial class ListProduct : System.Web.UI.Page
    {
        string myDataAppend;
        int pageIndex = WebCommon.PageIndex;
        int pageSize = WebCommon.PageSize10;
        string queryStr;
        ParamsHelper parms;
        string sqlWhere;
        string name;
        string menu;

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

            ltrMyData.Text = "<div id=\"myDataAppend\" style=\"display:none;\">" + myDataAppend + "</div>";
        }

        private void Bind()
        {
            GetSearchItem();

            List<ProductItemInfo> list = null;
            int totalRecords = 0;

            ProductItem bll = new ProductItem();
            list = bll.GetListByJoin(pageIndex, pageSize, out totalRecords, sqlWhere, parms == null ? null : parms.ToArray());

            rpData.DataSource = list;
            rpData.DataBind();

            myDataAppend += "<div id=\"myDataForPage\">[{\"PageIndex\":\"" + pageIndex + "\",\"PageSize\":\"" + pageSize + "\",\"TotalRecord\":\"" + totalRecords + "\",\"QueryStr\":\"" + queryStr + "\"}]</div>";
        }

        private void GetSearchItem()
        {
            myDataAppend += "<div id=\"myDataForSearch\">[{\"name\":\""+name+"\",\"menu\":\""+menu+"\"}]</div>";
            if (!string.IsNullOrWhiteSpace(name))
            {
                if (parms == null) parms = new ParamsHelper();

                sqlWhere += "and ProductName like @ProductName ";
                SqlParameter parm = new SqlParameter("@ProductName", SqlDbType.NVarChar, 50);
                parm.Value = "%" + name + "%";

                parms.Add(parm);
            }
            if (!string.IsNullOrWhiteSpace(menu))
            {
                Guid customMenuId = Guid.Empty;
                Guid.TryParse(menu, out customMenuId);
                if (!customMenuId.Equals(Guid.Empty))
                {
                    if (parms == null) parms = new ParamsHelper();

                    sqlWhere += "and CustomMenuId = @CustomMenuId ";
                    SqlParameter parm = new SqlParameter("@CustomMenuId", SqlDbType.UniqueIdentifier);
                    parm.Value = customMenuId;

                    parms.Add(parm);
                }
            }
        }

        private void GetParms(string key, NameValueCollection nvc)
        {
            string sSearchParm = string.Empty;
            switch (key)
            {
                case "pageIndex":
                    Int32.TryParse(nvc[key], out pageIndex);
                    break;
                case "pageSize":
                    Int32.TryParse(nvc[key], out pageSize);
                    break;
                case "name":
                    name = nvc[key];
                   
                    break;
                case "menu":
                    menu = nvc[key];
                    
                    break;
                default:
                    break;
            }

            if (!string.IsNullOrEmpty(sSearchParm))
            {
                myDataAppend += "<div id=\"myDataForSearch\">[{" + sSearchParm + "}]";
            }
        }
    }
}