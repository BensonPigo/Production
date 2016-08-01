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

namespace Sci.Production.Warehouse
{
    public partial class P03_Print : Sci.Win.Tems.PrintForm
    {
        
        DataRow CurrentDataRow;
        public P03_Print(DataRow row)
        {
            InitializeComponent();

            this.CurrentDataRow = row;
            
            // TODO: Complete member initialization
        }
        string outpa;
        
       // string sqlcmd;
      
        protected override bool ValidateInput()
        {
            var saveDialog = Sci.Utility.Excel.MyExcelPrg.GetSaveFileDialog(Sci.Utility.Excel.MyExcelPrg.filter_Excel);
            saveDialog.ShowDialog();
           outpa = saveDialog.FileName;
            if (outpa.Empty())
            {
                
                return false;
            }
            return base.ValidateInput();
        }
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
           
            DataRow row = this.CurrentDataRow;
            string id = row["ID"].ToString();
            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
            DataTable dt;
            string xlt;

            if (this.radioPanel1.Value == this.radioButton1.Value)
            {
                xlt = @"Warehouse_P03_Print-1.xltx";
                DBProxy.Current.Select("", @"select a.id [sp]
			                                       ,b.StyleID [style#]
			                                       ,a.SEQ1+a.SEQ2 [SEQ]
			                                       ,c.SuppID [Supp]
			                                       ,d.NameEN [Supp Name]
			                                       ,substring(convert(varchar,a.cfmetd, 101),1,5) [Sup. 1st Cfm ETA]
			                                       ,substring(convert(varchar,a.RevisedETD, 101),1,5) [RevisedETD]
		                                           ,a.Refno [Ref#]
                                                   ,dbo.getMtlDesc(a.id,a.SEQ1,a.SEQ2,2,0) [Description]
			                                       ,e.AbbCH [Chinese Abb]
			                                       ,f.HsCode [HS Code]
			                                       ,case a.FabricType 
			                                             when 'F' then 'Fabric'
			                                             when 'A'then 'Accessory'
			                                             else a.FabricType
			                                             end Material_Type
			                                       ,a.ColorID [Color]
			                                       ,a.SizeSpec [Size]
			                                       ,h.Currencyid [Currency]
			                                       ,a.UsedQty [Qty]
			                                       ,a.Qty [Order Qty]
		                                           ,a.NETQty [Net Qty]
			                                       ,a.NETQty+a.LossQty [Use Qty]
			                                       ,a.ShipQty [Ship Qty]
			                                       ,a.ShipFOC [F.O.C]
			                                       ,a.ApQty [AP Qty]
			                                       ,IIF(EXISTS(SELECT * FROM DBO.Export_Detail g
			                                            WHERE g.PoID = a.id
			                                            AND g.SEQ1 = a.seq1
			                                            AND g.SEQ2 =a.seq2
			                                            AND IsFormA = 1),'Y','') [FormA]
			                                       ,a.InputQty [Taipei Stock Qty]
			                                       ,a.POUnit [Unit]
			                                       ,a.Complete [Cmplt]
			                                       ,substring(convert(varchar, a.ata, 101),1,5) [Act. Eta]
			                                       ,(select id+',' from 
			                                           (select distinct id from export_detail  where poid =a.id and seq1=a.seq1 and seq2=a.seq2) t for xml path(''))  [WK#]
			                                       ,(select orderid+',' from 
			                                           (select ol.orderid  from PO_Supp_Detail_OrderList ol  where id =a.id and seq1=a.seq1 and seq2=a.seq2) ol for xml path(''))  [Order List]
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
			                                          (select r.Remark  from dbo.Receiving_Detail r where POID =a.id and seq1=a.seq1 and seq2=a.seq2 and remark !='') r for xml path('')) [Remark]
			                                from dbo.PO_Supp_Detail a
			                                left join dbo.Orders b on a.id=b.id
			                                left join dbo.PO_Supp c on c.id=a.id and c.SEQ1=a.SEQ1
			                                left join dbo.supp d on d.id=c.SuppID
			                                left join dbo.Fabric_Supp e on e.SCIRefno=a.SCIRefno and e.SuppID=c.SuppID
			                                left join dbo.Fabric_HsCode f on f.SCIRefno=a.SCIRefno and f.SuppID=c.SuppID and f.Year=Year(a.eta)
		                                    left join dbo.supp h on h.id=c.SuppID
			                                left join dbo.MDivisionPoDetail i on i.POID=a.ID
			                                where a.id=@ID", pars, out dt);			       
          }
          else  
          {
           xlt = @"Warehouse_P03_Print-2.xltx";
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
                                              ,FormA=IIF(EXISTS(SELECT * FROM DBO.Export_Detail g
                                                    WHERE g.PoID =a.id
                                                    AND g.SEQ1 = a.seq1
                                                    AND g.SEQ2 = a.seq2
                                                    AND IsFormA = 1)
                                              ,'Y','')
                                       from dbo.PO_Supp_Detail a
                                       left join dbo.orders b on a.id=b.id
                                       left join dbo.PO_Supp c on a.id=c.id and a.SEQ1=c.SEQ1
                                       left join dbo.Fabric_Supp d on d.SCIRefno=a.SCIRefno and d.SuppID=c.SuppID
                                       left join dbo.Fabric_HsCode e on e.SCIRefno=a.SCIRefno and e.SuppID=c.SuppID and e.year=year(a.ETA)
                                       left join dbo.Supp f on f.id=c.SuppID
                                       where a.id=@ID", pars, out dt);                          
          }
          SaveXltReportCls xl = new SaveXltReportCls(xlt);
          xl.dicDatas.Add("##sp", dt);
          xl.Save(outpa, false);
          return Result.True;
        }

        protected override bool OnToExcel(ReportDefinition report)
        {
            return true;
        }

        }
    }


    
    


