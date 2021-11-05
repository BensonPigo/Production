using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Win;
using Sci.Data;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public partial class R11 : Win.Tems.PrintForm
    {
        private DataTable[] printData;
        private StringBuilder sqlcmd = new StringBuilder();
        private List<SqlParameter> sqlParameters = new List<SqlParameter>();

        /// <inheritdoc/>
        public R11(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.EditMode = true;
            this.txtMdivision.Text = Sci.Env.User.Keyword;
            this.txtfactory.Text = Sci.Env.User.Factory;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.sqlcmd.Clear();
            this.sqlParameters.Clear();

            if (!this.dateSCIDelivery.Value1.HasValue &&
                !this.dateBuyerDelivery.Value1.HasValue &&
                !this.dateBuyerDelivery.Value1.HasValue &&
                MyUtility.Check.Empty(this.txtSP1.Text) &&
                MyUtility.Check.Empty(this.txtSP2.Text))
            {
                MyUtility.Msg.WarningBox("Please input a blue requirement at least.");
                return false;
            }

            string where = string.Empty;
            if (this.dateSCIDelivery.Value1.HasValue)
            {
                where += "\r\nand o.SciDelivery between @SCIDelivery1 and @SCIDelivery2";
                this.sqlParameters.Add(new SqlParameter("@SCIDelivery1", SqlDbType.Date) { Value = this.dateSCIDelivery.Value1 });
                this.sqlParameters.Add(new SqlParameter("@SCIDelivery2", SqlDbType.Date) { Value = this.dateSCIDelivery.Value2 });
            }

            if (this.dateBuyerDelivery.Value1.HasValue)
            {
                where += "\r\nand o.BuyerDelivery between @BuyerDelivery1 and @BuyerDelivery2";
                this.sqlParameters.Add(new SqlParameter("@BuyerDelivery1", SqlDbType.Date) { Value = this.dateBuyerDelivery.Value1 });
                this.sqlParameters.Add(new SqlParameter("@BuyerDelivery2", SqlDbType.Date) { Value = this.dateBuyerDelivery.Value2 });
            }

            if (this.dateSewInLine.Value1.HasValue)
            {
                where += "\r\nand o.SewInLine between @SewInLine1 and @SewInLine2";
                this.sqlParameters.Add(new SqlParameter("@SewInLine1", SqlDbType.Date) { Value = this.dateSewInLine.Value1 });
                this.sqlParameters.Add(new SqlParameter("@SewInLine2", SqlDbType.Date) { Value = this.dateSewInLine.Value2 });
            }

            if (!this.txtSP1.Text.Empty())
            {
                where += "\r\nand o.POID >= @SP1";
                this.sqlParameters.Add(new SqlParameter("@SP1", SqlDbType.VarChar, 13) { Value = this.txtSP1.Text });
            }

            if (!this.txtSP2.Text.Empty())
            {
                where += "\r\nand o.POID <= @SP2";
                this.sqlParameters.Add(new SqlParameter("@SP2", SqlDbType.VarChar, 13) { Value = this.txtSP2.Text });
            }

            if (!this.txtstyle1.Text.Empty())
            {
                where += "\r\nand o.StyleID = @StyleID";
                this.sqlParameters.Add(new SqlParameter("@StyleID", SqlDbType.VarChar, 15) { Value = this.txtstyle1.Text });
            }

            if (!this.txtbrand1.Text.Empty())
            {
                where += "\r\nand o.BrandID = @BrandID";
                this.sqlParameters.Add(new SqlParameter("@BrandID", SqlDbType.VarChar, 8) { Value = this.txtbrand1.Text });
            }

            if (!this.txtMdivision.Text.Empty())
            {
                where += "\r\nand o.MDivisionid = @M";
                this.sqlParameters.Add(new SqlParameter("@M", SqlDbType.VarChar, 8) { Value = this.txtMdivision.Text });
            }

            if (!this.txtfactory.Text.Empty())
            {
                where += "\r\nand o.FtyGroup = @FtyGroup";
                this.sqlParameters.Add(new SqlParameter("@FtyGroup", SqlDbType.VarChar, 8) { Value = this.txtfactory.Text });
            }

            this.sqlcmd.Append($@"
select ID, poid, StyleID, o.StyleUkey into #tmpOrders from Orders o where 1=1 {where}

select distinct	o.POID, o.StyleID, o.StyleUkey, oq.Article, oq.SizeCode
into #allAS
from #tmpOrders o
inner join Order_Qty oq on oq.ID = o.ID

--展開到 MarkerName, SizeCode (沒有 Article)
Select
	o.POID,
	o.StyleID,
	oe.FabricCombo,
	bof.Refno,
	oe.MarkerNo,
	oe.MarkerName,
	oe.MarkerLength,
	Fabric.Width,
	oes.SizeCode,
	Ratio = oes.Qty,
	ConsPC_SizeRatio =isnull(oe.ConsPC, 0) * isnull(oes.Qty, 0) / sum(oes.Qty) over(partition by oe.Ukey) * 1.0, -- ConsPC 用佔比分配
	oe.Article, -- 此欄位內容是紀錄多個Article
	oe.FabricPanelCode
into #tmp
From (select distinct POID, StyleID, StyleUkey from #tmpOrders) o
inner join Order_EachCons oe WITH (NOLOCK) on o.POID = oe.Id
left join Order_BOF bof WITH (NOLOCK) on bof.Id = oe.Id and bof.FabricCode = oe.FabricCode
left join Fabric WITH (NOLOCK) on Fabric.SCIRefno = bof.SCIRefno
left join Order_EachCons_SizeQty oes WITH (NOLOCK) on oes.Order_EachConsUkey = oe.Ukey

--把 Article 用逗號拆開, 展開
select t.POID, t.SizeCode, s.Article, t.ConsPC_SizeRatio, t.FabricCombo, t.Refno, t.MarkerNo, t.MarkerName, t.FabricPanelCode
into #byArticle
from #tmp t
outer apply(select Article = RTRIM(LTRIM(Data)) from [dbo].[SplitString](t.Article, ',') where data <>'')s

--把空白的 Article 補上所有 Article
select t.POID, t.SizeCode, x.Article, t.ConsPC_SizeRatio, t.FabricCombo, t.Refno, t.MarkerNo, t.MarkerName, t.FabricPanelCode
into #byArticleALL
from #byArticle t
outer apply(select Article from #allAS a where a.POID = t.POID and a.SizeCode = t.SizeCode)x
where isnull(t.Article, '') = ''
union all
select * from #byArticle t where Article <>''

--sheet 1
select
	t.POID,
	t.StyleID,
	t.Article,
	t.SizeCode,
	ConsPC = sum(ConsPC_SizeRatio)
from #allAS t
left join #byArticleALL a on a.POID = t.POID and a.SizeCode = t.SizeCode and a.Article = t.Article 
group by t.POID, t.StyleID, t.Article,t.SizeCode
order by t.POID, t.StyleID, t.Article,t.SizeCode
-----------------------------------------------------------------------------------------------------------------------------------------------------
--取得 PatternUkey
select t.POID, t.SizeCode, s.PatternUkey
into #sizePatternUkey
from (select distinct t.POID,t.StyleUkey,t.SizeCode from #allAS t) t
outer apply(select s.PatternUkey from dbo.GetPatternUkey(t.POID, '', '', t.StyleUkey, t.SizeCode)s)s

--準備 展開到 MarkerName, SizeCode, Article 的 FabricPanelCode, PatternUkey 資料
select x.*, s.PatternUkey
into #MarkerName_AS
from(select distinct t.POID, t.FabricCombo, t.Refno, t.MarkerNo, t.MarkerName, t.SizeCode, t.Article, t.FabricPanelCode	from #byArticleALL t)x
inner join #sizePatternUkey s on s.POID = x.POID and s.SizeCode = x.SizeCode

--找 Artwork 需要的 Article 都要帶入, 先準備 Articl 的 xml
select
	t.POID,
	t.StyleID,
	t.FabricCombo,
	t.Refno,
	t.MarkerNo,
	t.MarkerName,
	t.MarkerLength,
	t.Width,
	t.SizeCode,
	t.Ratio,
	ConsPC = t.ConsPC_SizeRatio,

	m.xml,
	t.FabricPanelCode
into #xmlArticle
from #tmp t
outer apply(
	select xml = (select distinct Article,PatternUkey from #MarkerName_AS m where  m.POID = t.POID and m.SizeCode = t.SizeCode and m.MarkerNo = t.MarkerNo  and m.MarkerName = t.MarkerName for XML RAW)
)m

--相同 << POID, SizeCode, FabricPanelCode, (多個Article), PatternUkey >> 組合 先縮減 再分別找 Artwork (為了提升效能)
select t.*, Artwork = dbo.GetArtwork(t.xml,t.SizeCode,t.FabricPanelCode)
into #ArtworkS
from(select distinct t.POID, t.SizeCode, t.FabricPanelCode, t.xml from #xmlArticle t)t

--sheet 2  展開到 MarkerName, SizeCode (多個 Article)
select
	t.POID,
	t.StyleID,
	t.FabricCombo,
	t.Refno,
	t.MarkerNo,
	t.MarkerName,
	t.MarkerLength,
	t.Width,
	t.SizeCode,
	t.Ratio,
	t.ConsPC,
	b.Artwork
from #xmlArticle t
inner join #ArtworkS b on b.POID = t.POID and b.SizeCode = t.SizeCode and b.FabricPanelCode = t.FabricPanelCode and b.xml = t.xml
order by t.POID, t.StyleID, t.FabricCombo, t.Refno, t.MarkerNo, t.MarkerName

drop table #tmp,#byArticle,#tmpOrders,#allAS,#byArticleALL,#sizePatternUkey,#MarkerName_AS,#xmlArticle,#ArtworkS
");

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            return DBProxy.Current.Select(null, this.sqlcmd.ToString(), this.sqlParameters, out this.printData);
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData[1].Rows.Count);
            if (this.printData[1].Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            string filename = "Cutting_R11.xltx";
            Excel.Application excelapp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\" + filename); // 預先開啟excel app
            if (this.printData[0].Rows.Count > 0)
            {
                MyUtility.Excel.CopyToXls(this.printData[0], string.Empty, filename, 1, false, null, excelapp, wSheet: excelapp.Sheets[1]);
            }

            if (this.printData[1].Rows.Count > 0)
            {
                MyUtility.Excel.CopyToXls(this.printData[1], string.Empty, filename, 1, false, null, excelapp, wSheet: excelapp.Sheets[2]);
            }

            excelapp.Columns.AutoFit();
            string excelfile = Class.MicrosoftFile.GetName("Cutting_R11");
            excelapp.ActiveWorkbook.SaveAs(excelfile);
            excelapp.Visible = true;
            Marshal.ReleaseComObject(excelapp);
            return true;
        }
    }
}
