using System.Data;
using System.Windows.Forms;
using Sci.Data;
using Ict;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// B04
    /// </summary>
    public partial class B04 : Sci.Win.Tems.Input1
    {
        /// <summary>
        /// B04
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public B04(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            MyUtility.Tool.SetupCombox(this.comboSharebase, 2, 1, "C,CBM,G,G.W.,,Number of Deliver Sheets");
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();

            // 撈Account Name資料
            string selectCommand = string.Format("select Name from AccountNo WITH (NOLOCK) where ID = '{0}'", this.CurrentMaintain["AccountID"].ToString());
            DataTable accountNoTable;
            DualResult selectResult = DBProxy.Current.Select("Finance", selectCommand, out accountNoTable);
            if (accountNoTable != null && accountNoTable.Rows.Count > 0)
            {
                this.displayAccountName.Text = MyUtility.Convert.GetString(accountNoTable.Rows[0]["Name"]);
            }
            else
            {
                this.displayAccountName.Text = string.Empty;
            }
        }
    }
}
