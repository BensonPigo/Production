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
                        this.DefaultWhere = string.Format("(select CT.FactoryID from Cutplan CP left join Cutting CT on CP.CuttingID=CT.ID where CP.ID = MarkerReq.Cutplanid) = '{0}'", this.queryfors.SelectedValue);
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
            #region 清空Cutplan 的request
            string clearCutplanidSql = string.Format("Update Cutplan set MarkerReqid ='' where MarkerReqid ='{0}'", this.CurrentMaintain["ID"]);
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
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["Status"] = "New";
            this.CurrentMaintain["mDivisionid"] = this.keyWord;
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
            if (MyUtility.Check.Empty(this.CurrentMaintain["cutplanid"]))
            {
                MyUtility.Msg.WarningBox("<Cutplan ID> can not be empty!");
                return false;
            }

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
        protected override DualResult ClickSavePost()
        {
            string updateCutplan = string.Format("Update Cutplan set MarkerReqid = '{0}' where id ='{1}'", this.CurrentMaintain["ID"], this.CurrentMaintain["Cutplanid"]);
            DualResult upResult;
            if (!(upResult = DBProxy.Current.Execute(null, updateCutplan)))
            {
                return upResult;
            }

            return base.ClickSavePost();
        }

        /// <inheritdoc/>
        protected override void OnDetailUIConvertToUpdate()
        {
            this.OnDetailUIConvertToMaintain();
            this.txtCutplan.ReadOnly = true;
        }

        private void TxtCutplan_Validating(object sender, CancelEventArgs e)
        {
            if (!this.EditMode)
            {
                return;
            }

            if (this.txtCutplan.OldValue.ToString() == this.txtCutplan.Text)
            {
                return;
            }

            string cmd = string.Format("Select * from Cutplan WITH (NOLOCK) Where id='{0}' and mDivisionid = '{1}'", this.txtCutplan.Text, this.keyWord);
            if (!MyUtility.Check.Seek(cmd, out DataRow cutdr, null))
            {
                this.txtCutplan.Text = string.Empty;
                MyUtility.Msg.WarningBox("<Cutplan ID> data not found!");
                return;
            }

            if (cutdr["markerreqid"].ToString() != string.Empty)
            {
                this.txtCutplan.Text = string.Empty;
                MyUtility.Msg.WarningBox(string.Format("<Cutplan ID> already created Bulk Marker Request<{0}>", cutdr["markerreqid"]));
                return;
            }

            if (cutdr["Status"].ToString() != "Confirmed")
            {
                this.txtCutplan.Text = string.Empty;
                MyUtility.Msg.WarningBox("The Cutplan not yet confirm.");
                return;
            }

            foreach (DataRow dr in this.DetailDatas)
            {
                dr.Delete();
            }
        }

        private void TxtCutplan_Validated(object sender, EventArgs e)
        {
            this.OnValidated(e);
            string cmd = string.Format("Select * from Cutplan WITH (NOLOCK) Where id='{0}' and mDivisionid = '{1}'", this.txtCutplan.Text, this.keyWord);
            if (MyUtility.Check.Seek(cmd, out DataRow cutdr, null))
            {
                this.displayCutCell.Text = cutdr["Cutcellid"].ToString();

                if (MyUtility.Check.Empty(cutdr["estcutdate"]))
                {
                    this.dateCuttingDate.Text = string.Empty;
                }
                else
                {
                    this.dateCuttingDate.Text = Convert.ToDateTime(cutdr["estcutdate"]).ToShortDateString();
                }

                string marker2sql = string.Format(
                    @"
;with t as (
Select o.POID
       , b.MarkerName
       , layer = sum(b.Layer)
       , b.MarkerNo
       , b.fabricCombo
       , PatternPanel = (Select PatternPanel+'+ ' 
                         From WorkOrder_PatternPanel c WITH (NOLOCK) 
                         Where c.WorkOrderUkey =a.WorkOrderUkey 
                         For XML path(''))
       , cuttingwidth = (Select cuttingwidth 
                         from Order_EachCons b WITH (NOLOCK)
                              , WorkOrder e WITH (NOLOCK) 
                         where e.Order_EachconsUkey = b.Ukey 
                               and a.WorkOrderUkey = e.Ukey)
       , o.styleid
       , o.seasonid
From Cutplan_Detail a WITH (NOLOCK) 
     , WorkOrder b WITH (NOLOCK) 
inner join Orders o WITH (NOLOCK) on b.orderid = o.ID
Where a.workorderukey = b.ukey 
      and a.id = '{0}' 
Group by o.POID,b.MarkerName,b.MarkerNo
         , b.fabricCombo,a.WorkOrderUkey
		 ,o.styleid,o.seasonid
)
select StyleID,Seasonid,POID,MarkerNo,Markername,FabricCombo,PatternPanel,cuttingwidth,sum(layer)as layer 
into #temp1
from t
group by StyleID,Seasonid,POID,MarkerNo,Markername,FabricCombo,PatternPanel,cuttingwidth
order by Markername



Select distinct o.POID
       , b.MarkerName
       , b.MarkerNo
       , b.fabricCombo
       , SizeRatio = (Select c.sizecode + '*' + convert (varchar(8), c.qty) + '/' 
                      From WorkOrder_SizeRatio c WITH (NOLOCK) 
                      Where a.WorkOrderUkey = c.WorkOrderUkey            
                      For XML path(''))        
       , o.styleid
       , o.seasonid
	   into #temp2
From Cutplan_Detail a WITH (NOLOCK) 
     , WorkOrder b WITH (NOLOCK) 
inner join Orders o WITH (NOLOCK) on b.orderid = o.id	
Where a.workorderukey = b.ukey 
      and a.id = '{0}'
order by Markername


select a.* 
,sizeRatio= (select  b.SizeRatio +''
	from #temp2 b
	where b.POID=a.POID
	and a.styleid=b.styleid
	and a.seasonid=b.seasonid
	and a.MarkerName=b.MarkerName
	and a.MarkerNo=b.MarkerNo
	and a.fabricCombo=b.fabricCombo
	For XML path(''))
from #temp1 a
order by Markername

DROP TABLE #temp1,#temp2

", this.txtCutplan.Text);

                DataTable gridTb = (DataTable)this.detailgridbs.DataSource;
                DualResult dResult = DBProxy.Current.Select(null, marker2sql, out DataTable markerTb);
                foreach (DataRow dr in markerTb.Rows)
                {
                    DataRow ndr = gridTb.NewRow();
                    ndr["styleid"] = dr["styleid"];
                    ndr["seasonid"] = dr["seasonid"];
                    ndr["orderid"] = dr["poid"];
                    ndr["SizeRatio"] = dr["SizeRatio"];
                    ndr["MarkerName"] = dr["MarkerName"];
                    ndr["Layer"] = dr["Layer"];
                    ndr["FabricCombo"] = dr["FabricCombo"];
                    ndr["MarkerNo"] = dr["MarkerNo"];
                    ndr["PatternPanel"] = dr["PatternPanel"];
                    ndr["cuttingwidth"] = dr["cuttingwidth"];
                    gridTb.Rows.Add(ndr);
                }
            }
        }

        private bool ToExcel(bool autoSave)
        {
            string cmdsql = string.Format(
            @"
            Select  o.styleid,a.orderid,o.seasonid,a.sizeRatio,a.markerno,a.markername,
                    a.layer,a.fabriccombo,
            (
                Select PatternPanel+'+ ' 
                From WorkOrder_PatternPanel c WITH (NOLOCK) 
                Where c.WorkOrderUkey =a.WorkOrderUkey 
                For XML path('')
            ) as PatternPanel,
            (
                Select cuttingwidth from Order_EachCons b WITH (NOLOCK) , WorkOrder e WITH (NOLOCK) 
                where e.Order_EachconsUkey = b.Ukey and a.WorkOrderUkey = e.Ukey  
            ) as cuttingwidth,ReqQty,ReleaseQty,ReleaseDate
            From MarkerReq_Detail a WITH (NOLOCK) left join Orders o WITH (NOLOCK) on a.orderid=o.id
            where a.id = '{0}'
            ", this.CurrentDetailData["ID"]);
            DualResult dResult = DBProxy.Current.Select(null, cmdsql, out DataTable excelTb);
            if (dResult)
            {
                string str = Env.Cfg.XltPathDir;
                Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Cutting_P05.xltx"); // 預先開啟excel app

                // Microsoft.Office.Interop.Excel._Workbook objBook = null;

                // if (MyUtility.Excel.CopyToXls(ExcelTb,"", "Cutting_P05.xltx", 5, !autoSave, null, objApp, false))
                if (MyUtility.Excel.CopyToXls(excelTb, string.Empty, "Cutting_P05.xltx", 5, showExcel: false, excelApp: objApp))
                {// 將datatable copy to excel
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
