using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// P01_ProductionOutput
    /// </summary>
    public partial class P01_ProductionOutput : Sci.Win.Subs.Base
    {
        private DataRow masterData;
        private string cuttingWorkType;
        private Ict.Win.DataGridViewGeneratorNumericColumnSettings sewingqty = new Ict.Win.DataGridViewGeneratorNumericColumnSettings();
        private Ict.Win.DataGridViewGeneratorNumericColumnSettings t = new Ict.Win.DataGridViewGeneratorNumericColumnSettings();
        private Ict.Win.DataGridViewGeneratorNumericColumnSettings b = new Ict.Win.DataGridViewGeneratorNumericColumnSettings();
        private Ict.Win.DataGridViewGeneratorNumericColumnSettings i = new Ict.Win.DataGridViewGeneratorNumericColumnSettings();
        private Ict.Win.DataGridViewGeneratorNumericColumnSettings o = new DataGridViewGeneratorNumericColumnSettings();
        private Ict.Win.DataGridViewGeneratorNumericColumnSettings cuttingqty = new Ict.Win.DataGridViewGeneratorNumericColumnSettings();
        private Ict.Win.DataGridViewGeneratorNumericColumnSettings loadoutput = new Ict.Win.DataGridViewGeneratorNumericColumnSettings();

        /// <summary>
        /// P01_ProductionOutput
        /// </summary>
        /// <param name="masterData">DataRow MasterData</param>
        public P01_ProductionOutput(DataRow masterData)
        {
            this.InitializeComponent();
            this.masterData = masterData;
            this.Text = "Production output - " + MyUtility.Convert.GetString(this.masterData["ID"]);
            this.cuttingWorkType = MyUtility.GetValue.Lookup(string.Format("select WorkType from Cutting WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(this.masterData["CuttingSP"])));
            this.tabPage2.Text = "Cutting output";
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            // 撈Summary資料
            string sqlCmd = string.Format(
                @"select (select Max(s.OutputDate)
		from SewingOutput_Detail sd WITH (NOLOCK) 
		inner join SewingOutput s WITH (NOLOCK) on sd.ID = s.ID
		where sd.OrderId = '{0}') as LastSewingDate,
isnull((dbo.getMinCompleteSewQty('{0}',null,null)),0) as SewingQty,
isnull((select SUM(c.Qty)
	   from Orders o WITH (NOLOCK) 
	   inner join CuttingOutput_WIP c WITH (NOLOCK) on o.ID = c.OrderID
	   where {1}),0) as CutQty", MyUtility.Convert.GetString(this.masterData["ID"]),
                string.Format("o.ID = '{0}'", MyUtility.Convert.GetString(this.masterData["ID"])));
            DataTable summaryQty;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out summaryQty);
            this.dateLastSewingOutputDate.Value = MyUtility.Convert.GetDate(summaryQty.Rows[0]["LastSewingDate"]);
            this.numSewingOrderQty.Value = MyUtility.Convert.GetInt(this.masterData["Qty"]);
            this.numOrderQty.Value = MyUtility.Convert.GetInt(this.masterData["Qty"]);
            this.numOrderQty_L.Value = MyUtility.Convert.GetInt(this.masterData["Qty"]);

            this.sewingqty.CellMouseDoubleClick += (s, e) =>
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    DataRow dr = this.gridSewingOutput.GetDataRow<DataRow>(e.RowIndex);
                    Sci.Production.PPIC.P01_ProductionOutput_SewingDetail callNextForm = new Sci.Production.PPIC.P01_ProductionOutput_SewingDetail(MyUtility.Convert.GetString(this.masterData["ID"]), "S", MyUtility.Convert.GetString(dr["Article"]), MyUtility.Convert.GetString(dr["SizeCode"]));
                    callNextForm.ShowDialog(this);
                }
            };

            this.t.CellMouseDoubleClick += (s, e) =>
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    DataRow dr = this.gridSewingOutput.GetDataRow<DataRow>(e.RowIndex);
                    Sci.Production.PPIC.P01_ProductionOutput_SewingDetail callNextForm = new Sci.Production.PPIC.P01_ProductionOutput_SewingDetail(MyUtility.Convert.GetString(this.masterData["ID"]), "T", MyUtility.Convert.GetString(dr["Article"]), MyUtility.Convert.GetString(dr["SizeCode"]));
                    callNextForm.ShowDialog(this);
                }
            };

            this.b.CellMouseDoubleClick += (s, e) =>
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    DataRow dr = this.gridSewingOutput.GetDataRow<DataRow>(e.RowIndex);
                    Sci.Production.PPIC.P01_ProductionOutput_SewingDetail callNextForm = new Sci.Production.PPIC.P01_ProductionOutput_SewingDetail(MyUtility.Convert.GetString(this.masterData["ID"]), "B", MyUtility.Convert.GetString(dr["Article"]), MyUtility.Convert.GetString(dr["SizeCode"]));
                    callNextForm.ShowDialog(this);
                }
            };

            this.i.CellMouseDoubleClick += (s, e) =>
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    DataRow dr = this.gridSewingOutput.GetDataRow<DataRow>(e.RowIndex);
                    Sci.Production.PPIC.P01_ProductionOutput_SewingDetail callNextForm = new Sci.Production.PPIC.P01_ProductionOutput_SewingDetail(MyUtility.Convert.GetString(this.masterData["ID"]), "I", MyUtility.Convert.GetString(dr["Article"]), MyUtility.Convert.GetString(dr["SizeCode"]));
                    callNextForm.ShowDialog(this);
                }
            };

            this.o.CellMouseDoubleClick += (s, e) =>
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    DataRow dr = this.gridSewingOutput.GetDataRow<DataRow>(e.RowIndex);
                    Sci.Production.PPIC.P01_ProductionOutput_SewingDetail callNextForm = new Sci.Production.PPIC.P01_ProductionOutput_SewingDetail(MyUtility.Convert.GetString(this.masterData["ID"]), "O", MyUtility.Convert.GetString(dr["Article"]), MyUtility.Convert.GetString(dr["SizeCode"]));
                    callNextForm.ShowDialog(this);
                }
            };

            // 設定Grid1的顯示欄位
            this.gridSewingOutput.IsEditingReadOnly = true;
            this.gridSewingOutput.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridSewingOutput)
                 .Text("Article", header: "Colorway", width: Widths.AnsiChars(8))
                 .Text("SizeCode", header: "Size", width: Widths.AnsiChars(8))
                 .Numeric("Qty", header: "Order Q'ty", width: Widths.AnsiChars(6))
                 .Numeric("SewQty", header: "Sewing Q'ty", width: Widths.AnsiChars(6), settings: this.sewingqty)
                 .Numeric("T", header: "Top", width: Widths.AnsiChars(6), settings: this.t)
                 .Numeric("B", header: "Bottom", width: Widths.AnsiChars(6), settings: this.b)
                 .Numeric("I", header: "Inner", width: Widths.AnsiChars(6), settings: this.i)
                 .Numeric("O", header: "Outer", width: Widths.AnsiChars(6), settings: this.o);

            #region 控制Column是否可被看見
            this.gridSewingOutput.Columns[4].Visible = false;
            this.gridSewingOutput.Columns[5].Visible = false;
            this.gridSewingOutput.Columns[6].Visible = false;
            this.gridSewingOutput.Columns[7].Visible = false;
            if (MyUtility.Convert.GetString(this.masterData["StyleUnit"]) == "SETS")
            {
                sqlCmd = string.Format("select Location from Style_Location WITH (NOLOCK) where StyleUkey = {0}", MyUtility.Convert.GetString(this.masterData["StyleUKey"]));
                DataTable styleLocation;
                result = DBProxy.Current.Select(null, sqlCmd, out styleLocation);
                if (styleLocation != null)
                {
                    foreach (DataRow dr in styleLocation.Rows)
                    {
                        if (MyUtility.Convert.GetString(dr["Location"]) == "T")
                        {
                            this.gridSewingOutput.Columns[4].Visible = true;
                        }

                        if (MyUtility.Convert.GetString(dr["Location"]) == "B")
                        {
                            this.gridSewingOutput.Columns[5].Visible = true;
                        }

                        if (MyUtility.Convert.GetString(dr["Location"]) == "I")
                        {
                            this.gridSewingOutput.Columns[6].Visible = true;
                        }

                        if (MyUtility.Convert.GetString(dr["Location"]) == "O")
                        {
                            this.gridSewingOutput.Columns[7].Visible = true;
                        }
                    }
                }
            }
            #endregion

            for (int j = 0; j < this.gridSewingOutput.ColumnCount; j++)
            {
                this.gridSewingOutput.Columns[j].SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            // 當Article變動時，幣一筆不一樣的資料背景顏色要改變
            this.gridSewingOutput.RowsAdded += (s, e) =>
            {
                DataTable dtData = (DataTable)this.listControlBindingSource1.DataSource;
                for (int j = 0; j < e.RowCount; j++)
                {
                    if (!MyUtility.Check.Empty(dtData.Rows[j]["LastArticle"]) && dtData.Rows[j]["LastArticle"].ToString() != dtData.Rows[j]["Article"].ToString())
                    {
                        this.gridSewingOutput.Rows[j].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 128);
                    }
                    else
                    {
                        this.gridSewingOutput.Rows[j].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255);
                    }
                }
            };

            this.cuttingqty.CellMouseDoubleClick += (s, e) =>
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    DataRow dr = this.gridCutting.GetDataRow<DataRow>(e.RowIndex);
                    Sci.Production.PPIC.P01_ProductionOutput_CuttingDetail callNextForm = new Sci.Production.PPIC.P01_ProductionOutput_CuttingDetail(this.cuttingWorkType, MyUtility.Convert.GetString(this.masterData["ID"]), "C", MyUtility.Convert.GetString(dr["Article"]), MyUtility.Convert.GetString(dr["SizeCode"]));
                    callNextForm.ShowDialog(this);
                }
            };

            // 設定Grid2的顯示欄位
            this.gridCutting.IsEditingReadOnly = true;
            this.gridCutting.DataSource = this.listControlBindingSource2;
            this.Helper.Controls.Grid.Generator(this.gridCutting)
                 .Text("Article", header: "Colorway", width: Widths.AnsiChars(8))
                 .Text("SizeCode", header: "Size", width: Widths.AnsiChars(8))
                 .Numeric("Qty", header: "Order Q'ty", width: Widths.AnsiChars(6))
                 .Numeric("CutQty", header: "Cutting Q'ty", width: Widths.AnsiChars(6), settings: this.cuttingqty);

            for (int j = 0; j < this.gridCutting.ColumnCount; j++)
            {
                this.gridCutting.Columns[j].SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            this.loadoutput.CellMouseDoubleClick += (s, e) =>
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    DataRow dr = this.gridLoading.GetDataRow<DataRow>(e.RowIndex);
                    Sci.Production.PPIC.P01_ProductionOutput_LoadingoutputDetail callNextForm = new Sci.Production.PPIC.P01_ProductionOutput_LoadingoutputDetail(MyUtility.Convert.GetString(this.masterData["ID"]), "S", MyUtility.Convert.GetString(dr["Article"]), MyUtility.Convert.GetString(dr["SizeCode"]));
                    callNextForm.ShowDialog(this);
                }
            };

            // 設定Grid3的顯示欄位
            this.gridLoading.IsEditingReadOnly = true;
            this.gridLoading.DataSource = this.listControlBindingSource3;
            this.Helper.Controls.Grid.Generator(this.gridLoading)
                  .Text("Article", header: "Colorway", width: Widths.AnsiChars(8))
                  .Text("SizeCode", header: "Size", width: Widths.AnsiChars(8))
                  .Numeric("Qty", header: "Order Q'ty", width: Widths.AnsiChars(6))
                  .Numeric("AccuInCome", header: "Loading Output", width: Widths.AnsiChars(6), settings: this.loadoutput);

            // 撈Sewing Data
            string locationTable = string.Empty;
            if (MyUtility.Check.Seek($"select 1 from Order_Location where OrderId = '{this.masterData["ID"]}'"))
            {
                locationTable = "Order_Location  sl WITH (NOLOCK) on sl.OrderId = o.id";
            }
            else
            {
                locationTable = "Style_Location  sl WITH (NOLOCK) on o.StyleUkey = sl.StyleUkey";
            }

            sqlCmd = string.Format(
                @"
with SewQty as (
	select	oq.Article
			, oq.SizeCode
			, oq.Qty
			, ComboType = sl.Location
			, QAQty = isnull(sum(sdd.QAQty),0)
	from Orders o WITH (NOLOCK) 
	inner join {2}
	inner join Order_Qty oq WITH (NOLOCK) on oq.ID = o.ID
	left join SewingOutput_Detail_Detail sdd WITH (NOLOCK) on sdd.OrderId = o.ID 
															  and sdd.Article = oq.Article 
															  and sdd.SizeCode = oq.SizeCode 
															  and sdd.ComboType = sl.Location
	where o.ID = '{0}'
	group by oq.Article,oq.SizeCode,oq.Qty,sl.Location
), 
minSewQty as (
	select	Article
			, SizeCode
			, QAQty = MIN(QAQty)
	from SewQty
	group by Article,SizeCode
),
PivotData as (
	select *
	from SewQty
	PIVOT (SUM(QAQty)
	FOR ComboType IN ([T],[B],[I],[O])) a
)
select	p.*
		, SewQty = m.QAQty 
		, LastArticle = LAG(p.Article,1,null) OVER (Order by oa.Seq,os.Seq)
from PivotData p
left join minSewQty m on m.Article = p.Article and m.SizeCode = p.SizeCode
left join Order_Article oa WITH (NOLOCK) on oa.ID = '{1}' and oa.Article = p.Article
left join Order_SizeCode os WITH (NOLOCK) on os.ID = '{1}' and os.SizeCode = p.SizeCode
order by oa.Seq,os.Seq;",
                MyUtility.Convert.GetString(this.masterData["ID"]),
                MyUtility.Convert.GetString(this.masterData["POID"]),
                locationTable);

            DataTable sewingData;
            result = DBProxy.Current.Select(null, sqlCmd, out sewingData);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            sqlCmd = string.Format(
                @"select oq.Article,oq.SizeCode,oq.Qty,sum(c.Qty) as CutQty
from Orders o WITH (NOLOCK) 
inner join Order_Qty oq WITH (NOLOCK) on oq.id = o.ID
left join CuttingOutput_WIP c WITH (NOLOCK) on c.OrderID = o.ID and c.Article = oq.Article and c.Size = oq.SizeCode
left join Order_Article oa WITH (NOLOCK) on oa.ID = o.POID and oa.Article = oq.Article
left join Order_SizeCode os WITH (NOLOCK) on os.ID = o.POID and os.SizeCode = oq.SizeCode
where {0}
group by oq.Article,oq.SizeCode,oq.Qty,oa.Seq,os.Seq
order by oa.Seq,os.Seq", string.Format("o.ID = '{0}'", MyUtility.Convert.GetString(this.masterData["ID"])));

            DataTable cuttingData;
            result = DBProxy.Current.Select(null, sqlCmd, out cuttingData);

            // bug fix:0000294: PPIC_P01_ProductionOutput
            this.numSewingQty.Value = MyUtility.Convert.GetInt(sewingData.Compute("sum(SewQty)", string.Empty));  // Sewing Q'ty
            this.numCuttingQty.Value = MyUtility.Convert.GetDecimal(cuttingData.Compute("sum(CutQty)", string.Empty));  // Cutting Q'ty

            sqlCmd = $"select OrderID = '{this.masterData["ID"]}', InStartDate = Null,InEndDate = Null,OutStartDate = Null,OutEndDate = Null into #enn ";
            string[] subprocessIDs = new string[] { "Loading" };
            string qtyBySetPerSubprocess = PublicPrg.Prgs.QtyBySetPerSubprocess(subprocessIDs, "#enn", bySP: true, isNeedCombinBundleGroup: true, isMorethenOrderQty: "1");
            sqlCmd += qtyBySetPerSubprocess + $@"
select oq.ID,oq.SizeCode,oq.Qty,AccuInCome=a.InQtyBySet,oq.Article,oq.SizeCode
from Order_Qty oq 
left join #QtyBySetPerSubprocessLoading a on a.OrderID = oq.ID and a.Article = oq.Article and a.SizeCode = oq.SizeCode
where oq.id = '{this.masterData["ID"]}'
";

            DataTable loadingoutput;
            result = DBProxy.Current.Select(null, sqlCmd, out loadingoutput);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.numLoadingQty.Value = MyUtility.Convert.GetDecimal(loadingoutput.Compute("sum(AccuInCome)", string.Empty));  // Sewing Q'ty

            this.listControlBindingSource1.DataSource = sewingData;
            this.listControlBindingSource2.DataSource = cuttingData;
            this.listControlBindingSource3.DataSource = loadingoutput;
        }

        // Sewing Q'ty
        private void NumSewingQty_DoubleClick(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_ProductionOutput_SewingDetail callNextForm = new Sci.Production.PPIC.P01_ProductionOutput_SewingDetail(MyUtility.Convert.GetString(this.masterData["ID"]), "A", string.Empty, string.Empty);
            callNextForm.ShowDialog(this);
        }

        private void NumLoadingQty_DoubleClick(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_ProductionOutput_LoadingoutputDetail callNextForm = new Sci.Production.PPIC.P01_ProductionOutput_LoadingoutputDetail(MyUtility.Convert.GetString(this.masterData["ID"]), "A", string.Empty, string.Empty);
            callNextForm.ShowDialog(this);
        }
    }
}
