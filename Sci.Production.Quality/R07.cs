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
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;

namespace Sci.Production.Quality
{
    public partial class R07 : Sci.Win.Tems.PrintForm
    {
        DateTime? DateArrStart, DateArrEnd;
        DateTime? DateSCIStart, DateSCIEnd;
        DateTime? DateSewStart, DateSewEnd;
        DateTime? DateEstStart, DateEstEnd;
        string spStrat, spEnd, Season, Brand, RefNo, Category, Supp;
        string MaterialType, Factory;
        int reportType;
        List<SqlParameter> lis;
        DataTable dt; string cmd;

        public R07(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            DataTable Material = null;
            string sqlM = (@" 
                        SELECT distinct case fabrictype
                               when 'F' then 'Fabric' 
	                           when 'A' then 'Accessory'
                               end fabrictype
                        FROM Po_supp_detail  WITH (NOLOCK) 
                        where fabrictype !='O'  AND fabrictype !=''
                        ");
            DBProxy.Current.Select("", sqlM, out Material);
            Material.DefaultView.Sort = "fabrictype";
            this.comboMaterialType.DataSource = Material;
            this.comboMaterialType.ValueMember = "fabrictype";
            this.comboMaterialType.DisplayMember = "fabrictype";
            this.comboMaterialType.SelectedIndex = 1;
            DataTable factory;
            DBProxy.Current.Select(null, "select distinct FTYGroup from Factory WITH (NOLOCK) order by FTYGroup", out factory);
            factory.Rows.Add(new string[] { "" });
            factory.DefaultView.Sort = "FTYGroup";
            MyUtility.Tool.SetupCombox(comboFactory, 1, factory);
            comboFactory.Text = Sci.Env.User.Factory;
            print.Enabled = false;
        }

        protected override bool ValidateInput()
        {
            bool date_Arrive_Empty = !this.dateArriveWHDate.HasValue, date_SCI_Empty = !this.dateSCIDelivery.HasValue, date_Sewing_Empty = !this.dateSewingInLineDate.HasValue, date_Est_Empty = !this.dateEstCuttingDate.HasValue,
               textBox_SP_Empty = this.txtSPStart.Text.Empty(), textBox_SP2_Empty = this.txtSPEnd.Text.Empty(), txtSEA_Empty = this.txtseason.Text.Empty()
          , txtBrand_Empty = this.txtbrand.Text.Empty(), txtRef_Empty = this.txtRefno.Text.Empty(), Cate_comboBox_Empty = this.comboCategory.Text.Empty(), Supp_Empty = !this.txtsupplier.Text.Empty()
          , MaterialType_Empty = !this.comboMaterialType.Text.Empty(), Factory_Empty = !this.comboFactory.Text.Empty();
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
            spEnd = txtSPStart.Text.ToString();
            Season = txtseason.Text;
            Brand = txtbrand.Text;
            RefNo = txtRefno.Text.ToString();
            Category = comboCategory.Text;
            Supp = txtsupplier.Text;
            MaterialType = comboMaterialType.Text;
            Factory = comboFactory.Text;
            reportType = rdbtnbyWK.Checked ? 1 : 2;
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
            } if (!this.txtseason.Text.Empty())
            {
                OWheres.Add("O.SeasonID = @Sea");
                lis.Add(new SqlParameter("@Sea", Season));
            } if (!this.txtbrand.Text.Empty())
            {
                OWheres.Add("O.BrandID = @Brand");
                lis.Add(new SqlParameter("@Brand", Brand));
            } if (!this.txtRefno.Text.Empty())
            {
                sqlWheres.Add("psd.Refno = @Ref");
                lis.Add(new SqlParameter("@Ref", RefNo));
            } if (!MyUtility.Check.Empty(this.comboCategory.Text))
            {
                OWheres.Add($"O.Category in ({this.comboCategory.SelectedValue})");
            } if (!this.txtsupplier.Text.Empty())
            {
                sqlWheres.Add("ps.SuppId = @Supp");
                lis.Add(new SqlParameter("@Supp", Supp));
            } if (!this.comboMaterialType.SelectedItem.ToString().Empty())
            {
                sqlWheres.Add("psd.fabrictype = @MaterialType");
                if (MaterialType == "Accessory")
                {
                    lis.Add(new SqlParameter("@MaterialType", "A"));
                }
                if (MaterialType == "Fabric")
                {
                    lis.Add(new SqlParameter("@MaterialType", "F"));
                }
            } if (!this.comboFactory.Text.Empty())
            {
                OWheres.Add("O.factoryid = @Factory");
                lis.Add(new SqlParameter("@Factory", Factory));
            }

            #endregion
            sqlWhere = string.Join(" and ", sqlWheres);
            RWhere = string.Join(" and ", RWheres);
            OWhere = string.Join(" and ", OWheres);
            if (!sqlWhere.Empty())
            {
                sqlWhere = " where " + sqlWhere;
            }
            if (!RWhere.Empty())
            {
                RWhere = " AND " + RWhere;
            }
            if (!OWhere.Empty())
            {
                OWhere = " AND " + OWhere;
            }
            #region --撈ListExcel資料--
            string byRoll = string.Empty;
            if (this.reportType == 2)
            {
                byRoll = ",Roll,Dyelot";
            }
            cmd = $@"
            select 
            Est.inspection
            ,[First].cutinline
            ,[Urgent Inspection]= 
	        iif(Est.inspection < [First].cutinline,
	        DATEDIFF(day,getdate(),Est.inspection),
	        datediff(day,getdate(),[First].cutinline))
            ,t.PoId
            ,t.seq1+'-'+t.seq2 [Seq]
            {byRoll}
            ,O.FactoryID
            ,O.BrandId
            , case PSD.fabrictype
              when 'F' then 'Fabric' 
              when 'A' then 'Accessory'
              end MaterialType
            ,O.StyleID
            ,O.SeasonId
            ,t.ExportId
            ,t.InvNo
            ,t.WhseArrival
            ,t.stockqty
            ,w.MinSciDelivery
            ,w.MinBuyerDelivery
            ,dbo.getMtlDesc(t.poid,t.seq1,t.seq2,1,0) [description]
            ,dbo.GetColorMultipleID(O.BrandId,PSD.ColorID) [ColorID]
            ,PS.SuppID
            ,Weave.WeaveTypeID
            ,t.id [ReceivingID]
            from (select r.WhseArrival,r.InvNo,r.ExportId,r.Id,r.PoId,r.seq1,r.seq2{byRoll},sum(stockqty) stockqty
			             from dbo.View_AllReceivingDetail r WITH (NOLOCK) 
			            where r.type='A'" + RWhere+ $@"
			            group by r.WhseArrival,r.InvNo,r.ExportId,r.Id,r.PoId,r.seq1,r.seq2{byRoll}) t
            inner join (select distinct id,Category,KPILETA from dbo.Orders o WITH (NOLOCK) 
			            where 1=1
			           " + OWhere+ @" ) x on x.id = T.POID
            inner join dbo.PO_Supp ps WITH (NOLOCK) on ps.id = T.POID and ps.SEQ1 = T.SEQ1
            inner join dbo.PO_Supp_Detail psd WITH (NOLOCK) on psd.ID = T.POID and psd.SEQ1 = T.SEQ1 and psd.SEQ2 = T.SEQ2
            outer apply dbo.getsci(t.poid,x.category) as w
            outer apply(
	            select case when x.Category='M' then DATEADD(day,7,t.WhseArrival)
	            when Datediff(day,x.kpileta,w.MinSciDelivery) >=21 
		            then iif(x.KPILETA <= DATEADD(day,3,t.WhseArrival)
	            ,DATEADD(day,7,t.WhseArrival),x.KPILETA)
	            when Datediff(day,x.kpileta,w.MinSciDelivery) < 21 
		            then iif(DATEADD(day,-21,w.MinSciDelivery) <= DATEADD(day,3,t.WhseArrival)
	            ,DATEADD(day,7,t.WhseArrival),DATEADD(day,-21,w.MinSciDelivery))
            end  inspection)  Est
            outer apply((select min(orders.CutInLine)cutinline from dbo.orders WITH (NOLOCK) where orders.poid= t.PoId))[First] 
            outer apply(select o.factoryid,o.BrandId,o.StyleID,o.SeasonId from dbo.orders o WITH (NOLOCK) where o.id = t.PoId)O
            outer apply(select f.WeaveTypeID from dbo.Fabric f WITH (NOLOCK) where f.scirefno = psd.SCIRefno)Weave" + sqlWhere;
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

            this.ShowWaitMessage("Excel Processing...");
            foreach (DataRow dr in dt.Rows)
            {
                dr["description"] = dr["description"].ToString().Trim();
            }

            string excelName = string.Empty;
            if (this.reportType == 1)
            {
                excelName = "Quality_R07";
            }
            else if (this.reportType == 2)
            {
                excelName = "Quality_R07_byRoll";
            }
            Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + $"\\{excelName}.xltx");
            MyUtility.Excel.CopyToXls(dt, "", $"{excelName}.xltx", 3, showExcel: false, showSaveMsg: false, excelApp: objApp);

            string d1 = (MyUtility.Check.Empty(DateArrStart)) ? "" : Convert.ToDateTime(DateArrStart).ToString("yyyy/MM/dd");
            string d2 = (MyUtility.Check.Empty(DateArrEnd)) ? "" : Convert.ToDateTime(DateArrEnd).ToString("yyyy/MM/dd");
            string d3 = (MyUtility.Check.Empty(DateSCIStart)) ? "" : Convert.ToDateTime(DateSCIStart).ToString("yyyy/MM/dd");
            string d4 = (MyUtility.Check.Empty(DateSCIEnd)) ? "" : Convert.ToDateTime(DateSCIEnd).ToString("yyyy/MM/dd");
            string d5 = (MyUtility.Check.Empty(DateSewStart)) ? "" : Convert.ToDateTime(DateSewStart).ToString("yyyy/MM/dd");
            string d6 = (MyUtility.Check.Empty(DateSewEnd)) ? "" : Convert.ToDateTime(DateSewEnd).ToString("yyyy/MM/dd");
            string d7 = (MyUtility.Check.Empty(DateEstStart)) ? "" : Convert.ToDateTime(DateEstStart).ToString("yyyy/MM/dd");
            string d8 = (MyUtility.Check.Empty(DateEstEnd)) ? "" : Convert.ToDateTime(DateEstEnd).ToString("yyyy/MM/dd");

            Excel.Worksheet worksheet = objApp.Sheets[1];
            worksheet.Cells[1, 2] = d1 + "~" + d2; //##ArrDate
            worksheet.Cells[1, 5] = d5 + "~" + d6; //##SewingDate
            worksheet.Cells[1, 9] = spStrat + "~" + spEnd; //##Spno
            worksheet.Cells[1, 13] = Brand; //##Brand
            worksheet.Cells[1, 16] = Category; //##Category
            worksheet.Cells[1, 20] = MaterialType; //##Material
            worksheet.Cells[2, 2] = d3 + "~" + d4; //##SciDelivery
            worksheet.Cells[2, 5] = d7 + "~" + d8; //##EstCutting
            worksheet.Cells[2, 9] = Season; //##Season
            worksheet.Cells[2, 13] = RefNo; //##Refno
            worksheet.Cells[2, 16] = Supp; //##Supplier
            worksheet.Cells[2, 20] = Factory; //##Factory

            worksheet.Rows.AutoFit();

            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName(excelName);
            objApp.ActiveWorkbook.SaveAs(strExcelName);
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);
            Marshal.ReleaseComObject(worksheet);

            strExcelName.OpenFile();
            #endregion 
            this.HideWaitMessage();
            return true;
        }
    }
}
