using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.PublicPrg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.PPIC
{
//移除Public未註解所產生的警告
#pragma warning disable 1591
    public partial class P10_P11_Import : Sci.Win.Forms.Base
    {
        DataRow dr_master;
        DataTable dt_detail;
        string formCode;
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        public P10_P11_Import(DataRow master, DataTable detail,string FormCode)
        {
            InitializeComponent();
            dr_master = master;
            dt_detail = detail;
            formCode = FormCode;
            string Type;
            if (!MyUtility.Check.Empty(dr_master["Type"].ToString()))
            {
                Type = (dr_master["Type"].ToString() == "L") ? " Lacking" : " Replacement";    
            }
            else
            {
                Type = "";
            }
            
            this.Text = "Batch Import" + " (" + formCode + Type + ") SP#= " + dr_master["Poid"].ToString();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            DataTable dt;
            Ict.DualResult result;
            string TypeSql = (formCode == "Fabric") ? " and FabricType = 'F'" : " and FabricType = 'A'";
            string sqlcmd = string.Format(Prgs.selePoItemSqlCmd, dr_master["POID"].ToString())+ TypeSql;
            if (result=DBProxy.Current.Select(null,sqlcmd,out dt))
            {
                if (dt.Rows.Count>0)
                {
                    dt.Columns.Add("Selected", typeof(bool));                                      
                }
                listControlBindingSource1.DataSource = dt;
               
            }
            else
            {
                ShowErr(sqlcmd.ToString(), result);
            }
            this.gridImport.IsEditingReadOnly = false;
            this.gridImport.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.gridImport)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)        
                .Text("seq", header: "Seq#", iseditingreadonly: true, width: Widths.AnsiChars(6)) 
                .Text("refno", header: "Ref#", iseditingreadonly: true, width: Widths.AnsiChars(8)) 
                .EditText("Description", header: "Description", iseditingreadonly: true, width: Widths.AnsiChars(35)) 
                .Text("colorid", header: "Color", iseditingreadonly: true, width: Widths.AnsiChars(8))
                .Text("SizeSpec", header: "Size", iseditingreadonly: true, width: Widths.AnsiChars(8)) 
                .Date("FinalETA", header: "ETA",iseditingreadonly:true,width: Widths.AnsiChars(10))
                .Numeric("inqty", header: "In Qty",iseditingreadonly:true,width: Widths.AnsiChars(6))
                .Numeric("stockunit", header: "Stock Unit", iseditingreadonly: true, width: Widths.AnsiChars(6))
                .Numeric("outqty", header: "Out Qty", iseditingreadonly: true, width: Widths.AnsiChars(6))
                .Numeric("adjustqty", header: "Adqty", iseditingreadonly: true, width: Widths.AnsiChars(6))
                .Numeric("balance", header: "Balance", iseditingreadonly: true, width: Widths.AnsiChars(6))
                .Numeric("linvqty", header: "Inventory Qty", iseditingreadonly: true, width: Widths.AnsiChars(6))        
               ;
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(gridImport) || gridImport.Rows.Count==0)
            {
                return;
            }
            DataTable dtGridBS1 = (DataTable)listControlBindingSource1.DataSource;
            dtGridBS1.Columns.Add("WhseInQty",typeof(decimal));
            dtGridBS1.Columns.Add("FTYInQty", typeof(decimal));
            dtGridBS1.Columns.Add("FTYLastRecvDate", typeof(DateTime));
            DataRow[] dr2 = dtGridBS1.Select("Selected = 1");

            foreach (DataRow tmp in dr2)
            {
                DataRow[] findrow = dt_detail.Select(string.Format(@"seq1='{0}' and seq2='{1}' and refno='{2}'", tmp["seq1"], tmp["seq2"], tmp["refno"]));
                if (findrow.Length > 0)
                {
                    DataTable WHdata;
                    DualResult whdr = DBProxy.Current.Select(null, string.Format("SELECT m.InQty,m.OutQty FROM MDivisionPoDetail m WITH (NOLOCK) inner join Orders o WITH (NOLOCK) on m.POID=o.ID inner join Factory f WITH (NOLOCK) on f.ID=o.FtyGroup WHERE m.POID = '{0}' AND m.Seq1 = '{1}' AND m.Seq2 = '{2}' AND f.MDivisionID = '{3}'", MyUtility.Convert.GetString(dr_master["POID"]), MyUtility.Convert.GetString(tmp["Seq1"]), MyUtility.Convert.GetString(tmp["Seq2"]), Sci.Env.User.Keyword), out WHdata);
                    if (whdr)
                    {
                        if (WHdata.Rows.Count > 0)
                        {
                            findrow[0]["WhseInQty"] = MyUtility.Convert.GetDecimal(WHdata.Rows[0]["InQty"]);
                            findrow[0]["FTYInQty"] = MyUtility.Convert.GetDecimal(WHdata.Rows[0]["OutQty"]);
                        }
                    }
                    DateTime? maxIssueDate = MaxIssueDate(MyUtility.Convert.GetString(tmp["Seq1"]), MyUtility.Convert.GetString(tmp["Seq2"]));
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
                    DataTable WHdata;
                    DualResult whdr = DBProxy.Current.Select(null, string.Format("SELECT m.InQty,m.OutQty FROM MDivisionPoDetail m WITH (NOLOCK) inner join Orders o WITH (NOLOCK) on m.POID=o.ID inner join Factory f WITH (NOLOCK) on f.ID=o.FtyGroup WHERE m.POID = '{0}' AND m.Seq1 = '{1}' AND m.Seq2 = '{2}' AND f.MDivisionID = '{3}'", MyUtility.Convert.GetString(dr_master["POID"]), MyUtility.Convert.GetString(tmp["Seq1"]), MyUtility.Convert.GetString(tmp["Seq2"]), Sci.Env.User.Keyword), out WHdata);
                    if (whdr)
                    {
                        if (WHdata.Rows.Count > 0)
                        {
                            tmp["WhseInQty"] = MyUtility.Convert.GetDecimal(WHdata.Rows[0]["InQty"]);
                            tmp["FTYInQty"] = MyUtility.Convert.GetDecimal(WHdata.Rows[0]["OutQty"]);
                        }
                    }
                    DateTime? maxIssueDate = MaxIssueDate(MyUtility.Convert.GetString(tmp["Seq1"]), MyUtility.Convert.GetString(tmp["Seq2"]));
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
                    dt_detail.ImportRow(tmp);
                }
               
            }
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private DateTime? MaxIssueDate(string Seq1, string Seq2)
        {
            DateTime? maxIssueDate = null;
            DataTable issueData;
            string sqlCmd = string.Format("select max(i.IssueDate) as IssueDate from Issue i WITH (NOLOCK) , Issue_Detail id WITH (NOLOCK) where i.Id = id.Id and id.PoId = '{0}' and id.Seq1 = '{1}' and id.Seq2 = '{2}'", MyUtility.Convert.GetString(dr_master["POID"]), Seq1, Seq2);
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out issueData);
            if (result)
            {
                maxIssueDate = MyUtility.Convert.GetDate(issueData.Rows[0]["IssueDate"]);
            }

            return maxIssueDate;
        }
    }
}
