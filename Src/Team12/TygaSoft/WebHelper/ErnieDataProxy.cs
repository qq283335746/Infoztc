using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Web;
using System.Web.Caching;
using System.Data.SqlClient;
using TygaSoft.Model;
using TygaSoft.BLL;
using TygaSoft.CacheDependencyFactory;

namespace TygaSoft.WebHelper
{
    public class ErnieDataProxy
    {
        private static readonly bool enableCaching = bool.Parse(ConfigurationManager.AppSettings["EnableCaching"]);
        //private static readonly int ernieTimeout = 0;

        public static IList<ErnieAllInfo> GetLatest()
        {
            Ernie bll = new Ernie();

            if (!enableCaching)
            {
                return bll.GetLatest();
            }

            string key = "ernie_latest";
            IList<ErnieAllInfo> data = (IList<ErnieAllInfo>)HttpRuntime.Cache[key];

            if (data == null)
            {
                double timeOut = 0;
                data = bll.GetLatest();
                if (data.Count > 0)
                {
                    var model = data[0];
                    timeOut = (int)(model.EndTime - model.StartTime).TotalMilliseconds;
                }

                AggregateCacheDependency cd = DependencyFacade.GetErnieDependency();
                HttpRuntime.Cache.Add(key, data, cd, DateTime.Now.AddMilliseconds(timeOut), Cache.NoSlidingExpiration, CacheItemPriority.High, null);
            }

            return data;
        }
    }
}
