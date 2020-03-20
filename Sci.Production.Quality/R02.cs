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
    public partial class R02 : Sci.Win.Tems.PrintForm
    {
        DateTime? DateArrStart; DateTime? DateArrEnd;
        DateTime? DateSCIStart; DateTime? DateSCIEnd;
        DateTime? DateSewStart; DateTime? DateSewEnd;
        DateTime? DateEstStart; DateTime? DateEstEnd;
        string spStrat; string spEnd; string Sea; string Brand; string Ref; string Category; string Supp; string Over;
        List<SqlParameter> lis;
        DataTable dt; string cmd;

        public R02(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.comboOverallResultStatus.SelectedIndex = 0;

            print.Enabled = false;
        }

        protected override bool ValidateInput()
        {
            bool date_Arrive_Empty = !this.dateArriveWHDate.HasValue, date_SCI_Empty = !this.dateSCIDelivery.HasValue, date_Sewing_Empty = !this.dateSewingInLineDate.HasValue, date_Est_Empty = !this.dateEstCuttingDate.HasValue,
               textBox_SP_Empty = this.txtSPStart.Text.Empty(), textBox_SP2_Empty = this.txtSPEnd.Text.Empty(), txtSEA_Empty = this.txtSeason.Text.Empty()
          , txtBrand_Empty = this.txtBrand.Text.Empty(), txtRef_Empty = this.txtRefno.Text.Empty(), Cate_comboBox_Empty = this.comboCategory.Text.Empty(), Supp_Empty = !this.txtsupplier.Text.Empty(), Over_comboBox_Empty = this.comboOverallResultStatus.Text.Empty();

            if (date_Arrive_Empty && date_SCI_Empty && date_Sewing_Empty && date_Est_Empty && textBox_SP_Empty && textBox_SP2_Empty)
            {
                dateArriveWHDate.Focus();
                MyUtility.Msg.ErrorBox("Please select 'Arrive W/H Date' or 'SCI Delivery' or 'Sewing in-line Date' or 'Est. Cutting Date' or 'SP#'  at least one field entry");
                return false;
            }

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
            Sea = txtSeason.Text;
            Brand = txtBrand.Text;
            Ref = txtRefno.Text.ToString();
            Category = comboCategory.Text;
            Supp = txtsupplier.TextBox1.Text;
            Over = comboOverallResultStatus.SelectedItem.ToString();

            lis = new List<SqlParameter>();
            string sqlWhere = "", RWhere = "", OWhere = "";
            List<string> sqlWheres = new List<string>();
            List<string> RWheres = new List<string>();
            List<string> OWheres = new List<string>();
            #region --組WHERE--

             if (!this.dateArriveWHDate.Value1.Empty())
             {
                 RWheres.Add("R.WhseArrival >= @ArrDate1");
                 lis.Add(new SqlParameter("@ArrDate1", DateArrStart));
             }
             if (!this.dateArriveWHDate.Value2.Empty())
             {
                 RWheres.Add("R.WhseArrival <= @ArrDate2");
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

            if (!this.dateEstCuttingDate.Value1.Empty() && !this.dateEstCuttingDate.Value2.Empty())
            {
                OWheres.Add("O.CutInLine between @Est1 and @Est2");
                lis.Add(new SqlParameter("@Est1", DateEstStart));
                lis.Add(new SqlParameter("@Est2", DateEstEnd));
            } 
            if (!this.txtSPStart.Text.Empty())
            {
                OWheres.Add("O.Id between @sp1 and @sp2");
                lis.Add(new SqlParameter("@sp1", spStrat));
                lis.Add(new SqlParameter("@sp2", spEnd));
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
                sqlWheres.Add("PS.Refno = @Ref");
                lis.Add(new SqlParameter("@Ref", Ref));
            } 
            if (!MyUtility.Check.Empty(this.comboCategory.Text))
            {
                OWheres.Add($"O.Category in ({this.comboCategory.SelectedValue})");
            }
            if (!this.txtsupplier.TextBox1.Text.Empty())
            {
                sqlWheres.Add("P.SuppId = @Supp");
                lis.Add(new SqlParameter("@Supp", Supp));

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
            OWhere = string.Join(" and ", OWheres);
            RWhere = string.Join(" and ", RWheres);
            if (sqlWheres.Count!=0)
            {
                sqlWhere = " where " + sqlWhere;
            }
            if (RWheres.Count!=0)
            {
                RWhere = " where " + RWhere;
            }
            if (OWheres.Count!=0)
            {
                OWhere = " where " + OWhere;
            }
            #region --撈ListExcel資料--

            cmd = string.Format(@"select A.POID,(A.seq1+'-'+A.seq2)SEQ,x.FactoryID,x.BrandID,x.StyleID,x.SeasonID,t.ExportId,t.InvNo,t.WhseArrival,
t.StockQty,
(Select MinSciDelivery from DBO.GetSCI(A.poid,x.Category))[MinSciDelivery],
(Select MinBuyerDelivery from DBO.GetSCI(A.poid,x.Category))[MinBuyerDelivery],
A.refno,
iif(C.Name is null,oc.name,c.name ) name,
PS.SizeSpec,
	                   PS.stockunit,(P.SuppID+'-'+s.AbbEN)Supplier,A.Result
	                   ,IIF(A.Status='Confirmed',A.InspQty,NULL)[Inspected Qty]
	                   ,IIF(A.Status='Confirmed',A.RejectQty,NULL)[Rejected Qty]
,IIF(A.Status='Confirmed', DefectText.Val ,NULL)[Defect Type]
	                   ,IIF(A.Status='Confirmed',A.InspDate,NULL)[Inspection Date],a.Remark
	                   ,AIRL_Encode.OvenEncode
,AIRL.NonOven,AIRL.Oven,AIRL.OvenScale,AIRL.OvenDate,AIRL.NonWash,AIRL.Wash,AIRL.WashScale,
	                   AIRL.WashDate
                from dbo.AIR A WITH (NOLOCK) 
                inner join (select r.WhseArrival,r.InvNo,r.ExportId,r.Id,rd.PoId,rd.seq1,rd.seq2,RD.StockQty from dbo.Receiving r WITH (NOLOCK) 
			                inner join dbo.Receiving_Detail rd WITH (NOLOCK) on rd.Id = r.Id "
                 + RWhere + @"
			                ) t
                on t.PoId = A.POID and t.Seq1 = A.SEQ1 and t.Seq2 = A.SEQ2 AND T.ID=a.ReceivingID
                inner join (select distinct id,O.factoryid,O.BrandID,O.StyleID,O.SeasonID,O.Category from dbo.Orders o WITH (NOLOCK) "
                 + OWhere + @"
			                 ) x on x. id = A.POID
                inner join dbo.PO_Supp P WITH (NOLOCK) on P.id = A.POID and P.SEQ1 = A.SEQ1 
                inner join dbo.PO_Supp_Detail PS WITH (NOLOCK) on PS.ID = A.POID and PS.SEQ1 = A.SEQ1 and PS.SEQ2 = A.SEQ2
                left join dbo.Color C WITH (NOLOCK) on C.ID = PS.ColorID and C.BrandId = x.BrandId
                inner join supp s WITH (NOLOCK) on s.id = P.SuppID
OUTER APPLY(
	SELECT  [Val]=  STUFF((
	SELECT ', '+ IIF(a.Defect = '' , '' ,ori.Data +'-'+ ISNULL(ad.Description,''))
	FROM [SplitString](a.Defect,'+') ori
	LEFT JOIN AccessoryDefect ad  WITH (NOLOCK) on ad.id = ori.Data
	 FOR XML PATH('')
	 ),1,1,'')

)DefectText
                OUTER APPLY(select * from dbo.AIR_Laboratory AL WITH (NOLOCK) where AL.OvenEncode = 1 and AL.ID = A.ID)AIRL
OUTER APPLY(select [OvenEncode]='Y' from dbo.AIR_Laboratory AL WITH (NOLOCK) where AL.ID = A.ID and NonOven =1 and NonWash =1)AIRL_Encode                
        outer apply (select name from dbo.color c where c.id=ps.colorid_old and C.BrandId = x.BrandId) as oc
               " + sqlWhere);
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
            //saveDialog.ShowDialog();
            //string outpath = saveDialog.FileName;
            //if (outpath.Empty())
            //{
            //    return false;
            //}

            Sci.Utility.Excel.SaveXltReportCls xl = new Sci.Utility.Excel.SaveXltReportCls("Quality_R02.xltx", keepApp: true);

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
            xl.DicDatas.Add("##body", dt);

            xl.Save(Sci.Production.Class.MicrosoftFile.GetName("Quality_R02"), false);
            ((Microsoft.Office.Interop.Excel.Worksheet)xl.ExcelApp.ActiveSheet).Columns.AutoFit();
            xl.FinishSave();
            return true;

        }
    }
}
