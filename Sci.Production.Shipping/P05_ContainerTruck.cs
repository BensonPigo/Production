using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;

namespace Sci.Production.Shipping
{
    public partial class P05_ContainerTruck : Sci.Win.Subs.Input4
    {
        public P05_ContainerTruck(bool canedit, string keyvalue1, string keyvalue2, string keyvalue3)
            : base(canedit, keyvalue1, keyvalue2, keyvalue3)
        {
            InitializeComponent();
        }

        protected override bool OnGridSetup()
        {
            Helper.Controls.Grid.Generator(this.grid)
                .Text("Type", header: "Container Type", width: Widths.AnsiChars(20))
                .Text("CTNRNo", header: "Container#", width: Widths.AnsiChars(10))
                .Text("SealNo", header: "Seal#", width: Widths.AnsiChars(10))
                .Text("TruckNo", header: "Truck#/Traile#", width: Widths.AnsiChars(10))
                .Text("AddBy", header: "Add by", width: Widths.AnsiChars(30),iseditingreadonly:true)
                .Text("EditBy", header: "Edit by", width: Widths.AnsiChars(30),iseditingreadonly:true);

            return true;
        }

        protected override void OnRequeryPost(DataTable datas)
        {
            base.OnRequeryPost(datas);

            datas.Columns.Add("AddBy");
            datas.Columns.Add("EditBy");
            foreach (DataRow gridData in datas.Rows)
            {
                gridData["AddBy"] = MyUtility.Convert.GetString(gridData["AddName"]) + " - " + MyUtility.GetValue.Lookup("Name", MyUtility.Convert.GetString(gridData["AddName"]),"Pass1","ID") + "   " + ((DateTime)gridData["AddDate"]).ToString(string.Format("{0}", Sci.Env.Cfg.DateTimeStringFormat));
                if (!MyUtility.Check.Empty(gridData["EditDate"]))
                {
                    gridData["EditBy"] = MyUtility.Convert.GetString(gridData["EditName"]) + " - " + MyUtility.GetValue.Lookup("Name", MyUtility.Convert.GetString(gridData["EditName"]), "Pass1", "ID") + "   " + ((DateTime)gridData["EditDate"]).ToString(string.Format("{0}", Sci.Env.Cfg.DateTimeStringFormat));
                }
                gridData.AcceptChanges();
            }
        }
    }
}
