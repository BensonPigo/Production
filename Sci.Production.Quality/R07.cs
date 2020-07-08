using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;

namespace Sci.Production.Quality
{
    public partial class R07 : Sci.Win.Tems.PrintForm
    {
        DateTime? DateArrStart;
        DateTime? DateArrEnd;
        DateTime? DateSCIStart;
        DateTime? DateSCIEnd;
        DateTime? DateSewStart;
        DateTime? DateSewEnd;
        DateTime? DateEstStart;
        DateTime? DateEstEnd;
        string spStrat;
        string spEnd;
        string Season;
        string Brand;
        string RefNo;
        string Category;
        string Supp;
        string MaterialType;
        string Factory;
        int reportType;
        List<SqlParameter> lis;
        DataTable dt; string cmd;

        public R07(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            DataTable Material = null;
            string sqlM = @" 
                        SELECT distinct case fabrictype
                               when 'F' then 'Fabric' 
	                           when 'A' then 'Accessory'
                               end fabrictype
                        FROM Po_supp_detail  WITH (NOLOCK) 
                        where fabrictype !='O'  AND fabrictype !=''
                        ";
            DBProxy.Current.Select(string.Empty, sqlM, out Material);
            Material.DefaultView.Sort = "fabrictype";
            this.comboMaterialType.DataSource = Material;
            this.comboMaterialType.ValueMember = "fabrictype";
            this.comboMaterialType.DisplayMember = "fabrictype";
            this.comboMaterialType.SelectedIndex = 1;
            DataTable factory;
            DBProxy.Current.Select(null, "select distinct FTYGroup from Factory WITH (NOLOCK) order by FTYGroup", out factory);
            factory.Rows.Add(new string[] { string.Empty });
            factory.DefaultView.Sort = "FTYGroup";
            MyUtility.Tool.SetupCombox(this.comboFactory, 1, factory);
            this.comboFactory.Text = Sci.Env.User.Factory;
            this.print.Enabled = false;
        }

        protected override bool ValidateInput()
        {
            bool date_Arrive_Empty = !this.dateArriveWHDate.HasValue, date_SCI_Empty = !this.dateSCIDelivery.HasValue, date_Sewing_Empty = !this.dateSewingInLineDate.HasValue, date_Est_Empty = !this.dateEstCuttingDate.HasValue,
               textBox_SP_Empty = this.txtSPStart.Text.Empty(), textBox_SP2_Empty = this.txtSPEnd.Text.Empty(), txtSEA_Empty = this.txtseason.Text.Empty(),
               txtBrand_Empty = this.txtbrand.Text.Empty(), txtRef_Empty = this.txtRefno.Text.Empty(), Cate_comboBox_Empty = this.comboCategory.Text.Empty(), Supp_Empty = !this.txtsupplier.Text.Empty(),
               MaterialType_Empty = !this.comboMaterialType.Text.Empty(), Factory_Empty = !this.comboFactory.Text.Empty();
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
            this.spEnd = this.txtSPStart.Text.ToString();
            this.Season = this.txtseason.Text;
            this.Brand = this.txtbrand.Text;
            this.RefNo = this.txtRefno.Text.ToString();
            this.Category = this.comboCategory.Text;
            this.Supp = this.txtsupplier.Text;
            this.MaterialType = this.comboMaterialType.Text;
            this.Factory = this.comboFactory.Text;
            this.reportType = this.rdbtnbyWK.Checked ? 1 : 2;
            this.lis = new List<SqlParameter>();
            string sqlWhere = string.Empty, RWhere = string.Empty, OWhere = string.Empty;
            List<string> sqlWheres = new List<string>();
            List<string> RWheres = new List<string>();
            List<string> OWheres = new List<string>();
            #region --組WHERE--
            if (!this.dateArriveWHDate.Value1.Empty())
            {
                RWheres.Add("R.WhseArrival >= @ArrDate1");
                this.lis.Add(new SqlParameter("@ArrDate1", this.DateArrStart));
            }

            if (!this.dateArriveWHDate.Value2.Empty())
            {
                RWheres.Add("R.WhseArrival <= @ArrDate2");
                this.lis.Add(new SqlParameter("@ArrDate2", this.DateArrEnd));
            }

            if (!this.dateSCIDelivery.Value1.Empty())
            {
                OWheres.Add("O.SciDelivery >= @SCIDate1");
                this.lis.Add(new SqlParameter("@SCIDate1", this.DateSCIStart));
            }

            if (!this.dateSCIDelivery.Value2.Empty())
            {
                OWheres.Add("O.SciDelivery <= @SCIDate2");
                this.lis.Add(new SqlParameter("@SCIDate2", this.DateSCIEnd));
            }

            if (!this.dateSewingInLineDate.Value1.Empty())
            {
                OWheres.Add("O.SewInLine >= @SewDate1");
                this.lis.Add(new SqlParameter("@SewDate1", this.DateSewStart));
            }

            if (!this.dateSewingInLineDate.Value2.Empty())
            {
                OWheres.Add("O.SewInLine <= @SewDate2");
                this.lis.Add(new SqlParameter("@SewDate2", this.DateSewEnd));
            }

            if (!this.dateEstCuttingDate.Value1.Empty())
            {
                OWheres.Add("O.CutInLine >= @Est1");
                this.lis.Add(new SqlParameter("@Est1", this.DateEstStart));
            }

            if (!this.dateEstCuttingDate.Value2.Empty())
            {
                OWheres.Add("O.CutInLine <= @Est2");
                this.lis.Add(new SqlParameter("@Est2", this.DateEstEnd));
            }

            if (!this.txtSPStart.Text.Empty())
            {
                OWheres.Add("O.Id between @sp1 and @sp2");
                this.lis.Add(new SqlParameter("@sp1", this.spStrat));
                this.lis.Add(new SqlParameter("@sp2", this.spEnd));
            }

            if (!this.txtseason.Text.Empty())
            {
                OWheres.Add("O.SeasonID = @Sea");
                this.lis.Add(new SqlParameter("@Sea", this.Season));
            }

            if (!this.txtbrand.Text.Empty())
            {
                OWheres.Add("O.BrandID = @Brand");
                this.lis.Add(new SqlParameter("@Brand", this.Brand));
            }

            if (!this.txtRefno.Text.Empty())
            {
                sqlWheres.Add("psd.Refno = @Ref");
                this.lis.Add(new SqlParameter("@Ref", this.RefNo));
            }

            if (!MyUtility.Check.Empty(this.comboCategory.Text))
            {
                OWheres.Add($"O.Category in ({this.comboCategory.SelectedValue})");
            }

            if (!this.txtsupplier.Text.Empty())
            {
                sqlWheres.Add("ps.SuppId = @Supp");
                this.lis.Add(new SqlParameter("@Supp", this.Supp));
            }

            if (!this.comboMaterialType.SelectedItem.ToString().Empty())
            {
                sqlWheres.Add("psd.fabrictype = @MaterialType");
                if (this.MaterialType == "Accessory")
                {
                    this.lis.Add(new SqlParameter("@MaterialType", "A"));
                }

                if (this.MaterialType == "Fabric")
                {
                    this.lis.Add(new SqlParameter("@MaterialType", "F"));
                }
            }

            if (!this.comboFactory.Text.Empty())
            {
                OWheres.Add("O.factoryid = @Factory");
                this.lis.Add(new SqlParameter("@Factory", this.Factory));
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

            this.cmd = $@"
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
			            where (r.type='A' or r.DataFrom = 'TransferIn')" + RWhere + $@"
			            group by r.WhseArrival,r.InvNo,r.ExportId,r.Id,r.PoId,r.seq1,r.seq2{byRoll}) t
            inner join (select distinct id,Category,KPILETA from dbo.Orders o WITH (NOLOCK) 
			            where 1=1
			           " + OWhere + @" ) x on x.id = T.POID
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
            res = DBProxy.Current.Select(string.Empty, this.cmd, this.lis, out this.dt);
            if (!res)
            {
                return res;
            }

            return res;
        }

        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.dt.Rows.Count);
            if (this.dt == null || this.dt.Rows.Count == 0)
            {
                MyUtility.Msg.ErrorBox("Data not found");
                return false;
            }

            this.ShowWaitMessage("Excel Processing...");
            foreach (DataRow dr in this.dt.Rows)
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
            MyUtility.Excel.CopyToXls(this.dt, string.Empty, $"{excelName}.xltx", 3, showExcel: false, showSaveMsg: false, excelApp: objApp);

            string d1 = MyUtility.Check.Empty(this.DateArrStart) ? string.Empty : Convert.ToDateTime(this.DateArrStart).ToString("yyyy/MM/dd");
            string d2 = MyUtility.Check.Empty(this.DateArrEnd) ? string.Empty : Convert.ToDateTime(this.DateArrEnd).ToString("yyyy/MM/dd");
            string d3 = MyUtility.Check.Empty(this.DateSCIStart) ? string.Empty : Convert.ToDateTime(this.DateSCIStart).ToString("yyyy/MM/dd");
            string d4 = MyUtility.Check.Empty(this.DateSCIEnd) ? string.Empty : Convert.ToDateTime(this.DateSCIEnd).ToString("yyyy/MM/dd");
            string d5 = MyUtility.Check.Empty(this.DateSewStart) ? string.Empty : Convert.ToDateTime(this.DateSewStart).ToString("yyyy/MM/dd");
            string d6 = MyUtility.Check.Empty(this.DateSewEnd) ? string.Empty : Convert.ToDateTime(this.DateSewEnd).ToString("yyyy/MM/dd");
            string d7 = MyUtility.Check.Empty(this.DateEstStart) ? string.Empty : Convert.ToDateTime(this.DateEstStart).ToString("yyyy/MM/dd");
            string d8 = MyUtility.Check.Empty(this.DateEstEnd) ? string.Empty : Convert.ToDateTime(this.DateEstEnd).ToString("yyyy/MM/dd");

            Excel.Worksheet worksheet = objApp.Sheets[1];
            worksheet.Cells[1, 2] = d1 + "~" + d2; // ##ArrDate
            worksheet.Cells[1, 5] = d5 + "~" + d6; // ##SewingDate
            worksheet.Cells[1, 9] = this.spStrat + "~" + this.spEnd; // ##Spno
            worksheet.Cells[1, 13] = this.Brand; // ##Brand
            worksheet.Cells[1, 16] = this.Category; // ##Category
            worksheet.Cells[1, 20] = this.MaterialType; // ##Material
            worksheet.Cells[2, 2] = d3 + "~" + d4; // ##SciDelivery
            worksheet.Cells[2, 5] = d7 + "~" + d8; // ##EstCutting
            worksheet.Cells[2, 9] = this.Season; // ##Season
            worksheet.Cells[2, 13] = this.RefNo; // ##Refno
            worksheet.Cells[2, 16] = this.Supp; // ##Supplier
            worksheet.Cells[2, 20] = this.Factory; // ##Factory

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
