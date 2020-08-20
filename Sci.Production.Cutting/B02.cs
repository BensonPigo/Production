using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ict;
using Sci.Production.Automation;
using Sci.Production.Prg;

namespace Sci.Production.Cutting
{
    public partial class B02 : Win.Tems.Input1
    {
        private string keyWord = Env.User.Keyword;

        /// <summary>
        /// Initializes a new instance of the <see cref="B02"/> class.
        /// </summary>
        /// <param name="menuitem"></param>
        public B02(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.DefaultFilter = string.Format("mDivisionid = '{0}'", this.keyWord);
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["mDivisionid"] = this.keyWord;
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtCellNo.ReadOnly = true;
        }

        /// <inheritdoc/>
        protected override DualResult ClickSavePost()
        {
            if (this.CurrentMaintain.RowState == DataRowState.Added ||
                (this.CurrentMaintain.RowState == DataRowState.Modified && this.CurrentMaintain.CompareDataRowVersionValue("Junk")))
            {
                DataTable dtCuttingCell = this.CurrentMaintain.Table.AsEnumerable().Where(s => s["ID"] == this.CurrentMaintain["ID"]).CopyToDataTable();
                Task.Run(() => new Guozi_AGV().SentCutCellToAGV(dtCuttingCell))
                    .ContinueWith(UtilityAutomation.AutomationExceptionHandler, TaskContinuationOptions.OnlyOnFaulted);
            }

            return base.ClickSavePost();
        }
    }
}
