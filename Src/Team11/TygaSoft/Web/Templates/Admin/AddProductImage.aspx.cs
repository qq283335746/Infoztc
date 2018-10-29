using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using TygaSoft.Model;
using TygaSoft.BLL;
using TygaSoft.WebHelper;
using TygaSoft.SysHelper;

namespace TygaSoft.Web.Templates.Admin
{
    public partial class AddProductImage : System.Web.UI.Page
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
            Dictionary<string, string> dicPicture = new Dictionary<string, string>();

            if (!productItemId.Equals(Guid.Empty))
            {
                ProductImage bll = new ProductImage();
                var model = bll.GetModel(productItemId);
                if (model != null)
                {
                    var li = ddlProductItem_ProductImage.Items.FindByValue(model.ProductItemId.ToString());
                    if (li != null) li.Selected = true;

                    XElement xel = XElement.Parse(model.PictureAppend);
                    var q = from x in xel.Descendants("Add")
                            select new { pictureId = x.Attribute("PictureId").Value };

                    StringBuilder sbInIds = new StringBuilder(1000);

                    foreach (var item in q)
                    {
                        sbInIds.AppendFormat("'{0}',", item.pictureId);
                    }

                    PictureProduct ppBll = new PictureProduct();
                    var pictureList = ppBll.GetListInIds(sbInIds.ToString().Trim(','));
                    if (pictureList != null && pictureList.Count > 0)
                    {
                        foreach (var picModel in pictureList)
                        {
                            dicPicture.Add(picModel.Id.ToString(), PictureUrlHelper.GetMPicture(picModel.FileDirectory, picModel.RandomFolder, picModel.FileExtension));
                        }
                    }
                }
            }

            if (dicPicture.Count == 0)
            {
                dicPicture.Add("", "../../Images/nopic.gif");
            }

            rpData.DataSource = dicPicture;
            rpData.DataBind();
        }

        private void BindProductItem()
        {
            ddlProductItem_ProductImage.Items.Add(new ListItem("请选择", "-1"));
            ProductItem piBll = new ProductItem();
            var dic = piBll.GetKeyValueByProductId(productId);
            if (dic == null)
            {
                return;
            }
            foreach (KeyValuePair<string, string> kvp in dic)
            {
                ddlProductItem_ProductImage.Items.Add(new ListItem(kvp.Value, kvp.Key));
            }
        }
    }
}