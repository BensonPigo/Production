using System.Runtime.InteropServices;

namespace Sci.Production.Prg
{
    /// <summary>
    /// RF Card USB (CHP_1800)
    /// </summary>
    public class BundleRFCardUSB
    {
        private delegate bool DelUsbPortOpen();

        private delegate bool DelUsbPortClose();

        private delegate ushort DelExeCmd(byte[] cmd, byte[] tDat, ushort tLen, byte[] rDat, ushort[] rLen, ushort tmV);

        private delegate ushort DelImageExeCmd(byte[] tDat, int tLen, byte[] rDat, ushort[] rLen, ushort tmV);

        private readonly DllInvoke dll;
        private readonly DelUsbPortOpen funcUsbPortOpen;
        private readonly DelUsbPortClose funcUsbPortClose;
        private readonly DelExeCmd funcExeCmd;
        private readonly DelImageExeCmd funcImageExeCmd;

        /// <summary>
        /// Initializes a new instance of the <see cref="BundleRFCardUSB"/> class.
        /// </summary>
        public BundleRFCardUSB()
        {
            // 設定dll黨名稱
            this.dll = new DllInvoke(".\\KytDll.dll");

            // 設定dll檔內的function名稱 ExecProtocol
            this.funcUsbPortOpen = (DelUsbPortOpen)this.dll.Invoke("UsbPortOpen", typeof(DelUsbPortOpen));
            this.funcUsbPortClose = (DelUsbPortClose)this.dll.Invoke("UsbPortClose", typeof(DelUsbPortClose));
            this.funcExeCmd = (DelExeCmd)this.dll.Invoke("ExeCmd", typeof(DelExeCmd));
            this.funcImageExeCmd = (DelImageExeCmd)this.dll.Invoke("ImageExeCmd", typeof(DelImageExeCmd));
        }

        /// <summary>
        /// Open the USB port to communicate with the terminal.
        /// </summary>
        /// <returns>Normal: 1, Error : 0</returns>
        public bool UsbPortOpen()
        {
            if (this.funcUsbPortOpen != null)
            {
                // 真正執行dll的function的地方
                return this.funcUsbPortOpen();
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Close the USB port to communicate with the terminal.
        /// </summary>
        /// <returns>Normal: 1, Error : 0</returns>
        public bool UsbPortClose()
        {
            if (this.funcUsbPortClose != null)
            {
                // 真正執行dll的function的地方
                return this.funcUsbPortClose();
            }
            else
            {
                return false;
            }
        }

        // [DllImport("KytDll.dll")]
        //       public static extern UInt32 SendCmd(byte[] IDCode, byte[] TData, uint Length);

        // [DllImport("KytDll.dll")]
        //        public static extern UInt32 GetResp(uint TmV);

        // [DllImport("KytDll.dll")]
        //        public static extern bool UsbWrite(byte[] Dat, uint Len);

        // [DllImport("KytDll.dll")]
        //        public static extern int UsbRead(byte[] Dat, uint Len);

        // [DllImport("KytDll.dll")]
        //        public static extern byte ReadUsbOneByte();

        /// <summary>
        /// Transmit the command at the terminal.
        /// </summary>
        /// <param name="cmd">The Pointer of the buffer that 3 Byte instruction(Cm0, Cm1 and Cm2)is filled. (EBCDIC)</param>
        /// <param name="tDat">The Pointer of the buffer that Data(Data field in the Command structure) of the command is filled.</param>
        /// <param name="tLen">The length of Data.</param>
        /// <param name="rDat">The Pointer of the buffer is data to receive from the Terminal.</param>
        /// <param name="rLen">The length of Data</param>
        /// <param name="tmV">
        /// The TmV is the time to wait the answer from the Terminal. it can be selected by you.
        ///  - Time recommend with commands
        ///    - All the move commands : 15sec (value: 15000).
        ///    - All the Setting commands: 2sec (value: 2000).
        ///    - TPH head clean command "P32": 15sec (value: 1500).
        ///    - Print Start command "P41": 5sec (value: 5000).
        ///    - All the RF commands : 2sec (value: 2000).
        ///    - All the MS/IC commands: 5sec (value: 5000).
        ///    * As these value is maximum time, the actual answer is more fast
        /// </param>
        /// <returns>2byte Error(0x0000 is good code). refer the Spec</returns>
        public ushort ExeCmd(byte[] cmd, byte[] tDat, ushort tLen, byte[] rDat, ushort[] rLen, ushort tmV)
        {
            if (this.funcUsbPortClose != null)
            {
                // 真正執行dll的function的地方
                return this.funcExeCmd(cmd, tDat, tLen, rDat, rLen, tmV);
            }
            else
            {
                return 0x99;
            }
        }

        /// <summary>
        /// This command is for Image print command only(P48,P49 commands).
        /// Transmit the command at the terminal.
        /// </summary>
        /// <param name="tDat">The Pointer of the buffer that Data(3byte Command + Data field in the Command structure) of the command is filled.</param>
        /// <param name="tLen">The length of Data.</param>
        /// <param name="rDat">The Pointer of the buffer is data to receive from the Terminal.</param>
        /// <param name="rLen">The length of Data</param>
        /// <param name="tmV">
        /// The TmV is the time to wait the answer from the Terminal. it can be selected by you.
        ///  - Time recommend with commands.
        ///    - All the move commands : 15sec (value: 15000).
        ///    - All the Setting commands: 2sec (value: 2000)..
        ///    - TPH head clean command "P32": 15sec (value: 1500).
        ///    - Print Start command "P41": 5sec (value: 5000).
        ///    - All the RF commands : 2sec (value: 2000).        ///    - All the MS/IC commands: 5sec (value: 5000).
        ///    * As these value is maximum time, the actual answer is more fast.
        /// </param>
        /// <returns>2byte Error(0x0000 is good code). refer the Spec.</returns>
        public ushort ImageExeCmd(byte[] tDat, int tLen, byte[] rDat, ushort[] rLen, ushort tmV)
        {
            if (this.funcImageExeCmd != null)
            {
                // 真正執行dll的function的地方
                return this.funcImageExeCmd(tDat, tLen, rDat, rLen, tmV);
            }
            else
            {
                return 0x99;
            }
        }
    }
}
