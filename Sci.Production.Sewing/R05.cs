using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Data;

namespace Sci.Production.Sewing
{
    /// <summary>
    /// R05
    /// </summary>
    public partial class R05 : Win.Tems.PrintForm
    {
        private DataTable printData;
        private DateTime? buyerDate1;
        private DateTime? buyerDate2;
        private DateTime? sciDate1;
        private DateTime? sciDate2;
        private string factory;
        private string sp_from;
        private string sp_to;
        private string brand;
        private string status;
        private string reportType;
        private string sqlCmd;

        /// <summary>
        /// R05
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R05(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            MyUtility.Tool.SetupCombox(this.comboReportType, 2, 1, "G,Garment Order Allocate Output,B,Booking Order Assign to Garment Order");
            this.cb_status.SelectedIndex = 0;
            this.comboReportType.SelectedIndex = 0;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.buyerDate1 = this.dateBuyerDelivery.Value1;
            this.buyerDate2 = this.dateBuyerDelivery.Value2;
            this.sciDate1 = this.dateSCIDelivery.Value1;
            this.sciDate2 = this.dateSCIDelivery.Value2;
            this.factory = this.txtfactory.Text;
            this.sp_from = this.txtsp_from.Text;
            this.sp_to = this.txtsp_to.Text;
            this.brand = this.txtbrand.Text;
            this.status = this.comboReportType.SelectedIndex == 0 ? this.cb_status.Text : string.Empty;
            this.reportType = this.comboReportType.SelectedValue.ToString();
            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            if (this.reportType == "G")
            {
                this.sqlCmd = this.SQL_GarmentOrder();
            }
            else if (this.reportType == "B")
            {
                this.sqlCmd = this.SQL_BookingOrder();
            }

            return Ict.Result.True;
        }

        private string SQL_GarmentOrder()
        {
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(string.Format(@"
Select  [Sp#] = OQG.ID
        ,[StyleID] = O.StyleID
        , o.BuyerDelivery
        , o.SciDelivery
        , o.CPU
        ,[Brand] = O.BrandID
        ,[Season] = O.SeasonID
        ,[*] = SL.Location
        ,[From SP#] = OQG.OrderIDFrom
        ,[Article] = OQG.Article
        ,[SizeCode] =OQG.SizeCode
        ,[ToSP_Cmf_PK_Qty] = PL.PackQty
        ,[ToSP_Qty] = IIF(OQG.Junk=0,OQG.Qty,0)
        ,[ToSp_allocate_qty] = SODDG.ToSp_allocate_qty
        ,[ToSp_Balance] =  IIF(OQG.Junk=0,OQG.Qty,0) - SODDG.ToSp_allocate_qty
        ,[ToSp_BuyerDelivery] = O.BuyerDelivery
        ,[FromSp_Sewing_qty] = SODD.FromSp_Sewing_qty
        ,[FromSp_Accu_Qty] = SODDG2.FromSp_Accu_Qty
        ,[FromSp_Avl_Qty] = SODD.FromSp_Sewing_qty  -  SODDG2.FromSp_Accu_Qty
        ,[Is_Trans_Qty] = IIF(SODDG.ToSp_allocate_qty >= IIF(OQG.Junk=0,OQG.Qty,0), 'N/A' , IIF((SODD.FromSp_Sewing_qty  -  SODDG2.FromSp_Accu_Qty) >= (IIF(OQG.Junk=0,OQG.Qty,0) - SODDG.ToSp_allocate_qty) , 'Y' , 'N' ))
         ,[Is_Trans_Qty_Excess] = IIF(SODDG.ToSp_allocate_qty > IIF(OQG.Junk=0,OQG.Qty,0) , 'Y' , 'N')
         ,[Junk] = IIF(OQG.Junk = 1, 'Y', 'N')
into #tmp
From Order_Qty_Garment OQG WITH (NOLOCK)
Inner join Orders O  WITH (NOLOCK) on OQG.id=O.id
Inner join Factory F  WITH (NOLOCK) on O.FactoryID=F.ID
Left join Style_Location  SL  WITH (NOLOCK) on O.styleukey=SL.Styleukey 
outer apply(select isnull(Sum(b.shipqty),0) AS PackQty
                from PackingList a  WITH (NOLOCK)
                inner join PackingList_Detail b on a.ID=b.ID
                where b.OrderID= OQG.ID
                      and b.Article = OQG.Article 
                        and b.SizeCode = OQG.SizeCode
                      and a.Status= 'Confirmed') as PL
outer apply(select Isnull(Sum(SewingOutput_Detail_Detail_Garment.QAQty),0) as ToSp_allocate_qty
                from SewingOutput_Detail_Detail_Garment  WITH (NOLOCK)
                Where SewingOutput_Detail_Detail_Garment.OrderId = OQG.ID
                		and SewingOutput_Detail_Detail_Garment.ComboType = SL.Location
                		and SewingOutput_Detail_Detail_Garment.Article =  OQG.Article 
                		and SewingOutput_Detail_Detail_Garment.SizeCode = OQG.SizeCode
                		and SewingOutput_Detail_Detail_Garment.OrderIDfrom = OQG.OrderIDFrom
                ) as SODDG
outer apply(select Isnull(Sum(SewingOutput_Detail_Detail.QAQty),0) as FromSp_Sewing_qty
            from SewingOutput_Detail_Detail  WITH (NOLOCK)
            Where SewingOutput_Detail_Detail.OrderId = OQG.OrderIDFrom
            		and SewingOutput_Detail_Detail.ComboType = SL.Location
            		and SewingOutput_Detail_Detail.Article = OQG.Article
            		and SewingOutput_Detail_Detail.SizeCode = OQG.SizeCode
             ) as SODD
outer apply(select Isnull(Sum(SewingOutput_Detail_Detail_Garment.QAQty),0) as FromSp_Accu_Qty
            from SewingOutput_Detail_Detail_Garment  WITH (NOLOCK)
            Where SewingOutput_Detail_Detail_Garment.ComboType = SL.Location
            		and SewingOutput_Detail_Detail_Garment.Article = OQG.Article
            		and SewingOutput_Detail_Detail_Garment.SizeCode =OQG.SizeCode
            		and SewingOutput_Detail_Detail_Garment.OrderIDfrom = OQG.OrderIDFrom
            ) as SODDG2
where 1 = 1 
                "));

            if (!MyUtility.Check.Empty(this.buyerDate1))
            {
                sqlCmd.Append(string.Format(" and o.BuyerDelivery >= '{0}' ", Convert.ToDateTime(this.buyerDate1).ToString("yyyy/MM/dd")));
            }

            if (!MyUtility.Check.Empty(this.buyerDate2))
            {
                sqlCmd.Append(string.Format(" and o.BuyerDelivery <= '{0}' ", Convert.ToDateTime(this.buyerDate2).ToString("yyyy/MM/dd")));
            }

            if (!MyUtility.Check.Empty(this.sciDate1))
            {
                sqlCmd.Append(string.Format(" and o.SciDelivery >= '{0}' ", Convert.ToDateTime(this.sciDate1).ToString("yyyy/MM/dd")));
            }

            if (!MyUtility.Check.Empty(this.sciDate2))
            {
                sqlCmd.Append(string.Format(" and o.SciDelivery <= '{0}' ", Convert.ToDateTime(this.sciDate2).ToString("yyyy/MM/dd")));
            }

            if (!MyUtility.Check.Empty(this.factory))
            {
                sqlCmd.Append(string.Format(" and o.FtyGroup = '{0}'", this.factory));
            }

            if (!MyUtility.Check.Empty(this.sp_from))
            {
                sqlCmd.Append(string.Format(" and OQG.ID >= '{0}'", this.sp_from));
            }

            if (!MyUtility.Check.Empty(this.sp_to))
            {
                sqlCmd.Append(string.Format(" and OQG.ID <= '{0}'", this.sp_to));
            }

            if (!MyUtility.Check.Empty(this.brand))
            {
                sqlCmd.Append(string.Format(" and o.BrandID = '{0}'", this.brand));
            }

            string status_condistion = string.Empty;
            if (this.status.Equals("Unfinished"))
            {
                status_condistion = " where ToSp_allocate_qty < ToSP_Qty";
            }
            else if (this.status.Equals("Finished"))
            {
                status_condistion = " where ToSp_allocate_qty = ToSP_Qty";
            }
            else if (this.status.Equals("Excess"))
            {
                status_condistion = " where ToSp_allocate_qty > ToSP_Qty";
            }

            sqlCmd.Append($@"
select t.[Sp#]
	, g.BtoG
into #tmp_Order_Qty_Garment
from ( select distinct [Sp#] from #tmp t {status_condistion}) t
outer apply (
	SELECT [BtoG] = CASE 
		 	 WHEN COUNT(*) > 1 THEN 'O'
		 	 WHEN g4.gQty = b4.bQty THEN 'A'
			 WHEN g4.gQty <> b4.bQty THEN 'O'
		 	 ELSE ''
		  END
	FROM (
		  SELECT DISTINCT [gPOID] = g3.POID,
			   [bPOID] = b3.POID
		  FROM Order_Qty_Garment og3 WITH (NOLOCK)
		  INNER JOIN Orders g3 WITH (NOLOCK) ON og3.ID = g3.ID AND g3.Category = 'G'
		  INNER JOIN Orders b3 WITH (NOLOCK) ON og3.OrderIDFrom = b3.ID AND b3.Category = 'B'
		  INNER JOIN (
			    SELECT DISTINCT [gPOID] = g2.POID, 
				     [bPOID] = b2.POID
			    FROM Order_Qty_Garment og2 WITH (NOLOCK)
			    INNER JOIN Orders g2 WITH (NOLOCK) ON og2.ID = g2.ID AND g2.Category = 'G'
			    INNER JOIN Orders b2 WITH (NOLOCK) ON og2.OrderIDFrom = b2.ID  AND b2.Category = 'B'
			    INNER JOIN Order_Qty_Garment og1 WITH (NOLOCK) ON og1.ID = t.[Sp#] AND og1.Junk = 0
			    INNER JOIN Orders g1 WITH (NOLOCK) ON og1.ID = g1.ID AND g1.Category = 'G'
			    INNER JOIN Orders b1 WITH (NOLOCK) ON og1.OrderIDFrom = b1.ID AND b1.Category = 'B'
			    WHERE (g1.POID = g2.POID OR b1.POID = b2.POID)
				and og2.Junk = 0
		  ) tmp ON (g3.POID = tmp.gPOID OR b3.POID = tmp.bPOID) AND og3.Junk = 0
	) tmp 
	OUTER APPLY (
		SELECT [gQty] = SUM(g4.Qty) 
		FROM Orders g4 WITH (NOLOCK) 
		WHERE tmp.gPOID = g4.POID 
		and g4.Category = 'G'
	) g4
	OUTER APPLY (
		SELECT [bQty] = SUM(b4.Qty) 
		FROM Orders b4 WITH (NOLOCK) 
		WHERE tmp.bPOID = b4.POID 
		and b4.Category = 'B'
	) b4 
	GROUP BY g4.gQty, b4.bQty
) g

select t.[Sp#]
     , t.[StyleID]
	 , t.BuyerDelivery
	 , t.SciDelivery
	 , t.CPU
     , t.[Brand]
     , t.[Season]
	 , g.[BtoG]
     , t.[*]
     , t.[From SP#]
     , t.[Article]
     , t.[SizeCode]
     , t.[ToSP_Cmf_PK_Qty]
     , t.[ToSP_Qty]
     , t.[ToSp_allocate_qty]
     , t.[ToSp_Balance]
     , t.[ToSp_BuyerDelivery]
     , t.[FromSp_Sewing_qty]
     , t.[FromSp_Accu_Qty]
     , t.[FromSp_Avl_Qty]
     , t.[Is_Trans_Qty]
     , t.[Is_Trans_Qty_Excess] 
	 , t.[Junk]
from #tmp t
inner join #tmp_Order_Qty_Garment g on t.[Sp#] = g.[Sp#]
{status_condistion}

drop table #tmp, #tmp_Order_Qty_Garment
");

            return sqlCmd.ToString();
        }

        private string SQL_BookingOrder()
        {
            string sqlWhere = string.Empty;
            if (!MyUtility.Check.Empty(this.buyerDate1))
            {
                sqlWhere += string.Format(" and o.BuyerDelivery >= '{0}' ", Convert.ToDateTime(this.buyerDate1).ToString("yyyy/MM/dd"));
            }

            if (!MyUtility.Check.Empty(this.buyerDate2))
            {
                sqlWhere += string.Format(" and o.BuyerDelivery <= '{0}' ", Convert.ToDateTime(this.buyerDate2).ToString("yyyy/MM/dd"));
            }

            if (!MyUtility.Check.Empty(this.sciDate1))
            {
                sqlWhere += string.Format(" and o.SciDelivery >= '{0}' ", Convert.ToDateTime(this.sciDate1).ToString("yyyy/MM/dd"));
            }

            if (!MyUtility.Check.Empty(this.sciDate2))
            {
                sqlWhere += string.Format(" and o.SciDelivery <= '{0}' ", Convert.ToDateTime(this.sciDate2).ToString("yyyy/MM/dd"));
            }

            if (!MyUtility.Check.Empty(this.factory))
            {
                sqlWhere += string.Format(" and o.FtyGroup = '{0}'", this.factory);
            }

            if (!MyUtility.Check.Empty(this.sp_from))
            {
                sqlWhere += string.Format(" and o.ID >= '{0}'", this.sp_from);
            }

            if (!MyUtility.Check.Empty(this.sp_to))
            {
                sqlWhere += string.Format(" and o.ID <= '{0}'", this.sp_to);
            }

            if (!MyUtility.Check.Empty(this.brand))
            {
                sqlWhere += string.Format(" and o.BrandID = '{0}'", this.brand);
            }

            string sqlCmd = $@"
select [Sp#] = o.ID	
	, [Style] = o.StyleID
	, [Buyer Delivery] = o.BuyerDelivery
	, [SCI Delivery] = o.SciDelivery
	, [KPI LETA] = o.KPILETA
	, [PF ETA] = o.PFETA
	, o.CPU
	, [B --> G Garment Type] = g.BtoG
	, [Brand] = o.BrandID
	, [Season] = o.SeasonID
	, [Garment SP#] = og.ID
	, [Article] = og.Article
	, og.SizeCode
	, og.Qty
into #tmp
from Order_Qty_Garment og
inner join Orders o WITH(NOLOCK) on og.OrderIDFrom = o.ID and o.Category = 'B'
outer apply (
	SELECT [BtoG] = IIF(COUNT(*) > 1 , 'O' ,IIF(SUM(g.[gQTY]) = SUM(g.[bQTY]), 'A' ,'O'))
	FROM (
		SELECT [gPOID] = g3.POID,
			[bPOID] = b3.POID,
			[gQTY] = SUM(g3.Qty),
			[bQTY] = SUM(b3.Qty)
		FROM Order_Qty_Garment og3
		INNER JOIN Orders g3 ON og3.ID = g3.ID
		INNER JOIN Orders b3 ON og3.OrderIDFrom = b3.ID 
		INNER JOIN (
				SELECT DISTINCT [gPOID] = g2.POID, 
					[bPOID] = b2.POID
				FROM Order_Qty_Garment og2 
				INNER JOIN Orders g2 ON og2.ID = g2.ID
				INNER JOIN Orders b2 ON og2.OrderIDFrom = b2.ID 
				INNER JOIN Order_Qty_Garment og1 ON og1.ID = og.ID AND og1.Junk = 0
				INNER JOIN Orders g1 ON og1.ID = g1.ID AND g1.Category = 'G'
				INNER JOIN Orders b1 ON og1.OrderIDFrom = b1.ID AND b1.Category = 'B'
				WHERE (g1.POID = g2.POID OR b1.POID = b2.POID)
			) tmp ON (g3.POID = tmp.gPOID OR b3.POID = tmp.bPOID) AND og3.Junk = 0
		GROUP BY g3.POID, b3.POID
	)g 
) g
where 1 = 1
{sqlWhere}
and og.Junk = 0

DECLARE @cols AS NVARCHAR(MAX), @query AS NVARCHAR(MAX)

SELECT @cols = STUFF((SELECT ',' + QUOTENAME(SizeCode)
                      FROM #tmp
					  GROUP BY SizeCode
					  ORDER BY 
							CASE WHEN TRY_CONVERT(INT, SizeCode) IS NOT NULL THEN SizeCode ELSE 999 END,
							SizeCode COLLATE Latin1_General_100_CI_AI
                      FOR XML PATH(''), TYPE
                     ).value('.', 'NVARCHAR(MAX)'), 1, 1, '')


SET @query = 'SELECT * FROM ( SELECT * FROM #tmp t ) src
              PIVOT ( MAX(Qty) FOR SizeCode IN (' + @cols + ') ) piv'
EXECUTE(@query)


drop table #tmp
                ";
            return sqlCmd;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            DualResult result = DBProxy.Current.Select(null, this.sqlCmd, out this.printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData.Rows.Count);

            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            this.ShowWaitMessage("Starting EXCEL...");
            bool excelresult = false;
            if (this.reportType == "G")
            {
                excelresult = this.ExcelCreateGarment();
            }
            else if (this.reportType == "B")
            {
                excelresult = this.ExcelCreateBooking();
            }

            if (!excelresult)
            {
                MyUtility.Msg.WarningBox(excelresult.ToString(), "Warning");
            }
            this.HideWaitMessage();
            return true;
        }

        private bool ExcelCreateGarment()
        {
            string excelFile = "Sewing_R05_GarmentOrderAllocateOutputReport.xltx";
            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + excelFile); // 開excelapp
            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
            bool result = MyUtility.Excel.CopyToXls(this.printData, string.Empty, xltfile: excelFile, headerRow: 1, excelApp: objApp);
            return result;
        }

        private bool ExcelCreateBooking()
        {
            string excelFile = "Sewing_R05_BookingOrderAssigntoGarmentOrderReport.xltx";
            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + excelFile); // 開excelapp
            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表

            for (int c = 1; c <= this.printData.Columns.Count; c++)
            {
                objSheets.Cells[1, c].Copy();
                if (c < this.printData.Columns.Count)
                {
                    objSheets.Cells[1, c + 1].PasteSpecial(Microsoft.Office.Interop.Excel.XlPasteType.xlPasteFormats, Microsoft.Office.Interop.Excel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, false, false);
                }
            }

            for (int c = 1; c <= this.printData.Columns.Count; c++)
            {
                objSheets.Cells[1, c] = this.printData.Columns[c - 1].ToString();
            }

            bool result = MyUtility.Excel.CopyToXls(this.printData, string.Empty, xltfile: excelFile, headerRow: 1, excelApp: objApp);
            return result;
        }

        private void ComboReportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cb_status.Enabled = true;
            if (((ComboBox)sender).SelectedIndex == 1)
            {
                this.cb_status.Enabled = false;
            }
        }
    }
}
