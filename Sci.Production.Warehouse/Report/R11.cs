using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Warehouse
{
    public partial class R11 : Win.Tems.PrintForm
    {
        // string season, factory, brand, mdivision, orderby, spno1, spno2, fabrictype, stocktype, refno1, refno2;
        private string season;

        // string season, factory, brand, mdivision, orderby, spno1, spno2, fabrictype, stocktype, refno1, refno2;
        private string factory;

        // string season, factory, brand, mdivision, orderby, spno1, spno2, fabrictype, stocktype, refno1, refno2;
        private string brand;

        // string season, factory, brand, mdivision, orderby, spno1, spno2, fabrictype, stocktype, refno1, refno2;
        private string mdivision;

        // string season, factory, brand, mdivision, orderby, spno1, spno2, fabrictype, stocktype, refno1, refno2;
        private string spno1;

        // string season, factory, brand, mdivision, orderby, spno1, spno2, fabrictype, stocktype, refno1, refno2;
        private string spno2;

        // string season, factory, brand, mdivision, orderby, spno1, spno2, fabrictype, stocktype, refno1, refno2;
        private string fabrictype;

        // string season, factory, brand, mdivision, orderby, spno1, spno2, fabrictype, stocktype, refno1, refno2;
        private string stocktype;

        // string season, factory, brand, mdivision, orderby, spno1, spno2, fabrictype, stocktype, refno1, refno2;
        private string refno1;

        // string season, factory, brand, mdivision, orderby, spno1, spno2, fabrictype, stocktype, refno1, refno2;
        private string refno2;
        private DateTime? issueDate1;
        private DateTime? issueDate2;
        private DateTime? buyerDelivery1;
        private DateTime? buyerDelivery2;
        private DataTable printData;

        public R11(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            DataTable factory;
            DBProxy.Current.Select(null, "select '' as ID union all select ID from Factory WITH (NOLOCK) ", out factory);
            this.txtMdivision.Text = Env.User.Keyword;
            MyUtility.Tool.SetupCombox(this.comboFabricType, 2, 1, ",ALL,F,Fabric,A,Accessory");
            this.comboFabricType.SelectedIndex = 0;
            MyUtility.Tool.SetupCombox(this.comboStockType, 2, 1, ",ALL,D,Bulk,E,Inventory");
            this.comboStockType.SelectedIndex = 0;
        }

        // 驗證輸入條件

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(this.dateScrapDate.Value1) && MyUtility.Check.Empty(this.dateScrapDate.Value2) &&
                MyUtility.Check.Empty(this.dateBuyerDelivery.Value1) && MyUtility.Check.Empty(this.dateBuyerDelivery.Value2) &&
                (MyUtility.Check.Empty(this.txtSPNoStart.Text) && MyUtility.Check.Empty(this.txtSPNoEnd.Text)))
            {
                MyUtility.Msg.WarningBox("< Scrap Date > & < Buyer Delivery > & < SP# > can't be empty!!");
                return false;
            }

            this.issueDate1 = this.dateScrapDate.Value1;
            this.issueDate2 = this.dateScrapDate.Value2;
            this.buyerDelivery1 = this.dateBuyerDelivery.Value1;
            this.buyerDelivery2 = this.dateBuyerDelivery.Value2;
            this.spno1 = this.txtSPNoStart.Text;
            this.spno2 = this.txtSPNoEnd.Text;
            this.season = this.txtseason.Text;
            this.mdivision = this.txtMdivision.Text;
            this.factory = this.txtfactory.Text;
            this.brand = this.txtbrand.Text;
            this.fabrictype = this.comboFabricType.SelectedValue.ToString();
            this.stocktype = this.comboStockType.SelectedValue.ToString();
            this.refno1 = this.txtRefnoStart.Text;
            this.refno2 = this.txtRefnoEnd.Text;

            return base.ValidateInput();
        }

        // 非同步取資料

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            #region -- sql parameters declare --
            System.Data.SqlClient.SqlParameter sp_spno1 = new System.Data.SqlClient.SqlParameter();
            sp_spno1.ParameterName = "@spno1";

            System.Data.SqlClient.SqlParameter sp_spno2 = new System.Data.SqlClient.SqlParameter();
            sp_spno2.ParameterName = "@spno2";

            System.Data.SqlClient.SqlParameter sp_season = new System.Data.SqlClient.SqlParameter();
            sp_season.ParameterName = "@season";

            System.Data.SqlClient.SqlParameter sp_mdivision = new System.Data.SqlClient.SqlParameter();
            sp_mdivision.ParameterName = "@MDivision";

            System.Data.SqlClient.SqlParameter sp_factory = new System.Data.SqlClient.SqlParameter();
            sp_factory.ParameterName = "@Factory";

            System.Data.SqlClient.SqlParameter sp_brand = new System.Data.SqlClient.SqlParameter();
            sp_brand.ParameterName = "@brand";

            System.Data.SqlClient.SqlParameter sp_refno1 = new System.Data.SqlClient.SqlParameter();
            sp_refno1.ParameterName = "@refno1";

            System.Data.SqlClient.SqlParameter sp_refno2 = new System.Data.SqlClient.SqlParameter();
            sp_refno2.ParameterName = "@refno2";

            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
            #endregion

            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(@"
with cte as (
    select  orderid = sd.FromPOID
            ,seq1 = sd.FromSeq1
            ,seq2 = sd.fromseq2
            ,roll = sd.FromRoll
            ,dyelot = sd.FromDyelot
            ,p.Refno
            ,[description] = dbo.getMtlDesc(sd.FromPOID,sd.FromSeq1,sd.fromseq2,2,0) 
            ,fabrictype = iif(p.FabricType = 'F','Fabric','Accessory') 
            ,weaventype = iif(p.FabricType = 'F',(select fabric.weavetypeid 
                                                  from dbo.fabric WITH (NOLOCK) 
                                                  where fabric.scirefno = p.scirefno)
                                                ,(select fabric.mtltypeid 
                                                  from dbo.fabric WITH (NOLOCK) 
                                                  where fabric.scirefno = p.scirefno))
            , ColorID = dbo.GetColorMultipleID(o.BrandID,p.ColorID) 
            , p.SizeSpec
            ,s.MDivisionID
            ,o.FactoryID
            ,o.BrandID
            ,o.SeasonID
            ,p.POUnit
            ,p.StockUnit
            ,p.Price
            ,unitprice = round(p.Price 
                               / (select unit.PriceRate from dbo.Unit WITH (NOLOCK) where id = p.POUnit) 
                               * dbo.getRate('FX'
                                              ,(select supp.CurrencyId 
                                                from dbo.Supp WITH (NOLOCK) 
                                                inner join dbo.po_supp on po_supp.suppid = supp.id 
                                                where po_supp.id = sd.FromPOID and po_supp.seq1 = sd.FromSeq1)
                                              ,'USD'
                                              ,GETDATE())
                               ,5) 
            ,rate = dbo.getRate('FX'
                                ,(select supp.CurrencyId 
                                  from dbo.Supp WITH (NOLOCK) 
                                  inner join dbo.po_supp on po_supp.suppid = supp.id 
                                  where po_supp.id = sd.FromPOID and po_supp.seq1 = sd.FromSeq1)
                                ,'USD'
                                ,GETDATE())  
            ,currencyid = (select supp.CurrencyId 
                           from dbo.Supp WITH (NOLOCK) 
                           inner join dbo.po_supp on po_supp.suppid = supp.id 
                           where po_supp.id = sd.FromPOID and po_supp.seq1 = sd.FromSeq1) 
            ,p.Qty + p.FOC as Qty
            ,p.NETQty
            ,p.LossQty
            ,scrapqty = Round(dbo.GetUnitQty(p.StockUnit, p.POUnit, sum(sd.Qty)), 2)
            ,location = dbo.Getlocation(fi.ukey)
            ,s.IssueDate
			,o.StyleID
			,o.OrderTypeID
			,o.Category 
			,ps.SuppID
			,Fi.InQty
from dbo.orders o WITH (NOLOCK) 
inner join dbo.SubTransfer_Detail sd WITH (NOLOCK) on o.id = sd.FromPOID
inner join dbo.SubTransfer s WITH (NOLOCK) on s.id = sd.id
inner join dbo.PO_Supp_Detail p WITH (NOLOCK) on p.id = sd.FromPOID and p.seq1 = sd.FromSeq1 and p.seq2 = sd.FromSeq2
left join dbo.FtyInventory Fi on sd.FromPoid = fi.poid and sd.fromSeq1 = fi.seq1 and sd.fromSeq2 = fi.seq2 
    and sd.fromRoll = fi.roll and sd.fromStocktype = fi.stocktype and sd.fromDyelot = fi.Dyelot
left join PO_Supp ps on ps.id = p.id and ps. SEQ1 = p.SEQ1
where s.Status = 'Confirmed' 
");

            #region --- 條件組合  ---
            if (!MyUtility.Check.Empty(this.stocktype))
            {
                sqlCmd.Append(string.Format(" and s.type = '{0}' ", this.stocktype));
            }
            else
            {
                sqlCmd.Append(" and s.type in ('D','E') ");
            }

            if (!MyUtility.Check.Empty(this.buyerDelivery1) || !MyUtility.Check.Empty(this.buyerDelivery2))
            {
                if (!MyUtility.Check.Empty(this.buyerDelivery1))
                {
                    sqlCmd.Append(string.Format(@" and '{0}' <= o.BuyerDelivery", Convert.ToDateTime(this.buyerDelivery1).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.buyerDelivery2))
                {
                    sqlCmd.Append(string.Format(@" and o.BuyerDelivery <= '{0}'", Convert.ToDateTime(this.buyerDelivery2).ToString("d")));
                }
            }

            if (!MyUtility.Check.Empty(this.spno1) && !MyUtility.Check.Empty(this.spno2))
            {
                // 若 sp 兩個都輸入則尋找 sp1 - sp2 區間的資料
                sqlCmd.Append(" and sd.FromPoid >= @spno1 and sd.FromPoid <= @spno2");
                sp_spno1.Value = this.spno1.PadRight(10, '0');
                sp_spno2.Value = this.spno2.PadRight(10, 'Z');
                cmds.Add(sp_spno1);
                cmds.Add(sp_spno2);
            }
            else if (!MyUtility.Check.Empty(this.spno1))
            {
                // 只有 sp1 輸入資料
                sqlCmd.Append(" and sd.FromPoid like @spno1 ");
                sp_spno1.Value = this.spno1 + "%";
                cmds.Add(sp_spno1);
            }
            else if (!MyUtility.Check.Empty(this.spno2))
            {
                // 只有 sp2 輸入資料
                sqlCmd.Append(" and sd.FromPoid like @spno2 ");
                sp_spno2.Value = this.spno2 + "%";
                cmds.Add(sp_spno2);
            }

            if (!MyUtility.Check.Empty(this.issueDate1) || !MyUtility.Check.Empty(this.issueDate2))
            {
                if (!MyUtility.Check.Empty(this.issueDate1))
                {
                    sqlCmd.Append(string.Format(@" and '{0}' <= s.issuedate", Convert.ToDateTime(this.issueDate1).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.issueDate2))
                {
                    sqlCmd.Append(string.Format(@" and s.issuedate <= '{0}'", Convert.ToDateTime(this.issueDate2).ToString("d")));
                }
            }

            if (!MyUtility.Check.Empty(this.season))
            {
                sqlCmd.Append(" and o.seasonid = @season");
                sp_season.Value = this.season;
                cmds.Add(sp_season);
            }

            if (!MyUtility.Check.Empty(this.mdivision))
            {
                sqlCmd.Append(" and S.mdivisionid = @MDivision");
                sp_mdivision.Value = this.mdivision;
                cmds.Add(sp_mdivision);
            }

            if (!MyUtility.Check.Empty(this.factory))
            {
                sqlCmd.Append(" and o.FactoryID = @Factory");
                sp_factory.Value = this.factory;
                cmds.Add(sp_factory);
            }

            if (!MyUtility.Check.Empty(this.fabrictype))
            {
                sqlCmd.Append(string.Format(@" and p.FabricType = '{0}'", this.fabrictype));
            }

            if (!MyUtility.Check.Empty(this.brand))
            {
                sqlCmd.Append(" and o.brandid = @brand");
                sp_brand.Value = this.brand;
                cmds.Add(sp_brand);
            }

            if (!MyUtility.Check.Empty(this.refno1) && !MyUtility.Check.Empty(this.refno2))
            {
                // Refno 兩個都輸入則尋找 Refno1 - Refno2 區間的資料
                sqlCmd.Append(" and p.refno >= @refno1 and p.refno <= @refno2");
                sp_refno1.Value = this.refno1;
                sp_refno2.Value = this.refno2;
                cmds.Add(sp_refno1);
                cmds.Add(sp_refno2);
            }
            else if (!MyUtility.Check.Empty(this.refno1))
            {
                // 只輸入 Refno1
                sqlCmd.Append(" and p.refno like @refno1");
                sp_refno1.Value = this.refno1 + "%";
                cmds.Add(sp_refno1);
            }
            else if (!MyUtility.Check.Empty(this.refno2))
            {
                // 只輸入 Refno2
                sqlCmd.Append(" and p.refno like @refno2");
                sp_refno2.Value = this.refno2 + "%";
                cmds.Add(sp_refno2);
            }
            #endregion

            sqlCmd.Append(@" group by sd.FromPOID, sd.FromSeq1, sd.fromseq2,sd.FromRoll,sd.FromDyelot, p.Refno, p.SCIRefno, p.FabricType,s.MDivisionID, o.FactoryID, o.BrandID, o.SeasonID, p.POUnit, p.StockUnit,p.Price ,p.Qty + p.FOC, p.NETQty, p.LossQty, s.IssueDate,fi.ukey,p.ColorID, p.SizeSpec,o.StyleID,o.OrderTypeID, o.Category,ps.SuppID,Fi.InQty)");

            // List & Summary 各撈自己需要的欄位
            if (this.radioSummary.Checked)
            {
                #region -- Summary Sql Command --
                sqlCmd.Append(string.Format(@"
select  t.orderid
        ,t.seq1
        ,t.seq2
        ,t.Refno
        ,t.description
		,t.StyleID
		,t.OrderTypeID 
		,Category = case when t.Category ='B' then 'Bulk'
					     when t.Category ='S' then 'Sample'
					     when t.Category ='M' then 'Material'
					     when t.Category ='O' then 'Other'
					     when t.Category ='G' then 'Garment'
					     when t.Category ='T' then 'SMLT'
					end
		,t.SuppID
		,(select AbbEN from Supp where id = t.SuppID)
        ,t.fabrictype
        ,t.weaventype
        ,t.ColorID
        ,t.SizeSpec
        ,t.MDivisionID
        ,t.FactoryID
        ,t.BrandID
        ,t.SeasonID
        ,t.POUnit
		,CurrencyID    
        ,unitprice
        ,t.Qty
        ,t.unitprice*t.Qty
        ,t.NETQty
        ,t.LossQty   
		,sum(InQty)
        ,sum(t.scrapqty)
        ,t.unitprice*sum(t.scrapqty)
        ,t.IssueDate
from cte t
group by t.orderid,t.seq1,t.seq2,t.description,t.Refno,t.fabrictype,t.weaventype,t.MDivisionID,t.FactoryID,t.BrandID,t.SeasonID,t.POUnit,unitprice,t.Qty,t.unitprice*t.Qty,t.NETQty,t.LossQty,t.IssueDate,t.ColorID,t.SizeSpec,t.StyleID,t.OrderTypeID, t.Category,t.SuppID,CurrencyID"));
                #endregion
            }
            else
            {
                #region -- List Sql Command --
                sqlCmd.Append(string.Format(@"
select  t.orderid
        ,t.seq1
        ,t.seq2
        ,t.roll
        ,t.dyelot
		,t.StyleID
		,t.OrderTypeID 
		,Category = case when t.Category ='B' then 'Bulk'
					     when t.Category ='S' then 'Sample'
					     when t.Category ='M' then 'Material'
					     when t.Category ='O' then 'Other'
					     when t.Category ='G' then 'Garment'
					     when t.Category ='T' then 'SMLT'
					end
		,t.SuppID
		,(select AbbEN from Supp where id = t.SuppID)
        ,t.description
        ,t.Refno
        ,t.fabrictype
        ,t.ColorID
        ,t.SizeSpec
        ,t.MDivisionID
        ,t.FactoryID
        ,t.BrandID
        ,t.SeasonID
        ,t.pounit
		,CurrencyID 
		,t.InQty
        ,unitprice
        ,t.scrapqty
        ,t.unitprice*t.scrapqty
        ,t.location
        ,t.IssueDate
from cte t"));
                #endregion
            }

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), cmds, out this.printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Ict.Result.True;
        }

        // 產生Excel

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData.Rows.Count);

            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            // if (checkBox1.Checked)
            //    MyUtility.Excel.CopyToXls(printData, "", "Warehouse_R11_Summary.xltx", 3);
            // else
            //    MyUtility.Excel.CopyToXls(printData, "", "Warehouse_R11_List.xltx", 3);
            string excelXltx = string.Empty;
            int descIndex = 0;

            if (this.radioSummary.Checked)
            {
                excelXltx = "Warehouse_R11_Summary.xltx";
                descIndex = 5;
            }
            else
            {
                excelXltx = "Warehouse_R11_List.xltx";
                descIndex = 6;
            }

            Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\" + excelXltx); // 預先開啟excel app
            MyUtility.Excel.CopyToXls(this.printData, string.Empty, excelXltx, 2, showExcel: false, showSaveMsg: false, excelApp: objApp);      // 將datatable copy to excel
            Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表

            this.ShowWaitMessage("Excel Processing...");
            for (int i = 1; i <= this.printData.Rows.Count; i++)
            {
                string str = objSheets.Cells[i + 3, descIndex].Value();
                str = MyUtility.Check.Empty(str) ? string.Empty : str;
                objSheets.Cells[i + 3, descIndex] = str.Trim();
            }

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName(this.radioSummary.Checked ? "Warehouse_R11_Summary" : "Warehouse_R11_List");
            objApp.ActiveWorkbook.SaveAs(strExcelName);
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);
            Marshal.ReleaseComObject(objSheets);

            strExcelName.OpenFile();
            #endregion
            this.HideWaitMessage();
            return true;
        }
    }
}
