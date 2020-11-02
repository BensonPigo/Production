using Ict;
using Sci.Data;
using Sci.Utility.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Sci.Production.Quality
{
    public partial class R04 : Win.Tems.PrintForm
    {
        private DateTime? DateRecStart; private DateTime? DateRecEnd;
        private DateTime? DateArrStart; private DateTime? DateArrEnd;
        private string Category; private string factory; private string M;
        private string OUTSTAN = string.Empty;
        private List<SqlParameter> lis; private DualResult res;
        private DataTable dt; private string cmd;

        public R04(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            DataTable m;
            DBProxy.Current.Select(null, "select '' as id union all select distinct id from MDivision WITH (NOLOCK)  ", out m);
            MyUtility.Tool.SetupCombox(this.comboM, 1, m);
            this.comboM.Text = Env.User.Keyword;

            DataTable factory;
            DBProxy.Current.Select(null, "select '' as FTYGroup union all select distinct FTYGroup from Factory WITH (NOLOCK) order by FTYGroup", out factory);
            MyUtility.Tool.SetupCombox(this.comboFactory, 1, factory);

            // comboFactory.Text = Sci.Env.User.Factory;
            this.print.Enabled = false;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.lis = new List<SqlParameter>();
            bool dateReceived_empty = !this.dateReceivedSampleDate.HasValue, dateArr_empty = !this.dateArriveWHDate.HasValue, cate_comboBox_Empty = this.comboCategory.Text.Empty(), comboFactory_Empty = this.comboFactory.Text.Empty(), comboM_Empty = this.comboM.Text.Empty(),
                checkOuter_empty = this.checkOutstandingOnly.Checked.Empty();
            if (dateReceived_empty && dateArr_empty)
            {
                this.dateReceivedSampleDate.Focus();
                MyUtility.Msg.ErrorBox("Please select 'Received Sample Date' or 'Arrive W/H Date' at least one field entry");
                return false;
            }

            if (this.checkOutstandingOnly.Checked == true)
            {
                this.OUTSTAN = "YES";
            }
            else
            {
                this.OUTSTAN = "NO";
            }

            string sqlWhere = string.Empty;
            string sqlRec = string.Empty;
            string sqlArr = string.Empty;
            string sqlOutStanding = string.Empty;
            string sqlFactorys = string.Empty;

            this.DateRecStart = this.dateReceivedSampleDate.Value1;
            this.DateRecEnd = this.dateReceivedSampleDate.Value2;
            this.DateArrStart = this.dateArriveWHDate.Value1;
            this.DateArrEnd = this.dateArriveWHDate.Value2;
            this.Category = this.comboCategory.Text;
            this.factory = this.comboFactory.Text;
            this.M = this.comboM.Text;
            List<string> sqlWheres = new List<string>();
            List<string> sqlRecDate = new List<string>();
            List<string> sqlArrDate = new List<string>();
            List<string> sqlFactory = new List<string>();
            #region --組WHERE--
            if (!this.dateReceivedSampleDate.Value1.Empty())
            {
                sqlRecDate.Add("ReceiveSampleDate >= @DateRecStart");
                this.lis.Add(new SqlParameter("@DateRecStart", this.DateRecStart));
            }

            if (!this.dateReceivedSampleDate.Value2.Empty())
            {
                sqlRecDate.Add("ReceiveSampleDate <= @DateRecEnd");
                this.lis.Add(new SqlParameter("@DateRecEnd", this.DateRecEnd));
            }

            if (!this.dateArriveWHDate.Value1.Empty())
            {
                sqlArrDate.Add("WhseArrival >= @DateArrStart");
                this.lis.Add(new SqlParameter("@DateArrStart", this.DateArrStart));
            }

            if (!this.dateArriveWHDate.Value2.Empty())
            {
                sqlArrDate.Add("WhseArrival <= @DateArrEnd");
                this.lis.Add(new SqlParameter("@DateArrEnd", this.DateArrEnd));
            }

            if (!MyUtility.Check.Empty(this.comboCategory.Text))
            {
                sqlWheres.Add($"Category in ({this.comboCategory.SelectedValue})");
            }

            if (!this.comboFactory.Text.ToString().Empty())
            {
                sqlFactory.Add("r.Factoryid = @Factory");
                this.lis.Add(new SqlParameter("@Factory", this.factory));
            }

            if (!this.comboM.Text.ToString().Empty())
            {
                sqlFactory.Add("r.MDivisionID=@MDivisionID");
                this.lis.Add(new SqlParameter("@MDivisionID", this.M));
            }

            if (this.checkOutstandingOnly.Checked == true)
            {
                sqlOutStanding = " and (C.Crocking ='' or C.Wash ='' or C.Heat ='' or oven_result.Result='' or ColorFastness_result.Result='')";
            }
            else
            {
                sqlOutStanding = " ";
            }

            #endregion

            sqlWhere = string.Join(" and ", sqlWheres);
            sqlRec = string.Join(" and ", sqlRecDate);
            sqlArr = string.Join(" and ", sqlArrDate);
            sqlFactorys = string.Join(" and ", sqlFactory);
            if (!sqlWhere.Empty())
            {
                sqlWhere = " AND " + sqlWhere;
            }

            if (!sqlRec.Empty())
            {
                sqlRec = " and " + sqlRec;
            }

            if (!sqlArr.Empty())
            {
                sqlArr = " AND " + sqlArr;
            }

            if (!sqlFactorys.Empty())
            {
                sqlFactorys = " and " + sqlFactorys;
            }

            sqlWhere += this.chkIncludeCancelOrder.Checked ? string.Empty : " and orders.Junk = 0 ";
            #region --撈ListExcel資料--

            this.cmd = string.Format(@" 
            with order_rawdata as
            (
	            select distinct poid from dbo.orders WITH (NOLOCK) 
	            where 1=1 
           " + sqlWhere + @"
            )
                select (select exportid from dbo.Receiving WITH (NOLOCK) where id = b.ReceivingID) [exportid]
                ,(select WhseArrival from  dbo.View_AllReceiving WITH (NOLOCK) where id = b.ReceivingID) [WhseArrival]
                ,o.FactoryID [factory]
                ,b.POID
                ,[Cancel] = IIF(o.Junk=1,'Y','N')
                ,b.seq1+'-'+b.seq2 [seq],c.ReceiveSampleDate
                ,(select suppid+'-'+supp.AbbEN from dbo.po_supp p WITH (NOLOCK) inner join dbo.supp WITH (NOLOCK) on supp.id = p.SuppID where p.id = b.POID and seq1 = b.seq1) [supplier]
                ,b.Refno
                ,(select p.ColorID from dbo.PO_Supp_Detail p WITH (NOLOCK) where p.id = b.POID and seq1 = b.seq1 AND seq2 = b.SEQ2) color
                ,o.Category 
                ,b.ArriveQty,oven_result.Result,c.Crocking,c.Heat,ColorFastness_result.Result,c.Wash
                from  FIR b WITH (NOLOCK) 
                inner join (select distinct a.id,a.PoId,a.Seq1,a.seq2,a.MDivisionID,a.factoryid 
                            from dbo.View_AllReceivingDetail a WITH (NOLOCK) 
                            where 1=1" + sqlArr + @")
                r on r.id = b.ReceivingID and r.PoId = b.POID and r.seq1 = b.seq1 and r.seq2 = b.SEQ2
                inner join FIR_Laboratory c WITH (NOLOCK) on c.ID = b.Id
                inner join order_rawdata d on d.POID = b.POID
                inner join Orders o with (nolock) on d.POID = o.ID
                OUTER APPLY(
	                select o1.Result from Oven o1 WITH (NOLOCK) inner join Oven_Detail o2 WITH (NOLOCK) on o2.Id = o1.ID
	                where o1.POID = b.POID and o2.seq1 = b.seq1 and o2.seq2 = b.SEQ2 and o1.Status = 'Confirmed')oven_result
                OUTER APPLY(
	                select  c1.Result from ColorFastness c1 WITH (NOLOCK) inner join ColorFastness_Detail  c2 WITH (NOLOCK) on c2.id = c1.id
	                where c1.POID = b.POID and c2.seq1 = b.seq1 and c2.seq2 = b.SEQ2 and c1.Status = 'Confirmed')ColorFastness_result
                where 1=1 "
                        + sqlFactorys
           + sqlOutStanding + sqlRec + @"
            ");
            #endregion

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            this.res = DBProxy.Current.Select(string.Empty, this.cmd, this.lis, out this.dt);
            if (!this.res)
            {
                return this.res;
            }

            return this.res;
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

            var saveDialog = MyExcelPrg.GetSaveFileDialog(MyExcelPrg.Filter_Excel);

            SaveXltReportCls xl = new SaveXltReportCls("Quality_R04.xltx", keepApp: true);

            string d1 = MyUtility.Check.Empty(this.DateRecStart) ? string.Empty : Convert.ToDateTime(this.DateRecStart).ToString("yyyy/MM/dd");
            string d2 = MyUtility.Check.Empty(this.DateRecEnd) ? string.Empty : Convert.ToDateTime(this.DateRecEnd).ToString("yyyy/MM/dd");
            string d3 = MyUtility.Check.Empty(this.DateArrStart) ? string.Empty : Convert.ToDateTime(this.DateArrStart).ToString("yyyy/MM/dd");
            string d4 = MyUtility.Check.Empty(this.DateArrEnd) ? string.Empty : Convert.ToDateTime(this.DateArrEnd).ToString("yyyy/MM/dd");

            xl.DicDatas.Add("##QADate", d1 + "~" + d2);
            xl.DicDatas.Add("##ArriveDate", d3 + "~" + d4);
            xl.DicDatas.Add("##Category", this.Category);
            xl.DicDatas.Add("##Factory", this.factory);
            xl.DicDatas.Add("##Outstanding", this.OUTSTAN);

            SaveXltReportCls.XltRptTable xlTable = new SaveXltReportCls.XltRptTable(this.dt)
            {
                ShowHeader = false,
            };
            xl.DicDatas.Add("##body", xlTable);

            xl.Save(Class.MicrosoftFile.GetName("Quality_R04"));
            ((Microsoft.Office.Interop.Excel.Worksheet)xl.ExcelApp.ActiveSheet).Columns.AutoFit();
            xl.FinishSave();
            return true;
        }
    }
}
