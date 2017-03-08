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
    public partial class P05 : Sci.Win.Tems.Input6
    {
        private string loginID = Sci.Env.User.UserID;
        private string keyWord = Sci.Env.User.Keyword;
        string fileNameExt, pathName;
        public P05(ToolStripMenuItem menuitem)
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
            Select a.*,
            al.LectraCode,
            (
                Select cuttingwidth from Order_EachCons b WITH (NOLOCK) , WorkOrder e WITH (NOLOCK) 
                where e.Order_EachconsUkey = b.Ukey and a.WorkOrderUkey = e.Ukey  
            ) as cuttingwidth,
            o.styleid,o.seasonid
            From MarkerReq_Detail a WITH (NOLOCK) left join Orders o WITH (NOLOCK) on a.orderid=o.id
            outer apply(
	            Select e.LectraCode 
	            from  WorkOrder e WITH (NOLOCK) 
                where  a.OrderID = e.id  and a.MarkerName = e.Markername and a.MarkerNo = e.MarkerNo
            )al
            where a.id = '{0}'
            ", masterID);
            this.DetailSelectCommand = cmdsql;
            return base.OnDetailSelectCommandPrepare(e);
        }
        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();
            Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("Styleid", header: "Style", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("OrderID", header: "SP#", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("Seasonid", header: "Season", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Text("SizeRatio", header: "Size Ratio", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("Markerno", header: "Flow No", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("MarkerName", header: "MarkerName", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Numeric("Layer", header: "Layers", width: Widths.AnsiChars(5), integer_places: 8, iseditingreadonly: true)
            .Text("LectraCode", header: "PatternPanel", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("fabriccombo", header: "FabricCombo", width: Widths.AnsiChars(2), iseditingreadonly: true)
            .Text("CuttingWidth", header: "Cutting Width", width: Widths.AnsiChars(8), iseditingreadonly: true)
             .Numeric("ReqQty", header: "# of Copies", width: Widths.AnsiChars(5), integer_places: 8)
             .Numeric("ReleaseQty", header: "# of Release", width: Widths.AnsiChars(5), integer_places: 8, iseditingreadonly: true)
             .Date("ReleaseDate", header: "Release Date", width: Widths.AnsiChars(10), iseditingreadonly: true);
            this.detailgrid.Columns[10].DefaultCellStyle.BackColor = Color.Pink;
        }
        protected override bool ClickDeleteBefore()
        {
            #region 判斷Confirmed
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
            #region 清空Cutplan 的request
            string clearCutplanidSql = string.Format("Update Cutplan set MarkerReqid ='' where MarkerReqid ='{0}'", CurrentMaintain["ID"]);
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
            DataRow[] dray = ((DataTable)detailgridbs.DataSource).Select("reqqty<=0");
            if (dray.Length != 0)
            {
                MyUtility.Msg.WarningBox("<# of Copies> can not be equal or less than 0");
                return;
            }
            #region update Master
            string updSql = string.Format("update MarkerReq set Status = 'Confirmed', editdate = getdate(), editname = '{0}' Where id='{1}'", loginID, CurrentMaintain["ID"]);
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
                        return;
                    }

                    _transactionscope.Complete();
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
            #endregion

        }
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.button2.Enabled = (CurrentMaintain["Status"].ToString() != "New");
            this.displayBox_Requestby.Text = PublicPrg.Prgs.GetAddOrEditBy(CurrentMaintain["AddName"]);
            this.label7.Text = CurrentMaintain["Status"].ToString();
            if (!MyUtility.Check.Empty(CurrentMaintain["sendDate"])) displayBox_LastSendDate.Text = Convert.ToDateTime(CurrentMaintain["sendDate"]).ToString("yyyy/MM/dd HH:mm:ss");
        }
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            #region 有Release 不可Unconfirm
            DataRow[] ary = ((DataTable)detailgridbs.DataSource).Select("releaseQty<>0");
            if (ary.Length != 0)
            {
                MyUtility.Msg.WarningBox("The record already released, you can not Unconfirm.");
                return;
            }
            #endregion
            string updSql = string.Format("update MarkerReq set Status = 'New', editdate = getdate(), editname = '{0}' Where id='{1}'", loginID, CurrentMaintain["ID"]);
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
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["Status"] = "New";
            CurrentMaintain["mDivisionid"] = keyWord;
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
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(CurrentMaintain["cutplanid"]))
            {
                MyUtility.Msg.WarningBox("<Cutplan ID> can not be empty!");
                return false;
            }
            string keyword = keyWord + "MK";
            string reqid = MyUtility.GetValue.GetID(keyword, "MarkerReq");
            if (string.IsNullOrWhiteSpace(reqid))
            {
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["ID"])) CurrentMaintain["ID"] = reqid;  //若單號為空，才要賦予新單號
            return base.ClickSaveBefore();
        }
        protected override DualResult ClickSavePost()
        {
            string updateCutplan = string.Format("Update Cutplan set MarkerReqid = '{0}' where id ='{1}'", CurrentMaintain["ID"], CurrentMaintain["Cutplanid"]);
            DualResult upResult;
            if (!(upResult = DBProxy.Current.Execute(null, updateCutplan)))
            {
                return upResult;
            }

            return base.ClickSavePost();
        }
        protected override void OnDetailUIConvertToUpdate()
        {
            base.OnDetailUIConvertToMaintain();
            textBox1.ReadOnly = true;
        }
        private void textBox1_Validating(object sender, CancelEventArgs e)
        {

            if (!this.EditMode) return;
            if (textBox1.OldValue.ToString() == textBox1.Text) return;
            string cmd = string.Format("Select * from Cutplan WITH (NOLOCK) Where id='{0}' and mDivisionid = '{1}'", textBox1.Text, keyWord);
            DataRow cutdr;
            if (!MyUtility.Check.Seek(cmd, out cutdr, null))
            {
                MyUtility.Msg.WarningBox("<Cutplan ID> data not found!");
                textBox1.Text = "";
                return;
            }
            if (cutdr["markerreqid"].ToString() != "")
            {
                MyUtility.Msg.WarningBox(string.Format("<Cutplan ID> already created Bulk Marker Request<{0}>", cutdr["markerreqid"]));
                textBox1.Text = "";
                return;
            }
            if (cutdr["Status"].ToString() != "Confirmed")
            {
                MyUtility.Msg.WarningBox("The Cutplan not yet confirm.");
                textBox1.Text = "";
                return;
            }
            foreach (DataRow dr in this.DetailDatas)
            {
                dr.Delete();
            }

        }

        private void textBox1_Validated(object sender, EventArgs e)
        {
            base.OnValidated(e);
            string cmd = string.Format("Select * from Cutplan WITH (NOLOCK) Where id='{0}' and mDivisionid = '{1}'", textBox1.Text, keyWord);
            DataRow cutdr;
            if (MyUtility.Check.Seek(cmd, out cutdr, null))
            {
                displayBox_Cell.Text = cutdr["Cutcellid"].ToString();

                if (MyUtility.Check.Empty(cutdr["estcutdate"]))
                    dateBox1.Text = "";
                else
                    dateBox1.Text = Convert.ToDateTime(cutdr["estcutdate"]).ToShortDateString();

                string marker2sql = string.Format(
                @"Select b.Orderid,b.MarkerName,sum(b.Layer) as layer,
                b.MarkerNo,a.WorkOrderUkey,b.fabricCombo,
                (
                    Select c.sizecode+'*'+convert(varchar(8),c.qty)+'/' 
                    From WorkOrder_SizeRatio c WITH (NOLOCK) 
                    Where a.WorkOrderUkey =c.WorkOrderUkey            
                    For XML path('')
                ) as SizeRatio,
				o.styleid,o.seasonid
                From Cutplan_Detail a WITH (NOLOCK) , WorkOrder b WITH (NOLOCK) 
                left join Orders o WITH (NOLOCK) on b.orderid=o.id
                Where a.workorderukey = b.ukey and a.id = '{0}'
			    Group by b.Orderid,b.MarkerName,b.MarkerNo,
                    b.fabricCombo,a.WorkOrderUkey,o.styleid,o.seasonid", textBox1.Text);
                DataTable markerTb;
                DataTable gridTb = ((DataTable)this.detailgridbs.DataSource);
                DualResult dResult = DBProxy.Current.Select(null, marker2sql, out markerTb);
                foreach (DataRow dr in markerTb.Rows)
                {
                    DataRow ndr = gridTb.NewRow();
                    ndr["styleid"] = dr["styleid"];
                    ndr["seasonid"] = dr["seasonid"];
                    ndr["OrderID"] = dr["OrderID"];
                    ndr["SizeRatio"] = dr["SizeRatio"];
                    ndr["MarkerName"] = dr["MarkerName"];
                    ndr["Layer"] = dr["Layer"];
                    ndr["FabricCombo"] = dr["FabricCombo"];
                    ndr["MarkerNo"] = dr["MarkerNo"];
                    ndr["WorkOrderUkey"] = dr["WorkOrderUkey"];
                    gridTb.Rows.Add(ndr);
                }
            }
        }

        private bool ToExcel(bool autoSave)
        {
            DataTable ExcelTb;
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
            ", CurrentDetailData["ID"]);
            DualResult dResult = DBProxy.Current.Select(null, cmdsql, out ExcelTb);
            if (dResult)
            {
                string str = Sci.Env.Cfg.XltPathDir;
                Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Cutting_P05.xltx"); //預先開啟excel app
                pathName = Sci.Env.Cfg.ReportTempDir + "Bulk_Marker_Request" + DateTime.Now.ToFileTime() + ".xls";
                string tmpName = Sci.Env.Cfg.ReportTempDir + "tmp.xls";
                //Microsoft.Office.Interop.Excel._Workbook objBook = null;


                if (MyUtility.Excel.CopyToXls(ExcelTb,"", "Cutting_P05.xltx", 5, !autoSave, null, objApp, false))
                {// 將datatable copy to excel

                    Microsoft.Office.Interop.Excel._Worksheet objSheet = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
                    Microsoft.Office.Interop.Excel._Workbook objBook = objApp.ActiveWorkbook;


                    objSheet.Cells[1, 1] = keyWord;   // 條件字串寫入excel
                    objSheet.Cells[3, 2] = CurrentMaintain["id"].ToString();
                    objSheet.Cells[3, 4] = Convert.ToDateTime(CurrentMaintain["EstCutDate"]).ToShortDateString();
                    objSheet.Cells[3, 6] = CurrentMaintain["CutCellid"].ToString();
                    objSheet.Cells[3, 8] = Sci.Production.PublicPrg.Prgs.GetAddOrEditBy(CurrentMaintain["AddName"]);

                    if (autoSave)
                    {
                        Random random = new Random();
                        pathName = Env.Cfg.ReportTempDir + "Bulk_Marker_Request - " + Convert.ToDateTime(DateTime.Now).ToString("yyyyMMddHHmmss") + " - " + Convert.ToString(Convert.ToInt32(random.NextDouble() * 10000)) + ".xlsx";
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
                        //System.IO.File.Delete(tmpName);
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

        private void button2_Click(object sender, EventArgs e)
        {
            //createfolder();
            if (!ToExcel(true))
            {
                return;
            }
            DataRow seekdr;
            if (MyUtility.Check.Seek("select * from mailto WITH (NOLOCK) where Id='004'", out seekdr))
            {
                string mailFrom = Sci.Env.User.MailAddress;
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
    }
}
