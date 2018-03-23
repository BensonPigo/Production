using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Quality
{
    public partial class P22 : Sci.Win.Tems.QueryForm
    {
        private DataTable dtDBSource;
        public P22(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();            
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.grid.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.grid)                
               .CheckBox("CFANeedInsp", header: "", trueValue: 1, falseValue: 0,iseditable:true)
               .Text("ID", header: "Pack ID", width: Widths.AnsiChars(15), iseditingreadonly: true)
               .Text("CTNStartNo", header: "CTN#", width: Widths.AnsiChars(7), iseditingreadonly: true)
               .Text("OrderID", header: "SP#", width: Widths.AnsiChars(15), iseditingreadonly: true)
               .Text("CustPoNo", header: "PO#", width: Widths.AnsiChars(13), iseditingreadonly: true)
               .Text("StyleID", header: "Style", width: Widths.AnsiChars(13), iseditingreadonly: true)
               .Text("SeasonID", header: "Season", width: Widths.AnsiChars(7), iseditingreadonly: true)
               .Text("BrandID", header: "Brand", width: Widths.AnsiChars(10), iseditingreadonly: true)
               .Text("Article", header: "Colorway", width: Widths.AnsiChars(8), iseditingreadonly: true)
               .Text("Color", header: "Color", width: Widths.AnsiChars(7), iseditingreadonly: true)
               .Text("SizeCode", header: "Size", width: Widths.AnsiChars(7), iseditingreadonly: true)
               .Numeric("QtyPerCTN", header: "Qty", width: Widths.AnsiChars(8), iseditingreadonly: true)
               .Text("Alias", header: "Destination", width: Widths.AnsiChars(7), iseditingreadonly: true)
               .Date("BuyerDelivery", header: "Buyer Delivery", width: Widths.AnsiChars(13), iseditingreadonly: true)
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
			select distinct QtyPerCTN from PackingList_Detail pd WITH (NOLOCK)
			where p2.id=pd.id and p2.CTNStartNo = pd.CTNStartNo			
		) q
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
and p1.Mdivisionid='{Sci.Env.User.Keyword}'
and p1.Type in ('B','L')
and p2.ReceiveDate is not null
and p2.TransferCFADate is null
and (po.Status ='New' or po.Status is null)
{listSQLFilter.JoinToString($"{Environment.NewLine} ")}
order by p2.ID,p2.CTNStartNo";
            #endregion            
            DualResult result = DBProxy.Current.Select("", strCmd, listSQLParameter, out dtDBSource);

            if (!result)
            {
                MyUtility.Msg.WarningBox(result.ToString());
            }
            else if (dtDBSource.Rows.Count < 1)
            {
                this.listControlBindingSource.DataSource = null;
                MyUtility.Msg.InfoBox("Data not found !");
            }
            else
            {
                DataTable dt = dtDBSource.Copy();
                this.listControlBindingSource.DataSource = dt;
            }
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            this.ShowWaitMessage("Data Loading...");
            Find();
            this.HideWaitMessage();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            DualResult resule;
            DataTable dt = (DataTable)this.listControlBindingSource.DataSource;
            if (MyUtility.Check.Empty(dt))
            {
                return;
            }
            this.ShowWaitMessage("Data Processing ...");

            string updateSqlCmd = string.Empty;

            foreach (DataRow dr in dt.Rows)
            {
                DataRow[] selectData = dtDBSource.Select($@"CFANeedInsp <> {dr["CFANeedInsp"]} and id='{dr["ID"]}' and CTNStartNo='{dr["CTNStartNo"]}'");
                if (selectData.Length > 0)
                {
                    int CFANeedInsp = dr["CFANeedInsp"].ToString() == "True" ? 1 : 0;
                    updateSqlCmd = updateSqlCmd + $@"
update PackingList_Detail 
set CFANeedInsp ={CFANeedInsp}
where id='{dr["id"]}'
and CTNStartNo ='{dr["CTNStartNo"]}'
";                   
                }
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
                            ShowErr(updateSqlCmd.ToString(), resule);
                            return;
                        }
                        transactionscope.Complete();
                        transactionscope.Dispose();
                        MyUtility.Msg.InfoBox("Save successful!");
                    }
                    catch (Exception ex)
                    {
                        transactionscope.Dispose();
                        ShowErr("Commit transaction error.", ex);
                        return;
                    }
                }
                transactionscope.Dispose();
                transactionscope = null;
            }
            this.HideWaitMessage();

            Find();
        }

        private void btnColse_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
