using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using TygaSoft.IDAL;
using TygaSoft.Model;
using TygaSoft.DALFactory;
using TygaSoft.SysHelper;

namespace TygaSoft.BLL
{
    public partial class UserBase
    {
        #region UserBase Member

        /// <summary>
        /// 获取话题用户键值对
        /// </summary>
        /// <returns></returns>
        public Dictionary<object, string> GetTopicUsers()
        {
            EnumRoleSpecific.RoleSpecific role = EnumRoleSpecific.RoleSpecific.TopicUsers;

            string sqlWhere = "and r.RoleName = @RoleName ";
            SqlParameter parm = new SqlParameter("@RoleName", SqlDbType.NVarChar, 256);
            parm.Value = role.ToString();

            return dal.GetKeyValueByUsersInRoles(sqlWhere, parm);
        }

        public Dictionary<object, string> GetUsersByRole(string roleName)
        {
            string sqlWhere = "and r.RoleName = @RoleName ";
            SqlParameter parm = new SqlParameter("@RoleName", SqlDbType.NVarChar, 256);
            parm.Value = roleName;

            return dal.GetKeyValueByUsersInRoles(sqlWhere, parm);
        }

        public IList<UserBaseInfo> GetListByJoin(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            return dal.GetListByJoin(pageIndex, pageSize, out totalRecords, sqlWhere, cmdParms);
        }

        #endregion
    }
}
