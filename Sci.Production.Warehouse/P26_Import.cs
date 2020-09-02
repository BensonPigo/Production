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
    public partial class P26_Import : Win.Subs.Base
    {
        private DataRow dr_master;
        private DataTable dt_detail;
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        protected DataTable dtArtwork;
        private Dictionary<string, string> selectedLocation = new Dictionary<string, string>();

        public P26_Import(DataRow master, DataTable detail)
        {
            this.InitializeComponent();
            this.dr_master = master;
            this.dt_detail = detail;
            this.EditMode = true;

            Dictionary<string, string> comboBox1_RowSource = new Dictionary<string, string>();
            comboBox1_RowSource.Add(string.Empty, "All");
            comboBox1_RowSource.Add("F", "Fabric");
            comboBox1_RowSource.Add("A", "Accessory");
            this.cmbMaterialType.DataSource = new BindingSource(comboBox1_RowSource, null);
            this.cmbMaterialType.ValueMember = "Key";
            this.cmbMaterialType.DisplayMember = "Value";

            DataTable dt = (DataTable)this.comboStockType.DataSource;
            dt.Rows.InsertAt(dt.NewRow(), 0);
            this.comboStockType.SelectedIndex = 0;
        }

        // Button Query
        private void BtnQuery_Click(object sender, EventArgs e)
        {
            StringBuilder strSQLCmd = new StringBuilder();
            string sp = this.txtSPNo.Text.TrimEnd();
            string refno = this.txtRef.Text.TrimEnd();
            string color = this.txtColor.Text.TrimEnd();
            string roll = this.txtRoll.Text.TrimEnd();
            string dyelot = this.txtDyelot.Text.TrimEnd();
            string transid = this.txtTransactionID.Text.TrimEnd();
            string wk = this.txtWk.Text.TrimEnd();
            string locationid = this.txtLocation.Text.TrimEnd();
            string materialType = this.cmbMaterialType.SelectedValue.ToString();
            string stockType = this.comboStockType.SelectedValue.ToString();

            // SP#, Transaction ID, Ref#,Location#,WK#不可同時為空
            bool sql1 = false;
            if (MyUtility.Check.Empty(sp) && MyUtility.Check.Empty(transid) && MyUtility.Check.Empty(locationid) && MyUtility.Check.Empty(refno) && MyUtility.Check.Empty(wk))
            {
                MyUtility.Msg.WarningBox("< SP# >, < Transaction ID# >, < Ref# >, < Location >, <WK#> can't all empty!!");
                return;
            }

            if (!MyUtility.Check.Empty(sp) || !MyUtility.Check.Empty(locationid) || !MyUtility.Check.Empty(refno) || !MyUtility.Check.Empty(wk))
            {
                sql1 = true;

                // 建立可以符合回傳的Cursor
                strSQLCmd.Append(string.Format(
                    @"
select   0 as selected
        , r.exportID
        , a.Poid
        , a.seq1
        , a.seq2
        , concat(Ltrim(Rtrim(a.seq1)), ' ', a.Seq2) as seq
        , a.Roll
        , a.Dyelot
        , a.InQty - a.OutQty + a.AdjustQty qty
        , a.Ukey
        , dbo.getmtldesc(a.poid,a.seq1,a.seq2,2,0) as [description] 
        , dbo.Getlocation(a.ukey) fromlocation
        , '' tolocation
        , '' id
		, a.Ukey ftyinventoryukey
        , p1.refno
        , p1.colorid
        , p1.sizespec
        , Receiving_Detail_ukey = min(rd.Ukey) 		
        , a.StockType
        , [LastEditDate] = LastEditDate.val
from dbo.FtyInventory a WITH (NOLOCK) 
left join dbo.FtyInventory_Detail b WITH (NOLOCK) on a.Ukey = b.Ukey
left join dbo.PO_Supp_Detail p1 WITH (NOLOCK) on p1.ID = a.PoId and p1.seq1 = a.SEQ1 and p1.SEQ2 = a.seq2
inner join dbo.Factory f on f.ID=p1.factoryID 
left join dbo.Receiving_Detail rd  WITH (NOLOCK) on rd.POID = a.POID and rd.Seq1 = a.Seq1 and rd.Seq2 = a.Seq2 and rd.StockType = a.StockType and rd.Roll = a.Roll and rd.Dyelot = a.Dyelot
left join dbo.Receiving r WITH (NOLOCK) on r.Id = rd.Id
outer apply(SELECT top 1 [val] = lt.EditDate
            FROM LocationTrans lt with (nolock)
            INNER JOIN LocationTrans_detail ltd with (nolock) ON lt.ID = ltd.ID
            WHERE lt.Status='Confirmed' AND ltd.FtyInventoryUkey = a.Ukey 
            order by lt.EditDate desc ) LastEditDate
where    f.MDivisionID='{0}' 
", Env.User.Keyword));

                if (!MyUtility.Check.Empty(sp))
                {
                    strSQLCmd.Append(string.Format(
                        @" 
        and a.poid like '%{0}%' ", sp));
                }

                if (!this.txtSeq.CheckSeq1Empty() && this.txtSeq.CheckSeq2Empty())
                {
                    strSQLCmd.Append(string.Format(
                        @" 
        and a.seq1 = '{0}'", this.txtSeq.Seq1));
                }
                else if (!this.txtSeq.CheckEmpty(showErrMsg: false))
                {
                    strSQLCmd.Append(string.Format(
                        @" 
        and a.seq1 = '{0}' and a.seq2='{1}'", this.txtSeq.Seq1, this.txtSeq.Seq2));
                }

                if (!MyUtility.Check.Empty(refno))
                {
                    strSQLCmd.Append(string.Format(
                        @" 
        and (select refno from dbo.PO_Supp_Detail P WITH (NOLOCK) where P.id = a.poid and P.seq1 = a.seq1 and P.seq2 = a.seq2 )='{0}'", refno));
                }

                if (!MyUtility.Check.Empty(locationid))
                {
                    strSQLCmd.Append(string.Format(
                        @" 
        and b.MtlLocationID = '{0}' ", locationid));
                }

                if (!MyUtility.Check.Empty(dyelot))
                {
                    strSQLCmd.Append(string.Format(
                        @" 
        and a.dyelot like '%{0}%' ", dyelot));
                }

                if (!MyUtility.Check.Empty(materialType))
                {
                    strSQLCmd.Append(string.Format(
                        @" 
        and p1.FabricType='{0}' ", materialType));
                }

                if (!MyUtility.Check.Empty(stockType))
                {
                    strSQLCmd.Append(string.Format(
                        @" 
        and a.StockType = {0} ", stockType));
                }

                if (!MyUtility.Check.Empty(wk))
                {
                    strSQLCmd.Append(string.Format(
                        @" 
        and r.ExportId='{0}'", wk));
                }

                if (!MyUtility.Check.Empty(color))
                {
                    strSQLCmd.Append(string.Format(
                        @" 
        and p1.ColorID='{0}'", color));
                }

                if (!MyUtility.Check.Empty(roll))
                {
                    strSQLCmd.Append(string.Format(
                        @" 
        and a.Roll like '%{0}%' ", roll));
                }

                strSQLCmd.Append(@" 
group by a.Poid, a.seq1, a.seq2, a.Roll, a.Dyelot, a.InQty , a.OutQty , a.AdjustQty, a.Ukey, p1.refno, p1.colorid, p1.sizespec, a.StockType, r.exportID, LastEditDate.val");
            }

            // break;

            // case "2":
            if (!MyUtility.Check.Empty(transid))
            {
                if (sql1)
                {
                    strSQLCmd.Append(" union ");
                }

                strSQLCmd.Append(string.Format(
                    @"
select  ul.*,
        [LastEditDate] = LastEditDate.val
from(
select  0 as selected
        , r1.exportID
        , a.Poid
        , a.seq1
        , a.seq2
        , concat(Ltrim(Rtrim(a.seq1)), ' ', a.Seq2) as seq
        , a.Roll
        , a.Dyelot
        , a.InQty - a.OutQty + a.AdjustQty qty
        , a.Ukey
        , dbo.getmtldesc(a.poid,a.seq1,a.seq2,2,0) as [description] 
        , dbo.Getlocation(a.ukey) as fromlocation
        , '' tolocation
        , '' id
        , a.ukey as ftyinventoryukey
        , (select refno from dbo.PO_Supp_Detail P WITH (NOLOCK) where P.id = a.poid and P.seq1 = a.seq1 and P.seq2 = a.seq2 ) refno
        , (select ColorID from dbo.PO_Supp_Detail P WITH (NOLOCK) where P.id = a.poid and P.seq1 = a.seq1 and P.seq2 = a.seq2 ) ColorID
		,sizespec=''
        , r2.Ukey Receiving_Detail_ukey
        , a.StockType
from dbo.Receiving r1 WITH (NOLOCK) 
inner join dbo.Receiving_Detail r2 WITH (NOLOCK) on r2.id = r1.Id
inner join dbo.FtyInventory a WITH (NOLOCK) on a.Poid = r2.PoId and a.Seq1 = r2.seq1 and a.seq2  = r2.seq2 and a.Roll = r2.Roll and a.stocktype = r2.stocktype and r2.Roll = a.Roll and r2.Dyelot = a.Dyelot
where   
        r1.Status = 'Confirmed' 
        and r1.Status = 'Confirmed' 
        and r1.mdivisionid='{2}'
        and r1.id = '{0}'
        {1}
union all
select  0 as selected
        , r.exportID
        , a.Poid
        , a.seq1
        , a.seq2
        , concat(Ltrim(Rtrim(a.seq1)), ' ', a.Seq2) as seq
        , a.Roll
        , a.Dyelot
        , a.InQty - a.OutQty + a.AdjustQty qty
        , a.Ukey
        , dbo.getmtldesc(a.poid,a.seq1,a.seq2,2,0) as [description] 
        , dbo.Getlocation(a.ukey) as fromlocation
        , '' tolocation
        , '' id
        , a.ukey ftyinventoryukey
        , (select refno from dbo.PO_Supp_Detail P WITH (NOLOCK) where P.id = a.poid and P.seq1 = a.seq1 and P.seq2 = a.seq2) refno
        , (select ColorID from dbo.PO_Supp_Detail P WITH (NOLOCK) where P.id = a.poid and P.seq1 = a.seq1 and P.seq2 = a.seq2 ) ColorID
		,sizespec=''
        , rd.Ukey Receiving_Detail_ukey
        , a.StockType
from dbo.SubTransfer r1 WITH (NOLOCK) 
inner join dbo.SubTransfer_Detail r2 WITH (NOLOCK) on r2.id = r1.Id
inner join dbo.FtyInventory a WITH (NOLOCK) on a.ukey = r2.fromftyinventoryukey
left join dbo.Receiving_Detail rd  WITH (NOLOCK) on rd.POID = a.POID and rd.Seq1 = a.Seq1 and rd.Seq2 = a.Seq2 and rd.StockType = a.StockType and rd.Roll = a.Roll and rd.Dyelot = a.Dyelot
left join dbo.Receiving r WITH (NOLOCK) on r.Id = rd.Id
where  
        r1.Status = 'Confirmed' 
        and r1.Status = 'Confirmed'
        and r1.mdivisionid='{2}'
        and r1.id = '{0}'
        {1}
union all
select  0 as selected
        , r.exportID
        , a.Poid
        , a.seq1
        , a.seq2
        , concat(Ltrim(Rtrim(a.seq1)), ' ', a.Seq2) as seq
        , a.Roll
        , a.Dyelot
        , a.InQty - a.OutQty + a.AdjustQty qty
        , a.Ukey
        , dbo.getmtldesc(a.poid,a.seq1,a.seq2,2,0) as [description] 
        , dbo.Getlocation(a.ukey) as fromlocation
        , '' tolocation
        ,  '' id
        , a.ukey ftyinventoryukey
        , (select refno from dbo.PO_Supp_Detail P WITH (NOLOCK) where P.id = a.poid and P.seq1 = a.seq1 and P.seq2 = a.seq2 ) refno
        , (select ColorID from dbo.PO_Supp_Detail P WITH (NOLOCK) where P.id = a.poid and P.seq1 = a.seq1 and P.seq2 = a.seq2 ) ColorID
		,sizespec=''
        , rd.Ukey Receiving_Detail_ukey
        , a.StockType
from dbo.Issue r1 WITH (NOLOCK) 
inner join dbo.Issue_Detail r2 WITH (NOLOCK) on r2.id = r1.Id
inner join dbo.FtyInventory a WITH (NOLOCK) on a.ukey = r2.ftyinventoryukey
left join dbo.Receiving_Detail rd  WITH (NOLOCK) on rd.POID = a.POID and rd.Seq1 = a.Seq1 and rd.Seq2 = a.Seq2 and rd.StockType = a.StockType and rd.Roll = a.Roll and rd.Dyelot = a.Dyelot
left join dbo.Receiving r WITH (NOLOCK) on r.Id = rd.Id
where   
        r1.Status = 'Confirmed' 
        and r1.Status = 'Confirmed' 
        and r1.mdivisionid='{2}'
        and r1.id = '{0}'
        {1}
union all
select  0 as selected
        , r.exportID
        , a.Poid
        , a.seq1
        , a.seq2
        , concat(Ltrim(Rtrim(a.seq1)), ' ', a.Seq2) as seq
        , a.Roll
        , a.Dyelot
        , a.InQty - a.OutQty + a.AdjustQty qty
        , a.Ukey
        , dbo.getmtldesc(a.poid,a.seq1,a.seq2,2,0) as [description] 
        , dbo.Getlocation(a.ukey) as fromlocation
        , '' tolocation
        , '' id
        , a.ukey ftyinventoryukey
        , (select refno from dbo.PO_Supp_Detail P WITH (NOLOCK) where P.id = a.poid and P.seq1 = a.seq1 and P.seq2 = a.seq2 ) refno
        , (select ColorID from dbo.PO_Supp_Detail P WITH (NOLOCK) where P.id = a.poid and P.seq1 = a.seq1 and P.seq2 = a.seq2 ) ColorID
		,sizespec=''
        , rd.Ukey Receiving_Detail_ukey
        , a.StockType
from dbo.ReturnReceipt r1 WITH (NOLOCK) 
inner join dbo.ReturnReceipt_Detail r2 WITH (NOLOCK) on r2.id = r1.Id
inner join dbo.FtyInventory a WITH (NOLOCK) on a.ukey = r2.ftyinventoryukey
left join dbo.Receiving_Detail rd  WITH (NOLOCK) on rd.POID = a.POID and rd.Seq1 = a.Seq1 and rd.Seq2 = a.Seq2 and rd.StockType = a.StockType and rd.Roll = a.Roll and rd.Dyelot = a.Dyelot
left join dbo.Receiving r WITH (NOLOCK) on r.Id = rd.Id
where   
        r1.Status = 'Confirmed' 
        and r1.Status = 'Confirmed' 
        and r1.mdivisionid='{2}'
        and r1.id = '{0}'
        {1}
union
select  0 as selected
        , r.exportID
        , a.Poid
        , a.seq1
        , a.seq2
        , concat(Ltrim(Rtrim(a.seq1)), ' ', a.Seq2) as seq
        , a.Roll
        , a.Dyelot
        , a.InQty - a.OutQty + a.AdjustQty qty
        , a.Ukey
        , dbo.getmtldesc(a.poid,a.seq1,a.seq2,2,0) as [description] 
        , dbo.Getlocation(a.ukey) as fromlocation
        , '' tolocation
        , '' id
        , a.ukey ftyinventoryukey 
        , (select refno from dbo.PO_Supp_Detail P WITH (NOLOCK) where P.id = a.poid and P.seq1 = a.seq1 and P.seq2 = a.seq2 ) refno
        , (select ColorID from dbo.PO_Supp_Detail P WITH (NOLOCK) where P.id = a.poid and P.seq1 = a.seq1 and P.seq2 = a.seq2 ) ColorID
		,sizespec=''
        , rd.Ukey Receiving_Detail_ukey
        , a.StockType
from dbo.TransferIn r1 WITH (NOLOCK) 
inner join dbo.TransferIn_Detail r2 WITH (NOLOCK) on r2.id = r1.Id
inner join dbo.FtyInventory a WITH (NOLOCK) on a.Poid = r2.PoId and a.Seq1 = r2.seq1 and a.seq2  = r2.seq2 and a.Roll = r2.Roll and a.stocktype = r2.stocktype
left join dbo.Receiving_Detail rd  WITH (NOLOCK) on rd.POID = a.POID and rd.Seq1 = a.Seq1 and rd.Seq2 = a.Seq2 and rd.StockType = a.StockType and rd.Roll = a.Roll and rd.Dyelot = a.Dyelot
left join dbo.Receiving r WITH (NOLOCK) on r.Id = rd.Id
where  
        r1.Status = 'Confirmed' 
        and r1.mdivisionid='{2}'
        and r1.id = '{0}' 
        {1} 
)ul
outer apply(SELECT top 1 [val] = lt.EditDate
            FROM LocationTrans lt with (nolock)
            INNER JOIN LocationTrans_detail ltd with (nolock) ON lt.ID = ltd.ID
            WHERE lt.Status='Confirmed' AND ltd.FtyInventoryUkey = ul.FtyInventoryUkey 
            order by lt.EditDate desc ) LastEditDate
",
                    transid,
                    MyUtility.Check.Empty(stockType) ? string.Empty : $"and a.StockType = {stockType}",
                    Env.User.Keyword));
            }

            // 增加 order by FtyInventory.POID, FtyInventory.Seq1, FtyInventory.Seq2,Receiving_Detail.Ukey,FtyInventory.StockType
            strSQLCmd.Insert(0, "select * from (");
            strSQLCmd.Append(@"
) a order by Receiving_Detail_ukey,StockType"); // Poid,seq1,seq2

            this.ShowWaitMessage("Data Loading....");
            DualResult result;
            if (!(result = DBProxy.Current.Select(null, strSQLCmd.ToString(), out this.dtArtwork)))
            {
                this.ShowErr(strSQLCmd.ToString(), result);
            }
            else
            {
                if (this.dtArtwork.Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!!");
                }

                this.listControlBindingSource1.DataSource = this.dtArtwork;

                // dtArtwork.DefaultView.Sort = "seq1,seq2,location,dyelot,balance desc";
            }

            this.HideWaitMessage();

            // 全部撈完，再利用Checked change事件，觸發filter過濾資料
            switch (this.BalanceQty.Checked)
            {
                case true:
                    this.BalanceQty.Checked = false;
                    this.BalanceQty.Checked = true;
                    break;
                case false:
                    this.BalanceQty.Checked = true;
                    this.BalanceQty.Checked = false;
                    break;
                default:
                    break;
            }
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.txtSPNo.Focus();
            #region Location 右鍵開窗
            DataGridViewGeneratorTextColumnSettings ts2 = new DataGridViewGeneratorTextColumnSettings();
            ts2.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    DataRow currentrow = this.gridImport.GetDataRow(this.gridImport.GetSelectedRowIndex());
                    Win.Tools.SelectItem2 item = PublicPrg.Prgs.SelectLocation(currentrow["Stocktype"].ToString(), currentrow["ToLocation"].ToString());
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    currentrow["ToLocation"] = item.GetSelectedString();
                    currentrow.EndEdit();
                }
            };

            ts2.CellValidating += (s, e) =>
            {
                if (this.EditMode && e.FormattedValue != null)
                {
                    DataRow dr = this.gridImport.GetDataRow(e.RowIndex);
                    dr["ToLocation"] = e.FormattedValue;
                    string sqlcmd = string.Format(
                        @"
SELECT  id
        , Description
        , StockType 
FROM    DBO.MtlLocation WITH (NOLOCK) 
WHERE   StockType='{0}'
        and junk != '1'", dr["Stocktype"].ToString());
                    DataTable dt;
                    DBProxy.Current.Select(null, sqlcmd, out dt);
                    string[] getLocation = dr["ToLocation"].ToString().Split(',').Distinct().ToArray();
                    bool selectId = true;
                    List<string> errLocation = new List<string>();
                    List<string> trueLocation = new List<string>();
                    foreach (string location in getLocation)
                    {
                        if (!dt.AsEnumerable().Any(row => row["id"].EqualString(location)) && !location.EqualString(string.Empty))
                        {
                            selectId &= false;
                            errLocation.Add(location);
                        }
                        else if (!location.EqualString(string.Empty))
                        {
                            trueLocation.Add(location);
                        }
                    }

                    if (!selectId)
                    {
                       e.Cancel = true;
                       MyUtility.Msg.WarningBox("Location : " + string.Join(",", errLocation.ToArray()) + "  Data not found !!", "Data not found");
                    }

                    trueLocation.Sort();
                    dr["ToLocation"] = string.Join(",", trueLocation.ToArray());

                    // 去除錯誤的Location將正確的Location填回
                    dr["selected"] = (!string.IsNullOrEmpty(dr["ToLocation"].ToString())) ? 1 : 0;

                    this.gridImport.RefreshEdit();
                }
            };
            #endregion Location 右鍵開窗
            #region stocktype validating
            DataGridViewGeneratorComboBoxColumnSettings stocktypeSet = new DataGridViewGeneratorComboBoxColumnSettings();

            stocktypeSet.CellValidating += (s, e) =>
            {
                if (this.EditMode && e.FormattedValue != null)
                {
                    DataRow drSelected = this.gridImport.GetDataRow(e.RowIndex);
                    if (e.FormattedValue.Equals(drSelected["stocktype"]))
                    {
                        return;
                    }

                    string getFtyInventorySql = $@"
select 
[Qty] = InQty - OutQty + AdjustQty ,
[fromlocation] = dbo.Getlocation(ukey),
[LastEditDate] = LastEditDate.val,
[FtyInventoryUkey] = FtyInventory.Ukey
from FtyInventory
outer apply(SELECT top 1 [val] = lt.EditDate
            FROM LocationTrans lt with (nolock)
            INNER JOIN LocationTrans_detail ltd with (nolock) ON lt.ID = ltd.ID
            WHERE lt.Status='Confirmed' AND ltd.FtyInventoryUkey = FtyInventory.Ukey 
            order by lt.EditDate desc ) LastEditDate
where
Poid = '{drSelected["poid"]}' and 
Seq1 = '{drSelected["Seq1"]}' and 
seq2  = '{drSelected["seq2"]}' and 
Roll = '{drSelected["Roll"]}' and 
stocktype = '{e.FormattedValue}'
";
                    DataRow dr;
                    if (MyUtility.Check.Seek(getFtyInventorySql, out dr))
                    {
                        drSelected["qty"] = dr["Qty"];
                        drSelected["FromLocation"] = dr["fromlocation"];
                        drSelected["stocktype"] = e.FormattedValue;
                        drSelected["LastEditDate"] = dr["LastEditDate"];
                        drSelected["FtyInventoryUkey"] = dr["FtyInventoryUkey"];
                        drSelected["ToLocation"] = string.Empty;
                    }
                    else
                    {
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox($"<Stock Type> data not found");
                        return;
                    }
                }
            };

            #endregion

            Ict.Win.UI.DataGridViewComboBoxColumn cbb_stocktype;

            this.gridImport.IsEditingReadOnly = false; // 必設定, 否則CheckBox會顯示圖示
            this.gridImport.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridImport)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk) // 0
                .Text("exportID", header: "WK#", width: Widths.AnsiChars(13), iseditingreadonly: true) // 1
                .Text("poid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true) // 1
                .Text("seq", header: "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true) // 2
                .Text("Roll", header: "Roll#", width: Widths.AnsiChars(9), iseditingreadonly: true) // 3
                .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true) // 4
                .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true) // 5
                .Text("colorid", header: "Color", width: Widths.AnsiChars(5), iseditingreadonly: true) // 6
                .Numeric("qty", header: "Qty", width: Widths.AnsiChars(10), decimal_places: 2, integer_places: 10, iseditingreadonly: true) // 7
                .ComboBox("stocktype", header: "Stock" + Environment.NewLine + "Type", width: Widths.AnsiChars(8), iseditable: true, settings: stocktypeSet).Get(out cbb_stocktype) // 8
                .Text("FromLocation", header: "FromLocation", iseditingreadonly: true) // 9
                .Text("ToLocation", header: "ToLocation", settings: ts2, iseditingreadonly: false) // 10
                .DateTime("LastEditDate", header: "Last Edit Date", iseditingreadonly: true)
            ;

            DataTable stocktypeSrc;
            string stocktypeGetSql = "select ID = replace(ID,'''',''), Name = rtrim(Name) from DropDownList WITH (NOLOCK) where Type = 'Pms_StockType' order by Seq";
            DBProxy.Current.Select(null, stocktypeGetSql, out stocktypeSrc);
            cbb_stocktype.DataSource = stocktypeSrc;
            cbb_stocktype.ValueMember = "ID";
            cbb_stocktype.DisplayMember = "Name";

            this.gridImport.Columns["ToLocation"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridImport.Columns["stocktype"].DefaultCellStyle.BackColor = Color.Pink;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            this.listControlBindingSource1.EndEdit();
            this.gridImport.ValidateControl();
            DataTable dtGridBS1 = (DataTable)this.listControlBindingSource1.DataSource;
            if (MyUtility.Check.Empty(dtGridBS1) || dtGridBS1.Rows.Count == 0)
            {
                return;
            }

            // 若無勾選，表示Qty <=0 的資料都被隱藏了，在這邊過濾掉
            // Qty > 0 一定都會在
            bool check_BalanceQty = this.BalanceQty.Checked;

            DataRow[] dr2 = check_BalanceQty ? dtGridBS1.Select("Selected = 1 AND Qty>0") : dtGridBS1.Select("Selected = 1");

            if (dr2.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select rows first!", "Warnning");
                return;
            }

            foreach (DataRow tmp in dr2)
            {
                DataRow[] findrow = this.dt_detail.AsEnumerable().Where(row => row.RowState != DataRowState.Deleted && row["poid"].EqualString(tmp["poid"].ToString()) && row["seq1"].EqualString(tmp["seq1"].ToString())
                                                                          && row["seq2"].EqualString(tmp["seq2"].ToString()) && row["roll"].EqualString(tmp["roll"].ToString())
                                                                          && row["dyelot"].EqualString(tmp["dyelot"].ToString())
                                                                          && row["stocktype"].EqualString(tmp["stocktype"].ToString())).ToArray();

                // DataRow[] findrow = dt_detail.Select(string.Format("poid = '{0}' and seq1 = '{1}' and seq2 = '{2}' and roll ='{3}'and dyelot='{4}'"
                //    , tmp["poid"].ToString(), tmp["seq1"].ToString(), tmp["seq2"].ToString(), tmp["roll"].ToString(), tmp["dyelot"].ToString()));
                if (findrow.Length > 0)
                {
                    findrow[0]["qty"] = tmp["qty"];
                    findrow[0]["tolocation"] = tmp["tolocation"];
                    findrow[0]["fromlocation"] = tmp["fromlocation"];
                }
                else
                {
                    tmp["id"] = this.dr_master["id"];
                    tmp.AcceptChanges();
                    tmp.SetAdded();
                    this.dt_detail.ImportRow(tmp);
                }
            }

            this.Close();
        }

        private void TxtLocation2_MouseDown(object sender, MouseEventArgs e)
        {
            Win.Tools.SelectItem2 item = PublicPrg.Prgs.SelectLocation(string.Empty, string.Empty);
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            var select_result = item.GetSelecteds()
                .GroupBy(s => new { StockType = s["StockType"].ToString(), StockTypeCode = s["StockTypeCode"].ToString() })
                .Select(g => new { g.Key.StockType, g.Key.StockTypeCode, ToLocation = string.Join(",", g.Select(i => i["id"])) });

            if (select_result.Count() > 0)
            {
                this.selectedLocation.Clear();
                this.txtLocation2.Text = string.Empty;
            }

            foreach (var result_item in select_result)
            {
                this.selectedLocation.Add(result_item.StockTypeCode, result_item.ToLocation);
                this.txtLocation2.Text += $"({result_item.StockType}:{result_item.ToLocation})";
            }
        }

        // private void radioPanel1_ValueChanged(object sender, EventArgs e)
        // {
        //    Sci.Win.UI.RadioPanel rdoG = (Sci.Win.UI.RadioPanel)sender;
        //    switch (rdoG.Value)
        //    {
        //        case "1":
        //            txtSPNo.ReadOnly = false;
        //            txtSeq.txtSeq_ReadOnly(false);
        //            txtRef.ReadOnly = false;
        //            txtLocation.ReadOnly = false;
        //            txtDyelot.ReadOnly = false;
        //            txtTransactionID.ReadOnly = true;
        //            txtTransactionID.Text = "";
        //            break;
        //        case "2":
        //            txtSPNo.ReadOnly = true;
        //            txtSeq.txtSeq_ReadOnly(true);
        //            txtRef.ReadOnly = true;
        //            txtLocation.ReadOnly = true;
        //            txtDyelot.ReadOnly = true;
        //            txtSPNo.Text = "";
        //            txtSeq.seq1 = "";
        //            txtSeq.seq2 = "";
        //            txtRef.Text = "";
        //            txtLocation.Text = "";
        //            txtDyelot.Text = "";
        //            txtTransactionID.ReadOnly = false;
        //            break;
        //    }
        // }
        private void BtnUpdateAllLocation_Click(object sender, EventArgs e)
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
                if (this.selectedLocation.ContainsKey(item["stocktype"].ToString()))
                {
                    item["tolocation"] = this.selectedLocation[item["stocktype"].ToString()];
                }
            }
        }

        // protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        // {
        //    bool re = base.ProcessCmdKey(ref msg, keyData);
        //    if (radioTransactionID.Focused && radioTransactionID.Checked == true)
        //    {
        //        if (keyData == Keys.Tab || keyData == Keys.Enter)
        //        {
        //            txtTransactionID.Select();
        //            return true;
        //        }
        //    }

        // if (txtTransactionID.Focused)
        //    {
        //        if(keyData == Keys.Tab || keyData == Keys.Enter)
        //        {
        //            txtTransactionID.TabStop = false;
        //            btnQuery.Select();
        //            return true;
        //        }
        //    }
        //    return re;
        // }

        // 動態顯示列表資料
        private void BalanceQty_CheckedChanged(object sender, EventArgs e)
        {
            this.Grid_Filter();
        }

        private void Grid_Filter()
        {
            string filter = string.Empty;
            if (this.gridImport.RowCount > 0)
            {
                switch (this.BalanceQty.Checked)
                {
                    case true:
                        if (MyUtility.Check.Empty(this.gridImport))
                        {
                            break;
                        }

                        // 這裡過濾的欄位，必須是剛剛SQL查出來的欄位，不是WHERE裡面的條件
                        filter = $@"qty > 0";

                        ((DataTable)this.listControlBindingSource1.DataSource).DefaultView.RowFilter = filter;
                        break;

                    case false:
                        if (MyUtility.Check.Empty(this.gridImport))
                        {
                            break;
                        }

                        filter = string.Empty;
                        ((DataTable)this.listControlBindingSource1.DataSource).DefaultView.RowFilter = filter;
                        break;
                }
            }
        }
    }
}
