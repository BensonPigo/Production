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
using Sci;

namespace Sci.Production.Subcon
{
    public partial class P01_FarmOutList : Sci.Win.Subs.Base
    {
        protected DataRow dr;
        public P01_FarmOutList( DataRow data)
        {
            InitializeComponent();
            dr = data;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            string selectCommand1 = string.Format(@"select B.id, A.issuedate,B.QTY,a.Status, A.ADDDATE 
                                                    FROM FarmOut A WITH (NOLOCK) ,FarmOut_Detail B WITH (NOLOCK) 
                                                    WHERE b.artworkpoid = '{0}' AND A.id = B. ID AND b.artworkpo_detailukey = {1}"
                                                    , dr["id"].ToString(), dr["ukey"].ToString());
            DataTable selectDataTable1;

            DualResult selectResult1 = DBProxy.Current.Select(null, selectCommand1, out selectDataTable1);

            if (selectResult1 == false) ShowErr(selectResult1);

            bindingSource1.DataSource = selectDataTable1;

            

            //設定Grid1的顯示欄位
            this.gridFarmOutList.IsEditingReadOnly = true;
            this.gridFarmOutList.DataSource = bindingSource1;
            Helper.Controls.Grid.Generator(this.gridFarmOutList)
                 .Text("id", header: "Farm-Out#", width: Widths.AnsiChars(13))
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
