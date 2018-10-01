using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TygaSoft.BLL;
using TygaSoft.Model;
using TygaSoft.WebHelper;

namespace TygaSoft.Web.Templates.Admin.AboutSite
{
    public partial class AddAdItemContent : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Request.QueryString["adItemId"]))
            {
                MessageBox.Messager(this.Page, "请先完成上一步的操作再进行此操作！", MessageContent.AlertTitle_Error, "error");
                return;
            }

            if (!Page.IsPostBack)
            {
                Bind();
            }
        }

        private void Bind()
        {
            Guid adItemId = Guid.Empty;
            Guid.TryParse(Request.QueryString["adItemId"], out adItemId);
            if (!adItemId.Equals(Guid.Empty))
            {
                AdItemContent bll = new AdItemContent();
                var model = bll.GetModel(adItemId);
                if (model != null)
                {
                    txtaDescr.Value = model.Descr;
                    txtaContent.Value = model.ContentText;
                }
            }
        }
    }
}