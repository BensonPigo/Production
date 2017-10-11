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

namespace Sci.Production.Shipping
{
    public partial class P40_AssignNLCode : Sci.Win.Subs.Base
    {
        public DataTable noNLCode, notInPo, unitNotFound;
        DataRow masterRow;
        Ict.Win.DataGridViewGeneratorTextColumnSettings nlcode = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
        Ict.Win.DataGridViewGeneratorTextColumnSettings nlcode2 = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
        public P40_AssignNLCode(DataTable NoNLCode, DataTable NotInPO, DataTable UnitNotFound, DataRow MasterData)
        {
            InitializeComponent();
            noNLCode = NoNLCode;
            notInPo = NotInPO;
            unitNotFound = UnitNotFound;
            masterRow = MasterData;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            #region NL Code的Validating
            nlcode.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.gridBelowDataIsnoNLCode.GetDataRow<DataRow>(e.RowIndex);
                    if (!MyUtility.Check.Empty(e.FormattedValue) && MyUtility.Convert.GetString(dr["NLCode"]) != MyUtility.Convert.GetString(e.FormattedValue))
                    {
                        DataRow seekData;
                        if (MyUtility.Check.Seek(string.Format("select NLCode,HSCode,UnitID from VNContract_Detail WITH (NOLOCK) where ID = '{0}' and NLCode = '{1}'", MyUtility.Convert.GetString(masterRow["VNContractID"]), MyUtility.Convert.GetString(e.FormattedValue)), out seekData))
                        {
                            dr["NLCode"] = e.FormattedValue;
                            dr["HSCode"] = seekData["HSCode"];
                            dr["CustomsUnit"] = seekData["UnitID"];
                        }
                        else
                        {
                            dr["NLCode"] = "";
                            dr["HSCode"] = "";
                            dr["CustomsUnit"] = "";
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox("Customs Code not found!!");
                            return;
                        }
                    }
                }
            };

            nlcode2.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.gridBelowDataisNotInpurchaseorder.GetDataRow<DataRow>(e.RowIndex);
                    if (!MyUtility.Check.Empty(e.FormattedValue) && MyUtility.Convert.GetString(dr["NLCode"]) != MyUtility.Convert.GetString(e.FormattedValue))
                    {
                        DataRow seekData;
                        if (MyUtility.Check.Seek(string.Format("select NLCode,HSCode,UnitID from VNContract_Detail WITH (NOLOCK) where ID = '{0}' and NLCode = '{1}'", MyUtility.Convert.GetString(masterRow["VNContractID"]), MyUtility.Convert.GetString(e.FormattedValue)), out seekData))
                        {
                            dr["NLCode"] = e.FormattedValue;
                            dr["HSCode"] = seekData["HSCode"];
                            dr["CustomsUnit"] = seekData["UnitID"];
                        }
                        else
                        {
                            dr["NLCode"] = "";
                            dr["HSCode"] = "";
                            dr["CustomsUnit"] = "";
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox("Customs Code not found!!");
                            return;
                        }
                    }
                }
            };
            #endregion

            this.gridBelowDataIsnoNLCode.IsEditingReadOnly = false;
            Helper.Controls.Grid.Generator(gridBelowDataIsnoNLCode)
                .Text("Refno", header: "Ref No.", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("BrandID", header: "Brand", width: Widths.AnsiChars(7), iseditingreadonly: true)
                .Text("Type", header: "Type", width: Widths.AnsiChars(7), iseditingreadonly: true)
                .Text("Description", header: "Description", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("NLCode", header: "Customs Code", width: Widths.AnsiChars(8), settings: nlcode)
                .Text("HSCode", header: "HS Code", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("CustomsUnit", header: "Unit", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Numeric("PcsWidth", header: "Width/Pcs(M)", decimal_places: 3)
                .Numeric("PcsLength", header: "Length/Pcs(M)", decimal_places: 3)
                .Numeric("PcsKg", header: "KG/Pcs", decimal_places: 3)
                .CheckBox("NoDeclare", header: "No need to declare", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0);

            gridBelowDataIsnoNLCode.Columns["NLCode"].DefaultCellStyle.BackColor = Color.LightYellow;
            gridBelowDataIsnoNLCode.Columns["PcsWidth"].DefaultCellStyle.BackColor = Color.LightYellow;
            gridBelowDataIsnoNLCode.Columns["PcsLength"].DefaultCellStyle.BackColor = Color.LightYellow;
            gridBelowDataIsnoNLCode.Columns["PcsKg"].DefaultCellStyle.BackColor = Color.LightYellow;
            gridBelowDataIsnoNLCode.Columns["NoDeclare"].DefaultCellStyle.BackColor = Color.LightYellow;

            this.gridBelowDataisNotInpurchaseorder.IsEditingReadOnly = false;
            Helper.Controls.Grid.Generator(gridBelowDataisNotInpurchaseorder)
                .Text("ID", header: "WK No.", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("POID", header: "SP#", width: Widths.AnsiChars(7), iseditingreadonly: true)
                .Text("Seq", header: "Seq", width: Widths.AnsiChars(7), iseditingreadonly: true)
                .Text("Description", header: "Description", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("Type", header: "Type", width: Widths.AnsiChars(7), iseditingreadonly: true)
                .Text("NLCode", header: "Customs Code", width: Widths.AnsiChars(8), settings: nlcode2)
                .Text("HSCode", header: "HS Code", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("CustomsUnit", header: "Unit", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Numeric("PcsWidth", header: "Width/Pcs(M)", decimal_places: 3)
                .Numeric("PcsLength", header: "Length/Pcs(M)", decimal_places: 3)
                .Numeric("PcsKg", header: "KG/Pcs", decimal_places: 3)
                .CheckBox("NoDeclare", header: "No need to declare", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0);

            gridBelowDataisNotInpurchaseorder.Columns["NLCode"].DefaultCellStyle.BackColor = Color.LightYellow;
            gridBelowDataisNotInpurchaseorder.Columns["PcsWidth"].DefaultCellStyle.BackColor = Color.LightYellow;
            gridBelowDataisNotInpurchaseorder.Columns["PcsLength"].DefaultCellStyle.BackColor = Color.LightYellow;
            gridBelowDataisNotInpurchaseorder.Columns["PcsKg"].DefaultCellStyle.BackColor = Color.LightYellow;
            gridBelowDataisNotInpurchaseorder.Columns["NoDeclare"].DefaultCellStyle.BackColor = Color.LightYellow;

            this.gridBelowDataisNoTransferformula.IsEditingReadOnly = true;
            Helper.Controls.Grid.Generator(gridBelowDataisNoTransferformula)
                .Text("OriUnit", header: "Unit", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("CustomsUnit", header: "Customs Unit", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("RefNo", header: "Ref No.", width: Widths.AnsiChars(20), iseditingreadonly: true);

            listControlBindingSource1.DataSource = noNLCode;
            listControlBindingSource2.DataSource = notInPo;
            listControlBindingSource3.DataSource = unitNotFound;
        }

        //Save NL Code & Re-Calculate
        private void btnSaveNLCode_Click(object sender, EventArgs e)
        {
            this.gridBelowDataIsnoNLCode.ValidateControl();
            listControlBindingSource1.EndEdit();

            DataRow[] dataCheck = noNLCode.Select("NLCode = ''");
            if (dataCheck.Length < 0)
            {
                MyUtility.Msg.WarningBox("Customs Code can't empty!!");
                return;
            }
            dataCheck = notInPo.Select("NLCode = ''");
            if (dataCheck.Length < 0)
            {
                MyUtility.Msg.WarningBox("Customs Code can't empty!!");
                return;
            }

            IList<string> updateCmds = new List<string>();
            foreach (DataRow dr in noNLCode.Rows)
            {
                if (MyUtility.Convert.GetString(dr["Type"]) == "F" || MyUtility.Convert.GetString(dr["Type"]) == "A")
                {
                    updateCmds.Add(string.Format("update Fabric set NLCode = '{0}', HSCode = '{1}', CustomsUnit = '{2}', PcsWidth = {3}, PcsLength = {4}, PcsKg = {5},NoDeclare = {6},NLCodeEditName = '{7}',NLCodeEditDate = GETDATE() where SCIRefno = '{8}';",
                        MyUtility.Convert.GetString(dr["NLCode"]), MyUtility.Convert.GetString(dr["HSCode"]), MyUtility.Convert.GetString(dr["CustomsUnit"]),
                        MyUtility.Check.Empty(dr["PcsWidth"]) ? "0" : MyUtility.Convert.GetString(dr["PcsWidth"]), MyUtility.Check.Empty(dr["PcsLength"]) ? "0" : MyUtility.Convert.GetString(dr["PcsLength"]),
                        MyUtility.Check.Empty(dr["PcsKg"]) ? "0" : MyUtility.Convert.GetString(dr["PcsKg"]), MyUtility.Convert.GetString(dr["NoDeclare"]),
                        Sci.Env.User.UserID, MyUtility.Convert.GetString(dr["SCIRefno"])));
                }
                else
                {
                    updateCmds.Add(string.Format("update LocalItem set NLCode = '{0}', HSCode = '{1}', CustomsUnit = '{2}', PcsWidth = {3}, PcsLength = {4}, PcsKg = {5},NoDeclare = {6},NLCodeEditName = '{7}',NLCodeEditDate = GETDATE() where RefNo = '{8}';",
                        MyUtility.Convert.GetString(dr["NLCode"]), MyUtility.Convert.GetString(dr["HSCode"]), MyUtility.Convert.GetString(dr["CustomsUnit"]),
                        MyUtility.Check.Empty(dr["PcsWidth"]) ? "0" : MyUtility.Convert.GetString(dr["PcsWidth"]), MyUtility.Check.Empty(dr["PcsLength"]) ? "0" : MyUtility.Convert.GetString(dr["PcsLength"]),
                        MyUtility.Check.Empty(dr["PcsKg"]) ? "0" : MyUtility.Convert.GetString(dr["PcsKg"]), MyUtility.Convert.GetString(dr["NoDeclare"]),
                        Sci.Env.User.UserID, MyUtility.Convert.GetString(dr["RefNo"])));
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

            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
