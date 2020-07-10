using System.Data;
using Ict;

namespace Sci.Win
{
    /// <summary>
    /// SUBBASE
    /// </summary>
    public partial class SUBBASE : Subs.Base
    {
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;

        /// <summary>
        /// Initializes a new instance of the <see cref="SUBBASE"/> class.
        /// </summary>
        public SUBBASE()
        {
            this.InitializeComponent();

            this.Helper.Controls.Grid.Generator(this.grid)
                .CheckBox().Get(out this.col_chk)
                .Text("id", header: "id")
                .Text("name", header: "name")
                ;

            this.chk_yes.Click += (s, e) =>
            {
                this.grid.SetCheckeds(this.col_chk);
            };
            this.chk_no.Click += (s, e) =>
            {
                this.grid.SetUncheckeds(this.col_chk);
            };
            this.get.Click += (s, e) =>
            {
                var datas = this.grid.GetCheckeds(this.col_chk);
                this.ShowInfo(datas.Count.ToString());
            };
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            DualResult result;

            DataTable datas;
            if (!(result = Data.DBProxy.Current.Select(null, "SELECT * FROM accno", out datas)))
            {
                this.ShowErr(result);
            }
            else
            {
                this.gridbs.DataSource = datas;
            }
        }
    }
}
