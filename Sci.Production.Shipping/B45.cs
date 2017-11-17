using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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

        // NL Code
        private void TxtNLCode_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(
                @"select NLCode,HSCode,UnitID
from VNContract_Detail WITH (NOLOCK) 
where ID in (select ID from VNContract WITH (NOLOCK) WHERE StartDate = (select MAX(StartDate) as MaxDate from VNContract WITH (NOLOCK) where Status = 'Confirmed') )
order by NLCode",
                "5,11,8",
                this.Text,
                false,
                ",",
                headercaptions: "Customs Code, HSCode, Unit");

            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            IList<DataRow> selectedData = item.GetSelecteds();
            this.CurrentMaintain["NLCode"] = item.GetSelectedString();
            this.CurrentMaintain["HSCode"] = selectedData[0]["HSCode"];
            this.CurrentMaintain["UnitID"] = selectedData[0]["UnitID"];
        }

        // NL Code
        private void TxtNLCode_Validating(object sender, CancelEventArgs e)
        {
            if (this.EditMode && this.txtNLCode.OldValue != this.txtNLCode.Text)
            {
                if (MyUtility.Check.Empty(this.txtNLCode.Text))
                {
                    this.CurrentMaintain["NLCode"] = string.Empty;
                    this.CurrentMaintain["HSCode"] = string.Empty;
                    this.CurrentMaintain["UnitID"] = string.Empty;
                }
                else
                {
                    DataRow nLCodeDate;
                    if (MyUtility.Check.Seek(
                        string.Format(
                        @"select NLCode,HSCode,UnitID
from VNContract_Detail WITH (NOLOCK) 
where ID in (select ID from VNContract WITH (NOLOCK) WHERE StartDate = (select MAX(StartDate) as MaxDate from VNContract WITH (NOLOCK) where Status = 'Confirmed') )
and NLCode = '{0}'", this.txtNLCode.Text), out nLCodeDate))
                    {
                        this.CurrentMaintain["NLCode"] = this.txtNLCode.Text;
                        this.CurrentMaintain["HSCode"] = nLCodeDate["HSCode"];
                        this.CurrentMaintain["UnitID"] = nLCodeDate["UnitID"];
                    }
                    else
                    {
                        this.CurrentMaintain["NLCode"] = string.Empty;
                        this.CurrentMaintain["HSCode"] = string.Empty;
                        this.CurrentMaintain["UnitID"] = string.Empty;
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox("The Customs Code is not in the Contract!!");
                        return;
                    }
                }
            }
        }
    }
}
