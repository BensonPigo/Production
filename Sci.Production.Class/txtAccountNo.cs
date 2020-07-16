using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Sci.Data;
using Sci.Win.UI;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace Sci.Production.Class
{
    /// <summary>
    /// TxtAccountNo
    /// </summary>
    public partial class TxtAccountNo : Win.UI._UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TxtAccountNo"/> class.
        /// </summary>
        public TxtAccountNo()
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        public Win.UI.TextBox TextBox1 { get; private set; }

        /// <inheritdoc/>
        public Win.UI.DisplayBox DisplayBox1 { get; private set; }

        /// <inheritdoc/>
        [Bindable(true)]
        public string TextBox1Binding
        {
            get { return this.TextBox1.Text; }
            set { this.TextBox1.Text = value; }
        }

        /// <inheritdoc/>
        [Bindable(true)]
        public string DisplayBox1Binding
        {
            get { return this.DisplayBox1.Text; }
            set { this.DisplayBox1.Text = value; }
        }

        /// <summary>
        /// 是否要顯示 Junk 的資料
        /// </summary>
        [Description("是否被Shipping B03使用")]
        public bool IsUnselectableShipB03 { get; set; } = false;

        private string oldValue = string.Empty;

        private void TextBox1_Validating(object sender, CancelEventArgs e)
        {
           // base.OnValidating(e);
            string textValue = this.TextBox1.Text;
            if (!string.IsNullOrWhiteSpace(textValue) && textValue != this.TextBox1.OldValue)
            {
                /*
                    有列在 AccountNoSetting 且 UnselectableShipB03 有勾選會計科目皆不可使用

                    判斷方法
                    1.	統治科目有勾選則『底下所有子科目』皆無法使用
                    2.	若統制科目沒有勾選則『依照子科目判斷是否有勾選』

                */

                string b03Filter = $@"
AND ID NOT IN (
		SELECT ID
		FROM AccountNoSetting 
		WHERE  LEN(ID) > 4 AND UnselectableShipB03 = 1
	)
AND SUBSTRING(ID,1,4) NOT IN (	
	SELECT ID
	FROM AccountNoSetting 
	WHERE  LEN(ID)=4 AND UnselectableShipB03 = 1
)
";

                string cmd = $@"
SELECT ID ,Name
FROM SciFMS_AccountNo
WHERE Junk = 0
{(this.IsUnselectableShipB03 ? b03Filter : string.Empty)}
AND ID = @ID
ORDER BY ID
";
                List<SqlParameter> paras = new List<SqlParameter>();
                paras.Add(new SqlParameter("@ID", textValue));

                if (!MyUtility.Check.Seek(cmd, paras))
                {
                    this.TextBox1.Text = string.Empty;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("< Account No: {0} > not found!!!", textValue));
                    return;
                }

                this.DataBindings.Cast<Binding>().ToList().ForEach(binding => binding.WriteValue());
            }
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            this.DisplayBox1.Text = MyUtility.GetValue.Lookup(string.Format(@"select name from SciFMS_AccountNo where id = '{0}' and junk = 0 ", this.TextBox1.Text));
        }

        private void TextBox1_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Win.Forms.Base myForm = (Win.Forms.Base)this.FindForm();
            if (myForm.EditMode == false || this.TextBox1.ReadOnly == true)
            {
                return;
            }

            /*
                有列在 AccountNoSetting 且 UnselectableShipB03 有勾選會計科目皆不可使用

                判斷方法
                1.	統治科目有勾選則『底下所有子科目』皆無法使用
                2.	若統制科目沒有勾選則『依照子科目判斷是否有勾選』

            */
            string b03Filter = $@"
AND ID NOT IN (
		SELECT ID
		FROM AccountNoSetting 
		WHERE  LEN(ID) > 4 AND UnselectableShipB03 = 1
	)
AND SUBSTRING(ID,1,4) NOT IN (	
	SELECT ID
	FROM AccountNoSetting 
	WHERE  LEN(ID)=4 AND UnselectableShipB03 = 1
)
";
            string cmd = $@"
SELECT ID ,Name
FROM SciFMS_AccountNo
WHERE Junk = 0
{(this.IsUnselectableShipB03 ? b03Filter : string.Empty)}
ORDER BY ID
";
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(cmd, "8,30", this.TextBox1.Text);

            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.TextBox1.Text = item.GetSelectedString();
        }

        private void TextBox1_Leave(object sender, EventArgs e)
        {
            string newValue = this.TextBox1.Text;
            if (this.oldValue != newValue)
            {
                if (MyUtility.Check.Empty(newValue))
                {
                    this.TextBox1.Text = string.Empty;
                    this.DisplayBox1.Text = string.Empty;
                    this.DataBindings.Cast<Binding>().ToList().ForEach(binding => binding.WriteValue());
                    return;
                }
                string b03Filter = $@"
AND ID NOT IN (
		SELECT ID
		FROM AccountNoSetting 
		WHERE  LEN(ID) > 4 AND UnselectableShipB03 = 1
	)
AND SUBSTRING(ID,1,4) NOT IN (	
	SELECT ID
	FROM AccountNoSetting 
	WHERE  LEN(ID)=4 AND UnselectableShipB03 = 1
)
";
                string cmd = $@"
SELECT ID ,Name
FROM SciFMS_AccountNo
WHERE Junk = 0
{(this.IsUnselectableShipB03 ? b03Filter : string.Empty)}
AND ID = @ID
ORDER BY ID
";
                List<SqlParameter> paras = new List<SqlParameter>();
                paras.Add(new SqlParameter("@ID", newValue));

                if (!MyUtility.Check.Seek(cmd, paras))
                {
                    this.TextBox1.Text = string.Empty;
                    this.DisplayBox1.Text = string.Empty;
                    MyUtility.Msg.WarningBox(string.Format("< Account No: {0} > not found!!!", newValue));
                    return;
                }

                this.oldValue = newValue;
                this.DataBindings.Cast<Binding>().ToList().ForEach(binding => binding.WriteValue());
            }
        }
    }
}
