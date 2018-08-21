using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TygaSoft.Model;
using TygaSoft.SqlServerDAL;

namespace TygaSoft.Web.Admin.TVVideo
{
    public partial class AddHwtv : System.Web.UI.Page
    {
        Guid Id;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Request.QueryString["Id"]))
            {
                Guid.TryParse(Request.QueryString["Id"], out Id);
            }

            if (!Page.IsPostBack)
            {
                Bind();
            }
        }

        private void Bind()
        {
            if(!Id.Equals(Guid.Empty))
            {
                Page.Title = "编辑电视台视频";
                HWTV bll = new HWTV();
                var model = bll.GetModel(Id);
                if (model != null)
                {
                    txtName.Value = model.HWTVName;
                    hId.Value = Id.ToString();
                    hTurnTo.Value = model.IsTurnTo.ToString();
                    txtSort.Value = model.Sort.ToString();
                    txtProgramURL.Value = model.ProgramAddress;

                    if(model.HWTVIcon != null && !string.IsNullOrEmpty(model.HWTVIcon.ToString()))
                    {
                        CommunionPictureInfo md = new CommunionPictureInfo();// GetModel(object Id)
                        CommunionPicture communionPicture = new CommunionPicture();
                        md = communionPicture.GetModel(Guid.Parse(model.HWTVIcon.ToString()));
                        if(md!=null)
                        {
                            imgSinglePicture.Src = string.Format("{0}{1}/PC/{1}_1{2}", md.FileDirectory, md.RandomFolder, md.FileExtension);
                            hdimg.Value = md.Id.ToString();
                        }
                    }

                    if (model.IsDisable)
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