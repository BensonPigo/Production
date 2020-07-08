using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.SubProcess
{
    /// <summary>
    /// P10
    /// </summary>
    public partial class P10 : Sci.Win.Tems.QueryForm
    {
        private int intRightColumnHeaderCount = 3;
        private object objStartDate;
        private object objEndDate;
        private bool UpdateMultipleGetValue = true;
        private DataRow DrUpdateMutipleColumn;

        /// <summary>
        /// P10
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P10(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.gridLeft.IsEditingReadOnly = false;
            this.comboBoxUpdateColumn.SelectedIndex = 0;
        }

        private Ict.Win.UI.DataGridViewNumericBoxColumn setEarlyInlinecolor;
        private Ict.Win.UI.DataGridViewTextBoxColumn setPPAInlinecolor;

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            #region PPA Group
            DataGridViewGeneratorTextColumnSettings setPPAGroup = new DataGridViewGeneratorTextColumnSettings();
            setPPAGroup.EditingMouseDown += (s, e) =>
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    DataRow dr = this.gridLeft.GetDataRow<DataRow>(e.RowIndex);
                    this.PPAGroupMouseRightClick(dr, dr["Group"].ToString());
                }
            };

            setPPAGroup.CellValidating += (s, e) =>
            {
                DataRow dr = this.gridLeft.GetDataRow<DataRow>(e.RowIndex);
                string newGroup = e.FormattedValue.ToString();
                string oldGroup = dr["Group"].ToString();

                if (newGroup.EqualString(oldGroup) == false)
                {
                    this.PPAGroupValidating(dr, newGroup);
                }
            };
            #endregion

            #region PPA Line
            DataGridViewGeneratorTextColumnSettings setPPALine = new DataGridViewGeneratorTextColumnSettings();
            setPPALine.EditingMouseDown += (s, e) =>
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    DataRow dr = this.gridLeft.GetDataRow<DataRow>(e.RowIndex);
                    this.PPALineMouseRightClick(dr, dr["Group"].ToString());
                }
            };

            setPPALine.CellValidating += (s, e) =>
            {
                DataRow dr = this.gridLeft.GetDataRow<DataRow>(e.RowIndex);
                string newID = e.FormattedValue.ToString();
                this.PPALineValidating(dr, newID);
            };
            #endregion

            #region Target Qty
            DataGridViewGeneratorNumericColumnSettings setTargetQty = new DataGridViewGeneratorNumericColumnSettings();
            setTargetQty.CellValidating += (s, e) =>
            {
                DataRow dr = this.gridLeft.GetDataRow<DataRow>(e.RowIndex);
                object newTargetQty = e.FormattedValue;
                object oldTargetQty = dr["TargetQty"];
                this.TargetQtyValidating(dr, newTargetQty, oldTargetQty);

                object oldEarlyInline = dr["EarlyInline"];
                object oldInline = dr["Inline"];
                this.EarlyInlineValidating(dr, oldEarlyInline, oldEarlyInline, oldInline, true);
            };
            #endregion

            #region PPA Feature
            DataGridViewGeneratorTextColumnSettings setPPAFeature = new DataGridViewGeneratorTextColumnSettings();
            setPPAFeature.EditingMouseDown += (s, e) =>
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    DataRow dr = this.gridLeft.GetDataRow<DataRow>(e.RowIndex);
                    this.PPAFeatureMouseRightClick(dr);
                }
            };
            #endregion

            #region SMV
            DataGridViewGeneratorNumericColumnSettings setSMV = new DataGridViewGeneratorNumericColumnSettings();
            setSMV.CellValidating += (s, e) =>
            {
                DataRow dr = this.gridLeft.GetDataRow<DataRow>(e.RowIndex);
                object newSMV = e.FormattedValue;
                this.SMVValidating(dr, newSMV);
            };
            #endregion

            #region Early Inline
            DataGridViewGeneratorNumericColumnSettings setEarlyInline = new DataGridViewGeneratorNumericColumnSettings();
            setEarlyInline.CellValidating += (s, e) =>
            {
                DataRow dr = this.gridLeft.GetDataRow<DataRow>(e.RowIndex);
                object newEarlyInline = e.FormattedValue;
                object oldEarlyInline = dr["EarlyInline"];
                object oldInline = dr["Inline"];

                this.EarlyInlineValidating(dr, newEarlyInline, oldEarlyInline, oldInline, false);
            };
            #endregion

            #region Learn Curve
            DataGridViewGeneratorTextColumnSettings setLearnCurve = new DataGridViewGeneratorTextColumnSettings();
            setEarlyInline.IsSupportNegative = true;
            setLearnCurve.EditingMouseDown += (s, e) =>
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    DataRow dr = this.gridLeft.GetDataRow<DataRow>(e.RowIndex);
                    string strOldSubProcessLearnCurveID = dr["SubProcessLearnCurveID"].ToString();
                    this.LearnCurveMouseRightClick(dr, strOldSubProcessLearnCurveID);
                }
            };

            setLearnCurve.CellValidating += (s, e) =>
            {
                DataRow dr = this.gridLeft.GetDataRow<DataRow>(e.RowIndex);
                string strOldSubProcessLearnCurveID = dr["SubProcessLearnCurveID"].ToString();
                string strNewSubProcessLearnCurveID = e.FormattedValue.ToString();
                this.LearnCurveValidating(dr, strOldSubProcessLearnCurveID, strNewSubProcessLearnCurveID);
            };
            #endregion

            this.Helper.Controls.Grid.Generator(this.gridLeft)
                .CheckBox("Sel", header: "Sel", trueValue: 1, falseValue: 0)
                .Text("Group", header: $"PPA{Environment.NewLine}Group", settings: setPPAGroup)
                .Text("SubProcessLineID", header: $"PPA{Environment.NewLine}Line", settings: setPPALine)
                .Text("SewingLine", header: $"Sewing{Environment.NewLine}Line", iseditingreadonly: true)
                .Text("StyleID", header: "Style", iseditingreadonly: true)
                .Text("OrderID", header: "SP#", iseditingreadonly: true)
                .Numeric("OrderQty", header: $"Order{Environment.NewLine}Qty", iseditingreadonly: true)
                .Numeric("OutputQty", header: $"Output{Environment.NewLine}Qty", iseditingreadonly: true)
                .Numeric("BalanceQty", header: $"Balance{Environment.NewLine}Qty", iseditingreadonly: true)
                .Text("ShowSewInLine", header: $"Sewing{Environment.NewLine}Inline", iseditingreadonly: true)
                .Numeric("TargetQty", header: $"Target{Environment.NewLine}Qty", settings: setTargetQty)
                .EditText("Feature", header: $"PPA{Environment.NewLine}Feature", iseditingreadonly: true, settings: setPPAFeature)
                .Numeric("SMV", header: $"PPA{Environment.NewLine}SMV", integer_places: 3, decimal_places: 4, maximum: (decimal)999.9999, settings: setSMV)
                .Numeric("EarlyInline", header: $"Early{Environment.NewLine}Inline", settings: setEarlyInline).Get(out this.setEarlyInlinecolor)
                .Text("SubProcessLearnCurveID", header: $"Learn{Environment.NewLine}Curve", settings: setLearnCurve)
                .Text("ShowInline", header: $"PPA{Environment.NewLine}Inline", iseditingreadonly: true).Get(out this.setPPAInlinecolor)
                .Text("ShowOffline", header: $"PPA{Environment.NewLine}Offline", iseditingreadonly: true)
                .Numeric("Manpower", header: $"PPA{Environment.NewLine}Manpower", iseditingreadonly: true)
                .CheckBox("Overload", header: $"Over{Environment.NewLine}load", trueValue: 1, falseValue: 0);

            for (int i = 0; i < this.gridLeft.Columns.Count; i++)
            {
                this.gridLeft.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                this.gridLeft.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            this.gridLeft.Columns["Group"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridLeft.Columns["SubProcessLineID"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridLeft.Columns["TargetQty"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridLeft.Columns["SMV"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridLeft.Columns["SubProcessLearnCurveID"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridLeft.Columns["Feature"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridLeft.Columns["TargetQty"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridLeft.Columns["Overload"].DefaultCellStyle.BackColor = Color.Pink;
        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            #region SQL parameter
            if ((decimal)this.numericBoxWorkHour.Value == 0
                || (decimal)this.numericBoxEFF.Value == 0)
            {
                MyUtility.Msg.WarningBox("WorkHour & EFF can not be zero or empty.");
                return;
            }

            string strSewingInlineStart = this.dateRangeSewingInline.Value1.Empty() ? string.Empty : ((DateTime)this.dateRangeSewingInline.Value1).ToString("yyyy/MM/dd");
            string strSewingInlineEnd = this.dateRangeSewingInline.Value2.Empty() ? string.Empty : ((DateTime)this.dateRangeSewingInline.Value2).ToString("yyyy/MM/dd");
            string strSciDeliveryStart = this.dateRangeSCIDelivery.Value1.Empty() ? string.Empty : ((DateTime)this.dateRangeSCIDelivery.Value1).ToString("yyyy/MM/dd");
            string strSciDeliveryEnd = this.dateRangeSCIDelivery.Value2.Empty() ? string.Empty : ((DateTime)this.dateRangeSCIDelivery.Value2).ToString("yyyy/MM/dd");

            if ((strSewingInlineStart.Empty() || strSewingInlineEnd.Empty())
                && (strSciDeliveryStart.Empty() || strSciDeliveryEnd.Empty())
                && (this.txtSPStart.Text.Empty() || this.txtSPEnd.Text.Empty()))
            {
                MyUtility.Msg.WarningBox("SP# and Sewing Inline and SCI Delivery can not all be empty.");
                return;
            }

            List<SqlParameter> listSQLParameter = new List<SqlParameter>();
            listSQLParameter.Add(new SqlParameter("@FactoryID", this.txtfactory1.Text));
            listSQLParameter.Add(new SqlParameter("@WorkingHour", (decimal)this.numericBoxWorkHour.Value));
            listSQLParameter.Add(new SqlParameter("@EFF", (decimal)this.numericBoxEFF.Value));
            listSQLParameter.Add(new SqlParameter("@EntendDetailDay", (decimal)this.numericBoxExtendDetailDay.Value));
            listSQLParameter.Add(new SqlParameter("@SPStart", this.txtSPStart.Text));
            listSQLParameter.Add(new SqlParameter("@SPEnd", this.txtSPEnd.Text));
            listSQLParameter.Add(new SqlParameter("@SewingInlineStart", strSewingInlineStart));
            listSQLParameter.Add(new SqlParameter("@SewingInlineEnd", strSewingInlineEnd));
            listSQLParameter.Add(new SqlParameter("@SciDeliveryStart", strSciDeliveryStart));
            listSQLParameter.Add(new SqlParameter("@SciDeliveryEnd", strSciDeliveryEnd));
            listSQLParameter.Add(new SqlParameter("@Style", this.txtStyle.Text));
            listSQLParameter.Add(new SqlParameter("@MDivisionID ", Sci.Env.User.Keyword));

            List<string> listSQLFilter = new List<string>();
            string strHolidayDateFactory = string.Empty;
            string strOverload = string.Empty;

            if (!this.chkcontainOverload.Checked)
            {
                listSQLParameter.Add(new SqlParameter("@Overload", "0"));
                strOverload = "where isnull(Overload,0) = @Overload";
            }

            listSQLFilter.Add("and o.MDivisionID = @MDivisionID");

            if (this.txtSPStart.Text.Empty() == false
                && this.txtSPEnd.Text.Empty() == false)
            {
                listSQLFilter.Add("and o.ID between @SPStart and @SPEnd");
            }

            if (strSewingInlineStart.Empty() == false
                && strSewingInlineEnd.Empty() == false)
            {
                listSQLFilter.Add("and o.SewInLine between @SewingInlineStart and @SewingInlineEnd");
            }

            if (strSciDeliveryStart.Empty() == false
                && strSciDeliveryEnd.Empty() == false)
            {
                listSQLFilter.Add("and o.SciDelivery between @SciDeliveryStart and @SciDeliveryEnd");
            }

            if (this.txtStyle.Text.Empty() == false)
            {
                listSQLFilter.Add("and o.StyleID = @Style");
            }

            if (this.txtfactory1.Text.Empty() == false)
            {
                listSQLFilter.Add("and o.FactoryID = @FactoryID");
                strHolidayDateFactory = "where FactoryID = @FactoryID";
            }
            #endregion

            this.ShowWaitMessage("Data Loading...");

            #region SQL Command
            string strCmd = $@"
--declare @FactoryID varchar (100) = 'GMM'
--declare @WorkingHour int = '10'
--declare @EFF int = '10'
--declare @EntendDetailDay int = '10'

-- 確認有哪些 order 符合 --
select distinct OrderID = o.ID
into #OrderList
from orders o with(nolock)
inner join Style_Feature sf with(nolock) on sf.styleUkey = o.StyleUkey
where sf.Type = 'PPA'
      {listSQLFilter.JoinToString($"{Environment.NewLine}   ")}
-- 總表 --
select *
	   , Ukey = ROW_NUMBER() over (order by OrderID, ID)
into #InfoTable
from (
	select Sel = 0
		   , [Group] = ''
		   , SubProcessLineID = ''
		   , SewingLine = o.SewLine
		   , StyleID = o.StyleID
		   , OrderID = o.ID
		   , OrderQty = o.Qty
		   , OutputQty = OutputQty.value
		   , BalanceQty = o.Qty - OutputQty.value
		   , SewInLine = o.SewInLine
		   , TargetQty = 0
		   , Feature = SF.Feature
		   , SMV = SF.SMV
		   , EarlyInline = 5
		   , SubProcessLearnCurveID = ''
		   , Inline = dbo.[CalculateWorkDate](o.SewInLine, -5, @FactoryID)
		   , Offline = null
		   , Manpower = 0
		   , ID = null
           ,o.CutInLine
           ,ps.Overload
	from orders o with(nolock)
	inner join #OrderList ol on o.ID = ol.OrderID
	outer apply (
		select top 1 Feature
			   , SMV
		from Style_Feature sf with(nolock)
		where sf.styleUkey = o.StyleUkey
			  and sf.Type = 'PPA'
		order by SMV desc
	) SF
	outer apply (
        select value = isnull (Sum(spod.QaQty), 0)
        from SubProcessOutput_Detail spod with(nolock)
        left join SubProcessOutput spo with(nolock) on spod.ID = spo.ID
        where spod.OrderId = o.ID
	          and spo.Status = 'Confirmed'
	) OutputQty
	left join PPASchedule ps with(nolock) on o.ID = ps.OrderID
	where ps.OrderID is null 
	union all

	select Sel = 0
		   , [Group] = ps.[Group]
		   , SubProcessLineID = ps.SubProcessLineID
		   , SewingLine = o.SewLine
		   , StyleID = o.StyleID
		   , OrderID = o.ID
		   , OrderQty = o.Qty
           , OutputQty = OutputQty.value
		   , BalanceQty = o.Qty - OutputQty.value
		   , SewInLine = o.SewInLine
		   , TargetQty = ps.TargetQty
		   , Feature = ps.Feature
		   , SMV = ps.SMV
		   , EarlyInline = ps.EarlyInline
		   , SubProcessLearnCurveID = ps.SubProcessLearnCurveID
		   , Inline = ps.Inline
		   , Offline = ps.Offline
		   , Manpower = ps.TargetQty 
		   				* ps.SMV
		   				/ 60
		   				/ @WorkingHour
		   				/ @EFF
		   , ID = ps.ID	
           ,o.CutInLine
           ,ps.Overload
	from PPASchedule ps with(nolock)
    inner join #OrderList ol on ps.OrderID = ol.OrderID
	left join orders o with(nolock) on ol.OrderID = o.ID
    outer apply (
        select value = isnull (Sum(spod.QaQty), 0)
        from SubProcessOutput_Detail spod with(nolock)
        left join SubProcessOutput spo with(nolock) on spod.ID = spo.ID
        where spod.OrderId = o.ID
	          and spo.Status = 'Confirmed'
	) OutputQty
) tmp
{strOverload}

select *
	   , ShowSewInLine = Right (CONVERT(varchar, SewInLine, 111), 5)
	   , ShowInline = Right (CONVERT(varchar, Inline, 111), 5)
	   , ShowOffline = Right (CONVERT(varchar, Offline, 111), 5)
from #InfoTable
Order by Feature,SewInLine,orderid,id

if (select count(1) from #InfoTable) > 0
begin
	-- 取得 Inline & Offline 最大值與最小值 【+- @EntendDetailDay 天】 作為區間的頭尾 --
	Declare @MinDate Date;
	Declare @MaxDate Date;

	select @MaxDate = dbo.[CalculateWorkDate](Max ([SewInLine]), @EntendDetailDay, @FactoryID)
           , @MinDate = dbo.[CalculateWorkDate](Min ([SewInLine]), 0 - @EntendDetailDay, @FactoryID)
	from (
		select SewInLine
		from #InfoTable
	) tmp
	where [SewInLine] is not null

	-- 展開日期區間 【必須排除假日】
	create table #WorkDate (
		OutputDate varchar(10)
		, WDate varchar(6)
		, WeekNum int
	);

	Declare @StartDate date = @MinDate
	Declare @EndDate date = @MaxDate

	insert into #WorkDate 
		(OutputDate, WeekNum) 
	values 
		(convert(varchar, @StartDate, 111), DATEPART(WEEK, @StartDate))

	while (DATEADD(DAY, 1, @StartDate) <= @EndDate)
	begin
		set @StartDate = DATEADD(DAY, 1, @StartDate)

		if (DATEPART (WEEKDAY, @StartDate) != 1
			and @StartDate not in (select HolidayDate
								   from Holiday with(nolock)
								   {strHolidayDateFactory}))
		begin
			insert into #WorkDate 
				(OutputDate, WeekNum) 
			values 
				(convert(varchar, @StartDate, 111), DATEPART(WEEK, @StartDate))
		end
	end

	select * from #WorkDate

	-- 組成 OutputDate Table
	Declare @OutputDateList nvarchar(max) = null;
	Declare @UpdateOutputDataZero nvarchar(max) = null;
	Declare @DateSQLCommand nvarchar(max) = null;
	Declare @OutputWeek nvarchar(max) = null;

	select @OutputDateList =  stuff((select Concat(',[', OutputDate, ']')
									 from (
										select OutputDate 
										from #WorkDate
									 ) a 
									 for xml path(''))
									, 1, 1, '')

	select @OutputDateList

	select @UpdateOutputDataZero =  (select Concat(',[', OutputDate, '] = iif ([', OutputDate, '] = 0, null, [', OutputDate, '])')
									 from (
										select OutputDate 
										from #WorkDate
									 ) a 
									 for xml path(''))

	select @OutputWeek =  stuff((select Concat(',', WeekNum)
								 from (
									 select WeekNum 
									 from #WorkDate
								 ) a 
								 for xml path(''))
								 , 1, 1, '')

	set @DateSQLCommand = N'
	select OrderID
		   , ID
		   , Ukey
		   , feature
		   , SewInLine
		   ' + @UpdateOutputDataZero + '
	into #OutputDateTable
	from (
		select ift.OrderID
			   , ift.ID
			   , ift.Ukey
			   , wd.OutputDate
			   , Qty = isnull (psd.Qty, 0)
			   , feature
			   , SewInLine
		from #InfoTable ift
		outer apply (
			select *
			from #WorkDate
		) wd
		left join PPASchedule_Detail psd on ift.OrderID = psd.OrderID
											and wd.OutputDate = psd.OutputDate
	) tmp 
	pivot (
		sum (qty) for OutputDate in (' + @OutputDateList + ')
	) a

	insert into #OutputDateTable
		(OrderID,' + @OutputDateList +')
	values
		(''01'',' + @OutputWeek + ')

	insert into #OutputDateTable
		(OrderID)
	values
		(''02'')

	select OrderID
		   , ID
		   , Ukey
		   ' + @UpdateOutputDataZero + '
	from #OutputDateTable
	where OrderID not in (''01'', ''02'')
	order by feature, SewInLine,orderid,id,ukey

	select OrderID
		   , ID
		   , Ukey
		   ' + @UpdateOutputDataZero + '
	from #OutputDateTable
	where OrderID in (''01'', ''02'')
	'

	EXECUTE sp_executesql @DateSQLCommand

	select psd.OrderID
		   , psd.ID
		   , ift.Ukey
		   , OutputDate = convert(varchar, psd.OutputDate, 111)
		   , psd.Remark
	from #InfoTable ift
	inner join PPASchedule_Detail psd on ift.OrderID = psd.OrderID
										 and ift.ID = psd.ID
	where psd.OutputDate in (select OutputDate 
							 from #WorkDate)
		  and psd.Remark is not null
	order by feature, SewInLine,orderid,id
end
--drop table #OrderList, #InfoTable, #WorkDate";
            #endregion

            /*
             * dtGridData [0 - 4]
             * 0. Left Grid
             * 1. Date Range
             * 2. Date Range Join String
             * 3. Right Grid
             * 4. Top Grid
             * 5. Remark Data
             */
            DataTable[] dtGridData;
            DualResult result = DBProxy.Current.Select(null, strCmd, listSQLParameter, out dtGridData);

            if (result == false)
            {
                MyUtility.Msg.WarningBox(result.ToString());
            }
            else if (dtGridData.Length == 1)
            {
                this.listControlBindingSourceTop.DataSource = null;
                this.listControlBindingSourceLeft.DataSource = null;
                this.listControlBindingSourceDateRange.DataSource = null;
                this.listControlBindingSourceRight.DataSource = null;
                this.listControlBindingSourceRemark.DataSource = null;
                this.objStartDate = null;
                this.objEndDate = null;
                this.DrUpdateMutipleColumn = null;
                this.gridTop.Columns.Clear();
                this.gridRight.Columns.Clear();
                MyUtility.Msg.InfoBox("Data not found.");
            }
            else
            {
                this.DrUpdateMutipleColumn = dtGridData[0].Clone().NewRow();

                #region Set Left Grid
                this.listControlBindingSourceLeft.DataSource = dtGridData[0];
                #endregion

                // 動態設定 OutputDate Grid
                #region Set Right Grid
                dtGridData[3].PrimaryKey = new DataColumn[] { dtGridData[3].Columns["OrderID"] };

                this.gridRight.Columns.Clear();
                var gridRightGenerator = this.Helper.Controls.Grid.Generator(this.gridRight);
                foreach (DataRow dr in dtGridData[1].Rows)
                {
                    gridRightGenerator.Numeric(dr["OutputDate"].ToString(), header: Convert.ToDateTime(dr["OutputDate"]).ToString("MM/dd") + Environment.NewLine + dr["WeekNum"].ToString(), iseditingreadonly: true);
                }

                this.objStartDate = dtGridData[1].Rows[0]["OutputDate"];
                this.objEndDate = dtGridData[1].Rows[dtGridData[1].Rows.Count - 1]["OutputDate"];

                for (int i = 0; i < this.gridRight.Columns.Count; i++)
                {
                    this.gridRight.Columns[i].Width = 60;
                    this.gridRight.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                }

                this.listControlBindingSourceDateRange.DataSource = dtGridData[2];
                this.listControlBindingSourceRight.DataSource = dtGridData[3];
                this.gridRight.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
                this.gridRight.ClearSelection();

                this.gridRight.Rows[0].Selected = true;
                #endregion

                // 動態設定 Daily CPU Grid
                #region Set Top Grid
                dtGridData[4].PrimaryKey = new DataColumn[] { dtGridData[4].Columns["OrderID"] };

                this.gridTop.Columns.Clear();
                var gridTopGenerator = this.Helper.Controls.Grid.Generator(this.gridTop);
                foreach (DataRow dr in dtGridData[1].Rows)
                {
                    gridTopGenerator.Numeric(dr["OutputDate"].ToString(), header: Convert.ToDateTime(dr["OutputDate"]).ToString("MM/dd"), iseditingreadonly: true);
                }

                for (int i = 0; i < this.gridTop.Columns.Count; i++)
                {
                    this.gridTop.Columns[i].Width = 60;
                }

                this.listControlBindingSourceTop.DataSource = dtGridData[4];

                this.gridTop.CurrentCell = null;
                this.gridTop.Rows[0].Visible = false;
                #endregion

                #region Set complete color (Green)
                int ii = 0;
                foreach (DataRow dr in ((DataTable)this.listControlBindingSourceLeft.DataSource).Rows)
                {
                    if (MyUtility.Convert.GetInt(dr["OutputQty"]) > 0)
                    {
                        this.CompleteColor_Green(dr, ii);
                    }

                    ii++;
                }
                #endregion

                #region Set Remark DataSource
                dtGridData[5].PrimaryKey = new DataColumn[] { dtGridData[5].Columns["Ukey"], dtGridData[5].Columns["OutputDate"] };
                this.listControlBindingSourceRemark.DataSource = dtGridData[5];

                #region Set Remark Color

                // foreach (DataRow dr in ((DataTable)this.listControlBindingSourceRemark.DataSource).Rows)
                // {
                //    this.RemarkColorChange(dr);
                // }
                #endregion
                #endregion

                #region Compute Daily CPU
                for (int i = this.intRightColumnHeaderCount; i < dtGridData[3].Columns.Count; i++)
                {
                    this.ComputDailyCPU(i);
                }
                #endregion
            }

            this.HideWaitMessage();
            this.ColumnColorChange();
        }

        private void CompleteColor_Green(DataRow dr, int ii)
        {
            int outputQty = MyUtility.Convert.GetInt(dr["OutputQty"]);

            foreach (DataColumn dc in ((DataTable)this.listControlBindingSourceRight.DataSource).Columns)
            {
                if (dc.ColumnName.ToUpper() != "ORDERID" && dc.ColumnName.ToUpper() != "ID" && dc.ColumnName.ToUpper() != "UKEY")
                {
                    int rightQty = MyUtility.Convert.GetInt(((DataTable)this.listControlBindingSourceRight.DataSource).Rows[ii][dc]);
                    if (outputQty < rightQty && !rightQty.EqualDecimal(0))
                    {
                        break;
                    }

                    if (outputQty >= rightQty && !rightQty.EqualDecimal(0))
                    {
                        this.gridRight.Rows[ii].Cells[dc.ColumnName].Style.BackColor = Color.Green;
                        outputQty = outputQty - rightQty;
                    }
                }
            }
        }

        // private void GridRight_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        // {
        //    if (e.RowIndex == -1)
        //    {
        //        return;
        //    }

        // DataRow drOutputInfo = this.gridRight.GetDataRow<DataRow>(e.RowIndex);

        // var orderID = drOutputInfo["OrderID"];
        //    var outputDate = this.gridRight.Columns[e.ColumnIndex].Name;
        //    var id = drOutputInfo["ID"];
        //    var ukey = drOutputInfo["Ukey"];

        // DataRow[] drRemark = ((DataTable)this.listControlBindingSourceRemark.DataSource).Select($"Ukey = '{ukey}' and OutputDate = '{outputDate}'");

        // if (drRemark.Length == 0)
        //    {
        //        string strRemark = this.EditRemark(string.Empty);

        // if (strRemark.Empty() == false)
        //        {
        //            DataRow newRemark = ((DataTable)this.listControlBindingSourceRemark.DataSource).NewRow();
        //            newRemark["OrderID"] = orderID;
        //            newRemark["OutputDate"] = outputDate;
        //            newRemark["ID"] = id;
        //            newRemark["Ukey"] = ukey;
        //            newRemark["Remark"] = strRemark;
        //            newRemark.EndEdit();

        // ((DataTable)this.listControlBindingSourceRemark.DataSource).Rows.Add(newRemark);
        //            //this.RemarkColorChange(newRemark);
        //        }
        //    }
        //    else
        //    {
        //        string strRemark = this.EditRemark(drRemark[0]["Remark"].ToString());

        // if (strRemark.Empty() == false)
        //        {
        //            drRemark[0]["Remark"] = strRemark;
        //            //this.RemarkColorChange(drRemark[0]);
        //        }
        //        else
        //        {
        //            //this.RemarkColorChange(drRemark[0], true);
        //            drRemark[0].Delete();
        //        }
        //    }
        // }
        private string EditRemark(string strRemark)
        {
            Win.Tools.EditMemo editUi = new Win.Tools.EditMemo(strRemark, "P10_Remark", true, null);
            editUi.Width = 300;
            editUi.Height = 300;

            if (editUi.ShowDialog() == DialogResult.OK)
            {
                strRemark = editUi.Memo;
            }

            return strRemark;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if ((DataTable)this.listControlBindingSourceLeft.DataSource == null)
            {
                return;
            }

            this.ShowWaitMessage("Data Processing ...");
            DataTable dtTitle = ((DataTable)this.listControlBindingSourceLeft.DataSource).AsEnumerable().CopyToDataTable();
            DataTable dtOutputDateQtyRemark;

            int intProcessCount = dtTitle.Rows.Count;
            int intProcessIndex = 1;

            #region 合併 OutputDate Qty 與 Remark
            DataTable dtOutputDateRemark = ((DataTable)this.listControlBindingSourceRemark.DataSource).Clone();

            if (((DataTable)this.listControlBindingSourceRemark.DataSource).AsEnumerable().Any(row => row.RowState != DataRowState.Deleted))
            {
                dtOutputDateRemark = ((DataTable)this.listControlBindingSourceRemark.DataSource).AsEnumerable().Where(row => row.RowState != DataRowState.Deleted).CopyToDataTable();
            }

            string strUnpivotSQL = $@"
select OrderID
    , ID
    , Ukey
    , OutputDate
    , Qty
from (
    select *
    from #tmp
) a
UNPIVOT (
    Qty for OutputDate in ({((DataTable)this.listControlBindingSourceDateRange.DataSource).Rows[0][0]})
) b
where Qty is not null
      and Qty > 0";

            DualResult result = MyUtility.Tool.ProcessWithDatatable((DataTable)this.listControlBindingSourceRight.DataSource, null, strUnpivotSQL, out dtOutputDateQtyRemark);
            if (result == false)
            {
                MyUtility.Msg.WarningBox(result.ToString());
                this.HideWaitMessage();
                return;
            }
            else
            {
                dtOutputDateQtyRemark.PrimaryKey = new DataColumn[] { dtOutputDateQtyRemark.Columns["Ukey"], dtOutputDateQtyRemark.Columns["OutputDate"] };
                dtOutputDateQtyRemark.Merge(dtOutputDateRemark);
            }
            #endregion

            TransactionScope transactionscope = new TransactionScope();

            using (transactionscope)
            {
                try
                {
                    foreach (DataRow drPPASchedule in dtTitle.Rows)
                    {
                        this.ShowWaitMessage($"Data Processing ({intProcessIndex} / {intProcessCount}) ...", 500);

                        List<SqlParameter> listSQLParameter = new List<SqlParameter>();
                        listSQLParameter.Add(new SqlParameter("@UserName", Sci.Env.User.UserID));
                        listSQLParameter.Add(new SqlParameter("@OrderID", drPPASchedule["OrderID"]));
                        listSQLParameter.Add(new SqlParameter("@StartDate", this.objStartDate));
                        listSQLParameter.Add(new SqlParameter("@EndDate", this.objEndDate));
                        listSQLParameter.Add(new SqlParameter("@MDivisionID ", Sci.Env.User.Keyword));

                        DataTable dtPPASchedule_Detail = dtOutputDateQtyRemark.Clone();

                        if (dtOutputDateQtyRemark.AsEnumerable().Any(row => row.RowState != DataRowState.Deleted
                                                                            && row["Ukey"].EqualDecimal(drPPASchedule["Ukey"])))
                        {
                            dtPPASchedule_Detail = dtOutputDateQtyRemark.AsEnumerable().Where(row => row.RowState != DataRowState.Deleted
                                                                                                        && row["Ukey"].EqualDecimal(drPPASchedule["Ukey"])).CopyToDataTable();
                        }

                        DataTable dtPPAID;
                        object objPPAID = null;
                        #region PPASchedule
                        string strUpdatePPASchedule = @"
Merge PPASchedule t
Using (
select *
from #tmp
) s on t.ID = s.ID
when matched then 
update set
	t.OrderID = s.OrderID
	, t.[Group] = s.[Group]
	, t.SubProcessLineID = s.SubProcessLineID
	, t.OutputQty = s.OutputQty
	, t.TargetQty = s.TargetQty
	, t.Feature = s.Feature
	, t.SMV = s.SMV
	, t.EarlyInline = s.EarlyInline
	, t.SubProcessLearnCurveID = s.SubProcessLearnCurveID
	, t.Inline = s.Inline
	, t.Offline = s.Offline
	, t.EditDate = GETDATE()
	, t.EditName = @UserName
    , t.MDivisionID = @MDivisionID
    , t.Overload = s.Overload
when not matched by target then
insert (
	OrderID		, [Group]	, SubProcessLineID		, OutputQty					, TargetQty
	, Feature	, SMV		, EarlyInline			, SubProcessLearnCurveID	, Inline
	, Offline	, AddDate	, AddName, MDivisionID,Overload)
values (
	s.OrderID	, s.[Group]	, s.SubProcessLineID	, s.OutputQty				, s.TargetQty
	, s.Feature	, s.SMV		, s.EarlyInline			, s.SubProcessLearnCurveID	, s.Inline
	, s.Offline	, GETDATE()	, @UserName, @MDivisionID,isnull(s.Overload,0));
select @@IDENTITY";
                        List<DataRow> listTmpTable = new List<DataRow>();
                        listTmpTable.Add(drPPASchedule);

                        result = MyUtility.Tool.ProcessWithDatatable(listTmpTable.CopyToDataTable(), null, strUpdatePPASchedule, out dtPPAID, paramters: listSQLParameter);

                        if (result == false)
                        {
                            MyUtility.Msg.WarningBox(result.ToString());
                            transactionscope.Dispose();
                            this.HideWaitMessage();
                            return;
                        }
                        else if (dtPPAID.Rows.Count == 0)
                        {
                            MyUtility.Msg.WarningBox("PPASchedule can not get ID.");
                        }
                        else
                        {
                            objPPAID = dtPPAID.Rows[0][0];
                        }
                        #endregion

                        #region Update PPASchedule_Detail ID
                        if (drPPASchedule["ID"] == DBNull.Value)
                        {
                            foreach (DataRow dr in dtPPASchedule_Detail.Rows)
                            {
                                dr["ID"] = objPPAID;
                            }
                        }
                        else
                        {
                            objPPAID = drPPASchedule["ID"];
                        }

                        listSQLParameter.Add(new SqlParameter("@ID", objPPAID));
                        #endregion

                        #region PPASchedule_Detail
                        string strUpdatePPASchedule_DetailSQL = @"
Merge PPASchedule_Detail t
Using (
select *
from #tmp
) s on t.ID = s.ID
and t.OrderID = s.OrderID
and t.OutputDate = s.OutputDate
when matched then 
update set
t.Qty = s.Qty
, t.Remark = s.Remark
when NOT MATCHED by Target then
insert (
ID	, OrderID	, OutputDate	, WeekOfYear					, Qty
, Remark)
values (
s.ID, s.OrderID	, s.OutputDate	, DATEPART(WEEK, s.OutputDate)	, s.Qty
, s.Remark);

delete t
from PPASchedule_Detail t
left join #tmp s on t.ID = s.ID
			and t.OrderID = s.OrderID
			and t.OutputDate = s.OutputDate
where s.ID is null
and t.ID= @ID
and t.OrderID = @OrderID
and t.OutputDate between @StartDate and @EndDate";

                        result = MyUtility.Tool.ProcessWithDatatable(dtPPASchedule_Detail, null, strUpdatePPASchedule_DetailSQL, out dtPPAID, paramters: listSQLParameter);

                        if (result == false)
                        {
                            MyUtility.Msg.WarningBox(result.ToString());
                            transactionscope.Dispose();
                            this.HideWaitMessage();
                            return;
                        }
                        #endregion
                    }

                    transactionscope.Complete();
                }
                catch (Exception ex)
                {
                    transactionscope.Dispose();
                    this.ShowErr("Commit transaction error.", ex);
                    this.HideWaitMessage();
                    return;
                }
                finally
                {
                    transactionscope.Dispose();
                }

                this.BtnQuery_Click(null, null);
            }

            this.HideWaitMessage();
        }

        private void ComboBoxUpdateColumn_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listControlBindingSourceLeft.DataSource != null)
            {
                this.txtUpdateColumn.Text = string.Empty;
                this.DrUpdateMutipleColumn = ((DataTable)this.listControlBindingSourceLeft.DataSource).NewRow();
            }
        }

        private void TxtUpdateColumn_Validating(object sender, CancelEventArgs e)
        {
            string strComboBoxColumnName = this.comboBoxUpdateColumn.Text;

            switch (strComboBoxColumnName)
            {
                case "PPA Group":
                    this.PPAGroupValidating(null, this.txtUpdateColumn.Text, this.UpdateMultipleGetValue);
                    break;
                case "PPA Line":
                    this.PPALineValidating(null, this.txtUpdateColumn.Text, this.UpdateMultipleGetValue);
                    break;
                case "Learn Curve":
                    this.LearnCurveValidating(null, this.DrUpdateMutipleColumn["SubProcessLearnCurveID"].ToString(), this.txtUpdateColumn.Text, this.UpdateMultipleGetValue);
                    break;
                case "Early Inline":
                    decimal earlyDate;
                    decimal.TryParse(this.txtUpdateColumn.Text, out earlyDate);
                    this.txtUpdateColumn.Text = earlyDate.ToString();
                    break;
                case "Over load":
                    if (this.txtUpdateColumn.Text != "0" && this.txtUpdateColumn.Text != "1")
                    {
                        MyUtility.Msg.WarningBox("Overload must be the 0 or 1 !!");
                        this.txtUpdateColumn.Text = string.Empty;
                    }

                    break;
            }
        }

        private void TxtUpdateColumn_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                string strComboBoxColumnName = this.comboBoxUpdateColumn.Text;

                switch (strComboBoxColumnName)
                {
                    case "PPA Group":
                        this.PPAGroupMouseRightClick(null, this.DrUpdateMutipleColumn["Group"].ToString(), this.UpdateMultipleGetValue);
                        break;
                    case "PPA Line":
                        this.PPALineMouseRightClick(null, this.DrUpdateMutipleColumn["Group"].ToString(), this.UpdateMultipleGetValue);
                        break;
                    case "PPA Feature":
                        break;
                    case "Learn Curve":
                        this.LearnCurveMouseRightClick(null, this.DrUpdateMutipleColumn["SubProcessLearnCurveID"].ToString(), this.UpdateMultipleGetValue);
                        break;
                }
            }
        }

        private void PictureBoxUpdateColumn_Click(object sender, EventArgs e)
        {
            string strComboBoxColumnName = this.comboBoxUpdateColumn.Text;
            if (((DataTable)this.listControlBindingSourceLeft.DataSource).AsEnumerable().Any(row => row["sel"].EqualDecimal(1)) == true)
            {
                foreach (DataRow dr in ((DataTable)this.listControlBindingSourceLeft.DataSource).Rows)
                {
                    if (dr["Sel"].EqualDecimal(1))
                    {
                        switch (strComboBoxColumnName)
                        {
                            case "PPA Group":
                            case "PPA Line":
                                dr["Group"] = this.DrUpdateMutipleColumn["Group"];
                                dr["SubProcessLineID"] = this.DrUpdateMutipleColumn["SubProcessLineID"];
                                dr.EndEdit();
                                break;
                            case "Target Qty":
                                this.TargetQtyValidating(dr, this.txtUpdateColumn.Text, dr["TargetQty"]);
                                break;
                            case "PPA Feature":
                                break;
                            case "Early Inline":
                                this.EarlyInlineValidating(dr, this.txtUpdateColumn.Text, dr["EarlyInline"], dr["Inline"], true);
                                break;
                            case "Learn Curve":
                                this.LearnCurveValidating(dr, dr["SubProcessLearnCurveID"].ToString(), this.txtUpdateColumn.Text);
                                break;
                            case "Over load":
                                if (!MyUtility.Check.Empty(this.txtUpdateColumn.Text))
                                {
                                    dr["Overload"] = this.txtUpdateColumn.Text == "1" ? true : false;
                                    dr.EndEdit();
                                }

                                break;
                        }
                    }
                }
            }
            else
            {
                MyUtility.Msg.InfoBox("Please select data first.");
            }
        }

        private void BtnToExcel_Click(object sender, EventArgs e)
        {
            if (this.listControlBindingSourceRight.DataSource.Empty() == false
                && this.listControlBindingSourceLeft.DataSource.Empty() == false)
            {
                DataTable dtMerge_TopRight = ((DataTable)this.listControlBindingSourceTop.DataSource).AsEnumerable().CopyToDataTable();
                dtMerge_TopRight.Merge((DataTable)this.listControlBindingSourceRight.DataSource);

                P10_ToExcel frm = new P10_ToExcel(
                    this.objStartDate,
                    this.objEndDate,
                    dtMerge_TopRight,
                    ((DataTable)this.listControlBindingSourceLeft.DataSource).AsEnumerable().CopyToDataTable());

                frm.ShowDialog();
            }
        }

        private void BtnColse_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #region PPASchedule Columns DataValidating & Mouse Right Click
        #region PPA Group
        private void PPAGroupMouseRightClick(DataRow dr, string strGroup, bool isUpdateMultipleGetValue = false)
        {
            this.PPA_GroupLine_Click(dr, strGroup, isUpdateMultipleGetValue);
        }

        private void PPAGroupValidating(DataRow dr, string newGroup, bool isUpdateMultipleGetValue = false)
        {
            if (newGroup.Empty() == false)
            {
                List<SqlParameter> listSQLParameter = new List<SqlParameter>();
                listSQLParameter.Add(new SqlParameter("@Group", newGroup));

                string strCheckGroupSQL = $@"
select ID
from SubProcessLine
where Type = 'PPA'
	  and Junk = 0 and MDivisionID ='{Sci.Env.User.Keyword}'
      and GroupID = @Group";

                if (MyUtility.Check.Seek(strCheckGroupSQL, listSQLParameter) == true)
                {
                    if (isUpdateMultipleGetValue)
                    {
                        this.txtUpdateColumn.Text = newGroup;

                        this.DrUpdateMutipleColumn["Group"] = newGroup;
                        this.DrUpdateMutipleColumn["SubProcessLineID"] = MyUtility.GetValue.Lookup(strCheckGroupSQL, listSQLParameter);
                        this.DrUpdateMutipleColumn.EndEdit();
                    }
                    else
                    {
                        dr["Group"] = newGroup;
                        dr["SubProcessLineID"] = MyUtility.GetValue.Lookup(strCheckGroupSQL, listSQLParameter);
                        dr.EndEdit();
                    }
                }
                else
                {
                    MyUtility.Msg.WarningBox($"<PPA Group : {newGroup}> not found!!");

                    if (isUpdateMultipleGetValue)
                    {
                        this.txtUpdateColumn.Text = string.Empty;

                        this.DrUpdateMutipleColumn["Group"] = string.Empty;
                        this.DrUpdateMutipleColumn["SubProcessLineID"] = string.Empty;
                        this.DrUpdateMutipleColumn.EndEdit();
                    }
                    else
                    {
                        dr["Group"] = string.Empty;
                        dr["SubProcessLineID"] = string.Empty;
                        dr.EndEdit();
                    }
                }
            }
            else if (dr != null)
            {
                dr["Group"] = string.Empty;
                dr["SubProcessLineID"] = string.Empty;
                dr.EndEdit();
            }
        }
        #endregion

        #region  PPA Line
        private void PPALineMouseRightClick(DataRow dr, string strGroup, bool isUpdateMultipleGetValue = false)
        {
            this.PPA_GroupLine_Click(dr, strGroup, isUpdateMultipleGetValue);
        }

        private void PPALineValidating(DataRow dr, string newID, bool isUpdateMultipleGetValue = false)
        {
            if (newID.Empty() == false)
            {
                List<SqlParameter> listSQLParameter = new List<SqlParameter>();
                listSQLParameter.Add(new SqlParameter("@ID", newID));

                string strCheckGroupSQL = $@"
select GroupID
from SubProcessLine
where Type = 'PPA'
	  and Junk = 0 and MDivisionID ='{Sci.Env.User.Keyword}'
      and ID = @ID";

                if (MyUtility.Check.Seek(strCheckGroupSQL, listSQLParameter) == true)
                {
                    if (isUpdateMultipleGetValue)
                    {
                        this.txtUpdateColumn.Text = newID;

                        this.DrUpdateMutipleColumn["Group"] = MyUtility.GetValue.Lookup(strCheckGroupSQL, listSQLParameter);
                        this.DrUpdateMutipleColumn["SubProcessLineID"] = newID;
                        this.DrUpdateMutipleColumn.EndEdit();
                    }
                    else
                    {
                        dr["Group"] = MyUtility.GetValue.Lookup(strCheckGroupSQL, listSQLParameter);
                        dr["SubProcessLineID"] = newID;
                        dr.EndEdit();
                    }
                }
                else
                {
                    MyUtility.Msg.WarningBox($"<PPA Line : {newID}> not found!!");

                    if (isUpdateMultipleGetValue)
                    {
                        this.txtUpdateColumn.Text = string.Empty;

                        this.DrUpdateMutipleColumn["Group"] = string.Empty;
                        this.DrUpdateMutipleColumn["SubProcessLineID"] = string.Empty;
                        this.DrUpdateMutipleColumn.EndEdit();
                    }
                    else
                    {
                        dr["Group"] = string.Empty;
                        dr["SubProcessLineID"] = string.Empty;
                        dr.EndEdit();
                    }
                }
            }
        }
        #endregion

        #region Target Qty
        private void TargetQtyValidating(DataRow dr, object newTargetQty, object oldTargetQty)
        {
            if (newTargetQty.EqualDecimal(oldTargetQty) == false)
            {
                dr["TargetQty"] = newTargetQty;

                if (this.ReCalculateOutputDateQty(dr) == false)
                {
                    dr["TargetQty"] = oldTargetQty;
                }

                dr["Manpower"] = this.ComputeManpower(dr);
                dr.EndEdit();
            }
        }
        #endregion

        #region PPA Feature
        private void PPAFeatureMouseRightClick(DataRow dr)
        {
            List<SqlParameter> listSQLParameter = new List<SqlParameter>();
            listSQLParameter.Add(new SqlParameter("@OrderID", dr["OrderID"]));
            string strGetFeatureSQL = @"
select sf.Feature
       , SMV = CONVERT(varchar, sf.SMV)
       , sf.Remark
from Style_Feature sf
inner join Orders o on sf.styleUkey = o.StyleUkey
where o.ID = @OrderID
	  and type = 'PPA'
order by Feature";

            DataTable dtSelectItems;
            DualResult result = DBProxy.Current.Select(null, strGetFeatureSQL, listSQLParameter, out dtSelectItems);

            Sci.Win.Tools.SelectItem2 items = new Win.Tools.SelectItem2(dtSelectItems, "Feature,SMV,Remark", "Feature,SMV,Remark", "10,6,20", dr["Feature"].ToString());
            DialogResult diaResult = items.ShowDialog();

            if (diaResult != DialogResult.Cancel)
            {
                dr["Feature"] = items.GetSelectedString();
                dr["SMV"] = items.GetSelecteds().Sum(row => Convert.ToDecimal(row["SMV"]));
                dr["Manpower"] = this.ComputeManpower(dr);
                dr.EndEdit();
            }
        }
        #endregion

        #region SMV
        private void SMVValidating(DataRow dr, object newSMV)
        {
            dr["SMV"] = newSMV;
            dr["Manpower"] = this.ComputeManpower(dr);

            // SMV 修改必須同時更新 Daily CPU
            DataRow drRightGrid = ((DataTable)this.listControlBindingSourceRight.DataSource).Select($"Ukey = {dr["Ukey"]}")[0];

            for (int i = this.intRightColumnHeaderCount; i < drRightGrid.Table.Columns.Count; i++)
            {
                if (drRightGrid[i] != DBNull.Value)
                {
                    this.ComputDailyCPU(i);
                }
            }

            dr.EndEdit();
        }
        #endregion

        #region Early Inline
        private void EarlyInlineValidating(DataRow dr, object newEarlyInline, object oldEarlyInline, object oldInline, bool flag)
        {
            if (newEarlyInline.EqualDecimal(oldEarlyInline) == false || flag)
            {
                dr["EarlyInline"] = newEarlyInline;
                dr.EndEdit();

                List<SqlParameter> listSQLParameter = new List<SqlParameter>();
                listSQLParameter.Add(new SqlParameter("@SewInLine", dr["SewInLine"]));
                listSQLParameter.Add(new SqlParameter("@EarlyInline", dr["EarlyInline"]));
                listSQLParameter.Add(new SqlParameter("@FactoryID", Sci.Env.User.Factory));

                #region Update PPA Inline
                string strGetNewPPAInlineDateSQL = @"select Inline = dbo.[CalculateWorkDate](@SewInLine, 0 - @EarlyInline, @FactoryID)";
                dr["Inline"] = MyUtility.GetValue.Lookup(strGetNewPPAInlineDateSQL, listSQLParameter);
                dr["ShowInline"] = Convert.ToDateTime(dr["Inline"]).ToString("MM/dd");
                #endregion

                if (this.ReCalculateOutputDateQty(dr) == false)
                {
                    dr["EarlyInline"] = oldEarlyInline;
                    dr["Inline"] = oldInline;
                    dr["ShowInline"] = Convert.ToDateTime(dr["Inline"]).ToString("MM/dd");

                    // 回朔資料時必須連 Output 一併回朔
                    this.ReCalculateOutputDateQty(dr);
                }

                dr.EndEdit();
            }
        }
        #endregion

        #region Learn Curve
        private void LearnCurveValidating(DataRow dr, string strOldSubProcessLearnCurveID, string strNewSubProcessLearnCurveID, bool isUpdateMultipleGetValue = false)
        {
            if (strOldSubProcessLearnCurveID.EqualString(strNewSubProcessLearnCurveID) == false)
            {
                if (strNewSubProcessLearnCurveID.Empty() == true)
                {
                    if (isUpdateMultipleGetValue == false)
                    {
                        dr["SubProcessLearnCurveID"] = string.Empty;

                        this.ReCalculateOutputDateQty(dr);
                        dr.EndEdit();
                    }
                }
                else
                {
                    List<SqlParameter> listSQLParameter = new List<SqlParameter>();
                    listSQLParameter.Add(new SqlParameter("@SubProcessLearnCurveID", strNewSubProcessLearnCurveID));

                    string strCheckLearnCurveSQL = @"
select ID
	   , Description
from SubProcessLearnCurve
where Type = 'PPA'
	  and Junk = 0
      and ID = @SubProcessLearnCurveID";

                    if (MyUtility.Check.Seek(strCheckLearnCurveSQL, listSQLParameter))
                    {
                        if (isUpdateMultipleGetValue)
                        {
                            this.txtUpdateColumn.Text = strNewSubProcessLearnCurveID;
                            this.DrUpdateMutipleColumn["SubProcessLearnCurveID"] = strNewSubProcessLearnCurveID;
                            this.DrUpdateMutipleColumn.EndEdit();
                        }
                        else
                        {
                            dr["SubProcessLearnCurveID"] = strNewSubProcessLearnCurveID;

                            this.ReCalculateOutputDateQty(dr);
                            dr.EndEdit();
                        }
                    }
                    else
                    {
                        MyUtility.Msg.WarningBox($"<Learn Curve : {strNewSubProcessLearnCurveID}> not found !!");

                        if (isUpdateMultipleGetValue)
                        {
                            this.txtUpdateColumn.Text = string.Empty;
                            this.DrUpdateMutipleColumn["SubProcessLearnCurveID"] = string.Empty;
                            this.DrUpdateMutipleColumn.EndEdit();
                        }
                        else
                        {
                            dr["SubProcessLearnCurveID"] = string.Empty;

                            this.ReCalculateOutputDateQty(dr);
                            dr.EndEdit();
                        }
                    }
                }
            }
        }

        private void LearnCurveMouseRightClick(DataRow dr, string strOldSubProcessLearnCurveID, bool isUpdateMultipleGetValue = false)
        {
            string strGetLearnCurveSQL = @"
select ID
	   , Description
from SubProcessLearnCurve
where Type = 'PPA'
	  and Junk = 0
order by ID";
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(strGetLearnCurveSQL, "10,20", strOldSubProcessLearnCurveID);

            DialogResult result = item.ShowDialog();
            if (result != DialogResult.Cancel)
            {
                if (isUpdateMultipleGetValue)
                {
                    this.txtUpdateColumn.Text = item.GetSelecteds()[0]["ID"].ToString();
                    this.DrUpdateMutipleColumn["SubProcessLearnCurveID"] = item.GetSelecteds()[0]["ID"].ToString();
                    this.DrUpdateMutipleColumn.EndEdit();
                }
                else
                {
                    dr["SubProcessLearnCurveID"] = item.GetSelecteds()[0]["ID"].ToString();

                    if (this.ReCalculateOutputDateQty(dr) == false)
                    {
                        dr["SubProcessLearnCurveID"] = string.Empty;
                    }

                    dr.EndEdit();
                }
            }
        }
        #endregion

        #region
        #endregion
        #endregion

        private void ColumnColorChange()
        {
            this.setEarlyInlinecolor.CellFormatting += (s, e) =>
            {
                DataRow dr = this.gridLeft.GetDataRow<DataRow>(e.RowIndex);
                if (e.RowIndex == -1)
                {
                    return;
                }

                Color newColor;
                if (Convert.ToDecimal(dr["EarlyInline"]) > 5)
                {
                    // 黃色
                    newColor = Color.Yellow;
                }
                else if (Convert.ToDecimal(dr["EarlyInline"]) < 5)
                {
                    // 灰色
                    newColor = Color.Gray;
                }
                else
                {
                    // 不變色
                    newColor = Color.White;
                }

                this.gridLeft.Rows[e.RowIndex].Cells["EarlyInline"].Style.BackColor = newColor;
            };

            this.setPPAInlinecolor.CellFormatting += (s, e) =>
            {
                DataRow dr = this.gridLeft.GetDataRow<DataRow>(e.RowIndex);
                if (e.RowIndex == -1)
                {
                    return;
                }

                Color newColor;
                if (MyUtility.Check.Empty(dr["CutInLine"]))
                {
                    newColor = Color.Red;
                }
                else if (MyUtility.Convert.GetDate(dr["Inline"]) <= MyUtility.Convert.GetDate(dr["CutInLine"]))
                {
                    newColor = Color.Red;
                }
                else
                {
                    newColor = Color.White;
                }

                this.gridLeft.Rows[e.RowIndex].Cells["ShowInline"].Style.BackColor = newColor;
            };
        }

        private void PPA_GroupLine_Click(DataRow dr, string strGroup, bool isUpdateMultipleGetValue)
        {
            string strGetListSQL = $@"
select [Group] = GroupID
       , ID
from SubProcessLine
where Type = 'PPA'
	  and Junk = 0 and MDivisionID ='{Sci.Env.User.Keyword}'
order by GroupID";

            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(strGetListSQL, "15,15", strGroup);

            DialogResult result = item.ShowDialog();
            if (result != DialogResult.Cancel)
            {
                if (isUpdateMultipleGetValue)
                {
                    switch (this.comboBoxUpdateColumn.Text)
                    {
                        case "PPA Group":
                            this.txtUpdateColumn.Text = item.GetSelecteds()[0]["Group"].ToString();
                            break;
                        case "PPA Line":
                            this.txtUpdateColumn.Text = item.GetSelecteds()[0]["ID"].ToString();
                            break;
                    }

                    this.DrUpdateMutipleColumn["Group"] = item.GetSelecteds()[0]["Group"].ToString();
                    this.DrUpdateMutipleColumn["SubProcessLineID"] = item.GetSelecteds()[0]["ID"].ToString();
                    this.DrUpdateMutipleColumn.EndEdit();
                }
                else
                {
                    dr["Group"] = item.GetSelecteds()[0]["Group"].ToString();
                    dr["SubProcessLineID"] = item.GetSelecteds()[0]["ID"].ToString();
                    dr.EndEdit();
                }
            }
        }

        private decimal ComputeManpower(DataRow dr)
        {
            return Math.Round(Convert.ToDecimal(dr["TargetQty"]) * Convert.ToDecimal(dr["SMV"]) / (decimal)60.0 / Convert.ToDecimal(this.numericBoxWorkHour.Text) / Convert.ToDecimal(this.numericBoxEFF.Text), MidpointRounding.AwayFromZero);
        }

        private bool ReCalculateOutputDateQty(DataRow dr)
        {
            List<SqlParameter> listSQLParameter = new List<SqlParameter>();
            listSQLParameter.Add(new SqlParameter("@Inline", dr["Inline"]));
            listSQLParameter.Add(new SqlParameter("@FactoryID", Sci.Env.User.Factory));
            listSQLParameter.Add(new SqlParameter("@OrderQty", dr["OrderQty"]));
            listSQLParameter.Add(new SqlParameter("@TargetQty", dr["TargetQty"]));
            listSQLParameter.Add(new SqlParameter("@SubProcessLearnCurveID", dr["SubProcessLearnCurveID"]));

            DualResult result;

            /*
             * TargetQty 必須大於 0
             * 否則不會有生產數量
             */
            #region OutputDate & DailyQty
            DataTable dtOutputDailyQty;
            string strGetOutputDailyQty = @"
select DailyQty = DailyQty.value
from (
	select OutputQty = Floor (@TargetQty * Efficiency / 100)
		   , RunningTotal = sum (Floor (@TargetQty * Efficiency / 100)) over (order by Day)
	from SubProcessLearnCurve splc
	inner join SubProcessLearnCurve_Detail splcd on splc.ukey = splcd.ukey
	where splc.Type = 'PPA'
		  and splc.ID = @SubProcessLearnCurveID
) tmp
outer apply (
	select value = case
					  when @OrderQty - RunningTotal >= 0 then OutputQty
					  else OutputQty + @OrderQty - RunningTotal
				   end
) DailyQty
where DailyQty.value > 0";

            result = DBProxy.Current.Select(null, strGetOutputDailyQty, listSQLParameter, out dtOutputDailyQty);

            if (result == false)
            {
                MyUtility.Msg.WarningBox(result.ToString());
                dr["Offline"] = DBNull.Value;
                dr["ShowOffline"] = DBNull.Value;
                this.UpdateOutputDateGrid(null, dr["Ukey"].ToString(), null);
                return false;
            }
            else
            {
                listSQLParameter.Add(new SqlParameter("@CountOutputDate", dtOutputDailyQty.Rows.Count));
            }
            #endregion

            #region Update PPA Offline
            /*
             * CountOutputDate 要扣除第一天生產
             * 之後的生產日期往上累計
             */
            string strGetNewPPAOfflineDateSQL = @"select Inline = dbo.[CalculateWorkDate](@Inline, @CountOutputDate - 1, @FactoryID)";
            dr["Offline"] = MyUtility.GetValue.Lookup(strGetNewPPAOfflineDateSQL, listSQLParameter);

            if (dr["Offline"].Empty() == false)
            {
                dr["ShowOffline"] = Convert.ToDateTime(dr["Offline"]).ToString("MM/dd");
            }
            #endregion

            string strStartDate = ((DateTime)dr["Inline"]).ToString("yyyy/MM/dd");

            string strUpdateResult = this.UpdateOutputDateGrid(strStartDate, dr["Ukey"].ToString(), dtOutputDailyQty);

            if (strUpdateResult.EqualString("OutOfRange"))
            {
                // 若超出範圍 Offline 必須清除
                dr["Offline"] = DBNull.Value;
                dr["ShowOffline"] = DBNull.Value;
                return false;
            }
            else if (strUpdateResult.EqualString("OutputEmpty"))
            {
                dr["Offline"] = DBNull.Value;
                dr["ShowOffline"] = DBNull.Value;
            }

            return true;
        }

        /// <summary>
        /// 更新 Output 的畫面
        /// </summary>
        /// <param name="strStartDate">生產的啟示日期</param>
        /// <param name="ukey">ukey</param>
        /// <param name="dtOutputDailyQty">每日產出</param>
        /// <returns>result</returns>
        private string UpdateOutputDateGrid(string strStartDate, string ukey, DataTable dtOutputDailyQty)
        {
            string updateResult = string.Empty;
            DataRow[] drOutput = ((DataTable)((BindingSource)this.gridRight.DataSource).DataSource).Select($"Ukey = '{ukey}'");

            if (drOutput.Length > 0)
            {
                int intRightGridColumns = drOutput[0].Table.Columns.Count;
                this.CleanOutputQtyRows(drOutput[0], intRightGridColumns);

                if (dtOutputDailyQty == null
                    || dtOutputDailyQty.Rows.Count > 0)
                {
                    if (drOutput[0].Table.Columns[strStartDate] == null)
                    {
                        // 【起始】日期超出範圍
                        MyUtility.Msg.WarningBox($"The allocate date is exceed min date, please re-schedule !!");
                        updateResult = "OutOfRange";
                    }
                    else
                    {
                        int intStartColumn = drOutput[0].Table.Columns[strStartDate].Ordinal;

                        if (intStartColumn + dtOutputDailyQty.Rows.Count > intRightGridColumns)
                        {
                            // 【結束】日期超出範圍
                            MyUtility.Msg.WarningBox($"The allocate date is exceed max date, please re-schedule !!");
                            updateResult = "OutOfRange";
                        }
                        else
                        {
                            foreach (DataRow drOutputDailyQty in dtOutputDailyQty.Rows)
                            {
                                if (intStartColumn < intRightGridColumns)
                                {
                                    drOutput[0][intStartColumn] = drOutputDailyQty["DailyQty"];
                                    this.ComputDailyCPU(intStartColumn);
                                    intStartColumn++;
                                }
                            }
                        }
                    }
                }
                else
                {
                    // 沒有生產數量
                    updateResult = "OutputEmpty";
                }
            }

            return updateResult;
        }

        /// <summary>
        /// 清除 SP# 有以下情況的 OutputQty
        /// 1. 資料輸入錯誤
        /// 2. 沒有產出
        /// </summary>
        /// <param name="drOutput">需修改的資料列</param>
        /// <param name="intRightGridColumns">該資料列的長度</param>
        private void CleanOutputQtyRows(DataRow drOutput, int intRightGridColumns)
        {
            for (int i = this.intRightColumnHeaderCount; i < intRightGridColumns; i++)
            {
                if (drOutput[i] != DBNull.Value)
                {
                    drOutput[i] = DBNull.Value;
                    this.ComputDailyCPU(i);
                }
            }

            drOutput.EndEdit();
        }

        /// <summary>
        /// 重算 Daily CPU
        /// </summary>
        /// <param name="columnIndex">需要重算 CPU 的欄位</param>
        private void ComputDailyCPU(int columnIndex)
        {
            DataTable dtRightGrid = (DataTable)((BindingSource)this.gridRight.DataSource).DataSource;
            DataTable dtTopGrid = (DataTable)((BindingSource)this.gridTop.DataSource).DataSource;
            var varStdTMS = MyUtility.GetValue.Lookup("select StdTMS from System");
            int intStdTMS = 0;

            intStdTMS = varStdTMS.Empty() ? 0 : Convert.ToInt32(varStdTMS);

            if (dtRightGrid.Rows.Count > 0)
            {
                decimal computCPU = 0;
                long returnCPU = 0;

                if (intStdTMS.EqualDecimal(0) == true)
                {
                    returnCPU = 0;
                }
                else
                {
                    for (int i = 0; i < dtRightGrid.Rows.Count; i++)
                    {
                        if (dtRightGrid.Rows[i][columnIndex].Empty() == false)
                        {
                            DataRow drLeftGrid = ((DataTable)((BindingSource)this.gridLeft.DataSource).DataSource).Rows[i];

                            decimal qty = Convert.ToDecimal(dtRightGrid.Rows[i][columnIndex]);
                            decimal smv = Convert.ToDecimal(drLeftGrid["SMV"]);

                            computCPU += qty * smv * (decimal)60.0 / intStdTMS;
                        }
                    }

                    returnCPU = Convert.ToInt64(Math.Round(computCPU, MidpointRounding.AwayFromZero));
                }

                switch (returnCPU)
                {
                    case 0:
                        dtTopGrid.Rows[1][columnIndex] = DBNull.Value;
                        break;
                    default:
                        dtTopGrid.Rows[1][columnIndex] = computCPU;
                        break;
                }

                dtTopGrid.Rows[1].EndEdit();
            }
        }

        private void GridLeft_Scroll(object sender, ScrollEventArgs e)
        {
            if (this.gridLeft.Rows.Count > 0
                && this.gridRight.Rows.Count > 0)
            {
                this.gridRight.FirstDisplayedScrollingRowIndex = this.gridLeft.FirstDisplayedScrollingRowIndex;
            }
        }

        private void GridRight_Scroll(object sender, ScrollEventArgs e)
        {
            if (this.gridRight.Rows.Count > 0
                && this.gridLeft.Rows.Count > 0 && this.gridRight.Rows.Count == this.gridLeft.Rows.Count)
            {
                this.gridLeft.FirstDisplayedScrollingRowIndex = this.gridRight.FirstDisplayedScrollingRowIndex;
            }

            if (this.gridRight.Columns.Count > 0
                && this.gridTop.Columns.Count > 0)
            {
                this.gridTop.FirstDisplayedScrollingColumnIndex = this.gridRight.FirstDisplayedScrollingColumnIndex;
            }
        }

        private void GridTop_Scroll(object sender, ScrollEventArgs e)
        {
            if (this.gridRight.Columns.Count > 0
                && this.gridTop.Columns.Count > 0 && this.gridRight.Columns.Count == this.gridTop.Columns.Count)
            {
                this.gridRight.FirstDisplayedScrollingColumnIndex = this.gridTop.FirstDisplayedScrollingColumnIndex;
            }
        }

        private void GridLeft_CurrentCellChanged(object sender, EventArgs e)
        {
            if (((BindingSource)this.gridLeft.DataSource).DataSource != null
                && ((BindingSource)this.gridRight.DataSource).DataSource != null
                && this.gridLeft.CurrentCell != null
                && this.gridRight.Rows.Count > this.gridLeft.CurrentCell.RowIndex
                && this.gridRight.CurrentCell.RowIndex != this.gridLeft.CurrentCell.RowIndex)
            {
                this.gridRight.CurrentCell = this.gridRight.Rows[this.gridLeft.CurrentCell.RowIndex].Cells[this.gridRight.FirstDisplayedScrollingColumnIndex];
            }
        }

        private void GridRight_CurrentCellChanged(object sender, EventArgs e)
        {
            if (((BindingSource)this.gridRight.DataSource).DataSource != null
                && ((BindingSource)this.gridLeft.DataSource).DataSource != null
                && this.gridRight.CurrentCell != null
                && this.gridLeft.Rows.Count > this.gridRight.CurrentCell.RowIndex
                && this.gridLeft.CurrentCell.RowIndex != this.gridRight.CurrentCell.RowIndex)
            {
                this.gridLeft.CurrentCell = this.gridLeft.Rows[this.gridRight.CurrentCell.RowIndex].Cells[this.gridLeft.FirstDisplayedScrollingColumnIndex];
            }
        }

        private int index;
        private string find = string.Empty;
        private DataRow[] find_dr;

        private void BtnLocate_Click(object sender, EventArgs e)
        {
            string ftext = this.txtLocate.Text;
            int fint = MyUtility.Convert.GetInt(ftext);
            if (MyUtility.Check.Empty(ftext))
            {
                return;
            }

            string find_new = $@"
Group like '%{ftext}%'
or SubProcessLineID like '%{ftext}%'
or SewingLine like '%{ftext}%'
or StyleID like '%{ftext}%'
or OrderID like '%{ftext}%'
or ShowSewInLine like '%{ftext}%'
or Feature like '%{ftext}%'
or SubProcessLearnCurveID like '%{ftext}%'
or ShowInline like '%{ftext}%'
or ShowOffline like '%{ftext}%'";
            if (!MyUtility.Check.Empty(fint))
            {
                find_new += $@"
or OrderQty = {fint}
or OutputQty = {fint}
or BalanceQty = {fint}
or TargetQty = {fint}
or SMV = {fint}
or EarlyInline = {fint}
or Manpower = {fint}";
            }

            DataTable detDtb = (DataTable)this.listControlBindingSourceLeft.DataSource;

            if (this.find != find_new)
            {
                this.find = find_new;
                this.find_dr = detDtb.Select(find_new);
                if (this.find_dr.Length == 0)
                {
                    MyUtility.Msg.WarningBox("Data not Found.");
                    return;
                }
                else
                {
                    this.index = 0;
                }
            }
            else
            {
                this.index++;
                if (this.find_dr == null)
                {
                    return;
                }

                if (this.index >= this.find_dr.Length)
                {
                    this.index = 0;
                }
            }

            this.listControlBindingSourceLeft.Position = detDtb.Rows.IndexOf(this.find_dr[this.index]);
        }
    }
}