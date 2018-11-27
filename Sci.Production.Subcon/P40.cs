using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Subcon
{
    public partial class P40 : Sci.Win.Tems.QueryForm
    {
        private Ict.Win.UI.DataGridViewTextBoxColumn col_StatusColor;
        List<string> sqlWhere = new List<string>();
        string InlineDate1; string InlineDate2;
        private DataTable dt1,dt2;
        private DataSet ds;
        private SqlCommand cmd;
        private bool cancel;

        public P40(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            EditMode = true;
            this.txtfactory.Text = Sci.Env.User.Factory;
            this.displaySubProcess.Text = "Loading";
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.grid.IsEditingReadOnly = true;
            this.grid.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.grid)

            .Text("FactoryID", header: "Factory", width: Widths.Auto(true))
            .Text("Location", header: "Location", width: Widths.Auto(true))
            .Text("OrderID", header: "SP No.", width: Widths.AnsiChars(14))
            .Text("Line", header: "Inline Line#", width: Widths.Auto(true))
            .Text("StyleID", header: "Style", width: Widths.AnsiChars(15))
            .Text("FComb", header: "Comb", width: Widths.Auto(true))
            .Text("ColorID", header: "Color", width: Widths.Auto(true))
            .Text("Pattern", header: "Pattern", width: Widths.AnsiChars(10))
            .Text("PtnDesc", header: "PtnDesc", width: Widths.Auto(true))
            .Text("Size", header: "Size", width: Widths.Auto(true))
            .Text("Artwork", header: "Artwork", width: Widths.Auto(true))
            .Text("OrderQty", header: "Order Qty per Size", width: Widths.Auto(true))
            .Numeric("LoadingQty", header: "Accu. Loading Qty" + Environment.NewLine + "per Parts", width: Widths.Auto(true), decimal_places: 2)
            .Numeric("Balance", header: "Balance Qty", width: Widths.Auto(true), decimal_places: 2)
            .Text("Status", header: "Status", width: Widths.AnsiChars(8)).Get(out col_StatusColor)
            ;
            Change_Color();
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
            if ((MyUtility.Check.Empty(txtSp1.Text) || MyUtility.Check.Empty(txtSp2.Text)) &&
               (MyUtility.Check.Empty(dateRangeInlineDate.Value1) || MyUtility.Check.Empty(dateRangeInlineDate.Value2)) &&
               MyUtility.Check.Empty(txtLocation.Text)
               )
            {
                MyUtility.Msg.WarningBox(@"SP#, Inline Date and Location cannot all be empty.");
                return;
            }

            if (!MyUtility.Check.Empty(txtSp1.Text) || !MyUtility.Check.Empty(txtSp2.Text))
            {
                if (MyUtility.Check.Empty(txtSp1.Text) || MyUtility.Check.Empty(txtSp2.Text))
                {
                    MyUtility.Msg.WarningBox(@"Must enter SP# value1 and value2");
                    return;
                }
            }

            // Query時,判斷sql並非Cancel狀態
            cancel = false;

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
                Query();
                this.bgWorkerUpdateInfo.ReportProgress(1);
            }
        }

        private void Query()
        {
            this.ShowLoadingText("ShowLoadingText");
            this.UseWaitCursor = true;
            this.sqlWhere.Clear();
            this.InlineDate1 = this.dateRangeInlineDate.Value1.Empty() ? string.Empty : ((DateTime)this.dateRangeInlineDate.Value1).ToString("yyyy/MM/dd");
            this.InlineDate2 = this.dateRangeInlineDate.Value2.Empty() ? string.Empty : ((DateTime)this.dateRangeInlineDate.Value2).ToString("yyyy/MM/dd");

            #region SqlWhere
            if (!MyUtility.Check.Empty(InlineDate1) && !MyUtility.Check.Empty(InlineDate2))
            {
                this.sqlWhere.Add($@"and o.SewInLine between '{InlineDate1}' and '{InlineDate2}'");
            }

            if (!MyUtility.Check.Empty(txtSp1.Text) && !MyUtility.Check.Empty(txtSp2.Text))
            {
                this.sqlWhere.Add($@"and b.Orderid between '{txtSp1.Text}' and '{txtSp2.Text}'");
            }

            if (!MyUtility.Check.Empty(txtLocation.Text))
            {
                this.sqlWhere.Add($@"and bio.LocationID = '{txtLocation.Text}' ");
            }

            if (!MyUtility.Check.Empty(txtfactory.Text))
            {
                this.sqlWhere.Add($@"and o.FtyGroup = '{txtfactory.Text}'");
            }

            if (!MyUtility.Check.Empty(txtsewingline.Text))
            {
                this.sqlWhere.Add($@"
and exists (select 1
from SewingSchedule_Detail ssd
where b.Orderid = ssd.OrderID
		and bd.SizeCode = ssd.SizeCode
		and ssd.SewingLineID = '{txtsewingline.Text}')");
            }

            if (!MyUtility.Check.Empty(txtCombo.Text))
            {
                this.sqlWhere.Add($@"and b.PatternPanel = '{txtCombo.Text}'");
            }

            if (!MyUtility.Check.Empty(txtArticle.Text))
            {
                this.sqlWhere.Add($@"and b.Article = '{txtArticle.Text}' ");
            }

            if (!MyUtility.Check.Empty(txtSize.Text))
            {
                this.sqlWhere.Add($@"and bd.SizeCode = '{txtSize.Text}' ");
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
into #BasBundleInfo
from Bundle b
inner join Bundle_Detail bd on bd.ID = b.ID
inner join Orders o on b.Orderid = o.ID
outer apply (
	select v = stuff ((	select CONCAT ('+', bda.SubprocessId)
						from Bundle_Detail_Art bda
						where bd.BundleNo = bda.Bundleno
						for xml path(''))
						, 1, 1, '')
) bda
left join BundleInOut bio on bio.BundleNo = bd.BundleNo							 
							 and bio.SubProcessId = 'Loading'
							 and bio.InComing is not null

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
			, Line = stuff ((select distinct concat ('/', ssd.SewingLineID)
							 from SewingSchedule_Detail ssd
							 where DisBundleInfo.Orderid = ssd.OrderID
								   and DisBundleInfo.Size = ssd.SizeCode
							 for xml path(''))
							 , 1, 1, '') 
			, o.StyleID
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
	from (
		select	OrderID
				, FComb
				, Colorid
				, Pattern
				, PtnDes
				, Size
				, Artwork
				, Qty = sum (Qty)
				, IsPair = count (IsPair)
		from #BasBundleInfo
		group by OrderID, FComb, Colorid, Pattern
				, PtnDes, Size, Artwork
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
			, o.StyleID
			, Line = stuff ((select distinct concat ('/', ssd.SewingLineID)
							 from SewingSchedule_Detail ssd
							 where DisBundleInfo.Orderid = ssd.OrderID
								   and DisBundleInfo.Size = ssd.SizeCode
							 for xml path(''))
							 , 1, 1, '') 
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
		select	OrderID
				, LocationID
				, FComb
				, Colorid
				, Pattern
				, PtnDes
				, Size
				, Artwork
				, Qty = sum (Qty)
				, IsPair = count (IsPair)
		from #BasBundleInfo
		group by OrderID, LocationID, FComb, Colorid, Pattern
				, PtnDes, Size, Artwork
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

drop table #BasBundleInfo
";
            #endregion
            
            ds= new DataSet();

            try
            {
                SqlConnection sqlConnection = null;
                DBProxy.Current.OpenConnection(null, out sqlConnection);
                this.cmd = new SqlCommand(sqlcmd, sqlConnection);
                cmd.CommandTimeout = 3000; // 設定time out 50分鐘
                SqlDataAdapter sqad = new SqlDataAdapter(cmd);
                sqad.Fill(ds);

                cmd.Dispose();
                sqlConnection.Close();
            }
            catch (SqlException ex)
            {
                if (!cancel)
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
                cancel = true;
                this.cmd.Cancel();
                this.btnQuery.Enabled = true;
            }

            if (dt2 == null || dt2.Rows.Count==0)
            {
                MyUtility.Msg.WarningBox("Data not found!");               
                return;
            }

            Excel.Application objApp = new Excel.Application();
            Sci.Utility.Report.ExcelCOM com= new Sci.Utility.Report.ExcelCOM(Sci.Env.Cfg.XltPathDir + "\\Subcon_P40.xltx", objApp);
            Excel.Worksheet worksheet = objApp.Sheets[1];

            com.WriteTable(dt2, 2);
            worksheet.get_Range($"A2:N{MyUtility.Convert.GetString(1 + dt2.Rows.Count)}").Borders.LineStyle = Excel.XlLineStyle.xlContinuous; // 畫線
            com.ExcelApp.ActiveWorkbook.Sheets[1].Select(Type.Missing);
            objApp.Visible = true;
            objApp.Columns.AutoFit();
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
            if (ds != null)
            {
                if (ds.Tables.Count == 0)
                {
                    return;
                }

                if (ds.Tables[0] == null || ds.Tables[0].Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!");
                    return;
                }

                if (ds.Tables.Count == 2)
                {
                    dt2 = ds.Tables[1].Copy();
                }

                dt1 = ds.Tables[0].Copy();
                this.listControlBindingSource1.DataSource = dt1;
            }
         
        }

        private void grid_MouseUp(object sender, MouseEventArgs e)
        {
            if (!this.bgWorkerUpdateInfo.IsBusy)
            {
                this.HideLoadingText();
                this.grid.Cursor = Cursors.Default;
                Application.UseWaitCursor = false;
            }
        }

        private void P40_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.bgWorkerUpdateInfo.IsBusy)
            {
                this.bgWorkerUpdateInfo.CancelAsync();
                if (cmd != null)
                {
                    cmd.Cancel();
                }
            }
        }
    }
}
