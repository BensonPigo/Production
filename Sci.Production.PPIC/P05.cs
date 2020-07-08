using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.PublicPrg;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// P05
    /// </summary>
    public partial class P05 : Win.Tems.QueryForm
    {
        private DataTable gridData;
        private DataGridViewGeneratorDateColumnSettings readyDate = new DataGridViewGeneratorDateColumnSettings();
        private DataGridViewGeneratorDateColumnSettings estPulloutDate = new DataGridViewGeneratorDateColumnSettings();
        private DataGridViewGeneratorComboBoxColumnSettings outReasonSet = new DataGridViewGeneratorComboBoxColumnSettings();

        /// <summary>
        /// P05
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public P05(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.EditMode = Prgs.GetAuthority(Sci.Env.User.UserID, "P05. Production Schedule", "CanEdit");

            // EditMode = false;
            this.InitializeComponent();

            // 自動帶出3個月後的最後一天
            this.dateUptoSCIDelivery.Value = DateTime.Today.AddMonths(4).AddDays(1 - DateTime.Today.AddMonths(4).Day - 1);
            if (!this.EditMode)
            {
                this.btnSaveAndQuit.Visible = false;
                this.btnQuitWithoutSave.Text = "Quit";
            }
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            Color backDefaultColor = this.gridProductionSchedule.DefaultCellStyle.BackColor;

            this.readyDate.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.gridProductionSchedule.GetDataRow<DataRow>(e.RowIndex);
                    if (MyUtility.Convert.GetDate(e.FormattedValue) != MyUtility.Convert.GetDate(dr["ReadyDate"]))
                    {
                        if (!MyUtility.Check.Empty(e.FormattedValue))
                        {
                            if (MyUtility.Convert.GetDate(e.FormattedValue) > Convert.ToDateTime(DateTime.Today).AddYears(1) || MyUtility.Convert.GetDate(e.FormattedValue) < Convert.ToDateTime(DateTime.Today).AddYears(-1))
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

            this.estPulloutDate.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.gridProductionSchedule.GetDataRow<DataRow>(e.RowIndex);
                    if (MyUtility.Convert.GetDate(e.FormattedValue) != MyUtility.Convert.GetDate(dr["EstPulloutDate"]))
                    {
                        if (!MyUtility.Check.Empty(e.FormattedValue))
                        {
                            if (MyUtility.Convert.GetDate(e.FormattedValue) > Convert.ToDateTime(DateTime.Today).AddYears(1) || MyUtility.Convert.GetDate(e.FormattedValue) < Convert.ToDateTime(DateTime.Today).AddYears(-1))
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

            // event
            this.outReasonSet.CellValidating += (s, e) =>
            {
                DataRow dr = this.gridProductionSchedule.GetDataRow<DataRow>(e.RowIndex);
                bool junk = MyUtility.Check.Seek(string.Format(
                    @"
select * 
from Reason 
where	ReasonTypeID='Delivery_OutStand'
		and id = '{0}'
        and Junk = 1", e.FormattedValue));
                if (junk)
                {
                    dr["OutReason"] = string.Empty;
                    dr["OutReasonDesc"] = string.Empty;
                    MyUtility.Msg.InfoBox(string.Format("The reason 「{0}」 is Junked! It cann't be selected!", e.FormattedValue));
                }
                else
                {
                    dr["OutReason"] = e.FormattedValue;
                    dr["OutReasonDesc"] = MyUtility.GetValue.Lookup(string.Format(
                        @"
select Name 
from Reason 
where	ReasonTypeID='Delivery_OutStand'
		and id = '{0}'", e.FormattedValue));
                }
            };

            // set comboBoxData
            DataTable comboBoxData;
            string strSqlSelect = @"
select  '' id
        , '' display

union all
select  id
        , display = concat(id, ' ', name) 
from Reason 
where ReasonTypeID = 'Delivery_OutStand'";

            DBProxy.Current.Select(null, strSqlSelect, out comboBoxData);

            Dictionary<string, string> di_OutReason = new Dictionary<string, string>();
            foreach (DataRow dr in comboBoxData.Rows)
            {
                di_OutReason.Add(dr["id"].ToString(), dr["display"].ToString());
            }

            this.outReasonSet.DataSource = new BindingSource(di_OutReason, null);
            this.outReasonSet.ValueMember = "key";
            this.outReasonSet.DisplayMember = "value";

            this.outReasonSet.EditingControlShowing += this.EditShowing;
            #endregion

            // Grid設定
            this.gridProductionSchedule.IsEditingReadOnly = !this.EditMode;
            this.gridProductionSchedule.DataSource = this.listControlBindingSource1;

            this.Helper.Controls.Grid.Generator(this.gridProductionSchedule)
                .Text("ID", header: "SP#", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("StyleID", header: "Style", width: Widths.AnsiChars(17), iseditingreadonly: true)
                .Date("SDPDate", header: "SDP Date", iseditingreadonly: true)
                .Numeric("OrderQty", header: "Total Order Qty", iseditingreadonly: true)
                .Numeric("Qty", header: "Order Qty by Shipmode", iseditingreadonly: true)
                .Text("Inconsistent", header: "*", width: Widths.AnsiChars(1), iseditingreadonly: true)
                .Numeric("AlloQty", header: "Allo Qty", iseditingreadonly: true)
                .Date("KPILETA", header: "KPI L/ETA", iseditingreadonly: true)
                .Date("MTLETA", header: "Act. MTL ETA(Master SP)", iseditingreadonly: true)
                .Text("MTLExport", header: string.Empty, width: Widths.AnsiChars(4), iseditingreadonly: true)
                .Date("SewETA", header: "Sew. MTL ETA(SP)", iseditingreadonly: true)
                .Date("PackETA", header: "Pkg. MTL ETA(SP)", iseditingreadonly: true)
                .Date("SewInLine", header: "Inline", iseditingreadonly: true)
                .Date("SewOffLine", header: "Offline", iseditingreadonly: true)
                .Date("ReadyDate", header: "Ready", settings: this.readyDate)
                .Date("EstPulloutDate", header: "Est. Pullout", settings: this.estPulloutDate)
                .Date("BuyerDelivery", header: "Buy Del", iseditingreadonly: true)
                .Numeric("Diff", header: "Diff", iseditingreadonly: true)
                .Text("SewLine", header: "Line", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Date("SCIDelivery", header: "SCI Del", iseditingreadonly: true)
                .ComboBox("OutReason", header: "Outstanding" + Environment.NewLine + "Reason", settings: this.outReasonSet)
                .ExtText("OutReasonDesc", header: "Outstanding" + Environment.NewLine + "Reason Desc", iseditingreadonly: true)
                .Text("OutRemark", header: "Outstanding" + Environment.NewLine + "Remark")
                .Text("ProdRemark", header: "Remark", width: Widths.AnsiChars(20));

            this.gridProductionSchedule.Columns["Inconsistent"].DefaultCellStyle.ForeColor = Color.Red;
            if (this.EditMode)
            {
                this.gridProductionSchedule.Columns["ReadyDate"].DefaultCellStyle.ForeColor = Color.Red;
                this.gridProductionSchedule.Columns["EstPulloutDate"].DefaultCellStyle.ForeColor = Color.Red;
                this.gridProductionSchedule.Columns["ProdRemark"].DefaultCellStyle.ForeColor = Color.Red;
                this.gridProductionSchedule.Columns["OutReason"].DefaultCellStyle.ForeColor = Color.Red;
                this.gridProductionSchedule.Columns["OutRemark"].DefaultCellStyle.ForeColor = Color.Red;
                this.gridProductionSchedule.Columns["ReadyDate"].DefaultCellStyle.BackColor = Color.Pink;
                this.gridProductionSchedule.Columns["EstPulloutDate"].DefaultCellStyle.BackColor = Color.Pink;
                this.gridProductionSchedule.Columns["ProdRemark"].DefaultCellStyle.BackColor = Color.Pink;
                this.gridProductionSchedule.Columns["OutReason"].DefaultCellStyle.BackColor = Color.Pink;
                this.gridProductionSchedule.Columns["OutRemark"].DefaultCellStyle.BackColor = Color.Pink;
            }

            this.gridProductionSchedule.CellToolTipTextNeeded += (s, e) =>
            {
                if (e.ColumnIndex == 7)
                {
                    e.ToolTipText = "material shipment arranged by L/ETA";
                }
            };

            this.gridProductionSchedule.RowsAdded += (s, e) =>
            {
                if (e.RowIndex < 0)
                {
                    return;
                }

                #region 變色規則，若 MTLDelay != 'Y' 則需變回預設的 Color
                int index = e.RowIndex;
                for (int i = 0; i < e.RowCount; i++)
                {
                    DataRow dr = this.gridProductionSchedule.GetDataRow(index);
                    DataGridViewRow dgvr = this.gridProductionSchedule.Rows[index];
                    dgvr.Cells["ID"].Style.BackColor = dr["MTLDelay"].ToString().EqualString("Y") ? Color.FromArgb(255, 255, 128) : backDefaultColor;
                    index++;
                }
                #endregion
            };
        }

        // Query
        private void BtnQuery_Click(object sender, EventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(string.Format(
                @"
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
            if (!MyUtility.Check.Empty(this.dateUptoSCIDelivery.Value))
            {
                sqlCmd.Append(string.Format(" and o.SciDelivery <= '{0}'", Convert.ToDateTime(this.dateUptoSCIDelivery.Value).ToString("d")));
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
            if (result = DBProxy.Current.Select(null, sqlCmd.ToString(), out this.gridData))
            {
                if (this.gridData.Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!");
                }
            }
            else
            {
                MyUtility.Msg.ErrorBox("Query fail!\r\n" + result.ToString());
            }

            this.listControlBindingSource1.DataSource = this.gridData;
            this.dateUptoSCIDelivery.ReadOnly = true;
            this.btnQuery.Enabled = false;
            this.btnToExcel.Enabled = true;
        }

        // Quit or Quit without Save
        private void BtnQuitWithoutSave_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Find Now
        private void BtnFindNow_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.listControlBindingSource1.DataSource))
            {
                return;
            }

            int index = this.listControlBindingSource1.Find("ID", this.txtLocateforSP.Text.ToString());
            if (index == -1)
            {
                MyUtility.Msg.WarningBox("Data was not found!!");
            }
            else
            {
                this.listControlBindingSource1.Position = index;
            }
        }

        // Save and Quit
        private void BtnSaveAndQuit_Click(object sender, EventArgs e)
        {
            string sql;
            if (!MyUtility.Check.Empty((DataTable)this.listControlBindingSource1.DataSource))
            {
                IList<string> updateCmds = new List<string>();
                this.gridProductionSchedule.ValidateControl();
                this.listControlBindingSource1.EndEdit();
                StringBuilder allSP = new StringBuilder();

                #region update SQL
                foreach (DataRow dr in ((DataTable)this.listControlBindingSource1.DataSource).Rows)
                {
                    if (dr.RowState == DataRowState.Modified)
                    {
                        updateCmds.Add(string.Format(
                            @"
                        update Order_QtyShip 
                        set     EstPulloutDate = {0}
                                , ReadyDate = {1}
                                , ProdRemark = '{2}'
                                , OutstandingReason = '{3}'
                                , OutstandingRemark = '{4}'
                                , EditName = '{5}'
                                , EditDate = GETDATE() 
                        where ID = '{6}' and Seq = '{7}'",
                            MyUtility.Check.Empty(dr["EstPulloutDate"]) ? "null" : "'" + Convert.ToDateTime(dr["EstPulloutDate"]).ToString("d") + "'",
                            MyUtility.Check.Empty(dr["ReadyDate"]) ? "null" : "'" + Convert.ToDateTime(dr["ReadyDate"]).ToString("d") + "'",
                            dr["ProdRemark"].ToString(),
                            dr["OutReason"].ToString(),
                            dr["OutRemark"].ToString(),
                            Sci.Env.User.UserID,
                            dr["ID"].ToString(),
                            dr["Seq"].ToString()));
                        allSP.Append(string.Format("'{0}',", dr["ID"].ToString()));

                        // 若Outstanding Reason或Outstanding Remark有值，則更新OutstandingInCharge
                        if (!MyUtility.Check.Empty(dr["OutReason"]) || !MyUtility.Check.Empty(dr["OutRemark"]))
                        {
                            sql = string.Format(
                                "update Order_QtyShip set OutstandingInCharge='{0}' where ID = '{1}' and Seq = '{2}' ",
                                Sci.Env.User.UserID,
                                dr["ID"].ToString(),
                                dr["Seq"].ToString());
                            updateCmds.Add(sql);
                        }
                    }
                }

                if (allSP.Length != 0)
                {
                    DataTable groupData;
                    try
                    {
                        MyUtility.Tool.ProcessWithDatatable(
                            (DataTable)this.listControlBindingSource1.DataSource,
                            "Id,ReadyDate,EstPulloutDate",
                            string.Format("select id,min(ReadyDate) as ReadyDate,min(EstPulloutDate) as EstPulloutDate from #tmp where Id in ({0}) group by Id", allSP.ToString().Substring(0, allSP.ToString().Length - 1)),
                            out groupData);
                    }
                    catch (Exception ex)
                    {
                        this.ShowErr("Save error.", ex);
                        return;
                    }

                    foreach (DataRow dr in groupData.Rows)
                    {
                        updateCmds.Add(string.Format(
                            "update Orders set ReadyDate = {0},PulloutDate = {1} where ID = '{2}'",
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
                foreach (DataRow dr in ((DataTable)this.listControlBindingSource1.DataSource).Rows)
                {
                    if (MyUtility.Check.Seek(string.Format(@"select 1 from Order_QtyShip where id='{0}' and seq='{1}' and  EstPulloutDate is null", dr["ID"].ToString(), dr["Seq"].ToString())))
                    {
                        updateCmds.Add(string.Format(
                            @"
                        update Order_QtyShip 
                        set     EstPulloutDate = {0}
                                , EditName = '{1}'
                                , EditDate = GETDATE() 
                        where ID = '{2}' and Seq = '{3}'",
                            MyUtility.Check.Empty(dr["EstPulloutDate"]) ? "null" : "'" + Convert.ToDateTime(dr["EstPulloutDate"]).ToString("d") + "'",
                            Sci.Env.User.UserID,
                            dr["ID"].ToString(),
                            dr["Seq"].ToString()));
                        allSP.Append(string.Format("'{0}',", dr["ID"].ToString()));
                    }
                }

                if (allSP.Length != 0)
                {
                    DataTable groupData1;
                    try
                    {
                        MyUtility.Tool.ProcessWithDatatable(
                            (DataTable)this.listControlBindingSource1.DataSource,
                            "Id,ReadyDate,EstPulloutDate",
                            string.Format("select id,min(EstPulloutDate) as EstPulloutDate from #tmp where Id in ({0}) group by Id", allSP.ToString().Substring(0, allSP.ToString().Length - 1)),
                            out groupData1);
                    }
                    catch (Exception ex)
                    {
                        this.ShowErr("Save error.", ex);
                        return;
                    }

                    foreach (DataRow dr in groupData1.Rows)
                    {
                        updateCmds.Add(string.Format(
                            "update Orders set  EditName= '{0}', PulloutDate = {1} where ID = '{2}'",
                            Sci.Env.User.UserID,
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

                this.Close();
            }
        }

        // To Excel
        private void BtnToExcel_Click(object sender, EventArgs e)
        {
            if ((DataTable)this.listControlBindingSource1.DataSource == null || ((DataTable)this.listControlBindingSource1.DataSource).Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("No data!!");
                return;
            }

            P05_Print callNextForm = new P05_Print((DataTable)this.listControlBindingSource1.DataSource, MyUtility.Check.Empty(this.dateUptoSCIDelivery.Value) ? string.Empty : Convert.ToDateTime(this.dateUptoSCIDelivery.Value).ToString("d"));
            callNextForm.ShowDialog(this);
        }

        private void EditShowing(object sender, Ict.Win.UI.DataGridViewEditingControlShowingEventArgs e)
        {
            ((Ict.Win.UI.DataGridViewComboBoxEditingControl)e.Control).DropDownWidth = (int)400;
        }
    }
}
