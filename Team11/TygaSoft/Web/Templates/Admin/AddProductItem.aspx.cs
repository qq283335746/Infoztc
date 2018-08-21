using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TygaSoft.Model;
using TygaSoft.BLL;

namespace TygaSoft.Web.Templates.Admin
{
    public partial class AddProductItem : System.Web.UI.Page
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
                BindIsEnable();

                BindIsDisable();

                Bind();
            }
        }

        private void Bind()
        {
            if (!Id.Equals(Guid.Empty))
            {
                ProductItem bll = new ProductItem();
                var model = bll.GetModelByJoin(Id);
                if (model != null)
                {
                    hProductItemId.Value = model.Id.ToString();
                    txtName.Value = model.Named;
                    txtSort.Value = model.Sort.ToString();
                    imgProductItemPicture.Src = string.IsNullOrWhiteSpace(model.MPicture) ? "../../Images/nopic.gif" : model.MPicture;
                    hImgProductItemPictureId.Value = model.PictureId.ToString();
                    txtEnableStartTime_ProductItem.Value = model.EnableStartTime == DateTime.MinValue ? "" : model.EnableStartTime.ToString("yyyy-MM-dd HH:mm:ss");
                    txtEnableEndTime_ProductItem.Value = model.EnableStartTime == DateTime.MinValue ? "" : model.EnableStartTime.ToString("yyyy-MM-dd HH:mm:ss");
                    var li = rbtnIsEnableList_ProductItem.Items.FindByValue(model.IsEnable.ToString().ToLower());
                    if (li != null) li.Selected = true;
                    li = rbtnList_ProductItem.Items.FindByValue(model.IsDisable.ToString().ToLower());
                    if (li != null) li.Selected = true;
                }
            }
        }

        private void BindIsEnable()
        {
            rbtnIsEnableList_ProductItem.Items.Add(new ListItem("上架", "false"));
            rbtnIsEnableList_ProductItem.Items.Add(new ListItem("下架", "true"));
            var li = rbtnIsEnableList_ProductItem.Items.FindByValue("false");
            if (li != null) li.Selected = true;
        }

        private void BindIsDisable()
        {
            rbtnList_ProductItem.Items.Add(new ListItem("否", "false"));
            rbtnList_ProductItem.Items.Add(new ListItem("是", "true"));
            var li = rbtnList_ProductItem.Items.FindByValue("false");
            if (li != null) li.Selected = true;
        }
    }
}