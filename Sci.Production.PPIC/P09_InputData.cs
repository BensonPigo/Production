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
    public partial class P09_InputData : Sci.Win.Subs.Input6A
    {
        private DataRow masterData;
        public P09_InputData(DataRow MasterData)
        {
            InitializeComponent();
            MyUtility.Tool.SetupCombox(comboBox1, 2, 1, "F,Factory,M,Mill,S,Subcon in Local,T,SCI dep. (purchase/s. mrs/sample room)");
            masterData = MasterData;
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
                if (textBox1.Text.IndexOf("'") != -1)
                {
                    MyUtility.Msg.WarningBox("Can not enter the  '  character!!");
                    textBox1.Text = "";
                    return;
                }

                DataRow firData;
                string sqlCmd = string.Format(@"select a.Seq1,a.Seq2,isnull(psd.ColorID,'') as ColorID,isnull(psd.Refno,'') as Refno,
isnull(psd.SCIRefno,'') as SCIRefno,iif(e.Eta is null, r.ETA, e.Eta) as ETA,isnull(r.ExportId,'') as ExportId,
isnull(r.InvNo,'') as InvNo,isnull(sum(rd.ShipQty),0) as EstInQty, isnull(sum(rd.ActualQty),0) as ActInQty,
dbo.getmtldesc(a.POID,a.Seq1,a.Seq2,2,0) as Description
from AIR a WITH (NOLOCK) 
left join Receiving r WITH (NOLOCK) on a.ReceivingID = r.Id
left join Receiving_Detail rd WITH (NOLOCK) on r.Id = rd.Id and a.Seq1 = rd.SEQ1 and a.Seq2 = rd.SEQ2
left join Export e WITH (NOLOCK) on r.ExportId = e.ID
left join PO_Supp_Detail psd WITH (NOLOCK) on a.POID = psd.ID and a.Seq1 = psd.SEQ1 and a.Seq2 = psd.SEQ2
where a.POID = '{0}' and a.Seq1 = '{1}' and a.Seq2 = '{2}' and a.Result = 'F'
group by a.Seq1,a.Seq2,psd.ColorID,psd.Refno,psd.SCIRefno,iif(e.Eta is null, r.ETA, e.Eta),r.ExportId,r.InvNo,dbo.getmtldesc(a.POID,a.Seq1,a.Seq2,2,0)
", MyUtility.Convert.GetString(masterData["POID"]), textBox1.Text.Length < 3 ? textBox1.Text : textBox1.Text.Substring(0, 3), textBox1.Text.Length < 5 ? textBox1.Text.Length < 4 ? "" : textBox1.Text.ToString().Substring(3,1) : textBox1.Text.ToString().Substring(3, 2));
                if (MyUtility.Check.Seek(sqlCmd, out firData))
                {
                    CurrentData["Seq"] = textBox1.Text;
                    CurrentData["Seq1"] = firData["Seq1"];
                    CurrentData["Seq2"] = firData["Seq2"];
                    CurrentData["Refno"] = firData["Refno"];
                    CurrentData["SCIRefno"] = firData["SCIRefno"];
                    CurrentData["ColorID"] = firData["ColorID"];
                    CurrentData["ETA"] = firData["ETA"];
                    CurrentData["INVNo"] = firData["InvNo"];
                    CurrentData["EstInQty"] = firData["EstInQty"];
                    CurrentData["ActInQty"] = firData["ActInQty"];
                    CurrentData["Description"] = firData["Description"];
                    CurrentData["ExportID"] = firData["ExportId"];
                }
                else
                {
                    DataRow poData;
                    sqlCmd = string.Format(@"select psd.Refno,psd.SCIRefno,psd.seq1,psd.seq2,psd.FabricType,psd.ColorID, 
dbo.getmtldesc(psd.ID,psd.SEQ1,psd.SEQ2,2,0) as Description 
from dbo.PO_Supp_Detail psd WITH (NOLOCK) , dbo.MDivisionPoDetail mpd WITH (NOLOCK) 
where id ='{0}' 
and psd.seq1 = '{1} ' 
and psd.seq2 = '{2}' 
and mpd.MDivisionId = '{3}'
and mpd.POID = psd.ID
and mpd.Seq1 = psd.SEQ1
and mpd.Seq2 = psd.SEQ2
and mpd.InQty > 0", MyUtility.Convert.GetString(masterData["POID"]), textBox1.Text.Length < 3 ? textBox1.Text : textBox1.Text.Substring(0, 3), textBox1.Text.Length < 5 ? textBox1.Text.Length < 4 ? "" : textBox1.Text.ToString().Substring(3, 1) : textBox1.Text.ToString().Substring(3, 2), Sci.Env.User.Keyword);
                    if (!MyUtility.Check.Seek(sqlCmd, out poData))
                    {
                        MyUtility.Msg.WarningBox(string.Format("< Seq: {0} > have no receive record!!!", textBox1.Text));
                        CurrentData["Seq"] = "";
                        CurrentData["Seq1"] = "";
                        CurrentData["Seq2"] = "";
                        e.Cancel = true;
                        return;
                    }
                    if (MyUtility.Convert.GetString(poData["FabricType"]) != "A")
                    {
                        MyUtility.Msg.WarningBox(string.Format("< Seq: {0} > is not accessory material!!!", textBox1.Text));
                        CurrentData["Seq"] = "";
                        CurrentData["Seq1"] = "";
                        CurrentData["Seq2"] = "";
                        e.Cancel = true;
                        return;
                    }

                    CurrentData["Seq"] = textBox1.Text;
                    CurrentData["Seq1"] = poData["Seq1"];
                    CurrentData["Seq2"] = poData["Seq2"];
                    CurrentData["Refno"] = poData["Refno"];
                    CurrentData["SCIRefno"] = poData["SCIRefno"];
                    CurrentData["ColorID"] = poData["ColorID"];
                    CurrentData["Description"] = poData["Description"];

                    sqlCmd = string.Format(@"select distinct r.InvNo,r.ExportId,iif(e.Eta is null, r.ETA,e.Eta) as ETA,
(select isnull(sum(ShipQty),0) from Receiving_Detail WITH (NOLOCK) where PoId = rd.PoId and Seq1 = rd.Seq1 and Seq2 = rd.Seq2) as ShipQty,
(select isnull(sum(ActualQty),0) from Receiving_Detail WITH (NOLOCK) where PoId = rd.PoId and Seq1 = rd.Seq1 and Seq2 = rd.Seq2) as ActQty
from Receiving_Detail rd WITH (NOLOCK) 
left join Receiving r WITH (NOLOCK) on rd.Id = r.Id
left join Export e WITH (NOLOCK) on r.ExportId = e.ID
where rd.PoId = '{0}' and rd.Seq1 = '{1}' and rd.Seq2 = '{2}' and r.Status = 'Confirmed'", MyUtility.Convert.GetString(masterData["POID"]), MyUtility.Convert.GetString(CurrentData["Seq1"]), MyUtility.Convert.GetString(CurrentData["Seq2"]));
                    DataTable ReceiveData;
                    DualResult result = DBProxy.Current.Select(null, sqlCmd, out ReceiveData);
                    if (result)
                    {
                        if (ReceiveData.Rows.Count <= 0)
                        {
                            ClearData();
                        }
                        else
                        {
                            if (ReceiveData.Rows.Count == 1)
                            {
                                CurrentData["ETA"] = ReceiveData.Rows[0]["ETA"];
                                CurrentData["INVNo"] = ReceiveData.Rows[0]["InvNo"];
                                CurrentData["EstInQty"] = ReceiveData.Rows[0]["ShipQty"];
                                CurrentData["ActInQty"] = ReceiveData.Rows[0]["ActQty"];
                                CurrentData["ExportID"] = ReceiveData.Rows[0]["ExportId"];
                            }
                            else
                            {
                                IList<DataRow> selectedReceiveData;
                                Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(ReceiveData, "INVNo", "25", "", "Invoice No");
                                DialogResult returnResult = item.ShowDialog();
                                if (returnResult == DialogResult.Cancel)
                                {
                                    ClearData();
                                }
                                else
                                {
                                    selectedReceiveData = item.GetSelecteds();
                                    CurrentData["ETA"] = selectedReceiveData[0]["ETA"];
                                    CurrentData["INVNo"] = selectedReceiveData[0]["InvNo"];
                                    CurrentData["EstInQty"] = selectedReceiveData[0]["ShipQty"];
                                    CurrentData["ActInQty"] = selectedReceiveData[0]["ActQty"];
                                    CurrentData["ExportID"] = selectedReceiveData[0]["ExportId"];
                                }
                            }
                        }
                    }
                    else
                    {
                        MyUtility.Msg.ErrorBox("Query receive data fail!!\r\n" + result.ToString());
                        ClearData();
                    }
                }
            }
        }
        
        //清除與Receiving相關的欄位值
        private void ClearData()
        {
            CurrentData["ETA"] = DBNull.Value;
            CurrentData["INVNo"] = "";
            CurrentData["EstInQty"] = 0;
            CurrentData["ActInQty"] = 0;
            CurrentData["ExportID"] = "";
        }

        //Invoice#
        private void textBox2_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            if (EditMode)
            {
                string sqlCmd = string.Format(@"select distinct r.InvNo,r.ExportId,iif(e.Eta is null, r.ETA,e.Eta) as ETA,
(select isnull(sum(ShipQty),0) from Receiving_Detail WITH (NOLOCK) where PoId = rd.PoId and Seq1 = rd.Seq1 and Seq2 = rd.Seq2) as ShipQty,
(select isnull(sum(ActualQty),0) from Receiving_Detail WITH (NOLOCK) where PoId = rd.PoId and Seq1 = rd.Seq1 and Seq2 = rd.Seq2) as ActQty
from Receiving_Detail rd WITH (NOLOCK) 
left join Receiving r WITH (NOLOCK) on rd.Id = r.Id
left join Export e WITH (NOLOCK) on r.ExportId = e.ID
where rd.PoId = '{0}' and rd.Seq1 = '{1}' and rd.Seq2 = '{2}' and r.Status = 'Confirmed'", MyUtility.Convert.GetString(masterData["POID"]), MyUtility.Convert.GetString(CurrentData["Seq1"]), MyUtility.Convert.GetString(CurrentData["Seq2"]));
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
                        CurrentData["ETA"] = selectedReceiveData[0]["ETA"];
                        CurrentData["INVNo"] = selectedReceiveData[0]["InvNo"];
                        CurrentData["EstInQty"] = selectedReceiveData[0]["ShipQty"];
                        CurrentData["ActInQty"] = selectedReceiveData[0]["ActQty"];
                        CurrentData["ExportID"] = selectedReceiveData[0]["ExportId"];
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
