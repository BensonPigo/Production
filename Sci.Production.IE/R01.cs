﻿using System.ComponentModel;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;

namespace Sci.Production.IE
{
    /// <summary>
    /// IE_R01
    /// </summary>
    public partial class R01 : Win.Tems.PrintForm
    {
        private string factory;
        private string style;
        private string season;
        private string team;
        //private string inline1;
        //private string inline2;
        //private string sewingline1;
        //private string sewingline2;
        private bool bolSummary;
        private bool bolBalancing;
        private string phase;
        private bool latestVersion;

        private DataTable printData;
        private StringBuilder sqlCmd = new StringBuilder();

        /// <summary>
        /// R01
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.comboSewingTeam1.SetDataSource();
            MyUtility.Tool.SetupCombox(this.comboPhase, 1, 1, ",Initial,Prelim,Final");
        }

        // Factory
        private void TxtFactory_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sqlCmd = "select distinct FTYGroup from Factory WITH (NOLOCK) where Junk = 0 AND FTYGroup!=''";

            Win.Tools.SelectItem item = new Win.Tools.SelectItem(sqlCmd, "8", this.txtFactory.Text, "Factory");
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.txtFactory.Text = item.GetSelectedString();
        }

        // Style
        private void TxtStyle_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sqlCmd = "select distinct ID,BrandID,Description from Style WITH (NOLOCK) where Junk = 0 order by ID";

            Win.Tools.SelectItem item = new Win.Tools.SelectItem(sqlCmd, "16,10,50", this.txtStyle.Text, "Style#,Brand,Description")
            {
                Width = 800,
            };
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.txtStyle.Text = item.GetSelectedString();
        }

        // Season
        private void TxtSeason_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sqlCmd = "select distinct ID from Season WITH (NOLOCK) where Junk = 0 ORDER BY ID DESC";
            Win.Tools.SelectItem item = new Win.Tools.SelectItem(sqlCmd, "10", this.txtSeason.Text, "Season")
            {
                Width = 300,
            };
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.txtSeason.Text = item.GetSelectedString();
        }

        /// <summary>
        /// ValidateInput 驗證輸入條件
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ValidateInput()
        {
            if (!this.dateInlineDate.HasValue1 && !this.dateInlineDate.HasValue2 && !this.dateSewingDate.HasValue1 && !this.dateSewingDate.HasValue2)
            {
                MyUtility.Msg.InfoBox("Please input <Inline Date> or <Sewing Date> first!!");
                return false;
            }

            this.factory = this.txtFactory.Text;
            this.style = this.txtStyle.Text;
            this.season = this.txtSeason.Text;
            this.team = this.comboSewingTeam1.Text;
            this.bolSummary = this.radioSummary.Checked;
            this.bolBalancing = this.chkBalancing.Checked;
            this.phase = this.comboPhase.Text;
            this.latestVersion = this.chkLastVersion.Checked;

            return base.ValidateInput();
        }

        /// <summary>
        /// OnAsyncDataLoad 非同步取資料
        /// </summary>
        /// <param name="e">e</param>
        /// <returns>DualResult</returns>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            this.sqlCmd.Clear();

            if (this.bolSummary)
            {
                this.sqlCmd.Append(@"
select
	lm.FactoryID,
	lm.StyleID,
	lm.ComboType,
	lm.SeasonID,
    lm.Phase,
	lm.BrandID,
	lm.Version,
	lm.SewingLineID,
	lm.Team,
	lm.CurrentOperators,
	lm.StandardOutput,
	lm.TaktTime,
	lm.TotalGSD,
	lm.TotalCycle,
	IIF(lm.HighestCycle = 0,0,Round(3600.0/lm.HighestCycle,2)) as EOLR,
	IIF(lm.HighestCycle*lm.CurrentOperators = 0,0,CONVERT(DECIMAL,lm.TotalGSD)/lm.HighestCycle/lm.CurrentOperators) as Eff,
	EffTarget = EffTarget.Target / 100.0,
	IIF(lm.HighestCycle*lm.CurrentOperators = 0,0,CONVERT(DECIMAL,lm.TotalCycle)/lm.HighestCycle/lm.CurrentOperators) as LB,
	[LinebalancingTarget] = LinebalancingTarget.Target / 100.0,
	[NotHitTargetType] =  iif(lm.Version = 1, 
								(select TypeGroup from IEReasonLBRnotHit_1st where Ukey = lm.IEReasonLBRnotHit_1stUkey),
								(select STUFF ((
										select distinct CONCAT (',', a.TypeGroup) 
											from (
												select lbr.TypeGroup
												from LineMapping_Detail l2 WITH (NOLOCK)
												inner join IEReasonLBRNotHit_Detail lbr WITH (NOLOCK) on l2.IEReasonLBRNotHit_DetailUkey = lbr.Ukey and lbr.junk = 0
												where lm.ID = l2.ID
											) a 
											for xml path('')
									), 1, 1, ''))
	) ,
    [TotalNoofNotHitTarget] = iif(lm.Version = 1,0,(select cnt = iif(count(*) = 0, '', cast(count(*) as varchar))
                    from (
	                    select distinct l2.NO, l2.IEReasonLBRNotHit_DetailUkey
	                    from LineMapping_Detail l2 WITH (NOLOCK)
	                    where lm.ID = l2.ID
	                    and ISNULL(l2.IEReasonLBRNotHit_DetailUkey, '') <> ''
                    )a )),
	[NotHitTargetReason] =  iif(lm.Version = 1, 
								(select Name from IEReasonLBRnotHit_1st where Ukey = lm.IEReasonLBRnotHit_1stUkey),
								(select STUFF ((
									select distinct CONCAT (',', a.Name ) 
										from (
											select lbr.Name 
											from LineMapping_Detail l2 WITH (NOLOCK)
											inner join IEReasonLBRNotHit_Detail lbr WITH (NOLOCK) on l2.IEReasonLBRNotHit_DetailUkey = lbr.Ukey and lbr.junk = 0
											where lm.ID = l2.ID
										) a 
										for xml path('')
								), 1, 1, ''))
	) ,
	IIF(lm.TaktTime*lm.CurrentOperators = 0,0,CONVERT(DECIMAL,lm.TotalCycle)/lm.TaktTime/lm.CurrentOperators) as LLEF,
	IIF(lm.HighestCycle * lm.CurrentOperators = 0,0,(ROUND(3600.0/lm.HighestCycle, 2) * (select isnull(CPU,0) from Style WITH (NOLOCK) where lm.BrandID = BrandID and lm.StyleID = ID and lm.SeasonID = SeasonID)/lm.CurrentOperators)) as PPH,
	isnull((select Name from Pass1 WITH (NOLOCK) where ID = lm.AddName),'') as CreateBy,
    lm.AddDate,
	isnull((select Name from Pass1 WITH (NOLOCK) where ID = lm.EditName),'') as EditBy,
    lm.EditDate
into #tmp
from LineMapping lm WITH (NOLOCK) 
outer apply(
	select top 1 c.Target
	from factory f
	left join ChgOverTarget c on c.MDivisionID= f.MDivisionID and c.EffectiveDate < iif(lm.Editdate is null,lm.adddate,lm.Editdate) and c. Type ='EFF.'
	where f.id = lm.factoryid
	order by EffectiveDate desc
)EffTarget
outer apply(
	select top 1 c.Target
	from factory f
	left join ChgOverTarget c on c.MDivisionID= f.MDivisionID and c.EffectiveDate < iif(lm.Editdate is null,lm.adddate,lm.Editdate) and c. Type ='LBR'
	where f.id = lm.factoryid
	order by EffectiveDate desc
)LinebalancingTarget 
");
            }
            else
            {
                // Detail
                this.sqlCmd.Append(@"
select distinct
	lm.FactoryID,
	lm.StyleID,
	lm.ComboType,
	lm.SeasonID,
    lm.Phase,
	lm.BrandID,
	lm.Version,
	lmd.No,
	lmd.TotalCycle,
	lmdavg.avgTotalCycle,
	[diffCycle] = (lmdavg.avgTotalCycle - lmd.TotalCycle) / lmdavg.avgTotalCycle,
	lbr.name
into #tmp
from LineMapping lm WITH (NOLOCK) 
inner join LineMapping_Detail lmd WITH (NOLOCK) on lm.ID = lmd.ID
left join IEReasonLBRNotHit_Detail lbr WITH (NOLOCK) on lmd.IEReasonLBRNotHit_DetailUkey = lbr.Ukey and lbr.junk = 0
outer apply (
	select avgTotalCycle = avg(TotalCycle)
	from 
	(
		select ID, NO, TotalCycle
		from LineMapping_Detail
		where ID = lm.ID
		and No <> '' 
		group by ID, NO, TotalCycle
	) lmdavg
)lmdavg
outer apply(
	select top 1 c.Target
	from factory f
	left join ChgOverTarget c on c.MDivisionID= f.MDivisionID 
				--and lm.status = 'Confirmed' 
				--and c.EffectiveDate < lm.Editdate 
				and c. Type ='LBR'
	where f.id = lm.factoryid
	order by EffectiveDate desc
)LinebalancingTarget 
");
            }

            this.sqlCmd.Append("where 1 = 1");
            if (!this.bolSummary)
            {
                this.sqlCmd.Append(@" 
and lmd.IsHide = 0
");
            }

            if (this.bolBalancing)
            {
                if (this.bolSummary)
                {
                    this.sqlCmd.Append(@"
and IIF(lm.HighestCycle*lm.CurrentOperators = 0,0,CONVERT(DECIMAL,lm.TotalCycle)/lm.HighestCycle/lm.CurrentOperators) * 100 < LinebalancingTarget.Target
");
                }
                else
                {
                    this.sqlCmd.Append(@"
and (((lmdavg.avgTotalCycle - lmd.TotalCycle) / lmdavg.avgTotalCycle) * 100 >  (100 - LinebalancingTarget.Target) 
	or  ((lmdavg.avgTotalCycle - lmd.TotalCycle) / lmdavg.avgTotalCycle) * 100 < (100 - LinebalancingTarget.Target) * -1 )
");
                }
            }

            if (!MyUtility.Check.Empty(this.factory))
            {
                this.sqlCmd.Append(string.Format(" and lm.FactoryID = '{0}'", this.factory));
            }

            if (!MyUtility.Check.Empty(this.style))
            {
                this.sqlCmd.Append(string.Format(" and lm.StyleID = '{0}'", this.style));
            }

            if (!MyUtility.Check.Empty(this.season))
            {
                this.sqlCmd.Append(string.Format(" and lm.SeasonID = '{0}'", this.season));
            }

            if (!MyUtility.Check.Empty(this.team))
            {
                this.sqlCmd.Append(string.Format(" and lm.Team = '{0}'", this.team));
            }

            if (!MyUtility.Check.Empty(this.phase))
            {
                this.sqlCmd.Append(string.Format(" and lm.Phase = '{0}'", this.phase));
            }

            if (this.latestVersion)
            {
                this.sqlCmd.Append(@"
 and lm.Version = (
	select MAX(l.Version)
	from LineMapping l
	where l.StyleUKey = lm.StyleUKey
	and l.FactoryID = lm.FactoryID
	and l.Phase = lm.Phase
    and l.SewingLineID = lm.SewingLineID
	group by l.StyleUKey, l.FactoryID,l.Phase,l.SewingLineID
 )
");
            }

            this.sqlCmd.Append(@"select * from #tmp t");

            #region Inline & Sewing Date is not null

            if (this.dateSewingDate.Value1.HasValue || this.dateSewingDate.Value2.HasValue || this.dateInlineDate.Value1.HasValue || this.dateInlineDate.Value2.HasValue)
            {
                string dateQuery = string.Empty;
                if (!MyUtility.Check.Empty(this.dateInlineDate.Value1))
                {
                    dateQuery += string.Format("and '{0}' <= convert(varchar(10), ss.Inline, 120) ", this.dateInlineDate.Value1.Value.ToString("yyyy-MM-dd"));
                }

                if (!MyUtility.Check.Empty(this.dateInlineDate.Value2))
                {
                    dateQuery += string.Format("and convert(varchar(10), ss.Inline, 120) <= '{0}' ", this.dateInlineDate.Value2.Value.ToString("yyyy-MM-dd"));
                }

                if (!MyUtility.Check.Empty(this.dateSewingDate.Value1))
                {
                    dateQuery += $@" 
and (convert(date,ss.Inline) >= '{this.dateSewingDate.Value1.Value.ToString("yyyy/MM/dd")}' or ('{this.dateSewingDate.Value1.Value.ToString("yyyy/MM/dd")}' between convert(date,ss.Inline) and convert(date,ss.Offline)))";
                }

                if (!MyUtility.Check.Empty(this.dateSewingDate.Value2))
                {
                    dateQuery += $@" 
and (convert(date,ss.Offline) <= '{this.dateSewingDate.Value2.Value.ToString("yyyy/MM/dd")}' or ('{this.dateSewingDate.Value2.Value.ToString("yyyy/MM/dd")}' between convert(date,ss.Inline) and convert(date,ss.Offline)))";
                }

                this.sqlCmd.Append($@"
 where exists(
	select *
	from SewingSchedule ss
	join Orders o on ss.OrderID = o.ID
	where 1=1
	{dateQuery}
	and o.StyleID = t.StyleID and o.SeasonID = t.SeasonID and o.BrandID = t.BrandID
 )
");
            }
            #endregion

            return Ict.Result.True;
        }

        /// <summary>
        /// OnToExcel 產生Excel
        /// </summary>
        /// <param name="report">report</param>
        /// <returns>bool</returns>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            DualResult result = DBProxy.Current.Select(null, this.sqlCmd.ToString(), out this.printData);
            if (!result)
            {
                this.ShowErr(result);
                return false;
            }

            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData.Rows.Count);

            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            this.ShowWaitMessage("Starting EXCEL...");
            string fileName = this.bolSummary ? "IE_R01_Summary" : "IE_R01_Detail";
            string strXltName = Env.Cfg.XltPathDir + $"\\{fileName}.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return false;
            }

            MyUtility.Excel.CopyToXls(this.printData, string.Empty, fileName + ".xltx", 1, false, null, excel);
            excel.Cells.EntireColumn.AutoFit();
            excel.Cells.EntireRow.AutoFit();
            this.HideWaitMessage();

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName(fileName);
            Microsoft.Office.Interop.Excel.Workbook workbook = excel.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(workbook);

            strExcelName.OpenFile();
            #endregion
            return true;
        }

        private void TxtFactory_Validating(object sender, CancelEventArgs e)
        {
            DataTable factoryData;
            string fac = string.Empty;
            string sqlCmd = "select distinct FTYGroup from Factory WITH (NOLOCK) where Junk = 0 AND FTYGroup!=''";
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out factoryData);
            foreach (DataRow dr in factoryData.Rows)
            {
                fac = dr["FTYGroup"].ToString();
                if (this.txtFactory.Text == fac)
                {
                    return;
                }
            }

            if (this.txtFactory.Text == string.Empty)
            {
                this.txtFactory.Text = string.Empty;
                return;
            }

            if (this.txtFactory.Text != fac)
            {
                this.txtFactory.Text = string.Empty;
                MyUtility.Msg.WarningBox("This Factory is wrong!");
                return;
            }
        }

        private void TxtStyle_Validating(object sender, CancelEventArgs e)
        {
            DataTable styleData;
            string sty = string.Empty;
            string sqlCmd = "select distinct ID,BrandID,Description from Style WITH (NOLOCK) where Junk = 0 order by ID";
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out styleData);
            foreach (DataRow dr in styleData.Rows)
            {
                sty = dr["ID"].ToString();
                if (this.txtStyle.Text == sty)
                {
                    return;
                }
            }

            if (this.txtStyle.Text == string.Empty)
            {
                this.txtStyle.Text = string.Empty;
                return;
            }

            if (this.txtStyle.Text != sty)
            {
                this.txtStyle.Text = string.Empty;
                MyUtility.Msg.WarningBox("This Style# is wrong!");
                return;
            }
        }

        private void TxtSeason_Validating(object sender, CancelEventArgs e)
        {
            DataTable seasonData;
            string season = string.Empty;
            string sqlCmd = "select distinct ID from Season WITH (NOLOCK) where Junk = 0";
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out seasonData);
            foreach (DataRow dr in seasonData.Rows)
            {
                season = dr["ID"].ToString();
                if (this.txtSeason.Text == season)
                {
                    return;
                }
            }

            if (this.txtSeason.Text == string.Empty)
            {
                this.txtSeason.Text = string.Empty;
                return;
            }

            if (this.txtSeason.Text != season)
            {
                this.txtSeason.Text = string.Empty;
                MyUtility.Msg.WarningBox("This Season is wrong!");
                return;
            }
        }
    }
}
