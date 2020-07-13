using System.Data;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.IE
{
    /// <summary>
    /// IE_B05_ThreadRatio
    /// </summary>
    public partial class B05_ThreadRatio : Win.Subs.Base
    {
        private string id;

        /// <summary>
        /// B05_ThreadRatio
        /// </summary>
        /// <param name="id">id</param>
        public B05_ThreadRatio(string id)
        {
            this.InitializeComponent();
            this.Id = id;
        }

        /// <summary>
        /// get,set Id
        /// </summary>
        public string Id
        {
            get
            {
                return this.id;
            }

            set
            {
                this.id = value;
            }
        }

        /// <summary>
        /// OnFormLoaded()
        /// </summary>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.gridArtworkSummary.IsEditingReadOnly = true;
            this.gridArtworkSummary.DataSource = this.listControlBindingSource1;

            this.Helper.Controls.Grid.Generator(this.gridArtworkSummary)
                .Text("SEQ", header: "SEQ", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Text("ThreadLocation", header: "Thread Location", width: Widths.AnsiChars(12), iseditingreadonly: true)
                .Numeric("UseRatio", header: "UseRatio", decimal_places: 2, iseditingreadonly: true)
                .Numeric("Allowance", header: "Start End Loss", decimal_places: 2, iseditingreadonly: true);

            string sqlCmd = string.Format("select * from MachineType_ThreadRatio where ID='{0}'", this.Id);
            DataTable gridData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out gridData);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query ThreadRatio fail\r\n" + result.ToString());
            }

            this.listControlBindingSource1.DataSource = gridData;
        }
    }
}
