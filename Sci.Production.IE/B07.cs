using System.Windows.Forms;

namespace Sci.Production.IE
{
    /// <summary>
    /// IE_B07
    /// </summary>
    public partial class B07 : Sci.Win.Tems.Input1
    {
        /// <summary>
        /// B07
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public B07(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.labelDescriptionEnglish.Text = "Description\r\n(English)";
            this.labelDescriptionChinese.Text = "Description\r\n(Chinese)";

            MyUtility.Tool.SetupCombox(this.comboType, 2, 1, "A,Accoessory,W,Workmanship");
        }
    }
}
