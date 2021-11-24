using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Win.Tems;

namespace Sci.Production.Centralized
{
    /// <summary>
    /// IE_B05
    /// </summary>
    public partial class IE_B05 : Sci.Win.Tems.Input6
    {
        /// <summary>
        /// IE_B05
        /// </summary>
        public IE_B05(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            string sql = string.Format("select * from [MachineType_ThreadRatio] where ID='{0}'", this.CurrentMaintain["ID"].ToString());
            if (MyUtility.Check.Seek(sql, this.ConnectionName))
            {
                this.btnThreadRatio.ForeColor = Color.Blue;
            }
            else
            {
                this.btnThreadRatio.ForeColor = DefaultForeColor;
            }
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();

            this.DetailSelectCommand = string.Format(
                @"
select * 
from ProductionTPE.dbo.MachineType_Detail
Where id = '{0}'
", masterID);

            return base.OnDetailSelectCommandPrepare(e);
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();

            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("FactoryID", header: "Factory", width: Widths.AnsiChars(11))
                .CheckBox("IsSubprocess", header: "Subprocess", width: Widths.AnsiChars(12), iseditable: true, trueValue: true, falseValue: false)
                .CheckBox("IsNonSewingLine", header: "Non-Sewing Line", width: Widths.AnsiChars(15), iseditable: true, trueValue: true, falseValue: false)
                .CheckBox("IsNotShownInP01", header: "Not shown in P01", width: Widths.AnsiChars(15), iseditable: true, trueValue: true, falseValue: false)
                .CheckBox("IsNotShownInP03", header: "Not shown in P03", width: Widths.AnsiChars(15), iseditable: true, trueValue: true, falseValue: false)
           ;
        }

        private void BtnThreadRatio_Click(object sender, EventArgs e)
        {
            IE_B05_ThreadRatio callNextForm = new IE_B05_ThreadRatio(this.CurrentMaintain["ID"].ToString());
            DialogResult result = callNextForm.ShowDialog(this);
        }
    }
}
