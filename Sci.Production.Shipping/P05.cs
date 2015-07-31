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
        private string sqlCmd, masterID, insertCmd, updateCmd;
        private DualResult result, result2;
        private DataTable selectData;
        private DataRow dr;

        public P05(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            gridicon.Append.Visible = false;
            gridicon.Insert.Visible = false;
            detailgrid.AllowUserToOrderColumns = true;
            InsertDetailGridOnDoubleClick = false;
        }

        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            masterID = (e.Master == null) ? "" : e.Master["ID"].ToString();
            this.DetailSelectCommand = string.Format(@"select p.GMTBookingLock,p.FactoryID,p.ID,
iif(p.OrderID='',(select cast(a.OrderID as nvarchar) +',' from (select distinct OrderID from PackingList_Detail pd where pd.ID = p.id) a for xml path('')),p.OrderID) as OrderID,
p.CargoReadyDate,iif(p.type = 'B',(select BuyerDelivery from Order_QtyShip where ID = p.OrderID and Seq = p.OrderShipmodeSeq),(select oq.BuyerDelivery from (select top 1 OrderID, OrderShipmodeSeq from PackingList_Detail pd where pd.ID = p.ID) a, Order_QtyShip oq where a.OrderID = oq.Id and a.OrderShipmodeSeq = oq.Seq)) as BuyerDelivery,
iif(p.type = 'B',(select SDPDate from Order_QtyShip where ID = p.OrderID and Seq = p.OrderShipmodeSeq),(select oq.SDPDate from (select top 1 OrderID, OrderShipmodeSeq from PackingList_Detail pd where pd.ID = p.ID) a, Order_QtyShip oq where a.OrderID = oq.Id and a.OrderShipmodeSeq = oq.Seq)) as SDPDate,
p.PulloutDate,p.ShipQty,p.CTNQty,p.GW,p.CBM,p.InvNo,
iif(p.type = 'B',(select CustCDID from Orders where ID = p.OrderID),(select o.CustCDID from Orders o, (select top 1 OrderID from PackingList_Detail pd where pd.ID = p.ID) a where o.ID = a.OrderID)) as CustCDID,
iif(p.type = 'B',(select Dest from Orders where ID = p.OrderID),(select o.Dest from Orders o, (select top 1 OrderID from PackingList_Detail pd where pd.ID = p.ID) a where o.ID = a.OrderID)) as Dest,
p.NW,p.NNW,p.Status,(select sum(CTNQty) from PackingList_Detail pd where pd.ID = p.ID and pd.ClogReceiveID != '') as ClogCTNQty,p.InspDate,p.ShipModeID
from PackingList p
where p.INVNo = '{0}'", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
  
            Dictionary<String, String> comboBox1_RowSource = new Dictionary<string, string>();
            comboBox1_RowSource.Add("", "");
            comboBox1_RowSource.Add("CY-CY", "CY-CY");
            comboBox1_RowSource.Add("CFS-CY", "CFS-CY");
            comboBox1_RowSource.Add("CFS-CFS", "CFS-CFS");
            comboBox1.DataSource = new BindingSource(comboBox1_RowSource, null);
            comboBox1.ValueMember = "Key";
            comboBox1.DisplayMember = "Value";

        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();

            #region AirPP List按鈕變色
            if (!this.EditMode)
            {
                sqlCmd = string.Format(@"select pd.ID
from PackingList p, PackingList_Detail pd, AirPP a
where p.INVNo = '{0}' and p.ID = pd.ID and a.OrderID = pd.OrderID and a.OrderShipmodeSeq = pd.OrderShipmodeSeq", CurrentMaintain["ID"].ToString());

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
                if (CurrentMaintain["Status"].ToString() == "New")
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
                                if (e.FormattedValue.ToString() != dr["GMTBookingLock"].ToString())
                                {
                                    if (e.FormattedValue.ToString() != "Y")
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
                                    sqlCmd = string.Format(@"select p.ID
from PackingList pl, Pullout p
where pl.ID = '{0}'
and pl.PulloutID = p.ID
and p.Status = 'Confirmed'", dr["ID"].ToString());

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
            CurrentMaintain["Shipper"] = Sci.Env.User.Factory;
            CurrentMaintain["InvDate"] = DateTime.Today;
            CurrentMaintain["Handle"] = Sci.Env.User.UserID;
            CurrentMaintain["ShipModeID"] = "SEA";
            CurrentMaintain["ShipTermID"] = "FOB";
        }

        protected override bool ClickEditBefore()
        {
            if (CurrentMaintain["Status"].ToString() == "Confirmed")
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
                numericBox7.ReadOnly = true;
                dateBox3.ReadOnly = true;
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
            }
        }

        protected override bool ClickDeleteBefore()
        {
            if (CurrentMaintain["Status"].ToString() == "Confirmed")
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
            if (MyUtility.Check.Seek(string.Format(@"select ShippingAPID from ShareExpense where InvNo = '{0}'", CurrentMaintain["ID"].ToString())))
            {
                MyUtility.Msg.WarningBox("This record have expense data, can't be deleted!");
                return false;
            }

            //只要Pullout Report已經Confirmed就不可以被刪除
            sqlCmd = string.Format(@"select distinct pl.PulloutID
from PackingList pl, Pullout p
where pl.INVNo = '{0}'
and pl.PulloutID = p.ID
and p.Status = 'Confirmed'", CurrentMaintain["ID"].ToString());
            result = DBProxy.Current.Select(null, sqlCmd, out selectData);
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

        protected override bool OnDeleteDetails()
        {
            updateCmd = string.Format("update PackingList set GMTBookingLock = '', INVNo = '' where INVNo = '{0}';", CurrentMaintain["ID"].ToString());
            result = DBProxy.Current.Execute(null, updateCmd);
            if (result)
            {
                return true;
            }
            else
            {
                MyUtility.Msg.ErrorBox(result.ToString());
                return false;
            }
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
            string newID = "";
            //新增單狀態下，取ID且檢查此ID是否存在
            if (IsDetailInserting)
            {
                newID = MyUtility.GetValue.Lookup("NegoRegion", CurrentMaintain["Shipper"].ToString(), "Factory", "ID").Trim() + Convert.ToDateTime(CurrentMaintain["InvDate"]).ToString("yyMM") + "-" + CurrentMaintain["InvSerial"].ToString().Trim() + "-" + MyUtility.GetValue.Lookup("ShipCode",CurrentMaintain["BrandID"].ToString(),"Brand","ID").Trim();
                if (MyUtility.Check.Seek(newID, "GMTBooking", "ID"))
                {
                    MyUtility.Msg.WarningBox("Inv. Serial already exist!!");
                    textBox1.Focus();
                    return false;
                }
            }

            //組出表身所有的PackingListID與加總ShipQty,CTNQty,NW,GW,NNW,CBM
            string allPackID = "";
            int ttlshipqty = 0, ttlctnqty = 0;
            double ttlnw = 0.0, ttlgw = 0.0, ttlnnw = 0.0, ttlcbm = 0.0;
            foreach (DataRow dr in DetailDatas)
            {
                allPackID = allPackID + "'" + dr["ID"].ToString() + "',";
                ttlshipqty = ttlshipqty + Convert.ToInt32(dr["ShipQty"]);
                ttlctnqty = ttlctnqty + Convert.ToInt32(dr["CTNQty"]);
                ttlnw = MyUtility.Math.Round(ttlnw + Convert.ToDouble(dr["NW"]), 3);
                ttlgw = MyUtility.Math.Round(ttlgw + Convert.ToDouble(dr["GW"]), 3);
                ttlnnw = MyUtility.Math.Round(ttlnnw + Convert.ToDouble(dr["NNW"]), 3);
                ttlcbm = MyUtility.Math.Round(ttlcbm + Convert.ToDouble(dr["CBM"]), 3);
            }
            #region 檢查訂單的Currency是否一致與Payterm與表頭是否一致
            if (allPackID != "")
            {
                sqlCmd = string.Format(@"with OrderData
as
(select distinct pd.ID,pd.OrderID,o.CurrecnyID,o.PayTermARID
 from PackingList_Detail pd, Orders o
 where pd.ID in ({0})
 and pd.OrderID = o.ID
 ),
CurrencyCount
as
(select CurrecnyID, isnull(COUNT(CurrecnyID),0) as CNT
 from OrderData
 group by CurrecnyID
)
Select od.ID,od.OrderID,od.CurrecnyID,od.PayTermARID,cc.CNT
from OrderData od
left join CurrencyCount cc on od.CurrecnyID = cc.CurrecnyID
order by cc.CNT desc", allPackID.Substring(0, allPackID.Length - 1));
                result = DBProxy.Current.Select(null, sqlCmd, out selectData);
                if (!result)
                {
                    MyUtility.Msg.ErrorBox(result.ToString());
                    return false;
                }
                if (selectData.Rows.Count > 0)
                {
                    string msg = "";
                    foreach (DataRow dr in selectData.Select(string.Format("CurrecnyID <> '{0}'", selectData.Rows[0]["CurrecnyID"].ToString())))
                    {
                        msg = msg + "Packing#:" + dr["ID"].ToString() + ",   SP No.:" + dr["OrderID"].ToString() + ",   Currency:" + dr["CurrecnyID"].ToString() + "\r\n";
                    }
                    if (msg != "")
                    {
                        MyUtility.Msg.WarningBox("Currency is inconsistent!!\r\nThe currency of most SP No. is " + selectData.Rows[0]["CurrecnyID"].ToString() + "\r\n" + msg);
                        return false;
                    }

                    msg = "";
                    foreach (DataRow dr in selectData.Select(string.Format("PayTermARID <> '{0}'", CurrentMaintain["PayTermARID"].ToString())))
                    {
                        msg = msg + "Packing#:" + dr["ID"].ToString() + ",   Payment Term:" + dr["PayTermARID"].ToString() + "\r\n";
                    }
                    if (msg != "")
                    {
                        MyUtility.Msg.WarningBox("Payment term in detail SP is different from garment booking!!\r\n"+ msg);
                    }
                }
            }
            #endregion

            #region 組Description
            string season = "", category = "";
            if (allPackID != "")
            {
                sqlCmd = string.Format(@"with OrderData
as
(select distinct o.Category,o.SeasonID
 from PackingList_Detail pd, Orders o
 where pd.ID in ({0})
 and pd.OrderID = o.ID
)
select (select CAST(a.Category as nvarchar)+'/' from (select distinct Category from OrderData) a for xml path('')) as Category,
(select CAST(a.SeasonID as nvarchar)+'/' from (select distinct SeasonID from OrderData) a for xml path('')) as Season", allPackID.Substring(0, allPackID.Length - 1));
                result = DBProxy.Current.Select(null, sqlCmd, out selectData);
                if (!result)
                {
                    MyUtility.Msg.ErrorBox(result.ToString());
                    return false;
                }
                if (selectData.Rows.Count > 0)
                {
                    season = selectData.Rows[0]["Season"].ToString().Substring(0, selectData.Rows[0]["Season"].ToString().Length-1);
                    category = selectData.Rows[0]["Category"].ToString().Substring(0, selectData.Rows[0]["Category"].ToString().Length - 1);
                }
            }
            CurrentMaintain["Description"] = CurrentMaintain["BrandID"].ToString() + ',' + season + ',' + CurrentMaintain["CustCDID"].ToString() + "," + CurrentMaintain["Dest"].ToString() + "," + category;
            #endregion

            //將表身加總的資料回寫回表頭
            CurrentMaintain["TotalShipQty"] = ttlshipqty;
            CurrentMaintain["TotalCTNQty"] = ttlctnqty;
            CurrentMaintain["TotalNW"] = MyUtility.Math.Round(ttlnw,2);
            CurrentMaintain["TotalGW"] = MyUtility.Math.Round(ttlgw, 2);
            CurrentMaintain["TotalNNW"] = MyUtility.Math.Round(ttlnnw, 2);
            CurrentMaintain["TotalCBM"] = MyUtility.Math.Round(ttlcbm, 2);
            if (IsDetailInserting)
            {
                CurrentMaintain["ID"] = newID;
            }
            return base.ClickSaveBefore();
        }

        protected override bool OnSaveDetail(IList<DataRow> details, ITableSchema detailtableschema)
        {
            IList<string> updateCmds = new List<string>();

            foreach (DataRow dr in details)
            {
                if (dr.RowState == DataRowState.Modified)
                {
                    updateCmds.Add(string.Format("update PackingList set GMTBookingLock = '{0}' where ID = '{1}';", dr["GMTBookingLock"].ToString(), dr["ID"].ToString()));
                }
                if (dr.RowState == DataRowState.Added)
                {
                    updateCmds.Add(string.Format("update PackingList set GMTBookingLock = '{0}', INVNo = '{1}' where ID = '{2}';", dr["GMTBookingLock"].ToString(), CurrentMaintain["ID"].ToString(), dr["ID"].ToString()));
                }
                if (dr.RowState == DataRowState.Deleted)
                {
                    updateCmds.Add(string.Format("update PackingList set GMTBookingLock = '', INVNo = '' where ID = '{0}';", dr["ID"].ToString()));
                }
            }
            if (updateCmds.Count != 0)
            {
                result = DBProxy.Current.Executes(null, updateCmds);
                if (result)
                {
                    return true;
                }
                else
                {
                    MyUtility.Msg.ErrorBox(result.ToString());
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        //Inv. Serial:移除空白值
        private void textBox1_Validated(object sender, EventArgs e)
        {
            CurrentMaintain["InvSerial"] = CurrentMaintain["InvSerial"].ToString().Replace(" ", "");
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
                if (dateBox2.Value > DateTime.Today.AddMonths(12) || dateBox2.Value < DateTime.Today.AddMonths(-12))
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
                    item = new Sci.Win.Tools.SelectItem(string.Format("select ID, CountryID, City from CustCD where BrandID = '{0}' order by ID", CurrentMaintain["BrandID"].ToString()), "17,3,17", textBox4.Text);
                    DialogResult returnResult = item.ShowDialog();
                    if (returnResult == DialogResult.Cancel) { return; }
                    textBox4.Text = item.GetSelectedString();
                }
                else
                {
                    item = new Sci.Win.Tools.SelectItem(string.Format("select ID, CountryID, City from CustCD where BrandID = '{0}' and CountryID = '{1}' order by ID", CurrentMaintain["BrandID"].ToString(), CurrentMaintain["Dest"].ToString()), "17,3,17", textBox4.Text);
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
                    if (!MyUtility.Check.Seek(string.Format("select ID, CountryID, City from CustCD where BrandID = '{0}' and ID = '{1}' order by ID", CurrentMaintain["BrandID"].ToString(), textBox4.Text), null))
                    {
                        MyUtility.Msg.WarningBox(string.Format("< CustCD: {0} > not found!!!", textBox4.Text));
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
                    //txtpaytermar1.TextBox1.Text = paytermAR;
                    CurrentMaintain["PayTermARID"] = paytermAR;
                }
            }
        }

        //檢查輸入的Cut-off Date是否正確
        private void dateBox3_Validating(object sender, CancelEventArgs e)
        {
            if (this.EditMode && !MyUtility.Check.Empty(dateBox3.Value))
            {
                if (dateBox3.Value > DateTime.Today.AddMonths(12) || dateBox3.Value < DateTime.Today.AddMonths(-12))
                {
                    MyUtility.Msg.WarningBox("< Cut-off Date > is invalid!!");
                    dateBox3.Value = null;
                    e.Cancel = true;
                    return;
                }
            }
        }

        //檢查輸入的ETD是否正確
        private void dateBox5_Validating(object sender, CancelEventArgs e)
        {
            if (this.EditMode && !MyUtility.Check.Empty(dateBox5.Value))
            {
                if (dateBox5.Value > DateTime.Today.AddMonths(12) || dateBox5.Value < DateTime.Today.AddMonths(-12))
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
                if (dateBox6.Value > DateTime.Today.AddMonths(12) || dateBox6.Value < DateTime.Today.AddMonths(-12))
                {
                    MyUtility.Msg.WarningBox("< ETA > is invalid!!");
                    dateBox6.Value = null;
                    e.Cancel = true;
                    return;
                }
            }
        }

        //表身Grid的Delete
        protected override void OnDetailGridDelete()
        {
            //檢查此筆記錄的Pullout Report是否已經Confirmed，若是則出訊息告知且無法刪除
            if (this.DetailDatas.Count > 0)
            {
                if (MyUtility.GetValue.Lookup(string.Format("select p.Status from Pullout p, PackingList pl where pl.ID = '{0}' and p.ID = pl.PulloutID", CurrentDetailData["ID"].ToString())) == "Confirmed")
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
            Sci.Production.Shipping.P05_AirPrePaidList callNextForm = new Sci.Production.Shipping.P05_AirPrePaidList(CurrentMaintain["ID"].ToString());
            callNextForm.ShowDialog(this);
        }

        //Expense Data
        private void button2_Click(object sender, EventArgs e)
        {
            Sci.Production.Shipping.P05_ExpenseData callNextForm = new Sci.Production.Shipping.P05_ExpenseData(CurrentMaintain["ID"].ToString(), "InvNo");
            callNextForm.ShowDialog(this);
        }

        // S/O Confirm History
        private void button5_Click(object sender, EventArgs e)
        {
            Sci.Win.UI.ShowHistory callNextForm = new Sci.Win.UI.ShowHistory("GMTBooking_History", CurrentMaintain["ID"].ToString(), "SOCFMDate", reasonType: "GMTBooking_SO", caption: "S/O Revised History");
            callNextForm.ShowDialog(this);
        }

        // S/O Confirm/UnConfirm
        private void button4_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(CurrentMaintain["SOCFMDate"]))
            {
                #region Confirm
                if (MyUtility.Check.Empty(CurrentMaintain["SONo"]) || MyUtility.Check.Empty(CurrentMaintain["ForwarderWhseID"]) || MyUtility.Check.Empty(CurrentMaintain["CutOffDate"]))
                {
                    MyUtility.Msg.WarningBox("< S/O # > , < Terminal/Whse# > and < Cut-off Date > can't be empty!!");
                    return;
                }

                //檢查表身的ShipMode與表頭的ShipMode如果不同就不可以Confirm
                if (!CheckShipMode())
                {
                    return;
                }

                bool firstCFM = !MyUtility.Check.Seek(string.Format("select ID from GMTBooking_History where ID = '{0}' and HisType = '{1}'", CurrentMaintain["ID"].ToString(), "SOCFMDate"));
                insertCmd = string.Format(@"insert into GMTBooking_History (ID,HisType,OldValue,NewValue,AddName,AddDate)
values ('{0}','{1}','{2}','{3}','{4}','{5}')", CurrentMaintain["ID"].ToString(), "SOCFMDate", firstCFM ? "" : "CFM", "Un CFM", Sci.Env.User.UserID, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                updateCmd = string.Format(@"update GMTBooking set SOCFMDate = '{0}' where ID = '{1}';
update PackingList set GMTBookingLock = 'Y' where INVNo = '{1}';", DateTime.Today.ToString("d"), CurrentMaintain["ID"].ToString());
                using (TransactionScope transactionScope = new TransactionScope())
                {
                    try
                    {
                        result = DBProxy.Current.Execute(null, insertCmd);
                        result2 = DBProxy.Current.Execute(null, updateCmd);

                        if (result && result2)
                        {
                            transactionScope.Complete();
                        }
                        else
                        {
                            MyUtility.Msg.WarningBox("Confirm failed, Pleaes re-try");
                        }
                    }
                    catch (Exception ex)
                    {
                        ShowErr("Commit transaction error.", ex);
                        return;
                    }
                }
                #endregion
            }
            else
            {
                #region UnConfirm
                if (MyUtility.GetValue.Lookup("Status", CurrentMaintain["ShipPlanID"].ToString(), "ShipPlan", "ID") == "Confirmed")
                {
                    MyUtility.Msg.WarningBox("Ship Plan already confirmed, can't Un CFM!!");
                    return;
                }

                Sci.Win.UI.SelectReason callReason = new Sci.Win.UI.SelectReason("GMTBooking_SO", true);
                DialogResult dResult = callReason.ShowDialog(this);
                if (dResult == System.Windows.Forms.DialogResult.OK)
                {
                    insertCmd = string.Format(@"insert into GMTBooking_History (ID,HisType,OldValue,NewValue,ReasonID,Remark,AddName,AddDate)
values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')", CurrentMaintain["ID"].ToString(), "SOCFMDate", "Un CFM", "CFM", callReason.ReturnReason, callReason.ReturnRemark, Sci.Env.User.UserID, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                    updateCmd = string.Format(@"update GMTBooking set SOCFMDate = null where ID = '{0}'", CurrentMaintain["ID"].ToString());

                    using (TransactionScope transactionScope = new TransactionScope())
                    {
                        try
                        {
                            result = DBProxy.Current.Execute(null, insertCmd);
                            result2 = DBProxy.Current.Execute(null, updateCmd);

                            if (result && result2)
                            {
                                transactionScope.Complete();
                            }
                            else
                            {
                                MyUtility.Msg.WarningBox("UnConfirm failed, Pleaes re-try");
                            }
                        }
                        catch (Exception ex)
                        {
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
            string msg = "";
            foreach (DataRow dr in ((DataTable)detailgridbs.DataSource).Select(string.Format("ShipModeID <> '{0}'", CurrentMaintain["ShipModeID"].ToString())))
            {
                msg = msg + "Packing#:" + dr["ID"].ToString() + ",   Shipping Mode:" + dr["ShipModeID"].ToString() + "\r\n";
            }
            if (msg != "")
            {
                MyUtility.Msg.WarningBox("Shipping mode is inconsistent!!\r\n" + msg);
                return false;
            }
            return true;
        }

        //Container/Truck
        private void button3_Click(object sender, EventArgs e)
        {
            Sci.Production.Shipping.P05_ContainerTruck callNextForm = new Sci.Production.Shipping.P05_ContainerTruck(this.IsSupportEdit, CurrentMaintain["ID"].ToString(), null, null);
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

            if (MyUtility.Check.Seek(string.Format("select ID from ShipMode where UseFunction like '%AirPP%' and ID = '{0}'", CurrentMaintain["ShipModeID"].ToString())))
            {
                sqlCmd = string.Format(@"select p.OrderID, p.OrderShipmodeSeq, isnull(a.ID,'') as AirPPID
from (Select distinct b.OrderID,b.OrderShipmodeSeq 
      from PackingList a, PackingList_Detail b 
      where a.INVNo = '{0}' and a.ID = b.ID) p
left join AirPP a on p.OrderID = a.OrderID and p.OrderShipmodeSeq = a.OrderShipmodeSeq and a.Status != 'Junked'",CurrentMaintain["ID"].ToString());
                result = DBProxy.Current.Select(null, sqlCmd, out selectData);
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

            sqlCmd = string.Format("update GMTBooking set Status = 'Confirmed', EditName = '{0}', EditDate = '{1}' where ID = '{2}'", Sci.Env.User.UserID, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), CurrentMaintain["ID"].ToString());
            result = DBProxy.Current.Execute(null, sqlCmd);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Confirm fail !\r\n" + result.ToString());
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
            if (MyUtility.GetValue.Lookup("Status", CurrentMaintain["ShipPlanID"].ToString(),"ShipPlan","ID") == "Confirmed")
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

            sqlCmd = string.Format("update GMTBooking set Status = 'New', EditName = '{0}', EditDate = '{1}' where ID = '{2}'", Sci.Env.User.UserID, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), CurrentMaintain["ID"].ToString());

            result = DBProxy.Current.Execute(null, sqlCmd);
            if (!result)
            {
                MyUtility.Msg.WarningBox("UnConfirm fail !\r\n" + result.ToString());
            }

            RenewData();
            OnDetailEntered();
            EnsureToolbarExt();
        }
    }
}
