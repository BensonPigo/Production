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

namespace Sci.Production.Warehouse
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
                                    FROM dbo.supp a left join country b on a.countryid = b.id 
                                    WHERE a.ID = '{0}' ", dr["suppid"].ToString());

            selectResult1 = DBProxy.Current.Select(null, selectCommand1, out selectDataTable1);
            if (selectResult1 == false) ShowErr(selectCommand1, selectResult1);

            if (MyUtility.Check.Empty(mtbs.DataSource)) 
            { 
                mtbs.DataSource = selectDataTable1;
                displayBox1.DataBindings.Add(new Binding("Text", mtbs, "id", true));
                textBox1.DataBindings.Add(new Binding("Text", mtbs, "zipcode", true));
                textBox2.DataBindings.Add(new Binding("Text", mtbs, "abbCH", true));
                textBox3.DataBindings.Add(new Binding("Text", mtbs, "abbEN", true));
                textBox4.DataBindings.Add(new Binding("Text", mtbs, "NameCH", true));
                textBox5.DataBindings.Add(new Binding("Text", mtbs, "NameEN", true));
                textBox6.DataBindings.Add(new Binding("Text", mtbs, "addressCH", true));
                textBox7.DataBindings.Add(new Binding("Text", mtbs, "tel", true));
                textBox8.DataBindings.Add(new Binding("Text", mtbs, "fax", true));
                textBox9.DataBindings.Add(new Binding("Text", mtbs, "currencyid", true));
                textBox10.DataBindings.Add(new Binding("Text", mtbs, "countryid", true));
                textBox11.DataBindings.Add(new Binding("Text", mtbs, "countryAlias", true));
                editBox1.DataBindings.Add(new Binding("Text", mtbs, "lockmemo", true));
                editBox2.DataBindings.Add(new Binding("Text", mtbs, "delaymemo", true));
                editBox3.DataBindings.Add(new Binding("Text", mtbs, "addressEN", true));
                dateBox1.DataBindings.Add(new Binding("Text", mtbs, "lockdate", true));
                dateBox2.DataBindings.Add(new Binding("Text", mtbs, "delay", true));
                checkBox1.DataBindings.Add(new Binding("Value", mtbs, "junk", true));
                checkBox2.DataBindings.Add(new Binding("Value", mtbs, "ThirdCountry", true));

            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
