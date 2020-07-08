using System.Windows.Forms;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// B02
    /// </summary>
    public partial class B02 : Sci.Win.Tems.Input1
    {
        /// <summary>
        /// B02
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public B02(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            MyUtility.Tool.SetupCombox(this.comboType, 2, 1, "L,Lacking,R,Replacement");
        }
    }
}
