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
        DataRow dr_master;
        DataTable dt_detail;
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        bool flag;
        string poType;
        protected DataTable dtArtwork;

        public P19_Import(DataRow master, DataTable detail)
        {
            InitializeComponent();
            dr_master = master;
            dt_detail = detail;
        }

        //Find Now Button
        private void button1_Click(object sender, EventArgs e)
        {
            String sp_b = this.textBox1.Text;

            if (string.IsNullOrWhiteSpace(sp_b))
            {
                MyUtility.Msg.WarningBox("< SP# > can't be empty!!");
                textBox1.Focus();
                return;
            }

            else
            {
                // 建立可以符合回傳的Cursor

                string strSQLCmd = string.Format(@"select 0 as selected ,'' id, c.mdivisionid,a.id as PoId,a.Seq1,a.Seq2,left(a.seq1+' ',3)+a.Seq2 as seq
,a.FabricType
,a.stockunit
,dbo.getmtldesc(a.id,a.seq1,a.seq2,2,0) Description
,c.Roll
,c.Dyelot
,0.00 as Qty
,'I' StockType
,c.ukey as ftyinventoryukey
,(select t.mtllocationid+',' from (select mtllocationid from dbo.ftyinventory_detail fd where fd.ukey = c.ukey) t for xml path('') ) location
,c.inqty-c.outqty + c.adjustqty as stockqty
from dbo.PO_Supp_Detail a 
inner join dbo.ftyinventory c on c.poid = a.id and c.seq1 = a.seq1 and c.seq2  = a.seq2 and c.stocktype = 'I'
Where a.id = '{0}' and c.lock = 0 and c.inqty-c.outqty + c.adjustqty > 0 and c.mdivisionid='{1}'", sp_b, Sci.Env.User.Keyword);

                Ict.DualResult result;
                MyUtility.Msg.WaitWindows("Data loading....");
                if (result = DBProxy.Current.Select(null, strSQLCmd, out dtArtwork))
                {
                    if (dtArtwork.Rows.Count == 0)
                    { MyUtility.Msg.WarningBox("Data not found!!"); }
                    listControlBindingSource1.DataSource = dtArtwork;
                    dtArtwork.Columns.Add("Balance", typeof(decimal));
                    dtArtwork.Columns["Balance"].Expression = "stockqty - qty";
                }
                else { ShowErr(strSQLCmd, result); }
                MyUtility.Msg.WaitClear();
            }
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

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

            Helper.Controls.Grid.Generator(this.grid1)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)   //0
                .Text("seq", header: "Seq#", iseditingreadonly: true, width: Widths.AnsiChars(6)) //1
                .Text("roll", header: "Roll#", iseditingreadonly: true, width: Widths.AnsiChars(10)) //2
                .Text("dyelot", header: "Dyelot", iseditingreadonly: true, width: Widths.AnsiChars(6)) //3
                .EditText("Description", header: "Description", iseditingreadonly: true) //4
                .Text("StockUnit", header: "Unit", iseditingreadonly: true)      //5
                .Numeric("stockqty", header: "Stock Qty", iseditingreadonly: true, decimal_places: 2, integer_places: 10) //6
                .Numeric("qty", header: "Out Qty", decimal_places: 2, integer_places: 10,settings:ns)  //7
                .Numeric("balance", header: "Balance", iseditingreadonly: true, decimal_places: 2, integer_places: 10) //8
                .Text("location", header: "Location", iseditingreadonly: true)      //9
               ; //

            this.grid1.Columns[7].DefaultCellStyle.BackColor = Color.Pink; 

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



        private void button2_Click(object sender, EventArgs e)
        {
            //listControlBindingSource1.EndEdit();
            grid1.ValidateControl();
            DataTable dtGridBS1 = (DataTable)listControlBindingSource1.DataSource;
            if (MyUtility.Check.Empty(dtGridBS1) || dtGridBS1.Rows.Count == 0) return;

            DataRow[] dr2 = dtGridBS1.Select("Selected = 1");
            if (dr2.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select rows first!", "Warnning");
                return;
            }

            dr2 = dtGridBS1.Select("qty = 0 and Selected = 1");
            if (dr2.Length > 0 )
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
