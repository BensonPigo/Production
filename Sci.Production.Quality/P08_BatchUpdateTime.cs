using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.PublicPrg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Transactions;

namespace Sci.Production.Quality
{
    /// <inheritdoc/>
    public partial class P08_BatchUpdateTime : Win.Tems.QueryForm
    {
        private int type;
        private DataTable dt;

        /// <inheritdoc/>
        public P08_BatchUpdateTime(int type)
        {
            this.InitializeComponent();
            this.EditMode = true;
            this.type = type;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.GridSetup();
        }

        private void GridSetup()
        {
            DataGridViewGeneratorCheckBoxColumnSettings col_Select = new DataGridViewGeneratorCheckBoxColumnSettings();

            this.Helper.Controls.Grid.Generator(this.grid1)
                 .CheckBox("Selected", header: string.Empty, trueValue: 1, falseValue: 0, settings: col_Select)
                 .Text("MINDQRCode", header: "MINDQRCode", width: Widths.AnsiChars(20), iseditingreadonly: true)
                 .DateTime("ScanTime", header: "Scan Time", width: Widths.AnsiChars(20), iseditingreadonly: true)
                 .Text("POID", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                 .Text("SEQ", header: "SEQ", width: Widths.AnsiChars(8), iseditingreadonly: true)
                 .Text("ReceivingID", header: "ReceivingID", width: Widths.AnsiChars(16), iseditingreadonly: true)
                 .Text("ExportID", header: "WK#", width: Widths.AnsiChars(20), iseditingreadonly: true)
                 ;
        }

        private void TxtScanQRCode_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtScanQRCode.Text))
            {
                return;
            }

            string sqlcmd = $@"
SELECT
    Selected = CAST(1 AS BIT)
    ,rd.MINDQRCode
    ,ScanTime = GETDATE()
    ,rd.POID
    ,rd.Seq1
    ,rd.Seq2
    ,SEQ = CONCAT(rd.Seq1, ' ', rd.Seq2)
    ,ReceivingID = rd.ID
    ,r.ExportId
    ,FIRID = f.ID
    ,rd.Roll
    ,rd.Dyelot
FROM Receiving_Detail rd WITH(NOLOCK)
INNER JOIN Receiving r WITH(NOLOCK) ON r.Id = rd.Id
INNER JOIN FIR f with (nolock) on r.id = f.ReceivingID and rd.PoId = F.POID and rd.Seq1 = F.SEQ1 and rd.Seq2 = F.SEQ2
INNER JOIN FIR_Shadebone fs with (nolock) on f.id = fs.ID and rd.Roll = fs.Roll and rd.Dyelot = fs.Dyelot
WHERE rd.MINDQRCode = @MINDQRCode

UNION ALL
SELECT
    Selected = CAST(0 AS BIT)
    ,rd.MINDQRCode
    ,ScanTime = GETDATE()
    ,rd.POID
    ,rd.Seq1
    ,rd.Seq2
    ,SEQ = CONCAT(rd.Seq1, ' ', rd.Seq2)
    ,ReceivingID = rd.ID
    ,ExportId = ''
    ,FIRID = f.ID
    ,rd.Roll
    ,rd.Dyelot
FROM TransferIn_Detail rd WITH(NOLOCK)
INNER JOIN TransferIn r WITH(NOLOCK) ON r.Id = rd.Id
INNER JOIN FIR f with (nolock) on r.id = f.ReceivingID and rd.PoId = F.POID and rd.Seq1 = F.SEQ1 and rd.Seq2 = F.SEQ2
INNER JOIN FIR_Shadebone fs with (nolock) on f.id = fs.ID and rd.Roll = fs.Roll and rd.Dyelot = fs.Dyelot
WHERE rd.MINDQRCode = @MINDQRCode
";
            List<SqlParameter> paras = new List<SqlParameter>();
            paras.Add(new SqlParameter("@MINDQRCode", this.txtScanQRCode.Text));

            DualResult result = DBProxy.Current.Select(null, sqlcmd, paras, out DataTable dt0);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            if (dt0.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found");
                return;
            }

            if (this.dt == null)
            {
                this.dt = dt0;
            }
            else
            {
                var sameMINDQRCode = this.dt.Select($"MINDQRCode = '{dt0.Rows[0]["MINDQRCode"]}'");
                if (sameMINDQRCode.Length > 0)
                {
                    sameMINDQRCode.Delete();
                    this.dt.AcceptChanges();
                }

                this.dt.Merge(dt0);
            }

            this.listControlBindingSource1.DataSource = this.dt;
            this.txtScanQRCode.Text = string.Empty;
            e.Cancel = true;
        }

        private void BtnRemove_Click(object sender, EventArgs e)
        {
            if (this.dt == null)
            {
                return;
            }

            this.dt.Select("Selected = 1").Delete();
            this.dt.AcceptChanges();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (this.dt == null || this.dt.Rows.Count == 0)
            {
                return;
            }

            string column = string.Empty;
            string updCutby = string.Empty;
            switch (this.type)
            {
                case 0:
                    column = "CutTime";
                    updCutby = $@"
, CutBy = case when fs.CutTime is null and ISNULL(fs.CutBy,'') = '' then '{Env.User.UserID}' else fs.CutBy end
, CutTimeEditName = case when fs.CutTime is not null and fs.CutBy != '' then '{Env.User.UserID}' else fs.CutTimeEditName end";
                    break;
                case 1:
                    column = "PasteTime";
                    break;
                case 2:
                    column = "PassQATime";
                    break;
            }

            string sqlcmd = $@"
UPDATE FIR_Shadebone
SET {column} = GETDATE()
{updCutby}
FROM FIR_Shadebone fs
INNER JOIN #tmp t ON t.FIRID = fs.ID AND t.Roll = fs.Roll AND t.Dyelot = fs.Dyelot
";
            using (TransactionScope transactionscope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    DualResult result = MyUtility.Tool.ProcessWithDatatable(this.dt, "FIRID,Roll,Dyelot", sqlcmd, out DataTable odt);
                    if (!result)
                    {
                        this.ShowErr(result);
                        return;
                    }

                    transactionscope.Complete();
                }
                catch (Exception ex)
                {
                    this.ShowErr(ex);
                    return;
                }
            }

            MyUtility.Msg.InfoBox("Successfully");
        }
    }
}
