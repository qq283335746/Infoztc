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

namespace TygaSoft.Web.Admin.Topic
{
    public partial class AddTopicSubject : System.Web.UI.Page
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
                Page.Title = "编辑话题";

                TopicSubject bll = new TopicSubject();
                DataSet ds = bll.GetModelOW(Id);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    txtTitle.Value = dt.Rows[0]["Title"].ToString();
                    txtContent.Value = dt.Rows[0]["ContentText"].ToString();
                    imgSinglePicture.Src = dt.Rows[0]["PictureId"] is DBNull ? "../../Images/nopic.gif" : string.Format("{0}{1}/PC/{1}_1{2}", dt.Rows[0]["FileDirectory"], dt.Rows[0]["RandomFolder"], dt.Rows[0]["FileExtension"]);
                    hImgSinglePictureId.Value = dt.Rows[0]["PictureId"].ToString();
                    //UserId.Value = dt.Rows[0]["UserId"].ToString();
                    hId.Value = Id.ToString();
                    isTop.Value = Request["isTop"];
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