using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Sci;
using Sci.Data;
using Ict;

namespace Sci.Production.Warehouse
{
    public partial class P03_Wkno : Sci.Win.Subs.Base
    {
        DataRow dr;
        public P03_Wkno(DataRow data)
        {
            InitializeComponent();
            dr = data;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            string selectCommand1 = string.Format(@"Select a.id, a.ETA, b.qty, b.foc, a.vessel, a.ShipModeID
                                                                                From export a, export_detail b
                                                                                Where a.id = b.id
                                                                                And b.poid = '{0}'
                                                                                And b.seq1 = '{1}'
                                                                                And b.seq2 = '{2}'"
                                                                            , dr["id"].ToString(), dr["seq1"].ToString(), dr["seq2"].ToString());
            DataTable selectDataTable1;
            DualResult selectResult1 = DBProxy.Current.Select(null, selectCommand1, out selectDataTable1);
            if (selectResult1 == false) ShowErr(selectCommand1, selectResult1);

            bindingSource1.DataSource = selectDataTable1;

            //設定Grid1的顯示欄位
            this.grid1.IsEditingReadOnly = true;
            this.grid1.DataSource = bindingSource1;
            Helper.Controls.Grid.Generator(this.grid1)
                 .Text("id", header: "WK#", width: Widths.AnsiChars(13))
                 .Date("issuedate", header: "ETA", width: Widths.AnsiChars(12))
                 .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(12), integer_places: 6, decimal_places: 4)
                 .Numeric("Foc", header: "FOC", width: Widths.AnsiChars(12), integer_places: 6, decimal_places: 4)
                 .Text("Vessel", header: "Vessel Name", width: Widths.AnsiChars(20))
                 .Text("Shipmodeid", header: "Ship Mode", width: Widths.AnsiChars(6));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
