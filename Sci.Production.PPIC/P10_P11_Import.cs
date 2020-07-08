using Ict;
using Sci.Data;
using Sci.Production.PublicPrg;
using System;
using System.Data;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// P10_P11_Import
    /// </summary>
    public partial class P10_P11_Import : Win.Forms.Base
    {
        private DataRow dr_master;
        private DataTable dt_detail;
        private string formCode;
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;

        /// <summary>
        /// Dr_master
        /// </summary>
        public DataRow Dr_master
        {
            get
            {
                return this.dr_master;
            }

            set
            {
                this.dr_master = value;
            }
        }

        /// <summary>
        /// Dt_detail
        /// </summary>
        public DataTable Dt_detail
        {
            get
            {
                return this.dt_detail;
            }

            set
            {
                this.dt_detail = value;
            }
        }

        /// <summary>
        /// FormCode
        /// </summary>
        public string FormCode
        {
            get
            {
                return this.formCode;
            }

            set
            {
                this.formCode = value;
            }
        }

        /// <summary>
        /// P10_P11_Import
        /// </summary>
        /// <param name="master">DataRow master</param>
        /// <param name="detail">DataTable detail</param>
        /// <param name="formCode">string formCode</param>
        public P10_P11_Import(DataRow master, DataTable detail, string formCode)
        {
            this.InitializeComponent();
            this.Dr_master = master;
            this.Dt_detail = detail;
            this.FormCode = formCode;
            string type;
            if (!MyUtility.Check.Empty(this.Dr_master["Type"].ToString()))
            {
                type = (this.Dr_master["Type"].ToString() == "L") ? " Lacking" : " Replacement";
            }
            else
            {
                type = string.Empty;
            }

            this.Text = "Batch Import" + " (" + this.FormCode + type + ") SP#= " + this.Dr_master["Poid"].ToString();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            DataTable dt;
            DualResult result;
            string typeSql = (this.FormCode == "Fabric") ? " and FabricType = 'F'" : " and FabricType = 'A'";
            string sqlcmd = string.Format(Prgs.selePoItemSqlCmd(), this.Dr_master["POID"].ToString()) + typeSql;
            if (result = DBProxy.Current.Select(null, sqlcmd, out dt))
            {
                if (dt.Rows.Count > 0)
                {
                    dt.Columns.Add("Selected", typeof(bool));
                }

                this.listControlBindingSource1.DataSource = dt;
            }
            else
            {
                this.ShowErr(sqlcmd.ToString(), result);
            }

            this.gridImport.IsEditingReadOnly = false;
            this.gridImport.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridImport)
                .CheckBox("Selected", header: string.Empty, width: Ict.Win.Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
                .Text("seq", header: "Seq#", iseditingreadonly: true, width: Ict.Win.Widths.AnsiChars(6))
                .Text("refno", header: "Ref#", iseditingreadonly: true, width: Ict.Win.Widths.AnsiChars(8))
                .EditText("Description", header: "Description", iseditingreadonly: true, width: Ict.Win.Widths.AnsiChars(35))
                .Text("colorid", header: "Color", iseditingreadonly: true, width: Ict.Win.Widths.AnsiChars(8))
                .Text("SizeSpec", header: "Size", iseditingreadonly: true, width: Ict.Win.Widths.AnsiChars(8))
                .Date("FinalETA", header: "ETA", iseditingreadonly: true, width: Ict.Win.Widths.AnsiChars(10))
                .Numeric("inqty", header: "In Qty", iseditingreadonly: true, width: Ict.Win.Widths.AnsiChars(6))
                .Numeric("stockunit", header: "Stock Unit", iseditingreadonly: true, width: Ict.Win.Widths.AnsiChars(6))
                .Numeric("outqty", header: "Out Qty", iseditingreadonly: true, width: Ict.Win.Widths.AnsiChars(6))
                .Numeric("adjustqty", header: "Adqty", iseditingreadonly: true, width: Ict.Win.Widths.AnsiChars(6))
                .Numeric("balance", header: "Balance", iseditingreadonly: true, width: Ict.Win.Widths.AnsiChars(6))
                .Numeric("linvqty", header: "Inventory Qty", iseditingreadonly: true, width: Ict.Win.Widths.AnsiChars(6))
                .Text("Status", header: "Status", iseditingreadonly: true, width: Ict.Win.Widths.AnsiChars(8))
               ;
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.gridImport) || this.gridImport.Rows.Count == 0)
            {
                return;
            }

            DataTable dtGridBS1 = (DataTable)this.listControlBindingSource1.DataSource;
            dtGridBS1.Columns.Add("WhseInQty", typeof(decimal));
            dtGridBS1.Columns.Add("FTYInQty", typeof(decimal));
            dtGridBS1.Columns.Add("FTYLastRecvDate", typeof(DateTime));
            DataRow[] dr2 = dtGridBS1.Select("Selected = 1");

            foreach (DataRow tmp in dr2)
            {
                DataRow[] findrow = this.Dt_detail.Select(string.Format(@"seq1='{0}' and seq2='{1}' and refno='{2}'", tmp["seq1"], tmp["seq2"], tmp["refno"]));
                if (findrow.Length > 0)
                {
                    DataTable wHdata;
                    DualResult whdr = DBProxy.Current.Select(null, string.Format("SELECT m.InQty,m.OutQty FROM MDivisionPoDetail m WITH (NOLOCK) inner join Orders o WITH (NOLOCK) on m.POID=o.ID inner join Factory f WITH (NOLOCK) on f.ID=o.FtyGroup WHERE m.POID = '{0}' AND m.Seq1 = '{1}' AND m.Seq2 = '{2}' AND f.MDivisionID = '{3}'", MyUtility.Convert.GetString(this.Dr_master["POID"]), MyUtility.Convert.GetString(tmp["Seq1"]), MyUtility.Convert.GetString(tmp["Seq2"]), Sci.Env.User.Keyword), out wHdata);
                    if (whdr)
                    {
                        if (wHdata.Rows.Count > 0)
                        {
                            findrow[0]["WhseInQty"] = MyUtility.Convert.GetDecimal(wHdata.Rows[0]["InQty"]);
                            findrow[0]["FTYInQty"] = MyUtility.Convert.GetDecimal(wHdata.Rows[0]["OutQty"]);
                        }
                    }

                    DateTime? maxIssueDate = this.MaxIssueDate(MyUtility.Convert.GetString(tmp["Seq1"]), MyUtility.Convert.GetString(tmp["Seq2"]));
                    if (MyUtility.Check.Empty(maxIssueDate))
                    {
                        findrow[0]["FTYLastRecvDate"] = DBNull.Value;
                    }
                    else
                    {
                        findrow[0]["FTYLastRecvDate"] = maxIssueDate;
                    }
                }
                else
                {
                    DataTable wHdata1;
                    DualResult whdr = DBProxy.Current.Select(null, string.Format("SELECT m.InQty,m.OutQty FROM MDivisionPoDetail m WITH (NOLOCK) inner join Orders o WITH (NOLOCK) on m.POID=o.ID inner join Factory f WITH (NOLOCK) on f.ID=o.FtyGroup WHERE m.POID = '{0}' AND m.Seq1 = '{1}' AND m.Seq2 = '{2}' AND f.MDivisionID = '{3}'", MyUtility.Convert.GetString(this.Dr_master["POID"]), MyUtility.Convert.GetString(tmp["Seq1"]), MyUtility.Convert.GetString(tmp["Seq2"]), Sci.Env.User.Keyword), out wHdata1);
                    if (whdr)
                    {
                        if (wHdata1.Rows.Count > 0)
                        {
                            tmp["WhseInQty"] = MyUtility.Convert.GetDecimal(wHdata1.Rows[0]["InQty"]);
                            tmp["FTYInQty"] = MyUtility.Convert.GetDecimal(wHdata1.Rows[0]["OutQty"]);
                        }
                    }

                    DateTime? maxIssueDate = this.MaxIssueDate(MyUtility.Convert.GetString(tmp["Seq1"]), MyUtility.Convert.GetString(tmp["Seq2"]));
                    if (MyUtility.Check.Empty(maxIssueDate))
                    {
                        tmp["FTYLastRecvDate"] = DBNull.Value;
                    }
                    else
                    {
                        tmp["FTYLastRecvDate"] = maxIssueDate;
                    }

                    tmp.AcceptChanges();
                    tmp.SetAdded();
                    this.Dt_detail.ImportRow(tmp);
                }
            }

            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private DateTime? MaxIssueDate(string seq1, string seq2)
        {
            DateTime? maxIssueDate = null;
            DataTable issueData;
            string sqlCmd = string.Format("select max(i.IssueDate) as IssueDate from Issue i WITH (NOLOCK) , Issue_Detail id WITH (NOLOCK) where i.Id = id.Id and id.PoId = '{0}' and id.Seq1 = '{1}' and id.Seq2 = '{2}'", MyUtility.Convert.GetString(this.Dr_master["POID"]), seq1, seq2);
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out issueData);
            if (result)
            {
                maxIssueDate = MyUtility.Convert.GetDate(issueData.Rows[0]["IssueDate"]);
            }

            return maxIssueDate;
        }
    }
}
