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
    public partial class P04 : Sci.Win.Tems.QueryForm
    {
        public P04(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
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
                .Text("BrandID", header: "Brand", iseditingreadonly: true, width: Widths.AnsiChars(7))
                .Date("SewInLine", header: "Sewing Inline", iseditingreadonly: true, width: Widths.AnsiChars(11))
                .Text("Version", header: "Version", iseditingreadonly: true, width: Widths.AnsiChars(7))
                .Numeric("ttlTimeDiff", header: "Total % time diff", decimal_places: 2, integer_places: 8, width: Widths.AnsiChars(10), iseditingreadonly: true)
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
,o.BrandID
,[SewInLine] = MIN(o.SewInLine)
,[Version] = isnull(lm.Version,'')
,[ttlTimeDiff] = IIF(isnull(lm.TotalGSD,0) = 0, 0, cast( Round( (cast((isnull(lm.TotalGSD,0) - isnull(lm.TotalCycle,0)) as float) / cast(isnull(lm.TotalGSD,0) as float)) * 100 ,2)  as float) )
,[Status] = isnull(lm.Status,'')
,[AddName] = isnull(AddName.IdAndNameAndExt,'')
,[EditName] = isnull(EditName.IdAndNameAndExt,'')
,lm.AddDate
,lm.EditDate
,lm.StyleUkey
,MaxVersion = ROW_NUMBER() over(partition by isnull(lm.StyleID,o.StyleID),isnull(lm.SeasonID,o.SeasonID),isnull(lm.BrandID,o.BrandID),isnull(lm.StyleUKey,o.StyleUKey) order by lm.Version desc)
from Orders o
left join LineMapping lm on lm.StyleUKey = o.StyleUkey

Outer Apply (
    select IdAndNameAndExt
    from dbo.GetPassName(lm.AddName)
)AddName
Outer Apply (
    select IdAndNameAndExt
    from dbo.GetPassName(lm.EditName)
)EditName
where o.Category = 'B'
{strWhere}
group by o.FtyGroup,o.StyleID,o.SeasonID,o.BrandID,lm.Version,lm.Status,AddName.IdAndNameAndExt,EditName.IdAndNameAndExt,lm.AddDate
,lm.TotalGSD,lm.TotalCycle,lm.EditDate,lm.StyleID,lm.SeasonID,lm.BrandID,lm.StyleUKey,o.StyleUkey
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
                this.Grid_Filter();
            }
            else
            {
                this.ShowErr(result);
            }

            this.HideWaitMessage();
        }

        private void Grid_Filter()
        {
            string filter = string.Empty;
            if (this.gridLineMappingStatus.RowCount > 0)
            {
                switch (this.chkAllStyle.Checked)
                {
                    case true:
                        if (MyUtility.Check.Empty(this.gridLineMappingStatus))
                        {
                            break;
                        }

                        if (this.chkLaster.Checked)
                        {
                            filter = @" MaxVersion = 1";
                        }

                        ((DataTable)this.listControlBindingSource1.DataSource).DefaultView.RowFilter = filter;
                        break;

                    case false:
                        if (MyUtility.Check.Empty(this.gridLineMappingStatus))
                        {
                            break;
                        }

                        if (this.chkLaster.Checked)
                        {
                            filter = $@" StyleUkey is null and MaxVersion = 1";
                        }
                        else
                        {
                            filter = $@" StyleUkey is null";
                        }

                        ((DataTable)this.listControlBindingSource1.DataSource).DefaultView.RowFilter = filter;
                        break;
                }
            }
        }

        private void ChkAllStyle_CheckedChanged(object sender, EventArgs e)
        {
            this.Grid_Filter();
        }

        private void ChkLaster_CheckedChanged(object sender, EventArgs e)
        {
            this.Grid_Filter();
        }
    }
}
