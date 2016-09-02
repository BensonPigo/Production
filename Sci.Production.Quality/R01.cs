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
        DateTime? LastDate1; DateTime? LastDate2;
        DateTime? ArrDate1;  DateTime? ArrDate2;
        DateTime? SCIDate1;  DateTime? SCIDate2;
        DateTime? SewDate1;  DateTime? SewDate2;
        DateTime? Est1;      DateTime? Est2;
        string sp1; string sp2; string Sea; string Brand; string Ref; string Cate; string Supp; string Over;
        List<SqlParameter> lis;
        DataTable dt; string cmd;
        public R01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            DataTable ORS = null;
            string sqlm = (@" select distinct case Category
                              when 'B' then 'Bulk'
                              WHEN 'S' THEN 'Sample'
                              WHEN 'M' THEN 'Material'
                              end [Category]
                            from Orders WHERE Category!='O'");
            DBProxy.Current.Select("", sqlm, out ORS);
            ORS.Rows.Add(new string[] { "" });
            ORS.DefaultView.Sort = "Category";
            this.Cate_comboBox.DataSource = ORS;
            this.Cate_comboBox.ValueMember = "Category";
            this.Cate_comboBox.DisplayMember = "Category";
            this.Cate_comboBox.SelectedIndex = 1;
            this.Over_comboBox.SelectedIndex = 0;
            
            print.Enabled = false;
        }
        protected override bool ValidateInput()
        {
            bool dateRange1_Empty = !this.Last_dateRange.HasValue, dateRange2_Empty = !this.Arr_dateRange.HasValue, dateRange3_Empty = !this.SCI_dateRange.HasValue, dateRange4_Empty = !this.Sew_dateRange.HasValue, dateRange5_Empty = !this.Est_dateRange.HasValue,
                textbox1_Empty = this.SP_textBox1.Text.Empty(), textbox2_Empty = this.SP_textBox2.Text.Empty(), txtSEA_Empty = this.txtSeason1.Text.Empty()
           , txtBrand_Empty = this.txtBrand1.Text.Empty(), txtRef_Empty = this.Ref_textBox.Text.Empty(), Cate_comboBox_Empty = this.Cate_comboBox.Text.Empty(), Supp_Empty = this.txtSupplier1.Text.Empty(), Over_comboBox_Empty = this.Over_comboBox.Text.Empty();

            if (dateRange1_Empty && dateRange2_Empty && dateRange3_Empty && textbox1_Empty && textbox2_Empty && dateRange4_Empty && dateRange5_Empty && textbox1_Empty && textbox2_Empty)
            {
                MyUtility.Msg.ErrorBox("Please select 'Last Inspection Date' or 'Arrive W/H Date' or 'SCI Delivery' or 'Sewing in-line Date' or 'Est. Cutting Date' or 'SP#'  at least one field entry");

                Arr_dateRange.Focus();

                return false;
            }

            LastDate1 = Last_dateRange.Value1;
            LastDate2 = Last_dateRange.Value2;
            ArrDate1 = Arr_dateRange.Value1;
            ArrDate2 = Arr_dateRange.Value2;
            SCIDate1 = SCI_dateRange.Value1;
            SCIDate2 = SCI_dateRange.Value2;
            SewDate1 = Sew_dateRange.Value1;
            SewDate2 = Sew_dateRange.Value2;
            Est1 = Est_dateRange.Value1;
            Est2 = Est_dateRange.Value2;
            sp1 = SP_textBox1.Text.ToString();
            sp2 = SP_textBox2.Text.ToString();
            Sea = txtSeason1.Text;
            Brand = txtBrand1.Text;
            Ref = Ref_textBox.Text.ToString();
            Cate = Cate_comboBox.SelectedItem.ToString();
            Supp = txtSupplier1.Text;
            Over = Over_comboBox.SelectedItem.ToString();

            lis = new List<SqlParameter>();
            string sqlWhere = ""; 
            List<string> sqlWheres = new List<string>();
            #region --組WHERE--

            if (!this.Last_dateRange.Value1.Empty() && !this.Last_dateRange.Value2.Empty())
            {
                sqlWheres.Add("F.PhysicalDate between @LastDate1 and @LastDate2");
                lis.Add(new SqlParameter("@LastDate1", LastDate1));
                lis.Add(new SqlParameter("@LastDate2", LastDate2));
            } if (!this.Arr_dateRange.Value1.Empty() && !this.Arr_dateRange.Value2.Empty())
            {
                sqlWheres.Add("R.WhseArrival between @ArrDate1 and @ArrDate2");
                lis.Add(new SqlParameter("@ArrDate1", ArrDate1));
                lis.Add(new SqlParameter("@ArrDate2", ArrDate2));
            } if (!this.SCI_dateRange.Value1.Empty() && !this.SCI_dateRange.Value2.Empty())
            {
                sqlWheres.Add("O.SciDelivery between @SCIDate1 and @SCIDate2");
                lis.Add(new SqlParameter("@SCIDate1", SCIDate1));
                lis.Add(new SqlParameter("@SCIDate2", SCIDate2));
            } if (!this.Sew_dateRange.Value1.Empty() && !this.Sew_dateRange.Value2.Empty())
            {
                sqlWheres.Add("O.SewInLine between @SewDate1 and @SewDate2");
                lis.Add(new SqlParameter("@SewDate1", SewDate1));
                lis.Add(new SqlParameter("@SewDate2", SewDate2));
            } if (!this.Est_dateRange.Value1.Empty() && !this.Est_dateRange.Value2.Empty())
            {
                sqlWheres.Add("O.CutInLine between @Est1 and @Est2");
                lis.Add(new SqlParameter("@Est1", Est1));
                lis.Add(new SqlParameter("@Est2", Est2));
            } if (!this.SP_textBox1.Text.Empty())
            {
                sqlWheres.Add("O.Id between @sp1 and @sp2");
                lis.Add(new SqlParameter("@sp1", sp1));
                lis.Add(new SqlParameter("@sp2", sp2));
            } if (!this.txtSeason1.Text.Empty())
            {
                sqlWheres.Add("O.SeasonID = @Sea");
                lis.Add(new SqlParameter("@Sea", Sea));
            } if (!this.txtBrand1.Text.Empty())
            {
                sqlWheres.Add("O.BrandID = @Brand");
                lis.Add(new SqlParameter("@Brand", Brand));
            } if (!this.Ref_textBox.Text.Empty())
            {
                sqlWheres.Add("P.Refno = @Ref");
                lis.Add(new SqlParameter("@Ref", Ref));
            } if (!this.Cate_comboBox.SelectedItem.ToString().Empty())
            {
                sqlWheres.Add("O.Category = @Cate");
                lis.Add(new SqlParameter("@Cate", Cate));
            } if (this.txtSupplier1.Text.Empty())
            {
                lis.Add(new SqlParameter("@Supp", Supp));

            } if (this.Over_comboBox.Text == "Pass")
            {
                sqlWheres.Add("dbo.GetFirResult(fir.id) = 'Pass'");

            } if (this.Over_comboBox.Text == "Fail")
            {
                sqlWheres.Add("dbo.GetFirResult(fir.id) = 'Faill'");

            } if (this.Over_comboBox.Text == "Empty Result")
            {
                sqlWheres.Add("dbo.GetFirResult(fir.id) = ' '");

            } if (this.Over_comboBox.Text == "N/A inspection & test")
            {
                sqlWheres.Add("dbo.GetFirResult(fir.id) = 'None'");

            }
            
            #endregion
            sqlWhere = string.Join(" and ", sqlWheres);
            if (!sqlWhere.Empty())
            {
                sqlWhere = " where " + sqlWhere;
            }
            #region --撈ListExcel資料--

            cmd = string.Format(@"
                SELECT F.POID,(F.SEQ1+'-'+F.SEQ2)SEQ,O.factoryid,O.BrandID,O.StyleID,O.SeasonID,
	                   R.ExportId,R.InvNo,R.WhseArrival,rd.StockQty,
	                   (SELECT MinSciDelivery FROM  DBO.GetSCI(F.Poid,O.Category))[MinSciDelivery],
	                   (SELECT MinBuyerDelivery  FROM  DBO.GetSCI(F.Poid,O.Category))[MinBuyerDelivery],
	                   F.Refno,P.ColorID,(SP.SuppID+'-'+s.AbbEN)Supplier,C.WeaveTypeID,
	                   IIF(F.Nonphysical = 1,'Y',' ')[N/A Physical],F.Result,F.Physical,F.PhysicalDate,
	                   F.TotalInspYds,F.Weight,F.WeightDate,F.ShadeBond,F.ShadeBondDate,F.Continuity,
	                   F.ContinuityDate,L.Result,IIF(L.nonCrocking=1,'Y',' ')[N/A Crocking],L.Crocking,
	                   L.CrockingDate,IIF(L.nonHeat=1,'Y',' ')[N/A Heat Shrinkage],L.Heat,L.HeatDate,
	                   IIF(L.nonWash=1,'Y',' ' )[N/A Wash Shrinkage],L.Wash,L.WashDate,OD.Result,CD.Result
                FROM   FIR F
                LEFT JOIN Orders O ON O.POID = F.POID
                LEFT JOIN Receiving_Detail RD on RD.Id = F.Id
                LEFT JOIN Receiving R ON  R.ID = RD.Id
                LEFT JOIN  PO_Supp_Detail P ON P.ID=F.POID and P.seq1 = F.SEQ1 and P.seq2 = F.Seq2
                inner join PO_Supp SP on SP.id = P.ID 
                inner join supp s on s.id = SP.SuppID AND SP.id = F.Poid and SP.seq1 = F.Seq1
                LEFT JOIN Fabric C ON C.SCIRefno = F.SCIRefno
                LEFT JOIN FIR_Laboratory L ON L.CrockingEncode=1 AND L.ID = F.ID AND L.SEQ1 = F.SEQ1 AND L.SEQ2 = F.SEQ2
                LEFT JOIN Oven OV ON OV.POID = F.POID
                INNER JOIN Oven_Detail OD ON OD.ID= OV.ID AND OD.SEQ1 = F.SEQ1 AND OD.SEQ2 = F.SEQ2 AND OV.Status ='Confirmed'
                INNER JOIN ColorFastness CF ON CF.Status = 'Confirmed'  AND CF.POID = F.POID 
                INNER JOIN ColorFastness_Detail CD ON CD.ID = CF.ID AND CD.SEQ1=F.SEQ1 AND CD.SEQ2 = F.SEQ2" + sqlWhere );
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

            Sci.Utility.Excel.SaveXltReportCls xl = new Sci.Utility.Excel.SaveXltReportCls("Quality_R01.xltx");

            string d1 = (MyUtility.Check.Empty(ArrDate1)) ? "" : Convert.ToDateTime(ArrDate1).ToString("yyyy/MM/dd");
            string d2 = (MyUtility.Check.Empty(ArrDate2)) ? "" : Convert.ToDateTime(ArrDate2).ToString("yyyy/MM/dd");
            string d3 = (MyUtility.Check.Empty(SCIDate1)) ? "" : Convert.ToDateTime(SCIDate1).ToString("yyyy/MM/dd");
            string d4 = (MyUtility.Check.Empty(SCIDate2)) ? "" : Convert.ToDateTime(SCIDate2).ToString("yyyy/MM/dd");
            string d5 = (MyUtility.Check.Empty(SewDate1)) ? "" : Convert.ToDateTime(SewDate1).ToString("yyyy/MM/dd");
            string d6 = (MyUtility.Check.Empty(SewDate2)) ? "" : Convert.ToDateTime(SewDate2).ToString("yyyy/MM/dd");
            string d7 = (MyUtility.Check.Empty(Est1)) ? "" : Convert.ToDateTime(Est1).ToString("yyyy/MM/dd");
            string d8 = (MyUtility.Check.Empty(Est2)) ? "" : Convert.ToDateTime(Est2).ToString("yyyy/MM/dd");
            xl.dicDatas.Add("##Arr", d1 + "~" + d2);
            xl.dicDatas.Add("##SCI", d3 + "~" + d4);
            xl.dicDatas.Add("##Sew", d5 + "~" + d6);
            xl.dicDatas.Add("##Est", d7 + "~" + d8);
            xl.dicDatas.Add("##SP", sp1 + "~" + sp2);
            xl.dicDatas.Add("##Sea", Sea);
            xl.dicDatas.Add("##Brand", Brand);
            xl.dicDatas.Add("##Ref", Ref);
            xl.dicDatas.Add("##Cate", Cate);
            xl.dicDatas.Add("##supp", Supp);
            xl.dicDatas.Add("##Over", Over);
            xl.dicDatas.Add("##body", dt);
            xl.Save(outpath, false);
            return true;
            
        }

       
    }
}
