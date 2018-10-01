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
                ServiceLink bll = new ServiceLink();
                var model = bll.GetModelByJoin(Id);
                if (model != null)
                {
                    hServiceLinkId.Value = model.Id.ToString();
                    txtNamed_ServiceLink.Value = model.Named;
                    imgPicture_ServiceLink.Src = string.IsNullOrWhiteSpace(model.MPicture) ? "../../Images/nopic.gif" : model.MPicture;
                    hPictureId_ServiceLink.Value = model.PictureId.ToString();
                    txtUrl_ServiceLink.Value = model.Url;
                    myDataAppend.Append("<div code=\"myDataForModel\">[{\"Id\":\"" + model.Id + "\",\"ServiceItemId\":\"" + model.ServiceItemId + "\",\"ServiceItemName\":\"" + model.ServiceItemName + "\",\"Sort\":\"" + model.Sort + "\"}]</div>");
                }
            }
        }
    }
}