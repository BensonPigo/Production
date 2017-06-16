using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Runtime.ExceptionServices;
using System.Configuration;

namespace Sci.Production.Subcon
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
            try
            {
                return (Delegate)Marshal.GetDelegateForFunctionPointer(api, t);
            }
            catch (Exception e)
            {
                MyUtility.Msg.ErrorBox(e.Message);
                return null;
            }
        }
    }

    class P23_bhtprtd
    {
        private delegate string bhtCallVB_func(int nhWnd, string pcParam, string FileNameBuf, int ProtocolType);
        DllInvoke dll;
        bhtCallVB_func func;

        public P23_bhtprtd()
        {
            dll = new DllInvoke(".\\Bhtprtd.dll");
            func = (bhtCallVB_func)dll.Invoke("ExecProtocol", typeof(bhtCallVB_func)); ;
        }

        public string ExecProtocol(int nhWnd, string pcParam, string FileNameBuf, int ProtocolType)
        {
            if (func != null)
            {

                return func(nhWnd, pcParam, FileNameBuf, ProtocolType);
            }
            else
            {
                return "";
            }
        }

        public string csharpExecProtocol(int nhWnd, string Options, int RcvMode, int ProtocolType)
        {
            string pcParam;
            string FileNameBuf = "                                                                                                    ";
            int CntNull;

            if (RcvMode ==2)
            {
		        pcParam = Options + " +R";
            }
            else
            {
                pcParam = Options + " -R";
            }



            return "";
        }

    }
}
