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

namespace Sci.Production.Planning
{
    public partial class R02 : Sci.Win.Tems.PrintForm
    {
        int selectindex = 0;
        string factory, mdivision, orderby, spno1, spno2, artworktype,subcons;
        DateTime? sciDelivery1, sciDelivery2, buyerDelivery1, buyerDelivery2, sewinline1, sewinline2
            , cutinline1, cutinline2;
        DataTable printData;
        decimal totalpoqty, totalpartsqty;

        public R02(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            txtMdivision1.Text = Sci.Env.User.Keyword;
            txtfactory1.Text = Sci.Env.User.Factory;
            cbxCategory.SelectedIndex = 0;
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(dateRange1.Value1) &&
                MyUtility.Check.Empty(dateRange2.Value1) &&
                MyUtility.Check.Empty(dateRange4.Value1) &&
                MyUtility.Check.Empty(dateRange3.Value1) &&
                (MyUtility.Check.Empty(txtSpno1.Text) || MyUtility.Check.Empty(txtSpno2.Text)))
            {
                MyUtility.Msg.WarningBox("< Buyer Delivery > & < SCI Delivery > & < In Line > & < Cut Inline > & < SP# > can't be empty!!");
                return false;
            }

            #region -- 擇一必輸的條件 --
            sciDelivery1 = dateRange1.Value1;
            sciDelivery2 = dateRange1.Value2;
            buyerDelivery1 = dateRange2.Value1;
            buyerDelivery2 = dateRange2.Value2;
            sewinline1 = dateRange4.Value1;
            sewinline2 = dateRange4.Value2;
            cutinline1 = dateRange3.Value1;
            cutinline2 = dateRange3.Value2;
            spno1 = txtSpno1.Text;
            spno2 = txtSpno2.Text;
            #endregion
            subcons = txtMultiSubcon1.Subcons;
            mdivision = txtMdivision1.Text;
            factory = txtfactory1.Text;
            selectindex = cbxCategory.SelectedIndex;
            artworktype = txtartworktype_fty1.Text;
            
            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            #region -- sql parameters declare --
            System.Data.SqlClient.SqlParameter sp_spno1 = new System.Data.SqlClient.SqlParameter();
            sp_spno1.ParameterName = "@spno1";

            System.Data.SqlClient.SqlParameter sp_spno2 = new System.Data.SqlClient.SqlParameter();
            sp_spno2.ParameterName = "@spno2";

            System.Data.SqlClient.SqlParameter sp_style = new System.Data.SqlClient.SqlParameter();
            sp_style.ParameterName = "@style";

            System.Data.SqlClient.SqlParameter sp_factory = new System.Data.SqlClient.SqlParameter();
            sp_factory.ParameterName = "@factory";

            System.Data.SqlClient.SqlParameter sp_mdivision = new System.Data.SqlClient.SqlParameter();
            sp_mdivision.ParameterName = "@MDivision";

            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
            #endregion

            StringBuilder sqlCmd = new StringBuilder();

            sqlCmd.Append(string.Format(@"
;with orderData as	-- 撈出符合的order資料
(
	select o1.MDivisionID,o1.ID,o1.StyleID,o1.StyleUkey,o1.FactoryID,o1.SewLine,o1.SewInLine,o1.SewOffLine
	,o1.SciDelivery,o1.BuyerDelivery,o1.MTLETA,o1.CutInLine
	,o2.ArtworkTypeID,o2.Qty stitch,o2.Price,o2.Cost,o2.TMS
	,(select Article +',' from (select rtrim(article) article from dbo.Order_Article where id = o1.ID) tmp for xml path('')) articles
	,o2.ArtworkID,o2.PatternCode,o2.PatternDesc,sum(o2.PoQty) OrderQty 
,DBO.GETSTDQTY(o1.id,'A',null,null) as stdqty
,DBO.getMinCompleteSewQty(o1.id,null,null) as garments
	from dbo.orders o1 
	inner join dbo.View_Order_Artworks o2 on o2.id = o1.ID
	where 1=1 "));

            //o1.BuyerDelivery between '20150101' and '20150731' AND O1.Finished = 0 AND O2.Price > 0 and o1.FtyGroup = 'MWI' 
            //	and o1.id='15040605CW002'
            #region --- 條件組合  ---
            if (!MyUtility.Check.Empty(buyerDelivery1))
            {
                sqlCmd.Append(string.Format(@" and o1.BuyerDelivery between '{0}' and '{1}'"
                , Convert.ToDateTime(buyerDelivery1).ToString("d"), Convert.ToDateTime(buyerDelivery2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(sciDelivery1))
            {
                sqlCmd.Append(string.Format(@" and o1.SciDelivery between '{0}' and '{1}'",
                Convert.ToDateTime(sciDelivery1).ToString("d"), Convert.ToDateTime(sciDelivery2).ToString("d")));
            }
            if (!MyUtility.Check.Empty(spno1))
            {
                sqlCmd.Append(" and o1.id >= @spno1 and o1.id <= @spno2");
                sp_spno1.Value = spno1;
                sp_spno2.Value = spno2;
                cmds.Add(sp_spno1);
                cmds.Add(sp_spno2);
            }

            if (!MyUtility.Check.Empty(sewinline1))
            {
                sqlCmd.Append(string.Format(@" and o1.sewinline between '{0}' and '{1}'"
                , Convert.ToDateTime(sewinline1).ToString("d"), Convert.ToDateTime(sewinline2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(cutinline1))
            {
                sqlCmd.Append(string.Format(@" and o1.cutinline between '{0}' and '{1}'"
                , Convert.ToDateTime(cutinline1).ToString("d"), Convert.ToDateTime(cutinline2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(artworktype))
            {
                sqlCmd.Append(string.Format(@" and o2.artworktypeid = '{0}'", artworktype));
            }

            if (!MyUtility.Check.Empty(mdivision))
            {
                sqlCmd.Append(" and o1.mdivisionid = @MDivision");
                sp_mdivision.Value = mdivision;
                cmds.Add(sp_mdivision);
            }

            if (!MyUtility.Check.Empty(factory))
            {
                sqlCmd.Append(" and o1.factoryid = @factory");
                sp_factory.Value = factory;
                cmds.Add(sp_factory);
            }        

            switch (selectindex)
            {
                case 0:
                    sqlCmd.Append(@" and (o1.Category = 'B' or o1.Category = 'S')");
                    break;
                case 1:
                    sqlCmd.Append(@" and o1.Category = 'B' ");
                    break;
                case 2:
                    sqlCmd.Append(@" and (o1.Category = 'S')");
                    break;
            }

            #endregion
            //sqlCmd.Append(")");
            sqlCmd.Append(string.Format(@" group by o1.MDivisionID,o1.ID,o1.StyleID,o1.StyleUkey,o1.FactoryID,o1.SewLine
,o1.SewInLine,o1.SewOffLine
	,o1.SciDelivery,o1.BuyerDelivery,o1.MTLETA,o1.CutInLine
	,o2.ArtworkTypeID,o2.Qty,o2.Price,o2.Cost,o2.TMS,o2.ArtworkID,o2.PatternCode,o2.PatternDesc
),
 artworkpoData as -- 撈出artworkpo
(
	select a1.ID poid,a1.status,a1.LocalSuppID
	,b1.PoQty poqty ,b1.Farmout,b1.Farmin,b1.ApQty,b1.ukey po_detailukey
	,orderData.*
	from dbo.ArtworkPO a1
	inner join dbo.ArtworkPO_Detail b1 on b1.ID= a1.ID
	inner join orderData on orderData.id = b1.OrderID and orderData.ArtworkTypeID = b1.ArtworkTypeID 
	and orderData.ArtworkID = b1.ArtworkId and orderData.PatternCode = b1.PatternCode
	where a1.Status = 'Approved'"));
            if (!MyUtility.Check.Empty(subcons))
            {
                sqlCmd.Append(string.Format(@" and a1.localsuppid in ({0})",subcons));
            }        
sqlCmd.Append(string.Format(@"
)
,  artwork_quot as (
select x.*,
x.LocalSuppID+'-'+(select LocalSupp.Abb from dbo.LocalSupp where id = x.LocalSuppID) supplier
,x2.Oven
,x2.Wash
,x2.Mockup
from artworkpoData x
outer apply (select t3.Mockup,t3.Oven,t3.Wash from  dbo.style_artwork t2 
inner join dbo.Style_Artwork_Quot t3 on t3.Ukey = t2.Ukey
inner join dbo.Order_Article t4 on t4.id = x.ID
where t2.StyleUkey = x.StyleUkey and t2.Article = t4.Article and t2.ArtworkTypeID = x.ArtworkTypeID and
t2.PatternCode = x.PatternCode and t3.PriceApv = 'Y'
) x2
)
, farm_in_out as(
select * 
from artwork_quot
left join (select farmin.IssueDate FarmInDate
			,FarmIn_Detail.Qty FarmInQty,NULL FarmOutDate,0 FarmOutQty,ArtworkPo_DetailUkey 
			from dbo.FarmIn inner join dbo.FarmIn_Detail on FarmIn_Detail.ID= FarmIn.ID 
			where status='Confirmed' ) m on  m.ArtworkPo_DetailUkey =  artwork_quot.po_detailukey
union all
select * 
from artwork_quot
left join (select NULL FarmInDate,0 FarmInQty,FarmOut.IssueDate FarmOutDate,FarmOut_Detail.Qty FarmOutQty,ArtworkPo_DetailUkey 
			from dbo.FarmOut inner join dbo.FarmOut_Detail on FarmOut_Detail.ID= FarmOut.ID where status='Confirmed') n 
			on n.ArtworkPo_DetailUkey =  artwork_quot.po_detailukey
)
"));
            if (checkBox1.Checked)
            {
                sqlCmd.Append(@"select k.FactoryID,k.ID,k.SewLine,k.StyleID,k.stitch,k.ArtworkTypeID,k.PatternCode+'-'+k.PatternDesc pattern
,k.supplier,k.articles,k.poqty,k.stitch*k.poqty total_stitch,k.MTLETA,k.SciDelivery,k.Oven,k.Wash,k.Wash,k.Mockup
,k.stdqty,k.CutInLine,k.SewInLine,k.SewOffLine
,k.Farmout,k.Farmout,k.garments 
,y.FarmOutDate,y.FarmOutQty,y.FarmInDate,y.FarmInQty
from artwork_quot k
left join farm_in_out y on y.ArtworkPo_DetailUkey = k.po_detailukey
order by k.FactoryID,k.ID,isnull(FarmOutDate,'99991231')");
            }
            else
            {
                sqlCmd.Append(@"select k.FactoryID,k.ID,k.SewLine,k.StyleID,k.stitch,k.ArtworkTypeID,k.PatternCode+'-'+k.PatternDesc pattern
,k.supplier,k.articles,k.poqty,k.stitch*k.poqty total_stitch,k.MTLETA,k.SciDelivery,k.Oven,k.Wash,k.Wash,k.Mockup
,k.stdqty,k.CutInLine,k.SewInLine,k.SewOffLine
,k.Farmout,k.Farmout,k.garments
from artwork_quot k
order by k.FactoryID,k.ID");
            }

            DBProxy.Current.DefaultTimeout = 1800;
            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), cmds, out printData);
            foreach (DataRow dr in printData.Rows) 
            {
             string poqty = dr["poqty"].ToString();
             string totalqty = dr["total_stitch"].ToString();
             totalpoqty = totalpoqty +=  Convert.ToDecimal(poqty);
             totalpartsqty = totalpartsqty += Convert.ToDecimal(totalqty);
            }
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
            
            if (printData.Rows.Count + 6 > 1048576)
            {
                MyUtility.Msg.WarningBox("Lines of Data is over 1,048,576 in excel file, please narrow down range of condition.");
                return false;
            }
            var saveDialog = Sci.Utility.Excel.MyExcelPrg.GetSaveFileDialog(Sci.Utility.Excel.MyExcelPrg.filter_Excel);
            saveDialog.ShowDialog();
            string outpath = saveDialog.FileName;
            if (outpath.Empty())
            {
                return false;
            }
            if (checkBox1.Checked)
            {
                Sci.Utility.Excel.SaveXltReportCls x1 = new Sci.Utility.Excel.SaveXltReportCls("Planning_R02_Detail.xltx");
                Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Planning_R02_Detail.xltx"); //預先開啟excel app
                MyUtility.Excel.CopyToXls(printData, "", "Planning_R02_Detail.xltx", 6, true, null, objApp);      // 將datatable copy to excel
                objApp.Visible = false;
                Microsoft.Office.Interop.Excel.Worksheet objSheet = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
                string d1 = (MyUtility.Check.Empty(sciDelivery1)) ? "" : Convert.ToDateTime(sciDelivery1).ToString("yyyy/MM/dd");
                string d2 = (MyUtility.Check.Empty(sciDelivery2)) ? "" : Convert.ToDateTime(sciDelivery2).ToString("yyyy/MM/dd");
                objSheet.Cells[4, 16] = d1 + "~" + d2;   // 條件字串寫入excel
                objSheet.Cells[3, 13] = totalpoqty;   // 條件字串寫入excel
                objSheet.Cells[4, 13] = totalpartsqty;   // 條件字串寫入excel
           
                objApp.Visible = true;
                if (objSheet != null) Marshal.FinalReleaseComObject(objSheet);    //釋放sheet
                if (objApp != null) Marshal.FinalReleaseComObject(objApp);          //釋放objApp
                return true;
            }
            else
            {
                Microsoft.Office.Interop.Excel.Application objApp2 = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Planning_R02.xltx"); //預先開啟excel app
                MyUtility.Excel.CopyToXls(printData, "", "Planning_R02.xltx", 6, true, null, objApp2);      // 將datatable copy to excel
                objApp2.Visible = false;
                Microsoft.Office.Interop.Excel.Worksheet objSheet2 = objApp2.ActiveWorkbook.Worksheets[1];   // 取得工作表
               
                string d3 = (MyUtility.Check.Empty(sciDelivery1)) ? "" : Convert.ToDateTime(sciDelivery1).ToString("yyyy/MM/dd");
                string d4 = (MyUtility.Check.Empty(sciDelivery2)) ? "" : Convert.ToDateTime(sciDelivery2).ToString("yyyy/MM/dd");
                objSheet2.Cells[4, 16] = d3 + "~" + d4;   // 條件字串寫入excel
                objSheet2.Cells[3, 13] = totalpoqty;   // 條件字串寫入excel
                objSheet2.Cells[4, 13] = totalpartsqty;   // 條件字串寫入excel
                objApp2.Visible = true;
            if (objSheet2 != null) Marshal.FinalReleaseComObject(objSheet2);    //釋放sheet
            if (objApp2 != null) Marshal.FinalReleaseComObject(objApp2);          //釋放objApp
            return true;
            }
         
        }
    }
}
