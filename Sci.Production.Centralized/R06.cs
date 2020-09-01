using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using Ict;
using Microsoft.Office.Interop.Excel;
using Sci.Data;
using Sci.Production.Prg;
using Sci.Win;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Centralized
{
    /// <summary>
    /// R06
    /// </summary>
    public partial class R06 : Sci.Win.Tems.PrintForm
    {
        private System.Data.DataTable[] printData;
        private DataSet dsAllData;
        private DateTime? date1;
        private DateTime? date2;
        private string mDivision;
        private string factory;
        private string brand;

        /// <summary>
        /// R06
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R06(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.comboM.Text = Env.User.Keyword;
        }

        /// <summary>
        /// OnFormLoaded
        /// </summary>
        protected override void OnFormLoaded()
        {
            #region load combobox Location M 預設顯示登入的M
            this.comboM.SetDefalutIndex();
            this.comboM.Text = Env.User.Keyword;
            #endregion

            #region load combobox Factory 預設顯示空白
            this.comboFactory.SetDefalutIndex(string.Empty);
            #endregion

            base.OnFormLoaded();
        }

        /// <summary>
        /// ValidateInput
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ValidateInput()
        {
            this.date1 = this.dateOutPutDate.Value1;
            this.date2 = this.dateOutPutDate.Value2;
            this.mDivision = this.comboM.Text;
            this.factory = this.comboFactory.Text;
            this.brand = this.txtbrand.Text;
            if (MyUtility.Check.Empty(this.date1) || MyUtility.Check.Empty(this.date2))
            {
                MyUtility.Msg.WarningBox("Output Date cannot be empty!");
                return false;
            }

            return base.ValidateInput();
        }

        /// <summary>
        /// OnAsyncDataLoad
        /// </summary>
        /// <param name="e">ReportEventArgs</param>
        /// <returns>DualResult</returns>
        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            DBProxy.Current.DefaultTimeout = 1800;  // timeout時間改為30分鐘

            StringBuilder strWhere = new StringBuilder();
            #region Filter Where
            if (!MyUtility.Check.Empty(this.date1))
            {
                strWhere.Append(string.Format(" and s.OutputDate >= '{0}'" + Environment.NewLine, Convert.ToDateTime(this.date1).ToString("yyyyMMdd")));
            }

            if (!MyUtility.Check.Empty(this.date2))
            {
                strWhere.Append(string.Format(" and s.OutputDate <= '{0}'" + Environment.NewLine, Convert.ToDateTime(this.date2).ToString("yyyyMMdd")));
            }

            if (!MyUtility.Check.Empty(this.mDivision))
            {
                strWhere.Append(string.Format(" and s.MDivisionID = '{0}'" + Environment.NewLine, this.mDivision));
            }

            if (!MyUtility.Check.Empty(this.factory))
            {
                strWhere.Append(string.Format(" and s.FactoryID = '{0}'" + Environment.NewLine, this.factory));
            }

            if (!MyUtility.Check.Empty(this.brand))
            {
                strWhere.Append(string.Format(" and o.BrandID = '{0}' ", this.brand));
            }
            #endregion

            string sqlcmd = string.Empty;
            sqlcmd = $@"
select 
o.MDivisionID
,FactoryID = o.FtyGroup
,OrderID = o.ID
,o.StyleID
,OL.Location
,o.CPU
,ol.Rate
,QAQty = SUM(sd.QAQty)
,o.CustPONO
into #tmpBase1
from SewingOutput_Detail SD
Inner join SewingOutput S on SD.ID=S.ID
Inner join Order_Location OL on SD.orderID=OL.OrderID
Inner join Orders O on SD.OrderID=O.ID 
Where O.LocalOrder=1 and O.SubconInType = '2'--(找出外代工的訂單)
AND EXISTS(SELECT 1 FROM Order_TmsCost OT WHERE SD.OrderID=OT.ID)
{strWhere}
GROUP BY o.MDivisionID ,o.FtyGroup,o.ID,o.StyleID
,OL.Location,o.CPU,ol.Rate,o.CustPONO

select 
o.MDivisionID
,FactoryID = o.FtyGroup
,OrderID = o.ID
,o.StyleID
,OL.Location
,o.CPU
,ol.Rate
,QAQty = SUM(sd.QAQty)
,o.CustPONO
into #tmpBase2
from SewingOutput_Detail SD
Inner join SewingOutput S on SD.ID=S.ID
Inner join Order_Location OL on SD.orderID=OL.OrderID
Inner join Orders O on SD.OrderID=O.ID 
where SD.OrderID in (select distinct CustPONO from #tmpBase1)--(第一次搜尋得到的來源訂單Orders.CustPONO)
AND EXISTS(SELECT 1 FROM Order_TmsCost OT WHERE SD.OrderID=OT.ID)
and S.OutputDate between '{Convert.ToDateTime(this.date1).ToString("yyyyMMdd")}' and '{Convert.ToDateTime(this.date2).ToString("yyyyMMdd")}'
GROUP BY o.MDivisionID ,o.FtyGroup,o.ID,o.StyleID
,OL.Location,o.CPU,ol.Rate,o.CustPONO


select ID,Seq,ArtworkUnit,ProductionUnit
into #AT
from ArtworkType WITH (NOLOCK)
where Classify in ('I','A','P') and IsTtlTMS = 0 and Junk = 0


--準備台北資料(須排除這些)
select ps.ID
into #TPEtmp
from PO_Supp ps WITH (NOLOCK)
inner join PO_Supp_Detail psd WITH (NOLOCK) on ps.ID=psd.id and ps.SEQ1=psd.Seq1
inner join Fabric fb WITH (NOLOCK) on psd.SCIRefno = fb.SCIRefno 
inner join MtlType ml WITH (NOLOCK) on ml.id = fb.MtlTypeID
where 1=1 and ml.Junk =0 and psd.Junk=0 and fb.Junk =0
and ml.isThread=1 
and ps.SuppID <> 'FTY' and ps.Seq1 not Like '5%'

-- 跑回圈 coursor

declare @OrderID varchar(18)
declare @CustPONO varchar(18)

DECLARE SourceCursor cursor for
(
	select distinct OrderID,CustPONO from #tmpBase1
)

open SourceCursor

FETCH NEXT FROM SourceCursor into @OrderID,@CustPONO
while @@FETCH_STATUS = 0
BEGIN
	

	-----orderid & ArtworkTypeID & Seq
	select distinct t.CustPONo,ot.ID,ot.ArtworkTypeID,ot.Seq,ot.Qty,ot.Price,ot.TMS
	,t.MDivisionID
	,t.FactoryID
	,t.OrderID
	,t.StyleID
	,t.Location
	,t.CPU
	,t.Rate
	,t.QAQty
	into #idat
	from #tmpBase1 t
	inner join Order_TmsCost ot WITH (NOLOCK) on ot.id = t.OrderId
	inner join orders o with(nolock) on o.ID = t.OrderId
	inner join #AT A on A.ID = ot.ArtworkTypeID
	where  
	(
		(ot.ArtworkTypeID = 'SP_THREAD' 
			and not exists(select 1 from #TPEtmp t where t.ID = o.POID)
			AND (OT.Qty !=0 OR OT.TMS !=0 OR OT.Price != 0)
		)
		or ot.ArtworkTypeID <> 'SP_THREAD'
	)
	and t.OrderID = @OrderID

	select distinct t.CustPONo,ot.ID,ot.ArtworkTypeID,ot.Seq,ot.Qty,ot.Price,ot.TMS
	,t.MDivisionID
	,t.FactoryID
	,t.OrderID
	,t.StyleID
	,t.Location
	,t.CPU
	,t.Rate
	,t.QAQty
	into #idat2
	from #tmpBase2 t
	inner join Order_TmsCost ot WITH (NOLOCK) on ot.id = t.OrderId
	inner join orders o with(nolock) on o.ID = t.OrderId
	inner join #AT A on A.ID = ot.ArtworkTypeID
	where  
	(
		(ot.ArtworkTypeID = 'SP_THREAD' 
			and not exists(select 1 from #TPEtmp t where t.ID = o.POID)
			AND (OT.Qty !=0 OR OT.TMS !=0 OR OT.Price != 0)
		)
		or ot.ArtworkTypeID <> 'SP_THREAD'
	)
	and t.OrderID = @CustPONO

	select distinct a.* 
	into #idatFinal
	from #idat a
	full outer join #idat2 b
	on a.CustPONo = b.ID and a.ArtworkTypeID = b.ArtworkTypeID
	where (a.Qty != b.Qty or a.Price != b.Price or a.QAQty != b.QAQty OR B.OrderID IS NULL OR A.OrderID IS NULL)

	select ID,Seq
	,ArtworkType_Unit = concat(ID,iif(Unit='QTY','(Price)',iif(Unit = '','','('+Unit+')'))),Unit
	,ArtworkType_CPU = iif(Unit = 'TMS',concat(ID,'(CPU)'),'')
into #atall
from(
	Select a.ID,a.Seq,Unit = a.ArtworkUnit from #AT a where ArtworkUnit !='' AND ProductionUnit !=''
	and exists(select * from #idatFinal b where b.ArtworkTypeID = a.ID)
	UNION
	Select a.ID,a.Seq,a.ProductionUnit from #AT a where ArtworkUnit !='' AND ProductionUnit !=''
	and exists(select * from #idatFinal b where b.ArtworkTypeID = a.ID)
	UNION
	Select a.ID,a.Seq,a.ArtworkUnit from #AT a where ArtworkUnit !='' AND ProductionUnit =''
	and exists(select * from #idatFinal b where b.ArtworkTypeID = a.ID)
	UNION
	Select a.ID,a.Seq,a.ProductionUnit from #AT a where ArtworkUnit ='' AND ProductionUnit !=''
	and exists(select * from #idatFinal b where b.ArtworkTypeID = a.ID)
	UNION
	Select a.ID,a.Seq,'' from #AT a where ArtworkUnit ='' AND ProductionUnit =''
	and exists(select * from #idatFinal b where b.ArtworkTypeID = a.ID)
)a

select *
into #atall2
from(
	select a.ID,a.Seq,c=1,a.ArtworkType_Unit,a.Unit from #atall a
	UNION
	select a.ID,a.Seq,2,a.ArtworkType_CPU,iif(a.ArtworkType_CPU='','','CPU')from #atall a
	where a.ArtworkType_CPU !=''
)b

declare @columnsName nvarchar(max) = stuff((select concat(',[',ArtworkType_Unit,']') from #atall2 for xml path('')),1,1,'')
declare @NameZ nvarchar(max) = (select concat(',[',ArtworkType_Unit,']=isnull([',ArtworkType_Unit,'],0)')from #atall2 for xml path(''))

declare @Artwork nvarchar(max) = 
( 
	select concat(
		',[',ArtworkType_Unit,']=sum(isnull(Rate*[',ArtworkType_Unit,'],0)) over(partition by t.MDivisionID,t.FactoryID,t.OrderId,t.StyleID)'
		,iif(ArtworkType_CPU = '', ''
		, concat(',[',ArtworkType_CPU,']=sum(isnull(Rate*[',ArtworkType_CPU,'],0)) over(partition by t.MDivisionID,t.FactoryID,t.OrderId,t.StyleID)'))	
	)
	from #atall for xml path('')
)

declare @TTL_Artwork nvarchar(max) = 
(
	select concat(
		',[TTL_',ArtworkType_Unit,']=Round(sum(o.QAQty*Rate*[',ArtworkType_Unit,'])over(partition by t.MDivisionID,t.FactoryID,t.OrderId,t.StyleID),'
		,iif(Unit='QTY','4','3'),')'
		,iif(ArtworkType_CPU = '', ''
		, concat(',[TTL_',ArtworkType_CPU,']=Round(sum(o.QAQty*Rate*[',ArtworkType_CPU,'])over(partition by t.MDivisionID,t.FactoryID,t.OrderId,t.StyleID),'
		,iif(Unit='QTY','4','3'),')'))
	)from #atall for xml path('')
)

-----by orderid & all ArtworkTypeID
declare @lastSql nvarchar(max) =N'
select orderid,FactoryID,qaqty '+@NameZ+N'
into #oid_at
from
(
	select orderid = i.ID
	, a.ArtworkType_Unit
	, i.qaqty
	, ptq=iif(a.Unit=''QTY'',i.Price,iif(a.Unit=''TMS'',i.TMS,iif(a.Unit=''CPU'',i.Price,i.Qty)))
	, i.FactoryID
	from #atall2 a 
	left join #idat i on i.ArtworkTypeID = a.ID and i.Seq = a.Seq
)a
PIVOT(min(ptq) for ArtworkType_Unit in('+@columnsName+N'))as pt
where orderid is not null

select orderid,FactoryID,qaqty '+@NameZ+N'
into #oid_at2
from
(
	select orderid = i.ID
	, a.ArtworkType_Unit
	, i.qaqty
	, ptq=iif(a.Unit=''QTY'',i.Price,iif(a.Unit=''TMS'',i.TMS,iif(a.Unit=''CPU'',i.Price,i.Qty)))
	, i.FactoryID
	from #atall2 a 
	left join #idat2 i on i.ArtworkTypeID = a.ID and i.Seq = a.Seq
)a
PIVOT(min(ptq) for ArtworkType_Unit in('+@columnsName+N'))as pt
where orderid is not null

select orderid,FactoryID,qaqty '+@NameZ+N'
into #oid_atFinal
from
(
	select orderid = i.ID
	, a.ArtworkType_Unit
	, i.qaqty
	, ptq=iif(a.Unit=''QTY'',i.Price,iif(a.Unit=''TMS'',i.TMS,iif(a.Unit=''CPU'',i.Price,i.Qty)))
	, i.FactoryID
	from #atall2 a 
	left join #idatFinal i on i.ArtworkTypeID = a.ID and i.Seq = a.Seq
)a
PIVOT(min(ptq) for ArtworkType_Unit in('+@columnsName+N'))as pt
where orderid is not null
'
+N'
select * 
into #tmpFinal1
from
(
	select distinct
		 t.MDivisionID
		,t.FactoryID		
		,t.OrderID
		,t.CustPONo
		,t.StyleID
		,t.Location			
		,CPU = t.CPU * t.Rate
		'+@Artwork+N'
		,t.QAQty
		,TotalCPU = t.CPU * t.Rate * t.QAQty
		'+@TTL_Artwork+N'
from #idat t 
left join #oid_at o on o.orderid = t.OrderId and 
                           o.FactoryID = t.FactoryID
)a
order by MDivisionID,FactoryID,OrderId'

+N'
select * 
into #tmpFinal2
from
(
	select distinct
		 t.MDivisionID
		,t.FactoryID		
		,t.OrderID
		,t.CustPONo
		,t.StyleID
		,t.Location		
		,CPU = t.CPU * t.Rate
		'+@Artwork+N'
		,t.QAQty
		,TotalCPU = t.CPU * t.Rate * t.QAQty		
		'+@TTL_Artwork+N'
from #idat2 t 
left join #oid_at2 o on o.orderid = t.OrderId and 
                           o.FactoryID = t.FactoryID
)a
order by MDivisionID,FactoryID,OrderId

--select Final1_id = CustPoNo,* from #tmpFinal1
--select Final2_id = OrderID,* from #tmpFinal2


select * from (
select id = OrderID,* from #tmpFinal1
union all
select id = OrderID ,* from #tmpFinal2 
) a


drop table #oid_at, #oid_at2,#oid_atFinal,#tmpFinal1,#tmpFinal2

'

EXEC sp_executesql @lastSql


drop table #atall,#atall2,#idat,#idat2,#idatFinal

	FETCH NEXT FROM SourceCursor INTO @OrderID,@CustPONO
END

CLOSE SourceCursor
DEALLOCATE SourceCursor


drop table #tmpBase1,#tmpBase2,#TPEtmp,#AT



";

            #region --由 appconfig 抓各個連線路徑
            this.SetLoadingText("Load connections... ");
            XDocument docx = XDocument.Load(System.Windows.Forms.Application.ExecutablePath + ".config");
            string[] strSevers = ConfigurationManager.AppSettings["ServerMatchFactory"].Split(new char[] { ';' });
            List<string> connectionString = new List<string>(); // ←主要是要重組 List connectionString
            foreach (string ss in strSevers)
            {
                if (ss.IndexOf("testing_PMS", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    continue;
                }

                var connections = docx.Descendants("modules").Elements().Where(y => y.FirstAttribute.Value.Contains(ss.Split(new char[] { ':' })[0].ToString())).Descendants("connectionStrings").Elements().Where(x => x.FirstAttribute.Value.Contains("Production")).Select(z => z.LastAttribute.Value).ToList()[0].ToString();
                connectionString.Add(connections);
            }

            if (connectionString == null || connectionString.Count == 0)
            {
                return new DualResult(false, "no connection loaded.");
            }
            #endregion

            DualResult result = new DualResult(true);
            this.dsAllData = new DataSet();

            foreach (string conString in connectionString)
            {
                SqlConnection conn;
                using (conn = new SqlConnection(conString))
                {
                    conn.Open();
                    result = DBProxy.Current.SelectByConn(conn, sqlcmd.ToString(), null, out this.printData);
                    if (!result)
                    {
                        DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                        return failResult;
                    }

                    if (this.printData != null && this.printData.Length > 0)
                    {
                        int maxTable = this.dsAllData.Tables.Count;
                        foreach (var dt in this.printData)
                        {
                            maxTable++;
                            dt.TableName = maxTable.ToString();
                            this.dsAllData.Tables.Add(dt);
                        }
                    }
                }
            }

            // timeout時間改回5分鐘
            DBProxy.Current.DefaultTimeout = 300;
            return Ict.Result.True;
        }

        /// <summary>
        /// On To Excel
        /// </summary>
        /// <param name="report">report</param>
        /// <returns>bool</returns>
        protected override bool OnToExcel(ReportDefinition report)
        {
            if (this.dsAllData == null || this.dsAllData.Tables.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            this.SetCount(this.dsAllData.Tables.Count);

            this.ShowWaitMessage("Starting EXCEL...");
            string excelFile = "Centralized_R06.xltx";
            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + excelFile); // 開excelapp
            Microsoft.Office.Interop.Excel.Worksheet objSheets;   // 取得工作表
            for (int s = 1; s <= this.dsAllData.Tables.Count; s++)
            {
                objSheets = objApp.ActiveWorkbook.Worksheets[s];
                objApp.ActiveWorkbook.Sheets.Add(After: objSheets);
                objSheets.Select();
                System.Data.DataTable dt = this.dsAllData.Tables[s - 1];
                objSheets.Name = dt.Rows[0]["ID"].ToString();

                int start_columns = 1;

                // 移除多餘的欄位
                dt.Columns.Remove("ID");
                dt.Columns.Remove("CustPONO");

                // 動態調整各sheet欄位名稱
                for (int c = start_columns; c <= dt.Columns.Count; c++)
                {
                    objSheets.Cells[1, c] = dt.Columns[c - 1].ColumnName;
                }

                // 寫入資料
                int intRowsCount = dt.Rows.Count;
                int rownum = 2; // 每筆資料匯入之位置
                int intColumns = dt.Columns.Count; // 匯入欄位數
                object[,] objArray = new object[intRowsCount, intColumns]; // 每列匯入欄位區間
                for (int intIndex_Row = 0; intIndex_Row < intRowsCount; intIndex_Row++)
                {
                    for (int intIndex_C = 0; intIndex_C < intColumns; intIndex_C++)
                    {
                        objArray[0, intIndex_C] = dt.Rows[intIndex_Row][intIndex_C];
                    }

                    objSheets.Range[$"A{intIndex_Row + rownum}:{Sci.Production.Prg.PrivUtils.getPosition(intColumns)}{intIndex_Row + rownum}"].Value2 = objArray;
                }

                // 比較差異, 並新增一行資料填入差異數值
                for (int i = 1; i <= intColumns; i++)
                {
                    string statColumns_Name = MyUtility.Convert.GetString(((Microsoft.Office.Interop.Excel.Range)objSheets.Cells[1, i]).Text);
                    if (statColumns_Name == "TotalCPU")
                    {
                        objSheets.Cells[intRowsCount + 2, i] = "TTL Diff:";
                        for (int cc = i; cc < intColumns; cc++)
                        {
                            string c_Name = Sci.Production.Prg.PrivUtils.getPosition(cc + 1);
                            if (intRowsCount <= 1)
                            {
                                objSheets.Cells[intRowsCount + 2, cc + 1] = $"={c_Name}{2}";
                            }
                            else
                            {
                                objSheets.Cells[intRowsCount + 2, cc + 1] = $"={c_Name}{intRowsCount} - {c_Name}{intRowsCount + 1}";
                            }
                        }

                        break;
                    }
                }

                // 畫線
                objSheets.Range[$"A1:{PrivUtils.getPosition(intColumns)}1"].Font.Bold = true;
                objSheets.Range[$"A1:{PrivUtils.getPosition(intColumns)}1"].Interior.ColorIndex = 43; // 底色為綠色
                objSheets.Range[$"A1:{PrivUtils.getPosition(intColumns)}{intRowsCount + 2}"].Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                // 欄寬調整
                objSheets.Range[string.Format("A:{0}", PrivUtils.getPosition(intColumns))].WrapText = false;
                objSheets.get_Range(string.Format("A:{0}", PrivUtils.getPosition(intColumns))).EntireColumn.AutoFit();

                // 刪除不必要的column(相減為0或空值資料)
                for (int j = intColumns; j > 0; j--)
                {
                    string rowValue = MyUtility.Convert.GetString(((Microsoft.Office.Interop.Excel.Range)objSheets.Cells[intRowsCount + 2, j]).Text);
                    if (rowValue == "TotalCPU")
                    {
                        break;
                    }

                    if (rowValue == "0")
                    {
                        objSheets.Columns[j].Delete();
                    }
                }
            }

            #region Save & Show Excel

            string strExcelName = Class.MicrosoftFile.GetName("Centralized_R06");
            Workbook workbook = objApp.Workbooks[1];
            workbook.SaveAs(strExcelName);
            workbook.Close();
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);
            Marshal.ReleaseComObject(workbook);

            strExcelName.OpenFile();
            #endregion

            this.HideWaitMessage();
            return true;
        }
    }
}
