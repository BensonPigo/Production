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
    public partial class P08_InputData : Sci.Win.Subs.Input6A
    {
        private DataRow masterData;
        public P08_InputData(DataRow MasterData)
        {
            InitializeComponent();
            MyUtility.Tool.SetupCombox(comboBox1, 2, 1, "F,Factory,M,Mill,S,Subcon in Local,T,SCI dep. (purchase/s. mrs/sample room)");
            masterData = MasterData;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            label10.Text = "Done 100% Fabric\r\nInspection\r\nReplacement\r\nRequest";
            label11.Text = "Defect  Point Under\r\n50 Per 100y in a Roll\r\n(A grade)";
            label12.Text = "Defect Point Over \r\n50 Per 100y in a Roll\r\n(B grade)";
            label16.Text = "No. of Rolls over 50\r\npoints per 100y";
            label17.Text = "Standard with / Rcvd\r\nwidth / No. of rolls with\r\nnarrow width";
            label24.Text = "After Cutting\r\nReplacement\r\nRequest";
        }

        protected override void OnAttached(DataRow data)
        {
            base.OnAttached(data);
            editBox2.Text = MyUtility.Check.Empty(data["Other"]) ? data["OtherReason"].ToString() : MyUtility.GetValue.Lookup(string.Format("select Name from Reason where ReasonTypeID = 'Damage Reason' and ID = '{0}'", data["Other"].ToString()));
            textBox2.Text = MyUtility.Check.Empty(data["AfterCutting"]) ? data["AfterCuttingReason"].ToString() : MyUtility.GetValue.Lookup(string.Format("select Name from Reason where ReasonTypeID = 'Damage Reason' and ID = '{0}'", data["AfterCutting"].ToString()));
        }

        protected override bool DoSave()
        {
            if (MyUtility.Check.Empty(CurrentData["Seq"]))
            {
                MyUtility.Msg.WarningBox("SEQ can't empty!!");
                textBox1.Focus();
                return false;
            }
            return base.DoSave();
        }

        //Seq
        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            if (EditMode && !MyUtility.Check.Empty(textBox1.Text) && textBox1.OldValue != textBox1.Text)
            {
                DataRow firData;
                string sqlCmd = string.Format(@"select f.Seq1,f.Seq2,isnull(psd.ColorID,'') as ColorID,isnull(psd.Refno,'') as Refno,
isnull(psd.SCIRefno,'') as SCIRefno,iif(e.Eta is null, r.ETA, e.Eta) as ETA,isnull(r.ExportId,'') as ExportId,
isnull(r.InvNo,'') as InvNo,isnull(sum(fp.TicketYds),0) as EstInQty,isnull(sum(fp.ActualYds),0) as ActInQty,
dbo.getmtldesc(f.POID,f.Seq1,f.Seq2,2,0) as Description
from FIR f
inner join FIR_Physical fp on f.ID = fp.ID
left join Receiving r on f.ReceivingID = r.Id
left join Export e on r.ExportId = e.ID
left join PO_Supp_Detail psd on f.POID = psd.ID and f.Seq1 = psd.SEQ1 and f.Seq2 = psd.SEQ2
where f.POID = '{0}' and f.Seq1 = '{1}' and f.Seq2 = '{2}' and fp.Result = 'F'
group by f.Seq1,f.Seq2,psd.ColorID,psd.Refno,psd.SCIRefno,iif(e.Eta is null, r.ETA, e.Eta),r.ExportId,r.InvNo,dbo.getmtldesc(f.POID,f.Seq1,f.Seq2,2,0)
", masterData["POID"].ToString(), textBox1.Text.Substring(0, 3), textBox1.Text.ToString().Substring(2, 2));
                if (MyUtility.Check.Seek(sqlCmd, out firData))
                {
                    CurrentData["Seq"] = textBox1.Text;
                    CurrentData["Seq1"] = firData["Seq1"].ToString();
                    CurrentData["Seq2"] = firData["Seq2"].ToString();
                    CurrentData["Refno"] = firData["Refno"].ToString();
                    CurrentData["SCIRefno"] = firData["SCIRefno"].ToString();
                    CurrentData["ColorID"] = firData["ColorID"].ToString();
                    CurrentData["ETA"] = firData["ETA"].ToString();
                    CurrentData["INVNo"] = firData["InvNo"].ToString();
                    CurrentData["EstInQty"] = firData["EstInQty"].ToString();
                    CurrentData["ActInQty"] = firData["ActInQty"].ToString();
                    CurrentData["Description"] = firData["Description"].ToString();
                    CurrentData["ExportID"] = firData["ExportId"].ToString();
                }
                else
                {
                    DataRow poData;
                    sqlCmd = string.Format(@"select Refno,SCIRefno,seq1,seq2,FabricType,ColorID, 
dbo.getmtldesc(id,seq1,seq2,2,0) as Description 
from dbo.PO_Supp_Detail
where id ='{0}' and seq1 = '{1}' and seq2 = '{2}' and InQty > 0", masterData["POID"].ToString(), textBox1.Text.Substring(0, 3), textBox1.Text.ToString().Substring(2, 2));
                    if (!MyUtility.Check.Seek(sqlCmd, out poData))
                    {
                        MyUtility.Msg.WarningBox(string.Format("< Seq: {0} > have no receive record!!!", textBox1.Text));
                        CurrentData["Seq"] = "";
                        CurrentData["Seq1"] = "";
                        CurrentData["Seq2"] = "";
                        e.Cancel = true;
                        return;
                    }
                    if (poData["FabricType"].ToString() != "F")
                    {
                        MyUtility.Msg.WarningBox(string.Format("< Seq: {0} > is not fabric material!!!", textBox1.Text));
                        CurrentData["Seq"] = "";
                        CurrentData["Seq1"] = "";
                        CurrentData["Seq2"] = "";
                        e.Cancel = true;
                        return;
                    }

                    CurrentData["Seq"] = textBox1.Text;
                    CurrentData["Seq1"] = poData["Seq1"].ToString();
                    CurrentData["Seq2"] = poData["Seq2"].ToString();
                    CurrentData["Refno"] = poData["Refno"].ToString();
                    CurrentData["SCIRefno"] = poData["SCIRefno"].ToString();
                    CurrentData["ColorID"] = poData["ColorID"].ToString();
                    CurrentData["Description"] = poData["Description"].ToString();

                    sqlCmd = string.Format(@"select distinct r.InvNo,r.ExportId,iif(e.Eta is null, r.ETA,e.Eta) as ETA,
(select isnull(sum(ShipQty),0) from Receiving_Detail where PoId = rd.PoId and Seq1 = rd.Seq1 and Seq2 = rd.Seq2) as ShipQty,
(select isnull(sum(ActualQty),0) from Receiving_Detail where PoId = rd.PoId and Seq1 = rd.Seq1 and Seq2 = rd.Seq2) as ActQty
from Receiving_Detail rd
left join Receiving r on rd.Id = r.Id
left join Export e on r.ExportId = e.ID
where rd.PoId = '{0}' and rd.Seq1 = '{1}' and rd.Seq2 = '{2}' and r.Status = 'Confirmed'", masterData["POID"].ToString(), CurrentData["Seq1"].ToString(), CurrentData["Seq2"].ToString());
                    DataTable ReceiveData;
                    DualResult result = DBProxy.Current.Select(null, sqlCmd, out ReceiveData);
                    if (result)
                    {
                        if (ReceiveData.Rows.Count <= 0)
                        {
                            CurrentData["ETA"] = null;
                            CurrentData["INVNo"] = "";
                            CurrentData["EstInQty"] = 0;
                            CurrentData["ActInQty"] = 0;
                            CurrentData["ExportID"] = "";
                        }
                        else
                        {
                            if (ReceiveData.Rows.Count == 1)
                            {
                                CurrentData["ETA"] = MyUtility.Check.Empty(ReceiveData.Rows[0]["ETA"])?(DateTime?)null:Convert.ToDateTime(ReceiveData.Rows[0]["ETA"]);
                                CurrentData["INVNo"] = ReceiveData.Rows[0]["InvNo"].ToString();
                                CurrentData["EstInQty"] = ReceiveData.Rows[0]["ShipQty"].ToString();
                                CurrentData["ActInQty"] = ReceiveData.Rows[0]["ActQty"].ToString();
                                CurrentData["ExportID"] = ReceiveData.Rows[0]["ExportId"].ToString();
                            }
                            else
                            {
                                IList<DataRow> selectedReceiveData;
                                Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(ReceiveData, "INVNo","25", "", "Invoice No");
                                DialogResult returnResult = item.ShowDialog();
                                if (returnResult == DialogResult.Cancel)
                                {
                                    CurrentData["ETA"] = null;
                                    CurrentData["INVNo"] = "";
                                    CurrentData["EstInQty"] = 0;
                                    CurrentData["ActInQty"] = 0;
                                    CurrentData["ExportID"] = "";
                                }
                                else
                                {
                                    selectedReceiveData = item.GetSelecteds();
                                    CurrentData["ETA"] = MyUtility.Check.Empty(selectedReceiveData[0]["ETA"]) ? (DateTime?)null : Convert.ToDateTime(selectedReceiveData[0]["ETA"]);
                                    CurrentData["INVNo"] = selectedReceiveData[0]["InvNo"].ToString();
                                    CurrentData["EstInQty"] = selectedReceiveData[0]["ShipQty"].ToString();
                                    CurrentData["ActInQty"] = selectedReceiveData[0]["ActQty"].ToString();
                                    CurrentData["ExportID"] = selectedReceiveData[0]["ExportId"].ToString();
                                }
                            }
                        }
                    }
                    else
                    {
                        MyUtility.Msg.ErrorBox("Query receive data fail!!\r\n" + result.ToString());
                        CurrentData["ETA"] = null;
                        CurrentData["INVNo"] = "";
                        CurrentData["EstInQty"] = 0;
                        CurrentData["ActInQty"] = 0;
                        CurrentData["ExportID"] = "";
                    }
                }
            }
        }

        //Invoice#
        private void textBox6_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            if (EditMode)
            {
                string sqlCmd = string.Format(@"select distinct r.InvNo,r.ExportId,iif(e.Eta is null, r.ETA,e.Eta) as ETA,
(select isnull(sum(ShipQty),0) from Receiving_Detail where PoId = rd.PoId and Seq1 = rd.Seq1 and Seq2 = rd.Seq2) as ShipQty,
(select isnull(sum(ActualQty),0) from Receiving_Detail where PoId = rd.PoId and Seq1 = rd.Seq1 and Seq2 = rd.Seq2) as ActQty
from Receiving_Detail rd
left join Receiving r on rd.Id = r.Id
left join Export e on r.ExportId = e.ID
where rd.PoId = '{0}' and rd.Seq1 = '{1}' and rd.Seq2 = '{2}' and r.Status = 'Confirmed'", masterData["POID"].ToString(), CurrentData["Seq1"].ToString(), CurrentData["Seq2"].ToString());
                DataTable ReceiveData;
                DualResult result = DBProxy.Current.Select(null, sqlCmd, out ReceiveData);
                if (result)
                {
                    IList<DataRow> selectedReceiveData;
                    Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(ReceiveData, "INVNo", "25", "", "Invoice No");
                    DialogResult returnResult = item.ShowDialog();
                    if (returnResult != DialogResult.Cancel)
                    {
                        selectedReceiveData = item.GetSelecteds();
                        CurrentData["ETA"] = MyUtility.Check.Empty(selectedReceiveData[0]["ETA"]) ? (DateTime?)null : Convert.ToDateTime(selectedReceiveData[0]["ETA"]);
                        CurrentData["INVNo"] = selectedReceiveData[0]["InvNo"].ToString();
                        CurrentData["EstInQty"] = selectedReceiveData[0]["ShipQty"].ToString();
                        CurrentData["ActInQty"] = selectedReceiveData[0]["ActQty"].ToString();
                        CurrentData["ExportID"] = selectedReceiveData[0]["ExportId"].ToString();
                    }
                }
                else
                {
                    MyUtility.Msg.ErrorBox("Query receive data fail!! Pls try again.\r\n" + result.ToString());
                }
            }
        }

        //Total defect points
        private void textBox7_Validated(object sender, EventArgs e)
        {
            if (EditMode && textBox7.OldValue != textBox7.Text)
            {
                CurrentData["AGradeDefect"] = textBox7.Text;
                if (MyUtility.Check.Empty(textBox7.Text))
                {
                    CurrentData["AGradeRequest"] = 0;
                }
                else
                {
                    CurrentData["AGradeRequest"] = Convert.ToDecimal(CurrentData["AGradeDefect"])*0.125m;
                }
                //計算TotalRequest
                CalculateTotalRequest();
            }
        }

        //Replacement Request / Yds
        private void numericBox6_Validated(object sender, EventArgs e)
        {
            if (EditMode && numericBox6.OldValue != numericBox6.Value)
            {
                CurrentData["AGradeRequest"] = numericBox6.Value;
                //計算TotalRequest
                CalculateTotalRequest();
            }
        }

        //100% replacement / Yds
        private void numericBox7_Validated(object sender, EventArgs e)
        {
            if (EditMode && numericBox7.OldValue != numericBox7.Value)
            {
                CurrentData["BGradeRequest"] = numericBox7.Value;
                //計算TotalRequest
                CalculateTotalRequest();
            }
        }

        //Replacement Request / Yds
        private void numericBox8_Validated(object sender, EventArgs e)
        {
            if (EditMode && numericBox8.OldValue != numericBox8.Value)
            {
                CurrentData["NarrowRequest"] = numericBox8.Value;
                //計算TotalRequest
                CalculateTotalRequest();
            }
        }

        //Replacement Request / Yds
        private void numericBox9_Validated(object sender, EventArgs e)
        {
            if (EditMode && numericBox9.OldValue != numericBox9.Value)
            {
                CurrentData["OtherRequest"] = numericBox9.Value;
                //計算TotalRequest
                CalculateTotalRequest();
            }
        }

        //計算TotalRequest
        private void CalculateTotalRequest()
        {
            CurrentData["TotalRequest"] = Convert.ToDecimal(CurrentData["AGradeRequest"]) + Convert.ToDecimal(CurrentData["BGradeRequest"]) + Convert.ToDecimal(CurrentData["NarrowRequest"]) + Convert.ToDecimal(CurrentData["OtherRequest"]);
        }

        //Reason
        private void editBox2_PopUp(object sender, Win.UI.EditBoxPopUpEventArgs e)
        {
            if (EditMode)
            {
                IList<DataRow> selectedReasonData;
                Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem("select ID,Name from Reason where ReasonTypeID = 'Damage Reason' and Junk = 0", "5,50", CurrentData["Other"].ToString());
                DialogResult returnResult = item.ShowDialog();
                if (returnResult != DialogResult.Cancel)
                {
                    selectedReasonData = item.GetSelecteds();
                    CurrentData["Other"] = selectedReasonData[0]["ID"].ToString();
                    editBox2.Text = selectedReasonData[0]["Name"].ToString();
                }
            }
        }

        //Replacement Reason
        private void textBox2_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            if (EditMode)
            {
                IList<DataRow> selectedReasonData;
                Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem("select ID,Name from Reason where ReasonTypeID = 'Damage Reason' and Junk = 0", "5,50", CurrentData["AfterCutting"].ToString());
                DialogResult returnResult = item.ShowDialog();
                if (returnResult != DialogResult.Cancel)
                {
                    selectedReasonData = item.GetSelecteds();
                    CurrentData["AfterCutting"] = selectedReasonData[0]["ID"].ToString();
                    textBox2.Text = selectedReasonData[0]["Name"].ToString();
                }
            }
        }
    }
}
