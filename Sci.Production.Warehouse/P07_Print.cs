using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
//using System.Data.SqlClient;
using Sci.Win;
using Sci;
using Sci.Production;
using Sci.Utility.Excel;
using System.Runtime.InteropServices;

namespace Sci.Production.Warehouse
{
    public partial class P07_Print : Sci.Win.Tems.PrintForm
    {
        DataTable dt;
        string id, Date1, Date2, ETA, Invoice, Wk, FTYID;
        public P07_Print(List<String> polist)
        {
            InitializeComponent();
            CheckControlEnable();
            this.poidList = polist;
        }
        List<String> poidList;

        protected override bool ValidateInput()
        {

            if (ReportResourceName == "P07_Report2.rdlc")
            {
                if (!MyUtility.Check.Empty(txtSPNo.Text) && !poidList.Contains(this.txtSPNo.Text.TrimEnd(), StringComparer.OrdinalIgnoreCase))
                {
                    MyUtility.Msg.ErrorBox("SP# is not found.");
                    return false;
                }
            }
            return base.ValidateInput();
        }

        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            DataRow row = this.CurrentDataRow;
            id = row["ID"].ToString();
            Date1 = (MyUtility.Check.Empty(row["PackingReceive"])) ? "" : ((DateTime)MyUtility.Convert.GetDate(row["PackingReceive"])).ToShortDateString();
            Date2 = (MyUtility.Check.Empty(row["WhseArrival"])) ? "" : ((DateTime)MyUtility.Convert.GetDate(row["WhseArrival"])).ToShortDateString();
            ETA = MyUtility.Check.Empty(row["ETA"]) ? "" : ((DateTime)MyUtility.Convert.GetDate(row["ETA"])).ToShortDateString();
            Invoice = row["invno"].ToString();
            Wk = row["exportid"].ToString();
            FTYID = row["Mdivisionid"].ToString();
         
            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
           
            DataTable dtTitle;
            DualResult titleResult = DBProxy.Current.Select("",
           @"select m.nameEN
             from  dbo.Receiving r WITH (NOLOCK) 
             left join dbo.MDivision m WITH (NOLOCK) 
             on m.id = r.MDivisionID 
             where m.id = r.MDivisionID
             and r.id = @ID", pars, out dtTitle);
            if (!titleResult) { this.ShowErr(titleResult); }
            string RptTitle = dtTitle.Rows[0]["nameEN"].ToString();
            e.Report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("RptTitle", RptTitle));
          
            e.Report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("ETA", ETA));
            e.Report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Invoice", Invoice));
            e.Report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Wk", Wk));
            e.Report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("FTYID", FTYID));
   
            string sql = @"
select  
	R.Roll
	,R.Dyelot
	,R.PoId
	,R.Seq1+'-'+R.Seq2 AS SEQ
	,[RefNo]=p.RefNo
	, [ColorID]=Color.Value 
	,f.WeaveTypeID
	,o.BrandID
	,IIF((p.ID = lag(p.ID,1,'')over (order by p.ID,p.seq1,p.seq2)  
			AND (p.seq1 = lag(p.seq1,1,'')over (order by p.ID,p.seq1,p.seq2))  
			AND(p.seq2 = lag(p.seq2,1,'')over (order by p.ID,p.seq1,p.seq2))) 
				,'',dbo.getMtlDesc(R.poid,R.seq1,R.seq2,2,0))[Desc]            
	,R.ShipQty
	,R.pounit
	,R.StockQty
	,R.StockUnit
	,r.ActualQty
	,[QtyVaniance]=R.ShipQty-R.ActualQty
	,R.Weight
	,R.ActualWeight
	,[Vaniance]=R.ActualWeight - R.Weight 
	,[SubQty]=sum(R.ShipQty) OVER (PARTITION BY R.POID ,R.SEQ1,R.SEQ2 )   
	,[SubGW]=sum(R.Weight) OVER (PARTITION BY R.POID ,R.SEQ1,R.SEQ2 ) 
	,[SubAW]=sum(R.ActualWeight) OVER (PARTITION BY R.POID ,R.SEQ1,R.SEQ2 )   
	,[SubStockQty]=sum(R.StockQty) OVER (PARTITION BY R.POID ,R.SEQ1,R.SEQ2 )   
	,[TotalReceivingQty]=IIF((p.ID = lag(p.ID,1,'')over (order by p.ID,p.seq1,p.seq2)  
			AND (p.seq1 = lag(p.seq1,1,'')over (order by p.ID,p.seq1,p.seq2))  
			AND(p.seq2 = lag(p.seq2,1,'')over (order by p.ID,p.seq1,p.seq2))) 
				,null,sum(R.StockQty) OVER (PARTITION BY R.POID ,R.SEQ1,R.SEQ2 ))
	,[SubVaniance]=R.ShipQty - R.ActualQty
	,R.Remark		
    ,ColorName=(select name from color where color.id = p.colorid and color.BrandId = p.BrandId )
from dbo.Receiving_Detail R WITH (NOLOCK) 
LEFT join dbo.PO_Supp_Detail p WITH (NOLOCK) on p.ID = R.POID and  p.SEQ1 = R.Seq1 and P.seq2 = R.Seq2 
left join orders o WITH (NOLOCK) on o.ID = r.PoId
LEFT JOIN Fabric f WITH (NOLOCK) ON p.SCIRefNo=f.SCIRefNo
OUTER APPLY(
 SELECT [Value]=
	 CASE WHEN f.MtlTypeID in ('EMB THREAD','SP THREAD','THREAD') THEN p.SuppColor
		 ELSE dbo.GetColorMultipleID(o.BrandID,p.ColorID)
	 END
)Color
where R.id = @ID";

            if (!MyUtility.Check.Empty(txtSPNo.Text))
            {
                pars.Add(new SqlParameter("@poid", txtSPNo.Text));
                sql += " and R.Poid = @poid";
            }

            DualResult result = DBProxy.Current.Select("",
            sql, pars, out dt);
            if (!result) { return result; }
            if (ReportResourceName == "P07_Report2.rdlc")
            {
                e.Report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Date2", Date2));
                List<P07_PrintData> data = dt.AsEnumerable()
                                .Select(row1 => new P07_PrintData()
                                {
                                    Roll = row1["Roll"].ToString(),
                                    Dyelot = row1["dyelot"].ToString(),
                                    POID = row1["PoId"].ToString(),
                                    SEQ = row1["SEQ"].ToString(),
                                    BrandID = row1["BrandID"].ToString(),
                                    Desc = row1["Desc"].ToString().TrimEnd(new char[] { '\n', '\r' }),
                                    ShipQty = row1["ShipQty"].ToString(),
                                    pounit = row1["pounit"].ToString(),
                                    StockQty = row1["StockQty"].ToString(),
                                    StockUnit = row1["StockUnit"].ToString(),
                                    SubStockQty = row1["SubStockQty"].ToString(),
                                    SubQty = row1["SubQty"].ToString(),
                                    Remark = row1["Remark"].ToString()

                                }).ToList();

                e.Report.ReportDataSource = data;

            }
            else
            {

                e.Report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Date1", Date1));
                List<P07_PrintData> data = dt.AsEnumerable()
                               .Select(row1 => new P07_PrintData()
                               {
                                   POID = row1["PoId"].ToString(),
                                   SEQ = row1["SEQ"].ToString(),
                                   BrandID = row1["BrandID"].ToString(),
                                   Roll = row1["Roll"].ToString(),
                                   Desc = row1["Desc"].ToString().TrimEnd(new char[] { '\n', '\r' }),
                                   ShipQty = row1["ShipQty"].ToString(),
                                   pounit = row1["pounit"].ToString(),
                                   GW = row1["Weight"].ToString(),
                                   AW = row1["ActualWeight"].ToString(),
                                   Vaniance = row1["Vaniance"].ToString(),
                                   SubQty = row1["SubQty"].ToString(),
                                   SubGW = row1["SubGW"].ToString(),
                                   SubAW = row1["SubAW"].ToString(),
                                   SubVaniance = row1["SubVaniance"].ToString(),
                                   Remark = row1["Remark"].ToString()
                               }).ToList();
                e.Report.ReportDataSource = data;

            }
                                 
            return Result.True;
        }

        public DataRow CurrentDataRow { get; set; }

        private void radioGroup1_ValueChanged(object sender, EventArgs e)
        {
            this.ReportResourceNamespace = typeof(P07_PrintData);
            this.ReportResourceAssembly = ReportResourceNamespace.Assembly;
            this.ReportResourceName = this.radioPanel1.Value == this.radioPLRcvReport.Value ? "P07_Report1.rdlc" : "P07_Report2.rdlc";
        }

        private void radioPLRcvReport_CheckedChanged(object sender, EventArgs e)
        {
            CheckControlEnable();
        }

        private void CheckControlEnable()
        {
            if (radioPLRcvReport.Checked == true)
            {
                txtSPNo.Enabled = false;
            }
            else
            {              
                txtSPNo.Enabled = true;                                            
            }
        }

        protected override bool OnToExcel(ReportDefinition report)
        {
            if (dt.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            if (ReportResourceName == "P07_Report2.rdlc")
            {
                Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Warehouse_P07_ArriveWearhouseReport.xltx"); //預先開啟excel app
                Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表

                int nRow = 7;

                objSheets.Cells[3, 1] = Date2;
                objSheets.Cells[4, 1] = "ETA:" + ETA;
                objSheets.Cells[5, 1] = "Invoice#:" + Invoice + "   From FTY ID:" + FTYID;
                objSheets.Cells[5, 11] = "WK#:" + Wk;              
                foreach (DataRow dr in dt.Rows)
                {
                    objSheets.Cells[nRow, 1] = dr["Roll"].ToString();
                    objSheets.Cells[nRow, 2] = dr["Dyelot"].ToString();
                    objSheets.Cells[nRow, 3] = dr["PoId"].ToString();
                    objSheets.Cells[nRow, 4] = dr["SEQ"].ToString();
                    objSheets.Cells[nRow, 5] = dr["Refno"].ToString();
                    objSheets.Cells[nRow, 6] = dr["ColorID"].ToString();
                    objSheets.Cells[nRow, 7] = dr["ColorName"].ToString();
                    objSheets.Cells[nRow, 8] = dr["WeaveTypeID"].ToString();
                    objSheets.Cells[nRow, 9] = dr["BrandID"].ToString();
                    objSheets.Cells[nRow, 10] = dr["Desc"].ToString();
                    objSheets.Cells[nRow, 11] = dr["Weight"].ToString();
                    objSheets.Cells[nRow, 12] = dr["ShipQty"].ToString()+" " + dr["POUnit"].ToString();
                    objSheets.Cells[nRow, 13] = dr["ActualQty"].ToString() + " " + dr["POUnit"].ToString();
                    objSheets.Cells[nRow, 14] = dr["StockQty"].ToString() + " " + dr["StockUnit"].ToString();
                    objSheets.Cells[nRow, 15] = MyUtility.Check.Empty(dr["TotalReceivingQty"]) ?
                        string.Empty : dr["TotalReceivingQty"].ToString() + " " + dr["POUnit"].ToString();
                    objSheets.Cells[nRow, 16] = dr["QtyVaniance"].ToString();
                    objSheets.Cells[nRow, 17] = dr["Remark"].ToString();
                    nRow++;
                }

                #region Save & Show Excel
                string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Warehouse_P07_ArriveWearhouseReport");
                objApp.ActiveWorkbook.SaveAs(strExcelName);
                objApp.Quit();
                Marshal.ReleaseComObject(objApp);
                Marshal.ReleaseComObject(objSheets);

                strExcelName.OpenFile();
                #endregion
            }
            else
            {
                Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Warehouse_P07_PackingListReveivingReport.xltx"); //預先開啟excel app
                Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表

                int nRow = 7;

                objSheets.Cells[3, 1] = Date1;
                objSheets.Cells[4, 1] = "ETA:" + ETA;
                objSheets.Cells[5, 1] = "Invoice#:" + Invoice + "   From FTY ID:" + FTYID;
                objSheets.Cells[5, 10] = "WK#:" + Wk;
                foreach (DataRow dr in dt.Rows)
                {
                    objSheets.Cells[nRow, 1] = dr["Roll"].ToString();
                    objSheets.Cells[nRow, 2] = dr["Dyelot"].ToString();
                    objSheets.Cells[nRow, 3] = dr["PoId"].ToString();
                    objSheets.Cells[nRow, 4] = dr["SEQ"].ToString();
                    objSheets.Cells[nRow, 5] = dr["RefNo"].ToString();
                    objSheets.Cells[nRow, 6] = dr["BrandID"].ToString();
                    objSheets.Cells[nRow, 7] = dr["Desc"].ToString();
                    objSheets.Cells[nRow, 8] = dr["ShipQty"].ToString() + " " + dr["pounit"].ToString();
                    objSheets.Cells[nRow, 9] = dr["ActualQty"].ToString() + " " + dr["pounit"].ToString();
                    objSheets.Cells[nRow, 10] = dr["StockQty"].ToString() + " " + dr["StockUnit"].ToString();
                    objSheets.Cells[nRow, 11] = dr["Weight"].ToString();
                    objSheets.Cells[nRow, 12] = dr["ActualWeight"].ToString();
                    objSheets.Cells[nRow, 13] = dr["QtyVaniance"].ToString();
                    objSheets.Cells[nRow, 14] = dr["Remark"].ToString();
                    nRow++;
                }

                #region Save & Show Excel
                string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Warehouse_P07_PackingListReveivingReport");
                objApp.ActiveWorkbook.SaveAs(strExcelName);
                objApp.Quit();
                Marshal.ReleaseComObject(objApp);
                Marshal.ReleaseComObject(objSheets);

                strExcelName.OpenFile();
                #endregion
            }
            

            return true;
        }
    }
}
