using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Automation;
using Sci.Win.Tools;
using System;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public partial class P04 : Win.Tems.Input6
    {
        private string loginID = Env.User.UserID;
        private string keyWord = Env.User.Keyword;
        private string fileNameExt;
        private string pathName;

        /// <summary>
        /// Initializes a new instance of the <see cref="P04"/> class.
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public P04(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = string.Format("MDivisionID = '{0}'", this.keyWord);
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            string querySql = string.Format(
                @"
select '' FTYGroup

union 
select distinct FTYGroup 
from Factory 
where MDivisionID = '{0}'", Env.User.Keyword);
            DBProxy.Current.Select(null, querySql, out DataTable queryDT);
            MyUtility.Tool.SetupCombox(this.queryfors, 1, queryDT);
            this.queryfors.SelectedIndex = 0;
            this.queryfors.SelectedIndexChanged += (s, e) =>
            {
                switch (this.queryfors.SelectedIndex)
                {
                    case 0:
                        this.DefaultWhere = string.Empty;
                        break;
                    default:
                        this.DefaultWhere = string.Format("(select FactoryID from Cutting where Cutplan.CuttingID = Cutting.ID) = '{0}'", this.queryfors.SelectedValue);
                        break;
                }

                this.ReloadDatas();
            };
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["id"].ToString();
            string cmdsql = string.Format(
            @"
            Select a.*,e.FabricCombo,e.seq1,e.seq2,e.FabricCode,e.SCIRefno,e.Refno,
            [Article] = stuff((Select distinct CONCAT('/ ', wpd.Article)
                            From dbo.WorkOrderForPlanning_Distribute wpd WITH (NOLOCK) 
                            Where wpd.WorkOrderForPlanningUkey = a.WorkOrderForPlanningUkey and wpd.Article!=''
                            For XML path('')),1,1,''),
            (
                Select c.sizecode+'/ '+convert(varchar(8),c.qty)+', ' 
                From WorkOrderForPlanning_SizeRatio c WITH (NOLOCK) 
                Where  c.WorkOrderForPlanningUkey =a.WorkOrderForPlanningUkey 
                
                For XML path('')
            ) as SizeCode,
            (
                Select c.sizecode+'/ '+convert(varchar(8),c.qty*e.layer)+', ' 
                From WorkOrderForPlanning_SizeRatio c WITH (NOLOCK) 
                Where  c.WorkOrderForPlanningUkey =a.WorkOrderForPlanningUkey and c.WorkOrderForPlanningUkey = e.Ukey
               
                For XML path('')
            ) as CutQty,
			e.FabricPanelCode
			,f.WeaveTypeID
            ,[FabricIssued] = 
	        (
	            SELECT val = IIF(SUM(iss.Qty) >= 1, 'Y', 'N')
                FROM Issue i
                INNER JOIN Issue_Summary iss ON i.id = iss.Id
                WHERE i.CutplanID = a.id AND iss.SCIRefno = e.SCIRefno AND iss.Colorid = a.colorid AND i.Status = 'Confirmed'
	        )
            ,[Issue_Qty] = 
	        (        
                SELECT val = isnull(SUM(iss.Qty),0)
                FROM Issue i
                INNER JOIN Issue_Summary iss ON i.id = iss.Id
                WHERE i.CutplanID = a.id AND iss.SCIRefno = e.SCIRefno AND iss.Colorid = a.colorid
	        )
            ,[Reason] = isnull(Reason.[Description], '')
            ,[ReasonID] = ci.Reason
            ,[EstCutDate] = isnull(ci.EstCutDate, b.EstCutDate)
            ,[EditName] = isnull(ci.EditName,'')
            ,[EditDate] = ci.EditDate
            ,[RequestorRemark] = isnull(ci.RequestorRemark,'')
            From Cutplan_Detail a 
            inner join Cutplan b WITH(NOLOCK) on a.id = b.ID
			INNER JOIN WorkOrderForPlanning e WITH (NOLOCK) ON a.WorkOrderForPlanningUkey = e.Ukey
			LEFT JOIN Fabric f WITH (NOLOCK) ON f.SCIRefno=e.SCIRefno
            LEFT JOIN CutPlan_IssueCutDate ci WITH (NOLOCK) on ci.id = b.id and ci.Refno = e.Refno and ci.Colorid = a.colorid
            LEFT JOIN CutReason Reason with (nolock) ON Reason.Junk = 0 AND Reason.type = 'RC' AND Reason.id = ci.Reason
            where a.id = '{0}'
            ", masterID);
            this.DetailSelectCommand = cmdsql;
            return base.OnDetailSelectCommandPrepare(e);
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();
            this.Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("Sewinglineid", header: "Line#", width: Widths.Auto(), iseditingreadonly: true)
            .Text("Cutref", header: "CutRef#", width: Widths.Auto(), iseditingreadonly: true)
            .Numeric("Cutno", header: "Seq", width: Widths.Auto(), integer_places: 3, iseditingreadonly: true)
            .Text("Fabriccombo", header: "Fabric Combo", width: Widths.Auto(), iseditingreadonly: true)
            .Text("Fabriccode", header: "Fabric Code", width: Widths.Auto(), iseditingreadonly: true)
            .Text("FabricPanelCode", header: "Fab_Panel Code", width: Widths.Auto(), iseditingreadonly: true)
            .Text("orderid", header: "SP#", width: Widths.Auto(), iseditingreadonly: true)
            .Text("WeaveTypeID", header: "Weave Type", width: Widths.Auto(), iseditingreadonly: true)
            .Text("SCIRefno", header: "SCIRefno", width: Widths.Auto(), iseditingreadonly: true)
            .Text("Refno", header: "Refno", width: Widths.Auto(), iseditingreadonly: true)
            .Text("Article", header: "Article", width: Widths.Auto(), iseditingreadonly: true)
            .Text("Colorid", header: "Color", width: Widths.Auto(), iseditingreadonly: true)
            .Text("EstCutDate", header: "Est. Cutting Date", width: Widths.Auto(), iseditingreadonly: true)
            .Text("Reason", header: "Reason", width: Widths.Auto(), iseditingreadonly: true)
            .Text("FabricIssued", header: "FabricIssued", width: Widths.Auto(), iseditingreadonly: true)
            .Text("SEQ1", header: "SEQ1", width: Widths.Auto(), iseditingreadonly: true)
            .Text("SEQ2", header: "SEQ2", width: Widths.Auto(), iseditingreadonly: true)
            .Text("SizeCode", header: "Size", width: Widths.Auto(), iseditingreadonly: true)
            .Text("CutQty", header: "Total CutQty", width: Widths.Auto(), iseditingreadonly: true)
            .Numeric("Cons", header: "Cons", width: Widths.Auto(), integer_places: 8, decimal_places: 2, iseditingreadonly: true)
            .Text("Remark", header: "Remark", width: Widths.Auto())
            .Text("RequestorRemark", header: "Requestor Remark", width: Widths.Auto())
            .Date("EditDate", header: "EditDate", width: Widths.Auto());

            this.detailgrid.Columns["Remark"].DefaultCellStyle.BackColor = Color.Pink;
        }

        /// <inheritdoc/>
        protected override bool ClickDeleteBefore()
        {
            #region 判斷Encode 不可,MarkerReqid 存在
            if (this.CurrentMaintain["Status"].ToString() == "Confirmed")
            {
                MyUtility.Msg.WarningBox("The record status is confimed, you can not delete.");
                return false;
            }
            #endregion

            return base.ClickDeleteBefore();
        }

        /// <inheritdoc/>
        protected override DualResult ClickDeletePost()
        {
            #region 清空WorkOrderForPlanning 的Cutplanid
            string clearCutplanidSql = string.Format("Update WorkOrderForPlanning set cutplanid ='' where cutplanid ='{0}'", this.CurrentMaintain["ID"]);
            #endregion
            DualResult upResult;
            if (!(upResult = DBProxy.Current.Execute(null, clearCutplanidSql)))
            {
                return upResult;
            }

            return base.ClickDeletePost();
        }

        /// <inheritdoc/>
        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            #region 建立Cutplan_Detail_Cons 資料
            string insert_cons = string.Format(
                        @"insert into Cutplan_Detail_Cons(id,poid,seq1,seq2,cons) 
                        select a.id,a.poid,b.seq1,b.seq2,sum(a.cons) as tt 
                        from Cutplan_Detail a WITH (NOLOCK) ,WorkOrderForPlanning b WITH (NOLOCK) 
                        where a.id='{0}' and a.WorkOrderForPlanningUkey = b.Ukey 
                        group by a.id,a.poid,b.seq1,b.seq2", this.CurrentMaintain["ID"]);
            #endregion
            string insertmk = string.Empty;
            string insert_mark2 = string.Empty;

            #region update Master

            // 1386: CUTTING_P04_Cutting Daily Plan。CONFIRM時，須回寫更新MarkerReqid。
            string updSql = $"update Cutplan set Status = 'Confirmed', editdate = getdate(), editname = '{this.loginID}' Where id='{this.CurrentMaintain["ID"]}'";
            #endregion
            #region transaction
            DualResult upResult;
            using (TransactionScope transactionscope = new TransactionScope())
            {
                if (!(upResult = DBProxy.Current.Execute(null, updSql)))
                {
                    transactionscope.Dispose();
                    this.ShowErr(upResult);
                    return;
                }

                if (!(upResult = DBProxy.Current.Execute(null, insert_cons)))
                {
                    transactionscope.Dispose();
                    this.ShowErr(upResult);
                    return;
                }

                if (insertmk != string.Empty)
                {
                    if (!(upResult = DBProxy.Current.Execute(null, insertmk)))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(upResult);
                        return;
                    }
                }

                if (insert_mark2 != string.Empty)
                {
                    if (!(upResult = DBProxy.Current.Execute(null, insert_mark2)))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(upResult);
                        return;
                    }
                }

                transactionscope.Complete();
            }

            this.SentToGensong_AutoWHFabric(true);
            MyUtility.Msg.InfoBox("Successfully");
            #endregion
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.btnSendMail.Enabled = this.CurrentMaintain["Status"].ToString() != "New";

            bool isConfirm = MyUtility.Convert.GetString(this.CurrentMaintain["Status"]) == "Confirmed";
            this.btnFabDeleteHistory.Enabled = isConfirm;
            this.btnEditFabricCutDate.Enabled = isConfirm & this.Perm.Edit && this.Perm.Delete;
            this.btnFabDelete.Enabled = isConfirm & this.Perm.Edit && this.Perm.Delete;
            this.detailgrid.AutoResizeColumns();
        }

        /// <inheritdoc/>
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            #region 有IssueFabric 不可Uncomfirm
            string query = string.Format("Select * from Issue WITH (NOLOCK) Where Cutplanid ='{0}'", this.CurrentMaintain["ID"]);
            DualResult dResult = DBProxy.Current.Select(null, query, out DataTable queryIssueFabric);
            if (dResult)
            {
                if (queryIssueFabric.Rows.Count != 0)
                {
                    MyUtility.Msg.WarningBox("The record already issued fabric, you can not Unconfirm.");
                    return;
                }
            }
            else
            {
                this.ShowErr(query, dResult);
                return;
            }
            #endregion
            string updSql = string.Format("Delete cutplan_Detail_Cons where id ='{1}';update Cutplan set Status = 'New', editdate = getdate(), editname = '{0}' Where id='{1}'", this.loginID, this.CurrentMaintain["ID"]);
            DualResult upResult;
            using (TransactionScope transactionscope = new TransactionScope())
            {
                if (!(upResult = DBProxy.Current.Execute(null, updSql)))
                {
                    transactionscope.Dispose();
                    this.ShowErr(upResult);
                    return;
                }

                transactionscope.Complete();
            }

            this.SentToGensong_AutoWHFabric(false);
            MyUtility.Msg.WarningBox("Successfully");
        }

        private void SentToGensong_AutoWHFabric(bool isConfirmed)
        {
            DataTable dtDetail = ((DataTable)this.detailgridbs.DataSource).DefaultView.ToTable(true, "ID", "WorkOrderForPlanningUkey");
            Gensong_AutoWHFabric.SentCutplan_Detail(true, dtDetail, isConfirmed);
        }

        /// <inheritdoc/>
        protected override bool ClickNew()
        {
            this.detailgrid.ValidateControl();
            var frm = new P04_Import();
            DialogResult dr = frm.ShowDialog(this);

            // dr == System.Windows.Forms.DialogResult.
            this.ReloadDatas();
            if (dr == DialogResult.OK)
            {
                var topID = frm.ImportedIDs[0];
                int newDataIdx = this.gridbs.Find("ID", topID);
                this.gridbs.Position = newDataIdx;
            }

            return true;
        }

        /// <inheritdoc/>
        protected override bool ClickEditBefore()
        {
            #region 判斷Encode 不可,MarkerReqid 存在
            if (this.CurrentMaintain["Status"].ToString() == "Confirmed")
            {
                MyUtility.Msg.WarningBox("The record status is confimed, you can not modify.");
                return false;
            }
            #endregion
            return base.ClickEditBefore();
        }

        private void Btnimport_Click(object sender, EventArgs e)
        {
            this.detailgrid.ValidateControl();
            var frm = new P04_Import();
            frm.ShowDialog(this);
        }

        private bool ToExcel(bool autoSave)
        {
            if (MyUtility.Check.Empty(this.CurrentDetailData))
            {
                MyUtility.Msg.InfoBox("No any data.");
                return false;
            }

            string cmdsql = string.Format(
@"select    cd.id,
            cd.sewinglineid,
            cd.orderid,
            w.seq1,
            w.seq2,
            cd.StyleID,
            cd.cutref,
            cd.cutno,
            w.FabricCombo,
            w.FabricCode,
            [SizeCode] = (
                            Select c.sizecode+'/ '+convert(varchar(8),c.qty)+', ' 
                            From WorkOrderForPlanning_SizeRatio c WITH (NOLOCK) 
                            Where  c.WorkOrderForPlanningUkey =cd.WorkOrderForPlanningUkey 
                            For XML path('')
                        ),
            [Article] = stuff((Select distinct CONCAT('/ ', wpd.Article)
                            From dbo.WorkOrderForPlanning_Distribute wpd WITH (NOLOCK) 
                            Where wpd.WorkOrderForPlanningUkey = cd.WorkOrderForPlanningUkey and wpd.Article!=''
                            For XML path('')),1,1,''),
            cd.colorid,
            [CutQty] = (
                            Select c.sizecode+'/ '+convert(varchar(8),c.qty*w.layer)+', ' 
                            From WorkOrderForPlanning_SizeRatio c WITH (NOLOCK) 
                            Where  c.WorkOrderForPlanningUkey =cd.WorkOrderForPlanningUkey and c.WorkOrderForPlanningUkey = w.Ukey
                            For XML path('')
                        ),
            cd.cons,
            isnull(f.DescDetail,'') as DescDetail,
            [EstCutDate] = iif(isnull(IsCutPlan_IssueCutDate.val,0) = 1 , IsCutPlan_IssueCutDate.EstCutDate ,b.EstCutDate),
            [Reason] = iif(isnull(IsCutPlan_IssueCutDate.val,0) = 1 , IsCutPlan_IssueCutDate.Reason , ''),
            [FabricIssued] = (
                                 select val = iif(SUM(1) >= 1 ,'Y','N') 
                              from Issue i WITH (NOLOCK)
                              inner join Issue_Summary iss WITH (NOLOCK) on i.id = iss.Id
                              where i.CutplanID = cd.id and iss.SCIRefno = w.SCIRefno and iss.Colorid = cd.colorid and i.Status = 'Confirmed'
                             ),
            cd.remark 
            from Cutplan_Detail cd WITH (NOLOCK) 
            inner join Cutplan b WITH(NOLOCK) on cd.id = b.ID
            inner join WorkOrderForPlanning w with (nolock) on cd.WorkOrderForPlanningUkey = w.Ukey
            left join Fabric f on f.SCIRefno = w.SCIRefno
            OUTER APPLY (
                         select 
                         [val] = isnull(IIF(COUNT(*) OVER (PARTITION BY ci.id, ci.Refno, ci.colorid) >= 1, 1, 0),0),
                         [EstCutDate] = isnull(ci.EstCutDate,''),
                         [Reason] = isnull(Reason.[Description],''),
                            [EditName] = isnull(ci.EditName,''),
                            [EditDate] = ci.EditDate,
                            [RequestorRemark] = isnull(ci.RequestorRemark,'')
                         from CutPlan_IssueCutDate ci WITH(NOLOCK)
                         LEFT JOIN CutReason Reason ON Reason.Junk = 0 AND Reason.type = 'RC' AND Reason.id = ci.Reason
                         where ci.id = b.id and ci.Refno = w.Refno and ci.Colorid = cd.colorid
                        )IsCutPlan_IssueCutDate
            where cd.id = '{0}'", this.CurrentDetailData["ID"]);
            DualResult dResult = DBProxy.Current.Select(null, cmdsql, out DataTable excelTb);

            if (dResult)
            {
                Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Cutting_P04.xltx"); // 預先開啟excel app

                // createfolder();
                // if (MyUtility.Excel.CopyToXls(ExcelTb, "", "Cutting_P04.xltx", 5, !autoSave, null, objApp, false))
                if (MyUtility.Excel.CopyToXls(excelTb, string.Empty, "Cutting_P04.xltx", 5, showExcel: false, excelApp: objApp))
                {// 將datatable copy to excel
                    Microsoft.Office.Interop.Excel._Worksheet objSheet = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
                    Microsoft.Office.Interop.Excel._Workbook objBook = objApp.ActiveWorkbook;

                    objSheet.Cells[1, 1] = this.keyWord;   // 條件字串寫入excel
                    objSheet.Cells[3, 2] = this.dateCuttingDate.Text;
                    objSheet.Cells[3, 5] = this.CurrentMaintain["POID"].ToString();
                    objSheet.Cells[3, 10] = this.CurrentMaintain["CutCellid"].ToString();
                    objSheet.Cells[3, 15] = PublicPrg.Prgs.GetAddOrEditBy(this.loginID);
                    this.pathName = Class.MicrosoftFile.GetName("Cutting_Daily_Plan");
                    objBook.SaveAs(this.pathName);
                    if (autoSave)
                    {
                        objBook.Close();
                        objApp.Workbooks.Close();
                        objApp.Quit();

                        Marshal.ReleaseComObject(objApp);
                        Marshal.ReleaseComObject(objSheet);
                        Marshal.ReleaseComObject(objBook);

                        if (objSheet != null)
                        {
                            Marshal.FinalReleaseComObject(objSheet);
                        }

                        if (objBook != null)
                        {
                            Marshal.FinalReleaseComObject(objBook);
                        }

                        if (objApp != null)
                        {
                            Marshal.FinalReleaseComObject(objApp);
                        }

                        objApp = null;
                        this.fileNameExt = this.pathName.Substring(this.pathName.LastIndexOf("\\") + 1);
                    }
                    else
                    {
                        objBook.Close();
                        objApp.Workbooks.Close();
                        objApp.Quit();

                        Marshal.ReleaseComObject(objApp);
                        Marshal.ReleaseComObject(objSheet);
                        Marshal.ReleaseComObject(objBook);

                        if (objSheet != null)
                        {
                            Marshal.FinalReleaseComObject(objSheet);    // 釋放sheet
                        }

                        if (objBook != null)
                        {
                            Marshal.FinalReleaseComObject(objBook);
                        }

                        if (objApp != null)
                        {
                            Marshal.FinalReleaseComObject(objApp);          // 釋放objApp
                        }

                        this.pathName.OpenFile();
                    }
                }
            }
            else
            {
                this.ShowErr(cmdsql, dResult);
                return false;
            }

            return true;
        }

        // protected void createfolder()
        // {
        //    if (!Directory.Exists(Sci.Env.Cfg.ReportTempDir))
        //        Directory.CreateDirectory(Sci.Env.Cfg.ReportTempDir);
        // }
        private void BtnSendMail_Click(object sender, EventArgs e)
        {
            // createfolder();
            if (!this.ToExcel(true))
            {
                return;
            }

            if (MyUtility.Check.Seek("select * from mailto WITH (NOLOCK) where Id='005'", out DataRow seekdr))
            {
                string mailFrom = Env.Cfg.MailFrom;
                string mailto = seekdr["ToAddress"].ToString();
                string cc = seekdr["ccAddress"].ToString();
                string content = seekdr["content"].ToString();
                string subject = "<" + this.CurrentMaintain["mDivisionid"].ToString() + ">BulkMarkerRequest#:" + this.CurrentMaintain["ID"].ToString();
                var email = new MailTo(mailFrom, mailto, cc, subject + "-" + this.fileNameExt, this.pathName, content, false, true);
                DialogResult dR = email.ShowDialog(this);
                if (dR == DialogResult.OK)
                {
                    DateTime nOW = DateTime.Now;
                    string sql = string.Format("Update MarkerReq set sendDate = '{0}'  where id ='{1}'", nOW.ToString("yyyy/MM/dd HH:mm:ss"), this.CurrentMaintain["ID"]);
                    DualResult result;
                    if (!(result = DBProxy.Current.Execute(null, sql)))
                    {
                        this.ShowErr(sql, result);
                    }
                    else
                    {
                        this.OnDetailEntered();
                    }
                }
            }

            // 刪除Excel File
            if (System.IO.File.Exists(this.pathName))
            {
                try
                {
                    System.IO.File.Delete(this.pathName);
                }
                catch (System.IO.IOException)
                {
                    MyUtility.Msg.WarningBox("Delete excel file fail!!");
                }
            }
        }

        /// <inheritdoc/>
        protected override bool ClickPrint()
        {
            this.ToExcel(false);
            return base.ClickPrint();
        }

        private void BtnFabricIssueList_Click(object sender, EventArgs e)
        {
            this.detailgrid.ValidateControl();
            var frm = new P04_FabricIssueList(this.CurrentMaintain["ID"].ToString().Trim());
            frm.ShowDialog(this);
        }

        private void BtnEditFabCutDate_Click(object sender, EventArgs e)
        {
            var dt = (DataTable)this.detailgridbs.DataSource;
            var frm = new P04_EditFabricCutDate(dt);
            frm.ShowDialog();
            this.RenewData();
        }

        private void BtnFabDelete_Click(object sender, EventArgs e)
        {
            var dt = (DataTable)this.detailgridbs.DataSource;
            var frm = new P04_FabricDelete(dt);
            frm.ShowDialog();
            this.RenewData();
        }

        private void BtnFabDeleteHistory_Click(object sender, EventArgs e)
        {
            var frm = new P04_FabricDeleteHistory(this.CurrentMaintain["ID"].ToString());
            frm.ShowDialog();
        }
    }
}