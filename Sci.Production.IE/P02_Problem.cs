﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;

namespace Sci.Production.IE
{
    public partial class P02_Problem : Sci.Win.Subs.Input4
    {

        public P02_Problem(bool canedit, string keyvalue1, string keyvalue2, string keyvalue3)
            : base(canedit, keyvalue1, keyvalue2, keyvalue3)
        {
            InitializeComponent();
        }

        protected override bool OnGridSetup()
        {
            Ict.Win.DataGridViewGeneratorTextColumnSettings iereason = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
            iereason.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode)
                {
                    if (e.Button == System.Windows.Forms.MouseButtons.Right)
                    {
                        if (e.RowIndex != -1)
                        {
                            DataRow dr = this.grid.GetDataRow<DataRow>(e.RowIndex);
                            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem("select ID,Description from IEReason where Type = 'CP' and Junk = 0 order by ID", "4,30", dr["IEReasonID"].ToString());
                            DialogResult returnResult = item.ShowDialog();
                            if (returnResult == DialogResult.Cancel) { return; }
                            IList<DataRow> selectedData = item.GetSelecteds();
                            dr["IEReasonID"] = item.GetSelectedString();
                            dr["IEReasonDesc"] = (selectedData[0])["Description"].ToString();
                        }
                    }
                }
            };

            Helper.Controls.Grid.Generator(this.grid)
                .Text("IEReasonDesc", header: "Problem Encountered", width: Widths.AnsiChars(15), iseditingreadonly: true, settings: iereason)
                .EditText("ShiftA", header: "Shift A", width: Widths.AnsiChars(30))
                .EditText("ShiftB", header: "Shift B", width: Widths.AnsiChars(30));
            return true;
        }

        protected override DualResult OnRequery()
        {
            string selectCommand = string.Format(@"select cp.*,ir.Description as IEReasonDesc 
from ChgOver_Problem cp
left join IEReason ir on cp.IEReasonID = ir.ID and ir.Type = 'CP'
where cp.ID = {0}", this.KeyValue1);
            Ict.DualResult returnResult;
            DataTable ChgOverProblem = new DataTable();
            returnResult = DBProxy.Current.Select(null, selectCommand, out ChgOverProblem);
            if (!returnResult)
            {
                return returnResult;
            }
            SetGrid(ChgOverProblem);
            return Result.True;
        }

    }
}
