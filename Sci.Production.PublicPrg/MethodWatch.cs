using Ict;
using Sci.Data;
using Sci.Production.Prg.Entity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sci.Production.Prg
{
    /// <summary>
    /// MethodWatch
    /// </summary>
    public class MethodWatch : IDisposable
    {
        private static readonly Stopwatch watch = new Stopwatch();

        /// <summary>
        /// MethodWatch
        /// </summary>
        static MethodWatch()
        {
            watch.Start();
        }

        private TimeSpan start;
        private int startLogSecond = 0;
        private string methodName = string.Empty;

        /// <summary>
        /// MethodWatch
        /// </summary>
        public MethodWatch()
        {
            this.start = watch.Elapsed;
        }

        /// <summary>
        /// MethodWatch
        /// </summary>
        /// <param name="startLogSecond">執行超過幾秒才紀錄</param>
        /// <param name="methodName">自定義Method名稱</param>
        public MethodWatch(int startLogSecond, string methodName = "")
        {
            this.start = watch.Elapsed;
            this.startLogSecond = startLogSecond;
            this.methodName = methodName;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            TimeSpan elapsed = watch.Elapsed - this.start;

            if (elapsed.Seconds > this.startLogSecond)
            {
                CallFromInfo callFromInfo = UtilityPMS.GetCallFrom();
                string finalMethodName = MyUtility.Check.Empty(this.methodName) ? callFromInfo.MethodName : this.methodName;
                string desc = $"{elapsed.Seconds}- {callFromInfo.CallFrom}";
                string sqlSaveTranslog = $@"
insert into TransLog(FunctionName, Description, StartTime, EndTime)
        values('{finalMethodName}', '{desc}', DATEADD(SECOND, -{elapsed.Seconds}, getdate()), getdate())
";

                DualResult result = DBProxy.Current.Execute(null, sqlSaveTranslog);
                if (!result)
                {
                    throw result.GetException();
                }
            }
        }
    }
}
