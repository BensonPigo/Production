using System;
using System.Text;
using System.Runtime.InteropServices;

namespace Sci.Production.Subcon
{
    public class DllInvoke
    {
        [DllImport("Kernel32.dll")]
        private extern static IntPtr LoadLibrary(string path);

        [DllImport("Kernel32.dll")]
        private extern static IntPtr GetProcAddress(IntPtr lib, string funcName);

        [DllImport("Kernel32.dll")]
        private extern static bool FreeLibrary(IntPtr lib);

        private IntPtr hLib;

        public DllInvoke(string DLLPath)
        {
            this.hLib = LoadLibrary(DLLPath);
        }

        ~DllInvoke()
        {
            FreeLibrary(this.hLib);
        }

        public Delegate Invoke(string APIName, Type t)
        {
            IntPtr api = GetProcAddress(this.hLib, APIName);
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
            // 設定dll黨名稱
            this.dll = new DllInvoke(".\\Bhtprtd.dll");

            // 設定dll檔內的function名稱 ExecProtocol
            this.func = (bhtCallVB_func)this.dll.Invoke("ExecProtocol", typeof(bhtCallVB_func));
        }

        public int ExecProtocol(IntPtr nhWnd, string pcParam, StringBuilder FileNameBuf, int ProtocolType)
        {
            if (this.func != null)
            {
                // 真正執行dll的function的地方
                return this.func(nhWnd, pcParam, FileNameBuf, ProtocolType);
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

        public string csharpExecProtocol(IntPtr nhWnd, string Options, int RcvMode, int ProtocolType)
        {
            string pcParam;
            StringBuilder FileNameBuf = new StringBuilder();

            // Set option string
            if (RcvMode == 2)
            {
                pcParam = Options + " +R";
            }
            else
            {
                pcParam = Options + " -R";
            }

            // 呼叫自定義方法，名稱命名與dll的function相同
            int Ret = this.ExecProtocol(nhWnd, pcParam, FileNameBuf, ProtocolType);

            // 設定回傳訊息
            string TransferFile = string.Empty;
            if (Ret == 0)
            {
                TransferFile = " 0 : Receive Success : " + FileNameBuf.ToString().Trim();
            }
            else
            {
                TransferFile = Ret.ToString() + " : Receive Error : " + FileNameBuf.ToString().Trim() + Environment.NewLine +
                               this.ErrorMapping(Ret) + Environment.NewLine +
                               @"Set up scanner steps:
1.create C:\temp\ floder.
2.connect scanner to computer.
3.set up connect port is 8.";
            }

            return TransferFile;
        }
    }
}
