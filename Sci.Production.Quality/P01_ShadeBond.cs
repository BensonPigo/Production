using Ict;
using Ict.Win;
using Sci.Production.Class;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Sci.Win;
using Sci.Data;
using System.Transactions;
using Sci.Win.Tools;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Data.SqlClient;
using System.Linq;

namespace Sci.Production.Quality
{
    public partial class P01_ShadeBond : Win.Subs.Input4
    {
        private DataRow maindr;
        private string loginID = Sci.Env.User.UserID;
        private string keyWord = Sci.Env.User.Keyword;
        string excelFile;
        string ID;

        public P01_ShadeBond(bool canedit, string keyvalue1, string keyvalue2, string keyvalue3, DataRow mainDr)
            : base(canedit, keyvalue1, keyvalue2, keyvalue3)
        {
            this.InitializeComponent();
            this.maindr = mainDr;
            this.ID = keyvalue1;
        }

        protected override void OnEditModeChanged()
        {
            base.OnEditModeChanged();
            this.button_enable();
        }

        protected override DualResult OnRequery()
        {
            #region Encode/Approve Enable
            this.button_enable();
            this.btnEncode.Text = MyUtility.Convert.GetBool(this.maindr["shadebondEncode"]) ? "Amend" : "Encode";
            this.btnApprove.Text = this.maindr["Status"].ToString() == "Approved" ? "Unapprove" : "Approve";
            #endregion

            this.save.Enabled = !MyUtility.Convert.GetBool(this.maindr["shadebondEncode"]);

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

            string Receiving_cmd = string.Format("select b.Refno from Receiving a WITH (NOLOCK) inner join FIR b WITH (NOLOCK) on a.Id=b.Receivingid where b.id='{0}'", this.maindr["id"]);
            DataRow rec_dr;
            if (MyUtility.Check.Seek(Receiving_cmd, out rec_dr))
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

            this.displayApprover.Text = this.maindr["ApproveDate"].ToString();
            this.displayArriveQty.Text = this.maindr["arriveQty"].ToString();
            this.dateArriveWHDate.Value = MyUtility.Convert.GetDate(this.maindr["whseArrival"]);
            this.dateLastInspectionDate.Value = MyUtility.Convert.GetDate(this.maindr["ShadeBondDate"]);
            this.displaySCIRefno1.Text = MyUtility.GetValue.Lookup("Description", this.maindr["SciRefno"].ToString(), "Fabric", "SCIRefno");
            this.displaySCIRefno.Text = this.maindr["SciRefno"].ToString();
            this.displaySEQ.Text = this.maindr["Seq1"].ToString() + "-" + this.maindr["Seq2"].ToString();
            this.displaySP.Text = this.maindr["POID"].ToString();
            this.displayWKNo.Text = this.maindr["Exportid"].ToString();
            this.checkNonShadeBond.Value = this.maindr["nonshadebond"].ToString();
            this.displayResult.Text = this.maindr["shadebond"].ToString();
            this.txtuserApprover.TextBox1.Text = this.maindr["Approve"].ToString();
            this.txtShadeboneInspector.Text = this.maindr["ShadeboneInspector"].ToString();
            return base.OnRequery();
        }

        protected override void OnRequeryPost(DataTable datas)
        {
            base.OnRequeryPost(datas);
            datas.Columns.Add("Selected", typeof(bool));
            datas.Columns.Add("Name", typeof(string));
            datas.Columns.Add("POID", typeof(string));
            datas.Columns.Add("SEQ1", typeof(string));
            datas.Columns.Add("SEQ2", typeof(string));

            foreach (DataRow dr in datas.Rows)
            {
                dr["Selected"] = false;
                dr["Name"] = MyUtility.GetValue.Lookup("Name", dr["Inspector"].ToString(), "Pass1", "ID");
                dr["poid"] = this.maindr["poid"];
                dr["SEQ1"] = this.maindr["SEQ1"];
                dr["SEQ2"] = this.maindr["SEQ2"];
            }
        }

        protected override bool OnGridSetup()
        {
            DataGridViewGeneratorTextColumnSettings Rollcell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings Scalecell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings ResulCell = Sci.Production.PublicPrg.Prgs.cellResult.GetGridCell();
            DataGridViewGeneratorTextColumnSettings InspectorCell = new DataGridViewGeneratorTextColumnSettings();

            #region Scale
            Scalecell.CellValidating += (s, e) =>
            {
                DataRow dr = this.grid.GetDataRow(e.RowIndex);
                string oldvalue = dr["Scale"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (!this.EditMode)
                {
                    return; // 非編輯模式
                }

                if (e.RowIndex == -1)
                {
                    return; // 沒東西 return
                }

                if (oldvalue == newvalue)
                {
                    return;
                }
                else
                {
                    if (MyUtility.Check.Empty(newvalue) && MyUtility.Check.Empty(dr["Result"]))
                    {
                        dr["InspDate"] = DBNull.Value;
                        dr["Inspector"] = string.Empty;
                        dr["Name"] = string.Empty;
                    }
                    else
                    {
                        dr["InspDate"] = DateTime.Now;
                        dr["Inspector"] = Sci.Env.User.UserID;
                        dr["Name"] = MyUtility.GetValue.Lookup($"SELECT Name FROM Pass1 WHERE ID='{Sci.Env.User.UserID}'");
                    }

                    dr["Scale"] = newvalue;
                }
            };

            Scalecell.EditingMouseDown += (s, e) =>
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
                    SelectItem item = new SelectItem("select ID from Scale where junk = 0 order by ID", "10,40", dr["Scale"].ToString().Trim());

                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    dr["Scale"] = item.GetSelectedString();
                    dr["InspDate"] = DateTime.Now;
                    dr["Inspector"] = Sci.Env.User.UserID;
                    dr["Name"] = MyUtility.GetValue.Lookup($"SELECT Name FROM Pass1 WHERE ID='{Sci.Env.User.UserID}'");
                }
            };

            #endregion

            #region Result
            ResulCell.CellValidating += (s, e) =>
            {
                DataRow dr = this.grid.GetDataRow(e.RowIndex);
                string oldvalue = dr["Result"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (!this.EditMode)
                {
                    return; // 非編輯模式
                }

                if (e.RowIndex == -1)
                {
                    return; // 沒東西 return
                }

                if (oldvalue == newvalue)
                {
                    return;
                }
                else
                {
                    if (MyUtility.Check.Empty(newvalue) && MyUtility.Check.Empty(dr["Scale"]))
                    {
                        dr["InspDate"] = DBNull.Value;
                        dr["Inspector"] = string.Empty;
                        dr["Name"] = string.Empty;
                    }
                    else
                    {
                        dr["InspDate"] = DateTime.Now;
                        dr["Inspector"] = Sci.Env.User.UserID;
                        dr["Name"] = MyUtility.GetValue.Lookup($"SELECT Name FROM Pass1 WHERE ID='{Sci.Env.User.UserID}'");
                    }

                    dr["Result"] = newvalue;
                }
            };

            #endregion

            #region Inspector

            InspectorCell.CellValidating += (s, e) =>
            {
                DataRow dr = this.grid.GetDataRow(e.RowIndex);
                string oldvalue = dr["Inspector"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (!this.EditMode)
                {
                    return; // 非編輯模式
                }

                if (e.RowIndex == -1)
                {
                    return; // 沒東西 return
                }

                if (MyUtility.Check.Empty(newvalue))
                {
                    dr["Name"] = string.Empty;
                }

                dr["Inspector"] = newvalue;

                // dr["Name"] = MyUtility.GetValue.Lookup($"SELECT Name FROM Pass1 WHERE ID='{newvalue}'");
            };
            #endregion

            this.Helper.Controls.Grid.Generator(this.grid)
            .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(5), trueValue: 1, falseValue: 0, iseditable: true)
            .Text("Roll", header: "Roll", width: Widths.AnsiChars(8), iseditingreadonly: true, settings: Rollcell)
            .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Numeric("Ticketyds", header: "Ticket Yds", width: Widths.AnsiChars(7), integer_places: 10, decimal_places: 2, iseditingreadonly: true)
            .Text("Scale", header: "Scale", width: Widths.AnsiChars(5), settings: Scalecell)
            .Text("Result", header: "Result", width: Widths.AnsiChars(5), iseditingreadonly: true, settings: ResulCell)
            .Date("InspDate", header: "Insp.Date", width: Widths.AnsiChars(10))
            .CellUser("Inspector", header: "Inspector", width: Widths.AnsiChars(10), userNamePropertyName: "Name", settings: InspectorCell)
            .Text("Name", header: "Name", width: Widths.AnsiChars(20))
            .Text("Remark", header: "Remark", width: Widths.AnsiChars(20));

            this.grid.Columns["Roll"].DefaultCellStyle.BackColor = Color.White;
            this.grid.Columns["Dyelot"].DefaultCellStyle.BackColor = Color.White;
            this.grid.Columns["Ticketyds"].DefaultCellStyle.BackColor = Color.White;

            this.grid.Columns["Scale"].DefaultCellStyle.BackColor = Color.MistyRose;
            this.grid.Columns["Result"].DefaultCellStyle.BackColor = Color.MistyRose;
            this.grid.Columns["InspDate"].DefaultCellStyle.BackColor = Color.MistyRose;
            this.grid.Columns["Inspector"].DefaultCellStyle.BackColor = Color.MistyRose;
            this.grid.Columns["Name"].DefaultCellStyle.BackColor = Color.White;
            this.grid.Columns["Remark"].DefaultCellStyle.BackColor = Color.MistyRose;

            this.grid.Columns["Result"].DefaultCellStyle.ForeColor = Color.Red;

            return true;
        }

        protected override void OnInsert()
        {
            DataTable Dt = (DataTable)this.gridbs.DataSource;
            base.OnInsert();

            DataRow selectDr = ((DataRowView)this.grid.GetSelecteds(SelectedSort.Index)[0]).Row;

            selectDr["Result"] = string.Empty;
            selectDr["Inspdate"] = DateTime.Now.ToShortDateString();
            selectDr["Inspector"] = this.loginID;
            selectDr["Name"] = MyUtility.GetValue.Lookup("Name", this.loginID, "Pass1", "ID");
            selectDr["poid"] = this.maindr["poid"];
            selectDr["SEQ1"] = this.maindr["SEQ1"];
            selectDr["SEQ2"] = this.maindr["SEQ2"];
            selectDr["scale"] = string.Empty;
        }

        protected override bool OnSaveBefore()
        {
            DataTable gridTb = (DataTable)this.gridbs.DataSource;

            return base.OnSaveBefore();
        }

        private void btnEncode_Click(object sender, EventArgs e)
        {
            string updatesql = string.Empty;
            DataTable gridTb = (DataTable)this.gridbs.DataSource;

            // 2018/12/13 ISP20181179 移除沒有資料沒法encode的判斷
            // if (MyUtility.Check.Empty(CurrentData) && this.btnEncode.Text=="Encode")
            // {
            //    MyUtility.Msg.WarningBox("Data not found! ");
            //    return;
            // }

            // 改為判斷 Result欄位是否全部 = ''
            if (this.btnEncode.Text == "Encode")
            {
                int ResultEmptyCount = gridTb.Select("Result = ''").Count();

                if (gridTb.Rows.Count == ResultEmptyCount)
                {
                    MyUtility.Msg.WarningBox("Must inspection one fabric !!! ");
                    return;
                }
            }

            #region 判斷 Scale,Result,Inspdate,Inspector 不可為空(如果全為空則不用檢查)

            DataRow[] AllEmpty_drArray = gridTb.Select("Scale='' AND Result='' AND Inspdate IS NULL AND Inspector=''");

            DataRow[] Total_drArray = gridTb.Select("Scale='' OR Result='' OR Inspdate IS NULL OR Inspector=''");

            DataRow[] Scale_drArray = gridTb.Select("Scale=''");
            DataRow[] Result_drArray = gridTb.Select("Result=''");
            DataRow[] Inspdate_drArray = gridTb.Select("Inspdate IS NULL");
            DataRow[] Inspector_drArray = gridTb.Select("Inspector=''");

            if (Total_drArray.Length != 0)
            {
                string errorMsg = string.Empty;

                foreach (DataRow row in Total_drArray)
                {
                    string singleRow = string.Empty;
                    List<string> colAry = new List<string>();

                    string Roll = row["Roll"].ToString();
                    string Dyelot = row["Dyelot"].ToString();

                    bool IsAllEmpty = AllEmpty_drArray.Where(o => o["Roll"].ToString() == Roll && o["Dyelot"].ToString() == Dyelot).Count() > 0;

                    // 如果全為空則不用檢查
                    if (IsAllEmpty)
                    {
                        continue;
                    }

                    /*
                     如果空請出現下列訊息 (多筆請斷行)
                    Roll:{0},Dyelot: {1} ,{2} can not be empty!
                    {0}: 有資料為null的Roll
                    {1}: 有資料為null的Dyelot
                    {2}: 哪個欄位為null ,如多個請用空格分開
                     */

                    // 判斷是哪個欄位空
                    bool IsScaleEmpty = Scale_drArray.Where(o => o["Roll"].ToString() == Roll && o["Dyelot"].ToString() == Dyelot).Count() > 0;
                    bool IsResultEmpty = Result_drArray.Where(o => o["Roll"].ToString() == Roll && o["Dyelot"].ToString() == Dyelot).Count() > 0;
                    bool IsInspdateEmpty = Inspdate_drArray.Where(o => o["Roll"].ToString() == Roll && o["Dyelot"].ToString() == Dyelot).Count() > 0;
                    bool IsInspectorEmpty = Inspector_drArray.Where(o => o["Roll"].ToString() == Roll && o["Dyelot"].ToString() == Dyelot).Count() > 0;
                    singleRow = string.Format("Roll:{0},Dyelot: {1} ,", Roll, Dyelot);

                    if (IsScaleEmpty)
                    {
                        colAry.Add("Scale");
                    }

                    if (IsResultEmpty)
                    {
                        colAry.Add("Result");
                    }

                    if (IsInspdateEmpty)
                    {
                        colAry.Add("Inspdate");
                    }

                    if (IsInspectorEmpty)
                    {
                        colAry.Add("Inspector");
                    }

                    singleRow += colAry.JoinToString(" ") + " can not be empty!";

                    errorMsg += singleRow + Environment.NewLine;
                }

                if (!MyUtility.Check.Empty(errorMsg))
                {
                    MyUtility.Msg.WarningBox(errorMsg);
                    return;
                }
            }

            #endregion

            if (!MyUtility.Convert.GetBool(this.maindr["shadebondEncode"])) // Encode
            {
                if (!MyUtility.Convert.GetBool(this.maindr["nonshadebond"])) // 只要沒勾選就要判斷，有勾選就可直接Encode
                {
                    if (MyUtility.GetValue.Lookup("WeaveTypeID", this.maindr["SCIRefno"].ToString(), "Fabric", "SciRefno") == "KNIT")
                    {
                        // 當Fabric.WeaveTypdID = 'Knit' 時必須每ㄧ缸都要有檢驗
                        DataTable dyeDt;
                        string cmd = string.Format(
                        @"
Select distinct dyelot from Receiving_Detail a WITH (NOLOCK) where 
a.id='{0}' and a.poid='{2}' and a.seq1 ='{3}' and a.seq2='{4}'  
and not exists 
(Select distinct dyelot from FIR_ShadeBone b WITH (NOLOCK) where b.id={1} and a.dyelot = b.dyelot)
union
Select distinct dyelot from TransferIn_Detail a WITH (NOLOCK) where 
a.id='{0}' and a.poid='{2}' and a.seq1 ='{3}' and a.seq2='{4}'  
and not exists 
(Select distinct dyelot from FIR_ShadeBone b WITH (NOLOCK) where b.id={1} and a.dyelot = b.dyelot)
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

                DataRow[] ResultAry = gridTb.Select("Result = 'Fail'");
                string result = "Pass";
                if (ResultAry.Length > 0)
                {
                    result = "Fail";
                }
                #region  寫入虛擬欄位
                this.maindr["shadebond"] = result;
                this.maindr["shadebondDate"] = DateTime.Now.ToShortDateString();
                this.maindr["shadebondEncode"] = true;
                this.maindr["EditName"] = this.loginID;
                this.maindr["EditDate"] = DateTime.Now.ToShortDateString();
                this.maindr["ShadeboneInspector"] = this.loginID;
                #endregion
                #region 判斷Result 是否要寫入
                string[] returnstr = Sci.Production.PublicPrg.Prgs.GetOverallResult_Status(this.maindr);
                #endregion
                #region  寫入實體Table
                updatesql = string.Format(
                @"Update Fir set shadebondDate = GetDate(),shadebondEncode=1,EditName='{0}',EditDate = GetDate(),shadebond = '{1}',Result ='{2}',Status='{4}',ShadeboneInspector = '{0}' where id ={3}", this.loginID, result, returnstr[0], this.maindr["ID"], returnstr[1]);
                #endregion

                 // *****Send Excel Email 尚未完成 需寄給Encoder的Teamleader 與 Supervisor*****
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
						 ), 1, 1, '')", Sci.Env.User.UserID);
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
                    var email = new MailTo(Sci.Env.Cfg.MailFrom, mailto, ccAddress, subject, this.excelFile, content, false, true);
                    email.ShowDialog(this);
                }

                // *********************************************************************************
                this.maindr["Result"] = returnstr[0];
                this.maindr["Status"] = returnstr[1];
            }
            else // Amend
            {
                #region  寫入虛擬欄位
                this.maindr["shadebond"] = string.Empty;
                this.maindr["shadebondDate"] = DBNull.Value;
                this.maindr["shadebondEncode"] = false;
                this.maindr["EditName"] = this.loginID;
                this.maindr["EditDate"] = DateTime.Now.ToShortDateString();
                this.maindr["ShadeboneInspector"] = string.Empty;

                // 判斷Result and Status 必須先確認shadebond="",判斷才會正確
                string[] returnstr = Sci.Production.PublicPrg.Prgs.GetOverallResult_Status(this.maindr);
                this.maindr["Result"] = returnstr[0];
                this.maindr["Status"] = returnstr[1];
                #endregion
                #region  寫入實體Table
                updatesql = string.Format(
                @"Update Fir set shadebondDate = null,shadebondEncode=0,EditName='{0}',EditDate = GetDate(),shadebond = '',Result ='{2}',Status='{3}', ShadeboneInspector = '' where id ={1}", this.loginID, this.maindr["ID"], returnstr[0], returnstr[1]);
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

                    // 更新PO.FIRInspPercent和AIRInspPercent
                    if (!(upResult = DBProxy.Current.Execute(null, $"exec UpdateInspPercent 'FIR','{this.maindr["POID"].ToString()}'; ")))
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
                    this.ShowErr("Commit transaction error.", ex);
                    return;
                }
            }

            this.OnRequery();
        }

        private void btnApprove_Click(object sender, EventArgs e)
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
                var email = new MailTo(Sci.Env.Cfg.MailFrom, mailto, mailCC, subject, this.excelFile, content, false, true);
                email.ShowDialog(this);
            }
            #endregion

            this.OnRequery();
        }

        protected override void OnUIConvertToMaintain()
        {
            base.OnUIConvertToMaintain();
            this.append.Visible = false;
            this.revise.Visible = false;
            this.delete.Visible = false;
        }

        private void button_enable()
        {
            if (this.maindr == null)
            {
                return;
            }

            this.btnEncode.Enabled = this.CanEdit && !this.EditMode && this.maindr["Status"].ToString() != "Approved";

            this.btnToExcel.Enabled = !this.EditMode;
            this.btnPrintFormatReport.Enabled = !this.EditMode;
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
                this.btnApprove.Enabled = this.CanEdit && !this.EditMode && lApprove == 1 && !MyUtility.Check.Empty(this.maindr["Result"]) && !MyUtility.Check.Empty(this.maindr["shadebond"]);
            }
            else
            {
                this.btnApprove.Enabled = this.CanEdit && !this.EditMode && lCheck == 1 && !MyUtility.Check.Empty(this.maindr["Result"]) && !MyUtility.Check.Empty(this.maindr["shadebond"]);
            }
        }

        private void btnToExcel_Click(object sender, EventArgs e)
        {
            this.ToExcel(false);
        }

        private bool ToExcel(bool isSendMail)
        {
            #region Excel Grid Value
            DataTable dt;
            DualResult xresult;
            if (xresult = DBProxy.Current.Select("Production", string.Format(
                @"
select Roll,Dyelot,TicketYds,Scale,Result
,[Inspdate]=convert(varchar,Inspdate, 111) 
,Inspector,Remark from FIR_Shadebone WITH (NOLOCK) where id='{0}' AND Result!= '' ", this.ID), out dt))
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
            string SeasonID = string.Empty;
            string ContinuityEncode = string.Empty;
            DualResult xresult1;
            if (xresult1 = DBProxy.Current.Select("Production", string.Format(
            "select Roll,Dyelot,Scale,a.Result,a.Inspdate,Inspector,a.Remark,B.ContinuityEncode,C.SeasonID from FIR_Shadebone a WITH (NOLOCK) left join FIR b WITH (NOLOCK) on a.ID=b.ID LEFT JOIN ORDERS C ON B.POID=C.ID where a.ID='{0}'", this.ID), out dt1))
            {
                if (dt1.Rows.Count == 0)
                {
                    SeasonID = string.Empty;
                    ContinuityEncode = string.Empty;
                }
                else
                {
                    SeasonID = dt1.Rows[0]["SeasonID"].ToString();
                    ContinuityEncode = dt1.Rows[0]["ContinuityEncode"].ToString();
                }
            }
            #endregion
            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Quality_P01_ShadeBand_Report.xltx"); // 預先開啟excel app
            objApp.Visible = false;
            MyUtility.Excel.CopyToXls(dt, string.Empty, "Quality_P01_ShadeBand_Report.xltx", 5, false, null, objApp);      // 將datatable copy to excel
            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
            objSheets.Cells[2, 2] = this.displaySP.Text.ToString();
            objSheets.Cells[2, 4] = this.displaySEQ.Text.ToString();
            objSheets.Cells[2, 6] = this.displayColor.Text.ToString();
            objSheets.Cells[2, 8] = this.displayStyle.Text.ToString();
            objSheets.Cells[2, 10] = SeasonID;

            string MCHandle = MyUtility.GetValue.Lookup($"SELECT MCHandle FROM Orders WHERE ID='{this.displaySP.Text.ToString()}'");
            objSheets.Cells[3, 2] = MyUtility.GetValue.Lookup($"SELECT dbo.getPass1_ExtNo('{MCHandle}')");

            objSheets.Cells[3, 4] = ContinuityEncode;
            objSheets.Cells[3, 6] = this.displayResult.Text.ToString();
            objSheets.Cells[3, 8] = this.dateLastInspectionDate.Value.HasValue ? this.dateLastInspectionDate.Value.Value.ToString("yyyy/MM/dd") : string.Empty;
            objSheets.Cells[3, 10] = this.displayBrand.Text.ToString();
            objSheets.Cells[4, 2] = this.displayRefno.Text.ToString();
            objSheets.Cells[4, 4] = this.displayArriveQty.Text.ToString();
            objSheets.Cells[4, 6] = this.dateArriveWHDate.Value.HasValue ? this.dateArriveWHDate.Value.Value.ToString("yyyy/MM/dd") : string.Empty;
            objSheets.Cells[4, 8] = this.txtsupplier.DisplayBox1.Text.ToString();
            objSheets.Cells[4, 10] = this.displayWKNo.Text.ToString();

            objSheets.Range[string.Format("A6:J{0}", dt.Rows.Count + 5)].Borders.Weight = 2; // 設定全框線

            // 合併儲存格
            for (int i = 1; i <= dt.Rows.Count; i++)
            {
                objSheets.Range[string.Format("H{0}:J{0}", (i + 5).ToString())].Merge(Type.Missing);
            }

            objApp.Cells.EntireColumn.AutoFit();    // 自動欄寬
            objApp.Cells.EntireRow.AutoFit();       ////自動欄高

            #region Save Excel
            this.excelFile = Sci.Production.Class.MicrosoftFile.GetName("QA_P01_ShadeBand");
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

        private void btnPrintFormatReport_Click(object sender, EventArgs e)
        {
            DataTable dt_title;
            DataTable dt_Exp;
            DualResult result;

            // 抓表頭資料
            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", Sci.Env.User.Factory));
            result = DBProxy.Current.Select(
                string.Empty,
                @"select NameEN from Factory WITH (NOLOCK) where id=@ID ", pars, out dt_title);
            if (!result)
            {
                this.ShowErr(result);
            }

            // 抓Invo,ETA 資料
            List<SqlParameter> par_Exp = new List<SqlParameter>();
            par_Exp.Add(new SqlParameter("@wkno", this.displayWKNo.Text));
            result = DBProxy.Current.Select(string.Empty, @"select id,Eta from Export where id=@wkno ", par_Exp, out dt_Exp);
            if (!result)
            {
                this.ShowErr(result);
            }

            // 變數區
            string Title = dt_title.Rows.Count == 0 ? string.Empty : dt_title.Rows[0]["NameEN"].ToString();
            string suppid = this.txtsupplier.TextBox1.Text + " - " + this.txtsupplier.DisplayBox1.Text;
            string Invno = dt_Exp.Rows.Count == 0 ? string.Empty : dt_Exp.Rows[0]["ID"].ToString();

            string Refno = "Ref#" + this.displayRefno.Text + " , " + this.displaySCIRefno1.Text;

            ReportDefinition report = new ReportDefinition();

            // @變數
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Title", Title));

            DataTable dt = new DataTable();
            DataRow dr;
            dt.Columns.Add(new DataColumn("Poid", typeof(string)));
            dt.Columns.Add(new DataColumn("FactoryID", typeof(string)));
            dt.Columns.Add(new DataColumn("Style", typeof(string)));
            dt.Columns.Add(new DataColumn("Color", typeof(string)));
            dt.Columns.Add(new DataColumn("DESC", typeof(string)));
            dt.Columns.Add(new DataColumn("Supp", typeof(string)));
            dt.Columns.Add(new DataColumn("Invo", typeof(string)));
            dt.Columns.Add(new DataColumn("ETA", typeof(string)));

            dr = dt.NewRow();
            dr["Poid"] = this.displaySP.Text;
            dr["FactoryID"] = Sci.Env.User.Factory;
            dr["Style"] = this.displayStyle.Text;
            dr["Color"] = this.displayColor.Text;
            dr["DESC"] = Refno;
            dr["Supp"] = suppid;
            dr["Invo"] = Invno;
            dr["ETA"] = dt_Exp.Rows.Count == 0 ? string.Empty : DateTime.Parse(dt_Exp.Rows[0]["ETA"].ToString()).ToString("yyyy-MM-dd").ToString();
            dt.Rows.Add(dr);

            List<P01_ShadeBond_Data> data = dt.AsEnumerable()
                .Select(row1 => new P01_ShadeBond_Data()
            {
                POID = row1["Poid"].ToString().Trim(),
                FactoryID = row1["FactoryID"].ToString().Trim(),
                Style = row1["Style"].ToString().Trim(),
                Color = row1["Color"].ToString().Trim(),
                DESC = row1["DESC"].ToString().Trim(),
                Supp = row1["Supp"].ToString().Trim(),
                Invo = row1["Invo"].ToString().Trim(),
                ETA = row1["ETA"].ToString() == string.Empty ? string.Empty : DateTime.Parse(row1["ETA"].ToString()).ToString("yyyy-MM-dd").ToString().Trim(),
            }).ToList();

            report.ReportDataSource = data;

            Type ReportResourceNamespace = typeof(P01_ShadeBond_Data);
            Assembly ReportResourceAssembly = ReportResourceNamespace.Assembly;
            string ReportResourceName = "P01_ShadeBond_Print.rdlc";

            IReportResource reportresource;
            if (!(result = ReportResources.ByEmbeddedResource(ReportResourceAssembly, ReportResourceNamespace, ReportResourceName, out reportresource)))
            {
                return;
            }

            report.ReportResource = reportresource;

            // 開啟 report view
            var frm = new Win.Subs.ReportView(report);
            frm.MdiParent = this.MdiParent;
            frm.Show();
        }

        private void BtnInspectedallpass_Click(object sender, EventArgs e)
        {
            this.grid.ValidateControl();
            DataRow[] drs = ((DataTable)this.gridbs.DataSource).Select("Selected = 1");
            if (drs.Length == 0)
            {
                return;
            }

            foreach (DataRow item in drs)
            {
                item["Scale"] = "4-5";
                item["Result"] = "Pass";
                item["Inspdate"] = DateTime.Now.ToShortDateString();
                item["Inspector"] = this.loginID;
                item["Name"] = MyUtility.GetValue.Lookup("Name", this.loginID, "Pass1", "ID");
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            this.grid.ValidateControl();
            DataRow[] drs = ((DataTable)this.gridbs.DataSource).Select("Selected = 1");
            if (drs.Length == 0)
            {
                return;
            }

            foreach (DataRow item in drs)
            {
                item["Result"] = "Fail";
                item["Inspdate"] = DateTime.Now.ToShortDateString();
                item["Inspector"] = this.loginID;
                item["Name"] = MyUtility.GetValue.Lookup("Name", this.loginID, "Pass1", "ID");
            }
        }
    }
}
