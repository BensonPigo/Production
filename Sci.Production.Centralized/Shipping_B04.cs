using Ict;
using Sci.Data;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Centralized
{
    /// <summary>
    /// Shipping_B04
    /// </summary>
    public partial class Shipping_B04 : Win.Tems.Input1
    {
        /// <summary>
        /// Shipping_B04
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public Shipping_B04(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();

            this.comboExpenseReason.Type = "Pms_SubType";

            MyUtility.Tool.SetupCombox(this.comboSharebase, 2, 1, "C,CBM,G,G.W.");

            // 禁止鍵盤輸入
            this.txtShipModeID.KeyDown += (s, args) => args.Handled = true;
            this.txtShipModeID.KeyPress += (s, args) => args.Handled = true;
            this.txtShipModeID.KeyUp += (s, args) => args.Handled = true;
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["AccountID"]))
            {
                MyUtility.Msg.WarningBox("Please insert Account No.!!");
                return false;
            }

            string sqlCheckPKDup = $@"
select 1 from ShareRule with (nolock) 
where   AccountID = '{this.CurrentMaintain["AccountID"]}' and
        ExpenseReason  = '{this.CurrentMaintain["ExpenseReason"]}' and
        ShareBase = '{this.CurrentMaintain["ShareBase"]}' ";

            if (MyUtility.Check.Seek(sqlCheckPKDup, "ProductionTPE") && this.CurrentMaintain.RowState == DataRowState.Added)
            {
                MyUtility.Msg.WarningBox("This [Account No.] + [Expense Reason] + [Share base] already exists!!");
                return false;
            }

            return base.ClickSaveBefore();
        }

        private void TxtShipModeID_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            DataTable dtShipModeID;

            DualResult result = DBProxy.Current.Select("Production", "Select ID, Description, LoadingType from ShipMode with (nolock) where junk = 0", out dtShipModeID);

            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            SelectItem2 selectItem = new SelectItem2(dtShipModeID, "ID,Description,LoadingType", "Code,Description,Loading Type", null, this.CurrentMaintain["ShipModeID"].ToString());

            DialogResult dialogResult = selectItem.ShowDialog();

            if (dialogResult != DialogResult.OK)
            {
                return;
            }

            IList<DataRow> selectResult = selectItem.GetSelecteds();

            if (selectResult.Count == 0)
            {
                this.CurrentMaintain["ShipModeID"] = string.Empty;
                return;
            }

            this.CurrentMaintain["ShipModeID"] = selectResult.Select(s => s["ID"].ToString()).JoinToString(",");
        }
    }
}
