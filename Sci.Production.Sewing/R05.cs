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
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;

namespace Sci.Production.Sewing
{
    public partial class R05 : Sci.Win.Tems.PrintForm
    {
        DataTable printData;
        DateTime? date1, date2;
        string factory, sp_from,sp_to,  brand, status;
        public R05(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
           // MyUtility.Tool.SetupCombox(cb_status, 1, 1, ",Unfinished,Finished,Excess,All");
            cb_status.SelectedIndex = 0;
        }
        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            date1 = dateBuyerDelivery.Value1;
            date2 = dateBuyerDelivery.Value2;
            factory = txtfactory.Text;
            sp_from = txtsp_from.Text;
            sp_to = txtsp_to.Text;
            brand = txtbrand.Text;
            status = cb_status.Text;

            return base.ValidateInput();
        }
        // 非同步取資料
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
                                    Where SewingOutput_Detail_Detail.OrderId = OQG.ID
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

            if (!MyUtility.Check.Empty(date1))
            {
                sqlCmd.Append(string.Format(" and O.BuyerDelivery >= '{0}' ", Convert.ToDateTime(date1).ToString("d")));
            }
            if (!MyUtility.Check.Empty(date2))
            {
                sqlCmd.Append(string.Format(" and O.BuyerDelivery <= '{0}' ", Convert.ToDateTime(date2).ToString("d")));
            }
            if (!MyUtility.Check.Empty(factory))
            {
                sqlCmd.Append(string.Format(" and O.FtyGroup = '{0}'", factory));
            }
            if (!MyUtility.Check.Empty(sp_from))
            {
                sqlCmd.Append(string.Format(" and OQG.ID >= '{0}'", sp_from));
            }
            if (!MyUtility.Check.Empty(sp_to))
            {
                sqlCmd.Append(string.Format(" and OQG.ID <= '{0}'", sp_to));
            }

            if (!MyUtility.Check.Empty(brand))
            {
                sqlCmd.Append(string.Format(" and O.BrandID = '{0}'", brand));
            }


            string status_condistion = "";
            if (status.Equals("Unfinished"))
            {
                status_condistion = " ToSp_allocate_qty < ToSP_Qty";
            }
            else if (status.Equals("Finished"))
            {
                status_condistion = " ToSp_allocate_qty = ToSP_Qty";
            }
            else if (status.Equals("Excess"))
            {
                status_condistion = " ToSp_allocate_qty > ToSP_Qty";
            }

            if (status_condistion.Length > 0) {
                sqlCmd.Insert(0, @"select * from (");
                sqlCmd.Append(@") as alldata where " + status_condistion);
            }
         

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out printData);
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

            this.ShowWaitMessage("Starting EXCEL...");
            string excelFile = "Sewing_R05_GarmentOrderAllocateOutputReport.xltx";
            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + excelFile);//開excelapp
            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
      
            bool result = MyUtility.Excel.CopyToXls(printData, "", xltfile: excelFile, headerRow: 1, excelApp: objApp);

            if (!result) { MyUtility.Msg.WarningBox(result.ToString(), "Warning"); }
            
            this.HideWaitMessage();
            return true;
        }
    }
}
