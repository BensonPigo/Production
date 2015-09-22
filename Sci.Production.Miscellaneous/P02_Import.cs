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
    public partial class P02_Import : Sci.Win.Subs.Base
    {
        private string factory = Sci.Env.User.Factory,localSuppid;
        private DataTable gridTable;
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        private DataTable detTable;
        public P02_Import(DataTable detTable,string localSuppid)
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
                .Text("miscid", header: "Miscellaneous", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("Description", header: "Description", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("MiscBrand", header: "Brand", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("currencyid", header: "Currency", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Numeric("Price", header: "Unit Price", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 4, iseditingreadonly: true)
                .Numeric("reqQty", header: "Req. Qty", width: Widths.AnsiChars(5), integer_places: 6, decimal_places: 2, iseditingreadonly: true)
                .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(5), integer_places: 6, decimal_places: 2, settings: cellamount, iseditingreadonly: true)
                .Text("UnitID", header: "PO Unit", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Numeric("Amount", header: "Amount", width: Widths.AnsiChars(10), integer_places: 13, decimal_places: 3, iseditingreadonly: true)
                .Text("MiscReqid", header: "Misc Requisition#", width: Widths.AnsiChars(13), iseditingreadonly: true);
            this.grid1.Columns[0].DefaultCellStyle.BackColor = Color.Pink;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string reqid1 = textBox1.Text;
            string reqid2 = textBox2.Text;
            string miscid1 = txtmisc1.Text;
            string miscid2 = txtmisc2.Text;
            string sql;


            if (MyUtility.Check.Empty(reqid1) && MyUtility.Check.Empty(reqid2) && MyUtility.Check.Empty(miscid1) && MyUtility.Check.Empty(miscid2))
            {
                MyUtility.Msg.WarningBox("At least on conditions <Requisition#> <Miscellaneous> must be entried.","Warning");
                return;
            }
            sql = @"Select 0 as sel,a.*, b.Description,a.MiscBrandid+'-'+c.Name as MiscBrand,e.Departmentid,b.Inspect,a.projectid
                ,a.Qty as ReqQty,a.Qty * a.Price as amount,d.Currencyid,b.Unitid,a.id as miscreqid,e.PurchaseType
                from  MiscReq e,MiscReq_Detail a 
                left join Misc b on a.miscid = b.id 
                left join MiscBrand c on c.id = a.MiscBrandid
                left join LocalSupp d on d.id = a.suppid ";
            sql = sql + string.Format(" where e.id = a.id and e.status = 'Approved' and e.Factoryid= '{0}' and a.Suppid = '{1}' and a.miscpoid = ''", factory, localSuppid);
            if (!MyUtility.Check.Empty(reqid1) && !MyUtility.Check.Empty(reqid2))
            {
                sql = sql + string.Format(" and a.id >= '{0}' and a.id <= '{1}'  ", reqid1, reqid2);
            }

            if (!MyUtility.Check.Empty(miscid1) && !MyUtility.Check.Empty(miscid2))
            {
                sql = sql + string.Format(" and a.miscid >= '{0}' and a.miscid <= '{1}' ", miscid1, miscid2);
            }

            DualResult rst = DBProxy.Current.Select("Production" ,sql, out gridTable);
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
                    DataRow[] findrow = this.detTable.Select(string.Format("MiscID = '{0}' and MiscReqID = '{1}' ", tmp["Miscid"].ToString(), tmp["MiscReqID"].ToString()));
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
                        nDr["projectid"] = tmp["projectid"];
                        nDr["MiscBrand"] = tmp["MiscBrand"];
                        nDr["MiscBrandid"] = tmp["MiscBrandid"];
                        nDr["Departmentid"] = tmp["Departmentid"];
                        nDr["PurchaseType"] = tmp["PurchaseType"];
                        nDr["Inspect"] = tmp["Inspect"];
                        nDr["Price"] = tmp["Price"];
                        nDr["Qty"] = tmp["Qty"];
                        nDr["UnitID"] = tmp["UnitID"];
                        nDr["Amount"] = tmp["Amount"];
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
