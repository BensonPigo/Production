using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.PPIC
{
    /// <inheritdoc/>
    public partial class B10 : Win.Tems.QueryForm
    {
        private string sp;
        private string brandID;
        private DateTime? BuyerDelivery_s;
        private DateTime? BuyerDelivery_e;
        private DateTime? SCIDelivery_s;
        private DateTime? SCIDelivery_e;
        private bool IncludeCancel;
        private bool IncludeFinish;

        /// <inheritdoc/>
        public B10(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.EditMode = true;

            DataGridViewGeneratorTextColumnSettings col_CAB = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings col_FinalDest = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings col_Customer_PO = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings col_AFS_STOCK_CATEGORY = new DataGridViewGeneratorTextColumnSettings();

            col_CAB.MaxLength = 10;
            col_FinalDest.MaxLength = 50;
            col_Customer_PO.MaxLength = 50;
            col_AFS_STOCK_CATEGORY.MaxLength = 50;

            this.grid.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.grid)
              .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
              .Text("ID", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
              .Text("StyleID", header: "Style", width: Widths.AnsiChars(7), iseditingreadonly: true)
              .Text("BrandID", header: "Brand", width: Widths.AnsiChars(20), iseditingreadonly: true)
              .Date("BuyerDelivery", header: "Buyer Dlv.", width: Widths.AnsiChars(13), iseditingreadonly: true)
              .Date("SCIDelivery", header: "SCI Dlv.", width: Widths.AnsiChars(13), iseditingreadonly: true)
              .Text("CAB", header: "CAB Code", width: Widths.AnsiChars(13), iseditingreadonly: false, settings: col_CAB)
              .Text("FinalDest", header: "Final Destination", width: Widths.AnsiChars(20), iseditingreadonly: false, settings: col_FinalDest)
              .Text("Customer_PO", header: "Customer PO", width: Widths.AnsiChars(20), iseditingreadonly: false, settings: col_Customer_PO)
              .Text("AFS_STOCK_CATEGORY", header: "AFS STOCK CATEGORY", width: Widths.AnsiChars(20), iseditingreadonly: false, settings: col_AFS_STOCK_CATEGORY)
          ;
        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            if (!this.CheckInput())
            {
                return;
            }

            this.Query();
        }

        private bool CheckInput()
        {
            if (MyUtility.Check.Empty(this.txtSP.Text) && !this.dateBuyerDev.HasValue1 && !this.dateBuyerDev.HasValue2 && !this.dateSCIDev.HasValue1 && !this.dateSCIDev.HasValue2)
            {
                MyUtility.Msg.WarningBox("<SP#>, <Buyer Delivery> and <SCI Delivery> cannot all be empty.");
                return false;
            }

            this.sp = this.txtSP.Text;
            this.brandID = this.txtBrand.Text;
            this.BuyerDelivery_s = this.dateBuyerDev.Value1;
            this.BuyerDelivery_e = this.dateBuyerDev.Value2;
            this.SCIDelivery_s = this.dateSCIDev.Value1;
            this.SCIDelivery_e = this.dateSCIDev.Value2;
            this.sp = this.txtSP.Text;
            this.IncludeCancel = this.chkIncludeCancel.Checked;
            this.IncludeFinish = this.chkIncludeHistory.Checked;

            return true;
        }

        private void Query()
        {
            DataTable dt;
            List<SqlParameter> parameters = new List<SqlParameter>();

            string cmd = $@"
SELECT [Selected]=0
		,ID
		,StyleID
		,BrandID
		,SeasonID
		,BuyerDelivery
		,SCIDelivery
		,CAB
		,FinalDest
		,Customer_PO
		,AFS_STOCK_CATEGORY

FROM Orders 
WHERE 1=1
";

            #region WHERE
            if (!MyUtility.Check.Empty(this.sp))
            {
                cmd += $@"AND ID=@ID" + Environment.NewLine;
                parameters.Add(new SqlParameter("@ID", this.sp));
            }

            if (!MyUtility.Check.Empty(this.brandID))
            {
                cmd += $@"AND BrandID=@BrandID" + Environment.NewLine;
                parameters.Add(new SqlParameter("@BrandID", this.brandID));
            }

            if (this.BuyerDelivery_s.HasValue && this.BuyerDelivery_e.HasValue)
            {
                cmd += $@"AND BuyerDelivery BETWEEN @BuyerDelivery_s AND @BuyerDelivery_e" + Environment.NewLine;
                parameters.Add(new SqlParameter("@BuyerDelivery_s", this.BuyerDelivery_s.Value));
                parameters.Add(new SqlParameter("@BuyerDelivery_e", this.BuyerDelivery_e.Value.AddDays(1).AddSeconds(-1)));
            }

            if (this.SCIDelivery_s.HasValue && this.SCIDelivery_e.HasValue)
            {
                cmd += $@"AND SCIDelivery BETWEEN @SCIDelivery_s AND @SCIDelivery_e" + Environment.NewLine;
                parameters.Add(new SqlParameter("@SCIDelivery_s", this.SCIDelivery_s.Value));
                parameters.Add(new SqlParameter("@SCIDelivery_e", this.SCIDelivery_e.Value.AddDays(1).AddSeconds(-1)));
            }

            if (!this.IncludeCancel)
            {
                cmd += $@"AND Junk = 0" + Environment.NewLine;
            }

            if (!this.IncludeFinish)
            {
                cmd += $@"AND Finished = 0" + Environment.NewLine;
            }

            #endregion

            DualResult r = DBProxy.Current.Select(null, cmd, parameters, out dt);

            if (!r)
            {
                this.ShowErr(r);
                this.grid.DataSource = null;
                return;
            }

            this.grid.DataSource = dt;

            if (dt.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found!!");
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            DataTable dataTable;
            if (!MyUtility.Check.Empty(this.grid.DataSource))
            {
                dataTable = (DataTable)this.grid.DataSource;
            }
            else
            {
                return;
            }

            DataRow[] selecteds = dataTable.Select("Selected=1");

            if (selecteds.Length == 0)
            {
                MyUtility.Msg.InfoBox("Please select data first.");
                return;
            }

            bool result = PPIC_B10_BatchUpdate(selecteds);
            if (result)
            {
                this.Query();
            }
        }

        private void BtnUploadFromMercury_Click(object sender, EventArgs e)
        {
            B10_UploadFromMercury form = new B10_UploadFromMercury();
            form.ShowDialog();
        }

        /// <inheritdoc/>
        public static bool PPIC_B10_BatchUpdate(DataRow[] selecteds)
        {
            StringBuilder builder = new StringBuilder();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string tmpTable = string.Empty;

            int count = 1;
            foreach (DataRow item in selecteds)
            {
                string tmp = $"SELECT [ID]='{item["ID"]}',[CAB]='{item["CAB"]}',[FinalDest]='{item["FinalDest"]}',Customer_PO='{item["Customer_PO"]}',[AFS_STOCK_CATEGORY]='{item["AFS_STOCK_CATEGORY"]}'";

                tmpTable += tmp + Environment.NewLine;

                if (count == 1)
                {
                    tmpTable += "INTO #source" + Environment.NewLine;
                }

                if (selecteds.Length > count)
                {
                    tmpTable += "UNION" + Environment.NewLine;
                }

                count++;
            }

            builder.Append($@"
{tmpTable}

UPDATE o
SET  o.CAB = s.CAB
    ,o.FinalDest = s.FinalDest
    ,o.Customer_PO = s.Customer_PO
    ,o.AFS_STOCK_CATEGORY = s.AFS_STOCK_CATEGORY
FROM #source s
INNER JOIN Orders o ON s.ID = o.ID

DROP TABLE #source
");

            DualResult r = DBProxy.Current.Execute(null, builder.ToString());

            if (!r)
            {
                MyUtility.Msg.WarningBox(r.GetException().Message);
                return false;
            }
            else
            {
                MyUtility.Msg.InfoBox("Success!!");
                return true;
            }
        }
    }
}
