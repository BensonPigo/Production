using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Runtime.ExceptionServices;

namespace Sci.Production.Packing
{
    class P09_IDX_CTRL
    {
        [DllImport("C:\\windows\\system32\\IDX_CTRL.dll", EntryPoint = "IdxCallVB")]
        static extern void IdxCallVB(int a, string b, Int32 c);

        [HandleProcessCorruptedStateExceptions]
        public void IdxCall(int Command, string Request, Int32 RequestSize)
        {
            try
            {
                IdxCallVB(Command, Request, RequestSize);
            }
            catch (AccessViolationException e)
            {
                MyUtility.Msg.InfoBox(e.Message);
            }
        }
    }
}
