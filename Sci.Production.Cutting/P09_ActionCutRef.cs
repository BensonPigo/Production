using Sci.Win.Tools;
using System;
using System.Data;
using System.Windows.Forms;
using static Sci.Production.Cutting.CuttingWorkOrder;

namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public partial class P09_ActionCutRef : Win.Tems.QueryForm
    {
        /// <inheritdoc/>
        public P09_ActionCutRef()
        {
            this.InitializeComponent();
            this.EditMode = true;
            this.Text = $"P09. {P09.dialogAction} CutRef";
            this.btnModify.Text = $"{P09.dialogAction}";
            this.txtCell.MDivisionID = Sci.Env.User.Keyword;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.SetCurrentDetailData();
        }

        private void SetCurrentDetailData()
        {
            if (P09.dialogAction == DialogAction.Edit)
            {
                this.numCutno.Text = P09.currentDetailData["CutNo"].ToString();
                this.numLayers.Text = P09.currentDetailData["Layer"].ToString();
                this.txtSeq1.Text = P09.currentDetailData["SEQ1"].ToString();
                this.txtSeq2.Text = P09.currentDetailData["SEQ2"].ToString();
                this.txtRefNo.Text = P09.currentDetailData["RefNo"].ToString();
                this.txtColor.Text = P09.currentDetailData["ColorID"].ToString();
                this.txtTone.Text = P09.currentDetailData["Tone"].ToString();
                this.numConsPC.Text = P09.currentDetailData["ConsPC"].ToString();
                this.txtMarkerName.Text = P09.currentDetailData["MarkerName"].ToString();
                this.txtMarkerNo.Text = P09.currentDetailData["MarkerNo"].ToString();
                this.txtMarkerLength.Text = P09.currentDetailData["MarkerLength"].ToString();
                this.txtActCuttingPerimeter.Text = P09.currentDetailData["ActCuttingPerimeter"].ToString();
                this.txtStraightLength.Text = P09.currentDetailData["StraightLength"].ToString();
                this.txtCurvedLength.Text = P09.currentDetailData["CurvedLength"].ToString();
                this.dateBoxEstCutDate.Value = MyUtility.Convert.GetDate(P09.currentDetailData["EstCutDate"]);
                this.txtSpreadingNo.Text = P09.currentDetailData["SpreadingNoID"].ToString();
                this.txtCell.Text = P09.currentDetailData["CutCellID"].ToString();
                this.txtDropDownList1.Text = P09.currentDetailData["Shift"].ToString();
            }
        }

        private void BtnModify_Click(object sender, EventArgs e)
        {
            if (P09.dialogAction == DialogAction.Edit)
            {
                this.Edit();
            }
            else
            {
                this.Create();
            }
        }

        private void Edit()
        {
            P09.currentDetailData["CutNo"] = this.numCutno.Value;
            P09.currentDetailData["Layer"] = this.numLayers.Value;
            P09.currentDetailData["SEQ1"] = this.txtSeq1.Text;
            P09.currentDetailData["SEQ2"] = this.txtSeq2.Text;
            P09.currentDetailData["RefNo"] = this.txtRefNo.Text;
            P09.currentDetailData["ColorID"] = this.txtColor.Text;
            P09.currentDetailData["Tone"] = this.txtTone.Text;
            P09.currentDetailData["ConsPC"] = this.numConsPC.Value;
            P09.currentDetailData["MarkerName"] = this.txtMarkerName.Text;
            P09.currentDetailData["MarkerNo"] = this.txtMarkerNo.Text;
            P09.currentDetailData["MarkerLength"] = this.txtMarkerLength.Text;
            P09.currentDetailData["ActCuttingPerimeter"] = this.txtActCuttingPerimeter.Text;
            P09.currentDetailData["StraightLength"] = this.txtStraightLength.Text;
            P09.currentDetailData["CurvedLength"] = this.txtCurvedLength.Text;
            P09.currentDetailData["MarkerLength_Mask"] = this.txtMarkerLength.Text;
            P09.currentDetailData["ActCuttingPerimeter_Mask"] = this.txtActCuttingPerimeter.Text;
            P09.currentDetailData["StraightLength_Mask"] = this.txtStraightLength.Text;
            P09.currentDetailData["CurvedLength_Mask"] = this.txtCurvedLength.Text;
            if (this.dateBoxEstCutDate.Value == null)
            {
                P09.currentDetailData["EstCutDate"] = DBNull.Value;
            }
            else
            {
                P09.currentDetailData["EstCutDate"] = this.dateBoxEstCutDate.Value;
            }

            P09.currentDetailData["SpreadingNoID"] = this.txtSpreadingNo.Text;
            P09.currentDetailData["CutCellID"] = this.txtCell.Text;
            P09.currentDetailData["Shift"] = this.txtDropDownList1.Text;



            P09.currentDetailData.EndEdit();
        }

        private void Create()
        {

        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void TxtMarkerLength_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.txtMarkerLength.Text = CuttingWorkOrder.SetMarkerLengthMaskString(this.txtMarkerLength.Text);
        }

        private void TxtActCuttingPerimeter_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.txtActCuttingPerimeter.Text = CuttingWorkOrder.SetMaskString(this.txtActCuttingPerimeter.Text);
        }

        private void TxtStraightLength_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.txtStraightLength.Text = CuttingWorkOrder.SetMaskString(this.txtStraightLength.Text);
        }

        private void TxtCurvedLength_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.txtCurvedLength.Text = CuttingWorkOrder.SetMaskString(this.txtCurvedLength.Text);
        }

        private void TxtSpreadingNo_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            SelectItem selectItem = CuttingWorkOrder.PopupSpreadingNo(this.txtSpreadingNo.Text);
            if (selectItem == null)
            {
                return;
            }

            this.txtSpreadingNo.Text = selectItem.GetSelectedString();
            this.txtCell.Text = selectItem.GetSelecteds()[0]["CutCellID"].ToString();
        }

        private void TxtSpreadingNo_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!CuttingWorkOrder.ValidatingSpreadingNo(this.txtSpreadingNo.Text, out DataRow drV))
            {
                this.txtSpreadingNo.Text = string.Empty;
                e.Cancel = true;
            }

            this.txtCell.Text = drV["CutCellID"].ToString();
        }

        private void TxtMarkerNo_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {

        }

        private void TxtMarkerNo_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }
    }
}
