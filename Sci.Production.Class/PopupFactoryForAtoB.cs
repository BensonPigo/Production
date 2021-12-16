using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.CallPmsAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Class
{
    /// <summary>
    /// PopupFactoryForAtoB
    /// </summary>
    public partial class PopupFactoryForAtoB : Sci.Win.Tems.QueryForm
    {
        string defaultCheckedItem = string.Empty;
        string localRgcode = string.Empty;
        Dictionary<string, DataTable> dicGridSource = new Dictionary<string, DataTable>();

        /// <summary>
        /// PopupFactoryForAtoB
        /// </summary>
        /// <param name="defaultCheckedItem">defaultCheckedItem</param>
        public PopupFactoryForAtoB(string defaultCheckedItem = "")
        {
            this.InitializeComponent();
            this.EditMode = true;
            this.DialogResult = DialogResult.Cancel;
            string sqlGetRegion = @"
select distinct SystemName from SystemWebAPIURL where countryID = (select Region from system )
union
select [SystemName] = RgCode from System
";
            this.defaultCheckedItem = defaultCheckedItem;
            DataTable dtFactory;
            DualResult result = DBProxy.Current.Select(null, sqlGetRegion, out dtFactory);
            if (!result)
            {
                this.ShowErr(result);
                this.Close();
                return;
            }

            this.comboFactory.DisplayMember = "SystemName";
            this.comboFactory.ValueMember = "SystemName";
            this.comboFactory.DataSource = dtFactory;
            this.localRgcode = MyUtility.GetValue.Lookup("select Rgcode from system");
            this.comboFactory.Text = this.localRgcode;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.Helper.Controls.Grid.Generator(this.gridFactory)
               .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
               .Text("Factory", header: "Factory", width: Widths.AnsiChars(10), iseditingreadonly: true);

            this.Query();
        }

        private void Query()
        {
            DataTable dtFactory;
            DualResult result;

            if (this.dicGridSource.ContainsKey(this.comboFactory.Text))
            {
                dtFactory = this.dicGridSource[this.comboFactory.Text];
            }
            else
            {
                if (this.localRgcode == this.comboFactory.Text)
                {
                    string sqlGetFactoryData = $@"
select [Selected] = 1, [Factory] = F.ID 
from Factory F WITH (NOLOCK) 
INNER JOIN System S ON S.RgCode=F.NegoRegion
where F.Junk = 0 and F.IsProduceFty = 1 order by F.ID";

                    result = DBProxy.Current.Select(null, sqlGetFactoryData, out dtFactory);
                }
                else
                {
                    string rgCode = MyUtility.GetValue.Lookup("select RgCode from system");
                    string sqlGetFactoryData = $@"
select [Selected] = 1, ID AS [Factory] from Factory
INNER JOIN [System] ON System.RgCode!= Factory.NegoRegion
where Factory.Junk = 0 and Factory.IsProduceFty = 1 AND NegoRegion='{rgCode}'
order by ID";

                    result = PackingA2BWebAPI.GetDataBySql(this.comboFactory.Text, sqlGetFactoryData, out dtFactory);
                }

                if (!result)
                {
                    this.ShowErr(result);
                    return;
                }

                this.dicGridSource.Add(this.comboFactory.Text, dtFactory);
            }

            this.gridFactory.DataSource = dtFactory;

            foreach (DataGridViewRow gridRow in this.gridFactory.Rows)
            {
                gridRow.Cells["Selected"].Value = 1;
            }
        }

        /// <summary>
        /// ResultFactory
        /// </summary>
        public string ResultFactory
        {
            get
            {
                var resultFactory = ((DataTable)this.gridFactory.DataSource).AsEnumerable().Where(s => MyUtility.Convert.GetInt(s["Selected"]) == 1);

                if (resultFactory.Any())
                {
                    return resultFactory.Select(s => s["Factory"].ToString()).JoinToString(",");
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// IsDataFromA2B
        /// </summary>
        public bool IsDataFromA2B
        {
            get
            {
                return this.localRgcode != this.comboFactory.Text;
            }
        }

        /// <summary>
        /// SystemName
        /// </summary>
        public string SystemName
        {
            get
            {
                return this.comboFactory.Text;
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void BtnSelect_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void ComboFactory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!MyUtility.Check.Empty(this.comboFactory.Text) && !MyUtility.Check.Empty(this.localRgcode))
            {
                this.Query();
            }
        }
    }
}
