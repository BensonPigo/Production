using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Sci;
using Sci.Data;
using Ict;

namespace Sci.Production.PublicForm.Material
{
    public partial class P03_Supplier : Sci.Win.Subs.Base
    {
        DataRow dr;
        DataTable selectDataTable1;
        DualResult selectResult1;

        public P03_Supplier(DataRow data)
        {
            dr = data;
            

            InitializeComponent();
            
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            string selectCommand1 = string.Format(@"SELECT a.*,b.alias as countryalias 
                                    FROM dbo.supp a WITH (NOLOCK) left join country b WITH (NOLOCK) on a.countryid = b.id 
                                    WHERE a.ID = '{0}' ", dr["suppid"].ToString());

            selectResult1 = DBProxy.Current.Select(null, selectCommand1, out selectDataTable1);
            if (selectResult1 == false) ShowErr(selectCommand1, selectResult1);

            if (MyUtility.Check.Empty(mtbs.DataSource)) 
            { 
                mtbs.DataSource = selectDataTable1;
                displaySupplierID.DataBindings.Add(new Binding("Text", mtbs, "id", true));
                txtZipCode.DataBindings.Add(new Binding("Text", mtbs, "zipcode", true));
                txtAbbrChinese.DataBindings.Add(new Binding("Text", mtbs, "abbCH", true));
                txtAbbrEnglish.DataBindings.Add(new Binding("Text", mtbs, "abbEN", true));
                txtCompanyCH.DataBindings.Add(new Binding("Text", mtbs, "NameCH", true));
                txtCompanyEN.DataBindings.Add(new Binding("Text", mtbs, "NameEN", true));
                txtAddressCH.DataBindings.Add(new Binding("Text", mtbs, "addressCH", true));
                txtTEL.DataBindings.Add(new Binding("Text", mtbs, "tel", true));
                txtFax.DataBindings.Add(new Binding("Text", mtbs, "fax", true));
                txtCurrency.DataBindings.Add(new Binding("Text", mtbs, "currencyid", true));
                txtNationality.DataBindings.Add(new Binding("Text", mtbs, "countryid", true));
                txtNationality2.DataBindings.Add(new Binding("Text", mtbs, "countryAlias", true));
                editLockDate.DataBindings.Add(new Binding("Text", mtbs, "lockmemo", true));
                editDelay.DataBindings.Add(new Binding("Text", mtbs, "delaymemo", true));
                editAddressEN.DataBindings.Add(new Binding("Text", mtbs, "addressEN", true));
                dateLockDate.DataBindings.Add(new Binding("Text", mtbs, "lockdate", true));
                dateDelay.DataBindings.Add(new Binding("Text", mtbs, "delay", true));
                checkJunk.DataBindings.Add(new Binding("Value", mtbs, "junk", true));
                checkThirdCountry.DataBindings.Add(new Binding("Value", mtbs, "ThirdCountry", true));

            }
            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
