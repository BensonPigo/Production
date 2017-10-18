using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;
using Sci;
using Sci.Production.PublicPrg;
using System.Linq;

namespace Sci.Production.PPIC
{
    public partial class P05 : Sci.Win.Tems.QueryForm
    {
        private DataTable gridData;
        Ict.Win.DataGridViewGeneratorDateColumnSettings readyDate = new Ict.Win.DataGridViewGeneratorDateColumnSettings();
        Ict.Win.DataGridViewGeneratorDateColumnSettings estPulloutDate = new Ict.Win.DataGridViewGeneratorDateColumnSettings();
        Ict.Win.DataGridViewGeneratorComboBoxColumnSettings outReasonSet = new Ict.Win.DataGridViewGeneratorComboBoxColumnSettings();
        public P05(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            EditMode = Prgs.GetAuthority(Sci.Env.User.UserID, "P05. Production Schedule", "CanEdit");
            //EditMode = false;
            InitializeComponent();
            //自動帶出3個月後的最後一天
            dateUptoSCIDelivery.Value = (DateTime.Today.AddMonths(4)).AddDays(1 - (DateTime.Today.AddMonths(4)).Day - 1);
            if (!EditMode)
            {
                btnSaveAndQuit.Visible = false;
                btnQuitWithoutSave.Text = "Quit";
            }
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            Color backDefaultColor = gridProductionSchedule.DefaultCellStyle.BackColor;

            readyDate.CellValidating += (s, e) =>
            {
                if (EditMode)
                {
                    DataRow dr = this.gridProductionSchedule.GetDataRow<DataRow>(e.RowIndex);
                    if (MyUtility.Convert.GetDate(e.FormattedValue) != MyUtility.Convert.GetDate(dr["ReadyDate"]))
                    {
                        if (!MyUtility.Check.Empty(e.FormattedValue))
                        {
                            if ((MyUtility.Convert.GetDate(e.FormattedValue) > Convert.ToDateTime(DateTime.Today).AddYears(1) || MyUtility.Convert.GetDate(e.FormattedValue) < Convert.ToDateTime(DateTime.Today).AddYears(-1)))
                            {
                                dr["ReadyDate"] = DBNull.Value;
                                e.Cancel = true;
                                MyUtility.Msg.WarningBox("< Ready date > is invalid!!");
                                return;
                            }
                            else
                            {
                                dr["ReadyDate"] = e.FormattedValue;
                                if (!MyUtility.Check.Empty(dr["BuyerDelivery"]) && MyUtility.Check.Empty(dr["EstPulloutDate"]))
                                {
                                    dr["Diff"] = ((TimeSpan)(MyUtility.Convert.GetDate(dr["BuyerDelivery"]) - MyUtility.Convert.GetDate(dr["ReadyDate"]))).Days;
                                }
                                else
                                {
                                    dr["Diff"] = DBNull.Value;
                                }
                            }
                        }
                        else
                        {
                            dr["ReadyDate"] = DBNull.Value;
                            if (MyUtility.Check.Empty(dr["EstPulloutDate"]))
                            {
                                dr["Diff"] = DBNull.Value;
                            }
                        }
                    }
                }
            };

            estPulloutDate.CellValidating += (s, e) =>
            {
                if (EditMode)
                {
                    DataRow dr = this.gridProductionSchedule.GetDataRow<DataRow>(e.RowIndex);
                    if (MyUtility.Convert.GetDate(e.FormattedValue) != MyUtility.Convert.GetDate(dr["EstPulloutDate"]))
                    {
                        if (!MyUtility.Check.Empty(e.FormattedValue))
                        {
                            if ((MyUtility.Convert.GetDate(e.FormattedValue) > Convert.ToDateTime(DateTime.Today).AddYears(1) || MyUtility.Convert.GetDate(e.FormattedValue) < Convert.ToDateTime(DateTime.Today).AddYears(-1)))
                            {
                                MyUtility.Msg.WarningBox("< Est. Pullout > is invalid!!");
                                dr["EstPulloutDate"] = DBNull.Value;
                                if (!MyUtility.Check.Empty(dr["BuyerDelivery"]) && !MyUtility.Check.Empty(dr["ReadyDate"]))
                                 {
                                     dr["Diff"] = ((TimeSpan)(MyUtility.Convert.GetDate(dr["BuyerDelivery"]) - MyUtility.Convert.GetDate(dr["ReadyDate"]))).Days;
                                 }
                                else
                                {
                                    dr["Diff"] = DBNull.Value;
                                }

                                e.Cancel = true;
                                return;
                            }
                            else
                            {
                                dr["EstPulloutDate"] = e.FormattedValue;
                                if (!MyUtility.Check.Empty(dr["BuyerDelivery"]))
                                {
                                    dr["Diff"] = ((TimeSpan)(MyUtility.Convert.GetDate(dr["BuyerDelivery"]) - MyUtility.Convert.GetDate(dr["EstPulloutDate"]))).Days;
                                }
                                else
                                {
                                    dr["Diff"] = DBNull.Value;
                                }
                            }
                        }
                        else
                        {
                            dr["EstPulloutDate"] = DBNull.Value;
                            if (!MyUtility.Check.Empty(dr["BuyerDelivery"]) && !MyUtility.Check.Empty(dr["ReadyDate"]))
                            {
                                dr["Diff"] = ((TimeSpan)(MyUtility.Convert.GetDate(dr["BuyerDelivery"]) - MyUtility.Convert.GetDate(dr["ReadyDate"]))).Days;
                            }
                            else
                            {
                                dr["Diff"] = DBNull.Value;
                            }
                        }
                    }
                }
            };


            #region OutStandingReason
            //event
            outReasonSet.CellValidating += (s, e) =>
            {
                DataRow dr = this.gridProductionSchedule.GetDataRow<DataRow>(e.RowIndex);
                bool junk = MyUtility.Check.Seek(string.Format(@"
select * 
from Reason 
where	ReasonTypeID='Delivery_OutStand'
		and id = '{0}'
        and Junk = 1", e.FormattedValue));
                if (junk)
                {
                    dr["OutReason"] = "";
                    dr["OutReasonDesc"] = "";
                    MyUtility.Msg.InfoBox(string.Format("The reason 「{0}」 is Junked! It cann't be selected!", e.FormattedValue));
                }
                else
                {
                    dr["OutReason"] = e.FormattedValue;
                    dr["OutReasonDesc"] = MyUtility.GetValue.Lookup(string.Format(@"
select Name 
from Reason 
where	ReasonTypeID='Delivery_OutStand'
		and id = '{0}'", e.FormattedValue));
                }
            };

            //set comboBoxData
            DataTable comboBoxData;
            DBProxy.Current.Select(null, @"
select  '' id
        , '' display

union all
select  id
        , display = concat(id, ' ', name) 
from Reason 
where ReasonTypeID = 'Delivery_OutStand'", out comboBoxData);

            Dictionary<string, string> di_OutReason = new Dictionary<string, string>();
            foreach (DataRow dr in comboBoxData.Rows)
            {
                di_OutReason.Add(dr["id"].ToString(), dr["display"].ToString());
            }

            outReasonSet.DataSource = new BindingSource(di_OutReason, null);
            outReasonSet.ValueMember = "key";
            outReasonSet.DisplayMember = "value";

            outReasonSet.EditingControlShowing += this.EditShowing;
            #endregion  

            //Grid設定
            this.gridProductionSchedule.IsEditingReadOnly = !EditMode;
            this.gridProductionSchedule.DataSource = listControlBindingSource1;

            Helper.Controls.Grid.Generator(this.gridProductionSchedule)
                .Text("ID", header: "SP#", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("StyleID", header: "Style", width: Widths.AnsiChars(17), iseditingreadonly: true)
                .Date("SDPDate", header: "SDP Date", iseditingreadonly: true)
                .Numeric("OrderQty", header: "Total Order Qty", iseditingreadonly: true)
                .Numeric("Qty", header: "Order Qty by Shipmode", iseditingreadonly: true)
                .Text("Inconsistent", header: "*", width: Widths.AnsiChars(1), iseditingreadonly: true)
                .Numeric("AlloQty", header: "Allo Qty", iseditingreadonly: true)
                .Date("KPILETA", header: "KPI L/ETA", iseditingreadonly: true)
                .Date("MTLETA", header: "R/MTL ETA", iseditingreadonly: true)
                .Text("MTLExport", header: "", width: Widths.AnsiChars(4), iseditingreadonly: true)
                .Date("SewETA", header: "Sew. MTL ETA(SP)", iseditingreadonly: true)
                .Date("PackETA", header: "Pkg. MTL ETA(SP)", iseditingreadonly: true)
                .Date("SewInLine", header: "Inline", iseditingreadonly: true)
                .Date("SewOffLine", header: "Offline", iseditingreadonly: true)
                .Date("ReadyDate", header: "Ready", settings: readyDate)
                .Date("EstPulloutDate", header: "Est. Pullout", settings: estPulloutDate)
                .Date("BuyerDelivery", header: "Buy Del", iseditingreadonly: true)
                .Numeric("Diff", header: "Diff", iseditingreadonly: true)
                .Text("SewLine", header: "Line", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Date("SCIDelivery", header: "SCI Del", iseditingreadonly: true)
                .ComboBox("OutReason", header: "Outstanding" + Environment.NewLine + "Reason", settings: outReasonSet)
                .ExtText("OutReasonDesc", header: "Outstanding" + Environment.NewLine + "Reason Desc", iseditingreadonly: true)
                .Text("OutRemark", header: "Outstanding" + Environment.NewLine + "Remark")
                .Text("ProdRemark", header: "Remark", width: Widths.AnsiChars(20));

            gridProductionSchedule.Columns["Inconsistent"].DefaultCellStyle.ForeColor = Color.Red;
            if (EditMode)
            {
                gridProductionSchedule.Columns["ReadyDate"].DefaultCellStyle.ForeColor = Color.Red;
                gridProductionSchedule.Columns["EstPulloutDate"].DefaultCellStyle.ForeColor = Color.Red;
                gridProductionSchedule.Columns["ProdRemark"].DefaultCellStyle.ForeColor = Color.Red;
                gridProductionSchedule.Columns["OutReason"].DefaultCellStyle.ForeColor = Color.Red;
                gridProductionSchedule.Columns["OutRemark"].DefaultCellStyle.ForeColor = Color.Red;
                gridProductionSchedule.Columns["ReadyDate"].DefaultCellStyle.BackColor = Color.Pink;
                gridProductionSchedule.Columns["EstPulloutDate"].DefaultCellStyle.BackColor = Color.Pink;
                gridProductionSchedule.Columns["ProdRemark"].DefaultCellStyle.BackColor = Color.Pink;
                gridProductionSchedule.Columns["OutReason"].DefaultCellStyle.BackColor = Color.Pink;
                gridProductionSchedule.Columns["OutRemark"].DefaultCellStyle.BackColor = Color.Pink;
            }
            
            gridProductionSchedule.CellToolTipTextNeeded += (s, e) =>
            {
                if (e.ColumnIndex == 7)
                {
                    e.ToolTipText = "material shipment arranged by L/ETA";
                }
            };

            gridProductionSchedule.RowsAdded += (s, e) =>
            {
                if (e.RowIndex < 0) return;

                #region 變色規則，若 MTLDelay != 'Y' 則需變回預設的 Color
                int index = e.RowIndex;
                for (int i = 0; i < e.RowCount; i++)
                {
                    DataRow dr = gridProductionSchedule.GetDataRow(index);
                    DataGridViewRow dgvr = gridProductionSchedule.Rows[index];
                    dgvr.Cells["ID"].Style.BackColor = (dr["MTLDelay"].ToString().EqualString("Y")) ? Color.FromArgb(255, 255, 128) : backDefaultColor;
                    index++;
                }
                #endregion 
            };
        }

        //Query
        private void btnQuery_Click(object sender, EventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(string.Format(@"
with tempData as (
    select  oq.Id
            , oq.Seq
            , o.StyleID
            , oq.SDPDate
            , OrderQty = o.Qty
            , oq.Qty
            , AlloQty = (select isnull(MIN(a.AlloQty),0)
                         from (   
                             select  sl.Location
                                     , isnull(SUM(ss.AlloQty),0) as AlloQty
                             from Style_Location sl WITH (NOLOCK) 
	                         left join Orders o WITH (NOLOCK) on o.ID = oq.ID and o.StyleUKey = sl.StyleUkey
	                         left join SewingSchedule ss WITH (NOLOCK) on ss.OrderID = o.ID and ss.ComboType = sl.Location
	                         where sl.StyleUkey = o.StyleUkey
	                         group by sl.Location
                         ) a
                        )
            , o.KPILETA
            , o.MTLETA
            , o.MTLExport
            , o.SewETA
            , o.PackETA
            , o.SewInLine
            , o.SewOffLine
--,oq.EstPulloutDate  改成跟舊系統一樣，先抓Order_QtyShip.EstPulloutDate。若為NULL，再抓Orders.BuyerDelivery
            , EstPulloutDate = iif(oq.EstPulloutDate is null , o.BuyerDelivery , oq.EstPulloutDate) 
            , oq.BuyerDelivery
            , o.SewLine
            , o.SciDelivery
            , oq.ProdRemark
            , ReadyDate = iif (oq.ReadyDate is null, iif (o.SewOffLine is null, null
                                                                              , (iif (DATEPART (WEEKDAY, DATEADD (day, s.ReadyDay, o.SewOffLine)) <= s.ReadyDay, DATEADD(day, s.ReadyDay + 1, o.SewOffLine)
                                                                                                                                                               , DATEADD(day, s.ReadyDay, o.SewOffLine))
                                                                                )
                                                         )
                                                   , oq.ReadyDate)  
            , MTLDelay = iif (p.MTLDelay is null, '', 'Y')
            , OutReason = oq.OutstandingReason
            , OutReasonDesc = ( select Reason.Name 
                                from Reason 
                                where   Reason.ReasonTypeID = 'Delivery_OutStand' 
                                        and Reason.id = oq.OutstandingReason)
            , OutRemark = oq.OutstandingRemark
    from Order_QtyShip oq WITH (NOLOCK) 
    left join Orders o WITH (NOLOCK) on o.ID = oq.Id
    left join PO p WITH (NOLOCK) on p.ID = o.POID
    left join System s WITH (NOLOCK) on 1=1
    where   o.Category in ('B','S','G')
            and o.PulloutComplete = 0
            and oq.Qty > 0
            and o.FtyGroup = '{0}'
            and o.Finished = 0 ", Sci.Env.User.Factory));
            if (!MyUtility.Check.Empty(dateUptoSCIDelivery.Value))
            {
                sqlCmd.Append(string.Format(" and o.SciDelivery <= '{0}'",Convert.ToDateTime(dateUptoSCIDelivery.Value).ToString("d")));
            }
            sqlCmd.Append(@"
)
select  *
        , Diff = iif (EstPulloutDate is not null, datediff(day, EstPulloutDate, BuyerDelivery)
                                                , iif (ReadyDate is not null, datediff (day,ReadyDate, BuyerDelivery)
                                                                            , 0)
                     )
        , Inconsistent = iif (AlloQty = OrderQty, '', '*') 
from tempData 
Order by tempData.Id");
            DualResult result;
            if (result = DBProxy.Current.Select(null, sqlCmd.ToString(), out gridData))
            {
                if (gridData.Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!");
                }
            }
            else
            {
                MyUtility.Msg.ErrorBox("Query fail!\r\n"+result.ToString());
            }
            listControlBindingSource1.DataSource = gridData;
            dateUptoSCIDelivery.ReadOnly = true;
            btnQuery.Enabled = false;
            btnToExcel.Enabled = true;
        }

        //Quit or Quit without Save
        private void btnQuitWithoutSave_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        //Find Now
        private void btnFindNow_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(listControlBindingSource1.DataSource)) return;
            int index = listControlBindingSource1.Find("ID", txtLocateforSP.Text.ToString());
            if (index == -1)
            { MyUtility.Msg.WarningBox("Data was not found!!"); }
            else
            { listControlBindingSource1.Position = index; }
        }

        //Save and Quit
        private void btnSaveAndQuit_Click(object sender, EventArgs e)
        {
            string sql;
            if (!MyUtility.Check.Empty((DataTable)listControlBindingSource1.DataSource))
            {
                IList<string> updateCmds = new List<string>();
                this.gridProductionSchedule.ValidateControl();
                listControlBindingSource1.EndEdit();
                StringBuilder allSP = new StringBuilder();

                #region update SQL
                foreach (DataRow dr in ((DataTable)listControlBindingSource1.DataSource).Rows)
                {
                    if (dr.RowState == DataRowState.Modified)
                    {
                        updateCmds.Add(string.Format(@"
                        update Order_QtyShip 
                        set     EstPulloutDate = {0}
                                , ReadyDate = {1}
                                , ProdRemark = '{2}'
                                , OutstandingReason = '{3}'
                                , OutstandingRemark = '{4}'
                                , EditName = '{5}'
                                , EditDate = GETDATE() 
                        where ID = '{6}' and Seq = '{7}'"
                        , MyUtility.Check.Empty(dr["EstPulloutDate"]) ? "null" : "'" + Convert.ToDateTime(dr["EstPulloutDate"]).ToString("d") + "'"
                        , MyUtility.Check.Empty(dr["ReadyDate"]) ? "null" : "'" + Convert.ToDateTime(dr["ReadyDate"]).ToString("d") + "'"
                        , dr["ProdRemark"].ToString()
                        , dr["OutReason"].ToString()
                        , dr["OutRemark"].ToString()
                        , Sci.Env.User.UserID
                        , dr["ID"].ToString()
                        , dr["Seq"].ToString()));
                        allSP.Append(string.Format("'{0}',", dr["ID"].ToString()));

                        //若Outstanding Reason或Outstanding Remark有值，則更新OutstandingInCharge
                        if (!MyUtility.Check.Empty(dr["OutReason"]) || !MyUtility.Check.Empty(dr["OutRemark"]))
                        {
                            sql = string.Format("update Order_QtyShip set OutstandingInCharge='{0}' where ID = '{1}' and Seq = '{2}' "
                                , Sci.Env.User.UserID, dr["ID"].ToString(), dr["Seq"].ToString());
                            updateCmds.Add(sql);
                        }

                    }
                }
                if (allSP.Length != 0)
                {
                    DataTable GroupData;
                    try
                    {
                        MyUtility.Tool.ProcessWithDatatable((DataTable)listControlBindingSource1.DataSource, "Id,ReadyDate,EstPulloutDate",
                            string.Format("select id,min(ReadyDate) as ReadyDate,min(EstPulloutDate) as EstPulloutDate from #tmp where Id in ({0}) group by Id", allSP.ToString().Substring(0, allSP.ToString().Length - 1)),
                            out GroupData);
                    }
                    catch (Exception ex)
                    {
                        ShowErr("Save error.", ex);
                        return;
                    }

                    foreach (DataRow dr in GroupData.Rows)
                    {
                        updateCmds.Add(string.Format("update Orders set ReadyDate = {0},PulloutDate = {1} where ID = '{2}'",
                            MyUtility.Check.Empty(dr["ReadyDate"]) ? "null" : "'" + Convert.ToDateTime(dr["ReadyDate"]).ToString("d") + "'",
                            MyUtility.Check.Empty(dr["EstPulloutDate"]) ? "null" : "'" + Convert.ToDateTime(dr["EstPulloutDate"]).ToString("d") + "'",
                            dr["Id"].ToString()));
                    }
                }

                if (updateCmds.Count != 0)
                {
                    DualResult result = DBProxy.Current.Executes(null, updateCmds);
                    if (!result)
                    {
                        MyUtility.Msg.ErrorBox("Save Fail!" + result.ToString());
                        return;
                    }
                }
                #endregion

                #region 避免user忘記更新EstPulloutDate,自動update EstPulloutDate and Order.PulloutDate
                updateCmds.Clear();
                foreach (DataRow dr in ((DataTable)listControlBindingSource1.DataSource).Rows)
                {
                    if (MyUtility.Check.Seek(string.Format(@"select 1 from Order_QtyShip where id='{0}' and seq='{1}' and  EstPulloutDate is null", dr["ID"].ToString(), dr["Seq"].ToString())))
                    {
                        updateCmds.Add(string.Format(@"
                        update Order_QtyShip 
                        set     EstPulloutDate = {0}
                                , EditName = '{1}'
                                , EditDate = GETDATE() 
                        where ID = '{2}' and Seq = '{3}'"
                        , MyUtility.Check.Empty(dr["EstPulloutDate"]) ? "null" : "'" + Convert.ToDateTime(dr["EstPulloutDate"]).ToString("d") + "'"
                        , Sci.Env.User.UserID
                        , dr["ID"].ToString()
                        , dr["Seq"].ToString()));
                        allSP.Append(string.Format("'{0}',", dr["ID"].ToString()));
                    }
                }
                if (allSP.Length != 0)
                {
                    DataTable GroupData;
                    try
                    {
                        MyUtility.Tool.ProcessWithDatatable((DataTable)listControlBindingSource1.DataSource, "Id,ReadyDate,EstPulloutDate",
                            string.Format("select id,min(EstPulloutDate) as EstPulloutDate from #tmp where Id in ({0}) group by Id", allSP.ToString().Substring(0, allSP.ToString().Length - 1)),
                            out GroupData);
                    }
                    catch (Exception ex)
                    {
                        ShowErr("Save error.", ex);
                        return;
                    }

                    foreach (DataRow dr in GroupData.Rows)
                    {
                        updateCmds.Add(string.Format("update Orders set  EditName= '{0}', PulloutDate = {1} where ID = '{2}'",
                        Sci.Env.User.UserID, MyUtility.Check.Empty(dr["EstPulloutDate"]) ? "null" : "'" + Convert.ToDateTime(dr["EstPulloutDate"]).ToString("d") + "'",
                            dr["Id"].ToString()));
                    }
                }

                if (updateCmds.Count != 0)
                {
                    DualResult result = DBProxy.Current.Executes(null, updateCmds);
                    if (!result)
                    {
                        MyUtility.Msg.ErrorBox("Save Fail!" + result.ToString());
                        return;
                    }
                }
                #endregion

                this.Close();
            }
        }

        //To Excel
        private void btnToExcel_Click(object sender, EventArgs e)
        {
            if ((DataTable)listControlBindingSource1.DataSource == null || ((DataTable)listControlBindingSource1.DataSource).Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("No data!!");
                return;
            }
            Sci.Production.PPIC.P05_Print callNextForm = new Sci.Production.PPIC.P05_Print((DataTable)listControlBindingSource1.DataSource, MyUtility.Check.Empty(dateUptoSCIDelivery.Value) ? "" : Convert.ToDateTime(dateUptoSCIDelivery.Value).ToString("d"));
            callNextForm.ShowDialog(this);
        }

        void EditShowing(object sender, Ict.Win.UI.DataGridViewEditingControlShowingEventArgs e)
        {
            ((Ict.Win.UI.DataGridViewComboBoxEditingControl)e.Control).DropDownWidth = (int)400;
        }
    }
}
