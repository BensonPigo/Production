using Ict.Win.UI;
using Ict.Win;
using Sci.Production.Prg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Widths = Ict.Win.Widths;
using Ict;
using Sci.Data;
using System.Data.SqlTypes;
using System.Transactions;

namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public partial class P04_FabricDelete : Win.Tems.QueryForm
    {
        private DataTable detailDataTable;
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk = new Ict.Win.UI.DataGridViewCheckBoxColumn();

        /// <inheritdoc/>
        public P04_FabricDelete(DataTable dataTable)
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
            this.grid1.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.grid1)
                .CheckBox("Sel", header: string.Empty, width: Widths.AnsiChars(2), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
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
                .Date("EstCutDate", header: "Est. Cutting Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Reason", header: "Reason", width: Widths.AnsiChars(25), iseditingreadonly: true)
                .Text("SEQ1", header: "SEQ1", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Text("SEQ2", header: "SEQ2", width: Widths.AnsiChars(2), iseditingreadonly: true)
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("CutQty", header: "Total\r\nCutQty", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Text("Cons", header: "Cons", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("Remark", header: "Remark", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("RequestorRemark", header: "RequestorRemark", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("EditName", header: "EditName", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Date("EditDate", header: "EditDate", width: Widths.AnsiChars(20), iseditingreadonly: true)
                ;
            this.grid1.DataSource = this.detailDataTable;
            this.grid1.CellClick += this.Detailgrid_CellClick;
            this.grid1.Columns[0].Frozen = true;
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
            query.ToList().ForEach(row => row["Sel"] = selValue);
        }

        private void BtnDelete_Save_Click(object sender, EventArgs e)
        {
            if (this.detailDataTable.Rows.Count == 0 || this.detailDataTable == null)
            {
                return;
            }

            DataTable dt = (DataTable)this.grid1.DataSource;

            // 先判斷是否有勾選，沒有就跳掉，有就往下走
            DataTable dt_1 = dt.Select("Sel = '1'").TryCopyToDataTable(dt);
            if (dt_1.Rows.Count == 0)
            {
                return;
            }

            // 判斷勾選的是否 IssueQty > 0
            foreach (DataRow row in dt_1.Rows)
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
                    MyUtility.Msg.WarningBox("Delete failed because fabric have been issued,Please check with Warehouse.");
                    return;
                }
            }

            string sqlCmd = string.Empty;
            foreach (DataRow dataRow in dt_1.Rows)
            {
                sqlCmd += $@"
                INSERT INTO CutPlan_DetailDeletedHistory
                (
                    ID
                    ,Sewinglineid
                    ,CutRef
                    ,CutNo
                    ,OrderID
                    ,StyleID
                    ,Colorid
                    ,Cons
                    ,WorkOrderForPlanningUkey
                    ,Remark
                    ,POID
                    ,Adddate          
                ) VALUES
                (
                    '{MyUtility.Convert.GetString(dataRow["ID"])}'
                    ,'{MyUtility.Convert.GetString(dataRow["Sewinglineid"])}'
                    ,'{MyUtility.Convert.GetString(dataRow["CutRef"])}'
                    ,'{MyUtility.Convert.GetString(dataRow["CutNo"])}'
                    ,'{MyUtility.Convert.GetString(dataRow["OrderID"])}'
                    ,'{MyUtility.Convert.GetString(dataRow["StyleID"])}'
                    ,'{MyUtility.Convert.GetString(dataRow["Colorid"])}'
                    ,'{MyUtility.Convert.GetString(dataRow["Cons"])}'
                    ,'{MyUtility.Convert.GetString(dataRow["WorkOrderForPlanningUkey"])}'
                    ,'{MyUtility.Convert.GetString(dataRow["Remark"])}'
                    ,'{MyUtility.Convert.GetString(dataRow["POID"])}'
                    ,GETDATE()
                )
                UPDATE WorkOrderForPlanning SET CutplanID = '' WHERE Ukey = '{MyUtility.Convert.GetString(dataRow["WorkOrderForPlanningUkey"])}'    
                Delete Cutplan_Detail_Cons WHERE ID = '{MyUtility.Convert.GetString(dataRow["ID"])}' AND Seq1 = '{MyUtility.Convert.GetString(dataRow["Seq1"])}' AND Seq2 = '{MyUtility.Convert.GetString(dataRow["Seq2"])}'
                Delete Cutplan_Detail WHERE ID = '{MyUtility.Convert.GetString(dataRow["ID"])}' AND WorkOrderForPlanningUkey = '{MyUtility.Convert.GetString(dataRow["WorkOrderForPlanningUkey"])}'
                ";
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
