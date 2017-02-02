using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.IE
{
    public partial class P01_SelectOperationCode : Sci.Win.Subs.Base
    {
    
        private DataTable gridData;
        public DataRow p01SelectOperationCode;
        public P01_SelectOperationCode()
        {
            InitializeComponent();
        }
        
        protected override void OnFormLoaded()
        {
            Ict.Win.DataGridViewGeneratorTextColumnSettings s1 = new DataGridViewGeneratorTextColumnSettings();
            base.OnFormLoaded();
            this.grid1.IsEditingReadOnly = true;
            this.grid1.DataSource = listControlBindingSource1;

            s1.CellMouseDoubleClick += (s, e) => 
            {
                if ( e.Button == MouseButtons.Left)
                {
                    DataGridViewSelectedRowCollection selectRows = grid1.SelectedRows;
                    foreach (DataGridViewRow datarow in selectRows)
                    {
                        p01SelectOperationCode = ((DataRowView)datarow.DataBoundItem).Row;
                        DialogResult = System.Windows.Forms.DialogResult.Cancel;
                    }
                }
            };

            Helper.Controls.Grid.Generator(this.grid1)
                .Text("ID", header: "ID", width: Widths.AnsiChars(20), iseditingreadonly: true,settings: s1)
                 .Text("DescEN", header: "Description", width: Widths.AnsiChars(30), iseditingreadonly: true)
                 .Numeric("SMV", header: "S.M.V",decimal_places:4, iseditingreadonly: true)
                 .Text("MachineTypeID", header: "Machine Code", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Numeric("SeamLength", header: "Seam Length",decimal_places:2, iseditingreadonly: true);

            string sqlCmd = "select ID,DescEN,SMV,MachineTypeID,SeamLength,MoldID,MtlFactorID from Operation where CalibratedCode = 1";
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out gridData);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query Operation fail\r\n"+result.ToString());
            }
            listControlBindingSource1.DataSource = gridData;
            numericBox3.Value = gridData.Rows.Count;
        }

        //Find
        private void button1_Click(object sender, EventArgs e)
        {
            StringBuilder filterCondition = new StringBuilder();
            if (!MyUtility.Check.Empty(textBox1.Text))
            {
                filterCondition.Append(string.Format(" ID like '%{0}%' and", textBox1.Text.Trim()));
            }
            if (textBox1.Text == "")
            {
                filterCondition.Append(string.Format("   "));
            }
            if (!MyUtility.Check.Empty(numericBox1.Value))
            {
                filterCondition.Append(string.Format(" SMV >= {0} and", numericBox1.Value.ToString().Trim()));
            }
            if (!MyUtility.Check.Empty(textBox2.Text))
            {
                filterCondition.Append(string.Format(" MachineTypeID like '%{0}%' and", textBox2.Text.Trim()));
            }
            if (!MyUtility.Check.Empty(numericBox2.Value))
            {
                filterCondition.Append(string.Format(" SeamLength >= {0} and", numericBox2.Value.ToString().Trim()));
            }
            if (!MyUtility.Check.Empty(textBox3.Text))
            {
                filterCondition.Append(string.Format(" DescEN like'%{0}%' and", textBox3.Text.Trim()));
            }
            
            if (filterCondition.Length > 0)
            {
                string filter = filterCondition.ToString().Substring(0, filterCondition.Length - 3);
                gridData.DefaultView.RowFilter = filter;
                numericBox3.Value = gridData.DefaultView.Count;
            }
        }

        //Select
        private void button2_Click(object sender, EventArgs e)
        {
            if (grid1.SelectedRows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Must Select one!!!");
                return;
            }

            DataGridViewSelectedRowCollection selectRows = grid1.SelectedRows;
            foreach (DataGridViewRow datarow in selectRows)
            {
                p01SelectOperationCode = ((DataRowView)datarow.DataBoundItem).Row;
            }
            
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }


    }
}
