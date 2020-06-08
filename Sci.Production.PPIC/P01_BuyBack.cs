using Ict;
using Ict.Win.UI;
using Sci.Data;
//using Sci.Trade.Class;
//using Sci.Trade.Class.Commons;
using Sci.Win.Tems;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DataGridViewComboBoxColumn = Ict.Win.UI.DataGridViewComboBoxColumn;
using DataGridViewTextBoxColumn = Ict.Win.UI.DataGridViewTextBoxColumn;
using Widths = Ict.Win.Widths;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// Order P01 BuyBack
    /// </summary>
    public partial class P01_BuyBack : Sci.Win.Subs.Base
    {
        /// <summary>
        /// is data change or not
        /// </summary>
        public bool IsDataChange { get; set; } = false;

        private const string StrBalance = "(B.)";
        private const string StrAssign = "(A.)";
        private const string StrOriginal = "(O.)";

        private bool canEdit = false;
        private string ID = string.Empty;

        private DataTable dtList;       // 存放SP清單 for Grid1
        private DataTable dtMain;       // 存放From SP資料 for Grid2
        private DataTable dtOriTable;   // 存放本張SP樞紐後的結果 for Grid3，因為可能Grid2還無分配資料，但還是要顯示Grid3
        private DataTable dtSizeCode;   // 存放本張SP的SizeCode

        private DataTable listFromSP;
        private DataTable listArticle;
        private DataTable listSizeGroup;

        private DataTable dtOrderQty;   // from P01_Qty

        ////private List<ItemChangeCls> changeList = new List<ItemChangeCls>();

        /// <inheritdoc/>
        public P01_BuyBack(bool canedit, string id, DataTable dtOrderQty = null)
        {
            this.InitializeComponent();
            this.canEdit = canedit;
            this.BtnEdit.Visible = canedit;
            this.ID = id;
            this.dtOrderQty = dtOrderQty;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.SetupGrid();

            //// 必須在Setup Grid之後
            this.EditMode = false;

            this.LoadList();
            this.LoadOriTableData();
            this.LoadData(false);

            this.SetCombo();
        }

        /// <summary>
        /// 1. Setup Grid
        /// 2. Create Grid2, Grid3 Column
        /// 3. Create dtMain, dtOriTable Column
        /// </summary>
        private void SetupGrid()
        {
            this.grid2.SelectionMode = DataGridViewSelectionMode.CellSelect;
            this.grid3.SelectionMode = DataGridViewSelectionMode.CellSelect;
            this.grid4.SelectionMode = DataGridViewSelectionMode.CellSelect;

            this.grid2.AutoGenerateColumns = true;
            this.grid3.AutoGenerateColumns = true;

            this.grid1.SupportEditMode = Win.UI.AdvEditModesReadOnly.True;
            this.grid2.SupportEditMode = Win.UI.AdvEditModesReadOnly.True;
            this.grid4.SupportEditMode = Win.UI.AdvEditModesReadOnly.True;
            this.grid1.IsEditingReadOnly = false;
            this.grid2.IsEditingReadOnly = false;
            ////this.grid3.IsEditingReadOnly = true;
            this.grid4.IsEditingReadOnly = false;

            var buyBackReasonCell = new Ict.Win.DataGridViewGeneratorComboBoxColumnSettings();
            {
                var sql = string.Format("select ID, Name from DropDownList where Type = 'BuyBack'");
                Ict.DualResult result;
                DataTable dropDownListTable = new DataTable();
                if (result = DBProxy.Current.Select(null, sql, out dropDownListTable))
                {
                    dropDownListTable.Rows.Add(dropDownListTable.NewRow());
                    buyBackReasonCell.DataSource = dropDownListTable;
                    buyBackReasonCell.DisplayMember = "Name";
                    buyBackReasonCell.ValueMember = "ID";
                }
            }

            Ict.Win.DataGridViewGeneratorTextColumnSettings popupSPCell = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
            popupSPCell.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    if (e.RowIndex == -1)
                    {
                        return;
                    }

                    DataRow dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                    List<string> articles = this.dtOriTable.AsEnumerable().Select(rr => rr["Article"].ToString()).ToList();
                    string articleStr = $"'{string.Join("','", articles)}'";

                    List<string> sizeCodes = this.dtSizeCode.AsEnumerable().Select(rr => rr["SizeCode"].ToString()).ToList();
                    string sizeCodeStr = $"'{string.Join("','", sizeCodes)}'";

                    //// 刪除已經選擇的項目
                    List<string> spList = this.dtList.AsEnumerable().Select(rr => rr["ID"].ToString()).ToList();

                    string cmd = $@"
select distinct o.ID, o.StyleID
from Orders o
inner join Brand on o.BrandID = Brand.ID
left join Order_Qty oq on o.ID = oq.ID
outer apply (select ID from Orders tmpO where o.BrandID = tmpO.BrandID and o.StyleID = tmpO.StyleID and tmpO.ID = '{this.ID}') sty
outer apply (select ID from Orders tmpO where o.BrandID = tmpO.BrandID and Brand.IsBuyBackNoLimitStyle = 1 and tmpO.ID = '{this.ID}') noLimitSty
where o.ID != '{this.ID}'
and (isnull(sty.ID, '') != '' or isnull(noLimitSty.ID, '') != '')
and o.Junk = 1
and o.NeedProduction = 1
and oq.Article in ({articleStr})
and oq.SizeCode in ({sizeCodeStr})";

                    DataTable data;
                    DBProxy.Current.Select(null,cmd,out data)
                    ;
                    //data.AsEnumerable().Where(rr => spList.Contains(rr["ID"].ToString())
                    //    && rr["ID"].ToString() != dr["ID"].ToString()).Delete();
                    foreach (var row in data.AsEnumerable().Where(rr => spList.Contains(rr["ID"].ToString())
                        && rr["ID"].ToString() != dr["ID"].ToString()))
                    {
                        row.Delete();
                    }

                    data.AcceptChanges();

                    //// 剩餘項目跳窗選擇
                    Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(data, "ID,StyleID", "13,10", dr["ID"].ToString(), filterColumns: "StyleID");
                    DialogResult returnResult = item.ShowDialog();
                    if (returnResult == DialogResult.Cancel)
                    {
                        return;
                    }

                    string id = item.GetSelectedString();
                    dr["ID"] = id;

                    this.LoadData(true, new List<string> { id });
                }
            };

            popupSPCell.CellValidating += (s, e) =>
            {
                if (this.EditMode && !MyUtility.Check.Empty(e.FormattedValue.ToString()))
                {
                    List<string> articles = this.dtOriTable.AsEnumerable().Select(rr => rr["Article"].ToString()).ToList();
                    string articleStr = $"'{string.Join("','", articles)}'";

                    string cmd = $@"
                        select o.ID from Orders o
                        left join Order_Qty oq on o.ID = oq.ID
                        outer apply (select ID from Orders tmpO where o.BrandID = tmpO.BrandID and o.StyleID = tmpO.StyleID and tmpO.ID = '{this.ID}') sty
                        where ID = '{e.FormattedValue.ToString()}' and isnull(sty.ID, '') != '' and Junk = 1
                        and oq.Article in ({articleStr})";

                    if (!MyUtility.Check.Seek(cmd, null))
                    {
                        DataRow dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                        MyUtility.Msg.ErrorBox(string.Format("< ID: {0} > not found!!!", e.FormattedValue.ToString()));
                        dr["ID"] = string.Empty;
                        return;
                    }
                }
            };

            this.Helper.Controls.Grid.Generator(this.grid1)
            .Text("ID", header: "SP#", width: Widths.AnsiChars(12), iseditingreadonly: true, settings: popupSPCell)
            .ComboBox("BuyBackReason", header: "Buy Back Reason", width: Widths.AnsiChars(13), settings: buyBackReasonCell);

            this.Helper.Controls.Grid.Generator(this.grid4)
            .Text("FromSP", header: "From SP#", width: Widths.AnsiChars(12), iseditingreadonly: true)
            .Numeric("BalanceQty", header: "Qty" + StrBalance, width: Widths.AnsiChars(5), decimal_places: 0, minimum: 0, maximum: 99999999, iseditingreadonly: false)
            .Numeric("AssignQty", header: "Qty", width: Widths.AnsiChars(5), decimal_places: 0, minimum: 0, maximum: 99999999, iseditingreadonly: false);

            this.Helper.Controls.Grid.Generator(this.grid2)
                .Text("FromSP", header: "From SP#", width: Widths.AnsiChars(12), iseditingreadonly: true)
                .Text("Article", header: "Article", width: Widths.AnsiChars(8), iseditingreadonly: true);

            this.Helper.Controls.Grid.Generator(this.grid3)
                .Text("Article", header: "Article", width: Widths.AnsiChars(8), iseditingreadonly: true);

            this.grid2.Columns[1].Frozen = true;
            this.grid3.Columns[0].Frozen = true;

            //// Load SizeCode
            this.dtSizeCode = this.GetSizeCode(this.ID);

            //// 建立Grid2、Grid3欄位
            foreach (DataRow row in this.dtSizeCode.Rows)
            {
                string sizecode = row["SizeCode"].ToString();
                this.Helper.Controls.Grid.Generator(this.grid2)
                    .Numeric(sizecode + StrBalance, header: sizecode + StrBalance, width: Widths.AnsiChars(5), iseditingreadonly: true, maximum: 999999, minimum: 0, decimal_places: 0)
                    .Numeric(sizecode + StrAssign, header: sizecode + StrAssign, width: Widths.AnsiChars(5), iseditingreadonly: true, maximum: 999999, minimum: 0, decimal_places: 0);

                this.Helper.Controls.Grid.Generator(this.grid3)
                    .Numeric(sizecode + StrOriginal, header: sizecode + StrOriginal, width: Widths.AnsiChars(5), iseditingreadonly: true, maximum: 999999, minimum: 0, decimal_places: 0)
                    .Numeric(sizecode + StrAssign, header: sizecode + StrAssign, width: Widths.AnsiChars(5), iseditingreadonly: true, maximum: 999999, minimum: 0, decimal_places: 0);
            }

            // 建立dtMain、dtOriTable
            this.dtMain = new DataTable();
            this.dtMain.Columns.Add("FromSP", typeof(string));
            this.dtMain.Columns.Add("Article", typeof(string));

            this.dtOriTable = new DataTable();
            this.dtOriTable.Columns.Add("Article", typeof(string));

            foreach (DataRow row in this.dtSizeCode.Rows)
            {
                string sizecode = row["SizeCode"].ToString();
                this.dtMain.Columns.Add(sizecode + StrBalance, typeof(int));
                this.dtMain.Columns.Add(sizecode + StrAssign, typeof(int));
                this.dtOriTable.Columns.Add(sizecode + StrOriginal, typeof(int));
                this.dtOriTable.Columns.Add(sizecode + StrAssign, typeof(int));
            }

            //// DataSource給值之後會讓Grid的所有欄位IsEditingReadOnly變為False
            this.grid2bs.DataSource = this.dtMain;
            this.SetGridIsEditingReadOnly(this.grid2, StrAssign);
        }

        private DataTable GetSizeCode(string id)
        {
            DataTable dt;
            var res = DBProxy.Current.Select(null, $"select SizeGroup, SizeCode from Order_SizeCode where id = (select PoID from Orders where id = '{id}') order by Seq", out dt);
            if (res == true)
            {
                return dt;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 載入Grid1的資料
        /// </summary>
        /// <returns> Excute success or not </returns>
        private DualResult LoadList()
        {
            this.dtList = new DataTable();
            DataTable tmp;
            string cmd = $"select OrderIDFrom as ID, BuyBackReason from Order_BuyBack where id = '{this.ID}'";
            DualResult res = DBProxy.Current.Select(null,cmd, out tmp);
            if (res == true)
            {
                this.dtList = tmp;
            }
            else
            {
                this.ShowErr(res);
            }

            this.grid1bs.DataSource = this.dtList;
            this.SetGridIsEditingReadOnly(this.grid1, string.Empty);

            return res;
        }

        /// <summary>
        /// 1. 載入dtOriTable
        /// 2. 給Grid3值
        /// </summary>
        /// <returns> Execute success or not </returns>
        private DualResult LoadOriTableData()
        {
            //// 載入dtOriTable***************************************
            string cmd = $@"
select 
    oa.Article, os.SizeGroup, os.SizeCode, isnull(oq.Qty, 0) Qty
from Orders o
left join Order_Article oa on o.ID = oa.id
left join Order_SizeCode os on o.POID = os.Id
left join Order_Qty oq on o.ID = oq.ID and oa.Article = oq.Article and os.SizeCode = oq.SizeCode
where o.id = '{this.ID}'
order by os.Seq, os.SizeCode, oa.Article";

            DataTable dt;
            DualResult res = DBProxy.Current.Select(null,cmd, out dt);

            if (res == false)
            {
                this.ShowErr(res);
                return res;
            }

            DataTable dtOriData;

            // 由P01_Qty傳入
            if (this.dtOrderQty != null)
            {
                dtOriData = this.dtOrderQty;
            }
            else
            {
                dtOriData = dt;
            }

            //// 給Grid3值***************************************
            foreach (DataRow row in dtOriData.Rows)
            {
                string article = row["Article"].ToString();
                string sizecode = row["SizeCode"].ToString();
                int qty = Convert.ToInt32(row["Qty"]);

                DataRow[] rows = this.dtOriTable.Select($"Article = '{article}'");

                if (rows.Length <= 0)
                {
                    //// 若沒有找到Key，表示資料為新增

                    DataRow newRow = this.dtOriTable.NewRow();
                    newRow["Article"] = article;

                    if (this.dtOriTable.Columns.Contains(sizecode + StrOriginal))
                    {
                        newRow[sizecode + StrOriginal] = qty;
                        newRow[sizecode + StrAssign] = 0;
                    }

                    this.dtOriTable.Rows.Add(newRow);
                }
                else
                {
                    //// 若有找到Key，表示為修改
                    DataRow currRow = rows[0];

                    if (this.dtOriTable.Columns.Contains(sizecode + StrOriginal))
                    {
                        currRow[sizecode + StrOriginal] = qty;
                    }
                }
            }

            var currCell = this.grid3.CurrentCell;

            //// DataSource給值之後會讓Grid的所有欄位IsEditingReadOnly變為False
            this.grid3bs.DataSource = this.dtOriTable;
            this.SetGridIsEditingReadOnly(this.grid3, string.Empty);

            this.grid3.CurrentCell = currCell;

            return res;
        }

        private void SetGridIsEditingReadOnly(Sci.Win.UI.Grid grid, string openColumn)
        {
            foreach (var col in grid.Columns)
            {
                if (col is DataGridViewComboBoxColumn)
                {
                    DataGridViewComboBoxColumn tmpCol = col as DataGridViewComboBoxColumn;
                    if (openColumn != string.Empty && tmpCol.DataPropertyName.Contains(openColumn) == true)
                    {
                        tmpCol.IsEditingReadOnly = false;
                    }
                    else
                    {
                        tmpCol.IsEditingReadOnly = true;
                    }
                }

                if (col is DataGridViewTextBoxBaseColumn)
                {
                    DataGridViewTextBoxBaseColumn tmpCol = col as DataGridViewTextBoxBaseColumn;
                    if (openColumn != string.Empty && tmpCol.DataPropertyName.Contains(openColumn) == true)
                    {
                        tmpCol.IsEditingReadOnly = false;
                    }
                    else
                    {
                        tmpCol.IsEditingReadOnly = true;
                    }
                }
            }
        }

        /// <summary>
        /// 載入Main資料，需要根據Grid1選擇SP重新載入，並透過ItemChangeCls載回User設定的資料、及選擇的Cell
        /// </summary>
        /// <param name="refresh"> Refresh or not </param>
        /// <param name="spList"> sp list </param>
        /// <returns> Execute success or not </returns>
        private DualResult LoadData(bool refresh, List<string> spList = null)
        {
            if (refresh == false)
            {
                this.dtMain.Rows.Clear();
            }

            if (spList == null)
            {
                if (this.dtList == null)
                {
                    return new DualResult(false, "Load SP# list first.");
                }

                spList = this.dtList.AsEnumerable().Where(row => row.RowState != DataRowState.Deleted).Select(rr => rr["ID"].ToString()).ToList();
            }

            string spStr = $"'{string.Join("','", spList)}'";

            DataTable dt;
            string cmd = $@"
	select
		oq.ID
		, oq.Article
		, oq.SizeCode
		, BalanceQty = oq.Qty - isnull((select sum(obq.Qty - iif(obq.ID = '{this.ID}', obq.Qty, 0)) from Order_BuyBack_Qty obq where obq.OrderIDFrom = oq.id and obq.Article = oq.Article and obq.SizeCode = oq.SizeCode), 0)
		, AssignQty = isnull((select sum(obq.Qty) from Order_BuyBack_Qty obq where obq.OrderIDFrom = oq.id and obq.Article = oq.Article and obq.SizeCode = oq.SizeCode and obq.ID = '{this.ID}'), 0)
	from Order_Qty oq
	where oq.ID in ({spStr})";

            DualResult res = DBProxy.Current.Select(null, cmd, out dt);
            if (res == false)
            {
                this.ShowErr(res);
                return new DualResult(false);
            }

            //// 找Key填入值
            foreach (DataRow row in dt.Rows)
            {
                string fromSP = row["ID"].ToString();
                string article = row["Article"].ToString();
                string sizecode = row["SizeCode"].ToString();
                int balanceQty = Convert.ToInt32(row["BalanceQty"]);
                int assignQty = Convert.ToInt32(row["AssignQty"]);

                DataRow[] rowsArticle = this.dtOriTable.Select($"Article = '{article}'");

                //// 跳過Article不符合的項目
                if (rowsArticle.Length <= 0)
                {
                    continue;
                }

                //// 跳過SizeCode不符合的項目
                if (this.dtSizeCode.AsEnumerable().Where(rr => rr["SizeCode"].ToString() == sizecode).Any() == false)
                {
                    continue;
                }

                DataRow[] rows = this.dtMain.Select($"FromSP = '{fromSP}' and Article = '{article}'");

                if (rows.Length <= 0)
                {
                    //// 若沒有找到Key，表示資料為新增
                    DataRow newRow = this.dtMain.NewRow();
                    newRow["FromSP"] = fromSP;
                    newRow["Article"] = article;
                    newRow[sizecode + StrBalance] = balanceQty;
                    if (refresh == false)
                    {
                        newRow[sizecode + StrAssign] = assignQty;
                    }

                    this.dtMain.Rows.Add(newRow);
                }
                else
                {
                    //// 若有找到Key，表示為修改
                    DataRow currRow = rows[0];
                    currRow[sizecode + StrBalance] = balanceQty;
                    if (refresh == false)
                    {
                        currRow[sizecode + StrAssign] = assignQty;
                    }
                }
            }

            if (refresh == false)
            {
                this.dtMain.AcceptChanges();
            }

            this.LoadAssign();

            return res;
        }

        private void LoadAssign()
        {
            foreach (DataRow sizeRow in this.dtSizeCode.Rows)
            {
                string sizecode = sizeRow["SizeCode"].ToString();

                this.LoadAssignBySizeCode(sizecode);
            }
        }

        private void LoadAssignBySizeCode(string sizecode)
        {
            var articleList = this.dtMain.AsEnumerable().Where(row => row.RowState != DataRowState.Deleted)
            .GroupBy(rr => new
            {
                article = rr["Article"].ToString()
            })
            .Select(rr => new
            {
                article = rr.Key.article,
                sumAssign = rr.Sum(rrr => MyUtility.Convert.GetInt(rrr[sizecode + StrAssign]))
            })
            .ToList();

            foreach (var item in articleList)
            {
                DataRow[] rows = this.dtOriTable.Select($"Article = '{item.article}'");
                if (rows.Length == 1)
                {
                    rows[0][sizecode + StrAssign] = item.sumAssign;
                }
                else
                {
                    //// Unbelivi
                    MyUtility.Msg.ErrorBox($"Something error, Article:{item.article} not found.", "error");
                }
            }

            this.grid3.Refresh();
        }

        /// <inheritdoc/>
        protected override void OnEditModeChanged()
        {
            base.OnEditModeChanged();

            if (this.BtnEdit == null)
            {
                return;
            }

            //// this.changeList.Clear();

            if (this.EditMode == true)
            {
                this.BtnEdit.Text = "Save";
                this.BtnClose.Text = "Undo";
                this.BtnAdd.Enabled = true;
                this.BtnDelete.Enabled = true;

                if (this.grid1.Columns.Count > 0)
                {
                    (this.grid1.Columns[1] as DataGridViewComboBoxColumn).IsEditingReadOnly = false;
                }
            }
            else
            {
                this.BtnEdit.Text = "Edit";
                this.BtnClose.Text = "Close";
                this.BtnAdd.Enabled = false;
                this.BtnDelete.Enabled = false;

                if (this.grid1.Columns.Count > 0)
                {
                    (this.grid1.Columns[1] as DataGridViewComboBoxColumn).IsEditingReadOnly = true;
                }
            }
        }

        /// <summary>
        /// 1. Check Assign > Balance
        /// 2. Check TotalAssign > Original
        /// 3. Show Message if error
        /// </summary>
        /// <returns> Check OK or not </returns>
        private bool CheckData()
        {
            //// Validate
            this.grid1.ValidateControl();
            this.grid2.ValidateControl();
            this.grid4.ValidateControl();

            //// Refresh Data
            this.LoadData(true);

            List<string> errEmpIDList = new List<string>();
            List<string> errEmpReasonList = new List<string>();
            List<string> errList1 = new List<string>(); //// FromSP, Article, SizeCode : Assign More than Balance
            List<string> errList2 = new List<string>(); //// Article, SizeCode : Assign More than Original

            this.dtList.AsEnumerable()
                .Where(row => row.RowState != DataRowState.Deleted)
                .Where(rr => MyUtility.Check.Empty(rr["ID"]))
                .ToList()
                .ForEach(rr => errEmpIDList.Add(rr["ID"].ToString()));

            this.dtList.AsEnumerable()
                .Where(row => row.RowState != DataRowState.Deleted)
                .Where(rr => MyUtility.Check.Empty(rr["BuyBackReason"]))
                .ToList()
                .ForEach(rr => errEmpReasonList.Add(rr["ID"].ToString()));

            this.dtMain.AsEnumerable()
                .Where(row => row.RowState != DataRowState.Deleted)
                .ToList()
                .ForEach(row =>
                {
                    string fromSP = row["FromSP"].ToString();
                    string article = row["Article"].ToString();

                    foreach (DataRow sizeRow in this.dtSizeCode.Rows)
                    {
                        string sizecode = sizeRow["SizeCode"].ToString();

                        if (row[sizecode + StrAssign] == DBNull.Value)
                        {
                            //// 代表沒設定Assign數量，可以不判斷之後也不儲存
                            continue;
                        }

                        int balanceQty = MyUtility.Convert.GetInt(row[sizecode + StrBalance]);
                        int assignQty = MyUtility.Convert.GetInt(row[sizecode + StrAssign]);

                        if (assignQty > balanceQty)
                        {
                            errList1.Add($"Size:{sizecode}  SP#:{fromSP}  Article:{article}  {assignQty} > {balanceQty}");
                        }
                    }
                });

            int orderQty = 0;
            int ttlAssignQty = 0;

            foreach (DataRow sizeRow in this.dtSizeCode.Rows)
            {
                string sizecode = sizeRow["SizeCode"].ToString();

                foreach (DataRow row in this.dtOriTable.Rows)
                {
                    string article = row["Article"].ToString();
                    int originalQty = MyUtility.Convert.GetInt(row[sizecode + StrOriginal]);
                    int assignQty = MyUtility.Convert.GetInt(row[sizecode + StrAssign]);
                    if (assignQty > originalQty)
                    {
                        errList2.Add($"Article:{article} SizeCode:{sizecode}  {assignQty} > {originalQty}");
                    }

                    orderQty += originalQty;
                    ttlAssignQty += assignQty;
                }
            }

            string errStr = string.Empty;

            if (errEmpIDList.Count() > 0)
            {
                errStr += "<SP#> is empty.";
            }

            if (errEmpReasonList.Count() > 0)
            {
                errStr += "<BuyBack Reason> is empty : "
                        + Environment.NewLine
                        + string.Join(Environment.NewLine, errEmpReasonList.OrderBy(rr => rr.ToString()).Distinct().ToList());
            }

            if (errList1.Count() > 0)
            {
                errStr += "<Assign Qty> more then <BalanceQty> : "
                    + Environment.NewLine
                    + string.Join(Environment.NewLine, errList1.OrderBy(rr => rr.ToString()).Distinct().ToList());
            }

            if (errList2.Count() > 0)
            {
                errStr += Environment.NewLine + "<Assign Qty> more then <Original Qty> : "
                    + Environment.NewLine
                    + string.Join(Environment.NewLine, errList2.OrderBy(rr => rr.ToString()).Distinct().ToList());
            }

            if (orderQty != ttlAssignQty)
            {
                errStr += Environment.NewLine + $"Total <Assign Qty> ({ttlAssignQty}) should be equal with order <Qty> ({orderQty}).";
            }

            if (errStr != string.Empty)
            {
                MyUtility.Msg.ErrorBox(errStr);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 1. Check Data
        /// 2. 建立SQL Command
        /// 3. Executes儲存資料
        /// </summary>
        /// <returns> Save Success or not </returns>
        private DualResult Save()
        {
            //// Check Data***************************************
            if (this.CheckData() == false)
            {
                return new DualResult(false, "Check fail");
            }

            //// 建立SQL Command***************************************
            List<SqlCommandText> cmdList = new List<SqlCommandText>();
            List<SqlParameter> plis = new List<SqlParameter>();
            DateTime dateNow = DateTime.Now;
            SqlCommandText sqlcmd;

            foreach (DataRow row in this.dtList.Rows)
            {
                string cmd = string.Empty;

                if (row.RowState == DataRowState.Deleted)
                {
                    string fromsp = row["ID", DataRowVersion.Original].ToString();
                    cmd = @"
                        delete from Order_BuyBack where ID = @ID and OrderIDFrom = @OrderIDFrom
                        delete from Order_BuyBack_Qty where ID = @ID and OrderIDFrom = @OrderIDFrom";

                    plis = new List<SqlParameter>();
                    plis.Add(new SqlParameter("@ID", this.ID));
                    plis.Add(new SqlParameter("@OrderIDFrom", fromsp));

                    sqlcmd = new SqlCommandText(cmd, plis);
                    cmdList.Add(sqlcmd);

                    continue;
                }

                if (row.RowState == DataRowState.Unchanged)
                {
                    continue;
                }

                string fromSP = row["ID"].ToString();
                string reason = row["BuyBackReason"].ToString();

                plis = new List<SqlParameter>();
                plis.Add(new SqlParameter("@ID", this.ID));
                plis.Add(new SqlParameter("@OrderIDFrom", fromSP));
                plis.Add(new SqlParameter("@BuyBackReason", reason));
                plis.Add(new SqlParameter("@UserID", Env.User.UserID));
                plis.Add(new SqlParameter("@date", dateNow));

                if (row.RowState == DataRowState.Added)
                {
                    cmd = @"
                    Insert into Order_BuyBack (ID, OrderIDFrom, BuyBackReason, AddName, AddDate) 
                    values (@ID, @OrderIDFrom, @BuyBackReason, @UserID, @date)";
                }

                if (row.RowState == DataRowState.Modified)
                {
                    cmd = @"
                    Update Order_BuyBack Set 
                        BuyBackReason = @BuyBackReason
                        , EditName = @UserID
                        , EditDate = @date 
                    where ID = @ID and OrderIDFrom = @OrderIDFrom";
                }

                sqlcmd = new SqlCommandText(cmd, plis);
                cmdList.Add(sqlcmd);
            }

            //// Add, Modify*************
            var dicAdd = this.GetTableCellAdd(this.dtMain, StrAssign);
            var dicModify = this.GetTableCellModify(this.dtMain, StrAssign);

            foreach (var item in dicAdd.Union(dicModify))
            {
                DataRow row = item.Key;
                string fromSP = row["FromSP"].ToString();
                string article = row["Article"].ToString();
                string cmd = string.Empty;

                foreach (var col in item.Value)
                {
                    string sizecode = col.Replace(StrAssign, string.Empty);
                    int qty = MyUtility.Convert.GetInt(row[col]);

                    //// Update Order_BuyBack_Qty
                    cmd = @"
if (@Qty = 0)
BEGIN
    Delete from Order_BuyBack_Qty where ID = @ID AND OrderIDFrom = @OrderIDFrom and Article = @Article and SizeCode = @SizeCode;
END
ELSE
if Exists(select 1 from Order_BuyBack_Qty obq where obq.ID = @ID and obq.OrderIDFrom = @OrderIDFrom and obq.Article = @Article and obq.SizeCode = @SizeCode)
BEGIN
    Update Order_BuyBack_Qty Set
        Qty = @Qty
        , EditName = @UserID
        , EditDate = @date 
    where ID = @ID and OrderIDFrom = @OrderIDFrom and Article = @Article and SizeCode = @SizeCode
END
ELSE
BEGIN
    insert into Order_BuyBack_Qty (ID, OrderIDFrom, Article, SizeCode, Qty, AddName, AddDate)
    values (@ID, @OrderIDFrom, @Article, @SizeCode, @Qty, @UserID, @date)
END
";

                    plis = new List<SqlParameter>();
                    plis.Add(new SqlParameter("@ID", this.ID));
                    plis.Add(new SqlParameter("@OrderIDFrom", fromSP));
                    plis.Add(new SqlParameter("@Article", article));
                    plis.Add(new SqlParameter("@SizeCode", sizecode));
                    plis.Add(new SqlParameter("@Qty", qty));
                    plis.Add(new SqlParameter("@UserID", Env.User.UserID));
                    plis.Add(new SqlParameter("@date", dateNow));

                    sqlcmd = new SqlCommandText(cmd, plis);
                    cmdList.Add(sqlcmd);
                }
            }

            //// Executes
            if (cmdList.Count > 0)
            {
                plis = new List<SqlParameter>();
                plis.Add(new SqlParameter("@ID", this.ID));
                plis.Add(new SqlParameter("@UserID", Env.User.UserID));
                string cmd = @"
declare @IsBuyBack table (OldValue bit, NewValue bit);

update o set IsBuyBack = IIF(hasBuyBack.c > 0, 1, 0)
output deleted.IsBuyBack, inserted.IsBuyBack into @IsBuyBack (OldValue, NewValue)
from Orders o
outer apply (select count(*) c from Order_BuyBack ob where ob.ID = o.ID) hasBuyBack
where o.ID = @ID

insert into TradeHIS_Order (TableName, HisType, SourceID, OldValue, NewValue, AddName, AddDate)
select 'Orders', 'IsBuyBack', @ID, IIF(OldValue = 1, 'Y', ''), IIF(NewValue = 1, 'Y', ''), @UserID, getdate()
from @IsBuyBack where OldValue != NewValue";
                sqlcmd = new SqlCommandText(cmd, plis);
                cmdList.Add(sqlcmd);

                DualResult res = DBProxy.Current.Executes(string.Empty, cmdList);
                if (res == false)
                {
                    MyUtility.Msg.ErrorBox(res.ToString());
                    return res;
                }

                this.IsDataChange = true;
            }
            else
            {
                return new DualResult(true, "No datas modify.");
            }

            return new DualResult(true);
        }

        private Dictionary<DataRow, List<string>> GetTableCellAdd(DataTable dtData, string columnKeyWord)
        {
            Dictionary<DataRow, List<string>> dic = new Dictionary<DataRow, List<string>>();

            foreach (DataRow row in dtData.Rows)
            {
                if (row.RowState != DataRowState.Added)
                {
                    continue;
                }

                for (int i = 0; i < dtData.Columns.Count; i++)
                {
                    if (dtData.Columns[i].ColumnName.Contains(columnKeyWord)
                        && MyUtility.Check.Empty(row[i]) == false)
                    {
                        if (dic.ContainsKey(row))
                        {
                            dic[row].Add(dtData.Columns[i].ColumnName);
                        }
                        else
                        {
                            dic.Add(row, new List<string> { dtData.Columns[i].ColumnName });
                        }
                    }
                }
            }

            return dic;
        }

        private Dictionary<DataRow, List<string>> GetTableCellModify(DataTable dtData, string columnKeyWord)
        {
            Dictionary<DataRow, List<string>> dic = new Dictionary<DataRow, List<string>>();

            foreach (DataRow row in dtData.Rows)
            {
                if (row.RowState != DataRowState.Modified)
                {
                    continue;
                }

                for (int i = 0; i < dtData.Columns.Count; i++)
                {
                    if (dtData.Columns[i].ColumnName.Contains(columnKeyWord)
                        && row[i].Equals(row[i, DataRowVersion.Original]) == false)
                    {
                        if (dic.ContainsKey(row))
                        {
                            dic[row].Add(dtData.Columns[i].ColumnName);
                        }
                        else
                        {
                            dic.Add(row, new List<string> { dtData.Columns[i].ColumnName });
                        }
                    }
                }
            }

            return dic;
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (this.EditMode == true)
            {
                // Do Save
                DualResult res = this.Save();

                if (res == true)
                {
                    //// Save ok
                    this.LoadList();
                    this.LoadOriTableData();
                    this.LoadData(false);

                    this.EditMode = false;
                }
                else
                {
                    //// Save fail
                }
            }
            else
            {
                // To Edit
                this.EditMode = true;
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            if (this.EditMode == true)
            {
                // RollBack
                this.LoadList();
                this.LoadOriTableData();
                this.LoadData(false);

                // To View
                this.EditMode = false;
            }
            else
            {
                this.Close();
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            this.grid2.ValidateControl();
            this.grid4.ValidateControl();

            //// Reload online data
            this.LoadOriTableData();
            this.LoadData(true);
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (this.EditMode == true)
            {
                this.dtList.Rows.Add();
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (this.EditMode == true)
            {
                if (this.grid1.CurrentRow == null)
                {
                    return;
                }

                DataRow row = ((DataRowView)this.grid1.CurrentRow.DataBoundItem).Row;
                string id = row["ID"].ToString();
                row.Delete();

                if (id != string.Empty)
                {
                    for (int i = this.dtMain.Rows.Count - 1; i >= 0; i--)
                    {
                        if (this.dtMain.Rows[i].RowState != DataRowState.Deleted
                            && this.dtMain.Rows[i]["FromSP"].ToString() == id)
                        {
                            this.dtMain.Rows[i].Delete();
                        }
                    }
                }
            }
        }

        private void Grid_CellFormatting(object sender, System.Windows.Forms.DataGridViewCellFormattingEventArgs e)
        {
            Sci.Win.UI.Grid thisGrid = (Sci.Win.UI.Grid)sender;
            DataGridViewColumn thisColumn = thisGrid.Columns[e.ColumnIndex];

            if (thisColumn.DataPropertyName.Contains(StrAssign))
            {
                e.CellStyle.BackColor = Color.FromArgb(128, 255, 255);
                if (this.EditMode)
                {
                    e.CellStyle.ForeColor = Color.FromArgb(255, 0, 0);
                }
                else
                {
                    e.CellStyle.ForeColor = Color.FromArgb(0, 0, 0);
                }
            }
        }

        private void Grid2_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //// Grid2 Load Assign by SizeCode
            if (this.grid2.Columns[e.ColumnIndex].DataPropertyName.Contains(StrAssign) == false)
            {
                return;
            }

            string sizecode = this.grid2.Columns[e.ColumnIndex].DataPropertyName.Replace(StrAssign, string.Empty);
            this.LoadAssignBySizeCode(sizecode);
            this.SetGrid3ValueToGrid4();
            ////this.Grid2_CellEnter(null, new DataGridViewCellEventArgs(e.ColumnIndex, e.RowIndex));
        }

        private void Grid2_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            //// Grid2 enter change Grid3 cell
            if (this.grid2.Columns[e.ColumnIndex].DataPropertyName.Contains(StrAssign) == false
                && this.grid2.Columns[e.ColumnIndex].DataPropertyName.Contains(StrBalance) == false)
            {
                return;
            }

            DataRow row = (this.grid2.CurrentRow.DataBoundItem as DataRowView).Row;
            string article = row["Article"].ToString();
            string sizecode = this.grid2.Columns[e.ColumnIndex].DataPropertyName.Replace(StrAssign, string.Empty).Replace(StrBalance, string.Empty);

            int rowIndex = this.grid3bs.Find("Article", article);
            this.grid3.CurrentCell = this.grid3[this.grid3.Columns[sizecode + StrAssign].Index, rowIndex];
        }

        private void Grid3_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (this.grid3.Columns[e.ColumnIndex].DataPropertyName.Contains(StrOriginal) == false
                && this.grid3.Columns[e.ColumnIndex].DataPropertyName.Contains(StrAssign) == false)
            {
                return;
            }

            this.SetGrid3ValueToGrid4();
        }

        private void SetGrid3ValueToGrid4()
        {
            if (this.grid3.CurrentCell == null)
            {
                return;
            }

            int colIdx = this.grid3.CurrentCell.ColumnIndex;

            string sizecode = this.grid3.Columns[colIdx].DataPropertyName.Replace(StrAssign, string.Empty).Replace(StrOriginal, string.Empty);
            string article = (this.grid3.CurrentRow.DataBoundItem as DataRowView).Row["Article"].ToString();

            DataTable dtTmp; // = this.dtMain.AsEnumerable().TryCopyToDataTable(this.dtMain);

            if (this.dtMain.AsEnumerable().Any())
            {
                dtTmp = this.dtMain.AsEnumerable().CopyToDataTable();
            }
            else
            {
                dtTmp = this.dtMain.Clone();
            }

            this.grid4bs.DataSource = dtTmp;
            this.grid4.Columns[1].DataPropertyName = sizecode + StrBalance;
            this.grid4.Columns[1].HeaderText = sizecode + StrBalance;
            this.grid4.Columns[2].DataPropertyName = sizecode + StrAssign;
            this.grid4.Columns[2].HeaderText = sizecode + StrAssign;
            this.grid4bs.Filter = $"Article = '{article}'";
        }

        private void Grid4_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //// Grid4 end edit update Grid2
            if (this.grid4.Columns[e.ColumnIndex].DataPropertyName.Contains(StrAssign) == false)
            {
                return;
            }

            this.grid4.EndEdit();

            DataRow row = (this.grid4.CurrentRow.DataBoundItem as DataRowView).Row;
            string fromsp = row["FromSP"].ToString();
            string article = row["Article"].ToString();
            string sizecode = this.grid4.Columns[2].DataPropertyName.Replace(StrAssign, string.Empty);

            DataRow[] rows = this.dtMain.Select($"FromSP = '{fromsp}' and Article = '{article}'");
            if (rows.Length > 0)
            {
                rows[0][sizecode + StrAssign] = row[sizecode + StrAssign];
            }

            this.LoadAssignBySizeCode(sizecode);
        }

        #region Set Combobox Filter
        private void SetCombo()
        {
            this.SetEmptyCombo(this.CboSP, "FromSP");
            this.SetEmptyCombo(this.CboArticle, "Article");
            this.SetEmptyCombo(this.CboSizeGroup, "SizeGroup");

            this.CboSP.DropDown += (s, e) =>
            {
                this.SetComboSubMethod(this.CboSP, "FromSP", ref this.dtMain, ref this.listFromSP);
            };

            this.CboArticle.DropDown += (s, e) =>
            {
                this.SetComboSubMethod(this.CboArticle, "Article", ref this.dtMain, ref this.listArticle);
            };

            this.CboSizeGroup.DropDown += (s, e) =>
            {
                this.SetComboSubMethod(this.CboSizeGroup, "SizeGroup", ref this.dtSizeCode, ref this.listSizeGroup);
            };
        }

        private void SetComboSubMethod(Sci.Win.UI.ComboBox cb, string keyName, ref DataTable data, ref DataTable list)
        {
            var obj = cb.SelectedValue;
            cb.SelectedValue = string.Empty;

            this.DataFilter();
            DataTable sourceTbl = data.DefaultView.ToTable();
            list = this.GetComboData(keyName, sourceTbl);
            cb.DisplayMember = keyName;
            cb.ValueMember = keyName;
            cb.DataSource = list;
            if (obj != null)
            {
                cb.SelectedValue = obj;
            }
        }

        private void SetEmptyCombo(Sci.Win.UI.ComboBox cb, string keyName)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(keyName, typeof(string));
            dt.Rows.Add();
            cb.DisplayMember = keyName;
            cb.ValueMember = keyName;
            cb.DataSource = dt;
        }

        private void Combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.DataFilter();
        }

        private void DataFilter()
        {
            string strWhere = "1=1";

            string fromsp = string.Empty;
            string article = string.Empty;
            string sizegroup = string.Empty;

            if (!MyUtility.Check.Empty(this.CboSP.SelectedValue))
            {
                fromsp = this.CboSP.SelectedValue.ToString();
            }

            if (!MyUtility.Check.Empty(this.CboArticle.SelectedValue))
            {
                article = this.CboArticle.SelectedValue.ToString();
            }

            if (!MyUtility.Check.Empty(fromsp))
            {
                strWhere += " And FromSP = '" + fromsp + "'";
            }

            if (!MyUtility.Check.Empty(article))
            {
                strWhere += " And Article = '" + article + "'";
            }

            this.grid2bs.Filter = strWhere;
        }

        private DataTable GetComboData(string groupColumn, DataTable sourceTable, bool addEmpty = true)
        {
            DataTable list;
            if (sourceTable == null || sourceTable.Rows.Count == 0)
            {
                list = null;
            }
            else
            {
                list = sourceTable.AsEnumerable().GroupBy(r => new { GroupCol = r[groupColumn] })
                                                 .Select(g => g.First())
                                                 .CopyToDataTable();
                if (addEmpty)
                {
                    if (list.Select("IsNull(" + groupColumn + ", '') = ''").Length == 0)
                    {
                        DataRow emptyRow = list.NewRow();
                        emptyRow[groupColumn] = string.Empty;
                        list.Rows.Add(emptyRow);
                    }
                }

                list.DefaultView.Sort = groupColumn;
            }

            return list;
        }

        private void CboSizeGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (DataRow row in this.dtSizeCode.Rows)
            {
                string sizecode = row["SizeCode"].ToString();
                string sizegroup = row["SizeGroup"].ToString();
                if (this.CboSizeGroup.SelectedValue == null
                    || this.CboSizeGroup.SelectedValue.ToString() == string.Empty
                    || sizegroup == this.CboSizeGroup.SelectedValue.ToString())
                {
                    this.grid2.Columns[sizecode + StrBalance].Visible = true;
                    this.grid2.Columns[sizecode + StrAssign].Visible = true;
                    this.grid3.Columns[sizecode + StrOriginal].Visible = true;
                    this.grid3.Columns[sizecode + StrAssign].Visible = true;
                }
                else
                {
                    this.grid2.Columns[sizecode + StrBalance].Visible = false;
                    this.grid2.Columns[sizecode + StrAssign].Visible = false;
                    this.grid3.Columns[sizecode + StrOriginal].Visible = false;
                    this.grid3.Columns[sizecode + StrAssign].Visible = false;
                }
            }
        }
        #endregion
    }
}
