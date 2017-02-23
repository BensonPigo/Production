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
using System.Transactions;
using Sci.Production.PublicPrg;

namespace Sci.Production.Shipping
{
    public partial class P08_ShareExpense : Sci.Win.Subs.Base
    {
        DataRow apData;
        DataTable SEData, SEGroupData;
        public P08_ShareExpense(DataRow APData)
        {
            InitializeComponent();
            apData = APData;
            displayBox1.Value = MyUtility.Convert.GetString(apData["CurrencyID"]);
            numericBox1.DecimalPlaces = MyUtility.Convert.GetInt(MyUtility.GetValue.Lookup("Exact", MyUtility.Convert.GetString(apData["CurrencyID"]), "Currency", "ID"));
            numericBox1.Value = MyUtility.Convert.GetDecimal(apData["Amount"]);
            ControlButton();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            Ict.Win.DataGridViewGeneratorTextColumnSettings BLNo = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
            Ict.Win.DataGridViewGeneratorTextColumnSettings WKNO = new Ict.Win.DataGridViewGeneratorTextColumnSettings();

            #region BLNo
            BLNo.CellValidating += (s, e) =>
            {
                if (!this.EditMode) return;//非編輯模式 
                if (e.RowIndex == -1) return; //沒東西 return
                if (MyUtility.Check.Empty(e.FormattedValue)) return; // 沒資料 return
                string cmd_type = string.Format(@"select * from ShippingAP where id='{0}'", apData["ID"]);
                DataRow dr ;
                DataRow drGrid = this.grid1.GetDataRow<DataRow>(e.RowIndex);

                DataTable dts = (DataTable)listControlBindingSource1.DataSource;
                if (drGrid["BLNO"].ToString().ToUpper() == e.FormattedValue.ToString().ToUpper()) return;
                if (MyUtility.Check.Seek(cmd_type,out dr))
                {
                    DataRow dr1;
                    //刪除異動資料
                    if (drGrid["BLNO"].ToString().ToUpper() != e.FormattedValue.ToString().ToUpper() && !MyUtility.Check.Empty(drGrid["BLNO"].ToString()))
                    {
                        string blno = drGrid["BLNO"].ToString().ToUpper();
                        int t = dts.Rows.Count;
                        for (int i = t-1; i >= 0; i--)
                        {
                            if (dts.Rows[i].RowState!=DataRowState.Deleted)
                            {
                                if (dts.Rows[i]["BLNO"].ToString().ToUpper() == blno)
                                {   //刪除
                                    dts.Rows[i].Delete();
                                }
                            }                            
                        }                                               
                        e.Cancel=true;// 不進入RowIndex判斷                            
                    }                    
                    // 檢查規則
                    if (dr["type"].ToString().ToUpper()=="IMPORT")
                    {
                        string chkImp = string.Format(@"with ExportData 
as 
(select 0 as Selected,ID as WKNo,Blno,ShipModeID,WeightKg as GW, Cbm, '' as InvNo, '' as ShippingAPID, 
 '' as Type, '' as CurrencyID, 0 as Amount, '' as ShareBase, 0 as FtyWK
 from Export WHERE BLNO='{0}'), 
FtyExportData 
as 
(select 0 as Selected,ID as WKNo,Blno,ShipModeID,WeightKg as GW, Cbm, '' as InvNo, '' as ShippingAPID, 
 '' as Type, '' as CurrencyID, 0 as Amount, '' as ShareBase, 1 as FtyWK
 from FtyExport
WHERE BLNO='{0}' AND TYPE IN (1,2,4)) 
select * from ExportData 
union all 
select * from FtyExportData ", e.FormattedValue.ToString());
                        if (!MyUtility.Check.Seek(chkImp,out dr1))
                        {
                            MyUtility.Msg.InfoBox("<BLNo:>" +e.FormattedValue.ToString()+" Not Found!!");                      
                            drGrid.Delete();
                            e.Cancel = true;
                            return;
                        }
                        else
                        {
                            //判斷修改的資料是否正確
                            //正確就將所有B/L No所有WK#都帶出來
                            DataTable dtAll;
                            string cmd_allWKNo = string.Format(@"with ExportData 
as 
(select a.WKNo,a.Blno,a.ShipModeID,a.GW, a.Cbm, '' as InvNo, '' as ShippingAPID, 
 '' as Type, '' as CurrencyID, 0 as Amount, '' as ShareBase, 0 as FtyWK from ShareExpense a
inner join Export b on a.BLNo=b.Blno and a.WKNo=b.ID and a.GW=b.WeightKg
and a.cbm=b.Cbm and a.ShipModeID=b.ShipModeID and a.FtyWK=0 
WHERE B.Blno='{0}'), 
FtyExportData 
as 
(select a.WKNo,a.Blno,a.ShipModeID,a.GW, a.Cbm, '' as InvNo, '' as ShippingAPID, 
 '' as Type, '' as CurrencyID, 0 as Amount, '' as ShareBase, 1 as FtyWK from ShareExpense a
inner join FtyExport b on a.BLNo=b.Blno and a.WKNo=b.ID and a.GW=b.WeightKg
and a.cbm=b.Cbm and a.ShipModeID=b.ShipModeID and a.FtyWK=1
WHERE B.Blno='{0}') 
select distinct * from ExportData 
union all 
select distinct * from FtyExportData ", e.FormattedValue.ToString());
                            DBProxy.Current.Select(null, cmd_allWKNo, out dtAll);
                            if (MyUtility.Check.Empty(dtAll)) return;
                            for (int i = 0; i < dtAll.Rows.Count; i++)
                            {
                                DataRow NewRow = ((DataTable)listControlBindingSource1.DataSource).NewRow();

                                NewRow["Blno"] = dtAll.Rows[i]["Blno"];
                                NewRow["Wkno"] = dtAll.Rows[i]["Wkno"];
                                NewRow["ShipModeID"] = dtAll.Rows[i]["ShipModeID"];
                                NewRow["GW"] = dtAll.Rows[i]["GW"];
                                NewRow["CBM"] = dtAll.Rows[i]["CBM"];
                                NewRow["Amount"] = dtAll.Rows[i]["Amount"];
                                ((DataTable)listControlBindingSource1.DataSource).Rows.Add(NewRow);

                            }
                        }
                    }
                    else
                    {
                        string chkExp = string.Format(@"
select 0 as Selected,ID as WKNo,Blno,ShipModeID,WeightKg as GW, Cbm, '' as InvNo, '' as ShippingAPID, 
 '' as Type, '' as CurrencyID, 0 as Amount, '' as ShareBase, 1 as FtyWK
 from FtyExport
WHERE BLNO='{0}' AND TYPE IN (3) 
 ", e.FormattedValue.ToString());
                        if (!MyUtility.Check.Seek(chkExp, out dr1))
                        {
                            MyUtility.Msg.InfoBox("<BLNo:>" + e.FormattedValue.ToString() + " Not Found!!");
                            drGrid.Delete();
                            e.Cancel = true;
                            return;
                        }
                        else
                        {
                            //判斷修改的資料是否正確
                            //正確就將所有B/L No所有WK#都帶出來
                            DataTable dtAll;
                            string cmd_allWKNo = string.Format(@"select DISTINCT A.INVNO as WKNo,A.Blno,A.ShipModeID,A.GW, B.Cbm, '' as InvNo, '' as ShippingAPID, 
 '' as Type, '' as CurrencyID, 0 as Amount, '' as ShareBase, 1 as FtyWK 
from ShareExpense a
inner join FtyExport b on a.BLNo=b.Blno and a.WKNo=b.ID and a.GW=b.WeightKg
and a.cbm=b.Cbm and a.ShipModeID=b.ShipModeID and a.FtyWK=1
WHERE B.Blno='{0}'", e.FormattedValue.ToString());
                            DBProxy.Current.Select(null, cmd_allWKNo, out dtAll);
                            if (MyUtility.Check.Empty(dtAll)) return;

                            for (int i = 0; i < dtAll.Rows.Count; i++)
                            {
                                DataRow NewRow = ((DataTable)listControlBindingSource1.DataSource).NewRow();

                                NewRow["Blno"] = dtAll.Rows[i]["Blno"];
                                NewRow["Wkno"] = dtAll.Rows[i]["Wkno"];
                                NewRow["ShipModeID"] = dtAll.Rows[i]["ShipModeID"];
                                NewRow["GW"] = dtAll.Rows[i]["GW"];
                                NewRow["CBM"] = dtAll.Rows[i]["CBM"];
                                NewRow["Amount"] = dtAll.Rows[i]["Amount"];
                                ((DataTable)listControlBindingSource1.DataSource).Rows.Add(NewRow);
                            }
                        }
                    }
                    //delete empty rows
                    int t1 = dts.Rows.Count;
                    for (int i = t1 - 1; i >= 0; i--)
                    {
                        if (dts.Rows[i].RowState != DataRowState.Deleted)
                        {
                            if (MyUtility.Check.Empty(dts.Rows[i]["Blno"].ToString()))
                            {
                                //刪除
                                dts.Rows[i].Delete();
                            }
                        }
                    }
                    e.Cancel = true;// 不進入RowIndex判斷    
                }
            };
#endregion
            #region WKNO
            WKNO.CellValidating += (s, e) =>
            {
                if (!this.EditMode) return;//非編輯模式 
                if (e.RowIndex == -1) return; //沒東西 return
                if (MyUtility.Check.Empty(e.FormattedValue)) return; // 沒資料 return
                string cmd_type = string.Format(@"select * from ShippingAP where id='{0}'", apData["ID"]);
                DataRow dr;
                DataRow drGrid = this.grid1.GetDataRow<DataRow>(e.RowIndex);

                DataTable dts = (DataTable)listControlBindingSource1.DataSource;
                if (drGrid["WKNO"].ToString().ToUpper() == e.FormattedValue.ToString().ToUpper()) return;
                if (MyUtility.Check.Seek(cmd_type, out dr))
                {
                    DataRow dr1;
                    // 檢查規則
                    if (dr["type"].ToString().ToUpper() == "IMPORT")
                    {
                        string chkImp = string.Format(@"with ExportData 
as 
(select id
 from Export WHERE id='{0}'), 
FtyExportData 
as 
(select id
 from FtyExport
WHERE id='{0}' AND TYPE IN (1,2,4)) 
select * from ExportData 
union all 
select * from FtyExportData ", e.FormattedValue.ToString());
                        if (!MyUtility.Check.Seek(chkImp, out dr1))
                        {
                            MyUtility.Msg.InfoBox("<WKNo:>" + e.FormattedValue.ToString() + " Not Found!!");
                            drGrid.Delete();
                            e.Cancel = true;
                            return;
                        }
                        else
                        {
                            //判斷修改的資料是否正確
                            //正確就將所有B/L No所有WK#都帶出來
                            DataTable dtAll;
                            #region 文件 
                            string cmd_allWKNo = string.Format(@"with ExportData 
as 
(select a.WKNo,a.Blno,a.ShipModeID,a.GW, a.Cbm, '' as InvNo, '' as ShippingAPID, 
 '' as Type, '' as CurrencyID, 0 as Amount, '' as ShareBase, 0 as FtyWK
from ShareExpense a
inner join Export b on a.BLNo=b.Blno and a.WKNo=b.ID and a.GW=b.WeightKg
and a.cbm=b.Cbm and a.ShipModeID=b.ShipModeID and a.FtyWK=0 
WHERE B.Blno='{0}'), 
FtyExportData 
as 
(select a.WKNo,a.Blno,a.ShipModeID,a.GW, a.Cbm, '' as InvNo, '' as ShippingAPID, 
 '' as Type, '' as CurrencyID, 0 as Amount, '' as ShareBase, 1 as FtyWK
from ShareExpense a
inner join FtyExport b on a.BLNo=b.Blno and a.WKNo=b.ID and a.GW=b.WeightKg
and a.cbm=b.Cbm and a.ShipModeID=b.ShipModeID and a.FtyWK=1
WHERE B.Blno='{0}') 
select distinct * from ExportData 
union all 
select distinct * from FtyExportData ", e.FormattedValue.ToString());
#endregion
                           
                           
                            DBProxy.Current.Select(null, cmd_allWKNo, out dtAll);
                            if (MyUtility.Check.Empty(dtAll)) return;
                            for (int i = 0; i < dtAll.Rows.Count; i++)
                            {
                                DataRow NewRow = ((DataTable)listControlBindingSource1.DataSource).NewRow();

                                NewRow["Blno"] = dtAll.Rows[i]["Blno"];
                                NewRow["Wkno"] = dtAll.Rows[i]["Wkno"];
                                NewRow["ShipModeID"] = dtAll.Rows[i]["ShipModeID"];
                                NewRow["GW"] = dtAll.Rows[i]["GW"];
                                NewRow["CBM"] = dtAll.Rows[i]["CBM"];
                                NewRow["Amount"] = dtAll.Rows[i]["Amount"];
                                ((DataTable)listControlBindingSource1.DataSource).Rows.Add(NewRow);

                            }
                        }
                    }
                    else
                    {
                        string chkExp = string.Format(@"
with GMTBookingData
as 
(select id from GMTBooking
where id='{0}'), 
 PackingListData
as 
(select id from PackingList
where type in ('F','L')
and id='{0}'), 
FtyExportData 
as 
(select id
 from FtyExport
WHERE id='{0}' AND TYPE =3) 
select * from GMTBookingData 
union all
select * from PackingListData
union all 
select * from FtyExportData
 ", e.FormattedValue.ToString());
                        if (!MyUtility.Check.Seek(chkExp, out dr1))
                        {
                            MyUtility.Msg.InfoBox("<WKNo:>" + e.FormattedValue.ToString() + " Not Found!!");
                            drGrid.Delete();
                            e.Cancel = true;
                            return;
                        }
                        else
                        {
                            //判斷修改的資料是否正確
                            //正確就將所有B/L No所有WK#都帶出來
                            DataTable dtAll;
                            string cmd_allWKNo = string.Format(@"with GMTBookingData
as 
(select a.BLNo,a.InvNo,a.ShipModeID,a.GW,a.CBM,'' as Type,'' as CurrencyID, 0 as Amount, '' as ShareBase, 1 as FtyWK 
from ShareExpense a
inner join GMTBooking b on a.InvNo=b.id and a.GW=b.TotalGW and a.CBM=b.TotalCBM
and a.ShipModeID=b.ShipModeID and a.FtyWK=0
WHERE a.InvNo='{0}'), 
 PackingListData
as 
(select a.BLNo,a.InvNo,a.ShipModeID,a.GW,a.CBM,'' as Type,'' as CurrencyID, 0 as Amount, '' as ShareBase, 1 as FtyWK 
from ShareExpense a
inner join PackingList b on a.InvNo=b.id and a.GW=b.GW and a.CBM=b.CBM
and a.ShipModeID=b.ShipModeID and a.FtyWK=0
WHERE a.InvNo='{0}'), 
FtyExportData 
as 
(select a.BLNo,a.InvNo,a.ShipModeID,a.GW,a.CBM,'' as Type,'' as CurrencyID, 0 as Amount, '' as ShareBase, 1 as FtyWK 
from ShareExpense a
inner join FtyExport b on a.BLNo=b.Blno and a.WKNo=b.ID and a.GW=b.WeightKg
and a.cbm=b.Cbm and a.ShipModeID=b.ShipModeID and a.FtyWK=1
WHERE a.InvNo='{0}') 
select distinct * from GMTBookingData 
union all
select distinct * from PackingListData
union all 
select distinct * from FtyExportData
", e.FormattedValue.ToString());
                            DBProxy.Current.Select(null, cmd_allWKNo, out dtAll);
                            if (MyUtility.Check.Empty(dtAll)) return;

                            for (int i = 0; i < dtAll.Rows.Count; i++)
                            {

                                drGrid["Blno"] = dtAll.Rows[i]["Blno"];
                                drGrid["InvNo"] = dtAll.Rows[i]["InvNo"];
                                drGrid["ShipModeID"] = dtAll.Rows[i]["ShipModeID"];
                                drGrid["GW"] = dtAll.Rows[i]["GW"];
                                drGrid["CBM"] = dtAll.Rows[i]["CBM"];
                                drGrid["Amount"] = dtAll.Rows[i]["Amount"];

                            }
                        }
                    }
                }
            };
            #endregion

            this.grid1.IsEditingReadOnly = false;
            grid1.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.grid1)
                .Text("BLNo", header: "B/L No.", width: Widths.AnsiChars(13), settings: BLNo)
                .Text(MyUtility.Convert.GetString(apData["Type"]) == "IMPORT" ? "WKNo" : "InvNo", header: MyUtility.Convert.GetString(apData["Type"]) == "IMPORT" ? "WK#/Fty WK#" : "GB#/Fty WK#/Packing#", width: Widths.AnsiChars(18),settings: WKNO)
                .Text("ShipModeID", header: "Shipping Mode", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Numeric("GW", header: "G.W.", decimal_places: 2, iseditingreadonly: true)
                .Numeric("CBM", header: "CBM", decimal_places: 2, iseditingreadonly: true)
                .Numeric("Amount", header: "Total Amount", decimal_places: 2,iseditingreadonly:true);
            grid1.SelectionChanged += (s, e) =>
            {
                DataRow dr = this.grid1.GetDataRow<DataRow>(grid1.GetSelectedRowIndex());
                if (EditMode == true)
                {
                    if (dr != null)
                    {
                        string filter = "ShippingAPID = ''";
                        SEData.DefaultView.RowFilter = filter;
                    }
                }
                else
                {
                    if (dr != null)
                    {
                        string filter = string.Format("BLNo = '{0}' and WKNo = '{1}' and InvNo = '{2}'", MyUtility.Convert.GetString(dr["BLNo"]), MyUtility.Convert.GetString(dr["WKNo"]), MyUtility.Convert.GetString(dr["InvNo"]));
                        SEData.DefaultView.RowFilter = filter;
                    }
                }
            };

            this.grid2.IsEditingReadOnly = true;
            grid2.DataSource = listControlBindingSource2;
            Helper.Controls.Grid.Generator(this.grid2)
                .Text("AccountID", header: "Account No", width: Widths.AnsiChars(8))
                .Text("AccountName", header: "Account Name", width: Widths.AnsiChars(30))
                .Numeric("Amount", header: "Amount", decimal_places: 2)
                .Text("ShareRule", header: "Share by", width: Widths.AnsiChars(22));
            QueryData();
        }

        private void QueryData()
        {
            DataRow queryData;
            string sqlCmd = string.Format("select isnull(sum(GW),0) as GW,isnull(sum(CBM),0) as CBM,isnull(Count(BLNo),0) as RecCount from (select distinct BLNo,WKNo,InvNo,GW,CBM from ShareExpense WITH (NOLOCK) where ShippingAPID = '{0}') a", MyUtility.Convert.GetString(apData["ID"]));
            MyUtility.Check.Seek(sqlCmd, out queryData);
            numericBox2.Value = MyUtility.Convert.GetDecimal(queryData["GW"]);
            numericBox3.Value = MyUtility.Convert.GetDecimal(queryData["CBM"]);
            numericBox4.Value = MyUtility.Convert.GetInt(queryData["RecCount"]);

            sqlCmd = string.Format(@"select sh.*,an.Name as AccountName,
case when sh.ShareBase = 'G' then 'G.W.' when sh.ShareBase = 'C' then 'CBM' else ' Number od Deliver Sheets' end as ShareRule 
from ShareExpense sh WITH (NOLOCK) 
left join [FinanceEN].dbo.AccountNo an on an.ID = sh.AccountID
where sh.ShippingAPID = '{0}' order by sh.AccountID", MyUtility.Convert.GetString(apData["ID"]));
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out SEData);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query Detail fail.\r\n" + result.ToString());
                return;
            }
            listControlBindingSource2.DataSource = SEData;

            sqlCmd = string.Format("select ShippingAPID,BLNo,WKNo,InvNo,Type,ShipModeID,GW,CBM,CurrencyID,ShipModeID,FtyWK,isnull(sum(Amount),0) as Amount from ShareExpense WITH (NOLOCK) where ShippingAPID = '{0}' group by ShippingAPID,BLNo,WKNo,InvNo,Type,ShipModeID,GW,CBM,CurrencyID,ShipModeID,FtyWK", MyUtility.Convert.GetString(apData["ID"]));
            result = DBProxy.Current.Select(null, sqlCmd, out SEGroupData);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query Group Data fail.\r\n" + result.ToString());
                return;
            }
            listControlBindingSource1.DataSource = SEGroupData;
        }

        private void ControlButton()
        {
            if (EditMode == true)
            {
                button5.Text = "Save";
                button6.Text = "Undo";
            }
            else
            {
                button5.Text = "Edit";
                button6.Text = "Close";
                return;
            }
            DataRow dr;
            if (MyUtility.Check.Seek(string.Format(@"select * from ShippingAP where id='{0}'", apData["ID"]),out dr))
            {
                if (!MyUtility.Check.Empty(dr))
                {
                    if (dr["SubType"].ToString().ToUpper()=="OTHER")
                    {
                        this.button1.Enabled = false;
                    }
                    else
                    {
                        this.button1.Enabled = true;
                    }
                }
                else
                {
                    return;
                }
            }
            if (MyUtility.Check.Empty(SEGroupData))
            {
                this.button2.Enabled = false;
                this.button3.Enabled = false;
                return;
            }
            else
            {
                if (SEGroupData.Rows.Count==0)
                {
                    this.button2.Enabled = false;
                    this.button3.Enabled = false;
                    return;
                }
                this.button2.Enabled = true;
                this.button3.Enabled = true;
            }
            
        }

        //Import
        private void button4_Click(object sender, EventArgs e)
        {
            if (MyUtility.Convert.GetString(apData["SubType"]) == "OTHER")
            {

                DialogResult buttonResult = MyUtility.Msg.InfoBox("If you want to import \"Garment Data\" please click 'Yes'.\r\nIf you want to import \"Material Data\" please click 'No'.\r\nIf you don't want to import data please click 'Cancel'.", "Warning", MessageBoxButtons.YesNoCancel);
                if (buttonResult == System.Windows.Forms.DialogResult.Cancel)
                {
                    return;
                }
                else
                {
                    if (buttonResult == System.Windows.Forms.DialogResult.Yes)
                    {
                        Sci.Production.Shipping.P08_ShareExpense_ImportGarment callNextForm = new Sci.Production.Shipping.P08_ShareExpense_ImportGarment(SEGroupData);
                        callNextForm.ShowDialog(this);
                    }
                    else
                    {
                        Sci.Production.Shipping.P08_ShareExpense_ImportMaterial callNextForm = new Sci.Production.Shipping.P08_ShareExpense_ImportMaterial(SEGroupData,apData);
                        callNextForm.ShowDialog(this);
                    }
                }
            }
            else
            {
                if (MyUtility.Convert.GetString(apData["SubType"]) == "GARMENT")
                {
                    Sci.Production.Shipping.P08_ShareExpense_ImportGarment callNextForm = new Sci.Production.Shipping.P08_ShareExpense_ImportGarment(SEGroupData);
                    callNextForm.ShowDialog(this);
                }
                else
                {
                    Sci.Production.Shipping.P08_ShareExpense_ImportMaterial callNextForm = new Sci.Production.Shipping.P08_ShareExpense_ImportMaterial(SEGroupData, apData);
                    callNextForm.ShowDialog(this);
                }
            }
        }

        //Delete
        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult buttonResult = MyUtility.Msg.WarningBox("Do you want to delete this data?", "Warning", MessageBoxButtons.YesNo);
            if (buttonResult == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            var deletedata = SEGroupData;
            if (null == deletedata) return;
            foreach (DataGridViewRow row in this.grid1.SelectedRows)
            {
                try
                {
                    this.grid1.Rows.Remove(row);
                }
                catch (Exception)
                {
                    
                    throw;
                }
               
            }
        }

        //Edit / Save
        private void button5_Click(object sender, EventArgs e)
        {
            if (button5.Text == "Edit")
            {
                this.EditMode = true;
            }
            else
            {
                bool forwarderFee = MyUtility.Check.Seek(string.Format("select se.AccountID from ShippingAP_Detail sd WITH (NOLOCK) , ShipExpense se WITH (NOLOCK) where sd.ID = '{0}' and sd.ShipExpenseID = se.ID and (se.AccountID = '61022001' or se.AccountID = '61012001')", MyUtility.Convert.GetString(apData["ID"])));
                bool haveSea = false, noExistNotSea = true;
                DataTable duplicData;
                DBProxy.Current.Select(null, "select BLNo,WKNo,InvNo from ShareExpense WITH (NOLOCK) where 1=0", out duplicData);
                StringBuilder msg = new StringBuilder();

                foreach (DataRow dr in ((DataTable)listControlBindingSource1.DataSource).ToList<DataRow>())
                {
                    if (dr.RowState != DataRowState.Deleted)
                    {
                        if (MyUtility.Check.Empty(dr["BLNo"]) && MyUtility.Check.Empty(dr["WKNo"]) && MyUtility.Check.Empty(dr["InvNo"]))
                        {
                            dr.Delete();
                            continue;
                        }

                        //檢查重複值
                        DataRow[] findrow = duplicData.Select(string.Format("BLNo = '{0}' and WKNo = '{1}' and InvNo = '{2}'", MyUtility.Convert.GetString(dr["BLNo"]), MyUtility.Convert.GetString(dr["WKNo"]), MyUtility.Convert.GetString(dr["InvNo"])));
                        if (findrow.Length == 0)
                        {
                            duplicData.ImportRow(dr);
                        }
                        else
                        {
                            if (MyUtility.Convert.GetString(apData["Type"]) == "IMPORT")
                            {
                                msg.Append(string.Format("{0}\r\n", MyUtility.Convert.GetString(dr["WKno"])));
                            }
                            else
                            {
                                msg.Append(string.Format("{0}\r\n", MyUtility.Convert.GetString(dr["InvNo"])));
                            }
                        }

                        //當有Forwarder費用且SubType不是Material時，要檢查如果有一筆Ship Mode為SEA時，全部的Ship Mode就都要為SEA
                        if (forwarderFee && (MyUtility.Convert.GetString(apData["Type"]) == "EXPORT" || MyUtility.Convert.GetString(apData["SubType"]) == "SISTER FACTORY TRANSFER"))
                        {
                            haveSea = haveSea || MyUtility.Convert.GetString(dr["ShipModeID"]) == "SEA";
                            noExistNotSea = noExistNotSea && MyUtility.Convert.GetString(dr["ShipModeID"]) == "SEA";
                        }
                    }
                }

                if (msg.Length > 0)
                {
                    MyUtility.Msg.WarningBox("Data is duplicate!\r\n"+msg.ToString());
                    return;
                }

                if (haveSea && !noExistNotSea)
                {
                    MyUtility.Msg.WarningBox("Shipping Mode is inconsistent, can't be save.");
                    return;
                }

                #region 將資料寫入Table
                IList<string> deleteCmds = new List<string>();
                IList<string> addCmds = new List<string>();

                string accNo = MyUtility.GetValue.Lookup(string.Format("select se.AccountID from ShippingAP_Detail sd WITH (NOLOCK) , ShipExpense se WITH (NOLOCK) where sd.ID = '{0}' and sd.ShipExpenseID = se.ID and se.AccountID != ''", MyUtility.Convert.GetString(apData["ID"])));
                //刪除實體資料
                foreach (DataRow dr in ((DataTable)listControlBindingSource1.DataSource).Rows)
                {
                    if (dr.RowState == DataRowState.Deleted)
                    {
                        deleteCmds.Add(string.Format("delete ShareExpense where ShippingAPID = '{0}' and BLNo = '{1}' and WKNo = '{2}' and InvNo = '{3}';", MyUtility.Convert.GetString(apData["ID"]), MyUtility.Convert.GetString(dr["BLNo", DataRowVersion.Original]).Trim(), MyUtility.Convert.GetString(dr["WKNo", DataRowVersion.Original]).Trim(), MyUtility.Convert.GetString(dr["InvNo", DataRowVersion.Original]).Trim()));
                        
                    }
                }
                //新增實體資料
                foreach (DataRow dr in SEGroupData.Rows)
                {
                    if (dr.RowState == DataRowState.Added)
                    {
                        addCmds.Add(string.Format(@"insert into ShareExpense (ShippingAPID,BLNo,WKNo,InvNo,Type,GW,CBM,CurrencyID,ShipModeID,FtyWK,AccountID)
 values ('{0}','{1}','{2}','{3}','{4}',{5},{6},'{7}','{8}',{9},'{10}');", MyUtility.Convert.GetString(apData["ID"]), MyUtility.Convert.GetString(dr["BLNo"]), MyUtility.Convert.GetString(dr["WKNo"]), MyUtility.Convert.GetString(dr["InvNo"]),
                                                                 MyUtility.Convert.GetString(apData["SubType"]), MyUtility.Convert.GetString(dr["GW"]), MyUtility.Convert.GetString(dr["CBM"]), MyUtility.Convert.GetString(apData["CurrencyID"]), MyUtility.Convert.GetString(dr["ShipModeID"]),
                                                                 MyUtility.Convert.GetString(dr["FtyWK"]) == "True" ? "1" : "0", accNo));
                    }
                    if (dr.RowState == DataRowState.Modified)
                    {
                        addCmds.Add(string.Format(@"update ShareExpense set ShipModeID = '{0}', GW = {1}, CBM = {2} where ShippingAPID = '{3}' and BLNo = '{4}' and WKNo = '{5}' and InvNo = '{6}';",
                                                                 MyUtility.Convert.GetString(dr["ShipModeID"]), MyUtility.Convert.GetString(dr["GW"]), MyUtility.Convert.GetString(dr["CBM"]), MyUtility.Convert.GetString(apData["ID"]), MyUtility.Convert.GetString(dr["BLNo"]), MyUtility.Convert.GetString(dr["WKNo"]), MyUtility.Convert.GetString(dr["InvNo"])));
                    }
                }
                if (deleteCmds.Count != 0 || addCmds.Count != 0)
                {
                    using (TransactionScope transactionScope = new TransactionScope())
                    {
                        try
                        {
                            DualResult result, result1;
                            bool lastResult = true;
                            string errmsg = "";
                            if (deleteCmds.Count != 0)
                            {
                                result = DBProxy.Current.Executes(null, deleteCmds);
                                if (result)
                                {
                                    lastResult = lastResult && true;
                                }
                                else
                                {
                                    lastResult = false;
                                    errmsg = result.ToString() + "\r\n";
                                }
                            }
                            if (addCmds.Count != 0)
                            {
                                result1 = DBProxy.Current.Executes(null, addCmds);
                                if (result1)
                                {
                                    lastResult = lastResult && true;
                                }
                                else
                                {
                                    lastResult = false;
                                    errmsg = errmsg + result1.ToString() + "\r\n";
                                }
                            }

                            bool returnValue = Prgs.CalculateShareExpense(MyUtility.Convert.GetString(apData["ID"]));
                            if (!returnValue)
                            {
                                errmsg = errmsg +"Calcute share expense failed.";
                                lastResult = false;
                            }

                            if (lastResult)
                            {
                                transactionScope.Complete();
                            }
                            else
                            {
                                transactionScope.Dispose();
                                MyUtility.Msg.WarningBox("Confirm failed, Pleaes re-try\r\n" + errmsg);
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            transactionScope.Dispose();
                            ShowErr("Commit transaction error.", ex);
                            return;
                        }
                    }
                }
                #endregion 

                this.EditMode = false;
            }
            ControlButton();
            QueryData();
        }

        //Close / Undo
        private void button6_Click(object sender, EventArgs e)
        {
            if (button6.Text == "Close")
            {
                Close();
            }
            else
            {
                DialogResult buttonResult = MyUtility.Msg.WarningBox("Discard changes?", "Warning", MessageBoxButtons.YesNo);
                if (buttonResult == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }
                this.EditMode = false;
                ControlButton();
                QueryData();
            }
        }

        //Re-Calculate
        private void button7_Click(object sender, EventArgs e)
        {
            if (MyUtility.Convert.GetString(apData["Type"]) == "IMPORT")
            {
                string deleteCmd = string.Format("delete ShareExpense where ShippingAPID = '{0}' and WKNo != '' and WKNo not in (select ID from Export where ID = WKNo and ID is not null)",MyUtility.Convert.GetString(apData["ID"]));
                DualResult result = DBProxy.Current.Execute(null, deleteCmd);
                if (!result)
                {
                    MyUtility.Msg.ErrorBox("Re-Calculate Delete faile\r\n"+result.ToString());
                    return;
                }
                #region 組Update Sql
                string updateCmd = string.Format(@"DECLARE @apid VARCHAR(13),
		@id VARCHAR(13),
		@shipmode VARCHAR(10),
		@blno VARCHAR(20),
		@gw NUMERIC(9,2),
		@cbm NUMERIC(10,3),
		@currency VARCHAR(3),
		@subtype VARCHAR(15)

SET @apid = '{0}'
DECLARE cursor_allExport CURSOR FOR
	select e.ID,e.ShipModeID,e.Blno,e.WeightKg,e.Cbm,s.CurrencyID,s.SubType
	from Export e WITH (NOLOCK) , ShareExpense se WITH (NOLOCK) , ShippingAP s WITH (NOLOCK) 
	where e.ID = se.WKNo
	and s.ID = se.ShippingAPID
	and s.ID = @apid
OPEN cursor_allExport
FETCH NEXT FROM cursor_allExport INTO @id,@shipmode,@blno,@gw,@cbm,@currency,@subtype
WHILE @@FETCH_STATUS = 0
BEGIN
	update ShareExpense 
	set ShipModeID = @shipmode, BLNo = @blno, GW = @gw, CBM = @cbm, CurrencyID = @currency, Type = @subtype
	where ShippingAPID = @apid and WKNo = @id

	FETCH NEXT FROM cursor_allExport INTO @id,@shipmode,@blno,@gw,@cbm,@currency,@subtype
END
CLOSE cursor_allExport", MyUtility.Convert.GetString(apData["ID"]));
                #endregion
                result = DBProxy.Current.Execute(null, updateCmd);
                if (!result)
                {
                    MyUtility.Msg.ErrorBox("Re-Calculate update faile\r\n" + result.ToString());
                    return;
                }

            }
            else
            {
                string deleteCmd = string.Format("delete ShareExpense where ShippingAPID = '{0}' and InvNo != '' and (InvNo not in (select ID from GMTBooking where ID = InvNo and ID is not null) and InvNo not in (select INVNo from PackingList where INVNo = ShareExpense.InvNo and INVNo is not null) and InvNo not in (select ID from FtyExport  where ID = InvNo and ID is not null))", MyUtility.Convert.GetString(apData["ID"]));
                DualResult result = DBProxy.Current.Execute(null, deleteCmd);
                if (!result)
                {
                    MyUtility.Msg.ErrorBox("Re-Calculate Delete faile\r\n" + result.ToString());
                    return;
                }
                #region 組Update Sql
                string updateCmd = string.Format(@"DECLARE @apid VARCHAR(13),
		@id VARCHAR(13),
		@shipmode VARCHAR(10),
		@blno VARCHAR(20),
		@gw NUMERIC(9,2),
		@cbm NUMERIC(10,3),
		@currency VARCHAR(3),
		@subtype VARCHAR(15)

SET @apid = '{0}'
DECLARE cursor_GB CURSOR FOR
	select g.ID,g.ShipModeID,g.TotalGW,g.TotalCBM,s.CurrencyID,s.SubType, '' as BLNo
	from GMTBooking g WITH (NOLOCK) , ShippingAP s WITH (NOLOCK) , ShareExpense se WITH (NOLOCK) 
	where g.ID = se.InvNo
	and s.id = se.ShippingAPID
	and se.FtyWK = 0
	and s.id = @apid

DECLARE cursor_FtyWK CURSOR FOR
	select f.ID,f.ShipModeID,f.WeightKg,f.Cbm,s.CurrencyID,s.SubType, f.Blno
	from FtyExport f WITH (NOLOCK) , ShippingAP s WITH (NOLOCK) , ShareExpense se WITH (NOLOCK) 
	where f.ID = se.InvNo
	and s.id = se.ShippingAPID
	and se.FtyWK = 1
	and s.id = @apid

DECLARE cursor_PackingList CURSOR FOR
	select p.ID,p.ShipModeID,p.GW,p.CBM,s.CurrencyID,s.SubType, '' as BLNo
	from PackingList p WITH (NOLOCK) , ShippingAP s WITH (NOLOCK) , ShareExpense se WITH (NOLOCK) 
	where p.ID = se.InvNo
	and (p.Type = 'F' or p.Type = 'L')
	and s.id = se.ShippingAPID
	and se.FtyWK = 0
	and s.id = @apid

OPEN cursor_GB
FETCH NEXT FROM cursor_GB INTO @id,@shipmode,@gw,@cbm,@currency,@subtype,@blno
WHILE @@FETCH_STATUS = 0
BEGIN
	update ShareExpense 
	set ShipModeID = @shipmode, BLNo = @blno, GW = @gw, CBM = @cbm, CurrencyID = @currency, Type = @subtype
	where ShippingAPID = @apid and InvNo = @id

	FETCH NEXT FROM cursor_GB INTO @id,@shipmode,@gw,@cbm,@currency,@subtype,@blno
END
CLOSE cursor_GB

OPEN cursor_FtyWK
FETCH NEXT FROM cursor_FtyWK INTO @id,@shipmode,@gw,@cbm,@currency,@subtype,@blno
WHILE @@FETCH_STATUS = 0
BEGIN
	update ShareExpense 
	set ShipModeID = @shipmode, BLNo = @blno, GW = @gw, CBM = @cbm, CurrencyID = @currency, Type = @subtype
	where ShippingAPID = @apid and InvNo = @id

	FETCH NEXT FROM cursor_FtyWK INTO @id,@shipmode,@gw,@cbm,@currency,@subtype,@blno
END
CLOSE cursor_FtyWK

OPEN cursor_PackingList
FETCH NEXT FROM cursor_PackingList INTO @id,@shipmode,@gw,@cbm,@currency,@subtype,@blno
WHILE @@FETCH_STATUS = 0
BEGIN
	update ShareExpense 
	set ShipModeID = @shipmode, BLNo = @blno, GW = @gw, CBM = @cbm, CurrencyID = @currency, Type = @subtype
	where ShippingAPID = @apid and InvNo = @id

	FETCH NEXT FROM cursor_PackingList INTO @id,@shipmode,@gw,@cbm,@currency,@subtype,@blno
END
CLOSE cursor_PackingList", MyUtility.Convert.GetString(apData["ID"]));
                #endregion
                result = DBProxy.Current.Execute(null, updateCmd);
                if (!result)
                {
                    MyUtility.Msg.ErrorBox("Re-Calculate update faile\r\n" + result.ToString());
                    return;
                }

            }
            bool returnValue = Prgs.CalculateShareExpense(MyUtility.Convert.GetString(apData["ID"]));
            if (!returnValue)
            {
                MyUtility.Msg.WarningBox("Re-calcute share expense failed, please retry later.");
            }

            QueryData();
        }

        //Append
        private void button1_Click(object sender, EventArgs e)
        {
            DataRow NewRow = ((DataTable)listControlBindingSource1.DataSource).NewRow();
            //DataRow newRow = SEGroupData.NewRow();
            NewRow["Blno"]="";
            NewRow["Wkno"] = "";
            NewRow["ShipModeID"] = "";
            NewRow["GW"] = 0;
            NewRow["CBM"] = 0;
            NewRow["Amount"] = 0;
            ((DataTable)listControlBindingSource1.DataSource).Rows.Add(NewRow);
            

        }
        //Delete All
        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult buttonResult = MyUtility.Msg.WarningBox("Do you want to delete all this data?", "Warning", MessageBoxButtons.YesNo);
            if (buttonResult == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            DataTable grid1DT = (DataTable)this.listControlBindingSource1.DataSource;
            int t = grid1DT.Rows.Count;
            var deletedata = SEGroupData;           
            if (null == deletedata) return;
            for (int i = t - 1; i >= 0; i--)
            {
                grid1DT.Rows[i].Delete();
            }
           

        }
    }
}
