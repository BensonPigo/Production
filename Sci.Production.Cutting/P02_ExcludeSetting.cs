using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Data;

namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public partial class P02_ExcludeSetting : Sci.Win.Tems.QueryForm
    {
        private readonly string id;

        /// <inheritdoc/>
        public P02_ExcludeSetting(string id, bool isSupportEdit)
        {
            this.InitializeComponent();
            this.id = id;
            this.btnSave.Enabled = isSupportEdit;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.GridSetup();
            this.Query();
        }

        private void GridSetup()
        {
            DataGridViewGeneratorCheckBoxColumnSettings exWip = new DataGridViewGeneratorCheckBoxColumnSettings();
            exWip.CellValidating += (s, e) =>
            {
                DataRow dr = this.grid1.GetDataRow(e.RowIndex);
                DataRow[] drs = ((DataTable)this.listControlBindingSource1.DataSource).Select($"FabricCombo = '{dr["FabricCombo"]}'");
                foreach (DataRow item in drs)
                {
                    item["ExWip"] = e.FormattedValue;
                    item.EndEdit();
                }
            };

            this.grid1.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.grid1)
                .Text("FabricCombo", header: "Fabric Combo", width: Widths.AnsiChars(4), iseditingreadonly: true)
                .CheckBox("ExWip", header: "Exclude in WIP", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0, settings: exWip)
                .CheckBox("NoNotch", header: "No Notch", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
                .Text("FabricCode", header: "Fabric#", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("Refno", header: "Refno", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Description", header: "Description", width: Widths.AnsiChars(55), iseditingreadonly: true)
                .Text("AddName", header: "Add Name", width: Widths.AnsiChars(25), iseditingreadonly: true)
                .DateTime("AddDate", header: "Add Date", width: Widths.AnsiChars(20), iseditingreadonly: true)
                ;
        }

        private void Query()
        {
            string sqlcmd = $@"
select distinct
	oe.FabricCombo,
	ExWip=CAST(isnull((select 1 from Cutting_WIPExcludePatternPanel cw with(nolock) where cw.ID = oe.Id and cw.PatternPanel = oe.FabricCombo), 0) as bit),
	oe.FabricCode,f.Refno,f.Description,
    [AddName] = concat(cw.AddName, '-'+ (select name from pass1 where id=cw.AddName)),
	cw.AddDate,
    oe.NoNotch
from Order_EachCons oe with(nolock)
inner join Order_BOF bof with(nolock) on bof.Id = oe.Id and bof.FabricCode = oe.FabricCode
LEFT JOIN Fabric f with(nolock) ON bof.SCIRefno=f.SCIRefno
left join Cutting_WIPExcludePatternPanel cw with(nolock) on cw.ID = oe.Id and cw.PatternPanel = oe.FabricCombo
where oe.Id = '{this.id}'
and oe.CuttingPiece <> 1
";
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out DataTable gridtb);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.listControlBindingSource1.DataSource = gridtb;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            string sqlcmd = $@"
Delete Cutting_WIPExcludePatternPanel where id = '{this.id}'
alter table #tmp alter column FabricCode varchar(3)

insert into Cutting_WIPExcludePatternPanel(ID,PatternPanel,AddName,AddDate)
select distinct '{this.id}',FabricCombo,'{Sci.Env.User.UserID}',getdate() from #tmp where ExWip = 1

update j set j.Success = 1 from JobCrashLog j where j.JobName = 'SetQtyBySubprocess' and j.Success = 0 and j.OrderID = '{this.id}'

update  oe set oe.NoNotch = t.NoNotch
from #tmp t
inner join Order_EachCons oe on oe.ID = '{this.id}' and t.FabricCode = oe.FabricCode

insert into JobCrashLog(JobName, OrderID)
values('SetQtyBySubprocess' , '{this.id}')

drop table #tmp
";
            DataTable sdt = (DataTable)this.listControlBindingSource1.DataSource;
            DualResult result1 = MyUtility.Tool.ProcessWithDatatable(sdt, string.Empty, sqlcmd, out DataTable odt);
            if (!result1)
            {
                this.ShowErr(result1);
                return;
            }

            this.Close();
        }
    }
}
