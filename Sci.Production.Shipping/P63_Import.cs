using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Prg;
using System;
using System.Data;
using System.Linq;

namespace Sci.Production.Shipping
{
    /// <inheritdoc/>
    public partial class P63_Import : Sci.Win.Subs.Base
    {
        private string masterID;
        private DataTable dt_detail;
        private readonly string minETD = "20220101";

        /// <inheritdoc/>
        public P63_Import(string masterID, DataTable detail)
        {
            this.InitializeComponent();
            this.masterID = masterID;
            this.dt_detail = detail;
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.gridGMTBooking.DataSource = this.gridGMTbs;
            this.gridGMTBooking.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.gridGMTBooking)
               .CheckBox("selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
               .Text("InvNo", header: "GB#", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Date("ETD", header: "On Board Date", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Date("ETA", header: "ETA", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Numeric("TotalShipQty", header: "Q'ty (Pcs)", width: Widths.AnsiChars(15), iseditingreadonly: true)
               ;
        }

        private void LoadData()
        {
            if ((MyUtility.Check.Empty(this.dateETD.Value1) && MyUtility.Check.Empty(this.dateETD.Value2)) &&
                (MyUtility.Check.Empty(this.txGBFrom.Text) && MyUtility.Check.Empty(this.txtGBTo.Text))
                )
            {
                MyUtility.Msg.WarningBox("Please input <On Board Date>, <GB#>.");
                return;
            }

            string where = string.Empty;

            #region Where

            if (!MyUtility.Check.Empty(this.dateETD.Value1))
            {
                where += $@" and GB.ETD >= '{((DateTime)this.dateETD.Value1).ToString("yyyy/MM/dd")}'";
            }

            if (!MyUtility.Check.Empty(this.dateETD.Value2))
            {
                where += $@" and GB.ETD <= '{((DateTime)this.dateETD.Value2).ToString("yyyy/MM/dd")}'";
            }

            if (!MyUtility.Check.Empty(this.txGBFrom.Text))
            {
                where += $@" and GB.ID >= '{this.txGBFrom.Text}'";
            }

            if (!MyUtility.Check.Empty(this.txtGBTo.Text))
            {
                where += $@" and GB.ID <= '{this.txtGBTo.Text}'";
            }

            if (!MyUtility.Check.Empty(this.txtBrand.Text))
            {
                where += $@"and GB.BrandID = '{this.txtBrand.Text}'";
            }

            #endregion

            string sqlcmd = $@"
SELECT  0 as [selected], 
        [ID] = '{this.masterID}',
        [InvNo] = GB.ID,
        GB.ETD,
        GB.ETA,
        GB.TotalShipQty 
FROM GMTBooking GB with (nolock)
INNER JOIN PackingList P with (nolock) ON P.INVNo=GB.ID
WHERE GB.ETD IS NOT NULL
AND GB.[Status]='Confirmed'
AND NOT Exists (SELECT 1 FROM KHCMTInvoice_Detail kd with (nolock) where GB.ID = kd.InvNo)
AND GB.ETD >= '{this.minETD}'
{where}
ORDER BY GB.ETD
";

            DualResult result;
            if (result = DBProxy.Current.Select(null, sqlcmd, out DataTable dt))
            {
                this.gridGMTbs.DataSource = dt;
            }
            else
            {
                this.ShowErr(sqlcmd, result);
            }
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            this.gridGMTBooking.ValidateControl();
            DataTable dtGridBS = (DataTable)this.gridGMTbs.DataSource;
            if (MyUtility.Check.Empty(dtGridBS) || dtGridBS.Rows.Count == 0)
            {
                return;
            }

            DataRow[] drSelected = dtGridBS.Select("Selected = 1");
            if (drSelected.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select at least one.", "Warnning");
                return;
            }

            foreach (DataRow tmp in drSelected)
            {
                bool isExists = this.dt_detail.AsEnumerable()
                    .Where(row => row.RowState != DataRowState.Deleted &&
                    row["InvNo"].EqualString(tmp["InvNo"].ToString())).Any();

                if (!isExists)
                {
                    this.dt_detail.ImportRowAdded(tmp);
                }
            }

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            this.LoadData();
        }
    }
}
