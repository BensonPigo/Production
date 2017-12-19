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

        private DateTime systemLockDate;
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
            if (MyUtility.Check.Seek("select SewLock,StdTMS from System WITH (NOLOCK) ", out sysData))
            {
                this.systemLockDate = (DateTime)MyUtility.Convert.GetDate(sysData["SewLock"]);
                this.systemTMS = MyUtility.Convert.GetDecimal(sysData["StdTMS"]);
            }
            else
            {
                MyUtility.Msg.ErrorBox("Query System fail, pls contact Taipei MIS!!");
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

      protected override void OnDetailEntered()
      {
         base.OnDetailEntered();
         this.oldttlqaqty = this.numQAOutput.Value;
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
inner join Factory f on mo.FtyGroup = f.ID
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
inner join Factory f on mo.FtyGroup = f.ID
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

            if (MyUtility.Convert.GetDate(this.CurrentMaintain["OutputDate"]) <= this.systemLockDate)
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
                MyUtility.Msg.WarningBox("Line can't empty!!");
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

         #region 若sewingoutput.outputDate <= system.sewlock 表身Qty要等於表頭的Qty
         DataTable sys;
         DBProxy.Current.Select(null, "select sewlock from system WITH (NOLOCK) ", out sys);
         DateTime? sod = MyUtility.Convert.GetDate(this.CurrentMaintain["outputDate"]);
         DateTime? sl = MyUtility.Convert.GetDate(sys.Rows[0][0]);
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

            if (MyUtility.Convert.GetDate(this.CurrentMaintain["OutputDate"]) <= this.systemLockDate)
            {
                MyUtility.Msg.WarningBox("The date less then System.sewLock date, can't deleted.");
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

                if (this.dateDate.Value <= this.systemLockDate)
                {
                    this.dateDate.Value = null;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox("< Date > can't early than System Lock Date:" + this.systemLockDate.ToString("d"));
                    return;
                }
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

                string updateCmd = string.Format(@"update SewingOutput set LockDate = null, Status = 'New' where ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));

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
    }
}
