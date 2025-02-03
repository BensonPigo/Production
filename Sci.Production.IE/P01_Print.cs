using Ict;
using Sci.Data;
using System;
using System.Data;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.IE
{
    /// <summary>
    /// IE_P01_Print
    /// </summary>
    public partial class P01_Print : Win.Tems.PrintForm
    {
        private DataRow masterData;
        private string artworktype;
        private string strLanguage;
        private string machineID;
        private bool nonSewing;
        private bool isPPA;
        private decimal efficiency;
        private DataTable printData;
        private DataTable dtcustcd;
        private DataTable artworkType;

        /// <summary>
        /// P01_Print
        /// </summary>
        /// <param name="masterData">masterData</param>
        public P01_Print(DataRow masterData)
        {
            this.InitializeComponent();
            this.EditMode = true;
            this.masterData = masterData;
            MyUtility.Tool.SetupCombox(this.comboLanguage, 2, 1, "en,English,cn,Chinese,vn,Vietnam,kh,Cambodia");
            this.comboLanguage.SelectedIndex = 0;
        }

        /// <summary>
        /// 驗證輸入條件
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(this.numEfficiencySetting.Value))
            {
                MyUtility.Msg.WarningBox("Efficiency setting can't empty!!");
                return false;
            }

            this.efficiency = MyUtility.Convert.GetInt(this.numEfficiencySetting.Value);
            string[] artworktypearry = this.textArtworkType.Text.Split(',');
            this.artworktype = "'" + artworktypearry.JoinToString("','") + "'";
            this.strLanguage = this.comboLanguage.SelectedValue.ToString();
            this.nonSewing = this.chkNonSewing.Checked;
            this.isPPA = this.chkPPA.Checked;

            return base.ValidateInput();
        }

        /// <summary>
        /// 非同步取資料
        /// </summary>
        /// <param name="e">e</param>
        /// <returns>DualResult</returns>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            string sqlCmd = string.Format(
                @"
	SELECT s.CdCodeID
        , s.CDCodeNew
		, ProductType = r2.Name
		, FabricType = r1.Name
		, s.Lining
		, s.Gender
		, Construction = d1.Name
	FROM Style s WITH(NOLOCK)
	left join DropDownList d1 WITH(NOLOCK) on d1.type= 'StyleConstruction' and d1.ID = s.Construction
	left join Reason r1 WITH(NOLOCK) on r1.ReasonTypeID= 'Fabric_Kind' and r1.ID = s.FabricType
	left join Reason r2 WITH(NOLOCK) on r2.ReasonTypeID= 'Style_Apparel_Type' and r2.ID = s.ApparelType
    where s.ID = '{0}' and s.SeasonID = '{1}' and s.BrandID = '{2}'",
                MyUtility.Convert.GetString(this.masterData["StyleID"]),
                MyUtility.Convert.GetString(this.masterData["SeasonID"]),
                MyUtility.Convert.GetString(this.masterData["BrandID"]));
            DBProxy.Current.Select(null, sqlCmd, out this.dtcustcd);

            sqlCmd = $@"
select Machine = stuff((
	select concat(', ', MachineTypeID, '*', cnt) 
	from (
		select td.MachineTypeID, cnt = COUNT(td.MachineTypeID)
		from TimeStudy_Detail td WITH (NOLOCK)
		left join MachineType m WITH (NOLOCK) on td.MachineTypeID = m.ID
		where td.ID = {this.masterData["ID"]} and td.MachineTypeID <> ''
        {(this.artworktype == "''" ? string.Empty : $" and m.ArtworkTypeID in({this.artworktype})")}
		group by MachineTypeID
	) a
	FOR XML PATH('')
),1,2,'')";
            this.machineID = MyUtility.GetValue.Lookup(sqlCmd);

            string ietmsUKEY = MyUtility.GetValue.Lookup($@"select i.Ukey from IETMS i WITH (NOLOCK) where  i.ID = '{this.masterData["IETMSID"]}' and i.Version='{this.masterData["IETMSversion"]}'");
            sqlCmd = $@"
--準備 SewingSeq , 同 P01 OnDetailSelectCommandPrepare 這段
SELECT
    td.Ukey
    ,SewingSeq = RIGHT('0000' + CAST(((ROW_NUMBER() OVER (ORDER BY td.[Seq])) ) AS VARCHAR(4)), 4)
INTO #tmpSewingSeq
FROM TimeStudy_Detail td
where ID = '{this.masterData["ID"]}'
and td.OperationID NOT LIKE '--%'
AND td.IsNonSewingLine = 0

select 
    seq = '0',
    SewingSeq = '0',
	OperationID = '--CUTTING',	
	MachineTypeID = null,
    PPA = '',
	Mold = null,
	Frequency = round(ProTMS, 4),
	SMV = round(ProTMS, 4),	
	PcsPerHour =IIF(ProTMS=0
                    ,0
                    ,round(3600/ProTMS, 1)
                ),
	Sewer=0,
	Annotation = null,	
	DescEN = null,
	[MasterPlusGroup]='',
    [Template] = ''
from[IETMS_Summary]
where location = '' and[IETMSUkey] = '{ietmsUKEY}' and ArtworkTypeID = 'Cutting'
and  ArtworkTypeID in ({this.artworktype})

union all
select 
    seq = '0',
    SewingSeq = '0',
	OperationID = 'PROCIPF00001',	
	MachineTypeID = 'CUT',
    PPA = '',
	Mold = null,
	Frequency = round(ProTMS, 4),
	SMV = round(ProTMS, 4),	
	PcsPerHour =IIF(ProTMS=0
                    ,0
                    ,round(3600/ProTMS, 1)
                ),
	Sewer=0,
	Annotation = 	null,
	DescEN = '**Cutting',
	[MasterPlusGroup]='',
    [Template] = ''
from[IETMS_Summary]
where location = '' and[IETMSUkey] = '{ietmsUKEY}' and ArtworkTypeID = 'Cutting'
and  ArtworkTypeID in ({this.artworktype})

union all
";
            sqlCmd += $@"
select DISTINCT
    td.Seq
    , SewingSeq = iif(td.SewingSeq = '' ,isnull(tmp.SewingSeq,''), td.SewingSeq)
    , td.OperationID
    , td.MachineTypeID
    , PPA = ISNULL(d.Name,'')
    , td.Mold
    , td.Frequency
    , td.SMV
    , td.PcsPerHour
    , td.Sewer
    , td.Annotation
    , [DescEN] = case when '{this.strLanguage}' = 'cn' then isnull(o.DescCH,o.DescEN)
                   when '{this.strLanguage}' = 'vn' then isnull(o.DescVN,o.DescEN)
                   when '{this.strLanguage}' = 'kh' then isnull(o.DescKH,o.DescEN)
     else o.DescEN end
    , o.MasterPlusGroup
    , td.Template
from TimeStudy_Detail td WITH (NOLOCK) 
left join #tmpSewingSeq tmp on tmp.ukey = td.ukey
left join Operation o WITH (NOLOCK) on td.OperationID = o.ID
left join MachineType m WITH (NOLOCK) on td.MachineTypeID = m.ID
left join MachineType_Detail md WITH (NOLOCK) on m.ID = md.ID 
left join DropDownList d (NOLOCK) on d.ID=td.PPA AND d.Type = 'PMS_IEPPA'
where td.ID = {MyUtility.Convert.GetString(this.masterData["ID"])}
and td.OperationID not like '--%'
{(this.artworktype == "''" ? string.Empty : $"and m.ArtworkTypeID in ({this.artworktype})")}
{(!this.nonSewing ? " and ISNULL(md.IsNonSewingLine ,0) != 1 " : string.Empty)}
{(!this.isPPA ? " and td.PPA  != 'C' " : string.Empty)}

union all
-- OperationID like '--%' 都要顯示, 不依據 artworktype
select DISTINCT
    td.Seq
    , SewingSeq = iif(td.SewingSeq = '' ,isnull(tmp.SewingSeq,''), td.SewingSeq)
    , td.OperationID
    , td.MachineTypeID
    , PPA =  ISNULL(d.Name,'')
    , td.Mold
    , td.Frequency
    , td.SMV
    , td.PcsPerHour
    , td.Sewer
    , td.Annotation
    , [DescEN] = case when '{this.strLanguage}' = 'cn' then isnull(o.DescCH,o.DescEN)
                   when '{this.strLanguage}' = 'vn' then isnull(o.DescVN,o.DescEN)
                   when '{this.strLanguage}' = 'kh' then isnull(o.DescKH,o.DescEN)
     else o.DescEN end
    , o.MasterPlusGroup
    , td.Template
from TimeStudy_Detail td WITH (NOLOCK) 
left join #tmpSewingSeq tmp on tmp.ukey = td.ukey
left join Operation o WITH (NOLOCK) on td.OperationID = o.ID
left join MachineType m WITH (NOLOCK) on td.MachineTypeID = m.ID
left join MachineType_Detail md WITH (NOLOCK) on m.ID = md.ID 
left join DropDownList d (NOLOCK) on d.ID=td.PPA AND d.Type = 'PMS_IEPPA'
where td.ID = {MyUtility.Convert.GetString(this.masterData["ID"])}
and td.OperationID like '--%'
{(!this.nonSewing ? " and ISNULL(md.IsNonSewingLine ,0) != 1 " : string.Empty)}
{(!this.isPPA ? " and td.PPA  != 'C' " : string.Empty)}

";

            sqlCmd += $@"
union all
select 
    seq = '9960',
    SewingSeq = '9960',
	OperationID = '--IPF',	
	MachineTypeID = null,
    PPA = '',
	Mold = null,
	Frequency = sum(round(ProTMS, 4)),
	SMV = sum(round(ProTMS, 4)),	
	PcsPerHour = sum(
	                IIF(ProTMS=0
	                ,0
	                ,round(3600/ProTMS, 1))	
	            ),
	Sewer=0,
	Annotation = null,	
	DescEN = null,
	[MasterPlusGroup]='',
    [Template] = ''
from [IETMS_Summary]
where location = '' and [IETMSUkey] = '{ietmsUKEY}' and ArtworkTypeID <> 'Cutting'
and  ArtworkTypeID in ({this.artworktype})


union all
select
    seq = '9970',
    SewingSeq = '9970',
	OperationID = 'PROCIPF00002',	
	MachineTypeID = 'M',
    PPA = '',
	Mold = null,
	Frequency = round(ProTMS, 4),
	SMV = round(ProTMS, 4),	
	PcsPerHour = IIF(ProTMS=0
                    ,0
                    ,round(3600/ProTMS, 1)
                ),
	Sewer=0,
	Annotation = null,
	DescEN = '**Inspection',
	[MasterPlusGroup]='',
    [Template] = ''
from [IETMS_Summary]
where location = '' and [IETMSUkey] = '{ietmsUKEY}' and ArtworkTypeID = 'Inspection'
and  ArtworkTypeID in ({this.artworktype})

union all
select 
    seq = '9980',
    SewingSeq = '9980',
	OperationID = 'PROCIPF00004',	
	MachineTypeID = 'MM2',
    PPA = '',
	Mold = null,
	Frequency = round(ProTMS, 4),
	SMV = round(ProTMS, 4),	
	PcsPerHour = IIF(ProTMS=0
                    ,0
                    ,round(3600/ProTMS, 1)
                ),
	Sewer=0,
	Annotation = null,
	DescEN = '**Pressing',
	[MasterPlusGroup]='',
    [Template] = ''
from [IETMS_Summary]
where location = '' and [IETMSUkey] = '{ietmsUKEY}' and ArtworkTypeID = 'Pressing'
and  ArtworkTypeID in ({this.artworktype})

union all
select 	
    seq = '9990',
    SewingSeq = '9990',
	OperationID = 'PROCIPF00003',	
	MachineTypeID = 'MM2',
    PPA = '',
	Mold = null,
	Frequency = round(ProTMS, 4),
	SMV = round(ProTMS, 4),	
	PcsPerHour = IIF(ProTMS=0
                    ,0
                    ,round(3600/ProTMS, 1)
                ),
	Sewer=0,
	Annotation = null,
	DescEN =  '**Packing',
	[MasterPlusGroup]='',
    [Template] = ''
from [IETMS_Summary]
where location = '' and [IETMSUkey] = '{ietmsUKEY}' and ArtworkTypeID = 'Packing'
and  ArtworkTypeID in ({this.artworktype})
order by SewingSeq
";
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out this.printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            sqlCmd = string.Format(
                @"select isnull(m.ArtworkTypeID,'') as ArtworkTypeID,sum(td.SMV) as ttlSMV
from TimeStudy_Detail td WITH (NOLOCK) 
left join MachineType m WITH (NOLOCK) on td.MachineTypeID = m.ID
where td.ID = {0}{1}
group by isnull(m.ArtworkTypeID,'')",
                MyUtility.Convert.GetString(this.masterData["ID"]),
                this.artworktype == "''" ? string.Empty : $" and m.ArtworkTypeID in ({this.artworktype})");
            result = DBProxy.Current.Select(null, sqlCmd, out this.artworkType);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Ict.Result.True;
        }

        /// <summary>
        /// 產生Excel
        /// </summary>
        /// <param name="report">report</param>
        /// <returns>bool</returns>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            string chkdata = string.Empty;
            if (this.chkCutting.Checked || this.chkInspection.Checked || this.chkPacking.Checked || this.chkPressing.Checked)
            {
                if (this.chkCutting.Checked)
                {
                    if (this.printData.Select("OperationID = 'PROCIPF00001'").Length == 0)
                    {
                        chkdata += "Cutting,";
                    }
                }

                if (this.chkInspection.Checked)
                {
                    if (this.printData.Select("OperationID = 'PROCIPF00002'").Length == 0)
                    {
                        chkdata += "Inspection,";
                    }
                }

                if (this.chkPacking.Checked)
                {
                    if (this.printData.Select("OperationID = 'PROCIPF00003'").Length == 0)
                    {
                        chkdata += "Packing,";
                    }
                }

                if (this.chkPressing.Checked)
                {
                    if (this.printData.Select("OperationID = 'PROCIPF00004'").Length == 0)
                    {
                        chkdata += "Pressing,";
                    }
                }
            }

            if (chkdata.Length > 0)
            {
                MyUtility.Msg.WarningBox($"CIPF no have {chkdata} data");
                return false;
            }

            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData.Rows.Count);

            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            string strXltName = Env.Cfg.XltPathDir + "\\IE_P01_Print.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return false;
            }

            string cdcodeID = string.Empty;
            string cdCodeNew = string.Empty;
            string productType = string.Empty;
            string fabricType = string.Empty;
            string lining = string.Empty;
            string gender = string.Empty;
            string construction = string.Empty;
            if (this.dtcustcd.Rows.Count > 0)
            {
                cdcodeID = this.dtcustcd.Rows[0]["CdCodeID"].ToString();
                cdCodeNew = this.dtcustcd.Rows[0]["CDCodeNew"].ToString();
                productType = this.dtcustcd.Rows[0]["ProductType"].ToString();
                fabricType = this.dtcustcd.Rows[0]["FabricType"].ToString();
                lining = this.dtcustcd.Rows[0]["Lining"].ToString();
                gender = this.dtcustcd.Rows[0]["Gender"].ToString();
                construction = this.dtcustcd.Rows[0]["Construction"].ToString();
            }

            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];
            worksheet.Cells[1, 1] = string.Format("Factory GSD - {0}", MyUtility.Convert.GetString(this.masterData["Phase"]));
            worksheet.Cells[3, 2] = MyUtility.Convert.GetString(this.masterData["StyleID"]);
            worksheet.Cells[3, 5] = MyUtility.Convert.GetString(this.masterData["SeasonID"]);
            worksheet.Cells[3, 8] = cdcodeID;
            worksheet.Cells[2, 8] = cdCodeNew;
            worksheet.Cells[2, 9] = "Product Type:" + productType;
            worksheet.Cells[2, 10] = "Fabric Type:" + fabricType;
            worksheet.Cells[3, 9] = "Lining:" + lining;
            worksheet.Cells[3, 10] = "Gender:" + gender;
            worksheet.Cells[2, 12] = "Construction:" + construction;
            worksheet.Cells[3, 13] = Convert.ToDateTime(DateTime.Today).ToString("yyyy/MM/dd");
            worksheet.Cells[4, 15] = MyUtility.Convert.GetString(this.efficiency) + "%";
            worksheet.Columns[3].ColumnWidth = 18.4;

            // 填內容值
            int intRowsStart = 5;
            object[,] objArray = new object[1, 16];
            foreach (DataRow dr in this.printData.Rows)
            {
                objArray[0, 0] = intRowsStart - 4;
                objArray[0, 1] = dr["Seq"];
                objArray[0, 2] = dr["SewingSeq"];
                objArray[0, 3] = dr["OperationID"];
                objArray[0, 4] = dr["MachineTypeID"];
                objArray[0, 5] = dr["MasterPlusGroup"];
                objArray[0, 6] = dr["PPA"];
                objArray[0, 7] = dr["Mold"];
                objArray[0, 8] = dr["Template"];
                objArray[0, 9] = dr["DescEN"];
                objArray[0, 10] = dr["Annotation"];
                objArray[0, 11] = dr["Frequency"];
                objArray[0, 12] = dr["SMV"];
                objArray[0, 13] = dr["PcsPerHour"];
                objArray[0, 14] = dr["Sewer"];
                objArray[0, 15] = MyUtility.Math.Round(MyUtility.Convert.GetDecimal(dr["PcsPerHour"]) * (this.efficiency / 100), 1);

                worksheet.Range[string.Format("A{0}:P{0}", intRowsStart)].Value2 = objArray;
                intRowsStart++;
            }

            intRowsStart++;
            worksheet.Range[string.Format("A{0}:B{0}", intRowsStart)].Merge(Type.Missing);
            worksheet.Range[string.Format("A{0}:B{0}", intRowsStart)].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
            worksheet.Cells[intRowsStart, 1] = "Machine:";
            worksheet.Range[string.Format("C{0}:O{0}", intRowsStart)].Merge(Type.Missing);
            worksheet.Range[string.Format("C{0}:O{0}", intRowsStart)].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;

            // Machine:
            worksheet.Cells[intRowsStart, 3] = this.machineID;

            worksheet.Range[string.Format("A{0}:O{0}", intRowsStart)].Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom).Weight = 3; // 1: 虛線, 2:實線, 3:粗體線
            worksheet.Range[string.Format("A{0}:O{0}", intRowsStart)].Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom).LineStyle = 1;

            intRowsStart++;
            worksheet.Range[string.Format("A{0}:C{0}", intRowsStart)].Merge(Type.Missing);
            worksheet.Range[string.Format("A{0}:C{0}", intRowsStart)].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
            worksheet.Cells[intRowsStart, 1] = "Total Sewing Time/Pc:";
            worksheet.Cells[intRowsStart, 4] = MyUtility.Convert.GetString(this.masterData["TotalSewingTime"]);
            worksheet.Cells[intRowsStart, 5] = "Sec.";
            worksheet.Cells[intRowsStart, 7] = "Prepared by:";
            worksheet.Range[string.Format("H{0}:O{0}", intRowsStart)].Merge(Type.Missing);
            string cipfrow = string.Empty;
            if (this.chkCutting.Checked || this.chkInspection.Checked || this.chkPacking.Checked || this.chkPressing.Checked)
            {
                intRowsStart++;
                worksheet.Range[string.Format("A{0}:C{0}", intRowsStart)].Merge(Type.Missing);
                worksheet.Range[string.Format("A{0}:C{0}", intRowsStart)].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                string chk = string.Empty;
                decimal ttl = 0;
                if (this.chkCutting.Checked)
                {
                    if (this.printData.Select("OperationID = 'PROCIPF00001'").Length > 0)
                    {
                        chk += "Cutting,";
                        ttl += MyUtility.Convert.GetDecimal(this.printData.Select("OperationID = 'PROCIPF00001'")[0]["SMV"]);
                    }
                }

                if (this.chkInspection.Checked)
                {
                    if (this.printData.Select("OperationID = 'PROCIPF00002'").Length > 0)
                    {
                        chk += "Inspection,";
                        ttl += MyUtility.Convert.GetDecimal(this.printData.Select("OperationID = 'PROCIPF00002'")[0]["SMV"]);
                    }
                }

                if (this.chkPacking.Checked)
                {
                    if (this.printData.Select("OperationID = 'PROCIPF00003'").Length > 0)
                    {
                        chk += "Packing,";
                        ttl += MyUtility.Convert.GetDecimal(this.printData.Select("OperationID = 'PROCIPF00003'")[0]["SMV"]);
                    }
                }

                if (this.chkPressing.Checked)
                {
                    if (this.printData.Select("OperationID = 'PROCIPF00004'").Length > 0)
                    {
                        chk += "Pressing";
                        ttl += MyUtility.Convert.GetDecimal(this.printData.Select("OperationID = 'PROCIPF00004'")[0]["SMV"]);
                    }
                }

                worksheet.Cells[intRowsStart, 1] = $"Total Time/Pc(Include {chk}):";
                worksheet.Cells[intRowsStart, 4] = MyUtility.Convert.GetString(MyUtility.Convert.GetDecimal(this.masterData["TotalSewingTime"]) + ttl);
                worksheet.Cells[intRowsStart, 5] = "Sec.";
                worksheet.Range[string.Format("H{0}:N{0}", intRowsStart)].Merge(Type.Missing);
                cipfrow = $"A{intRowsStart}";
            }

            intRowsStart++;
            worksheet.Range[string.Format("A{0}:G{0}", intRowsStart)].Merge(Type.Missing);
            worksheet.Range[string.Format("A{0}:G{0}", intRowsStart)].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
            StringBuilder artwork = new StringBuilder();
            foreach (DataRow dr in this.artworkType.Rows)
            {
                artwork.Append(string.Format("{0}: {1} Sec\r\n", Convert.ToString(dr["ArtworkTypeID"]).PadRight(20, ' '), MyUtility.Convert.GetString(MyUtility.Math.Round(MyUtility.Convert.GetDecimal(dr["ttlSMV"]), 0))));
            }

            worksheet.Cells[intRowsStart, 1] = artwork.ToString();

            intRowsStart++;
            worksheet.Range[string.Format("A{0}:C{0}", intRowsStart)].Merge(Type.Missing);
            worksheet.Range[string.Format("A{0}:C{0}", intRowsStart)].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
            worksheet.Cells[intRowsStart, 1] = "Number of Sewer:";
            worksheet.Cells[intRowsStart, 4] = MyUtility.Convert.GetString(this.masterData["NumberSewer"]);
            worksheet.Cells[intRowsStart, 5] = "Sewer";
            worksheet.Cells[intRowsStart, 7] = "Approved by:";
            worksheet.Range[string.Format("H{0}:N{0}", intRowsStart)].Merge(Type.Missing);

            intRowsStart++;
            worksheet.Range[string.Format("A{0}:C{0}", intRowsStart)].Merge(Type.Missing);
            worksheet.Range[string.Format("A{0}:C{0}", intRowsStart)].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
            worksheet.Cells[intRowsStart, 1] = "100% Output/Hr:";

            // 避免分母為0的錯誤
            if (MyUtility.Convert.GetDecimal(this.masterData["TotalSewingTime"]) > 0 && MyUtility.Convert.GetDecimal(this.masterData["NumberSewer"]) > 0)
            {
                worksheet.Cells[intRowsStart, 4] = MyUtility.Math.Round(3600 / MyUtility.Convert.GetDecimal(this.masterData["TotalSewingTime"]) * MyUtility.Convert.GetDecimal(this.masterData["NumberSewer"]), 0);
            }

            worksheet.Cells[intRowsStart, 5] = "Pcs";

            intRowsStart++;
            worksheet.Range[string.Format("A{0}:C{0}", intRowsStart)].Merge(Type.Missing);
            worksheet.Range[string.Format("A{0}:C{0}", intRowsStart)].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
            worksheet.Cells[intRowsStart, 1] = string.Format("{0}%", MyUtility.Convert.GetString(this.efficiency));

            // 避免分母為0的錯誤
            if (MyUtility.Convert.GetDecimal(this.masterData["TotalSewingTime"]) > 0 && MyUtility.Convert.GetDecimal(this.masterData["NumberSewer"]) > 0)
            {
                worksheet.Cells[intRowsStart, 4] = MyUtility.Math.Round(3600 / MyUtility.Convert.GetDecimal(this.masterData["TotalSewingTime"]) * MyUtility.Convert.GetDecimal(this.masterData["NumberSewer"]) * this.efficiency / 100, 0);
            }

            worksheet.Cells[intRowsStart, 5] = "Pcs";

            intRowsStart++;
            worksheet.Range[string.Format("A{0}:C{0}", intRowsStart)].Merge(Type.Missing);
            worksheet.Range[string.Format("A{0}:C{0}", intRowsStart)].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
            worksheet.Cells[intRowsStart, 1] = "PCS/HR/Sewer:";

            // 避免分母為0的錯誤
            if (MyUtility.Convert.GetDecimal(this.masterData["TotalSewingTime"]) > 0 && MyUtility.Convert.GetDecimal(this.masterData["NumberSewer"]) > 0)
            {
                worksheet.Cells[intRowsStart, 4] = MyUtility.Math.Round(3600 / MyUtility.Convert.GetDecimal(this.masterData["TotalSewingTime"]), 2);
            }

            worksheet.Cells[intRowsStart, 5] = "Pcs";
            worksheet.Cells[intRowsStart, 7] = "Noted by:";
            worksheet.Range[string.Format("H{0}:N{0}", intRowsStart)].Merge(Type.Missing);

            excel.Cells.EntireRow.AutoFit();
            if (!MyUtility.Check.Empty(cipfrow))
            {
                worksheet.get_Range(cipfrow).RowHeight = 33;
            }

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("IE_P01_Print");
            Microsoft.Office.Interop.Excel.Workbook workbook = excel.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(workbook);
            Marshal.ReleaseComObject(worksheet);

            strExcelName.OpenFile();
            #endregion
            return true;
        }

        private void TextArtworkType_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sqlCmd = "select id from ArtworkType where Seq like '1%' and junk = 0";
            Win.Tools.SelectItem2 item = new Win.Tools.SelectItem2(sqlCmd, string.Empty, string.Empty, this.textArtworkType.Text);
            if (item.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            this.textArtworkType.Text = item.GetSelectedString();
        }
    }
}
