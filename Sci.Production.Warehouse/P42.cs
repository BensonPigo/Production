﻿using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P42 : Win.Tems.QueryForm
    {
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;

        /// <inheritdoc/>
        public P42(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.EditMode = true;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.checkEmptyEachCons.Checked = true;
            this.checkEmptyMtlETA.Checked = true;
            this.dateInline.Value = DateTime.Now;
            this.dateOffline.Value = DateTime.Now;
            Color backDefaultColor = this.gridCuttingTapeQuickAdjust.DefaultCellStyle.BackColor;

            this.gridCuttingTapeQuickAdjust.RowsAdded += (s, e) =>
            {
                if (e.RowIndex < 0)
                {
                    return;
                }

                #region 變色規則，若 EachConsApv != '' 則需變回預設的 Color
                int index = e.RowIndex;
                for (int i = 0; i < e.RowCount; i++)
                {
                    DataGridViewRow dr = this.gridCuttingTapeQuickAdjust.Rows[index];
                    dr.DefaultCellStyle.BackColor = MyUtility.Check.Empty(dr.Cells["EachConsApv"].Value) ? Color.FromArgb(255, 128, 192) : backDefaultColor;
                    index++;
                }
                #endregion
            };

            DataGridViewGeneratorTextColumnSettings ts1 = new DataGridViewGeneratorTextColumnSettings();
            ts1.CellMouseDoubleClick += (s, e) =>
            {
                if (e.RowIndex < 0)
                {
                    return;
                }

                DataRow ddr = this.gridCuttingTapeQuickAdjust.GetDataRow<DataRow>(e.RowIndex);
                DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
                string colnm = this.gridCuttingTapeQuickAdjust.Columns[e.ColumnIndex].DataPropertyName;
                string expression = colnm + " = '" + ddr[colnm].ToString().TrimEnd() + "'";
                DataRow[] drfound = dt.Select(expression);

                foreach (var item in drfound)
                {
                    item["selected"] = true;
                }
            };

            this.gridCuttingTapeQuickAdjust.CellValueChanged += (s, e) =>
            {
                if (this.gridCuttingTapeQuickAdjust.Columns[e.ColumnIndex].Name == this.col_chk.Name)
                {
                    this.Sum_checkedqty();
                }
            };
            DataGridViewGeneratorDateColumnSettings ts2 = new DataGridViewGeneratorDateColumnSettings();
            ts2.CellValidating += (s, e) =>
            {
                if (!MyUtility.Check.Empty(e.FormattedValue))
                {
                    DataRow dr = ((Win.UI.Grid)((DataGridViewColumn)s).DataGridView).GetDataRow(e.RowIndex);
                    if (MyUtility.Check.Empty(dr["tapeoffline"]))
                    {
                        return;
                    }

                    if (DateTime.Compare((DateTime)e.FormattedValue, (DateTime)dr["tapeoffline"]) > 0)
                    {
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox("Inline can't be late than Offline!!");
                    }
                }
            };

            DataGridViewGeneratorDateColumnSettings ts3 = new DataGridViewGeneratorDateColumnSettings();
            ts3.CellValidating += (s, e) =>
            {
                if (!MyUtility.Check.Empty(e.FormattedValue))
                {
                    DataRow dr = ((Win.UI.Grid)((DataGridViewColumn)s).DataGridView).GetDataRow(e.RowIndex);
                    if (MyUtility.Check.Empty(dr["tapeinline"]))
                    {
                        return;
                    }

                    if (DateTime.Compare((DateTime)e.FormattedValue, (DateTime)dr["tapeinline"]) < 0)
                    {
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox("Offline can't be earlier than inline!!");
                    }
                }
            };

            // 設定Grid1的顯示欄位
            this.gridCuttingTapeQuickAdjust.IsEditingReadOnly = false; // 必設定, 否則CheckBox會顯示圖示
            this.gridCuttingTapeQuickAdjust.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridCuttingTapeQuickAdjust)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
                .Text("FactoryID", header: "Factory", width: Widths.AnsiChars(5), settings: ts1, iseditingreadonly: true)
                .Text("POID", header: "SP#", width: Widths.AnsiChars(13), settings: ts1, iseditingreadonly: true)
                .Text("StyleID", header: "Style", width: Widths.AnsiChars(13), settings: ts1, iseditingreadonly: true)
                .Text("EachConsApv", header: "Each Cons.", width: Widths.AnsiChars(10), settings: ts1, iseditingreadonly: true)
                .Text("ETA", header: "Mtl. ETA", width: Widths.AnsiChars(10), settings: ts1, iseditingreadonly: true)
                .Text("fstSewinline", header: "1st. Sewing" + Environment.NewLine + "Date", width: Widths.AnsiChars(10), settings: ts1, iseditingreadonly: true)
                .Text("cutType", header: "Type of Cutting", width: Widths.AnsiChars(13), settings: ts1, iseditingreadonly: true)
                .Text("cutwidth", header: "Cut Width", width: Widths.AnsiChars(10), settings: ts1, iseditingreadonly: true)
                .Numeric("qty", header: "Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2, iseditingreadonly: true)
                .Text("stockunit", header: "Stock Unit", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Date("TapeInline", header: "Inline", width: Widths.AnsiChars(10), settings: ts2)
                .Date("TapeOffline", header: "Offline", width: Widths.AnsiChars(10), settings: ts3)
                .Numeric("ArrivedQty", header: "Arrived Qty", width: Widths.AnsiChars(2), iseditingreadonly: true)
                .Numeric("ReleasedQty", header: "Released Qty", width: Widths.AnsiChars(2), iseditingreadonly: true)
                .Text("seq1", header: "Seq1", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Text("seq2", header: "Seq2", width: Widths.AnsiChars(2), iseditingreadonly: true)
                .Date("fstSCIdlv", header: "SCI Dlv.", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Date("fstBuyerDlv", header: "Buyer Dlv.", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Refno", header: "Ref#", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("color", header: "Color Desc", width: Widths.AnsiChars(13), iseditingreadonly: true)
                ;
        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            string sewinline_b, sewinline_e, sciDelivery_b, sciDelivery_e, buyerdlv_b, buyerdlv_e, strFty;
            sewinline_b = null;
            sewinline_e = null;
            sciDelivery_b = null;
            sciDelivery_e = null;
            buyerdlv_b = null;
            buyerdlv_e = null;
            strFty = null;

            if (this.txtfactory.Text != null)
            {
                strFty = this.txtfactory.Text;
            }

            if (this.dateSewingInline.Value1 != null)
            {
                sewinline_b = this.dateSewingInline.Text1;
            }

            if (this.dateSewingInline.Value2 != null)
            {
                sewinline_e = this.dateSewingInline.Text2;
            }

            if (this.dateSCIDelivery.Value1 != null)
            {
                sciDelivery_b = this.dateSCIDelivery.Text1;
            }

            if (this.dateSCIDelivery.Value2 != null)
            {
                sciDelivery_e = this.dateSCIDelivery.Text2;
            }

            if (this.dateBuyerDelivery.Value1 != null)
            {
                buyerdlv_b = this.dateBuyerDelivery.Text1;
            }

            if (this.dateBuyerDelivery.Value2 != null)
            {
                buyerdlv_e = this.dateBuyerDelivery.Text2;
            }

            if ((sewinline_b == null && sewinline_e == null) &&
                (sciDelivery_b == null && sciDelivery_e == null) &&
                (buyerdlv_b == null && buyerdlv_e == null))
            {
                MyUtility.Msg.WarningBox("< Buyer Delivery > or < SCI Delivery > or < Fist Inline Date > can't be empty!!");
                this.dateSCIDelivery.Focus1();
                return;
            }

            string sqlcmd = $@"
select 0 as Selected,c.POID,EachConsApv = format(a.EachConsApv,'yyyy/MM/dd'),psd.FactoryID,c.MdivisionId
    ,ETA = format(
    (
        select max(FinalETA)
        from 
        (
            SELECT psd1.FinalETA
            FROM PO_Supp_Detail psd1 WITH (NOLOCK)
            inner join PO_Supp_Detail_Spec psdsC1 WITH (NOLOCK) on psdsC1.ID = psd.id and psdsC1.seq1 = psd.seq1 and psdsC1.seq2 = psd.seq2 and psdsC1.SpecColumnID = 'Color'
            WHERE psd1.ID = psd.ID 
            AND psd1.SCIRefno = psd.SCIRefno
            AND isnull(psdsC1.SpecValue, '') = isnull(psdsC.SpecValue, '')

            UNION ALL
            SELECT psd2.FinalETA
            FROM PO_Supp_Detail psd1 WITH (NOLOCK)
            inner join PO_Supp_Detail psd2 WITH (NOLOCK) on psd1.StockPOID = psd2.ID and psd1.StockSeq1 = psd2.SEQ1 and psd1.StockSeq2 = psd2.SEQ2
            inner join PO_Supp_Detail_Spec psdsC1 WITH (NOLOCK) on psdsC1.ID = psd.id and psdsC1.seq1 = psd.seq1 and psdsC1.seq2 = psd.seq2 and psdsC1.SpecColumnID = 'Color'
            WHERE psd1.ID = psd.ID
            AND psd1.SCIRefno = psd.SCIRefno
            AND isnull(psdsC1.SpecValue, '') = isnull(psdsC.SpecValue, '')
        ) tmp
    ),'yyyy/MM/dd')
	,format(MIN(a.SewInLine),'yyyy/MM/dd') as FstSewinline
    ,psd.Special AS cutType
	,qty = round(dbo.GetUnitQty(psd.POUnit, stockunit.value, psd.Qty), iif(stockunit.value!='',(select unit.Round from unit WITH (NOLOCK) where id = stockunit.value),2))
	,stockunit = stockunit.value
	,isnull(psdsS.SpecValue, '') cutwidth
    ,psd.Refno,psd.SEQ1,psd.SEQ2
    ,c.TapeInline,c.TapeOffline
	,min(a.SciDelivery) FstSCIdlv
	,min(a.BuyerDelivery) FstBuyerDlv
	,(select color.Name from color WITH (NOLOCK) where color.id = isnull(psdsC.SpecValue, '') and color.BrandId = a.brandid ) as color
    ,a.StyleID
	,ArrivedQty = m.InQty
	,ReleasedQty = m.OutQty
from orders a WITH (NOLOCK) 
inner join Po_supp_detail psd WITH (NOLOCK) on a.poid = psd.id
inner join dbo.cuttingtape_detail c WITH (NOLOCK) on c.mdivisionid = '{Env.User.Keyword}' and c.poid = psd.id and c.seq1 = psd.seq1 and c.seq2 = psd.seq2
left join dbo.MDivisionPoDetail m WITH (NOLOCK) on m.POID = psd.ID and m.seq1 = psd.SEQ1 and m.Seq2 = psd.Seq2
left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
left join PO_Supp_Detail_Spec psdsS WITH (NOLOCK) on psdsS.ID = psd.id and psdsS.seq1 = psd.seq1 and psdsS.seq2 = psd.seq2 and psdsS.SpecColumnID = 'Size'
outer apply( select value =  iif(psd.stockunit = '',dbo.GetStockUnitBySPSeq( psd.id,psd.SEQ1,psd.SEQ2),psd.stockunit) ) stockunit
WHERE A.IsForecast = 0 AND A.Junk = 0 AND A.LocalOrder = 0 AND a.category not in('M','T')
AND psd.SEQ1 = 'A1'
AND ((psd.Special NOT LIKE ('%DIE CUT%')) and psd.Special is not null)
";

            if (!MyUtility.Check.Empty(strFty))
            {
                sqlcmd += $@" and psd.FactoryID ='{strFty}' ";
            }

            if (!MyUtility.Check.Empty(sciDelivery_b))
            {
                sqlcmd += string.Format(@" and a.SciDelivery between '{0}' and '{1}'", sciDelivery_b, sciDelivery_e);
            }

            if (!string.IsNullOrWhiteSpace(sewinline_b))
            {
                sqlcmd += string.Format(@" and a.sewinline between '{0}' and '{1}'", sewinline_b, sewinline_e);
            }

            if (!string.IsNullOrWhiteSpace(buyerdlv_b))
            {
                sqlcmd += string.Format(@" and a.BuyerDelivery between '{0}' and '{1}'", buyerdlv_b, buyerdlv_e);
            }

            sqlcmd += @"
GROUP BY c.MdivisionId,psd.FactoryID,c.POID,a.EachConsApv,psd.Special,psd.Qty,psdsS.SpecValue,psd.Refno,psd.SEQ1,psd.SEQ2,c.TapeInline,c.TapeOffline,psd.ID,psdsC.SpecValue,psd.SCIRefno,a.brandid,psd.POUnit, stockunit.value,a.StyleID
,m.InQty
,m.OutQty

ORDER BY psd.FactoryID,c.POID
";
            DualResult result;
            if (result = DBProxy.Current.Select(null, sqlcmd, out DataTable dtData))
            {
                if (dtData.Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!!");
                }

                this.listControlBindingSource1.DataSource = dtData;
                this.CheckBoxs_Status();
            }
            else
            {
                this.ShowErr(sqlcmd, result);
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
            if (dt == null || dt.Rows.Count == 0)
            {
                return;
            }

            DataRow[] find;
            find = dt.Select("Selected = 1");
            if (find.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select rows first!", "Warnning");
                return;
            }

            DialogResult dResult = MyUtility.Msg.QuestionBox("Are you sure to save it?", "Question", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);
            if (dResult.ToString().ToUpper() == "NO")
            {
                return;
            }

            string sqlcmd = string.Empty;

            foreach (DataRow item in find)
            {
                sqlcmd += @"update cuttingtape_detail ";
                if (MyUtility.Check.Empty(item["TapeInline"]))
                {
                    sqlcmd += "set tapeinline = null ";
                }
                else
                {
                    sqlcmd += string.Format(@"set tapeinline ='{0}'", ((DateTime)item["tapeinline"]).ToShortDateString());
                }

                if (MyUtility.Check.Empty(item["TapeOffline"]))
                {
                    sqlcmd += ",tapeoffline = null ";
                }
                else
                {
                    sqlcmd += string.Format(@",tapeoffline = '{0}'", ((DateTime)item["TapeOffline"]).ToShortDateString());
                }

                sqlcmd += string.Format(
                    @" where poid ='{0}' and seq1 = '{1}' and seq2 = '{2}' and mdivisionid='{3}';",
                    item["POID"],
                    item["seq1"],
                    item["seq2"],
                    item["mdivisionid"]);

                // 回寫表頭TapeFirstInline,EditName,EditDate欄位
                sqlcmd += string.Format(
                    @"  update a set EditName='{0}',EditDate=GETDATE(),TapeFirstInline=b.TapeFirstInline
                                            from dbo.cuttingtape as a
                                            inner join
                                            (
                                                select MIN(CuttingTape_Detail.TapeInline) as TapeFirstInline,cuttingtape.POID,cuttingtape.MDivisionID
                                                from dbo.cuttingtape 
                                                inner join dbo.CuttingTape_Detail on cuttingtape.POID=CuttingTape_Detail.POID --and cuttingtape.MDivisionID=CuttingTape_Detail.MDivisionID
                                                where cuttingtape.poid ='{1}' and cuttingtape.mdivisionid='{2}'
                                                Group by TapeFirstInline,cuttingtape.POID,cuttingtape.MDivisionID
                                            ) as b on a.MDivisionID=b.MDivisionID and a.POID=b.POID
                                            where a.poid ='{1}' and a.mdivisionid='{2}'",
                    Env.User.UserID,
                    item["POID"],
                    item["mdivisionid"]);
            }

            DualResult result = DBProxy.Current.Execute(null, sqlcmd);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }
            else
            {
                MyUtility.Msg.InfoBox("Save successful!!");
            }
        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {
            this.listControlBindingSource1.EndEdit();
            DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
            if (dt == null || dt.Rows.Count == 0)
            {
                return;
            }

            DataRow[] drfound = dt.Select("selected = 1");

            foreach (var item in drfound)
            {
                if (this.dateInline.Value != null)
                {
                    item["tapeinline"] = this.dateInline.Value;
                }
                else
                {
                    item["tapeinline"] = DBNull.Value;
                }
            }
        }

        private void PictureBox2_Click(object sender, EventArgs e)
        {
            this.listControlBindingSource1.EndEdit();
            DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
            if (dt == null || dt.Rows.Count == 0)
            {
                return;
            }

            DataRow[] drfound = dt.Select("selected = 1");
            foreach (var item in drfound)
            {
                if (this.dateOffline.Value != null)
                {
                    item["tapeoffline"] = this.dateOffline.Value;
                }
                else
                {
                    item["tapeoffline"] = DBNull.Value;
                }
            }
        }

        private void BtnFind_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.listControlBindingSource1.DataSource))
            {
                return;
            }

            int index = this.listControlBindingSource1.Find("poid", this.txtLocateForSP.Text.TrimEnd());
            if (index == -1)
            {
                MyUtility.Msg.WarningBox("Data was not found!!");
            }
            else
            {
                this.listControlBindingSource1.Position = index;
            }
        }

        private void Sum_checkedqty()
        {
            this.listControlBindingSource1.EndEdit();
            DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
            object localPrice = dt.Compute("Sum(qty)", "selected = 1");
            this.displayCheckedQty.Value = MyUtility.Convert.GetDecimal(localPrice).ToString("F2");
        }

        private void CheckEmptyEachCons_CheckedChanged(object sender, EventArgs e)
        {
            this.CheckBoxs_Status();
        }

        private void CheckEmptyMtlETA_CheckedChanged(object sender, EventArgs e)
        {
            this.CheckBoxs_Status();
        }

        private void CheckBoxs_Status()
        {
            if (this.listControlBindingSource1.DataSource == null)
            {
                return;
            }

            string formatStr = string.Empty;
            if (this.checkEmptyEachCons.Checked)
            {
                formatStr += "EachConsApv is not null ";
            }

            if (this.checkEmptyMtlETA.Checked)
            {
                formatStr += formatStr.EqualString(string.Empty) ? "ETA is not null" : "and ETA is not null";
            }

            this.listControlBindingSource1.Filter = string.Format(formatStr);
        }
    }
}
