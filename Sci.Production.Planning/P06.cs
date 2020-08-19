using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Class;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Planning
{
    /// <inheritdoc/>
    public partial class P06 : Win.Tems.QueryForm
    {
        private DataTable QuertData;

        /// <summary>
        /// Initializes a new instance of the <see cref="P06"/> class.
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public P06(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.EditMode = true;
            this.gridIcon1.Insert.Visible = false;
            DualResult result;
            string sqlcmd = @"
select id='',name='' ,seq=0
union all
select ID
       , Name = rtrim(Name),Seq
from DropDownList WITH (NOLOCK) 
where Type = 'PMS_CriticalActivity' 
order by Seq";
            if (result = DBProxy.Current.Select(null, sqlcmd, out DataTable dt))
            {
                this.comboColumnType.DataSource = dt;
                this.comboColumnType.DisplayMember = "Name";
                this.comboColumnType.ValueMember = "ID";
            }
            else
            {
                this.ShowErr(result);
            }

            this.comboColumnType.SelectedIndex = 0;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            CellDropDownList dropdown = (CellDropDownList)CellDropDownList.GetGridCell("PMS_CriticalActivity");
            dropdown.EditingControlShowing += (s, e) =>
            {
                if (s == null || e == null)
                {
                    return;
                }

                var eventArgs = (Ict.Win.UI.DataGridViewEditingControlShowingEventArgs)e;
                ComboBox cb = eventArgs.Control as ComboBox;
                if (cb != null)
                {
                    cb.SelectionChangeCommitted -= this.Combo_SelectionChangeCommitted;
                    cb.SelectionChangeCommitted += this.Combo_SelectionChangeCommitted;
                }
            };
            base.OnFormLoaded();
            this.grid.IsEditingReadOnly = false;
            this.grid.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.grid)
                .Text("OrderID", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .CheckBox("Junk", header: "Cancel Order", iseditable: false)
                .Text("BrandID", header: "Brand", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("StyleID", header: "Style#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Date("SciDelivery", header: "SCI Dlv.", iseditingreadonly: true)
                .ComboBox("ColumnType", header: "Column Type", width: Widths.AnsiChars(20), settings: dropdown)
                .Date("OriginalDate", header: "Original Date", iseditingreadonly: true)
                .Date("NewTargetDate", header: "New Target Date", iseditingreadonly: false);

            this.grid.Columns["NewTargetDate"].DefaultCellStyle.BackColor = Color.Pink;
        }

        private void Combo_SelectionChangeCommitted(object sender, EventArgs e)
        {
            string newValue = ((Ict.Win.UI.DataGridViewComboBoxEditingControl)sender).EditingControlFormattedValue.ToString();
            DataRow dr = this.grid.GetDataRow(this.listControlBindingSource1.Position);
            DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
            DataRow[] drSame = dt.Select($@" orderID='{dr["OrderID"]}' and ColumnType = '{newValue}'");
            if (drSame.Length > 0)
            {
                MyUtility.Msg.WarningBox("<OrderID> and <Column Type> cannot be duplicate!");
                dr["ColumnType"] = string.Empty;
                dr["OriginalDate"] = DBNull.Value;
                dr["NewTargetDate"] = DBNull.Value;
            }
            else
            {
                string sqlcmd = $@"
select [OriginalDate] = 
	case '{newValue}'
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
,[NewTargetDate] = (select TargetDate from CriticalActivity where OrderID = o.id and DropDownListID='{newValue}')
from Orders o
left join Style s on o.StyleUkey=s.Ukey
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
where o.id='{dr["OrderID"]}' 
";
                DataRow drOriginalDate;
                if (MyUtility.Check.Seek(sqlcmd, out drOriginalDate))
                {
                    dr["OriginalDate"] = drOriginalDate["OriginalDate"];
                    dr["NewTargetDate"] = drOriginalDate["NewTargetDate"];
                }

                dr["ColumnType"] = newValue;
            }

            dr.EndEdit();
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

            if (!this.chkIncludeCancelOrder.Checked)
            {
                listSQLFilter.Add("and (o.Junk=0 or (o.Junk=1 and o.NeedProduction=1))");
            }
            #endregion

            this.ShowWaitMessage("Data Loading....");

            #region Sql Command

            string strCmd = $@"

select  o.id as OrderID,o.Poid,o.BrandID,o.StyleID,o.SciDelivery ,[ColumnType] = c.DropDownListID
,[NewTargetDate] = c.TargetDate
,o. KPILETA ,o.FactoryID,o.MTLETA,o.Sewinline,s.NoNeedPPMeeting,o.SewOffLine, o.Junk
into #tmp
from Orders o
left join Style s on o.StyleUkey=s.Ukey
left join CriticalActivity c on o.ID=c.orderid
where 1=1
and o.MDivisionID = '{Env.User.Keyword}'
and o.LocalOrder = 0
{listSQLFilter.JoinToString($"{Environment.NewLine} ")}


select t.OrderID,Poid,BrandID,StyleID,SciDelivery , t.Junk,isnull(ColumnType,'') as ColumnType
,[OriginalDate] =
	case ColumnType
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
, NewTargetDate
from #tmp t
outer apply(
	select t.KPILETA 
	where exists (select 1 from dbo.PO_Supp_Detail p WITH (NOLOCK) 
	inner join dbo.fabric f on f.SCIRefno=p.SCIRefno 
	inner join MtlType m on m.id = f.MtlTypeID  
	where p.id = POID
	and m.IssueType!='Packing' and  p.FabricType = 'F' 
	)
) FabricReceiving
outer apply(
	select t.KPILETA 
	where exists (select 1 from dbo.PO_Supp_Detail p WITH (NOLOCK) 
	inner join dbo.fabric f WITH (NOLOCK) on f.SCIRefno=p.SCIRefno 
	inner join MtlType m WITH (NOLOCK) on m.id = f.MtlTypeID  
	where m.IssueType!='Packing' and p.id = POID and p.FabricType !='F' )
) AccessoryReceiving
outer apply(
	select t.KPILETA 
	where exists (select 1 from dbo.PO_Supp_Detail p WITH (NOLOCK) 
	inner join dbo.fabric f WITH (NOLOCK) on f.SCIRefno=p.SCIRefno 
	inner join MtlType m WITH (NOLOCK) on m.id = f.MtlTypeID  
	where m.IssueType='Packing' and p.id = POID )
) PackingMaterialRreceiving
outer apply(
	select min(date) date
	from dbo.WorkHour a WITH (NOLOCK) 
	where FactoryID = FactoryID and a.Hours > 0 and a.date>=dateadd(day,5,MTLETA) and	Holiday=0
) MaterialInspection
outer apply(
	select min(date) date 
	from dbo.WorkHour a WITH (NOLOCK) 
	where FactoryID = FactoryID and a.Hours > 0 and a.date>=dateadd(day,5,MTLETA) and Holiday=0
) CuttingInline 
outer apply(
	select max(date) date 
	from dbo.WorkHour a WITH (NOLOCK) 
	where FactoryID = FactoryID and a.Hours > 0 and a.date<=dateadd(day,-1,Sewinline) 
	and Holiday=0 and NoNeedPPMeeting=1
) FactoryPPMeeting
outer apply(
	select  min(s.OutputDate) min_outputDate
	from dbo.system WITH (NOLOCK) ,dbo.GarmentTest g WITH (NOLOCK) 
	INNER JOIN dbo.GarmentTest_Detail gd WITH (NOLOCK) on gd.ID = g.ID
	left join (dbo.SewingOutput_Detail_Detail sdd WITH (NOLOCK) 
	inner join dbo.SewingOutput s WITH (NOLOCK) on s.id = sdd.ID)  on sdd.OrderId = g.orderid and sdd.Article = g.Article
	where g.OrderID = t.OrderID
)WashTestResultReceiving 
outer apply(
	select min(date) date 
	from dbo.WorkHour a WITH (NOLOCK) 
	where FactoryID = FactoryID and a.Hours > 0 and a.date>=dateadd(day,1,SewOffLine) and Holiday=0
) CartonFinished

drop table #tmp
";
            #endregion
            DualResult result = DBProxy.Current.Select(string.Empty, strCmd, listSQLParameter, out this.QuertData);

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

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtSPNo.Text) &&
                MyUtility.Check.Empty(this.txtbrand.Text) &&
                MyUtility.Check.Empty(this.txtstyle.Text) &&
                (this.dateSCIDlvDate.Value1.Empty() || this.dateSCIDlvDate.Value2.Empty()) &&
                MyUtility.Check.Empty(this.comboColumnType.Text) &&
                !this.checkTargetDate.Checked)
            {
                MyUtility.Msg.WarningBox("query condition can not all is empty, you must enter at least one !!");
                return;
            }

            this.Query();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            this.listControlBindingSource1.EndEdit();
            DualResult result;
            DataTable dtResult;
            DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
            if (dt == null || dt.Rows.Count < 1)
            {
                return;
            }

            string sqlcmd = $@"
Merge CriticalActivity as t
Using(
   select * from (
	    select * 	    
	    from #tmp where ColumnType !=''
    ) t    
) as s
on s.OrderID = t.OrderID and t.DropDownListID = s.ColumnType
when matched then
	update set
	t.TargetDate = s.NewTargetDate,
	t.EditName = '{Env.User.UserID}',
	t.EditDate = GetDate()
when not matched by target then
	insert (OrderID,DropDownListID,TargetDate,EditName,EditDate)
	values(s.OrderID,s.ColumnType,s.NewTargetDate,'{Env.User.UserID}',GetDate());

-- 只要TargetDate是空值,就刪除已存在資料
delete t
from CriticalActivity t
where exists (
	 select * from (
	    select * 
	    ,[row] = ROW_NUMBER() over(partition by orderid,ColumnType order by NewTargetDate desc)
	    from #tmp 
    ) s
    where s.row=1 and (s.NewTargetDate is null or s.NewTargetDate ='')
and s.OrderID=t.OrderID and s.ColumnType=t.DropDownListID 
)

";

            TransactionScope transactionscope = new TransactionScope();
            SqlConnection sqlConn = null;
            DBProxy.Current.OpenConnection(null, out sqlConn);
            using (transactionscope)
            using (sqlConn)
            {
                try
                {
                    if (!(result = MyUtility.Tool.ProcessWithDatatable(dt, string.Empty, sqlcmd, out dtResult, "#tmp", conn: sqlConn)))
                    {
                        throw new Exception(result.Messages.ToString());
                    }

                    transactionscope.Complete();
                    transactionscope.Dispose();
                }
                catch (Exception ex)
                {
                    transactionscope.Dispose();
                    result = new DualResult(false, "Commit transaction error.", ex);
                    return;
                }
                finally
                {
                    transactionscope.Dispose();
                }
            }

            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            MyUtility.Msg.InfoBox("Save successful");
            this.Query();
        }

        private void GridIcon1_AppendClick(object sender, EventArgs e)
        {
            this.grid.ValidateControl();
            this.listControlBindingSource1.EndEdit();
            DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
            if (MyUtility.Check.Empty(dt))
            {
                return;
            }

            int position = this.listControlBindingSource1.Position;
            DataRow drSelect = this.grid.GetDataRow(position);
            if (drSelect != null)
            {
                DataRow dr = dt.NewRow();
                dr["OrderID"] = drSelect["OrderID"];
                dr["BrandID"] = drSelect["BrandID"];
                dr["StyleID"] = drSelect["StyleID"];
                dr["SciDelivery"] = drSelect["SciDelivery"];
                dr["ColumnType"] = string.Empty;
                dr["OriginalDate"] = DBNull.Value;
                dr["NewTargetDate"] = DBNull.Value;
                dt.Rows.InsertAt(dr, position + 1);
                dt.AcceptChanges();
            }
        }

        private void GridIcon1_RemoveClick(object sender, EventArgs e)
        {
            this.grid.ValidateControl();
            this.listControlBindingSource1.EndEdit();
            DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
            if (MyUtility.Check.Empty(dt))
            {
                return;
            }

            DataRow drSelect = this.grid.GetDataRow(this.listControlBindingSource1.Position);
            if (drSelect != null)
            {
                drSelect.Delete();
                dt.AcceptChanges();
            }
        }
    }
}
