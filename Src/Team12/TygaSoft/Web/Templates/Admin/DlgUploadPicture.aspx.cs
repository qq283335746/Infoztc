using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.IO;

namespace TygaSoft.Web.Templates.Admin
{
    public partial class DlgUploadPicture : System.Web.UI.Page
    {
        string dlgId;
        string dlgParentId;
        string submitUrl;
        string funName;
        bool isMutilSelect;
        StringBuilder myDataAppend;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!string.IsNullOrWhiteSpace(Request.QueryString["dlgId"]))
                {
                    dlgId = Request.QueryString["dlgId"].Trim();
                }
                if (!string.IsNullOrWhiteSpace(Request.QueryString["dlgParentId"]))
                {
                    dlgParentId = Request.QueryString["dlgParentId"].Trim();
                }
                if (!string.IsNullOrWhiteSpace(Request.QueryString["submitUrl"]))
                {
                    submitUrl = Request.QueryString["submitUrl"].Trim();
                }
                if (!string.IsNullOrWhiteSpace(Request.QueryString["funName"]))
                {
                    funName = Request.QueryString["funName"].Trim();
                }
                if (!string.IsNullOrWhiteSpace(Request.QueryString["isMutil"]))
                {
                    isMutilSelect = Request.QueryString["isMutil"].Trim().ToLower() == "true";
                }

                myDataAppend = new StringBuilder(250);

                //Bind();

                //ltrMyData.Text = "<div id=\"myDataAppend_" + dlgId + "\" style=\"display:none;\">" + myDataAppend.ToString() + "</div>";
                myDataAppend.Append(File.ReadAllText(Server.MapPath("~/Templates/JsUploadPicture.txt")));
                myDataAppend.Replace("{DlgId}", dlgId);
                myDataAppend.Replace("{DlgParentId}", dlgParentId);
                myDataAppend.Replace("{DlgParentHref}", "/t/yt.html?dlgId=" + dlgParentId + "&funName=" + funName + "&isMutil=" + isMutilSelect + "");
                myDataAppend.Replace("{SubmitUrl}", submitUrl);
                myDataAppend.Replace("{FunName}", funName);

                ltrMyData.Text = myDataAppend.ToString();
            }
        }

        private void Bind()
        {
            //myDataAppend.Append("<div code=\"myDataForDlg\">[{\"DlgId\":\"" + dlgId + "\",\"DlgParentId\":\"" + dlgParentId + "\",\"DlgParentHref\":\"" + dlgParentHref + "\"}]</div>");
        }
    }
}