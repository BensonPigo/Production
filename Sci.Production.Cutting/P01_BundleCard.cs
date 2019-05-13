﻿using Ict;
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
        private string M;
        public P01_BundleCard(string cID, string m)
        {
            InitializeComponent();
            cutid = cID;
            M = m;
            requery();
            gridSetup();
            this.gridBundleCard.AutoResizeColumns();
        }
        private void requery()
        {
            string sqlcmd = $@"
Select a.id,b.BundleNo,a.orderid,a.cdate,a.cutref,a.PatternPanel,a.cutno,b.sizecode,b.bundlegroup,b.Qty,b.PrintDate
from Bundle a WITH (NOLOCK)
inner join Bundle_Detail b WITH (NOLOCK) on a.id = b.Id
inner join Orders o WITH (NOLOCK) on a.Orderid = o.ID and a.MDivisionID = o.MDivisionID
where a.POID = '{cutid}' and a.MDivisionID  = '{M}'
order by b.BundleNo
";
            DataTable gridtb;
            DualResult dr = DBProxy.Current.Select(null, sqlcmd, out gridtb);
            gridBundleCard.DataSource = gridtb;
        }
        private void gridSetup()
        {
            Helper.Controls.Grid.Generator(this.gridBundleCard)
                .Text("bundleno", header: "Bundle No.", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("orderid", header: "SP#", width: Widths.AnsiChars(16), iseditingreadonly: true)
                .Date("cdate", header: "Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("cutref", header: "Cut Ref", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("PatternPanel", header: "Comb", width: Widths.AnsiChars(1), iseditingreadonly: true)
                .Numeric("cutno", header: "Cut No.", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Text("sizecode", header: "Size", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Numeric("bundlegroup", header: "Bundle Group", width: Widths.AnsiChars(7), iseditingreadonly: true)
                .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(7), iseditingreadonly: true)
                .DateTime("PrintDate", header: "PrintDate", width: Widths.AnsiChars(8), iseditingreadonly: true);

        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
