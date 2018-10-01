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
    public partial class AddActivityPlayer : System.Web.UI.Page
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
                Page.Title = "编辑选手";

                ActivityPlayerNew bll = new ActivityPlayerNew();
                DataSet ds = bll.GetModelOW(Id);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    txtName.Value = dt.Rows[0]["Named"].ToString();
                    txtAge.Value = dt.Rows[0]["Age"].ToString();
                    txtOccupation.Value = dt.Rows[0]["Occupation"].ToString();
                    txtPhone.Value = dt.Rows[0]["Phone"].ToString();
                    txtLocation.Value = dt.Rows[0]["Location"].ToString();
                    txtProfessional.Value = dt.Rows[0]["Professional"].ToString();
                    txtDescr.Value = dt.Rows[0]["Descr"].ToString();
                    imgSinglePicture.Src = dt.Rows[0]["PictureId"] is DBNull ? "../../Images/nopic.gif" : string.Format("{0}{1}/PC/{1}_1{2}", dt.Rows[0]["FileDirectory"], dt.Rows[0]["RandomFolder"], dt.Rows[0]["FileExtension"]);
                    hImgSinglePictureId.Value = dt.Rows[0]["PictureId"].ToString();
                    txtActualVoteCount.Value = dt.Rows[0]["VoteCount"].ToString();
                    txtUpdateVoteCount.Value = dt.Rows[0]["VirtualVoteCount"].ToString();
                    hId.Value = Id.ToString();
                    asId.Value = dt.Rows[0]["ActivityId"].ToString();

                    PlayerPictureNew bllPP = new PlayerPictureNew();
                    DataSet dsPP = bllPP.GetListOW(dt.Rows[0]["Id"].ToString());
                    if (dsPP != null && dsPP.Tables.Count > 0 && dsPP.Tables[0].Rows != null && dsPP.Tables[0].Rows.Count > 0)
                    {
                        DataTable dtPP = dsPP.Tables[0];
                        int index = 0;
                        foreach (DataRow row in dtPP.Rows)
                        {
                            if (index > 0)
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
                            index++;
                        }
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
            else
            {
                asId.Value = Request["asId"];
            }
        }
    }
}