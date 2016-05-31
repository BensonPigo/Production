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
                textBox1.ReadOnly = data.RowState == DataRowState.Added ? false : true;
            }
        }

        protected override bool DoSave()
        {
            if (MyUtility.Check.Empty(textBox1.Text))
            {
                MyUtility.Msg.WarningBox("< Code > can not be empty!");
                this.textBox1.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(editBox1.Text))
            {
                MyUtility.Msg.WarningBox("< Mail to > can not be empty!");
                this.editBox1.Focus();
                return false;
            }

            return base.DoSave();
        }
    }
}
