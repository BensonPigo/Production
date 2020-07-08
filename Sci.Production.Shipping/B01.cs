using System.Data;
using System.Windows.Forms;
using Ict.Win;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// B01
    /// </summary>
    public partial class B01 : Win.Tems.Input6
    {
        /// <summary>
        /// B01
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public B01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override bool ClickEditBefore()
        {
            this.txtbrand.IsSupportEditMode = false;
            this.txtubconForwarder.TextBox1.IsSupportEditMode = false;
            this.txtShipMode.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            return base.ClickEditBefore();
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();

            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("WhseNo", header: "Warehouse#", width: Widths.AnsiChars(50))
                .EditText("Address", header: "Address", width: Widths.AnsiChars(30));
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["BrandID"]))
            {
                this.txtbrand.Focus();
                MyUtility.Msg.WarningBox("< Brand > can not be empty!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["ShipModeID"].ToString()))
            {
                this.txtShipMode.Focus();
                MyUtility.Msg.WarningBox("< Ship Mode > can not be empty!");
                return false;
            }

            if (string.IsNullOrWhiteSpace(this.CurrentMaintain["Forwarder"].ToString()))
            {
                this.txtubconForwarder.Focus();
                MyUtility.Msg.WarningBox("< Forwarder > can not be empty!");
                return false;
            }

            int recordCount = 0;
            foreach (DataRow dr in this.DetailDatas)
            {
                if (MyUtility.Check.Empty(dr["WhseNo"]))
                {
                    dr.Delete();
                    continue;
                }

                recordCount += 1;
            }

            if (recordCount == 0)
            {
                MyUtility.Msg.WarningBox("Details data can't empty!!");
                return false;
            }

            this.txtbrand.IsSupportEditMode = true;
            this.txtubconForwarder.TextBox1.IsSupportEditMode = true;
            this.txtShipMode.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override void ClickUndo()
        {
            this.txtbrand.IsSupportEditMode = true;
            this.txtubconForwarder.TextBox1.IsSupportEditMode = true;
            this.txtShipMode.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            base.ClickUndo();
        }
    }
}
