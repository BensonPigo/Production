using Ict.Win;
using System.Data;
using Sci.Data;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Ict;
using Sci.Production.PublicPrg;
using System.Diagnostics;

namespace Sci.Production.Quality
{
    public partial class P06_Detail : Win.Subs.Input4
    {
        private string loginID = Sci.Env.User.UserID;
        private DataRow maindr;
        private DualResult result;
        private string PoID;
        private string ID;
        private DataTable dtColorFastness;  // dtOven
        private bool newOven = false;
        private bool isSee = false;
        bool canEdit = true;

        public P06_Detail(bool canedit, string id, string keyvalue2, string keyvalue3, DataRow mainDr, string Poid)
            : base(canedit, id, keyvalue2, keyvalue3)
        {
            this.InitializeComponent();
            this.maindr = mainDr;
            this.PoID = Poid.Trim();
            this.ID = id.Trim();
            this.isSee = true;
            this.canEdit = canedit;

            // 判斷是否為新資料
            if (MyUtility.Check.Empty(this.maindr))
            {
                this.newOven = true;
            }
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            DataTable dt;
            if (!(this.result = DBProxy.Current.Select(null, $@"select * from ColorFastness_Detail WITH (NOLOCK) where id='{this.ID}'", out dt)))
            {
                this.ShowErr(this.result);
                return;
            }

            if (this.ID.Equals("New") && dt.Rows.Count > 0)
            {
                this.result = DBProxy.Current.Execute(null, "delete ColorFastness where id = 'New';delete ColorFastness_Detail where id = 'New';");
                if (!this.result)
                {
                    this.ShowErr(this.result);
                }
            }

            if (dt.Rows.Count == 0 && this.CanEdit)
            {
                this.OnUIConvertToMaintain();
            }

            this.btnToExcel.Enabled = !this.EditMode;
            this.btnToPDF.Enabled = !this.EditMode;
            this.comboTempt.Text = MyUtility.Check.Empty(this.comboTempt.Text) ? "0" : this.comboTempt.Text;
            this.comboCycle.Text = MyUtility.Check.Empty(this.comboCycle.Text) ? "0" : this.comboCycle.Text;
        }

        protected override void OnEditModeChanged()
        {
            DataTable dt;
            DBProxy.Current.Select(null, string.Format("select * from ColorFastness_Detail WITH (NOLOCK) where id='{0}'", this.ID), out dt);

            if (dt.Rows.Count >= 1)
            {
                this.newOven = false;
            }

            base.OnEditModeChanged();
            if (this.isSee)
            {
                this.btnToExcel.Enabled = !this.EditMode;
                this.btnToPDF.Enabled = !this.EditMode;
                this.btnEncode.Enabled = this.canEdit && !this.EditMode;
            }
        }

        protected override DualResult OnRequery(out DataTable datas)
        {
            Dictionary<string, string> Result_RowSource = new Dictionary<string, string>();
            Result_RowSource.Add("Pass", "Pass");
            Result_RowSource.Add("Fail", "Fail");
            Result_RowSource.Add(" ", " ");
            this.comboResult.DataSource = new BindingSource(Result_RowSource, null);
            this.comboResult.ValueMember = "Key";
            this.comboResult.DisplayMember = "Value";

            #region 表頭設定
            DualResult dResult;
            string cmd = "select * from ColorFastness WITH (NOLOCK) where id=@id";

            List<SqlParameter> sqm = new List<SqlParameter>();
            sqm.Add(new SqlParameter("@id", this.ID));
            if (dResult = DBProxy.Current.Select(null, cmd, sqm, out this.dtColorFastness))
            {
                if (this.dtColorFastness.Rows.Count > 0)
                {
                    this.txtNoofTest.Text = this.dtColorFastness.Rows[0]["testno"].ToString();
                    this.txtSP.Text = this.dtColorFastness.Rows[0]["POID"].ToString();
                    this.dateTestDate.Value = Convert.ToDateTime(this.dtColorFastness.Rows[0]["Inspdate"]);
                    this.txtArticle.Text = this.dtColorFastness.Rows[0]["article"].ToString();
                    this.txtuserInspector.TextBox1Binding = this.dtColorFastness.Rows[0]["inspector"].ToString();
                    this.txtRemark.Text = this.dtColorFastness.Rows[0]["Remark"].ToString();
                    this.comboResult.SelectedValue = this.dtColorFastness.Rows[0]["Result"].ToString();
                    this.comboCycle.Text = this.dtColorFastness.Rows[0]["Cycle"].ToString();
                    this.comboDetergent.Text = this.dtColorFastness.Rows[0]["Detergent"].ToString();
                    this.comboDryProcess.Text = this.dtColorFastness.Rows[0]["Drying"].ToString();
                    this.comboTempt.Text = this.dtColorFastness.Rows[0]["Temperature"].ToString();
                    this.comboMachineUs.Text = this.dtColorFastness.Rows[0]["Machine"].ToString();
                    if (this.dtColorFastness.Rows[0]["Status"].ToString() == "New" || this.dtColorFastness.Rows[0]["Status"].ToString() == string.Empty)
                    {
                        this.btnEncode.Text = "Encode";
                        this.save.Enabled = true;
                    }
                    else
                    {
                        this.btnEncode.Text = "Amend";
                        this.save.Enabled = false;
                    }
                }
                else
                {
                    this.txtNoofTest.Text = string.Empty;
                    this.txtSP.Text = this.PoID;
                    this.dateTestDate.Value = null;
                    this.txtArticle.Text = string.Empty;
                    this.txtuserInspector.TextBox1Binding = this.loginID;
                    this.txtRemark.Text = string.Empty;
                    this.comboResult.Text = " ";
                    this.comboCycle.Text = string.Empty;
                    this.comboDetergent.Text = string.Empty;
                    this.comboDryProcess.Text = string.Empty;
                    this.comboTempt.Text = string.Empty;
                    this.comboMachineUs.Text = string.Empty;
                }
            }
            #endregion

            return base.OnRequery(out datas);
        }

        // 重組grid view
        protected override void OnRequeryPost(DataTable datas)
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
                string cmdd = "select * from PO_Supp_Detail WITH (NOLOCK) where id=@id and seq1=@seq1 and seq2=@seq2";
                spm.Add(new SqlParameter("@id", this.PoID));
                spm.Add(new SqlParameter("@seq1", datas.Rows[i]["seq1"]));
                spm.Add(new SqlParameter("@seq2", datas.Rows[i]["seq2"]));
                DBProxy.Current.Select(null, cmdd, spm, out dtpo);

                // Suppliers
                List<SqlParameter> spmSupp = new List<SqlParameter>();
                string cmddSupp =
                    @"SELECT a.ID,a.SuppID,a.SEQ1,a.SuppID+'-'+b.AbbEN as supplier 
                    from PO_Supp a WITH (NOLOCK) 
                    left join supp b WITH (NOLOCK) on a.SuppID=b.ID
                    where a.ID=@id
                    and a.seq1=@seq1";
                spmSupp.Add(new SqlParameter("@id", this.PoID));
                spmSupp.Add(new SqlParameter("@seq1", datas.Rows[i]["seq1"]));
                DBProxy.Current.Select(null, cmddSupp, spmSupp, out dtsupp);
                if (dtpo.Rows.Count <= 0)
                {
                    dr["SCIRefno"] = string.Empty;
                    dr["Colorid"] = string.Empty;
                    dr["Refno"] = string.Empty;
                }
                else
                {
                    dr["SEQ"] = datas.Rows[i]["seq1"].ToString().PadRight(3, ' ') + "-" + datas.Rows[i]["seq2"].ToString().TrimEnd();

                    // dr["SEQ"] = datas.Rows[i]["seq1"].ToString() + "- " + datas.Rows[i]["seq2"].ToString();
                    dr["SCIRefno"] = dtpo.Rows[0]["SCIRefno"].ToString();
                    dr["Refno"] = dtpo.Rows[0]["Refno"].ToString();
                    dr["Colorid"] = dtpo.Rows[0]["Colorid"].ToString();
                    dr["Supplier"] = dtsupp.Rows[0]["supplier"].ToString();
                    dr["LastUpdate"] = datas.Rows[i]["EditName"].ToString() + " - " + datas.Rows[i]["EditDate"].ToString();
                }

                i++;
            }
            #endregion

        }

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
            #region groupCell
            DataGridViewGeneratorTextColumnSettings groupCell = new DataGridViewGeneratorTextColumnSettings();
            groupCell.EditingTextChanged += (s, e) =>
            {
                DataRow dr = this.grid.GetDataRow(e.RowIndex);

                var ctl = (Ict.Win.UI.DataGridViewTextBoxEditingControl)this.grid.EditingControl;
                string groupValue = ctl.EditingControlFormattedValue.ToString();
                if (groupValue.ToString().Length > 2)
                {
                    dr["ColorFastnessGroup"] = this.SQlText(groupValue, 2);
                }
            };
            groupCell.CellValidating += (s, e) =>
            {
                DataRow dr = this.grid.GetDataRow(e.RowIndex);
                string groupcell = this.SQlText(e.FormattedValue.ToString(), 2);
                dr["ColorFastnessGroup"] = groupcell;
            };
            #endregion

            #region -- seqMskCell
            DataGridViewGeneratorMaskedTextColumnSettings seqMskCell = new DataGridViewGeneratorMaskedTextColumnSettings();

            seqMskCell.CellMouseClick += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                if (this.EditMode == false)
                {
                    return;
                }

                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    DataRow dr = this.grid.GetDataRow(e.RowIndex);
                    string item_cmd = string.Format("select seq1 +'-'+ seq2 AS SEQ,scirefno,refno,colorid from PO_Supp_Detail WITH (NOLOCK) where id='{0}' and FabricType='F'", this.PoID);
                    SelectItem item = new SelectItem(item_cmd, "5,5,15,12", dr["SEQ"].ToString());
                    DialogResult dresult = item.ShowDialog();
                    if (dresult == DialogResult.Cancel)
                    {
                        return;
                    }

                    char splitChar = '-';
                    string[] seqSplit = item.GetSelectedString().Split(splitChar);
                    if (seqSplit[0].ToString().Length <= 2)
                    {
                        seqSplit[0] = seqSplit[0] + " ";
                    }

                    dr["SEQ"] = seqSplit[0] + "-" + seqSplit[1];
                    dr["seq1"] = seqSplit[0];
                    dr["seq2"] = seqSplit[1];

                    dr["scirefno"] = item.GetSelecteds()[0]["scirefno"].ToString();
                    dr["refno"] = item.GetSelecteds()[0]["refno"].ToString();
                    dr["colorid"] = item.GetSelecteds()[0]["colorid"].ToString();
                    dr.EndEdit();
                }
            };
            seqMskCell.EditingMouseDown += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                if (this.EditMode == false)
                {
                    return;
                }

                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    DataRow dr = this.grid.GetDataRow(e.RowIndex);
                    string item_cmd = string.Format("select seq1 +'-'+ seq2 AS SEQ,scirefno,refno,colorid from PO_Supp_Detail WITH (NOLOCK) where id='{0}' and FabricType='F'", this.PoID);
                    SelectItem item = new SelectItem(item_cmd, "5,5,15,12", dr["SEQ"].ToString());
                    DialogResult dresult = item.ShowDialog();
                    if (dresult == DialogResult.Cancel)
                    {
                        return;
                    }

                    char splitChar = '-';
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

                    dr["scirefno"] = item.GetSelecteds()[0]["scirefno"].ToString();
                    dr["refno"] = item.GetSelecteds()[0]["refno"].ToString();
                    dr["colorid"] = item.GetSelecteds()[0]["colorid"].ToString();
                    dr.EndEdit();
                }
            };

            seqMskCell.CellValidating += (s, e) =>
            {
                DataRow dr = this.grid.GetDataRow(e.RowIndex);
                if (!this.EditMode)
                {
                    return; // 非編輯模式
                }

                if (e.RowIndex == -1)
                {
                    return; // 沒東西 return
                }

                if (dr["SEQ"].ToString().Replace("-", string.Empty) == e.FormattedValue.ToString().Replace("-", string.Empty))
                {
                    return;
                }

                if (MyUtility.Check.Empty(e.FormattedValue))
                {
                    dr["seq1"] = string.Empty;
                    dr["seq2"] = string.Empty;
                    dr["SEQ"] = string.Empty;
                    dr["scirefno"] = string.Empty;
                    dr["refno"] = string.Empty;
                    dr["colorid"] = string.Empty;
                    dr.EndEdit();
                    return; // 沒資料 return
                }

                DataTable dt;
                DataTable dt1;
                string seq1 = e.FormattedValue.ToString().PadRight(5).Substring(0, 3),
                    seq2 = e.FormattedValue.ToString().PadRight(5).Substring(3, 2);

                string sql_cmd = string.Format("select seq1,seq2 from PO_Supp_Detail WITH (NOLOCK) where id='{0}' and FabricType='F' and seq1='{1}' and seq2='{2}'", this.PoID, seq1, seq2);

                DBProxy.Current.Select(null, sql_cmd, out dt);
                if (dt.Rows.Count <= 0)
                {
                    // e.Cancel = true;//將value卡住,沒輸入正確or清空不給離開
                    var ctl = (Ict.Win.UI.DataGridViewMaskedTextBoxEditingControl)this.grid.EditingControl;
                    if (ctl != null)
                    {
                        ctl.Text = string.Empty;
                    }

                    dr["seq1"] = string.Empty;
                    dr["seq2"] = string.Empty;
                    dr["SEQ"] = string.Empty;
                    dr["scirefno"] = string.Empty;
                    dr["refno"] = string.Empty;
                    dr["colorid"] = string.Empty;
                    dr.EndEdit();
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("<SEQ#: {0}> doesn't exist in Data!", e.FormattedValue));
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

                DBProxy.Current.Select(
                    null,
                    string.Format("select scirefno,refno,colorid from PO_Supp_Detail WITH (NOLOCK) where id='{0}' and seq1='{1}' and seq2='{2}' and FabricType='F'", this.PoID, dr["seq1"], dr["seq2"]), out dt1);
                dr["scirefno"] = dt1.Rows[0]["scirefno"].ToString();
                dr["refno"] = dt1.Rows[0]["refno"].ToString();
                dr["colorid"] = dt1.Rows[0]["colorid"].ToString();

                // SEQ changed 判斷Roll# 是否存在
                string cmd = "SELECT Roll,Dyelot from FtyInventory WITH (NOLOCK) where poid=@poid and Seq1=@seq1 and Seq2=@seq2 and Roll=@Roll ";
                List<SqlParameter> spam = new List<SqlParameter>();
                spam.Add(new SqlParameter("@poid", this.PoID));
                spam.Add(new SqlParameter("@seq1", dr["seq1"]));
                spam.Add(new SqlParameter("@seq2", dr["seq2"]));
                spam.Add(new SqlParameter("@Roll", dr["Roll"]));
                DBProxy.Current.Select(null, cmd, spam, out dt1);
                if (MyUtility.Check.Empty(dr["Roll"]))
                {
                    return;
                }

                if (dt1.Rows.Count <= 0)
                {
                    dr["Roll"] = string.Empty;
                    dr["Dyelot"] = string.Empty;
                    dr.EndEdit();
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("<Roll: {0}> doesn't exist in Data!", e.FormattedValue));
                    return;
                }

                dr.EndEdit();
            };
            #endregion

            #region -- rollCell
            DataGridViewGeneratorTextColumnSettings rollCell = new DataGridViewGeneratorTextColumnSettings();
            rollCell.CellMouseClick += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                if (this.EditMode == false)
                {
                    return;
                }

                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    DataRow dr = this.grid.GetDataRow(e.RowIndex);
                    string item_cmd = "SELECT DISTINCT Roll,Dyelot from FtyInventory WITH (NOLOCK) where poid=@poid and Seq1=@seq1 and Seq2=@seq2 order by roll,Dyelot";
                    List<SqlParameter> spam = new List<SqlParameter>();
                    spam.Add(new SqlParameter("@poid", this.PoID));
                    spam.Add(new SqlParameter("@seq1", dr["seq1"]));
                    spam.Add(new SqlParameter("@seq2", dr["seq2"]));
                    SelectItem item = new SelectItem(item_cmd, spam, "10,10", dr["Roll"].ToString());
                    DialogResult dresult = item.ShowDialog();
                    if (dresult == DialogResult.Cancel)
                    {
                        return;
                    }

                    dr["Roll"] = item.GetSelectedString();
                    dr["Dyelot"] = item.GetSelecteds()[0]["Dyelot"].ToString().Trim();
                    dr.EndEdit();
                }
            };
            rollCell.EditingMouseDown += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                if (this.EditMode == false)
                {
                    return;
                }

                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    DataRow dr = this.grid.GetDataRow(e.RowIndex);
                    string item_cmd = "SELECT DISTINCT Roll,Dyelot from FtyInventory WITH (NOLOCK) where poid=@poid and Seq1=@seq1 and Seq2=@seq2 order by roll,Dyelot";
                    List<SqlParameter> spam = new List<SqlParameter>();
                    spam.Add(new SqlParameter("@poid", this.PoID));
                    spam.Add(new SqlParameter("@seq1", dr["seq1"]));
                    spam.Add(new SqlParameter("@seq2", dr["seq2"]));
                    SelectItem item = new SelectItem(item_cmd, spam, "10,10", dr["Roll"].ToString());
                    DialogResult dresult = item.ShowDialog();
                    if (dresult == DialogResult.Cancel)
                    {
                        return;
                    }

                    dr["Roll"] = item.GetSelectedString();
                    dr["Dyelot"] = item.GetSelecteds()[0]["Dyelot"].ToString().Trim();
                    dr.EndEdit();
                }
            };
            rollCell.CellValidating += (s, e) =>
            {
                DataRow dr = this.grid.GetDataRow(e.RowIndex);
                string oldvalue = dr["Roll"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (oldvalue.Equals(newvalue))
                {
                    return;
                }

                if (!this.EditMode)
                {
                    return; // 非編輯模式
                }

                if (e.RowIndex == -1)
                {
                    return; // 沒東西 return
                }

                if (MyUtility.Check.Empty(e.FormattedValue)) // 沒填入資料,清空
                {
                    dr["Roll"] = string.Empty;
                    dr["Dyelot"] = string.Empty;
                    dr.EndEdit();
                    return;
                }

                DataTable dt;
                string cmd = "SELECT Roll,Dyelot from FtyInventory WITH (NOLOCK) where poid=@poid and Seq1=@seq1 and Seq2=@seq2 and Roll=@Roll ";
                List<SqlParameter> spam = new List<SqlParameter>();
                spam.Add(new SqlParameter("@poid", this.PoID));
                spam.Add(new SqlParameter("@seq1", dr["seq1"]));
                spam.Add(new SqlParameter("@seq2", dr["seq2"]));
                spam.Add(new SqlParameter("@Roll", e.FormattedValue));
                DBProxy.Current.Select(null, cmd, spam, out dt);
                if (dt.Rows.Count <= 0)
                {
                    dr["Roll"] = string.Empty;
                    dr["Dyelot"] = string.Empty;
                    dr.EndEdit();
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("<Roll: {0}> doesn't exist in Data!", e.FormattedValue));
                    return;
                }
                else
                {
                    dr["Roll"] = e.FormattedValue;
                    dr["Dyelot"] = dt.Rows[0]["Dyelot"].ToString().Trim();
                }

                dr.EndEdit();
            };
            #endregion

            #region -- chgCell
            DataGridViewGeneratorTextColumnSettings chgCell = new DataGridViewGeneratorTextColumnSettings();

            chgCell.CellMouseClick += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                if (this.EditMode == false)
                {
                    return;
                }

                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    DataRow dr = this.grid.GetDataRow(e.RowIndex);
                    string item_cmd = "select id from Scale WITH (NOLOCK) where Junk=0 ";

                    SelectItem item = new SelectItem(item_cmd, "10", dr["Changescale"].ToString());
                    DialogResult dresult = item.ShowDialog();
                    if (dresult == DialogResult.Cancel)
                    {
                        return;
                    }

                    dr["Changescale"] = item.GetSelectedString();
                    dr.EndEdit();
                }
            };

            chgCell.EditingMouseDown += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                if (this.EditMode == false)
                {
                    return;
                }

                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    DataRow dr = this.grid.GetDataRow(e.RowIndex);
                    string item_cmd = "select id from Scale WITH (NOLOCK) where Junk=0 ";

                    SelectItem item = new SelectItem(item_cmd, "10", dr["Changescale"].ToString());
                    DialogResult dresult = item.ShowDialog();
                    if (dresult == DialogResult.Cancel)
                    {
                        return;
                    }

                    var ctl = (Ict.Win.UI.DataGridViewTextBoxEditingControl)this.grid.EditingControl;
                    dr["Changescale"] = item.GetSelectedString();
                    ctl.Text = dr["Changescale"].ToString();
                    dr.EndEdit();
                }
            };

            chgCell.CellValidating += (s, e) =>
            {
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
                    return; // 沒資料 return
                }

                DataTable dt;
                DataRow dr = this.grid.GetDataRow(e.RowIndex);
                string cmd = "select id from Scale WITH (NOLOCK) where Junk=0  and id=@ChangeScale";
                List<SqlParameter> spam = new List<SqlParameter>();
                spam.Add(new SqlParameter("@ChangeScale", e.FormattedValue));

                DBProxy.Current.Select(null, cmd, spam, out dt);
                if (dt.Rows.Count <= 0)
                {
                    dr["Changescale"] = string.Empty;
                    dr.EndEdit();
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("<Color Change Scal: {0}> doesn't exist in Data!", e.FormattedValue));
                    return;
                }
            };
            #endregion

            #region -- staCell
            DataGridViewGeneratorTextColumnSettings staCell = new DataGridViewGeneratorTextColumnSettings();

            staCell.CellMouseClick += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                if (this.EditMode == false)
                {
                    return;
                }

                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    DataRow dr = this.grid.GetDataRow(e.RowIndex);
                    string item_cmd = "select id from Scale WITH (NOLOCK) where Junk=0 ";

                    SelectItem item = new SelectItem(item_cmd, "10", dr["StainingScale"].ToString());
                    DialogResult dresult = item.ShowDialog();
                    if (dresult == DialogResult.Cancel)
                    {
                        return;
                    }

                    dr["StainingScale"] = item.GetSelectedString();
                    dr.EndEdit();
                }
            };
            staCell.EditingMouseDown += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                if (this.EditMode == false)
                {
                    return;
                }

                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    DataRow dr = this.grid.GetDataRow(e.RowIndex);
                    string item_cmd = "select id from Scale WITH (NOLOCK) where Junk=0 ";

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
                    dr.EndEdit();
                }
            };

            staCell.CellValidating += (s, e) =>
            {
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
                    return; // 沒資料 return
                }

                DataTable dt;
                DataRow dr = this.grid.GetDataRow(e.RowIndex);
                string cmd = "select id from Scale WITH (NOLOCK) where Junk=0  and id=@StainingScale";
                List<SqlParameter> spam = new List<SqlParameter>();
                spam.Add(new SqlParameter("@StainingScale", e.FormattedValue));

                DBProxy.Current.Select(null, cmd, spam, out dt);
                if (dt.Rows.Count <= 0)
                {
                    dr["StainingScale"] = string.Empty;
                    dr.EndEdit();
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("<Color Staining Scale: {0}> doesn't exist in Data!", e.FormattedValue));
                    return;
                }
            };
            #endregion

            #region Result All
            DataGridViewGeneratorTextColumnSettings resultChangeCell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings resultStainCell = new DataGridViewGeneratorTextColumnSettings();
            resultStainCell.CellMouseDoubleClick += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                DataRow dr = this.grid.GetDataRow(e.RowIndex);
                if (dr["ResultStain"].ToString().ToUpper() == "PASS")
                {
                    dr["ResultStain"] = "Fail";
                }
                else
                {
                    dr["ResultStain"] = "Pass";
                }

                if (dr["ResultChange"].ToString().ToUpper() == "PASS" && dr["ResultStain"].ToString().ToUpper() == "PASS")
                {
                    dr["Result"] = "Pass";
                }
                else
                {
                    dr["Result"] = "Fail";
                }

                dr.EndEdit();
            };

            resultChangeCell.CellMouseDoubleClick += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                DataRow dr = this.grid.GetDataRow(e.RowIndex);
                if (dr["ResultChange"].ToString().ToUpper() == "PASS")
                {
                    dr["ResultChange"] = "Fail";
                }
                else
                {
                    dr["ResultChange"] = "Pass";
                }

                if (dr["ResultChange"].ToString().ToUpper() == "PASS" && dr["ResultStain"].ToString().ToUpper() == "PASS")
                {
                    dr["Result"] = "Pass";
                }
                else
                {
                    dr["Result"] = "Fail";
                }

                dr.EndEdit();
            };
            #endregion

            DataGridViewGeneratorTextColumnSettings resultCell = Sci.Production.PublicPrg.Prgs.cellResult.GetGridCell();
            seqMskCell.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;

            this.Helper.Controls.Grid.Generator(this.grid)
                .Date("SubmitDate", "Submit Date", width: Widths.AnsiChars(8))
                .Text("ColorFastnessGroup", "Body", width: Widths.AnsiChars(5), settings: groupCell)
                .MaskedText("SEQ", "CCC-CC", "SEQ#", width: Widths.AnsiChars(7), settings: seqMskCell)
                .Text("Roll", header: "Roll#", width: Widths.AnsiChars(10), settings: rollCell)
                .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("Refno", header: "Refno", width: Widths.AnsiChars(12), iseditingreadonly: true)
                .Text("SCIRefno", header: "SCI Refno", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("Colorid", header: "Color", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("Result", header: "Result", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("Changescale", header: "Color Change Scale", width: Widths.AnsiChars(16), settings: chgCell)
                .Text("ResultChange", header: "Result(Change)", width: Widths.AnsiChars(8), settings: resultChangeCell, iseditingreadonly: true)
                .Text("StainingScale", header: "Color Staining Scale", width: Widths.AnsiChars(10), settings: staCell)
                .Text("ResultStain", header: "Result(Staining)", width: Widths.AnsiChars(8), settings: resultStainCell, iseditingreadonly: true)
                .Text("Remark", header: "Remark", width: Widths.AnsiChars(30))
                .Text("LastUpdate", header: "LastUpdate", width: Widths.AnsiChars(30), iseditingreadonly: true);
            return true;
        }

        protected override bool OnSaveBefore()
        {
            if (MyUtility.Check.Empty(this.txtArticle.Text))
            {
                this.txtArticle.Select();
                MyUtility.Msg.WarningBox("<Article> cannot be empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(this.txtuserInspector.TextBox1.Text))
            {
                MyUtility.Msg.WarningBox("<Inspector> cannot be empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(this.dateTestDate.Value))
            {
                MyUtility.Msg.WarningBox("<Test Date> cannot be empty!!");
                return false;
            }

            foreach (DataRow dr in ((DataTable)this.gridbs.DataSource).Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    if (MyUtility.Check.Empty(dr["ResultChange"]) || MyUtility.Check.Empty(dr["ResultStain"]))
                    {
                        MyUtility.Msg.WarningBox(" Result(Change)/ Result(Staining) can not be empty");
                        return false;
                    }
                }
            }

            return base.OnSaveBefore();
        }

        protected override DualResult OnSave()
        {
            DualResult upResult;
            string update_cmd = string.Empty;
            DataTable dt;
            DBProxy.Current.Select(null, $@"select Max(testno) as testMaxNo from ColorFastness WITH (NOLOCK) where poid='{this.PoID}'", out dt);
            int testMaxNo = MyUtility.Convert.GetInt(dt.Rows[0]["testMaxNo"]);
            int Temperature = MyUtility.Check.Empty(this.comboTempt.Text) ? 0 : MyUtility.Convert.GetInt(this.comboTempt.Text);
            int Cycle = MyUtility.Check.Empty(this.comboCycle.Text) ? 0 : MyUtility.Convert.GetInt(this.comboCycle.Text);
            string Detergent = MyUtility.Check.Empty(this.comboDetergent.Text) ? string.Empty : this.comboDetergent.Text;
            string Machine = MyUtility.Check.Empty(this.comboMachineUs.Text) ? string.Empty : this.comboMachineUs.Text;
            string Drying = MyUtility.Check.Empty(this.comboDryProcess.Text) ? string.Empty : this.comboDryProcess.Text;

            DataRow dr = ((DataTable)this.gridbs.DataSource).NewRow();
            for (int i = ((DataTable)this.gridbs.DataSource).Rows.Count; i > 0; i--)
            {
                dr = ((DataTable)this.gridbs.DataSource).Rows[i - 1];

                // 刪除
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

                // ii.刪除空白SEQ 的ColorFastness_Detail資料
                if (MyUtility.Check.Empty(dr["SEQ"]))
                {
                    dr.Delete();
                    continue;
                }

                string Today = DateTime.Now.ToShortDateString();

                // 新增
                if (dr.RowState == DataRowState.Added)
                {
                    if (this.newOven) // insert 新資料進ColorFastness
                    {
                        this.ID = MyUtility.GetValue.GetID(Sci.Env.User.Keyword + "CF", "ColorFastness", DateTime.Today, 2, "ID", null);
                        this.KeyValue1 = this.ID;
                        string insCmd = @"                                            
            insert into ColorFastness(ID,POID,TestNo,InspDate,Article,Status,Inspector,Remark,addName,addDate,Temperature,Cycle,Detergent,Machine,Drying)
            values(@id ,@poid,@Testno,GETDATE(),@Article,'New',@logid,@remark,@logid,GETDATE(),@Temperature,@Cycle,@Detergent,@Machine,@Drying)";
                        List<SqlParameter> spamAddNew = new List<SqlParameter>();
                        spamAddNew.Add(new SqlParameter("@id", this.ID)); // New ID
                        spamAddNew.Add(new SqlParameter("@poid", this.PoID));
                        spamAddNew.Add(new SqlParameter("@article", this.txtArticle.Text));
                        spamAddNew.Add(new SqlParameter("@logid", this.loginID));
                        spamAddNew.Add(new SqlParameter("@remark", this.txtRemark.Text));
                        spamAddNew.Add(new SqlParameter("@Testno", testMaxNo + 1));
                        spamAddNew.Add(new SqlParameter("@Temperature", Temperature));
                        spamAddNew.Add(new SqlParameter("@Cycle", Cycle));
                        spamAddNew.Add(new SqlParameter("@Detergent", Detergent));
                        spamAddNew.Add(new SqlParameter("@Machine", Machine));
                        spamAddNew.Add(new SqlParameter("@Drying", Drying));
                        upResult = DBProxy.Current.Execute(null, insCmd, spamAddNew);
                        if (upResult)
                        {
                            this.newOven = false;
                        }
                        else
                        {
                            this.ShowErr(upResult);
                            return upResult;
                        }
                    }
                }
            }

            string editCmd = $@"
update ColorFastness 
set 
inspdate='{this.dateTestDate.Text}'
,Article='{this.txtArticle.Text}'
,Inspector='{this.loginID}'
,remark='{this.txtRemark.Text}'
,EditName='{this.loginID}'
,EditDate='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}'
,Temperature={Temperature}
,Cycle={Cycle}
,Detergent='{Detergent}'
,Machine='{Machine}'
,Drying='{Drying}'
where id='{this.ID}'";

            if (!(upResult = DBProxy.Current.Execute(null, editCmd)))
            {
                this.ShowErr(upResult);
            }

            // 更新PO.FIRLabInspPercent
            if (!(upResult = DBProxy.Current.Execute(null, $"exec UpdateInspPercent 'LabColorFastness','{this.PoID}'")))
            {
                this.ShowErr(upResult);
            }

            return base.OnSave();
        }

        #region 表頭Article 右鍵事件: 1.右鍵selectItem 2.判斷validated
        private void txtArticle_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.EditMode == false)
            {
                return;
            }

            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                string cmd =
                    @"select distinct oq.article 
                    from orders o, order_qty oq 
                    where o.id = oq.id and oq.qty > 0 
                    and o.poid =@poid ";
                List<SqlParameter> spm = new List<SqlParameter>();
                spm.Add(new SqlParameter("@poid", this.PoID));
                SelectItem item = new SelectItem(cmd, spm, "12", null, null);
                DialogResult result = item.ShowDialog();
                if (result == DialogResult.Cancel)
                {
                    return;
                }

                this.txtArticle.Text = item.GetSelectedString();
            }
        }

        #endregion

        private void btnEncode_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)this.gridbs.DataSource;
            bool result = true;

            if (this.btnEncode.Text == "Encode")
            {
                if (dt.Rows.Count <= 0)
                {
                    MyUtility.Msg.WarningBox("Data is empty please Append first!");
                    return;
                }
                else
                {
                    foreach (DataRow dr in ((DataTable)this.gridbs.DataSource).Rows)
                    {
                        if (dr.RowState != DataRowState.Deleted)
                        {
                            if (MyUtility.Check.Empty(dr["ColorFastnessGroup"]))
                            {
                                MyUtility.Msg.WarningBox("<Group> can not be empty!");
                                return;
                            }

                            if (MyUtility.Check.Empty(dr["Seq"]))
                            {
                                MyUtility.Msg.WarningBox("<SEQ> can not be empty!");
                                return;
                            }

                            if (MyUtility.Check.Empty(dr["Changescale"]))
                            {
                                MyUtility.Msg.WarningBox("<Color Change Scale> can not be empty!");
                                return;
                            }

                            if (MyUtility.Check.Empty(dr["StainingScale"]))
                            {
                                MyUtility.Msg.WarningBox("<Color Staining Scale> can not be empty!");
                                return;
                            }

                            if (MyUtility.Check.Empty(dr["Result"]))
                            {
                                MyUtility.Msg.WarningBox("<Result> can not be empty!");
                                return;
                            }

                            if (dr["Result"].ToString().Trim().ToUpper() == "FAIL")
                            {
                                result = false;
                                DBProxy.Current.Execute(null, string.Format("update ColorFastness set result='Fail',status='Confirmed',editname='{0}',editdate='{1}' where id='{2}'", this.loginID, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), dr["id"]));
                            }

                            if (dr["Result"].ToString().Trim().ToUpper() == "PASS" && result)
                            {
                                DBProxy.Current.Execute(null, string.Format("update ColorFastness set result='Pass',status='Confirmed',editname='{0}',editdate='{1}' where id='{2}'", this.loginID, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), dr["id"]));
                            }
                        }
                    }
                }

                string sqlcmd = string.Empty;

                if (!MyUtility.Check.Empty(sqlcmd))
                {
                    DualResult dresult = DBProxy.Current.Execute(null, sqlcmd);
                    if (!dresult)
                    {
                        this.ShowErr(dresult);
                    }
                }
            }

            // Amend
            else
            {
                DBProxy.Current.Execute(null, string.Format("update ColorFastness set result='',status='New',editname='{0}',editdate='{1}' where id='{2}'", this.loginID, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), dt.Rows[0]["id"]));
            }

            // 更新PO.FIRLabInspPercent
            DualResult result_check;
            if (!(result_check = DBProxy.Current.Execute(null, $"exec UpdateInspPercent 'LabColorFastness','{this.PoID}'")))
            {
                this.ShowErr(result_check);
            }

            this.OnRequery();
        }

        private void btnToExcel_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)this.gridbs.DataSource;
            string[] columnNames = new string[] { "ColorFastnessGroup", "SEQ", "Roll", "Dyelot", "SCIRefno", "Colorid", "Supplier", "Changescale", "StainingScale", "Result", "Remark" };
            var ret = Array.CreateInstance(typeof(object), dt.Rows.Count, this.grid.Columns.Count) as object[,];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow row = dt.Rows[i];
                for (int j = 0; j < columnNames.Length; j++)
                {
                    ret[i, j] = row[columnNames[j]];
                }
            }

            if (dt.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return;
            }

            string StyleID = string.Empty;
            string SeasonID = string.Empty;
            string BrandID = string.Empty;
            DataTable dtPo;
            DBProxy.Current.Select(null, string.Format("select * from PO WITH (NOLOCK) where id='{0}'", this.PoID), out dtPo);
            if (dtPo.Rows.Count > 0)
            {
                StyleID = dtPo.Rows[0]["StyleID"].ToString();
                SeasonID = dtPo.Rows[0]["SeasonID"].ToString();
                BrandID = dtPo.Rows[0]["BrandID"].ToString();
            }

            string strXltName = Sci.Env.Cfg.XltPathDir + "\\Quality_P06_Detail_Report.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return;
            }

            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            worksheet.Cells[1, 2] = this.txtSP.Text.ToString();
            worksheet.Cells[1, 4] = StyleID;
            worksheet.Cells[1, 6] = SeasonID;
            worksheet.Cells[1, 8] = this.txtArticle.Text.ToString();
            worksheet.Cells[1, 10] = this.txtNoofTest.Text.ToString();
            worksheet.Cells[2, 2] = this.dtColorFastness.Rows.Count > 0 ? this.dtColorFastness.Rows[0]["status"].ToString() : string.Empty;
            worksheet.Cells[2, 4] = this.comboResult.Text;
            worksheet.Cells[2, 6] = this.dateTestDate.Text;
            worksheet.Cells[2, 8] = this.txtuserInspector.TextBox1.Text.ToString();
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

            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Quality_P06_Detail_Report");
            excel.ActiveWorkbook.SaveAs(strExcelName);
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(worksheet);

            strExcelName.OpenFile();
            #endregion
        }

        private void btnToPDF_Click(object sender, EventArgs e)
        {
            DataTable dtSubDate;
            if (!(this.result = DBProxy.Current.Select(null, $@"select distinct CONVERT(varchar(100), SubmitDate, 111) as SubmitDate from ColorFastness_Detail WITH (NOLOCK) where id='{this.ID}'", out dtSubDate)))
            {
                this.ShowErr(this.result);
                return;
            }

            if (dtSubDate.Rows.Count < 1)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return;
            }

            DataTable dtOrders;
            string StyleID, SeasonID, CustPONo, BrandID, StyleUkey;
            if (!(this.result = DBProxy.Current.Select(null, $@"select * from Orders WITH (NOLOCK) where poid='{this.PoID}'", out dtOrders)))
            {
                this.ShowErr(this.result);
                return;
            }

            if (dtOrders.Rows.Count == 0)
            {
                StyleID = string.Empty;
                SeasonID = string.Empty;
                CustPONo = string.Empty;
                BrandID = string.Empty;
                StyleUkey = string.Empty;
            }
            else
            {
                StyleID = dtOrders.Rows[0]["StyleID"].ToString();
                StyleUkey = dtOrders.Rows[0]["StyleUkey"].ToString();
                SeasonID = dtOrders.Rows[0]["SeasonID"].ToString();
                CustPONo = dtOrders.Rows[0]["CustPONo"].ToString();
                BrandID = dtOrders.Rows[0]["BrandID"].ToString();
            }

            string strXltName = Sci.Env.Cfg.XltPathDir + "\\Quality_P06_Detail_Report_ToPDF.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return;
            }

            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            for (int c = 1; c < dtSubDate.Rows.Count; c++)
            {
                Microsoft.Office.Interop.Excel.Worksheet worksheetFirst = excel.ActiveWorkbook.Worksheets[1];
                Microsoft.Office.Interop.Excel.Worksheet worksheetn = excel.ActiveWorkbook.Worksheets[1 + c];
                worksheetFirst.Copy(worksheetn);
            }

            int nSheet = 1;
            for (int i = 0; i < dtSubDate.Rows.Count; i++)
            {
                worksheet = excel.ActiveWorkbook.Worksheets[nSheet];
                worksheet.Cells[3, 4] = dtSubDate.Rows[i]["submitDate"].ToString();
                worksheet.Cells[3, 7] = this.dateTestDate.Text;
                worksheet.Cells[3, 9] = this.txtSP.Text;
                worksheet.Cells[3, 12] = BrandID;

                worksheet.Cells[5, 4] = StyleID;
                worksheet.Cells[5, 10] = CustPONo;
                worksheet.Cells[5, 12] = this.txtArticle.Text;
                worksheet.Cells[6, 4] = Convert.ToString(MyUtility.GetValue.Lookup($@"select StyleName from Style WITH (NOLOCK) where ukey='{StyleUkey}'", null));
                worksheet.Cells[6, 10] = SeasonID;

                worksheet.Cells[9, 4] = MyUtility.Check.Empty(this.comboTempt.Text) ? "0" : this.comboTempt.Text + "˚C";
                worksheet.Cells[9, 7] = MyUtility.Check.Empty(this.comboCycle.Text) ? "0" : this.comboCycle.Text;
                worksheet.Cells[9, 9] = this.comboDetergent.Text;
                worksheet.Cells[10, 4] = this.comboMachineUs.Text;
                worksheet.Cells[10, 7] = this.comboDryProcess.Text;
                worksheet.Cells[72, 8] = MyUtility.GetValue.Lookup("Name", this.txtuserInspector.TextBox1Binding, "Pass1", "ID");

                DataTable dtBody = (DataTable)this.gridbs.DataSource;
                DataRow[] dr = dtBody.Select(MyUtility.Check.Empty(dtSubDate.Rows[i]["submitDate"]) ? $@"submitDate is null" : $"submitDate = '{dtSubDate.Rows[i]["submitDate"]}'");
                for (int ii = 1; ii < dr.Length; ii++)
                {
                    Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range("A13:A13", Type.Missing).EntireRow;
                    rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                    Marshal.ReleaseComObject(rngToInsert);
                }

                int k = 0;
                foreach (DataRow row in dr)
                {
                    Microsoft.Office.Interop.Excel.Range rang = worksheet.Range[worksheet.Cells[2][13 + k], worksheet.Cells[12][13 + k]];
                    rang.NumberFormat = "@";
                    worksheet.Cells[13 + k, 2] = row["ColorFastnessGroup"];
                    worksheet.Cells[13 + k, 3] = row["SEQ"].ToString();
                    worksheet.Cells[13 + k, 4] = row["Roll"];
                    worksheet.Cells[13 + k, 5] = row["Dyelot"].ToString();
                    worksheet.Cells[13 + k, 6] = row["Refno"];
                    worksheet.Cells[13 + k, 7] = row["Colorid"];
                    worksheet.Cells[13 + k, 8] = row["changeScale"].ToString();
                    worksheet.Cells[13 + k, 9] = row["ResultChange"];
                    worksheet.Cells[13 + k, 10] = row["StainingScale"].ToString();
                    worksheet.Cells[13 + k, 11] = row["ResultStain"];
                    worksheet.Cells[13 + k, 12] = row["Remark"].ToString().Trim();
                    rang.Font.Bold = false;
                    rang.Font.Size = 12;

                    // 水平,垂直置中
                    rang.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                    rang.VerticalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                    k++;
                }

                nSheet++;
            }
            #region Save & Show Excel
            string strFileName = Sci.Production.Class.MicrosoftFile.GetName("Quality_P06_Detail_Report_ToPDF");
            string strPDFFileName = Sci.Production.Class.MicrosoftFile.GetName("Quality_P06_Detail_Report_ToPDF", Sci.Production.Class.PDFFileNameExtension.PDF);
            excel.ActiveWorkbook.SaveAs(strFileName);
            excel.Quit();

            if (ConvertToPDF.ExcelToPDF(strFileName, strPDFFileName))
            {
                ProcessStartInfo startInfo = new ProcessStartInfo(strPDFFileName);
                Process.Start(startInfo);
            }
            #endregion
            Marshal.ReleaseComObject(worksheet);
            Marshal.ReleaseComObject(excel);
        }
    }
}
