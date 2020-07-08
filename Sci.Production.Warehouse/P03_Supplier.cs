using System;
using System.Data;
using System.Windows.Forms;
using Sci.Data;
using Ict;

namespace Sci.Production.Warehouse
{
    public partial class P03_Supplier : Sci.Win.Subs.Base
    {
        DataRow dr;
        DataTable selectDataTable1;
        DualResult selectResult1;

        public P03_Supplier(DataRow data)
        {
            this.dr = data;

            this.InitializeComponent();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            string selectCommand1 = string.Format(
                @"SELECT a.*,b.alias as countryalias 
                                    FROM dbo.supp a WITH (NOLOCK) left join country b WITH (NOLOCK) on a.countryid = b.id 
                                    WHERE a.ID = '{0}' ", this.dr["suppid"].ToString());

            this.selectResult1 = DBProxy.Current.Select(null, selectCommand1, out this.selectDataTable1);
            if (this.selectResult1 == false)
            {
                this.ShowErr(selectCommand1, this.selectResult1);
            }

            if (MyUtility.Check.Empty(this.mtbs.DataSource))
            {
                this.mtbs.DataSource = this.selectDataTable1;
                this.displaySupplierID.DataBindings.Add(new Binding("Text", this.mtbs, "id", true));
                this.txtZipCode.DataBindings.Add(new Binding("Text", this.mtbs, "zipcode", true));
                this.txtAbbrChinese.DataBindings.Add(new Binding("Text", this.mtbs, "abbCH", true));
                this.txtAbbrEnglish.DataBindings.Add(new Binding("Text", this.mtbs, "abbEN", true));
                this.txtCompanyCH.DataBindings.Add(new Binding("Text", this.mtbs, "NameCH", true));
                this.txtCompanyEN.DataBindings.Add(new Binding("Text", this.mtbs, "NameEN", true));
                this.txtAddressCH.DataBindings.Add(new Binding("Text", this.mtbs, "addressCH", true));
                this.txtTEL.DataBindings.Add(new Binding("Text", this.mtbs, "tel", true));
                this.txtFax.DataBindings.Add(new Binding("Text", this.mtbs, "fax", true));
                this.txtCurrency.DataBindings.Add(new Binding("Text", this.mtbs, "currencyid", true));
                this.txtNationality.DataBindings.Add(new Binding("Text", this.mtbs, "countryid", true));
                this.txtNationality2.DataBindings.Add(new Binding("Text", this.mtbs, "countryAlias", true));
                this.editLockDate.DataBindings.Add(new Binding("Text", this.mtbs, "lockmemo", true));
                this.editDelay.DataBindings.Add(new Binding("Text", this.mtbs, "delaymemo", true));
                this.editAddressEN.DataBindings.Add(new Binding("Text", this.mtbs, "addressEN", true));
                this.dateLockDate.DataBindings.Add(new Binding("Text", this.mtbs, "lockdate", true));
                this.dateDelay.DataBindings.Add(new Binding("Text", this.mtbs, "delay", true));
                this.checkJunk.DataBindings.Add(new Binding("Value", this.mtbs, "junk", true));
                this.checkThirdCountry.DataBindings.Add(new Binding("Value", this.mtbs, "ThirdCountry", true));
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
