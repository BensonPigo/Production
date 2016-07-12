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
using System.Runtime.InteropServices;


namespace Sci.Production.Quality
{
    public partial class P01_PhysicalInspection : Sci.Win.Subs.Input4
    {
        private DataRow maindr;
        private string loginID = Sci.Env.User.UserID;
        private string keyWord = Sci.Env.User.Keyword;
        private bool firstQuery = true;
        DataTable Fir_physical_Defect;
        public P01_PhysicalInspection(bool canedit, string keyvalue1, string keyvalue2, string keyvalue3,DataRow mainDr)
            : base(canedit, keyvalue1, keyvalue2, keyvalue3)

        {
            InitializeComponent();
            

            txtsupplier1.TextBox1.IsSupportEditMode = false;
            txtsupplier1.TextBox1.ReadOnly = true;
            txtuser1.TextBox1.IsSupportEditMode = false;
            txtuser1.TextBox1.ReadOnly = true;
            maindr = mainDr;
            this.textID.Text = keyvalue1;
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

            string order_cmd = string.Format("Select * from orders where id='{0}'", maindr["POID"]);
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
            result_box.Text = maindr["physical"].ToString();
            txtuser1.TextBox1.Text = maindr["Approve"].ToString();
            
            return base.OnRequery();
            
        }
        protected override void OnRequeryPost(DataTable datas)
        {
            
            base.OnRequeryPost(datas);
            datas.Columns.Add("Name", typeof(string));
            datas.Columns.Add("POID", typeof(string));
            datas.Columns.Add("SEQ1", typeof(string));
            datas.Columns.Add("SEQ2", typeof(string));
            datas.Columns.Add("NewKey", typeof(int));
            int i = 0;
            foreach (DataRow dr in datas.Rows)
            {
                dr["Name"] = MyUtility.GetValue.Lookup("Name", dr["Inspector"].ToString(), "Pass1", "ID");
                dr["NewKey"] = i;
                dr["poid"] = maindr["poid"];
                dr["SEQ1"] = maindr["SEQ1"];
                dr["SEQ2"] = maindr["SEQ2"];

                i++;
            }
            #region 撈取下一層資料Defect
            string str_defect = string.Format("Select a.* ,b.NewKey from Fir_physical_Defect a,#tmp b Where a.id = b.id and a.FIR_PhysicalDetailUKey = b.DetailUkey");

            MyUtility.Tool.ProcessWithDatatable(datas, "ID,NewKey,DetailUkey", str_defect, out Fir_physical_Defect);
            #endregion
        }
        
        protected override bool OnGridSetup()
        {

            
            DataGridViewGeneratorTextColumnSettings Rollcell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorNumericColumnSettings Ydscell = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings TotalPointcell = new DataGridViewGeneratorNumericColumnSettings();
            #region TotalPoint Double Click
            TotalPointcell.EditingMouseDoubleClick += (s, e) =>
            {
                grid.ValidateControl();
                P01_PhysicalInspection_Defect frm = new P01_PhysicalInspection_Defect(Fir_physical_Defect);
                frm.Set(EditMode, Datas, grid.GetDataRow(e.RowIndex));
                frm.ShowDialog(this);     
            };
            #endregion
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
            .Numeric("Ticketyds", header: "Ticket Yds", width: Widths.AnsiChars(7), integer_places: 8, decimal_places: 2, iseditingreadonly: true)
            .Numeric("Actualyds", header: "Act.Yds\nInspected", width: Widths.AnsiChars(7), integer_places: 8, decimal_places: 2)
            .Numeric("fullwidth", header: "Cut. Width", width: Widths.AnsiChars(7), integer_places: 8, decimal_places: 2)//misstake
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
        protected override DualResult OnSave()
        {
            DualResult upResult = new DualResult(true) ;
            #region Fir_Physical //因為要抓取DetailUkey 所以要自己Overridr
            string update_cmd = "";
            List<string> append_cmd = new List<string>();
            DataTable idenDt;
            string iden;

            foreach (DataRow dr in Datas)
            {
                if (dr.RowState == DataRowState.Deleted)
                {
                    update_cmd = update_cmd + string.Format(
                    @"Delete From Fir_physical Where DetailUkey = {0} ;",
                    dr["DetailUKey", DataRowVersion.Original]);
                }
                int bolMoisture = 0;
                string inspdate;
                if (MyUtility.Check.Empty(dr["InspDate"])) inspdate = ""; //判斷Inspect Date
                else inspdate = Convert.ToDateTime(dr["InspDate"]).ToShortDateString();
                
                if (MyUtility.Convert.GetBool(dr["Moisture"])) bolMoisture = 1; 
                if (dr.RowState == DataRowState.Added)
                {
                    string add_cmd = "";
                    
                    add_cmd =string.Format(
                    @"Insert into Fir_Physical
(ID,Roll,Dyelot,TicketYds,ActualYds,FullWidth,ActualWidth,TotalPoint,PointRate,Grade,Result,Remark,InspDate,Inspector,Moisture,AddName,AddDate) 
Values({0},'{1}','{2}',{3},{4},{5},{6},{7},{8},'{9}','{10}','{11}','{12}','{13}',{14},'{15}',GetDate()) ;",
                            dr["ID"], dr["roll"], dr["Dyelot"], dr["TicketYds"], dr["ActualYds"], dr["FullWidth"], dr["ActualWidth"], dr["TotalPoint"], dr["PointRate"], dr["Grade"], dr["Result"], dr["Remark"], inspdate, dr["Inspector"], bolMoisture, loginID);
                    add_cmd = add_cmd + "select @@IDENTITY as ii";
                    #region 先存入Table 撈取Idenitiy
                    upResult = DBProxy.Current.Select(null, add_cmd, out idenDt);
                    if (upResult)
                    {
                        iden = idenDt.Rows[0]["ii"].ToString(); //取出Identity

                        DataRow[] Ary = Fir_physical_Defect.Select(string.Format("NewKey={0}", dr["NewKey"]));
                        if (Ary.Length > 0)
                        {
                            foreach (DataRow Ary_dr in Ary)
                            {
                                Ary_dr["FIR_PhysicalDetailUKey"] = iden;
                            }
                        }
                    }
                    else
                    {
                        return upResult;
                    }
                    #endregion
                }

                if (dr.RowState == DataRowState.Modified)
                {
                    update_cmd = update_cmd + string.Format(
                        @"Update Fir_Physical set Roll = '{0}' ,Dyelot='{1}',TicketYds = {2},ActualYds = {3},FullWidth ={4},TotalPoint ={6},PointRate={7},Grade='{8}',Result='{9}',Remark='{10}',InspDate='{11}',Inspector='{12}',Moisture={13},EditName = '{14}',EditDate = GetDate() 
Where DetailUkey = {15};",
                           dr["roll"], dr["Dyelot"], dr["TicketYds"], dr["ActualYds"], dr["FullWidth"], dr["ActualWidth"], dr["TotalPoint"], dr["PointRate"], dr["Grade"], dr["Result"], dr["Remark"], inspdate, dr["Inspector"], bolMoisture, loginID,dr["DetailUkey"]);
                }
            }
            if (update_cmd != "")
            {
                upResult = DBProxy.Current.Execute(null, update_cmd);
                if (!upResult) return upResult;
            }
            #endregion 

            #region Fir_Physical_Defect
            string update_cmd1 = "";
            foreach (DataRow dr in Fir_physical_Defect.Rows)
            {

                if (dr.RowState == DataRowState.Deleted)
                {
                    update_cmd1 = update_cmd1 + string.Format(
                    @"Delete From Fir_physical_Defect Where ID = {0} and FIR_PhysicalDetailUKey = {1} and DefectLocation ='{2}';",
                    dr["ID", DataRowVersion.Original], dr["FIR_PhysicalDetailUKey", DataRowVersion.Original], dr["DefectLocation", DataRowVersion.Original]);
                }
                if (dr.RowState == DataRowState.Added)
                {
                    update_cmd1 = update_cmd1 + string.Format(
                        @"Insert into Fir_Physical_Defect(ID,FIR_PhysicalDetailUKey,DefectLocation,DefectRecord,Point) 
                            Values({0},{1},'{2}','{3}',{4});",
                            dr["ID"], dr["FIR_PhysicalDetailUKey"], dr["DefectLocation"], dr["DefectRecord"], dr["Point"]);
                }
                if (dr.RowState == DataRowState.Modified)
                {
                    update_cmd1 = update_cmd1 + string.Format(
                        @"Update Fir_Physical_Defect set DefectLocation = '{2}',DefectRecord = '{3}',Point = {4}
                            Where ID = {0} and FIR_PhysicalDetailUKey = {1};",
                            dr["ID"], dr["FIR_PhysicalDetailUKey"], dr["DefectLocation"], dr["DefectRecord"], dr["Point"]);
                }
            }
            if (update_cmd1 != "")
            {
                upResult = DBProxy.Current.Execute(null, update_cmd1);
            }
            #endregion
            return upResult;
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
                        @"Select distinct dyelot from Receiving_Detail a where 
                        a.id='{0}' and a.poid='{2}' and a.seq1 ='{3}' and a.seq2='{4}'  
                        and not exists 
                        (Select distinct dyelot from FIR_Physical b where b.id={1} and a.dyelot = b.dyelot)"
                        , maindr["receivingid"], maindr["id"], maindr["POID"], maindr["seq1"], maindr["seq2"]);
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
                string[] returnstr = Sci.Production.PublicPrg.Prgs.GetOverallResult_Status(maindr["ID"]);
                #endregion 
                #region  寫入實體Table
                updatesql = string.Format(
                @"Update Fir set PhysicalDate = GetDate(),PhysicalEncode=1,EditName='{0}',EditDate = GetDate(),Physical = '{1}',Result ='{2}',TotalDefectPoint = {4},TotalInspYds = {5},Status='{6}' where id ={3}", loginID, result, returnstr[0], maindr["ID"], sumPoint, sumTotalYds, returnstr[1]);
                #endregion
                 //*****Send Excel Email 尚未完成 需寄給Encoder的Teamleader 與 Supervisor*****

                //*********************************************************************************
                maindr["Result"] = returnstr[0];
                maindr["Status"] = returnstr[1];
            }
            else //Amend
            {
                string[] returnstr = Sci.Production.PublicPrg.Prgs.GetOverallResult_Status(maindr["ID"]);
                #region  寫入虛擬欄位
                maindr["Physical"] = "";
                maindr["PhysicalDate"] = DBNull.Value;
                maindr["PhysicalEncode"] = false;
                maindr["Status"] = returnstr[1];
                maindr["EditName"] = loginID;
                maindr["EditDate"] = DateTime.Now.ToShortDateString();
                maindr["TotalDefectPoint"] = 0;
                maindr["TotalInspYds"] = 0;
                maindr["Result"] = returnstr[0];
                #endregion 
                #region  寫入實體Table
                updatesql = string.Format(
                @"Update Fir set PhysicalDate = null,PhysicalEncode=0,EditName='{0}',EditDate = GetDate(),Physical = '',Result ='{2}',TotalDefectPoint = 0,TotalInspYds = 0,Status='{3}' where id ={1}", loginID, maindr["ID"], returnstr[0], returnstr[1]);
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
            string menupk = MyUtility.GetValue.Lookup("Pkey", "Sci.Production.Quality.P01", "MenuDetail", "FormName");
            string pass0pk = MyUtility.GetValue.Lookup("FKPass0", loginID, "Pass1", "ID");
            DataRow pass2_dr;
            string pass2_cmd = string.Format("Select * from Pass2 Where FKPass0 ={0} and FKMenu={1}", pass0pk, menupk);
            int lApprove = 0; //有Confirm權限皆可按Pass的Approve, 有Check權限才可按Fail的Approve(TeamLeader 有Approve權限,Supervisor有Check)
            int lCheck = 0;
            if (MyUtility.Check.Seek(pass2_cmd, out pass2_dr))
            {
                lApprove = pass2_dr["CanConfirm"].ToString() == "True" ? 1 : 0;
                lCheck = pass2_dr["CanCheck"].ToString() == "True" ? 1 : 0;
            }
            if (maindr["Result"].ToString() == "Pass")
            {
                approve_button.Enabled = this.CanEdit && !this.EditMode && lApprove == 1 && !MyUtility.Check.Empty(maindr["Result"]);
            }
            else
            {
                approve_button.Enabled = this.CanEdit && !this.EditMode && lCheck == 1 && !MyUtility.Check.Empty(maindr["Result"]);
            }
        }
        
        private void button3_Click(object sender, EventArgs e)
        {
            #region DataTables && 共用變數
            //FabricDefect 基本資料 DB
            DataTable dtBasic;
            DualResult bResult;
            if (bResult = DBProxy.Current.Select("Production", "SELECT id,type,DescriptionEN FROM FabricDefect order by ID", out dtBasic))
            {
                if (dtBasic.Rows.Count < 1)
                {
                    MyUtility.Msg.WarningBox("Data not found!");
                    return;
                }
            }
            //FIR_Physical_Defect DB
            DataTable dt;
            DualResult dResult;
            if (dResult = DBProxy.Current.Select("Production", string.Format("select * from FIR_Physical A LEFT JOIN FIR_Physical_Defect B ON A.DetailUkey = B.FIR_PhysicalDetailUKey WHERE A.ID='{0}'", textID.Text), out dt))
            {
                if (dt.Rows.Count < 1 )
                {
                    MyUtility.Msg.WarningBox("Data not found!");
                }
            }

            #endregion
           


            string strXltName = Sci.Env.Cfg.XltPathDir + "P01_Physical_Inspection_Report.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);                  
            excel.Visible = true;
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            #region FabricDefect 基本資料
            int counts = dtBasic.Rows.Count;
            int int_X = 6;
            int int_Y = 1;
            int int_z = 0;//判斷是否為第一次

            string typeColumn = "typeColumn";
            MyUtility.Msg.WaitWindows("Starting EXCEL...");

            for (int i=0 ; i < counts; i++)
            {
                if (dtBasic.Rows[i]["type"].ToString() != typeColumn && dtBasic.Rows[i]["type"].ToString() != null)
                {
                    int_X = 6;
                    if (int_Y == 1 && int_z==0) //first time
                    {
                        worksheet.Cells[6, int_Y] = "Code".ToString();
                        typeColumn = dtBasic.Rows[i]["type"].ToString();
                        worksheet.Cells[6, int_Y + 1] = typeColumn.ToString();
                        int_X++;
                         int_z = 1;
                    }
                    else
                    {
                        int_X++;
                        int_Y = int_Y + 2;
                        worksheet.Cells[6, int_Y] = "Code".ToString();
                        typeColumn = dtBasic.Rows[i]["type"].ToString();
                        worksheet.Cells[6, int_Y + 1] = typeColumn.ToString();                    
                    }                    
                }
                worksheet.Cells[int_X, int_Y] = dtBasic.Rows[i]["id"].ToString();
                worksheet.Cells[int_X, int_Y+1] = dtBasic.Rows[i]["DescriptionEN"].ToString();
                    int_X++;
            }
            #endregion
            #region FIR_Physical
            
         

            int gridCounts = grid.RowCount;
            int rowcount = 0;

            for (int i = 0; i < gridCounts; i++)
            {
                
                excel.Cells[14+(i*8), 1] = this.grid.Rows[rowcount].Cells["Roll"].Value.ToString();
                excel.Cells[14+(i*8), 2] = this.grid.Rows[rowcount].Cells["Ticketyds"].Value.ToString();
                excel.Cells[14+(i*8), 3] = this.grid.Rows[rowcount].Cells["Actualyds"].Value.ToString();
                excel.Cells[14+(i*8), 4] = this.grid.Rows[rowcount].Cells["fullwidth"].Value.ToString();
                excel.Cells[14+(i*8), 5] = this.grid.Rows[rowcount].Cells["fullwidth"].Value.ToString();
                excel.Cells[14+(i*8), 6] = this.grid.Rows[rowcount].Cells["actualwidth"].Value.ToString();
                excel.Cells[14+(i*8), 7] = this.grid.Rows[rowcount].Cells["totalpoint"].Value.ToString();
                excel.Cells[14+(i*8), 8] = this.grid.Rows[rowcount].Cells["pointRate"].Value.ToString();
                excel.Cells[14+(i*8), 9] = this.grid.Rows[rowcount].Cells["Grade"].Value.ToString();
                excel.Cells[14+(i*8), 10] = this.grid.Rows[rowcount].Cells["Result"].Value.ToString();
                rowcount++;
                

                #region FIR_Physical_Defect
                int dtRowCount = (int)dt.Rows.Count;
                for (int ii = 1; ii < 11; ii++)
                {
                    if (ii % 2 == 1)
                    {
                        excel.Cells[15+(i*8), ii] = "Yards";
                        
                        if (dtRowCount >= ii)
                        {
                            excel.Cells[16+(i*8), ii] = dt.Rows[ii - 1]["DefectLocation"];
                        }
                       
                    }
                    else
                    {
                        excel.Cells[15+(i*8), ii] = "Defect";
                        if (dtRowCount >= ii)
                        {
                            excel.Cells[16+(i*8), ii] = dt.Rows[i]["DefectRecord"];
                        }                       
                    }
                }
                //變色 titile
                worksheet.Range[excel.Cells[15 + (i * 8), 1], excel.Cells[15 + (i * 8), 10]].Interior.color = Color.Pink;
                worksheet.Range[excel.Cells[15 + (i * 8), 1], excel.Cells[15 + (i * 8), 10]].Borders.LineStyle = 1;
                
                #endregion
                DataTable dtcombo;
                DualResult dcResult;

                if (dcResult = DBProxy.Current.Select("Production", string.Format(
		@"select a.ID,a.Roll,'Continuity' type_c,a.Result,a.Remark,a.Inspector,'Shadebond' type_s,b.Result,b.Remark,b.Inspector,'Weight' type_w,c.Result,c.Remark,c.Inspector from FIR_Continuity a
		left join FIR_Shadebond b on a.ID=b.ID and a.Roll=b.Roll
		left join FIR_Weight c on a.ID=c.ID and a.Roll=c.Roll
		where a.ID='{0}' and a.Roll='{1}'", textID.Text,dt.Rows[i]["roll"].ToString()), out dtcombo))
                {
                    if (dtcombo.Rows.Count < 1 )
                    {
                        excel.Cells[17 + (i * 8), 2] = "Result";
                        excel.Cells[17 + (i * 8), 3] = "Comment";
                        excel.Cells[17 + (i * 8), 4] = "Inspector";
                        excel.Cells[18 + (i * 8), 1] = "Contiunity ";
                        excel.Cells[19 + (i * 8), 1] = "Shad bond";
                        excel.Cells[20 + (i * 8), 1] = "Weight";
                        excel.Cells[21 + (i * 8), 1] = "Moisture";
                        worksheet.Range[excel.Cells[17 + (i * 8), 1], excel.Cells[17 + (i * 8), 4]].Interior.Color = Color.Pink;
                        worksheet.Range[excel.Cells[17 + (i * 8), 1], excel.Cells[17 + (i * 8), 4]].Borders.LineStyle = 1;
                    }
                    else
                    {
                        excel.Cells[17 + (i * 8), 2] = "Result";
                        excel.Cells[17 + (i * 8), 3] = "Comment";
                        excel.Cells[17 + (i * 8), 4] = "Inspector";
                        excel.Cells[18 + (i * 8), 1] = "Contiunity ";
                        excel.Cells[19 + (i * 8), 1] = "Shad bond";
                        excel.Cells[20 + (i * 8), 1] = "Weight";
                        excel.Cells[21 + (i * 8), 1] = "Moisture";

                        excel.Cells[19 + (i * 8), 2] = dtcombo.Rows[i]["Result"].ToString();
                        excel.Cells[19 + (i * 8), 3] = dtcombo.Rows[i]["Comment"].ToString();
                        excel.Cells[19 + (i * 8), 4] = dtcombo.Rows[i]["Inspector"].ToString();

                        excel.Cells[20 + (i * 8), 2] = dtcombo.Rows[i]["Result"].ToString();
                        excel.Cells[20 + (i * 8), 3] = dtcombo.Rows[i]["Comment"].ToString();
                        excel.Cells[20 + (i * 8), 4] = dtcombo.Rows[i]["Inspector"].ToString();

                        excel.Cells[21 + (i * 8), 2] = dtcombo.Rows[i]["Result"].ToString();
                        excel.Cells[21 + (i * 8), 3] = dtcombo.Rows[i]["Comment"].ToString();
                        excel.Cells[21 + (i * 8), 4] = dtcombo.Rows[i]["Inspector"].ToString();
                        worksheet.Range[excel.Cells[17 + (i * 8), 1], excel.Cells[17 + (i * 8), 4]].Interior.Color = Color.Pink;
                        worksheet.Range[excel.Cells[17 + (i * 8), 1], excel.Cells[17 + (i * 8), 4]].Borders.LineStyle = 1;
                    }
                }        
            }

            #endregion

            Random random = new Random();
            string excelFile = "Excelaaa" + Convert.ToDateTime(DateTime.Now).ToString("yyyyMMddHHmmss") + " - " + Convert.ToString(Convert.ToInt32(random.NextDouble() * 10000)) + ".xlsx";
            worksheet.SaveAs("D:\\Excel\\QA_P01" + excelFile);

            if (worksheet != null) Marshal.FinalReleaseComObject(worksheet);    //釋放sheet
            if (excel != null) Marshal.FinalReleaseComObject(excel);          //釋放objApp


        }
    }
}
