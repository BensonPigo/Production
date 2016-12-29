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
    public partial class P31_Import : Sci.Win.Subs.Base
    {
        DataRow dr_master;
        DataTable dt_detail;
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        protected DataTable dtBorrow;
        private Dictionary<string, string> di_stocktype = new Dictionary<string, string>();

        public P31_Import(DataRow master, DataTable detail)
        {
            InitializeComponent();
            dr_master = master;
            dt_detail = detail;
            di_stocktype.Add("B", "Bulk");
            di_stocktype.Add("I", "Inventory");
        }

        //Find Now Button
        private void button1_Click(object sender, EventArgs e)
        {
            StringBuilder strSQLCmd = new StringBuilder();
            String sp = this.textBox1.Text.TrimEnd();
            String seq = this.textBox2.Text.TrimEnd();
            String fromSP = this.textBox3.Text.TrimEnd();

            if (string.IsNullOrWhiteSpace(sp) || string.IsNullOrWhiteSpace(seq) || string.IsNullOrWhiteSpace(fromSP))
            {
                MyUtility.Msg.WarningBox("< To SP# Seq> <From SP#> can't be empty!!");
                textBox1.Focus();
                return;
            }

            else
            {
                // 建立可以符合回傳的Cursor

                strSQLCmd.Append(string.Format(@"select 0 as selected ,'' id
,c.Roll FromRoll
,c.Dyelot FromDyelot
,c.ukey as fromftyinventoryukey
,c.mdivisionid as FromMdivisionid
, a.id as FromPoId
,a.Seq1 as FromSeq1
,a.Seq2 as FromSeq2
,left(a.seq1+' ',3)+a.Seq2 as fromseq
,dbo.getmtldesc(a.id,a.seq1,a.seq2,2,0) as [Description]
,a.stockunit
,c.StockType as fromstocktype
,c.InQty - c.OutQty + c.AdjustQty balance
,stuff((select ',' + mtllocationid from (select mtllocationid from ftyinventory_detail where ukey = c.ukey)t for xml path('')), 1, 1, '') as location
,left(b.seq1+' ',3)+b.Seq2 as toseq
,c.Roll toroll
,c.Dyelot todyelot
,0.00 as Qty
,'B' ToStocktype
,'{4}' as tomdivisionid
,b.id topoid
,b.seq1 toseq1
,b.seq2 toseq2
,a.fabrictype
from dbo.PO_Supp_Detail a 
inner join dbo.ftyinventory c on c.poid = a.id and c.seq1 = a.seq1 and c.seq2  = a.seq2 
left join dbo.po_supp_detail b on b.Refno = a.Refno and b.SizeSpec = a.SizeSpec and b.ColorID = a.ColorID and b.BrandId = a.BrandId
Where a.id = '{0}' and b.id = '{1}' and b.seq1 = '{2}' and b.seq2='{3}' and c.mdivisionid='{4}'
and  c.lock = 0 and c.inqty-c.outqty + c.adjustqty > 0", fromSP, sp, seq.Substring(0, 3), seq.Substring(3, 2), Sci.Env.User.Keyword)); // 

                MyUtility.Msg.WaitWindows("Data Loading....");
                Ict.DualResult result;
                if (result = DBProxy.Current.Select(null, strSQLCmd.ToString(), out dtBorrow))
                {
                    if (dtBorrow.Rows.Count == 0)
                    { MyUtility.Msg.WarningBox("Data not found!!"); }
                    listControlBindingSource1.DataSource = dtBorrow;
                    dtBorrow.DefaultView.Sort = "fromseq1,fromseq2,location,fromdyelot,balance desc";
                }
                else { ShowErr(strSQLCmd.ToString(), result); }
                MyUtility.Msg.WaitClear();
            }
        }

        private void sum_checkedqty()
        {
            listControlBindingSource1.EndEdit();
            DataTable dt = (DataTable)listControlBindingSource1.DataSource;
            Object localPrice = dt.Compute("Sum(qty)", "selected = 1");
            this.displayBox1.Value = localPrice.ToString();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            Ict.Win.DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            ns.CellValidating += (s, e) =>
                {
                    if (this.EditMode && !MyUtility.Check.Empty(e.FormattedValue))
                    {
                        grid1.GetDataRow(grid1.GetSelectedRowIndex())["qty"] = e.FormattedValue;
                        grid1.GetDataRow(grid1.GetSelectedRowIndex())["selected"] = true;
                        this.sum_checkedqty();
                    }
                };

            this.grid1.CellValueChanged += (s, e) =>
            {
                if (grid1.Columns[e.ColumnIndex].Name == col_chk.Name)
                {
                    this.sum_checkedqty();
                }
            };

            Ict.Win.UI.DataGridViewComboBoxColumn cbb_stocktype;

            this.grid1.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.grid1.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.grid1)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)   //0
                .Text("fromroll", header: "Roll", iseditingreadonly: true, width: Widths.AnsiChars(6)) //1
                .Text("fromdyelot", header: "Dyelot", iseditingreadonly: true, width: Widths.AnsiChars(6)) //2
                .Text("fromseq", header: "From" + Environment.NewLine + "Seq#", iseditingreadonly: true, width: Widths.AnsiChars(6)) //3
                .EditText("Description", header: "Description", iseditingreadonly: true, width: Widths.AnsiChars(25)) //4
                .Text("StockUnit", header: "Unit", iseditingreadonly: true)      //5
                .ComboBox("fromstocktype", header: "From" + Environment.NewLine + "Stock" + Environment.NewLine + "Type", iseditable: false).Get(out cbb_stocktype)    //6
                .Numeric("balance", header: "Stock Qty", iseditable: true, decimal_places: 2, integer_places: 10) //7
                .Text("location", header: "From Location", iseditingreadonly: true)      //8
                .Text("toseq", header: "To" + Environment.NewLine + "Seq#", iseditingreadonly: true, width: Widths.AnsiChars(6)) //9
                .Text("toroll", header: "To" + Environment.NewLine + "Roll", width: Widths.AnsiChars(6)) //10
                .Numeric("qty", header: "Issue" + Environment.NewLine + "Qty", decimal_places: 2, integer_places: 10, settings: ns)  //11
               ;

            this.grid1.Columns[10].DefaultCellStyle.BackColor = Color.Pink;
            this.grid1.Columns[11].DefaultCellStyle.BackColor = Color.Pink;

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
            StringBuilder warningmsg = new StringBuilder();

            grid1.ValidateControl();
            grid1.EndEdit();

            if (MyUtility.Check.Empty(dtBorrow) || dtBorrow.Rows.Count == 0) return;

            DataRow[] dr2 = dtBorrow.Select("Selected = 1");
            if (dr2.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select rows first!", "Warnning");
                return;
            }

            dr2 = dtBorrow.Select("qty = 0 and Selected = 1");
            if (dr2.Length > 0)
            {
                MyUtility.Msg.WarningBox("Qty of selected row can't be zero!", "Warning");
                return;
            }

            dr2 = dtBorrow.Select("qty <> 0 and Selected = 1");
            foreach (DataRow row in dr2)
            {
                if (row["fabrictype"].ToString().ToUpper() == "F" && (MyUtility.Check.Empty(row["toroll"]) || MyUtility.Check.Empty(row["todyelot"])))
                {
                    warningmsg.Append(string.Format(@"To SP#: {0} To Seq#: {1}-{2} To Roll#:{3} To Dyelot:{4} Roll and Dyelot can't be empty"
                        , row["topoid"], row["toseq1"], row["toseq2"], row["toroll"], row["todyelot"]) + Environment.NewLine);
                }

                if (row["fabrictype"].ToString().ToUpper() != "F")
                {
                    row["toroll"] = "";
                    row["todyelot"] = "";
                }
            }
            if (!MyUtility.Check.Empty(warningmsg.ToString()))
            {
                MyUtility.Msg.WarningBox(warningmsg.ToString());
                return;
            }
            foreach (DataRow tmp in dr2)
            {
                DataRow[] findrow = dt_detail.Select(
                    string.Format(@"fromftyinventoryukey = {0} 
                    and tomdivisionid = '{1}' and topoid = '{2}' and toseq1 = '{3}' and toseq2 = '{4}' and toroll ='{5}'and todyelot='{6}' and tostocktype='{7}' "
                    , tmp["fromftyinventoryukey"]
                    , tmp["tomdivisionid"], tmp["topoid"], tmp["toseq1"], tmp["toseq2"], tmp["toroll"], tmp["todyelot"], tmp["tostocktype"]));

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

        // To SP# Valid
        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            string sp = textBox1.Text.TrimEnd();
            string seq = textBox2.Text.PadRight(5, ' ');

            DataRow tmp;

            if (MyUtility.Check.Empty(sp)) return;

            if (MyUtility.Check.Empty(textBox2.Text.TrimEnd()))
            {
                if (!MyUtility.Check.Seek(string.Format("select 1 where exists(select * from po_supp_detail where id ='{0}')"
                    , sp), null))
                {
                    MyUtility.Msg.WarningBox("SP# is not found!!");
                    e.Cancel = true;
                    return;
                }
            }
            else
            {
                if (!MyUtility.Check.Seek(string.Format(@"select sizespec,refno,colorid,dbo.getmtldesc(id,seq1,seq2,2,0) as [description]
                        from po_supp_detail where id ='{0}' and seq1 = '{1}' and seq2 = '{2}'", sp, seq.Substring(0, 3), seq.Substring(3, 2)), out tmp, null))
                {
                    MyUtility.Msg.WarningBox("SP#-Seq is not found!!");
                    e.Cancel = true;
                    return;
                }
                else
                {
                    this.displayBox2.Value = tmp["sizespec"];
                    this.displayBox3.Value = tmp["refno"];
                    this.displayBox4.Value = tmp["colorid"];
                    this.editBox1.Text = tmp["description"].ToString();
                }
            }

        }
        // To Seq# Valid
        private void textBox2_Validating(object sender, CancelEventArgs e)
        {
            DataRow tmp;
            string sp = textBox1.Text.TrimEnd();
            if (MyUtility.Check.Empty(sp) || MyUtility.Check.Empty(textBox2.Text.TrimEnd())) return;
            string seq = textBox2.Text.PadRight(5, ' ');

            if (!MyUtility.Check.Seek(string.Format(@"select sizespec,refno,colorid,dbo.getmtldesc(id,seq1,seq2,2,0) as [description] from po_supp_detail where id ='{0}' 
                        and seq1 = '{1}' and seq2 = '{2}'", sp, seq.Substring(0, 3), seq.Substring(3, 2)), out tmp, null))
            {
                MyUtility.Msg.WarningBox("SP#-Seq is not found!!");
                e.Cancel = true;
                return;
            }
            else
            {
                this.displayBox2.Value = tmp["sizespec"];
                this.displayBox3.Value = tmp["refno"];
                this.displayBox4.Value = tmp["colorid"];
                this.editBox1.Text = tmp["description"].ToString();
            }

        }
    }
}
