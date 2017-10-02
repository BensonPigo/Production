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
using System.Runtime.InteropServices;
using System.Data.SqlClient;

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

            this.txtCutoffDate.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "CutOffDate", true, System.Windows.Forms.DataSourceUpdateMode.OnValidation, emptyDTMask, Sci.Env.Cfg.DateTimeStringFormat));
            this.txtCutoffDate.Mask = dateTimeMask;

        }

        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            masterID = (e.Master == null) ? "1=0" : string.Format("p.INVNo = '{0}'",MyUtility.Convert.GetString(e.Master["ID"]));
            this.DetailSelectCommand = string.Format(@"
select  p.GMTBookingLock
        , p.FactoryID,p.ID
        , OrderID = STUFF ((select CONCAT (',', cast (a.OrderID as nvarchar)) 
                            from (
                                select distinct OrderID 
                                from PackingList_Detail pd WITH (NOLOCK) 
                                where pd.ID = p.id
                            ) a 
                            for xml path('')
                          ), 1, 1, '') 
        , p.CargoReadyDate
        , BuyerDelivery = ( select   oq.BuyerDelivery 
                            from (
                                select  top 1 OrderID
                                        , OrderShipmodeSeq 
                                from PackingList_Detail pd WITH (NOLOCK) 
                                where pd.ID = p.ID
                            ) a, Order_QtyShip oq 
                            where   a.OrderID = oq.Id 
                                    and a.OrderShipmodeSeq = oq.Seq)
        , SDPDate = (select oq.SDPDate 
                     from (
                         select  top 1 OrderID
                                 , OrderShipmodeSeq 
                         from PackingList_Detail pd WITH (NOLOCK) 
                         where pd.ID = p.ID
                     ) a, Order_QtyShip oq WITH (NOLOCK) 
                     where   a.OrderID = oq.Id 
                             and a.OrderShipmodeSeq = oq.Seq)
        , p.PulloutDate
        , p.ShipQty
        , p.CTNQty
        , p.GW
        , p.CBM
        , p.InvNo
        , CustCDID = (select o.CustCDID 
                      from (
                        select top 1 OrderID 
                        from PackingList_Detail pd WITH (NOLOCK) 
                        where pd.ID = p.ID
                      ) a, Orders o WITH (NOLOCK)  
                      where o.ID = a.OrderID)
        , Dest = (select o.Dest 
                  from (
                        select top 1 OrderID 
                        from PackingList_Detail pd WITH (NOLOCK) 
                        where pd.ID = p.ID
                  ) a, Orders o WITH (NOLOCK)
                  where o.ID = a.OrderID)
        , p.NW
        , p.NNW
        , p.Status
        , ClogCTNQty = (select sum(CTNQty) 
                        from PackingList_Detail pd WITH (NOLOCK) 
                        where   pd.ID = p.ID 
                                and pd.ReceiveDate is not null)
        , p.InspDate
        , p.ShipModeID
from PackingList p WITH (NOLOCK) 
where {0}", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            MyUtility.Tool.SetupCombox(comboContainerType, 1, 1, ",CY-CY,CFS-CY,CFS-CFS");         
           
        }             

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();

            txtTerminalWhse.Text = MyUtility.GetValue.Lookup("WhseNo", MyUtility.Convert.GetString(CurrentMaintain["ForwarderWhse_DetailUKey"]), "ForwarderWhse_Detail", "UKey");
           
                
            
            #region AirPP List按鈕變色
            if (!this.EditMode)
            {
                string sqlCmd = string.Format(@"select pd.ID
from PackingList p WITH (NOLOCK) , PackingList_Detail pd WITH (NOLOCK) , AirPP a WITH (NOLOCK) 
where p.INVNo = '{0}' and p.ID = pd.ID and a.OrderID = pd.OrderID and a.OrderShipmodeSeq = pd.OrderShipmodeSeq", MyUtility.Convert.GetString(CurrentMaintain["ID"]));

                if (MyUtility.Check.Seek(sqlCmd))
                {
                    btnAirPPList.ForeColor = Color.Red;
                    btnAirPPList.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
                }
                else
                {
                    btnAirPPList.ForeColor = Color.Black;
                    btnAirPPList.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
                }
            }
            #endregion

            #region S/O CFM按鈕權限
            if (!this.EditMode)
            {
                if (MyUtility.Convert.GetString(CurrentMaintain["Status"]) == "New")
                {
                    btnCFM.Enabled = true;
                }
                else
                {
                    btnCFM.Enabled = false;
                }
                if (MyUtility.Check.Empty(CurrentMaintain["SOCFMDate"]))
                {
                    btnCFM.Text = "CFM";
                }
                else
                {
                    btnCFM.Text = "Un CFM";
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
                                        dr["GMTBookingLock"] = "Y";
                                        e.Cancel = true;
                                        MyUtility.Msg.WarningBox("It should be only 'Y' or ''!");
                                        return;
                                    }
                                }
                            }
                            else
                            {
                                if (dr["GMTBookingLock"].ToString() == "Y")
                                {
                                    string sqlCmd = string.Format(@"select p.ID
from PackingList pl WITH (NOLOCK) , Pullout p WITH (NOLOCK) 
where pl.ID = '{0}'
and pl.PulloutID = p.ID
and p.Status = 'Confirmed'", MyUtility.Convert.GetString(dr["ID"]));

                                    if (MyUtility.Check.Seek(sqlCmd))
                                    {
                                        dr["GMTBookingLock"] = "Y";
                                        e.Cancel = true;
                                        MyUtility.Msg.WarningBox("Pullout report already confirmed, can't  unlock!");
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
                                    dr["CargoReadyDate"] = null;
                                    e.Cancel = true;
                                    MyUtility.Msg.WarningBox("< Cargo Ready Date > is invalid!!");
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
            txtInvSerial.ReadOnly = true;
            txtbrand.ReadOnly = true;
            txtCountryDestination.TextBox1.ReadOnly = true;
            if (CurrentMaintain["Status"].ToString().ToUpper()=="NEW")
            {
                txtShipmodeShippingMode.ReadOnly = false;    
            }
            else
            {
                txtShipmodeShippingMode.ReadOnly = true;    
            }
            

            if (!MyUtility.Check.Empty(CurrentMaintain["SOCFMDate"]))
            {
                dateInvDate.ReadOnly = true;
                txtfactoryShipper.ReadOnly = true;
                dateFCRDate.ReadOnly = true;
                txtCustCD.ReadOnly = true;
                txtUserHandle.TextBox1.ReadOnly = true;
                txtSubconForwarder.TextBox1.ReadOnly = true;
                comboContainerType.ReadOnly = true;
                txtSONo.ReadOnly = true;
                //textBox7.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.EditModeAndNonReadOnly;
               // textBox6.ReadOnly = true;
                col_lock.IsEditingReadOnly = true;
                col_crd.IsEditingReadOnly = true;
                detailgrid.Columns[0].DefaultCellStyle.ForeColor = Color.Black;
                detailgrid.Columns[4].DefaultCellStyle.ForeColor = Color.Black;
                btnImportfrompackinglist.Enabled = false;
                gridicon.Remove.Enabled = false;
                txtCutoffDate.ReadOnly = true;
                txtTerminalWhse.ReadOnly = true;
            }
            else
            {
                gridicon.Remove.Enabled = true;
                col_lock.IsEditingReadOnly = false;
                col_crd.IsEditingReadOnly = false;
                detailgrid.Columns[0].DefaultCellStyle.ForeColor = Color.Red;
                detailgrid.Columns[4].DefaultCellStyle.ForeColor = Color.Red;
                txtCutoffDate.ReadOnly = false;
                txtTerminalWhse.ReadOnly = false;
                //textBox7.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.EditModeAndReadOnly;
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
            if (MyUtility.Check.Seek(string.Format(@"select ShippingAPID from ShareExpense WITH (NOLOCK) where InvNo = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ID"]))))
            {
                MyUtility.Msg.WarningBox("This record have expense data, can't be deleted!");
                return false;
            }

            //只要Pullout Report已經Confirmed就不可以被刪除
            string sqlCmd = string.Format(@"
select  distinct pl.PulloutID
from PackingList pl WITH (NOLOCK) 
     , Pullout p WITH (NOLOCK) 
where   pl.INVNo = '{0}'
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
            DualResult result;
            #region 檢查必輸欄位
            if (MyUtility.Check.Empty(CurrentMaintain["InvSerial"]))
            {
                txtInvSerial.Focus();
                MyUtility.Msg.WarningBox("Inv. Serial can't empty!!");
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["InvDate"]))
            {
                dateInvDate.Focus();
                MyUtility.Msg.WarningBox("Inv. Date can't empty!!");
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["Shipper"]))
            {
                txtfactoryShipper.Focus();
                MyUtility.Msg.WarningBox("Shipper can't empty!!");
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["BrandID"]))
            {
                txtbrand.Focus();
                MyUtility.Msg.WarningBox("Brand can't empty!!");
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["FCRDate"]))
            {
                dateFCRDate.Focus();
                MyUtility.Msg.WarningBox("FCR Date can't empty!!");
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["CustCDID"]))
            {
                txtCustCD.Focus();
                MyUtility.Msg.WarningBox("CustCD can't empty!!");
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["Dest"]))
            {
                txtCountryDestination.TextBox1.Focus();
                MyUtility.Msg.WarningBox("Destination can't empty!!");
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["PayTermARID"]))
            {
                txtpaytermarPaymentTerm.TextBox1.Focus();
                MyUtility.Msg.WarningBox("Payment Term can't empty!!");
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["ShipModeID"]))
            {
                txtShipmodeShippingMode.Focus();
                MyUtility.Msg.WarningBox("Shipping Mode can't empty!!");
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["ShipTermID"]))
            {
                txtShiptermShipmentTerm.Focus();
                MyUtility.Msg.WarningBox("Shipment Term can't empty!!");
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["Handle"]))
            {
                txtUserHandle.TextBox1.Focus();
                MyUtility.Msg.WarningBox("Handle can't empty!!");
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["Forwarder"]))
            {
                txtSubconForwarder.TextBox1.Focus();
                MyUtility.Msg.WarningBox("Forwarder can't empty!!");
                return false;
            }

            string sqlCmd = string.Format(@"select fwd.WhseNo,fwd.address,fwd.UKey from ForwarderWhse fw WITH (NOLOCK) , ForwarderWhse_Detail fwd WITH (NOLOCK) 
where fw.ID = fwd.ID
and fw.BrandID = '{0}'
and fw.Forwarder = '{1}'
and fw.ShipModeID = '{2}'
and  fwd.WhseNo = '{3}'
order by fwd.WhseNo", MyUtility.Convert.GetString(CurrentMaintain["BrandID"]), MyUtility.Convert.GetString(CurrentMaintain["Forwarder"]), MyUtility.Convert.GetString(CurrentMaintain["ShipModeID"]),txtTerminalWhse.Text);
            if (!MyUtility.Check.Empty(txtTerminalWhse.Text)) {
                if (!MyUtility.Check.Seek(sqlCmd, ""))
                {
                    txtTerminalWhse.Focus();
                    MyUtility.Msg.WarningBox("Whse# is not found!!");
                    return false;
                };
            }
         

            #endregion            
            //新增單狀態下，取ID且檢查此ID是否存在
            if (IsDetailInserting)
            {
                string newID = MyUtility.GetValue.Lookup("NegoRegion", MyUtility.Convert.GetString(CurrentMaintain["Shipper"]), "Factory", "ID").Trim() + Convert.ToDateTime(CurrentMaintain["InvDate"]).ToString("yyMM") + "-" + MyUtility.Convert.GetString(CurrentMaintain["InvSerial"]).Trim() + "-" + MyUtility.GetValue.Lookup("ShipCode",MyUtility.Convert.GetString(CurrentMaintain["BrandID"]),"Brand","ID").Trim();
                if (MyUtility.Check.Seek(newID, "GMTBooking", "ID"))
                {
                    txtInvSerial.Focus();
                    MyUtility.Msg.WarningBox("Inv. Serial already exist!!");
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
                 sqlCmd = string.Format(@"with OrderData
as
(select distinct pd.ID,pd.OrderID,o.CurrencyID,o.PayTermARID
 from PackingList_Detail pd WITH (NOLOCK) , Orders o WITH (NOLOCK) 
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
                result = DBProxy.Current.Select(null, sqlCmd, out selectData);
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

            #region 簡查 BrandID 是否與表頭一致
            string checkBrandSQL = string.Format(@"
select  Msg = Concat('The brand of <SP#> ', tmpSP.value, ' is not ', '{0}')
from #tmp
outer apply (
    select value = data
    from dbo.SplitString(#tmp.OrderID, ',') 
) tmpSP
left join Orders o on tmpSP.value = o.ID
where   o.BrandID is null
        or o.BrandID != '{0}'", this.txtbrand.Text);
            DataTable checkBrandDt;
            result =  MyUtility.Tool.ProcessWithDatatable(((DataTable)((BindingSource)detailgrid.DataSource).DataSource), "", checkBrandSQL, out checkBrandDt, "#tmp");
            if (!result)
            {
                MyUtility.Msg.WarningBox(result.Description);
                return false;
            }
            if (checkBrandDt != null && checkBrandDt.Rows.Count > 0)
            {
                StringBuilder ErrorMsg = new StringBuilder("Garment Booking cannot be created !!");
                foreach (DataRow checkBrandDr in checkBrandDt.Rows)
                {
                    ErrorMsg.Append(Environment.NewLine + checkBrandDr["Msg"]);
                }
                MyUtility.Msg.WarningBox(ErrorMsg.ToString());
                return false;
            }
            #endregion 

            #region 組Description
            string season = "", category = "";
            if (allPackID.Length > 0)
            {
                 sqlCmd = string.Format(@"with OrderData
as
(select distinct o.Category,o.SeasonID
 from PackingList_Detail pd WITH (NOLOCK) , Orders o WITH (NOLOCK) 
 where pd.ID in ({0})
 and pd.OrderID = o.ID
)
select (select CAST(a.Category as nvarchar)+'/' from (select distinct Category from OrderData) a for xml path('')) as Category,
(select CAST(a.SeasonID as nvarchar)+'/' from (select distinct SeasonID from OrderData) a for xml path('')) as Season", allPackID.ToString().Substring(0, allPackID.Length - 1));
                result = DBProxy.Current.Select(null, sqlCmd, out selectData);
                if (!result)
                {
                    MyUtility.Msg.ErrorBox(result.ToString());
                    return false;
                }
                if (selectData.Rows.Count > 0)
                {
                    if (!MyUtility.Check.Empty(selectData.Rows[0]["Season"]))
                    {
                        season = MyUtility.Convert.GetString(selectData.Rows[0]["Season"]).Substring(0, MyUtility.Convert.GetString(selectData.Rows[0]["Season"]).Length - 1);
                        category = MyUtility.Convert.GetString(selectData.Rows[0]["Category"]).Substring(0, MyUtility.Convert.GetString(selectData.Rows[0]["Category"]).Length - 1);
                    }                   
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
            worksheet.Cells[10, 2] = MyUtility.Convert.GetString(CurrentMaintain["PayTermARID"]) + " - " + MyUtility.GetValue.Lookup(string.Format("select Description from PayTermAR WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["PayTermARID"])));
            worksheet.Cells[11, 2] = MyUtility.Convert.GetString(CurrentMaintain["Description"]);
            worksheet.Cells[12, 2] = MyUtility.Convert.GetString(CurrentMaintain["Remark"]);

            worksheet.Cells[3, 5] = MyUtility.Convert.GetString(CurrentMaintain["Dest"]) + " - " + MyUtility.GetValue.Lookup(string.Format("select NameEN from Country WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["Dest"])));
            worksheet.Cells[4, 5] = MyUtility.Convert.GetString(CurrentMaintain["ShipModeID"]);
            worksheet.Cells[5, 5] = MyUtility.Convert.GetString(CurrentMaintain["ShipTermID"]);
            worksheet.Cells[6, 5] = MyUtility.Convert.GetInt(CurrentMaintain["TotalShipQty"]);
            worksheet.Cells[7, 5] = MyUtility.Convert.GetInt(CurrentMaintain["TotalCTNQty"]);
            worksheet.Cells[8, 5] = MyUtility.Convert.GetDecimal(CurrentMaintain["TotalGW"]);
            worksheet.Cells[9, 5] = MyUtility.Convert.GetDecimal(CurrentMaintain["TotalCBM"]);
            worksheet.Cells[10, 5] = MyUtility.Convert.GetDecimal(CurrentMaintain["TotalNW"]);
            worksheet.Cells[11, 5] = MyUtility.Convert.GetDecimal(CurrentMaintain["TotalNNW"]);

            worksheet.Cells[3, 9] = MyUtility.Convert.GetString(CurrentMaintain["Handle"]) + "  " + MyUtility.GetValue.Lookup(string.Format("select Name from Pass1 WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["Handle"])));
            worksheet.Cells[4, 9] = MyUtility.Convert.GetString(CurrentMaintain["Forwarder"]) + "  " + MyUtility.GetValue.Lookup(string.Format("select Name from LocalSupp WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["Forwarder"])));
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

            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Shipping_P05");
            excel.ActiveWorkbook.SaveAs(strExcelName);
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(worksheet);

            strExcelName.OpenFile();
            #endregion
            return base.ClickPrint();
        }

        //Inv. Serial:移除空白值
        private void txtInvSerial_Validated(object sender, EventArgs e)
        {
            this.txtInvSerial.Text = this.txtInvSerial.Text.ToString().Replace(" ", "");            
            CurrentMaintain["InvSerial"] = MyUtility.Convert.GetString(CurrentMaintain["InvSerial"]).Replace(" ", "");
            
            }   

        //檢查輸入的Inv. Date是否正確
        private void dateInvDate_Validating(object sender, CancelEventArgs e)
        {
            if (this.EditMode && !MyUtility.Check.Empty(dateInvDate.Value))
            {
                if (dateInvDate.Value > DateTime.Today.AddDays(180) || dateInvDate.Value < DateTime.Today.AddDays(-180))
                {
                    dateInvDate.Value = null;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox("< Inv. Date > is invalid, it exceeds +/-180 days!!");
                    return;
                }
            }
        }

        //輸入Brand後自動帶出Payment Term
        private void txtbrand_Validated(object sender, EventArgs e)
        {
            if (this.EditMode && txtbrand.OldValue != txtbrand.Text)
            {
                GetPaytermAP();
            }
        }

        //檢查輸入的FCR Date是否正確
        private void dateFCRDate_Validating(object sender, CancelEventArgs e)
        {
            if (this.EditMode && !MyUtility.Check.Empty(dateFCRDate.Value))
            {
                if (!CheckDate((DateTime)MyUtility.Convert.GetDate(dateFCRDate.Value), -12, 12))
                {
                    dateFCRDate.Value = null;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox("< FCR Date > is invalid!!");
                    return;
                }

                //新增單時，自動將FCR Date寫入Inv. Date欄位
                if (MyUtility.Check.Empty(CurrentMaintain["ID"]))
                {
                    CurrentMaintain["FCRDate"] = dateFCRDate.Value;
                    CurrentMaintain["InvDate"] = dateFCRDate.Value;
                }
            }
        }

        //CustCD按右鍵
        private void txtCustCD_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            if (this.EditMode)
            {
                Sci.Win.Tools.SelectItem item;
                if (MyUtility.Check.Empty(CurrentMaintain["ID"]))
                {
                    item = new Sci.Win.Tools.SelectItem(string.Format("select ID, CountryID, City from CustCD WITH (NOLOCK) where BrandID = '{0}' order by ID", MyUtility.Convert.GetString(CurrentMaintain["BrandID"])), "17,3,17", txtCustCD.Text);
                    DialogResult returnResult = item.ShowDialog();
                    if (returnResult == DialogResult.Cancel) { return; }
                    txtCustCD.Text = item.GetSelectedString();
                }
                else
                {
                    item = new Sci.Win.Tools.SelectItem(string.Format("select ID, CountryID, City from CustCD WITH (NOLOCK) where BrandID = '{0}' and CountryID = '{1}' order by ID", MyUtility.Convert.GetString(CurrentMaintain["BrandID"]), MyUtility.Convert.GetString(CurrentMaintain["Dest"])), "17,3,17", txtCustCD.Text);
                    DialogResult returnResult = item.ShowDialog();
                    if (returnResult == DialogResult.Cancel) { return; }
                    txtCustCD.Text = item.GetSelectedString();
                }
            }
            this.txtCustCD.ValidateControl();
        }

        //檢查輸入的CustCD是否正確
        private void txtCustCD_Validating(object sender, CancelEventArgs e)
        {
            if (this.EditMode && !MyUtility.Check.Empty(txtCustCD.Text) && txtCustCD.OldValue != txtCustCD.Text)
            {
                if (!MyUtility.Check.Empty(CurrentMaintain["BrandID"]))
                {
                    //sql參數
                    System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter("@brandid", MyUtility.Convert.GetString(CurrentMaintain["BrandID"]));
                    System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter("@custcdid", txtCustCD.Text);

                    IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                    cmds.Add(sp1);
                    cmds.Add(sp2);

                    DataTable CustCDData;
                    string sqlCmd = "select ID, CountryID, City from CustCD WITH (NOLOCK) where BrandID = @brandid and ID = @custcdid order by ID";
                    DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out CustCDData);

                    if (!result || CustCDData.Rows.Count <= 0)
                    {
                        if (!result)
                        {
                            MyUtility.Msg.WarningBox("Sql connection fail!!\r\n"+result.ToString());
                        }
                        else
                        {
                            MyUtility.Msg.WarningBox(string.Format("< CustCD: {0} > not found!!!", txtCustCD.Text));
                        }
                        txtCustCD.Text = "";
                        e.Cancel = true;
                        return;
                    }
                    else
                    {
                        CurrentMaintain["CustCDID"] = txtCustCD.Text;
                        GetPaytermAP();
                    }
                }
            }
        }

        private void txtCustCD_Validated(object sender, EventArgs e)
        {
            if (this.EditMode && !MyUtility.Check.Empty(txtCustCD.Text) && txtCustCD.OldValue != txtCustCD.Text)
            {
                CurrentMaintain["Dest"] = MyUtility.GetValue.Lookup(string.Format("SELECT CountryID FROM CustCD WITH (NOLOCK) WHERE BrandID = '{0}' AND ID = '{1}'", MyUtility.Convert.GetString(CurrentMaintain["BrandID"]), txtCustCD.Text));
            }
        }  

        //自動帶出PaytermARID
        private void GetPaytermAP()
        {
            if (MyUtility.Check.Empty(txtbrand.Text))
            {
                txtpaytermarPaymentTerm.TextBox1.Text = "";
            }
            else
            {
                string paytermAR = "";
                if (MyUtility.Check.Empty(txtCustCD.Text))
                {
                    paytermAR = MyUtility.GetValue.Lookup("PayTermARIDBulk", txtbrand.Text, "Brand", "ID");
                }
                else
                {
                    paytermAR = MyUtility.GetValue.Lookup(string.Format("select PayTermARIDBulk from CustCD WITH (NOLOCK) where BrandID = '{0}' and ID = '{1}'", txtbrand.Text, txtCustCD.Text));
                }

                if (paytermAR != "")
                {
                    CurrentMaintain["PayTermARID"] = paytermAR;
                }
            }
        }

        //檢查輸入的Cut-off Date是否正確
        private void txtCutoffDate_Validating(object sender, CancelEventArgs e)
        {
            if ((txtCutoffDate.Text == "/  /     :  :"))
            {
                this.txtVslvoyFltNo.Focus();
                this.txtCutoffDate.Text = "";
                CurrentMaintain["CutoffDate"] = DBNull.Value;
                return;
            }
            if (this.EditMode && txtCutoffDate.Text != emptyDTMask)
            {
                string cutOffDate = txtCutoffDate.Text.Substring(0, 10).Replace(" ","1");
                try
                {
                    if (!CheckDate((DateTime)MyUtility.Convert.GetDate(cutOffDate), -12, 12))
                    {
                        txtCutoffDate.Text = null;
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox("< Cut-off Date > is invalid!!");
                        return;
                    }
                }
                catch (Exception )
                {
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox("< Cut-off Date > is invalid!!");
                    return;
                }
                
            }
        }

        //檢查輸入的ETD是否正確

        private void dateETD_Validating(object sender, CancelEventArgs e)
        {
            if (this.EditMode && !MyUtility.Check.Empty(dateETD.Value))
            {
                if (!CheckDate((DateTime)MyUtility.Convert.GetDate(dateETD.Value), -12, 12))
                {
                    dateETD.Value = null;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox("< ETD > is invalid!!");
                    return;
                }
            }
        }

        //檢查輸入的ETA是否正確
        private void dateETA_Validating(object sender, CancelEventArgs e)
        {
            if (this.EditMode && !MyUtility.Check.Empty(dateETA.Value))
            {
                if (!CheckDate((DateTime)MyUtility.Convert.GetDate(dateETA.Value), -12, 12))
                {
                    dateETA.Value = null;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox("< ETA > is invalid!!");
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
                if (MyUtility.GetValue.Lookup(string.Format("select p.Status from Pullout p WITH (NOLOCK) , PackingList pl WITH (NOLOCK) where pl.ID = '{0}' and p.ID = pl.PulloutID", MyUtility.Convert.GetString(CurrentDetailData["ID"]))) == "Confirmed")
                {
                    MyUtility.Msg.WarningBox("Pullout report already confirmed, can't be deleted!");
                    return;
                }
            }
            base.OnDetailGridDelete();
        }

        //AirPP List
        private void btnAirPPList_Click(object sender, EventArgs e)
        {
            Sci.Production.Shipping.P05_AirPrePaidList callNextForm = new Sci.Production.Shipping.P05_AirPrePaidList(MyUtility.Convert.GetString(CurrentMaintain["ID"]));
            callNextForm.ShowDialog(this);
        }

        //Expense Data
        private void btnExpenseData_Click(object sender, EventArgs e)
        {
            Sci.Production.Shipping.P05_ExpenseData callNextForm = new Sci.Production.Shipping.P05_ExpenseData(MyUtility.Convert.GetString(CurrentMaintain["ID"]), "InvNo");
            callNextForm.ShowDialog(this);
        }

        // S/O Confirm History
        private void btnH_Click(object sender, EventArgs e)
        {
            Sci.Win.UI.ShowHistory callNextForm = new Sci.Win.UI.ShowHistory("GMTBooking_History", MyUtility.Convert.GetString(CurrentMaintain["ID"]), "SOCFMDate", reasonType: "GMTBooking_SO", caption: "S/O Revised History");
            callNextForm.ShowDialog(this);
        }

        // S/O Confirm/UnConfirm
        private void btnCFM_Click(object sender, EventArgs e)
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

                bool firstCFM = !MyUtility.Check.Seek(string.Format("select ID from GMTBooking_History WITH (NOLOCK) where ID = '{0}' and HisType = '{1}'", MyUtility.Convert.GetString(CurrentMaintain["ID"]), "SOCFMDate"));
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
        private void btnContainerTruck_Click(object sender, EventArgs e)
        {
            Sci.Production.Shipping.P05_ContainerTruck callNextForm = new Sci.Production.Shipping.P05_ContainerTruck(this.IsSupportEdit, MyUtility.Convert.GetString(CurrentMaintain["ID"]), null, null);
            callNextForm.ShowDialog(this);
        }

        //Import from packing list
        private void btnImportfrompackinglist_Click(object sender, EventArgs e)
        {
            //Brand, CustCD, Destination, Ship Mode不可以為空
            if (MyUtility.Check.Empty(CurrentMaintain["BrandID"]))
            {
                txtbrand.Focus();
                MyUtility.Msg.WarningBox("< Brand > can't empty!");
                return;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["CustCDID"]))
            {
                txtCustCD.Focus();
                MyUtility.Msg.WarningBox("< CustCD > can't empty!");
                return;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["Dest"]))
            {
                txtCountryDestination.TextBox1.Focus();
                MyUtility.Msg.WarningBox("< Destination > can't empty!");
                return;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["ShipModeID"]))
            {
                txtShipmodeShippingMode.Focus();
                MyUtility.Msg.WarningBox("< Shipping Mode > can't empty!");
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
            if (MyUtility.Check.Seek(string.Format("select ID from ShipMode WITH (NOLOCK) where UseFunction like '%AirPP%' and ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ShipModeID"]))))
            {
                string sqlCmd = string.Format(@"select p.OrderID, p.OrderShipmodeSeq, isnull(a.ID,'') as AirPPID
from (Select distinct b.OrderID,b.OrderShipmodeSeq 
      from PackingList a WITH (NOLOCK) , PackingList_Detail b WITH (NOLOCK) 
      where a.INVNo = '{0}' and a.ID = b.ID) p
left join AirPP a WITH (NOLOCK) on p.OrderID = a.OrderID and p.OrderShipmodeSeq = a.OrderShipmodeSeq and a.Status != 'Junked'", MyUtility.Convert.GetString(CurrentMaintain["ID"]));
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
            //Check PackingList 全部是否都Confirmed
            DualResult resultPkl;
            if (resultPkl = DBProxy.Current.Select(null, string.Format(@"select * from PackingList WITH (NOLOCK) where  invno='{0}' ", MyUtility.Convert.GetString(CurrentMaintain["ID"])), out selectData))
            {
                if (selectData.Rows.Count>0)
                {
                    DataRow[] row = selectData.Select("status<>'Confirmed'");
                    StringBuilder MSG = new StringBuilder();
                    if (row.Length > 0)
                    {
                        foreach (DataRow dr in row)
                        {
                            MSG.Append(string.Format("Packing NO : {0}\n\r",dr["ID"]));
                        }
                        MyUtility.Msg.WarningBox(@"PackingList not yet confirmed,please confirm listed below first!! " + MSG.ToString());
                        return;   
                    }
                }
                else
                {
                    StringBuilder msg1 = new StringBuilder();
                    msg1.Append(string.Format("InvoNO: {0}", MyUtility.Convert.GetString(CurrentMaintain["ID"])));
                    MyUtility.Msg.WarningBox("InvoNo doesn't exist in Packing List, can't confirm!" + msg1.ToString());
                    return;
                }
               
            }
            else
            {
                MyUtility.Msg.ErrorBox(resultPkl.ToString());
                return;
            }

            string updateCmd = string.Format("update GMTBooking set Status = 'Confirmed', EditName = '{0}', EditDate = GETDATE() where ID = '{1}'", Sci.Env.User.UserID, MyUtility.Convert.GetString(CurrentMaintain["ID"]));
            DualResult result1 = DBProxy.Current.Execute(null, updateCmd);
            if (!result1)
            {
                MyUtility.Msg.ErrorBox("Confirm fail !\r\n" + result1.ToString());
                return;
            }

           
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

           
        }

        //Terminal/Whse#
        private void txtTerminalWhse_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            DataTable dt;
            string sqlCmd = string.Format(@"select fwd.WhseNo,fwd.address,fwd.UKey from ForwarderWhse fw WITH (NOLOCK) , ForwarderWhse_Detail fwd WITH (NOLOCK) 
where fw.ID = fwd.ID
and fw.BrandID = '{0}'
and fw.Forwarder = '{1}'
and fw.ShipModeID = '{2}'
order by fwd.WhseNo", MyUtility.Convert.GetString(CurrentMaintain["BrandID"]), MyUtility.Convert.GetString(CurrentMaintain["Forwarder"]), MyUtility.Convert.GetString(CurrentMaintain["ShipModeID"]));
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out dt);

            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(dt, "WhseNo,address", "20,20", MyUtility.Convert.GetString(txtTerminalWhse.Text));

            DialogResult result1 = item.ShowDialog();
            if (result1 == DialogResult.Cancel) { return; }
            IList<DataRow> dr =  item.GetSelecteds();
            txtTerminalWhse.Text = item.GetSelectedString();
            CurrentMaintain["ForwarderWhse_DetailUKey"] = dr[0]["Ukey"];
        }

        private void txtTerminalWhse_Validating(object sender, CancelEventArgs e)
        {
            if (MyUtility.Check.Empty(txtTerminalWhse.Text))
            {
                this.txtTerminalWhse.Text = "";
                CurrentMaintain["ForwarderWhse_DetailUKey"] = 0;
                return;
            }
            DataTable dt;
            string sqlCmd = string.Format(@"select fwd.WhseNo,fwd.UKey from ForwarderWhse fw WITH (NOLOCK) , ForwarderWhse_Detail fwd WITH (NOLOCK) 
where fw.ID = fwd.ID
and fwd.whseno = '{0}'
order by fwd.WhseNo", this.txtTerminalWhse.Text.ToString().Trim());
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out dt);
            if (result)
            {
                if (dt.Rows.Count>=1)
                {
                    this.txtTerminalWhse.Text = dt.Rows[0]["WhseNo"].ToString();
                    CurrentMaintain["ForwarderWhse_DetailUKey"] = dt.Rows[0]["Ukey"].ToString();
                }
                else
                {
                    CurrentMaintain["ForwarderWhse_DetailUKey"] = 0;
                    this.txtTerminalWhse.Text = "";
                    MyUtility.Msg.WarningBox("Whse# is not found!!");
                    e.Cancel = true;                    
                }
            }           
        }
       
        private void maskedTextBox1_Validated(object sender, EventArgs e)
        {
            MyUtility.Msg.InfoBox("validated");
        }

        private void txtfactoryShipper_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            #region SQL CMD
            string sqlcmd = string.Format(@"
Select DISTINCT Factory = ID
from Factory WITH (NOLOCK) 
where Junk = 0
order by ID");
            #endregion
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlcmd, "8", this.txtfactoryShipper.Text, false, ",");
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel) { return; }
            this.txtfactoryShipper.Text = item.GetSelectedString();
        }

        private void txtfactoryShipper_Validating(object sender, CancelEventArgs e)
        {
            #region SQL Parameter
            List<SqlParameter> listSqlPar = new List<SqlParameter>();
            listSqlPar.Add(new SqlParameter("@str", this.txtfactoryShipper.Text));
            #endregion
            string str = this.txtfactoryShipper.Text;
            #region SQL CMD
            string sqlcmd = string.Format(@"
Select DISTINCT Factory = ID
from Factory WITH (NOLOCK) 
where Junk = 0
      and ID = @str
order by ID");
            #endregion
            if (!string.IsNullOrWhiteSpace(str) && str != this.txtfactoryShipper.OldValue)
            {
                if (MyUtility.Check.Seek(sqlcmd, listSqlPar) == false)
                {
                    this.txtfactoryShipper.Text = "";
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("< Factory : {0} > not found!!!", str));
                    return;
                }
            }
        }
    }
}
