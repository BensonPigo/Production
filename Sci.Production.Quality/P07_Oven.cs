using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using System.Data.SqlClient;
using Sci.Data;


namespace Sci.Production.Quality
{
    public partial class P07_Oven : Sci.Win.Subs.Input2A
    {
        private DataRow maindr;
        private string ID, PoID, SEQ1, SEQ2;
        private bool EDIT;
        string sql;
        DataRow DR;


        public P07_Oven(bool canedit, string id, string Poid, string seq1, string seq2, DataRow mainDr)
        {
            InitializeComponent();
            maindr = mainDr;            
            ID = id.Trim();
            PoID = Poid.Trim();
            SEQ1 = seq1.Trim();
            SEQ2 = seq2.Trim();

            #region 設定可否編輯
            if (!canedit) EDIT = false;
            else
            { 
                EDIT = true;
                SetUpdate(maindr);
            }
            #endregion

        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            #region [comboResult]
            Dictionary<String, String> Result_RowSource = new Dictionary<string, string>();
            Result_RowSource.Add("", "");
            Result_RowSource.Add("Pass", "Pass");
            Result_RowSource.Add("Fail", "Fail");
            comboResult.DataSource = new BindingSource(Result_RowSource, null);
            comboResult.ValueMember = "Key";
            comboResult.DisplayMember = "Value";
            #endregion

            #region [btnEncode]
            this.btnSave.Enabled = false;
            save.Visible = false;
            if (EDIT)
            {
                if (Convert.ToBoolean(maindr["OvenEncode"]))
                {
                    btnEncode.Text = "Amend";
                    btnEncode.Enabled = true;
                }
                else
                {
                    btnEncode.Text = "Encode";
                    btnEncode.Enabled = true;
                    this.btnSave.Enabled = true;
                }
            }
            else
            {
                undo.Visible = false;
                this.btnSave.Visible = false;
                btnClose.Visible = true;

            }
            #endregion
            OnRequery();
        }

        private void OnRequery()
        {
             sql = string.Format(@"select  C.ExportId , B.ArriveQty , E.StockUnit , E.SizeSpec , B.SCIRefno
	                                    , B.Refno , B.Suppid + '-' + D.AbbEN as supplier , E.ColorID
                                    from AIR_Laboratory A
                                    left join AIR B on A.id=B.id
                                    left join Receiving C on C.id=B.receivingID
                                    left join Supp D on D.ID=B.Suppid
                                    left join PO_Supp_Detail E on E.ID=A.POID and E.SEQ1=A.SEQ1 and E.SEQ2=A.SEQ2
                                    where A.id={0} and A.POID='{1}' and A.SEQ1='{2}' and A.SEQ2='{3}'", ID,PoID,SEQ1,SEQ2);
            if (MyUtility.Check.Seek(sql, out DR))
            {
                sp_text.Text = maindr["SEQ1"] + "-" + maindr["SEQ2"];
                WKNO_text.Text = DR["ExportId"].ToString().Trim();
                Qty_text.Value = Convert.ToDecimal(DR["ArriveQty"]);
                Unit_text.Text = DR["StockUnit"].ToString().Trim();
                Size_text.Text = DR["SizeSpec"].ToString().Trim();
                SciRefno.Text = DR["SCIRefno"].ToString().Trim();
                BrandRefno_text.Text = DR["Refno"].ToString().Trim();
                Supplier_text.Text = DR["supplier"].ToString().Trim();
                Color_text.Text = DR["ColorID"].ToString().Trim();
            }

            comboResult.SelectedValue2 = maindr["Oven"].ToString().Trim();

        }

        private void txtScale_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Sci.Win.Forms.Base myForm = (Sci.Win.Forms.Base)this.FindForm();
            if (!EDIT) return;
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem("Select id from Scale where junk=0", "10", this.txtScale.Text);
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel) { return; }
            this.txtScale.Text = item.GetSelectedString();
        }

        private void txtScale_Validating(object sender, CancelEventArgs e)
        {
            string textValue = this.txtScale.Text;
            if (!string.IsNullOrWhiteSpace(textValue) && textValue != this.txtScale.OldValue)
            {
                if (!MyUtility.Check.Seek(string.Format(@"Select id from Scale where junk=0 and id = '{0}'", textValue)))
                {
                    MyUtility.Msg.WarningBox(string.Format("< Scale: {0} > not found!!!", textValue));
                    this.txtScale.Text = "";
                    e.Cancel = true;
                    return;
                }
            }
        }

        //[Encode][Amend]
        private void btnEncode_Click(object sender, EventArgs e)
        {
            if (btnEncode.Text == "Encode")
            {

                #region Valid
                if (MyUtility.Check.Empty(comboResult.SelectedValue2))
                {
                    MyUtility.Msg.WarningBox("[Result] can not be empty !!");
                    return;
                }
                if (MyUtility.Check.Empty(OvenDate.Value))
                {
                    MyUtility.Msg.WarningBox("[Inspect Date] can not be empty !!");
                    return;
                }
                if (MyUtility.Check.Empty(txtuser1.TextBox1.Text))
                {
                    MyUtility.Msg.WarningBox("[Lab Tech] can not be empty !!");
                    return;
                }
                #endregion

                #region ii.填入Air_Laboratory.OvenEncode = T。iv.填入Air_Laboratory.EditName=Userid, Air_Laboratory.EditDate=Datetime()
                sql = string.Format(@"update Air_Laboratory set OvenEncode=1 , editName='{0}', editDate='{1}'
                                      where ID='{2}' and POID='{3}' and SEQ1='{4}' and SEQ2='{5}'"
                                    , Sci.Env.User.UserID, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ID, PoID, SEQ1, SEQ2);
                DBProxy.Current.Execute(null, sql);
                #endregion

                #region iii.Air_Laboratory.WashEncode 為.T.時或Air_Laboratory.NonWash=.T.時，判斷當Air_Laboratory.Oven 與Air_Laboratory.Wash 只要有一個為’Fail’則Air_Laboratory.Result=‘Fail’，否則填入Air_Laboratory.Result=‘Pass’
                if (Convert.ToBoolean(maindr["WashEncode"]) || Convert.ToBoolean(maindr["NonWash"]))
                {
                    if (maindr["Oven"].ToString().Trim() == "Fail" || maindr["Wash"].ToString().Trim() == "Fail")
                    {
                        sql = string.Format(@"update Air_Laboratory set Result='Fail' 
                                      where ID='{0}' and POID='{1}' and SEQ1='{2}' and SEQ2='{3}'"
                                    , ID, PoID, SEQ1, SEQ2);
                        DBProxy.Current.Execute(null, sql);
                    }
                    else
                    {
                        sql = string.Format(@"update Air_Laboratory set Result='Pass' 
                                      where ID='{0}' and POID='{1}' and SEQ1='{2}' and SEQ2='{3}'"
                                    , ID, PoID, SEQ1, SEQ2);
                        DBProxy.Current.Execute(null, sql);
                    }
                }
                #endregion

                btnEncode.Text = "Amend";
                this.btnSave.Enabled = false;

            }
            else if (btnEncode.Text == "Amend")
            {

                #region i.將Air_Laboratory.OvenEncode 改為.F。ii.清空Air_Laboratory.Result。iii.填入Air_Laboratory.EditName=Userid, Air_Laboratory.EditDate=Datetime()
                sql = string.Format(@"update Air_Laboratory set OvenEncode=0 , Result='' ,  editName='{0}', editDate='{1}'
                                      where ID='{2}' and POID='{3}' and SEQ1='{4}' and SEQ2='{5}'"
                                    , Sci.Env.User.UserID, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ID, PoID, SEQ1, SEQ2);
                DBProxy.Current.Execute(null, sql);
                #endregion

                btnEncode.Text = "Encode";
                //btnEncode.Enabled = false;
                this.btnSave.Enabled = true;

            }
        }
        //修改寫法,先保留! 20161105        
        //protected override bool OnSaveBefore()
        //{
        //    if (save.Text == "Edit")
        //    {
        //        txtScale.Enabled = true;
        //        comboResult.Enabled = true;
        //        txtRemark.Enabled = true;
        //        txtuser1.Enabled = true;
        //        OvenDate.Enabled = true;
        //        save.Text = "save";
        //        this.btnEncode.Enabled = false;
        //        return false;
        //    }
        //    else
        //    {
        //        if (MyUtility.Check.Empty(txtuser1.TextBox1.Text))
        //        {
        //            CurrentData["OvenInspector"] = Sci.Env.User.UserID;
        //        }
        //        if (MyUtility.Check.Empty(OvenDate.Value))
        //        {
        //            CurrentData["OvenDate"] = DateTime.Now;
        //        }
        //        txtScale.Enabled = false;
        //        comboResult.Enabled = false;
        //        txtRemark.Enabled = false;
        //        txtuser1.Enabled = false;
        //        OvenDate.Enabled = false;
        //        save.Text = "Edit";
        //        this.btnEncode.Enabled = true;
        //        return true;
        //    }

        //}
     
       
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (this.btnSave.Text == "Edit")
            {
                txtScale.Enabled = true;
                comboResult.Enabled = true;
                txtRemark.Enabled = true;
                txtuser1.Enabled = true;
                OvenDate.Enabled = true;
                this.btnSave.Text = "Save";
                this.btnEncode.Enabled = false;              
                this.btnClose.Visible = false;
                this.undo.Visible = true;
                return ;
            }
            else
            {
                if (MyUtility.Check.Empty(txtuser1.TextBox1.Text))
                {
                    CurrentData["OvenInspector"] = Sci.Env.User.UserID;
                }
                if (MyUtility.Check.Empty(OvenDate.Value))
                {
                    CurrentData["OvenDate"] = DateTime.Now;
                }
                txtScale.Enabled = false;
                comboResult.Enabled = false;
                txtRemark.Enabled = false;
                txtuser1.Enabled = false;
                this.btnClose.Visible = true;
                this.undo.Visible = false;
                OvenDate.Enabled = false;
                string sqlcmd = string.Format(@"update AIR_Laboratory
set OvenScale = '{3}',
Oven='{4}',
OvenRemark='{5}',
OvenInspector='{6}',
OvenDate='{7}'
where POID='{0}' and seq1='{1}' and SEQ2='{2}'",
PoID, SEQ1, SEQ2, txtScale.Text, this.comboResult.Text, txtRemark.Text, txtuser1.TextBox1.Text, ((DateTime)this.OvenDate.Value).ToShortDateString());
                DBProxy.Current.Execute(null, sqlcmd);
                this.btnSave.Text = "Edit";
                this.btnEncode.Enabled = true;
                return ;
            }
          
            
        }

    }
}
