using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TygaSoft.SqlServerDAL;

namespace TygaSoft.Web.Admin.TVVideo
{
    public partial class AddTVProgram : System.Web.UI.Page
    {
        Guid Id;
        Guid hwId;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Request.QueryString["Id"]))
            {
                Guid.TryParse(Request.QueryString["Id"], out Id);
            }

            if (!string.IsNullOrWhiteSpace(Request.QueryString["hwid"]))
            {
                Guid.TryParse(Request.QueryString["hwid"], out hwId);
                hHWid.Value = hwId.ToString();
            }

            if (!Page.IsPostBack)
            {
                Bind();
            }
        }

        private void Bind()
        {
            if (!Id.Equals(Guid.Empty))
            {
                Page.Title = "编辑电视台视频二级";
                TVProgram bll = new TVProgram();
                var model = bll.GetModel(Id);
                if (model != null)
                {
                    txtName.Value = model.ProgramName;
                    hId.Value = Id.ToString();
                    hHWid.Value = model.HWTVId.ToString();
                    txtSort.Value = model.Sort.ToString();
                    txtProgramURL.Value = model.ProgramAddress;
                    txtTVScID.Value = model.TVScID;
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