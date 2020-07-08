using System;
using System.ComponentModel;
using System.Data;
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
    public partial class P42 : Win.Tems.Input6
    {
        private DataGridViewGeneratorTextColumnSettings nlcode = new DataGridViewGeneratorTextColumnSettings();
        private DataGridViewGeneratorTextColumnSettings brand = new DataGridViewGeneratorTextColumnSettings();
        private DataGridViewGeneratorTextColumnSettings refno = new DataGridViewGeneratorTextColumnSettings();
        private DataGridViewGeneratorTextColumnSettings fabricType = new DataGridViewGeneratorTextColumnSettings();
        private DataGridViewGeneratorTextColumnSettings usageUnit = new DataGridViewGeneratorTextColumnSettings();

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
select v.id,vdd.Refno,vdd.FabricType,vdd.NLCode,vdd.Qty,c.HSCode,c.UnitID,vdd.BrandID,vdd.UsageUnit
from VNContractQtyAdjust v WITH (NOLOCK) 
inner join VNContractQtyAdjust_Detail vd WITH (NOLOCK) on v.ID = vd.ID
left join VNContractQtyAdjust_Detail_Detail vdd WITH (NOLOCK) on v.ID = vdd.ID and vd.NLCode = vdd.NLCode
left join VNContract_Detail c WITH (NOLOCK) on c.ID = v.VNContractID and c.NLCode = vd.NLCode
where {0}
order by TRY_CONVERT(int, SUBSTRING(vd.NLCode, 3, LEN(vd.NLCode))), vd.NLCode", masterID);
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
            #region brand的Validating
            this.brand.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    if (!MyUtility.Check.Empty(e.FormattedValue))
                    {
                        if (MyUtility.Convert.GetString(dr["BrandID"]) != MyUtility.Convert.GetString(e.FormattedValue))
                        {
                            string sqlchk = $@"select 1 from Brand with(nolock) where ID = '{e.FormattedValue}' ";
                            if (!MyUtility.Check.Seek(sqlchk) && MyUtility.Convert.GetString(dr["FabricType"]).EqualString("L"))
                            {
                                dr["BrandID"] = DBNull.Value;
                                MyUtility.Msg.WarningBox("Brand not found!!");
                                dr.EndEdit();
                                return;
                            }
                            else
                            {
                                dr["BrandID"] = e.FormattedValue;
                                dr.EndEdit();
                                this.BRT(dr, e);
                            }
                        }
                    }
                    else
                    {
                        dr["BrandID"] = string.Empty;
                    }

                    dr.EndEdit();
                }
            };
            #endregion
            #region refno的Validating
            this.refno.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    if (!MyUtility.Check.Empty(e.FormattedValue))
                    {
                        dr["refno"] = e.FormattedValue;
                        dr.EndEdit();
                        this.BRT(dr, e);
                    }
                }
            };
            #endregion
            #region fabricType的Validating
            this.fabricType.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    if (!MyUtility.Check.Empty(e.FormattedValue))
                    {
                        dr["fabricType"] = e.FormattedValue;
                        dr.EndEdit();
                        if (!(MyUtility.Convert.GetString(dr["FabricType"]) == "F" || MyUtility.Convert.GetString(dr["FabricType"]) == "A"))
                        {
                            dr["usageUnit"] = string.Empty;
                            dr.EndEdit();
                        }

                        this.BRT(dr, e);
                    }
                }
            };
            #endregion
            #region usageUnit的Validating
            this.usageUnit.CellEditable += (s, e) =>
            {
                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (!(MyUtility.Convert.GetString(dr["FabricType"]) == "F" || MyUtility.Convert.GetString(dr["FabricType"]) == "A"))
                {
                    e.IsEditable = false;
                }
            };

            this.usageUnit.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    if (!MyUtility.Check.Empty(e.FormattedValue))
                    {
                        dr["usageUnit"] = e.FormattedValue;
                        dr.EndEdit();
                        this.BRT(dr, e);
                    }
                }
            };
            #endregion
            DataGridViewGeneratorNumericColumnSettings stockQtySetting = new DataGridViewGeneratorNumericColumnSettings();
            stockQtySetting.IsSupportNegative = true;

            base.OnDetailGridSetup();

            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("BrandID", header: "Brand", width: Widths.AnsiChars(10), settings: this.brand)
                .Text("Refno", header: "Ref No.", width: Widths.AnsiChars(10), settings: this.refno)
                .Text("FabricType", header: "Type", width: Widths.AnsiChars(10), settings: this.fabricType)
                .Text("UsageUnit", header: "UsageUnit", width: Widths.AnsiChars(8), settings: this.usageUnit)
                .Text("HSCode", header: "HS Code", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("NLCode", header: "Customs Code", width: Widths.AnsiChars(12), settings: this.nlcode)
                .Numeric("Qty", header: "Qty", decimal_places: 3, width: Widths.AnsiChars(15), settings: stockQtySetting)
                .Text("UnitID", header: "Unit", width: Widths.AnsiChars(8), iseditingreadonly: true);
        }

        private void BRT(DataRow dr, Ict.Win.UI.DataGridViewCellValidatingEventArgs e)
        {
            string type = MyUtility.Convert.GetString(dr["FabricType"]);
            bool isNeedInputBrand = !MyUtility.Check.Empty(dr["BrandID"]) && !MyUtility.Check.Empty(dr["Refno"]) && !MyUtility.Check.Empty(dr["FabricType"]) && !MyUtility.Check.Empty(dr["usageUnit"]);
            bool isNoNeedInputBrand = (type.EqualString("L") || type.EqualString("Misc")) && !MyUtility.Check.Empty(dr["Refno"]) && !MyUtility.Check.Empty(dr["FabricType"]);

            if (isNeedInputBrand ||
                isNoNeedInputBrand)
            {
                string usageUnit = string.Empty;
                if (type.EqualString("Misc") || type.EqualString("L"))
                {
                    dr["BrandID"] = string.Empty;
                }

                if (type.EqualString("F") || type.EqualString("A"))
                {
                    usageUnit = MyUtility.Convert.GetString(dr["usageUnit"]);
                }

                DataRow seekData = this.GetNLCodeInfo(dr["Refno"].ToString(), type, usageUnit);
                if (seekData == null)
                {
                    dr["HSCode"] = string.Empty;
                    dr["NLCode"] = string.Empty;
                    dr["Qty"] = 0;
                    dr["UnitID"] = string.Empty;
                    MyUtility.Msg.WarningBox("Customs Code not found!!");
                }
                else
                {
                    dr["NLCode"] = seekData["NLCode"];
                    dr["HSCode"] = seekData["HSCode"];
                    dr["UnitID"] = seekData["UnitID"];
                }

                dr.EndEdit();
            }
            else
            {
                dr["HSCode"] = string.Empty;
                dr["NLCode"] = string.Empty;
                dr["Qty"] = 0;
                dr["UnitID"] = string.Empty;
                dr.EndEdit();
            }
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

        protected override DualResult ClickDeletePost()
        {
            string sqldelete = $@"delete [VNContractQtyAdjust_Detail] where id = '{this.CurrentMaintain["id"]}'";
            DualResult result = DBProxy.Current.Execute(null, sqldelete);
            if (!result)
            {
                return Result.F(result.ToString());
            }

            return base.ClickDeletePost();
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

            if (MyUtility.Check.Empty(this.CurrentMaintain["ReasonID"]))
            {
                this.txtShippingReason1.TextBox1.Focus();
                MyUtility.Msg.WarningBox("Reason cannot be empty.");
                return false;
            }

            if (MyUtility.Convert.GetBool(MyUtility.GetValue.Lookup($"select IsFtyWk from ShippingReason where id = '{this.CurrentMaintain["ReasonID"]}'")) &&
                MyUtility.Check.Empty(this.CurrentMaintain["WKNo"]))
            {
                MyUtility.Msg.WarningBox($"Reason '{this.txtShippingReason1.DisplayBox1.Text}' need input Fty WK# on adjustment memo.");
                return false;
            }

            #endregion
            #region
            foreach (DataRow dr in this.DetailDatas)
            {
                if (MyUtility.Check.Empty(dr["BrandID"]) && !MyUtility.Convert.GetString(dr["FabricType"]).EqualString("L") && !MyUtility.Convert.GetString(dr["FabricType"]).EqualString("Misc"))
                {
                    MyUtility.Msg.WarningBox("Brand cannot be empty!");
                    return false;
                }

                if (MyUtility.Check.Empty(dr["Refno"]))
                {
                    MyUtility.Msg.WarningBox("Ref No. cannot be empty!");
                    return false;
                }

                if (MyUtility.Check.Empty(dr["FabricType"]))
                {
                    MyUtility.Msg.WarningBox("FabricType cannot be empty!");
                    return false;
                }
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

            #region 檢查 Refno+NLCode是否存在[Fabric]
            foreach (DataRow dr in this.DetailDatas)
            {
                string fabricType = MyUtility.Convert.GetString(dr["fabricType"]);
                if (fabricType.EqualString("A") || fabricType.EqualString("F"))
                {
                    string sqlchk = $@"select 1 from Fabric with(nolock) where refno = '{dr["refno"]}' and nlcode = '{dr["nlcode"]}'";
                    if (!MyUtility.Check.Seek(sqlchk))
                    {
                        MyUtility.Msg.WarningBox($"Ref No. :{dr["refno"]} , Customs Code : {dr["nlcode"]} not exists!");
                        return false;
                    }
                }
                else if (fabricType.EqualString("L"))
                {
                    string sqlchk = $@"select 1 from LocalItem with(nolock) where ltrim(refno) = '{dr["refno"]}' and nlcode = '{dr["nlcode"]}'";
                    if (!MyUtility.Check.Seek(sqlchk))
                    {
                        MyUtility.Msg.WarningBox($"Ref No. :{dr["refno"]} , Customs Code : {dr["nlcode"]} not exists!");
                        return false;
                    }
                }
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

                range = worksheet.Range[string.Format("A{0}:F{0}", intRowsRead)];
                objCellArray = range.Value;

                DataRow newRow = ((DataTable)this.detailgridbs.DataSource).NewRow();
                string type = MyUtility.Convert.GetString(MyUtility.Excel.GetExcelCellValue(objCellArray[1, 3], "C"));
                newRow["Refno"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 2], "C");
                newRow["usageUnit"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 5], "C");
                seekData = this.GetNLCodeInfo(newRow["Refno"].ToString(), type, MyUtility.Convert.GetString(newRow["usageUnit"]));
                if (seekData == null)
                {
                    errNLCode.Append(string.Format("Customs Code: {0}\r\n", MyUtility.Convert.GetString(MyUtility.Excel.GetExcelCellValue(objCellArray[1, 2], "C"))));
                    continue;
                }
                else
                {
                    newRow["BrandID"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 1], "C");
                    newRow["FabricType"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 3], "C");
                    newRow["NLCode"] = seekData["NLCode"];
                    newRow["Qty"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 4], "N");
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
select
	psd.Refno,
	FabricType=f1.type,
	f1.NLCode,
	f1.BrandId,
	f1.HSCode,
	UnitID=f1.CustomsUnit,
	qty=sum(ad.QtyBefore-ad.QtyAfter),
	UsageUnit = FabricUsage.v
into #tmp
from Adjust a with(nolock)
inner join Adjust_Detail ad with(nolock) on ad.ID=a.ID
inner join PO_Supp_Detail psd with(nolock) on psd.id = ad.POID and psd.SEQ1 = ad.Seq1 and psd .seq2 = ad. seq2
left join orders o with(nolock) on o.id = ad.POID
left join brand b with(nolock) on b.id = o.BrandID
left join fabric f with(nolock) on psd.SCIRefno = f.SCIRefno
outer apply(
	select top 1 f.*
	from Fabric f with(nolock)
	inner join brand b2 with(nolock) on f.BrandID = b2.id 
	where f.Refno = psd.Refno and b2.BrandGroup = b.BrandGroup
	order by f.NLCodeEditDate desc
)f1
outer apply (
    select v = isnull (f.UsageUnit, f1.SCIRefno)
) FabricUsage
where a.Type = 'R' and a.id = '{this.txtWKNo.Text}'
group by psd.Refno,f1.type,f1.NLCode,f1.BrandId,f1.HSCode,f1.CustomsUnit,FabricUsage.v

select Refno,FabricType,NLCode,BrandId,HSCode,UnitID,qty = case when FabricType = 'A' then A.Qty when FabricType = 'F' then F.Qty end, t.UsageUnit
from #tmp t
outer apply(
	select top 1
		[Qty] = [dbo].getVNUnitTransfer(f.Type,f.UsageUnit,f.CustomsUnit,t.qty,0,f.PcsWidth,f.PcsLength,f.PcsKg,
		IIF(CustomsUnit = 'M2',M2Rate.value,Rate.value),IIF(CustomsUnit = 'M2',M2UnitRate.value,UnitRate.value),default)
	from Fabric f with (nolock)
	outer apply (select [value] = RateValue from dbo.View_Unitrate where FROM_U = f.UsageUnit and TO_U = f.CustomsUnit) Rate
	outer apply (select [value] = RateValue from dbo.View_Unitrate where FROM_U = f.UsageUnit and TO_U = 'M') M2Rate
	outer apply (select [value] = Rate from Unit_Rate WITH (NOLOCK) where UnitFrom = f.UsageUnit and UnitTo = f.CustomsUnit) UnitRate
	outer apply (select [value] = Rate from Unit_Rate WITH (NOLOCK) where UnitFrom = f.UsageUnit and UnitTo = 'M') M2UnitRate
	where f.Refno = t.Refno and f.BrandID = t.BrandId order by f.NLCodeEditDate desc
)A
outer apply(
	select top 1
		[Qty] = [dbo].getVNUnitTransfer('F','YDS',f.CustomsUnit,t.qty,f.Width,f.PcsWidth,f.PcsLength,f.PcsKg,
		IIF(f.CustomsUnit = 'M2',M2Rate.value,isnull(Rate.value,1)),IIF(CustomsUnit = 'M2',M2UnitRate.value,UnitRate.value),default)
	from Fabric f with (nolock)
	outer apply (select [value] = RateValue from dbo.View_Unitrate where FROM_U = 'YDS' and TO_U = f.CustomsUnit) Rate
	outer apply (select [value] = RateValue from dbo.View_Unitrate where FROM_U = 'YDS' and TO_U = 'M') M2Rate
	outer apply (select [value] = Rate from Unit_Rate WITH (NOLOCK) where UnitFrom = 'YDS' and UnitTo = f.CustomsUnit) UnitRate
	outer apply (select [value] = Rate from Unit_Rate WITH (NOLOCK) where UnitFrom = 'YDS' and UnitTo = 'M') M2UnitRate
	where f.Refno = t.Refno and f.BrandID = t.BrandId order by f.NLCodeEditDate desc
)F

drop table #tmp
";
            }
            #endregion

            #region only Local
            chksql = $@"select 1 from AdjustLocal where id = '{this.txtWKNo.Text}' and Type = 'R'";
            if (MyUtility.Check.Seek(chksql))
            {
                sqlcmd = $@"
select
	ald.Refno,
	li.NLCode,
	BrandId=o.BrandID,
	li.HSCode,
	UnitID=li.CustomsUnit,
	Qty=sum(ald.QtyBefore-ald.QtyAfter)
into #tmp
from AdjustLocal al with(nolock)
inner join AdjustLocal_Detail ald with(nolock)on ald.id = al.id
left join LocalItem li with(nolock)on ltrim(li.RefNo) = ald.Refno
left join orders o with(nolock) on o.id = ald.POID
where al.Type = 'R' and al.id = '{this.txtWKNo.Text}'
group by ald.Refno,li.NLCode,o.BrandID,li.HSCode,li.CustomsUnit

select Refno,FabricType='L',NLCode,BrandId,HSCode,UnitID,L.Qty,UsageUnit=''
from #tmp t
outer apply(
	select  
		[Qty] = [dbo].getVNUnitTransfer(li.Category,li.UnitID,isnull(li.CustomsUnit,''),t.Qty,0,li.PcsWidth,li.PcsLength,li.PcsKg,IIF(li.CustomsUnit = 'M2',M2Rate.value,Rate.value),IIF(li.CustomsUnit = 'M2',M2UnitRate.value,UnitRate.value),li.Refno)
	from LocalItem li with (nolock) 
	outer apply (select [value] = RateValue from dbo.View_Unitrate where FROM_U = li.UnitID and TO_U = li.CustomsUnit) Rate
	outer apply (select [value] = RateValue from dbo.View_Unitrate where FROM_U = li.UnitID and TO_U = 'M') M2Rate
	outer apply (select [value] = Rate from Unit_Rate WITH (NOLOCK) where UnitFrom = li.UnitID and UnitTo = li.CustomsUnit) UnitRate
	outer apply (select [value] = Rate from Unit_Rate WITH (NOLOCK) where UnitFrom = li.UnitID and UnitTo = 'M') M2UnitRate
	where ltrim(li.Refno) = t.Refno
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
	fed.refno,
	fed.FabricType,
	NLCode = case when fed.FabricType = 'A'or fed.FabricType = 'F' then f1.NLCode
			    when fed.FabricType = '' then li.NLCode
				end,
	f1.BrandID,
	HSCode = case when fed.FabricType = 'A'or fed.FabricType = 'F' then f1.HSCode
			        when fed.FabricType = '' then li.HSCode
					end,
	UnitID= case when fed.FabricType = 'A'or fed.FabricType = 'F' then f1.CustomsUnit
			         when fed.FabricType = '' then li.CustomsUnit
					 end,
	Qty = case when fed.FabricType = 'A' then a.Qty
			            when fed.FabricType = 'F' then FF.Qty
			            when fed.FabricType = '' then L.Qty
					end,
	f1.UsageUnit
into #tmp
from FtyExport fe WITH (NOLOCK)
inner join FtyExport_Detail fed WITH (NOLOCK) on fe.id=fed.id	
left join LocalItem li WITH (NOLOCK) on ltrim(li.Refno) = fed.RefNo
left join orders o with(nolock) on o.id = fed.POID
left join brand b with(nolock) on b.id = o.BrandID
outer apply(
	select top 1 f.*
	from Fabric f with(nolock)
	inner join brand b2 with(nolock) on f.BrandID = b2.id 
	where f.Refno = fed.Refno and b2.BrandGroup = b.BrandGroup
	order by f.NLCodeEditDate desc
)f1
outer apply(
	select top 1
		[Qty] = [dbo].getVNUnitTransfer(f.Type,f.UsageUnit,f.CustomsUnit,fed.qty,0,f.PcsWidth,f.PcsLength,f.PcsKg,IIF(CustomsUnit = 'M2',M2Rate.value,Rate.value),IIF(CustomsUnit = 'M2',M2UnitRate.value,UnitRate.value),default)
	from Fabric f with (nolock)
	outer apply (select [value] = RateValue from dbo.View_Unitrate where FROM_U = f.UsageUnit and TO_U = f.CustomsUnit) Rate
	outer apply (select [value] = RateValue from dbo.View_Unitrate where FROM_U = f.UsageUnit and TO_U = 'M') M2Rate
	outer apply (select [value] = Rate from Unit_Rate WITH (NOLOCK) where UnitFrom = f.UsageUnit and UnitTo = f.CustomsUnit) UnitRate
	outer apply (select [value] = Rate from Unit_Rate WITH (NOLOCK) where UnitFrom = f.UsageUnit and UnitTo = 'M') M2UnitRate
	where f.Refno = fed.refno and f.BrandID = f1.BrandID
)A
outer apply(
	select top 1
		[Qty] = [dbo].getVNUnitTransfer('F','YDS',f.CustomsUnit,fed.qty,f.Width,f.PcsWidth,f.PcsLength,f.PcsKg,IIF(f.CustomsUnit = 'M2',M2Rate.value,isnull(Rate.value,1)),IIF(CustomsUnit = 'M2',M2UnitRate.value,UnitRate.value),default)
	from Fabric f with (nolock)
	outer apply (select [value] = RateValue from dbo.View_Unitrate where FROM_U = 'YDS' and TO_U = f.CustomsUnit) Rate
	outer apply (select [value] = RateValue from dbo.View_Unitrate where FROM_U = 'YDS' and TO_U = 'M') M2Rate
	outer apply (select [value] = Rate from Unit_Rate WITH (NOLOCK) where UnitFrom = 'YDS' and UnitTo = f.CustomsUnit) UnitRate
	outer apply (select [value] = Rate from Unit_Rate WITH (NOLOCK) where UnitFrom = 'YDS' and UnitTo = 'M') M2UnitRate
	where f.Refno = fed.refno and f.BrandID = f1.BrandID
)FF
outer apply(
	select
			[Qty] = [dbo].getVNUnitTransfer(li.Category,li.UnitID,isnull(li.CustomsUnit,''),fed.qty,0,li.PcsWidth,li.PcsLength,li.PcsKg,IIF(li.CustomsUnit = 'M2',M2Rate.value,Rate.value),IIF(li.CustomsUnit = 'M2',M2UnitRate.value,UnitRate.value),li.Refno)
	from LocalItem li with (nolock) 
	outer apply (select [value] = RateValue from dbo.View_Unitrate where FROM_U = li.UnitID and TO_U = li.CustomsUnit) Rate
	outer apply (select [value] = RateValue from dbo.View_Unitrate where FROM_U = li.UnitID and TO_U = 'M') M2Rate
	outer apply (select [value] = Rate from Unit_Rate WITH (NOLOCK) where UnitFrom = li.UnitID and UnitTo = li.CustomsUnit) UnitRate
	outer apply (select [value] = Rate from Unit_Rate WITH (NOLOCK) where UnitFrom = li.UnitID and UnitTo = 'M') M2UnitRate
	where li.Refno = fed.refno
)L
where fe.Type = 3 and fe.id='{this.txtWKNo.Text}'

select Refno,FabricType,NLCode,BrandId,HSCode,UnitID,UsageUnit, qty = sum(qty)
from #tmp
group by Refno,FabricType,NLCode,BrandId,HSCode,UnitID,UsageUnit

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

        private void BtnDownloadexcel(object sender, EventArgs e)
        {
            string strXltName = Sci.Env.Cfg.XltPathDir + "\\Shipping_P42_ImportExcelFormat.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return;
            }

            excel.Visible = true;
        }

        private DataRow GetNLCodeInfo(string refno, string fabricType, string usageUnit)
        {
            string nlCode = string.Empty;
            if (fabricType.EqualString("A") || fabricType.EqualString("F"))
            {
                nlCode = MyUtility.GetValue.Lookup($"select top 1 NLCode from Fabric with(nolock) where refno = '{refno}' and Type='{fabricType}' and usageUnit = '{usageUnit}' ");
            }
            else if (fabricType.EqualString("L"))
            {
                nlCode = MyUtility.GetValue.Lookup($"select top 1 NLCode from LocalItem with(nolock) where ltrim(refno) = '{refno}'");
            }
            else if (fabricType.EqualString("Misc"))
            {
                nlCode = MyUtility.GetValue.Lookup($"select top 1 NLCode from SciMachine_Misc with(nolock) where ltrim(ID) = '{refno}'");
            }

            StringBuilder errNLCode = new StringBuilder();
            string chkVNContract_Detail = $@"
select NLCode,HSCode,UnitID from VNContract_Detail WITH (NOLOCK) where ID = '{this.CurrentMaintain["VNContractID"]}' and NLCode = '{nlCode}'";
            DataRow seekData;
            if (MyUtility.Check.Seek(chkVNContract_Detail, out seekData))
            {
                return seekData;
            }
            else
            {
                return null;
            }
        }
    }
}
