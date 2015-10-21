using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;

namespace Sci.Production.PPIC
{
    public partial class P04_ComboType : Sci.Win.Subs.Input4
    {
        private string styleUnit;
        public P04_ComboType(bool canedit, string keyvalue1, string keyvalue2, string keyvalue3, string StyleUnit)
            : base(canedit, keyvalue1, keyvalue2, keyvalue3)
        {
            InitializeComponent();
            styleUnit = StyleUnit;
        }

        protected override bool OnGridSetup()
        {
            Ict.Win.DataGridViewGeneratorTextColumnSettings combotype = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
            #region Artwork Type的Right Click & Validating
            combotype.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode)
                {
                    if (e.Button == System.Windows.Forms.MouseButtons.Right)
                    {
                        if (e.RowIndex != -1)
                        {
                            DataRow dr = this.grid.GetDataRow<DataRow>(e.RowIndex);
                            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem("select ID,Name from DropDownList where TYPE = 'Location' order by Seq", "5,10", dr["Location"].ToString());
                            DialogResult returnResult = item.ShowDialog();
                            if (returnResult == DialogResult.Cancel) { return; }
                            dr["Location"] = item.GetSelectedString();
                            dr.EndEdit();
                        }
                    }
                }
            };

            combotype.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.grid.GetDataRow<DataRow>(e.RowIndex);
                    if (!string.IsNullOrWhiteSpace(e.FormattedValue.ToString()))
                    {
                        
                        if (!MyUtility.Check.Seek(string.Format("select ID from DropDownList where TYPE = 'Location' and ID = '{0}'", e.FormattedValue.ToString())))
                        {
                            MyUtility.Msg.WarningBox(string.Format("< Combo Type: {0} > not found!!!", e.FormattedValue.ToString()));
                            dr["Location"] = "";
                            e.Cancel = true;
                            dr.EndEdit();
                            return;
                        }
                        else
                        {
                            dr["Location"] = e.FormattedValue.ToString();
                            dr.EndEdit();
                        }
                    }
                }
            };
            #endregion
            Helper.Controls.Grid.Generator(this.grid)
                .Text("Location", header: "Combo Type", width: Widths.AnsiChars(1), settings: combotype)
                .Numeric("Rate", header: "Rate (%)", decimal_places: 2, integer_places: 3, maximum: 100m, minimum: 0m, width: Widths.AnsiChars(5))
                .Text("AddName", header: "Add Name", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .DateTime("AddDate", header: "Add Date", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("EditName", header: "Last Edit Name", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .DateTime("EditDate", header: "Last Edit Date", width: Widths.AnsiChars(20), iseditingreadonly: true);
            return true;
        }

        protected override bool OnSaveBefore()
        {
            grid.ValidateControl();
            gridbs.EndEdit();
            int cnt = 0;
            decimal sumRate = 100m; //Rate加總要等於100
            foreach (DataRow dr in Datas)
            {
                if (MyUtility.Check.Empty(dr["Location"]))
                {
                    dr.Delete();
                    continue;
                }
                cnt++;
                sumRate = sumRate - (MyUtility.Check.Empty(dr["Rate"]) ? 0 : Convert.ToDecimal(dr["Rate"]));
            }
            //如果此Style為PCS，則輸入的資料只能為1筆
            if (styleUnit == "PCS" && cnt != 1)
            {
                MyUtility.Msg.WarningBox("This style unit is 'PCS', so can't more than 1 record!!");
                return false;
            }

            //Rate加總要等於100
            if (sumRate != 0m)
            {
                MyUtility.Msg.WarningBox("Total Rate(%) must be 100.");
                return false;
            }
            return base.OnSaveBefore();
        }
    }
}
