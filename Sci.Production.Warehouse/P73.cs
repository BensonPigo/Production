using Ict;
using Ict.Win;
using Microsoft.Reporting.WinForms;
using Sci.Data;
using Sci.Production.Automation.LogicLayer;
using Sci.Production.Prg.Entity;
using Sci.Production.PublicPrg;
using Sci.Win;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P73 : Win.Tems.Input6
    {
        /// <inheritdoc/>
        public P73(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = string.Format("MDivisionID = '{0}'", Env.User.Keyword);

            this.gridicon.Append.Enabled = false;
            this.gridicon.Append.Visible = false;
            this.gridicon.Insert.Enabled = false;
            this.gridicon.Insert.Visible = false;
        }

        /// <inheritdoc/>
        public P73(ToolStripMenuItem menuitem, string transID)
            : base(menuitem)
        {
            this.InitializeComponent();
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
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();

            this.DetailSelectCommand = $@"
SELECT loid.*
, Seq = Concat (loid.Seq1, ' ', loid.Seq2)
,[Refno] = ISNULL(lom.Refno,'')
, [Desc] = ISNULL(lom.[Desc],'')
, [Color] = ISNULL(lom.Color,'')
FROM LocalOrderLocationTrans_Detail loid
LEFT JOIN LocalOrderMaterial lom ON lom.POID = loid.POID AND loid.Seq1 = lom.Seq1 AND loid.Seq2 = lom.Seq2
LEFT JOIN LocalOrderInventory loi ON loid.POID = loi.POID
	AND loid.Seq1 = loi.Seq1 AND loid.Seq2 = loi.Seq2 AND loid.Roll = loi.Roll AND loid.Dyelot = loi.Dyelot AND loid.StockType = loi.StockType
where   loid.ID = '{masterID}'
";
            return base.OnDetailSelectCommandPrepare(e);
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["MDivisionID"] = Env.User.Keyword;
            this.CurrentMaintain["FactoryID"] = Env.User.Factory;
            this.CurrentMaintain["Status"] = EnumStatus.New;
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
        protected override bool ClickEditBefore()
        {
            // !EMPTY(APVName) OR !EMPTY(Closed)，只能編輯remark欄。
            if (this.CurrentMaintain["Status"].EqualString("CONFIRMED"))
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't modify.", "Warning");
                return false;
            }

            return base.ClickEditBefore();
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            StringBuilder warningmsg = new StringBuilder();

            // Check ToLocation is not empty
            bool isDetailKeyColEmpty = this.DetailDatas
                                      .Where(s => MyUtility.Check.Empty(s["ToLocation"]))
                                      .Any();

            if (isDetailKeyColEmpty)
            {
                MyUtility.Msg.WarningBox("To Location cannot be empty.");
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
                string tmpId = MyUtility.GetValue.GetID(Env.User.Keyword + "OL", "LocalOrderLocationTrans", DateTime.Now, sequenceMode: 2);
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
        protected override DualResult OnRenewDataDetailPost(RenewDataPostEventArgs e)
        {
            return base.OnRenewDataDetailPost(e);
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            #region Status Label

            this.label25.Text = this.CurrentMaintain["status"].ToString();

            #endregion Status Label
        }

        /// <inheritdoc/>
        protected override void OnDetailGridInsert(int index = -1)
        {
            base.OnDetailGridInsert(index);
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            #region Location 右鍵開窗

            DataGridViewGeneratorTextColumnSettings ts2 = new DataGridViewGeneratorTextColumnSettings();
            ts2.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    Win.Tools.SelectItem2 item = Prgs.SelectLocation("B", this.CurrentDetailData["tolocation"].ToString());
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    this.CurrentDetailData["tolocation"] = item.GetSelectedString();
                }
            };

            ts2.CellValidating += (s, e) =>
            {
                if (this.EditMode && e.FormattedValue != null)
                {
                    this.CurrentDetailData["tolocation"] = e.FormattedValue;
                    string sqlcmd = string.Format(
                        @"
SELECT  id
        , Description
        , StockType 
FROM    DBO.MtlLocation WITH (NOLOCK) 
WHERE   StockType='{0}'
        and junk != '1'", this.CurrentDetailData["stocktype"].ToString());
                    DataTable dt;
                    DBProxy.Current.Select(null, sqlcmd, out dt);
                    string[] getLocation = this.CurrentDetailData["tolocation"].ToString().Split(',').Distinct().ToArray();
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
                        MyUtility.Msg.WarningBox("Cannot found Location : " + string.Join(",", errLocation.ToArray()), "Data not found");
                    }

                    trueLocation.Sort();
                    this.CurrentDetailData["tolocation"] = string.Join(",", trueLocation.ToArray());

                    // 去除錯誤的Location將正確的Location填回
                }
            };
            #endregion Location 右鍵開窗

            #region 欄位設定

            this.Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("poid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true) // 0
            .Text("seq", header: "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true) // 1
            .Text("Roll", header: "Roll#", width: Widths.AnsiChars(9), iseditingreadonly: true) // 2
            .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true) // 3
            .Text("Refno", header: "Refno", width: Widths.AnsiChars(10), iseditingreadonly: true) // 4
            .EditText("Desc", header: "Description", width: Widths.AnsiChars(15), iseditingreadonly: true) // 5
            .Text("Color", header: "Color", width: Widths.AnsiChars(5), iseditingreadonly: true) // 6
            .Numeric("qty", header: "Qty", width: Widths.AnsiChars(10), decimal_places: 2, integer_places: 10, iseditingreadonly: true) // 7
            .Text("FromLocation", header: "FromLocation", iseditingreadonly: true) // 8
            .Text("ToLocation", header: "ToLocation", settings: ts2, iseditingreadonly: false, width: Widths.AnsiChars(14)) // 9
            ;

            #endregion 欄位設定
            this.detailgrid.Columns["ToLocation"].DefaultCellStyle.BackColor = Color.Pink;
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

            if (MyUtility.Check.Empty(this.CurrentMaintain["IssueDate"]))
            {
                MyUtility.Msg.WarningBox("Issue Date cannot be empty.");
                return;
            }

            // 取得 LocalOrderInventory資料
            DualResult result = Prgs.GetLocalOrderInventoryData((DataTable)this.detailgridbs.DataSource, this.Name, out DataTable dtLocalOrderInventory);

            #region 檢查 Barcode不可為空
            if (Prgs.IsAutomation())
            {
                // 檢查 Barcode不可為空
                if (!Prgs.CheckBarCode(dtLocalOrderInventory, this.Name, isLocalOrderInventory: true))
                {
                    return;
                }
            }
            #endregion

            #region 排除Location 包含WMS & 非WMS資料
            string sqlcmd = @"
select * from
(
select * 
	, rowCnt = ROW_NUMBER() over(Partition by POID,Seq1,Seq2,Roll,Dyelot,ToLocation order by IsWMS)
	from (
		select distinct t.POID,t.Seq1,t.Seq2,t.Roll,t.Dyelot,IsWMS = isnull( ml.IsWMS,0),t.ToLocation
		from #tmp t
		outer apply(
			select ml.IsWMS
			from MtlLocation ml
			inner join dbo.SplitString(t.ToLocation,',') sp on sp.Data = ml.ID
		)ml
	) a
) final
where rowCnt = 2

drop table #tmp
";
            if (!(result = MyUtility.Tool.ProcessWithDatatable((DataTable)this.detailgridbs.DataSource, string.Empty, sqlcmd, out DataTable dtCheck)))
            {
                this.ShowErr(result);
                return;
            }
            else
            {
                if (dtCheck != null && dtCheck.Rows.Count > 0)
                {
                    string errmsg = string.Empty;
                    foreach (DataRow tmp in dtCheck.Rows)
                    {
                        errmsg += $@"SP#: {tmp["poid"]} Seq#: {tmp["seq1"]}-{tmp["seq2"]} Roll#: {tmp["roll"]} Dyelot: {tmp["Dyelot"]} ToLocation: {tmp["ToLocation"]}" + Environment.NewLine;
                    }

                    MyUtility.Msg.WarningBox("These material exists in WMS Location and non-WMS location in same time , please revise below detail location column data." + Environment.NewLine + errmsg, "Warning");
                    return;
                }
            }
            #endregion

            #region 檢查From/To Location是否為空值
            string sqlWMSLocation = $@"
select distinct td.POID,seq = concat(Ltrim(Rtrim(td.seq1)), ' ', td.Seq2),td.Roll,td.Dyelot
 , StockType = case td.StockType 
		when 'B' then 'Bulk' 
		when 'I' then 'Inventory' 
		when 'O' then 'Scrap' 
		else td.StockType 
		end
 , td.FromLocation
 , td.ToLocation
from LocalOrderLocationTrans_Detail td
left join MtlLocation m on td.FromLocation = m.ID
left join MtlLocation m2 on td.ToLocation= m2.ID
where (m.IsWMS =1 or m2.IsWMS= 1)
and (td.FromLocation = '' or td.ToLocation = '')
and td.id = '{this.CurrentMaintain["ID"]}'
";
            if (!(result = DBProxy.Current.Select(string.Empty, sqlWMSLocation, out DataTable dtLocationDetail)))
            {
                this.ShowErr(result);
                return;
            }

            if (MyUtility.Check.Seek(@"select * from System where WH_MtlTransChkLocation = 1"))
            {
                if (dtLocationDetail != null && dtLocationDetail.Rows.Count > 0)
                {
                    // change column name
                    dtLocationDetail.Columns["PoId"].ColumnName = "SP#";
                    dtLocationDetail.Columns["seq"].ColumnName = "Seq";
                    dtLocationDetail.Columns["Roll"].ColumnName = "Roll";
                    dtLocationDetail.Columns["Dyelot"].ColumnName = "Dyelot";
                    dtLocationDetail.Columns["StockType"].ColumnName = "Stock Type";
                    Prgs.ChkLocationEmpty(dtLocationDetail, "LocalOrder", "SP#,Seq,Roll,Dyelot");
                    return;
                }
            }
            #endregion

            #region 更新庫存位置  LocalOrderInventory
            var data_LocalOrder_73 = (from b in ((DataTable)this.detailgridbs.DataSource).AsEnumerable() 
                                      select new
                                {
                                    poid = b.Field<string>("poid"),
                                    seq1 = b.Field<string>("seq1"),
                                    seq2 = b.Field<string>("seq2"),
                                    stocktype = b.Field<string>("stocktype"),
                                    qty = b.Field<decimal>("qty"),
                                    ToLocation = b.Field<string>("ToLocation"),
                                    roll = b.Field<string>("roll"),
                                    dyelot = b.Field<string>("dyelot"),
                                }).ToList();

            string upd_LocalOrder_73 = Prgs.UpdateLocalOrderInventory_IO("Location", null, true);
            #endregion

            DataTable dtToWMS = ((DataTable)this.detailgridbs.DataSource).Clone();
            foreach (DataRow dr2 in this.DetailDatas)
            {
                string sqlchk = $@"
select 1 from MtlLocation m
inner join SplitString('{dr2["ToLocation"]}',',') sp on m.ID = sp.Data
where m.IsWMS = 0";
                if (MyUtility.Check.Seek(sqlchk))
                {
                    dtToWMS.ImportRow(dr2);
                }
            }

            if (!Prgs_WMS.LockNotWMS(dtToWMS))
            {
                return;
            }

            Exception errMsg = null;
            List<AutoRecord> autoRecordListP70 = new List<AutoRecord>();
            using (TransactionScope transactionscope = new TransactionScope())
            {
                try
                {
                    if (!(result = DBProxy.Current.Execute(null, $"update LocalOrderLocationTrans set status = 'Confirmed', editname = '{Env.User.UserID}', editdate = GETDATE() where id = '{this.CurrentMaintain["id"]}'")))
                    {
                        throw result.GetException();
                    }

                    if (!(result = MyUtility.Tool.ProcessWithObject(data_LocalOrder_73, string.Empty, upd_LocalOrder_73, out DataTable resulttb, "#TmpSource")))
                    {
                        throw result.GetException();
                    }

                    Prgs_WMS.UnLockorDeleteNotWMS_LocalOrder(dtToWMS, EnumStatus.Delete, autoRecordListP70, 1);
                    transactionscope.Complete();
                }
                catch (Exception ex)
                {
                    errMsg = ex;
                }
            }

            if (!MyUtility.Check.Empty(errMsg))
            {
                // P73 調整 Tolocation 不是自動倉, 過程有任何錯誤, 要發給 WMS 要求(UnLock)
                autoRecordListP70.Clear();
                Prgs_WMS.UnLockorDeleteNotWMS_LocalOrder(dtToWMS, EnumStatus.UnLock, autoRecordListP70, 1);
                Prgs_WMS.UnLockorDeleteNotWMS_LocalOrder(dtToWMS, EnumStatus.UnLock, autoRecordListP70, 2);
                this.ShowErr(errMsg);
                return;
            }

            // 調整後 Tolocation 不是自動倉, 要發給 WMS 要求撤回(Delete) P70
            Prgs_WMS.UnLockorDeleteNotWMS_LocalOrder(dtToWMS, EnumStatus.Delete, autoRecordListP70, 2);

            // AutoWHFabric WebAPI
            Prgs_WMS.WMSprocess(false, (DataTable)this.detailgridbs.DataSource, this.Name, EnumStatus.New, EnumStatus.Confirm, dtLocalOrderInventory);
            MyUtility.Msg.InfoBox("Confirmed successful");
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            this.detailgrid.EndEdit();
            var frm = new P73_Import((DataTable)this.detailgridbs.DataSource);
            frm.ShowDialog(this);
            this.RenewData();
        }

        /// <inheritdoc/>
        protected override bool ClickPrint()
        {
            if (this.CurrentMaintain["status"].ToString().ToUpper() != "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("Data is not confirmed, can't print.", "Warning");
                return false;
            }

            DataRow row = this.CurrentMaintain;
            string id = row["ID"].ToString();
            string remark = this.CurrentMaintain["Remark"].ToString();
            string issuedate = ((DateTime)MyUtility.Convert.GetDate(row["issuedate"])).ToShortDateString();

            #region  抓表頭資料
            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@MDivision", Env.User.Keyword));
            DataTable dt;
            DualResult result = DBProxy.Current.Select(string.Empty, @"select NameEN from MDivision where id = @MDivision", pars, out dt);
            if (!result)
            {
                this.ShowErr(result);
            }

            if (dt == null || dt.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found!!!", "DataTable dt");
                return false;
            }

            string rptTitle = dt.Rows[0]["NameEN"].ToString();
            ReportDefinition report = new ReportDefinition();
            report.ReportParameters.Add(new ReportParameter("RptTitle", rptTitle));
            report.ReportParameters.Add(new ReportParameter("ID", id));
            report.ReportParameters.Add(new ReportParameter("Remark", remark));
            report.ReportParameters.Add(new ReportParameter("issuedate", issuedate));
            #endregion

            #region  抓表身資料
            pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
            DataTable dd;
            string cmd = @"
select lod.POID
,[Seq] = CONCAT( lod.Seq1,' ',lod.Seq2)
,[Desc] = concat(lm.[Desc],char(10),'Color : ', lm.Color)
,lod.Seq1,lod.Seq2
,lod.Roll
,lod.Dyelot
,lm.Unit
,lod.Qty
,[StockType] = case lod.StockType 
					when 'B' then 'Bulk'
					when 'I' then 'Inventory'
					when 'O' then 'Scrap'
					else lod.StockType end
,lod.FromLocation
,[ToLocation] = Location.val	
,[Total]=sum(lod.Qty) OVER (PARTITION BY lod.POID ,lod.seq1,lod.Seq2)
from LocalOrderLocationTrans_Detail lod
left join LocalOrderInventory loi on loi.POID = lod.POID
	and lod.Seq1 = loi.Seq1 and lod.Seq2 = loi.Seq2
	and lod.Roll = loi.Roll and lod.Dyelot = loi.Dyelot
	and lod.StockType = loi.StockType
left join LocalOrderMaterial lm on lm.POID = lod.POID 
and lm.Seq1 = lod.Seq1
and lm.seq2	=lod.seq2
outer apply (SELECT val =  
Stuff(
	(select distinct concat( ',',loil.MtlLocationID)   
	from LocalOrderInventory_Location loil with (nolock) 
	where loi.Ukey = loil.LocalOrderInventoryUkey                                
	FOR XML PATH('')),1,1
,'')  
) Location
where lod.id= @ID";
            result = DBProxy.Current.Select(string.Empty, cmd, pars, out dd);
            if (!result)
            {
                this.ShowErr(result);
            }

            if (dd == null || dd.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found!!!", "DataTabe dd");
                return false;
            }

            // 傳 list 資料
            List<P26_PrintData> data = dd.AsEnumerable()
                .Select(row1 => new P26_PrintData()
                {
                    POID = row1["POID"].ToString().Trim(),
                    SEQ = row1["SEQ"].ToString().Trim(),
                    DESC = row1["DESC"].ToString().Trim(),
                    Unit = row1["unit"].ToString().Trim(),
                    Roll = row1["Roll"].ToString().Trim(),
                    Dyelot = row1["Dyelot"].ToString().Trim(),
                    QTY = row1["QTY"].ToString().Trim(),
                    From_Location = row1["FromLocation"].ToString().Trim(),
                    ToLocation = row1["ToLocation"].ToString().Trim(),
                    Total = row1["Total"].ToString().Trim(),
                    StockType = row1["StockType"].ToString().Trim(),
                }).ToList();

            report.ReportDataSource = data;
            #endregion

            // 指定是哪個 RDLC
            #region  指定是哪個 RDLC

            // DualResult result;
            Type reportResourceNamespace = typeof(P26_PrintData);
            Assembly reportResourceAssembly = reportResourceNamespace.Assembly;
            string reportResourceName = "P73_Print.rdlc";

            IReportResource reportresource;
            if (!(result = ReportResources.ByEmbeddedResource(reportResourceAssembly, reportResourceNamespace, reportResourceName, out reportresource)))
            {
                // this.ShowException(result);
                return false;
            }

            report.ReportResource = reportresource;
            #endregion

            // 開啟 report view
            var frm = new Win.Subs.ReportView(report);
            frm.MdiParent = this.MdiParent;
            frm.Show();

            return true;
        }
    }
}