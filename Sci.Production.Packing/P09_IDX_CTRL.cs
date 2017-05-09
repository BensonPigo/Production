using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Runtime.ExceptionServices;
using System.Configuration;

namespace Sci.Production.Packing
{
    public class DllInvoke
    {
        [DllImport("Kernel32.dll")]
        private extern static IntPtr LoadLibrary(String path);

        [DllImport("Kernel32.dll")]
        private extern static IntPtr GetProcAddress(IntPtr lib, String funcName);

        [DllImport("Kernel32.dll")]
        private extern static bool FreeLibrary(IntPtr lib);

        private IntPtr hLib;
        public DllInvoke(String DLLPath)
        {
            hLib = LoadLibrary(DLLPath);
        }

        ~DllInvoke()
        {
            FreeLibrary(hLib);
        }

        public Delegate Invoke(String APIName, Type t)
        {
            IntPtr api = GetProcAddress(hLib, APIName);
            return (Delegate)Marshal.GetDelegateForFunctionPointer(api, t);
        }
    }

    class P09_IDX_CTRL
    {
        private delegate int IdxCallVB_func(int command, string Request, int RequestSize);
        DllInvoke dll;
        IdxCallVB_func func;

        public P09_IDX_CTRL()
        {
            dll = new DllInvoke(".\\IDX_CTRL.dll");
            func = (IdxCallVB_func)dll.Invoke("IdxCallVB", typeof(IdxCallVB_func));
        }
        public void IdxCall(int command, string Request, int RequestSize)
        {
            func(command, Request, RequestSize);
        }
    }
}
