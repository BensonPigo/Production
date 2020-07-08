using System.Windows.Forms;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// B01
    /// </summary>
    public partial class B01 : Sci.Win.Tems.Input1
    {
        /// <summary>
        /// B01
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public B01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            MyUtility.Tool.SetupCombox(this.comboType, 2, 1, "L,Lacking,R,Replacement");
        }
    }
}
