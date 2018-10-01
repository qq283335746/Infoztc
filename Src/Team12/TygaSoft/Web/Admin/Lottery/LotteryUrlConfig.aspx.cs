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

namespace TygaSoft.Web.Admin.Lottery
{
    public partial class LotteryUrlConfig : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDiv();
            }
        }

        private void BindDiv()
        {
            InitItems bll = new InitItems();
            List<InitItemsInfo> list = new List<InitItemsInfo>();
            ParamsHelper parms = new ParamsHelper();

            string sqlWhere = "and ItemType = @ItemType ";
            SqlParameter parm = new SqlParameter("@ItemType", SqlDbType.VarChar, 30);
            parm.Value = "ztc-qxc";
            parms.Add(parm);
            list = bll.GetList(sqlWhere, parms.ToArray()).ToList();

            StringBuilder outHtml = new StringBuilder();
            foreach (InitItemsInfo info in list)
            {
                switch (info.ItemCode)
                {
                    case "CPshenma":
                        txtCPshenma.Value = info.ItemKey;
                        break;
                    case "CPshiping":
                        txtCPshiping.Value = info.ItemKey;
                        break;
                    case "CPhuodong":
                        txtCPhuodong.Value = info.ItemKey;
                        break;
                }
            }
        }
    }
}