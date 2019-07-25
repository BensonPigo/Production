using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;

namespace Sci.Production.Logistic
{
    /// <summary>
    /// Logistic_R01
    /// </summary>
    public partial class R01 : Sci.Win.Tems.PrintForm
    {
        private DateTime? buyerDelivery1;
        private DateTime? buyerDelivery2;
        private DateTime? sciDelivery1;
        private DateTime? sciDelivery2;
        private string mDivision;
        private string brand;
        private DataTable printData;

        /// <summary>
        /// get,set SciDelivery1
        /// </summary>
        public DateTime? SciDelivery1
        {
            get
            {
                return this.sciDelivery1;
            }

            set
            {
                this.sciDelivery1 = value;
            }
        }

        /// <summary>
        /// R01
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public R01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            DataTable mDivision;
            DBProxy.Current.Select(null, "select '' as ID union all select ID from MDivision WITH (NOLOCK) ", out mDivision);
            MyUtility.Tool.SetupCombox(this.comboM, 1, mDivision);
            this.comboM.Text = Sci.Env.User.Keyword;
        }

        /// <summary>
        /// 驗證輸入條件
        /// </summary>
        /// <returns>base.ValidateInput()</returns>
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(this.dateBuyerDelivery.Value1) && MyUtility.Check.Empty(this.dateSCIDelivery.Value1))
            {
                MyUtility.Msg.WarningBox("Buyer Delivery or SCI Delivery can't be empty!!");
                return false;
            }

            this.buyerDelivery1 = this.dateBuyerDelivery.Value1;
            this.buyerDelivery2 = this.dateBuyerDelivery.Value2;
            this.SciDelivery1 = this.dateSCIDelivery.Value1;
            this.sciDelivery2 = this.dateSCIDelivery.Value2;
            this.mDivision = this.comboM.Text;
            this.brand = this.txtbrand.Text;

            return base.ValidateInput();
        }

        /// <summary>
        /// 非同步取資料
        /// </summary>
        /// <param name="e">Win.ReportEventArgs</param>
        /// <returns>Result</returns>
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(@"
select  o.FactoryID
        , o.MCHandle
        , o.SewLine
        , o.ID
        , o.BrandId
        , o.StyleID
        , s.StyleName
        , o.CustPONo
        , o.Customize1
		, o.Junk
        , oq.BuyerDelivery
        , oq.ShipmodeID
		, oq.Seq
        , o.SciDelivery
        , o.TotalCTN
		, o.DRYCTN
        , o.PackErrCTN
        , o.ClogCTN
		, o.CfaCTN
        , o.PulloutCTNQty
into #tmp_Orders
from Orders o WITH (NOLOCK) 
inner join Order_QtyShip oq WITH (NOLOCK) on o.ID = oq.Id
inner join Style s  WITH (NOLOCK) on o.StyleUkey = s.Ukey
where o.Category = 'B'");

            if (!MyUtility.Check.Empty(this.buyerDelivery1))
            {
                sqlCmd.Append(string.Format(" and oq.BuyerDelivery >= '{0}'", Convert.ToDateTime(this.buyerDelivery1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.buyerDelivery2))
            {
                sqlCmd.Append(string.Format(" and oq.BuyerDelivery <= '{0}'", Convert.ToDateTime(this.buyerDelivery2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.SciDelivery1))
            {
                sqlCmd.Append(string.Format(" and o.SciDelivery >= '{0}'", Convert.ToDateTime(this.SciDelivery1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.sciDelivery2))
            {
                sqlCmd.Append(string.Format(" and o.SciDelivery <= '{0}'", Convert.ToDateTime(this.sciDelivery2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.brand))
            {
                sqlCmd.Append(string.Format(" and o.BrandID = '{0}'", this.brand));
            }

            if (!MyUtility.Check.Empty(this.mDivision))
            {
                sqlCmd.Append(string.Format(" and o.MDivisionID = '{0}'", this.mDivision));
            }

            sqlCmd.Append(@"

select distinct pd.OrderID,pd.OrderShipmodeSeq,pd.ClogLocationId
into #tmp_ClogLocationId
from PackingList_Detail pd WITH (NOLOCK) 
inner join #tmp_Orders o on pd.OrderID = o.ID  and pd.OrderShipmodeSeq = o.Seq
where ClogLocationId !='' 
and ClogLocationId is not null and pd.DisposeFromClog= 0  

select pd.OrderID,pd.OrderShipmodeSeq,sum(CTNQty) CTNQty, sum(ShipQty) GMTQty
into #tmp_CTNQty
from PackingList_Detail pd WITH (NOLOCK) 
inner join #tmp_Orders o on pd.OrderID = o.ID  and pd.OrderShipmodeSeq = o.Seq 
where pd.DisposeFromClog= 0
group by pd.OrderID,pd.OrderShipmodeSeq

select pd.OrderID,pd.OrderShipmodeSeq,sum(CTNQty) ClogQty, sum(ShipQty) ClogGMTQty
into #tmp_ClogQty
from PackingList_Detail pd WITH (NOLOCK) 
inner join #tmp_Orders o on pd.OrderID = o.ID  and pd.OrderShipmodeSeq = o.Seq 
where pd.ReceiveDate is not null and pd.DisposeFromClog= 0
group by pd.OrderID,pd.OrderShipmodeSeq
 
select  pd.OrderID,pd.OrderShipmodeSeq,sum (pd.CTNQty) PullQty
into #tmp_PullQty
from PackingList p WITH (NOLOCK) 
inner join PackingList_Detail pd WITH (NOLOCK) on p.ID = pd.ID 
inner join #tmp_Orders o on pd.OrderID = o.ID  and pd.OrderShipmodeSeq = o.Seq  
where p.PulloutID != '' and pd.DisposeFromClog= 0
group by pd.OrderID,pd.OrderShipmodeSeq

select pd.OrderID,sum(ShipQty) TtlGMTQty
into #tmp_TtlGMTQty
from PackingList_Detail pd WITH (NOLOCK) 
inner join (select distinct id from #tmp_Orders) o on pd.OrderID = o.ID   
where pd.DisposeFromClog= 0
group by pd.OrderID
 
select pd.OrderID,sum(ShipQty) TtlClogGMTQty
into #tmp_TtlClogGMTQty
from PackingList_Detail pd WITH (NOLOCK) 
inner join (select distinct id from #tmp_Orders) o on pd.OrderID = o.ID   
where ReceiveDate is not null and pd.DisposeFromClog= 0
group by pd.OrderID

select pd.OrderID,sum(ShipQty) TtlPullGMTQty
into #tmp_TtlPullGMTQty
from Pullout p WITH (NOLOCK) 
inner join Pullout_Detail pd WITH (NOLOCK) on pd.ID = p.ID 
inner join (select distinct id from #tmp_Orders) o on pd.OrderID = o.ID  
where p.Status <> 'New'
group by pd.OrderID
 
select pd.OrderID,pd.OrderShipmodeSeq,sum(ShipQty) PullGMTQty
into #tmp_PullGMTQty
from Pullout p
inner join Pullout_Detail pd WITH (NOLOCK) on pd.ID = p.ID 
inner join #tmp_Orders o on pd.OrderID = o.ID and pd.OrderShipmodeSeq = o.Seq 
where p.Status <> 'New'
group by pd.OrderID,pd.OrderShipmodeSeq



select o.*,
	 Location = stuff ((select concat(',',ClogLocationId)  from #tmp_ClogLocationId
				where   OrderID = o.ID 
                and OrderShipmodeSeq = o.Seq
				order by ClogLocationId
							for xml path('')
					) ,1,1,'') 
	,CTNQty = isnull(ctn.CTNQty, 0)
	,ClogQty = isnull(clog.ClogQty,0)
	,PullQty = isnull(pull.PullQty,0)
	,TtlGMTQty = isnull(ttlg.TtlGMTQty,0)
	,TtlClogGMTQty = isnull(ttlc.TtlClogGMTQty,0)
	,TtlPullGMTQty = isnull(ttlp.TtlPullGMTQty,0)
	,GMTQty = isnull(ctn.GMTQty,0)
	,ClogGMTQty = isnull(clog.ClogGMTQty,0)
	,PullGMTQty = isnull(pullG.PullGMTQty,0)
into #tmp 
from #tmp_Orders o
left join #tmp_CTNQty ctn on o.ID = ctn.OrderID and o.Seq = ctn.OrderShipmodeSeq
left join #tmp_ClogQty clog on o.ID = clog.OrderID and o.Seq = clog.OrderShipmodeSeq
left join #tmp_PullQty pull on o.ID = pull.OrderID and o.Seq = pull.OrderShipmodeSeq
left join #tmp_TtlGMTQty ttlg on o.ID = ttlg.OrderID 
left join #tmp_TtlClogGMTQty ttlc on o.ID = ttlc.OrderID
left join #tmp_TtlPullGMTQty ttlp on o.ID = ttlp.OrderID 
left join #tmp_PullGMTQty pullG on o.ID = pullG.OrderID and o.Seq = pullG.OrderShipmodeSeq 

drop table #tmp_Orders,#tmp_ClogLocationId,#tmp_CTNQty,#tmp_ClogQty,#tmp_PullQty,#tmp_TtlGMTQty,#tmp_TtlClogGMTQty,#tmp_TtlPullGMTQty,#tmp_PullGMTQty

select t.ID,RetCtnBySP = count(cr.ID)
into #tmp2
from #tmp t left join ClogReturn cr on cr.OrderID = t.ID
group by t.ID

select 
     t.FactoryID
    ,t.MCHandle
    ,t.SewLine
    ,t.ID
    ,t.BrandID
    ,t.StyleID
    ,t.StyleName
    ,t.CustPONo
    ,t.Customize1
    ,t.SciDelivery
    ,[Junk]= IIF(t.Junk=1,'Y','')
    ,t.BuyerDelivery
    ,t.ShipmodeID
    ,t.Location
	,t.TotalCTN,DRYCTN=isnull(t.DRYCTN,0),PackErrCTN = isnull(t.PackErrCTN,0),ClogCTN=isnull(t.ClogCTN,0),CfaCTN=isnull(t.CfaCTN,0),t2.RetCtnBySP
	,[Bal Ctn by SP#]=isnull(t.TotalCTN,0)-isnull(t.ClogCTN,0) -isnull(t.DRYCTN,0) -isnull(t.CfaCTN,0) - isnull(t.PackErrCTN,0)
	,[% by SP#]=iif(isnull(t.TtlGMTQty,0)=0,0,Round(1-((t.TtlGMTQty-isnull(t.TtlClogGMTQty,0))/t.TtlGMTQty),2)*100)
	,[Ctn SDP by SP#]=iif(isnull(t.TotalCTN,0)=0, 0,ROUND(isnull(t.ClogCTN,0)/t.TotalCTN,2)*100)
	,t.PulloutCTNQty,t.TtlGMTQty,t.TtlClogGMTQty
	,[Bal Qty by SP#] = isnull(t.TtlGMTQty,0)-isnull(t.TtlClogGMTQty,0)
	,[Qty SDP by SP#]=iiF(isnull(t.TtlGMTQty,0)=0,0,ROUND(isnull(t.TtlClogGMTQty,0)/t.TtlGMTQty,2)*100)
	,t.TtlPullGMTQty,t.CTNQty,t.ClogQty
	,[Bal Ctn by Shipmode]=isnull(t.CTNQty,0)-isnull(t.ClogQty,0)
	,[Ctn SDP by Shipmode]=iiF(isnull(t.CTNQty,0)=0,0,ROUND(isnull(t.ClogQty,0)/t.CTNQty,2)*100)
	,t.PullQty
	,[CTN in CLOG]=isnull(t.ClogQty,0)-isnull(t.PullQty,0)
	,t.GMTQty,t.ClogGMTQty
	,[Bal Qty by Shipmode]=isnull(t.GMTQty,0)-isnull(t.PullGMTQty,0)
	,[Qty SDP by Shipmode]=iif(isnull(t.GMTQty,0)=0,0,ROUND(isnull(t.ClogGMTQty,0)/t.GMTQty,2)*100)
	,t.PullGMTQty
from #tmp t,#tmp2 t2
where t.id = t2.ID
order by t.FactoryID,t.ID,t.BuyerDelivery

drop table #tmp,#tmp2
                
");

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out this.printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Result.True;
        }

        /// <summary>
        /// 產生Excel
        /// </summary>
        /// <param name="report">Win.ReportDefinition</param>
        /// <returns>bool</returns>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData.Rows.Count);

            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            this.ShowWaitMessage("Starting EXCEL...");
            string strXltName = Sci.Env.Cfg.XltPathDir + "\\Logistic_R01_CartonStatusReport.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return false;
            }

            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            worksheet.Cells[2, 1] = string.Format(
                "Buyer Delivery: {0} ~ {1}             SCI Delivery: {2} ~ {3}             M: {4}             Brand: {5}",
                MyUtility.Check.Empty(this.buyerDelivery1) ? string.Empty : Convert.ToDateTime(this.buyerDelivery1).ToString("d"),
                MyUtility.Check.Empty(this.buyerDelivery2) ? string.Empty : Convert.ToDateTime(this.buyerDelivery2).ToString("d"),
                MyUtility.Check.Empty(this.SciDelivery1) ? string.Empty : Convert.ToDateTime(this.SciDelivery1).ToString("d"),
                MyUtility.Check.Empty(this.sciDelivery2) ? string.Empty : Convert.ToDateTime(this.sciDelivery2).ToString("d"),
                this.mDivision,
                this.brand);

            MyUtility.Excel.CopyToXls(this.printData, string.Empty, "Logistic_R01_CartonStatusReport.xltx", 3, false, null, excel);// 將datatable copy to excel
            excel.Cells.EntireColumn.AutoFit();
            excel.Cells.EntireRow.AutoFit();

            #region Save & show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Logistic_R01_CartonStatusReport");
            Microsoft.Office.Interop.Excel.Workbook workbook = excel.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(worksheet);
            Marshal.ReleaseComObject(excel);

            strExcelName.OpenFile();
            #endregion
            this.HideWaitMessage();
            return true;
        }
    }
}
