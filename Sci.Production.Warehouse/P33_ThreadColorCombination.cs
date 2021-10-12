using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Data;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P33_ThreadColorCombination : Sci.Win.Subs.Base
    {
        private string poid = string.Empty;

        /// <inheritdoc/>
        public P33_ThreadColorCombination(string pOID)
        {
            this.InitializeComponent();
            this.poid = pOID;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            DataGridViewGeneratorTextColumnSettings operation = new DataGridViewGeneratorTextColumnSettings();

            // 點兩下開視窗
            operation.CellMouseDoubleClick += (s, e) =>
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    var dr = this.gridLeft.GetDataRow<DataRow>(e.RowIndex);
                    if (dr == null)
                    {
                        return;
                    }

                    string style_ThreadColorCombo_History_Ukey = MyUtility.Convert.GetString(dr["Ukey"]);

                    P33_Style_ThreadColorCombo_Operation form = new P33_Style_ThreadColorCombo_Operation(style_ThreadColorCombo_History_Ukey);
                    form.ShowDialog();
                }
            };

            string cmd = $@"
select sth.*
from Orders O
Inner join PO P on O.POID=P.ID
INNER JOIN Style_ThreadColorCombo_History sth ON sth.Version=p.ThreadVersion AND sth.StyleUkey=o.StyleUkey
where o.ID = '{this.poid}'
";
            DataTable leftDt;
            DBProxy.Current.Select(null, cmd, out leftDt);
            this.Left_Datasource.DataSource = leftDt;

            cmd = $@"
select sth.*
from Orders O
Inner join PO P on O.POID=P.ID
INNER JOIN Style_QTThreadColorCombo_History sth ON sth.Version=p.ThreadVersion AND sth.StyleUkey=o.StyleUkey
where o.ID = '{this.poid}'
";
            DataTable rightDt;
            DBProxy.Current.Select(null, cmd, out rightDt);
            this.Right_DataSource.DataSource = rightDt;

            this.Helper.Controls.Grid.Generator(this.gridLeft)
           .Text("Thread_ComboID", header: "Thread Combination", width: Widths.Auto(true), iseditingreadonly: true, settings: operation)
           .Text("MachineTypeID", header: "Machine Type", width: Widths.Auto(true), iseditingreadonly: true)
           .Button(header: "Color Combination", onclick: new EventHandler<DataGridViewCellEventArgs>(this.Buttoncell));

            this.Helper.Controls.Grid.Generator(this.gridRight)
           .Text("FabricPanelCode", header: "Fabric Panel Code", width: Ict.Win.Widths.AnsiChars(2))
           .Button(header: "Color Combination", onclick: new EventHandler<DataGridViewCellEventArgs>(this.QTButtoncell));
        }

        private void Buttoncell(object sender, DataGridViewCellEventArgs e)
        {
            DataTable detTable = (DataTable)((BindingSource)this.gridLeft.DataSource).DataSource;

            DataRow dr_Style_ThreadColorCombo_History = detTable.Rows[e.RowIndex];
            P33_Thread_Detail form = new P33_Thread_Detail(dr_Style_ThreadColorCombo_History);

            form.ShowDialog();
        }

        private void QTButtoncell(object sender, DataGridViewCellEventArgs e)
        {
            DataTable detTable = (DataTable)((BindingSource)this.gridRight.DataSource).DataSource;

            DataRow dr_Style_QTThreadColorCombo_History = detTable.Rows[e.RowIndex];
            P33_QTThread_Detail form = new P33_QTThread_Detail(dr_Style_QTThreadColorCombo_History);

            form.ShowDialog();
        }
    }
}
