using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Sewing
{
    /// <summary>
    /// B05_BatchImport
    /// </summary>
    public partial class B05_BatchImport : Sci.Win.Tems.QueryForm
    {

        /// <summary>
        /// B05_BatchImport
        /// </summary>
        public B05_BatchImport()
        {
            this.InitializeComponent();
            this.EditMode = true;
            this.displayFactory.Text = Env.User.Factory;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            DataGridViewGeneratorNumericColumnSettings setEfficiency = new DataGridViewGeneratorNumericColumnSettings();

            setEfficiency.CellValidating += (s, e) =>
            {
                decimal efficiency = MyUtility.Convert.GetDecimal(e.FormattedValue);

                if (efficiency > 0)
                {
                    DataRow curRow = this.gridSewingOutputEfficiency.GetDataRow(e.RowIndex);
                    curRow["select"] = true;
                    curRow["Efficiency"] = efficiency;
                }
            };

            base.OnFormLoaded();
            this.Helper.Controls.Grid.Generator(this.gridSewingOutputEfficiency)
              .CheckBox("select", trueValue: true, falseValue: false)
              .Text("FactoryID", header: "Factory", iseditingreadonly: true)
              .Text("BrandID", header: "Brand", iseditingreadonly: true)
              .Text("StyleID", header: "Style", width: Widths.AnsiChars(20), iseditingreadonly: true)
              .Text("SeasonID", header: "Season", iseditingreadonly: true)
              .Numeric("Efficiency", header: "Set Eff(%)", decimal_places: 2, iseditingreadonly: false, settings: setEfficiency);

            this.gridSewingOutputEfficiency.Columns["Efficiency"].DefaultCellStyle.BackColor = Color.Pink;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtBrand.Text) &&
                MyUtility.Check.Empty(this.txtStyle.Text) &&
                MyUtility.Check.Empty(this.txtSeason.Text))
            {
                MyUtility.Msg.WarningBox("<Brand>,<Style>,<Season> Enter at least one");
                return;
            }

            string sqlGetData = string.Empty;
            string sqlWhere = string.Empty;

            if (!MyUtility.Check.Empty(this.txtBrand.Text))
            {
                sqlWhere += $" and Style.BrandID = '{this.txtBrand.Text}' ";
            }

            if (!MyUtility.Check.Empty(this.txtStyle.Text))
            {
                sqlWhere += $" and Style.ID = '{this.txtStyle.Text}' ";
            }

            if (!MyUtility.Check.Empty(this.txtSeason.Text))
            {
                sqlWhere += $" and Style.SeasonID = '{this.txtSeason.Text}' ";
            }

            sqlGetData = $@"
select 
[select] = cast(0 as bit),
FactoryID = '{this.displayFactory.Text}',
Style.BrandID,
StyleID = Style.ID,
Style.SeasonID,
[Efficiency] = 0,
StyleUkey = Style.Ukey
from Style with (nolock)
where Style.Junk=0
and not exists(select 1 from SewingOutputEfficiency with (nolock) 
                where SewingOutputEfficiency.StyleUkey = Style.Ukey and SewingOutputEfficiency.FactoryID = '{this.displayFactory.Text}')
{sqlWhere}
";

            DataTable dtSewingOutputEfficiency;
            DualResult result = DBProxy.Current.Select(null, sqlGetData, out dtSewingOutputEfficiency);

            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.gridSewingOutputEfficiency.DataSource = dtSewingOutputEfficiency;
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            if (this.gridSewingOutputEfficiency.DataSource == null)
            {
                return;
            }

            var listChecked = ((DataTable)this.gridSewingOutputEfficiency.DataSource).AsEnumerable().Where(s => (bool)s["select"]);

            if (!listChecked.Any())
            {
                MyUtility.Msg.WarningBox("Please select data first");
                return;
            }

            foreach (DataRow drImportItem in listChecked)
            {
                string sqlInsertSewingOutputEfficiency = $@"
if exists(select 1 from SewingOutputEfficiency where StyleUkey = '{drImportItem["StyleUkey"]}' and FactoryID = '{drImportItem["FactoryID"]}')
    return

insert into SewingOutputEfficiency(FactoryID, StyleUkey, StyleID, BrandID, SeasonID, Efficiency, Junk, AddName, AddDate)
    values('{drImportItem["FactoryID"]}', '{drImportItem["StyleUkey"]}', '{drImportItem["StyleID"]}', '{drImportItem["BrandID"]}', '{drImportItem["SeasonID"]}', @Efficiency, 0, '{Env.User.UserID}', getdate())
";
                List<SqlParameter> listPar = new List<SqlParameter> { new SqlParameter("@Efficiency", drImportItem["Efficiency"]) };

                DualResult result = DBProxy.Current.Execute(null, sqlInsertSewingOutputEfficiency, listPar);

                if (!result)
                {
                    this.ShowErr(result);
                    return;
                }
            }

            MyUtility.Msg.InfoBox("Import Success");
            this.btnQuery.PerformClick();
        }
    }
}
