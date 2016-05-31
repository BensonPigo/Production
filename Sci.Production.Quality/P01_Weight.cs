using Ict;
using Ict.Win;
using Sci.Production.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Sci.Win;
using Sci.Data;
using System.Transactions;
using Sci.Win.Tools;
using Sci.Production.Quality;


namespace Sci.Production.Quality
{
    public partial class P01_Weight : Sci.Win.Subs.Input4
    {
        private DataRow maindr;
        private string loginID = Sci.Env.User.UserID;
        private string keyWord = Sci.Env.User.Keyword;
        public P01_Weight(bool canedit, string keyvalue1, string keyvalue2, string keyvalue3, DataRow mainDr)
            : base(canedit, keyvalue1, keyvalue2, keyvalue3)

        {
            InitializeComponent();
            

            txtsupplier1.TextBox1.IsSupportEditMode = false;
            txtsupplier1.TextBox1.ReadOnly = true;
            txtuser1.TextBox1.IsSupportEditMode = false;
            txtuser1.TextBox1.ReadOnly = true;
            maindr = mainDr;
            string order_cmd = string.Format("Select * from orders where id='{0}'",maindr["POID"]);
            DataRow order_dr;
            if (MyUtility.Check.Seek(order_cmd, out order_dr))
            {
                brand_box.Text = order_dr["Brandid"].ToString();
                style_box.Text = order_dr["Styleid"].ToString();
            }
            else
            {
                brand_box.Text = "";
                style_box.Text = "";
            }
            string po_cmd = string.Format("Select * from po_supp where id='{0}' and seq1 = '{1}'", maindr["POID"], maindr["seq1"]);
            DataRow po_dr;
            if (MyUtility.Check.Seek(po_cmd, out po_dr))
            {
                txtsupplier1.TextBox1.Text = po_dr["suppid"].ToString();
                
            }
            else
            {
                txtsupplier1.TextBox1.Text = "";
            }

            approve_box.Text = maindr["ApproveDate"].ToString();
            arriveqty_box.Text = maindr["arriveQty"].ToString();
            arrwhdate_box.Text = MyUtility.Convert.GetDate(maindr["whseArrival"]).ToString();
            brandrefno_box.Text = maindr["SCIRefno"].ToString();
            color_box.Text = maindr["Colorid"].ToString();
            lastinspdate_box.Text = MyUtility.Convert.GetDate(maindr["physicalDate"]).ToString();
            refdesc_box.Text = MyUtility.GetValue.Lookup("Description", maindr["SciRefno"].ToString(), "Fabric", "SCIRefno");
            scirefno_box.Text = maindr["SciRefno"].ToString();           
            seq_box.Text = maindr["Seq1"].ToString() + "-" + maindr["Seq2"].ToString();
            sp_box.Text = maindr["POID"].ToString();    
            wk_box.Text = maindr["Exportid"].ToString();
            checkBox1.Value = maindr["nonphysical"].ToString();
            comboBox1.Text = maindr["physical"].ToString();

            

            
        }
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            Dictionary<String, String> comboBox1_RowSource = new Dictionary<string, string>();
            comboBox1_RowSource.Add("Pass", "Pass");
            comboBox1_RowSource.Add("Fail", "Fail");
            comboBox1.DataSource = new BindingSource(comboBox1_RowSource, null);
            comboBox1.ValueMember = "Key";
            comboBox1.DisplayMember = "Value";

        }
        protected override void OnEditModeChanged()
        {
            base.OnEditModeChanged();
            button_enable();
        }
        protected override DualResult OnRequery()
        {
            #region Encode/Approve Enable
            button_enable();
            encode_button.Text = MyUtility.Convert.GetBool(maindr["PhysicalEncode"]) ? "Amend" : "Encode";
            approve_button.Text = maindr["Status"].ToString() == "Approved" ? "Unapprove" : "Approve";
            #endregion

            this.save.Enabled = !MyUtility.Convert.GetBool(maindr["PhysicalEncode"]);


            return base.OnRequery();
        }
        protected override void OnRequeryPost(DataTable datas)
        {
            
            base.OnRequeryPost(datas);
            datas.Columns.Add("Name", typeof(string));
            datas.Columns.Add("POID", typeof(string));
            datas.Columns.Add("SEQ1", typeof(string));
            datas.Columns.Add("SEQ2", typeof(string));
            foreach (DataRow dr in datas.Rows)
            {
                dr["Name"] = MyUtility.GetValue.Lookup("Name", dr["Inspector"].ToString(), "Pass1", "ID");
                dr["poid"] = maindr["poid"];
                dr["SEQ1"] = maindr["SEQ1"];
                dr["SEQ2"] = maindr["SEQ2"];

            }
        }

        protected override bool OnGridSetup()
        {

            DataGridViewGeneratorTextColumnSettings Rollcell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorNumericColumnSettings Ydscell = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings TotalPointcell = new DataGridViewGeneratorNumericColumnSettings();
            #region Roll
            Rollcell.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode == false) return;
                if (e.RowIndex == -1) return;
                if (e.Button == MouseButtons.Right)
                {
                    // Parent form 若是非編輯狀態就 return 
                    DataRow dr = grid.GetDataRow(e.RowIndex);
                    SelectItem sele;
                    string roll_cmd = string.Format("Select roll,dyelot,StockQty from Receiving_Detail Where id='{0}' and poid ='{1}' and seq1 = '{2}' and seq2 ='{3}'", maindr["Receivingid"], maindr["Poid"], maindr["seq1"], maindr["seq2"]);
                    sele = new SelectItem(roll_cmd, "15,10,10",dr["roll"].ToString(), false, ",");
                    DialogResult result = sele.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    e.EditingControl.Text = sele.GetSelectedString();
                }
            };
            Rollcell.CellValidating += (s, e) =>
            {
                DataRow dr = grid.GetDataRow(e.RowIndex);
                string oldvalue = dr["Roll"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (this.EditMode == false) return;
                if (oldvalue == newvalue) return;
                string roll_cmd = string.Format("Select roll,dyelot,StockQty from Receiving_Detail Where id='{0}' and poid ='{1}' and seq1 = '{2}' and seq2 ='{3}' and roll='{4}'", maindr["Receivingid"], maindr["Poid"], maindr["seq1"], maindr["seq2"],e.FormattedValue);
                DataRow roll_dr;
                if (MyUtility.Check.Seek(roll_cmd, out roll_dr))
                {
                    dr["Roll"] = roll_dr["Roll"];
                    dr["Dyelot"] = roll_dr["Dyelot"];
                    dr["Ticketyds"] = roll_dr["StockQty"];
                    dr.EndEdit();
                }
                else
                {
                    MyUtility.Msg.WarningBox("<Roll> data not found!");
                    dr["Roll"] = "";
                    dr["Dyelot"] = "";
                    dr["Ticketyds"] = "";
                    dr.EndEdit();
                    e.Cancel = true;
                    return;
                }  
            };
            #endregion
            #region Act Yds
            Ydscell.CellValidating += (s, e) =>
            {
                DataRow dr = grid.GetDataRow(e.RowIndex);
                string oldvalue = dr["actualyds"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (this.EditMode == false) return;
                if (oldvalue == newvalue) return;
                double pointrate = Math.Round((MyUtility.Convert.GetDouble(dr["totalpoint"]) / MyUtility.Convert.GetDouble(e.FormattedValue)) * 100, 2);
                dr["pointrate"] = pointrate;
                dr["actualyds"] = newvalue;
                dr.EndEdit();
            };
            #endregion
            Helper.Controls.Grid.Generator(this.grid)
            .Text("Roll", header: "Roll", width: Widths.AnsiChars(8), settings: Rollcell)
            .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(4), iseditingreadonly: true)
            .Numeric("DeclaredMass", header: "DeclaredMass", width: Widths.AnsiChars(7), integer_places: 8, decimal_places: 2, iseditingreadonly: true)
            .Numeric("averageWeightM2", header: "Act.Yds\nInspected", width: Widths.AnsiChars(7), integer_places: 8, decimal_places: 2)
            .Numeric("fullwidth", header: "Full width", width: Widths.AnsiChars(7), integer_places: 5, decimal_places: 2)
            .Numeric("actualwidth", header: "Actual Width", width: Widths.AnsiChars(7), integer_places: 5, decimal_places: 2)
            .Numeric("totalpoint", header: "Total Points", width: Widths.AnsiChars(7), integer_places: 6, iseditingreadonly: true, settings: TotalPointcell)
            .Numeric("pointRate", header: "Point Rate \nper 100yds", width: Widths.AnsiChars(5), iseditingreadonly: true, integer_places: 6)
            .Text("Result", header: "Result", width: Widths.AnsiChars(5), iseditingreadonly: true)
            .Text("Grade", header: "Grade", width: Widths.AnsiChars(1), iseditingreadonly: true)
            .CheckBox("moisture", header: "Moisture", width: Widths.AnsiChars(2), iseditable: true, trueValue: 1, falseValue: 0)
            .Text("Remark", header: "Remark", width: Widths.AnsiChars(20))
            .Date("InspDate", header: "Insp.Date", width: Widths.AnsiChars(10))
            .CellUser("Inspector",header:"Inspector", width: Widths.AnsiChars(10),userNamePropertyName:"Name")
            .Text("Name", header: "Name", width: Widths.AnsiChars(20), iseditingreadonly: true);

            grid.Columns[0].DefaultCellStyle.BackColor = Color.MistyRose;
            grid.Columns[3].DefaultCellStyle.BackColor = Color.MistyRose;
            grid.Columns[4].DefaultCellStyle.BackColor = Color.MistyRose;
            grid.Columns[5].DefaultCellStyle.BackColor = Color.MistyRose;
            grid.Columns[6].DefaultCellStyle.BackColor = Color.LightYellow;
            grid.Columns[10].DefaultCellStyle.BackColor = Color.MistyRose;
            grid.Columns[11].DefaultCellStyle.BackColor = Color.MistyRose;
            grid.Columns[12].DefaultCellStyle.BackColor = Color.MistyRose;
            grid.Columns[13].DefaultCellStyle.BackColor = Color.MistyRose;


            return true;

        }

        protected override void OnInsert()
        {
            DataTable Dt = (DataTable)gridbs.DataSource;
            int Maxi = MyUtility.Convert.GetInt(Dt.Compute("Max(NewKey)",""));
            base.OnInsert();
            
            DataRow selectDr = ((DataRowView)grid.GetSelecteds(SelectedSort.Index)[0]).Row;

            selectDr["Inspdate"] = DateTime.Now.ToShortDateString();
            selectDr["Inspector"] = loginID;
            selectDr["Name"] = MyUtility.GetValue.Lookup("Name", loginID, "Pass1", "ID");
            selectDr["NewKey"] = Maxi + 1;
            selectDr["Moisture"] =0;
            selectDr["poid"] = maindr["poid"];
            selectDr["SEQ1"] = maindr["SEQ1"];
            selectDr["SEQ2"] = maindr["SEQ2"];
        }

        protected override bool OnSaveBefore()
        {
            DataTable gridTb = (DataTable)gridbs.DataSource;
            #region 判斷空白不可存檔
            DataRow[] drArray;
            drArray = gridTb.Select("Roll=''");
            if (drArray.Length != 0)
            {
                MyUtility.Msg.WarningBox("<Roll> can not be empty.");
                return false;
            }
            drArray = gridTb.Select("actualyds=0");
            if (drArray.Length != 0)
            {
                MyUtility.Msg.WarningBox("<Actual Yds Inspected> can not be empty.");
                return false;
            }
            drArray = gridTb.Select("fullwidth=0");
            if (drArray.Length != 0)
            {
                MyUtility.Msg.WarningBox("<Full Width> can not be empty.");
                return false;
            }
            drArray = gridTb.Select("actualwidth=0");
            if (drArray.Length != 0)
            {
                MyUtility.Msg.WarningBox("<Actual Width> can not be empty.");
                return false;
            }
            drArray = gridTb.Select("Inspdate is null");
            if (drArray.Length != 0)
            {
                MyUtility.Msg.WarningBox("<Insection Date> can not be empty.");
                return false;
            }
            drArray = gridTb.Select("inspector=''");
            if (drArray.Length != 0)
            {
                MyUtility.Msg.WarningBox("<Inspector> can not be empty.");
                return false;
            }
            #endregion


            return base.OnSaveBefore();
        }

        private void encode_button_Click(object sender, EventArgs e)
        {
            string updatesql ="";
            if (!MyUtility.Convert.GetBool(maindr["PhysicalEncode"])) //Encode
            {
                if (!MyUtility.Convert.GetBool(maindr["nonPhysical"])) //只要沒勾選就要判斷，有勾選就可直接Encode
                {
                    //至少收料的每ㄧ缸都要有檢驗紀錄 ,找尋有收料的缸沒在檢驗出現
                    DataTable dyeDt;
                    string cmd = string.Format(
                        @"Select distinct dyelot from Receiving_Detail where id='{0}' and poid='{2}' and seq1 ='{3}' and seq2='{4}'  
                        and  dyelot not in 
                        (Select distinct dyelot from FIR_Physical where id='{1}')", maindr["receivingid"], maindr["id"],maindr["POID"],maindr["seq1"],maindr["seq2"]);
                    DualResult dResult = DBProxy.Current.Select(null, cmd, out dyeDt);
                    if (dResult)
                    {
                        if (dyeDt.Rows.Count > 0)
                        {
                            string dye = "";
                            foreach (DataRow dr in dyeDt.Rows)
                            {
                                dye = dye + dr["Dyelot"].ToString() + ",";
                            }
                            MyUtility.Msg.WarningBox("<Dyelot>:" + dye + " Each Dyelot must be inspected!");
                            return;
                        }
                    }
                }
                DataTable gridTb = (DataTable)gridbs.DataSource;
                DataRow[] ResultAry = gridTb.Select("Result = 'Fail'");
                string result = "Pass";
                if (ResultAry.Length > 0) result = "Fail";
                #region  寫入虛擬欄位
                maindr["Physical"] = result;
                maindr["PhysicalDate"] = DateTime.Now.ToShortDateString();
                maindr["PhysicalEncode"] = true;
                maindr["EditName"] = loginID;
                maindr["EditDate"] = DateTime.Now.ToShortDateString();
                int sumPoint = MyUtility.Convert.GetInt(gridTb.Compute("Sum(totalpoint)", ""));
                decimal sumTotalYds = MyUtility.Convert.GetDecimal(gridTb.Compute("Sum(actualyds)", ""));
                maindr["TotalDefectPoint"] = sumPoint;
                maindr["TotalInspYds"] = sumTotalYds;
                #endregion 
                #region 判斷Result 是否要寫入
                string allResult = "";
                if ((!MyUtility.Check.Empty(maindr["Physical"]) || MyUtility.Convert.GetBool(maindr["Nonphysical"])) && (!MyUtility.Check.Empty(maindr["Weight"]) || MyUtility.Convert.GetBool(maindr["NonWeight"])) && (!MyUtility.Check.Empty(maindr["ShadeBond"]) || MyUtility.Convert.GetBool(maindr["NonShadeBond"])) && (!MyUtility.Check.Empty(maindr["Continuity"]) || MyUtility.Convert.GetBool(maindr["NonContinuity"])))
                {
                    if (maindr["Physical"].ToString() == "Fail" || maindr["Weight"].ToString() == "Fail" || maindr["ShadeBond"].ToString() == "Fail" || maindr["Continuity"].ToString() == "Fail") allResult = "Fail";
                    else allResult = "Pass";

                    maindr["Status"] = "Confirmed";

                }
                #endregion 
                #region  寫入實體Table
                updatesql = string.Format(
                @"Update Fir set PhysicalDate = GetDate(),PhysicalEncode=1,EditName='{0}',EditDate = GetDate(),Physical = '{1}',Result ='{2}',TotalDefectPoint = {4},TotalInspYds = {5},Status='Confirmed' where id ={3}", loginID, result, allResult, maindr["ID"], sumPoint, sumTotalYds);
                #endregion
                 //*****Send Excel Email 尚未完成 需寄給Encoder的Teamleader 與 Supervisor*****

                //*********************************************************************************
                maindr["Result"] = allResult;
            }
            else //Amend
            {
                #region  寫入虛擬欄位
                maindr["Physical"] = "";
                maindr["PhysicalDate"] = DBNull.Value;
                maindr["PhysicalEncode"] = false;
                maindr["Status"] = "New";
                maindr["EditName"] = loginID;
                maindr["EditDate"] = DateTime.Now.ToShortDateString();
                maindr["TotalDefectPoint"] = 0;
                maindr["TotalInspYds"] = 0;
                maindr["Result"] = "";
                #endregion 
                #region  寫入實體Table
                updatesql = string.Format(
                @"Update Fir set PhysicalDate = null,PhysicalEncode=0,EditName='{0}',EditDate = GetDate(),Physical = '',Result ='',TotalDefectPoint = 0,TotalInspYds = 0,Status='New' where id ={1}", loginID, maindr["ID"]);
                #endregion
            }
            DualResult upResult;
            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    if (!(upResult = DBProxy.Current.Execute(null, updatesql)))
                    {
                        _transactionscope.Dispose();
                        return;
                    }
                    _transactionscope.Complete();
                    MyUtility.Msg.WarningBox("Successfully");
                }
                catch (Exception ex)
                {
                    _transactionscope.Dispose();
                    ShowErr("Commit transaction error.", ex);
                    return;
                }
            }
            OnRequery();
        }

        private void approve_button_Click(object sender, EventArgs e)
        {
            string updatesql = "";

            if (maindr["Status"].ToString() == "Confirmed")
            {
                maindr["Status"] = "Approved";
                maindr["Approve"] = loginID;
                maindr["ApproveDate"] = DateTime.Now.ToShortDateString();
                maindr["EditName"] = loginID;
                maindr["EditDate"] = DateTime.Now;
                #region  寫入實體Table
                updatesql = string.Format(
                @"Update Fir set Status = 'Approved',Approve='{0}',EditName='{0}',EditDate = GetDate(),ApproveDate = GetDate() where id ={1}", loginID, maindr["ID"]);
                #endregion
            }
            else
            {
                maindr["Status"] = "Confirmed";
                maindr["Approve"] = "";
                maindr["ApproveDate"] = DBNull.Value;
                maindr["EditName"] = loginID;
                maindr["EditDate"] = DateTime.Now;
                #region  寫入實體Table
                updatesql = string.Format(
                @"Update Fir set Status = 'Confirmed',Approve='',EditName='{0}',EditDate = GetDate(),ApproveDate = null where id ={1}", loginID, maindr["ID"]);
                #endregion
            }
            DualResult upResult;
            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    if (!(upResult = DBProxy.Current.Execute(null, updatesql)))
                    {
                        _transactionscope.Dispose();
                        return;
                    }
                    _transactionscope.Complete();
                    MyUtility.Msg.WarningBox("Successfully");
                }
                catch (Exception ex)
                {
                    _transactionscope.Dispose();
                    ShowErr("Commit transaction error.", ex);
                    return;
                }
            }
            //*****Send Excel Email 尚未完成 需寄給Factory MC*****

            //*********************************************************************************
            OnRequery();
        }

        private void button_enable()
        {
            if (maindr == null) return;
            encode_button.Enabled = this.CanEdit && !this.EditMode && maindr["Status"].ToString() != "Approved";
            string menupk = MyUtility.GetValue.Lookup("Ukey", "Sci.Production.Quality.P01", "MenuDetail", "FormName");
            string pass0pk = MyUtility.GetValue.Lookup("FKPass0", loginID, "Pass1", "ID");
            DataRow pass2_dr;
            string pass2_cmd = string.Format("Select * from Pass2 Where FKPass0 ={0} and FKMenu={1}", pass0pk, menupk);
            int lApprove = 0; //有Confirm權限皆可按Pass的Approve, 有Check權限才可按Fail的Approve(TeamLeader 有Approve權限,Supervisor有Check)
            int lCheck = 0;
            if (MyUtility.Check.Seek(pass2_cmd, out pass2_dr))
            {
                lApprove = MyUtility.Convert.GetInt(pass2_dr["CanConfirm"]);
                lCheck = MyUtility.Convert.GetInt(pass2_dr["CanCheck"]);
            }
            if (maindr["Result"].ToString() == "Pass")
            {
                approve_button.Enabled = !this.CanEdit && !this.EditMode && lApprove == 1 && !MyUtility.Check.Empty(maindr["Result"]);
            }
            else
            {
                approve_button.Enabled = !this.CanEdit && !this.EditMode && lCheck == 1 && !MyUtility.Check.Empty(maindr["Result"]);
            }
        }
    }
}
