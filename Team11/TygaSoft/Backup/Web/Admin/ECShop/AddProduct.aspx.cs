using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TygaSoft.Model;
using TygaSoft.BLL;

namespace TygaSoft.Web.Admin.ECShop
{
    public partial class AddProduct : System.Web.UI.Page
    {
        Guid Id;
        string myDataAppend;


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

            ltrMyData.Text = "<div id=\"myDataAppend\" style=\"display:none;\">" + myDataAppend + "</div>";
        }

        private void Bind()
        {
            if (!Id.Equals(Guid.Empty))
            {
                Page.Title = "编辑商品";

                ProductItem piBll = new ProductItem();
                var model = piBll.GetModelByJoin(Id);
                if (model != null)
                {
                    txtProductName.Value = model.ProductName;
                    txtSubTitle.Value = model.SubTitle;
                    txtOriginalPrice.Value = model.OriginalPrice.ToString("N");
                    txtProductPrice.Value = model.ProductPrice.ToString("N");
                    txtDiscount.Value = model.Discount.ToString();
                    txtDiscountDescri.Value = model.DiscountDescri;
                    txtStockNum.Value = model.StockNum.ToString();
                    txtContent.Value = model.Descr;
                    hId.Value = Id.ToString();

                    ProductPicture ppBll = new ProductPicture();

                    myDataAppend += "<div id=\"myDataForModel\">[";
                    myDataAppend += "{\"ProductPictureId\":\"" + model.ProductPictureId + "\",\"ImageUrl\":\"" + model.ImageUrl + "\",\"OtherPicture\":" + ppBll.GetJson(model.OtherPicture) + ",\"SizeItem\":" + model.SizeItem + ",\"SizePicture\":\"" + model.SizePicture + "\",\"CategoryId\":\"" + model.CategoryId + "\",\"BrandId\":\"" + model.BrandId + "\",\"CustomMenuId\":\"" + model.CustomMenuId.ToString().Replace(Guid.Empty.ToString(),"") + "\"}";
                    myDataAppend += "]</div>";
                }
            }
        }
    }
}