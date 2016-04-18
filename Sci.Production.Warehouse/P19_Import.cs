using System;
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
        private void button1_Click(object sender, EventArgs e)
        {
            if (cbbStockType.SelectedIndex < 0)
            {
                MyUtility.Msg.WarningBox("< Stock Type > can't be empty!!");
                this.cbbStockType.Focus();
                return;
            }

            if (MyUtility.Check.Empty(this.txtSP.Text))
            {
                MyUtility.Msg.WarningBox("< SP# > can't be empty!!");
                txtSP.Focus();
                return;
            }


            StringBuilder sbSQLCmd = new StringBuilder();
            String stocktype = this.cbbStockType.SelectedValue.ToString();
            String sp = this.txtSP.Text;
            String seq = this.txtSeq.Text.TrimEnd();

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

            sbSQLCmd.Append(string.Format(@"select 0 as selected ,'' id, c.mdivisionid,a.id as PoId,a.Seq1,a.Seq2,left(a.seq1+' ',3)+a.Seq2 as seq
,a.FabricType
,a.stockunit
,dbo.getmtldesc(a.id,a.seq1,a.seq2,2,0) Description
,c.Roll
,c.Dyelot
,0.00 as Qty
,c.StockType
,c.ukey as ftyinventoryukey
,(select t.mtllocationid+',' from (select mtllocationid from dbo.ftyinventory_detail fd where fd.ukey = c.ukey) t for xml path('') ) location
,c.inqty-c.outqty + c.adjustqty as stockqty
from dbo.PO_Supp_Detail a 
inner join dbo.ftyinventory c on c.poid = a.id and c.seq1 = a.seq1 and c.seq2  = a.seq2 
Where c.lock = 0 and c.inqty-c.outqty + c.adjustqty > 0 
and a.id = @sp and c.mdivisionid='{0}' and c.stocktype = '{1}'", Sci.Env.User.Keyword, stocktype));

            if (!MyUtility.Check.Empty(seq))
            {
                sbSQLCmd.Append(string.Format(@" and a.seq1 = @seq1 and a.seq2=@seq2"));
                sp_seq1.Value = seq.Substring(0, 3);
                sp_seq2.Value = seq.Substring(3, 2);
                cmds.Add(sp_seq1);
                cmds.Add(sp_seq2);
            }

            Ict.DualResult result;
            MyUtility.Msg.WaitWindows("Data loading....");
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
            MyUtility.Msg.WaitClear();

        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            MyUtility.Tool.SetupCombox(cbbStockType, 2, 1, "B,Bulk,I,Inventory");
            cbbStockType.SelectedIndex = 0;

            this.grid1.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.grid1.DataSource = listControlBindingSource1;

            Ict.Win.DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            ns.CellValidating += (s, e) =>
            {
                if (this.EditMode && !MyUtility.Check.Empty(e.FormattedValue))
                {
                    grid1.GetDataRow(grid1.GetSelectedRowIndex())["qty"] = e.FormattedValue;
                    grid1.GetDataRow(grid1.GetSelectedRowIndex())["selected"] = true;
                }
            };

            Ict.Win.UI.DataGridViewComboBoxColumn cbb_stocktype;
            Ict.Win.UI.DataGridViewNumericBoxColumn nb_qty;

            Helper.Controls.Grid.Generator(this.grid1)
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

            // 全選
            checkBox1.Click += (s, e) =>
            {
                if (null != col_chk)
                {
                    this.grid1.SetCheckeds(col_chk);
                    if (col_chk.Index == this.grid1.CurrentCellAddress.X)
                    {
                        if (this.grid1.IsCurrentCellInEditMode) this.grid1.RefreshEdit();
                    }
                }
            };

            // 全不選
            checkBox2.Click += (s, e) =>
            {
                if (null != col_chk)
                {
                    this.grid1.SetUncheckeds(col_chk);
                    if (col_chk.Index == this.grid1.CurrentCellAddress.X)
                    {
                        if (this.grid1.IsCurrentCellInEditMode) this.grid1.RefreshEdit();
                    }
                }
            };
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        //Import
        private void button2_Click(object sender, EventArgs e)
        {
            //listControlBindingSource1.EndEdit();
            grid1.ValidateControl();
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
                DataRow[] findrow = dt_detail.Select(string.Format("poid = '{0}' and seq1 = '{1}' and seq2 = '{2}'and roll = '{3}'and dyelot = '{4}' and stocktype='{5}'"
                    , tmp["poid"].ToString(), tmp["seq1"].ToString(), tmp["seq2"].ToString(), tmp["roll"].ToString(), tmp["dyelot"].ToString(), tmp["stocktype"].ToString()));

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
