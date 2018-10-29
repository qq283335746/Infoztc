using System.Configuration;
using System.Web.Caching;
using System.Collections.Generic;
using TygaSoft.ICacheDependency;

namespace TygaSoft.CacheDependencyFactory
{
    public static class DependencyFacade
    {
        private static readonly string path = ConfigurationManager.AppSettings["CacheDependencyAssembly"];

        public static AggregateCacheDependency GetErnieDependency()
        {
            if (!string.IsNullOrEmpty(path))
                return DependencyAccess.CreateErnieDependency().GetDependency();
            else
                return null;
        }
    }
}
