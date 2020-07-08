using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using Ict.Win;
using Ict;
using Sci.Data;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// P40_AssignNLCode
    /// </summary>
    public partial class P40_AssignNLCode : Sci.Win.Subs.Base
    {
        private DataTable noNLCode;
        private DataTable notInPo;
        private DataTable unitNotFound;
        private DataRow masterRow;
        private Ict.Win.DataGridViewGeneratorTextColumnSettings nlcode = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
        private Ict.Win.DataGridViewGeneratorTextColumnSettings nlcode2 = new Ict.Win.DataGridViewGeneratorTextColumnSettings();

        /// <summary>
        /// NoNLCode
        /// </summary>
        public DataTable NoNLCode
        {
            get
            {
                return this.noNLCode;
            }

            set
            {
                this.noNLCode = value;
            }
        }

        /// <summary>
        /// NotInPo
        /// </summary>
        public DataTable NotInPo
        {
            get
            {
                return this.notInPo;
            }

            set
            {
                this.notInPo = value;
            }
        }

        /// <summary>
        /// UnitNotFound
        /// </summary>
        public DataTable UnitNotFound
        {
            get
            {
                return this.unitNotFound;
            }

            set
            {
                this.unitNotFound = value;
            }
        }

        /// <summary>
        /// P40_AssignNLCode
        /// </summary>
        /// <param name="noNLCode">noNLCode</param>
        /// <param name="notInPO">notInPO</param>
        /// <param name="unitNotFound">unitNotFound</param>
        /// <param name="masterData">masterData</param>
        public P40_AssignNLCode(DataTable noNLCode, DataTable notInPO, DataTable unitNotFound, DataRow masterData)
        {
            this.InitializeComponent();
            this.NoNLCode = noNLCode;
            this.NotInPo = notInPO;
            this.UnitNotFound = unitNotFound;
            this.masterRow = masterData;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            #region NL Code的Validating
            this.nlcode.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.gridBelowDataIsnoNLCode.GetDataRow<DataRow>(e.RowIndex);
                    if (!MyUtility.Check.Empty(e.FormattedValue) && MyUtility.Convert.GetString(dr["NLCode"]) != MyUtility.Convert.GetString(e.FormattedValue))
                    {
                        DataRow seekData;
                        if (MyUtility.Check.Seek(string.Format("select NLCode,HSCode,UnitID from VNContract_Detail WITH (NOLOCK) where ID = '{0}' and NLCode = '{1}'", MyUtility.Convert.GetString(this.masterRow["VNContractID"]), MyUtility.Convert.GetString(e.FormattedValue)), out seekData))
                        {
                            dr["NLCode"] = e.FormattedValue;
                            dr["HSCode"] = seekData["HSCode"];
                            dr["CustomsUnit"] = seekData["UnitID"];
                        }
                        else
                        {
                            dr["NLCode"] = string.Empty;
                            dr["HSCode"] = string.Empty;
                            dr["CustomsUnit"] = string.Empty;
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox("Customs Code not found!!");
                            return;
                        }
                    }
                }
            };

            this.nlcode2.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.gridBelowDataisNotInpurchaseorder.GetDataRow<DataRow>(e.RowIndex);
                    if (!MyUtility.Check.Empty(e.FormattedValue) && MyUtility.Convert.GetString(dr["NLCode"]) != MyUtility.Convert.GetString(e.FormattedValue))
                    {
                        DataRow seekData;
                        if (MyUtility.Check.Seek(string.Format("select NLCode,HSCode,UnitID from VNContract_Detail WITH (NOLOCK) where ID = '{0}' and NLCode = '{1}'", MyUtility.Convert.GetString(this.masterRow["VNContractID"]), MyUtility.Convert.GetString(e.FormattedValue)), out seekData))
                        {
                            dr["NLCode"] = e.FormattedValue;
                            dr["HSCode"] = seekData["HSCode"];
                            dr["CustomsUnit"] = seekData["UnitID"];
                        }
                        else
                        {
                            dr["NLCode"] = string.Empty;
                            dr["HSCode"] = string.Empty;
                            dr["CustomsUnit"] = string.Empty;
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox("Customs Code not found!!");
                            return;
                        }
                    }
                }
            };
            #endregion

            this.gridBelowDataIsnoNLCode.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.gridBelowDataIsnoNLCode)
                .Text("Refno", header: "Ref No.", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("BrandID", header: "Brand", width: Widths.AnsiChars(7), iseditingreadonly: true)
                .Text("Type", header: "Type", width: Widths.AnsiChars(7), iseditingreadonly: true)
                .Text("Description", header: "Description", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("NLCode", header: "Customs Code", width: Widths.AnsiChars(12), settings: this.nlcode)
                .Text("HSCode", header: "HS Code", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("CustomsUnit", header: "Unit", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Numeric("PcsWidth", header: "Width/Pcs(M)", decimal_places: 3)
                .Numeric("PcsLength", header: "Length/Pcs(M)", decimal_places: 3)
                .Numeric("PcsKg", header: "KG/Pcs", decimal_places: 3)
                .CheckBox("NoDeclare", header: "No need to declare", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0);

            this.gridBelowDataIsnoNLCode.Columns["NLCode"].DefaultCellStyle.BackColor = Color.LightYellow;
            this.gridBelowDataIsnoNLCode.Columns["PcsWidth"].DefaultCellStyle.BackColor = Color.LightYellow;
            this.gridBelowDataIsnoNLCode.Columns["PcsLength"].DefaultCellStyle.BackColor = Color.LightYellow;
            this.gridBelowDataIsnoNLCode.Columns["PcsKg"].DefaultCellStyle.BackColor = Color.LightYellow;
            this.gridBelowDataIsnoNLCode.Columns["NoDeclare"].DefaultCellStyle.BackColor = Color.LightYellow;

            this.gridBelowDataisNotInpurchaseorder.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.gridBelowDataisNotInpurchaseorder)
                .Text("ID", header: "WK No.", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("POID", header: "SP#", width: Widths.AnsiChars(7), iseditingreadonly: true)
                .Text("Seq", header: "Seq", width: Widths.AnsiChars(7), iseditingreadonly: true)
                .Text("Description", header: "Description", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("Type", header: "Type", width: Widths.AnsiChars(7), iseditingreadonly: true)
                .Text("NLCode", header: "Customs Code", width: Widths.AnsiChars(12), settings: this.nlcode2)
                .Text("HSCode", header: "HS Code", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("CustomsUnit", header: "Unit", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Numeric("PcsWidth", header: "Width/Pcs(M)", decimal_places: 3)
                .Numeric("PcsLength", header: "Length/Pcs(M)", decimal_places: 3)
                .Numeric("PcsKg", header: "KG/Pcs", decimal_places: 3)
                .CheckBox("NoDeclare", header: "No need to declare", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0);

            this.gridBelowDataisNotInpurchaseorder.Columns["NLCode"].DefaultCellStyle.BackColor = Color.LightYellow;
            this.gridBelowDataisNotInpurchaseorder.Columns["PcsWidth"].DefaultCellStyle.BackColor = Color.LightYellow;
            this.gridBelowDataisNotInpurchaseorder.Columns["PcsLength"].DefaultCellStyle.BackColor = Color.LightYellow;
            this.gridBelowDataisNotInpurchaseorder.Columns["PcsKg"].DefaultCellStyle.BackColor = Color.LightYellow;
            this.gridBelowDataisNotInpurchaseorder.Columns["NoDeclare"].DefaultCellStyle.BackColor = Color.LightYellow;

            this.gridBelowDataisNoTransferformula.IsEditingReadOnly = true;
            this.Helper.Controls.Grid.Generator(this.gridBelowDataisNoTransferformula)
                .Text("OriUnit", header: "Unit", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("CustomsUnit", header: "Customs Unit", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("RefNo", header: "Ref No.", width: Widths.AnsiChars(20), iseditingreadonly: true);

            this.listControlBindingSource1.DataSource = this.NoNLCode;
            this.listControlBindingSource2.DataSource = this.NotInPo;
            this.listControlBindingSource3.DataSource = this.UnitNotFound;
        }

        // Save NL Code & Re-Calculate
        private void BtnSaveNLCode_Click(object sender, EventArgs e)
        {
            this.gridBelowDataIsnoNLCode.ValidateControl();
            this.listControlBindingSource1.EndEdit();

            DataRow[] dataCheck = this.NoNLCode.Select("NLCode = ''");
            if (dataCheck.Length < 0)
            {
                MyUtility.Msg.WarningBox("Customs Code can't empty!!");
                return;
            }

            dataCheck = this.NotInPo.Select("NLCode = ''");
            if (dataCheck.Length < 0)
            {
                MyUtility.Msg.WarningBox("Customs Code can't empty!!");
                return;
            }

            IList<string> updateCmds = new List<string>();
            foreach (DataRow dr in this.NoNLCode.Rows)
            {
                if (MyUtility.Convert.GetString(dr["Type"]) == "F" || MyUtility.Convert.GetString(dr["Type"]) == "A")
                {
                    updateCmds.Add(string.Format(
                        "update Fabric set NLCode = '{0}', HSCode = '{1}', CustomsUnit = '{2}', PcsWidth = {3}, PcsLength = {4}, PcsKg = {5},NoDeclare = {6},NLCodeEditName = '{7}',NLCodeEditDate = GETDATE() where SCIRefno = '{8}';",
                        MyUtility.Convert.GetString(dr["NLCode"]),
                        MyUtility.Convert.GetString(dr["HSCode"]),
                        MyUtility.Convert.GetString(dr["CustomsUnit"]),
                        MyUtility.Check.Empty(dr["PcsWidth"]) ? "0" : MyUtility.Convert.GetString(dr["PcsWidth"]),
                        MyUtility.Check.Empty(dr["PcsLength"]) ? "0" : MyUtility.Convert.GetString(dr["PcsLength"]),
                        MyUtility.Check.Empty(dr["PcsKg"]) ? "0" : MyUtility.Convert.GetString(dr["PcsKg"]),
                        MyUtility.Convert.GetString(dr["NoDeclare"]),
                        Sci.Env.User.UserID,
                        MyUtility.Convert.GetString(dr["SCIRefno"]).Replace("'", "''")));
                }
                else
                {
                    updateCmds.Add(string.Format(
                        "update LocalItem set NLCode = '{0}', HSCode = '{1}', CustomsUnit = '{2}', PcsWidth = {3}, PcsLength = {4}, PcsKg = {5},NoDeclare = {6},NLCodeEditName = '{7}',NLCodeEditDate = GETDATE() where RefNo = '{8}';",
                        MyUtility.Convert.GetString(dr["NLCode"]),
                        MyUtility.Convert.GetString(dr["HSCode"]),
                        MyUtility.Convert.GetString(dr["CustomsUnit"]),
                        MyUtility.Check.Empty(dr["PcsWidth"]) ? "0" : MyUtility.Convert.GetString(dr["PcsWidth"]),
                        MyUtility.Check.Empty(dr["PcsLength"]) ? "0" : MyUtility.Convert.GetString(dr["PcsLength"]),
                        MyUtility.Check.Empty(dr["PcsKg"]) ? "0" : MyUtility.Convert.GetString(dr["PcsKg"]),
                        MyUtility.Convert.GetString(dr["NoDeclare"]),
                        Sci.Env.User.UserID,
                        MyUtility.Convert.GetString(dr["RefNo"]).Replace("'", "''")));
                }
            }

            if (updateCmds.Count > 0)
            {
                DualResult result = DBProxy.Current.Executes(null, updateCmds);
                if (!result)
                {
                    MyUtility.Msg.WarningBox("Save data fail!! Please try again.\r\n{0}", result.ToString());
                    return;
                }
            }

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
