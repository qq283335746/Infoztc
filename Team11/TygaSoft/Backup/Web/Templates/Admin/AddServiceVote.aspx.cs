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
    public partial class AddServiceVote : System.Web.UI.Page
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
                ServiceVote bll = new ServiceVote();
                var model = bll.GetModelByJoin(Id);
                if (model != null)
                {
                    myDataAppend.Append("<div code=\"myDataForModel\">[{\"Id\":\"" + model.Id + "\",\"ServiceItemId\":\"" + model.ServiceItemId + "\",\"ServiceItemName\":\"" + model.ServiceItemName + "\",\"Sort\":\"" + model.Sort + "\"}]</div>");

                    hServiceVoteId.Value = model.Id.ToString();
                    txtNamed_ServiceVote.Value = model.Named;
                    imgHeadPicture_ServiceVote.Src = string.IsNullOrWhiteSpace(model.MPicture) ? "../../Images/nopic.gif" : model.MPicture;
                    hHeadPictureId_ServiceVote.Value = model.HeadPictureId.ToString();
                    txtaDescr_ServiceVote.Value = model.Descr;
                    txtaContent_ServiceVote.Value = model.ContentText;
                }
            }
        }
    }
}