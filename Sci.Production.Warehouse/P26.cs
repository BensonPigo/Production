using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.PublicPrg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    public partial class P26 : Sci.Win.Tems.Input6
    {
        private Dictionary<string, string> di_fabrictype = new Dictionary<string, string>();
        private Dictionary<string, string> di_stocktype = new Dictionary<string, string>();

        public P26(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.DefaultFilter = string.Format("MDivisionID = '{0}'", Sci.Env.User.Keyword);
            Dictionary<string, string> di_Stocktype = new Dictionary<string, string>();
            di_Stocktype.Add("B", "Bulk");
            di_Stocktype.Add("I", "Inventory");

            comboBox1.DataSource = new BindingSource(di_Stocktype, null);
            comboBox1.ValueMember = "Key";
            comboBox1.DisplayMember = "Value";

            gridicon.Append.Enabled = false;
            gridicon.Append.Visible = false;
            gridicon.Insert.Enabled = false;
            gridicon.Insert.Visible = false;
            
        }

        public P26(ToolStripMenuItem menuitem, string transID)
            : base(menuitem)
        {
            InitializeComponent();
            this.DefaultFilter = string.Format("MDivisionID = '{0}'", Sci.Env.User.Keyword);
            this.IsSupportNew = false;
            this.IsSupportEdit = false;
            this.IsSupportDelete = false;
            this.IsSupportConfirm = false;
            this.IsSupportUnconfirm = false;

            gridicon.Append.Enabled = false;
            gridicon.Append.Visible = false;
            gridicon.Insert.Enabled = false;
            gridicon.Insert.Visible = false;

        }

        // 新增時預設資料
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["MDivisionID"] = Sci.Env.User.Keyword;
            CurrentMaintain["IssueDate"] = DateTime.Now;
            CurrentMaintain["Status"] = "New";
            comboBox1.SelectedIndex = 0;
        }

        private void ChangeDetailColor()
        {
            detailgrid.RowPostPaint += (s, e) =>
            {
                if (!this.EditMode)
                {
                    DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                    if (detailgrid.Rows.Count <= e.RowIndex || e.RowIndex < 0) return;

                    int i = e.RowIndex;
                    if (MyUtility.Check.Empty(dr["stocktype"]) || MyUtility.Check.Empty(dr["stockunit"]))
                    {
                        detailgrid.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(255, 192, 203);
                    }
                }
            };
        }

        // delete前檢查
        protected override bool ClickDeleteBefore()
        {
            DataRow dr = grid.GetDataRow<DataRow>(grid.GetSelectedRowIndex());
            if (dr["status"].ToString().ToUpper() == "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't delete.", "Warning");
                return false;
            }
            return base.ClickDeleteBefore();
        }

        // edit前檢查
        protected override bool ClickEditBefore()
        {
            //!EMPTY(APVName) OR !EMPTY(Closed)，只能編輯remark欄。
            DataRow dr = grid.GetDataRow<DataRow>(grid.GetSelectedRowIndex());
            if (dr["status"].ToString().ToUpper() == "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't modify.", "Warning");
                return false;
            }
            return base.ClickEditBefore();
        }

        // save前檢查 & 取id
        protected override bool ClickSaveBefore()
        {
            DataTable result = null;
            StringBuilder warningmsg = new StringBuilder();

            if (DetailDatas.Count == 0)
            {
                MyUtility.Msg.WarningBox("Detail can't be empty", "Warning");
                return false;
            }

            //取單號
            if (this.IsDetailInserting)
            {
                string tmpId = Sci.MyUtility.GetValue.GetID(Sci.Env.User.Keyword + "LH", "LocationTrans", (DateTime)CurrentMaintain["Issuedate"]);
                if (MyUtility.Check.Empty(tmpId))
                {
                    MyUtility.Msg.WarningBox("Get document ID fail!!");
                    return false;
                }
                CurrentMaintain["id"] = tmpId;
            }

            return base.ClickSaveBefore();
        }

        // grid 加工填值
        protected override DualResult OnRenewDataDetailPost(RenewDataPostEventArgs e)
        {
            return base.OnRenewDataDetailPost(e);
        }

        //refresh
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            #region Status Label

            label25.Text = CurrentMaintain["status"].ToString();

            #endregion Status Label
        }

        // detail 新增時設定預設值
        protected override void OnDetailGridInsert(int index = -1)
        {
            base.OnDetailGridInsert(index);
        }

        // Detail Grid 設定
        protected override void OnDetailGridSetup()
        {
            #region Location 右鍵開窗

            Ict.Win.DataGridViewGeneratorTextColumnSettings ts2 = new DataGridViewGeneratorTextColumnSettings();
            ts2.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    Sci.Win.Tools.SelectItem2 item = Prgs.SelectLocation(CurrentMaintain["stocktype"].ToString(), CurrentDetailData["tolocation"].ToString());
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    CurrentDetailData["tolocation"] = item.GetSelectedString();
                }
            };

            #endregion Location 右鍵開窗

            #region 欄位設定

            Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("poid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)  //0
            .Text("seq", header: "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true)  //1
            .Text("Roll", header: "Roll#", width: Widths.AnsiChars(9), iseditingreadonly: true)    //2
            .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(5), iseditingreadonly: true)    //3
            .EditText("Description", header: "Description", width: Widths.AnsiChars(15), iseditingreadonly: true)    //4
            .Text("colorid", header: "Color", width: Widths.AnsiChars(5), iseditingreadonly: true)    //5
            .Text("SizeSpec", header: "SizeSpec", width: Widths.AnsiChars(5), iseditingreadonly: true)    //6
            .Numeric("qty", header: "Qty", width: Widths.AnsiChars(10), decimal_places: 2, integer_places: 10, iseditingreadonly: true)    //7
            .Text("FromLocation", header: "FromLocation", iseditingreadonly: true)    //8
            .Text("ToLocation", header: "ToLocation", settings: ts2, iseditingreadonly: true, width: Widths.AnsiChars(14))    //9
            ;     //

            #endregion 欄位設定
            this.detailgrid.Columns[9].DefaultCellStyle.BackColor = Color.Pink;

        }

        //Confirm
        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            var dr = this.CurrentMaintain;
            if (null == dr) return;

            StringBuilder sqlupd2 = new StringBuilder();
            String sqlupd3 = "";
            DualResult result, result2;
            string sqlupd2_A = "";
            string sqlupd2_FIO = "";

            #region 更新表頭狀態資料

            sqlupd3 = string.Format(@"update dbo.LocationTrans set status='Confirmed', editname = '{0}' , editdate = GETDATE()
                                where id = '{1}'", Env.User.UserID, CurrentMaintain["id"]);

            #endregion 更新表頭狀態資料

            #region 更新庫存數量 mdivisionPoDetail
            var bs1 = (from b in ((DataTable)detailgridbs.DataSource).AsEnumerable()
                       group b by new
                       {
                           mdivisionid = b.Field<string>("mdivisionid"),
                           poid = b.Field<string>("poid"),
                           seq1 = b.Field<string>("seq1"),
                           seq2 = b.Field<string>("seq2")
                       } into m
                       select new Prgs_POSuppDetailData_A
                       {
                           mdivisionid = m.First().Field<string>("mdivisionid"),
                           poid = m.First().Field<string>("poid"),
                           seq1 = m.First().Field<string>("seq1"),
                           seq2 = m.First().Field<string>("seq2"),
                           location = string.Join(",", m.Select(r => r.Field<string>("ToLocation")).Distinct()),
                           qty = 0,
                           stocktype = CurrentMaintain["stocktype"].ToString()
                       }).ToList();

            sqlupd2_A = Prgs.UpdateMPoDetail_A(2, bs1, true);

            #endregion
            #region 更新庫存數量 po_supp_detail & ftyinventory
            sqlupd2_FIO = @"
alter table #TmpSource alter column mdivisionid varchar(10)
alter table #TmpSource alter column poid varchar(20)
alter table #TmpSource alter column seq1 varchar(3)
alter table #TmpSource alter column seq2 varchar(3)
alter table #TmpSource alter column roll varchar(15)

merge dbo.FtyInventory as target
using #TmpSource as s
    on target.mdivisionid = s.mdivisionid and target.poid = s.poid and target.seq1 = s.seq1 
	and target.seq2 = s.seq2 and target.roll = s.roll
when matched then
    update
    set inqty = isnull(inqty,0.00) + 0
when not matched then
    insert ( [MDivisionPoDetailUkey],[mdivisionid],[Poid],[Seq1],[Seq2],[Roll],[Dyelot],[InQty])
    values ((select ukey from dbo.MDivisionPoDetail 
			 where mdivisionid = s.mdivisionid and poid = s.poid and seq1 = s.seq1 and seq2 = s.seq2)
			 ,s.mdivisionid,s.poid,s.seq1,s.seq2,s.roll,s.dyelot,0);

select tolocation,[ukey] = f.ukey
into #tmp_L_K 
from #TmpSource s
left join ftyinventory f on f.mdivisionid = s.mdivisionid and f.poid = s.poid 
						 and f.seq1 = s.seq1 and f.seq2 = s.seq2 and f.roll = s.roll
merge dbo.ftyinventory_detail as t
using #tmp_L_K as s on t.ukey = s.ukey and isnull(t.mtllocationid,'') = isnull(s.tolocation,'')
when not matched then
    insert ([ukey],[mtllocationid]) 
	values (s.ukey,isnull(s.tolocation,''));

delete t from FtyInventory_Detail t
where  exists(select 1 from #tmp_L_K x where x.ukey=t.Ukey and x.tolocation != t.MtlLocationID)
";
            #endregion 更新庫存數量 po_supp_detail & ftyinventory

            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    DataTable resulttb;
                    if (!(result = MyUtility.Tool.ProcessWithObject(bs1, "", sqlupd2_A, out resulttb, "#TmpSource")))
                    {
                        _transactionscope.Dispose();
                        ShowErr(result);
                        return;
                    }
                    if (!(result = MyUtility.Tool.ProcessWithDatatable
                        ((DataTable)detailgridbs.DataSource, "", sqlupd2_FIO, out resulttb, "#TmpSource")))
                    {
                        _transactionscope.Dispose();
                        ShowErr(result);
                        return;
                    }
                    if (!(result = DBProxy.Current.Execute(null, sqlupd3)))
                    {
                        _transactionscope.Dispose();
                        ShowErr(sqlupd3, result);
                        return;
                    }

                    _transactionscope.Complete();
                    MyUtility.Msg.InfoBox("Confirmed successful");
                }
                catch (Exception ex)
                {
                    _transactionscope.Dispose();
                    ShowErr("Commit transaction error.", ex);
                    return;
                }
            }
            _transactionscope.Dispose();
            _transactionscope = null;
            this.RenewData();
            this.OnDetailEntered();
            this.EnsureToolbarExt();
        }

        //寫明細撈出的sql command
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["ID"].ToString();

            this.DetailSelectCommand = string.Format(@"select a.id,a.mdivisionid,a.PoId,a.Seq1,a.Seq2,left(a.seq1+' ',3)+a.Seq2 as seq
,(select p1.colorid from PO_Supp_Detail p1 where p1.ID = a.PoId and p1.seq1 = a.SEQ1 and p1.SEQ2 = a.seq2) as colorid
,(select p1.sizespec from PO_Supp_Detail p1 where p1.ID = a.PoId and p1.seq1 = a.SEQ1 and p1.SEQ2 = a.seq2) as sizespec
,a.Roll
,a.Dyelot
,a.Qty
,a.FromLocation
,a.ToLocation
,a.ftyinventoryukey
,ukey
,dbo.getmtldesc(a.poid,a.seq1,a.seq2,2,0) as [description]
from dbo.LocationTrans_detail a
Where a.id = '{0}' ", masterID);

            return base.OnDetailSelectCommandPrepare(e);

        }

        private void button6_Click(object sender, EventArgs e)
        {
            var frm = new Sci.Production.Warehouse.P26_Import(CurrentMaintain, (DataTable)detailgridbs.DataSource);
            frm.ShowDialog(this);
            this.RenewData();
        }

        //217: WAREHOUSE_P26_Mtl Location update，2.當表身已經有值時，編輯時若換了stock type則表身要一併清空。
        private void comboBox1_Validating(object sender, CancelEventArgs e)
        {
            if (this.EditMode && !MyUtility.Check.Empty(comboBox1.SelectedValue) && comboBox1.SelectedValue != comboBox1.OldValue)
            {
                if (detailgridbs.DataSource != null && ((DataTable)detailgridbs.DataSource).Rows.Count > 0)
                {
                    ((DataTable)detailgridbs.DataSource).Rows.Clear();
                }
            }
        }


    }
}