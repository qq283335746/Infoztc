using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.IO;
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
        string funName;
        string keyword;
        bool isMutilSelect;
        string dlgId;

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
                if (!string.IsNullOrWhiteSpace(Request.QueryString["funName"]))
                {
                    funName = Request.QueryString["funName"].Trim();
                }
                if (!string.IsNullOrWhiteSpace(Request.QueryString["keyword"]))
                {
                    keyword = Request.QueryString["keyword"].Trim();
                }
                if (!string.IsNullOrWhiteSpace(Request.QueryString["isMutil"]))
                {
                    isMutilSelect = Request.QueryString["isMutil"].Trim().ToLower() == "true";
                }

                dlgId = Request.QueryString["dlgId"].Trim();

                myDataAppend = new StringBuilder(300);
                myDataAppend.Append(File.ReadAllText(Server.MapPath("~/Templates/JsSelectPicture.txt")));

                Bind();

                //ltrMyData.Text = "<div id=\"myDataAppend_" + dlgId + "\" style=\"display:none;\">" + myDataAppend.ToString() + "</div>";

                myDataAppend.Replace("{DlgId}", dlgId);
                myDataAppend.Replace("{DlgHref}", "/a/tyy.html?dlgId="+dlgId+"&funName=" + funName + "");
                myDataAppend.Replace("{Keyword}", keyword);
                myDataAppend.Replace("{IsMutilSelect}", isMutilSelect.ToString().ToLower());

                ltrMyData.Text = myDataAppend.ToString();
            }
        }

        private void Bind()
        {
            if (!string.IsNullOrEmpty(funName))
            {
                int totalRecords = 0;
                switch (funName)
                {
                    case "CategoryPicture":
                        CategoryPicture categorypBll = new CategoryPicture();
                        rpData.DataSource = categorypBll.GetList(pageIndex, pageSize, out totalRecords, sqlWhere, parms == null ? null : parms.ToArray());
                        rpData.DataBind();

                        break;
                    case "ProductPicture":
                        ProductPicture ppBll = new ProductPicture();
                        rpData.DataSource = ppBll.GetList(pageIndex, pageSize, out totalRecords, sqlWhere, parms == null ? null : parms.ToArray());
                        rpData.DataBind();

                        break;
                    case "SizePicture":
                        SizePicture spBll = new SizePicture();
                        rpData.DataSource = spBll.GetList(pageIndex, pageSize, out totalRecords, sqlWhere, parms == null ? null : parms.ToArray());
                        rpData.DataBind();
                        break;
                    case "ContentPicture":
                        ContentPicture cpBll = new ContentPicture();
                        rpData.DataSource = cpBll.GetList(pageIndex, pageSize, out totalRecords, sqlWhere, parms == null ? null : parms.ToArray());
                        rpData.DataBind();
                        break;
                    case "ServicePicture":
                        ServicePicture servepBll = new ServicePicture();
                        rpData.DataSource = servepBll.GetList(pageIndex, pageSize, out totalRecords, sqlWhere, parms == null ? null : parms.ToArray());
                        rpData.DataBind();
                        break;
                    default:
                        break;
                }

                myDataAppend.Replace("{TotalRecord}", totalRecords.ToString());
                myDataAppend.Replace("{PageIndex}", pageIndex.ToString());
                myDataAppend.Replace("{PageSize}", pageSize.ToString());

                //myDataAppend.Append("<div code=\"myDataForPage\">[{\"PageIndex\":\"" + pageIndex + "\",\"PageSize\":\"" + pageSize + "\",\"TotalRecord\":\"" + totalRecords + "\"}]</div>");
                //myDataAppend.Append("<div code=\"myDataForDlg\">[{\"DlgId\":\"" + dlgId + "\",\"DlgHref\":\"/a/tyy.html\",\"IsMutilSelect\":\"" + isMutilSelect + "\"}]</div>");
            }
        }
    }
}