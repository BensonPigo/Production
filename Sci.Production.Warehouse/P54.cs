using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Win;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P54 : Win.Tems.Input6
    {
        /// <inheritdoc/>
        public P54(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.gridicon.Append.Enabled = false;
            this.gridicon.Append.Visible = false;
            this.gridicon.Insert.Enabled = false;
            this.gridicon.Insert.Visible = false;
            this.DefaultFilter = string.Format("MDivisionID = '{0}'", Env.User.Keyword);
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
        protected override bool ClickSaveBefore()
        {
            DataTable dt;
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

            string sqlcmd = $@"select 
                            [SP#] = td.poid
                            ,[seq] = Concat (td.Seq1, ' ', td.Seq2 )
                            ,td.Roll
                            ,td.Dyelot
                            , StockType =
                                        CASE td.StockType
                                            WHEN 'B' THEN 'Bulk'
                                            WHEN 'I' THEN 'Inventory'
                                        END
                            ,[Transfer to Sub con ID] = td.ID
                            ,[Transfer to Sub con Status] = t.Status
                            from TransferToSubcon t with(nolock)
                            left join TransferToSubcon_Detail td with(nolock) on t.ID = td.ID
                            where exists(
                            select 1 from #detailTmp ti
                            where 
                            td.POID = ti.PoId
                            and td.Seq1  = ti.Seq1 
                            and td.Seq2  = ti.Seq2
                            and td.Roll = ti.Roll 
                            and td.Dyelot = ti.Dyelot
                            and td.StockType = ti.StockType
                            ) 
                            and t.subcon = '{this.CurrentMaintain["Subcon"]}'
                            and t.id <> '{this.CurrentMaintain["ID"]}'";

            DualResult dualResult = MyUtility.Tool.ProcessWithDatatable(dataTable, null, sqlcmd, out dt, "#detailTmp");

            if (!dualResult)
            {
                MyUtility.Msg.WarningBox(dualResult.ToString());
                return false;
            }

            if (dt.Rows.Count != 0)
            {
                string msg = $"({this.CurrentMaintain["Subcon"]})Fabric already existed in other transfer to subcon record.";
                MyUtility.Msg.ShowMsgGrid(dt, msg: msg, caption: "Warning");
                return false;
            }

            if (!MyUtility.Check.Seek($"Select 1 from TransferToSubcon where ID = '{this.CurrentMaintain["ID"]}'"))
            {
                this.CurrentMaintain["ID"] = MyUtility.GetValue.GetID(Env.User.Keyword + "TB", "TransferToSubcon", (DateTime)MyUtility.Convert.GetDate(this.CurrentMaintain["AddDate"]));
            }

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.labelNotApprove.Text = this.CurrentMaintain["Status"].ToString();
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            #region 欄位設定
            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("POID", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true) // 0
                .Text("Seq", header: "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true) // 1
                .Text("Roll", header: "Roll", width: Widths.AnsiChars(6), iseditingreadonly: true) // 2
                .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(10), iseditingreadonly: true) // 3
                .Text("StockTypeDisplay", header: "Stock Type", width: Widths.AnsiChars(4), iseditingreadonly: true)
                .Text("Refno", header: "Refno", width: Widths.AnsiChars(10), iseditingreadonly: true) // 4
                .Numeric("ReceivingQty", header: "Receiving Qty", decimal_places: 2, width: Widths.AnsiChars(15), iseditingreadonly: true) // 5
                .Numeric("Qty", header: "Transfer Out Qty", decimal_places: 2, width: Widths.AnsiChars(15), iseditingreadonly: true) // 6
                .Text("StockUnit", header: "Stock Unit", width: Widths.AnsiChars(13), iseditingreadonly: true) // 7
                .Text("Description", header: "Description", width: Widths.AnsiChars(50), iseditingreadonly: true) // 8
                .Text("Tone", header: "Tone/Grp", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Numeric("RecvKG", header: "Recv (Kg)", width: Widths.AnsiChars(5), decimal_places: 2, iseditingreadonly: true)
                .Text("StyleID", header: "Style", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("Location", header: "Location", iseditingreadonly: true, width: Widths.AnsiChars(25))
                .Text("WK#", header: "WK#", iseditingreadonly: true, width: Widths.AnsiChars(25))
                ;
            #endregion 欄位設定
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();
            this.DetailSelectCommand = $@"
            select 
            td.POID
            ,[Seq] =Concat ( td. Seq1, ' ', td.Seq2 )
            ,td.Roll
            ,td.Dyelot
            ,psd.Refno
            ,[ReceivingQty] = isnull( rdQty.ActualQty,0) + isnull(tidQty.Qty,0)
            ,td.Qty
            ,td.RecvKG
            ,[StockUnit] = psd.StockUnit
            ,[Description] = tDescription.val            
            ,[DESC] = IIF((td.POID =   lag(td.POID,1,'') over (order by td.POid, td.seq1, td.seq2, td.Roll, td.Dyelot) 
		            AND(td.seq1 = lag(td.seq1,1,'')over (order by td.poid, td.seq1, td.seq2, td.Roll, td.Dyelot))
		            AND(td.seq2 = lag(td.seq2,1,'')over (order by td.poid, td.seq1, td.seq2, td.Roll, td.Dyelot))) 
		            ,'',dbo.getMtlDesc(td.poid,td.seq1,td.seq2,2,0))
            ,td.ukey
            ,td.seq1
            ,td.seq2
            ,td.ID
            ,td.stocktype
            ,StockTypeDisplay =
                 CASE td.StockType
                     WHEN 'B' THEN 'Bulk'
                     WHEN 'I' THEN 'Inventory'
                 END
            ,fi.Tone
            ,[Total]=sum(td.Qty) OVER (PARTITION BY td.POID ,td.Seq1, td.Seq2 )
            ,o.StyleID
            ,[Location] = Dbo.GetLocation(fi.Ukey)
            ,[WK#] = WK.ExportId
            from TransferToSubcon tt with(nolock)
            inner join TransferToSubcon_Detail td with(nolock) on tt.ID = td.ID
            left join PO_Supp_Detail psd with(nolock) on td.POID =  psd.ID and td.Seq1 = psd.SEQ1 and td.Seq2 = psd.SEQ2
            INNER JOIN FtyInventory fi WITH (NOLOCK)
                ON fi.POID = td.PoId
                    AND fi.Seq1 = td.Seq1
                    AND fi.Seq2 = td.Seq2
                    AND fi.Roll = td.Roll
                    AND fi.Dyelot = td.Dyelot
                    AND fi.StockType = td.StockType
            INNER JOIN Orders o WITH (NOLOCK) ON o.ID = td.POID
            outer apply
            (
	            select rd.ActualQty 
	            from Receiving_Detail rd with(nolock)
	            inner join Receiving r with(nolock) on rd.Id = r.id
	            where r.Type = 'A' and
		                td.POID = rd.PoId and
		                td.Seq1 = rd.Seq1 and 
		                td.Seq2 = rd.Seq2 and 
		                td.Roll = rd.Roll and 
		                td.Dyelot = rd.Dyelot
            )rdQty
            outer apply
            (
	            select tid.Qty
	            from TransferIn_Detail tid with(nolock)
	            where  td.POID = tid.PoId and 
		                td.Seq1 = tid.Seq1 and 
		                td.Seq2 = tid.Seq2 and
		                td.Roll = tid.Roll and 
		                td.Dyelot = tid.Dyelot
            )tidQty
            outer apply
            (
	            select val = dbo.getMtlDesc ( td.POID, td.Seq1, td.Seq2, 2, 0 )
            )tDescription
            OUTER APPLY (
                select ExportId = Stuff((
                    select concat(',',ExportId)
                    from (
                            select distinct r.ExportId
                            from Receiving_Detail rd WITH (NOLOCK)
                            inner join Receiving r WITH (NOLOCK) on rd.Id = r.Id
                            where td.POID = rd.PoId and td.Seq1 = rd.Seq1
		                    and td.Seq2 = rd.Seq2 and td.Roll = rd.Roll
		                    and td.Dyelot = rd.Dyelot and r.ExportId <> ''
                        )s
                    for xml path ('')
                ) , 1, 1, '')
            )WK
            where tt.id = '{masterID}'
            order by td.Poid, td.seq1, td.seq2, td.Roll, td.Dyelot";
            return base.OnDetailSelectCommandPrepare(e);
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
            if (this.CurrentMaintain["Status"].ToString() == "Confirmed")
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't delete.", "Warning");
                return false;
            }

            return base.ClickDeleteBefore();
        }

        /// <inheritdoc/>
        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            DualResult dualResult;
            DataTable dataTable = (DataTable)this.detailgridbs.DataSource;

            this.RenewData(); // 先重載資料, 避免雙開程式狀況
            if (this.CurrentMaintain == null)
            {
                return;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["TransferOutDate"]))
            {
                MyUtility.Msg.WarningBox("Transfer out Date cannot be empty.");
                return;
            }

            string sqlcmd_f = $@"select 
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
            dualResult = MyUtility.Tool.ProcessWithDatatable(dataTable, string.Empty, sqlcmd_f, out DataTable dt);

            if (!dualResult)
            {
                MyUtility.Msg.WarningBox(dualResult.ToString());
                return;
            }

            if (dt.Rows.Count != 0)
            {
                string msg = "Fabric transfer to sub con not return yet.";
                MyUtility.Msg.ShowMsgGrid(dt, msg: msg, caption: "Warning");
                return;
            }

            string sqlcmd = $@"select
                            [SP#] = td.poid
                            ,[Seq] = Concat (td.Seq1, ' ', td.Seq2 )
                            ,td.Roll
                            ,td.Dyelot
                            , StockType =
                                        CASE td.StockType
                                            WHEN 'B' THEN 'Bulk'
                                            WHEN 'I' THEN 'Inventory'
                                        END
                            ,[Transfer to Sub con ID] = td.ID
                            ,[Transfer to Sub con Status] = t.Status
                            from TransferToSubcon t with(nolock)
                            left join TransferToSubcon_Detail td with(nolock) on t.ID = td.ID
                            where exists(
                            select 1 from #detailTmp ti
                            where 
                            td.POID = ti.PoId
                            and td.Seq1  = ti.Seq1 
                            and td.Seq2  = ti.Seq2
                            and td.Roll = ti.Roll 
                            and td.Dyelot = ti.Dyelot
                            and td.StockType = ti.StockType
                            ) 
                            and t.subcon = '{this.CurrentMaintain["Subcon"]}'
                            and t.id <> '{this.CurrentMaintain["ID"]}'";

            dualResult = MyUtility.Tool.ProcessWithDatatable(dataTable, null, sqlcmd, out dt, "#detailTmp");

            if (!dualResult)
            {
                MyUtility.Msg.WarningBox(dualResult.ToString());
                return;
            }

            if (dt.Rows.Count != 0)
            {
                string msg = $"({this.CurrentMaintain["Subcon"]})Fabric already existed in other transfer to subcon record.";
                MyUtility.Msg.ShowMsgGrid(dt, msg: msg, caption: "Warning");
                return;
            }

            Exception errMsg = null;
            using (TransactionScope transactionscope = new TransactionScope())
            {
                try
                {
                    string sql_tup = $@"update TransferToSubcon set status = 'Confirmed', EditDate = GetDate(), EditName = '{Env.User.UserID}' where id = '{this.CurrentMaintain["id"]}'";
                    if (!(dualResult = DBProxy.Current.Execute(null, sql_tup)))
                    {
                        throw dualResult.GetException();
                    }

                    string updateColumn = MyUtility.Convert.GetString(this.CurrentMaintain["Subcon"]).EqualString("GARMENT WASH SURCHAR") ? ",GMTWashStatus = 'Ongoing'" : string.Empty;
                    string sql_up = $@"
UPDATE f
SET SubConStatus = '{this.CurrentMaintain["Subcon"]}'
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

                    transactionscope.Complete();
                }
                catch (Exception ex)
                {
                    errMsg = ex;
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
                            [SP#] = td.POID
                            ,[Seq] = Concat ( td.Seq1, ' ', td.Seq2 )
                            ,td.Roll
                            ,td.Dyelot
                            , StockType =
                                        CASE td.StockType
                                            WHEN 'B' THEN 'Bulk'
                                            WHEN 'I' THEN 'Inventory'
                                        END
                            ,[Sub con return ID] = sd.id
                            ,[Sub con return status] = s.Status
                            from SubconReturn_Detail sd
                            inner join SubconReturn s on s.id = sd.id
                            inner join #tmp td on 
                            sd.POID = td.POID 
                            and sd.Seq1 = td.Seq1
                            and sd.Seq2 = td.Seq2
                            and sd.Roll = td.Roll
                            and sd.Dyelot = td.Dyelot
                            and sd.StockType =td.StockType
                            and s.Subcon = '{this.CurrentMaintain["Subcon"]}'";

            dualResult = MyUtility.Tool.ProcessWithDatatable(dataTable, null, sqlcmd, out DataTable dt);

            if (!dualResult)
            {
                MyUtility.Msg.WarningBox(dualResult.ToString());
                return;
            }

            if (dt.Rows.Count != 0)
            {
                string msg = $"({this.CurrentMaintain["Subcon"]})Fabric already existed in sub con return record.";
                MyUtility.Msg.ShowMsgGrid(dt, msg: msg, caption: "Warning");
                return;
            }

            Exception errMsg = null;
            using (TransactionScope transactionscope = new TransactionScope())
            {
                try
                {
                    string sql_tup = $@"update TransferToSubcon set status = 'New', EditDate = GetDate(), EditName = '{Env.User.UserID}' where id = '{this.CurrentMaintain["id"]}'";
                    if (!(dualResult = DBProxy.Current.Execute(null, sql_tup)))
                    {
                        throw dualResult.GetException();
                    }

                    string updateColumn = MyUtility.Convert.GetString(this.CurrentMaintain["Subcon"]).EqualString("GMT WASH") ? ",GMTWashStatus = ''" : string.Empty;
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
            string date = MyUtility.Convert.GetDate(this.CurrentMaintain["TransferOutDate"]).Value.ToString("yyyy/MM/dd");

            ReportDefinition report = new ReportDefinition();
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("RptTitle", rptTitle));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("ID", id));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Subcon", subcon));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Remark", remark));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("date", date));

            // 傳 list 資料
            List<P54_PrintData> data = this.DetailDatas.AsEnumerable()
                .Select(row1 => new P54_PrintData()
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

            Type reportResourceNamespace = typeof(P54_PrintData);
            Assembly reportResourceAssembly = reportResourceNamespace.Assembly;
            string reportResourceName = "P54_Print.rdlc";

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
            string strSubcon = MyUtility.Convert.GetString(this.CurrentMaintain["Subcon"]);
            if (MyUtility.Check.Empty(this.txtSubcon.Text))
            {
                return;
            }

            string sqlcmd = $@"select ID from ArtworkType where IsSubcon = 1 and ID = '{this.txtSubcon.Text}'";
            if (MyUtility.Check.Seek(sqlcmd))
            {
                return;
            }

            MyUtility.Msg.WarningBox(string.Format("Cannot found sub con <{0}>", strSubcon));
            e.Cancel = true;
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["Subcon"]))
            {
                MyUtility.Msg.WarningBox("Please choose sub con before import material.");
                return;
            }

            var win = new P54_Import(this.CurrentMaintain, (DataTable)this.detailgridbs.DataSource);
            win.ShowDialog(this);
            this.RenewData();
        }
    }
}
