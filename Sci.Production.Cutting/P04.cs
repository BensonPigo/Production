﻿using Ict;
using Ict.Win;
using Sci.Production.Class;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Sci.Win;
using Sci.Data;
using System.Transactions;
using Sci.Win.Tools;
using System.Runtime.InteropServices;
using System.IO;

namespace Sci.Production.Cutting
{
    public partial class P04 : Sci.Win.Tems.Input6
    {
        private string loginID = Sci.Env.User.UserID;
        private string keyWord = Sci.Env.User.Keyword;
        string fileNameExt, pathName;
        public P04(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.DefaultFilter = string.Format("MDivisionID = '{0}'", keyWord);
        }
        protected override DualResult OnDetailSelectCommandPrepare(Win.Tems.InputMasterDetail.PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["id"].ToString();
            string cmdsql = string.Format(
            @"
            Select a.*,e.FabricCombo,e.seq1,e.seq2,e.FabricCode,e.SCIRefno,e.Refno,
            (
                Select distinct Article+'/ ' 
			    From dbo.WorkOrder_Distribute b WITH (NOLOCK) 
			    Where b.workorderukey = a.WorkOrderUkey and b.article!=''
                For XML path('')
            ) as article,
            (
                Select c.sizecode+'/ '+convert(varchar(8),c.qty)+', ' 
                From WorkOrder_SizeRatio c WITH (NOLOCK) 
                Where  c.WorkOrderUkey =a.WorkOrderUkey 
                
                For XML path('')
            ) as SizeCode,
            (
                Select c.sizecode+'/ '+convert(varchar(8),c.qty*e.layer)+', ' 
                From WorkOrder_SizeRatio c WITH (NOLOCK) 
                Where  c.WorkOrderUkey =a.WorkOrderUkey and c.WorkOrderUkey = e.Ukey
               
                For XML path('')
            ) as CutQty,
			e.FabricPanelCode
            From Cutplan_Detail a WITH (NOLOCK) , WorkOrder e WITH (NOLOCK) 
            where a.id = '{0}' and a.WorkOrderUkey = e.Ukey
            ", masterID);
            this.DetailSelectCommand = cmdsql;
            return base.OnDetailSelectCommandPrepare(e);
        }
        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();
            Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("Sewinglineid", header: "Line#", width: Widths.Auto(), iseditingreadonly: true)
            .Text("Cutref", header: "CutRef#", width: Widths.Auto(), iseditingreadonly: true)
            .Numeric("Cutno", header: "Cut#", width: Widths.Auto(), integer_places: 3, iseditingreadonly: true)
            .Text("Fabriccombo", header: "Fabric Combo", width: Widths.Auto(), iseditingreadonly: true)
            .Text("Fabriccode", header: "Fabric Code", width: Widths.Auto(), iseditingreadonly: true)
            .Text("FabricPanelCode", header: "Fab_Panel Code", width: Widths.Auto(), iseditingreadonly: true)
            .Text("orderid", header: "SP#", width: Widths.Auto(), iseditingreadonly: true)
            .Text("SEQ1", header: "SEQ1", width: Widths.Auto(), iseditingreadonly: true)
            .Text("SEQ2", header: "SEQ2", width: Widths.Auto(), iseditingreadonly: true)
            .Text("Article", header: "Article", width: Widths.Auto(), iseditingreadonly: true)
            .Text("Colorid", header: "Color", width: Widths.Auto(), iseditingreadonly: true)
            .Text("SizeCode", header: "Size", width: Widths.Auto(), iseditingreadonly: true)
            .Text("CutQty", header: "Total CutQty", width: Widths.Auto(), iseditingreadonly: true)
            .Text("SCIRefno", header: "SCIRefno", width: Widths.Auto(), iseditingreadonly: true)
            .Text("Refno", header: "Refno", width: Widths.Auto(), iseditingreadonly: true)
            .Numeric("Cons", header: "Cons", width: Widths.Auto(), integer_places: 8, decimal_places: 2, iseditingreadonly: true)
            .Text("Remark", header: "Remark", width: Widths.Auto());
            this.detailgrid.Columns["Remark"].DefaultCellStyle.BackColor = Color.Pink;            
        }
        protected override bool ClickDeleteBefore()
        {
            #region 判斷Encode 不可,MarkerReqid 存在
            if (CurrentMaintain["Status"].ToString() == "Confirmed")
            {
                MyUtility.Msg.WarningBox("The record status is confimed, you can not delete.");
                return false;
            }
            #endregion

            return base.ClickDeleteBefore();
        }
        protected override DualResult ClickDeletePost()
        {
            #region 清空WorkOrder 的Cutplanid
            string clearCutplanidSql = string.Format("Update WorkOrder set cutplanid ='' where cutplanid ='{0}'", CurrentMaintain["ID"]);
            #endregion
            DualResult upResult;
            if (!(upResult = DBProxy.Current.Execute(null, clearCutplanidSql)))
            {
                return upResult;
            }
            return base.ClickDeletePost();
        }
        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            #region 建立Cutplan_Detail_Cons 資料
            DataTable detailTb = (DataTable)detailgridbs.DataSource;
            string insert_cons = string.Format(
                        @"insert into Cutplan_Detail_Cons(id,poid,seq1,seq2,cons) 
                        select a.id,a.poid,b.seq1,b.seq2,sum(a.cons) as tt 
                        from Cutplan_Detail a WITH (NOLOCK) ,workorder b WITH (NOLOCK) 
                        where a.id='{0}' and a.workorderukey = b.Ukey 
                        group by a.id,a.poid,b.seq1,b.seq2", CurrentMaintain["ID"]);
            #endregion
            string insertmk = "";
            string insert_mark2 = "";
            #region 建立Bulk request

            #region ID
            string keyword = keyWord + "MK";
            string reqid = MyUtility.GetValue.GetID(keyword, "MarkerReq");
            if (string.IsNullOrWhiteSpace(reqid))
            {
                return;
            }
            #endregion
            insertmk = string.Format(
            @"Insert into MarkerReq
            (id,estcutdate,mDivisionid,CutCellid,Status,Cutplanid,AddName,AddDate) 
            values('{0}','{1}','{2}','{3}','New','{4}','{5}',getdate());",
            reqid,dateCuttingDate.Text, CurrentMaintain["mDivisionid"],
            CurrentMaintain["cutcellid"], CurrentMaintain["ID"], loginID);

            #region 表身
            string marker2sql = string.Format(
            @"Select b.Orderid,b.MarkerName,sum(b.Layer) as layer,
            b.MarkerNo,a.WorkOrderUkey,b.fabricCombo,
            (
                Select c.sizecode+'*'+convert(varchar(8),c.qty)+'/' 
                From WorkOrder_SizeRatio c WITH (NOLOCK) 
                Where a.WorkOrderUkey =c.WorkOrderUkey            
                For XML path('')
            ) as SizeRatio
            From Cutplan_Detail a WITH (NOLOCK) , WorkOrder b WITH (NOLOCK) 
            Where a.workorderukey = b.ukey and a.id = '{0}'
			Group by b.Orderid,b.MarkerName,b.MarkerNo,
                b.fabricCombo,a.WorkOrderUkey", CurrentMaintain["ID"]);
            #endregion
            DataTable markerTb;

            DualResult dResult = DBProxy.Current.Select(null, marker2sql, out markerTb);
            if (dResult)
            {
                foreach (DataRow dr in markerTb.Rows)
                {
                    insert_mark2 = insert_mark2 + string.Format(
                    @"Insert into MarkerReq_Detail      
                    (ID,OrderID,SizeRatio,MarkerName,Layer,FabricCombo,MarkerNo,WorkOrderUkey) 
                    Values('{0}','{1}','{2}','{3}',{4},'{5}','{6}','{7}');",
                        reqid, dr["OrderID"], dr["SizeRatio"], dr["MarkerName"],
                        dr["Layer"], dr["FabricCombo"], dr["MarkerNo"], dr["WorkOrderUkey"]);
                }
            }
            else
            {
                ShowErr(marker2sql, dResult);
                return;
            }
            #endregion

            #region update Master
            //1386: CUTTING_P04_Cutting Daily Plan。CONFIRM時，須回寫更新MarkerReqid。
            string updSql = string.Format("update Cutplan set  MarkerReqid = '{2}' , Status = 'Confirmed', editdate = getdate(), editname = '{0}' Where id='{1}'", loginID, CurrentMaintain["ID"], reqid);
            #endregion
            #region transaction
            DualResult upResult;
            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    if (!(upResult = DBProxy.Current.Execute(null, updSql)))
                    {
                        _transactionscope.Dispose();
                        ShowErr("Commit transaction error.");
                        return;
                    }
                    if (!(upResult = DBProxy.Current.Execute(null, insert_cons)))
                    {
                        _transactionscope.Dispose();
                        ShowErr("Commit transaction error.");
                        return;
                    }
                    if (insertmk != "")
                    {
                        if (!(upResult = DBProxy.Current.Execute(null, insertmk)))
                        {
                            _transactionscope.Dispose();
                            ShowErr("Commit transaction error.");
                            return;
                        }
                    }
                    if (insert_mark2 != "")
                    {
                        if (!(upResult = DBProxy.Current.Execute(null, insert_mark2)))
                        {
                            _transactionscope.Dispose();
                            ShowErr("Commit transaction error.");
                            return;
                        }
                    }
                    _transactionscope.Complete();
                    _transactionscope.Dispose();
                    MyUtility.Msg.InfoBox("Successfully");
                }
                catch (Exception ex)//絕對進不來catch
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
            EnsureToolbarExt();
            #endregion

        }
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.btnSendMail.Enabled = (CurrentMaintain["Status"].ToString() != "New");
            this.detailgrid.AutoResizeColumns();
        }
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            #region 有Marker Req 不可Unconfirm
            if (!MyUtility.Check.Empty(CurrentMaintain["markerreqid"]))
            {
                MyUtility.Msg.WarningBox("The record already create Marker request, you can not Unconfirm.");
                return;
            }
            #endregion
            #region 有IssueFabric 不可Uncomfirm
            DataTable queryIssueFabric;
            string Query = string.Format("Select * from Issue WITH (NOLOCK) Where Cutplanid ='{0}'", CurrentMaintain["ID"]);
            DualResult dResult = DBProxy.Current.Select(null, Query, out queryIssueFabric);
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
                ShowErr(Query, dResult);
                return;
            }
            #endregion
            string updSql = string.Format("Delete cutplan_Detail_Cons where id ='{1}';update Cutplan set Status = 'New', editdate = getdate(), editname = '{0}' Where id='{1}'", loginID, CurrentMaintain["ID"]);
            DualResult upResult;
            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    if (!(upResult = DBProxy.Current.Execute(null, updSql)))
                    {
                        _transactionscope.Dispose();
                        return;
                    }
                    _transactionscope.Complete();
                    _transactionscope.Dispose();
                    MyUtility.Msg.WarningBox("Successfully");
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
            EnsureToolbarExt();
        }
        protected override bool ClickNew()
        {
            detailgrid.ValidateControl();
            var frm = new Sci.Production.Cutting.P04_Import();
            frm.ShowDialog(this);
            this.ReloadDatas();
            return true;
        }
        
        protected override bool ClickEditBefore()
        {
            #region 判斷Encode 不可,MarkerReqid 存在
            if (CurrentMaintain["Status"].ToString() == "Confirmed")
            {
                MyUtility.Msg.WarningBox("The record status is confimed, you can not modify.");
                return false;
            }
            #endregion
            return base.ClickEditBefore();
        }

        private void btnimport_Click(object sender, EventArgs e)
        {
            detailgrid.ValidateControl();
            var frm = new Sci.Production.Cutting.P04_Import();
            frm.ShowDialog(this);
        }

        private bool ToExcel(bool autoSave)
        {
            if (MyUtility.Check.Empty(CurrentDetailData))
            {
                MyUtility.Msg.InfoBox("No any data.");
                return false;
            }
            DataTable ExcelTb;
            string cmdsql = string.Format(
            @"select cd.id,cd.sewinglineid,cd.orderid,w.seq1,w.seq2,cd.StyleID,cd.cutref,cd.cutno,w.FabricCombo,w.FabricCode,
(
    Select c.sizecode+'/ '+convert(varchar(8),c.qty)+', ' 
    From WorkOrder_SizeRatio c WITH (NOLOCK) 
    Where  c.WorkOrderUkey =cd.WorkOrderUkey 
                
    For XML path('')
) as SizeCode,
(
    Select distinct Article+'/ ' 
	From dbo.WorkOrder_Distribute b WITH (NOLOCK) 
	Where b.workorderukey = cd.WorkOrderUkey and b.article!=''
    For XML path('')
) as article,cd.colorid,
(
    Select c.sizecode+'/ '+convert(varchar(8),c.qty*w.layer)+', ' 
    From WorkOrder_SizeRatio c WITH (NOLOCK) 
    Where  c.WorkOrderUkey =cd.WorkOrderUkey and c.WorkOrderUkey = w.Ukey
               
    For XML path('')
) as CutQty,
cd.cons,isnull(f.DescDetail,'') as DescDetail,cd.remark 
from Cutplan_Detail cd WITH (NOLOCK) 
inner join WorkOrder w on cd.WorkorderUkey = w.Ukey
left join Fabric f on f.SCIRefno = w.SCIRefno
where cd.id = '{0}'", CurrentDetailData["ID"]);
            DualResult dResult = DBProxy.Current.Select(null, cmdsql, out ExcelTb);
            
            if (dResult)
            {
                Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Cutting_P04.xltx"); //預先開啟excel app
                pathName = Sci.Env.Cfg.ReportTempDir + "Cutting_Daily_Plan" + DateTime.Now.ToFileTime() + ".xls";
                string tmpName = Sci.Env.Cfg.ReportTempDir + "tmp.xls";

                //createfolder();
                if (MyUtility.Excel.CopyToXls(ExcelTb, "", "Cutting_P04.xltx", 5, !autoSave, null, objApp, false))
                {// 將datatable copy to excel
                    Microsoft.Office.Interop.Excel._Worksheet objSheet = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
                    Microsoft.Office.Interop.Excel._Workbook objBook = objApp.ActiveWorkbook;

                    objSheet.Cells[1, 1] = keyWord;   // 條件字串寫入excel
                    objSheet.Cells[3, 2] = dateCuttingDate.Text;
                    objSheet.Cells[3, 5] = CurrentMaintain["POID"].ToString();
                    objSheet.Cells[3, 9] = CurrentMaintain["CutCellid"].ToString();
                    objSheet.Cells[3, 12] = Sci.Production.PublicPrg.Prgs.GetAddOrEditBy(loginID);

                    if (autoSave)
                    {
                        Random random = new Random();

                        pathName = Env.Cfg.ReportTempDir + "Cutting_Daily_Plan - " + Convert.ToDateTime(DateTime.Now).ToString("yyyyMMddHHmmss") + " - " + Convert.ToString(Convert.ToInt32(random.NextDouble() * 10000)) + ".xlsx";
                        objBook.SaveAs(pathName);
                        objBook.Close();
                        objApp.Workbooks.Close();
                        objApp.Quit();

                        Marshal.ReleaseComObject(objApp);
                        Marshal.ReleaseComObject(objSheet);
                        Marshal.ReleaseComObject(objBook);

                        if (objSheet != null) Marshal.FinalReleaseComObject(objSheet);
                        if (objBook != null) Marshal.FinalReleaseComObject(objBook);
                        if (objApp != null) Marshal.FinalReleaseComObject(objApp);
                        objApp = null;
                        fileNameExt = pathName.Substring(pathName.LastIndexOf("\\") + 1);
                    }
                    else
                    {
                        if (objSheet != null) Marshal.FinalReleaseComObject(objSheet);    //釋放sheet
                        if (objApp != null) Marshal.FinalReleaseComObject(objApp);          //釋放objApp
                    }
                }
            }
            else
            {
                ShowErr(cmdsql, dResult);
                return false;
            }
            return true;
        }

        //protected void createfolder()
        //{
        //    if (!Directory.Exists(Sci.Env.Cfg.ReportTempDir))
        //        Directory.CreateDirectory(Sci.Env.Cfg.ReportTempDir);
        //}

        private void btnSendMail_Click(object sender, EventArgs e)
        {
            //createfolder();
            if (!ToExcel(true))
            {
                return;
            }
            DataRow seekdr;
            if (MyUtility.Check.Seek("select * from mailto WITH (NOLOCK) where Id='004'", out seekdr))
            {
                string mailFrom = Sci.Env.Cfg.MailFrom;
                string mailto = seekdr["ToAddress"].ToString();
                string cc = seekdr["ccAddress"].ToString();
                string content = seekdr["content"].ToString();
                string subject = "<" + CurrentMaintain["mDivisionid"].ToString() + ">BulkMarkerRequest#:" + CurrentMaintain["ID"].ToString();

                var email = new MailTo(mailFrom, mailto, cc, subject + "-" + fileNameExt, pathName, content, false, false);
                DialogResult DR = email.ShowDialog(this);
                if (DR == DialogResult.OK)
                {
                    DateTime NOW = DateTime.Now;
                    string sql = string.Format("Update MarkerReq set sendDate = '{0}'  where id ='{1}'", NOW.ToString("yyyy/MM/dd HH:mm:ss"), CurrentMaintain["ID"]);
                    DualResult Result;
                    if (!(Result = DBProxy.Current.Execute(null, sql)))
                    {
                        ShowErr(sql, Result);
                    }
                    else
                    {
                        CurrentMaintain["sendDate"] = NOW;
                        this.OnDetailEntered();
                    }
                }

            }
            //刪除Excel File
            if (System.IO.File.Exists(pathName))
            {
                try
                {
                    System.IO.File.Delete(pathName);
                }
                catch (System.IO.IOException)
                {
                    MyUtility.Msg.WarningBox("Delete excel file fail!!");
                }
            }
        }

        protected override bool ClickPrint()
        {
            ToExcel(false);
            return base.ClickPrint();
        }

        private void btnFabricIssueList_Click(object sender, EventArgs e)
        {
            detailgrid.ValidateControl();
            var frm = new Sci.Production.Cutting.P04_FabricIssueList(CurrentMaintain["ID"].ToString().Trim());
            frm.ShowDialog(this);
        }
    }
}
