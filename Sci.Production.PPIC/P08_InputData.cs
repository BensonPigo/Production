using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Ict;
using Sci.Data;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// P08_InputData
    /// </summary>
    public partial class P08_InputData : Win.Subs.Input6A
    {
        private DataRow masterData;

        /// <summary>
        /// P08_InputData
        /// </summary>
        /// <param name="masterData">DataRow MasterData</param>
        public P08_InputData(DataRow masterData)
        {
            this.InitializeComponent();
            this.masterData = masterData;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.label10.Text = "Done 100% Fabric\r\nInspection\r\nReplacement\r\nRequest";
            this.label11.Text = "Defect  Point Under\r\n50 Per 100y in a Roll\r\n(A grade)";
            this.label12.Text = "Defect Point Over \r\n50 Per 100y in a Roll\r\n(B grade)";
            this.labelNoOfRollsOver50.Text = "No. of Rolls over 50\r\npoints per 100y";
            this.labelWidthNoOfRollsWith.Text = "Standard with / Rcvd\r\nwidth / No. of rolls with\r\nnarrow width";
            this.label24.Text = "After Cutting\r\nReplacement\r\nRequest";
        }

        /// <inheritdoc/>
        protected override void OnAttached(DataRow data)
        {
            base.OnAttached(data);
            this.editReason.Text = MyUtility.Check.Empty(data["Other"]) ? MyUtility.Convert.GetString(data["OtherReason"]) : MyUtility.GetValue.Lookup(string.Format("select Name from Reason WITH (NOLOCK) where ReasonTypeID = 'Damage Reason' and ID = '{0}'", MyUtility.Convert.GetString(data["Other"])));
            this.txtReplacementReason.Text = MyUtility.Check.Empty(data["AfterCutting"]) ? MyUtility.Convert.GetString(data["AfterCuttingReason"]) : MyUtility.GetValue.Lookup(string.Format("select Name from Reason WITH (NOLOCK) where ReasonTypeID = 'Damage Reason' and ID = '{0}'", MyUtility.Convert.GetString(data["AfterCutting"])));
        }

        /// <inheritdoc/>
        protected override bool DoSave()
        {
            if (MyUtility.Check.Empty(this.CurrentData["Seq"]))
            {
                MyUtility.Msg.WarningBox("SEQ can't empty!!");
                this.txtSEQ.Focus();
                return false;
            }

            decimal finalNeedQty = this.CurrentData["FinalNeedQty"] == null ? 0 : MyUtility.Convert.GetDecimal(this.CurrentData["FinalNeedQty"]);

            if (finalNeedQty <= 0)
            {
                MyUtility.Msg.WarningBox("<Final Needed Q'ty> Cannot be less than or equal to 0");
                return false;
            }

            return base.DoSave();
        }

        // Seq
        private void TxtSEQ_Validating(object sender, CancelEventArgs e)
        {
            if (this.EditMode && !MyUtility.Check.Empty(this.txtSEQ.Text) && this.txtSEQ.OldValue != this.txtSEQ.Text)
            {
                if (this.txtSEQ.Text.IndexOf("'") != -1)
                {
                    this.txtSEQ.Text = string.Empty;
                    MyUtility.Msg.WarningBox("Can not enter the  '  character!!");
                    return;
                }

                string[] seqSplit = this.txtSEQ.Text.Split(' ');

                if (seqSplit.Length != 2)
                {
                    MyUtility.Msg.WarningBox("SEQ input format is wrong!!");
                    return;
                }

                string seq1 = seqSplit[0];
                string seq2 = seqSplit[1];
                string sqlCmd = string.Format(
                    @"select f.Seq1,f.Seq2,isnull(psd.ColorID,'') as ColorID,isnull(psd.Refno,'') as Refno,
isnull(psd.SCIRefno,'') as SCIRefno,iif(e.Eta is null, r.ETA, e.Eta) as ETA,isnull(r.ExportId,'') as ExportId,
isnull(r.InvNo,'') as InvNo,isnull(sum(fp.TicketYds),0) as EstInQty,isnull(sum(fp.ActualYds),0) as ActInQty,
dbo.getmtldesc(f.POID,f.Seq1,f.Seq2,2,0) as Description, psd.StockUnit
from FIR f WITH (NOLOCK) 
inner join FIR_Physical fp WITH (NOLOCK) on f.ID = fp.ID
left join Receiving r WITH (NOLOCK) on f.ReceivingID = r.Id
left join Export e WITH (NOLOCK) on r.ExportId = e.ID
left join PO_Supp_Detail psd WITH (NOLOCK) on f.POID = psd.ID and f.Seq1 = psd.SEQ1 and f.Seq2 = psd.SEQ2
where f.POID = '{0}' and f.Seq1 = '{1}' and f.Seq2 = '{2}' and fp.Result = 'F'
group by f.Seq1,f.Seq2,psd.ColorID,psd.Refno,psd.SCIRefno,iif(e.Eta is null, r.ETA, e.Eta),r.ExportId,r.InvNo,dbo.getmtldesc(f.POID,f.Seq1,f.Seq2,2,0), psd.StockUnit",
                    MyUtility.Convert.GetString(this.masterData["POID"]),
                    seq1,
                    seq2);

                if (MyUtility.Check.Seek(sqlCmd, out DataRow firData))
                {
                    this.CurrentData["Seq"] = this.txtSEQ.Text;
                    this.CurrentData["Seq1"] = firData["Seq1"];
                    this.CurrentData["Seq2"] = firData["Seq2"];
                    this.CurrentData["Refno"] = firData["Refno"];
                    this.CurrentData["SCIRefno"] = firData["SCIRefno"];
                    this.CurrentData["ColorID"] = firData["ColorID"];
                    this.CurrentData["ETA"] = firData["ETA"];
                    this.CurrentData["INVNo"] = firData["InvNo"];
                    this.CurrentData["EstInQty"] = firData["EstInQty"];
                    this.CurrentData["ActInQty"] = firData["ActInQty"];
                    this.CurrentData["Description"] = firData["Description"];
                    this.CurrentData["ExportID"] = firData["ExportId"];
                    this.CurrentData["ReplacementUnit"] = firData["StockUnit"];
                }
                else
                {
                    sqlCmd = string.Format(
                        @"select psd.Refno,psd.SCIRefno,psd.seq1,psd.seq2,psd.FabricType,psd.ColorID, 
dbo.getmtldesc(psd.ID,psd.SEQ1,psd.SEQ2,2,0) as Description, psd.StockUnit
from dbo.PO_Supp_Detail psd WITH (NOLOCK) 
inner join dbo.Factory f on psd.FactoryID=f.ID
, dbo.MDivisionPoDetail mpd WITH (NOLOCK) 
where psd.id ='{0}' 
and psd.seq1 = '{1}' 
and psd.seq2 = '{2}' 
and f.MDivisionID = '{3}'
and mpd.POID = psd.ID
and mpd.Seq1 = psd.SEQ1
and mpd.Seq2 = psd.SEQ2
and mpd.InQty > 0",
                        MyUtility.Convert.GetString(this.masterData["POID"]),
                        seq1,
                        seq2,
                        Env.User.Keyword);

                    if (!MyUtility.Check.Seek(sqlCmd, out DataRow poData))
                    {
                        this.CurrentData["Seq"] = string.Empty;
                        this.CurrentData["Seq1"] = string.Empty;
                        this.CurrentData["Seq2"] = string.Empty;
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox(string.Format("< Seq: {0} > have no receive record!!!", this.txtSEQ.Text));
                        return;
                    }

                    if (MyUtility.Convert.GetString(poData["FabricType"]) != "F")
                    {
                        MyUtility.Msg.WarningBox(string.Format("< Seq: {0} > is not fabric material!!!", this.txtSEQ.Text));
                        this.CurrentData["Seq"] = string.Empty;
                        this.CurrentData["Seq1"] = string.Empty;
                        this.CurrentData["Seq2"] = string.Empty;
                        e.Cancel = true;
                        return;
                    }

                    this.CurrentData["Seq"] = this.txtSEQ.Text;
                    this.CurrentData["Seq1"] = poData["Seq1"];
                    this.CurrentData["Seq2"] = poData["Seq2"];
                    this.CurrentData["Refno"] = poData["Refno"];
                    this.CurrentData["SCIRefno"] = poData["SCIRefno"];
                    this.CurrentData["ColorID"] = poData["ColorID"];
                    this.CurrentData["Description"] = poData["Description"];
                    this.CurrentData["ReplacementUnit"] = poData["StockUnit"];

                    sqlCmd = string.Format(
                        @"select distinct r.InvNo,r.ExportId,iif(e.Eta is null, r.ETA,e.Eta) as ETA,
(select isnull(sum(ShipQty),0) from Receiving_Detail WITH (NOLOCK) where PoId = rd.PoId and Seq1 = rd.Seq1 and Seq2 = rd.Seq2) as ShipQty,
(select isnull(sum(ActualQty),0) from Receiving_Detail WITH (NOLOCK) where PoId = rd.PoId and Seq1 = rd.Seq1 and Seq2 = rd.Seq2) as ActQty
from Receiving_Detail rd WITH (NOLOCK) 
left join Receiving r WITH (NOLOCK) on rd.Id = r.Id
left join Export e WITH (NOLOCK) on r.ExportId = e.ID
where rd.PoId = '{0}' and rd.Seq1 = '{1}' and rd.Seq2 = '{2}' and r.Status = 'Confirmed'",
                        MyUtility.Convert.GetString(this.masterData["POID"]),
                        MyUtility.Convert.GetString(this.CurrentData["Seq1"]),
                        MyUtility.Convert.GetString(this.CurrentData["Seq2"]));

                    DualResult result = DBProxy.Current.Select(null, sqlCmd, out DataTable receiveData);
                    if (result)
                    {
                        if (receiveData.Rows.Count <= 0)
                        {
                            this.ClearData();
                        }
                        else
                        {
                            if (receiveData.Rows.Count == 1)
                            {
                                // CurrentData["ETA"] = MyUtility.Check.Empty(ReceiveData.Rows[0]["ETA"])?(DateTime?)null:Convert.ToDateTime(ReceiveData.Rows[0]["ETA"]);
                                this.CurrentData["ETA"] = receiveData.Rows[0]["ETA"];
                                this.CurrentData["INVNo"] = receiveData.Rows[0]["InvNo"];
                                this.CurrentData["EstInQty"] = receiveData.Rows[0]["ShipQty"];
                                this.CurrentData["ActInQty"] = receiveData.Rows[0]["ActQty"];
                                this.CurrentData["ExportID"] = receiveData.Rows[0]["ExportId"];
                            }
                            else
                            {
                                IList<DataRow> selectedReceiveData;
                                Win.Tools.SelectItem item = new Win.Tools.SelectItem(receiveData, "INVNo", "25", string.Empty, "Invoice No");
                                DialogResult returnResult = item.ShowDialog();
                                if (returnResult == DialogResult.Cancel)
                                {
                                    this.ClearData();
                                }
                                else
                                {
                                    selectedReceiveData = item.GetSelecteds();
                                    this.CurrentData["ETA"] = selectedReceiveData[0]["ETA"];
                                    this.CurrentData["INVNo"] = selectedReceiveData[0]["InvNo"];
                                    this.CurrentData["EstInQty"] = selectedReceiveData[0]["ShipQty"];
                                    this.CurrentData["ActInQty"] = selectedReceiveData[0]["ActQty"];
                                    this.CurrentData["ExportID"] = selectedReceiveData[0]["ExportId"];
                                }
                            }
                        }
                    }
                    else
                    {
                        MyUtility.Msg.ErrorBox("Query receive data fail!!\r\n" + result.ToString());
                        this.ClearData();
                    }
                }
            }
        }

        // 清除與Receiving相關的欄位值
        private void ClearData()
        {
            this.CurrentData["ETA"] = DBNull.Value;
            this.CurrentData["INVNo"] = string.Empty;
            this.CurrentData["EstInQty"] = 0;
            this.CurrentData["ActInQty"] = 0;
            this.CurrentData["ExportID"] = string.Empty;
        }

        // Invoice#
        private void TxtInvoice_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            if (this.EditMode)
            {
                string sqlCmd = string.Format(
                    @"select distinct r.InvNo,r.ExportId,iif(e.Eta is null, r.ETA,e.Eta) as ETA,
(select isnull(sum(ShipQty),0) from Receiving_Detail WITH (NOLOCK) where PoId = rd.PoId and Seq1 = rd.Seq1 and Seq2 = rd.Seq2) as ShipQty,
(select isnull(sum(ActualQty),0) from Receiving_Detail WITH (NOLOCK) where PoId = rd.PoId and Seq1 = rd.Seq1 and Seq2 = rd.Seq2) as ActQty
from Receiving_Detail rd WITH (NOLOCK) 
left join Receiving r WITH (NOLOCK) on rd.Id = r.Id
left join Export e WITH (NOLOCK) on r.ExportId = e.ID
where rd.PoId = '{0}' and rd.Seq1 = '{1}' and rd.Seq2 = '{2}' and r.Status = 'Confirmed'",
                    MyUtility.Convert.GetString(this.masterData["POID"]),
                    MyUtility.Convert.GetString(this.CurrentData["Seq1"]),
                    MyUtility.Convert.GetString(this.CurrentData["Seq2"]));

                DualResult result = DBProxy.Current.Select(null, sqlCmd, out DataTable receiveData);
                if (result)
                {
                    IList<DataRow> selectedReceiveData;
                    Win.Tools.SelectItem item = new Win.Tools.SelectItem(receiveData, "INVNo", "25", string.Empty, "Invoice No");
                    DialogResult returnResult = item.ShowDialog();
                    if (returnResult != DialogResult.Cancel)
                    {
                        selectedReceiveData = item.GetSelecteds();
                        this.CurrentData["ETA"] = selectedReceiveData[0]["ETA"];
                        this.CurrentData["INVNo"] = selectedReceiveData[0]["InvNo"];
                        this.CurrentData["EstInQty"] = selectedReceiveData[0]["ShipQty"];
                        this.CurrentData["ActInQty"] = selectedReceiveData[0]["ActQty"];
                        this.CurrentData["ExportID"] = selectedReceiveData[0]["ExportId"];
                    }
                }
                else
                {
                    MyUtility.Msg.ErrorBox("Query receive data fail!! Pls try again.\r\n" + result.ToString());
                }
            }
        }

        // Replacement Request / Yds
        private void NumReplacementRequestYds_Validated(object sender, EventArgs e)
        {
            if (this.EditMode)
            {
                this.CurrentData["AGradeRequest"] = this.numReplacementRequestYds.Value;

                // 計算TotalRequest
                this.CalculateTotalRequest();
            }
        }

        // 100% replacement / Yds
        private void Num100ReplacementYds_Validated(object sender, EventArgs e)
        {
            if (this.EditMode)
            {
                this.CurrentData["BGradeRequest"] = this.num100ReplacementYds.Value;

                // 計算TotalRequest
                this.CalculateTotalRequest();
            }
        }

        // Replacement Request / Yds
        private void NumNarrowReplacementRequestYds_Validated(object sender, EventArgs e)
        {
            if (this.EditMode)
            {
                this.CurrentData["NarrowRequest"] = this.numNarrowReplacementRequestYds.Value;

                // 計算TotalRequest
                this.CalculateTotalRequest();
            }
        }

        // Replacement Request / Yds
        private void NumOtherReplacementRequestYds_Validated(object sender, EventArgs e)
        {
            if (this.EditMode)
            {
                this.CurrentData["OtherRequest"] = this.numOtherReplacementRequestYds.Value;

                // 計算TotalRequest
                this.CalculateTotalRequest();
            }
        }

        // 計算TotalRequest
        private void CalculateTotalRequest()
        {
            this.CurrentData["TotalRequest"] = Convert.ToDecimal(this.CurrentData["AGradeRequest"]) + Convert.ToDecimal(this.CurrentData["BGradeRequest"]) + Convert.ToDecimal(this.CurrentData["NarrowRequest"]) + Convert.ToDecimal(this.CurrentData["OtherRequest"]);
        }

        // Reason
        private void EditReason_PopUp(object sender, Win.UI.EditBoxPopUpEventArgs e)
        {
            if (this.EditMode)
            {
                IList<DataRow> selectedReasonData;
                Win.Tools.SelectItem item = new Win.Tools.SelectItem("select ID,Name from Reason WITH (NOLOCK) where ReasonTypeID = 'Damage Reason' and Junk = 0", "5,50", MyUtility.Convert.GetString(this.CurrentData["Other"]));
                DialogResult returnResult = item.ShowDialog();
                if (returnResult != DialogResult.Cancel)
                {
                    selectedReasonData = item.GetSelecteds();
                    this.CurrentData["Other"] = selectedReasonData[0]["ID"];
                    this.editReason.Text = MyUtility.Convert.GetString(selectedReasonData[0]["Name"]);
                }
            }
        }

        // Replacement Reason
        private void TxtReplacementReason_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            if (this.EditMode)
            {
                IList<DataRow> selectedReasonData;
                Win.Tools.SelectItem item = new Win.Tools.SelectItem("select ID,Name from Reason WITH (NOLOCK) where ReasonTypeID = 'Damage Reason' and Junk = 0", "5,50", MyUtility.Convert.GetString(this.CurrentData["AfterCutting"]));
                DialogResult returnResult = item.ShowDialog();
                if (returnResult != DialogResult.Cancel)
                {
                    selectedReasonData = item.GetSelecteds();
                    this.CurrentData["AfterCutting"] = selectedReasonData[0]["ID"];
                    this.txtReplacementReason.Text = MyUtility.Convert.GetString(selectedReasonData[0]["Name"]);
                }
            }
        }

        private void NumTotalDefectPoints_Validated(object sender, EventArgs e)
        {
            if (this.EditMode)
            {
                this.CurrentData["AGradeDefect"] = this.numTotalDefectPoints.Value;
                if (MyUtility.Check.Empty(this.numTotalDefectPoints.Value))
                {
                    this.CurrentData["AGradeRequest"] = 0;
                }
                else
                {
                    this.CurrentData["AGradeRequest"] = MyUtility.Convert.GetDecimal(this.CurrentData["AGradeDefect"]) * 0.125m;
                }

                // 計算TotalRequest
                this.CalculateTotalRequest();
            }
        }
    }
}
