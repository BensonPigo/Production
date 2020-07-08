using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Sewing
{
    /// <summary>
    /// P02
    /// </summary>
    public partial class P02 : Sci.Win.Tems.Input6
    {
        private Ict.Win.DataGridViewGeneratorTextColumnSettings orderID = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
        private Ict.Win.DataGridViewGeneratorNumericColumnSettings qaqty = new Ict.Win.DataGridViewGeneratorNumericColumnSettings();
        private Ict.Win.DataGridViewGeneratorNumericColumnSettings inlineqty = new Ict.Win.DataGridViewGeneratorNumericColumnSettings();

        private DateTime? SewingMonthlyLockDate;
        private decimal systemTMS = 0;
        private decimal? oldttlqaqty;

        /// <summary>
        /// P02
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P02(ToolStripMenuItem menuitem)
              : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = "FactoryID = '" + Sci.Env.User.Factory + "' and Category = 'M'";
            DataRow sysData;
            if (MyUtility.Check.Seek("select StdTMS from System WITH (NOLOCK) ", out sysData))
            {
                this.systemTMS = MyUtility.Convert.GetDecimal(sysData["StdTMS"]);
            }
            else
            {
                MyUtility.Msg.ErrorBox("Query System fail, pls contact Taipei MIS!!");
            }
        }

        /// <inheritdoc/>
        protected override void EnsureToolbarExt()
        {
            base.EnsureToolbarExt();
            if (this.Perm.Send)
            {
                this.toolbar.cmdSend.Enabled = true;
            }

            if (this.CurrentMaintain != null)
            {
                string strSqlcmd = $@"select * from SewingOutput_DailyUnlock
where UnLockDate is null and SewingOutputID='{this.CurrentMaintain["ID"]}'";
                if (MyUtility.Check.Seek(strSqlcmd) &&
                    this.Perm.Recall &&
                    string.Compare(this.CurrentMaintain["Status"].ToString(), "Sent") == 0)
                {
                    this.toolbar.cmdRecall.Enabled = true;
                }
                else
                {
                    this.toolbar.cmdRecall.Enabled = false;
                }
            }
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();
            this.DetailSelectCommand = string.Format(
                @"select sd.*,mo.MockupID,mo.Qty,(select isnull(sum(QAQty),0) from SewingOutput_Detail WITH (NOLOCK) where OrderId = sd.OrderId and ID != sd.ID) as AccuQty,
mo.Qty-(select isnull(sum(QAQty),0) from SewingOutput_Detail WITH (NOLOCK) where OrderId = sd.OrderId and ID != sd.ID) as VarQty,
mo.Qty-(select isnull(sum(QAQty),0) from SewingOutput_Detail WITH (NOLOCK) where OrderId = sd.OrderId and ID != sd.ID)-sd.QAQty as BalQty,
round(iif(sd.InlineQty = 0,0,sd.QAQty/sd.InlineQty),2)*100 as RFT
from SewingOutput_Detail sd WITH (NOLOCK) 
left join MockupOrder mo WITH (NOLOCK) on mo.ID = sd.OrderId
where sd.ID = '{0}'",
                masterID);

            return base.OnDetailSelectCommandPrepare(e);
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();
            #region Right click & Validating
            this.orderID.EditingMouseDown += (s, e) =>
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    if (this.EditMode)
                    {
                        if (e.RowIndex != -1)
                        {
                            DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                            string sqlCmd = string.Format(
                                @"
select  mo.ID
        , mo.MockupID
        , mo.StyleID
        , mo.SeasonID
        , mo.BrandID 
from MockupOrder mo WITH (NOLOCK) 
inner join Factory f on mo.factoryid = f.ID
where   mo.Junk = 0 
        and mo.FTYGroup = '{0}'
        and f.IsProduceFty = 1",
                                Sci.Env.User.Factory);

                            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlCmd, "15,13,15,5,10", dr["OrderID"].ToString(), "ID,MockupID,Style,Season,Brand");
                            item.Size = new System.Drawing.Size(700, 600);
                            DialogResult returnResult = item.ShowDialog();
                            if (returnResult == DialogResult.Cancel)
                            {
                                return;
                            }
                            else
                            {
                                if (e.EditingControl.Text.Trim() != item.GetSelectedString().Trim())
                                {
                                    e.EditingControl.Text = item.GetSelectedString();

                                    // sql參數
                                    System.Data.SqlClient.SqlParameter sp1e = new System.Data.SqlClient.SqlParameter("@factoryid", Sci.Env.User.Factory);
                                    System.Data.SqlClient.SqlParameter sp2e = new System.Data.SqlClient.SqlParameter("@id", e.EditingControl.Text);

                                    IList<System.Data.SqlClient.SqlParameter> cmdse = new List<System.Data.SqlClient.SqlParameter>();
                                    cmdse.Add(sp1e);
                                    cmdse.Add(sp2e);
                                    DataTable moDatae;
                                    string sqlCmde = "select * from MockupOrder WITH (NOLOCK) where Junk = 0 and FTYGroup = @factoryid and ID = @id";
                                    DualResult result = DBProxy.Current.Select(null, sqlCmde, cmdse, out moDatae);
                                    if (!result || moDatae.Rows.Count <= 0)
                                    {
                                        if (!result)
                                        {
                                            MyUtility.Msg.WarningBox("Sql connection fail!\r\n" + result.ToString());
                                        }
                                        else
                                        {
                                            MyUtility.Msg.WarningBox(string.Format("Data not found!!!"));
                                        }

                                        dr["OrderID"] = string.Empty;
                                        dr["TMS"] = 0;
                                        dr["MockupID"] = string.Empty;
                                        dr["Qty"] = 0;
                                        dr["AccuQty"] = 0;
                                        dr["VarQty"] = 0;
                                        dr["QAQty"] = 0;
                                        dr["BalQty"] = 0;
                                        dr["InlineQty"] = 0;
                                        dr["DefectQty"] = 0;
                                        dr["WorkHour"] = 0;
                                        dr["RFT"] = 0;
                                        e.EditingControl.ValidateControl();
                                        return;
                                    }
                                    else
                                    {
                                        dr["OrderID"] = e.EditingControl.Text;
                                        dr["TMS"] = MyUtility.Math.Round(MyUtility.Convert.GetDecimal(moDatae.Rows[0]["CPU"]) * MyUtility.Convert.GetDecimal(moDatae.Rows[0]["CPUFactor"]) * this.systemTMS);
                                        dr["MockupID"] = MyUtility.Convert.GetString(moDatae.Rows[0]["MockupID"]);
                                        dr["Qty"] = MyUtility.Convert.GetInt(moDatae.Rows[0]["Qty"]);
                                        dr["AccuQty"] = MyUtility.Convert.GetInt(MyUtility.GetValue.Lookup(string.Format("select isnull(sum(QAQty),0) from SewingOutput_Detail WITH (NOLOCK) where OrderId = '{0}'", MyUtility.Convert.GetString(dr["OrderID"]))));
                                        dr["VarQty"] = MyUtility.Convert.GetInt(dr["Qty"]) - MyUtility.Convert.GetInt(dr["AccuQty"]);
                                        dr["QAQty"] = 0;
                                        dr["BalQty"] = MyUtility.Convert.GetInt(dr["Qty"]) - MyUtility.Convert.GetInt(dr["AccuQty"]) - MyUtility.Convert.GetInt(dr["QAQty"]);
                                        dr["InlineQty"] = 0;
                                        dr["DefectQty"] = 0;
                                        dr["WorkHour"] = 0;
                                        dr["RFT"] = 0;
                                    }
                                }
                            }
                        }
                    }
                }
            };

            this.orderID.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    if (!MyUtility.Check.Empty(e.FormattedValue) && MyUtility.Convert.GetString(e.FormattedValue) != MyUtility.Convert.GetString(dr["OrderID"]))
                    {
                        // sql參數
                        System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter("@factoryid", Sci.Env.User.Factory);
                        System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter("@id", MyUtility.Convert.GetString(e.FormattedValue));

                        IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                        cmds.Add(sp1);
                        cmds.Add(sp2);

                        DataTable moData;
                        string sqlCmd = @"
select mo.* 
from MockupOrder mo WITH (NOLOCK) 
inner join Factory f on mo.factoryid = f.ID
where   mo.Junk = 0 
        and mo.FTYGroup = @factoryid 
        and mo.ID = @id
        and f.IsProduceFty = 1";
                        DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out moData);
                        if (!result || moData.Rows.Count <= 0)
                        {
                            if (!result)
                            {
                                MyUtility.Msg.WarningBox("Sql connection fail!\r\n" + result.ToString());
                            }
                            else
                            {
                                MyUtility.Msg.WarningBox(string.Format("Data not found!!!"));
                            }

                            dr["OrderID"] = string.Empty;
                            dr["TMS"] = 0;
                            dr["MockupID"] = string.Empty;
                            dr["Qty"] = 0;
                            dr["AccuQty"] = 0;
                            dr["VarQty"] = 0;
                            dr["QAQty"] = 0;
                            dr["BalQty"] = 0;
                            dr["InlineQty"] = 0;
                            dr["DefectQty"] = 0;
                            dr["WorkHour"] = 0;
                            dr["RFT"] = 0;
                            e.Cancel = true;
                            return;
                        }
                        else
                        {
                            dr["OrderID"] = e.FormattedValue.ToString();
                            dr["TMS"] = MyUtility.Math.Round(MyUtility.Convert.GetDecimal(moData.Rows[0]["CPU"]) * MyUtility.Convert.GetDecimal(moData.Rows[0]["CPUFactor"]) * this.systemTMS);
                            dr["MockupID"] = MyUtility.Convert.GetString(moData.Rows[0]["MockupID"]);
                            dr["Qty"] = MyUtility.Convert.GetInt(moData.Rows[0]["Qty"]);
                            dr["AccuQty"] = MyUtility.Convert.GetInt(MyUtility.GetValue.Lookup(string.Format("select isnull(sum(QAQty),0) from SewingOutput_Detail WITH (NOLOCK) where OrderId = '{0}'", MyUtility.Convert.GetString(dr["OrderID"]))));
                            dr["VarQty"] = MyUtility.Convert.GetInt(dr["Qty"]) - MyUtility.Convert.GetInt(dr["AccuQty"]);
                            dr["QAQty"] = 0;
                            dr["BalQty"] = MyUtility.Convert.GetInt(dr["Qty"]) - MyUtility.Convert.GetInt(dr["AccuQty"]) - MyUtility.Convert.GetInt(dr["QAQty"]);
                            dr["InlineQty"] = 0;
                            dr["DefectQty"] = 0;
                            dr["WorkHour"] = 0;
                            dr["RFT"] = 0;
                        }
                    }
                    else if (MyUtility.Check.Empty(e.FormattedValue))
                    {
                        dr["OrderID"] = string.Empty;
                        dr["TMS"] = 0;
                        dr["MockupID"] = string.Empty;
                        dr["Qty"] = 0;
                        dr["AccuQty"] = 0;
                        dr["VarQty"] = 0;
                        dr["QAQty"] = 0;
                        dr["BalQty"] = 0;
                        dr["InlineQty"] = 0;
                        dr["DefectQty"] = 0;
                        dr["WorkHour"] = 0;
                        dr["RFT"] = 0;
                    }
                }
            };

            this.qaqty.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    if (!MyUtility.Check.Empty(e.FormattedValue) && MyUtility.Convert.GetInt(e.FormattedValue) != MyUtility.Convert.GetInt(dr["QAQty"]))
                    {
                        if (MyUtility.Convert.GetInt(e.FormattedValue) > MyUtility.Convert.GetInt(dr["VarQty"]))
                        {
                            dr["QAQty"] = dr["QAQty"];
                            MyUtility.Msg.WarningBox("Output Qty can't exceed Variance!");
                            return;
                        }
                        else
                        {
                            dr["QAQty"] = MyUtility.Convert.GetInt(e.FormattedValue);
                            dr["BalQty"] = MyUtility.Convert.GetInt(dr["Qty"]) - MyUtility.Convert.GetInt(dr["AccuQty"]) - MyUtility.Convert.GetInt(dr["QAQty"]);
                            this.ReCalculateDefectAndRFT(dr);
                        }
                    }
                }
            };

            this.inlineqty.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    if (!MyUtility.Check.Empty(e.FormattedValue) && MyUtility.Convert.GetInt(e.FormattedValue) != MyUtility.Convert.GetInt(dr["InlineQty"]))
                    {
                        dr["InlineQty"] = MyUtility.Convert.GetInt(e.FormattedValue);
                        this.ReCalculateDefectAndRFT(dr);
                    }
                }
            };

            #endregion

            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("OrderID", header: "Mockup order#", width: Widths.AnsiChars(13), settings: this.orderID)
                .Text("MockupID", header: "Mockup type", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Numeric("TMS", header: "TMS", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Numeric("Qty", header: "Order Qty", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Numeric("AccuQty", header: "Accu. Qty", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Numeric("VarQty", header: "Variance", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Numeric("QAQty", header: "QA Output", width: Widths.AnsiChars(5), settings: this.qaqty)
                .Numeric("BalQty", header: "Bal. Qty", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Numeric("InlineQty", header: "Prod. Output", width: Widths.AnsiChars(5), settings: this.inlineqty)
                .Numeric("DefectQty", header: "Defect Qty", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Numeric("WorkHour", header: "W'Hours", decimal_places: 3, width: Widths.AnsiChars(5))
                .Numeric("RFT", header: "RFT(%)", width: Widths.AnsiChars(5), iseditingreadonly: true);
        }

        /// <inheritdoc/>
        protected override void OnDetailGridDelete()
        {
            base.OnDetailGridDelete();
            this.CurrentMaintain["QAQty"] = ((DataTable)this.detailgridbs.DataSource).DefaultView.ToTable().Compute("sum(QAQty)", string.Empty);
            this.CurrentMaintain["InlineQty"] = ((DataTable)this.detailgridbs.DataSource).DefaultView.ToTable().Compute("sum(InlineQty)", string.Empty);

            DataTable tms;
            try
            {
                MyUtility.Tool.ProcessWithDatatable(((DataTable)this.detailgridbs.DataSource).DefaultView.ToTable(), "QAQty,TMS,OrderID", "select round(isnull(sum(cast(QAQty as float)*cast(TMS as float)),0)/sum(cast(QAQty as float)),0) as TtlTMS from #tmp", out tms, "#tmp");
            }
            catch (Exception ex)
            {
                this.ShowErr("Calculate error.", ex);
                return;
            }

            this.CurrentMaintain["TMS"] = tms.Rows[0]["TtlTMS"];
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
            this.CurrentMaintain["Category"] = "M";
            this.CurrentMaintain["OutputDate"] = DateTime.Today.AddDays(-1).ToString("d");
            this.CurrentMaintain["Shift"] = "D";
            this.CurrentMaintain["Team"] = "A";
            this.CurrentMaintain["WorkHour"] = 0;
            this.CurrentMaintain["Status"] = "New";
            this.CurrentMaintain["MDivisionID"] = Sci.Env.User.Keyword;
        }

        /// <inheritdoc/>
        protected override bool ClickEditBefore()
        {
            if (MyUtility.Convert.GetString(this.CurrentMaintain["Status"]) == "Locked")
            {
                MyUtility.Msg.WarningBox("This resord is < Locked >, can't modify!!");
                return false;
            }

            return base.ClickEditBefore();
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();

            this.dateDate.ReadOnly = true;

            if (MyUtility.Convert.GetDate(this.CurrentMaintain["OutputDate"]) <= this.SewingMonthlyLockDate)
            {
                this.txtsewinglineLine.ReadOnly = true;
                this.numManpower.ReadOnly = true;
                this.numWHours.ReadOnly = true;
            }
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            // 檢查欄位值不可為空
            if (MyUtility.Check.Empty(this.CurrentMaintain["OutputDate"]))
            {
                this.dateDate.Focus();
                MyUtility.Msg.WarningBox("Date can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["SewingLineID"]))
            {
                this.txtsewinglineLine.Focus();
                MyUtility.Msg.WarningBox("Line# can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["Manpower"]))
            {
                this.numManpower.Focus();
                MyUtility.Msg.WarningBox("Manpower can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["WorkHour"]))
            {
                this.numWHours.Focus();
                MyUtility.Msg.WarningBox("W/Hours(Day) can't empty!!");
                return false;
            }

            this.CalculateManHour();

            DataTable sumQty;
            int recCnt = 0, gridQaQty, gridInlineQty, gridDefectQty;
            decimal gridWHours, gridTms = 0;
            try
            {
                MyUtility.Tool.ProcessWithDatatable((DataTable)this.detailgridbs.DataSource, "WorkHour,QAQty,InlineQty,DefectQty,OrderID", "select isnull(sum(WorkHour),0) as sumWorkHour,isnull(sum(QAQty),0) as sumQaqty,isnull(sum(InlineQty),0) as sumInlineQty,isnull(sum(DefectQty),0) as sumDefectQty from #tmp where (OrderID <> '' or OrderID is not null)", out sumQty, "#tmp");
            }
            catch (Exception ex)
            {
                this.ShowErr("Calculate error.", ex);
                return false;
            }

            if (sumQty == null)
            {
                gridQaQty = 0;
                gridInlineQty = 0;
                gridDefectQty = 0;
                gridWHours = 0;
            }
            else
            {
                gridQaQty = MyUtility.Convert.GetInt(sumQty.Rows[0]["sumQAQty"]);
                gridInlineQty = MyUtility.Convert.GetInt(sumQty.Rows[0]["sumInlineQty"]);
                gridDefectQty = MyUtility.Convert.GetInt(sumQty.Rows[0]["sumDefectQty"]);
                gridWHours = MyUtility.Convert.GetDecimal(sumQty.Rows[0]["sumWorkHour"]);
            }

            foreach (DataRow dr in this.DetailDatas)
            {
                if (MyUtility.Check.Empty(dr["OrderID"]))
                {
                    dr.Delete();
                    continue;
                }

                gridTms = gridTms + gridQaQty == 0 ? 0 : (MyUtility.Convert.GetDecimal(dr["TMS"]) * MyUtility.Convert.GetDecimal(dr["QAQty"]) / MyUtility.Convert.GetDecimal(gridQaQty));
                recCnt += 1;
            }

            if (recCnt == 0)
            {
                this.detailgrid.Focus();
                MyUtility.Msg.WarningBox("< Detail > can't be empty!");
                return false;
            }

            if (gridWHours != MyUtility.Convert.GetDecimal(this.CurrentMaintain["WorkHour"]))
            {
                MyUtility.Msg.WarningBox("The working hours summary is not equal to working hours/day, please correct, or else can't be saved.");
                return false;
            }

            #region 若sewingoutput.outputDate <= SewingMonthlyLock.LockDate 表身Qty要等於表頭的Qty
            DateTime? sod = MyUtility.Convert.GetDate(this.CurrentMaintain["outputDate"]);
            DateTime? sl = MyUtility.Convert.GetDate(MyUtility.GetValue.Lookup($"select LockDate from SewingMonthlyLock where FactoryID = '{this.CurrentMaintain["FactoryID"]}'"));
            if (sod <= sl)
            {
                decimal nQ = 0;
                foreach (DataRow dr in this.DetailDatas)
                {
                    if (!MyUtility.Check.Empty(dr["QAQty"]))
                    {
                        nQ += MyUtility.Convert.GetDecimal(dr["QAQty"]);
                    }
                }

                if (nQ != this.oldttlqaqty)
                {
                    MyUtility.Msg.WarningBox("QA Output shouled be the same as before.");
                    return false;
                }
            }
            #endregion

            #region 若status = Sent ，表身[QA Qty]總和與[Manhours]必為相同

            if (MyUtility.Convert.GetString(this.CurrentMaintain["Status"]).EqualString("Sent"))
            {
                decimal totalQAQty = 0;
                decimal manhour = MyUtility.Convert.GetDecimal(this.CurrentMaintain["ManHour"]);
                foreach (DataRow dr in this.DetailDatas)
                {
                    totalQAQty += MyUtility.Convert.GetDecimal(dr["QAQty"]);
                }

                if (totalQAQty != manhour)
                {
                    MyUtility.Msg.WarningBox("The reocord is already lock so [QA Output]、[Manhours] can not modify!");
                    return false;
                }
            }
            #endregion

            // GetID
            if (this.IsDetailInserting)
            {
                string id = MyUtility.GetValue.GetID(MyUtility.GetValue.Lookup("FtyGroup", Sci.Env.User.Factory, "Factory", "ID") + "MM", "SewingOutput", DateTime.Today, 3, "Id", null);
                if (MyUtility.Check.Empty(id))
                {
                    MyUtility.Msg.WarningBox("GetID fail, please try again!");
                    return false;
                }

                this.CurrentMaintain["ID"] = id;
            }

            this.CurrentMaintain["QAQty"] = gridQaQty;
            this.CurrentMaintain["InlineQty"] = gridInlineQty;
            this.CurrentMaintain["DefectQty"] = gridDefectQty;
            this.CurrentMaintain["TMS"] = MyUtility.Math.Round(gridTms, 0);
            this.CurrentMaintain["Efficiency"] = MyUtility.Convert.GetDecimal(this.CurrentMaintain["TMS"]) * MyUtility.Convert.GetDecimal(this.CurrentMaintain["ManHour"]) == 0 ? 0 : MyUtility.Convert.GetDecimal(gridQaQty) / (3600 / MyUtility.Convert.GetDecimal(this.CurrentMaintain["TMS"]) * MyUtility.Convert.GetDecimal(this.CurrentMaintain["ManHour"])) * 100;
            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override void ClickSaveAfter()
        {
            base.ClickSaveAfter();
            this.txtsewinglineLine.Enabled = true;
        }

        /// <inheritdoc/>
        protected override bool ClickDeleteBefore()
        {
            if (MyUtility.Convert.GetString(this.CurrentMaintain["Status"]) == "Locked")
            {
                MyUtility.Msg.WarningBox("This resord is < Locked >, can't delete!!");
                return false;
            }

            if (MyUtility.Convert.GetDate(this.CurrentMaintain["OutputDate"]) <= this.SewingMonthlyLockDate)
            {
                MyUtility.Msg.WarningBox($"The date less then {this.SewingMonthlyLockDate} , can't deleted.");
                return false;
            }

            return base.ClickDeleteBefore();
        }

        // Date
        private void DateDate_Validating(object sender, CancelEventArgs e)
        {
            if (this.EditMode && !MyUtility.Check.Empty(this.dateDate.Value))
            {
                if (this.dateDate.Value > DateTime.Today)
                {
                    this.dateDate.Value = null;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox("< Date > is greater than today, please pay attention!!");
                    return;
                }

                this.SewingMonthlyLockDate = MyUtility.Convert.GetDate(MyUtility.GetValue.Lookup($"select LockDate from SewingMonthlyLock where FactoryID = '{this.CurrentMaintain["FactoryID"]}'"));
                if (this.dateDate.Value <= this.SewingMonthlyLockDate)
                {
                    this.dateDate.Value = null;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox("< Date > can't early than System Lock Date:" + ((DateTime)this.SewingMonthlyLockDate).ToString("d"));
                    return;
                }
            }
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain))
            {
                return;
            }

            this.SewingMonthlyLockDate = MyUtility.Convert.GetDate(MyUtility.GetValue.Lookup($"select LockDate from SewingMonthlyLock where FactoryID = '{this.CurrentMaintain["FactoryID"]}'"));
            base.OnDetailEntered();
            this.oldttlqaqty = this.numQAOutput.Value;
            bool isSend = this.CurrentMaintain["Status"].ToString() == "Sent";

            switch (MyUtility.Convert.GetString(this.CurrentMaintain["Status"]))
            {
                case "Sent":
                    this.lbstatus.Text = "Daily Lock";
                    break;
                case "Locked":
                    this.lbstatus.Text = "Monthly Lock";
                    break;
                default:
                    this.lbstatus.Text = string.Empty;
                    break;
            }

            if (isSend)
            {
                this.btnReqUnlock.Visible = true;
                this.IsSupportRecall = true;
            }
            else
            {
                this.btnReqUnlock.Visible = false;
                this.IsSupportRecall = false;
            }
        }

        private void CalculateManHour()
        {
            decimal manpower = MyUtility.Convert.GetDecimal(this.CurrentMaintain["Manpower"]);
            decimal workHour = MyUtility.Convert.GetDecimal(this.CurrentMaintain["WorkHour"]);
            this.CurrentMaintain["ManHour"] = Convert.ToString(manpower * workHour);
        }

        // Manpower
        private void NumManpower_Validated(object sender, EventArgs e)
        {
            this.CalculateManHour();
        }

        // W/Hours(Day)
        private void NumWHours_Validated(object sender, EventArgs e)
        {
            this.CalculateManHour();
        }

        private void ReCalculateDefectAndRFT(DataRow dr)
        {
            dr["DefectQty"] = MyUtility.Convert.GetInt(dr["InlineQty"]) - MyUtility.Convert.GetInt(dr["QAQty"]);
            dr["RFT"] = MyUtility.Check.Empty(dr["InlineQty"]) ? 0 : MyUtility.Math.Round(MyUtility.Convert.GetInt(dr["QAQty"]) / MyUtility.Convert.GetDecimal(dr["InlineQty"]), 2) * 100;
        }

        // Share < working hours > to SP#
        private void BtnShareWworkingHoursToSP_Click(object sender, EventArgs e)
        {
            DataTable sumQaQty;
            try
            {
                MyUtility.Tool.ProcessWithDatatable(((DataTable)this.detailgridbs.DataSource).DefaultView.ToTable(), "QAQty,TMS,OrderID", "select isnull(sum(QAQty*TMS),0) as sumQaqty,isnull(count(QAQty),0) as RecCnt from #tmp", out sumQaQty, "#tmp");
            }
            catch (Exception ex)
            {
                this.ShowErr("Calculate error.", ex);
                return;
            }

            if (sumQaQty == null)
            {
                return;
            }

            int recCnt = MyUtility.Convert.GetInt(sumQaQty.Rows[0]["RecCnt"]);
            decimal ttlQaqty = MyUtility.Convert.GetDecimal(sumQaQty.Rows[0]["sumQaqty"]);

            decimal subSum = 0;
            foreach (DataRow dr in ((DataTable)this.detailgridbs.DataSource).Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    recCnt = recCnt - 1;
                    if (recCnt == 0)
                    {
                        dr["WorkHour"] = MyUtility.Convert.GetDecimal(this.CurrentMaintain["WorkHour"]) - subSum;
                    }
                    else
                    {
                        dr["WorkHour"] = ttlQaqty == 0 ? 0 : MyUtility.Math.Round(MyUtility.Convert.GetDecimal(dr["QAQty"]) * MyUtility.Convert.GetDecimal(dr["TMS"]) / ttlQaqty * MyUtility.Convert.GetDecimal(this.CurrentMaintain["WorkHour"]), 3);
                    }

                    subSum = subSum + MyUtility.Convert.GetDecimal(dr["WorkHour"]);
                }
            }
        }

        /// <inheritdoc/>
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            Sci.Win.UI.SelectReason callReason = new Sci.Win.UI.SelectReason("Sewing_RVS", true);
            DialogResult dResult = callReason.ShowDialog(this);
            if (dResult == System.Windows.Forms.DialogResult.OK)
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
                    Sci.Env.User.UserID);

                string updateCmd = $@"
update SewingOutput 
set LockDate = null, Status = 'New'
, EditDate = GetDate(), EditName = '{Sci.Env.User.UserID}'
where ID = '{MyUtility.Convert.GetString(this.CurrentMaintain["ID"])}'";

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
                        this.ShowErr("Commit transaction error.", ex);
                        return;
                    }
                }
            }
        }

        private void BtnReqUnlock_Click(object sender, EventArgs e)
        {
            Sci.Win.UI.SelectReason callReason = new Sci.Win.UI.SelectReason("Sewing_RVS");
            DialogResult dResult = callReason.ShowDialog(this);

            if (dResult == System.Windows.Forms.DialogResult.OK)
            {
                string toAddress = MyUtility.GetValue.Lookup($@"

SELECT CONCAT(p1.EMail,';')  
FROM Factory f  
INNER JOIN Pass1 p1 ON p1.id = f.Manager  
WHERE  f.ID = '{this.CurrentMaintain["FactoryID"]}' AND p1.EMail <>''

UNION ALL

SELECT TOP 2 CONCAT(p1.EMail,';')  
FROM  Pass1 p1 
INNER JOIN Pass0 p0 ON p0.PKey=p1.FKPass0  
INNER JOIN Pass2 p2 ON p2.PKey=p0.PKey 
WHERE p1.Factory LIKE ('%{this.CurrentMaintain["FactoryID"]}%') AND p1.ID <> 'SCIMIS' AND p1.EMail <>''
AND p0.PKey IN ( 
				SELECT FKPass0 
				FROM Pass2 WHERE MenuName = 'Sewing' AND BarPrompt='P02. Mockup daily output' 
				AND CanRecall = 1)  
FOR XML PATH('')

SELECT Factory,*
FROM Pass1
WHERE ID = 'SCIMIS'

");

                #region 填寫Mail需要的資料
                string ccAddress = string.Empty;
                string subject = "Unlock Sewing(Mockup)";
                string od = string.Empty;

                string outputDate = string.Empty;
                if (!MyUtility.Check.Empty(this.CurrentMaintain["OutputDate"]))
                {
                    outputDate = ((DateTime)this.CurrentMaintain["OutputDate"]).ToString("yyyy/MM/dd");
                }

                string description = $@"Date : {outputDate}
Factory : {this.CurrentMaintain["FactoryID"]}
Line# : {this.CurrentMaintain["SewingLineID"]}
Team : {this.CurrentMaintain["Team"]}
Shift : {this.CurrentMaintain["Shift"]}
QA Output: {this.CurrentMaintain["QAQty"]}
Manhours : {this.CurrentMaintain["Manhour"]}
-----------------------------------------------
Reason : {MyUtility.GetValue.Lookup($@"SELECT Name FROM Reason WHERE ReasonTypeID='Sewing_RVS' AND id= '{callReason.ReturnReason}'")}
Remark : {callReason.ReturnRemark}
";
                #endregion

                // 塞進MailTo物件
                var email = new MailTo(Sci.Env.Cfg.MailFrom, toAddress, ccAddress, subject, null, description, false, true);

                // email畫面關閉後額外塞入CC人員
                email.SendingBefore += this.Email_SendingBefore;
                email.ShowDialog(this);

                if (email.DialogResult == DialogResult.OK)
                {
                    // 寄信成功後寫入SewingOutput_DailyUnlock
                    string sqlcmd = $@"
INSERT INTO SewingOutput_DailyUnlock (SewingOutputID ,ReasonID ,Remark ,RequestDate ,RequestName)
values('{this.CurrentMaintain["ID"]}' ,'{callReason.ReturnReason}' ,'{callReason.ReturnRemark}' ,getdate() ,'{Sci.Env.User.UserID}')";

                    DualResult rs = DBProxy.Current.Execute("Production", sqlcmd);
                    if (!rs)
                    {
                        this.ShowErr(rs);
                    }
                }
            }
        }

        /// <summary>
        /// email畫面關閉後額外塞入CC人員
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void Email_SendingBefore(object sender, MailTo.SendMailBeforeArg e)
        {
            e.Mail.CC.Add("planning@sportscity.com.tw");
            e.Mail.CC.Add("team3@sportscity.com.tw");
        }

        private void BtnBatchRecall_Click(object sender, EventArgs e)
        {
            if (!this.Perm.Recall)
            {
                MyUtility.Msg.WarningBox("You have no permission.");
                return;
            }

            P02_BatchRecall form = new P02_BatchRecall();
            form.ShowDialog(this);
        }

        protected override void ClickSend()
        {
            base.ClickSend();
            string sqlcmdChk = $@"
select 1
FROM SewingOutput s
INNER JOIN SewingOutput_Detail sd ON sd.ID = s.ID
INNER JOIN MockupOrder mo ON mo.ID = sd.OrderId
where 1=1
    and s.OutputDate < = CAST (GETDATE() AS DATE) 
    and s.LockDate is null 
    and s.FactoryID  = '{Sci.Env.User.Factory}'
";
            if (!MyUtility.Check.Seek(sqlcmdChk))
            {
                MyUtility.Msg.WarningBox("Already lock now!");
                return;
            }

            if (MyUtility.Msg.QuestionBox("Lock sewing data?") == DialogResult.No)
            {
                return;
            }

            string sqlcmd = $@"
update  s 
set s.LockDate = CONVERT(date, GETDATE()) , s.Status='Sent'
, s.editname='{Sci.Env.User.UserID}' 
, s.editdate=getdate()
FROM SewingOutput s
INNER JOIN SewingOutput_Detail sd ON sd.ID = s.ID
INNER JOIN MockupOrder mo ON mo.ID = sd.OrderId
where 1=1
    and s.OutputDate < = CAST (GETDATE() AS DATE) 
    and s.LockDate is null 
    and s.FactoryID  = '{Sci.Env.User.Factory}'
";

            using (TransactionScope scope = new TransactionScope())
            {
                DualResult upResult;
                if (!MyUtility.Check.Empty(sqlcmd))
                {
                    if (!(upResult = DBProxy.Current.Execute(null, sqlcmd)))
                    {
                        this.ShowErr(upResult);
                        return;
                    }
                }

                scope.Complete();
            }

            if (MyUtility.Check.Seek($@"select 1 from Factory where type !='S' and id = '{Sci.Env.User.Factory}'"))
            {
                Sci.Production.Sewing.P01.SendMail();
            }
        }

        protected override void ClickRecall()
        {
            base.ClickRecall();
            bool hasReq = false;

            if (!this.Perm.Recall)
            {
                MyUtility.Msg.WarningBox("You have no permission.");
                return;
            }

            if (MyUtility.Msg.QuestionBox("Unlock sewing data?") == DialogResult.No)
            {
                return;
            }

            hasReq = MyUtility.Check.Seek($"SELECT 1 FROM SewingOutput_DailyUnlock WHERE SewingOutputID = '{this.CurrentMaintain["ID"]}'");

            if (!hasReq)
            {
                MyUtility.Msg.WarningBox("No Unlock Request!");
                return;
            }

            #region SQL語法

            string sqlCmd = $@"
DECLARE @reasonID nvarchar(5)
DECLARE @remark nvarchar(max)
DECLARE @ukey bigint

SELECT TOP 1 @ukey=ukey ,@reasonID=reasonID ,@remark=remark 
FROM SewingOutput_DailyUnlock 
WHERE SewingOutputID = '{this.CurrentMaintain["ID"]}' 
ORDER by Ukey DESC

--Recall 一律是從Sent改回New
INSERT INTO SewingOutput_History (ID ,HisType ,OldValue ,NewValue ,ReasonID ,Remark ,AddName ,AddDate)
VALUES ('{this.CurrentMaintain["ID"]}','Status' ,'Sent' ,'New' ,isnull(@reasonID,''),isnull(@remark,''),'{Sci.Env.User.UserID}' ,GETDATE())

Update SewingOutput_DailyUnlock SET 
UnLockDate = GETDATE() ,UnLockName= '{Sci.Env.User.UserID}'
where ukey=@ukey

UPDATE SewingOutput SET Status='New', LockDate = NULL 
, editname='{Sci.Env.User.UserID}' 
, editdate=getdate()
WHERE ID = '{this.CurrentMaintain["ID"]}' 
";
            #endregion

            using (TransactionScope scope = new TransactionScope())
            {
                DualResult upResult;
                if (!MyUtility.Check.Empty(sqlCmd))
                {
                    if (!(upResult = DBProxy.Current.Execute(null, sqlCmd)))
                    {
                        this.ShowErr(upResult);
                    }
                }

                scope.Complete();
            }

            MyUtility.Msg.InfoBox("Unlock data successfully!");
        }
    }
}
