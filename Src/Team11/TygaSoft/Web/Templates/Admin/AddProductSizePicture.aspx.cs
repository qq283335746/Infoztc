using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TygaSoft.Model;
using TygaSoft.BLL;
using TygaSoft.WebHelper;
using TygaSoft.SysHelper;

namespace TygaSoft.Web.Templates.Admin
{
    public partial class AddProductSizePicture : System.Web.UI.Page
    {
        Guid productId;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Request.QueryString["productId"]))
            {
                Guid.TryParse(Request.QueryString["productId"], out productId);
            }
            if (productId.Equals(Guid.Empty))
            {
                MessageBox.Messager(this.Page, "参数[productId]无效，请检查", "错误提示", "error");
                return;
            }

            if (!Page.IsPostBack)
            {
                BindProductItem();

                Bind();
            }
        }

        private void Bind()
        {
            if (!string.IsNullOrWhiteSpace(Request.QueryString["productItemId"]))
            {
                Guid productItemId = Guid.Empty;
                Guid.TryParse(Request.QueryString["productItemId"], out productItemId);
                if (!productItemId.Equals(Guid.Empty))
                {
                    ProductSizePicture bll = new ProductSizePicture();
                    var model = bll.GetModelByJoin(productId, productItemId);
                    if (model != null)
                    {
                        hAction_ProductSizePicture.Value = Request.QueryString["action"];
                        txtName.Value = model.Named;
                        string pictureUrl = "";
                        if (!string.IsNullOrWhiteSpace(model.FileDirectory))
                        {
                            pictureUrl = PictureUrlHelper.GetMPicture(model.FileDirectory, model.RandomFolder, model.FileExtension);
                        }
                        imgProductSizePicture.Src = string.IsNullOrWhiteSpace(pictureUrl) ? "../../Images/nopic.gif" : pictureUrl;
                        hImgProductSizePictureId.Value = model.PictureId.ToString();
                        var li = ddlProductItem_ProductSizePicture.Items.FindByValue(model.ProductItemId.ToString());
                        if (li != null) li.Selected = true;
                    }
                }
            }
        }

        private void BindProductItem()
        {
            ddlProductItem_ProductSizePicture.Items.Add(new ListItem("请选择", "-1"));
            ProductItem piBll = new ProductItem();
            var dic = piBll.GetKeyValueByProductId(productId);
            if (dic == null)
            {
                return;
            }
            foreach (KeyValuePair<string, string> kvp in dic)
            {
                ddlProductItem_ProductSizePicture.Items.Add(new ListItem(kvp.Value, kvp.Key));
            }
        }
    }
}