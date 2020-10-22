using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;

namespace Sci.Production.Planning
{
    /// <summary>
    /// R01
    /// </summary>
    public partial class R01 : Win.Tems.PrintForm
    {
        private string factory;
        private string mdivision;
        private string category;
        private DateTime? sciDelivery1;
        private DateTime? sciDelivery2;
        private DataTable printData;
        private StringBuilder condition = new StringBuilder();

        /// <summary>
        /// R01
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.txtMdivision.Text = Env.User.Keyword;
            this.txtfactory.Text = Env.User.Factory;
            this.dateSCIDelivery.Value1 = DateTime.Now.AddDays(-DateTime.Now.Day + 1);
            this.dateSCIDelivery.Value2 = DateTime.Now.AddMonths(1).AddDays(-DateTime.Now.AddMonths(1).Day);
        }

        /// <summary>
        /// ValidateInput
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(this.dateSCIDelivery.Value1))
            {
                MyUtility.Msg.WarningBox(" < SCI Delivery > can't be empty!!");
                return false;
            }

            #region -- 擇一必輸的條件 --
            this.sciDelivery1 = this.dateSCIDelivery.Value1;
            this.sciDelivery2 = this.dateSCIDelivery.Value2;
            #endregion
            this.mdivision = this.txtMdivision.Text;
            this.factory = this.txtfactory.Text;
            this.category = this.comboCategory.SelectedValue.ToString();

            return base.ValidateInput();
        }

        /// <summary>
        /// OnAsyncDataLoad
        /// </summary>
        /// <param name="e">e</param>
        /// <returns>DualResult</returns>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            #region -- sql parameters declare --
            System.Data.SqlClient.SqlParameter sp_factory = new System.Data.SqlClient.SqlParameter
            {
                ParameterName = "@factory",
            };

            System.Data.SqlClient.SqlParameter sp_mdivision = new System.Data.SqlClient.SqlParameter
            {
                ParameterName = "@MDivision",
            };

            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
            #endregion

            StringBuilder sqlCmd = new StringBuilder();

            sqlCmd.Append(@"select * into #tmporders from orders o where 1 = 1");

            #region --- 條件組合  ---
            if (!MyUtility.Check.Empty(this.sciDelivery1))
            {
                sqlCmd.Append(string.Format(@" and o.SciDelivery >= '{0}'", Convert.ToDateTime(this.sciDelivery1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.sciDelivery2))
            {
                sqlCmd.Append(string.Format(@" and o.SciDelivery <= '{0}'", Convert.ToDateTime(this.sciDelivery2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.mdivision))
            {
                sqlCmd.Append(" and o.mdivisionid = @MDivision");
                sp_mdivision.Value = this.mdivision;
                cmds.Add(sp_mdivision);
            }

            if (!MyUtility.Check.Empty(this.factory))
            {
                sqlCmd.Append(" and o.factoryid = @factory");
                sp_factory.Value = this.factory;
                cmds.Add(sp_factory);
            }

            sqlCmd.Append($" and o.Category in ({this.category})");
            #endregion
            this.condition.Clear();
            this.condition.Append(string.Format(@"SCI Delivery : {0} ~ {1}", Convert.ToDateTime(this.sciDelivery1).ToString("d"), Convert.ToDateTime(this.sciDelivery2).ToString("d")));
            sqlCmd.Append(@"
Select 
	[Factory Name] = o.FactoryID,
	[No of Styles] = count(o.styleid),
	[Order Allocation (Qty)] = sum(o.Qty),
	[Order Allocation (CPU)] = Sum(o.qty * o.cpu * o.CPUFactor)
into #m
from #tmporders o
group by o.FactoryID
order by o.FactoryID
-------------------
SELECT o.FactoryID,o.StyleID,min(o.SewInLine)omsl
into #a
FROM #tmporders o
group by o.FactoryID,o.StyleID 
select o2.FactoryID,o2.StyleID,min(o2.SewInLine) o2msl
into #b
from Orders o2 
WHERE SewInLine IS NOT NULL 
group by o2.FactoryID,o2.StyleID 
select
	 a.FactoryID,
	[New Styles] = count(a.StyleID)
into #m2
from #a a left join #b b on a.FactoryID = b.FactoryID and a.StyleID = b.StyleID
where b.o2msl is null or a.omsl <= b.o2msl
group by a.FactoryID
-------------------
select 
	[Factory Name],
	[No of Styles],
	[New Styles] = isnull([New Styles],0),
	[Order Allocation (Qty)],
	[Order Allocation (CPU)]
into #ltm
from #m m left join #m2 m2 on m.[Factory Name] = m2.FactoryID
-------------------
select 
o.FactoryID,
	[Artwork Type] = ot.ArtworkTypeID,
	[No. of Style] = count(o.StyleID),
	[Ttl. Order Qty] = sum(o.Qty),
	[Total PCS/ Stitch] = Sum(ot. qty),
	[Unit] = ot.ArtworkUnit,
	[Total TMS] = sum(ot.TMS)
into #lts
from #tmporders o
inner join dbo.Order_TmsCost ot on ot.ID = o.ID and ot.Price+ot.Qty+ot.TMS > 0
inner join dbo.ArtworkType a on a.id = ot.ArtworkTypeID and a.IsSubprocess = 1 
group by o.FactoryID,ot.ArtworkTypeID,ot.ArtworkUnit
-------------------
select * into #ltm2 
from #ltm ,(select distinct [Artwork Type] from #lts)a
-------------------
select
	[Factory Name],
	[No of Styles],
	[New Styles],
	[Order Allocation (Qty)],
	[Order Allocation (CPU)],
	m.[Artwork Type],
	[No. of Style],
	[Ttl. Order Qty],
	[Total PCS/ Stitch],
	[Unit],
	[Total TMS],
	[% Based on order allocation] = IIF([Order Allocation (Qty)] = 0,'0',format(convert(float,[Ttl. Order Qty])/convert(float,[Order Allocation (Qty)]),'P')),
	[% Based Subprocess allocation] = IIF(sum([Ttl. Order Qty])over(partition by m.[Artwork Type]) = 0,'0',format(convert(float,[Ttl. Order Qty])/convert(float,sum([Ttl. Order Qty])over(partition by m.[Artwork Type])),'P')),
	[o2] = 1
into #lu
from #ltm2 m left join #lts s on m.[Factory Name] = s.FactoryID and m.[Artwork Type] =s.[Artwork Type]
order by s.[Artwork Type],[Factory Name]
-------------------
select
	[Artwork Type],
	[No. of Style] = sum([No. of Style]),
	[Ttl. Order Qty] = sum([Ttl. Order Qty]),
	[Total PCS/ Stitch] = sum([Total PCS/ Stitch]),
	[Unit] = '',
	[Total TMS] = sum([Total TMS]),
	[% Based on order allocation] = '',
	[% Based Subprocess allocation] = ''
into #ltsr 
from #lts u
group by [Artwork Type]
-------------------
select 	[Factory Name],	  [No of Styles],	  [New Styles],     [Order Allocation (Qty)], [Order Allocation (CPU)],
        [Artwork Type],	  [No. of Style],	  [Ttl. Order Qty], [Total PCS/ Stitch],      [Unit],	   [Total TMS],
        [% Based on order allocation],        [% Based Subprocess allocation]
from
(
	select
		[Factory Name],	
		[No of Styles] = format([No of Styles],'#,0.'),
		[New Styles] = format([New Styles],'#,0.'),
		[Order Allocation (Qty)] = format([Order Allocation (Qty)],'#,0.'),
		[Order Allocation (CPU)] = format([Order Allocation (CPU)],'#,0.'),
		[Artwork Type],
		[No. of Style] = format([No. of Style],'#,0.'),
		[Ttl. Order Qty] = format([Ttl. Order Qty],'#,0.'),
		[Total PCS/ Stitch] = format([Total PCS/ Stitch],'#,0.'),
		[Unit],
		[Total TMS] = format([Total TMS],'#,0.'),
		[% Based on order allocation],	[% Based Subprocess allocation],	[o2]
	from(
		select 	[Factory Name],	  [No of Styles],	  [New Styles],     [Order Allocation (Qty)], [Order Allocation (CPU)],
                [Artwork Type],	  [No. of Style],	  [Ttl. Order Qty], [Total PCS/ Stitch],      [Unit],	   [Total TMS],
                [% Based on order allocation],        [% Based Subprocess allocation],            [o2]
		from #lu
		union all
		select *
		from
		(
			select
				[Factory Name] = 'Subtotal',
				[No of Styles] = sum(m.[No of Styles]),
				[New Styles] = sum(m.[New Styles]),
				[Order Allocation (Qty)] = sum(m.[Order Allocation (Qty)]),
				[Order Allocation (CPU)] = sum(m.[Order Allocation (CPU)])
			from #ltm  m
		)a,(select *,[o2] = 2 from #ltsr)b
	)u
	union all
	select *
	from
	(
	select
		[Factory Name] = 'Percentage',
		[No of Styles] = null,
		[New Styles] = format(convert(float,sum(m.[New Styles]))/convert(float,sum([No of Styles])),'P'),
		[Order Allocation (Qty)] = 'Ave CPU/Pc',
		[Order Allocation (CPU)] = format(convert(float,sum(m.[Order Allocation (CPU)]))/convert(float,sum([Order Allocation (Qty)])),'P')
	from #ltm  m
	)ba,(
		select 
			[Artwork Type],
			[No. of Style] = format(convert(float,[No. of Style])/convert(float,[No of Styles]),'P'),
			[Ttl. Order Qty] = format(convert(float,[Ttl. Order Qty])/convert(float,[Order Allocation (Qty)]),'P'),
			[Total PCS/ Stitch] = null,	[Unit] = null,	[Total TMS] = null,	[% Based on order allocation] = null,
            [% Based Subprocess allocation] = null, [o2] = 3
		from(
			select * from(
				select
					[Factory Name] = 'Subtotal',
					[No of Styles] = sum(m.[No of Styles]),
					[New Styles] = sum(m.[New Styles]),
					[Order Allocation (Qty)] = sum(m.[Order Allocation (Qty)]),
					[Order Allocation (CPU)] = sum(m.[Order Allocation (CPU)])
				from #ltm  m
			)a,(select * from #ltsr)b
		)c
	)bb
)al
order by [Artwork Type],[o2],[Factory Name]
drop table #tmporders,#m,#m2,#a,#b,#ltm,#lts,#lu,#ltsr,#ltm2");

            DBProxy.Current.DefaultTimeout = 1800;
            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), cmds, out this.printData);

            DBProxy.Current.DefaultTimeout = 0;
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Ict.Result.True;
        }

        /// <summary>
        /// OnToExcel
        /// </summary>
        /// <param name="report">report</param>
        /// <returns>bool</returns>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData.Rows.Count);

            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            if (this.printData.Columns.Count > 16384)
            {
                MyUtility.Msg.WarningBox("Columns of Data is over 16,384 in excel file, please narrow down range of condition.");
                return false;
            }

            if (this.printData.Rows.Count + 6 > 1048576)
            {
                MyUtility.Msg.WarningBox("Lines of Data is over 1,048,576 in excel file, please narrow down range of condition.");
                return false;
            }

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Planning_R01.xltx"); // 預先開啟excel app
            MyUtility.Excel.CopyToXls(this.printData, string.Empty, "Planning_R01.xltx", 4, false, null, objApp);      // 將datatable copy to excel
            objApp.Visible = false;
            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表

            Microsoft.Office.Interop.Excel.Range usedRange = objSheets.UsedRange;
            Microsoft.Office.Interop.Excel.Range rows = usedRange.Rows;
            int count = 0;

            foreach (Microsoft.Office.Interop.Excel.Range row in rows)
            {
                if (count > 0)
                {
                    Microsoft.Office.Interop.Excel.Range firstCell = row.Cells[1];
                    row.Borders.Color = Color.Black;
                    string firstCellValue = firstCell.Value as string;
                    if (firstCellValue == null)
                    {
                        continue;
                    }

                    if (firstCellValue.StrEndsWith("Subtotal"))
                    {
                        row.Font.Bold = true;
                    }

                    if (firstCellValue.StrEndsWith("Percentage"))
                    {
                        row.Font.Bold = true;
                        row.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].Weight = 4; // 上
                        row.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].Weight = 4; // 下
                        row.Borders.Color = Color.Black;
                    }

                    Marshal.ReleaseComObject(firstCell);
                }

                count++;
            }

            objSheets.Columns.AutoFit();
            objSheets.Cells[2, 1] = this.condition.ToString();   // 條件字串寫入excel
            objSheets.Cells[3, 2] = DateTime.Now.ToShortDateString();  // 列印日期寫入excel

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("Planning_R01");
            Microsoft.Office.Interop.Excel.Workbook workbook = objApp.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);
            Marshal.ReleaseComObject(objSheets);
            Marshal.ReleaseComObject(usedRange);
            Marshal.ReleaseComObject(rows);
            Marshal.ReleaseComObject(workbook);

            strExcelName.OpenFile();
            #endregion
            return true;
        }
    }
}
