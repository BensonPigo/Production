using Ict;
using Ict.Win;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Sci.Data;

namespace Sci.Production.Cutting
{
    public partial class P01_BundleCard : Sci.Win.Subs.Base
    {
        private string cutid;
        public P01_BundleCard(string cID)
        {
            InitializeComponent();
            cutid = cID;
        }
        private void requert()
        {
            string sqlcmd = String.Format(
            @"Select a.id,b.bundleno,a.orderid,a.cdate,a.cutrefno,a.fabriccombo,a.cutno,b.sizecode,b.bundlegroup,b.Qty 
            from Bundle a, Bundle_Detail b 
            Where a.id = b.id and a.Cuttingid = '{0}'",cutid);
            DataTable gridtb;
            DualResult dr = DBProxy.Current.Select(null, sqlcmd, out gridtb);
            grid1.DataSource = gridtb;
        }
        private void gridSetup()
        {
            Helper.Controls.Grid.Generator(this.grid1)
                .Text("id", header: "ID", width: Widths.AnsiChars(46), iseditingreadonly: true)
                .Text("bundleno", header: "Bundle No.", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("orderid", header: "SP No", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Date("cdate", header: "Date", width: Widths.AnsiChars(7), iseditingreadonly: true)
                .Text("cutrefno", header: "Cut Ref#", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("fabriccombo", header: "Comb", width: Widths.AnsiChars(1), iseditingreadonly: true)
                .Numeric("cutno", header: "Cut No.", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Text("sizecode", header: "Suze", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Numeric("bundlegroup", header: "Bundle Group", width: Widths.AnsiChars(7), iseditingreadonly: true)
                .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(7), iseditingreadonly: true);

        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
