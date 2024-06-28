using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.IE
{
    /// <inheritdoc/>
    public partial class P04 : Sci.Win.Tems.QueryForm
	{
        /// <inheritdoc/>
        public P04(ToolStripMenuItem menuitem)
			: base(menuitem)
		{
			this.InitializeComponent();
			this.EditMode = true;
		}

		protected override void OnFormLoaded()
		{
			base.OnFormLoaded();

			#region 欄位設定
			this.Helper.Controls.Grid.Generator(this.gridLineMappingStatus)
				.Text("FtyGroup", header: "Factory", iseditingreadonly: true, width: Widths.AnsiChars(7))
				.Text("StyleID", header: "Style#", iseditingreadonly: true, width: Widths.AnsiChars(14))
				.Text("SeasonID", header: "Season", iseditingreadonly: true, width: Widths.AnsiChars(7))
                .Text("CPU", header: "CPU/pc", iseditingreadonly: true, width: Widths.AnsiChars(7))
                .Text("BrandID", header: "Brand", iseditingreadonly: true, width: Widths.AnsiChars(7))
				.Date("SewInLine", header: "Sewing Inline", iseditingreadonly: true, width: Widths.AnsiChars(11))
                .Text("LineMapping", header: "Line Mapping", iseditingreadonly: true, width: Widths.AnsiChars(7))
                .Text("Version", header: "Version", iseditingreadonly: true, width: Widths.AnsiChars(7))
				.Text("Phase", header: "Phase", iseditingreadonly: true, width: Widths.AnsiChars(7))
				.Text("SewingLineID", header: "Sewing Line", iseditingreadonly: true, width: Widths.AnsiChars(7))
				.Text("Team", header: "Team", iseditingreadonly: true, width: Widths.AnsiChars(5))
				.Numeric("ttlTimeDiff", header: "Total % time diff", decimal_places: 2, integer_places: 8, width: Widths.AnsiChars(10), iseditingreadonly: true)
				.Numeric("LBR", header: "LBR", decimal_places: 2, width: Widths.AnsiChars(5), iseditingreadonly: true)
				.Text("Status", header: "Status", iseditingreadonly: true, width: Widths.AnsiChars(9))
				.Text("AddName", header: "Add Name", iseditingreadonly: true, width: Widths.AnsiChars(23))
				.Text("EditName", header: "Edit Name", iseditingreadonly: true, width: Widths.AnsiChars(23))
				.DateTime("AddDate", header: "Add Date", iseditingreadonly: true)
				.DateTime("EditDate", header: "Edit Date", iseditingreadonly: true)
				;
			#endregion
		}

		private void BtnQuery_Click(object sender, EventArgs e)
		{
			if (MyUtility.Check.Empty(this.dateInlineDate.Value1) || MyUtility.Check.Empty(this.dateInlineDate.Value2))
			{
				MyUtility.Msg.WarningBox("Please fill < Sewing Inline >.");
				this.dateInlineDate.Focus();
				return;
			}

			this.Query();
		}

		private void Query()
		{
			DataTable dtData = new DataTable();
			this.listControlBindingSource1.DataSource = null;
			string strWhere = "";
			if (!MyUtility.Check.Empty(this.dateInlineDate.Value1) && !MyUtility.Check.Empty(this.dateInlineDate.Value2))
			{
				strWhere += $@" and o.SewInLine between '{string.Format("{0:yyyy-MM-dd}", this.dateInlineDate.Value1)}' and '{string.Format("{0:yyyy-MM-dd}", this.dateInlineDate.Value2)}'" + Environment.NewLine;
			}

            if (!MyUtility.Check.Empty(this.txtfty.Text))
			{
				strWhere += $@" and o.FtyGroup = '{this.txtfty.Text}'";
			}

			string sqlcmd = $@"
select o.FtyGroup
,o.StyleID
,o.SeasonID
,[CPU] = isnull(s.CPU,0)
,o.BrandID
,[LineMapping] = 'IE P03'
,[SewInLine] = MIN(o.SewInLine)
,[Version] = isnull(lm.Version,'')
,[Phase] = isnull(lm.Phase,'')
,[SewingLineID] = isnull(lm.SewingLineID,'')
,[Team] = isnull(lm.Team,'')
,[ttlTimeDiff] = IIF(isnull(lm.TotalGSD,0) = 0, 0, cast( Round( (cast((isnull(lm.TotalGSD,0) - isnull(lm.TotalCycle,0)) as float) / cast(isnull(lm.TotalGSD,0) as float)) * 100, 2)  as float) )
,[LBR] = IIF(isnull(lm.HighestCycle,0) * isnull(lm.CurrentOperators,0) = 0, 0,  cast( Round(isnull(lm.TotalCycle,0)/isnull(lm.HighestCycle,0)/isnull(lm.CurrentOperators,0) * 100, 2) as float))
,[Status] = isnull(lm.Status,'')
,[AddName] = isnull(AddName.IdAndNameAndExt,'')
,[EditName] = isnull(EditName.IdAndNameAndExt,'')
,lm.AddDate
,lm.EditDate
,lm.StyleUkey
,MaxVersion = ROW_NUMBER() over(partition by 
								isnull(f.FTYGroup,o.FtyGroup)
								,isnull(lm.StyleID,o.StyleID)
								,isnull(lm.SeasonID,o.SeasonID)
								,isnull(lm.BrandID,o.BrandID)
								,isnull(lm.Phase,'')
								,isnull(lm.SewingLineID,'')
								,isnull(lm.Team,'')
								,isnull(lm.StyleUKey,o.StyleUKey) 
								order by lm.Version desc)
from Orders o
inner join factory f on f.id=o.FactoryID
left join LineMapping lm on lm.StyleUKey = o.StyleUkey
left join Style s on s.Ukey = lm.StyleUKey
Outer Apply (
    select IdAndNameAndExt
    from dbo.GetPassName(lm.AddName)
)AddName
Outer Apply (
    select IdAndNameAndExt
    from dbo.GetPassName(lm.EditName)
)EditName
where o.Category = 'B' and f.IsProduceFty = 1
{strWhere}
group by o.FtyGroup,o.StyleID,o.SeasonID,o.BrandID,lm.Version,lm.Status,AddName.IdAndNameAndExt,EditName.IdAndNameAndExt,lm.AddDate
,lm.TotalGSD,lm.TotalCycle,lm.EditDate,lm.StyleID,lm.SeasonID,lm.BrandID,lm.StyleUKey,o.StyleUkey
,lm.Phase,lm.SewingLineID,lm.Team,lm.HighestCycle,lm.CurrentOperators
,f.FTYGroup,s.CPU

union

select o.FtyGroup
,o.StyleID
,o.SeasonID
,[CPU] = isnull(s.CPU,0)
,o.BrandID
,[LineMapping] = 'IE P06'
,[SewInLine] = MIN(o.SewInLine)
,[Version] = isnull(lmb.Version,'')
,[Phase] = isnull(lmb.Phase,'')
,[SewingLineID] = isnull(lmb.SewingLineID,'')
,[Team] = isnull(lmb.Team,'')
,[ttlTimeDiff] = IIF(isnull(lmb.TotalGSDTime,0) = 0, 0, cast( Round( (cast((isnull(lmb.TotalGSDTime,0) - isnull(lmb.TotalCycleTime,0)) as float) / cast(isnull(lmb.TotalGSDTime,0) as float)) * 100, 2)  as float) )
,[LBR] = IIF(isnull(lmb.HighestCycleTime,0) * 0 = 0, 0,  cast( Round(isnull(lmb.TotalCycleTime,0)/isnull(lmb.HighestCycleTime,0) / 0 * 100, 2) as float))
,[Status] = isnull(lmb.Status,'')
,[AddName] = isnull(AddName.IdAndNameAndExt,'')
,[EditName] = isnull(EditName.IdAndNameAndExt,'')
,lmb.AddDate
,lmb.EditDate
,lmb.StyleUkey
,MaxVersion = ROW_NUMBER() over(partition by 
								isnull(f.FTYGroup,o.FtyGroup)
								,isnull(lmb.StyleID,o.StyleID)
								,isnull(lmb.SeasonID,o.SeasonID)
								,isnull(lmb.BrandID,o.BrandID)
								,isnull(lmb.Phase,'')
								,isnull(lmb.SewingLineID,'')
								,isnull(lmb.Team,'')
								,isnull(lmb.StyleUKey,o.StyleUKey) 
								order by lmb.Version desc)
from Orders o
inner join factory f on f.id=o.FactoryID
inner join LineMappingBalancing lmb on lmb.StyleUKey = o.StyleUkey
LEFT join Style s on s.Ukey = lmb.StyleUKey
Outer Apply (
    select IdAndNameAndExt
    from dbo.GetPassName(lmb.AddName)
)AddName
Outer Apply (
    select IdAndNameAndExt
    from dbo.GetPassName(lmb.EditName)
)EditName
where o.Category = 'B' and f.IsProduceFty = 1
{strWhere}
group by o.FtyGroup,o.StyleID,o.SeasonID,o.BrandID,lmb.Version,lmb.Status,AddName.IdAndNameAndExt,EditName.IdAndNameAndExt,lmb.AddDate
,lmb.TotalGSDTime,lmb.TotalCycleTime,lmb.EditDate,lmb.StyleID,lmb.SeasonID,lmb.BrandID,lmb.StyleUKey,o.StyleUkey
,lmb.Phase,lmb.SewingLineID,lmb.Team,lmb.HighestCycleTime --,lmb.CurrentOperators
,f.FTYGroup,s.CPU

union 
select o.FtyGroup
,o.StyleID
,o.SeasonID
,[CPU] = isnull(s.CPU,0)
,o.BrandID
,[LineMapping] = 'IE P05'
,[SewInLine] = MIN(o.SewInLine)
,[Version] = isnull(alm.Version,'')
,[Phase] = isnull(alm.Phase,'')
,[SewingLineID] = ''
,[Team] = ''
,[ttlTimeDiff] = 0
,[LBR] = 0
,[Status] = isnull(alm.Status,'')
,[AddName] = isnull(AddName.IdAndNameAndExt,'')
,[EditName] = isnull(EditName.IdAndNameAndExt,'')
,alm.AddDate
,alm.EditDate
,alm.StyleUkey
,MaxVersion = ROW_NUMBER() over(partition by 
								isnull(f.FTYGroup,o.FtyGroup)
								,isnull(alm.StyleID,o.StyleID)
								,isnull(alm.SeasonID,o.SeasonID)
								,isnull(alm.BrandID,o.BrandID)
								,isnull(alm.Phase,'')
								,isnull(alm.StyleUKey,o.StyleUKey) 
								order by alm.Version desc)
from Orders o
inner join factory f on f.id=o.FactoryID
inner join AutomatedLineMapping alm on alm.StyleUKey = o.StyleUkey
LEFT join Style s on s.Ukey = alm.StyleUKey
Outer Apply (
    select IdAndNameAndExt
    from dbo.GetPassName(alm.AddName)
)AddName
Outer Apply (
    select IdAndNameAndExt
    from dbo.GetPassName(alm.EditName)
)EditName
where o.Category = 'B' and f.IsProduceFty = 1
{strWhere}
group by o.FtyGroup,o.StyleID,o.SeasonID,o.BrandID,alm.Version,alm.Status,AddName.IdAndNameAndExt,EditName.IdAndNameAndExt,alm.AddDate
,alm.EditDate,alm.StyleID,alm.SeasonID,alm.BrandID,alm.StyleUKey,o.StyleUkey
,alm.Phase,s.CPU
,f.FTYGroup
order by o.FtyGroup,o.StyleID,o.SeasonID,o.BrandID

";
			this.ShowWaitMessage("Data Loading....");

			DualResult result;
			if (result = DBProxy.Current.Select(null, sqlcmd, out dtData))
			{
				if (dtData.Rows.Count == 0)
				{
					MyUtility.Msg.WarningBox("Data not found!!");
				}

				this.listControlBindingSource1.DataSource = dtData;
				// this.Grid_Filter();
			}
			else
			{
				this.ShowErr(result);
			}

			this.HideWaitMessage();
		}

        /// <inheritdoc/>
        private void Grid_Filter()
		{
			string filter = string.Empty;
			if (this.gridLineMappingStatus.RowCount > 0)
			{

				if (this.chkLaster.Checked)
				{
					filter = $@" MaxVersion = 1";
				}
				else
				{
					filter = string.Empty;
				}

				((DataTable)this.listControlBindingSource1.DataSource).DefaultView.RowFilter = filter;
			}
		}


		private void ChkLaster_CheckedChanged(object sender, EventArgs e)
		{
			this.Grid_Filter();
		}
	}
}
