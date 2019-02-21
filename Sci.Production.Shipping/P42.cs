﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// menuitem
    /// </summary>
    public partial class P42 : Sci.Win.Tems.Input6
    {
        private Ict.Win.DataGridViewGeneratorTextColumnSettings nlcode = new Ict.Win.DataGridViewGeneratorTextColumnSettings();

        /// <summary>
        /// P42
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P42(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            if (this.EditMode)
            {
                if (MyUtility.Convert.GetString(this.CurrentMaintain["Status"]).ToUpper() == "CONFIRMED")
                {
                    this.detailgrid.IsEditingReadOnly = true;
                }
                else
                {
                    this.detailgrid.IsEditingReadOnly = false;
                }

                this.detailgrid.EnsureStyle();
            }
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "1=0" : "v.ID = " + MyUtility.Convert.GetString(e.Master["ID"]);
            this.DetailSelectCommand = string.Format(
                @"
select v.id,vdd.Refno,vdd.FabricType,vd.NLCode,vd.Qty,c.HSCode,c.UnitID
from VNContractQtyAdjust v WITH (NOLOCK) 
inner join VNContractQtyAdjust_Detail vd WITH (NOLOCK) on v.ID = vd.ID
left join VNContractQtyAdjust_Detail_Detail vdd WITH (NOLOCK) on v.ID = vdd.ID and vd.NLCode = vdd.NLCode
left join VNContract_Detail c WITH (NOLOCK) on c.ID = v.VNContractID and c.NLCode = vd.NLCode
where {0}
order by CONVERT(int,SUBSTRING(vd.NLCode,3,3))", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            #region NL Code的Validating
            this.nlcode.CellValidating += (s, e) =>
                {
                    if (this.EditMode)
                    {
                        DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                        if (!MyUtility.Check.Empty(e.FormattedValue))
                        {
                            if (MyUtility.Convert.GetString(dr["nlcode"]) != MyUtility.Convert.GetString(e.FormattedValue))
                            {
                                DataRow seekData;
                                if (!MyUtility.Check.Seek(
                                    string.Format(
                                    "select HSCode,UnitID from VNContract_Detail WITH (NOLOCK) where ID = '{0}' and NLCode = '{1}'",
                                    MyUtility.Convert.GetString(this.CurrentMaintain["VNContractID"]),
                                    MyUtility.Convert.GetString(e.FormattedValue)),
                                    out seekData))
                                {
                                    dr["HSCode"] = string.Empty;
                                    dr["NLCode"] = string.Empty;
                                    dr["Qty"] = 0;
                                    dr["UnitID"] = string.Empty;
                                    e.Cancel = true;
                                    MyUtility.Msg.WarningBox("Customs Code not found!!");
                                    return;
                                }
                                else
                                {
                                    dr["HSCode"] = seekData["HSCode"];
                                    dr["NLCode"] = e.FormattedValue;
                                    dr["UnitID"] = seekData["UnitID"];
                                }
                            }
                        }
                        else
                        {
                            dr["HSCode"] = string.Empty;
                            dr["NLCode"] = string.Empty;
                            dr["Qty"] = 0;
                            dr["UnitID"] = string.Empty;
                        }
                    }
                };
            #endregion
            DataGridViewGeneratorNumericColumnSettings stockQtySetting = new DataGridViewGeneratorNumericColumnSettings();
            stockQtySetting.IsSupportNegative = true;

            base.OnDetailGridSetup();

            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("HSCode", header: "HS Code", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Refno", header: "Ref No.", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("NLCode", header: "Customs Code", width: Widths.AnsiChars(7), settings: this.nlcode)
                .Numeric("Qty", header: "Stock Qty", decimal_places: 3, width: Widths.AnsiChars(15), settings: stockQtySetting)
                .Text("UnitID", header: "Unit", width: Widths.AnsiChars(8), iseditingreadonly: true);
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["Status"] = "New";
            this.CurrentMaintain["CDate"] = DateTime.Today;
            this.CurrentMaintain["VNContractID"] = MyUtility.GetValue.Lookup("select top 1 ID from VNContract WITH (NOLOCK) where StartDate <= GETDATE() and EndDate >= GETDATE() and Status = 'Confirmed'");
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();

            if (MyUtility.Convert.GetString(this.CurrentMaintain["Status"]).ToUpper() == "CONFIRMED")
            {
                this.dateDate.ReadOnly = true;
                this.txtContractNo.ReadOnly = true;
                this.txtRemark.ReadOnly = true;
                this.btnImportfromExcel.Enabled = false;
                this.gridicon.Append.Enabled = false;
                this.gridicon.Insert.Enabled = false;
                this.gridicon.Remove.Enabled = false;
            }
        }

        /// <inheritdoc/>
        protected override bool ClickDeleteBefore()
        {
            if (MyUtility.Convert.GetString(this.CurrentMaintain["Status"]).ToUpper() == "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("This record already confirmed, can't delete!!");
                return false;
            }

            return base.ClickDeleteBefore();
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            #region 檢查必輸欄位
            if (MyUtility.Check.Empty(this.CurrentMaintain["CDate"]))
            {
                MyUtility.Msg.WarningBox("Date can't empty!!");
                this.dateDate.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["VNContractID"]))
            {
                this.txtContractNo.Focus();
                MyUtility.Msg.WarningBox("Contract no. can't empty!!");
                return false;
            }
            #endregion

            #region 刪除表身Qty為0的資料
            int recCount = 0;
            foreach (DataRow dr in this.DetailDatas)
            {
                if (MyUtility.Check.Empty(dr["Qty"]))
                {
                    dr.Delete();
                    continue;
                }

                recCount++;
            }

            if (recCount == 0)
            {
                MyUtility.Msg.WarningBox("Detail can't empty!!");
                return false;
            }
            #endregion

            return base.ClickSaveBefore();
        }

        protected override DualResult ClickSavePost()
        {
            string insertuUdataDetail = $@"
select ID,NLCode,Qty = sum(Qty)
into #tmps
from #tmp t
group by ID,NLCode

merge VNContractQtyAdjust_Detail t
using #tmps s
on t.ID = s.ID and t.NLCode = s.NLCode
when matched then update set
	t.Qty = s.Qty
when not matched by target then
	insert(ID,NLCode,Qty)
	values(s.ID,s.NLCode,s.Qty)
when not matched by source and t.id in(select id from #tmps) then
	delete
;
drop table #tmp,#tmps
";
            DataTable dt;
            DualResult result = MyUtility.Tool.ProcessWithDatatable((DataTable)this.detailgridbs.DataSource, string.Empty, insertuUdataDetail, out dt);
            if (!result)
            {
                return Result.F(result.ToString());
            }

            return base.ClickSavePost();
        }

        /// <inheritdoc/>
        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            string updateCmds = string.Format(
                "update VNContractQtyAdjust set EditDate = GETDATE(), EditName = '{0}', Status = 'Confirmed' where ID = {1}",
                Sci.Env.User.UserID,
                MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));

            DualResult result = DBProxy.Current.Execute(null, updateCmds);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Confirm fail!!\r\n" + result.ToString());
                return;
            }
        }

        /// <inheritdoc/>
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();

            string updateCmds = string.Format(
                "update VNContractQtyAdjust set EditDate = GETDATE(), EditName = '{0}', Status = 'New' where ID = {1}",
                Sci.Env.User.UserID,
                MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));

            DualResult result = DBProxy.Current.Execute(null, updateCmds);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Unconfirm fail!!\r\n" + result.ToString());
                return;
            }
        }

        protected override bool ClickPrint()
        {

            //if (MyUtility.Check.Empty(this.CurrentDetailData))
            //{
            //    MyUtility.Msg.InfoBox("No any data.");
            //    return false;
            //}
            #region
            DataTable dt;
            string sqlcmd = $@"
select
	[seq]=ROW_NUMBER()over(order by vaqd.NLCode,vd.HSCode),
	vaqd.NLCode,
	vcd.DescVI,
	vd.HSCode,
	ORIGIN=null,
	vaqd.Qty,
	vd.UnitID,
	PRICE=null,
	AMOUNT=null,
	TAXFREECODE=null
from VNContractQtyAdjust vaq
inner join VNContractQtyAdjust_Detail vaqd with(nolock)on vaq.id = vaqd.id
left join VNContract_Detail vd with(nolock)on vd.ID = vaq.VNContractID and vd.NLCode=vaqd.NLCode
left join VNNLCodeDesc vcd with(nolock)on vcd.NLCode=vd.NLCode
where vaq.id = '{this.CurrentMaintain["ID"]}'
order by vaqd.NLCode,vd.HSCode
";
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out dt);

            if (!result)
            {
                this.ShowErr(result);
                return false;
            }
            #endregion
            string excelFileName = "Shipping_P42";
            Microsoft.Office.Interop.Excel.Application excelApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + $"\\{excelFileName}.xltx"); // 預先開啟excel app
            MyUtility.Excel.CopyToXls(dt, string.Empty, $"{excelFileName}.xltx", 1, false, null, excelApp);
            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName(excelFileName);
            Microsoft.Office.Interop.Excel.Workbook workbook = excelApp.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            excelApp.Quit();
            Marshal.ReleaseComObject(excelApp);
            Marshal.ReleaseComObject(workbook);
            strExcelName.OpenFile();
            #endregion 
            return base.ClickPrint();
        }

        // Import from excel
        private void BtnImportfromExcel_Click(object sender, EventArgs e)
        {
            string excelFile = MyUtility.File.GetFile("Excel files (*.xlsx)|*.xlsx");
            if (MyUtility.Check.Empty(excelFile))
            {
                return;
            }

            // 刪除表身Grid資料
            foreach (DataRow dr in this.DetailDatas)
            {
                dr.Delete();
            }

            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(excelFile);
            if (excel == null)
            {
                return;
            }

            DataRow seekData;
            StringBuilder errNLCode = new StringBuilder();

            this.ShowWaitMessage("Starting EXCEL...");
            excel.Visible = false;
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];
            int intRowsCount = worksheet.UsedRange.Rows.Count;
            int intColumnsCount = worksheet.UsedRange.Columns.Count;
            int intRowsStart = 2;
            int intRowsRead = intRowsStart - 1;

            Microsoft.Office.Interop.Excel.Range range;
            object[,] objCellArray;

            while (intRowsRead < intRowsCount)
            {
                intRowsRead++;

                range = worksheet.Range[string.Format("A{0}:E{0}", intRowsRead)];
                objCellArray = range.Value;

                if (!MyUtility.Check.Seek(
                    string.Format(
                        "select HSCode,UnitID from VNContract_Detail WITH (NOLOCK) where ID = '{0}' and Refno = '{1}'",
                        MyUtility.Convert.GetString(this.CurrentMaintain["VNContractID"]),
                        MyUtility.Convert.GetString(MyUtility.Excel.GetExcelCellValue(objCellArray[1, 1], "C"))),
                    out seekData))
                {
                    errNLCode.Append(string.Format("Customs Code: {0}\r\n", MyUtility.Convert.GetString(MyUtility.Excel.GetExcelCellValue(objCellArray[1, 1], "C"))));
                    continue;
                }
                else
                {
                    DataRow newRow = ((DataTable)this.detailgridbs.DataSource).NewRow();
                    newRow["Refno"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 1], "C");
                    string type = MyUtility.Convert.GetString(MyUtility.Excel.GetExcelCellValue(objCellArray[1, 2], "C"));
                    string nlCode = string.Empty;
                    if (type.EqualString("A") || type.EqualString("F"))
                    {
                        nlCode = MyUtility.GetValue.Lookup($"select distinct NLCode from Fabric with(nolock) where refno = '{newRow["Refno"]}'");
                    }
                    else if (type.EqualString("L"))
                    {
                        nlCode = MyUtility.GetValue.Lookup($"select NLCode from LocalItem with(nolock) where refno = '{newRow["Refno"]}'");
                    }

                    newRow["NLCode"] = nlCode;
                    newRow["Qty"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 3], "N");
                    newRow["HSCode"] = seekData["HSCode"];
                    newRow["UnitID"] = seekData["UnitID"];
                    ((DataTable)this.detailgridbs.DataSource).Rows.Add(newRow);
                }
            }

            excel.Workbooks.Close();
            excel.Quit();
            excel = null;
            this.HideWaitMessage();
            if (!MyUtility.Check.Empty(errNLCode.ToString()))
            {
                MyUtility.Msg.WarningBox(string.Format("Below Customs Code is not in B43. Customs Contract - Contract No.: {0}\r\n{1}", MyUtility.Convert.GetString(this.CurrentMaintain["VNContractID"]), errNLCode.ToString()));
            }

            MyUtility.Msg.InfoBox("Import Complete!!");
        }

        private void TxtWKNo_Validating(object sender, CancelEventArgs e)
        {
            string chksql;
            chksql = $@"select 1 from Adjust where id = '{this.txtWKNo.Text}' and Type = 'R'";
            if (MyUtility.Check.Seek(chksql))
            {

            }

            chksql = $@"select 1 from FtyExport where id = '{this.txtWKNo.Text}' and Type = 3";
            if (MyUtility.Check.Seek(chksql))
            {

            }
        }
    }
}
