using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TygaSoft.SysHelper;
using TygaSoft.Model;
using TygaSoft.BLL;

namespace TygaSoft.Web.Templates.Admin.Lottery
{
    public partial class DlgErnieItem : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindDdlNumType();

                BindCbNum();

                Bind();
            }
        }

        private void Bind()
        {
            Guid Id = Guid.Empty;
            if (!string.IsNullOrWhiteSpace(Request.QueryString["Id"])) Guid.TryParse(Request.QueryString["Id"], out Id);
            if (!Id.Equals(Guid.Empty))
            {
                ErnieItem bll = new ErnieItem();
                var model = bll.GetModel(Id);
                if (model != null)
                {
                    hErnieItemId.Value = model.Id.ToString();
                    var li = ddlNumType.Items.FindByValue(model.NumType);
                    if (li != null) li.Selected = true;
                    string[] nums = model.Num.Split(new char[]{','},StringSplitOptions.RemoveEmptyEntries);
                    foreach(var num in nums)
                    {
                        li = cbListNum.Items.FindByValue(num);
                        if (li != null) li.Selected = true;
                    }
                    txtAppearRatio.Value = Math.Round((double)model.AppearRatio,2).ToString();
                    
                }
            }
        }

        /// <summary>
        /// 绑定奖励类型
        /// </summary>
        private void BindDdlNumType()
        {
            ddlNumType.Items.Add(new ListItem("请选择", "-1"));
            string[] items = Enum.GetNames(typeof(EnumData.ErnieItemNumType));
            foreach (var item in items)
            {
                ddlNumType.Items.Add(new ListItem(item, item));
            }
        }

        /// <summary>
        /// 绑定奖励数值
        /// </summary>
        private void BindCbNum()
        {
            List<int> list = new List<int>();
            for (var i = 1; i < 11; i++)
            {
                list.Add(i);
            }
            cbListNum.DataSource = list;
            cbListNum.DataBind();
        }
    }
}