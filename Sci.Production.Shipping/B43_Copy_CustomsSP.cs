using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;
using static Sci.Production.Shipping.Customs_WebAPI;

namespace Sci.Production.Shipping
{
    public partial class B43_Copy_CustomsSP : Win.Subs.Base
    {
        private DataRow drMaster;
        private ListCustomsCopyLoad dataAPI = new ListCustomsCopyLoad();
        private ListCustomsAllData dataSetAPI = new ListCustomsAllData();

        public B43_Copy_CustomsSP(DataRow drMain)
        {
            this.InitializeComponent();
            this.drMaster = drMain;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.txtSubconFromFty.Text = this.drMaster["SubconFromSystem"].ToString();
            this.txtSubconFromContract.Text = this.drMaster["SubconFromContractID"].ToString();

            this.txtSubconInFty.Text = MyUtility.GetValue.Lookup("select RgCode from system");
            this.txtSubconInContractNo.Text = this.drMaster["ID"].ToString();

            this.gridSubconFrom.IsEditingReadOnly = true;
            this.gridSubconFrom.DataSource = this.SubConFromDataBinding;
            this.Helper.Controls.Grid.Generator(this.gridSubconFrom)
                .Text("CustomSP", header: "Custom SP#", width: Widths.AnsiChars(8))
                .Text("Version", header: "Ver.", width: Widths.AnsiChars(7))
                .Date("CDate", header: "Date", width: Widths.AnsiChars(10))
                .Text("Category", header: "Category", width: Widths.AnsiChars(8))
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(8))
                .Text("Article", header: "Color Way", width: Widths.AnsiChars(16))
                .Text("SizeGroup", header: "Size Group", width: Widths.AnsiChars(16));

            this.gridSubconIn.IsEditingReadOnly = true;
            this.gridSubconIn.DataSource = this.SubConToDataBinding;
            this.Helper.Controls.Grid.Generator(this.gridSubconIn)
                .Text("CustomSP", header: "Custom SP#", width: Widths.AnsiChars(8))
                .Text("Version", header: "Ver.", width: Widths.AnsiChars(7))
                .Date("CDate", header: "Date", width: Widths.AnsiChars(10))
                .Text("Category", header: "Category", width: Widths.AnsiChars(8))
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(8))
                .Text("Article", header: "Color Way", width: Widths.AnsiChars(16))
                .Text("SizeGroup", header: "Size Group", width: Widths.AnsiChars(16))
                ;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnFind_Click(object sender, EventArgs e)
        {
            DataTable dt;
            string sqlcmd = $@"
select t.CustomSP
,t.Version
,t.CDate
,t.Category
,t.SizeCode
,Article = Article.value
,SizeGroup = SizeGroup.value
from VNConsumption T
outer apply(
	select value = Stuff((
		select concat(',',Article)
		from (
				select 	distinct Article
				from dbo.VNConsumption_Article s
				where s.id = t.ID
			) s
		for xml path ('')
	) , 1, 1, '')
)Article
outer apply(
	select value = Stuff((
		select concat(',',SizeCode)
		from (
				select 	distinct SizeCode
				from dbo.VNConsumption_SizeCode s
				where s.id = t.ID
			) s
		for xml path ('')
	) , 1, 1, '')
)SizeGroup
where StyleID = '{this.txtStyle.Text}' 
and BrandID='{this.txtbrand.Text}'
and VNContractID = '{this.txtSubconInContractNo.Text}'";

            DualResult result;

            if (!(result = DBProxy.Current.Select(string.Empty, sqlcmd, out dt)))
            {
                this.ShowErr(result);
                return;
            }
            else if (dt != null && dt.Rows.Count > 0)
            {
                this.gridSubconIn.DataSource = dt;
            }

            #region Get WebAPI Data
            DataColumn dc = new DataColumn();
            DataRow dr;
            DataTable dtAPI = new DataTable();
            dtAPI.Columns.Add("CustomSP", typeof(string));
            dtAPI.Columns.Add("Version", typeof(string));
            dtAPI.Columns.Add("CDate", typeof(DateTime));
            dtAPI.Columns.Add("Category", typeof(string));
            dtAPI.Columns.Add("SizeCode", typeof(string));
            dtAPI.Columns.Add("Article", typeof(string));
            dtAPI.Columns.Add("SizeGroup", typeof(string));

            if (Production.Shipping.Utility_WebAPI.IsSystemWebAPIEnable(this.txtSubconFromFty.Text, "VN"))
            {
                return;
            }

            Production.Shipping.Utility_WebAPI.GetCustomsCopyLoad(this.txtStyle.Text, this.txtbrand.Text, this.txtSubconFromContract.Text, out this.dataAPI);

            if (this.dataAPI != null && this.dataAPI.CustomsCopyLoadDt.Count > 0)
            {
                this.dataAPI.CustomsCopyLoadDt.ForEach((x) =>
                {
                    dr = dtAPI.NewRow();
                    dr["CustomSP"] = x.CustomSP;
                    dr["Version"] = x.Version;
                    dr["CDate"] = x.CDate;
                    dr["Category"] = x.Category;
                    dr["SizeCode"] = x.SizeCode;
                    dr["Article"] = x.Article;
                    dr["SizeGroup"] = x.SizeGroup;
                    dtAPI.Rows.Add(dr);
                });

                if (dtAPI != null && dtAPI.Rows.Count > 0)
                {
                    this.gridSubconFrom.DataSource = dtAPI;
                }

                #endregion
            }
        }

        /// <summary>
        /// Return underlying type if type is Nullable otherwise return the type
        /// </summary>
        public static Type GetCoreType(Type t)
        {
            if (t != null)
            {
                if (!t.IsValueType)
                {
                    return t;
                }
                else
                {
                    return Nullable.GetUnderlyingType(t);
                }
            }
            else
            {
                return t;
            }
        }

        private void BtnCopyCustomsSP_Click(object sender, EventArgs e)
        {
            string sqlcmd = $@"";

            if (Production.Shipping.Utility_WebAPI.IsSystemWebAPIEnable(this.txtSubconFromFty.Text, "VN"))
            {
                return;
            }

            DataSet ds = new DataSet();
            DataTable dtVNConsumption = new DataTable();
            DataTable dtVNConsumption_Detail = new DataTable();
            DataTable dtVNConsumption_Detail_Detail = new DataTable();
            DataTable dtVNConsumption_Article = new DataTable();
            DataTable dtVNConsumption_SizeCode = new DataTable();

            Production.Shipping.Utility_WebAPI.GetCustomsAllData(this.txtStyle.Text, this.txtbrand.Text, this.txtSubconFromContract.Text, out dataSetAPI);
            if (this.dataSetAPI != null)
            {
                if (this.dataSetAPI.GetCustomsAllDs.Tables["VNConsumption"].Rows.Count > 0)
                {
                    dtVNConsumption = this.dataSetAPI.GetCustomsAllDs.Tables["VNConsumption"].Copy();
                    dtVNConsumption_Detail = this.dataSetAPI.GetCustomsAllDs.Tables["VNConsumption_Detail"].Copy();
                    dtVNConsumption_Detail_Detail = this.dataSetAPI.GetCustomsAllDs.Tables["VNConsumption_Detail_Detail"].Copy();
                    dtVNConsumption_Article = this.dataSetAPI.GetCustomsAllDs.Tables["VNConsumption_Article"].Copy();
                    dtVNConsumption_SizeCode = this.dataSetAPI.GetCustomsAllDs.Tables["VNConsumption_SizeCode"].Copy();
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }

            TransactionScope transactionscope = new TransactionScope();
            SqlConnection sqlConn = null;
            DBProxy.Current.OpenConnection(null, out sqlConn);
            using (transactionscope)
            using (sqlConn)
            {
                try
                {
                    string sqlUpdate = string.Empty;
                    DualResult result;
                    DataTable resulttb;
                    sqlUpdate = $@"
delete t from VNConsumption t where exists(	select 1 from #tmp s where s.id = t.id)
insert into VNConsumption select * from #tmp";

                    if (!(result = MyUtility.Tool.ProcessWithDatatable(dtVNConsumption, string.Empty, sqlUpdate, out resulttb)))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(result);
                        return;
                    }

                    sqlUpdate = $@"
                    delete t from VNConsumption_Detail t where exists(	select 1 from #tmp s where s.id = t.id)
                    insert into VNConsumption_Detail select * from #tmp";
                    if (!(result = MyUtility.Tool.ProcessWithDatatable(dtVNConsumption_Detail, string.Empty, sqlUpdate, out resulttb)))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(result);
                        return;
                    }

                    sqlUpdate = $@"
                    delete t from VNConsumption_Article t where exists(	select 1 from #tmp s where s.id = t.id)
                    insert into VNConsumption_Article select * from #tmp";
                    if (!(result = MyUtility.Tool.ProcessWithDatatable(dtVNConsumption_Article, string.Empty, sqlUpdate, out resulttb)))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(result);
                        return;
                    }

                    sqlUpdate = $@"
                    delete t from VNConsumption_SizeCode t where exists(	select 1 from #tmp s where s.id = t.id)
                    insert into VNConsumption_SizeCode select * from #tmp";
                    if (!(result = MyUtility.Tool.ProcessWithDatatable(dtVNConsumption_SizeCode, string.Empty, sqlUpdate, out resulttb)))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(result);
                        return;
                    }

                    sqlUpdate = $@"
                    delete t from VNConsumption_Detail_Detail t where exists(	select 1 from #tmp s where s.id = t.id)

                    insert into VNConsumption_Detail_Detail(
                        [ID] ,[NLCode] ,[SCIRefno],[RefNo],[Qty],[LocalItem]
                        ,[FabricBrandID],[FabricType],[OldFabricUkey],[OldFabricVer],[SystemQty],[UserCreate],[StockQty]
                        ,[StockUnit],[HSCode],[UnitID],[UsageQty],[UsageUnit]) 
                    select [ID],[NLCode],[SCIRefno],[RefNo],[Qty],[LocalItem],[FabricBrandID],[FabricType]
                        ,[OldFabricUkey],[OldFabricVer],[SystemQty],[UserCreate],[StockQty],[StockUnit]
                        ,[HSCode],[UnitID],[UsageQty],[UsageUnit] from #tmp";
                    if (!(result = MyUtility.Tool.ProcessWithDatatable(dtVNConsumption_Detail_Detail, string.Empty, sqlUpdate, out resulttb)))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(result);
                        return;
                    }

                    transactionscope.Complete();
                    transactionscope.Dispose();
                    MyUtility.Msg.InfoBox("Copy Customs successful");
                }
                catch (Exception ex)
                {
                    transactionscope.Dispose();
                    this.ShowErr("Commit transaction error.", ex);
                    return;
                }
                finally
                {
                    transactionscope.Dispose();
                }
            }
        }
    }
}