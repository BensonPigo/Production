using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Prg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Quality
{
    /// <inheritdoc/>
    public partial class P30_MDFailbyCarton : Win.Tems.QueryForm
    {
        private string sp;

        /// <inheritdoc/>
        public P30_MDFailbyCarton(string sp)
        {
            this.InitializeComponent();
            this.sp = sp;
            this.grid.CellPainting += this.Grid_CellPainting;
            this.grid.CellFormatting += this.Grid_CellFormatting;
        }

        private void Grid_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex == this.grid.Rows.Count - 1 || e.ColumnIndex < 0)
            {
                return;
            }

            if (this.grid.Columns[e.ColumnIndex].Name != "ID")
            {
                return;
            }

            e.AdvancedBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
            if (this.IsTheSameNextCellValue("ID", e.RowIndex, this.grid))
            {
                e.AdvancedBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.None;
            }
            else
            {
                e.AdvancedBorderStyle.Bottom = this.grid.AdvancedCellBorderStyle.Bottom;
            }
        }

        private void Grid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex == 0 || this.grid.Columns[e.ColumnIndex].Name != "ID")
            {
                return;
            }

            if (this.IsTheSamePreviousCellValue("ID", e.RowIndex, this.grid))
            {
                e.Value = string.Empty;
                e.FormattingApplied = true;
            }
        }

        private bool IsTheSamePreviousCellValue(string column, int row, DataGridView tarGrid)
        {
            DataGridViewCell cell1 = tarGrid[column, row];
            DataGridViewCell cell2 = tarGrid[column, row - 1];
            if (cell1.Value == null || cell2.Value == null)
            {
                return false;
            }

            return cell1.Value.ToString() == cell2.Value.ToString();
        }

        private bool IsTheSameNextCellValue(string column, int row, DataGridView tarGrid)
        {
            DataGridViewCell cell1 = tarGrid[column, row];
            DataGridViewCell cell2 = tarGrid[column, row + 1];
            if (cell1.Value == null || cell2.Value == null)
            {
                return false;
            }

            return cell1.Value.ToString() == cell2.Value.ToString();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.GridSetup();
            this.Query();
        }

        private void GridSetup()
        {
            this.Helper.Controls.Grid.Generator(this.grid)
                .Text("ID", header: "Pack ID", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("CTNStartNo", header: "CTN#", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("Article", header: "Article", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("ShipQty", header: "Qty", width: Widths.AnsiChars(10), decimal_places: 0, iseditingreadonly: true)
                .Numeric("PackingErrQty1st", header: "First MD Fail Qty", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("PackingErrQty", header: "Current MD Fail Qty", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("PassRate", header: "Pass Rate", width: Widths.AnsiChars(10), iseditingreadonly: true)
                ;

            this.Helper.Controls.Grid.Generator(this.grid1)
                .DateTime("AddDate", header: "Transfer Packing Error Date", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("AddName", header: "Transferred By", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Numeric("ErrQty", header: "ErrQty", width: Widths.AnsiChars(10), decimal_places: 0, iseditingreadonly: true)
                .Text("ErrorType", header: "Error Type", width: Widths.AnsiChars(40), iseditingreadonly: true)
                ;
        }

        private void Query()
        {
            string sqlcmd = $@"
select pd.id,pd.CTNStartNo,
    ShipQty = sum(pd.ShipQty),
	PackingErrQty = iif(pd.PackingErrorID = '00006', pd.PackingErrQty, 0),
	ScanQty = sum(iif(pd.ScanQty > pd.ShipQty, pd.ShipQty, pd.ScanQty))
into #tmp
from PackingList_Detail pd with(nolock)
where pd.OrderID = '{this.sp}'
group by pd.id,pd.CTNStartNo,iif(pd.PackingErrorID = '00006', pd.PackingErrQty, 0)

select *,
	PassRate =
	    case when isnull(ShipQty, 0) = 0 then concat(0, '%')
		when isnull(ScanQty, 0) < isnull(ShipQty, 0) then Null
	    else concat(round((isnull(ShipQty, 0) - isnull(PackingErrQty1st, 0)) / cast(isnull(ShipQty, 0)as float) * 100, 2),  '%')
	    end
from #tmp pd
outer apply(select top 1 PackingErrQty1st = ErrQty from PackErrTransfer pe with(nolock) where pe.PackingListID = pd.id and pe.CTNStartNo = pd.CTNStartNo and pe.PackingErrorID  = '00006' order by AddDate)ErrQty
outer apply(select Article = stuff((select distinct concat(',', Article) from PackingList_Detail pd2 with(nolock) where pd2.id = pd.id and pd2.CTNStartNo = pd.CTNStartNo for xml path('')),1,1,''))Article
outer apply(select SizeCode = stuff((select distinct concat(',', SizeCode) from PackingList_Detail pd2 with(nolock) where pd2.id = pd.id and pd2.CTNStartNo = pd.CTNStartNo for xml path('')),1,1,''))SizeCode

-- CTN# 若是數值先依照數值排序
order by pd.id,iif(ISNUMERIC(CTNStartno)=0,'ZZZZZZZZ',RIGHT(REPLICATE('0', 8) + CTNStartno, 8)), RIGHT(REPLICATE('0', 8) + CTNStartno, 8)


select pe.PackingListID,pe.CTNStartNo,pe.AddDate,pe.AddName,pe.ErrQty, ErrorType = concat(pe.PackingErrorID, '-' + per.Description)
from #tmp pd
inner join PackErrTransfer pe with(nolock) on pe.PackingListID = pd.id and pe.CTNStartNo = pd.CTNStartNo
left join PackingError per on per.ID = pe.PackingErrorID and per.Type = 'TP'
order by AddDate

drop table #tmp
";

            DualResult res = DBProxy.Current.Select(null, sqlcmd, out DataTable[] dt);
            if (!res)
            {
                this.ShowErr(res);
            }

            this.listControlBindingSource.DataSource = dt[0];
            this.listControlBindingSource1.DataSource = dt[1];

            DataRow[] drs = dt[0].Select($"PassRate is not null");
            if (drs.Length == 0)
            {
                this.txtPassRate.Text = string.Empty;
            }
            else
            {
                decimal shipQty = drs.AsEnumerable().Sum(s => MyUtility.Convert.GetDecimal(s["ShipQty"]));
                decimal errQty1st = drs.AsEnumerable().Sum(s => MyUtility.Convert.GetDecimal(s["PackingErrQty1st"]));
                this.txtPassRate.Text = $"{Math.Round((shipQty - errQty1st) / shipQty * 100, 2)}%";
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Grid_SelectionChanged(object sender, EventArgs e)
        {
            if (this.grid.CurrentDataRow != null)
            {
                this.listControlBindingSource1.Filter = $"PackingListID = '{this.grid.CurrentDataRow["ID"]}' and CTNStartNo = '{this.grid.CurrentDataRow["CTNStartNo"]}'";
            }
        }
    }
}
