using System;
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
using System.Data.SqlClient;

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
select v.id,vdd.Refno,vdd.FabricType,vd.NLCode,vd.Qty,c.HSCode,c.UnitID,vdd.BrandID
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
                .Text("BrandID", header: "Brand", width: Widths.AnsiChars(10), iseditingreadonly: true)
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
            if (MyUtility.Check.Empty(this.CurrentDetailData))
            {
                MyUtility.Msg.InfoBox("No any data.");
                return false;
            }
            #region
            DataTable dt;
            string sqlcmd = $@"
select
	[seq]=ROW_NUMBER()over(order by vaqd.NLCode),
	vaqd.NLCode,
	vcd.DescVI,
	vd.HSCode,
	ORIGIN=isnull(b.NameEN,'TAIWAN'),
	vaqd.Qty,
	vd.UnitID,
	PRICE=null,
	AMOUNT=null,
	TAXFREECODE=null
from VNContractQtyAdjust vaq
inner join VNContractQtyAdjust_Detail vaqd with(nolock)on vaq.id = vaqd.id
left join VNContract_Detail vd with(nolock)on vd.ID = vaq.VNContractID and vd.NLCode=vaqd.NLCode
left join VNNLCodeDesc vcd with(nolock)on vcd.NLCode=vd.NLCode
outer apply(
	select top 1 FromSite
	from VNImportDeclaration vid with(nolock)
	where vid.VNContractID = vaq.VNContractID and vid.DeclareNo = vaq.DeclareNo
)a
outer apply(
	select c.NameEn from Country c with(nolock)where c.id = a.FromSite
)b
where vaq.id = '{this.CurrentMaintain["ID"]}'
order by vaqd.NLCode
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

                DataRow newRow = ((DataTable)this.detailgridbs.DataSource).NewRow();
                string type = MyUtility.Convert.GetString(MyUtility.Excel.GetExcelCellValue(objCellArray[1, 2], "C"));
                newRow["Refno"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 1], "C");
                string nlCode = string.Empty;
                if (type.EqualString("A") || type.EqualString("F"))
                {
                    nlCode = MyUtility.GetValue.Lookup($"select distinct NLCode from Fabric with(nolock) where refno = '{newRow["Refno"]}'");
                }
                else if (type.EqualString("L"))
                {
                    nlCode = MyUtility.GetValue.Lookup($"select NLCode from LocalItem with(nolock) where refno = '{newRow["Refno"]}'");
                }

                string chkVNContract_Detail = $@"
select HSCode,UnitID from VNContract_Detail WITH (NOLOCK) where ID = '{this.CurrentMaintain["VNContractID"]}' and NLCode = '{nlCode}'";

                if (!MyUtility.Check.Seek(chkVNContract_Detail, out seekData))
                {
                    errNLCode.Append(string.Format("Customs Code: {0}\r\n", MyUtility.Convert.GetString(MyUtility.Excel.GetExcelCellValue(objCellArray[1, 1], "C"))));
                    continue;
                }
                else
                {
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
            if (this.txtWKNo.Text.Empty())
            {
                ((DataTable)this.detailgridbs.DataSource).Clear();
                return;
            }

            string chksql;
            string sqlcmd = string.Empty;
            #region A F
            chksql = $@"select 1 from Adjust where id = '{this.txtWKNo.Text}' and Type = 'R'";
            if (MyUtility.Check.Seek(chksql))
            {
                sqlcmd = $@"
select distinct f.type,
	HSCode = f.HSCode,
	psd.Refno,
	f.NLCode,
	qty=ad.QtyAfter-ad.QtyBefore,
	f.CustomsUnit,
	psd.BrandId,
	ad.ukey --for distinct 因用refno串Fabric會展開
into #tmp
from Adjust a with(nolock)
inner join Adjust_Detail ad with(nolock) on ad.ID=a.ID
inner join PO_Supp_Detail psd with(nolock) on psd.id = ad.POID and psd.SEQ1 = ad.Seq1 and psd .seq2 = ad. seq2
left join Fabric f with(nolock) on f.Refno = psd.Refno
where a.Type = 'R' and a.id = '{this.txtWKNo.Text}'

select Type,HSCode,Refno,NLCode,CustomsUnit,BrandId,qty = sum(qty)
into #tmp2
from #tmp 
group by Type,HSCode,Refno,NLCode,CustomsUnit,BrandId

select HSCode,Refno,NLCode,qty = case when type = 'A' then A.Qty when type = 'F' then F.Qty end,UnitID=t.CustomsUnit
into #tmp3
from #tmp2 t
outer apply(
	select
		[Qty] = [dbo].getVNUnitTransfer(f.Type,f.UsageUnit,f.CustomsUnit,t.qty,0,f.PcsWidth,f.PcsLength,f.PcsKg,
		IIF(CustomsUnit = 'M2',M2Rate.value,Rate.value),IIF(CustomsUnit = 'M2',M2UnitRate.value,UnitRate.value))
	from Fabric f with (nolock)
	outer apply (select [value] = RateValue from dbo.View_Unitrate where FROM_U = f.UsageUnit and TO_U = f.CustomsUnit) Rate
	outer apply (select [value] = RateValue from dbo.View_Unitrate where FROM_U = f.UsageUnit and TO_U = 'M') M2Rate
	outer apply (select [value] = Rate from Unit_Rate WITH (NOLOCK) where UnitFrom = f.UsageUnit and UnitTo = f.CustomsUnit) UnitRate
	outer apply (select [value] = Rate from Unit_Rate WITH (NOLOCK) where UnitFrom = f.UsageUnit and UnitTo = 'M') M2UnitRate
	where f.Refno = t.Refno and f.BrandId = t.BrandId
)A
outer apply(
	select
		[Qty] = [dbo].getVNUnitTransfer('F','YDS',f.CustomsUnit,t.qty,f.Width,f.PcsWidth,f.PcsLength,f.PcsKg,
		IIF(f.CustomsUnit = 'M2',M2Rate.value,isnull(Rate.value,1)),IIF(CustomsUnit = 'M2',M2UnitRate.value,UnitRate.value))
	from Fabric f with (nolock)
	outer apply (select [value] = RateValue from dbo.View_Unitrate where FROM_U = 'YDS' and TO_U = f.CustomsUnit) Rate
	outer apply (select [value] = RateValue from dbo.View_Unitrate where FROM_U = 'YDS' and TO_U = 'M') M2Rate
	outer apply (select [value] = Rate from Unit_Rate WITH (NOLOCK) where UnitFrom = 'YDS' and UnitTo = f.CustomsUnit) UnitRate
	outer apply (select [value] = Rate from Unit_Rate WITH (NOLOCK) where UnitFrom = 'YDS' and UnitTo = 'M') M2UnitRate
	where f.Refno = t.Refno and f.BrandId = t.BrandId
)F

select HSCode,Refno,NLCode,UnitID,qty = sum(qty) from #tmp3 group by HSCode,Refno,NLCode,UnitID

drop table #tmp,#tmp2,#tmp3
";
            }
            #endregion

            #region only Local
            chksql = $@"select 1 from AdjustLocal where id = '{this.txtWKNo.Text}' and Type = 'R'";
            if (MyUtility.Check.Seek(chksql))
            {
                sqlcmd = $@"
select
	li.HSCode,
	ald.Refno,
	li.NLCode,
	Qty=sum(ald.QtyAfter - ald.QtyBefore),
	UnitID=li.CustomsUnit
into #tmp
from AdjustLocal al with(nolock)
inner join AdjustLocal_Detail ald with(nolock)on ald.id = al.id
left join LocalItem li with(nolock)on li.RefNo = ald.Refno
where al.Type = 'R' and al.id = '{this.txtWKNo.Text}'
group by li.HSCode,ald.Refno,li.NLCode,li.CustomsUnit

select HSCode,Refno,NLCode,UnitID,L.Qty
from #tmp t
outer apply(
	select  
			[Qty] = [dbo].getVNUnitTransfer('',li.UnitID,isnull(li.CustomsUnit,''),t.Qty,0,li.PcsWidth,li.PcsLength,li.PcsKg,IIF(li.CustomsUnit = 'M2',M2Rate.value,Rate.value),IIF(li.CustomsUnit = 'M2',M2UnitRate.value,UnitRate.value))
	from LocalItem li with (nolock) 
	outer apply (select [value] = RateValue from dbo.View_Unitrate where FROM_U = li.UnitID and TO_U = li.CustomsUnit) Rate
	outer apply (select [value] = RateValue from dbo.View_Unitrate where FROM_U = li.UnitID and TO_U = 'M') M2Rate
	outer apply (select [value] = Rate from Unit_Rate WITH (NOLOCK) where UnitFrom = li.UnitID and UnitTo = li.CustomsUnit) UnitRate
	outer apply (select [value] = Rate from Unit_Rate WITH (NOLOCK) where UnitFrom = li.UnitID and UnitTo = 'M') M2UnitRate
	where li.Refno = t.Refno
)L

drop table #tmp
";
            }
            #endregion

            #region 3
            chksql = $@"select 1 from FtyExport where id = '{this.txtWKNo.Text}' and Type = 3";
            if (MyUtility.Check.Seek(chksql))
            {
                sqlcmd = $@"
select
	HSCode = case when fed.FabricType = 'A'or fed.FabricType = 'F' then f1.HSCode
			        when fed.FabricType = '' then li.HSCode
					end
	,NLCode = case when fed.FabricType = 'A'or fed.FabricType = 'F' then f1.NLCode
			         when fed.FabricType = '' then li.NLCode
					 end
	,RefNo = fed.refno
	,Qty = case when fed.FabricType = 'A' then a.Qty
			            when fed.FabricType = 'F' then FF.Qty
			            when fed.FabricType = '' then L.Qty
					end
	,UnitID= case when fed.FabricType = 'A'or fed.FabricType = 'F' then f1.CustomsUnit
			         when fed.FabricType = '' then li.CustomsUnit
					 end
	,fe.id,f1.BrandID
into #tmp
from FtyExport fe WITH (NOLOCK)
inner join FtyExport_Detail fed WITH (NOLOCK) on fe.id=fed.id	
left join Fabric f1 WITH (NOLOCK) on f1.Refno=fed.Refno			
left join LocalItem li WITH (NOLOCK) on li.Refno = fed.RefNo
outer apply(
	select  
		[Qty] = [dbo].getVNUnitTransfer(f.Type,f.UsageUnit,f.CustomsUnit,fed.qty,0,f.PcsWidth,f.PcsLength,f.PcsKg,IIF(CustomsUnit = 'M2',M2Rate.value,Rate.value),IIF(CustomsUnit = 'M2',M2UnitRate.value,UnitRate.value))
	from Fabric f with (nolock)
	outer apply (select [value] = RateValue from dbo.View_Unitrate where FROM_U = f.UsageUnit and TO_U = f.CustomsUnit) Rate
	outer apply (select [value] = RateValue from dbo.View_Unitrate where FROM_U = f.UsageUnit and TO_U = 'M') M2Rate
	outer apply (select [value] = Rate from Unit_Rate WITH (NOLOCK) where UnitFrom = f.UsageUnit and UnitTo = f.CustomsUnit) UnitRate
	outer apply (select [value] = Rate from Unit_Rate WITH (NOLOCK) where UnitFrom = f.UsageUnit and UnitTo = 'M') M2UnitRate
	where f.Refno = fed.refno and f.BrandID = f1.BrandID
)A
outer apply(
	select  
		[Qty] = [dbo].getVNUnitTransfer('F','YDS',f.CustomsUnit,fed.qty,f.Width,f.PcsWidth,f.PcsLength,f.PcsKg,IIF(f.CustomsUnit = 'M2',M2Rate.value,isnull(Rate.value,1)),IIF(CustomsUnit = 'M2',M2UnitRate.value,UnitRate.value))
	from Fabric f with (nolock)
	outer apply (select [value] = RateValue from dbo.View_Unitrate where FROM_U = 'YDS' and TO_U = f.CustomsUnit) Rate
	outer apply (select [value] = RateValue from dbo.View_Unitrate where FROM_U = 'YDS' and TO_U = 'M') M2Rate
	outer apply (select [value] = Rate from Unit_Rate WITH (NOLOCK) where UnitFrom = 'YDS' and UnitTo = f.CustomsUnit) UnitRate
	outer apply (select [value] = Rate from Unit_Rate WITH (NOLOCK) where UnitFrom = 'YDS' and UnitTo = 'M') M2UnitRate
	where f.Refno = fed.refno and f.BrandID = f1.BrandID
)FF
outer apply(
	select  
			[Qty] = [dbo].getVNUnitTransfer('',li.UnitID,isnull(li.CustomsUnit,''),fed.qty,0,li.PcsWidth,li.PcsLength,li.PcsKg,IIF(li.CustomsUnit = 'M2',M2Rate.value,Rate.value),IIF(li.CustomsUnit = 'M2',M2UnitRate.value,UnitRate.value))
	from LocalItem li with (nolock) 
	outer apply (select [value] = RateValue from dbo.View_Unitrate where FROM_U = li.UnitID and TO_U = li.CustomsUnit) Rate
	outer apply (select [value] = RateValue from dbo.View_Unitrate where FROM_U = li.UnitID and TO_U = 'M') M2Rate
	outer apply (select [value] = Rate from Unit_Rate WITH (NOLOCK) where UnitFrom = li.UnitID and UnitTo = li.CustomsUnit) UnitRate
	outer apply (select [value] = Rate from Unit_Rate WITH (NOLOCK) where UnitFrom = li.UnitID and UnitTo = 'M') M2UnitRate
	where li.Refno = fed.refno
)L
where fe.Type = 3 and fe.id='{this.txtWKNo.Text}'

select HSCode,NLCode,RefNo,UnitID,qty=sum(qty)
from #tmp
group by HSCode,NLCode,RefNo,UnitID

drop table #tmp
";
            }
            #endregion
            DataTable dt;
            if (sqlcmd.Empty())
            {
                return;
            }

            DualResult result = DBProxy.Current.Select(null, sqlcmd, out dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            ((DataTable)this.detailgridbs.DataSource).Clear();
            foreach (DataRow dr in dt.Rows)
            {
                ((DataTable)this.detailgridbs.DataSource).ImportRow(dr);
            }
        }
    }
}
