using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.PublicPrg;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Transactions;
using System.Windows.Forms;
using static Sci.Production.PublicPrg.Prgs;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// P05
    /// </summary>
    public partial class P05 : Win.Tems.Input6
    {
        private Ict.Win.UI.DataGridViewTextBoxColumn col_lock;
        private Ict.Win.UI.DataGridViewDateBoxColumn col_crd;
        private DataGridViewGeneratorNumericColumnSettings shipqty = new DataGridViewGeneratorNumericColumnSettings();
        private string masterID;
        private DataTable selectData;
        private string dateTimeMask = string.Empty;
        private string emptyDTMask = string.Empty;
        private string empmask;
        private string dtmask;
        private DateTime? FBDate_Ori;

        /// <summary>
        /// P05
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P05(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.gridicon.Append.Visible = false;
            this.gridicon.Insert.Visible = false;
            this.detailgrid.AllowUserToOrderColumns = true;
            this.InsertDetailGridOnDoubleClick = false;

            // 組Cut off date的mask
            for (int i = 0; i < Env.Cfg.DateTimeStringFormat.Length; i++)
            {
                this.dtmask = Env.Cfg.DateTimeStringFormat.Substring(i, 1) == "/" || Env.Cfg.DateTimeStringFormat.Substring(i, 1) == ":" ? Env.Cfg.DateTimeStringFormat.Substring(i, 1) : Env.Cfg.DateTimeStringFormat.Substring(i, 1) == " " ? " " : "0";
                this.empmask = Env.Cfg.DateTimeStringFormat.Substring(i, 1) == "/" || Env.Cfg.DateTimeStringFormat.Substring(i, 1) == ":" ? Env.Cfg.DateTimeStringFormat.Substring(i, 1) : Env.Cfg.DateTimeStringFormat.Substring(i, 1) == "s" ? string.Empty : " ";
                this.dateTimeMask += this.dtmask;
                this.emptyDTMask += this.empmask;
            }

            this.txtCutoffDate.DataBindings.Add(new Binding("Text", this.mtbs, "CutOffDate", true, DataSourceUpdateMode.OnValidation, this.emptyDTMask, Env.Cfg.DateTimeStringFormat));
            this.txtCutoffDate.Mask = this.dateTimeMask;
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            this.masterID = (e.Master == null) ? "1=0" : string.Format("p.INVNo = '{0}'", MyUtility.Convert.GetString(e.Master["ID"]));
            this.DetailSelectCommand = string.Format(
                @"
select  p.GMTBookingLock
        , FactoryID = STUFF ((select CONCAT (',',a.FactoryID) 
                            from (
                                select distinct o.FactoryID
                                from PackingList_Detail pd WITH (NOLOCK) 
								left join orders o WITH (NOLOCK) on o.id = pd.OrderID 
                                where pd.ID = p.id
                            ) a 
                            for xml path('')
                          ), 1, 1, '') 
		, p.ID
        , OrderID = STUFF ((select CONCAT (',', cast (a.OrderID as nvarchar)) 
                            from (
                                select pd.OrderID 
                                from PackingList_Detail pd WITH (NOLOCK) 
                                left join AirPP ap With (NoLock) on pd.OrderID = ap.OrderID
                                                                     and pd.OrderShipmodeSeq = ap.OrderShipmodeSeq
                                where pd.ID = p.id
                                group by pd.OrderID, pd.OrderShipmodeSeq, ap.ID
                            ) a 
							order by a.OrderID
                            for xml path('')
                          ), 1, 1, '') 
        , OrderShipmodeSeq = STUFF ((select CONCAT (',', cast (a.OrderShipmodeSeq as nvarchar)) 
                                     from (
                                         select pd.OrderShipmodeSeq 
                                         from PackingList_Detail pd WITH (NOLOCK) 
                                         left join AirPP ap With (NoLock) on pd.OrderID = ap.OrderID
                                                                              and pd.OrderShipmodeSeq = ap.OrderShipmodeSeq
                                         where pd.ID = p.id
                                         group by pd.OrderID, pd.OrderShipmodeSeq, ap.ID
                                     ) a 
                                     for xml path('')
                                   ), 1, 1, '') 
        , IDD = STUFF ((select distinct CONCAT (',', Format(oqs.IDD, 'yyyy/MM/dd')) 
                            from PackingList_Detail pd WITH (NOLOCK) 
                            inner join Order_QtyShip oqs with (nolock) on oqs.ID = pd.OrderID and oqs.Seq = pd.OrderShipmodeSeq
                            where pd.ID = p.id and oqs.IDD is not null
                            for xml path('')
                          ), 1, 1, '') 
		,[PONo]=STUFF ((select CONCAT (',',a.CustPONo) 
                            from (
                                select distinct o.CustPONo
                                from PackingList_Detail pd WITH (NOLOCK) 
								left join orders o WITH (NOLOCK) on o.id = pd.OrderID 
                                where pd.ID = p.id AND o.CustPONo<>'' AND o.CustPONo IS NOT NULL
                            ) a 
                            for xml path('')
                          ), 1, 1, '') 
        , AirPPID = STUFF ((select CONCAT (',', cast (a.ID as nvarchar)) 
                            from (
                                select ap.ID
                                from PackingList_Detail pd WITH (NOLOCK) 
                                left join AirPP ap With (NoLock) on pd.OrderID = ap.OrderID
                                                                    and pd.OrderShipmodeSeq = ap.OrderShipmodeSeq
                                where pd.ID = p.id
                                group by pd.OrderID, pd.OrderShipmodeSeq, ap.ID
                            ) a  WHERE a.ID IS NOT NULL
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
        , P.pulloutid
        , OrderQty = STUFF ((select CONCAT (',', cast (a.Qty as nvarchar)) 
                            from (
                                select distinct o.id,o.Qty
                                from PackingList_Detail pd WITH (NOLOCK) 
                                inner join orders o with(nolock) on o.id= pd.orderid
                                where pd.ID = p.id
                            ) a 
							order by a.id
                            for xml path('')
                          ), 1, 1, '') 
         , SewingOutputQty = STUFF ((select CONCAT (',', cast (sum(sod.qaqty) as nvarchar)) 
                            from (
                                select pd.OrderID
                                from PackingList_Detail pd WITH (NOLOCK) 
                                where pd.ID = p.id
                                group by pd.OrderID
                            ) a 
                            inner join SewingOutput_Detail_Detail sod with(nolock) on sod.orderid= a.orderid
							group by sod.OrderId
							order by sod.OrderId
                            for xml path('')
                          ), 1, 1, '')         , Pullout.sendtotpe
    ,pl2.APPBookingVW,pl2.APPEstAmtVW
from PackingList p WITH (NOLOCK) 
left join Pullout WITH (NOLOCK) on Pullout.id=p.Pulloutid
outer apply(
	select APPBookingVW = ISNULL(sum(p2.APPBookingVW),0) 
	,APPEstAmtVW = ISNULL(sum(p2.APPEstAmtVW),0)
	from PackingList_Detail p2
	where p2.ID=p.ID
) pl2
where {0}", this.masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            MyUtility.Tool.SetupCombox(this.comboContainerType, 1, 1, ",CY-CY,CFS-CY,CFS-CFS");
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();

            this.txtTerminalWhse.Text = MyUtility.GetValue.Lookup("WhseNo", MyUtility.Convert.GetString(this.CurrentMaintain["ForwarderWhse_DetailUKey"]), "ForwarderWhse_Detail", "UKey");
            this.displayBoxDeclarationID.Text = MyUtility.GetValue.Lookup("ID", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]), "VNExportDeclaration", "INVNo");
            this.displayBoxCustomsNo.Text = MyUtility.GetValue.Lookup("DeclareNo", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]), "VNExportDeclaration", "INVNo");
            this.btnRemark.Enabled = this.CurrentMaintain != null;
            this.btnRemark.ForeColor = !MyUtility.Check.Empty(this.CurrentMaintain["Remark"]) ? Color.Blue : Color.Black;

            #region AirPP List按鈕變色
            if (!this.EditMode)
            {
                string sqlCmd = string.Format(
                    @"select pd.ID
from PackingList p WITH (NOLOCK) , PackingList_Detail pd WITH (NOLOCK) , AirPP a WITH (NOLOCK) 
where p.INVNo = '{0}' and p.ID = pd.ID and a.OrderID = pd.OrderID and a.OrderShipmodeSeq = pd.OrderShipmodeSeq", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));

                if (MyUtility.Check.Seek(sqlCmd))
                {
                    this.btnAirPPList.ForeColor = Color.Red;
                    this.btnAirPPList.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold);
                }
                else
                {
                    this.btnAirPPList.ForeColor = Color.Black;
                    this.btnAirPPList.Font = new Font("Microsoft Sans Serif", 10F);
                }
            }
            #endregion

            #region S/O CFM按鈕權限
            if (!this.IsDetailInserting)
            {
                if (MyUtility.Convert.GetString(this.CurrentMaintain["Status"]) == "New")
                {
                    this.btnCFM.Enabled = true;
                }
                else
                {
                    this.btnCFM.Enabled = false;
                }

                if (MyUtility.Check.Empty(this.CurrentMaintain["SOCFMDate"]))
                {
                    this.btnCFM.Text = "CFM";
                }
                else
                {
                    this.btnCFM.Text = "Un CFM";
                }
            }
            else
            {
                this.btnCFM.Enabled = false;
            }
            #endregion

            #region Include Foundry : Enable = Foundry.Checked
            this.btnFoundryList.Enabled = this.chkFoundry.Checked;
            #endregion
            if (!MyUtility.Check.Empty(this.CurrentMaintain["FBDate"]))
            {
                this.FBDate_Ori = DateTime.Parse(this.CurrentMaintain["FBDate"].ToString());
            }
            else
            {
                this.FBDate_Ori = null;
            }

            this.txtPulloutPort1.BrandID = this.txtbrand.Text;
            this.txtPulloutPort1.ShipModeID = this.txtShipmodeShippingMode.SelectedValue;

            this.ControlColor();
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();

            this.shipqty.EditingMouseDoubleClick += (s, e) =>
                {
                    if (e.Button == MouseButtons.Left)
                    {
                        if (e.RowIndex != -1)
                        {
                            P05_QtyBreakDown callNextForm = new P05_QtyBreakDown(this.CurrentMaintain);
                            callNextForm.Set(false, this.DetailDatas, this.CurrentDetailData);
                            callNextForm.ShowDialog(this);
                        }
                    }
                };

            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("GMTBookingLock", header: "Lock", width: Widths.AnsiChars(1)).Get(out this.col_lock)
                .Text("FactoryID", header: "Factory#", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("ID", header: "Packing #", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("OrderID", header: "SP No.", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("OrderShipmodeSeq", header: "Seq", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("PONo", header: "PO No.", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("AirPPID", header: "APP#", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Date("BuyerDelivery", header: "Delivery", iseditingreadonly: true)
                .Text("IDD", header: "Intended Delivery", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("OrderQty", header: "Order Ttl Qty", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("SewingOutputQty", header: "Prod. Output Ttl Qty", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("PulloutID", header: "Pullout ID", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Date("PulloutDate", header: "Pull out Date", iseditingreadonly: true)
                .Date("SendToTPE", header: "Send to SCI", iseditingreadonly: true)
                .Numeric("ShipQty", header: "Q'ty", iseditingreadonly: true, settings: this.shipqty)
                .Numeric("CTNQty", header: "CTN Q'ty", iseditingreadonly: true)
                .Numeric("GW", header: "G.W.", decimal_places: 3, iseditingreadonly: true)
                .Numeric("CBM", header: "CBM", decimal_places: 3, iseditingreadonly: true)
                .Text("CustCDID", header: "CustCD", width: Widths.AnsiChars(16), iseditingreadonly: true)
                .Text("Dest", header: "Destination", width: Widths.AnsiChars(2), iseditingreadonly: true)
                .Numeric("NW", header: "ttl N.W.", decimal_places: 3, iseditingreadonly: true)
                .Numeric("NNW", header: "ttl N.N.W.", decimal_places: 3, iseditingreadonly: true)
                .Numeric("APPBookingVW", header: "V.W. for APP booking", decimal_places: 2, iseditingreadonly: true)
                .Numeric("APPEstAmtVW", header: "V.W for APP est. Amt", decimal_places: 2, iseditingreadonly: true)
                .Text("Status", header: "Status", width: Widths.AnsiChars(9), iseditingreadonly: true)
                .Numeric("ClogCTNQty", header: "CTN in C-Logs", iseditingreadonly: true)
                .Date("InspDate", header: "Est. Inspection date", iseditingreadonly: true)
                .Date("CargoReadyDate", header: "Cargo Ready Date").Get(out this.col_crd);

            #region 欄位值檢查
            this.detailgrid.CellValidating += (s, e) =>
                {
                    if (this.EditMode)
                    {
                        DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                        if (this.detailgrid.Columns[e.ColumnIndex].DataPropertyName == this.col_lock.DataPropertyName)
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
                                    string sqlCmd = string.Format(
                                        @"select p.ID
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

                        if (this.detailgrid.Columns[e.ColumnIndex].DataPropertyName == this.col_crd.DataPropertyName)
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

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["Status"] = "New";
            this.CurrentMaintain["InvDate"] = DateTime.Today;
            this.CurrentMaintain["Handle"] = Env.User.UserID;
            this.CurrentMaintain["ShipModeID"] = "SEA";
            this.CurrentMaintain["ShipTermID"] = "FOB";
        }

        /// <inheritdoc/>
        protected override bool ClickEditBefore()
        {
            #region  表身任一筆Orders.ID的Orders.GMTComplete 不可為 'S'
            bool gMTCompleteCheck = this.GMTCompleteCheck();
            if (!gMTCompleteCheck)
            {
                return false;
            }
            #endregion

            return base.ClickEditBefore();
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtInvSerial.ReadOnly = true;
            this.txtbrand.ReadOnly = true;
            this.txtCountryDestination.TextBox1.ReadOnly = true;
            this.txtShipmodeShippingMode.ReadOnly = true;

            if (!MyUtility.Check.Empty(this.CurrentMaintain["SOCFMDate"]))
            {
                this.dateInvDate.ReadOnly = true;
                this.txtfactoryShipper.ReadOnly = true;
                this.dateFCRDate.ReadOnly = true;
                this.txtCustCD.ReadOnly = true;
                this.txtUserHandle.TextBox1.ReadOnly = true;
                this.txtSubconForwarder.TextBox1.ReadOnly = true;
                this.comboContainerType.ReadOnly = true;
                this.txtSONo.ReadOnly = true;

                // textBox7.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.EditModeAndNonReadOnly;
                // textBox6.ReadOnly = true;
                this.col_lock.IsEditingReadOnly = true;
                this.col_crd.IsEditingReadOnly = true;
                this.detailgrid.Columns[0].DefaultCellStyle.ForeColor = Color.Black;
                this.detailgrid.Columns[4].DefaultCellStyle.ForeColor = Color.Black;
                this.btnImportfrompackinglist.Enabled = false;
                this.gridicon.Remove.Enabled = false;
                this.txtCutoffDate.ReadOnly = true;
                this.txtTerminalWhse.ReadOnly = true;
                this.txtDocumentRefNo.ReadOnly = true;
            }
            else
            {
                this.gridicon.Remove.Enabled = true;
                this.col_lock.IsEditingReadOnly = false;
                this.col_crd.IsEditingReadOnly = false;
                this.detailgrid.Columns[0].DefaultCellStyle.ForeColor = Color.Red;
                this.detailgrid.Columns[4].DefaultCellStyle.ForeColor = Color.Red;
                this.txtCutoffDate.ReadOnly = false;
                this.txtTerminalWhse.ReadOnly = false;

                // textBox7.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.EditModeAndReadOnly;
            }

            switch (this.CurrentMaintain["Status"].ToString().ToUpper())
            {
                case "NEW":
                    this.txtShipmodeShippingMode.ReadOnly = false;
                    break;
                case "CONFIRMED":
                    // Confirm後, 仍可以按[Edit] 編輯[No Export Charge]欄位
                    this.txtpaytermarPaymentTerm.TextBox1.ReadOnly = true;
                    this.txtShiptermShipmentTerm.ReadOnly = true;
                    this.txtPulloutPort1.TextBox1.ReadOnly = true;
                    this.dateInvDate.ReadOnly = true;
                    this.dateFCRDate.ReadOnly = true;
                    this.txtCustCD.ReadOnly = true;
                    this.txtUserHandle.TextBox1.ReadOnly = true;
                    this.txtSubconForwarder.TextBox1.ReadOnly = true;
                    this.comboContainerType.ReadOnly = true;
                    this.txtSONo.ReadOnly = true;
                    this.col_lock.IsEditingReadOnly = true;
                    this.col_crd.IsEditingReadOnly = true;
                    this.detailgrid.Columns[0].DefaultCellStyle.ForeColor = Color.Black;
                    this.detailgrid.Columns[4].DefaultCellStyle.ForeColor = Color.Black;
                    this.btnImportfrompackinglist.Enabled = false;
                    this.gridicon.Remove.Enabled = false;
                    this.btnRemark.Enabled = false;
                    this.txtCutoffDate.ReadOnly = true;
                    this.txtTerminalWhse.ReadOnly = true;
                    break;
            }
        }

        /// <inheritdoc/>
        protected override bool ClickDeleteBefore()
        {
            if (MyUtility.Convert.GetString(this.CurrentMaintain["Status"]) == "Confirmed")
            {
                MyUtility.Msg.WarningBox("This record is < Confirmed >, can't be deleted!");
                return false;
            }

            if (!MyUtility.Check.Empty(this.CurrentMaintain["SOCFMDate"]))
            {
                MyUtility.Msg.WarningBox("This record already S/O Confirmed, can't be deleted!");
                return false;
            }

            // 如果表身有資料就不可以delete
            if (((DataTable)this.detailgridbs.DataSource).Rows.Count > 0)
            {
                MyUtility.Msg.WarningBox("This record had Packing List. Can't be deleted!");
                return false;
            }

            // 已經有做出口費用分攤就不可以被刪除
            if (MyUtility.Check.Seek(string.Format(@"select ShippingAPID from ShareExpense WITH (NOLOCK) where InvNo = '{0}' and (Junk = 0 or Junk is null)", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]))))
            {
                MyUtility.Msg.WarningBox("This record have expense data, can't be deleted!");
                return false;
            }

            // 只要Pullout Report已經Confirmed就不可以被刪除
            string sqlCmd = string.Format(
                @"
select  distinct pl.PulloutID
from PackingList pl WITH (NOLOCK) 
     , Pullout p WITH (NOLOCK) 
where   pl.INVNo = '{0}'
        and pl.PulloutID = p.ID
        and p.Status = 'Confirmed'", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out this.selectData);
            if (result)
            {
                if (this.selectData.Rows.Count > 0)
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

        /// <inheritdoc/>
        protected override DualResult OnDeleteDetails()
        {
            IList<string> updateCmd = new List<string>();
            updateCmd.Add(string.Format("update PackingList set GMTBookingLock = '', INVNo = '', ShipPlanID = '' where INVNo = '{0}';", MyUtility.Convert.GetString(this.CurrentMaintain["ID"])));
            updateCmd.Add(string.Format("Delete GMTBooking_CTNR where ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["ID"])));
            updateCmd.Add(string.Format("Delete GMTBooking_History where ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["ID"])));

            DualResult result = DBProxy.Current.Executes(null, updateCmd);
            return result;
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            DualResult result;
            #region 檢查必輸欄位

            // [S/O#], [Terminal/Whse#], [Cut-Off Date]都輸入, 或是都不輸入
            if ((!MyUtility.Check.Empty(this.CurrentMaintain["SONo"]) || !MyUtility.Check.Empty(this.txtTerminalWhse.Text) || !MyUtility.Check.Empty(this.CurrentMaintain["CutOffDate"])) &&
                (MyUtility.Check.Empty(this.CurrentMaintain["SONo"]) || MyUtility.Check.Empty(this.txtTerminalWhse.Text) || MyUtility.Check.Empty(this.CurrentMaintain["CutOffDate"])))
            {
                MyUtility.Msg.WarningBox("GB can't be saved due to S/O Info. (S/O#, Terminal/Whse#, Cut-Off Date/Time) are not complete");
                return false;
            }

            // 當此三欄([S/O#], [Terminal/Whse#], [Cut-Off Date])皆有值，但[S/O Cfm Date]為空，保存時會跳出請示"GB can't be saved due to missing S/O Cfm Date"
            if (!MyUtility.Check.Empty(this.CurrentMaintain["SONo"]) && !MyUtility.Check.Empty(this.txtTerminalWhse.Text) && !MyUtility.Check.Empty(this.CurrentMaintain["CutOffDate"]) && MyUtility.Check.Empty(this.CurrentMaintain["SOCFMDate"]))
            {
                MyUtility.Msg.WarningBox("GB can't be saved due to missing S/O Cfm Date");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["InvSerial"]))
            {
                this.txtInvSerial.Focus();
                MyUtility.Msg.WarningBox("Inv. Serial can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["InvDate"]))
            {
                this.dateInvDate.Focus();
                MyUtility.Msg.WarningBox("Inv. Date can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["BrandID"]))
            {
                this.txtbrand.Focus();
                MyUtility.Msg.WarningBox("Brand can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["FCRDate"]))
            {
                this.dateFCRDate.Focus();
                MyUtility.Msg.WarningBox("FCR Date can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["CustCDID"]))
            {
                this.txtCustCD.Focus();
                MyUtility.Msg.WarningBox("CustCD can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["Dest"]))
            {
                this.txtCountryDestination.TextBox1.Focus();
                MyUtility.Msg.WarningBox("Destination can't empty!!");
                return false;
            }
            else
            {
                if (MyUtility.Convert.GetString(this.CurrentMaintain["Dest"]) == "ZZ")
                {
                    MyUtility.Msg.WarningBox("Destination cannot be 「Other」!!");
                    return false;
                }
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["PayTermARID"]))
            {
                this.txtpaytermarPaymentTerm.TextBox1.Focus();
                MyUtility.Msg.WarningBox("Payment Term can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["ShipModeID"]))
            {
                this.txtShipmodeShippingMode.Focus();
                MyUtility.Msg.WarningBox("Shipping Mode can't empty!!");
                return false;
            }
            else
            {
                List<string> shipmodeIDs = new List<string> { "SEA", "S-A/C", "S-A/P" };
                if (shipmodeIDs.Where(x => x.EqualString(this.CurrentMaintain["ShipModeID"])).ToList().Count > 0 &&
                    MyUtility.Check.Empty(this.CurrentMaintain["DischargePortID"]))
                {
                    this.txtPulloutPort1.Focus();
                    MyUtility.Msg.WarningBox("Port of Discharge can't empty!!");
                    return false;
                }
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["ShipTermID"]))
            {
                this.txtShiptermShipmentTerm.Focus();
                MyUtility.Msg.WarningBox("Shipment Term can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["Handle"]))
            {
                this.txtUserHandle.TextBox1.Focus();
                MyUtility.Msg.WarningBox("Handle can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["Forwarder"]))
            {
                this.txtSubconForwarder.TextBox1.Focus();
                MyUtility.Msg.WarningBox("Forwarder can't empty!!");
                return false;
            }

            if (!MyUtility.Check.Empty(this.CurrentMaintain["SOCFMDate"]))
            {
                if (this.IsKeyColumnEmpty())
                {
                    return false;
                }
            }

            if (!MyUtility.Check.Empty(this.CurrentMaintain["CYCFS"]))
            {
                string chkLoadingType = $@"select LoadingType from ShipMode where id = '{this.CurrentMaintain["ShipModeID"]}'";
                string loadingType = MyUtility.GetValue.Lookup(chkLoadingType);
                var x = loadingType.Split(',').ToList();
                if (!x.Contains(MyUtility.Convert.GetString(this.CurrentMaintain["CYCFS"])))
                {
                    MyUtility.Msg.WarningBox($"Shipmode {this.CurrentMaintain["ShipModeID"]} cannot choose loading type {this.CurrentMaintain["CYCFS"]}");
                    return false;
                }
            }

            string sqlCmd = string.Format(
            @"select fwd.WhseNo,fwd.address,fwd.UKey from ForwarderWhse fw WITH (NOLOCK) , ForwarderWhse_Detail fwd WITH (NOLOCK) 
where fw.ID = fwd.ID
and fw.BrandID = '{0}'
and fw.Forwarder = '{1}'
and fw.ShipModeID = '{2}'
and  fwd.WhseNo = '{3}'
order by fwd.WhseNo",
            MyUtility.Convert.GetString(this.CurrentMaintain["BrandID"]),
            MyUtility.Convert.GetString(this.CurrentMaintain["Forwarder"]),
            MyUtility.Convert.GetString(this.CurrentMaintain["ShipModeID"]),
            this.txtTerminalWhse.Text);

            if (!MyUtility.Check.Empty(this.txtTerminalWhse.Text))
            {
                if (!MyUtility.Check.Seek(sqlCmd, string.Empty))
                {
                    this.txtTerminalWhse.Focus();
                    MyUtility.Msg.WarningBox("Terminal/Whse# is not found!!");
                    return false;
                }
            }

            #endregion

            #region 檢查LocalSupp_Bank
            DualResult resultCheckLocalSupp_BankStatus = Prgs.CheckLocalSupp_BankStatus(this.CurrentMaintain["Forwarder"].ToString(), Prgs.CallFormAction.Save);
            if (!resultCheckLocalSupp_BankStatus)
            {
                return false;
            }
            #endregion

            if (!this.DischargePortIDCheck())
            {
                return false;
            }

            // 表身GMTBooking.ShipModeID 不存在Order_QtyShip 就return
            if (!this.CheckShipMode())
            {
                return false;
            }

            this.CheckIDD();

            #region 檢查Act FCR Date 不可晚於今日or早於一個月前
            if (!MyUtility.Check.Empty(this.CurrentMaintain["FBDate"]))
            {
                DateTime fBDate = DateTime.Parse(this.CurrentMaintain["FBDate"].ToString());
                if (this.FBDate_Ori != fBDate)
                {
                    if (DateTime.Compare(fBDate, DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd")).AddMonths(-1)) < 0 ||
                        DateTime.Compare(fBDate, DateTime.Now) > 0)
                    {
                        MyUtility.Msg.WarningBox("<Forward Booking Date> cannot be later than today or one month earlier !");
                        return false;
                    }
                }
            }

            #endregion

            #region 檢查Shipper
            if (!this.CheckShipper())
            {
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["Shipper"]))
            {
                this.txtfactoryShipper.Focus();
                MyUtility.Msg.WarningBox("Shipper can't empty!!");
                return false;
            }
            #endregion
            #region 若 No Export Charge 有勾選，同時 Expense Data 也有資料，
            if (MyUtility.Convert.GetBool(this.CurrentMaintain["NoExportCharges"]) && this.ControlColor())
            {
                MyUtility.Msg.WarningBox("This record have expense data, please double check.");
            }
            #endregion

            #region 新增單狀態下，取ID且檢查此ID是否存在
            if (this.IsDetailInserting)
            {
                string fac = MyUtility.GetValue.Lookup($@"
select top 1 Factoryid from PackingList_Detail pd WITH (NOLOCK) 
left join orders o WITH (NOLOCK) on o.id = pd.OrderID  where pd.id = '{this.DetailDatas[0]["ID"].ToString().Split(',')[0]}'
");
                string newID = MyUtility.GetValue.Lookup("NegoRegion", fac, "Factory", "ID").Trim() + Convert.ToDateTime(this.CurrentMaintain["InvDate"]).ToString("yyMM") + "-" + MyUtility.Convert.GetString(this.CurrentMaintain["InvSerial"]).Trim() + "-" + MyUtility.GetValue.Lookup("ShipCode", MyUtility.Convert.GetString(this.CurrentMaintain["BrandID"]), "Brand", "ID").Trim();
                if (MyUtility.Check.Seek(newID, "GMTBooking", "ID"))
                {
                    this.txtInvSerial.Focus();
                    MyUtility.Msg.WarningBox("Inv. Serial already exist!!");
                    return false;
                }

                this.CurrentMaintain["ID"] = newID;
            }
            #endregion

            #region 組出表身所有的PackingListID與加總ShipQty,CTNQty,NW,GW,NNW,CBM
            StringBuilder allPackID = new StringBuilder();
            int ttlshipqty = 0, ttlctnqty = 0;
            double ttlnw = 0.0, ttlgw = 0.0, ttlnnw = 0.0, ttlcbm = 0.0, ttlAPPBookingVW = 0.0, ttlAPPEstAmtVW = 0.0;
            foreach (DataRow dr in this.DetailDatas)
            {
                allPackID.Append(string.Format("'{0}',", MyUtility.Convert.GetString(dr["ID"])));
                ttlshipqty += MyUtility.Convert.GetInt(dr["ShipQty"]);
                ttlctnqty += MyUtility.Convert.GetInt(dr["CTNQty"]);
                ttlnw = MyUtility.Math.Round(ttlnw + MyUtility.Convert.GetDouble(dr["NW"]), 3);
                ttlgw = MyUtility.Math.Round(ttlgw + MyUtility.Convert.GetDouble(dr["GW"]), 3);
                ttlnnw = MyUtility.Math.Round(ttlnnw + MyUtility.Convert.GetDouble(dr["NNW"]), 3);
                ttlcbm = MyUtility.Math.Round(ttlcbm + MyUtility.Convert.GetDouble(dr["CBM"]), 4);
                ttlAPPBookingVW = MyUtility.Math.Round(ttlAPPBookingVW + MyUtility.Convert.GetDouble(dr["APPBookingVW"]), 2);
                ttlAPPEstAmtVW = MyUtility.Math.Round(ttlAPPEstAmtVW + MyUtility.Convert.GetDouble(dr["APPEstAmtVW"]), 2);
            }
            #endregion

            #region 檢查訂單的Currency是否一致與Payterm與表頭是否一致
            if (allPackID.Length > 0)
            {
                sqlCmd = string.Format(
                    @"with OrderData
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
                result = DBProxy.Current.Select(null, sqlCmd, out this.selectData);
                if (!result)
                {
                    MyUtility.Msg.ErrorBox(result.ToString());
                    return false;
                }

                if (this.selectData.Rows.Count > 0)
                {
                    StringBuilder msg = new StringBuilder();
                    foreach (DataRow dr in this.selectData.Select(string.Format("CurrencyID <> '{0}'", MyUtility.Convert.GetString(this.selectData.Rows[0]["CurrencyID"]))))
                    {
                        msg.Append(string.Format("Packing#:{0},   SP No.:{1},   Currency:{2}\r\n", MyUtility.Convert.GetString(dr["ID"]), MyUtility.Convert.GetString(dr["OrderID"]), MyUtility.Convert.GetString(dr["CurrencyID"])));
                    }

                    if (msg.Length > 0)
                    {
                        MyUtility.Msg.WarningBox("Currency is inconsistent!!\r\nThe currency of most SP No. is " + MyUtility.Convert.GetString(this.selectData.Rows[0]["CurrencyID"]) + "\r\n" + msg.ToString());
                        return false;
                    }

                    msg.Clear();
                    foreach (DataRow dr in this.selectData.Select(string.Format("PayTermARID <> '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["PayTermARID"]))))
                    {
                        msg.Append(string.Format("Packing#:{0},   Payment Term:{1}\r\n", MyUtility.Convert.GetString(dr["ID"]), MyUtility.Convert.GetString(dr["PayTermARID"])));
                    }

                    if (msg.Length > 0)
                    {
                        MyUtility.Msg.WarningBox("Payment term in detail SP is different from garment booking!!\r\n" + msg.ToString());
                    }
                }
            }
            #endregion

            #region 簡查 BrandID 是否與表頭一致
            string checkBrandSQL = string.Format(
                @"
select  Msg = Concat('The brand of <SP#> ', tmpSP.value, ' is not ', '{0}')
from #tmp
outer apply (
    select value = data
    from dbo.SplitString(#tmp.OrderID, ',') 
) tmpSP
left join Orders o on tmpSP.value = o.ID
where   o.BrandID is null
        or o.BrandID != '{0}'", this.txtbrand.Text);
            result = MyUtility.Tool.ProcessWithDatatable((DataTable)((BindingSource)this.detailgrid.DataSource).DataSource, string.Empty, checkBrandSQL, out DataTable checkBrandDt, "#tmp");
            if (!result)
            {
                MyUtility.Msg.WarningBox(result.Description);
                return false;
            }

            if (checkBrandDt != null && checkBrandDt.Rows.Count > 0)
            {
                StringBuilder errorMsg = new StringBuilder("Garment Booking cannot be created !!");
                foreach (DataRow checkBrandDr in checkBrandDt.Rows)
                {
                    errorMsg.Append(Environment.NewLine + checkBrandDr["Msg"]);
                }

                MyUtility.Msg.WarningBox(errorMsg.ToString());
                return false;
            }
            #endregion

            #region 組Description
            string season = string.Empty, category = string.Empty;
            if (allPackID.Length > 0)
            {
                sqlCmd = string.Format(
                    @"with OrderData
as
(select distinct o.Category,o.SeasonID
 from PackingList_Detail pd WITH (NOLOCK) , Orders o WITH (NOLOCK) 
 where pd.ID in ({0})
 and pd.OrderID = o.ID
)
select (select CAST(a.Category as nvarchar)+'/' from (select distinct Category from OrderData) a for xml path('')) as Category,
(select CAST(a.SeasonID as nvarchar)+'/' from (select distinct SeasonID from OrderData) a for xml path('')) as Season", allPackID.ToString().Substring(0, allPackID.Length - 1));
                result = DBProxy.Current.Select(null, sqlCmd, out this.selectData);
                if (!result)
                {
                    MyUtility.Msg.ErrorBox(result.ToString());
                    return false;
                }

                if (this.selectData.Rows.Count > 0)
                {
                    if (!MyUtility.Check.Empty(this.selectData.Rows[0]["Season"]))
                    {
                        season = MyUtility.Convert.GetString(this.selectData.Rows[0]["Season"]).Substring(0, MyUtility.Convert.GetString(this.selectData.Rows[0]["Season"]).Length - 1);
                        category = MyUtility.Convert.GetString(this.selectData.Rows[0]["Category"]).Substring(0, MyUtility.Convert.GetString(this.selectData.Rows[0]["Category"]).Length - 1);
                    }
                }
            }

            this.CurrentMaintain["Description"] = MyUtility.Convert.GetString(this.CurrentMaintain["BrandID"]) + ',' + season + ',' + MyUtility.Convert.GetString(this.CurrentMaintain["CustCDID"]) + "," + MyUtility.Convert.GetString(this.CurrentMaintain["Dest"]) + "," + category;
            #endregion

            // 將表身加總的資料回寫回表頭
            this.CurrentMaintain["TotalShipQty"] = ttlshipqty;
            this.CurrentMaintain["TotalCTNQty"] = ttlctnqty;
            this.CurrentMaintain["TotalNW"] = MyUtility.Math.Round(ttlnw, 3);
            this.CurrentMaintain["TotalGW"] = MyUtility.Math.Round(ttlgw, 3);
            this.CurrentMaintain["TotalNNW"] = MyUtility.Math.Round(ttlnnw, 3);
            this.CurrentMaintain["TotalCBM"] = MyUtility.Math.Round(ttlcbm, 4);
            this.CurrentMaintain["TotalAPPBookingVW"] = ttlAPPBookingVW;
            this.CurrentMaintain["TotalAPPEstAmtVW"] = ttlAPPEstAmtVW;
            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override DualResult OnSaveDetail(IList<DataRow> details, ITableSchema detailtableschema)
        {
            IList<string> updateCmds = new List<string>();

            foreach (DataRow dr in details)
            {
                if (dr.RowState == DataRowState.Modified)
                {
                    updateCmds.Add(string.Format("update PackingList set GMTBookingLock = '{0}' , ShipPlanID = '{2}' where ID = '{1}';", MyUtility.Convert.GetString(dr["GMTBookingLock"]), MyUtility.Convert.GetString(dr["ID"]), MyUtility.Convert.GetString(this.CurrentMaintain["ShipPlanID"])));
                }

                if (dr.RowState == DataRowState.Added)
                {
                    updateCmds.Add(string.Format("update PackingList set GMTBookingLock = '{0}', INVNo = '{1}', ShipPlanID = '{2}' where ID = '{3}';", MyUtility.Convert.GetString(dr["GMTBookingLock"]), MyUtility.Convert.GetString(this.CurrentMaintain["ID"]), MyUtility.Convert.GetString(this.CurrentMaintain["ShipPlanID"]), MyUtility.Convert.GetString(dr["ID"])));
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

            return Ict.Result.True;
        }

        /// <inheritdoc/>
        protected override bool ClickPrint()
        {
            string strXltName = Env.Cfg.XltPathDir + "\\Shipping_P05.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return false;
            }

            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];
            worksheet.Cells[1, 1] = MyUtility.Convert.GetString(this.CurrentMaintain["Shipper"]);
            worksheet.Cells[3, 2] = MyUtility.Convert.GetString(this.CurrentMaintain["ID"]);
            worksheet.Cells[4, 2] = MyUtility.Convert.GetString(this.CurrentMaintain["InvSerial"]);
            worksheet.Cells[5, 2] = MyUtility.Check.Empty(this.CurrentMaintain["InvDate"]) ? string.Empty : Convert.ToDateTime(this.CurrentMaintain["InvDate"]).ToString("d");
            worksheet.Cells[6, 2] = MyUtility.Convert.GetString(this.CurrentMaintain["Shipper"]);
            worksheet.Cells[7, 2] = MyUtility.Convert.GetString(this.CurrentMaintain["BrandID"]);
            worksheet.Cells[8, 2] = MyUtility.Check.Empty(this.CurrentMaintain["FCRDate"]) ? string.Empty : Convert.ToDateTime(this.CurrentMaintain["FCRDate"]).ToString("d");
            worksheet.Cells[9, 2] = MyUtility.Convert.GetString(this.CurrentMaintain["CustCDID"]);
            worksheet.Cells[10, 2] = MyUtility.Convert.GetString(this.CurrentMaintain["PayTermARID"]) + " - " + MyUtility.GetValue.Lookup(string.Format("select Description from PayTermAR WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["PayTermARID"])));
            worksheet.Cells[11, 2] = MyUtility.Convert.GetString(this.CurrentMaintain["Description"]);
            worksheet.Cells[12, 2] = MyUtility.Convert.GetString(this.CurrentMaintain["Remark"]);

            worksheet.Cells[3, 5] = MyUtility.Convert.GetString(this.CurrentMaintain["Dest"]) + " - " + MyUtility.GetValue.Lookup(string.Format("select NameEN from Country WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["Dest"])));
            worksheet.Cells[4, 5] = MyUtility.Convert.GetString(this.CurrentMaintain["ShipModeID"]);
            worksheet.Cells[5, 5] = MyUtility.Convert.GetString(this.CurrentMaintain["ShipTermID"]);
            worksheet.Cells[6, 5] = MyUtility.Convert.GetInt(this.CurrentMaintain["TotalShipQty"]);
            worksheet.Cells[7, 5] = MyUtility.Convert.GetInt(this.CurrentMaintain["TotalCTNQty"]);
            worksheet.Cells[8, 5] = MyUtility.Convert.GetDecimal(this.CurrentMaintain["TotalGW"]);
            worksheet.Cells[9, 5] = MyUtility.Convert.GetDecimal(this.CurrentMaintain["TotalCBM"]);
            worksheet.Cells[10, 5] = MyUtility.Convert.GetDecimal(this.CurrentMaintain["TotalNW"]);
            worksheet.Cells[11, 5] = MyUtility.Convert.GetDecimal(this.CurrentMaintain["TotalNNW"]);

            worksheet.Cells[3, 9] = MyUtility.Convert.GetString(this.CurrentMaintain["Handle"]) + "  " + MyUtility.GetValue.Lookup(string.Format("select Name from Pass1 WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["Handle"])));
            worksheet.Cells[4, 9] = MyUtility.Convert.GetString(this.CurrentMaintain["Forwarder"]) + "  " + MyUtility.GetValue.Lookup(string.Format("select Name from LocalSupp WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["Forwarder"])));
            worksheet.Cells[5, 9] = MyUtility.Convert.GetString(this.CurrentMaintain["CYCFS"]);
            worksheet.Cells[6, 9] = MyUtility.Convert.GetString(this.CurrentMaintain["SONo"]);
            worksheet.Cells[7, 9] = MyUtility.GetValue.Lookup("WhseNo", MyUtility.Convert.GetString(this.CurrentMaintain["ForwarderWhse_DetailUKey"]), "ForwarderWhse_Detail", "UKey");
            worksheet.Cells[8, 9] = MyUtility.Check.Empty(this.CurrentMaintain["CutOffDate"]) ? string.Empty : Convert.ToDateTime(this.CurrentMaintain["CutOffDate"]).ToString("yyyy/MM/dd HH:mm:ss");
            worksheet.Cells[9, 9] = MyUtility.Check.Empty(this.CurrentMaintain["SOCFMDate"]) ? string.Empty : Convert.ToDateTime(this.CurrentMaintain["SOCFMDate"]).ToString("d");
            worksheet.Cells[10, 9] = MyUtility.Convert.GetString(this.CurrentMaintain["Vessel"]);
            worksheet.Cells[11, 9] = MyUtility.Check.Empty(this.CurrentMaintain["ETD"]) ? string.Empty : Convert.ToDateTime(this.CurrentMaintain["ETD"]).ToString("d");
            worksheet.Cells[12, 9] = MyUtility.Check.Empty(this.CurrentMaintain["ETA"]) ? string.Empty : Convert.ToDateTime(this.CurrentMaintain["ETA"]).ToString("d");

            int intRowsStart = 14;
            DataTable gridData = (DataTable)this.detailgridbs.DataSource;
            int dataRowCount = gridData.Rows.Count;
            object[,] objArray = new object[1, 13];
            for (int i = 0; i < dataRowCount; i++)
            {
                DataRow dr = gridData.Rows[i];
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
                worksheet.Range[string.Format("A{0}:M{0}", rownum)].Value2 = objArray;
            }

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("Shipping_P05");
            excel.ActiveWorkbook.SaveAs(strExcelName);
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(worksheet);

            strExcelName.OpenFile();
            #endregion
            return base.ClickPrint();
        }

        // Inv. Serial:移除空白值
        private void TxtInvSerial_Validated(object sender, EventArgs e)
        {
            this.txtInvSerial.Text = this.txtInvSerial.Text.ToString().Replace(" ", string.Empty);
            this.CurrentMaintain["InvSerial"] = MyUtility.Convert.GetString(this.CurrentMaintain["InvSerial"]).Replace(" ", string.Empty);
        }

        // 檢查輸入的Inv. Date是否正確
        private void DateInvDate_Validating(object sender, CancelEventArgs e)
        {
            if (this.EditMode && !MyUtility.Check.Empty(this.dateInvDate.Value))
            {
                if (this.dateInvDate.Value > DateTime.Today.AddDays(180) || this.dateInvDate.Value < DateTime.Today.AddDays(-180))
                {
                    this.dateInvDate.Value = null;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox("< Inv. Date > is invalid, it exceeds +/-180 days!!");
                    return;
                }
            }
        }

        // 輸入Brand後自動帶出Payment Term
        private void Txtbrand_Validated(object sender, EventArgs e)
        {
            if (this.EditMode && this.txtbrand.OldValue != this.txtbrand.Text)
            {
                this.GetPaytermAP();
                this.txtPulloutPort1.BrandID = this.txtbrand.Text;
            }
        }

        // 檢查輸入的FCR Date是否正確
        private void DateFCRDate_Validating(object sender, CancelEventArgs e)
        {
            if (this.EditMode && !MyUtility.Check.Empty(this.dateFCRDate.Value))
            {
                if (!this.CheckDate((DateTime)MyUtility.Convert.GetDate(this.dateFCRDate.Value), -12, 12))
                {
                    this.dateFCRDate.Value = null;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox("< FCR Date > is invalid!!");
                    return;
                }

                // 新增單時，自動將FCR Date寫入Inv. Date欄位
                if (MyUtility.Check.Empty(this.CurrentMaintain["ID"]))
                {
                    this.CurrentMaintain["FCRDate"] = this.dateFCRDate.Value;
                    this.CurrentMaintain["InvDate"] = this.dateFCRDate.Value;
                }
            }
        }

        // CustCD按右鍵
        private void TxtCustCD_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            if (this.EditMode)
            {
                Win.Tools.SelectItem item;
                if (MyUtility.Check.Empty(this.CurrentMaintain["ID"]))
                {
                    item = new Win.Tools.SelectItem(string.Format("select ID, CountryID, City from CustCD WITH (NOLOCK) where BrandID = '{0}' and junk = 0 order by ID", MyUtility.Convert.GetString(this.CurrentMaintain["BrandID"])), "17,3,17", this.txtCustCD.Text);
                    DialogResult returnResult = item.ShowDialog();
                    if (returnResult == DialogResult.Cancel)
                    {
                        return;
                    }

                    this.txtCustCD.Text = item.GetSelectedString();
                }
                else
                {
                    item = new Win.Tools.SelectItem(string.Format("select ID, CountryID, City from CustCD WITH (NOLOCK) where BrandID = '{0}' and CountryID = '{1}' and junk = 0 order by ID", MyUtility.Convert.GetString(this.CurrentMaintain["BrandID"]), MyUtility.Convert.GetString(this.CurrentMaintain["Dest"])), "17,3,17", this.txtCustCD.Text);
                    DialogResult returnResult = item.ShowDialog();
                    if (returnResult == DialogResult.Cancel)
                    {
                        return;
                    }

                    this.txtCustCD.Text = item.GetSelectedString();
                }
            }

            this.txtCustCD.ValidateControl();
        }

        // 檢查輸入的CustCD是否正確
        private void TxtCustCD_Validating(object sender, CancelEventArgs e)
        {
            if (this.EditMode && !MyUtility.Check.Empty(this.txtCustCD.Text) && this.txtCustCD.OldValue != this.txtCustCD.Text)
            {
                if (!MyUtility.Check.Empty(this.CurrentMaintain["BrandID"]))
                {
                    // sql參數
                    System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter("@brandid", MyUtility.Convert.GetString(this.CurrentMaintain["BrandID"]));
                    System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter("@custcdid", this.txtCustCD.Text);

                    IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                    cmds.Add(sp1);
                    cmds.Add(sp2);

                    string sqlCmd = "select ID, CountryID, City from CustCD WITH (NOLOCK) where BrandID = @brandid and ID = @custcdid order by ID";
                    DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out DataTable custCDData);
                    if (!result || custCDData.Rows.Count <= 0)
                    {
                        if (!result)
                        {
                            MyUtility.Msg.WarningBox("Sql connection fail!!\r\n" + result.ToString());
                        }
                        else
                        {
                            MyUtility.Msg.WarningBox(string.Format("< CustCD: {0} > not found!!!", this.txtCustCD.Text));
                        }

                        this.txtCustCD.Text = string.Empty;
                        e.Cancel = true;
                        return;
                    }
                    else
                    {
                        this.CurrentMaintain["CustCDID"] = this.txtCustCD.Text;
                        this.GetPaytermAP();
                    }
                }
            }
        }

        private void TxtCustCD_Validated(object sender, EventArgs e)
        {
            if (this.EditMode && !MyUtility.Check.Empty(this.txtCustCD.Text) && this.txtCustCD.OldValue != this.txtCustCD.Text)
            {
                this.CurrentMaintain["Dest"] = MyUtility.GetValue.Lookup(string.Format("SELECT CountryID FROM CustCD WITH (NOLOCK) WHERE BrandID = '{0}' AND ID = '{1}'", MyUtility.Convert.GetString(this.CurrentMaintain["BrandID"]), this.txtCustCD.Text));
            }
        }

        // 自動帶出PaytermARID
        private void GetPaytermAP()
        {
            if (MyUtility.Check.Empty(this.txtbrand.Text))
            {
                this.txtpaytermarPaymentTerm.TextBox1.Text = string.Empty;
            }
            else
            {
                string paytermAR;
                if (MyUtility.Check.Empty(this.txtCustCD.Text))
                {
                    paytermAR = MyUtility.GetValue.Lookup("PayTermARIDBulk", this.txtbrand.Text, "Brand", "ID");
                }
                else
                {
                    paytermAR = MyUtility.GetValue.Lookup(string.Format("select PayTermARIDBulk from CustCD WITH (NOLOCK) where BrandID = '{0}' and ID = '{1}'", this.txtbrand.Text, this.txtCustCD.Text));
                }

                if (paytermAR != string.Empty)
                {
                    this.CurrentMaintain["PayTermARID"] = paytermAR;
                }
            }
        }

        // 檢查輸入的Cut-off Date是否正確
        private void TxtCutoffDate_Validating(object sender, CancelEventArgs e)
        {
            string oldvalue = this.txtCutoffDate.OldValue;
            if (this.txtCutoffDate.Text != this.txtCutoffDate.OldValue && !MyUtility.Check.Empty(this.CurrentMaintain["SOCFMDate"]))
            {
                if (!this.UpSOCFMDate())
                {
                    this.txtCutoffDate.Text = oldvalue;
                    return;
                }
            }

            if (this.txtCutoffDate.Text == "/  /     :  :")
            {
                this.txtCutoffDate.Text = string.Empty;
                this.CurrentMaintain["CutoffDate"] = DBNull.Value;
                this.CurrentMaintain.EndEdit();
                return;
            }

            if (this.EditMode && this.txtCutoffDate.Text != this.emptyDTMask)
            {
                string cutOffDate = this.txtCutoffDate.Text.Substring(0, 10).Replace(" ", "1");
                try
                {
                    if (!this.CheckDate((DateTime)MyUtility.Convert.GetDate(cutOffDate), -12, 12))
                    {
                        this.txtCutoffDate.Text = null;
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox("< Cut-off Date > is invalid!!");
                        return;
                    }
                }
                catch (Exception)
                {
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox("< Cut-off Date > is invalid!!");
                    return;
                }
            }
        }

        private bool CheckDate(DateTime dT, int before, int after)
        {
            if (dT > DateTime.Today.AddMonths(after) || dT < DateTime.Today.AddMonths(before))
            {
                return false;
            }

            return true;
        }

        /// <inheritdoc/>
        protected override void OnDetailGridDelete()
        {
            // 檢查此筆記錄的Pullout Report是否已經Confirmed，若是則出訊息告知且無法刪除
            if (this.DetailDatas.Count > 0)
            {
                if (MyUtility.GetValue.Lookup(string.Format("select p.Status from Pullout p WITH (NOLOCK) , PackingList pl WITH (NOLOCK) where pl.ID = '{0}' and p.ID = pl.PulloutID", MyUtility.Convert.GetString(this.CurrentDetailData["ID"]))) == "Confirmed")
                {
                    MyUtility.Msg.WarningBox("Pullout report already confirmed, can't be deleted!");
                    return;
                }

                if (!MyUtility.Check.Empty(this.CurrentDetailData["PulloutDate"]))
                {
                    MyUtility.Msg.WarningBox(string.Format("Pullout Date of Pcaking No: {0} not empty, can't delete!", this.CurrentDetailData["id"]));
                    return;
                }
            }

            base.OnDetailGridDelete();
        }

        // AirPP List
        private void BtnAirPPList_Click(object sender, EventArgs e)
        {
            P05_AirPrePaidList callNextForm = new P05_AirPrePaidList(MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));
            callNextForm.ShowDialog(this);
        }

        // Expense Data
        private void BtnExpenseData_Click(object sender, EventArgs e)
        {
            P05_ExpenseData callNextForm = new P05_ExpenseData(MyUtility.Convert.GetString(this.CurrentMaintain["ID"]), "InvNo", false);
            callNextForm.ShowDialog(this);
        }

        // S/O Confirm History
        private void BtnH_Click(object sender, EventArgs e)
        {
            Win.UI.ShowHistory callNextForm = new Win.UI.ShowHistory("GMTBooking_History", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]), "SOCFMDate", reasonType: "GMTBooking_SO", caption: "S/O Revised History");
            callNextForm.ShowDialog(this);
        }

        // AirPPStatus
        private void BtnAirPPStatus_Click(object sender, EventArgs e)
        {
            P05_AirPrePaidStatus callNextForm = new P05_AirPrePaidStatus();
            callNextForm.ShowDialog(this);
        }

        // S/O Confirm/UnConfirm
        private void BtnCFM_Click(object sender, EventArgs e)
        {
            #region  表身任一筆Orders.ID的Orders.GMTComplete 不可為 'S'
            bool gMTCompleteCheck = this.GMTCompleteCheck();
            if (!gMTCompleteCheck)
            {
                return;
            }
            #endregion

            if (MyUtility.Check.Empty(this.CurrentMaintain["SOCFMDate"]))
            {
                this.CheckIDD();
                if (this.IsKeyColumnEmpty())
                {
                    return;
                }

                // 檢查表身的ShipMode與表頭的ShipMode如果不同就不可以Confirm
                if (!this.CheckShipMode())
                {
                    return;
                }

                P05_SOCFMDate cfmBox = new P05_SOCFMDate(this.CurrentMaintain);
                cfmBox.ShowDialog();
            }
            else
            {
                if (!this.UpSOCFMDate())
                {
                    return;
                }
            }

            this.ReloadSOCFMDate();
            this.RenewData();
            this.OnDetailEntered();
        }

        // 檢查表身的ShipMode與表頭的ShipMode要相同 & ShipModeID 不存在Order_QtyShip 就return
        private bool CheckShipMode()
        {
            StringBuilder msg = new StringBuilder();

            var dtShipMode = ((DataTable)this.detailgridbs.DataSource).AsEnumerable().Where(s => s.RowState != DataRowState.Deleted);
            if (dtShipMode == null || dtShipMode.Count() == 0)
            {
                return true;
            }

            DualResult result;
            string strSql;
            foreach (DataRow dr in dtShipMode)
            {
                #region 檢查Packing List 的ship mode
                strSql = $"select ShipModeID from PackingList with (nolock) where ID = '{dr["ID"]}' and ShipModeID <> '{this.CurrentMaintain["ShipModeID"].ToString()}'";
                bool isPackListShipModeInconsistent = MyUtility.Check.Seek(strSql, out DataRow drPackingShipModeCheckResult);
                if (isPackListShipModeInconsistent)
                {
                    msg.Append(string.Format("Packing#:{0},   Shipping Mode:{1}\r\n", MyUtility.Convert.GetString(dr["ID"]), MyUtility.Convert.GetString(drPackingShipModeCheckResult["ShipModeID"])));
                    continue;
                }
                #endregion

                #region 檢查Order_QtyShip 的ship mode
                strSql = $@"
select distinct oq.ID,oq.Seq,oq.ShipmodeID
from PackingList p  with (nolock)
inner join PackingList_Detail pd with (nolock) on p.ID=pd.ID
inner join Order_QtyShip oq with (nolock) on oq.id = pd.OrderID and oq.Seq = pd.OrderShipmodeSeq
inner join Orders o with (nolock) on oq.ID = o.ID
where p.id='{dr["ID"]}' and p.ShipModeID  <> oq.ShipmodeID and o.Category <> 'S'
";
                result = DBProxy.Current.Select(null, strSql, out DataTable dtCheckResult);
                if (!result)
                {
                    this.ShowErr(result);
                    return result;
                }

                if (dtCheckResult.Rows.Count > 0)
                {
                    foreach (DataRow drError in dtCheckResult.Rows)
                    {
                        msg.Append($"Order ID:{drError["ID"]},   Seq{drError["Seq"]},   Shipping Mode:{drError["ShipmodeID"]}\r\n");
                    }
                }
                #endregion
            }

            if (msg.Length > 0)
            {
                MyUtility.Msg.WarningBox("Shipping mode is inconsistent!!\r\n" + msg.ToString());
                return false;
            }

            return true;
        }

        // Import from packing list
        private void BtnImportfrompackinglist_Click(object sender, EventArgs e)
        {
            // Brand, CustCD, Destination, Ship Mode不可以為空
            if (MyUtility.Check.Empty(this.CurrentMaintain["BrandID"]))
            {
                this.txtbrand.Focus();
                MyUtility.Msg.WarningBox("< Brand > can't empty!");
                return;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["CustCDID"]))
            {
                this.txtCustCD.Focus();
                MyUtility.Msg.WarningBox("< CustCD > can't empty!");
                return;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["Dest"]))
            {
                this.txtCountryDestination.TextBox1.Focus();
                MyUtility.Msg.WarningBox("< Destination > can't empty!");
                return;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["ShipModeID"]))
            {
                this.txtShipmodeShippingMode.Focus();
                MyUtility.Msg.WarningBox("< Shipping Mode > can't empty!");
                return;
            }

            P05_ImportFromPackingList callNextForm = new P05_ImportFromPackingList(this.CurrentMaintain, (DataTable)this.detailgridbs.DataSource);
            callNextForm.ShowDialog(this);
        }

        /// <inheritdoc/>
        protected override void ClickConfirm()
        {
            base.ClickConfirm();

            #region 檢查LocalSupp_Bank
            DualResult resultCheckLocalSupp_BankStatus = Prgs.CheckLocalSupp_BankStatus(this.CurrentMaintain["Forwarder"].ToString(), Prgs.CallFormAction.Confirm);
            if (!resultCheckLocalSupp_BankStatus)
            {
                return;
            }
            #endregion

            #region  表身任一筆Orders.ID的Orders.GMTComplete 不可為 'S'
            bool gMTCompleteCheck = this.GMTCompleteCheck();
            if (!gMTCompleteCheck)
            {
                return;
            }
            #endregion

            if (MyUtility.Check.Empty(this.CurrentMaintain["CYCFS"]))
            {
                MyUtility.Msg.WarningBox("Please input <Loading  Type> first!");
                return;
            }

            // 還沒做S/O CFM的話，不可以Confrim
            if (MyUtility.Check.Empty(this.CurrentMaintain["SOCFMDate"]))
            {
                MyUtility.Msg.WarningBox("S/O not yet confirmed, can't confirm!");
                return;
            }

            // Inv. Date不可晚於FCR Date
            if (Convert.ToDateTime(this.CurrentMaintain["InvDate"]) > Convert.ToDateTime(this.CurrentMaintain["FCRDate"]))
            {
                MyUtility.Msg.WarningBox("< Inv. Date > can't exceed < FCR Date >!");
                return;
            }

            if (this.IsKeyColumnEmpty())
            {
                return;
            }

            // 檢查表身的ShipMode與表頭的ShipMode如果不同 & ShipModeID 不存在Order_QtyShip
            // 就不可以Confirm
            if (!this.CheckShipMode())
            {
                return;
            }

            this.CheckIDD();

            if (MyUtility.Convert.GetString(this.CurrentMaintain["ShipModeID"]) == "A/P" ||
                MyUtility.Convert.GetString(this.CurrentMaintain["ShipModeID"]) == "S-A/P" ||
                MyUtility.Convert.GetString(this.CurrentMaintain["ShipModeID"]) == "E/P")
            {
                DataTable tmp = (DataTable)this.detailgridbs.DataSource;
                string sqlcmdchk = $@"
SELECT AirPP.Forwarder,t.id
From #tmp t
inner join PackingList_Detail pd with(nolock) on pd.id = t.id
inner join AirPP with(nolock) on AirPP.OrderID = pd.OrderID and AirPP.OrderShipmodeSeq = pd.OrderShipmodeSeq
";
                DualResult dualResult = MyUtility.Tool.ProcessWithDatatable(tmp, string.Empty, sqlcmdchk, out DataTable dt);
                if (!dualResult)
                {
                    this.ShowErr(dualResult);
                }

                if (dt.Rows.Count > 0)
                {
                    List<string> packingListID = dt.AsEnumerable().Where(w => MyUtility.Convert.GetString(w["Forwarder"]) != MyUtility.Convert.GetString(this.CurrentMaintain["Forwarder"])).Select(s => MyUtility.Convert.GetString(s["id"])).Distinct().ToList();
                    if (packingListID.Count > 0)
                    {
                        string pid = string.Join(",", packingListID);
                        string msg = $@"Forwarder is different from APP request, please double check.
Packing List : {pid}";
                        MyUtility.Msg.WarningBox(msg);
                    }
                }
            }

            if (!this.CheckShipper(false))
            {
                return;
            }

            // 有Cancel Order 不能confirmed
            string errmsg = Prgs.ChkCancelOrder(this.CurrentMaintain["id"].ToString());
            if (!MyUtility.Check.Empty(errmsg))
            {
                MyUtility.Msg.WarningBox(errmsg);
                return;
            }

            // shipper 不可為空
            if (MyUtility.Check.Empty(this.CurrentMaintain["Shipper"]))
            {
                this.txtfactoryShipper.Focus();
                MyUtility.Msg.WarningBox("Shipper can't empty!!");
                return;
            }

            if (!Prgs.CheckExistsOrder_QtyShip_Detail(iNVNo: MyUtility.Convert.GetString(this.CurrentMaintain["ID"])))
            {
                return;
            }

            DualResult result;

            // 當ShipMode為A/P,A/P-C,E/P,S-A/P時，要檢查是否都有AirPP單號
            if (MyUtility.Check.Seek(string.Format("select ID from ShipMode WITH (NOLOCK) where UseFunction like '%AirPP%' and ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["ShipModeID"]))))
            {
                string sqlCmd = string.Format(
                    @";with AirPPChk as (
select p.OrderID,p.OrderShipmodeSeq,p.ID, isnull(a.ID,'') as AirPPID,o.Category,isnull(a.status,'') as status
from (Select distinct a.ID,b.OrderID,b.OrderShipmodeSeq 
      from PackingList a WITH (NOLOCK) , PackingList_Detail b WITH (NOLOCK) 
      where a.INVNo = '{0}' and a.ID = b.ID) p
left join orders o WITH (NOLOCK) on o.ID = p.OrderID
left join AirPP a WITH (NOLOCK) on p.OrderID = a.OrderID and p.OrderShipmodeSeq = a.OrderShipmodeSeq),
AirPPChk_JunkChk as (
select *,
--相同SP + seq 有一筆以上，若其中一筆是Junked，這筆就不做判斷
[ShowFlag] = (select iif((count(*) - sum(iif(status = 'Junked',1,0))) > 0 and apc.status = 'Junked',0,1) from AirPPChk where orderid = apc.OrderID and OrderShipmodeSeq = apc.OrderShipmodeSeq )
 from AirPPChk apc
)
select apc.ID as PackingID, apc.AirPPID,apc.Category,apc.status as AirPPStatus,aps.status as StatusDesc,aps.Followup
from AirPPChk_JunkChk apc
left join AirPPStatus aps WITH (NOLOCK) on apc.status = isnull(aps.AirPPStatus,'')
where apc.status <> 'Locked' and ShowFlag = 1 ", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));
                result = DBProxy.Current.Select(null, sqlCmd, out this.selectData);
                if (result)
                {
                    // 如果此SP的 Category='S' 且 shipmode='E/P'時, 沒有AirPP# , 也可以Confirm，這邊不check category = 'S'
                    // Issue ISP20180033 Lock才能被Confirm
                    DataRow[] row;
                    if (this.CurrentMaintain["ShipModeID"].Equals("E/P"))
                    {
                        row = this.selectData.Select(" Category <> 'S' ");
                    }
                    else
                    {
                        row = this.selectData.Select();
                    }

                    if (row.Length > 0)
                    {
                        StringBuilder airPP_err = new StringBuilder();
                        airPP_err.Append("DO NOT arrange Air-Prepaid shipment due to APP# is not on GM Team Locked status." + Environment.NewLine + Environment.NewLine);
                        foreach (DataRow dr in row)
                        {
                            airPP_err.Append($"Packing#{dr["PackingID"]} APP#{dr["AirPPID"]} Status: {dr["StatusDesc"]} - {dr["Followup"]}" + Environment.NewLine);
                        }

                        airPP_err.Append(Environment.NewLine + @"If Shipping arrange Air-Prepaid shipment before GM Team Lock, PPIC and Shipping Dept. have to take the responsibility for the Air-Prepaid. 
Please follow up based on the Air-Prepaid Status.");
                        P05_ErrorMsg errMsg = new P05_ErrorMsg(airPP_err.ToString());
                        errMsg.ShowDialog();
                        return;
                    }
                }
                else
                {
                    MyUtility.Msg.ErrorBox(result.ToString());
                    return;
                }
            }

            // Check PackingList 全部是否都Confirmed
            DualResult resultPkl;
            if (resultPkl = DBProxy.Current.Select(null, string.Format(@"select * from PackingList WITH (NOLOCK) where  invno='{0}' ", MyUtility.Convert.GetString(this.CurrentMaintain["ID"])), out this.selectData))
            {
                if (this.selectData.Rows.Count > 0)
                {
                    DataRow[] row = this.selectData.Select("status<>'Confirmed'");
                    StringBuilder mSG = new StringBuilder();
                    if (row.Length > 0)
                    {
                        foreach (DataRow dr in row)
                        {
                            mSG.Append(string.Format("Packing NO : {0}\n\r", dr["ID"]));
                        }

                        MyUtility.Msg.WarningBox(@"PackingList not yet confirmed,please confirm listed below first!! " + mSG.ToString());
                        return;
                    }
                }
                else
                {
                    StringBuilder msg1 = new StringBuilder();
                    msg1.Append(string.Format("InvoNO: {0}", MyUtility.Convert.GetString(this.CurrentMaintain["ID"])));
                    MyUtility.Msg.WarningBox("InvoNo doesn't exist in Packing List, can't confirm!" + msg1.ToString());
                    return;
                }
            }
            else
            {
                MyUtility.Msg.ErrorBox(resultPkl.ToString());
                return;
            }

            // TotalCBM重新計算
            string updateCmd = string.Format(
@"update a 
set a.TotalCBM = b.TotalCBM, Status = 'Confirmed', EditName = '{0}', EditDate = GETDATE()
from GMTBooking a
inner join (
    select b.INVNo, sum(CBM) TotalCBM
    from PackingList b
    where b.INVNo = '{1}'
    group by b.INVNo
)b on a.id = b.INVNo
where ID = '{1}'",
Env.User.UserID,
MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));
            DualResult result1 = DBProxy.Current.Execute(null, updateCmd);
            if (!result1)
            {
                MyUtility.Msg.ErrorBox("Confirm fail !\r\n" + result1.ToString());
                return;
            }
        }

        /// <inheritdoc/>
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();

            string sqlchk = $@"select 1 from BIRInvoice  where ExVoucherID <>'' and id = '{this.CurrentMaintain["BIRID"]}'";
            if (MyUtility.Check.Seek(sqlchk))
            {
                MyUtility.Msg.WarningBox("Cannot unconfirm because already created voucher no");
                return;
            }

            // Ship plan已經Confirm就不可以做Unconfirm
            if (MyUtility.GetValue.Lookup("Status", MyUtility.Convert.GetString(this.CurrentMaintain["ShipPlanID"]), "ShipPlan", "ID") == "Confirmed")
            {
                MyUtility.Msg.WarningBox("Ship Plan already confirmed, can't unconfirm!");
                return;
            }

            // 問是否要做Unconfirm，確定才繼續往下做
            DialogResult buttonResult = MyUtility.Msg.WarningBox("Are you sure you want to < Unconfirm > this data?", "Warning", MessageBoxButtons.YesNo);
            if (buttonResult == DialogResult.No)
            {
                return;
            }

            Win.UI.SelectReason callReason = new Win.UI.SelectReason("GMTBooking_UnCFM", true);
            DialogResult dResult = callReason.ShowDialog(this);
            if (dResult == DialogResult.OK)
            {
                if (callReason.ReturnReason == string.Empty)
                {
                    MyUtility.Msg.WarningBox("Reason can not be empty.");
                    return;
                }
                else
                {
                    string insertCmd = string.Format(
                        @"insert into SewingOutput_History (ID,HisType,OldValue,NewValue,ReasonID,Remark,AddName,AddDate)
                    values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}',GETDATE())",
                        MyUtility.Convert.GetString(this.CurrentMaintain["ID"]),
                        "Status",
                        "Locked",
                        "New",
                        callReason.ReturnReason,
                        callReason.ReturnRemark,
                        Env.User.UserID);

                    string insert = $@"
    INSERT INTO GMTBooking_History  ([ID],[HisType],[OldValue],[NewValue],[ReasonID],[Remark],[AddName],[AddDate])
         VALUES
               (    '{this.CurrentMaintain["ID"]}'
                   ,'GBUnCFM'
                   ,'CFM'
                   ,'Un CFM'
                   ,'{callReason.ReturnReason}'
                   ,'{callReason.ReturnRemark}'
                   ,'{Env.User.UserID}'
                   ,GETDATE()
                )

    ";
                    string updateCmd = string.Format("update GMTBooking set Status = 'New', EditName = '{0}', EditDate = GETDATE() where ID = '{1}'", Env.User.UserID, MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));

                    using (TransactionScope transactionScope = new TransactionScope())
                    {
                        try
                        {
                            DualResult result2 = DBProxy.Current.Execute(null, insert);
                            DualResult result3 = DBProxy.Current.Execute(null, updateCmd);

                            if (result2 && result3)
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
                            this.ShowErr("Commit transaction error.", ex);
                            return;
                        }
                    }
                }
            }
            else
            {
                return;
            }
        }

        // Terminal/Whse#
        private void TxtTerminalWhse_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sqlCmd = string.Format(
                @"select fwd.WhseNo,fwd.address,fwd.UKey from ForwarderWhse fw WITH (NOLOCK) , ForwarderWhse_Detail fwd WITH (NOLOCK) 
where fw.ID = fwd.ID
and fw.BrandID = '{0}'
and fw.Forwarder = '{1}'
and fw.ShipModeID = '{2}'
order by fwd.WhseNo",
                MyUtility.Convert.GetString(this.CurrentMaintain["BrandID"]),
                MyUtility.Convert.GetString(this.CurrentMaintain["Forwarder"]),
                MyUtility.Convert.GetString(this.CurrentMaintain["ShipModeID"]));

            DualResult result = DBProxy.Current.Select(null, sqlCmd, out DataTable dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            Win.Tools.SelectItem item = new Win.Tools.SelectItem(dt, "WhseNo,address", "20,20", MyUtility.Convert.GetString(this.txtTerminalWhse.Text));

            DialogResult result1 = item.ShowDialog();
            if (result1 == DialogResult.Cancel)
            {
                return;
            }

            IList<DataRow> dr = item.GetSelecteds();
            this.txtTerminalWhse.Text = item.GetSelectedString();
            this.CurrentMaintain["ForwarderWhse_DetailUKey"] = dr[0]["Ukey"];
        }

        private void TxtTerminalWhse_Validating(object sender, CancelEventArgs e)
        {
            string oldvalue = this.txtTerminalWhse.OldValue;
            if (this.txtTerminalWhse.Text != this.txtTerminalWhse.OldValue && !MyUtility.Check.Empty(this.CurrentMaintain["SOCFMDate"]))
            {
                if (!this.UpSOCFMDate())
                {
                    this.txtTerminalWhse.Text = oldvalue;
                    return;
                }
            }

            if (MyUtility.Check.Empty(this.txtTerminalWhse.Text))
            {
                this.txtTerminalWhse.Text = string.Empty;
                this.CurrentMaintain["ForwarderWhse_DetailUKey"] = 0;
                return;
            }

            string sqlCmd = string.Format(
                @"select fwd.WhseNo,fwd.UKey from ForwarderWhse fw WITH (NOLOCK) , ForwarderWhse_Detail fwd WITH (NOLOCK) 
where fw.ID = fwd.ID
and fwd.whseno = '{0}'
order by fwd.WhseNo", this.txtTerminalWhse.Text.ToString().Trim());
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out DataTable dt);
            if (result)
            {
                if (dt.Rows.Count >= 1)
                {
                    this.txtTerminalWhse.Text = dt.Rows[0]["WhseNo"].ToString();
                    this.CurrentMaintain["ForwarderWhse_DetailUKey"] = dt.Rows[0]["Ukey"].ToString();
                }
                else
                {
                    this.CurrentMaintain["ForwarderWhse_DetailUKey"] = 0;
                    this.txtTerminalWhse.Text = string.Empty;
                    MyUtility.Msg.WarningBox("Terminal/Whse# is not found!!");
                    e.Cancel = true;
                }
            }
        }

        private void MaskedTextBox1_Validated(object sender, EventArgs e)
        {
            MyUtility.Msg.InfoBox("validated");
        }

        private void TxtSONo_Validating(object sender, CancelEventArgs e)
        {
            string oldvalue = this.txtSONo.OldValue;
            if (this.txtSONo.Text != this.txtSONo.OldValue && !MyUtility.Check.Empty(this.CurrentMaintain["SOCFMDate"]))
            {
                if (!this.UpSOCFMDate())
                {
                    this.txtSONo.Text = oldvalue;
                    return;
                }
            }
        }

        private bool UpSOCFMDate()
        {
            #region UnConfirm
            if (MyUtility.GetValue.Lookup("Status", MyUtility.Convert.GetString(this.CurrentMaintain["ShipPlanID"]), "ShipPlan", "ID") == "Confirmed")
            {
                MyUtility.Msg.WarningBox("Ship Plan already confirmed, can't Un CFM!!");
                return false;
            }

            Win.UI.SelectReason callReason = new Win.UI.SelectReason("GMTBooking_SO", true);
            DialogResult dResult = callReason.ShowDialog(this);
            if (dResult == DialogResult.OK)
            {
                string insertCmd = string.Format(
                    @"insert into GMTBooking_History (ID,HisType,OldValue,NewValue,ReasonID,Remark,AddName,AddDate)
values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}',GETDATE())",
                    MyUtility.Convert.GetString(this.CurrentMaintain["ID"]),
                    "SOCFMDate",
                    "Un CFM",
                    "CFM",
                    callReason.ReturnReason,
                    callReason.ReturnRemark,
                    Env.User.UserID);

                string updateCmd = string.Format(@"update GMTBooking set SOCFMDate = null where ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));

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
                            return false;
                        }
                    }
                    catch (Exception ex)
                    {
                        transactionScope.Dispose();
                        this.ShowErr("Commit transaction error.", ex);
                        return false;
                    }
                }

                this.ReloadSOCFMDate();
                this.OnDetailEntered();
                return true;
            }
            else
            {
                this.OnDetailEntered();
                return false;
            }
            #endregion
        }

        private void ReloadSOCFMDate()
        {
            string sqlSOcfmDate = $@"select SOCFMDate  from GMTBooking where id = '{this.CurrentMaintain["ID"]}'";
            string upSOCFMDate = MyUtility.GetValue.Lookup(sqlSOcfmDate);
            if (MyUtility.Check.Empty(upSOCFMDate))
            {
                this.CurrentMaintain["SOCFMDate"] = DBNull.Value;
            }
            else
            {
                this.CurrentMaintain["SOCFMDate"] = upSOCFMDate;
            }
        }

        private void BtnBatchImportSO_Click_1(object sender, EventArgs e)
        {
            P05_BatchImportSO form = new P05_BatchImportSO();
            form.ShowDialog();
        }

        private void BtnUnCfmHis_Click(object sender, EventArgs e)
        {
            P05_UnconfirmHistory dialog = new P05_UnconfirmHistory(this.CurrentMaintain["ID"].ToString());
            dialog.ShowDialog(this);
        }

        private bool CheckShipper(bool showmsg = true)
        {
            string sP = string.Empty;
            if (this.DetailDatas.Count > 0)
            {
                foreach (DataRow dr in this.DetailDatas)
                {
                    sP += "'" + dr["Orderid"].ToString().Replace(",", "','") + "',";
                }

                string sqlcmd = $@"
select ShipperID=isnull(f1.ShipperID, f2.ShipperID)
from Orders o
outer apply(
    select ShipperID 
    from FtyShipper_Detail f 
    where o.FactoryID = f.FactoryID 
          and o.SeasonID = f.SeasonID 
          and  f.BrandID = '{this.txtbrand.Text}' 
          and o.BuyerDelivery between f.BeginDate and f.EndDate
)f1
outer apply(
    select ShipperID 
    from FtyShipper_Detail f 
    where o.FactoryID = f.FactoryID 
          and f.SeasonID = '' 
          and f.BrandID = '{this.txtbrand.Text}' 
          and o.BuyerDelivery between f.BeginDate and f.EndDate
)f2
where o.ID in ({sP.Substring(0, sP.Length - 1)})
group by isnull(f1.ShipperID, f2.ShipperID)
order by min(o.BuyerDelivery)
";
                DualResult result = DBProxy.Current.Select(string.Empty, sqlcmd, out DataTable dtShipper);
                if (!result)
                {
                    this.ShowErr(result);
                    return false;
                }

                if (dtShipper.Rows.Count > 1)
                {
                    this.CurrentMaintain["Shipper"] = dtShipper.Rows[0]["ShipperID"].ToString();
                    if (!showmsg)
                    {
                        return true;
                    }

                    if (MyUtility.Msg.QuestionBox($"Shipper has more than two data, and {dtShipper.Rows[0]["ShipperID"]} will be used as Shipper.") == DialogResult.Yes)
                    {
                        return true;
                    }

                    return false;
                }
                else if (dtShipper.Rows.Count == 1)
                {
                    this.CurrentMaintain["Shipper"] = dtShipper.Rows[0]["ShipperID"].ToString();
                }
                else
                {
                    MyUtility.Msg.WarningBox("Shipper not found! ");
                    return false;
                }
            }

            return true;
        }

        private bool ControlColor()
        {
            string sqlCmd = $@"
select 1
from ShareExpense se WITH (NOLOCK) 
LEFT JOIN SciFMS_AccountNo a on se.AccountID = a.ID
where se.InvNo = '{this.CurrentMaintain["ID"]}'
and se.junk=0";

            DualResult result = DBProxy.Current.Select(null, sqlCmd, out DataTable gridData);
            if (!result)
            {
                this.ShowErr(result);
                return false;
            }

            if (gridData.Rows.Count > 0)
            {
                this.btnExpenseData.ForeColor = Color.Blue;
                return true;
            }
            else
            {
                this.btnExpenseData.ForeColor = Color.Black;
                return false;
            }
        }

        private void BtnFoundryList_Click(object sender, EventArgs e)
        {
            P05_FoundryList dialog = new P05_FoundryList(this.CurrentMaintain["ID"].ToString(), this.CurrentMaintain["ShipModeID"].ToString());
            dialog.ShowDialog(this);
        }

        private void BtnRemark_Click(object sender, EventArgs e)
        {
            Win.Tools.EditMemo callNextForm = new Win.Tools.EditMemo(MyUtility.Convert.GetString(this.CurrentMaintain["Remark"]), "Remark", this.EditMode, null);

            if (callNextForm.ShowDialog() == DialogResult.OK)
            {
                this.CurrentMaintain["Remark"] = callNextForm.Memo;
            }
        }

        private void CheckIDD()
        {
            if (this.DetailDatas.Count == 0)
            {
                return;
            }
            #region 檢查傳入的SP 維護的IDD是否都為同一天(沒維護度不判斷)
            List<Order_QtyShipKey> listOrder_QtyShipKey = new List<Order_QtyShipKey>();
            string sqlGetOrderSeq = $@"
alter table #tmp alter column ID varchar(13)
select  distinct OrderID,OrderShipmodeSeq
from PackingList_Detail pd with (nolock)
where exists(select 1 from #tmp t where t.ID = pd.ID)
";
            DualResult result = MyUtility.Tool.ProcessWithDatatable(this.DetailDatas.CopyToDataTable(), "ID", sqlGetOrderSeq, out DataTable dtOrderSeq);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            foreach (DataRow dr in dtOrderSeq.Rows)
            {
                listOrder_QtyShipKey.Add(new Order_QtyShipKey
                {
                    SP = dr["OrderID"].ToString(),
                    Seq = dr["OrderShipmodeSeq"].ToString(),
                });
            }

            Prgs.CheckIDDSame(listOrder_QtyShipKey);
            #endregion
        }

        private bool IsKeyColumnEmpty()
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["SONo"]) ||
                MyUtility.Check.Empty(this.CurrentMaintain["ForwarderWhse_DetailUKey"]) ||
                MyUtility.Check.Empty(this.CurrentMaintain["CutOffDate"]) ||
                MyUtility.Check.Empty(this.CurrentMaintain["DocumentRefNo"]))
            {
                MyUtility.Msg.WarningBox(@"< S/O # > , < Terminal/Whse# >, < Cut-off Date > and < Document Ref#> can't be
empty!!
p.s. < Document Ref#> format as below
-----------------------------------------
ADI, RBK, LLL: GTN ASN#
U.A: SNC ASN#
N.FACE: NGC#
DOME: EEM#
NB: XPC#
NIKE, REI, GYMSHARK: S/O#
Offline Order: S/O#
-----------------------------------------");
                return true;
            }

            return false;
        }

        private void BtnDocumentRefNoFormat_Click(object sender, EventArgs e)
        {
            SelectItem selectItem = new SelectItem("select [Brand] = ID, [Content] = Name from DropDownList where Type = 'DocumentRefNoFormat' order by Seq", "18,12", string.Empty);
            selectItem.ShowDialog();
        }

        private bool GMTCompleteCheck()
        {
            #region 表身任一筆Orders.ID的Orders.GMTComplete 不可為 'S'
            DataTable isGMTComplete = new DataTable();
            isGMTComplete.ColumnsStringAdd("SP#");
            isGMTComplete.ColumnsDateTimeAdd("Complete Date");
            foreach (DataRow dr in this.DetailDatas)
            {
                // 拆解Order ID
                List<string> orders = MyUtility.Convert.GetString(dr["OrderID"]).Split(',').ToList();
                foreach (var order in orders)
                {
                    string cmd = $@"SELECT [SP#]=ID ,[Complete Date]=CMPLTDATE FROM Orders WITH(NOLOCK) WHERE GMTComplete='S' AND ID = '{order}'";
                    DataTable dt;
                    DBProxy.Current.Select(null, cmd, out dt);
                    bool find = dt.Rows.Count > 0;
                    if (find)
                    {
                        foreach (DataRow r in dt.Rows)
                        {
                            isGMTComplete.ImportRow(r);
                        }
                    }
                }
            }

            if (isGMTComplete.Rows.Count > 0)
            {
                var m = MyUtility.Msg.ShowMsgGrid(isGMTComplete, "GMT Complete Status is updated to S on following dates, assuming the shipment is still to be arranged, please contact TPE Finance Dept. to update GMT Complete Status", "GMT Complete Status check");
                m.Width = 800;
                m.grid1.Columns[0].Width = 150;
                m.grid1.Columns[1].Width = 150;
                m.TopMost = true;
                return false;
            }
            else
            {
                return true;
            }
            #endregion
        }

        private bool DischargePortIDCheck()
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["DischargePortID"]))
            {
                return true;
            }

            // 1. 檢查對應到 PortByBrandShipmode 可得到 DischargePortID 與 Brand
            string sqlcmd = $@"
select 1
from PortByBrandShipmode
where PulloutPortID  = '{this.CurrentMaintain["DischargePortID"]}'
and BrandID =  '{this.CurrentMaintain["BrandID"]}'";
            if (!MyUtility.Check.Seek(sqlcmd))
            {
                MyUtility.Msg.WarningBox("Brand not match to Port Discharge .");
                return false;
            }

            // 2. 檢查 GMTBooking.Dest = PulloutPort.CountryID
//            sqlcmd = $@"
//SELECT SeaPort,AirPort
//FROM PulloutPort p 
//WHERE p.Junk = 0 
//and  p.id = '{this.CurrentMaintain["DischargePortID"]}'
//and p.CountryID = '{this.CurrentMaintain["Dest"]}'
//and Junk = 0
//";

            //if (!MyUtility.Check.Seek(sqlcmd, out DataRow dr))
            //{
            //    MyUtility.Msg.WarningBox("Destination not match to country of Port Discharge .");
            //    return false;
            //}

            //if (MyUtility.Check.Seek(sqlcmd, out DataRow dr))
            //{
            //    // 3.shipmode 是 Sea PulloutPort 必須設定 SeaPort = 1. S-A/C, S-A/P PulloutPort 必須設定 SeaPort = 1 or AirPort = 1
            //    string shipModeID = MyUtility.Convert.GetString(this.CurrentMaintain["ShipModeID"]);
            //    if (shipModeID == "SEA")
            //    {
            //        if (!MyUtility.Convert.GetBool(dr["SeaPort"]))
            //        {
            //            MyUtility.Msg.WarningBox("Shipmode not match to Port Discharge .");
            //            return false;
            //        }
            //    }
            //    else if (shipModeID == "S-A/C" || shipModeID == "S-A/P")
            //    {
            //        if (!MyUtility.Convert.GetBool(dr["SeaPort"]) && !MyUtility.Convert.GetBool(dr["AirPort"]))
            //        {
            //            MyUtility.Msg.WarningBox("Shipmode not match to Port Discharge .");
            //            return false;
            //        }
            //    }
            //}

            return true;
        }

        private void TxtShipmodeShippingMode_SelectedValueChanged(object sender, EventArgs e)
        {
            this.txtPulloutPort1.ShipModeID = this.txtShipmodeShippingMode.SelectedValue;
        }
    }
}
