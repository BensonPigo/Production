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
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Subcon
{
    public partial class P40 : Win.Tems.QueryForm
    {
        private Ict.Win.UI.DataGridViewTextBoxColumn col_StatusColor;
        List<string> sqlWhere = new List<string>();
        string InlineDate1; string InlineDate2;
        private DataTable dtFormGrid;
        private DataTable dtExcel;
        private DataTable dtBundleGroupQty;

        private DataSet ds;
        private SqlCommand cmd;
        private bool cancel;

        public P40(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.EditMode = true;
            this.txtfactory.Text = Sci.Env.User.Factory;
            this.displaySubProcess.Text = "Loading";
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.grid.IsEditingReadOnly = true;
            DataGridViewGeneratorTextColumnSettings bundleGroup = new DataGridViewGeneratorTextColumnSettings();
            bundleGroup.EditingMouseDown += (s, d) =>
            {
                if (d.Button != MouseButtons.Right)
                {
                    return;
                }

                DataRow drSelected = this.grid.GetDataRow(d.RowIndex);

                if (drSelected == null)
                {
                    return;
                }

                this.ShowBundleGroupDetailQty(drSelected);
            };

            bundleGroup.CellMouseDoubleClick += (s, d) =>
            {
                DataRow drSelected = this.grid.GetDataRow(d.RowIndex);

                if (drSelected == null)
                {
                    return;
                }

                this.ShowBundleGroupDetailQty(drSelected);
            };

            this.Helper.Controls.Grid.Generator(this.grid)
            .Text("FactoryID", header: "Factory", width: Widths.Auto(true))
            .Text("LocationID", header: "Location", width: Widths.Auto(true))
            .Text("OrderID", header: "SP No.", width: Widths.AnsiChars(14))
            .Text("POID", header: "Master SP", width: Widths.AnsiChars(14))
            .Text("Line", header: "Inline Line#", width: Widths.Auto(true))
            .Text("StyleID", header: "Style", width: Widths.AnsiChars(15))
            .Text("BundleGroup", header: "Group", width: Widths.AnsiChars(20), settings: bundleGroup)
            .Text("FComb", header: "Comb", width: Widths.Auto(true))
            .Text("ColorID", header: "Color", width: Widths.Auto(true))
            .Text("Pattern", header: "Pattern", width: Widths.AnsiChars(10))
            .Text("PtnDes", header: "PtnDesc", width: Widths.AnsiChars(10))
            .Text("Size", header: "Size", width: Widths.Auto(true))
            .Text("Artwork", header: "Artwork", width: Widths.Auto(true))
            .Text("OrderQty", header: "Order Qty per Size", width: Widths.Auto(true))
            .Numeric("LoadingQty", header: "Accu. Loading Qty" + Environment.NewLine + "per Parts", width: Widths.Auto(true), decimal_places: 2)
            .Numeric("Balance", header: "Balance Qty", width: Widths.Auto(true), decimal_places: 2)
            .Text("Status", header: "Status", width: Widths.AnsiChars(8)).Get(out this.col_StatusColor)
            ;
            this.Change_Color();
        }

        private void Change_Color()
        {
            this.col_StatusColor.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.grid.GetDataRow(e.RowIndex);
                if (dr == null)
                {
                    return;
                }

                string status = dr["Status"].ToString();
                switch (status)
                {
                    case "Lacking":
                        e.CellStyle.BackColor = Color.Red;
                        break;

                    case "Complete":
                        e.CellStyle.BackColor = Color.Green;
                        break;
                    case "Excess":
                        e.CellStyle.BackColor = Color.Yellow;
                        break;
                }
            };
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            if ((MyUtility.Check.Empty(this.txtSp1.Text) || MyUtility.Check.Empty(this.txtSp2.Text)) &&
               (MyUtility.Check.Empty(this.dateRangeInlineDate.Value1) || MyUtility.Check.Empty(this.dateRangeInlineDate.Value2)) &&
               MyUtility.Check.Empty(this.txtLocation.Text))
            {
                MyUtility.Msg.WarningBox(@"SP#, Inline Date and Location cannot all be empty.");
                return;
            }

            if (!MyUtility.Check.Empty(this.txtSp1.Text) || !MyUtility.Check.Empty(this.txtSp2.Text))
            {
                if (MyUtility.Check.Empty(this.txtSp1.Text) || MyUtility.Check.Empty(this.txtSp2.Text))
                {
                    MyUtility.Msg.WarningBox(@"Must enter SP# value1 and value2");
                    return;
                }
            }

            // Query時,判斷sql並非Cancel狀態
            this.cancel = false;

            if (!this.bgWorkerUpdateInfo.IsBusy)
            {
                // 子執行緒開始執行
                this.btnQuery.Enabled = false;
                this.bgWorkerUpdateInfo.RunWorkerAsync();
                this.bgWorkerUpdateInfo.WorkerReportsProgress = true;
            }
        }

        private void bgWorkerUpdateInfo_DoWork(object sender, DoWorkEventArgs e)
        {
            if (this.bgWorkerUpdateInfo.CancellationPending == true)
            {
                e.Cancel = true;
            }
            else
            {
                this.Query();
                this.bgWorkerUpdateInfo.ReportProgress(1);
            }
        }

        private void Query()
        {
            this.ShowLoadingText("Data Loading...");
            this.UseWaitCursor = true;
            this.sqlWhere.Clear();
            this.InlineDate1 = this.dateRangeInlineDate.Value1.Empty() ? string.Empty : ((DateTime)this.dateRangeInlineDate.Value1).ToString("yyyy/MM/dd");
            this.InlineDate2 = this.dateRangeInlineDate.Value2.Empty() ? string.Empty : ((DateTime)this.dateRangeInlineDate.Value2).ToString("yyyy/MM/dd");

            #region SqlWhere
            if (!MyUtility.Check.Empty(this.InlineDate1) && !MyUtility.Check.Empty(this.InlineDate2))
            {
                this.sqlWhere.Add($@"and o.SewInLine between '{this.InlineDate1}' and '{this.InlineDate2}'");
            }

            if (!MyUtility.Check.Empty(this.txtSp1.Text) && !MyUtility.Check.Empty(this.txtSp2.Text))
            {
                this.sqlWhere.Add($@"and b.Orderid between '{this.txtSp1.Text}' and '{this.txtSp2.Text}'");
            }

            if (!MyUtility.Check.Empty(this.txtLocation.Text))
            {
                this.sqlWhere.Add($@"and bio.LocationID = '{this.txtLocation.Text}' ");
            }

            if (!MyUtility.Check.Empty(this.txtfactory.Text))
            {
                this.sqlWhere.Add($@"and o.FtyGroup = '{this.txtfactory.Text}'");
            }

            if (!MyUtility.Check.Empty(this.txtsewingline.Text))
            {
                this.sqlWhere.Add($@"
and exists (select 1
from SewingSchedule_Detail ssd
where b.Orderid = ssd.OrderID
		and bd.SizeCode = ssd.SizeCode
		and ssd.SewingLineID = '{this.txtsewingline.Text}')");
            }

            if (!MyUtility.Check.Empty(this.txtCombo.Text))
            {
                this.sqlWhere.Add($@"and b.PatternPanel = '{this.txtCombo.Text}'");
            }

            if (!MyUtility.Check.Empty(this.txtArticle.Text))
            {
                this.sqlWhere.Add($@"and b.Article = '{this.txtArticle.Text}' ");
            }

            if (!MyUtility.Check.Empty(this.txtSize.Text))
            {
                this.sqlWhere.Add($@"and bd.SizeCode = '{this.txtSize.Text}' ");
            }
            #endregion

            #region SqlCom
            string sqlcmd = $@"
use Production;

-- 準備 Bundle 相關資訊 --
select	LocationID = isnull (bio.LocationID, '')
		, b.Orderid
		, FComb = b.PatternPanel
		, b.Colorid
		, Pattern = bd.Patterncode
		, PtnDes = bd.PatternDesc
		, Size = bd.SizeCode
		, Artwork = isnull (bda.v, '')
		, Qty = case 
					when isnull (bio.BundleNo, '') = '' then 0
					else isnull (bd.Qty, 0)
				end
		, bd.IsPair
        , bd.BundleGroup
into #BasBundleInfo
from Bundle b
inner join Bundle_Detail bd on bd.ID = b.ID
inner join Orders o on b.Orderid = o.ID  and o.MDivisionID  = b.MDivisionID 
outer apply (
	select v = stuff ((	select distinct CONCAT ('+', bda.SubprocessId)
						from Bundle_Detail_Art bda
						where bd.BundleNo = bda.Bundleno
						for xml path(''))
						, 1, 1, '')
) bda
left join BundleInOut bio on bio.BundleNo = bd.BundleNo							 
							 and bio.SubProcessId = 'Loading'
							 and bio.InComing is not null
                             and isnull(bio.RFIDProcessLocationID,'') = ''
where	1=1 
{this.sqlWhere.JoinToString($"{Environment.NewLine}")}
		

	-- 組合 Location 與計算數量 -- 
	select	o.FactoryID
			, LocationID = stuff ((	select distinct CONCAT ('/', bas.LocationID)
									from #BasBundleInfo bas
									where	bas.OrderID = DisBundleInfo.OrderID
											and bas.FComb = DisBundleInfo.FComb
											and bas.Colorid = DisBundleInfo.Colorid
											and bas.Pattern = DisBundleInfo.Pattern
											and bas.PtnDes = DisBundleInfo.PtnDes
											and bas.Size = DisBundleInfo.Size
											and bas.Artwork = DisBundleInfo.Artwork
											and isnull (bas.LocationID, '') != ''
									for xml path(''))
								   , 1, 1, '')
			, DisBundleInfo.OrderID
			, o.POID
			, Line = stuff ((select distinct concat ('/', ssd.SewingLineID)
							 from SewingSchedule_Detail ssd
							 where DisBundleInfo.Orderid = ssd.OrderID
								   and DisBundleInfo.Size = ssd.SizeCode
							 for xml path(''))
							 , 1, 1, '') 
			, o.StyleID
			, DisBundleInfo.BundleGroup
			, DisBundleInfo.FComb
			, DisBundleInfo.Colorid
			, DisBundleInfo.Pattern
			, DisBundleInfo.PtnDes
			, DisBundleInfo.Size
			, DisBundleInfo.Artwork
			, OrderQty = oq.Qty
			, LoadingQty = AccuLoading.Qty
			, Balance = oq.Qty - AccuLoading.Qty
			, [Status] = case
							when oq.Qty - AccuLoading.Qty > 0 then 'Lacking'
							when oq.Qty = AccuLoading.Qty then 'Complete'
							else 'Excess'
						 end
            , DisBundleInfo.isPair
	from (
		select	  bbi.OrderID
				, bbi.FComb
				, bbi.Colorid
				, bbi.Pattern
				, bbi.PtnDes
				, bbi.Size
				, bbi.Artwork
				, Qty = sum (Qty)
				, IsPair = count (case when IsPair = 1 then 1 end)
				, BundleGroup = stuff ((select distinct concat ('/', bb.BundleGroup)
							 from #BasBundleInfo bb
							 where	bb.OrderID = bbi.OrderID   and
									bb.FComb   = bbi.FComb     and
									bb.Colorid = bbi.Colorid   and
									bb.Pattern = bbi.Pattern   and
									bb.PtnDes  = bbi.PtnDes    and
									bb.Size	   = bbi.Size	   and
									bb.Artwork = bbi.Artwork   and
                                    bb.Qty > 0
							 for xml path(''))
							 , 1, 1, '') 
		from #BasBundleInfo bbi
		group by bbi.OrderID, bbi.FComb, bbi.Colorid, bbi.Pattern
				, bbi.PtnDes, bbi.Size, bbi.Artwork
	) DisBundleInfo
	left join Orders o on DisBundleInfo.Orderid = o.ID 
	outer apply (
		select Qty = sum (oq.Qty)
		from Order_Qty oq
		where DisBundleInfo.Orderid = oq.ID
			  and DisBundleInfo.Size = oq.SizeCode
	) oq
	outer apply (
		select Qty = Floor (DisBundleInfo.Qty / case
													when IsPair > 0 then 2
													else 1
												end)
	) AccuLoading
	order by o.FtyGroup, DisBundleInfo.OrderID, o.StyleID, DisBundleInfo.FComb
			, DisBundleInfo.Colorid, DisBundleInfo.Pattern, DisBundleInfo.Size
			, DisBundleInfo.Artwork

	-- 拆分 Location 計算數量 --
	select	o.FactoryID
			, DisBundleInfo.LocationID
			, DisBundleInfo.Orderid
            , o.POID
			, o.StyleID
			, Line = stuff ((select distinct concat ('/', ssd.SewingLineID)
							 from SewingSchedule_Detail ssd
							 where DisBundleInfo.Orderid = ssd.OrderID
								   and DisBundleInfo.Size = ssd.SizeCode
							 for xml path(''))
							 , 1, 1, '') 
            , DisBundleInfo.BundleGroup
			, DisBundleInfo.FComb
			, DisBundleInfo.Colorid
			, DisBundleInfo.Pattern
			, DisBundleInfo.PtnDes
			, DisBundleInfo.Size
			, DisBundleInfo.Artwork
			, OrderQty = oq.Qty
			, AccuLoadingQty = Floor (sum (DisBundleInfo.Qty) over (partition by OrderID, FComb, ColorID, Pattern, PtnDes, Size, Artwork) / case
																																				when IsPair > 0 then 2
																																				else 1
																																			 end)
			, AccuPerLocation = AccuPerLocation.Qty
	from (
		select	  bbi.OrderID
				, bbi.LocationID
				, bbi.FComb
				, bbi.Colorid
				, bbi.Pattern
				, bbi.PtnDes
				, bbi.Size
				, bbi.Artwork
				, Qty = sum (Qty)
				, IsPair = count (case when IsPair = 1 then 1 end)
				, BundleGroup = stuff ((select distinct concat ('/', bb.BundleGroup)
							 from #BasBundleInfo bb
							 where	bb.OrderID = bbi.OrderID   and
									bb.FComb   = bbi.FComb     and
									bb.Colorid = bbi.Colorid   and
									bb.Pattern = bbi.Pattern   and
									bb.PtnDes  = bbi.PtnDes    and
									bb.Size	   = bbi.Size	   and
									bb.Artwork = bbi.Artwork   and
                                    bb.Qty > 0
							 for xml path(''))
							 , 1, 1, '') 
		from #BasBundleInfo bbi
		group by bbi.OrderID, bbi.LocationID, bbi.FComb, bbi.Colorid, bbi.Pattern
				, bbi.PtnDes, bbi.Size, bbi.Artwork
	) DisBundleInfo
	left join Orders o on DisBundleInfo.Orderid = o.ID
	outer apply (
		select Qty = sum (oq.Qty)
		from Order_Qty oq
		where DisBundleInfo.Orderid = oq.ID
			  and DisBundleInfo.Size = oq.SizeCode
	) oq
	outer apply (
		select Qty = Floor (DisBundleInfo.Qty / case
													when IsPair > 0 then 2
													else 1
												end)
	) AccuPerLocation
	order by o.FtyGroup, DisBundleInfo.OrderID, o.StyleID, DisBundleInfo.FComb
			, DisBundleInfo.Colorid, DisBundleInfo.Pattern, DisBundleInfo.Size
			, DisBundleInfo.Artwork, DisBundleInfo.LocationID

    select OrderID ,
		   FComb	 ,
		   Colorid	 ,
		   Pattern	 ,
		   PtnDes	 ,
		   Size		 ,
		   Artwork	 ,
		   BundleGroup,
           isPair,
		   Qty = sum(isnull(Qty,0))
	from #BasBundleInfo
    where Qty > 0
	group by	OrderID,
				FComb	 ,
				Colorid	 ,
				Pattern	 ,
				PtnDes	 ,
				Size		 ,
				Artwork	 ,
                isPair,
				BundleGroup

drop table #BasBundleInfo
";
            #endregion

            this.ds = new DataSet();

            try
            {
                SqlConnection sqlConnection = null;
                DBProxy.Current.OpenConnection(null, out sqlConnection);
                this.cmd = new SqlCommand(sqlcmd, sqlConnection);
                this.cmd.CommandTimeout = 3000; // 設定time out 50分鐘
                SqlDataAdapter sqad = new SqlDataAdapter(this.cmd);
                sqad.Fill(this.ds);

                this.cmd.Dispose();
                sqlConnection.Close();
            }
            catch (SqlException ex)
            {
                if (!this.cancel)
                {
                    MyUtility.Msg.WarningBox(ex.Message);
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnToExcel_Click(object sender, EventArgs e)
        {
            if (this.bgWorkerUpdateInfo.IsBusy)
            {
                this.bgWorkerUpdateInfo.WorkerSupportsCancellation = true;
                this.bgWorkerUpdateInfo.CancelAsync();
                this.cancel = true;
                this.cmd.Cancel();
                this.btnQuery.Enabled = true;
            }

            if ((this.dtFormGrid == null || this.dtExcel.Rows.Count == 0) || this.ds.Tables.Count != 3)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return;
            }

            Excel.Application objApp = new Excel.Application();
            Utility.Report.ExcelCOM com = new Utility.Report.ExcelCOM(Sci.Env.Cfg.XltPathDir + "\\Subcon_P40.xltx", objApp);
            Excel.Worksheet worksheet = objApp.Sheets[1];

            com.WriteTable(this.dtExcel, 2);
            worksheet.get_Range($"A2:P{MyUtility.Convert.GetString(1 + this.dtExcel.Rows.Count)}").Borders.LineStyle = Excel.XlLineStyle.xlContinuous; // 畫線
            com.ExcelApp.ActiveWorkbook.Sheets[1].Select(Type.Missing);
            objApp.Visible = true;
            objApp.Columns.AutoFit();
            if (worksheet.Columns["G:G"].ColumnWidth > 50)
            {
                worksheet.Columns["G:G"].ColumnWidth = 50;
            }

            objApp.Rows.AutoFit();
            string Excelfile = Sci.Production.Class.MicrosoftFile.GetName("Subcon_P40");
            objApp.ActiveWorkbook.SaveAs(Excelfile);
            Marshal.ReleaseComObject(worksheet);
            Marshal.ReleaseComObject(objApp);
        }

        private void bgWorkerUpdateInfo_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.HideLoadingText();
            this.grid.Cursor = Cursors.Default;
            this.UseWaitCursor = false;

            this.btnQuery.Enabled = true;
            if (this.ds != null)
            {
                this.listControlBindingSource1.DataSource = null;
                if (this.ds.Tables.Count == 0)
                {
                    return;
                }

                if ((this.ds.Tables[0] == null || this.ds.Tables[0].Rows.Count == 0) ||
                    (this.ds.Tables.Count != 3))
                {
                    MyUtility.Msg.WarningBox("Data not found!");
                    if (this.dtFormGrid != null)
                    {
                        this.dtFormGrid.Clear();
                        this.dtExcel.Clear();
                        this.dtBundleGroupQty.Clear();
                    }

                    return;
                }

                this.dtFormGrid = this.ds.Tables[0].AsEnumerable().CopyToDataTable();
                this.dtExcel = this.ds.Tables[1].AsEnumerable().CopyToDataTable();
                if (this.ds.Tables[2].Rows.Count > 0)
                {
                    this.dtBundleGroupQty = this.ds.Tables[2].AsEnumerable().CopyToDataTable();
                }

                this.listControlBindingSource1.DataSource = this.dtFormGrid;
            }
        }

        private void P40_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.bgWorkerUpdateInfo.IsBusy)
            {
                this.bgWorkerUpdateInfo.CancelAsync();
                if (this.cmd != null)
                {
                    this.cancel = true;
                    this.cmd.Cancel();
                }
            }
        }

        private void ShowBundleGroupDetailQty(DataRow drSelected)
        {
            if (this.dtBundleGroupQty != null && this.dtBundleGroupQty.Rows.Count > 0)
            {
                var resultBundleQtyDetail = this.dtBundleGroupQty.AsEnumerable()
                                                                           .Where(src => src["OrderID"].Equals(drSelected["OrderID"]) &&
                                                                                            src["FComb"].Equals(drSelected["FComb"]) &&
                                                                                            src["Colorid"].Equals(drSelected["Colorid"]) &&
                                                                                            src["Pattern"].Equals(drSelected["Pattern"]) &&
                                                                                            src["PtnDes"].Equals(drSelected["PtnDes"]) &&
                                                                                            src["Size"].Equals(drSelected["Size"]) &&
                                                                                            src["Artwork"].Equals(drSelected["Artwork"]));
                if (resultBundleQtyDetail.Any())
                {
                    string msgIsPair = (int)drSelected["isPair"] > 0 ? "Cut-part is pair." : string.Empty;
                    DataTable dtResult = resultBundleQtyDetail.OrderByDescending(src => src["Qty"])
                                                                           .ThenBy(src => src["BundleGroup"])
                                                                           .CopyToDataTable();
                    MyUtility.Msg.ShowMsgGrid_LockScreen(dtResult, msg: msgIsPair, caption: "Group Detail Qty", shownColumns: "BundleGroup,Qty");
                }
            }
        }
    }
}
