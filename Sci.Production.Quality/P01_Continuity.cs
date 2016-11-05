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
using Sci.Trade.Class.Commons;
using System.Runtime.InteropServices;


namespace Sci.Production.Quality
{
    public partial class P01_Continuity : Sci.Win.Subs.Input4
    {
        private DataRow maindr;
        private string loginID = Sci.Env.User.UserID;
        private string keyWord = Sci.Env.User.Keyword;
        public P01_Continuity(bool canedit, string keyvalue1, string keyvalue2, string keyvalue3, DataRow mainDr)
            : base(canedit, keyvalue1, keyvalue2, keyvalue3)

        {
            InitializeComponent();
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
            encode_button.Text = MyUtility.Convert.GetBool(maindr["ContinuityEncode"]) ? "Amend" : "Encode";
            approve_button.Text = maindr["Status"].ToString() == "Approved" ? "Unapprove" : "Approve";
            #endregion
            
            this.save.Enabled = !MyUtility.Convert.GetBool(maindr["ContinuityEncode"]);

            txtsupplier1.TextBox1.IsSupportEditMode = false;
            txtsupplier1.TextBox1.ReadOnly = true;
            txtuser1.TextBox1.IsSupportEditMode = false;
            txtuser1.TextBox1.ReadOnly = true;
            
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
            string Receiving_cmd = string.Format("select a.exportid,a.WhseArrival ,b.Refno from Receiving a inner join FIR b on a.Id=b.Receivingid where b.id='{0}'", maindr["id"]);
            DataRow rec_dr;
            if (MyUtility.Check.Seek(Receiving_cmd,out rec_dr))
            {
                wk_box.Text = rec_dr["exportid"].ToString();
                arrwhdate_box.Value = MyUtility.Convert.GetDate(rec_dr["WhseArrival"]);
                brandrefno_box.Text = rec_dr["Refno"].ToString();
            }
            else
            {
                wk_box.Text = "";
                brandrefno_box.Text = "";
            }
            string po_supp_detail_cmd = string.Format("select SCIRefno,colorid from PO_Supp_Detail where id='{0}' and seq1='{1}' and seq2='{2}'", maindr["POID"], maindr["seq1"], maindr["seq2"]);
            DataRow po_supp_detail_dr;
            if (MyUtility.Check.Seek(po_supp_detail_cmd, out po_supp_detail_dr))
            {                            
                color_box.Text = po_supp_detail_dr["colorid"].ToString();
            }
            else
            {               
                color_box.Text = "";                
            }

            scirefno_box.Text = maindr["SCIRefno"].ToString();
            approve_box.Text = maindr["ApproveDate"].ToString();
            arriveqty_box.Text = maindr["arriveQty"].ToString();
            lastinspdate_box.Value = MyUtility.Convert.GetDate(maindr["ContinuityDate"]);
            refdesc_box.Text = MyUtility.GetValue.Lookup("Description", maindr["SciRefno"].ToString(), "Fabric", "SCIRefno");
            seq_box.Text = maindr["Seq1"].ToString() + "-" + maindr["Seq2"].ToString();
            sp_box.Text = maindr["POID"].ToString();
            checkBox1.Value = maindr["nonContinuity"].ToString();
            result_box.Text = maindr["Continuity"].ToString();
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
            DataGridViewGeneratorTextColumnSettings Resultcell = new DataGridViewGeneratorTextColumnSettings();
            
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
                    string roll_cmd = string.Format("Select roll,dyelot from Receiving_Detail Where id='{0}' and poid ='{1}' and seq1 = '{2}' and seq2 ='{3}'", maindr["Receivingid"], maindr["Poid"], maindr["seq1"], maindr["seq2"]);
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
                string roll_cmd = string.Format("Select roll,dyelot from Receiving_Detail Where id='{0}' and poid ='{1}' and seq1 = '{2}' and seq2 ='{3}' and roll='{4}'", maindr["Receivingid"], maindr["Poid"], maindr["seq1"], maindr["seq2"],e.FormattedValue);
                DataRow roll_dr;
                if (MyUtility.Check.Seek(roll_cmd, out roll_dr))
                {
                    dr["Roll"] = roll_dr["Roll"];
                    dr["Dyelot"] = roll_dr["Dyelot"];
                    dr.EndEdit();
                }
                else
                {
                    MyUtility.Msg.WarningBox("<Roll> data not found!");
                    dr["Roll"] = "";
                    dr["Dyelot"] = "";
                    dr.EndEdit();
                    e.Cancel = true;
                    return;
                }  
            };
            #endregion

            #region Resultcell
            Resultcell.CellMouseDoubleClick += (s, e) =>
            {
                if (!this.EditMode) return;
                DataRow dr = grid.GetDataRow(e.RowIndex);
                if (dr["Result"].ToString() == "Pass") dr["Result"] = "Fail";
                else dr["Result"] = "Pass";
            };
            #endregion

            Helper.Controls.Grid.Generator(this.grid)
            .Text("Roll", header: "Roll", width: Widths.AnsiChars(8), settings: Rollcell)
            .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(4), iseditingreadonly: true)
            .CellScale("Scale", header: "Scale", width: Widths.AnsiChars(5))
            .Text("Result", header: "Result", width: Widths.AnsiChars(5), iseditingreadonly: true,settings: Resultcell)
            .Date("InspDate", header: "Insp.Date", width: Widths.AnsiChars(10))
            .CellUser("Inspector", header: "Inspector", width: Widths.AnsiChars(10), userNamePropertyName: "Name")
            .Text("Name", header: "Name", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .Text("Remark", header: "Remark", width: Widths.AnsiChars(20));

            grid.Columns[0].DefaultCellStyle.BackColor = Color.MistyRose;
            grid.Columns[2].DefaultCellStyle.BackColor = Color.MistyRose;
            grid.Columns[3].DefaultCellStyle.BackColor = Color.MistyRose;
            grid.Columns[3].DefaultCellStyle.ForeColor = Color.Red;

            grid.Columns[4].DefaultCellStyle.BackColor = Color.MistyRose;
            grid.Columns[5].DefaultCellStyle.BackColor = Color.MistyRose;
            grid.Columns[6].DefaultCellStyle.BackColor = Color.MistyRose;
            return true;

        }

        protected override void OnInsert()
        {
            DataTable Dt = (DataTable)gridbs.DataSource;
            base.OnInsert();
            
            DataRow selectDr = ((DataRowView)grid.GetSelecteds(SelectedSort.Index)[0]).Row;

            selectDr["Result"] = "";
            selectDr["Inspdate"] = DateTime.Now.ToShortDateString();
            selectDr["Inspector"] = loginID;
            selectDr["Name"] = MyUtility.GetValue.Lookup("Name", loginID, "Pass1", "ID");
            selectDr["poid"] = maindr["poid"];
            selectDr["SEQ1"] = maindr["SEQ1"];
            selectDr["SEQ2"] = maindr["SEQ2"];
            selectDr["scale"] = "";
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
            drArray = gridTb.Select("Scale=''");
            if (drArray.Length != 0)
            {
                MyUtility.Msg.WarningBox("<Scale> can not be empty.");
                return false;
            }

            drArray = gridTb.Select("Result=''");
            if (drArray.Length != 0)
            {
                MyUtility.Msg.WarningBox("<Rresult> can not be empty.");
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
            if (!MyUtility.Convert.GetBool(maindr["ContinuityEncode"])) //Encode
            {
                if (!MyUtility.Convert.GetBool(maindr["nonContinuity"])) //只要沒勾選就要判斷，有勾選就可直接Encode
                {
                    if (MyUtility.GetValue.Lookup("WeaveTypeID", maindr["SCIRefno"].ToString(), "Fabric", "SciRefno") == "KNIT")
                    {
                        //當Fabric.WeaveTypdID = 'Knit' 時必須每ㄧ缸都要有檢驗
                        DataTable dyeDt;
                        string cmd = string.Format(
                        @"Select distinct dyelot from Receiving_Detail a where 
                        a.id='{0}' and a.poid='{2}' and a.seq1 ='{3}' and a.seq2='{4}'  
                        and not exists 
                        (Select distinct dyelot from FIR_Continuity b where b.id={1} and a.dyelot = b.dyelot)"
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
                                MyUtility.Msg.WarningBox("<Dyelot>:" + dye + " Each Dyelot must be test!");
                                return;
                            }
                        }
                    }
                }
                DataTable gridTb = (DataTable)gridbs.DataSource;
                DataRow[] ResultAry = gridTb.Select("Result = 'Fail'");
                string result = "Pass";
                if (ResultAry.Length > 0) result = "Fail";
                #region  寫入虛擬欄位
                maindr["Continuity"] = result;
                maindr["ContinuityDate"] = DateTime.Now.ToShortDateString();
                maindr["ContinuityEncode"] = true;
                maindr["EditName"] = loginID;
                maindr["EditDate"] = DateTime.Now.ToShortDateString();
                #endregion 
                #region 判斷Result 是否要寫入
                string[] returnstr = Sci.Production.PublicPrg.Prgs.GetOverallResult_Status(maindr["ID"]);
                #endregion 
                #region  寫入實體Table
                updatesql = string.Format(
                @"Update Fir set ContinuityDate = GetDate(),ContinuityEncode=1,EditName='{0}',EditDate = GetDate(),Continuity = '{1}',Result ='{2}',Status='{4}' where id ={3}", loginID, result, returnstr[0], maindr["ID"], returnstr[1]);
                #endregion
                 //*****Send Excel Email 尚未完成 需寄給Encoder的Teamleader 與 Supervisor*****

                //*********************************************************************************
                maindr["Result"] = returnstr[0];
                maindr["Status"] = returnstr[1];
            }
            else //Amend
            {
                #region 判斷Result 是否要寫入
                string[] returnstr = Sci.Production.PublicPrg.Prgs.GetOverallResult_Status(maindr["ID"]);
                #endregion 

                #region  寫入虛擬欄位
                maindr["Continuity"] = "";
                maindr["ContinuityDate"] = DBNull.Value;
                maindr["ContinuityEncode"] = false;
                maindr["Status"] = returnstr[1];
                maindr["EditName"] = loginID;
                maindr["EditDate"] = DateTime.Now.ToShortDateString();
                maindr["Result"] = returnstr[0];
                #endregion 


                #region  寫入實體Table
                updatesql = string.Format(
                @"Update Fir set ContinuityDate = null,ContinuityEncode=0,EditName='{0}',EditDate = GetDate(),Continuity = '',Result ='{2}',Status='{3}' where id ={1}", loginID, maindr["ID"], returnstr[0], returnstr[1]);
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
                lApprove =pass2_dr["CanConfirm"].ToString()=="True"? 1 : 0;
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
            
            #region Excel Grid Value
            DataTable dt;
            DualResult xresult;
             if (xresult = DBProxy.Current.Select("Production", string.Format("select Roll,Dyelot,Scale,Result,Inspdate,Inspector,Remark from FIR_Continuity where id='{0}'", textID.Text), out dt))
            //if (xresult = DBProxy.Current.Select("Production", "select Roll,Dyelot,Scale,Result,Inspdate,Inspector,Remark from FIR_Continuity  where id in ('489066')", out dt)) //測試用
            {
                if (dt.Rows.Count <= 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!");
                    return;
                }
            }
            #endregion
            #region Excel 表頭值
            DataTable dt1;
            DualResult xresult1;
            //if (xresult1 = DBProxy.Current.Select("Production",
              //  "select Roll,Dyelot,Scale,a.Result,a.Inspdate,Inspector,a.Remark,B.ContinuityEncode,C.SeasonID from FIR_Continuity a left join FIR b on a.ID=b.ID LEFT JOIN ORDERS C ON B.POID=C.ID where a.ID in ('489066')", out dt1))//測試用
            if (xresult1 = DBProxy.Current.Select("Production", string.Format(
            "select Roll,Dyelot,Scale,a.Result,a.Inspdate,Inspector,a.Remark,B.ContinuityEncode,C.SeasonID from FIR_Continuity a left join FIR b on a.ID=b.ID LEFT JOIN ORDERS C ON B.POID=C.ID where a.ID='{0}'", textID.Text), out dt1))
            {
                if (dt1.Rows.Count <= 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!");
                    return;
                }
            }
            #endregion
            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\P01_Continuity_Report.xltx"); //預先開啟excel app
            MyUtility.Excel.CopyToXls(dt, "", "P01_Continuity_Report.xltx", 5, true, null, objApp);      // 將datatable copy to excel
            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
            objSheets.Cells[2, 2] = sp_box.Text.ToString();
            objSheets.Cells[2, 4] = seq_box.Text.ToString();
            objSheets.Cells[2, 6] = color_box.Text.ToString();
            objSheets.Cells[2, 8] = style_box.Text.ToString();
            objSheets.Cells[2, 10] = dt1.Rows[0]["SeasonID"];//Season can not find
            objSheets.Cells[3, 2] = scirefno_box.Text.ToString();
            objSheets.Cells[3, 4] = dt1.Rows[0]["ContinuityEncode"]; //Encode can not find
            objSheets.Cells[3, 6] = result_box.Text.ToString();
            objSheets.Cells[3, 8] = lastinspdate_box.Value;
            objSheets.Cells[3, 10] = brand_box.Text.ToString();
            objSheets.Cells[4, 2] = brandrefno_box.Text.ToString();
            objSheets.Cells[4, 4] = arriveqty_box.Text.ToString();
            //objSheets.Cells[4, 6] = ((DateTime)arrwhdate_box.Text).ToShortDateString();
            objSheets.Cells[4, 6] = arrwhdate_box.Value;
            //objSheets.Cells[4, 8] = txtsupplier1.Text.ToString();
            objSheets.Cells[4, 8] = txtsupplier1.DisplayBox1.Text.ToString();
            objSheets.Cells[4, 10] = wk_box.Text.ToString();

            objApp.Cells.EntireColumn.AutoFit();    //自動欄寬
            objApp.Cells.EntireRow.AutoFit();       ////自動欄高

            if (objSheets != null) Marshal.FinalReleaseComObject(objSheets);    //釋放sheet
            if (objApp != null) Marshal.FinalReleaseComObject(objApp);          //釋放objApp

          
        }

      
    }
}
