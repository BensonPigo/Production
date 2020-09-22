using System;
using System.ComponentModel;
using System.Data;
using Ict;
using Sci.Data;

namespace Sci.Production.Subcon
{
    public partial class P36_ModifyAfterSent : Win.Subs.Base
    {
        private DataRow dr;
        private DataTable dtData;

        public P36_ModifyAfterSent(DataRow data)
        {
            this.InitializeComponent();
            DualResult result;
            this.dr = data;
            if (!(result = DBProxy.Current.Select(null, string.Format(@"select * from localdebit WITH (NOLOCK) where id = '{0}'", this.dr["id"]), out this.dtData)))
            {
                this.ShowErr(result);
                return;
            }

            this.mtbs.DataSource = this.dtData;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            // TaipeiDBC = true or 1 >> 台北轉入, 其餘為工廠建立
            if (MyUtility.Check.Empty(this.dtData.Rows[0]["TaipeiDBC"]))
            {
                this.numExchange.ReadOnly = true;
                this.numAmount.ReadOnly = false;
            }
            else
            {
                this.numExchange.ReadOnly = false;
                this.numAmount.ReadOnly = true;
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            this.ReCalculateTax();
            if (!MyUtility.Tool.CursorUpdateTable(this.dtData, "localdebit", null))
            {
                MyUtility.Msg.WarningBox("Save failed!");
            }
            else
            {
                this.Close();
            }
        }

        private void NumExchange_Validating(object sender, CancelEventArgs e)
        {
            if (MyUtility.Check.Empty(this.numExchange.Value))
            {
                MyUtility.Msg.WarningBox("Exchange value cannot be 0!");
                e.Cancel = true;
                return;
            }

            this.dtData.Rows[0]["Exchange"] = this.numExchange.Value;
            this.numAmount.Value = Math.Round(MyUtility.Convert.GetDecimal(this.dtData.Rows[0]["TaipeiAMT"]) * MyUtility.Convert.GetDecimal(this.numExchange.Value), 2);
            this.dtData.Rows[0]["amount"] = this.numAmount.Value;
            this.ReCalculateTax();
        }

        private void ReCalculateTax()
        {
            #region 計算TAX
            decimal amount = MyUtility.Convert.GetDecimal(this.dtData.Rows[0]["amount"]);
            decimal taxRate = MyUtility.Convert.GetDecimal(this.dtData.Rows[0]["taxrate"]);
            int exact = MyUtility.Convert.GetInt(MyUtility.GetValue.Lookup(string.Format("Select exact from Currency WITH (NOLOCK) where id = '{0}'", this.dtData.Rows[0]["currencyId"]), null));
            this.dtData.Rows[0]["tax"] = Math.Round((amount * taxRate) / 100, exact);

            #endregion
        }

        private void Numtaxrate_Validating(object sender, CancelEventArgs e)
        {
            this.dtData.Rows[0]["taxrate"] = this.numtaxrate.Text;
            this.ReCalculateTax();
        }

        private void NumAmount_Validating(object sender, CancelEventArgs e)
        {
            this.dtData.Rows[0]["amount"] = this.numAmount.Text;
            this.ReCalculateTax();
        }
    }
}
