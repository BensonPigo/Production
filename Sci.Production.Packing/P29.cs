using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Sci.Data;
using Ict;
using System.Transactions;

namespace Sci.Production.Packing
{
    /// <summary>
    /// P29
    /// </summary>
    public partial class P29 : Win.Tems.QueryForm
    {
        private DataTable dtMaster;
        private DataTable dtDetail;

        /// <summary>
        /// P29
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P29(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            #region GridMain Setting
            this.Helper.Controls.Grid.Generator(this.gridMain)
                .Date("PackingAuditDate", header: "Audit Date", iseditingreadonly: true)
                .Text("PackingListID", header: "Pack ID", iseditingreadonly: true)
                .Text("CTN", header: "CTN#", iseditingreadonly: true)
                .Numeric("Qty", header: "Qty", iseditingreadonly: true)
                .Numeric("Discrepancy", header: "Discrepancy", iseditingreadonly: true)
                .Text("SP", header: "SP#", iseditingreadonly: true)
                .Text("PO", header: "PO#", iseditingreadonly: true)
                .Text("Style", header: "Style#", iseditingreadonly: true)
                .Text("Brand", header: "Brand", iseditingreadonly: true)
                .Text("Destination", header: "Destination", iseditingreadonly: true)
                .Date("BuyerDelivery", header: "Buyer Delivery", iseditingreadonly: true)
                .Date("SCIDelivery", header: "SCI Delivery", iseditingreadonly: true)
                .Text("Barcode", header: "Barcode", iseditingreadonly: true)
                .Text("AuditBy", header: "Audit By", iseditingreadonly: true)
                .Text("AuditTime", header: "Audit Time", iseditingreadonly: true)
                .Date("PassDate", header: "Pass Date", iseditingreadonly: true)
                ;
            #endregion

            #region GridDetail Setting
            this.Helper.Controls.Grid.Generator(this.gridDetail)
                .Text("Description", header: "Error Description", iseditingreadonly: true)
                .Numeric("Qty", header: "Qty", iseditingreadonly: true)
                ;
            #endregion
        }

        private void ButtonFindNow_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.dateAuditDate.TextBox1.Value) &&
                MyUtility.Check.Empty(this.dateAuditDate.TextBox2.Value) &&
                MyUtility.Check.Empty(this.txtPackID.Text) &&
                MyUtility.Check.Empty(this.txtSP.Text))
            {
                MyUtility.Msg.WarningBox("Conditions cannot be all empty!!");
                return;
            }

            this.FindNow();
        }

        private void FindNow()
        {
            #region SQL Parameter
            List<SqlParameter> listSqlParameter = new List<SqlParameter>();
            listSqlParameter.Add(new SqlParameter("@PackID", this.txtPackID.Text));
            listSqlParameter.Add(new SqlParameter("@SP", this.txtSP.Text));
            listSqlParameter.Add(new SqlParameter("@PackingAuditDate_S", this.dateAuditDate.Value1.Empty() ? string.Empty : ((DateTime)this.dateAuditDate.Value1).ToString("yyyy/MM/dd 00:00:00")));
            listSqlParameter.Add(new SqlParameter("@PackingAuditDate_E", this.dateAuditDate.Value2.Empty() ? string.Empty : ((DateTime)this.dateAuditDate.Value2).ToString("yyyy/MM/dd 23:59:59")));
            #endregion

            #region BuyerDelivery Filte
            string strWhere = string.Empty;
            if (!this.dateAuditDate.Value1.Empty() && !this.dateAuditDate.Value2.Empty())
            {
                strWhere = " and c.PackingAuditDate between @PackingAuditDate_S and @PackingAuditDate_E";
            }
            else if (!this.dateAuditDate.Value1.Empty() && this.dateAuditDate.Value2.Empty())
            {
                strWhere = " and @PackingAuditDate_S <= c.PackingAuditDate";
            }
            else if (this.dateAuditDate.Value1.Empty() && !this.dateAuditDate.Value2.Empty())
            {
                strWhere = " and c.PackingAuditDate <= @PackingAuditDate_E";
            }

            if (!string.IsNullOrEmpty(this.txtPackID.Text))
            {
                strWhere += " and c.PackingListID = @PackID";
            }

            if (!string.IsNullOrEmpty(this.txtSP.Text))
            {
                strWhere += " and c.OrderID = @SP";
            }

            #endregion

            #region SQL Command
            string strSqlCmd = $@"
select ID, Name
into #mesPass1
from [ExtendServer].ManufacturingExecution.dbo.Pass1

select [PackingAuditDate] = c.PackingAuditDate
	,[PackingListID] = c.PackingListID
	,[CTN] = c.CTNStartNo
	,[Qty] = pd_QtyPerCTN.Qty
	,[Discrepancy] = c.Qty
	,[SP] = c.OrderID
	,[PO] = o.CustPONo
	,[Style] = o.StyleID
	,[Brand] = o.BrandID
	,[Destination] = co.Alias
	,[BuyerDelivery] = o.BuyerDelivery
	,[SCIDelivery] = o.SciDelivery
	,[Barcode] = pd_Barcode.Barcode
	,[AuditBy] = c.AddName + '-' + pass1.Name
	,[AuditTime] = Format(c.AddDate, 'yyyy/MM/dd HH:mm:ss')
	,[PassDate] = PassDate.AddDate
	,c.ID
into #tmp
from CTNPackingAudit c WITH(NOLOCK)
inner join Orders o WITH(NOLOCK) on c.OrderID = o.ID
left join Country co WITH(NOLOCK) on o.Dest = co.ID
left join #mesPass1 Pass1 on Pass1.ID = c.AddName
outer apply (
	select Qty = SUM(QtyPerCTN)
	from PackingList_Detail pd WITH(NOLOCK)
	where pd.SCICtnNo = c.SCICtnNo
)pd_QtyPerCTN
outer apply (
	SELECT Barcode = Stuff((
		select distinct concat('/',Barcode) 
		from PackingList_Detail pd WITH(NOLOCK)
		where pd.SCICtnNo = c.SCICtnNo
		FOR XML PATH(''))
	,1,1,'')
)pd_Barcode
outer apply(
	select top 1 AddDate 
	from CTNPackingAudit 
	where Status='Pass' 
	and AddDate> = c.AddDate
)PassDate
where 1 = 1
{strWhere}

select * from #tmp
order by PackingListID, CTN,PackingAuditDate

select cd.ID,cd.PackingReasonID,pr.Description,cd.Qty 
from CTNPackingAudit_Detail cd
inner join #tmp t on t.ID = cd.ID
left join PackingReason pr on cd.PackingReasonID = pr.ID and pr.Type = 'PA'

drop table #mesPass1
";
            #endregion

            this.ShowWaitMessage("Data Loading...");
            #region Set Grid Data
            DataSet datas = null;

            if (!PublicPrg.Prgs.SelectSet(string.Empty, strSqlCmd, out datas, listSqlParameter))
            {
                MyUtility.Msg.WarningBox(strSqlCmd, "DB error!!");
                return;
            }

            if (this.listControlBindingSource1.DataSource != null)
            {
                this.listControlBindingSource1.DataSource = null;
            }

            if (this.listControlBindingSource2.DataSource != null)
            {
                this.listControlBindingSource2.DataSource = null;
            }

            datas.Tables[0].AcceptChanges();
            datas.Tables[1].AcceptChanges();

            if (datas.Tables[0].Rows.Count == 0)
            {
                return;
            }

            this.dtMaster = datas.Tables[0];
            this.dtMaster.TableName = "Master";

            this.dtDetail = datas.Tables[1];
            this.dtDetail.TableName = "Detail";

            DataRelation relation = new DataRelation(
              "rel1",
              new DataColumn[] { this.dtMaster.Columns["ID"] },
              new DataColumn[] { this.dtDetail.Columns["ID"] });

            datas.Relations.Add(relation);

            this.listControlBindingSource1.DataSource = datas;
            this.listControlBindingSource1.DataMember = "Master";
            this.listControlBindingSource2.DataSource = this.listControlBindingSource1;
            this.listControlBindingSource2.DataMember = "rel1";
            this.gridMain.AutoResizeColumns();
            this.gridDetail.AutoResizeColumns();

            #endregion
            this.HideWaitMessage();
        }
    }
}
