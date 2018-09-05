﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Warehouse
{
    public partial class R11 : Sci.Win.Tems.PrintForm
    {
        //string season, factory, brand, mdivision, orderby, spno1, spno2, fabrictype, stocktype, refno1, refno2;
        string season, factory, brand, mdivision, spno1, spno2, fabrictype, stocktype, refno1, refno2;
        DateTime? issueDate1, issueDate2, buyerDelivery1, buyerDelivery2;
        DataTable printData;

        public R11(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            DataTable factory;
            DBProxy.Current.Select(null, "select '' as ID union all select ID from Factory WITH (NOLOCK) ", out factory);
            txtMdivision.Text = Sci.Env.User.Keyword;
            MyUtility.Tool.SetupCombox(comboFabricType, 2, 1, ",ALL,F,Fabric,A,Accessory");
            comboFabricType.SelectedIndex = 0;
            MyUtility.Tool.SetupCombox(comboStockType,2,1, ",ALL,D,Bulk,E,Inventory");
            comboStockType.SelectedIndex = 0;
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(dateScrapDate.Value1) && MyUtility.Check.Empty(dateScrapDate.Value2) &&
                MyUtility.Check.Empty(dateBuyerDelivery.Value1) && MyUtility.Check.Empty(dateBuyerDelivery.Value2) &&
                (MyUtility.Check.Empty(txtSPNoStart.Text) && MyUtility.Check.Empty(txtSPNoEnd.Text))) 
            {
                MyUtility.Msg.WarningBox("< Scrap Date > & < Buyer Delivery > & < SP# > can't be empty!!");
                return false;
            }

            issueDate1 = dateScrapDate.Value1;
            issueDate2 = dateScrapDate.Value2;
            buyerDelivery1 = dateBuyerDelivery.Value1;
            buyerDelivery2 = dateBuyerDelivery.Value2;
            spno1 = txtSPNoStart.Text;
            spno2 = txtSPNoEnd.Text;
            season = txtseason.Text;
            mdivision = txtMdivision.Text;
            factory = txtfactory.Text;
            brand = txtbrand.Text;
            fabrictype = comboFabricType.SelectedValue.ToString();
            stocktype = comboStockType.SelectedValue.ToString();
            refno1 = txtRefnoStart.Text;
            refno2 = txtRefnoEnd.Text;

            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
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
			,CAST(Fi.InQty AS INT ) AS InQty
from dbo.orders o WITH (NOLOCK) 
inner join dbo.SubTransfer_Detail sd WITH (NOLOCK) on o.id = sd.FromPOID
inner join dbo.SubTransfer s WITH (NOLOCK) on s.id = sd.id
inner join dbo.PO_Supp_Detail p WITH (NOLOCK) on p.id = sd.FromPOID and p.seq1 = sd.FromSeq1 and p.seq2 = sd.FromSeq2
left join dbo.FtyInventory Fi on sd.FromPoid = fi.poid and sd.fromSeq1 = fi.seq1 and sd.fromSeq2 = fi.seq2 
    and sd.fromRoll = fi.roll and sd.fromStocktype = fi.stocktype
left join PO_Supp ps on ps.id = p.id and ps. SEQ1 = p.SEQ1
where s.Status = 'Confirmed' 
");

            #region --- 條件組合  ---
            if (!MyUtility.Check.Empty(stocktype))
            {
                sqlCmd.Append(string.Format(" and s.type = '{0}' ", stocktype));
            }
            else
            {
                sqlCmd.Append(" and s.type in ('D','E') ");
            }

            if (!MyUtility.Check.Empty(buyerDelivery1) || !MyUtility.Check.Empty(buyerDelivery2))
            {
                if (!MyUtility.Check.Empty(buyerDelivery1))
                    sqlCmd.Append(string.Format(@" and '{0}' <= o.BuyerDelivery", Convert.ToDateTime(buyerDelivery1).ToString("d")));
                if (!MyUtility.Check.Empty(buyerDelivery2))
                    sqlCmd.Append(string.Format(@" and o.BuyerDelivery <= '{0}'", Convert.ToDateTime(buyerDelivery2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(spno1) && !MyUtility.Check.Empty(spno2))
            {
                // 若 sp 兩個都輸入則尋找 sp1 - sp2 區間的資料
                sqlCmd.Append(" and sd.FromPoid >= @spno1 and sd.FromPoid <= @spno2");
                sp_spno1.Value = spno1.PadRight(10, '0');
                sp_spno2.Value = spno2.PadRight(10, 'Z');
                cmds.Add(sp_spno1);
                cmds.Add(sp_spno2);
            }

            else if (!MyUtility.Check.Empty(spno1))
            {
                // 只有 sp1 輸入資料
                sqlCmd.Append(" and sd.FromPoid like @spno1 ");
                sp_spno1.Value = spno1 + "%";
                cmds.Add(sp_spno1);
            }
            else if (!MyUtility.Check.Empty(spno2))
            {
                // 只有 sp2 輸入資料
                sqlCmd.Append(" and sd.FromPoid like @spno2 ");
                sp_spno2.Value = spno2 + "%";
                cmds.Add(sp_spno2);
            }
            if (!MyUtility.Check.Empty(issueDate1) || !MyUtility.Check.Empty(issueDate2))
            {
                if(!MyUtility.Check.Empty(issueDate1))
                    sqlCmd.Append(string.Format(@" and '{0}' <= s.issuedate", Convert.ToDateTime(issueDate1).ToString("d")));
                if (!MyUtility.Check.Empty(issueDate2))
                    sqlCmd.Append(string.Format(@" and s.issuedate <= '{0}'", Convert.ToDateTime(issueDate2).ToString("d")));
            }
            if (!MyUtility.Check.Empty(season))
            {
                sqlCmd.Append(" and o.seasonid = @season");
                sp_season.Value = season;
                cmds.Add(sp_season);
            }
            if (!MyUtility.Check.Empty(mdivision))
            {
                sqlCmd.Append(" and S.mdivisionid = @MDivision");
                sp_mdivision.Value = mdivision;
                cmds.Add(sp_mdivision);
            }
            if (!MyUtility.Check.Empty(factory))
            {
                sqlCmd.Append(" and o.FactoryID = @Factory");
                sp_factory.Value = factory;
                cmds.Add(sp_factory);
            }
            if (!MyUtility.Check.Empty(fabrictype))
            {
                sqlCmd.Append(string.Format(@" and p.FabricType = '{0}'",fabrictype));
            }
            
            if (!MyUtility.Check.Empty(brand))
            {
                sqlCmd.Append(" and o.brandid = @brand");
                sp_brand.Value = brand;
                cmds.Add(sp_brand);
            }

            if (!MyUtility.Check.Empty(refno1) && !MyUtility.Check.Empty(refno2))
            {
                //Refno 兩個都輸入則尋找 Refno1 - Refno2 區間的資料
                sqlCmd.Append(" and p.refno >= @refno1 and p.refno <= @refno2");
                sp_refno1.Value = refno1;
                sp_refno2.Value = refno2;
                cmds.Add(sp_refno1);
                cmds.Add(sp_refno2);
            }
            else if (!MyUtility.Check.Empty(refno1))
            {
                //只輸入 Refno1
                sqlCmd.Append(" and p.refno like @refno1");
                sp_refno1.Value = refno1 + "%";
                cmds.Add(sp_refno1);
            }
            else if (!MyUtility.Check.Empty(refno2))
            {
                //只輸入 Refno2
                sqlCmd.Append(" and p.refno like @refno2");
                sp_refno2.Value = refno2 + "%";
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
		,t.InQty
        ,sum(t.scrapqty)
        ,t.unitprice*sum(t.scrapqty)
        ,t.IssueDate
from cte t
group by t.orderid,t.seq1,t.seq2,t.description,t.Refno,t.fabrictype,t.weaventype,t.MDivisionID,t.FactoryID,t.BrandID,t.SeasonID,t.POUnit,unitprice,t.Qty,t.unitprice*t.Qty,t.NETQty,t.LossQty,t.IssueDate,t.ColorID,t.SizeSpec,t.StyleID,t.OrderTypeID, t.Category,t.SuppID,CurrencyID,t.InQty"));
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

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), cmds, out printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }
            return Result.True;
        }

        // 產生Excel
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            SetCount(printData.Rows.Count);

            if (printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            //if (checkBox1.Checked)
            //    MyUtility.Excel.CopyToXls(printData, "", "Warehouse_R11_Summary.xltx", 3);
            //else
            //    MyUtility.Excel.CopyToXls(printData, "", "Warehouse_R11_List.xltx", 3);

            string ExcelXltx = "";
            int DescIndex = 0;

            if (this.radioSummary.Checked)
            {
                ExcelXltx = "Warehouse_R11_Summary.xltx";
                DescIndex = 5;
            }
            else
            {
                ExcelXltx = "Warehouse_R11_List.xltx";
                DescIndex = 6;
            }

            Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\" + ExcelXltx); //預先開啟excel app
            MyUtility.Excel.CopyToXls(printData, "", ExcelXltx, 2, showExcel: false, showSaveMsg: false, excelApp: objApp);      // 將datatable copy to excel
            Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表

            this.ShowWaitMessage("Excel Processing...");
            for (int i = 1; i <= printData.Rows.Count; i++)
            {
                string str = objSheets.Cells[i + 3, DescIndex].Value();
                str = (MyUtility.Check.Empty(str)) ? "" : str ;
                objSheets.Cells[i + 3, DescIndex] = str.Trim();
            }

            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName(this.radioSummary.Checked ? "Warehouse_R11_Summary" : "Warehouse_R11_List");
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
