﻿using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.PublicPrg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P19_Import : Win.Subs.Base
    {
        private DataRow dr_master;  // 抓主頁的表頭資料用
        private DataTable dt_detail;    // 將匯入資料寫入主頁的明細用
        private Dictionary<string, string> di_stocktype = new Dictionary<string, string>();
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        private DataTable dtImportData;

        /// <inheritdoc/>
        public P19_Import(DataRow master, DataTable detail)
        {
            this.InitializeComponent();
            this.di_stocktype.Add("B", "Bulk");
            this.di_stocktype.Add("I", "Inventory");
            this.dr_master = master;
            this.dt_detail = detail;
        }

        // Find Now Button
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        private void BtnFindNow_Click(object sender, EventArgs e)
        {
            if ((this.comboStockType.SelectedIndex < 0 || MyUtility.Check.Empty(this.txtSPNo.Text)) &&
                (this.comboStockType.SelectedIndex < 0 || MyUtility.Check.Empty(this.txtWKno.Text)))
            {
                MyUtility.Msg.WarningBox("<SP#> and <WK#> cannot all be empty.");
                return;
            }

            StringBuilder sbSQLCmd = new StringBuilder();
            string stocktype = this.comboStockType.SelectedValue.ToString();
            string sp = this.txtSPNo.Text;

            #region -- sql parameters declare --

            System.Data.SqlClient.SqlParameter sp_sp = new System.Data.SqlClient.SqlParameter();
            sp_sp.ParameterName = "@sp";
            sp_sp.Value = sp;

            System.Data.SqlClient.SqlParameter sp_seq1 = new System.Data.SqlClient.SqlParameter();
            sp_seq1.ParameterName = "@seq1";

            System.Data.SqlClient.SqlParameter sp_seq2 = new System.Data.SqlClient.SqlParameter();
            sp_seq2.ParameterName = "@seq2";

            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
            cmds.Add(sp_sp);

            #endregion

            // 建立可以符合回傳的Cursor
            sbSQLCmd.Append($@"
select  0 as selected 
        , '' id
		, [ExportID] = Stuff((select distinct concat(',', r.ExportId)				
                from Receiving r WITH (NOLOCK)
                inner join Receiving_Detail rd WITH (NOLOCK) on r.id = rd.id
				where rd.PoId = FI.POID and rd.Seq1 = FI.SEQ1 and rd.Seq2 = FI.SEQ2 and rd.Roll = FI.Roll and rd.Dyelot = FI.Dyelot
				FOR XML PATH('')),1,1,'')
        , FI.PoId
        , FI.Seq1
        , FI.Seq2
        , concat(Ltrim(Rtrim(FI.seq1)), ' ', FI.Seq2) as seq
        , [FabricType]= (SELECT Name FROM DropDownList WHERE Type='FabricType_Condition' AND ID = PSD.FabricType )
        , [stockunit] = dbo.GetStockUnitBySPSeq(FI.Poid,FI.Seq1,FI.Seq2)		
        , dbo.getmtldesc(FI.POID,FI.seq1,FI.seq2,2,0) Description
        , FI.Roll
        , FI.Dyelot
        , 0.00 as Qty
        , FI.inqty - FI.outqty + FI.adjustqty - FI.ReturnQty as Balance
        , FI.StockType
        , FI.ukey as ftyinventoryukey
        , dbo.Getlocation(FI.ukey) location
        , FI.inqty - FI.outqty + FI.adjustqty - FI.ReturnQty as stockqty
		, [ToPOID]=''
		, [ToSeq]=''
		, [ToSeq1]=''
		, [ToSeq2]=''
		, PSD.Refno
		, Color = IIF(Fabric.MtlTypeID = 'EMB THREAD' OR Fabric.MtlTypeID = 'SP THREAD' OR Fabric.MtlTypeID = 'THREAD' 
												,IIF( PSD.SuppColor = '' or PSD.SuppColor is null,dbo.GetColorMultipleID(o.BrandID, isnull(psdsC.SpecValue, '')),PSD.SuppColor)
												,dbo.GetColorMultipleID(o.BrandID, isnull(psdsC.SpecValue, ''))
											)
		, SizeSpec= isnull(psdsS.SpecValue, '')
        , FI.lock
        , FI.Tone
FROM FtyInventory FI WITH (NOLOCK)
LEFT JOIN View_WH_Orders O WITH (NOLOCK) ON O.ID = FI.POID
LEFT JOIN Factory F WITH (NOLOCK) ON F.ID = O.FactoryID
LEFT JOIN PO_Supp_Detail PSD WITH (NOLOCK) ON PSD.ID=FI.POID AND PSD.SEQ1 = FI.SEQ1 AND PSD.SEQ2=FI.SEQ2
left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
left join PO_Supp_Detail_Spec psdsS WITH (NOLOCK) on psdsS.ID = psd.id and psdsS.seq1 = psd.seq1 and psdsS.seq2 = psd.seq2 and psdsS.SpecColumnID = 'Size'
left join Fabric on Fabric.SCIRefno = psd.SCIRefno
Where ( F.MDivisionID = '{Env.User.Keyword}' OR o.MDivisionID= '{Env.User.Keyword}' )
        and FI.inqty - FI.outqty + FI.adjustqty - FI.ReturnQty > 0         
        and FI.stocktype = '{stocktype}'
");

            if (!MyUtility.Check.Empty(sp))
            {
                sbSQLCmd.Append(@"
        and FI.Poid = @sp ");
            }

            sp_seq1.Value = this.txtSeq1.Seq1;
            sp_seq2.Value = this.txtSeq1.Seq2;
            cmds.Add(sp_seq1);
            cmds.Add(sp_seq2);
            if (!this.txtSeq1.CheckSeq1Empty() && this.txtSeq1.CheckSeq2Empty())
            {
                sbSQLCmd.Append(@"
        and FI.seq1 = @seq1 ");
            }
            else if (!this.txtSeq1.CheckEmpty(showErrMsg: false))
            {
                sbSQLCmd.Append(@" 
        and FI.seq1 = @seq1 and FI.seq2 = @seq2");
                sp_seq1.Value = this.txtSeq1.Seq1;
                sp_seq2.Value = this.txtSeq1.Seq2;
            }

            if (!MyUtility.Check.Empty(this.txtWKno.Text))
            {
                sbSQLCmd.Append($@" 
AND exists (select 1 
            from Receiving r WITH (NOLOCK)
            inner join Receiving_Detail rd WITH (NOLOCK) on r.id = rd.id
			where rd.PoId = FI.POID and rd.Seq1 = FI.SEQ1 and rd.Seq2 = FI.SEQ2 and rd.Roll = FI.Roll and rd.Dyelot = FI.Dyelot
            and r.ExportId like '%{this.txtWKno.Text}%' )");
            }

            if (this.comboFabric.SelectedValue.ToString().ToUpper() != "ALL")
            {
                sbSQLCmd.Append($" AND PSD.FabricType= '{this.comboFabric.SelectedValue.ToString()}' ");
            }

            DualResult result;
            this.ShowWaitMessage("Data Loading....");
            if (result = DBProxy.Current.Select(null, sbSQLCmd.ToString(), cmds, out this.dtImportData))
            {
                if (this.dtImportData.Select("lock = 1").Any())
                {
                    MyUtility.Msg.WarningBox("Some material has been locked, please check material status in WH P38.");
                }
                else
                {
                    if (!this.dtImportData.Select("lock = 0").Any())
                    {
                        MyUtility.Msg.WarningBox("Data not found!!");
                    }
                }

                this.dtImportData.Select("lock = 1").Delete();
                this.listControlBindingSource1.DataSource = this.dtImportData;
                this.dtImportData.DefaultView.Sort = "poid,seq1,seq2,location,dyelot,roll";
            }
            else
            {
                this.ShowErr(sbSQLCmd.ToString(), result);
            }

            this.HideWaitMessage();
        }

        private void Sum_checkedqty()
        {
            this.listControlBindingSource1.EndEdit();
            DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
            object localPrice = dt.Compute("Sum(qty)", "selected = 1");
            this.displayTotal.Value = localPrice.ToString();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            MyUtility.Tool.SetupCombox(this.comboStockType, 2, 1, "B,Bulk,I,Inventory");
            this.comboStockType.SelectedIndex = 0;

            DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            ns.CellValidating += (s, e) =>
            {
                if (this.EditMode && !MyUtility.Check.Empty(e.FormattedValue))
                {
                    this.gridImport.GetDataRow(this.gridImport.GetSelectedRowIndex())["qty"] = e.FormattedValue;
                    this.gridImport.GetDataRow(this.gridImport.GetSelectedRowIndex())["selected"] = true;
                    this.Sum_checkedqty();

                    DataRow dr = this.gridImport.GetDataRow(e.RowIndex);
                    dr["Balance"] = (decimal)dr["stockqty"] - (decimal)e.FormattedValue;
                    dr.EndEdit();
                }
            };

            Ict.Win.UI.DataGridViewComboBoxColumn cbb_stocktype;
            Ict.Win.UI.DataGridViewNumericBoxColumn nb_qty;

            this.gridImport.CellValueChanged += (s, e) =>
            {
                if (this.gridImport.Columns[e.ColumnIndex].Name == this.col_chk.Name)
                {
                    DataRow dr = this.gridImport.GetDataRow(e.RowIndex);
                    if (Convert.ToBoolean(dr["selected"]) == true && Convert.ToDecimal(dr["qty"].ToString()) == 0)
                    {
                        dr["qty"] = dr["stockQty"];
                    }
                    else if (Convert.ToBoolean(dr["selected"]) == false)
                    {
                        dr["qty"] = 0;
                    }

                    dr.EndEdit();

                    this.Sum_checkedqty();
                }
            };
            this.gridImport.IsEditingReadOnly = false; // 必設定, 否則CheckBox會顯示圖示
            this.gridImport.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridImport)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk) // 0
                .Text("ExportID", header: "WK#", iseditingreadonly: true, width: Widths.AnsiChars(20)) // 3
                .Text("PoId", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(14)) // 3
                .Text("seq", header: "Seq#", iseditingreadonly: true, width: Widths.AnsiChars(6)) // 1
                .Text("roll", header: "Roll#", iseditingreadonly: true, width: Widths.AnsiChars(10)) // 2
                .Text("dyelot", header: "Dyelot", iseditingreadonly: true, width: Widths.AnsiChars(8)) // 3
                .Text("FabricType", header: "Material Type", iseditingreadonly: true, width: Widths.AnsiChars(8)) // 3
                .EditText("Description", header: "Description", iseditingreadonly: true) // 4
                .Text("Tone", header: "Tone/Grp", iseditingreadonly: true, width: Widths.AnsiChars(8))
                .Text("Refno", header: "Ref#", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("Color", header: "Color", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("SizeSpec", header: "Size", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("StockUnit", header: "Unit", iseditingreadonly: true) // 5
                .Numeric("stockqty", header: "Stock Qty", iseditingreadonly: true, decimal_places: 2, integer_places: 10) // 6
                .Numeric("qty", header: "Out Qty", decimal_places: 2, integer_places: 10, settings: ns).Get(out nb_qty) // 7
                .Numeric("balance", header: "Balance", iseditingreadonly: true, decimal_places: 2, integer_places: 10) // 8
                .Text("location", header: "Location", iseditingreadonly: true) // 9
                .ComboBox("stocktype", header: "Stock Type", iseditable: false).Get(out cbb_stocktype)
                .Text("ToPOID", header: "To POID", iseditingreadonly: true)
                .Text("ToSeq", header: "To Seq", iseditingreadonly: true)
               ;

            nb_qty.DefaultCellStyle.BackColor = Color.Pink;

            cbb_stocktype.DataSource = new BindingSource(this.di_stocktype, null);
            cbb_stocktype.ValueMember = "Key";
            cbb_stocktype.DisplayMember = "Value";
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Import
        private void BtnImport_Click(object sender, EventArgs e)
        {
            // listControlBindingSource1.EndEdit();
            this.gridImport.ValidateControl();
            DataTable dtGridBS1 = (DataTable)this.listControlBindingSource1.DataSource;
            if (MyUtility.Check.Empty(dtGridBS1) || dtGridBS1.Rows.Count == 0)
            {
                return;
            }

            DataRow[] dr2 = dtGridBS1.Select("Selected = 1");
            if (dr2.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select row(s) first!", "Warnning");
                return;
            }

            dr2 = dtGridBS1.Select("qty = 0 and Selected = 1");
            if (dr2.Length > 0)
            {
                MyUtility.Msg.WarningBox("Qty of selected row can't be zero!", "Warning");
                return;
            }

            dr2 = dtGridBS1.Select("qty <> 0 and Selected = 1");
            foreach (DataRow tmp in dr2)
            {
                DataRow[] findrow = this.dt_detail.AsEnumerable()
                    .Where(w => w.RowState != DataRowState.Deleted
                        && w["ExportID"].EqualString(tmp["ExportID"].ToString())
                        && w["poid"].EqualString(tmp["poid"].ToString())
                        && w["seq1"].EqualString(tmp["seq1"])
                        && w["seq2"].EqualString(tmp["seq2"].ToString())
                        && w["ToPOID"].EqualString(tmp["ToPOID"].ToString())
                        && w["Toseq1"].EqualString(tmp["Toseq1"])
                        && w["Toseq2"].EqualString(tmp["Toseq2"].ToString())
                        && w["roll"].EqualString(tmp["roll"])
                        && w["dyelot"].EqualString(tmp["dyelot"])
                        && w["stockType"].EqualString(tmp["stockType"])).ToArray();

                if (findrow.Length > 0)
                {
                    findrow[0]["qty"] = tmp["qty"];
                }
                else
                {
                    tmp["id"] = this.dr_master["id"];

                    // 拆解ToSeq
                    List<string> seqList = ToSeqSplit(MyUtility.Convert.GetString(tmp["ToSeq"]));
                    tmp["ToSeq1"] = seqList[0];
                    tmp["ToSeq2"] = seqList.Count > 1 ? seqList[1] : string.Empty;

                    tmp.AcceptChanges();
                    tmp.SetAdded();
                    this.dt_detail.ImportRow(tmp);
                }
            }

            this.Close();
        }

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        public static List<string> ToSeqSplit(string toSeq)
        {
            List<string> result = new List<string>();
            toSeq = toSeq.Replace(" ", string.Empty);

            if (toSeq.Length == 4)
            {
                // ToSeq1
                result.Add(toSeq.Substring(0, 2).Replace(" ", string.Empty));

                // ToSeq2
                result.Add(toSeq.Substring(2, 2).Replace(" ", string.Empty));
            }
            else if (toSeq.Length > 4)
            {
                // ToSeq1
                result.Add(toSeq.Substring(0, 3).Replace(" ", string.Empty));

                // ToSeq2
                result.Add(toSeq.Substring(3, 2).Replace(" ", string.Empty));
            }
            else
            {
                // ToSeq1
                result.Add(toSeq.Replace(" ", string.Empty));
            }

            return result;
        }
    }
}
