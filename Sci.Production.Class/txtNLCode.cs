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
        private DataRow mainDataRow;

        /// <summary>
        /// MainDataRow
        /// </summary>
        public DataRow MainDataRow
        {
            get { return this.mainDataRow; } set { this.mainDataRow = value; }
        }

        /// <summary>
        /// HSCode
        /// </summary>
        public string HSCode
        {
            get { return this.hSCode; }
        }

        /// <summary>
        /// HSCode
        /// </summary>
        public string CustomsUnit
        {
            get { return this.customsUnit; }
        }

        /// <summary>
        /// HSCode
        /// </summary>
        public DataRow HSCodeCustomsUnit { get
            {
                DataRow drResult;
                string getNLCodeInfo = string.Format(
                        @"select NLCode,HSCode,UnitID
from VNContract_Detail WITH (NOLOCK) 
where ID in (select ID from VNContract WITH (NOLOCK) WHERE StartDate = (select MAX(StartDate) as MaxDate from VNContract WITH (NOLOCK) where Status = 'Confirmed') )
and NLCode = '{0}'", this.Text);
                MyUtility.Check.Seek(getNLCodeInfo, out drResult);
                return drResult;
            }
        }

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

            if (this.MainDataRow != null)
            {
                this.MainDataRow[this.DataBindings[0].BindingMemberInfo.BindingField] = selectedData[0]["NLCode"].ToString();
                this.OldValue = selectedData[0]["NLCode"].ToString();
                if (this.IsNeedUpdateHSCodeAndCustomsUnit(this.MainDataRow, selectedData[0]))
                {
                    this.hSCode = selectedData[0]["HSCode"].ToString();
                    this.MainDataRow["HSCode"] = selectedData[0]["HSCode"].ToString();

                    this.customsUnit = selectedData[0]["UnitID"].ToString();
                    this.MainDataRow["CustomsUnit"] = selectedData[0]["UnitID"].ToString();
                }
            }
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
                DataRow nLCodeDate = this.HSCodeCustomsUnit;
                if (nLCodeDate != null)
                {
                    this.MainDataRow[this.DataBindings[0].BindingMemberInfo.BindingField] = nLCodeDate["NLCode"].ToString();

                    if (this.IsNeedUpdateHSCodeAndCustomsUnit(this.MainDataRow, nLCodeDate))
                    {
                        this.hSCode = nLCodeDate["HSCode"].ToString();
                        this.MainDataRow["HSCode"] = nLCodeDate["HSCode"].ToString();

                        this.customsUnit = nLCodeDate["UnitID"].ToString();
                        this.MainDataRow["CustomsUnit"] = nLCodeDate["UnitID"].ToString();
                    }
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
        }

        private bool IsNeedUpdateHSCodeAndCustomsUnit(DataRow curDataRow, DataRow newDataRow)
        {
            if (MyUtility.Check.Empty(curDataRow["CustomsUnit"]) &&
                MyUtility.Check.Empty(curDataRow["HSCode"]))
            {
                return true;
            }

            bool isCustomsUnitChanged = curDataRow["CustomsUnit"].ToString() != newDataRow["UnitID"].ToString();

            bool isHSCodeChanged = curDataRow["HSCode"].ToString() != newDataRow["HSCode"].ToString();

            DialogResult dialogResult = DialogResult.Yes;

            if (isCustomsUnitChanged || isHSCodeChanged)
            {
               dialogResult = MyUtility.Msg.QuestionBox($@"{newDataRow["NLCode"]} [HS Code] or [Customs Unit] does not match the current data. Do you want to overwrite the data?");
            }

            return dialogResult == DialogResult.Yes;
        }
    }
}
