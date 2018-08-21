using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using TygaSoft.WebHelper;
using TygaSoft.BLL;
using TygaSoft.Model;

namespace TygaSoft.Web.Admin.Sys
{
    public partial class AddUserBase : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindSex();

                Bind();
            }
        }

        private void Bind()
        {
            string userName = Request.QueryString["userName"];
            if (string.IsNullOrWhiteSpace(userName))
            {
                MessageBox.Messager(this.Page, MessageContent.Request_InvalidError, MessageContent.AlertTitle_Sys_Info, "error");
                return;
            }
            MembershipUser user = Membership.GetUser(userName);
            if (user == null)
            {
                MessageBox.Messager(this.Page, MessageContent.GetString(MessageContent.Request_NotExist,"用户【"+userName+"】"), MessageContent.AlertTitle_Sys_Info, "error");
                return;
            }
            hName.Value = userName;

            UserBase ubBll = new UserBase();
            var model = ubBll.GetModel(user.ProviderUserKey);
            if (model != null)
            {
                txtNickname.Value = model.Nickname;
                imgSinglePicture.Src = string.IsNullOrWhiteSpace(model.HeadPicture) ? "../../Images/nopic.gif" : model.HeadPicture;
                var li = rbtnSex.Items.FindByValue(model.Sex);
                if (li != null) li.Selected = true;
                txtMobilePhone.Value = model.MobilePhone;
                txtTotalGold.Value = model.TotalGold.ToString();
                txtTotalSilver.Value = model.TotalSilver.ToString();
                txtTotalIntegral.Value = model.TotalIntegral.ToString();
                txtSilverLevel.Value = model.SilverLevel.ToString();
                txtIntegralLevel.Value = model.IntegralLevel.ToString();
                txtColorLevel.Value = model.ColorLevel.ToString();
            }
        }

        private void BindSex()
        {
            rbtnSex.Items.Add(new ListItem("男", "男"));
            rbtnSex.Items.Add(new ListItem("女", "女"));
            rbtnSex.Items[0].Selected = true;
        }
    }
}