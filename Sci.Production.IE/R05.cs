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
using System.Data.SqlTypes;

namespace Sci.Production.IE
{
    /// <summary>
    /// IE_R05
    /// </summary>
    public partial class R05 : Win.Tems.PrintForm
    {
        private bool bolPPA;
        private bool bolIsHide;

        private string category;

        private bool bolFtyGSD;
        private bool bolLineMapping;
        private string reportType;

        private string ExcelType;

        private bool bolIncludeSeparted;

        private void SetExcelType()
        {
            this.ExcelType = $@"{this.reportType}_{this.category}";
        }

        private string date1;
        private string date2;
        private string factory;
        private string brand;
        private List<string> seasons;
        private bool version;
        private DataTable printData;

        /// <summary>
        /// R05
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R05(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.EditMode = true;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.txtfactory1.ReadOnly = true;
        }
        /// <summary>
        /// ValidateInput 驗證輸入條件
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ValidateInput()
        {
            if (!this.dateDate.HasValue1 || !this.dateDate.HasValue2)
            {
                MyUtility.Msg.InfoBox("Please input <Create/ Edit date> first!!");
                return false;
            }

            this.bolPPA = this.radioISPPA.Checked;
            this.bolIsHide = this.radioIsHide.Checked;
            this.bolFtyGSD = this.rdoFtyGSD.Checked;
            this.bolLineMapping = this.rdoLineMapping.Checked;
            if (this.radioISPPA.Checked)
            {
                this.category = "PPA";
            }

            if (this.radioIsHide.Checked)
            {
                this.category = "Hide";
            }

            if (this.rdoFtyGSD.Checked)
            {
                this.reportType = "FtyGSD";
            }

            if (this.rdoLineMapping.Checked)
            {
                this.reportType = "LineMapping";
            }

            this.SetExcelType();
            this.bolIncludeSeparted = this.chkIncludeSeparted.Checked;
            this.date1 = this.dateDate.Value1.Value.ToString("yyyyMMdd");
            this.date2 = this.dateDate.Value2.Value.ToString("yyyyMMdd");
            this.factory = this.txtfactory1.Text;
            this.brand = this.txtbrand1.Text;
            this.seasons = this.txtmultiSeason1.Text.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
            this.version = this.chkLatestVersion.Checked;

            return base.ValidateInput();
        }

        private string GetSQL()
        {
            StringBuilder sqlCmd = new StringBuilder();
            List<SqlParameter> paras = new List<SqlParameter>();
            paras.Add(new SqlParameter("@date1", this.date1));
            paras.Add(new SqlParameter("@date2", this.date2));

            if (this.ExcelType == "LineMapping_PPA" && !this.bolIncludeSeparted)
            {
                sqlCmd.Append($@"
select l.StyleID
	, l.SeasonID
	, l.BrandID
	, l.ComboType
	, l.Version
	, l.FactoryID
	, ld.OriNO
    , [DefaultPPA]=''
	, ld.No
    , [PPA] = IIF(ld.IsPPA=1 ,'Centralized' ,'Sewing line')
	, ld.MachineTypeID
	, o.MasterPlusGroup
	, ld.OperationID
	, o.DescEN
	, ld.Annotation
    , ld.GSD
    , ld.Cycle
	, l.Status
	, l.AddName
	, l.AddDate
	, l.EditName
	, l.EditDate
from LineMapping l WITH (NOLOCK)
inner join LineMapping_Detail ld WITH (NOLOCK) on l.ID =ld.ID and Left(ld.OperationID, 2) <> '--'
left join Operation o WITH (NOLOCK) on ld.OperationID = o.ID
outer apply (
	select [Version] = max(Version),FactoryID
	from LineMapping l2 WITH (NOLOCK) 
    where l.StyleUkey = l2.StyleUkey and l.FactoryID = l2.FactoryID
    group by FactoryID
)lMax
where (ld.IsPPA = 0 or ld.IsHide = 0)
and IsNull(l.EditDate, l.AddDate) between @date1 and @date2
");
            }
            else if (this.ExcelType == "LineMapping_PPA" && this.bolIncludeSeparted)
            {
                sqlCmd.Append($@"
select l.StyleID
	, l.SeasonID
	, l.BrandID
	, l.ComboType
	, l.Version
	, l.FactoryID
	, ld.OriNO
    , [DefaultPPA]=''
	, ld.No
    , [PPA] = IIF(ld.IsPPA=1 ,'Centralized' ,'Sewing line')
	, ld.MachineTypeID
	, o.MasterPlusGroup
	, ld.OperationID
	, o.DescEN
	, ld.Annotation
    , ld.GSD
    , ld.Cycle
	, l.Status
	, l.AddName
	, l.AddDate
	, l.EditName
	, l.EditDate
    , ld.IsPPA
from LineMapping l WITH (NOLOCK)
inner join LineMapping_Detail ld WITH (NOLOCK) on l.ID =ld.ID and Left(ld.OperationID, 2) <> '--'
left join Operation o WITH (NOLOCK) on ld.OperationID = o.ID
outer apply (
	select [Version] = max(Version),FactoryID
	from LineMapping l2 WITH (NOLOCK) 
    where l.StyleUkey = l2.StyleUkey and l.FactoryID = l2.FactoryID
    group by FactoryID
)lMax
where IsNull(l.EditDate, l.AddDate) between @date1 and @date2
");
            }
            else if (this.ExcelType == "LineMapping_Hide" && !this.bolIncludeSeparted)
            {
                sqlCmd.Append($@"
select l.StyleID
	, l.SeasonID
	, l.BrandID
	, l.ComboType
	, l.Version
	, l.FactoryID
	, ld.OriNO
    , [DefaultHide] =  IIF(md.IsNonSewingLine=1 ,'Non-sewing line' ,'Subprocess')
	, ld.No
    , [Hide] = IIF(ld.IsHide=1 ,'Y' ,'N')
	, ld.MachineTypeID
	, o.MasterPlusGroup
	, ld.OperationID
	, o.DescEN
	, ld.Annotation
    , ld.GSD
    , ld.Cycle
	, l.Status
	, l.AddName
	, l.AddDate
	, l.EditName
	, l.EditDate
from LineMapping l WITH (NOLOCK)
inner join LineMapping_Detail ld WITH (NOLOCK) on l.ID =ld.ID and Left(ld.OperationID, 2) <> '--'
left join Operation o WITH (NOLOCK) on ld.OperationID = o.ID
left join MachineType m on m.ID = ld.MachineTypeID 
left join MachineType_Detail md on m.ID = md.ID and md.FactoryID = l.FactoryID
outer apply (
	select [Version] = max(Version),FactoryID
	from LineMapping l2 WITH (NOLOCK) 
    where l.StyleUkey = l2.StyleUkey and l.FactoryID = l2.FactoryID
    group by FactoryID
)lMax
where ld.MachineTypeID <> '' and ld.MachineTypeID is not null
and ld.IsHide = 1
and ld.IsPPA = 0
and IsNull(l.EditDate, l.AddDate) between @date1 and @date2
");
            }
            else if (this.ExcelType == "LineMapping_Hide" && this.bolIncludeSeparted)
            {
                sqlCmd.Append($@"
select l.StyleID
	, l.SeasonID
	, l.BrandID
	, l.ComboType
	, l.Version
	, l.FactoryID
	, ld.OriNO
    , [DefaultHide] =  IIF(md.IsNonSewingLine=1 ,'Non-sewing line' ,'Subprocess')
	, ld.No
    , [Hide] = IIF(ld.IsHide=1 ,'Y' ,'N')
	, ld.MachineTypeID
	, o.MasterPlusGroup
	, ld.OperationID
	, o.DescEN
	, ld.Annotation
    , ld.GSD
    , ld.Cycle
	, l.Status
	, l.AddName
	, l.AddDate
	, l.EditName
	, l.EditDate
    , md.IsNonSewingLine 
from LineMapping l WITH (NOLOCK)
inner join LineMapping_Detail ld WITH (NOLOCK) on l.ID =ld.ID and Left(ld.OperationID, 2) <> '--'
left join Operation o WITH (NOLOCK) on ld.OperationID = o.ID
left join MachineType m on m.ID = ld.MachineTypeID 
left join MachineType_Detail md on m.ID = md.ID and md.FactoryID = l.FactoryID
outer apply (
	select [Version] = max(Version),FactoryID
	from LineMapping l2 WITH (NOLOCK) 
    where l.StyleUkey = l2.StyleUkey and l.FactoryID = l2.FactoryID
    group by FactoryID
)lMax
where ld.MachineTypeID <> '' and ld.MachineTypeID is not null
and IsNull(l.EditDate, l.AddDate) between @date1 and @date2
");
            }
            else if (this.ExcelType == "FtyGSD_PPA" && !this.bolIncludeSeparted)
            {
                sqlCmd.Append($@"
select l.StyleID
	, l.SeasonID
	, l.BrandID
	, l.ComboType
    , l.Phase
	, l.Version
	, md.FactoryID
	, ld.Seq
	, ld.OperationID
	, o.DescEN
	, ld.Annotation
    , ld.Frequency
    , ld.SMV
    , [DefaultPPA] =  IIF(idd.IsPPA=1 , 'Centralized' ,'')
    , [PPA] =  IIF(idd.IsPPA=1 , 'Centralized' , 'Sewing line')
	, ld.MachineTypeID
	, o.MasterPlusGroup
	, [StdSMV] = round(  idd.SMV * ( isnull(idd.MtlFactorRate ,0) / 100 + 1 ) * idd.Frequency * 60  ,3)
	, l.Status
	, l.AddName
	, l.AddDate
	, l.EditName
	, l.EditDate
from TimeStudy l WITH (NOLOCK)
inner join TimeStudy_Detail ld WITH (NOLOCK) on l.ID =ld.ID and Left(ld.OperationID, 2) <> '--'

inner join Style s WITH (NOLOCK) on s.ID =l.StyleID and s.SeasonID = l.SeasonID and s.BrandID = l.BrandID
left join IETMS i WITH (NOLOCK) on s.IETMSID = i.ID and s.IETMSVersion = i.Version 
left join IETMS_Detail idd WITH (NOLOCK) on i.Ukey = idd.IETMSUkey and ld.Seq = idd.SEQ

left join Operation o WITH (NOLOCK) on ld.OperationID = o.ID and idd.OperationID = o.ID
left join MachineType m on m.ID = ld.MachineTypeID  and m.ID = o.MachineTypeID
left join MachineType_Detail md on m.ID = md.ID and md.FactoryID='{Sci.Env.User.Factory}'
outer apply (
	select [Version] = max(Version),StyleID,SeasonID,BrandID
	from TimeStudy l2 WITH (NOLOCK) 
    where l.StyleID = l2.StyleID and l.SeasonID = l2.SeasonID and l.BrandID = l2.BrandID
    group by StyleID,SeasonID,BrandID
)lMax
where ld.MachineTypeID <> '' and ld.MachineTypeID is not null
and IsNull(l.EditDate, l.AddDate) between @date1 and @date2
");
            }
            else if (this.ExcelType == "FtyGSD_PPA" && this.bolIncludeSeparted)
            {
                sqlCmd.Append($@"
select l.StyleID
	, l.SeasonID
	, l.BrandID
	, l.ComboType
    , l.Phase
	, l.Version
	, md.FactoryID
	, ld.Seq
	, ld.OperationID
	, o.DescEN
	, ld.Annotation
    , ld.Frequency
    , ld.SMV
    , [DefaultPPA] =  IIF(idd.IsPPA=1 , 'Centralized' ,'')
    , [PPA] =  IIF(idd.IsPPA=1 , 'Centralized' , 'Sewing line')
	, ld.MachineTypeID
	, o.MasterPlusGroup
	, [StdSMV] = round(  idd.SMV * ( isnull(idd.MtlFactorRate ,0) / 100 + 1 ) * idd.Frequency * 60  ,3)
	, l.Status
	, l.AddName
	, l.AddDate
	, l.EditName
	, l.EditDate
	, idd.IsPPA
from TimeStudy l WITH (NOLOCK)
inner join TimeStudy_Detail ld WITH (NOLOCK) on l.ID =ld.ID and Left(ld.OperationID, 2) <> '--'

inner join Style s WITH (NOLOCK) on s.ID =l.StyleID and s.SeasonID = l.SeasonID and s.BrandID = l.BrandID
left join IETMS i WITH (NOLOCK) on s.IETMSID = i.ID and s.IETMSVersion = i.Version 
left join IETMS_Detail idd WITH (NOLOCK) on i.Ukey = idd.IETMSUkey and ld.Seq = idd.SEQ

left join Operation o WITH (NOLOCK) on ld.OperationID = o.ID and idd.OperationID = o.ID
left join MachineType m on m.ID = ld.MachineTypeID  and m.ID = o.MachineTypeID
left join MachineType_Detail md on m.ID = md.ID and md.FactoryID='{Sci.Env.User.Factory}'
outer apply (
	select [Version] = max(Version),StyleID,SeasonID,BrandID
	from TimeStudy l2 WITH (NOLOCK) 
    where l.StyleID = l2.StyleID and l.SeasonID = l2.SeasonID and l.BrandID = l2.BrandID
    group by StyleID,SeasonID,BrandID
)lMax
where ld.MachineTypeID <> '' and ld.MachineTypeID is not null
and IsNull(l.EditDate, l.AddDate) between @date1 and @date2
");
            }
            else if (this.ExcelType == "FtyGSD_Hide" && !this.bolIncludeSeparted)
            {
                sqlCmd.Append($@"
select l.StyleID
	, l.SeasonID
	, l.BrandID
	, l.ComboType
    , l.Phase
	, l.Version
	, md.FactoryID
	, ld.Seq
	, ld.OperationID
	, o.DescEN
	, ld.Annotation
    , ld.Frequency
    , ld.SMV
	, [DefaultSubprocess] = IIF(md.IsSubprocess=1 , 'Y' ,'')
	, [Subprocess] = IIF(ld.IsSubprocess=1 , 'Y' ,'')
	, [DefaultNonSewingLine] = IIF(md.IsNonSewingLine=1 , 'Y' ,'')
	, NonSewingLine = IIF(md.IsNonSewingLine=1 , 'Y' ,'')
	, ld.MachineTypeID
	, o.MasterPlusGroup
	, [StdSMV] = round(  idd.SMV * ( isnull(idd.MtlFactorRate ,0) / 100 + 1 ) * idd.Frequency * 60  ,3)
	, l.Status
	, l.AddName
	, l.AddDate
	, l.EditName
	, l.EditDate
from TimeStudy l WITH (NOLOCK)
inner join TimeStudy_Detail ld WITH (NOLOCK) on l.ID =ld.ID and Left(ld.OperationID, 2) <> '--'
inner join Style s WITH (NOLOCK) on s.ID =l.StyleID and s.SeasonID = l.SeasonID and s.BrandID = l.BrandID
left join IETMS i WITH (NOLOCK) on s.IETMSID = i.ID and s.IETMSVersion = i.Version 
left join IETMS_Detail idd WITH (NOLOCK) on i.Ukey = idd.IETMSUkey and ld.Seq = idd.SEQ
left join Operation o WITH (NOLOCK) on ld.OperationID = o.ID and idd.OperationID = o.ID
left join MachineType m on m.ID = ld.MachineTypeID  and m.ID = o.MachineTypeID
left join MachineType_Detail md on m.ID = md.ID and md.FactoryID='{Sci.Env.User.Factory}'
outer apply (
	select [Version] = max(Version),StyleID,SeasonID,BrandID
	from TimeStudy l2 WITH (NOLOCK) 
    where l.StyleID = l2.StyleID and l.SeasonID = l2.SeasonID and l.BrandID = l2.BrandID
    group by StyleID,SeasonID,BrandID
)lMax
where ld.MachineTypeID <> '' and ld.MachineTypeID is not null
and IsNull(l.EditDate, l.AddDate) between @date1 and @date2
");
            }
            else if (this.ExcelType == "FtyGSD_Hide" && this.bolIncludeSeparted)
            {
                sqlCmd.Append($@"
select l.StyleID
	, l.SeasonID
	, l.BrandID
	, l.ComboType
    , l.Phase
	, l.Version
	, md.FactoryID
	, ld.Seq
	, ld.OperationID
	, o.DescEN
	, ld.Annotation
    , ld.Frequency
    , ld.SMV
	, [DefaultSubprocess] = IIF(md.IsSubprocess=1 , 'Y' ,'')
	, [Subprocess] = IIF(ld.IsSubprocess=1 , 'Y' ,'')
	, [DefaultNonSewingLine] = IIF(md.IsNonSewingLine=1 , 'Y' ,'')
	, NonSewingLine = IIF(md.IsNonSewingLine=1 , 'Y' ,'')
	, ld.MachineTypeID
	, o.MasterPlusGroup
	, [StdSMV] = round(  idd.SMV * ( isnull(idd.MtlFactorRate ,0) / 100 + 1 ) * idd.Frequency * 60  ,3)
	, l.Status
	, l.AddName
	, l.AddDate
	, l.EditName
	, l.EditDate
    , md.IsSubprocess
	, md.IsNonSewingLine 
from TimeStudy l WITH (NOLOCK)
inner join TimeStudy_Detail ld WITH (NOLOCK) on l.ID =ld.ID and Left(ld.OperationID, 2) <> '--'
inner join Style s WITH (NOLOCK) on s.ID =l.StyleID and s.SeasonID = l.SeasonID and s.BrandID = l.BrandID
left join IETMS i WITH (NOLOCK) on s.IETMSID = i.ID and s.IETMSVersion = i.Version 
left join IETMS_Detail idd WITH (NOLOCK) on i.Ukey = idd.IETMSUkey and ld.Seq = idd.SEQ
left join Operation o WITH (NOLOCK) on ld.OperationID = o.ID and idd.OperationID = o.ID
left join MachineType m on m.ID = ld.MachineTypeID  and m.ID = o.MachineTypeID
left join MachineType_Detail md on m.ID = md.ID and md.FactoryID='{Sci.Env.User.Factory}'
outer apply (
	select [Version] = max(Version),StyleID,SeasonID,BrandID
	from TimeStudy l2 WITH (NOLOCK) 
    where l.StyleID = l2.StyleID and l.SeasonID = l2.SeasonID and l.BrandID = l2.BrandID
    group by StyleID,SeasonID,BrandID
)lMax
where ld.MachineTypeID <> '' and ld.MachineTypeID is not null
and IsNull(l.EditDate, l.AddDate) between @date1 and @date2
");
            }

            if (!MyUtility.Check.Empty(this.factory) && this.reportType != "FtyGSD")
            {
                // FtyGSD的報表只能限定找登入者工廠的資料
                sqlCmd.Append("And l.FactoryID = @FactoryID" + Environment.NewLine);
            }

            if (!MyUtility.Check.Empty(this.brand))
            {
                sqlCmd.Append("And l.BrandID = @BrandID" + Environment.NewLine);
            }

            if (this.seasons.Count > 0)
            {
                sqlCmd.Append($"And l.SeasonID in ('{string.Join("','", this.seasons)}')" + Environment.NewLine);
            }

            if (this.version && this.reportType != "FtyGSD")
            {
                sqlCmd.Append("And l.Version = lMax.Version and l.FactoryID = lMax.FactoryID" + Environment.NewLine);
            }

            return sqlCmd.ToString();
        }

        /// <summary>
        /// OnAsyncDataLoad 非同步取資料
        /// </summary>
        /// <param name="e">e</param>
        /// <returns>DualResult</returns>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            List<SqlParameter> paras = new List<SqlParameter>();
            paras.Add(new SqlParameter("@date1", this.date1));
            paras.Add(new SqlParameter("@date2", this.date2));

            if (!MyUtility.Check.Empty(this.factory) && this.reportType != "FtyGSD")
            {
                // FtyGSD的報表只能限定找登入者工廠的資料
                paras.Add(new SqlParameter("@FactoryID", this.factory));

            }

            if (!MyUtility.Check.Empty(this.brand))
            {
                paras.Add(new SqlParameter("@BrandID", this.brand));
            }

            string sqlCmd = this.GetSQL();

            //            sqlCmd.Append($@"
            //select l.StyleID
            //	, l.SeasonID
            //	, l.BrandID
            //	, l.ComboType
            //	, l.Version
            //	, l.FactoryID
            //	, ld.OriNO
            //	, ld.No
            //	, ld.MachineTypeID
            //	, o.MasterPlusGroup
            //	, ld.OperationID
            //	, o.DescEN
            //	, ld.Annotation
            //	, [IsPPA] = iif(ld.IsPPA = 1, 'Y', '')
            //	, [IsHide] = iif(ld.IsHide = 1, 'Y', '')
            //	, l.Status
            //	, l.AddName
            //	, l.AddDate
            //	, l.EditName
            //	, l.EditDate
            //from LineMapping l WITH (NOLOCK)
            //inner join LineMapping_Detail ld WITH (NOLOCK) on l.ID =ld.ID and Left(ld.OperationID, 2) <> '--'
            //left join Operation o WITH (NOLOCK) on ld.OperationID = o.ID
            //outer apply (
            //	select [Version] = max(Version),FactoryID
            //	from LineMapping l2 WITH (NOLOCK) 
            //    where l.StyleUkey = l2.StyleUkey and l.FactoryID = l2.FactoryID
            //    group by FactoryID
            //)lMax
            //where IsNull(l.EditDate, l.AddDate) between @date1 and @date2" + Environment.NewLine);

            //            if (!MyUtility.Check.Empty(this.factory))
            //            {
            //                paras.Add(new SqlParameter("@FactoryID", this.factory));
            //                sqlCmd.Append("And l.FactoryID = @FactoryID" + Environment.NewLine);
            //            }

            //            if (!MyUtility.Check.Empty(this.brand))
            //            {
            //                paras.Add(new SqlParameter("@BrandID", this.brand));
            //                sqlCmd.Append("And l.BrandID = @BrandID" + Environment.NewLine);
            //            }

            //            if (this.seasons.Count > 0)
            //            {
            //                sqlCmd.Append($"And l.SeasonID in ('{string.Join("','", this.seasons)}')" + Environment.NewLine);
            //            }

            //            if (this.version)
            //            {
            //                sqlCmd.Append("And l.Version = lMax.Version and l.FactoryID = lMax.FactoryID" + Environment.NewLine);
            //            }

            //            if (this.bolPPA)
            //            {
            //                sqlCmd.Append("And ld.IsPPA = 1" + Environment.NewLine);
            //            }
            //            else
            //            {
            //                sqlCmd.Append("And ld.IsHide = 1" + Environment.NewLine);
            //            }

            //            sqlCmd.Append("Order by l.FactoryID, l.StyleID, l.BrandID, l.Version, ld.NO " + Environment.NewLine);

            DualResult result = DBProxy.Current.Select(null, sqlCmd, paras, out this.printData);
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
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData.Rows.Count);

            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            DataTable oriData;
            DataTable sheet1Data = new DataTable();
            DataTable sheet2Data = new DataTable();
            DataTable sheet3Data = new DataTable();
            this.ShowWaitMessage("Starting EXCEL...");
            string strXltName = Env.Cfg.XltPathDir;

            oriData = this.printData;
            if (this.ExcelType == "LineMapping_PPA" && !this.bolIncludeSeparted)
            {
                strXltName += "IE_R05_line Mapping_PPA_not include detail.xltx";
            }
            else if (this.ExcelType == "LineMapping_PPA" && this.bolIncludeSeparted)
            {
                // 砍掉 H欄 Default PPA、 I欄 No.、V欄
                strXltName += "IE_R05_line Mapping_PPA_include detail.xltx";
                sheet1Data = this.printData.Select("IsPPA  =  1 ").Any() ? this.printData.Select("IsPPA =  1 ").CopyToDataTable() : new DataTable();
                sheet2Data = this.printData.Select("IsPPA  =  0 ").Any() ? this.printData.Select("IsPPA =  0 ").CopyToDataTable() : new DataTable();

                if (this.printData.Select("IsPPA  =  1 ").Any())
                {
                    sheet1Data.Columns.Remove("DefaultPPA");
                    sheet1Data.Columns.Remove("No");
                    sheet1Data.Columns.Remove("IsPPA");
                }
                if (this.printData.Select("IsPPA  =  0 ").Any())
                {
                    sheet2Data.Columns.Remove("DefaultPPA");
                    sheet2Data.Columns.Remove("No");
                    sheet2Data.Columns.Remove("IsPPA");
                }
                oriData.Columns.Remove("IsPPA");
            }
            else if (this.ExcelType == "LineMapping_Hide" && !this.bolIncludeSeparted)
            {
                // 砍掉 H欄 Default PPA、 I欄 No.、V欄
                strXltName += "IE_R05_line Mapping_Hide_not include detail.xltx";
            }
            else if (this.ExcelType == "LineMapping_Hide" && this.bolIncludeSeparted)
            {
                strXltName += "IE_R05_line Mapping_Hide_include detail.xltx";
                // 砍掉 I欄 No.、V欄
                sheet1Data = this.printData.Select("IsNonSewingLine  =  0 ").Any() ? this.printData.Select("IsNonSewingLine =  0 ").CopyToDataTable() : new DataTable();
                sheet2Data = this.printData.Select("IsNonSewingLine  =  1 ").Any() ? this.printData.Select("IsNonSewingLine =  1 ").CopyToDataTable() : new DataTable();
                sheet3Data = this.printData.Select("IsNonSewingLine  IS NULL ").Any() ? this.printData.Select("IsNonSewingLine  IS NULL ").CopyToDataTable() : new DataTable();

                if (this.printData.Select("IsNonSewingLine  =  0 ").Any())
                {
                    sheet1Data.Columns.Remove("No");
                    sheet1Data.Columns.Remove("IsNonSewingLine");
                }
                if (this.printData.Select("IsNonSewingLine  =  1 ").Any())
                {
                    sheet2Data.Columns.Remove("No");
                    sheet2Data.Columns.Remove("IsNonSewingLine");
                }
                if (this.printData.Select("IsNonSewingLine  IS NULL ").Any())
                {
                    sheet3Data.Columns.Remove("No");
                    sheet3Data.Columns.Remove("IsNonSewingLine");
                }

                oriData.Columns.Remove("IsNonSewingLine");
            }
            else if (this.ExcelType == "FtyGSD_PPA" && !this.bolIncludeSeparted)
            {
                strXltName += "IE_R05_Fty GSD_PPA_not include detail.xltx";
            }
            else if (this.ExcelType == "FtyGSD_PPA" && this.bolIncludeSeparted)
            {
                strXltName += "IE_R05_Fty GSD_PPA_include detail.xltx";
                // sheet 1、2 砍掉 X欄
                sheet1Data = this.printData.Select("IsPPA = 1").Any() ? this.printData.Select("IsPPA =  1 ").CopyToDataTable() : new DataTable();
                sheet2Data = this.printData.Select("IsPPA = 0").Any() ? this.printData.Select("IsPPA =  0 ").CopyToDataTable() : new DataTable();

                if (this.printData.Select("IsPPA = 1").Any())
                {
                    sheet1Data.Columns.Remove("IsPPA");
                }
                if (this.printData.Select("IsPPA = 0").Any())
                {
                    sheet2Data.Columns.Remove("IsPPA");
                }
                oriData.Columns.Remove("IsPPA");
            }
            else if (this.ExcelType == "FtyGSD_Hide" && !this.bolIncludeSeparted)
            {
                strXltName += "IE_R05_Fty GSD_Hide_not include detail.xltx";
            }
            else if (this.ExcelType == "FtyGSD_Hide" && this.bolIncludeSeparted)
            {
                // sheet 2、3 砍掉 Z、AA欄
                strXltName += "IE_R05_Fty GSD_Hide_include detail.xltx";
                sheet1Data = this.printData.Select("IsSubprocess = 1 ").Any() ? this.printData.Select("IsSubprocess = 1 ").CopyToDataTable() : new DataTable();
                sheet2Data = this.printData.Select("IsNonSewingLine = 0 ").Any() ? this.printData.Select("IsNonSewingLine = 0").CopyToDataTable() : new DataTable();

                if (this.printData.Select("IsSubprocess = 1").Any())
                {
                    sheet1Data.Columns.Remove("IsSubprocess");
                    sheet1Data.Columns.Remove("IsNonSewingLine");
                }
                if (this.printData.Select("IsNonSewingLine = 0").Any())
                {
                    sheet2Data.Columns.Remove("IsSubprocess");
                    sheet2Data.Columns.Remove("IsNonSewingLine");
                }
                oriData.Columns.Remove("IsSubprocess");
                oriData.Columns.Remove("IsNonSewingLine");
            }

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(strXltName); // 預先開啟excel app
            if (objApp == null)
            {
                return false;
            }

            MyUtility.Excel.CopyToXls(oriData, string.Empty, "IE_R05.xltx", 1, false, null, objApp, wSheet: objApp.Sheets[1]); // 將datatable copy to excel

            if (sheet1Data.Rows != null && sheet1Data.Rows.Count > 0)
            {
                MyUtility.Excel.CopyToXls(sheet1Data, string.Empty, "IE_R05.xltx", 1, false, null, objApp, wSheet: objApp.Sheets[2]);
            }

            if (sheet2Data.Rows != null && sheet2Data.Rows.Count > 0)
            {
                MyUtility.Excel.CopyToXls(sheet2Data, string.Empty, "IE_R05.xltx", 1, false, null, objApp, wSheet: objApp.Sheets[3]);
            }

            if (sheet3Data.Rows != null && sheet3Data.Rows.Count > 0)
            {
                MyUtility.Excel.CopyToXls(sheet3Data, string.Empty, "IE_R05.xltx", 1, false, null, objApp, wSheet: objApp.Sheets[4]);
            }

            //MyUtility.Excel.CopyToXls(this.printData, string.Empty, "IE_R05.xltx", 1, false, null, objApp, wSheet: objApp.Sheets[1]); // 將datatable copy to excel
            objApp.Cells.EntireRow.AutoFit();
            objApp.Visible = true;
            Marshal.ReleaseComObject(objApp);
            this.HideWaitMessage();
            return true;
        }

        private void RdoFtyGSD_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rdoFtyGSD.Checked)
            {
                this.txtfactory1.ReadOnly = true;
                this.txtfactory1.Text = string.Empty;
                //this.txtfactory1.Text = Sci.Env.User.Factory;
            }
            else
            {
                this.txtfactory1.ReadOnly = false;
                //this.txtfactory1.Text = string.Empty;
            }
        }
    }
}
