using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P69_Transaction : Win.Subs.Base
    {
        private readonly DataRow drMain;
        private DataTable dtRight;
        private bool isReCalculate = false;

        /// <inheritdoc/>
        public P69_Transaction(DataRow data)
        {
            this.InitializeComponent();
            this.drMain = data;
            this.displaySeq.Text = this.drMain["Seq"].ToString();
            this.displayDesc.Text = this.drMain["Description"].ToString();
            this.displayInQty.Text = this.drMain["InQty"].ToString();
            this.displayOutQty.Text = this.drMain["OutQty"].ToString();
            this.displayAdjustQry.Text = this.drMain["AdjustQty"].ToString();
            this.displayBalQty.Text = this.drMain["Balance"].ToString();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            Ict.Win.UI.DataGridViewTextBoxColumn cbb_ContainerCode_Left;
            Ict.Win.UI.DataGridViewTextBoxColumn cbb_ContainerCode_Right;

            #region 開窗
            DataGridViewGeneratorTextColumnSettings openOtherWH = new DataGridViewGeneratorTextColumnSettings();
            openOtherWH.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.gridDetail.CurrentDataRow;
                if (dr == null)
                {
                    return;
                }

                switch (dr["name"].ToString().Substring(0, 3))
                {
                    case "P70":
                        var p70 = new P70(null, dr["Transaction"].ToString());
                        p70.ShowDialog(this);
                        break;
                    case "P71":
                        var p71 = new P71(null, dr["Transaction"].ToString());
                        p71.ShowDialog(this);
                        break;
                    case "P72":
                        var p72 = new P72(null, dr["Transaction"].ToString());
                        p72.ShowDialog(this);
                        break;
                }
            };
            #endregion

            this.Helper.Controls.Grid.Generator(this.gridDetail)
            .Date("Date", header: "Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("Transaction", header: "Transaction#", width: Widths.AnsiChars(14), iseditingreadonly: true, settings: openOtherWH)
            .Text("Name", header: "Name", width: Widths.AnsiChars(30), iseditingreadonly: true)
            .Numeric("ArrivedQty", header: "Arrived Qty", width: Widths.AnsiChars(4), iseditingreadonly: true, decimal_places: 2)
            .Numeric("ReleasedQty", header: "Released Qty", width: Widths.AnsiChars(4), iseditingreadonly: true, decimal_places: 2)
            .Numeric("AdjustQty", header: "Adjust Qty", width: Widths.AnsiChars(4), iseditingreadonly: true, decimal_places: 2)
            .Numeric("Balance", header: "Balance", width: Widths.AnsiChars(4), iseditingreadonly: true, decimal_places: 2)
            .Text("Location", header: "Location", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("Remark", header: "Remark", width: Widths.AnsiChars(30), iseditingreadonly: true)
            ;

            this.Helper.Controls.Grid.Generator(this.gridLeft)
            .Text("Roll", header: "Roll", iseditingreadonly: true, width: Widths.AnsiChars(8))
            .Text("Dyelot", header: "Dyelot", iseditingreadonly: true, width: Widths.AnsiChars(8))
            .Text("Tone", header: "Tone/Grp", iseditingreadonly: true, width: Widths.AnsiChars(8))
            .Numeric("InQty", header: "In Qty", iseditingreadonly: true, decimal_places: 2, width: Widths.AnsiChars(4))
            .Numeric("OutQty", header: "Out Qty", iseditingreadonly: true, decimal_places: 2, width: Widths.AnsiChars(4))
            .Numeric("AdjustQty", header: "Adjust Qty", iseditingreadonly: true, decimal_places: 2, width: Widths.AnsiChars(4))
            .Numeric("Balance", header: "Balance", iseditingreadonly: true, decimal_places: 2, width: Widths.AnsiChars(4))
            .Text("BulkLocation", header: "Location", iseditingreadonly: true, width: Widths.AnsiChars(14))
            .Text("ContainerCode", header: "Container Code", iseditingreadonly: true, width: Widths.AnsiChars(14)).Get(out cbb_ContainerCode_Left)
            ;

            this.Helper.Controls.Grid.Generator(this.gridRight)
            .Text("Date", header: "Date", iseditingreadonly: true, width: Widths.AnsiChars(9))
            .Text("Transaction", header: "Transaction ID", iseditingreadonly: true, width: Widths.AnsiChars(14))
            .Text("Name", header: "Name", iseditingreadonly: true, width: Widths.AnsiChars(30))
            .Numeric("ArrivedQty", header: "In Qty", iseditingreadonly: true, decimal_places: 2, width: Widths.AnsiChars(4))
            .Numeric("ReleasedQty", header: "Out Qty", iseditingreadonly: true, decimal_places: 2, width: Widths.AnsiChars(4))
            .Numeric("AdjustQty", header: "Adjust Qty", iseditingreadonly: true, decimal_places: 2, width: Widths.AnsiChars(4))
            .Numeric("Balance", header: "Bal. Qty", iseditingreadonly: true, decimal_places: 2, width: Widths.AnsiChars(4))
            .Text("Location", header: "Location", iseditingreadonly: true)
            .Text("ContainerCode", header: "Container Code", iseditingreadonly: true, width: Widths.AnsiChars(14)).Get(out cbb_ContainerCode_Right);

            cbb_ContainerCode_Left.Visible = Automation.UtilityAutomation.IsAutomationEnable;
            cbb_ContainerCode_Right.Visible = Automation.UtilityAutomation.IsAutomationEnable;
            this.Query();
        }

        private void Query()
        {
            string sqlcmd = $@"
            DECLARE @POID VARCHAR(13) = '{this.drMain["POID"]}'
            DECLARE @Seq1 VARCHAR(3) = '{this.drMain["Seq1"]}'
            DECLARE @Seq2 VARCHAR(2) = '{this.drMain["Seq2"]}'

            select  *   
            into #tmp
            from (
                select  [Date] =lor.WhseArrival,
                        [Transaction] = lor.ID,
                        [Name] = 'P70. Material Receiving (Local Order)',
                        [ArrivedQty] = lord.Qty,
                        [ReleasedQty] = 0,
                        [AdjustQty] = 0,
                        [Location] = lord.Location,
                        [Remark] = lor.Remark,
			            [POID] =lord.POID,
			            [Seq1] = lord.Seq1,
			            [Seq2] = lord.Seq2,
			            [Roll] = lord.Roll,
			            [Dyelot] = lord.Dyelot,
			            [StockType] = lord.StockType,
			            [ContainerCode] = lord.ContainerCode,
			            [Tone] = lord.Tone
                        , lor.EditDate
                from    LocalOrderReceiving_Detail lord with (nolock)
                inner join  LocalOrderReceiving lor  with (nolock) on lor.ID = lord.ID
                where   lord.POID = @POID and
                        lord.Seq1 = @Seq1 AND
			            lord.Seq2 = @Seq2 AND
                        lor.Status = 'Confirmed'
                union all
                select  [Date] = loi.IssueDate,
			            [Transaction] = loi.ID,
			            [Name] = 'P71. Issue Local Order Material',
			            [ArrivedQty] = 0,
                        [ReleasedQty] = loid.Qty,
                        [AdjustQty] = 0,
                        [Location] = '',
                        [Remark] = loi.Remark,
			            [POID] =loid.POID,
			            [Seq1] = loid.Seq1,
			            [Seq2] = loid.Seq2,
			            [Roll] = loid.Roll,
			            [Dyelot] = loid.Dyelot,
			            [StockType] = loid.StockType,
			            [ContainerCode] = '',
			            [Tone] = ''
                        , loi.EditDate
                from    LocalOrderIssue_Detail loid with (nolock)
                inner   join  LocalOrderIssue loi with (nolock) on loid.ID = loi.ID
                where   loid.POID = @POID and
                        loid.Seq1 = @Seq1 AND
			            loid.Seq2 = @Seq2 AND
                        loi.Status = 'Confirmed'
                union all
                select  [Date] = loat.IssueDate,
			            [Transaction] = loat.ID,
			            [Name] = 'P72. Adjust Bulk Qty (Local Order)',
			            [ArrivedQty] = 0,
                        [ReleasedQty] = 0,
                        [AdjustQty] = loatd.QtyAfter - loatd.QtyBefore,
                        [Location] = '',
                        [Remark] = loat.Remark,
			            [POID] =loatd.POID,
			            [Seq1] = loatd.Seq1,
			            [Seq2] = loatd.Seq2,
			            [Roll] = loatd.Roll,
			            [Dyelot] = loatd.Dyelot,
			            [StockType] = loatd.StockType,
			            [ContainerCode] = '',
			            [Tone] = ''
                        , loat.EditDate
                from    LocalOrderAdjust_Detail loatd with (nolock)
                inner   join  LocalOrderAdjust loat with (nolock) on loat.ID = loatd.ID
                where   loatd.POID = @POID and
                        loatd.Seq1 = @Seq1 AND
			            loatd.Seq2 = @Seq2 AND
                        loat.Status = 'Confirmed'
            ) a

            SELECT
            *,[NO] = DENSE_RANK() OVER (ORDER BY d.poid, d.seq1, d.seq2, d.Roll, d.dyelot, d.stocktype)
            into #tmpDetail
            from #tmp d
            ------------------------  Detail 資訊  ------------------------
            SELECT 
            [DATE],
            [Transaction],
            [Name],
            [ArrivedQty] = sum(ArrivedQty),
            [ReleasedQty] = sum(ReleasedQty),
            [AdjustQty] = sum(AdjustQty),
            [balance]  = sum(sum(ArrivedQty) - sum(ReleasedQty) + sum(AdjustQty)) over (order by [Date], [EditDate], [Transaction]) ,
            [Location] = [Location].val,
            [Remark],
            [POID]
            from #tmpDetail a
            OUTER APPLY
            (
	            select val = stuff((
	            select concat(',',tmp.[Location]) from
	            (
		            SELECT distinct  t.[Location]
		            FROM #tmpDetail t
		            WHERE  t.[DATE] = a.[DATE] and t.[Transaction] = a.[Transaction] and t.[Name] = a.[Name]
	            ) tmp for xml path('')),1,1,'')
            )Location
            GROUP BY [Date], [EditDate], [Transaction],[Name],[Remark],[POID],Location.val
            ORDER BY [Date], [EditDate], [Transaction]
            ------------------------   Left 資訊   ------------------------
            SELECT 
            loi.POID,
            loi.Seq1,
            loi.Seq2,
            loi.Roll,
            loi.Dyelot,
            loi.StockType,
            loi.Tone,
            loi.InQty,
            loi.OutQty,
            loi.AdjustQty,
            [Balance] = (loi.InQty - loi.OutQty + loi.AdjustQty) ,
            [BulkLocation] = BulkLocation.val ,
            loi.ContainerCode
            FROM LocalOrderInventory loi with (nolock)
            OUTER APPLY
            (
	            select val = stuff((
	            select concat(',',tmp.MtlLocationID) from
	            (
		            SELECT distinct  lol.MtlLocationID
		            FROM LocalOrderInventory a WITH(NOLOCK)
		            INNER JOIN LocalOrderInventory_Location lol ON lol.LocalOrderInventoryUkey = loi.Ukey
		            WHERE a.POID = loi.POID AND a.Seq1 = loi.Seq1 AND a.Seq2 = loi.Seq2
		            and lol.MtlLocationID != ''
		            and lol.MtlLocationID is not null
	            ) tmp for xml path('')),1,1,'')
            )BulkLocation
            WHERE loi.POID = @POID AND loi.Seq1 = @Seq1 AND loi.Seq2 = @Seq2
            ORDER BY loi.Roll,loi.Dyelot

            ------------------------ Right 資訊 ------------------------
            ;WITH CumulativeQty AS 
            (
                SELECT 
                d.*,
                [CumulativeArrivedQty] = SUM(ArrivedQty) OVER (PARTITION BY d.poid, d.seq1, d.seq2, d.Roll, d.dyelot, d.stocktype ORDER BY [No], [Date], [EditDate], [Transaction]),
                [CumulativeReleasedQty] = SUM(ReleasedQty) OVER (PARTITION BY d.poid, d.seq1, d.seq2, d.Roll, d.dyelot, d.stocktype ORDER BY [No], [Date], [EditDate], [Transaction]),
                [CumulativeAdjustQty] = SUM(AdjustQty) OVER (PARTITION BY d.poid, d.seq1, d.seq2, d.Roll, d.dyelot, d.stocktype ORDER BY [No], [Date], [EditDate], [Transaction])
                FROM #tmpDetail d
                group by poid,seq1,seq2,d.Roll,dyelot,stocktype,tone,date,location,remark,ContainerCode,[Transaction],name, [EditDate],[No],ArrivedQty,ReleasedQty,AdjustQty
            )
            SELECT 
                [POID] = d.poid,
                [Seq1] = d.seq1,
                [Seq2] = d.Seq2,
                [Roll] = d.Roll,
                [Dyelot] = d.Dyelot,
                [StockType] = d.StockType,
                [Date] = d.Date,
                [Transaction] = d.[Transaction],
                [Name] = d.[Name],
                [ArrivedQty] = SUM(d.ArrivedQty),
                [ReleasedQty] = SUM(d.ReleasedQty),
                [AdjustQty] = SUM(d.AdjustQty),
                [Balance] = [CumulativeArrivedQty] - [CumulativeReleasedQty] + [CumulativeAdjustQty],
                [Location] = d.[Location],
                [Remark] = d.[Remark],
                [NO] = d.[No]
            FROM CumulativeQty d
            GROUP BY poid, seq1, seq2, d.Roll, dyelot, stocktype, tone, date, location, remark, ContainerCode, [Transaction], name, [EditDate], [No], [CumulativeArrivedQty], [CumulativeReleasedQty] , [CumulativeAdjustQty]
            ORDER BY [No], [Date], [EditDate], [Transaction]
            
			------------------------ 表頭資訊 ------------------------
            select 
            InQty = sum(ArrivedQty),
            OutQty = sum(ReleasedQty),
            AdjustQty = sum(AdjustQty),
            Balance = sum(ArrivedQty) - sum(ReleasedQty) + sum(AdjustQty)
            from #tmpDetail
            drop table #tmpDetail,#tmp";
            DataTable[] dtResults;
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out dtResults);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.gridDetail.DataSource = dtResults[0];
            this.listLeftDataBindingSource.DataSource = dtResults[1];
            this.dtRight = dtResults[2];

            if (this.isReCalculate)
            {
                if (dtResults[3].Rows.Count <= 0)
                {
                    return;
                }

                this.displayInQty.Text = MyUtility.Convert.GetDecimal(dtResults[3].Rows[0]["InQty"]).ToString();
                this.displayOutQty.Text = MyUtility.Convert.GetDecimal(dtResults[3].Rows[0]["OutQty"]).ToString();
                this.displayAdjustQry.Text = MyUtility.Convert.GetDecimal(dtResults[3].Rows[0]["AdjustQty"]).ToString();
                this.displayBalQty.Text = MyUtility.Convert.GetDecimal(dtResults[3].Rows[0]["Balance"]).ToString();
            }
        }

        private void GridLeft_SelectionChanged(object sender, EventArgs e)
        {
            if (this.gridLeft.SelectedRows.Count == 0)
            {
                this.gridRight.DataSource = null;
                return;
            }

            var nowPosition = this.listLeftDataBindingSource.Position;
            var dt = (DataTable)this.listLeftDataBindingSource.DataSource;

            string poid = dt.Rows[nowPosition]["POID"].ToString();
            string seq1 = dt.Rows[nowPosition]["seq1"].ToString();
            string seq2 = dt.Rows[nowPosition]["seq2"].ToString();
            string roll = dt.Rows[nowPosition]["Roll"].ToString();
            string dyelot = dt.Rows[nowPosition]["Dyelot"].ToString();
            string stocktype = dt.Rows[nowPosition]["StockType"].ToString();

            if (this.dtRight == null || this.dtRight.Rows.Count == 0)
            {
                return;
            }

            var srcRight = this.dtRight.AsEnumerable()
                                        .Where(s => s["POID"].ToString() == poid &&
                                                    s["Seq1"].ToString() == seq1 &&
                                                    s["Seq2"].ToString() == seq2 &&
                                                    s["Roll"].ToString() == roll &&
                                                    s["Dyelot"].ToString() == dyelot &&
                                                    s["StockType"].ToString() == stocktype);

            if (!srcRight.Any())
            {
                this.gridRight.DataSource = null;
                return;
            }

            this.gridRight.DataSource = srcRight.CopyToDataTable();
        }

        private void BtnReCalculate_Click(object sender, EventArgs e)
        {
            string sqlcmd = $@"
            DECLARE @POID VARCHAR(13) = '{this.drMain["POID"]}'
            DECLARE @Seq1 VARCHAR(3) = '{this.drMain["Seq1"]}'
            DECLARE @Seq2 VARCHAR(2) = '{this.drMain["Seq2"]}'

            select  POID,Seq1,Seq2,Roll,Dyelot,StockType, InQty = sum(ArrivedQty), OutQty = sum(ReleasedQty), AdjustQty = sum(AdjustQty)
            into    #tmpDetail
            from (
                select  [Date] =lor.WhseArrival,
                        [Transaction] = lor.ID,
                        [Name] = 'P70. Material Receiving (Local Order)',
                        [ArrivedQty] = lord.Qty,
                        [ReleasedQty] = 0,
                        [AdjustQty] = 0,
                        [Location] = lord.Location,
                        [Remark] = lor.Remark,
			            [POID] =lord.POID,
			            [Seq1] = lord.Seq1,
			            [Seq2] = lord.Seq2,
			            [Roll] = lord.Roll,
			            [Dyelot] = lord.Dyelot,
			            [StockType] = lord.StockType,
			            [ContainerCode] = lord.ContainerCode,
			            [Tone] = lord.Tone
                from    LocalOrderReceiving_Detail lord with (nolock)
                inner join  LocalOrderReceiving lor  with (nolock) on lor.ID = lord.ID
                where   lord.POID = @POID and
                        lord.Seq1 = @Seq1 AND
			            lord.Seq2 = @Seq2 AND
                        lor.Status = 'Confirmed'
                union all
                select  [Date] = loi.IssueDate,
			            [Transaction] = loi.ID,
			            [Name] = 'P71. Issue Local Order Material',
			            [ArrivedQty] = 0,
                        [ReleasedQty] = loid.Qty,
                        [AdjustQty] = 0,
                        [Location] = '',
                        [Remark] = loi.Remark,
			            [POID] =loid.POID,
			            [Seq1] = loid.Seq1,
			            [Seq2] = loid.Seq2,
			            [Roll] = loid.Roll,
			            [Dyelot] = loid.Dyelot,
			            [StockType] = loid.StockType,
			            [ContainerCode] = '',
			            [Tone] = ''
                from    LocalOrderIssue_Detail loid with (nolock)
                inner   join  LocalOrderIssue loi with (nolock) on loid.ID = loi.ID
                where   loid.POID = @POID and
                        loid.Seq1 = @Seq1 AND
			            loid.Seq2 = @Seq2 AND
                        loi.Status = 'Confirmed'
                union all
                select  [Date] = loat.IssueDate,
			            [Transaction] = loat.ID,
			            [Name] = 'P72. Adjust Bulk Qty (Local Order)',
			            [ArrivedQty] = 0,
                        [ReleasedQty] = 0,
                        [AdjustQty] = loatd.QtyAfter - loatd.QtyBefore,
                        [Location] = '',
                        [Remark] = loat.Remark,
			            [POID] =loatd.POID,
			            [Seq1] = loatd.Seq1,
			            [Seq2] = loatd.Seq2,
			            [Roll] = loatd.Roll,
			            [Dyelot] = loatd.Dyelot,
			            [StockType] = loatd.StockType,
			            [ContainerCode] = '',
			            [Tone] = ''
                from    LocalOrderAdjust_Detail loatd with (nolock)
                inner   join  LocalOrderAdjust loat with (nolock) on loat.ID = loatd.ID
                where   loatd.POID = @POID and
                        loatd.Seq1 = @Seq1 AND
			            loatd.Seq2 = @Seq2 AND
                        loat.Status = 'Confirmed'
            ) a
            group by POID,Seq1,Seq2,Roll,Dyelot,StockType

            update loi
            set
                InQty = t.InQty,
                OutQty = t.OutQty,
                AdjustQty = t.AdjustQty
            from LocalOrderInventory loi  
            inner join #tmpDetail  t on loi.POID        = t.POID        AND
							            loi.Seq1        = t.Seq1        AND
							            loi.Seq2        = t.Seq1        AND
							            loi.Roll        = t.Roll        AND
							            loi.Dyelot      = t.Dyelot      AND
							            loi.StockType   = t.StockType
            DROP TABLE #tmpDetail";
            DualResult result = DBProxy.Current.Execute(null, sqlcmd);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            MyUtility.Msg.InfoBox("Finished!");
            this.isReCalculate = true;
            this.Query();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
