using Ict;
using Ict.Win;
using Microsoft.Reporting.WinForms;
using Sci.Data;
using Sci.Production.Automation;
using Sci.Production.Automation.LogicLayer;
using Sci.Production.Prg.Entity;
using Sci.Production.PublicPrg;
using Sci.Win;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P13 : Win.Tems.Input6
    {
        private ReportViewer viewer;

        /// <inheritdoc/>
        public P13(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.InsertDetailGridOnDoubleClick = false;
            this.viewer = new ReportViewer
            {
                Dock = DockStyle.Fill,
            };
            this.Controls.Add(this.viewer);

            // MDivisionID 是 P13 寫入 => Sci.Env.User.Keyword
            this.DefaultFilter = string.Format("Type='D' and MDivisionID = '{0}'", Env.User.Keyword);
            this.gridicon.Append.Enabled = false;
            this.gridicon.Append.Visible = false;
            this.gridicon.Insert.Enabled = false;
            this.gridicon.Insert.Visible = false;
        }

        /// <inheritdoc/>
        public P13(ToolStripMenuItem menuitem, string transID)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = string.Format("Type='D' and id='{0}'", transID);
            this.IsSupportNew = false;
            this.IsSupportEdit = false;
            this.IsSupportDelete = false;
            this.IsSupportConfirm = false;
            this.IsSupportUnconfirm = false;
            this.gridicon.Append.Enabled = false;
            this.gridicon.Append.Visible = false;
            this.gridicon.Insert.Enabled = false;
            this.gridicon.Insert.Visible = false;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            bool isAutomationEnable = Automation.UtilityAutomation.IsAutomationEnable;
            this.dgToPlace.SetDefalutIndex();
            this.dgToPlace.Visible = isAutomationEnable;
            this.lblToPlace.Visible = isAutomationEnable;
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["MDivisionID"] = Env.User.Keyword;
            this.CurrentMaintain["FactoryID"] = Env.User.Factory;
            this.CurrentMaintain["Status"] = "New";
            this.CurrentMaintain["Type"] = "D";
            this.CurrentMaintain["ToPlace"] = this.dgToPlace.SelectedValue;
            this.CurrentMaintain["IssueDate"] = DateTime.Now;
        }

        /// <inheritdoc/>
        protected override bool ClickDeleteBefore()
        {
            if (this.CurrentMaintain["Status"].EqualString("CONFIRMED"))
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't delete.", "Warning");
                return false;
            }

            return base.ClickDeleteBefore();
        }

        /// <inheritdoc/>
        protected override DualResult ClickDeletePost()
        {
            // 當表身被刪除時，要判斷是否[Issue_Detail.ukey]有在[FIR_Physical].[Issue_DetailUkey]中，若有則將[FIR_Physical].[Issue_DetailUkey]更新成0。
            string iD = this.CurrentMaintain["ID"].ToString();

            List<string> ukeyList = this.DetailDatas.AsEnumerable().Select(o => o["Ukey"].ToString()).ToList();
            string ukeys = string.Join(",", ukeyList);
            string cmd = $@"
UPDATE FIR_Physical
SET Issue_DetailUkey = 0
WHERE Issue_DetailUkey IN ({ukeys})
";
            DualResult upResult;
            if (!(upResult = DBProxy.Current.Execute(null, cmd)))
            {
                return upResult;
            }

            return base.ClickDeletePost();
        }

        /// <inheritdoc/>
        protected override bool ClickEditBefore()
        {
            if (this.CurrentMaintain["Status"].EqualString("CONFIRMED"))
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't modify.", "Warning");
                return false;
            }

            return base.ClickEditBefore();
        }

        /// <inheritdoc/>
        protected override bool ClickPrint()
        {
            if (this.CurrentMaintain["status"].ToString().ToUpper() != "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("Data is not confirmed, can't print.", "Warning");
                return false;
            }

            WH_Print p = new WH_Print(this.CurrentMaintain, "P13")
            {
                CurrentDataRow = this.CurrentMaintain,
            };

            p.ShowDialog();

            // 代表要列印 RDLC
            if (p.IsPrintRDLC)
            {
                DataRow row = this.CurrentMaintain;
                string id = row["ID"].ToString();
                string remark = row["Remark"].ToString();
                string cDate = ((DateTime)MyUtility.Convert.GetDate(row["issuedate"])).ToShortDateString();
                string confirmTime = MyUtility.Convert.GetDate(row["EditDate"]).HasValue ? MyUtility.Convert.GetDate(row["EditDate"]).Value.ToString("yyyy/MM/dd HH:mm:ss") : string.Empty;
                string preparedBy = this.editby.Text;
                #region -- 撈表頭資料 --
                List<SqlParameter> pars = new List<SqlParameter>
            {
                new SqlParameter("@MDivision", Env.User.Keyword),
                new SqlParameter("@ID", id),
            };
                DualResult result = DBProxy.Current.Select(string.Empty, @"select NameEn from MDivision where id = @MDivision", pars, out DataTable dt);

                if (!result)
                {
                    this.ShowErr(result);
                }

                if (dt == null || dt.Rows.Count == 0)
                {
                    MyUtility.Msg.InfoBox("Data not found !!!", "DataTable dt");
                    return false;
                }

                string ftyGroup = string.Empty;
                foreach (DataRow item in ((DataTable)this.detailgridbs.DataSource).DefaultView.ToTable(true, "FtyGroup").Rows)
                {
                    ftyGroup += MyUtility.Convert.GetString(item["FtyGroup"]) + ",";
                }

                ftyGroup = ftyGroup.Substring(0, ftyGroup.Length - 1 >= 0 ? ftyGroup.Length - 1 : 0);

                string rptTitle = dt.Rows[0]["NameEN"].ToString();
                ReportDefinition report = new ReportDefinition();
                report.ReportParameters.Add(new ReportParameter("RptTitle", rptTitle));
                report.ReportParameters.Add(new ReportParameter("ID", id));
                report.ReportParameters.Add(new ReportParameter("Remark", remark));
                report.ReportParameters.Add(new ReportParameter("issuetime", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")));
                report.ReportParameters.Add(new ReportParameter("FtyGroup", ftyGroup));
                report.ReportParameters.Add(new ReportParameter("confirmTime", confirmTime));
                report.ReportParameters.Add(new ReportParameter("preparedBy", preparedBy));
                #endregion
                #region -- 撈表身資料 --
                pars = new List<SqlParameter>
            {
                new SqlParameter("@ID", id),
            };
                string sqlcmd = @"
select id.POID,
	    id.seq1 +  '-'  + id.seq2 as SEQ,
        p.Scirefno,
	    p.seq1,
	    p.seq2,
	    [desc] =IIF((p.ID = lag(p.ID,1,'')over (order by id.POID,p.seq1,p.seq2,fi.Tone,fi.ContainerCode,id.Dyelot,id.Roll) 
					AND(p.seq1 = lag(p.seq1,1,'')over (order by id.POID,p.seq1,p.seq2,fi.Tone,fi.ContainerCode,id.Dyelot,id.Roll))
					AND(p.seq2 = lag(p.seq2,1,'')over (order by id.POID,p.seq1,p.seq2,fi.Tone,fi.ContainerCode,id.Dyelot,id.Roll))) 
					,''
					,dbo.getMtlDesc(id.poid,id.seq1,id.seq2,2,0)
				)
				------ + Tone------
				 + char(10) + char(13) + IIF((p.ID = lag(p.ID,1,'')over (order by id.POID,p.seq1,p.seq2,fi.Tone,fi.ContainerCode,id.Dyelot,id.Roll) 
					AND(p.seq1 = lag(p.seq1,1,'')over (order by id.POID,p.seq1,p.seq2,fi.Tone,fi.ContainerCode,id.Dyelot,id.Roll))
					AND(p.seq2 = lag(p.seq2,1,'')over (order by id.POID,p.seq1,p.seq2,fi.Tone,fi.ContainerCode,id.Dyelot,id.Roll))
					AND(fi.Tone = lag(fi.Tone,1,'')over (order by id.POID,p.seq1,p.seq2,fi.Tone,fi.ContainerCode,id.Dyelot,id.Roll))) 
					,''
					,'Tone/Grp : '
                    ) + fi.Tone
				------ + MDesc------
                    + char(10) + char(13) + IIF((p.ID = lag(p.ID,1,'')over (order by id.POID,p.seq1,p.seq2,fi.Tone,fi.ContainerCode,id.Dyelot,id.Roll) 
					AND(p.seq1 = lag(p.seq1,1,'')over (order by id.POID,p.seq1,p.seq2,fi.Tone,fi.ContainerCode,id.Dyelot,id.Roll))
					AND(p.seq2 = lag(p.seq2,1,'')over (order by id.POID,p.seq1,p.seq2,fi.Tone,fi.ContainerCode,id.Dyelot,id.Roll))
					AND(fi.Tone = lag(fi.Tone,1,'')over (order by id.POID,p.seq1,p.seq2,fi.Tone,fi.ContainerCode,id.Dyelot,id.Roll))) 
					,''
					,isnull(iif(p.FabricType='F', 'Relaxation Type：'+(select FabricRelaxationID from [dbo].[SciMES_RefnoRelaxtime] where Refno = p.Refno), ''),'')
                    ),
	    id.Roll,
	    id.Dyelot,
	    id.Qty,
	    p.StockUnit,
        dbo.Getlocation(fi.ukey) [location],
        ContainerCode = IIF((p.ID = lag(p.ID,1,'')over (order by id.POID,p.seq1,p.seq2,fi.Tone,fi.ContainerCode,id.Dyelot,id.Roll) 
					AND(p.seq1 = lag(p.seq1,1,'')over (order by id.POID,p.seq1,p.seq2,fi.Tone,fi.ContainerCode,id.Dyelot,id.Roll))
					AND(p.seq2 = lag(p.seq2,1,'')over (order by id.POID,p.seq1,p.seq2,fi.Tone,fi.ContainerCode,id.Dyelot,id.Roll))
					AND(fi.ContainerCode = lag(fi.ContainerCode,1,'')over (order by id.POID,p.seq1,p.seq2,fi.Tone,fi.ContainerCode,id.Dyelot,id.Roll))
					) 
					,''
					,fi.ContainerCode),
	    [Total]=sum(id.Qty) OVER (PARTITION BY id.POID ,id.seq1, id.seq2)
		,[RecvKG] = case when rd.ActualQty is not null 
						then case when rd.ActualQty <> id.Qty
								then ''
								else cast(iif(ISNULL(rd.ActualWeight,0) > 0, rd.ActualWeight, rd.Weight) as varchar(20))
							 end
						else case when td.ActualQty <> id.Qty
								then ''
								else cast(iif(ISNULL(td.ActualWeight,0) > 0, td.ActualWeight, td.Weight) as varchar(20))
							 end							
					end
from dbo.Issue_Detail id WITH (NOLOCK) 
left join dbo.PO_Supp_Detail p WITH (NOLOCK) on p.id= id.poid and p.SEQ1 = id.Seq1 and p.seq2 = id.Seq2
left join FtyInventory fi WITH (NOLOCK) on id.POID = fi.POID
						and id.Seq1 = fi.Seq1 
						and id.Seq2 = fi.Seq2
						and id.Dyelot = fi.Dyelot
						and id.Roll = fi.Roll 
						and id.StockType = fi.StockType
Outer apply (
	select [Weight] = SUM(rd.Weight)
		, [ActualWeight] = SUM(rd.ActualWeight)
		, [ActualQty] = SUM(rd.ActualQty)
	from Receiving_Detail rd WITH (NOLOCK) 
	where fi.POID = rd.PoId
	and fi.Seq1 = rd.Seq1
	and fi.Seq2 = rd.Seq2 
	and fi.Dyelot = rd.Dyelot
	and fi.Roll = rd.Roll
	and fi.StockType = rd.StockType
	and p.FabricType = 'F'
)rd
Outer apply (
	select [Weight] = SUM(td.Weight)
		, [ActualWeight] = SUM(td.ActualWeight)
		, [ActualQty] = SUM(td.Qty)
	from TransferIn_Detail td WITH (NOLOCK) 
	where fi.POID = td.PoId
	and fi.Seq1 = td.Seq1
	and fi.Seq2 = td.Seq2 
	and fi.Dyelot = td.Dyelot
	and fi.Roll = td.Roll
	and fi.StockType = td.StockType
	and p.FabricType = 'F'
)td
where id.id= @ID
order by id.POID,p.seq1,p.seq2,fi.Tone,fi.ContainerCode,id.Dyelot,id.Roll
";
                result = DBProxy.Current.Select(string.Empty, sqlcmd, pars, out DataTable dtDetail);
                if (!result)
                {
                    this.ShowErr(sqlcmd, result);
                }

                if (dtDetail == null || dtDetail.Rows.Count == 0)
                {
                    MyUtility.Msg.InfoBox("Data not found !!!", "DataTable dtDetail");
                    return false;
                }

                // 傳 list 資料
                List<P13_PrintData> data = dtDetail.AsEnumerable()
                    .Select(row1 => new P13_PrintData()
                    {
                        POID = row1["POID"].ToString().Trim(),
                        SEQ = row1["SEQ"].ToString().Trim(),
                        DESC = row1["desc"].ToString().Trim(),
                        Location = row1["Location"].ToString().Trim() + Environment.NewLine + row1["ContainerCode"].ToString().Trim(),
                        StockUnit = row1["StockUnit"].ToString().Trim(),
                        Roll = row1["Roll"].ToString().Trim(),
                        DYELOT = row1["Dyelot"].ToString().Trim(),
                        QTY = row1["Qty"].ToString().Trim(),
                        TotalQTY = row1["Total"].ToString().Trim(),
                        RecvKG = row1["RecvKG"].ToString().Trim(),
                    }).ToList();

                report.ReportDataSource = data;
                #endregion
                #region 指定是哪個 RDLC

                // DualResult result;
                Type reportResourceNamespace = typeof(P13_PrintData);
                Assembly reportResourceAssembly = reportResourceNamespace.Assembly;
                string reportResourceName = "P13_Print.rdlc";

                if (!(result = ReportResources.ByEmbeddedResource(reportResourceAssembly, reportResourceNamespace, reportResourceName, out IReportResource reportresource)))
                {
                    // this.ShowException(result);
                    return false;
                }

                report.ReportResource = reportresource;

                // 開啟 report view
                var frm = new Win.Subs.ReportView(report)
                {
                    MdiParent = this.MdiParent,
                };
                frm.Show();
                #endregion
            }

            return base.ClickPrint();
        }

        private void MySubreportEventHandler(object sender, SubreportProcessingEventArgs e)
        {
            e.DataSources.Add(new ReportDataSource("DataSet1", (DataTable)this.detailgridbs.DataSource));
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            StringBuilder warningmsg = new StringBuilder();

            #region 必輸檢查

            if (MyUtility.Check.Empty(this.CurrentMaintain["IssueDate"]))
            {
                MyUtility.Msg.WarningBox("< Issue Date >  can't be empty!", "Warning");
                this.dateIssueDate.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["Whsereasonid"]))
            {
                MyUtility.Msg.WarningBox("< Reason >  can't be empty!", "Warning");
                this.txtwhseReason.TextBox1.Focus();
                return false;
            }
            else
            {
                if (this.CurrentMaintain["Whsereasonid"].ToString() == "00005" && MyUtility.Check.Empty(this.CurrentMaintain["remark"]))
                {
                    MyUtility.Msg.WarningBox("< Remark >  can't be empty!", "Warning");
                    this.editRemark.Focus();
                    return false;
                }
            }

            if (this.CurrentMaintain["Whsereasonid"].ToString() == "00007")
            {
                string cmd = $@"select 1 from Cutplan where MDivisionid='{Sci.Env.User.Keyword}' AND ID = '{this.CurrentMaintain["CutplanID"]}' ";
                if (!MyUtility.Check.Seek(cmd))
                {
                    MyUtility.Msg.WarningBox("There is no this <Cutting Plan ID>");
                    return false;
                }
            }

            #endregion 必輸檢查

            foreach (DataRow row in this.DetailDatas)
            {
                if (MyUtility.Check.Empty(row["seq1"]) || MyUtility.Check.Empty(row["seq2"]))
                {
                    warningmsg.Append($@"SP#: {row["poid"]} Seq#: {row["seq1"]}-{row["seq2"]} can't be empty" + Environment.NewLine);
                }

                if (MyUtility.Check.Empty(row["Qty"]))
                {
                    warningmsg.Append($@"SP#: {row["poid"]} Seq#: {row["seq1"]}-{row["seq2"]} Roll#:{row["roll"]} Dyelot:{row["dyelot"]} Issue Qty can't be empty" + Environment.NewLine);
                }
            }

            if (!MyUtility.Check.Empty(warningmsg.ToString()))
            {
                MyUtility.Msg.WarningBox(warningmsg.ToString());
                return false;
            }

            if (this.DetailDatas.Count == 0)
            {
                MyUtility.Msg.WarningBox("Detail can't be empty", "Warning");
                return false;
            }

            // 取單號
            if (this.IsDetailInserting)
            {
                string tmpId = MyUtility.GetValue.GetID(Env.User.Keyword + "II", "Issue", (DateTime)this.CurrentMaintain["Issuedate"]);
                if (MyUtility.Check.Empty(tmpId))
                {
                    MyUtility.Msg.WarningBox("Get document ID fail!!");
                    return false;
                }

                this.CurrentMaintain["id"] = tmpId;
            }

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            bool isAutomationEnable = Automation.UtilityAutomation.IsAutomationEnable;
            this.dgToPlace.Visible = isAutomationEnable;
            this.lblToPlace.Visible = isAutomationEnable;

            this.txtwhseReason.DisplayBox1.Text = MyUtility.GetValue.Lookup("Description", this.txtwhseReason.Type.ToString() + this.txtwhseReason.TextBox1.Text.ToString(), "WhseReason", "Type+ID");
            #region Status Label

            this.labelNotApprove.Text = this.CurrentMaintain["status"].ToString();

            #endregion Status Label

            if (this.CurrentMaintain["status"].ToString().EqualString("Confirmed"))
            {
                this.btnPrintFabricSticker.Enabled = true;
            }
            else
            {
                this.btnPrintFabricSticker.Enabled = false;
            }

            // System.Automation=1 和confirmed 且 有P99 Use 權限的人才可以看到此按紐
            if (UtilityAutomation.IsAutomationEnable && (this.CurrentMaintain["Status"].ToString().ToUpper() == "CONFIRMED") &&
                MyUtility.Check.Seek($@"
select * from Pass1
where (FKPass0 in (select distinct FKPass0 from Pass2 where BarPrompt = 'P99. Send to WMS command Status' and Used = 'Y') or IsMIS = 1 or IsAdmin = 1)
and ID = '{Sci.Env.User.UserID}'"))
            {
                this.btnCallP99.Visible = true;
            }
            else
            {
                this.btnCallP99.Visible = false;
            }

            this.DetailGridColVisibleByReason();
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            Ict.Win.UI.DataGridViewTextBoxColumn cbb_ContainerCode;
            #region 欄位設定
            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("FtyGroup", header: "Factory", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .CellPOIDWithSeqRollDyelot("poid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true) // 0
                .Text("seq", header: "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true) // 1
                .Text("roll", header: "Roll", width: Widths.AnsiChars(6), iseditingreadonly: true) // 2
                .Text("dyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true) // 3
                .Text("Tone", header: "Tone/Grp", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true) // 4
                .Text("stockunit", header: "Unit", iseditingreadonly: true) // 5
                .EditText("Article", header: "Article", iseditingreadonly: true, width: Widths.AnsiChars(15)) // 8
                .Numeric("NetQty", header: "Used Qty", iseditingreadonly: true, decimal_places: 2, integer_places: 10)
                .Numeric("LossQty", header: "Loss Qty", iseditingreadonly: true, decimal_places: 2, integer_places: 10)
                .Numeric("qty", header: "Issue Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10) // 6
                .Text("Location", header: "Bulk Location", iseditingreadonly: true) // 7
                .Text("ContainerCode", header: "Container Code", iseditingreadonly: true).Get(out cbb_ContainerCode)
                .Numeric("balance", header: "Stock Qty", iseditingreadonly: true, decimal_places: 2, integer_places: 10)
                .Text("Color", header: "Color", width: Widths.AnsiChars(10), iseditingreadonly: true) // 7
                .Text("Size", header: "Size", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("GMTWash", header: "GMT Wash", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("MCHandle", header: "MC Handle", width: Widths.AnsiChars(20), iseditingreadonly: true)
            ;
            #endregion 欄位設定

            // 僅有自動化工廠 ( System.Automation = 1 )才需要顯示該欄位 by ISP20220035
            cbb_ContainerCode.Visible = Automation.UtilityAutomation.IsAutomationEnable;
        }

        /// <inheritdoc/>
        protected override void ClickConfirm()
        {
            this.RenewData(); // 先重載資料, 避免雙開程式狀況
            base.ClickConfirm();
            if (this.CurrentMaintain == null)
            {
                return;
            }

            // 取得 FtyInventory 資料 (包含PO_Supp_Detail.FabricType)
            DualResult result = Prgs.GetFtyInventoryData((DataTable)this.detailgridbs.DataSource, this.Name, out DataTable dtOriFtyInventory);
            string ids = string.Empty;

            // 檢查 Barcode不可為空
            if (!Prgs.CheckBarCode(dtOriFtyInventory, this.Name))
            {
                return;
            }

            #region 檢查 FtyInventory.SubConStatus
            List<long> listFtyInventoryUkey = this.DetailDatas.Select(s => MyUtility.Convert.GetLong(s["FtyInventoryUkey"])).ToList();
            if (!Prgs_WMS.CheckFtyInventorySubConStatus(listFtyInventoryUkey))
            {
                return;
            }
            #endregion

            #region 檢查物料Location 是否存在WMS
            if (!PublicPrg.Prgs.Chk_WMS_Location(this.CurrentMaintain["ID"].ToString(), this.Name))
            {
                return;
            }
            #endregion

            #region 檢查庫存項lock
            string sqlcmd = string.Format(
                @"
Select  d.poid
        ,d.seq1
        ,d.seq2
        ,d.Roll
        ,d.Qty
        ,isnull(f.InQty,0) - isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0) as balanceQty,d.Dyelot
from dbo.Issue_Detail d WITH (NOLOCK) 
inner join FtyInventory f WITH (NOLOCK) on d.poid = f.poid and d.seq1 = f.seq1 and d.seq2 = f.seq2 and d.Dyelot = f.Dyelot
    and d.roll = f.roll and d.stocktype = f.stocktype
where f.lock=1 and d.Id = '{0}'", this.CurrentMaintain["id"]);
            if (!(result = DBProxy.Current.Select(null, sqlcmd, out DataTable datacheck)))
            {
                this.ShowErr(sqlcmd, result);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += $"SP#: {tmp["poid"]} Seq#: {tmp["seq1"]}-{tmp["seq2"]} Roll#: {tmp["roll"]} Dyelot: {tmp["Dyelot"]} is locked!!" + Environment.NewLine;
                    }

                    MyUtility.Msg.WarningBox("Material Locked!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }
            #endregion

            #region 檢查庫存項WMSLock
            if (!Prgs.ChkWMSLock(this.CurrentMaintain["id"].ToString(), "Issue_Detail"))
            {
                return;
            }
            #endregion

            #region 檢查Location是否為空值
            string sqlLocation = $@"
 select td.POID,seq = concat(Ltrim(Rtrim(td.seq1)), ' ', td.Seq2),td.Roll,td.Dyelot
 , StockType = case td.StockType 
		when 'B' then 'Bulk' 
		when 'I' then 'Inventory' 
		when 'O' then 'Scrap' 
		else td.StockType 
		end
 , [Location] = dbo.Getlocation(f.ukey)
 from Issue_Detail td
 left join Production.dbo.FtyInventory f on f.POID = td.POID 
	and f.Seq1=td.Seq1 and f.Seq2=td.Seq2 
	and f.Roll=td.Roll and f.Dyelot=td.Dyelot
    and f.StockType = td.StockType
where td.ID = '{this.CurrentMaintain["ID"]}'
";

            if (!(result = DBProxy.Current.Select(string.Empty, sqlLocation, out DataTable dtLocationDetail)))
            {
                this.ShowErr(result.ToString());
                return;
            }

            if (MyUtility.Check.Seek(@"select * from System where WH_MtlTransChkLocation = 1"))
            {
                if (dtLocationDetail != null && dtLocationDetail.Rows.Count > 0)
                {
                    // Location
                    DataRow[] dtArry = dtLocationDetail.Select(@"Location = '' or Location is null");
                    if (dtArry != null && dtArry.Length > 0)
                    {
                        DataTable dtLocation_Empty = dtArry.CopyToDataTable();

                        // change column name
                        dtLocation_Empty.Columns["PoId"].ColumnName = "SP#";
                        dtLocation_Empty.Columns["seq"].ColumnName = "Seq";
                        dtLocation_Empty.Columns["Roll"].ColumnName = "Roll";
                        dtLocation_Empty.Columns["Dyelot"].ColumnName = "Dyelot";
                        dtLocation_Empty.Columns["StockType"].ColumnName = "Stock Type";

                        Prgs.ChkLocationEmpty(dtLocation_Empty, string.Empty, @"SP#,Seq,Roll,Dyelot,Stock Type");
                        return;
                    }
                }
            }

            #endregion

            #region 檢查負數庫存

            sqlcmd = string.Format(
                @"
Select  d.poid
        ,d.seq1
        ,d.seq2
        ,d.Roll
        ,d.Qty
        ,isnull(f.InQty,0) - isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0) as balanceQty,d.Dyelot
from dbo.Issue_Detail d WITH (NOLOCK) 
left join FtyInventory f WITH (NOLOCK) on d.poid = f.poid and d.seq1 = f.seq1 and d.seq2 = f.seq2 and d.Dyelot = f.Dyelot
    and d.roll = f.roll and d.stocktype = f.stocktype
where (isnull(f.InQty,0) - isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0) - d.Qty < 0) and d.Id = '{0}'", this.CurrentMaintain["id"]);
            if (!(result = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                this.ShowErr(sqlcmd, result);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += $"SP#: {tmp["poid"]} Seq#: {tmp["seq1"]}-{tmp["seq2"]} Roll#: {tmp["roll"]} Dyelot: {tmp["Dyelot"]}'s balance: {tmp["balanceqty"]} is less than issue qty: {tmp["qty"]}" + Environment.NewLine;
                    }

                    MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }

            #endregion 檢查負數庫存

            #region 更新庫存數量  ftyinventory
            var bs1 = (from b in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
                       group b by new
                       {
                           poid = b.Field<string>("poid"),
                           seq1 = b.Field<string>("seq1"),
                           seq2 = b.Field<string>("seq2"),
                           stocktype = b.Field<string>("stocktype"),
                       }
                        into m
                       select new Prgs_POSuppDetailData
                       {
                           Poid = m.First().Field<string>("poid"),
                           Seq1 = m.First().Field<string>("seq1"),
                           Seq2 = m.First().Field<string>("seq2"),
                           Stocktype = m.First().Field<string>("stocktype"),
                           Qty = m.Sum(w => w.Field<decimal>("qty")),
                       }).ToList();
            StringBuilder sqlupd2_B = new StringBuilder();
            sqlupd2_B.Append(Prgs.UpdateMPoDetail(4, null, true));
            var bsfio = (from m in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
                         select new
                         {
                             poid = m.Field<string>("poid"),
                             seq1 = m.Field<string>("seq1"),
                             seq2 = m.Field<string>("seq2"),
                             stocktype = m.Field<string>("stocktype"),
                             qty = m.Field<decimal>("qty"),
                             location = m.Field<string>("location"),
                             roll = m.Field<string>("roll"),
                             dyelot = m.Field<string>("dyelot"),
                         }).ToList();
            string sqlupd2_FIO = Prgs.UpdateFtyInventory_IO(4, null, true);
            #endregion

            Exception errMsg = null;
            using (TransactionScope transactionscope = new TransactionScope(TransactionScopeOption.Required, new TimeSpan(0, 20, 0)))
            {
                try
                {
                    DataTable resulttb;
                    if (!(result = MyUtility.Tool.ProcessWithObject(bs1, string.Empty, sqlupd2_B.ToString(), out resulttb, "#TmpSource")))
                    {
                        throw result.GetException();
                    }

                    if (!(result = MyUtility.Tool.ProcessWithObject(bsfio, string.Empty, sqlupd2_FIO, out resulttb, "#TmpSource")))
                    {
                        throw result.GetException();
                    }

                    // Barcode 需要判斷新的庫存, 在更新 FtyInventory 之後
                    if (!(result = Prgs.UpdateWH_Barcode(true, (DataTable)this.detailgridbs.DataSource, this.Name, out bool fromNewBarcode, dtOriFtyInventory)))
                    {
                        throw result.GetException();
                    }

                    if (!(result = DBProxy.Current.Execute(null, $"update Issue set status = 'Confirmed', editname = '{Env.User.UserID}', editdate = GETDATE() where id = '{this.CurrentMaintain["id"]}'")))
                    {
                        throw result.GetException();
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

            // AutoWHFabric WebAPI
            Prgs_WMS.WMSprocess(false, (DataTable)this.detailgridbs.DataSource, this.Name, EnumStatus.New, EnumStatus.Confirm, dtOriFtyInventory);
            MyUtility.Msg.InfoBox("Confirmed successful");
        }

        /// <inheritdoc/>
        protected override void ClickUnconfirm()
        {
            this.RenewData(); // 先重載資料, 避免雙開程式狀況
            base.ClickUnconfirm();
            if (this.CurrentMaintain == null ||
                MyUtility.Msg.QuestionBox("Do you want to unconfirme it?") == DialogResult.No)
            {
                return;
            }

            DBProxy.Current.DefaultTimeout = 1200; // Timeout 為20分鐘

            // 取得 FtyInventory 資料
            DualResult result = Prgs.GetFtyInventoryData((DataTable)this.detailgridbs.DataSource, this.Name, out DataTable dtOriFtyInventory);
            string ids = string.Empty;

            #region 檢查物料Location 是否存在WMS
            if (!PublicPrg.Prgs.Chk_WMS_Location(this.CurrentMaintain["ID"].ToString(), this.Name))
            {
                MyUtility.Msg.WarningBox("Material Location is from WMS system cannot confirmed or unconfirmed. ", "Warning");
                DBProxy.Current.DefaultTimeout = 300; // Timeout 為5分鐘
                return;
            }
            #endregion

            #region 檢查庫存項lock
            string sqlcmd = string.Format(
                @"
Select  d.poid
        ,d.seq1
        ,d.seq2
        ,d.Roll
        ,d.Qty
        ,isnull(f.InQty,0) - isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0) as balanceQty,d.Dyelot
from dbo.Issue_Detail d WITH (NOLOCK) 
inner join FtyInventory f WITH (NOLOCK) on d.poid = f.poid and d.seq1 = f.seq1 and d.seq2 = f.seq2 and d.Dyelot = f.Dyelot
    and d.roll = f.roll and d.stocktype = f.stocktype
where f.lock=1 and d.Id = '{0}'", this.CurrentMaintain["id"]);
            if (!(result = DBProxy.Current.Select(null, sqlcmd, out DataTable datacheck)))
            {
                this.ShowErr(sqlcmd, result);
                DBProxy.Current.DefaultTimeout = 300; // Timeout 為5分鐘
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += $"SP#: {tmp["poid"]} Seq#: {tmp["seq1"]}-{tmp["seq2"]} Roll#: {tmp["roll"]} Dyelot: {tmp["Dyelot"]} is locked!!" + Environment.NewLine;
                    }

                    MyUtility.Msg.WarningBox("Material Locked!!" + Environment.NewLine + ids, "Warning");
                    DBProxy.Current.DefaultTimeout = 300; // Timeout 為5分鐘
                    return;
                }
            }
            #endregion

            #region 檢查庫存項WMSLock
            if (!Prgs.ChkWMSLock(this.CurrentMaintain["id"].ToString(), "Issue_Detail"))
            {
                DBProxy.Current.DefaultTimeout = 300; // Timeout 為5分鐘
                return;
            }
            #endregion

            #region 檢查負數庫存

            sqlcmd = string.Format(
                @"
Select  d.poid
        ,d.seq1
        ,d.seq2
        ,d.Roll
        ,d.Qty
        ,isnull(f.InQty,0) - isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0) as balanceQty,d.Dyelot   
from dbo.Issue_Detail d WITH (NOLOCK) 
left join FtyInventory f WITH (NOLOCK) on d.poid = f.poid and d.seq1 = f.seq1 and d.seq2 = f.seq2 and d.Dyelot = f.Dyelot
    and d.roll = f.roll and d.stocktype = f.stocktype
where (isnull(f.InQty,0) - isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0) + d.Qty < 0) and d.Id = '{0}'", this.CurrentMaintain["id"]);
            if (!(result = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                this.ShowErr(sqlcmd, result);
                DBProxy.Current.DefaultTimeout = 300; // Timeout 為5分鐘
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += $"SP#: {tmp["poid"]} Seq#: {tmp["seq1"]}-{tmp["seq2"]} Roll#: {tmp["roll"]} Dyelot: {tmp["Dyelot"]}'s balance: {tmp["balanceqty"]} is less than issue qty: {tmp["qty"]}" + Environment.NewLine;
                    }

                    MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                    DBProxy.Current.DefaultTimeout = 300; // Timeout 為5分鐘
                    return;
                }
            }

            #endregion 檢查負數庫存

            #region 檢查資料有任一筆WMS已完成, 就不能unConfirmed
            if (!Prgs.ChkWMSCompleteTime((DataTable)this.detailgridbs.DataSource, "Issue_Detail"))
            {
                return;
            }
            #endregion

            #region 更新庫存數量  ftyinventory
            var bs1 = (from b in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
                       group b by new
                       {
                           poid = b.Field<string>("poid"),
                           seq1 = b.Field<string>("seq1"),
                           seq2 = b.Field<string>("seq2"),
                           stocktype = b.Field<string>("stocktype"),
                       }
                        into m
                       select new Prgs_POSuppDetailData
                       {
                           Poid = m.First().Field<string>("poid"),
                           Seq1 = m.First().Field<string>("seq1"),
                           Seq2 = m.First().Field<string>("seq2"),
                           Stocktype = m.First().Field<string>("stocktype"),
                           Qty = -m.Sum(w => w.Field<decimal>("qty")),
                       }).ToList();
            StringBuilder sqlupd2_B = new StringBuilder();
            sqlupd2_B.Append(Prgs.UpdateMPoDetail(4, null, false));

            var bsfio = (from m in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
                         select new
                         {
                             poid = m.Field<string>("poid"),
                             seq1 = m.Field<string>("seq1"),
                             seq2 = m.Field<string>("seq2"),
                             stocktype = m.Field<string>("stocktype"),
                             qty = -m.Field<decimal>("qty"),
                             location = m.Field<string>("location"),
                             roll = m.Field<string>("roll"),
                             dyelot = m.Field<string>("dyelot"),
                         }).ToList();
            string sqlupd2_FIO = Prgs.UpdateFtyInventory_IO(4, null, false);
            #endregion

            #region UnConfirmed 廠商能上鎖→PMS更新→廠商更新

            // 先確認 WMS 能否上鎖, 不能直接 return
            if (!Prgs_WMS.WMSLock((DataTable)this.detailgridbs.DataSource, dtOriFtyInventory, this.Name, EnumStatus.Unconfirm))
            {
                return;
            }

            // PMS 的資料更新
            Exception errMsg = null;
            List<AutoRecord> autoRecordList = new List<AutoRecord>();
            using (TransactionScope transactionscope = new TransactionScope(TransactionScopeOption.Required, new TimeSpan(0, 20, 0)))
            {
                try
                {
                    DataTable resulttb;
                    if (!(result = MyUtility.Tool.ProcessWithObject(bs1, string.Empty, sqlupd2_B.ToString(), out resulttb, "#TmpSource")))
                    {
                        throw result.GetException();
                    }

                    if (!(result = MyUtility.Tool.ProcessWithObject(bsfio, string.Empty, sqlupd2_FIO, out resulttb, "#TmpSource")))
                    {
                        throw result.GetException();
                    }

                    if (!(result = Prgs.UpdateWH_Barcode(false, (DataTable)this.detailgridbs.DataSource, this.Name, out bool fromNewBarcode, dtOriFtyInventory)))
                    {
                        throw result.GetException();
                    }

                    if (!(result = DBProxy.Current.Execute(null, $@"update Issue set status = 'New', editname = '{Env.User.UserID}', editdate = GETDATE() where id = '{this.CurrentMaintain["id"]}'")))
                    {
                        throw result.GetException();
                    }

                    // transactionscope 內, 準備 WMS 資料 & 將資料寫入 AutomationCreateRecord (Delete, Unconfirm)
                    Prgs_WMS.WMSprocess(false, (DataTable)this.detailgridbs.DataSource, this.Name, EnumStatus.Delete, EnumStatus.Unconfirm, dtOriFtyInventory, typeCreateRecord: 1, autoRecord: autoRecordList);
                    transactionscope.Complete();
                }
                catch (Exception ex)
                {
                    errMsg = ex;
                }
            }

            if (!MyUtility.Check.Empty(errMsg))
            {
                Prgs_WMS.WMSUnLock(false, (DataTable)this.detailgridbs.DataSource, this.Name, EnumStatus.UnLock, EnumStatus.Unconfirm, dtOriFtyInventory);
                this.ShowErr(errMsg);
                DBProxy.Current.DefaultTimeout = 300; // Timeout 為5分鐘
                return;
            }

            // PMS 更新之後,才執行WMS
            Prgs_WMS.WMSprocess(false, (DataTable)this.detailgridbs.DataSource, this.Name, EnumStatus.Delete, EnumStatus.Unconfirm, dtOriFtyInventory, typeCreateRecord: 2, autoRecord: autoRecordList);
            MyUtility.Msg.InfoBox("UnConfirmed successful");
            #endregion

            DBProxy.Current.DefaultTimeout = 300; // Timeout 為5分鐘
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();
            this.DetailSelectCommand = $@"
select  o.FtyGroup
        , a.id
        , a.PoId
        , a.Seq1
        , a.Seq2
        , concat(Ltrim(Rtrim(a.seq1)), ' ', a.Seq2) as seq
        , psd.FabricType
        , psd.stockunit
        , dbo.getmtldesc(a.poid,a.seq1,a.seq2,2,0) as [description]
        , a.Roll
        , a.Dyelot
        , c.Tone
        , a.Qty
        , a.StockType
        , Isnull(c.inqty - c.outqty + c.adjustqty - c.ReturnQty,0.00) as balance
        , dbo.Getlocation(c.ukey) location
        , [ContainerCode] = c.ContainerCode
        , a.ukey
		, psd.NetQty
		, psd.LossQty
        , [Article] = case  when a.Seq1 like 'T%' then Stuff((Select distinct concat( ',',tcd.Article) 
			                                                         From dbo.Orders as o 
			                                                         Inner Join dbo.Style as s On s.Ukey = o.StyleUkey
			                                                         Inner Join dbo.Style_ThreadColorCombo as tc On tc.StyleUkey = s.Ukey
			                                                         Inner Join dbo.Style_ThreadColorCombo_Detail as tcd On tcd.Style_ThreadColorComboUkey = tc.Ukey 
			                                                         where	o.POID = a.PoId and
			                                                         		tcd.SuppId = p.SuppId and
			                                                         		tcd.SCIRefNo   = psd.SCIRefNo	and
			                                                         		tcd.ColorID	   = isnull(psdsC.SpecValue, '')
			                                                         FOR XML PATH('')),1,1,'') 
                            else '' end
        , [Color] =
			IIF(f.MtlTypeID = 'EMB THREAD' OR f.MtlTypeID = 'SP THREAD' OR f.MtlTypeID = 'THREAD' 
			,IIF( psd.SuppColor = '' or psd.SuppColor is null,dbo.GetColorMultipleID(o.BrandID,isnull(psdsC.SpecValue, '')),psd.SuppColor)
			,dbo.GetColorMultipleID(o.BrandID,isnull(psdsC.SpecValue, '')))
		, [Size]= isnull(psdsS.SpecValue, '')
        , [GMTWash] = isnull(GMTWash.val, '')
        , [FtyInventoryUkey] = c.Ukey
        , [MCHandle] = (select IdAndName from dbo.GetPassName(o.MCHandle))
from dbo.issue_detail as a WITH (NOLOCK) 
left join Orders o on a.poid = o.id
left join PO_Supp_Detail psd WITH (NOLOCK) on psd.ID = a.PoId and psd.seq1 = a.SEQ1 and psd.SEQ2 = a.seq2
left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
left join PO_Supp_Detail_Spec psdsS WITH (NOLOCK) on psdsS.ID = psd.id and psdsS.seq1 = psd.seq1 and psdsS.seq2 = psd.seq2 and psdsS.SpecColumnID = 'Size'
left join PO_Supp p WITH (NOLOCK) on p.ID = psd.ID and psd.seq1 = p.SEQ1
left join dbo.ftyinventory c WITH (NOLOCK) on c.poid = a.poid and c.seq1 = a.seq1 and c.seq2  = a.seq2 
    and c.stocktype = 'B' and c.roll=a.roll and a.Dyelot = c.Dyelot
left join fabric f with(nolock) on f.SCIRefno = psd.SCIRefno
outer apply(
    select top 1 [val] =  case  when sr.Status = 'Confirmed' then 'Done'
			                    when tt.Status = 'Confirmed' then 'Ongoing'
			                    else '' end
    from TransferToSubcon_Detail ttd with (nolock)
    inner join TransferToSubcon tt with (nolock) on tt.ID = ttd.ID
    left join  SubconReturn_Detail srd with (nolock) on srd.TransferToSubcon_DetailUkey = ttd.Ukey
    left join  SubconReturn sr with (nolock) on sr.ID = srd.ID and sr.Status = 'Confirmed'
    where   ttd.POID = c.PoId and
			ttd.Seq1 = c.Seq1 and 
            ttd.Seq2 = c.Seq2 and
			ttd.Dyelot = c.Dyelot and 
            ttd.Roll = c.Roll and
			ttd.StockType = c.StockType and
            tt.Subcon = 'GMT Wash'
) GMTWash
Where a.id = '{masterID}'
";

            return base.OnDetailSelectCommandPrepare(e);
        }

        private void BtnClearQtyIsEmpty_Click(object sender, EventArgs e)
        {
            this.detailgrid.ValidateControl();

            // detailgridbs.EndEdit();
            // ((DataTable)detailgridbs.DataSource).Select("qty=0.00 or qty is null").ToList().ForEach(r => ((DataTable)detailgridbs.DataSource).Rows.Remove(r));
            // 不可以用 Remove()，因為datatable還沒有更新回資料庫中，被Remove的資料沒有真正刪除。
            ((DataTable)this.detailgridbs.DataSource).Select("qty=0.00 or qty is null").ToList().ForEach(r => r.Delete());
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["WhseReasonID"]))
            {
                MyUtility.Msg.WarningBox("Please choose reason first !!");
                return;
            }

            var frm = new P13_Import(this.CurrentMaintain, (DataTable)this.detailgridbs.DataSource);
            frm.ShowDialog(this);
            this.RenewData();
        }

        private void BtnAccumulatedQty_Click(object sender, EventArgs e)
        {
            var frm = new P13_AccumulatedQty(this.CurrentMaintain)
            {
                P13 = this,
            };
            frm.ShowDialog(this);
        }

        private void BtnFind_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.detailgridbs.DataSource))
            {
                return;
            }

            int index = this.detailgridbs.Find("poid", this.txtLocateForSP.Text.TrimEnd());
            if (index == -1)
            {
                MyUtility.Msg.WarningBox("Data was not found!!");
            }
            else
            {
                this.detailgridbs.Position = index;
            }
        }

        private void BtCutRef_Click(object sender, EventArgs e)
        {
            var frm = new P10_CutRef(this.CurrentMaintain);
            frm.ShowDialog(this);
        }

        private void BtnPrintFabricSticker_Click(object sender, EventArgs e)
        {
            new P13_FabricSticker(this.CurrentMaintain["ID"], "Issue_Detail").ShowDialog();
        }

        private void TxtwhseReason_Validated(object sender, EventArgs e)
        {
            this.DetailGridColVisibleByReason();
        }

        private void TxtwhseReason_Leave(object sender, EventArgs e)
        {
            this.DetailGridColVisibleByReason();
        }

        private void DetailGridColVisibleByReason()
        {
            if (this.CurrentMaintain["whseReasonID"].ToString() == "00006")
            {
                this.detailgrid.Columns["NetQty"].Visible = true;
                this.detailgrid.Columns["LossQty"].Visible = true;
                this.detailgrid.Columns["Article"].Visible = true;
            }
            else
            {
                this.detailgrid.Columns["NetQty"].Visible = false;
                this.detailgrid.Columns["LossQty"].Visible = false;
                this.detailgrid.Columns["Article"].Visible = false;
            }

            if (this.CurrentMaintain["whseReasonID"].ToString() == "00007" && MyUtility.Convert.GetString(this.CurrentMaintain["status"]) == "New")
            {
            }
            else
            {
                if (this.EditMode)
                {
                    this.CurrentMaintain["CutplanID"] = string.Empty;
                }
            }

            this.txtCuttingPlanID.ReadOnly = !this.EditMode;
        }

        private void BtnCallP99_Click(object sender, EventArgs e)
        {
            P99_CallForm.CallForm(this.CurrentMaintain["ID"].ToString(), this.Name, this);
        }

        private void TxtCuttingPlanID_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string selectCommand = $@"
select ID
from Cutplan
where MDivisionid = '{Sci.Env.User.Keyword}'
";
            Win.Tools.SelectItem item = new Win.Tools.SelectItem(selectCommand, "20", MyUtility.Convert.GetString(this.CurrentMaintain["CutplanID"]));

            DialogResult returnResult = item.ShowDialog();

            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.CurrentMaintain["CutplanID"] = item.GetSelectedString();
        }

        private void TxtCuttingPlanID_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string cuttingPlanID = this.txtCuttingPlanID.Text;
            if (MyUtility.Check.Empty(cuttingPlanID))
            {
                return;
            }

            string cmd = $@"
select ID
from Cutplan
where MDivisionid = '{Sci.Env.User.Keyword}'
AND ID = @ID
";
            bool exists = MyUtility.Check.Seek(cmd, new List<SqlParameter>() { new SqlParameter("@ID", cuttingPlanID) });

            if (!exists || MyUtility.Convert.GetString(this.CurrentMaintain["Whsereasonid"]) != "00007")
            {
                this.txtCuttingPlanID.Text = string.Empty;
                e.Cancel = true;
                MyUtility.Msg.WarningBox("Cutting Plan ID not found, or W/H Reason is not 00007.");
                return;
            }
        }
    }
}