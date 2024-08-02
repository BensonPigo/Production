using Ict;
using Ict.Win;
using Sci.Production.Prg;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static Sci.Production.Cutting.CuttingWorkOrder;

namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public partial class Cutpartcheck : Win.Subs.Base
    {
        private CuttingForm form;
        private string CuttingID;
        private IList<DataRow> DetailDatas;
        private DataTable dtDistribute;
        private DataTable dtSizeRatio;

        /// <summary>
        /// Initializes a new instance of the <see cref="Cutpartcheck"/> class.
        /// </summary>
        /// <param name="form">P02 / P09</param>
        /// <param name="cuttingID">Cut ID</param>
        /// <param name="detailDatas">DetailData</param>
        /// <param name="dtDistribute">currrent Distribute</param>
        /// <param name="dtPatternPanel">currrent PatternPanel</param>
        /// <param name="dtSizeRatio">P02 size Ratio Grid的當前資料</param>
        public Cutpartcheck(CuttingForm form, string cuttingID, IList<DataRow> detailDatas, DataTable dtSizeRatio, DataTable dtDistribute = null)
        {
            this.InitializeComponent();

            this.form = form;
            this.Text = $"Cut Parts Check<SP:{cuttingID}>";
            this.CuttingID = cuttingID;
            this.DetailDatas = detailDatas;
            this.dtSizeRatio = dtSizeRatio?.AsEnumerable().Where(w => w.RowState != DataRowState.Deleted).TryCopyToDataTable(dtSizeRatio);
            this.dtDistribute = dtDistribute?.AsEnumerable().Where(w => w.RowState != DataRowState.Deleted).TryCopyToDataTable(dtDistribute);
            this.Query();
            this.GridSetup();
            this.gridCutpartcheck.AutoResizeColumns();
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

            this.gridCutpartcheck.DataSource = dtCutPartCheck;
        }

        private void GridSetup()
        {
            this.Helper.Controls.Grid.Generator(this.gridCutpartcheck)
                .Text("ID", header: "SP #", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("Article", header: "Article", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("SizeCode", header: "SizeCode", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("Colorid", header: "Color", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("Patternpanel", header: "Comb", width: Widths.AnsiChars(2), iseditingreadonly: true)
                .Numeric("Qty", header: "Prd Qty", width: Widths.AnsiChars(7), iseditingreadonly: true)
                .Numeric("CutQty", header: "Cut Qty", width: Widths.AnsiChars(7), iseditingreadonly: true)
                .Numeric("Variance", header: "Variance", width: Widths.AnsiChars(7), iseditingreadonly: true)
                ;

            #region Grid 變色規則
            Color backDefaultColor = this.gridCutpartcheck.DefaultCellStyle.BackColor;
            this.gridCutpartcheck.RowsAdded += (s, e) =>
            {
                if (e.RowIndex < 0)
                {
                    return;
                }

                foreach (DataGridViewRow gridDr in this.gridCutpartcheck.Rows)
                {
                    DataRow dr = this.gridCutpartcheck.GetDataRow(gridDr.Index);

                    gridDr.DefaultCellStyle.BackColor = dr["Patternpanel"].ToString().EqualString("=") ? Color.Pink : backDefaultColor;
                    if (MyUtility.Convert.GetBool(dr["IsCancel"]))
                    {
                        gridDr.DefaultCellStyle.ForeColor = Color.Gray;
                    }
                    else
                    {
                        gridDr.DefaultCellStyle.ForeColor = Color.Black;
                    }
                }
            };
            #endregion
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
