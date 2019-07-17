using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading.Tasks;
using System.Text;
using TygaSoft.Model;
using TygaSoft.BLL;
using TygaSoft.SysHelper;

namespace TygaSoft.Web.Templates.Admin
{
    public partial class AddServiceContent : System.Web.UI.Page
    {
        StringBuilder myDataAppend;
        Guid Id;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!string.IsNullOrWhiteSpace(Request.QueryString["Id"]))
                {
                    Guid.TryParse(Request.QueryString["Id"], out Id);
                }

                BindIsDisable();

                if (myDataAppend == null) myDataAppend = new StringBuilder();
                myDataAppend.Append("<div id=\"myDataAppend_ServiceContent\" style=\"display:none;\">");

                Bind();

                myDataAppend.Append("</div>");

                ltrMyData.Text = myDataAppend.ToString();
            }
        }

        private void Bind()
        {
            if (!Id.Equals(Guid.Empty))
            {
                ServiceContent bll = new ServiceContent();
                var model = bll.GetModelByJoin(Id);
                if (model != null)
                {
                    myDataAppend.Append("<div code=\"myDataForModel\">[{\"Id\":\"" + model.Id + "\",\"ServiceItemId\":\"" + model.ServiceItemId + "\",\"ServiceItemName\":\"" + model.ServiceItemName + "\",\"Sort\":\"" + model.Sort + "\"}]</div>");

                    hServiceContentId.Value = model.Id.ToString();
                    txtNamed_ServiceContent.Value = model.Named;

                    string pictureUrl = "";
                    if (!string.IsNullOrWhiteSpace(model.FileDirectory))
                    {
                        pictureUrl = PictureUrlHelper.GetMPicture(model.FileDirectory, model.RandomFolder, model.FileExtension);
                    }
                    imgPicture_ServiceContent.Src = string.IsNullOrWhiteSpace(pictureUrl) ? "../../Images/nopic.gif" : pictureUrl;
                    hPictureId_ServiceContent.Value = model.PictureId.ToString();
                    txtaDescr_ServiceContent.Value = model.Descr;
                    txtaContent_ServiceContent.Value = model.ContentText;
                    txtEnableStartTime_ServiceContent.Value = model.EnableStartTime == DateTime.MinValue ? "" : model.EnableStartTime.ToString("yyyy-MM-dd HH:mm:ss");
                    txtEnableEndTime_ServiceContent.Value = model.EnableStartTime == DateTime.MinValue ? "" : model.EnableStartTime.ToString("yyyy-MM-dd HH:mm:ss");
                    var li = rbtnList_ServiceContent.Items.FindByValue(model.IsDisable.ToString().ToLower());
                    if (li != null) li.Selected = true;
                }
            }
        }

        private void BindIsDisable()
        {
            rbtnList_ServiceContent.Items.Add(new ListItem("否", "false"));
            rbtnList_ServiceContent.Items.Add(new ListItem("是", "true"));
            var li = rbtnList_ServiceContent.Items.FindByValue("false");
            if (li != null) li.Selected = true;
        }
    }
}