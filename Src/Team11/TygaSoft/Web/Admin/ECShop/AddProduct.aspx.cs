using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Newtonsoft.Json;
using TygaSoft.Model;
using TygaSoft.BLL;
using TygaSoft.SysHelper;

namespace TygaSoft.Web.Admin.ECShop
{
    public partial class AddProduct : System.Web.UI.Page
    {
        Guid Id;
        StringBuilder myDataAppend;


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
            if (myDataAppend == null)
            {
                ltrMyData.Text = "<div id=\"myDataAppend\" style=\"display:none;\"></div>";
            }
            else
            {
                ltrMyData.Text = "<div id=\"myDataAppend\" style=\"display:none;\">" + myDataAppend.ToString() + "</div>";
            }
        }

        private void Bind()
        {
            if (!Id.Equals(Guid.Empty))
            {
                Page.Title = "编辑商品";

                Product piBll = new Product();
                var model = piBll.GetModelByJoin(Id);
                if (model != null)
                {
                    txtName.Value = model.Named;
                    txtSort.Value = model.Sort.ToString();

                    string pictureUrl = "";
                    if (!string.IsNullOrWhiteSpace(model.FileDirectory))
                    {
                        pictureUrl = PictureUrlHelper.GetMPicture(model.FileDirectory, model.RandomFolder, model.FileExtension);
                    }
                    imgProductPicture.Src = string.IsNullOrWhiteSpace(pictureUrl) ? "../../Images/nopic.gif" : pictureUrl;
                    hImgProductPictureId.Value = model.PictureId.ToString();
                    txtEnableStartTime.Value = model.EnableStartTime.ToString("yyyy-MM-dd HH:mm:ss");
                    txtEnableEndTime.Value = model.EnableEndTime.ToString("yyyy-MM-dd HH:mm:ss");
                    hId.Value = Id.ToString();

                    if (myDataAppend == null) myDataAppend = new StringBuilder();
                    myDataAppend.Append("<div id=\"myDataForModel\">[");
                    myDataAppend.Append("{\"IsEnable\":\"" + model.IsEnable.ToString().ToLower() + "\",\"IsDisable\":\"" + model.IsDisable.ToString().ToLower() + "\",\"CategoryId\":\"" + model.CategoryId + "\",\"BrandId\":\"" + model.BrandId + "\",\"MenuId\":\"" + model.MenuId.ToString().Replace(Guid.Empty.ToString(), "") + "\"}");
                    myDataAppend.Append("]</div>");
                }
            }
        }
    }
}