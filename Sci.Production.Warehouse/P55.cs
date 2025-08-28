using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.PublicPrg;
using Sci.Win;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P55 : Win.Tems.Input6
    {
        /// <inheritdoc/>
        public P55(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = string.Format($"MDivisionID = '{Env.User.Keyword}'");
            this.gridicon.Append.Enabled = false;
            this.gridicon.Append.Visible = false;
            this.gridicon.Insert.Enabled = false;
            this.gridicon.Insert.Visible = false;
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["MDivisionID"] = Env.User.Keyword;
            this.CurrentMaintain["FactoryID"] = Env.User.Factory;
            this.CurrentMaintain["Status"] = "New";
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            #region -- Location 右鍵開窗 --
            DataGridViewGeneratorTextColumnSettings ts = new DataGridViewGeneratorTextColumnSettings();
            ts.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    DataRow dr_nowgridrow = this.detailgrid.GetDataRow(e.RowIndex);
                    Win.Tools.SelectItem2 item = Prgs.SelectLocation(dr_nowgridrow["stocktype"].ToString(), dr_nowgridrow["location"].ToString());
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    this.detailgrid.GetDataRow(e.RowIndex)["location"] = item.GetSelectedString();
                    this.detailgrid.GetDataRow(e.RowIndex).EndEdit();
                }
            };

            ts.CellValidating += (s, e) =>
            {
                if (this.EditMode && e.FormattedValue != null)
                {
                    DataRow dr_nowgridrow = this.detailgrid.GetDataRow(e.RowIndex);

                    // 去除錯誤的Location將正確的Location填回
                    string newLocation = string.Empty;
                    DualResult result = P55_Import.CheckDetailStockTypeLocation(dr_nowgridrow["stocktype"].ToString(), e.FormattedValue.ToString(), out newLocation);
                    if (!result)
                    {
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox(result.Description);
                    }

                    this.CurrentDetailData["Location"] = newLocation;
                }
            };
            #endregion Location 右鍵開窗

            #region 欄位設定
            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("POID", header: "SP#", width: Widths.AnsiChars(15), iseditingreadonly: true) // 0
                .Text("Seq", header: "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true) // 1
                .Text("Roll", header: "Roll", width: Widths.AnsiChars(6), iseditingreadonly: true) // 2
                .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true) // 3
                .Text("StockTypeDisplay", header: "Stock Type", width: Widths.AnsiChars(4), iseditingreadonly: true)
                .Text("Refno", header: "Refno", width: Widths.AnsiChars(15), iseditingreadonly: true) // 4
                .Numeric("ReceivingQty", header: "Receiving Qty", iseditingreadonly: true, decimal_places: 2, integer_places: 10) // 5
                .Text("Location", header: "Location", iseditingreadonly: false, width: Widths.AnsiChars(20), settings: ts)　// 6
                .Numeric("Qty", header: "Transfer Out Qty", iseditingreadonly: true, decimal_places: 2, integer_places: 10) // 7
                .Text("StockUnit", header: "Stock Unit", iseditingreadonly: true) // 8
                .Text("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true) // 9
                .Text("Tone", header: "Tone/Grp", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Numeric("RecvKG", header: "Recv (Kg)", width: Widths.AnsiChars(5), decimal_places: 2, iseditingreadonly: true)
                .Text("StyleID", header: "Style", width: Widths.AnsiChars(8), iseditingreadonly: true)
                ;
            #endregion 欄位設定
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();
            this.DetailSelectCommand = $@"
            select 
            srd.POID
            ,[Seq] = Concat ( srd.Seq1, ' ', srd.Seq2 )
            ,srd.Seq1
            ,srd.Seq2
            ,srd.Roll
            ,srd.Dyelot
            ,psd.Refno
            ,[ReceivingQty] = isnull( rdQty.ActualQty,0) + isnull(tidQty.Qty,0)
            ,psd.StockUnit
            ,ttsdtQty.Qty
            ,ttsdtQty.RecvKG
            ,[Description] = tDescription.val
            ,[DESC] = IIF((srd.POID =   lag(srd.POID,1,'') over (order by srd.POid, srd.seq1, srd.seq2, srd.Roll, srd.Dyelot) 
		            AND(srd.seq1 = lag(srd.seq1,1,'')over (order by srd.poid, srd.seq1, srd.seq2, srd.Roll, srd.Dyelot))
		            AND(srd.seq2 = lag(srd.seq2,1,'')over (order by srd.poid, srd.seq1, srd.seq2, srd.Roll, srd.Dyelot))) 
		            ,'',dbo.getMtlDesc(srd.poid,srd.seq1,srd.seq2,2,0))
            ,srd.Ukey
            ,srd.ID
            ,srd.StockType
            ,StockTypeDisplay =
                 CASE srd.StockType
                     WHEN 'B' THEN 'Bulk'
                     WHEN 'I' THEN 'Inventory'
                 END
            ,fi.Tone
            ,[Total]=sum(ttsdtQty.Qty) OVER (PARTITION BY srd.POID ,srd.Seq1, srd.Seq2 )
            ,o.StyleID
            ,srd.Location
            from SubconReturn sr with(nolock)
            inner join SubconReturn_Detail srd with(nolock) on sr.id = srd.id
            left join PO_Supp_Detail psd with(nolock) on srd.POID =  psd.ID and srd.Seq1 = psd.SEQ1 and srd.Seq2 = psd.SEQ2
            INNER JOIN FtyInventory fi WITH (NOLOCK)
                ON fi.POID = srd.PoId
                    AND fi.Seq1 = srd.Seq1
                    AND fi.Seq2 = srd.Seq2
                    AND fi.Roll = srd.Roll
                    AND fi.Dyelot = srd.Dyelot
                    AND fi.StockType = srd.StockType
            INNER JOIN Orders o WITH (NOLOCK) ON o.ID = srd.POID
            outer apply
            (
	            select rd.ActualQty 
	            from Receiving_Detail rd with(nolock)
	            inner join Receiving r with(nolock) on rd.Id = r.id
	            where r.Type = 'A' and
		                srd.POID = rd.PoId and
		                srd.Seq1 = rd.Seq1 and 
		                srd.Seq2 = rd.Seq2 and 
		                srd.Roll = rd.Roll and 
		                srd.Dyelot = rd.Dyelot
            )rdQty
            outer apply
            (
	            select tid.Qty
	            from TransferIn_Detail tid with(nolock)
	            where  srd.POID = tid.PoId and 
		               srd.Seq1 = tid.Seq1 and 
		               srd.Seq2 = tid.Seq2 and
		               srd.Roll = tid.Roll and 
		               srd.Dyelot = tid.Dyelot
            )tidQty
            outer apply
            (
	            select val = dbo.getMtlDesc ( srd.POID, srd.Seq1, srd.Seq2, 2, 0 )
            )tDescription
            outer apply
            (
	            select 
	            td.Qty
                ,td.RecvKG
	            from TransferToSubcon_Detail td with(nolock)
	            where td.Ukey = srd.TransferToSubcon_DetailUkey and
		              td.POID = srd.POID and
		              td.Seq1 = srd.Seq1 and
		              td.Seq2 = srd.Seq2 and
		              td.Roll = srd.Roll and
		              td.Dyelot = srd.Dyelot
            )ttsdtQty

            where sr.ID = '{masterID}'
            order by srd.poid, srd.seq1, srd.seq2, srd.Roll, srd.Dyelot";
            return base.OnDetailSelectCommandPrepare(e);
        }

        private void TxtSubcon_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Win.UI.TextBox prodText = (Win.UI.TextBox)sender;
            Win.Tools.SelectItem item;
            string selectCommand = "select ID from ArtworkType where IsSubcon = 1";
            item = new Win.Tools.SelectItem(selectCommand, "20", prodText.Text);
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            if (!MyUtility.Check.Empty(prodText.Text))
            {
                if (MyUtility.Msg.QuestionBox("Sub con already changed, system will clean detail data, do you want to switch the sub con ?") == DialogResult.No)
                {
                    return;
                }

                ((DataTable)this.detailgridbs.DataSource).Select().ToList().ForEach(r => r.Delete()); // 清除表身
            }

            prodText.Text = item.GetSelectedString();
        }

        private void TxtSubcon_Validating(object sender, CancelEventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtSubcon.Text))
            {
                return;
            }

            string sqlcmd = $@"select ID from ArtworkType where IsSubcon = 1 and ID = '{this.txtSubcon.Text}'";
            if (MyUtility.Check.Seek(sqlcmd))
            {
                return;
            }

            MyUtility.Msg.WarningBox(string.Format("Cannot found sub con <{0}>", this.txtSubcon.Text));
            e.Cancel = true;
        }

        private void BtnFind_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.detailgridbs.DataSource))
            {
                return;
            }

            DataRow dt = ((DataTable)this.detailgridbs.DataSource).Select($"POID like '%{this.txtLocateForSP.Text.TrimEnd()}%'").FirstOrDefault();
            if (dt == null)
            {
                MyUtility.Msg.WarningBox("Data was not found!!");
                return;
            }

            int index = this.detailgridbs.Find("POID", dt["POID"].ToString());
            this.detailgridbs.Position = index;
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["Subcon"]))
            {
                MyUtility.Msg.WarningBox("Please choose sub con before import material.");
                return;
            }

            var win = new P55_Import(this.CurrentMaintain, (DataTable)this.detailgridbs.DataSource);
            win.ShowDialog(this);
            this.RenewData();
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            DualResult dualResult;
            DataTable dataTable = (DataTable)this.detailgridbs.DataSource;

            if (MyUtility.Check.Empty(this.CurrentMaintain["Subcon"]))
            {
                MyUtility.Msg.WarningBox("Sub con  can't be empty!", "Warning");
                this.txtSubcon.Focus();
                return false;
            }

            if (this.DetailDatas.Count == 0)
            {
                MyUtility.Msg.WarningBox("Detail can't be empty", "Warning");
                return false;
            }

            string sqlcmd = string.Empty;
            sqlcmd = $@"select 
                        [SP#] = td.poid
                        ,[Seq] = Concat ( td.Seq1, ' ', td.Seq2 )
                        ,td.Roll
                        ,td.Dyelot
                        , StockType =
                                    CASE td.StockType
                                        WHEN 'B' THEN 'Bulk'
                                        WHEN 'I' THEN 'Inventory'
                                    END
                        ,[Sub con status] = f.SubConStatus 
                        from FtyInventory f with(nolock)
                        inner join #tmp td with(nolock) on 
                        f.POID = td.POID 
                        and f.Seq1 = td.Seq1
                        and f.Seq2 = td.Seq2
                        and f.Roll = td.Roll
                        and f.Dyelot = td.Dyelot
                        and f.StockType =td.StockType
                        where f.SubConStatus <> '{this.CurrentMaintain["Subcon"]}'";
            dualResult = MyUtility.Tool.ProcessWithDatatable(dataTable, string.Empty, sqlcmd, out DataTable dt);

            if (!dualResult)
            {
                MyUtility.Msg.WarningBox(dualResult.ToString());
                return false;
            }

            if (dt.Rows.Count != 0)
            {
                string msg = $"({this.CurrentMaintain["Subcon"]}) Fabric not transfer to sub con or still transfer to other sub con not return yet.";
                MyUtility.Msg.ShowMsgGrid(dt, msg: msg, caption: "Warning");
                return false;
            }

            sqlcmd = string.Empty;
            sqlcmd = $@"select 
                        [SP#] = sd.poid
                        ,[seq] = Concat (sd.Seq1, ' ', sd.Seq2 )
                        ,sd.Roll
                        ,sd.Dyelot
                        , StockType =
                                    CASE sd.StockType
                                        WHEN 'B' THEN 'Bulk'
                                        WHEN 'I' THEN 'Inventory'
                                    END
                        ,[Sub con Return ID] = sd.ID
                        ,[Sub con Return Status] = s.Status
                        from SubconReturn s with(nolock)
                        left join SubconReturn_Detail sd with(nolock) on s.ID = sd.ID
                        where exists(
                        select 1 from #tmp ti
                        where 
                        sd.POID = ti.PoId
                        and sd.Seq1  = ti.Seq1 
                        and sd.Seq2  = ti.Seq2
                        and sd.Roll = ti.Roll 
                        and sd.Dyelot = ti.Dyelot
                        and sd.StockType = ti.StockType
                        ) 
                        and s.subcon = '{this.CurrentMaintain["Subcon"]}'
                        and s.id <> '{this.CurrentMaintain["ID"]}'";
            dualResult = MyUtility.Tool.ProcessWithDatatable(dataTable, string.Empty, sqlcmd, out DataTable dataTb);

            if (!dualResult)
            {
                MyUtility.Msg.WarningBox(dualResult.ToString());
                return false;
            }

            if (dataTb.Rows.Count != 0)
            {
                string msg = $"({this.CurrentMaintain["Subcon"]})Fabric already existed in other subcon return record.";
                MyUtility.Msg.ShowMsgGrid(dataTb, msg: msg, caption: "Warning");
                return false;
            }

            if (!MyUtility.Check.Seek($"Select 1 from SubconReturn where ID = '{this.CurrentMaintain["ID"]}'"))
            {
                this.CurrentMaintain["ID"] = MyUtility.GetValue.GetID(Env.User.Keyword + "BR", "SubconReturn", (DateTime)MyUtility.Convert.GetDate(this.CurrentMaintain["AddDate"]));
            }

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            DualResult dualResult;
            DataTable dataTable = (DataTable)this.detailgridbs.DataSource;
            string sqlcmd = string.Empty;

            this.RenewData(); // 先重載資料, 避免雙開程式狀況
            if (this.CurrentMaintain == null)
            {
                return;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["ReturnDate"]))
            {
                MyUtility.Msg.WarningBox("Return Date cannot be empty.");
                return;
            }

            sqlcmd = $@"select 
                        [SP#] = td.poid
                        ,[Seq] = Concat ( td.Seq1, ' ', td.Seq2 )
                        ,td.Roll
                        ,td.Dyelot
                        , StockType =
                                    CASE td.StockType
                                        WHEN 'B' THEN 'Bulk'
                                        WHEN 'I' THEN 'Inventory'
                                    END
                        ,[Sub con status] = f.SubConStatus 
                        from FtyInventory f with(nolock)
                        inner join #tmp td with(nolock) on 
                        f.POID = td.POID 
                        and f.Seq1 = td.Seq1
                        and f.Seq2 = td.Seq2
                        and f.Roll = td.Roll
                        and f.Dyelot = td.Dyelot
                        and f.StockType =td.StockType
                        where f.SubConStatus <> '{this.CurrentMaintain["SubCon"]}'";
            dualResult = MyUtility.Tool.ProcessWithDatatable(dataTable, string.Empty, sqlcmd, out DataTable dt);

            if (!dualResult)
            {
                MyUtility.Msg.WarningBox(dualResult.ToString());
                return;
            }

            if (dt.Rows.Count != 0)
            {
                string msg = $"({this.CurrentMaintain["SubCon"]}) Fabric not transfer to sub con or still transfer to other sub con not return yet";
                MyUtility.Msg.ShowMsgGrid(dt, msg: msg, caption: "Warning");
                return;
            }

            sqlcmd = string.Empty;
            sqlcmd = $@"select
                        [SP#] = sd.poid
                        ,[Seq] = Concat (sd.Seq1, ' ', sd.Seq2 )
                        ,sd.Roll
                        ,sd.Dyelot
                        , StockType =
                                    CASE sd.StockType
                                        WHEN 'B' THEN 'Bulk'
                                        WHEN 'I' THEN 'Inventory'
                                    END
                        ,[Sub con Return ID] = sd.ID
                        ,[Sub con Return Status] = s.Status
                        from SubconReturn s with(nolock)
                        left join SubconReturn_Detail sd with(nolock) on s.ID = sd.ID
                        where exists(
                        select 1 from #tmp ti
                        where 
                        sd.POID = ti.PoId
                        and sd.Seq1  = ti.Seq1 
                        and sd.Seq2  = ti.Seq2
                        and sd.Roll = ti.Roll 
                        and sd.Dyelot = ti.Dyelot
                        and sd.StockType = ti.StockType
                        ) 
                        and s.subcon = '{this.CurrentMaintain["Subcon"]}'
                        and s.id <> '{this.CurrentMaintain["ID"]}'";

            dualResult = MyUtility.Tool.ProcessWithDatatable(dataTable, null, sqlcmd, out dt);

            if (!dualResult)
            {
                MyUtility.Msg.WarningBox(dualResult.ToString());
                return;
            }

            if (dt.Rows.Count != 0)
            {
                string msg = $"({this.CurrentMaintain["Subcon"]}) Fabric already existed in other subcon return record.";
                MyUtility.Msg.ShowMsgGrid(dt, msg: msg, caption: "Warning");
                return;
            }

            #region 檢查物料Location 是否存在WMS
            if (!PublicPrg.Prgs.Chk_WMS_Location(this.CurrentMaintain["ID"].ToString(), this.Name))
            {
                return;
            }
            #endregion

            #region -- 更新庫存數量 MDivisionPoDetail --
            var data_MD_2T = (from b in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
                              group b by new
                              {
                                  poid = b.Field<string>("poid").Trim(),
                                  seq1 = b.Field<string>("seq1").Trim(),
                                  seq2 = b.Field<string>("seq2").Trim(),
                                  stocktype = b.Field<string>("stocktype").Trim(),
                              }
                                into m
                              select new Prgs_POSuppDetailData
                              {
                                  Poid = m.First().Field<string>("poid"),
                                  Seq1 = m.First().Field<string>("seq1"),
                                  Seq2 = m.First().Field<string>("seq2"),
                                  Stocktype = m.First().Field<string>("stocktype"),
                                  Qty = m.Sum(w => w.Field<decimal>("qty")),
                                  Location = string.Join(",", m.Select(r => r.Field<string>("location")).Distinct()),
                              }).ToList();
            #endregion
            #region -- 更新庫存數量  ftyinventory --

            int mtlAutoLock = MyUtility.Convert.GetBool(MyUtility.GetValue.Lookup("select MtlAutoLock from system")) ? 1 : 0;
            var data_Fty_2T = (from b in this.DetailDatas
                               select new
                               {
                                   poid = b.Field<string>("poid"),
                                   seq1 = b.Field<string>("seq1"),
                                   seq2 = b.Field<string>("seq2"),
                                   stocktype = b.Field<string>("stocktype"),
                                   qty = b.Field<decimal>("qty"),
                                   tolocation = b.Field<string>("location"),
                                   roll = b.Field<string>("roll"),
                                   dyelot = b.Field<string>("dyelot"),
                               }).ToList();

            string upd_Fty_2T = Prgs.UpdateFtyInventory_IO(27, null, true, mtlAutoLock);
            #endregion 更新庫存數量  ftyinventory

            Exception errMsg = null;
            using (TransactionScope transactionscope = new TransactionScope())
            {
                DBProxy.Current.OpenConnection(null, out SqlConnection sqlConn);
                using (sqlConn)
                {
                    try
                    {
                        string sql_tup = $@"update Subconreturn set status = 'Confirmed', EditDate = GetDate(), EditName = '{Env.User.UserID}' where id = '{this.CurrentMaintain["id"]}'";
                        if (!(dualResult = DBProxy.Current.Execute(null, sql_tup)))
                        {
                            throw dualResult.GetException();
                        }

                        string updateColumn = MyUtility.Convert.GetString(this.CurrentMaintain["Subcon"]).EqualString("GMT WASH") ? ",GMTWashStatus = 'Done'" : string.Empty;
                        string sql_up = $@"
UPDATE f
SET SubConStatus = ''
    {updateColumn}
FROM FtyInventory AS f WITH (NOLOCK)
INNER JOIN #tmp td WITH (NOLOCK)
    ON f.POID = td.POID
    AND f.Seq1 = td.Seq1
    AND f.Seq2 = td.Seq2
    AND f.Roll = td.Roll
    AND f.Dyelot = td.Dyelot
    AND f.StockType = td.StockType
";
                        if (!(dualResult = MyUtility.Tool.ProcessWithDatatable(dataTable, string.Empty, sql_up, out DataTable dtt)))
                        {
                            throw dualResult.GetException();
                        }

                        /*
                            * 先更新 FtyInventory 後更新 MDivisionPoDetail
                            * 所有 MDivisionPoDetail 資料都在 Transaction 中更新，
                            * 因為要在同一 SqlConnection 之下執行
                            */
                        // FtyInventory 庫存
                        DataTable resulttb;
                        if (!(dualResult = MyUtility.Tool.ProcessWithObject(data_Fty_2T, string.Empty, upd_Fty_2T, out resulttb, "#TmpSource", conn: sqlConn)))
                        {
                            throw dualResult.GetException();
                        }

                        if (data_MD_2T.Count > 0)
                        {
                            string upd_MD_2T = Prgs.UpdateMPoDetail(0, data_MD_2T, true, sqlConn: sqlConn);
                            if (!(dualResult = MyUtility.Tool.ProcessWithObject(data_MD_2T, string.Empty, upd_MD_2T, out resulttb, "#TmpSource", conn: sqlConn)))
                            {
                                throw dualResult.GetException();
                            }
                        }

                        transactionscope.Complete();
                    }
                    catch (Exception ex)
                    {
                        errMsg = ex;
                    }
                }
            }

            if (!MyUtility.Check.Empty(errMsg))
            {
                this.ShowErr(errMsg);
                return;
            }

            MyUtility.Msg.InfoBox("Confirmed successful");
        }

        /// <inheritdoc/>
        protected override void ClickUnconfirm()
        {
            this.RenewData(); // 先重載資料, 避免雙開程式狀況
            base.ClickUnconfirm();
            DualResult dualResult;
            DataTable dataTable = (DataTable)this.detailgridbs.DataSource;
            if (this.CurrentMaintain == null ||
                MyUtility.Msg.QuestionBox("Do you want to unconfirme it?") == DialogResult.No)
            {
                return;
            }

            string sqlcmd = $@"select 
                               [SP#] = td.poid
                               ,[Seq] = Concat ( td.Seq1, ' ', td.Seq2 )
                               ,td.Roll
                               ,td.Dyelot
                                , StockType =
                                         CASE td.StockType
                                             WHEN 'B' THEN 'Bulk'
                                             WHEN 'I' THEN 'Inventory'
                                         END
                               ,[Sub con status] = f.SubConStatus 
                               from FtyInventory f with(nolock)
                               inner join #tmp td with(nolock) on 
                               f.POID = td.POID 
                               and f.Seq1 = td.Seq1
                               and f.Seq2 = td.Seq2
                               and f.Roll = td.Roll
                               and f.Dyelot = td.Dyelot
                               and f.StockType =td.StockType
                               where f.SubConStatus <> ''";

            dualResult = MyUtility.Tool.ProcessWithDatatable(dataTable, null, sqlcmd, out DataTable dt);

            if (!dualResult)
            {
                MyUtility.Msg.WarningBox(dualResult.ToString());
                return;
            }

            if (dt.Rows.Count != 0)
            {
                string msg = $"({this.CurrentMaintain["Subcon"]})Fabric still transfer to other sub con not return yet.";
                MyUtility.Msg.ShowMsgGrid(dt, msg: msg, caption: "Warning");
                return;
            }

            Exception errMsg = null;
            using (TransactionScope transactionscope = new TransactionScope())
            {
                try
                {
                    string sql_tup = $@"update SubconReturn set status = 'New', EditDate = GetDate(), EditName = '{Env.User.UserID}' where id = '{this.CurrentMaintain["id"]}'";
                    if (!(dualResult = DBProxy.Current.Execute(null, sql_tup)))
                    {
                        throw dualResult.GetException();
                    }

                    string updateColumn = MyUtility.Convert.GetString(this.CurrentMaintain["Subcon"]).EqualString("GMT WASH") ? ",GMTWashStatus = 'Ongoing'" : string.Empty;
                    string sql_up = $@"
UPDATE f
SET SubConStatus = '{this.CurrentMaintain["SubCon"]}'
    {updateColumn}
FROM FtyInventory AS f WITH (NOLOCK)
INNER JOIN #tmp td WITH (NOLOCK)
    ON f.POID = td.POID
    AND f.Seq1 = td.Seq1
    AND f.Seq2 = td.Seq2
    AND f.Roll = td.Roll
    AND f.Dyelot = td.Dyelot
    AND f.StockType = td.StockType";
                    if (!(dualResult = MyUtility.Tool.ProcessWithDatatable(dataTable, string.Empty, sql_up, out DataTable dtt)))
                    {
                        throw dualResult.GetException();
                    }

                    transactionscope.Complete();
                }
                catch (Exception ex)
                {
                    errMsg = ex;
                }
            }

            MyUtility.Msg.InfoBox("UnConfirmed successful");
        }

        /// <inheritdoc/>
        protected override bool ClickEditBefore()
        {
            if (this.CurrentMaintain["Status"].ToString() == "Confirmed")
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't modify.", "Warning");
                return false;
            }

            return base.ClickEditBefore();
        }

        /// <inheritdoc/>
        protected override bool ClickDeleteBefore()
        {
            // 從DB取得最新Status, 避免多工時, 畫面上資料不是最新的狀況
            this.RenewData();
            if (this.CurrentMaintain["Status"].EqualString("CONFIRMED"))
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't delete.", "Warning");

                // 重新整理畫面
                this.OnRefreshClick();
                return false;
            }

            // 重新整理畫面
            this.OnRefreshClick();
            return base.ClickDeleteBefore();
        }

        /// <inheritdoc/>
        protected override bool ClickPrint()
        {
            if (!MyUtility.Convert.GetString(this.CurrentMaintain["Status"]).EqualString("Confirmed"))
            {
                MyUtility.Msg.WarningBox("Data is not confirmed, can't print.");
                return false;
            }

            // 抓M的EN NAME
            DualResult result = DBProxy.Current.Select(string.Empty, $@"select NameEN from MDivision where ID='{Env.User.Keyword}'", out DataTable dtNAME);
            if (!result)
            {
                this.ShowErr(result);
                return false;
            }

            string rptTitle = dtNAME.Rows[0]["NameEN"].ToString();
            string id = this.CurrentMaintain["ID"].ToString();
            string subcon = this.CurrentMaintain["subcon"].ToString();
            string remark = this.CurrentMaintain["Remark"].ToString().Trim().Replace("\r", " ").Replace("\n", " ");
            string date = MyUtility.Convert.GetDate(this.CurrentMaintain["ReturnDate"]).Value.ToString("yyyy/MM/dd");

            ReportDefinition report = new ReportDefinition();
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("RptTitle", rptTitle));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("ID", id));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Subcon", subcon));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Remark", remark));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("date", date));

            // 傳 list 資料
            List<P55_PrintData> data = this.DetailDatas.AsEnumerable()
                .Select(row1 => new P55_PrintData()
                {
                    POID = row1["POID"].ToString().Trim(),
                    SEQ = row1["SEQ"].ToString().Trim(),
                    Roll = row1["Roll"].ToString().Trim(),
                    Dyelot = row1["Dyelot"].ToString().Trim(),
                    DESC = (MyUtility.Check.Empty(row1["DESC"]) == false) ? row1["DESC"].ToString().Trim() + Environment.NewLine + row1["Poid"].ToString().Trim() + Environment.NewLine + "Recv(Kg) : " + row1["RecvKG"].ToString().Trim() : "Recv(Kg) :" + row1["RecvKG"].ToString().Trim(),
                    Tone = row1["Tone"].ToString().Trim(),
                    Stocktype = row1["StockTypeDisplay"].ToString().Trim(),
                    Unit = row1["StockUnit"].ToString().Trim(),
                    QTY = row1["QTY"].ToString().Trim(),
                    Total = row1["Total"].ToString().Trim(),
                    Location = row1["Location"].ToString().Trim(),
                }).ToList();

            report.ReportDataSource = data;

            Type reportResourceNamespace = typeof(P55_PrintData);
            Assembly reportResourceAssembly = reportResourceNamespace.Assembly;
            string reportResourceName = "P55_Print.rdlc";

            if (!(result = ReportResources.ByEmbeddedResource(reportResourceAssembly, reportResourceNamespace, reportResourceName, out IReportResource reportresource)))
            {
                this.ShowErr(result);
                return false;
            }

            report.ReportResource = reportresource;

            // 開啟 report view
            var frm = new Win.Subs.ReportView(report);
            frm.MdiParent = this.MdiParent;
            frm.Show();

            return base.ClickPrint();
        }
    }
}
