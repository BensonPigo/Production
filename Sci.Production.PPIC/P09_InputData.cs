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
    /// P09_InputData
    /// </summary>
    public partial class P09_InputData : Win.Subs.Input6A
    {
        private DataRow masterData;

        /// <summary>
        /// P09_InputData
        /// </summary>
        /// <param name="masterData">DataRow MasterData</param>
        public P09_InputData(DataRow masterData)
        {
            this.InitializeComponent();
            this.masterData = masterData;
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

                DataRow firData;
                string sqlCmd = $@"
select a.Seq1,a.Seq2,isnull(psdsC.SpecValue,'') as ColorID,isnull(psd.Refno,'') as Refno,
isnull(psd.SCIRefno,'') as SCIRefno,iif(e.Eta is null, r.ETA, e.Eta) as ETA,isnull(r.ExportId,'') as ExportId,
isnull(r.InvNo,'') as InvNo,isnull(sum(rd.ShipQty),0) as EstInQty, isnull(sum(rd.ActualQty),0) as ActInQty,
dbo.getmtldesc(a.POID,a.Seq1,a.Seq2,2,0) as DescriptionDetail, psd.StockUnit
from AIR a WITH (NOLOCK) 
left join Receiving r WITH (NOLOCK) on a.ReceivingID = r.Id
left join Receiving_Detail rd WITH (NOLOCK) on r.Id = rd.Id and a.Seq1 = rd.SEQ1 and a.Seq2 = rd.SEQ2
left join Export e WITH (NOLOCK) on r.ExportId = e.ID
left join PO_Supp_Detail psd WITH (NOLOCK) on a.POID = psd.ID and a.Seq1 = psd.SEQ1 and a.Seq2 = psd.SEQ2
left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
where a.POID = '{this.masterData["POID"]}' and a.Seq1 = '{seq1}' and a.Seq2 = '{seq2}' and a.Result = 'F'
group by a.Seq1,a.Seq2,psdsC.SpecValue,psd.Refno,psd.SCIRefno,iif(e.Eta is null, r.ETA, e.Eta),r.ExportId,r.InvNo,dbo.getmtldesc(a.POID,a.Seq1,a.Seq2,2,0), psd.StockUnit
";

                if (MyUtility.Check.Seek(sqlCmd, out firData))
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
                    this.CurrentData["DescriptionDetail"] = firData["DescriptionDetail"];
                    this.CurrentData["ExportID"] = firData["ExportId"];
                    this.CurrentData["ReplacementUnit"] = firData["StockUnit"];
                }
                else
                {
                    DataRow poData;
                    sqlCmd = $@"
select psd.Refno,psd.SCIRefno,psd.seq1,psd.seq2,psd.FabricType,ColorID = isnull(psdsC.SpecValue ,''), 
dbo.getmtldesc(psd.ID,psd.SEQ1,psd.SEQ2,2,0) as DescriptionDetail, psd.StockUnit
from dbo.PO_Supp_Detail psd WITH (NOLOCK) 
inner join dbo.Factory f on psd.FactoryID=f.ID
inner join dbo.MDivisionPoDetail mpd WITH (NOLOCK) on mpd.POID = psd.ID and mpd.Seq1 = psd.SEQ1 and mpd.Seq2 = psd.SEQ2
left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
where psd.id ='{this.masterData["POID"]}' 
and psd.seq1 = '{seq1} ' 
and psd.seq2 = '{seq2}' 
and f.MDivisionId = '{Env.User.Keyword}'
and mpd.InQty > 0
";

                    if (!MyUtility.Check.Seek(sqlCmd, out poData))
                    {
                        this.CurrentData["Seq"] = string.Empty;
                        this.CurrentData["Seq1"] = string.Empty;
                        this.CurrentData["Seq2"] = string.Empty;
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox(string.Format("< Seq: {0} > have no receive record!!!", this.txtSEQ.Text));
                        return;
                    }

                    if (MyUtility.Convert.GetString(poData["FabricType"]) != "A")
                    {
                        MyUtility.Msg.WarningBox(string.Format("< Seq: {0} > is not accessory material!!!", this.txtSEQ.Text));
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
                    this.CurrentData["DescriptionDetail"] = poData["DescriptionDetail"];
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

                    DataTable receiveData;
                    DualResult result = DBProxy.Current.Select(null, sqlCmd, out receiveData);
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

                DataTable receiveData;
                DualResult result = DBProxy.Current.Select(null, sqlCmd, out receiveData);
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
    }
}
