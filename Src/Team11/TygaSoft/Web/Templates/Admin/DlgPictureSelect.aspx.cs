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
    public partial class DlgPictureSelect : System.Web.UI.Page
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
                myDataAppend.Append(File.ReadAllText(Server.MapPath("~/Templates/JsPictureSelect.txt")));

                Bind();

                myDataAppend.Replace("{DlgId}", dlgId);
                myDataAppend.Replace("{DlgHref}", "/t/tpicture.html?dlgId=" + dlgId + "&funName=" + funName + "");
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
                    case "AdvertisementPicture":
                        AdvertisementPicture adpBll = new AdvertisementPicture();
                        rpData.DataSource = adpBll.GetList(pageIndex, pageSize, out totalRecords, sqlWhere, parms == null ? null : parms.ToArray());
                        rpData.DataBind();
                        break;
                    case "PictureBrand":
                        PictureBrand pbBll = new PictureBrand();
                        rpData.DataSource = pbBll.GetList(pageIndex, pageSize, out totalRecords, sqlWhere, parms == null ? null : parms.ToArray());
                        rpData.DataBind();
                        break;
                    case "PictureCategory":
                        PictureCategory pcBll = new PictureCategory();
                        rpData.DataSource = pcBll.GetList(pageIndex, pageSize, out totalRecords, sqlWhere, parms == null ? null : parms.ToArray());
                        rpData.DataBind();
                        break;
                    case "PictureProduct":
                        PictureProduct ppBll = new PictureProduct();
                        rpData.DataSource = ppBll.GetList(pageIndex, pageSize, out totalRecords, sqlWhere, parms == null ? null : parms.ToArray());
                        rpData.DataBind();
                        break;
                    case "PictureProductSize":
                        PictureProductSize ppsBll = new PictureProductSize();
                        rpData.DataSource = ppsBll.GetList(pageIndex, pageSize, out totalRecords, sqlWhere, parms == null ? null : parms.ToArray());
                        rpData.DataBind();
                        break;
                    case "PictureServiceItem":
                        PictureServiceItem psiBll = new PictureServiceItem();
                        rpData.DataSource = psiBll.GetList(pageIndex, pageSize, out totalRecords, sqlWhere, parms == null ? null : parms.ToArray());
                        rpData.DataBind();
                        break;
                    case "PictureServiceVote":
                        PictureServiceVote psvBll = new PictureServiceVote();
                        rpData.DataSource = psvBll.GetList(pageIndex, pageSize, out totalRecords, sqlWhere, parms == null ? null : parms.ToArray());
                        rpData.DataBind();
                        break;
                    case "PictureServiceLink":
                        PictureServiceLink pslBll = new PictureServiceLink();
                        rpData.DataSource = pslBll.GetList(pageIndex, pageSize, out totalRecords, sqlWhere, parms == null ? null : parms.ToArray());
                        rpData.DataBind();
                        break;
                    case "PictureServiceContent":
                        PictureServiceContent pscBll = new PictureServiceContent();
                        rpData.DataSource = pscBll.GetList(pageIndex, pageSize, out totalRecords, sqlWhere, parms == null ? null : parms.ToArray());
                        rpData.DataBind();
                        break;
                    default:
                        break;
                }

                myDataAppend.Replace("{TotalRecord}", totalRecords.ToString());
                myDataAppend.Replace("{PageIndex}", pageIndex.ToString());
                myDataAppend.Replace("{PageSize}", pageSize.ToString());

            }
        }
    }
}