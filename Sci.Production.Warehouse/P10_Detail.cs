using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Sci;
using Ict.Win;
using Ict;
using Sci.Data;
using System.Linq;

namespace Sci.Production.Warehouse
{
    public partial class P10_Detail : Sci.Win.Subs.Input8A
    {
        public P10_Detail()
        {
            InitializeComponent();
            this.KeyField1 = "id";
            this.KeyField2 = "Issue_SummaryUkey";
        }

        protected override void OnSubDetailInsert(int index = -1)
        {
            var frm = new Sci.Production.Warehouse.P10_Detail_Detail(CurrentDetailData, (DataTable)gridbs.DataSource);
            frm.ShowDialog(this);
            //base.OnSubDetailInsert(index);
            //CurrentSubDetailData["Issue_SummaryUkey"] = 0;
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            DataTable temp =(DataTable)gridbs.DataSource;
            if (!temp.Columns.Contains("balanceqty"))
            {
                DataTable dtFtyinventory;
                StringBuilder strSQLCmd = new StringBuilder();

                #region -- sqlcmd query --
                strSQLCmd.Append(string.Format(@"select ISD.* , FTY.InQty,FTY.OutQty,FTY.AdjustQty,FTY.InQty-FTY.OutQty+FTY.AdjustQty as balanceqty,[location]=dbo.Getlocation(FTY.Ukey)   
	from [dbo].[Issue_Detail] ISD 
	left join [dbo].FtyInventory FTY on ISD.FtyInventoryUkey=FTY.Ukey
	where ISD.Id='{0}' and ISD.Issue_SummaryUkey='{1}'"
                    , CurrentDetailData["id"].ToString(), CurrentDetailData["Ukey"].ToString()));
                #endregion

                Ict.DualResult result;
                if (result = DBProxy.Current.Select(null, strSQLCmd.ToString(), out dtFtyinventory))
                {
                    gridbs.DataSource = dtFtyinventory;
                    dtFtyinventory.DefaultView.Sort = "seq1,seq2,location,dyelot,balanceqty desc";
                }
                else { ShowErr(strSQLCmd.ToString(), result); }
            }

            this.dis_ID.Text = CurrentDetailData["id"].ToString();
            this.dis_scirefno.Text = CurrentDetailData["scirefno"].ToString();
            this.dis_poid.Text = CurrentDetailData["poid"].ToString();
            this.dis_colorid.Text = CurrentDetailData["colorid"].ToString();
            //this.dis_sizespec.Text = CurrentDetailData["sizespec"].ToString();
            this.dis_desc.Text = CurrentDetailData["description"].ToString();
            this.num_requestqty.Text = CurrentDetailData["requestqty"].ToString();
            this.num_accuIssue.Text = CurrentDetailData["accu_issue"].ToString();
       
            string STRrequestqty = CurrentDetailData["requestqty"].ToString();
            string STRaccu_issue = CurrentDetailData["accu_issue"].ToString();
            decimal DECrequestqty;
            decimal DECaccu_issue;
            if (!decimal.TryParse(STRrequestqty, out DECrequestqty))
            { DECrequestqty = 0; }
            if (!decimal.TryParse(STRaccu_issue, out DECaccu_issue))
            { DECaccu_issue = 0; }
            this.num_balance.Value = DECrequestqty - DECaccu_issue;
            this.num_issue.Text = CurrentDetailData["qty"].ToString();
            string STRqty = CurrentDetailData["qty"].ToString();
            decimal DECqty;
            if (!decimal.TryParse(STRqty, out DECqty))
            { DECqty = 0; }
            this.num_variance.Value = DECrequestqty - DECaccu_issue - DECqty;
        }

        protected override bool OnGridSetup()
        {
            Helper.Controls.Grid.Generator(this.grid)
                //.Text("id", header: "id", width: Widths.AnsiChars(13), iseditingreadonly: true)  //0
                //.Numeric("Issue_SummaryUkey", header: "Issue_SummaryUkey", width: Widths.AnsiChars(8), integer_places: 10)    //6
            //.Text("FtyInventoryUkey", header: "FtyInventory" + Environment.NewLine + "Ukey", width: Widths.AnsiChars(8), iseditingreadonly: true)    //0
            .CellPOIDWithSeqRollDyelot("poid", header: "poid", width: Widths.AnsiChars(14), iseditingreadonly: true)  //1
            .Text("seq1", header: "seq1", width: Widths.AnsiChars(4), iseditingreadonly: true)  //2
            .Text("seq2", header: "seq2", width: Widths.AnsiChars(3), iseditingreadonly: true)  //3
            .Text("roll", header: "roll", width: Widths.AnsiChars(10), iseditingreadonly: true)  //4
            .Text("dyelot", header: "dyelot", width: Widths.AnsiChars(6), iseditingreadonly: true)  //5
            .Numeric("qty", header: "Issue Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 8)    //6
            .Text("location", header: "Bulk" + Environment.NewLine + "Location", width: Widths.AnsiChars(10), iseditingreadonly: true)  //5
            .Numeric("inqty", header: "In Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 8, iseditingreadonly: true)    //6
            .Numeric("outqty", header: "Out Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 8, iseditingreadonly: true)    //6
            .Numeric("adjustqty", header: "Adjust" + Environment.NewLine + "Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 8, iseditingreadonly: true)    //6
            .Numeric("balanceqty", header: "Balance" + Environment.NewLine + "Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 8, iseditingreadonly: true)    //6
            ;     //

            this.grid.Columns[5].DefaultCellStyle.BackColor = Color.Pink;

            return true;
        }

        protected override bool OnSaveBefore()
        {
            grid.ValidateControl();
            var dr2 = ((DataTable)gridbs.DataSource).Select("qty = 0");
            if (dr2.Length > 0)
            {
                MyUtility.Msg.WarningBox("Issue Qty of selected row can't be zero!", "Warning");
                return false;
            }
            var dr2_ba = ((DataTable)gridbs.DataSource).Select("qty > balanceqty");
            if (dr2_ba.Length > 0)
            {
                MyUtility.Msg.WarningBox("Issue Qty of selected row can't be more then balance qty!", "Warning");
                return false;
            }
            return base.OnSaveBefore();
        } 

        private void btnAutoPick_Click(object sender, EventArgs e)
        {
            var issued = PublicPrg.Prgs.autopick(CurrentDetailData);
            if (issued == null)
            {
                return;
            }

            DataTable subDT = (DataTable)gridbs.DataSource;

            subDT.Clear();
            foreach (DataRow dr2 in issued)
            {
                dr2.AcceptChanges();
                dr2.SetAdded();
                subDT.ImportRow(dr2);
            }

        }
    }
}
