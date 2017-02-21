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
using sxrc = Sci.Utility.Excel.SaveXltReportCls;

namespace Sci.Production.Quality
{
    public partial class R04 : Sci.Win.Tems.PrintForm
    {   
        DateTime? DateRecStart; DateTime? DateRecEnd;
        DateTime? DateArrStart; DateTime? DateArrEnd;
        string Category; string factory; string Outstanding = "0"; string M;
        string OUTSTAN = "";
        List<SqlParameter> lis; DualResult res;
        DataTable dt; string cmd;
        public R04(ToolStripMenuItem menuitem)
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
            ORS.DefaultView.Sort = "Category";
            this.comboCategory.DataSource = ORS;
            this.comboCategory.ValueMember = "Category";
            this.comboCategory.DisplayMember = "Category";
            this.comboCategory.SelectedIndex = 0;
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
            bool DateReceived_empty = !DateReceivedSample.HasValue, DateArr_empty = !DateArriveWH.HasValue, Cate_comboBox_Empty = this.comboCategory.Text.Empty(), comboFactory_Empty = this.comboFactory.Text.Empty(),comboM_Empty=this.comboM.Text.Empty(),
                checkOuter_empty = checkOutstandingOnly.Checked.Empty();
            if (DateReceived_empty && DateArr_empty)
            {
                MyUtility.Msg.ErrorBox("Please select 'Received Sample Date' or 'Arrive W/H Date' at least one field entry");

                DateReceivedSample.Focus();
                return false;
            }
            if (checkOutstandingOnly.Checked == true)
            {
                Outstanding = "1";
                OUTSTAN = "YES";
            }
            else { OUTSTAN = "NO";
            Outstanding = "0";
            }
            lis.Add(new SqlParameter("@Outstanding", Outstanding));

            string sqlWhere = ""; string sqlRec = ""; string sqlArr = "";

            DateRecStart = DateReceivedSample.Value1;
            DateRecEnd = DateReceivedSample.Value2;
            DateArrStart = DateArriveWH.Value1;
            DateArrEnd = DateArriveWH.Value2;
            Category = comboCategory.Text;
            factory = comboFactory.Text;
            M = comboM.Text;
            List<string> sqlWheres = new List<string>();
            List<string> sqlRecDate = new List<string>();
            List<string> sqlArrDate = new List<string>();
            #region --組WHERE--

            if (!this.DateReceivedSample.Value1.Empty() && !this.DateReceivedSample.Value2.Empty())
            {
                sqlRecDate.Add("ReceiveSampleDate between @DateRecStart and @DateRecEnd");
                lis.Add(new SqlParameter("@DateRecStart", DateRecStart));
                lis.Add(new SqlParameter("@DateRecEnd", DateRecEnd));
            } if (!this.DateArriveWH.Value1.Empty() && !this.DateArriveWH.Value2.Empty())
            {
                sqlArrDate.Add("WhseArrival between @DateArrStart and @DateArrEnd");
                lis.Add(new SqlParameter("@DateArrStart", DateArrStart));
                lis.Add(new SqlParameter("@DateArrEnd", DateArrEnd));
            }
            if (!this.comboCategory.SelectedItem.ToString().Empty())
            {
                sqlWheres.Add("Category = @Cate");
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
            }
            

            if (!this.comboFactory.Text.ToString().Empty())
            {
                sqlWheres.Add("Factoryid = @Factory");
                lis.Add(new SqlParameter("@Factory", factory));
            }
            if (!this.comboM.Text.ToString().Empty())
            {
                sqlWheres.Add("MDivisionID=@MDivisionID");
                lis.Add(new SqlParameter("@MDivisionID", M));
            }

            #endregion
           
            sqlWhere = string.Join(" and ", sqlWheres);
            sqlRec = string.Join(" and ", sqlRecDate);
            sqlArr = string.Join(" and ", sqlArrDate);
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

            #region --撈ListExcel資料--

            cmd = string.Format(@" 
            with order_rawdata as
            (
	            select distinct poid from dbo.orders WITH (NOLOCK) 
	            where 1=1 and orders.Junk = 0
           " + sqlWhere + @"
            )
                select (select exportid from dbo.Receiving WITH (NOLOCK) where id = b.ReceivingID) [exportid]
                ,(select WhseArrival from dbo.Receiving WITH (NOLOCK) where id = b.ReceivingID) [WhseArrival]
                ,(select top 1 orders.FactoryID from orders WITH (NOLOCK) where id = b.POID) [factory]
                ,b.POID,b.seq1+'-'+b.seq2 [seq],c.ReceiveSampleDate
                ,(select suppid+'-'+supp.AbbEN from dbo.po_supp p WITH (NOLOCK) inner join dbo.supp WITH (NOLOCK) on supp.id = p.SuppID where p.id = b.POID and seq1 = b.seq1) [supplier]
                ,b.Refno
                ,(select p.ColorID from dbo.PO_Supp_Detail p WITH (NOLOCK) where p.id = b.POID and seq1 = b.seq1 AND seq2 = b.SEQ2) color
                ,(select top 1 orders.[category] from orders WITH (NOLOCK) where id = b.POID) [category]
                ,b.ArriveQty,oven_result.Result,c.Crocking,c.Heat,ColorFastness_result.Result,c.Wash
                from  FIR b WITH (NOLOCK) 
                inner join (select distinct a.id,a1.PoId,a1.Seq1,a1.seq2 from Receiving a WITH (NOLOCK) 
				                inner join Receiving_Detail a1 WITH (NOLOCK) on a1.Id = a.Id
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
                where 1=1 and (@Outstanding = 1 and (C.Crocking ='' or C.Wash ='' or C.Heat ='' or oven_result.Result='' or ColorFastness_result.Result=''))
                " + sqlRec+@"
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
            var saveDialog = Sci.Utility.Excel.MyExcelPrg.GetSaveFileDialog(Sci.Utility.Excel.MyExcelPrg.filter_Excel);
           // saveDialog.ShowDialog();
            //string outpath = saveDialog.FileName;
            //if (outpath.Empty())
            //{
            //    return false;
            //}
            
            Sci.Utility.Excel.SaveXltReportCls xl = new Sci.Utility.Excel.SaveXltReportCls("Quality_R04.xltx");
           // string xltPath = System.IO.Path.Combine(Env.Cfg.XltPathDir,"Quality_R04.xltx");
           // Microsoft.Office.Interop.Excel.Application xlt = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Quality_R04.xltx");//預先開啟excel app     
          
            string d1 = (MyUtility.Check.Empty(DateRecStart)) ? "" : Convert.ToDateTime(DateRecStart).ToString("yyyy/MM/dd");
            string d2 = (MyUtility.Check.Empty(DateRecEnd)) ? "" : Convert.ToDateTime(DateRecEnd).ToString("yyyy/MM/dd");
            string d3 = (MyUtility.Check.Empty(DateArrStart)) ? "" : Convert.ToDateTime(DateArrStart).ToString("yyyy/MM/dd");
            string d4 = (MyUtility.Check.Empty(DateArrEnd)) ? "" : Convert.ToDateTime(DateArrEnd).ToString("yyyy/MM/dd");
            //sxrc xl = new sxrc(xltPath);

            xl.dicDatas.Add("##QADate", d1 + "~" + d2);
            xl.dicDatas.Add("##ArriveDate", d3 + "~" + d4);
            xl.dicDatas.Add("##Category", Category);
            xl.dicDatas.Add("##Factory", factory);
            xl.dicDatas.Add("##Outstanding", OUTSTAN);
            xl.dicDatas.Add("##body", dt);
            //xl.Save(outpath, false);
            xl.Save();
            return true;
        }
    }
}
