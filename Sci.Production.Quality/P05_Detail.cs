using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.PublicPrg;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Quality
{
    /// <inheritdoc/>
    public partial class P05_Detail : Win.Subs.Input4
    {
        private readonly string loginID = Env.User.UserID;
        private readonly string aa = Env.User.Keyword;
        private readonly DataRow maindr;
        private readonly string PoID;
        private string ID;
        private DataTable dtOven;
        private bool newOven = false;
        private bool isModify = false;  // 註記[Test Date][Article][Inspector][Remark]是否修改
        private bool isSee = false;
        private bool canEdit = true;

        /// <inheritdoc/>
        public P05_Detail(bool canedit, string id, string keyvalue2, string keyvalue3, DataRow mainDr, string poid)
            : base(canedit, id, keyvalue2, keyvalue3)
        {
            this.InitializeComponent();
            this.maindr = mainDr;
            this.PoID = poid.Trim();
            this.ID = id.Trim();
            this.canEdit = canedit;
            this.isSee = true;

            // 判斷是否為新資料
            if (MyUtility.Check.Empty(this.maindr))
            {
                this.newOven = true;
            }
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            var result = DBProxy.Current.Select(null, string.Format("select * from oven_detail WITH (NOLOCK) where id='{0}'", this.ID), out DataTable dt);

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

        /// <inheritdoc/>
        protected override void OnEditModeChanged()
        {
            var result = DBProxy.Current.Select(null, string.Format("select * from oven_detail WITH (NOLOCK) where id='{0}'", this.ID), out DataTable dt);
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

        /// <inheritdoc/>
        protected override DualResult OnRequery(out DataTable datas)
        {
            Dictionary<string, string> result_RowSource = new Dictionary<string, string>
            {
                { "Pass", "Pass" },
                { "Fail", "Fail" },
            };
            this.comboResult.DataSource = new BindingSource(result_RowSource, null);
            this.comboResult.ValueMember = "Key";
            this.comboResult.DisplayMember = "Value";
            #region 表頭設定

            DualResult dResult;
            string cmd = "select * from oven WITH (NOLOCK) where id=@id";

            List<SqlParameter> sqm = new List<SqlParameter> { new SqlParameter("@id", this.ID) };
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
                }
            }

            #endregion

            return base.OnRequery(out datas);
        }

        // 重組grid view

        /// <inheritdoc/>
        protected override void OnRequeryPost(DataTable datas)
        {
            base.OnRequeryPost(datas);

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
                DBProxy.Current.Select(null, cmdd, spm, out DataTable dtpo);

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
                DBProxy.Current.Select(null, cmddSupp, spmSupp, out DataTable dtsupp);
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
                    if (MyUtility.Check.Seek(string.Format(@"select * from DBO.View_ShowName where id='{0}'", datas.Rows[0]["EditNAme"].ToString()), out DataRow dr1))
                    {
                        dr["LastUpdate"] = dr1["ID"].ToString() + " - " + dr1["Name_Extno"].ToString();
                    }
                }

                i++;
            }
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        protected override bool OnGridSetup()
        {
            DataGridViewGeneratorTextColumnSettings groupCell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorMaskedTextColumnSettings seqMskCell = new DataGridViewGeneratorMaskedTextColumnSettings
            {
                TextMaskFormat = MaskFormat.ExcludePromptAndLiterals,
            };
            DataGridViewGeneratorTextColumnSettings rollCell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings chgCell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings staCell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings resultCell = Prgs.CellResult.GetGridCell();
            DataGridViewGeneratorTextColumnSettings resultChangeCell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings resultStainCell = new DataGridViewGeneratorTextColumnSettings();

            #region groupCell
            groupCell.EditingTextChanged += (s, e) =>
            {
                DataRow dr = this.grid.GetDataRow(e.RowIndex);
                var ctl = (Ict.Win.UI.DataGridViewTextBoxEditingControl)this.grid.EditingControl;
                var groupValue = ctl.EditingControlFormattedValue.ToString();

                if (int.TryParse(groupValue, out int n))
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
                    // 沒填入資料,清空
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
                DBProxy.Current.Select(null, sql_cmd, out DataTable dt1);
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
                List<SqlParameter> spam = new List<SqlParameter>
                {
                    new SqlParameter("@poid", this.PoID),
                    new SqlParameter("@seq1", dr["seq1"]),
                    new SqlParameter("@seq2", dr["seq2"]),
                    new SqlParameter("@Roll", dr["roll"]),
                };
                DBProxy.Current.Select(null, cmd, spam, out DataTable dt);
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
                    List<SqlParameter> spam = new List<SqlParameter>
                    {
                        new SqlParameter("@poid", this.PoID),
                        new SqlParameter("@seq1", dr["seq1"]),
                        new SqlParameter("@seq2", dr["seq2"]),
                    };
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
                    List<SqlParameter> spam = new List<SqlParameter>
                    {
                        new SqlParameter("@poid", this.PoID),
                        new SqlParameter("@seq1", dr["seq1"]),
                        new SqlParameter("@seq2", dr["seq2"]),
                    };
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

                if (MyUtility.Check.Empty(e.FormattedValue))
                {
                    // 沒填入資料,清空dyelot
                    dr["Roll"] = string.Empty;
                    dr["Dyelot"] = string.Empty;
                    dr.EndEdit();
                    return;
                }

                string cmd = "SELECT Roll,Dyelot from FtyInventory WITH (NOLOCK) where poid=@poid and Seq1=@seq1 and Seq2=@seq2 and Roll=@Roll ";
                List<SqlParameter> spam = new List<SqlParameter>
                {
                    new SqlParameter("@poid", this.PoID),
                    new SqlParameter("@seq1", dr["seq1"]),
                    new SqlParameter("@seq2", dr["seq2"]),
                    new SqlParameter("@Roll", e.FormattedValue),
                };
                DBProxy.Current.Select(null, cmd, spam, out DataTable dt);
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

                DataRow dr = this.grid.GetDataRow(e.RowIndex);
                string cmd = "select id from Scale WITH (NOLOCK) where Junk=0  and id=@ChangeScale";
                List<SqlParameter> spam = new List<SqlParameter> { new SqlParameter("@ChangeScale", e.FormattedValue) };

                DBProxy.Current.Select(null, cmd, spam, out DataTable dt);
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

                DataRow dr = this.grid.GetDataRow(e.RowIndex);
                string cmd = "select id from Scale WITH (NOLOCK) where Junk=0  and id=@StainingScale";
                List<SqlParameter> spam = new List<SqlParameter> { new SqlParameter("@StainingScale", e.FormattedValue) };

                DBProxy.Current.Select(null, cmd, spam, out DataTable dt);
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

            DataGridViewGeneratorComboBoxColumnSettings temperature = new DataGridViewGeneratorComboBoxColumnSettings();
            DataTable temperatureDT = new DataTable();
            temperatureDT.Columns.Add("Temperature", typeof(int));
            temperatureDT.Columns.Add("TemperatureDisplay", typeof(string));
            DataRow newRow = temperatureDT.NewRow();
            newRow["Temperature"] = 0;
            newRow["TemperatureDisplay"] = string.Empty;
            temperatureDT.Rows.Add(newRow);
            newRow = temperatureDT.NewRow();
            newRow["Temperature"] = 70;
            newRow["TemperatureDisplay"] = "70";
            temperatureDT.Rows.Add(newRow);
            newRow = temperatureDT.NewRow();
            newRow["Temperature"] = 90;
            newRow["TemperatureDisplay"] = "90";
            temperatureDT.Rows.Add(newRow);
            temperature.DataSource = temperatureDT; // new BindingSource(temperatureItem, null);
            temperature.ValueMember = "Temperature";
            temperature.DisplayMember = "TemperatureDisplay";

            DataGridViewGeneratorComboBoxColumnSettings time = new DataGridViewGeneratorComboBoxColumnSettings();
            DataTable timeDT = new DataTable();
            timeDT.Columns.Add("Time", typeof(int));
            timeDT.Columns.Add("TimeDisplay", typeof(string));
            newRow = timeDT.NewRow();
            newRow["Time"] = 0;
            newRow["TimeDisplay"] = string.Empty;
            timeDT.Rows.Add(newRow);
            newRow = timeDT.NewRow();
            newRow["Time"] = 24;
            newRow["TimeDisplay"] = "24";
            timeDT.Rows.Add(newRow);
            newRow = timeDT.NewRow();
            newRow["Time"] = 48;
            newRow["TimeDisplay"] = "48";
            timeDT.Rows.Add(newRow);
            time.DataSource = timeDT;
            time.ValueMember = "Time";
            time.DisplayMember = "TimeDisplay";

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
                .Text("LastUpdate", header: "LastUpdate", width: Widths.AnsiChars(30), iseditingreadonly: true)
                .ComboBox("Temperature", "Temperature(˚C)", width: Widths.AnsiChars(10), settings: temperature)
                .ComboBox("Time", "Time(hrs)", width: Widths.AnsiChars(10), settings: time)
                ;
            return true;
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        protected override DualResult OnSave()
        {
            DualResult upResult = new DualResult(true);
            string update_cmd = string.Empty;

            string sqlcmd = string.Format(@"select Max(TestNO) as MaxNO from oven WITH (NOLOCK) where poid='{0}'", this.PoID);
            DBProxy.Current.Select(null, sqlcmd, out DataTable dt);
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

                string today = DateTime.Now.ToShortDateString();

                // 新增
                if (dr.RowState == DataRowState.Added)
                {
                    if (this.newOven)
                    {
                        // insert 新資料進oven
                        DBProxy.Current.Select(null, "select Max(id) as id from Oven WITH (NOLOCK) ", out DataTable dtID);

                        int newID = MyUtility.Convert.GetInt(dtID.Rows[0]["id"]);
                        this.ID = (newID + 1).ToString();
                        this.KeyValue1 = this.ID;

                        string insCmd = @"
SET IDENTITY_INSERT oven ON
insert into Oven(ID,POID,TestNo,InspDate,Article,Result,Status,Inspector,Remark,addName,addDate,Temperature,Time)
values(@id ,@poid,@testNO,GETDATE(),@Article,'','New',@logid,@remark,@logid,GETDATE(),@Temperature,@Time)
SET IDENTITY_INSERT oven off";
                        List<SqlParameter> spamAddNew = new List<SqlParameter>
                        {
                            new SqlParameter("@id", this.ID), // New ID
                            new SqlParameter("@poid", this.PoID),
                            new SqlParameter("@article", this.txtArticle.Text),
                            new SqlParameter("@logid", this.loginID),
                            new SqlParameter("@remark", this.txtRemark.Text),
                            new SqlParameter("@testNO", testno + 1),
                        };
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
                    List<SqlParameter> spamEdit = new List<SqlParameter>
                    {
                        new SqlParameter("@id", this.ID), // New ID
                        new SqlParameter("@insDate", this.dateTestDate.Value),
                        new SqlParameter("@article", this.txtArticle.Text),
                        new SqlParameter("@insor", this.loginID),
                        new SqlParameter("@remark", this.txtRemark.Text),
                        new SqlParameter("@EditName", this.loginID),
                        new SqlParameter("@EditDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                    };
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

        /// <inheritdoc/>
        protected override void OnInsert()
        {
            DataTable dtGrid = (DataTable)this.gridbs.DataSource;
            DBProxy.Current.Select(null, string.Format("select * from oven_detail WITH (NOLOCK) where id='{0}'", this.ID), out DataTable dt);

            DataTable dtGrid_Max = (DataTable)this.gridbs.DataSource;
            DBProxy.Current.Select(null, string.Format("select  max(OvenGroup) as max from oven_detail WITH (NOLOCK) where id='{0}'", this.ID), out dtGrid_Max);
            int max = MyUtility.Convert.GetInt(dtGrid_Max.Rows[0]["max"]);
            int data_rows = dt.Rows.Count;
            int grid_rows = dtGrid.Rows.Count;
            int intTen; // 判斷十位數
            base.OnInsert();

            // 新增detail
            // if (Data_rows <= 0)
            // {
            // 第一筆0
            if (grid_rows == 0)
            {
                dtGrid.Rows[grid_rows]["OvenGroup"] = "01";
                intTen = 0;
                return;
            }

            int group = MyUtility.Convert.GetInt(dtGrid.Rows[grid_rows - 1]["ovengroup"].ToString().Substring(1, 1));
            int groupAll = MyUtility.Convert.GetInt(dtGrid.Rows[grid_rows - 1]["ovengroup"]);

            if (group == 9 && groupAll < 10)
            {
                intTen = 1;
                dtGrid.Rows[grid_rows]["ovengroup"] = intTen.ToString() + "0";
                return;
            }
            else if (group == 9 && groupAll > 10)
            {
                if (groupAll == 99)
                {
                    intTen = 0;
                    dtGrid.Rows[grid_rows]["ovengroup"] = intTen.ToString() + "1";
                    return;
                }

                intTen = MyUtility.Convert.GetInt(groupAll.ToString().Substring(0, 1)) + 1;
                dtGrid.Rows[grid_rows]["ovengroup"] = intTen.ToString() + "0";
                return;
            }

            if (groupAll < 9)
            {
                intTen = 0;
                group++;
                dtGrid.Rows[grid_rows]["ovengroup"] = intTen.ToString() + group.ToString();
            }
            else
            {
                intTen = MyUtility.Convert.GetInt(groupAll.ToString().Substring(0, 1));
                group++;
                dtGrid.Rows[grid_rows]["ovengroup"] = intTen.ToString() + group.ToString();
            }
        }

        #region 表頭Article 右鍵事件: 1.右鍵selectItem 2.判斷validated
        private void TxtArticle_MouseDown(object sender, MouseEventArgs e)
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
                List<SqlParameter> spm = new List<SqlParameter> { new SqlParameter("@poid", this.PoID) };
                SelectItem item = new SelectItem(cmd, spm, "12", null, null);
                DialogResult result = item.ShowDialog();
                if (result == DialogResult.Cancel)
                {
                    return;
                }

                this.txtArticle.Text = item.GetSelectedString();
            }
        }

        private void TxtArticle_Validating(object sender, CancelEventArgs e)
        {
            if (!this.EditMode || this.txtArticle.Text.Empty())
            {
                return;
            }

            DualResult dresult;
            string cmd = @"select distinct oq.article 
                    from orders o, order_qty oq 
                    where o.id = oq.id and oq.qty > 0 
                    and o.poid =@poid
                    and oq.article=@art";
            List<SqlParameter> spm = new List<SqlParameter>
            {
                new SqlParameter("@poid", this.PoID),
                new SqlParameter("@art", this.txtArticle.Text),
            };
            if (dresult = DBProxy.Current.Select(null, cmd, spm, out DataTable dt))
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

        private void BtnEncode_Click(object sender, EventArgs e)
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

        private void BtnToExcel_Click(object sender, EventArgs e)
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

            string styleID;
            string seasonID;
            string status;
            string brandID;
            DBProxy.Current.Select(null, string.Format("select * from PO WITH (NOLOCK) where id='{0}'", this.PoID), out DataTable dtPo);
            if (dtPo.Rows.Count == 0)
            {
                styleID = string.Empty;
                seasonID = string.Empty;
                status = string.Empty;
                brandID = string.Empty;
            }
            else
            {
                styleID = dtPo.Rows[0]["StyleID"].ToString();
                seasonID = dtPo.Rows[0]["SeasonID"].ToString();
                status = this.dtOven.Rows[0]["status"].ToString();
                brandID = dtPo.Rows[0]["BrandID"].ToString();
            }

            string strXltName = Env.Cfg.XltPathDir + "\\Quality_P05_Detail_Report.xltx";
            Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return;
            }

            Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            worksheet.Cells[1, 2] = this.txtSP.Text.ToString();
            worksheet.Cells[1, 4] = styleID;
            worksheet.Cells[1, 6] = dtPo.Rows[0]["SeasonID"].ToString();
            worksheet.Cells[1, 8] = seasonID;
            worksheet.Cells[1, 10] = this.txtNoofTest.Text.ToString();
            worksheet.Cells[2, 2] = status;
            worksheet.Cells[2, 4] = this.comboResult.Text;
            worksheet.Cells[2, 6] = this.dateTestDate.Text;
            worksheet.Cells[2, 8] = this.txtuserInspector.TextBox1.Text.ToString();
            worksheet.Cells[2, 10] = brandID;

            int startRow = 4;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                worksheet.Cells[startRow + i, 1] = ret[i, 0];
                worksheet.Cells[startRow + i, 2] = ret[i, 1];
                worksheet.Cells[startRow + i, 3] = ret[i, 2];
                worksheet.Cells[startRow + i, 4] = ret[i, 3];
                worksheet.Cells[startRow + i, 5] = ret[i, 4];
                worksheet.Cells[startRow + i, 6] = ret[i, 5];
                worksheet.Cells[startRow + i, 7] = ret[i, 6];
                worksheet.Cells[startRow + i, 8] = ret[i, 7];
                worksheet.Cells[startRow + i, 9] = ret[i, 8];
                worksheet.Cells[startRow + i, 10] = ret[i, 9];
                worksheet.Cells[startRow + i, 11] = ret[i, 10];
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

        private void BtnToPDF_Click(object sender, EventArgs e)
        {
            DualResult result;
            DataTable dt = (DataTable)this.gridbs.DataSource;
            if (!(result = DBProxy.Current.Select(null, $@"select distinct convert(varchar(100),submitDate,111) as submitDate  from Oven_Detail  where id={this.ID}", out DataTable dtdist)))
            {
                this.ShowErr(result);
                return;
            }

            if (dt.Rows.Count < 1)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return;
            }

            if (!(result = DBProxy.Current.Select(null, string.Format("select * from Orders WITH (NOLOCK) where poid='{0}'", this.PoID), out DataTable dtOrders)))
            {
                this.ShowErr(result);
                return;
            }

            string styleUkey = string.Empty;
            string styleID = string.Empty;
            string seasonID = string.Empty;
            string custPONo = string.Empty;
            string brandID = string.Empty;
            if (dtOrders.Rows.Count > 0)
            {
                styleUkey = dtOrders.Rows[0]["StyleUkey"].ToString();
                styleID = dtOrders.Rows[0]["StyleID"].ToString();
                seasonID = dtOrders.Rows[0]["SeasonID"].ToString();
                custPONo = dtOrders.Rows[0]["CustPONo"].ToString();
                brandID = dtOrders.Rows[0]["BrandID"].ToString();
            }

            string strXltName = Env.Cfg.XltPathDir + "\\Quality_P05_Detail_Report_ToPDF.xltx";
            Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return;
            }

            excel.DisplayAlerts = false;

            // 預設頁在第4頁，前3頁是用來複製的格式，最後在刪除
            // 依據 submitDate 複製分頁
            int defaultSheet = 4;
            for (int c = 1; c < dtdist.Rows.Count; c++)
            {
                Excel.Worksheet worksheetFirst = excel.ActiveWorkbook.Worksheets[defaultSheet];
                Excel.Worksheet worksheetn = excel.ActiveWorkbook.Worksheets[defaultSheet + c];
                worksheetFirst.Copy(worksheetn);
            }

            Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            // 依據 submitDate 填入表頭資訊
            for (int i = 0; i < dtdist.Rows.Count; i++)
            {
                worksheet = excel.ActiveWorkbook.Worksheets[i + defaultSheet];
                worksheet.Cells[4, 3] = dtdist.Rows[i]["submitDate"].ToString();
                worksheet.Cells[4, 6] = this.dateTestDate.Text;
                worksheet.Cells[4, 9] = this.txtSP.Text;
                worksheet.Cells[4, 14] = brandID;
                worksheet.Cells[6, 3] = styleID;
                worksheet.Cells[6, 9] = custPONo;
                worksheet.Cells[6, 14] = this.txtArticle.Text;
                worksheet.Cells[7, 3] = Convert.ToString(MyUtility.GetValue.Lookup($@"select StyleName from Style WITH (NOLOCK) where Ukey ='{styleUkey}'", null));
                worksheet.Cells[7, 9] = seasonID;
            }

            // 細項
            int setRow = 78; // 78 列為一頁
            int headerRow = 9; // 表頭那頁前9列為固定
            int signatureRow = 4; // 簽名有4列
            int frameRow = 34; // 框 33列 + 1 列空白
            int alladdSheet = 0;
            Excel.Worksheet worksheetDetail = excel.ActiveWorkbook.Worksheets[1];
            Excel.Worksheet worksheetSignature = excel.ActiveWorkbook.Worksheets[2];
            Excel.Worksheet worksheetFrame = excel.ActiveWorkbook.Worksheets[3];
            for (int i = 0; i < dtdist.Rows.Count; i++)
            {
                DataRow[] dr = dt.Select(MyUtility.Check.Empty(dtdist.Rows[i]["submitDate"]) ? $@"submitDate is null" : $"submitDate = '{dtdist.Rows[i]["submitDate"].ToString()}'");

                int underHeaderRow = setRow - headerRow;
                if (dr.Length > underHeaderRow)
                {
                    int overRow = dr.Length - underHeaderRow;
                    int addSheets = (int)Math.Ceiling(overRow * 1.0 / setRow);

                    // 有表頭那頁下方的細項格線
                    worksheet = excel.ActiveWorkbook.Worksheets[defaultSheet + alladdSheet + i];
                    for (int j = 0; j < underHeaderRow; j++)
                    {
                        Excel.Range paste1 = worksheet.get_Range($"A{headerRow + 1 + j}", Type.Missing);
                        Excel.Range r = worksheetDetail.get_Range("A1").EntireRow;
                        paste1.Insert(Excel.XlInsertShiftDirection.xlShiftDown, r.Copy(Type.Missing));

                        // 細項資料
                        this.SetDetailData(worksheet, j + headerRow + 1, dr[j]);
                    }

                    // 額外細項分頁
                    for (int k = 0; k < addSheets; k++)
                    {
                        // 新增細項分頁
                        Excel.Worksheet worksheetn = excel.ActiveWorkbook.Worksheets[defaultSheet + alladdSheet + i + k + 1];
                        worksheetDetail.Copy(worksheetn);

                        #region worksheetn 的細項格線
                        worksheetn = excel.ActiveWorkbook.Worksheets[defaultSheet + alladdSheet + i + k + 1];
                        worksheetn.get_Range("A1").EntireRow.Delete();

                        int addrow = overRow;
                        if (overRow > setRow)
                        {
                            addrow = setRow;
                            overRow -= setRow;
                        }
                        else
                        {
                            overRow = 0;
                        }

                        for (int j = 0; j < addrow; j++)
                        {
                            Excel.Range paste1 = worksheetn.get_Range($"A{j + 1}", Type.Missing);
                            Excel.Range r = worksheetDetail.get_Range("A1").EntireRow;
                            paste1.Insert(Excel.XlInsertShiftDirection.xlShiftDown, r.Copy(Type.Missing));

                            // 細項資料
                            this.SetDetailData(worksheetn, j + 1, dr[j + underHeaderRow + (k * setRow)]);
                        }
                        #endregion

                        int afterSignatureRow = 0;

                        // 簽名列
                        if (overRow <= 0)
                        {
                            if (addrow < setRow - signatureRow)
                            {
                                Excel.Range paste2 = worksheetn.get_Range($"A{addrow + 1}", Type.Missing);
                                Excel.Range r2 = worksheetSignature.get_Range("A1:A4").EntireRow;
                                paste2.Insert(Excel.XlInsertShiftDirection.xlShiftDown, r2.Copy(Type.Missing));
                                afterSignatureRow = addrow + 1 + signatureRow;
                            }
                            else
                            {
                                // 因簽名列塞不小，增加分頁
                                alladdSheet++;
                                worksheetn = excel.ActiveWorkbook.Worksheets[defaultSheet + alladdSheet + i + k + 1];
                                worksheetSignature.Copy(worksheetn);
                                afterSignatureRow = signatureRow;
                            }
                        }

                        #region 加入 4*10 的框
                        worksheetn = excel.ActiveWorkbook.Worksheets[defaultSheet + alladdSheet + i + k + 1];

                        // 共要加入幾組 frameNum
                        int frameNum = (int)Math.Ceiling(dr.Length * 1.0 / 2);

                        // 有簽名列那頁下方還有空間放下
                        if (afterSignatureRow <= setRow - frameRow)
                        {
                            Excel.Range paste2 = worksheetn.get_Range($"A{afterSignatureRow + 1}", Type.Missing);
                            Excel.Range r2 = worksheetFrame.get_Range("A9:A42").EntireRow;
                            paste2.Insert(Excel.XlInsertShiftDirection.xlShiftDown, r2.Copy(Type.Missing));
                            frameNum--;
                            afterSignatureRow += frameRow;

                            if (afterSignatureRow <= setRow - frameRow)
                            {
                                paste2 = worksheetn.get_Range($"A{afterSignatureRow + 1}", Type.Missing);
                                paste2.Insert(Excel.XlInsertShiftDirection.xlShiftDown, r2.Copy(Type.Missing));
                                frameNum--;
                            }
                        }

                        bool g1 = true;
                        for (int f = 0; f < frameNum; f++)
                        {
                            // 此頁第一組
                            if (g1)
                            {
                                alladdSheet++;
                                worksheetn = excel.ActiveWorkbook.Worksheets[defaultSheet + alladdSheet + i + k + 1];
                                worksheetFrame.Copy(worksheetn);
                            }

                            // 此頁第2組
                            else
                            {
                                worksheetn = excel.ActiveWorkbook.Worksheets[defaultSheet + alladdSheet + i + k + 1];
                                Excel.Range paste2 = worksheetn.get_Range($"A43", Type.Missing);
                                Excel.Range r2 = worksheetFrame.get_Range("A9:A42").EntireRow;
                                paste2.Insert(Excel.XlInsertShiftDirection.xlShiftDown, r2.Copy(Type.Missing));
                            }

                            g1 = !g1;
                        }
                        #endregion
                    }

                    alladdSheet += addSheets;
                }
                else
                {
                    // 有表頭那頁下方的細項格線
                    worksheet = excel.ActiveWorkbook.Worksheets[defaultSheet + alladdSheet + i];
                    for (int j = 0; j < dr.Length; j++)
                    {
                        Excel.Range paste1 = worksheet.get_Range($"A{headerRow + 1 + j}", Type.Missing);
                        Excel.Range r = worksheetDetail.get_Range("A1").EntireRow;
                        paste1.Insert(Excel.XlInsertShiftDirection.xlShiftDown, r.Copy(Type.Missing));

                        // 細項資料
                        this.SetDetailData(worksheet, j + headerRow + 1, dr[j]);
                    }

                    int afterSignatureRow;

                    // 簽名列
                    if (dr.Length < underHeaderRow - signatureRow)
                    {
                        Excel.Range paste2 = worksheet.get_Range($"A{dr.Length + headerRow + 1}", Type.Missing);
                        Excel.Range r2 = worksheetSignature.get_Range("A1:A4").EntireRow;
                        paste2.Insert(Excel.XlInsertShiftDirection.xlShiftDown, r2.Copy(Type.Missing));
                        afterSignatureRow = dr.Length + headerRow + signatureRow;
                    }
                    else
                    {
                        // 因簽名列塞不小，增加分頁
                        alladdSheet++;
                        Excel.Worksheet worksheetn = excel.ActiveWorkbook.Worksheets[defaultSheet + alladdSheet + i];
                        worksheetSignature.Copy(worksheetn);
                        afterSignatureRow = signatureRow;
                    }

                    #region 加入 4*10 的框
                    worksheet = excel.ActiveWorkbook.Worksheets[defaultSheet + alladdSheet + i];

                    // 共要加入幾組 frameNum
                    int frameNum = (int)Math.Ceiling(dr.Length * 1.0 / 2);

                    // 有簽名列那頁下方還有空間放下
                    if (afterSignatureRow <= setRow - frameRow)
                    {
                        Excel.Range paste2 = worksheet.get_Range($"A{afterSignatureRow + 1}", Type.Missing);
                        Excel.Range r2 = worksheetFrame.get_Range("A9:A42").EntireRow;
                        paste2.Insert(Excel.XlInsertShiftDirection.xlShiftDown, r2.Copy(Type.Missing));
                        frameNum--;
                        afterSignatureRow += frameRow;

                        if (afterSignatureRow <= setRow - frameRow)
                        {
                            paste2 = worksheet.get_Range($"A{afterSignatureRow + 1}", Type.Missing);
                            paste2.Insert(Excel.XlInsertShiftDirection.xlShiftDown, r2.Copy(Type.Missing));
                            frameNum--;
                        }
                    }

                    bool g1 = true;
                    for (int f = 0; f < frameNum; f++)
                    {
                        // 此頁第一組
                        if (g1)
                        {
                            alladdSheet++;
                            Excel.Worksheet worksheetn = excel.ActiveWorkbook.Worksheets[defaultSheet + alladdSheet + i];
                            worksheetFrame.Copy(worksheetn);
                        }

                        // 此頁第2組
                        else
                        {
                            Excel.Worksheet worksheetn = excel.ActiveWorkbook.Worksheets[defaultSheet + alladdSheet + i];
                            Excel.Range paste2 = worksheetn.get_Range($"A43", Type.Missing);
                            Excel.Range r2 = worksheetFrame.get_Range("A9:A42").EntireRow;
                            paste2.Insert(Excel.XlInsertShiftDirection.xlShiftDown, r2.Copy(Type.Missing));
                        }

                        g1 = !g1;
                    }
                    #endregion
                }
            }

            for (int i = 0; i < 3; i++)
            {
                worksheet = excel.ActiveWorkbook.Worksheets[1];
                worksheet.Delete();
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

        private void SetDetailData(Excel.Worksheet worksheet, int setRow, DataRow dr)
        {
            worksheet.Cells[setRow, 2] = dr["Refno"];
            worksheet.Cells[setRow, 3] = dr["Colorid"];
            worksheet.Cells[setRow, 4] = dr["Dyelot"];
            worksheet.Cells[setRow, 6] = dr["Roll"];
            worksheet.Cells[setRow, 7] = dr["Changescale"];
            worksheet.Cells[setRow, 9] = dr["ResultChange"];
            worksheet.Cells[setRow, 10] = dr["StainingScale"];
            worksheet.Cells[setRow, 11] = dr["ResultStain"];
            worksheet.Cells[setRow, 12] = MyUtility.Convert.GetString(dr["Temperature"]) + "˚C";
            worksheet.Cells[setRow, 14] = MyUtility.Convert.GetString(dr["Time"]) + " hrs";
            worksheet.Cells[setRow, 15] = dr["Remark"];
        }

        private void TxtuserInspector_Validating(object sender, CancelEventArgs e)
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
