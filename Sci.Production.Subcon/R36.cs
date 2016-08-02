using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Subcon
{
    public partial class R36 : Sci.Win.Tems.PrintForm
    {
        public R36(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.comboBox1.SelectedIndex = 0;
            this.comboBox2.SelectedIndex = 0;
            this.comboBox3.SelectedIndex = 0;
            this.comboBox4.SelectedIndex = 0;
        }
        protected override bool ValidateInput()
        {
            bool dateRange1 = !this.dateRange1.HasValue, dateRange2 = !this.dateRange2.HasValue, dateRange3 = !this.dateRange3.HasValue, textbox1 = !this.textBox1.Text.Empty(), textbox2 = !this.textBox2.Text.Empty(), txtLocalSupp1 = !this.txtLocalSupp1.TextBox1.Text.Empty()
                , txtuser1 = !this.txtuser1.TextBox1.Text.Empty(), txtuser2 = !this.txtuser1.TextBox1.Text.Empty(), txtfactory1 = !this.txtfactory1.Text.Empty(), comboBox2 = !this.comboBox2.Text.Empty(), comboBox3 = !this.comboBox3.Text.Empty(), dateRange4 = !this.dateRange4.HasValue, dateRange5 = !this.dateRange5.HasValue;
           
            if (!dateRange1 || !dateRange2 || !dateRange3 || !textbox1 || !textbox2 || !txtLocalSupp1 || !txtuser1 || !txtuser2 || !txtfactory1 || !comboBox2 || !comboBox3
               || !dateRange4 || !dateRange5)
            {
                MyUtility.Msg.ErrorBox("Please select at least one field entry");
      
                textBox1.Focus();
                              
                return false;
            }
   
            
            return base.ValidateInput();
        }
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            
            
            
            
            
            return Result.True;
        }
       
    }
}
