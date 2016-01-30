using System;
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
    public partial class P03_Print : Sci.Win.Tems.PrintForm
    {
        DataRow masterData;
        int orderQty;
        string reportType, ctn1, ctn2, specialInstruction;
        DataTable printData, ctnDim, qtyCtn, articleSizeTtlShipQty, printGroupData, clipData, qtyBDown;
        public P03_Print(DataRow MasterData, int OrderQty)
        {
            InitializeComponent();
            masterData = MasterData;
            orderQty = OrderQty;
            radioButton1.Checked = true;
            ControlPrintFunction(false);
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            ControlPrintFunction(radioButton4.Checked);
        }

        //控制元件是否可使用
        private void ControlPrintFunction(bool isSupport)
        {
            this.IsSupportToPrint = isSupport;
            this.IsSupportToExcel = !isSupport;
            textBox1.Enabled = isSupport;
            textBox2.Enabled = isSupport;
            if (!isSupport)
            {
                textBox1.Text = "";
                textBox2.Text = "";
            }
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            reportType = radioButton1.Checked ? "1" : radioButton2.Checked ? "2" : radioButton3.Checked ? "3" : "4";
            ctn1 = textBox1.Text;
            ctn2 = textBox2.Text;

            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            if (reportType == "1" || reportType == "2")
            {
                return QueryPrintData();
            }
            else if (reportType == "3")
            {
                DualResult result = PublicPrg.Prgs.QueryPackingGuideReportData(MyUtility.Convert.GetString(masterData["ID"]), out printData, out ctnDim, out qtyCtn, out articleSizeTtlShipQty, out printGroupData, out clipData, out specialInstruction);
                return result;
            }
            else
            {
            }
            return Result.True;
        }

        private DualResult QueryPrintData()
        {
            string sqlCmd = string.Format(@"with tmpGroup
as
(
select OrderID,OrderShipmodeSeq,Article,Color,SizeCode,QtyPerCTN,NW,GW,NNW,NWPerPcs,min(Seq) as MinSeq, max(Seq) as MaxSeq 
from PackingList_Detail 
where ID = '{0}'
group by OrderID,OrderShipmodeSeq,Article,Color,SizeCode,QtyPerCTN,NW,GW,NNW,NWPerPcs
)
select t.*,o.StyleID,o.Customize1,o.CustPONo,c.Alias,oq.EstPulloutDate,os.SizeSpec,isnull(o.MarkFront,'') as MarkFront,
isnull(o.MarkBack,'') as MarkBack,isnull(o.MarkLeft,'') as MarkLeft,isnull(o.MarkRight,'') as MarkRight,
(select sum(CTNQty) from PackingList_Detail where Id = '{0}' and ReceiveDate is not null) as InClogQty,
(select CTNStartNo from PackingList_Detail where ID = '{0}' and Seq = t.MinSeq) as CTNStartNo,
(select CTNStartNo from PackingList_Detail where ID = '{0}' and Seq = t.MaxSeq) as CTNEndNo
from tmpGroup t
left join Orders o on o.ID = t.OrderID
left join Order_QtyShip oq on oq.Id = t.OrderID and oq.Seq = t.OrderShipmodeSeq
left join Country c on c.ID = o.Dest
left join Order_SizeSpec os on os.Id = o.POID and SizeItem = 'S01' and os.SizeCode = t.SizeCode
order by MinSeq", MyUtility.Convert.GetString(masterData["ID"]));
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
	SELECT RefNo,CTNStartNo FROM PackingList_Detail WHERE ID = @packinglistid and CTNQty > 0 ORDER BY Seq

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
INSERT INTO @tempPackingListDetail (RefNo,CTNNo) VALUES (@orirefnno,@insertrefno)
--關閉cursor與參數的關聯
CLOSE cursor_PackingListDetail
--將cursor物件從記憶體移除
DEALLOCATE cursor_PackingListDetail

select distinct t.RefNo,l.Description, STR(l.CtnLength,8,4)+'\'+STR(l.CtnWidth,8,4)+'\'+STR(l.CtnHeight,8,4) as Dimension, l.CtnUnit, 
(select CTNNo+',' from @tempPackingListDetail where RefNo = t.RefNo for xml path(''))as Ctn,
l.CBM*(select sum(CTNQty) from PackingList_Detail where ID = @packinglistid and Refno = t.RefNo) as TtlCBM
from @tempPackingListDetail t
left join LocalItem l on l.RefNo = t.RefNo
order by RefNo", MyUtility.Convert.GetString(masterData["ID"]));
            result = DBProxy.Current.Select(null, sqlCmd, out ctnDim);
            if (!result)
            {
                return result;
            }

            sqlCmd = string.Format(@"DECLARE @packinglistid VARCHAR(13),
		@orderid VARCHAR(13),
		@sizecode VARCHAR(8),
		@article VARCHAR(8),
		@dataseq VARCHAR(2),
		@sizecount DECIMAL,
		@poid VARCHAR(13),
		@tmpdatalist VARCHAR(160),
		@datalen INT,
		@tmpdata VARCHAR(9),
		@reporttype INT,
		@tmpdata2 VARCHAR(160),
		@qty INT,
		@cbm FLOAT

SET @packinglistid = '{0}'
SET @reporttype = {1} --1:for Adidas/UA/Saucony/NB, 2:for LLL/TNF

select distinct @orderid = OrderID from PackingList_Detail where ID = @packinglistid
select @sizecount = count(distinct SizeCode) from PackingList_Detail where ID = @packinglistid
select @poid = POID from Orders where ID = @orderid

--撈出此次出貨的Size Code
DECLARE cursor_SizeData CURSOR FOR
	SELECT distinct rtrim(pd.SizeCode),os.Seq 
	FROM PackingList_Detail pd
	LEFT JOIN Order_SizeCode os on os.Id = @poid and os.SizeCode = pd.SizeCode
	WHERE pd.ID = @packinglistid
	order by os.Seq

--撈出此次出貨的Article
DECLARE cursor_ArticleData CURSOR FOR
	SELECT distinct rtrim(pd.Article),oa.Seq 
	FROM PackingList_Detail pd
	LEFT JOIN Order_Article oa on oa.id = pd.OrderID and oa.Article = pd.Article
	WHERE pd.ID = @packinglistid
	order by oa.Seq

--建立暫存PackingList_Detail資料
DECLARE @tempQtyBDown TABLE (
   DataList VARCHAR(160)
)

--填入Size Code
SET @tmpdatalist = '        '
OPEN cursor_SizeData
FETCH NEXT FROM cursor_SizeData INTO @sizecode, @dataseq
WHILE @@FETCH_STATUS = 0
BEGIN
	BEGIN
		SET @datalen = LEN(@sizecode)
		SET @tmpdata = IIF(@datalen = 1,'     ',IIF(@datalen = 2 or @datalen = 3,'    ',IIF(@datalen = 4,'   ',IIF(@datalen = 5 or @datalen = 6,'  ','')))) + @sizecode + IIF(@datalen = 1 or @datalen = 2,'    ',IIF(@datalen = 3 or @datalen = 4 or @datalen = 5,'   ',IIF(@datalen = 6 or @datalen = 7,'  ',' ')))
		SET @tmpdatalist = @tmpdatalist  + @tmpdata
	END
	FETCH NEXT FROM cursor_SizeData INTO @sizecode, @dataseq
END
CLOSE cursor_SizeData

IF(@reporttype = 1)
	SET @tmpdata2 = '   Total'
ELSE
	SET @tmpdata2 = '   Total   TTL CTN QTY     TTL CNM'
SET @tmpdatalist = @tmpdatalist + @tmpdata2

INSERT INTO @tempQtyBDown(DataList) VALUES(@tmpdatalist)

SET @tmpdatalist = ''
DECLARE @i INT
SET @i = 0
WHILE (@i <= @sizecount)
BEGIN
	SET @tmpdatalist = @tmpdatalist + '-------- '

	SET @i = @i + 1
END
SET @tmpdatalist = @tmpdatalist + IIF(@reporttype = 1,'--------','-------- -------------- ------------')
INSERT INTO @tempQtyBDown(DataList) VALUES(@tmpdatalist)

--填入Break down
OPEN cursor_ArticleData
FETCH NEXT FROM cursor_ArticleData INTO @article, @dataseq
WHILE @@FETCH_STATUS = 0
BEGIN
	SET @tmpdatalist = @article + REPLICATE(' ', 9 - len(@article))
	OPEN cursor_SizeData
	FETCH NEXT FROM cursor_SizeData INTO @sizecode, @dataseq
	WHILE @@FETCH_STATUS = 0
	BEGIN
		select @qty = sum(ShipQty) from PackingList_Detail where Id = @packinglistid and Article = @article and SizeCode = @sizecode
		SET @datalen = len(@qty)
		SET @tmpdata = IIF(@datalen = 1,'    ',IIF(@datalen = 2 or @datalen = 3,'   ',IIF(@datalen = 4,'  ',IIF(@datalen = 5 or @datalen = 6,' ','')))) + CONVERT(VARCHAR,@qty) + IIF(@datalen = 1 or @datalen = 2,'    ',IIF(@datalen = 3 or @datalen = 4 or @datalen = 5,'   ',IIF(@datalen = 6 or @datalen = 7,'  ',' ')))
		SET @tmpdatalist = @tmpdatalist  + @tmpdata
			
		FETCH NEXT FROM cursor_SizeData INTO @sizecode, @dataseq
	END
	CLOSE cursor_SizeData
	
	select @qty = sum(ShipQty) from PackingList_Detail where Id = @packinglistid and Article = @article
	SET @datalen = len(@qty)
	SET @tmpdata2 = IIF(@datalen = 1,'    ',IIF(@datalen = 2 or @datalen = 3,'   ',IIF(@datalen = 4,'  ',IIF(@datalen = 5 or @datalen = 6,' ','')))) + convert(VARCHAR,@qty) + IIF(@datalen = 1 or @datalen = 2,'    ',IIF(@datalen = 3 or @datalen = 4 or @datalen = 5,'   ',IIF(@datalen = 6 or @datalen = 7,'  ',' ')))
	IF(@reporttype = 2)
		BEGIN
			select @qty = Sum(CTNQty) from PackingList_Detail where Id = @packinglistid and Article = @article
			SET @datalen = len(@qty)
			SET @tmpdata2 = @tmpdata2 + IIF(@datalen = 1,'      ',IIF(@datalen = 2 or @datalen = 3,'     ',IIF(@datalen = 4 or @datalen = 5,'    ',IIF(@datalen = 6 or @datalen = 7,'   ',IIF(@datalen = 8 or @datalen = 9 or @datalen = 10,'  ',IIF(@datalen = 11 or @datalen = 12,' ','')))))) + convert(VARCHAR,@qty) + IIF(@datalen = 1 or @datalen = 2,'        ',IIF(@datalen = 3 or @datalen = 4,'       ',IIF(@datalen = 5 or @datalen = 6,'      ',IIF(@datalen = 7 or @datalen = 8,'     ',IIF(@datalen = 9,'    ',IIF(@datalen = 10 or @datalen = 11,'   ',IIF(@datalen = 12 or @datalen = 13,'  ',' ')))))))

			select @cbm = sum(pd.CTNQty*l.CBM) from PackingList_Detail pd left join LocalItem l on l.RefNo = pd.RefNo where pd.ID = @packinglistid and pd.Article = @article
			SET @datalen = len(@cbm)
			SET @tmpdata2 = @tmpdata2 + IIF(@datalen = 1,'      ',IIF(@datalen = 2 or @datalen = 3,'     ',IIF(@datalen = 4 or @datalen = 5,'    ',IIF(@datalen = 6 or @datalen = 7,'   ',IIF(@datalen = 8 or @datalen = 9 or @datalen = 10,'  ',IIF(@datalen = 11 or @datalen = 12,' ','')))))) + convert(VARCHAR,@cbm) + IIF(@datalen = 1 or @datalen = 2,'        ',IIF(@datalen = 3 or @datalen = 4,'       ',IIF(@datalen = 5 or @datalen = 6,'      ',IIF(@datalen = 7 or @datalen = 8,'     ',IIF(@datalen = 9,'    ',IIF(@datalen = 10 or @datalen = 11,'   ',IIF(@datalen = 12 or @datalen = 13,'  ',' ')))))))
		END
	
	SET @tmpdatalist = @tmpdatalist + @tmpdata2

	INSERT INTO @tempQtyBDown(DataList) VALUES(@tmpdatalist)
	FETCH NEXT FROM cursor_ArticleData INTO @article, @dataseq
END
CLOSE cursor_ArticleData

SET @tmpdatalist = ''
SET @i = 0
WHILE (@i <= @sizecount)
BEGIN
	SET @tmpdatalist = @tmpdatalist + '-------- '

	SET @i = @i + 1
END
SET @tmpdatalist = @tmpdatalist + IIF(@reporttype = 1,'--------','-------- -------------- ------------')
INSERT INTO @tempQtyBDown(DataList) VALUES(@tmpdatalist)

SET @tmpdatalist = 'TTL.     '
OPEN cursor_SizeData
FETCH NEXT FROM cursor_SizeData INTO @sizecode, @dataseq
WHILE @@FETCH_STATUS = 0
BEGIN
	select @qty = sum(ShipQty) from PackingList_Detail where Id = @packinglistid and SizeCode = @sizecode
	SET @datalen = len(@qty)
	SET @tmpdata2 = IIF(@datalen = 1,'    ',IIF(@datalen = 2 or @datalen = 3,'   ',IIF(@datalen = 4,'  ',IIF(@datalen = 5 or @datalen = 6,' ','')))) + convert(VARCHAR,@qty) + IIF(@datalen = 1 or @datalen = 2,'    ',IIF(@datalen = 3 or @datalen = 4 or @datalen = 5,'   ',IIF(@datalen = 6 or @datalen = 7,'  ',' ')))
	SET @tmpdatalist = @tmpdatalist  + @tmpdata2

	FETCH NEXT FROM cursor_SizeData INTO @sizecode, @dataseq
END
CLOSE cursor_SizeData
select @qty = sum(ShipQty) from PackingList_Detail where Id = @packinglistid
SET @datalen = len(@qty)
SET @tmpdata2 = IIF(@datalen = 1,'    ',IIF(@datalen = 2 or @datalen = 3,'   ',IIF(@datalen = 4,'  ',IIF(@datalen = 5 or @datalen = 6,' ','')))) + convert(VARCHAR,@qty) + IIF(@datalen = 1 or @datalen = 2,'    ',IIF(@datalen = 3 or @datalen = 4 or @datalen = 5,'   ',IIF(@datalen = 6 or @datalen = 7,'  ',' ')))

IF(@reporttype = 2)
BEGIN
	select @qty = Sum(CTNQty) from PackingList_Detail where Id = @packinglistid
	SET @datalen = len(@qty)
	SET @tmpdata2 = @tmpdata2 + IIF(@datalen = 1,'      ',IIF(@datalen = 2 or @datalen = 3,'     ',IIF(@datalen = 4 or @datalen = 5,'    ',IIF(@datalen = 6 or @datalen = 7,'   ',IIF(@datalen = 8 or @datalen = 9 or @datalen = 10,'  ',IIF(@datalen = 11 or @datalen = 12,' ','')))))) + convert(VARCHAR,@qty) + IIF(@datalen = 1 or @datalen = 2,'        ',IIF(@datalen = 3 or @datalen = 4,'       ',IIF(@datalen = 5 or @datalen = 6,'      ',IIF(@datalen = 7 or @datalen = 8,'     ',IIF(@datalen = 9,'    ',IIF(@datalen = 10 or @datalen = 11,'   ',IIF(@datalen = 12 or @datalen = 13,'  ',' ')))))))

	select @cbm = sum(pd.CTNQty*l.CBM) from PackingList_Detail pd left join LocalItem l on l.RefNo = pd.RefNo where pd.ID = @packinglistid
	SET @datalen = len(@cbm)
	SET @tmpdata2 = @tmpdata2 + IIF(@datalen = 1,'      ',IIF(@datalen = 2 or @datalen = 3,'     ',IIF(@datalen = 4 or @datalen = 5,'    ',IIF(@datalen = 6 or @datalen = 7,'   ',IIF(@datalen = 8 or @datalen = 9 or @datalen = 10,'  ',IIF(@datalen = 11 or @datalen = 12,' ','')))))) + convert(VARCHAR,@cbm) + IIF(@datalen = 1 or @datalen = 2,'        ',IIF(@datalen = 3 or @datalen = 4,'       ',IIF(@datalen = 5 or @datalen = 6,'      ',IIF(@datalen = 7 or @datalen = 8,'     ',IIF(@datalen = 9,'    ',IIF(@datalen = 10 or @datalen = 11,'   ',IIF(@datalen = 12 or @datalen = 13,'  ',' ')))))))
END
SET @tmpdatalist = @tmpdatalist  + @tmpdata2
INSERT INTO @tempQtyBDown(DataList) VALUES(@tmpdatalist)

DEALLOCATE cursor_SizeData
DEALLOCATE cursor_ArticleData

select * from @tempQtyBDown", MyUtility.Convert.GetString(masterData["ID"]), reportType);
            result = DBProxy.Current.Select(null, sqlCmd, out qtyBDown);
            return result;

        }

        // 產生Excel
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            if (reportType == "1" || reportType == "2")
            {
                string strXltName = Sci.Env.Cfg.XltPathDir + "Packing_P03_PackingListReport.xltx";
                Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
                if (excel == null) return false;
                MyUtility.Msg.WaitWindows("Starting to excel...");
                Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

                worksheet.Cells[2, 3] = MyUtility.Convert.GetString(masterData["ID"]);
                worksheet.Cells[4, 1] = MyUtility.Convert.GetString(printData.Rows[0]["OrderID"]);
                worksheet.Cells[4, 3] = MyUtility.Convert.GetString(printData.Rows[0]["StyleID"]);
                worksheet.Cells[4, 6] = MyUtility.Convert.GetString(printData.Rows[0]["Customize1"]);
                worksheet.Cells[4, 10] = MyUtility.Convert.GetInt(printData.Rows[0]["CustPONo"]);
                worksheet.Cells[4, 13] = MyUtility.Convert.GetString("INVNo");
                worksheet.Cells[6, 1] = MyUtility.Convert.GetString(masterData["CustCDID"]);
                worksheet.Cells[6, 3] = MyUtility.Convert.GetString(masterData["ShipModeID"]);
                worksheet.Cells[6, 6] = MyUtility.Convert.GetString(printData.Rows[0]["InClogQty"]) + " / " + MyUtility.Convert.GetString(masterData["CTNQty"]) + "   ( " + MyUtility.Convert.GetString(MyUtility.Math.Round(MyUtility.Convert.GetDecimal(printData.Rows[0]["InClogQty"]) / MyUtility.Convert.GetDecimal(masterData["CTNQty"]), 4) * 100) + "% )";
                worksheet.Cells[6, 10] = MyUtility.Convert.GetInt(printData.Rows[0]["Alias"]);
                worksheet.Cells[6, 13] = MyUtility.Check.Empty(printData.Rows[0]["EstPulloutDate"]) ? "" : Convert.ToDateTime(printData.Rows[0]["EstPulloutDate"]).ToString("d");

                //當要列印的筆數超過22筆，就要插入Row，因為範本只留22筆記錄的空間
                if (printData.Rows.Count > 22)
                {
                    for (int i = 1; i <= printData.Rows.Count - 22; i++)
                    {
                        Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range("A9:A9", Type.Missing).EntireRow;
                        rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                    }
                }

                int excelRow = 8;
                string ctnStartNo = "XXXXXX";
                foreach (DataRow dr in printData.Rows)
                {
                    int ctns = MyUtility.Convert.GetInt(dr["CTNEndNo"]) - MyUtility.Convert.GetInt(dr["CTNStartNo"]) + 1;
                    if (ctnStartNo != MyUtility.Convert.GetString(dr["CTNStartNo"]))
                    {
                        worksheet.Cells[excelRow, 1] = MyUtility.Convert.GetString(dr["CTNStartNo"]);
                        worksheet.Cells[excelRow, 3] = MyUtility.Convert.GetString(ctns);
                        worksheet.Cells[excelRow, 9] = MyUtility.Convert.GetString(dr["NW"]);
                        worksheet.Cells[excelRow, 10] = MyUtility.Convert.GetString(dr["GW"]);
                        worksheet.Cells[excelRow, 11] = MyUtility.Convert.GetString(dr["NNW"]);
                        worksheet.Cells[excelRow, 13] = MyUtility.Convert.GetString(MyUtility.Convert.GetDecimal(dr["NW"]) * ctns);
                        worksheet.Cells[excelRow, 14] = MyUtility.Convert.GetString(MyUtility.Convert.GetDecimal(dr["GW"]) * ctns);
                        worksheet.Cells[excelRow, 15] = MyUtility.Convert.GetString(MyUtility.Convert.GetDecimal(dr["NNW"]) * ctns);

                        if (MyUtility.Convert.GetString(dr["CTNStartNo"]) != MyUtility.Convert.GetString(dr["CTNEndNo"]))
                        {
                            worksheet.Cells[excelRow, 2] = MyUtility.Convert.GetString(dr["CTNEndNo"]);
                        }
                    }
                    worksheet.Cells[excelRow, 4] = MyUtility.Convert.GetString(dr["Article"]) + " " + MyUtility.Convert.GetString(dr["Color"]);
                    worksheet.Cells[excelRow, 5] = MyUtility.Convert.GetString(dr["SizeCode"]);
                    worksheet.Cells[excelRow, 6] = MyUtility.Convert.GetString(dr["SizeSpec"]);
                    worksheet.Cells[excelRow, 7] = MyUtility.Convert.GetString(dr["QtyPerCTN"]);
                    worksheet.Cells[excelRow, 8] = MyUtility.Convert.GetString(MyUtility.Convert.GetInt(dr["QtyPerCTN"]) * ctns);
                    worksheet.Cells[excelRow, 12] = MyUtility.Check.Empty(dr["NWPerPcs"]) ? "" : MyUtility.Convert.GetString(dr["NWPerPcs"]);

                    ctnStartNo = MyUtility.Convert.GetString(dr["CTNStartNo"]);
                    excelRow++;
                }

                worksheet.Range[String.Format("A{0}:O{0}", excelRow)].Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop).Weight = 2; //1: 虛線, 2:實線, 3:粗體線
                worksheet.Range[String.Format("A{0}:O{0}", excelRow)].Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop).LineStyle = 1;
                worksheet.Cells[excelRow, 2] = "Total";
                if (excelRow > 8)
                {
                    worksheet.Cells[excelRow, 3] = string.Format("=SUM(C8:C{0})", MyUtility.Convert.GetString(excelRow - 1));
                    worksheet.Cells[excelRow, 8] = string.Format("=SUM(H8:H{0})", MyUtility.Convert.GetString(excelRow - 1));
                    worksheet.Cells[excelRow, 13] = string.Format("=SUM(M8:M{0})", MyUtility.Convert.GetString(excelRow - 1));
                    worksheet.Cells[excelRow, 14] = string.Format("=SUM(N8:N{0})", MyUtility.Convert.GetString(excelRow - 1));
                }
                if (excelRow <= 30)
                {
                    excelRow = 30;
                }

                //Carton Dimension:
                excelRow++;
                StringBuilder ctnDimension = new StringBuilder();
                foreach (DataRow dr in ctnDim.Rows)
                {
                    ctnDimension.Append(string.Format("{0} - {1} - {2} {3}, (CTN#:{4}){5}  \r\n",
                        MyUtility.Convert.GetString(dr["RefNo"]), MyUtility.Convert.GetString(dr["Description"]), MyUtility.Convert.GetString(dr["Dimension"]), MyUtility.Convert.GetString(dr["CtnUnit"]),
                        MyUtility.Convert.GetString(dr["Ctn"]).Substring(0, MyUtility.Convert.GetString(dr["Ctn"]).Length - 1),
                        reportType == "1" ? ", ttlCBM:" + MyUtility.Convert.GetString(dr["TtlCBM"]) : ""));
                }
                worksheet.Cells[excelRow, 3] = ctnDimension.Length > 0 ? ctnDimension.ToString() : "";

                //Remarks
                excelRow++;
                worksheet.Cells[excelRow, 3] = MyUtility.Convert.GetString(masterData["Remark"]);

                // Color/Size Breakdown
                excelRow = excelRow + 2;
                if (qtyBDown.Rows.Count > 5)
                {
                    Microsoft.Office.Interop.Excel.Range rngToCopy = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(excelRow))).EntireRow;
                    for (int i = 1; i <= qtyBDown.Rows.Count - 5; i++)
                    {
                        Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range(string.Format("A{0}", MyUtility.Convert.GetString(excelRow + 1)), Type.Missing).EntireRow;
                        rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown, rngToCopy.Copy(Type.Missing));
                    }
                }
                foreach (DataRow dr in qtyBDown.Rows)
                {
                    worksheet.Cells[excelRow, 1] = MyUtility.Convert.GetString(dr["DataList"]);
                    excelRow++;
                }


                //Shipment mark
                excelRow = excelRow + 3;
                worksheet.Cells[excelRow, 1] = MyUtility.Convert.GetString(printData.Rows[0]["MarkFront"]);
                worksheet.Cells[excelRow, 8] = MyUtility.Convert.GetString(printData.Rows[0]["MarkBack"]);

                worksheet.Cells[excelRow + 13, 1] = MyUtility.Convert.GetString(printData.Rows[0]["MarkLeft"]);
                worksheet.Cells[excelRow + 13, 8] = MyUtility.Convert.GetString(printData.Rows[0]["MarkRight"]);

                MyUtility.Msg.WaitClear();
                excel.CutCopyMode = Microsoft.Office.Interop.Excel.XlCutCopyMode.xlCopy;
                excel.Visible = true;
            }
            else if (reportType == "3")
            {
                PublicPrg.Prgs.PackingListToExcel_PackingGuideReport("Packing_P03_PackingGuideReport.xltx", printData, ctnDim, qtyCtn, articleSizeTtlShipQty, printGroupData, clipData, masterData, orderQty, specialInstruction);

            }

            return true;
        }
    }
}
