using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;

namespace Sci.Production.Planning
{
    /// <summary>
    /// R02
    /// </summary>
    public partial class R02 : Win.Tems.PrintForm
    {
        private string factory;
        private string mdivision;
        private string spno1;
        private string spno2;
        private string artworktype;
        private string subcons;
        private string strDateRange;
        private string strMinDate;
        private string strMaxDate;
        private DateTime? sciDelivery1;
        private DateTime? sciDelivery2;
        private DateTime? buyerDelivery1;
        private DateTime? buyerDelivery2;
        private DateTime? sewinline1;
        private DateTime? sewinline2;
        private DateTime? cutinline1;
        private DateTime? cutinline2;
        private DataTable printData;
        private decimal totalpoqty;
        private decimal totalpartsqty;
        private string category;

        /// <summary>
        /// R02
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R02(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.txtMdivision.Text = Env.User.Keyword;
            this.txtfactory.Text = Env.User.Factory;
        }

        /// <summary>
        /// ValidateInput
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(this.dateSCIDelivery.Value1) &&
                MyUtility.Check.Empty(this.dateInLineDate.Value1) &&
                MyUtility.Check.Empty(this.dateBuyerDelivery.Value1) &&
                MyUtility.Check.Empty(this.dateCutInline.Value1) &&
                (MyUtility.Check.Empty(this.txtSPNoStart.Text) || MyUtility.Check.Empty(this.txtSPNoEnd.Text)))
            {
                MyUtility.Msg.WarningBox("< Buyer Delivery > & < SCI Delivery > & < In Line > & < Cut Inline > & < SP# > can't be empty!!");
                return false;
            }

            #region -- 擇一必輸的條件 --
            this.sciDelivery1 = this.dateSCIDelivery.Value1;
            this.sciDelivery2 = this.dateSCIDelivery.Value2;
            this.buyerDelivery1 = this.dateInLineDate.Value1;
            this.buyerDelivery2 = this.dateInLineDate.Value2;
            this.sewinline1 = this.dateBuyerDelivery.Value1;
            this.sewinline2 = this.dateBuyerDelivery.Value2;
            this.cutinline1 = this.dateCutInline.Value1;
            this.cutinline2 = this.dateCutInline.Value2;
            this.spno1 = this.txtSPNoStart.Text;
            this.spno2 = this.txtSPNoEnd.Text;
            #endregion

            this.strMinDate = this.GetMinOrMax("Min", this.sciDelivery1, this.buyerDelivery1, this.sewinline1, this.cutinline1);
            this.strMaxDate = this.GetMinOrMax("Max", this.sciDelivery2, this.buyerDelivery2, this.sewinline2, this.cutinline2);
            this.strDateRange = this.strMinDate + "~" + this.strMaxDate;

            this.subcons = this.txtMultiSubconSubcon.Subcons;
            this.mdivision = this.txtMdivision.Text;
            this.factory = this.txtfactory.Text;
            this.artworktype = this.txtartworktype_ftySubProcess.Text;
            this.category = this.comboCategory.SelectedValue.ToString();

            return base.ValidateInput();
        }

        /// <summary>
        /// OnAsyncDataLoad
        /// </summary>
        /// <param name="e">e</param>
        /// <returns>DualResult</returns>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
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
	,o2.ArtworkID,o2.PatternCode,o2.PatternDesc,sum(o2.PoQty) OrderQty 
,DBO.GETSTDQTY(o1.id,'A',null,null) as stdqty
,DBO.getMinCompleteSewQty(o1.id,null,null) as garments
	from dbo.orders o1 WITH (NOLOCK) 
	inner join dbo.View_Order_Artworks o2 on o2.id = o1.ID
	where 1=1 "));

            #region --- 條件組合  ---
            if (!MyUtility.Check.Empty(this.buyerDelivery1))
            {
                sqlCmd.Append(string.Format(@" and o1.BuyerDelivery >= '{0}'", Convert.ToDateTime(this.buyerDelivery1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.buyerDelivery2))
            {
                sqlCmd.Append(string.Format(@" and o1.BuyerDelivery <= '{0}'", Convert.ToDateTime(this.buyerDelivery2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.sciDelivery1))
            {
                sqlCmd.Append(string.Format(@" and o1.SciDelivery >= '{0}'", Convert.ToDateTime(this.sciDelivery1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.sciDelivery2))
            {
                sqlCmd.Append(string.Format(@" and o1.SciDelivery <= '{0}'", Convert.ToDateTime(this.sciDelivery2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.spno1))
            {
                sqlCmd.Append(" and o1.id >= @spno1 ");
                sp_spno1.Value = this.spno1;
                cmds.Add(sp_spno1);
            }

            if (!MyUtility.Check.Empty(this.spno2))
            {
                sqlCmd.Append(" and o1.id <= @spno2");
                sp_spno2.Value = this.spno2;
                cmds.Add(sp_spno2);
            }

            if (!MyUtility.Check.Empty(this.sewinline1))
            {
                sqlCmd.Append(string.Format(@" and o1.sewinline >= '{0}'", Convert.ToDateTime(this.sewinline1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.sewinline2))
            {
                sqlCmd.Append(string.Format(@" and o1.sewinline <= '{0}'", Convert.ToDateTime(this.sewinline2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.cutinline1))
            {
                sqlCmd.Append(string.Format(@" and o1.cutinline >= '{0}'", Convert.ToDateTime(this.cutinline1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.cutinline2))
            {
                sqlCmd.Append(string.Format(@" and o1.cutinline <= '{0}'", Convert.ToDateTime(this.cutinline2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.artworktype))
            {
                sqlCmd.Append(string.Format(@" and o2.artworktypeid = '{0}'", this.artworktype));
            }

            if (!MyUtility.Check.Empty(this.mdivision))
            {
                sqlCmd.Append(" and o1.mdivisionid = @MDivision");
                sp_mdivision.Value = this.mdivision;
                cmds.Add(sp_mdivision);
            }

            if (!MyUtility.Check.Empty(this.factory))
            {
                sqlCmd.Append(" and o1.factoryid = @factory");
                sp_factory.Value = this.factory;
                cmds.Add(sp_factory);
            }

            sqlCmd.Append($" and o1.Category in ({this.category})");
            #endregion

            sqlCmd.Append(string.Format(@" group by o1.MDivisionID,o1.ID,o1.StyleID,o1.StyleUkey,o1.FactoryID,o1.SewLine
,o1.SewInLine,o1.SewOffLine
	,o1.SciDelivery,o1.BuyerDelivery,o1.MTLETA,o1.CutInLine
	,o2.ArtworkTypeID,o2.Qty,o2.Price,o2.Cost,o2.TMS,o2.ArtworkID,o2.PatternCode,o2.PatternDesc
),
 artworkpoData as -- 撈出artworkpo
(
	select a1.ID poid,a1.status,a1.LocalSuppID
	,b1.PoQty poqty ,b1.Farmout,b1.Farmin,b1.ApQty,b1.ukey po_detailukey, b1.Article, b1.SizeCode
	,orderData.*
	from dbo.ArtworkPO a1 WITH (NOLOCK) 
	inner join dbo.ArtworkPO_Detail b1 WITH (NOLOCK) on b1.ID= a1.ID
	inner join orderData on orderData.id = b1.OrderID and orderData.ArtworkTypeID = b1.ArtworkTypeID 
	and orderData.ArtworkID = b1.ArtworkId and orderData.PatternCode = b1.PatternCode
	where a1.Status = 'Approved'"));
            if (!MyUtility.Check.Empty(this.subcons))
            {
                sqlCmd.Append(string.Format(@" and a1.localsuppid in ({0})", this.subcons));
            }

            sqlCmd.Append(string.Format(@"
)
,  artwork_quot as (
select x.*,
x.LocalSuppID+'-'+(select LocalSupp.Abb from dbo.LocalSupp WITH (NOLOCK) where id = x.LocalSuppID) supplier
,x2.Oven
,x2.Wash
,x2.Mockup
from artworkpoData x
outer apply (select t3.Mockup,t3.Oven,t3.Wash from  dbo.style_artwork t2 WITH (NOLOCK) 
inner join dbo.Style_Artwork_Quot t3 WITH (NOLOCK) on t3.Ukey = t2.Ukey
inner join dbo.Order_Article t4 WITH (NOLOCK) on t4.id = x.ID
where t2.StyleUkey = x.StyleUkey and t2.Article = t4.Article and t2.ArtworkTypeID = x.ArtworkTypeID and
t2.PatternCode = x.PatternCode and t3.PriceApv = 'Y'
) x2
)
, farm_in_out as(
select * 
from artwork_quot
left join (select farmin.IssueDate FarmInDate
			,FarmIn_Detail.Qty FarmInQty,NULL FarmOutDate,0 FarmOutQty,ArtworkPo_DetailUkey 
			from dbo.FarmIn WITH (NOLOCK) inner join dbo.FarmIn_Detail WITH (NOLOCK) on FarmIn_Detail.ID= FarmIn.ID 
			where status='Confirmed' ) m on  m.ArtworkPo_DetailUkey =  artwork_quot.po_detailukey
union all
select * 
from artwork_quot
left join (select NULL FarmInDate,0 FarmInQty,FarmOut.IssueDate FarmOutDate,FarmOut_Detail.Qty FarmOutQty,ArtworkPo_DetailUkey 
			from dbo.FarmOut WITH (NOLOCK) inner join dbo.FarmOut_Detail WITH (NOLOCK) on FarmOut_Detail.ID= FarmOut.ID where status='Confirmed') n 
			on n.ArtworkPo_DetailUkey =  artwork_quot.po_detailukey
)
"));
            if (this.checkIncludeFarmOutInDate.Checked)
            {
                sqlCmd.Append(@"select k.FactoryID,k.ID,k.SewLine,k.StyleID,k.stitch,k.ArtworkTypeID,k.PatternCode+'-'+k.PatternDesc pattern
,k.supplier,k.article,k.SizeCode,k.poqty,k.stitch*k.poqty total_stitch,k.MTLETA,k.SciDelivery,k.BuyerDelivery
,iif(k.oven='1900-01-01','1',convert(varchar,k.oven)) as Oven
,iif(k.Wash='1900-01-01','1',convert(varchar,k.Wash)) as Wash
,iif(k.Wash='1900-01-01','1',convert(varchar,k.Wash)) as Wash
,iif(k.Mockup='1900-01-01','1',convert(varchar,k.Mockup)) as Mockup
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
,k.supplier,k.article,k.SizeCode,k.poqty,k.stitch*k.poqty total_stitch,k.MTLETA,k.SciDelivery,k.BuyerDelivery
,iif(k.oven='1900-01-01','1',convert(varchar,k.oven)) as Oven
,iif(k.Wash='1900-01-01','1',convert(varchar,k.Wash)) as Wash
,iif(k.Wash='1900-01-01','1',convert(varchar,k.Wash)) as Wash
,iif(k.Mockup='1900-01-01','1',convert(varchar,k.Mockup)) as Mockup
,k.stdqty,k.CutInLine,k.SewInLine,k.SewOffLine
,k.Farmout,k.Farmout,k.garments
from artwork_quot k
order by k.FactoryID,k.ID");
            }

            DBProxy.Current.DefaultTimeout = 1800;
            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), cmds, out this.printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            this.totalpoqty = 0;
            this.totalpartsqty = 0;  // 初始化
            foreach (DataRow dr in this.printData.Rows)
            {
                string poqty = dr["poqty"].ToString();
                string totalqty = dr["total_stitch"].ToString();
                this.totalpoqty = this.totalpoqty += Convert.ToDecimal(poqty);
                this.totalpartsqty = this.totalpartsqty += Convert.ToDecimal(totalqty);
            }

            DBProxy.Current.DefaultTimeout = 0;
            return Ict.Result.True;
        }

        /// <summary>
        /// OnToExcel
        /// </summary>
        /// <param name="report">report</param>
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

            if (this.printData.Columns.Count > 16384)
            {
                MyUtility.Msg.WarningBox("Columns of Data is over 16,384 in excel file, please narrow down range of condition.");
                return false;
            }

            if (this.printData.Rows.Count + 6 > 1048576)
            {
                MyUtility.Msg.WarningBox("Lines of Data is over 1,048,576 in excel file, please narrow down range of condition.");
                return false;
            }

            if (this.checkIncludeFarmOutInDate.Checked)
            {
                Utility.Excel.SaveXltReportCls x1 = new Utility.Excel.SaveXltReportCls("Planning_R02_Detail.xltx");
                Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Planning_R02_Detail.xltx"); // 預先開啟excel app
                MyUtility.Excel.CopyToXls(this.printData, string.Empty, "Planning_R02_Detail.xltx", 6, false, null, objApp);      // 將datatable copy to excel
                objApp.Visible = false;
                Microsoft.Office.Interop.Excel.Worksheet objSheet = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表

                objSheet.Cells[4, 16] = this.strDateRange;   // 條件字串寫入excel
                objSheet.Cells[3, 13] = this.totalpoqty;   // 條件字串寫入excel
                objSheet.Cells[4, 13] = this.totalpartsqty;   // 條件字串寫入excel

                #region Save & Show Excel
                string strExcelName = Class.MicrosoftFile.GetName("Planning_R02_Detail");
                Microsoft.Office.Interop.Excel.Workbook workbook = objApp.ActiveWorkbook;
                workbook.SaveAs(strExcelName);
                workbook.Close();
                objApp.Quit();
                Marshal.ReleaseComObject(objApp);
                Marshal.ReleaseComObject(objSheet);
                Marshal.ReleaseComObject(workbook);

                strExcelName.OpenFile();
                #endregion
                return true;
            }
            else
            {
                Microsoft.Office.Interop.Excel.Application objApp2 = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Planning_R02.xltx"); // 預先開啟excel app
                MyUtility.Excel.CopyToXls(this.printData, string.Empty, "Planning_R02.xltx", 6, false, null, objApp2);      // 將datatable copy to excel
                objApp2.Visible = false;
                Microsoft.Office.Interop.Excel.Worksheet objSheet2 = objApp2.ActiveWorkbook.Worksheets[1];   // 取得工作表

                objSheet2.Cells[4, 16] = this.strDateRange;   // 條件字串寫入excel
                objSheet2.Cells[3, 13] = this.totalpoqty;   // 條件字串寫入excel
                objSheet2.Cells[4, 13] = this.totalpartsqty;   // 條件字串寫入excel

                #region Save & Show Excel
                string strExcelName = Class.MicrosoftFile.GetName("Planning_R02");
                Microsoft.Office.Interop.Excel.Workbook workbook = objApp2.ActiveWorkbook;
                workbook.SaveAs(strExcelName);
                workbook.Close();
                objApp2.Quit();
                Marshal.ReleaseComObject(objApp2);
                Marshal.ReleaseComObject(objSheet2);
                Marshal.ReleaseComObject(workbook);

                strExcelName.OpenFile();
                #endregion
                return true;
            }
        }

        // 依type傳回最大或最小日期
        private string GetMinOrMax(string type, DateTime? dt1, DateTime? dt2, DateTime? dt3, DateTime? dt4)
        {
            List<string> list = new List<string>();
            if (!MyUtility.Check.Empty(dt1))
            {
                list.Add(Convert.ToDateTime(dt1).ToString("yyyy/MM/dd"));
            }

            if (!MyUtility.Check.Empty(dt2))
            {
                list.Add(Convert.ToDateTime(dt2).ToString("yyyy/MM/dd"));
            }

            if (!MyUtility.Check.Empty(dt3))
            {
                list.Add(Convert.ToDateTime(dt3).ToString("yyyy/MM/dd"));
            }

            if (!MyUtility.Check.Empty(dt4))
            {
                list.Add(Convert.ToDateTime(dt4).ToString("yyyy/MM/dd"));
            }

            list.Sort();
            if (list.Count > 0)
            {
                if (type == "Min")
                {
                    return list[0];
                }
                else
                {
                    return list[list.Count - 1];
                }
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
