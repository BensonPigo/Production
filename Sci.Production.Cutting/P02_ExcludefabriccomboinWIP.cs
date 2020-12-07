using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Prg;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public partial class P02_ExcludefabriccomboinWIP : Sci.Win.Tems.QueryForm
    {
        private readonly string id;

        /// <inheritdoc/>
        public P02_ExcludefabriccomboinWIP(string id)
        {
            this.InitializeComponent();
            this.id = id;
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
            this.grid1.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.grid1)
                .Text("FabricCombo", header: "Fabric Combo", width: Widths.AnsiChars(4), iseditingreadonly: true)
                .CheckBox("ExWip", header: "Exclude in WIP", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
                .Text("FabricCode", header: "Fabric#", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("Refno", header: "Refno", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Description", header: "Description", width: Widths.AnsiChars(55), iseditingreadonly: true)
                ;
        }

        private void Query()
        {
            string sqlcmd = $@"
select distinct
	oe.FabricCombo,
	ExWip=CAST(isnull((select 1 from Cutting_WIPExcludePatternPanel cw with(nolock) where cw.ID = oe.Id and cw.PatternPanel = oe.FabricCombo), 0) as bit),
	oe.FabricCode,f.Refno,f.Description
from Order_EachCons oe with(nolock)
inner join Order_BOF bof with(nolock) on bof.Id = oe.Id and bof.FabricCode = oe.FabricCode
LEFT JOIN Fabric f with(nolock) ON bof.SCIRefno=f.SCIRefno
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
insert into Cutting_WIPExcludePatternPanel(ID,PatternPanel,AddName,AddDate)
select '{this.id}',FabricCombo,'{Sci.Env.User.UserID}',getdate() from #tmp where ExWip = 1
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
