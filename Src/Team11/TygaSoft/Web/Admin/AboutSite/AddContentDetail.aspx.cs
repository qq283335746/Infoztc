using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TygaSoft.Model;
using TygaSoft.BLL;
using TygaSoft.SysHelper;

namespace TygaSoft.Web.Admin.AboutSite
{
    public partial class AddContentDetail : System.Web.UI.Page
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

                BindIsDisable();

                Bind();
            }
        }

        private void Bind()
        {
            if (!Id.Equals(Guid.Empty))
            {
                Page.Title = "编辑内容";

                ContentDetail bll = new ContentDetail();
                var model = bll.GetModelByJoin(Id);
                if (model != null)
                {
                    hId.Value = Id.ToString();
                    txtTitle.Value = model.Title;
                    txtParent.Value = model.ContentTypeId.ToString();
                    string pictureUrl = "";
                    if (!string.IsNullOrWhiteSpace(model.FileDirectory))
                    {
                        pictureUrl = PictureUrlHelper.GetMPicture(model.FileDirectory, model.RandomFolder, model.FileExtension);
                    }
                    imgPicture.Src = string.IsNullOrWhiteSpace(pictureUrl) ? "../../Images/nopic.gif" : pictureUrl;
                    hPictureId.Value = model.PictureId.ToString();
                    txtaDescr.Value = model.Descr;
                    txtContent.Value = model.ContentText;
                    txtVirtualViewCount.Value = model.VirtualViewCount.ToString();
                    txtSort.Value = model.Sort.ToString();
                    var li = rbtnlIsDisable.Items.FindByValue(model.IsDisable.ToString().ToLower());
                    if (li != null) li.Selected = true;
                }
            }
        }

        private void BindIsDisable()
        {
            rbtnlIsDisable.Items.Add(new ListItem("否", "false"));
            rbtnlIsDisable.Items.Add(new ListItem("是", "true"));
            var li = rbtnlIsDisable.Items.FindByValue("false");
            if (li != null) li.Selected = true;
        }
    }
}