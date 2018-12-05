using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci;
using Sci.Data;

namespace Sci.Production.Subcon
{
    public partial class P36_ModifyAfterSent : Sci.Win.Subs.Base
    {
        DataRow dr;
        DataTable dtData;
        public P36_ModifyAfterSent(DataRow Data)
        {
            InitializeComponent();
            DualResult result;
            dr = Data;
            if (!(result = DBProxy.Current.Select(null, string.Format(@"select * from localdebit WITH (NOLOCK) where id = '{0}'", dr["id"]), out dtData)))
            {
                ShowErr(result);
                return;
            }
            mtbs.DataSource = dtData;

        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            // TaipeiDBC = true or 1 >> 台北轉入, 其餘為工廠建立

            if (MyUtility.Check.Empty(dtData.Rows[0]["TaipeiDBC"]))
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
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            ReCalculateTax();
            if (!MyUtility.Tool.CursorUpdateTable(dtData, "localdebit", null))
            {
                MyUtility.Msg.WarningBox("Save failed!");
            }
            else
            {
                this.Close();
            }
        }

        private void numExchange_Validating(object sender, CancelEventArgs e)
        {
            if (MyUtility.Check.Empty(numExchange.Value))
            {
                MyUtility.Msg.WarningBox("Exchange value cannot be 0!");
                e.Cancel = true;
                return;
            }

            dtData.Rows[0]["Exchange"] = numExchange.Value;
            numAmount.Value = Math.Round(MyUtility.Convert.GetDecimal(dtData.Rows[0]["TaipeiAMT"]) * MyUtility.Convert.GetDecimal(numExchange.Value), 2);
            dtData.Rows[0]["amount"] = numAmount.Value;
            ReCalculateTax();
        }

        private void ReCalculateTax()
        {
            #region 計算TAX
            decimal amount = MyUtility.Convert.GetDecimal(dtData.Rows[0]["amount"]);
            decimal TaxRate = MyUtility.Convert.GetDecimal(dtData.Rows[0]["taxrate"]);
            int Exact = MyUtility.Convert.GetInt(MyUtility.GetValue.Lookup(string.Format("Select exact from Currency WITH (NOLOCK) where id = '{0}'", dtData.Rows[0]["currencyId"]), null));
            dtData.Rows[0]["tax"] = Math.Round((amount * TaxRate) / 100, Exact);

            #endregion
        }

        private void numtaxrate_Validating(object sender, CancelEventArgs e)
        {
            dtData.Rows[0]["taxrate"] = numtaxrate.Text;
            ReCalculateTax();
        }

        private void numAmount_Validating(object sender, CancelEventArgs e)
        {
            dtData.Rows[0]["amount"] = numAmount.Text;
            ReCalculateTax();
        }
    }
}
