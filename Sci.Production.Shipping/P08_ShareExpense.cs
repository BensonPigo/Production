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
    /// <summary>
    /// P08_ShareExpense
    /// </summary>
    public partial class P08_ShareExpense : Sci.Win.Subs.Base
    {
        private DataRow apData;
        private DataTable SEData;
        private DataTable SEGroupData;

        /// <summary>
        /// P08_ShareExpense
        /// </summary>
        /// <param name="aPData">aPData</param>
        public P08_ShareExpense(DataRow aPData)
        {
            this.InitializeComponent();
            this.apData = aPData;
            this.displayCurrency.Value = MyUtility.Convert.GetString(this.apData["CurrencyID"]);
            this.numTtlAmt.DecimalPlaces = MyUtility.Convert.GetInt(MyUtility.GetValue.Lookup("Exact", MyUtility.Convert.GetString(this.apData["CurrencyID"]), "Currency", "ID"));
            this.numTtlAmt.Value = MyUtility.Convert.GetDecimal(this.apData["Amount"]);
            this.ControlButton();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            Ict.Win.DataGridViewGeneratorTextColumnSettings bLNo = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
            Ict.Win.DataGridViewGeneratorTextColumnSettings wKNO = new Ict.Win.DataGridViewGeneratorTextColumnSettings();

            #region BLNo
            bLNo.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return; // 非編輯模式
                }

                if (e.RowIndex == -1)
                {
                    return; // 沒東西 return
                }

                if (MyUtility.Check.Empty(e.FormattedValue))
                {
                    return; // 沒資料 return
                }

                string cmd_type = string.Format(@"select * from ShippingAP where id='{0}'", this.apData["ID"]);
                DataRow dr;
                DataRow drGrid = this.gridBLNo.GetDataRow<DataRow>(e.RowIndex);

                DataTable dts = (DataTable)this.listControlBindingSource1.DataSource;
                if (drGrid["BLNO"].ToString().ToUpper() == e.FormattedValue.ToString().ToUpper())
                {
                    return;
                }

                if (MyUtility.Check.Seek(cmd_type, out dr))
                {
                    // 刪除異動資料
                    if (drGrid["BLNO"].ToString().ToUpper() != e.FormattedValue.ToString().ToUpper() && !MyUtility.Check.Empty(drGrid["BLNO"].ToString()))
                    {
                        string blno = drGrid["BLNO"].ToString().ToUpper();
                        int t = dts.Rows.Count;
                        for (int i = t - 1; i >= 0; i--)
                        {
                            if (dts.Rows[i].RowState != DataRowState.Deleted)
                            {
                                if (dts.Rows[i]["BLNO"].ToString().ToUpper() == blno)
                                { // 刪除
                                    dts.Rows[i].Delete();
                                }
                            }
                        }

                        e.Cancel = true; // 不進入RowIndex判斷
                    }

                    // 檢查規則
                    // Type=Export FrtyExport(type=3)
                    if (dr["type"].ToString().ToUpper() == "EXPORT")
                    {
                        // 判斷資料是否存在
                        string chkExp = string.Format(
                            @"
select 0 as Selected,ID as WKNo,ShipModeID,WeightKg as GW, Cbm, '' as ShippingAPID, Blno,
'' as InvNo,'' as Type,'' as CurrencyID,0 as Amount,'' as ShareBase,1 as FtyWK
from FtyExport WITH (NOLOCK) 
 where Type = 3  and blno='{0}'
 ", e.FormattedValue.ToString());
                        DataTable dtExp;
                        DBProxy.Current.Select(null, chkExp, out dtExp);
                        if (MyUtility.Check.Empty(dtExp))
                        {
                            return;
                        }

                        if (dtExp.Rows.Count == 0)
                        {
                            drGrid.Delete();
                            e.Cancel = true;
                            MyUtility.Msg.InfoBox("<BLNo:>" + e.FormattedValue.ToString() + " Not Found!!");
                            return;
                        }
                        else
                        {
                            for (int i = 0; i < dtExp.Rows.Count; i++)
                            {
                                DataRow newRow = ((DataTable)this.listControlBindingSource1.DataSource).NewRow();
                                newRow["Blno"] = dtExp.Rows[i]["Blno"];
                                newRow["InvNo"] = dtExp.Rows[i]["Wkno"];
                                newRow["ShipModeID"] = dtExp.Rows[i]["ShipModeID"];
                                newRow["GW"] = dtExp.Rows[i]["GW"];
                                newRow["CBM"] = dtExp.Rows[i]["CBM"];
                                newRow["Amount"] = dtExp.Rows[i]["Amount"];
                                ((DataTable)this.listControlBindingSource1.DataSource).Rows.Add(newRow);
                            }
                        }
                    }

                    // Import FtyExport(type<>3)
                    else
                    {
                        string chkImp = string.Format(
                            @"
with FTY as
(
select 0 as Selected,ID as WKNo,ShipModeID,WeightKg as GW, Cbm, '' as ShippingAPID, Blno,
'' as InvNo,'' as Type,'' as CurrencyID,0 as Amount,'' as ShareBase,1 as FtyWK
from FtyExport WITH (NOLOCK) 
 where Type <> 3  and blno='{0}'
 ),
Expt as
(select 0 as Selected,ID as WKNo,ShipModeID,WeightKg as GW, Cbm, '' as ShippingAPID, Blno,
'' as InvNo,'' as Type,'' as CurrencyID,0 as Amount,'' as ShareBase,1 as FtyWK
from Export WITH (NOLOCK) 
 where blno='{0}')
select * from FTY
union all 
select * from Expt", e.FormattedValue.ToString());
                        DataTable dtImp;
                        DBProxy.Current.Select(null, chkImp, out dtImp);
                        if (MyUtility.Check.Empty(dtImp))
                        {
                            return;
                        }

                        if (dtImp.Rows.Count == 0)
                        {
                            drGrid.Delete();
                            e.Cancel = true;
                            MyUtility.Msg.InfoBox("<BLNo:>" + e.FormattedValue.ToString() + " Not Found!!");
                            return;
                        }
                        else
                        {
                            for (int i = 0; i < dtImp.Rows.Count; i++)
                            {
                                DataRow newRow = ((DataTable)this.listControlBindingSource1.DataSource).NewRow();

                                newRow["Blno"] = dtImp.Rows[i]["Blno"];
                                newRow["Wkno"] = dtImp.Rows[i]["Wkno"];
                                newRow["ShipModeID"] = dtImp.Rows[i]["ShipModeID"];
                                newRow["GW"] = dtImp.Rows[i]["GW"];
                                newRow["CBM"] = dtImp.Rows[i]["CBM"];
                                newRow["Amount"] = dtImp.Rows[i]["Amount"];
                                ((DataTable)this.listControlBindingSource1.DataSource).Rows.Add(newRow);
                            }
                        }
                    }

                    // delete empty rows
                    int t1 = dts.Rows.Count;
                    for (int i = t1 - 1; i >= 0; i--)
                    {
                        if (dts.Rows[i].RowState != DataRowState.Deleted)
                        {
                            // if (MyUtility.Check.Empty(dts.Rows[i]["Blno"].ToString())&& dr["type"].ToString().ToUpper()=="IMPORT")
                            // {
                            //    //刪除
                            //    dts.Rows[i].Delete();
                            // }
                            if (MyUtility.Check.Empty(dts.Rows[i]["Blno"].ToString()) && MyUtility.Check.Empty(dts.Rows[i]["WKNO"].ToString()) && MyUtility.Check.Empty(dts.Rows[i]["InvNO"].ToString()))
                            {
                                // 刪除
                                dts.Rows[i].Delete();
                            }
                        }
                    }

                    e.Cancel = true; // 不進入RowIndex判斷
                }
            };
#endregion
            #region WKNO
            wKNO.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return; // 非編輯模式
                }

                if (e.RowIndex == -1)
                {
                    return; // 沒東西 return
                }

                if (MyUtility.Check.Empty(e.FormattedValue))
                {
                    return; // 沒資料 return
                }

                string cmd_type = string.Format(@"select * from ShippingAP where id='{0}'", this.apData["ID"]);
                DataRow dr;
                DataRow drGrid = this.gridBLNo.GetDataRow<DataRow>(e.RowIndex);

                DataTable dts = (DataTable)this.listControlBindingSource1.DataSource;
                if (drGrid["WKNO"].ToString().ToUpper() == e.FormattedValue.ToString().ToUpper())
                {
                    return;
                }

                if (MyUtility.Check.Seek(cmd_type, out dr))
                {
                    // TYPE= Export : GB,Packing(type=F,L),FTYExport(type=3)
                    if (dr["Type"].ToString().ToUpper() == "EXPORT")
                    {
                        string chkExp = string.Format(
                            @"
with GB as 
(select distinct 0 as Selected,g.ID as InvNo,g.ShipModeID,g.TotalGW as GW, g.TotalCBM as CBM,
 '' as ShippingAPID, '' as BLNo, '' as WKNo, '' as Type, '' as CurrencyID, 0 as Amount,
 '' as ShareBase, 0 as FtyWK 
 from GMTBooking g  WITH (NOLOCK) 
 left join GMTBooking_CTNR gc WITH (NOLOCK) on gc.ID = g.ID 
 left Join PackingList p WITH (NOLOCK) on p.INVNo = g.ID 
 where 1=1  and g.id='{0}' ), 
PL as 
(select distinct 0 as Selected,ID as InvNo,ShipModeID,GW,CBM, '' as ShippingAPID, '' as BLNo,
'' as WKNo,'' as Type,'' as CurrencyID,0 as Amount, '' as ShareBase,0 as FtyWK 
 from PackingList WITH (NOLOCK) 
 where  (Type = 'F' or Type = 'L')  and id='{0}' ) ,
FTY AS
(select 0 as Selected,fe.ID as WKNo,fe.ShipModeID,WeightKg as GW, fe.Cbm, '' as ShippingAPID, Blno,
'' as InvNo,'' as Type,'' as CurrencyID,0 as Amount,'' as ShareBase,1 as FtyWK
from FtyExport fe WITH (NOLOCK) 
 where fe.Type = 3  and fe.id='{0}')
select * from GB 
union all 
select * from PL
union all
SELECT * FROM FTY
 ", e.FormattedValue.ToString());
                        DataTable dtExp;
                        DBProxy.Current.Select(null, chkExp, out dtExp);
                        if (dtExp == null && dtExp.Rows.Count == 0)
                        {
                            drGrid.Delete();
                            e.Cancel = true;
                            MyUtility.Msg.InfoBox("<WKNo:>" + e.FormattedValue.ToString() + " Not Found!!");
                            return;
                        }
                        else
                        {
                             string accNo = MyUtility.GetValue.Lookup(string.Format("select se.AccountID from ShippingAP_Detail sd WITH (NOLOCK) , ShipExpense se WITH (NOLOCK) where sd.ID = '{0}' and sd.ShipExpenseID = se.ID and se.AccountID != ''", MyUtility.Convert.GetString(this.apData["ID"])));
                            for (int i = 0; i < dtExp.Rows.Count; i++)
                            {
                                string strSqlcmd = $@"
 select 1 from AirPP a WITH (NOLOCK) 
 where exists (
	select 1 from PackingList p WITH (NOLOCK) 
	inner join PackingList_Detail pl WITH (NOLOCK)  on p.ID= pl.ID
	where pl.OrderID= a.OrderID
	and p.INVNo='{dtExp.Rows[i]["InvNo"]}'
 )";

                                if (!MyUtility.Check.Seek(strSqlcmd) &&
                                (dtExp.Rows[i]["ShipModeID"].ToString().CompareTo("A/P") == 0 ||
                                    dtExp.Rows[i]["ShipModeID"].ToString().CompareTo("E/P") == 0 ||
                                    dtExp.Rows[i]["ShipModeID"].ToString().CompareTo("E/P-C") == 0) &&
                                   (accNo.Substring(0, 4).CompareTo("6105") == 0 || accNo.Substring(0, 4).CompareTo("5912") == 0))
                                {
                                    MyUtility.Msg.WarningBox(@"Please maintain [Shipping P01 Air Pre-Paid] first if [Shipping Mode] is A/P, E/P or E/P-C !!");
                                    return;
                                }

                                drGrid["Blno"] = dtExp.Rows[i]["Blno"];
                                drGrid["InvNo"] = dtExp.Rows[i]["InvNo"];
                                drGrid["ShipModeID"] = dtExp.Rows[i]["ShipModeID"];
                                drGrid["GW"] = dtExp.Rows[i]["GW"];
                                drGrid["CBM"] = dtExp.Rows[i]["CBM"];
                                drGrid["Amount"] = dtExp.Rows[i]["Amount"];
                            }
                        }
                    }

                    // Type=Import
                    else
                    {
                        string chkImp = string.Format(
                            @"
with 
ExportData as 
(select 0 as Selected,ID as WKNo,Blno,ShipModeID,WeightKg as GW, Cbm, '' as InvNo, '' as ShippingAPID, 
 '' as Type, '' as CurrencyID, 0 as Amount, '' as ShareBase, 0 as FtyWK
 from Export WITH (NOLOCK)  
 where 1 = 1 and id='{0}' )
,
FtyExportData as 
(select 0 as Selected,ID as WKNo,Blno,ShipModeID,WeightKg as GW, Cbm, '' as InvNo, '' as ShippingAPID, 
 '' as Type, '' as CurrencyID, 0 as Amount, '' as ShareBase, 1 as FtyWK
 from FtyExport WITH (NOLOCK) 
 where Type <> 3  and id='{0}') 
select * from ExportData 
union all 
select * from FtyExportData ", e.FormattedValue.ToString());
                        DataTable dtImp;
                        DBProxy.Current.Select(null, chkImp, out dtImp);
                        if (MyUtility.Check.Empty(dtImp))
                        {
                            return;
                        }

                        if (dtImp.Rows.Count == 0)
                        {
                            drGrid.Delete();
                            e.Cancel = true;
                            MyUtility.Msg.InfoBox("<WKNo:>" + e.FormattedValue.ToString() + " Not Found!!");
                            return;
                        }
                        else
                        {
                             string accNo = MyUtility.GetValue.Lookup(string.Format("select se.AccountID from ShippingAP_Detail sd WITH (NOLOCK) , ShipExpense se WITH (NOLOCK) where sd.ID = '{0}' and sd.ShipExpenseID = se.ID and se.AccountID != ''", MyUtility.Convert.GetString(this.apData["ID"])));
                            for (int i = 0; i < dtImp.Rows.Count; i++)
                            {
                                string strSqlcmd = $@"
 select 1 from AirPP a WITH (NOLOCK) 
 where exists (
	select 1 from PackingList p WITH (NOLOCK) 
	inner join PackingList_Detail pl WITH (NOLOCK)  on p.ID= pl.ID
	where pl.OrderID= a.OrderID
	and p.INVNo='{dtImp.Rows[i]["InvNo"]}'
 )";
                                if (!MyUtility.Check.Seek(strSqlcmd) &&
                                (dtImp.Rows[i]["ShipModeID"].ToString().CompareTo("A/P") == 0 ||
                                    dtImp.Rows[i]["ShipModeID"].ToString().CompareTo("E/P") == 0 ||
                                    dtImp.Rows[i]["ShipModeID"].ToString().CompareTo("E/P-C") == 0) &&
                                   (accNo.Substring(0, 4).CompareTo("6105") == 0 || accNo.Substring(0, 4).CompareTo("5912") == 0))
                                {
                                    MyUtility.Msg.WarningBox(@"Please maintain [Shipping P01 Air Pre-Paid] first if [Shipping Mode] is A/P, E/P or E/P-C !!");
                                    return;
                                }

                                drGrid["Blno"] = dtImp.Rows[i]["Blno"];
                                drGrid["Wkno"] = dtImp.Rows[i]["Wkno"];
                                drGrid["ShipModeID"] = dtImp.Rows[i]["ShipModeID"];
                                drGrid["GW"] = dtImp.Rows[i]["GW"];
                                drGrid["CBM"] = dtImp.Rows[i]["CBM"];
                                drGrid["Amount"] = dtImp.Rows[i]["Amount"];
                            }
                        }
                    }
                }
            };
            #endregion

            this.gridBLNo.IsEditingReadOnly = false;
            this.gridBLNo.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridBLNo)
                .Text("BLNo", header: "B/L No.", width: Widths.AnsiChars(13), settings: bLNo)
                .Text(MyUtility.Convert.GetString(this.apData["Type"]) == "IMPORT" ? "WKNo" : "InvNo", header: MyUtility.Convert.GetString(this.apData["Type"]) == "IMPORT" ? "WK#/Fty WK#" : "GB#/Fty WK#/Packing#", width: Widths.AnsiChars(18), settings: wKNO)
                .Text("ShipModeID", header: "Shipping Mode", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Numeric("GW", header: "G.W.", decimal_places: 3, iseditingreadonly: true)
                .Numeric("CBM", header: "CBM", decimal_places: 4, iseditingreadonly: true)
                .Numeric("Amount", header: "Total Amount", decimal_places: 2, iseditingreadonly: true);
            this.gridBLNo.SelectionChanged += (s, e) =>
            {
                DataRow dr = this.gridBLNo.GetDataRow<DataRow>(this.gridBLNo.GetSelectedRowIndex());
                if (this.EditMode == true)
                {
                    if (dr != null)
                    {
                        string filter = "ShippingAPID = ''";
                        this.SEData.DefaultView.RowFilter = filter;
                    }
                }
                else
                {
                    if (dr != null)
                    {
                        string filter = string.Format("BLNo = '{0}' and WKNo = '{1}' and InvNo = '{2}'", MyUtility.Convert.GetString(dr["BLNo"]), MyUtility.Convert.GetString(dr["WKNo"]), MyUtility.Convert.GetString(dr["InvNo"]));
                        this.SEData.DefaultView.RowFilter = filter;
                    }
                }
            };

            this.gridAccountID.IsEditingReadOnly = true;
            this.gridAccountID.DataSource = this.listControlBindingSource2;
            this.Helper.Controls.Grid.Generator(this.gridAccountID)
                .Text("AccountID", header: "Account No", width: Widths.AnsiChars(8))
                .Text("AccountName", header: "Account Name", width: Widths.AnsiChars(30))
                .Numeric("Amount", header: "Amount", decimal_places: 2)
                .Text("ShareRule", header: "Share by", width: Widths.AnsiChars(22));

            string strCheckSql = $@"select 1 from ShareExpense WITH (NOLOCK)  where ShippingAPID = '{this.apData["ID"]}' and (Junk=0 or junk is null)";

            if (
                this.apData["SubType"].ToString().ToUpper() == "GARMENT" &&
                this.apData["Type"].ToString().ToUpper() == "EXPORT" &&
                !MyUtility.Check.Seek(strCheckSql))
            {
                this.AppendData();
            }

            this.QueryData();

        }

        private void AppendData()
        {
            string strSqlCmd = $@"
merge ShareExpense t
using (
	select top 1
[ShippingAPID] = '{this.apData["ID"]}'
,[BLNo] = BLNo
,[WKNo] = ''
,[InvNo] = id
,[Type] = '{this.apData["SubType"]}'
,[GW] = TotalGW
,[CBM] = TotalCBM
,[CurrencyID] = '{this.apData["CurrencyID"]}'
,[ShipModeID] = ShipModeID
,[FtyWK] = 0
,[AccountID] = (
	select top 1 se.AccountID from ShippingAP_Detail sd WITH(NOLOCK) , ShipExpense se WITH(NOLOCK)
   where sd.ID = '{this.apData["ID"]}' and sd.ShipExpenseID = se.ID and se.AccountID != '')
,[Junk] = 0
from GMTBooking g WITH (NOLOCK) 
where BLNo='{this.apData["BLNO"]}' or BL2No='{this.apData["BLNO"]}' ) as s 
on	t.ShippingAPID = s.ShippingAPID 
	and t.BLNO = s.BLNO
	and t.WKNO = s.WKNO
	and t.InvNo = s.InvNo
when not matched then 
	insert (ShippingAPID, BLNo, WKNo, InvNo, Type, GW, CBM, CurrencyID, ShipModeID, FtyWK, AccountID, Junk)
	values (s.ShippingAPID, s.BLNo, s.WKNo, s.InvNo, s.Type, s.GW, s.CBM, s.CurrencyID, s.ShipModeID, s.FtyWK, s.AccountID, s.Junk) ;";

            DualResult result;

            if (!(result = DBProxy.Current.Execute(string.Empty, strSqlCmd)))
            {
                this.ShowErr(result);
                return;
            }

            // 重新計算
            bool returnValue = Prgs.CalculateShareExpense(MyUtility.Convert.GetString(this.apData["ID"]));
            if (!returnValue)
            {
                MyUtility.Msg.WarningBox("Calcute share expense failed.");
            }
        }

        private void QueryData()
        {
            DataRow queryData;
            string sqlCmd = string.Format(
                @"
select  isnull(sum(GW),0) as GW
        , isnull(sum(CBM),0) as CBM
        , isnull(Count(BLNo),0) as RecCount 
from (
    select  distinct BLNo
            , WKNo
            , InvNo
            , GW
            , CBM 
    from ShareExpense WITH (NOLOCK) 
    where   ShippingAPID = '{0}'
            and (Junk = 0 or Junk is null)
) a", MyUtility.Convert.GetString(this.apData["ID"]));
            MyUtility.Check.Seek(sqlCmd, out queryData);
            this.numTtlGW.Value = MyUtility.Convert.GetDecimal(queryData["GW"]);
            this.numTtlCBM.Value = MyUtility.Convert.GetDecimal(queryData["CBM"]);
            this.numTtlNumberofDeliverySheets.Value = MyUtility.Convert.GetInt(queryData["RecCount"]);

            sqlCmd = string.Format(
                @"
select  sh.Junk,sh.ShippingAPID,sh.WKNo,sh.InvNo,sh.Type,sh.GW,sh.CBM,sh.Amount,sh.ShipModeID,sh.BLNO
        ,sh.AccountID
        , an.Name as AccountName
        , case 
            when sh.ShareBase = 'G' then 'G.W.' 
            when sh.ShareBase = 'C' then 'CBM' 
            else ' Number of Deliver Sheets' 
          end as ShareRule 
from ShareExpense sh WITH (NOLOCK) 
left join [FinanceEN].dbo.AccountNo an on an.ID = sh.AccountID
where   sh.ShippingAPID = '{0}' 
        and (sh.Junk = 0 or sh.Junk is null)
order by sh.AccountID", MyUtility.Convert.GetString(this.apData["ID"]));
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out this.SEData);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query Detail fail.\r\n" + result.ToString());
                return;
            }

            this.listControlBindingSource2.DataSource = this.SEData;

            sqlCmd = string.Format(
                @"
select  ShippingAPID
        , se.Blno
        , WKNo
        , InvNo
        , se.Type
        , ShipModeID
        , GW
        , CBM
        , CurrencyID
        , ShipModeID
        , FtyWK
        , isnull(sum(Amount),0) as Amount 
        , '' as SubTypeRule
from ShareExpense se WITH (NOLOCK) 
where   ShippingAPID = '{0}' 
        and (Junk = 0 or Junk is null)
group by ShippingAPID,se.BLNo,WKNo,InvNo,se.Type,ShipModeID,GW,CBM,CurrencyID,ShipModeID,FtyWK
", MyUtility.Convert.GetString(this.apData["ID"]));
            result = DBProxy.Current.Select(null, sqlCmd, out this.SEGroupData);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query Group Data fail.\r\n" + result.ToString());
                return;
            }

            this.listControlBindingSource1.DataSource = this.SEGroupData;
        }

        private void ControlButton()
        {
            if (this.EditMode == true)
            {
                this.btnSave.Text = "Save";
                this.btnUndo.Text = "Undo";
            }
            else
            {
                this.btnSave.Text = "Edit";
                this.btnUndo.Text = "Close";
                return;
            }

            // DataRow dr;
            // 移除subType=Other, 無法append!
            // if (MyUtility.Check.Seek(string.Format(@"select * from ShippingAP where id='{0}'", apData["ID"]),out dr))
            // {
            //    if (!MyUtility.Check.Empty(dr))
            //    {
            //        if (dr["SubType"].ToString().ToUpper()=="OTHER")
            //        {
            //            this.button1.Enabled = false;
            //        }
            //        else
            //        {
            //            this.button1.Enabled = true;
            //        }
            //    }
            //    else
            //    {
            //        return;
            //    }
            // }
            if (MyUtility.Check.Empty(this.SEGroupData))
            {
                this.btnDelete.Enabled = false;
                this.btnDeleteAll.Enabled = false;
                return;
            }
            else
            {
                if (this.SEGroupData.Rows.Count == 0)
                {
                    this.btnDelete.Enabled = false;
                    this.btnDeleteAll.Enabled = false;
                    return;
                }

                this.btnDelete.Enabled = true;
                this.btnDeleteAll.Enabled = true;
            }
        }

        // Import
        private void BtnImport_Click(object sender, EventArgs e)
        {
            int t;

            // 為EXPORT import時, "不要"將Packing FOC的資料query出來讓user選
            t = MyUtility.Convert.GetString(this.apData["Type"]) == "EXPORT" ? 0 : 1;
            if (MyUtility.Convert.GetString(this.apData["SubType"]) == "OTHER" && MyUtility.Convert.GetString(this.apData["Type"]) == "EXPORT")
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
                        Sci.Production.Shipping.P08_ShareExpense_ImportGarment callNextForm = new Sci.Production.Shipping.P08_ShareExpense_ImportGarment(this.SEGroupData, t);
                        callNextForm.ShowDialog(this);
                    }
                    else
                    {
                        Sci.Production.Shipping.P08_ShareExpense_ImportMaterial callNextForm = new Sci.Production.Shipping.P08_ShareExpense_ImportMaterial(this.SEGroupData, this.apData);
                        callNextForm.ShowDialog(this);
                    }
                }
            }
            else
            {
                if (MyUtility.Convert.GetString(this.apData["SubType"]) == "GARMENT")
                {
                    Sci.Production.Shipping.P08_ShareExpense_ImportGarment callNextForm = new Sci.Production.Shipping.P08_ShareExpense_ImportGarment(this.SEGroupData, t);
                    callNextForm.ShowDialog(this);
                }
                else
                {
                    Sci.Production.Shipping.P08_ShareExpense_ImportMaterial callNextForm = new Sci.Production.Shipping.P08_ShareExpense_ImportMaterial(this.SEGroupData, this.apData);
                    callNextForm.ShowDialog(this);
                }
            }
        }

        // Delete
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            DialogResult buttonResult = MyUtility.Msg.WarningBox("Do you want to delete this data?", "Warning", MessageBoxButtons.YesNo);
            if (buttonResult == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            var deletedata = this.SEGroupData;
            if (deletedata == null)
            {
                return;
            }

            foreach (DataGridViewRow row in this.gridBLNo.SelectedRows)
            {
                try
                {
                    this.gridBLNo.Rows.Remove(row);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        // Edit / Save
        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (this.btnSave.Text == "Edit")
            {
                this.EditMode = true;
            }
            else
            {
                this.gridBLNo.ValidateControl();                
                bool forwarderFee = MyUtility.Check.Seek(string.Format("select se.AccountID from ShippingAP_Detail sd WITH (NOLOCK) , ShipExpense se WITH (NOLOCK) where sd.ID = '{0}' and sd.ShipExpenseID = se.ID and (se.AccountID = '61022001' or se.AccountID = '61012001')", MyUtility.Convert.GetString(this.apData["ID"])));
                bool haveSea = false, noExistNotSea = true, noAirpp = false;
                DataTable duplicData;
                DBProxy.Current.Select(null, "select BLNo,WKNo,InvNo from ShareExpense WITH (NOLOCK) where 1=0", out duplicData);

                // 取得AccountNo
                  string accNo = MyUtility.GetValue.Lookup(string.Format("select se.AccountID from ShippingAP_Detail sd WITH (NOLOCK) , ShipExpense se WITH (NOLOCK) where sd.ID = '{0}' and sd.ShipExpenseID = se.ID and se.AccountID != ''", MyUtility.Convert.GetString(this.apData["ID"])));

                StringBuilder msg = new StringBuilder();
                foreach (DataRow dr in ((DataTable)this.listControlBindingSource1.DataSource).ToList<DataRow>())
                {
                    if (dr.RowState != DataRowState.Deleted)
                    {
                        if (MyUtility.Check.Empty(dr["BLNo"]) && MyUtility.Check.Empty(dr["WKNo"]) && MyUtility.Check.Empty(dr["InvNo"]))
                        {
                            dr.Delete();
                            continue;
                        }

                        // 檢查重複值
                        DataRow[] findrow = null;

                        findrow = duplicData.Select(string.Format(
                            @"BLNo = '{0}' and WKNo = '{1}' and InvNo = '{2}'",
                            MyUtility.Check.Empty(dr["BLNo"].ToString()) ? "Empty" : dr["BLNo"].ToString(),
                            MyUtility.Check.Empty(dr["WKNo"].ToString()) ? "Empty" : dr["WKNo"].ToString(),
                            MyUtility.Check.Empty(dr["InvNo"].ToString()) ? "Empty" : dr["InvNo"].ToString()));

                        // findrow = duplicData.Select(string.Format(@"BLNo = '{0}' and WKNo = '{1}' and InvNo = '{2}'", MyUtility.Convert.GetString(dr["BLNo"]), MyUtility.Convert.GetString(dr["WKNo"]), MyUtility.Convert.GetString(dr["InvNo"])));
                        if (findrow.Length == 0)
                        {
                            duplicData.ImportRow(dr);
                            for (int i = 0; i < duplicData.Rows.Count; i++)
                            {
                                if (MyUtility.Check.Empty(duplicData.Rows[i]["BLNo"].ToString()))
                                {
                                    duplicData.Rows[i]["BLNo"] = "Empty";
                                }

                                if (MyUtility.Check.Empty(duplicData.Rows[i]["WKNo"].ToString()))
                                {
                                    duplicData.Rows[i]["WKNo"] = "Empty";
                                }

                                if (MyUtility.Check.Empty(duplicData.Rows[i]["InvNo"].ToString()))
                                {
                                    duplicData.Rows[i]["InvNo"] = "Empty";
                                }
                            }
                        }
                        else
                        {
                            if (MyUtility.Convert.GetString(this.apData["Type"]) == "IMPORT")
                            {
                                msg.Append(string.Format("{0}\r\n", MyUtility.Convert.GetString(dr["WKno"])));
                            }
                            else
                            {
                                msg.Append(string.Format("{0}\r\n", MyUtility.Convert.GetString(dr["InvNo"])));
                            }
                        }

                        // 當有Forwarder費用且SubType不是Material時，要檢查如果有一筆Ship Mode為SEA時，全部的Ship Mode就都要為SEA
                        if (forwarderFee && (MyUtility.Convert.GetString(this.apData["Type"]) == "EXPORT" || MyUtility.Convert.GetString(this.apData["SubType"]) == "SISTER FACTORY TRANSFER"))
                        {
                            haveSea = haveSea || MyUtility.Convert.GetString(dr["ShipModeID"]) == "SEA";
                            noExistNotSea = noExistNotSea && MyUtility.Convert.GetString(dr["ShipModeID"]) == "SEA";
                        }

                        // Account Name含Air-Prepaid (6105/5912) 時, GB上的Ship Mode必須為A/P或E/P或 E/P-C,且該GB項下的SP須有APP No.
                        string strSqlcmd = $@"
select 1 from AirPP
where exists(	select 1 
from PackingList pl
	inner join PackingList_Detail pld on pl.id=pld.ID
	 where INVNo='{dr["InvNo"]}'
	 and pld.OrderID=AirPP.OrderID)";

                        if (!MyUtility.Check.Seek(strSqlcmd) &&
                            (dr["ShipModeID"].ToString().CompareTo("A/P") == 0 ||
                             dr["ShipModeID"].ToString().CompareTo("E/P") == 0 ||
                             dr["ShipModeID"].ToString().CompareTo("E/P-C") == 0) &&
                             (accNo.Substring(0, 4).CompareTo("6105") == 0 ||
                              accNo.Substring(0, 4).CompareTo("5912") == 0))
                        {
                            noAirpp = true;
                        }
                    }
                }

                if (msg.Length > 0)
                {
                    MyUtility.Msg.WarningBox("Data is duplicate!\r\n" + msg.ToString());
                    return;
                }

                if (haveSea && !noExistNotSea)
                {
                    MyUtility.Msg.WarningBox("Shipping Mode is inconsistent, can't be save.");
                    return;
                }

                if (noAirpp)
                {
                    MyUtility.Msg.WarningBox(@"Please maintain [Shipping P01 Air Pre-Paid] first if [Shipping Mode] is A/P, E/P or E/P-C !!");
                    return;
                }

                #region 將資料寫入Table
                IList<string> deleteCmds = new List<string>();
                IList<string> addCmds = new List<string>();

                // Junk實體資料
                foreach (DataRow dr in ((DataTable)this.listControlBindingSource1.DataSource).Rows)
                {
                    if (dr.RowState == DataRowState.Deleted)
                    {
                        deleteCmds.Add(string.Format(
                            @"
update s 
set s.Junk = 1
from  ShareExpense s
where   s.ShippingAPID = '{0}' 
        and s.BLNo = '{1}' 
        and s.WKNo = '{2}' 
        and s.InvNo = '{3}';",
                            MyUtility.Convert.GetString(this.apData["ID"]),
                            MyUtility.Convert.GetString(dr["BLNo", DataRowVersion.Original]).Trim(),
                            MyUtility.Convert.GetString(dr["WKNo", DataRowVersion.Original]).Trim(),
                            MyUtility.Convert.GetString(dr["InvNo", DataRowVersion.Original]).Trim()));
                    }
                }

                // 新增實體資料
                foreach (DataRow dr in this.SEGroupData.Rows)
                {
                    if (dr.RowState == DataRowState.Added)
                    {
                        addCmds.Add(string.Format(
                            @"
merge ShareExpense t
using (select '{0}', '{1}', '{2}', '{3}') as s (ShippingAPID, BLNO, WKNO, InvNo)
on	t.ShippingAPID = s.ShippingAPID 
	and t.BLNO = s.BLNO
	and t.WKNO = s.WKNO
	and t.InvNo = s.InvNo
when matched then
	update set t.Junk = 0
    ,ShipModeID = '{8}'
    , GW = {5}
    , CBM = {6} 
when not matched then 
	insert (ShippingAPID, BLNo, WKNo, InvNo, Type, GW, CBM, CurrencyID, ShipModeID, FtyWK, AccountID, Junk)
	values ('{0}', '{1}', '{2}', '{3}', '{4}', {5}, {6}, '{7}', '{8}', {9}, '{10}', 0);",
                            MyUtility.Convert.GetString(this.apData["ID"]),
                            MyUtility.Convert.GetString(dr["BLNo"]),
                            MyUtility.Convert.GetString(dr["WKNo"]),
                            MyUtility.Convert.GetString(dr["InvNo"]),
                            MyUtility.Convert.GetString(this.apData["SubType"]),
                            MyUtility.Convert.GetString(dr["GW"]),
                            MyUtility.Convert.GetString(dr["CBM"]),
                            MyUtility.Convert.GetString(this.apData["CurrencyID"]),
                            MyUtility.Convert.GetString(dr["ShipModeID"]),
                            MyUtility.Convert.GetString(dr["FtyWK"]) == "True" ? "1" : "0",
                            accNo));
                    }

                    if (dr.RowState == DataRowState.Modified)
                    {
                        addCmds.Add(string.Format(
                            @"
update ShareExpense 
set ShipModeID = '{0}'
    , GW = {1}
    , CBM = {2} 
where   ShippingAPID = '{3}' 
        and BLNo = '{4}' 
        and WKNo = '{5}' 
        and InvNo = '{6}';",
                            MyUtility.Convert.GetString(dr["ShipModeID"]),
                            MyUtility.Convert.GetString(dr["GW"]),
                            MyUtility.Convert.GetString(dr["CBM"]),
                            MyUtility.Convert.GetString(this.apData["ID"]),
                            MyUtility.Convert.GetString(dr["BLNo"]),
                            MyUtility.Convert.GetString(dr["WKNo"]),
                            MyUtility.Convert.GetString(dr["InvNo"])));
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
                            string errmsg = string.Empty;
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

                            bool returnValue = Prgs.CalculateShareExpense(MyUtility.Convert.GetString(this.apData["ID"]));
                            if (!returnValue)
                            {
                                errmsg = errmsg + "Calcute share expense failed.";
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
                            this.ShowErr("Commit transaction error.", ex);
                            return;
                        }
                    }
                }
                #endregion

                this.EditMode = false;
            }

            this.ControlButton();
            this.QueryData();
        }

        // Close / Undo
        private void BtnUndo_Click(object sender, EventArgs e)
        {
            if (this.btnUndo.Text == "Close")
            {
                this.Close();
            }
            else
            {
                DialogResult buttonResult = MyUtility.Msg.WarningBox("Discard changes?", "Warning", MessageBoxButtons.YesNo);
                if (buttonResult == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }

                this.EditMode = false;
                this.ControlButton();
                this.QueryData();
            }
        }

        // Re-Calculate
        private void BtnReCalculate_Click(object sender, EventArgs e)
        {
            if (MyUtility.Convert.GetString(this.apData["Type"]) == "IMPORT")
            {
                string deleteCmd = string.Format(
                    @"
update s
set s.Junk = 1
from ShareExpense s
where s.ShippingAPID = '{0}' and s.WKNo != '' 
and s.WKNo not in (select ID from Export where ID = s.WKNo and ID is not null)
and s.WKNo not in (select ID from FtyExport where ID = s.WKNo and ID is not null)", MyUtility.Convert.GetString(this.apData["ID"]));
                DualResult result = DBProxy.Current.Execute(null, deleteCmd);
                if (!result)
                {
                    MyUtility.Msg.ErrorBox("Re-Calculate Delete faile\r\n" + result.ToString());
                    return;
                }
                #region 組Update Sql
                string updateCmd = string.Format(
                    @"
DECLARE @apid VARCHAR(13),
		@id VARCHAR(13),
		@shipmode VARCHAR(10),
		@blno VARCHAR(20),
		@gw NUMERIC(9,2),
		@cbm NUMERIC(10,4),
		@currency VARCHAR(3),
		@subtype VARCHAR(15)

SET @apid = '{0}'
DECLARE cursor_allExport CURSOR FOR
	select  e.ID
            , e.ShipModeID
            , e.Blno
            , e.WeightKg
            , e.Cbm
            , s.CurrencyID
            , s.SubType
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
CLOSE cursor_allExport", MyUtility.Convert.GetString(this.apData["ID"]));
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
                string deleteCmd = string.Format(
                    @"
update s
set s.Junk = 1
from ShareExpense s
where s.ShippingAPID = '{0}' and s.InvNo != '' 
and (
	s.InvNo not in (select ID from GMTBooking where ID = s.InvNo and ID is not null) 
	and s.InvNo not in (select INVNo from PackingList where INVNo = s.InvNo and INVNo is not null) 
	and s.InvNo not in (select ID from FtyExport  where ID = s.InvNo and ID is not null)
	and s.invno not in (select id from Export where id=s.InvNo and id is not null)
)", MyUtility.Convert.GetString(this.apData["ID"]));
                DualResult result = DBProxy.Current.Execute(null, deleteCmd);
                if (!result)
                {
                    MyUtility.Msg.ErrorBox("Re-Calculate Delete faile\r\n" + result.ToString());
                    return;
                }
                #region 組Update Sql
                string updateCmd = string.Format(
                    @"
DECLARE @apid VARCHAR(13),
		@id VARCHAR(13),
		@shipmode VARCHAR(10),
		@blno VARCHAR(20),
		@gw NUMERIC(9,2),
		@cbm NUMERIC(10,4),
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
CLOSE cursor_PackingList", MyUtility.Convert.GetString(this.apData["ID"]));
                #endregion
                result = DBProxy.Current.Execute(null, updateCmd);
                if (!result)
                {
                    MyUtility.Msg.ErrorBox("Re-Calculate update faile\r\n" + result.ToString());
                    return;
                }
            }

            bool returnValue = Prgs.CalculateShareExpense(MyUtility.Convert.GetString(this.apData["ID"]));
            if (!returnValue)
            {
                MyUtility.Msg.WarningBox("Re-calcute share expense failed, please retry later.");
            }

            this.QueryData();
        }

        // Append
        private void BtnAppend_Click(object sender, EventArgs e)
        {
            DataRow newRow = ((DataTable)this.listControlBindingSource1.DataSource).NewRow();
            newRow["Blno"] = string.Empty;
            newRow["Wkno"] = string.Empty;
            newRow["ShipModeID"] = string.Empty;
            newRow["GW"] = 0;
            newRow["CBM"] = 0;
            newRow["Amount"] = 0;
            ((DataTable)this.listControlBindingSource1.DataSource).Rows.Add(newRow);
        }

        // Delete All
        private void BtnDeleteAll_Click(object sender, EventArgs e)
        {
            DialogResult buttonResult = MyUtility.Msg.WarningBox("Do you want to delete all this data?", "Warning", MessageBoxButtons.YesNo);
            if (buttonResult == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            DataTable grid1DT = (DataTable)this.listControlBindingSource1.DataSource;
            int t = grid1DT.Rows.Count;
            var deletedata = this.SEGroupData;
            if (deletedata == null)
            {
                return;
            }

            for (int i = t - 1; i >= 0; i--)
            {
                grid1DT.Rows[i].Delete();
            }
        }
    }
}
