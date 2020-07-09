using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Thread
{
    /// <summary>
    /// B05
    /// </summary>
    public partial class B05 : Win.Tems.Input6
    {
        private string keyWord = Env.User.Keyword;
        private DataTable gridTb;
        private bool gtbflag = false;

        /// <summary>
        /// B05
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public B05(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.DefaultFilter = "category like '%Thread%'";

            this.InitializeComponent();
            Dictionary<string, string> comboBox1_RowSource2 = new Dictionary<string, string>();
            comboBox1_RowSource2.Add("ThreadColor", "Thread Color");
            comboBox1_RowSource2.Add("Location", "Thread Location");

            this.comboThreadColorLocation.ValueMember = "Key";
            this.comboThreadColorLocation.DisplayMember = "Value";
            this.comboThreadColorLocation.DataSource = new BindingSource(comboBox1_RowSource2, null);
            this.dateTransactionDate.TextBox1.Text = DateTime.Now.AddDays(-180).ToShortDateString();
            this.dateTransactionDate.TextBox2.Text = DateTime.Now.ToShortDateString();
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["refno"].ToString();

            this.DetailSelectCommand = string.Format(
                @"select a.*,b.description as colordesc
            from threadstock a WITH (NOLOCK)
            left join threadcolor b WITH (NOLOCK)on a.threadcolorid = b.id 
            where a.refno = '{0}' ",
                masterID);

            // string sql = @"Select cdate, id, '' as name, 0.0 as Newin,0.0 as Newout,0.0 as Newbalance, 0.0 as Usedin,0.0 as Usedout ,
            //                            0.0 as Usedbalance,'' as ThreadColorid,'' as ThreadLocationid, '' as editname
            //                            from ThreadIncoming a where 1=0";
            //            DualResult sqlReault = DBProxy.Current.Select(null, sql, out gridTb);
            return base.OnDetailSelectCommandPrepare(e);
        }

        /// <inheritdoc/>
        protected override bool OnGridSetup()
        {
            this.Helper.Controls.Grid.Generator(this.detailgrid)
           .Text("Threadcolorid", header: "Thread Color", width: Widths.AnsiChars(15))
           .Text("colordesc", header: "Color Description", width: Widths.AnsiChars(20))
           .Text("ThreadLocationid", header: "Location", width: Widths.AnsiChars(10))
           .Numeric("NewCone", header: "New Cone", width: Widths.AnsiChars(5), integer_places: 5)
           .Numeric("UsedCone", header: "Used Cone", width: Widths.AnsiChars(5), integer_places: 5);

            this.Helper.Controls.Grid.Generator(this.grid1)
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

        /// <inheritdoc/>
        protected override void OnRefreshClick()
        {
            int dgi = this.detailgrid.GetSelectedRowIndex();

            // base.OnRefreshClick();
            this.RenewData();
            this.OnDetailEntered();

            // detailgridbs.Filter = ""; //清空Filter
            // dateRange1.TextBox1.Text = DateTime.Now.AddDays(-180).ToShortDateString();
            // dateRange1.TextBox2.Text = DateTime.Now.ToShortDateString();
            // transrecord(dateRange1.TextBox1.Text, dateRange1.TextBox2.Text);
            // grid1.DataSource = gridTb; //因重新Generator 所以要重給
            this.detailgrid.SelectRowTo(dgi);

            // OnDetailGridRowChanged();
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            string sql = @"Select cdate, id, '' as name, 0.0 as Newin,0.0 as Newout,0.0 as Newbalance, 0.0 as Usedin,0.0 as Usedout ,
                            0.0 as Usedbalance,'' as ThreadColorid,'' as ThreadLocationid, '' as editname 
                            from ThreadIncoming a WITH (NOLOCK)where 1=0";
            DualResult sqlReault = DBProxy.Current.Select(null, sql, out this.gridTb);
            this.grid1.DataSource = this.gridTb; // 因重新Generator 所以要重給
            this.detailgridbs.Filter = string.Empty; // 清空Filter
            this.dateTransactionDate.TextBox1.Text = DateTime.Now.AddDays(-180).ToShortDateString();
            this.dateTransactionDate.TextBox2.Text = DateTime.Now.ToShortDateString();
            this.Transrecord(this.dateTransactionDate.TextBox1.Text, this.dateTransactionDate.TextBox2.Text);
            this.OnDetailGridRowChanged();
            this.grid1.DataSource = this.gridTb; // 重新Binding Grid1
        }

        /// <inheritdoc/>
        protected override void OnDetailGridRowChanged()
        {
            base.OnDetailGridRowChanged();

            int index = this.gridbs.Position;
            if (index == -1)
            {
                index = 0;
            }

            if (this.CurrentDetailData == null || MyUtility.Check.Empty(this.gridTb))
            {
                return;
            }

            this.gridTb.DefaultView.RowFilter = string.Format("ThreadColorid = '{0}' and ThreadLocationid = '{1}'", this.CurrentDetailData["ThreadColorid"], this.CurrentDetailData["ThreadLocationid"]);
        }

        private void Initqty(string date2, int recal = 0) // Init Qty
        {
            string date = Convert.ToDateTime(date2).AddDays(-1).ToShortDateString();
            if (recal == 1)
            {
                date = date2;
            }

            string sql = string.Format("dbo.usp_ThreadTransactionList @refno='{0}' ,@date1 = '{1}' ,@date2='{2}'", this.CurrentMaintain["Refno"], string.Empty, date);
            DataTable tb, inittb;
            DualResult res = DBProxy.Current.Select(null, sql, out tb);
            MyUtility.Tool.ProcessWithDatatable(tb, "ThreadColorid,ThreadLocationid,Newin,NewOut,UsedIn,UsedOut,NewBalance,UsedBalance", @"Select ThreadColorid,ThreadLocationid,(isnull(sum(NewIn),0)-isnull(sum(NewOut),0)) as NewBalance,(isnull(sum(Usedin),0)-isnull(sum(UsedOut),0)) as UsedBalance from #tmp group by ThreadColorid,ThreadLocationid", out inittb);
            string updatestock = string.Empty;
            foreach (DataRow dr in inittb.Rows)
            {
                if (recal == 1)
                {
                    updatestock = updatestock + string.Format("Update ThreadStock set Newcone = {0},UsedCone = {1} where refno ='{2}' and ThreadColorid = '{3}' and ThreadLocationid ='{4}' ;", dr["NewBalance"], dr["UsedBalance"], this.CurrentMaintain["Refno"], dr["ThreadColorid"], dr["ThreadLocationid"]);
                }
                else
                {
                    DataRow ndr = this.gridTb.NewRow();
                    ndr["Name"] = "Init";
                    ndr["ThreadColorid"] = dr["ThreadColorid"];
                    ndr["ThreadLocationid"] = dr["ThreadLocationid"];
                    ndr["Newbalance"] = dr["NewBalance"];
                    ndr["Usedbalance"] = dr["UsedBalance"];
                    this.gridTb.Rows.Add(ndr);
                    if (!this.gtbflag)
                    {
                        this.gtbflag = true;
                    }
                }
            }

            if (recal == 1)
            {
                #region update Inqty,Status
                DualResult upResult;
                TransactionScope transactionscope = new TransactionScope();
                using (transactionscope)
                {
                    try
                    {
                        if (!(upResult = DBProxy.Current.Execute(null, updatestock)))
                        {
                            transactionscope.Dispose();
                            MessageBox.Show("No data re-calculate ");
                            return;
                        }

                        transactionscope.Complete();
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
                #endregion
            }
        }

        private void Transrecord(string date1, string date2)
        {
            this.Initqty(date1); // 計算Init
            string sql = string.Format("dbo.usp_ThreadTransactionList @refno='{0}',@date1 = '{1}' ,@date2='{2}'", this.CurrentMaintain["Refno"], date1, date2);
            DataTable tb;
            DualResult res = DBProxy.Current.Select(null, sql, out tb);
            if (res)
            {
                decimal newIn, newOut, usedIn, usedOut, newbal, usedbal, newbalance = 0, usedbalance = 0;
                string preThreadColorid = string.Empty;
                string preThreadLocationid = string.Empty;
                if (this.gtbflag)
                {
                    this.gridTb.Merge(tb);
                }
                else
                {
                    this.gridTb = tb.Copy();
                }

                this.gtbflag = false;

                var gridTbOrderBy = this.gridTb.AsEnumerable().OrderBy(s => s["ThreadColorid"]).ThenBy(s => s["ThreadLocationid"]).ThenBy(s => MyUtility.Check.Empty(s["cdate"]) ? DateTime.MinValue : (DateTime)s["cdate"]);

                foreach (var dr in gridTbOrderBy)
                {
                    if (dr["ThreadColorid"].Equals(preThreadColorid) || dr["ThreadLocationid"].Equals(preThreadLocationid))
                    {
                        newbalance = 0;
                        usedbalance = 0;
                        preThreadColorid = dr["ThreadColorid"].ToString();
                        preThreadLocationid = dr["ThreadLocationid"].ToString();
                    }

                    newIn = 0;
                    newOut = 0;
                    usedIn = 0;
                    usedOut = 0;
                    newbal = 0;
                    usedbal = 0;
                    if (!MyUtility.Check.Empty(dr["newIn"]))
                    {
                        newIn = (decimal)dr["newIn"];
                    }

                    if (!MyUtility.Check.Empty(dr["newOut"]))
                    {
                        newOut = (decimal)dr["newOut"];
                    }

                    if (!MyUtility.Check.Empty(dr["usedIn"]))
                    {
                        usedIn = (decimal)dr["usedIn"];
                    }

                    if (!MyUtility.Check.Empty(dr["usedOut"]))
                    {
                        usedOut = (decimal)dr["usedOut"];
                    }

                    if (!MyUtility.Check.Empty(dr["Newbalance"]))
                    {
                        newbal = (decimal)dr["Newbalance"];
                    }

                    if (!MyUtility.Check.Empty(dr["Usedbalance"]))
                    {
                        usedbal = (decimal)dr["Usedbalance"];
                    }

                    newbalance = newbalance + newIn - newOut + newbal;
                    usedbalance = usedbalance + usedIn - usedOut + usedbal;
                    dr["Newbalance"] = newbalance;
                    dr["Usedbalance"] = usedbalance;
                }
            }
        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            this.gridTb.Clear();
            this.Transrecord(this.dateTransactionDate.TextBox1.Text, this.dateTransactionDate.TextBox2.Text);
            this.grid1.DataSource = this.gridTb;
            this.OnDetailGridRowChanged();
        }

        private void BtnFilter_Click(object sender, EventArgs e)
        {
            // 移到指定那筆
            string refno = this.txtThreadColorLocation.Text;
            string filter = this.comboThreadColorLocation.Text;
            if (filter == "Thread Color")
            {
                this.detailgridbs.Filter = string.Format("ThreadColorid = '{0}'", refno);
            }

            if (filter == "Thread Location")
            {
                this.detailgridbs.Filter = string.Format("ThreadLocationid = '{0}'", refno);
            }

            if (MyUtility.Check.Empty(refno))
            {
                this.detailgridbs.Filter = string.Empty; // 清空Filter
            }
        }

        private void BtnRecalculateStockQty_Click(object sender, EventArgs e)
        {
            this.Initqty(DateTime.Now.ToShortDateString(), 1);
            this.OnRefreshClick();
        }
    }
}
