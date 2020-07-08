using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Data;

namespace Sci.Production.Sewing
{
    /// <summary>
    /// R05
    /// </summary>
    public partial class R05 : Sci.Win.Tems.PrintForm
    {
        private DataTable printData;
        private DateTime? date1;
        private DateTime? date2;
        private string factory;
        private string sp_from;
        private string sp_to;
        private string brand;
        private string status;

        /// <summary>
        /// R05
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R05(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();

            // MyUtility.Tool.SetupCombox(cb_status, 1, 1, ",Unfinished,Finished,Excess,All");
            this.cb_status.SelectedIndex = 0;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.date1 = this.dateBuyerDelivery.Value1;
            this.date2 = this.dateBuyerDelivery.Value2;
            this.factory = this.txtfactory.Text;
            this.sp_from = this.txtsp_from.Text;
            this.sp_to = this.txtsp_to.Text;
            this.brand = this.txtbrand.Text;
            this.status = this.cb_status.Text;

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(string.Format(@"
                        Select  [Sp#] = OQG.ID
                                ,[StyleID] = O.StyleID
                                ,[Brand] = O.BrandID
                                ,[Season] = O.SeasonID
                                ,[*] = SL.Location
                                ,[From SP#] = OQG.OrderIDFrom
                                ,[Article] = OQG.Article
                                ,[SizeCode] =OQG.SizeCode
                                ,[ToSP_Cmf_PK_Qty] = PL.PackQty
                                ,[ToSP_Qty] = IIF(OQG.Junk=0,OQG.Qty,0)
                                ,[ToSp_allocate_qty] = SODDG.ToSp_allocate_qty
                                ,[ToSp_Balance] =  IIF(OQG.Junk=0,OQG.Qty,0) - SODDG.ToSp_allocate_qty
                                ,[ToSp_BuyerDelivery] = O.BuyerDelivery
                                ,[FromSp_Sewing_qty] = SODD.FromSp_Sewing_qty
                                ,[FromSp_Accu_Qty] = SODDG2.FromSp_Accu_Qty
                                ,[FromSp_Avl_Qty] = SODD.FromSp_Sewing_qty  -  SODDG2.FromSp_Accu_Qty
                                ,[Is_Trans_Qty] = IIF(SODDG.ToSp_allocate_qty >= IIF(OQG.Junk=0,OQG.Qty,0), 'N/A' , IIF((SODD.FromSp_Sewing_qty  -  SODDG2.FromSp_Accu_Qty) >= (IIF(OQG.Junk=0,OQG.Qty,0) - SODDG.ToSp_allocate_qty) , 'Y' , 'N' ))
                                ,[Is_Trans_Qty_Excess] = IIF(SODDG.ToSp_allocate_qty > IIF(OQG.Junk=0,OQG.Qty,0) , 'Y' , 'N')
                        From Order_Qty_Garment OQG WITH (NOLOCK)
                        Inner join Orders O  WITH (NOLOCK) on OQG.id=O.id
                        Inner join Factory F  WITH (NOLOCK) on O.FactoryID=F.ID
                        Left join Style_Location  SL  WITH (NOLOCK) on O.styleukey=SL.Styleukey 
                        outer apply(select isnull(Sum(b.shipqty),0) AS PackQty
		                                from PackingList a  WITH (NOLOCK)
		                                inner join PackingList_Detail b on a.ID=b.ID
		                                where b.OrderID= OQG.ID
		                                      and b.Article = OQG.Article 
                                                and b.SizeCode = OQG.SizeCode
		                                      and a.Status= 'Confirmed') as PL
                        outer apply(select Isnull(Sum(SewingOutput_Detail_Detail_Garment.QAQty),0) as ToSp_allocate_qty
                                        from SewingOutput_Detail_Detail_Garment  WITH (NOLOCK)
                                        Where SewingOutput_Detail_Detail_Garment.OrderId = OQG.ID
                                        		and SewingOutput_Detail_Detail_Garment.ComboType = SL.Location
                                        		and SewingOutput_Detail_Detail_Garment.Article =  OQG.Article 
                                        		and SewingOutput_Detail_Detail_Garment.SizeCode = OQG.SizeCode
                                        		and SewingOutput_Detail_Detail_Garment.OrderIDfrom = OQG.OrderIDFrom
                                        ) as SODDG
                        outer apply(select Isnull(Sum(SewingOutput_Detail_Detail.QAQty),0) as FromSp_Sewing_qty
                                    from SewingOutput_Detail_Detail  WITH (NOLOCK)
                                    Where SewingOutput_Detail_Detail.OrderId = OQG.OrderIDFrom
                                    		and SewingOutput_Detail_Detail.ComboType = SL.Location
                                    		and SewingOutput_Detail_Detail.Article = OQG.Article
                                    		and SewingOutput_Detail_Detail.SizeCode = OQG.SizeCode
                                     ) as SODD
                        outer apply(select Isnull(Sum(SewingOutput_Detail_Detail_Garment.QAQty),0) as FromSp_Accu_Qty
                                    from SewingOutput_Detail_Detail_Garment  WITH (NOLOCK)
                                    Where SewingOutput_Detail_Detail_Garment.ComboType = SL.Location
                                    		and SewingOutput_Detail_Detail_Garment.Article = OQG.Article
                                    		and SewingOutput_Detail_Detail_Garment.SizeCode =OQG.SizeCode
                                    		and SewingOutput_Detail_Detail_Garment.OrderIDfrom = OQG.OrderIDFrom
                                    ) as SODDG2
                        where 1 = 1 
                "));

            if (!MyUtility.Check.Empty(this.date1))
            {
                sqlCmd.Append(string.Format(" and O.BuyerDelivery >= '{0}' ", Convert.ToDateTime(this.date1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.date2))
            {
                sqlCmd.Append(string.Format(" and O.BuyerDelivery <= '{0}' ", Convert.ToDateTime(this.date2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.factory))
            {
                sqlCmd.Append(string.Format(" and O.FtyGroup = '{0}'", this.factory));
            }

            if (!MyUtility.Check.Empty(this.sp_from))
            {
                sqlCmd.Append(string.Format(" and OQG.ID >= '{0}'", this.sp_from));
            }

            if (!MyUtility.Check.Empty(this.sp_to))
            {
                sqlCmd.Append(string.Format(" and OQG.ID <= '{0}'", this.sp_to));
            }

            if (!MyUtility.Check.Empty(this.brand))
            {
                sqlCmd.Append(string.Format(" and O.BrandID = '{0}'", this.brand));
            }

            string status_condistion = string.Empty;
            if (this.status.Equals("Unfinished"))
            {
                status_condistion = " ToSp_allocate_qty < ToSP_Qty";
            }
            else if (this.status.Equals("Finished"))
            {
                status_condistion = " ToSp_allocate_qty = ToSP_Qty";
            }
            else if (this.status.Equals("Excess"))
            {
                status_condistion = " ToSp_allocate_qty > ToSP_Qty";
            }

            if (status_condistion.Length > 0)
            {
                sqlCmd.Insert(0, @"select * from (");
                sqlCmd.Append(@") as alldata where " + status_condistion);
            }

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out this.printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Result.True;
        }

        /// <inheritdoc/>
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
            string excelFile = "Sewing_R05_GarmentOrderAllocateOutputReport.xltx";
            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + excelFile); // 開excelapp
            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表

            bool result = MyUtility.Excel.CopyToXls(this.printData, string.Empty, xltfile: excelFile, headerRow: 1, excelApp: objApp);

            if (!result)
            {
                MyUtility.Msg.WarningBox(result.ToString(), "Warning");
            }

            this.HideWaitMessage();
            return true;
        }
    }
}
