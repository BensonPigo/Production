using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Sci.Production.Packing
{
    class P09_IDX_CTRL
    {       
        [DllImport("I:\\MIS\\ERP 2014\\PMS\\Scan & Pack auto conveyer\\Dll\\IDX_CTRL.dll", EntryPoint = "IdxCallVB")]
        static extern string IdxCallVB(int a, string b, Int32 c);
        public string test()
        {
            return IdxCallVB(1, "8:?", 4);
        }
    }
}
