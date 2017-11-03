using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Ict.Win;
using Ict;

namespace Sci.Production.Basic
{
    /// <summary>
    /// B04_BankData
    /// </summary>
    public partial class B04_BankData : Win.Subs.Input4
    {
        /// <summary>
        /// B04_BankData
        /// </summary>
        /// <param name="canedit">canedit</param>
        /// <param name="keyvalue1">keyvalue1</param>
        /// <param name="keyvalue2">keyvalue2</param>
        /// <param name="keyvalue3">keyvalue3</param>
        public B04_BankData(bool canedit, string keyvalue1, string keyvalue2, string keyvalue3)
            : base(canedit, keyvalue1, keyvalue2, keyvalue3)
        {
            this.InitializeComponent();
            this.DoForm = new B04_BankData_Input();
            this.Text = "Bank data (" + this.KeyValue1.Trim() + ")";
        }

        /// <inheritdoc/>
        protected override bool OnGridSetup()
        {
            Ict.Win.DataGridViewGeneratorTextColumnSettings ts = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
            ts.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode)
                {
                    if (e.Button == System.Windows.Forms.MouseButtons.Right)
                    {
                        if (e.RowIndex != -1)
                        {
                            DataRow dr = this.grid.GetDataRow<DataRow>(e.RowIndex);
                            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem("select ID,Alias from Country WITH (NOLOCK) where Junk = 0 order by ID", "4,30", dr["CountryID"].ToString());
                            DialogResult returnResult = item.ShowDialog();
                            if (returnResult == DialogResult.Cancel)
                            {
                                return;
                            }

                            dr["CountryID"] = item.GetSelectedString();
                            IList<DataRow> selectedData = item.GetSelecteds();
                            if (selectedData.Count > 0)
                            {
                                dr["CountryName"] = selectedData[0]["Alias"].ToString();
                            }
                        }
                    }
                }
            };

            ts.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.grid.GetDataRow<DataRow>(e.RowIndex);
                    if (!string.IsNullOrWhiteSpace(e.FormattedValue.ToString()))
                    {
                        if (!MyUtility.Check.Seek(e.FormattedValue.ToString(), "Country", "ID"))
                        {
                            dr["CountryID"] = string.Empty;
                            dr["CountryName"] = string.Empty;
                            MyUtility.Msg.WarningBox(string.Format("< Country: {0} > not found!!!", e.FormattedValue.ToString()));
                        }
                        else
                        {
                            dr["CountryID"] = e.FormattedValue.ToString().ToUpper();
                            dr["CountryName"] = MyUtility.GetValue.Lookup("Alias", dr["CountryID"].ToString(), "Country", "Id");
                        }
                    }
                }
            };

            this.Helper.Controls.Grid.Generator(this.grid)
                .CheckBox("IsDefault", header: "Default", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
                .Text("AccountNo", header: "Account No.", width: Widths.AnsiChars(20))
                .Text("SWIFTCode", header: "SWIFT", width: Widths.AnsiChars(11))
                .Text("AccountName", header: "Account Name", width: Widths.AnsiChars(20))
                .Text("BankName", header: "Bank Name", width: Widths.AnsiChars(20))
                .Text("CountryID", header: "Country", width: Widths.AnsiChars(2), settings: ts)
                .Text("CountryName", header: "Country Name", width: Widths.AnsiChars(10), iseditable: false)
                .Text("City", header: "City", width: Widths.AnsiChars(20))
                .Text("MidBankName", header: "Intermediary Bank", width: Widths.AnsiChars(20))
                .Text("MidSWIFTCode", header: "Intermediary Bank-SWIFT Code", width: Widths.AnsiChars(11))
                .Text("Remark", header: "Remark", width: Widths.AnsiChars(10))
                .Text("CreateBy", header: "Create By", width: Widths.AnsiChars(30), iseditable: false)
                .Text("EditBy", header: "Edit By", width: Widths.AnsiChars(30), iseditable: false);
            return true;
        }

        /// <inheritdoc/>
        protected override void OnRequeryPost(DataTable datas)
        {
            base.OnRequeryPost(datas);
            datas.Columns.Add("CountryName");
            datas.Columns.Add("CreateBy");
            datas.Columns.Add("EditBy");
            foreach (DataRow gridData in datas.Rows)
            {
                gridData["CountryName"] = MyUtility.GetValue.Lookup("Alias", gridData["CountryID"].ToString(), "Country", "ID");
                gridData["CreateBy"] = gridData["AddName"].ToString() + " - " + MyUtility.GetValue.Lookup("Name", gridData["AddName"].ToString(), "Pass1", "ID") + "   " + ((DateTime)gridData["AddDate"]).ToString(string.Format("{0}", Sci.Env.Cfg.DateTimeStringFormat));
                if (gridData["EditDate"] != System.DBNull.Value)
                {
                    gridData["EditBy"] = gridData["EditName"].ToString() + " - " + MyUtility.GetValue.Lookup("Name", gridData["EditName"].ToString(), "Pass1", "ID") + "   " + ((DateTime)gridData["EditDate"]).ToString(string.Format("{0}", Sci.Env.Cfg.DateTimeStringFormat));
                }

                gridData.AcceptChanges();
            }
        }

        /// <inheritdoc/>
        protected override bool OnSaveBefore()
        {
            int defaultCount = 0;

            foreach (DataRow gridData in this.Datas)
            {
                if (MyUtility.Check.Empty(gridData["AccountNo"]))
                {
                    MyUtility.Msg.WarningBox("The field < Account No. > can not be empty!");
                    return false;
                }

                if (MyUtility.Check.Empty(gridData["AccountName"]))
                {
                    MyUtility.Msg.WarningBox("The field < Account Name > can not be empty!");
                    return false;
                }

                if (MyUtility.Check.Empty(gridData["BankName"]))
                {
                    MyUtility.Msg.WarningBox("The field < Bank Name > can not be empty!");
                    return false;
                }

                if (MyUtility.Check.Empty(gridData["CountryID"]))
                {
                    MyUtility.Msg.WarningBox("The field < Country > can not be empty!");
                    return false;
                }

                if (gridData["IsDefault"].ToString() == "True")
                {
                    defaultCount = defaultCount + 1;
                }
            }

            if (defaultCount != 1)
            {
                MyUtility.Msg.WarningBox("The field < Default > can only collude and select one!");
                return false;
            }

            return base.OnSaveBefore();
        }
    }
}
