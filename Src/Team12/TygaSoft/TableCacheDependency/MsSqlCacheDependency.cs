using System.Web.Caching;
using System.Configuration;
using TygaSoft.ICacheDependency;

namespace TygaSoft.TableCacheDependency
{
    public abstract class MsSqlCacheDependency : IMsSqlCacheDependency
    {
        protected char[] configurationSeparator = new char[] { ',' };

        protected AggregateCacheDependency dependency = new AggregateCacheDependency();

        protected MsSqlCacheDependency(string configKey)
        {
            //string dbName = ConfigurationManager.AppSettings["CacheDatabaseName"];
            string tableConfig = ConfigurationManager.AppSettings[configKey];
            string[] tables = tableConfig.Split(configurationSeparator);
            string dbName = tables[tables.Length - 1];

            foreach (string tableName in tables)
            {
                if (tableName != dbName)
                    dependency.Add(new SqlCacheDependency(dbName, tableName));
            }
        }

        public AggregateCacheDependency GetDependency()
        {
            return dependency;
        }
    }
}
