using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;
using System.Linq;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// P05_ContainerTruck
    /// </summary>
    public partial class P10_ContainerTruck : Win.Subs.Input4
    {
        private string ShipPlanID;
        private Dictionary<string, string> di_CYCFS = new Dictionary<string, string>();
        Action reloadParant;

        /// <summary>
        /// P05_ContainerTruck
        /// </summary>
        /// <param name="canedit">canedit</param>
        /// <param name="keyvalue1">keyvalue1</param>
        /// <param name="keyvalue2">keyvalue2</param>
        /// <param name="keyvalue3">keyvalue3</param>
        /// <param name="shipPlanID">shipPlanID</param>
        public P10_ContainerTruck(bool canedit, string keyvalue1, string keyvalue2, string keyvalue3, string shipPlanID, Action ReloadParant)
            : base(canedit, keyvalue1, keyvalue2, keyvalue3)
        {
            this.ShipPlanID = shipPlanID;
            this.InitializeComponent();
            this.di_CYCFS.Add("20 STD", "20 STD");
            this.di_CYCFS.Add("40 STD", "40 STD");
            this.di_CYCFS.Add("40HQ", "40HQ");
            this.di_CYCFS.Add("45HQ", "45HQ");
            this.reloadParant = ReloadParant;
        }

        /// <inheritdoc/>
        protected override bool OnGridSetup()
        {
            Ict.Win.UI.DataGridViewComboBoxColumn cbb_CYCFS;

            DataGridViewGeneratorTextColumnSettings id = new DataGridViewGeneratorTextColumnSettings();

            // 限制最大字數，避免寫入DB錯誤
            DataGridViewGeneratorTextColumnSettings CTNRNo = new DataGridViewGeneratorTextColumnSettings() { MaxLength = 20 };
            DataGridViewGeneratorTextColumnSettings SealNo = new DataGridViewGeneratorTextColumnSettings() { MaxLength = 15 };
            DataGridViewGeneratorTextColumnSettings TruckNo = new DataGridViewGeneratorTextColumnSettings() { MaxLength = 20 };

            id.EditingMouseDown += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                if (this.EditMode == false)
                {
                    return;
                }

                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    DataTable selectDt;
                    string strSelectSqlCmd = $@"
select [GB#]=g.ID,LoadingType=g.CYCFS,b.BrandGroup,g.Forwarder,g.CutOffDate
from GMTBooking g WITH(NOLOCK)
inner join Brand b with(nolock) on b.ID = g.BrandID
inner join ShipMode s with(nolock) on s.id = g.ShipModeID 
where g.shipPlanID = '{this.ShipPlanID}'
and s.IncludeSeaShipping = 1
and g.CYCFS = 'CY-CY'";
                    DBProxy.Current.Select(null, strSelectSqlCmd, out selectDt);

                    Win.Tools.SelectItem selectItem = new Win.Tools.SelectItem(selectDt, "GB#,LoadingType", "20,10", this.CurrentData["ID"].ToString());
                    DialogResult result = selectItem.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    this.CurrentData["ID"] = selectItem.GetSelectedString();
                    this.CurrentData["CYCFS"] = selectItem.GetSelecteds()[0]["LoadingType"];
                    this.CurrentData["BrandGroup"] = selectItem.GetSelecteds()[0]["BrandGroup"];
                    this.CurrentData["Forwarder"] = selectItem.GetSelecteds()[0]["Forwarder"];
                    this.CurrentData["CutOffDate"] = selectItem.GetSelecteds()[0]["CutOffDate"];
                    this.CurrentData.EndEdit();
                }
            };

            id.CellValidating += (s, e) =>
            {
                if (!this.EditMode || MyUtility.Check.Empty(e.FormattedValue) || e.RowIndex == -1)
                {
                    return;
                }

                string chkgbid = $@"
select g.id,g.CYCFS ,b.BrandGroup,g.Forwarder,g.CutOffDate
from GMTBooking g WITH (NOLOCK) 
inner join Brand b with(nolock) on b.ID = g.BrandID
inner join ShipMode s with(nolock) on s.id = g.ShipModeID 
where g.shipPlanID = '{this.ShipPlanID}' and g.id = '{e.FormattedValue}'
and s.IncludeSeaShipping = 1
and g.CYCFS = 'CY-CY'
";
                DataRow dr;
                if (MyUtility.Check.Seek(chkgbid, out dr))
                {
                    this.CurrentData["ID"] = e.FormattedValue;
                    this.CurrentData["CYCFS"] = dr["CYCFS"];
                    this.CurrentData["BrandGroup"] = dr["BrandGroup"];
                    this.CurrentData["Forwarder"] = dr["Forwarder"];
                    this.CurrentData["CutOffDate"] = dr["CutOffDate"];
                }
                else
                {
                    MyUtility.Msg.WarningBox("No this GB#");
                    this.CurrentData["ID"] = string.Empty;
                }

                this.CurrentData.EndEdit();
            };

            this.Helper.Controls.Grid.Generator(this.grid)
            .ComboBox("Type", header: "Container Type", width: Widths.AnsiChars(20)).Get(out cbb_CYCFS)
            .Text("ID", header: "GB#", width: Widths.AnsiChars(20), settings: id)
            .Text("CYCFS", header: "Loading Type", iseditable: false)
            .Text("CTNRNo", header: "Container#", width: Widths.AnsiChars(20), settings: CTNRNo)
            .Text("SealNo", header: "Seal#", width: Widths.AnsiChars(15), settings: SealNo)
            .Text("TruckNo", header: "Truck#/Traile#", width: Widths.AnsiChars(20), settings: TruckNo)
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
            for (int i = dt.Rows.Count - 1; i >= 0; i--)
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
            #region 不同Container Type，Container #不得相同
            var sCTNRNo = dt.AsEnumerable().Where(w => w.RowState != DataRowState.Deleted).Select(s => new { CTNRNo = MyUtility.Convert.GetString(s["CTNRNo"]) }).Distinct().ToList();
            foreach (var item in sCTNRNo)
            {
                if (dt.AsEnumerable().Where(w => w.RowState != DataRowState.Deleted && MyUtility.Convert.GetString(w["CTNRNo"]) == item.CTNRNo).
                        Select(s => new { Type = MyUtility.Convert.GetString(s["Type"]) }).Distinct().Count() > 1)
                {
                    MyUtility.Msg.WarningBox("<Container#> can not be in different <Container Type>!");
                    return false;
                }
            }

            #endregion

            #region 若相同Brand, Forwarder, Loading Type, Cut-Off Date才能放在同一個Container#
            string inCTNRNo = "'" + string.Join("','", dt.AsEnumerable().Where(w => w.RowState != DataRowState.Deleted).Select(s => MyUtility.Convert.GetString(s["CTNRNo"]))) + "'";
            string inPkey = "'" + string.Join("','", dt.AsEnumerable().Where(w => w.RowState != DataRowState.Deleted).Select(s => MyUtility.Convert.GetString(s["id"]) + MyUtility.Convert.GetString(s["CTNRNo"]) + MyUtility.Convert.GetString(s["TruckNo"]))) + "'";
            string sqlchk = $@"
select distinct gc.CTNRNo,b.BrandGroup,g.Forwarder,g.CYCFS,g.CutOffDate
from GMTBooking_CTNR gc with(nolock)
inner join GMTBooking g with(nolock) on gc.id = g.id
inner join Brand b with(nolock) on b.ID = g.BrandID
where gc.CTNRNo in ({inCTNRNo})
and concat(gc.id, gc.CTNRNo,gc.TruckNo)not in ({inPkey})
";
            DataTable chkdt;
            DualResult result = DBProxy.Current.Select(null, sqlchk, out chkdt);
            if (!result)
            {
                this.ShowErr(result);
                return false;
            }

            foreach (DataRow dr in chkdt.Rows)
            {
                foreach (DataRow drs in dt.AsEnumerable().Where(w => w.RowState != DataRowState.Deleted && MyUtility.Convert.GetString(w["CTNRNo"]).EqualString(MyUtility.Convert.GetString(dr["CTNRNo"]))))
                {
                    if (!MyUtility.Convert.GetString(dr["BrandGroup"]).EqualString(MyUtility.Convert.GetString(drs["BrandGroup"])) ||
                        !MyUtility.Convert.GetString(dr["Forwarder"]).EqualString(MyUtility.Convert.GetString(drs["Forwarder"])) ||
                        !MyUtility.Convert.GetString(dr["CYCFS"]).EqualString(MyUtility.Convert.GetString(drs["CYCFS"])) ||
                        !MyUtility.Convert.GetString(dr["CutOffDate"]).EqualString(MyUtility.Convert.GetString(drs["CutOffDate"])))
                    {
                        MyUtility.Msg.WarningBox("GB# can be added to the same Container# only GBs with the same Brand、Forwarder、Loading Type and Cut-Off Date");
                        return false;
                    }
                }
            }

            foreach (DataRow dr in dt.AsEnumerable().Where(w => w.RowState != DataRowState.Deleted))
            {
                foreach (DataRow drs in dt.AsEnumerable().Where(w => w.RowState != DataRowState.Deleted && MyUtility.Convert.GetString(w["CTNRNo"]).EqualString(MyUtility.Convert.GetString(dr["CTNRNo"]))))
                {
                    if (!MyUtility.Convert.GetString(dr["BrandGroup"]).EqualString(MyUtility.Convert.GetString(drs["BrandGroup"])) ||
                        !MyUtility.Convert.GetString(dr["Forwarder"]).EqualString(MyUtility.Convert.GetString(drs["Forwarder"])) ||
                        !MyUtility.Convert.GetString(dr["CYCFS"]).EqualString(MyUtility.Convert.GetString(drs["CYCFS"])) ||
                        !MyUtility.Convert.GetString(dr["CutOffDate"]).EqualString(MyUtility.Convert.GetString(drs["CutOffDate"])))
                    {
                        MyUtility.Msg.WarningBox("GB# can be added to the same Container# only GBs with the same Brand、Forwarder、Loading Type and Cut-Off Date");
                        return false;
                    }
                }
            }
            #endregion
            return base.OnSaveBefore();
        }

        protected override void OnSaveAfter()
        {
            base.OnSaveAfter();
            this.reloadParant();
        }

        protected override void OnRequired()
        {
            base.OnRequired();
            DataTable datas;
            string sqlcmd = $@"
select gc.*,g.CYCFS,b.BrandGroup,g.Forwarder,g.CutOffDate
from GMTBooking_CTNR gc with(nolock)
inner join GMTBooking g with(nolock) on gc.id = g.id
inner join Brand b with(nolock) on b.ID = g.BrandID
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
