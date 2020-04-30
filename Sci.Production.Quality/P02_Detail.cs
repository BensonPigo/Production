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
using System.Data.SqlClient;

namespace Sci.Production.Quality
{
    public partial class P02_Detail : Sci.Win.Subs.Input6A
    {
        #region 改版原因事項
        /*
         使用Inpute6A理由: 可以使用切換上下筆的功能
         *但是Inpute6A需要直接存進DB,只好手刻存檔以及Encode功能
         *Inpute6A原始按鈕save and Undo功能不使用理由:1.更多可調整性,避免被底層綁死
         *注意! P02_Detail WorkAlias沒有特別作用,所有的繫結資料(mtbs - )來源都是上一層的CurrentDetailData(detail GridView)   
         */
        #endregion
        private string loginID = Sci.Env.User.UserID;        
        private bool canedit;
        private string id;
        private string receivingID;
        private string poid;
        private string seq1;
        private string seq2;

        public P02_Detail(bool CanEdit, string airID,string spNo)
        {
            InitializeComponent();
            id = airID;                          
            canedit = CanEdit;
            btn_status(id);
            button_enable(canedit);            
           
            string air_cmd = string.Format("select * from air WITH (NOLOCK) where id='{0}'", id);
            DataRow dr;
            if (MyUtility.Check.Seek(air_cmd, out dr))
            {
                txtSEQ.Text = dr["SEQ1"].ToString() + " - " + dr["SEQ2"].ToString();
                this.seq1 = dr["SEQ1"].ToString();
                this.seq2 = dr["SEQ2"].ToString();
                this.receivingID = dr["ReceivingID"].ToString();
                poid = dr["Poid"].ToString();
            }
            else
            {
                txtSEQ.Text = "";
                this.receivingID = string.Empty;
                this.seq1 = string.Empty;
                this.seq2 = string.Empty;
                poid = string.Empty;
            }
            
            Dictionary<String, String> comboBox1_RowSource = new Dictionary<string, string>();
            comboBox1_RowSource.Add("Approval", "Approval");
            comboBox1_RowSource.Add("N/A", "N/A");
            comboBox1_RowSource.Add("Pass", "Pass");
            comboBox1_RowSource.Add("Fail", "Fail");
            comboResult.DataSource = new BindingSource(comboBox1_RowSource, null);
            comboResult.ValueMember = "Key";
            comboResult.DisplayMember = "Value";

        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            button_enable(canedit);            
        }

        private void btnAmend_Click(object sender, EventArgs e)
        {
            string updatesql = "";
            string updatesql1 = "";
            DualResult chkresult;
            DataTable dt;

            if (this.btnAmend.Text == "Amend")
            {
                DialogResult btnAmend = MyUtility.Msg.QuestionBox("Are you sure want to <Amend> this data?", "Question", MessageBoxButtons.YesNo);
                if (btnAmend == DialogResult.No)
                {
                    return;
                }
                else
                {                   
                    #region  寫入實體Table Amend
                    updatesql1 = string.Format(
                    "Update Air set Status = 'New',EditDate=CONVERT(VARCHAR(20), GETDATE(), 120),EditName='{0}' where id ='{1}'",
                     loginID, id);
                                       

                    DualResult upResult1;
                    TransactionScope _transactionscope1 = new TransactionScope();
                    using (_transactionscope1)
                    {
                        try
                        {
                            if (!(upResult1 = DBProxy.Current.Execute(null, updatesql1)))
                            {
                                _transactionscope1.Dispose();

                                return;
                            }

                            _transactionscope1.Complete();
                            _transactionscope1.Dispose();
                            btn_status(id);
                            this.btnEdit.Text = "Edit";
                            this.btnAmend.Text = "Encode";
                            this.btnEdit.Enabled = true;
                        }
                        catch (Exception ex)
                        {
                            _transactionscope1.Dispose();
                            ShowErr("Commit transaction error.", ex);
                            return;
                        }
                    }

                    #endregion
                }   
            }

            else
            {
                if (MyUtility.Check.Empty(txtInspectedQty) || MyUtility.Check.Empty(comboResult.Text) || MyUtility.Check.Empty(txtInspector) || MyUtility.Check.Empty(dateInspectDate))
                {
                    if (MyUtility.Check.Empty(txtInspectedQty.Text))
                    {
                        this.txtInspectedQty.Focus();
                        MyUtility.Msg.InfoBox("<Inspected> can not be null");
                        return;
                    }
                    else if (MyUtility.Check.Empty(comboResult.SelectedValue))
                    {
                        this.comboResult.Focus();
                        MyUtility.Msg.InfoBox("<Result> can not be null");
                        return;
                    }
                    else if (MyUtility.Check.Empty(dateInspectDate))
                    {
                        this.dateInspectDate.Focus();
                        MyUtility.Msg.InfoBox("<Inspdate> can not be null");
                        return;
                    }
                    else if (MyUtility.Check.Empty(txtInspector))
                    {
                        this.txtInspector.Focus();
                        MyUtility.Msg.InfoBox("<Inspector> can not be null");
                        return;
                    }
                }
                #region  寫入實體Table Encode
                if (comboResult.SelectedValue.ToString() == "Approval")
                {
                    MyUtility.Msg.InfoBox("<Result> Can not be Approval.");
                    return;
                }
                updatesql = string.Format(
                "Update Air set Status = 'Confirmed',EditDate=CONVERT(VARCHAR(20), GETDATE(), 120),EditName='{0}' where id ='{1}'",
                loginID, id);

                string strInspAutoLockAcc = MyUtility.GetValue.Lookup("SELECT InspAutoLockAcc FROM System");

                if (MyUtility.Convert.GetBool(strInspAutoLockAcc))
                {
                    switch (this.comboResult.Text.ToString())
                    {
                        case "Fail":
                            updatesql += Environment.NewLine + $@"
UPDATE f SET 
Lock = 1 , LockName='{Sci.Env.User.UserID}' ,LockDate=GETDATE(), F.Remark='Auto Lock by QA_P02.Accessory Inspection'
FROM FtyInventory f 
WHERE f.POID='{this.poid}' AND f.Seq1='{this.seq1}' AND f.Seq2='{this.seq2}'";
                            break;

                        case "Pass":

                            chkresult = DBProxy.Current.Select(null, $@"
SELECT DISTINCT  Result
FROM AIR
WHERE POID='{this.poid}' AND Seq1='{this.seq1} ' AND Seq2='{this.seq2}'
AND ID<>'{this.id}' AND ReceivingID<>'{this.receivingID}'
", out dt);
                            if (!chkresult)
                            {
                                ShowErr("Commit transaction error.", chkresult);
                                return;
                            }

                            bool isAllPass = false;


                            //=1表示有相同POID Seq 1 2，且Result只有Pass一種結果
                            if (dt.Rows.Count == 1)
                            {
                                if (dt.Rows[0]["Result"].ToString() == "Pass")
                                {
                                    isAllPass = true;
                                }
                            }
                            //表示無相同POID Seq 1 2
                            if (dt.Rows.Count == 0)
                            {
                                isAllPass = true;
                            }

                            if (isAllPass)
                            {
                                updatesql += Environment.NewLine + $@"
UPDATE f SET 
Lock = 0 , LockName='{Sci.Env.User.UserID}' ,LockDate=GETDATE(), F.Remark='Auto unLock by QA_P02.Accessory Inspection'
FROM FtyInventory f 
WHERE f.POID='{this.poid}' AND f.Seq1='{this.seq1}' AND f.Seq2='{this.seq2}'";
                            }

                            break;
                        default:
                            break;
                    }
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
                        this.btnAmend.Text = "Amend";
                        this.btnEdit.Text = "Edit";
                        this.btnEdit.Enabled = false;
                        
                        btn_status(this.id);
                    }
                    catch (Exception ex)
                    {
                        _transactionscope.Dispose();
                        ShowErr("Commit transaction error.", ex);
                        return;
                    }
                }
                #endregion


                //ISP20200575 Encode全部執行後
                string sqlcmd = $@"select distinct orderid=o.ID from Orders o with(nolock) where o.poid = '{poid}'";
                DataTable dtid;
                DualResult result1 = DBProxy.Current.Select(string.Empty, sqlcmd, out dtid);
                if (!result1)
                {
                    this.ShowErr(result1);
                }
                else
                {
                    string sqlup = $@"
update a 
set Status = 'Preparing'
from #tmp t
inner join AccessoryOrderList a with(nolock) on a.OrderID = t.orderid and a.Status = 'Waiting'
where dbo.GetAirQaRecord(t.orderid) ='PASS'
";
                    SqlConnection sqlConn = null;
                    DBProxy.Current.OpenConnection("ManufacturingExecution", out sqlConn);
                    result1 = MyUtility.Tool.ProcessWithDatatable(dtid, string.Empty, sqlup, out dtid, "#tmp", sqlConn);
                    if (!result1)
                    {
                        this.ShowErr(result1);
                    }
                }
            }
            //更新PO.FIRInspPercent和AIRInspPercent
            DualResult result;
            if (!(result = DBProxy.Current.Execute(null, $"exec UpdateInspPercent 'AIR','{poid}';")))
            {
                this.ShowErr(result);
            }
        }

        //Save and Edit
        private void btnEdit_Click(object sender, EventArgs e)
        {
            string strSqlcmd = "";
            this.btnAmend.Enabled = false;
            


            DualResult result;
            DataTable dt;
            strSqlcmd = string.Format("Select * from AIR WITH (NOLOCK) where ID='{0}' ", id);
            if (result = DBProxy.Current.Select(null, strSqlcmd, null, out dt))
            {
                if (dt.Rows.Count > 0)
                {
                    if (this.btnEdit.Text == "Save")
                    {
                        if (MyUtility.Check.Empty(txtInspectedQty.Text))
                        {
                            this.txtInspectedQty.Focus();
                            MyUtility.Msg.InfoBox("<Inspected> can not be null");
                            return;
                        }
                        if (MyUtility.Check.Empty(comboResult.SelectedValue))
                        {
                            this.comboResult.Focus();
                            MyUtility.Msg.InfoBox("<Result> can not be null");
                            return;
                        }
                        if ((txtRejectedQty.Text != "0.00" || txtRejectedQty.Text != "") && (MyUtility.Check.Empty(editDefect)))
                        {
                            this.editDefect.Focus();
                            MyUtility.Msg.InfoBox("When <Rejected Qty> has any value then <Defect> can not be empty !");
                            return;
                        }
                        if ((!MyUtility.Check.Empty(editDefect.Text)) && (this.txtRejectedQty.Text == "0.00"))
                        {
                            this.txtRejectedQty.Focus();
                            MyUtility.Msg.InfoBox("<Rejected Qty> can not be empty ,when <Defect> has not empty ! ");
                            return;
                        }
                        if ((this.comboResult.Text.ToString() == "Fail") && (MyUtility.Convert.GetDecimal(this.txtRejectedQty.Text) == 0 || MyUtility.Check.Empty(editDefect)))
                        {
                            this.txtRejectedQty.Focus();
                            MyUtility.Msg.InfoBox("When <Result> is Fail then <Rejected Qty> can not be empty !");
                            return;
                        }
                        string updatesql = "";
                        #region  寫入實體Table Encode
                        string InspDate = MyUtility.Check.Empty(dateInspectDate.Value) ? "Null" : "'" + string.Format("{0:yyyy-MM-dd}", dateInspectDate.Value) + "'";
                        updatesql = string.Format(
                        "Update Air set InspQty= '{0}',RejectQty='{1}',Inspdate = {2},Inspector = '{3}',Result= '{4}',Defect='{5}',Remark='{6}' where id ='{7}'",
                        this.txtInspectedQty.Text, this.txtRejectedQty.Text,
                        InspDate, txtInspector.TextBox1.Text, comboResult.Text, editDefect.Text, txtRemark.Text, id);

                        DualResult upResult;
                        TransactionScope _transactionscope = new TransactionScope();
                        using (_transactionscope)
                        {
                            try
                            {
                                if (!(upResult = DBProxy.Current.Execute(null, updatesql)))
                                {
                                    _transactionscope.Dispose();
                                    MyUtility.Msg.WarningBox("Update Fail!!");
                                    return;
                                }
                                _transactionscope.Complete();
                                _transactionscope.Dispose();
                                MyUtility.Msg.InfoBox("Successfully");
                                this.btnAmend.Text = "Encode";
                                this.btnEdit.Text = "Edit";
                                this.btnAmend.Enabled = true;

                            }
                            catch (Exception ex)
                            {
                                _transactionscope.Dispose();
                                ShowErr("Commit transaction error.", ex);
                                return;
                            }
                        }
                        #endregion
                        this.txtInspectedQty.ReadOnly = true;
                        this.txtRejectedQty.ReadOnly = true;
                        this.dateInspectDate.ReadOnly = true;
                        this.txtInspector.TextBox1.ReadOnly = true;
                        this.txtsupplier.TextBox1.ReadOnly = true;
                        this.comboResult.ReadOnly = true;
                        this.txtRemark.ReadOnly = true;
                        this.editDefect.ReadOnly = true;
                        this.btnClose.Text = "Close";
                        this.left.Enabled = true;
                        this.right.Enabled = true;
                        this.EditMode = false;//因為從上一層進來是false,導致popup功能無法使用,所以才改變EditMode
                        return;
                    }
                    else
                    {
                        this.EditMode = true;//因為從上一層進來是false,導致popup功能無法使用,所以才改變EditMode
                        if (MyUtility.Check.Empty(this.dateInspectDate.Value))
                        {
                            this.dateInspectDate.Value = DateTime.Today;
                            CurrentData["InspDate"] = string.Format("{0:yyyy-MM-dd}", dateInspectDate.Value);

                        }
                        if (MyUtility.Check.Empty(this.txtInspector.TextBox1.Text))
                        {
                            this.txtInspector.TextBox1.Text = Sci.Env.User.UserID;
                            CurrentData["Inspector"] = Sci.Env.User.UserID;
                        }
                       this.EditMode = true;//因為從上一層進來是false,導致popup功能無法使用,所以才改變EditMode
                       if (MyUtility.Check.Empty(this.dateInspectDate.Value))
                       {
                           this.dateInspectDate.Value = DateTime.Today;
                           CurrentData["InspDate"] = string.Format("{0:yyyy-MM-dd}", dateInspectDate.Value);

                       }
                       if (MyUtility.Check.Empty(this.txtInspector.TextBox1.Text))
                       {
                           this.txtInspector.TextBox1.Text = Sci.Env.User.UserID;
                           CurrentData["Inspector"] = Sci.Env.User.UserID;
                       }
                        if (dt.Rows[0]["Status"].ToString().Trim() == "Confirmed")
                        {
                            this.btnAmend.Enabled = true;
                            MyUtility.Msg.InfoBox("It's already Confirmed");
                            return;
                        }
                        if (dt.Rows[0]["Status"].ToString().Trim() != "Confirmed")
                        {
                            this.txtInspectedQty.ReadOnly = false;
                            this.txtRejectedQty.ReadOnly = false;
                            this.dateInspectDate.ReadOnly = false;
                            this.txtInspector.TextBox1.ReadOnly = false;
                            this.txtsupplier.TextBox1.ReadOnly = true;
                            this.comboResult.ReadOnly = false;
                            this.txtRemark.ReadOnly = false;
                            this.editDefect.ReadOnly = true;
                            this.btnEdit.Text = "Save";
                            this.btnClose.Text = "Undo";
                            this.left.Enabled = false;
                            this.right.Enabled = false;
                            return;
                        }
                    }
                }
            }
        }

        private void editDefect_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.btnEdit.Text!="Save") return;
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                string sqlcmd = "select id,description from AccessoryDefect WITH (NOLOCK) ";
                SelectItem2 item = new SelectItem2(sqlcmd, "Code,Description","10,30", null, null, null, null);
                DialogResult result = item.ShowDialog();
                if (result == DialogResult.Cancel) { return; }
                this.editDefect.Text = item.GetSelectedString().Replace(",", "+");

            }
        }

        private void button_enable(bool canedit)
        {
            // Visable
            this.btnEdit.Visible = (bool)canedit;
            this.btnAmend.Visible = (bool)canedit;
            // Enable
            btnEdit.Enabled = this.btnAmend.Text.ToString() == "Encode" ? true : false;
            

        }
       
        private void btn_status(object id)
        {
            string air_cmd = string.Format("select * from air WITH (NOLOCK) where id='{0}'", id);
            DataTable dt;
            DualResult result;
            if (result=DBProxy.Current.Select(null,air_cmd, out dt))
            {
                if (dt.Rows[0]["Status"].ToString().Trim() == "Confirmed")
                {
                    this.btnAmend.Text = "Amend";
                }
                else
                {
                    this.btnAmend.Text = "Encode";
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (this.btnClose.Text.ToString().ToUpper() == "UNDO")
            {
                DialogResult buttonQ = MyUtility.Msg.QuestionBox("Ensure undo?", "Question", MessageBoxButtons.OKCancel);
                if (buttonQ==DialogResult.OK)
                {
                    this.txtInspectedQty.ReadOnly = true;
                    this.txtRejectedQty.ReadOnly = true;
                    this.dateInspectDate.ReadOnly = true;
                    this.txtInspector.TextBox1.ReadOnly = true;
                    this.comboResult.ReadOnly = true;
                    this.txtRemark.ReadOnly = true;
                    this.editDefect.ReadOnly = true;
                    this.btnClose.Text = "Close";
                    this.btnEdit.Text = "Edit";
                    this.left.Enabled = true;
                    this.right.Enabled = true;
                    this.EditMode = false;
                    OnAttached(CurrentData);
                    ReBindingData();                   
                    return;
                }
                else
                {
                    return;
                }               
            }
            else
            {
                this.Close();
            }            
        }

        /// <summary>
        /// 將CurrentData 返回原來的值
        /// 特別針對Undo去處理
        /// </summary>
        /// <param name="data"></param>
        protected override void OnAttached(DataRow data)
        {
            data.RejectChanges();
            base.OnAttached(data);
        }

        private void ChangeData()
        {
            id = this.CurrentData["Id"].ToString();
            string air_cmd = string.Format("select * from air WITH (NOLOCK) where id='{0}'", id);
    
            DataTable dt;
            DBProxy.Current.Select(null, air_cmd, out dt);
            if (!MyUtility.Check.Empty(dt) || dt.Rows.Count > 0)
            {
                txtSEQ.Text = dt.Rows[0]["SEQ1"].ToString() + " - " + dt.Rows[0]["SEQ2"].ToString();
                this.receivingID = dt.Rows[0]["ReceivingID"].ToString();
                this.seq1 = dt.Rows[0]["SEQ1"].ToString();
                this.seq2 = dt.Rows[0]["SEQ2"].ToString();
                if (dt.Rows[0]["Status"].ToString().Trim() == "Confirmed")
                {
                    this.btnAmend.Text = "Amend";
                }
                else
                {
                    this.btnAmend.Text = "Encode";
                }
            }
            else
            {
                txtSEQ.Text = "";
                this.receivingID = string.Empty;
                this.seq1 = string.Empty;
                this.seq2 = string.Empty;
            }
            button_enable(canedit);
        }

        private void right_Click(object sender, EventArgs e)
        {
            ChangeData();
        }

        private void left_Click(object sender, EventArgs e)
        {
            ChangeData();
        }

        /// <summary>
        /// 重新ReloadData
        /// 特別針對Undo去處理
        /// </summary>
        private void ReBindingData()
        {
            txtInspectedQty.Text = CurrentData["InspQty"].ToString();
            txtRejectedQty.Text = CurrentData["RejectQty"].ToString();
            dateInspectDate.Value = MyUtility.Convert.GetDate(CurrentData["InspDate"]);
            txtInspector.TextBox1.Text = CurrentData["Inspector"].ToString();
            comboResult.SelectedValue = CurrentData["Result1"].ToString();
        
        }
    }
}
