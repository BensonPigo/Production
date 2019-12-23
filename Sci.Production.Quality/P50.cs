using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Quality
{
    public partial class P50 : Sci.Win.Tems.QueryForm
    {
        DataTable dtDefectOutput;
        DataTable dtDefectRank;
        public P50(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.txtFactory.Text = Env.User.Factory;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

        }

        private void Query()
        {
            string sqlQuery = $@"
    Create table #TimeRange
(
	ColSerNo     int,
	StartTime datetime,
	EndTime datetime,
	DisplayTitle varchar(20)
)
    
    Declare @_i int = 0
    Declare @Startime datetime = @inputStartime
    Declare @Factory varchar(8) = @inputFactory
    Declare @TimeRangeStart datetime
    Declare @TimeRangeEnd datetime
    Declare @DisplayTitle varchar(20)
    WHILE (@_i < 24)
     BEGIN
    	set @TimeRangeStart = DATEADD( HOUR,@_i,@Startime)
    	set @TimeRangeEnd = DATEADD( HOUR,@_i + 2,@Startime)
    	set @DisplayTitle = Format(@TimeRangeStart,'HH:mm') + '~' + Format(dateadd(SECOND,-1,@TimeRangeEnd),'HH:mm')
    	insert into #TimeRange values(0,@TimeRangeStart,@TimeRangeEnd,@DisplayTitle)
	
	    Set @_i=@_i+ 2
     END
    
    declare @MaxWorkTime datetime
    declare @MinWorkTime datetime
    
    select @MaxWorkTime = max(WorkTime), @MinWorkTime = min(WorkTime)
    from [Dashboard].[dbo].[ReworkTotal] with (nolock)
     where WorkDate = @Startime and FactoryID = @Factory
    
    if(@MaxWorkTime is null)
    begin
    	return
    end
    
    delete #TimeRange where EndTime <= @MinWorkTime or StartTime > @MaxWorkTime
    update #TimeRange set ColSerNo  = updColSerNo.ColSerNo
	from #TimeRange  
	inner join (select StartTime,[ColSerNo] = ROW_NUMBER() OVER (ORDER BY StartTime) + 1 from #TimeRange) updColSerNo on #TimeRange.StartTime = updColSerNo.StartTime


	select  gdc.id
            , gdc.Description
            , tr.DisplayTitle
            , tr.ColSerNo
	        , [DefectQty] = sum(rt.qty)
	into #DefectOutput
	from [Dashboard].[dbo].[ReworkTotal] rt with (nolock)
    inner join #TimeRange tr on rt.WorkTime >= tr.StartTime and rt.WorkTime < tr.EndTime
	inner join GarmentDefectCode gdc on (rt.FailCode = gdc.ID or rt.FailCode = gdc.ReworkTotalFailCode) and gdc.Junk = 0
	where rt.WorkDate = @Startime and rt.FactoryID = @Factory
	Group by rt.WorkDate, gdc.id,gdc.Description, tr.DisplayTitle, tr.ColSerNo
	
	Declare @DisplayTitleCol varchar(400)
	SELECT @DisplayTitleCol =  Stuff((select concat( ',','['+DisplayTitle+']')   from #TimeRange order by StartTime FOR XML PATH('')),1,1,'') 

	Declare @sqlResult nvarchar(max) = N'
	select *
	from ( select   ID,
				    Description,
				    DefectQty,
				    DisplayTitle
				    from #DefectOutput 
        )a
	PIVOT   
	(  
		sum(DefectQty)  
		FOR DisplayTitle IN ('+ @DisplayTitleCol +')  
	) AS b	
	order by ID
	'
	select *
	from (	select  DisplayTitle,
			        ID,
			        Description,
			        DefectQty,
			        [DefectRank] = RANK() OVER (PARTITION BY DisplayTitle ORDER BY DefectQty desc),
                    ColSerNo
			from #DefectOutput ) a
	where DefectRank <= 5

	exec sp_executesql @sqlResult
";
            DataTable[] dtResults;
            DualResult result = DBProxy.Current.Select(null,
                sqlQuery,
                new List<SqlParameter>() {  new SqlParameter("@inputStartime", this.dateBoxOutputDate.Value),
                                            new SqlParameter("@inputFactory", this.txtFactory.Text)},
                out dtResults);

            if(!result)
            {
                this.ShowErr(result);
                return;
            }

            if (dtResults.Length == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return;
            }

            this.dtDefectRank = dtResults[0];
            this.dtDefectOutput = dtResults[1];
            this.AdjustDataGridViewColumn(this.dtDefectOutput, this.gridDefectOutput);
            this.bindingGridDefectOutput.DataSource = this.dtDefectOutput;
            this.gridDefectOutput.AutoResizeColumns();
            this.gridDefectOutput.Columns["Description"].Width = 215;
            this.GridDefectTop5CellColorChange();
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.dateBoxOutputDate.Value))
            {
                MyUtility.Msg.WarningBox("<OutputDate> must be entered");
                return;
            }

            if (MyUtility.Check.Empty(this.txtFactory.Text))
            {
                MyUtility.Msg.WarningBox("<Factory> must be entered");
                return;
            }

            this.Query();
        }

        private void AdjustDataGridViewColumn(DataTable dataTable, DataGridView dataGridView)
        {
            dataGridView.Columns.Clear();


            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                DataColumn dc = dataTable.Columns[i];

                DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
                col.Name = dc.ColumnName;
                col.ValueType = dc.DataType;
                if (i > 1)
                {
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
                col.DataPropertyName = dc.ColumnName;
                col.HeaderText = dc.Caption;
                col.Visible = true;
                dataGridView.Columns.Add(col);
            }
        }

        private void GridDefectTop5CellColorChange()
        {
            var listDefectRank = this.dtDefectRank.AsEnumerable();
            foreach (DataGridViewRow gridRow in this.gridDefectOutput.Rows)
            {
                foreach (DataGridViewCell gridCell in gridRow.Cells)
                {
                    if (gridCell.ColumnIndex < 2)
                    {
                        continue;
                    }

                    bool isTop5Defect = listDefectRank
                                        .Any(s => (int)s["ColSerNo"] == gridCell.ColumnIndex &&
                                                    s["ID"].ToString() == gridRow.Cells[0].Value.ToString());
                    if (isTop5Defect)
                    {
                        gridCell.Style.ForeColor = Color.Red;
                    }
                }
            }
        }

        private void gridDefectOutput_Sorted(object sender, EventArgs e)
        {
            this.GridDefectTop5CellColorChange();
        }
    }
}
