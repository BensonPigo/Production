using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Packing
{
    /// <summary>
    /// Packing_Packing
    /// </summary>
    public partial class R02 : Sci.Win.Tems.PrintForm
    {
        /// <summary>
        /// R02
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R02(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();

            DataTable factory;
            DBProxy.Current.Select(null, "select '' union all select distinct FtyGroup from Factory WITH (NOLOCK) ", out factory);
            MyUtility.Tool.SetupCombox(this.comboFactory, 1, factory);
            this.comboFactory.Text = Sci.Env.User.Factory;
            this.txtMdivision1.Text = Sci.Env.User.Keyword;
        }

        private string _sp1;
        private string _sp2;
        private string _po1;
        private string _po2;
        private string _bdate1;
        private string _bdate2;
        private string _scidate1;
        private string _scidate2;
        private string _offdate1;
        private string _offdate2;
        private string _scanDate1;
        private string _scanDate2;
        private string _brand;
        private string _mDivision;
        private string _factory;
        private string _POCompletion;
        private bool _bulk;
        private bool _sample;
        private bool _garment;
        private DataTable[] _printData;

        /// <summary>
        /// ValidateInput
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ValidateInput()
        {
            this._sp1 = this.txtSPNoStart.Text;
            this._sp2 = this.txtSPNoEnd.Text;
            this._po1 = this.txtPONoStart.Text;
            this._po2 = this.txtPONoEnd.Text;

            if (!MyUtility.Check.Empty(this.dateBuyerDelivery.Value1))
            {
                this._bdate1 = Convert.ToDateTime(this.dateBuyerDelivery.Value1).ToString("d");
            }
            else
            {
                this._bdate1 = null;
            }

            if (!MyUtility.Check.Empty(this.dateBuyerDelivery.Value2))
            {
                this._bdate2 = Convert.ToDateTime(this.dateBuyerDelivery.Value2).ToString("d");
            }
            else
            {
                this._bdate2 = null;
            }

            if (!MyUtility.Check.Empty(this.dateSCIDelivery.Value1))
            {
                this._scidate1 = Convert.ToDateTime(this.dateSCIDelivery.Value1).ToString("d");
            }
            else
            {
                this._scidate1 = null;
            }

            if (!MyUtility.Check.Empty(this.dateSCIDelivery.Value2))
            {
                this._scidate2 = Convert.ToDateTime(this.dateSCIDelivery.Value2).ToString("d");
            }
            else
            {
                this._scidate2 = null;
            }

            if (!MyUtility.Check.Empty(this.dateOffline.Value1))
            {
                this._offdate1 = Convert.ToDateTime(this.dateOffline.Value1).ToString("d");
            }
            else
            {
                this._offdate1 = null;
            }

            if (!MyUtility.Check.Empty(this.dateOffline.Value2))
            {
                this._offdate2 = Convert.ToDateTime(this.dateOffline.Value2).ToString("d");
            }
            else
            {
                this._offdate2 = null;
            }

            if (!MyUtility.Check.Empty(this.dateScanDate.Value1))
            {
                this._scanDate1 = Convert.ToDateTime(this.dateScanDate.Value1).ToString("d");
            }
            else
            {
                this._scanDate1 = null;
            }

            if (!MyUtility.Check.Empty(this.dateScanDate.Value2))
            {
                this._scanDate2 = this.dateScanDate.Value2.Value.AddDays(1).AddSeconds(-1).ToString("yyyy/MM/dd hh:mm:ss");
            }
            else
            {
                this._scanDate2 = null;
            }

            this._brand = this.txtbrand.Text;
            this._mDivision = this.txtMdivision1.Text;
            this._factory = this.comboFactory.Text;
            this._POCompletion = this.cmbPOcompletion.Text;
            this._bulk = this.chkBulk.Checked;
            this._sample = this.chkSample.Checked;
            this._garment = this.chkGarment.Checked;

            return base.ValidateInput();
        }

        /// <summary>
        /// OnAsyncDataLoad
        /// </summary>
        /// <param name="e">e</param>
        /// <returns>DualResult</returns>
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            #region where
            string where = string.Empty;
            if (!MyUtility.Check.Empty(this._sp1))
            {
                where += $" and  o.id>= '{this._sp1}' ";
            }

            if (!MyUtility.Check.Empty(this._sp2))
            {
                where += $" and  o.id <= '{this._sp2}' ";
            }

            if (!MyUtility.Check.Empty(this._po1))
            {
                where += $" and o.CustPONo >= '{this._po1}' ";
            }

            if (!MyUtility.Check.Empty(this._po2))
            {
                where += $" and  o.CustPONo <= '{this._po2}' ";
            }

            if (!MyUtility.Check.Empty(this._bdate1))
            {
                where += $" and  o.BuyerDelivery >= '{this._bdate1}' ";
            }

            if (!MyUtility.Check.Empty(this._bdate2))
            {
                where += $" and  o.BuyerDelivery <= '{this._bdate2}' ";
            }

            if (!MyUtility.Check.Empty(this._scidate1))
            {
                where += $" and  o.SciDelivery >= '{this._scidate1}' ";
            }

            if (!MyUtility.Check.Empty(this._scidate2))
            {
                where += $" and  o.SciDelivery <= '{this._scidate2}' ";
            }

            if (!MyUtility.Check.Empty(this._offdate1))
            {
                where += $" and  o.SewOffLine >= '{this._offdate1}' ";
            }

            if (!MyUtility.Check.Empty(this._offdate2))
            {
                where += $" and  o.SewOffLine <= '{this._offdate2}' ";
            }

            if (!MyUtility.Check.Empty(this._scanDate1))
            {
                where += $" and  pld.ScanEditDate  >= '{this._scanDate1}' ";
            }

            if (!MyUtility.Check.Empty(this._scanDate2))
            {
                where += $" and  pld.ScanEditDate  <= '{this._scanDate2}' ";
            }

            if (!MyUtility.Check.Empty(this._brand))
            {
                where += $" and  o.BrandID= '{this._brand}' ";
            }

            if (!MyUtility.Check.Empty(this._mDivision))
            {
                where += $" and o.MDivisionID = '{this._mDivision}' ";
            }

            if (!MyUtility.Check.Empty(this._factory))
            {
                where += $" and o.FactoryID = '{this._factory}' ";
            }

            string having = string.Empty;
            if (this._POCompletion.EqualString("Complete"))
            {
                having += $"having iif(isnull(o.Qty,0)=0,CAST(0 AS FLOAT),sum(pld.ScanQty)/CAST(o.Qty AS FLOAT)) = 1 ";
            }
            else if (this._POCompletion.EqualString("InComplete"))
            {
                having += $"having iif(isnull(o.Qty,0)=0,CAST(0 AS FLOAT),sum(pld.ScanQty)/CAST(o.Qty AS FLOAT)) < 1 ";
            }

            List<string> category = new List<string>();
            if (this._bulk)
            {
                category.Add("'B'");
            }

            if (this._sample)
            {
                category.Add("'S'");
            }

            if (this._garment)
            {
                category.Add("'G'");
            }

            if (category.Count > 0)
            {
                where += $" and o.Category in ({string.Join(",", category)})";
            }
            else
            {
                where += $" and o.Category not in  ('B','S','G')";
            }

            where += this.chkIncludeCancelOrder.Checked ? string.Empty : " and o.Junk = 0 ";
            #endregion

            string sqlcmd = $@"
select
	o.MDivisionID,o.FactoryID,o.SewLine,o.CustPONo,o.ID,
	Junk=iif(o.Junk = 0,'N','Y'),
	o.StyleID,
	o.Category,
	VasShas=iif(o.VasShas = 0,'N','Y'),
	o.SeasonID,o.ProgramID,o.CdCodeID,o.Qty,o.CustCDID,o.Dest,o.SewInLine,o.SewOffLine,o.ShipModeList,o.SciDelivery,
	o.BuyerDelivery,
	ScanQty=sum(pld.ScanQty),
	BalanceQty=o.Qty-sum(pld.ScanQty),
	POCompletion=round(iif(isnull(o.Qty,0)=0,CAST(0 AS FLOAT),sum(pld.ScanQty)/CAST(o.Qty AS FLOAT)),2)
into #tmp
from Orders o
inner join PackingList_Detail pld on o.id = pld.OrderID
where 1=1
{where}
group by o.MDivisionID,o.FactoryID,o.SewLine,o.CustPONo,o.ID,o.Junk,o.StyleID,o.Category,o.VasShas,
o.SeasonID,o.ProgramID,o.CdCodeID,o.Qty,o.Dest,o.SewInLine,o.SewOffLine,o.ShipModeList,o.SciDelivery,
o.BuyerDelivery,o.CustCDID
{having}
order by o.CustPONo

select	MDivisionID,FactoryID,SewLine,CustPONo,ID,Junk,StyleID,Category,VasShas,
		SeasonID,ProgramID,CdCodeID,Qty,CustCDID,Dest,SewInLine,SewOffLine,ShipModeList,SciDelivery,
		BuyerDelivery,carton.TtlCtnQty,carton.TtlRemainCtnQty,ScanQty,BalanceQty,POCompletion
from #tmp t
outer apply( select [TtlCtnQty] = sum(p.CTNQty),[TtlRemainCtnQty] = sum(iif(p.ScanEditDate is null or p.Lacking = 1,p.CTNQty,0))
			 from PackingList_Detail p with (nolock)
			 where p.OrderID = t.ID
		  ) carton

select 
    o.SewLine,o.BuyerDelivery,
	o.CustPONo,o.id
    ,Junk
    ,o.StyleID,
    pld.Article,pld.SizeCode,pld.ID,pld.CTNStartNo,[CTN Barcode] = pld.ID+pld.CTNStartNo
    ,[Barcode] = isnull(c7.Barcode,'')
    ,[PC/CTN] = pld.QtyPerCTN
	,[Scanned Qty] = pld.ScanQty
	,[Balance Qty] = pld.ShipQty -pld.ScanQty	
	from #tmp o
inner join PackingList_Detail pld on o.id = pld.OrderID
outer apply(
	select Barcode = stuff((
		select iif(pld2.Barcode!='',  concat('/',pld2.Barcode),'')
		from PackingList_Detail pld2 with (nolock)
		where pld2.id = pld.ID and pld2.OrderID = pld.OrderID and pld2.CTNStartNo = pld.CTNStartNo
		order by pld2.id,pld2.OrderID,pld2.CTNStartNo
		for xml path('')
	),1,1,'')
)c7
order by o.CustPONo

drop table #tmp
";

            #region Get Data
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out this._printData);
            if (!result)
            {
                return result;
            }
            #endregion

            return Result.True;
        }

        /// <summary>
        /// OnToExcel
        /// </summary>
        /// <param name="report">report</param>
        /// <returns>bool</returns>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            #region check printData
            if (this._printData[0].Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found.");
                return false;
            }
            #endregion

            this.SetCount(this._printData[1].Rows.Count);
            this.ShowWaitMessage("Excel Processing");

            string excelName = "Packing_R02";
            Excel.Application excelApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + $"\\{excelName}.xltx");
            Excel.Worksheet worksheet = excelApp.Sheets[1];
            worksheet.Cells[2, 2] = this._sp1 + "~" + this._sp2;
            worksheet.Cells[2, 5] = this._po1 + "~" + this._po2;
            worksheet.Cells[2, 8] = this._bdate1 + "~" + this._bdate2;
            worksheet.Cells[2, 11] = this._scidate1 + "~" + this._scidate2;
            worksheet.Cells[2, 15] = this._offdate1 + "~" + this._offdate2;
            worksheet.Cells[2, 18] = this._scanDate1 + "~" + this._scanDate2;
            worksheet.Cells[2, 21] = this._brand;
            worksheet.Cells[2, 25] = this._mDivision;
            worksheet.Cells[2, 27] = this._factory;
            worksheet.Cells[2, 29] = this.cmbPOcompletion.Text;

            string strcategory = (this.chkBulk.Checked ? "Bulk," : string.Empty) + (this.chkSample.Checked ? "Sample," : string.Empty) + (this.chkGarment.Checked ? "Garment," : string.Empty);
            worksheet.Cells[2, 31] = strcategory.Substring(0, strcategory.Length - 1);

            MyUtility.Excel.CopyToXls(this._printData[0], string.Empty, $"{excelName}.xltx", 3, false, null, excelApp, wSheet: excelApp.Sheets[1]); // 將datatable copy to excel
            excelApp.DisplayAlerts = false;

            worksheet = excelApp.Sheets[2];
            worksheet.Cells[2, 2] = this._sp1 + "~" + this._sp2;
            worksheet.Cells[2, 5] = this._po1 + "~" + this._po2;
            worksheet.Cells[2, 8] = this._bdate1 + "~" + this._bdate2;
            worksheet.Cells[3, 2] = this._scidate1 + "~" + this._scidate2;
            worksheet.Cells[3, 5] = this._offdate1 + "~" + this._offdate2;
            worksheet.Cells[3, 9] = this._scanDate1 + "~" + this._scanDate2;
            worksheet.Cells[4, 2] = this._brand;
            worksheet.Cells[4, 4] = this._mDivision;
            worksheet.Cells[4, 7] = this._factory;
            worksheet.Cells[4, 9] = this.cmbPOcompletion.Text;
            worksheet.Cells[4, 11] = strcategory.Substring(0, strcategory.Length - 1);
            MyUtility.Excel.CopyToXls(this._printData[1], string.Empty, $"{excelName}.xltx", 5, false, null, excelApp, wSheet: excelApp.Sheets[2]);
            worksheet = excelApp.Sheets[1];
            worksheet.Columns.AutoFit();
            #region 釋放上面開啟過excel物件
            string strExcelName = Class.MicrosoftFile.GetName(excelName);
            Excel.Workbook workbook = excelApp.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            excelApp.Quit();
            if (excelApp != null)
            {
                Marshal.FinalReleaseComObject(excelApp);
            }
            #endregion
            this.HideWaitMessage();
            strExcelName.OpenFile();
            return true;
        }
    }
}
