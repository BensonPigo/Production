using Ict.Win;
using System.Data;
using System.Windows.Forms;
using Ict;

namespace Sci.Production.IE
{
    /// <summary>
    /// B12
    /// </summary>
    public partial class B12 : Sci.Win.Tems.Input2
    {
        /// <summary>
        /// B12
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public B12(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string id = e.Master.Field<string>("ID");
            string sqlCmd = string.Empty;
            sqlCmd = $@"
Select ID, Description,junk
From MachineGroup
Where MasterGroupID = '{id}' 
 order by junk,ID";

            this.DetailSelectCommand = sqlCmd;
            return base.OnDetailSelectCommandPrepare(e);
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("ID", header: "Machine Group ID", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Description", header: "Machine Desc.", width: Widths.AnsiChars(30), iseditingreadonly: true)
                .CheckBox("junk", "Junk", iseditable: false)
                ;
        }
    }
}
