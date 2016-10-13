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
using System.Transactions;

namespace Sci.Production.Shipping
{
    public partial class P05 : Sci.Win.Tems.Input6
    {
        Ict.Win.UI.DataGridViewTextBoxColumn col_lock;
        Ict.Win.UI.DataGridViewDateBoxColumn col_crd;
        Ict.Win.DataGridViewGeneratorNumericColumnSettings shipqty = new Ict.Win.DataGridViewGeneratorNumericColumnSettings();
        private string masterID;
        private DataTable selectData;
        private DataRow dr;
        string dateTimeMask = "", emptyDTMask = "",empmask,dtmask;

        public P05(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            gridicon.Append.Visible = false;
            gridicon.Insert.Visible = false;
            detailgrid.AllowUserToOrderColumns = true;
            InsertDetailGridOnDoubleClick = false;
            //組Cut off date的mask
            for (int i = 0; i < Sci.Env.Cfg.DateTimeStringFormat.Length;i++)
            {
                dtmask = Sci.Env.Cfg.DateTimeStringFormat.Substring(i, 1) == "/" || Sci.Env.Cfg.DateTimeStringFormat.Substring(i, 1) == ":" ? Sci.Env.Cfg.DateTimeStringFormat.Substring(i, 1) : Sci.Env.Cfg.DateTimeStringFormat.Substring(i, 1) == " " ? " " : "0";
                empmask = Sci.Env.Cfg.DateTimeStringFormat.Substring(i, 1) == "/" || Sci.Env.Cfg.DateTimeStringFormat.Substring(i, 1) == ":" ? Sci.Env.Cfg.DateTimeStringFormat.Substring(i, 1) : Sci.Env.Cfg.DateTimeStringFormat.Substring(i, 1) == "s" ? "":" ";
                dateTimeMask = dateTimeMask + dtmask;
                emptyDTMask = emptyDTMask + empmask;
            }

            //this.textBox6.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "CutOffDate", true, System.Windows.Forms.DataSourceUpdateMode.OnValidation, emptyDTMask, Sci.Env.Cfg.DateTimeStringFormat));
            //this.textBox6.Mask = dateTimeMask;
        }

        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            masterID = (e.Master == null) ? "1=0" : string.Format("p.INVNo = '{0}'",MyUtility.Convert.GetString(e.Master["ID"]));
            this.DetailSelectCommand = string.Format(@"select p.GMTBookingLock,p.FactoryID,p.ID,
(select cast(a.OrderID as nvarchar) +',' from (select distinct OrderID from PackingList_Detail pd where pd.ID = p.id) a for xml path('')) as OrderID,
p.CargoReadyDate,(select oq.BuyerDelivery from (select top 1 OrderID, OrderShipmodeSeq from PackingList_Detail pd where pd.ID = p.ID) a, Order_QtyShip oq where a.OrderID = oq.Id and a.OrderShipmodeSeq = oq.Seq) as BuyerDelivery,
(select oq.SDPDate from (select top 1 OrderID, OrderShipmodeSeq from PackingList_Detail pd where pd.ID = p.ID) a, Order_QtyShip oq where a.OrderID = oq.Id and a.OrderShipmodeSeq = oq.Seq) as SDPDate,
p.PulloutDate,p.ShipQty,p.CTNQty,p.GW,p.CBM,p.InvNo,
(select o.CustCDID from Orders o, (select top 1 OrderID from PackingList_Detail pd where pd.ID = p.ID) a where o.ID = a.OrderID) as CustCDID,
(select o.Dest from Orders o, (select top 1 OrderID from PackingList_Detail pd where pd.ID = p.ID) a where o.ID = a.OrderID) as Dest,
p.NW,p.NNW,p.Status,(select sum(CTNQty) from PackingList_Detail pd where pd.ID = p.ID and pd.ReceiveDate is not null) as ClogCTNQty,p.InspDate,p.ShipModeID
from PackingList p
where {0}", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            MyUtility.Tool.SetupCombox(comboBox1, 1, 1, ",CY-CY,CFS-CY,CFS-CFS");
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();

            textBox7.Text = MyUtility.GetValue.Lookup("WhseNo", MyUtility.Convert.GetString(CurrentMaintain["ForwarderWhse_DetailUKey"]), "ForwarderWhse_Detail", "UKey");

            #region AirPP List按鈕變色
            if (!this.EditMode)
            {
                string sqlCmd = string.Format(@"select pd.ID
from PackingList p, PackingList_Detail pd, AirPP a
where p.INVNo = '{0}' and p.ID = pd.ID and a.OrderID = pd.OrderID and a.OrderShipmodeSeq = pd.OrderShipmodeSeq", MyUtility.Convert.GetString(CurrentMaintain["ID"]));

                if (MyUtility.Check.Seek(sqlCmd))
                {
                    button1.ForeColor = Color.Red;
                    button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
                }
                else
                {
                    button1.ForeColor = Color.Black;
                    button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
                }
            }
            #endregion

            #region S/O CFM按鈕權限
            if (!this.EditMode)
            {
                if (MyUtility.Convert.GetString(CurrentMaintain["Status"]) == "New")
                {
                    button4.Enabled = true;
                }
                else
                {
                    button4.Enabled = false;
                }
                if (MyUtility.Check.Empty(CurrentMaintain["SOCFMDate"]))
                {
                    button4.Text = "CFM";
                }
                else
                {
                    button4.Text = "Un CFM";
                }
            }
            #endregion
        }

        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();

            shipqty.EditingMouseDoubleClick += (s, e) =>
                {
                    if (e.Button == System.Windows.Forms.MouseButtons.Left)
                    {
                        if (e.RowIndex != -1)
                        {
                            dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                            Sci.Production.Shipping.P05_QtyBreakDown callNextForm = new Sci.Production.Shipping.P05_QtyBreakDown(CurrentMaintain);
                            callNextForm.Set(false, this.DetailDatas, this.CurrentDetailData);
                            callNextForm.ShowDialog(this);
                        }
                    }
                };


            Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("GMTBookingLock", header: "Lock", width: Widths.AnsiChars(1)).Get(out col_lock)
                .Text("FactoryID", header: "Factory#", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("ID", header: "Packing #", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("OrderID", header: "SP No.", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Date("CargoReadyDate", header: "Cargo Ready Date").Get(out col_crd)
                .Date("BuyerDelivery", header: "Delivery", iseditingreadonly: true)
                .Date("SDPDate", header: "SDP Date", iseditingreadonly: true)
                .Date("PulloutDate", header: "Pull out Date", iseditingreadonly: true)
                .Numeric("ShipQty", header: "Q'ty", iseditingreadonly: true, settings: shipqty)
                .Numeric("CTNQty", header: "CTN Q'ty", iseditingreadonly: true)
                .Numeric("GW", header: "G.W.", decimal_places: 3, iseditingreadonly: true)
                .Numeric("CBM", header: "CBM", decimal_places: 3, iseditingreadonly: true)
                .Text("CustCDID", header: "CustCD", width: Widths.AnsiChars(16), iseditingreadonly: true)
                .Text("Dest", header: "Destination", width: Widths.AnsiChars(2), iseditingreadonly: true)
                .Numeric("NW", header: "ttl N.W.", decimal_places: 3, iseditingreadonly: true)
                .Numeric("NNW", header: "ttl N.N.W.", decimal_places: 3, iseditingreadonly: true)
                .Text("Status", header: "Status", width: Widths.AnsiChars(9), iseditingreadonly: true)
                .Numeric("ClogCTNQty", header: "CTN in C-Logs", iseditingreadonly: true)
                .Date("InspDate", header: "Est. Inspection date", iseditingreadonly: true);

            #region 欄位值檢查
            detailgrid.CellValidating += (s, e) =>
                {
                    if (this.EditMode)
                    {
                        DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                        if (detailgrid.Columns[e.ColumnIndex].DataPropertyName == col_lock.DataPropertyName)
                        {
                            if (!MyUtility.Check.Empty(e.FormattedValue))
                            {
                                if (MyUtility.Convert.GetString(e.FormattedValue) != MyUtility.Convert.GetString(dr["GMTBookingLock"]))
                                {
                                    if (MyUtility.Convert.GetString(e.FormattedValue) != "Y")
                                    {
                                        MyUtility.Msg.WarningBox("It should be only 'Y' or ''!");
                                        dr["GMTBookingLock"] = "Y";
                                        e.Cancel = true;
                                        return;
                                    }
                                }
                            }
                            else
                            {
                                if (dr["GMTBookingLock"].ToString() == "Y")
                                {
                                    string sqlCmd = string.Format(@"select p.ID
from PackingList pl, Pullout p
where pl.ID = '{0}'
and pl.PulloutID = p.ID
and p.Status = 'Confirmed'", MyUtility.Convert.GetString(dr["ID"]));

                                    if (MyUtility.Check.Seek(sqlCmd))
                                    {
                                        MyUtility.Msg.WarningBox("Pullout report already confirmed, can't  unlock!");
                                        dr["GMTBookingLock"] = "Y";
                                        e.Cancel = true;
                                        return;
                                    }
                                }
                            }
                        }

                        if (detailgrid.Columns[e.ColumnIndex].DataPropertyName == col_crd.DataPropertyName)
                        {
                            if (!MyUtility.Check.Empty(e.FormattedValue))
                            {
                                if (Convert.ToDateTime(e.FormattedValue) > DateTime.Today.AddMonths(12) || Convert.ToDateTime(e.FormattedValue) < DateTime.Today.AddMonths(-12))
                                {
                                    MyUtility.Msg.WarningBox("< Cargo Ready Date > is invalid!!");
                                    dr["CargoReadyDate"] = null;
                                    e.Cancel = true;
                                    return;
                                }
                            }
                        }
                    }
                };
            #endregion
        }

        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["Status"] = "New";
            CurrentMaintain["Shipper"] = Sci.Env.User.Keyword;
            CurrentMaintain["InvDate"] = DateTime.Today;
            CurrentMaintain["Handle"] = Sci.Env.User.UserID;
            CurrentMaintain["ShipModeID"] = "SEA";
            CurrentMaintain["ShipTermID"] = "FOB";
        }

        protected override bool ClickEditBefore()
        {
            if (MyUtility.Convert.GetString(CurrentMaintain["Status"]) == "Confirmed")
            {
                MyUtility.Msg.WarningBox("This record is < Confirmed >, can't be modified!");
                return false;
            }

            return base.ClickEditBefore();
        }

        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            textBox1.ReadOnly = true;
            txtbrand1.ReadOnly = true;
            txtcountry1.TextBox1.ReadOnly = true;
            txtshipmode1.ReadOnly = true;

            if (!MyUtility.Check.Empty(CurrentMaintain["SOCFMDate"]))
            {
                dateBox1.ReadOnly = true;
                txtfactory1.ReadOnly = true;
                dateBox2.ReadOnly = true;
                textBox4.ReadOnly = true;
                txtuser1.TextBox1.ReadOnly = true;
                txtsubcon1.TextBox1.ReadOnly = true;
                comboBox1.ReadOnly = true;
                textBox3.ReadOnly = true;
                textBox7.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.EditModeAndNonReadOnly;
               // textBox6.ReadOnly = true;
                col_lock.IsEditingReadOnly = true;
                col_crd.IsEditingReadOnly = true;
                detailgrid.Columns[0].DefaultCellStyle.ForeColor = Color.Black;
                detailgrid.Columns[4].DefaultCellStyle.ForeColor = Color.Black;
                button6.Enabled = false;
                gridicon.Remove.Enabled = false;
            }
            else
            {
                gridicon.Remove.Enabled = true;
                col_lock.IsEditingReadOnly = false;
                col_crd.IsEditingReadOnly = false;
                detailgrid.Columns[0].DefaultCellStyle.ForeColor = Color.Red;
                detailgrid.Columns[4].DefaultCellStyle.ForeColor = Color.Red;
                textBox7.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.EditModeAndReadOnly;
            }
        }

        protected override bool ClickDeleteBefore()
        {
            if (MyUtility.Convert.GetString(CurrentMaintain["Status"]) == "Confirmed")
            {
                MyUtility.Msg.WarningBox("This record is < Confirmed >, can't be deleted!");
                return false;
            }

            if (!MyUtility.Check.Empty(CurrentMaintain["SOCFMDate"]))
            {
                MyUtility.Msg.WarningBox("This record already S/O Confirmed, can't be deleted!");
                return false;
            }

            //已經有做出口費用分攤就不可以被刪除
            if (MyUtility.Check.Seek(string.Format(@"select ShippingAPID from ShareExpense where InvNo = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ID"]))))
            {
                MyUtility.Msg.WarningBox("This record have expense data, can't be deleted!");
                return false;
            }

            //只要Pullout Report已經Confirmed就不可以被刪除
            string sqlCmd = string.Format(@"select distinct pl.PulloutID
from PackingList pl, Pullout p
where pl.INVNo = '{0}'
and pl.PulloutID = p.ID
and p.Status = 'Confirmed'", MyUtility.Convert.GetString(CurrentMaintain["ID"]));
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out selectData);
            if (result)
            {
                if (selectData.Rows.Count > 0)
                {
                    MyUtility.Msg.WarningBox("Pullout report already confirmed, can't be deleted!");
                    return false;
                }
            }
            else
            {
                MyUtility.Msg.ErrorBox(result.ToString());
                return false;
            }

            return base.ClickDeleteBefore();
        }

        protected override DualResult OnDeleteDetails()
        {
            IList<string> updateCmd = new List<string>();
            updateCmd.Add(string.Format("update PackingList set GMTBookingLock = '', INVNo = '', ShipPlanID = '' where INVNo = '{0}';", MyUtility.Convert.GetString(CurrentMaintain["ID"])));
            updateCmd.Add(string.Format("Delete GMTBooking_CTNR where ID = '{0}'",MyUtility.Convert.GetString(CurrentMaintain["ID"])));
            updateCmd.Add(string.Format("Delete GMTBooking_History where ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ID"])));
            
            DualResult result = DBProxy.Current.Executes(null, updateCmd);
            return result;
        }

        protected override bool ClickSaveBefore()
        {
            #region 檢查必輸欄位
            if (MyUtility.Check.Empty(CurrentMaintain["InvSerial"]))
            {
                MyUtility.Msg.WarningBox("Inv. Serial can't empty!!");
                textBox1.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["InvDate"]))
            {
                MyUtility.Msg.WarningBox("Inv. Date can't empty!!");
                dateBox1.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["Shipper"]))
            {
                MyUtility.Msg.WarningBox("Shipper can't empty!!");
                txtfactory1.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["BrandID"]))
            {
                MyUtility.Msg.WarningBox("Brand can't empty!!");
                txtbrand1.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["FCRDate"]))
            {
                MyUtility.Msg.WarningBox("FCR Date can't empty!!");
                dateBox2.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["CustCDID"]))
            {
                MyUtility.Msg.WarningBox("CustCD can't empty!!");
                textBox4.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["Dest"]))
            {
                MyUtility.Msg.WarningBox("Destination can't empty!!");
                txtcountry1.TextBox1.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["PayTermARID"]))
            {
                MyUtility.Msg.WarningBox("Payment Term can't empty!!");
                txtpaytermar1.TextBox1.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["ShipModeID"]))
            {
                MyUtility.Msg.WarningBox("Shipping Mode can't empty!!");
                txtshipmode1.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["ShipTermID"]))
            {
                MyUtility.Msg.WarningBox("Shipment Term can't empty!!");
                txtshipterm1.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["Handle"]))
            {
                MyUtility.Msg.WarningBox("Handle can't empty!!");
                txtuser1.TextBox1.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["Forwarder"]))
            {
                MyUtility.Msg.WarningBox("Forwarder can't empty!!");
                txtsubcon1.TextBox1.Focus();
                return false;
            }
            #endregion
            
            //新增單狀態下，取ID且檢查此ID是否存在
            if (IsDetailInserting)
            {
                string newID = MyUtility.GetValue.Lookup("NegoRegion", MyUtility.Convert.GetString(CurrentMaintain["Shipper"]), "Factory", "ID").Trim() + Convert.ToDateTime(CurrentMaintain["InvDate"]).ToString("yyMM") + "-" + MyUtility.Convert.GetString(CurrentMaintain["InvSerial"]).Trim() + "-" + MyUtility.GetValue.Lookup("ShipCode",MyUtility.Convert.GetString(CurrentMaintain["BrandID"]),"Brand","ID").Trim();
                if (MyUtility.Check.Seek(newID, "GMTBooking", "ID"))
                {
                    MyUtility.Msg.WarningBox("Inv. Serial already exist!!");
                    textBox1.Focus();
                    return false;
                }
                CurrentMaintain["ID"] = newID;
            }

            //組出表身所有的PackingListID與加總ShipQty,CTNQty,NW,GW,NNW,CBM
            StringBuilder allPackID = new StringBuilder();
            int ttlshipqty = 0, ttlctnqty = 0;
            double ttlnw = 0.0, ttlgw = 0.0, ttlnnw = 0.0, ttlcbm = 0.0;
            foreach (DataRow dr in DetailDatas)
            {
                allPackID.Append(string.Format("'{0}',",MyUtility.Convert.GetString(dr["ID"])));
                ttlshipqty = ttlshipqty + MyUtility.Convert.GetInt(dr["ShipQty"]);
                ttlctnqty = ttlctnqty + MyUtility.Convert.GetInt(dr["CTNQty"]);
                ttlnw = MyUtility.Math.Round(ttlnw + MyUtility.Convert.GetDouble(dr["NW"]), 3);
                ttlgw = MyUtility.Math.Round(ttlgw + MyUtility.Convert.GetDouble(dr["GW"]), 3);
                ttlnnw = MyUtility.Math.Round(ttlnnw + MyUtility.Convert.GetDouble(dr["NNW"]), 3);
                ttlcbm = MyUtility.Math.Round(ttlcbm + MyUtility.Convert.GetDouble(dr["CBM"]), 3);
            }
            #region 檢查訂單的Currency是否一致與Payterm與表頭是否一致
            if (allPackID.Length > 0)
            {
                string sqlCmd = string.Format(@"with OrderData
as
(select distinct pd.ID,pd.OrderID,o.CurrencyID,o.PayTermARID
 from PackingList_Detail pd, Orders o
 where pd.ID in ({0})
 and pd.OrderID = o.ID
 ),
CurrencyCount
as
(select CurrencyID, isnull(COUNT(CurrencyID),0) as CNT
 from OrderData
 group by CurrencyID
)
Select od.ID,od.OrderID,od.CurrencyID,od.PayTermARID,cc.CNT
from OrderData od
left join CurrencyCount cc on od.CurrencyID = cc.CurrencyID
order by cc.CNT desc", allPackID.ToString().Substring(0, allPackID.Length - 1));
                DualResult result = DBProxy.Current.Select(null, sqlCmd, out selectData);
                if (!result)
                {
                    MyUtility.Msg.ErrorBox(result.ToString());
                    return false;
                }
                if (selectData.Rows.Count > 0)
                {
                    StringBuilder msg = new StringBuilder();
                    foreach (DataRow dr in selectData.Select(string.Format("CurrencyID <> '{0}'", MyUtility.Convert.GetString(selectData.Rows[0]["CurrencyID"]))))
                    {
                        msg.Append(string.Format("Packing#:{0},   SP No.:{1},   Currency:{2}\r\n", MyUtility.Convert.GetString(dr["ID"]), MyUtility.Convert.GetString(dr["OrderID"]), MyUtility.Convert.GetString(dr["CurrencyID"])));
                    }
                    if (msg.Length > 0)
                    {
                        MyUtility.Msg.WarningBox("Currency is inconsistent!!\r\nThe currency of most SP No. is " + MyUtility.Convert.GetString(selectData.Rows[0]["CurrencyID"]) + "\r\n" + msg.ToString());
                        return false;
                    }

                    msg.Clear();
                    foreach (DataRow dr in selectData.Select(string.Format("PayTermARID <> '{0}'", MyUtility.Convert.GetString(CurrentMaintain["PayTermARID"]))))
                    {
                        msg.Append(string.Format("Packing#:{0},   Payment Term:{1}\r\n", MyUtility.Convert.GetString(dr["ID"]),MyUtility.Convert.GetString(dr["PayTermARID"])));
                    }
                    if (msg.Length > 0)
                    {
                        MyUtility.Msg.WarningBox("Payment term in detail SP is different from garment booking!!\r\n"+ msg.ToString());
                    }
                }
            }
            #endregion

            #region 組Description
            string season = "", category = "";
            if (allPackID.Length > 0)
            {
                string sqlCmd = string.Format(@"with OrderData
as
(select distinct o.Category,o.SeasonID
 from PackingList_Detail pd, Orders o
 where pd.ID in ({0})
 and pd.OrderID = o.ID
)
select (select CAST(a.Category as nvarchar)+'/' from (select distinct Category from OrderData) a for xml path('')) as Category,
(select CAST(a.SeasonID as nvarchar)+'/' from (select distinct SeasonID from OrderData) a for xml path('')) as Season", allPackID.ToString().Substring(0, allPackID.Length - 1));
                DualResult result = DBProxy.Current.Select(null, sqlCmd, out selectData);
                if (!result)
                {
                    MyUtility.Msg.ErrorBox(result.ToString());
                    return false;
                }
                if (selectData.Rows.Count > 0)
                {
                    season = MyUtility.Convert.GetString(selectData.Rows[0]["Season"]).Substring(0, MyUtility.Convert.GetString(selectData.Rows[0]["Season"]).Length-1);
                    category = MyUtility.Convert.GetString(selectData.Rows[0]["Category"]).Substring(0, MyUtility.Convert.GetString(selectData.Rows[0]["Category"]).Length - 1);
                }
            }
            CurrentMaintain["Description"] = MyUtility.Convert.GetString(CurrentMaintain["BrandID"]) + ',' + season + ',' + MyUtility.Convert.GetString(CurrentMaintain["CustCDID"]) + "," + MyUtility.Convert.GetString(CurrentMaintain["Dest"]) + "," + category;
            #endregion

            //將表身加總的資料回寫回表頭
            CurrentMaintain["TotalShipQty"] = ttlshipqty;
            CurrentMaintain["TotalCTNQty"] = ttlctnqty;
            CurrentMaintain["TotalNW"] = MyUtility.Math.Round(ttlnw,2);
            CurrentMaintain["TotalGW"] = MyUtility.Math.Round(ttlgw, 2);
            CurrentMaintain["TotalNNW"] = MyUtility.Math.Round(ttlnnw, 2);
            CurrentMaintain["TotalCBM"] = MyUtility.Math.Round(ttlcbm, 2);

            return base.ClickSaveBefore();
        }

        protected override DualResult OnSaveDetail(IList<DataRow> details, ITableSchema detailtableschema)
        {
            IList<string> updateCmds = new List<string>();

            foreach (DataRow dr in details)
            {
                if (dr.RowState == DataRowState.Modified)
                {
                    updateCmds.Add(string.Format("update PackingList set GMTBookingLock = '{0}' where ID = '{1}';", MyUtility.Convert.GetString(dr["GMTBookingLock"]), MyUtility.Convert.GetString(dr["ID"])));
                }
                if (dr.RowState == DataRowState.Added)
                {
                    updateCmds.Add(string.Format("update PackingList set GMTBookingLock = '{0}', INVNo = '{1}', ShipPlanID = '{2}' where ID = '{3}';", MyUtility.Convert.GetString(dr["GMTBookingLock"]), MyUtility.Convert.GetString(CurrentMaintain["ID"]), MyUtility.Convert.GetString(CurrentMaintain["ShipPlanID"]), MyUtility.Convert.GetString(dr["ID"])));
                }
                if (dr.RowState == DataRowState.Deleted)
                {
                    updateCmds.Add(string.Format("update PackingList set GMTBookingLock = '', INVNo = '', ShipPlanID = '' where ID = '{0}';", MyUtility.Convert.GetString(dr["ID", DataRowVersion.Original])));
                }
            }
            if (updateCmds.Count != 0)
            {
                DualResult result = DBProxy.Current.Executes(null, updateCmds);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Update OackingList fail\r\n" + result.ToString());
                    return failResult;
                }
            }
            return Result.True;
        }

        protected override bool ClickPrint()
        {
            string strXltName = Sci.Env.Cfg.XltPathDir + "\\Shipping_P05.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null) return false;
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];
            worksheet.Cells[1, 1] = MyUtility.Convert.GetString(CurrentMaintain["Shipper"]);
            worksheet.Cells[3, 2] = MyUtility.Convert.GetString(CurrentMaintain["ID"]);
            worksheet.Cells[4, 2] = MyUtility.Convert.GetString(CurrentMaintain["InvSerial"]);
            worksheet.Cells[5, 2] = MyUtility.Check.Empty(CurrentMaintain["InvDate"]) ? "" : Convert.ToDateTime(CurrentMaintain["InvDate"]).ToString("d");
            worksheet.Cells[6, 2] = MyUtility.Convert.GetString(CurrentMaintain["Shipper"]);
            worksheet.Cells[7, 2] = MyUtility.Convert.GetString(CurrentMaintain["BrandID"]);
            worksheet.Cells[8, 2] = MyUtility.Check.Empty(CurrentMaintain["FCRDate"]) ? "" : Convert.ToDateTime(CurrentMaintain["FCRDate"]).ToString("d");
            worksheet.Cells[9, 2] = MyUtility.Convert.GetString(CurrentMaintain["CustCDID"]);
            worksheet.Cells[10, 2] = MyUtility.Convert.GetString(CurrentMaintain["PayTermARID"]) + " - " + MyUtility.GetValue.Lookup(string.Format("select Description from PayTermAR where ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["PayTermARID"])));
            worksheet.Cells[11, 2] = MyUtility.Convert.GetString(CurrentMaintain["Description"]);
            worksheet.Cells[12, 2] = MyUtility.Convert.GetString(CurrentMaintain["Remark"]);

            worksheet.Cells[3, 5] = MyUtility.Convert.GetString(CurrentMaintain["Dest"]) + " - " + MyUtility.GetValue.Lookup(string.Format("select NameEN from Country where ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["Dest"])));
            worksheet.Cells[4, 5] = MyUtility.Convert.GetString(CurrentMaintain["ShipModeID"]);
            worksheet.Cells[5, 5] = MyUtility.Convert.GetString(CurrentMaintain["ShipTermID"]);
            worksheet.Cells[6, 5] = MyUtility.Convert.GetInt(CurrentMaintain["TotalShipQty"]);
            worksheet.Cells[7, 5] = MyUtility.Convert.GetInt(CurrentMaintain["TotalCTNQty"]);
            worksheet.Cells[8, 5] = MyUtility.Convert.GetDecimal(CurrentMaintain["TotalGW"]);
            worksheet.Cells[9, 5] = MyUtility.Convert.GetDecimal(CurrentMaintain["TotalCBM"]);
            worksheet.Cells[10, 5] = MyUtility.Convert.GetDecimal(CurrentMaintain["TotalNW"]);
            worksheet.Cells[11, 5] = MyUtility.Convert.GetDecimal(CurrentMaintain["TotalNNW"]);

            worksheet.Cells[3, 9] = MyUtility.Convert.GetString(CurrentMaintain["Handle"]) + "  " + MyUtility.GetValue.Lookup(string.Format("select Name from Pass1 where ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["Handle"])));
            worksheet.Cells[4, 9] = MyUtility.Convert.GetString(CurrentMaintain["Forwarder"]) + "  " + MyUtility.GetValue.Lookup(string.Format("select Name from LocalSupp where ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["Forwarder"])));
            worksheet.Cells[5, 9] = MyUtility.Convert.GetString(CurrentMaintain["CYCFS"]);
            worksheet.Cells[6, 9] = MyUtility.Convert.GetString(CurrentMaintain["SONo"]);
            worksheet.Cells[7, 9] = MyUtility.GetValue.Lookup("WhseNo", MyUtility.Convert.GetString(CurrentMaintain["ForwarderWhse_DetailUKey"]), "ForwarderWhse_Detail", "UKey");
            worksheet.Cells[8, 9] = MyUtility.Check.Empty(CurrentMaintain["CutOffDate"]) ? "" : Convert.ToDateTime(CurrentMaintain["CutOffDate"]).ToString("yyyy/MM/dd HH:mm:ss");
            worksheet.Cells[9, 9] = MyUtility.Check.Empty(CurrentMaintain["SOCFMDate"]) ? "" : Convert.ToDateTime(CurrentMaintain["SOCFMDate"]).ToString("d");
            worksheet.Cells[10, 9] = MyUtility.Convert.GetString(CurrentMaintain["Vessel"]);
            worksheet.Cells[11, 9] = MyUtility.Check.Empty(CurrentMaintain["ETD"]) ? "" : Convert.ToDateTime(CurrentMaintain["ETD"]).ToString("d");
            worksheet.Cells[12, 9] = MyUtility.Check.Empty(CurrentMaintain["ETA"]) ? "" : Convert.ToDateTime(CurrentMaintain["ETA"]).ToString("d");

            int intRowsStart = 14;
            DataTable GridData = (DataTable)detailgridbs.DataSource;
            int dataRowCount = GridData.Rows.Count;
            object[,] objArray = new object[1, 13];
            for (int i = 0; i < dataRowCount; i++)
            {
                DataRow dr = GridData.Rows[i];
                int rownum = intRowsStart + i;
                objArray[0, 0] = dr["FactoryID"];
                objArray[0, 1] = dr["ID"];
                objArray[0, 2] = dr["OrderID"];
                objArray[0, 3] = dr["CargoReadyDate"];
                objArray[0, 4] = dr["BuyerDelivery"];
                objArray[0, 5] = dr["SDPDate"];
                objArray[0, 6] = dr["PulloutDate"];
                objArray[0, 7] = dr["ShipQty"];
                objArray[0, 8] = dr["CTNQty"];
                objArray[0, 9] = dr["GW"];
                objArray[0, 10] = dr["CBM"];
                objArray[0, 11] = dr["NW"];
                objArray[0, 12] = dr["NNW"];
                worksheet.Range[String.Format("A{0}:M{0}", rownum)].Value2 = objArray;
            }
            excel.Visible = true;
            
            return base.ClickPrint();
        }

        //Inv. Serial:移除空白值
        private void textBox1_Validated(object sender, EventArgs e)
        {
            CurrentMaintain["InvSerial"] = MyUtility.Convert.GetString(CurrentMaintain["InvSerial"]).Replace(" ", "");
        }

        //檢查輸入的Inv. Date是否正確
        private void dateBox1_Validating(object sender, CancelEventArgs e)
        {
            if (this.EditMode && !MyUtility.Check.Empty(dateBox1.Value))
            {
                if (dateBox1.Value > DateTime.Today.AddDays(180) || dateBox1.Value < DateTime.Today.AddDays(-180))
                {
                    MyUtility.Msg.WarningBox("< Inv. Date > is invalid, it exceeds +/-180 days!!");
                    dateBox1.Value = null;
                    e.Cancel = true;
                    return;
                }
            }
        }

        //輸入Brand後自動帶出Payment Term
        private void txtbrand1_Validated(object sender, EventArgs e)
        {
            if (this.EditMode && txtbrand1.OldValue != txtbrand1.Text)
            {
                GetPaytermAP();
            }
        }

        //檢查輸入的FCR Date是否正確
        private void dateBox2_Validating(object sender, CancelEventArgs e)
        {
            if (this.EditMode && !MyUtility.Check.Empty(dateBox2.Value))
            {
                if (!CheckDate((DateTime)MyUtility.Convert.GetDate(dateBox2.Value), -12, 12))
                {
                    MyUtility.Msg.WarningBox("< FCR Date > is invalid!!");
                    dateBox2.Value = null;
                    e.Cancel = true;
                    return;
                }

                //新增單時，自動將FCR Date寫入Inv. Date欄位
                if (MyUtility.Check.Empty(CurrentMaintain["ID"]))
                {
                    CurrentMaintain["FCRDate"] = dateBox2.Value;
                    CurrentMaintain["InvDate"] = dateBox2.Value;
                }
            }
        }

        //CustCD按右鍵
        private void textBox4_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            if (this.EditMode)
            {
                Sci.Win.Tools.SelectItem item;
                if (MyUtility.Check.Empty(CurrentMaintain["ID"]))
                {
                    item = new Sci.Win.Tools.SelectItem(string.Format("select ID, CountryID, City from CustCD where BrandID = '{0}' order by ID", MyUtility.Convert.GetString(CurrentMaintain["BrandID"])), "17,3,17", textBox4.Text);
                    DialogResult returnResult = item.ShowDialog();
                    if (returnResult == DialogResult.Cancel) { return; }
                    textBox4.Text = item.GetSelectedString();
                }
                else
                {
                    item = new Sci.Win.Tools.SelectItem(string.Format("select ID, CountryID, City from CustCD where BrandID = '{0}' and CountryID = '{1}' order by ID", MyUtility.Convert.GetString(CurrentMaintain["BrandID"]), MyUtility.Convert.GetString(CurrentMaintain["Dest"])), "17,3,17", textBox4.Text);
                    DialogResult returnResult = item.ShowDialog();
                    if (returnResult == DialogResult.Cancel) { return; }
                    textBox4.Text = item.GetSelectedString();
                }
            }
        }

        //檢查輸入的CustCD是否正確
        private void textBox4_Validating(object sender, CancelEventArgs e)
        {
            if (this.EditMode && !MyUtility.Check.Empty(textBox4.Text) && textBox4.OldValue != textBox4.Text)
            {
                if (!MyUtility.Check.Empty(CurrentMaintain["BrandID"]))
                {
                    //sql參數
                    System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter("@brandid", MyUtility.Convert.GetString(CurrentMaintain["BrandID"]));
                    System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter("@custcdid", textBox4.Text);

                    IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                    cmds.Add(sp1);
                    cmds.Add(sp2);

                    DataTable CustCDData;
                    string sqlCmd = "select ID, CountryID, City from CustCD where BrandID = @brandid and ID = @custcdid order by ID";
                    DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out CustCDData);

                    if (!result || CustCDData.Rows.Count <= 0)
                    {
                        if (!result)
                        {
                            MyUtility.Msg.WarningBox("Sql connection fail!!\r\n"+result.ToString());
                        }
                        else
                        {
                            MyUtility.Msg.WarningBox(string.Format("< CustCD: {0} > not found!!!", textBox4.Text));
                        }
                        textBox4.Text = "";
                        e.Cancel = true;
                        return;
                    }
                    else
                    {
                        CurrentMaintain["CustCDID"] = textBox4.Text;
                        GetPaytermAP();
                    }
                }
            }
        }

        //自動帶出PaytermARID
        private void GetPaytermAP()
        {
            if (MyUtility.Check.Empty(txtbrand1.Text))
            {
                txtpaytermar1.TextBox1.Text = "";
            }
            else
            {
                string paytermAR = "";
                if (MyUtility.Check.Empty(textBox4.Text))
                {
                    paytermAR = MyUtility.GetValue.Lookup("PayTermARIDBulk", txtbrand1.Text, "Brand", "ID");
                }
                else
                {
                    paytermAR = MyUtility.GetValue.Lookup(string.Format("select PayTermARIDBulk from CustCD where BrandID = '{0}' and ID = '{1}'", txtbrand1.Text, textBox4.Text));
                }

                if (paytermAR != "")
                {
                    CurrentMaintain["PayTermARID"] = paytermAR;
                }
            }
        }

        //檢查輸入的Cut-off Date是否正確
        //private void textBox6_Validating(object sender, CancelEventArgs e)
        //{
        //    if (this.EditMode && textBox6.Text != emptyDTMask)
        //    {
        //        string cutOffDate = textBox6.Text.Substring(0, 10).Replace(" ","1");
        //        if (!CheckDate((DateTime)MyUtility.Convert.GetDate(cutOffDate), -12, 12))
        //        {
        //            MyUtility.Msg.WarningBox("< Cut-off Date > is invalid!!");
        //            textBox6.Text = null;
        //            e.Cancel = true;
        //            return;
        //        }
        //    }
        //}

        //檢查輸入的ETD是否正確

        private void dateBox5_Validating(object sender, CancelEventArgs e)
        {
            if (this.EditMode && !MyUtility.Check.Empty(dateBox5.Value))
            {
                if (!CheckDate((DateTime)MyUtility.Convert.GetDate(dateBox5.Value), -12, 12))
                {
                    MyUtility.Msg.WarningBox("< ETD > is invalid!!");
                    dateBox5.Value = null;
                    e.Cancel = true;
                    return;
                }
            }
        }

        //檢查輸入的ETA是否正確
        private void dateBox6_Validating(object sender, CancelEventArgs e)
        {
            if (this.EditMode && !MyUtility.Check.Empty(dateBox6.Value))
            {
                if (!CheckDate((DateTime)MyUtility.Convert.GetDate(dateBox6.Value), -12, 12))
                {
                    MyUtility.Msg.WarningBox("< ETA > is invalid!!");
                    dateBox6.Value = null;
                    e.Cancel = true;
                    return;
                }
            }
        }

        private bool CheckDate(DateTime DT, int Before, int After)
        {
            if (DT > DateTime.Today.AddMonths(After) || DT < DateTime.Today.AddMonths(Before))
            {
                return false;
            }
            return true;
        }

        //表身Grid的Delete
        protected override void OnDetailGridDelete()
        {
            //檢查此筆記錄的Pullout Report是否已經Confirmed，若是則出訊息告知且無法刪除
            if (this.DetailDatas.Count > 0)
            {
                if (MyUtility.GetValue.Lookup(string.Format("select p.Status from Pullout p, PackingList pl where pl.ID = '{0}' and p.ID = pl.PulloutID", MyUtility.Convert.GetString(CurrentDetailData["ID"]))) == "Confirmed")
                {
                    MyUtility.Msg.WarningBox("Pullout report already confirmed, can't be deleted!");
                    return;
                }
            }
            base.OnDetailGridDelete();
        }

        //AirPP List
        private void button1_Click(object sender, EventArgs e)
        {
            Sci.Production.Shipping.P05_AirPrePaidList callNextForm = new Sci.Production.Shipping.P05_AirPrePaidList(MyUtility.Convert.GetString(CurrentMaintain["ID"]));
            callNextForm.ShowDialog(this);
        }

        //Expense Data
        private void button2_Click(object sender, EventArgs e)
        {
            Sci.Production.Shipping.P05_ExpenseData callNextForm = new Sci.Production.Shipping.P05_ExpenseData(MyUtility.Convert.GetString(CurrentMaintain["ID"]), "InvNo");
            callNextForm.ShowDialog(this);
        }

        // S/O Confirm History
        private void button5_Click(object sender, EventArgs e)
        {
            Sci.Win.UI.ShowHistory callNextForm = new Sci.Win.UI.ShowHistory("GMTBooking_History", MyUtility.Convert.GetString(CurrentMaintain["ID"]), "SOCFMDate", reasonType: "GMTBooking_SO", caption: "S/O Revised History");
            callNextForm.ShowDialog(this);
        }

        // S/O Confirm/UnConfirm
        private void button4_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(CurrentMaintain["SOCFMDate"]))
            {
                #region Confirm
                if (MyUtility.Check.Empty(CurrentMaintain["SONo"]) || MyUtility.Check.Empty(CurrentMaintain["ForwarderWhse_DetailUKey"]) || MyUtility.Check.Empty(CurrentMaintain["CutOffDate"]))
                {
                    MyUtility.Msg.WarningBox("< S/O # > , < Terminal/Whse# > and < Cut-off Date > can't be empty!!");
                    return;
                }

                //檢查表身的ShipMode與表頭的ShipMode如果不同就不可以Confirm
                if (!CheckShipMode())
                {
                    return;
                }

                bool firstCFM = !MyUtility.Check.Seek(string.Format("select ID from GMTBooking_History where ID = '{0}' and HisType = '{1}'", MyUtility.Convert.GetString(CurrentMaintain["ID"]), "SOCFMDate"));
                string insertCmd = string.Format(@"insert into GMTBooking_History (ID,HisType,OldValue,NewValue,AddName,AddDate)
values ('{0}','{1}','{2}','{3}','{4}',GETDATE())", MyUtility.Convert.GetString(CurrentMaintain["ID"]), "SOCFMDate", firstCFM ? "" : "CFM", "Un CFM", Sci.Env.User.UserID);
                string updateCmd = string.Format(@"update GMTBooking set SOCFMDate = '{0}' where ID = '{1}';
update PackingList set GMTBookingLock = 'Y' where INVNo = '{1}';", DateTime.Today.ToString("d"), MyUtility.Convert.GetString(CurrentMaintain["ID"]));
                using (TransactionScope transactionScope = new TransactionScope())
                {
                    try
                    {
                        DualResult result = DBProxy.Current.Execute(null, insertCmd);
                        DualResult result2 = DBProxy.Current.Execute(null, updateCmd);

                        if (result && result2)
                        {
                            transactionScope.Complete();
                        }
                        else
                        {
                            transactionScope.Dispose();
                            MyUtility.Msg.WarningBox("Confirm failed, Pleaes re-try");
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        transactionScope.Dispose();
                        ShowErr("Commit transaction error.", ex);
                        return;
                    }
                }
                #endregion
            }
            else
            {
                #region UnConfirm
                if (MyUtility.GetValue.Lookup("Status", MyUtility.Convert.GetString(CurrentMaintain["ShipPlanID"]), "ShipPlan", "ID") == "Confirmed")
                {
                    MyUtility.Msg.WarningBox("Ship Plan already confirmed, can't Un CFM!!");
                    return;
                }

                Sci.Win.UI.SelectReason callReason = new Sci.Win.UI.SelectReason("GMTBooking_SO", true);
                DialogResult dResult = callReason.ShowDialog(this);
                if (dResult == System.Windows.Forms.DialogResult.OK)
                {
                    string insertCmd = string.Format(@"insert into GMTBooking_History (ID,HisType,OldValue,NewValue,ReasonID,Remark,AddName,AddDate)
values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}',GETDATE())", MyUtility.Convert.GetString(CurrentMaintain["ID"]), "SOCFMDate", "Un CFM", "CFM", callReason.ReturnReason, callReason.ReturnRemark, Sci.Env.User.UserID);
                    string updateCmd = string.Format(@"update GMTBooking set SOCFMDate = null where ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ID"]));

                    using (TransactionScope transactionScope = new TransactionScope())
                    {
                        try
                        {
                            DualResult result = DBProxy.Current.Execute(null, insertCmd);
                            DualResult result2 = DBProxy.Current.Execute(null, updateCmd);

                            if (result && result2)
                            {
                                transactionScope.Complete();
                            }
                            else
                            {
                                transactionScope.Dispose();
                                MyUtility.Msg.WarningBox("UnConfirm failed, Pleaes re-try");
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            transactionScope.Dispose();
                            ShowErr("Commit transaction error.", ex);
                            return;
                        }
                    }
                }
                #endregion
            }
            RenewData();
            OnDetailEntered();
        }

        //檢查表身的ShipMode與表頭的ShipMode要相同
        private bool CheckShipMode()
        {
            StringBuilder msg = new StringBuilder();
            foreach (DataRow dr in ((DataTable)detailgridbs.DataSource).Select(string.Format("ShipModeID <> '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ShipModeID"]))))
            {
                msg.Append(string.Format("Packing#:{0},   Shipping Mode:{1}\r\n", MyUtility.Convert.GetString(dr["ID"]), MyUtility.Convert.GetString(dr["ShipModeID"])));
            }
            if (msg.Length > 0)
            {
                MyUtility.Msg.WarningBox("Shipping mode is inconsistent!!\r\n" + msg.ToString());
                return false;
            }
            return true;
        }

        //Container/Truck
        private void button3_Click(object sender, EventArgs e)
        {
            Sci.Production.Shipping.P05_ContainerTruck callNextForm = new Sci.Production.Shipping.P05_ContainerTruck(this.IsSupportEdit, MyUtility.Convert.GetString(CurrentMaintain["ID"]), null, null);
            callNextForm.ShowDialog(this);
        }

        //Import from packing list
        private void button6_Click(object sender, EventArgs e)
        {
            //Brand, CustCD, Destination, Ship Mode不可以為空
            if (MyUtility.Check.Empty(CurrentMaintain["BrandID"]))
            {
                MyUtility.Msg.WarningBox("< Brand > can't empty!");
                txtbrand1.Focus();
                return;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["CustCDID"]))
            {
                MyUtility.Msg.WarningBox("< CustCD > can't empty!");
                textBox4.Focus();
                return;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["Dest"]))
            {
                MyUtility.Msg.WarningBox("< Destination > can't empty!");
                txtcountry1.TextBox1.Focus();
                return;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["ShipModeID"]))
            {
                MyUtility.Msg.WarningBox("< Shipping Mode > can't empty!");
                txtshipmode1.Focus();
                return;
            }

            Sci.Production.Shipping.P05_ImportFromPackingList callNextForm = new Sci.Production.Shipping.P05_ImportFromPackingList(CurrentMaintain, (DataTable)detailgridbs.DataSource);
            callNextForm.ShowDialog(this);
        }

        //Confirm
        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            //還沒做S/O CFM的話，不可以Confrim
            if (MyUtility.Check.Empty(CurrentMaintain["SOCFMDate"]))
            {
                MyUtility.Msg.WarningBox("S/O not yet confirmed, can't confirm!");
                return;
            }

            //Inv. Date不可晚於FCR Date
            if (Convert.ToDateTime(CurrentMaintain["InvDate"]) > Convert.ToDateTime(CurrentMaintain["FCRDate"]))
            {
                MyUtility.Msg.WarningBox("< Inv. Date > can't exceed < FCR Date >!");
                return;
            }

            //檢查表身的ShipMode與表頭的ShipMode如果不同就不可以Confirm
            if (!CheckShipMode())
            {
                return;
            }

            //當ShipMode為A/P,A/P-C,E/P,S-A/P時，要檢查是否都有AirPP單號
            if (MyUtility.Check.Seek(string.Format("select ID from ShipMode where UseFunction like '%AirPP%' and ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ShipModeID"]))))
            {
                string sqlCmd = string.Format(@"select p.OrderID, p.OrderShipmodeSeq, isnull(a.ID,'') as AirPPID
from (Select distinct b.OrderID,b.OrderShipmodeSeq 
      from PackingList a, PackingList_Detail b 
      where a.INVNo = '{0}' and a.ID = b.ID) p
left join AirPP a on p.OrderID = a.OrderID and p.OrderShipmodeSeq = a.OrderShipmodeSeq and a.Status != 'Junked'",MyUtility.Convert.GetString(CurrentMaintain["ID"]));
                DualResult result = DBProxy.Current.Select(null, sqlCmd, out selectData);
                if (result)
                {
                    DataRow[] row = selectData.Select("AirPPID = ''");
                    if (row.Length > 0)
                    {
                        MyUtility.Msg.WarningBox("Still have not yet assigned AirPP No.!");
                        return;
                    }
                }
                else
                {
                    MyUtility.Msg.ErrorBox(result.ToString());
                    return;
                }
            }

            string updateCmd = string.Format("update GMTBooking set Status = 'Confirmed', EditName = '{0}', EditDate = GETDATE() where ID = '{1}'", Sci.Env.User.UserID, MyUtility.Convert.GetString(CurrentMaintain["ID"]));
            DualResult result1 = DBProxy.Current.Execute(null, updateCmd);
            if (!result1)
            {
                MyUtility.Msg.ErrorBox("Confirm fail !\r\n" + result1.ToString());
                return;
            }

            RenewData();
            OnDetailEntered();
            EnsureToolbarExt();
        }

        //UnConfirm
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            //Ship plan已經Confirm就不可以做Unconfirm
            if (MyUtility.GetValue.Lookup("Status", MyUtility.Convert.GetString(CurrentMaintain["ShipPlanID"]),"ShipPlan","ID") == "Confirmed")
            {
                MyUtility.Msg.WarningBox("Ship Plan already confirmed, can't unconfirm!");
                return;
            }

            //問是否要做Unconfirm，確定才繼續往下做
            DialogResult buttonResult = MyUtility.Msg.WarningBox("Are you sure you want to < Unconfirm > this data?", "Warning", MessageBoxButtons.YesNo);
            if (buttonResult == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            string updateCmd = string.Format("update GMTBooking set Status = 'New', EditName = '{0}', EditDate = GETDATE() where ID = '{1}'", Sci.Env.User.UserID, MyUtility.Convert.GetString(CurrentMaintain["ID"]));

            DualResult result = DBProxy.Current.Execute(null, updateCmd);
            if (!result)
            {
                MyUtility.Msg.WarningBox("UnConfirm fail !\r\n" + result.ToString());
            }

            RenewData();
            OnDetailEntered();
            EnsureToolbarExt();
        }

        //Terminal/Whse#
        private void textBox7_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            DataTable dt;
            string sqlCmd = string.Format(@"select fwd.WhseNo,fwd.UKey from ForwarderWhse fw, ForwarderWhse_Detail fwd
where fw.ID = fwd.ID
and fw.BrandID = '{0}'
and fw.Forwarder = '{1}'
and fw.ShipModeID = '{2}'
order by fwd.WhseNo", MyUtility.Convert.GetString(CurrentMaintain["BrandID"]), MyUtility.Convert.GetString(CurrentMaintain["Forwarder"]), MyUtility.Convert.GetString(CurrentMaintain["ShipModeID"]));
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out dt);

            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(dt, "WhseNo", "50", MyUtility.Convert.GetString(textBox7.Text));

            DialogResult result1 = item.ShowDialog();
            if (result1 == DialogResult.Cancel) { return; }
            IList<DataRow> dr =  item.GetSelecteds();
            textBox7.Text = item.GetSelectedString();
            CurrentMaintain["ForwarderWhse_DetailUKey"] = dr[0]["Ukey"];
        }        
    }
}
