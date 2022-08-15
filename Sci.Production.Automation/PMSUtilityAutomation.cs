using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sci.Data;
using Sci.Production.Prg;
using Sci.Production.PublicPrg;
using static AutomationErrMsg;
using static PmsWebApiUtility20.WebApiTool;
using DualResult = Ict.DualResult;

namespace Sci.Production.Automation
{

    /// <summary>
    /// PMSUtilityAutomation
    /// </summary>
    public class PMSUtilityAutomation : BaseUtilityAutomation
    {
        /// <summary>
        /// UtilityAutomation
        /// </summary>
        public static readonly PMSUtilityAutomation UtilityAutomation = new PMSUtilityAutomation();

        /// <summary>
        /// IsSunrise_FinishingProcessesEnable
        /// </summary>
        public static bool IsSunrise_FinishingProcessesEnable => UtilityAutomation.IsModuleAutomationEnable(Sunrise_FinishingProcesses.sunriseSuppID, Sunrise_FinishingProcesses.moduleName);

        /// <inheritdoc/>
        public override bool IsAutomationEnable => Prgs.IsAutomation();

        /// <inheritdoc/>
        public override string ModuleType
        {
            get
            {
                if (PmsWebAPI.IsDummy)
                {
                    return "Dummy";
                }
                else
                {
                    return "Formal";
                }
            }
        }

        /// <inheritdoc/>
        public override bool OpenAll_AutomationCheckMsg => MyUtility.Convert.GetBool(ConfigurationManager.AppSettings["OpenAll_AutomationCheckMsg"]);

        /// <inheritdoc/>
        public override string UserID => Env.User.UserID;

        /// <inheritdoc/>
        public override DualResult OpenConnection(string connName, out SqlConnection sqlConnection)
        {
            return DBProxy._OpenConnection(connName, out sqlConnection);
        }

        /// <inheritdoc/>
        public override DualResult Select(string connname, string sqlCmd, IList<SqlParameter> sqlPars, out DataTable dtResult)
        {
            return DBProxy.Current.Select(connname, sqlCmd, sqlPars, out dtResult);
        }
    }
}
