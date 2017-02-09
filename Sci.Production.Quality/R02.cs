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
            DataTable ORS = null;
            string sqlm = (@" 
                        select
                             Category=name
                        from  dbo.DropDownList
                        where type = 'Category' and id != 'O'
                        ");
            DBProxy.Current.Select("", sqlm, out ORS);
            ORS.Rows.Add(new string[] { "" });
            ORS.DefaultView.Sort = "Category";
            this.comboCategory.DataSource = ORS;
            this.comboCategory.ValueMember = "Category";
            this.comboCategory.DisplayMember = "Category";
            this.comboCategory.SelectedIndex = 1;
            this.comboOverallResSta.SelectedIndex = 0;

            print.Enabled = false;
        }
        protected override bool ValidateInput()
        {
            bool date_Arrive_Empty = !this.DateArriveWH.HasValue, date_SCI_Empty = !this.DateSCIDelivery.HasValue, date_Sewing_Empty = !this.DateSewInLine.HasValue, date_Est_Empty = !this.DateEstCutting.HasValue,
               textBox_SP_Empty = this.txtSPStart.Text.Empty(), textBox_SP2_Empty = this.txtSPEnd.Text.Empty(), txtSEA_Empty = this.txtSeason.Text.Empty()
          , txtBrand_Empty = this.txtBrand.Text.Empty(), txtRef_Empty = this.txtRefno.Text.Empty(), Cate_comboBox_Empty = this.comboCategory.Text.Empty(), Supp_Empty = !this.txtsupplier.Text.Empty(), Over_comboBox_Empty = this.comboOverallResSta.Text.Empty();

            if (date_Arrive_Empty && date_SCI_Empty && date_Sewing_Empty && date_Est_Empty && textBox_SP_Empty && textBox_SP2_Empty)
            {
                MyUtility.Msg.ErrorBox("Please select 'Arrive W/H Date' or 'SCI Delivery' or 'Sewing in-line Date' or 'Est. Cutting Date' or 'SP#'  at least one field entry");

                DateArriveWH.Focus();

                return false;
            }

            DateArrStart = DateArriveWH.Value1;
            DateArrEnd = DateArriveWH.Value2;
            DateSCIStart = DateSCIDelivery.Value1;
            DateSCIEnd = DateSCIDelivery.Value2;
            DateSewStart = DateSewInLine.Value1;
            DateSewEnd = DateSewInLine.Value2;
            DateEstStart = DateEstCutting.Value1;
            DateEstEnd = DateEstCutting.Value2;
            spStrat = txtSPStart.Text.ToString();
            spEnd = txtSPEnd.Text.ToString();
            Sea = txtSeason.Text;
            Brand = txtBrand.Text;
            Ref = txtRefno.Text.ToString();
            Category = comboCategory.Text;
            Supp = txtsupplier.Text;
            Over = comboOverallResSta.SelectedItem.ToString();

            lis = new List<SqlParameter>();
            string sqlWhere = "", RWhere = "", OWhere = "";
            List<string> sqlWheres = new List<string>();
            List<string> RWheres = new List<string>();
            List<string> OWheres = new List<string>();
            #region --組WHERE--

             if (!this.DateArriveWH.Value1.Empty() && !this.DateArriveWH.Value2.Empty())
            {
                RWheres.Add("R.WhseArrival between @ArrDate1 and @ArrDate2");
                lis.Add(new SqlParameter("@ArrDate1", DateArrStart));
                lis.Add(new SqlParameter("@ArrDate2", DateArrEnd));
            } if (!this.DateSCIDelivery.Value1.Empty() && !this.DateSCIDelivery.Value2.Empty())
            {
                OWheres.Add("O.SciDelivery between @SCIDate1 and @SCIDate2");
                lis.Add(new SqlParameter("@SCIDate1", DateSCIStart));
                lis.Add(new SqlParameter("@SCIDate2", DateSCIEnd));
            } if (!this.DateSewInLine.Value1.Empty() && !this.DateSewInLine.Value2.Empty())
            {
                OWheres.Add("O.SewInLine between @SewDate1 and @SewDate2");
                lis.Add(new SqlParameter("@SewDate1", DateSewStart));
                lis.Add(new SqlParameter("@SewDate2", DateSewEnd));
            } if (!this.DateEstCutting.Value1.Empty() && !this.DateEstCutting.Value2.Empty())
            {
                OWheres.Add("O.CutInLine between @Est1 and @Est2");
                lis.Add(new SqlParameter("@Est1", DateEstStart));
                lis.Add(new SqlParameter("@Est2", DateEstEnd));
            } if (!this.txtSPStart.Text.Empty())
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
                sqlWheres.Add("PS.Refno = @Ref");
                lis.Add(new SqlParameter("@Ref", Ref));
            } if (!this.comboCategory.SelectedItem.ToString().Empty())
            {
                OWheres.Add("O.Category = @Cate");
                if (Category == "Bulk")
                {
                    lis.Add(new SqlParameter("@Cate", "B"));
                }
                if (Category == "Sample")
                {
                    lis.Add(new SqlParameter("@Cate", "S"));
                }
                if (Category == "Material")
                {
                    lis.Add(new SqlParameter("@Cate", "M"));
                }  
            } if (!this.txtsupplier.Text.Empty())
            {
                sqlWheres.Add("P.SuppId = @Supp");
                lis.Add(new SqlParameter("@Supp", Supp));

            } if (this.comboOverallResSta.Text == "Pass")
            {
                sqlWheres.Add("dbo.GetFirResult(A.id) = 'Pass'");

            } if (this.comboOverallResSta.Text == "Fail")
            {
                sqlWheres.Add("dbo.GetFirResult(A.id) = 'Faill'");

            } if (this.comboOverallResSta.Text == "Empty Result")
            {
                sqlWheres.Add("dbo.GetFirResult(A.id) = ' '");

            } if (this.comboOverallResSta.Text == "N/A inspection & test")
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
            if (!RWheres.Empty())
            {
                RWhere = " where " + RWhere;
            }
            if (!OWheres.Empty())
            {
                OWhere = " where " + OWhere;
            }
            #region --撈ListExcel資料--

            cmd = string.Format(@"select A.POID,(A.seq1+'-'+A.seq2)SEQ,x.FactoryID,x.BrandID,x.StyleID,x.SeasonID,t.ExportId,t.InvNo,t.WhseArrival,
                       t.StockQty,
	                   (Select MinSciDelivery from DBO.GetSCI(A.Poid,x.Category))[MinSciDelivery],
	                   (Select MinBuyerDelivery from DBO.GetSCI(A.Poid,x.Category))[MinBuyerDelivery],A.refno,C.Name,PS.SizeSpec,
	                   PS.stockunit,(P.SuppID+'-'+s.AbbEN)Supplier,A.Result
	                   ,IIF(A.Status='Confirme',A.InspQty,NULL)[Inspected Qty]
	                   ,IIF(A.Status='Confirme',A.RejectQty,NULL)[Rejected Qty]
	                   ,IIF(A.Status='Confirme',A.Defect,NULL)[Defect Type]
	                   ,IIF(A.Status='Confirme',A.InspDate,NULL)[Inspection Date]
	                   ,AIRL.Result,AIRL.NonOven,AIRL.Oven,AIRL.OvenScale,AIRL.OvenDate,AIRL.NonWash,AIRL.Wash,AIRL.WashScale,
	                   AIRL.WashDate
                from dbo.AIR A
                inner join (select distinct r.WhseArrival,r.InvNo,r.ExportId,r.Id,rd.PoId,rd.seq1,rd.seq2,RD.StockQty from dbo.Receiving r
			                inner join dbo.Receiving_Detail rd on rd.Id = r.Id "
			     + RWhere + @"
			                ) t
                on t.PoId = A.POID and t.Seq1 = A.SEQ1 and t.Seq2 = A.SEQ2
                inner join (select distinct O.POID,O.Factoryid,O.BrandId,O.StyleID,O.SeasonId,O.Category from dbo.Orders o "
                 + OWhere + @"
			                 ) x on x. poid = A.POID
                inner join dbo.PO_Supp P on P.id = A.POID and P.SEQ1 = A.SEQ1 
                inner join dbo.PO_Supp_Detail PS on PS.ID = A.POID and PS.SEQ1 = A.SEQ1 and PS.SEQ2 = A.SEQ2
                INNER join dbo.Color C on C.ID = PS.ColorID and C.BrandId = PS.BrandId
                inner join supp s on s.id = P.SuppID
                OUTER APPLY(select * from dbo.AIR_Laboratory AL where AL.OvenEncode = 1 and AL.ID = A.ID)AIRL
               " +sqlWhere);
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
            if (dt == null || dt.Rows.Count == 0)
            {
                MyUtility.Msg.ErrorBox("Data not found");
                return false;
            }
            var saveDialog = Sci.Utility.Excel.MyExcelPrg.GetSaveFileDialog(Sci.Utility.Excel.MyExcelPrg.filter_Excel);
            saveDialog.ShowDialog();
            string outpath = saveDialog.FileName;
            if (outpath.Empty())
            {
                return false;
            }

            Sci.Utility.Excel.SaveXltReportCls xl = new Sci.Utility.Excel.SaveXltReportCls("Quality_R02.xltx");

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
            xl.dicDatas.Add("##body", dt);
            xl.Save(outpath, false);
            return true;
            
        }
    }
}
