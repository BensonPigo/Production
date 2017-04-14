using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Basic
{
    public partial class B02_MailTo_Detail : Sci.Win.Subs.Input6A
    {
        public B02_MailTo_Detail()
        {
            InitializeComponent();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            OnAttached(CurrentData);
        }

        protected override void OnAttached(DataRow data)
        {
            base.OnAttached(data);

            if (EditMode)
            {
                txtCode.ReadOnly = !MyUtility.Check.Empty(data["id"]);
            }
        }

        protected override bool DoSave()
        {
            if (MyUtility.Check.Empty(txtCode.Text))
            {
                MyUtility.Msg.WarningBox("< Code > can not be empty!");
                this.txtCode.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(editMailTo.Text))
            {
                MyUtility.Msg.WarningBox("< Mail to > can not be empty!");
                this.editMailTo.Focus();
                return false;
            }

            return base.DoSave();
        }
    }
}
