﻿using System;
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
                                MyUtility.Msg.WarningBox("< Ready date > is invalid!!");
                                dr["ReadyDate"] = DBNull.Value;
                                e.Cancel = true;
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

            //Grid設定
            this.gridProductionSchedule.IsEditingReadOnly = !EditMode;
            this.gridProductionSchedule.DataSource = listControlBindingSource1;

            Helper.Controls.Grid.Generator(this.gridProductionSchedule)
                .Text("ID", header: "SP#", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("StyleID", header: "Style", width: Widths.AnsiChars(15), iseditingreadonly: true)
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
                .Text("ProdRemark", header: "Remark", width: Widths.AnsiChars(20));

            gridProductionSchedule.Columns["Inconsistent"].DefaultCellStyle.ForeColor = Color.Red;
            if (EditMode)
            {
                gridProductionSchedule.Columns["ReadyDate"].DefaultCellStyle.ForeColor = Color.Red;
                gridProductionSchedule.Columns["EstPulloutDate"].DefaultCellStyle.ForeColor = Color.Red;
                gridProductionSchedule.Columns["ProdRemark"].DefaultCellStyle.ForeColor = Color.Red;
                gridProductionSchedule.Columns["ReadyDate"].DefaultCellStyle.BackColor = Color.Pink;
                gridProductionSchedule.Columns["EstPulloutDate"].DefaultCellStyle.BackColor = Color.Pink;
                gridProductionSchedule.Columns["ProdRemark"].DefaultCellStyle.BackColor = Color.Pink;
            }

            gridProductionSchedule.RowPostPaint += (s, e) =>
            {
                DataRow dr = gridProductionSchedule.GetDataRow(e.RowIndex);
                if (gridProductionSchedule.Rows.Count <= e.RowIndex || e.RowIndex < 0) return;

                int i = e.RowIndex;
                if (dr["MTLDelay"].ToString() == "Y")
                {
                    gridProductionSchedule.Rows[i].Cells["ID"].Style.BackColor = Color.FromArgb(255, 255, 128);
                }
            };
            
            gridProductionSchedule.CellToolTipTextNeeded += (s, e) =>
                {
                    if (e.ColumnIndex == 7)
                    {
                        e.ToolTipText = "material shipment arranged by L/ETA";
                    }
                };
        }

        //Query
        private void button1_Click(object sender, EventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(string.Format(@"with tempData
as 
(select oq.Id,oq.Seq,o.StyleID,oq.SDPDate,o.Qty as OrderQty,oq.Qty,
 (select isnull(MIN(a.AlloQty),0)
  from (select sl.Location,isnull(SUM(ss.AlloQty),0) as AlloQty
        from Style_Location sl WITH (NOLOCK) 
	    left join Orders o WITH (NOLOCK) on o.ID = oq.ID and o.StyleUKey = sl.StyleUkey
	    left join SewingSchedule ss WITH (NOLOCK) on ss.OrderID = o.ID and ss.ComboType = sl.Location
	    where sl.StyleUkey = o.StyleUkey
	    group by sl.Location) a) as AlloQty,o.KPILETA,o.MTLETA,o.MTLExport,o.SewETA,o.PackETA,
 o.SewInLine,o.SewOffLine

--,oq.EstPulloutDate  改成跟舊系統一樣，先抓Order_QtyShip.EstPulloutDate。若為NULL，再抓Orders.BuyerDelivery
,iif(oq.EstPulloutDate is null , o.BuyerDelivery , oq.EstPulloutDate) EstPulloutDate

,oq.BuyerDelivery,o.SewLine,o.SciDelivery,oq.ProdRemark,
 iif(oq.ReadyDate is null,iif(o.SewOffLine is null, null,(iif(DATEPART(WEEKDAY,DATEADD(day,s.ReadyDay,o.SewOffLine)) <= s.ReadyDay,DATEADD(day,s.ReadyDay+1,o.SewOffLine),DATEADD(day,s.ReadyDay,o.SewOffLine)))),oq.ReadyDate) as ReadyDate,
 iif(p.MTLDelay is null,'','Y') as MTLDelay
 from Order_QtyShip oq WITH (NOLOCK) 
 left join Orders o WITH (NOLOCK) on o.ID = oq.Id
 left join PO p WITH (NOLOCK) on p.ID = o.POID
 left join System s WITH (NOLOCK) on 1=1
 where (o.Category = 'B' or o.Category = 'S')
 and o.PulloutComplete = 0
 and oq.Qty > 0
 and o.FtyGroup = '{0}'
 and o.Finished = 0 ", Sci.Env.User.Factory));
            if (!MyUtility.Check.Empty(dateUptoSCIDelivery.Value))
            {
                sqlCmd.Append(string.Format(" and o.SciDelivery <= '{0}'",Convert.ToDateTime(dateUptoSCIDelivery.Value).ToString("d")));
            }
            sqlCmd.Append(@")
select *,
iif(EstPulloutDate is not null, datediff(day,EstPulloutDate,BuyerDelivery), iif(ReadyDate is not null,datediff(day,ReadyDate,BuyerDelivery),0)) as Diff,
iif(AlloQty = OrderQty,'','*') as Inconsistent from tempData Order by tempData.Id");
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
            this.gridProductionSchedule.AutoResizeColumns();
        }

        //Quit or Quit without Save
        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        //Find Now
        private void button2_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(listControlBindingSource1.DataSource)) return;
            int index = listControlBindingSource1.Find("ID", txtLocateforSP.Text.ToString());
            if (index == -1)
            { MyUtility.Msg.WarningBox("Data was not found!!"); }
            else
            { listControlBindingSource1.Position = index; }
        }

        //Save and Quit
        private void button3_Click(object sender, EventArgs e)
        {
            if (!MyUtility.Check.Empty((DataTable)listControlBindingSource1.DataSource))
            {
                IList<string> updateCmds = new List<string>();
                this.gridProductionSchedule.ValidateControl();
                listControlBindingSource1.EndEdit();
                StringBuilder allSP = new StringBuilder();
                foreach (DataRow dr in ((DataTable)listControlBindingSource1.DataSource).Rows)
                {
                    if (dr.RowState == DataRowState.Modified)
                    {
                        updateCmds.Add(string.Format(@"update Order_QtyShip set EstPulloutDate = {0}, ReadyDate = {1}, ProdRemark = '{2}', EditName = '{3}', EditDate = GETDATE() where ID = '{4}' and Seq = '{5}'",
                            MyUtility.Check.Empty(dr["EstPulloutDate"]) ? "null" : "'" + Convert.ToDateTime(dr["EstPulloutDate"]).ToString("d") + "'",
                            MyUtility.Check.Empty(dr["ReadyDate"]) ? "null" : "'" + Convert.ToDateTime(dr["ReadyDate"]).ToString("d") + "'",
                            dr["ProdRemark"].ToString(),Sci.Env.User.UserID, dr["ID"].ToString(), dr["Seq"].ToString()));
                        allSP.Append(string.Format("'{0}',", dr["ID"].ToString()));
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
                this.Close();
            }
        }

        //To Excel
        private void button5_Click(object sender, EventArgs e)
        {
            if ((DataTable)listControlBindingSource1.DataSource == null || ((DataTable)listControlBindingSource1.DataSource).Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("No data!!");
                return;
            }
            Sci.Production.PPIC.P05_Print callNextForm = new Sci.Production.PPIC.P05_Print((DataTable)listControlBindingSource1.DataSource);
            callNextForm.ShowDialog(this);
        }
    }
}
