using Ict;
using Ict.Win;

using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Sci.Data;
using System.Transactions;
using Sci.Win.Tools;
using System.Runtime.InteropServices;

namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public partial class P05 : Win.Tems.Input6
    {
        private string loginID = Env.User.UserID;
        private string keyWord = Env.User.Keyword;
        private string fileNameExt;
        private string pathName;

        /// <summary>
        /// Initializes a new instance of the <see cref="P05"/> class.
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public P05(ToolStripMenuItem menuitem)
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
                        this.DefaultWhere = string.Format("MarkerReq.FactoryID = '{0}'", this.queryfors.SelectedValue);
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
            Select a.*,
            o.styleid,o.seasonid
            From MarkerReq_Detail a WITH (NOLOCK) left join Orders o WITH (NOLOCK) on a.orderid=o.id
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
            .Text("Styleid", header: "Style", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("orderid", header: "SP#", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("Seasonid", header: "Season", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Text("SizeRatio", header: "Size Ratio", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("Markerno", header: "Flow No", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("MarkerName", header: "MarkerName", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Numeric("Layer", header: "Layers", width: Widths.AnsiChars(5), integer_places: 8, iseditingreadonly: true)
            .Text("PatternPanel", header: "PatternPanel", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("fabriccombo", header: "FabricCombo", width: Widths.AnsiChars(2), iseditingreadonly: true)
            .Text("cuttingwidth", header: "Cutting Width", width: Widths.AnsiChars(8), iseditingreadonly: true)
             .Numeric("ReqQty", header: "# of Copies", width: Widths.AnsiChars(5), integer_places: 8)
             .Numeric("ReleaseQty", header: "# of Release", width: Widths.AnsiChars(5), integer_places: 8, iseditingreadonly: true)
             .Date("ReleaseDate", header: "Release Date", width: Widths.AnsiChars(10), iseditingreadonly: true);
            this.detailgrid.Columns["ReqQty"].DefaultCellStyle.BackColor = Color.Pink;
        }

        /// <inheritdoc/>
        protected override bool ClickDeleteBefore()
        {
            #region 判斷Confirmed
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
            string sqlcmd = $@"DELETE MarkerReq_Detail_CutRef WHERE id = '{this.CurrentDetailData["ID"]}'";
            DualResult dResult = DBProxy.Current.Execute(null, sqlcmd);
            if (!dResult)
            {
                this.ShowErr(sqlcmd, dResult);
                return dResult;
            }

            return base.ClickDeletePost();
        }

        /// <inheritdoc/>
        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            DataRow[] dray = ((DataTable)this.detailgridbs.DataSource).Select("reqqty<=0");
            if (dray.Length != 0)
            {
                MyUtility.Msg.WarningBox("<# of Copies> can not be equal or less than 0");
                return;
            }
            #region update Master
            string updSql = string.Format("update MarkerReq set Status = 'Confirmed', editdate = getdate(), editname = '{0}' Where id='{1}'", this.loginID, this.CurrentMaintain["ID"]);
            #endregion
            #region transaction
            DualResult upResult;
            TransactionScope transactionscope = new TransactionScope();
            using (transactionscope)
            {
                if (!(upResult = DBProxy.Current.Execute(null, updSql)))
                {
                    transactionscope.Dispose();
                    this.ShowErr(upResult);
                    return;
                }

                transactionscope.Complete();
            }

            MyUtility.Msg.WarningBox("Successfully");
            #endregion
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.btnSendMail.Enabled = this.CurrentMaintain["Status"].ToString() != "New";
            this.displayRequestedBy.Text = PublicPrg.Prgs.GetAddOrEditBy(this.CurrentMaintain["AddName"]);
            this.label7.Text = this.CurrentMaintain["Status"].ToString();
            if (!MyUtility.Check.Empty(this.CurrentMaintain["sendDate"]))
            {
                this.displayLastSendDate.Text = Convert.ToDateTime(this.CurrentMaintain["sendDate"]).ToString("yyyy/MM/dd HH:mm:ss");
            }
        }

        /// <inheritdoc/>
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            #region 有Release 不可Unconfirm
            DataRow[] ary = ((DataTable)this.detailgridbs.DataSource).Select("releaseQty<>0");
            if (ary.Length != 0)
            {
                MyUtility.Msg.WarningBox("The record already released, you can not Unconfirm.");
                return;
            }
            #endregion
            string updSql = string.Format("update MarkerReq set Status = 'New', editdate = getdate(), editname = '{0}' Where id='{1}'", this.loginID, this.CurrentMaintain["ID"]);
            DualResult upResult;
            TransactionScope transactionscope = new TransactionScope();
            using (transactionscope)
            {
                if (!(upResult = DBProxy.Current.Execute(null, updSql)))
                {
                    transactionscope.Dispose();
                    this.ShowErr(upResult);
                    return;
                }

                transactionscope.Complete();
            }

            MyUtility.Msg.WarningBox("Successfully");
        }

        /// <inheritdoc/>
        protected override bool ClickNew()
        {
            this.detailgrid.ValidateControl();
            var frm = new P05_Import();
            DialogResult dr = frm.ShowDialog(this);

            if (dr == DialogResult.OK)
            {
                this.ReloadDatas();
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

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            string keyword = this.keyWord + "MK";
            string reqid = MyUtility.GetValue.GetID(keyword, "MarkerReq");
            if (string.IsNullOrWhiteSpace(reqid))
            {
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["ID"]))
            {
                this.CurrentMaintain["ID"] = reqid;  // 若單號為空，才要賦予新單號
            }

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override void OnDetailUIConvertToUpdate()
        {
            this.OnDetailUIConvertToMaintain();
        }

        private bool ToExcel(bool autoSave)
        {
            string cmdsql = $@"
Select o.styleid
, mrd.orderid
, o.seasonid
, mrd.sizeRatio
, mrd.markerno
, mrd.markername
, mrd.layer
, mrd.fabriccombo
, mrd.PatternPanel
, mrd.CuttingWidth
, mrd.ReqQty
, mrd.ReleaseQty
, mrd.ReleaseDate
From MarkerReq_Detail mrd WITH (NOLOCK)
left join Orders o WITH (NOLOCK) on mrd.orderid = o.id
where mrd.id = '{this.CurrentDetailData["ID"]}'";

            DualResult dResult = DBProxy.Current.Select(null, cmdsql, out DataTable excelTb);
            if (dResult)
            {
                string str = Env.Cfg.XltPathDir;
                Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Cutting_P05.xltx"); // 預先開啟excel app

                // Microsoft.Office.Interop.Excel._Workbook objBook = null;

                // if (MyUtility.Excel.CopyToXls(ExcelTb,"", "Cutting_P05.xltx", 5, !autoSave, null, objApp, false))
                if (MyUtility.Excel.CopyToXls(excelTb, string.Empty, "Cutting_P05.xltx", 5, showExcel: false, excelApp: objApp))
                {
                    // 將datatable copy to excel
                    Microsoft.Office.Interop.Excel._Worksheet objSheet = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
                    Microsoft.Office.Interop.Excel._Workbook objBook = objApp.ActiveWorkbook;

                    objSheet.Cells[1, 1] = this.keyWord;   // 條件字串寫入excel
                    objSheet.Cells[3, 2] = this.CurrentMaintain["id"].ToString();
                    objSheet.Cells[3, 4] = Convert.ToDateTime(this.CurrentMaintain["EstCutDate"]).ToShortDateString();
                    objSheet.Cells[3, 6] = this.CurrentMaintain["CutCellid"].ToString();
                    objSheet.Cells[3, 8] = PublicPrg.Prgs.GetAddOrEditBy(this.CurrentMaintain["AddName"]);
                    this.pathName = Class.MicrosoftFile.GetName("Bulk_Marker_Request");
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

                        // System.IO.File.Delete(tmpName);
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

                        // 釋放sheet
                        if (objSheet != null)
                        {
                            Marshal.FinalReleaseComObject(objSheet);
                        }

                        if (objBook != null)
                        {
                            Marshal.FinalReleaseComObject(objBook);
                        }

                        // 釋放objApp
                        if (objApp != null)
                        {
                            Marshal.FinalReleaseComObject(objApp);
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

        private void BtnSendMail_Click(object sender, EventArgs e)
        {
            // createfolder();
            if (!this.ToExcel(true))
            {
                return;
            }

            if (MyUtility.Check.Seek("select * from mailto WITH (NOLOCK) where Id='004'", out DataRow seekdr))
            {
                string mailFrom = Env.Cfg.MailFrom;
                string mailto = seekdr["ToAddress"].ToString();
                string cc = seekdr["ccAddress"].ToString();
                string content = seekdr["content"].ToString();
                string subject = "<" + this.CurrentMaintain["mDivisionid"].ToString() + ">BulkMarkerRequest#:" + this.CurrentMaintain["ID"].ToString();

                var email = new MailTo(mailFrom, mailto, cc, subject + "-" + this.fileNameExt, this.pathName, content, false, true);
                DialogResult dr = email.ShowDialog(this);
                if (dr == DialogResult.OK)
                {
                    DateTime now = DateTime.Now;
                    string sql = string.Format("Update MarkerReq set sendDate = '{0}'  where id ='{1}'", now.ToString("yyyy/MM/dd HH:mm:ss"), this.CurrentMaintain["ID"]);
                    DualResult result;
                    if (!(result = DBProxy.Current.Execute(null, sql)))
                    {
                        this.ShowErr(sql, result);
                    }
                    else
                    {
                        this.CurrentMaintain["sendDate"] = now;
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
    }
}
