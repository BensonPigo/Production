using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Prg;
using Sci.Production.Prg.Entity;
using Sci.Production.PublicPrg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P01_MaterialCompare : Win.Subs.Base
    {
        private DataTable masterDt;
        private DataTable detailDt;
        private string poid;

        /// <summary>
        /// P01_Seq1Item_His
        /// 從Production複製過來
        /// </summary>
        /// <param name="poid">poid</param>
        public P01_MaterialCompare(string poid)
        {
            this.InitializeComponent();
            this.poid = poid;
            this.Text += $"Material Compare - {poid}";
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            #region Master Grid
            this.Helper.Controls.Grid.Generator(this.grid1)
                .Text("Type", header: "Type", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("RefNo", header: "RefNo", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("StockUnit", header: "Stock Unit", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Numeric("StockNetQty", header: "Total Net Qty", width: Widths.AnsiChars(5), iseditingreadonly: true, maximum: 999999, minimum: 0, decimal_places: 2)
                .Numeric("StockLossQty", header: "Total Loss Qty", width: Widths.AnsiChars(5), iseditingreadonly: true, maximum: 999999, minimum: 0, decimal_places: 2)
                .Numeric("StockUnitQty", header: "Total Qty", width: Widths.AnsiChars(5), iseditingreadonly: true, maximum: 999999, minimum: 0, decimal_places: 2)
                .Numeric("StockBalanceQty", header: "Balance Qty", width: Widths.AnsiChars(5), iseditingreadonly: true, maximum: 999999, minimum: 0, decimal_places: 2)
                .Text("SpecColor", header: "Color", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("SpecSize", header: "Size", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("SpecSizeUnit", header: "Size Unit", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("SpecZipperInsert", header: "Zipper Insert", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("SpecArticle", header: "Article", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("SpecCOO", header: "COO", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("SpecGender", header: "Gender", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("SpecCustomerSize", header: "Customer Size", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("SpecDecLabelSize", header: "Dec. Label Size", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("SpecBrandFactoryCode", header: "Brand FTY Code", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("SpecStyle", header: "Style", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("SpecStyleLocation", header: "Style Location", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("SpecSeason", header: "Season", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("SpecCareCode", header: "CareCode", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("SpecCustomerPO", header: "Customer PO", width: Widths.AnsiChars(15), iseditingreadonly: true)
                ;
            #endregion

            #region Detail Grid
            this.Helper.Controls.Grid.Generator(this.grid2)
                .Text("Seq1", header: "Seq1", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Text("Seq2", header: "Seq2", width: Widths.AnsiChars(2), iseditingreadonly: true)
                .Text("RefNo", header: "Ref#", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("SuppID", header: "Supplier", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Date("ShowCfmETD", header: "Sup. del 1st cfm", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Date("ShowRevisedETD", header: "Sup. del. rvsd", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Date("FirstETA", header: "Sup. del. rvsd eta", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("StockUnit", header: "Stock Unit", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Numeric("StockNetQty", header: "NetQty", width: Widths.AnsiChars(5), iseditingreadonly: true, maximum: 999999, minimum: 0, decimal_places: 2)
                .Numeric("StockLossQty", header: "LossQty", width: Widths.AnsiChars(5), iseditingreadonly: true, maximum: 999999, minimum: 0, decimal_places: 2)
                .Numeric("StockUnitQty", header: "Qty", width: Widths.AnsiChars(5), iseditingreadonly: true, maximum: 999999, minimum: 0, decimal_places: 2)
                .Numeric("StockFOC", header: "FOC", width: Widths.AnsiChars(5), iseditingreadonly: true, maximum: 999999, minimum: 0, decimal_places: 2)
                .EditText("OrderIdList", header: "Order List", width: Widths.AnsiChars(20), iseditingreadonly: true)
                ;
            #endregion

            this.BindingGrid();
        }

        private void BindingGrid()
        {
            Dictionary<ComparePO3Item, List<ComparePO3Item>> dicComparePO3 = this.LoadData(this.poid);
            if (dicComparePO3 == null)
            {
                return;
            }

            this.SetTable();

            int key = 0;
            foreach (var m in dicComparePO3)
            {
                var rowM = this.masterDt.NewRow();
                rowM["Type"] = m.Key.CompareState;
                rowM["RefNo"] = m.Key.RefNo;
                rowM["StockNetQty"] = m.Key.StockNetQty.GetValueOrDefault(0);
                rowM["StockLossQty"] = m.Key.StockLossQty.GetValueOrDefault(0);
                rowM["StockUnitQty"] = m.Key.StockUnitQty.GetValueOrDefault(0);
                rowM["StockBalanceQty"] = m.Key.StockBalanceQty.GetValueOrDefault(0);
                rowM["SpecColor"] = m.Key.SpecColor;
                rowM["SpecSize"] = m.Key.SpecSize;
                rowM["SpecSizeUnit"] = m.Key.SpecSizeUnit;
                rowM["SpecZipperInsert"] = m.Key.SpecZipperInsert;
                rowM["SpecArticle"] = m.Key.SpecArticle;
                rowM["SpecCOO"] = m.Key.SpecCOO;
                rowM["SpecGender"] = m.Key.SpecGender;
                rowM["SpecCustomerSize"] = m.Key.SpecCustomerSize;
                rowM["SpecDecLabelSize"] = m.Key.SpecDecLabelSize;
                rowM["SpecBrandFactoryCode"] = m.Key.SpecBrandFactoryCode;
                rowM["SpecStyle"] = m.Key.SpecStyle;
                rowM["SpecStyleLocation"] = m.Key.SpecStyleLocation;
                rowM["SpecSeason"] = m.Key.SpecSeason;
                rowM["SpecCareCode"] = m.Key.SpecCareCode;
                rowM["SpecCustomerPO"] = m.Key.SpecCustomerPO;
                rowM["MappingKey"] = key;
                if (m.Key.CompareState == ComparePO3StateEnu.Delete)
                {
                    rowM["StockUnit"] = string.Empty;
                }
                else if (m.Value == null)
                {
                    rowM["StockUnit"] = m.Key.StockUnit;
                }
                else
                {
                    rowM["StockUnit"] = m.Value[0].StockUnit;
                }

                this.masterDt.Rows.Add(rowM);

                if (m.Value != null)
                {
                    foreach (var d in m.Value)
                    {
                        var rowD = this.detailDt.NewRow();
                        rowD["Seq1"] = d.Seq1;
                        rowD["Seq2"] = d.Seq2;
                        rowD["RefNo"] = d.RefNo;
                        rowD["SuppID"] = d.SuppID;
                        rowD["ShowCfmETD"] = d.ShowCfmETD;
                        rowD["ShowRevisedETD"] = d.ShowRevisedETD;
                        rowD["FirstETA"] = d.FirstETA;
                        rowD["StockUnit"] = d.StockUnit;
                        rowD["StockNetQty"] = d.StockNetQty.GetValueOrDefault(0);
                        rowD["StockLossQty"] = d.StockLossQty.GetValueOrDefault(0);
                        rowD["StockUnitQty"] = d.StockUnitQty.GetValueOrDefault(0);
                        rowD["StockFOC"] = d.StockFOC.GetValueOrDefault(0);
                        rowD["OrderIdList"] = d.OrderIdList;
                        rowD["MappingKey"] = key;
                        this.detailDt.Rows.Add(rowD);
                    }
                }

                key++;
            }

            this.listControlBindingSource1.DataSource = this.masterDt;
            this.listControlBindingSource2.DataSource = this.detailDt;
            this.grid1.DataSource = this.listControlBindingSource1;
            this.grid2.DataSource = this.listControlBindingSource2;

            if (this.detailDt != null && this.detailDt.Rows.Count > 0)
            {
                this.listControlBindingSource1.Position = this.masterDt.Rows.Count - 1;
                this.ListControlBindingSource1_PositionChanged(null, null);
            }
        }

        private void SetTable()
        {
            this.masterDt = new DataTable();
            this.masterDt.ColumnsStringAdd("Type");
            this.masterDt.ColumnsStringAdd("RefNo");
            this.masterDt.ColumnsStringAdd("StockUnit");
            this.masterDt.ColumnsDecimalAdd("StockNetQty");
            this.masterDt.ColumnsDecimalAdd("StockLossQty");
            this.masterDt.ColumnsDecimalAdd("StockUnitQty");
            this.masterDt.ColumnsDecimalAdd("StockBalanceQty");
            this.masterDt.ColumnsStringAdd("SpecColor");
            this.masterDt.ColumnsStringAdd("SpecSize");
            this.masterDt.ColumnsStringAdd("SpecSizeUnit");
            this.masterDt.ColumnsStringAdd("SpecZipperInsert");
            this.masterDt.ColumnsStringAdd("SpecArticle");
            this.masterDt.ColumnsStringAdd("SpecCOO");
            this.masterDt.ColumnsStringAdd("SpecGender");
            this.masterDt.ColumnsStringAdd("SpecCustomerSize");
            this.masterDt.ColumnsStringAdd("SpecDecLabelSize");
            this.masterDt.ColumnsStringAdd("SpecBrandFactoryCode");
            this.masterDt.ColumnsStringAdd("SpecStyle");
            this.masterDt.ColumnsStringAdd("SpecStyleLocation");
            this.masterDt.ColumnsStringAdd("SpecSeason");
            this.masterDt.ColumnsStringAdd("SpecCareCode");
            this.masterDt.ColumnsStringAdd("SpecCustomerPO");
            this.masterDt.ColumnsIntAdd("MappingKey");

            this.detailDt = new DataTable();
            this.detailDt.ColumnsStringAdd("Seq1");
            this.detailDt.ColumnsStringAdd("Seq2");
            this.detailDt.ColumnsStringAdd("RefNo");
            this.detailDt.ColumnsStringAdd("SuppID");
            this.detailDt.ColumnsStringAdd("ShowCfmETD");
            this.detailDt.ColumnsStringAdd("ShowRevisedETD");
            this.detailDt.ColumnsStringAdd("FirstETA");
            this.detailDt.ColumnsStringAdd("StockUnit");
            this.detailDt.ColumnsDecimalAdd("StockNetQty");
            this.detailDt.ColumnsDecimalAdd("StockLossQty");
            this.detailDt.ColumnsDecimalAdd("StockUnitQty");
            this.detailDt.ColumnsDecimalAdd("StockFOC");
            this.detailDt.ColumnsStringAdd("OrderIdList");
            this.detailDt.ColumnsIntAdd("MappingKey");
        }

        private Dictionary<ComparePO3Item, List<ComparePO3Item>> LoadData(string poID)
        {
            DataTable tmpPo_Supp_Detail = new DataTable();

            SqlConnection sqlConn;
            DBProxy.Current.OpenConnection(null, out sqlConn);
            DBProxy.Current.DefaultTimeout = 900;
            DualResult result;
            List<SqlParameter> paras = new List<SqlParameter>();

            DataTable tmpOrders;
            string sqlCmd = string.Empty;

            sqlCmd = "Select * From dbo.Orders Where ID = @PoID";
            paras.Add(new SqlParameter("@PoID", poID));

            result = DBProxy.Current.Select(null, sqlCmd, paras, out tmpOrders);
            if (!result)
            {
                MyUtility.Msg.ErrorBox(result.ToString());
                return null;
            }

            if (tmpOrders.Rows.Count == 0)
            {
                return null;
            }

            string brandID = tmpOrders.Rows[0]["BrandID"].ToString();
            string programID = tmpOrders.Rows[0]["ProgramID"].ToString();
            string category = tmpOrders.Rows[0]["Category"].ToString();
            bool expendArticle = Prgs.IsExpendArticle(poID);
            int testType = 3;

            result = Prgs.CreateTmpPOTable(sqlConn);
            if (!result)
            {
                MyUtility.Msg.ErrorBox(result.ToString());
                return null;
            }

            // BOF
            result = Prgs.TransferToPO_1_ForBOF(sqlConn, poID, brandID, programID, category, testType, expendArticle);
            if (!result)
            {
                MyUtility.Msg.ErrorBox(result.ToString());
                return null;
            }

            // BOA
            result = Prgs.TransferToPO_1_ForBOA(sqlConn, poID, brandID, programID, category, testType, expendArticle);
            if (!result)
            {
                MyUtility.Msg.ErrorBox(result.ToString());
                return null;
            }

            // A Item
            result = Prgs.TransferToPO_1_ForAItem(sqlConn, poID, brandID, programID, category, testType);
            if (!result)
            {
                MyUtility.Msg.ErrorBox(result.ToString());
                return null;
            }

            result = Prgs.TransferToPO_1_ForThreadAllowance(sqlConn, poID);
            if (!result)
            {
                MyUtility.Msg.ErrorBox(result.ToString());
                return null;
            }

            // temp Table填資料
            result = Prgs.TransferToPO_2(sqlConn, poID, false, testType);
            if (!result)
            {
                MyUtility.Msg.ErrorBox(result.ToString());
                return null;
            }

            sqlCmd = @"
Select ID = #tmpPO_Supp_Detail.ID
    , #tmpPO_Supp_Detail.Seq1
    , #tmpPO_Supp_Detail.Seq2
    , #tmpPO_Supp_Detail.RefNo
    , #tmpPO_Supp_Detail.SCIRefNo
    , #tmpPO_Supp.SuppID
    , tmpOrderList.OrderIdList
    , Po_Supp_Detail.StockUnit
    , StockNetQty = dbo.GetUnitQty(#tmpPO_Supp_Detail.POUnit, Po_Supp_Detail.StockUnit, #tmpPO_Supp_Detail.NetQty)
    , StockLossQty = dbo.GetUnitQty(#tmpPO_Supp_Detail.POUnit, Po_Supp_Detail.StockUnit, #tmpPO_Supp_Detail.LossQty)
    , StockUnitQty = dbo.GetUnitQty(#tmpPO_Supp_Detail.POUnit, Po_Supp_Detail.StockUnit, #tmpPO_Supp_Detail.Qty)
    , StockFOC = dbo.GetUnitQty(#tmpPO_Supp_Detail.POUnit, Po_Supp_Detail.StockUnit, #tmpPO_Supp_Detail.FOC)
    , BomSpec.*
From #tmpPO_Supp_Detail
Left Join (Select ID, Seq1, Seq2, OrderIdList = (Select SubString(OrderID,9,5)+'/' From #tmpPO_Supp_Detail_OrderList as tmp Where tmp.ID = #tmpPO_Supp_Detail_OrderList.ID And tmp.Seq1 = #tmpPO_Supp_Detail_OrderList.Seq1 And tmp.Seq2 = #tmpPO_Supp_Detail_OrderList.Seq2 for XML path('')) 
            From #tmpPO_Supp_Detail_OrderList
            Group by ID, Seq1, Seq2
            ) as tmpOrderList
    On     #tmpPO_Supp_Detail.ID = tmpOrderList.ID
    And #tmpPO_Supp_Detail.Seq1 = tmpOrderList.Seq1
    And #tmpPO_Supp_Detail.Seq2 = tmpOrderList.Seq2
Left join Fabric on Fabric.SciRefno = #tmpPO_Supp_Detail.SciRefno
Left join #tmpPO_Supp on #tmpPO_Supp_Detail.id = #tmpPO_Supp.id and #tmpPO_Supp_Detail.Seq1 = #tmpPO_Supp.Seq1
left join Po_Supp_Detail on Po_Supp_Detail.id = #tmpPO_Supp_Detail.id and Po_Supp_Detail.SEQ1 = #tmpPO_Supp_Detail.SEQ1 and Po_Supp_Detail.SEQ2 = #tmpPO_Supp_Detail.SEQ2
--Outer Apply (Select * From dbo.GetStatusByPO(#tmpPO_Supp_Detail.ID, #tmpPO_Supp_Detail.Seq1, '')) as PoStatus
outer apply (
	select SpecColor = p.Color
	, SpecSize = p.Size
	, SpecSizeUnit = p.SizeUnit
	, SpecZipperInsert = p.ZipperInsert
	, SpecArticle = p.Article
	, SpecCOO = p.COO
	, SpecGender = p.Gender
	, SpecCustomerSize = p.CustomerSize
	, SpecDecLabelSize = p.DecLabelSize
	, SpecBrandFactoryCode = p.BrandFactoryCode
	, SpecStyle = p.Style
	, SpecStyleLocation = p.StyleLocation
	, SpecSeason = p.Season
	, SpecCareCode = p.CareCode
	, SpecCustomerPO = p.CustomerPO
	from 
	(
		select BomTypeID = BomType.ID, SpecValue = isnull(po3s.SpecValue, '')
		from Production.dbo.BomType with (nolock)
		left join #tmpPO_Supp_Detail_Spec po3s with (nolock) on po3s.ID = #tmpPO_Supp_Detail.ID and po3s.Seq1 = #tmpPO_Supp_Detail.Seq1 and po3s.Seq2 = #tmpPO_Supp_Detail.Seq2 and BomType.ID = po3s.SpecColumnID
	)tmp
	pivot
	(
		MAX(SpecValue) for BomTypeID in (Color, Size, SizeUnit, ZipperInsert, Article, COO, Gender, CustomerSize, DecLabelSize, BrandFactoryCode, Style, StyleLocation, Season, CareCode, CustomerPO)
	) as p
) BomSpec
where #tmpPO_Supp_Detail.Seq1 not like 'A%'
Order by #tmpPO_Supp_Detail.ID, #tmpPO_Supp_Detail.Seq1, #tmpPO_Supp_Detail.Seq2
";
            result = DBProxy.Current.SelectByConn(sqlConn, sqlCmd, out tmpPo_Supp_Detail);
            if (!result)
            {
                MyUtility.Msg.ErrorBox(result.ToString());
                return null;
            }

            List<ComparePO3Item> newPO3 = new List<ComparePO3Item>();
            List<ComparePO3Item> oldPO3 = new List<ComparePO3Item>();

            tmpPo_Supp_Detail.AsEnumerable().ToList().ForEach(row => newPO3.Add(new ComparePO3Item(row, ComparePO3VersionEnu.New)));

            string oldCmd = @"
select ID = po3.ID
    , po3.Seq1
    , po3.Seq2
    , po3.RefNo
    , po3.SCIRefNo
    , po2.SuppID
    , viewPo3.ShowCfmETD, viewPo3.ShowRevisedETD, viewPo3.FirstETA
    , po3.StockUnit
    , StockNetQty = dbo.GetUnitQty(po3.POUnit, po3.StockUnit, NetQty)
    , StockLossQty = dbo.GetUnitQty(po3.POUnit, po3.StockUnit, LossQty)
    , StockUnitQty = dbo.GetUnitQty(po3.POUnit, po3.StockUnit, Qty)
    , StockFOC = dbo.GetUnitQty(po3.POUnit, po3.StockUnit, FOC)
    , BomSpec.*
from Production.dbo.PO_Supp_Detail po3 
outer apply (select seq1New = iif(po3.Seq1 like '7%', po3.OutputSeq1, po3.Seq1)
					, seq2New = iif(po3.Seq1 like '7%', po3.OutputSeq2, po3.Seq2)) newItem
inner join Production.dbo.PO_Supp po2 on po3.ID = po2.ID and newItem.seq1New = po2.SEQ1
left join View_Po3WithStock viewPo3 on po3.id = viewPo3.ID and po3.Seq1 = viewPo3.Seq1 and po3.Seq2 = viewPo3.Seq2
outer apply (
	select SpecColor = p.Color
	, SpecSize = p.Size
	, SpecSizeUnit = p.SizeUnit
	, SpecZipperInsert = p.ZipperInsert
	, SpecArticle = p.Article
	, SpecCOO = p.COO
	, SpecGender = p.Gender
	, SpecCustomerSize = p.CustomerSize
	, SpecDecLabelSize = p.DecLabelSize
	, SpecBrandFactoryCode = p.BrandFactoryCode
	, SpecStyle = p.Style
	, SpecStyleLocation = p.StyleLocation
	, SpecSeason = p.Season
	, SpecCareCode = p.CareCode
	, SpecCustomerPO = p.CustomerPO
	from 
	(
		select BomTypeID = BomType.ID, SpecValue = isnull(po3s.SpecValue, '')
		from Production.dbo.BomType with (nolock)
		left join Production.dbo.PO_Supp_Detail_Spec po3s with (nolock) on po3s.ID = po3.ID and po3s.Seq1 = po3.Seq1 and po3s.Seq2 = po3.Seq2 and BomType.ID = po3s.SpecColumnID
	)tmp
	pivot
	(
		MAX(SpecValue) for BomTypeID in (Color, Size, SizeUnit, ZipperInsert, Article, COO, Gender, CustomerSize, DecLabelSize, BrandFactoryCode, Style, StyleLocation, Season, CareCode, CustomerPO)
	) as p
) BomSpec
where po3.id = @poid
and po3.Seq1 not like 'A%'

drop table #tmpPO_Supp,#tmpPO_Supp_Detail,#tmpPO_Supp_Detail_Keyword,#tmpPO_Supp_Detail_OrderList,#tmpPO_Supp_Detail_Spec
";

            DualResult dualResult = DBProxy.Current.Select(null, oldCmd, new List<SqlParameter>() { new SqlParameter("@poID", poID) }, out DataTable oldPO3Tbl);
            if (!dualResult)
            {
                this.ShowErr(dualResult);
                return null;
            }

            oldPO3Tbl.AsEnumerable().ToList().ForEach(row => oldPO3.Add(new ComparePO3Item(row, ComparePO3VersionEnu.Old)));

            return this.DoMixPO3(newPO3, oldPO3);
        }

        private Dictionary<ComparePO3Item, List<ComparePO3Item>> DoMixPO3(List<ComparePO3Item> newPO3, List<ComparePO3Item> oldPO3)
        {
            Dictionary<ComparePO3Item, List<ComparePO3Item>> dic = new Dictionary<ComparePO3Item, List<ComparePO3Item>>();

            foreach (ComparePO3Item n in newPO3)
            {
                var olds = oldPO3
                    .Where(o => Prg.ProjExts.PropertiesEqual<ComparePO3GroupItem>(n.GroupKey, o.GroupKey, null))
                    .ToList();

                if (olds.Count() > 0)
                {
                    // n.BalanceQty = n.Qty - olds.Sum(oldItem => oldItem.Qty);
                    n.StockBalanceQty = n.StockUnitQty - olds.Sum(oldItem => oldItem.StockUnitQty);

                    // 新規格在舊項目有資料
                    foreach (var ii in olds)
                    {
                        n.CompareState = ComparePO3StateEnu.Same;

                        if (dic.ContainsKey(n) == true)
                        {
                            dic[n].Add(ii);
                        }
                        else
                        {
                            dic.Add(n, new List<ComparePO3Item>() { ii });
                        }
                    }
                }
                else
                {
                    // 新規格在舊項目沒有資料
                    n.CompareState = ComparePO3StateEnu.New;
                    dic.Add(n, null);
                }
            }

            ComparePO3Item deleteItem = new ComparePO3Item(ComparePO3VersionEnu.Old);
            deleteItem.CompareState = ComparePO3StateEnu.Delete;

            foreach (ComparePO3Item o in oldPO3)
            {
                var news = newPO3
                    .Where(n => Prg.ProjExts.PropertiesEqual<ComparePO3GroupItem>(n.GroupKey, o.GroupKey, null))
                    .ToList();

                if (news.Count() <= 0)
                {
                    if (dic.ContainsKey(deleteItem) == true)
                    {
                        dic[deleteItem].Add(o);
                    }
                    else
                    {
                        // 舊規格在新項目沒有資料
                        o.CompareState = ComparePO3StateEnu.Delete;
                        dic.Add(deleteItem, new List<ComparePO3Item>() { o });
                    }
                }
            }

            return dic.OrderBy(rr => rr.Key.SCIRefNo).ToDictionary(k => k.Key, v => v.Value);
        }

        private void ListControlBindingSource1_PositionChanged(object sender, EventArgs e)
        {
            DataRow dr = this.grid1.GetDataRow(this.listControlBindingSource1.Position);
            if (dr == null)
            {
                return;
            }

            this.listControlBindingSource2.DataSource = this.detailDt.AsEnumerable()
                                                            .Where(x => x["MappingKey"].ToString() == dr["MappingKey"].ToString())
                                                            .OrderBy(x => x["Seq1"])
                                                            .TryCopyToDataTable(this.detailDt);
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
