﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;

namespace Sci.Production.PPIC
{
    public partial class P04_GarmentLeadTimeByFactory : Sci.Win.Subs.Input4
    {
        public P04_GarmentLeadTimeByFactory(bool canedit, string keyvalue1, string keyvalue2, string keyvalue3)
            : base(canedit, keyvalue1, keyvalue2, keyvalue3)
        {
            InitializeComponent();
        }

        protected override bool OnGridSetup()
        {
            Helper.Controls.Grid.Generator(this.grid)
                .Text("FactoryID", header: "Factory", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Numeric("GMTLT", header: "Garment L/T", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("CreateBy", header: "Create By", width: Widths.AnsiChars(30), iseditingreadonly: true)
                .Text("EditBy", header: "Edit By", width: Widths.AnsiChars(30), iseditingreadonly: true);
            
            return true;
        }

        protected override void OnRequeryPost(DataTable datas)
        {
            base.OnRequeryPost(datas);
            datas.Columns.Add("CreateBy");
            datas.Columns.Add("EditBy");
            foreach (DataRow gridData in datas.Rows)
            {
                gridData["CreateBy"] = gridData["AddName"].ToString() + " - " + MyUtility.GetValue.Lookup("Name", gridData["AddName"].ToString(), "Pass1", "ID") + "   " + ((DateTime)gridData["AddDate"]).ToString(string.Format("{0}", Sci.Env.Cfg.DateTimeStringFormat));
                if (gridData["EditDate"] != System.DBNull.Value)
                {
                    gridData["EditBy"] = gridData["EditName"].ToString() + " - " + MyUtility.GetValue.Lookup("Name", gridData["EditName"].ToString(), "Pass1", "ID") + "   " + ((DateTime)gridData["EditDate"]).ToString(string.Format("{0}", Sci.Env.Cfg.DateTimeStringFormat));
                }
                gridData.AcceptChanges();
            }
        }
    }
}
