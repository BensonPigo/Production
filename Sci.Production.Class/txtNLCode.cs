using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Sci.Win.UI;

namespace Sci.Production.Class
{
    /// <summary>
    /// txtNLCode
    /// </summary>
    public partial class TxtNLCode : Win.UI.TextBox
    {
        private string hSCode;
        private string customsUnit;

        /// <summary>
        /// HSCode
        /// </summary>
        public string HSCode { get { return this.hSCode; } }

        /// <summary>
        /// HSCode
        /// </summary>
        public string CustomsUnit { get { return this.customsUnit; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="TxtNLCode"/> class.
        /// </summary>
        public TxtNLCode()
        {
            this.Size = new System.Drawing.Size(100, 23);
        }

        /// <inheritdoc/>
        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            Win.Tools.SelectItem item = new Win.Tools.SelectItem(
                @"
select NLCode,HSCode,UnitID
from VNContract_Detail WITH (NOLOCK) 
where ID in (select ID from VNContract WITH (NOLOCK) WHERE StartDate = (select MAX(StartDate) as MaxDate from VNContract WITH (NOLOCK) where Status = 'Confirmed') )
order by NLCode",
                "5,11,8",
                this.Text,
                false,
                ",",
                headercaptions: "Customs Code, HSCode, Unit");

            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            IList<DataRow> selectedData = item.GetSelecteds();
            this.Text = item.GetSelectedString();
            this.hSCode = selectedData[0]["HSCode"].ToString();
            this.customsUnit = selectedData[0]["UnitID"].ToString();

            base.OnPopUp(e);
        }

        /// <inheritdoc/>
        protected override void OnValidating(CancelEventArgs e)
        {
            string str = this.Text;
            if (str == this.OldValue)
            {
                return;
            }

            if (MyUtility.Check.Empty(str))
            {
                this.hSCode = string.Empty;
                this.customsUnit = string.Empty;
            }
            else
            {
                DataRow nLCodeDate;
                if (MyUtility.Check.Seek(
                    string.Format(
                        @"select NLCode,HSCode,UnitID
from VNContract_Detail WITH (NOLOCK) 
where ID in (select ID from VNContract WITH (NOLOCK) WHERE StartDate = (select MAX(StartDate) as MaxDate from VNContract WITH (NOLOCK) where Status = 'Confirmed') )
and NLCode = '{0}'", this.Text), out nLCodeDate))
                {
                    this.hSCode = nLCodeDate["HSCode"].ToString();
                    this.customsUnit = nLCodeDate["UnitID"].ToString();
                }
                else
                {
                    this.Text = string.Empty;
                    this.hSCode = string.Empty;
                    this.customsUnit = string.Empty;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox("The Customs Code is not in the Contract!!");
                    base.OnValidating(e);
                    return;
                }
            }

            base.OnValidating(e);
        }
    }
}
