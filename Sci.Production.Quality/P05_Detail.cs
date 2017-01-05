using Ict.Win;
using System.Data;
using Ict.Win;
using Sci.Data;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Transactions;
using System.Windows.Forms;
using Ict;

namespace Sci.Production.Quality
{
    public partial class P05_Detail : Sci.Win.Subs.Input4
    {
        private string loginID = Sci.Env.User.UserID;
        private string  aa = Sci.Env.User.Keyword;
        private DataRow maindr;
        private string PoID,ID;
        private DataTable dtOven;
        private bool newOven=false;
        private bool isModify = false;  //註記[Test Date][Article][Inspector][Remark]是否修改
        private bool isSee = false;
        bool canEdit = true;
        

        public P05_Detail(bool canedit, string id, string keyvalue2, string keyvalue3, DataRow mainDr,string Poid)
            : base(canedit, id, keyvalue2, keyvalue3)
        {
            InitializeComponent();
            maindr = mainDr;
            PoID = Poid.Trim();
            ID = id.Trim();
            this.canEdit = canedit;
            isSee = true;
            //判斷是否為新資料
            if (MyUtility.Check.Empty(maindr))
            {               
                newOven = true;                
            }
          
        }
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            DataTable dt;
            var result = DBProxy.Current.Select(null, string.Format("select * from oven_detail where id='{0}'", ID), out dt);
            //ＣreateNew open EditMode
            if (dt.Rows.Count == 0 && canEdit)
            {
                //this.EditMode = true;
                this.OnUIConvertToMaintain();                
            }
            this.ToExcel.Enabled = !this.EditMode;
        }

        protected override void OnEditModeChanged()
        {
            DataTable dt;
            var result =DBProxy.Current.Select(null,string.Format("select * from oven_detail where id='{0}'",ID),out dt);
            if (dt.Rows.Count>=1)
            {
                newOven = false;
            }
            
            base.OnEditModeChanged();
            if (isSee)
            {
                this.ToExcel.Enabled =  !this.EditMode;
                this.encode_btn.Enabled = canEdit && !this.EditMode;
            }
            
        }

        protected override Ict.DualResult OnRequery(out System.Data.DataTable datas)
        {
      
            Dictionary<String, String> Result_RowSource = new Dictionary<string, string>();
            Result_RowSource.Add("Pass", "Pass");
            Result_RowSource.Add("Fail", "Fail");
            comboBox1.DataSource = new BindingSource(Result_RowSource, null);
            comboBox1.ValueMember = "Key";
            comboBox1.DisplayMember = "Value";
            #region 表頭設定
                
            Ict.DualResult dResult;
            string cmd = "select * from oven where id=@id";      
                      
            List<SqlParameter> sqm = new List<SqlParameter>();
            sqm.Add(new SqlParameter("@id",ID));
            if (dResult = DBProxy.Current.Select(null, cmd, sqm, out dtOven))
            {
                if (dtOven.Rows.Count > 0)
                {                    
                    this.testno.Text = dtOven.Rows[0]["testno"].ToString();
                    this.poid.Text = dtOven.Rows[0]["POID"].ToString();
                    this.inspdate.Value = Convert.ToDateTime(dtOven.Rows[0]["Inspdate"]);
                    this.article.Text = dtOven.Rows[0]["article"].ToString();
                    this.txtuser1.TextBox1Binding = dtOven.Rows[0]["inspector"].ToString();
                    this.remark.Text = dtOven.Rows[0]["Remark"].ToString();
                    comboBox1.SelectedValue = dtOven.Rows[0]["Result"].ToString();
                    if (dtOven.Rows[0]["Status"].ToString()=="New" || dtOven.Rows[0]["Status"].ToString()=="")
                    {
                        this.encode_btn.Text="Encode";
                        this.save.Enabled = true;
                    }
                    else
                    {
                        this.encode_btn.Text = "Amend";
                        this.save.Enabled = false;
                    }
                       
                }
                else
                {
                    this.testno.Text = "";
                    this.poid.Text = PoID;
                    this.inspdate.Value = null;
                    this.article.Text = "";
                    this.txtuser1.TextBox1Binding = loginID;
                    this.remark.Text = "";
                    this.comboBox1.SelectedValue = "";
                }
            }           
          
            #endregion

            return base.OnRequery(out datas);
        }
        // 重組grid view 
        protected override void OnRequeryPost(System.Data.DataTable datas)
        {
            
            base.OnRequeryPost(datas);
            DataTable dtpo,dtsupp;
            // Gridview新增欄位
            datas.Columns.Add("SCIRefno", typeof(string));
            datas.Columns.Add("Refno", typeof(string));
            datas.Columns.Add("Colorid", typeof(string));
            datas.Columns.Add("SEQ", typeof(string));
            datas.Columns.Add("LastUpdate", typeof(string));
            datas.Columns.Add("Supplier",typeof(string));
            int i = 0;
            //跑迴圈丟值進去
            foreach (DataRow dr in datas.Rows)
            {
                if (datas.Rows.Count<=0)
                {
                    return;
                }   
                List<SqlParameter> spm = new List<SqlParameter>();
                string cmdd = "select * from PO_Supp_Detail where id=@id and seq1=@seq1 and seq2=@seq2 and FabricType='F'";
                spm.Add(new SqlParameter("@id", PoID));
                spm.Add(new SqlParameter("@seq1", datas.Rows[i]["seq1"]));
                spm.Add(new SqlParameter("@seq2", datas.Rows[i]["seq2"]));
                DBProxy.Current.Select(null, cmdd, spm, out dtpo);
                //Suppliers
                List<SqlParameter> spmSupp = new List<SqlParameter>();
                string cmddSupp =
@"SELECT a.ID,a.SuppID,a.SEQ1,a.SuppID+'-'+b.AbbEN as supplier 
from PO_Supp a
left join supp b on a.SuppID=b.ID
where a.ID=@id
and a.seq1=@seq1";
                spmSupp.Add(new SqlParameter("@id", PoID));
                spmSupp.Add(new SqlParameter("@seq1", datas.Rows[i]["seq1"]));
                DBProxy.Current.Select(null, cmddSupp, spmSupp, out dtsupp);
                if (dtpo.Rows.Count <= 0)
                {
                     dr["SCIRefno"] = "";
                     dr["Colorid"] = "";
                }
                else
                {
                    dr["SEQ"] = datas.Rows[i]["seq1"].ToString().PadRight(3,' ') + "-" + datas.Rows[i]["seq2"].ToString().TrimEnd();
                    //dr["SEQ"] = datas.Rows[i]["seq1"].ToString() + "" + datas.Rows[i]["seq2"].ToString();
                    dr["SCIRefno"] = dtpo.Rows[0]["SCIRefno"].ToString();
                    dr["refno"] = dtpo.Rows[0]["refno"].ToString();
                    dr["Colorid"] = dtpo.Rows[0]["Colorid"].ToString();
                    dr["Supplier"] = dtsupp.Rows[0]["supplier"].ToString();
                    dr["LastUpdate"] = datas.Rows[i]["EditName"].ToString() + " - " + datas.Rows[i]["EditDate"].ToString();   
                }
                             
                i++;
            }

        }
        // 限定字串長度
        public string SQlText(string sqlInput, int maxLength)
        {
            if (!MyUtility.Check.Empty(sqlInput))
            {
                sqlInput = sqlInput.Trim();
                if (sqlInput.Length > maxLength)
                {
                    sqlInput = sqlInput.Substring(0, maxLength);
                }
            }
            return sqlInput;
        }

        protected override bool OnGridSetup()
        {            
            DataGridViewGeneratorTextColumnSettings groupCell = new DataGridViewGeneratorTextColumnSettings();         
            Ict.Win.DataGridViewGeneratorMaskedTextColumnSettings seqMskCell = new DataGridViewGeneratorMaskedTextColumnSettings();
            seqMskCell.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
            //DataGridViewGeneratorTextColumnSettings seqCell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings rollCell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings chgCell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings staCell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings resultCell = new DataGridViewGeneratorTextColumnSettings();

            #region groupCell
            //groupCell.EditingValueChanged +=(s,e) =>
            //{
            //    DataRow dr = grid.GetDataRow(e.RowIndex);
            //    var ctl = (Ict.Win.UI.DataGridViewNumericBoxEditingControl)this.grid.EditingControl;                
            //    string groupValue = ctl.EditingControlFormattedValue.ToString();
            //    if (groupValue.ToString().Length > 2)
            //    {
            //        dr["OvenGroup"] = SQlText(groupValue, 2);
            //    }               
            //};
            groupCell.EditingTextChanged += (s, e) =>
            {
                DataRow dr = grid.GetDataRow(e.RowIndex);
                var ctl = (Ict.Win.UI.DataGridViewTextBoxEditingControl)this.grid.EditingControl;
                var groupValue = ctl.EditingControlFormattedValue.ToString();

                int n;
                if (int.TryParse(groupValue, out n))
                {
                    if (groupValue.ToString().Length > 2)
                    {
                        dr["OvenGroup"] = SQlText(groupValue, 2);
                    }
                }
                else
                {
                    dr["OvenGroup"] = "";
                }
               
            };
            groupCell.CellValidating += (s, e) =>
            {
                if (this.EditMode == false) return;
                DataRow dr = grid.GetDataRow(e.RowIndex);
                if (e.FormattedValue.ToString().Length != 2)
                {
                    dr["OvenGroup"] = "0" + e.FormattedValue.ToString();
                }
            };
            #endregion

            #region seqMskCell
            seqMskCell.CellMouseClick += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                if (this.EditMode == false) return;
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    DataRow dr = grid.GetDataRow(e.RowIndex);
                    string item_cmd = string.Format("select RTRIM(seq1) +'-'+ RTRIM(seq2) AS SEQ,scirefno,refno,colorid from PO_Supp_Detail where id='{0}' and FabricType='F'", PoID);
                    SelectItem item = new SelectItem(item_cmd, "5,5,15,12", dr["SEQ"].ToString());
                    DialogResult dresult = item.ShowDialog();
                    if (dresult == DialogResult.Cancel)
                    {
                        return;
                    }
                    Char splitChar = '-';
                    string[] seqSplit = item.GetSelectedString().Split(splitChar);
                    dr["seq1"] = seqSplit[0];
                    dr["seq2"] = seqSplit[1];
                    if (seqSplit[0].ToString().Length <= 2)
                    {
                        seqSplit[0] = seqSplit[0] + " ";
                    }
                    dr["SEQ"] = seqSplit[0] + "-" + seqSplit[1];
                    dr["scirefno"] = item.GetSelecteds()[0]["scirefno"].ToString();
                    dr["refno"] = item.GetSelecteds()[0]["refno"].ToString();
                    dr["colorid"] = item.GetSelecteds()[0]["colorid"].ToString();

                }

            };
            seqMskCell.EditingMouseDown += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                if (this.EditMode == false) return;
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    DataRow dr = grid.GetDataRow(e.RowIndex);
                    string item_cmd = string.Format("select RTRIM(seq1) +'-'+ RTRIM(seq2) AS SEQ,scirefno,refno,colorid from PO_Supp_Detail where id='{0}' and FabricType='F'", PoID);
                    SelectItem item = new SelectItem(item_cmd, "5,5,15,12", dr["SEQ"].ToString());
                    DialogResult dresult = item.ShowDialog();
                    if (dresult == DialogResult.Cancel)
                    {
                        return;
                    }
                    Char splitChar = '-';
                    string[] seqSplit = item.GetSelectedString().Split(splitChar);
                    dr["seq1"] = seqSplit[0];
                    dr["seq2"] = seqSplit[1];
                    if (seqSplit[0].ToString().Length <= 2)
                    {
                        seqSplit[0] = seqSplit[0] + " ";
                    }
                    dr["SEQ"] = seqSplit[0] + "-" + seqSplit[1];
                    dr["scirefno"] = item.GetSelecteds()[0]["scirefno"].ToString();
                    dr["refno"] = item.GetSelecteds()[0]["refno"].ToString();
                    dr["colorid"] = item.GetSelecteds()[0]["colorid"].ToString();

                }

            };
            seqMskCell.CellValidating += (s, e) =>
            {
                if (!this.EditMode) return;
                if (MyUtility.Check.Empty(e.FormattedValue)) { return; }
                DataRow dr = grid.GetDataRow(e.RowIndex);
                DataTable dt;
                DataTable dt1;

                dr["seq1"] = e.FormattedValue.ToString().PadRight(5).Substring(0, 3);
                dr["seq2"] = e.FormattedValue.ToString().PadRight(5).Substring(3, 2);
                if (dr["seq1"].ToString().Length != 3)
                {
                    dr["SEQ"] = dr["seq1"] + " -" + dr["seq2"];
                }
                else
                {
                    dr["SEQ"] = dr["seq1"] + "-" + dr["seq2"];
                }
                // dr["SEQ"] = e.FormattedValue;
                string sql_cmd = string.Format("select seq1,seq2 from PO_Supp_Detail where id='{0}' and FabricType='F' and seq1='{1}' and seq2='{2}'", PoID, dr["seq1"].ToString().Trim(), dr["seq2"].ToString().Trim());
                DBProxy.Current.Select(null, sql_cmd, out dt1);
                if (dt1.Rows.Count <= 0)
                {
                    MyUtility.Msg.InfoBox("<SEQ#> doesn't exist in Data!");
                    dr["SEQ"] = "";
                    dr["seq1"] = "";
                    dr["seq2"] = "";
                    dr["scirefno"] = "";
                    dr["refno"] = "";
                    dr["colorid"] = "";
                    dr.EndEdit();
                    e.Cancel = true; 
                    return;
                }
                dr["SEQ"].ToString().Replace("_", " ");

                DBProxy.Current.Select(null,
                   string.Format("select scirefno,refno,colorid from PO_Supp_Detail where id='{0}' and seq1='{1}' and seq2='{2}' and FabricType='F'", PoID, dr["seq1"], dr["seq2"]), out dt1);
                dr["scirefno"] = dt1.Rows[0]["scirefno"].ToString();
                dr["refno"] = dt1.Rows[0]["refno"].ToString();
                dr["colorid"] = dt1.Rows[0]["colorid"].ToString();

                // SEQ changed 判斷Roll# 是否存在
                if (MyUtility.Check.Empty(dr["Roll"])) return;
                string cmd = "SELECT Roll,Dyelot from FtyInventory where poid=@poid and Seq1=@seq1 and Seq2=@seq2 and Roll=@Roll ";
                List<SqlParameter> spam = new List<SqlParameter>();
                spam.Add(new SqlParameter("@poid", PoID));
                spam.Add(new SqlParameter("@seq1", dr["seq1"]));
                spam.Add(new SqlParameter("@seq2", dr["seq2"]));
                spam.Add(new SqlParameter("@Roll", dr["roll"]));
                DBProxy.Current.Select(null, cmd, spam, out dt);
                if (dt.Rows.Count <= 0)
                {
                    MyUtility.Msg.InfoBox("<Roll> doesn't exist in Data!");
                    dr["Roll"] = "";
                    dr["Dyelot"] = "";
                    return;
                }             

            };
            #endregion

            #region rollCell
            rollCell.CellMouseClick += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                if (this.EditMode == false) return;
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {

                    DataRow dr = grid.GetDataRow(e.RowIndex);
                    #region 新資料 不判斷SEQ
                    //if (newOven) //
                    //{                       
                    //    string item_cmd = "SELECT Roll,Dyelot from FtyInventory where poid=@poid and roll <>'' ";
                    //    List<SqlParameter> spam = new List<SqlParameter>();
                    //    spam.Add(new SqlParameter("@poid", PoID));
                    //    SelectItem item = new SelectItem(item_cmd, spam, "10,10", dr["Roll"].ToString());
                    //    DialogResult dresult = item.ShowDialog();
                    //    if (dresult == DialogResult.Cancel)
                    //    {
                    //        return;
                    //    }
                    //    dr["Roll"] = item.GetSelectedString();
                    //    dr["Dyelot"] = item.GetSelecteds()[0]["Dyelot"].ToString().TrimEnd();

                    //}
                    //else
                    //{
                    #endregion                    

                    string item_cmd = "SELECT DISTINCT Roll,Dyelot from FtyInventory where poid=@poid and Seq1=@seq1 and Seq2=@seq2 order by roll";
                    List<SqlParameter> spam = new List<SqlParameter>();
                    spam.Add(new SqlParameter("@poid", PoID));
                    spam.Add(new SqlParameter("@seq1", dr["seq1"]));
                    spam.Add(new SqlParameter("@seq2", dr["seq2"]));
                    SelectItem item = new SelectItem(item_cmd, spam, "10,10", dr["Roll"].ToString());
                    DialogResult dresult = item.ShowDialog();
                    if (dresult == DialogResult.Cancel)
                    {
                        return;
                    }
                    dr["Roll"] = item.GetSelectedString();
                    dr["Dyelot"] = item.GetSelecteds()[0]["Dyelot"].ToString().TrimEnd();
                  

                }

            };
            rollCell.EditingMouseDown += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                if (this.EditMode == false) return;
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {

                    DataRow dr = grid.GetDataRow(e.RowIndex);
                    #region 新資料 不判斷SEQ
                     //if (newOven) //
                    //{
                    //    string item_cmd = "SELECT Roll,Dyelot from FtyInventory where poid=@poid and roll <>'' ";
                    //    List<SqlParameter> spam = new List<SqlParameter>();
                    //    spam.Add(new SqlParameter("@poid", PoID));
                    //    SelectItem item = new SelectItem(item_cmd, spam, "10,10", dr["Roll"].ToString());
                    //    DialogResult dresult = item.ShowDialog();
                    //    if (dresult == DialogResult.Cancel)
                    //    {
                    //        return;
                    //    }
                    //    dr["Roll"] = item.GetSelectedString();
                    //    dr["Dyelot"] = item.GetSelecteds()[0]["Dyelot"].ToString().TrimEnd();

                    //}
                    //else
                    //{
                    #endregion                   

                    string item_cmd = "SELECT DISTINCT Roll,Dyelot from FtyInventory where poid=@poid and Seq1=@seq1 and Seq2=@seq2 order by roll";
                    List<SqlParameter> spam = new List<SqlParameter>();
                    spam.Add(new SqlParameter("@poid", PoID));
                    spam.Add(new SqlParameter("@seq1", dr["seq1"]));
                    spam.Add(new SqlParameter("@seq2", dr["seq2"]));
                    SelectItem item = new SelectItem(item_cmd, spam, "10,10", dr["Roll"].ToString());
                    DialogResult dresult = item.ShowDialog();
                    if (dresult == DialogResult.Cancel)
                    {
                        return;
                    }
                    dr["Roll"] = item.GetSelectedString();
                    dr["Dyelot"] = item.GetSelecteds()[0]["Dyelot"].ToString().TrimEnd();
                

                }

            };
            rollCell.CellValidating += (s, e) =>
            {
                if (this.EditMode == false) return;
                if (MyUtility.Check.Empty(e.FormattedValue)) { return; }
                DataTable dt;
                DataRow dr = grid.GetDataRow(e.RowIndex);

                #region 新資料 不判斷SEQ
                //if (newOven)//
                //{
                //    string cmd = "SELECT Roll,Dyelot from FtyInventory where poid=@poid and roll <>'' ";
                //    List<SqlParameter> spam = new List<SqlParameter>();
                //    spam.Add(new SqlParameter("@poid", PoID));                   
                //    DBProxy.Current.Select(null, cmd, spam, out dt);
                //    if (dt.Rows.Count <= 0)
                //    {                        
                //        MyUtility.Msg.InfoBox("<Roll> doesn't exist in Data!");
                //        dr["Roll"] = "";
                //        dr["Dyelot"] = "";
                //        return;
                //    }
                //    else
                //    {
                //        List<SqlParameter> spamUpdate = new List<SqlParameter>();
                //        spamUpdate.Add(new SqlParameter("@dyelot", dr["dyelot"]));
                //        DBProxy.Current.Execute(null, "update FtyInventory set dyelot=@dyelot", spamUpdate);
                //    }
                //}
                //else
                //{                
                #endregion
                    
                string cmd = "SELECT Roll,Dyelot from FtyInventory where poid=@poid and Seq1=@seq1 and Seq2=@seq2 and Roll=@Roll ";
                List<SqlParameter> spam = new List<SqlParameter>();
                spam.Add(new SqlParameter("@poid", PoID));
                spam.Add(new SqlParameter("@seq1", dr["seq1"]));
                spam.Add(new SqlParameter("@seq2", dr["seq2"]));
                spam.Add(new SqlParameter("@Roll", e.FormattedValue));
                DBProxy.Current.Select(null, cmd, spam, out dt);
                if (dt.Rows.Count <= 0)
                {
                    MyUtility.Msg.InfoBox("<Roll> doesn't exist in Data!");
                    dr["Roll"] = "";
                    dr["Dyelot"] = "";
                    dr.EndEdit();
                    e.Cancel = true;
                    return;
                }
                else
                {

                    dr["Roll"] = e.FormattedValue;
                    dr["Dyelot"] = dt.Rows[0]["Dyelot"].ToString().Trim();
                }             

            };
            #endregion

            #region chgCell
            chgCell.EditingMouseDown += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                if (this.EditMode == false) return;
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    DataRow dr = grid.GetDataRow(e.RowIndex);
                    string item_cmd = "select id from Scale where Junk=0 ";

                    SelectItem item = new SelectItem(item_cmd, "10", dr["Changescale"].ToString());
                    DialogResult dresult = item.ShowDialog();
                    if (dresult == DialogResult.Cancel)
                    {
                        return;
                    }
                    dr["Changescale"] = item.GetSelectedString();
                }

            };
            chgCell.CellMouseClick += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                if (this.EditMode == false) return;
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    DataRow dr = grid.GetDataRow(e.RowIndex);
                    string item_cmd = "select id from Scale where Junk=0 ";

                    SelectItem item = new SelectItem(item_cmd, "10", dr["Changescale"].ToString());
                    DialogResult dresult = item.ShowDialog();
                    if (dresult == DialogResult.Cancel)
                    {
                        return;
                    }
                    dr["Changescale"] = item.GetSelectedString();
                }

            };
            chgCell.CellValidating += (s, e) =>
            {
                if (this.EditMode == false) return;
                if (MyUtility.Check.Empty(e.FormattedValue)) return;
                DataTable dt;
                DataRow dr = grid.GetDataRow(e.RowIndex);
                string cmd = "select id from Scale where Junk=0  and id=@ChangeScale";
                List<SqlParameter> spam = new List<SqlParameter>();
                spam.Add(new SqlParameter("@ChangeScale", e.FormattedValue));

                DBProxy.Current.Select(null, cmd, spam, out dt);
                if (dt.Rows.Count <= 0)
                {
                    MyUtility.Msg.InfoBox("<Color Change Scal> doesn't exist in Data!");
                    dr["Changescale"] = "";
                    dr.EndEdit();
                    e.Cancel = true;
                    return;
                }
            };
            #endregion

            #region staCell
            staCell.CellMouseClick += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                if (this.EditMode == false) return;
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    DataRow dr = grid.GetDataRow(e.RowIndex);
                    string item_cmd = "select id from Scale where Junk=0 ";

                    SelectItem item = new SelectItem(item_cmd, "10", dr["StainingScale"].ToString());
                    DialogResult dresult = item.ShowDialog();
                    if (dresult == DialogResult.Cancel)
                    {
                        return;
                    }
                    dr["StainingScale"] = item.GetSelectedString();
                }

            };
            staCell.EditingMouseDown += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                if (this.EditMode == false) return;
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    DataRow dr = grid.GetDataRow(e.RowIndex);
                    string item_cmd = "select id from Scale where Junk=0 ";

                    SelectItem item = new SelectItem(item_cmd, "10", dr["StainingScale"].ToString());
                    DialogResult dresult = item.ShowDialog();
                    if (dresult == DialogResult.Cancel)
                    {
                        return;
                    }
                    dr["StainingScale"] = item.GetSelectedString();
                }

            };
            staCell.CellValidating += (s, e) =>
            {
                if (this.EditMode == false) return;
                if (MyUtility.Check.Empty(e.FormattedValue )) return;
                DataTable dt;
                DataRow dr = grid.GetDataRow(e.RowIndex);
                string cmd = "select id from Scale where Junk=0  and id=@StainingScale";
                List<SqlParameter> spam = new List<SqlParameter>();
                spam.Add(new SqlParameter("@StainingScale", e.FormattedValue));

                DBProxy.Current.Select(null, cmd, spam, out dt);
                if (dt.Rows.Count <= 0)
                {
                    MyUtility.Msg.InfoBox("<Color Staining Scale> doesn't exist in Data!");
                    dr["StainingScale"] = "";
                    dr.EndEdit();
                    e.Cancel = true;
                    return;
                }
            };
            #endregion

            #region resultCell
            resultCell.CharacterCasing = CharacterCasing.Normal;

            resultCell.CellMouseDoubleClick += (s, e) =>
            {
                if (this.EditMode == false) return;
                DataRow dr = grid.GetDataRow(e.RowIndex);

                if (dr["result"].ToString().ToUpper() == "PASS")
                {
                    var ctl = (Ict.Win.UI.DataGridViewTextBoxEditingControl)this.grid.EditingControl;
                    dr["result"] = "Fail";
                    ctl.Text = dr["result"].ToString();
                }
                else
                {
                    var ctl = (Ict.Win.UI.DataGridViewTextBoxEditingControl)this.grid.EditingControl;
                    dr["result"] = "Pass";
                    ctl.Text = dr["result"].ToString();
                }
            };
            #endregion
        
            Helper.Controls.Grid.Generator(this.grid)                           
               .Text("OvenGroup", "Group", width: Widths.AnsiChars(5), settings: groupCell)                
                .MaskedText("SEQ", "CCC-CC", "SEQ#", width: Widths.AnsiChars(7), settings: seqMskCell)
                .Text("Roll", header: "Roll#", width: Widths.AnsiChars(10), settings: rollCell)
                .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(5),iseditingreadonly:true)
                .Text("Refno", header: "Refno", width: Widths.AnsiChars(12), iseditingreadonly: true)
                .Text("SCIRefno", header: "SCI Refno", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("Colorid", header: "Color", width: Widths.AnsiChars(8),iseditingreadonly:true)
                .Text("Changescale", header: "Color Change Scale", width: Widths.AnsiChars(16), settings: chgCell)
                .Text("StainingScale", header: "Color Staining Scale", width: Widths.AnsiChars(10), settings: staCell)
                .Text("Result", header: "Result", width: Widths.AnsiChars(8),settings:resultCell,iseditingreadonly:true)
                .Text("Remark", header: "Remark", width: Widths.AnsiChars(30))
                .Text("LastUpdate", header: "LastUpdate", width: Widths.AnsiChars(30), iseditingreadonly: true);
            return true;
        }
        protected override bool OnSaveBefore()
        {
            if (MyUtility.Check.Empty(this.article.Text))
            {
                MyUtility.Msg.InfoBox("<Article> cannot be empty!!");
                this.article.Select();
                return false;
            }
            if (MyUtility.Check.Empty(this.txtuser1.TextBox1.Text))
            {
                MyUtility.Msg.InfoBox("<Inspector> cannot be empty!!");
                return false;
            }
            if (MyUtility.Check.Empty(this.inspdate.Value))
            {
                MyUtility.Msg.InfoBox("<Test Date> cannot be empty!!");
                return false;
            }
            return base.OnSaveBefore();
        }
        protected override Ict.DualResult OnSave()
        {
            DualResult upResult = new DualResult(true);
            string update_cmd = "";
            DataTable dt;

            string sqlcmd=string.Format(@"select Max(TestNO) as MaxNO from oven where poid='{0}'",PoID);
            DBProxy.Current.Select(null,sqlcmd,out dt);
            int testno = MyUtility.Convert.GetInt(dt.Rows[0]["MaxNO"]);
            
            DataRow dr = ((DataTable)gridbs.DataSource).NewRow();
            for (int i = ((DataTable)gridbs.DataSource).Rows.Count; i > 0; i--)
            {                
                dr = ((DataTable)gridbs.DataSource).Rows[i-1];
                //刪除
                if (dr.RowState == DataRowState.Deleted)
                {
                    List<SqlParameter> spamDet = new List<SqlParameter>();
                    update_cmd = "Delete From Oven_Detail Where id =@id and ovenGroup=@ovenGroup and seq1=@seq1 and seq2=@seq2 ";
                    spamDet.Add(new SqlParameter("@id", dr["ID", DataRowVersion.Original]));
                    spamDet.Add(new SqlParameter("@ovenGroup", dr["ovenGroup", DataRowVersion.Original]));

                    string seq1 = dr["SEQ", DataRowVersion.Original].ToString().Split('-')[0].Trim();
                    string seq2 = dr["SEQ", DataRowVersion.Original].ToString().Split('-')[1].Trim();
                    spamDet.Add(new SqlParameter("@seq1", seq1));
                    spamDet.Add(new SqlParameter("@seq2", seq2));

                    upResult = DBProxy.Current.Execute(null, update_cmd, spamDet);
                    continue;
                }

                //ii.刪除空白SEQ 的Oven_Detail資料
                if (MyUtility.Check.Empty(dr["SEQ"]))
                {
                    dr.Delete();
                    continue;
                }
                                
                string Today = DateTime.Now.ToShortDateString();
                
                //新增
                if (dr.RowState==DataRowState.Added)
                {
                    
                    if (newOven)// insert 新資料進oven
                    {
                        string insCmd = @"
SET IDENTITY_INSERT oven ON
insert into Oven(ID,POID,TestNo,InspDate,Article,Result,Status,Inspector,Remark,addName,addDate)
values(@id ,@poid,@testNO,GETDATE(),@Article,'Pass','New',@logid,@remark,@logid,GETDATE())
SET IDENTITY_INSERT oven off";
                        List<SqlParameter> spamAddNew = new List<SqlParameter>();
                        spamAddNew.Add(new SqlParameter("@id", ID));//New ID
                        spamAddNew.Add(new SqlParameter("@poid", PoID));
                        spamAddNew.Add(new SqlParameter("@article", this.article.Text));
                        spamAddNew.Add(new SqlParameter("@logid", loginID));
                        spamAddNew.Add(new SqlParameter("@remark", this.remark.Text));
                        spamAddNew.Add(new SqlParameter("@testNO", testno + 1));
                        upResult= DBProxy.Current.Execute(null, insCmd,spamAddNew);
                    }                
                }
                if (dr.RowState == DataRowState.Modified || isModify)
                {
                    string editCmd = @"update oven set inspdate=@insDate,Article=@Article,Inspector=@insor,remark=@remark , EditName=@EditName , EditDate=@EditDate
                                       where id=@id";
                    List<SqlParameter> spamEdit = new List<SqlParameter>();
                    spamEdit.Add(new SqlParameter("@id", ID));//New ID
                    spamEdit.Add(new SqlParameter("@insDate", this.inspdate.Value));
                    spamEdit.Add(new SqlParameter("@article", this.article.Text));
                    spamEdit.Add(new SqlParameter("@insor", loginID));
                    spamEdit.Add(new SqlParameter("@remark", this.remark.Text));
                    spamEdit.Add(new SqlParameter("@EditName", loginID));
                    spamEdit.Add(new SqlParameter("@EditDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                    upResult= DBProxy.Current.Execute(null, editCmd,spamEdit);
                }
            }

            return base.OnSave();


        }

        protected override void OnInsert()
        {

            DataTable dt;

            DataTable dtGrid = (DataTable)gridbs.DataSource;
            DBProxy.Current.Select(null, string.Format("select * from oven_detail where id='{0}'", ID), out dt);

            DataTable dtGrid_Max = (DataTable)gridbs.DataSource;
            DBProxy.Current.Select(null, string.Format("select  max(OvenGroup) as max from oven_detail where id='{0}'", ID), out dtGrid_Max);
            int Max = MyUtility.Convert.GetInt(dtGrid_Max.Rows[0]["max"]);
            int Data_rows = dt.Rows.Count;
            int Grid_rows = dtGrid.Rows.Count;
            int intTen;//判斷十位數
            base.OnInsert();
            //新增detail
            //if (Data_rows <= 0)
            //{
                if (Grid_rows == 0)//第一筆0
                {
                    dtGrid.Rows[Grid_rows]["OvenGroup"] = "01";
                    intTen = 0;
                    return;
                }
                int group = MyUtility.Convert.GetInt((dtGrid.Rows[Grid_rows - 1]["ovengroup"]).ToString().Substring(1, 1));
                int groupAll = MyUtility.Convert.GetInt((dtGrid.Rows[Grid_rows - 1]["ovengroup"]));


                if (group == 9 &&  groupAll<10)
                {
                    intTen=1;
                    dtGrid.Rows[Grid_rows]["ovengroup"] = intTen.ToString() + "0";
                    return;
                }
                else if (group == 9 && groupAll > 10)
                {
                    if (groupAll==99)
                    {
                        intTen = 0;
                        dtGrid.Rows[Grid_rows]["ovengroup"] = intTen.ToString() + "1";
                        return;
                    }
                    intTen = MyUtility.Convert.GetInt(groupAll.ToString().Substring(0, 1)) + 1;
                    dtGrid.Rows[Grid_rows]["ovengroup"] = intTen.ToString() + "0";
                    return;
                }
                if (groupAll < 9)
                {
                    intTen = 0; 
                     group++;
                     dtGrid.Rows[Grid_rows]["ovengroup"] =  intTen.ToString() + group.ToString();
                }
                else
                {
                    intTen = MyUtility.Convert.GetInt(groupAll.ToString().Substring(0, 1));
                    group++;
                    dtGrid.Rows[Grid_rows]["ovengroup"] = intTen.ToString() + group.ToString();

                }

                //intTen = (groupAll < 9) ? 0 : MyUtility.Convert.GetInt(groupAll.ToString().Substring(0, 1)) + 1; ;
               
          
        }

        #region 表頭Article 右鍵事件: 1.右鍵selectItem 2.判斷validated
        private void article_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.EditMode == false) return;
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                string cmd =
@"select a.Article from Order_Qty a
left join Oven b on a.ID=b.POID
where a.id=@poid
group by a.Article";
                List<SqlParameter> spm = new List<SqlParameter>();
                spm.Add(new SqlParameter("@poid", PoID));
                SelectItem item = new SelectItem(cmd, spm, "12", null, null);
                DialogResult result = item.ShowDialog();
                if (result == DialogResult.Cancel)
                {
                    return;
                    
                }
                this.article.Text = item.GetSelectedString();
            }
        }
       
        private void article_Validated(object sender, EventArgs e)
        {
            if (!this.EditMode || this.article.Text.Empty()) { return; }
            DualResult dresult;
            DataTable dt;
            string cmd = "select * from order_qty where article=@art";
            List<SqlParameter> spm = new List<SqlParameter>();
            spm.Add(new SqlParameter("@art", article.Text));
            if (dresult = DBProxy.Current.Select(null,cmd,spm,out dt))
            {
                if (dt.Rows.Count<=0)
                {
                    MyUtility.Msg.InfoBox("Article doesn't exist in orders");
                    article.Text = "";
                    article.Focus();
                }
            }
            else
            {
                return;
            }
        }
#endregion

        private void encode_btn_Click(object sender, EventArgs e)
        {
            DataTable dt =(DataTable)gridbs.DataSource;            
            bool result = true;

            if (this.encode_btn.Text == "Encode")
            {
                if (dt.Rows.Count <= 0)
	            {
                    MyUtility.Msg.InfoBox("Data is empty please Append first!");
                    return;
	            }

                else
                {
                    foreach (DataRow dr in ((DataTable)gridbs.DataSource).Rows)
                    {
                        if (MyUtility.Check.Empty(dr["ovenGroup"]))
                        {
                            MyUtility.Msg.InfoBox("<Group> can not be empty!");
                            return;
                        }
                        if (MyUtility.Check.Empty(dr["Seq"]))
                        {
                            MyUtility.Msg.InfoBox("<SEQ> can not be empty!");
                            return;
                        }
                        if (MyUtility.Check.Empty(dr["Changescale"]))
                        {
                            MyUtility.Msg.InfoBox("<Color Change Scale> can not be empty!");
                            return;
                        }
                        if (MyUtility.Check.Empty(dr["StainingScale"]))
                        {
                            MyUtility.Msg.InfoBox("<Color Staining Scale> can not be empty!");
                            return;
                        }
                        if (MyUtility.Check.Empty(dr["Result"]))
                        {
                            MyUtility.Msg.InfoBox("<Result> can not be empty!");
                            return;
                        }
                        if (dr["Result"].ToString().Trim().ToUpper() == "FAIL")
                        {
                            result = false;
                            DBProxy.Current.Execute(null, string.Format("update oven set result='Fail',status='Confirmed',editname='{0}',editdate='{1}' where id='{2}'", loginID, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), dr["id"]));

                        }
                        if (dr["Result"].ToString().Trim().ToUpper()== "PASS" && result)
                        {
                            DBProxy.Current.Execute(null, string.Format("update oven set result='Pass',status='Confirmed',editname='{0}',editdate='{1}' where id='{2}'", loginID, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), dr["id"]));
                            
                        }
                    }
                }
              
            }
            // Amend
            else
            {
                DBProxy.Current.Execute(null, string.Format("update oven set result='',status='New',editname='{0}',editdate='{1}' where id='{2}'", loginID, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), dt.Rows[0]["id"]));
            }
            OnRequery();
        }
         
        private void ToExcel_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)gridbs.DataSource;
            string[] columnNames = new string[] { "OvenGroup", "SEQ", "Roll", "Dyelot", "SCIRefno", "Colorid", "Supplier", "Changescale", "StainingScale", "Result","Remark" };
            var ret = Array.CreateInstance(typeof(object), dt.Rows.Count, grid.Columns.Count) as object[,];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow row = dt.Rows[i];
                for (int j = 0; j < columnNames.Length; j++)
                {
                    ret[i, j] = row[columnNames[j]];
                }
            }
            if (dt.Rows.Count==0)
            {
                 MyUtility.Msg.WarningBox("Data not found!");
                 return;
            }
            DataTable dtPo;
            string StyleID;
            string SeasonID;
            string status;
            string BrandID;
            DBProxy.Current.Select(null, string.Format("select * from PO where id='{0}'", PoID), out dtPo);
            if (dtPo.Rows.Count==0)
            {
                 StyleID="";
                 SeasonID="";
                 status="";
                 BrandID="";
            }
            else
            {
                StyleID = dtPo.Rows[0]["StyleID"].ToString();
                SeasonID = dtPo.Rows[0]["SeasonID"].ToString();
                status = dtOven.Rows[0]["status"].ToString();
                BrandID = dtPo.Rows[0]["BrandID"].ToString();
            }
            

            string strXltName = Sci.Env.Cfg.XltPathDir + "\\Quality_P05_Detail_Report.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null) return;
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            worksheet.Cells[1, 2] = this.poid.Text.ToString();
            worksheet.Cells[1, 4] = StyleID;
            worksheet.Cells[1, 6] = dtPo.Rows[0]["SeasonID"].ToString();
            worksheet.Cells[1, 8] = SeasonID;
            worksheet.Cells[1, 10] = this.testno.Text.ToString();
            worksheet.Cells[2, 2] = status;
            worksheet.Cells[2, 4] = this.comboBox1.Text;
            worksheet.Cells[2, 6] = this.inspdate.Text;
            worksheet.Cells[2, 8] = this.txtuser1.TextBox1.Text.ToString();
            worksheet.Cells[2, 10] = BrandID;

            int StartRow = 4;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                worksheet.Cells[StartRow + i, 1] = ret[i, 0];
                worksheet.Cells[StartRow + i, 2] = ret[i, 1];
                worksheet.Cells[StartRow + i, 3] = ret[i, 2];
                worksheet.Cells[StartRow + i, 4] = ret[i, 3];
                worksheet.Cells[StartRow + i, 5] = ret[i, 4];
                worksheet.Cells[StartRow + i, 6] = ret[i, 5];
                worksheet.Cells[StartRow + i, 7] = ret[i, 6];
                worksheet.Cells[StartRow + i, 8] = ret[i, 7];
                worksheet.Cells[StartRow + i, 9] = ret[i, 8];
                worksheet.Cells[StartRow + i, 10] = ret[i, 9];
                worksheet.Cells[StartRow + i, 11] = ret[i, 10];
            }

            worksheet.Cells.EntireColumn.AutoFit();
            worksheet.Cells.EntireRow.AutoFit();

            worksheet.Select();
            MyUtility.Msg.WaitClear();
            excel.Visible = true;

            if (excel != null) Marshal.FinalReleaseComObject(excel); //釋放sheet
            if (worksheet != null) Marshal.FinalReleaseComObject(worksheet); //釋放objApp

        }

        private void TextChanged(object sender, EventArgs e)
        {
            isModify = true;
        }

        private void txtuser1_Validating(object sender, CancelEventArgs e)
        {
            isModify = true;
        }

        


    }
}
