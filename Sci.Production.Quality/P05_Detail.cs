using Ict.Win;
using System.Data;
using Sci.Data;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Ict;
using Sci.Production.PublicPrg;
using System.Diagnostics;

namespace Sci.Production.Quality
{
    public partial class P05_Detail : Win.Subs.Input4
    {
        private string loginID = Env.User.UserID;
        private string aa = Env.User.Keyword;
        private DataRow maindr;
        private string PoID;
        private string ID;
        private DataTable dtOven;
        private bool newOven = false;
        private bool isModify = false;  // 註記[Test Date][Article][Inspector][Remark]是否修改
        private bool isSee = false;
        bool canEdit = true;

        public P05_Detail(bool canedit, string id, string keyvalue2, string keyvalue3, DataRow mainDr, string Poid)
            : base(canedit, id, keyvalue2, keyvalue3)
        {
            this.InitializeComponent();
            this.maindr = mainDr;
            this.PoID = Poid.Trim();
            this.ID = id.Trim();
            this.canEdit = canedit;
            this.isSee = true;

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
            var result = DBProxy.Current.Select(null, string.Format("select * from oven_detail WITH (NOLOCK) where id='{0}'", this.ID), out dt);

            if (!result)
            {
                this.ShowErr(result);
            }

            if (this.ID.Equals("0") && dt.Rows.Count > 0)
            {
                result = DBProxy.Current.Execute(null, "delete oven where id = 0;delete oven_Detail where id = 0;");
                if (!result)
                {
                    this.ShowErr(result);
                }
            }

            // CreateNew open EditMode
            if (dt.Rows.Count == 0 && this.canEdit)
            {
                // this.EditMode = true;
                this.OnUIConvertToMaintain();
            }

            this.btnToExcel.Enabled = !this.EditMode;
            this.btnToPDF.Enabled = !this.EditMode;
        }

        protected override void OnEditModeChanged()
        {
            DataTable dt;
            var result = DBProxy.Current.Select(null, string.Format("select * from oven_detail WITH (NOLOCK) where id='{0}'", this.ID), out dt);
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
            this.comboResult.DataSource = new BindingSource(Result_RowSource, null);
            this.comboResult.ValueMember = "Key";
            this.comboResult.DisplayMember = "Value";
            #region 表頭設定

            DualResult dResult;
            string cmd = "select * from oven WITH (NOLOCK) where id=@id";

            List<SqlParameter> sqm = new List<SqlParameter>();
            sqm.Add(new SqlParameter("@id", this.ID));
            if (dResult = DBProxy.Current.Select(null, cmd, sqm, out this.dtOven))
            {
                if (this.dtOven.Rows.Count > 0)
                {
                    this.txtNoofTest.Text = this.dtOven.Rows[0]["testno"].ToString();
                    this.txtSP.Text = this.dtOven.Rows[0]["POID"].ToString();
                    this.dateTestDate.Value = Convert.ToDateTime(this.dtOven.Rows[0]["Inspdate"]);
                    this.txtArticle.Text = this.dtOven.Rows[0]["article"].ToString();
                    this.txtuserInspector.TextBox1Binding = this.dtOven.Rows[0]["inspector"].ToString();
                    this.txtRemark.Text = this.dtOven.Rows[0]["Remark"].ToString();
                    this.numTemperature.Value = MyUtility.Convert.GetInt(this.dtOven.Rows[0]["Temperature"]);
                    this.numTime.Value = MyUtility.Convert.GetInt(this.dtOven.Rows[0]["Time"]);
                    this.comboResult.SelectedValue = this.dtOven.Rows[0]["Result"].ToString();
                    if (this.dtOven.Rows[0]["Status"].ToString() == "New" || this.dtOven.Rows[0]["Status"].ToString() == string.Empty)
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
                    this.comboResult.SelectedValue = string.Empty;
                    this.numTemperature.Value = 0;
                    this.numTime.Value = 0;
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
            DataRow dr1;

            // Gridview新增欄位
            datas.Columns.Add("SCIRefno", typeof(string));
            datas.Columns.Add("Refno", typeof(string));
            datas.Columns.Add("Colorid", typeof(string));
            datas.Columns.Add("SEQ", typeof(string));
            datas.Columns.Add("LastUpdate", typeof(string));
            datas.Columns.Add("Supplier", typeof(string));
            int i = 0;

            // 跑迴圈丟值進去
            foreach (DataRow dr in datas.Rows)
            {
                if (datas.Rows.Count <= 0)
                {
                    return;
                }

                List<SqlParameter> spm = new List<SqlParameter>();
                string cmdd = "select * from PO_Supp_Detail WITH (NOLOCK) where id=@id and seq1=@seq1 and seq2=@seq2 and FabricType='F'";
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
                }
                else
                {
                    dr["SEQ"] = datas.Rows[i]["seq1"].ToString().PadRight(3, ' ') + "-" + datas.Rows[i]["seq2"].ToString().TrimEnd();
                    dr["SCIRefno"] = dtpo.Rows[0]["SCIRefno"].ToString();
                    dr["refno"] = dtpo.Rows[0]["refno"].ToString();
                    dr["Colorid"] = dtpo.Rows[0]["Colorid"].ToString();
                    dr["Supplier"] = dtsupp.Rows[0]["supplier"].ToString();
                    if (MyUtility.Check.Seek(string.Format(@"select * from DBO.View_ShowName where id='{0}'", datas.Rows[0]["EditNAme"].ToString()), out dr1))
                    {
                        dr["LastUpdate"] = dr1["ID"].ToString() + " - " + dr1["Name_Extno"].ToString();
                    }
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
            DataGridViewGeneratorMaskedTextColumnSettings seqMskCell = new DataGridViewGeneratorMaskedTextColumnSettings();
            seqMskCell.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
            DataGridViewGeneratorTextColumnSettings rollCell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings chgCell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings staCell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings resultCell = Prgs.cellResult.GetGridCell();
            DataGridViewGeneratorTextColumnSettings resultChangeCell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings resultStainCell = new DataGridViewGeneratorTextColumnSettings();

            #region groupCell
            groupCell.EditingTextChanged += (s, e) =>
            {
                DataRow dr = this.grid.GetDataRow(e.RowIndex);
                var ctl = (Ict.Win.UI.DataGridViewTextBoxEditingControl)this.grid.EditingControl;
                var groupValue = ctl.EditingControlFormattedValue.ToString();

                int n;
                if (int.TryParse(groupValue, out n))
                {
                    if (groupValue.ToString().Length > 2)
                    {
                        dr["OvenGroup"] = this.SQlText(groupValue, 2);
                    }
                }
                else
                {
                    dr["OvenGroup"] = string.Empty;
                }

                dr.EndEdit();
            };
            groupCell.CellValidating += (s, e) =>
            {
                if (this.EditMode == false)
                {
                    return;
                }

                DataRow dr = this.grid.GetDataRow(e.RowIndex);
                if (e.FormattedValue.ToString().Length != 2)
                {
                    dr["OvenGroup"] = "0" + e.FormattedValue.ToString();
                }

                dr.EndEdit();
            };
            #endregion

            #region seqMskCell
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

                if (e.Button == MouseButtons.Right)
                {
                    DataRow dr = this.grid.GetDataRow(e.RowIndex);
                    string item_cmd = string.Format("select RTRIM(seq1) +'-'+ RTRIM(seq2) AS SEQ,scirefno,refno,colorid from PO_Supp_Detail WITH (NOLOCK) where id='{0}' and FabricType='F'", this.PoID);
                    SelectItem item = new SelectItem(item_cmd, "5,5,15,12", dr["SEQ"].ToString());
                    DialogResult dresult = item.ShowDialog();
                    if (dresult == DialogResult.Cancel)
                    {
                        return;
                    }

                    char splitChar = '-';
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

                if (e.Button == MouseButtons.Right)
                {
                    DataRow dr = this.grid.GetDataRow(e.RowIndex);
                    string item_cmd = string.Format("select RTRIM(seq1) +'-'+ RTRIM(seq2) AS SEQ,scirefno,refno,colorid from PO_Supp_Detail WITH (NOLOCK) where id='{0}' and FabricType='F'", this.PoID);
                    SelectItem item = new SelectItem(item_cmd, "5,5,15,12", dr["SEQ"].ToString());
                    DialogResult dresult = item.ShowDialog();
                    if (dresult == DialogResult.Cancel)
                    {
                        return;
                    }

                    char splitChar = '-';
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
                    dr.EndEdit();
                }
            };
            seqMskCell.CellValidating += (s, e) =>
            {
                DataRow dr = this.grid.GetDataRow(e.RowIndex);
                DataTable dt;
                DataTable dt1;
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
                    dr["SEQ"] = string.Empty;
                    dr["seq1"] = string.Empty;
                    dr["seq2"] = string.Empty;
                    dr["scirefno"] = string.Empty;
                    dr["refno"] = string.Empty;
                    dr["colorid"] = string.Empty;
                    dr.EndEdit();
                    return;
                }

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

                string sql_cmd = string.Format("select seq1,seq2 from PO_Supp_Detail WITH (NOLOCK) where id='{0}' and FabricType='F' and seq1='{1}' and seq2='{2}'", this.PoID, dr["seq1"].ToString().Trim(), dr["seq2"].ToString().Trim());
                DBProxy.Current.Select(null, sql_cmd, out dt1);
                if (dt1.Rows.Count <= 0)
                {
                    dr["SEQ"] = string.Empty;
                    dr["seq1"] = string.Empty;
                    dr["seq2"] = string.Empty;
                    dr["scirefno"] = string.Empty;
                    dr["refno"] = string.Empty;
                    dr["colorid"] = string.Empty;
                    dr.EndEdit();
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("<SEQ#: {0}> doesn't exist in Data!", e.FormattedValue));
                    return;
                }

                dr["SEQ"].ToString().Replace("_", " ");

                DBProxy.Current.Select(
                    null,
                    string.Format("select scirefno,refno,colorid from PO_Supp_Detail WITH (NOLOCK) where id='{0}' and seq1='{1}' and seq2='{2}' and FabricType='F'", this.PoID, dr["seq1"], dr["seq2"]), out dt1);
                dr["scirefno"] = dt1.Rows[0]["scirefno"].ToString();
                dr["refno"] = dt1.Rows[0]["refno"].ToString();
                dr["colorid"] = dt1.Rows[0]["colorid"].ToString();
                dr.EndEdit();

                // SEQ changed 判斷Roll# 是否存在
                if (MyUtility.Check.Empty(dr["Roll"]))
                {
                    return;
                }

                string cmd = "SELECT Roll,Dyelot from FtyInventory WITH (NOLOCK) where poid=@poid and Seq1=@seq1 and Seq2=@seq2 and Roll=@Roll ";
                List<SqlParameter> spam = new List<SqlParameter>();
                spam.Add(new SqlParameter("@poid", this.PoID));
                spam.Add(new SqlParameter("@seq1", dr["seq1"]));
                spam.Add(new SqlParameter("@seq2", dr["seq2"]));
                spam.Add(new SqlParameter("@Roll", dr["roll"]));
                DBProxy.Current.Select(null, cmd, spam, out dt);
                if (dt.Rows.Count <= 0)
                {
                    dr["Roll"] = string.Empty;
                    dr["Dyelot"] = string.Empty;
                    MyUtility.Msg.WarningBox(string.Format("<Roll: {0}> doesn't exist in Data!", e.FormattedValue));
                    dr.EndEdit();
                    return;
                }
            };
            #endregion

            #region rollCell
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

                if (e.Button == MouseButtons.Right)
                {
                    DataRow dr = this.grid.GetDataRow(e.RowIndex);
                    #region 新資料 不判斷SEQ

                    // if (newOven) //
                    // {
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

                    // }
                    // else
                    // {
                    #endregion

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
                    dr["Dyelot"] = item.GetSelecteds()[0]["Dyelot"].ToString().TrimEnd();
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

                if (e.Button == MouseButtons.Right)
                {
                    DataRow dr = this.grid.GetDataRow(e.RowIndex);
                    #region 新資料 不判斷SEQ

                     // if (newOven) //
                    // {
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

                    // }
                    // else
                    // {
                    #endregion

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
                    dr["Dyelot"] = item.GetSelecteds()[0]["Dyelot"].ToString().TrimEnd();
                    dr.EndEdit();
                }
            };
            rollCell.CellValidating += (s, e) =>
            {
                DataTable dt;
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

                if (oldvalue.Equals(newvalue))
                {
                    return;
                }

                if (MyUtility.Check.Empty(e.FormattedValue)) // 沒填入資料,清空dyelot
                {
                    dr["Roll"] = string.Empty;
                    dr["Dyelot"] = string.Empty;
                    dr.EndEdit();
                    return;
                }

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
                    dr.EndEdit();
                }
            };
            #endregion

            #region chgCell
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

                if (e.Button == MouseButtons.Right)
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

                if (e.Button == MouseButtons.Right)
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

            #region staCell
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

                if (e.Button == MouseButtons.Right)
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

                if (e.Button == MouseButtons.Right)
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

            #region result
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

            this.Helper.Controls.Grid.Generator(this.grid)
                .Date("SubmitDate", "Submit Date", width: Widths.AnsiChars(8))
                .Text("OvenGroup", "Group", width: Widths.AnsiChars(5), settings: groupCell)
                .MaskedText("SEQ", "CCC-CC", "SEQ#", width: Widths.AnsiChars(7), settings: seqMskCell)
                .Text("Roll", header: "Roll#", width: Widths.AnsiChars(10), settings: rollCell)
                .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("Refno", header: "Refno", width: Widths.AnsiChars(12), iseditingreadonly: true)
                .Text("SCIRefno", header: "SCI Refno", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("Colorid", header: "Color", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("Result", header: "Result", width: Widths.AnsiChars(8), iseditingreadonly: true) // not yet
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

            return base.OnSaveBefore();
        }

        protected override DualResult OnSave()
        {
            DualResult upResult = new DualResult(true);
            string update_cmd = string.Empty;
            DataTable dt;

            string sqlcmd = string.Format(@"select Max(TestNO) as MaxNO from oven WITH (NOLOCK) where poid='{0}'", this.PoID);
            DBProxy.Current.Select(null, sqlcmd, out dt);
            int testno = MyUtility.Convert.GetInt(dt.Rows[0]["MaxNO"]);

            DataRow dr = ((DataTable)this.gridbs.DataSource).NewRow();
            for (int i = ((DataTable)this.gridbs.DataSource).Rows.Count; i > 0; i--)
            {
                dr = ((DataTable)this.gridbs.DataSource).Rows[i - 1];

                // 刪除
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

                // ii.刪除空白SEQ 的Oven_Detail資料
                if (MyUtility.Check.Empty(dr["SEQ"]))
                {
                    dr.Delete();
                    continue;
                }

                string Today = DateTime.Now.ToShortDateString();

                // 新增
                if (dr.RowState == DataRowState.Added)
                {
                    if (this.newOven) // insert 新資料進oven
                    {
                        DataTable dtID;
                        DBProxy.Current.Select(null, "select Max(id) as id from Oven WITH (NOLOCK) ", out dtID);

                        int newID = MyUtility.Convert.GetInt(dtID.Rows[0]["id"]);
                        this.ID = (newID + 1).ToString();
                        this.KeyValue1 = this.ID;

                        string insCmd = @"
SET IDENTITY_INSERT oven ON
insert into Oven(ID,POID,TestNo,InspDate,Article,Result,Status,Inspector,Remark,addName,addDate,Temperature,Time)
values(@id ,@poid,@testNO,GETDATE(),@Article,'','New',@logid,@remark,@logid,GETDATE(),@Temperature,@Time)
SET IDENTITY_INSERT oven off";
                        List<SqlParameter> spamAddNew = new List<SqlParameter>();
                        spamAddNew.Add(new SqlParameter("@id", this.ID)); // New ID
                        spamAddNew.Add(new SqlParameter("@poid", this.PoID));
                        spamAddNew.Add(new SqlParameter("@article", this.txtArticle.Text));
                        spamAddNew.Add(new SqlParameter("@logid", this.loginID));
                        spamAddNew.Add(new SqlParameter("@remark", this.txtRemark.Text));
                        spamAddNew.Add(new SqlParameter("@testNO", testno + 1));
                        spamAddNew.Add(new SqlParameter("@Temperature", this.numTemperature.Value));
                        spamAddNew.Add(new SqlParameter("@Time", this.numTime.Value));
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

                if (dr.RowState == DataRowState.Modified || this.isModify)
                {
                    string editCmd = @"update oven set inspdate=@insDate,Article=@Article,Inspector=@insor,remark=@remark , EditName=@EditName,EditDate=@EditDate,Temperature=@Temperature,
                      Time=@Time
                                       where id=@id";
                    List<SqlParameter> spamEdit = new List<SqlParameter>();
                    spamEdit.Add(new SqlParameter("@id", this.ID)); // New ID
                    spamEdit.Add(new SqlParameter("@insDate", this.dateTestDate.Value));
                    spamEdit.Add(new SqlParameter("@article", this.txtArticle.Text));
                    spamEdit.Add(new SqlParameter("@insor", this.loginID));
                    spamEdit.Add(new SqlParameter("@remark", this.txtRemark.Text));
                    spamEdit.Add(new SqlParameter("@EditName", this.loginID));
                    spamEdit.Add(new SqlParameter("@EditDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                    spamEdit.Add(new SqlParameter("@Temperature", this.numTemperature.Value));
                    spamEdit.Add(new SqlParameter("@Time", this.numTime.Value));
                    upResult = DBProxy.Current.Execute(null, editCmd, spamEdit);
                }
            }

            // 更新PO.LabOvenPercent
            DualResult result = this.UpdateInspPercent();
            if (!result)
            {
                return Ict.Result.F(result.ToString());
            }

            return base.OnSave();
        }

        protected override void OnInsert()
        {
            DataTable dt;

            DataTable dtGrid = (DataTable)this.gridbs.DataSource;
            DBProxy.Current.Select(null, string.Format("select * from oven_detail WITH (NOLOCK) where id='{0}'", this.ID), out dt);

            DataTable dtGrid_Max = (DataTable)this.gridbs.DataSource;
            DBProxy.Current.Select(null, string.Format("select  max(OvenGroup) as max from oven_detail WITH (NOLOCK) where id='{0}'", this.ID), out dtGrid_Max);
            int Max = MyUtility.Convert.GetInt(dtGrid_Max.Rows[0]["max"]);
            int Data_rows = dt.Rows.Count;
            int Grid_rows = dtGrid.Rows.Count;
            int intTen; // 判斷十位數
            base.OnInsert();

            // 新增detail
            // if (Data_rows <= 0)
            // {
            if (Grid_rows == 0) // 第一筆0
                {
                    dtGrid.Rows[Grid_rows]["OvenGroup"] = "01";
                    intTen = 0;
                    return;
                }

            int group = MyUtility.Convert.GetInt(dtGrid.Rows[Grid_rows - 1]["ovengroup"].ToString().Substring(1, 1));
            int groupAll = MyUtility.Convert.GetInt(dtGrid.Rows[Grid_rows - 1]["ovengroup"]);

            if (group == 9 && groupAll < 10)
                {
                    intTen = 1;
                    dtGrid.Rows[Grid_rows]["ovengroup"] = intTen.ToString() + "0";
                    return;
                }
                else if (group == 9 && groupAll > 10)
                {
                    if (groupAll == 99)
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
                    dtGrid.Rows[Grid_rows]["ovengroup"] = intTen.ToString() + group.ToString();
                }
                else
                {
                    intTen = MyUtility.Convert.GetInt(groupAll.ToString().Substring(0, 1));
                    group++;
                    dtGrid.Rows[Grid_rows]["ovengroup"] = intTen.ToString() + group.ToString();
                }
        }

        #region 表頭Article 右鍵事件: 1.右鍵selectItem 2.判斷validated
        private void txtArticle_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.EditMode == false)
            {
                return;
            }

            if (e.Button == MouseButtons.Right)
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

        private void txtArticle_Validating(object sender, CancelEventArgs e)
        {
            if (!this.EditMode || this.txtArticle.Text.Empty())
            {
                return;
            }

            DualResult dresult;
            DataTable dt;
            string cmd = @"select distinct oq.article 
                    from orders o, order_qty oq 
                    where o.id = oq.id and oq.qty > 0 
                    and o.poid =@poid
                    and oq.article=@art";
            List<SqlParameter> spm = new List<SqlParameter>();
            spm.Add(new SqlParameter("@poid", this.PoID));
            spm.Add(new SqlParameter("@art", this.txtArticle.Text));
            if (dresult = DBProxy.Current.Select(null, cmd, spm, out dt))
            {
                if (dt.Rows.Count <= 0)
                {
                    this.txtArticle.Text = string.Empty;
                    this.txtArticle.Select();
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("<Article: {0}> doesn't exist in orders", this.txtArticle.Text));
                    return;
                }
            }
            else
            {
                return;
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
                        if (MyUtility.Check.Empty(dr["ovenGroup"]))
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
                            DBProxy.Current.Execute(null, string.Format("update oven set result='Fail',status='Confirmed',editname='{0}',editdate='{1}' where id='{2}'", this.loginID, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), dr["id"]));
                        }

                        if (dr["Result"].ToString().Trim().ToUpper() == "PASS" && result)
                        {
                            DBProxy.Current.Execute(null, string.Format("update oven set result='Pass',status='Confirmed',editname='{0}',editdate='{1}' where id='{2}'", this.loginID, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), dr["id"]));
                        }
                    }
                }
            }

            // Amend
            else
            {
                DBProxy.Current.Execute(null, string.Format("update oven set result='',status='New',editname='{0}',editdate='{1}' where id='{2}'", this.loginID, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), dt.Rows[0]["id"]));
            }

            // 更新PO.LabOvenPercent
            DualResult res = this.UpdateInspPercent();
            if (!res)
            {
                this.ShowErr(res);
                return;
            }

            this.OnRequery();
        }

        private void btnToExcel_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)this.gridbs.DataSource;
            string[] columnNames = new string[] { "OvenGroup", "SEQ", "Roll", "Dyelot", "SCIRefno", "Colorid", "Supplier", "Changescale", "StainingScale", "Result", "Remark" };
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

            DataTable dtPo;
            string StyleID;
            string SeasonID;
            string status;
            string BrandID;
            DBProxy.Current.Select(null, string.Format("select * from PO WITH (NOLOCK) where id='{0}'", this.PoID), out dtPo);
            if (dtPo.Rows.Count == 0)
            {
                 StyleID = string.Empty;
                 SeasonID = string.Empty;
                 status = string.Empty;
                 BrandID = string.Empty;
            }
            else
            {
                StyleID = dtPo.Rows[0]["StyleID"].ToString();
                SeasonID = dtPo.Rows[0]["SeasonID"].ToString();
                status = this.dtOven.Rows[0]["status"].ToString();
                BrandID = dtPo.Rows[0]["BrandID"].ToString();
            }

            string strXltName = Env.Cfg.XltPathDir + "\\Quality_P05_Detail_Report.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return;
            }

            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            worksheet.Cells[1, 2] = this.txtSP.Text.ToString();
            worksheet.Cells[1, 4] = StyleID;
            worksheet.Cells[1, 6] = dtPo.Rows[0]["SeasonID"].ToString();
            worksheet.Cells[1, 8] = SeasonID;
            worksheet.Cells[1, 10] = this.txtNoofTest.Text.ToString();
            worksheet.Cells[2, 2] = status;
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
            string strExcelName = Class.MicrosoftFile.GetName("Quality_P05_Detail_Report");
            excel.ActiveWorkbook.SaveAs(strExcelName);
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(worksheet);

            strExcelName.OpenFile();
            #endregion
        }

        private new void TextChanged(object sender, EventArgs e)
        {
            this.isModify = true;
        }

        private void btnToPDF_Click(object sender, EventArgs e)
        {
            DualResult result;
            DataTable dt = (DataTable)this.gridbs.DataSource;
            DataTable dtdist;
            if (!(result = DBProxy.Current.Select(null, $@"select distinct convert(varchar(100),submitDate,111) as submitDate  from Oven_Detail  where id={this.ID}", out dtdist)))
            {
                this.ShowErr(result);
                return;
            }

            if (dt.Rows.Count < 1)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return;
            }

            DataTable dtOrders;
            string StyleUkey;
            string StyleID;
            string SeasonID;
            string CustPONo;
            string BrandID;
            if (!(result = DBProxy.Current.Select(null, string.Format("select * from Orders WITH (NOLOCK) where poid='{0}'", this.PoID), out dtOrders)))
            {
                this.ShowErr(result);
                return;
            }

            if (dtOrders.Rows.Count == 0)
            {
                StyleUkey = string.Empty;
                SeasonID = string.Empty;
                CustPONo = string.Empty;
                BrandID = string.Empty;
                StyleID = string.Empty;
            }
            else
            {
                StyleUkey = dtOrders.Rows[0]["StyleUkey"].ToString();
                StyleID = dtOrders.Rows[0]["StyleID"].ToString();
                SeasonID = dtOrders.Rows[0]["SeasonID"].ToString();
                CustPONo = dtOrders.Rows[0]["CustPONo"].ToString();
                BrandID = dtOrders.Rows[0]["BrandID"].ToString();
            }

            string strXltName = Env.Cfg.XltPathDir + "\\Quality_P05_Detail_Report_ToPDF.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return;
            }

            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            for (int c = 1; c < dtdist.Rows.Count; c++)
            {
                Microsoft.Office.Interop.Excel.Worksheet worksheetFirst = excel.ActiveWorkbook.Worksheets[1];
                Microsoft.Office.Interop.Excel.Worksheet worksheetn = excel.ActiveWorkbook.Worksheets[1 + c];
                worksheetFirst.Copy(worksheetn);
            }

            int nSheet = 1;
            for (int i = 0; i < dtdist.Rows.Count; i++)
            {
                worksheet = excel.ActiveWorkbook.Worksheets[nSheet];
                worksheet.Cells[4, 3] = dtdist.Rows[i]["submitDate"].ToString();
                worksheet.Cells[4, 5] = this.dateTestDate.Text;
                worksheet.Cells[4, 7] = this.txtSP.Text;
                worksheet.Cells[4, 10] = BrandID;
                worksheet.Cells[6, 3] = StyleID;
                worksheet.Cells[6, 6] = CustPONo;
                worksheet.Cells[6, 9] = this.txtArticle.Text;
                worksheet.Cells[7, 3] = Convert.ToString(MyUtility.GetValue.Lookup($@"select StyleName from Style WITH (NOLOCK) where Ukey ='{StyleUkey}'", null));
                worksheet.Cells[7, 6] = SeasonID;
                worksheet.Cells[10, 3] = this.numTemperature.Value + "˚C";
                worksheet.Cells[10, 7] = this.numTime.Value + "hrs";
                DataRow[] dr = dt.Select(MyUtility.Check.Empty(dtdist.Rows[i]["submitDate"]) ? $@"submitDate is null" : $"submitDate = '{dtdist.Rows[i]["submitDate"].ToString()}'");

                for (int ii = 0; ii < dr.Length; ii++)
                {
                    worksheet.Cells[14 + ii, 2] = dr[ii]["Refno"];
                    worksheet.Cells[14 + ii, 3] = dr[ii]["Colorid"];
                    worksheet.Cells[14 + ii, 4] = dr[ii]["Dyelot"];
                    worksheet.Cells[14 + ii, 5] = dr[ii]["Roll"];
                    worksheet.Cells[14 + ii, 6] = dr[ii]["Changescale"];
                    worksheet.Cells[14 + ii, 7] = dr[ii]["ResultChange"];
                    worksheet.Cells[14 + ii, 8] = dr[ii]["StainingScale"];
                    worksheet.Cells[14 + ii, 9] = dr[ii]["ResultStain"];
                    worksheet.Cells[14 + ii, 10] = dr[ii]["Remark"];

                    Microsoft.Office.Interop.Excel.Range rg1 = worksheet.Range[worksheet.Cells[2][14 + ii], worksheet.Cells[10][14 + ii]];

                    // 加框線
                    rg1.Borders.LineStyle = 1;
                    rg1.Borders.Weight = 3;
                    rg1.WrapText = true; // 自動換列

                    // 水平,垂直置中
                    rg1.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                    rg1.VerticalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                }

                #region singnature
                for (int m = 0; m < 3; m++)
                {
                    // 設定range 變數
                    Microsoft.Office.Interop.Excel.Range rgSign = worksheet.Range[worksheet.Cells[8][12 + dr.Length + 3 + m], worksheet.Cells[10][12 + dr.Length + 3 + m]]; // 14

                    // 設定邊框
                    rgSign.Borders.LineStyle = 1;
                    rgSign.Borders.Weight = 3;

                    // 置中
                    rgSign.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                    rgSign.VerticalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                    switch (m)
                    {
                        case 0:
                            rgSign.Font.Bold = true;
                            rgSign.Cells[1, 1] = "Signature";
                            break;
                        case 1:
                            rgSign.Cells[1, 1] = MyUtility.GetValue.Lookup("Name", this.txtuserInspector.TextBox1Binding, "Pass1", "ID");
                            break;
                        case 2:
                            rgSign.Font.Bold = true;
                            rgSign.Cells[1, 1] = "Checked by:";
                            break;
                    }

                    rgSign.Merge();
                }
                #endregion

                #region 畫Original After Test 固定框線格式

                #region 判斷超過一頁則減少畫框數量
                int cntFrame = 5;
                if (dr.Length > 18 && dr.Length < 55)
                {
                    // 將會增長的dataRow -18 基礎數, 再除上固定畫框的7格 +1
                    cntFrame = 5 - (((dr.Length - 18) / 7) + 1);
                }
                #endregion

                int rowcnt = 19 + dr.Length;
                #region 畫Original After Test
                if (cntFrame > 0)
                {
                    for (int t = 0; t < 2; t++)
                    {
                        Microsoft.Office.Interop.Excel.Range rgText = worksheet.Range[worksheet.Cells[2 + (t * 5)][rowcnt], worksheet.Cells[5 + (t * 5)][rowcnt]];

                        // 置中
                        rgText.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                        rgText.VerticalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                        rgText.Font.Bold = true;

                        switch (t)
                        {
                            case 0:
                                rgText.Cells[1, 1] = "Original";
                                break;
                            case 1:
                                rgText.Cells[1, 1] = "After Test";
                                break;
                        }

                        rgText.Merge();
                    }

                    rowcnt++;
                }
                #endregion

                #region 畫固定外框
                for (int o = 0; o < cntFrame; o++)
                {
                    // 畫一對框線
                    for (int oo = 0; oo < 2; oo++)
                    {
                        // 設定range 變數
                        Microsoft.Office.Interop.Excel.Range rf = worksheet.Range[worksheet.Cells[2 + (oo * 5)][rowcnt], worksheet.Cells[5 + (oo * 5)][rowcnt + 5]];

                        // 設定邊框
                        rf.BorderAround2(LineStyle: 1);

                        // 畫固定格式小框
                        if (oo == 0)
                        {
                            // 設定range 變數
                            Microsoft.Office.Interop.Excel.Range rff = worksheet.Range[worksheet.Cells[2][rowcnt + 4], worksheet.Cells[2][rowcnt + 5]];

                            // 設定邊框
                            rff.BorderAround2(LineStyle: 1);
                            rff.Cells[1, 1] = "Article#";
                        }
                    }

                    rowcnt += 7;
                }
                #endregion

                #endregion
                nSheet++;
            }

            #region Save & Show Excel
            string strFileName = Class.MicrosoftFile.GetName("Quality_P05_Detail_Report_ToPDF");
            string strPDFFileName = Class.MicrosoftFile.GetName("Quality_P05_Detail_Report_ToPDF", Class.PDFFileNameExtension.PDF);
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

        private void txtuserInspector_Validating(object sender, CancelEventArgs e)
        {
            this.isModify = true;
        }

        private DualResult UpdateInspPercent()
        {
            // 更新PO.LabOvenPercent
            DualResult upResult;
            if (!(upResult = DBProxy.Current.Execute(null, $"exec UpdateInspPercent 'LabOven','{this.txtSP.Text}'")))
            {
                return upResult;
            }

            return upResult;
        }
    }
}
