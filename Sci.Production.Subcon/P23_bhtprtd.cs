using System;
using System.Text;
using System.Runtime.InteropServices;

namespace Sci.Production.Subcon
{
    public class DllInvoke
    {
        [DllImport("Kernel32.dll")]
        private static extern IntPtr LoadLibrary(string path);

        [DllImport("Kernel32.dll")]
        private static extern IntPtr GetProcAddress(IntPtr lib, string funcName);

        [DllImport("Kernel32.dll")]
        private static extern bool FreeLibrary(IntPtr lib);

        private IntPtr hLib;

        public DllInvoke(string dLLPath)
        {
            this.hLib = LoadLibrary(dLLPath);
        }

        ~DllInvoke()
        {
            FreeLibrary(this.hLib);
        }

        public Delegate Invoke(string aPIName, Type t)
        {
            IntPtr api = GetProcAddress(this.hLib, aPIName);
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
        private delegate int BhtCallVB_func(IntPtr nhWnd, string pcParam, StringBuilder fileNameBuf, int protocolType);

        private DllInvoke dll;
        private BhtCallVB_func func;

        public P23_bhtprtd()
        {
            // 設定dll黨名稱
            this.dll = new DllInvoke(".\\Bhtprtd.dll");

            // 設定dll檔內的function名稱 ExecProtocol
            this.func = (BhtCallVB_func)this.dll.Invoke("ExecProtocol", typeof(BhtCallVB_func));
        }

        public int ExecProtocol(IntPtr nhWnd, string pcParam, StringBuilder fileNameBuf, int protocolType)
        {
            if (this.func != null)
            {
                // 真正執行dll的function的地方
                return this.func(nhWnd, pcParam, fileNameBuf, protocolType);
            }
            else
            {
                return 0;
            }
        }

        private string ErrorMapping(int errorCode)
        {
            string error_msg = string.Empty;

            switch (errorCode)
            {
                case 1: error_msg = "Designated file not found."; break;
                case 2: error_msg = "File name entered in wrong format."; break;
                case 3: error_msg = "Number of records exceeding 32767."; break;
                case 4: error_msg = "Field length is out of range."; break;
                case 5: error_msg = "Number of fields is out of range."; break;
                case 6: error_msg = "Record length is out of range."; break;
                case 7: error_msg = "Parameter mismatch."; break;
                case 8: error_msg = "Field length not found."; break;
                case 9: error_msg = "Option mismatch."; break;
                case 30: error_msg = "Invalid protocol."; break;
                case 40: error_msg = "ID error."; break;
                case 51: error_msg = "Communication error. (Tx timeout)"; break;
                case 52: error_msg = "Communication error. (Rx timeout)"; break;
                case 53: error_msg = "Communication error. (Tx NAK counter expired)"; break;
                case 54: error_msg = "Communication error. (Rx NAK counter expired)"; break;
                case 55: error_msg = "Communication error. (Illegal EOT received)"; break;
                case 60: error_msg = "Now transmitting."; break;
                case 61: error_msg = "Now receiving."; break;
                case 70: error_msg = "Illegal heading text format."; break;
                case 71: error_msg = "Path not found."; break;
                case 72: error_msg = "Disk memory full."; break;
                case 73: error_msg = "Insufficient memory."; break;
                case 74: error_msg = "No timer available."; break;
                case 75: error_msg = "Designated Com Port cannot open."; break;
                case 76: error_msg = "Illegal record format."; break;
                case 77: error_msg = "Wrong file received."; break;
                case 90: error_msg = "Aborted by break key."; break;
                case 91: error_msg = "Aborted by the other party."; break;
                case 99: error_msg = "General failure."; break;
                default:
                    break;
            }

            return error_msg;
        }

        public string CsharpExecProtocol(IntPtr nhWnd, string options, int rcvMode, int protocolType)
        {
            string pcParam;
            StringBuilder fileNameBuf = new StringBuilder();

            // Set option string
            if (rcvMode == 2)
            {
                pcParam = options + " +R";
            }
            else
            {
                pcParam = options + " -R";
            }

            // 呼叫自定義方法，名稱命名與dll的function相同
            int ret = this.ExecProtocol(nhWnd, pcParam, fileNameBuf, protocolType);

            // 設定回傳訊息
            string transferFile = string.Empty;
            if (ret == 0)
            {
                transferFile = " 0 : Receive Success : " + fileNameBuf.ToString().Trim();
            }
            else
            {
                transferFile = ret.ToString() + " : Receive Error : " + fileNameBuf.ToString().Trim() + Environment.NewLine +
                               this.ErrorMapping(ret) + Environment.NewLine +
                               @"Set up scanner steps:
1.create C:\temp\ floder.
2.connect scanner to computer.
3.set up connect port is 8.";
            }

            return transferFile;
        }
    }
}
