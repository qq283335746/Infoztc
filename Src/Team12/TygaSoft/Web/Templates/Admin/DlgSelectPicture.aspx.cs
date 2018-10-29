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

                myDataAppend.Replace("{DlgId}", dlgId);
                myDataAppend.Replace("{DlgHref}", "/t/yt.html?dlgId=" + dlgId + "&funName=" + funName + "");
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
                    case "CommunionPicture":
                        CommunionPicture communionpBll = new CommunionPicture();
                        rpData.DataSource = communionpBll.GetList(pageIndex, pageSize, out totalRecords, "", null);
                        rpData.DataBind();
                        break;
                    case "UserHeadPicture":
                        UserHeadPicture uhpBll = new UserHeadPicture();
                        rpData.DataSource = uhpBll.GetList(pageIndex, pageSize, out totalRecords, "", null);
                        rpData.DataBind();
                        break;
                    case "ActivityPhotoPicture":
                        ActivityPhotoPicture appBll = new ActivityPhotoPicture();
                        rpData.DataSource = appBll.GetList(pageIndex, pageSize, out totalRecords, "", null);
                        rpData.DataBind();
                        break;
                    case "ActivityPlayerPhotoPicture":
                        ActivityPlayerPhotoPicture apppBll = new ActivityPlayerPhotoPicture();
                        rpData.DataSource = apppBll.GetList(pageIndex, pageSize, out totalRecords, "", null);
                        rpData.DataBind();
                        break;
                    case "PictureScratchLotto":
                        PictureScratchLotto pslBll = new PictureScratchLotto();
                        rpData.DataSource = pslBll.GetList(pageIndex, pageSize, out totalRecords, "", null);
                        rpData.DataBind();
                        break;
                    case "InformationPicture":
                        PictureInformation piBll = new PictureInformation();
                        rpData.DataSource = piBll.GetList(pageIndex, pageSize, out totalRecords, "", null);
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