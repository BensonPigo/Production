using System.Windows.Forms;

namespace Sci.Production.IE
{
    /// <summary>
    /// IE_B06
    /// </summary>
    public partial class B06 : Win.Tems.Input1
    {
        /// <summary>
        /// B06
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public B06(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();

            this.displayCode.Text = this.grid.SelectedRows[0].Cells["MachineGroup"].Value.ToString();
        }
    }
}
