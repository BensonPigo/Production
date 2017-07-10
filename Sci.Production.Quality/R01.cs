using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
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
        List<SqlParameter> lis;
        DataTable dt; string cmd;
        public R01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            DataTable ORS = null;
            string sqlm = (@" 
                        select
                             Category=name
                        from  dbo.DropDownList WITH (NOLOCK) 
                        where type = 'Category' and id != 'O'
                        ");
            DBProxy.Current.Select("", sqlm, out ORS);
            ORS.Rows.Add(new string[] { "" });
            ORS.DefaultView.Sort = "Category";
            this.comboCategory.DataSource = ORS;
            this.comboCategory.ValueMember = "Category";
            this.comboCategory.DisplayMember = "Category";
            this.comboCategory.SelectedIndex = 1;
            this.comboOverallResultStatus.SelectedIndex = 0;
            
            print.Enabled = false;
        }
        protected override bool ValidateInput()
        {
            bool date_Last_Empty = !this.dateLastPhysicalInspDate.HasValue, date_Arrive_Empty = !this.dateArriveWHDate.HasValue, date_SCI_Empty = !this.dateSCIDelivery.HasValue, date_Sewing_Empty = !this.dateSewingInLineDate.HasValue, date_Est_Empty = !this.dateEstCuttingDate.HasValue,
                textBox_SP_Empty = this.txtSPStart.Text.Empty(), textBox_SP2_Empty = this.txtSPEnd.Text.Empty(), txtSeason_Empty = this.txtSeason.Text.Empty()
           , txtBrand_Empty = this.txtBrand.Text.Empty(), txtRef_Empty = this.txtRefno.Text.Empty(), Cate_comboBox_Empty = this.comboCategory.Text.Empty(), Supp_Empty = this.txtsupplier.Text.Empty(), Over_comboBox_Empty = this.comboOverallResultStatus.Text.Empty();

            if (date_Last_Empty && date_Arrive_Empty && date_SCI_Empty && textBox_SP_Empty && textBox_SP2_Empty && date_Sewing_Empty && date_Est_Empty )
            {
                dateArriveWHDate.Focus();
                MyUtility.Msg.ErrorBox("Please select 'Last Inspection Date' or 'Arrive W/H Date' or 'SCI Delivery' or 'Sewing in-line Date' or 'Est. Cutting Date' or 'SP#'  at least one field entry");
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
            Sea = txtSeason.Text;
            Brand = txtBrand.Text;
            Ref = txtRefno.Text.ToString();
            Category = comboCategory.Text;
            Supp = txtsupplier.Text;
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
            } if (!this.txtSeason.Text.Empty())
            {
                OWheres.Add("O.SeasonID = @Sea");
                lis.Add(new SqlParameter("@Sea", Sea));
            } if (!this.txtBrand.Text.Empty())
            {
                OWheres.Add("O.BrandID = @Brand");
                lis.Add(new SqlParameter("@Brand", Brand));
            } if (!this.txtRefno.Text.Empty())
            {
                sqlWheres.Add("P.Refno = @Ref");
                lis.Add(new SqlParameter("@Ref", Ref));
            } if (!this.comboCategory.SelectedItem.ToString().Empty())
            {
                if (Category != "")
                {
                    OWheres.Add("O.Category = @Cate");
                    if (Category == "Bulk")
                    {
                        lis.Add(new SqlParameter("@Cate", "B"));
                    }
                    else if (Category == "Sample")
                    {
                        lis.Add(new SqlParameter("@Cate", "S"));
                    }
                    else if (Category == "Material")
                    {
                        lis.Add(new SqlParameter("@Cate", "M"));
                    }
                    else
                    {
                        lis.Add(new SqlParameter("@Cate", "''"));
                    }
                }
            } if (!this.txtsupplier.Text.Empty())
            {
                sqlWheres.Add("SP.SuppId = @Supp");
                lis.Add(new SqlParameter("@Supp", Supp));

            } if (this.comboOverallResultStatus.Text == "Pass")
            {
                sqlWheres.Add("dbo.GetFirResult(F.id) = 'Pass'");

            } if (this.comboOverallResultStatus.Text == "Fail")
            {
                sqlWheres.Add("dbo.GetFirResult(F.id) = 'Faill'");

            } if (this.comboOverallResultStatus.Text == "Empty Result")
            {
                sqlWheres.Add("dbo.GetFirResult(F.id) = ' '");

            } if (this.comboOverallResultStatus.Text == "N/A inspection & test")
            {
                sqlWheres.Add("dbo.GetFirResult(F.id) = 'None'");

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

            cmd = string.Format(@"
    select  
	F.POID,(F.SEQ1+'-'+F.SEQ2)SEQ,O.factoryid,O.BrandID,O.StyleID,O.SeasonID,
	t.ExportId,t.InvNo,t.WhseArrival,
	SUM(t.StockQty) AS StockQty1,
	(SELECT MinSciDelivery FROM  DBO.GetSCI(F.Poid,O.Category))[MinSciDelivery],
	(SELECT MinBuyerDelivery  FROM  DBO.GetSCI(F.Poid,O.Category))[MinBuyerDelivery],
	F.Refno,P.ColorID,(SP.SuppID+'-'+s.AbbEN)Supplier,
	C.WeaveTypeID,
	IIF(F.Nonphysical = 1,'Y',' ')[N/A Physical],
	F.Result,
	F.Physical,
	F.PhysicalDate,
	F.TotalInspYds,
	F.Weight,
	F.WeightDate,
	F.ShadeBond,
	F.ShadeBondDate,
	F.Continuity,
	F.ContinuityDate,
	L.Result AS Result2,
	IIF(L.nonCrocking=1,'Y',' ')[N/A Crocking],
	LC.Crocking,	LC.CrockingDate,
	IIF(L.nonHeat=1,'Y',' ')[N/A Heat Shrinkage],
	LH.Heat,LH.HeatDate,
	IIF(L.nonWash=1,'Y',' ' )[N/A Wash Shrinkage],
	LW.Wash,LW.WashDate,
	V.Result AS RESULT3,
	CFD.Result AS RESULT4,
    ps1.LocalMR
from dbo.FIR F WITH (NOLOCK) 
    inner join (select R.WhseArrival,R.InvNo,R.ExportId,R.Id,rd.PoId,RD.seq1,RD.seq2,RD.StockQty
			    from dbo.Receiving R WITH (NOLOCK) 
			    inner join dbo.Receiving_Detail RD WITH (NOLOCK) on RD.Id = R.Id"
                + RWhere+ @" 
			    ) t
    on t.PoId = F.POID and t.Seq1 = F.SEQ1 and t.Seq2 = F.SEQ2 AND T.Id=F.ReceivingID
    inner join (select distinct poid,O.factoryid,O.BrandID,O.StyleID,O.SeasonID,O.Category,LocalMR from dbo.Orders o WITH (NOLOCK) "
                + OWhere+ @"
		        ) O on O.poid = F.POID
    inner join dbo.PO_Supp SP WITH (NOLOCK) on SP.id = F.POID and SP.SEQ1 = F.SEQ1
    inner join dbo.PO_Supp_Detail P WITH (NOLOCK) on P.ID = F.POID and P.SEQ1 = F.SEQ1 and P.SEQ2 = F.SEQ2
    inner join supp s WITH (NOLOCK) on s.id = SP.SuppID 
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
        select od.Result from dbo.Oven ov WITH (NOLOCK) 
        inner join dbo.Oven_Detail od WITH (NOLOCK) on od.ID = ov.ID
        where ov.POID=F.POID and od.SEQ1=F.Seq1 and seq2=F.Seq2 and ov.Status='Confirmed'
        )V
OUTER APPLY(
         select distinct cd.Result from dbo.ColorFastness CF WITH (NOLOCK) inner join dbo.ColorFastness_Detail cd WITH (NOLOCK) on cd.ID = CF.ID
        where CF.Status = 'Confirmed' and CF.POID=F.POID and cd.SEQ1=F.Seq1 and cd.seq2=F.Seq2
        )CFD
Outer apply(
	select (id+' - '+ name + ' #'+extno) LocalMR from Pass1 where id=o.LocalMR
) ps1 " + sqlWhere) + @" 
GROUP BY 
F.POID,F.SEQ1,F.SEQ2,O.factoryid,O.BrandID,O.StyleID,O.SeasonID,
t.ExportId,t.InvNo,t.WhseArrival,
F.Refno,P.ColorID,C.WeaveTypeID,O.Category
,F.Result,F.Physical,F.PhysicalDate,
F.TotalInspYds,F.Weight,F.WeightDate,F.ShadeBond,F.ShadeBondDate,F.Continuity,
F.ContinuityDate,L.Result,LC.Crocking,
LC.CrockingDate,LH.Heat,LH.HeatDate,
LW.Wash,LW.WashDate,V.Result,CFD.Result,SP.SuppID,S.AbbEN,F.Nonphysical,L.nonCrocking,L.nonHeat,L.nonWash,ps1.LocalMR
ORDER BY POID,SEQ";
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
            var saveDialog = Sci.Utility.Excel.MyExcelPrg.GetSaveFileDialog(Sci.Utility.Excel.MyExcelPrg.filter_Excel);

            Sci.Utility.Excel.SaveXltReportCls xl = new Sci.Utility.Excel.SaveXltReportCls("Quality_R01.xltx");
            Sci.Utility.Excel.SaveXltReportCls.xltRptTable dt1 = new Utility.Excel.SaveXltReportCls.xltRptTable(dt);
            string d1 = (MyUtility.Check.Empty(DateArrStart)) ? "" : Convert.ToDateTime(DateArrStart).ToString("yyyy/MM/dd");
            string d2 = (MyUtility.Check.Empty(DateArrEnd)) ? "" : Convert.ToDateTime(DateArrEnd).ToString("yyyy/MM/dd");
            string d3 = (MyUtility.Check.Empty(DateSCIStart)) ? "" : Convert.ToDateTime(DateSCIStart).ToString("yyyy/MM/dd");
            string d4 = (MyUtility.Check.Empty(DateSCIEnd)) ? "" : Convert.ToDateTime(DateSCIEnd).ToString("yyyy/MM/dd");
            string d5 = (MyUtility.Check.Empty(DateSewStart)) ? "" : Convert.ToDateTime(DateSewStart).ToString("yyyy/MM/dd");
            string d6 = (MyUtility.Check.Empty(DateSewEnd)) ? "" : Convert.ToDateTime(DateSewEnd).ToString("yyyy/MM/dd");
            string d7 = (MyUtility.Check.Empty(DateEstStart)) ? "" : Convert.ToDateTime(DateEstStart).ToString("yyyy/MM/dd");
            string d8 = (MyUtility.Check.Empty(DateEstEnd)) ? "" : Convert.ToDateTime(DateEstEnd).ToString("yyyy/MM/dd");
            xl.dicDatas.Add("##Arr", d1 + "~" + d2);
            xl.dicDatas.Add("##SCI", d3 + "~" + d4);
            xl.dicDatas.Add("##Sew", d5 + "~" + d6);
            xl.dicDatas.Add("##Est", d7 + "~" + d8);
            xl.dicDatas.Add("##SP", spStrat + "~" + spEnd);
            xl.dicDatas.Add("##Sea", Sea);
            xl.dicDatas.Add("##Brand", Brand);
            xl.dicDatas.Add("##Ref", Ref);
            xl.dicDatas.Add("##Cate", Category);
            xl.dicDatas.Add("##supp", Supp);
            xl.dicDatas.Add("##Over", Over);
            xl.dicDatas.Add("##body", dt1);
            Microsoft.Office.Interop.Excel.Worksheet wks = xl.ExcelApp.ActiveSheet;
            dt1.ShowHeader = false;
            xl.Save();
            wks.Columns.AutoFit();
            return true;
            
        }

       
    }
}
