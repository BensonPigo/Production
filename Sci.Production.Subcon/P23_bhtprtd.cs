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

    public class P23_bhtprtd
    {
        private delegate int bhtCallVB_func(IntPtr nhWnd, string pcParam, StringBuilder FileNameBuf, int ProtocolType);
        DllInvoke dll;
        bhtCallVB_func func;

        public P23_bhtprtd()
        {
            //設定dll黨名稱
            dll = new DllInvoke(".\\Bhtprtd.dll");
            //設定dll檔內的function名稱 ExecProtocol
            func = (bhtCallVB_func)dll.Invoke("ExecProtocol", typeof(bhtCallVB_func)); ;
        }

        public int ExecProtocol(IntPtr nhWnd, string pcParam, StringBuilder FileNameBuf, int ProtocolType)
        {
            if (func != null)
            {
                //真正執行dll的function的地方
                return func(nhWnd, pcParam, FileNameBuf, ProtocolType);
            }
            else
            {
                return 0;
            }
        }
        //
        public string csharpExecProtocol(IntPtr nhWnd, string Options, int RcvMode, int ProtocolType)
        {
            string pcParam;
            StringBuilder FileNameBuf = new StringBuilder();

            //Set option string
            if (RcvMode == 2)
            {
                pcParam = Options + " +R";
            }
            else
            {
                pcParam = Options + " -R";
            }
            //呼叫自定義方法，名稱命名與dll的function相同
            int Ret = ExecProtocol(nhWnd, pcParam, FileNameBuf, ProtocolType);
            //設定回傳訊息
            string TransferFile = "";
            if (Ret == 0)
            {
                TransferFile = " 0 : Receive Success : " + FileNameBuf.ToString().Trim();
            }
            else
            {
                TransferFile = Ret.ToString() + " : Receive Error : " + FileNameBuf.ToString().Trim();
            }

            return TransferFile;
        }

    }
}
