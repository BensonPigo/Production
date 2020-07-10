using System.Data;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// P01_CuttingCombo
    /// </summary>
    public partial class P01_CuttingCombo : Win.Subs.Base
    {
        private string poID;

        /// <summary>
        /// P01_CuttingCombo
        /// </summary>
        /// <param name="pOID">string pOID</param>
        public P01_CuttingCombo(string pOID)
        {
            this.InitializeComponent();
            this.poID = pOID;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            DataTable gridData;
            string sqlCmd = string.Format("select CuttingSP,ID from Orders WITH (NOLOCK) where POID = '{0}'", this.poID);
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out gridData);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query data fail!!" + result.ToString());
            }

            this.listControlBindingSource1.DataSource = gridData;

            // 設定Grid1的顯示欄位
            this.gridCuttingCombo.IsEditingReadOnly = true;
            this.gridCuttingCombo.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridCuttingCombo)
                .Text("CuttingSP", header: "Cutting SP#", width: Widths.AnsiChars(13))
                .Text("ID", header: "SP#", width: Widths.AnsiChars(15));
        }
    }
}
