using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Sci;
using Sci.Win;
using Sci.Data;
using Ict;
using Ict.Win;
using System.Linq;
using Sci.Production.Class;

namespace Sci.Production.Miscellaneous
{
    public partial class P05_Import : Sci.Win.Subs.Base
    {
        private DataTable gridTable;
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        private string factory = Sci.Env.User.Factory;
        private DataTable detTable;
        public P05_Import(DataTable detTable,string incomingid)
        {
            InitializeComponent();
            // TODO: Complete member initialization
            this.detTable = detTable;
            String sql = String.Format(
            @"select 1 as sel,a.*, b.description ,a.qty as inqty,a.qty as passqty,'' as remark
            from MiscIn_detail a,Misc b,miscpo_Detail c,MiscIn d 
            where a.id= '{0}' and  a.miscid = b.id and b.Inspect=1 and c.miscid = a.miscid 
            and a.MiscPOID = c.id and a.seq1 = c.seq1 and a.seq2 = c.seq2 and c.inqty-c.inspqty>0 
            and d.id = a.id and d.factoryid = '{1}'", incomingid, factory );
            DualResult dResult = DBProxy.Current.Select(null, sql, out gridTable);
            if (dResult)
            {
                if (gridTable.Rows.Count == 0)
                {
                    MyUtility.Msg.InfoBox("No Data");
                }
            }
        }
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
             //設定Grid1的顯示欄位
            this.grid1.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.grid1.DataSource = gridTable;
            Helper.Controls.Grid.Generator(this.grid1)
            .CheckBox("Sel", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)
            .Text("MiscPOID", header: "PO No.", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("SEQ1", header: "SEQ1", width: Widths.AnsiChars(3), iseditingreadonly: true)
            .Text("SEQ2", header: "SEQ2", width: Widths.AnsiChars(2), iseditingreadonly: true)
            .Text("MiscID", header: "Miscellaneous", width: Widths.AnsiChars(23), iseditingreadonly: true)
            .Text("Description", header: "Description", width: Widths.AnsiChars(25), iseditingreadonly: true)
            .Numeric("InQty", header: "In Qty", width: Widths.AnsiChars(8), integer_places: 6, decimal_places: 2, iseditingreadonly: true)
            .Numeric("PassQty", header: "Pass Qty", width: Widths.AnsiChars(8), integer_places: 6, decimal_places: 2)
            .Text("Remark", header: "Remark", width: Widths.AnsiChars(5))
            .Date("Inspdeadline", header: "Inspect Lead Time", width: Widths.AnsiChars(10), iseditingreadonly: true);
            this.grid1.Columns[0].DefaultCellStyle.BackColor = Color.Pink;
            this.grid1.Columns[7].DefaultCellStyle.BackColor = Color.Pink;
            this.grid1.Columns[8].DefaultCellStyle.BackColor = Color.Pink;   
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
  
            grid1.ValidateControl();



            if (gridTable.Rows.Count == 0) return;

            DataRow[] dr2 = gridTable.Select("Qty = 0 and Sel= 1");
            if (dr2.Length > 0)
            {
                MyUtility.Msg.WarningBox("<Qty> of selected row can't be zero!", "Warning");
                return;
            }
            dr2 = gridTable.Select("sel = 1");
            if (dr2.Length > 0)
            {
                foreach (DataRow tmp in dr2)
                {
                    DataRow[] findrow = this.detTable.Select(string.Format("MiscPOID = '{0}' and SEQ1 = '{1}' and SEQ2 = '{2}'", tmp["MiscPOID"].ToString(), tmp["SEQ1"].ToString(), tmp["SEQ2"].ToString()));
  
                    if (findrow.Length > 0)
                    {
                        findrow[0]["PassQty"] = tmp["PassQty"];
                        findrow[0]["Remark"] = tmp["Remark"];
                    }
                    else 
                    {
                        DataRow nDr = this.detTable.NewRow();
                        nDr["MiscPOID"] = tmp["MiscPOID"];
                        nDr["Miscid"] = tmp["Miscid"];
                        nDr["SEQ1"] = tmp["SEQ1"];
                        nDr["SEQ2"] = tmp["SEQ2"];
                        nDr["description"] = tmp["description"];
                        nDr["InQty"] = tmp["InQty"];
                        nDr["Inspdeadline"] = tmp["Inspdeadline"];
                        nDr["PassQty"] = tmp["PassQty"];
                        nDr["Remark"] = tmp["Remark"];
                        this.detTable.Rows.Add(nDr);
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
