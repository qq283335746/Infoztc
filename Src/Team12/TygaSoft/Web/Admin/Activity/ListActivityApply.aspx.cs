using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Specialized;
using System.Text;
using TygaSoft.Model;
using TygaSoft.BLL;
using TygaSoft.WebHelper;
using TygaSoft.DBUtility;
using TygaSoft.SysHelper;

namespace TygaSoft.Web.Admin.Activity
{
    public partial class ListActivityApply : System.Web.UI.Page
    {
        StringBuilder myDataAppend = new StringBuilder(300);
        int pageIndex = WebCommon.PageIndex;
        int pageSize = WebCommon.PageSize10;
        string queryStr;
        ParamsHelper parms;
        string sqlWhere;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                NameValueCollection nvc = Request.QueryString;
                int index = 0;
                foreach (string item in nvc.AllKeys)
                {
                    GetParms(item, nvc);

                    if (item != "pageIndex" && item != "pageSize")
                    {
                        index++;
                        if (index > 1) queryStr += "&";
                        queryStr += string.Format("{0}={1}", item, Server.HtmlEncode(nvc[item]));
                    }
                }

                Bind();
            }

            ltrMyData.Text = "<div id=\"myDataAppend\" style=\"display:none;\">" + myDataAppend.ToString() + "</div>";
        }

        private void Bind()
        {
            int totalRecords = 0;
            if (!string.IsNullOrEmpty(asId.Value))
            {
                //查询条件 
                GetSearchItem();


                ActivitySignUp bll = new ActivitySignUp();

                rpData.DataSource = bll.GetListOW(pageIndex, pageSize, out totalRecords, sqlWhere, parms == null ? null : parms.ToArray());
                rpData.DataBind();
            }
            myDataAppend.Append("<div id=\"myDataForPage\" style=\"display:none;\">[{\"PageIndex\":\"" + pageIndex + "\",\"PageSize\":\"" + pageSize + "\",\"TotalRecord\":\"" + totalRecords + "\",\"QueryStr\":\"" + queryStr + "\"}]</div>");
        }

        private void GetSearchItem()
        {
            if (!string.IsNullOrWhiteSpace(asId.Value))
            {
                if (parms == null) parms = new ParamsHelper();

                sqlWhere += " AND asu.ActivityId = @ActivityId ";
                SqlParameter parm = new SqlParameter("@ActivityId", SqlDbType.UniqueIdentifier);
                parm.Value = Guid.Parse(asId.Value);
                parms.Add(parm);
            }
        }

        private void GetParms(string key, NameValueCollection nvc)
        {
            switch (key)
            {
                case "pageIndex":
                    Int32.TryParse(nvc[key], out pageIndex);
                    break;
                case "pageSize":
                    Int32.TryParse(nvc[key], out pageSize);
                    break;
                case "asId":
                    asId.Value = nvc[key];
                    break;
                default:
                    break;
            }
        }

        public string ExportExcel()
        {
            DataTable ExcelData = CreateExcelData(asId.Value);
            if (ExcelData == null)
            {
                return "1";
            }
            NPOIHelper_ToExcel Helper_ToExcel = new NPOIHelper_ToExcel(ExcelData, "活动报名名单");
            Helper_ToExcel.addExcelColumnWidth(0, 10);
            Helper_ToExcel.SheetName = "报名人员名单";
            Helper_ToExcel.HeaderText = "报名人员名单";
            //Helper_ToExcel.AddGroupByName("报名人员名单");
            Helper_ToExcel.ExportExcelByWebAsExcel();

            return "";
        }

        public DataTable CreateExcelData(string id)
        {
            DataTable ExcelData = new DataTable();
            ExcelData.Columns.Add("活动标题", typeof(string));
            ExcelData.Columns.Add("报名姓名", typeof(string));
            ExcelData.Columns.Add("性别", typeof(string));
            ExcelData.Columns.Add("联系方式", typeof(string));
            ExcelData.Columns.Add("报名昵称", typeof(string));
            ExcelData.Columns.Add("最后更新时间", typeof(string));

            ParamsHelper parms = new ParamsHelper();
            string sqlWhere = " AND asu.ActivityId = @ActivityId ";
            SqlParameter parm = new SqlParameter("@ActivityId", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(id);
            parms.Add(parm);
            ActivitySignUp bll = new ActivitySignUp();
            DataSet ds = bll.ExportExcel(sqlWhere, parms == null ? null : parms.ToArray());

            if (ds == null)
            {
                return null;
            }

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                DataRow NewRow = ExcelData.NewRow();
                NewRow["活动标题"] = row["Title"].ToString();
                NewRow["报名姓名"] = row["UserName"].ToString();
                NewRow["性别"] = row["Sex"].ToString();
                NewRow["联系方式"] = row["MobilePhone"].ToString();
                NewRow["报名昵称"] = row["Nickname"].ToString();
                NewRow["最后更新时间"] = row["LastUpdatedDate"].ToString();

                ExcelData.Rows.Add(NewRow);
            }
            return ExcelData;
        }

        protected void exp_Click(object sender, EventArgs e)
        {
            ExportExcel();
        }

    }
}