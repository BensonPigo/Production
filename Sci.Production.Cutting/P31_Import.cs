using Ict;
using Ict.Win;
using Sci.Andy.ExtensionMethods;
using Sci.Data;
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

namespace Sci.Production.Cutting
{
    public partial class P31_Import : Win.Tems.QueryForm
    {
        private DataRow CurrentMaintain;
        private DataTable dtDetail;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_CutplanID;
        public DataTable dtDetail_New;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="currentMaintain">currentMaintain</param>
        /// <param name="dtDetail">dtDetail</param>
        public P31_Import(DataRow currentMaintain, DataTable dtDetail)
        {
            this.InitializeComponent();
            this.CurrentMaintain = currentMaintain;
            this.dtDetail = dtDetail;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.GridSetup();
        }

        private void GridSetup()
        {
            DataGridViewGeneratorTextColumnSettings cutRef = new DataGridViewGeneratorTextColumnSettings();
            cutRef.EditingControlShowing += (s, e) =>
            {
                if (e.Control is Ict.Win.UI.TextBoxBase textBoxBase)
                {
                    DataRow dr = this.grid.GetDataRow(e.RowIndex);
                    textBoxBase.ReadOnly = !MyUtility.Check.Empty(dr["Cutref"]);
                }
            };
            cutRef.CellValidating += (s, e) =>
            {
                if (this.EditMode == false || MyUtility.Check.Empty(e.FormattedValue))
                {
                    return;
                }

                DataRow dr = this.grid.GetDataRow(e.RowIndex);
                string oldvalue = MyUtility.Convert.GetString(dr["CutRef"]);
                string newvalue = MyUtility.Convert.GetString(e.FormattedValue);
                if (oldvalue == newvalue)
                {
                    return;
                }

                if (((DataTable)this.listControlBindingSource.DataSource).AsEnumerable().Where(w => MyUtility.Convert.GetString(w["CutRef"]) == newvalue).Any())
                {
                    MyUtility.Msg.WarningBox($"CutRef:{newvalue} already exists!");
                    dr["CutRef"] = string.Empty;
                    return;
                }

                if (this.CheckCutrefToDay(newvalue))
                {
                    MyUtility.Msg.WarningBox($"Cannot add in curtref# {newvalue}, the cutref# has exists today.");
                    dr["CutRef"] = string.Empty;
                    return;
                }

                if (MyUtility.Check.Empty(dr["CutplanID"]))
                {
                    MyUtility.Msg.WarningBox("The CutPlan# has not been created yet, unable to proceed.");
                    return;
                }

                string sqlcmd = $@"
select * from dbo.GetSpreadingSchedule('{this.CurrentMaintain["FactoryID"]}','','',0,'{e.FormattedValue}')
ORDER BY OrderID ,Cutno ,SpreadingSchdlSeq";
                DualResult result = DBProxy.Current.Select(null, sqlcmd, out DataTable dt);
                if (!result)
                {
                    this.ShowErr(result);
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox($"Data not found!");
                    dr["CutRef"] = string.Empty;
                    return;
                }

                dr.Delete();
                foreach (DataRow drc in dt.Rows)
                {
                    ((DataTable)this.grid.DataSource).ImportRowAdded(drc);
                }

                DataTable detDtb = (DataTable)this.grid.DataSource; // 要被搜尋的grid
                this.listControlBindingSource.Position = detDtb.Rows.IndexOf(detDtb.Select($"CutRef = '{newvalue}'")[0]);
            };

            this.grid.DataSource = this.listControlBindingSource;

            this.grid.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.grid)
               .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(2), iseditable: true, trueValue: 1, falseValue: 0)
               .Text("CutRef", header: "CutRef#", width: Widths.AnsiChars(6), settings: cutRef)
               .Numeric("Cutno", header: "SEQ", width: Widths.AnsiChars(3), iseditingreadonly: true)
               .Text("Markername", header: "Maker\r\nName", width: Widths.AnsiChars(10), iseditingreadonly: true)
               .Text("FabricCombo", header: "Fabric\r\nCombo", width: Widths.AnsiChars(3), iseditingreadonly: true)
               .Text("FabricPanelCode", header: "Fab_Panel\r\nCode", width: Widths.AnsiChars(3), iseditingreadonly: true)
               .Text("Article", header: "Article", width: Widths.AnsiChars(8), iseditingreadonly: true)
               .Text("ColorID", header: "Color", width: Widths.AnsiChars(3), iseditingreadonly: true)
               .Text("multisize", header: "Size", width: Widths.AnsiChars(3), iseditingreadonly: true)
               .Numeric("Layer", header: "Layers", width: Widths.AnsiChars(3), iseditingreadonly: true)
               .Text("TotalCutQty", header: "Total\r\nCutQty", width: Widths.AnsiChars(12), iseditingreadonly: true)
               .Text("orderid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
               .Text("Seq1", header: "Seq1", width: Widths.AnsiChars(3), iseditingreadonly: true)
               .Text("Seq2", header: "Seq2", width: Widths.AnsiChars(3), iseditingreadonly: true)
               .Text("Refno", header: "Ref#", width: Widths.AnsiChars(12), iseditingreadonly: true)
               .Text("WeaveTypeID", header: "WeaveType", width: Widths.AnsiChars(15), iseditingreadonly: true)
               .Numeric("Cons", header: "Cons", width: Widths.AnsiChars(10), decimal_places: 4, iseditingreadonly: true)
               .Text("CutplanID", header: "CutPlan#", width: Widths.AnsiChars(14), iseditingreadonly: true).Get(out this.col_CutplanID)
               .Text("IssueID", header: "Issue ID", width: Widths.AnsiChars(14), iseditingreadonly: true)
                ;

            this.Change_record();
        }

        private void Change_record()
        {
            this.col_CutplanID.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.grid.GetDataRow(e.RowIndex);
                if (!MyUtility.Check.Empty(dr["diffEstCutDate"]))
                {
                    e.CellStyle.ForeColor = Color.Red;
                }
                else
                {
                    e.CellStyle.ForeColor = Color.Black;
                }
            };
        }

        private bool CheckCutrefToDay(string cutref)
        {
            string sqlcmd = $@"
            declare @today date = getdate()
select 1
from SpreadingSchedule ss
inner join SpreadingSchedule_Detail ssd on ss.Ukey = ssd.SpreadingScheduleUkey
where ssd.CutRef = '{cutref}'
and ss.EstCutDate = @today
and ss.Ukey <> '{this.CurrentMaintain["Ukey"]}'
";
            return MyUtility.Check.Seek(sqlcmd);
        }

        private void BtnFind_Click(object sender, EventArgs e)
        {
            if (this.dateEstCut.Value == null && this.txtCell1.Text.IsEmpty() && this.txtCutPlanID.IsEmpty() && this.txtOrderID.Text.IsEmpty())
            {
                MyUtility.Msg.WarningBox($"At least one of <Est. Cut Date>, <Cut Cell>, <CutPlan#>, or <SP#> must be entered.");
                return;
            }

            this.GetWorkOrderData();
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            this.dtDetail_New = this.dtDetail.Clone();

            if (this.listControlBindingSource.DataSource == null)
            {
                return;
            }

            ((DataTable)this.listControlBindingSource.DataSource).AsEnumerable().Where(r => Convert.ToBoolean(r["Selected"]) && (this.dtDetail.Rows.Count == 0 || !this.dtDetail.AsEnumerable().Where(o => o["CutRef"].ToString() == r["CutRef"].ToString()).Any())).ToList().ForEach(r =>
            {
                var newRow = this.dtDetail_New.NewRow();

                foreach (DataColumn column in ((DataTable)this.listControlBindingSource.DataSource).Columns)
                {
                    string columnName = column.ColumnName;

                    // 如果當前欄位名稱不在排除列表中，則進行複製
                    if (this.dtDetail_New.Columns.Contains(columnName))
                    {
                        newRow[columnName] = r[columnName];
                    }
                }

                newRow["SpreadingScheduleukey"] = this.CurrentMaintain["Ukey"];

                this.dtDetail_New.Rows.Add(newRow);
            });

            this.Close();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 取得資料
        /// </summary>
        private void GetWorkOrderData()
        {
            string strEstCut = this.dateEstCut.Value == null ? string.Empty : this.dateEstCut.Text;

            string sqlcmd = $@"
select Selected = 0,* from dbo.GetSpreadingSchedule('{this.CurrentMaintain["FactoryID"]}','{strEstCut}','{this.txtCell1.Text}',0,'')
where (CutPlanID = '{this.txtCutPlanID.Text}' or '{this.txtCutPlanID.Text}' = '') and (OrderID = '{this.txtOrderID.Text}' or '{this.txtOrderID.Text}' = '')
ORDER BY OrderID ,Cutno ,SpreadingSchdlSeq";
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out DataTable dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            // 若已存在表身，則自動勾選
            dt.AsEnumerable().Where(r => this.dtDetail.AsEnumerable().Where(o => o["CutRef"].ToString() == r["CutRef"].ToString()).Any()).ToList().ForEach(r =>
            {
                r["Selected"] = "1";
            });

            this.listControlBindingSource.DataSource = dt;
        }
    }
}
