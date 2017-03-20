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

namespace Sci.Production.Planning
{
    public partial class R15 : Sci.Win.Tems.PrintForm
    {
        int selectindex = 0;
        string factory, mdivision, orderby, spno1, spno2, custcd, brandid;
        DateTime? sciDelivery1, sciDelivery2, CustRqsDate1, CustRqsDate2, BuyerDelivery1, BuyerDelivery2
            , CutOffDate1, CutOffDate2, planDate1, planDate2;
        DataTable printData, dtArtworkType;
        StringBuilder artworktypes = new StringBuilder();
        bool isArtwork;
        public R15(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            txtMdivision1.Text = Sci.Env.User.Keyword;
            txtfactory1.Text = Sci.Env.User.Factory;
            cbxCategory.SelectedIndex = 1;  //Bulk
            MyUtility.Tool.SetupCombox(cbxOrderBy, 2, 1, "orderid,SPNO,brandid,Brand");
            cbxOrderBy.SelectedIndex = 0;
            dateRangeBuyerDelivery.Select();
            dateRangeBuyerDelivery.Value1 = DateTime.Now;
            dateRangeBuyerDelivery.Value2 = DateTime.Now.AddDays(30);
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(dateRangeSciDelivery.Value1) &&
                MyUtility.Check.Empty(dateRangeCustRqsDate.Value1) &&
                MyUtility.Check.Empty(dateRangeBuyerDelivery.Value1) &&
                MyUtility.Check.Empty(dateRangeCutOffDate.Value1) &&
                MyUtility.Check.Empty(dateRangePlanDate.Value1) &&
                (MyUtility.Check.Empty(txtSpno1.Text) || MyUtility.Check.Empty(txtSpno2.Text)))
            {
                MyUtility.Msg.WarningBox("< Buyer Delivery > & < SCI Delivery > & < Cust RQS Date > & < Cut Off Date > & < Plan Date > & < SP# > can't be empty!!");
                return false;
            }

            #region -- 擇一必輸的條件 --
            sciDelivery1 = dateRangeSciDelivery.Value1;
            sciDelivery2 = dateRangeSciDelivery.Value2;
            CustRqsDate1 = dateRangeCustRqsDate.Value1;
            CustRqsDate2 = dateRangeCustRqsDate.Value2;
            BuyerDelivery1 = dateRangeBuyerDelivery.Value1;
            BuyerDelivery2 = dateRangeBuyerDelivery.Value2;
            CutOffDate1 = dateRangeCutOffDate.Value1;
            CutOffDate2 = dateRangeCutOffDate.Value2;
            planDate1 = dateRangePlanDate.Value1;
            planDate2 = dateRangePlanDate.Value2;
            spno1 = txtSpno1.Text;
            spno2 = txtSpno2.Text;
            #endregion
            brandid = txtbrand1.Text;
            custcd = txtcustcd1.Text;
            mdivision = txtMdivision1.Text;
            factory = txtfactory1.Text;
            selectindex = cbxCategory.SelectedIndex;
            orderby = cbxOrderBy.SelectedValue.ToString();
            isArtwork = chkbArtowk.Checked;
            if (isArtwork)
            {
                DualResult result;
                if (!(result = DBProxy.Current.Select("", "select id from dbo.artworktype WITH (NOLOCK) where istms=1 or isprice= 1 order by seq", out dtArtworkType)))
                {
                    MyUtility.Msg.WarningBox(result.ToString());
                    return false;
                }

                if (dtArtworkType.Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox("Artwork Type data not found, Please inform MIS to check !");
                    return false;
                }

                artworktypes.Clear();
                for (int i = 0; i < dtArtworkType.Rows.Count; i++)
                {
                    artworktypes.Append(string.Format(@"[{0}],", dtArtworkType.Rows[i]["id"].ToString()));
                }
            }

            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
            StringBuilder sqlCmd = new StringBuilder();

            sqlCmd.Append(string.Format(@"
                select o.MDivisionID,o.FactoryID,S.BuyerDelivery,o.SciDelivery,O.CRDDate,O.CFMDate
                ,O.ID OrderID,O.Dest,O.StyleID,O.SeasonID,O.ProjectID,O.Customize1,O.BuyMonth
                ,O.CustPONo,O.BrandID,O.CustCDID,O.ProgramID,O.CdCodeID,O.CPU,O.Qty,S.Qty [Qty_byShip]
                ,O.FOCQty,O.PoPrice,O.CMPPrice,O.KPILETA,O.LETA,O.MTLETA,O.SewETA
                ,O.PackETA,O.MTLComplete,O.SewInLine,O.SewOffLine,O.CutInLine,O.CutOffLine
                ,O.Category,O.IsForecast,O.PulloutDate,O.ActPulloutDate,O.SMR,O.MRHandle
                ,O.MCHandle,O.OrigBuyerDelivery,O.DoxType
                ,O.TotalCTN,O.FtyCTN,O.ClogCTN
                ,O.VasShas,O.TissuePaper,O.MTLExport,O.SewLine,O.ShipModeList,s.ShipmodeID,O.PlanDate
                ,O.FirstProduction,O.Finished,O.FtyGroup,O.OrderTypeID,O.SpecialMark,O.GFR
                ,O.SampleReason,O.InspDate,O.MnorderApv,O.FtyKPI,O.KPIChangeReason
                ,O.StyleUkey
                ,O.POID
                into #cte 
                from dbo.Orders o WITH (NOLOCK) inner join Order_QtyShip s WITH (NOLOCK) on s.id = o.ID
                WHERE 1=1 
                "));

            #region --- 條件組合  ---
            if (!MyUtility.Check.Empty(sciDelivery1))
            {
                sqlCmd.Append(string.Format(@" and o.SciDelivery >= '{0}'", Convert.ToDateTime(sciDelivery1).ToString("d")));
            }
            if (!MyUtility.Check.Empty(sciDelivery2))
            {
                sqlCmd.Append(string.Format(@" and o.SciDelivery <= '{0}'", Convert.ToDateTime(sciDelivery2).ToString("d")));
            }
            if (!MyUtility.Check.Empty(BuyerDelivery1))
            {
                sqlCmd.Append(string.Format(@" and s.BuyerDelivery >= '{0}'", Convert.ToDateTime(BuyerDelivery1).ToString("d")));
            }
            if (!MyUtility.Check.Empty(BuyerDelivery2))
            {
                sqlCmd.Append(string.Format(@" and s.BuyerDelivery <= '{0}'", Convert.ToDateTime(BuyerDelivery2).ToString("d")));
            }
            if (!MyUtility.Check.Empty(CustRqsDate1))
            {
                sqlCmd.Append(string.Format(@" and o.CRDDate >= '{0}'", Convert.ToDateTime(CustRqsDate1).ToString("d")));
            }
            if (!MyUtility.Check.Empty(CustRqsDate2))
            {
                sqlCmd.Append(string.Format(@" and o.CRDDate <= '{0}'", Convert.ToDateTime(CustRqsDate2).ToString("d")));
            }
            if (!MyUtility.Check.Empty(CutOffDate1))
            {
                sqlCmd.Append(string.Format(@" and o.SDPDate >= '{0}'", Convert.ToDateTime(CutOffDate1).ToString("d")));
            }
            if (!MyUtility.Check.Empty(CutOffDate2))
            {
                sqlCmd.Append(string.Format(@" and o.SDPDate <= '{0}'", Convert.ToDateTime(CutOffDate2).ToString("d")));
            }
            if (!MyUtility.Check.Empty(planDate1))
            {
                sqlCmd.Append(string.Format(@" and o.PlanDate >= '{0}'", Convert.ToDateTime(planDate1).ToString("d")));
            }
            if (!MyUtility.Check.Empty(planDate2))
            {
                sqlCmd.Append(string.Format(@" and o.PlanDate <= '{0}'", Convert.ToDateTime(planDate2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(spno1))
            {
                sqlCmd.Append(" and o.id >= @spno1 ");
                cmds.Add(new System.Data.SqlClient.SqlParameter("@spno1", spno1));
            }
            if (!MyUtility.Check.Empty(spno2))
            {
                sqlCmd.Append(" and o.id <= @spno2 ");
                cmds.Add(new System.Data.SqlClient.SqlParameter("@spno2", spno2));
            }

            if (!MyUtility.Check.Empty(brandid))
            {
                sqlCmd.Append(string.Format(@" and o.brandid = @brandid"));
                cmds.Add(new System.Data.SqlClient.SqlParameter("@brandid", brandid));
            }

            if (!MyUtility.Check.Empty(custcd))
            {
                sqlCmd.Append(string.Format(@" and o.CustCDID = @custcd"));
                cmds.Add(new System.Data.SqlClient.SqlParameter("@custcd", custcd));
            }

            if (!MyUtility.Check.Empty(mdivision))
            {
                sqlCmd.Append(" and o.mdivisionid = @MDivision");
                cmds.Add(new System.Data.SqlClient.SqlParameter("@MDivision", mdivision));
            }

            if (!MyUtility.Check.Empty(factory))
            {
                sqlCmd.Append(" and o.factoryid = @factory");
                cmds.Add(new System.Data.SqlClient.SqlParameter("@factory", factory));
            }

            switch (selectindex)
            {
                case 0:
                    sqlCmd.Append(@" and (o.Category = 'B' or o.Category = 'S')");
                    break;
                case 1:
                    sqlCmd.Append(@" and o.Category = 'B' ");
                    break;
                case 2:
                    sqlCmd.Append(@" and (o.Category = 'S')");
                    break;
            }

            #endregion

            #region -- 有列印Artwork --
            if (isArtwork)
            {
                sqlCmd.Append(@"
                --依取得的訂單資料取得訂單的 TMS Cost
                select aa.orderid,bb.ArtworkTypeID,iif(cc.IsTMS=1,bb.tms,bb.price) price_tms 
                into #rawdata_tmscost
                from #cte aa 
                inner join dbo.Order_TmsCost bb on bb.id = aa.orderid
                inner join dbo.ArtworkType cc on cc.id = bb.ArtworkTypeID
                where IsTMS =1 or IsPrice = 1
                ");

                sqlCmd.Append(string.Format(@"--將取得Tms Cost做成樞紐表
                select * 
                into #tmscost_pvt
                from #rawdata_tmscost
                pivot
                (
                    sum(price_tms)
                    for artworktypeid in ( {0})
                )as pvt ", artworktypes.ToString().Substring(0, artworktypes.ToString().Length - 1)));
            }
            #endregion

            sqlCmd.Append(string.Format(@"
                -- 依撈出來的order資料(cte)去找各製程的WIP
                select 
                 t.OrderID
                 ,(SELECT SUM(CWIP.Qty) FROM DBO.CuttingOutput_WIP CWIP WITH (NOLOCK) WHERE CWIP.OrderID = T.OrderID) cut_qty
                 ,(SELECT MIN(a.cDate) from dbo.CuttingOutput a WITH (NOLOCK) 
	                 inner join dbo.CuttingOutput_Detail b WITH (NOLOCK) on b.id = a.id 
	                 inner join dbo.WorkOrder_Distribute c WITH (NOLOCK) on c.WorkOrderUkey = b.WorkOrderUkey
	                 where c.OrderID = t.OrderID) first_cut_date
                ,(select MIN(isnull(tt.qaqty,0)) from dbo.style_location sl WITH (NOLOCK) left join 
	                (SELECT b.ComboType,sum(b.QAQty) qaqty FROM DBO.SewingOutput a WITH (NOLOCK) 
	                inner join dbo.SewingOutput_Detail b WITH (NOLOCK) on b.ID = a.ID
	                where b.OrderId = t.OrderID
	                group by ComboType ) tt on tt.ComboType = sl.Location
	                where sl.StyleUkey = t.StyleUkey) sewing_output

                ,t.StyleUkey
                ,(select min(qty) qty from (
                select sum(b.Qty) qty ,c.PatternCode,c.ArtworkID 
                from dbo.farmin a WITH (NOLOCK) inner join dbo.FarmIn_Detail b WITH (NOLOCK) on b.id = a.id 
                right join (select distinct v.ArtworkTypeID, v.Article,v.ArtworkID,v.PatternCode 
                from dbo.View_Order_Artworks v 
                where v.ID=t.OrderID) c on c.ArtworkTypeID = a.ArtworkTypeId 
                and c.PatternCode = b.PatternCode 
                and c.ArtworkID = b.ArtworkID
                where a.ArtworkTypeId='EMBROIDERY' 
	                and b.Orderid = t.OrderID
	                group by c.PatternCode,c.ArtworkID) x) EMBROIDERY_qty

                ,(select min(qty) qty from (
                select sum(b.Qty) qty ,c.PatternCode,c.ArtworkID 
                from dbo.farmin a WITH (NOLOCK) inner join dbo.FarmIn_Detail b WITH (NOLOCK) on b.id = a.id 
                right join (select distinct v.ArtworkTypeID,v.ArtworkID,v.PatternCode 
                from dbo.View_Order_Artworks v where v.ID=t.OrderID) c 
                on c.ArtworkTypeID = a.ArtworkTypeId 
                and c.PatternCode = b.PatternCode 
                and c.ArtworkID = b.ArtworkID
                where a.ArtworkTypeId='BONDING' 
	                and b.Orderid = t.OrderID
	                group by c.PatternCode,c.ArtworkID) x) BONDING_qty

                ,(select min(qty) qty from (
                select sum(b.Qty) qty ,c.PatternCode,c.ArtworkID 
                from dbo.farmin a WITH (NOLOCK) inner join dbo.FarmIn_Detail b WITH (NOLOCK) on b.id = a.id 
                right join (select distinct v.ArtworkTypeID,v.ArtworkID,v.PatternCode 
                from dbo.View_Order_Artworks v 
                where v.ID=t.OrderID) c on c.ArtworkTypeID = a.ArtworkTypeId 
                and c.PatternCode = b.PatternCode 
                and c.ArtworkID = b.ArtworkID
                where a.ArtworkTypeId='PRINTING' 
	                and b.Orderid = t.OrderID
	                group by c.PatternCode,c.ArtworkID) x) PRINTING_qty
                ,SEWOUTPUT.*
                into #cte2
                from #cte t
                outer apply (SELECT min(X.OutputDate) firstSewingDate, max(X.OutputDate) lastestSewingDate
                ,sum(X.QAQty) QAQTY ,AVG(X.QAQTY) AVG_QAQTY
                from (SELECT a.OutputDate,sum(a.QAQty) QAQty FROM DBO.SewingOutput a WITH (NOLOCK) 
                inner join dbo.SewingOutput_Detail b WITH (NOLOCK) on b.ID = a.ID
                where b.OrderId = t.OrderID group by a.OutputDate ) X) SEWOUTPUT

                select t.MDivisionID,t.FactoryID,t.SewLine,t.BuyerDelivery,t.SewInLine,t.SewOffLine
                ,t.BrandID,t.OrderID
                ,t.Dest,t.StyleID,t.OrderTypeID
                ,t.ShipmodeID,'' [OrderNo],t.CustPONo,t.CustCDID,t.ProgramID,t.CdCodeID,t.KPILETA
                ,t.LETA,t.MTLETA,t.SewETA,t.PackETA,t.CPU
                ,t.Qty_byShip,#cte2.first_cut_date,#cte2.cut_qty,#cte2.EMBROIDERY_qty,#cte2.BONDING_qty
                ,#cte2.PRINTING_qty,#cte2.sewing_output,t.qty+t.FOCQty - #cte2.sewing_output [Balance]
                ,#cte2.firstSewingDate,#cte2.AVG_QAQTY
                ,DATEADD(DAY,iif(isnull(#cte2.AVG_QAQTY,0) = 0,0,ceiling((t.qty+t.FOCQty - #cte2.sewing_output)/(#cte2.AVG_QAQTY*1.0))),#cte2.firstSewingDate) [Est_offline]
                ,IIF(isnull(t.TotalCTN,0)=0, 0, round(t.ClogCTN / (t.TotalCTN*1.0),4) * 100 ) [pack_rate]
                ,t.TotalCTN
                --,t.FtyCTN
                ,t.TotalCTN-t.FtyCTN as FtyCtn
                , t.ClogCTN, t.InspDate, t.ActPulloutDate,t.FtyKPI
                --,t.KPIChangeReason
                ,KPIChangeReason.KPIChangeReason  KPIChangeReason
                ,t.PlanDate, dbo.getTPEPass1(t.SMR) [SMR], dbo.getTPEPass1(T.MRHandle) [Handle]
                ,(select dbo.getTPEPass1(p.POSMR) from dbo.PO p WITH (NOLOCK) where p.ID =t.POID) [PO SMR]
                ,(select dbo.getTPEPass1(p.POHandle) from dbo.PO p WITH (NOLOCK) where p.ID =t.POID) [PO Handle]
                --,(select dbo.getTPEPass1(p.McHandle) from dbo.PO p WITH (NOLOCK) where p.ID =t.POID) [MC Handle]
                ,dbo.getTPEPass1(t.McHandle) [MC Handle]
                ,t.DoxType
                ,(select article+',' from (select distinct q.Article  from dbo.Order_Qty q WITH (NOLOCK) where q.ID = t.OrderID) t for xml path('')) article_list
                --, t.Customize1 [SpecMark]
                , (select Name from Reason WITH (NOLOCK) where ReasonTypeID = 'Style_SpecialMark' and ID = t.SpecialMark) [SpecMark]
                , t.GFR, t.SampleReason
                ,(select s.StdTms * t.CPU from System s WITH (NOLOCK) ) [TMS]"));
            if (isArtwork) 
                sqlCmd.Append(string.Format(@",{0} ",artworktypes.ToString().Substring(0, artworktypes.ToString().Length - 1)));
            sqlCmd.Append(string.Format(@" from #cte t inner join #cte2 on #cte2.OrderID = t.OrderID"));
            if (isArtwork) 
                sqlCmd.Append(string.Format(@" left join #tmscost_pvt on #tmscost_pvt.orderid = t.orderid "));

            //KPIChangeReason
            sqlCmd.Append(@"  outer apply ( select ID + '-' + Name KPIChangeReason  from Reason 
				where ReasonTypeID = 'Order_BuyerDelivery' and ID = t.KPIChangeReason 
				and t.KPIChangeReason !='' and t.KPIChangeReason is not null ) KPIChangeReason  ");

            sqlCmd.Append(string.Format(@" order by {0}", orderby));


            DBProxy.Current.DefaultTimeout = 1800;
            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), cmds, out printData);
            DBProxy.Current.DefaultTimeout = 0;
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }
            return Result.True;
        }

        // 產生Excel
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            SetCount(printData.Rows.Count);

            if (printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            if (printData.Columns.Count > 16384)
            {
                MyUtility.Msg.WarningBox("Columns of Data is over 16,384 in excel file, please narrow down range of condition.");
                return false;
            }

            if (printData.Rows.Count + 1 > 1048576)
            {
                MyUtility.Msg.WarningBox("Lines of Data is over 1,048,576 in excel file, please narrow down range of condition.");
                return false;
            }

            if (isArtwork)
            {
                Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Planning_R15_WIP.xltx"); //預先開啟excel app
                MyUtility.Excel.CopyToXls(printData, "", "Planning_R15_WIP.xltx", 1, true, null, objApp);      // 將datatable copy to excel
                Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表

                for (int i = 0; i < dtArtworkType.Rows.Count; i++)  //列印動態欄位的表頭
                {
                    objSheets.Cells[1, 55 + i] = dtArtworkType.Rows[i]["id"].ToString();
                }

                //首列資料篩選
                Microsoft.Office.Interop.Excel.Range firstRow = (Microsoft.Office.Interop.Excel.Range)objSheets.Rows[1];
                firstRow.AutoFilter(1, Type.Missing, Microsoft.Office.Interop.Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);
                objApp.Cells.EntireColumn.AutoFit();  //自動欄寬

                if (objSheets != null) Marshal.FinalReleaseComObject(objSheets);    //釋放sheet
                if (objApp != null) Marshal.FinalReleaseComObject(objApp);          //釋放objApp
            }
            else
            {
                Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Planning_R15_WIP.xltx"); //預先開啟excel app
                MyUtility.Excel.CopyToXls(printData, "", "Planning_R15_WIP.xltx", 1, true, null, objApp);      // 將datatable copy to excel
                Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表

                //首列資料篩選
                Microsoft.Office.Interop.Excel.Range firstRow = (Microsoft.Office.Interop.Excel.Range)objSheets.Rows[1];
                firstRow.AutoFilter(1, Type.Missing, Microsoft.Office.Interop.Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);
                objApp.Cells.EntireColumn.AutoFit();  //自動欄寬

                if (objSheets != null) Marshal.FinalReleaseComObject(objSheets);    //釋放sheet
                if (objApp != null) Marshal.FinalReleaseComObject(objApp);          //釋放objApp
            }
            
            return true;
        }

        private void R15_Load(object sender, EventArgs e)
        {

        }
    }
}
