using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using TygaSoft.IDAL;
using TygaSoft.Model;
using TygaSoft.DALFactory;

namespace TygaSoft.BLL
{
    public partial class PlayerPictureNew
    {
        /// <summary>
        /// 获取满足当前条件的数据列表
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public DataSet GetListOW(object playerId)
        {
            return dal.GetListOW(playerId);
        }
    }
}
