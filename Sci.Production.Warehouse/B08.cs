using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class B08 : Sci.Win.Tems.Input1
    {
        private string codeKey;

        /// <inheritdoc/>
        public B08(ToolStripMenuItem menuitem, string type)
            : base(menuitem)
        {
            this.InitializeComponent();
            switch (type)
            {
                case "QAB27":
                    this.Text = "B27 Fabric Inspection Related Mail Group";
                    this.DefaultFilter = "Code = '007'";
                    this.codeKey = "007";
                    break;
                case "WHB08":
                    this.Text = "B08 Material Exception Mail To";
                    this.DefaultFilter = "Code = '104'";
                    this.codeKey = "104";
                    break;
                default:
                    this.Text = "B08 Material Exception Mail To";
                    this.DefaultFilter = "Code = '104'";
                    this.codeKey = "104";
                    break;
            }
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["FactoryID"]))
            {
                MyUtility.Msg.WarningBox($"Factory can't empty!");
                return false;
            }

            if (this.IsDetailInserting)
            {
                string sqlcmd = $"select 1 from MailGroup where Code = '{this.codeKey}' and FactoryID = '{this.CurrentMaintain["FactoryID"]}'";
                if (MyUtility.Check.Seek(sqlcmd))
                {
                    MyUtility.Msg.WarningBox($"Factory {this.CurrentMaintain["FactoryID"]} already exists.");
                    return false;
                }
            }

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["Code"] = $"{this.codeKey}";
        }
    }
}
