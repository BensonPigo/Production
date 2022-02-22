using System;
using System.Data;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// P03_Print
    /// </summary>
    public partial class P03_Print : Win.Tems.PrintForm
    {
        private string eta1;
        private string eta2;
        private string factory;
        private string shipmode;
        private string handle;
        private string ext;
        private string email;
        private string type;
        private DataRow masterData;
        private DataTable detailData;
        private DataTable printData;

        /// <inheritdoc/>
        public P03_Print(DataRow masterData, DataTable detailData, string type = "")
        {
            this.InitializeComponent();
            this.radioDetailReport.Checked = true;
            this.dateETA.Enabled = false;
            this.txtfactory.Enabled = false;
            this.masterData = masterData;
            this.detailData = detailData;
            this.type = type;

            DataTable dt;
            DBProxy.Current.Select(null, "select '' as ID union all select ID from ShipMode WITH (NOLOCK) where UseFunction like '%WK%' ", out dt);
            MyUtility.Tool.SetupCombox(this.comboBox1, 1, dt);
            this.comboBox1.SelectedIndex = 0;
        }

        // 控制ETA & Factory可否輸入
        private void RadioListReport_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioListReport.Checked)
            {
                this.dateETA.Enabled = true;
                this.txtfactory.Enabled = true;
                this.comboBox1.Enabled = true;
            }
            else
            {
                this.dateETA.Enabled = false;
                this.txtfactory.Enabled = false;
                this.comboBox1.Enabled = false;
            }
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.eta1 = MyUtility.Check.Empty(this.dateETA.Value1) ? string.Empty : Convert.ToDateTime(this.dateETA.Value1).ToString("yyyy/MM/dd");
            this.eta2 = MyUtility.Check.Empty(this.dateETA.Value2) ? string.Empty : Convert.ToDateTime(this.dateETA.Value2).ToString("yyyy/MM/dd");
            this.factory = this.txtfactory.Text;
            this.shipmode = this.comboBox1.Text;
            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            if (this.radioDetailReport.Checked)
            {
                string sqlCmd = string.Format("select * from TPEPass1 WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(this.masterData["Handle"]));
                DataTable tPEPass1;
                DualResult result = DBProxy.Current.Select(null, sqlCmd, out tPEPass1);
                if (!result || tPEPass1.Rows.Count <= 0)
                {
                    this.handle = string.Empty;
                    this.ext = string.Empty;
                    this.email = string.Empty;
                }
                else
                {
                    this.handle = MyUtility.Convert.GetString(tPEPass1.Rows[0]["Name"]);
                    this.ext = MyUtility.Convert.GetString(tPEPass1.Rows[0]["ExtNo"]);
                    this.email = MyUtility.Convert.GetString(tPEPass1.Rows[0]["EMail"]);
                }
            }
            else if (this.radioRollDyelot.Checked)
            {
                string sqlCmd = @"
select  [WK#]
        ,[SP#]
        ,[Seq1]
        ,[Seq2]
        ,[C/No]
        ,[Full C/No]
        ,[LOT No]
        ,[Full LOT No]
        ,[Qty]
        ,[FOC]
        ,[NetKg]
        ,[WeiKg]
        ,[Location]
        ,[MIND QR Code]
        ,[Remark]
from (
	select ed.Ukey
		, [WK#]  = ed.id
		, [SP#] = ed.poid
		, [Seq1] = ed.seq1
		, [Seq2] = ed.seq2
		, [C/No] = convert(varchar(8), isnull(pll.PackageNo, isnull(edc.Carton, '')))
		, [Full C/No] = convert(varchar(50), isnull(pll.PackageNo, isnull(edc.Carton, '')))
		, [LOT No] = convert(varchar(8), isnull(pll.BatchNo, isnull(edc.LotNo, '')))
		, [Full LOT No] = convert(varchar(50), isnull(pll.BatchNo, isnull(edc.LotNo, '')))
		, [Qty] = x.Qty
		, [FOC] = x.FOC
		, [NetKg] = isnull(pll.NW, isnull(edc.NetKg, ed.NetKg))
		, [WeiKg] = isnull(pll.GW, isnull(edc.WeightKg, ed.WeightKg))
		, [Location] = ''
		, [MIND QR Code] = pll.QRCode
		, [Remark] = ''
	from dbo.Export_Detail ed WITH (NOLOCK) 
	inner join dbo.PO_Supp_Detail psd WITH (NOLOCK) on ed.PoID= psd.id   
													 and ed.Seq1 = psd.SEQ1    
													 and ed.Seq2 = psd.SEQ2
	left join Export_Detail_Carton edc with (nolock) on ed.Ukey = edc.Export_DetailUkey
	left join Poshippinglist pl with (nolock) on pl.POID = ed.POID and pl.Seq1 = ed.Seq1
	left join POShippingList_Line pll WITH (NOLOCK) ON  pll.POShippingList_Ukey = pl.Ukey 
														and pll.Line = ed.Seq2 
														and pll.PackageNo = isnull(edc.Carton, pll.PackageNo) 
														and pll.BatchNo = isnull(edc.LotNo, pll.BatchNo)
	outer apply(
		select Qty = isnull(pll.ShipQty, isnull(edc.Qty, ed.Qty)) 
				, FOC = isnull(pll.FOC, isnull(edc.Foc, ed.Foc)) 
	)x
	where psd.FabricType = 'F'
	and ed.id = @ExportID
	union 

	select ed.Ukey
		, [WK#] = ed.id
		, [SP#] = ed.poid
		, [Seq1] = ed.seq1
		, [Seq2] = ed.seq2
		, [C/No] = ''
		, [Full C/No] = ''
		, [LOT No] = ''
		, [Full LOT No] = ''
		, [Qty] = x.Qty
		, [FOC] = x.FOC
		, [NetKg] = isnull(edc.NetKg, ed.NetKg)
		, [WeiKg] = isnull(edc.WeightKg, ed.WeightKg)
		, [Location] = ''
		, [MIND QR Code] = ''
		, [Remark] = ''
	from dbo.Export_Detail ed WITH (NOLOCK) 
	inner join dbo.PO_Supp_Detail psd WITH (NOLOCK) on ed.PoID= psd.id   
													 and ed.Seq1 = psd.SEQ1    
													 and ed.Seq2 = psd.SEQ2
	outer apply (
		select edc.Export_DetailUkey, edc.Id, edc.PoID, edc.Seq1, edc.Seq2
			, Qty = SUM(edc.Qty)
			, Foc = SUM(edc.Foc)
			, NetKg = SUM(edc.NetKg)
			, WeightKg = SUM(edc.WeightKg)
		from Export_Detail_Carton edc
		where Export_DetailUkey = ed.Ukey
		group by edc.Export_DetailUkey, edc.Id, edc.PoID, edc.Seq1, edc.Seq2
	) edc
	outer apply(	
		select Qty = isnull(edc.Qty, ed.Qty)
				, FOC = isnull(edc.Foc, ed.Foc) 
	)x
	where psd.FabricType = 'A'
	and ed.id = @ExportID
) tm
order by tm.Ukey
";
                List<SqlParameter> listPar = new List<SqlParameter>() { new SqlParameter("@ExportID", this.masterData["ID"]) };
                DualResult result = DBProxy.Current.Select(null, sqlCmd, listPar, out this.printData);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                    return failResult;
                }
            }
            else
            {
                string sqlCmd = string.Format(
                    @"select e.ID,e.Eta,e.Blno,e.InvNo,e.PackingArrival,e.PortArrival,e.WhseArrival,e.DocArrival,e.Sono,e.Vessel,isnull(t.Name,'') as Name,isnull(t.ExtNo,'') as ExtNo,isnull(t.EMail,'') as EMail ,
case when e.Payer= 'S' then 'By Sci Taipei Office(Sender)' when e.Payer= 'M' then 'By Mill(Sender)' when e.Payer= 'F' then 'By Factory(Receiver)' else '' end as Payer
,[Loading] = e.ExportPort+'-'+e.ExportCountry,e.shipmodeID
from Export e WITH (NOLOCK) 
left join TPEPass1 t WITH (NOLOCK) on e.Handle = t.ID
where 1=1 and e.Junk = 0 {0}{1}{2}{3}
order by e.ID",
                    MyUtility.Check.Empty(this.eta1) ? string.Empty : " and e.Eta >= '" + this.eta1 + "'",
                    MyUtility.Check.Empty(this.eta2) ? string.Empty : " and e.Eta <= '" + this.eta2 + "'",
                    MyUtility.Check.Empty(this.factory) ? string.Empty : " and e.FactoryID = '" + this.factory + "'",
                    MyUtility.Check.Empty(this.shipmode) ? string.Empty : " and e.shipmodeID = '" + this.shipmode + "'");

                DualResult result = DBProxy.Current.Select(null, sqlCmd, out this.printData);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                    return failResult;
                }
            }

            return Ict.Result.True;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            if (this.radioDetailReport.Checked)
            {
                string filename = this.type == string.Empty ? "Shipping_P03_Detail" : "Warehouse_P02_Detail";
                string strXltName = Env.Cfg.XltPathDir + $"\\{filename}.xltx";
                Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
                if (excel == null)
                {
                    return false;
                }

                Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

                worksheet.Cells[2, 2] = MyUtility.Convert.GetString(this.masterData["ID"]);
                worksheet.Cells[2, 6] = MyUtility.Convert.GetString(this.masterData["INVNo"]);
                worksheet.Cells[2, 9] = MyUtility.Check.Empty(this.masterData["PackingArrival"]) ? string.Empty : Convert.ToDateTime(this.masterData["PackingArrival"]).ToString("yyyy/MM/dd");
                worksheet.Cells[3, 2] = MyUtility.Check.Empty(this.masterData["Eta"]) ? string.Empty : Convert.ToDateTime(this.masterData["Eta"]).ToString("yyyy/MM/dd");
                worksheet.Cells[3, 6] = MyUtility.Convert.GetString(this.masterData["Payer"]) == "S" ? "By Sci Taipei Office(Sender)" : MyUtility.Convert.GetString(this.masterData["Payer"]) == "M" ? "By Mill(Sender)" : MyUtility.Convert.GetString(this.masterData["Payer"]) == "F" ? "By Factory(Receiver)" : string.Empty;
                worksheet.Cells[3, 9] = MyUtility.Check.Empty(this.masterData["PortArrival"]) ? string.Empty : Convert.ToDateTime(this.masterData["PortArrival"]).ToString("yyyy/MM/dd");
                worksheet.Cells[4, 2] = MyUtility.Convert.GetString(this.masterData["Consignee"]);
                worksheet.Cells[4, 6] = MyUtility.Convert.GetString(this.masterData["Blno"]);
                worksheet.Cells[4, 9] = MyUtility.Check.Empty(this.masterData["WhseArrival"]) ? string.Empty : Convert.ToDateTime(this.masterData["WhseArrival"]).ToString("yyyy/MM/dd");
                worksheet.Cells[5, 2] = MyUtility.Convert.GetString(this.masterData["Packages"]);
                worksheet.Cells[5, 6] = MyUtility.Convert.GetString(this.masterData["Vessel"]);
                worksheet.Cells[5, 9] = MyUtility.Check.Empty(this.masterData["DocArrival"]) ? string.Empty : Convert.ToDateTime(this.masterData["DocArrival"]).ToString("yyyy/MM/dd");
                worksheet.Cells[6, 2] = MyUtility.Convert.GetString(this.masterData["CYCFS"]);
                worksheet.Cells[6, 6] = MyUtility.Convert.GetString(this.masterData["NetKg"]) + " / " + MyUtility.Convert.GetString(this.masterData["WeightKg"]);
                worksheet.Cells[7, 2] = MyUtility.Convert.GetString(this.masterData["Sono"]);
                worksheet.Cells[7, 6] = MyUtility.Convert.GetString(this.masterData["Cbm"]);
                worksheet.Cells[8, 2] = MyUtility.Convert.GetString(this.masterData["ExportPort"] + "-" + this.masterData["ExportCountry"]);
                worksheet.Cells[8, 6] = MyUtility.Convert.GetString(this.masterData["Remark"]);
                worksheet.Cells[9, 2] = this.handle;
                worksheet.Cells[9, 6] = this.ext;
                worksheet.Cells[9, 8] = this.email;

                int rownum = 11;
                object[,] objArray = new object[1, 21];

                foreach (DataRow dr in this.detailData.Rows)
                {
                    objArray[0, 0] = dr["FactoryID"];
                    objArray[0, 1] = dr["ProjectID"];
                    objArray[0, 2] = dr["POID"];
                    objArray[0, 3] = dr["SCIDlv"];
                    objArray[0, 4] = dr["Category"];
                    objArray[0, 5] = dr["InspDate"];
                    objArray[0, 6] = dr["Seq"];
                    if (this.type == string.Empty)
                    {
                        objArray[0, 7] = dr["Preshrink"];
                        objArray[0, 8] = dr["Supp"];
                        objArray[0, 9] = dr["Description"];
                        objArray[0, 10] = dr["UnitId"];
                        objArray[0, 11] = dr["ColorID"];
                        objArray[0, 12] = dr["SizeSpec"];
                        objArray[0, 13] = dr["OrderQty"];
                        objArray[0, 14] = dr["Qty"];
                        objArray[0, 15] = dr["Foc"];
                        objArray[0, 16] = dr["BalanceQty"];
                        objArray[0, 17] = dr["NetKg"];
                        objArray[0, 18] = dr["WeightKg"];
                    }
                    else
                    {
                        objArray[0, 7] = dr["RefNo"];
                        objArray[0, 8] = dr["MtlTypeID"];
                        objArray[0, 9] = dr["Preshrink"];
                        objArray[0, 10] = dr["Supp"];
                        objArray[0, 11] = dr["Description"];
                        objArray[0, 12] = dr["UnitId"];
                        objArray[0, 13] = dr["ColorID"];
                        objArray[0, 14] = dr["SizeSpec"];
                        objArray[0, 15] = dr["OrderQty"];
                        objArray[0, 16] = dr["Qty"];
                        objArray[0, 17] = dr["Foc"];
                        objArray[0, 18] = dr["BalanceQty"];
                        objArray[0, 19] = dr["NetKg"];
                        objArray[0, 20] = dr["WeightKg"];
                    }

                    worksheet.Range[string.Format("A{0}:U{0}", rownum)].Value2 = objArray;

                    rownum++;
                }

                #region Save & Show Excel
                string strExcelName = Class.MicrosoftFile.GetName("Shipping_P03_Detail");
                excel.ActiveWorkbook.SaveAs(strExcelName);
                excel.Quit();
                Marshal.ReleaseComObject(excel);
                Marshal.ReleaseComObject(worksheet);

                strExcelName.OpenFile();
                #endregion
            }
            else if (this.radioRollDyelot.Checked)
            {
                // 顯示筆數於PrintForm上Count欄位
                this.SetCount(this.printData.Rows.Count);

                if (this.printData.Rows.Count <= 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!");
                    return false;
                }

                string strXltName = Env.Cfg.XltPathDir + "\\Shipping_P03_RollDyelot.xltx";
                Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
                if (excel == null)
                {
                    return false;
                }

                Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

                bool result = MyUtility.Excel.CopyToXls(this.printData, string.Empty, xltfile: "Shipping_P03_RollDyelot.xltx", headerRow: 1, excelApp: excel, showSaveMsg: false, wSheet: worksheet);
                if (!result)
                {
                    MyUtility.Msg.WarningBox(result.ToString(), "Warning");
                }

                Marshal.ReleaseComObject(worksheet);
                Marshal.ReleaseComObject(excel);
            }
            else
            {
                // 顯示筆數於PrintForm上Count欄位
                this.SetCount(this.printData.Rows.Count);

                if (this.printData.Rows.Count <= 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!");
                    return false;
                }

                string strXltName = Env.Cfg.XltPathDir + "\\Shipping_P03_List.xltx";
                Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
                if (excel == null)
                {
                    return false;
                }

                Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];
                worksheet.Cells[2, 2] = (MyUtility.Check.Empty(this.eta1) ? string.Empty : this.eta1) + " ~ " + (MyUtility.Check.Empty(this.eta2) ? string.Empty : this.eta2);
                worksheet.Cells[2, 7] = MyUtility.Check.Empty(this.factory) ? "All" : this.factory;

                int rownum = 4;
                object[,] objArray = new object[1, 14];
                foreach (DataRow dr in this.printData.Rows)
                {
                    objArray[0, 0] = dr["ID"];
                    objArray[0, 1] = dr["shipmodeID"];
                    objArray[0, 2] = dr["Loading"];
                    objArray[0, 3] = dr["Eta"];
                    objArray[0, 4] = dr["Blno"];
                    objArray[0, 5] = dr["Vessel"];
                    objArray[0, 6] = dr["InvNo"];
                    objArray[0, 7] = dr["Payer"];
                    objArray[0, 8] = dr["PackingArrival"];
                    objArray[0, 9] = dr["PortArrival"];
                    objArray[0, 10] = dr["WhseArrival"];
                    objArray[0, 11] = dr["DocArrival"];
                    objArray[0, 12] = dr["Name"];
                    objArray[0, 13] = dr["ExtNo"];
                    worksheet.Range[string.Format("A{0}:N{0}", rownum)].Value2 = objArray;

                    rownum++;
                }

                excel.Cells.EntireColumn.AutoFit();
                excel.Cells.EntireRow.AutoFit();

                #region Save & Show Excel
                string strExcelName = Class.MicrosoftFile.GetName("Shipping_P03_List");
                excel.ActiveWorkbook.SaveAs(strExcelName);
                excel.Quit();
                Marshal.ReleaseComObject(excel);
                Marshal.ReleaseComObject(worksheet);

                strExcelName.OpenFile();
                #endregion
            }

            return true;
        }
    }
}
