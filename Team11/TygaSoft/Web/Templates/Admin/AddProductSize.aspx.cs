using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TygaSoft.Model;
using TygaSoft.BLL;
using TygaSoft.WebHelper;

namespace TygaSoft.Web.Templates.Admin
{
    public partial class AddProductSize : System.Web.UI.Page
    {
        Guid productId;
        Guid productItemId;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Request.QueryString["productId"]))
            {
                Guid.TryParse(Request.QueryString["productId"], out productId);
            }
            if (!string.IsNullOrWhiteSpace(Request.QueryString["productItemId"]))
            {
                Guid.TryParse(Request.QueryString["productItemId"], out productItemId);
            }
            if (productId.Equals(Guid.Empty))
            {
                MessageBox.Messager(this.Page, "参数（productId）无效，请检查", "错误提示", "error");
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
            IList<string> listSize = new List<string>();

            if (!productItemId.Equals(Guid.Empty))
            {
                ProductSize bll = new ProductSize();
                var model = bll.GetModel(productItemId);
                if (model != null)
                {
                    var li = ddlProductItem_ProductSize.Items.FindByValue(model.ProductItemId.ToString());
                    if (li != null) li.Selected = true;

                    XElement xel = XElement.Parse(model.SizeAppend);
                    var q = from x in xel.Descendants("Data")
                            select new { name = x.Element("Name").Value };
                    foreach (var item in q)
                    {
                        listSize.Add(item.name);
                    }
                }
            }

            if (listSize.Count == 0)
            {
                listSize.Add("");
            }

            rpData.DataSource = listSize;
            rpData.DataBind();
        }

        private void BindProductItem()
        {
            ddlProductItem_ProductSize.Items.Add(new ListItem("请选择", "-1"));
            ProductItem piBll = new ProductItem();
            var dic = piBll.GetKeyValueByProductId(productId);
            if (dic == null)
            {
                return;
            }
            foreach (KeyValuePair<string, string> kvp in dic)
            {
                ddlProductItem_ProductSize.Items.Add(new ListItem(kvp.Value, kvp.Key));
            }
        }
    }
}