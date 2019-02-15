using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// B45
    /// </summary>
    public partial class B45 : Sci.Win.Tems.Input1
    {
        /// <summary>
        /// B45
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public B45(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["Type"] = 1;
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtNLCode.ReadOnly = true;
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["NLCode"]))
            {
                MyUtility.Msg.WarningBox("Customs Code can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["Qty"]))
            {
                MyUtility.Msg.WarningBox("Qty can't empty!!");
                return false;
            }

            return base.ClickSaveBefore();
        }

        private void TxtRefno_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sqlRefnoList = "select Refno,NLCode,HSCode,CustomsUnit from LocalItem with (nolock) where junk = 0 order by Refno";
            SelectItem selectItem = new SelectItem(sqlRefnoList, "20,10,10,8", null, headercaptions: "Ref No.");
            DialogResult dialogResult = selectItem.ShowDialog();

            if (dialogResult == DialogResult.OK)
            {
                IList<DataRow> popResult = selectItem.GetSelecteds();
                this.CurrentMaintain["Refno"] = popResult[0]["Refno"];
                this.CurrentMaintain["NLCode"] = popResult[0]["NLCode"];
                this.CurrentMaintain["HSCode"] = popResult[0]["HSCode"];
                this.CurrentMaintain["UnitID"] = popResult[0]["CustomsUnit"];
            }
        }

        private void TxtRefno_Validating(object sender, CancelEventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtRefno.Text))
            {
                return;
            }

            string sqlCheckRefno = "select NLCode,HSCode,CustomsUnit from LocalItem with (nolock) where Refno = @refno and junk = 0";
            DataRow drResult;
            List<SqlParameter> parCheckRefno = new List<SqlParameter>() { new SqlParameter("@refno", this.txtRefno.Text) };
            bool isRefnoExists = MyUtility.Check.Seek(sqlCheckRefno, parCheckRefno, out drResult);
            if (isRefnoExists)
            {
                this.CurrentMaintain["Refno"] = this.txtRefno.Text;
                this.CurrentMaintain["NLCode"] = drResult["NLCode"];
                this.CurrentMaintain["HSCode"] = drResult["HSCode"];
                this.CurrentMaintain["UnitID"] = drResult["CustomsUnit"];
            }
            else
            {
                this.CurrentMaintain["NLCode"] = string.Empty;
                this.CurrentMaintain["HSCode"] = string.Empty;
                this.CurrentMaintain["UnitID"] = string.Empty;
                MyUtility.Msg.WarningBox($"<Ref No.>{this.txtRefno.Text} not found");
                e.Cancel = true;
                return;
            }
        }
    }
}
