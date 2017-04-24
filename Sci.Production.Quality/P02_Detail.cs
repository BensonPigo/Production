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
    public partial class P02_Detail : Sci.Win.Subs.Input6A
    {
        private string loginID = Sci.Env.User.UserID;
        private DataRow maindr;
        private bool canedit;
      

        public P02_Detail(bool CanEdit, string airID, DataRow mainDr)
        {
            InitializeComponent();
            string id = airID;
            this.txtID.Text = id.ToString();
            this.comboResult.ReadOnly = false;
            maindr = mainDr;
            canedit = CanEdit;
            btn_status(id);
            button_enable(canedit);
           
            string air_cmd = string.Format("select * from air WITH (NOLOCK) where id='{0}'", id);
            DataRow dr;

            Dictionary<String, String> comboBox1_RowSource = new Dictionary<string, string>();
            comboBox1_RowSource.Add("Pass", "Pass");
            comboBox1_RowSource.Add("Fail", "Fail");
            comboResult.DataSource = new BindingSource(comboBox1_RowSource, null);
            comboResult.ValueMember = "Key";
            comboResult.DisplayMember = "Value";

            // 串接table Po_Supp_Detail
            DataTable dtPoSuppDetail;
            Ict.DualResult pstResult;
            if (pstResult = DBProxy.Current.Select(null, string.Format("select B.SCIRefno,B.Refno,a.ColorID,a.ColorID,a.StockUnit,a.SizeSpec from PO_Supp_Detail a WITH (NOLOCK) left join AIR b WITH (NOLOCK) on a.ID=b.POID and a.SEQ1=b.SEQ1 and a.SEQ2=b.SEQ2 where b.ID='{0}'", id), out dtPoSuppDetail))
            {
                if (dtPoSuppDetail.Rows.Count != 0)
                {
                    txtSCIRefno.Text = dtPoSuppDetail.Rows[0]["SCIRefno"].ToString();
                    txtRefno.Text = dtPoSuppDetail.Rows[0]["Refno"].ToString();                   
                    txtUnit.Text = dtPoSuppDetail.Rows[0]["StockUnit"].ToString();
                    txtSize.Text = dtPoSuppDetail.Rows[0]["sizespec"].ToString();
                    txtColor.Text = dtPoSuppDetail.Rows[0]["ColorID"].ToString();
                }

            }
            DataTable dtSupplier;
            DBProxy.Current.Select(null, string.Format(@"select a.Suppid,b.AbbEN from AIR a WITH (NOLOCK) inner join Supp b WITH (NOLOCK) on a.Suppid=b.ID
                                  where a.ID='{0}'", id), out dtSupplier);
            if (dtSupplier.Rows.Count != 0)
            {
                txtsupplier.TextBox1.Text = dtSupplier.Rows[0]["Suppid"].ToString();
                txtsupplier.DisplayBox1.Text = dtSupplier.Rows[0]["AbbEN"].ToString();
            }
            else
            {
                txtsupplier.TextBox1.Text = "";
                txtsupplier.DisplayBox1.Text = "";
            }
            //串接table Receiving
            DataTable dtRec;
            Ict.DualResult wknoResult;
            if (wknoResult = DBProxy.Current.Select(null, string.Format("select * from Receiving a WITH (NOLOCK) left join AIR b WITH (NOLOCK) on a.Id=b.ReceivingID where b.ID='{0}' ", id), out dtRec))
            {
                if (dtRec.Rows.Count>0)
                {
                    txtWKNO.Text = dtRec.Rows[0]["exportid"].ToString();
                }
                else
                {
                    txtWKNO.Text = "";
                }
                
            }

            if (MyUtility.Check.Seek(air_cmd, out dr))
            {
                txtSEQ.Text = dr["SEQ1"].ToString() + " - " + dr["SEQ2"].ToString();
                txtInspectedQty.Text = dr["inspQty"].ToString();
                txtRejectedQty.Text = dr["REjectQty"].ToString();
                dateInspectDate.Value = MyUtility.Convert.GetDate(dr["inspdate"]);                  
                txtInspector.Text = dr["inspector"].ToString();
                txtRemark.Text = dr["remark"].ToString();
                comboResult.SelectedValue = dr["Result"].ToString();                
                this.editDefect.Text = dr["Defect"].ToString();
                this.txtArriveQty.Text = dr["ArriveQty"].ToString();
            }
            else
            {
                txtSEQ.Text = "";
                txtInspectedQty.Text = "";
                txtRejectedQty.Text = "";
                dateInspectDate.Text = "";
                txtInspector.Text = "";
                txtRemark.Text = "";
                this.comboResult.DisplayMember = "";
                this.editDefect.Text = "";
                this.txtArriveQty.Text = "";
            }

        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            button_enable(canedit);

        }
        // 上下筆
        //public DualResult Set(bool canedit, IList<DataRow> dtList, DataRow current)
        //{
        //    private void Bind(int pos)
        //    {
        //        virtual protected void OnAttaching(DataRow data)
        //    }
        //}
        

        private void save_Click(object sender, EventArgs e)
        {
            string strSqlcmd = "";
            this.btnAmend.Enabled = false;

            DualResult result;
            DataTable dt;
            strSqlcmd = string.Format("Select * from AIR WITH (NOLOCK) where ID='{0}' ", txtID.Text);          
            if (result = DBProxy.Current.Select(null, strSqlcmd, null, out dt))
            {
                if (dt.Rows.Count > 0)
                {
                    if (this.save.Text == "Save")
                    {
                        if (MyUtility.Check.Empty(txtInspectedQty.Text))
                        {
                            MyUtility.Msg.InfoBox("<Inspected> can not be null");
                            this.txtInspectedQty.Focus();
                            return;
                        }
                        if (MyUtility.Check.Empty(comboResult.SelectedValue))
                        {
                            MyUtility.Msg.InfoBox("<Result> can not be null");
                            this.comboResult.Focus();
                            return;
                        }
                        if (MyUtility.Check.Empty(dateInspectDate))
                        {
                            MyUtility.Msg.InfoBox("<Inspdate> can not be null");
                            this.dateInspectDate.Focus();
                            return;
                        }
                        if (MyUtility.Check.Empty(txtInspector))
                        {
                            MyUtility.Msg.InfoBox("<Inspector> can not be null");
                            this.txtInspector.Focus();
                            return;

                        }
                        if ((txtRejectedQty.Text != "0.00" || txtRejectedQty.Text != "") && (MyUtility.Check.Empty(editDefect)))
                        {
                            MyUtility.Msg.InfoBox("When <Rejected Qty> has any value then <Defect> can not be empty !");
                            this.editDefect.Focus();
                            return;
                        }                        
                        if (( !MyUtility.Check.Empty(editDefect.Text)) && (this.txtRejectedQty.Text == "0.00"))
                        {
                            MyUtility.Msg.InfoBox("<Rejected Qty> can not be empty ,when <Defect> has not empty ! ");
                            this.txtRejectedQty.Focus();
                            return;
                        }
                        if ((this.comboResult.Text.ToString() == "Fail") && (this.txtRejectedQty.Text == "0.00" || MyUtility.Check.Empty(editDefect)))
                        {
                            MyUtility.Msg.InfoBox("When <Result> is Fail then <Rejected Qty> can not be empty !");
                            this.txtRejectedQty.Focus();
                            return;
                        }
                        string updatesql = "";
                        #region  寫入實體Table Encode
                        updatesql = string.Format(
                        "Update Air set InspQty= '{0}',RejectQty='{1}',Inspdate = '{2}',Inspector = '{3}',Result= '{4}',Defect='{5}',Remark='{6}' where id ='{7}'",
                        this.txtInspectedQty.Text, this.txtRejectedQty.Text,string.Format("{0:yyyy-MM-dd}",dateInspectDate.Value) , txtInspector.Text, comboResult.Text, editDefect.Text, txtRemark.Text,  txtID.Text);
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
                                MyUtility.Msg.WarningBox("Successfully");
                                this.btnAmend.Text = "Encode";
                                this.save.Text = "Edit";
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
                        this.txtInspector.ReadOnly = true;
                        this.comboResult.ReadOnly = true;
                        this.txtRemark.ReadOnly = true;
                        this.editDefect.ReadOnly = true;
                        this.undo.Text = "Close";
                        return;
                    }
                    else
                    {
                        if (dt.Rows[0]["Status"].ToString().Trim() == "Confirmed")
                        {
                            MyUtility.Msg.InfoBox("It's already Confirmed");
                            this.btnAmend.Enabled = true;
                            return;
                        }
                        if (dt.Rows[0]["Status"].ToString().Trim() != "Confirmed")
                        {
                            this.txtInspectedQty.ReadOnly = false;
                            this.txtRejectedQty.ReadOnly = false;
                            this.dateInspectDate.ReadOnly = false;
                            this.txtInspector.ReadOnly = false;
                            this.comboResult.ReadOnly = false;
                            this.txtRemark.ReadOnly = false;
                            this.editDefect.ReadOnly = false;
                            this.save.Text = "Save";
                            this.undo.Text = "Undo";
                            
                            return;
                        }
                    }
                }
            }
         
        }

               

        private void Encode_Click(object sender, EventArgs e)
        {
            string updatesql = "";
            string updatesql1 = "";

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
                     loginID, txtID.Text);
                                       

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
                            btn_status(this.txtID.Text);
                            this.save.Text = "Edit";
                            this.btnAmend.Text = "Encode";
                            this.save.Enabled = true;
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
                        MyUtility.Msg.InfoBox("<Inspected> can not be null");
                        this.txtInspectedQty.Focus();
                        return;
                    }
                    else if (MyUtility.Check.Empty(comboResult.SelectedValue))
                    {
                        MyUtility.Msg.InfoBox("<Result> can not be null");
                        this.comboResult.Focus();
                        return;
                    }
                    else if (MyUtility.Check.Empty(dateInspectDate))
                    {
                        MyUtility.Msg.InfoBox("<Inspdate> can not be null");
                        this.dateInspectDate.Focus();
                        return;
                    }
                    else if (MyUtility.Check.Empty(txtInspector))
                    {
                        MyUtility.Msg.InfoBox("<Inspector> can not be null");
                        this.txtInspector.Focus();
                        return;

                    }
                }

                #region  寫入實體Table Encode
                updatesql = string.Format(
                "Update Air set Status = 'Confirmed',EditDate=CONVERT(VARCHAR(20), GETDATE(), 120),EditName='{0}' where id ='{1}'",
                loginID, txtID.Text);
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
                        MyUtility.Msg.WarningBox("Successfully");
                        this.btnAmend.Text = "Amend";
                        this.save.Text = "Edit";
                        this.save.Enabled = false;
                        
                        btn_status(this.txtID.Text);
                    }
                    catch (Exception ex)
                    {
                        _transactionscope.Dispose();
                        ShowErr("Commit transaction error.", ex);
                        return;
                    }
                }
                #endregion
            }


        }
                
        private void editBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.editDefect.ReadOnly == true)
            {
                return;
            }
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                string sqlcmd = "select id,description from AccessoryDefect WITH (NOLOCK) ";
                SelectItem2 item = new SelectItem2(sqlcmd, "15,12", null);
                DialogResult result = item.ShowDialog();
                if (result == DialogResult.Cancel) { return; }
                this.editDefect.Text = item.GetSelectedString().Replace(",", "+");


            }
        }
        private void button_enable(bool canedit)
        {
            // Visable
            this.save.Visible = (bool)canedit ;
            this.undo.Visible = (bool)canedit ;
            this.btnAmend.Visible = (bool)canedit;
            this.btnClose.Visible = !(bool)canedit;
            // Enable
            save.Enabled = this.btnAmend.Text.ToString() == "Encode" ? true : false;

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
            this.Close();
        }

        private void undo_Click(object sender, EventArgs e)
        {
            if (this.undo.Text=="Undo")
            {
                return;
            }
            else
            {
                this.Close();
            }
                
        }

    }
}
