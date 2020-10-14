using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Sci.Production.Quality
{
    public partial class R01 : Win.Tems.PrintForm
    {
        private DateTime? DateLastStart; private DateTime? DateLastEnd;
        private DateTime? DateArrStart;  private DateTime? DateArrEnd;
        private DateTime? DateSCIStart;  private DateTime? DateSCIEnd;
        private DateTime? DateSewStart;  private DateTime? DateSewEnd;
        private DateTime? DateEstStart;  private DateTime? DateEstEnd;
        private string spStrat; private string spEnd; private string Sea; private string Brand; private string Ref; private string Category; private string Supp; private string Over;
        private string wkStrat;
        private string wkEnd;
        private List<SqlParameter> lis;
        private DataTable dt; private string cmd;

        /// <summary>
        /// Initializes a new instance of the <see cref="R01"/> class.
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public R01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.comboOverallResultStatus.SelectedIndex = 0;

            this.print.Enabled = false;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            bool date_Last_Empty = !this.dateLastPhysicalInspDate.HasValue, date_Arrive_Empty = !this.dateArriveWHDate.HasValue, date_SCI_Empty = !this.dateSCIDelivery.HasValue, date_Sewing_Empty = !this.dateSewingInLineDate.HasValue, date_Est_Empty = !this.dateEstCuttingDate.HasValue, textBox_SP_Empty = this.txtSPStart.Text.Empty(), textBox_SP2_Empty = this.txtSPEnd.Text.Empty();
            bool textBox_WK_Empty = this.txtWK_start.Text.Empty(), textBox_WK2_Empty = this.txtWK_end.Text.Empty();

            if (date_Last_Empty && date_Arrive_Empty && date_SCI_Empty && textBox_SP_Empty && textBox_SP2_Empty && date_Sewing_Empty && date_Est_Empty && textBox_WK_Empty && textBox_WK2_Empty)
            {
                this.dateArriveWHDate.Focus();
                MyUtility.Msg.ErrorBox("Please select 'Last Inspection Date' or 'Arrive W/H Date' or 'SCI Delivery' or 'Sewing in-line Date' or 'Est. Cutting Date' or 'SP#' or 'WK#' at least one field entry");
                return false;
            }

            this.DateLastStart = this.dateLastPhysicalInspDate.Value1;
            this.DateLastEnd = this.dateLastPhysicalInspDate.Value2;
            this.DateArrStart = this.dateArriveWHDate.Value1;
            this.DateArrEnd = this.dateArriveWHDate.Value2;
            this.DateSCIStart = this.dateSCIDelivery.Value1;
            this.DateSCIEnd = this.dateSCIDelivery.Value2;
            this.DateSewStart = this.dateSewingInLineDate.Value1;
            this.DateSewEnd = this.dateSewingInLineDate.Value2;
            this.DateEstStart = this.dateEstCuttingDate.Value1;
            this.DateEstEnd = this.dateEstCuttingDate.Value2;
            this.spStrat = this.txtSPStart.Text.ToString();
            this.spEnd = this.txtSPEnd.Text.ToString();
            this.wkStrat = this.txtWK_start.Text.ToString();
            this.wkEnd = this.txtWK_end.Text.ToString();
            this.Sea = this.txtSeason.Text;
            this.Brand = this.txtBrand.Text;
            this.Ref = this.txtRefno.Text.ToString();
            this.Category = this.comboCategory.Text;
            this.Supp = this.txtsupplier.TextBox1.Text;
            this.Over = this.comboOverallResultStatus.SelectedItem.ToString();

            this.lis = new List<SqlParameter>();
            string sqlWhere = string.Empty, rWhere = string.Empty, oWhere = string.Empty;
            List<string> sqlWheres = new List<string>();
            List<string> rWheres = new List<string>();
            List<string> oWheres = new List<string>();
            #region --組WHERE--
            if (!this.dateLastPhysicalInspDate.Value1.Empty())
            {
                sqlWheres.Add("F.PhysicalDate >= @LastDate1");
                this.lis.Add(new SqlParameter("@LastDate1", this.DateLastStart));
            }

            if (!this.dateLastPhysicalInspDate.Value2.Empty())
            {
                sqlWheres.Add("F.PhysicalDate <= @LastDate2");
                this.lis.Add(new SqlParameter("@LastDate2", this.DateLastEnd));
            }

            if (!this.dateArriveWHDate.Value1.Empty())
            {
                rWheres.Add("rd.WhseArrival >= @ArrDate1");
                this.lis.Add(new SqlParameter("@ArrDate1", this.DateArrStart));
            }

            if (!this.dateArriveWHDate.Value2.Empty())
            {
                rWheres.Add("rd.WhseArrival <= @ArrDate2");
                this.lis.Add(new SqlParameter("@ArrDate2", this.DateArrEnd));
            }

            if (!this.dateSCIDelivery.Value1.Empty())
            {
                oWheres.Add("O.SciDelivery >= @SCIDate1");
                this.lis.Add(new SqlParameter("@SCIDate1", this.DateSCIStart));
            }

            if (!this.dateSCIDelivery.Value2.Empty())
            {
                oWheres.Add("O.SciDelivery <= @SCIDate2");
                this.lis.Add(new SqlParameter("@SCIDate2", this.DateSCIEnd));
            }

            if (!this.dateSewingInLineDate.Value1.Empty())
            {
                oWheres.Add("O.SewInLine >= @SewDate1");
                this.lis.Add(new SqlParameter("@SewDate1", this.DateSewStart));
            }

            if (!this.dateSewingInLineDate.Value2.Empty())
            {
                oWheres.Add("O.SewInLine <= @SewDate2");
                this.lis.Add(new SqlParameter("@SewDate2", this.DateSewEnd));
            }

            if (!this.dateEstCuttingDate.Value1.Empty())
            {
                oWheres.Add("O.CutInLine >= @Est1");
                this.lis.Add(new SqlParameter("@Est1", this.DateEstStart));
            }

            if (!this.dateEstCuttingDate.Value2.Empty())
            {
                oWheres.Add("O.CutInLine <= @Est2");
                this.lis.Add(new SqlParameter("@Est2", this.DateEstEnd));
            }

            if (!this.txtSPStart.Text.Empty())
            {
                oWheres.Add("O.Id between @sp1 and @sp2");
                this.lis.Add(new SqlParameter("@sp1", this.spStrat));
                this.lis.Add(new SqlParameter("@sp2", this.spEnd));
            }

            if (!MyUtility.Check.Empty(this.wkStrat))
            {
                rWheres.Add("rd.ExportId between @wk1 and @wk2");
                this.lis.Add(new SqlParameter("@wk1", this.wkStrat));
                this.lis.Add(new SqlParameter("@wk2", this.wkEnd));
            }

            if (!this.txtSeason.Text.Empty())
            {
                oWheres.Add("O.SeasonID = @Sea");
                this.lis.Add(new SqlParameter("@Sea", this.Sea));
            }

            if (!this.txtBrand.Text.Empty())
            {
                oWheres.Add("O.BrandID = @Brand");
                this.lis.Add(new SqlParameter("@Brand", this.Brand));
            }

            if (!this.txtRefno.Text.Empty())
            {
                sqlWheres.Add("P.Refno = @Ref");
                this.lis.Add(new SqlParameter("@Ref", this.Ref));
            }

            if (!MyUtility.Check.Empty(this.comboCategory.Text))
            {
                if (this.Category != string.Empty)
                {
                    oWheres.Add($"O.Category in ({this.comboCategory.SelectedValue})");
                }
            }

            if (!this.txtsupplier.TextBox1.Text.Empty())
            {
                sqlWheres.Add("SP.SuppId = @Supp");
                this.lis.Add(new SqlParameter("@Supp", this.Supp));
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
            oWhere = string.Join(" and ", oWheres);
            rWhere = string.Join(" and ", rWheres);
            if (!sqlWhere.Empty())
            {
                sqlWhere = " where " + sqlWhere;
            }

            if (!rWhere.Empty())
            {
                rWhere = " where " + rWhere;
            }

            if (!oWhere.Empty())
            {
                oWhere = " where " + oWhere;
            }
            #region --撈ListExcel資料--

            this.cmd = $@"
SET ARITHABORT ON

select 
    [BalanceQty]=sum(fit.inqty - fit.outqty + fit.adjustqty) 
    ,rd.poid
    ,rd.seq1
    ,rd.seq2
    ,RD.ID
INTO #balanceTmp
from dbo.View_AllReceivingDetail rd
inner join FIR f on f.POID=rd.poid AND  f.ReceivingID = rd.id AND f.seq1 = rd.seq1 and f.seq2 = rd.Seq2
inner join FtyInventory fit on fit.poid = rd.PoId and fit.seq1 = rd.seq1 and fit.seq2 = rd.Seq2 AND fit.StockType=rd.StockType and fit.Roll=rd.Roll and fit.Dyelot=rd.Dyelot
where 1=1 
    {rWhere.Replace("where", "AND")}
    {sqlWhere.Replace("where", "AND").Replace("SP.", "f.").Replace("P.", "f.")}
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
	,[StockQty1] = t.StockQty
    ,[InvStock] = t.InvStock
    ,[BulkStock] = t.BulkStock
	,[BalanceQty] = IIF(BalanceQty.BalanceQty=0,NULL,BalanceQty.BalanceQty)
	,[TotalRollsCalculated] = t.TotalRollsCalculated
    ,mp.ALocation
    ,LT.BulkLocationDate
	,mp.BLocation	
	,[MinSciDelivery] = (SELECT MinSciDelivery FROM  DBO.GetSCI(F.Poid,O.Category))
	,[MinBuyerDelivery] = (SELECT MinBuyerDelivery  FROM  DBO.GetSCI(F.Poid,O.Category))
	,F.Refno,C.Description,P.ColorID,(SP.SuppID+'-'+s.AbbEN)Supplier
	,C.WeaveTypeID
	,[N/A Physical] = IIF(F.Nonphysical = 1,'Y',' ')
	,F.Result
	,F.Physical
	,[PhysicalInspector] = (select name from Pass1 where id = f.PhysicalInspector)
	,F.PhysicalDate
	,fta.ActualYds
	,[InspectionRate] = ROUND(iif(t.StockQty = 0,0,CAST (fta.ActualYds/t.StockQty AS FLOAT)) ,3)
	,ftp.TotalPoint
	,F.Weight
	,[WeightInspector] = (select name from Pass1 where id = f.WeightInspector)
	,F.WeightDate
	,F.ShadeBond
	,[ShadeboneInspector] = (select name from Pass1 where id = f.ShadeboneInspector)
	,F.ShadeBondDate
	,F.Continuity
	,[ContinuityInspector] = (select name from Pass1 where id = f.ContinuityInspector)
	,F.ContinuityDate
	,F.Odor
	,[OdorInspector] = (select name from Pass1 where id = f.OdorInspector)
	,F.OdorDate
	,[Result2] = L.Result
	,[N/A Crocking] = IIF(L.nonCrocking=1,'Y',' ')
	,LC.Crocking
	,fl.CrockingInspector
	,LC.CrockingDate
	,[N/A Heat Shrinkage] = IIF(L.nonHeat=1,'Y',' ')
	,LH.Heat
	,fl.HeatInspector
	,LH.HeatDate
	,[N/A Wash Shrinkage] = IIF(L.nonWash=1,'Y',' ' )
	,LW.Wash
	,fl.WashInspector
	,LW.WashDate
	,RESULT3 = V.Result
	,[OvenInspector] = v2.Name
	,RESULT4 = CFD.Result
	,[CFInspector] = cfd2.Name
	,ps1.LocalMR
into #tmpFinal
from dbo.FIR F WITH (NOLOCK) 
cross apply(
	select rd.WhseArrival,rd.InvNo,rd.ExportId,rd.Id,rd.PoId,RD.seq1,RD.seq2
	,[StockQty] = sum(RD.StockQty)
	,[InvStock] = iif(rd.StockType = 'I', sum(RD.StockQty), 0)
	,[BulkStock] = iif(rd.StockType = 'B', sum(RD.StockQty), 0)
    ,TotalRollsCalculated = count(1)
	from dbo.View_AllReceivingDetail rd WITH (NOLOCK) 
	where rd.PoId = F.POID and rd.Seq1 = F.SEQ1 and rd.Seq2 = F.SEQ2 AND rd.Id=F.ReceivingID
    {rWhere.Replace("where", "AND")}
    group by rd.WhseArrival,rd.InvNo,rd.ExportId,rd.Id,rd.PoId,RD.seq1,RD.seq2,rd.StockType
) t
inner join (
    select distinct poid,O.factoryid,O.BrandID,O.StyleID,O.SeasonID,O.Category,id 
    from dbo.Orders o WITH (NOLOCK)  
    {oWhere}
) O on O.id = F.POID
inner join dbo.PO_Supp SP WITH (NOLOCK) on SP.id = F.POID and SP.SEQ1 = F.SEQ1
inner join dbo.PO_Supp_Detail P WITH (NOLOCK) on P.ID = F.POID and P.SEQ1 = F.SEQ1 and P.SEQ2 = F.SEQ2
inner join supp s WITH (NOLOCK) on s.id = SP.SuppID 
LEFT JOIN #balanceTmp BalanceQty ON BalanceQty.poid = f.POID and BalanceQty.seq1 = f.seq1 and BalanceQty.seq2 =f.seq2 AND BalanceQty.ID = f.ReceivingID
left join MDivisionPoDetail mp on mp.POID=f.POID and mp.Seq1=f.SEQ1 and mp.Seq2=f.SEQ2
OUTER APPLY(
	SELECT * FROM  Fabric C WITH (NOLOCK) WHERE C.SCIRefno = F.SCIRefno
)C
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
select Result = Stuff((
		select concat(',',Result)
		from (
			select distinct od.Result 
			from dbo.Oven ov WITH (NOLOCK) 
			inner join dbo.Oven_Detail od WITH (NOLOCK) on od.ID = ov.ID
			left join pass1 WITH (NOLOCK) on pass1.id = ov.Inspector
			where ov.POID=F.POID and od.SEQ1=F.Seq1 and seq2=F.Seq2 
			and ov.Status='Confirmed'
		) s
		for xml path ('')
	) , 1, 1, '')
)V
OUTER APPLY(
select [name ]= Stuff((
		select concat(',',name)
		from (
			select distinct pass1.name 
			from dbo.Oven ov WITH (NOLOCK) 
			inner join dbo.Oven_Detail od WITH (NOLOCK) on od.ID = ov.ID
			left join pass1 WITH (NOLOCK) on pass1.id = ov.Inspector
			where ov.POID=F.POID and od.SEQ1=F.Seq1 and seq2=F.Seq2 
			and ov.Status='Confirmed'
		) s
		for xml path ('')
	) , 1, 1, '')
)V2
OUTER APPLY(
select Result = Stuff((
		select concat(',',Result)
		from (
			select distinct cd.Result
			from dbo.ColorFastness CF WITH (NOLOCK) 
			inner join dbo.ColorFastness_Detail cd WITH (NOLOCK) on cd.ID = CF.ID
			left join pass1 WITH (NOLOCK) on pass1.id = cf.Inspector
			where CF.Status = 'Confirmed' 
			and CF.POID=F.POID and cd.SEQ1=F.Seq1 and cd.seq2=F.Seq2
		) s
		for xml path ('')
	) , 1, 1, '')
)CFD
OUTER APPLY(
select Name = Stuff((
		select concat(',',Name)
		from (
			select distinct Pass1.Name
			from dbo.ColorFastness CF WITH (NOLOCK) 
			inner join dbo.ColorFastness_Detail cd WITH (NOLOCK) on cd.ID = CF.ID
			left join pass1 WITH (NOLOCK) on pass1.id = cf.Inspector
			where CF.Status = 'Confirmed' 
			and CF.POID=F.POID and cd.SEQ1=F.Seq1 and cd.seq2=F.Seq2
		) s
		for xml path ('')
	) , 1, 1, '')
)CFD2
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
{sqlWhere} 
ORDER BY POID,SEQ
OPTION (OPTIMIZE FOR UNKNOWN)


select
     tf.POID
	,tf.SEQ
	,tf.factoryid
	,tf.BrandID
	,tf.StyleID
	,tf.SeasonID
	,tf.ExportId
	,tf.InvNo
	,tf.WhseArrival
	,tf.StockQty1
    ,tf.InvStock
    ,tf.BulkStock
	,tf.BalanceQty
	,tf.TotalRollsCalculated
    ,tf.ALocation
    ,tf.BulkLocationDate
	,tf.BLocation	
	,tf.MinSciDelivery
	,tf.MinBuyerDelivery
	,tf.Refno
    ,tf.Description
    ,tf.ColorID
    ,tf.Supplier
	,tf.WeaveTypeID
	,tf.[N/A Physical]
	,tf.Result
	,tf.Physical
	,tf.[PhysicalInspector]
	,tf.PhysicalDate
	,tf.ActualYds
    ,tf.InspectionRate
	,tf.TotalPoint
	,tf.Weight
	,tf.WeightInspector
	,tf.WeightDate
	,tf.ShadeBond
	,tf.ShadeboneInspector
	,tf.ShadeBondDate
	,tf.Continuity
	,tf.ContinuityInspector
	,tf.ContinuityDate
	,tf.Odor
	,tf.OdorInspector
	,tf.OdorDate
	,tf.Result2
	,tf.[N/A Crocking]
	,tf.Crocking
	,tf.CrockingInspector
	,tf.CrockingDate
	,tf.[N/A Heat Shrinkage]
	,tf.Heat
	,tf.HeatInspector
	,tf.HeatDate
	,tf.[N/A Wash Shrinkage]
	,tf.Wash
	,tf.WashInspector
	,tf.WashDate
	,tf.RESULT3
	,tf.OvenInspector
	,tf.RESULT4
	,tf.CFInspector
    ,tf.LocalMR
from #tmpFinal tf
ORDER BY POID,SEQ

";
            #endregion
            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            DualResult res;
            int defaultTimeout = DBProxy.Current.DefaultTimeout;
            DBProxy.Current.DefaultTimeout = 1800;
            res = DBProxy.Current.Select(string.Empty, this.cmd, this.lis, out this.dt);
            DBProxy.Current.DefaultTimeout = defaultTimeout;
            if (!res)
            {
                return res;
            }

            return res;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.dt.Rows.Count);
            if (this.dt == null || this.dt.Rows.Count == 0)
            {
                MyUtility.Msg.ErrorBox("Data not found");
                return false;
            }

            _ = Utility.Excel.MyExcelPrg.GetSaveFileDialog(Utility.Excel.MyExcelPrg.Filter_Excel);

            Utility.Excel.SaveXltReportCls xl = new Utility.Excel.SaveXltReportCls("Quality_R01.xltx", keepApp: true);
            Utility.Excel.SaveXltReportCls.XltRptTable dt1 = new Utility.Excel.SaveXltReportCls.XltRptTable(this.dt);
            string d1 = MyUtility.Check.Empty(this.DateArrStart) ? string.Empty : Convert.ToDateTime(this.DateArrStart).ToString("yyyy/MM/dd");
            string d2 = MyUtility.Check.Empty(this.DateArrEnd) ? string.Empty : Convert.ToDateTime(this.DateArrEnd).ToString("yyyy/MM/dd");
            string d3 = MyUtility.Check.Empty(this.DateSCIStart) ? string.Empty : Convert.ToDateTime(this.DateSCIStart).ToString("yyyy/MM/dd");
            string d4 = MyUtility.Check.Empty(this.DateSCIEnd) ? string.Empty : Convert.ToDateTime(this.DateSCIEnd).ToString("yyyy/MM/dd");
            string d5 = MyUtility.Check.Empty(this.DateSewStart) ? string.Empty : Convert.ToDateTime(this.DateSewStart).ToString("yyyy/MM/dd");
            string d6 = MyUtility.Check.Empty(this.DateSewEnd) ? string.Empty : Convert.ToDateTime(this.DateSewEnd).ToString("yyyy/MM/dd");
            string d7 = MyUtility.Check.Empty(this.DateEstStart) ? string.Empty : Convert.ToDateTime(this.DateEstStart).ToString("yyyy/MM/dd");
            string d8 = MyUtility.Check.Empty(this.DateEstEnd) ? string.Empty : Convert.ToDateTime(this.DateEstEnd).ToString("yyyy/MM/dd");
            xl.DicDatas.Add("##Arr", d1 + "~" + d2);
            xl.DicDatas.Add("##SCI", d3 + "~" + d4);
            xl.DicDatas.Add("##Sew", d5 + "~" + d6);
            xl.DicDatas.Add("##Est", d7 + "~" + d8);
            xl.DicDatas.Add("##SP", this.spStrat + "~" + this.spEnd);
            xl.DicDatas.Add("##Sea", this.Sea);
            xl.DicDatas.Add("##Brand", this.Brand);
            xl.DicDatas.Add("##Ref", this.Ref);
            xl.DicDatas.Add("##Cate", this.Category);
            xl.DicDatas.Add("##supp", this.Supp);
            xl.DicDatas.Add("##Over", this.Over);
            xl.DicDatas.Add("##body", dt1);
            dt1.ShowHeader = false;

            xl.Save(Class.MicrosoftFile.GetName("Quality_R01"));
            ((Microsoft.Office.Interop.Excel.Worksheet)xl.ExcelApp.ActiveSheet).Columns.AutoFit();
            xl.FinishSave();
            return true;
        }
    }
}
