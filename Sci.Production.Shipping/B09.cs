using System.Data;
using System.Windows.Forms;
using Ict.Win;
using Sci.Production.Prg;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// B09
    /// </summary>
    public partial class B09 : Win.Tems.Input6
    {
        /// <summary>
        /// B09
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public B09(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// B09
        /// </summary>
        public B09()
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override bool ClickEditBefore()
        {
            return base.ClickEditBefore();
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();

            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("Email", header: "Email", width: Widths.AnsiChars(50));
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["ID"]))
            {
                this.txtLocalSupp1.TextBox1.Focus();
                MyUtility.Msg.WarningBox("< Consignee > can not be empty!");
                return false;
            }

            foreach (DataRow row in this.DetailDatas)
            {
                if (!row["Email"].Empty() && !row["Email"].ToString().IsEmail())
                {
                    MyUtility.Msg.WarningBox("< Email > format error!");
                    return false;
                }
            }

            return base.ClickSaveBefore();
        }
    }
}
