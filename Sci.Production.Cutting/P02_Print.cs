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
using System.Data.SqlClient;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Cutting
{
    public partial class P02_Print : Sci.Win.Tems.PrintForm
    {
        string spreadingType = "";
        string S1, S2,Poid="";
        private string keyword = Sci.Env.User.Keyword;
        int SheetCount = 1;
        DataTable WorkorderTb, WorkorderSizeTb, WorkorderDisTb, WorkorderPatternTb, CutrefTb, RequestTb, CutDisOrderIDTb, CutSizeTb, SizeTb, CutQtyTb, MarkerTB, FabricComboTb,IssueTb;
        DataRow detDr, OrderDr;
        public P02_Print(DataRow workorderDr,string poid)
        {
            InitializeComponent();
            detDr = workorderDr;
            Poid = poid;

            Cutref_ra.Checked = true;
            textBox1.Text = detDr["CutRef"].ToString();
            textBox2.Text = detDr["CutRef"].ToString();

        }

        protected override bool ValidateInput()
        {
            S1 = textBox1.Text;
            S2 = textBox2.Text;

            if (MyUtility.Check.Empty(S1) || MyUtility.Check.Empty(S2))
            {
                MyUtility.Msg.WarningBox("<Range> can not be empty", "Warning");
                return false;
            }
            return base.ValidateInput();
        }

        private void Requ_ra_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            label1.Text = Requ_ra.Checked ? "Cutplan ID" : "Cut RefNo";
        }

        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            List<SqlParameter> paras = new List<SqlParameter>();
            paras.Add(new SqlParameter("@Cutref1", S1));
            paras.Add(new SqlParameter("@Cutref2", S2));
            string byType;

            if (Cutref_ra.Checked)  byType = "Cutref";
            else byType = "Cutplanid";
            
            string workorder_cmd = string.Format("Select a.*,b.Description,b.width,dbo.MarkerLengthToYDS(MarkerLength) as yds from Workorder a Left Join Fabric b on a.SciRefno = b.SciRefno Where {1}>=@Cutref1 and {1}<=@Cutref2 and a.id='{0}'", detDr["ID"],byType);
            DualResult dResult = DBProxy.Current.Select(null, workorder_cmd, paras,out WorkorderTb);
            if (!dResult) return dResult;
            workorder_cmd = string.Format("Select {1},a.Cutno,a.Colorid,a.Layer,a.Cons,b.* from Workorder a,Workorder_Distribute b Where {1}>=@Cutref1 and {1}<=@Cutref2 and a.id='{0}' and a.ukey = b.workorderukey", detDr["ID"],byType);
            dResult = DBProxy.Current.Select(null, workorder_cmd, paras, out WorkorderDisTb);
            if (!dResult) return dResult;
            workorder_cmd = string.Format("Select {1},a.MarkerName,a.MarkerNo,MarkerLength,Cons,a.Layer,a.Cutno,a.colorid,c.seq,FabricCombo,b.* from Workorder a,Workorder_SizeRatio b,Order_SizeCode c Where {1}>=@Cutref1 and {1}<=@Cutref2 and a.id='{0}' and a.ukey = b.workorderukey and a.id = c.id and b.id = c.id and b.sizecode = c.sizecode order by c.seq", detDr["ID"], byType);
            dResult = DBProxy.Current.Select(null, workorder_cmd, paras, out WorkorderSizeTb);
            if (!dResult) return dResult;
            workorder_cmd = string.Format("Select {1},b.*,Markername,FabricCombo from Workorder a,Workorder_PatternPanel b Where {1}>=@Cutref1 and {1}<=@Cutref2 and a.id='{0}' and a.ukey = b.workorderukey", detDr["ID"], byType);
            dResult = DBProxy.Current.Select(null, workorder_cmd, paras, out WorkorderPatternTb);
            if (!dResult) return dResult;
            MyUtility.Check.Seek(string.Format("Select * from Orders Where id='{0}'",detDr["ID"]),out OrderDr);
            MyUtility.Tool.ProcessWithDatatable(WorkorderTb, string.Format("{0},estCutDate",byType), string.Format("Select distinct {0},estCutDate From #tmp ",byType), out CutrefTb);
            MyUtility.Tool.ProcessWithDatatable(WorkorderDisTb, string.Format("{0},OrderID",byType), string.Format("Select distinct {0},OrderID From #tmp",byType), out CutDisOrderIDTb); //整理sp
            MyUtility.Tool.ProcessWithDatatable(WorkorderSizeTb, string.Format("{0},MarkerName,MarkerNo,MarkerLength,SizeCode,Cons,Qty,seq,FabricCombo", byType), string.Format("Select distinct {0},MarkerName,MarkerNo,MarkerLength,SizeCode,Cons,Qty,seq,FabricCombo,dbo.MarkerLengthToYDS(MarkerLength) as yds From #tmp order by FabricCombo,MarkerName,seq", byType), out CutSizeTb); //整理SizeGroup,Qty
            MyUtility.Tool.ProcessWithDatatable(WorkorderSizeTb, string.Format("{0},SizeCode,seq", byType), string.Format("Select distinct {0},SizeCode,seq From #tmp order by seq ", byType), out SizeTb); //整理Size
            MyUtility.Tool.ProcessWithDatatable(WorkorderSizeTb, string.Format("{0},MarkerName", byType), string.Format("Select distinct {0},MarkerName From #tmp ", byType), out MarkerTB); //整理MarkerName
            MyUtility.Tool.ProcessWithDatatable(WorkorderTb, string.Format("{0},FabricCombo,SCIRefno", byType), string.Format("Select distinct {0},a.FabricCombo,a.SCIRefno,b.Description,b.width  From #tmp a Left Join Fabric b on a.SciRefno = b.SciRefno", byType), out FabricComboTb); //整理FabricCombo     

            if (Requ_ra.Checked)
            {
                string Issue_cmd = string.Format("Select a.Cutplanid,b.Qty,b.Dyelot,b.Roll,Max(c.yds) as yds,c.Colorid from Issue a,Issue_Detail b, #tmp c Where a.id=b.id and c.Cutplanid = a.Cutplanid and c.SEQ1 = b.SEQ1 and c.SEQ2 = b.SEQ2 group by a.Cutplanid,b.Qty,b.Dyelot,b.Roll,c.Colorid order by Dyelot,roll", detDr["ID"], byType);
                MyUtility.Tool.ProcessWithDatatable(WorkorderTb, "Cutplanid,SEQ1,SEQ2,yds,Colorid", Issue_cmd, out IssueTb); //整理FabricCombo     
            }
            return Result.True; 
        }

        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            
            SheetCount = CutrefTb.Rows.Count;
            if (SheetCount == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }
            
            MyUtility.Msg.WaitWindows("Starting EXCEL...");
            if (Cutref_ra.Checked) return ByCutrefExcel();
            else return ByRequestExcel();
            MyUtility.Msg.WaitClear();
            //return true;
        }
        

        public bool ByRequestExcel()
        {
            string strXltName = Sci.Env.Cfg.XltPathDir + "\\Cutting_P02_SpreadingReportbyRequest.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null) return false;
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            #region 寫入共用欄位
            worksheet.Cells[1, 6] = keyword;
            worksheet.Cells[3, 2] = DateTime.Now.ToShortDateString();
            worksheet.Cells[9, 2] = OrderDr["Styleid"];
            worksheet.Cells[10, 2] = OrderDr["Seasonid"];
            worksheet.Cells[10, 13] = OrderDr["Sewline"];
            for (int nColumn = 3; nColumn <= 21; nColumn += 3)
            {
                worksheet.Cells[28, nColumn] = OrderDr["Styleid"];
                worksheet.Cells[29, nColumn] = detDr["ID"];
            }

            int nSheet = 1;
            string spList = "";
            DataRow[] WorkorderArry;
            DataRow[] WorkorderDisArry;
            DataRow[] WorkorderSizeArry;
            DataRow[] WorkorderPatternArry;
            DataRow[] WorkorderOrderIDArry;
            DataRow[] SizeArry;
            DataRow[] SizeCodeArry;
            DataRow[] MarkerArry;
            DataRow[] FabricComboArry;
            DataRow[] IssueArry;
            string pattern = "", line = "";
            int nDisCount = 0;
            Double disRow = 0;
            string size = "", Ratio = "";
            int TotConsRowS = 19, TotConsRowE = 20, nSizeColumn = 0;
            foreach (DataRow Cutrefdr in CutrefTb.Rows)
            {
                spList = "";

                if (nSheet >= 2) //有兩筆以上才做其他Sheet
                {
                    worksheet = excel.ActiveWorkbook.Worksheets[nSheet - 1];
                    worksheet.Copy(Type.Missing, worksheet);
                }
                worksheet = excel.ActiveWorkbook.Worksheets[nSheet];
                worksheet.Select();
                worksheet.Name = Cutrefdr["Cutplanid"].ToString();
                worksheet.Cells[3, 19] = Cutrefdr["Cutplanid"].ToString();
                worksheet.Cells[9, 13] = ((DateTime)MyUtility.Convert.GetDate(Cutrefdr["Estcutdate"])).ToShortDateString();
                nSheet++;
            }
            nSheet = 1;
            #endregion
            foreach (DataRow Cutrefdr in CutrefTb.Rows)
            {
                worksheet = excel.ActiveWorkbook.Worksheets[nSheet];
                worksheet.Select();
                string Cutplanid = Cutrefdr["Cutplanid"].ToString();
                #region 撈表身Detail Array
                WorkorderArry = WorkorderTb.Select(string.Format("Cutplanid = '{0}'", Cutplanid));
                WorkorderDisArry = WorkorderDisTb.Select(string.Format("Cutplanid='{0}'", Cutplanid));
                WorkorderSizeArry = WorkorderSizeTb.Select(string.Format("Cutplanid='{0}'", Cutplanid));
                WorkorderOrderIDArry = CutDisOrderIDTb.Select(string.Format("Cutplanid='{0}'", Cutplanid), "Orderid");
                FabricComboArry = FabricComboTb.Select(string.Format("Cutplanid='{0}'", Cutplanid));
                SizeCodeArry = SizeTb.Select(string.Format("Cutplanid='{0}'", Cutplanid), "SEQ");
                MarkerArry = MarkerTB.Select(string.Format("Cutplanid = '{0}'", Cutplanid));
                IssueArry = IssueTb.Select(string.Format("Cutplanid = '{0}'", Cutplanid));
                #endregion

                if (WorkorderArry.Length > 0)
                {
                    worksheet.Cells[8, 13] = WorkorderArry[0]["MarkerDownLoadId"].ToString();
                    #region 從後面開始寫 先寫Refno,Color

                    for (int nColumn = 3; nColumn <= 22; nColumn += 3)
                    {
                        worksheet.Cells[25, nColumn] = WorkorderArry[0]["Refno"];
                        worksheet.Cells[26, nColumn] = WorkorderArry[0]["Colorid"];
                    }
                    #endregion
                }
                int copyRow = 0;
                int RowRange = 6;

                if (FabricComboArry.Length > 0)
                {
                    foreach (DataRow FabricComboDr in FabricComboArry)
                    {
                        if (copyRow > 0)
                        {
                            Excel.Range r = worksheet.get_Range("A" + ((12 + RowRange * (copyRow-1))).ToString(), "A" + ((12 + RowRange * (copyRow-1)) + RowRange-1).ToString()).EntireRow;
                            r.Copy();
                            r.Insert(Excel.XlInsertShiftDirection.xlShiftDown); //新增Row
                        }
                        WorkorderPatternArry = WorkorderPatternTb.Select(string.Format("Cutplanid='{0}' and FabricCombo = '{1}'", Cutplanid, FabricComboDr["FabricCombo"]), "PatternPanel");
                        pattern = "";
                        if (WorkorderPatternArry.Length > 0)
                        {
                            foreach (DataRow patDr in WorkorderPatternArry)
                            {
                                if (!patDr["PatternPanel"].ToString().InList(pattern))
                                {
                                    pattern = pattern + patDr["PatternPanel"].ToString() + ",";
                                }
                            }
                        }
                        int FabricRow = (12 + RowRange * copyRow);
                        worksheet.Cells[FabricRow, 2] = FabricComboDr["FabricCombo"].ToString();
                        worksheet.Cells[FabricRow, 5] = pattern;
                        worksheet.Cells[FabricRow, 9] = FabricComboDr["Description"].ToString();
                        worksheet.Cells[FabricRow, 19] = FabricComboDr["width"].ToString();

                        copyRow++;
                    }
                }
                #region OrderSP List, Line List
                if (WorkorderOrderIDArry.Length > 0)
                {
                    foreach (DataRow DisDr in WorkorderOrderIDArry)
                    {

                        if (DisDr["OrderID"].ToString() != "EXCESS")
                        {
                            if (!DisDr["OrderID"].ToString().InList(spList,"\\"))
                            {
                                spList = spList + DisDr["OrderID"].ToString() + "\\";
                            }
                            
                        }
                        #region SewingLine
                        line = line + MyUtility.GetValue.Lookup("Sewline", DisDr["OrderID"].ToString(), "Orders", "ID") + "\\";
                        #endregion
                    }
                    worksheet.Cells[8, 2] = spList;
                    worksheet.Cells[10, 13] = line;
                }
                #endregion

                #region Markname
                int nRow = 11;


                if (MarkerArry.Length > 0)
                {

                    size = "";
                    Ratio = "";
                    #region Size,Ratio
                    foreach (DataRow MarkerDr in MarkerArry)
                    {
                        Excel.Range r = worksheet.get_Range("A" + nRow.ToString(), "A" + nRow.ToString()).EntireRow;
                        r.Copy();
                        r.Insert(Excel.XlInsertShiftDirection.xlShiftDown); //新增Row
                        nRow++;

                        SizeArry = CutSizeTb.Select(string.Format("Cutplanid='{0}' and MarkerName = '{1}'", Cutplanid,MarkerDr["MarkerName"]));
                        if (SizeArry.Length > 0)
                        {
                            size = "";
                            Ratio = "";
                            foreach (DataRow sizeDr in SizeArry)
                            {
                                size = size + sizeDr["SizeCode"].ToString() + ",";
                                Ratio = Ratio + MyUtility.Convert.GetDouble(sizeDr["Qty"]).ToString() + ",";
                            }
                            worksheet.Cells[nRow, 1] = SizeArry[0]["MarkerName"].ToString();
                            worksheet.Cells[nRow, 4] = SizeArry[0]["MarkerNo"].ToString();
                            worksheet.Cells[nRow, 6] = SizeArry[0]["MarkerLength"].ToString() + "\n" + SizeArry[0]["yds"].ToString();
                        }

                        worksheet.Cells[nRow, 10] = size;
                        worksheet.Cells[nRow, 12] = Ratio;
                        
                    }
                    #endregion   
                }
                #endregion
                nRow = nRow + 3; //Size
                string str_PIVOT = "";
                nSizeColumn = 4;
                foreach (DataRow dr in SizeCodeArry)
                {
                    str_PIVOT = str_PIVOT + string.Format("[{0}],", dr["SizeCode"].ToString());
                    //寫入Size
                    for (int i = 0; i < FabricComboTb.Rows.Count; i++)
                    {
                        worksheet.Cells[nRow+(RowRange*i), nSizeColumn] = dr["SizeCode"].ToString();
                    }
                    nSizeColumn++;
                }
                str_PIVOT = str_PIVOT.Substring(0, str_PIVOT.Length - 1);
                string Pivot_cmd = string.Format(
                @"Select * From
                (
                    Select FabricCombo,MarkerName,Cutno,Colorid,SizeCode,Cons,Layer,(Qty*Layer) as TotalQty from 
                    #tmp
                    Where Cutplanid = '{0} '
                ) as mTb
                Pivot(Sum(TotalQty)
                for SizeCode in ({1})) as pIvT 
                order by FabricCombo,Cutno,Colorid",
Cutplanid, str_PIVOT); 
                if (CutQtyTb != null)
                {
                    CutQtyTb.Clear();
                }
                MyUtility.Tool.ProcessWithDatatable(WorkorderSizeTb, "FabricCombo,MarkerName,Cutno,Colorid,SizeCode,Qty,Layer,Cutplanid,Cons", Pivot_cmd, out CutQtyTb);
                nRow = nRow + 1;
                bool lfirstComb = true;
                string fabColor = "";
                foreach (DataRow FabricComboDr in FabricComboTb.Rows)
                {
                    DataRow[] CutQtyArray = CutQtyTb.Select(string.Format("FabricCombo = '{0}'", FabricComboDr["FabricCombo"]));
                    if (CutQtyArray.Length > 0)
                    {
                        int copyrow = 0;
                        nRow = lfirstComb ? nRow : nRow + 4;
                        lfirstComb = false;
                        TotConsRowS = nRow; //第一個Cons
                        foreach (DataRow cutqtydr in CutQtyArray)
                        {
                            if (copyrow > 0)
                            {
                                Excel.Range r = worksheet.get_Range("A" + (nRow).ToString(), "A" + (nRow).ToString()).EntireRow;
                                r.Copy();
                                r.Insert(Excel.XlInsertShiftDirection.xlShiftDown, Excel.XlInsertFormatOrigin.xlFormatFromRightOrBelow); //新增Row
                            }
                            worksheet.Cells[nRow, 1] = cutqtydr["Cutno"].ToString();
                            worksheet.Cells[nRow, 2] = cutqtydr["Colorid"].ToString();
                            worksheet.Cells[nRow, 3] = cutqtydr["Layer"].ToString();
                            worksheet.Cells[nRow, 20] = cutqtydr["Cons"].ToString();
                            fabColor = cutqtydr["Colorid"].ToString(); ;
                            for (int nSizeDetail = 0; nSizeDetail < SizeCodeArry.Length; nSizeDetail++)
                            {
                                worksheet.Cells[nRow, nSizeDetail + 4] = cutqtydr[6 + nSizeDetail].ToString(); //+4因為從第四個Column 開始 nSizeDetail +4 是因為Table 從第四個開始是Size
                            }
                            nRow++;
                            copyrow++;
                        }
                        TotConsRowE = nRow; //最後一個Cons
                        #region Total Cons
                        nRow = nRow + 1;
                        worksheet.Cells[nRow, 20] = string.Format("=SUM(T{0}:T{1})", TotConsRowS, TotConsRowE);
                        worksheet.Cells[nRow, 18] = fabColor;
                        #endregion
                    }
                }
                nRow = nRow + 4; //Roll Table
                #region Issue Roll,Dyelot
                if (IssueArry.Length > 0)
                {
                    bool lfirstdr = true;
                    foreach (DataRow IssueDr in IssueArry)
                    {
                        if (!lfirstdr)
                        {
                            Excel.Range r = worksheet.get_Range("A" + (nRow).ToString(), "A" + (nRow).ToString()).EntireRow;
                            r.Copy();
                            r.Insert(Excel.XlInsertShiftDirection.xlShiftDown, Excel.XlInsertFormatOrigin.xlFormatFromRightOrBelow); //新增Row
                        }
                        lfirstdr = false;
                        worksheet.Cells[nRow, 1] = IssueDr["Roll"].ToString();
                        worksheet.Cells[nRow, 2] = IssueDr["Colorid"].ToString();
                        worksheet.Cells[nRow, 4] = IssueDr["Dyelot"].ToString();
                        worksheet.Cells[nRow, 6] = IssueDr["Qty"].ToString();

                        //1401: CUTTING_P02_SpreadingReport。[LAYERS]欄位資料清空
                        //worksheet.Cells[nRow, 9] = MyUtility.Convert.GetDouble(IssueDr["yds"])==0? 0: Math.Ceiling(MyUtility.Convert.GetDouble(IssueDr["Qty"])/MyUtility.Convert.GetDouble(IssueDr["yds"]));
                        
                        nRow++;
                    }
                }
                #endregion
                nSheet++;
            }
            excel.Visible = true;
            return true;
        }

        public bool ByCutrefExcel()
        {
            int nSizeColumn;
            SheetCount = CutrefTb.Rows.Count;
            #region By Cutref
            string strXltName = Sci.Env.Cfg.XltPathDir + "\\Cutting_P02_SpreadingReportbyCutref.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null) return false;
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            #region 寫入共用欄位
            worksheet.Cells[1, 6] = keyword;
            worksheet.Cells[3, 2] = DateTime.Now.ToShortDateString();
            worksheet.Cells[9, 2] = OrderDr["Styleid"];
            worksheet.Cells[10, 2] = OrderDr["Seasonid"];
            worksheet.Cells[10, 13] = OrderDr["Sewline"];
            for (int nColumn = 3; nColumn <= 21; nColumn += 3)
            {
                worksheet.Cells[32, nColumn] = OrderDr["Styleid"];
                worksheet.Cells[33, nColumn] = detDr["ID"];
            }

            #endregion

            int nSheet = 1;
            string spList = "";
            DataRow[] WorkorderArry;
            DataRow[] WorkorderDisArry;
            DataRow[] WorkorderSizeArry;
            DataRow[] WorkorderPatternArry;
            DataRow[] WorkorderOrderIDArry;
            DataRow[] SizeArry;
            DataRow[] SizeCodeArry;
            string pattern = "", line = "";
            int nDisCount = 0;
            Double disRow = 0;
            string size = "", Ratio = "";
            int TotConsRowS = 19, TotConsRowE = 20;
            foreach (DataRow Cutrefdr in CutrefTb.Rows)
            {
                spList = "";

                if (nSheet >= 2) //有兩筆以上才做其他Sheet
                {
                    worksheet = excel.ActiveWorkbook.Worksheets[nSheet - 1];
                    worksheet.Copy(Type.Missing, worksheet);
                }
                worksheet = excel.ActiveWorkbook.Worksheets[nSheet];
                worksheet.Select();
                worksheet.Name = Cutrefdr["Cutref"].ToString();
                worksheet.Cells[3, 19] = Cutrefdr["Cutref"].ToString();
                worksheet.Cells[9, 13] = ((DateTime)MyUtility.Convert.GetDate(Cutrefdr["Estcutdate"])).ToShortDateString();
                nSheet++;
            }
            nSheet = 1;
            foreach (DataRow Cutrefdr in CutrefTb.Rows)
            {
                worksheet = excel.ActiveWorkbook.Worksheets[nSheet];
                worksheet.Select();
                string cutref = Cutrefdr["Cutref"].ToString();
                #region 撈表身Detail Array
                WorkorderArry = WorkorderTb.Select(string.Format("Cutref = '{0}'", cutref));
                WorkorderDisArry = WorkorderDisTb.Select(string.Format("Cutref='{0}'", cutref));
                WorkorderSizeArry = WorkorderSizeTb.Select(string.Format("Cutref='{0}'", cutref));
                WorkorderPatternArry = WorkorderPatternTb.Select(string.Format("Cutref='{0}'", cutref), "PatternPanel");
                WorkorderOrderIDArry = CutDisOrderIDTb.Select(string.Format("Cutref='{0}'", cutref), "Orderid");
                SizeArry = CutSizeTb.Select(string.Format("Cutref='{0}'", cutref), "SEQ");
                SizeCodeArry = SizeTb.Select(string.Format("Cutref='{0}'", cutref), "SEQ");
                #endregion

                if (WorkorderArry.Length > 0)
                {
                    pattern = "";
                    worksheet.Cells[8, 13] = WorkorderArry[0]["MarkerDownLoadId"].ToString();
                    worksheet.Cells[13, 2] = WorkorderArry[0]["FabricCombo"].ToString();
                    if (WorkorderPatternArry.Length > 0)
                    {
                        foreach (DataRow patDr in WorkorderPatternArry)
                        {
                            if (!patDr["PatternPanel"].ToString().InList(pattern))
                            {
                                pattern = pattern + patDr["PatternPanel"].ToString() + ",";
                            }
                        }
                    }

                    worksheet.Cells[13, 2] = WorkorderArry[0]["FabricCombo"].ToString();
                    worksheet.Cells[13, 5] = pattern;
                    worksheet.Cells[13, 9] = WorkorderArry[0]["Description"].ToString();
                    worksheet.Cells[13, 19] = WorkorderArry[0]["width"].ToString();
                    #region 從後面開始寫 先寫Refno,Color

                    for (int nColumn = 3; nColumn <= 22; nColumn += 3)
                    {
                        worksheet.Cells[29, nColumn] = WorkorderArry[0]["Refno"];
                        worksheet.Cells[30, nColumn] = WorkorderArry[0]["Colorid"];
                    }
                    #endregion
                }
                #region OrderSP List, Line List
                if (WorkorderOrderIDArry.Length > 0)
                {
                    foreach (DataRow DisDr in WorkorderOrderIDArry)
                    {

                        if (DisDr["OrderID"].ToString() != "EXCESS")
                        {
                            spList = spList + DisDr["OrderID"].ToString() + "\\";
                        }
                        #region SewingLine
                        line = line + MyUtility.GetValue.Lookup("Sewline", DisDr["OrderID"].ToString(), "Orders", "ID") + "\\";
                        #endregion
                    }
                    worksheet.Cells[8, 2] = spList;
                }
                #endregion



                #region Markname
                int nrow = 12;


                if (SizeArry.Length > 0)
                {

                    size = "";
                    Ratio = "";
                    #region Size,Ratio
                    foreach (DataRow sizeDr in WorkorderSizeArry)
                    {
                        size = size + sizeDr["SizeCode"].ToString() + ",";
                        Ratio = Ratio + MyUtility.Convert.GetDouble(sizeDr["Qty"]).ToString() + ",";
                    }
                    #endregion
                    worksheet.Cells[12, 1] = WorkorderArry[0]["MarkerName"].ToString();
                    worksheet.Cells[12, 4] = WorkorderArry[0]["MarkerNo"].ToString();
                    worksheet.Cells[12, 6] = WorkorderArry[0]["MarkerLength"].ToString() + "\n" + WorkorderArry[0]["yds"].ToString();
                    worksheet.Cells[12, 10] = size;
                    worksheet.Cells[12, 12] = Ratio;


                }
                #endregion

                #region Distribute to SP#
                if (WorkorderDisArry.Length > 0)
                {
                    nrow = 16; //到Distribute ROW
                    nDisCount = WorkorderDisArry.Length;
                    disRow = Math.Ceiling(Convert.ToDouble(nDisCount) / 2); //每一個Row 有兩個可以用
                    int arrayrow = 0;
                    for (int i = 0; i < disRow; i++)
                    {
                        if (i > 0)
                        {
                            Excel.Range r = worksheet.get_Range("A" + (nrow - 1).ToString(), "A" + (nrow - 1).ToString()).EntireRow;
                            r.Copy();
                            r.Insert(Excel.XlInsertShiftDirection.xlShiftDown, Excel.XlInsertFormatOrigin.xlFormatFromRightOrBelow); //新增Row
                        }
                        arrayrow = i * 2;
                        worksheet.Cells[nrow, 1] = WorkorderDisArry[arrayrow]["OrderID"].ToString();
                        worksheet.Cells[nrow, 4] = WorkorderDisArry[arrayrow]["Article"].ToString();
                        worksheet.Cells[nrow, 7] = WorkorderDisArry[arrayrow]["SizeCode"].ToString();
                        worksheet.Cells[nrow, 9] = WorkorderDisArry[arrayrow]["Qty"].ToString();
                        if (arrayrow + 1 < nDisCount)
                        {
                            worksheet.Cells[nrow, 11] = WorkorderDisArry[arrayrow + 1]["OrderID"].ToString();
                            worksheet.Cells[nrow, 14] = WorkorderDisArry[arrayrow + 1]["Article"].ToString();
                            worksheet.Cells[nrow, 17] = WorkorderDisArry[arrayrow + 1]["SizeCode"].ToString();
                            worksheet.Cells[nrow, 19] = WorkorderDisArry[arrayrow + 1]["Qty"].ToString();
                        }
                        else
                        {
                            worksheet.Cells[nrow, 11] = "";
                            worksheet.Cells[nrow, 14] = "";
                            worksheet.Cells[nrow, 17] = "";
                            worksheet.Cells[nrow, 19] = "";
                        }
                        nrow++;
                    }
                    // nrow = nrow + Convert.ToInt16(disRow);
                }

                #endregion

                string str_PIVOT = "";
                nSizeColumn = 4;
                foreach (DataRow dr in SizeArry)
                {
                    str_PIVOT = str_PIVOT + string.Format("[{0}],", dr["SizeCode"].ToString());
                    //寫入Size
                    worksheet.Cells[nrow + 1, nSizeColumn] = dr["SizeCode"].ToString();
                    nSizeColumn++;
                }
                str_PIVOT = str_PIVOT.Substring(0, str_PIVOT.Length - 1);
                string Pivot_cmd = string.Format(
                @"Select * From
                (
                    Select Cutno,Colorid,SizeCode,Cons,Layer,(Qty*Layer) as TotalQty from 
                    #tmp
                    Where Cutref = '{0} '
                ) as mTb
                Pivot(Sum(TotalQty)
                for SizeCode in ({1})) as pIvT 
                order by Cutno,Colorid",
cutref, str_PIVOT); 
                if (CutQtyTb != null)
                {
                    CutQtyTb.Clear();
                }
                MyUtility.Tool.ProcessWithDatatable(WorkorderSizeTb, "Cutno,Colorid,SizeCode,Qty,Layer,Cutref,Cons", Pivot_cmd, out CutQtyTb);
                nrow = nrow + 2;
                int copyrow = 0;
                TotConsRowS = nrow; //第一個Cons
                foreach (DataRow cutqtydr in CutQtyTb.Rows)
                {
                    if (copyrow > 0)
                    {
                        Excel.Range r = worksheet.get_Range("A" + (nrow).ToString(), "A" + (nrow).ToString()).EntireRow;
                        r.Copy();
                        r.Insert(Excel.XlInsertShiftDirection.xlShiftDown, Excel.XlInsertFormatOrigin.xlFormatFromRightOrBelow); //新增Row
                    }


                    worksheet.Cells[nrow, 1] = cutqtydr["Cutno"].ToString();
                    worksheet.Cells[nrow, 2] = cutqtydr["Colorid"].ToString();
                    worksheet.Cells[nrow, 3] = cutqtydr["Layer"].ToString();
                    worksheet.Cells[nrow, 20] = cutqtydr["Cons"].ToString();
                    for (int nSizeDetail = 0; nSizeDetail < SizeArry.Length; nSizeDetail++)
                    {
                        worksheet.Cells[nrow, nSizeDetail + 4] = cutqtydr[4 + nSizeDetail].ToString(); //+4因為從第四個Column 開始 nSizeDetail +4 是因為Table 從第四個開始是Size
                    }
                    nrow++;
                    copyrow++;
                }
                TotConsRowE = nrow - 1; //最後一個Cons
                #region Total Cons
                worksheet.Cells[nrow+1, 20] = string.Format("=SUM(T{0}:T{1})", TotConsRowS, TotConsRowE);
                #endregion
                nSheet++;
            }
            #endregion //End By CutRef
            excel.Visible = true;
            return true;
        }
    }
}
