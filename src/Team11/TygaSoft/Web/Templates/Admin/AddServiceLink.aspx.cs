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
    public partial class AddServiceLink : System.Web.UI.Page
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
                myDataAppend.Append("<div id=\"myDataAppend_ServiceLink\" style=\"display:none;\">");

                Bind();

                myDataAppend.Append("</div>");

                ltrMyData.Text = myDataAppend.ToString();
            }
        }

        private void Bind()
        {
            if (!Id.Equals(Guid.Empty))
            {
                ServiceLink bll = new ServiceLink();
                var model = bll.GetModelByJoin(Id);
                if (model != null)
                {
                    hServiceLinkId.Value = model.Id.ToString();
                    txtNamed_ServiceLink.Value = model.Named;

                    string pictureUrl = "";
                    if (!string.IsNullOrWhiteSpace(model.FileDirectory))
                    {
                        pictureUrl = PictureUrlHelper.GetMPicture(model.FileDirectory, model.RandomFolder, model.FileExtension);
                    }

                    imgPicture_ServiceLink.Src = string.IsNullOrWhiteSpace(pictureUrl) ? "../../Images/nopic.gif" : pictureUrl;
                    hPictureId_ServiceLink.Value = model.PictureId.ToString();
                    txtUrl_ServiceLink.Value = model.Url;
                    txtEnableStartTime_ServiceLink.Value = model.EnableStartTime == DateTime.MinValue ? "" : model.EnableStartTime.ToString("yyyy-MM-dd HH:mm:ss");
                    txtEnableEndTime_ServiceLink.Value = model.EnableStartTime == DateTime.MinValue ? "" : model.EnableStartTime.ToString("yyyy-MM-dd HH:mm:ss");
                    var li = rbtnList_ServiceLink.Items.FindByValue(model.IsDisable.ToString().ToLower());
                    if (li != null) li.Selected = true;

                    myDataAppend.Append("<div code=\"myDataForModel\">[{\"Id\":\"" + model.Id + "\",\"ServiceItemId\":\"" + model.ServiceItemId + "\",\"ServiceItemName\":\"" + model.ServiceItemName + "\",\"Sort\":\"" + model.Sort + "\"}]</div>");
                }
            }
        }

        private void BindIsDisable()
        {
            rbtnList_ServiceLink.Items.Add(new ListItem("否", "false"));
            rbtnList_ServiceLink.Items.Add(new ListItem("是", "true"));
            var li = rbtnList_ServiceLink.Items.FindByValue("false");
            if (li != null) li.Selected = true;
        }
    }
}