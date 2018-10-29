using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TygaSoft.Model;
using TygaSoft.BLL;
using TygaSoft.WebHelper;

namespace TygaSoft.Web.Templates.Admin
{
    public partial class AddProductDetail : System.Web.UI.Page
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
                MessageBox.Messager(this.Page, "参数[productId]无效，请检查","错误提示","error");
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
                    ProductDetail bll = new ProductDetail();
                    var model = bll.GetModel(productId, productItemId);
                    if (model != null)
                    {
                        hAction_ProductDetail.Value = Request.QueryString["action"];
                        var li = ddlProductItem.Items.FindByValue(model.ProductItemId.ToString());
                        if (li != null) li.Selected = true;
                        txtOriginalPrice.Value = model.OriginalPrice.ToString();
                        txtProductPrice.Value = model.ProductPrice.ToString();
                        txtDiscount.Value = model.Discount.ToString();
                        txtDiscountDescri.Value = model.DiscountDescr;
                        txtaContent.Value = model.ContentText;
                    }
                }
            }
        }

        private void BindProductItem()
        {
            ddlProductItem.Items.Add(new ListItem("请选择", "-1"));
            ProductItem piBll = new ProductItem();
            var dic = piBll.GetKeyValueByProductId(productId);
            if (dic == null)
            {
                return;
            }
            foreach (KeyValuePair<string, string> kvp in dic)
            {
                ddlProductItem.Items.Add(new ListItem(kvp.Value, kvp.Key));
            }
        }
    }
}