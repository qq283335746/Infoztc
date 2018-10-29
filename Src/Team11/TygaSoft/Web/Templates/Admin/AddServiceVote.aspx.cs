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

                BindIsDisable();

                if (myDataAppend == null) myDataAppend = new StringBuilder();
                myDataAppend.Append("<div id=\"myDataAppend_ServiceVote\" style=\"display:none;\">");

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
                    string pictureUrl = "";
                    if (!string.IsNullOrWhiteSpace(model.FileDirectory))
                    {
                        pictureUrl = PictureUrlHelper.GetMPicture(model.FileDirectory, model.RandomFolder, model.FileExtension);
                    }
                    imgHeadPicture_ServiceVote.Src = string.IsNullOrWhiteSpace(pictureUrl) ? "../../Images/nopic.gif" : pictureUrl;
                    hHeadPictureId_ServiceVote.Value = model.HeadPictureId.ToString();
                    txtaDescr_ServiceVote.Value = model.Descr;
                    txtaContent_ServiceVote.Value = model.ContentText;
                    txtEnableStartTime_ServiceVote.Value = model.EnableStartTime == DateTime.MinValue ? "" : model.EnableStartTime.ToString("yyyy-MM-dd HH:mm:ss");
                    txtEnableEndTime_ServiceVote.Value = model.EnableStartTime == DateTime.MinValue ? "" : model.EnableStartTime.ToString("yyyy-MM-dd HH:mm:ss");
                    var li = rbtnList_ServiceVote.Items.FindByValue(model.IsDisable.ToString().ToLower());
                    if (li != null) li.Selected = true;
                }
            }
        }

        private void BindIsDisable()
        {
            rbtnList_ServiceVote.Items.Add(new ListItem("否", "false"));
            rbtnList_ServiceVote.Items.Add(new ListItem("是", "true"));
            var li = rbtnList_ServiceVote.Items.FindByValue("false");
            if (li != null) li.Selected = true;
        }
    }
}