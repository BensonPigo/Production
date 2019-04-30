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
    /// <summary>
    /// P05_ContainerTruck
    /// </summary>
    public partial class P10_ContainerTruck : Sci.Win.Subs.Input4
    {
        private string ShipPlanID;
        private Dictionary<string, string> di_CYCFS = new Dictionary<string, string>();

        /// <summary>
        /// P05_ContainerTruck
        /// </summary>
        /// <param name="canedit">canedit</param>
        /// <param name="keyvalue1">keyvalue1</param>
        /// <param name="keyvalue2">keyvalue2</param>
        /// <param name="keyvalue3">keyvalue3</param>
        /// <param name="shipPlanID">shipPlanID</param>
        public P10_ContainerTruck(bool canedit, string keyvalue1, string keyvalue2, string keyvalue3 , string shipPlanID)
            : base(canedit, keyvalue1, keyvalue2, keyvalue3)
        {
            this.ShipPlanID = shipPlanID;
            this.InitializeComponent();
            this.di_CYCFS.Add("CFS", "CFS");
            this.di_CYCFS.Add("20 STD", "20 STD");
            this.di_CYCFS.Add("40 STD", "40 STD");
            this.di_CYCFS.Add("40HQ", "40HQ");
            this.di_CYCFS.Add("45HQ", "45HQ");
            this.di_CYCFS.Add("AIR", "AIR");
        }

        /// <inheritdoc/>
        protected override bool OnGridSetup()
        {
            Ict.Win.UI.DataGridViewComboBoxColumn cbb_CYCFS;

            Ict.Win.DataGridViewGeneratorTextColumnSettings id = new DataGridViewGeneratorTextColumnSettings();
            id.CellValidating += (s, e) =>
            {
                if (!this.EditMode || MyUtility.Check.Empty(e.FormattedValue) || e.RowIndex == -1)
                {
                    return;
                }

                string chkgbid = $@"select id,CYCFS from GMTBooking g WITH (NOLOCK) where g.shipPlanID = '{this.ShipPlanID}' and id = '{e.FormattedValue}'";
                DataRow dr;
                if (MyUtility.Check.Seek(chkgbid,out dr))
                {
                    this.CurrentData["ID"] = e.FormattedValue;
                    this.CurrentData["CYCFS"] = dr["CYCFS"];
                }
                else
                {
                    MyUtility.Msg.WarningBox("No this GB#");
                    this.CurrentData["ID"] = string.Empty;
                }

                this.CurrentData.EndEdit();
            };

            this.Helper.Controls.Grid.Generator(this.grid)
            .Text("ID", header: "GB#", width: Widths.AnsiChars(20), settings: id)
            .Text("CYCFS", header: "Loading Type", iseditable: false)
            .ComboBox("Type", header: "Container Type", width: Widths.AnsiChars(20)).Get(out cbb_CYCFS)
            .Text("CTNRNo", header: "Container#", width: Widths.AnsiChars(10))
            .Text("SealNo", header: "Seal#", width: Widths.AnsiChars(10))
            .Text("TruckNo", header: "Truck#/Traile#", width: Widths.AnsiChars(10))
            .Text("AddBy", header: "Add by", width: Widths.AnsiChars(30), iseditingreadonly: true)
            .Text("EditBy", header: "Edit by", width: Widths.AnsiChars(30), iseditingreadonly: true);

            cbb_CYCFS.DataSource = new BindingSource(this.di_CYCFS, null);
            cbb_CYCFS.ValueMember = "Key";
            cbb_CYCFS.DisplayMember = "Value";
            return true;
        }

        /// <inheritdoc/>
        protected override bool OnSaveBefore()
        {
            DataTable dt = (DataTable)this.gridbs.DataSource;
            for (int i = dt.Rows.Count -1 ; i >= 0; i--)
            {
                if (dt.Rows[i].RowState != DataRowState.Deleted &&
                    MyUtility.Check.Empty(dt.Rows[i]["TYPE"].ToString()) &&
                    MyUtility.Check.Empty(dt.Rows[i]["CTNRNo"].ToString()) &&
                    MyUtility.Check.Empty(dt.Rows[i]["SealNo"].ToString()) &&
                    MyUtility.Check.Empty(dt.Rows[i]["TruckNo"].ToString()))
                {
                    dt.Rows[i].Delete();
                }
            }

            return base.OnSaveBefore();
        }

        protected override void OnRequired()
        {
            base.OnRequired();
            DataTable datas;
            string sqlcmd = $@"
            select gc.*
            from GMTBooking_CTNR gc with(nolock)
            inner join GMTBooking g with(nolock) on gc.id = g.id
            where g.ShipPlanID ='{this.ShipPlanID}'
            ";
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out datas);
            if (!result)
            {
                return;
            }

            this.SetGrid(datas);
            datas.Columns.Add("AddBy");
            datas.Columns.Add("EditBy");
            foreach (DataRow gridData in datas.Rows)
            {
                gridData["AddBy"] = MyUtility.Convert.GetString(gridData["AddName"]) + " - " + MyUtility.GetValue.Lookup("Name", MyUtility.Convert.GetString(gridData["AddName"]), "Pass1", "ID") + "   " + ((DateTime)gridData["AddDate"]).ToString(string.Format("{0}", Sci.Env.Cfg.DateTimeStringFormat));
                if (!MyUtility.Check.Empty(gridData["EditDate"]))
                {
                    gridData["EditBy"] = MyUtility.Convert.GetString(gridData["EditName"]) + " - " + MyUtility.GetValue.Lookup("Name", MyUtility.Convert.GetString(gridData["EditName"]), "Pass1", "ID") + "   " + ((DateTime)gridData["EditDate"]).ToString(string.Format("{0}", Sci.Env.Cfg.DateTimeStringFormat));
                }

                gridData.AcceptChanges();
            }
        }
    }
}
