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
using Sci.Production.PublicPrg;
using Excel = Microsoft.Office.Interop.Excel;
using System.Linq;
using System.Diagnostics;

namespace Sci.Production.Quality
{
    public partial class P01_Weight : Sci.Win.Subs.Input4
    {
        private DataRow maindr;
        private string loginID = Sci.Env.User.UserID;
        private string keyWord = Sci.Env.User.Keyword;
        string excelFile;

        public P01_Weight(bool canedit, string keyvalue1, string keyvalue2, string keyvalue3, DataRow mainDr)
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
            btnEncode.Text = MyUtility.Convert.GetBool(maindr["WeightEncode"]) ? "Amend" : "Encode";
            btnApprove.Text = maindr["Status"].ToString() == "Approved" ? "Unapprove" : "Approve";
            #endregion

            this.save.Enabled = !MyUtility.Convert.GetBool(maindr["WeightEncode"]);

            txtsupplier.TextBox1.IsSupportEditMode = false;
            txtsupplier.TextBox1.ReadOnly = true;
            txtuserApprover.TextBox1.IsSupportEditMode = false;
            txtuserApprover.TextBox1.ReadOnly = true;

            string order_cmd = string.Format("Select * from orders WITH (NOLOCK) where id='{0}'", maindr["POID"]);
            DataRow order_dr;
            if (MyUtility.Check.Seek(order_cmd, out order_dr))
            {
                displayBrand.Text = order_dr["Brandid"].ToString();
                displayStyle.Text = order_dr["Styleid"].ToString();
            }
            else
            {
                displayBrand.Text = "";
                displayStyle.Text = "";
            }
            string po_cmd = string.Format("Select * from po_supp WITH (NOLOCK) where id='{0}' and seq1 = '{1}'", maindr["POID"], maindr["seq1"]);
            DataRow po_dr;
            if (MyUtility.Check.Seek(po_cmd, out po_dr))
            {
                txtsupplier.TextBox1.Text = po_dr["suppid"].ToString();

            }
            else
            {
                txtsupplier.TextBox1.Text = "";
            }
            string Receiving_cmd = string.Format("select b.Refno from Receiving a WITH (NOLOCK) inner join FIR b WITH (NOLOCK) on a.Id=b.Receivingid where b.id='{0}'", maindr["id"]);
            DataRow rec_dr;
            if (MyUtility.Check.Seek(Receiving_cmd, out rec_dr))
            {
                displayRefno.Text = rec_dr["Refno"].ToString();
            }
            else
            {
                displayRefno.Text = "";
            }
            string po_supp_detail_cmd = string.Format("select SCIRefno,colorid from PO_Supp_Detail WITH (NOLOCK) where id='{0}' and seq1='{1}' and seq2='{2}'", maindr["POID"], maindr["seq1"], maindr["seq2"]);
            DataRow po_supp_detail_dr;
            if (MyUtility.Check.Seek(po_supp_detail_cmd, out po_supp_detail_dr))
            {                
                displayColor.Text = po_supp_detail_dr["colorid"].ToString();
            }
            else
            {
                displayColor.Text = "";
            }

            displaySCIRefno.Text = maindr["SCIRefno"].ToString();               
            displayApprover.Text = maindr["ApproveDate"].ToString();
            displayArriveQty.Text = maindr["arriveQty"].ToString();
            dateArriveWHDate.Value = MyUtility.Convert.GetDate(maindr["whseArrival"]);
            dateLastInspectionDate.Value = MyUtility.Convert.GetDate(maindr["WeightDate"]);
            displaySCIRefno1.Text = MyUtility.GetValue.Lookup("Description", maindr["SciRefno"].ToString(), "Fabric", "SCIRefno");
            displaySEQ.Text = maindr["Seq1"].ToString() + "-" + maindr["Seq2"].ToString();
            displaySP.Text = maindr["POID"].ToString();
            displayWKNo.Text = maindr["Exportid"].ToString();
            checkNonWeightTest.Value = maindr["nonWeight"].ToString();
            displayResult.Text = maindr["Weight"].ToString();
            txtuserApprover.TextBox1.Text = maindr["Approve"].ToString();
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
            DataGridViewGeneratorNumericColumnSettings averageWeightM2cell = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorTextColumnSettings ResulCell = Sci.Production.PublicPrg.Prgs.cellResult.GetGridCell();
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
                    string roll_cmd = string.Format("Select roll,dyelot from Receiving_Detail WITH (NOLOCK) Where id='{0}' and poid ='{1}' and seq1 = '{2}' and seq2 ='{3}'", maindr["Receivingid"], maindr["Poid"], maindr["seq1"], maindr["seq2"]);
                    sele = new SelectItem(roll_cmd, "15,10,10",dr["roll"].ToString(), false, ",");
                    DialogResult result = sele.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    dr["Roll"] = sele.GetSelecteds()[0]["Roll"].ToString().Trim();
                    dr["Dyelot"] = sele.GetSelecteds()[0]["Dyelot"].ToString().Trim();
                }
            };
            Rollcell.CellValidating += (s, e) =>
            {
                DataRow dr = grid.GetDataRow(e.RowIndex);
                string oldvalue = dr["Roll"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (!this.EditMode) return;//非編輯模式 
                if (e.RowIndex == -1) return; //沒東西 return
                if (MyUtility.Check.Empty(e.FormattedValue))//沒填入資料,清空dyelot
                {
                    dr["Roll"] = "";
                    dr["Dyelot"] = "";
                    return;
                }
                if (oldvalue == newvalue) return;
                string roll_cmd = string.Format("Select roll,dyelot from Receiving_Detail WITH (NOLOCK) Where id='{0}' and poid ='{1}' and seq1 = '{2}' and seq2 ='{3}' and roll='{4}'", maindr["Receivingid"], maindr["Poid"], maindr["seq1"], maindr["seq2"], e.FormattedValue);
                DataRow roll_dr;
                if (MyUtility.Check.Seek(roll_cmd, out roll_dr))
                {
                    dr["Roll"] = roll_dr["Roll"];
                    dr["Dyelot"] = roll_dr["Dyelot"];
                    dr.EndEdit();
                }
                else
                {
                    dr["Roll"] = "";
                    dr["Dyelot"] = "";
                    dr.EndEdit();
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("<Roll: {0}> data not found!", e.FormattedValue));
                    return;
                }  
            };
            #endregion
            #region Difference
            averageWeightM2cell.CellValidating += (s, e) =>
            {
                DataRow dr = grid.GetDataRow(e.RowIndex);
                string oldvalue = dr["averageWeightM2"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (this.EditMode == false) return;
                if (oldvalue == newvalue) return;
                decimal M2 = MyUtility.Convert.GetDecimal(dr["WeightM2"]);
                decimal AvgM2 = MyUtility.Convert.GetDecimal(e.FormattedValue);
                decimal diff = M2==0 ? 0 : Math.Round(((AvgM2 - M2) / M2) * 100, 2);
                dr["averageWeightM2"] = AvgM2;
                dr["Difference"] = diff;
                dr.EndEdit();
            };
            #endregion
          
            Helper.Controls.Grid.Generator(this.grid)
            .Date("SubmitDate", header: "Submit Date", width: Widths.AnsiChars(10))
            .Text("Roll", header: "Roll", width: Widths.AnsiChars(8), settings: Rollcell)
            .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(4), iseditingreadonly: true)
            .Numeric("WeightM2", header: "DeclaredMass", width: Widths.AnsiChars(7), integer_places: 4, decimal_places: 1, iseditingreadonly: true)
            .Numeric("averageWeightM2", header: "Average Mass", width: Widths.AnsiChars(7), integer_places: 4, decimal_places: 1,settings: averageWeightM2cell)
            .Numeric("Difference", header: "Diff%", width: Widths.AnsiChars(7), integer_places: 5, decimal_places: 2, iseditingreadonly: true)
            .Text("Result", header: "Result", width: Widths.AnsiChars(5), iseditingreadonly: true, settings: ResulCell)
            .Date("InspDate", header: "Insp.Date", width: Widths.AnsiChars(10))
            .CellUser("Inspector", header: "Inspector", width: Widths.AnsiChars(10), userNamePropertyName: "Name")
            .Text("Name", header: "Name", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .Text("Remark", header: "Remark", width: Widths.AnsiChars(20));

            grid.Columns["Roll"].DefaultCellStyle.BackColor = Color.MistyRose;
            grid.Columns["averageWeightM2"].DefaultCellStyle.BackColor = Color.MistyRose;
            grid.Columns["Result"].DefaultCellStyle.BackColor = Color.MistyRose;
            grid.Columns["Result"].DefaultCellStyle.ForeColor = Color.Red;

            grid.Columns["InspDate"].DefaultCellStyle.BackColor = Color.MistyRose;
            grid.Columns["Inspector"].DefaultCellStyle.BackColor = Color.MistyRose;
            grid.Columns["Remark"].DefaultCellStyle.BackColor = Color.MistyRose;
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
            selectDr["WeightM2"] = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup("WeightM2", maindr["SciRefno"].ToString(), "Fabric", "SciRefno"));
            selectDr["difference"] = 0;
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
            drArray = gridTb.Select("averageWeightM2=0");
            if (drArray.Length != 0)
            {
                MyUtility.Msg.WarningBox("<Average Mass> can not be empty.");
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

        private void btnEncode_Click(object sender, EventArgs e)
        {
            string updatesql ="";
            if (MyUtility.Check.Empty(CurrentData) && btnEncode.Text=="Encode")
            {
                MyUtility.Msg.WarningBox("Data not found! ");
                return;
            }
            if (!MyUtility.Convert.GetBool(maindr["WeightEncode"])) //Encode
            {
                if (!MyUtility.Convert.GetBool(maindr["nonWeight"])) //只要沒勾選就要判斷，有勾選就可直接Encode
                {
                    if (MyUtility.GetValue.Lookup("WeaveTypeID", maindr["SCIRefno"].ToString(), "Fabric", "SciRefno") == "KNIT")
                    {
                        //當Fabric.WeaveTypdID = 'Knit' 時必須每ㄧ缸都要有檢驗
                        DataTable dyeDt;
                        string cmd = string.Format(
                        @"Select distinct dyelot from Receiving_Detail a WITH (NOLOCK) where 
                        a.id='{0}' and a.poid='{2}' and a.seq1 ='{3}' and a.seq2='{4}'  
                        and not exists 
                        (Select distinct dyelot from FIR_Weight b WITH (NOLOCK) where b.id={1} and a.dyelot = b.dyelot)"
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
                maindr["Weight"] = result;
                maindr["WeightDate"] = DateTime.Now.ToShortDateString();
                maindr["WeightEncode"] = true;
                maindr["EditName"] = loginID;
                maindr["EditDate"] = DateTime.Now.ToShortDateString();
                #endregion 
                #region 判斷Result 是否要寫入
                string[] returnstr = Sci.Production.PublicPrg.Prgs.GetOverallResult_Status(maindr);
                #endregion 
                #region  寫入實體Table
                updatesql = string.Format(
                @"Update Fir set WeightDate = GetDate(),WeightEncode=1,EditName='{0}',EditDate = GetDate(),Weight = '{1}',Result ='{2}',Status='{4}' where id ={3}", loginID, result, returnstr[0], maindr["ID"], returnstr[1]);
                #endregion
                #region Excel Email 需寄給Encoder的Teamleader 與 Supervisor*****
                DataTable dt_Leader;
                string cmd_leader = string.Format(@"
select ToAddress = stuff ((select concat (';', tmp.email)
						  from (
							  select distinct email from pass1
							  where id in (select Supervisor from pass1 where  id='{0}')
							         or id in (select Manager from Pass1 where id = '{0}')
						  ) tmp
						  for xml path('')
						 ), 1, 1, '')", Sci.Env.User.UserID);
                DBProxy.Current.Select("", cmd_leader, out dt_Leader);
                if (!MyUtility.Check.Empty(dt_Leader)
                    && dt_Leader.Rows.Count > 0)
                {
                    string mailto = dt_Leader.Rows[0]["ToAddress"].ToString();
                    string ccAddress = Env.User.MailAddress;
                    string subject = string.Format(MyUtility.GetValue.Lookup("Subject", "007", "MailTo", "ID"), this.displaySP.Text, this.displayRefno.Text, this.displayColor.Text);
                    string content = string.Format(MyUtility.GetValue.Lookup("content", "007", "MailTo", "ID"), this.displaySP.Text, this.displayRefno.Text, this.displayColor.Text)
                                     + Environment.NewLine
                                     + "Please Approve and Check Fabric Inspection";
                    ToExcel(true);
                    var email = new MailTo(Sci.Env.Cfg.MailFrom, mailto, ccAddress, subject, excelFile, content, false, true);
                    email.ShowDialog(this);
                }
                #endregion

                maindr["Result"] = returnstr[0];
                maindr["Status"] = returnstr[1];
            }
            else //Amend
            {

                #region  寫入虛擬欄位
                maindr["Weight"] = "";
                maindr["WeightDate"] = DBNull.Value;
                maindr["WeightEncode"] = false;                
                maindr["EditName"] = loginID;
                maindr["EditDate"] = DateTime.Now.ToShortDateString();

                //判斷Result and Status 必須先確認Weight="", 判斷才會正確
                string[] returnstr = Sci.Production.PublicPrg.Prgs.GetOverallResult_Status(maindr);
                maindr["Result"] = returnstr[0];
                maindr["Status"] = returnstr[1];
                #endregion 
                #region  寫入實體Table
                updatesql = string.Format(
                @"Update Fir set WeightDate = null,WeightEncode=0,EditName='{0}',EditDate = GetDate(),Weight = '',Result ='{2}',Status='{3}' where id ={1}", loginID, maindr["ID"], returnstr[0], returnstr[1]);
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
                    _transactionscope.Dispose();
                    MyUtility.Msg.InfoBox("Successfully");
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

        private void btnApprove_Click(object sender, EventArgs e)
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
                    _transactionscope.Dispose();
                    MyUtility.Msg.InfoBox("Successfully");
                }
                catch (Exception ex)
                {
                    _transactionscope.Dispose();
                    ShowErr("Commit transaction error.", ex);
                    return;
                }
            }
            #region *****Send Excel Email 完成 需寄給Factory MC*****
            if (this.btnApprove.Text.EqualString("Approve"))
            {
                string strToAddress = MyUtility.GetValue.Lookup("ToAddress", "007", "MailTo", "ID");
                string mailto = strToAddress;
                string mailCC = MyUtility.GetValue.Lookup("CCAddress", "007", "MailTo", "ID");
                string subject = string.Format(MyUtility.GetValue.Lookup("Subject", "007", "MailTo", "ID"), this.displaySP.Text, this.displayRefno.Text, this.displayColor.Text);
                string content = string.Format(MyUtility.GetValue.Lookup("content", "007", "MailTo", "ID"), this.displaySP.Text, this.displayRefno.Text, this.displayColor.Text);

                ToExcel(true);
                var email = new MailTo(Sci.Env.Cfg.MailFrom, mailto, mailCC, subject, excelFile, content, false, true);
                email.ShowDialog(this);
            }
            #endregion
            OnRequery();
        }

        private void button_enable()
        {
            if (maindr == null) return;
            btnEncode.Enabled = this.CanEdit && !this.EditMode && maindr["Status"].ToString() != "Approved";
            this.btnToExcel.Enabled = !this.EditMode;
            string menupk = MyUtility.GetValue.Lookup("Pkey", "Sci.Production.Quality.P01", "MenuDetail", "FormName");
            string pass0pk = MyUtility.GetValue.Lookup("FKPass0", loginID, "Pass1", "ID");
            DataRow pass2_dr;
            string pass2_cmd = string.Format("Select * from Pass2 WITH (NOLOCK) Where FKPass0 ='{0}' and FKMenu='{1}'", pass0pk, menupk);
            int lApprove = 0; //有Confirm權限皆可按Pass的Approve, 有Check權限才可按Fail的Approve(TeamLeader 有Approve權限,Supervisor有Check)
            int lCheck = 0;
            if (MyUtility.Check.Seek(pass2_cmd, out pass2_dr))
            {
                lApprove = pass2_dr["CanConfirm"].ToString() == "True" ? 1 : 0;
                lCheck = pass2_dr["CanCheck"].ToString() == "True" ? 1 : 0;
            }
            if (maindr["Result"].ToString() == "Pass")
            {
                btnApprove.Enabled = this.CanEdit && !this.EditMode && lApprove == 1 && !MyUtility.Check.Empty(maindr["Result"]) && !MyUtility.Check.Empty(maindr["Weight"]);
            }
            else
            {
                btnApprove.Enabled = this.CanEdit && !this.EditMode && lCheck == 1 && !MyUtility.Check.Empty(maindr["Result"]) && !MyUtility.Check.Empty(maindr["Weight"]);
            }
        }

        private void btnToExcel_Click(object sender, EventArgs e)
        {
            ToExcel(false);
        }

        private bool ToExcel(bool isSendMail)
        {
            #region Excel Grid Value
            DataTable dt;
            DualResult xresult;
            if (xresult = DBProxy.Current.Select("Production", string.Format("select Roll,Dyelot,WeightM2,averageWeightM2,Difference,Result,Inspdate,Inspector,Remark from FIR_Weight WITH (NOLOCK) where id='{0}'", textID.Text), out dt))
            {
                if (dt.Rows.Count <= 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!");
                    return false;
                }
            }
            #endregion
            #region Excel 表頭值
            DataTable dt1;
            string SeasonID = "";
            string ContinuityEncode = "";
            DualResult xresult1;

            if (xresult1 = DBProxy.Current.Select("Production", string.Format(
               "select Roll,Dyelot,WeightM2,averageWeightM2,Difference,A.Result,A.Inspdate,Inspector,B.ContinuityEncode,C.SeasonID from FIR_Weight a WITH (NOLOCK) left join FIR b WITH (NOLOCK) on a.ID=b.ID LEFT JOIN ORDERS C ON B.POID=C.ID where a.ID='{0}'", textID.Text), out dt1))
            {
                if (dt1.Rows.Count > 0)
                {
                    SeasonID = dt1.Rows[0]["SeasonID"].ToString();
                    ContinuityEncode = dt1.Rows[0]["ContinuityEncode"].ToString();
                }                
            }
            #endregion
            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Quality_P01_Weight_Report.xltx"); //預先開啟excel app
            objApp.Visible = false;
            MyUtility.Excel.CopyToXls(dt, "", "Quality_P01_Weight_Report.xltx", 5, false, null, objApp);      // 將datatable copy to excel
            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
            objSheets.Cells[2, 2] = displaySP.Text.ToString();
            objSheets.Cells[2, 4] = displaySEQ.Text.ToString();
            objSheets.Cells[2, 6] = displayColor.Text.ToString();
            objSheets.Cells[2, 8] = displayStyle.Text.ToString();
            objSheets.Cells[2, 10] = SeasonID;
            objSheets.Cells[3, 2] = displaySCIRefno.Text.ToString();
            objSheets.Cells[3, 4] = ContinuityEncode;
            objSheets.Cells[3, 6] = displayResult.Text.ToString();
            objSheets.Cells[3, 8] = dateLastInspectionDate.Value;
            objSheets.Cells[3, 10] = displayBrand.Text.ToString();
            objSheets.Cells[4, 2] = displayRefno.Text.ToString();
            objSheets.Cells[4, 4] = displayArriveQty.Text.ToString();
            objSheets.Cells[4, 6] = dateArriveWHDate.Value;
            objSheets.Cells[4, 8] = txtsupplier.DisplayBox1.Text.ToString();
            objSheets.Cells[4, 10] = displayWKNo.Text.ToString();

            objApp.Cells.EntireColumn.AutoFit();    //自動欄寬
            objApp.Cells.EntireRow.AutoFit();       ////自動欄高

            #region Save Excel
            excelFile = Sci.Production.Class.MicrosoftFile.GetName("QA_P01_Weight");
            objApp.ActiveWorkbook.SaveAs(excelFile);
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);
            Marshal.ReleaseComObject(objSheets);
            #endregion

            if (!isSendMail)
            {
                excelFile.OpenFile();
            }
            return true;
        }

        private void buttonToPDF_Click(object sender, EventArgs e)
        {
            DataTable dt;
            DualResult xresult;
            if (xresult = DBProxy.Current.Select("Production", string.Format("select Roll,Dyelot,WeightM2,averageWeightM2,Difference,Result,Inspdate,Inspector,Remark from FIR_Weight WITH (NOLOCK) where id='{0}'", textID.Text), out dt))
            {
                if (dt.Rows.Count <= 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!");
                    return;
                }
            }
            this.ShowWaitMessage("To PDF Processing...");
            Excel.Application objApp = new Excel.Application();
            objApp.DisplayAlerts = false;
            objApp.Workbooks.Add();
            Excel.Sheets exlSheets = objApp.Worksheets;
            Excel.Worksheet newSheet = exlSheets.Item[1];
            exlSheets.Item[1].Delete();
            exlSheets.Item[1].Delete();
            
            ExcelHeadData excelHeadData = new ExcelHeadData();
            excelHeadData.SPNo = maindr["POID"].ToString();
            excelHeadData.Brand = displayBrand.Text;
            excelHeadData.StyleNo = displayStyle.Text;

            DataRow drOrder;
            MyUtility.Check.Seek($@"select o.CustPONo,s.StyleName from dbo.orders o WITH (NOLOCK) 
left join dbo.style s on o.StyleID = s.ID and o.BrandID = s.BrandID and o.SeasonID = s.SeasonID where o.ID = '{maindr["POID"].ToString()}'",out drOrder);
            excelHeadData.PONumber = drOrder["CustPONo"].ToString();
            excelHeadData.StyleName = drOrder["StyleName"].ToString();
            excelHeadData.ArriveQty = maindr["arriveQty"].ToString();
            excelHeadData.FabricRefNo = displayRefno.Text;
            excelHeadData.FabricColor = displayColor.Text;
            excelHeadData.FabricDesc = MyUtility.GetValue.Lookup("Description", maindr["SciRefno"].ToString(), "Fabric", "SCIRefno");
            //取得資料
            //先取Article
            string article_sql = $@"SELECT distinct(oc.article)  as article
FROM [Order_BOF]  bof
inner join PO_Supp_Detail p on p.id=bof.id and bof.SCIRefno=p.SCIRefno
inner join Order_ColorCombo OC on oc.id=p.id and oc.FabricCode=bof.FabricCode
where bof.id='{maindr["POID"].ToString()}' and p.seq1='{maindr["Seq1"].ToString()}' and p.seq2='{maindr["Seq2"].ToString()}'
 ";
            DataTable dt_article;
            DualResult result = DBProxy.Current.Select(null, article_sql, out dt_article);
            if (!result)
            {
                MyUtility.Msg.ErrorBox(result.Messages.ToString());
                return;
            }
            //取得Header資料
            

            //產生excel
            using (dt_article)
            {
                foreach (DataRow articleDr in dt_article.Rows)
                {
                    excelHeadData.ArticleNo = articleDr["article"].ToString();
                    var detailbyDate = from r1 in Datas.AsEnumerable()
                                  group r1 by new
                                  {
                                      SubmitDate = r1["SubmitDate"],
                                      Inspdate = r1["Inspdate"]
                                  } into g
                                  select new
                                  {
                                      SubmitDate = g.Key.SubmitDate,
                                      Inspdate = g.Key.Inspdate
                                  };
                 
                    foreach (var dateFilter in detailbyDate)
                    {
                        excelHeadData.SubmitDate = dateFilter.SubmitDate.Equals(DBNull.Value) ? "" : ((DateTime)dateFilter.SubmitDate).ToOADate().ToString();
                        excelHeadData.ReportDate = dateFilter.Inspdate.Equals(DBNull.Value) ? "" : ((DateTime)dateFilter.Inspdate).ToOADate().ToString();
                        //產生新sheet並填入標題等資料
                        exlSheets.Add();
                        
                        newSheet = exlSheets.Item[1];
                        AddSheetAndHead(excelHeadData, newSheet);

                        //填入detail資料
                        DataRow[] dr_detail = Datas.Where(s => s["SubmitDate"].Equals(dateFilter.SubmitDate) && s["Inspdate"].Equals(dateFilter.Inspdate)).ToArray();
                        string signature = dr_detail.GroupBy(s => s["Inspector"]).Select(s => MyUtility.GetValue.Lookup("Name", s.Key.ToString(), "Pass1", "ID")).JoinToString(",");
                        int detail_start = 16;
                        foreach (DataRow dr in dr_detail)
                        {
                            newSheet.Cells[detail_start, 1].Value = "'" + dr["Dyelot"];
                            newSheet.Cells[detail_start, 2].Value =dr["Roll"];
                            newSheet.Cells[detail_start, 3].Value = dr["WeightM2"];
                            newSheet.Cells[detail_start, 4].Value = dr["AverageWeightM2"];
                            newSheet.Cells[detail_start, 5].Value = dr["Difference"];
                            newSheet.Cells[detail_start, 6].Value = dr["Result"];
                            newSheet.Cells[detail_start, 7].Value = dr["Remark"];
                            newSheet.Range[$"G{detail_start}", $"I{detail_start}"].Merge();
                            detail_start++;
                        }

                        newSheet.get_Range($"A16:I{detail_start-1}").Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                        newSheet.get_Range($"A16:I{detail_start-1}").Borders.Weight = Excel.XlBorderWeight.xlMedium;
                       
                        //簽名欄
                        newSheet.Cells[detail_start + 1, 6].Value = "Signature";
                        newSheet.Cells[detail_start + 1, 6].Font.Bold = true;
                        newSheet.Range[$"F{detail_start+1}", $"I{detail_start+1}"].Merge();
                        newSheet.Cells[detail_start + 2, 6].Value = signature;
                        newSheet.Range[$"F{detail_start + 2}", $"I{detail_start + 2}"].Merge();
                        newSheet.Cells[detail_start + 3, 6].Value = "Checked by:";
                        newSheet.Range[$"F{detail_start + 3}", $"I{detail_start + 3}"].Merge();
                        newSheet.get_Range($"F{detail_start + 1}:I{detail_start + 3}").Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                        newSheet.get_Range($"F{detail_start + 1}:I{detail_start + 3}").Borders.Weight = Excel.XlBorderWeight.xlMedium;
                        newSheet.get_Range($"F{detail_start + 1}:I{detail_start + 3}").Font.Size = 9;
                        //全部置中
                        newSheet.get_Range($"A1:I{detail_start + 3}").HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                        newSheet.get_Range($"A1:I{detail_start + 3}").Font.Name = "Arial";

                        //版面保持一頁
                        newSheet.PageSetup.Zoom = false;
                        newSheet.PageSetup.FitToPagesWide = 1;

                        //針對merge後會過長的欄位作高度調整
                        double newHeight;
                        newSheet.Range["A7", "A7"].RowHeight = MeasureTextHeight(excelHeadData.StyleName, 54);
                        newHeight = MeasureTextHeight(excelHeadData.FabricDesc, 59);
                        newSheet.Range["A12", "A12"].RowHeight = newSheet.Rows[12].Height > newHeight ? newSheet.Rows[12].Height : newHeight;
                        newHeight = MeasureTextHeight(excelHeadData.FabricRefNo, 13);
                        newSheet.Range["A12", "A12"].RowHeight = newSheet.Rows[12].Height > newHeight ? newSheet.Rows[12].Height : newHeight;
                        newSheet.PageSetup.PrintArea = $"A1:I{detail_start + 3}";
                    }

            
                }
            }
            
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("QA_P01_Weight");
            objApp.ActiveWorkbook.SaveAs(strExcelName);
            objApp.ActiveWorkbook.Close(true, Type.Missing, Type.Missing);
            objApp.Quit();
            Marshal.ReleaseComObject(exlSheets);
            Marshal.ReleaseComObject(objApp);
            
            string strPDFFileName = Sci.Production.Class.MicrosoftFile.GetName("QA_P01_Weight", Sci.Production.Class.PDFFileNameExtension.PDF);
            if (ConvertToPDF.ExcelToPDF(strExcelName, strPDFFileName))
            {
                ProcessStartInfo startInfo = new ProcessStartInfo(strPDFFileName);
                Process.Start(startInfo);
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
            this.HideWaitMessage();
        }

        
        private void AddSheetAndHead(ExcelHeadData headData, Excel.Worksheet tmpSheet)
        {
            //設定欄位寬度
            tmpSheet.Columns[1].ColumnWidth = 13.63;
            tmpSheet.Columns[2].ColumnWidth = 10.5;
            tmpSheet.Columns[3].ColumnWidth = 12.80;
            tmpSheet.Columns[4].ColumnWidth = 12.80;
            tmpSheet.Columns[5].ColumnWidth = 11.5;
            tmpSheet.Columns[6].ColumnWidth = 7.1;
            tmpSheet.Columns[7].ColumnWidth = 9.38;
            tmpSheet.Columns[8].ColumnWidth = 8;
            tmpSheet.Columns[9].ColumnWidth = 10.5;
            tmpSheet.Columns.Font.Size = 10;

            tmpSheet.Range["A1", "I1"].Merge();
            tmpSheet.Range["F4", "G4"].Merge();
            tmpSheet.Range["B6", "C6"].Merge();
            tmpSheet.Range["E6", "F6"].Merge();
            tmpSheet.Range["H6", "I6"].Merge();
            tmpSheet.Range["B7", "F7"].Merge();
            tmpSheet.Range["H7", "I7"].Merge();
            tmpSheet.Range["B11", "C11"].Merge();
            tmpSheet.Range["D11", "I11"].Merge();
            tmpSheet.Range["B12", "C12"].Merge();
            tmpSheet.Range["D12", "I12"].Merge();
            tmpSheet.Range["G15", "I15"].Merge();

            tmpSheet.Cells[1, 1].Value = "Fabric Weight Test Report";
            tmpSheet.Cells[1, 1].Font.Bold = true;
            tmpSheet.Cells[1, 1].Font.Size = 18;


            tmpSheet.Cells[4, 1].Value = "Submit Date";
            tmpSheet.Cells[4, 1].Font.Bold = true;
            tmpSheet.Cells[4, 2].Value = headData.SubmitDate;
            tmpSheet.Cells[4, 2].NumberFormat = "YYYY/MM/DD";
            tmpSheet.Cells[4, 3].Value = "Report  Date";
            tmpSheet.Cells[4, 3].Font.Bold = true;
            tmpSheet.Cells[4, 4].Value = headData.ReportDate;
            tmpSheet.Cells[4, 4].NumberFormat = "YYYY/MM/DD";
            tmpSheet.Cells[4, 5].Value = "SP No";
            tmpSheet.Cells[4, 5].Font.Bold = true;
            tmpSheet.Cells[4, 6].Value = headData.SPNo;
            tmpSheet.Cells[4, 8].Value = "Brand";
            tmpSheet.Cells[4, 8].Font.Bold = true;
            tmpSheet.Cells[4, 9].Value = "'" + headData.Brand;

            tmpSheet.Cells[6, 1].Value = "Style No";
            tmpSheet.Cells[6, 1].Font.Bold = true;
            tmpSheet.Cells[6, 2].Value = "'" + headData.StyleNo;
            tmpSheet.Cells[6, 4].Value = "PO Number";
            tmpSheet.Cells[6, 4].Font.Bold = true;
            tmpSheet.Cells[6, 5].Value = "'" + headData.PONumber;
            tmpSheet.Cells[6, 7].Value = "Article No";
            tmpSheet.Cells[6, 7].Font.Bold = true;
            tmpSheet.Cells[6, 8].Value = "'" + headData.ArticleNo;

            tmpSheet.Cells[7, 1].Value = "Style Name";
            tmpSheet.Cells[7, 1].Font.Bold = true;
            tmpSheet.Cells[7, 2].Value = "'" + headData.StyleName;
            tmpSheet.Cells[7, 2].WrapText = true;
            tmpSheet.Cells[7, 7].Value = "Arrive Qty";
            tmpSheet.Cells[7, 7].Font.Bold = true;
            tmpSheet.Cells[7, 8].Value = headData.ArriveQty;
            tmpSheet.Cells[7, 8].NumberFormat = "##,###,###.00";

            tmpSheet.Cells[11, 1].Value = "Fabric Ref No.";
            tmpSheet.Cells[11, 1].Font.Bold = true;
            tmpSheet.Cells[11, 2].Value = "Fabric Color";
            tmpSheet.Cells[11, 2].Font.Bold = true;
            tmpSheet.Cells[11, 4].Value = "Fabric Description";
            tmpSheet.Cells[11, 4].Font.Bold = true;
            tmpSheet.Cells[12, 1].Value = "'" + headData.FabricRefNo;
            tmpSheet.Cells[12, 1].WrapText = true;
            tmpSheet.Cells[12, 2].Value = "'" + headData.FabricColor;
            tmpSheet.Cells[12, 4].Value = "'" + headData.FabricDesc;
            tmpSheet.Cells[12, 4].WrapText = true;

            tmpSheet.Cells[15, 1].Value = "Dye Lot";
            tmpSheet.Cells[15, 1].Font.Bold = true;
            tmpSheet.Cells[15, 2].Value = "Roll No.";
            tmpSheet.Cells[15, 2].Font.Bold = true;
            tmpSheet.Cells[15, 3].Value = "Declared Mass";
            tmpSheet.Cells[15, 3].Font.Bold = true;
            tmpSheet.Cells[15, 4].Value = "Average Mass";
            tmpSheet.Cells[15, 4].Font.Bold = true;
            tmpSheet.Cells[15, 5].Value = "%DIFRENCE";
            tmpSheet.Cells[15, 5].Font.Bold = true;
            tmpSheet.Cells[15, 6].Value = "Result";
            tmpSheet.Cells[15, 6].Font.Bold = true;
            tmpSheet.Cells[15, 7].Value = "Remark";
            tmpSheet.Cells[15, 7].Font.Bold = true;
            tmpSheet.Rows.AutoFit();

            tmpSheet.get_Range("A4:I4").Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            tmpSheet.get_Range("A4:I4").Borders.Weight = Excel.XlBorderWeight.xlMedium;

            tmpSheet.get_Range("A6:I7").Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            tmpSheet.get_Range("A6:I7").Borders.Weight = Excel.XlBorderWeight.xlMedium;

            tmpSheet.get_Range("A11:I12").Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            tmpSheet.get_Range("A11:I12").Borders.Weight = Excel.XlBorderWeight.xlMedium;

            tmpSheet.get_Range("A15:I15").Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            tmpSheet.get_Range("A15:I15").Borders.Weight = Excel.XlBorderWeight.xlMedium;

     
        }

        private double MeasureTextHeight(string text, int width)
        {
            if (string.IsNullOrEmpty(text)) return 0.0;
            var bitmap = new Bitmap(1, 1);
            var graphics = Graphics.FromImage(bitmap);

            var pixelWidth = Convert.ToInt32(width * 7.5);  //7.5 pixels per excel column width
            var drawingFont = new Font("Arial", 11);
            var size = graphics.MeasureString(text, drawingFont, pixelWidth);

            //72 DPI and 96 points per inch.  Excel height in points with max of 409 per Excel requirements.
            return Math.Min(Convert.ToDouble(size.Height) * 72 / 96, 409);
        }

        private class ExcelHeadData
        {
            public string SubmitDate;
            public string ReportDate;
            public string SPNo;
            public string Brand;
            public string StyleNo;
            public string PONumber;
            public string ArticleNo;
            public string StyleName;
            public string ArriveQty;
            public string FabricRefNo;
            public string FabricColor;
            public string FabricDesc;

        }
    }
}
    