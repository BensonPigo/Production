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
using System.Linq;

namespace Sci.Production.Warehouse
{
    public partial class P26_Import : Sci.Win.Subs.Base
    {
        DataRow dr_master;
        DataTable dt_detail;
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        protected DataTable dtArtwork;

        public P26_Import(DataRow master, DataTable detail)
        {
            InitializeComponent();
            dr_master = master;
            dt_detail = detail;
            this.EditMode = true;
        }

        //Button Query
        private void button1_Click(object sender, EventArgs e)
        {
            StringBuilder strSQLCmd = new StringBuilder();
            String sp = this.textBox1.Text.TrimEnd();
            String seq = this.textBox2.Text.TrimEnd();
            String refno = this.textBox5.Text.TrimEnd();
            String locationid = this.textBox6.Text.TrimEnd();
            String dyelot = this.textBox7.Text.TrimEnd();
            String transid = this.textBox4.Text.TrimEnd();

            switch (radioPanel1.Value)
            {
                case "1":
                    if (string.IsNullOrWhiteSpace(sp))
                    {
                        MyUtility.Msg.WarningBox("< SP# > can't be empty!!");
                        textBox1.Focus();
                        return;
                    }
                        
                    else
                    {
                        // 建立可以符合回傳的Cursor
                        strSQLCmd.Append(string.Format(@"select distinct 0 as selected,a.mdivisionid,a.Poid,a.seq1,a.seq2,left(a.seq1+' ',3)+a.Seq2 as seq
,a.Roll,a.Dyelot,a.InQty - a.OutQty + a.AdjustQty qty,a.Ukey ftyinventoryukey
,dbo.getmtldesc(a.poid,a.seq1,a.seq2,2,0) as [description] 
,stuff((select ',' + t.mtllocationid from (select mtllocationid from dbo.ftyinventory_detail where ukey = a.ukey) t for xml path('')), 1, 1, '') fromlocation
,'' tolocation, '' id
,p1.refno
,p1.colorid
,p1.sizespec
from dbo.FtyInventory a
    left join dbo.FtyInventory_Detail b on a.Ukey = b.Ukey
    left join dbo.PO_Supp_Detail p1 on p1.ID = a.PoId and p1.seq1 = a.SEQ1 and p1.SEQ2 = a.seq2
where A.StockType='{1}' AND  A.Lock = 0 and a.InQty - a.OutQty + a.AdjustQty > 0
and a.poid='{0}'", sp, dr_master["stocktype"].ToString())); // 
                        if (!MyUtility.Check.Empty(seq))
                        {
                            strSQLCmd.Append(string.Format(@" and a.seq1 = '{0}' and a.seq2='{1}'", seq.Substring(0, 3), seq.Substring(3, 2)));
                        }
                        if (!MyUtility.Check.Empty(refno))
                        {
                            strSQLCmd.Append(string.Format(@" and (select refno from dbo.PO_Supp_Detail P where P.id = a.poid and P.seq1 = a.seq1 and P.seq2 = a.seq2 )='{0}'", refno));
                        }
                        if (!MyUtility.Check.Empty(locationid))
                        {
                            strSQLCmd.Append(string.Format(@" and b.MtlLocationID = '{0}' ", locationid));
                        }
                        if (!MyUtility.Check.Empty(dyelot))
                        {
                            strSQLCmd.Append(string.Format(@" and a.dyelot='{0}' ", dyelot));
                        }
                    }
                    break;

                case "2":
                    if (string.IsNullOrWhiteSpace(transid))
                    {
                        MyUtility.Msg.WarningBox("< Transaction ID# > can't be empty!!");
                        textBox4.Focus();
                        return;
                    }
                    strSQLCmd.Append(string.Format(@"select 0 as selected,a.mdivisionid,a.Poid,a.seq1,a.seq2,left(a.seq1+' ',3)+a.Seq2 as seq,a.Roll,a.Dyelot,a.InQty - a.OutQty + a.AdjustQty qty,a.Ukey
,dbo.getmtldesc(a.poid,a.seq1,a.seq2,2,0) as [description] 
,stuff((select ',' + t.mtllocationid from (select mtllocationid from dbo.ftyinventory_detail where dbo.ftyinventory_detail.ukey = a.ukey) t for xml path('')), 1, 1, '') as fromlocation
,'' tolocation
, '' id
,(select refno from dbo.PO_Supp_Detail P where P.id = a.poid and P.seq1 = a.seq1 and P.seq2 = a.seq2 ) refno
,a.ukey as ftyinventoryukey
from dbo.Receiving r1 
    inner join dbo.Receiving_Detail r2 on r2.id = r1.Id
    inner join dbo.FtyInventory a on a.mdivisionid = r2.mdivisionid and a.Poid = r2.PoId and a.Seq1 = r2.seq1 and a.seq2  = r2.seq2 and a.Roll = r2.Roll and a.stocktype = r2.stocktype
where A.StockType='{1}' AND  A.Lock = 0 and a.InQty - a.OutQty + a.AdjustQty > 0 and r1.Status = 'Confirmed'
and r1.id = '{0}'
union all
select 0 as selected,a.mdivisionid,a.Poid,a.seq1,a.seq2,left(a.seq1+' ',3)+a.Seq2 as seq,a.Roll,a.Dyelot,a.InQty - a.OutQty + a.AdjustQty qty,a.Ukey
,dbo.getmtldesc(a.poid,a.seq1,a.seq2,2,0) as [description] 
,stuff((select ',' + t.mtllocationid from (select mtllocationid from dbo.ftyinventory_detail where dbo.ftyinventory_detail.ukey = a.ukey) t for xml path('')), 1, 1, '') as fromlocation
,'' tolocation
, '' id
,a.ukey ftyinventoryukey
,(select refno from dbo.PO_Supp_Detail P where P.id = a.poid and P.seq1 = a.seq1 and P.seq2 = a.seq2) refno
from dbo.SubTransfer r1 
    inner join dbo.SubTransfer_Detail r2 on r2.id = r1.Id
    inner join dbo.FtyInventory a on a.ukey = r2.fromftyinventoryukey
where A.StockType='{1}' AND  A.Lock = 0 and a.InQty - a.OutQty + a.AdjustQty > 0 and r1.Status = 'Confirmed'
and r1.id = '{0}'
union all
select 0 as selected,a.mdivisionid,a.Poid,a.seq1,a.seq2,left(a.seq1+' ',3)+a.Seq2 as seq,a.Roll,a.Dyelot,a.InQty - a.OutQty + a.AdjustQty qty,a.Ukey
,dbo.getmtldesc(a.poid,a.seq1,a.seq2,2,0) as [description] 
,stuff((select ',' + t.mtllocationid from (select mtllocationid from dbo.ftyinventory_detail where dbo.ftyinventory_detail.ukey = a.ukey) t for xml path('')), 1, 1, '') as fromlocation
,'' tolocation, '' id
,a.ukey ftyinventoryukey
,(select refno from dbo.PO_Supp_Detail P where P.id = a.poid and P.seq1 = a.seq1 and P.seq2 = a.seq2 ) refno
from dbo.Issue r1 
    inner join dbo.Issue_Detail r2 on r2.id = r1.Id
    inner join dbo.FtyInventory a on a.ukey = r2.ftyinventoryukey
where A.StockType='{1}' AND  A.Lock = 0 and a.InQty - a.OutQty + a.AdjustQty > 0 and r1.Status = 'Confirmed'
and r1.id = '{0}'
union all
select 0 as selected,a.mdivisionid,a.Poid,a.seq1,a.seq2,left(a.seq1+' ',3)+a.Seq2 as seq,a.Roll,a.Dyelot,a.InQty - a.OutQty + a.AdjustQty qty,a.Ukey
,dbo.getmtldesc(a.poid,a.seq1,a.seq2,2,0) as [description] 
,stuff((select ',' + t.mtllocationid from (select mtllocationid from dbo.ftyinventory_detail where dbo.ftyinventory_detail.ukey = a.ukey) t for xml path('')), 1, 1, '') as fromlocation
,'' tolocation, '' id
,a.ukey ftyinventoryukey
,(select refno from dbo.PO_Supp_Detail P where P.id = a.poid and P.seq1 = a.seq1 and P.seq2 = a.seq2 ) refno
from dbo.ReturnReceipt r1 
    inner join dbo.ReturnReceipt_Detail r2 on r2.id = r1.Id
    inner join dbo.FtyInventory a on a.ukey = r2.ftyinventoryukey
where A.StockType='{1}' AND  A.Lock = 0 and a.InQty - a.OutQty + a.AdjustQty > 0 and r1.Status = 'Confirmed'
and r1.id = '{0}'
union
select 0 as selected,a.mdivisionid,a.Poid,a.seq1,a.seq2,left(a.seq1+' ',3)+a.Seq2 as seq,a.Roll,a.Dyelot,a.InQty - a.OutQty + a.AdjustQty qty,a.Ukey
,dbo.getmtldesc(a.poid,a.seq1,a.seq2,2,0) as [description] 
,stuff((select ',' + t.mtllocationid from (select mtllocationid from dbo.ftyinventory_detail where dbo.ftyinventory_detail.ukey = a.ukey) t for xml path('')), 1, 1, '') as fromlocation
,'' tolocation, '' id
,a.ukey ftyinventoryukey 
,(select refno from dbo.PO_Supp_Detail P where P.id = a.poid and P.seq1 = a.seq1 and P.seq2 = a.seq2 ) refno
from dbo.TransferIn r1 
    inner join dbo.TransferIn_Detail r2 on r2.id = r1.Id
    inner join dbo.FtyInventory a on a.mdivisionid = r2.mdivisionid and a.Poid = r2.PoId and a.Seq1 = r2.seq1 and a.seq2  = r2.seq2 and a.Roll = r2.Roll and a.stocktype = r2.stocktype
where A.StockType='{1}' AND  A.Lock = 0 and a.InQty - a.OutQty + a.AdjustQty > 0 and r1.Status = 'Confirmed'
and r1.id = '{0}' ", transid, dr_master["stocktype"].ToString())); // 

                    break;
            }


            MyUtility.Msg.WaitWindows("Data Loading....");
            Ict.DualResult result;
            if (!(result = DBProxy.Current.Select(null, strSQLCmd.ToString(), out dtArtwork)))
            {
                ShowErr(strSQLCmd.ToString(), result);
            }
            else
            {
                if (dtArtwork.Rows.Count == 0)
                { MyUtility.Msg.WarningBox("Data not found!!"); }

                listControlBindingSource1.DataSource = dtArtwork;
                //dtArtwork.DefaultView.Sort = "seq1,seq2,location,dyelot,balance desc";
            }
            MyUtility.Msg.WaitClear();

        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            textBox1.Focus();
            #region Location 右鍵開窗
            Ict.Win.DataGridViewGeneratorTextColumnSettings ts2 = new DataGridViewGeneratorTextColumnSettings();
            ts2.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    DataRow currentrow = grid1.GetDataRow(grid1.GetSelectedRowIndex());
                    Sci.Win.Tools.SelectItem2 item = PublicPrg.Prgs.SelectLocation(dr_master["Stocktype"].ToString(), currentrow["ToLocation"].ToString());
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    currentrow["ToLocation"] = item.GetSelectedString();
                }
            };

            ts2.CellValidating += (s, e) =>
            {
                if (this.EditMode && e.FormattedValue != null)
                {
                    DataRow dr = grid1.GetDataRow(e.RowIndex);
                    dr["ToLocation"] = e.FormattedValue;
                    string sqlcmd = string.Format(@"SELECT id,Description,StockType FROM DBO.MtlLocation WHERE StockType='{0}' and mdivisionid='{1}'", dr_master["Stocktype"].ToString(), Sci.Env.User.Keyword);
                    DataTable dt;
                    DBProxy.Current.Select(null, sqlcmd, out dt);
                    string[] getLocation = dr["ToLocation"].ToString().Split(',').Distinct().ToArray();
                    bool selectId = true;
                    List<string> errLocation = new List<string>();
                    List<string> trueLocation = new List<string>();
                    foreach (string location in getLocation)
                    {
                        if (!dt.AsEnumerable().Any(row => row["id"].EqualString(location)) && !(location.EqualString("")))
                        {
                            selectId &= false;
                            errLocation.Add(location);
                        }
                        else if (!(location.EqualString("")))
                        {
                            trueLocation.Add(location);
                        }
                    }

                    if (!selectId)
                    {
                        MyUtility.Msg.WarningBox("Location : " + string.Join(",", (errLocation).ToArray()) + "  Data not found !!", "Data not found");
                        e.Cancel = true;
                    }
                    trueLocation.Sort();
                    dr["ToLocation"] = string.Join(",", (trueLocation).ToArray());
                    //去除錯誤的Location將正確的Location填回
                }
            };
            #endregion Location 右鍵開窗

            this.grid1.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.grid1.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.grid1)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)   //0
                 .Text("poid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)  //1
            .Text("seq", header: "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true)  //2
            .Text("Roll", header: "Roll#", width: Widths.AnsiChars(9), iseditingreadonly: true)    //3
            .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(5), iseditingreadonly: true)    //4
            .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true)    //5
            .Numeric("qty", header: "Qty", width: Widths.AnsiChars(10), decimal_places: 2, integer_places: 10, iseditingreadonly: true)    //6
            .Text("FromLocation", header: "FromLocation", iseditingreadonly: true)    //7
            .Text("ToLocation", header: "ToLocation", settings: ts2, iseditingreadonly: false)    //8
            ;
            this.grid1.Columns[8].DefaultCellStyle.BackColor = Color.Pink;

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
            listControlBindingSource1.EndEdit();
            grid1.ValidateControl();
            DataTable dtGridBS1 = (DataTable)listControlBindingSource1.DataSource;
            if (MyUtility.Check.Empty(dtGridBS1) || dtGridBS1.Rows.Count == 0) return;

            DataRow[] dr2 = dtGridBS1.Select("Selected = 1");
            if (dr2.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select rows first!", "Warnning");
                return;
            }

            foreach (DataRow tmp in dr2)
            {
                DataRow[] findrow = dt_detail.Select(string.Format("poid = '{0}' and seq1 = '{1}' and seq2 = '{2}' and roll ='{3}'and dyelot='{4}'"
                    , tmp["poid"].ToString(), tmp["seq1"].ToString(), tmp["seq2"].ToString(), tmp["roll"].ToString(), tmp["dyelot"].ToString()));

                if (findrow.Length > 0)
                {
                    findrow[0]["qty"] = tmp["qty"];
                    findrow[0]["tolocation"] = tmp["tolocation"];
                    findrow[0]["fromlocation"] = tmp["fromlocation"];
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

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            string sp = textBox1.Text.TrimEnd();
            string seq = textBox2.Text.PadRight(5, ' ');

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
                if (!MyUtility.Check.Seek(string.Format(@"select 1 where exists(select * from po_supp_detail where id ='{0}' 
                        and seq1 = '{1}' and seq2 = '{2}')", sp, seq.Substring(0, 3), seq.Substring(3, 2)), null))
                {
                    MyUtility.Msg.WarningBox("SP#-Seq is not found!!");
                    e.Cancel = true;
                    return;
                }
            }

        }

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

        private void textBox3_MouseDown(object sender, MouseEventArgs e)
        {
            Sci.Win.Tools.SelectItem2 item = PublicPrg.Prgs.SelectLocation("B", "");
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel) { return; }
            textBox3.Text = item.GetSelectedString();
        }

        private void radioPanel1_ValueChanged(object sender, EventArgs e)
        {
            Sci.Win.UI.RadioPanel rdoG = (Sci.Win.UI.RadioPanel)sender;
            switch (rdoG.Value)
            {
                case "1":
                    textBox1.ReadOnly = false;
                    textBox2.ReadOnly = false;
                    textBox5.ReadOnly = false;
                    textBox6.ReadOnly = false;
                    textBox7.ReadOnly = false;
                    textBox4.ReadOnly = true;
                    textBox4.Text = "";
                    break;
                case "2":
                    textBox1.ReadOnly = true;
                    textBox2.ReadOnly = true;
                    textBox5.ReadOnly = true;
                    textBox6.ReadOnly = true;
                    textBox7.ReadOnly = true;
                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox5.Text = "";
                    textBox6.Text = "";
                    textBox7.Text = "";
                    textBox4.ReadOnly = false;
                    break;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            listControlBindingSource1.EndEdit();
            DataTable dt = (DataTable)listControlBindingSource1.DataSource;
            if (dt == null || dt.Rows.Count == 0) return;
            DataRow[] drfound = dt.Select("selected = 1");

            foreach (var item in drfound)
            {
                item["tolocation"] = this.textBox3.Text;
            }
        }
    }
}
