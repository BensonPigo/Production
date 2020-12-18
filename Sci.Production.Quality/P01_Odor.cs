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

namespace Sci.Production.Quality
{
    /// <inheritdoc/>
    public partial class P01_Odor : Win.Subs.Input4
    {
        private readonly DataRow maindr;
        private readonly string loginID = Env.User.UserID;
        private readonly string keyWord = Env.User.Keyword;
        private string excelFile;

        /// <inheritdoc/>
        public P01_Odor(bool canedit, string keyvalue1, string keyvalue2, string keyvalue3, DataRow mainDr)
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
            this.btnEncode.Text = MyUtility.Convert.GetBool(this.maindr["OdorEncode"]) ? "Amend" : "Encode";
            this.btnApprove.Text = this.maindr["Status"].ToString() == "Approved" ? "Unapprove" : "Approve";
            #endregion

            this.save.Enabled = !MyUtility.Convert.GetBool(this.maindr["OdorEncode"]);

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

            string receiving_cmd = string.Format("select a.exportid,a.WhseArrival ,b.Refno from Receiving a WITH (NOLOCK) inner join FIR b WITH (NOLOCK) on a.Id=b.Receivingid where b.id='{0}'", this.maindr["id"]);
            DataRow rec_dr;
            if (MyUtility.Check.Seek(receiving_cmd, out rec_dr))
            {
                this.displayWKNo.Text = rec_dr["exportid"].ToString();
                this.displayRefno.Text = rec_dr["Refno"].ToString();
            }
            else
            {
                this.displayWKNo.Text = string.Empty;
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
            this.dateLastInspectionDate.Value = MyUtility.Convert.GetDate(this.maindr["OdorDate"]);
            this.dateArriveWHDate.Value = MyUtility.Convert.GetDate(this.maindr["WhseArrival"]);
            this.displaySCIRefno1.Text = MyUtility.GetValue.Lookup("Description", this.maindr["SciRefno"].ToString(), "Fabric", "SCIRefno");
            this.displaySEQ.Text = this.maindr["Seq1"].ToString() + "-" + this.maindr["Seq2"].ToString();
            this.displaySP.Text = this.maindr["POID"].ToString();
            this.checkNonOdor.Value = this.maindr["nonOdor"].ToString();
            this.displayResult.Text = this.maindr["Odor"].ToString();
            this.txtuserApprover.TextBox1.Text = this.maindr["Approve"].ToString();
            this.txtOdorInspector.Text = this.maindr["OdorInspector"].ToString();
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
            DataGridViewGeneratorTextColumnSettings resultcell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings resulCell = PublicPrg.Prgs.CellResult.GetGridCell();

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

                // 手動輸入,oldvalue <> newvalue,就不會return並且繼續判斷
                if (oldvalue == newvalue)
                {
                    return;
                }

                string roll_cmd = string.Format("Select roll,dyelot,StockQty from Receiving_Detail WITH (NOLOCK) Where id='{0}' and poid ='{1}' and seq1 = '{2}' and seq2 ='{3}' and roll='{4}'", this.maindr["Receivingid"], this.maindr["Poid"], this.maindr["seq1"], this.maindr["seq2"], e.FormattedValue);

                if (!MyUtility.Check.Seek(roll_cmd))
                {
                    roll_cmd = string.Format("Select roll,dyelot,StockQty=qty from TransferIn_Detail WITH (NOLOCK) Where id='{0}' and poid ='{1}' and seq1 = '{2}' and seq2 ='{3}' and roll='{4}'", this.maindr["Receivingid"], this.maindr["Poid"], this.maindr["seq1"], this.maindr["seq2"], e.FormattedValue);
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

            this.Helper.Controls.Grid.Generator(this.grid)
            .Text("Roll", header: "Roll", width: Widths.AnsiChars(8), settings: rollcell)
            .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("Result", header: "Result", width: Widths.AnsiChars(5), iseditingreadonly: true, settings: resulCell)
            .Date("InspDate", header: "Insp.Date", width: Widths.AnsiChars(10))
            .CellUser("Inspector", header: "Inspector", width: Widths.AnsiChars(10), userNamePropertyName: "Name")
            .Text("Name", header: "Name", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .Text("Remark", header: "Remark", width: Widths.AnsiChars(20));

            this.grid.Columns["Roll"].DefaultCellStyle.BackColor = Color.MistyRose;
            this.grid.Columns["Result"].DefaultCellStyle.BackColor = Color.MistyRose;
            this.grid.Columns["Result"].DefaultCellStyle.ForeColor = Color.Red;

            this.grid.Columns["InspDate"].DefaultCellStyle.BackColor = Color.MistyRose;
            this.grid.Columns["Inspector"].DefaultCellStyle.BackColor = Color.MistyRose;
            this.grid.Columns["Name"].DefaultCellStyle.BackColor = Color.MistyRose;
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        private void BtnEncode_Click(object sender, EventArgs e)
        {
            string updatesql = string.Empty;
            if (MyUtility.Check.Empty(this.CurrentData) && this.btnEncode.Text == "Encode")
            {
                MyUtility.Msg.WarningBox("Data not found! ");
                return;
            }

            // Encode
            if (!MyUtility.Convert.GetBool(this.maindr["OdorEncode"]))
            {
                // 只要沒勾選就要判斷，有勾選就可直接Encode
                if (!MyUtility.Convert.GetBool(this.maindr["nonOdor"]))
                {
                    DataTable dyeDt;
                    string cmd = string.Format(
                    @"
Select distinct dyelot from Receiving_Detail a WITH (NOLOCK) where 
a.id='{0}' and a.poid='{2}' and a.seq1 ='{3}' and a.seq2='{4}'  
and not exists 
(Select distinct dyelot from FIR_Odor b WITH (NOLOCK) where b.id={1} and a.dyelot = b.dyelot)
union
Select distinct dyelot from TransferIn_Detail a WITH (NOLOCK) where 
a.id='{0}' and a.poid='{2}' and a.seq1 ='{3}' and a.seq2='{4}'  
and not exists 
(Select distinct dyelot from FIR_Odor b WITH (NOLOCK) where b.id={1} and a.dyelot = b.dyelot)
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

                DataTable gridTb = (DataTable)this.gridbs.DataSource;
                DataRow[] resultAry = gridTb.Select("Result = 'Fail'");
                string result = "Pass";
                if (resultAry.Length > 0)
                {
                    result = "Fail";
                }
                #region  寫入虛擬欄位
                this.maindr["Odor"] = result;
                this.maindr["OdorDate"] = DateTime.Now.ToShortDateString();
                this.maindr["OdorEncode"] = true;
                this.maindr["EditName"] = this.loginID;
                this.maindr["EditDate"] = DateTime.Now.ToShortDateString();
                this.maindr["OdorInspector"] = this.loginID;
                #endregion
                #region 判斷Result 是否要寫入
                string[] returnstr = PublicPrg.Prgs.GetOverallResult_Status(this.maindr);
                #endregion
                #region  寫入實體Table
                updatesql = string.Format(
                @"Update Fir set OdorDate = GetDate(),OdorEncode=1,EditName='{0}',EditDate = GetDate(),Odor = '{1}',Result ='{2}',Status='{4}',OdorInspector='{0}'  where id ={3}", this.loginID, result, returnstr[0], this.maindr["ID"], returnstr[1]);
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
                this.maindr["Odor"] = string.Empty;
                this.maindr["OdorDate"] = DBNull.Value;
                this.maindr["OdorEncode"] = false;
                this.maindr["EditName"] = this.loginID;
                this.maindr["EditDate"] = DateTime.Now.ToShortDateString();
                this.maindr["OdorInspector"] = string.Empty;

                // 判斷Result and Status 必須先確認Odor="",判斷才會正確
                string[] returnstr = PublicPrg.Prgs.GetOverallResult_Status(this.maindr);
                this.maindr["Result"] = returnstr[0];
                this.maindr["Status"] = returnstr[1];
                #endregion

                #region  寫入實體Table
                updatesql = string.Format(
                @"Update Fir set OdorDate = null,OdorEncode=0,EditName='{0}',EditDate = GetDate(),Odor = '',Result ='{2}',Status='{3}',OdorInspector='' where id ={1}", this.loginID, this.maindr["ID"], returnstr[0], returnstr[1]);
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

                    // 更新PO.FIRInspPercent和AIRInspPercent
                    if (!(upResult = DBProxy.Current.Execute(null, $"exec UpdateInspPercent 'FIR','{this.maindr["POID"].ToString()}'; ")))
                    {
                        transactionscope.Dispose();
                        return;
                    }

                    transactionscope.Complete();
                    transactionscope.Dispose();
                    MyUtility.Msg.WarningBox("Successfully");
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
                this.btnApprove.Enabled = this.CanEdit && !this.EditMode && lApprove == 1 && !MyUtility.Check.Empty(this.maindr["Result"]) && !MyUtility.Check.Empty(this.maindr["Odor"]);
            }
            else
            {
                this.btnApprove.Enabled = this.CanEdit && !this.EditMode && lCheck == 1 && !MyUtility.Check.Empty(this.maindr["Result"]) && !MyUtility.Check.Empty(this.maindr["Odor"]);
            }
        }

        private void BtnToExcel_Click(object sender, EventArgs e)
        {
            this.ToExcel(false);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        private bool ToExcel(bool isSendMail)
        {
            DataTable dt;
            DualResult xresult;
            if (xresult = DBProxy.Current.Select("Production", string.Format("select Roll,Dyelot,Result,Inspdate,Inspector,Remark from FIR_Odor WITH (NOLOCK) where id='{0}'", this.textID.Text), out dt))
            {
                if (dt.Rows.Count <= 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!");
                    return false;
                }
            }
            #region Excel 表頭值
            DataTable dt1;
            DualResult xresult1;
            string odorEncode = string.Empty;
            string seasonID = string.Empty;
            if (xresult1 = DBProxy.Current.Select("Production", string.Format(
            "select Roll,Dyelot,a.Result,a.Inspdate,Inspector,a.Remark,B.OdorEncode,C.SeasonID from FIR_Odor a WITH (NOLOCK) left join FIR b WITH (NOLOCK) on a.ID=b.ID LEFT JOIN ORDERS C ON B.POID=C.ID where a.ID='{0}'", this.textID.Text), out dt1))
            {
                if (dt1.Rows.Count > 0)
                {
                    odorEncode = dt1.Rows[0]["OdorEncode"].ToString();
                    seasonID = dt1.Rows[0]["SeasonID"].ToString();
                }
            }
            #endregion
            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Quality_P01_Odor_Report.xltx"); // 預先開啟excel app
            objApp.Visible = false;

            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
            Microsoft.Office.Interop.Excel.Range rngToCopy = objSheets.get_Range("A6:A6").EntireRow;
            for (int i = 0; i < dt1.Rows.Count - 1; i++)
            {
                Microsoft.Office.Interop.Excel.Range rngToInsert = objSheets.get_Range("A6", Type.Missing).EntireRow;
                rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown, rngToCopy.Copy(Type.Missing));
                Marshal.ReleaseComObject(rngToInsert);
            }

            MyUtility.Excel.CopyToXls(dt, string.Empty, "Quality_P01_Odor_Report.xltx", 5, false, null, objApp);      // 將datatable copy to excel
            objSheets.Cells[2, 2] = this.displaySP.Text.ToString();
            objSheets.Cells[2, 4] = this.displaySEQ.Text.ToString();
            objSheets.Cells[2, 6] = this.displayColor.Text.ToString();
            objSheets.Cells[2, 8] = this.displayStyle.Text.ToString();
            objSheets.Cells[2, 10] = seasonID;
            objSheets.Cells[3, 2] = this.displaySCIRefno.Text.ToString();
            objSheets.Cells[3, 4] = odorEncode;
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
            this.excelFile = Class.MicrosoftFile.GetName("Quality_P01_Odor_Report");
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
    }
}
