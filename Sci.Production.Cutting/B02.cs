using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ict;
using Sci.Production.Automation;
using Sci.Production.Prg;

namespace Sci.Production.Cutting
{
    public partial class B02 : Sci.Win.Tems.Input1
    {
        private string keyWord = Sci.Env.User.Keyword;

        public B02(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.DefaultFilter = string.Format("mDivisionid = '{0}'", this.keyWord);
            this.InitializeComponent();
        }

        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["mDivisionid"] = this.keyWord;
        }

        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtCellNo.ReadOnly = true;
        }

        protected override DualResult ClickSavePost()
        {
            if (this.CurrentMaintain.RowState == DataRowState.Added ||
                (this.CurrentMaintain.RowState == DataRowState.Modified && this.CurrentMaintain.CompareDataRowVersionValue("Junk")))
            {
                DataTable dtCuttingCell = this.CurrentMaintain.Table.AsEnumerable().Where(s => s["ID"] == this.CurrentMaintain["ID"]).CopyToDataTable();
                Task.Run(() => new Guozi_AGV().SentCutCellToAGV(dtCuttingCell));
            }

            return base.ClickSavePost();
        }
    }
}
