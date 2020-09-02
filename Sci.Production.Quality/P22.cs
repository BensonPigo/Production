using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Quality
{
    public partial class P22 : Win.Tems.QueryForm
    {
        private DataTable dtDBSource;
        private string excelFile;

        public P22(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.grid.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.grid)
               .CheckBox("CFANeedInsp", header: string.Empty, trueValue: 1, falseValue: 0, iseditable: true)
               .Text("ID", header: "Pack ID", width: Widths.AnsiChars(15), iseditingreadonly: true)
               .Text("CTNStartNo", header: "CTN#", width: Widths.AnsiChars(7), iseditingreadonly: true)
               .Text("OrderID", header: "SP#", width: Widths.AnsiChars(15), iseditingreadonly: true)
               .Text("CustPoNo", header: "PO#", width: Widths.AnsiChars(13), iseditingreadonly: true)
               .Text("StyleID", header: "Style", width: Widths.AnsiChars(13), iseditingreadonly: true)
               .Text("SeasonID", header: "Season", width: Widths.AnsiChars(7), iseditingreadonly: true)
               .Text("BrandID", header: "Brand", width: Widths.AnsiChars(10), iseditingreadonly: true)
               .Text("Article", header: "Colorway", width: Widths.AnsiChars(8), iseditingreadonly: true)
               .Text("Color", header: "Color", width: Widths.AnsiChars(7), iseditingreadonly: true)
               .Text("SizeCode", header: "Size", width: Widths.AnsiChars(10), iseditingreadonly: true)
               .Text("QtyPerCTN", header: "Qty", width: Widths.AnsiChars(10), iseditingreadonly: true)
               .Text("Alias", header: "Destination", width: Widths.AnsiChars(7), iseditingreadonly: true)
               .Date("BuyerDelivery", header: "Buyer Delivery", width: Widths.AnsiChars(13), iseditingreadonly: true)
               .Date("ClogCFM", header: "CLOG CFM", width: Widths.AnsiChars(13), iseditingreadonly: true)
               .Text("ClogLocationID", header: "Location No", width: Widths.AnsiChars(10), iseditingreadonly: true)
               .Text("Remark", header: "Remark", width: Widths.AnsiChars(15), iseditingreadonly: true);
        }

        private void Find()
        {
            string strSciDeliveryStart = this.dateRangeSCIDelivery.Value1.Empty() ? string.Empty : ((DateTime)this.dateRangeSCIDelivery.Value1).ToString("yyyy/MM/dd");
            string strSciDeliveryEnd = this.dateRangeSCIDelivery.Value2.Empty() ? string.Empty : ((DateTime)this.dateRangeSCIDelivery.Value2).ToString("yyyy/MM/dd");
            #region SqlParameter
            List<SqlParameter> listSQLParameter = new List<SqlParameter>();
            listSQLParameter.Add(new SqlParameter("@OrderID", this.txtSPNo.Text));
            listSQLParameter.Add(new SqlParameter("@PoNo", this.txtPoNo.Text));
            listSQLParameter.Add(new SqlParameter("@PackID", this.txtPackID.Text));
            listSQLParameter.Add(new SqlParameter("@SciDeliveryStart", strSciDeliveryStart));
            listSQLParameter.Add(new SqlParameter("@SciDeliveryEnd", strSciDeliveryEnd));
            #endregion

            #region SQL Filter
            List<string> listSQLFilter = new List<string>();
            if (!MyUtility.Check.Empty(strSciDeliveryStart)
                && !MyUtility.Check.Empty(strSciDeliveryEnd))
            {
                listSQLFilter.Add("and o.SciDelivery between @SciDeliveryStart and @SciDeliveryEnd");
            }

            if (!MyUtility.Check.Empty(this.txtSPNo.Text))
            {
                listSQLFilter.Add("and o.id = @OrderID");
            }

            if (!MyUtility.Check.Empty(this.txtPoNo.Text))
            {
                listSQLFilter.Add("and o.CustPoNo= @PoNo");
            }

            if (!MyUtility.Check.Empty(this.txtPackID.Text))
            {
                listSQLFilter.Add("and p2.id= @PackID");
            }

            #endregion

            #region Sql Command

            string strCmd = $@"
select distinct
CFANeedInsp
,p2.ID
,p2.CTNStartNo
,o1.OrderID 
,o.CustPONo
,o.StyleID
,o.SeasonID
,o.BrandID
,p2.Article
,p2.Color
,s.SizeCode
,q.QtyPerCTN
,c.Alias
,o.BuyerDelivery
,p2.ClogLocationId
,p2.remark
,p2.Seq
,p2.ReceiveDate as ClogCFM
from PackingList_Detail p2 WITH (NOLOCK)
inner join PackingList p1 WITH (NOLOCK) on p2.id=p1.id
left join Pullout po WITH (NOLOCK) on po.ID=p1.PulloutID
inner join orders o WITH (NOLOCK) on o.id	= p2.orderid
left join Country c WITH (NOLOCK) on c.id=o.dest
outer apply(
	select sizecode = stuff((
		select concat('/',Sizecode)
		from (
			select distinct sizecode 
			from PackingList_Detail pd WITH (NOLOCK)
			where p2.id=pd.id and p2.CTNStartNo = pd.CTNStartNo			
		) s
		outer apply (
			select seq from Order_SizeCode WITH (NOLOCK)
			where sizecode = s.sizecode and id=o.poid
		)s2
		order by s2.Seq 
		for xml path('')
	),1,1,'')
) s
outer apply(
	select QtyPerCTN = stuff((
		select concat('/',QtyPerCTN)
		from (
			select distinct QtyPerCTN,sizecode 
			from PackingList_Detail pd WITH (NOLOCK)
			where p2.id=pd.id and p2.CTNStartNo = pd.CTNStartNo			
		) q
		outer apply (
			select seq from Order_SizeCode WITH (NOLOCK)
			where sizecode = q.sizecode and id=o.poid
		)s2
		order by s2.Seq 
		for xml path('')
	),1,1,'')
) q
outer apply(
	select OrderID = stuff((
		select concat('/',OrderID)
		from (
			select distinct OrderID from PackingList_Detail pd WITH (NOLOCK)
			where p2.orderid=pd.orderid
		) o1
		for xml path('')
	),1,1,'')
) o1
where p2.CTNStartNo<>''
and p1.Mdivisionid='{Env.User.Keyword}'
and p1.Type in ('B','L')
and p2.TransferCFADate is null
and p2.DisposeFromClog= 0
and (po.Status ='New' or po.Status is null)
{listSQLFilter.JoinToString($"{Environment.NewLine} ")}
order by p2.ID,p2.Seq";
            #endregion
            DualResult result = DBProxy.Current.Select(string.Empty, strCmd, listSQLParameter, out this.dtDBSource);

            if (!result)
            {
                MyUtility.Msg.WarningBox(result.ToString());
            }
            else if (this.dtDBSource.Rows.Count < 1)
            {
                this.listControlBindingSource.DataSource = null;
                MyUtility.Msg.InfoBox("Data not found !");
            }
            else
            {
                DataTable dt = this.dtDBSource.Copy();
                this.listControlBindingSource.DataSource = dt;
                this.Grid_Filter();
            }
        }

        private void BtnFind_Click(object sender, EventArgs e)
        {
            this.ShowWaitMessage("Data Loading...");
            this.Find();
            this.HideWaitMessage();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            DualResult resule;
            DataTable dt = (DataTable)this.listControlBindingSource.DataSource;
            DataTable dtToExcel;
            if (MyUtility.Check.Empty(dt))
            {
                return;
            }

            this.ShowWaitMessage("Data Processing ...");

            string updateSqlCmd = string.Empty;

            /*
             透過ProcessWithDatatable將表身table與DB比對不同
             有不同資料的再跑迴圈
            */
            DataTable selectData = null;
            MyUtility.Tool.ProcessWithDatatable(dt, string.Empty, @"
select distinct a.* from #tmp a
inner join PackingList_Detail b on a.id=b.id and a.ctnstartno=b.ctnstartno
where a.CFANeedInsp <> b.CFANeedInsp and b.DisposeFromClog= 0", out selectData);

            foreach (DataRow dr in selectData.Rows)
            {
                int CFANeedInsp = dr["CFANeedInsp"].ToString() == "True" ? 1 : 0;
                updateSqlCmd = updateSqlCmd + $@"
update PackingList_Detail 
set CFANeedInsp ={CFANeedInsp} ,CFASelectInspDate = GETDATE()
where id='{dr["id"]}'
and DisposeFromClog= 0
and CTNStartNo ='{dr["CTNStartNo"]}'
";
            }

            if (updateSqlCmd.Length > 0)
            {
                TransactionScope transactionscope = new TransactionScope();
                using (transactionscope)
                {
                    try
                    {
                        if (!(resule = DBProxy.Current.Execute(null, updateSqlCmd.ToString())))
                        {
                            transactionscope.Dispose();
                            this.ShowErr(updateSqlCmd.ToString(), resule);
                            return;
                        }

                        transactionscope.Complete();
                        transactionscope.Dispose();
                        MyUtility.Msg.InfoBox("Save successful!");
                    }
                    catch (Exception ex)
                    {
                        transactionscope.Dispose();
                        this.ShowErr("Commit transaction error.", ex);
                        return;
                    }
                }

                transactionscope.Dispose();
                transactionscope = null;
            }

            this.HideWaitMessage();

            // 變更比對用的Datatable
            this.dtDBSource.Clear();
            this.dtDBSource = ((DataTable)this.listControlBindingSource.DataSource).Copy();

            #region to Excel

            MyUtility.Tool.ProcessWithDatatable(selectData, string.Empty, @"
select * from #tmp 
where CFANeedInsp = 1 ", out dtToExcel);
            if (dtToExcel != null && dtToExcel.Rows.Count > 0)
            {
                Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Quality_P22.xltx"); // 預先開啟excel app;
                MyUtility.Excel.CopyToXls(dtToExcel, string.Empty, "Quality_P22.xltx", 2, false, null, objApp);
                Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
                objApp.Cells.EntireColumn.AutoFit();    // 自動欄寬
                objApp.Cells.EntireRow.AutoFit();       ////自動欄高
                Microsoft.Office.Interop.Excel.Range rangeColumnA = objSheets.Columns["A", Type.Missing];
                rangeColumnA.Hidden = true;
                this.excelFile = Class.MicrosoftFile.GetName("Quality_P22");
                Microsoft.Office.Interop.Excel.Workbook workbook = objApp.ActiveWorkbook;
                workbook.SaveAs(this.excelFile);
                workbook.Close();
                objApp.Quit();
                Marshal.ReleaseComObject(objApp);
                Marshal.ReleaseComObject(workbook);

                DataRow dr;
                if (MyUtility.Check.Seek($@"select * from MailTo where id='015'", out dr))
                {
                    string userEmail = MyUtility.GetValue.Lookup($@"select email from pass1 where id='{Env.User.UserID}'");

                    var email = new MailTo(Env.Cfg.MailFrom, dr["ToAddress"].ToString(), dr["CCAddress"].ToString() + ";" + userEmail, "[" + DateTime.Now.ToString("yyyy-MM-dd") + "] " + dr["Subject"].ToString(), this.excelFile, dr["Content"].ToString(), false, true);
                    email.ShowDialog(this);
                }
                else
                {
                    MyUtility.Msg.WarningBox("MailTo #15 not yet to setting!");
                }
            }
            #endregion
        }

        private void BtnColse_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CheckCartonsInClog_CheckedChanged(object sender, EventArgs e)
        {
            this.Grid_Filter();
        }

        private void Grid_Filter()
        {
            DataTable dt = (DataTable)this.listControlBindingSource.DataSource;

            if (!MyUtility.Check.Empty(dt) && dt.Rows.Count > 0)
            {
                string filter = string.Empty;
                switch (this.checkCartonsInClog.Checked)
                {
                    case false:
                        if (MyUtility.Check.Empty(this.grid))
                        {
                            break;
                        }

                        filter = string.Empty;
                        ((DataTable)this.listControlBindingSource.DataSource).DefaultView.RowFilter = filter;
                        break;

                    case true:
                        if (MyUtility.Check.Empty(this.grid))
                        {
                            break;
                        }

                        filter = " ClogCFM is not null ";
                        ((DataTable)this.listControlBindingSource.DataSource).DefaultView.RowFilter = filter;
                        break;
                }
            }
        }
    }
}
