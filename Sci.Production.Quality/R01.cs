using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Quality
{
    public partial class R01 : Sci.Win.Tems.PrintForm
    {
        DateTime? DateLastStart; DateTime? DateLastEnd;
        DateTime? DateArrStart;  DateTime? DateArrEnd;
        DateTime? DateSCIStart;  DateTime? DateSCIEnd;
        DateTime? DateSewStart;  DateTime? DateSewEnd;
        DateTime? DateEstStart;  DateTime? DateEstEnd;
        string spStrat; string spEnd; string Sea; string Brand; string Ref; string Category; string Supp; string Over;
        string wkStrat;
        string wkEnd;
        List<SqlParameter> lis;
        DataTable dt; string cmd;
        public R01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.comboOverallResultStatus.SelectedIndex = 0;
            
            print.Enabled = false;
        }
        protected override bool ValidateInput()
        {
            bool date_Last_Empty = !this.dateLastPhysicalInspDate.HasValue, date_Arrive_Empty = !this.dateArriveWHDate.HasValue, date_SCI_Empty = !this.dateSCIDelivery.HasValue, date_Sewing_Empty = !this.dateSewingInLineDate.HasValue, date_Est_Empty = !this.dateEstCuttingDate.HasValue,
                textBox_SP_Empty = this.txtSPStart.Text.Empty(), textBox_SP2_Empty = this.txtSPEnd.Text.Empty(), txtSeason_Empty = this.txtSeason.Text.Empty()
           , txtBrand_Empty = this.txtBrand.Text.Empty(), txtRef_Empty = this.txtRefno.Text.Empty(), Cate_comboBox_Empty = this.comboCategory.Text.Empty(), Supp_Empty = this.txtsupplier.TextBox1.Text.Empty(), Over_comboBox_Empty = this.comboOverallResultStatus.Text.Empty()
           , textBox_WK_Empty = this.txtWK_start.Text.Empty(), textBox_WK2_Empty = this.txtWK_end.Text.Empty();

            if (date_Last_Empty && date_Arrive_Empty && date_SCI_Empty && textBox_SP_Empty && textBox_SP2_Empty && date_Sewing_Empty && date_Est_Empty && textBox_WK_Empty && textBox_WK2_Empty)
            {
                dateArriveWHDate.Focus();
                MyUtility.Msg.ErrorBox("Please select 'Last Inspection Date' or 'Arrive W/H Date' or 'SCI Delivery' or 'Sewing in-line Date' or 'Est. Cutting Date' or 'SP#' or 'WK#' at least one field entry");
                return false;
            }

            DateLastStart = dateLastPhysicalInspDate.Value1;
            DateLastEnd = dateLastPhysicalInspDate.Value2;
            DateArrStart = dateArriveWHDate.Value1;
            DateArrEnd = dateArriveWHDate.Value2;
            DateSCIStart = dateSCIDelivery.Value1;
            DateSCIEnd = dateSCIDelivery.Value2;
            DateSewStart = dateSewingInLineDate.Value1;
            DateSewEnd = dateSewingInLineDate.Value2;
            DateEstStart = dateEstCuttingDate.Value1;
            DateEstEnd = dateEstCuttingDate.Value2;
            spStrat = txtSPStart.Text.ToString();
            spEnd = txtSPEnd.Text.ToString();
            wkStrat = txtWK_start.Text.ToString();
            wkEnd = txtWK_end.Text.ToString();
            Sea = txtSeason.Text;
            Brand = txtBrand.Text;
            Ref = txtRefno.Text.ToString();
            Category = comboCategory.Text;
            Supp = txtsupplier.TextBox1.Text;
            Over = comboOverallResultStatus.SelectedItem.ToString();

            lis = new List<SqlParameter>();
            string sqlWhere = "",RWhere ="",OWhere=""; 
            List<string> sqlWheres = new List<string>();
            List<string> RWheres = new List<string>();
            List<string> OWheres = new List<string>();
            #region --組WHERE--
            if (!this.dateLastPhysicalInspDate.Value1.Empty())
            {
                sqlWheres.Add("F.PhysicalDate >= @LastDate1");
                lis.Add(new SqlParameter("@LastDate1", DateLastStart));
            }
            if (!this.dateLastPhysicalInspDate.Value2.Empty())
            {
                sqlWheres.Add("F.PhysicalDate <= @LastDate2");
                lis.Add(new SqlParameter("@LastDate2", DateLastEnd));
            }

            if (!this.dateArriveWHDate.Value1.Empty())
            {
                RWheres.Add("rd.WhseArrival >= @ArrDate1");
                lis.Add(new SqlParameter("@ArrDate1", DateArrStart));
            }
            if (!this.dateArriveWHDate.Value2.Empty())
            {
                RWheres.Add("rd.WhseArrival <= @ArrDate2");
                lis.Add(new SqlParameter("@ArrDate2", DateArrEnd));
            }
            
            if (!this.dateSCIDelivery.Value1.Empty())
            {
                OWheres.Add("O.SciDelivery >= @SCIDate1");
                lis.Add(new SqlParameter("@SCIDate1", DateSCIStart));
            }

            if (!this.dateSCIDelivery.Value2.Empty())
            {
                OWheres.Add("O.SciDelivery <= @SCIDate2");
                lis.Add(new SqlParameter("@SCIDate2", DateSCIEnd));
            }

            if (!this.dateSewingInLineDate.Value1.Empty())
            {
                OWheres.Add("O.SewInLine >= @SewDate1");
                lis.Add(new SqlParameter("@SewDate1", DateSewStart));
            }

            if (!this.dateSewingInLineDate.Value2.Empty())
            {
                OWheres.Add("O.SewInLine <= @SewDate2");
                lis.Add(new SqlParameter("@SewDate2", DateSewEnd));
            }

            if (!this.dateEstCuttingDate.Value1.Empty())
            {
                OWheres.Add("O.CutInLine >= @Est1");
                lis.Add(new SqlParameter("@Est1", DateEstStart));
            }

            if (!this.dateEstCuttingDate.Value2.Empty())
            {
                OWheres.Add("O.CutInLine <= @Est2");
                lis.Add(new SqlParameter("@Est2", DateEstEnd));
            }

            if (!this.txtSPStart.Text.Empty())
            {
                OWheres.Add("O.Id between @sp1 and @sp2");
                lis.Add(new SqlParameter("@sp1", spStrat));
                lis.Add(new SqlParameter("@sp2", spEnd));
            }

            if (!MyUtility.Check.Empty(wkStrat))
            {
                RWheres.Add("rd.ExportId between @wk1 and @wk2");
                lis.Add(new SqlParameter("@wk1", wkStrat));
                lis.Add(new SqlParameter("@wk2", wkEnd));
            }

            if (!this.txtSeason.Text.Empty())
            {
                OWheres.Add("O.SeasonID = @Sea");
                lis.Add(new SqlParameter("@Sea", Sea));
            }

            if (!this.txtBrand.Text.Empty())
            {
                OWheres.Add("O.BrandID = @Brand");
                lis.Add(new SqlParameter("@Brand", Brand));
            }

            if (!this.txtRefno.Text.Empty())
            {
                sqlWheres.Add("P.Refno = @Ref");
                lis.Add(new SqlParameter("@Ref", Ref));
            }

            if (!MyUtility.Check.Empty(this.comboCategory.Text))
            {
                if (Category != "")
                {
                    OWheres.Add($"O.Category in ({this.comboCategory.SelectedValue})");
                }
            }

            if (!this.txtsupplier.TextBox1.Text.Empty())
            {
                sqlWheres.Add("SP.SuppId = @Supp");
                lis.Add(new SqlParameter("@Supp", Supp));

            }

            if (this.comboOverallResultStatus.Text == "Pass")
            {
                sqlWheres.Add("f.result = 'Pass'");

            }

            if (this.comboOverallResultStatus.Text == "Fail")
            {
                sqlWheres.Add("f.result = 'Fail'");

            }

            if (this.comboOverallResultStatus.Text == "Empty Result")
            {
                sqlWheres.Add("f.result  = '' ");

            }

            #endregion
            sqlWhere = string.Join(" and ", sqlWheres);
            OWhere = string.Join(" and ", OWheres);
            RWhere = string.Join(" and ", RWheres);
            if (!sqlWhere.Empty())
            {
                sqlWhere = " where " + sqlWhere;
            }
            if (!RWhere.Empty())
            {
                RWhere = " where " + RWhere;
            }
            if (!OWhere.Empty())
            {
                OWhere = " where " + OWhere;
            }
            #region --撈ListExcel資料--

            cmd = string.Format($@"
SET ARITHABORT ON

    select 
    [BalanceQty]=sum(fit.inqty - fit.outqty + fit.adjustqty) 
    ,rd.poid
    ,rd.seq1
    ,rd.seq2
    ,RD.ID
    INTO #balanceTmp
from dbo.View_AllReceivingDetail rd
inner join FIR f on f.ReceivingID = rd.id AND f.POID=rd.poid AND  f.seq1 = rd.seq1 and f.seq2 = rd.Seq2
inner join FtyInventory fit on fit.poid = rd.PoId and fit.seq1 = rd.seq1 and fit.seq2 = rd.Seq2 AND fit.StockType=rd.StockType and fit.Roll=rd.Roll and fit.Dyelot=rd.Dyelot
    where 1=1
    {RWhere.Replace("where","AND")}
    {sqlWhere.Replace("where", "AND").Replace("SP.", "f.").Replace("P.","f.")}
    GROUP BY rd.poid,rd.seq1,rd.seq2,RD.ID

    select  
	F.POID
	,(F.SEQ1+'-'+F.SEQ2)SEQ
	,O.factoryid
	,O.BrandID
	,O.StyleID
	,O.SeasonID
	,t.ExportId
	,t.InvNo
	,t.WhseArrival
	,SUM(t.StockQty) AS StockQty1
	,[BalanceQty]=IIF(BalanceQty.BalanceQty=0,NULL,BalanceQty.BalanceQty)
	,t.TotalRollsCalculated
    ,mp.ALocation
    ,LT.BulkLocationDate
	,mp.BLocation	
	,[MinSciDelivery] = (SELECT MinSciDelivery FROM  DBO.GetSCI(F.Poid,O.Category))
	,[MinBuyerDelivery] = (SELECT MinBuyerDelivery  FROM  DBO.GetSCI(F.Poid,O.Category)),
	F.Refno,P.ColorID,(SP.SuppID+'-'+s.AbbEN)Supplier,
	C.WeaveTypeID,
	IIF(F.Nonphysical = 1,'Y',' ')[N/A Physical],
	F.Result,
	F.Physical,
	[PhysicalInspector] = (select name from Pass1 where id = f.PhysicalInspector),
	--F.PhysicalInspector,
	F.PhysicalDate,
	fta.ActualYds,
    ROUND(iif(SUM(t.StockQty) = 0,0,CAST (fta.ActualYds/SUM(t.StockQty) AS FLOAT)) ,3),
	ftp.TotalPoint,
	F.Weight,
	[WeightInspector] = (select name from Pass1 where id = f.WeightInspector),
	F.WeightDate,
	F.ShadeBond,
	[ShadeboneInspector] = (select name from Pass1 where id = f.ShadeboneInspector),
	F.ShadeBondDate,
	F.Continuity,
	[ContinuityInspector] = (select name from Pass1 where id = f.ContinuityInspector),
	F.ContinuityDate,
	F.Odor,
	[OdorInspector] = (select name from Pass1 where id = f.OdorInspector),
	F.OdorDate,
	L.Result AS Result2,
	IIF(L.nonCrocking=1,'Y',' ')[N/A Crocking],
	LC.Crocking,
	fl.CrockingInspector,
	LC.CrockingDate,
	IIF(L.nonHeat=1,'Y',' ')[N/A Heat Shrinkage],
	LH.Heat,
	fl.HeatInspector,
	LH.HeatDate,
	IIF(L.nonWash=1,'Y',' ' )[N/A Wash Shrinkage],
	LW.Wash,
	fl.WashInspector,
	LW.WashDate,
	V.Result AS RESULT3,
	[OvenInspector] = v.Name,
	CFD.Result AS RESULT4,
	[CFInspector] = cfd.Name,
    ps1.LocalMR
from dbo.FIR F WITH (NOLOCK) 
    inner join (select rd.WhseArrival,rd.InvNo,rd.ExportId,rd.Id,rd.PoId,RD.seq1,RD.seq2,RD.StockQty,
				TotalRollsCalculated=sum(iif(RD.StockQty>0,1,0)) over (partition by rd.id,rd.PoId,RD.seq1,RD.seq2,rd.ExportId)
			    from dbo.View_AllReceivingDetail rd WITH (NOLOCK) "
                + RWhere+ @" 
			    ) t
    on t.PoId = F.POID and t.Seq1 = F.SEQ1 and t.Seq2 = F.SEQ2 AND T.Id=F.ReceivingID
    inner join (select distinct poid,O.factoryid,O.BrandID,O.StyleID,O.SeasonID,O.Category,id from dbo.Orders o WITH (NOLOCK)  "
                + OWhere+ @"
		        ) O on O.id = F.POID
    inner join dbo.PO_Supp SP WITH (NOLOCK) on SP.id = F.POID and SP.SEQ1 = F.SEQ1
    inner join dbo.PO_Supp_Detail P WITH (NOLOCK) on P.ID = F.POID and P.SEQ1 = F.SEQ1 and P.SEQ2 = F.SEQ2
    inner join supp s WITH (NOLOCK) on s.id = SP.SuppID 
	LEFT JOIN #balanceTmp BalanceQty ON BalanceQty.poid = f.POID and BalanceQty.seq1 = f.seq1 and BalanceQty.seq2 =f.seq2 AND BalanceQty.ID = f.ReceivingID
    left join MDivisionPoDetail mp on mp.POID=f.POID and mp.Seq1=f.SEQ1 and mp.Seq2=f.SEQ2
    OUTER APPLY(SELECT * FROM  Fabric C WITH (NOLOCK) WHERE C.SCIRefno = F.SCIRefno)C
OUTER APPLY(
		SELECT * FROM  FIR_Laboratory L WITH (NOLOCK) WHERE 1=1		
		AND L.ID = F.ID AND L.SEQ1 = F.SEQ1 AND L.SEQ2 = F.SEQ2
		)L
OUTER APPLY(
		SELECT * FROM  FIR_Laboratory L WITH (NOLOCK) WHERE 1=1
		AND L.CrockingEncode=1 
		AND L.ID = F.ID AND L.SEQ1 = F.SEQ1 AND L.SEQ2 = F.SEQ2
		)LC
OUTER APPLY(
		SELECT * FROM  FIR_Laboratory L WITH (NOLOCK) WHERE 1=1 
		AND L.HeatEncode=1 
		AND L.ID = F.ID AND L.SEQ1 = F.SEQ1 AND L.SEQ2 = F.SEQ2
		)LH
OUTER APPLY(
		SELECT * FROM  FIR_Laboratory L WITH (NOLOCK) WHERE 1=1
		AND L.WashEncode=1 
		AND L.ID = F.ID AND L.SEQ1 = F.SEQ1 AND L.SEQ2 = F.SEQ2
		)LW
OUTER APPLY(
        select od.Result, pass1.name from dbo.Oven ov WITH (NOLOCK) 
        inner join dbo.Oven_Detail od WITH (NOLOCK) on od.ID = ov.ID
        left join pass1 WITH (NOLOCK) on pass1.id = ov.Inspector
        where ov.POID=F.POID and od.SEQ1=F.Seq1 and seq2=F.Seq2 and ov.Status='Confirmed'
        )V
OUTER APPLY(
        select distinct cd.Result, pass1.name from dbo.ColorFastness CF WITH (NOLOCK) 
        inner join dbo.ColorFastness_Detail cd WITH (NOLOCK) on cd.ID = CF.ID
        left join pass1 WITH (NOLOCK) on pass1.id = cf.Inspector
        where CF.Status = 'Confirmed' and CF.POID=F.POID and cd.SEQ1=F.Seq1 and cd.seq2=F.Seq2
        )CFD
Outer apply(
	select (A.id+' - '+ A.name + ' #'+A.extno) LocalMR 
    from orders od 
    inner join pass1 a on a.id=od.LocalMR 
    where od.id=o.POID
) ps1
outer apply(select TotalPoint = Sum(fp.TotalPoint) from FIR_Physical fp where fp.id=f.id)ftp
outer apply(select ActualYds = Sum(fp.ActualYds) from FIR_Physical fp where fp.id=f.id)fta
outer apply(select  CrockingInspector = (select name from Pass1 where id = CrockingInspector)
	,HeatInspector = (select name from Pass1 where id = HeatInspector)
	,WashInspector = (select name from Pass1 where id = WashInspector)
	from FIR_Laboratory where Id=f.ID
)FL
outer apply
(
	SELECT Max(a.EditDate) BulkLocationDate
	FROM LocationTrans a WITH (NOLOCK) 
	inner join LocationTrans_detail as b WITH (NOLOCK) on a.ID = b.ID 
	WHERE a.status = 'Confirmed' and b.stocktype='B'
	AND b.Poid=f.POID and b.Seq1=f.SEQ1 and b.Seq2=f.SEQ2
)LT
--OUTER APPLY(
--	 select [BalanceQty]=sum(fit.inqty) - sum(fit.outqty) + sum(fit.adjustqty) 
--	 from Receiving_Detail rd
--	 inner join FIR fir on fir.ReceivingID = rd.id
--	 inner join FtyInventory fit on fit.poid = rd.PoId and fit.seq1 = rd.seq1 and fit.seq2 = rd.Seq2
--	 where FIt.StockType IN('B','I') AND rd.poid = f.POID and rd.seq1 = f.seq1 and rd.seq2 =f.seq2 AND RD.ID = f.ReceivingID
--)BalanceQty

" + sqlWhere) + @" 
GROUP BY 
F.POID,F.SEQ1,F.SEQ2,O.factoryid,O.BrandID,O.StyleID,O.SeasonID,
t.ExportId,t.InvNo,t.WhseArrival,
F.Refno,P.ColorID,C.WeaveTypeID,O.Category
,F.Result,F.Physical,F.PhysicalDate,
F.TotalInspYds,fta.ActualYds,F.Weight,F.WeightDate,F.ShadeBond,F.ShadeBondDate,F.Continuity,
F.ContinuityDate,L.Result,LC.Crocking,
LC.CrockingDate,LH.Heat,LH.HeatDate,
LW.Wash,LW.WashDate,V.Result,CFD.Result,SP.SuppID,S.AbbEN,F.Nonphysical,L.nonCrocking,L.nonHeat,L.nonWash,ps1.LocalMR,
ftp.TotalPoint,F.Odor,F.OdorDate,f.PhysicalInspector,f.WeightInspector
,f.ShadeboneInspector,f.ContinuityInspector,f.OdorInspector
,fl.CrockingInspector,fl.HeatInspector,fl.WashInspector,v.Name,cfd.Name
,t.TotalRollsCalculated
,BalanceQty.BalanceQty
,mp.ALocation,mp.BLocation,LT.BulkLocationDate

ORDER BY POID,SEQ
OPTION (OPTIMIZE FOR UNKNOWN)
";
            #endregion
            return base.ValidateInput();
        }
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            DualResult res;
            res = DBProxy.Current.Select("", cmd, lis, out dt);
            if (!res)
            {
                return res;
            }
            return res;
        }
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            SetCount(dt.Rows.Count);
            if (dt == null || dt.Rows.Count == 0)
            {
                MyUtility.Msg.ErrorBox("Data not found");
                return false;
            } 
            var saveDialog = Sci.Utility.Excel.MyExcelPrg.GetSaveFileDialog(Sci.Utility.Excel.MyExcelPrg.Filter_Excel);

            Sci.Utility.Excel.SaveXltReportCls xl = new Sci.Utility.Excel.SaveXltReportCls("Quality_R01.xltx", keepApp: true);
            Sci.Utility.Excel.SaveXltReportCls.XltRptTable dt1 = new Utility.Excel.SaveXltReportCls.XltRptTable(dt);
            string d1 = (MyUtility.Check.Empty(DateArrStart)) ? "" : Convert.ToDateTime(DateArrStart).ToString("yyyy/MM/dd");
            string d2 = (MyUtility.Check.Empty(DateArrEnd)) ? "" : Convert.ToDateTime(DateArrEnd).ToString("yyyy/MM/dd");
            string d3 = (MyUtility.Check.Empty(DateSCIStart)) ? "" : Convert.ToDateTime(DateSCIStart).ToString("yyyy/MM/dd");
            string d4 = (MyUtility.Check.Empty(DateSCIEnd)) ? "" : Convert.ToDateTime(DateSCIEnd).ToString("yyyy/MM/dd");
            string d5 = (MyUtility.Check.Empty(DateSewStart)) ? "" : Convert.ToDateTime(DateSewStart).ToString("yyyy/MM/dd");
            string d6 = (MyUtility.Check.Empty(DateSewEnd)) ? "" : Convert.ToDateTime(DateSewEnd).ToString("yyyy/MM/dd");
            string d7 = (MyUtility.Check.Empty(DateEstStart)) ? "" : Convert.ToDateTime(DateEstStart).ToString("yyyy/MM/dd");
            string d8 = (MyUtility.Check.Empty(DateEstEnd)) ? "" : Convert.ToDateTime(DateEstEnd).ToString("yyyy/MM/dd");
            xl.DicDatas.Add("##Arr", d1 + "~" + d2);
            xl.DicDatas.Add("##SCI", d3 + "~" + d4);
            xl.DicDatas.Add("##Sew", d5 + "~" + d6);
            xl.DicDatas.Add("##Est", d7 + "~" + d8);
            xl.DicDatas.Add("##SP", spStrat + "~" + spEnd);
            xl.DicDatas.Add("##Sea", Sea);
            xl.DicDatas.Add("##Brand", Brand);
            xl.DicDatas.Add("##Ref", Ref);
            xl.DicDatas.Add("##Cate", Category);
            xl.DicDatas.Add("##supp", Supp);
            xl.DicDatas.Add("##Over", Over);
            xl.DicDatas.Add("##body", dt1);            
            dt1.ShowHeader = false;       

            xl.Save(Sci.Production.Class.MicrosoftFile.GetName("Quality_R01"));
            ((Microsoft.Office.Interop.Excel.Worksheet)xl.ExcelApp.ActiveSheet).Columns.AutoFit();
            xl.FinishSave();
            return true;
            
        }
    }
}
