using System;
using System.Data;
using Ict.Win;
using Sci.Data;
using Ict;

namespace Sci.Production.Warehouse
{
    public partial class P03_Wkno : Win.Subs.Base
    {
        DataRow dr;

        public P03_Wkno(DataRow data)
        {
            this.InitializeComponent();
            this.dr = data;
            this.Text += string.Format(" ({0}-{1}- {2})", this.dr["id"].ToString(),
this.dr["seq1"].ToString(),
this.dr["seq2"].ToString());
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            string selectCommand1 = string.Format(
                @"
Select a.id
	   , a.ETA
	   , a.WhseArrival
	   , b.qty
	   , b.foc
	   , a.vessel
	   , a.ShipModeID
       , a.Blno
From export a WITH (NOLOCK) 
	 , export_detail b WITH (NOLOCK) 
Where a.id = b.id
And b.poid = '{0}'
And b.seq1 = '{1}'
And b.seq2 = '{2}'", this.dr["id"].ToString(), this.dr["seq1"].ToString(), this.dr["seq2"].ToString());

            DataTable selectDataTable1;
            DualResult selectResult1 = DBProxy.Current.Select(null, selectCommand1, out selectDataTable1);
            if (selectResult1 == false)
            {
                this.ShowErr(selectCommand1, selectResult1);
            }

            this.bindingSource1.DataSource = selectDataTable1;

            // 設定Grid1的顯示欄位
            this.gridWkno.IsEditingReadOnly = true;
            this.gridWkno.DataSource = this.bindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridWkno)
                 .Text("id", header: "WK#", width: Widths.AnsiChars(18))
                 .Date("ETA", header: "ETA", width: Widths.AnsiChars(12))
                 .Date("WhseArrival", header: "Arrive W/H Date", width: Widths.AnsiChars(12))
                 .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(12), integer_places: 6, decimal_places: 4)
                 .Numeric("Foc", header: "FOC", width: Widths.AnsiChars(12), integer_places: 6, decimal_places: 4)
                 .Text("Vessel", header: "Vessel Name", width: Widths.AnsiChars(20))
                 .Text("Shipmodeid", header: "Ship Mode", width: Widths.AnsiChars(6))
                 .Text("Blno", header: "B/L No.", width: Widths.AnsiChars(20));
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
