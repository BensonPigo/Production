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
using System.IO;
using System.Linq;

namespace Sci.Production.Cutting
{
    public partial class P08 : Sci.Win.Tems.Input6
    {
        private string loginID = Sci.Env.User.UserID;
        private string keyWord = Sci.Env.User.Keyword;
        string fileNameExt, pathName;
        public P08(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.DefaultFilter = string.Format("MDivisionID = '{0}'", keyWord);
        }
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.gridicon.Append.Visible = false;
            this.gridicon.Insert.Visible = false;
            DataTable queryDT;
            string querySql = string.Format(@"
select '' FTYGroup

union 
select distinct FTYGroup 
from Factory 
where MDivisionID = '{0}'", Sci.Env.User.Keyword);
            DBProxy.Current.Select(null, querySql, out queryDT);
            MyUtility.Tool.SetupCombox(queryfors, 1, queryDT);
            queryfors.SelectedIndex = 0;
            queryfors.SelectedIndexChanged += (s, e) =>
            {
                switch (queryfors.SelectedIndex)
                {
                    case 0:
                        this.DefaultWhere = "";
                        break;
                    default:
                        this.DefaultWhere = string.Format("(select FactoryID from Cutting where CutTapePlan.CuttingID = Cutting.ID) = '{0}'", queryfors.SelectedValue);
                        break;
                }
                this.ReloadDatas();
            };
        }
        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();
            Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("FabricCombo", header: "Fab Combo", width: Widths.Auto(), iseditingreadonly: true)
            .Text("MarkerName", header: "Mark Name", width: Widths.Auto(), iseditingreadonly: true)
            .Text("SEQ1", header: "SEQ1", width: Widths.Auto(), iseditingreadonly: true)
            .Text("SEQ2", header: "SEQ2", width: Widths.Auto(), iseditingreadonly: true)
            .Text("Article", header: "Article", width: Widths.Auto(), iseditingreadonly: true)
            .Text("Colorid", header: "Color", width: Widths.Auto(), iseditingreadonly: true)
            .Text("direction", header: "Type of Cutting", width: Widths.Auto(), iseditingreadonly: true)
            .Text("CuttingWidth", header: "Cut Width", width: Widths.Auto(), iseditingreadonly: true)
            .Numeric("Qty", header: "Qty", width: Widths.Auto(), integer_places: 5, decimal_places: 2, iseditingreadonly: true)
            .Numeric("IssueQty", header: "Release Qty", width: Widths.Auto(), integer_places: 5, decimal_places: 2)
            .Text("Dyelot", header: "Dyelot", width: Widths.Auto())
            .Text("Refno", header: "Refno", width: Widths.Auto(), iseditingreadonly: true)
            .Numeric("ConsPC", header: "Cons", width: Widths.Auto(), integer_places: 8, decimal_places: 4, iseditingreadonly: true)
            .Text("Remark", header: "Remark", width: Widths.Auto())
            ;
            this.detailgrid.Columns["IssueQty"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["Dyelot"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["Remark"].DefaultCellStyle.BackColor = Color.Pink;
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
        protected override bool ClickNew()
        {
            detailgrid.ValidateControl();
            var frm = new Sci.Production.Cutting.P08_Import();
            DialogResult dr = frm.ShowDialog(this);
            //dr == System.Windows.Forms.DialogResult.
            this.ReloadDatas();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                var topID = frm.importedIDs[0];
                int newDataIdx = gridbs.Find("ID", topID);
                gridbs.Position = newDataIdx;
            }
            return true;
        }
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            #region 存在 Issue 不可 Uncomfirm
            DataTable dt;
            string Query = string.Format("Select 1 from Issue WITH (NOLOCK) Where Cutplanid ='{0}'", CurrentMaintain["ID"]);
            DualResult result = DBProxy.Current.Select(null, Query, out dt);
            if (!result)
            {
                ShowErr(result);
                return;

            }

            if (dt.Rows.Count != 0)
            {
                MyUtility.Msg.WarningBox("The record already issued fabric, you can not Unconfirm.");
                return;
            }
            #endregion

            string updSql = $@"update CutTapePlan_Detail set status = 'New',[EditDate]=getdate(),[EditName]='{Sci.Env.User.UserID}' where id = '{this.CurrentMaintain["ID"]}'";

            if (!(result = DBProxy.Current.Execute(null, updSql)))
            {
                ShowErr(result);
                return;
            }
        }
        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            #region 檢查不能空值  表頭: EstCutDate / 表身:Dyelot, IssueQty[Release Qty]
            if (MyUtility.Check.Empty(this.CurrentMaintain["EstCutDate"]))
            {
                MyUtility.Msg.WarningBox("Est. Cutting Date can't empty!");
                return;
            }

            if (this.DetailDatas.AsEnumerable().Where(w=> MyUtility.Check.Empty(w["Dyelot"]) || MyUtility.Check.Empty(w["IssueQty"])).Any())
            {
                MyUtility.Msg.WarningBox("Dyelot or Release Qty can't empty!");
                return;
            }
            #endregion

            string updSql = $@"update CutTapePlan_Detail set status = 'Confirmed',[EditDate]=getdate(),[EditName]='{Sci.Env.User.UserID}' where id = '{this.CurrentMaintain["ID"]}'";

            DualResult result = DBProxy.Current.Execute(null, updSql);
            if (!result)
            {
                ShowErr(result);
                return;
            }
        }
        protected override bool ClickSaveBefore()
        {
            #region 檢查表頭 Est. Cut Date 只能輸入今天和未來的日期 , 一定要填
            // Est. Cut Date 一定要填
            if (MyUtility.Check.Empty(this.CurrentMaintain["EstCutDate"]))
            {
                MyUtility.Msg.WarningBox("Est. Cutting Date can't empty!");
                return false;
            }

            // 只能輸入今天和未來的日期
            if (((DateTime)this.CurrentMaintain["EstCutDate"]).Date < DateTime.Today)
            {
                MyUtility.Msg.WarningBox("Est. Cutting Date can't earlier than today!");
                return false;
            }
            #endregion

            #region 檢查表身 Grid 要有資料, Release Qty 要大於 0
            // Grid 要有資料
            if (this.DetailDatas.Count == 0)
            {
                MyUtility.Msg.WarningBox("Detail can't empty!");
                return false;
            }

            // Release Qty 要大於 0
            if (this.DetailDatas.AsEnumerable().Where(w => MyUtility.Convert.GetDecimal(w["IssueQty"]) <= 0).Any())
            {
                MyUtility.Msg.WarningBox("Dyelot or Release Qty can't empty!");
                return false;
            }
            #endregion

            #region 檢查 Release Qty, 依據 FabCombo, MarkerName, ColorID 做群組加總, Sum of Release Qty 不能大於該群組的 Qty
            var x = this.DetailDatas.AsEnumerable()
                .GroupBy(g => new
                {
                    FabricCombo = g["FabricCombo"].ToString(),
                    MarkerName = g["MarkerName"].ToString(),
                    ColorID = g["ColorID"].ToString(),
                    Qty = MyUtility.Convert.GetDecimal(g["Qty"])
                })
                .Select(s => new
                {
                    s.Key.FabricCombo,
                    s.Key.MarkerName,
                    s.Key.ColorID,
                    s.Key.Qty,
                    IssueQty = s.Sum(i => MyUtility.Convert.GetDecimal(i["IssueQty"]))
                });
            if (x.Where(w => w.IssueQty > w.Qty).Any())
            {
                MyUtility.Msg.WarningBox("According to <Fab Combo>, <Mark Name> and <Color> the total <Release Qty> cannot be greater than <Qty>");
                return false;
            }

            #endregion

            return base.ClickSaveBefore();
        }
        protected override bool ClickDeleteBefore()
        {
            #region 判斷 Confirmed 不可刪除
            if (CurrentMaintain["Status"].ToString() == "Confirmed")
            {
                MyUtility.Msg.WarningBox("The record status is confimed, you can not delete.");
                return false;
            }
            #endregion

            return base.ClickDeleteBefore();
        }
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.btnSendMail.Enabled = CurrentMaintain["Status"].ToString() != "New";
            this.detailgrid.AutoResizeColumns();
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
                Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Cutting_P08.xltx"); //預先開啟excel app
              

                //createfolder();
                //if (MyUtility.Excel.CopyToXls(ExcelTb, "", "Cutting_P08.xltx", 5, !autoSave, null, objApp, false))
                if (MyUtility.Excel.CopyToXls(ExcelTb, "", "Cutting_P08.xltx", 5, showExcel: false, excelApp: objApp))
                {// 將datatable copy to excel
                    Microsoft.Office.Interop.Excel._Worksheet objSheet = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
                    Microsoft.Office.Interop.Excel._Workbook objBook = objApp.ActiveWorkbook;

                    objSheet.Cells[1, 1] = keyWord;   // 條件字串寫入excel
                    objSheet.Cells[3, 2] = dateCuttingDate.Text;
                    objSheet.Cells[3, 5] = CurrentMaintain["POID"].ToString();
                    objSheet.Cells[3, 10] = CurrentMaintain["SpreadingNoID"].ToString();
                    objSheet.Cells[3, 12] = CurrentMaintain["CutCellid"].ToString();
                    objSheet.Cells[3, 15] = Sci.Production.PublicPrg.Prgs.GetAddOrEditBy(loginID);
                    pathName = Sci.Production.Class.MicrosoftFile.GetName("Cutting_Daily_Plan");
                    objBook.SaveAs(pathName);
                    if (autoSave)
                    {
                      
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
                        objBook.Close();
                        objApp.Workbooks.Close();
                        objApp.Quit();

                        Marshal.ReleaseComObject(objApp);
                        Marshal.ReleaseComObject(objSheet);
                        Marshal.ReleaseComObject(objBook);

                        if (objSheet != null) Marshal.FinalReleaseComObject(objSheet);    //釋放sheet
                        if (objBook != null) Marshal.FinalReleaseComObject(objBook);
                        if (objApp != null) Marshal.FinalReleaseComObject(objApp);          //釋放objApp

                        pathName.OpenFile();
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
        private void btnSendMail_Click(object sender, EventArgs e)
        {
          
            //createfolder();
            if (!ToExcel(true))
            {
                return;
            }
            DataRow seekdr;
            if (MyUtility.Check.Seek("select * from mailto WITH (NOLOCK) where Id='005'", out seekdr))
            {
                string mailFrom = Sci.Env.Cfg.MailFrom;
                string mailto = seekdr["ToAddress"].ToString();
                string cc = seekdr["ccAddress"].ToString();
                string content = seekdr["content"].ToString();
                string subject = "<" + CurrentMaintain["mDivisionid"].ToString() + ">BulkMarkerRequest#:" + CurrentMaintain["ID"].ToString(); 
                var email = new MailTo(mailFrom, mailto, cc, subject + "-" + fileNameExt, pathName, content, false, true);
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
