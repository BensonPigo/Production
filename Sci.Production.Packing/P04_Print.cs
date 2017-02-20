﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.Packing
{
    public partial class P04_Print : Sci.Win.Tems.PrintForm
    {
        DataRow masterData;
        string reportType, ctn1, ctn2, destination;
        DataTable printData, ctnDim;
        public P04_Print(DataRow MasterData)
        {
            InitializeComponent();
            masterData = MasterData;
            radioButton1.Checked = true;
            ControlPrintFunction(true);
        }

        //Packing Guide Report
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            ControlPrintFunction(radioButton1.Checked);
        }

        //控制元件是否可使用
        private void ControlPrintFunction(bool isSupport)
        {
            this.IsSupportToPrint = !isSupport;
            this.IsSupportToExcel = isSupport;
            textBox1.Enabled = !isSupport;
            textBox2.Enabled = !isSupport;
            if (isSupport)
            {
                textBox1.Text = "";
                textBox2.Text = "";
            }
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            reportType = radioButton1.Checked ? "1" : "2";
            ctn1 = textBox1.Text;
            ctn2 = textBox2.Text;
            ReportResourceName = "BarcodePrint.rdlc";

            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            if (reportType == "1")
            {
                destination = MyUtility.GetValue.Lookup(string.Format("select Alias from Country WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(masterData["Dest"])));
                string sqlCmd = string.Format(@"select pd.OrderID,o.StyleID,o.Customize1,o.CustPONo,pd.CTNStartNo,pd.CTNEndNo,pd.CTNQty,pd.Article,
pd.Color,pd.SizeCode,pd.ShipQty,pd.NW,pd.GW,pd.NNW,pd.NWPerPcs,pd.NW*pd.CTNQty as TtlNW,
pd.GW*pd.CTNQty as TtlGW,pd.NNW*pd.CTNQty as TtlNNW
from PackingList_Detail pd WITH (NOLOCK) 
left join Orders o WITH (NOLOCK) on o.ID = pd.OrderID
where pd.ID = '{0}'
order by pd.Seq", MyUtility.Convert.GetString(masterData["ID"]));
                DualResult result = DBProxy.Current.Select(null, sqlCmd, out printData);
                if (!result)
                {
                    return result;
                }

                sqlCmd = string.Format(@"Declare @packinglistid VARCHAR(13),
		@refno VARCHAR(21), 
		@ctnstartno VARCHAR(6),
		@firstctnno VARCHAR(6),
		@lastctnno VARCHAR(6),
		@orirefnno VARCHAR(21),
		@insertrefno VARCHAR(13)

set @packinglistid = '{0}'

--建立暫存PackingList_Detail資料
DECLARE @tempPackingListDetail TABLE (
   RefNo VARCHAR(21),
   CTNNo VARCHAR(13)
)

--撈出PackingList_Detail
DECLARE cursor_PackingListDetail CURSOR FOR
	SELECT RefNo,CTNStartNo FROM PackingList_Detail WITH (NOLOCK) WHERE ID = @packinglistid and CTNQty > 0 and RefNo <> '' ORDER BY Seq

--開始run cursor
OPEN cursor_PackingListDetail
--將第一筆資料填入變數
FETCH NEXT FROM cursor_PackingListDetail INTO @refno, @ctnstartno
SET @firstctnno = @ctnstartno
SET @lastctnno = @ctnstartno
SET @orirefnno = @refno
WHILE @@FETCH_STATUS = 0
BEGIN
	IF(@orirefnno <> @refno)
		BEGIN
			IF(@firstctnno = @lastctnno)
				BEGIN
					SET @insertrefno = @firstctnno
				END
			ELSE
				BEGIN
					SET @insertrefno = @firstctnno + '-' + @lastctnno
				END
			INSERT INTO @tempPackingListDetail (RefNo,CTNNo) VALUES (@orirefnno,@insertrefno)

			--數值重新記錄
			SET @orirefnno = @refno
			SET @firstctnno = @ctnstartno
			SET @lastctnno = @ctnstartno
		END
	ELSE
		BEGIN
			--紀錄箱號
			SET @lastctnno = @ctnstartno
		END

	FETCH NEXT FROM cursor_PackingListDetail INTO @refno, @ctnstartno
END
--最後一筆資料
if(@insertrefno = '' OR @insertrefno IS NULL) 
SET	@insertrefno = @firstctnno
if(@orirefnno <> '')
INSERT INTO @tempPackingListDetail (RefNo,CTNNo) VALUES (@orirefnno,@insertrefno)

--關閉cursor與參數的關聯
CLOSE cursor_PackingListDetail
--將cursor物件從記憶體移除
DEALLOCATE cursor_PackingListDetail

select distinct t.RefNo,l.Description, STR(l.CtnLength,8,4)+'\'+STR(l.CtnWidth,8,4)+'\'+STR(l.CtnHeight,8,4) as Dimension, l.CtnUnit, 
(select CTNNo+',' from @tempPackingListDetail where RefNo = t.RefNo for xml path(''))as Ctn,
l.CBM*(select sum(CTNQty) from PackingList_Detail WITH (NOLOCK) where ID = @packinglistid and Refno = t.RefNo) as TtlCBM
from @tempPackingListDetail t
left join LocalItem l on l.RefNo = t.RefNo
order by RefNo", MyUtility.Convert.GetString(masterData["ID"]));
                result = DBProxy.Current.Select(null, sqlCmd, out ctnDim);
                return result;
            }
            else
            {
                DualResult result = PublicPrg.Prgs.PackingBarcodePrint(MyUtility.Convert.GetString(masterData["ID"]), ctn1, ctn2, out printData);
                if (!result)
                {
                    return result;
                }

                e.Report.ReportDataSource = printData;
            }
            return Result.True;
        }

        // 產生Excel
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            if (reportType == "1")
            {
                string strXltName = Sci.Env.Cfg.XltPathDir + "\\Packing_P04_PackingListReport.xltx";
                Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
                if (excel == null) return false;
                this.ShowWaitMessage("Starting to excel...");
                Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

                worksheet.Cells[3, 1] = MyUtility.Convert.GetString(masterData["ID"]);
                worksheet.Cells[3, 3] = MyUtility.Convert.GetString(masterData["INVNo"]);
                worksheet.Cells[3, 6] = MyUtility.Convert.GetString(masterData["ShipModeID"]);
                worksheet.Cells[3, 9] = MyUtility.Convert.GetString(masterData["ShipQty"]);
                worksheet.Cells[3, 11] = MyUtility.Convert.GetInt(masterData["CTNQty"]);
                worksheet.Cells[3, 12] = MyUtility.Convert.GetString(masterData["CBM"]);
                worksheet.Cells[3, 15] = MyUtility.Convert.GetString(masterData["Dest"]) + "-" + destination;

                //當要列印的筆數超過16筆，就要插入Row，因為範本只留16筆記錄的空間
                if (printData.Rows.Count > 16)
                {
                    for (int i = 1; i <= printData.Rows.Count - 16; i++)
                    {
                        Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range("A6:A6", Type.Missing).EntireRow;
                        rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                    }
                }

                int excelRow = 5;
                foreach (DataRow dr in printData.Rows)
                {
                    worksheet.Cells[excelRow, 1] = MyUtility.Convert.GetString(dr["OrderID"]);
                    worksheet.Cells[excelRow, 2] = MyUtility.Convert.GetString(dr["StyleID"]);
                    worksheet.Cells[excelRow, 3] = MyUtility.Convert.GetString(dr["Customize1"]);
                    worksheet.Cells[excelRow, 4] = MyUtility.Convert.GetString(dr["CustPONo"]);
                    worksheet.Cells[excelRow, 5] = MyUtility.Check.Empty(dr["CTNQty"]) ? "" : MyUtility.Convert.GetString(dr["CTNStartNo"]);
                    worksheet.Cells[excelRow, 6] = MyUtility.Convert.GetString(dr["CTNStartNo"]) == MyUtility.Convert.GetString(dr["CTNEndNo"]) ? "" : MyUtility.Convert.GetString(dr["CTNEndNo"]);
                    worksheet.Cells[excelRow, 7] = MyUtility.Check.Empty(dr["CTNQty"]) ? "" : MyUtility.Convert.GetString(dr["CTNQty"]);
                    worksheet.Cells[excelRow, 8] = MyUtility.Convert.GetString(dr["Article"]) + " " + MyUtility.Convert.GetString(dr["Color"]);
                    worksheet.Cells[excelRow, 9] = MyUtility.Convert.GetString(dr["SizeCode"]);
                    worksheet.Cells[excelRow, 10] = MyUtility.Convert.GetString(dr["ShipQty"]);
                    worksheet.Cells[excelRow, 11] = MyUtility.Check.Empty(dr["NW"]) ? "" : MyUtility.Convert.GetString(dr["NW"]);
                    worksheet.Cells[excelRow, 12] = MyUtility.Check.Empty(dr["GW"]) ? "" : MyUtility.Convert.GetString(dr["GW"]);
                    worksheet.Cells[excelRow, 13] = MyUtility.Check.Empty(dr["NNW"]) ? "" : MyUtility.Convert.GetString(dr["NNW"]);
                    worksheet.Cells[excelRow, 14] = MyUtility.Check.Empty(dr["NWPerPcs"]) ? "" : MyUtility.Convert.GetString(dr["NWPerPcs"]);
                    worksheet.Cells[excelRow, 15] = MyUtility.Check.Empty(dr["TtlNW"]) ? "" : MyUtility.Convert.GetString(dr["TtlNW"]);
                    worksheet.Cells[excelRow, 16] = MyUtility.Check.Empty(dr["TtlGW"]) ? "" : MyUtility.Convert.GetString(dr["TtlGW"]);
                    worksheet.Cells[excelRow, 17] = MyUtility.Check.Empty(dr["TtlNNW"]) ? "" : MyUtility.Convert.GetString(dr["TtlNNW"]);
                    excelRow++;
                }

                worksheet.Range[String.Format("A{0}:Q{0}", excelRow)].Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop).Weight = 2; //1: 虛線, 2:實線, 3:粗體線
                worksheet.Range[String.Format("A{0}:Q{0}", excelRow)].Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop).LineStyle = 1;
                worksheet.Cells[excelRow, 6] = "Total";

                if (excelRow > 5)
                {
                    worksheet.Cells[excelRow, 7] = string.Format("=SUM(G5:G{0})", MyUtility.Convert.GetString(excelRow - 1));
                    worksheet.Cells[excelRow, 10] = string.Format("=SUM(J5:J{0})", MyUtility.Convert.GetString(excelRow - 1));
                    worksheet.Cells[excelRow, 15] = string.Format("=SUM(O5:O{0})", MyUtility.Convert.GetString(excelRow - 1));
                    worksheet.Cells[excelRow, 16] = string.Format("=SUM(P5:P{0})", MyUtility.Convert.GetString(excelRow - 1));
                    worksheet.Cells[excelRow, 17] = string.Format("=SUM(Q5:Q{0})", MyUtility.Convert.GetString(excelRow - 1));
                }

                if (excelRow <= 21)
                {
                    excelRow = 21;
                }

                //Carton Dimension:
                excelRow++;
                StringBuilder ctnDimension = new StringBuilder();
                foreach (DataRow dr in ctnDim.Rows)
                {
                    ctnDimension.Append(string.Format("{0} - {1} - {2} {3}, (CTN#:{4}) \r\n",
                        MyUtility.Convert.GetString(dr["RefNo"]), MyUtility.Convert.GetString(dr["Description"]), MyUtility.Convert.GetString(dr["Dimension"]), MyUtility.Convert.GetString(dr["CtnUnit"]),
                        MyUtility.Convert.GetString(dr["Ctn"]).Substring(0, MyUtility.Convert.GetString(dr["Ctn"]).Length - 1)));
                }
                worksheet.Cells[excelRow, 3] = ctnDimension.Length > 0 ? ctnDimension.ToString() : "";

                //Remarks
                excelRow++;
                worksheet.Cells[excelRow, 3] = MyUtility.Convert.GetString(masterData["Remark"]);

                this.HideWaitMessage();
                excel.Visible = true;
            }
            return true;
        }
    }
}
