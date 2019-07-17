using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TygaSoft.Model;
using TygaSoft.BLL;
using TygaSoft.WebHelper;

namespace TygaSoft.Web.Admin.Question
{
    public partial class AddAnswerOption : System.Web.UI.Page
    {
        Guid Id;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request.QueryString["Id"] != null)
                {
                    Guid.TryParse(Request.QueryString["Id"], out Id);
                }

                Bind();
            }
        }

        private void Bind()
        {
            if (!Id.Equals(Guid.Empty))
            {
                Page.Title = "编辑答案想选";

                AnswerOption bll = new AnswerOption();
                var model = bll.GetModel(Id);
                if (model != null)
                {
                    txtName.Value = model.OptionContent;
                    qsId.Value = model.QuestionSubjectId.ToString();
                    selectIsTrue.Value = model.IsTrue.ToString();
                    sort.Value = model.Sort.ToString();
                    hId.Value = Id.ToString();
                    if (model.IsDisable)
                    {
                        rdFalse.Checked = false;
                        rdTrue.Checked = true;
                    }
                    else
                    {
                        rdFalse.Checked = true;
                        rdTrue.Checked = false;
                    }
                }
            }
            else
            {
                qsId.Value = Request["qsId"];
            }
        }
    }
}