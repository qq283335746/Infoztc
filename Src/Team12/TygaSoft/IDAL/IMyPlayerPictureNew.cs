﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using TygaSoft.Model;

namespace TygaSoft.IDAL
{
    public partial interface IPlayerPictureNew
    {
        /// <summary>
        /// 获取满足当前条件的数据列表
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        DataSet GetListOW(object Id);
    }
}
