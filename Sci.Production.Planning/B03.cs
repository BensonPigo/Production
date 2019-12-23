﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.Planning
{
    /// <summary>
    /// B03
    /// </summary>
    public partial class B03 : Sci.Win.Tems.Input6
    {
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
            var frm = new Sci.Production.Planning.B03_Copy(dr);
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
        /// ClickSaveAfter
        /// </summary>
        protected override void ClickSaveAfter()
        {
            this.Filter_detailgrid();
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
            Ict.Win.DataGridViewGeneratorTextColumnSettings ts4 = new DataGridViewGeneratorTextColumnSettings();
            ts4.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    Sci.Win.Tools.SelectItem item;
                    string sqlcmd;

                    //sqlcmd = "select id,abb,currencyid from localsupp WITH (NOLOCK) where junk = 0 and IsFactory = 0 order by ID";
                    sqlcmd = @"
select l.id ,l.abb ,l.currencyid 
from LocalSupp l WITH (NOLOCK) 
left join LocalSupp_Bank lb WITH (NOLOCK)  ON l.id=lb.id 
WHERE l.Junk=0 and lb.Status= 'Confirmed'  AND l.IsFactory = 0
order by ID
";
                    item = new Sci.Win.Tools.SelectItem(sqlcmd, "10,15,5", null);
                    item.Size = new System.Drawing.Size(480, 500);
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    var x = item.GetSelecteds();

                    this.CurrentDetailData["localsuppid"] = x[0]["id"].ToString();
                    this.CurrentDetailData["suppname"] = x[0]["abb"].ToString();
                    this.CurrentDetailData["currencyid"] = x[0]["currencyid"].ToString();
                }
            };
            #endregion
            #region -- Supplier Valid --
            ts4.CellValidating += (s, e) =>
            {
                if (this.EditMode && !MyUtility.Check.Empty(e.FormattedValue))
                {
                    DataRow find;
                    if (MyUtility.Check.Seek(string.Format("Select * from localsupp WITH (NOLOCK) where isfactory=0 and junk=0 and id='{0}'", e.FormattedValue), out find))
                    {
                        this.CurrentDetailData["localsuppid"] = find["id"].ToString();
                        this.CurrentDetailData["suppname"] = find["abb"].ToString();
                        this.CurrentDetailData["currencyid"] = find["currencyid"].ToString();
                        return;
                    }
                    else
                    {
                        this.CurrentDetailData["localsuppid"] = string.Empty;
                        this.CurrentDetailData["suppname"] = string.Empty;
                        this.CurrentDetailData["currencyid"] = string.Empty;
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
                }
            };
            #endregion

            Ict.Win.UI.DataGridViewComboBoxColumn col_PriceApv;
            Dictionary<string, string> comboBox1_RowSource = new Dictionary<string, string>();
            comboBox1_RowSource.Add("Y", "Y");
            comboBox1_RowSource.Add("N", "N");
            comboBox1_RowSource.Add(string.Empty, "N/A");

            #region 欄位設定
            this.Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("localsuppid", header: "Supplier", width: Widths.AnsiChars(6), settings: ts4)
            .Text("suppname", header: "Name", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("currencyid", header: "Currency", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Numeric("price", header: "Price", width: Widths.AnsiChars(8), decimal_places: 4, integer_places: 10)
            .Date("oven", header: "Oven Test", width: Widths.AnsiChars(10))
            .Date("wash", header: "Wash Test", width: Widths.AnsiChars(10))
            .Date("mockup", header: "Mockup Test", width: Widths.AnsiChars(10))
            .ComboBox("priceApv", header: "Price Approve").Get(out col_PriceApv)
            ;
            #endregion 欄位設定

            col_PriceApv.DataSource = new BindingSource(comboBox1_RowSource, null);
            col_PriceApv.ValueMember = "Key";
            col_PriceApv.DisplayMember = "Value";

            #region 欄位設定
            this.Helper.Controls.Grid.Generator(this.gridArtworkType)
            .Text("artworktypeid", header: "Artwork Type", width: Widths.AnsiChars(16), iseditingreadonly: true)
            .Text("article", header: "Article", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("artworkid", header: "Artwork", width: Widths.AnsiChars(10))
            .Text("artworkname", header: "Artwork Name", width: Widths.AnsiChars(20))
            .Text("patterncode", header: "Cut Part", width: Widths.AnsiChars(6))
            .Text("patterndesc", header: "Cut Part Name", width: Widths.AnsiChars(20))
            .Numeric("tms", header: "TMS", width: Widths.AnsiChars(8), integer_places: 10)
            .Numeric("qty", header: "QTY", width: Widths.AnsiChars(8), integer_places: 10)
            .Text("unit", header: "Unit", width: Widths.AnsiChars(10))
            .Numeric("cost", header: "Cost", width: Widths.AnsiChars(8), decimal_places: 4, integer_places: 10)
            .Text("remark", header: "Remark", width: Widths.AnsiChars(20))
            ;

            #endregion 欄位設定

            this.gridArtworkType.DataSource = this.listControlBindingSource1;
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
                @"select A.*,S.ABB SuppName from style_artwork_quot a WITH (NOLOCK)
INNER JOIN LocalSupp S WITH (NOLOCK) ON S.ID = A.LocalSuppId Where a.styleUkey = {0}
ORDER BY UKEY
", masterID);

            string sqlcmd = string.Format(
                @"select t.*,B.ArtworkUnit AS unit from style_artwork t WITH (NOLOCK)
LEFT JOIN ArtworkType B WITH (NOLOCK) ON t.ArtworkTypeID=B.ID where styleukey={0}", masterID);
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
        }

        private void Grid1_SelectionChanged(object sender, EventArgs e)
        {
            this.Filter_detailgrid();
        }

        private void Filter_detailgrid()
        {
            if (this.gridArtworkType.GetSelectedRowIndex() >= 0)
            {
                this.detailgridbs.Filter = "ukey=" + this.gridArtworkType.GetDataRow(this.gridArtworkType.GetSelectedRowIndex())["ukey"].ToString();
            }
        }
    }
}
