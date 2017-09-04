﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Sci;
using Sci.Data;
using Ict;
using Ict.Win;
using Sci.Win;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Packing
{
    public partial class P14_Print_OrderList : Sci.Win.Tems.QueryForm
    {
        DataTable dt;
        string date1, date2, packID, SPNo,_cmd;
        public P14_Print_OrderList(DataTable dt, string date1, string date2, string packID, string SPNo,string cmd)
        {
            
            this.dt = dt;
            this.date1 = date1;
            this.date2 = date2;
            this.packID = packID;
            this.SPNo = SPNo;
            _cmd = cmd;
            InitializeComponent();
            EditMode = true;
        }
        string TransferSlipNo;
        private bool ToExcel()
        {
            if (radioTransferList.Checked)
            {
                DataTable dt2 = dt.Copy();
                dt2.Columns.Remove("tid");
                #region ListCheck
                toExcel("Packing_P14.xltx", 4, dt2);
                #endregion
            }
            if (radioTransferSlip.Checked)
            {
                #region SlipCheck
                TransferSlipNo = MyUtility.GetValue.GetID(Sci.Env.User.Keyword + "TC", "TransferToClog", DateTime.Today, 2, "TransferSlipNo", null);
                #region 存TransferSlipNo
                DataTable a;
                string update_TransferSlipNo = string.Format(@"
update t set TransferSlipNo = '{0}'
from TransferToClog t
where id in(select tid from #tmp where isnull(TransferSlipNo,'') = '')

--回去找出TransferSlipNo包含的資料
select distinct TransferSlipNo into #tmp_TransferSlipNo from TransferToClog t where id in(select distinct tid from #tmp)

select  *
        , rn = ROW_NUMBER() over(order by Id,OrderID,(RIGHT(REPLICATE('0', 6) + rtrim(ltrim(CTNStartNo)), 6)))
        , rn1 = ROW_NUMBER() over(order by TRY_CONVERT(int, CTNStartNo) ,(RIGHT(REPLICATE('0', 6) + rtrim(ltrim(CTNStartNo)), 6)))
from (
    select  1 as selected
            , t.TransferDate
            , t.TransferSlipNo
            , t.PackingListID
            , t.OrderID
            , t.CTNStartNo
            , pd.Id
            , isnull(o.StyleID,'') as StyleID,isnull(o.BrandID,'') as BrandID,isnull(o.Customize1,'') as Customize1
            , isnull(o.CustPONo,'') as CustPONo,isnull(c.Alias,'') as Dest
            , isnull(o.FactoryID,'') as FactoryID
            , convert(varchar, oq.BuyerDelivery, 111) as BuyerDelivery
            , t.AddDate
			, tid = t.id
    from TransferToClog t WITH (NOLOCK) 
    left join Orders o WITH (NOLOCK) on t.OrderID =  o.ID
    left join Country c WITH (NOLOCK) on o.Dest = c.ID
    left join PackingList_Detail pd WITH (NOLOCK) on  pd.ID = t.PackingListID 
                                                        and pd.OrderID = t.OrderID 
                                                        and pd.CTNStartNo = t.CTNStartNo 
                                                        and pd.CTNQty > 0
    left join Order_QtyShip oq WITH (NOLOCK) on  oq.Id = pd.OrderID 
                                                    and oq.Seq = pd.OrderShipmodeSeq
    where t.MDivisionID = 'MAI' and t.TransferSlipNo in (select TransferSlipNo from #tmp_TransferSlipNo)
) X order by rn

", TransferSlipNo, _cmd);
                MyUtility.Tool.ProcessWithDatatable(dt, "tid,TransferSlipNo", update_TransferSlipNo, out a);
                #endregion
                DataTable b;
                MyUtility.Tool.ProcessWithDatatable(a, @"Selected,TransferSlipNo,TransferDate,PackingListID,OrderID,CTNStartNo,StyleID,BrandID,Customize1,CustPONo,Dest,FactoryID,BuyerDelivery,AddDate,tid",
                 @"select TransferDate,TransferSlipNo,PackingListID,OrderID,CTNStartNo,StyleID,BrandID,Customize1,CustPONo,Dest,FactoryID,BuyerDelivery,AddDate,tid from #tmp", out b);

                var Slip = (from p in b.AsEnumerable()
                            group p by new
                            {
                                PackingListID = p["PackingListID"].ToString(),
                                OrderID = p["OrderID"].ToString(),
                                TransferSlipNo = p["TransferSlipNo"].ToString()
                            } into m
                            select new PackData
                            {
                                TTL_Qty = m.Count(r => !r["CTNStartNo"].Empty()).ToString(),
                                PackID = m.First()["PackingListID"].ToString(),
                                OrderID = m.First()["OrderID"].ToString(),
                                PONo = m.First()["CustPONo"].ToString(),
                                Dest = m.First()["Dest"].ToString(),
                                BuyerDelivery = m.First()["BuyerDelivery"].ToString(),
                                CartonNum = string.Join(", ", m.Select(r => r["CTNStartNo"].ToString().Trim())),
                                TransferSlipNo = m.First()["TransferSlipNo"].ToString()
                            }).ToList();
                //
                string sql = @"
select  t.TTL_Qty, 
        t.PackID, 
        t.OrderID, 
        t.PONo, 
        ttlCtn = (select count(*) from PackingList_detail pk WITH (NOLOCK) where t.PackID = pk.ID and t.OrderID = pk.OrderID and ctnQty > 0),
        a.ClogLocationId,
        t.Dest,
        t.BuyerDelivery,
        t.CartonNum,
        t.TransferSlipNo
from  #Tmp t
outer apply(
	select ClogLocationId = stuff((
		select concat(',',ClogLocationId)
		from(
			select distinct pld.ClogLocationId
			from PackingList_Detail pld
			where pld.ID = t.PackID and pld.ClogLocationId !=''
		)dis
		for xml path('')
	),1,1,'')
)a
            ";                
                DataTable k;
                MyUtility.Tool.ProcessWithObject(Slip, "", sql, out k);
                toExcel("Packing_P14_TransferSlip.xltx", 4, k);
                #endregion
            }
            return true;
        }

        private void toExcel(string xltFile, int headerRow, DataTable ExcelTable)
        {
            string strExcelProcessName = "";
            if (ExcelTable == null || ExcelTable.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("No data!!");
                return;
            }
            this.ShowWaitMessage("Excel Processing...");

            Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\" + xltFile); //預先開啟excel app
            //objApp.Visible = true;
            

            if (xltFile.EqualString("Packing_P14_TransferSlip.xltx"))
            {
                
                Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
                
                #region Set Login M & Transfer Date

                string strLoginM = MyUtility.GetValue.Lookup(string.Format("select NameEN from Factory where ID = '{0}'", Sci.Env.User.Keyword));
                if (!strLoginM.Empty())
                {
                    objSheets.Cells[1, 1] = strLoginM;
                }
                #endregion
                if (date1 != null && date2 != null)
                    objSheets.Cells[3, 8] = date1 + " ~ " + date2;
                //TransferSlipNo欄位 distinct
                DataTable myDT = ExcelTable.DefaultView.ToTable(true, new string[] { "TransferSlipNo" });
                //依據TransferSlipNo數量複製sheet
                int countsheet = myDT.Rows.Count;
                for (int i = 0; i < countsheet; i++)
                {
                    if (i > 0)
                    {
                        Microsoft.Office.Interop.Excel.Worksheet worksheet1 = ((Microsoft.Office.Interop.Excel.Worksheet)objApp.ActiveWorkbook.Worksheets[1]);
                        Microsoft.Office.Interop.Excel.Worksheet worksheetn = ((Microsoft.Office.Interop.Excel.Worksheet)objApp.ActiveWorkbook.Worksheets[i + 1]);
                        worksheet1.Copy(worksheetn);
                    }
                }
                int j = 1;
                foreach (DataRow dr in myDT.Rows)
                {
                    objSheets = objApp.ActiveWorkbook.Worksheets[j];   // 取得工作表
                    objSheets.Name = dr["TransferSlipNo"].ToString();//工作表名稱
                    objSheets.Cells[3, 3] = dr["TransferSlipNo"].ToString();
                    DataTable pdt = ExcelTable.Select(string.Format("TransferSlipNo = '{0}'", dr["TransferSlipNo"].ToString())).CopyToDataTable();

                    pdt.Columns.Remove("TransferSlipNo");
                    bool result = MyUtility.Excel.CopyToXls(pdt, "", showExcel: false, xltfile: xltFile, headerRow: headerRow, excelApp: objApp, wSheet: objSheets);
                    if (!result)
                    {
                        MyUtility.Msg.WarningBox(result.ToString(), "Warning");
                        return;
                    }
                    decimal sumTTL = 0;
                    int r = pdt.Rows.Count;
                    for (int i = 1; i <= r; i++)
                    {
                        sumTTL += Convert.ToDecimal(pdt.Rows[i - 1]["TTL_Qty"]);
                        string str = objSheets.Cells[i + headerRow, 8].Value;
                        if (!MyUtility.Check.Empty(str))
                            objSheets.Cells[i + headerRow, 8] = str.Trim();
                    }

                    objSheets.get_Range(string.Format("A5:I{0}", r + 4)).Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;

                    objSheets.Cells[r + headerRow + 1, 1] = "Sub. TTL CTN:";
                    objSheets.Cells[r + headerRow + 1, 2] = sumTTL;
                    objSheets.get_Range("F1:F1").ColumnWidth = 20;
                    objSheets.get_Range("I1:I1").ColumnWidth = 70;
                    objSheets.Rows.AutoFit();

                    j++;
                }

                
                strExcelProcessName = "Packing_P14_TransferSlip";
                #region Save & Show Excel
                string strExcelName = Sci.Production.Class.MicrosoftFile.GetName(strExcelProcessName);
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

                if (objSheets != null) Marshal.FinalReleaseComObject(objSheets);    //釋放sheet
                if (objApp != null) Marshal.FinalReleaseComObject(objApp);          //釋放objApp
            }
            if (xltFile.EqualString("Packing_P14.xltx"))
            {
                bool result = MyUtility.Excel.CopyToXls(ExcelTable, "", showExcel: false, xltfile: xltFile, headerRow: headerRow, excelApp: objApp);
                Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表

                if (!result)
                {
                    MyUtility.Msg.WarningBox(result.ToString(), "Warning");
                    return;
                }

                #region Set Login M & Transfer Date

                string strLoginM = MyUtility.GetValue.Lookup(string.Format("select NameEN from Factory where ID = '{0}'", Sci.Env.User.Keyword));
                if (!strLoginM.Empty())
                {
                    objSheets.Cells[1, 1] = strLoginM;
                }
                #endregion
                if (date1 != null && date2 != null)
                    objSheets.Cells[3, 2] = date1 + " ~ " + date2;
                strExcelProcessName = "Packing_P14";
                int r = ExcelTable.Rows.Count;
                objSheets.get_Range(string.Format("A5:M{0}", r + 4)).Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                objSheets.Columns.AutoFit();
                objSheets.Rows.AutoFit();
                #region Save & Show Excel
                string strExcelName = Sci.Production.Class.MicrosoftFile.GetName(strExcelProcessName);
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

                if (objSheets != null) Marshal.FinalReleaseComObject(objSheets);    //釋放sheet
                if (objApp != null) Marshal.FinalReleaseComObject(objApp);          //釋放objApp
            }
            
        }

        private void btnToExcel_Click(object sender, EventArgs e)
        {
            ToExcel();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
        
        public class PackData
        {
            public string TTL_Qty { get; set; }            
            public string TransferSlipNo { get; set; }
            public string PackID { get; set; }
            public string OrderID { get; set; }
            public string PONo { get; set; }
            public string Dest { get; set; }
            public string BuyerDelivery { get; set; }
            public string CartonNum { get; set; }
        }
    }
}
