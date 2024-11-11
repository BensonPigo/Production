using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Sci.CfgSection;

namespace Sci.Production.Prg
{
    /// <summary>
    /// CentralizedClass
    /// </summary>
    public static class CentralizedClass
    {
        /// <summary>
        /// 所有PMSDB or TestingDB連線字串
        /// </summary>
        /// <param name="factoryID">篩選Factory可傳入,串接</param>
        /// <param name="excludeModules">排除的Module連線可傳入,串接</param>
        /// <param name="connectionName">要取得的connectionName</param>
        /// <returns>List<string></returns>
        public static List<string> AllFactoryConnectionString(string factoryID = "", string excludeModules = "", string connectionName = "Production")
        {
            List<string> listExcludeModules = new List<string>() { "testing_PMS" };
            if (!MyUtility.Check.Empty(excludeModules))
            {
                listExcludeModules.AddRange(excludeModules.Split(',').ToList());
            }

            var listServerFactory = ConfigurationManager.AppSettings["ServerMatchFactory"].Split(new char[] { ';' })
                .Select(s =>
                {
                    string[] serverFactoryInfo = s.Split(':');
                    return new
                    {
                        FactoryModule = serverFactoryInfo[0],
                        ListFactory = serverFactoryInfo.Length == 1 ? new List<string>() : s.Split(':')[1].Split(',').ToList(),
                    };
                })
                .Where(s => !listExcludeModules.Contains(s.FactoryModule));
            List<string> listFactory = factoryID.Split(',').Select(s => s.Trim()).ToList();
            List<string> connectionString = new List<string>();
            var cfgsection = (CfgSection)ConfigurationManager.GetSection("sci");

            if (cfgsection != null)
            {
                foreach (CfgSection.Module it in cfgsection.Modules)
                {
                    if (!listServerFactory.Any(s => s.FactoryModule == it.Name))
                    {
                        continue;
                    }

                    // 判斷傳入的Factory是否在server設定中
                    if (!MyUtility.Check.Empty(factoryID) &&
                        !listServerFactory.Any(s => s.FactoryModule == it.Name &&
                                                    s.ListFactory.Any(factory => listFactory.Contains(factory))))
                    {
                        continue;
                    }

                    foreach (ConnectionStringElement connectionStringItem in it.ConnectionStrings)
                    {
                        if (connectionStringItem.Name == connectionName)
                        {
                            connectionString.Add(connectionStringItem.ConnectionString);
                            break;
                        }
                    }
                }
            }

            return connectionString;
        }

        /// <summary>
        /// 取得config connect資訊
        /// </summary>
        /// <param name="module">module</param>
        /// <param name="connectionName">connectionName</param>
        /// <returns>string</returns>
        public static string GetConfigConnectionString(string module, string connectionName)
        {
            var cfgsection = (CfgSection)ConfigurationManager.GetSection("sci");

            if (cfgsection != null)
            {
                foreach (CfgSection.Module it in cfgsection.Modules)
                {
                    if (it.Name != module)
                    {
                        continue;
                    }

                    foreach (ConnectionStringElement connectionStringItem in it.ConnectionStrings)
                    {
                        if (connectionStringItem.Name == connectionName)
                        {
                            return connectionStringItem.ConnectionString;
                        }
                    }
                }
            }

            return string.Empty;
        }
    }
}
