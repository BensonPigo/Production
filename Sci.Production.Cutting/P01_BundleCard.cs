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
            requery();
            gridSetup();
            this.grid1.AutoResizeColumns();
        }
        private void requery()
        {
            string sqlcmd = String.Format(
           @"Select a.id,b.bundleno,a.orderid,a.cdate,a.cutref,a.PatternPanel,a.cutno,b.sizecode,b.bundlegroup,b.Qty
            ,PrintDate
            from Bundle a, Bundle_Detail b 
            Where a.id = b.id and a.POID = '{0}' order by BundleNo", cutid);
            DataTable gridtb;
            DualResult dr = DBProxy.Current.Select(null, sqlcmd, out gridtb);
            grid1.DataSource = gridtb;
        }
        private void gridSetup()
        {
            Helper.Controls.Grid.Generator(this.grid1)
                .Text("bundleno", header: "Bundle No.", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("orderid", header: "SP#", width: Widths.AnsiChars(16), iseditingreadonly: true)
                .Date("cdate", header: "Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("cutref", header: "Cut Ref", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("PatternPanel", header: "Comb", width: Widths.AnsiChars(1), iseditingreadonly: true)
                .Numeric("cutno", header: "Cut No.", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Text("sizecode", header: "Size", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Numeric("bundlegroup", header: "Bundle Group", width: Widths.AnsiChars(7), iseditingreadonly: true)
                .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(7), iseditingreadonly: true)
                .Text("PrintDate", header: "PrintDate", width: Widths.AnsiChars(8), iseditingreadonly: true);

        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
