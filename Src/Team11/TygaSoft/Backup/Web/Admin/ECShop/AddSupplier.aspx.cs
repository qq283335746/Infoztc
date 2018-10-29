using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TygaSoft.Model;
using TygaSoft.BLL;
using TygaSoft.WebHelper;

namespace TygaSoft.Web.Admin.ECShop
{
    public partial class AddSupplier : System.Web.UI.Page
    {
        Guid Id;

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
        }

        private void Bind()
        {
            if (!Id.Equals(Guid.Empty))
            {
                Page.Title = "编辑供应商信息";

                Supplier bll = new Supplier();
                var model = bll.GetModel(Id);
                if (model != null)
                {
                    txtName.Value = model.SupplierName;
                    txtPhone.Value = model.Phone.Trim();
                    txtAddress.Value = model.Address;
                    lbHProvinceCity.InnerText = model.ProvinceCityName;
                    lbProvinceCity.InnerText = model.ProvinceCityName.Replace("#", " ");
                    hId.Value = Id.ToString();

                }
            }
        }
    }
}