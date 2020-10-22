using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;
using System.Linq;

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
        private void BtnFindNow_Click(object sender, EventArgs e)
        {
            if (this.comboStockType.SelectedIndex < 0)
            {
                MyUtility.Msg.WarningBox("< Stock Type > can't be empty!!");
                this.comboStockType.Focus();
                return;
            }

            if (MyUtility.Check.Empty(this.txtSPNo.Text))
            {
                MyUtility.Msg.WarningBox("< SP# > can't be empty!!");
                this.txtSPNo.Focus();
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
            sbSQLCmd.Append(string.Format(
                @"
select  0 as selected 
        , '' id
		, [ExportID] = Export.ExportID
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
        , FI.inqty-FI.outqty + FI.adjustqty as Balance
        , FI.StockType
        , FI.ukey as ftyinventoryukey
        , dbo.Getlocation(FI.ukey) location
        , FI.inqty - FI.outqty + FI.adjustqty as stockqty
		, [ToPOID]=''
		, [ToSeq]=''
		, [ToSeq1]=''
		, [ToSeq2]=''
FROM FtyInventory FI 
LEFT JOIN Orders O ON O.ID = FI.POID
LEFT JOIN Factory F ON F.ID = O.FactoryID
LEFT JOIN PO_Supp_Detail PSD ON PSD.ID=FI.POID AND PSD.SEQ1 = FI.SEQ1 AND PSD.SEQ2=FI.SEQ2
outer apply(
	select ExportID = Stuff((
		select concat(',',id)
		from (
				select 	distinct id
				from dbo.Export_Detail d
				where d.PoID=fi.POID and d.Seq1=fi.Seq1 and d.Seq2=fi.Seq2
			) s
		for xml path ('')
	) , 1, 1, '')
)Export
Where FI.lock = 0 
and ( F.MDivisionID = '{0}' OR o.MDivisionID= '{0}' )
        and FI.inqty - FI.outqty + FI.adjustqty > 0 
        and FI.Poid = @sp 
        and FI.stocktype = '{1}'", Env.User.Keyword, stocktype));

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
                sbSQLCmd.Append($" AND ExportID like '%{this.txtWKno.Text}%'");
            }

            if (this.comboFabric.SelectedValue.ToString().ToUpper() != "ALL")
            {
                sbSQLCmd.Append($" AND PSD.FabricType= '{this.comboFabric.SelectedValue.ToString()}' ");
            }

            DualResult result;
            this.ShowWaitMessage("Data Loading....");
            if (result = DBProxy.Current.Select(null, sbSQLCmd.ToString(), cmds, out this.dtImportData))
            {
                if (this.dtImportData.Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!!");
                }
                else
                {
                    this.dtImportData.DefaultView.Sort = "poid,seq1,seq2,location,dyelot,roll";
                }

                this.listControlBindingSource1.DataSource = this.dtImportData;
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
            Ict.Win.UI.DataGridViewTextBoxColumn cbb_ToPOID;
            Ict.Win.UI.DataGridViewTextBoxColumn cbb_ToSeq;

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
                .Text("FabricType", header: "Fabric Type", iseditingreadonly: true, width: Widths.AnsiChars(8)) // 3
                .EditText("Description", header: "Description", iseditingreadonly: true) // 4
                .Text("StockUnit", header: "Unit", iseditingreadonly: true) // 5
                .Numeric("stockqty", header: "Stock Qty", iseditingreadonly: true, decimal_places: 2, integer_places: 10) // 6
                .Numeric("qty", header: "Out Qty", decimal_places: 2, integer_places: 10, settings: ns).Get(out nb_qty) // 7
                .Numeric("balance", header: "Balance", iseditingreadonly: true, decimal_places: 2, integer_places: 10) // 8
                .Text("location", header: "Location", iseditingreadonly: true) // 9
                .ComboBox("stocktype", header: "Stock Type", iseditable: false).Get(out cbb_stocktype)
                .Text("ToPOID", header: "To POID", iseditingreadonly: false).Get(out cbb_ToPOID)
                .Text("ToSeq", header: "To Seq", iseditingreadonly: false).Get(out cbb_ToSeq)
               ;

            nb_qty.DefaultCellStyle.BackColor = Color.Pink;
            cbb_ToPOID.DefaultCellStyle.BackColor = Color.Pink;
            cbb_ToSeq.DefaultCellStyle.BackColor = Color.Pink;

            cbb_ToPOID.MaxLength = 13;
            cbb_ToSeq.MaxLength = 5;

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
                DataRow[] findrow = this.dt_detail.AsEnumerable().Where(row => row.RowState != DataRowState.Deleted
                                                                          && row["poid"].EqualString(tmp["poid"].ToString()) && row["seq1"].EqualString(tmp["seq1"])
                                                                          && row["seq2"].EqualString(tmp["seq2"].ToString()) && row["roll"].EqualString(tmp["roll"])
                                                                          && row["dyelot"].EqualString(tmp["dyelot"]) && row["stockType"].EqualString(tmp["stockType"])).ToArray();

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
