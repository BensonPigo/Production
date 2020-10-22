using Ict;
using Ict.Win;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Sci.Data;
using System.Transactions;
using Sci.Win.Tools;
using System.Runtime.InteropServices;
using Sci.Production.PublicPrg;
using Excel = Microsoft.Office.Interop.Excel;
using System.Linq;
using System.Diagnostics;

namespace Sci.Production.Quality
{
    /// <inheritdoc/>
    public partial class P01_Weight : Win.Subs.Input4
    {
        private readonly DataRow maindr;
        private readonly string loginID = Env.User.UserID;
        private readonly string keyWord = Env.User.Keyword;
        private string excelFile;

        /// <inheritdoc/>
        public P01_Weight(bool canedit, string keyvalue1, string keyvalue2, string keyvalue3, DataRow mainDr)
            : base(canedit, keyvalue1, keyvalue2, keyvalue3)
        {
            this.InitializeComponent();
            this.maindr = mainDr;
            this.textID.Text = keyvalue1;
        }

        /// <inheritdoc/>
        protected override void OnEditModeChanged()
        {
            base.OnEditModeChanged();
            this.Button_enable();
        }

        /// <inheritdoc/>
        protected override DualResult OnRequery()
        {
            #region Encode/Approve Enable
            this.Button_enable();
            this.btnEncode.Text = MyUtility.Convert.GetBool(this.maindr["WeightEncode"]) ? "Amend" : "Encode";
            this.btnApprove.Text = this.maindr["Status"].ToString() == "Approved" ? "Unapprove" : "Approve";
            #endregion

            this.save.Enabled = !MyUtility.Convert.GetBool(this.maindr["WeightEncode"]);

            this.txtsupplier.TextBox1.IsSupportEditMode = false;
            this.txtsupplier.TextBox1.ReadOnly = true;
            this.txtuserApprover.TextBox1.IsSupportEditMode = false;
            this.txtuserApprover.TextBox1.ReadOnly = true;

            string order_cmd = string.Format("Select * from orders WITH (NOLOCK) where id='{0}'", this.maindr["POID"]);
            DataRow order_dr;
            if (MyUtility.Check.Seek(order_cmd, out order_dr))
            {
                this.displayBrand.Text = order_dr["Brandid"].ToString();
                this.displayStyle.Text = order_dr["Styleid"].ToString();
            }
            else
            {
                this.displayBrand.Text = string.Empty;
                this.displayStyle.Text = string.Empty;
            }

            string po_cmd = string.Format("Select * from po_supp WITH (NOLOCK) where id='{0}' and seq1 = '{1}'", this.maindr["POID"], this.maindr["seq1"]);
            DataRow po_dr;
            if (MyUtility.Check.Seek(po_cmd, out po_dr))
            {
                this.txtsupplier.TextBox1.Text = po_dr["suppid"].ToString();
            }
            else
            {
                this.txtsupplier.TextBox1.Text = string.Empty;
            }

            string receiving_cmd = string.Format("select b.Refno from Receiving a WITH (NOLOCK) inner join FIR b WITH (NOLOCK) on a.Id=b.Receivingid where b.id='{0}'", this.maindr["id"]);
            DataRow rec_dr;
            if (MyUtility.Check.Seek(receiving_cmd, out rec_dr))
            {
                this.displayRefno.Text = rec_dr["Refno"].ToString();
            }
            else
            {
                this.displayRefno.Text = string.Empty;
            }

            string po_supp_detail_cmd = string.Format("select SCIRefno,colorid from PO_Supp_Detail WITH (NOLOCK) where id='{0}' and seq1='{1}' and seq2='{2}'", this.maindr["POID"], this.maindr["seq1"], this.maindr["seq2"]);
            DataRow po_supp_detail_dr;
            if (MyUtility.Check.Seek(po_supp_detail_cmd, out po_supp_detail_dr))
            {
                this.displayColor.Text = po_supp_detail_dr["colorid"].ToString();
            }
            else
            {
                this.displayColor.Text = string.Empty;
            }

            this.displaySCIRefno.Text = this.maindr["SCIRefno"].ToString();
            this.displayApprover.Text = this.maindr["ApproveDate"].ToString();
            this.displayArriveQty.Text = this.maindr["arriveQty"].ToString();
            this.dateArriveWHDate.Value = MyUtility.Convert.GetDate(this.maindr["whseArrival"]);
            this.dateLastInspectionDate.Value = MyUtility.Convert.GetDate(this.maindr["WeightDate"]);
            this.displaySCIRefno1.Text = MyUtility.GetValue.Lookup("Description", this.maindr["SciRefno"].ToString(), "Fabric", "SCIRefno");
            this.displaySEQ.Text = this.maindr["Seq1"].ToString() + "-" + this.maindr["Seq2"].ToString();
            this.displaySP.Text = this.maindr["POID"].ToString();
            this.displayWKNo.Text = this.maindr["Exportid"].ToString();
            this.checkNonWeightTest.Value = this.maindr["nonWeight"].ToString();
            this.displayResult.Text = this.maindr["Weight"].ToString();
            this.txtuserApprover.TextBox1.Text = this.maindr["Approve"].ToString();
            this.txtWeightInspector.Text = this.maindr["WeightInspector"].ToString();
            return base.OnRequery();
        }

        /// <inheritdoc/>
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
                dr["poid"] = this.maindr["poid"];
                dr["SEQ1"] = this.maindr["SEQ1"];
                dr["SEQ2"] = this.maindr["SEQ2"];
            }
        }

        /// <inheritdoc/>
        protected override bool OnGridSetup()
        {
            DataGridViewGeneratorTextColumnSettings rollcell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorNumericColumnSettings averageWeightM2cell = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorTextColumnSettings resulCell = Prgs.CellResult.GetGridCell();
            #region Roll
            rollcell.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode == false)
                {
                    return;
                }

                if (e.RowIndex == -1)
                {
                    return;
                }

                if (e.Button == MouseButtons.Right)
                {
                    // Parent form 若是非編輯狀態就 return
                    DataRow dr = this.grid.GetDataRow(e.RowIndex);
                    SelectItem sele;
                    string roll_cmd = string.Format("Select roll,dyelot from Receiving_Detail WITH (NOLOCK) Where id='{0}' and poid ='{1}' and seq1 = '{2}' and seq2 ='{3}'", this.maindr["Receivingid"], this.maindr["Poid"], this.maindr["seq1"], this.maindr["seq2"]);

                    if (!MyUtility.Check.Seek(roll_cmd))
                    {
                        roll_cmd = string.Format("Select roll,dyelot from TransferIn_Detail WITH (NOLOCK) Where id='{0}' and poid ='{1}' and seq1 = '{2}' and seq2 ='{3}'", this.maindr["Receivingid"], this.maindr["Poid"], this.maindr["seq1"], this.maindr["seq2"]);
                    }

                    sele = new SelectItem(roll_cmd, "15,10,10", dr["roll"].ToString(), false, ",");
                    DialogResult result = sele.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    dr["Roll"] = sele.GetSelecteds()[0]["Roll"].ToString().Trim();
                    dr["Dyelot"] = sele.GetSelecteds()[0]["Dyelot"].ToString().Trim();
                }
            };
            rollcell.CellValidating += (s, e) =>
            {
                DataRow dr = this.grid.GetDataRow(e.RowIndex);
                string oldvalue = dr["Roll"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (!this.EditMode)
                {
                    return; // 非編輯模式
                }

                if (e.RowIndex == -1)
                {
                    return; // 沒東西 return
                }

                if (MyUtility.Check.Empty(e.FormattedValue))
                {
                    // 沒填入資料,清空dyelot
                    dr["Roll"] = string.Empty;
                    dr["Dyelot"] = string.Empty;
                    return;
                }

                if (oldvalue == newvalue)
                {
                    return;
                }

                string roll_cmd = string.Format("Select roll,dyelot from Receiving_Detail WITH (NOLOCK) Where id='{0}' and poid ='{1}' and seq1 = '{2}' and seq2 ='{3}' and roll='{4}'", this.maindr["Receivingid"], this.maindr["Poid"], this.maindr["seq1"], this.maindr["seq2"], e.FormattedValue);

                if (!MyUtility.Check.Seek(roll_cmd))
                {
                    roll_cmd = string.Format("Select roll,dyelot from TransferIn_Detail WITH (NOLOCK) Where id='{0}' and poid ='{1}' and seq1 = '{2}' and seq2 ='{3}' and roll='{4}'", this.maindr["Receivingid"], this.maindr["Poid"], this.maindr["seq1"], this.maindr["seq2"], e.FormattedValue);
                }

                DataRow roll_dr;
                if (MyUtility.Check.Seek(roll_cmd, out roll_dr))
                {
                    dr["Roll"] = roll_dr["Roll"];
                    dr["Dyelot"] = roll_dr["Dyelot"];
                    dr.EndEdit();
                }
                else
                {
                    dr["Roll"] = string.Empty;
                    dr["Dyelot"] = string.Empty;
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
                DataRow dr = this.grid.GetDataRow(e.RowIndex);
                string oldvalue = dr["averageWeightM2"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (this.EditMode == false)
                {
                    return;
                }

                if (oldvalue == newvalue)
                {
                    return;
                }

                decimal m2 = MyUtility.Convert.GetDecimal(dr["WeightM2"]);
                decimal avgM2 = MyUtility.Convert.GetDecimal(e.FormattedValue);
                decimal diff = m2 == 0 ? 0 : Math.Round(((avgM2 - m2) / m2) * 100, 2);
                dr["averageWeightM2"] = avgM2;
                dr["Difference"] = diff;
                dr.EndEdit();
            };
            #endregion

            this.Helper.Controls.Grid.Generator(this.grid)
            .Date("SubmitDate", header: "Submit Date", width: Widths.AnsiChars(10))
            .Text("Roll", header: "Roll", width: Widths.AnsiChars(8), settings: rollcell)
            .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Numeric("WeightM2", header: "DeclaredMass", width: Widths.AnsiChars(7), integer_places: 4, decimal_places: 1, iseditingreadonly: true)
            .Numeric("averageWeightM2", header: "Average Mass", width: Widths.AnsiChars(7), integer_places: 4, decimal_places: 1, settings: averageWeightM2cell)
            .Numeric("Difference", header: "Diff%", width: Widths.AnsiChars(7), integer_places: 5, decimal_places: 2, iseditingreadonly: true)
            .Text("Result", header: "Result", width: Widths.AnsiChars(5), iseditingreadonly: true, settings: resulCell)
            .Date("InspDate", header: "Insp.Date", width: Widths.AnsiChars(10))
            .CellUser("Inspector", header: "Inspector", width: Widths.AnsiChars(10), userNamePropertyName: "Name")
            .Text("Name", header: "Name", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .Text("Remark", header: "Remark", width: Widths.AnsiChars(20));

            this.grid.Columns["Roll"].DefaultCellStyle.BackColor = Color.MistyRose;
            this.grid.Columns["averageWeightM2"].DefaultCellStyle.BackColor = Color.MistyRose;
            this.grid.Columns["Result"].DefaultCellStyle.BackColor = Color.MistyRose;
            this.grid.Columns["Result"].DefaultCellStyle.ForeColor = Color.Red;

            this.grid.Columns["InspDate"].DefaultCellStyle.BackColor = Color.MistyRose;
            this.grid.Columns["Inspector"].DefaultCellStyle.BackColor = Color.MistyRose;
            this.grid.Columns["Remark"].DefaultCellStyle.BackColor = Color.MistyRose;
            return true;
        }

        /// <inheritdoc/>
        protected override void OnInsert()
        {
            DataTable dt = (DataTable)this.gridbs.DataSource;
            base.OnInsert();

            DataRow selectDr = ((DataRowView)this.grid.GetSelecteds(SelectedSort.Index)[0]).Row;

            selectDr["Result"] = string.Empty;
            selectDr["Inspdate"] = DateTime.Now.ToShortDateString();
            selectDr["Inspector"] = this.loginID;
            selectDr["Name"] = MyUtility.GetValue.Lookup("Name", this.loginID, "Pass1", "ID");
            selectDr["poid"] = this.maindr["poid"];
            selectDr["SEQ1"] = this.maindr["SEQ1"];
            selectDr["SEQ2"] = this.maindr["SEQ2"];
            selectDr["WeightM2"] = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup("WeightM2", this.maindr["SciRefno"].ToString(), "Fabric", "SciRefno"));
            selectDr["difference"] = 0;
        }

        /// <inheritdoc/>
        protected override bool OnSaveBefore()
        {
            DataTable gridTb = (DataTable)this.gridbs.DataSource;
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

        private void BtnEncode_Click(object sender, EventArgs e)
        {
            string updatesql = string.Empty;
            if (MyUtility.Check.Empty(this.CurrentData) && this.btnEncode.Text == "Encode")
            {
                MyUtility.Msg.WarningBox("Data not found! ");
                return;
            }

            if (!MyUtility.Convert.GetBool(this.maindr["WeightEncode"]))
            {
                // Encode
                if (!MyUtility.Convert.GetBool(this.maindr["nonWeight"]))
                {
                    // 只要沒勾選就要判斷，有勾選就可直接Encode
                    if (MyUtility.GetValue.Lookup("WeaveTypeID", this.maindr["SCIRefno"].ToString(), "Fabric", "SciRefno") == "KNIT")
                    {
                        // 當Fabric.WeaveTypdID = 'Knit' 時必須每ㄧ缸都要有檢驗
                        DataTable dyeDt;
                        string cmd = string.Format(
                        @"
Select distinct dyelot from Receiving_Detail a WITH (NOLOCK) where 
a.id='{0}' and a.poid='{2}' and a.seq1 ='{3}' and a.seq2='{4}'  
and not exists 
(Select distinct dyelot from FIR_Weight b WITH (NOLOCK) where b.id={1} and a.dyelot = b.dyelot)
union
Select distinct dyelot from TransferIn_Detail a WITH (NOLOCK) where 
a.id='{0}' and a.poid='{2}' and a.seq1 ='{3}' and a.seq2='{4}'  
and not exists 
(Select distinct dyelot from FIR_Weight b WITH (NOLOCK) where b.id={1} and a.dyelot = b.dyelot)
",
                        this.maindr["receivingid"], this.maindr["id"], this.maindr["POID"], this.maindr["seq1"], this.maindr["seq2"]);
                        DualResult dResult = DBProxy.Current.Select(null, cmd, out dyeDt);
                        if (dResult)
                        {
                            if (dyeDt.Rows.Count > 0)
                            {
                                string dye = string.Empty;
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

                DataTable gridTb = (DataTable)this.gridbs.DataSource;
                DataRow[] resultAry = gridTb.Select("Result = 'Fail'");
                string result = "Pass";
                if (resultAry.Length > 0)
                {
                    result = "Fail";
                }
                #region  寫入虛擬欄位
                this.maindr["Weight"] = result;
                this.maindr["WeightDate"] = DateTime.Now.ToShortDateString();
                this.maindr["WeightEncode"] = true;
                this.maindr["EditName"] = this.loginID;
                this.maindr["EditDate"] = DateTime.Now.ToShortDateString();
                this.maindr["WeightInspector"] = this.loginID;
                #endregion
                #region 判斷Result 是否要寫入
                string[] returnstr = Prgs.GetOverallResult_Status(this.maindr);
                #endregion
                #region  寫入實體Table
                updatesql = string.Format(
                @"Update Fir set WeightDate = GetDate(),WeightEncode=1,EditName='{0}',EditDate = GetDate(),Weight = '{1}',Result ='{2}',Status='{4}',WeightInspector = '{0}' where id ={3}", this.loginID, result, returnstr[0], this.maindr["ID"], returnstr[1]);
                #endregion
                #region Excel Email 需寄給Encoder的Teamleader 與 Supervisor*****
                DataTable dt_Leader;
                string cmd_leader = string.Format(
                    @"
select ToAddress = stuff ((select concat (';', tmp.email)
						  from (
							  select distinct email from pass1
							  where id in (select Supervisor from pass1 where  id='{0}')
							         or id in (select Manager from Pass1 where id = '{0}')
						  ) tmp
						  for xml path('')
						 ), 1, 1, '')", Env.User.UserID);
                DBProxy.Current.Select(string.Empty, cmd_leader, out dt_Leader);
                if (!MyUtility.Check.Empty(dt_Leader)
                    && dt_Leader.Rows.Count > 0)
                {
                    string mailto = dt_Leader.Rows[0]["ToAddress"].ToString();
                    string ccAddress = Env.User.MailAddress;
                    string subject = string.Format(MyUtility.GetValue.Lookup("Subject", "007", "MailTo", "ID"), this.displaySP.Text, this.displayRefno.Text, this.displayColor.Text);
                    string content = string.Format(MyUtility.GetValue.Lookup("content", "007", "MailTo", "ID"), this.displaySP.Text, this.displayRefno.Text, this.displayColor.Text)
                                     + Environment.NewLine
                                     + "Please Approve and Check Fabric Inspection";
                    this.ToExcel(true);
                    var email = new MailTo(Env.Cfg.MailFrom, mailto, ccAddress, subject, this.excelFile, content, false, true);
                    email.ShowDialog(this);
                }
                #endregion

                this.maindr["Result"] = returnstr[0];
                this.maindr["Status"] = returnstr[1];
            }
            else
            {
                // Amend
                #region  寫入虛擬欄位
                this.maindr["Weight"] = string.Empty;
                this.maindr["WeightDate"] = DBNull.Value;
                this.maindr["WeightEncode"] = false;
                this.maindr["EditName"] = this.loginID;
                this.maindr["EditDate"] = DateTime.Now.ToShortDateString();
                this.maindr["WeightInspector"] = string.Empty;

                // 判斷Result and Status 必須先確認Weight="", 判斷才會正確
                string[] returnstr = Prgs.GetOverallResult_Status(this.maindr);
                this.maindr["Result"] = returnstr[0];
                this.maindr["Status"] = returnstr[1];
                #endregion
                #region  寫入實體Table
                updatesql = string.Format(
                @"Update Fir set WeightDate = null,WeightEncode=0,EditName='{0}',EditDate = GetDate(),Weight = '',Result ='{2}',Status='{3}',WeightInspector = ''  where id ={1}", this.loginID, this.maindr["ID"], returnstr[0], returnstr[1]);
                #endregion
            }

            DualResult upResult;
            TransactionScope transactionscope = new TransactionScope();
            using (transactionscope)
            {
                try
                {
                    if (!(upResult = DBProxy.Current.Execute(null, updatesql)))
                    {
                        transactionscope.Dispose();
                        return;
                    }

                    if (!(upResult = DBProxy.Current.Execute(null, $"exec UpdateInspPercent 'FIR','{this.maindr["POID"].ToString()}'; ")))
                    {
                        transactionscope.Dispose();
                        return;
                    }

                    transactionscope.Complete();
                    transactionscope.Dispose();
                    MyUtility.Msg.InfoBox("Successfully");
                }
                catch (Exception ex)
                {
                    transactionscope.Dispose();
                    this.ShowErr("Commit transaction error.", ex);
                    return;
                }
            }

            this.OnRequery();
        }

        private void BtnApprove_Click(object sender, EventArgs e)
        {
            string updatesql = string.Empty;

            if (this.maindr["Status"].ToString() == "Confirmed")
            {
                this.maindr["Status"] = "Approved";
                this.maindr["Approve"] = this.loginID;
                this.maindr["ApproveDate"] = DateTime.Now.ToShortDateString();
                this.maindr["EditName"] = this.loginID;
                this.maindr["EditDate"] = DateTime.Now;
                #region  寫入實體Table
                updatesql = string.Format(
                @"Update Fir set Status = 'Approved',Approve='{0}',EditName='{0}',EditDate = GetDate(),ApproveDate = GetDate() where id ={1}", this.loginID, this.maindr["ID"]);
                #endregion
            }
            else
            {
                this.maindr["Status"] = "Confirmed";
                this.maindr["Approve"] = string.Empty;
                this.maindr["ApproveDate"] = DBNull.Value;
                this.maindr["EditName"] = this.loginID;
                this.maindr["EditDate"] = DateTime.Now;
                #region  寫入實體Table
                updatesql = string.Format(
                @"Update Fir set Status = 'Confirmed',Approve='',EditName='{0}',EditDate = GetDate(),ApproveDate = null where id ={1}", this.loginID, this.maindr["ID"]);
                #endregion
            }

            DualResult upResult;
            TransactionScope transactionscope = new TransactionScope();
            using (transactionscope)
            {
                try
                {
                    if (!(upResult = DBProxy.Current.Execute(null, updatesql)))
                    {
                        transactionscope.Dispose();
                        return;
                    }

                    transactionscope.Complete();
                    transactionscope.Dispose();
                    MyUtility.Msg.InfoBox("Successfully");
                }
                catch (Exception ex)
                {
                    transactionscope.Dispose();
                    this.ShowErr("Commit transaction error.", ex);
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

                this.ToExcel(true);
                var email = new MailTo(Env.Cfg.MailFrom, mailto, mailCC, subject, this.excelFile, content, false, true);
                email.ShowDialog(this);
            }
            #endregion
            this.OnRequery();
        }

        private void Button_enable()
        {
            if (this.maindr == null)
            {
                return;
            }

            this.btnEncode.Enabled = this.CanEdit && !this.EditMode && this.maindr["Status"].ToString() != "Approved";
            this.btnToExcel.Enabled = !this.EditMode;
            string menupk = MyUtility.GetValue.Lookup("Pkey", "Sci.Production.Quality.P01", "MenuDetail", "FormName");
            string pass0pk = MyUtility.GetValue.Lookup("FKPass0", this.loginID, "Pass1", "ID");
            DataRow pass2_dr;
            string pass2_cmd = string.Format("Select * from Pass2 WITH (NOLOCK) Where FKPass0 ='{0}' and FKMenu='{1}'", pass0pk, menupk);
            int lApprove = 0; // 有Confirm權限皆可按Pass的Approve, 有Check權限才可按Fail的Approve(TeamLeader 有Approve權限,Supervisor有Check)
            int lCheck = 0;
            if (MyUtility.Check.Seek(pass2_cmd, out pass2_dr))
            {
                lApprove = pass2_dr["CanConfirm"].ToString() == "True" ? 1 : 0;
                lCheck = pass2_dr["CanCheck"].ToString() == "True" ? 1 : 0;
            }

            if (this.maindr["Result"].ToString() == "Pass")
            {
                this.btnApprove.Enabled = this.CanEdit && !this.EditMode && lApprove == 1 && !MyUtility.Check.Empty(this.maindr["Result"]) && !MyUtility.Check.Empty(this.maindr["Weight"]);
            }
            else
            {
                this.btnApprove.Enabled = this.CanEdit && !this.EditMode && lCheck == 1 && !MyUtility.Check.Empty(this.maindr["Result"]) && !MyUtility.Check.Empty(this.maindr["Weight"]);
            }
        }

        private void BtnToExcel_Click(object sender, EventArgs e)
        {
            this.ToExcel(false);
        }

        private bool ToExcel(bool isSendMail)
        {
            #region Excel Grid Value
            DataTable dt;
            DualResult xresult;
            if (xresult = DBProxy.Current.Select("Production", string.Format("select Roll,Dyelot,WeightM2,averageWeightM2,Difference,Result,Inspdate,Inspector,Remark from FIR_Weight WITH (NOLOCK) where id='{0}'", this.textID.Text), out dt))
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
            string seasonID = string.Empty;
            string continuityEncode = string.Empty;
            DualResult xresult1;
            string cmd = $@"select Roll,Dyelot,WeightM2,averageWeightM2,Difference,A.Result,A.Inspdate,Inspector,B.ContinuityEncode,C.SeasonID from FIR_Weight a WITH (NOLOCK) left join FIR b WITH (NOLOCK) on a.ID=b.ID LEFT JOIN ORDERS C ON B.POID=C.ID where a.ID='{this.textID.Text}'";
            xresult1 = DBProxy.Current.Select("Production", cmd, out dt1);

            if (xresult1)
            {
                if (dt1.Rows.Count > 0)
                {
                    seasonID = dt1.Rows[0]["SeasonID"].ToString();
                    continuityEncode = dt1.Rows[0]["ContinuityEncode"].ToString();
                }
            }
            #endregion
            Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Quality_P01_Weight_Report.xltx"); // 預先開啟excel app
            objApp.Visible = false;
            MyUtility.Excel.CopyToXls(dt, string.Empty, "Quality_P01_Weight_Report.xltx", 5, false, null, objApp);      // 將datatable copy to excel
            Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
            objSheets.Cells[2, 2] = this.displaySP.Text.ToString();
            objSheets.Cells[2, 4] = this.displaySEQ.Text.ToString();
            objSheets.Cells[2, 6] = this.displayColor.Text.ToString();
            objSheets.Cells[2, 8] = this.displayStyle.Text.ToString();
            objSheets.Cells[2, 10] = seasonID;
            objSheets.Cells[3, 2] = this.displaySCIRefno.Text.ToString();
            objSheets.Cells[3, 4] = continuityEncode;
            objSheets.Cells[3, 6] = this.displayResult.Text.ToString();
            objSheets.Cells[3, 8] = this.dateLastInspectionDate.Value;
            objSheets.Cells[3, 10] = this.displayBrand.Text.ToString();
            objSheets.Cells[4, 2] = this.displayRefno.Text.ToString();
            objSheets.Cells[4, 4] = this.displayArriveQty.Text.ToString();
            objSheets.Cells[4, 6] = this.dateArriveWHDate.Value;
            objSheets.Cells[4, 8] = this.txtsupplier.DisplayBox1.Text.ToString();
            objSheets.Cells[4, 10] = this.displayWKNo.Text.ToString();

            objApp.Cells.EntireColumn.AutoFit();    // 自動欄寬
            objApp.Cells.EntireRow.AutoFit();       ////自動欄高

            #region Save Excel
            this.excelFile = Class.MicrosoftFile.GetName("QA_P01_Weight");
            objApp.ActiveWorkbook.SaveAs(this.excelFile);
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);
            Marshal.ReleaseComObject(objSheets);
            #endregion

            if (!isSendMail)
            {
                this.excelFile.OpenFile();
            }

            return true;
        }

        private void ButtonToPDF_Click(object sender, EventArgs e)
        {
            DataTable dt;
            DualResult xresult;
            if (xresult = DBProxy.Current.Select("Production", string.Format("select Roll,Dyelot,WeightM2,averageWeightM2,Difference,Result,Inspdate,Inspector,Remark from FIR_Weight WITH (NOLOCK) where id='{0}'", this.textID.Text), out dt))
            {
                if (dt.Rows.Count <= 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!");
                    return;
                }
            }

            this.ShowWaitMessage("To PDF Processing...");
            Excel.Application objApp = new Excel.Application
            {
                DisplayAlerts = false,
            };
            objApp.Workbooks.Add();
            Excel.Sheets exlSheets = objApp.Worksheets;
            Excel.Worksheet newSheet = exlSheets.Item[1];
            exlSheets.Item[1].Delete();
            exlSheets.Item[1].Delete();

            ExcelHeadData excelHeadData = new ExcelHeadData
            {
                SPNo = this.maindr["POID"].ToString(),
                Brand = this.displayBrand.Text,
                StyleNo = this.displayStyle.Text,
            };

            DataRow drOrder;
            MyUtility.Check.Seek(
                $@"select o.CustPONo,s.StyleName from dbo.orders o WITH (NOLOCK) 
left join dbo.style s on o.StyleUkey = s.ukey where o.ID = '{this.maindr["POID"].ToString()}'", out drOrder);
            excelHeadData.PONumber = drOrder["CustPONo"].ToString();
            excelHeadData.StyleName = drOrder["StyleName"].ToString();
            excelHeadData.ArriveQty = this.maindr["arriveQty"].ToString();
            excelHeadData.FabricRefNo = this.displayRefno.Text;
            excelHeadData.FabricColor = this.displayColor.Text;
            excelHeadData.FabricDesc = MyUtility.GetValue.Lookup("Description", this.maindr["SciRefno"].ToString(), "Fabric", "SCIRefno");

            // 取得資料
            // 先取Article
            string article_sql = $@"SELECT distinct(oc.article)  as article
FROM [Order_BOF]  bof
inner join PO_Supp_Detail p on p.id=bof.id and bof.SCIRefno=p.SCIRefno
inner join Order_ColorCombo OC on oc.id=p.id and oc.FabricCode=bof.FabricCode
where bof.id='{this.maindr["POID"].ToString()}' and p.seq1='{this.maindr["Seq1"].ToString()}' and p.seq2='{this.maindr["Seq2"].ToString()}'
 ";
            DataTable dt_article;
            DualResult result = DBProxy.Current.Select(null, article_sql, out dt_article);
            if (!result)
            {
                MyUtility.Msg.ErrorBox(result.Messages.ToString());
                return;
            }

            // 取得Header資料

            // 產生excel
            using (dt_article)
            {
                if (dt_article.Rows.Count != 0)
                {
                    // 若有Article，PDF則分若有Article顯示，沒有的話則空白
                    foreach (DataRow articleDr in dt_article.Rows)
                    {
                        excelHeadData.ArticleNo = articleDr["article"].ToString();
                        var detailbyDate = from r1 in this.Datas.AsEnumerable()
                                      group r1 by new
                                      {
                                          SubmitDate = r1["SubmitDate"],
                                          Inspdate = r1["Inspdate"],
                                      }
                                        into g
                                      select new
                                      {
                                          SubmitDate = g.Key.SubmitDate,
                                          Inspdate = g.Key.Inspdate,
                                      };

                        foreach (var dateFilter in detailbyDate)
                        {
                            excelHeadData.SubmitDate = dateFilter.SubmitDate.Equals(DBNull.Value) ? string.Empty : ((DateTime)dateFilter.SubmitDate).ToOADate().ToString();
                            excelHeadData.ReportDate = dateFilter.Inspdate.Equals(DBNull.Value) ? string.Empty : ((DateTime)dateFilter.Inspdate).ToOADate().ToString();

                            // 產生新sheet並填入標題等資料
                            exlSheets.Add();

                            newSheet = exlSheets.Item[1];
                            this.AddSheetAndHead(excelHeadData, newSheet);

                            // 填入detail資料
                            DataRow[] dr_detail = this.Datas.Where(s => s["SubmitDate"].Equals(dateFilter.SubmitDate) && s["Inspdate"].Equals(dateFilter.Inspdate)).ToArray();
                            string signature = dr_detail.GroupBy(s => s["Inspector"]).Select(s => MyUtility.GetValue.Lookup("Name", s.Key.ToString(), "Pass1", "ID")).JoinToString(",");
                            int detail_start = 16;
                            foreach (DataRow dr in dr_detail)
                            {
                                newSheet.Cells[detail_start, 1].Value = "'" + dr["Dyelot"];
                                newSheet.Cells[detail_start, 2].Value = "'" + dr["Roll"];
                                newSheet.Cells[detail_start, 3].Value = dr["WeightM2"];
                                newSheet.Cells[detail_start, 4].Value = dr["AverageWeightM2"];
                                newSheet.Cells[detail_start, 5].Value = dr["Difference"];
                                newSheet.Cells[detail_start, 6].Value = dr["Result"];
                                newSheet.Cells[detail_start, 7].Value = dr["Remark"];
                                newSheet.Range[$"G{detail_start}", $"I{detail_start}"].Merge();
                                detail_start++;
                            }

                            newSheet.get_Range($"A16:I{detail_start - 1}").Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                            newSheet.get_Range($"A16:I{detail_start - 1}").Borders.Weight = Excel.XlBorderWeight.xlMedium;

                            // 簽名欄
                            newSheet.Cells[detail_start + 1, 6].Value = "Signature";
                            newSheet.Cells[detail_start + 1, 6].Font.Bold = true;
                            newSheet.Range[$"F{detail_start + 1}", $"I{detail_start + 1}"].Merge();
                            newSheet.Cells[detail_start + 2, 6].Value = signature;
                            newSheet.Range[$"F{detail_start + 2}", $"I{detail_start + 2}"].Merge();
                            newSheet.Cells[detail_start + 3, 6].Value = "Checked by:";
                            newSheet.Range[$"F{detail_start + 3}", $"I{detail_start + 3}"].Merge();
                            newSheet.get_Range($"F{detail_start + 1}:I{detail_start + 3}").Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                            newSheet.get_Range($"F{detail_start + 1}:I{detail_start + 3}").Borders.Weight = Excel.XlBorderWeight.xlMedium;
                            newSheet.get_Range($"F{detail_start + 1}:I{detail_start + 3}").Font.Size = 9;

                            // 全部置中
                            newSheet.get_Range($"A1:I{detail_start + 3}").HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                            newSheet.get_Range($"A1:I{detail_start + 3}").Font.Name = "Arial";

                            // 版面保持一頁
                            newSheet.PageSetup.Zoom = false;
                            newSheet.PageSetup.FitToPagesWide = 1;

                            // 針對merge後會過長的欄位作高度調整
                            double newHeight;
                            newSheet.Range["A7", "A7"].RowHeight = this.MeasureTextHeight(excelHeadData.StyleName, 54);
                            newHeight = this.MeasureTextHeight(excelHeadData.FabricDesc, 59);
                            newSheet.Range["A12", "A12"].RowHeight = newSheet.Rows[12].Height > newHeight ? newSheet.Rows[12].Height : newHeight;
                            newHeight = this.MeasureTextHeight(excelHeadData.FabricRefNo, 13);
                            newSheet.Range["A12", "A12"].RowHeight = newSheet.Rows[12].Height > newHeight ? newSheet.Rows[12].Height : newHeight;
                            newSheet.PageSetup.PrintArea = $"A1:I{detail_start + 3}";
                        }
                    }
                }
                else
                {
                    excelHeadData.ArticleNo = string.Empty;
                    var detailbyDate = from r1 in this.Datas.AsEnumerable()
                                       group r1 by new
                                       {
                                           SubmitDate = r1["SubmitDate"],
                                           Inspdate = r1["Inspdate"],
                                       }
                                        into g
                                       select new
                                       {
                                           SubmitDate = g.Key.SubmitDate,
                                           Inspdate = g.Key.Inspdate,
                                       };

                    foreach (var dateFilter in detailbyDate)
                    {
                        excelHeadData.SubmitDate = dateFilter.SubmitDate.Equals(DBNull.Value) ? string.Empty : ((DateTime)dateFilter.SubmitDate).ToOADate().ToString();
                        excelHeadData.ReportDate = dateFilter.Inspdate.Equals(DBNull.Value) ? string.Empty : ((DateTime)dateFilter.Inspdate).ToOADate().ToString();

                        // 產生新sheet並填入標題等資料
                        exlSheets.Add();

                        newSheet = exlSheets.Item[1];
                        this.AddSheetAndHead(excelHeadData, newSheet);

                        // 填入detail資料
                        DataRow[] dr_detail = this.Datas.Where(s => s["SubmitDate"].Equals(dateFilter.SubmitDate) && s["Inspdate"].Equals(dateFilter.Inspdate)).ToArray();
                        string signature = dr_detail.GroupBy(s => s["Inspector"]).Select(s => MyUtility.GetValue.Lookup("Name", s.Key.ToString(), "Pass1", "ID")).JoinToString(",");
                        int detail_start = 16;
                        foreach (DataRow dr in dr_detail)
                        {
                            newSheet.Cells[detail_start, 1].Value = "'" + dr["Dyelot"];
                            newSheet.Cells[detail_start, 2].Value = "'" + dr["Roll"];
                            newSheet.Cells[detail_start, 3].Value = dr["WeightM2"];
                            newSheet.Cells[detail_start, 4].Value = dr["AverageWeightM2"];
                            newSheet.Cells[detail_start, 5].Value = dr["Difference"];
                            newSheet.Cells[detail_start, 6].Value = dr["Result"];
                            newSheet.Cells[detail_start, 7].Value = dr["Remark"];
                            newSheet.Range[$"G{detail_start}", $"I{detail_start}"].Merge();
                            detail_start++;
                        }

                        newSheet.get_Range($"A16:I{detail_start - 1}").Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                        newSheet.get_Range($"A16:I{detail_start - 1}").Borders.Weight = Excel.XlBorderWeight.xlMedium;

                        // 簽名欄
                        newSheet.Cells[detail_start + 1, 6].Value = "Signature";
                        newSheet.Cells[detail_start + 1, 6].Font.Bold = true;
                        newSheet.Range[$"F{detail_start + 1}", $"I{detail_start + 1}"].Merge();
                        newSheet.Cells[detail_start + 2, 6].Value = signature;
                        newSheet.Range[$"F{detail_start + 2}", $"I{detail_start + 2}"].Merge();
                        newSheet.Cells[detail_start + 3, 6].Value = "Checked by:";
                        newSheet.Range[$"F{detail_start + 3}", $"I{detail_start + 3}"].Merge();
                        newSheet.get_Range($"F{detail_start + 1}:I{detail_start + 3}").Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                        newSheet.get_Range($"F{detail_start + 1}:I{detail_start + 3}").Borders.Weight = Excel.XlBorderWeight.xlMedium;
                        newSheet.get_Range($"F{detail_start + 1}:I{detail_start + 3}").Font.Size = 9;

                        // 全部置中
                        newSheet.get_Range($"A1:I{detail_start + 3}").HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                        newSheet.get_Range($"A1:I{detail_start + 3}").Font.Name = "Arial";

                        // 版面保持一頁
                        newSheet.PageSetup.Zoom = false;
                        newSheet.PageSetup.FitToPagesWide = 1;

                        // 針對merge後會過長的欄位作高度調整
                        double newHeight;
                        newSheet.Range["A7", "A7"].RowHeight = this.MeasureTextHeight(excelHeadData.StyleName, 54);
                        newHeight = this.MeasureTextHeight(excelHeadData.FabricDesc, 59);
                        newSheet.Range["A12", "A12"].RowHeight = newSheet.Rows[12].Height > newHeight ? newSheet.Rows[12].Height : newHeight;
                        newHeight = this.MeasureTextHeight(excelHeadData.FabricRefNo, 13);
                        newSheet.Range["A12", "A12"].RowHeight = newSheet.Rows[12].Height > newHeight ? newSheet.Rows[12].Height : newHeight;
                        newSheet.PageSetup.PrintArea = $"A1:I{detail_start + 3}";
                    }
                }
            }

            string strExcelName = Class.MicrosoftFile.GetName("QA_P01_Weight");
            objApp.ActiveWorkbook.SaveAs(strExcelName);
            objApp.ActiveWorkbook.Close(true, Type.Missing, Type.Missing);
            objApp.Quit();
            Marshal.ReleaseComObject(exlSheets);
            Marshal.ReleaseComObject(objApp);

            string strPDFFileName = Class.MicrosoftFile.GetName("QA_P01_Weight", Class.PDFFileNameExtension.PDF);
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
            // 設定欄位寬度
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
            if (string.IsNullOrEmpty(text))
            {
                return 0.0;
            }

            var bitmap = new Bitmap(1, 1);
            var graphics = Graphics.FromImage(bitmap);

            var pixelWidth = Convert.ToInt32(width * 7.5);  // 7.5 pixels per excel column width
            var drawingFont = new Font("Arial", 11);
            var size = graphics.MeasureString(text, drawingFont, pixelWidth);

            // 72 DPI and 96 points per inch.  Excel height in points with max of 409 per Excel requirements.
            return Math.Min(Convert.ToDouble(size.Height) * 72 / 96, 409);
        }

        private class ExcelHeadData
        {
            /// <inheritdoc/>
            public string SubmitDate;

            /// <inheritdoc/>
            public string ReportDate;

            /// <inheritdoc/>
            public string SPNo;

            /// <inheritdoc/>
            public string Brand;

            /// <inheritdoc/>
            public string StyleNo;

            /// <inheritdoc/>
            public string PONumber;

            /// <inheritdoc/>
            public string ArticleNo;

            /// <inheritdoc/>
            public string StyleName;

            /// <inheritdoc/>
            public string ArriveQty;

            /// <inheritdoc/>
            public string FabricRefNo;

            /// <inheritdoc/>
            public string FabricColor;

            /// <inheritdoc/>
            public string FabricDesc;
        }
    }
}
