using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.IO;
using TygaSoft.SysHelper;
using TygaSoft.Model;
using TygaSoft.BLL;
using TygaSoft.WebHelper;

namespace TygaSoft.Web.Templates.Admin.AboutSite
{
    public partial class AddAdItem : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindCbbActionType();

                Bind();
            }
        }

        private void Bind()
        {
            Guid adItemId = Guid.Empty;
            if (!string.IsNullOrWhiteSpace(Request.QueryString["adItemId"])) Guid.TryParse(Request.QueryString["adItemId"], out adItemId);
            if (!adItemId.Equals(Guid.Empty))
            {
                ddlActionType.Enabled = false;

                AdItem bll = new AdItem();
                var model = bll.GetModelByJoin(adItemId);
                if (model == null)
                {
                    MessageBox.Messager(this.Page, MessageContent.GetString(MessageContent.Submit_Data_NotExists, "广告项ID【" + adItemId + "】"), MessageContent.AlertTitle_Ex_Error, "error");
                    return;
                }
                hAdItemId.Value = model.Id.ToString();
                hImgPictureId.Value = model.PictureId.ToString();
                imgPicture.Src = PictureUrlHelper.GetMPicture(model.FileDirectory, model.RandomFolder, model.FileExtension);
                var li = ddlActionType.Items.FindByValue(model.ActionTypeId.ToString());
                if (li != null) li.Selected = true;
                txtSort.Value = model.Sort.ToString();
                cbIsDisable.Checked = model.IsDisable;

                ltrMyOldData.Text = "<span id=\"myOldData\" style=\"display:none;\">{\"PictureId\":\"" + model.PictureId + "\",\"ActionTypeId\":\"" + model.ActionTypeId + "\",\"Sort\":\"" + model.Sort + "\",\"IsDisable\":\"" + model.IsDisable + "\"}</span>";
            }
        }

        /// <summary>
        /// 绑定广告作用类别
        /// </summary>
        private void BindCbbActionType()
        {
            ddlActionType.Items.Add(new ListItem("请选择", "-1"));
            ContentType ctBll = new ContentType();
            var dic = ctBll.GetKeyValueByParent("AdvertisementCategory");
            foreach (var kvp in dic)
            {
                ddlActionType.Items.Add(new ListItem(kvp.Value, kvp.Key.ToString()));
            }
        }
    }
}