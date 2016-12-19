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
using Sci.Production.PublicPrg;

namespace Sci.Production.Warehouse
{
    public partial class P37_Import : Sci.Win.Subs.Base
    {
        DataRow dr_master;
        DataTable dt_detail;
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        protected DataTable dtScrap;

        public P37_Import(DataRow master, DataTable detail)
        {
            InitializeComponent();
            dr_master = master;
            dt_detail = detail;
        }

        //Find Now Button
        private void button1_Click(object sender, EventArgs e)
        {
            StringBuilder strSQLCmd = new StringBuilder();
            String sp = this.textBox1.Text.TrimEnd();
            String seq = this.textBox2.Text.TrimEnd();
            String transid = this.textBox3.Text.TrimEnd();
            String wkno = this.textBox4.Text.TrimEnd();

            if (MyUtility.Check.Empty(sp) && MyUtility.Check.Empty(transid) && MyUtility.Check.Empty(wkno))
            {
                MyUtility.Msg.WarningBox("Please fill < SP# > or < Transaction > or < WK# > can't be empty!!");
                textBox1.Focus();
                return;
            }
            else
            {
                // 建立可以符合回傳的Cursor
                #region -- SQL Command --
                if (MyUtility.Check.Empty(transid) && MyUtility.Check.Empty(wkno))
                {
                    strSQLCmd.Append(string.Format(@"select 0 as selected 
,'' id
, '' ExportId
,null as ETA
,f.PoId,f.seq1,f.seq2,f.seq1+f.seq2 as seq,f.Roll,f.Dyelot,p1.stockunit
,f.StockType 
,f.InQty - f.OutQty + f.AdjustQty balance
,0.00 as qty
,(select t.MtlLocationID+',' from (select MtlLocationID from FtyInventory_Detail where Ukey = f.Ukey) t for xml path('')) as location
,dbo.getMtlDesc(f.PoId,f.seq1,f.seq2,2,0) [description]
,f.ukey ftyinventoryukey
,f.mdivisionid
from dbo.FtyInventory f 
left join PO_Supp_Detail p1 on p1.ID = f.PoId and p1.seq1 = f.SEQ1 and p1.SEQ2 = f.seq2
where f.InQty - f.OutQty + f.AdjustQty > 0 and f.lock=0 and stocktype !='O' and f.mdivisionid='{0}'", Sci.Env.User.Keyword));
                }
                else
                {
                    strSQLCmd.Append(string.Format(@"select 0 as selected 
,'' id
, a.ExportId,a.ETA,b.PoId,b.seq1,b.seq2,b.seq1+b.seq2 as seq,b.Roll,b.Dyelot,p1.stockunit
,b.StockType 
,f.InQty - f.OutQty + f.AdjustQty balance
,0.00 as qty
,(select t.MtlLocationID+',' from (select MtlLocationID from FtyInventory_Detail where Ukey = f.Ukey) t for xml path('')) as location
,dbo.getMtlDesc(b.PoId,b.seq1,b.seq2,2,0) [description]
,f.ukey ftyinventoryukey
,f.mdivisionid
from dbo.Receiving a inner join dbo.Receiving_Detail b on a.id = b.id
inner join dbo.FtyInventory f on f.POID = b.PoId and f.seq1 = b.seq1 and f.seq2 = b.Seq2 and f.stocktype = b.stocktype and f.MDivisionID = b.mdivisionid
left join PO_Supp_Detail p1 on p1.ID = b.PoId and p1.seq1 = b.SEQ1 and p1.SEQ2 = b.seq2
where f.InQty - f.OutQty + f.AdjustQty > 0 and f.lock=0 and a.Status = 'Confirmed' and a.mdivisionid='{0}' and b.StockType!='O'

", Sci.Env.User.Keyword));
                }
                #endregion

                System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
                sp1.ParameterName = "@sp1";

                System.Data.SqlClient.SqlParameter seq1 = new System.Data.SqlClient.SqlParameter();
                seq1.ParameterName = "@seq1";

                System.Data.SqlClient.SqlParameter seq2 = new System.Data.SqlClient.SqlParameter();
                seq2.ParameterName = "@seq2";

                System.Data.SqlClient.SqlParameter sp3 = new System.Data.SqlClient.SqlParameter();
                sp3.ParameterName = "@transid";

                System.Data.SqlClient.SqlParameter sp4 = new System.Data.SqlClient.SqlParameter();
                sp4.ParameterName = "@wkno";

                IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();

                if (!MyUtility.Check.Empty(sp))
                {
                    strSQLCmd.Append(@" and f.poid = @sp1 ");
                    sp1.Value = sp;
                    cmds.Add(sp1);
                }

                if (!MyUtility.Check.Empty(seq))
                {
                    strSQLCmd.Append(@" and f.seq1 = @seq1 and f.seq2 = @seq2");
                    seq1.Value = seq.Substring(0, 3);
                    seq2.Value = seq.Substring(3, 2);
                    cmds.Add(seq1);
                    cmds.Add(seq2);
                }

                if (!MyUtility.Check.Empty(transid))
                {
                    strSQLCmd.Append(@" and a.id = @transid ");
                    sp3.Value = transid;
                    cmds.Add(sp3);
                }

                if (!MyUtility.Check.Empty(wkno))
                {
                    strSQLCmd.Append(@" and a.exportid = @wkno ");
                    sp4.Value = wkno;
                    cmds.Add(sp4);
                }

                MyUtility.Msg.WaitWindows("Data Loading....");
                Ict.DualResult result;
                if (result = DBProxy.Current.Select(null, strSQLCmd.ToString(), cmds, out dtScrap))
                {
                    if (dtScrap.Rows.Count == 0)
                    { MyUtility.Msg.WarningBox("Data not found!!"); }
                    else
                    {
                        dtScrap.DefaultView.Sort = "poid,seq1,seq2,location,dyelot";
                    }
                    listControlBindingSource1.DataSource = dtScrap;
                }
                else { ShowErr(strSQLCmd.ToString(), result); }
                MyUtility.Msg.WaitClear();
            }
        }
        //Form Load
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            #region -- Transfer Qty Valid --
            Ict.Win.DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            ns.CellValidating += (s, e) =>
                {
                    if (this.EditMode && !MyUtility.Check.Empty(e.FormattedValue))
                    {
                        grid1.GetDataRow(grid1.GetSelectedRowIndex())["qty"] = e.FormattedValue;
                        grid1.GetDataRow(grid1.GetSelectedRowIndex())["selected"] = true;
                    }
                };
            #endregion
            
            Ict.Win.DataGridViewGeneratorTextColumnSettings ns2 = new DataGridViewGeneratorTextColumnSettings();
            ns2.CellFormatting = (s, e) =>
            {
                DataRow dr = grid1.GetDataRow(e.RowIndex);
                switch (dr["StockType"].ToString())
                {
                    case "B":
                        e.Value = "Bulk";
                        break;
                    case "I":
                        e.Value = "Inventory";
                        break;
                    case "O":
                        e.Value = "Obsolete";
                        break;
                }
            };

            this.grid1.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.grid1.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.grid1)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)   //0
                .Text("exportid", header: "WK#", iseditingreadonly: true, width: Widths.AnsiChars(14)) //1
                .Text("eta", header: "ETA", iseditingreadonly: true, width: Widths.AnsiChars(10)) //2
                .Text("poid", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(13)) //3
                .Text("seq", header: "SEQ", iseditingreadonly: true, width: Widths.AnsiChars(6)) //4
                .Text("roll", header: "Roll#", iseditingreadonly: true, width: Widths.AnsiChars(8)) //5
                .Text("dyelot", header: "Dyelot", iseditingreadonly: true, width: Widths.AnsiChars(6)) //6
                .Text("stocktype", header: "stocktype", iseditingreadonly: true, width: Widths.AnsiChars(6), settings: ns2) //7
                .Numeric("balance", header: "Balance" + Environment.NewLine + "Qty", iseditable: false, decimal_places: 2, integer_places: 10, width: Widths.AnsiChars(6)) //8
                .Numeric("Qty", header: "Return" + Environment.NewLine + "Qty", decimal_places: 2, integer_places: 10, settings: ns, width: Widths.AnsiChars(6))  //9
                .Text("location", header: "Location", width: Widths.AnsiChars(30), iseditingreadonly: true)    //10
                .EditText("Description", header: "Description", iseditingreadonly: true, width: Widths.AnsiChars(20)) //11
               ;

            this.grid1.Columns[9].DefaultCellStyle.BackColor = Color.Pink;

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

        //Close
        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Import
        private void button2_Click(object sender, EventArgs e)
        {
            string remark = "";
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
            if (dr2.Length > 0)
            {
                MyUtility.Msg.WarningBox("Scrap Qty of selected row can't be zero!", "Warning");
                return;
            }

            dr2 = dtGridBS1.Select("qty <> 0 and Selected = 1");
            foreach (DataRow tmp in dr2)
            {
                DataRow[] findrow = dt_detail.Select(string.Format(@"FtyinventoryUkey = {0} "
                    ,  tmp["FtyInventoryUkey"]));

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
                if (!remark.Contains(tmp["exportid"].ToString()))
                {
                    remark += tmp["exportid"].ToString() + ",";
                }
            }
            dr_master["remark"] = remark;
            this.Close();
        }

        //SP# Valid
        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            string sp = textBox1.Text.TrimEnd();
            string seq = textBox2.Text.PadRight(5, ' ');

            if (MyUtility.Check.Empty(sp)) return;

            if (MyUtility.Check.Empty(textBox2.Text.TrimEnd()))
            {
                if (!MyUtility.Check.Seek(string.Format("select 1 where exists(select * from ftyinventory where poid ='{0}' and mdivisionid='{1}')"
                    , sp, Sci.Env.User.Keyword), null))
                {
                    MyUtility.Msg.WarningBox("SP# is not found!!");
                    e.Cancel = true;
                    return;
                }
            }
            else
            {
                if (!MyUtility.Check.Seek(string.Format(@"select 1 where exists(select * from mdivisionpodetail where poid ='{0}' 
                        and seq1 = '{1}' and seq2 = '{2}' and mdivisionid='{3}')", sp, seq.Substring(0, 3), seq.Substring(3, 2), Sci.Env.User.Keyword), null))
                {
                    MyUtility.Msg.WarningBox("SP#-Seq is not found!!");
                    e.Cancel = true;
                    return;
                }
            }

        }

        //Seq Valid
        private void textBox2_Validating(object sender, CancelEventArgs e)
        {
            string sp = textBox1.Text.TrimEnd();
            if (MyUtility.Check.Empty(sp) || MyUtility.Check.Empty(textBox2.Text.TrimEnd())) return;
            string seq = textBox2.Text.PadRight(5, ' ');

            if (!MyUtility.Check.Seek(string.Format(@"select 1 where exists(select * from po_supp_detail where id ='{0}' 
                        and seq1 = '{1}' and seq2 = '{2}')", sp, seq.Substring(0, 3), seq.Substring(3, 2)), null))
            {
                MyUtility.Msg.WarningBox("SP#-Seq is not found!!");
                e.Cancel = true;
                return;
            }

        }

    }
}
