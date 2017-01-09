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
using System.Linq;

namespace Sci.Production.Warehouse
{
    public partial class P36_Import : Sci.Win.Subs.Base
    {
        DataRow dr_master;
        DataTable dt_detail;
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        protected DataTable dtScrap;

        public P36_Import(DataRow master, DataTable detail)
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

            if (string.IsNullOrWhiteSpace(sp))
            {
                MyUtility.Msg.WarningBox("< SP# > can't be empty!!");
                textBox1.Focus();
                return;
            }

            else
            {
                // 建立可以符合回傳的Cursor
                #region -- SQL Command --
                strSQLCmd.Append(string.Format(@"select 0 as selected 
,'' id
, c.ukey as FromFtyinventoryUkey
, c.mdivisionid  as fromMdivisionid
, a.id as fromPoId
,a.Seq1 as fromseq1
,a.Seq2 as fromseq2
,left(a.seq1+' ',3)+a.Seq2 as fromseq
,dbo.getmtldesc(a.id,a.seq1,a.seq2,2,0) as [Description]
,c.Roll as fromRoll
,c.Dyelot as fromDyelot
,c.StockType as fromStocktype
,c.inqty-c.outqty + c.adjustqty as balance
,0.00 as qty
,isnull(stuff((select ',' + cast(mtllocationid as varchar) 
		from (select mtllocationid from ftyinventory_detail where ukey = c.ukey)t for xml path('')), 1, 1, ''),'') as location
,a.FabricType
,a.stockunit
,a.InputQty
,a.id as topoid
,a.SEQ1 as toseq1
,a.SEQ2 as toseq2
,c.Roll as toroll
,c.Dyelot as todyelot
,'I' as toStocktype
,'' tolocation
,'{0}' as toMdivisionid
from dbo.PO_Supp_Detail a 
inner join dbo.ftyinventory c on c.poid = a.id and c.seq1 = a.seq1 and c.seq2  = a.seq2 
Where c.lock = 0 and c.InQty-c.OutQty+c.AdjustQty > 0 and c.stocktype = 'O' and c.mdivisionid='{0}'", Sci.Env.User.Keyword));
                #endregion

                System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
                sp1.ParameterName = "@sp1";

                System.Data.SqlClient.SqlParameter seq1 = new System.Data.SqlClient.SqlParameter();
                seq1.ParameterName = "@seq1";

                System.Data.SqlClient.SqlParameter seq2 = new System.Data.SqlClient.SqlParameter();
                seq2.ParameterName = "@seq2";

                IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();

                if (!MyUtility.Check.Empty(sp))
                {
                    strSQLCmd.Append(@" and a.id = @sp1 ");
                    sp1.Value = sp;
                    cmds.Add(sp1);
                }

                if (!MyUtility.Check.Empty(seq))
                {
                    strSQLCmd.Append(@" and a.seq1 = @seq1 and a.seq2 = @seq2");
                    seq1.Value = seq.Substring(0, 3);
                    seq2.Value = seq.Substring(3, 2);
                    cmds.Add(seq1);
                    cmds.Add(seq2);
                }

                this.ShowWaitMessage("Data Loading....");
                Ict.DualResult result;
                if (result = DBProxy.Current.Select(null, strSQLCmd.ToString(), cmds, out dtScrap))
                {
                    if (dtScrap.Rows.Count == 0)
                    { MyUtility.Msg.WarningBox("Data not found!!"); }
                    else
                    {
                        dtScrap.DefaultView.Sort = "fromseq1,fromseq2,location,fromdyelot";
                    }
                    listControlBindingSource1.DataSource = dtScrap;
                }
                else { ShowErr(strSQLCmd.ToString(), result); }
                this.HideWaitMessage();
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
            #region -- toLocation 右鍵開窗 --

            Ict.Win.DataGridViewGeneratorTextColumnSettings ts2 = new DataGridViewGeneratorTextColumnSettings();
            ts2.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    Sci.Win.Tools.SelectItem2 item = Prgs.SelectLocation(grid1.GetDataRow(grid1.GetSelectedRowIndex())["tostocktype"].ToString()
                        , grid1.GetDataRow(grid1.GetSelectedRowIndex())["tolocation"].ToString());
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    grid1.GetDataRow(grid1.GetSelectedRowIndex())["tolocation"] = item.GetSelectedString();
                }
            };

            ts2.CellValidating += (s, e) =>
            {
                if (this.EditMode && e.FormattedValue != null)
                {
                    DataRow dr = grid1.GetDataRow(e.RowIndex);
                    dr["ToLocation"] = e.FormattedValue;
                    string sqlcmd = string.Format(@"SELECT id,Description,StockType FROM DBO.MtlLocation WHERE StockType='{0}' and mdivisionid='{1}'", dr["ToStocktype"].ToString(), Sci.Env.User.Keyword);
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
            #endregion

            this.grid1.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.grid1.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.grid1)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)   //0
                .Text("frompoid", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(14)) //1
                .Text("fromseq", header: "Seq#", iseditingreadonly: true, width: Widths.AnsiChars(6)) //2
                .Text("fromroll", header: "Roll", iseditingreadonly: true, width: Widths.AnsiChars(6)) //3
                .Text("fromdyelot", header: "Dyelot", iseditingreadonly: true, width: Widths.AnsiChars(6)) //4
                .EditText("Description", header: "Description", iseditingreadonly: true, width: Widths.AnsiChars(20)) //5
                .Text("stockunit", header: "Unit", iseditingreadonly: true, width: Widths.AnsiChars(6)) //6
                .Numeric("balance", header: "Balance" + Environment.NewLine + "Qty", iseditable: false, decimal_places: 2, integer_places: 10, width: Widths.AnsiChars(6)) //7
                .Numeric("Qty", header: "Transfer" + Environment.NewLine + "Qty", decimal_places: 2, integer_places: 10, settings: ns, width: Widths.AnsiChars(6))  //8
                .Text("tolocation", header: "Location", iseditingreadonly: false, settings: ts2, width: Widths.AnsiChars(20))    //9
               ;

            this.grid1.Columns[8].DefaultCellStyle.BackColor = Color.Pink;
            this.grid1.Columns[9].DefaultCellStyle.BackColor = Color.Pink;
            //this.grid1.Columns[].DefaultCellStyle.BackColor = Color.Pink;
        }

        //Close
        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Import
        private void button2_Click(object sender, EventArgs e)
        {
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
                MyUtility.Msg.WarningBox("Transfer Qty of selected row can't be zero!", "Warning");
                return;
            }

            dr2 = dtGridBS1.Select("qty <> 0 and Selected = 1");
            foreach (DataRow tmp in dr2)
            {
                DataRow[] findrow = dt_detail.Select(string.Format(@"FromFtyinventoryUkey = {7} and tomdivisionid = '{0}' and topoid = '{1}' and toseq1 = '{2}' and toseq2 = '{3}' 
                        and toroll ='{4}'and todyelot='{5}' and tostocktype='{6}'"
                    , tmp["toMdivisionid"], tmp["topoid"], tmp["toseq1"], tmp["toseq2"], tmp["toroll"], tmp["todyelot"], tmp["tostocktype"], tmp["fromFtyInventoryUkey"]));

                if (findrow.Length > 0)
                {
                    findrow[0]["qty"] = tmp["qty"];
                    findrow[0]["tolocation"] = tmp["tolocation"];
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

        //Update All
        private void button4_Click(object sender, EventArgs e)
        {
            //grid1.ValidateControl();
            listControlBindingSource1.EndEdit();

            if (dtScrap == null || dtScrap.Rows.Count == 0) return;
            DataRow[] drfound = dtScrap.Select("selected = 1");

            foreach (var item in drfound)
            {
                item["tolocation"] = displayBox1.Text;
            }
        }

        private void displayBox1_MouseDown(object sender, MouseEventArgs e)
        {
            #region Location 右鍵開窗

            if (this.EditMode && e.Button == MouseButtons.Right)
            {
                Sci.Win.Tools.SelectItem2 item = Prgs.SelectLocation("I", displayBox1.Text);
                DialogResult result = item.ShowDialog();
                if (result == DialogResult.Cancel) { return; }
                displayBox1.Text = item.GetSelectedString();
            }

            #endregion Location 右鍵開窗
        }
    }
}
