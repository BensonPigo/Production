using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;

namespace Sci.Production.IE
{
    /// <summary>
    /// IE_P02_Problem
    /// </summary>
    public partial class P02_Problem : Win.Subs.Input4
    {
        /// <summary>
        /// P02_Problem
        /// </summary>
        /// <param name="canedit">canedit</param>
        /// <param name="keyvalue1">keyvalue1</param>
        /// <param name="keyvalue2">keyvalue2</param>
        /// <param name="keyvalue3">keyvalue3</param>
        public P02_Problem(bool canedit, string keyvalue1, string keyvalue2, string keyvalue3)
            : base(canedit, keyvalue1, keyvalue2, keyvalue3)
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// OnGridSetup
        /// </summary>
        /// <returns>bool</returns>
        protected override bool OnGridSetup()
        {
            DataGridViewGeneratorTextColumnSettings iereason = new DataGridViewGeneratorTextColumnSettings();
            iereason.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode)
                {
                    if (e.Button == System.Windows.Forms.MouseButtons.Right)
                    {
                        if (e.RowIndex != -1)
                        {
                            DataRow dr = this.grid.GetDataRow<DataRow>(e.RowIndex);
                            Win.Tools.SelectItem item = new Win.Tools.SelectItem("select ID,Description from IEReason WITH (NOLOCK) where Type = 'CP' and Junk = 0 order by ID", "4,30", dr["IEReasonID"].ToString());
                            DialogResult returnResult = item.ShowDialog();
                            if (returnResult == DialogResult.Cancel)
                            {
                                return;
                            }

                            IList<DataRow> selectedData = item.GetSelecteds();
                            dr["IEReasonID"] = item.GetSelectedString();
                            dr["IEReasonDesc"] = selectedData[0]["Description"].ToString();
                        }
                    }
                }
            };

            this.Helper.Controls.Grid.Generator(this.grid)
                .Text("IEReasonDesc", header: "Problem Encountered", width: Widths.AnsiChars(15), iseditingreadonly: true, settings: iereason)
                .EditText("ShiftA", header: "Shift A", width: Widths.AnsiChars(30))
                .EditText("ShiftB", header: "Shift B", width: Widths.AnsiChars(30));
            return true;
        }

        /// <summary>
        /// OnRequery
        /// </summary>
        /// <returns>DualResult</returns>
        protected override DualResult OnRequery()
        {
            string selectCommand = string.Format(
                @"select cp.*,ir.Description as IEReasonDesc 
from ChgOver_Problem cp WITH (NOLOCK) 
left join IEReason ir WITH (NOLOCK) on cp.IEReasonID = ir.ID and ir.Type = 'CP'
where cp.ID = {0}", this.KeyValue1);
            DualResult returnResult;
            DataTable chgOverProblem = new DataTable();
            returnResult = DBProxy.Current.Select(null, selectCommand, out chgOverProblem);
            if (!returnResult)
            {
                return returnResult;
            }

            this.SetGrid(chgOverProblem);
            return Result.True;
        }

        // Save -- Append/Revise/Delete按鈕要隱藏
        private void Save_Click(object sender, EventArgs e)
        {
            this.append.Visible = false;
            this.revise.Visible = false;
            this.delete.Visible = false;
        }
    }
}
