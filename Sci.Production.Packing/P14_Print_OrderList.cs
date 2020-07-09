using System;
using System.Collections.Generic;
using System.Data;
using Sci;
using Ict;
using Ict.Win;
using Sci.Win;
using System.Linq;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;

namespace Sci.Production.Packing
{
    /// <summary>
    /// Packing_P14_Print_OrderList
    /// </summary>
    public partial class P14_Print_OrderList : Win.Tems.QueryForm
    {
        private DataTable dt;
        private string date1;
        private string date2;
        private string packID;
        private string SPNo;
        private string _cmd;

        /// <summary>
        /// P14_Print_OrderList
        /// </summary>
        /// <param name="dt">dt</param>
        /// <param name="date1">date1</param>
        /// <param name="date2">date2</param>
        /// <param name="packID">packID</param>
        /// <param name="sPNo">SPNo</param>
        /// <param name="cmd">cmd</param>
        public P14_Print_OrderList(DataTable dt, string date1, string date2, string packID, string sPNo, string cmd)
        {
            this.dt = dt;
            this.date1 = date1;
            this.date2 = date2;
            this.packID = packID;
            this.SPNo = sPNo;
            this._cmd = cmd;
            this.InitializeComponent();
            this.EditMode = true;
        }

        private string TransferSlipNo;

        /// <summary>
        /// ToExcel
        /// </summary>
        /// <returns>bool</returns>
        private bool ToExcel()
        {
            if (this.radioTransferList.Checked)
            {
                DataTable dt2 = this.dt.Copy();
                dt2.Columns.Remove("tid");
                #region ListCheck
                this.ToExcel1("Packing_P14.xltx", 4, dt2);
                #endregion
            }

            if (this.radioTransferSlip.Checked)
            {
                #region SlipCheck
                this.TransferSlipNo = MyUtility.GetValue.GetID(Env.User.Keyword + "TC", "TransferToClog", DateTime.Today, 2, "TransferSlipNo", null);
                #region 存TransferSlipNo
                DataTable a;
                string update_TransferSlipNo = string.Format(
                    @"
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
    where t.MDivisionID = '{1}' and t.TransferSlipNo in (select TransferSlipNo from #tmp_TransferSlipNo)
) X order by rn

",
                    this.TransferSlipNo,
                    Env.User.Keyword);
                MyUtility.Tool.ProcessWithDatatable(this.dt, "tid,TransferSlipNo", update_TransferSlipNo, out a);
                #endregion
                DataTable b;
                string sqlcmd = @"select TransferDate,TransferSlipNo,PackingListID,OrderID,CTNStartNo,StyleID,BrandID,Customize1,CustPONo,Dest,FactoryID,BuyerDelivery,AddDate,tid from #tmp";

                MyUtility.Tool.ProcessWithDatatable(a, @"Selected,TransferSlipNo,TransferDate,PackingListID,OrderID,CTNStartNo,StyleID,BrandID,Customize1,CustPONo,Dest,FactoryID,BuyerDelivery,AddDate,tid", sqlcmd, out b);

                var slip = (from p in b.AsEnumerable()
                            group p by new
                            {
                                PackingListID = p["PackingListID"].ToString(),
                                OrderID = p["OrderID"].ToString(),
                                TransferSlipNo = p["TransferSlipNo"].ToString(),
                            }

into m
                            select new PackData
                            {
                                TTL_Qty = m.Count(r => !r["CTNStartNo"].Empty()).ToString(),
                                PackID = m.First()["PackingListID"].ToString(),
                                OrderID = m.First()["OrderID"].ToString(),
                                PONo = m.First()["CustPONo"].ToString(),
                                Dest = m.First()["Dest"].ToString(),
                                BuyerDelivery = m.First()["BuyerDelivery"].ToString(),
                                CartonNum = string.Join(", ", m.Select(r => r["CTNStartNo"].ToString().Trim())),
                                TransferSlipNo = m.First()["TransferSlipNo"].ToString(),
                            }).ToList();
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
                MyUtility.Tool.ProcessWithObject(slip, string.Empty, sql, out k);
                this.ToExcel1("Packing_P14_TransferSlip.xltx", 4, k);
                #endregion
            }

            return true;
        }

        private void ToRdlc()
        {
            string strLoginM = MyUtility.GetValue.Lookup(string.Format("select NameEN from MDivision where ID = '{0}'", Env.User.Keyword));
            ReportDefinition report = new ReportDefinition();
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Title", strLoginM));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("TransferDate", this.date1 + " ~ " + this.date2));
            Type reportResourceNamespace;
            string reportResourceName;
            if (this.radioTransferList.Checked)
            {
                #region List
                DataTable dt2 = this.dt.Copy();
                dt2.Columns.Remove("tid");

                List<P14_PrintData> data = dt2.AsEnumerable()
               .Select(row1 => new P14_PrintData()
               {
                   TransferDate = ((DateTime)row1["TransferDate"]).ToString("yyyy/MM/dd").Trim(),
                   TransferSlipNo = row1["TransferSlipNo"].ToString().Trim(),
                   PackingListID = row1["PackingListID"].ToString().Trim(),
                   OrderID = row1["OrderID"].ToString().Trim(),
                   CTNStartNo = row1["CTNStartNo"].ToString().Trim(),
                   StyleID = row1["StyleID"].ToString().Trim(),
                   BrandID = row1["BrandID"].ToString().Trim(),
                   Customize1 = row1["Customize1"].ToString().Trim(),
                   CustPONo = row1["CustPONo"].ToString().Trim(),
                   Dest = row1["Dest"].ToString().Trim(),
                   Factory = row1["FactoryID"].ToString().Trim(),
                   BuyerDelivery = row1["BuyerDelivery"].ToString().Trim(),
                   AddDate = ((DateTime)row1["AddDate"]).ToString("yyyy/MM/dd").Trim(),
               }).ToList();

                report.ReportDataSource = data;

                reportResourceNamespace = typeof(P14_PrintData);
                reportResourceName = "P14_Print.rdlc";
                #endregion
            }
            else
            {
                #region SlipCheck
                this.TransferSlipNo = MyUtility.GetValue.GetID(Env.User.Keyword + "TC", "TransferToClog", DateTime.Today, 2, "TransferSlipNo", null);
                #region 存TransferSlipNo
                DataTable dtTransferSlipNo, dtTransferSlipNoDetail, dtFinal;
                string sqlcmd = string.Format(
                    @"
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
            , b.TTL_PCS
            , isnull(o.FactoryID,'') as FactoryID
            , convert(varchar, oq.BuyerDelivery, 111) as BuyerDelivery
            , t.AddDate
			, tid = t.id
            , o.SeasonID
    from TransferToClog t WITH (NOLOCK) 
    left join Orders o WITH (NOLOCK) on t.OrderID =  o.ID
    left join Country c WITH (NOLOCK) on o.Dest = c.ID
    left join PackingList_Detail pd WITH (NOLOCK) on  pd.ID = t.PackingListID 
                                                        and pd.OrderID = t.OrderID 
                                                        and pd.CTNStartNo = t.CTNStartNo 
                                                        and pd.CTNQty > 0
    left join Order_QtyShip oq WITH (NOLOCK) on  oq.Id = pd.OrderID 
                                                    and oq.Seq = pd.OrderShipmodeSeq
    outer apply(
	    select [TTL_PCS]=ISNULL(SUM(shipQty),0)
	    from PackingList_Detail pld
	    where pld.ID = t.PackingListID and pld.CTNStartNo = t.CTNStartNo 
    )b
    where t.MDivisionID = '{1}' and t.TransferSlipNo in (select TransferSlipNo from #tmp_TransferSlipNo)
) X order by rn

",
                    this.TransferSlipNo,
                    Env.User.Keyword);
                MyUtility.Tool.ProcessWithDatatable(this.dt, "tid,TransferSlipNo", sqlcmd, out dtTransferSlipNo);
                #endregion
                sqlcmd = @"
select TransferDate,TransferSlipNo,PackingListID,OrderID,CTNStartNo,StyleID,BrandID,Customize1,CustPONo,Dest,TTL_PCS,FactoryID,BuyerDelivery,AddDate,tid,SeasonID
from #tmp";

                MyUtility.Tool.ProcessWithDatatable(dtTransferSlipNo, @"Selected,TransferSlipNo,TransferDate,PackingListID,OrderID,CTNStartNo,StyleID,BrandID,Customize1,CustPONo,Dest,TTL_PCS,FactoryID,BuyerDelivery,AddDate,tid,SeasonID", sqlcmd, out dtTransferSlipNoDetail);

                List<PackData> slip = (from p in dtTransferSlipNoDetail.AsEnumerable()
                            group p by new
                            {
                                PackingListID = p["PackingListID"].ToString(),
                                OrderID = p["OrderID"].ToString(),
                                TransferSlipNo = p["TransferSlipNo"].ToString(),
                            }

                            into m
                            select new PackData
                            {
                                TTL_Qty = m.Count(r => !r["CTNStartNo"].Empty()).ToString(),
                                PackID = m.First()["PackingListID"].ToString(),
                                OrderID = m.First()["OrderID"].ToString(),
                                PONo = m.First()["CustPONo"].ToString(),
                                Dest = m.First()["Dest"].ToString(),
                                TTL_PCS = m.Sum(st => st.Field<int>("TTL_PCS")).ToString(),
                                BuyerDelivery = m.First()["BuyerDelivery"].ToString(),
                                CartonNum = string.Join(", ", m.Select(r => r["CTNStartNo"].ToString().Trim())),
                                TransferSlipNo = m.First()["TransferSlipNo"].ToString(),
                                Customize1 = m.First()["Customize1"].ToString(),
                                SeasonID = m.First()["SeasonID"].ToString(),
                            }).ToList();

                sqlcmd = @"
select  t.TTL_Qty, 
        t.PackID, 
        t.OrderID, 
        t.PONo, 
        ttlCtn = (select count(*) from PackingList_detail pk WITH (NOLOCK) where t.PackID = pk.ID and t.OrderID = pk.OrderID and ctnQty > 0),
        a.ClogLocationId,
        t.Dest,
        t.TTL_PCS,
        t.BuyerDelivery,
        t.CartonNum,
        t.TransferSlipNo,        
        isnull(t.Customize1,'') as Customize1,
        t.SeasonID
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
                MyUtility.Tool.ProcessWithObject(slip, string.Empty, sqlcmd, out dtFinal);

                List<P14_PrintData_SLIP> data = dtFinal.AsEnumerable()
              .Select(row1 => new P14_PrintData_SLIP()
              {
                  TTL_Qty = row1["TTL_Qty"].ToString().Trim(),
                  PackID = row1["PackID"].ToString().Trim(),
                  OrderID = row1["OrderID"].ToString().Trim(),
                  PONo = row1["PONo"].ToString().Trim(),
                  ttlCtn = row1["ttlCtn"].ToString().Trim(),
                  ClogLocationId = row1["ClogLocationId"].ToString().Trim(),
                  Dest = row1["Dest"].ToString().Trim(),
                  TTL_PCS = row1["TTL_PCS"].ToString().Trim(),
                  BuyerDelivery = row1["BuyerDelivery"].ToString().Trim(),
                  CartonNum = row1["CartonNum"].ToString().Trim(),
                  TransferSlipNo = row1["TransferSlipNo"].ToString().Trim(),
                  Customize1 = row1["Customize1"].ToString().Trim(),
                  SeasonID = row1["SeasonID"].ToString().Trim(),
              }).ToList();

                report.ReportDataSource = data;

                reportResourceNamespace = typeof(P14_PrintData_SLIP);

                reportResourceName = "P14_Print_SLIP.rdlc";
                #endregion
            }

            Assembly reportResourceAssembly = reportResourceNamespace.Assembly;
            IReportResource reportresource;
            ReportResources.ByEmbeddedResource(reportResourceAssembly, reportResourceNamespace, reportResourceName, out reportresource);

            report.ReportResource = reportresource;

            // 開啟 report view
            var frm = new Win.Subs.ReportView(report);
            frm.MdiParent = this.MdiParent;
            frm.Show();
        }

        private void ToExcel1(string xltFile, int headerRow, DataTable excelTable)
        {
            string strExcelProcessName = string.Empty;
            if (excelTable == null || excelTable.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("No data!!");
                return;
            }

            this.ShowWaitMessage("Excel Processing...");

            Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\" + xltFile);

            // 預先開啟excel app
            // objApp.Visible = true;
            if (xltFile.EqualString("Packing_P14_TransferSlip.xltx"))
            {
                Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表

                #region Set Login M & Transfer Date

                string strLoginM = MyUtility.GetValue.Lookup(string.Format("select NameEN from Factory where ID = '{0}'", Env.User.Keyword));
                if (!strLoginM.Empty())
                {
                    objSheets.Cells[1, 1] = strLoginM;
                }
                #endregion
                if (this.date1 != null && this.date2 != null)
                {
                    objSheets.Cells[3, 8] = this.date1 + " ~ " + this.date2;
                }

                // TransferSlipNo欄位 distinct
                DataTable myDT = excelTable.DefaultView.ToTable(true, new string[] { "TransferSlipNo" });

                // 依據TransferSlipNo數量複製sheet
                int countsheet = myDT.Rows.Count;
                for (int i = 0; i < countsheet; i++)
                {
                    if (i > 0)
                    {
                        Excel.Worksheet worksheet1 = (Excel.Worksheet)objApp.ActiveWorkbook.Worksheets[1];
                        Excel.Worksheet worksheetn = (Excel.Worksheet)objApp.ActiveWorkbook.Worksheets[i + 1];
                        worksheet1.Copy(worksheetn);
                    }
                }

                int j = 1;
                foreach (DataRow dr in myDT.Rows)
                {
                    objSheets = objApp.ActiveWorkbook.Worksheets[j];   // 取得工作表
                    objSheets.Name = dr["TransferSlipNo"].ToString(); // 工作表名稱
                    objSheets.Cells[3, 3] = dr["TransferSlipNo"].ToString();
                    DataTable pdt = excelTable.Select(string.Format("TransferSlipNo = '{0}'", dr["TransferSlipNo"].ToString())).CopyToDataTable();

                    pdt.Columns.Remove("TransferSlipNo");
                    bool result = MyUtility.Excel.CopyToXls(pdt, string.Empty, showExcel: false, xltfile: xltFile, headerRow: headerRow, excelApp: objApp, wSheet: objSheets);
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
                        {
                            objSheets.Cells[i + headerRow, 8] = str.Trim();
                        }
                    }

                    objSheets.get_Range(string.Format("A5:I{0}", r + 4)).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                    objSheets.Cells[r + headerRow + 1, 1] = "Sub. TTL CTN:";
                    objSheets.Cells[r + headerRow + 1, 2] = sumTTL;
                    objSheets.get_Range("F1:F1").ColumnWidth = 20;
                    objSheets.get_Range("I1:I1").ColumnWidth = 70;
                    objSheets.Rows.AutoFit();

                    j++;
                }

                strExcelProcessName = "Packing_P14_TransferSlip";
                #region Save & Show Excel
                string strExcelName = Class.MicrosoftFile.GetName(strExcelProcessName);
                Excel.Workbook workbook = objApp.ActiveWorkbook;
                workbook.SaveAs(strExcelName);
                workbook.Close();
                objApp.Quit();
                Marshal.ReleaseComObject(objApp);
                Marshal.ReleaseComObject(objSheets);
                Marshal.ReleaseComObject(workbook);

                strExcelName.OpenFile();
                #endregion
                this.HideWaitMessage();

                if (objSheets != null)
                {
                    Marshal.FinalReleaseComObject(objSheets);    // 釋放sheet
                }

                if (objApp != null)
                {
                    Marshal.FinalReleaseComObject(objApp);          // 釋放objApp
                }
            }

            if (xltFile.EqualString("Packing_P14.xltx"))
            {
                bool result = MyUtility.Excel.CopyToXls(excelTable, string.Empty, showExcel: false, xltfile: xltFile, headerRow: headerRow, excelApp: objApp);
                Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表

                if (!result)
                {
                    MyUtility.Msg.WarningBox(result.ToString(), "Warning");
                    return;
                }

                #region Set Login M & Transfer Date

                string strLoginM = MyUtility.GetValue.Lookup(string.Format("select NameEN from Factory where ID = '{0}'", Env.User.Keyword));
                if (!strLoginM.Empty())
                {
                    objSheets.Cells[1, 1] = strLoginM;
                }
                #endregion
                if (this.date1 != null && this.date2 != null)
                {
                    objSheets.Cells[3, 2] = this.date1 + " ~ " + this.date2;
                }

                strExcelProcessName = "Packing_P14";
                int r = excelTable.Rows.Count;
                objSheets.get_Range(string.Format("A5:M{0}", r + 4)).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                objSheets.Columns.AutoFit();
                objSheets.Rows.AutoFit();
                #region Save & Show Excel
                string strExcelName = Class.MicrosoftFile.GetName(strExcelProcessName);
                Excel.Workbook workbook = objApp.ActiveWorkbook;
                workbook.SaveAs(strExcelName);
                workbook.Close();
                objApp.Quit();
                Marshal.ReleaseComObject(objApp);
                Marshal.ReleaseComObject(objSheets);
                Marshal.ReleaseComObject(workbook);

                strExcelName.OpenFile();
                #endregion
                this.HideWaitMessage();

                if (objSheets != null)
                {
                    Marshal.FinalReleaseComObject(objSheets);    // 釋放sheet
                }

                if (objApp != null)
                {
                    Marshal.FinalReleaseComObject(objApp);          // 釋放objApp
                }
            }
        }

        private void BtnToExcel_Click(object sender, EventArgs e)
        {
            // this.ToExcel();
            this.ToRdlc();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// PackData
        /// </summary>
        public class PackData
        {
            /// <summary>
            /// TTL_Qty
            /// </summary>
            public string TTL_Qty { get; set; }

            /// <summary>
            /// TransferSlipNo
            /// </summary>
            public string TransferSlipNo { get; set; }

            /// <summary>
            /// PackID
            /// </summary>
            public string PackID { get; set; }

            /// <summary>
            /// OrderID
            /// </summary>
            public string OrderID { get; set; }

            /// <summary>
            /// PONo
            /// </summary>
            public string PONo { get; set; }

            /// <summary>
            /// Dest
            /// </summary>
            public string Dest { get; set; }

            /// <summary>
            /// Dest
            /// </summary>
            public string TTL_PCS { get; set; }

            /// <summary>
            /// BuyerDelivery
            /// </summary>
            public string BuyerDelivery { get; set; }

            /// <summary>
            /// CartonNum
            /// </summary>
            public string CartonNum { get; set; }

            /// <summary>
            /// Customize1
            /// </summary>
            public string Customize1 { get; set; }

            /// <summary>
            /// SeasonID
            /// </summary>
            public string SeasonID { get; set; }
        }
    }
}
