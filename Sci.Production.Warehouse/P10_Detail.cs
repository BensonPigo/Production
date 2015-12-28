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
            this.dis_ID.Text = CurrentDetailData["id"].ToString();
            this.dis_scirefno.Text = CurrentDetailData["scirefno"].ToString();
            this.dis_poid.Text = CurrentDetailData["poid"].ToString();
            this.dis_colorid.Text = CurrentDetailData["colorid"].ToString();
            this.dis_sizespec.Text = CurrentDetailData["sizespec"].ToString();
            this.dis_desc.Text = CurrentDetailData["description"].ToString();
            this.num_requestqty.Text = CurrentDetailData["requestqty"].ToString();
            this.num_accuIssue.Text = CurrentDetailData["accu_issue"].ToString();
            this.num_balance.Value = (decimal)CurrentDetailData["requestqty"] - (decimal)CurrentDetailData["accu_issue"];
            this.num_issue.Text = CurrentDetailData["qty"].ToString();
            this.num_variance.Value = (decimal)CurrentDetailData["requestqty"] - (decimal)CurrentDetailData["accu_issue"] - (decimal)CurrentDetailData["qty"];
        }

        protected override bool OnGridSetup()
        {
            Helper.Controls.Grid.Generator(this.grid)
                //.Text("id", header: "id", width: Widths.AnsiChars(13), iseditingreadonly: true)  //0
                //.Numeric("Issue_SummaryUkey", header: "Issue_SummaryUkey", width: Widths.AnsiChars(8), integer_places: 10)    //6
            .Text("FtyInventoryUkey", header: "FtyInventory" + Environment.NewLine + "Ukey", width: Widths.AnsiChars(8), iseditingreadonly: true)    //0
            .CellPOIDWithSeqRollDyelot("poid", header: "poid", width: Widths.AnsiChars(14))  //1
            .Text("seq1", header: "seq1", width: Widths.AnsiChars(4))  //2
            .Text("seq2", header: "seq2", width: Widths.AnsiChars(3))  //3
            .Text("roll", header: "roll", width: Widths.AnsiChars(10))  //4
            .Text("dyelot", header: "dyelot", width: Widths.AnsiChars(6))  //5
            .Numeric("qty", header: "Issue Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 8)    //6
            .Text("location", header: "Bulk" + Environment.NewLine + "Location", width: Widths.AnsiChars(10), iseditingreadonly: true)  //5
            .Numeric("inqty", header: "In Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 8, iseditingreadonly: true)    //6
            .Numeric("outqty", header: "Out Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 8, iseditingreadonly: true)    //6
            .Numeric("adjustqty", header: "Adjust" + Environment.NewLine + "Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 8, iseditingreadonly: true)    //6
            .Numeric("balanceqty", header: "Balance" + Environment.NewLine + "Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 8, iseditingreadonly: true)    //6
            ;     //

            this.grid.Columns[6].DefaultCellStyle.BackColor = Color.Pink;

            return true;
        }

        protected override bool OnSaveBefore()
        {
            var dr2 = ((DataTable)gridbs.DataSource).Select("qty = 0");
            if (dr2.Length > 0)
            {
                MyUtility.Msg.WarningBox("Issue Qty of selected row can't be zero!", "Warning");
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
