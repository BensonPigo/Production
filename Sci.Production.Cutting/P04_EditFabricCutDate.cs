using Ict;
using Ict.Win;
using Ict.Win.UI;
using Sci.Data;
using Sci.Production.Class;
using Sci.Production.Prg;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;
using Widths = Ict.Win.Widths;

namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public partial class P04_EditFabricCutDate : Win.Tems.QueryForm
    {
        private DataTable detailDataTable;

        /// <inheritdoc/>
        public P04_EditFabricCutDate(DataTable dataTable)
        {
            this.InitializeComponent();
            this.detailDataTable = dataTable.Select("Issue_Qty = 0").TryCopyToDataTable(dataTable);
            DataColumn column_Sel = new DataColumn("Sel", typeof(int));
            column_Sel.DefaultValue = 0;
            this.detailDataTable.Columns.Add(column_Sel);
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.GridSetup();
        }

        private void GridSetup()
        {
            DataGridViewGeneratorDateColumnSettings estCutDate = new DataGridViewGeneratorDateColumnSettings();
            DataGridViewGeneratorTextColumnSettings col_reason = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings col_RequestRemark = new DataGridViewGeneratorTextColumnSettings();

            col_RequestRemark.CellValidating += (s, e) =>
            {
                var dr = this.grid1.GetDataRow(e.RowIndex);

                var query = ((DataTable)this.grid1.DataSource).AsEnumerable()
                    .Where(x =>
                    x.Field<string>("Refno") == MyUtility.Convert.GetString(dr["Refno"]) &&
                        x.Field<string>("ColorID") == MyUtility.Convert.GetString(dr["ColorID"]));
                query.ToList().ForEach(row =>
                {
                    row["RequestorRemark"] = e.FormattedValue;
                });
            };

            col_reason.EditingMouseDown += (s, e) =>
            {
                if (e.RowIndex == -1 || e.Button != MouseButtons.Right)
                {
                    return;
                }

                DataRow dr = this.grid1.GetDataRow(e.RowIndex);
                string sqlcmd = $@"select Description, ID from CutReason  where Type = 'RC' and Junk = 0";
                SelectItem sele = new SelectItem(sqlcmd, "20", null) { Width = 333 };
                DialogResult result = sele.ShowDialog();
                if (result == DialogResult.Cancel)
                {
                    return;
                }

                DataRow resultSelect = sele.GetSelecteds()[0];

                var query = ((DataTable)this.grid1.DataSource).AsEnumerable()
                    .Where(x =>
                    x.Field<string>("Refno") == MyUtility.Convert.GetString(dr["Refno"]) &&
                        x.Field<string>("ColorID") == MyUtility.Convert.GetString(dr["ColorID"]));

                query.ToList().ForEach(row =>
                {
                    row["ReasonID"] = resultSelect["ID"];
                    row["Reason"] = resultSelect["Description"];
                });

                this.grid1.Refresh();
            };

            estCutDate.CellValidating += (s, e) =>
            {
                if (e == null)
                {
                    return;
                }

                var dr = this.grid1.GetDataRow(e.RowIndex);

                var query = ((DataTable)this.grid1.DataSource).AsEnumerable()
                    .Where(x =>
                    x.Field<string>("Refno") == MyUtility.Convert.GetString(dr["Refno"]) &&
                        x.Field<string>("ColorID") == MyUtility.Convert.GetString(dr["ColorID"]));

                query.ToList().ForEach(row =>
                {
                    row["EstCutDate"] = e.FormattedValue;
                });
            };

            this.grid1.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.grid1)
                .CheckBox("Sel", header: string.Empty, width: Widths.AnsiChars(2), iseditable: true, trueValue: 1, falseValue: 0)
                .Text("Sewinglineid", header: "Line#", width: Widths.AnsiChars(4), iseditingreadonly: true)
                .Text("Cutref", header: "CutRef#", width: Widths.AnsiChars(4), iseditingreadonly: true)
                .Text("Cutno", header: "Cut#", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Text("Fabriccombo", header: "Fabric\r\nCombo", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Text("Fabriccode", header: "Fabric\r\nCode", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Text("FabricPanelCode", header: "Fab_Panel\r\nCode", iseditingreadonly: true)
                .Text("orderid", header: "SP#", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("WeaveTypeID", header: "Weave Type", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("SCIRefno", header: "SCIRefno", width: Widths.AnsiChars(25), iseditingreadonly: true)
                .Text("Refno", header: "Refno", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("Article", header: "Article", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Colorid", header: "Color", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Date("EstCutDate", header: "Est. Cutting Date", width: Widths.AnsiChars(10), settings: estCutDate)
                .Text("Reason", header: "Reason", width: Widths.AnsiChars(25), iseditingreadonly: true, settings: col_reason)
                .Text("SEQ1", header: "SEQ1", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Text("SEQ2", header: "SEQ2", width: Widths.AnsiChars(2), iseditingreadonly: true)
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("CutQty", header: "Total\r\nCutQty", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Text("Cons", header: "Cons", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("Remark", header: "Remark", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("RequestorRemark", header: "RequestorRemark", width: Widths.AnsiChars(20), settings: col_RequestRemark)
                .Text("EditName", header: "EditName", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Date("EditDate", header: "EditDate", width: Widths.AnsiChars(20), iseditingreadonly: true)
                ;
            this.grid1.DataSource = this.detailDataTable;
            this.grid1.CellClick += this.Detailgrid_CellClick;

            this.grid1.Columns["EstCutDate"].DefaultCellStyle.BackColor = Color.Pink;
            this.grid1.Columns["Reason"].DefaultCellStyle.BackColor = Color.Pink;
            this.grid1.Columns["RequestorRemark"].DefaultCellStyle.BackColor = Color.Pink;

            this.grid1.Columns[0].Frozen = true;

            // EstCutDate、Reason，ReadOnly的預設值為true
            for (int i = 0; i < this.grid1.Rows.Count; i++)
            {
                DataGridViewDateBoxCell ecd = (DataGridViewDateBoxCell)this.grid1.Rows[i].Cells["EstCutDate"];
                DataGridViewCell r = (DataGridViewCell)this.grid1.Rows[i].Cells["Reason"];
                DataGridViewCell rr = (DataGridViewCell)this.grid1.Rows[i].Cells["RequestorRemark"];

                ecd.ReadOnly = true;
                r.ReadOnly = true;
                rr.ReadOnly = true;
            }
        }

        private void Detailgrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || this.grid1 == null)
            {
                return;
            }

            DataRow dr = this.grid1.GetDataRow(e.RowIndex);
            var selValue = MyUtility.Convert.GetString(dr["Sel"]);
            var query = ((DataTable)this.grid1.DataSource).AsEnumerable()
                        .Where(x =>
                        x.Field<string>("Refno") == MyUtility.Convert.GetString(dr["Refno"]) &&
                        x.Field<string>("ColorID") == MyUtility.Convert.GetString(dr["ColorID"]));
            query.ToList().ForEach(row =>
            {
                row["Sel"] = selValue;
            });
            foreach (DataGridViewRow dataGridViewRow in this.grid1.Rows)
            {
                var sel = MyUtility.Convert.GetString(dataGridViewRow.Cells["Sel"].Value);
                DataGridViewCell ecd = dataGridViewRow.Cells["EstCutDate"];
                DataGridViewCell r = dataGridViewRow.Cells["Reason"];
                DataGridViewCell rr = dataGridViewRow.Cells["RequestorRemark"];

                ecd.ReadOnly = sel == "1" ? false : true;
                r.ReadOnly = sel == "1" ? false : true;
                rr.ReadOnly = sel == "1" ? false : true;
            }
        }

        private void BtnEdit_Save_Click(object sender, EventArgs e)
        {
            if (this.detailDataTable.Rows.Count == 0 || this.detailDataTable == null)
            {
                return;
            }

            DataTable dt = (DataTable)this.grid1.DataSource;

            // 先判斷是否有勾選，沒有就跳掉，有就往下走
            DataTable dtSelcted = dt.Select("Sel = '1'").TryCopyToDataTable(dt);
            if (dtSelcted.Rows.Count == 0)
            {
                return;
            }

            // 判斷勾選是否有EstCutDate、Reason有值
            DataTable dt_Empty = dtSelcted.Select("CONVERT(EstCutDate, System.String) = '' OR ISNULL(Reason, '') = ''").TryCopyToDataTable(dtSelcted);
            if (dt_Empty.Rows.Count > 0)
            {
                MyUtility.Msg.WarningBox("Checked item’s Reason and EstCutDate can not be empty.");
                return;
            }

            DataTable distinctTable = dtSelcted.AsEnumerable().GroupBy(row => new
            {
                ID = row.Field<string>("ID"),
                Refno = row.Field<string>("Refno"),
                EstCutDate = row.Field<DateTime>("EstCutDate"),
                Reason = row.Field<string>("Reason"),
                FabricIssued = row.Field<string>("FabricIssued"),
                RequestorRemark = row.Field<string>("RequestorRemark"),
            }).Select(group => group.First()).TryCopyToDataTable(dtSelcted);

            // 判斷勾選的是否 IssueQty > 0
            foreach (DataRow row in distinctTable.Rows)
            {
                var cutplanID = MyUtility.Convert.GetString(row["ID"]);
                var sciRefno = MyUtility.Convert.GetString(row["SCIRefno"]);
                var colorID = MyUtility.Convert.GetString(row["ColorID"]);
                string issueQty = MyUtility.GetValue.Lookup($@"
                SELECT val = isnull(SUM(iss.Qty),0)
                FROM Issue i
                INNER JOIN Issue_Summary iss ON i.id = iss.Id
                WHERE i.CutplanID = '{cutplanID}' AND iss.SCIRefno = '{sciRefno}' AND iss.Colorid = '{colorID}'
                ");

                if (MyUtility.Convert.GetDecimal(issueQty) > 0)
                {
                    MyUtility.Msg.WarningBox("Edit failed because fabric have been issued,Please check with Warehouse.");
                    return;
                }
            }

            string sqlCmd = string.Empty;

            foreach (DataRow dataRow in distinctTable.Rows)
            {
                var cutplanID = MyUtility.Convert.GetString(dataRow["ID"]);
                var refno = MyUtility.Convert.GetString(dataRow["Refno"]);
                var colorID = MyUtility.Convert.GetString(dataRow["ColorID"]);
                var isUpdate = MyUtility.GetValue.Lookup($@"Select 1 From CutPlan_IssueCutDate Where ID = '{cutplanID}' and Refno = '{refno}' and ColorID = '{colorID}'");
                string estCutDate = ((DateTime)dataRow["EstCutDate"]).ToString("yyyy/MM/dd");

                if (isUpdate == "1")
                {
                    sqlCmd += $@"
                    UPDATE CutPlan_IssueCutDate SET 
                    EstCutDate = '{estCutDate}'
                    ,Reason = '{MyUtility.Convert.GetString(dataRow["ReasonID"])}'
                    , RequestorRemark = '{MyUtility.Convert.GetString(dataRow["RequestorRemark"])}'
                    , EditDate = GETDATE()
                    WHERE ID = '{cutplanID}' AND Refno = '{refno}' AND Colorid = '{colorID}'
                    ";
                }
                else
                {
                    sqlCmd += $@"
                    INSERT INTO CutPlan_IssueCutDate(ID,Refno,Colorid,EstCutDate,Reason,FabricIssued,RequestorRemark,EditName,EditDate) 
                    VALUES 
                    (
                    '{cutplanID}'
                    ,'{refno}'
                    ,'{colorID}'
                    ,'{estCutDate}'
                    ,'{MyUtility.Convert.GetString(dataRow["ReasonID"])}'
                    , iif('{MyUtility.Convert.GetString(dataRow["FabricIssued"])}' = 'Y',1,0)
                    ,'{MyUtility.Convert.GetString(dataRow["RequestorRemark"])}'
                    ,'{Env.User.UserID}'
                    ,GETDATE()
                    )
                    ";
                }
            }

            DualResult upResult;
            using (TransactionScope transactionscope = new TransactionScope())
            {
                if (!(upResult = DBProxy.Current.Execute(null, sqlCmd)))
                {
                    transactionscope.Dispose();
                    this.ShowErr(upResult);
                    return;
                }

                transactionscope.Complete();
            }

            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
