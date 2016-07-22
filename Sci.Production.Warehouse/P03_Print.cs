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
        public P03_Print()
        {
            InitializeComponent();
            // TODO: Complete member initialization
        }
        DataTable dt;
        string sqlcmd;
        protected override bool ValidateInput()
        {
           
            sqlcmd = @"select  a.id[poid]
            ,b.StyleID[style]
            ,a.SEQ1+a.SEQ2[SEQ]
            ,dbo.getMtlDesc(a.id,a.SEQ1,a.seq2,2,0)[Desc]
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
            AND IsFormA = 1),'Y','')
            from dbo.PO_Supp_Detail a
            left join dbo.orders b
            on
            a.id=b.id
            left join dbo.PO_Supp c
            on
            a.id=c.id and a.SEQ1=c.SEQ1
            left join dbo.Fabric_Supp d
            on
            d.SCIRefno=a.SCIRefno and d.SuppID=c.SuppID
            left join dbo.Fabric_HsCode e
            on 
            e.SCIRefno=a.SCIRefno and e.SuppID=c.SuppID and e.year=year(a.ETA)
            left join dbo.Supp f
            on 
            f.id=c.SuppID
            where a.id='13111582CCS'";
            return base.ValidateInput();
        }
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            return  DBProxy.Current.Select("", sqlcmd, out dt); 
        }
            protected override bool OnToExcel(Win.ReportDefinition report)
            {
                if (dt == null || dt.Rows.Count == 0)
                {
                    MyUtility.Msg.ErrorBox("Data not found");
                    return false;
                }

                var saveDialog = Sci.Utility.Excel.MyExcelPrg.GetSaveFileDialog(Sci.Utility.Excel.MyExcelPrg.filter_Excel);
                saveDialog.ShowDialog();
                string outpa = saveDialog.FileName;
                if (outpa.Empty())
                {
                    return true;
                }
                 if (this.radioPanel1.Value == this.radioButton1.Value) {
                    Sci.Utility.Excel.SaveXltReportCls x2 = new Utility.Excel.SaveXltReportCls("Warehouse_P03_Print-1.xltx");
                    x2.dicDatas.Add("##poid", dt);                    
                    x2.Save(outpa, false);

                if (this.radioPanel1.Value == this.radioButton2.Value) {
                    Sci.Utility.Excel.SaveXltReportCls xl = new Utility.Excel.SaveXltReportCls("Warehouse_P03_Print-2.xltx");
                    xl.dicDatas.Add("##poid", dt);                    
                    xl.Save(outpa, false);

                }
                return true;
            }
       
            }
           
        }
    
    


