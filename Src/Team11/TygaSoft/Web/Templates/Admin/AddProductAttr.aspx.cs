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
    public partial class AddProductAttr : System.Web.UI.Page
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
            Dictionary<string, string> dicAttr = new Dictionary<string, string>();

            if (!productItemId.Equals(Guid.Empty))
            {
                ProductAttr bll = new ProductAttr();
                var model = bll.GetModel(productItemId);
                if (model != null)
                {
                    var li = ddlProductItem.Items.FindByValue(model.ProductItemId.ToString());
                    if (li != null) li.Selected = true;

                    XElement xel = XElement.Parse(model.AttrValue);
                    var q = from x in xel.Descendants("Attr")
                            select new {name = x.Element("Name").Value,value = x.Element("Value").Value};
                    foreach (var item in q)
                    {
                        dicAttr.Add(item.name, item.value);
                    }
                }
            }

            if (dicAttr.Count == 0)
            {
                dicAttr.Add("", "");
            }

            rpData.DataSource = dicAttr;
            rpData.DataBind();
        }

        /// <summary>
        /// 绑定商品项
        /// </summary>
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