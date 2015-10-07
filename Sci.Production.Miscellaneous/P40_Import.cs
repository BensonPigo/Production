using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Sci;
using Sci.Win;
using Sci.Win.Tools;
using Ict;
using Ict.Data;

namespace Sci.Production.Miscellaneous
{
    public partial class P40_Import : Sci.Win.Subs.Base
    {
        private DataTable detTable;
        private DataTable gridTable;
        private string suppid;
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        public P40_Import(DataTable detTable,string localSuppid)
        {
            InitializeComponent();
            this.detTable = detTable;
            this.suppid = localSuppid;
        }
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            DataGridViewGeneratorNumericColumnSettings QtyCell = new DataGridViewGeneratorNumericColumnSettings();
            QtyCell.CellValidating += (s, e) =>
            {
                if (e.RowIndex == -1 || !this.EditMode) return;
                int index = e.RowIndex;
                if (e.FormattedValue == null) return;
                gridTable.Rows[index]["Amount"] = (decimal)e.FormattedValue * (decimal)gridTable.Rows[index]["Price"];
                gridTable.Rows[index]["Qty"] = (decimal)e.FormattedValue;
                gridTable.Rows[index].EndEdit();
            };
            this.grid1.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.grid1.DataSource = gridTable;
            Helper.Controls.Grid.Generator(this.grid1)
            .CheckBox("Sel", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)
            .Text("MiscPOID", header: "PO#", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("SEQ1", header: "SEQ1", width: Widths.AnsiChars(3), iseditingreadonly: true)
            .Text("SEQ2", header: "SEQ2", width: Widths.AnsiChars(2), iseditingreadonly: true)
            .Text("MiscID", header: "Miscellaneous", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("Description", header: "Description", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("MiscReqid", header: "Miscellaneous Req#", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("PurchaseType", header: "Type", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("Projectid", header: "Project Name", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Numeric("InQty", header: "Accu. In Qty", width: Widths.AnsiChars(5), integer_places: 6, decimal_places: 2, iseditingreadonly: true)
            .Numeric("ApQty", header: "Accu. Paid Qty", width: Widths.AnsiChars(5), integer_places: 6, decimal_places: 2, iseditingreadonly: true)
            .CheckBox("Inspect", header: "Need to Inspect", iseditable: false)
            .Numeric("InspQty", header: "Accu. Insp. Qty", width: Widths.AnsiChars(5), integer_places: 6, decimal_places: 2, iseditingreadonly: true)
            .Numeric("Balance", header: "Balance", width: Widths.AnsiChars(5), integer_places: 6, decimal_places: 2, iseditingreadonly: true)
            .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(5), integer_places: 6, decimal_places: 2, settings: QtyCell)
            .Text("Unitid", header: "Unit", width: Widths.AnsiChars(5), iseditingreadonly: true)
            .Numeric("Price", header: "Price", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 4, iseditingreadonly: true)
            .Numeric("Amount", header: "Amount", width: Widths.AnsiChars(10), integer_places: 13, decimal_places: 4, iseditingreadonly: true);
            grid1.Columns[14].DefaultCellStyle.BackColor = Color.Pink;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string poId1 = this.textBox1.Text, poId2 = this.textBox2.Text;
            if (MyUtility.Check.Empty(poId1) && MyUtility.Check.Empty(poId2))
            {
                MyUtility.Msg.WarningBox("<PO#> can not empty.");
                this.textBox1.Focus();
                return;
            }
            //DataTable gridTable2;
            string sql = string.Format(
            @" Select * from (select 0 as Sel ,a.ID as Miscpoid,a.SEQ1,a.Seq2,a.Miscid,b.description,
            b.Unitid,a.price,c.PurchaseType,a.projectid,b.inspect,a.departmentid,inspQty,MiscReqid,apqty,
            a.price * a.qty as Amount,inQty,iif(b.Inspect=1,a.InspQty -a.ApQty,a.InQty-a.ApQty) as balance,
            iif(b.Inspect=1,a.InspQty -a.ApQty,a.InQty-a.ApQty) as Qty 
            from MiscPO e,MiscPO_Detail a
            Left Join Misc b on b.id = a.MiscID
            Left Join MiscReq c on a.miscreqid = c.id
            where a.id >= '{0}' and a.id <='{2}' and e.id=a.id and e.localsuppid ='{1}' and e.Status = 'Approved' and (InQty-ApQty>0 or inspQty-apQty >0)) tmp where balance > 0 ", poId1, this.suppid,poId2);

            DualResult rst = DBProxy.Current.Select(null, sql, out gridTable);
           
            if (rst)
            {
                if (gridTable.Rows.Count == 0)
                { MyUtility.Msg.WarningBox("Data not found!!"); }

                this.grid1.DataSource = gridTable;
            }
            else
            {
                ShowErr(sql);
                return;
            }
            

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            grid1.ValidateControl();
            if (gridTable ==null||gridTable.Rows.Count == 0) return;

            DataRow[] dr2 = gridTable.Select("Sel= 1");
            if (dr2.Length > 0)
            {
                foreach (DataRow dr in dr2)
                {
                    DataRow[] findrow = this.detTable.Select(string.Format("MiscPOID = '{0}' and SEQ1 = '{1}' and SEQ2 = '{2}'", dr["Miscpoid"].ToString(), dr["SEQ1"].ToString(), dr["SEQ2"].ToString()));
                    if (findrow.Length == 0)
                    {
                        DataRow nDr = this.detTable.NewRow();
                        nDr["Miscpoid"] = dr["Miscpoid"];
                        nDr["SEQ1"] = dr["SEQ1"];
                        nDr["SEQ2"] = dr["SEQ2"];
                        nDr["MiscID"] = dr["MiscID"];
                        nDr["Description"] = dr["Description"];
                        nDr["Unitid"] = dr["Unitid"];
                        nDr["price"] = dr["price"];
                        nDr["inQty"] = dr["inQty"];
                        nDr["inspQty"] = dr["inspQty"];
                        nDr["Inspect"] = dr["Inspect"];
                        nDr["departmentid"] = dr["departmentid"];
                        nDr["PurchaseType"] = dr["PurchaseType"];
                        nDr["MiscReqid"] = dr["MiscReqid"];
                        nDr["Projectid"] = dr["Projectid"];
                        nDr["apQty"] = dr["apQty"];
                        nDr["Qty"] = dr["Qty"];
                        nDr["Balance"] = dr["Balance"];
                        nDr["Amount"] = (decimal)dr["Price"] * (decimal)dr["Qty"];
                        this.detTable.Rows.Add(nDr);
                    }
                    else
                    {
                        findrow[0]["Qty"] = dr["Qty"];
                    }
                }
            }
            else
            {
                MyUtility.Msg.WarningBox("Please select rows first!", "Warnning");
                return;
            }
            this.Close();
        }
    }
}
