using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Sci.Production.Cutting
{
    public partial class P02_AutoDistToSP : Sci.Win.Tems.QueryForm
    {
        DataRow Detailrow;
        DataTable SizeratioTb;
        DataTable DistqtyTb;
        DataTable PatternPanel_Current;
        DataTable PatternPanel_notCurrent;
        DataTable SourceDt;

        public P02_AutoDistToSP(DataRow detailrow, DataTable sizeratioTb, DataTable distqtyTb, DataTable patternPanel_Current, DataTable patternPanel_notCurrent)
        {
            InitializeComponent();

            this.EditMode = true;
            Detailrow = detailrow;
            SizeratioTb = sizeratioTb;
            DistqtyTb = distqtyTb;
            PatternPanel_Current = patternPanel_Current;
            PatternPanel_notCurrent = patternPanel_notCurrent;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.numDistQty.Value = MyUtility.Convert.GetInt(Detailrow["layer"]) * MyUtility.Convert.GetInt(SizeratioTb.Compute("Sum(Qty)", $@"Workorderukey = '{Detailrow["ukey"]}' and newkey = {Detailrow["newkey"]}"));
            this.GridSetup();
            this.Query();
        }

        private void Query()
        {
            string sizes = SizeratioTb.Select($@"Workorderukey = '{Detailrow["ukey"]}' and newkey = {Detailrow["newkey"]}")
                .AsEnumerable().Select(s => MyUtility.Convert.GetString(s["SizeCode"])).ToList().JoinToString("','");

            string sqlcmd = $@"
SELECT
	Sel = CAST(0 AS bit),
	Seq = '',
	OrderId =	oq.id, 
	Article = OQ.article,
	SizeCode = OQ.sizecode, 
	OrderQty = OQ.qty,
	AccuDistQty = 0,
	BalQty = 0,
	DENSE_RANK = DENSE_RANK() over(order by OQ.sizecode),
    
    -- 分配時才寫入欄位
    Qty = 0,
    WorkOrderUkey = cast({Detailrow["Ukey"]} as bigint),
    NewKey = cast({Detailrow["NewKey"]} as bigint),
    ID = '{Detailrow["ID"]}'
FROM order_qty oq 
INNER JOIN orders o ON o.id = oq.id 
WHERE o.poid = '{Detailrow["ID"]}' and oq.SizeCode in ('{sizes}')
ORDER BY OQ.sizecode,oq.id,OQ.article 
";
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out SourceDt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.listControlBindingSource1.DataSource = SourceDt;

            this.CalColumnsQty();
        }

        private void GridSetup()
        {
            #region 欄位事件
            DataGridViewGeneratorCheckBoxColumnSettings sel = new DataGridViewGeneratorCheckBoxColumnSettings();
            DataGridViewGeneratorNumericColumnSettings balQty = new DataGridViewGeneratorNumericColumnSettings();
            sel.CellValidating += (s, e) =>
            {
                DataRow dr = grid1.GetDataRow(e.RowIndex);
                dr["Sel"] = e.FormattedValue;
                dr.EndEdit();
                ReWriteSeq();
                this.CalTtlBal();
            };

            balQty.CellValidating += (s, e) =>
            {
                DataRow dr = grid1.GetDataRow(e.RowIndex);
                dr["BalQty"] = e.FormattedValue;
                dr.EndEdit();
                this.CalTtlBal();
            };
            #endregion

            this.grid1.IsEditingReadOnly = false;
            Helper.Controls.Grid.Generator(this.grid1)
            .CheckBox("Sel", header: "", width: Widths.AnsiChars(2), iseditable: true, trueValue: 1, falseValue: 0, settings: sel)
            .Numeric("Seq", header: "Dist. Seq", width: Widths.AnsiChars(3), iseditingreadonly: true)
            .Text("OrderId", header: "SP#", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("Article", header: "Article", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("SizeCode", header: "Size", width: Widths.AnsiChars(4), iseditingreadonly: true)
            .Numeric("OrderQty", header: "Order Qty", width: Widths.AnsiChars(3), iseditingreadonly: true)
            .Numeric("AccuDistQty", header: "Accu. Dist. Qty", width: Widths.AnsiChars(3), iseditingreadonly: true)
            .Numeric("BalQty", header: "Bal. Qty", width: Widths.AnsiChars(3), integer_places: 6, maximum: 999999, minimum: 0, settings: balQty)
            ;

            #region 關閉排序功能
            for (int i = 0; i < this.grid1.ColumnCount; i++)
            {
                this.grid1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            #endregion

            grid1.Columns["Sel"].DefaultCellStyle.BackColor = Color.Pink;
            grid1.Columns["BalQty"].DefaultCellStyle.BackColor = Color.Pink;
        }

        private void ReWriteSeq()
        {
            // 清空沒勾選
            foreach (DataRow dr in SourceDt.Select("Sel = 0"))
            {
                dr["Seq"] = string.Empty;
            }

            // 按原本Seq順序重編已有Seq
            foreach (string size in SourceDt.Select("Sel = 1 and Seq <> ''").AsEnumerable()
                .Select(s => MyUtility.Convert.GetString(s["SizeCode"])).Distinct().ToList())
            {
                int seq = 1;
                foreach (DataRow dr in SourceDt.Select($"Sel = 1 and Seq <> '' and SizeCode = '{size}'").AsEnumerable()
                    .OrderBy(o => MyUtility.Convert.GetInt(o["Seq"])))
                {
                    dr["Seq"] = seq;
                    seq++;
                }
            }

            DataRow[] drs;
            // 寫入勾選且Seq空白
            foreach (DataRow dr in SourceDt.Select("Sel = 1 and Seq = ''"))
            {
                int sizeMaxSeq = 0;
                drs = SourceDt.Select($"Sel = 1 and Seq <> '' and SizeCode = '{dr["SizeCode"]}'");
                if (drs.Count() > 0)
                {
                    sizeMaxSeq = drs.AsEnumerable().Select(s => MyUtility.Convert.GetInt(s["Seq"])).Max();
                }

                sizeMaxSeq++;
                dr["Seq"] = sizeMaxSeq;
                dr.EndEdit();
            }
        }

        private void CalColumnsQty()
        {
            // Accu. Dist. Qty 其它WorkOrder (組合相同[WorkOrder_PatternPanel]),[Size Ratio],[SP#],[Article],[Color] 發的件數總和

            // 這次 WorkOrder 底下的 PatternPanel
            var patternPanelList = PatternPanel_Current.AsEnumerable().Select(s => MyUtility.Convert.GetString(s["PatternPanel"])).ToList();

            // 非此 WorkOrder 底下的 PatternPanel
            var notThisPP = PatternPanel_notCurrent
                .AsEnumerable().Select(s => new
                {
                    Workorderukey = MyUtility.Convert.GetLong(s["Workorderukey"]),
                    newkey = MyUtility.Convert.GetLong(s["newkey"]),
                    PatternPanel = MyUtility.Convert.GetString(s["PatternPanel"]),
                }).ToList();

            string color = MyUtility.Convert.GetString(this.Detailrow["ColorID"]);
            var groupWorkorder = notThisPP.Select(s => new { s.Workorderukey, s.newkey }).Distinct().ToList();
            var recordWorkorder = notThisPP.Select(s => new { s.Workorderukey, s.newkey }).Where(w => 1 == 0).ToList();

            foreach (var item in groupWorkorder)
            {
                var x = notThisPP.Where(w => w.Workorderukey == item.Workorderukey && w.newkey == item.newkey)
                    .Select(s => s.PatternPanel).ToList();

                // PatternPanel 組合相同 且 WorkOrder的顏色相同, 紀錄這組 Workorderukey, newkey
                if (x.All(patternPanelList.Contains) && x.Count == patternPanelList.Count &&
                    this.Detailrow.Table.Select($@"ColorID = '{color}' and Ukey = '{item.Workorderukey}' and newkey = '{item.newkey}'").Count() > 0)
                {
                    recordWorkorder.Add(item);
                }
            }

            foreach (DataRow dr in SourceDt.Rows)
            {
                // 找相同 OrderID, Article, SizeCode
                var asList = DistqtyTb
                    .Select($@"OrderID ='{dr["OrderID"]}' and Article = '{dr["Article"]}' and SizeCode = '{dr["SizeCode"]}'")
                    .AsEnumerable()
                    .Select(s => new
                    {
                        Workorderukey = MyUtility.Convert.GetLong(s["Workorderukey"]),
                        newkey = MyUtility.Convert.GetLong(s["newkey"]),
                        Qty = MyUtility.Convert.GetInt(s["Qty"]),
                    })
                    .ToList();

                // 兩個清單 join 為其它筆 WorkOrder 的 (PatternPanel 組合), ColorID, OrderID, Article, SizeCode 與這筆相同, 把這些 Distribute.Qty 總和
                dr["AccuDistQty"] = (from t1 in asList
                                     join t2 in recordWorkorder
                                     on new { t1.Workorderukey, t1.newkey }
                                     equals new { t2.Workorderukey, t2.newkey }
                                     select t1.Qty).Sum();
                int bal = MyUtility.Convert.GetInt(dr["OrderQty"]) - MyUtility.Convert.GetInt(dr["AccuDistQty"]);
                dr["BalQty"] = bal < 0 ? 0 : bal;
            }

            this.CalTtlBal();
        }

        private void CalTtlBal()
        {
            if (SourceDt == null || SourceDt.Rows.Count == 0) return;

            this.numBalQty.Value = this.numDistQty.Value - MyUtility.Convert.GetInt(SourceDt.Compute("sum(balQty)", "Sel = 1"));
        }

        private void BtnDist_Click(object sender, EventArgs e)
        {
            this.grid1.ValidateControl();
            DataRow[] drs = SourceDt.Select("Sel = 1");
            if (drs.Count() == 0)
            {
                MyUtility.Msg.WarningBox("Please selected datas first!");
                return;
            }

            DataTable processDT = drs.CopyToDataTable();

            // 準備每個 SizeCode 能分配總數的清單
            var sList = processDT.AsEnumerable().Select(s => MyUtility.Convert.GetString(s["SizeCode"])).ToList();
            var sizeTtlQty = SizeratioTb.Select($"WorkOrderUkey = '{Detailrow["Ukey"]}' and newkey = '{Detailrow["NewKey"]}'").AsEnumerable()
                .Select(s => new
                {
                    SizeCode = MyUtility.Convert.GetString(s["SizeCode"]),
                    Qty = MyUtility.Convert.GetInt(Detailrow["Layer"]) * MyUtility.Convert.GetInt(s["Qty"])
                }).ToList();

            #region 分配每個 SizeCoode 
            foreach (var item in sizeTtlQty)
            {
                int sizeTTlQty = item.Qty;
                // 依 Seq 順序分配
                var processSize = processDT.AsEnumerable().Where(w => MyUtility.Convert.GetString(w["SizeCode"]) == item.SizeCode).ToList();
                if (processSize.Count() == 0)
                {
                    string artile = SourceDt.AsEnumerable().Where(w => MyUtility.Convert.GetString(w["SizeCode"]) == item.SizeCode)
                        .Select(s => MyUtility.Convert.GetString(s["Article"])).FirstOrDefault();
                    AddEXCESS(processDT, artile, item.SizeCode, item.Qty);
                }

                foreach (DataRow bySeq in processSize.OrderBy(o => MyUtility.Convert.GetInt(o["Seq"])))
                {
                    if (sizeTTlQty <= MyUtility.Convert.GetInt(bySeq["BalQty"]))
                        bySeq["Qty"] = sizeTTlQty;
                    else
                        bySeq["Qty"] = bySeq["BalQty"];

                    sizeTTlQty -= MyUtility.Convert.GetInt(bySeq["BalQty"]);

                    if (sizeTTlQty < 0) break;

                    // 若還有剩下
                    if (MyUtility.Convert.GetInt(bySeq["Seq"]) == processSize.Max(s => MyUtility.Convert.GetInt(s["Seq"])) && sizeTTlQty > 0)
                    {
                        AddEXCESS(processDT, MyUtility.Convert.GetString(bySeq["Article"]), MyUtility.Convert.GetString(bySeq["SizeCode"]), sizeTTlQty);
                    }
                }
            }
            #endregion

            // 先清除此 WorkOrder 的 WorkOrder_Distribute
            P02.DeleteThirdDatas(this.DistqtyTb, MyUtility.Convert.GetLong(Detailrow["Ukey"]), MyUtility.Convert.GetLong(Detailrow["NewKey"]));

            foreach (DataRow row in processDT.Select($"Qty > 0"))
            {
                row.AcceptChanges();
                row.SetAdded();
                this.DistqtyTb.ImportRow(row);
            }

            this.Close();
        }

        private void AddEXCESS(DataTable processDT, string article, string sizeCode, int qty)
        {
            DataRow excessRow = processDT.NewRow();
            excessRow["WorkOrderUkey"] = Detailrow["Ukey"];
            excessRow["NewKey"] = Detailrow["NewKey"];
            excessRow["ID"] = Detailrow["ID"];
            excessRow["OrderID"] = "EXCESS";
            excessRow["Article"] = article;
            excessRow["SizeCode"] = sizeCode;
            excessRow["Qty"] = qty;
            processDT.Rows.Add(excessRow);
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Grid1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex > 0 && e.ColumnIndex < 7 && e.RowIndex > -1)
            {
                if (MyUtility.Convert.GetInt(this.SourceDt.Rows[e.RowIndex]["DENSE_RANK"]) % 2 == 0)
                {
                    e.CellStyle.BackColor = Color.FromArgb(128, 255, 255);
                }
            }
        }

        private void Grid1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                this.grid1.ValidateControl();
                this.ReWriteSeq();
                this.CalTtlBal();
            }
        }
    }
}
