using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TygaSoft.Model;

namespace TygaSoft.IDAL
{
    public partial interface IProvinceCity
    {
        /// <summary>
        /// 获取所有省份
        /// </summary>
        /// <returns></returns>
        Dictionary<object, string> GetProvince();

        /// <summary>
        /// 获取当前父节点下的所有子节点
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        Dictionary<object, string> GetChild(Guid parentId);

        /// <summary>
        /// 获取对应的数据
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        ProvinceCityInfo GetModel(string name);
    }
}
