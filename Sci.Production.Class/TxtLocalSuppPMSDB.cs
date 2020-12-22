using Ict;
using Sci.Data;
using System;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

namespace Sci.Production.Class
{
    /// <summary>
    /// TxtLocalSuppPMSDB
    /// </summary>
    public partial class TxtLocalSuppPMSDB : Win.UI._UserControl
    {
        /// <summary>
        /// 是否要顯示 Junk 的資料
        /// </summary>
        [Description("是否要顯示 LocalSupp.IsFactory 的資料")]
        public bool IsFactory { get; set; } = false;

        /// <summary>
        /// 連線字串
        /// </summary>
        [Description("連線字串")]
        public string ConnectionName { get; set; } = "ProductionTPE";

        /// <summary>
        /// Initializes a new instance of the <see cref="TxtLocalSuppPMSDB"/> class.
        /// </summary>
        public TxtLocalSuppPMSDB()
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
                string sql = string.Format(
                    @"
select l.ID
from 
(
	select l.ID from [PMSDB\PH1].Production.dbo.LocalSupp l where l.Junk = 0 {1} union
	select l.ID from [PMSDB\PH2].Production.dbo.LocalSupp l where l.Junk = 0 {1} union
	select l.ID from [PMSDB\ESP].Production.dbo.LocalSupp l where l.Junk = 0 {1} union
	select l.ID from [PMSDB\SNP].Production.dbo.LocalSupp l where l.Junk = 0 {1} union
	select l.ID from [PMSDB\SPT].Production.dbo.LocalSupp l where l.Junk = 0 {1} union
	select l.ID from [PMSDB\SPS].Production.dbo.LocalSupp l where l.Junk = 0 {1} union
	select l.ID from [PMSDB\SPR].Production.dbo.LocalSupp l where l.Junk = 0 {1} union
	select l.ID from [PMSDB\HZG].Production.dbo.LocalSupp l where l.Junk = 0 {1} union
	select l.ID from [PMSDB\HXG].Production.dbo.LocalSupp l where l.Junk = 0 {1} union
	select l.ID from [PMSDB\NAI].Production.dbo.LocalSupp l where l.Junk = 0 {1}
) l
where l.ID = '{0}'
group by l.ID
",
                    textValue,
                    this.IsFactory ? "and IsFactory = 1" : string.Empty);

                if (!MyUtility.Check.Seek(sql, this.ConnectionName))
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

            string sql = string.Format(
                @"
select l.ID
	, [Name] = MAX(Name)
	, [Abb] = MAX(Abb)
from 
(
	select l.ID,l.Name,l.Abb from [PMSDB\PH1].Production.dbo.LocalSupp l where l.Junk = 0 {0} union
	select l.ID,l.Name,l.Abb from [PMSDB\PH2].Production.dbo.LocalSupp l where l.Junk = 0 {0} union
	select l.ID,l.Name,l.Abb from [PMSDB\ESP].Production.dbo.LocalSupp l where l.Junk = 0 {0} union
	select l.ID,l.Name,l.Abb from [PMSDB\SNP].Production.dbo.LocalSupp l where l.Junk = 0 {0} union
	select l.ID,l.Name,l.Abb from [PMSDB\SPT].Production.dbo.LocalSupp l where l.Junk = 0 {0} union
	select l.ID,l.Name,l.Abb from [PMSDB\SPS].Production.dbo.LocalSupp l where l.Junk = 0 {0} union
	select l.ID,l.Name,l.Abb from [PMSDB\SPR].Production.dbo.LocalSupp l where l.Junk = 0 {0} union
	select l.ID,l.Name,l.Abb from [PMSDB\HZG].Production.dbo.LocalSupp l where l.Junk = 0 {0} union
	select l.ID,l.Name,l.Abb from [PMSDB\HXG].Production.dbo.LocalSupp l where l.Junk = 0 {0} union
	select l.ID,l.Name,l.Abb from [PMSDB\NAI].Production.dbo.LocalSupp l where l.Junk = 0 {0}
) l
group by l.ID 
order by l.ID
",
                this.IsFactory ? "and IsFactory = 1" : string.Empty);

            DualResult result = DBProxy.Current.Select(this.ConnectionName, sql, out DataTable dt);
            Win.Tools.SelectItem item = new Win.Tools.SelectItem(dt, "ID,Name,Abb", "8,30,20", this.TextBox1.Text)
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
            string sql = string.Format(
                @"
select l.ID
	, [Name] = MAX(Name)
	, [Abb] = MAX(Abb)
from 
(
	select l.ID,l.Name,l.Abb from [PMSDB\PH1].Production.dbo.LocalSupp l where l.Junk = 0 {1} union
	select l.ID,l.Name,l.Abb from [PMSDB\PH2].Production.dbo.LocalSupp l where l.Junk = 0 {1} union
	select l.ID,l.Name,l.Abb from [PMSDB\ESP].Production.dbo.LocalSupp l where l.Junk = 0 {1} union
	select l.ID,l.Name,l.Abb from [PMSDB\SNP].Production.dbo.LocalSupp l where l.Junk = 0 {1} union
	select l.ID,l.Name,l.Abb from [PMSDB\SPT].Production.dbo.LocalSupp l where l.Junk = 0 {1} union
	select l.ID,l.Name,l.Abb from [PMSDB\SPS].Production.dbo.LocalSupp l where l.Junk = 0 {1} union
	select l.ID,l.Name,l.Abb from [PMSDB\SPR].Production.dbo.LocalSupp l where l.Junk = 0 {1} union
	select l.ID,l.Name,l.Abb from [PMSDB\HZG].Production.dbo.LocalSupp l where l.Junk = 0 {1} union
	select l.ID,l.Name,l.Abb from [PMSDB\HXG].Production.dbo.LocalSupp l where l.Junk = 0 {1} union
	select l.ID,l.Name,l.Abb from [PMSDB\NAI].Production.dbo.LocalSupp l where l.Junk = 0 {1}
) l
where l.ID = '{0}'
group by l.ID 
order by l.ID
",
                this.TextBox1.Text.ToString(),
                this.IsFactory ? "and IsFactory = 1" : string.Empty);
            DualResult result = DBProxy.Current.Select(this.ConnectionName, sql, out DataTable dt);
            if (result)
            {
                this.DisplayBox1.Text = dt.Rows[0]["Name"].ToString();
            }
        }
    }
}
