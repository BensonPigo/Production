using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Xml.Linq;
using System.Configuration;
using Ict;
using Sci.Data;
using Ict.Win;

namespace Sci.Production.Centralized
{
    /// <summary>
    /// P01
    /// </summary>
    public partial class P01 : Win.Tems.QueryForm
    {
        private DataTable detailDt;
        private DataTable byMDt;
        private DataTable byBrandDt;
        private Win.MatrixHelper _matrix;

        /// <summary>
        /// P01
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.dateSewingOutput.Value = DateTime.Today.AddDays(-1);
            this.comboBoxDisplayBy.SelectedItem = "M";
            this.comboBoxValue.SelectedItem = "Delay Qty / Total Qty";
        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            this.detailDt = null;
            this.byMDt = null;
            this.byBrandDt = null;
            this.grid1.DataSource = null;
            DataTable tmpDetailDt = null, tmpMDt = null, tmpBrandDt = null;
            #region Sewing Output & Buyer Delivery 必須都輸入
            if (this.dateRangeBuyerDelivery.Value1.Empty() || this.dateRangeBuyerDelivery.Value2.Empty())
            {
                MyUtility.Msg.WarningBox("Buyer Delivery can't all be empty!!");
                return;
            }

            if (this.dateSewingOutput.Value.Empty())
            {
                MyUtility.Msg.WarningBox("Sewing Output can't be empty!!");
                return;
            }
            #endregion
            #region set SQL Parameter
            List<SqlParameter> listParameter = new List<SqlParameter>();
            listParameter.Add(new SqlParameter("@StartBuyerDelivery", ((DateTime)this.dateRangeBuyerDelivery.Value1).ToString("yyyy/MM/dd")));
            listParameter.Add(new SqlParameter("@EndBuyerDelivery", ((DateTime)this.dateRangeBuyerDelivery.Value2).ToString("yyyy/MM/dd")));
            listParameter.Add(new SqlParameter("@SewingOutput", ((DateTime)this.dateSewingOutput.Value).ToString("yyyy/MM/dd")));
            listParameter.Add(new SqlParameter("@CountryID", this.txtcountry.TextBox1.Text));
            #endregion
            #region SQL Filte
            Dictionary<string, string> sqlFilte = new Dictionary<string, string>();
            sqlFilte.Add("CountryID", this.txtcountry.TextBox1.Text.Empty() ? string.Empty : "and f.CountryID = @CountryID");
            #endregion
            #region SQL Command
            StringBuilder sqlCmd = new StringBuilder(string.Empty);
            sqlCmd.Append(string.Format(
                @"
with temporder AS (
	select	StdOutput = isnull ((select top 1 stdq from getstdq(o.ID)), 0)
			, o.* 
	from Orders o
	inner join Factory f on o.FactoryID = f.ID 
	--排除ADIDAS 中 mi-adidas、SLC、PPC  order，及Sample、Local order
	where	((o.ProjectID != 'ZMII' and o.ProjectID != 'CR' and o.OrderTypeID != 'PPC') or o.BrandID != 'ADIDAS') 
			and o.Category='B' 
			and o.LocalOrder=0 
			and (o.Junk=0 or (o.Junk=1 and o.NeedProduction=1))
			and o.BuyerDelivery between @StartBuyerDelivery and @EndBuyerDelivery
            -- CountryID
            {0}
			and f.IsProduceFty = 1
)
select	O.ID
		, Category = case O.Category 
						when 'B' then 'Bulk'
						when 'S' then 'Sample' 
						else O.Category 
					 end
		, EType = EType.value
		, O.MDivisionID
		, O.FactoryID
		, O.BrandID
		, O.StyleID
		, O.SeasonID
		, O.CdCodeID
		, O.CPU
		, CPU_EType = IIF (O.StyleUnit = 'sets', Convert(varchar, (O.CPU * isnull (SL.Rate, 0) / 100)), '0')
		, O.Qty
		, O.SciDelivery
		, BuyerDelivery = convert (varchar, O.BuyerDelivery, 111)
		, Multi_Delivery = (select IIF (count (*) > 1, 'Y', 'N')
							from Order_QtyShip
							where ID = O.ID)
		, QtyDelivery = stuff ((select CHAR(10) + tmp.value
								from (
									select value = CONCAT (_OQ.Qty, ', ', _OQ.ShipmodeID, ', ', _OQ.BuyerDelivery)
									from Order_QtyShip _OQ
									where ID = O.ID
								) tmp
								for xml path('')), 1, 1, '')
		, O.SewInLine
		, O.SewOffLine
		, LineNum = stuff ((select CHAR (10) + tmp.value
                            from (
								select value = CONCAT(_SS.SewingLineID, '  (', CONVERT(varchar, _SS.Inline, 111), ' ~ ' , CONVERT(varchar, _SS.Offline, 111), ')')
                                from SewingSchedule _SS
                                where _SS.OrderID = O.ID
                            ) tmp
                            for xml path('')), 1, 1, '')
		, SewingComplete = SewingComplete.value
		, PdnDays = IIF (SewingComplete.value = 'Y', '0', Convert(varchar, PdnDays.value))
		, StdOutput = IIF (SewingComplete.value = 'Y', '0', Convert(varchar, O.StdOutput))
		, DaysSinceInline = IIF (SewingComplete.value = 'Y', '0' , Convert(varchar, DaysSinceInline.value))
		, AccuStdOutput = AccuStdOutput.value
		, AccuActOutput = AccuActOutput.value
		, VarianceQty = IIF (SewingComplete.value = 'Y', 0, AccuActOutput.value - AccuStdOutput.value)
		, VarianceDays =  IIF (VarianceDays.value > 0, CEILING(VarianceDays.value), FLOOR(VarianceDays.value))
		, DaysToDelivery = DaysToDelivery.value
		, DaysNeedForProd = DaysNeedForProd.value
		, PostSewingDays = PostSewingDays.value
		, Variance = Variance.value
		, PotentialDelayRisk = IIF (O.BuyerDelivery < @SewingOutput, '-'
																   , IIF (Variance.value < 0, 'Y'
																							, 'N')
								   )
		, SMR = dbo.getPass1_ExtNo (O.SMR)
		, MRHandle = dbo.getPass1_ExtNo (O.MRHandle)
from temporder O
left join Style_Location SL on SL.StyleUkey=O.StyleUkey
outer apply (
	select value = SL.Location 
) EType
outer apply (
	select value = IIF (SUM (_SOD.QAQty) >= O.Qty, 'Y', '')
	from Tradedb.trade.dbo.SewingOutput_Detail _SOD
	left join Tradedb.trade.dbo.SewingOutput _SO on _SOD.ID = _SO.ID
	where	_SOD.OrderId = O.ID
			and _SOD.ComboType = EType.value
) SewingComplete
outer apply (
	select value = SUM (_SS.WorkDay)
	from SewingSchedule _SS
	where	_SS.OrderID = O.ID
			and _SS.ComboType = EType.value
) PdnDays
outer apply (
		select value = count(*)
		from SewingSchedule _SS
		where	_SS.OrderID = O.ID
				and _SS.ComboType = EType.value
) dataCount
outer apply (
    select  Inline = _SS.Inline
            , Offline = _SS.Offline
            , SewingLineID = _SS.SewingLineID
            , APSNo
    from SewingSchedule _SS
    where   _SS.OrderID = O.ID
            and _SS.ComboType = EType.value
            and dataCount.value = 1
) SewingScheduleData
outer apply (
	select value = IIF (dataCount.value > 1, 0
                 , IIF (O.SewInline <= @SewingOutput, count(*)
                 , 0 - count(*)
                   )
          )
	from WorkHour _WH
	where	_WH.SewingLineID = SewingScheduleData.SewingLineID
			and _WH.Date >= SewingScheduleData.Inline
			--SewingOutputDate
			and _WH.Date <= @SewingOutput
			and _WH.Holiday = 0
) DaysSinceInline
outer apply (
	select	value = IIF (SewingComplete.value = 'Y', 0 
												   , (select	IIF (SUM (StdQ) >= O.Qty, O.Qty, SUM (StdQ))
													  from dbo.getDailystdq (SewingScheduleData.APSNo) _getStdq
													  where _getStdq.Date <= @SewingOutput))
) AccuStdOutput
outer apply (
	select value = isnull (SUM (_SOD.QAQty), 0)
	from Tradedb.trade.dbo.SewingOutput _SO
	inner join Tradedb.trade.dbo.SewingOutput_Detail _SOD on _SO.ID = _SOD.ID
	where	_SOD.OrderId = O.ID
			and _SOD.ComboType = 'T'
			--SewingOutputDate
			and _SO.OutputDate <= @SewingOutput
) AccuActOutput
outer apply (
	select value = count (*) 
	from Holiday _H
	where	--Holiday 必須排除星期日，星期日另外算
            DATEPART (WEEKDAY, _H.HolidayDate) != 7
			--SewingOutputDate
			and _H.HolidayDate between O.BuyerDelivery and @SewingOutput
) Holidays
outer apply (
	select value = IIF (SewingComplete.value = 'Y' or o.StdOutput = 0, 0
																	 , (AccuActOutput.value - AccuStdOutput.value) / o.StdOutput)
) VarianceDays
outer apply (
	select value = IIF (@SewingOutput <= O.BuyerDelivery, tmp.value, (0 - tmp.value))
	from (
		select value = DateDiff (Day, O.BuyerDelivery, @SewingOutput) - Holidays.value - dbo.getDateRangeSundayCount (O.BuyerDelivery, @SewingOutput)
	) tmp
) DaysToDelivery
outer apply (
	select value = count (distinct _SO.OutputDate)
	from Tradedb.trade.dbo.SewingOutput _SO
	inner join Tradedb.trade.dbo.SewingOutput_Detail _SOD on _SO.ID = _SOD.ID
	where	_SOD.OrderId = O.ID
			and _SOD.ComboType = EType.value
) nTtlSewDays 
outer apply (
	select value = ISNULL (SUM (_SOD.QAQty), 0)
	from Tradedb.trade.dbo.SewingOutput _SO
	inner join Tradedb.trade.dbo.SewingOutput_Detail _SOD on _SO.ID = _SOD.ID
	where	_SOD.OrderId = O.ID
			and _SOD.ComboType = EType.value
) nTtlOutPut  
outer apply (
	select value = IIF (nTtlSewDays.value > 0, (nTtlOutPut.value / nTtlSewDays.value), o.StdOutput)
) nAvgActOut
outer apply (
	select value = IIF (SewingComplete.value = 'Y', 0, tmp.value)
	from (
		select value = IIF (nAvgActOut.value > 0, CEILING((O.Qty - AccuActOutput.value) / nAvgActOut.value), PdnDays.value)
	) tmp
) DaysNeedForProd
outer apply (
	select value = (select isnull (Max (_AT.PostSewingDays), 0)
					from Order_TmsCost _OT
					left join Artworktype _AT on _OT.Seq = _AT.Seq
					where	_OT.ID = O.ID
							and _OT.Price != 0)
) PostSewingDays
outer apply (
	select value = DaysToDelivery.value - DaysNeedForProd.value - PostSewingDays.value
) Variance
order by O.FactoryID, O.ID
", sqlFilte["CountryID"]));
            #endregion
            #region 由 appconfig 取得所有連線路徑
            this.ShowWaitMessage("Load connections...");
            XDocument docx = XDocument.Load(Application.ExecutablePath + ".config");
            string[] strServers = ConfigurationManager.AppSettings["ServerMatchFactory"].Split(new char[] { ';' });
            List<string> connectionString = new List<string>();
            foreach (string ss in strServers)
            {
                var connections = docx.Descendants("modules").Elements().Where(y => y.FirstAttribute.Value.Contains(ss.Split(new char[] { ':' })[0].ToString())).Descendants("connectionStrings").Elements().Where(x => x.FirstAttribute.Value.Contains("Production")).Select(z => z.LastAttribute.Value).ToList()[0].ToString();
                connectionString.Add(connections);
            }

            if (connectionString == null || connectionString.Count == 0)
            {
                MyUtility.Msg.WarningBox("no connection loaded.");
                return;
            }

            this.HideWaitMessage();
            #endregion
            #region SQL Processing
            for (int i = 0; i < connectionString.Count; i++)
            {
                string conString = connectionString[i];
                this.ShowWaitMessage(string.Format("Load data from connection {0}/{1} ", i + 1, connectionString.Count));
                SqlConnection conn;
                using (conn = new SqlConnection(conString))
                {
                    DataTable outDt;
                    DualResult result = DBProxy.Current.SelectByConn(conn, sqlCmd.ToString(), listParameter, out outDt);
                    if (!result)
                    {
                        this.ShowErr(result);
                        this.HideWaitMessage();
                        return;
                    }

                    if (tmpDetailDt == null)
                    {
                        tmpDetailDt = outDt;
                    }
                    else
                    {
                        tmpDetailDt.Merge(outDt);
                    }
                }
            }

            this.HideWaitMessage();
            #endregion
            if (tmpDetailDt == null || tmpDetailDt.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found!");
            }
            else
            {
                #region 依照 Detail 預先組出 Matrix Data
                string str = @"
select *
from (
	select	M = t.MDivisionID
			, t.BuyerDelivery
			, Cpu = FORMAT (sum (t.CPU * t.Qty), '#,0.##')
			, Qty = FORMAT (sum (t.Qty), '#,0.###')
			, strDelayCpuDividedByCpu = Concat (FORMAT (sum (Variance.value * t.CPU), '#,0.##'), '/', FORMAT (sum (t.CPU * t.Qty), '#,0.##'))
			, strDelayQtyDividedByQty = Concat (FORMAT (sum (Variance.value), '#,0.###'), '/', FORMAT (sum (t.Qty), '#,0.###'))
			, DelayCpuDividedByCpu = Concat (FORMAT (sum (Variance.value * t.CPU) / sum (t.CPU * t.Qty) * 100, '#,0.##'), ' %')
	from #detailTmp t
    outer apply (   
        select  value = iif (t.VarianceQty < 0, ABS (t.VarianceQty), 0)
    ) Variance
	group By t.MDivisionID, t.BuyerDelivery

	union all
	select 	M = t.MDivisionID
			, BuyerDelivery = 'Total'
			, Cpu = FORMAT (sum (t.CPU * t.Qty), '#,0.##')
			, Qty = FORMAT (sum (t.Qty), '#,0.###')
			, strDelayCpuDividedByCpu = Concat (FORMAT (sum (Variance.value * t.CPU), '#,0.##'), '/', FORMAT (sum (t.CPU * t.Qty), '#,0.##'))
			, strDelayQtyDividedByQty = Concat (FORMAT (sum (Variance.value), '#,0.###'), '/', FORMAT (sum (t.Qty), '#,0.###'))
			, DelayCpuDividedByCpu = Concat (FORMAT (sum (Variance.value * t.CPU) / sum (t.CPU * t.Qty) * 100, '#,0.##'), ' %')
	from #detailTmp t
    outer apply (   
        select  value = iif (t.VarianceQty < 0, ABS (t.VarianceQty), 0)
    ) Variance
	group By t.MDivisionID

	union all
	select 	M = 'Total'
			, BuyerDelivery = t.BuyerDelivery
			, Cpu = FORMAT (sum (t.CPU * t.Qty), '#,0.##')
			, Qty = FORMAT (sum (t.Qty), '#,0.###')
			, strDelayCpuDividedByCpu = Concat (FORMAT (sum (Variance.value * t.CPU), '#,0.##'), '/', FORMAT (sum (t.CPU * t.Qty), '#,0.##'))
			, strDelayQtyDividedByQty = Concat (FORMAT (sum (Variance.value), '#,0.###'), '/', FORMAT (sum (t.Qty), '#,0.###'))
			, DelayCpuDividedByCpu = Concat (FORMAT (sum (Variance.value * t.CPU) / sum (t.CPU * t.Qty) * 100, '#,0.##'), ' %')
	from #detailTmp t
    outer apply (   
        select  value = iif (t.VarianceQty < 0, ABS (t.VarianceQty), 0)
    ) Variance
	group By t.BuyerDelivery

	union all
	select 	M = 'Total'
			, BuyerDelivery = 'Total'
			, Cpu = FORMAT (sum (t.CPU * t.Qty), '#,0.##')
			, Qty = FORMAT (sum (t.Qty), '#,0.###')
			, strDelayCpuDividedByCpu = Concat (FORMAT (sum (Variance.value * t.CPU), '#,0.##'), '/', FORMAT (sum (t.CPU * t.Qty), '#,0.##'))
			, strDelayQtyDividedByQty = Concat (FORMAT (sum (Variance.value), '#,0.###'), '/', FORMAT (sum (t.Qty), '#,0.###'))
			, DelayCpuDividedByCpu = Concat (FORMAT (sum (Variance.value * t.CPU) / sum (t.CPU * t.Qty) * 100, '#,0.##'), ' %')
	from #detailTmp t
    outer apply (   
        select  value = iif (t.VarianceQty < 0, ABS (t.VarianceQty), 0)
    ) Variance
) M

select *
from (
	select	Brand = t.BrandID
			, t.BuyerDelivery
			, Cpu = FORMAT (sum (t.CPU * t.Qty), '#,0.##')
			, Qty = FORMAT (sum (t.Qty), '#,0.###')
			, strDelayCpuDividedByCpu = Concat (FORMAT (sum (Variance.value * t.CPU), '#,0.##'), '/', FORMAT (sum (t.CPU * t.Qty), '#,0.##'))
			, strDelayQtyDividedByQty = Concat (FORMAT (sum (Variance.value), '#,0.###'), '/', FORMAT (sum (t.Qty), '#,0.###'))
			, DelayCpuDividedByCpu = Concat (FORMAT (sum (Variance.value * t.CPU) / sum (t.CPU * t.Qty) * 100, '#,0.##'), ' %')
	from #detailTmp t
    outer apply (   
        select  value = iif (t.VarianceQty < 0, ABS (t.VarianceQty), 0)
    ) Variance
	group By t.BrandID, t.BuyerDelivery

	union all
	select 	Brand = t.BrandID
			, BuyerDelivery = 'Total'
			, Cpu = FORMAT (sum (t.CPU * t.Qty), '#,0.##')
			, Qty = FORMAT (sum (t.Qty), '#,0.###')
			, strDelayCpuDividedByCpu = Concat (FORMAT (sum (Variance.value * t.CPU), '#,0.##'), '/', FORMAT (sum (t.CPU * t.Qty), '#,0.##'))
			, strDelayQtyDividedByQty = Concat (FORMAT (sum (Variance.value), '#,0.###'), '/', FORMAT (sum (t.Qty), '#,0.###'))
			, DelayCpuDividedByCpu = Concat (FORMAT (sum (Variance.value * t.CPU) / sum (t.CPU * t.Qty) * 100, '#,0.##'), ' %')
	from #detailTmp t
    outer apply (   
        select  value = iif (t.VarianceQty < 0, ABS (t.VarianceQty), 0)
    ) Variance
	group By t.BrandID

	union all
	select 	Brand = 'Total'
			, BuyerDelivery = t.BuyerDelivery
			, Cpu = FORMAT (sum (t.CPU * t.Qty), '#,0.##')
			, Qty = FORMAT (sum (t.Qty), '#,0.###')
			, strDelayCpuDividedByCpu = Concat (FORMAT (sum (Variance.value * t.CPU), '#,0.##'), '/', FORMAT (sum (t.CPU * t.Qty), '#,0.##'))
			, strDelayQtyDividedByQty = Concat (FORMAT (sum (Variance.value), '#,0.###'), '/', FORMAT (sum (t.Qty), '#,0.###'))
			, DelayCpuDividedByCpu = Concat (FORMAT (sum (Variance.value * t.CPU) / sum (t.CPU * t.Qty) * 100, '#,0.##'), ' %')
	from #detailTmp t
    outer apply (   
        select  value = iif (t.VarianceQty < 0, ABS (t.VarianceQty), 0)
    ) Variance
	group By t.BuyerDelivery

	union all
	select 	Brand = 'Total'
			, BuyerDelivery = 'Total'
			, Cpu = FORMAT (sum (t.CPU * t.Qty), '#,0.##')
			, Qty = FORMAT (sum (t.Qty), '#,0.###')
			, strDelayCpuDividedByCpu = Concat (FORMAT (sum (Variance.value * t.CPU), '#,0.##'), '/', FORMAT (sum (t.CPU * t.Qty), '#,0.##'))
			, strDelayQtyDividedByQty = Concat (FORMAT (sum (Variance.value), '#,0.###'), '/', FORMAT (sum (t.Qty), '#,0.###'))
			, DelayCpuDividedByCpu = Concat (FORMAT (sum (Variance.value * t.CPU) / sum (t.CPU * t.Qty) * 100, '#,0.##'), ' %')
	from #detailTmp t
    outer apply (   
        select  value = iif (t.VarianceQty < 0, ABS (t.VarianceQty), 0)
    ) Variance
) Brand";
                DataTable[] outArrayDt;
                MyUtility.Tool.ProcessWithDatatable(tmpDetailDt, string.Empty, str, out outArrayDt, "#detailTmp");
                tmpMDt = outArrayDt[0];
                tmpBrandDt = outArrayDt[1];
                #endregion
                this.detailDt = tmpDetailDt;
                this.byMDt = tmpMDt;
                this.byBrandDt = tmpBrandDt;
                this.GridMatrixChange();
            }
        }

        private void ComboBoxDisplayBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.GridMatrixChange();
        }

        private void ComboBoxValue_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.GridMatrixChange();
        }

        private void GridMatrixChange()
        {
            DataTable matrixDataDt;
            string displayBy = string.Empty, showValue = string.Empty;
            switch (this.comboBoxDisplayBy.Text)
            {
                case "Brand":
                    if (this.byBrandDt == null || this.byBrandDt.Rows.Count == 0)
                    {
                        return;
                    }

                    matrixDataDt = this.byBrandDt;
                    displayBy = "Brand";
                    break;
                case "M":
                default:
                    if (this.byMDt == null || this.byMDt.Rows.Count == 0)
                    {
                        return;
                    }

                    matrixDataDt = this.byMDt;
                    displayBy = "M";
                    break;
            }

            switch (this.comboBoxValue.Text)
            {
                case "CPU":
                    showValue = "Cpu";
                    break;
                case "Qty":
                    showValue = "Qty";
                    break;
                case "Delay CPU / Total CPU":
                    showValue = "strDelayCpuDividedByCpu";
                    break;
                case "Delay Qty / Total Qty":
                    showValue = "strDelayQtyDividedByQty";
                    break;
                case "% (delay cpu / total cpu)":
                    showValue = "DelayCpuDividedByCpu";
                    break;
                default:
                    showValue = "strDelayQtyDividedByQty";
                    break;
            }

            this.grid1.DataSource = this.listControlBindingSource1;
            this._matrix = new Win.MatrixHelper(this, this.grid1, this.listControlBindingSource1);
            this._matrix.XMap.Name = "BuyerDelivery";
            this._matrix.YMap.Name = displayBy;
            this._matrix
                .SetColDef(showValue, width: Widths.AnsiChars(4))
                .AddXColDef("BuyerDelivery")
                .AddYColDef(displayBy, header: displayBy, width: Widths.AnsiChars(4))
                ;
            this._matrix.IsXColEditable = false;
            this._matrix.IsYColEditable = false;

            DataTable dtX, dtY;
            string strX = @"
select  distinct BuyerDelivery
from #tmp";
            string strY = string.Format(
                @"
select *
from (
    select  distinct {0}
    from #tmp
    where #tmp.{0} != 'Total'

    union all
    select  {0} = 'Total'
) Y
", displayBy);
            MyUtility.Tool.ProcessWithDatatable(matrixDataDt, string.Empty, strX, out dtX);
            MyUtility.Tool.ProcessWithDatatable(matrixDataDt, string.Empty, strY, out dtY);
            this._matrix.Clear();
            try
            {
                this._matrix.Sets(matrixDataDt, dtX, dtY);
            }
            catch (Exception ex)
            {
                this.ShowErr(ex);
            }

            ((DataTable)this.listControlBindingSource1.DataSource).Rows[0].Delete();
            this.grid1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
        }

        private void Grid1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex > 0)
            {
                string rowHeader = this.grid1.Rows[e.RowIndex].Cells[0].Value.ToString(),
                        columnHeader = this.grid1.Columns[e.ColumnIndex].HeaderText;

                DataTable showDt = null;

                // Click => 都是 Total 則全顯示
                if (rowHeader.EqualString("Total") && columnHeader.EqualString("Total"))
                {
                    showDt = this.detailDt.AsEnumerable().Where(row => true).CopyToDataTable();
                }

                // Click => 只有 M & Brand 是 Total，則挑出指定的 BuyerDelivery
                else if (rowHeader.EqualString("Total") && !columnHeader.EqualString("Total"))
                {
                    if (this.detailDt.AsEnumerable().Any(row => row["BuyerDelivery"].EqualString(columnHeader)))
                    {
                        showDt = this.detailDt.AsEnumerable().Where(row => row["BuyerDelivery"].EqualString(columnHeader)).CopyToDataTable();
                    }
                }
                else
                {
                    string filteBy;
                    switch (this.comboBoxDisplayBy.Text)
                    {
                        case "Brand":
                            filteBy = "BrandID";
                            break;
                        case "M":
                        default:
                            filteBy = "MDivisionID";
                            break;
                    }

                    // Click => 有指定的 M or Brand 但是 BuyerDelivery = Total，則指挑出指定的 M or Brand
                    if (columnHeader.EqualString("Total"))
                    {
                        if (this.detailDt.AsEnumerable().Any(row => row[filteBy].EqualString(rowHeader)))
                        {
                            showDt = this.detailDt.AsEnumerable().Where(row => row[filteBy].EqualString(rowHeader)).CopyToDataTable();
                        }
                    }

                    // Click => 有指定的 【M or Brand】& 【BuyerDelivery】
                    else
                    {
                        if (this.detailDt.AsEnumerable().Any(row => row[filteBy].EqualString(rowHeader) && row["BuyerDelivery"].EqualString(columnHeader)))
                        {
                            showDt = this.detailDt.AsEnumerable().Where(row => row[filteBy].EqualString(rowHeader) && row["BuyerDelivery"].EqualString(columnHeader)).CopyToDataTable();
                        }
                    }
                }

                if (showDt != null && showDt.Rows.Count > 0)
                {
                    P01_DetailData p1 = new P01_DetailData(showDt);
                    p1.ShowDialog(this);
                    showDt.Dispose();
                }
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
