using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TygaSoft.BLL;
using TygaSoft.Model;

namespace TygaSoft.Web.Admin.Lottery
{
    public partial class AddErnie : System.Web.UI.Page
    {
        private Guid Id;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Request.QueryString["Id"]))
            {
                Guid.TryParse(Request.QueryString["Id"], out Id);
            }
            if (!Page.IsPostBack)
            {
                BindIsDisable();

                Bind();
            }
        }

        private void Bind()
        {
            Ernie bll = new Ernie();

            if (!Id.Equals(Guid.Empty))
            {
                Page.Title = "编辑摇奖设定";

                var model = bll.GetModel(Id);
                if (model == null) return;

                hErnieId.Value = model.Id.ToString();
                txtStartTime.Value = model.StartTime.ToString("yyyy-MM-dd HH:mm");
                txtEndTime.Value = model.EndTime.ToString("yyyy-MM-dd HH:mm");
                txtUserBetMaxCount.Value = model.UserBetMaxCount.ToString();
                var li = rdIsDisable.Items.FindByValue(model.IsDisable.ToString().ToLower());
                if (li != null) li.Selected = true;
            }
            else
            {
                if (bll.IsExistLatest())
                {
                    abtnSaveErnie.Visible = false;
                    lbMsg.InnerText = "系统提示：已存在摇奖设置且未结束，无法再添加摇奖设置！";
                }
            }
        }

        private void BindIsDisable()
        {
            rdIsDisable.Items.Add(new ListItem("否", "false"));
            rdIsDisable.Items.Add(new ListItem("是", "true"));
            var li = rdIsDisable.Items.FindByValue("false");
            if (li != null) li.Selected = true;
        }
    }
}