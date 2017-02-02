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
    public partial class P02 : Sci.Win.Tems.Input6
    {
        Ict.Win.DataGridViewGeneratorTextColumnSettings orderID = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
        Ict.Win.DataGridViewGeneratorNumericColumnSettings qaqty = new Ict.Win.DataGridViewGeneratorNumericColumnSettings();
        Ict.Win.DataGridViewGeneratorNumericColumnSettings inlineqty = new Ict.Win.DataGridViewGeneratorNumericColumnSettings();

        private DateTime systemLockDate;
        private decimal systemTMS = 0;

        public P02(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.DefaultFilter = "FactoryID = '" + Sci.Env.User.Factory + "' and Category = 'M'";

            DataRow sysData;
            if (MyUtility.Check.Seek("select SewLock,StdTMS from System", out sysData))
            {
                systemLockDate = (DateTime)MyUtility.Convert.GetDate(sysData["SewLock"]);
                systemTMS = MyUtility.Convert.GetDecimal(sysData["StdTMS"]);
            }
            else
            {
                MyUtility.Msg.ErrorBox("Query System fail, pls contact Taipei MIS!!");
            }
        }

        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["ID"].ToString();
            this.DetailSelectCommand = string.Format(@"select sd.*,mo.MockupID,mo.Qty,(select isnull(sum(QAQty),0) from SewingOutput_Detail where OrderId = sd.OrderId and ID != sd.ID) as AccuQty,
mo.Qty-(select isnull(sum(QAQty),0) from SewingOutput_Detail where OrderId = sd.OrderId and ID != sd.ID) as VarQty,
mo.Qty-(select isnull(sum(QAQty),0) from SewingOutput_Detail where OrderId = sd.OrderId and ID != sd.ID)-sd.QAQty as BalQty,
round(iif(sd.InlineQty = 0,0,sd.QAQty/sd.InlineQty),2)*100 as RFT
from SewingOutput_Detail sd
left join MockupOrder mo on mo.ID = sd.OrderId
where sd.ID = '{0}'", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();
            #region Right click & Validating
            orderID.EditingMouseDown += (s, e) =>
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    if (this.EditMode)
                    {
                        if (e.RowIndex != -1)
                        {
                            DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                            string sqlCmd = string.Format("select ID,MockupID,StyleID,SeasonID,BrandID from MockupOrder where Junk = 0 and FTYGroup = '{0}'", Sci.Env.User.Factory);
                            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlCmd, "15,13,15,5,10", dr["OrderID"].ToString(), "ID,MockupID,Style,Season,Brand");
                            item.Size = new System.Drawing.Size(700, 600);
                            DialogResult returnResult = item.ShowDialog();
                            if (returnResult == DialogResult.Cancel) { return; }
                            e.EditingControl.Text = item.GetSelectedString();
                        }
                    }
                }
            };

            orderID.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    if (!MyUtility.Check.Empty(e.FormattedValue) && MyUtility.Convert.GetString(e.FormattedValue) != MyUtility.Convert.GetString(dr["OrderID"]))
                    {
                        //sql參數
                        System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter("@factoryid",Sci.Env.User.Factory);
                        System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter("@id", MyUtility.Convert.GetString(e.FormattedValue));

                        IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                        cmds.Add(sp1);
                        cmds.Add(sp2);

                        DataTable moData;
                        string sqlCmd = "select * from MockupOrder where Junk = 0 and FTYGroup = @factoryid and ID = @id";
                        DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out moData);
                        if (!result || moData.Rows.Count <= 0)
                        {
                            if (!result)
                            {
                                MyUtility.Msg.WarningBox("Sql connection fail!\r\n"+result.ToString());
                            }
                            else
                            {
                                MyUtility.Msg.WarningBox(string.Format("Data not found!!!"));
                            }
                            
                            dr["OrderID"] = "";
                            dr["TMS"] = 0;
                            dr["MockupID"] = "";
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
                            dr["TMS"] = MyUtility.Math.Round(MyUtility.Convert.GetDecimal(moData.Rows[0]["CPU"]) * MyUtility.Convert.GetDecimal(moData.Rows[0]["CPUFactor"]) * systemTMS);
                            dr["MockupID"] = MyUtility.Convert.GetString(moData.Rows[0]["MockupID"]);
                            dr["Qty"] = MyUtility.Convert.GetInt(moData.Rows[0]["Qty"]);
                            dr["AccuQty"] = MyUtility.Convert.GetInt(MyUtility.GetValue.Lookup(string.Format("select isnull(sum(QAQty),0) from SewingOutput_Detail where OrderId = '{0}'", MyUtility.Convert.GetString(dr["OrderID"]))));
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
            };

            qaqty.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    if (!MyUtility.Check.Empty(e.FormattedValue) && MyUtility.Convert.GetInt(e.FormattedValue) != MyUtility.Convert.GetInt(dr["QAQty"]))
                    {
                        if (MyUtility.Convert.GetInt(e.FormattedValue) > MyUtility.Convert.GetInt(dr["VarQty"]))
                        {
                            MyUtility.Msg.WarningBox("Output Qty can't exceed Variance!");
                            dr["QAQty"] = dr["QAQty"];
                            return;
                        }
                        else
                        {
                            dr["QAQty"] = MyUtility.Convert.GetInt(e.FormattedValue);
                            dr["BalQty"] = MyUtility.Convert.GetInt(dr["Qty"]) - MyUtility.Convert.GetInt(dr["AccuQty"]) - MyUtility.Convert.GetInt(dr["QAQty"]);
                            ReCalculateDefectAndRFT(dr);
                        }
                    }
                }
            };

            inlineqty.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    if (!MyUtility.Check.Empty(e.FormattedValue) && MyUtility.Convert.GetInt(e.FormattedValue) != MyUtility.Convert.GetInt(dr["InlineQty"]))
                    {
                        dr["InlineQty"] = MyUtility.Convert.GetInt(e.FormattedValue);
                        ReCalculateDefectAndRFT(dr);
                    }
                }
            };

            #endregion

            Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("OrderID", header: "Mockup order#", width: Widths.AnsiChars(13), settings: orderID)
                .Text("MockupID", header: "Mockup type", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Numeric("TMS", header: "TMS", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Numeric("Qty", header: "Order Qty", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Numeric("AccuQty", header: "Accu. Qty", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Numeric("VarQty", header: "Variance", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Numeric("QAQty", header: "QA Output", width: Widths.AnsiChars(5), settings: qaqty)
                .Numeric("BalQty", header: "Bal. Qty", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Numeric("InlineQty", header: "Prod. Output", width: Widths.AnsiChars(5), settings: inlineqty)
                .Numeric("DefectQty", header: "Defect Qty", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Numeric("WorkHour", header: "W'Hours", decimal_places: 3, width: Widths.AnsiChars(5))
                .Numeric("RFT", header: "RFT(%)", width: Widths.AnsiChars(5), iseditingreadonly: true);
        }

        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
            CurrentMaintain["Category"] = "M";
            CurrentMaintain["OutputDate"] = DateTime.Today.AddDays(-1).ToString("d");
            CurrentMaintain["Shift"] = "D";
            CurrentMaintain["Team"] = "A";
            CurrentMaintain["WorkHour"] = 0;
            CurrentMaintain["Status"] = "New";
            CurrentMaintain["MDivisionID"] = Sci.Env.User.Keyword;
        }

        protected override bool ClickEditBefore()
        {
            if (MyUtility.Convert.GetString(CurrentMaintain["Status"]) == "Locked")
            {
                MyUtility.Msg.WarningBox("This resord is < Locked >, can't modify!!");
                return false;
            }
            return base.ClickEditBefore();
        }

        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();

            dateBox1.ReadOnly = true;

            if (MyUtility.Convert.GetDate(CurrentMaintain["OutputDate"]) <= systemLockDate)
            {
                txtsewingline1.ReadOnly = true;
                numericBox1.ReadOnly = true;
                numericBox2.ReadOnly = true;
            }
        }

        protected override bool ClickSaveBefore()
        {
            //檢查欄位值不可為空
            if (MyUtility.Check.Empty(CurrentMaintain["OutputDate"]))
            {
                MyUtility.Msg.WarningBox("Date can't empty!!");
                dateBox1.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["SewingLineID"]))
            {
                MyUtility.Msg.WarningBox("Line can't empty!!");
                txtsewingline1.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["Manpower"]))
            {
                MyUtility.Msg.WarningBox("Manpower can't empty!!");
                numericBox1.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["WorkHour"]))
            {
                MyUtility.Msg.WarningBox("W/Hours(Day) can't empty!!");
                numericBox2.Focus();
                return false;
            }

            DataTable SumQty;
            int recCnt = 0, gridQaQty, gridInlineQty , gridDefectQty;
            decimal gridWHours, gridTms = 0;
            try
            {
                MyUtility.Tool.ProcessWithDatatable(((DataTable)detailgridbs.DataSource), "WorkHour,QAQty,InlineQty,DefectQty,OrderID", "select isnull(sum(WorkHour),0) as sumWorkHour,isnull(sum(QAQty),0) as sumQaqty,isnull(sum(InlineQty),0) as sumInlineQty,isnull(sum(DefectQty),0) as sumDefectQty from #tmp where (OrderID <> '' or OrderID is not null)", out SumQty, "#tmp");
            }
            catch (Exception ex)
            {
                ShowErr("Calculate error.", ex);
                return false;
            }

            if (SumQty == null)
            {
                gridQaQty = 0;
                gridInlineQty = 0;
                gridDefectQty = 0;
                gridWHours = 0;
            }
            else
            {
                gridQaQty = MyUtility.Convert.GetInt(SumQty.Rows[0]["sumQAQty"]);
                gridInlineQty = MyUtility.Convert.GetInt(SumQty.Rows[0]["sumInlineQty"]);
                gridDefectQty = MyUtility.Convert.GetInt(SumQty.Rows[0]["sumDefectQty"]);
                gridWHours = MyUtility.Convert.GetDecimal(SumQty.Rows[0]["sumWorkHour"]);
            }


            foreach (DataRow dr in DetailDatas)
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
                MyUtility.Msg.WarningBox("< Detail > can't be empty!");
                detailgrid.Focus();
                return false;
            }

            if (gridWHours != MyUtility.Convert.GetDecimal(CurrentMaintain["WorkHour"]))
            {
                MyUtility.Msg.WarningBox("The working hours summary is not equal to working hours/day, please correct, or else can't be saved.");
                return false;
            }

            //GetID
            if (IsDetailInserting)
            {
                string id = MyUtility.GetValue.GetID(MyUtility.GetValue.Lookup("FtyGroup", Sci.Env.User.Factory,"Factory","ID") + "MM", "SewingOutput", DateTime.Today, 3, "Id", null);
                if (MyUtility.Check.Empty(id))
                {
                    MyUtility.Msg.WarningBox("GetID fail, please try again!");
                    return false;
                }
                CurrentMaintain["ID"] = id;
            }

            CurrentMaintain["QAQty"] = gridQaQty;
            CurrentMaintain["InlineQty"] = gridInlineQty;
            CurrentMaintain["DefectQty"] = gridDefectQty;
            CurrentMaintain["TMS"] = MyUtility.Math.Round(gridTms, 0);
            CurrentMaintain["Efficiency"] = MyUtility.Convert.GetDecimal(CurrentMaintain["TMS"]) * MyUtility.Convert.GetDecimal(CurrentMaintain["ManHour"]) == 0 ? 0 : MyUtility.Convert.GetDecimal(gridQaQty) / (3600 / MyUtility.Convert.GetDecimal(CurrentMaintain["TMS"]) * MyUtility.Convert.GetDecimal(CurrentMaintain["ManHour"])) * 100;
            return base.ClickSaveBefore();
        }

        protected override void ClickSaveAfter()
        {
            base.ClickSaveAfter();
            txtsewingline1.Enabled = true;
        }

        protected override bool ClickDeleteBefore()
        {
            if (MyUtility.Convert.GetString(CurrentMaintain["Status"]) == "Locked")
            {
                MyUtility.Msg.WarningBox("This resord is < Locked >, can't delete!!");
                return false;
            }

            if (MyUtility.Convert.GetDate(CurrentMaintain["OutputDate"]) <= systemLockDate)
            {
                MyUtility.Msg.WarningBox("The date less then System.sewLock date, can't deleted.");
                return false;
            }

            return base.ClickDeleteBefore();
        }

        //Date
        private void dateBox1_Validating(object sender, CancelEventArgs e)
        {
            if (this.EditMode && !MyUtility.Check.Empty(dateBox1.Value))
            {
                if (dateBox1.Value > DateTime.Today)
                {
                    MyUtility.Msg.WarningBox("< Date > is greater than today, please pay attention!!");
                    dateBox1.Value = null;
                    e.Cancel = true;
                    return;
                }
                if (dateBox1.Value < systemLockDate)
                {
                    MyUtility.Msg.WarningBox("< Date > can't early than System Lock Date:" + systemLockDate.ToString("d"));
                    dateBox1.Value = null;
                    e.Cancel = true;
                    return;
                }
            }
        }

        private void CalculateManHour()
        {
            decimal manpower = MyUtility.Convert.GetDecimal(CurrentMaintain["Manpower"]);
            decimal workHour = MyUtility.Convert.GetDecimal(CurrentMaintain["WorkHour"]);
            CurrentMaintain["ManHour"] = Convert.ToString(manpower * workHour);
        }

        //Manpower
        private void numericBox1_Validated(object sender, EventArgs e)
        {
            CalculateManHour();
        }

        //W/Hours(Day)
        private void numericBox2_Validated(object sender, EventArgs e)
        {
            CalculateManHour();
        }

        private void ReCalculateDefectAndRFT(DataRow dr)
        {
            dr["DefectQty"] = MyUtility.Convert.GetInt(dr["InlineQty"]) - MyUtility.Convert.GetInt(dr["QAQty"]);
            dr["RFT"] = MyUtility.Check.Empty(dr["InlineQty"]) ? 0 : MyUtility.Math.Round(MyUtility.Convert.GetInt(dr["QAQty"]) / MyUtility.Convert.GetDecimal(dr["InlineQty"]), 2) * 100;
        }

        //Share < working hours > to SP#
        private void button1_Click(object sender, EventArgs e)
        {
            DataTable SumQaQty;
            try
            {
                MyUtility.Tool.ProcessWithDatatable(((DataTable)detailgridbs.DataSource), "QAQty,TMS,OrderID", "select isnull(sum(QAQty*TMS),0) as sumQaqty,isnull(count(QAQty),0) as RecCnt from #tmp", out SumQaQty, "#tmp");
            }
            catch (Exception ex)
            {
                ShowErr("Calculate error.", ex);
                return;
            }

            if (SumQaQty == null)
            {
                return;
            }

            int recCnt = MyUtility.Convert.GetInt(SumQaQty.Rows[0]["RecCnt"]);
            decimal ttlQaqty = MyUtility.Convert.GetDecimal(SumQaQty.Rows[0]["sumQaqty"]);

            decimal subSum = 0;
            foreach (DataRow dr in ((DataTable)detailgridbs.DataSource).Rows)
            {
                recCnt = recCnt - 1;
                if (dr.RowState != DataRowState.Deleted)
                {
                    if (recCnt == 0)
                    {
                        dr["WorkHour"] = MyUtility.Convert.GetDecimal(CurrentMaintain["WorkHour"]) - subSum;
                    }
                    else
                    {
                        dr["WorkHour"] = ttlQaqty == 0 ? 0 : MyUtility.Math.Round(MyUtility.Convert.GetDecimal(dr["QAQty"]) * MyUtility.Convert.GetDecimal(dr["TMS"]) / ttlQaqty * MyUtility.Convert.GetDecimal(CurrentMaintain["WorkHour"]), 3);
                    }
                    subSum = subSum + MyUtility.Convert.GetDecimal(dr["WorkHour"]);
                }
            }
        }

        //UnConfirm
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            Sci.Win.UI.SelectReason callReason = new Sci.Win.UI.SelectReason("Sewing_RVS", true);
            DialogResult dResult = callReason.ShowDialog(this);
            if (dResult == System.Windows.Forms.DialogResult.OK)
            {
                string insertCmd = string.Format(@"insert into SewingOutput_History (ID,HisType,OldValue,NewValue,ReasonID,Remark,AddName,AddDate)
values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}',GETDATE())", MyUtility.Convert.GetString(CurrentMaintain["ID"]), "Status", "Locked", "New", callReason.ReturnReason, callReason.ReturnRemark, Sci.Env.User.UserID);
                string updateCmd = string.Format(@"update SewingOutput set LockDate = null, Status = 'New' where ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ID"]));

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

            RenewData();
            OnDetailEntered();
            EnsureToolbarExt();
        }        
    }
}
