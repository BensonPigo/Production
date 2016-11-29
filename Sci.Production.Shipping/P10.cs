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
    public partial class P10 : Sci.Win.Tems.Input6
    {
        Ict.Win.UI.DataGridViewDateBoxColumn col_inspdate;
        Ict.Win.UI.DataGridViewDateBoxColumn col_pulloutdate;
        DataTable plData;
        DataSet allData = new DataSet();
        IList<string> updateCmds = new List<string>();

        public P10(ToolStripMenuItem menuitem)
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
            string masterID = (e.Master == null) ? "1=0" : string.Format("p.ShipPlanID ='{0}'", MyUtility.Convert.GetString(e.Master["ID"]));
            string sqlCmd = string.Format(@"select p.ID,
(select cast(a.OrderID as nvarchar) +',' from (select distinct OrderID from PackingList_Detail pd where pd.ID = p.id) a for xml path('')) as OrderID,
(select oq.BuyerDelivery from (select top 1 OrderID, OrderShipmodeSeq from PackingList_Detail pd where pd.ID = p.ID) a, Order_QtyShip oq where a.OrderID = oq.Id and a.OrderShipmodeSeq = oq.Seq) as BuyerDelivery,
p.Status,p.CTNQty,p.CBM,(select sum(CTNQty) from PackingList_Detail pd where pd.ID = p.ID and pd.ReceiveDate is not null) as ClogCTNQty,
p.InspDate,p.InspStatus,p.PulloutDate,p.InvNo,p.MDivisionID
from PackingList p
where {0} order by p.ID", masterID);
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out plData);


            masterID = (e.Master == null) ? "1=0" : string.Format("g.ShipPlanID ='{0}'", MyUtility.Convert.GetString(e.Master["ID"]));
            this.DetailSelectCommand = string.Format(@"select g.ID,g.BrandID,g.ShipModeID,(g.Forwarder+' - '+(select ls.Abb from LocalSupp ls where ls.ID = g.Forwarder)) as Forwarder,g.CYCFS,g.SONo,g.CutOffDate,g.ForwarderWhse_DetailUKey, isnull(fd.WhseNo,'') as WhseNo,iif(g.Status='Confirmed','GB Confirmed',iif(g.SOCFMDate is null,'','S/O Confirmed')) as Status,g.TotalCTNQty,g.TotalCBM,
(select isnull(sum(pd.CTNQty),0) from PackingList p,PackingList_Detail pd where p.INVNo = g.ID and p.ID = pd.ID and pd.ReceiveDate is not null) as ClogCTNQty
from GMTBooking g
left join ForwarderWhse_Detail fd on g.ForwarderWhse_DetailUKey = fd.UKey
where {0} order by g.ID", masterID);

            return base.OnDetailSelectCommandPrepare(e);
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            button1.Enabled = !EditMode && MyUtility.Convert.GetString(CurrentMaintain["Status"]) != "Confirmed" && PublicPrg.Prgs.GetAuthority(Sci.Env.User.UserID, "P10. Ship Plan", "CanEdit");
        }

        protected override void OnDetailUIConvertToMaintain()
        {
            base.OnDetailUIConvertToMaintain();
            grid1.IsEditingReadOnly = false;
        }

        protected override void OnDetailUIConvertToView()
        {
            base.OnDetailUIConvertToView();
            grid1.IsEditingReadOnly = true;
        }

        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();

            Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("ID", header: "GB#", width: Widths.AnsiChars(22), iseditingreadonly: true)
                .Text("BrandID", header: "Brand", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("ShipModeID", header: "Ship Mode", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("Forwarder", header: "Forwarder", width: Widths.AnsiChars(17), iseditingreadonly: true)
                .Text("CYCFS", header: "Container Type", width: Widths.AnsiChars(7), iseditingreadonly: true)
                .Text("SONo", header: "S/O No.", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .DateTime("CutOffdate", header: "Cut-off Date/Time", iseditingreadonly: true)
                .Numeric("WhseNo", header: "Container Terminals", iseditingreadonly: true)
                .Text("Status", header: "GB Status", width: Widths.AnsiChars(16), iseditingreadonly: true)
                .Numeric("TotalCTNQty", header: "Total Cartons", iseditingreadonly: true)
                .Numeric("TotalCBM", header: "Total CBM", decimal_places: 2, iseditingreadonly: true)
                .Numeric("ClogCTNQty", header: "Total CTN Q'ty at C-Logs", iseditingreadonly: true);
            detailgrid.SelectionChanged += (s, e) =>
            {
                grid1.ValidateControl();
                DataRow dr = this.detailgrid.GetDataRow<DataRow>(detailgrid.GetSelectedRowIndex());
                if (dr != null)
                {
                    string filter = string.Format("InvNo = '{0}'", MyUtility.Convert.GetString(dr["ID"]));
                    plData.DefaultView.RowFilter = filter;
                }
            };

            grid1.DataSource = listControlBindingSource1;
            grid1.IsEditingReadOnly = false;
            Helper.Controls.Grid.Generator(this.grid1)
                .Text("ID", header: "Packing No.", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("OrderID", header: "SP#", width: Widths.AnsiChars(16), iseditingreadonly: true)
                .Date("BuyerDelivery", header: "Delivery", iseditingreadonly: true)
                .Text("Status", header: "Packing Status", width: Widths.AnsiChars(9), iseditingreadonly: true)
                .Numeric("CTNQty", header: "CTN Qty", iseditingreadonly: true)
                .Numeric("CBM", header: "CBM", decimal_places: 3, iseditingreadonly: true)
                .Numeric("ClogCTNQty", header: "CTN Q'ty at C-Logs", iseditingreadonly: true)
                .Date("InspDate", header: "Est. Inspection Date").Get(out col_inspdate)
                .Text("InspStatus", header: "Inspection Status", width: Widths.AnsiChars(10))
                .Date("PulloutDate", header: "Pullout Date").Get(out col_pulloutdate);
            #region 欄位值檢查
            grid1.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                    if (grid1.Columns[e.ColumnIndex].DataPropertyName == col_inspdate.DataPropertyName)
                    {
                        if (!MyUtility.Check.Empty(e.FormattedValue))
                        {
                            if (MyUtility.Convert.GetDate(e.FormattedValue) != MyUtility.Convert.GetDate(dr["InspDate"]))
                            {
                                if (MyUtility.Convert.GetDate(e.FormattedValue) > DateTime.Today.AddMonths(1) || MyUtility.Convert.GetDate(e.FormattedValue) < DateTime.Today.AddMonths(-1))
                                {
                                    MyUtility.Msg.WarningBox("< Est. Inspection Date > is invalid!!");
                                    dr["InspDate"] = null;
                                    e.Cancel = true;
                                    return;
                                }
                            }
                        }
                    }

                    //輸入的Pullout date或原本的Pullout date的Pullout Report如果已經Confirmed的話，就不可以被修改
                    if (grid1.Columns[e.ColumnIndex].DataPropertyName == col_pulloutdate.DataPropertyName)
                    {

                        if (MyUtility.Convert.GetDate(e.FormattedValue) != MyUtility.Convert.GetDate(dr["PulloutDate"]))
                        {
                            object newPulloutDate = MyUtility.Convert.GetDate(e.FormattedValue);
                            if (!MyUtility.Check.Empty(dr["PulloutDate"]) && CheckPullout((DateTime)MyUtility.Convert.GetDate(dr["PulloutDate"]), MyUtility.Convert.GetString(dr["MDivisionID"])))
                            {
                                PulloutMsg(dr, (DateTime)MyUtility.Convert.GetDate(dr["PulloutDate"]));
                                e.Cancel = true;
                                dr.EndEdit();
                                return;
                            }

                            if (!MyUtility.Check.Empty(e.FormattedValue) && CheckPullout((DateTime)MyUtility.Convert.GetDate(e.FormattedValue), MyUtility.Convert.GetString(dr["MDivisionID"])))
                            {
                                PulloutMsg(dr, (DateTime)MyUtility.Convert.GetDate(e.FormattedValue));
                                e.Cancel = true;
                                dr.EndEdit();
                                return;
                            }
                        }
                    }
                }
            };
            #endregion
        }

        protected override DualResult OnRenewDataDetailPost(RenewDataPostEventArgs e)
        {
            listControlBindingSource1.DataSource = plData;
            return base.OnRenewDataDetailPost(e);
        }

        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["CDate"] = DateTime.Today;
            CurrentMaintain["Status"] = "New";
            listControlBindingSource1.DataSource = plData;
        }

        protected override bool ClickEditBefore()
        {
            if (MyUtility.Convert.GetString(CurrentMaintain["Status"]) == "Confirmed")
            {
                MyUtility.Msg.WarningBox(string.Format("This record status is < {0} >, can't be edit!", MyUtility.Convert.GetString(CurrentMaintain["Status"])));
                return false;
            }
            return base.ClickEditBefore();
        }

        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            if (MyUtility.Convert.GetString(CurrentMaintain["Status"]) != "New")
            {
                button2.Enabled = false;
            }
        }

        protected override void ClickUndo()
        {
            base.ClickUndo();
            ((DataTable)listControlBindingSource1.DataSource).RejectChanges();
        }

        protected override bool ClickSaveBefore()
        {
            //GetID
            if (IsDetailInserting)
            {
                string id = MyUtility.GetValue.GetID(Sci.Env.User.Keyword+"SH", "ShipPlan", DateTime.Today, 3, "Id", null);
                if (MyUtility.Check.Empty(id))
                {
                    MyUtility.Msg.WarningBox("GetID fail, please try again!");
                    return false;
                }
                CurrentMaintain["ID"] = id;
            }
            return base.ClickSaveBefore();
        }

        protected override DualResult OnSaveDetail(IList<DataRow> details, ITableSchema detailtableschema)
        {
            updateCmds.Clear();
            grid1.EndEdit();
            listControlBindingSource1.EndEdit();

            foreach (DataRow dr in ((DataTable)listControlBindingSource1.DataSource).Rows)
            {
                if (dr.RowState == DataRowState.Modified || dr.RowState == DataRowState.Added)
                {
                    UpdatePLCmd(dr);
                    continue;
                }
            }

            foreach (DataRow dr in details)
            {
                if (dr.RowState == DataRowState.Modified || dr.RowState == DataRowState.Added)
                {
                    updateCmds.Add(string.Format("update GMTBooking set ShipPlanID = '{0}' where ID = '{1}';", MyUtility.Convert.GetString(CurrentMaintain["ID"]), MyUtility.Convert.GetString(dr["ID"])));
                    continue;
                }

                if (dr.RowState == DataRowState.Deleted)
                {
                    updateCmds.Add(string.Format("update GMTBooking set ShipPlanID = '' where ID = '{0}';", MyUtility.Convert.GetString(dr["ID",DataRowVersion.Original])));
                    updateCmds.Add(DeletePLCmd("InvNo", MyUtility.Convert.GetString(dr["ID",DataRowVersion.Original])));
                    continue;
                }
            }

            //執行更新
            if (updateCmds.Count != 0)
            {
                DualResult result;
                result = DBProxy.Current.Executes(null, updateCmds);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Update fail!!\r\n" + result.ToString());
                    return failResult;
                }
            }
            return Result.True;
        }

        protected override bool ClickDeleteBefore()
        {
            if (MyUtility.Convert.GetString(CurrentMaintain["Status"]) != "New")
            {
                MyUtility.Msg.WarningBox(string.Format("This record status is < {0} >, can't be delete!", MyUtility.Convert.GetString(CurrentMaintain["Status"])));
                return false;
            }
            return base.ClickDeleteBefore();
        }

        protected override DualResult OnDeleteDetails()
        {
            updateCmds.Clear();
            updateCmds.Add(string.Format("update GMTBooking set ShipPlanID = '' where ShipPlanID = '{0}';", MyUtility.Convert.GetString(CurrentMaintain["ID"])));
            updateCmds.Add(DeletePLCmd("ShipPlanID", MyUtility.Convert.GetString(CurrentMaintain["ID"])));
            DualResult result = DBProxy.Current.Executes(null, updateCmds);

            return result;
        }

        protected override bool ClickPrint()
        {
            if (MyUtility.Check.Empty(CurrentMaintain["ID"]))
            {
                return false;
            }

            string sqlCmd = string.Format(@"select p.ShipPlanID,p.INVNo,g.BrandID,g.ShipModeID, (g.Forwarder+'-'+ls.Abb) as Forwarder, g.CYCFS,
g.SONo,g.CutOffDate,isnull(fd.WhseNo,'') as WhseNo,
iif(g.Status='Confirmed','GB Confirmed',iif(g.SOCFMDate is null,'','S/O Confirmed')) as Status,
g.TotalCTNQty,g.TotalCBM,
(select isnull(sum(pd1.CTNQty),0) from PackingList p1,PackingList_Detail pd1 where p1.INVNo = g.ID and p1.ID = pd1.ID and pd1.ReceiveDate is not null) as ClogCTNQty,
p.ID,(select cast(a.OrderID as nvarchar) +',' from (select distinct OrderID from PackingList_Detail pd where pd.ID = p.id) a for xml path('')) as OrderID,
(select oq.BuyerDelivery from (select top 1 OrderID, OrderShipmodeSeq from PackingList_Detail pd where pd.ID = p.ID) a, Order_QtyShip oq where a.OrderID = oq.Id and a.OrderShipmodeSeq = oq.Seq) as BuyerDelivery,
p.Status as PLStatus,p.CTNQty,p.CBM,(select sum(CTNQty) from PackingList_Detail pd where pd.ID = p.ID and pd.ReceiveDate is not null) as PLClogCTNQty,
p.InspDate,p.InspStatus,p.PulloutDate
from PackingList p
left join GMTBooking g on p.INVNo = g.ID
left join ForwarderWhse_Detail fd on g.ForwarderWhse_DetailUKey = fd.UKey
left join LocalSupp ls on g.Forwarder = ls.ID
where p.ShipPlanID = '{0}'
order by p.INVNo,p.ID", MyUtility.Convert.GetString(CurrentMaintain["ID"]));
            DataTable ExcelData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out ExcelData);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Query data fail!!\r\n" + result.ToString());
                return false;
            }

            string strXltName = Sci.Env.Cfg.XltPathDir + "\\Shipping_P10.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null) return false;
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            int intRowsStart = 2;
            int dataRowCount = ExcelData.Rows.Count;
            object[,] objArray = new object[1, 23];
            for (int i = 0; i < dataRowCount; i++)
            {
                DataRow dr = ExcelData.Rows[i];
                int rownum = intRowsStart + i;
                objArray[0, 0] = dr["ShipPlanID"];
                objArray[0, 1] = dr["INVNo"];
                objArray[0, 2] = dr["BrandID"];
                objArray[0, 3] = dr["ShipModeID"];
                objArray[0, 4] = dr["Forwarder"];
                objArray[0, 5] = dr["CYCFS"];
                objArray[0, 6] = dr["SONo"];
                objArray[0, 7] = MyUtility.Check.Empty(dr["CutOffDate"]) ? "" : Convert.ToDateTime(dr["CutOffDate"]).ToString(string.Format("{0}", Sci.Env.Cfg.DateTimeStringFormat));
                objArray[0, 8] = dr["WhseNo"];
                objArray[0, 9] = dr["Status"];
                objArray[0, 10] = dr["TotalCTNQty"];
                objArray[0, 11] = dr["TotalCBM"];
                objArray[0, 12] = dr["ClogCTNQty"];
                objArray[0, 13] = dr["ID"];
                objArray[0, 14] = dr["OrderID"];
                objArray[0, 15] = dr["BuyerDelivery"];
                objArray[0, 16] = dr["PLStatus"];
                objArray[0, 17] = dr["CTNQty"];
                objArray[0, 18] = dr["CBM"];
                objArray[0, 19] = dr["PLClogCTNQty"];
                objArray[0, 20] = dr["InspDate"];
                objArray[0, 21] = dr["InspStatus"];
                objArray[0, 22] = dr["PulloutDate"];
                worksheet.Range[String.Format("A{0}:W{0}", rownum)].Value2 = objArray;
            }
            excel.Visible = true;

            return base.ClickPrint();
        }

        //組Update PackingList的SQL
        private void UpdatePLCmd(DataRow pldatarow)
        {
            updateCmds.Add(string.Format("update PackingList set ShipPlanID = '{0}', InspDate = {1}, InspStatus = '{2}', PulloutDate = {3} where ID = '{4}';"
                        , MyUtility.Convert.GetString(CurrentMaintain["ID"]), MyUtility.Check.Empty(pldatarow["InspDate"]) ? "null" : "'" + Convert.ToDateTime(pldatarow["InspDate"]).ToString("d") + "'"
                        , MyUtility.Convert.GetString(pldatarow["InspStatus"]), MyUtility.Check.Empty(pldatarow["PulloutDate"]) ? "null" : "'" + Convert.ToDateTime(pldatarow["PulloutDate"]).ToString("d") + "'", MyUtility.Convert.GetString(pldatarow["ID"])));
        }

        //組(Delete)Update PackingList的SQL
        private string DeletePLCmd(string ColumnName, string ID)
        {
            return string.Format("update PackingList set ShipPlanID = '', PulloutDate = null, InspDate = null, InspStatus = '' where {0} = '{1}';", ColumnName, ID);
        }

        //檢查Pullout report是否已經Confirm
        private bool CheckPullout(DateTime pulloutDate, string mdivisionid)
        {
            return MyUtility.Check.Seek(string.Format("select ID from Pullout where PulloutDate = '{0}' and MDivisionID = '{1}' and Status <> 'New'", Convert.ToDateTime(pulloutDate).ToString("d"), mdivisionid));
        }

        //Process Pullout Date Message
        private void PulloutMsg(DataRow dr, DateTime dt)
        {
            MyUtility.Msg.WarningBox("Pullout date:" + Convert.ToDateTime(dt).ToString("d") + " already exist pullout report and have been confirmed, can't modify!");
            dr["PulloutDate"] = dr["PulloutDate"];
        }

        //Import Data
        private void button2_Click(object sender, EventArgs e)
        {
            Sci.Production.Shipping.P10_ImportData callNextForm = new Sci.Production.Shipping.P10_ImportData(CurrentMaintain, (DataTable)detailgridbs.DataSource, (DataTable)listControlBindingSource1.DataSource);
            callNextForm.ShowDialog(this);
        }

        //Update Pullout Date
        private void button1_Click(object sender, EventArgs e)
        {
            Sci.Production.Shipping.P10_UpdatePulloutDate callNextForm = new Sci.Production.Shipping.P10_UpdatePulloutDate(CurrentMaintain);
            callNextForm.ShowDialog(this);
            RenewData();
        }

        //表身Grid的Delete
        protected override void OnDetailGridDelete()
        {
            //檢查此筆記錄的Pullout Data是否還有值，若是則出訊息告知且無法刪除
            if (this.DetailDatas.Count > 0)
            {
                grid1.ValidateControl();
                foreach (DataRow pldr in plData.Select(string.Format("InvNo = '{0}'", MyUtility.Convert.GetString(CurrentDetailData["ID"]))))
                {
                    if (!MyUtility.Check.Empty(pldr["PulloutDate"]))
                    {
                        MyUtility.Msg.WarningBox(string.Format("Pullout date of Packing No.:{0} is not empty, can't delete!", MyUtility.Convert.GetString(pldr["ID"])));
                        return;
                    }
                }

                if (this.DetailDatas.Count-1 <= 0)
                {
                    string filter = "InvNo = ''";
                    plData.DefaultView.RowFilter = filter;
                }
            }
            base.OnDetailGridDelete();
        }

        //Check
        protected override void ClickCheck()
        {
            base.ClickCheck();
            string updateCmd = string.Format("update ShipPlan set Status = 'Checked', CFMDate = '{0}', EditName = '{1}', EditDate = GETDATE() where ID = '{2}'", DateTime.Today.ToString("d"), Sci.Env.User.UserID, MyUtility.Convert.GetString(CurrentMaintain["ID"]));

            DualResult result = DBProxy.Current.Execute(null, updateCmd);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Check fail !\r\n" + result.ToString());
                return;
            }
            RenewData();
            OnDetailEntered();
            EnsureToolbarExt();
        }

        //Un Check
        protected override void ClickUncheck()
        {
            base.ClickUncheck();
            string updateCmd = string.Format("update ShipPlan set Status = 'New', CFMDate = null, EditName = '{0}', EditDate = GETDATE() where ID = '{1}'", Sci.Env.User.UserID, MyUtility.Convert.GetString(CurrentMaintain["ID"]));

            DualResult result = DBProxy.Current.Execute(null, updateCmd);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Uncheck fail !\r\n" + result.ToString());
                return;
            }
            RenewData();
            OnDetailEntered();
            EnsureToolbarExt();
        }

        //Confirm
        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            //Pullout Date有空值就不可以Confirm
            string sqlCmd = string.Format("select ID,InvNo from PackingList where ShipPlanID = '{0}' and PulloutDate is null", MyUtility.Convert.GetString(CurrentMaintain["ID"]));
            DataTable dt;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out dt);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Check Pullout Date error:\r\n" + result.ToString());
                return;
            }
            else
            {
                StringBuilder msg = new StringBuilder();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        msg.Append(string.Format("GB#: {0}, Packing No:{1}\n\r", MyUtility.Convert.GetString(dr["InvNo"]), MyUtility.Convert.GetString(dr["ID"])));
                    }
                    MyUtility.Msg.WarningBox("Below data's pullout date is empty, can' confirm!!\r\n" + msg.ToString());
                    return;
                }
            }
            //Inspection date不為空但是Inspection status為空就不可以Confirm
            sqlCmd = string.Format("select ID,InvNo from PackingList where ShipPlanID = '{0}' and InspDate is not null and InspStatus = ''", MyUtility.Convert.GetString(CurrentMaintain["ID"]));
            result = DBProxy.Current.Select(null, sqlCmd, out dt);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Check Inspection error:\r\n" + result.ToString());
                return;
            }
            else
            {
                StringBuilder msg1 = new StringBuilder();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        msg1.Append(string.Format("GB#: {0}, Packing No:{1}\n\r", MyUtility.Convert.GetString(dr["InvNo"]), MyUtility.Convert.GetString(dr["ID"])));
                    }
                    MyUtility.Msg.WarningBox("Below data's est. inspection date not empty but inspection status is empty, can' confirm!!\r\n" + msg1.ToString());
                    return;
                }
            }

            //Garment Booking還沒Confirm就不可以做Confirm
            sqlCmd = string.Format("select ID from GMTBooking where ShipPlanID = '{0}' and Status = 'New'", MyUtility.Convert.GetString(CurrentMaintain["ID"]));
            result = DBProxy.Current.Select(null, sqlCmd, out dt);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Check GMTBooking error:\r\n" + result.ToString());
                return;
            }
            else
            {
                StringBuilder msg2 = new StringBuilder();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        msg2.Append(string.Format("GB#:{0}", MyUtility.Convert.GetString(dr["ID"])));
                    }
                    MyUtility.Msg.WarningBox("Garment Booking's status not yet confirm, can' confirm!!\r\n" + msg2.ToString());
                    return;
                }
            }
            string updateCmd = string.Format("update ShipPlan set Status = 'Confirmed',CFMDate = GETDATE(), EditName = '{0}', EditDate = GETDATE() where ID = '{1}'", Sci.Env.User.UserID, MyUtility.Convert.GetString(CurrentMaintain["ID"]));

            result = DBProxy.Current.Execute(null, updateCmd);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Confirm fail !\r\n" + result.ToString());
                return;
            }
            RenewData();
            OnDetailEntered();
            EnsureToolbarExt();
        }

        //Un Confirm
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            string updateCmd = string.Format("update ShipPlan set Status = 'New',CFMDate =Null ,EditName = '{0}', EditDate = GETDATE() where ID = '{1}'", Sci.Env.User.UserID, MyUtility.Convert.GetString(CurrentMaintain["ID"]));

            DualResult result = DBProxy.Current.Execute(null, updateCmd);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Unconfirm fail !\r\n" + result.ToString());
                return;
            }
            RenewData();
            OnDetailEntered();
            EnsureToolbarExt();
        }
    }
}
