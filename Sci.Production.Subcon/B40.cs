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

namespace Sci.Production.Subcon
{
    public partial class B40 : Sci.Win.Tems.Input1
    {
        public B40(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            comboload();
        }

        private void comboload()
        {            
            DataTable dtSubprocessID;
            DualResult Result;
            if (Result = DBProxy.Current.Select(null, "select ID from Subprocess WITH (NOLOCK) where Junk = '0'", out dtSubprocessID))
            {
                this.comboSubprocess.DataSource = dtSubprocessID;
                this.comboSubprocess.DisplayMember = "ID";
            }
            else { ShowErr(Result); }

            Dictionary<String, String> comboType_RowSource = new Dictionary<string, string>();
            comboType_RowSource.Add("1", "In");
            comboType_RowSource.Add("2", "Out");
            comboType_RowSource.Add("3", "In/Out");
            comboStockType.DataSource = new BindingSource(comboType_RowSource, null);
            comboStockType.ValueMember = "Key";
            comboStockType.DisplayMember = "Value";
        
        }
        protected override void ClickCopyAfter()
        {
            txtID.ReadOnly = false;
        }

        protected override bool ClickNew()
        {
            txtID.ReadOnly = false;
            return base.ClickNew();
        }

        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(txtID.Text))
            {
                MyUtility.Msg.WarningBox("ID can not empty!");
                return false;
            }
            if (MyUtility.Check.Empty(comboSubprocess.Text)|| MyUtility.Check.Empty(comboStockType.Text))
            {
                MyUtility.Msg.WarningBox("Sub-process and Stock Type can not empty!");
                return false;
            }
            
            return base.ClickSaveBefore();
        }

        protected override void ClickSaveAfter()
        {
            base.ClickSaveAfter();
            txtID.ReadOnly = true;
        }
        protected override void ClickUndo()
        {
            base.ClickUndo();
            this.txtID.ReadOnly = true;
        }
    }
}
