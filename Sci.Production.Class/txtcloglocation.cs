using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sci.Win.UI;
using Sci.Data;
using Ict.Win;

namespace Sci.Production.Class
{
    public partial class txtcloglocation : Sci.Win.UI.TextBox
    {
        public txtcloglocation()
        {
            this.Size = new System.Drawing.Size(80, 23);
            this.IsSupportSytsemContextMenu = false;
        }

        private Control mDivisionObject;
        [Category("Custom Properties")]
        public Control MDivisionObjectName
        {
            set { this.mDivisionObject = value; }
            get { return this.mDivisionObject; }
        }

        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {            
            base.OnPopUp(e);
            if (e.IsHandled) return;

            Sci.Win.Tems.Base myform = (Sci.Win.Tems.Base)this.FindForm();
            if (myform.EditMode)
            {
                string sql = "select ID,Description,MDivisionID from ClogLocation order by ID";
                if (this.mDivisionObject != null && !string.IsNullOrWhiteSpace((string)this.mDivisionObject.Text))
                {
                    sql = string.Format("select ID,Description,MDivisionID from ClogLocation where MDivisionID = '{0}' order by ID", this.mDivisionObject.Text);
                }
                DataTable tbClogLocation;
                DBProxy.Current.Select("Production", sql, out tbClogLocation);
                Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(tbClogLocation, "ID,Description,MDivisionID", "10,40,10", this.Text, "ID,Description,M");

                DialogResult result = item.ShowDialog();
                if (result == DialogResult.Cancel) { return; }
                this.Text = item.GetSelectedString();
                e.IsHandled = true;
            }
        }

        protected override void OnValidating(CancelEventArgs e)
        {
            base.OnValidating(e);

            string str = this.Text;
            if (!MyUtility.Check.Empty(str))
            {
                if (str != this.OldValue)
                {
                    if (MyUtility.Check.Seek(str, "ClogLocation", "id") == false)
                    {
                        MyUtility.Msg.WarningBox(string.Format("< ClogLocation : {0} > not found!!!", str));
                        this.Text = "";
                        e.Cancel = true;
                        return;
                    }
                }
                else
                {
                    if (this.mDivisionObject != null)
                    {
                        if (!string.IsNullOrWhiteSpace((string)this.mDivisionObject.Text))
                        {
                            string selectCommand = string.Format("select ID from ClogLocation where MDivisionID = '{0}' and ID = '{1}'", (string)this.mDivisionObject.Text, str);
                            if (!MyUtility.Check.Seek(selectCommand, null))
                            {
                                MyUtility.Msg.WarningBox(string.Format("< ClogLocation : {0} > not found!!!", str));
                                this.Text = "";
                                e.Cancel = true;
                                return;
                            }
                        }
                    }
                }
            }
        }
    }
}
