﻿using System;
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
using System.Runtime.InteropServices;
using System.Linq;

namespace Sci.Production.Cutting
{
    public partial class P02_Print : Sci.Win.Tems.PrintForm
    {
        string S1, S2,Poid="";
        string cp,cr;
        private string keyword = Sci.Env.User.Keyword;
        int SheetCount = 1;
        DataTable WorkorderTb, WorkorderSizeTb, WorkorderDisTb, WorkorderPatternTb, CutrefTb, CutDisOrderIDTb, CutSizeTb, SizeTb, CutQtyTb, MarkerTB, FabricComboTb,IssueTb;
        DataRow detDr, OrderDr;
        int _worktype;
        public P02_Print(DataRow workorderDr,string poid,int worktype)
        {
            InitializeComponent();
            detDr = workorderDr;
            Poid = poid;
            _worktype = worktype;
            radioByCutRefNo.Checked = true;
            txtCutRefNoStart.Text = detDr["CutRef"].ToString();
            txtCutRefNoEnd.Text = detDr["CutRef"].ToString();
            cr = detDr["CutRef"].ToString();
            cp = detDr["CutplanID"].ToString();
            txtCutRefNoStart.Select();
        }

        protected override bool ValidateInput()
        {
            S1 = txtCutRefNoStart.Text;
            S2 = txtCutRefNoEnd.Text;

            if (MyUtility.Check.Empty(S1) || MyUtility.Check.Empty(S2))
            {
                MyUtility.Msg.WarningBox("<Range> can not be empty", "Warning");
                return false;
            }
            return base.ValidateInput();
        }

        private void radioByCutplanId_CheckedChanged(object sender, EventArgs e)
        {
            txtCutRefNoStart.Text = radioByCutplanId.Checked ? cp : cr;
            txtCutRefNoEnd.Text = radioByCutplanId.Checked ? cp : cr;
            labelCutRefNo.Text = radioByCutplanId.Checked ? "Cutplan ID" : "Cut RefNo";
        }

        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            List<SqlParameter> paras = new List<SqlParameter>();
            paras.Add(new SqlParameter("@Cutref1", S1));
            paras.Add(new SqlParameter("@Cutref2", S2));
            string byType, byType2;

            if (radioByCutRefNo.Checked)  byType = "Cutref";
            else byType = "Cutplanid";
            if (radioByCutRefNo.Checked) byType2 = ",shc";
            else byType2 = "";
            string workorder_cmd = string.Format(@"Select a.*,b.Description,b.width,dbo.MarkerLengthToYDS(MarkerLength) as yds 
    ,shc = iif(isnull(shc.RefNo,'')='','','Shrinkage Issue, Spreading Backward Speed: 2, Loose Tension')
from Workorder a WITH (NOLOCK) Left Join Fabric b WITH (NOLOCK) on a.SciRefno = b.SciRefno 
outer apply(select RefNo from ShrinkageConcern where RefNo=a.RefNo and Junk=0) shc            
            Where {1}>=@Cutref1 and {1}<=@Cutref2 and a.id='{0}'", detDr["ID"], byType);
            DualResult dResult = DBProxy.Current.Select(null, workorder_cmd, paras,out WorkorderTb);
            if (!dResult) return dResult;

            workorder_cmd = string.Format("Select {1},a.Cutno,a.Colorid,a.Layer,a.Cons,b.* from Workorder a WITH (NOLOCK) ,Workorder_Distribute b WITH (NOLOCK) Where {1}>=@Cutref1 and {1}<=@Cutref2 and a.id='{0}' and a.ukey = b.workorderukey", detDr["ID"], byType);
            dResult = DBProxy.Current.Select(null, workorder_cmd, paras, out WorkorderDisTb);
            if (!dResult) return dResult;

            workorder_cmd = string.Format("Select {1},a.MarkerName,a.MarkerNo,MarkerLength,Cons,a.Layer,a.Cutno,a.colorid,c.seq,a.FabricPanelCode,b.* from Workorder a WITH (NOLOCK) ,Workorder_SizeRatio b WITH (NOLOCK) ,Order_SizeCode c WITH (NOLOCK) Where {1}>=@Cutref1 and {1}<=@Cutref2 and a.id='{0}' and a.ukey = b.workorderukey and a.id = c.id and b.id = c.id and b.sizecode = c.sizecode order by c.seq", detDr["ID"], byType);
            dResult = DBProxy.Current.Select(null, workorder_cmd, paras, out WorkorderSizeTb);
            if (!dResult) return dResult;

            workorder_cmd = string.Format("Select {1},b.*,Markername,a.FabricPanelCode from Workorder a,Workorder_PatternPanel b Where {1}>=@Cutref1 and {1}<=@Cutref2 and a.id='{0}' and a.ukey = b.workorderukey", detDr["ID"], byType);
            dResult = DBProxy.Current.Select(null, workorder_cmd, paras, out WorkorderPatternTb);
            if (!dResult) return dResult;

            MyUtility.Check.Seek(string.Format("Select * from Orders WITH (NOLOCK) Where id='{0}'", detDr["ID"]), out OrderDr);

            MyUtility.Tool.ProcessWithDatatable(WorkorderTb, string.Format("{0},estCutDate{1}", byType,byType2), string.Format("Select distinct {0},estCutDate{1} From #tmp ", byType, byType2), out CutrefTb);

            MyUtility.Tool.ProcessWithDatatable(WorkorderDisTb, string.Format("{0},OrderID",byType), string.Format("Select distinct {0},OrderID From #tmp",byType), out CutDisOrderIDTb); //整理sp

            MyUtility.Tool.ProcessWithDatatable(WorkorderSizeTb, string.Format("{0},MarkerName,MarkerNo,MarkerLength,SizeCode,Cons,Qty,seq,FabricPanelCode", byType), string.Format("Select distinct {0},MarkerName,MarkerNo,MarkerLength,SizeCode,Cons,Qty,seq,FabricPanelCode,dbo.MarkerLengthToYDS(MarkerLength) as yds From #tmp order by FabricPanelCode,MarkerName,seq", byType), out CutSizeTb); //整理SizeGroup,Qty

            MyUtility.Tool.ProcessWithDatatable(WorkorderSizeTb, string.Format("{0},SizeCode,seq", byType), string.Format("Select distinct {0},SizeCode,seq From #tmp order by seq ", byType), out SizeTb); //整理Size

            MyUtility.Tool.ProcessWithDatatable(WorkorderSizeTb, string.Format("{0},MarkerName", byType), string.Format("Select distinct {0},MarkerName From #tmp ", byType), out MarkerTB); //整理MarkerName

            MyUtility.Tool.ProcessWithDatatable(WorkorderTb, string.Format("{0},FabricPanelCode,SCIRefno,shc", byType), string.Format("Select distinct {0},a.FabricPanelCode,a.SCIRefno,b.Description,b.width,shc  From #tmp a Left Join Fabric b on a.SciRefno = b.SciRefno", byType), out FabricComboTb); //整理FabricPanelCode     

            if (radioByCutplanId.Checked)
            {
                string Issue_cmd = string.Format("Select a.Cutplanid,b.Qty,b.Dyelot,b.Roll,Max(c.yds) as yds,c.Colorid from Issue a WITH (NOLOCK) ,Issue_Detail b WITH (NOLOCK) , #tmp c Where a.id=b.id and c.Cutplanid = a.Cutplanid and c.SEQ1 = b.SEQ1 and c.SEQ2 = b.SEQ2 group by a.Cutplanid,b.Qty,b.Dyelot,b.Roll,c.Colorid order by Dyelot,roll", detDr["ID"], byType);
                MyUtility.Tool.ProcessWithDatatable(WorkorderTb, "Cutplanid,SEQ1,SEQ2,yds,Colorid", Issue_cmd, out IssueTb); //整理FabricPanelCode     
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

            //this.ShowWaitMessage("Starting EXCEL...");
            if (radioByCutRefNo.Checked) return ByCutrefExcel();
            else return ByRequestExcel();
            //return true;
        }
        
        public bool ByRequestExcel()
        {
            string strXltName = Sci.Env.Cfg.XltPathDir + "\\Cutting_P02_SpreadingReportbyRequest.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null) return false;
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            #region 寫入共用欄位
            worksheet.Cells[1, 6] = OrderDr["factoryid"];
            worksheet.Cells[3, 2] = DateTime.Now.ToShortDateString();
            worksheet.Cells[3, 7] = detDr["SpreadingNoID"];
            worksheet.Cells[3, 12] = detDr["CutCellid"];
            worksheet.Cells[9, 2] = OrderDr["Styleid"];
            worksheet.Cells[10, 2] = OrderDr["Seasonid"];
            worksheet.Cells[10, 13] = OrderDr["Sewline"];
            for (int nColumn = 3; nColumn <= 21; nColumn += 3)
            {
                worksheet.Cells[36, nColumn] = OrderDr["Styleid"];
                worksheet.Cells[37, nColumn] = detDr["ID"];
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
                spList = "";
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
                        worksheet.Cells[33, nColumn] = WorkorderArry[0]["Refno"];
                        worksheet.Cells[34, nColumn] = WorkorderArry[0]["Colorid"];
                    }
                    #endregion
                }
                int copyRow = 0;
                int RowRange = 6;
                int tmpn=13;
                if (FabricComboArry.Length > 0)
                {
                    foreach (DataRow FabricComboDr in FabricComboArry)
                    {
                        if (copyRow > 0)
                        {
                            Excel.Range r = worksheet.get_Range("A" + ((12 + RowRange * (copyRow - 1))).ToString(), "A" + ((12 + RowRange * (copyRow - 1)) + RowRange - 1).ToString()).EntireRow;
                            r.Copy();
                            r.Insert(Excel.XlInsertShiftDirection.xlShiftDown); //新增Row
                        }

                        WorkorderPatternArry = WorkorderPatternTb.Select(string.Format("Cutplanid='{0}' and FabricPanelCode = '{1}'", Cutplanid, FabricComboDr["FabricPanelCode"]), "PatternPanel");
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
                        worksheet.Cells[FabricRow, 2] = FabricComboDr["FabricPanelCode"].ToString();
                        worksheet.Cells[FabricRow, 5] = pattern;

                        string fd = FabricComboDr["Description"].ToString();
                        worksheet.Cells[FabricRow, 9] = fd;
                        int fl = 48;
                        int fla = fd.Length / fl;
                        for (int i = 1; i <= fla; i++)
                        {
                            if (fd.Length > fl * i)
                            {
                                Microsoft.Office.Interop.Excel.Range rangeRow13 = (Microsoft.Office.Interop.Excel.Range)worksheet.Rows[13, System.Type.Missing];
                                rangeRow13.RowHeight = 19.125 * (i + 1);
                            }
                        }

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
                    int l = 54;
                    int la = spList.Length / l;
                    for (int i = 1; i <= la; i++)
                    {
                        if (spList.Length > l * i)
                        {
                            Microsoft.Office.Interop.Excel.Range rangeRow8 = (Microsoft.Office.Interop.Excel.Range)worksheet.Rows[8, System.Type.Missing];
                            rangeRow8.RowHeight = 20.25 * (i + 1);
                        }
                    }
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
                            double unit = Convert.ToDouble(SizeArry[0]["yds"]) * 0.9144;
                            worksheet.Cells[nRow, 1] = SizeArry[0]["MarkerName"].ToString();
                            worksheet.Cells[nRow, 4] = SizeArry[0]["MarkerNo"].ToString();
                            worksheet.Cells[nRow, 6] = SizeArry[0]["MarkerLength"].ToString() + "\n" + SizeArry[0]["yds"].ToString() + "Y (" + unit + "M)";
                        }

                        worksheet.Cells[nRow, 10] = size;
                        worksheet.Cells[nRow, 12] = Ratio;

                        int l = 11;
                        int la = size.Length / l;
                        int la2 = Ratio.Length / l;
                        for (int i = 1; i <= la; i++)
                        {
                            if (size.Length > l * i)
                            {
                                Microsoft.Office.Interop.Excel.Range rangeRow12 = (Microsoft.Office.Interop.Excel.Range)worksheet.Rows[nRow, System.Type.Missing];
                                rangeRow12.RowHeight = 16.875 * (i + 1);
                            }
                        }
                          
                    }
                    #endregion   
                }
                #endregion
                tmpn=nRow + 2;
                nRow = nRow + 3; //Size
                string str_PIVOT = "";
                nSizeColumn = 4;
                DataRow[] FabricComboTbsia = FabricComboTb.Select(string.Format("Cutplanid = '{0}'", Cutplanid));
                foreach (DataRow dr in SizeCodeArry)
                {
                    str_PIVOT = str_PIVOT + string.Format("[{0}],", dr["SizeCode"].ToString());
                    //寫入Size
                    for (int i = 0; i < FabricComboTbsia.Length; i++)
                    {
                        worksheet.Cells[nRow+(RowRange*i), nSizeColumn] = dr["SizeCode"].ToString();
                    }
                    nSizeColumn++;
                }
                str_PIVOT = str_PIVOT.Substring(0, str_PIVOT.Length - 1);
                string Pivot_cmd = string.Format(
                @"Select * From
                (
                    Select FabricPanelCode,MarkerName,Cutno,Colorid,SizeCode,Cons,Layer,(Qty*Layer) as TotalQty from 
                    #tmp
                    Where Cutplanid = '{0} '
                ) as mTb
                Pivot(Sum(TotalQty)
                for SizeCode in ({1})) as pIvT 
                order by FabricPanelCode,Cutno,Colorid",
Cutplanid, str_PIVOT); 
                if (CutQtyTb != null)
                {
                    CutQtyTb.Clear();
                }
                MyUtility.Tool.ProcessWithDatatable(WorkorderSizeTb, "FabricPanelCode,MarkerName,Cutno,Colorid,SizeCode,Qty,Layer,Cutplanid,Cons", Pivot_cmd, out CutQtyTb);
                nRow = nRow + 1;
                bool lfirstComb = true;
                string fabColor = "";
                DataRow[] FabricComboTbsi = FabricComboTb.Select(string.Format("Cutplanid = '{0}'", Cutplanid));
                foreach (DataRow FabricComboDr in FabricComboTbsi)
                {
                    if (!MyUtility.Check.Empty(FabricComboDr["shc"]))
                    {
                        Microsoft.Office.Interop.Excel.Range rng = (Microsoft.Office.Interop.Excel.Range)worksheet.Rows[tmpn, Type.Missing];
                        rng.Insert(Microsoft.Office.Interop.Excel.XlDirection.xlDown);
                        Microsoft.Office.Interop.Excel.Range rng2 = (Microsoft.Office.Interop.Excel.Range)worksheet.get_Range("I" + tmpn, "U" + tmpn);
                        rng2.Merge();
                        rng2.Cells.Font.Color = Color.Red;
                        rng2.Cells.Font.Bold = true;
                        worksheet.Cells[tmpn, 9] = FabricComboDr["shc"].ToString();
                        tmpn++;
                        nRow++;
                    }
                    tmpn += 6;
                    DataRow[] CutQtyArray = CutQtyTb.Select(string.Format("FabricPanelCode = '{0}'", FabricComboDr["FabricPanelCode"]));
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
                                tmpn++;
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
            //重製Mode以取消Copy區塊
            worksheet.Application.CutCopyMode = Microsoft.Office.Interop.Excel.XlCutCopyMode.xlCopy;

            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Cutting_P02_SpreadingReportbyRequest");
            Excel.Workbook workbook = excel.Workbooks[1];
            workbook.SaveAs(strExcelName);
            workbook.Close();
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(workbook);
            Marshal.ReleaseComObject(worksheet);

            strExcelName.OpenFile();
            #endregion 
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
            worksheet.Cells[1, 6] = OrderDr["factoryid"];
            worksheet.Cells[3, 2] = DateTime.Now.ToShortDateString();
            worksheet.Cells[3, 7] = detDr["SpreadingNoID"];
            worksheet.Cells[3, 12] = detDr["CutCellid"];
            worksheet.Cells[9, 2] = OrderDr["Styleid"];
            worksheet.Cells[10, 2] = OrderDr["Seasonid"];
            worksheet.Cells[10, 13] = OrderDr["Sewline"];

            for (int nColumn = 3; nColumn <= 21; nColumn += 3)
            {
                worksheet.Cells[40, nColumn] = OrderDr["Styleid"];
                worksheet.Cells[41, nColumn] = detDr["ID"];
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
            int TotConsRowS = 18, TotConsRowE = 19;
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
                spList = "";
                worksheet = excel.ActiveWorkbook.Worksheets[nSheet];
                worksheet.Select();
                string cutref = Cutrefdr["Cutref"].ToString();
                #region 撈表身Detail Array
                WorkorderArry = WorkorderTb.Select(string.Format("Cutref = '{0}'", cutref));
                WorkorderDisArry = WorkorderDisTb.Select(string.Format("Cutref='{0}'", cutref));
                WorkorderSizeArry = WorkorderSizeTb.Select(string.Format("Cutref='{0}'", cutref));
                WorkorderPatternArry = WorkorderPatternTb.Select(string.Format("Cutref='{0}'", cutref), "PatternPanel");
                WorkorderOrderIDArry = CutDisOrderIDTb.Select(string.Format("Cutref='{0}'", cutref), "Orderid");
                SizeArry = (CutSizeTb.DefaultView.ToTable(true, new string[] { "Cutref" , "SizeCode" })).Select(string.Format("Cutref='{0}'", cutref));
                SizeCodeArry = SizeTb.Select(string.Format("Cutref='{0}'", cutref), "SEQ");
                #endregion

                if (WorkorderArry.Length > 0)
                {
                    pattern = "";
                    worksheet.Cells[8, 13] = WorkorderArry[0]["MarkerDownLoadId"].ToString();
                    worksheet.Cells[13, 2] = WorkorderArry[0]["FabricPanelCode"].ToString();
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

                    worksheet.Cells[13, 2] = WorkorderArry[0]["FabricPanelCode"].ToString();
                    worksheet.Cells[13, 5] = pattern;
                    string fd = "#" + WorkorderArry[0]["SCIRefno"].ToString().Trim() + " " + WorkorderArry[0]["Description"].ToString();
                    worksheet.Cells[13, 9] = fd;
                    int fl = 48;
                    int fla = fd.Length / fl;
                    for (int i = 1; i <= fla; i++)
                    {
                        if (fd.Length > fl * i)
                        {
                            Microsoft.Office.Interop.Excel.Range rangeRow13 = (Microsoft.Office.Interop.Excel.Range)worksheet.Rows[13, System.Type.Missing];
                            rangeRow13.RowHeight = 19.125 * (i + 1);
                        }
                    }        
                    worksheet.Cells[13, 20] = WorkorderArry[0]["width"].ToString();
                    #region 從後面開始寫 先寫Refno,Color

                    for (int nColumn = 3; nColumn <= 22; nColumn += 3)
                    {
                        worksheet.Cells[37, nColumn] = WorkorderArry[0]["Refno"];
                        worksheet.Cells[38, nColumn] = WorkorderArry[0]["Colorid"];
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
                    int l = 54;
                    int la = spList.Length / l;
                    for (int i = 1; i <= la; i++)
                    {
                        if (spList.Length > l*i)
                        {
                            Microsoft.Office.Interop.Excel.Range rangeRow8 = (Microsoft.Office.Interop.Excel.Range)worksheet.Rows[8, System.Type.Missing];
                            rangeRow8.RowHeight = 20.25*(i+1);
                        }  
                    }                                      
                }
                #endregion



                #region Markname
                int nrow = 12;


                if (SizeArry.Length > 0)
                {

                    size = "";
                    Ratio = "";
                    #region Size,Ratio
                    DataRow[] wtmp = WorkorderSizeTb.DefaultView.ToTable(false, new string[] { "Cutref", "SizeCode" }).Select(string.Format("Cutref='{0}'", cutref));                   
                    DataRow[] wtmp2 = WorkorderSizeTb.DefaultView.ToTable(false, new string[] { "Cutref", "Qty" }).Select(string.Format("Cutref='{0}'", cutref));
                    foreach (DataRow sDr in wtmp)
                    {
                        size = size + sDr["SizeCode"].ToString() + ",";
                    }
                    foreach (DataRow sDr in wtmp2)
                    {
                        Ratio = Ratio + MyUtility.Convert.GetDouble(sDr["Qty"]).ToString() + ",";
                    }
                    #endregion
                    double unit = Convert.ToDouble(WorkorderArry[0]["yds"]) * 0.9144;
                    worksheet.Cells[12, 1] = WorkorderArry[0]["MarkerName"].ToString();
                    worksheet.Cells[12, 4] = WorkorderArry[0]["MarkerNo"].ToString();
                    worksheet.Cells[12, 6] = WorkorderArry[0]["MarkerLength"].ToString() + "\n" + WorkorderArry[0]["yds"].ToString() + "Y (" + unit + "M)" ;
                    worksheet.Cells[12, 10] = size;
                    worksheet.Cells[12, 12] = Ratio;
                    int l = 11;
                    int la = size.Length / l;
                    int la2 = Ratio.Length / l;
                    for (int i = 1; i <= la; i++)
                    {
                        if (size.Length > l * i)
                        {
                            Microsoft.Office.Interop.Excel.Range rangeRow12 = (Microsoft.Office.Interop.Excel.Range)worksheet.Rows[12, System.Type.Missing];
                            rangeRow12.RowHeight = 16.875 * (i + 1);
                        }
                    }                 

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
                string Pivot_cmd = "";
                DualResult drwst;
                foreach (DataRow dr in SizeArry)
                {
                    str_PIVOT = str_PIVOT + string.Format("[{0}],", dr["SizeCode"].ToString());
                    //寫入Size
                    worksheet.Cells[nrow + 1, nSizeColumn] = dr["SizeCode"].ToString();
                    nSizeColumn++;
                }
                str_PIVOT = str_PIVOT.Substring(0, str_PIVOT.Length - 1);

                //if (_worktype == 2)
                //{
                    Pivot_cmd = string.Format(@"
Select Cutno,Colorid,SizeCode,Cons,Layer,workorderukey,(Qty*Layer) as TotalQty from 
#tmp
Where Cutref = '{0}'", cutref);
//                }
//                else
//                {                    
//                    Pivot_cmd = string.Format(
//@"Select * From
//(
//    Select Cutno,Colorid,SizeCode,Cons,Layer,(Qty*Layer) as TotalQty from 
//    #tmp
//    Where Cutref = '{0}'
//) as mTb
//Pivot(Sum(TotalQty)
//for SizeCode in ({1})) as pIvT 
//order by Cutno,Colorid", cutref, str_PIVOT);
//                }

                if (CutQtyTb != null)
                {
                    CutQtyTb.Clear();
                }
                drwst = MyUtility.Tool.ProcessWithDatatable(WorkorderSizeTb, "Cutno,Colorid,SizeCode,Qty,Layer,Cutref,Cons,workorderukey", Pivot_cmd, out CutQtyTb);
                if (!drwst)
                {
                    MyUtility.Msg.ErrorBox("SQL command Pivot_cmd error!");
                    return false;
                }
                nrow = nrow + 2;
                int copyrow = 0;
                TotConsRowS = nrow; //第一個Cons
                //foreach (DataRow cutqtydr in CutQtyTb.Rows)
                //{
                //    if (copyrow > 0)
                //    {
                //        Excel.Range r = worksheet.get_Range("A" + (nrow).ToString(), "A" + (nrow).ToString()).EntireRow;
                //        r.Copy();
                //        r.Insert(Excel.XlInsertShiftDirection.xlShiftDown, Excel.XlInsertFormatOrigin.xlFormatFromRightOrBelow); //新增Row
                //    }
                //    worksheet.Cells[nrow, 1] = cutqtydr["Cutno"].ToString();
                //    worksheet.Cells[nrow, 2] = cutqtydr["Colorid"].ToString();
                //    worksheet.Cells[nrow, 3] = cutqtydr["Layer"].ToString();
                //    worksheet.Cells[nrow, 20] = cutqtydr["Cons"].ToString();
                //    for (int nSizeDetail = 0; nSizeDetail < SizeArry.Length; nSizeDetail++)
                //    {
                //        worksheet.Cells[nrow, nSizeDetail + 4] = cutqtydr[5].ToString(); //+4因為從第四個Column 開始 nSizeDetail +4 是因為Table 從第四個開始是Size
                //    }
                //    nrow++;
                //    copyrow++;
                //}

                var distinct_CutQtyTb = from r1 in CutQtyTb.AsEnumerable()
                                        group r1 by new
                                        {
                                            Cutno = r1.Field<decimal>("Cutno"),
                                            Colorid = r1.Field<string>("Colorid"),
                                            Layer = r1.Field<decimal>("Layer"),
                                            workorderukey = r1.Field<long>("workorderukey"),
                                            Cons = r1.Field<decimal>("Cons")
                                        } into g
                                        select new
                                        {
                                            Cutno = g.Key.Cutno,
                                            Colorid = g.Key.Colorid,
                                           Layer = g.Key.Layer,
                                            workorderukey = g.Key.workorderukey,
                                            Cons = g.Key.Cons
                                        };

                foreach (var dis_dr in distinct_CutQtyTb)
                {
                    if (copyrow > 0)
                    {
                        Excel.Range r = worksheet.get_Range("A" + (nrow).ToString(), "A" + (nrow).ToString()).EntireRow;
                        r.Copy();
                        r.Insert(Excel.XlInsertShiftDirection.xlShiftDown, Excel.XlInsertFormatOrigin.xlFormatFromRightOrBelow); //新增Row
                    }
                    worksheet.Cells[nrow, 1] = dis_dr.Cutno;
                    worksheet.Cells[nrow, 2] = dis_dr.Colorid;
                    worksheet.Cells[nrow, 3] = dis_dr.Layer;
                    worksheet.Cells[nrow, 20] = dis_dr.Cons;

                    foreach (DataRow dr in CutQtyTb.Select(string.Format("workorderukey = '{0}'", dis_dr.workorderukey)))
                    {

                        for (int i = 0; i < SizeArry.Length; i++)
                        {
                            if (SizeArry[i].Field<string>("SizeCode").Equals(dr["SizeCode"]))
                            {
                                worksheet.Cells[nrow, i + 4] = dr["TotalQty"];

                            }

                        }

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

            nSheet = 1;
            foreach (DataRow Cutrefdr in CutrefTb.Rows)
            {
                worksheet = excel.ActiveWorkbook.Worksheets[nSheet];
                worksheet.Select();
                if (!MyUtility.Check.Empty(Cutrefdr["shc"]))
                {
                    Excel.Range r = worksheet.get_Range("A14", "A14").EntireRow;
                    r.Insert(Excel.XlInsertShiftDirection.xlShiftDown); //新增Row
                    Microsoft.Office.Interop.Excel.Range rng2 = (Microsoft.Office.Interop.Excel.Range)worksheet.get_Range("I14:U14");
                    rng2.Merge();
                    rng2.Cells.Font.Color = Color.Red;
                    rng2.Cells.Font.Bold = true;
                    worksheet.Cells[14, 9] = Cutrefdr["shc"];
                }
                nSheet++;
            }
            #endregion //End By CutRef
            //重製Mode以取消Copy區塊
            worksheet.Application.CutCopyMode = Microsoft.Office.Interop.Excel.XlCutCopyMode.xlCopy;
            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Cutting_P02_SpreadingReportbyCutref");
            Excel.Workbook workbook = excel.Workbooks[1];
            workbook.SaveAs(strExcelName);
            workbook.Close();
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            strExcelName.OpenFile();
            #endregion 
            return true;
        }
    }
}
