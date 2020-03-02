using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci;
using Sci.Data;

namespace Sci.Production.Warehouse
{
    public partial class P33_Detail_Detail : Sci.Win.Subs.Base
    {
        public Sci.Win.Subs.Base P33_Detail;
        DataRow dr_master;
        DataTable dt_detail;
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        protected DataTable dtFtyinventory;

        public P33_Detail_Detail(DataRow master, DataTable detail)
        {
            InitializeComponent();
            dr_master = master;
            dt_detail = detail;
        }

        private void sum_checkedqty()
        {
            listControlBindingSource1.EndEdit();
            DataTable dt = (DataTable)listControlBindingSource1.DataSource;
            Object localPrice = dt.Compute("Sum(IssueQty)", "selected = 1");

            this.numIssueQty.Value = Convert.ToDecimal(dt.Compute("Sum(IssueQty)", "selected = 1"));

            //if (!MyUtility.Check.Empty(localPrice) && !MyUtility.Check.Empty(dr_master["Use Qty By Stock Unit"].ToString()))
            //{
            //    numRequestVariance.Value = Convert.ToDecimal(dr_master["Use Qty By Stock Unit"].ToString()) - Convert.ToDecimal(localPrice.ToString());
            //}
            //this.displayTotalQty.Value = localPrice.ToString();              
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.displayRefno.Text = dr_master["refno"].ToString();
            this.displaySPNo.Text = dr_master["poid"].ToString();
            this.displayColorID.Text = dr_master["SuppColor"].ToString();
            this.editDesc.Text = dr_master["DescDetail"].ToString();

            StringBuilder strSQLCmd = new StringBuilder();
            #region -- sqlcmd query -- 

            strSQLCmd.Append($@"
select 0 as selected 
       , PoId = a.id
       , a.Seq1
       , a.Seq2
       , [Description] = dbo.getmtldesc(a.id,a.seq1,a.seq2,2,0)
       , [BulkQty] = c.inqty-c.outqty + c.adjustqty
	   , [IssueQty]=0.00
	   , [BulkLocation]=FTYD.MtlLocationID
from dbo.PO_Supp_Detail a WITH (NOLOCK) 
inner join dbo.ftyinventory c WITH (NOLOCK) on c.poid = a.id and c.seq1 = a.seq1 and c.seq2  = a.seq2 
Left JOIN FtyInventory_Detail FTYD WITH (NOLOCK)  ON FTYD.Ukey= c.Ukey
Where a.id = '{dr_master["poid"]}' 
and a.SCIRefno='{dr_master["SCIRefno"]}' 
and a.SuppColor='{dr_master["SuppColor"]}'
and c.stocktype = 'B'

");
            #endregion

            P33_Detail.ShowWaitMessage("Data Loading....");
            Ict.DualResult result;
            if (result = DBProxy.Current.Select(null, strSQLCmd.ToString(), out dtFtyinventory))
            {
                if (dtFtyinventory.Rows.Count == 0)
                { MyUtility.Msg.WarningBox("Data not found!!"); }
                listControlBindingSource1.DataSource = dtFtyinventory;
                //dtFtyinventory.DefaultView.Sort = "dyelot,balanceqty desc";
            }
            else { ShowErr(strSQLCmd.ToString(), result); }
            P33_Detail.HideWaitMessage();

            Ict.Win.DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            ns.CellValidating += (s, e) =>
                {
                    if (this.EditMode && !MyUtility.Check.Empty(e.FormattedValue))
                    {
                        gridRollNo.GetDataRow(gridRollNo.GetSelectedRowIndex())["IssueQty"] = e.FormattedValue;
                        gridRollNo.GetDataRow(gridRollNo.GetSelectedRowIndex())["selected"] = true;
                        this.sum_checkedqty();
                    }
                };
            
            this.gridRollNo.CellValueChanged += (s, e) =>
            {
                if (gridRollNo.Columns[e.ColumnIndex].Name == col_chk.Name)
                {
                    DataRow dr = gridRollNo.GetDataRow(e.RowIndex);
                    if (Convert.ToBoolean(dr["selected"]) == true && Convert.ToDecimal(dr["IssueQty"].ToString()) == 0)
                    {
                        dr["IssueQty"] = dr["BulkQty"];
                    }
                    else if (Convert.ToBoolean(dr["selected"]) == false)
                    {
                        dr["IssueQty"] = 0;
                    }
                    dr.EndEdit();
                    this.sum_checkedqty();
                }
            };
            
            this.gridRollNo.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.gridRollNo.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.gridRollNo)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)
                .Text("seq1", header: "Seq1", iseditingreadonly: true, width: Widths.AnsiChars(6))
                .Text("seq2", header: "Seq2", iseditingreadonly: true, width: Widths.AnsiChars(6))
                .Numeric("BulkQty", header: "Bulk Qty", iseditingreadonly: true, decimal_places: 2, integer_places: 10) 
                .Numeric("IssueQty", header: "Issue Qty", iseditable: true, decimal_places: 2, integer_places: 10, settings: ns)  
                .Text("BulkLocation", header: "Bulk Location", iseditingreadonly: true) 
                ;

            this.gridRollNo.Columns["IssueQty"].DefaultCellStyle.BackColor = Color.Pink;

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            //listControlBindingSource1.EndEdit();
            gridRollNo.ValidateControl();
            DataTable dtGridBS1 = (DataTable)listControlBindingSource1.DataSource;
            if (MyUtility.Check.Empty(dtGridBS1) || dtGridBS1.Rows.Count == 0) return;

            DataRow[] dr2 = dtGridBS1.Select("Selected = 1");
            if (dr2.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select rows first!", "Warnning");
                return;
            }

            dr2 = dtGridBS1.Select("qty = 0 and Selected = 1");
            if (dr2.Length > 0)
            {
                MyUtility.Msg.WarningBox("Issue Qty of selected row can't be zero!", "Warning");
                return;
            }

            dr2 = dtGridBS1.Select("qty > balanceqty and Selected = 1");
            if (dr2.Length > 0)
            {
                MyUtility.Msg.WarningBox("Issue Qty of selected row can't be more then balance qty!", "Warning");
                return;
            }

            dr2 = dtGridBS1.Select("qty <> 0 and Selected = 1");
            foreach (DataRow tmp in dr2)
            {
                //DataRow[] findrow = dt_detail.Select(string.Format("ftyinventoryukey = {0}" , tmp["ftyinventoryukey"]));
                DataRow[] findrow = dt_detail.Select(string.Format("poid = '{0}' and seq1 = '{1}' and seq2 = '{2}' and roll = '{3}' and dyelot = '{4}'",
                    tmp["poid"].ToString(), tmp["seq1"].ToString(), tmp["seq2"].ToString(), tmp["roll"].ToString(), tmp["dyelot"].ToString()));

                if (findrow.Length > 0)
                {
                    findrow[0]["qty"] = tmp["qty"];
                }
                else
                {
                    tmp["id"] = dr_master["id"];
                    tmp.AcceptChanges();
                    tmp.SetAdded();
                    dt_detail.ImportRow(tmp);
                }
            }


            this.Close();
        }
    }
}
