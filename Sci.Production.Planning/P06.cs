using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Planning
{
    public partial class P06 : Sci.Win.Tems.QueryForm
    {
        private DataTable QuertData;

        public P06(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.comboColumnType.SelectedIndex = 0;
        }

        protected override void OnFormLoaded()
        {
            cellDropDownList dropdown = (cellDropDownList)cellDropDownList.GetGridCell("PMS_CriticalActivity");
            dropdown.CellValidating += (s, e) =>
            {
                
            };

            base.OnFormLoaded();
            this.grid.IsEditingReadOnly = false;
            this.grid.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.grid)
                .Text("OrderID", header: "SP#", width: Widths.Auto(), iseditingreadonly: true)
                .Text("BrandID", header: "Brand", width: Widths.Auto(), iseditingreadonly: true)
                .Text("StyleID", header: "Style#", width: Widths.Auto(), iseditingreadonly: true)
                .Date("SciDelivery", header: "SCI Dlv.", iseditingreadonly: true)
                .ComboBox("ColumnType", header: "Column Type", width: Widths.AnsiChars(20), settings: dropdown)
                .Date("OriginalDate", header: "Original Date", iseditingreadonly: true)
                .Date("NewTargetDate", header: "New Target Date", iseditingreadonly: true);

        }

        private void Query()
        {
            string strSCIDlvDateStart = this.dateSCIDlvDate.Value1.Empty() ? string.Empty : ((DateTime)this.dateSCIDlvDate.Value1).ToString("yyyy/MM/dd");
            string strSCIDlvDateEnd = this.dateSCIDlvDate.Value2.Empty() ? string.Empty : ((DateTime)this.dateSCIDlvDate.Value2).ToString("yyyy/MM/dd");

            #region SqlParameter
            List<SqlParameter> listSQLParameter = new List<SqlParameter>();
            listSQLParameter.Add(new SqlParameter("@OrderID", this.txtSPNo.Text));
            listSQLParameter.Add(new SqlParameter("@Brand", this.txtbrand.Text));
            listSQLParameter.Add(new SqlParameter("@Style", this.txtstyle.Text));
            listSQLParameter.Add(new SqlParameter("@SCIDlvStart", strSCIDlvDateStart));
            listSQLParameter.Add(new SqlParameter("@SCIDlvEnd", strSCIDlvDateEnd));
            listSQLParameter.Add(new SqlParameter("@ComboType", this.comboColumnType.Text));
            #endregion

            #region SQL Filter
            List<string> listSQLFilter = new List<string>();
            if (!MyUtility.Check.Empty(strSCIDlvDateStart)
                && !MyUtility.Check.Empty(strSCIDlvDateEnd))
            {
                listSQLFilter.Add("and o.SciDelivery between @SCIDlvStart and @SCIDlvEnd");
            }

            if (!MyUtility.Check.Empty(this.txtSPNo.Text))
            {
                listSQLFilter.Add("and o.id = @OrderID");
            }

            if (!MyUtility.Check.Empty(this.txtbrand.Text))
            {
                listSQLFilter.Add("and o.BrandID= @Brand");
            }

            if (!MyUtility.Check.Empty(this.txtstyle.Text))
            {
                listSQLFilter.Add("and o.StyleID= @Style");
            }

            if (!MyUtility.Check.Empty(this.comboColumnType.Text))
            {
                listSQLFilter.Add("and c.DropDownListID= @ComboType");
            }

            if (this.checkTargetDate.Checked)
            {
                listSQLFilter.Add("and c.TargetDate is not null");
            }
            #endregion

            this.ShowWaitMessage("Data Loading....");

            #region Sql Command

            string strCmd = $@"
select o.id as OrderID,o.BrandID,o.StyleID,o.SciDelivery ,[ColumnType] = c.DropDownListID
,[OriginalDate] = 
	case c.DropDownListID
	when 'Fabric Receiving' then FabricReceiving.KPILETA
	when 'Accessory Receiving' then AccessoryReceiving.KPILETA
	when 'Packing Material Receiving' then PackingMaterialRreceiving.KPILETA
	when 'Material Inspection Result' then MaterialInspection.date
	when 'Cutting Inline Date (Est.)' then CuttingInline.date
	when 'Factory PP Meeting' then FactoryPPMeeting.date
	when 'Wash Test Result Receiving' then WashTestResultReceiving.min_outputDate
	when 'Carton Finished' then CartonFinished.date
	else null
end
,[NewTargetDate] = c.TargetDate
from Orders o
left join Style s on o.StyleUkey=s.Ukey
left join CriticalActivity c on o.ID=c.orderid
outer apply(
	select o.KPILETA 
	where exists (select * from dbo.PO_Supp_Detail p WITH (NOLOCK) 
	inner join dbo.fabric f on f.SCIRefno=p.SCIRefno 
	inner join MtlType m on m.id = f.MtlTypeID  
	where m.IssueType!='Packing' and p.id = o.POID and p.FabricType = 'F' )
) FabricReceiving
outer apply(
	select o.KPILETA 
	where exists (select * from dbo.PO_Supp_Detail p WITH (NOLOCK) 
	inner join dbo.fabric f WITH (NOLOCK) on f.SCIRefno=p.SCIRefno 
	inner join MtlType m WITH (NOLOCK) on m.id = f.MtlTypeID  
	where m.IssueType!='Packing' and p.id = o.POID and p.FabricType!='F' )
) AccessoryReceiving
outer apply(
	select o.KPILETA where exists (select * from dbo.PO_Supp_Detail p WITH (NOLOCK) 
	inner join dbo.fabric f WITH (NOLOCK) on f.SCIRefno=p.SCIRefno 
	inner join MtlType m WITH (NOLOCK) on m.id = f.MtlTypeID  
	where m.IssueType='Packing' and p.id = o.POID )
) PackingMaterialRreceiving
outer apply(
	select min(date) date
	from dbo.WorkHour a WITH (NOLOCK) 
	where FactoryID = o.FactoryID and a.Hours > 0 and a.date>=dateadd(day,5,o.MTLETA) and	Holiday=0
) MaterialInspection
outer apply(
	select min(date) date from dbo.WorkHour a WITH (NOLOCK) 
	where FactoryID = o.FactoryID and a.Hours > 0 and a.date>=dateadd(day,5,o.MTLETA) and Holiday=0
) CuttingInline 
outer apply(
	select max(date) date from dbo.WorkHour a WITH (NOLOCK) 
	where FactoryID = o.FactoryID and a.Hours > 0 and a.date<=dateadd(day,-1,o.Sewinline) 
	and Holiday=0 and s.NoNeedPPMeeting=1
) FactoryPPMeeting
outer apply(
	select  min(s.OutputDate) min_outputDate
	from dbo.system WITH (NOLOCK) ,dbo.GarmentTest g WITH (NOLOCK) 
	INNER JOIN dbo.GarmentTest_Detail gd WITH (NOLOCK) on gd.ID = g.ID
	left join (dbo.SewingOutput_Detail_Detail sdd WITH (NOLOCK) 
	inner join dbo.SewingOutput s WITH (NOLOCK) on s.id = sdd.ID)  on sdd.OrderId = g.orderid and sdd.Article = g.Article
	where g.OrderID = o.ID
)WashTestResultReceiving 
outer apply(
	select min(date) date 
	from dbo.WorkHour a WITH (NOLOCK) 
	where FactoryID = o.FactoryID and a.Hours > 0 and a.date>=dateadd(day,1,o.SewOffLine) and Holiday=0
) CartonFinished

where 1=1
and o.MDivisionID = '{Sci.Env.User.Keyword}'
and o.Junk = 0
and o.Qty > 0
{listSQLFilter.JoinToString($"{Environment.NewLine} ")}
";
            #endregion
            DualResult result = DBProxy.Current.Select("", strCmd, listSQLParameter, out this.QuertData);

            if (!result)
            {
                MyUtility.Msg.WarningBox(result.ToString());
            }
            else if (this.QuertData.Rows.Count < 1)
            {
                this.listControlBindingSource1.DataSource = null;
                MyUtility.Msg.InfoBox("Data not found !");
            }
            else
            {
                this.listControlBindingSource1.DataSource = this.QuertData;
            }

            this.HideWaitMessage();
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtSPNo.Text) &&
                MyUtility.Check.Empty(this.txtbrand.Text) &&
                MyUtility.Check.Empty(this.txtstyle.Text) &&
                MyUtility.Check.Empty(this.comboColumnType.Text) &&
                !this.checkTargetDate.Checked)
            {
                MyUtility.Msg.WarningBox("query condition can not all is empty, you must enter at least one !!");
                return;
            }

            this.Query();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

        }
    }
}
