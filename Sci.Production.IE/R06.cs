using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace Sci.Production.IE
{
    /// <summary>
    /// IE_R06
    /// </summary>
    public partial class R06 : Win.Tems.PrintForm
    {
        private bool bolDetail;
        private string date1;
        private string date2;
        private string style;
        private string factory;
        private string brand;
        private string artworkType;
        private List<string> seasons;
        private bool bolShowSheet;
        private bool version;
        private DataTable[] printData;

        /// <summary>
        /// R06
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R06(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.EditMode = true;
        }

        /// <summary>
        /// ValidateInput 驗證輸入條件
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ValidateInput()
        {
            if (!this.dateDate.HasValue1 || !this.dateDate.HasValue2)
            {
                MyUtility.Msg.InfoBox("Please input <Add/ Edit date> first!!");
                return false;
            }

            this.bolDetail = this.radioDetail.Checked;
            this.date1 = this.dateDate.Value1.Value.ToString("yyyyMMdd");
            this.date2 = this.dateDate.Value2.Value.ToString("yyyyMMdd");
            this.style = this.txtstyle.Text;
            this.factory = this.txtfactory1.Text;
            this.brand = this.txtbrand1.Text;
            this.seasons = this.txtmultiSeason1.Text.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
            this.artworkType = this.txtartworktype_fty1.Text;
            this.version = this.chkLatestVersion.Checked;
            this.bolShowSheet = this.chkShowSheet.Checked;

            return base.ValidateInput();
        }

        /// <summary>
        /// OnAsyncDataLoad 非同步取資料
        /// </summary>
        /// <param name="e">e</param>
        /// <returns>DualResult</returns>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            string whereArtworkType = string.Empty;
            List<SqlParameter> paras = new List<SqlParameter>();
            paras.Add(new SqlParameter("@date1", this.date1));
            paras.Add(new SqlParameter("@date2", this.date2));

            sqlCmd.Append(string.Format(
                $@"
declare @lastSql nvarchar(max) = ''

select t.*
into #tmp_TimeStudy
from TimeStudy t WITH (NOLOCK) 
outer apply (
	select [Version] = max(Version)
	from TimeStudy t2 WITH (NOLOCK) 
    where t.StyleID = t2.StyleID 
	and t.SeasonID = t2.SeasonID 
	and t.BrandID = t2.BrandID
)tMax
where (t.AddDate between @date1 and @date2
	or t.EditDate between @date1 and @date2)
" + Environment.NewLine));

            #region "where 條件"
            if (!MyUtility.Check.Empty(this.style))
            {
                paras.Add(new SqlParameter("@StyleID", this.style));
                sqlCmd.Append("And t.StyleID = @StyleID" + Environment.NewLine);
            }

            if (!MyUtility.Check.Empty(this.brand))
            {
                paras.Add(new SqlParameter("@BrandID", this.brand));
                sqlCmd.Append("And t.BrandID = @BrandID" + Environment.NewLine);
            }

            if (this.seasons.Count > 0)
            {
                sqlCmd.Append($"And t.SeasonID in ('{string.Join("','", this.seasons)}')" + Environment.NewLine);
            }

            if (this.version)
            {
                sqlCmd.Append("And t.Version = tMax.Version" + Environment.NewLine);
            }

            if (!MyUtility.Check.Empty(this.factory))
            {
                paras.Add(new SqlParameter("@FactoryID", this.factory));
                sqlCmd.Append(@"
and exists (select 1 
	from TimeStudy t2 WITH (NOLOCK) 
	left join LineMapping m WITH (NOLOCK) on m.StyleID = t2.StyleID 
						and m.SeasonID = t2.SeasonID 
						and m.BrandID = t2.BrandID 
						and m.ComboType = t2.ComboType 
						and m.TimeStudyVersion = t2.Version 
	where t2.ID = t.ID
	and isnull(m.FactoryID, @FactoryID) = @FactoryID
)" + Environment.NewLine);
            }

            if (!MyUtility.Check.Empty(this.artworkType))
            {
                paras.Add(new SqlParameter("@ArtworkTypeID", this.artworkType));
                whereArtworkType = "and m.ArtworkTypeID = @ArtworkTypeID";
                sqlCmd.Append(@"
and exists (
	select 1
	From TimeStudy_Detail td WITH (NOLOCK) 
	Left join Operation o WITH (NOLOCK) on o.id = td.OperationID
	Left join MachineType m WITH (NOLOCK) on m.id = o. MachineTypeID
	where td.ID = t.ID
	and m.ArtworkTypeID = @ArtworkTypeID
)" + Environment.NewLine);
            }
            #endregion

            if (this.bolDetail)
            {
                sqlCmd.Append(string.Format(
                    $@"
select td.ID
	, [ArtworkTypeID] = isnull('Fty GSD Artwork ' + m.ArtworkTypeID, '') 
	, [TMS] = sum(td.SMV)
into #tmp_ftyArtworkType
from TimeStudy_Detail td WITH (NOLOCK) 
left join MachineType m WITH (NOLOCK) on td.MachineTypeID = m.ID
where exists (select 1 from #tmp_TimeStudy t where t.ID = td.ID)
{whereArtworkType}
group by td.ID, m.ArtworkTypeID
having sum(td.SMV) > 0
order by m.ArtworkTypeID

select i.ID, i.Version
	, ArtworkTypeID = isnull('Std GSD Artwork ' + m.ArtworkTypeID, '')
	, SMV = Sum(round(id.SMV * (isnull(id.MtlFactorRate, 0) / 100 + 1) * id.Frequency * 60, 3))
into #tmp_GSDArtworkType
From IETMS i 
inner join IETMS_Detail id on i.Ukey = id.IETMSUkey
Left join Operation o on o.id = id.OperationID
Left join machineType m on m.id = o. MachineTypeID
where exists (select 1 from #tmp_TimeStudy t where t.IETMSID = i.ID and t.IETMSVersion = i.Version) 
{whereArtworkType}
group by i.ID, i.Version, m.ArtworkTypeID
having Sum(round(id.SMV * (isnull(id.MtlFactorRate, 0) / 100 + 1) * id.Frequency * 60, 3)) > 0

declare @columnsNameFTY nvarchar(max) = stuff((select distinct concat(',[',ArtworkTypeID,']') from #tmp_ftyArtworkType for xml path('')),1,1,'')
declare @NameFTY nvarchar(max) = (select distinct concat(',[',ArtworkTypeID,']')from #tmp_ftyArtworkType for xml path(''))
declare @columnsNameGSD nvarchar(max) = stuff((select distinct concat(',[',ArtworkTypeID,']') from #tmp_GSDArtworkType for xml path('')),1,1,'')
declare @NameGSD nvarchar(max) = (select distinct concat(',[',ArtworkTypeID,']')from #tmp_GSDArtworkType for xml path(''))

set @lastSql += N'
select ID'+@NameFTY+N'
into #tmp_ftyArtworkType_PIVOT
from #tmp_ftyArtworkType t
PIVOT(
	SUM(TMS) for ArtworkTypeID in(' + @columnsNameFTY + N')
)as pt

select ID,Version'+@NameGSD+N'
into #tmp_GSDArtworkType_PIVOT
from #tmp_GSDArtworkType t
PIVOT(
	SUM(SMV) for ArtworkTypeID in(' + @columnsNameGSD + N')
)as pt
'
" + Environment.NewLine));
            }

            sqlCmd.Append(string.Format(
                $@"
set @lastSql += N'
select [Factory] = Stuff((select distinct concat(''/'',m.FactoryID)
							from LineMapping m 
							where m.StyleID = t.StyleID 
							and m.SeasonID = t.SeasonID 
							and m.BrandID = t.BrandID 
							and m.ComboType = t.ComboType 
							and m.TimeStudyVersion  = t.Version 
						FOR XML PATH('''')) ,1,1,'''') 
	, [Brand] = t.BrandID
	, [Style] = t.StyleID
	, [ComboType] = t.ComboType
	, [Season] = t.SeasonID
	, [Phase] = t.Phase
	, [Version] = t.Version
	, [Fty GSD Total SMV (sec)] = FtyTotal.SMV
	{(this.bolDetail ? "'+@NameFTY+N'" : string.Empty)}
    , [Std GSD Act. Factory] = Stuff((select distinct concat(''/'', sd.Factory)
							from SMNotice_Detail sd
							where sd.ID = t.IETMSID
							and sd.Factory <> ''''
						FOR XML PATH('''')) ,1,1,'''') 
	, [Std GSD Apply No.] = t.IETMSID
	, [Std GSD Apply Version] = t.IETMSVersion
	, [Std GSD Total SMV (sec)] = STDTotal.SMV
    {(this.bolDetail ? "'+@NameGSD+N'" : string.Empty)}
	, t.AddName
	, [AddDate] = format(t.AddDate, ''yyyy/MM/dd HH:mm:ss'')
	, t.EditName
	, [EditDate] = format(t.EditDate, ''yyyy/MM/dd HH:mm:ss'')
from #tmp_TimeStudy t
{(this.bolDetail ?
@"
left join #tmp_ftyArtworkType_PIVOT f on t.id = f.id 
left join #tmp_GSDArtworkType_PIVOT g on t.IETMSID = g.ID and t.IETMSVersion = g.Version" : string.Empty)}

Outer apply (
	select SMV = SUM(td.SMV)
	from TimeStudy_Detail td 
	Left join Operation o on o.id = td.OperationID
	Left join MachineType m on m.id = o. MachineTypeID
	where td.ID = t.ID
)FtyTotal
Outer apply (
	select SMV = Sum(round(id.SMV * (isnull(id.MtlFactorRate, 0) / 100 + 1) * id.Frequency * 60, 3))
	From IETMS i 
	inner join IETMS_Detail id on id.IETMSUkey = i.ukey 
	Left join Operation o on o.id = id.OperationID
	Left join machineType m on m.id = o. MachineTypeID
	where exists (select 1 from #tmp_TimeStudy t2 where t.ID = t2.ID and t2.IETMSID = i.ID and t2.IETMSVersion = i.Version)
)STDTotal
'
--print @lastSql
EXEC sp_executesql @lastSql

"));

            if (this.bolShowSheet)
            {
                if (!this.bolDetail)
                {
                    sqlCmd.Append(string.Format(
                        $@"
select td.ID
	, [ArtworkTypeID] = isnull('Fty GSD Artwork ' + m.ArtworkTypeID, '') 
	, [TMS] = sum(td.SMV)
into #tmp_ftyArtworkType
from TimeStudy_Detail td WITH (NOLOCK) 
left join MachineType m WITH (NOLOCK) on td.MachineTypeID = m.ID
where exists (select 1 from #tmp_TimeStudy t where t.ID = td.ID)
{whereArtworkType}
group by td.ID, m.ArtworkTypeID
having sum(td.SMV) > 0
order by m.ArtworkTypeID

select i.ID, i.Version
	, [ArtworkTypeID] = isnull('Std GSD Artwork ' + m.ArtworkTypeID, '')
	, [SMV] = sum(round(id.SMV * (isnull(id.MtlFactorRate, 0) / 100 + 1) * id.Frequency * 60, 3))
into #tmp_GSDArtworkType
From IETMS i 
inner join IETMS_Detail id on i.Ukey = id.IETMSUkey
Left join Operation o on o.id = id.OperationID
Left join machineType m on m.id = o. MachineTypeID
where exists (select 1 from #tmp_TimeStudy t where t.IETMSID = i.ID and t.IETMSVersion = i.Version) 
{whereArtworkType}
group by i.ID, i.Version, m.ArtworkTypeID
having sum(round(id.SMV * (isnull(id.MtlFactorRate, 0) / 100 + 1) * id.Frequency * 60, 3)) > 0
" + Environment.NewLine));
                }

                sqlCmd.Append(
                    $@"
declare @ArtworkTypeIDloop as varchar(50) = ''
declare @loopSql nvarchar(max) = ''

declare _cursor Cursor For
select distinct
	[ArtworkTypeID] = REPLACE(m.ArtworkTypeID, 'Fty GSD Artwork ', '')
from #tmp_ftyArtworkType m
where 1=1
{whereArtworkType.Replace("@ArtworkTypeID", "'Fty GSD Artwork ' + @ArtworkTypeID")}
order by [ArtworkTypeID]

Open _cursor
Fetch Next From _cursor Into @ArtworkTypeIDloop
while (@@FETCH_STATUS = 0)
begin

	set @loopSql = N'
select ID, [Fty GSD Artwork '+@ArtworkTypeIDloop+N']
into #tmp_ftyArtworkType_PIVOT
from
(
	select *
	from #tmp_ftyArtworkType m
	where m.ArtworkTypeID = ''Fty GSD Artwork '+@ArtworkTypeIDloop+N'''
)m
PIVOT(
	SUM(TMS) for ArtworkTypeID in([Fty GSD Artwork ' + @ArtworkTypeIDloop + N'])
)as pt

select ID, Version, [Std GSD Artwork '+@ArtworkTypeIDloop+N']
into #tmp_GSDArtworkType_PIVOT
from 
(
	select *
	from #tmp_GSDArtworkType m
	where m.ArtworkTypeID = ''Std GSD Artwork '+@ArtworkTypeIDloop+N'''
)m
PIVOT(
	SUM(SMV) for ArtworkTypeID in([Std GSD Artwork ' + @ArtworkTypeIDloop + N'])
)as pt


select [Factory] = Stuff((select distinct concat(''/'',m.FactoryID)
							from LineMapping m 
							where m.StyleID = t.StyleID 
							and m.SeasonID = t.SeasonID 
							and m.BrandID = t.BrandID 
							and m.ComboType = t.ComboType 
							and m.TimeStudyVersion  = t.Version 
						FOR XML PATH('''')) ,1,1,'''') 
	, [Brand] = t.BrandID
	, [Style] = t.StyleID
	, [ComboType] = t.ComboType
	, [Season] = t.SeasonID
	, [Phase] = t.Phase
	, [Version] = t.Version
	, f.[Fty GSD Artwork '+@ArtworkTypeIDloop+N']
	, g.[Std GSD Artwork '+@ArtworkTypeIDloop+N']
from #tmp_TimeStudy t
left join #tmp_ftyArtworkType_PIVOT f on t.id = f.id
left join #tmp_GSDArtworkType_PIVOT g on t.IETMSID = g.ID and t.IETMSVersion = g.Version
where exists (
	select ArtworkTypeID
	From TimeStudy_Detail td
	Left join Operation o on o.id = td.OperationID
	Left join MachineType m on m.id = o. MachineTypeID
	where td.ID = t.ID
	and m.ArtworkTypeID = '''+@ArtworkTypeIDloop+'''
)'
		--print @loopSql
		EXEC sp_executesql @loopSql

	Fetch Next From _cursor Into @ArtworkTypeIDloop
end

close _cursor
Deallocate _cursor
" + Environment.NewLine);
            }

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), paras, out this.printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return result;
        }

        /// <summary>
        /// OnToExcel 產生Excel
        /// </summary>
        /// <param name="report">report</param>
        /// <returns>bool</returns>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            if (this.printData.Count() == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            if (this.printData[0].Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData[0].Rows.Count);

            this.ShowWaitMessage("Starting EXCEL...");
            string strXltName = Env.Cfg.XltPathDir + "\\IE_R06.xltx";
            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(strXltName); // 預先開啟excel app
            Microsoft.Office.Interop.Excel.Sheets exlSheets = objApp.Worksheets;
            Microsoft.Office.Interop.Excel.Worksheet workSheets;
            if (objApp == null)
            {
                return false;
            }

            for (int i = 0; i < this.printData.Count() - 1; i++)
            {
                exlSheets.Add();
            }

            workSheets = objApp.Sheets[1];
            for (int i = 0; i < this.printData[0].Columns.Count; i++)
            {
                workSheets.Cells[1, i + 1] = this.printData[0].Columns[i].ColumnName;
            }

            workSheets.Name = this.bolDetail ? "Detail" : "Summary";
            MyUtility.Excel.CopyToXls(this.printData[0], string.Empty, "IE_R06.xltx", 1, false, null, objApp, wSheet: workSheets);
            string r = MyUtility.Excel.ConvertNumericToExcelColumn(this.printData[0].Columns.Count); // EditDate
            workSheets.get_Range("A1", r + "1").Cells.Interior.Color = Color.LightGreen;
            workSheets.get_Range("A1", r + "1").AutoFilter(1);
            workSheets.get_Range("A1", r + "1").Font.Bold = true;
            workSheets.UsedRange.Columns[r].NumberFormat = "yyyy/MM/dd HH:mm:ss";
            r = MyUtility.Excel.ConvertNumericToExcelColumn(this.printData[0].Columns.Count - 2); // AddDate
            workSheets.UsedRange.Columns[r].NumberFormat = "yyyy/MM/dd HH:mm:ss";
            r = MyUtility.Excel.ConvertNumericToExcelColumn(3); // Style
            workSheets.UsedRange.Columns[r].NumberFormat = "@";
            r = MyUtility.Excel.ConvertNumericToExcelColumn(7); // Version
            workSheets.UsedRange.Columns[r].NumberFormat = "@";

            if (this.bolShowSheet && this.printData.Count() > 1)
            {
                for (int i = 1; i <= this.printData.Count() - 1; i++)
                {
                    workSheets = objApp.Sheets[i + 1];
                    for (int j = 0; j < this.printData[i].Columns.Count; j++)
                    {
                        workSheets.Cells[1, j + 1] = this.printData[i].Columns[j].ColumnName;
                    }

                    workSheets.Name = this.ReplaceExcelName(this.printData[i].Columns[this.printData[i].Columns.Count - 1].ColumnName.Replace("Std GSD Artwork ", string.Empty));
                    MyUtility.Excel.CopyToXls(this.printData[i], string.Empty, "IE_R06.xltx", 1, false, null, objApp, wSheet: workSheets);
                    string r2 = MyUtility.Excel.ConvertNumericToExcelColumn(this.printData[i].Columns.Count);
                    workSheets.get_Range("A1", r2 + "1").Cells.Interior.Color = Color.LightGreen;
                    workSheets.get_Range("A1", r2 + "1").AutoFilter(1);
                    workSheets.get_Range("A1", r2 + "1").Font.Bold = true;
                }
            }

            objApp.Cells.EntireRow.AutoFit();
            objApp.Visible = true;
            Marshal.ReleaseComObject(objApp);
            this.HideWaitMessage();
            return true;
        }

        private string ReplaceExcelName(string name)
        {
            return name.Replace(@"\", string.Empty).Replace(@"/", string.Empty).Replace(@"?", string.Empty).Replace(@"*", string.Empty).Replace(@"[", string.Empty).Replace(@"]", string.Empty);
        }
    }
}
