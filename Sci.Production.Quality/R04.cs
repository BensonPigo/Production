using Ict;
using Sci.Data;
using Sci.Utility.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using sxrc = Sci.Utility.Excel.SaveXltReportCls;

namespace Sci.Production.Quality
{
    public partial class R04 : Sci.Win.Tems.PrintForm
    {   
        DateTime? DateRecStart; DateTime? DateRecEnd;
        DateTime? DateArrStart; DateTime? DateArrEnd;
        string Category; string factory; string M;
        string OUTSTAN = "";
        List<SqlParameter> lis; DualResult res;
        DataTable dt; string cmd;

        public R04(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            DataTable M;
            DBProxy.Current.Select(null, "select '' as id union all select distinct id from MDivision WITH (NOLOCK)  ", out M);
            MyUtility.Tool.SetupCombox(comboM, 1, M);
            comboM.Text = Sci.Env.User.Keyword;

            DataTable factory;
            DBProxy.Current.Select(null, "select '' as FTYGroup union all select distinct FTYGroup from Factory WITH (NOLOCK) order by FTYGroup", out factory);
            MyUtility.Tool.SetupCombox(comboFactory, 1, factory);
            //comboFactory.Text = Sci.Env.User.Factory;
            print.Enabled = false;
        }

        protected override bool ValidateInput()
        {
            lis = new List<SqlParameter>();
            bool DateReceived_empty = !dateReceivedSampleDate.HasValue, DateArr_empty = !dateArriveWHDate.HasValue, Cate_comboBox_Empty = this.comboCategory.Text.Empty(), comboFactory_Empty = this.comboFactory.Text.Empty(),comboM_Empty=this.comboM.Text.Empty(),
                checkOuter_empty = checkOutstandingOnly.Checked.Empty();
            if (DateReceived_empty && DateArr_empty)
            {
                dateReceivedSampleDate.Focus();
                MyUtility.Msg.ErrorBox("Please select 'Received Sample Date' or 'Arrive W/H Date' at least one field entry");
                return false;
            }
            if (checkOutstandingOnly.Checked == true)
            {
                OUTSTAN = "YES";
            }
            else
            {
                OUTSTAN = "NO";
            }

            string sqlWhere = ""; string sqlRec = ""; string sqlArr = ""; string sqlOutStanding = ""; string sqlFactorys = "";

            DateRecStart = dateReceivedSampleDate.Value1;
            DateRecEnd = dateReceivedSampleDate.Value2;
            DateArrStart = dateArriveWHDate.Value1;
            DateArrEnd = dateArriveWHDate.Value2;
            Category = comboCategory.Text;
            factory = comboFactory.Text;
            M = comboM.Text;
            List<string> sqlWheres = new List<string>();
            List<string> sqlRecDate = new List<string>();
            List<string> sqlArrDate = new List<string>();
            List<string> sqlFactory = new List<string>();     
            #region --組WHERE--
            if (!this.dateReceivedSampleDate.Value1.Empty())
            {
                sqlRecDate.Add("ReceiveSampleDate >= @DateRecStart");
                lis.Add(new SqlParameter("@DateRecStart", DateRecStart));
            }
            if (!this.dateReceivedSampleDate.Value2.Empty())
            {
                sqlRecDate.Add("ReceiveSampleDate <= @DateRecEnd");
                lis.Add(new SqlParameter("@DateRecEnd", DateRecEnd));
            }
            if (!this.dateArriveWHDate.Value1.Empty())
            {
                sqlArrDate.Add("WhseArrival >= @DateArrStart");
                lis.Add(new SqlParameter("@DateArrStart", DateArrStart));
            }
            if (!this.dateArriveWHDate.Value2.Empty())
            {
                sqlArrDate.Add("WhseArrival <= @DateArrEnd");
                lis.Add(new SqlParameter("@DateArrEnd", DateArrEnd));
            }
            if (!MyUtility.Check.Empty(this.comboCategory.Text))
            {
                sqlWheres.Add($"Category in ({this.comboCategory.SelectedValue})");
            }
            if (!this.comboFactory.Text.ToString().Empty())
            {
                sqlFactory.Add("r.Factoryid = @Factory");
                lis.Add(new SqlParameter("@Factory", factory));
            }
            if (!this.comboM.Text.ToString().Empty())
            {
                sqlFactory.Add("r.MDivisionID=@MDivisionID");
                lis.Add(new SqlParameter("@MDivisionID", M));
            }
            if (checkOutstandingOnly.Checked == true)
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
            } if (!sqlRec.Empty())
            {
                sqlRec = " and " + sqlRec;
            } if (!sqlArr.Empty())
            {
                sqlArr = " AND " + sqlArr;
            }
            if (!sqlFactorys.Empty())
            {
                sqlFactorys = " and " + sqlFactorys;
            }

            #region --撈ListExcel資料--

            cmd = string.Format(@" 
            with order_rawdata as
            (
	            select distinct poid from dbo.orders WITH (NOLOCK) 
	            where 1=1 and orders.Junk = 0
           " + sqlWhere + @"
            )
                select (select exportid from dbo.Receiving WITH (NOLOCK) where id = b.ReceivingID) [exportid]
                ,(select WhseArrival from  dbo.View_AllReceiving WITH (NOLOCK) where id = b.ReceivingID) [WhseArrival]
                ,(select top 1 orders.FactoryID from orders WITH (NOLOCK) where id = b.POID) [factory]
                ,b.POID,b.seq1+'-'+b.seq2 [seq],c.ReceiveSampleDate
                ,(select suppid+'-'+supp.AbbEN from dbo.po_supp p WITH (NOLOCK) inner join dbo.supp WITH (NOLOCK) on supp.id = p.SuppID where p.id = b.POID and seq1 = b.seq1) [supplier]
                ,b.Refno
                ,(select p.ColorID from dbo.PO_Supp_Detail p WITH (NOLOCK) where p.id = b.POID and seq1 = b.seq1 AND seq2 = b.SEQ2) color
                ,(select top 1 orders.[category] from orders WITH (NOLOCK) where id = b.POID) [category]
                ,b.ArriveQty,oven_result.Result,c.Crocking,c.Heat,ColorFastness_result.Result,c.Wash
                from  FIR b WITH (NOLOCK) 
                inner join (select distinct a.id,a.PoId,a.Seq1,a.seq2,a.MDivisionID,a.factoryid 
                            from dbo.View_AllReceivingDetail a WITH (NOLOCK) 
                            where 1=1" + sqlArr + @")
                r on r.id = b.ReceivingID and r.PoId = b.POID and r.seq1 = b.seq1 and r.seq2 = b.SEQ2
                inner join FIR_Laboratory c WITH (NOLOCK) on c.ID = b.Id
                inner join order_rawdata d on d.POID = b.POID
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

        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
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
           
            Sci.Utility.Excel.SaveXltReportCls xl = new Sci.Utility.Excel.SaveXltReportCls("Quality_R04.xltx", keepApp: true);

            string d1 = (MyUtility.Check.Empty(DateRecStart)) ? "" : Convert.ToDateTime(DateRecStart).ToString("yyyy/MM/dd");
            string d2 = (MyUtility.Check.Empty(DateRecEnd)) ? "" : Convert.ToDateTime(DateRecEnd).ToString("yyyy/MM/dd");
            string d3 = (MyUtility.Check.Empty(DateArrStart)) ? "" : Convert.ToDateTime(DateArrStart).ToString("yyyy/MM/dd");
            string d4 = (MyUtility.Check.Empty(DateArrEnd)) ? "" : Convert.ToDateTime(DateArrEnd).ToString("yyyy/MM/dd");
            
            xl.DicDatas.Add("##QADate", d1 + "~" + d2);
            xl.DicDatas.Add("##ArriveDate", d3 + "~" + d4);
            xl.DicDatas.Add("##Category", Category);
            xl.DicDatas.Add("##Factory", factory);
            xl.DicDatas.Add("##Outstanding", OUTSTAN);

            SaveXltReportCls.XltRptTable xlTable = new SaveXltReportCls.XltRptTable(dt);
            xlTable.ShowHeader = false;
            xl.DicDatas.Add("##body", xlTable);

            xl.Save(Sci.Production.Class.MicrosoftFile.GetName("Quality_R04"));
            ((Microsoft.Office.Interop.Excel.Worksheet)xl.ExcelApp.ActiveSheet).Columns.AutoFit();
            xl.FinishSave();
            return true;
        }
    }
}
