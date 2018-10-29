using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using TygaSoft.IDAL;
using TygaSoft.Model;
using TygaSoft.DALFactory;


namespace TygaSoft.BLL
{
    public class ScratchLotto
    {
        private static readonly IScratchLotto dal = DataAccess.CreateScratchLotto();

        /// <summary>
        /// 修改预定中奖次数
        /// </summary>
        /// <returns></returns>
        public int UpdateWinningTimes()
        {
            return dal.UpdateWinningTimes();
        }
    }
}
