using System;
using System.Runtime.InteropServices;

namespace Sci.Production.Packing
{
    /// <summary>
    /// Packing_DllInvoke
    /// </summary>
    public class DllInvoke
    {
        [DllImport("Kernel32.dll")]
        private static extern IntPtr LoadLibrary(string path);

        [DllImport("Kernel32.dll")]
        private static extern IntPtr GetProcAddress(IntPtr lib, string funcName);

        [DllImport("Kernel32.dll")]
        private static extern bool FreeLibrary(IntPtr lib);

        private IntPtr hLib;

        /// <summary>
        /// DllInvoke
        /// </summary>
        /// <param name="dLLPath">dLLPath</param>
        public DllInvoke(string dLLPath)
        {
            this.hLib = LoadLibrary(dLLPath);
        }

        /// <summary>
        /// DllInvoke
        /// </summary>
        ~DllInvoke()
        {
            FreeLibrary(this.hLib);
        }

        /// <summary>
        /// Invoke
        /// </summary>
        /// <param name="aPIName">APIName</param>
        /// <param name="t">t</param>
        /// <returns>return</returns>
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
}
