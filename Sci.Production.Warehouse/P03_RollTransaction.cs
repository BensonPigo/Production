using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Sci;
using Sci.Data;
using Ict;
using System.Linq;
using System.Data.SqlClient;
using Sci.Win;
using Sci.Utility.Excel;
using System.Runtime.InteropServices;



namespace Sci.Production.Warehouse
{
    public partial class P03_RollTransaction : Sci.Win.Subs.Base
    {
        DataRow dr;
        DataTable dtFtyinventory, dtTrans, dtSummary;
        DataSet data = new DataSet();
        decimal useQty = 0;
        Boolean bUseQty = false; 
        public P03_RollTransaction(DataRow data)
        {
            InitializeComponent();
            
            dr = data;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();            
            this.Text += string.Format(@" ({0}-{1}-{2})", dr["id"], dr["seq1"], dr["seq2"]);  //351: WAREHOUSE_P03_RollTransaction_Transaction Detail by Roll#，3.Tool bar要帶出SP# & Seq
            this.displaySeqNo.Text = dr["seq1"].ToString() + "-" + dr["seq2"].ToString();
            this.displayDescription.Text = MyUtility.GetValue.Lookup(string.Format("select dbo.getmtldesc('{0}','{1}','{2}',2,0)", dr["id"].ToString(), dr["seq1"].ToString(), dr["seq2"].ToString()));
            this.numArrivedQtyBySeq.Value = MyUtility.Check.Empty( dr["inqty"]) ? decimal.Parse("0.00"): decimal.Parse(dr["inqty"].ToString());
            this.numReleasedQtyBySeq.Value = MyUtility.Check.Empty(dr["outqty"]) ? decimal.Parse("0.00") : decimal.Parse(dr["outqty"].ToString());

            //this.numericBox3.Value = (MyUtility.Check.Empty(dr["inqty"]) ? decimal.Parse("0.00") : decimal.Parse(dr["inqty"].ToString())) -
            //  ( MyUtility.Check.Empty(dr["outqty"]) ? decimal.Parse("0.00") : decimal.Parse(dr["outqty"].ToString())) +(MyUtility.Check.Empty(dr["outqty"]) ? decimal.Parse("0.00") : decimal.Parse(dr["adjustqty"].ToString()));
            decimal IN = (MyUtility.Check.Empty(dr["inqty"]) ? decimal.Parse("0.00") : decimal.Parse(dr["inqty"].ToString()));
            decimal OUT = (MyUtility.Check.Empty(dr["outqty"]) ? decimal.Parse("0.00") : decimal.Parse(dr["outqty"].ToString()));
            decimal ADJ = (MyUtility.Check.Empty(dr["adjustqty"]) ? decimal.Parse("0.00") : decimal.Parse(dr["adjustqty"].ToString()));
            this.numBalQtyBySeq.Value = IN - OUT + ADJ;

            #region "顯示DTM"
            DataTable dtmDt;
            string sql = string.Format(@"
                select id,seq1,seq2,qty,ShipQty,Refno,ColorID,iif(qty <= shipqty, 'True','False') bUseQty
                into #tmp
                from Po_Supp_Detail
                where id = '{0}'
                and seq1 = '{1}'
                and seq2 = '{2}'
 
                select isnull(Round(dbo.getUnitQty(a.POUnit, a.StockUnit, (isnull(A.NETQty,0)+isnull(A.lossQty,0))), 2),0.0) as UseQty,b.bUseQty
                from Po_Supp_Detail a
                inner join #tmp b on a.id = b.id and a.Refno = b.Refno and a.ColorID = b.ColorID and b.bUseQty = 'True' 
                where a.id = '{0}'
                and a.seq1 = 'A1' 

                drop table #tmp
            ", dr["id"].ToString(), dr["seq1"].ToString(), dr["seq2"].ToString());
            DualResult dtmResult = DBProxy.Current.Select(null, sql, out dtmDt);
            if (dtmResult == false) ShowErr(sql, dtmResult);
            if (!MyUtility.Check.Empty(dtmDt)) {
                if (dtmDt.Rows.Count > 0) {
                    bUseQty = MyUtility.Convert.GetBool(dtmDt.Rows[0]["bUseQty"]);
                    useQty = bUseQty ? MyUtility.Convert.GetDecimal(dtmDt.Rows[0]["useQty"]) : useQty;
                }
            }
            #endregion

            #region Grid1 - Sql command
            string selectCommand1
                = string.Format(@"Select a.Roll,a.Dyelot
                                ,[stocktype] = case when stocktype = 'B' then 'Bulk'
                                                    when stocktype = 'I' then 'Invertory'
			                                        when stocktype = 'O' then 'Scrap' End
                                                ,a.InQty,a.OutQty,a.AdjustQty
                                                ,a.InQty - a.OutQty + a.AdjustQty as balance
                                                ,dbo.Getlocation(a.ukey)  MtlLocationID 
                                            from FtyInventory a WITH (NOLOCK) 
                                            where a.Poid = '{0}'
                                                and a.Seq1 = '{1}'
                                                and a.Seq2 = '{2}' 
                                                --and MDivisionPoDetailUkey is not null  --避免下面Relations發生問題
                                                --and MDivisionID='{3}'  --新增MDivisionID條件，避免下面DataRelation出錯
                                                and StockType <> 'O'  --C倉不用算
                                            order by a.dyelot,a.roll,a.stocktype"
                , dr["id"].ToString()
                , dr["seq1"].ToString()
                , dr["seq2"].ToString()
                , Sci.Env.User.Keyword);
            #endregion

            #region Grid2 
            // 因為需求ISP20191233 要抓到BalanceQty, 故將sql command 移到共用function: "RollTranscation"
             dtTrans = Sci.Production.PublicPrg.Prgs.RollTranscation(dr["id"].ToString(), dr["seq1"].ToString(), dr["seq2"].ToString());

            if (dtTrans == null)
            {
                return;
            }
            #endregion
            DualResult selectResult1 = DBProxy.Current.Select(null, selectCommand1, out dtFtyinventory);
            if (selectResult1 == false) ShowErr(selectCommand1, selectResult1);
            dtFtyinventory.TableName = "dtFtyinventory";
            dtSummary = dtFtyinventory.Clone();
            dtSummary.Columns.Add("rollcount", typeof(int));
            dtSummary.Columns.Add("DTM", typeof(decimal));
            bindingSource3.DataSource = dtSummary;

            dtTrans.TableName = "dtTrans";
            data.Tables.Add(dtFtyinventory);
            data.Tables.Add(dtTrans);
            data.Tables.Add("dtSummary");

            //remove [Dyelot] DataRelation
            //DataRelation relation = new DataRelation("rel1"
            //    , new DataColumn[] { dtFtyinventory.Columns["Roll"], dtFtyinventory.Columns["Dyelot"], dtFtyinventory.Columns["StockType"] }
            //    , new DataColumn[] { dtTrans.Columns["roll"], dtTrans.Columns["dyelot"], dtTrans.Columns["stocktype"] }
            //    );
            //105.12.23 Jimmy
            if (dtFtyinventory.Rows.Count == 0 || dtTrans.Rows.Count == 0)
            {
                //MyUtility.Msg.ErrorBox("Data not found!!");
                return;
            }

            try
            {
                DataRelation relation = new DataRelation("Rol1"
               , new DataColumn[] { dtFtyinventory.Columns["Roll"], dtFtyinventory.Columns["StockType"],dtFtyinventory.Columns["Dyelot"] }
               , new DataColumn[] { dtTrans.Columns["roll"], dtTrans.Columns["stocktype"],dtTrans.Columns["Dyelot"] }
               );

                data.Relations.Add(relation);
                bindingSource1.DataSource = data;
                bindingSource1.DataMember = "dtFtyinventory";
                bindingSource2.DataSource = bindingSource1;
                bindingSource2.DataMember = "Rol1";

                //設定Grid1的顯示欄位
                this.gridFtyinventory.IsEditingReadOnly = true;
                this.gridFtyinventory.DataSource = bindingSource1;
                Helper.Controls.Grid.Generator(this.gridFtyinventory)
                     .Text("Roll", header: "Roll#", width: Widths.AnsiChars(8))
                     .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(8))
                     .Text("stocktype", header: "Stock Type", width: Widths.AnsiChars(10))
                     .Numeric("InQty", header: "Arrived Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                     .Numeric("OutQty", header: "Released Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                     .Numeric("AdjustQty", header: "Adjust Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                     .Numeric("Balance", header: "Balance", width: Widths.AnsiChars(10), integer_places: 6, decimal_places: 2)
                     .Text("MtlLocationID", header: "Location", width: Widths.AnsiChars(10))
                     ;

                //設定Grid2的顯示欄位
                this.gridTrans.IsEditingReadOnly = true;
                this.gridTrans.DataSource = bindingSource2;
                Helper.Controls.Grid.Generator(this.gridTrans)
                    .Date("issuedate", header: "Date", width: Widths.AnsiChars(10))
                     .Text("id", header: "Transaction ID", width: Widths.AnsiChars(13))
                     .Text("name", header: "Name", width: Widths.AnsiChars(13))
                     .Numeric("inqty", header: "Arrived Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                     .Numeric("outQty", header: "Released Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                     .Numeric("Adjust", header: "Adjust Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                     .Numeric("Balance", header: "Balance", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2);

                //設定Grid3的顯示欄位
                this.gridSummary.IsEditingReadOnly = true;
                this.gridSummary.DataSource = bindingSource3;
                Helper.Controls.Grid.Generator(this.gridSummary)
                     .Text("dyelot", header: "Dyelot", width: Widths.AnsiChars(8))
                     .Numeric("rollcount", header: "# of Rolls", width: Widths.AnsiChars(6), integer_places: 6, decimal_places: 0)
                     .Text("roll", header: "Rolls", width: Widths.AnsiChars(13))
                     .Numeric("inqty", header: "Arrived Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                     .Numeric("outQty", header: "Released Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                     .Numeric("AdjustQty", header: "Adjust Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                     .Numeric("Balance", header: "Balance", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                     .Numeric("DTM", header: "DTM", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                     ;
                gridSummary.Columns["DTM"].Visible = bUseQty;
            }
            catch
            {
                MyUtility.Msg.ErrorBox("Data error ,Please doubleclick 'Balance' field to click 'Re-Calculate' button for recalculate inventory qty, then retry to doubleclick this 'Release Qty' field.!!");
                return;
            }
            this.comboStockType.Text = "ALL";
            change_Color();
        }

        private void change_Color()
        {   
            for (int i = 0; i < gridTrans.Rows.Count; i++)
            {
                DataRow dr = gridTrans.GetDataRow(i);
                if (gridTrans.Rows.Count <= i || i < 0)
                {
                    return;
                }

                if (dr["Name"].ToString() == "P16. Issue Fabric Lacking & Replacement")
                {
                    string sqlcmd = $@"select 1 from Issuelack where id='{dr["id"]}' and type='L'";
                    if (MyUtility.Check.Seek(sqlcmd))
                    {
                        this.gridTrans.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(190, 190, 190);
                    }
                    else
                    {
                        this.gridTrans.Rows[i].DefaultCellStyle.BackColor = Color.White;
                    }
                }
            }

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void comboStockType_SelectedIndexChanged(object sender, EventArgs e)
        {
            bindingSource1_PositionChanged(sender, e);  //687: WAREHOUSE_P03_RollTransaction_Transaction Detail by Roll#，1.Grid3值不對
            switch (comboStockType.SelectedIndex)
            {
                case 0:
                    bindingSource1.Filter = "";
                    break;
                case 1:
                    bindingSource1.Filter = "stocktype='Bulk'";
                    break;
                case 2:
                    bindingSource1.Filter = "stocktype='Invertory'";
                    break;
            }
        }

        private void gridFtyinventory_SelectionChanged(object sender, EventArgs e)
        {
            change_Color();
        }

        private void bindingSource1_PositionChanged(object sender, EventArgs e)
        {
            string[] tmpStocktype = new string[] { "", "" };
            
            switch (comboStockType.SelectedIndex)
            {
                case -1:
                    tmpStocktype[0] = "Bulk";
                    tmpStocktype[1] = "Invertory";
                    break;
                case 0:
                    tmpStocktype[0] = "Bulk";
                    tmpStocktype[1] = "Invertory";
                    break;
                case 1:
                    tmpStocktype[0] = "Bulk";
                    break;
                case 2:
                    tmpStocktype[0] = "Invertory";
                    break;
            }

            var tmp = from b in dtFtyinventory.AsEnumerable()
                      where tmpStocktype.Contains(b.Field<string>("StockType"))
                       group b by new
                       {
                           Dyelot = b.Field<string>("Dyelot")
                       } into m
                       select new
                       {
                           dyelot = m.First().Field<string>("Dyelot"),
                           rollcount = m.Count(),
                           roll = string.Join(";", m.Select(r => r.Field<string>("roll")).Distinct()),
                           inqty = m.Sum(w => w.Field<decimal>("inqty")),
                           outQty = m.Sum(w => w.Field<decimal>("outqty")),
                           AdjustQty = m.Sum(i => i.Field<decimal>("AdjustQty")),
                           balance = m.Sum(w => w.Field<decimal>("inqty")) - m.Sum(w => w.Field<decimal>("outqty")) + m.Sum(i => i.Field<decimal>("AdjustQty")),
                           DTM = numArrivedQtyBySeq.Value == 0 ? 0 : m.Sum(w => w.Field<decimal>("inqty")) / numArrivedQtyBySeq.Value * useQty
                       };

            dtSummary.Rows.Clear();
            tmp.ToList().ForEach(q2 => dtSummary.Rows.Add(q2.roll, q2.dyelot, null, q2.inqty, q2.outQty, q2.AdjustQty, q2.balance, null, q2.rollcount, q2.DTM));
            
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            DataRow row = this.dr;
            string id = row["ID"].ToString();
            string seq1 = row["seq1"].ToString();
            string seq2 = row["seq2"].ToString();
            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
            pars.Add(new SqlParameter("@seq1", seq1));
            pars.Add(new SqlParameter("@seq2", seq2));
            DualResult result;
            DataTable dtt, dt;
            string sqlcmd = string.Format(@"
select  a.id [SP]
        , a.SEQ1+'-'+a.SEQ2 [SEQ]
        , a.Refno [Ref]
        , a.ColorID [Color]
        , b.InQty [Arrived_Qty_by_Seq]
        , b.OutQty [Released_Qty_by_Seq]
        , b.InQty-b.OutQty+b.AdjustQty [Bal_Qty]
        , [Description] = dbo.getMtlDesc(a.id,a.SEQ1,a.SEQ2,2,0) 
from dbo.PO_Supp_Detail a WITH (NOLOCK) 
inner join dbo.MDivisionPoDetail b WITH (NOLOCK) on a.id = b.POID 
                                                    and a.SEQ1 = b.Seq1 
                                                    and a.SEQ2=b.Seq2
where   a.id = @ID 
        and a.seq1 = @seq1 
        and a.seq2=@seq2");
            result = DBProxy.Current.Select("", sqlcmd, pars, out dt);
            if (!result)
            {
                ShowErr(result);
                return;
            }
            DBProxy.Current.Select("", @"
select  c.Roll[Roll]
        , c.Dyelot [Dyelot]
        , [Stock_Type] = Case c.StockType 
                            when 'B' THEN 'Bulk' 
                            WHEN 'I' THEN 'Inventory' 
                            ELSE 'Scrap' 
                         END
        , c.InQty [Arrived_Qty]
        , c.OutQty [Released_Qty]
        , c.AdjustQty [Adjust_Qty]
        , c.InQty-c.OutQty+c.AdjustQty [Balance]
        , [Location]=dbo.Getlocation(c.Ukey)
from dbo.FtyInventory c WITH (NOLOCK) 
where   c.poid = @ID 
        and c.seq1 = @seq1 
        and c.seq2 = @seq2", pars, out dtt);
            if (dtt.Rows.Count==0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return;
            }

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Warehouse_P03_RollTransaction.xltx"); //預先開啟excel app
            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
            objSheets.Cells[1, 1] = MyUtility.GetValue.Lookup(string.Format(@"
select NameEn
from Factory
where id = '{0}'", Sci.Env.User.Keyword));
            objSheets.Cells[3, 2] = MyUtility.Convert.GetString(dt.Rows[0]["SP"].ToString());
            objSheets.Cells[3, 4] = MyUtility.Convert.GetString(dt.Rows[0]["SEQ"].ToString());
            objSheets.Cells[3, 6] = MyUtility.Convert.GetString(dt.Rows[0]["REF"].ToString());
            objSheets.Cells[3, 8] = MyUtility.Convert.GetString(dt.Rows[0]["Color"].ToString());
            objSheets.Cells[4, 2] = MyUtility.Convert.GetString(dt.Rows[0]["Arrived_Qty_by_Seq"].ToString());
            objSheets.Cells[4, 4] = MyUtility.Convert.GetString(dt.Rows[0]["Released_Qty_by_Seq"].ToString());
            objSheets.Cells[4, 6] = MyUtility.Convert.GetString(dt.Rows[0]["Bal_Qty"].ToString());
            objSheets.Cells[5, 2] = MyUtility.Convert.GetString(dt.Rows[0]["Description"].ToString());

            MyUtility.Excel.CopyToXls(dtt, "", "Warehouse_P03_RollTransaction.xltx", 6, true, null, objApp);      // 將datatable copy to excel

            Marshal.ReleaseComObject(objSheets);
            return; 
        }
    }
}