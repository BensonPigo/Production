using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;

namespace Sci.Production.Warehouse
{
    public partial class P42 : Win.Tems.QueryForm
    {
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;

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
                 .Text("EachConsApv", header: "Each Cons.", width: Widths.AnsiChars(10), settings: ts1, iseditingreadonly: true)
                 .Text("ETA", header: "Mtl. ETA", width: Widths.AnsiChars(10), settings: ts1, iseditingreadonly: true)
                 .Text("fstSewinline", header: "1st. Sewing" + Environment.NewLine + "Date", width: Widths.AnsiChars(10), settings: ts1, iseditingreadonly: true)
                 .Text("cutType", header: "Type of Cutting", width: Widths.AnsiChars(13), settings: ts1, iseditingreadonly: true)
                 .Text("cutwidth", header: "Cut Width", width: Widths.AnsiChars(10), settings: ts1, iseditingreadonly: true)
                 .Numeric("qty", header: "Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2, iseditingreadonly: true)
                  .Text("stockunit", header: "Stock Unit", width: Widths.AnsiChars(8), iseditingreadonly: true)
                  .Date("TapeInline", header: "Inline", width: Widths.AnsiChars(10), settings: ts2)
                  .Date("TapeOffline", header: "Offline", width: Widths.AnsiChars(10), settings: ts3)
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
            DataTable dtData;
            string sewinline_b, sewinline_e, sciDelivery_b, sciDelivery_e, buyerdlv_b, buyerdlv_e;
            sewinline_b = null;
            sewinline_e = null;
            sciDelivery_b = null;
            sciDelivery_e = null;
            buyerdlv_b = null;
            buyerdlv_e = null;
            bool eachchk, mtletachk;
            eachchk = this.checkEmptyEachCons.Checked;
            mtletachk = this.checkEmptyMtlETA.Checked;

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

            string sqlcmd
                = string.Format(
                    @"
select 0 as Selected,c.POID,EachConsApv = format(a.EachConsApv,'yyyy/MM/dd'),b.FactoryID,c.MdivisionId
,format((select max(FinalETA) from 
	(select po_supp_detail.FinalETA from PO_Supp_Detail WITH (NOLOCK) 
		WHERE PO_Supp_Detail.ID = B.ID 
		AND PO_Supp_Detail.SCIRefno = B.SCIRefno AND PO_Supp_Detail.ColorID = b.ColorID 
	union all
	select b1.FinalETA from PO_Supp_Detail a1 WITH (NOLOCK) , PO_Supp_Detail b1 WITH (NOLOCK) 
		where a1.ID = B.ID AND a1.SCIRefno = B.SCIRefno AND a1.ColorID = b.ColorID
		AND a1.StockPOID = b1.ID and a1.Stockseq1 = b1.SEQ1 and a1.StockSeq2 = b1.SEQ2
	) tmp),'yyyy/MM/dd') as ETA
	,format(MIN(a.SewInLine),'yyyy/MM/dd') as FstSewinline
    ,b.Special AS cutType
	,qty = round(dbo.GetUnitQty(b.POUnit, stockunit.value, b.Qty), iif(stockunit.value!='',(select unit.Round from unit WITH (NOLOCK) where id = stockunit.value),2))
	,stockunit = stockunit.value
	,b.SizeSpec cutwidth,B.Refno,B.SEQ1,B.SEQ2
    ,c.TapeInline,c.TapeOffline
	,min(a.SciDelivery) FstSCIdlv
	,min(a.BuyerDelivery) FstBuyerDlv
	,(select color.Name from color WITH (NOLOCK) where color.id = b.ColorID and color.BrandId = a.brandid ) as color
from orders a WITH (NOLOCK) inner join Po_supp_detail b WITH (NOLOCK) on a.poid = b.id
inner join dbo.cuttingtape_detail c WITH (NOLOCK) on c.mdivisionid = '{0}' and c.poid = b.id and c.seq1 = b.seq1 and c.seq2 = b.seq2
outer apply( select value =  iif(b.stockunit = '',dbo.GetStockUnitBySPSeq( b.id,B.SEQ1,B.SEQ2),b.stockunit) ) stockunit
WHERE A.IsForecast = 0 AND A.Junk = 0 AND A.LocalOrder = 0 AND a.category not in('M','T')
AND B.SEQ1 = 'A1'
AND ((B.Special NOT LIKE ('%DIE CUT%')) and B.Special is not null)", Env.User.Keyword);
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

            sqlcmd += "GROUP BY c.MdivisionId,b.FactoryID,c.POID,a.EachConsApv,B.Special,B.Qty,B.SizeSpec,B.Refno,B.SEQ1,B.SEQ2,c.TapeInline,c.TapeOffline,B.ID,B.ColorID,b.SCIRefno,a.brandid,b.POUnit, stockunit.value";

// 20161215 CheckBox 選項用 checkBoxs_Status() 取代
//            if (eachchk && mtletachk) sqlcmd += @" having EachConsApv is not null and (SELECT MAX(FinalETA) FROM
// (SELECT PO_SUPP_DETAIL.FinalETA FROM PO_Supp_Detail
// WHERE PO_Supp_Detail.ID = B.ID
// AND PO_Supp_Detail.SCIRefno = B.SCIRefno AND PO_Supp_Detail.ColorID = b.ColorID
// UNION ALL
// SELECT B1.FinalETA FROM PO_Supp_Detail a1, PO_Supp_Detail b1
// WHERE a1.ID = B.ID AND a1.SCIRefno = B.SCIRefno AND a1.ColorID = b.ColorID
// AND a1.StockPOID = b1.ID and a1.Stockseq1 = b1.SEQ1 and a1.Stockseq2 = b1.SEQ2
// ) tmp) is not null";
//            else
//            {
//                if (eachchk) sqlcmd += " having EachConsApv is not null";
//                if (mtletachk) sqlcmd += @" having (SELECT MAX(FinalETA) FROM
// (SELECT PO_SUPP_DETAIL.FinalETA FROM PO_Supp_Detail
// WHERE PO_Supp_Detail.ID = B.ID
// AND PO_Supp_Detail.SCIRefno = B.SCIRefno AND PO_Supp_Detail.ColorID = b.ColorID
// UNION ALL
// SELECT B1.FinalETA FROM PO_Supp_Detail a1, PO_Supp_Detail b1
// WHERE a1.ID = B.ID AND a1.SCIRefno = B.SCIRefno AND a1.ColorID = b.ColorID
// AND a1.StockPOID = b1.ID and a1.Stockseq1 = b1.SEQ1 and a1.Stockseq2 = b1.SEQ2
// ) tmp) is not null";
//            }
            sqlcmd += @" ORDER BY b.FactoryID,c.POID";
            DualResult result;
            if (result = DBProxy.Current.Select(null, sqlcmd, out dtData))
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
            DualResult result;
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

            if (!(result = DBProxy.Current.Execute(null, sqlcmd)))
            {
                MyUtility.Msg.WarningBox("Save failed, Pleaes re-try");
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
