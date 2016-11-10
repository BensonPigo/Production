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
    public partial class P06_Detail : Sci.Win.Subs.Input4
    {
        private string loginID = Sci.Env.User.UserID;
        private DataRow maindr;
        private string PoID, ID;
        private DataTable dtColorFastness;  //dtOven
        private bool newOven = false;
        private bool isModify = false;  //註記[Test Date][Article][Inspector][Remark]是否修改
        private bool isSee = false;
        bool canEdit = true;

        public P06_Detail(bool canedit, string id, string keyvalue2, string keyvalue3, DataRow mainDr,string Poid)
            : base(canedit, id, keyvalue2, keyvalue3)
        {
            InitializeComponent();
            maindr = mainDr;
            PoID = Poid.Trim();
            ID = id.Trim();
            isSee = true;
            this.canEdit = canedit;
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
            string sqlCmd=string.Format(@"select * from ColorFastness_Detail where id='{0}'",ID);
            DBProxy.Current.Select(null, sqlCmd, out dt);
            if (dt.Rows.Count==0 && CanEdit)
            {
                this.OnUIConvertToMaintain();
            }
        }
        protected override void OnEditModeChanged()
        {
            DataTable dt;
            DBProxy.Current.Select(null, string.Format("select * from ColorFastness_Detail where id='{0}'", ID), out dt);

            if (dt.Rows.Count >= 1)
            {
                newOven = false;
            }
            base.OnEditModeChanged();
            if (isSee)
            {
                this.ToExcel.Enabled = canEdit && !this.EditMode;
                this.encode_btn.Enabled = canEdit && !this.EditMode;
            }
        }

        protected override Ict.DualResult OnRequery(out System.Data.DataTable datas)
        {
            Dictionary<String, String> Result_RowSource = new Dictionary<string, string>();
            Result_RowSource.Add("Pass", "Pass");
            Result_RowSource.Add("Fail", "Fail");
            Result_RowSource.Add(" ", " ");
            comboBox1.DataSource = new BindingSource(Result_RowSource, null);
            comboBox1.ValueMember = "Key";
            comboBox1.DisplayMember = "Value";
            
            #region 表頭設定
            Ict.DualResult dResult;
            string cmd = "select * from ColorFastness where id=@id";

            List<SqlParameter> sqm = new List<SqlParameter>();
            sqm.Add(new SqlParameter("@id", ID));
            if (dResult = DBProxy.Current.Select(null, cmd, sqm, out dtColorFastness))
            {
                if (dtColorFastness.Rows.Count > 0)
                {
                    this.testno.Text = dtColorFastness.Rows[0]["testno"].ToString();
                    this.poid.Text = dtColorFastness.Rows[0]["POID"].ToString();
                    this.inspdate.Value = Convert.ToDateTime(dtColorFastness.Rows[0]["Inspdate"]);
                    this.article.Text = dtColorFastness.Rows[0]["article"].ToString();
                    this.txtuser1.TextBox1Binding = dtColorFastness.Rows[0]["inspector"].ToString();
                    this.remark.Text = dtColorFastness.Rows[0]["Remark"].ToString();
                    comboBox1.SelectedValue = dtColorFastness.Rows[0]["Result"].ToString();
                    if (dtColorFastness.Rows[0]["Status"].ToString() == "New" || dtColorFastness.Rows[0]["Status"].ToString() == "")
                    {
                        this.encode_btn.Text = "Encode";
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
                    //this.comboBox1.Text = "";
                    this.comboBox1.SelectedValue = " ";
                }
            }
            #endregion

            return base.OnRequery(out datas);
        }

        // 重組grid view 
        protected override void OnRequeryPost(System.Data.DataTable datas)
        {
            base.OnRequeryPost(datas);
            DataTable dtpo, dtsupp;
            // Gridview新增欄位
            datas.Columns.Add("SCIRefno", typeof(string));
            datas.Columns.Add("Refno", typeof(string));
            datas.Columns.Add("Colorid", typeof(string));
            datas.Columns.Add("SEQ", typeof(string));
            datas.Columns.Add("LastUpdate", typeof(string));
            datas.Columns.Add("Supplier", typeof(string));
            int i = 0;

            #region 跑迴圈丟值進去
            foreach (DataRow dr in datas.Rows)
            {
                if (datas.Rows.Count <= 0)
                {
                    return;
                }
                List<SqlParameter> spm = new List<SqlParameter>();
                string cmdd = "select * from PO_Supp_Detail where id=@id and seq1=@seq1 and seq2=@seq2";
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
                    dr["SEQ"] = datas.Rows[i]["seq1"].ToString().PadRight(3, ' ') + "-" + datas.Rows[i]["seq2"].ToString().TrimEnd();
                    //dr["SEQ"] = datas.Rows[i]["seq1"].ToString() + "- " + datas.Rows[i]["seq2"].ToString();
                    dr["SCIRefno"] = dtpo.Rows[0]["SCIRefno"].ToString();
                    dr["Colorid"] = dtpo.Rows[0]["Colorid"].ToString();
                    dr["Supplier"] = dtsupp.Rows[0]["supplier"].ToString();
                    dr["LastUpdate"] = datas.Rows[i]["EditName"].ToString() + " - " + datas.Rows[i]["EditDate"].ToString();
                }

                i++;
            }
            #endregion

        }

        protected override bool OnGridSetup()
        {

            #region -- seqMskCell
            Ict.Win.DataGridViewGeneratorMaskedTextColumnSettings seqMskCell = new DataGridViewGeneratorMaskedTextColumnSettings();

            seqMskCell.CellMouseClick += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                if (this.EditMode == false) return;
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    DataRow dr = grid.GetDataRow(e.RowIndex);
                    DataTable dt;
                    string item_cmd = string.Format("select seq1 +'-'+ seq2 AS SEQ,scirefno,colorid from PO_Supp_Detail where id='{0}' and FabricType='F'", PoID);                   
                    SelectItem item = new SelectItem(item_cmd, "5,5,15,12", dr["SEQ"].ToString());
                    DialogResult dresult = item.ShowDialog();
                    if (dresult == DialogResult.Cancel)
                    {
                        return;
                    }
                    Char splitChar = '-';
                    string[] seqSplit = item.GetSelectedString().Split(splitChar);
                    if (seqSplit[0].ToString().Length <= 2)
                    {
                        seqSplit[0] = seqSplit[0] + " ";
                    }
                    dr["SEQ"] = seqSplit[0] + "-" + seqSplit[1];                    
                    dr["seq1"] = seqSplit[0];
                    dr["seq2"] = seqSplit[1];

                    DBProxy.Current.Select(null,
                   string.Format("select scirefno,refno,colorid from PO_Supp_Detail where id='{0}' and seq1='{1}' and seq2='{2}' and FabricType='F'", PoID, dr["seq1"], dr["seq2"]), out dt);
                    dr["scirefno"] = dt.Rows[0]["scirefno"].ToString();
                    dr["refno"] = dt.Rows[0]["refno"].ToString();
                    dr["colorid"] = dt.Rows[0]["colorid"].ToString();
                }

            };
            seqMskCell.EditingMouseDown += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                if (this.EditMode == false) return;
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    DataRow dr = grid.GetDataRow(e.RowIndex);
                    DataTable dt;
                    string item_cmd = string.Format("select seq1 +'-'+ seq2 AS SEQ,scirefno,colorid from PO_Supp_Detail where id='{0}' and FabricType='F'", PoID);
                    //string item_cmd = string.Format("select seq1,seq2,scirefno,colorid from PO_Supp_Detail where id='{0}' and FabricType='F'", PoID);
                    SelectItem item = new SelectItem(item_cmd, "5,5,15,12", dr["SEQ"].ToString());
                    DialogResult dresult = item.ShowDialog();
                    if (dresult == DialogResult.Cancel)
                    {
                        return;
                    }
                    Char splitChar = '-';
                    string[] seqSplit = item.GetSelectedString().Split(splitChar);
                    if (seqSplit[0].ToString().Length <= 2)
                    {
                        seqSplit[0] = seqSplit[0] + " ";
                    }
                    dr["SEQ"] = seqSplit[0] + "-" + seqSplit[1];
                    var ctl = (Ict.Win.UI.DataGridViewMaskedTextBoxEditingControl)this.grid.EditingControl;
                    ctl.Text = dr["SEQ"].ToString();
                    dr["seq1"] = seqSplit[0];
                    dr["seq2"] = seqSplit[1];
                    DBProxy.Current.Select(null,
                  string.Format("select scirefno,refno,colorid from PO_Supp_Detail where id='{0}' and seq1='{1}' and seq2='{2}' and FabricType='F'", PoID, dr["seq1"], dr["seq2"]), out dt);
                    dr["scirefno"] = dt.Rows[0]["scirefno"].ToString();
                    dr["refno"] = dt.Rows[0]["refno"].ToString();
                    dr["colorid"] = dt.Rows[0]["colorid"].ToString();
                }

            };

            seqMskCell.CellValidating += (s, e) =>
            {
                if (!this.EditMode) return;
                if (MyUtility.Check.Empty(e.FormattedValue)) { return; }
                DataRow dr = grid.GetDataRow(e.RowIndex);
                DataTable dt;
                string seq1 = e.FormattedValue.ToString().PadRight(5).Substring(0, 3),
                    seq2 = e.FormattedValue.ToString().PadRight(5).Substring(3, 2);

                // dr["SEQ"] = e.FormattedValue;
                string sql_cmd = string.Format("select seq1,seq2 from PO_Supp_Detail where id='{0}' and FabricType='F' and seq1='{1}' and seq2='{2}'", PoID, seq1, seq2);
                DBProxy.Current.Select(null, sql_cmd, out dt);
                if (dt.Rows.Count <= 0)
                {
                    //e.Cancel = true;//將value卡住,沒輸入正確or清空不給離開
                    var ctl = (Ict.Win.UI.DataGridViewMaskedTextBoxEditingControl)this.grid.EditingControl;
                    ctl.Text = "";
                    //e.FormattedValue = "";
                    // this.grid.CurrentCell.Value = "";
                    MyUtility.Msg.InfoBox("<SEQ#> doesn't exist in Data!");
                    dr["seq1"] = "";
                    dr["seq2"] = "";
                    return;
                }
                dr["seq1"] = seq1;
                dr["seq2"] = seq2;
                if (dr["seq1"].ToString().Length != 3)
                {
                    dr["SEQ"] = dr["seq1"] + " -" + dr["seq2"];
                }
                else
                {
                    dr["SEQ"] = dr["seq1"] + "-" + dr["seq2"];
                }
            };
            #endregion

            #region -- rollCell
            DataGridViewGeneratorTextColumnSettings rollCell = new DataGridViewGeneratorTextColumnSettings();
            rollCell.CellMouseClick += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                if (this.EditMode == false) return;
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    DataRow dr = grid.GetDataRow(e.RowIndex);
                    if (newOven) //新資料 不判斷SEQ
                    {
                        string item_cmd = "SELECT Roll,Dyelot from FtyInventory where poid=@poid and roll <>''";
                        List<SqlParameter> spam = new List<SqlParameter>();
                        spam.Add(new SqlParameter("@poid", PoID));
                        SelectItem item = new SelectItem(item_cmd, spam, "10,10", dr["Roll"].ToString());
                        DialogResult dresult = item.ShowDialog();
                        if (dresult == DialogResult.Cancel)
                        {
                            return;
                        }
                        dr["Roll"] = item.GetSelectedString();
                        dr["Dyelot"] = item.GetSelecteds()[0]["Dyelot"].ToString().TrimEnd();

                    }
                    else
                    {
                        string item_cmd = "SELECT Roll,Dyelot from FtyInventory where poid=@poid and Seq1=@seq1 and Seq2=@seq2";
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

                }

            };
            rollCell.EditingMouseDown += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                if (this.EditMode == false) return;
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    DataRow dr = grid.GetDataRow(e.RowIndex);
                    if (newOven) //新資料 不判斷SEQ
                    {
                        string item_cmd = "SELECT Roll,Dyelot from FtyInventory where poid=@poid and roll <>''";
                        List<SqlParameter> spam = new List<SqlParameter>();
                        spam.Add(new SqlParameter("@poid", PoID));
                        SelectItem item = new SelectItem(item_cmd, spam, "10,10", dr["Roll"].ToString());
                        DialogResult dresult = item.ShowDialog();
                        if (dresult == DialogResult.Cancel)
                        {
                            return;
                        }
                        dr["Roll"] = item.GetSelectedString();
                        dr["Dyelot"] = item.GetSelecteds()[0]["Dyelot"].ToString().TrimEnd();

                    }
                    else
                    {
                        string item_cmd = "SELECT Roll,Dyelot from FtyInventory where poid=@poid and Seq1=@seq1 and Seq2=@seq2";
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

                }

            };
            rollCell.CellValidating += (s, e) =>
            {
                if (this.EditMode == false) return;
                if (MyUtility.Check.Empty(e.FormattedValue)) return;
                DataTable dt;
                DataRow dr = grid.GetDataRow(e.RowIndex);

                if (newOven)//新資料 不判斷SEQ
                {
                    string cmd = "SELECT Roll,Dyelot from FtyInventory where poid=@poid and roll <>'' ";
                    List<SqlParameter> spam = new List<SqlParameter>();
                    spam.Add(new SqlParameter("@poid", PoID));
                    DBProxy.Current.Select(null, cmd, spam, out dt);
                    if (dt.Rows.Count <= 0)
                    {
                        var ctl = (Ict.Win.UI.DataGridViewTextBoxEditingControl)this.grid.EditingControl;
                        ctl.Text = "";
                        this.grid.CurrentCell.Value = "";
                        MyUtility.Msg.InfoBox("<Roll> doesn't exist in Data!");
                        dr["Dyelot"] = "";
                        return;

                    }
                    else
                    {
                        List<SqlParameter> spamUpdate = new List<SqlParameter>();
                        spamUpdate.Add(new SqlParameter("@dyelot", dr["dyelot"]));
                        DBProxy.Current.Execute(null, "update FtyInventory set dyelot=@dyelot", spamUpdate);
                    }
                }
                else
                {
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
                        return;
                    }
                    else
                    {
                        dr["Roll"] = e.FormattedValue;
                        dr["Dyelot"] = dt.Rows[0]["Dyelot"].ToString().Trim();
                    }
                }


            };
            #endregion

            #region -- chgCell
            DataGridViewGeneratorTextColumnSettings chgCell = new DataGridViewGeneratorTextColumnSettings();
          
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
                    var ctl = (Ict.Win.UI.DataGridViewTextBoxEditingControl)this.grid.EditingControl;                
                    dr["Changescale"] = item.GetSelectedString();
                    ctl.Text = dr["Changescale"].ToString();  
                    
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
                    return;
                }
            };
            #endregion

            #region -- staCell
            DataGridViewGeneratorTextColumnSettings staCell = new DataGridViewGeneratorTextColumnSettings();

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
                    var ctl = (Ict.Win.UI.DataGridViewTextBoxEditingControl)this.grid.EditingControl;
                    dr["StainingScale"] = item.GetSelectedString();
                    ctl.Text = dr["StainingScale"].ToString();
                    this.grid.InvalidateCell(e.ColumnIndex, e.RowIndex);
                }

            };

            staCell.CellValidating += (s, e) =>
            {
                if (this.EditMode == false) return;
                if (MyUtility.Check.Empty(e.FormattedValue)) return;
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
                    return;
                }
            };
            #endregion

            #region -- resultCell
            DataGridViewGeneratorTextColumnSettings resultCell = new DataGridViewGeneratorTextColumnSettings();


            resultCell.CellMouseDoubleClick += (s, e) =>
            {
                if (this.EditMode == false) return;
                DataRow dr = grid.GetDataRow(e.RowIndex);
                if (dr["result"].ToString() == "PASS") 
                {
                    var ctl = (Ict.Win.UI.DataGridViewTextBoxEditingControl)this.grid.EditingControl;                   
                    dr["result"] = "FAIL";
                    ctl.Text = dr["result"].ToString();
                }
                else
                {
                    var ctl = (Ict.Win.UI.DataGridViewTextBoxEditingControl)this.grid.EditingControl;           
                    dr["result"] = "PASS";
                    ctl.Text = dr["result"].ToString();
                }
            };
            #endregion
            seqMskCell.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
           
            #region MouseClick

            
            #endregion

            #region Valid
          
            
            
            #endregion

            Helper.Controls.Grid.Generator(this.grid)
                .MaskedText("ColorFastnessGroup", "CC", "Body", width: Widths.AnsiChars(5))//, settings: groupCell)
                //.Text("ColorFastnessGroup", "Body", width: Widths.AnsiChars(5))//, settings: groupCell)
                .MaskedText("SEQ", "CCC-CC", "SEQ#", width: Widths.AnsiChars(7), settings: seqMskCell)  
                .Text("Roll", header: "Roll#", width: Widths.AnsiChars(5), settings: rollCell)
                .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("SCIRefno", header: "SCI Refno", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("Colorid", header: "Color", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("Changescale", header: "Color Change Scale", width: Widths.AnsiChars(16), settings: chgCell)
                .Text("StainingScale", header: "Color Staining Scale", width: Widths.AnsiChars(10), settings: staCell)
                .Text("Result", header: "Result", width: Widths.AnsiChars(8), settings: resultCell)
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

            DataRow dr = ((DataTable)gridbs.DataSource).NewRow();
            for (int i = ((DataTable)gridbs.DataSource).Rows.Count; i > 0; i--)
            {
                dr = ((DataTable)gridbs.DataSource).Rows[i - 1];
                //刪除
                if (dr.RowState == DataRowState.Deleted)
                {
                    List<SqlParameter> spamDet = new List<SqlParameter>();
                    update_cmd = "Delete From ColorFastness_Detail Where id =@id and ColorFastnessGroup=@ColorFastnessGroup and seq1=@seq1 and seq2=@seq2 ";
                    spamDet.Add(new SqlParameter("@id", dr["ID", DataRowVersion.Original]));
                    spamDet.Add(new SqlParameter("@ColorFastnessGroup", dr["ColorFastnessGroup", DataRowVersion.Original]));

                    string seq1 = dr["SEQ", DataRowVersion.Original].ToString().Split('-')[0].Trim();
                    string seq2 = dr["SEQ", DataRowVersion.Original].ToString().Split('-')[1].Trim();
                    spamDet.Add(new SqlParameter("@seq1", seq1));
                    spamDet.Add(new SqlParameter("@seq2", seq2));

                    upResult = DBProxy.Current.Execute(null, update_cmd, spamDet);
                    continue;
                }

                //ii.刪除空白SEQ 的ColorFastness_Detail資料
                if (MyUtility.Check.Empty(dr["SEQ"]))
                {
                    dr.Delete();
                    continue;
                }

                string Today = DateTime.Now.ToShortDateString();
                #region 判斷Crocking Result
                DataTable gridDt = (DataTable)gridbs.DataSource;
                DataRow[] ResultAry = gridDt.Select("Result='Fail'");
                string result = "Pass";               
                if (ResultAry.Length > 0) result = "Fail";
                #endregion

                //新增
                if (dr.RowState == DataRowState.Added)
                {

                    if (newOven)  //insert 新資料進ColorFastness
                    {
                        string insCmd = @"                                            
                                            insert into ColorFastness(ID,POID,TestNo,InspDate,Article,Result,Status,Inspector,Remark,addName,addDate)
                                            values(@id ,@poid,'1',GETDATE(),@Article,@Result,'New',@logid,@remark,@logid,GETDATE())";
                        List<SqlParameter> spamAddNew = new List<SqlParameter>();
                        spamAddNew.Add(new SqlParameter("@id", ID));//New ID
                        spamAddNew.Add(new SqlParameter("@poid", PoID));
                        spamAddNew.Add(new SqlParameter("@article", this.article.Text));
                        spamAddNew.Add(new SqlParameter("@logid", loginID));
                        spamAddNew.Add(new SqlParameter("@remark", this.remark.Text));
                        spamAddNew.Add(new SqlParameter("@Result", result));
                        
                        upResult = DBProxy.Current.Execute(null, insCmd, spamAddNew);
                    }
                }
                if (dr.RowState == DataRowState.Modified && isModify)
                {
                    string editCmd = @"update ColorFastness set inspdate=@insDate,Article=@Article,Inspector=@insor,remark=@remark , EditName=@EditName , EditDate=@EditDate,result =@result
                                                       where id=@id";
                    List<SqlParameter> spamEdit = new List<SqlParameter>();
                    spamEdit.Add(new SqlParameter("@id", ID));//New ID
                    spamEdit.Add(new SqlParameter("@insDate", this.inspdate.Value));
                    spamEdit.Add(new SqlParameter("@article", this.article.Text));
                    spamEdit.Add(new SqlParameter("@insor", loginID));
                    spamEdit.Add(new SqlParameter("@remark", this.remark.Text));
                    spamEdit.Add(new SqlParameter("@EditName", loginID));
                    spamEdit.Add(new SqlParameter("@EditDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                    spamEdit.Add(new SqlParameter("@result", result));
                    upResult = DBProxy.Current.Execute(null, editCmd, spamEdit);
                }
            }
            #region 換寫法 暫時保存
            //            foreach (DataRow dr in ((DataTable)gridbs.DataSource).Rows)
//            {
//                //刪除
//                if (dr.RowState == DataRowState.Deleted)
//                {
//                    List<SqlParameter> spamDet = new List<SqlParameter>();
//                    update_cmd = "Delete From ColorFastness_Detail Where id =@id and ColorFastnessGroup=@ColorFastnessGroup and seq1=@seq1 and seq2=@seq2 ";
//                    spamDet.Add(new SqlParameter("@id", dr["ID", DataRowVersion.Original]));
//                    spamDet.Add(new SqlParameter("@ColorFastnessGroup", dr["ColorFastnessGroup", DataRowVersion.Original]));

//                    string seq1 = dr["SEQ", DataRowVersion.Original].ToString().Split('-')[0].Trim();
//                    string seq2 = dr["SEQ", DataRowVersion.Original].ToString().Split('-')[1].Trim();
//                    spamDet.Add(new SqlParameter("@seq1", seq1));
//                    spamDet.Add(new SqlParameter("@seq2", seq2));

//                    upResult = DBProxy.Current.Execute(null, update_cmd, spamDet);
//                    continue;
//                }

//                //ii.刪除空白SEQ 的ColorFastness_Detail資料
//                if (MyUtility.Check.Empty(dr["SEQ"]))
//                {
//                    //List<SqlParameter> spamDet = new List<SqlParameter>();
//                    //update_cmd = "Delete From ColorFastness_Detail Where id =@id and ColorFastnessGroup=@ColorFastnessGroup and seq1='' and seq2='' ";
//                    //spamDet.Add(new SqlParameter("@id", dr["ID", DataRowVersion.Original]));
//                    //spamDet.Add(new SqlParameter("@ColorFastnessGroup", dr["ColorFastnessGroup", DataRowVersion.Original]));
//                    //upResult = DBProxy.Current.Execute(null, update_cmd, spamDet);
//                    dr.Delete();
//                    continue;
//                }

//                string Today = DateTime.Now.ToShortDateString();

//                //新增
//                if (dr.RowState == DataRowState.Added)
//                {

//                    if (newOven)  //insert 新資料進ColorFastness
//                    {
//                        string insCmd = @"
//                            SET IDENTITY_INSERT ColorFastness ON
//                            insert into ColorFastness(ID,POID,TestNo,InspDate,Article,Result,Status,Inspector,Remark,addName,addDate)
//                            values(@id ,@poid,'1',GETDATE(),@Article,'Pass','New',@logid,@remark,@logid,GETDATE())
//                            SET IDENTITY_INSERT ColorFastness off";
//                        List<SqlParameter> spamAddNew = new List<SqlParameter>();
//                        spamAddNew.Add(new SqlParameter("@id", ID));//New ID
//                        spamAddNew.Add(new SqlParameter("@poid", PoID));
//                        spamAddNew.Add(new SqlParameter("@article", this.article.Text));
//                        spamAddNew.Add(new SqlParameter("@logid", loginID));
//                        spamAddNew.Add(new SqlParameter("@remark", this.remark.Text));
//                        upResult = DBProxy.Current.Execute(null, insCmd, spamAddNew);
//                    }
//                }
//                if (dr.RowState == DataRowState.Modified || isModify)
//                {
//                    string editCmd = @"update ColorFastness set inspdate=@insDate,Article=@Article,Inspector=@insor,remark=@remark , EditName=@EditName , EditDate=@EditDate
//                                       where id=@id";
//                    List<SqlParameter> spamEdit = new List<SqlParameter>();
//                    spamEdit.Add(new SqlParameter("@id", ID));//New ID
//                    spamEdit.Add(new SqlParameter("@insDate", this.inspdate.Value));
//                    spamEdit.Add(new SqlParameter("@article", this.article.Text));
//                    spamEdit.Add(new SqlParameter("@insor", loginID));
//                    spamEdit.Add(new SqlParameter("@remark", this.remark.Text));
//                    spamEdit.Add(new SqlParameter("@EditName", loginID));
//                    spamEdit.Add(new SqlParameter("@EditDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
//                    upResult = DBProxy.Current.Execute(null, editCmd, spamEdit);
//                }
            //            }
            #endregion
            return base.OnSave();
        }
       
        // 20161021 新增,讓使用者自行輸入
        protected override void OnInsert()
        {
            DataTable dt;
            DataTable dtGrid = (DataTable)gridbs.DataSource;
            DBProxy.Current.Select(null, string.Format("select * from ColorFastness_Detail where id='{0}'", ID), out dt);
            int rows = dt.Rows.Count;
            int rows1 = dtGrid.Rows.Count;
            base.OnInsert();
            if (rows <= 0)
            {
                if (rows1==0)
                {
                    dtGrid.Rows[rows1]["ColorFastnessGroup"] = 01;
                }
                else
                {
                    int group = MyUtility.Convert.GetInt(dtGrid.Rows[rows1 - 1]["ColorFastnessGroup"]);

                    dtGrid.Rows[rows1]["ColorFastnessGroup"] = group + 1;
                }                
            }
            else
            {
                int group = MyUtility.Convert.GetInt(dtGrid.Rows[rows1 - 1]["ColorFastnessGroup"]);

                dtGrid.Rows[rows1]["ColorFastnessGroup"] = group + 1;
            }
        }

        #region 表頭Article 右鍵事件: 1.右鍵selectItem 2.判斷validated
        private void article_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.EditMode == false) return;
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                string cmd =
                    @"select a.Article from Order_Qty a
                    left join ColorFastness b on a.ID=b.POID
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
            DualResult dresult;
            DataTable dt;
            string cmd = "select * from order_qty where article=@art";
            List<SqlParameter> spm = new List<SqlParameter>();
            spm.Add(new SqlParameter("@art", article.Text));
            if (dresult = DBProxy.Current.Select(null, cmd, spm, out dt))
            {
                if (dt.Rows.Count <= 0)
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
            DataTable dt = (DataTable)gridbs.DataSource;
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
                        if (MyUtility.Check.Empty(dr["ColorFastnessGroup"]))
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
                        if (dr["Result"].ToString().Trim() == "FAIL")
                        {
                            result = false;
                            DBProxy.Current.Execute(null, string.Format("update ColorFastness set result='Fail',status='Confirmed',editname='{0}',editdate='{1}' where id='{2}'", loginID, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), dr["id"]));

                        }
                        if (dr["Result"].ToString().Trim() == "PASS" && result)
                        {
                            DBProxy.Current.Execute(null, string.Format("update ColorFastness set result='Pass',status='Confirmed',editname='{0}',editdate='{1}' where id='{2}'", loginID, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), dr["id"]));

                        }
                    }
                }

            }
            // Amend
            else
            {
                DBProxy.Current.Execute(null, string.Format("update ColorFastness set result='',status='New',editname='{0}',editdate='{1}' where id='{2}'", loginID, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), dt.Rows[0]["id"]));
            }
            OnRequery();
        }

        private void ToExcel_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)gridbs.DataSource;
            string[] columnNames = new string[] { "ColorFastnessGroup", "SEQ", "Roll", "Dyelot", "SCIRefno", "Colorid", "Supplier", "Changescale", "StainingScale", "Result", "Remark" };
            var ret = Array.CreateInstance(typeof(object), dt.Rows.Count, grid.Columns.Count) as object[,];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow row = dt.Rows[i];
                for (int j = 0; j < columnNames.Length; j++)
                {
                    ret[i, j] = row[columnNames[j]];
                }
            }
            DataTable dtPo;
            DualResult sResult;
            if (sResult = DBProxy.Current.Select(null, string.Format("select * from PO where id='{0}'", PoID), out dtPo))
            {
                if (dtPo.Rows.Count <= 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!");
                    return;
                }
            }

            string strXltName = Sci.Env.Cfg.XltPathDir + "\\P06_Detail_Report.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null) return;
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            worksheet.Cells[1, 2] = this.poid.Text.ToString();
            worksheet.Cells[1, 4] = dtPo.Rows[0]["StyleID"].ToString();
            worksheet.Cells[1, 6] = dtPo.Rows[0]["SeasonID"].ToString();
            worksheet.Cells[1, 8] = this.article.Text.ToString();
            worksheet.Cells[1, 10] = this.testno.Text.ToString();
            worksheet.Cells[2, 2] = dtColorFastness.Rows[0]["status"].ToString();
            worksheet.Cells[2, 4] = this.comboBox1.Text;
            worksheet.Cells[2, 6] = this.inspdate.Text;
            worksheet.Cells[2, 8] = this.txtuser1.TextBox1.Text.ToString();
            worksheet.Cells[2, 10] = dtPo.Rows[0]["BrandID"].ToString();

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
