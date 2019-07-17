using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using TygaSoft.Model;
using TygaSoft.BLL;
using TygaSoft.WebHelper;

namespace TygaSoft.Web.Templates.Admin
{
    public partial class AddProductStock : System.Web.UI.Page
    {
        Guid Id;
        Guid productId;
        StringBuilder myDataAppend;

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
                Bind();
            }
            if (myDataAppend == null)
            {
                ltrMyData.Text = "<div id=\"myDataAppend_ProductStock\" style=\"display:none;\"></div>";
            }
            else
            {
                ltrMyData.Text = "<div id=\"myDataAppend_ProductStock\" style=\"display:none;\">" + myDataAppend.ToString() + "</div>";
            }

        }

        private void Bind()
        {
            if (!string.IsNullOrWhiteSpace(Request.QueryString["Id"]))
            {
                Guid.TryParse(Request.QueryString["Id"], out Id);
            }

            if (!Id.Equals(Guid.Empty))
            {
                ProductStock bll = new ProductStock();
                var model = bll.GetModel(Id);
                if (model != null)
                {
                    hProductStockId.Value = model.Id.ToString();
                    txtName.Value = model.StockNum.ToString();

                    if (myDataAppend == null) myDataAppend = new StringBuilder();
                    myDataAppend.Append("<div id=\"myDataForModel_ProductStock\">[");
                    myDataAppend.Append("{\"ProductId\":\"" + model.ProductId + "\",\"ProductItemId\":\"" + model.ProductItemId + "\",\"ProductSize\":\"" + model.ProductSize + "\"}");
                    myDataAppend.Append("]</div>");
                }
            }
        }
    }
}