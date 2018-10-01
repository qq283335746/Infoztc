using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using TygaSoft.Model;
using TygaSoft.IDAL;
using TygaSoft.DALFactory;

namespace TygaSoft.BLL
{
    public partial class SysEnum
    {
        #region Member

        /// <summary>
        /// 获取属于当前代号下的所有子节点
        /// </summary>
        /// <param name="enumCode"></param>
        /// <returns></returns>
        public Dictionary<object, string> GetKeyValueByParent(string parentCode)
        {
            string sqlWhere = "and t2.EnumCode = @EnumCode ";
            SqlParameter parm = new SqlParameter("@EnumCode", SqlDbType.NVarChar, 50);
            parm.Value = parentCode;

            return dal.GetKeyValue(sqlWhere, parm);
        }

        #endregion
    }
}
