using Sci.Win.Tools;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
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

            if (MyUtility.Check.Empty(this.CurrentMaintain["VNContractID"]))
            {
                MyUtility.Msg.WarningBox("Contract No. cannot be empty.");
                this.txtContractNo.Focus();
                return false;
            }

            return base.ClickSaveBefore();
        }

        private void TxtRefno_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            this.PopCustomRefno("Refno");
        }

        private void TxtNLCode_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            this.PopCustomRefno("NLCode");
        }

        private void TxtRefno_Validating(object sender, CancelEventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtRefno.Text))
            {
                return;
            }

            if (!this.IsExistsCustomRefno())
            {
                MyUtility.Msg.WarningBox($"<Refno>{this.txtRefno.Text} not found");
                e.Cancel = true;
                return;
            }
        }

        private void TxtNLCode_Validating(object sender, CancelEventArgs e)
        {
            string sqlNLCode = this.txtNLCode.Text;
            if (MyUtility.Check.Empty(sqlNLCode))
            {
                return;
            }

            if (!this.IsExistsCustomRefno())
            {
                MyUtility.Msg.WarningBox($"<Customs Code>{sqlNLCode} not found");
                e.Cancel = true;
                return;
            }

            string sqlGetOtherInfo = "select top 1 HSCode,UnitID from VNContract_Detail with (nolock) where NLCode = @NLCode order by AddDate Desc ";
            List<SqlParameter> parGetOtherInfo = new List<SqlParameter>() { new SqlParameter("@NLCode", sqlNLCode) };
            DataRow drOtherInfo;
            if (MyUtility.Check.Seek(sqlGetOtherInfo, parGetOtherInfo, out drOtherInfo))
            {
                this.CurrentMaintain["NLCode"] = sqlNLCode;
                this.CurrentMaintain["HSCode"] = drOtherInfo["HSCode"];
                this.CurrentMaintain["UnitID"] = drOtherInfo["UnitID"];
            }
        }

        private bool IsExistsCustomRefno()
        {
            string sqlWhere = string.Empty;
            if (!MyUtility.Check.Empty(this.txtRefno.Text))
            {
                sqlWhere += " and Refno = @refno";
            }

            if (!MyUtility.Check.Empty(this.txtNLCode.Text))
            {
                sqlWhere += " and NLCode = @NLCode";
            }

            DataRow drResult;
            List<SqlParameter> parCheckRefno = new List<SqlParameter>() { new SqlParameter("@refno", this.txtRefno.Text) };
            parCheckRefno.Add(new SqlParameter("@NLCode", this.txtNLCode.Text));

            string sqlCheckRefno = $@"select [Type] = 'L',UnitID from LocalItem with (nolock) where junk = 0 {sqlWhere}
                                     union all
                                     select Type,[UnitID] = UsageUnit from fabric with (nolock) where junk = 0 {sqlWhere}";
            bool isRefnoExists = MyUtility.Check.Seek(sqlCheckRefno, parCheckRefno, out drResult);
            if (!isRefnoExists)
            {
                return false;
            }

            this.CurrentMaintain["FabricType"] = drResult["Type"];
            this.CurrentMaintain["StockUnit"] = drResult["UnitID"];

            return true;
        }

        private void PopCustomRefno(string popFrom)
        {
            string sqlWhere = string.Empty;
            if (!MyUtility.Check.Empty(this.txtRefno.Text))
            {
                sqlWhere += " and Refno = @refno";
            }

            if (!MyUtility.Check.Empty(this.txtNLCode.Text))
            {
                sqlWhere += " and NLCode = @NLCode";
            }

            List<SqlParameter> parCheckRefno = new List<SqlParameter>() { new SqlParameter("@refno", this.txtRefno.Text) };
            parCheckRefno.Add(new SqlParameter("@NLCode", this.txtNLCode.Text));

            string sqlRefnoList = $@" select {popFrom} from
                                        (select {popFrom} from LocalItem with (nolock) where junk = 0 {sqlWhere}
                                        union
                                        select distinct {popFrom} from Fabric with (nolock) where junk = 0 {sqlWhere}) a
                                        order by {popFrom}";
            SelectItem selectItem = new SelectItem(sqlRefnoList, parCheckRefno, "20,10,10,8", null, headercaptions: string.Empty);
            DialogResult dialogResult = selectItem.ShowDialog();

            if (dialogResult == DialogResult.OK)
            {
                IList<DataRow> popResult = selectItem.GetSelecteds();
                this.CurrentMaintain[popFrom] = popResult[0][popFrom];
            }
        }
    }
}
