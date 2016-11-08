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


namespace Sci.Production.IE
{
    public partial class P03 : Sci.Win.Tems.Input6
    {
        private object totalGSD, totalCycleTime;
        private DataTable EmployeeData;
        public P03(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            MyUtility.Tool.SetupCombox(comboBox1, 1, 1, "A,B");
            detailgrid.AllowUserToOrderColumns = true;
            InsertDetailGridOnDoubleClick = false;
            gridicon.Append.Visible = false;
        }

        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["ID"].ToString();
            this.DetailSelectCommand = string.Format(@"select ld.*,o.DescEN as Description,e.Name as EmployeeName,e.Skill as EmployeeSkill,iif(ld.Cycle = 0,0,ROUND(ld.GSD/ld.Cycle,2)*100) as Efficiency
from LineMapping_Detail ld
left join Employee e on ld.EmployeeID = e.ID
left join Operation o on ld.OperationID = o.ID
where ld.ID = '{0}' order by ld.No,ld.GroupKey", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            string sqlCmd = string.Format("select Description,CPU from Style where Ukey = {0}",CurrentMaintain["StyleUkey"].ToString());
            DataRow StyleData;
            if (MyUtility.Check.Seek(sqlCmd, out StyleData))
            {
                displayBox3.Value = StyleData["Description"];
                numericBox8.Value = Convert.ToDecimal(StyleData["CPU"]);
            }
            else
            {
                displayBox3.Value = "";
                numericBox8.Value = 0;
            }
            CalculateValue(0);
            SaveCalculateValue();
            button1.Enabled = !MyUtility.Check.Empty(CurrentMaintain["IEReasonID"]);
        }

        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();
            Ict.Win.DataGridViewGeneratorTextColumnSettings no = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
            Ict.Win.DataGridViewGeneratorNumericColumnSettings cycle = new DataGridViewGeneratorNumericColumnSettings();
            Ict.Win.DataGridViewGeneratorTextColumnSettings machine = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
            Ict.Win.DataGridViewGeneratorTextColumnSettings operatorid = new Ict.Win.DataGridViewGeneratorTextColumnSettings();

            #region No.的Valid
            no.CellValidating += (s, e) =>
                {
                    if (EditMode)
                    {
                        DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                        if (MyUtility.Check.Empty(e.FormattedValue) || (e.FormattedValue.ToString() != dr["No"].ToString()))
                        {
                            string oldValue = MyUtility.Check.Empty(dr["No"]) ? "" : dr["No"].ToString();
                            dr["No"] = MyUtility.Check.Empty(e.FormattedValue) ? "" : e.FormattedValue.ToString().Trim().PadLeft(4, '0');
                            dr.EndEdit();

                            ReclculateGridGSDCycleTime(oldValue);
                            ReclculateGridGSDCycleTime(dr["No"].ToString());

                        }
                    }
                };
            #endregion
            #region Cycle的Valid
            cycle.CellValidating += (s, e) =>
            {
                if (EditMode)
                {
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    if (MyUtility.Check.Empty(e.FormattedValue))
                    {
                        dr["Cycle"] = 0;
                    }
                    else
                    {
                        if (e.FormattedValue.ToString() != dr["Cycle"].ToString())
                        {
                            if (MyUtility.Convert.GetDecimal(e.FormattedValue) < 0)
                            {
                                MyUtility.Msg.WarningBox("Cycle time can't less than 0!!");
                                dr["Cycle"] = 0;
                            }
                            else
                            {
                                if (MyUtility.Convert.GetDecimal(e.FormattedValue) > MyUtility.Convert.GetDecimal(dr["GSD"]))
                                {
                                    MyUtility.Msg.WarningBox("Cycle time can't greater than GSD Time!!");
                                    dr["Cycle"] = dr["GSD"];
                                }
                                else
                                {
                                    dr["Cycle"] = MyUtility.Convert.GetDecimal(e.FormattedValue);
                                }
                            }
                        }
                    }
                    dr.EndEdit();
                    ReclculateGridGSDCycleTime(MyUtility.Check.Empty(dr["No"]) ? "" : dr["No"].ToString());
                }
            };
            #endregion
            #region Machine Type的按右鍵與Validating
            machine.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode)
                {
                    if (e.Button == System.Windows.Forms.MouseButtons.Right)
                    {
                        if (e.RowIndex != -1)
                        {
                            DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                            string sqlCmd = "select ID,Description from MachineType where Junk = 0";
                            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlCmd, "8,40", dr["MachineTypeID"].ToString());
                            DialogResult returnResult = item.ShowDialog();
                            if (returnResult == DialogResult.Cancel) { return; }
                            e.EditingControl.Text = item.GetSelectedString();
                        }
                    }
                }
            };

            machine.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    if (!MyUtility.Check.Empty(e.FormattedValue) && e.FormattedValue.ToString() != dr["MachineTypeID"].ToString())
                    {
                        //sql參數
                        System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
                        sp1.ParameterName = "@id";
                        sp1.Value = e.FormattedValue.ToString();

                        IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                        cmds.Add(sp1);
                        string sqlCmd = "select ID from MachineType where Junk = 0 and ID = @id";
                        DataTable MachineData;
                        DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out MachineData);
                        if (!result)
                        {
                            MyUtility.Msg.WarningBox("Sql connection fail!!\r\n"+result.ToString());
                            dr["MachineTypeID"] = "";
                            e.Cancel = true;
                            return;
                        }
                        else
                        {
                            if (MachineData.Rows.Count <= 0)
                            {
                                MyUtility.Msg.WarningBox(string.Format("< Machine Type: {0} > not found!!!", e.FormattedValue.ToString()));
                                dr["MachineTypeID"] = "";
                                e.Cancel = true;
                                return;
                            }
                        }
                    }
                }
            };
            #endregion
            #region Operator ID No.的按右鍵與Validating
            operatorid.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode)
                {
                    if (e.Button == System.Windows.Forms.MouseButtons.Right)
                    {
                        if (e.RowIndex != -1)
                        {
                            DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                            
                            GetEmployee(null);

                            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(EmployeeData, "ID,Name,Skill,SewingLineID,FactoryID", "10,30,20,2,8", dr["EmployeeID"].ToString());
                            DialogResult returnResult = item.ShowDialog();
                            if (returnResult == DialogResult.Cancel) { return; }
                            IList<DataRow> selectedData = item.GetSelecteds();
                            dr["EmployeeID"] = selectedData[0]["ID"];
                            dr["EmployeeName"] = selectedData[0]["Name"];
                            dr["EmployeeSkill"] = selectedData[0]["Skill"];
                            dr.EndEdit();
                        }
                    }
                }
            };
            operatorid.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    if (MyUtility.Check.Empty(e.FormattedValue))
                    {
                        ReviseEmployeeToEmpty(dr);
                        return;
                    }

                    if (e.FormattedValue.ToString() != dr["EmployeeID"].ToString())
                    {
                        GetEmployee(e.FormattedValue.ToString());
                        if (EmployeeData.Rows.Count <= 0)
                        {
                            MyUtility.Msg.WarningBox(string.Format("< Employee ID: {0} > not found!!!", e.FormattedValue.ToString()));
                            ReviseEmployeeToEmpty(dr);
                            e.Cancel = true;
                            return;
                        }
                        else
                        {
                            dr["EmployeeID"] = EmployeeData.Rows[0]["ID"];
                            dr["EmployeeName"] = EmployeeData.Rows[0]["Name"];
                            dr["EmployeeSkill"] = EmployeeData.Rows[0]["Skill"];
                            dr.EndEdit();
                        }
                    }
                }
            };
            #endregion

            Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("No", header: "No.", width: Widths.AnsiChars(4), settings: no)
                .EditText("Description", header: "Operation", width: Widths.AnsiChars(30), iseditingreadonly: true)
                .EditText("Annotation", header: "Annotation", width: Widths.AnsiChars(30), iseditingreadonly: true)
                .Numeric("GSD", header: "GSD Time", width: Widths.AnsiChars(5), decimal_places: 2, iseditingreadonly: true)
                .Numeric("TotalGSD", header: "Ttl GSD Time", width: Widths.AnsiChars(6), decimal_places: 2, iseditingreadonly: true)
                .Numeric("Cycle", header: "Cycle Time", width: Widths.AnsiChars(5), integer_places: 4, decimal_places: 2, minimum: 0, settings: cycle)
                .Numeric("TotalCycle", header: "Ttl Cycle Time", width: Widths.AnsiChars(6), decimal_places: 2, iseditingreadonly: true)
                .Text("MachineTypeID", header: "Machine Type", width: Widths.AnsiChars(10), settings: machine)
                .Text("EmployeeID", header: "Operator ID No.", width: Widths.AnsiChars(10), settings: operatorid)
                .Text("EmployeeName", header: "Operator Name", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("EmployeeSkill", header: "Skill", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("Efficiency", header: "Eff(%)", width: Widths.AnsiChars(6), decimal_places: 2, iseditingreadonly: true);

            //因為資料顯示已有排序，所以按Grid Header不可以做排序
            for (int i = 0; i < detailgrid.ColumnCount; i++)
            {
                detailgrid.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            //detailgrid.RowsAdded += (s, e) =>
            //{
                
            //    DataTable dtData = (DataTable)detailgridbs.DataSource;
            //    for (int i = 0; i < e.RowCount; i++)
            //    {
            //        DataRow dr = detailgrid.GetDataRow(i + e.RowIndex);
            //        if (dr["New"].ToString().ToUpper() == "TRUE")
            //        {
            //            detailgrid.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(255, 186, 117);
            //        }
                    
            //    }
            //};

            //detailgrid.RowValidated += (s, e) =>
            //{
            //    DataTable dtData = (DataTable)detailgridbs.DataSource;
            //    if (dtData.Rows[e.RowIndex].RowState != DataRowState.Deleted && dtData.Rows[e.RowIndex]["New"].ToString().ToUpper() == "TRUE")
            //    {
            //        detailgrid.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(255, 186, 117);
            //    }
            //};

            detailgrid.RowPrePaint += detailgrid_RowPrePaint;
        }

        void detailgrid_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataRow dr = ((DataRowView)detailgrid.Rows[e.RowIndex].DataBoundItem).Row;

            if (dr["New"].ToString().ToUpper() == "TRUE")
            {
                detailgrid.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(255, 186, 117);
            }
        }

        //撈出Employee資料
        private void GetEmployee(string ID)
        {
            string sqlCmd;
            //sql參數
            System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter("@factoryid", CurrentMaintain["FactoryID"].ToString());
            System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter();
            sp2.ParameterName = "@id";
            if (ID != null)
            {
                sp2.Value = ID;
            }
            else
            {
                sp2.Value = DBNull.Value;
            }
            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
            cmds.Add(sp1);
            cmds.Add(sp2);

            if (MyUtility.Check.Empty(CurrentMaintain["FactoryID"]))
            {
                sqlCmd = "select ID,Name,Skill,SewingLineID,FactoryID from Employee where ResignationDate is null" + (ID == null ? "" : " and ID = @id");
            }
            else
            {
                sqlCmd = "select ID,Name,Skill,SewingLineID,FactoryID from Employee where ResignationDate is null and FactoryID = @factoryid" + (ID == null ? "" : " and ID = @id");
            }
            DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out EmployeeData);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Sql connection fail!!\r\n" + result.ToString());
            }
        }

        //將Employee相關欄位值清空
        private void ReviseEmployeeToEmpty(DataRow dr)
        {
            dr["EmployeeID"] = "";
            dr["EmployeeName"] = "";
            dr["EmployeeSkill"] = "";
            dr.EndEdit();
        }

        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["Status"] = "New";
            textBox2.BackColor = Color.White;
        }

        protected override void ClickCopyAfter()
        {
            base.ClickCopyAfter();
            CurrentMaintain["Version"] = DBNull.Value;
            CurrentMaintain["IEReasonID"] = "";
            CurrentMaintain["Status"] = "New";
            textBox2.BackColor = Color.White;
        }

        protected override bool ClickEditBefore()
        {
            if (!PublicPrg.Prgs.GetAuthority(CurrentMaintain["AddName"].ToString()))
            {
                MyUtility.Msg.WarningBox("This record is not created by yourself, so you can't modify this record!!");
                return false;
            }

            if (CurrentMaintain["Status"].ToString().ToUpper() == "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("This record already confirmed, so can't modify this record!!");
                return false;
            }
            textBox2.BackColor = Color.White;
            return true;
        }

        protected override bool ClickDeleteBefore()
        {
            if (!PublicPrg.Prgs.GetAuthority(CurrentMaintain["AddName"].ToString()))
            {
                MyUtility.Msg.WarningBox("This record is not created by yourself, so you can't delete this record!!");
                return false;
            }

            if (CurrentMaintain["Status"].ToString().ToUpper() == "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("This record already confirmed, so can't delete this record!!");
                return false;
            }
            return true;
        }

        protected override bool ClickSaveBefore()
        {
            #region 檢查必輸欄位
            if (MyUtility.Check.Empty(CurrentMaintain["StyleID"]))
            {
                MyUtility.Msg.WarningBox("Style can't empty");
                textBox7.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["ComboType"]))
            {
                MyUtility.Msg.WarningBox("Combo type can't empty");
                textBox2.Focus();
                return false;
            }
            #endregion
            #region 檢查表身不可為空
            DataRow[] findrow = ((DataTable)detailgridbs.DataSource).Select("No = '' or No is null");
            if (findrow.Length > 0)
            {
                MyUtility.Msg.WarningBox("< No. > can't empty!!");
                return false;
            }
            #endregion
            object sumGSD = ((DataTable)detailgridbs.DataSource).Compute("sum(GSD)", "");
            object sumCycle = ((DataTable)detailgridbs.DataSource).Compute("sum(Cycle)", "");
            object maxHighGSD = ((DataTable)detailgridbs.DataSource).Compute("max(TotalGSD)", "");
            object maxHighCycle = ((DataTable)detailgridbs.DataSource).Compute("max(TotalCycle)", "");
            //object countopts = ((DataTable)detailgridbs.DataSource).Compute("count(No)", "");

            int countopts = 0;
            string no = "";
            foreach (DataRow dr in DetailDatas)
            {
                if (!MyUtility.Check.Empty(dr["No"]) && no != dr["No"].ToString())
                {
                    countopts += 1;
                    no = dr["No"].ToString();
                }
            }

            CurrentMaintain["TotalGSD"] = sumGSD;
            CurrentMaintain["TotalCycle"] = sumCycle;
            CurrentMaintain["HighestGSD"] = maxHighGSD;
            CurrentMaintain["HighestCycle"] = maxHighCycle;
            CurrentMaintain["CurrentOperators"] = countopts;
            CurrentMaintain["StandardOutput"] = MyUtility.Check.Empty(CurrentMaintain["TotalCycle"]) ? 0 : MyUtility.Math.Round(3600 * MyUtility.Convert.GetDecimal(CurrentMaintain["CurrentOperators"]) / MyUtility.Convert.GetDecimal(CurrentMaintain["TotalCycle"]), 0); ;
            CurrentMaintain["DailyDemand"] = MyUtility.Math.Round(MyUtility.Convert.GetDecimal(CurrentMaintain["Workhour"]) * MyUtility.Convert.GetDecimal(CurrentMaintain["StandardOutput"]), 0);
            CurrentMaintain["TaktTime"] = MyUtility.Check.Empty(CurrentMaintain["DailyDemand"]) ? 0 : MyUtility.Math.Round(MyUtility.Convert.GetDecimal(CurrentMaintain["NetTime"]) / MyUtility.Convert.GetDecimal(CurrentMaintain["DailyDemand"]), 0); ;

            //Vision為空的話就要填值
            if (MyUtility.Check.Empty(CurrentMaintain["Version"]))
            {
                string newVersion = MyUtility.GetValue.Lookup(string.Format("select isnull(max(Version),0)+1 as Newversion from LineMapping where StyleUKey =  {0}",CurrentMaintain["StyleUkey"].ToString()));
                if (MyUtility.Check.Empty(newVersion))
                {
                    MyUtility.Msg.WarningBox("Get Version fail!!");
                    return false;
                }
                CurrentMaintain["Version"] = newVersion;
            }
            textBox2.BackColor = textBox7.BackColor;
            return true;
        }
        
        protected override void OnDetailGridInsertClick()
        {
            //先紀錄目前Grid所指道的那筆資料
            DataRow tmp = detailgrid.GetDataRow(detailgrid.GetSelectedRowIndex());
            SumNoGSDCycleTime(CurrentDetailData["GroupKey"].ToString());
           // base.OnDetailGridInsertClick();
            DataRow newrow = detailgrid.GetDataRow(detailgrid.GetSelectedRowIndex());
            newrow.ItemArray = tmp.ItemArray;//將剛剛紀錄的資料複製到新增的那筆record
            CurrentDetailData["New"] = true;
            AssignNoGSDCycleTime(CurrentDetailData["GroupKey"].ToString());
            base.OnDetailGridInsertClick();
        }

        protected override void OnDetailGridDelete()
        {
            if (CurrentDetailData["New"].ToString().ToUpper() == "FALSE")
            {
                MyUtility.Msg.WarningBox("This record is set up by system, can't delete!!");
                return;
            }
            string no = CurrentDetailData["No"].ToString(); //紀錄要被刪除的No
            string groupkey = CurrentDetailData["GroupKey"].ToString();
            SumNoGSDCycleTime(groupkey);
            base.OnDetailGridDelete();
            AssignNoGSDCycleTime(groupkey);
            ReclculateGridGSDCycleTime(no);//傳算被刪除掉的No的TotalGSD & Total Cycle Time
        }

        protected override bool ClickPrint()
        {
            Sci.Production.IE.P03_Print callNextForm = new Sci.Production.IE.P03_Print(CurrentMaintain, MyUtility.Convert.GetDecimal(numericBox8.Value));
            callNextForm.ShowDialog(this);
            return base.ClickPrint();
        }

        protected override void ClickUndo()
        {
            base.ClickUndo();
            textBox2.BackColor = textBox7.BackColor;
        }

        //加總傳入的GroupKey的GSD & Cycle Time
        private void SumNoGSDCycleTime(string GroupKey)
        {
            totalGSD = ((DataTable)detailgridbs.DataSource).Compute("sum(GSD)", string.Format("GroupKey = {0}", GroupKey));
            totalCycleTime = ((DataTable)detailgridbs.DataSource).Compute("sum(Cycle)", string.Format("GroupKey = {0}", GroupKey));
        }

        //填輸入的GroupKey的GSD & Cycle Time
        private void AssignNoGSDCycleTime(string GroupKey)
        {
            object countRec = ((DataTable)detailgridbs.DataSource).Compute("count(GroupKey)", string.Format("GroupKey = {0}", GroupKey));
            decimal avgGSD = MyUtility.Check.Empty(Convert.ToDecimal(countRec)) ? MyUtility.Convert.GetDecimal(totalGSD) : Math.Round(MyUtility.Convert.GetDecimal(totalGSD) / MyUtility.Convert.GetDecimal(countRec), 2);
            decimal avgCycleTime = MyUtility.Check.Empty(MyUtility.Convert.GetDecimal(countRec)) ? MyUtility.Convert.GetDecimal(totalCycleTime) : Math.Round(MyUtility.Convert.GetDecimal(totalCycleTime) / MyUtility.Convert.GetDecimal(countRec), 2);
            DataRow[] findRow = ((DataTable)detailgridbs.DataSource).Select(string.Format("GroupKey = {0}", GroupKey));
            int i = 0;
            decimal sumGSD = 0, sumCycleTime = 0;

            //平均分配Cycle Time與GSD，若有餘數就放置最後一筆
            foreach (DataRow dr in findRow)
            {
                i++;
                if (i >= MyUtility.Convert.GetInt(countRec))
                {
                    dr["GSD"] = MyUtility.Convert.GetDecimal(totalGSD) - sumGSD;
                    dr["Cycle"] = MyUtility.Convert.GetDecimal(totalCycleTime) - sumCycleTime;
                }
                else
                {
                    dr["GSD"] = avgGSD;
                    dr["Cycle"] = avgCycleTime;
                    sumGSD = sumGSD + MyUtility.Convert.GetDecimal(dr["GSD"]);
                    sumCycleTime = sumCycleTime + MyUtility.Convert.GetDecimal(dr["Cycle"]);
                }
            }

            foreach (DataRow dr in findRow)
            {
                ReclculateGridGSDCycleTime(dr["No"].ToString());
            }
        }

        //計算DailyDemand,NetTime,TaktTime,LLER,Ideal Target / Hr. (100%), Ideal Daily demand/shift, Ideal Takt Time欄位值
        private void CalculateValue(int Type)
        {
            if (Type == 1)
            {
                CurrentMaintain["DailyDemand"] = MyUtility.Math.Round(MyUtility.Convert.GetDecimal(CurrentMaintain["Workhour"]) * MyUtility.Convert.GetDecimal(CurrentMaintain["StandardOutput"]), 0);
                CurrentMaintain["NetTime"] = MyUtility.Math.Round(MyUtility.Convert.GetDecimal(CurrentMaintain["Workhour"]) * 3600, 0);
                CurrentMaintain["TaktTime"] = MyUtility.Check.Empty(CurrentMaintain["DailyDemand"]) ? 0 : MyUtility.Math.Round(MyUtility.Convert.GetDecimal(CurrentMaintain["NetTime"]) / MyUtility.Convert.GetDecimal(CurrentMaintain["DailyDemand"]), 0);
            }
            numericBox23.Value = MyUtility.Check.Empty(MyUtility.Convert.GetDecimal(CurrentMaintain["TaktTime"]) * MyUtility.Convert.GetDecimal(CurrentMaintain["CurrentOperators"])) ? 0 : MyUtility.Math.Round(MyUtility.Convert.GetDecimal(CurrentMaintain["TotalCycle"]) / MyUtility.Convert.GetDecimal(CurrentMaintain["TaktTime"]) / MyUtility.Convert.GetDecimal(CurrentMaintain["CurrentOperators"]) * 100, 2);
            numericBox7.Value = MyUtility.Check.Empty(CurrentMaintain["TotalGSD"]) ? 0 : MyUtility.Math.Round(3600 * MyUtility.Convert.GetDecimal(CurrentMaintain["IdealOperators"]) / MyUtility.Convert.GetDecimal(CurrentMaintain["TotalGSD"]), 0);
            numericBox6.Value = MyUtility.Math.Round(MyUtility.Convert.GetDecimal(CurrentMaintain["Workhour"]) * MyUtility.Convert.GetDecimal(numericBox7.Value), 0);
            numericBox5.Value = MyUtility.Check.Empty(MyUtility.Convert.GetDecimal(numericBox6.Value)) ? 0 : MyUtility.Math.Round(MyUtility.Convert.GetDecimal(CurrentMaintain["NetTime"]) / MyUtility.Convert.GetDecimal(numericBox6.Value), 0);
        }

        //計算Total % time diff,Highest % time diff,Effieiency(%),Effieiency(%),PPH,LBR欄位值
        private void SaveCalculateValue()
        {
            numericBox11.Value = MyUtility.Check.Empty(CurrentMaintain["TotalGSD"]) ? 0 : MyUtility.Math.Round(((MyUtility.Convert.GetDecimal(CurrentMaintain["TotalGSD"]) - MyUtility.Convert.GetDecimal(CurrentMaintain["TotalCycle"])) / MyUtility.Convert.GetDecimal(CurrentMaintain["TotalGSD"])) * 100, 2);
            numericBox15.Value = MyUtility.Check.Empty(CurrentMaintain["HighestGSD"]) ? 0 : MyUtility.Math.Round(((MyUtility.Convert.GetDecimal(CurrentMaintain["HighestGSD"]) - MyUtility.Convert.GetDecimal(CurrentMaintain["HighestCycle"])) / MyUtility.Convert.GetDecimal(CurrentMaintain["HighestGSD"])) * 100, 2);
            numericBox17.Value = MyUtility.Check.Empty(CurrentMaintain["HighestCycle"]) ? 0 : MyUtility.Math.Round(3600 / MyUtility.Convert.GetDecimal(CurrentMaintain["HighestCycle"]), 2);
            numericBox18.Value = MyUtility.Check.Empty(MyUtility.Convert.GetDecimal(CurrentMaintain["HighestCycle"]) * MyUtility.Convert.GetDecimal(CurrentMaintain["CurrentOperators"])) ? 0 : MyUtility.Math.Round((MyUtility.Convert.GetDecimal(CurrentMaintain["TotalGSD"]) / MyUtility.Convert.GetDecimal(CurrentMaintain["HighestCycle"]) / MyUtility.Convert.GetDecimal(CurrentMaintain["CurrentOperators"])) * 100, 2);
            numericBox21.Value = MyUtility.Check.Empty(CurrentMaintain["CurrentOperators"]) ? 0 : MyUtility.Math.Round(MyUtility.Convert.GetDecimal(numericBox17.Value) * MyUtility.Convert.GetDecimal(numericBox8.Value) / MyUtility.Convert.GetDecimal(CurrentMaintain["CurrentOperators"]), 4);
            numericBox22.Value = MyUtility.Check.Empty(MyUtility.Convert.GetDecimal(CurrentMaintain["HighestCycle"]) * MyUtility.Convert.GetDecimal(CurrentMaintain["CurrentOperators"])) ? 0 : MyUtility.Math.Round(MyUtility.Convert.GetDecimal(CurrentMaintain["TotalCycle"]) / MyUtility.Convert.GetDecimal(CurrentMaintain["HighestCycle"]) / MyUtility.Convert.GetDecimal(CurrentMaintain["CurrentOperators"]) * 100, 2);
        }

        //重新計算Grid的Cycle Time
        private void ReclculateGridGSDCycleTime(string No)
        {
            Object GSD = ((DataTable)detailgridbs.DataSource).Compute("Sum(GSD)", string.Format("No = '{0}'", No));
            Object Cycle = ((DataTable)detailgridbs.DataSource).Compute("Sum(Cycle)", string.Format("No = '{0}'", No));

            DataRow[] findRow = ((DataTable)detailgridbs.DataSource).Select(string.Format("No = '{0}'", No));
            if (findRow.Length > 0)
            {
                foreach (DataRow dr in findRow)
                {
                    dr["TotalGSD"] = GSD;
                    dr["TotalCycle"] = Cycle;
                    dr.EndEdit();
                }
            }
        }

        //No. of Hours
        private void numericBox14_Validated(object sender, EventArgs e)
        {
            CalculateValue(1);
        }

        //Ideal No. of Oprts
        private void numericBox13_Validated(object sender, EventArgs e)
        {
            CalculateValue(0);
        }

        //Factory
        private void textBox1_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem("select ID from Factory where Junk = 0 order by ID", "8", textBox1.Text);
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel) { return; }
            textBox1.Text = item.GetSelectedString();
        }

        //Style#
        private void textBox7_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sqlCmd = "select ID,SeasonID,BrandID,Description,CPU,Ukey from Style where Junk = 0 order by ID,SeasonID";

            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlCmd, "15,10,8,50,5,0", textBox7.Text, "Style#,Season,Brand,Description,CPU,Key");
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel) { return; }
            IList<DataRow> styleData = item.GetSelecteds();
            CurrentMaintain["StyleID"] = styleData[0]["ID"];
            CurrentMaintain["SeasonID"] = styleData[0]["SeasonID"];
            CurrentMaintain["BrandID"] = styleData[0]["BrandID"];
            CurrentMaintain["StyleUKey"] = styleData[0]["Ukey"];
            displayBox3.Value = styleData[0]["Description"];
            numericBox8.Value = MyUtility.Convert.GetDecimal(styleData[0]["CPU"]);

            DataTable ComboType;
            DualResult result = DBProxy.Current.Select(null, string.Format("select Location from Style_Location where StyleUkey = {0}", CurrentMaintain["StyleUKey"].ToString()), out ComboType);
            if (result)
            {
                if (ComboType.Rows.Count > 1)
                {
                    item = new Sci.Win.Tools.SelectItem(ComboType,"Location","2","","Combo Type");
                    returnResult = item.ShowDialog();
                    if (returnResult == DialogResult.Cancel) { return; }
                    CurrentMaintain["ComboType"] = item.GetSelectedString();
                }
                else
                {
                    if (ComboType.Rows.Count != 0)
                    {
                        CurrentMaintain["ComboType"] = ComboType.Rows[0]["Location"];
                    }
                    else
                    {
                        CurrentMaintain["ComboType"] = "";
                    }
                }
            }

        }

        //Combo Type
        private void textBox2_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(string.Format("select Location from Style_Location where StyleUkey = {0}", CurrentMaintain["StyleUKey"].ToString()), "2", "", "Combo Type");
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel) { return; }
            CurrentMaintain["ComboType"] = item.GetSelectedString();
        }

        //撈出ChgOverTarget資料
        private string FindTarget(string Type)
        {
            return MyUtility.GetValue.Lookup(string.Format(@"select Target from ChgOverTarget where Type = '{0}' and MDivisionID = '{1}' and EffectiveDate = (
select MAX(EffectiveDate) from ChgOverTarget where Type = '{0}' and MDivisionID = '{1}' and EffectiveDate <= GETDATE())", Type, Sci.Env.User.Keyword));
        }

        //Confirm
        protected override void ClickConfirm()
        {
            base.ClickConfirm();

            if (!PublicPrg.Prgs.GetAuthority(CurrentMaintain["AddName"].ToString()))
            {
                MyUtility.Msg.WarningBox("This record is not created by yourself, so can't confirm!");
                return;
            }

            string lBRTarget = FindTarget("LBR");
            string lLERTarget = FindTarget("LLER");
            bool checkLBR = (!MyUtility.Check.Empty(lBRTarget) && Convert.ToDecimal(numericBox22.Value) < Convert.ToDecimal(lBRTarget));
            bool checkLLER = (!MyUtility.Check.Empty(lLERTarget) && Convert.ToDecimal(numericBox23.Value) < Convert.ToDecimal(lLERTarget));
            string notHitReasonID = "";

            if (checkLBR || checkLLER)
            {
                StringBuilder msg = new StringBuilder();
                if (checkLBR)
                {
                    msg.Append("LBR is lower than target.\r\n");
                }

                if (checkLLER)
                {
                    msg.Append("LLER is lower than target.\r\n");
                }
                MyUtility.Msg.WarningBox(msg.ToString()+"Please select not hit target reason.");
                string sqlCmd = "select ID, Description from IEReason where Type = 'LM' and Junk = 0";
                Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlCmd, "5,30","");
                DialogResult returnResult = item.ShowDialog();
                if (returnResult == DialogResult.Cancel) { return; }
                notHitReasonID = item.GetSelectedString();
            }

            DualResult result;
            string updateCmd = string.Format("update LineMapping set Status = 'Confirmed', IEReasonID = '{0}',EditName = '{1}', EditDate = GETDATE() where ID = {2}", notHitReasonID, Sci.Env.User.UserID, CurrentMaintain["ID"].ToString());
            result = DBProxy.Current.Execute(null, updateCmd);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Confirm fail!\r\n" + result.ToString());
                return;
            }
            result = RenewData();
            OnDetailEntered();
            EnsureToolbarExt();
        }

        //Unconfirm
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            
            DualResult result;
            string updateCmd = string.Format("update LineMapping set Status = 'New', IEReasonID = '',EditName = '{0}', EditDate = GETDATE() where ID = {1}", Sci.Env.User.UserID, CurrentMaintain["ID"].ToString());
            result = DBProxy.Current.Execute(null, updateCmd);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Unconfirm fail!\r\n" + result.ToString());
                return;
            }
            result = RenewData();
            OnDetailEntered();
            EnsureToolbarExt();
        }

        //Not hit target reason
        private void button1_Click(object sender, EventArgs e)
        {
            //不使用MyUtility.Msg.InfoBox的原因為MyUtility.Msg.InfoBox都有MessageBoxIcon
            MessageBox.Show(MyUtility.GetValue.Lookup(string.Format("select Description from IEReason where Type = 'LM' and ID = '{0}'", CurrentMaintain["IEReasonID"].ToString())).PadRight(60), caption: "Not hit target reason");
        }

        //Copy from other line mapping
        private void button2_Click(object sender, EventArgs e)
        {
            Sci.Production.IE.P03_CopyFromOtherStyle callNextForm = new Sci.Production.IE.P03_CopyFromOtherStyle();
            DialogResult result = callNextForm.ShowDialog(this);
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                DataTable copyLineMapDetail;
                string sqlCmd = string.Format(@"select null as ID,ld.No,ld.Annotation,ld.GSD,ld.TotalGSD,ld.Cycle,ld.TotalCycle,ld.MachineTypeID,ld.OperationID,ld.MoldID,ld.GroupKey,ld.New,ld.EmployeeID,
o.DescEN as Description,e.Name as EmployeeName,e.Skill as EmployeeSkill,iif(ld.Cycle = 0,0,ROUND(ld.GSD/ld.Cycle,2)*100) as Efficiency
from LineMapping_Detail ld
left join Employee e on ld.EmployeeID = e.ID
left join Operation o on ld.OperationID = o.ID
where ld.ID = {0} order by ld.No", callNextForm.P03CopyLineMapping["ID"].ToString());
                DualResult selectResult = DBProxy.Current.Select(null, sqlCmd, out copyLineMapDetail);
                if (!selectResult)
                {
                    MyUtility.Msg.ErrorBox("Query copy linemapping detail fail!!\r\n"+selectResult.ToString());
                    return;
                }

                //刪除現有表身資料
                foreach (DataRow dr in DetailDatas)
                {
                    dr.Delete();
                }

                //將要複製的資料寫入表身Grid
                foreach (DataRow dr in copyLineMapDetail.Rows)
                {
                    if (callNextForm.P03CopyLineMapping["FactoryID"].ToString() != CurrentMaintain["FactoryID"].ToString())
                    {
                        dr["EmployeeID"] = "";
                        dr["EmployeeName"] = "";
                        dr["EmployeeSkill"] = "";
                        dr.EndEdit();
                    }
                    dr.AcceptChanges();
                    dr.SetAdded();
                    ((DataTable)detailgridbs.DataSource).ImportRow(dr);
                }
                
                //填入表頭資料
                CurrentMaintain["IdealOperators"] = callNextForm.P03CopyLineMapping["IdealOperators"].ToString();
                CurrentMaintain["CurrentOperators"] = callNextForm.P03CopyLineMapping["CurrentOperators"].ToString();
                CurrentMaintain["StandardOutput"] = callNextForm.P03CopyLineMapping["StandardOutput"].ToString();
                CurrentMaintain["DailyDemand"] = callNextForm.P03CopyLineMapping["DailyDemand"].ToString();
                CurrentMaintain["Workhour"] = callNextForm.P03CopyLineMapping["Workhour"].ToString();
                CurrentMaintain["NetTime"] = callNextForm.P03CopyLineMapping["NetTime"].ToString();
                CurrentMaintain["TaktTime"] = callNextForm.P03CopyLineMapping["TaktTime"].ToString();
                CurrentMaintain["TotalGSD"] = callNextForm.P03CopyLineMapping["TotalGSD"].ToString();
                CurrentMaintain["TotalCycle"] = callNextForm.P03CopyLineMapping["TotalCycle"].ToString();
                CurrentMaintain["HighestGSD"] = callNextForm.P03CopyLineMapping["HighestGSD"].ToString();
                CurrentMaintain["HighestCycle"] = callNextForm.P03CopyLineMapping["HighestCycle"].ToString();
                CalculateValue(0);
            }
        }

        //Copy from GSD
        private void button3_Click(object sender, EventArgs e)
        {
            //刪除現有表身資料
            foreach (DataRow dr in DetailDatas)
            {
                dr.Delete();
            }
            DataRow timeStudy;
            DataTable timeStudy_Detail;
            string sqlCmd = string.Format(@"select t.* from TimeStudy t, Style s
where t.StyleID = s.ID 
and t.BrandID = s.BrandID 
and t.SeasonID = s.SeasonID 
and s.Ukey = {0}
and t.ComboType = '{1}'", CurrentMaintain["StyleUkey"].ToString(), CurrentMaintain["ComboType"].ToString());
            if (!MyUtility.Check.Seek(sqlCmd, out timeStudy))
            {
                MyUtility.Msg.WarningBox("Fty GSD data not found!!");
                return;
            }

            sqlCmd = string.Format(@"select null as ID,td.Seq as No,td.Annotation,td.SMV as GSD,td.SMV as TotalGSD,td.SMV as Cycle,td.SMV as TotalCycle,td.MachineTypeID,td.OperationID,td.Mold as MoldID,0 as GroupKey,0 as New,'' as EmployeeID,
o.DescEN as Description,'' as EmployeeName,'' as EmployeeSkill,100 as Efficiency
from TimeStudy_Detail td
left join Operation o on td.OperationID = o.ID
where td.ID = {0} order by td.Seq", timeStudy["ID"].ToString());
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out timeStudy_Detail);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query Fty GSD detail fail!!");
                return;
            }


            //將要複製的資料寫入表身Grid
            int i = 0;
            foreach (DataRow dr in timeStudy_Detail.Rows)
            {
                dr["GroupKey"] = ++i;
                dr.AcceptChanges();
                dr.SetAdded();
                ((DataTable)detailgridbs.DataSource).ImportRow(dr);
            }
            object sumSMV = timeStudy_Detail.Compute("sum(GSD)", "");
            object maxSMV = timeStudy_Detail.Compute("max(GSD)", "");
            //填入表頭資料
            CurrentMaintain["IdealOperators"] = timeStudy["NumberSewer"].ToString();
            CurrentMaintain["CurrentOperators"] = i;
            CurrentMaintain["StandardOutput"] = MyUtility.Convert.GetDecimal(sumSMV) == 0 ? 0 : MyUtility.Math.Round(3600 * i / MyUtility.Convert.GetDecimal(sumSMV));
            CurrentMaintain["DailyDemand"] = MyUtility.Math.Round(MyUtility.Convert.GetDecimal(CurrentMaintain["StandardOutput"]) * MyUtility.Convert.GetDecimal(CurrentMaintain["Workhour"]), 0);
            CurrentMaintain["TaktTime"] = MyUtility.Convert.GetDecimal(CurrentMaintain["DailyDemand"]) == 0 ? 0 : MyUtility.Math.Round(MyUtility.Convert.GetDecimal(CurrentMaintain["NetTime"]) / MyUtility.Convert.GetDecimal(CurrentMaintain["DailyDemand"]), 0);
            CurrentMaintain["TotalGSD"] = sumSMV;
            CurrentMaintain["TotalCycle"] = sumSMV;
            CurrentMaintain["HighestGSD"] = maxSMV;
            CurrentMaintain["HighestCycle"] = maxSMV;
            CalculateValue(0);
        }
    }
}
