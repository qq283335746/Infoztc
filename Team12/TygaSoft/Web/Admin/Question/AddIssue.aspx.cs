using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using TygaSoft.Model;
using TygaSoft.BLL;
using TygaSoft.WebHelper;
using TygaSoft.DBUtility;
using System.Data;
using System.Data.SqlClient;

namespace TygaSoft.Web.Admin.Question
{
    public partial class AddIssue : System.Web.UI.Page
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
                BindQBList();
            }
        }

        private void Bind()
        {
            if (!Id.Equals(Guid.Empty))
            {
                Page.Title = "编辑发布";

                ActivityRelease bll = new ActivityRelease();
                var model = bll.GetModel(Id);
                if (model != null)
                {
                    txtName.Value = model.Title;
                    startDate.Value = model.StartDate.ToString("yyyy-MM-dd HH:mm:ss");
                    endDate.Value = model.EndDate.ToString("yyyy-MM-dd HH:mm:ss");
                    txtCount.Value = model.QuestionCount.ToString();
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
        }

        private void BindQBList()
        {
            StringBuilder outHtml = new StringBuilder();
            List<ActivityQuestionBankInfo> listAQB = new List<ActivityQuestionBankInfo>();

            if (!Id.Equals(Guid.Empty))
            {
                ActivityQuestionBank bllAQB = new ActivityQuestionBank();
                ParamsHelper parms = new ParamsHelper();
                string sqlWhere = "and ActivityReleaseId like @ActivityReleaseId ";
                SqlParameter parm = new SqlParameter("@ActivityReleaseId", SqlDbType.UniqueIdentifier);
                parm.Value = Id;
                parms.Add(parm);
                listAQB = bllAQB.GetList(sqlWhere, parms == null ? null : parms.ToArray()).ToList();
            }

            QuestionBank bll = new QuestionBank();
            List<QuestionBankInfo> list = bll.GetList().ToList();
            foreach (QuestionBankInfo info in list)
            {
                int count = 0;
                if (listAQB.Count > 0)
                {
                    Guid qbId = Guid.Parse(info.Id.ToString());
                    ActivityQuestionBankInfo model = listAQB.Find(p => p.QuestionBankId == qbId);
                    count = model == null ? 0 : model.QuestionCount;
                }

                outHtml.AppendFormat("<div style=\"padding-top:5px\"><label>{0}</label>&nbsp;<input type=\"text\" id=\"{1}\" name=\"qbList\" value=\"{2}\" class=\"easyui-numberbox\"" +
                    "data-options=\"required:true,missingMessage:'必填项'\" style=\"width:60px\" />&nbsp;道</div>", info.Named, info.Id, count);
            }
            qbList.InnerHtml = outHtml.ToString();
        }
    }
}