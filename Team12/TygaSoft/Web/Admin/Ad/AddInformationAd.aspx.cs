using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TygaSoft.Model;
using TygaSoft.BLL;
using System.Data;

namespace TygaSoft.Web.Admin.Ad
{
    public partial class AddInformationAd : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Bind();
            }
        }

        private void Bind()
        {
            var Id = Guid.Empty;
            if (!string.IsNullOrWhiteSpace(Request.QueryString["Id"])) Guid.TryParse(Request.QueryString["Id"], out Id);
            if (!Id.Equals(Guid.Empty))
            {
                Page.Title = "编辑广告";

                //获取当前Id的数据，并赋值到表单即可
                InformationAd bll = new InformationAd();
                DataSet ds = bll.GetModelOW(Id);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    txtTitle.Value = dt.Rows[0]["Title"].ToString();
                    txtContent.Value = dt.Rows[0]["ContentText"].ToString();
                    txtDescr.Value = dt.Rows[0]["Descr"].ToString();
                    hId.Value = Id.ToString();
                    startDate.Value = Convert.ToDateTime(dt.Rows[0]["StartDate"]).ToString("yyyy-MM-dd HH:mm");
                    endDate.Value = Convert.ToDateTime(dt.Rows[0]["EndDate"]).ToString("yyyy-MM-dd HH:mm");
                    txtUrl.Value = dt.Rows[0]["Url"].ToString();

                    InformationAdPicture bllPP = new InformationAdPicture();
                    DataSet dsPP = bllPP.GetListOW(Id.ToString());
                    if (dsPP != null && dsPP.Tables.Count > 0 && dsPP.Tables[0].Rows != null && dsPP.Tables[0].Rows.Count > 0)
                    {
                        DataTable dtPP = dsPP.Tables[0];
                        foreach (DataRow row in dtPP.Rows)
                        {
                            string html = "<div class=\"row_col w110 mb10\" style=\"width:200px;\">" +
                                "<table style=\"width:100%;\">" +
                                    "<tr>" +
                                        "<td style=\"width:130px; vertical-align:top;\">" +
                                            "<img src=\"{0}\" alt=\"\" width=\"110px\" height=\"110px\" />" +
                                            "<input type=\"hidden\" name=\"PicId\" value=\"{1}\"/>" +
                                        "</td>" +
                                        "<td style=\"width:70px;\">" +
                                            "<a href=\"javascript:void(0)\" class=\"easyui-linkbutton\" data-options=\"iconCls:'icon-remove',plain:true\" onclick=\"$(this).parents('.row_col').remove()\">删 除</a>" +
                                        "</td>" +
                                    "</tr>" +
                                "</table>" +
                            "</div>";
                            string picSrc = row["PictureId"] is DBNull ? "../../Images/nopic.gif" : string.Format("{0}{1}/PC/{1}_1{2}", row["FileDirectory"], row["RandomFolder"], row["FileExtension"]);
                            html = string.Format(html, picSrc, row["PictureId"].ToString());
                            imgContentPicture.InnerHtml += html;
                        }
                    }

                    if (0 == Convert.ToInt32(dt.Rows[0]["ViewType"]))
                    {
                        rdViewType1.Checked = false;
                        rdViewType0.Checked = true;
                        divContent.Style.Add("display", "none");
                        divUrl.Style.Add("display", "block");
                    }
                    else
                    {
                        rdViewType1.Checked = true;
                        rdViewType0.Checked = false;
                        divContent.Style.Add("display", "block");
                        divUrl.Style.Add("display", "none");
                    }
                }
            }
        }
    }
}