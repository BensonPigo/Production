﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci;
using Sci.Data;
using System.Linq;

namespace Sci.Production.Warehouse
{
    public partial class P19_Import : Sci.Win.Subs.Base
    {
        DataRow dr_master;  // 抓主頁的表頭資料用
        DataTable dt_detail;    // 將匯入資料寫入主頁的明細用
        private Dictionary<string, string> di_stocktype = new Dictionary<string, string>();
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        protected DataTable dtImportData;

        public P19_Import(DataRow master, DataTable detail)
        {
            InitializeComponent();
            di_stocktype.Add("B", "Bulk");
            di_stocktype.Add("I", "Inventory");
            dr_master = master;
            dt_detail = detail;
        }

        //Find Now Button
        private void btnFindNow_Click(object sender, EventArgs e)
        {
            if (comboStockType.SelectedIndex < 0)
            {
                MyUtility.Msg.WarningBox("< Stock Type > can't be empty!!");
                this.comboStockType.Focus();
                return;
            }

            if (MyUtility.Check.Empty(this.txtSPNo.Text))
            {
                MyUtility.Msg.WarningBox("< SP# > can't be empty!!");
                txtSPNo.Focus();
                return;
            }


            StringBuilder sbSQLCmd = new StringBuilder();
            String stocktype = this.comboStockType.SelectedValue.ToString();
            String sp = this.txtSPNo.Text;

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

            sbSQLCmd.Append(string.Format(@"
select  0 as selected 
        , '' id
        , a.id as PoId
        , a.Seq1
        , a.Seq2
        , concat(Ltrim(Rtrim(a.seq1)), ' ', a.Seq2) as seq
        , a.FabricType
        , a.stockunit
        , dbo.getmtldesc(a.id,a.seq1,a.seq2,2,0) Description
        , c.Roll
        , c.Dyelot
        , 0.00 as Qty
        , c.StockType
        , c.ukey as ftyinventoryukey
        , dbo.Getlocation(c.ukey) location
        , c.inqty-c.outqty + c.adjustqty as stockqty
from dbo.PO_Supp_Detail a WITH (NOLOCK) 
inner join dbo.ftyinventory c WITH (NOLOCK) on c.poid = a.id and c.seq1 = a.seq1 and c.seq2  = a.seq2 
inner join dbo.Orders on c.POID = orders.id
inner join dbo.Factory on orders.FactoryID = factory.ID
Where   c.lock = 0 
and factory.MDivisionID = '{0}' 
        and c.inqty-c.outqty + c.adjustqty > 0 
        and a.id = @sp 
        and c.stocktype = '{1}'", Sci.Env.User.Keyword, stocktype));

            sp_seq1.Value = txtSeq1.seq1;
            sp_seq2.Value = txtSeq1.seq2;
            cmds.Add(sp_seq1);
            cmds.Add(sp_seq2);
            if (!txtSeq1.checkSeq1Empty() && txtSeq1.checkSeq2Empty())
            {
                sbSQLCmd.Append(@"
        and a.seq1 = @seq1 ");
            }else if (!txtSeq1.checkEmpty(showErrMsg: false))
            {
                sbSQLCmd.Append(@" 
        and a.seq1 = @seq1 and a.seq2 = @seq2");
                sp_seq1.Value = txtSeq1.seq1;
                sp_seq2.Value = txtSeq1.seq2;

            }

            Ict.DualResult result;
            this.ShowWaitMessage("Data Loading....");
            if (result = DBProxy.Current.Select(null, sbSQLCmd.ToString(),cmds, out dtImportData))
            {
                if (dtImportData.Rows.Count == 0)
                { MyUtility.Msg.WarningBox("Data not found!!"); }
                listControlBindingSource1.DataSource = dtImportData;
                dtImportData.DefaultView.Sort = "poid,seq1,seq2,location,dyelot,roll";
                dtImportData.Columns.Add("Balance", typeof(decimal));
                dtImportData.Columns["Balance"].Expression = "stockqty - qty";
            }
            else { ShowErr(sbSQLCmd.ToString(), result); }
            this.HideWaitMessage();

        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            MyUtility.Tool.SetupCombox(comboStockType, 2, 1, "B,Bulk,I,Inventory");
            comboStockType.SelectedIndex = 0;

            this.gridImport.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.gridImport.DataSource = listControlBindingSource1;

            Ict.Win.DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            ns.CellValidating += (s, e) =>
            {
                if (this.EditMode && !MyUtility.Check.Empty(e.FormattedValue))
                {
                    gridImport.GetDataRow(gridImport.GetSelectedRowIndex())["qty"] = e.FormattedValue;
                    gridImport.GetDataRow(gridImport.GetSelectedRowIndex())["selected"] = true;
                }
            };

            Ict.Win.UI.DataGridViewComboBoxColumn cbb_stocktype;
            Ict.Win.UI.DataGridViewNumericBoxColumn nb_qty;

            this.gridImport.CellValueChanged += (s, e) =>
            {
                if (gridImport.Columns[e.ColumnIndex].Name == col_chk.Name)
                {
                    DataRow dr = gridImport.GetDataRow(e.RowIndex);
                    if (Convert.ToBoolean(dr["selected"]) == true && Convert.ToDecimal(dr["qty"].ToString()) == 0)
                    {
                        dr["qty"] = dr["stockQty"];
                    }
                    else if (Convert.ToBoolean(dr["selected"]) == false)
                    {
                        dr["qty"] = 0;
                    }
                    dr.EndEdit();
                }
            };

            Helper.Controls.Grid.Generator(this.gridImport)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)   //0
                .Text("seq", header: "Seq#", iseditingreadonly: true, width: Widths.AnsiChars(6)) //1
                .Text("roll", header: "Roll#", iseditingreadonly: true, width: Widths.AnsiChars(10)) //2
                .Text("dyelot", header: "Dyelot", iseditingreadonly: true, width: Widths.AnsiChars(6)) //3
                .EditText("Description", header: "Description", iseditingreadonly: true) //4
                .Text("StockUnit", header: "Unit", iseditingreadonly: true)      //5
                .Numeric("stockqty", header: "Stock Qty", iseditingreadonly: true, decimal_places: 2, integer_places: 10) //6
                .Numeric("qty", header: "Out Qty", decimal_places: 2, integer_places: 10, settings: ns).Get(out nb_qty)  //7
                .Numeric("balance", header: "Balance", iseditingreadonly: true, decimal_places: 2, integer_places: 10) //8
                .Text("location", header: "Location", iseditingreadonly: true)      //9
                .ComboBox("stocktype", header: "Stock Type", iseditable: false).Get(out cbb_stocktype)
               ; //

            nb_qty.DefaultCellStyle.BackColor = Color.Pink;

            cbb_stocktype.DataSource = new BindingSource(di_stocktype, null);
            cbb_stocktype.ValueMember = "Key";
            cbb_stocktype.DisplayMember = "Value";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        //Import
        private void btnImport_Click(object sender, EventArgs e)
        {
            //listControlBindingSource1.EndEdit();
            gridImport.ValidateControl();
            DataTable dtGridBS1 = (DataTable)listControlBindingSource1.DataSource;
            if (MyUtility.Check.Empty(dtGridBS1) || dtGridBS1.Rows.Count == 0) 
                return;

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
                DataRow[] findrow = dt_detail.AsEnumerable().Where(row => row.RowState != DataRowState.Deleted
                                                                          && row["poid"].EqualString(tmp["poid"].ToString()) && row["seq1"].EqualString(tmp["seq1"])
                                                                          && row["seq2"].EqualString(tmp["seq2"].ToString()) && row["roll"].EqualString(tmp["roll"])
                                                                          && row["dyelot"].EqualString(tmp["dyelot"]) && row["stockType"].EqualString(tmp["stockType"])).ToArray();
 
                if (findrow.Length > 0)
                {
                    findrow[0]["qty"] = tmp["qty"];
                }
                else
                {
                    tmp["id"] = dr_master["id"];
                    tmp.AcceptChanges();
                    tmp.SetAdded();
                    dt_detail.ImportRow(tmp);
                }
            }


            this.Close();
        }
    }
}
