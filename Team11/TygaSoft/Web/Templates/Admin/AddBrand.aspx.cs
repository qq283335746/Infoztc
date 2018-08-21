using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TygaSoft.BLL;
using TygaSoft.SysHelper;

namespace TygaSoft.Web.Templates.Admin
{
    public partial class AddBrand : System.Web.UI.Page
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
            Guid Id = Guid.Empty;
            if (!string.IsNullOrWhiteSpace(Request.QueryString["Id"]))
            {
                Guid.TryParse(Request.QueryString["Id"], out Id);
            }
            string action = Request.QueryString["action"];
            switch (action)
            {
                case "add":
                    InitAdd(Id);
                    break;
                case "edit":
                    InitEdit(Id);
                    break;
                default:
                    break;
            }
        }

        private void InitAdd(Guid parentId)
        {
            hParentId.Value = parentId.ToString();
        }

        private void InitEdit(Guid Id)
        { 
            Brand bll = new Brand();
            var model = bll.GetModelByJoin(Id);
            if (model != null)
            {
                hId.Value = model.Id.ToString();
                hParentId.Value = model.ParentId.ToString();
                txtName.Value = model.BrandName;
                txtCode.Value = model.BrandCode;
                hCategoryId.Value = model.CategoryId.ToString();
                txtSort.Value = model.Sort.ToString();
                txtRemark.Value = model.Remark;
                string pictureUrl = "";
                if (!string.IsNullOrWhiteSpace(model.FileDirectory))
                {
                    pictureUrl = PictureUrlHelper.GetMPicture(model.FileDirectory, model.RandomFolder, model.FileExtension);
                }
                imgBrandPicture.Src = string.IsNullOrWhiteSpace(pictureUrl) ? "../../Images/nopic.gif" : pictureUrl;
                hBrandPictureId.Value = model.PictureId.ToString();
            }
        }
    }
}