using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TygaSoft.Model;
using TygaSoft.BLL;
using TygaSoft.SysHelper;

namespace TygaSoft.Web.Templates.Admin
{
    public partial class AddServiceItem : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindIsDisable();

                Bind();
            }
        }

        private void Bind()
        {
            string action = Request.QueryString["action"];
            if (string.IsNullOrWhiteSpace(action)) return;
            switch (action)
            {
                case "add":
                    BindAdd();
                    break;
                case "edit":
                    BindEdit();
                    break;
                default:
                    break;
            }
        }

        private void BindIsDisable()
        {
            rbtnList.Items.Add(new ListItem("否", "false"));
            rbtnList.Items.Add(new ListItem("是", "true"));
            var li = rbtnList.Items.FindByValue("false");
            if (li != null) li.Selected = true;
        }

        private void BindAdd()
        {
            Guid Id = Guid.Empty;
            if (!string.IsNullOrWhiteSpace(Request.QueryString["Id"]))
            {
                Guid.TryParse(Request.QueryString["Id"], out Id);
            }
            if (!Id.Equals(Guid.Empty))
            {
                ServiceItem bll = new ServiceItem();
                var model = bll.GetModelByJoin(Id);
                if (model != null)
                {
                    lbParent.InnerText = model.Named;
                    hParentId.Value = model.Id.ToString();
                }
            }
        }

        private void BindEdit()
        {
            Guid Id = Guid.Empty;
            if (!string.IsNullOrWhiteSpace(Request.QueryString["Id"]))
            {
                Guid.TryParse(Request.QueryString["Id"], out Id);
            }
            if (!Id.Equals(Guid.Empty))
            {
                ServiceItem bll = new ServiceItem();
                var model = bll.GetModelByJoin(Id);
                if (model != null)
                {
                    hId.Value = model.Id.ToString();
                    if (!string.IsNullOrWhiteSpace(model.ParentName))
                    {
                        lbParent.InnerText = model.ParentName;
                        hParentId.Value = model.ParentId.ToString();
                    }
                    else
                    {
                        lbParent.InnerText = "分类根";
                        hParentId.Value = Guid.Empty.ToString();
                    }

                    txtName.Value = model.Named;

                    string pictureUrl = "";
                    if (!string.IsNullOrWhiteSpace(model.FileDirectory))
                    {
                        pictureUrl = PictureUrlHelper.GetMPicture(model.FileDirectory, model.RandomFolder, model.FileExtension);
                    }
                    imgServicePicture.Src = string.IsNullOrWhiteSpace(pictureUrl) ? "../../Images/nopic.gif" : pictureUrl;
                    hImgServicePictureId.Value = model.PictureId.ToString();
                    txtSort.Value = model.Sort.ToString();
                    txtEnableStartTime.Value = model.EnableStartTime == DateTime.MinValue ? "" : model.EnableStartTime.ToString("yyyy-MM-dd HH:mm:ss");
                    txtEnableEndTime.Value = model.EnableEndTime == DateTime.MinValue ? "" : model.EnableStartTime.ToString("yyyy-MM-dd HH:mm:ss");
                    var li = rbtnList.Items.FindByValue(model.IsDisable.ToString().ToLower());
                    if (li != null) li.Selected = true;
                }
            }
        }
    }
}