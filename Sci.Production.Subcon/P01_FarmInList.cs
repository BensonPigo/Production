﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;


namespace Sci.Production.Subcon
{
    public partial class P01_FarmInList : Sci.Win.Subs.Base
    {
        protected DataRow dr;
        public P01_FarmInList(DataRow data)
        {
            InitializeComponent();
            dr = data;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            string selectCommand1 = string.Format(@"SELECT B.id, A.issuedate, B.QTY, A.Status , A.ADDDATE 
                                FROM FarmIn A, FarmIn_Detail B 
                                WHERE artworkpoid = '{0}' AND A.id = B.id AND b.artworkpo_detailukey = {1}"
                                , dr["id"].ToString(), dr["ukey"].ToString());
            DataTable selectDataTable1;

            DualResult selectResult1 = DBProxy.Current.Select(null, selectCommand1, out selectDataTable1);
            if (selectResult1 == false) ShowErr(selectCommand1,selectResult1);

            bindingSource1.DataSource = selectDataTable1;

            //設定Grid1的顯示欄位
            this.grid1.IsEditingReadOnly = true;
            this.grid1.DataSource = bindingSource1;
            Helper.Controls.Grid.Generator(this.grid1)
                 .Text("id", header: "Farm-In#", width: Widths.AnsiChars(13))
                 .Date("issuedate", header: "Date", width: Widths.AnsiChars(13))
                 .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(6), integer_places: 6, decimal_places: 0)
                 .Text("Status", header: "Status", width: Widths.AnsiChars(8))
                 .DateTime("adddate", header: "Create Date", width: Widths.AnsiChars(20));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
