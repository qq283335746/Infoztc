﻿using System;
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

namespace TygaSoft.Web.Admin.ActivityNew
{
    public partial class ListWinningRecord : System.Web.UI.Page
    {
        StringBuilder myDataAppend = new StringBuilder(300);
        int pageIndex = WebCommon.PageIndex;
        int pageSize = WebCommon.PageSize10;
        string queryStr;
        ParamsHelper parms;
        string sqlWhere;
        string num;

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
            //查询条件 
            GetSearchItem();

            int totalRecords = 0;
            WinningRecord bll = new WinningRecord();

            sqlWhere += "and ActivityPrizeId <> '00000000-0000-0000-0000-000000000000' ";

            rpData.DataSource = bll.GetListOW(pageIndex, pageSize, out totalRecords, sqlWhere, parms == null ? null : parms.ToArray());
            rpData.DataBind();

            myDataAppend.Append("<div id=\"myDataForPage\" style=\"display:none;\">[{\"PageIndex\":\"" + pageIndex + "\",\"PageSize\":\"" + pageSize + "\",\"TotalRecord\":\"" + totalRecords + "\",\"QueryStr\":\"" + queryStr + "\"}]</div>");
        }

        private void GetSearchItem()
        {
            if (!string.IsNullOrEmpty(asId.Value))
            {
                if (parms == null) parms = new ParamsHelper();

                sqlWhere += "and WR.ActivityId = @ActivityId ";
                SqlParameter parm = new SqlParameter("@ActivityId", SqlDbType.UniqueIdentifier);
                parm.Value = Guid.Parse(asId.Value);
                parms.Add(parm);
            }

            if (!string.IsNullOrWhiteSpace(num))
            {
                if (parms == null) parms = new ParamsHelper();

                sqlWhere += "and WR.MobilePhone like @MobilePhone ";
                SqlParameter parm = new SqlParameter("@MobilePhone", SqlDbType.VarChar, 20);
                parm.Value = "%" + num + "%";
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
                case "num":
                    txtNum.Value = nvc[key];
                    num = nvc[key];
                    break;
                case "asId":
                    asId.Value = nvc[key];
                    break;
                default:
                    break;
            }
        }
    }
}