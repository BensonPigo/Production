using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using System.Data.SqlClient;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Linq;
using ZXing;
using ZXing.QrCode.Internal;
using ZXing.QrCode;

namespace Sci.Production.Cutting
{
    public partial class P02_Print : Win.Tems.PrintForm
    {
        string S1;
        string S2;
        string Poid = string.Empty;
        string cp;
        string cr;
        private string keyword = Sci.Env.User.Keyword;
        private string cutrefSort;
        int SheetCount = 1;
        DataTable WorkorderTb;
        DataTable WorkorderSizeTb;
        DataTable WorkorderDisTb;
        DataTable WorkorderPatternTb;
        DataTable CutrefTb;
        DataTable CutDisOrderIDTb;
        DataTable CutSizeTb;
        DataTable SizeTb;
        DataTable CutQtyTb;
        DataTable MarkerTB;
        DataTable FabricComboTb;
        DataTable IssueTb;
        DataRow detDr;
        DataRow OrderDr;
        int _worktype;

        public P02_Print(DataRow workorderDr, string poid, int worktype)
        {
            this.InitializeComponent();
            this.detDr = workorderDr;
            this.Poid = poid;
            this._worktype = worktype;
            this.radioByCutRefNo.Checked = true;
            this.txtCutRefNoStart.Text = this.detDr["CutRef"].ToString();
            this.txtCutRefNoEnd.Text = this.detDr["CutRef"].ToString();
            this.cr = this.detDr["CutRef"].ToString();
            this.cp = this.detDr["CutplanID"].ToString();
            this.txtCutRefNoStart.Select();
            this.cmbSort.SelectedIndex = 0;
        }

        protected override bool ValidateInput()
        {
            this.S1 = this.txtCutRefNoStart.Text;
            this.S2 = this.txtCutRefNoEnd.Text;

            if (MyUtility.Check.Empty(this.S1) || MyUtility.Check.Empty(this.S2))
            {
                MyUtility.Msg.WarningBox("<Range> can not be empty", "Warning");
                return false;
            }

            this.cutrefSort = this.cmbSort.Text;
            return base.ValidateInput();
        }

        private void radioByCutplanId_CheckedChanged(object sender, EventArgs e)
        {
            this.txtCutRefNoStart.Text = this.radioByCutplanId.Checked ? this.cp : this.cr;
            this.txtCutRefNoEnd.Text = this.radioByCutplanId.Checked ? this.cp : this.cr;
            this.labelCutRefNo.Text = this.radioByCutplanId.Checked ? "Cutplan ID" : "Cut RefNo";
        }

        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            List<SqlParameter> paras = new List<SqlParameter>();
            paras.Add(new SqlParameter("@Cutref1", this.S1));
            paras.Add(new SqlParameter("@Cutref2", this.S2));
            string byType, byType2;
            string strOrderby = string.Empty;

            string sqlFabricKind = string.Empty;
            string sqlFabricKindinto = string.Empty;
            string sqlFabricKindjoin = string.Empty;
            if (this.radioByCutRefNo.Checked)
            {
                byType = "Cutref";
                byType2 = ",shc";
                if (this.cutrefSort.ToLower().EqualString("CutRef#"))
                {
                    strOrderby = "order by Cutref";
                }
                else
                {
                    strOrderby = "order by SpreadingNoID,CutCellID,Cutref";
                }

                sqlFabricKind = $@"
SELECT distinct w.CutRef, wp.PatternPanel, x.FabricKind
into #tmp3
FROM #tmp W
INNER JOIN WorkOrder_PatternPanel WP ON W.Ukey = WP.WorkOrderUkey
outer apply(
	SELECT  FabricKind=DD.id + '-' + DD.NAME ,Refno
	FROM dropdownlist DD 
	OUTER apply(
			SELECT OB.kind, 
			OCC.id, 
			OCC.article, 
			OCC.colorid, 
			OCC.fabricpanelcode, 
			OCC.patternpanel ,
			Refno
		FROM order_colorcombo OCC 
		INNER JOIN order_bof OB ON OCC.id = OB.id AND OCC.fabriccode = OB.fabriccode
		where exists(select 1 from WorkOrder_Distribute wd where wd.WorkOrderUkey = W.Ukey and wd.Article = OCC.Article)
	) LIST 
	WHERE LIST.id = w.id 
	AND LIST.patternpanel = wp.patternpanel 
	AND DD.[type] = 'FabricKind' 
	AND DD.id = LIST.kind 
)x

select CutRef,ct = count(1) into #tmp4 from(select distinct CutRef,FabricKind from #tmp3)x group by CutRef

select t4.CutRef,FabricKind = IIF(t4.ct = 1, x1.FabricKind, x2.FabricKind)
into #tmp5
from #tmp4 t4
outer apply(
	select distinct t3.FabricKind
	from #tmp3 t3
	where t3.CutRef = t4.CutRef and t4.ct = 1
)x1
outer apply(
	select FabricKind = STUFF((
		select concat(', ', t3.FabricKind, ': ', t3.PatternPanel)
		from #tmp3 t3
		where t3.CutRef = t4.CutRef and t4.ct > 1
		for XML path('')
	),1,2,'')
)x2
";
                sqlFabricKindinto = $@" , rn=min(rn) into #tmp6 ";
                sqlFabricKindjoin = $@"select t6.*,t5.FabricKind from #tmp6 t6 inner join #tmp5 t5 on t5.CutRef = t6.CutRef order by rn";
            }
            else
            {
                byType = "Cutplanid";
                byType2 = string.Empty;
                strOrderby = "order by cutref";
            }

            string workorder_cmd = string.Format(
                @"
Select a.*,b.Description,b.width,dbo.MarkerLengthToYDS(MarkerLength) as yds 
    ,shc = iif(isnull(shc.RefNo,'')='','','Shrinkage Issue, Spreading Backward Speed: 2, Loose Tension')
from Workorder a WITH (NOLOCK)
Left Join Fabric b WITH (NOLOCK) on a.SciRefno = b.SciRefno 
outer apply(select RefNo from ShrinkageConcern where RefNo=a.RefNo and Junk=0) shc            
Where {1}>=@Cutref1 and {1}<=@Cutref2 
and a.id='{0}'
{2}
", this.detDr["ID"], byType, strOrderby);

            DualResult dResult = DBProxy.Current.Select(null, workorder_cmd, paras, out this.WorkorderTb);
            if (!dResult)
            {
                return dResult;
            }

            workorder_cmd = string.Format("Select {1},a.Cutno,a.Colorid,a.Layer,a.Cons,b.* from Workorder a WITH (NOLOCK) ,Workorder_Distribute b WITH (NOLOCK) Where {1}>=@Cutref1 and {1}<=@Cutref2 and a.id='{0}' and a.ukey = b.workorderukey order by b.OrderID,b.Article,b.SizeCode", this.detDr["ID"], byType);
            dResult = DBProxy.Current.Select(null, workorder_cmd, paras, out this.WorkorderDisTb);
            if (!dResult)
            {
                return dResult;
            }

            workorder_cmd = string.Format("Select {1},a.MarkerName,a.MarkerNo,MarkerLength,Cons,a.Layer,a.Cutno,a.colorid,c.seq,a.FabricPanelCode,b.* from Workorder a WITH (NOLOCK) ,Workorder_SizeRatio b WITH (NOLOCK) ,Order_SizeCode c WITH (NOLOCK) Where {1}>=@Cutref1 and {1}<=@Cutref2 and a.id='{0}' and a.ukey = b.workorderukey and a.id = c.id and b.id = c.id and b.sizecode = c.sizecode order by c.seq", this.detDr["ID"], byType);
            dResult = DBProxy.Current.Select(null, workorder_cmd, paras, out this.WorkorderSizeTb);
            if (!dResult)
            {
                return dResult;
            }

            workorder_cmd = string.Format("Select {1},b.*,Markername,a.FabricPanelCode from Workorder a,Workorder_PatternPanel b Where {1}>=@Cutref1 and {1}<=@Cutref2 and a.id='{0}' and a.ukey = b.workorderukey", this.detDr["ID"], byType);
            dResult = DBProxy.Current.Select(null, workorder_cmd, paras, out this.WorkorderPatternTb);
            if (!dResult)
            {
                return dResult;
            }

            MyUtility.Check.Seek(string.Format("Select * from Orders WITH (NOLOCK) Where id='{0}'", this.detDr["ID"]), out this.OrderDr);

            string sqlCutrefTb = $@"
Select {byType},estCutDate{byType2},rn=ROW_NUMBER()over({strOrderby} ) into #tmp2 From #tmp

{sqlFabricKind}

select {byType},estCutDate{byType2} {sqlFabricKindinto} from #tmp2 group by {byType},estCutDate{byType2} order by min(rn)

{sqlFabricKindjoin}
";
            MyUtility.Tool.ProcessWithDatatable(this.WorkorderTb, "SpreadingNoID,CutCellID,Cutref,Cutplanid,estCutDate,shc,ukey,id", sqlCutrefTb, out this.CutrefTb);

            MyUtility.Tool.ProcessWithDatatable(this.WorkorderDisTb, string.Format("{0},OrderID", byType), string.Format("Select distinct {0},OrderID From #tmp", byType), out this.CutDisOrderIDTb); // 整理sp

            MyUtility.Tool.ProcessWithDatatable(this.WorkorderSizeTb, string.Format("{0},MarkerName,MarkerNo,MarkerLength,SizeCode,Cons,Qty,seq,FabricPanelCode", byType), string.Format("Select distinct {0},MarkerName,MarkerNo,MarkerLength,SizeCode,Cons,Qty,seq,FabricPanelCode,dbo.MarkerLengthToYDS(MarkerLength) as yds From #tmp order by FabricPanelCode,MarkerName,seq", byType), out this.CutSizeTb); // 整理SizeGroup,Qty

            MyUtility.Tool.ProcessWithDatatable(this.WorkorderSizeTb, string.Format("{0},SizeCode,seq", byType), string.Format("Select distinct {0},SizeCode,seq From #tmp order by seq ", byType), out this.SizeTb); // 整理Size

            MyUtility.Tool.ProcessWithDatatable(this.WorkorderSizeTb, string.Format("{0},MarkerName", byType), string.Format("Select distinct {0},MarkerName From #tmp ", byType), out this.MarkerTB); // 整理MarkerName

            MyUtility.Tool.ProcessWithDatatable(this.WorkorderTb, string.Format("{0},FabricPanelCode,SCIRefno,shc", byType), string.Format("Select distinct {0},a.FabricPanelCode,a.SCIRefno,b.Description,b.width,shc  From #tmp a Left Join Fabric b on a.SciRefno = b.SciRefno", byType), out this.FabricComboTb); // 整理FabricPanelCode

            if (this.radioByCutplanId.Checked)
            {
                string Issue_cmd = string.Format("Select a.Cutplanid,b.Qty,b.Dyelot,b.Roll,Max(c.yds) as yds,c.Colorid from Issue a WITH (NOLOCK) ,Issue_Detail b WITH (NOLOCK) , #tmp c Where a.id=b.id and c.Cutplanid = a.Cutplanid and c.SEQ1 = b.SEQ1 and c.SEQ2 = b.SEQ2 group by a.Cutplanid,b.Qty,b.Dyelot,b.Roll,c.Colorid order by Dyelot,roll", this.detDr["ID"], byType);
                MyUtility.Tool.ProcessWithDatatable(this.WorkorderTb, "Cutplanid,SEQ1,SEQ2,yds,Colorid", Issue_cmd, out this.IssueTb); // 整理FabricPanelCode
            }

            return Result.True;
        }

        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            this.SheetCount = this.CutrefTb.Rows.Count;
            if (this.SheetCount == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            // this.ShowWaitMessage("Starting EXCEL...");
            if (this.radioByCutRefNo.Checked)
            {
                return this.ByCutrefExcel();
            }
            else
            {
                return this.ByRequestExcel();
            }

            // return true;
        }

        public bool ByRequestExcel()
        {
            string strXltName = Sci.Env.Cfg.XltPathDir + "\\Cutting_P02_SpreadingReportbyRequest.xltx";
            Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return false;
            }

            excel.Visible = false;
            Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            #region 寫入共用欄位
            worksheet.Cells[1, 6] = this.OrderDr["factoryid"];
            worksheet.Cells[3, 2] = DateTime.Now.ToShortDateString();
            worksheet.Cells[3, 7] = this.detDr["SpreadingNoID"];
            worksheet.Cells[3, 12] = this.detDr["CutCellid"];
            worksheet.Cells[9, 2] = this.OrderDr["Styleid"];
            worksheet.Cells[10, 2] = this.OrderDr["Seasonid"];
            worksheet.Cells[10, 13] = this.OrderDr["Sewline"];
            for (int nColumn = 3; nColumn <= 21; nColumn += 3)
            {
                worksheet.Cells[36, nColumn] = this.OrderDr["Styleid"];
                worksheet.Cells[37, nColumn] = this.detDr["ID"];
            }

            int nSheet = 1;
            string spList = string.Empty;
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
            string pattern = string.Empty, line = string.Empty;
            string size = string.Empty, Ratio = string.Empty;
            int TotConsRowS = 19, TotConsRowE = 20, nSizeColumn = 0;
            foreach (DataRow Cutrefdr in this.CutrefTb.Rows)
            {
                spList = string.Empty;

                if (nSheet >= 2) // 有兩筆以上才做其他Sheet
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
            foreach (DataRow Cutrefdr in this.CutrefTb.Rows)
            {
                spList = string.Empty;
                worksheet = excel.ActiveWorkbook.Worksheets[nSheet];
                worksheet.Select();
                string Cutplanid = Cutrefdr["Cutplanid"].ToString();
                #region 撈表身Detail Array
                WorkorderArry = this.WorkorderTb.Select(string.Format("Cutplanid = '{0}'", Cutplanid));
                WorkorderDisArry = this.WorkorderDisTb.Select(string.Format("Cutplanid='{0}'", Cutplanid));
                WorkorderSizeArry = this.WorkorderSizeTb.Select(string.Format("Cutplanid='{0}'", Cutplanid));
                WorkorderOrderIDArry = this.CutDisOrderIDTb.Select(string.Format("Cutplanid='{0}'", Cutplanid), "Orderid");
                FabricComboArry = this.FabricComboTb.Select(string.Format("Cutplanid='{0}'", Cutplanid));
                SizeCodeArry = this.SizeTb.Select(string.Format("Cutplanid='{0}'", Cutplanid), "SEQ");
                MarkerArry = this.MarkerTB.Select(string.Format("Cutplanid = '{0}'", Cutplanid));
                IssueArry = this.IssueTb.Select(string.Format("Cutplanid = '{0}'", Cutplanid));
                #endregion

                if (WorkorderArry.Length > 0)
                {
                    worksheet.Cells[8, 13] = WorkorderArry[0]["MarkerDownLoadId"].ToString();
                    worksheet.Cells[3, 7] = WorkorderArry[0]["SpreadingNoID"].ToString();
                    worksheet.Cells[3, 12] = WorkorderArry[0]["CutCellid"].ToString();
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
                int tmpn = 13;
                if (FabricComboArry.Length > 0)
                {
                    foreach (DataRow FabricComboDr in FabricComboArry)
                    {
                        if (copyRow > 0)
                        {
                            Excel.Range r = worksheet.get_Range("A" + (12 + (RowRange * (copyRow - 1))).ToString(), "A" + ((12 + (RowRange * (copyRow - 1))) + RowRange - 1).ToString()).EntireRow;
                            r.Copy();
                            r.Insert(Excel.XlInsertShiftDirection.xlShiftDown); // 新增Row
                        }

                        WorkorderPatternArry = this.WorkorderPatternTb.Select(string.Format("Cutplanid='{0}' and FabricPanelCode = '{1}'", Cutplanid, FabricComboDr["FabricPanelCode"]), "PatternPanel");
                        pattern = string.Empty;
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

                        int FabricRow = 12 + (RowRange * copyRow);
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
                                Excel.Range rangeRow13 = (Excel.Range)worksheet.Rows[13, System.Type.Missing];
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
                            if (!DisDr["OrderID"].ToString().InList(spList, "\\"))
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
                            Excel.Range rangeRow8 = (Excel.Range)worksheet.Rows[8, System.Type.Missing];
                            rangeRow8.RowHeight = 20.25 * (i + 1);
                        }
                    }
                }
                #endregion

                #region Markname
                int nRow = 11;

                if (MarkerArry.Length > 0)
                {
                    size = string.Empty;
                    Ratio = string.Empty;
                    #region Size,Ratio
                    foreach (DataRow MarkerDr in MarkerArry)
                    {
                        Excel.Range r = worksheet.get_Range("A" + nRow.ToString(), "A" + nRow.ToString()).EntireRow;
                        r.Copy();
                        r.Insert(Excel.XlInsertShiftDirection.xlShiftDown); // 新增Row
                        nRow++;

                        SizeArry = this.CutSizeTb.Select(string.Format("Cutplanid='{0}' and MarkerName = '{1}'", Cutplanid, MarkerDr["MarkerName"]));
                        if (SizeArry.Length > 0)
                        {
                            size = string.Empty;
                            Ratio = string.Empty;
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
                                Excel.Range rangeRow12 = (Excel.Range)worksheet.Rows[nRow, System.Type.Missing];
                                rangeRow12.RowHeight = 16.875 * (i + 1);
                            }
                        }
                    }
                    #endregion
                }
                #endregion
                tmpn = nRow + 2;
                nRow = nRow + 3; // Size
                string str_PIVOT = string.Empty;
                nSizeColumn = 4;
                DataRow[] FabricComboTbsia = this.FabricComboTb.Select(string.Format("Cutplanid = '{0}'", Cutplanid));
                foreach (DataRow dr in SizeCodeArry)
                {
                    str_PIVOT = str_PIVOT + string.Format("[{0}],", dr["SizeCode"].ToString());

                    // 寫入Size
                    for (int i = 0; i < FabricComboTbsia.Length; i++)
                    {
                        worksheet.Cells[nRow + (RowRange * i), nSizeColumn] = dr["SizeCode"].ToString();
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
                if (this.CutQtyTb != null)
                {
                    this.CutQtyTb.Clear();
                }

                MyUtility.Tool.ProcessWithDatatable(this.WorkorderSizeTb, "FabricPanelCode,MarkerName,Cutno,Colorid,SizeCode,Qty,Layer,Cutplanid,Cons", Pivot_cmd, out this.CutQtyTb);
                nRow = nRow + 1;
                bool lfirstComb = true;
                string fabColor = string.Empty;
                DataRow[] FabricComboTbsi = this.FabricComboTb.Select(string.Format("Cutplanid = '{0}'", Cutplanid));
                foreach (DataRow FabricComboDr in FabricComboTbsi)
                {
                    if (!MyUtility.Check.Empty(FabricComboDr["shc"]))
                    {
                        Excel.Range rng = (Excel.Range)worksheet.Rows[tmpn, Type.Missing];
                        rng.Insert(Microsoft.Office.Interop.Excel.XlDirection.xlDown);
                        Excel.Range rng2 = (Excel.Range)worksheet.get_Range("I" + tmpn, "U" + tmpn);
                        rng2.Merge();
                        rng2.Cells.Font.Color = Color.Red;
                        rng2.Cells.Font.Bold = true;
                        worksheet.Cells[tmpn, 9] = FabricComboDr["shc"].ToString();
                        tmpn++;
                        nRow++;
                    }

                    tmpn += 6;
                    DataRow[] CutQtyArray = this.CutQtyTb.Select(string.Format("FabricPanelCode = '{0}'", FabricComboDr["FabricPanelCode"]));
                    if (CutQtyArray.Length > 0)
                    {
                        int copyrow = 0;
                        nRow = lfirstComb ? nRow : nRow + 4;
                        lfirstComb = false;
                        TotConsRowS = nRow; // 第一個Cons
                        foreach (DataRow cutqtydr in CutQtyArray)
                        {
                            if (copyrow > 0)
                            {
                                Excel.Range r = worksheet.get_Range("A" + nRow.ToString(), "A" + nRow.ToString()).EntireRow;
                                r.Copy();
                                r.Insert(Excel.XlInsertShiftDirection.xlShiftDown, Excel.XlInsertFormatOrigin.xlFormatFromRightOrBelow); // 新增Row
                                tmpn++;
                            }

                            worksheet.Cells[nRow, 1] = cutqtydr["Cutno"].ToString();
                            worksheet.Cells[nRow, 2] = cutqtydr["Colorid"].ToString();
                            worksheet.Cells[nRow, 3] = cutqtydr["Layer"].ToString();
                            worksheet.Cells[nRow, 20] = cutqtydr["Cons"].ToString();
                            fabColor = cutqtydr["Colorid"].ToString();
                            for (int nSizeDetail = 0; nSizeDetail < SizeCodeArry.Length; nSizeDetail++)
                            {
                                worksheet.Cells[nRow, nSizeDetail + 4] = cutqtydr[6 + nSizeDetail].ToString(); // +4因為從第四個Column 開始 nSizeDetail +4 是因為Table 從第四個開始是Size
                            }

                            nRow++;
                            copyrow++;
                        }

                        TotConsRowE = nRow; // 最後一個Cons
                        #region Total Cons
                        nRow = nRow + 1;
                        worksheet.Cells[nRow, 20] = string.Format("=SUM(T{0}:T{1})", TotConsRowS, TotConsRowE);
                        worksheet.Cells[nRow, 18] = fabColor;
                        #endregion
                    }
                }

                nRow = nRow + 4; // Roll Table
                #region Issue Roll,Dyelot
                if (IssueArry.Length > 0)
                {
                    bool lfirstdr = true;
                    foreach (DataRow IssueDr in IssueArry)
                    {
                        if (!lfirstdr)
                        {
                            Excel.Range r = worksheet.get_Range("A" + nRow.ToString(), "A" + nRow.ToString()).EntireRow;
                            r.Copy();
                            r.Insert(Excel.XlInsertShiftDirection.xlShiftDown, Excel.XlInsertFormatOrigin.xlFormatFromRightOrBelow); // 新增Row
                        }

                        lfirstdr = false;
                        worksheet.Cells[nRow, 1] = IssueDr["Roll"].ToString();
                        worksheet.Cells[nRow, 2] = IssueDr["Colorid"].ToString();
                        worksheet.Cells[nRow, 4] = IssueDr["Dyelot"].ToString();
                        worksheet.Cells[nRow, 6] = IssueDr["Qty"].ToString();

                        // 1401: CUTTING_P02_SpreadingReport。[LAYERS]欄位資料清空
                        // worksheet.Cells[nRow, 9] = MyUtility.Convert.GetDouble(IssueDr["yds"])==0? 0: Math.Ceiling(MyUtility.Convert.GetDouble(IssueDr["Qty"])/MyUtility.Convert.GetDouble(IssueDr["yds"]));
                        nRow++;
                    }
                }
                #endregion

                nSheet++;
            }

            // 重製Mode以取消Copy區塊
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
            this.SheetCount = this.CutrefTb.Rows.Count;
            #region By Cutref
            string strXltName = Sci.Env.Cfg.XltPathDir + "\\Cutting_P02_SpreadingReportbyCutref.xltx";
            Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return false;
            }

            Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            #region 寫入共用欄位
            worksheet.Cells[1, 6] = this.OrderDr["factoryid"];
            worksheet.Cells[3, 2] = DateTime.Now.ToShortDateString();

            // worksheet.Cells[3, 7] = detDr["SpreadingNoID"];
            // worksheet.Cells[3, 12] = detDr["CutCellid"];
            worksheet.Cells[9, 2] = this.OrderDr["Styleid"];
            worksheet.Cells[10, 2] = this.OrderDr["Seasonid"];
            worksheet.Cells[10, 13] = this.OrderDr["Sewline"];

            for (int nColumn = 3; nColumn <= 21; nColumn += 3)
            {
                worksheet.Cells[40, nColumn] = this.OrderDr["Styleid"];
                worksheet.Cells[41, nColumn] = this.detDr["ID"];
            }

            #endregion

            int nSheet = 1;
            string spList = string.Empty;
            DataRow[] WorkorderArry;
            DataRow[] WorkorderDisArry;
            DataRow[] WorkorderSizeArry;
            DataRow[] WorkorderPatternArry;
            DataRow[] WorkorderOrderIDArry;
            DataRow[] SizeArry;
            DataRow[] SizeCodeArry;
            string pattern = string.Empty, line = string.Empty;
            int nDisCount = 0;
            double disRow = 0;
            string size = string.Empty, Ratio = string.Empty;
            int TotConsRowS = 18, TotConsRowE = 19;
            foreach (DataRow Cutrefdr in this.CutrefTb.Rows)
            {
                spList = string.Empty;

                if (nSheet >= 2) // 有兩筆以上才做其他Sheet
                {
                    worksheet = excel.ActiveWorkbook.Worksheets[nSheet - 1];
                    worksheet.Copy(Type.Missing, worksheet);
                }

                worksheet = excel.ActiveWorkbook.Worksheets[nSheet];
                worksheet.Select();
                worksheet.Name = Cutrefdr["Cutref"].ToString();
                worksheet.Cells[3, 18] = Cutrefdr["Cutref"].ToString();
                worksheet.Cells[9, 13] = ((DateTime)MyUtility.Convert.GetDate(Cutrefdr["Estcutdate"])).ToShortDateString();
                worksheet.Cells[14, 14] = MyUtility.Convert.GetString(Cutrefdr["FabricKind"]);
                nSheet++;
            }

            nSheet = 1;
            foreach (DataRow Cutrefdr in this.CutrefTb.Rows)
            {
                Clipboard.SetDataObject(this.NewQRcode(MyUtility.Convert.GetString(Cutrefdr["Cutref"])));
                worksheet = excel.ActiveWorkbook.Worksheets[nSheet];
                Excel.Range rng = worksheet.get_Range("T2:U3");
                worksheet.Paste(rng, false);
                nSheet++;
            }

            nSheet = 1;
            foreach (DataRow Cutrefdr in this.CutrefTb.Rows)
            {
                spList = string.Empty;
                worksheet = excel.ActiveWorkbook.Worksheets[nSheet];
                worksheet.Select();
                string cutref = Cutrefdr["Cutref"].ToString();
                #region 撈表身Detail Array
                WorkorderArry = this.WorkorderTb.Select(string.Format("Cutref = '{0}'", cutref));
                WorkorderDisArry = this.WorkorderDisTb.Select(string.Format("Cutref='{0}'", cutref));
                WorkorderSizeArry = this.WorkorderSizeTb.Select(string.Format("Cutref='{0}'", cutref));
                WorkorderPatternArry = this.WorkorderPatternTb.Select(string.Format("Cutref='{0}'", cutref), "PatternPanel");
                WorkorderOrderIDArry = this.CutDisOrderIDTb.Select(string.Format("Cutref='{0}'", cutref), "Orderid");
                SizeArry = this.CutSizeTb.DefaultView.ToTable(true, new string[] { "Cutref", "SizeCode" }).Select(string.Format("Cutref='{0}'", cutref));
                SizeCodeArry = this.SizeTb.Select(string.Format("Cutref='{0}'", cutref), "SEQ");
                #endregion

                if (WorkorderArry.Length > 0)
                {
                    pattern = string.Empty;
                    worksheet.Cells[8, 13] = WorkorderArry[0]["MarkerDownLoadId"].ToString();
                    worksheet.Cells[13, 2] = WorkorderArry[0]["FabricPanelCode"].ToString();
                    worksheet.Cells[3, 7] = WorkorderArry[0]["SpreadingNoID"].ToString();
                    worksheet.Cells[3, 12] = WorkorderArry[0]["CutCellid"].ToString();
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
                            Excel.Range rangeRow13 = (Excel.Range)worksheet.Rows[13, System.Type.Missing];
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
                        if (spList.Length > l * i)
                        {
                            Excel.Range rangeRow8 = (Excel.Range)worksheet.Rows[8, System.Type.Missing];
                            rangeRow8.RowHeight = 20.25 * (i + 1);
                        }
                    }
                }
                #endregion
                #region Markname
                int nrow = 12;

                if (SizeArry.Length > 0)
                {
                    size = string.Empty;
                    Ratio = string.Empty;
                    #region Size,Ratio
                    DataRow[] wtmp = this.WorkorderSizeTb.DefaultView.ToTable(false, new string[] { "Cutref", "SizeCode" }).Select(string.Format("Cutref='{0}'", cutref));
                    DataRow[] wtmp2 = this.WorkorderSizeTb.DefaultView.ToTable(false, new string[] { "Cutref", "Qty" }).Select(string.Format("Cutref='{0}'", cutref));
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
                    worksheet.Cells[12, 6] = WorkorderArry[0]["MarkerLength"].ToString() + "\n" + WorkorderArry[0]["yds"].ToString() + "Y (" + unit + "M)";
                    worksheet.Cells[12, 10] = size;
                    worksheet.Cells[12, 12] = Ratio;
                    int l = 11;
                    int la = size.Length / l;
                    int la2 = Ratio.Length / l;
                    for (int i = 1; i <= la; i++)
                    {
                        if (size.Length > l * i)
                        {
                            Excel.Range rangeRow12 = (Excel.Range)worksheet.Rows[12, System.Type.Missing];
                            rangeRow12.RowHeight = 16.875 * (i + 1);
                        }
                    }
                }
                #endregion

                #region Distribute to SP#
                if (WorkorderDisArry.Length > 0)
                {
                    nrow = 16; // 到Distribute ROW
                    nDisCount = WorkorderDisArry.Length;
                    disRow = Math.Ceiling(Convert.ToDouble(nDisCount) / 2); // 每一個Row 有兩個可以用
                    int arrayrow = 0;
                    for (int i = 0; i < disRow; i++)
                    {
                        if (i > 0)
                        {
                            Excel.Range r = worksheet.get_Range("A" + (nrow - 1).ToString(), "A" + (nrow - 1).ToString()).EntireRow;
                            r.Copy();
                            r.Insert(Excel.XlInsertShiftDirection.xlShiftDown, Excel.XlInsertFormatOrigin.xlFormatFromRightOrBelow); // 新增Row
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
                            worksheet.Cells[nrow, 11] = string.Empty;
                            worksheet.Cells[nrow, 14] = string.Empty;
                            worksheet.Cells[nrow, 17] = string.Empty;
                            worksheet.Cells[nrow, 19] = string.Empty;
                        }

                        nrow++;
                    }

                    // nrow = nrow + Convert.ToInt16(disRow);
                }
                #endregion

                string str_PIVOT = string.Empty;
                nSizeColumn = 4;
                string Pivot_cmd = string.Empty;
                DualResult drwst;
                foreach (DataRow dr in SizeArry)
                {
                    str_PIVOT = str_PIVOT + string.Format("[{0}],", dr["SizeCode"].ToString());

                    // 寫入Size
                    worksheet.Cells[nrow + 1, nSizeColumn] = dr["SizeCode"].ToString();
                    nSizeColumn++;
                }

                str_PIVOT = str_PIVOT.Substring(0, str_PIVOT.Length - 1);

                Pivot_cmd = string.Format(
                    @"
Select Cutno,Colorid,SizeCode,Cons,Layer,workorderukey,(Qty*Layer) as TotalQty from 
#tmp
Where Cutref = '{0}'", cutref);

                if (this.CutQtyTb != null)
                {
                    this.CutQtyTb.Clear();
                }

                drwst = MyUtility.Tool.ProcessWithDatatable(this.WorkorderSizeTb, "Cutno,Colorid,SizeCode,Qty,Layer,Cutref,Cons,workorderukey", Pivot_cmd, out this.CutQtyTb);
                if (!drwst)
                {
                    MyUtility.Msg.ErrorBox("SQL command Pivot_cmd error!");
                    return false;
                }

                nrow = nrow + 2;
                int copyrow = 0;
                TotConsRowS = nrow; // 第一個Cons

                var distinct_CutQtyTb = from r1 in this.CutQtyTb.AsEnumerable()
                                        group r1 by new
                                        {
                                            Cutno = r1.Field<decimal>("Cutno"),
                                            Colorid = r1.Field<string>("Colorid"),
                                            Layer = r1.Field<decimal>("Layer"),
                                            workorderukey = r1.Field<long>("workorderukey"),
                                            Cons = r1.Field<decimal>("Cons"),
                                        }
                                        into g
                                        select new
                                        {
                                            Cutno = g.Key.Cutno,
                                            Colorid = g.Key.Colorid,
                                            Layer = g.Key.Layer,
                                            workorderukey = g.Key.workorderukey,
                                            Cons = g.Key.Cons,
                                        };

                foreach (var dis_dr in distinct_CutQtyTb)
                {
                    if (copyrow > 0)
                    {
                        Excel.Range r = worksheet.get_Range("A" + nrow.ToString(), "A" + nrow.ToString()).EntireRow;
                        r.Copy();
                        r.Insert(Excel.XlInsertShiftDirection.xlShiftDown, Excel.XlInsertFormatOrigin.xlFormatFromRightOrBelow); // 新增Row
                    }

                    worksheet.Cells[nrow, 1] = dis_dr.Cutno;
                    worksheet.Cells[nrow, 2] = dis_dr.Colorid;
                    worksheet.Cells[nrow, 3] = dis_dr.Layer;
                    worksheet.Cells[nrow, 20] = dis_dr.Cons;

                    foreach (DataRow dr in this.CutQtyTb.Select(string.Format("workorderukey = '{0}'", dis_dr.workorderukey)))
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

                TotConsRowE = nrow - 1; // 最後一個Cons
                #region Total Cons
                worksheet.Cells[nrow + 1, 20] = string.Format("=SUM(T{0}:T{1})", TotConsRowS, TotConsRowE);
                #endregion
                nSheet++;
            }

            nSheet = 1;
            foreach (DataRow Cutrefdr in this.CutrefTb.Rows)
            {
                worksheet = excel.ActiveWorkbook.Worksheets[nSheet];
                worksheet.Select();
                if (!MyUtility.Check.Empty(Cutrefdr["shc"]))
                {
                    Excel.Range r = worksheet.get_Range("A14", "A14").EntireRow;
                    r.Insert(Excel.XlInsertShiftDirection.xlShiftDown); // 新增Row
                    Excel.Range rng2 = (Excel.Range)worksheet.get_Range("I14:U14");
                    rng2.Merge();
                    rng2.Cells.Font.Color = Color.Red;
                    rng2.Cells.Font.Bold = true;
                    worksheet.Cells[14, 9] = Cutrefdr["shc"];
                }

                nSheet++;
            }
            #endregion //End By CutRef

            // 重製Mode以取消Copy區塊
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

        private Bitmap NewQRcode(string strBarcode)
        {
            /*
  Level L (Low)      7%  of codewords can be restored.
  Level M (Medium)   15% of codewords can be restored.
  Level Q (Quartile) 25% of codewords can be restored.
  Level H (High)     30% of codewords can be restored.
*/
            BarcodeWriter writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions
                {
                    // Create Photo
                    Height = 79,
                    Width = 79,
                    Margin = 0,
                    CharacterSet = "UTF-8",
                    PureBarcode = true,

                    // 錯誤修正容量
                    // L水平    7%的字碼可被修正
                    // M水平    15%的字碼可被修正
                    // Q水平    25%的字碼可被修正
                    // H水平    30%的字碼可被修正
                    ErrorCorrection = ErrorCorrectionLevel.H,
                },
            };

            // Bitmap resizeQRcode = new Bitmap(writer.Write(strBarcode), new Size(38, 38));
            return writer.Write(strBarcode);
        }
    }
}
