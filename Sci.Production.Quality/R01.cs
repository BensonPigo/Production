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
            string sqlm = (@" 
                        select
                             Category=name
                        from  dbo.DropDownList
                        where type = 'Category' and id != 'O'
                        ");
            DBProxy.Current.Select("", sqlm, out ORS);
            ORS.Rows.Add(new string[] { "" });
            ORS.DefaultView.Sort = "Category";
            this.combobox_Cate.DataSource = ORS;
            this.combobox_Cate.ValueMember = "Category";
            this.combobox_Cate.DisplayMember = "Category";
            this.combobox_Cate.SelectedIndex = 1;
            this.comboBox_Over.SelectedIndex = 0;
            
            print.Enabled = false;
        }
        protected override bool ValidateInput()
        {
            bool date_Last_Empty = !this.date_Last.HasValue, date_Arrive_Empty = !this.date_Arrive.HasValue, date_SCI_Empty = !this.date_SCI.HasValue, date_Sewing_Empty = !this.date_Sewing.HasValue, date_Est_Empty = !this.date_Est.HasValue,
                textBox_SP_Empty = this.textBox_SP.Text.Empty(), textBox_SP2_Empty = this.textBox_SP2.Text.Empty(), txtSeason_Empty = this.txtSeason1.Text.Empty()
           , txtBrand_Empty = this.txtBrand1.Text.Empty(), txtRef_Empty = this.textBox_Ref.Text.Empty(), Cate_comboBox_Empty = this.combobox_Cate.Text.Empty(), Supp_Empty = this.txtsupplier1.Text.Empty(), Over_comboBox_Empty = this.comboBox_Over.Text.Empty();

            if (date_Last_Empty && date_Arrive_Empty && date_SCI_Empty && textBox_SP_Empty && textBox_SP2_Empty && date_Sewing_Empty && date_Est_Empty )
            {
                MyUtility.Msg.ErrorBox("Please select 'Last Inspection Date' or 'Arrive W/H Date' or 'SCI Delivery' or 'Sewing in-line Date' or 'Est. Cutting Date' or 'SP#'  at least one field entry");

                date_Arrive.Focus();

                return false;
            }

            LastDate1 = date_Last.Value1;
            LastDate2 = date_Last.Value2;
            ArrDate1 = date_Arrive.Value1;
            ArrDate2 = date_Arrive.Value2;
            SCIDate1 = date_SCI.Value1;
            SCIDate2 = date_SCI.Value2;
            SewDate1 = date_Sewing.Value1;
            SewDate2 = date_Sewing.Value2;
            Est1 = date_Est.Value1;
            Est2 = date_Est.Value2;
            sp1 = textBox_SP.Text.ToString();
            sp2 = textBox_SP2.Text.ToString();
            Sea = txtSeason1.Text;
            Brand = txtBrand1.Text;
            Ref = textBox_Ref.Text.ToString();
            Cate = combobox_Cate.Text;
            Supp = txtsupplier1.Text;
            Over = comboBox_Over.SelectedItem.ToString();

            lis = new List<SqlParameter>();
            string sqlWhere = "",RWhere ="",OWhere=""; 
            List<string> sqlWheres = new List<string>();
            List<string> RWheres = new List<string>();
            List<string> OWheres = new List<string>();
            #region --組WHERE--

            if (!this.date_Last.Value1.Empty() && !this.date_Last.Value2.Empty())
            {
                sqlWheres.Add("F.PhysicalDate between @LastDate1 and @LastDate2");
                lis.Add(new SqlParameter("@LastDate1", LastDate1));
                lis.Add(new SqlParameter("@LastDate2", LastDate2));
            } if (!this.date_Arrive.Value1.Empty() && !this.date_Arrive.Value2.Empty())
            {
                RWheres.Add("R.WhseArrival between @ArrDate1 and @ArrDate2");
                lis.Add(new SqlParameter("@ArrDate1", ArrDate1));
                lis.Add(new SqlParameter("@ArrDate2", ArrDate2));
            } if (!this.date_SCI.Value1.Empty() && !this.date_SCI.Value2.Empty())
            {
                OWheres.Add("O.SciDelivery between @SCIDate1 and @SCIDate2");
                lis.Add(new SqlParameter("@SCIDate1", SCIDate1));
                lis.Add(new SqlParameter("@SCIDate2", SCIDate2));
            } if (!this.date_Sewing.Value1.Empty() && !this.date_Sewing.Value2.Empty())
            {
                OWheres.Add("O.SewInLine between @SewDate1 and @SewDate2");
                lis.Add(new SqlParameter("@SewDate1", SewDate1));
                lis.Add(new SqlParameter("@SewDate2", SewDate2));
            } if (!this.date_Est.Value1.Empty() && !this.date_Est.Value2.Empty())
            {
                OWheres.Add("O.CutInLine between @Est1 and @Est2");
                lis.Add(new SqlParameter("@Est1", Est1));
                lis.Add(new SqlParameter("@Est2", Est2));
            } if (!this.textBox_SP.Text.Empty())
            {
                OWheres.Add("O.Id between @sp1 and @sp2");
                lis.Add(new SqlParameter("@sp1", sp1));
                lis.Add(new SqlParameter("@sp2", sp2));
            } if (!this.txtSeason1.Text.Empty())
            {
                OWheres.Add("O.SeasonID = @Sea");
                lis.Add(new SqlParameter("@Sea", Sea));
            } if (!this.txtBrand1.Text.Empty())
            {
                OWheres.Add("O.BrandID = @Brand");
                lis.Add(new SqlParameter("@Brand", Brand));
            } if (!this.textBox_Ref.Text.Empty())
            {
                sqlWheres.Add("P.Refno = @Ref");
                lis.Add(new SqlParameter("@Ref", Ref));
            } if (!this.combobox_Cate.SelectedItem.ToString().Empty())
            {
                sqlWheres.Add("D.Category = @Cate");
                lis.Add(new SqlParameter("@Cate", Cate));
            } if (this.txtsupplier1.Text.Empty())
            {
                lis.Add(new SqlParameter("@Supp", Supp));

            } if (this.comboBox_Over.Text == "Pass")
            {
                sqlWheres.Add("dbo.GetFirResult(F.id) = 'Pass'");

            } if (this.comboBox_Over.Text == "Fail")
            {
                sqlWheres.Add("dbo.GetFirResult(F.id) = 'Faill'");

            } if (this.comboBox_Over.Text == "Empty Result")
            {
                sqlWheres.Add("dbo.GetFirResult(F.id) = ' '");

            } if (this.comboBox_Over.Text == "N/A inspection & test")
            {
                sqlWheres.Add("dbo.GetFirResult(F.id) = 'None'");

            }
            
            #endregion
            sqlWhere = string.Join(" and ", sqlWheres);
            OWhere = string.Join(" and ", OWheres);
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
                        select  F.POID,(F.SEQ1+'-'+F.SEQ2)SEQ,O.factoryid,O.BrandID,O.StyleID,O.SeasonID,
	                            t.ExportId,t.InvNo,t.WhseArrival,t.StockQty,
	                            (SELECT MinSciDelivery FROM  DBO.GetSCI(F.Poid,O.Category))[MinSciDelivery],
	                            (SELECT MinBuyerDelivery  FROM  DBO.GetSCI(F.Poid,O.Category))[MinBuyerDelivery],
	                            F.Refno,P.ColorID,(SP.SuppID+'-'+s.AbbEN)Supplier,C.WeaveTypeID,
	                            IIF(F.Nonphysical = 1,'Y',' ')[N/A Physical],F.Result,F.Physical,F.PhysicalDate,
	                            F.TotalInspYds,F.Weight,F.WeightDate,F.ShadeBond,F.ShadeBondDate,F.Continuity,
	                            F.ContinuityDate,L.Result,IIF(L.nonCrocking=1,'Y',' ')[N/A Crocking],L.Crocking,
	                            L.CrockingDate,IIF(L.nonHeat=1,'Y',' ')[N/A Heat Shrinkage],L.Heat,L.HeatDate,
	                            IIF(L.nonWash=1,'Y',' ' )[N/A Wash Shrinkage],L.Wash,L.WashDate,V.Result,CFD.Result
                        from dbo.FIR F
                        inner join (select distinct R.WhseArrival,R.InvNo,R.ExportId,R.Id,rd.PoId,RD.seq1,RD.seq2,RD.StockQty
			                        from dbo.Receiving R
			                        inner join dbo.Receiving_Detail RD on RD.Id = R.Id"
                                    +RWhere+@" 
			                        ) t
                        on t.PoId = F.POID and t.Seq1 = F.SEQ1 and t.Seq2 = F.SEQ2
                        inner join (select distinct poid,O.factoryid,O.BrandID,O.StyleID,O.SeasonID,O.Category from dbo.Orders o "
                                    +OWhere+@"
		                           ) O on O.poid = F.POID
                        OUTER APPLY( select Category=name
                                     from  dbo.DropDownList
                                     where type = 'Category' and id != 'O')D
                        inner join dbo.PO_Supp SP on SP.id = F.POID and SP.SEQ1 = F.SEQ1 and SP.SEQ1 = F.SEQ2
                        inner join dbo.PO_Supp_Detail P on P.ID = F.POID and P.SEQ1 = F.SEQ1 and P.SEQ2 = F.SEQ2
                        OUTER APPLY(SELECT * FROM  supp s WHERE s.id = SP.SuppID AND SP.id = F.Poid and SP.seq1 = F.Seq1)s
                        OUTER APPLY(SELECT * FROM  Fabric C WHERE C.SCIRefno = F.SCIRefno)C
                        OUTER APPLY(SELECT * FROM  FIR_Laboratory L WHERE L.CrockingEncode=1 AND L.ID = F.ID AND L.SEQ1 = F.SEQ1 AND L.SEQ1 = F.SEQ2)L
                        OUTER APPLY(select od.Result from dbo.Oven ov 
                        inner join dbo.Oven_Detail od on od.ID = ov.ID
                        where ov.POID=F.POID and od.SEQ1=F.Seq1 and seq2=F.Seq2 and ov.Status='Confirmed'
                        )V
                        OUTER APPLY(select cd.Result from dbo.ColorFastness CF inner join dbo.ColorFastness_Detail cd on cd.ID = CF.ID
                        where CF.Status = 'Confirmed' and CF.POID=F.POID and cd.SEQ1=F.Seq1 and cd.seq2=F.Seq2
                        )CFD" + sqlWhere );
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
