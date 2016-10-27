using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Thread
{
    public partial class B05 : Sci.Win.Tems.Input6
    {
        private string keyWord = Sci.Env.User.Keyword;
        private DataTable gridTb;
        public B05(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.DefaultFilter = "category like '%Thread%'";

            InitializeComponent();
            Dictionary<String, String> comboBox1_RowSource2 = new Dictionary<string, string>();
            comboBox1_RowSource2.Add("ThreadColor", "Thread Color");
            comboBox1_RowSource2.Add("Location", "Thread Location");

            comboBox1.ValueMember = "Key";
            comboBox1.DisplayMember = "Value";
            comboBox1.DataSource = new BindingSource(comboBox1_RowSource2, null);
            dateRange1.TextBox1.Text = DateTime.Now.AddDays(-180).ToShortDateString();
            dateRange1.TextBox2.Text = DateTime.Now.ToShortDateString();
        }
        protected override DualResult OnDetailSelectCommandPrepare(Win.Tems.InputMasterDetail.PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["refno"].ToString();

            this.DetailSelectCommand = string.Format(@"select a.*,b.description as colordesc
            from threadstock a 
            left join threadcolor b on a.threadcolorid = b.id 
            where a.refno = '{0}' and mDivisionid = '{1}'", masterID, keyWord);

            string sql = @"Select cdate, id, '' as name, 0.0 as Newin,0.0 as Newout,0.0 as Newbalance, 0.0 as Usedin,0.0 as Usedout ,
                            0.0 as Usedbalance,'' as ThreadColorid,'' as ThreadLocationid, '' as editname 
                            from ThreadIncoming a where 1=0";
            DualResult sqlReault = DBProxy.Current.Select(null, sql, out gridTb);

            return base.OnDetailSelectCommandPrepare(e);
        }
        protected override bool OnGridSetup()
        {
             Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("Threadcolorid", header: "Thread Color", width: Widths.AnsiChars(15))
            .Text("colordesc", header: "Color Description", width: Widths.AnsiChars(20))
            .Text("ThreadLocationid", header: "Location", width: Widths.AnsiChars(10))
            .Numeric("NewCone", header: "New Cone", width: Widths.AnsiChars(5), integer_places: 5)
            .Numeric("UsedCone", header: "Used Cone", width: Widths.AnsiChars(5), integer_places: 5);

             Helper.Controls.Grid.Generator(this.grid1)
             .Date("cDate", header: "Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
             .Text("ID", header: "Transaction#", width: Widths.AnsiChars(13), iseditingreadonly: true)
             .Text("Name", header: "Name", width: Widths.AnsiChars(25), iseditingreadonly: true)
             .Numeric("Newin", header: "New Cone\nIn Qty", width: Widths.AnsiChars(5), integer_places: 9, iseditingreadonly: true)
             .Numeric("Newout", header: "New Cone\nOut Qty", width: Widths.AnsiChars(5), integer_places: 9, iseditingreadonly: true)
              .Numeric("Newbalance", header: "New Cone\nBalance", width: Widths.AnsiChars(5), integer_places: 9, iseditingreadonly: true)
             .Numeric("Usedin", header: "Used Cone\nIn Qty", width: Widths.AnsiChars(5), integer_places: 9, iseditingreadonly: true)
              .Numeric("Usedout", header: "Used Cone\nOut Qty", width: Widths.AnsiChars(5), integer_places: 9, iseditingreadonly: true)
             .Numeric("Usedbalance", header: "Used Cone\nBalance", width: Widths.AnsiChars(5), integer_places: 9, iseditingreadonly: true)
             .Text("editname", header: "Last update", width: Widths.AnsiChars(20), iseditingreadonly: true);
             return base.OnGridSetup();
        }
        protected override void OnRefreshClick()
        {
            int dgi = detailgrid.GetSelectedRowIndex();
            base.OnRefreshClick();
            detailgridbs.Filter = ""; //清空Filter
            dateRange1.TextBox1.Text = DateTime.Now.AddDays(-180).ToShortDateString();
            dateRange1.TextBox2.Text = DateTime.Now.ToShortDateString();
            transrecord(dateRange1.TextBox1.Text, dateRange1.TextBox2.Text);
            grid1.DataSource = gridTb; //因重新Generator 所以要重給
            detailgrid.SelectRowTo(dgi);
            OnDetailGridRowChanged();
        }
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            detailgridbs.Filter = ""; //清空Filter
            dateRange1.TextBox1.Text = DateTime.Now.AddDays(-180).ToShortDateString();
            dateRange1.TextBox2.Text = DateTime.Now.ToShortDateString();
            transrecord(dateRange1.TextBox1.Text, dateRange1.TextBox2.Text);
            grid1.DataSource = gridTb; //因重新Generator 所以要重給
            OnDetailGridRowChanged();
        }
        protected override void OnDetailGridRowChanged()
        {
            base.OnDetailGridRowChanged();

            int index = this.gridbs.Position;
            if (index == -1) index = 0;
            if (CurrentDetailData == null) return;
            gridTb.DefaultView.RowFilter = string.Format("ThreadColorid = '{0}' and ThreadLocationid = '{1}'", CurrentDetailData["ThreadColorid"], CurrentDetailData["ThreadLocationid"]);
        }

        private void initqty(string date2, int recal=0) //Init Qty
        {
            string date = Convert.ToDateTime(date2).AddDays(-1).ToShortDateString();
            if (recal == 1) date = date2;
            string sql = string.Format("dbo.usp_ThreadTransactionList @refno='{0}',@mDivisionid='{1}',@date1 = '{2}' ,@date2='{3}'", CurrentMaintain["Refno"], keyWord, "", date);
            DataTable tb,inittb;
            DualResult res = DBProxy.Current.Select(null, sql, out tb);
            MyUtility.Tool.ProcessWithDatatable(tb, "ThreadColorid,ThreadLocationid,Newin,NewOut,UsedIn,UsedOut,NewBalance,UsedBalance", @"Select ThreadColorid,ThreadLocationid,(isnull(sum(NewIn),0)-isnull(sum(NewOut),0)) as NewBalance,(isnull(sum(Usedin),0)-isnull(sum(UsedOut),0)) as UsedBalance from #tmp group by ThreadColorid,ThreadLocationid", out inittb);
            string updatestock = "";
            foreach (DataRow dr in inittb.Rows)
            {
                if (recal == 1)
                {
                    updatestock = updatestock + string.Format("Update ThreadStock set Newcone = {0},UsedCone = {1} where refno ='{2}' and ThreadColorid = '{3}' and ThreadLocationid ='{4}' and mDivisionid = '{5}';", dr["NewBalance"], dr["UsedBalance"], CurrentMaintain["Refno"], dr["ThreadColorid"], dr["ThreadLocationid"], keyWord);
                }
                else
                {
                    DataRow ndr = gridTb.NewRow();
                    ndr["Name"] = "Init";
                    ndr["ThreadColorid"] = dr["ThreadColorid"];
                    ndr["ThreadLocationid"] = dr["ThreadLocationid"];
                    ndr["Newbalance"] = dr["NewBalance"];
                    ndr["Usedbalance"] = dr["UsedBalance"];
                    gridTb.Rows.Add(ndr);
                }
            }
            if (recal == 1)
            {
                #region update Inqty,Status
                DualResult upResult;
                TransactionScope _transactionscope = new TransactionScope();
                using (_transactionscope)
                {
                    try
                    {
                        if (!(upResult = DBProxy.Current.Execute(null, updatestock)))
                        {
                            _transactionscope.Dispose();
                            MessageBox.Show("No data re-calculate ");
                            return;
                        }

                        _transactionscope.Complete();
                    }
                    catch (Exception ex)
                    {
                        _transactionscope.Dispose();
                        ShowErr("Commit transaction error.", ex);
                        return;
                    }
                }
                _transactionscope.Dispose();
                _transactionscope = null;
                #endregion
            }
        }

        private void transrecord(string date1, string date2)
        {
            initqty(date1); //計算Init
            string sql = string.Format("dbo.usp_ThreadTransactionList @refno='{0}',@mDivisionid='{1}',@date1 = '{2}' ,@date2='{3}'", CurrentMaintain["Refno"], keyWord, date1, date2);
            DataTable tb;
            DualResult res = DBProxy.Current.Select(null, sql, out tb);
            if (res)
            {
                decimal  newIn, newOut, usedIn, usedOut,newbal,usedbal, newbalance =0, usedbalance=0 ;
                gridTb=tb.Copy();
                foreach (DataRow drg in DetailDatas)
                {
                    newbalance = 0;
                    usedbalance = 0 ;
                    foreach (DataRow dr in gridTb.Rows)
                    {
                        if (drg["ThreadColorid"].ToString() != dr["ThreadColorid"].ToString() || drg["ThreadLocationid"].ToString() != dr["ThreadLocationid"].ToString()) continue; //group by 計算

                        newIn = 0;
                        newOut = 0;
                        usedIn = 0;
                        usedOut = 0;
                        newbal = 0;
                        usedbal = 0;
                        if (!MyUtility.Check.Empty(dr["newIn"])) newIn = (decimal)dr["newIn"];
                        if (!MyUtility.Check.Empty(dr["newOut"])) newOut = (decimal)dr["newOut"];
                        if (!MyUtility.Check.Empty(dr["usedIn"])) usedIn = (decimal)dr["usedIn"];
                        if (!MyUtility.Check.Empty(dr["usedOut"])) usedOut = (decimal)dr["usedOut"];
                        if (!MyUtility.Check.Empty(dr["Newbalance"])) newbal = (decimal)dr["Newbalance"];
                        if (!MyUtility.Check.Empty(dr["Usedbalance"])) usedbal = (decimal)dr["Usedbalance"];
                        newbalance = newbalance + newIn - newOut + newbal;
                        usedbalance = usedbalance + usedIn - usedOut + usedbal;
                        dr["Newbalance"] = newbalance;
                        dr["Usedbalance"] = usedbalance;
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            gridTb.Clear();
            transrecord(dateRange1.TextBox1.Text, dateRange1.TextBox2.Text);
            this.grid1.DataSource = gridTb;
            OnDetailGridRowChanged();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //移到指定那筆
            string refno = textBox1.Text;
            string filter = comboBox1.Text;
            if(filter =="Thread Color")
                detailgridbs.Filter = string.Format("ThreadColorid = '{0}'", refno);
            if (filter == "Thread Location")
                detailgridbs.Filter = string.Format("ThreadLocationid = '{0}'", refno);
            if (MyUtility.Check.Empty(refno)) detailgridbs.Filter = ""; //清空Filter
        }

        private void button2_Click(object sender, EventArgs e)
        {
            initqty(DateTime.Now.ToShortDateString(), 1);
            OnRefreshClick();
        }
    }
}
