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
    public partial class P04_Import : Sci.Win.Subs.Base
    {
        private DataTable gridTable;
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        private string factory = Sci.Env.User.Factory;
        private DataTable detTable;
        object cDate;
        public P04_Import(DataTable detTable,object dt)
        {
            InitializeComponent();
            // TODO: Complete member initialization
            this.detTable = detTable;
            cDate = dt;
        }
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
             //設定Grid1的顯示欄位
            this.grid1.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.grid1.DataSource = gridTable;
            Helper.Controls.Grid.Generator(this.grid1)
            .CheckBox("Sel", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)
            .Text("MiscPOID", header: "PO#", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("TPEPOID", header: "Taipei PO#", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("SEQ1", header: "SEQ1", width: Widths.AnsiChars(3), iseditingreadonly: true)
            .Text("SEQ2", header: "SEQ2", width: Widths.AnsiChars(2), iseditingreadonly: true)
            .Text("Miscid", header: "Miscellaneous", width: Widths.AnsiChars(23), iseditingreadonly: true)
            .Text("Description", header: "Description", width: Widths.AnsiChars(25), iseditingreadonly: true)
             .Numeric("Price", header: "Price", width: Widths.AnsiChars(8), integer_places: 6, decimal_places: 2, iseditingreadonly: true)
            .Numeric("POQty", header: "PO Qty", width: Widths.AnsiChars(5), integer_places: 6, decimal_places: 2, iseditingreadonly: true)
            .Numeric("OnRoad", header: "On Road", width: Widths.AnsiChars(5), integer_places: 6, decimal_places: 2, iseditingreadonly: true)
            .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(5), integer_places: 6, decimal_places: 2)
            .Text("Unitid", header: "Unit", width: Widths.AnsiChars(5), iseditingreadonly: true)
            .Date("Inspdeadline", header: "Inspect Lead Time", width: Widths.AnsiChars(10))
            .Text("Unitid", header: "Unit", width: Widths.AnsiChars(5), iseditingreadonly: true)
            .Text("Departmentid", header: "Department", width: Widths.AnsiChars(8), iseditingreadonly: true);
            this.grid1.Columns[0].DefaultCellStyle.BackColor = Color.Pink;
            this.grid1.Columns[7].DefaultCellStyle.BackColor = Color.Pink;
            this.grid1.Columns[8].DefaultCellStyle.BackColor = Color.Pink;   
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string poid1 = textBox1.Text;
            string poid2 = textBox2.Text;
            string miscid1 = txtmisc1.Text;
            string miscid2 = txtmisc2.Text;
            if (MyUtility.Check.Empty(poid1) && MyUtility.Check.Empty(poid1) && MyUtility.Check.Empty(miscid1) && MyUtility.Check.Empty(miscid2))
            {
                MyUtility.Msg.WarningBox("At least on conditions <PO#> <Miscellaneous> must be entried.","Warning");
                return;
            }
            string sql = string.Format(
            @"Select 1 as sel, a.id as miscpoid,a.seq1,a.seq2,a.miscid,a.qty as poqty, b.description,b.InspLeadTime,
            a.unitid,a.Qty-a.inQty as Qty,a.Qty-a.inQty as OnRoad,a.tpepoid,a.price,a.departmentid,
            cast(null as Datetime) as Inspdeadline,b.Inspect,a.miscreqid
            FROM Miscpo c ,MiscPO_detail a 
            Left Join Misc b on a.Miscid = b.id 
            WHERE c.ID = a.ID and c.Status = 'Approved' and a.Qty-a.InQty > 0 and c.factoryid='{0}' ", factory);
            if (!MyUtility.Check.Empty(poid1))
            {
                sql = sql + string.Format(" and a.id >= '{0}'", poid1);
            }
            if (!MyUtility.Check.Empty(poid2))
            {
                sql = sql + string.Format(" and a.id <= '{0}'", poid2);
            }
            if (!MyUtility.Check.Empty(miscid1))
            {
                sql = sql + string.Format(" and a.miscid >= '{0}'", miscid1);
            }
            if (!MyUtility.Check.Empty(miscid2))
            {
                sql = sql + string.Format(" and a.miscid <= '{0}'", miscid2);
            }
            sql = sql + " order by miscpoid,seq1,seq2";
            DualResult rst = DBProxy.Current.Select(null, sql, out gridTable);
            if (rst)
            {
                if (gridTable.Rows.Count == 0)
                { MyUtility.Msg.WarningBox("Data not found!!"); }

                foreach (DataRow dr in gridTable.Rows)
                {
                    if (dr["Inspect"].ToString() == "True")
                    {
                        if (MyUtility.Check.Empty(dr["InspLeadTime"]))
                        {
                            double nDl = Convert.ToDouble(MyUtility.GetValue.Lookup("select MiscInspdate from System"));
                            dr["InspDeadline"] = ((DateTime)cDate).AddDays(nDl);
                        }
                        else //若Leadtime 非空就加上為空就用System
                        {
                            dr["InspDeadline"] = ((DateTime)cDate).AddDays((double)((decimal)dr["InspLeadTime"]));
                        }

                    }
                }
                this.grid1.DataSource = gridTable;
            }
            else
            {
                ShowErr(sql, rst);
            }

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
                        findrow[0]["Qty"] = tmp["Qty"];
                        findrow[0]["Inspdeadline"] = tmp["Inspdeadline"];
                        findrow[0]["OnRoad"] = tmp["OnRoad"];
                    }
                    else 
                    {
                        DataRow nDr = this.detTable.NewRow();
                        nDr["MiscPOID"] = tmp["MiscPOID"];
                        nDr["Miscid"] = tmp["Miscid"];
                        nDr["SEQ1"] = tmp["SEQ1"];
                        nDr["SEQ2"] = tmp["SEQ2"];
                        nDr["description"] = tmp["description"];
                        nDr["POQty"] = tmp["POQty"];
                        nDr["Qty"] = tmp["Qty"];
                        nDr["Inspdeadline"] = tmp["Inspdeadline"];
                        nDr["OnRoad"] = tmp["OnRoad"];
                        nDr["Unitid"] = tmp["Unitid"];
                        nDr["TPEPOID"] = tmp["TPEPOID"];
                        nDr["Price"] = tmp["Price"];
                        nDr["Departmentid"] = tmp["Departmentid"];
                        nDr["MiscReqid"] = tmp["MiscReqid"];
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
