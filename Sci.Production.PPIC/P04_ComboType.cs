using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// P04_ComboType
    /// </summary>
    public partial class P04_ComboType : Win.Subs.Input4
    {
        private string styleUnit;

        /// <summary>
        /// P04_ComboType
        /// </summary>
        /// <param name="canedit">bool canedit</param>
        /// <param name="keyvalue1">string keyvalue1</param>
        /// <param name="keyvalue2">string keyvalue2</param>
        /// <param name="keyvalue3">string keyvalue3</param>
        /// <param name="styleUnit">string styleUnit</param>
        public P04_ComboType(bool canedit, string keyvalue1, string keyvalue2, string keyvalue3, string styleUnit)
            : base(canedit, keyvalue1, keyvalue2, keyvalue3)
        {
            this.InitializeComponent();
            this.styleUnit = styleUnit;
        }

        /// <inheritdoc/>
        protected override bool OnGridSetup()
        {
            DataGridViewGeneratorTextColumnSettings combotype = new DataGridViewGeneratorTextColumnSettings();
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
                            Win.Tools.SelectItem item = new Win.Tools.SelectItem("select ID,Name from DropDownList WITH (NOLOCK) where TYPE = 'Location' order by Seq", "5,10", dr["Location"].ToString());
                            DialogResult returnResult = item.ShowDialog();
                            if (returnResult == DialogResult.Cancel)
                            {
                                return;
                            }

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
                        // sql參數
                        System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter("@combotype", e.FormattedValue.ToString());

                        IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                        cmds.Add(sp1);
                        DataTable comboType;
                        string sqlCmd = "select ID from DropDownList WITH (NOLOCK) where TYPE = 'Location' and ID = @combotype";
                        DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out comboType);

                        if (!result || comboType.Rows.Count <= 0)
                        {
                            if (!result)
                            {
                                MyUtility.Msg.WarningBox("Sql connection fail!!\r\n" + result.ToString());
                            }
                            else
                            {
                                MyUtility.Msg.WarningBox(string.Format("< Combo Type: {0} > not found!!!", e.FormattedValue.ToString()));
                            }

                            dr["Location"] = string.Empty;
                            e.Cancel = true;
                            dr.EndEdit();
                            return;
                        }
                    }
                }
            };
            #endregion
            this.Helper.Controls.Grid.Generator(this.grid)
                .Text("Location", header: "Combo Type", width: Widths.AnsiChars(1), settings: combotype)
                .Numeric("Rate", header: "Rate (%)", decimal_places: 2, integer_places: 3, maximum: 100m, minimum: 0m, width: Widths.AnsiChars(5))
                .Text("AddName", header: "Add Name", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .DateTime("AddDate", header: "Add Date", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("EditName", header: "Last Edit Name", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .DateTime("EditDate", header: "Last Edit Date", width: Widths.AnsiChars(20), iseditingreadonly: true);
            return true;
        }

        /// <inheritdoc/>
        protected override bool OnSaveBefore()
        {
            this.grid.ValidateControl();
            this.gridbs.EndEdit();
            int cnt = 0;
            decimal sumRate = 100m; // Rate加總要等於100
            foreach (DataRow dr in this.Datas)
            {
                if (MyUtility.Check.Empty(dr["Location"]))
                {
                    dr.Delete();
                    continue;
                }

                cnt++;
                sumRate = sumRate - (MyUtility.Check.Empty(dr["Rate"]) ? 0 : MyUtility.Convert.GetDecimal(dr["Rate"]));
            }

            // 如果此Style為PCS，則輸入的資料只能為1筆
            if (this.styleUnit == "PCS" && cnt != 1)
            {
                MyUtility.Msg.WarningBox("This style unit is 'PCS', so can't more than 1 record!!");
                return false;
            }

            // Rate加總要等於100
            if (sumRate != 0m)
            {
                MyUtility.Msg.WarningBox("Total Rate(%) must be 100.");
                return false;
            }

            return base.OnSaveBefore();
        }
    }
}
