using Sci.Production.Prg.Entity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sci.Production.Prg
{
    /// <summary>
    /// UtilityPMS
    /// </summary>
    public static class UtilityPMS
    {
        /// <summary>
        /// GetCallFrom
        /// </summary>
        /// <returns>CallFromInfo</returns>
        public static CallFromInfo GetCallFrom()
        {
            CallFromInfo callFromInfo = new CallFromInfo();
            StackTrace stackTrace = new StackTrace();
            var sciStackTrace = stackTrace.GetFrames().Where(s => (s.GetMethod().DeclaringType.FullName.Contains("Sci.Production") ||
                                                                       s.GetMethod().DeclaringType.FullName.Contains("ProductionWebAPI.Controllers")) &&
                                                                      !s.GetMethod().DeclaringType.FullName.Contains("Sci.Production.Program"));
            MethodBase methodBase = sciStackTrace.Any() ? sciStackTrace.Last().GetMethod() : stackTrace.GetFrame(7).GetMethod();

            callFromInfo.CallFrom = methodBase.DeclaringType.FullName;
            if (callFromInfo.CallFrom.Contains("+"))
            {
                callFromInfo.CallFrom = callFromInfo.CallFrom.Split('+')[0];
            }

            callFromInfo.MethodName = methodBase.Name;
            if (callFromInfo.MethodName.Contains("<") && callFromInfo.MethodName.Contains(">"))
            {
                callFromInfo.MethodName = callFromInfo.MethodName.Split('<')[1].Split('>')[0];
            }

            return callFromInfo;
        }
    }
}
