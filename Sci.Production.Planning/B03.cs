﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.PublicForm;

namespace Sci.Production.Planning
{
    /// <summary>
    /// B03
    /// </summary>
    public partial class B03 : Win.Tems.Input6
    {
        private Ict.Win.UI.DataGridViewTextBoxColumn col_Supplier;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_Size;
        private Ict.Win.UI.DataGridViewDateBoxColumn col_OvenTest;
        private Ict.Win.UI.DataGridViewDateBoxColumn col_WashTest;
        private Ict.Win.UI.DataGridViewDateBoxColumn col_MockupTest;
        private Ict.Win.UI.DataGridViewNumericBoxColumn col_Price;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_Remark;
        private Ict.Win.UI.DataGridViewComboBoxColumn col_PriceApv;

        private DataTable style_artwork;

        /// <summary>
        /// B03
        /// </summary>
        /// <param name="menuitem">PlanningB03</param>
        public B03(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// ClickEditAfter
        /// </summary>
        protected override void ClickEditAfter()
        {
            this.refresh.Enabled = true;
            base.ClickEditAfter();
        }

        /// <summary>
        /// ClickCopy
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ClickCopy()
        {
            if (MyUtility.Check.Empty(this.CurrentDetailData))
            {
                MyUtility.Msg.WarningBox("No Quot. data can't be copied, Please add quot. detail first!");
                return true;
            }

            DataRow dr = this.gridArtworkType.GetDataRow<DataRow>(this.gridArtworkType.GetSelectedRowIndex());
            var frm = new B03_Copy(dr);
            frm.ShowDialog(this);
            this.RenewData();
            return true;
        }

        /// <summary>
        /// ClickSaveBefore
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ClickSaveBefore()
        {
            this.gridArtworkType.ValidateControl();
            DataTable errorTable = new DataTable("errorTable");
            errorTable.Columns.Add("Artwork Type");
            errorTable.Columns.Add("Article");
            errorTable.Columns.Add("Artwork");
            errorTable.Columns.Add("Artwork Name");
            errorTable.Columns.Add("Cut Part");
            errorTable.Columns.Add("Supplier");
            errorTable.Columns.Add("Name");
            errorTable.Columns.Add("Size");
            errorTable.Columns.Add("Taipei Price");
            errorTable.Columns.Add("Local Price");

            var list = ((DataTable)this.detailgridbs.DataSource).AsEnumerable().Where(s => s.RowState == DataRowState.Added || s.RowState == DataRowState.Detached);
            for (int i = 0; i < this.gridArtworkType.RowCount; i++)
            {
                DataRow gridArtworkTypedataRow = this.gridArtworkType.GetDataRow(i);
                if (list != null)
                {
                    foreach (DataRow dataRow in list)
                    {
                        if (MyUtility.Convert.GetDecimal(dataRow["Price"]) > MyUtility.Convert.GetDecimal(gridArtworkTypedataRow["Cost"])
                        && MyUtility.Convert.GetString(dataRow["UKEY"]) == MyUtility.Convert.GetString(gridArtworkTypedataRow["UKEY"])
                        && MyUtility.Check.Empty(dataRow["Remark"]))
                        {
                            DataRow workRow = errorTable.NewRow();
                            workRow["Artwork Type"] = gridArtworkTypedataRow["artworktypeid"];
                            workRow["Article"] = gridArtworkTypedataRow["article"];
                            workRow["Artwork"] = gridArtworkTypedataRow["artworkid"];
                            workRow["Artwork Name"] = gridArtworkTypedataRow["artworkname"];
                            workRow["Cut Part"] = gridArtworkTypedataRow["patterncode"];
                            workRow["Supplier"] = dataRow["localsuppid"];
                            workRow["Name"] = dataRow["suppname"];
                            workRow["Size"] = dataRow["SizeCode"];
                            workRow["Taipei Price"] = gridArtworkTypedataRow["Cost"];
                            workRow["Local Price"] = dataRow["price"];
                            errorTable.Rows.Add(workRow);
                        }
                    }
                }
            }

            if (errorTable.Rows.Count != 0)
            {
                string msg = "Local Quotation has exceeded Taipei Quotation and no reason entered, are you sure you want to saving?\r\nPlease refer to the list below.";
                MessageYESNO win = new MessageYESNO(msg, errorTable, "Warning");
                win.ShowDialog(this);
                if (!win.isYN)
                {
                    return false;
                }
            }

            this.detailgridbs.Filter = string.Empty;
            StringBuilder warningmsg = new StringBuilder();
            foreach (DataRow row in this.DetailDatas)
            {
                if (MyUtility.Check.Empty(row["localsuppid"]))
                {
                    warningmsg.Append(string.Format(@"Ukey: {0}  , Supplier can't be empty", row["ukey"])
                        + Environment.NewLine);
                }
            }

            if (!MyUtility.Check.Empty(warningmsg.ToString()))
            {
                this.detailgridbs.Filter = "ukey=" + this.gridArtworkType.GetDataRow(this.gridArtworkType.GetSelectedRowIndex())["ukey"].ToString();
                MyUtility.Msg.WarningBox(warningmsg.ToString());
                return false;
            }

            return base.ClickSaveBefore();
        }

        /// <summary>
        /// ClickSavePost
        /// </summary>
        /// <returns>true</returns>
        protected override DualResult ClickSavePost()
        {
            this.gridArtworkType.ValidateControl();
            var listUpdateStyle_Artwork = ((DataTable)this.listControlBindingSource1.DataSource).AsEnumerable().Where(s => s.RowState == DataRowState.Modified);
            if (listUpdateStyle_Artwork.Any())
            {
                string updSql;
                DualResult result;
                foreach (DataRow itemStyle_Artwork in listUpdateStyle_Artwork)
                {
                    updSql = $@"update Style_Artwork set ActStitch = {itemStyle_Artwork["ActStitch"]} where Ukey = {itemStyle_Artwork["Ukey"]} ";
                    result = DBProxy.Current.Execute(null, updSql);
                    if (!result)
                    {
                        return result;
                    }
                }
            }

            return base.ClickSavePost();
        }

        /// <summary>
        /// ClickSaveAfter
        /// </summary>
        protected override void ClickSaveAfter()
        {
            this.Filter_detailgrid(this.gridArtworkType.GetSelectedRowIndex());
            base.ClickSaveAfter();
        }

        /// <summary>
        /// OnDetailGridInsert
        /// </summary>
        /// <param name="index">index</param>
        protected override void OnDetailGridInsert(int index = -1)
        {
            this.detailgridbs.Filter = string.Empty;
            base.OnDetailGridInsert(index);
            if (this.gridArtworkType.GetSelectedRowIndex() >= 0)
            {
                this.CurrentDetailData["ukey"] = this.gridArtworkType.GetDataRow(this.gridArtworkType.GetSelectedRowIndex())["ukey"].ToString();
                this.detailgridbs.Filter = "ukey=" + this.gridArtworkType.GetDataRow(this.gridArtworkType.GetSelectedRowIndex())["ukey"].ToString();
            }
        }

        /// <summary>
        /// OnDetailGridSetup
        /// </summary>
        protected override void OnDetailGridSetup()
        {
            #region Supplier 右鍵開窗
            DataGridViewGeneratorTextColumnSettings ts4 = new DataGridViewGeneratorTextColumnSettings();
            ts4.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    Win.Tools.SelectItem item;
                    string sqlcmd;

                    sqlcmd = @"
select l.id ,l.abb ,l.currencyid  ,l.IsSintexSubcon
,[IsSintex] = iif(l.IsSintexSubcon =1,'Y','N'),
from LocalSupp l WITH (NOLOCK) 
WHERE l.Junk=0  AND l.IsFactory = 0
order by ID
";
                    item = new Win.Tools.SelectItem(sqlcmd, "10,15,5", null)
                    {
                        Size = new System.Drawing.Size(480, 500),
                    };
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    var x = item.GetSelecteds();

                    this.CurrentDetailData["localsuppid"] = x[0]["id"].ToString();
                    this.CurrentDetailData["suppname"] = x[0]["abb"].ToString();
                    this.CurrentDetailData["currencyid"] = x[0]["currencyid"].ToString();
                    this.CurrentDetailData["IsSintex"] = x[0]["IsSintex"].ToString();
                    if (MyUtility.Convert.GetBool(x[0]["IsSintexSubcon"]) && 
                    MyUtility.Convert.GetString(this.gridArtworkType.CurrentDataRow["ArtworkTypeID"]).ToUpper() == "PRINTING")
                    {
                        this.CurrentDetailData["price"] = this.gridArtworkType.CurrentDataRow["cost"];
                    }

                    this.CurrentDetailData.EndEdit();
                }
            };
            #endregion
            #region -- Supplier Valid --
            ts4.CellValidating += (s, e) =>
            {
                if (this.EditMode && !MyUtility.Check.Empty(e.FormattedValue))
                {
                    DataRow find;
                    if (MyUtility.Check.Seek(string.Format("Select [IsSintex] = iif(IsSintexSubcon =1,'Y','N'),* from localsupp WITH (NOLOCK) where isfactory=0 and junk=0 and id='{0}'", e.FormattedValue), out find))
                    {
                        this.CurrentDetailData["localsuppid"] = find["id"].ToString();
                        this.CurrentDetailData["suppname"] = find["abb"].ToString();
                        this.CurrentDetailData["currencyid"] = find["currencyid"].ToString();
                        this.CurrentDetailData["IsSintex"] = find["IsSintex"].ToString();
                        if (MyUtility.Convert.GetBool(find["IsSintexSubcon"]) &&
                    MyUtility.Convert.GetString(this.gridArtworkType.CurrentDataRow["ArtworkTypeID"]).ToUpper() == "PRINTING")
                        {
                            this.CurrentDetailData["price"] = this.gridArtworkType.CurrentDataRow["cost"];
                        }

                        this.CurrentDetailData.EndEdit();
                        return;
                    }
                    else
                    {
                        this.CurrentDetailData["localsuppid"] = string.Empty;
                        this.CurrentDetailData["suppname"] = string.Empty;
                        this.CurrentDetailData["currencyid"] = string.Empty;
                        this.CurrentDetailData["IsSintex"] = "N";
                        this.CurrentDetailData["price"] = 0;
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox("Supplier is not found!", "Warning");
                        return;
                    }
                }

                if (e.FormattedValue.ToString() == string.Empty)
                {
                    this.CurrentDetailData["localsuppid"] = string.Empty;
                    this.CurrentDetailData["suppname"] = string.Empty;
                    this.CurrentDetailData["currencyid"] = string.Empty;
                    this.CurrentDetailData["IsSintex"] = "N";
                    this.CurrentDetailData["price"] = 0;
                }
            };
            #endregion

            #region Size column set
            DataGridViewGeneratorTextColumnSettings colSize = new DataGridViewGeneratorTextColumnSettings();
            colSize.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    Sci.Win.Tools.SelectItem item;
                    string sqlcmd;

                    sqlcmd = $@"
select SizeCode
from Style_Sizecode WITH (NOLOCK) 
WHERE StyleUkey = '{this.CurrentMaintain["Ukey"]}'
order by Seq ASC
";
                    item = new Sci.Win.Tools.SelectItem(sqlcmd, null, null);
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    var x = item.GetSelecteds();

                    this.CurrentDetailData["SizeCode"] = x[0]["SizeCode"].ToString();
                }
            };

            colSize.CellValidating += (s, e) =>
            {
                if (this.EditMode && !MyUtility.Check.Empty(e.FormattedValue))
                {
                    bool isSizeExists = MyUtility.Check.Seek($"Select 1 from Style_Sizecode WITH (NOLOCK) where StyleUkey = '{this.CurrentMaintain["Ukey"]}' and SizeCode = '{e.FormattedValue}'");
                    if (!isSizeExists)
                    {
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox("Size is not found!", "Warning");
                        return;
                    }
                }
            };
            #endregion

            
            Dictionary<string, string> comboBox1_RowSource = new Dictionary<string, string>();
            comboBox1_RowSource.Add("Y", "Y");
            comboBox1_RowSource.Add("N", "N");
            comboBox1_RowSource.Add(string.Empty, "N/A");

            #region 欄位設定
            this.Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("localsuppid", header: "Supplier", width: Widths.AnsiChars(6), settings: ts4).Get(out this.col_Supplier)
            .Text("suppname", header: "Name", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("IsSintex", header: "IsSintex", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("currencyid", header: "Currency", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("SizeCode", header: "Size", width: Widths.AnsiChars(10), settings: colSize).Get(out this.col_Size)
            .Numeric("price", header: "Price", width: Widths.AnsiChars(8), decimal_places: 4, integer_places: 10).Get(out this.col_Price)
            .Date("oven", header: "Oven Test", width: Widths.AnsiChars(10)).Get(out this.col_OvenTest)
            .Date("wash", header: "Wash Test", width: Widths.AnsiChars(10)).Get(out this.col_WashTest)
            .Date("mockup", header: "Mockup Test", width: Widths.AnsiChars(10)).Get(out this.col_MockupTest)
            .ComboBox("priceApv", header: "Price Approve").Get(out this.col_PriceApv)
            .Text("Remark", header: "Remark", width: Widths.AnsiChars(10)).Get(out this.col_Remark)
            ;
            this.detailgrid.Columns["Remark"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["Remark"].DefaultCellStyle.ForeColor = Color.Red;
            #endregion 欄位設定

            this.col_PriceApv.DataSource = new BindingSource(comboBox1_RowSource, null);
            this.col_PriceApv.ValueMember = "Key";
            this.col_PriceApv.DisplayMember = "Value";

            #region 欄位設定
            this.Helper.Controls.Grid.Generator(this.gridArtworkType)
            .Text("artworktypeid", header: "Artwork Type", width: Widths.AnsiChars(16), iseditingreadonly: true)
            .Text("article", header: "Article", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("artworkid", header: "Artwork", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("artworkname", header: "Artwork Name", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .Text("patterncode", header: "Cut Part", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Text("patterndesc", header: "Cut Part Name", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .Numeric("tms", header: "TMS", width: Widths.AnsiChars(8), integer_places: 10, iseditingreadonly: true)
            .Numeric("qty", header: "QTY", width: Widths.AnsiChars(8), integer_places: 10, iseditingreadonly: true)
            .Text("unit", header: "Unit", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Numeric("ActStitch", header: "Act. Stitch", width: Widths.AnsiChars(10), decimal_places: 0, iseditingreadonly: false)
            .Numeric("cost", header: "Cost", width: Widths.AnsiChars(8), decimal_places: 4, integer_places: 10, iseditingreadonly: true)
            .Text("remark", header: "Remark", width: Widths.AnsiChars(20), iseditingreadonly: true)
            ;

            #endregion 欄位設定

            this.gridArtworkType.DataSource = this.listControlBindingSource1;
            this.detailgrid.RowEnter += this.Detailgrid_RowEnter;
        }

        private void Detailgrid_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || this.EditMode == false)
            {
                return;
            }

            var data = ((DataRowView)this.detailgrid.Rows[e.RowIndex].DataBoundItem).Row;
            if (data == null)
            {
                return;
            }

            // AutoCrate為1表示資料來源為(P01計算出來的就不能再讓使用者修改線種,顏色,總長度)
            if (data["IsSintex"].ToString() == "Y")
            {
                this.col_Supplier.IsEditingReadOnly = true;
                this.col_Size.IsEditingReadOnly = true;
                this.col_Price.IsEditingReadOnly = true;
                this.col_OvenTest.IsEditingReadOnly = true;
                this.col_WashTest.IsEditingReadOnly = true;
                this.col_MockupTest.IsEditingReadOnly = true;
                this.col_PriceApv.IsEditingReadOnly= true;
                this.col_Remark.IsEditingReadOnly = true;
            }
            else
            {
                this.col_Supplier.IsEditingReadOnly = false;
                this.col_Size.IsEditingReadOnly = false;
                this.col_Price.IsEditingReadOnly = false;
                this.col_OvenTest.IsEditingReadOnly = false;
                this.col_WashTest.IsEditingReadOnly = false;
                this.col_MockupTest.IsEditingReadOnly = false;
                this.col_PriceApv.IsEditingReadOnly = false;
                this.col_Remark.IsEditingReadOnly = false;
            }
        }

        /// <summary>
        /// OnDetailSelectCommandPrepare
        /// </summary>
        /// <param name="e">PrepareDetailSelectCommandEventArgs</param>
        /// <returns>DualResult</returns>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["ukey"].ToString();
            this.DetailSelectCommand = string.Format(
                @"
select A.*,S.ABB SuppName 
,[IsSintex] = iif(s.IsSintexSubcon =1,'Y','N')
from style_artwork_quot a WITH (NOLOCK)
INNER JOIN LocalSupp S WITH (NOLOCK) ON S.ID = A.LocalSuppId Where a.styleUkey = {0}
ORDER BY UKEY
", masterID);
            string countryID = MyUtility.GetValue.Lookup($@"select CountryID from MDivision where id = '{Env.User.Keyword}'");
            string sqlcmd = $@"
select
	 sa.[StyleUkey]
	,sa.[ArtworkTypeID]
	,sa.[Article]
	,sa.[PatternCode]
	,sa.[PatternDesc]
	,sa.[ArtworkID]
	,sa.[ArtworkName]
	,sa.[Qty]
	,sa.[Price]
	,[Cost] =Case when sa.[ArtworkTypeID] = 'Printing' and sa.PrintType <> 'C' then PrintingCost.Cost
	              when sa.[ArtworkTypeID] = 'EMBROIDERY' then [dbo].[GetEmboideryCost]('{countryID}',Season.SeasonSCIID, sa.Qty, 0)
				  else sa.Cost
				  end
	,sa.[Remark]
	,sa.[Ukey]
	,sa.[AddName]
	,sa.[AddDate]
	,sa.[EditName]
	,sa.[EditDate]
	,sa.[TMS]
	,sa.[TradeUkey]
	,sa.[SMNoticeID]
	,sa.[PatternVersion]
	,sa.[ActStitch]
	,sa.[PPU]
	,unit = B.ArtworkUnit
from style_artwork sa WITH (NOLOCK)
LEFT JOIN ArtworkType B WITH (NOLOCK) ON sa.ArtworkTypeID=B.ID
Left join Style on Style.Ukey =sa.StyleUkey
left join Season on Season.ID = Style.SeasonID and Season.BrandID = Style.BrandID
Outer Apply (	
    Select Top 1 Cost = Isnull(CostWithRatio, 0) * sa.Qty
	From dbo.GetPrintingCost(IsNull('{countryID}', ''), Isnull(Season.SeasonSCIID, '')
							, IsNull(sa.InkType, ''), IsNull(sa.Colors, '')
							, ISNull(sa.Length, 0), IsNull(sa.Width, 0), IsNull(sa.AntiMigration, 0)
							)
) as PrintingCost
 where sa.styleukey={masterID}
";
            DBProxy.Current.Select(null, sqlcmd, out this.style_artwork);

            return base.OnDetailSelectCommandPrepare(e);
        }

        /// <summary>
        /// OnDetailEntered
        /// </summary>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.listControlBindingSource1.DataSource = null;
            this.listControlBindingSource1.DataSource = this.style_artwork;
            this.gridArtworkType.AutoResizeColumns();
            this.GridArtworkTypeCellFormat();
        }

        private void Filter_detailgrid(int rowIndex)
        {
            if (rowIndex >= 0)
            {
                this.detailgridbs.Filter = "ukey=" + this.gridArtworkType.GetDataRow(rowIndex)["ukey"].ToString();
            }
        }

        private void GridArtworkTypeCellFormat()
        {
            if (!this.EditMode)
            {
                return;
            }

            foreach (DataGridViewRow item in this.gridArtworkType.Rows)
            {
                string unit = item.Cells["unit"].Value.ToString();

                if (unit == "STITCH")
                {
                    item.Cells["ActStitch"].ReadOnly = false;
                    item.Cells["ActStitch"].Style.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    item.Cells["ActStitch"].ReadOnly = true;
                    item.Cells["ActStitch"].Style.ForeColor = System.Drawing.Color.Black;
                }
            }
        }

        private void GridArtworkType_Sorted(object sender, EventArgs e)
        {
            this.GridArtworkTypeCellFormat();
        }

        private void GridArtworkType_RowSelecting(object sender, Ict.Win.UI.DataGridViewRowSelectingEventArgs e)
        {
            this.Filter_detailgrid(e.RowIndex);
        }
    }
}
