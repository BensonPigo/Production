using Ict;
using Sci.Data;
using Sci.Production.Prg;
using System;
using System.Data;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Packing
{
    public partial class R04 : Win.Tems.PrintForm
    {
        private string summaryBy;
        private string sp1;
        private string sp2;
        private string packingno1;
        private string packingno2;
        private string po1;
        private string po2;
        private string brand;
        private string mDivision;
        private string factory;
        private string bdate1;
        private string bdate2;
        private DataTable _printData;

        /// <summary>
        /// R04
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R04(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();

            this.txtfactory.Text = Env.User.Factory;
            this.txtMdivision1.Text = Env.User.Keyword;
            this.cmbSummaryBy.SelectedIndex = 0;
        }

        /// <summary>
        /// ValidateInput
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ValidateInput()
        {
            this.summaryBy = this.cmbSummaryBy.Text;
            this.packingno1 = this.txtPackingStart.Text;
            this.packingno2 = this.txtPackingEnd.Text;
            this.sp1 = this.txtSPNoStart.Text;
            this.sp2 = this.txtSPNoEnd.Text;
            this.po1 = this.txtPONoStart.Text;
            this.po2 = this.txtPONoEnd.Text;

            if (!MyUtility.Check.Empty(this.dateBuyerDelivery.Value1))
            {
                this.bdate1 = Convert.ToDateTime(this.dateBuyerDelivery.Value1).ToString("yyyy/MM/dd");
            }
            else
            {
                this.bdate1 = null;
            }

            if (!MyUtility.Check.Empty(this.dateBuyerDelivery.Value2))
            {
                this.bdate2 = Convert.ToDateTime(this.dateBuyerDelivery.Value2).ToString("yyyy/MM/dd");
            }
            else
            {
                this.bdate2 = null;
            }

            this.brand = this.txtbrand.Text;
            this.mDivision = this.txtMdivision1.Text;
            this.factory = this.txtfactory.Text;

            return base.ValidateInput();
        }

        /// <summary>
        /// OnAsyncDataLoad
        /// </summary>
        /// <param name="e">e</param>
        /// <returns>DualResult</returns>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            string sqlcmd = string.Empty;
            string where = string.Empty;

            #region Where

            // SP#
            if (!MyUtility.Check.Empty(this.sp1) && !MyUtility.Check.Empty(this.sp2))
            {
                where += $" and p.OrderID Between '{this.sp1}' and '{this.sp2}' " + Environment.NewLine;
            }
            else if (!MyUtility.Check.Empty(this.sp1) && MyUtility.Check.Empty(this.sp2))
            {
                where += $" and p.OrderID >= '{this.sp1}' " + Environment.NewLine;
            }
            else if (MyUtility.Check.Empty(this.sp1) && !MyUtility.Check.Empty(this.sp2))
            {
                where += $" and p.OrderID <= '{this.sp2}' " + Environment.NewLine;
            }

            // PackingID
            if (!MyUtility.Check.Empty(this.packingno1) && !MyUtility.Check.Empty(this.packingno2))
            {
                where += $" and p.ID Between '{this.packingno1}' and '{this.packingno2}' " + Environment.NewLine;
            }
            else if (!MyUtility.Check.Empty(this.packingno1) && MyUtility.Check.Empty(this.packingno2))
            {
                where += $" and p.ID >= '{this.packingno1}' " + Environment.NewLine;
            }
            else if (MyUtility.Check.Empty(this.packingno1) && !MyUtility.Check.Empty(this.packingno2))
            {
                where += $" and p.ID <= '{this.packingno2}' " + Environment.NewLine;
            }

            // PO#
            if (!MyUtility.Check.Empty(this.po1) && !MyUtility.Check.Empty(this.po2))
            {
                where += $" and o.CustPONo Between '{this.po1}' and '{this.po2}' " + Environment.NewLine;
            }
            else if (!MyUtility.Check.Empty(this.po1) && MyUtility.Check.Empty(this.po2))
            {
                where += $" and o.CustPONo >= '{this.po1}' " + Environment.NewLine;
            }
            else if (MyUtility.Check.Empty(this.po1) && !MyUtility.Check.Empty(this.po2))
            {
                where += $" and o.CustPONo <= '{this.po2}' " + Environment.NewLine;
            }

            // BuyerDelivery
            if (!MyUtility.Check.Empty(this.bdate1) && !MyUtility.Check.Empty(this.bdate2))
            {
                where += $" and o.BuyerDelivery  Between '{this.bdate1}' and '{this.bdate2}' " + Environment.NewLine;
            }
            else if (!MyUtility.Check.Empty(this.bdate1) && MyUtility.Check.Empty(this.bdate2))
            {
                where += $" and o.BuyerDelivery  >= '{this.bdate1}' " + Environment.NewLine;
            }
            else if (MyUtility.Check.Empty(this.bdate1) && !MyUtility.Check.Empty(this.bdate2))
            {
                where += $" and o.BuyerDelivery  <= '{this.bdate2}' " + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(this.brand))
            {
                where += $" and o.BrandID = '{this.brand}' " + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(this.mDivision))
            {
                where += $" and o.MDivisionID = '{this.mDivision}' " + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(this.factory))
            {
                where += $" and p.FactoryID = '{this.factory}' " + Environment.NewLine;
            }
            #endregion

            if (this.summaryBy == "By Size")
            {
                sqlcmd = $@"
--By Size
select p.FactoryID
	,o.BuyerDelivery
	,p.id
	,p.OrderID
	,o.StyleID
	,o.SeasonID
	,Color = pd.Article + '-' + pd.Color
	,o.Customize1
	,o.CustPONo
	,o.CustCDID
	,Destination = c.NameEN
	,pd.SizeCode
	,CTNStartNo = (
						select CTNStartNo = Stuff((
								select ','+  pl.CTNStartNo 
								from PackingList_Detail pl
								where pl.ID=p.Id AND pl.SizeCode=pd.SizeCode AND pl.Article=pd.Article AND pl.Color=pd.Color
								order by pl.Seq
								for xml path('')
						),1,1,'')
					)
	,Qty = Sum(pld.QtyPerCTN)
	,li.Description
	,os.SizeGroup,os.Seq
from PackingGuide p with(nolock)
inner join PackingGuide_Detail pd with(nolock) on p.id = pd.Id
left join Order_QtyShip oq on oq.Id=p.OrderID and oq.Seq=p.OrderShipmodeSeq
left join Orders o ON o.ID=p.OrderID
left join Order_SizeCode os on os.Id=o.POID and os.SizeCode=pd.SizeCode
left join Country c ON c.ID=o.Dest
left join Order_SizeCode oz ON oz.ID = o.POID and oz.SizeCode=pd.SizeCode
left join PackingList_Detail pld on pld.ID=p.Id AND pld.SizeCode=pd.SizeCode AND pld.Article=pd.Article AND pld.Color=pd.Color
left join LocalItem li on li.RefNo= pd.RefNo
where 1=1 
{where}
    
group by p.FactoryID,o.BuyerDelivery,p.id,p.OrderID,o.StyleID,o.SeasonID,pd.Article,pd.Color,o.Customize1,o.CustPONo,o.CustCDID,c.NameEN,pd.SizeCode,li.Description
,os.SizeGroup,os.Seq
order by p.FactoryID,p.id,os.SizeGroup,os.Seq

";
            }
            else if (this.summaryBy == "By Carton#")
            {
                sqlcmd = $@"
--By Carton# 
select p.FactoryID
	,o.BuyerDelivery
	,p.id
	,p.OrderID
	,o.StyleID
	,o.SeasonID
	,Color = pd.Article + '-' + pd.Color
	,o.Customize1
	,o.CustPONo
	,o.CustCDID
	,Destination = c.NameEN
	,pd.SizeCode /*= (
						select SizeCode = Stuff((
								select DISTINCT ','+  pl.SizeCode 
								from PackingList_Detail pl
								where pl.ID=p.Id AND pl.CTNStartNo = pld.CTNStartNo AND pl.Article=pd.Article AND pl.Color=pd.Color
								for xml path('')
						),1,1,'')
					)*/
	,pld.CTNStartNo
	,Qty = Sum(pld.QtyPerCTN)
	,li.Description
    ,oq.Seq
from PackingGuide p with(nolock)
inner join PackingGuide_Detail pd with(nolock) on p.id = pd.Id
left join Order_QtyShip oq on oq.Id=p.OrderID and oq.Seq=p.OrderShipmodeSeq
left join Orders o ON o.ID=p.OrderID
left join Country c ON c.ID=o.Dest
left join Order_SizeCode oz ON oz.ID = o.POID and oz.SizeCode=pd.SizeCode
left join PackingList_Detail pld on pld.ID=p.Id AND pld.SizeCode=pd.SizeCode AND pld.Article=pd.Article AND pld.Color=pd.Color
left join LocalItem li on li.RefNo= pd.RefNo
where 1=1 --AND pld.CTNQty=1
{where}

group by p.FactoryID,o.BuyerDelivery,p.id,p.OrderID,o.StyleID,o.SeasonID,pd.Article,pd.Color,o.Customize1,o.CustPONo,o.CustCDID,c.NameEN,pd.SizeCode,pld.CTNStartNo,li.Description
,oq.Seq
order by p.FactoryID,p.id,LEN(pld.CTNStartNo), pld.CTNStartNo,oq.Seq
";
            }

            DualResult result = DBProxy.Current.Select(null, sqlcmd, out this._printData);
            if (!result)
            {
                return Ict.Result.F(result.ToString());
            }

            if (this._printData.Rows.Count != 0)
            {

                if (this.summaryBy == "By Size")
                {
                    this._printData.Columns.Remove("SizeGroup");
                    this._printData.Columns.Remove("Seq");
                }
                else if (this.summaryBy == "By Carton#")
                {
                    this._printData.Columns.Remove("Seq");
                }
            }
            return Ict.Result.True;
        }

        /// <summary>
        /// OnToExcel
        /// </summary>
        /// <param name="report">report</param>
        /// <returns>bool</returns>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            #region check printData
            if (this._printData.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found.");
                return false;
            }
            #endregion
            this.SetCount(this._printData.Rows.Count);
            this.ShowWaitMessage("Excel Processing");

            #region To Excel
            string excelName = "Packing_R04";
            Excel.Application excelApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + $"\\{excelName}.xltx");
            ExcelPrg.CopyToXlsAutoSplitSheet(this._printData, string.Empty, $"{excelName}.xltx", 1, false, null, excelApp, wSheet: excelApp.Sheets[1]);

            Excel.Worksheet worksheet = excelApp.ActiveWorkbook.Worksheets[1];
            worksheet.Columns.AutoFit();

            #endregion
            #region 釋放上面開啟過excel物件
            string strExcelName = Class.MicrosoftFile.GetName(excelName);
            Excel.Workbook workbook = excelApp.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            excelApp.Quit();

            if (worksheet != null)
            {
                Marshal.FinalReleaseComObject(worksheet);
            }

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
