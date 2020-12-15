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
    /// R11
    /// </summary>
    public partial class R11 : Win.Tems.PrintForm
    {
        private decimal months;
        private string factory;
        private string mdivision;
        private string category;
        private string category2;
        private DateTime? sciDelivery1;
        private DateTime? sciDelivery2;
        private DataTable printData;
        private DataTable dtArtworkType;
        private StringBuilder condition = new StringBuilder();
        private StringBuilder artworktypes = new StringBuilder();
        private StringBuilder pvtid = new StringBuilder();

        /// <summary>
        /// R11
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R11(ToolStripMenuItem menuitem)
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
            if (MyUtility.Check.Empty(this.dateSCIDelivery.Value1))
            {
                MyUtility.Msg.WarningBox(" < SCI Delivery > can't be empty!!");
                return false;
            }

            #region -- 必輸的條件 --
            this.sciDelivery1 = this.dateSCIDelivery.Value1;
            this.sciDelivery2 = this.dateSCIDelivery.Value2;
            #endregion
            this.mdivision = this.txtMdivision.Text;
            this.factory = this.txtfactory.Text;
            this.category = this.comboCategory.SelectedValue.ToString();
            this.category2 = this.comboCategory.Text;

            this.months = this.numNewStyleBaseOn.Value;

            DualResult result;
            if (!(result = DBProxy.Current.Select(string.Empty, "select id from dbo.artworktype WITH (NOLOCK) where istms=1 or isprice= 1 order by seq", out this.dtArtworkType)))
            {
                MyUtility.Msg.WarningBox(result.ToString());
                return false;
            }

            if (this.dtArtworkType.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Artwork Type data not found, Please inform MIS to check !");
                return false;
            }

            this.artworktypes.Clear();
            this.pvtid.Clear();
            for (int i = 0; i < this.dtArtworkType.Rows.Count; i++)
            {
                if (i == 0)
                {
                    this.artworktypes.Append(string.Format(@"[{0}]", this.dtArtworkType.Rows[i]["id"].ToString()));
                }
                else
                {
                    this.artworktypes.Append(string.Format(@", [{0}]", this.dtArtworkType.Rows[i]["id"].ToString()));
                }

                this.pvtid.Append($"{"\v"}, [{this.dtArtworkType.Rows[i]["id"].ToString()}] = isnull([{this.dtArtworkType.Rows[i]["id"].ToString()}], 0) {"\n"}");
            }

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

            System.Data.SqlClient.SqlParameter sp_factory = new System.Data.SqlClient.SqlParameter
            {
                ParameterName = "@factory",
            };

            System.Data.SqlClient.SqlParameter sp_mdivision = new System.Data.SqlClient.SqlParameter
            {
                ParameterName = "@MDivision",
            };

            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();

            #endregion

            StringBuilder sqlCmd = new StringBuilder();
            string whereIncludeCancelOrder = this.chkIncludeCancelOrder.Checked ? string.Empty : " and o.Junk = 0 ";
            sqlCmd.Append(string.Format($@"
SELECT o.FtyGroup,o.Styleid,o.SeasonID,o.CdCodeID,o.Qty,o.CPU,o.category,o.StyleUkey,o.id
    ,SciDelivery = min(o.SciDelivery)over(partition by o.Styleid)
    ,oa.Article,o.brandid
into #tmpo
FROM Orders o WITH (NOLOCK)
outer apply(
	select Article= STUFF((select distinct CONCAT(',',Article) 
					from Order_Article oa WITH (NOLOCK)
					where oa.id=o.ID
					for xml path('')),1,1,'')
) oa
Where 1=1 {whereIncludeCancelOrder} "));
            #region --- 條件組合  ---
            this.condition.Clear();
            if (!MyUtility.Check.Empty(this.sciDelivery1))
            {
                sqlCmd.Append(string.Format(@" and o.SciDelivery >= '{0}'", Convert.ToDateTime(this.sciDelivery1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.sciDelivery2))
            {
                sqlCmd.Append(string.Format(@" and o.SciDelivery <= '{0}'", Convert.ToDateTime(this.sciDelivery2).ToString("d")));
            }
            #region [condition]處理
            if (!MyUtility.Check.Empty(this.sciDelivery1) && !MyUtility.Check.Empty(this.sciDelivery2))
            {
                this.condition.Append(string.Format(@"SCI Delivery : {0} ~ {1} ", Convert.ToDateTime(this.sciDelivery1).ToString("d"), Convert.ToDateTime(this.sciDelivery2).ToString("d")));
            }
            else if (!MyUtility.Check.Empty(this.sciDelivery1) && MyUtility.Check.Empty(this.sciDelivery2))
            {
                this.condition.Append(string.Format(@"SCI Delivery : {0} ~ ", Convert.ToDateTime(this.sciDelivery1).ToString("d")));
            }
            else if (MyUtility.Check.Empty(this.sciDelivery1) && !MyUtility.Check.Empty(this.sciDelivery2))
            {
                this.condition.Append(string.Format(@"SCI Delivery :  ~ {0}", Convert.ToDateTime(this.sciDelivery2).ToString("d")));
            }
            #endregion
            if (!MyUtility.Check.Empty(this.mdivision))
            {
                sqlCmd.Append(" and o.mdivisionid = @MDivision");
                sp_mdivision.Value = this.mdivision;
                cmds.Add(sp_mdivision);
                this.condition.Append(string.Format(@"    M : {0}", this.mdivision));
            }

            if (!MyUtility.Check.Empty(this.factory))
            {
                sqlCmd.Append(" and o.FtyGroup = @factory");
                sp_factory.Value = this.factory;
                cmds.Add(sp_factory);
                this.condition.Append(string.Format(@"    Factory : {0}", this.factory));
            }

            sqlCmd.Append($" and o.Category in ({this.category})");

            this.condition.Append(string.Format(@"    Category : {0}", this.category2));
            #endregion

            sqlCmd.Append(string.Format(@"
select distinct o.FtyGroup,o.Styleid,oa.Article,oa.Brand,
o.SeasonID,o.CdCodeID,o.CPU,TQty = sum(o.Qty),TCPU = sum(o.CPU*o.Qty)
into #tmpol
from #tmpo o
outer apply(
	select Article= STUFF((select distinct CONCAT(',',Article) 
					from #tmpo oa
					where oa.styleid=o.styleid
					for xml path('')),1,1,''),
           Brand = STUFF((select distinct CONCAT(',',Brandid) 
					from #tmpo oa
					where oa.styleid=o.styleid
					for xml path('')) ,1,1,'')
           
) oa
group by o.FtyGroup,o.Styleid,oa.Article,oa.Brand,o.SeasonID,o.CdCodeID,o.CPU
--
select 
	o.StyleID
	,qty = sod.QAQty
	,MH = so.manpower * sod.WorkHour
	,tms = iif(o.Category = 'S',o.CPU*StdTMS,o.CPU*s.StdTMS*(dbo.GetOrderLocation_Rate(o.id,sod.ComboType)/100))
    --,tms = iif(o.Category = 'S',o.CPU*StdTMS,o.CPU*s.StdTMS*(ol.rate/100))
	,S = sum(iif(o.Category = 'S',1,0)) over(partition by o.StyleID)
	,B = sum(iif(o.Category = 'B',1,0)) over(partition by o.StyleID)
	,OutputDate
	,SewingLineID
into #tmp_AR_Basic
from System s,(select distinct StyleID,CdCodeID,SciDelivery from  #tmpo) o2
inner join Orders o WITH (NOLOCK) on o2.StyleID = o.StyleID and o2.CdCodeID = o.CdCodeID
inner join SewingOutput_Detail sod  WITH (NOLOCK) on sod.OrderId = o.ID
inner join SewingOutput so  WITH (NOLOCK) on sod.id = so.id
--inner join Style_Location sl  WITH (NOLOCK) on sl.StyleUkey = o.StyleUkey AND sl.Location = iif(o.StyleUnit = 'PCS',sl.Location,sod.ComboType)
--inner join Order_Location ol  WITH (NOLOCK) on ol.OrderId = o.ID AND ol.Location = sod.ComboType
--outer apply (select value = dbo.GetOrderLocation_Rate(o.id,sod.ComboType)) ol_rate
where 1=1"));
            if (!MyUtility.Check.Empty(this.mdivision))
            {
                sqlCmd.Append(" and o.mdivisionid = @MDivision");
            }

            if (!MyUtility.Check.Empty(this.factory))
            {
                sqlCmd.Append(" and o.FtyGroup = @factory");
            }

            if (this.numNewStyleBaseOn.Value != 0)
            {
                sqlCmd.Append(string.Format(@" and  dateadd(month,{0},o2.SciDelivery ) < so.OutputDate", -this.months));
                this.condition.Append(string.Format(@"    New Style base on {0} month(s)", this.months));
            }

            sqlCmd.Append($" and o.Category in ({this.category})");

            sqlCmd.Append(
                $@"
select a.StyleID
	,A = IIF(Sum(a.MH) = 0,' ',format(sum(a.tms*a.qty)/(3600*Sum(a.MH)),'P'))	
into #tmp_A
from #tmp_AR_Basic a 
group by a.StyleID
--
select a.StyleID
	,R =    case    when max_OutputDate is null then 'New Style'
                    when a.S > 0 AND a.B = 0 then 'New Style'
                    else concat(min(a.SewingLineID),'(',format(b.max_OutputDate,'yyyy/MM/dd'),')')
                    end
into #tmp_R
from #tmp_AR_Basic a inner join (select StyleID,max_OutputDate = max(OutputDate) from #tmp_AR_Basic group by StyleID) b
on a.StyleID = b.StyleID and a.OutputDate = b.max_OutputDate
group by a.StyleID,b.max_OutputDate,S,B
--
select o.StyleID,P = sum(st.Price)
into #tmp_P
from #tmpo o2 
inner join orders o  WITH (NOLOCK) on o.StyleID = o2.StyleID and o.SeasonID = o2.SeasonID
inner join style_tmscost st  WITH (NOLOCK) on st.StyleUkey = o.StyleUkey
where ArtworkTypeID ='GMT WASH'
group by o.StyleID,st.ArtworkTypeID
--
select *
into #cls
from
(
	select o.StyleID,otc.ArtworkTypeID,max(iif(at.IsTMS=1,otc.tms,otc.price)) price_tms 
	from #tmpo o
	inner join dbo.Order_TmsCost otc  WITH (NOLOCK) on otc.id = o.id
	inner join dbo.ArtworkType at  WITH (NOLOCK)  on at.id = otc.ArtworkTypeID
	where at.IsTMS =1 or at.IsPrice = 1
	group by o.StyleID,otc.ArtworkTypeID
)a
pivot
(
    sum(price_tms)
    for artworktypeid in ({this.artworktypes.ToString()})
)as pvt 

select o.FtyGroup
	   , o.StyleID
	   , o.Article
	   , o.Brand
	   , o.SeasonID
	   , o.CdCodeID
	   , CPU = format(o.CPU,'0.00')
	   , o.TQty
	   , TCPU = format(o.TCPU,'0.00')
	   , a.A
	   , R = isnull(r.R,'New Style')
	   , W = iif(w.P=0 or w.P is null,'N','Y')
	   {this.pvtid.ToString()}
from #tmpol o
left join #tmp_A a on a.StyleID = o.StyleID
left join #tmp_R r on r.StyleID = o.StyleID
left join #tmp_P w on w.StyleID = o.StyleID
left join #cls s on s.StyleID = o.StyleID
order by o.FtyGroup,o.StyleID,o.SeasonID

drop table #tmpo,#tmpol,#tmp_AR_Basic,#tmp_A,#tmp_R,#tmp_P,#cls");

            DBProxy.Current.DefaultTimeout = 10800;
            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), cmds, out this.printData);
            DBProxy.Current.DefaultTimeout = 0;
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
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

            this.ShowWaitMessage("Data Loading...");

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Planning_R11.xltx"); // 預先開啟excel app
            MyUtility.Excel.CopyToXls(this.printData, string.Empty, "Planning_R11.xltx", 3, false, null, objApp);      // 將datatable copy to excel
            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
            objSheets.Cells[2, 1] = this.condition.ToString();   // 條件字串寫入excel

            string strArtworkType = string.Empty;

            // 列印動態欄位的表頭
            for (int i = 0; i < this.dtArtworkType.Rows.Count; i++)
            {
                strArtworkType = this.dtArtworkType.Rows[i]["id"].ToString().Trim();

                #region ArtworkType折行處理，讓EXCEL看起來較美觀
                switch (strArtworkType)
                {
                    case "BONDING (MACHINE)":
                        strArtworkType = "BONDING" + Environment.NewLine + "(MACHINE)";
                        break;
                    case "BONDING (HAND)":
                        strArtworkType = "BONDING" + Environment.NewLine + "(HAND)";
                        break;
                    case "HEAT TRANSFER":
                        strArtworkType = "HEAT" + Environment.NewLine + "TRANSFER";
                        break;
                    case "DIE CUT":
                        strArtworkType = "DIE" + Environment.NewLine + "CUT";
                        break;
                    case "SUBLIMATION PRINT":
                        strArtworkType = "SUBLIMATION" + Environment.NewLine + "PRINT";
                        break;
                    case "QUILTING(AT)":
                        strArtworkType = "QUILTING" + Environment.NewLine + "(AT)";
                        break;
                    case "SUBLIMATION SPRAY":
                        strArtworkType = "SUBLIMATION" + Environment.NewLine + "SPRAY";
                        break;
                    case "SUBLIMATION ROLLER":
                        strArtworkType = "SUBLIMATION" + Environment.NewLine + "ROLLER";
                        break;
                    case "SMALL HOT PRESS":
                        strArtworkType = "SMALL" + Environment.NewLine + "HOT PRESS";
                        break;
                    case "BIG HOT PRESS":
                        strArtworkType = "BIG" + Environment.NewLine + "HOT PRESS";
                        break;
                    case "DOWN FILLING":
                        strArtworkType = "DOWN" + Environment.NewLine + "FILLING";
                        break;
                    case "ZIG ZAG":
                        strArtworkType = "ZIG" + Environment.NewLine + "ZAG";
                        break;
                    case "QUILTING(HAND)":
                        strArtworkType = "QUILTING" + Environment.NewLine + "(HAND)";
                        break;
                    case "D-chain ZIG ZAG":
                        strArtworkType = "D-chain" + Environment.NewLine + "ZIG ZAG";
                        break;
                    case "REAL FLATSEAM":
                        strArtworkType = "REAL" + Environment.NewLine + "FLATSEAM";
                        break;
                    case "BIG HOT FOR BONDING":
                        strArtworkType = "BIG HOT" + Environment.NewLine + "FOR BONDING";
                        break;
                    case "EMBOSS/DEBOSS":
                        strArtworkType = "EMBOSS/" + Environment.NewLine + "DEBOSS";
                        break;
                    case "GMT WASH":
                        strArtworkType = "GMT" + Environment.NewLine + "WASH";
                        break;
                    case "PAD PRINTING":
                        strArtworkType = "PAD" + Environment.NewLine + "PRINTING";
                        break;
                    case "Garment Dye":
                        strArtworkType = "Garment" + Environment.NewLine + "Dye";
                        break;
                    default:
                        break;
                }
                #endregion

                objSheets.Cells[3, 13 + i] = strArtworkType;
            }

            objApp.Cells.EntireColumn.AutoFit();    // 自動欄寬
            objApp.Cells.EntireRow.AutoFit();       // 自動欄高
            objApp.Cells.EntireColumn.AutoFit();    // 自動欄寬，不知為何還要多跑一次才會讓格式變美，先讓他多跑一次
            objApp.Cells.EntireRow.AutoFit();       // 自動欄高
            objSheets.get_Range("C1:C1").ColumnWidth = 50;
            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("Planning_R11");
            Microsoft.Office.Interop.Excel.Workbook workbook = objApp.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);
            Marshal.ReleaseComObject(objSheets);
            Marshal.ReleaseComObject(workbook);

            strExcelName.OpenFile();
            #endregion
            this.HideWaitMessage();
            return true;
        }
    }
}
