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
            this.textID.Text = id.ToString();
            this.comboBox1.ReadOnly = false;
            maindr = mainDr;
            canedit = CanEdit;
            btn_status(id);
            button_enable(canedit);


            string air_cmd = string.Format("select * from air WITH (NOLOCK) where id='{0}'", id);
            DataRow dr;

            Dictionary<String, String> comboBox1_RowSource = new Dictionary<string, string>();
            comboBox1_RowSource.Add("Pass", "Pass");
            comboBox1_RowSource.Add("Fail", "Fail");
            comboBox1.DataSource = new BindingSource(comboBox1_RowSource, null);
            comboBox1.ValueMember = "Key";
            comboBox1.DisplayMember = "Value";

            // 串接table Po_Supp_Detail
            DataTable dtPoSuppDetail;
            Ict.DualResult pstResult;
            if (pstResult = DBProxy.Current.Select(null, string.Format("select B.SCIRefno,B.Refno,a.ColorID,a.ColorID,a.StockUnit,a.SizeSpec from PO_Supp_Detail a WITH (NOLOCK) left join AIR b WITH (NOLOCK) on a.ID=b.POID and a.SEQ1=b.SEQ1 and a.SEQ2=b.SEQ2 where b.ID='{0}'", id), out dtPoSuppDetail))
            {
                if (dtPoSuppDetail.Rows.Count != 0)
                {
                    ref_text.Text = dtPoSuppDetail.Rows[0]["SCIRefno"].ToString();
                    brand_text.Text = dtPoSuppDetail.Rows[0]["Refno"].ToString();                   
                    unit_text.Text = dtPoSuppDetail.Rows[0]["StockUnit"].ToString();
                    size_text.Text = dtPoSuppDetail.Rows[0]["sizespec"].ToString();
                    color_text.Text = dtPoSuppDetail.Rows[0]["ColorID"].ToString();
                }

            }
            DataTable dtSupplier;
            DBProxy.Current.Select(null, string.Format(@"select a.Suppid,b.AbbEN from AIR a WITH (NOLOCK) inner join Supp b WITH (NOLOCK) on a.Suppid=b.ID
                                  where a.ID='{0}'", id), out dtSupplier);
            if (dtSupplier.Rows.Count != 0)
            {
                txtsupplier1.TextBox1.Text = dtSupplier.Rows[0]["Suppid"].ToString();
                txtsupplier1.DisplayBox1.Text = dtSupplier.Rows[0]["AbbEN"].ToString();
            }
            else
            {
                txtsupplier1.TextBox1.Text = "";
                txtsupplier1.DisplayBox1.Text = "";
            }
            //串接table Receiving
            DataTable dtRec;
            Ict.DualResult wknoResult;
            if (wknoResult = DBProxy.Current.Select(null, string.Format("select * from Receiving a WITH (NOLOCK) left join AIR b WITH (NOLOCK) on a.Id=b.ReceivingID where b.ID='{0}' ", id), out dtRec))
            {
                if (dtRec.Rows.Count>0)
                {
                    wkno_text.Text = dtRec.Rows[0]["exportid"].ToString();
                }
                else
                {
                    wkno_text.Text = "";
                }
                
            }

            if (MyUtility.Check.Seek(air_cmd, out dr))
            {
                seq_text.Text = dr["SEQ1"].ToString() + " - " + dr["SEQ2"].ToString();
                inspQty_text.Text = dr["inspQty"].ToString();
                RejQty_text.Text = dr["REjectQty"].ToString();
                InsDate_text.Value = MyUtility.Convert.GetDate(dr["inspdate"]);                  
                Instor_text.Text = dr["inspector"].ToString();
                Remark_text.Text = dr["remark"].ToString();
                this.comboBox1.DisplayMember = dr["Result"].ToString();
                this.editBox1.Text = dr["Defect"].ToString();
                this.Arrive_qty_text.Text = dr["ArriveQty"].ToString();
            }
            else
            {
                seq_text.Text = "";
                inspQty_text.Text = "";
                RejQty_text.Text = "";
                InsDate_text.Text = "";
                Instor_text.Text = "";
                Remark_text.Text = "";
                this.comboBox1.DisplayMember = "";
                this.editBox1.Text = "";
                this.Arrive_qty_text.Text = "";
            }

        }

        //++1206
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            if (canedit)
            {
                this.undo.Text = "Close";
            }

        }

        private void save_Click(object sender, EventArgs e)
        {
            string strSqlcmd = "";
            this.Encode.Enabled = false;
            //++1206
            this.undo.Text = "Undo";

            DualResult result;
            DataTable dt;
            strSqlcmd = string.Format("Select * from AIR WITH (NOLOCK) where ID='{0}' ", textID.Text);          
            if (result = DBProxy.Current.Select(null, strSqlcmd, null, out dt))
            {
                if (dt.Rows.Count > 0)
                {
                    if (this.save.Text == "Save")
                    {
                        if (MyUtility.Check.Empty(inspQty_text.Text))
                        {
                            MyUtility.Msg.InfoBox("<Inspected> can not be null");
                            this.inspQty_text.Focus();
                            return;
                        }
                        if (MyUtility.Check.Empty(comboBox1.SelectedValue))
                        {
                            MyUtility.Msg.InfoBox("<Result> can not be null");
                            this.comboBox1.Focus();
                            return;
                        }
                        if (MyUtility.Check.Empty(InsDate_text))
                        {
                            MyUtility.Msg.InfoBox("<Inspdate> can not be null");
                            this.InsDate_text.Focus();
                            return;
                        }
                        if (MyUtility.Check.Empty(Instor_text))
                        {
                            MyUtility.Msg.InfoBox("<Inspector> can not be null");
                            this.Instor_text.Focus();
                            return;

                        }
                        if ((RejQty_text.Text != "0.00" || RejQty_text.Text != "") && (MyUtility.Check.Empty(editBox1)))
                        {
                            MyUtility.Msg.InfoBox("When <Rejected Qty> has any value then <Defect> can not be empty !");
                            this.editBox1.Focus();
                            return;
                        }
                        //if ((editBox1.Text != null || editBox1.Text != "") && (this.RejQty_text.Text == "0.00"))
                        if (( !MyUtility.Check.Empty(editBox1.Text)) && (this.RejQty_text.Text == "0.00"))
                        {
                            MyUtility.Msg.InfoBox("<Rejected Qty> can not be empty ,when <Defect> has not empty ! ");
                            this.RejQty_text.Focus();
                            return;
                        }
                        if ((this.comboBox1.Text.ToString() == "Fail") && (this.RejQty_text.Text == "0.00" || MyUtility.Check.Empty(editBox1)))
                        {
                            MyUtility.Msg.InfoBox("When <Result> is Fail then <Rejected Qty> can not be empty !");
                            this.RejQty_text.Focus();
                            return;
                        }
                        string updatesql = "";
                        #region  寫入實體Table Encode
                        updatesql = string.Format(
                        "Update Air set InspQty= '{0}',RejectQty='{1}',Inspdate = '{2}',Inspector = '{3}',Result= '{4}',Defect='{5}',Remark='{6}' where id ='{7}'",
                        this.inspQty_text.Text, this.RejQty_text.Text,string.Format("{0:yyyy-MM-dd}",InsDate_text.Value) , Instor_text.Text, comboBox1.Text, editBox1.Text, Remark_text.Text,  textID.Text);
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
                                this.Encode.Text = "Encode";
                                this.save.Text = "Edit";
                                this.Encode.Enabled = true;
                               
                            }
                            catch (Exception ex)
                            {
                                _transactionscope.Dispose();
                                ShowErr("Commit transaction error.", ex);
                                return;
                            }
                        }
                        #endregion
                        this.inspQty_text.ReadOnly = true;
                        this.RejQty_text.ReadOnly = true;
                        this.InsDate_text.ReadOnly = true;
                        this.Instor_text.ReadOnly = true;
                        this.comboBox1.ReadOnly = true;
                        this.Remark_text.ReadOnly = true;
                        this.editBox1.ReadOnly = true;
                        this.undo.Text = "Close";
                        return;
                    }
                    else
                    {
                        if (dt.Rows[0]["Status"].ToString().Trim() == "Confirmed")
                        {
                            MyUtility.Msg.InfoBox("It's already Confirmed");
                            this.Encode.Enabled = true;
                            return;
                        }
                        if (dt.Rows[0]["Status"].ToString().Trim() != "Confirmed")
                        {
                            this.inspQty_text.ReadOnly = false;
                            this.RejQty_text.ReadOnly = false;
                            this.InsDate_text.ReadOnly = false;
                            this.Instor_text.ReadOnly = false;
                            this.comboBox1.ReadOnly = false;
                            this.Remark_text.ReadOnly = false;
                            this.editBox1.ReadOnly = false;
                            this.save.Text = "Save";
                            
                            return;
                        }
                    }
                }
            }
            //++1206
            this.undo.Visible = false;
            this.btnClose.Visible = true;
        }

               

        private void Encode_Click(object sender, EventArgs e)
        {
            string updatesql = "";
            string updatesql1 = "";

            if (this.Encode.Text == "Amend")
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
                     loginID, textID.Text);
                                       

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
                            btn_status(this.textID.Text);
                            this.save.Text = "Edit";
                            this.Encode.Text = "Encode";
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
                if (MyUtility.Check.Empty(inspQty_text) || MyUtility.Check.Empty(comboBox1.Text) || MyUtility.Check.Empty(Instor_text) || MyUtility.Check.Empty(InsDate_text))
                {
                    if (MyUtility.Check.Empty(inspQty_text.Text))
                    {
                        MyUtility.Msg.InfoBox("<Inspected> can not be null");
                        this.inspQty_text.Focus();
                        return;
                    }
                    else if (MyUtility.Check.Empty(comboBox1.SelectedValue))
                    {
                        MyUtility.Msg.InfoBox("<Result> can not be null");
                        this.comboBox1.Focus();
                        return;
                    }
                    else if (MyUtility.Check.Empty(InsDate_text))
                    {
                        MyUtility.Msg.InfoBox("<Inspdate> can not be null");
                        this.InsDate_text.Focus();
                        return;
                    }
                    else if (MyUtility.Check.Empty(Instor_text))
                    {
                        MyUtility.Msg.InfoBox("<Inspector> can not be null");
                        this.Instor_text.Focus();
                        return;

                    }
                }

                #region  寫入實體Table Encode
                updatesql = string.Format(
                "Update Air set Status = 'Confirmed',EditDate=CONVERT(VARCHAR(20), GETDATE(), 120),EditName='{0}' where id ='{1}'",
                loginID, textID.Text);
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
                        this.Encode.Text = "Amend";
                        this.save.Text = "Edit";
                        this.save.Enabled = false;
                        
                        btn_status(this.textID.Text);
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
            if (this.editBox1.ReadOnly == true)
            {
                return;
            }
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                string sqlcmd = "select id,description from AccessoryDefect WITH (NOLOCK) ";
                SelectItem2 item = new SelectItem2(sqlcmd, "15,12", null);
                DialogResult result = item.ShowDialog();
                if (result == DialogResult.Cancel) { return; }
                this.editBox1.Text = item.GetSelectedString().Replace(",", "+");


            }
        }
        private void button_enable(bool canedit)
        {

            save.Enabled = (bool)canedit && this.Encode.Text.ToString()=="Encode";
            if (maindr["Result"].ToString() == "Pass")
            {
                Encode.Enabled = (bool)canedit;
               
            }
        
        }

        private void undo_Click(object sender, EventArgs e)
        {
            return;
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
                    this.Encode.Text = "Amend";
                }
                else
                {
                    this.Encode.Text = "Encode";
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
