namespace Sci.Production.Packing
{
    /// <summary>
    /// P09_IDX_CTRL
    /// </summary>
    public class P09_IDX_CTRL
    {
        private delegate int IdxCallVB_func(int command, string request, int requestSize);

        private DllInvoke dll;
        private IdxCallVB_func func;

        /// <summary>
        /// P09_IDX_CTRL
        /// </summary>
        public P09_IDX_CTRL()
        {
            this.dll = new DllInvoke(".\\IDX_CTRL.dll");
            this.func = (IdxCallVB_func)this.dll.Invoke("IdxCallVB", typeof(IdxCallVB_func));
        }

        /// <summary>
        /// IdxCall
        /// </summary>
        /// <param name="command">command</param>
        /// <param name="request">Request</param>
        /// <param name="requestSize">RequestSize</param>
        /// <returns>bool</returns>
        public bool IdxCall(int command, string request, int requestSize)
        {
            if (this.func != null)
            {
                this.func(command, request, requestSize);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
