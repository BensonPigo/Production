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

namespace Sci.Production.Logistic
{
    public partial class P05 : Sci.Win.Tems.Input6
    {
        Ict.Win.UI.DataGridViewDateBoxColumn col_inspdate;
        Ict.Win.UI.DataGridViewDateBoxColumn col_pulloutdate;
        DataTable gbData,plData;
        DataSet allData = new DataSet();
        IList<string> updateCmds = new List<string>();


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
            string masterID = (e.Master == null) ? "1=0" : string.Format("p.ShipPlanID ='{0}'",e.Master["ID"].ToString());
            string sqlCmd = string.Format(@"select p.ID,
iif(p.OrderID='',(select cast(a.OrderID as nvarchar) +',' from (select distinct OrderID from PackingList_Detail pd where pd.ID = p.id) a for xml path('')),p.OrderID) as OrderID,
iif(p.type = 'B',(select BuyerDelivery from Order_QtyShip where ID = p.OrderID and Seq = p.OrderShipmodeSeq),(select oq.BuyerDelivery from (select top 1 OrderID, OrderShipmodeSeq from PackingList_Detail pd where pd.ID = p.ID) a, Order_QtyShip oq where a.OrderID = oq.Id and a.OrderShipmodeSeq = oq.Seq)) as BuyerDelivery,
p.Status,p.CTNQty,p.CBM,(select sum(CTNQty) from PackingList_Detail pd where pd.ID = p.ID and pd.ClogReceiveID != '') as ClogCTNQty,
p.InspDate,p.InspStatus,p.PulloutDate,p.InvNo
from PackingList p
where {0}", masterID);
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out plData);
            

            masterID = (e.Master == null) ? "1=0" : string.Format("g.ShipPlanID ='{0}'", e.Master["ID"].ToString());
            this.DetailSelectCommand = string.Format(@"select g.ID,g.BrandID,g.ShipModeID,(g.Forwarder+' - '+(select ls.Abb from LocalSupp ls where ls.ID = g.Forwarder)) as Forwarder,g.CYCFS,g.SONo,g.CutOffDate,g.ForwarderWhseID,iif(g.Status='Confirmed','GB Confirmed',iif(g.SOCFMDate is null,'','S/O Confirmed')) as Status,g.TotalCTNQty,g.TotalCBM,
(select isnull(sum(pd.CTNQty),0) from PackingList p,PackingList_Detail pd where p.INVNo = g.ID and p.ID = pd.ID and pd.ReceiveDate is not null) as ClogCTNQty
from GMTBooking g
where {0}", masterID);

            return base.OnDetailSelectCommandPrepare(e);
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
                .Text("ID", header: "GB#", width: Widths.AnsiChars(25),iseditingreadonly: true)
                .Text("BrandID", header: "Brand", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("ShipModeID", header: "Ship Mode", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("Forwarder", header: "Forwarder", width: Widths.AnsiChars(17), iseditingreadonly: true)
                .Text("CYCFS", header: "Container Type", width: Widths.AnsiChars(7), iseditingreadonly: true)
                .Text("SONo", header: "S/O No.", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .DateTime("CutOffdate", header: "Cut-off Date/Time", iseditingreadonly: true)
                .Numeric("ForwarderWhseID", header: "Container Terminals", iseditingreadonly: true)
                .Text("Status", header: "GB Status", width: Widths.AnsiChars(16), iseditingreadonly: true)
                .Numeric("TotalCTNQty", header: "Total Cartons", iseditingreadonly: true)
                .Numeric("TotalCBM", header: "Total CBM", decimal_places: 2, iseditingreadonly: true)
                .Numeric("ClogCTNQty", header: "Total CTN Q'ty at C-Logs", iseditingreadonly: true);
            detailgrid.SelectionChanged += (s, e) =>
            {
                DataRow dr = this.detailgrid.GetDataRow<DataRow>(detailgrid.GetSelectedRowIndex());
                if (dr != null)
                {
                    string filter = string.Format("InvNo = '{0}'", dr["ID"].ToString());
                    plData.DefaultView.RowFilter = filter;
                }
            };
            
            grid1.DataSource = listControlBindingSource1;
            grid1.IsEditingReadOnly = false;
            Helper.Controls.Grid.Generator(this.grid1)
                .Text("ID", header: "Packing No.", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("OrderID", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
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
                            if ((!MyUtility.Check.Empty(dr["InspDate"]) && Convert.ToDateTime(e.FormattedValue) != Convert.ToDateTime(dr["InspDate"])) || MyUtility.Check.Empty(dr["InspDate"]))
                            {
                                if (Convert.ToDateTime(e.FormattedValue) > DateTime.Today.AddMonths(1) || Convert.ToDateTime(e.FormattedValue) < DateTime.Today.AddMonths(-1))
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
                        if (!(MyUtility.Check.Empty(e.FormattedValue) || MyUtility.Check.Empty(dr["PulloutDate"])))
                        {
                            if (e.FormattedValue.ToString() != dr["PulloutDate"].ToString())
                            {
                                if (CheckPullout(Convert.ToDateTime(dr["PulloutDate"]), dr["FactoryID"].ToString()))
                                {
                                    PulloutMsg(dr, Convert.ToDateTime(dr["PulloutDate"]));
                                    e.Cancel = true;
                                    dr.EndEdit();
                                    return;
                                }
                                if (CheckPullout(Convert.ToDateTime(e.FormattedValue), dr["FactoryID"].ToString()))
                                {
                                    PulloutMsg(dr, Convert.ToDateTime(e.FormattedValue));
                                    e.Cancel = true;
                                    dr.EndEdit();
                                    return;
                                }
                            }
                        }
                        else
                        {
                            if (MyUtility.Check.Empty(dr["PulloutDate"]))
                            {
                                if (CheckPullout(Convert.ToDateTime(e.FormattedValue), dr["FactoryID"].ToString()))
                                {
                                    PulloutMsg(dr, Convert.ToDateTime(e.FormattedValue));
                                    e.Cancel = true;
                                    dr.EndEdit();
                                    return;
                                }
                            }
                            else
                            {
                                if (CheckPullout(Convert.ToDateTime(dr["PulloutDate"]), dr["FactoryID"].ToString()))
                                {
                                    PulloutMsg(dr, Convert.ToDateTime(dr["PulloutDate"]));
                                    e.Cancel = true;
                                    dr.EndEdit();
                                    return;
                                }
                            }
                        }
                    }
                }
            };
            #endregion
        }

        protected override DualResult OnRenewDataDetailPost(RenewDataPostEventArgs e)
        {
            //listControlBindingSource1.DataSource = null;
            listControlBindingSource1.DataSource = plData;
            return base.OnRenewDataDetailPost(e);
        }

        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["CDate"] = DateTime.Today;
            CurrentMaintain["Status"] = "New";
            //listControlBindingSource1.DataSource = null;
            listControlBindingSource1.DataSource = plData;
        }

        protected override bool ClickEditBefore()
        {
            if (CurrentMaintain["Status"].ToString() != "New")
            {
                MyUtility.Msg.WarningBox(string.Format("This record status is < {0} >, can't be edit!",CurrentMaintain["Status"].ToString()));
                return false;
            }
            return base.ClickEditBefore();
        }

        protected override bool ClickSaveBefore()
        {
            //GetID
            if (IsDetailInserting)
            {
                string id = MyUtility.GetValue.GetID(MyUtility.GetValue.Lookup("NegoRegion", Sci.Env.User.Factory, "Factory", "ID").Trim(), "ShipPlan", DateTime.Today, 4, "Id", null);
                if (MyUtility.Check.Empty(id))
                {
                    MyUtility.Msg.WarningBox("GetID fail, please try again!");
                    return false;
                }
                CurrentMaintain["ID"] = id;
            }
            return base.ClickSaveBefore();
        }

        protected override bool OnSaveDetail(IList<DataRow> details, ITableSchema detailtableschema)
        {
            updateCmds.Clear();
            grid1.EndEdit();
            listControlBindingSource1.EndEdit();

            foreach (DataRow dr in details)
            {
                if (dr.RowState == DataRowState.Modified || dr.RowState == DataRowState.Added)
                {
                    updateCmds.Add(string.Format("update GMTBooking set ShipPlanID = '{0}' where ID = '{1}';", CurrentMaintain["ID"].ToString(), dr["ID"].ToString()));
                    continue;
                }

                if (dr.RowState == DataRowState.Deleted)
                {
                    updateCmds.Add(string.Format("update GMTBooking set ShipPlanID = '' where ID = '{0}';", dr["ID"].ToString()));
                    updateCmds.Add(DeletePLCmd("InvNo",dr["ID"].ToString()));
                    continue;
                }
            }

            foreach (DataRow dr in ((DataTable)listControlBindingSource1.DataSource).Rows)
            {
                if (dr.RowState == DataRowState.Modified || dr.RowState == DataRowState.Added)
                {
                    UpdatePLCmd(dr);
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
                    MyUtility.Msg.ErrorBox(result.ToString());
                    return false;
                }
            }

            return true;
        }

        protected override bool ClickDeleteBefore()
        {
            if (CurrentMaintain["Status"].ToString() != "New")
            {
                MyUtility.Msg.WarningBox(string.Format("This record status is < {0} >, can't be delete!", CurrentMaintain["Status"].ToString()));
                return false;
            }
            return base.ClickDeleteBefore();
        }

        protected override bool OnDeleteDetails()
        {
            updateCmds.Clear();
            updateCmds.Add(string.Format("update GMTBooking set ShipPlanID = '' where ShipPlanID = '{0}';", CurrentMaintain["ID"].ToString()));
            updateCmds.Add(DeletePLCmd("ShipPlanID", CurrentMaintain["ID"].ToString()));
            DualResult result = DBProxy.Current.Executes(null, updateCmds);
            if (!result)
            {
                MyUtility.Msg.ErrorBox(result.ToString());
                return false;
            }
            return true;
        }

        //組Update PackingList的SQL
        private void UpdatePLCmd(DataRow pldatarow)
        {
            updateCmds.Add(string.Format("update PackingList set ShipPlanID = '{0}', InspDate = {1}, InspStatus = '{2}', PulloutDate = {3} where ID = '{4}';"
                        , CurrentMaintain["ID"].ToString(), MyUtility.Check.Empty(pldatarow["InspDate"]) ? "null" : "'" + Convert.ToDateTime(pldatarow["InspDate"]).ToString("d") + "'"
                        , pldatarow["InspStatus"].ToString(), MyUtility.Check.Empty(pldatarow["PulloutDate"]) ? "null" : "'" + Convert.ToDateTime(pldatarow["PulloutDate"]).ToString("d") + "'", pldatarow["ID"].ToString()));
        }

        //組(Delete)Update PackingList的SQL
        private string DeletePLCmd(string ColumnName, string ID)
        {
            return string.Format("update PackingList set ShipPlanID = '', PulloutDate = null, InspDate = null, InspStatus = '' where {0} = '{1}';", ColumnName, ID);
        }

        //檢查Pullout report是否已經Confirm
        private bool CheckPullout(DateTime pulloutDate, string factory)
        {
            return MyUtility.Check.Seek(string.Format("select ID from Pullout where PulloutDate = '{0}' and FactoryID = '{1}' and Status = 'Confirmed'", Convert.ToDateTime(pulloutDate).ToString("d"), factory.ToString()));
        }

        //Process Pullout Date Message
        private void PulloutMsg(DataRow dr, DateTime dt)
        {
            MyUtility.Msg.WarningBox("Pullout date:" + Convert.ToDateTime(dt).ToString("d") + " already exist pullout report and have been confirmed, can't modify!");
            dr["PulloutDate"] = MyUtility.Check.Empty(dr["PulloutDate"], null, dr["PulloutDate"].ToString());
        }

        //Import Data
        private void button2_Click(object sender, EventArgs e)
        {
            Sci.Production.Logistic.P05_ImportData callNextForm = new Sci.Production.Logistic.P05_ImportData(CurrentMaintain, (DataTable)detailgridbs.DataSource, (DataTable)listControlBindingSource1.DataSource);
            callNextForm.ShowDialog(this);
        }

        //Update Pullout Date
        private void button1_Click(object sender, EventArgs e)
        {
            Sci.Production.Logistic.P05_UpdatePulloutDate callNextForm = new Sci.Production.Logistic.P05_UpdatePulloutDate(CurrentMaintain);
            callNextForm.ShowDialog(this);
        }

        //表身Grid的Delete
        protected override void OnDetailGridDelete()
        {
            //檢查此筆記錄的Pullout Data是否還有值，若是則出訊息告知且無法刪除
            if (this.DetailDatas.Count > 0)
            {
                foreach (DataRow pldr in plData.Select(string.Format("InvNo = '{0}'", CurrentDetailData["ID"].ToString())))
                {
                    if (!MyUtility.Check.Empty(pldr["PulloutDate"]))
                    {
                        MyUtility.Msg.WarningBox(string.Format("Pullout date of Packing No.:{0} is not empty, can't delete!",pldr["ID"].ToString()));
                        return;
                    }
                }
            }
            base.OnDetailGridDelete();
        }

        //Check
        protected override void ClickCheck()
        {
            base.ClickCheck();
            string updateCmd = string.Format("update ShipPlan set Status = 'Checked', CFMDate = '{0}', EditName = '{1}', EditDate = '{2}' where ID = '{3}'", DateTime.Today.ToString("d"), Sci.Env.User.UserID, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), CurrentMaintain["ID"].ToString());

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
            string updateCmd = string.Format("update ShipPlan set Status = 'New', CFMDate = null, EditName = '{0}', EditDate = '{1}' where ID = '{2}'", Sci.Env.User.UserID, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), CurrentMaintain["ID"].ToString());

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
            string sqlCmd = string.Format("select ID,InvNo from PackingList where ShipPlanID = '{0}' and PulloutDate is null", CurrentMaintain["ID"].ToString());
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
                        msg.Append(string.Format("GB#: {0}, Packing No:{1}\n\r", dr["InvNo"].ToString(), dr["ID"].ToString()));
                    }
                    MyUtility.Msg.WarningBox("Below data's pullout date is empty, can' confirm!!\r\n" + msg.ToString());
                    return;
                }
            }
            //Inspection date不為空但是Inspection status為空就不可以Confirm
            sqlCmd = string.Format("select ID,InvNo from PackingList where ShipPlanID = '{0}' and InspDate is not null and InspStatus = ''", CurrentMaintain["ID"].ToString());
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
                        msg1.Append(string.Format("GB#: {0}, Packing No:{1}\n\r", dr["InvNo"].ToString(), dr["ID"].ToString()));
                    }
                    MyUtility.Msg.WarningBox("Below data's est. inspection date not empty but inspection status is empty, can' confirm!!\r\n" + msg1.ToString());
                    return;
                }
            }

            //Garment Booking還沒Confirm就不可以做Confirm
            sqlCmd = string.Format("select ID from GMTBooking where ShipPlanID = '{0}' and Status = 'New'",CurrentMaintain["ID"].ToString());
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
                        msg2.Append(string.Format("GB#:{0}",dr["ID"].ToString()));
                    }
                    MyUtility.Msg.WarningBox("Garment Booking's status not yet confirm, can' confirm!!\r\n" + msg2.ToString());
                    return;
                }
            }
            string updateCmd = string.Format("update ShipPlan set Status = 'Confirmed', EditName = '{0}', EditDate = '{1}' where ID = '{2}'", Sci.Env.User.UserID, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), CurrentMaintain["ID"].ToString());

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
            string updateCmd = string.Format("update ShipPlan set Status = 'Checked', EditName = '{0}', EditDate = '{1}' where ID = '{2}'", Sci.Env.User.UserID, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), CurrentMaintain["ID"].ToString());

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
