using Sci.Data;
using Sci.Win.Tools;
using System.Windows.Forms;

namespace Sci.Production.Centralized
{
    /// <inheritdoc/>
    public partial class QA_B02 : Win.Tems.Input1
    {
        /// <inheritdoc/>
        public QA_B02(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();

            if (!DBProxy.Current.DefaultModuleName.Contains("testing"))
            {
                this.ConnectionName = "ProductionTPE";
            }
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtCode.ReadOnly = true;
        }
    }
}
