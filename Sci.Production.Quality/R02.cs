using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Sci.Production.Quality
{
    /// <inheritdoc/>
    public partial class R02 : Win.Tems.PrintForm
    {
        private DateTime? DateArrStart; private DateTime? DateArrEnd;
        private DateTime? DateSCIStart; private DateTime? DateSCIEnd;
        private DateTime? DateSewStart; private DateTime? DateSewEnd;
        private DateTime? DateEstStart; private DateTime? DateEstEnd;
        private string spStrat; private string spEnd; private string Sea; private string Brand; private string Ref; private string Category; private string Supp; private string Over;
        private List<SqlParameter> lis;
        private DataTable dt; private string cmd;

        /// <summary>
        /// Initializes a new instance of the <see cref="R02"/> class.
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public R02(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.comboOverallResultStatus.SelectedIndex = 0;

            this.print.Enabled = false;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            bool date_Arrive_Empty = !this.dateArriveWHDate.HasValue, date_SCI_Empty = !this.dateSCIDelivery.HasValue, date_Sewing_Empty = !this.dateSewingInLineDate.HasValue, date_Est_Empty = !this.dateEstCuttingDate.HasValue,
               textBox_SP_Empty = this.txtSPStart.Text.Empty(), textBox_SP2_Empty = this.txtSPEnd.Text.Empty(), txtSEA_Empty = this.txtSeason.Text.Empty(),
               txtBrand_Empty = this.txtBrand.Text.Empty(), txtRef_Empty = this.txtRefno.Text.Empty(), cate_comboBox_Empty = this.comboCategory.Text.Empty(), supp_Empty = !this.txtsupplier.Text.Empty(), over_comboBox_Empty = this.comboOverallResultStatus.Text.Empty();

            if (date_Arrive_Empty && date_SCI_Empty && date_Sewing_Empty && date_Est_Empty && textBox_SP_Empty && textBox_SP2_Empty)
            {
                this.dateArriveWHDate.Focus();
                MyUtility.Msg.ErrorBox("Please select 'Arrive W/H Date' or 'SCI Delivery' or 'Sewing in-line Date' or 'Est. Cutting Date' or 'SP#'  at least one field entry");
                return false;
            }

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

            if (!this.dateArriveWHDate.Value1.Empty())
             {
                 rWheres.Add("R.WhseArrival >= @ArrDate1");
                 this.lis.Add(new SqlParameter("@ArrDate1", this.DateArrStart));
             }

            if (!this.dateArriveWHDate.Value2.Empty())
             {
                 rWheres.Add("R.WhseArrival <= @ArrDate2");
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

            if (!this.dateEstCuttingDate.Value1.Empty() && !this.dateEstCuttingDate.Value2.Empty())
            {
                oWheres.Add("O.CutInLine between @Est1 and @Est2");
                this.lis.Add(new SqlParameter("@Est1", this.DateEstStart));
                this.lis.Add(new SqlParameter("@Est2", this.DateEstEnd));
            }

            if (!this.txtSPStart.Text.Empty())
            {
                oWheres.Add("O.Id between @sp1 and @sp2");
                this.lis.Add(new SqlParameter("@sp1", this.spStrat));
                this.lis.Add(new SqlParameter("@sp2", this.spEnd));
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
                sqlWheres.Add("PS.Refno = @Ref");
                this.lis.Add(new SqlParameter("@Ref", this.Ref));
            }

            if (!MyUtility.Check.Empty(this.comboCategory.Text))
            {
                oWheres.Add($"O.Category in ({this.comboCategory.SelectedValue})");
            }

            if (!this.txtsupplier.TextBox1.Text.Empty())
            {
                sqlWheres.Add("P.SuppId = @Supp");
                this.lis.Add(new SqlParameter("@Supp", this.Supp));
            }

            if (this.comboOverallResultStatus.Text == "Pass")
            {
                sqlWheres.Add("dbo.GetFirResult(A.id) = 'Pass'");
            }

            if (this.comboOverallResultStatus.Text == "Fail")
            {
                sqlWheres.Add("dbo.GetFirResult(A.id) = 'Faill'");
            }

            if (this.comboOverallResultStatus.Text == "Empty Result")
            {
                sqlWheres.Add("dbo.GetFirResult(A.id) = ' '");
            }

            if (this.comboOverallResultStatus.Text == "N/A inspection & test")
            {
                sqlWheres.Add("dbo.GetFirResult(A.id) = 'None'");
            }

            #endregion
            sqlWhere = string.Join(" and ", sqlWheres);
            oWhere = string.Join(" and ", oWheres);
            rWhere = string.Join(" and ", rWheres);
            if (sqlWheres.Count != 0)
            {
                sqlWhere = " where " + sqlWhere;
            }

            if (rWheres.Count != 0)
            {
                rWhere = " where " + rWhere;
            }

            if (oWheres.Count != 0)
            {
                oWhere = " where " + oWhere;
            }
            #region --撈ListExcel資料--

            this.cmd = string.Format(
                @"
select A.POID
	,(A.seq1+'-'+A.seq2)SEQ
	,x.FactoryID
	,x.BrandID
	,x.StyleID
	,x.SeasonID
	,t.ExportId
	,t.InvNo
	,t.WhseArrival
	,t.StockQty
	,(Select MinSciDelivery from DBO.GetSCI(A.poid,x.Category))[MinSciDelivery]
	,(Select MinBuyerDelivery from DBO.GetSCI(A.poid,x.Category))[MinBuyerDelivery]
	,A.refno
	,[Article] = Style.Article
	,[MaterialType] = fabric.MtlTypeID
	,iif(C.Name is null,oc.name,c.name ) name
	,PS.SizeSpec
	,PS.stockunit
	,(P.SuppID+'-'+s.AbbEN)Supplier
	,[OrderQty] = Round(dbo.getUnitQty(PS.POUnit, PS.StockUnit, isnull(PS.Qty, 0)), 2)
	,A.Result
	,IIF(A.Status='Confirmed',A.InspQty,NULL)[Inspected Qty]
	,IIF(A.Status='Confirmed',A.RejectQty,NULL)[Rejected Qty]
	,IIF(A.Status='Confirmed', DefectText.Val ,NULL)[Defect Type]
	,IIF(A.Status='Confirmed',A.InspDate,NULL)[Inspection Date]
	,a.Remark
	,AIRL_Encode.OvenEncode
	,AIRL.NonOven
	,AIRL.Oven
	,AIRL.OvenScale
	,AIRL.OvenDate
	,AIRL.NonWash
	,AIRL.Wash
	,AIRL.WashScale
	,AIRL.WashDate
from dbo.AIR A WITH (NOLOCK) 
inner join (
		select r.WhseArrival,r.InvNo,r.ExportId,r.Id,r.PoId,r.seq1,r.seq2,r.StockQty 
        from dbo.View_AllReceivingDetail r WITH (NOLOCK)   
		{0}
) t on t.PoId = A.POID and t.Seq1 = A.SEQ1 and t.Seq2 = A.SEQ2 AND T.ID=a.ReceivingID
inner join (
		select distinct id,O.factoryid,O.BrandID,O.StyleID,O.SeasonID,O.Category 
		from dbo.Orders o WITH (NOLOCK)  
		{1}
) x on x. id = A.POID
inner join dbo.PO_Supp P WITH (NOLOCK) on P.id = A.POID and P.SEQ1 = A.SEQ1 
inner join dbo.PO_Supp_Detail PS WITH (NOLOCK) on PS.ID = A.POID and PS.SEQ1 = A.SEQ1 and PS.SEQ2 = A.SEQ2
left join dbo.Color C WITH (NOLOCK) on C.ID = PS.ColorID and C.BrandId = x.BrandId
inner join supp s WITH (NOLOCK) on s.id = P.SuppID
left join fabric WITH (NOLOCK) on fabric.SCIRefno = PS.scirefno
OUTER APPLY (
	SELECT  [Val]=  STUFF((
	SELECT ', '+ IIF(a.Defect = '' , '' ,ori.Data +'-'+ ISNULL(ad.Description,''))
	FROM [SplitString](a.Defect,'+') ori
	LEFT JOIN AccessoryDefect ad  WITH (NOLOCK) on ad.id = ori.Data
	 FOR XML PATH('')
	 ),1,1,'')
)DefectText
OUTER APPLY ( select * from dbo.AIR_Laboratory AL WITH (NOLOCK) where AL.OvenEncode = 1 and AL.ID = A.ID)AIRL
OUTER APPLY ( select [OvenEncode]='Y' from dbo.AIR_Laboratory AL WITH (NOLOCK) where AL.ID = A.ID and NonOven =1 and NonWash =1)AIRL_Encode                
OUTER APPLY ( select name from dbo.color c where c.id=ps.colorid_old and C.BrandId = x.BrandId) as oc
OUTER APPLY (
	select [Article] = Stuff((
		select distinct concat( ',', tcd.Article) 
		from Style s WITH (NOLOCK)
		Inner Join Style_ThreadColorCombo as tc WITH (NOLOCK) On tc.StyleUkey = s.Ukey
		Inner Join Style_ThreadColorCombo_Detail as tcd WITH (NOLOCK) On tcd.Style_ThreadColorComboUkey = tc.Ukey
		where s.ID = x.StyleID
        and tcd.SuppId = P.SuppId
        and tcd.SCIRefNo = PS.SCIRefNo
        and tcd.ColorID = PS.ColorID
        and PS.SEQ1 like 'T%' 
        and exists (select 1 from MDivisionPoDetail m WITH (NOLOCK) where m.POID = x.ID)
	FOR XML PATH('')),1,1,'') 
)Style
{2}",
                rWhere,
                oWhere,
                sqlWhere);
            #endregion
            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            DualResult res;
            res = DBProxy.Current.Select(string.Empty, this.cmd, this.lis, out this.dt);
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

            var saveDialog = Utility.Excel.MyExcelPrg.GetSaveFileDialog(Utility.Excel.MyExcelPrg.Filter_Excel);

            // saveDialog.ShowDialog();
            // string outpath = saveDialog.FileName;
            // if (outpath.Empty())
            // {
            //    return false;
            // }
            Utility.Excel.SaveXltReportCls xl = new Utility.Excel.SaveXltReportCls("Quality_R02.xltx", keepApp: true);

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
            xl.DicDatas.Add("##body", this.dt);

            xl.Save(Class.MicrosoftFile.GetName("Quality_R02"), false);
            ((Microsoft.Office.Interop.Excel.Worksheet)xl.ExcelApp.ActiveSheet).Columns.AutoFit();
            xl.FinishSave();
            return true;
        }
    }
}
