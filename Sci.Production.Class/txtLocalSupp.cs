﻿using Ict;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Sci.Production.Class
{
    /// <summary>
    /// TxtLocalSupp
    /// </summary>
    public partial class TxtLocalSupp : Win.UI._UserControl
    {
        /// <summary>
        /// 是否要顯示 IsFactory 的資料
        /// </summary>
        [Description("是否只顯示 LocalSupp.IsFactory 的資料")]
        public bool IsFactory { get; set; } = false;

        /// <summary>
        /// 是否要顯示 IsMisc 的資料
        /// </summary>
        [Description("是否只顯示 LocalSupp.IsMisc 的資料")]
        public bool IsMisc { get; set; } = false;

        /// <summary>
        /// 是否要顯示 Is Misc Overseas 的資料
        /// </summary>
        [Description("是否只顯示 LocalSupp.IsMiscOverseas 的資料")]
        public bool IsMiscOverseas { get; set; } = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="TxtLocalSupp"/> class.
        /// </summary>
        public TxtLocalSupp()
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        /// <inheritdoc/>
        public Win.UI.TextBox TextBox1 { get; private set; }

        /// <inheritdoc/>
        public Win.UI.DisplayBox DisplayBox1 { get; set; }

        /// <inheritdoc/>
        [Bindable(true)]
        public string TextBox1Binding
        {
            get { return this.TextBox1.Text; }
            set { this.TextBox1.Text = value;  }
        }

        /// <inheritdoc/>
        [Bindable(true)]
        public string DisplayBox1Binding
        {
            get { return this.DisplayBox1.Text; }
            set { this.DisplayBox1.Text = value; }
        }

        private void TextBox1_Validating(object sender, CancelEventArgs e)
        {
           // base.OnValidating(e);
            string textValue = this.TextBox1.Text;

            if (!string.IsNullOrWhiteSpace(textValue) && textValue != this.TextBox1.OldValue)
            {
                string sql = $@"
select l.ID
from dbo.LocalSupp l WITH (NOLOCK) 
where l.Junk=0 AND l.ID ='{textValue}'
";
                if (this.IsFactory)
                {
                    sql += "and l.IsFactory = 1" + Environment.NewLine;
                }

                if (this.IsMisc)
                {
                    sql += "and l.IsMisc = 1" + Environment.NewLine;
                }

                if (this.IsMiscOverseas)
                {
                    sql += "and l.IsMiscOverseas = 1" + Environment.NewLine;
                }

                if (!MyUtility.Check.Seek(sql))
                {
                    this.TextBox1.Text = string.Empty;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("< LocalSupplier Code: {0} > not found!!!", textValue));
                    return;
                }
            }

            this.ValidateControl();
        }

        private void TextBox1_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Win.Forms.Base myForm = (Win.Forms.Base)this.FindForm();
            if (myForm.EditMode == false || this.TextBox1.ReadOnly == true)
            {
                return;
            }

            string sql = $@"
select l.ID,l.Name,l.Abb 
from dbo.LocalSupp l WITH (NOLOCK) 
where l.Junk=0 
";
            if (this.IsFactory)
            {
                sql += "and l.IsFactory = 1" + Environment.NewLine;
            }

            if (this.IsMisc)
            {
                sql += "and l.IsMisc = 1" + Environment.NewLine;
            }

            if (this.IsMiscOverseas)
            {
                sql += "and l.IsMiscOverseas = 1" + Environment.NewLine;
            }

            sql += "order by l.ID" + Environment.NewLine;

            Win.Tools.SelectItem item = new Win.Tools.SelectItem(
                sql,
                "8,30,20",
                this.TextBox1.Text)
            {
                Width = 650,
            };
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.TextBox1.Text = item.GetSelectedString();
            this.ValidateControl();
            this.DisplayBox1.Text = item.GetSelecteds()[0]["Name"].ToString().TrimEnd();
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            this.DisplayBox1.Text = MyUtility.GetValue.Lookup("Abb", this.TextBox1.Text.ToString(), "LocalSupp", "ID");
        }
    }
}
