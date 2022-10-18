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
using System.Windows.Forms.VisualStyles;

namespace Sci.Production.PPIC
{
    public partial class P23 : Sci.Win.Tems.QueryForm
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="P23"/> class.
        /// </summary>
        /// <param name="menuitem"></param>
        public P23(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.EditMode = true;
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtStyle.Text) || MyUtility.Check.Empty(this.comboBrand.Text))
            {
                MyUtility.Msg.WarningBox("Style# & Brand cannot be empty.");
                return;
            }

            // filter
            if (MyUtility.Check.Empty(this.listControlBindingSource1.DataSource))
            {
                return;
            }

            int index = this.listControlBindingSource1.Find("StyleBrand", this.txtStyle.Text.Trim() + this.comboBrand.Text);
            if (index == -1)
            {
                MyUtility.Msg.WarningBox("Data not found! ");
            }
            else
            {
                this.listControlBindingSource1.Position = index;
            }
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            // combo Datasource
            Ict.DualResult cbResult;
            if (cbResult = DBProxy.Current.Select(null, @"Select ID from Brand where Junk = 0", out DataTable dtCountry))
            {
                this.comboBrand.DataSource = dtCountry;
                this.comboBrand.DisplayMember = "ID";
                this.comboBrand.ValueMember = "ID";
            }
            else
            {
                this.ShowErr(cbResult);
            }

            this.comboBrand.Text = "ADIDAS";

            // Grid設定
            this.grid.IsEditingReadOnly = false;
            this.grid.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.grid)
                .Text("StyleID", header: "Style", width: Widths.AnsiChars(25), iseditable: false)
                .Text("SeasonID", header: "Season", width: Widths.AnsiChars(15), iseditable: false)
                .Numeric("ForecastQty", header: "Forecast Qty", width: Widths.AnsiChars(20), iseditable: false)
                .Numeric("OrderQty", header: "Order Qty", width: Widths.AnsiChars(20), iseditable: false)
                .Text("Status", header: "Status", width: Widths.AnsiChars(15), iseditable: false)
                ;

            this.grid.CellDoubleClick += this.Grid_CellDoubleClick;

            // 一進入畫面, 先開啟Data Filter 選擇Season
            P23_DataFilter frm = new P23_DataFilter();
            frm.ShowDialog(this);
            this.txtSeason.Text = P23_DataFilter.Season;
            this.Find();
        }

        private P24 callP24 = null;
        private void Grid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            this.grid.ValidateControl();
            DataRow dr = this.grid.GetDataRow(e.RowIndex);

            if (this.callP24 != null && this.callP24.Visible == true)
            {
                this.callP24.P24Call(dr["StyleID"].ToString(), dr["SeasonID"].ToString(), "ADIDAS");
                this.callP24.Activate();
            }
            else
            {
                this.P24FormOpen(dr["StyleID"].ToString(), dr["SeasonID"].ToString(), "ADIDAS");
            }
        }

        private void P24FormOpen(string style, string season, string brand)
        {
            ToolStripMenuItem p24MenuItem = null;
            foreach (ToolStripMenuItem toolMenuItem in Env.App.MainMenuStrip.Items)
            {
                if (toolMenuItem.Text.EqualString("PPIC"))
                {
                    foreach (var subMenuItem in toolMenuItem.DropDown.Items)
                    {
                        if (subMenuItem.GetType().Equals(typeof(ToolStripMenuItem)))
                        {
                            if (((ToolStripMenuItem)subMenuItem).Text.EqualString("P24. Query Handover List"))
                            {
                                p24MenuItem = (ToolStripMenuItem)subMenuItem;
                                break;
                            }
                        }
                    }
                }
            }

            this.callP24 = new P24(style, season, brand, p24MenuItem);
            this.callP24.MdiParent = this.MdiParent;
            this.callP24.Show();
        }

        private void btnDataFilter_Click(object sender, EventArgs e)
        {
            P23_DataFilter frm = new P23_DataFilter();
            frm.ShowDialog(this);
            this.txtSeason.Text = P23_DataFilter.Season;
            this.Find();
        }

        private void Find()
        {
            string sqlcmd = $@"
select a.StyleID, a.SeasonID
,[ForecastQty] = sum(a.ForecastQty)
,[OrderQty] = sum(a.OrderQty)
,[Status] = iif(StatusCnt.cnt > 1, 'DONE', 'ON-GOING') 
,[StyleBrand] = Ltrim(Rtrim(a.StyleID)) + Ltrim(Rtrim(a.BrandID))
into #tmp
from (
	select o.StyleID,o.SeasonID,o.BrandID,o.StyleUkey, iif(o.Category = '', o.Qty, 0) as ForecastQty, iif(o.Category = 'B', o.Qty, 0) as OrderQty
	from Season s
	inner join Orders o on o.SeasonID = s.ID and o.BrandID = s.BrandID
	inner join Factory f on o.FactoryID = f.ID
	inner join System sy on sy.RgCode = f.NegoRegion
	where 1=1
	and s.SeasonSCIID = '{this.txtSeason.Text}'
	and o.BrandID = 'ADIDAS'
	and o.LocalOrder = 0
	and o.Category in ('B','S','')
	and o.Junk = 0
) a
outer apply(
	select cnt = Count(ID)
    from Orders
    where Category in ('B','S')
    and BrandID = 'ADIDAS'
    and SeasonID = a.SeasonID
    and StyleID = a.StyleID
    and OrderTypeID in ('SEALING SPL','SEALING SPL W/O ETA','SIZE SET','BULK KEEPING')
    and LocalOrder = 0
)StatusCnt
group by a.StyleID, a.SeasonID,StatusCnt.cnt,a.BrandID

select *
,[ttlStyleCnt] = (select cnt = count(1) from (select distinct StyleID from #tmp)a )
,[DoneCnt] = ROUND(cast((select cnt = count(1) from (select distinct StyleID from #tmp where Status = 'DONE' )a ) as float) / (select cnt = count(1) from (select distinct StyleID from #tmp)a ) ,4) * 100
,[OnGoingCnt] = 100 - ROUND(cast((select cnt = count(1) from (select distinct StyleID from #tmp where Status = 'DONE' )a ) as float) / (select cnt = count(1) from (select distinct StyleID from #tmp)a ) ,4) * 100
from #tmp

drop table #tmp
";

            DualResult result = DBProxy.Current.Select(null, sqlcmd, out DataTable dtGrid);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Query data fail.\r\n" + result.ToString());
            }

            this.listControlBindingSource1.DataSource = dtGrid;

            if (dtGrid != null && dtGrid.Rows.Count > 0)
            {
                this.labTtlStyles.Text = dtGrid.Rows[0]["ttlStyleCnt"].ToString();
                this.labDone.Text = dtGrid.Rows[0]["DoneCnt"].ToString() + "%";
                this.labonGoing.Text = dtGrid.Rows[0]["OnGoingCnt"].ToString() + "%";
            }
            else
            {
                this.labTtlStyles.Text = "0";
                this.labDone.Text = "0%";
                this.labonGoing.Text = "0%";
            }
        }

        private void txtStyle_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            if (this.listControlBindingSource1 != null)
            {
                DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
                if (dt.Rows.Count > 0)
                {
                    Win.Tools.SelectItem item;
                    string sqlcmd = $@"
select distinct StyleID from #tmp
";
                    MyUtility.Tool.ProcessWithDatatable(dt, string.Empty, sqlcmd, out DataTable dtStyle);
                    item = new Win.Tools.SelectItem(dtStyle, "StyleID", "16", this.Text);
                    DialogResult returnResult = item.ShowDialog();
                    if (returnResult == DialogResult.Cancel)
                    {
                        return;
                    }

                    this.txtStyle.Text = item.GetSelectedString();
                }
            }
        }
    }
}
