using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.PublicPrg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Windows.Forms;
using System.Reflection;
using Microsoft.Reporting.WinForms;
using System.Data.SqlClient;
using Sci.Win;
using Sci;
using Sci.Production;
using Sci.Utility.Excel;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Warehouse
{
    public partial class P03_Print : Sci.Win.Tems.PrintForm
    {
        DataTable dt;
        DataRow CurrentDataRow;
        string order_by = "";
        public P03_Print(DataRow row,int sort_by)
        {
            InitializeComponent();
            if (sort_by == 0)
            {
                order_by = " order by  a.refno ,a.id, iif(a.FabricType='F',1,iif(a.FabricType='A',2,3)), dbo.GetColorMultipleID(b.BrandID,a.ColorID)";
            }
            else {
                order_by = " order by a.id,a.seq1 , a.seq2";
            }
            this.CurrentDataRow = row;
            print.Visible = false;
            
            // TODO: Complete member initialization
        }
       // string outpa;
        
       // string sqlcmd;
      
        protected override bool ValidateInput()
        {
           // var saveDialog = Sci.Utility.Excel.MyExcelPrg.GetSaveFileDialog(Sci.Utility.Excel.MyExcelPrg.filter_Excel);
           // saveDialog.ShowDialog();
           //outpa = saveDialog.FileName;
           // if (outpa.Empty())
           // {
                
           //     return false;
           // }
            return base.ValidateInput();
        }

        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
           
            DataRow row = this.CurrentDataRow;
            string id = row["ID"].ToString();
            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
            //string xlt;

            if (this.radioPanel1.Value == this.radioMaterialStatus.Value)
            {
                //xlt = @"Warehouse_P03_Print-1.xltx";
                //PO_Supp_tmp為了Chinese Abb Fabric_Supp 的suppID如果找不到，請找 PO_Supp_Detail.SCIRefno seq1最小的且suppID在 Fabric_Supp是有資料的那筆
                DBProxy.Current.Select("", @"with PO_Supp_tmp as (
                                                 select aa.*,bb.AbbCH from 
                                                 (select a.ID,a.SCIRefno,FIRST_VALUE(b.SuppID) OVER (partition by a.SCIRefno ORDER BY b.SEQ1 ) SuppID ,a.SEQ1 ,a.SEQ2
                                                 from dbo.PO_Supp_Detail a WITH (NOLOCK) 
                                                 left join dbo.PO_Supp b WITH (NOLOCK) on b.id=a.id and b.SEQ1=a.SEQ1
                                                 where a.id=@ID ) aa
                                                 left join dbo.Fabric_Supp bb on bb.SCIRefno = aa.SCIRefno and bb.SuppID = aa.SuppID 
                                                )
                                                select a.id [sp]
			                                       ,b.StyleID [style#]
			                                       ,a.SEQ1+a.SEQ2 [SEQ]
			                                       ,c.SuppID [Supp]
			                                       ,d.NameEN [Supp Name]
			                                       ,substring(convert(varchar,a.cfmetd, 101),1,5) [Sup. 1st Cfm ETA]
			                                       ,substring(convert(varchar,a.RevisedETA, 101),1,5) [RevisedETD]
		                                           ,a.Refno [Ref#]
                                                   ,dbo.getMtlDesc(a.id,a.SEQ1,a.SEQ2,2,0) [Description]
			                                       ,iif(e.AbbCH is null, j.AbbCH, e.AbbCH) [Chinese Abb]
			                                       ,f.HsCode [HS Code]
			                                       ,case a.FabricType 
			                                             when 'F' then 'Fabric'
			                                             when 'A'then 'Accessory'
			                                             else a.FabricType
			                                             end Material_Type
			                                       ,dbo.GetColorMultipleID(b.BrandID,a.ColorID) [Color]
			                                       ,a.SizeSpec [Size]
			                                       ,h.Currencyid [Currency]
			                                       ,a.UsedQty [Qty]
			                                       ,a.Qty [Order Qty]
		                                           ,a.NETQty [Net Qty]
			                                       ,a.NETQty+a.LossQty [Use Qty]
			                                       ,a.ShipQty [Ship Qty]
			                                       ,a.ShipFOC [F.O.C]
			                                       ,a.ApQty [AP Qty]
			                                       ,IIF(EXISTS(SELECT * FROM DBO.Export_Detail g WITH (NOLOCK) 
			                                            WHERE g.PoID = a.id
			                                            AND g.SEQ1 = a.seq1
			                                            AND g.SEQ2 =a.seq2
			                                            AND IsFormA = 1),'Y','') [FormA]
			                                       ,a.InputQty [Taipei Stock Qty]
			                                       ,a.POUnit [Unit]
			                                       ,a.Complete [Cmplt]
			                                       ,substring(convert(varchar, a.FinalETA, 101),1,5) [Act. Eta]
			                                       ,(select id+',' from 
			                                           (select distinct id from export_detail WITH (NOLOCK)  where poid =a.id and seq1=a.seq1 and seq2=a.seq2) t for xml path(''))  [WK#]
			                                       ,(select orderid+',' from 
			                                           (select ol.orderid  from PO_Supp_Detail_OrderList ol WITH (NOLOCK)  where id =a.id and seq1=a.seq1 and seq2=a.seq2) ol for xml path(''))  [Order List]
			                                       ,i.InQty [Arrived Qty]
			                                       ,a.StockUnit [Unit]
			                                       ,i.OutQty [Released Qty]
			                                       ,i.AdjustQty [Adjust Qty]
			                                       ,i.InQty-i.OutQty+i.AdjustQty [Balance]
			                                       ,i.LInvQty [Stock Qty]
			                                       ,i.LObQty [Scrap Qty]
			                                       ,i.ALocation [Bulk Location]
			                                       ,i.BLocation [Stock Location]
                                                   ,[FIR]=dbo.getinspectionresult(a.id,a.seq1,a.seq2)
			                                       ,(select Remark+',' from 
			                                          (select r.Remark  from dbo.Receiving_Detail r WITH (NOLOCK) where POID =a.id and seq1=a.seq1 and seq2=a.seq2 and remark !='') r for xml path('')) [Remark]
			                                        ,a.junk
                                            from dbo.PO_Supp_Detail a WITH (NOLOCK) 
			                                left join dbo.Orders b WITH (NOLOCK) on a.id=b.id
			                                left join dbo.PO_Supp c WITH (NOLOCK) on c.id=a.id and c.SEQ1=a.SEQ1
			                                left join dbo.supp d WITH (NOLOCK) on d.id=c.SuppID
			                                left join dbo.Fabric_Supp e WITH (NOLOCK) on e.SCIRefno=a.SCIRefno and e.SuppID=c.SuppID
			                                left join dbo.Fabric_HsCode f WITH (NOLOCK) on f.SCIRefno=a.SCIRefno and f.SuppID=c.SuppID and f.Year=Year(a.eta)
		                                    left join dbo.supp h WITH (NOLOCK) on h.id=c.SuppID
			                                left join dbo.MDivisionPoDetail i WITH (NOLOCK) on i.POID=a.ID and a.SEQ1=i.Seq1 and a.SEQ2=i.Seq2
                                            left join PO_Supp_tmp j on a.ID = j.ID and a.SEQ1 = j.SEQ1 and a.SEQ2 = j.SEQ2
			                                where a.id=@ID  " + order_by, pars, out dt);			       
          }
          else  
          {
           //xlt = @"Warehouse_P03_Print-2.xltx";
           DBProxy.Current.Select("", @"select a.id [sp]
                                              ,b.StyleID [style]
                                              ,a.SEQ1+a.SEQ2 [SEQ]
                                              ,[Desc]=dbo.getMtlDesc(a.id,a.SEQ1,a.seq2,2,0)
                                              ,chinese_abb=d.AbbCH
                                              ,case a.fabrictype
                                                  when 'F' then 'Fabric'
                                                  when 'A' THEN 'Accessory'
                                                  Else a.FabricType
                                                  end Material_Type
                                              ,Hs_code=e.HsCode
                                              ,supp=c.SuppID
                                              ,Supp_Name=f.AbbEN
                                              ,Currency=f.Currencyid
                                              ,Del=substring(convert(varchar, a.cfmetd, 101),1,5)
                                              ,Used_Qty=a.UsedQty
                                              ,Order_Qty=a.qty
                                              ,Taipei_Stock=a.InputQty
                                              ,Unit=a.POUnit
                                              ,TTL_Qty=a.ShipQty
                                              ,FOC=a.FOC
                                              ,ty=convert(numeric(5,2), iif( isnull(a.Qty,0)=0,100,a.shipqty/a.qty*100))
                                              ,OK=a.Complete
                                              ,Exp_Date=substring(convert(varchar,a.eta, 101),1,5)
                                              ,FormA=IIF(EXISTS(SELECT * FROM DBO.Export_Detail g WITH (NOLOCK) 
                                                    WHERE g.PoID =a.id
                                                    AND g.SEQ1 = a.seq1
                                                    AND g.SEQ2 = a.seq2
                                                    AND IsFormA = 1)
                                              ,'Y','')
                                             ,a.junk
                                       from dbo.PO_Supp_Detail a WITH (NOLOCK) 
                                       left join dbo.orders b WITH (NOLOCK) on a.id=b.id
                                       left join dbo.PO_Supp c WITH (NOLOCK) on a.id=c.id and a.SEQ1=c.SEQ1
                                       left join dbo.Fabric_Supp d WITH (NOLOCK) on d.SCIRefno=a.SCIRefno and d.SuppID=c.SuppID
                                       left join dbo.Fabric_HsCode e WITH (NOLOCK) on e.SCIRefno=a.SCIRefno and e.SuppID=c.SuppID and e.year=year(a.ETA)
                                       left join dbo.Supp f WITH (NOLOCK) on f.id=c.SuppID
                                       where a.id=@ID " + order_by, pars, out dt);                          
          }
          //SaveXltReportCls xl = new SaveXltReportCls(xlt);
          //xl.dicDatas.Add("##sp", dt);
          //xl.Save(outpa, false);
          return Result.True;
        }

        protected override bool OnToExcel(ReportDefinition report)
        {
          

            if (dt.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }
            // 顯示筆數於PrintForm上Count欄位
            SetCount(dt.Rows.Count);
            if (this.radioPanel1.Value == this.radioMaterialStatus.Value)
            {
              
                Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Warehouse_P03_Print-1.xltx"); //預先開啟excel app
                MyUtility.Excel.CopyToXls(dt, "", "Warehouse_P03_Print-1.xltx", 1, false, null, objApp);      // 將datatable copy to excel
                Excel.Worksheet worksheet = objApp.Sheets[1];
              
                for (int i = 0; i < dt.Rows.Count; i++) {
                    if (dt.Rows[i]["junk"].ToString().Equals("True")) {
                        worksheet.Range[worksheet.Cells[1][i + 2], worksheet.Cells[40][i + 2]].Interior.ColorIndex = 15;
                    }
                }
                worksheet.Columns[41].Delete();
                string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Warehouse_P03");
                objApp.ActiveWorkbook.SaveAs(strExcelName);
                objApp.Quit();
                Marshal.ReleaseComObject(objApp);
                Marshal.ReleaseComObject(worksheet);

                strExcelName.OpenFile();
            }
            else {
                Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Warehouse_P03_Print-2.xltx"); //預先開啟excel app
                MyUtility.Excel.CopyToXls(dt, "", "Warehouse_P03_Print-2.xltx", 1, false, null, objApp);      // 將datatable copy to excel
                Excel.Worksheet worksheet = objApp.Sheets[1];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["junk"].ToString().Equals("True"))
                    {
                        worksheet.Range[worksheet.Cells[1][i + 2], worksheet.Cells[21][i + 2]].Interior.ColorIndex = 15;
                    }
                }
                worksheet.Columns[22].Delete();
                string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Warehouse_P03");
                objApp.ActiveWorkbook.SaveAs(strExcelName);
                objApp.Quit();
                Marshal.ReleaseComObject(objApp);
                Marshal.ReleaseComObject(worksheet);

                strExcelName.OpenFile();
            }
           

            return true;
        }
        }
    }