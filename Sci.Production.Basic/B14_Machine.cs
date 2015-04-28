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

namespace Sci.Production.Basic
{
    public partial class B14_Machine : Sci.Win.Subs.Base
    {
        protected DataRow motherData;
        public B14_Machine(DataRow data)
        {
            InitializeComponent();
            this.motherData = data;
        }

        protected override void OnFormLoaded()
        {
            //撈Grid資料
            string selectCommand = string.Format("select ID,Description from MachineType where ArtworkTypeID = '{0}' or ArtworkTypeDetail = '{0}'", this.motherData["ID"].ToString());
            
            DataTable selectDataTable;
            DualResult selectResult1 = DBProxy.Current.Select(null, selectCommand, out selectDataTable);
            bindingSource1.DataSource = selectDataTable;

            //設定Grid1的顯示欄位
            this.grid1.IsEditingReadOnly = false;
            this.grid1.DataSource = bindingSource1;
            Helper.Controls.Grid.Generator(this.grid1)
                 .Text("ID", header: "ID", width: Widths.AnsiChars(10))
                 .Text("Description", header: "Description", width: Widths.AnsiChars(40));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
