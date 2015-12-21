using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.PublicPrg;
using System.Linq;

namespace Sci.Production.Planning
{
    public partial class B03 : Sci.Win.Tems.Input6
    {
        DataTable style_artwork;
        public B03(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }

        //
        protected override bool ClickCopy()
        {
            if (MyUtility.Check.Empty(CurrentDetailData))
            {
                MyUtility.Msg.WarningBox("No Quot. data can't be copied, Please add quot. detail first!");
                return true;
            }
            DataRow dr = grid1.GetDataRow<DataRow>(grid1.GetSelectedRowIndex());
            var frm = new Sci.Production.Planning.B03_Copy(dr);
            frm.ShowDialog(this);
            this.RenewData();
            return true;
        }

        // save前檢查 & 取id
        protected override bool ClickSaveBefore()
        {
            detailgridbs.Filter = "";
            StringBuilder warningmsg = new StringBuilder();
            foreach (DataRow row in DetailDatas)
            {
                if (MyUtility.Check.Empty(row["localsuppid"]))
                {
                    warningmsg.Append(string.Format(@"Ukey: {0}  , Supplier can't be empty", row["ukey"])
                        + Environment.NewLine);
                }
            }
            if (!MyUtility.Check.Empty(warningmsg.ToString()))
            {
                MyUtility.Msg.WarningBox(warningmsg.ToString());
                detailgridbs.Filter = "ukey=" + grid1.GetDataRow(grid1.GetSelectedRowIndex())["ukey"].ToString();
                return false;
            }
            return base.ClickSaveBefore();
        }

        protected override void ClickSaveAfter()
        {
            filter_detailgrid();
            base.ClickSaveAfter();
            
        }
        // detail 新增時設定預設值
        protected override void OnDetailGridInsert(int index = -1)
        {
            detailgridbs.Filter = "";
            base.OnDetailGridInsert(index);
            CurrentDetailData["ukey"] = grid1.GetDataRow(grid1.GetSelectedRowIndex())["ukey"].ToString();
            detailgridbs.Filter = "ukey=" + grid1.GetDataRow(grid1.GetSelectedRowIndex())["ukey"].ToString();
        }

        // Detail Grid 設定
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

                    sqlcmd = "select id,abb,currencyid from localsupp where junk = 0 and IsFactory = 0 order by ID";
                    item = new Sci.Win.Tools.SelectItem(sqlcmd, "10,30,10", null);

                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    var x = item.GetSelecteds();
                    CurrentDetailData["localsuppid"] = x[0]["id"].ToString();
                    CurrentDetailData["suppname"] = x[0]["abb"].ToString();
                    CurrentDetailData["currencyid"] = x[0]["currencyid"].ToString();
                }
            };
            #endregion
            #region -- Supplier Valid --
            ts4.CellValidating += (s, e) =>
            {
                if (this.EditMode && !MyUtility.Check.Empty(e.FormattedValue))
                {
                    DataRow find;
                    if (MyUtility.Check.Seek(string.Format("Select * from localsupp where isfactory=0 and junk=0 and id='{0}'", e.FormattedValue), out find))
                    {
                        CurrentDetailData["localsuppid"] = find["id"].ToString();
                        CurrentDetailData["suppname"] = find["abb"].ToString();
                        CurrentDetailData["currencyid"] = find["currencyid"].ToString();
                    }
                    else
                    {
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox("Supplier is not found!", "Warning");
                        return;
                    }
                }
            };
            #endregion

            Ict.Win.UI.DataGridViewComboBoxColumn col_PriceApv;
            Dictionary<String, String> comboBox1_RowSource = new Dictionary<string, string>();
            comboBox1_RowSource.Add("Y", "Y");
            comboBox1_RowSource.Add("N", "N");
            comboBox1_RowSource.Add("", "N/A");

            #region 欄位設定
            Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("Ukey", header: "Ukey", width: Widths.AnsiChars(6), iseditingreadonly: true)  //0
            .Text("localsuppid", header: "Supplier", width: Widths.AnsiChars(6), settings: ts4)  //1
            .Text("suppname", header: "Name", width: Widths.AnsiChars(10), iseditingreadonly: true)  //2
            .Text("currencyid", header: "Currency", width: Widths.AnsiChars(10), iseditingreadonly: true)  //3
            .Numeric("price", header: "Price", width: Widths.AnsiChars(8), decimal_places: 4, integer_places: 10)  //4
            .Date("oven", header: "Oven Test", width: Widths.AnsiChars(10))    //5
            .Date("wash", header: "Wash Test", width: Widths.AnsiChars(10))    //6
            .Date("mockup", header: "Mockup Test", width: Widths.AnsiChars(10))    //7
            .ComboBox("priceApv", header: "Price Approve").Get(out col_PriceApv)    //8
            ;     //
            #endregion 欄位設定

            col_PriceApv.DataSource = new BindingSource(comboBox1_RowSource, null);
            col_PriceApv.ValueMember = "Key";
            col_PriceApv.DisplayMember = "Value";

            #region 欄位設定
            Helper.Controls.Grid.Generator(this.grid1)
            .Text("Ukey", header: "Ukey", width: Widths.AnsiChars(6), iseditingreadonly: true)  //0
            .Text("artworktypeid", header: "Artwork Type", width: Widths.AnsiChars(16), iseditingreadonly: true)  //0
            .Text("article", header: "Article", width: Widths.AnsiChars(10), iseditingreadonly: true)  //1
            .Text("artworkid", header: "Artwork", width: Widths.AnsiChars(10))  //2
            .Text("artworkname", header: "Artwork Name", width: Widths.AnsiChars(20))  //3
            .Text("patterncode", header: "Cut Part", width: Widths.AnsiChars(6))  //4
            .Text("patterndesc", header: "Cut Part Name", width: Widths.AnsiChars(20))  //5
            .Numeric("tms", header: "TMS", width: Widths.AnsiChars(8), integer_places: 10)  //6
            .Numeric("qty", header: "QTY", width: Widths.AnsiChars(8), integer_places: 10)  //7
            .Text("unit", header: "Unit", width: Widths.AnsiChars(10))  //8
            .Numeric("cost", header: "Cost", width: Widths.AnsiChars(8), decimal_places: 4, integer_places: 10)  //9
            .Text("remark", header: "Remark", width: Widths.AnsiChars(20))  //10
            ;     //

            #endregion 欄位設定

            this.grid1.DataSource = listControlBindingSource1;
        }

        //寫明細撈出的sql command
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["ukey"].ToString();
            this.DetailSelectCommand = string.Format(@"select A.*,S.ABB SuppName from style_artwork_quot a 
INNER JOIN LocalSupp S ON S.ID = A.LocalSuppId Where a.styleUkey = {0}", masterID);

            DBProxy.Current.Select(null, string.Format(@"select * from style_artwork t where styleukey={0}", masterID), out style_artwork);

            return base.OnDetailSelectCommandPrepare(e);

        }

        //refresh
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            listControlBindingSource1.DataSource = style_artwork;
        }

        private void grid1_SelectionChanged(object sender, EventArgs e)
        {
            filter_detailgrid();
        }

        private void filter_detailgrid()
        {
            if (grid1.GetSelectedRowIndex() >= 0)
            {
                detailgridbs.Filter = "ukey=" + grid1.GetDataRow(grid1.GetSelectedRowIndex())["ukey"].ToString();
                //((DataTable)detailgridbs.DataSource).DefaultView.RowFilter = "ukey=" + grid1.GetDataRow(grid1.GetSelectedRowIndex())["ukey"].ToString();
            }
        }

    }
}
