using Ict;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sxrc = Sci.Utility.Excel.SaveXltReportCls;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using Sci.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using ZXing.QrCode.Internal;
using ZXing;
using ZXing.QrCode;
using Sci.Andy;

namespace Sci.Production.Cutting
{
    /// <summary>
    /// P02, P09共用
    /// </summary>
    public partial class CuttingWorkOrder
    {
        /// <summary>
        /// Key = Table名稱，Value = 無欄位的清單
        /// </summary>
        public Dictionary<string, List<string>> DicTableLostColumns
        {
            get
            {
                if (this._dicTableLostColumns == null)
                {
                    this.GetDictionaryTableLostColumns();
                }

                return this._dicTableLostColumns;
            }

            private set
            {

            }
        }

        private Dictionary<string, List<string>> _dicTableLostColumns;

        /// <summary>
        /// For Cutting P02、P09的範本檔下載
        /// </summary>
        /// <param name="fromCutting">Type FromCutting</param>
        /// <param name="errMsg">錯誤訊息</param>
        /// <returns>成功失敗</returns>
        public bool DownloadSampleFile(FromCutting fromCutting, out string errMsg)
        {
            errMsg = string.Empty;
            string xltxName = fromCutting == FromCutting.P02 ? "Cutting_P02. Import Marker Template Download" : "Cutting_P09. Import Marker Template Download";
            try
            {
                Sci.Utility.Excel.SaveXltReportCls xl = new Sci.Utility.Excel.SaveXltReportCls($"{xltxName}.xltx");
                xl.BoOpenFile = true;
                xl.Save();
                return true;
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 取得Print資料
        /// </summary>
        /// <param name="fromCutting">From P02、P09</param>
        /// <param name="drInfoFrom">原資料帶入</param>
        /// <param name="s1">參數起</param>
        /// <param name="s2">參數迄</param>
        /// <param name="printType">"Cutref"、"Cutplanid"</param>
        /// <param name="sortType">排序，"Cutref"、"SpreadingNoID,CutCellID,Cutref"</param>
        /// <param name="arrDtType">輸出資料表陣列，配合Print的ToExcel時用</param>
        /// <returns>DualResult</returns>
        public DualResult GetPrintData(FromCutting fromCutting, DataRow drInfoFrom, string s1, string s2, string printType, string sortType, out DataTable[] arrDtType)
        {
            string tableName = this.GetTableName(fromCutting);
            string tbPatternPanel = tableName + "_PatternPanel";
            string tbDistribute = tableName + "_Distribute";
            string tbSizeRatio = tableName + "_SizeRatio";
            bool isTbPatternPanel = this.CheckTableExist(tbPatternPanel);
            bool isTbDistribute = this.CheckTableExist(tbDistribute);
            bool isTbSizeRatio = this.CheckTableExist(tbSizeRatio);
            string tbUkey = tableName + "Ukey";
            arrDtType = new DataTable[13];
            List<SqlParameter> paras = new List<SqlParameter>
            {
                new SqlParameter("@Cutref1", s1),
                new SqlParameter("@Cutref2", s2),
            };

            string sqlParas = string.Empty;
            foreach (var item in paras)
            {
                sqlParas += $"DECLARE {item.ParameterName} varchar(50) ='{item.Value}';\r\n";
            }

            string byType, byType2;
            string strOrderby = string.Empty;

            string sqlFabricKind = string.Empty;
            string sqlFabricKindinto = string.Empty;
            string sqlFabricKindjoin = string.Empty;
            if (printType == "Cutref" && isTbDistribute)
            {
                byType = "Cutref";
                byType2 = ",shc";
                strOrderby = "order by " + sortType;
                sqlFabricKind = $@"
SELECT distinct w.CutRef, wp.PatternPanel, x.FabricKind
into #tmp3
FROM #tmp W
INNER JOIN {tbPatternPanel} WP ON W.Ukey = WP.{tbUkey}
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
		where exists(select 1 from {tbDistribute} wd where wd.{tbUkey} = W.Ukey and wd.Article = OCC.Article)
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

            string sqlColByType = this.CheckAndGetColumns(fromCutting, byType);
            string sqlWhereByType = this.CheckTableLostColumns(fromCutting, byType) ? "1=1" : $"{byType}>= @Cutref1 and {byType}<= @Cutref2";

            string workorder_cmd =
                $@"
Select a.AddDate
,a.AddName
,a.ColorID
,a.Cons
,a.ConsPC
,{this.CheckAndGetColumns(fromCutting, "a.CutCellID")}
,{this.CheckAndGetColumns(fromCutting, "a.CutNo")}
,{this.CheckAndGetColumns(fromCutting, "a.CutPlanID")}
, a.CutRef
,a.EditDate
,a.EditName
,a.EstCutDate
,a.FabricCode
,a.FabricCombo
,a.FabricPanelCode
,a.FactoryID
,a.ID
,a.IsCreateByUser
,a.Layer
,a.MarkerLength
,a.MarkerName
,a.MarkerNo
--,a.MarkerVersion
,a.MDivisionID
,a.Order_EachconsUkey
,{this.CheckAndGetColumns(fromCutting, "a.OrderID")}
, a.RefNo
,a.SCIRefNo
,a.Seq1
,a.Seq2
,{this.CheckAndGetColumns(fromCutting, "a.SpreadingNoID")}
, a.Tone
,a.Ukey
,a.WKETA
,b.Description
,b.width
,dbo.MarkerLengthToYDS(a.MarkerLength) as yds 
,shc = iif(isnull(shc.RefNo,'')='','','Shrinkage Issue, Spreading Backward Speed: 2, Loose Tension')
,oe.NoNotch
from {tableName} a WITH (NOLOCK)
Left Join Fabric b WITH (NOLOCK) on a.SciRefno = b.SciRefno
Left Join Order_EachCons oe with (nolock) on oe.Ukey = a.Order_EachconsUkey
outer apply(select RefNo from ShrinkageConcern where RefNo=a.RefNo and Junk=0) shc            
Where {sqlWhereByType}
and a.id='{drInfoFrom["ID"]}'
{this.OrderByWithCheckColumns(fromCutting, strOrderby)}
";

            DualResult dResult = DBProxy.Current.Select(null, workorder_cmd, paras, out arrDtType[(int)TableType.WorkorderTb]);
            if (!dResult)
            {
                return dResult;
            }

            string tbDistributeColumns = string.Empty;
            string tbDistributeJoin = string.Empty;
            string tbDistributeWhere = "1=1";
            string tbDistributeOrderBy = string.Empty;
            if (isTbDistribute)
            {
                tbDistributeColumns = @"
,b.WorkOrderForOutputUkey
,b.ID
,b.OrderID
,b.Article
,b.SizeCode
,b.Qty
,s.SewLineList
";
                tbDistributeJoin = $@"
inner join {tbDistribute} b WITH (NOLOCK) on  a.ukey = b.{tbUkey} 
outer apply(
	select SewLineList = Stuff((
		select concat('\',SewLine)
		from (
				select SewLine
				from dbo.orders d
				where exists(
					select 1 from {tbDistribute} where {tbUkey} = a.ukey
					and orderid = d.id
				)
			) s
		for xml path ('')
	) , 1, 1, '')
) s";
                tbDistributeWhere = $"a.ukey = b.{tbUkey}";
                tbDistributeOrderBy = "order by b.OrderID,b.Article,b.SizeCode";
            }

            workorder_cmd = $@"
Select {sqlColByType},{this.CheckAndGetColumns(fromCutting, "a.Cutno")},a.Colorid,a.Layer,a.Cons
{tbDistributeColumns}
from {tableName} a WITH (NOLOCK) 
{tbDistributeJoin}
Where {sqlWhereByType} and a.id='{drInfoFrom["ID"]}' and {tbDistributeWhere}
{tbDistributeOrderBy}";
            //if (isTbDistribute)
            {
                dResult = DBProxy.Current.Select(null, workorder_cmd, paras, out arrDtType[(int)TableType.WorkorderDisTb]);
                if (!dResult)
                {
                    return dResult;
                }
            }

            workorder_cmd = $"Select {sqlColByType},a.MarkerName,a.MarkerNo,a.MarkerLength,a.Cons,a.Layer,{this.CheckAndGetColumns(fromCutting, "a.Cutno")}, a.colorid,c.seq,a.FabricPanelCode,b.* from {tableName} a WITH (NOLOCK) ,{tbSizeRatio} b WITH (NOLOCK) ,Order_SizeCode c WITH (NOLOCK) Where {sqlWhereByType} and a.id='{drInfoFrom["ID"]}' and a.ukey = b.{tbUkey} and a.id = c.id and b.id = c.id and b.sizecode = c.sizecode order by c.seq";
            dResult = DBProxy.Current.Select(null, workorder_cmd, paras, out arrDtType[(int)TableType.WorkorderSizeTb]);
            if (!dResult)
            {
                return dResult;
            }

            workorder_cmd = $"Select {sqlColByType},b.*,a.Markername,a.FabricPanelCode from {tableName} a,{tbPatternPanel} b Where {sqlWhereByType} and a.id='{drInfoFrom["ID"]}' and a.ukey = b.{tbUkey}";
            dResult = DBProxy.Current.Select(null, workorder_cmd, paras, out arrDtType[(int)TableType.WorkorderPatternTb]);
            if (!dResult)
            {
                return dResult;
            }

            //if (isTbDistribute)
            {
                string sqlCutrefTb = $@"
    Select {sqlColByType},estCutDate{byType2},rn=ROW_NUMBER()over({this.OrderByWithCheckColumns(fromCutting, strOrderby)}) into #tmp2 From #tmp

    {sqlFabricKind}

    select {sqlColByType},estCutDate{byType2} {sqlFabricKindinto} from #tmp2 group by {sqlColByType},estCutDate{byType2} order by min(rn)

    {sqlFabricKindjoin}
    ";
                MyUtility.Tool.ProcessWithDatatable(arrDtType[(int)TableType.WorkorderTb], "SpreadingNoID,CutCellID,Cutref,CutPlanID,estCutDate,shc,ukey,id", sqlCutrefTb, out arrDtType[(int)TableType.CutrefTb]);
            }

            if (isTbDistribute)
            {
                // 因要使用的欄位都是在Distribute因此直接不輸出該表<TableType.CutDisOrderIDTb>，引用前皆需判斷isTbDistribute
                MyUtility.Tool.ProcessWithDatatable(arrDtType[(int)TableType.WorkorderDisTb], $"{sqlColByType},OrderID,SewLineList", $@"Select distinct {sqlColByType},OrderID,SewLineList From #tmp", out arrDtType[(int)TableType.CutDisOrderIDTb]); // 整理sp，此處的OrderID是來自Orders
            }

            MyUtility.Tool.ProcessWithDatatable(arrDtType[(int)TableType.WorkorderSizeTb], $"{sqlColByType},MarkerName,MarkerNo,MarkerLength,SizeCode,Cons,Qty,seq,FabricPanelCode", $"Select distinct {sqlColByType},MarkerName,MarkerNo,MarkerLength,SizeCode,Cons,Qty,seq,FabricPanelCode,dbo.MarkerLengthToYDS(MarkerLength) as yds From #tmp order by FabricPanelCode,MarkerName,seq", out arrDtType[(int)TableType.CutSizeTb]); // 整理SizeGroup,Qty

            MyUtility.Tool.ProcessWithDatatable(arrDtType[(int)TableType.WorkorderSizeTb], $"{sqlColByType},SizeCode,seq", $"Select distinct {sqlColByType},SizeCode,seq From #tmp order by seq ", out arrDtType[(int)TableType.SizeTb]); // 整理Size

            MyUtility.Tool.ProcessWithDatatable(arrDtType[(int)TableType.WorkorderSizeTb], $"{sqlColByType},MarkerName", $"Select distinct {sqlColByType},MarkerName From #tmp ", out arrDtType[(int)TableType.MarkerTB]); // 整理MarkerName

            MyUtility.Tool.ProcessWithDatatable(arrDtType[(int)TableType.WorkorderTb], $"{sqlColByType},FabricPanelCode,SCIRefno,shc", $"Select distinct {sqlColByType},a.FabricPanelCode,a.SCIRefno,b.Description,b.width,shc  From #tmp a Left Join Fabric b on a.SciRefno = b.SciRefno", out arrDtType[(int)TableType.FabricComboTb]); // 整理FabricPanelCode

            if (fromCutting == FromCutting.P02)
            {
                // 已限定是Plan，就不特別處理
                string issue_cmd = $"Select a.Cutplanid,b.Qty,b.Dyelot,b.Roll,Max(c.yds) as yds,c.Colorid from Issue a WITH (NOLOCK) ,Issue_Detail b WITH (NOLOCK) , #tmp c Where a.id=b.id and c.Cutplanid = a.Cutplanid and c.SEQ1 = b.SEQ1 and c.SEQ2 = b.SEQ2 group by a.Cutplanid,b.Qty,b.Dyelot,b.Roll,c.Colorid order by Dyelot,roll";
                MyUtility.Tool.ProcessWithDatatable(arrDtType[(int)TableType.WorkorderTb], "Cutplanid,SEQ1,SEQ2,yds,Colorid", issue_cmd, out arrDtType[(int)TableType.IssueTb]); // 整理FabricPanelCode
            }

            return Ict.Result.True;
        }

        /// <summary>
        /// 取得Excel資料
        /// </summary>
        /// <param name="fromCutting">From P02、P09</param>
        /// <param name="dataSource">detailgridbs.DataSource</param>
        /// <returns>bool</returns>
        public DualResult GetExcelData(FromCutting fromCutting, object dataSource)
        {
            string tableName = this.GetTableName(fromCutting);
            string tbDistribute = tableName + "_Distribute";
            string tbUkey = tableName + "Ukey";
            bool isTbDistribute = this.CheckTableExist(tbDistribute);
            DualResult dualResult;
            try
            {
                // keepApp=true 產生excel後才可修改編輯
                sxrc sxr = new sxrc();
                Excel.Application app = sxr.ExcelApp;
                Excel.Worksheet wks = app.ActiveWorkbook.Sheets[1];
                app.DisplayAlerts = false;
                wks.Cells[1, 1] = "##tbl";
                DataTable dt = (DataTable)dataSource;
                DataTable dt2 = new DataTable();

                // 欄位，有順序性
                List<string> listColumns = new List<string>();
                listColumns.Add("[CutRef#] = Cutref");
                listColumns.Add("[Cut#] = Cutno");
                listColumns.Add("[Marker Name] = MarkerName");
                listColumns.Add("[Fabric Combo]=Fabriccombo");
                listColumns.Add("[Fab_Panel Code]=FabricPanelCode");
                listColumns.Add("[Article]=Article");
                listColumns.Add("[Color]=Colorid");
                listColumns.Add("[Size]=SizeCode");
                listColumns.Add("[Layers]=Layer");
                listColumns.Add("[Total CutQty]=CutQty");
                listColumns.Add("[SP#]=getsp.value");
                listColumns.Add("[SEQ1]=SEQ1");
                listColumns.Add("[SEQ2]=SEQ2");
                listColumns.Add("[Fabric Arr Date]=Fabeta");
                listColumns.Add("[WK ETA]=WKETA");
                listColumns.Add("[Est. Cut Date]=estcutdate");
                listColumns.Add("[Sewing inline]=sewinline");
                listColumns.Add("[Spreading No]=SpreadingNoID");
                listColumns.Add("[Cut Cell]=Cutcellid");
                listColumns.Add("[Shift]=Shift");
                listColumns.Add("[Cutplan#]=Cutplanid");
                listColumns.Add("[Act. Cut Date]=actcutdate");
                listColumns.Add("[Edit Name]=Edituser");
                listColumns.Add("[Edit Date]=EditDate");
                listColumns.Add("[Add Name]=Adduser");
                listColumns.Add("[Add Date]=AddDate");
                listColumns.Add("[Key]=UKey");
                listColumns.Add("[Apply #]=MarkerNo");
                listColumns.Add("[Apply ver]=MarkerVersion");
                listColumns.Add("[Download ID]=MarkerDownloadID");
                listColumns.Add("[EachCons Apply #]=EachconsMarkerNo");
                listColumns.Add("[EachCons Apply ver]=EachconsMarkerVersion");
                listColumns.Add("[EachCons Download ID]=EachconsMarkerDownloadID");
                listColumns.Add("[ActCutting Perimeter]=ActCuttingPerimeterNew");
                listColumns.Add("[StraightLength]=StraightLengthNew");
                listColumns.Add("[CurvedLength]=CurvedLengthNew");

                // For 在Excel更改格式用
                Dictionary<string, string> dicSpecColumnAndFormula = new Dictionary<string, string>();
                dicSpecColumnAndFormula.Add("[Fabric Arr Date]=Fabeta", "yyyy/MM/dd");
                dicSpecColumnAndFormula.Add("[WK ETA]=WKETA", "yyyy/MM/dd");
                dicSpecColumnAndFormula.Add("[Est. Cut Date]=estcutdate", "yyyy/MM/dd");
                dicSpecColumnAndFormula.Add("[Sewing inline]=sewinline", "yyyy/MM/dd");
                dicSpecColumnAndFormula.Add("[Act. Cut Date]=actcutdate", "yyyy/MM/dd");
                dicSpecColumnAndFormula.Add("[Edit Date]=EditDate", "yyyy/MM/dd hh:mm:ss");
                dicSpecColumnAndFormula.Add("[Add Date]=AddDate", "yyyy/MM/dd hh:mm:ss");

                // 對應P02或P09要移除的欄位
                List<string> listColumnsRemoved = new List<string>();
                if (fromCutting == FromCutting.P02)
                {
                    listColumnsRemoved.Add("[Cut#] = Cutno");
                    listColumnsRemoved.Add("[Spreading No]= SpreadingNoID");
                    listColumnsRemoved.Add("[Cut Cell] = Cutcellid");
                    listColumnsRemoved.Add("[Shift] = Shift");
                    listColumnsRemoved.Add("[Apply ver] = MarkerVersion");
                    listColumnsRemoved.Add("[ActCutting Perimeter] = ActCuttingPerimeterNew");
                    listColumnsRemoved.Add("[StraightLength] = StraightLengthNew");
                    listColumnsRemoved.Add("[CurvedLength] = CurvedLengthNew");
                    listColumnsRemoved.Add("[SP#]=getsp.value");

                    listColumnsRemoved.Add("[Size]=SizeCode");
                    listColumnsRemoved.Add("[Total CutQty]=CutQty");
                    listColumnsRemoved.Add("[Apply ver]=MarkerVersion");
                    listColumnsRemoved.Add("[Download ID]=MarkerDownloadID");
                    listColumnsRemoved.Add("[EachCons Download ID]=EachconsMarkerDownloadID");
                    listColumnsRemoved.Add("[ActCutting Perimeter]=ActCuttingPerimeterNew");
                    listColumnsRemoved.Add("[StraightLength]=StraightLengthNew");
                    listColumnsRemoved.Add("[CurvedLength]=CurvedLengthNew");

                    listColumnsRemoved.Add("[Sewing inline]=sewinline");
                    listColumnsRemoved.Add("[Act. Cut Date]=actcutdate");
                }
                else
                {
                    listColumnsRemoved.Add("[Cutplan#]=Cutplanid");

                    listColumnsRemoved.Add("[Size]=SizeCode");
                    listColumnsRemoved.Add("[Total CutQty]=CutQty");
                    listColumnsRemoved.Add("[Apply ver]=MarkerVersion");
                    listColumnsRemoved.Add("[Download ID]=MarkerDownloadID");
                    listColumnsRemoved.Add("[EachCons Download ID]=EachconsMarkerDownloadID");
                    listColumnsRemoved.Add("[ActCutting Perimeter]=ActCuttingPerimeterNew");
                    listColumnsRemoved.Add("[StraightLength]=StraightLengthNew");
                    listColumnsRemoved.Add("[CurvedLength]=CurvedLengthNew");

                    listColumnsRemoved.Add("[Article]=Article");
                    listColumnsRemoved.Add("[EachCons Apply #]=EachconsMarkerNo");
                    listColumnsRemoved.Add("[EachCons Apply ver]=EachconsMarkerVersion");
                }

                listColumns = listColumns.Except(listColumnsRemoved).ToList();

                string joinDistribute = isTbDistribute
                    ? $@"Outer Apply(
 SELECT value = STUFF(
	(Select DISTINCT ', ' + Rtrim(wd.OrderID)
		From {tbDistribute} wd WITH (NOLOCK)
		inner join Orders o  with (nolock) on o.ID = wd.OrderID
		Where wd.{tbUkey} = tmp.Ukey and wd.Article <>'' 
		For XML PATH ('')
	), 1, 1, '')
)getsp
"
                    : string.Empty;
                string strOrderBy = fromCutting == FromCutting.P02
                    ? "Order by Article,MarkerName,Ukey"
                    : "Order by MarkerName,Ukey";

                string sqlCmd = $@"
Select 
{listColumns.JoinToString("\n,")}
FROM #tmp tmp
{joinDistribute}
{strOrderBy}
";
                var result = MyUtility.Tool.ProcessWithDatatable(dt, null, sqlCmd, out dt2);
                if (!result)
                {
                    dualResult = result;
                    return dualResult;
                }

                sxrc.XltRptTable xrt = new sxrc.XltRptTable(dt2);
                xrt.ShowHeader = true;
                xrt.BoAutoFitColumn = true;
                xrt.BoFreezePanes = true;
                xrt.BoAddFilter = true;
                sxr.DicDatas.Add("##tbl", xrt);
                Excel.Range range;
                for (int i = 0; i < listColumns.Count; i++)
                {
                    string key = listColumns[i];
                    if (dicSpecColumnAndFormula.ContainsKey(key))
                    {
                        range = wks.Range[wks.Cells[1, i + 1], wks.Cells[1 + dt2.Rows.Count, i + 1]];
                        range.NumberFormatLocal = dicSpecColumnAndFormula[key];
                    }
                }

                sxr.Save();
                dualResult = new DualResult(true);
            }
            catch (Exception ex)
            {
                dualResult = new DualResult(false, ex.Message);
            }

            return dualResult;
        }

        /// <summary>
        /// 取得各Table沒有的欄位清單
        /// </summary>
        private void GetDictionaryTableLostColumns()
        {
            string sql = $@"
--取出WorkOrderForPlanning的欄位
SELECT tbName=TABLE_NAME,colName = COLUMN_NAME
into #forPlan
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = N'WorkOrderForPlanning'
--取出WorkOrderForOutput的欄位
SELECT tbName=TABLE_NAME,colName = COLUMN_NAME
into #forOutput
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = N'WorkOrderForOutput'

--撈出各Table沒有的欄位
select tbName='WorkOrderForOutput',LostColName=A.colName
from #forPlan A
left join #forOutput B
on A.colName=B.colName
where B.colName is null
union
select tbName='WorkOrderForPlanning',LostColName=B.colName
from #forOutput B
left join #forPlan A
on B.colName=A.colName
where A.colName is null

drop table #forPlan,#forOutput
";
            DataTable dt;
            DBProxy.Current.Select(null, sql, out dt);
            var dic = dt.AsEnumerable()
                .GroupBy(r => r["tbName"].ToString())
                .ToDictionary(g => g.Key, g => g.Select(r => r["LostColName"].ToString()).ToList());
            var comparer = StringComparer.OrdinalIgnoreCase;
            this._dicTableLostColumns = new Dictionary<string, List<string>>(dic, comparer);
            #region 紀錄撈出的結果，方便查看，若資料結構有異動，可以同步補上
            /*
               tbName                  LostColName                  有無用到(在該cs檔Ctrl+F搜尋LostColName的值)
               WorkOrderForOutput      CutPlanID                    V
               WorkOrderForOutput      Remark                       X
               WorkOrderForOutput      Type                         X
               WorkOrderForPlanning    ActCuttingPerimeter          X
               WorkOrderForPlanning    CurvedLength                 X
               WorkOrderForPlanning    CutCellID                    V
               WorkOrderForPlanning    CutNo                        V
               WorkOrderForPlanning    CuttingMethod                X
               WorkOrderForPlanning    GroupID                      X
               WorkOrderForPlanning    MarkerVersion                X
               WorkOrderForPlanning    OrderID                      V
               WorkOrderForPlanning    Shift                        X
               WorkOrderForPlanning    SourceFrom                   X
               WorkOrderForPlanning    SpreadingNoID                V
               WorkOrderForPlanning    SpreadingRemark              X
               WorkOrderForPlanning    SpreadingStatus              X
               WorkOrderForPlanning    StraightLength               X
               WorkOrderForPlanning    UnfinishedCuttingReason      X
               WorkOrderForPlanning    UnfinishedCuttingRemark      X
               WorkOrderForPlanning    WorkOrderForPlanningUkey     X
            => CutPlanID, CutCellID, CutNo, OrderID, SpreadingNoID
            */
            #endregion
        }

        /// <summary>
        /// 檢查目標Table是否無該欄位
        /// </summary>
        /// <param name="fromCutting">From P02、P09</param>
        /// <param name="column">檢查的Column</param>
        /// <returns>true代表無該欄位</returns>
        public bool CheckTableLostColumns(FromCutting fromCutting, string column)
        {
            string tableNameWorkOrder = this.GetTableName(fromCutting);
            return this.DicTableLostColumns.ContainsKey(tableNameWorkOrder)
                && this.DicTableLostColumns[tableNameWorkOrder].FindIndex(x => x.Equals(column, StringComparison.OrdinalIgnoreCase)) != -1;
        }

        /// <summary>
        /// 檢查若不存在的欄位則補上設定空字串
        /// </summary>
        /// <param name="fromCutting">From P02、P09</param>
        /// <param name="column">輸入欄位</param>
        /// <returns>回傳欄位</returns>
        private string CheckAndGetColumns(FromCutting fromCutting, string column)
        {
            string checkColumn = column.Contains(".") ? column.Split('.')[1] : column;
            return this.CheckTableLostColumns(fromCutting, checkColumn) ? checkColumn + "=''" : column;
        }

        /// <summary>
        /// 檢查Order By的欄位是否存在，並重組出Order By字串
        /// </summary>
        /// <param name="fromCutting">From P02、P09</param>
        /// <param name="orderBy">整串Order By的字串(輸入檢查)</param>
        /// <returns>整串Order By的字串(輸出回傳)</returns>
        private string OrderByWithCheckColumns(FromCutting fromCutting, string orderBy)
        {
            orderBy = Regex.Replace(orderBy, "order by", string.Empty, RegexOptions.IgnoreCase);
            List<string> listOrderByColumns = orderBy.Split(',').ToList();
            List<string> listResult = new List<string>();
            foreach (var item in listOrderByColumns)
            {
                var column = Regex.Replace(item, "asc", string.Empty, RegexOptions.IgnoreCase);
                column = Regex.Replace(column, "desc", string.Empty, RegexOptions.IgnoreCase);
                column = column.Trim();
                if (!this.CheckTableLostColumns(fromCutting, column))
                {
                    listResult.Add(item);
                }
            }

            return "order by " + string.Join(",", listResult);
        }

        /// <summary>
        /// 取得Table名稱
        /// </summary>
        /// <param name="fromCutting">From P02、P09</param>
        /// <returns>string</returns>
        private string GetTableName(FromCutting fromCutting)
        {
            return fromCutting == FromCutting.P02 ? "WorkOrderForPlanning" : "WorkOrderForOutput";
        }

        private bool CheckTableExist(string tableName)
        {
            string sql = $@"SELECT distinct 1
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = N'{tableName}'";
            return mySqlClient.FindRow(null, sql) != null;
        }

        /// <summary>
        /// 將Print資料輸出Excel報表
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="arrDtType">資料表陣列，可由GetPrintData產出</param>
        /// <param name="fromCutting">From P02、P09</param>
        /// <param name="printType">列印哪種報表</param>
        /// <param name="errMsg">錯誤訊息</param>
        /// <returns>bool</returns>
        public bool PrintToExcel(string id, DataTable[] arrDtType, FromCutting fromCutting, string printType, out string errMsg)
        {
            bool result;
            if (printType == "Cutref")
            {
                result = this.ByCutrefExcel(id, arrDtType, fromCutting, out errMsg);
            }
            else
            {
                result = this.ByRequestExcel(id, arrDtType, out errMsg);
            }

            return result;
        }

        /// <summary>
        /// By Request Excel (Only For Cutting P02，因此CutPlanID必定有值 => 可參考GetDictionaryTableLostColumns)
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="arrDtType">arrDtType</param>
        /// <param name="errMsg">errMsg</param>
        /// <returns>bool</returns>
        private bool ByRequestExcel(string id, DataTable[] arrDtType, out string errMsg)
        {
            errMsg = string.Empty;
            try
            {
                string strXltName = Env.Cfg.XltPathDir + "\\Cutting_P02_SpreadingReportbyRequest.xltx";
                DataRow orderDr;
                MyUtility.Check.Seek(string.Format("Select * from Orders WITH (NOLOCK) Where id='{0}'", id), out orderDr);
                Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
                if (excel == null)
                {
                    return false;
                }

                excel.Visible = false;
                Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

                #region 寫入共用欄位
                worksheet.Cells[1, 6] = orderDr["factoryid"];
                worksheet.Cells[3, 2] = DateTime.Now.ToShortDateString();
                worksheet.Cells[3, 7] = string.Empty;
                worksheet.Cells[3, 12] = string.Empty;
                worksheet.Cells[9, 2] = orderDr["Styleid"];
                worksheet.Cells[10, 2] = orderDr["Seasonid"];
                //worksheet.Cells[10, 13] = arrDtType[(int)TableType.CutDisOrderIDTb].Rows[0]["SewLineList"].ToString();
                //orderDr["Sewline"];
                for (int nColumn = 3; nColumn <= 21; nColumn += 3)
                {
                    worksheet.Cells[36, nColumn] = orderDr["Styleid"];
                    worksheet.Cells[37, nColumn] = id;
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
                foreach (DataRow cutrefdr1 in arrDtType[(int)TableType.CutrefTb].Rows)
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
                foreach (DataRow cutrefdr in arrDtType[(int)TableType.CutrefTb].Rows)
                {
                    spList = string.Empty;
                    worksheet = excel.ActiveWorkbook.Worksheets[nSheet];
                    worksheet.Select();
                    string cutplanid = cutrefdr["Cutplanid"].ToString();
                    #region 撈表身Detail Array
                    workorderArry = arrDtType[(int)TableType.WorkorderTb].Select(string.Format("Cutplanid = '{0}'", cutplanid));
                    workorderDisArry = arrDtType[(int)TableType.WorkorderDisTb].Select(string.Format("Cutplanid='{0}'", cutplanid));
                    workorderSizeArry = arrDtType[(int)TableType.WorkorderSizeTb].Select(string.Format("Cutplanid='{0}'", cutplanid));
                    //workorderOrderIDArry = arrDtType[(int)TableType.CutDisOrderIDTb].Select(string.Format("Cutplanid='{0}'", cutplanid), "Orderid");
                    fabricComboArry = arrDtType[(int)TableType.FabricComboTb].Select(string.Format("Cutplanid='{0}'", cutplanid));
                    sizeCodeArry = arrDtType[(int)TableType.SizeTb].Select(string.Format("Cutplanid='{0}'", cutplanid), "SEQ");
                    markerArry = arrDtType[(int)TableType.MarkerTB].Select(string.Format("Cutplanid = '{0}'", cutplanid));
                    issueArry = arrDtType[(int)TableType.IssueTb].Select(string.Format("Cutplanid = '{0}'", cutplanid));
                    #endregion

                    if (workorderArry.Length > 0)
                    {
                        //worksheet.Cells[8, 13] = workorderArry[0]["MarkerDownLoadId"].ToString();
                        worksheet.Cells[3, 7] = string.Empty;
                        worksheet.Cells[3, 12] = string.Empty;
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

                            workorderPatternArry = arrDtType[(int)TableType.WorkorderPatternTb].Select(string.Format("Cutplanid='{0}' and FabricPanelCode = '{1}'", cutplanid, fabricComboDr["FabricPanelCode"]), "PatternPanel");
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

                    //#region OrderSP List, Line List
                    //if (workorderOrderIDArry.Length > 0)
                    //{
                    //    foreach (DataRow disDr in workorderOrderIDArry)
                    //    {
                    //        if (disDr["OrderID"].ToString() != "EXCESS")
                    //        {
                    //            if (!disDr["OrderID"].ToString().InList(spList, "\\"))
                    //            {
                    //                spList = spList + disDr["OrderID"].ToString() + "\\";
                    //            }
                    //        }
                    //        #region SewingLine
                    //        line = line + MyUtility.GetValue.Lookup("Sewline", disDr["OrderID"].ToString(), "Orders", "ID") + "\\";
                    //        #endregion
                    //    }

                    //    worksheet.Cells[8, 2] = spList;
                    //    //worksheet.Cells[10, 13] = arrDtType[(int)TableType.CutDisOrderIDTb].Rows[0]["SewLineList"].ToString();
                    //    int l = 54;
                    //    int la = spList.Length / l;
                    //    for (int i = 1; i <= la; i++)
                    //    {
                    //        if (spList.Length > l * i)
                    //        {
                    //            Excel.Range rangeRow8 = (Excel.Range)worksheet.Rows[8, Type.Missing];
                    //            rangeRow8.RowHeight = 20.25 * (i + 1);
                    //        }
                    //    }
                    //}
                    //#endregion

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

                            sizeArry = arrDtType[(int)TableType.CutSizeTb].Select(string.Format("Cutplanid='{0}' and MarkerName = '{1}'", cutplanid, markerDr["MarkerName"]));
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
                    DataRow[] fabricComboTbsia = arrDtType[(int)TableType.FabricComboTb].Select(string.Format("Cutplanid = '{0}'", cutplanid));
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
                    if (arrDtType[(int)TableType.CutQtyTb] != null)
                    {
                        arrDtType[(int)TableType.CutQtyTb].Clear();
                    }

                    MyUtility.Tool.ProcessWithDatatable(arrDtType[(int)TableType.WorkorderSizeTb], "FabricPanelCode,MarkerName,Cutno,Colorid,SizeCode,Qty,Layer,Cutplanid,Cons", pivot_cmd, out arrDtType[(int)TableType.CutQtyTb]);
                    nRow = nRow + 1;
                    bool lfirstComb = true;
                    string fabColor = string.Empty;
                    DataRow[] fabricComboTbsi = arrDtType[(int)TableType.FabricComboTb].Select(string.Format("Cutplanid = '{0}'", cutplanid));
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
                        DataRow[] cutQtyArray = arrDtType[(int)TableType.CutQtyTb].Select(string.Format("FabricPanelCode = '{0}'", fabricComboDr1["FabricPanelCode"]));
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
            catch (Exception ex)
            {
                errMsg = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// By Cutref Excel (Cutting P02、P09皆可用)
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="arrDtType">arrDtType</param>
        /// <param name="fromCutting">fromCutting</param>
        /// <param name="errMsg">errMsg</param>
        /// <returns>bool</returns>
        private bool ByCutrefExcel(string id, DataTable[] arrDtType, FromCutting fromCutting, out string errMsg)
        {
            errMsg = string.Empty;
            string tableName = this.GetTableName(fromCutting);
            string tbDistribute = tableName + "_Distribute";
            string tbUkey = tableName + "Ukey";
            bool isTbDistribute = this.CheckTableExist(tbDistribute);
            try
            {
                bool isP02 = fromCutting == CuttingWorkOrder.FromCutting.P02;
                int nSizeColumn;
                //if (isTbDistribute)
                {
                    int sheetCount = arrDtType[(int)TableType.CutrefTb].Rows.Count;
                    if (sheetCount == 0)
                    {
                        errMsg = "Data not found!";
                        return false;
                    }
                }

                DataRow orderDr;
                MyUtility.Check.Seek(string.Format("Select * from Orders WITH (NOLOCK) Where id='{0}'", id), out orderDr);

                #region By Cutref
                string strXltName = Env.Cfg.XltPathDir + "\\Cutting_P02_SpreadingReportbyCutref.xltx";
                Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
                if (excel == null)
                {
                    return false;
                }

                Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

                #region 寫入共用欄位
                worksheet.Cells[1, 6] = orderDr["factoryid"];
                worksheet.Cells[3, 2] = DateTime.Now.ToShortDateString();

                // worksheet.Cells[3, 7] = detDr["SpreadingNoID"];
                // worksheet.Cells[3, 12] = detDr["CutCellid"];
                worksheet.Cells[9, 2] = orderDr["Styleid"];
                worksheet.Cells[10, 2] = orderDr["Seasonid"];
                if (isTbDistribute)
                {
                    worksheet.Cells[10, 13] = arrDtType[(int)TableType.CutDisOrderIDTb].Rows[0]["SewLineList"].ToString();
                }

                for (int nColumn = 3; nColumn <= 21; nColumn += 3)
                {
                    worksheet.Cells[40, nColumn] = orderDr["Styleid"];
                    worksheet.Cells[41, nColumn] = id;
                }

                #endregion

                int nSheet = 1;
                string spList = string.Empty;
                DataRow[] workorderArry;
                DataRow[] workorderDisArry = null;
                DataRow[] workorderSizeArry;
                DataRow[] workorderPatternArry;
                DataRow[] workorderOrderIDArry = null;
                DataRow[] sizeArry;
                DataRow[] sizeCodeArry;
                string pattern = string.Empty, line = string.Empty;
                int nDisCount = 0;
                double disRow = 0;
                string size = string.Empty, ratio = string.Empty;
                int totConsRowS = 18, totConsRowE = 19;
                //if (isTbDistribute)
                {
                    foreach (DataRow cutrefdr in arrDtType[(int)TableType.CutrefTb].Rows)
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
                        worksheet.Cells[9, 13] = MyUtility.Check.Empty(cutrefdr["Estcutdate"]) == false ? ((DateTime)MyUtility.Convert.GetDate(cutrefdr["Estcutdate"])).ToShortDateString() : String.Empty;
                        worksheet.Cells[14, 14] = MyUtility.Convert.GetString(cutrefdr["FabricKind"]);
                        nSheet++;
                    }

                    nSheet = 1;
                    foreach (DataRow cutrefdr1 in arrDtType[(int)TableType.CutrefTb].Rows)
                    {
                        Bitmap cutRefQRCode = this.NewQRcode(MyUtility.Convert.GetString(cutrefdr1["Cutref"].ToString()));
                        worksheet = excel.ActiveWorkbook.Worksheets[nSheet];
                        Excel.Range rng = worksheet.Range["T2:U3"];

                        // 將圖片存儲為暫時檔案
                        string tempFilePath = System.IO.Path.GetTempFileName();
                        cutRefQRCode.Save(tempFilePath);

                        // 將圖片插入到工作表中的指定位置
                        Excel.Shape pictureShape = worksheet.Shapes.AddPicture(
                            tempFilePath,
                            Microsoft.Office.Core.MsoTriState.msoFalse,
                            Microsoft.Office.Core.MsoTriState.msoCTrue,
                            (float)rng.Left,
                            (float)rng.Top,
                            (float)rng.Height,
                            (float)rng.Height);

                        // 刪除暫時檔案
                        System.IO.File.Delete(tempFilePath);

                        nSheet++;
                    }

                    nSheet = 1;
                    foreach (DataRow cutrefdr2 in arrDtType[(int)TableType.CutrefTb].Rows)
                    {
                        spList = string.Empty;
                        worksheet = excel.ActiveWorkbook.Worksheets[nSheet];
                        worksheet.Select();
                        string cutref = cutrefdr2["Cutref"].ToString();
                        #region 撈表身Detail Array
                        workorderArry = arrDtType[(int)TableType.WorkorderTb].Select(string.Format("Cutref = '{0}'", cutref));
                        //if (isTbDistribute)
                        {
                            workorderDisArry = arrDtType[(int)TableType.WorkorderDisTb].Select(string.Format("Cutref='{0}'", cutref));
                        }

                        workorderSizeArry = arrDtType[(int)TableType.WorkorderSizeTb].Select(string.Format("Cutref='{0}'", cutref));
                        workorderPatternArry = arrDtType[(int)TableType.WorkorderPatternTb].Select(string.Format("Cutref='{0}'", cutref), "PatternPanel");
                        if (isTbDistribute)
                        {
                            workorderOrderIDArry = arrDtType[(int)TableType.CutDisOrderIDTb].Select(string.Format("Cutref='{0}'", cutref), "Orderid");
                        }

                        sizeArry = arrDtType[(int)TableType.CutSizeTb].DefaultView.ToTable(true, new string[] { "Cutref", "SizeCode" }).Select(string.Format("Cutref='{0}'", cutref));
                        sizeCodeArry = arrDtType[(int)TableType.SizeTb].Select(string.Format("Cutref='{0}'", cutref), "SEQ");
                        #endregion

                        if (workorderArry.Length > 0)
                        {
                            pattern = string.Empty;
                            worksheet.Cells[8, 13] = "Ryan超胖";
                            worksheet.Cells[13, 2] = workorderArry[0]["FabricPanelCode"].ToString();
                            worksheet.Cells[3, 7] = workorderArry[0]["SpreadingNoID"].ToString();
                            worksheet.Cells[3, 12] = workorderArry[0]["CutCellid"].ToString();
                            worksheet.Cells[22, 2] = workorderArry[0]["Tone"].ToString();
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
                        if (workorderOrderIDArry != null && workorderOrderIDArry.Length > 0)
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
                                if (spList.Length <= l * i)
                                {
                                    continue;
                                }

                                double rowHeight = 20.25 * (i + 1);
                                Excel.Range rangeRow8 = (Excel.Range)worksheet.Rows[8, Type.Missing];

                                // row高只能到409
                                if (rowHeight > 409)
                                {
                                    rangeRow8.RowHeight = 409;
                                    break;
                                }

                                rangeRow8.RowHeight = 20.25 * (i + 1);
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
                            DataRow[] wtmp = arrDtType[(int)TableType.WorkorderSizeTb].DefaultView.ToTable(false, new string[] { "Cutref", "SizeCode" }).Select(string.Format("Cutref='{0}'", cutref));
                            DataRow[] wtmp2 = arrDtType[(int)TableType.WorkorderSizeTb].DefaultView.ToTable(false, new string[] { "Cutref", "Qty" }).Select(string.Format("Cutref='{0}'", cutref));
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
                            string markerNo = MyUtility.Convert.GetString(workorderArry[0]["MarkerNo"]);
                            string markerNo2 = string.Empty;
                            if (markerNo.Length >= 2)
                            {
                                markerNo2 = markerNo.Substring(markerNo.Length - 2);
                            }

                            worksheet.Cells[12, 1] = workorderArry[0]["MarkerName"].ToString();
                            worksheet.Cells[12, 3] = markerNo2;
                            worksheet.Cells[12, 4] = markerNo;
                            worksheet.Cells[12, 6] = workorderArry[0]["MarkerLength"].ToString() + "\n" + workorderArry[0]["yds"].ToString() + "Y (" + unit + "M)";
                            worksheet.Cells[12, 10] = size;
                            worksheet.Cells[12, 12] = ratio;
                            if (!MyUtility.Convert.GetBool(workorderArry[0]["NoNotch"]))
                            {
                                worksheet.Cells[12, 16] = string.Empty;
                            }

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
                        if (isTbDistribute && workorderDisArry.Length > 0)
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

                        pivot_cmd =
                            $@"
Select Cutno,Colorid,SizeCode,Cons,Layer,{tbUkey},(Qty*Layer) as TotalQty from 
#tmp
Where Cutref = '{cutref}'";

                        if (arrDtType[(int)TableType.CutQtyTb] != null)
                        {
                            arrDtType[(int)TableType.CutQtyTb].Clear();
                        }

                        drwst = MyUtility.Tool.ProcessWithDatatable(arrDtType[(int)TableType.WorkorderSizeTb], $"Cutno,Colorid,SizeCode,Qty,Layer,Cutref,Cons,{tbUkey}", pivot_cmd, out arrDtType[(int)TableType.CutQtyTb]);
                        if (!drwst)
                        {
                            MyUtility.Msg.ErrorBox("SQL command Pivot_cmd error!");
                            return false;
                        }

                        nrow = nrow + 2;
                        int copyrow = 0;
                        totConsRowS = nrow; // 第一個Cons

                        var distinct_CutQtyTb = from r1 in arrDtType[(int)TableType.CutQtyTb].AsEnumerable()
                                                group r1 by new
                                                {
                                                    Cutno = r1.Field<int>("Cutno"),
                                                    Colorid = r1.Field<string>("Colorid"),
                                                    Layer = r1.Field<int>("Layer"),
                                                    workorderukey_fromCutting = r1.Field<int>(tbUkey),
                                                    Cons = r1.Field<decimal>("Cons"),
                                                }
                                                into g
                                                select new
                                                {
                                                    g.Key.Cutno,
                                                    g.Key.Colorid,
                                                    g.Key.Layer,
                                                    g.Key.workorderukey_fromCutting,
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

                            foreach (DataRow dr in arrDtType[(int)TableType.CutQtyTb].Select($"{tbUkey} = '{dis_dr.workorderukey_fromCutting}'"))
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
                    foreach (DataRow cutrefdr3 in arrDtType[(int)TableType.CutrefTb].Rows)
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
            catch (Exception ex)
            {
                errMsg = ex.Message;
                errMsg += "\n" + ex.StackTrace.ToString();
                return false;
            }
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

        /// <summary>
        /// 0=WorkorderTb,
        /// 1=WorkorderSizeTb,
        /// 2=WorkorderDisTb,
        /// 3=WorkorderPatternTb,
        /// 4=CutrefTb,
        /// 5=CutDisOrderIDTb,
        /// 6=CutSizeTb,
        /// 7=SizeTb,
        /// 8=CutQtyTb,
        /// 9=MarkerTB,
        /// 10=FabricComboTb,
        /// 11=IssueTb,
        /// 12=OrderInfo,
        /// </summary>
        private enum TableType
        {
            WorkorderTb,
            WorkorderSizeTb,
            WorkorderDisTb,
            WorkorderPatternTb,
            CutrefTb,
            CutDisOrderIDTb,
            CutSizeTb,
            SizeTb,
            CutQtyTb,
            MarkerTB,
            FabricComboTb,
            IssueTb,
            OrderInfo,
        }

        /// <summary>
        /// 判斷由Cutting的P02、P09哪一個呼叫
        /// </summary>
        public enum FromCutting
        {
            /// <summary>
            /// From Cutting P02 - Planning
            /// </summary>
            P02,

            /// <summary>
            /// From Cutting P09 - Output
            /// </summary>
            P09,
        }
    }
}
