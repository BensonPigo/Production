using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Sci.Data;
using Ict;

namespace Sci.Production.Shipping
{
    public partial class B04 : Sci.Win.Tems.Input1
    {
        public B04(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            MyUtility.Tool.SetupCombox(comboBox1, 2, 1, "C,CBM,G,G.W.,,Number of Deliver Sheets");
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            //撈Account Name資料
            string selectCommand = string.Format("select Name from AccountNo where ID = '{0}'", CurrentMaintain["AccountID"].ToString());
            DataTable AccountNoTable;
            DualResult selectResult = DBProxy.Current.Select("Finance", selectCommand, out AccountNoTable);
            if (AccountNoTable != null && AccountNoTable.Rows.Count > 0)
            {
                this.displayBox2.Text = MyUtility.Convert.GetString(AccountNoTable.Rows[0]["Name"]);
            }
            else
            {
                this.displayBox2.Text = "";
            }
        }
    }
}
