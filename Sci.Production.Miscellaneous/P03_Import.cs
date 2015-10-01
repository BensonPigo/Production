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

namespace Sci.Production.Miscellaneous
{
    public partial class P03_Import : Sci.Win.Subs.Base
    {
        private string factory = Sci.Env.User.Factory,localSuppid;
        private DataTable gridTable;
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        private DataTable detTable;
        public P03_Import(DataTable detTable,string localSuppid)
        {
            InitializeComponent();
            // TODO: Complete member initialization
            this.detTable = detTable;
            this.localSuppid = localSuppid;
        }
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
             //設定Grid1的顯示欄位



            DataGridViewGeneratorNumericColumnSettings cellamount = new DataGridViewGeneratorNumericColumnSettings();
            cellamount.CellValidating += (s, e) =>
            {
                if (e.FormattedValue == null) return;
                int ind = e.RowIndex;
                gridTable.Rows[ind]["Amount"] = (decimal)e.FormattedValue * (decimal)gridTable.Rows[ind]["Price"];
                gridTable.Rows[ind]["Qty"] = (decimal)e.FormattedValue;
                gridTable.Rows[ind].EndEdit();
            };

            this.grid1.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.grid1.DataSource = gridTable;

            Helper.Controls.Grid.Generator(this.grid1)
                .CheckBox("Sel", header: "Sel",trueValue:1,falseValue:0)
                .Text("Miscid", header: "Miscellaneous", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("Description", header: "Description", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("MiscBrandid", header: "Miscellaneous Brand", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Currencyid", header: "Currency", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Numeric("Price", header: "Unit Price", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 4, iseditingreadonly: true)
                .Numeric("reqQty", header: "Req. Qty", width: Widths.AnsiChars(5), integer_places: 6, decimal_places: 2, iseditingreadonly: true)
                .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(5), integer_places: 6, decimal_places: 2, settings: cellamount)
                .Text("UnitID", header: "Unit", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Numeric("Amount", header: "Amount", width: Widths.AnsiChars(10), integer_places: 13, decimal_places: 3, iseditingreadonly: true)
                .Text("MiscReqid", header: "Miscellaneous Requisition#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                 .Text("projectid", header: "Project", width: Widths.AnsiChars(10), iseditingreadonly: true);
            this.grid1.Columns[0].DefaultCellStyle.BackColor = Color.Pink;
            this.grid1.Columns[6].DefaultCellStyle.BackColor = Color.Pink;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string reqid1 = textBox1.Text;
            string reqid2 = textBox2.Text;
            string date1 = dateRange1.TextBox1.Text;
            string date2 = dateRange1.TextBox2.Text;
            string miscid1 = txtmisc1.Text;
            string miscid2 = txtmisc2.Text;
            string sql;


            if (MyUtility.Check.Empty(reqid1) && MyUtility.Check.Empty(reqid2) && MyUtility.Check.Empty(dateRange1.TextBox1.Value) && MyUtility.Check.Empty(dateRange1.TextBox2.Value) && MyUtility.Check.Empty(miscid1) && MyUtility.Check.Empty(miscid2))
            {
                MyUtility.Msg.WarningBox("At least on conditions <Requisition#> <Req. Date> <Miscellaneous> must be entried.","Warning");
                return;
            }
            sql = @"Select 0 as sel, a.*,b.description,b.unitid,b.inspect,
                    a.miscbrandid+'-'+d.name as MiscBrand,a.qty as reqqty,
                    a.qty*a.price as amount ,a.id as MiscReqid,f.Currencyid,e.Departmentid
                    from MiscReq e ,Miscreq_Detail a 
                    left join Misc b on b.id = a.Miscid 
                    left join MiscBrand d on d.id = a.MiscBrandid 
                    left join Supp f on f.id = a.suppid
                    Where e.id=a.id and e.status = 'Approved' and e.PurchaseFrom = 'T' and a.miscpoid = ''";
            sql = sql + string.Format("and e.Factoryid= '{0}' ", factory);
            if (!MyUtility.Check.Empty(reqid1) && !MyUtility.Check.Empty(reqid2))
            {
                sql = sql + string.Format(" and e.id >= '{0}' and e.id <= '{1}'", reqid1, reqid2);
            }
            if (!MyUtility.Check.Empty(dateRange1.TextBox1.Value) && !MyUtility.Check.Empty(dateRange1.TextBox2.Value))
            {
                sql = sql + string.Format(" and e.cdate >= '{0}' and e.cdate <= '{1}'", date1, date2);
            }
            if (!MyUtility.Check.Empty(miscid1) && !MyUtility.Check.Empty(miscid2))
            {
                sql = sql + string.Format(" and a.miscid >= '{0}' and a.miscid <= '{1}'", miscid1, miscid2);
            }

            DualResult rst = DBProxy.Current.Select(null, sql, out gridTable);
            if (rst)
            {
                if (gridTable.Rows.Count == 0)
                { MyUtility.Msg.WarningBox("Data not found!!"); }
                
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
                    tmp["ID"] = "";
                    DataRow[] findrow = this.detTable.Select(string.Format("MiscID = '{0}' and MiscReqid = '{1}' ", tmp["MiscID"].ToString(), tmp["MiscReqid"].ToString()));
                    if (findrow.Length > 0)
                    {
                        findrow[0]["Qty"] = tmp["Qty"];
                    }
                    else
                    {
                        DataRow nDr = this.detTable.NewRow();
                        nDr["MiscReqid"] = tmp["MiscReqid"];
                        nDr["MiscID"] = tmp["MiscID"];
                        nDr["description"] = tmp["description"];
                        nDr["MiscBrandid"] = tmp["MiscBrandid"];
                        nDr["Price"] = tmp["Price"];
                        nDr["ReqQty"] = tmp["ReqQty"];
                        nDr["Qty"] = tmp["Qty"];
                        nDr["UnitID"] = tmp["UnitID"];
                        nDr["Currencyid"] = tmp["Currencyid"];
                        nDr["Amount"] = tmp["Amount"];
                        nDr["Inspect"] = tmp["Inspect"];
                        nDr["Departmentid"] = tmp["Departmentid"];
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
