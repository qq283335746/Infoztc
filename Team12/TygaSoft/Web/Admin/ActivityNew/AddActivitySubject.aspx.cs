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
    public partial class AddActivitySubject : System.Web.UI.Page
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
                Page.Title = "编辑活动";

                ActivitySubjectNew bll = new ActivitySubjectNew();
                DataSet ds = bll.GetModelOW(Id);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    txtTitle.Value = dt.Rows[0]["Title"].ToString();
                    txtContent.Value = dt.Rows[0]["ContentText"].ToString();
                    startDate.Value = Convert.ToDateTime(dt.Rows[0]["StartDate"]).ToString("yyyy-MM-dd HH:mm");
                    endDate.Value = Convert.ToDateTime(dt.Rows[0]["EndDate"]).ToString("yyyy-MM-dd HH:mm");
                    imgSinglePicture.Src = dt.Rows[0]["PictureId"] is DBNull ? "../../Images/nopic.gif" : string.Format("{0}{1}/PC/{1}_1{2}", dt.Rows[0]["FileDirectory"], dt.Rows[0]["RandomFolder"], dt.Rows[0]["FileExtension"]);
                    hImgSinglePictureId.Value = dt.Rows[0]["PictureId"].ToString();
                    txtSort.Value = dt.Rows[0]["Sort"].ToString();
                    txtMaxVoteCount.Value = dt.Rows[0]["MaxVoteCount"].ToString();
                    txtMaxSignUpCount.Value = dt.Rows[0]["MaxSignUpCount"].ToString();
                    txtActualSignUpCount.Value = dt.Rows[0]["SignUpCount"].ToString();
                    txtUpdateSignUpCount.Value = dt.Rows[0]["VirtualSignUpCount"].ToString();
                    txtSignUpRule.Value = dt.Rows[0]["SignUpRule"].ToString();
                    txtViewCount.Value = dt.Rows[0]["ViewCount"].ToString();
                    txtPrizeProbability.Value = dt.Rows[0]["PrizeProbability"].ToString();
                    txtPrizeRule.Value = dt.Rows[0]["PrizeRule"].ToString();
                    hId.Value = Id.ToString();

                    string sHideAttr = dt.Rows[0]["HiddenAttribute"].ToString();
                    if (sHideAttr.Contains("Professional"))
                    {
                        Professional.Checked = true;
                    }

                    if (Convert.ToBoolean(dt.Rows[0]["IsPrize"]))
                    {
                        rdPrizeFalse.Checked = false;
                        rdPrizeTrue.Checked = true;
                        divPrize.Style.Add("display", "block");
                    }
                    else
                    {
                        rdPrizeFalse.Checked = true;
                        rdPrizeTrue.Checked = false;
                        divPrize.Style.Add("display", "none");
                    }

                    if (Convert.ToBoolean(dt.Rows[0]["IsDisable"]))
                    {
                        rdFalse.Checked = false;
                        rdTrue.Checked = true;
                    }
                    else
                    {
                        rdFalse.Checked = true;
                        rdTrue.Checked = false;
                    }
                }
            }
        }
    }
}