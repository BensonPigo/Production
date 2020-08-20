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
    /// <inheritdoc/>
    public partial class P02_Print : Win.Tems.PrintForm
    {
        private string S1;
        private string S2;
        private string Poid = string.Empty;
        private string cp;
        private string cr;
        private string keyword = Env.User.Keyword;
        private string cutrefSort;
        private int SheetCount = 1;
        private DataTable WorkorderTb;
        private DataTable WorkorderSizeTb;
        private DataTable WorkorderDisTb;
        private DataTable WorkorderPatternTb;
        private DataTable CutrefTb;
        private DataTable CutDisOrderIDTb;
        private DataTable CutSizeTb;
        private DataTable SizeTb;
        private DataTable CutQtyTb;
        private DataTable MarkerTB;
        private DataTable FabricComboTb;
        private DataTable IssueTb;
        private DataRow detDr;
        private DataRow OrderDr;
        private int _worktype;

        /// <summary>
        /// Initializes a new instance of the <see cref="P02_Print"/> class.
        /// </summary>
        /// <param name="workorderDr">workorder Dr</param>
        /// <param name="poid">POID</param>
        /// <param name="worktype">Work Type</param>
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

        /// <inheritdoc/>
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

        private void RadioByCutplanId_CheckedChanged(object sender, EventArgs e)
        {
            this.txtCutRefNoStart.Text = this.radioByCutplanId.Checked ? this.cp : this.cr;
            this.txtCutRefNoEnd.Text = this.radioByCutplanId.Checked ? this.cp : this.cr;
            this.labelCutRefNo.Text = this.radioByCutplanId.Checked ? "Cutplan ID" : "Cut RefNo";
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            List<SqlParameter> paras = new List<SqlParameter>
            {
                new SqlParameter("@Cutref1", this.S1),
                new SqlParameter("@Cutref2", this.S2),
            };
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
",
                this.detDr["ID"],
                byType,
                strOrderby);

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
                string issue_cmd = string.Format("Select a.Cutplanid,b.Qty,b.Dyelot,b.Roll,Max(c.yds) as yds,c.Colorid from Issue a WITH (NOLOCK) ,Issue_Detail b WITH (NOLOCK) , #tmp c Where a.id=b.id and c.Cutplanid = a.Cutplanid and c.SEQ1 = b.SEQ1 and c.SEQ2 = b.SEQ2 group by a.Cutplanid,b.Qty,b.Dyelot,b.Roll,c.Colorid order by Dyelot,roll", this.detDr["ID"], byType);
                MyUtility.Tool.ProcessWithDatatable(this.WorkorderTb, "Cutplanid,SEQ1,SEQ2,yds,Colorid", issue_cmd, out this.IssueTb); // 整理FabricPanelCode
            }

            return Ict.Result.True;
        }

        /// <inheritdoc/>
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

        /// <summary>
        /// By Request Excel
        /// </summary>
        /// <returns>bool</returns>
        public bool ByRequestExcel()
        {
            string strXltName = Env.Cfg.XltPathDir + "\\Cutting_P02_SpreadingReportbyRequest.xltx";
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
            DataRow[] workorderArry;
            DataRow[] workorderDisArry;
            DataRow[] workorderSizeArry;
            DataRow[] workorderPatternArry;
            DataRow[] workorderOrderIDArry;
            DataRow[] sizeArry;
            DataRow[] sizeCodeArry;
            DataRow[] markerArry;
            DataRow[] fabricComboArry;
            DataRow[] issueArry;
            string pattern = string.Empty, line = string.Empty;
            string size = string.Empty, ratio = string.Empty;
            int totConsRowS = 19, totConsRowE = 20, nSizeColumn = 0;
            foreach (DataRow cutrefdr1 in this.CutrefTb.Rows)
            {
                spList = string.Empty;

                // 有兩筆以上才做其他Sheet
                if (nSheet >= 2)
                {
                    worksheet = excel.ActiveWorkbook.Worksheets[nSheet - 1];
                    worksheet.Copy(Type.Missing, worksheet);
                }

                worksheet = excel.ActiveWorkbook.Worksheets[nSheet];
                worksheet.Select();
                worksheet.Name = cutrefdr1["Cutplanid"].ToString();
                worksheet.Cells[3, 19] = cutrefdr1["Cutplanid"].ToString();
                worksheet.Cells[9, 13] = ((DateTime)MyUtility.Convert.GetDate(cutrefdr1["Estcutdate"])).ToShortDateString();
                nSheet++;
            }

            nSheet = 1;
            #endregion
            foreach (DataRow cutrefdr in this.CutrefTb.Rows)
            {
                spList = string.Empty;
                worksheet = excel.ActiveWorkbook.Worksheets[nSheet];
                worksheet.Select();
                string cutplanid = cutrefdr["Cutplanid"].ToString();
                #region 撈表身Detail Array
                workorderArry = this.WorkorderTb.Select(string.Format("Cutplanid = '{0}'", cutplanid));
                workorderDisArry = this.WorkorderDisTb.Select(string.Format("Cutplanid='{0}'", cutplanid));
                workorderSizeArry = this.WorkorderSizeTb.Select(string.Format("Cutplanid='{0}'", cutplanid));
                workorderOrderIDArry = this.CutDisOrderIDTb.Select(string.Format("Cutplanid='{0}'", cutplanid), "Orderid");
                fabricComboArry = this.FabricComboTb.Select(string.Format("Cutplanid='{0}'", cutplanid));
                sizeCodeArry = this.SizeTb.Select(string.Format("Cutplanid='{0}'", cutplanid), "SEQ");
                markerArry = this.MarkerTB.Select(string.Format("Cutplanid = '{0}'", cutplanid));
                issueArry = this.IssueTb.Select(string.Format("Cutplanid = '{0}'", cutplanid));
                #endregion

                if (workorderArry.Length > 0)
                {
                    worksheet.Cells[8, 13] = workorderArry[0]["MarkerDownLoadId"].ToString();
                    worksheet.Cells[3, 7] = workorderArry[0]["SpreadingNoID"].ToString();
                    worksheet.Cells[3, 12] = workorderArry[0]["CutCellid"].ToString();
                    #region 從後面開始寫 先寫Refno,Color

                    for (int nColumn = 3; nColumn <= 22; nColumn += 3)
                    {
                        worksheet.Cells[33, nColumn] = workorderArry[0]["Refno"];
                        worksheet.Cells[34, nColumn] = workorderArry[0]["Colorid"];
                    }
                    #endregion
                }

                int copyRow = 0;
                int rowRange = 6;
                int tmpn = 13;
                if (fabricComboArry.Length > 0)
                {
                    foreach (DataRow fabricComboDr in fabricComboArry)
                    {
                        if (copyRow > 0)
                        {
                            Excel.Range r = worksheet.get_Range("A" + (12 + (rowRange * (copyRow - 1))).ToString(), "A" + ((12 + (rowRange * (copyRow - 1))) + rowRange - 1).ToString()).EntireRow;
                            r.Copy();
                            r.Insert(Excel.XlInsertShiftDirection.xlShiftDown); // 新增Row
                        }

                        workorderPatternArry = this.WorkorderPatternTb.Select(string.Format("Cutplanid='{0}' and FabricPanelCode = '{1}'", cutplanid, fabricComboDr["FabricPanelCode"]), "PatternPanel");
                        pattern = string.Empty;
                        if (workorderPatternArry.Length > 0)
                        {
                            foreach (DataRow patDr in workorderPatternArry)
                            {
                                if (!patDr["PatternPanel"].ToString().InList(pattern))
                                {
                                    pattern = pattern + patDr["PatternPanel"].ToString() + ",";
                                }
                            }
                        }

                        int fabricRow = 12 + (rowRange * copyRow);
                        worksheet.Cells[fabricRow, 2] = fabricComboDr["FabricPanelCode"].ToString();
                        worksheet.Cells[fabricRow, 5] = pattern;

                        string fd = fabricComboDr["Description"].ToString();
                        worksheet.Cells[fabricRow, 9] = fd;
                        int fl = 48;
                        int fla = fd.Length / fl;
                        for (int i = 1; i <= fla; i++)
                        {
                            if (fd.Length > fl * i)
                            {
                                Excel.Range rangeRow13 = (Excel.Range)worksheet.Rows[13, Type.Missing];
                                rangeRow13.RowHeight = 19.125 * (i + 1);
                            }
                        }

                        worksheet.Cells[fabricRow, 19] = fabricComboDr["width"].ToString();
                        copyRow++;
                    }
                }

                #region OrderSP List, Line List
                if (workorderOrderIDArry.Length > 0)
                {
                    foreach (DataRow disDr in workorderOrderIDArry)
                    {
                        if (disDr["OrderID"].ToString() != "EXCESS")
                        {
                            if (!disDr["OrderID"].ToString().InList(spList, "\\"))
                            {
                                spList = spList + disDr["OrderID"].ToString() + "\\";
                            }
                        }
                        #region SewingLine
                        line = line + MyUtility.GetValue.Lookup("Sewline", disDr["OrderID"].ToString(), "Orders", "ID") + "\\";
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
                            Excel.Range rangeRow8 = (Excel.Range)worksheet.Rows[8, Type.Missing];
                            rangeRow8.RowHeight = 20.25 * (i + 1);
                        }
                    }
                }
                #endregion

                #region Markname
                int nRow = 11;

                if (markerArry.Length > 0)
                {
                    size = string.Empty;
                    ratio = string.Empty;
                    #region Size,Ratio
                    foreach (DataRow markerDr in markerArry)
                    {
                        Excel.Range r = worksheet.get_Range("A" + nRow.ToString(), "A" + nRow.ToString()).EntireRow;
                        r.Copy();
                        r.Insert(Excel.XlInsertShiftDirection.xlShiftDown); // 新增Row
                        nRow++;

                        sizeArry = this.CutSizeTb.Select(string.Format("Cutplanid='{0}' and MarkerName = '{1}'", cutplanid, markerDr["MarkerName"]));
                        if (sizeArry.Length > 0)
                        {
                            size = string.Empty;
                            ratio = string.Empty;
                            foreach (DataRow sizeDr in sizeArry)
                            {
                                size = size + sizeDr["SizeCode"].ToString() + ",";
                                ratio = ratio + MyUtility.Convert.GetDouble(sizeDr["Qty"]).ToString() + ",";
                            }

                            double unit = Convert.ToDouble(sizeArry[0]["yds"]) * 0.9144;
                            worksheet.Cells[nRow, 1] = sizeArry[0]["MarkerName"].ToString();
                            worksheet.Cells[nRow, 4] = sizeArry[0]["MarkerNo"].ToString();
                            worksheet.Cells[nRow, 6] = sizeArry[0]["MarkerLength"].ToString() + "\n" + sizeArry[0]["yds"].ToString() + "Y (" + unit + "M)";
                        }

                        worksheet.Cells[nRow, 10] = size;
                        worksheet.Cells[nRow, 12] = ratio;

                        int l = 11;
                        int la = size.Length / l;
                        int la2 = ratio.Length / l;
                        for (int i = 1; i <= la; i++)
                        {
                            if (size.Length > l * i)
                            {
                                Excel.Range rangeRow12 = (Excel.Range)worksheet.Rows[nRow, Type.Missing];
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
                DataRow[] fabricComboTbsia = this.FabricComboTb.Select(string.Format("Cutplanid = '{0}'", cutplanid));
                foreach (DataRow dr in sizeCodeArry)
                {
                    str_PIVOT = str_PIVOT + string.Format("[{0}],", dr["SizeCode"].ToString());

                    // 寫入Size
                    for (int i = 0; i < fabricComboTbsia.Length; i++)
                    {
                        worksheet.Cells[nRow + (rowRange * i), nSizeColumn] = dr["SizeCode"].ToString();
                    }

                    nSizeColumn++;
                }

                str_PIVOT = str_PIVOT.Substring(0, str_PIVOT.Length - 1);
                string pivot_cmd = string.Format(
                @"Select * From
                (
                    Select FabricPanelCode,MarkerName,Cutno,Colorid,SizeCode,Cons,Layer,(Qty*Layer) as TotalQty from 
                    #tmp
                    Where Cutplanid = '{0} '
                ) as mTb
                Pivot(Sum(TotalQty)
                for SizeCode in ({1})) as pIvT 
                order by FabricPanelCode,Cutno,Colorid",
                cutplanid,
                str_PIVOT);
                if (this.CutQtyTb != null)
                {
                    this.CutQtyTb.Clear();
                }

                MyUtility.Tool.ProcessWithDatatable(this.WorkorderSizeTb, "FabricPanelCode,MarkerName,Cutno,Colorid,SizeCode,Qty,Layer,Cutplanid,Cons", pivot_cmd, out this.CutQtyTb);
                nRow = nRow + 1;
                bool lfirstComb = true;
                string fabColor = string.Empty;
                DataRow[] fabricComboTbsi = this.FabricComboTb.Select(string.Format("Cutplanid = '{0}'", cutplanid));
                foreach (DataRow fabricComboDr1 in fabricComboTbsi)
                {
                    if (!MyUtility.Check.Empty(fabricComboDr1["shc"]))
                    {
                        Excel.Range rng = (Excel.Range)worksheet.Rows[tmpn, Type.Missing];
                        rng.Insert(Excel.XlDirection.xlDown);
                        Excel.Range rng2 = (Excel.Range)worksheet.get_Range("I" + tmpn, "U" + tmpn);
                        rng2.Merge();
                        rng2.Cells.Font.Color = Color.Red;
                        rng2.Cells.Font.Bold = true;
                        worksheet.Cells[tmpn, 9] = fabricComboDr1["shc"].ToString();
                        tmpn++;
                        nRow++;
                    }

                    tmpn += 6;
                    DataRow[] cutQtyArray = this.CutQtyTb.Select(string.Format("FabricPanelCode = '{0}'", fabricComboDr1["FabricPanelCode"]));
                    if (cutQtyArray.Length > 0)
                    {
                        int copyrow = 0;
                        nRow = lfirstComb ? nRow : nRow + 4;
                        lfirstComb = false;
                        totConsRowS = nRow; // 第一個Cons
                        foreach (DataRow cutqtydr in cutQtyArray)
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
                            for (int nSizeDetail = 0; nSizeDetail < sizeCodeArry.Length; nSizeDetail++)
                            {
                                worksheet.Cells[nRow, nSizeDetail + 4] = cutqtydr[6 + nSizeDetail].ToString(); // +4因為從第四個Column 開始 nSizeDetail +4 是因為Table 從第四個開始是Size
                            }

                            nRow++;
                            copyrow++;
                        }

                        totConsRowE = nRow; // 最後一個Cons
                        #region Total Cons
                        nRow = nRow + 1;
                        worksheet.Cells[nRow, 20] = string.Format("=SUM(T{0}:T{1})", totConsRowS, totConsRowE);
                        worksheet.Cells[nRow, 18] = fabColor;
                        #endregion
                    }
                }

                nRow = nRow + 4; // Roll Table
                #region Issue Roll,Dyelot
                if (issueArry.Length > 0)
                {
                    bool lfirstdr = true;
                    foreach (DataRow issueDr in issueArry)
                    {
                        if (!lfirstdr)
                        {
                            Excel.Range r = worksheet.get_Range("A" + nRow.ToString(), "A" + nRow.ToString()).EntireRow;
                            r.Copy();
                            r.Insert(Excel.XlInsertShiftDirection.xlShiftDown, Excel.XlInsertFormatOrigin.xlFormatFromRightOrBelow); // 新增Row
                        }

                        lfirstdr = false;
                        worksheet.Cells[nRow, 1] = issueDr["Roll"].ToString();
                        worksheet.Cells[nRow, 2] = issueDr["Colorid"].ToString();
                        worksheet.Cells[nRow, 4] = issueDr["Dyelot"].ToString();
                        worksheet.Cells[nRow, 6] = issueDr["Qty"].ToString();

                        // 1401: CUTTING_P02_SpreadingReport。[LAYERS]欄位資料清空
                        // worksheet.Cells[nRow, 9] = MyUtility.Convert.GetDouble(IssueDr["yds"])==0? 0: Math.Ceiling(MyUtility.Convert.GetDouble(IssueDr["Qty"])/MyUtility.Convert.GetDouble(IssueDr["yds"]));
                        nRow++;
                    }
                }
                #endregion

                nSheet++;
            }

            // 重製Mode以取消Copy區塊
            worksheet.Application.CutCopyMode = Excel.XlCutCopyMode.xlCopy;

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("Cutting_P02_SpreadingReportbyRequest");
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

        /// <summary>
        /// By Cutref Excel
        /// </summary>
        /// <returns>bool</returns>
        public bool ByCutrefExcel()
        {
            int nSizeColumn;
            this.SheetCount = this.CutrefTb.Rows.Count;
            #region By Cutref
            string strXltName = Env.Cfg.XltPathDir + "\\Cutting_P02_SpreadingReportbyCutref.xltx";
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
            DataRow[] workorderArry;
            DataRow[] workorderDisArry;
            DataRow[] workorderSizeArry;
            DataRow[] workorderPatternArry;
            DataRow[] workorderOrderIDArry;
            DataRow[] sizeArry;
            DataRow[] sizeCodeArry;
            string pattern = string.Empty, line = string.Empty;
            int nDisCount = 0;
            double disRow = 0;
            string size = string.Empty, ratio = string.Empty;
            int totConsRowS = 18, totConsRowE = 19;
            foreach (DataRow cutrefdr in this.CutrefTb.Rows)
            {
                spList = string.Empty;

                // 有兩筆以上才做其他Sheet
                if (nSheet >= 2)
                {
                    worksheet = excel.ActiveWorkbook.Worksheets[nSheet - 1];
                    worksheet.Copy(Type.Missing, worksheet);
                }

                worksheet = excel.ActiveWorkbook.Worksheets[nSheet];
                worksheet.Select();
                worksheet.Name = cutrefdr["Cutref"].ToString();
                worksheet.Cells[3, 18] = cutrefdr["Cutref"].ToString();
                worksheet.Cells[9, 13] = ((DateTime)MyUtility.Convert.GetDate(cutrefdr["Estcutdate"])).ToShortDateString();
                worksheet.Cells[14, 14] = MyUtility.Convert.GetString(cutrefdr["FabricKind"]);
                nSheet++;
            }

            nSheet = 1;
            foreach (DataRow cutrefdr1 in this.CutrefTb.Rows)
            {
                Clipboard.SetDataObject(this.NewQRcode(MyUtility.Convert.GetString(cutrefdr1["Cutref"])));
                worksheet = excel.ActiveWorkbook.Worksheets[nSheet];
                Excel.Range rng = worksheet.get_Range("T2:U3");
                worksheet.Paste(rng, false);
                nSheet++;
            }

            nSheet = 1;
            foreach (DataRow cutrefdr2 in this.CutrefTb.Rows)
            {
                spList = string.Empty;
                worksheet = excel.ActiveWorkbook.Worksheets[nSheet];
                worksheet.Select();
                string cutref = cutrefdr2["Cutref"].ToString();
                #region 撈表身Detail Array
                workorderArry = this.WorkorderTb.Select(string.Format("Cutref = '{0}'", cutref));
                workorderDisArry = this.WorkorderDisTb.Select(string.Format("Cutref='{0}'", cutref));
                workorderSizeArry = this.WorkorderSizeTb.Select(string.Format("Cutref='{0}'", cutref));
                workorderPatternArry = this.WorkorderPatternTb.Select(string.Format("Cutref='{0}'", cutref), "PatternPanel");
                workorderOrderIDArry = this.CutDisOrderIDTb.Select(string.Format("Cutref='{0}'", cutref), "Orderid");
                sizeArry = this.CutSizeTb.DefaultView.ToTable(true, new string[] { "Cutref", "SizeCode" }).Select(string.Format("Cutref='{0}'", cutref));
                sizeCodeArry = this.SizeTb.Select(string.Format("Cutref='{0}'", cutref), "SEQ");
                #endregion

                if (workorderArry.Length > 0)
                {
                    pattern = string.Empty;
                    worksheet.Cells[8, 13] = workorderArry[0]["MarkerDownLoadId"].ToString();
                    worksheet.Cells[13, 2] = workorderArry[0]["FabricPanelCode"].ToString();
                    worksheet.Cells[3, 7] = workorderArry[0]["SpreadingNoID"].ToString();
                    worksheet.Cells[3, 12] = workorderArry[0]["CutCellid"].ToString();
                    if (workorderPatternArry.Length > 0)
                    {
                        foreach (DataRow patDr in workorderPatternArry)
                        {
                            if (!patDr["PatternPanel"].ToString().InList(pattern))
                            {
                                pattern = pattern + patDr["PatternPanel"].ToString() + ",";
                            }
                        }
                    }

                    worksheet.Cells[13, 2] = workorderArry[0]["FabricPanelCode"].ToString();
                    worksheet.Cells[13, 5] = pattern;
                    string fd = "#" + workorderArry[0]["SCIRefno"].ToString().Trim() + " " + workorderArry[0]["Description"].ToString();
                    worksheet.Cells[13, 9] = fd;
                    int fl = 48;
                    int fla = fd.Length / fl;
                    for (int i = 1; i <= fla; i++)
                    {
                        if (fd.Length > fl * i)
                        {
                            Excel.Range rangeRow13 = (Excel.Range)worksheet.Rows[13, Type.Missing];
                            rangeRow13.RowHeight = 19.125 * (i + 1);
                        }
                    }

                    worksheet.Cells[13, 20] = workorderArry[0]["width"].ToString();
                    #region 從後面開始寫 先寫Refno,Color

                    for (int nColumn = 3; nColumn <= 22; nColumn += 3)
                    {
                        worksheet.Cells[37, nColumn] = workorderArry[0]["Refno"];
                        worksheet.Cells[38, nColumn] = workorderArry[0]["Colorid"];
                    }
                    #endregion
                }
                #region OrderSP List, Line List
                if (workorderOrderIDArry.Length > 0)
                {
                    foreach (DataRow disDr in workorderOrderIDArry)
                    {
                        if (disDr["OrderID"].ToString() != "EXCESS")
                        {
                            spList = spList + disDr["OrderID"].ToString() + "\\";
                        }
                        #region SewingLine
                        line = line + MyUtility.GetValue.Lookup("Sewline", disDr["OrderID"].ToString(), "Orders", "ID") + "\\";
                        #endregion
                    }

                    worksheet.Cells[8, 2] = spList;
                    int l = 54;
                    int la = spList.Length / l;
                    for (int i = 1; i <= la; i++)
                    {
                        if (spList.Length > l * i)
                        {
                            Excel.Range rangeRow8 = (Excel.Range)worksheet.Rows[8, Type.Missing];
                            rangeRow8.RowHeight = 20.25 * (i + 1);
                        }
                    }
                }
                #endregion
                #region Markname
                int nrow = 12;

                if (sizeArry.Length > 0)
                {
                    size = string.Empty;
                    ratio = string.Empty;
                    #region Size,Ratio
                    DataRow[] wtmp = this.WorkorderSizeTb.DefaultView.ToTable(false, new string[] { "Cutref", "SizeCode" }).Select(string.Format("Cutref='{0}'", cutref));
                    DataRow[] wtmp2 = this.WorkorderSizeTb.DefaultView.ToTable(false, new string[] { "Cutref", "Qty" }).Select(string.Format("Cutref='{0}'", cutref));
                    foreach (DataRow sDr in wtmp)
                    {
                        size = size + sDr["SizeCode"].ToString() + ",";
                    }

                    foreach (DataRow sDr in wtmp2)
                    {
                        ratio = ratio + MyUtility.Convert.GetDouble(sDr["Qty"]).ToString() + ",";
                    }
                    #endregion
                    double unit = Convert.ToDouble(workorderArry[0]["yds"]) * 0.9144;
                    worksheet.Cells[12, 1] = workorderArry[0]["MarkerName"].ToString();
                    worksheet.Cells[12, 4] = workorderArry[0]["MarkerNo"].ToString();
                    worksheet.Cells[12, 6] = workorderArry[0]["MarkerLength"].ToString() + "\n" + workorderArry[0]["yds"].ToString() + "Y (" + unit + "M)";
                    worksheet.Cells[12, 10] = size;
                    worksheet.Cells[12, 12] = ratio;
                    int l = 11;
                    int la = size.Length / l;
                    int la2 = ratio.Length / l;
                    for (int i = 1; i <= la; i++)
                    {
                        if (size.Length > l * i)
                        {
                            Excel.Range rangeRow12 = (Excel.Range)worksheet.Rows[12, Type.Missing];
                            rangeRow12.RowHeight = 16.875 * (i + 1);
                        }
                    }
                }
                #endregion

                #region Distribute to SP#
                if (workorderDisArry.Length > 0)
                {
                    nrow = 16; // 到Distribute ROW
                    nDisCount = workorderDisArry.Length;
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
                        worksheet.Cells[nrow, 1] = workorderDisArry[arrayrow]["OrderID"].ToString();
                        worksheet.Cells[nrow, 4] = workorderDisArry[arrayrow]["Article"].ToString();
                        worksheet.Cells[nrow, 7] = workorderDisArry[arrayrow]["SizeCode"].ToString();
                        worksheet.Cells[nrow, 9] = workorderDisArry[arrayrow]["Qty"].ToString();
                        if (arrayrow + 1 < nDisCount)
                        {
                            worksheet.Cells[nrow, 11] = workorderDisArry[arrayrow + 1]["OrderID"].ToString();
                            worksheet.Cells[nrow, 14] = workorderDisArry[arrayrow + 1]["Article"].ToString();
                            worksheet.Cells[nrow, 17] = workorderDisArry[arrayrow + 1]["SizeCode"].ToString();
                            worksheet.Cells[nrow, 19] = workorderDisArry[arrayrow + 1]["Qty"].ToString();
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
                string pivot_cmd = string.Empty;
                DualResult drwst;
                foreach (DataRow dr in sizeArry)
                {
                    str_PIVOT = str_PIVOT + string.Format("[{0}],", dr["SizeCode"].ToString());

                    // 寫入Size
                    worksheet.Cells[nrow + 1, nSizeColumn] = dr["SizeCode"].ToString();
                    nSizeColumn++;
                }

                str_PIVOT = str_PIVOT.Substring(0, str_PIVOT.Length - 1);

                pivot_cmd = string.Format(
                    @"
Select Cutno,Colorid,SizeCode,Cons,Layer,workorderukey,(Qty*Layer) as TotalQty from 
#tmp
Where Cutref = '{0}'", cutref);

                if (this.CutQtyTb != null)
                {
                    this.CutQtyTb.Clear();
                }

                drwst = MyUtility.Tool.ProcessWithDatatable(this.WorkorderSizeTb, "Cutno,Colorid,SizeCode,Qty,Layer,Cutref,Cons,workorderukey", pivot_cmd, out this.CutQtyTb);
                if (!drwst)
                {
                    MyUtility.Msg.ErrorBox("SQL command Pivot_cmd error!");
                    return false;
                }

                nrow = nrow + 2;
                int copyrow = 0;
                totConsRowS = nrow; // 第一個Cons

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
                                            g.Key.Cutno,
                                            g.Key.Colorid,
                                            g.Key.Layer,
                                            g.Key.workorderukey,
                                            g.Key.Cons,
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
                        for (int i = 0; i < sizeArry.Length; i++)
                        {
                            if (sizeArry[i].Field<string>("SizeCode").Equals(dr["SizeCode"]))
                            {
                                worksheet.Cells[nrow, i + 4] = dr["TotalQty"];
                            }
                        }
                    }

                    nrow++;
                    copyrow++;
                }

                totConsRowE = nrow - 1; // 最後一個Cons
                #region Total Cons
                worksheet.Cells[nrow + 1, 20] = string.Format("=SUM(T{0}:T{1})", totConsRowS, totConsRowE);
                #endregion
                nSheet++;
            }

            nSheet = 1;
            foreach (DataRow cutrefdr3 in this.CutrefTb.Rows)
            {
                worksheet = excel.ActiveWorkbook.Worksheets[nSheet];
                worksheet.Select();
                if (!MyUtility.Check.Empty(cutrefdr3["shc"]))
                {
                    Excel.Range r = worksheet.get_Range("A14", "A14").EntireRow;
                    r.Insert(Excel.XlInsertShiftDirection.xlShiftDown); // 新增Row
                    Excel.Range rng2 = (Excel.Range)worksheet.get_Range("I14:U14");
                    rng2.Merge();
                    rng2.Cells.Font.Color = Color.Red;
                    rng2.Cells.Font.Bold = true;
                    worksheet.Cells[14, 9] = cutrefdr3["shc"];
                }

                nSheet++;
            }
            #endregion //End By CutRef

            // 重製Mode以取消Copy區塊
            worksheet.Application.CutCopyMode = Excel.XlCutCopyMode.xlCopy;
            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("Cutting_P02_SpreadingReportbyCutref");
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
