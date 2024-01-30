using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.PublicPrg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Quality
{
    /// <inheritdoc/>
    public partial class P01_BatchUpdateContinuity : Win.Tems.QueryForm
    {
        private string poid;
        private DataTable dt;
        private DataTable dtScale;
        private DataTable dtResult;

        /// <inheritdoc/>
        public P01_BatchUpdateContinuity(string poid)
        {
            this.InitializeComponent();
            this.EditMode = true;
            this.poid = poid;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.LoadBaseDatas();
            this.GridSetup();
        }

        private void LoadBaseDatas()
        {
            var sql = $@"select ID = '' union all select ID from Scale where junk = 0 order by ID";
            var result = DBProxy.Current.Select(null, sql, out this.dtScale);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            sql = $@"select Result = '' union all select 'Pass' union all select 'Fail'";
            result = DBProxy.Current.Select(null, sql, out this.dtResult);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.comboBoxScale.DataSource = this.dtScale;
            this.comboBoxScale.ValueMember = "ID";
            this.comboBoxScale.DisplayMember = "ID";

            this.comboBoxResult.DataSource = this.dtResult;
            this.comboBoxResult.ValueMember = "Result";
            this.comboBoxResult.DisplayMember = "Result";
        }

        private void GridSetup()
        {
            DataGridViewGeneratorCheckBoxColumnSettings col_Select = new DataGridViewGeneratorCheckBoxColumnSettings();
            col_Select.CellValidating += (s, e) =>
            {
                DataRow dr = this.grid.GetDataRow<DataRow>(e.RowIndex);
                bool currentValue = MyUtility.Convert.GetBool(dr["Selected"]);
                bool newValue = MyUtility.Convert.GetBool(e.FormattedValue);

                if (currentValue == newValue)
                {
                    return;
                }

                dr["Selected"] = e.FormattedValue;
                dr.EndEdit();
                if (MyUtility.Convert.GetBool(dr["Selected"]))
                {
                    dr["Inspdate"] = DateTime.Now;
                    dr["Inspector"] = Env.User.UserID;
                    dr["Name"] = Env.User.UserName;
                }
                else
                {
                    dr["Inspdate"] = DBNull.Value;
                    dr["Inspector"] = string.Empty;
                    dr["Name"] = string.Empty;
                }
            };

            this.grid.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.grid)
                .CheckBox("Selected", header: string.Empty, trueValue: 1, falseValue: 0, settings: col_Select)
                .Text("ExportID", header: "WK#", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("SEQ1", header: "SEQ1", width: Widths.AnsiChars(2), iseditingreadonly: true)
                .Text("SEQ2", header: "SEQ2", width: Widths.AnsiChars(2), iseditingreadonly: true)
                .Text("Roll", header: "Roll", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Numeric("Ticketyds", header: "Ticket Yds", width: Widths.AnsiChars(6), integer_places: 10, decimal_places: 2, iseditingreadonly: true)
                .ComboBox("Scale", header: "Scale", width: Widths.AnsiChars(5), settings: this.Col_Scale())
                .ComboBox("Result", header: "Result", width: Widths.AnsiChars(6), settings: this.Col_Result())
                .Date("Inspdate", header: "Insp.Date", width: Widths.AnsiChars(10))
                .Text("Inspector", header: "Inspector", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Name", header: "Name", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .EditText("Remark", header: "Remark", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("ContinuityEncodeDisplay", header: "Encode", width: Widths.AnsiChars(8), iseditingreadonly: true)
                ;
            this.grid.Columns["Scale"].DefaultCellStyle.BackColor = Color.MistyRose;
            this.grid.Columns["Result"].DefaultCellStyle.BackColor = Color.MistyRose;
            this.grid.Columns["Inspdate"].DefaultCellStyle.BackColor = Color.MistyRose;

            #region Grid 變色規則
            Color backDefaultColor = this.grid.DefaultCellStyle.BackColor;

            this.grid.RowsAdded += (s, e) =>
            {
                foreach (DataGridViewRow dr in this.grid.Rows)
                {
                    dr.DefaultCellStyle.BackColor = MyUtility.Convert.GetString(dr.Cells["ContinuityEncodeDisplay"].Value) == "Encode" ? Color.Gray : Color.White;
                    dr.ReadOnly = MyUtility.Convert.GetString(dr.Cells["ContinuityEncodeDisplay"].Value) == "Encode" ? true : false;
                }
            };
            #endregion
        }

        private DataGridViewGeneratorComboBoxColumnSettings Col_Scale()
        {
            return new DataGridViewGeneratorComboBoxColumnSettings
            {
                DataSource = this.dtScale.Copy(),
                ValueMember = "ID",
                DisplayMember = "ID",
            };
        }

        private DataGridViewGeneratorComboBoxColumnSettings Col_Result()
        {
            return new DataGridViewGeneratorComboBoxColumnSettings
            {
                DataSource = this.dtResult.Copy(),
                ValueMember = "Result",
                DisplayMember = "Result",
            };
        }

        private void TxtScanQRCode_Validating(object sender, CancelEventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtScanQRCode.Text))
            {
                return;
            }

            string sqlcmd = $@"
--DECLARE @MINDQRCode varchar(80)

SELECT
    Selected = CAST(0 AS BIT)
    ,r.ExportID
    ,rd.MINDQRCode
    ,rd.POID
    ,rd.Seq1
    ,rd.Seq2
    ,rd.Roll
    ,rd.Dyelot
    ,fs.TicketYds
    ,fs.Scale
    ,fs.Result
    ,fs.Inspdate
    ,fs.Inspector
    ,Name = (SELECT Name FROM Pass1 WHERE ID = fs.Inspector)
    ,fs.Remark
    ,f.ContinuityEncode
    ,ContinuityEncodeDisplay = IIF(f.ContinuityEncode = 1, 'Encode', 'Amend')
    ,FIRID = f.ID
FROM Receiving_Detail rd WITH(NOLOCK)
INNER JOIN Receiving r WITH(NOLOCK) ON r.Id = rd.Id
INNER JOIN FIR f with (nolock) on r.id = f.ReceivingID and rd.PoId = F.POID and rd.Seq1 = F.SEQ1 and rd.Seq2 = F.SEQ2
LEFT JOIN FIR_Continuity fs with (nolock) on f.id = fs.ID and rd.Roll = fs.Roll and rd.Dyelot = fs.Dyelot
WHERE rd.MINDQRCode = @MINDQRCode

UNION ALL
SELECT
    Selected = CAST(0 AS BIT)
    ,ExportID = ''
    ,rd.MINDQRCode
    ,rd.POID
    ,rd.Seq1
    ,rd.Seq2
    ,rd.Roll
    ,rd.Dyelot
    ,fs.TicketYds
    ,fs.Scale
    ,fs.Result
    ,fs.Inspdate
    ,fs.Inspector
    ,Name = (SELECT Name FROM Pass1 WHERE ID = fs.Inspector)
    ,fs.Remark
    ,f.ContinuityEncode
    ,ContinuityEncodeDisplay = IIF(f.ContinuityEncode = 1, 'Encode', 'Amend')
    ,FIRID = f.ID
FROM TransferIn_Detail rd WITH(NOLOCK)
INNER JOIN TransferIn r WITH(NOLOCK) ON r.Id = rd.Id
INNER JOIN FIR f with (nolock) on r.id = f.ReceivingID and rd.PoId = F.POID and rd.Seq1 = F.SEQ1 and rd.Seq2 = F.SEQ2
LEFT JOIN FIR_Continuity fs with (nolock) on f.id = fs.ID and rd.Roll = fs.Roll and rd.Dyelot = fs.Dyelot
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

            if (MyUtility.Convert.GetString(dt0.Rows[0]["POID"]) != this.poid)
            {
                MyUtility.Msg.WarningBox("SP# is different!");
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

        private void BtnSave_Click(object sender, EventArgs e)
        {
            this.grid.ValidateControl();
            if (this.dt == null || this.dt.Select("Selected = 1").Length == 0)
            {
                return;
            }

            string sqlcmd = $@"
UPDATE fs
SET
    Scale = t.Scale
    ,Result = t.Result
    ,Inspdate = t.Inspdate
    ,Inspector = t.Inspector
    ,EditDate = GetDate()
    ,EditName = t.Inspector
FROM FIR_Continuity fs
INNER JOIN #tmp t on t.FIRID = fs.ID AND t.Roll = fs.Roll AND t.Dyelot = fs.Dyelot
";
            using (TransactionScope transactionscope = new TransactionScope())
            {
                try
                {
                    DualResult result = MyUtility.Tool.ProcessWithDatatable(this.dt.Select("Selected = 1").CopyToDataTable(), string.Empty, sqlcmd, out DataTable odt);
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

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnInspected_Click(object sender, EventArgs e)
        {
            if (this.dt == null)
            {
                return;
            }

            this.grid.ValidateControl();
            foreach (DataRow dr in this.dt.Select("Selected = 1"))
            {
                dr["Scale"] = this.comboBoxScale.SelectedValue.ToString();
                dr["Result"] = this.comboBoxResult.SelectedValue.ToString();
            }
        }

        private void Grid_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (this.listControlBindingSource1.DataSource == null || e.ColumnIndex != 0)
            {
                return;
            }

            this.grid.ValidateControl();
            foreach (DataRow dr in this.dt.Rows)
            {
                if (!MyUtility.Convert.GetBool(dr["ContinuityEncode"]))
                {
                    if (MyUtility.Convert.GetBool(dr["Selected"]))
                    {
                        dr["Inspdate"] = DateTime.Now;
                        dr["Inspector"] = Env.User.UserID;
                        dr["Name"] = Env.User.UserName;
                    }
                    else
                    {
                        dr["Inspdate"] = DBNull.Value;
                        dr["Inspector"] = string.Empty;
                        dr["Name"] = string.Empty;
                    }
                }
            }
        }
    }
}
