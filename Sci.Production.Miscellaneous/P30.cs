using Ict.Win;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Sci;
using Sci.Data;
using Sci.Win;
using Ict;
using Ict.Data;


namespace Sci.Production.Miscellaneous
{
    public partial class P30 : Sci.Win.Tems.QueryForm
    {
        DataTable grid = new DataTable();
        public P30(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            
            InitializeComponent();
           // DataTable grid = new DataTable();
            grid.Columns.Add("FactoryID", typeof(string));
            grid.Columns.Add("POID", typeof(string));
            grid.Columns.Add("SEQ", typeof(string));
            grid.Columns.Add("miscid", typeof(string));
            grid.Columns.Add("Carton", typeof(string));
            grid.Columns.Add("TPEPOID", typeof(string));
            grid.Columns.Add("Description", typeof(string));
            grid.Columns.Add("ExportID", typeof(string));
            grid.Columns.Add("Invno", typeof(string));
            grid.Columns.Add("ETA", typeof(DateTime));
            grid.Columns.Add("WhseArrival", typeof(DateTime));
            grid.Columns.Add("Qty", typeof(decimal));
            grid.Columns.Add("FOC", typeof(decimal));
            grid.Columns.Add("Unitid", typeof(string));
            grid.Columns.Add("Balance", typeof(decimal));
            grid.Columns.Add("ReqID", typeof(string));
            grid.Columns.Add("CurrencyId", typeof(string));
            grid.Columns.Add("Price", typeof(decimal));
            grid.Columns.Add("Amount", typeof(decimal));
        }
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.grid1.IsEditingReadOnly = true;
            this.grid1.DataSource = grid;

            Helper.Controls.Grid.Generator(this.grid1)
            .Text("Factoryid", header: "Factory", width: Widths.AnsiChars(8))
            .Text("POID", header: "PO#", width: Widths.AnsiChars(13))
            .Text("SEQ", header: "SEQ", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Text("MiscID", header: "Miscellaneous", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .Text("Description", header: "Description", width: Widths.AnsiChars(25), iseditingreadonly: true)
            .Text("ExportID", header: "WK#", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("Invno", header: "Invoice#", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Date("ETA", header: "ETA", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("Carton", header: "Carton#", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Date("WhseArrival", header: "Arrive W/H \n Date", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("TPEPOID", header: "Taipei PO#", width: Widths.AnsiChars(13))
            .Text("Unitid", header: "Unit", width: Widths.AnsiChars(5))
            .Numeric("Qty", header: "Export Qty", width: Widths.AnsiChars(5), integer_places: 9, iseditingreadonly: true)
            .Numeric("FOC", header: "F.O.C.", width: Widths.AnsiChars(5), integer_places: 9)
            .Numeric("Balance", header: "Balance", width: Widths.AnsiChars(5), integer_places: 9)
            .Text("ReqID", header: "Fty Req#", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("CurrencyId", header: "Currency", width: Widths.AnsiChars(3), iseditingreadonly: true)
            .Numeric("Price", header: "Unit Price", width: Widths.AnsiChars(8), integer_places: 9,decimal_places:3)
            .Numeric("Amount", header: "Amount", width: Widths.AnsiChars(8), integer_places: 10, decimal_places: 3);

        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            grid.Clear(); //清空Grid


            string poId1, poId2, factoryId, eta1, eta2, strWhere, strWhereMG, strSql, miscid1, miscid2;
            DataTable queryExport;
            poId1 = this.textBox1.Text;
            poId2 = this.textBox2.Text;
            miscid1 = txtmisc1.Text;
            miscid2 = txtmisc2.Text;
            factoryId = this.txtfactory1.Text;

            eta1 = this.dateRange2.TextBox1.Text.ToString();
            eta2 = this.dateRange2.TextBox2.Text.ToString();
            if (MyUtility.Check.Empty(poId1) && MyUtility.Check.Empty(poId2) && MyUtility.Check.Empty(miscid1) && MyUtility.Check.Empty(miscid2) && MyUtility.Check.Empty(factoryId) && MyUtility.Check.Empty(this.dateRange2.TextBox1.Value) && MyUtility.Check.Empty(this.dateRange2.TextBox2.Value))
            {
                MyUtility.Msg.WarningBox("Must be entyied one condition");
                this.textBox1.Focus();
                return;
            }
            strWhere = "Where a.id = b.id and POtype='M' and fabricType = 'O' ";
            if (!MyUtility.Check.Empty(this.dateRange2.TextBox1.Value)) strWhere = strWhere + string.Format(" and eta>='{0}'", eta1);
            if (!MyUtility.Check.Empty(this.dateRange2.TextBox2.Value)) strWhere = strWhere + string.Format(" and eta<='{0}'", eta2);
            strSql = "with tmp1 as (Select a.invno,a.eta,a.Whsearrival,b.* from [production].[dbo].Export a, [production].[dbo].Export_Detail b " + strWhere + " ), ";

            strWhereMG = " ";
            if (!MyUtility.Check.Empty(poId1)) strWhereMG = strWhereMG + string.Format(" and c.id>='{0}'", poId1);
            if (!MyUtility.Check.Empty(poId2)) strWhereMG = strWhereMG + string.Format(" and c.id<='{0}'", poId2);
            if (!MyUtility.Check.Empty(miscid1)) strWhereMG = strWhereMG + string.Format(" and miscid>='{0}'", miscid1);
            if (!MyUtility.Check.Empty(miscid2)) strWhereMG = strWhereMG + string.Format(" and miscid<='{0}'", miscid2);
            if (!MyUtility.Check.Empty(factoryId)) strWhereMG = strWhereMG + string.Format(" and factoryId='{0}'", factoryId);
            strSql = strSql + " tmp2 as (Select c.id as miscpoid,c.PurchaseFrom,c.Currencyid,c.factoryid,a.id,a.miscid,a.TPEPOID,a.SEQ1,a.seq2,a.price,a.unitid,a.MiscReqid,a.suppid,b.description from  Miscpo c,MiscPO_Detail a Left join Misc b on a.Miscid = b.id where c.id = a.id " + strWhereMG + " )";
            strSql = strSql + " Select * from tmp1 a,tmp2 b where a.poid = b.TPEPOID and a.seq1 = b.seq1 and a.seq2 = b.seq2";
            try
            {
                DBProxy.Current.DefaultTimeout = 60;
                DualResult sqlresult = DBProxy.Current.Select(null, strSql, out queryExport);
                DBProxy.Current.DefaultTimeout = 0;
                if (sqlresult)
                {

                    foreach (DataRow dr in queryExport.Rows)
                    {
                        DataRow drgrid = grid.NewRow();
                        
                        drgrid["TPEPOID"] = dr["POID"];
                        drgrid["SEQ"] = dr["SEQ1"].ToString() + dr["SEQ2"].ToString();
                        drgrid["invno"] = dr["Invno"];
                        drgrid["ExportID"] = dr["ID"];
                        drgrid["ETA"] = dr["ETA"];
                        drgrid["Qty"] = dr["Qty"];
                        drgrid["Whsearrival"] = dr["Whsearrival"];
                        drgrid["Carton"] = dr["Carton"];
                        drgrid["FOC"] = dr["FOC"];
                        drgrid["Qty"] = dr["Qty"];
                        drgrid["Balance"] = (decimal)dr["BalanceQty"] + (decimal)dr["BalanceFOC"];
                        drgrid["POID"] = dr["miscpoid"];
                        drgrid["Miscid"] = dr["Miscid"];
                        drgrid["Factoryid"] = dr["Factoryid"];
                        drgrid["Description"] = dr["Description"];
                        drgrid["Price"] = dr["price"];
                        drgrid["Unitid"] = dr["Unitid"];
                        drgrid["Amount"] = (decimal)dr["Price"] * (decimal)dr["Qty"];
                        drgrid["ReqID"] = dr["MiscReqid"];
                        if (dr["PurchaseFrom"].ToString() == "L")
                        {
                            drgrid["Currencyid"] = dr["Currencyid"];
                        }
                        else
                        {
                            drgrid["Currencyid"] = MyUtility.GetValue.Lookup("Currencyid", dr["Suppid"].ToString(), "Supp", "ID", "Production");
                        }
                        grid.Rows.Add(drgrid);
                    }
                }
                else
                {
                    MyUtility.Msg.ErrorBox("Production Connect fail");
                    return;
                }
            }
            catch (Exception ex)
            {
                ShowErr("Commit transaction error.", ex);
                return;
            }
            if (grid.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (this.grid1.RowCount == 0)
            {
                MyUtility.Msg.WarningBox("There are no data");
                return;
            }
            if (!MyUtility.File.ExportGridToExcel("Import Schedule List(Miscellaneous)", this.grid1))
            {
                MyUtility.Msg.ErrorBox("Excel create fail");
            }
        }
    }
}
