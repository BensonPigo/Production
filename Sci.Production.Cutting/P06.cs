using Ict;
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

namespace Sci.Production.Cutting
{
    public partial class P06 : Sci.Win.Tems.Input6
    {
        private string loginID = Sci.Env.User.UserID;
        private string keyWord = Sci.Env.User.Keyword; 
        public P06(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.DefaultFilter = string.Format("MDivisionID = '{0}' and Status !='New'", keyWord);
        }
        protected override DualResult OnDetailSelectCommandPrepare(Win.Tems.InputMasterDetail.PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["id"].ToString();
            string cmdsql = string.Format(
            @"
            Select a.*,
            (
                Select PatternPanel+'+ ' 
                From WorkOrder_PatternPanel c
                Where c.WorkOrderUkey =a.WorkOrderUkey 
                For XML path('')
            ) as PatternPanel,
            (
                Select cuttingwidth from Order_EachCons b, WorkOrder e
                where e.Order_EachconsUkey = b.Ukey and a.WorkOrderUkey = e.Ukey  
            ) as cuttingwidth,
            o.styleid,o.seasonid
            From MarkerReq_Detail a left join Orders o on a.orderid=o.id
            where a.id = '{0}'
            ", masterID);
            this.DetailSelectCommand = cmdsql;            
            return base.OnDetailSelectCommandPrepare(e);
        }
        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();
            Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("Styleid", header: "Style", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("OrderID", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("Seasonid", header: "Season", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("SizeRatio", header: "Size Ratio", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("Markerno", header: "Flow No", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("MarkerName", header: "MarkerName", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Numeric("Layer", header: "Layers", width: Widths.AnsiChars(5), integer_places: 8, iseditingreadonly: true)
            .Text("PatternPanel", header: "PatternPanel", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("FabricCombo", header: "FabricCombo", width: Widths.AnsiChars(2), iseditingreadonly: true)
            .Text("CuttingWidth", header: "Cutting Width", width: Widths.AnsiChars(8), iseditingreadonly: true)
             .Numeric("ReqQty", header: "# of Copies", width: Widths.AnsiChars(5), integer_places: 8, iseditingreadonly: true)
             .Numeric("ReleaseQty", header: "# of Release", width: Widths.AnsiChars(5), integer_places: 8)
             .Date("ReleaseDate", header: "Release Date", width: Widths.AnsiChars(10));
            this.detailgrid.Columns[10].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns[11].DefaultCellStyle.BackColor = Color.Pink;
        }
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.label7.Text = CurrentMaintain["Status"].ToString();
            this.displayBox_Requestby.Text = PublicPrg.Prgs.GetAddOrEditBy(CurrentMaintain["AddName"]);
        }
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.GetValue.Lookup("Status",CurrentMaintain["ID"].ToString(),"MarkerReq","ID")=="New")
            {
                MyUtility.Msg.WarningBox("The Record not yet confirm");
                return false;
            }
            return base.ClickSaveBefore();
        }
        protected override DualResult ClickSavePost()
        {
            DataRow[] dray = ((DataTable)detailgridbs.DataSource).Select("releaseqty <>0");
            if (dray.Length != 0)
            {
                string updateSql = string.Format("Update MarkerReq set Status = 'Sent' where id ='{0}'", CurrentMaintain["ID"]);
                DualResult upResult;
                if (!(upResult = DBProxy.Current.Execute(null, updateSql)))
                {
                    return upResult;
                }
            }
            else
            {
                string updateSql = string.Format("Update MarkerReq set Status = 'Confirmed' where id ='{0}'", CurrentMaintain["ID"]);
                DualResult upResult;
                if (!(upResult = DBProxy.Current.Execute(null, updateSql)))
                {
                    return upResult;
                }
            }
           
            return base.ClickSavePost();
        }
        protected override void ClickSaveAfter()
        {
            base.ClickSaveAfter();
            OnDetailEntered();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            DataTable ExcelTb;
            string cmdsql = string.Format(
            @"
            Select  o.styleid,a.orderid,o.seasonid,a.sizeRatio,a.markerno,a.markername,
                    a.layer,a.fabriccombo,
            (
                Select PatternPanel+'+ ' 
                From WorkOrder_PatternPanel c
                Where c.WorkOrderUkey =a.WorkOrderUkey 
                For XML path('')
            ) as PatternPanel,
            (
                Select cuttingwidth from Order_EachCons b, WorkOrder e
                where e.Order_EachconsUkey = b.Ukey and a.WorkOrderUkey = e.Ukey  
            ) as cuttingwidth,ReqQty,ReleaseQty,ReleaseDate
            From MarkerReq_Detail a left join Orders o on a.orderid=o.id
            where a.id = '{0}'
            ", CurrentDetailData["ID"]);
            DualResult dResult = DBProxy.Current.Select(null, cmdsql, out ExcelTb);
            if (dResult)
            {
                string str = Sci.Env.Cfg.XltPathDir;
                Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Cutting_P05.xltx"); //預先開啟excel app
                string pathName = Sci.Env.Cfg.ReportTempDir + "Bulk_Marker_Request" + DateTime.Now.ToFileTime() + ".xls";
                string tmpName = Sci.Env.Cfg.ReportTempDir + "tmp.xls";
                //Microsoft.Office.Interop.Excel._Workbook objBook = null;


                if (MyUtility.Excel.CopyToXls(ExcelTb, pathName, "Cutting_P05.xltx", 5, false, null, objApp))
                {// 將datatable copy to excel
                    //Microsoft.Office.Interop.Excel.Application oleApp = MyUtility.Excel.ConnectExcel(tmpName);
                    Microsoft.Office.Interop.Excel._Worksheet objSheet = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
                    Microsoft.Office.Interop.Excel._Workbook objBook = objApp.ActiveWorkbook;
                    //oleApp.Visible = false;
                    //oleApp.DisplayAlerts = false;
                    //objBook = oleApp.ActiveWorkbook;
                    //objSheet = objBook.Worksheets["Sheet1"];

                    objSheet.Cells[1, 1] = keyWord;   // 條件字串寫入excel
                    objSheet.Cells[3, 2] = CurrentMaintain["id"].ToString();
                    objSheet.Cells[3, 4] = CurrentMaintain["EstCutDate"].ToString();
                    objSheet.Cells[3, 6] = CurrentMaintain["CutCellid"].ToString();
                    objSheet.Cells[3, 8] = Sci.Production.PublicPrg.Prgs.GetAddOrEditBy(loginID);
                    objBook.Save();
                    //oleApp.Workbooks[1].SaveAs(pathName);
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
                    string fileNameExt = pathName.Substring(pathName.LastIndexOf("\\") + 1);

                    DataRow seekdr;
                    if (MyUtility.Check.Seek("select * from mailto where Id='003'", out seekdr))
                    {
                        string mailto = seekdr["ToAddress"].ToString();
                        string cc = seekdr["ccAddress"].ToString();
                        string content = seekdr["content"].ToString();
                        string subject = "<" + CurrentMaintain["mDivisionid"].ToString() + ">BulkMarkerRequest#:" + CurrentMaintain["ID"].ToString();

                        var email = new MailTo(Env.Cfg.MailFrom, mailto, cc, subject + "-" + fileNameExt, pathName,
content, false, false);
                        email.ShowDialog(this);

                    }
                }
            }
            else
            {
                ShowErr(cmdsql, dResult);
                return;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            grid.ValidateControl();
            string reDate = dateBox2.Text;
            foreach (DataRow dr in DetailDatas)
            {
                dr["releaseDate"] = reDate;
            }
        }

    }
}
