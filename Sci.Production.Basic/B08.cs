using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci;
using Sci.Data;

namespace Sci.Production.Basic
{
    public partial class B08 : Sci.Win.Tems.Input1
    {
        public B08(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string selectCommand = string.Format("select * from CDCode_Content where ID = '{0}'", this.CurrentMaintain["ID"].ToString());
            DataTable selectDataTable;
            DualResult selectResult = DBProxy.Current.Select(null, selectCommand, out selectDataTable);
            if (selectDataTable.Rows.Count == 0)
            {
                string insertCommand = string.Format("insert into CDCode_Content(ID) values ('{0}'); ", this.CurrentMaintain["ID"].ToString());
                DualResult result;
                if (result = DBProxy.Current.Execute(null, insertCommand))
                {
                    selectResult = DBProxy.Current.Select(null, selectCommand, out selectDataTable);
                }
                else
                {
                    MessageBox.Show("Insert data fail!! Please click button again. \n" + result.ToString());
                    return;
                }
            }
            bool hasEditauthority = true; //等Pass2結構確定後再來撈此資料
            Sci.Production.Basic.B08_ProductionFabricType rm = new Sci.Production.Basic.B08_ProductionFabricType(hasEditauthority, selectDataTable.Rows[0]);
            rm.ShowDialog(this);

        }
    }
}
