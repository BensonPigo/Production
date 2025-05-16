using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Prg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static Sci.Production.Cutting.CuttingWorkOrder;

namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public partial class Cutpartchecksummary : Win.Subs.Base
    {
        private CuttingForm form;
        private string CuttingID;
        private IList<DataRow> DetailDatas;
        private DataTable dtDistribute;
        private DataTable dtSizeRatio;
        private List<string> listPatternPanels;

        /// <summary>
        /// Initializes a new instance of the <see cref="Cutpartchecksummary"/> class.
        /// </summary>
        /// <param name="form">P02 / P09</param>
        /// <param name="cuttingID">Cut ID</param>
        /// <param name="sourceTable">source Table</param>
        /// <param name="detailDatas">DetailData</param>
        /// <param name="dtDistribute">P09 當前Distribute資料</param>
        /// <param name="dtSizeRatio">P02 size Ratio Grid的當前資料</param>
        public Cutpartchecksummary(CuttingForm form, string cuttingID, IList<DataRow> detailDatas, DataTable dtSizeRatio, DataTable dtDistribute = null)
        {
            this.InitializeComponent();
            this.form = form;
            this.Text = $"Cut Parts Check Summary<SP:{cuttingID}>";
            this.CuttingID = cuttingID;
            this.DetailDatas = detailDatas;
            this.dtSizeRatio = dtSizeRatio?.AsEnumerable().Where(w => w.RowState != DataRowState.Deleted).TryCopyToDataTable(dtSizeRatio);
            this.dtDistribute = dtDistribute?.AsEnumerable().Where(w => w.RowState != DataRowState.Deleted).TryCopyToDataTable(dtDistribute);
            this.Query();
            this.GridSetup();
            this.gridCutpartchecksummary.AutoResizeColumns();
        }

        private void Query()
        {
            // 把當前的資料進行整合
            DataTable dtWorkOrder = ProcessWorkOrder_CutPartCheck(this.form, this.DetailDatas, this.dtSizeRatio, this.dtDistribute);
            DualResult result = GetBase_CutPartCheck(this.form, this.CuttingID, dtWorkOrder, out DataTable dtCutPartCheck);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.gridCutpartchecksummary.DataSource = ProcessCutpartCheckSummary(dtCutPartCheck);

            this.listPatternPanels = dtCutPartCheck.AsEnumerable().Where(row => MyUtility.Convert.GetString(row["Patternpanel"]) != "=").Select(row => MyUtility.Convert.GetString(row["Patternpanel"])).Distinct().ToList();
        }

        private void GridSetup()
        {
            this.Helper.Controls.Grid.Generator(this.gridCutpartchecksummary)
                .Text("ID", header: "SP #", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("Article", header: "Article", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("SizeCode", header: "SizeCode", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Numeric("Qty", header: "Order Qty", width: Widths.AnsiChars(7), iseditingreadonly: true)
                .Text("Complete", header: "Complete", width: Widths.AnsiChars(1), iseditingreadonly: true)
                ;

            var patternPanels = ((DataTable)this.gridCutpartchecksummary.DataSource).AsEnumerable()
                .Select(row => MyUtility.Convert.GetString(row["Patternpanel"]))
                .Where(patternPanelValue => patternPanelValue != "=")
                .Distinct();

            foreach (string patternPanel in this.listPatternPanels)
            {
                this.Helper.Controls.Grid.Generator(this.gridCutpartchecksummary)
                    .Numeric(patternPanel, header: patternPanel, width: Widths.AnsiChars(7)).Get(out Ict.Win.UI.DataGridViewNumericBoxColumn col_patternPanel);
                col_patternPanel.CellFormatting += (s, e) =>
                {
                    // 未完成 & 不是 Cancel, 數字顯示 紅色
                    DataRow dr = this.gridCutpartchecksummary.GetDataRow(e.RowIndex);
                    if (MyUtility.Convert.GetInt(dr[patternPanel]) < MyUtility.Convert.GetInt(dr["Qty"]) && !MyUtility.Convert.GetBool(dr["IsCancel"]))
                    {
                        e.CellStyle.ForeColor = Color.Red;
                    }
                };
            }

            Color backDefaultColor = this.gridCutpartchecksummary.DefaultCellStyle.BackColor;
            this.gridCutpartchecksummary.RowsAdded += (s, e) =>
            {
                int index = 0;
                string article = string.Empty;
                foreach (DataGridViewRow gridViewRow in this.gridCutpartchecksummary.Rows)
                {
                    DataRow dr = this.gridCutpartchecksummary.GetDataRow(gridViewRow.Index);
                    bool isCancel = MyUtility.Convert.GetBool(dr["IsCancel"]);
                    gridViewRow.DefaultCellStyle.ForeColor = isCancel ? Color.Gray : Color.Black;

                    if (index == 0)
                    {
                        article = MyUtility.Convert.GetString(dr["Article"]);
                        index++;
                        continue;
                    }

                    if (gridViewRow.Cells[1].Value.ToString() != article)
                    {
                        this.gridCutpartchecksummary.Rows[index - 1].DefaultCellStyle.BackColor = Color.Pink;
                        article = gridViewRow.Cells[1].Value.ToString();
                    }
                    else
                    {
                        this.gridCutpartchecksummary.Rows[index - 1].DefaultCellStyle.BackColor = backDefaultColor;
                    }

                    index++;
                }
            };
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
