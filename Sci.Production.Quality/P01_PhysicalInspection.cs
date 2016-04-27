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

namespace Sci.Production.Quality
{
    public partial class P01_PhysicalInspection : Sci.Win.Subs.Base
    {
        DataRow dr;
        public P01_PhysicalInspection(long id)
        {
        }
        public P01_PhysicalInspection(DataRow data)
        {
            InitializeComponent();
            dr = data;
            this.Text += string.Format(" ({0}-{1}- {2})", dr["id"].ToString()
, dr["seq1"].ToString()
, dr["seq2"].ToString());
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            
            DataTable selectDataTable1;
            //DualResult selectResult1 = DBProxy.Current.Select(null, selectCommand1, out selectDataTable1);
            //if (selectResult1 == false) ShowErr(selectCommand1, selectResult1);

            //bindingSource1.DataSource = selectDataTable1;

            //設定Grid1的顯示欄位
        //    this.grid1.IsEditingReadOnly = true;
        //    this.grid1.DataSource = bindingSource1;
        //    Helper.Controls.Grid.Generator(this.grid1)
        //         .Text("roll", header: "Roll#", width: Widths.AnsiChars(13))
        //         .Text("dyelot", header: "Dyelot", width: Widths.AnsiChars(13))
        //         .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(13), integer_places: 8, decimal_places: 2)
        //         ;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
