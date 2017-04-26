﻿using System;
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
            this.gridDetail.IsEditingReadOnly = true;
            this.gridDetail.DataSource = listControlBindingSource1;

            s1.CellMouseDoubleClick += (s, e) => 
            {
                if ( e.Button == MouseButtons.Left)
                {
                    DataGridViewSelectedRowCollection selectRows = gridDetail.SelectedRows;
                    foreach (DataGridViewRow datarow in selectRows)
                    {
                        p01SelectOperationCode = ((DataRowView)datarow.DataBoundItem).Row;
                        DialogResult = System.Windows.Forms.DialogResult.Cancel;
                    }
                }
            };

            Helper.Controls.Grid.Generator(this.gridDetail)
                .Text("ID", header: "ID", width: Widths.AnsiChars(20), iseditingreadonly: true,settings: s1)
                 .Text("DescEN", header: "Description", width: Widths.AnsiChars(30), iseditingreadonly: true)
                 .Numeric("SMV", header: "S.M.V",decimal_places:4, iseditingreadonly: true)
                 .Text("MachineTypeID", header: "Machine Code", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Numeric("SeamLength", header: "Seam Length",decimal_places:2, iseditingreadonly: true);

            string sqlCmd = "select ID,DescEN,SMV,MachineTypeID,SeamLength,MoldID,MtlFactorID from Operation WITH (NOLOCK) where CalibratedCode = 1";
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out gridData);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query Operation fail\r\n"+result.ToString());
            }
            listControlBindingSource1.DataSource = gridData;
            numCount.Value = gridData.Rows.Count;
        }

        //Find
        private void button1_Click(object sender, EventArgs e)
        {
            StringBuilder filterCondition = new StringBuilder();
            if (!MyUtility.Check.Empty(txtID.Text))
            {
                filterCondition.Append(string.Format(" ID like '%{0}%' and", txtID.Text.Trim()));
            }
            if (txtID.Text == "")
            {
                filterCondition.Append(string.Format("   "));
            }
            if (!MyUtility.Check.Empty(numSMV.Value))
            {
                filterCondition.Append(string.Format(" SMV >= {0} and", numSMV.Value.ToString().Trim()));
            }
            if (!MyUtility.Check.Empty(txtMachineCode.Text))
            {
                filterCondition.Append(string.Format(" MachineTypeID like '%{0}%' and", txtMachineCode.Text.Trim()));
            }
            if (!MyUtility.Check.Empty(numSeamLength.Value))
            {
                filterCondition.Append(string.Format(" SeamLength >= {0} and", numSeamLength.Value.ToString().Trim()));
            }
            if (!MyUtility.Check.Empty(txtDescription.Text))
            {
                filterCondition.Append(string.Format(" DescEN like'%{0}%' and", txtDescription.Text.Trim()));
            }
            
            if (filterCondition.Length > 0)
            {
                string filter = filterCondition.ToString().Substring(0, filterCondition.Length - 3);
                gridData.DefaultView.RowFilter = filter;
                numCount.Value = gridData.DefaultView.Count;
            }
            this.gridDetail.AutoResizeColumns();
        }

        //Select
        private void button2_Click(object sender, EventArgs e)
        {
            if (gridDetail.SelectedRows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Must Select one!!!");
                return;
            }

            DataGridViewSelectedRowCollection selectRows = gridDetail.SelectedRows;
            foreach (DataGridViewRow datarow in selectRows)
            {
                p01SelectOperationCode = ((DataRowView)datarow.DataBoundItem).Row;
            }
            
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }


    }
}
