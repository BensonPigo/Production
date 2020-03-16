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
using System.Linq;
using System.Configuration;
using System.Data.SqlClient;

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
        private DataTable SAPP;
        private bool Apflag;
        private string LocalSuppID;

        /// <summary>
        /// P08_ShareExpense
        /// </summary>
        /// <param name="aPData">aPData</param>
        public P08_ShareExpense(DataRow aPData, bool apflag)
        {
            this.InitializeComponent();
            this.apData = aPData;
            this.Apflag = apflag;
            this.displayCurrency.Value = MyUtility.Convert.GetString(this.apData["CurrencyID"]);
            this.numTtlAmt.DecimalPlaces = MyUtility.Convert.GetInt(MyUtility.GetValue.Lookup("Exact", MyUtility.Convert.GetString(this.apData["CurrencyID"]), "Currency", "ID"));
            this.numTtlAmt.Value = MyUtility.Convert.GetDecimal(this.apData["Amount"]);
            this.LocalSuppID = MyUtility.Convert.GetString(this.apData["LocalSuppID"]);
            this.ControlButton();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            Ict.Win.DataGridViewGeneratorTextColumnSettings bLNo = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
            Ict.Win.DataGridViewGeneratorTextColumnSettings bL2No = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
            Ict.Win.DataGridViewGeneratorTextColumnSettings wKNO = new Ict.Win.DataGridViewGeneratorTextColumnSettings();

            #region BLNo
            bLNo.CellValidating += (s, e) =>
            {
                if (!this.EditMode ||
                    e.RowIndex == -1 ||
                    MyUtility.Check.Empty(e.FormattedValue))
                {
                    return; // 非編輯模式
                }

                string cmd_type = string.Format(@"select * from ShippingAP where id='{0}'", this.apData["ID"]);
                DataRow dr;
                DataRow drGrid = this.gridBLNo.GetDataRow<DataRow>(e.RowIndex);

                DataTable dts = (DataTable)this.listControlBindingSource1.DataSource;
                if (drGrid["BLNO"].ToString().ToUpper() == e.FormattedValue.ToString().ToUpper() ||
                    (!drGrid.RowState.Equals(DataRowState.Added) && (e.FormattedValue.ToString().ToUpper().EqualString(drGrid["BLNO", DataRowVersion.Original].ToString().ToUpper()) || MyUtility.Check.Empty(drGrid["BLNO", DataRowVersion.Original]))))
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
                                newRow["Bl2no"] = string.Empty;
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
                                newRow["Bl2no"] = string.Empty;
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
            #region BL2No
            bL2No.CellValidating += (s, e) =>
            {
                if (!this.EditMode ||
                    e.RowIndex == -1 ||
                    MyUtility.Check.Empty(e.FormattedValue))
                {
                    return;
                }

                string cmd_type = string.Format(@"select * from ShippingAP where id='{0}'", this.apData["ID"]);
                DataRow drGrid = this.gridBLNo.GetDataRow<DataRow>(e.RowIndex);
                DataTable dts = (DataTable)this.listControlBindingSource1.DataSource;
                if (drGrid["BL2NO"].ToString().ToUpper() == e.FormattedValue.ToString().ToUpper() ||
                    (!drGrid.RowState.Equals(DataRowState.Added) && (e.FormattedValue.ToString().ToUpper().EqualString(drGrid["BL2NO", DataRowVersion.Original].ToString().ToUpper()) || MyUtility.Check.Empty(drGrid["BL2NO", DataRowVersion.Original]))))
                {
                    return;
                }

                if (MyUtility.Check.Seek(cmd_type))
                {
                    // 刪除異動資料
                    if (drGrid["BL2NO"].ToString().ToUpper() != e.FormattedValue.ToString().ToUpper() && !MyUtility.Check.Empty(drGrid["BL2NO"].ToString()))
                    {
                        string blno = drGrid["BL2NO"].ToString().ToUpper();
                        int t = dts.Rows.Count;
                        for (int i = t - 1; i >= 0; i--)
                        {
                            if (dts.Rows[i].RowState != DataRowState.Deleted)
                            {
                                if (dts.Rows[i]["BL2NO"].ToString().ToUpper() == blno)
                                { // 刪除
                                    dts.Rows[i].Delete();
                                }
                            }
                        }

                        e.Cancel = true; // 不進入RowIndex判斷
                    }

                    // 判斷資料是否存在
                    string strChk =
$@"select 0 as Selected,g.ID as InvNo,g.ShipModeID,g.TotalGW as GW, g.TotalCBM as CBM,
	'' as ShippingAPID ,g.BLNo, g.BL2No
	,  '' as WKNo, '' as Type, '' as CurrencyID, 0 as Amount,
	'' as ShareBase, 0 as FtyWK 
from GMTBooking g where g.BL2No ='{e.FormattedValue.ToString()}'";
                    DataTable dtBl2No;
                    DBProxy.Current.Select(null, strChk, out dtBl2No);
                    if (MyUtility.Check.Empty(dtBl2No))
                    {
                        return;
                    }

                    if (dtBl2No.Rows.Count == 0)
                    {
                        drGrid.Delete();
                        e.Cancel = true;
                        MyUtility.Msg.InfoBox("<BL2No:>" + e.FormattedValue.ToString() + " Not Found!!");
                        return;
                    }
                    else
                    {
                        for (int i = 0; i < dtBl2No.Rows.Count; i++)
                        {
                            // 第一筆寫回原資料列
                            if (i == 0)
                            {
                                drGrid["Blno"] = dtBl2No.Rows[i]["Blno"];
                                drGrid["Bl2no"] = dtBl2No.Rows[i]["Bl2no"];
                                drGrid["InvNo"] = dtBl2No.Rows[i]["InvNo"];
                                drGrid["ShipModeID"] = dtBl2No.Rows[i]["ShipModeID"];
                                drGrid["GW"] = dtBl2No.Rows[i]["GW"];
                                drGrid["CBM"] = dtBl2No.Rows[i]["CBM"];
                                drGrid["Amount"] = dtBl2No.Rows[i]["Amount"];
                            }
                            else
                            {
                                DataRow newRow = ((DataTable)this.listControlBindingSource1.DataSource).NewRow();
                                newRow["Blno"] = dtBl2No.Rows[i]["Blno"];
                                newRow["Bl2no"] = dtBl2No.Rows[i]["Bl2no"];
                                newRow["InvNo"] = dtBl2No.Rows[i]["InvNo"];
                                newRow["ShipModeID"] = dtBl2No.Rows[i]["ShipModeID"];
                                newRow["GW"] = dtBl2No.Rows[i]["GW"];
                                newRow["CBM"] = dtBl2No.Rows[i]["CBM"];
                                newRow["Amount"] = dtBl2No.Rows[i]["Amount"];
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
                if (!this.EditMode ||
                    e.RowIndex == -1 ||
                    MyUtility.Check.Empty(e.FormattedValue))
                {
                    return; // 非編輯模式
                }

                string cmd_type = string.Format(@"select * from ShippingAP where id='{0}'", this.apData["ID"]);
                DataRow dr;
                DataRow drGrid = this.gridBLNo.GetDataRow<DataRow>(e.RowIndex);

                DataTable dts = (DataTable)this.listControlBindingSource1.DataSource;
                if (drGrid["WKNO"].ToString().ToUpper() == e.FormattedValue.ToString().ToUpper() ||
                    (!drGrid.RowState.Equals(DataRowState.Added) && (e.FormattedValue.ToString().ToUpper().EqualString(drGrid["WKNO", DataRowVersion.Original].ToString().ToUpper()) || MyUtility.Check.Empty(drGrid["WKNO", DataRowVersion.Original]))))
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
(   
    select distinct 0 as Selected,g.ID as InvNo,g.ShipModeID,g.TotalGW as GW, g.TotalCBM as CBM,
	'' as ShippingAPID ,g.BLNo, g.BL2No
	,  '' as WKNo, '' as Type, '' as CurrencyID, 0 as Amount,
	'' as ShareBase, 0 as FtyWK 
	from GMTBooking g  WITH (NOLOCK) 
	left join GMTBooking_CTNR gc WITH (NOLOCK) on gc.ID = g.ID 
	left Join PackingList p WITH (NOLOCK) on p.INVNo = g.ID 
    where 1=1  and g.id='{0}' 
), 
PL as 
(   select distinct 0 as Selected,ID as InvNo,ShipModeID,GW,CBM, '' as ShippingAPID, '' as BLNo,'' as BL2No,
    '' as WKNo,'' as Type,'' as CurrencyID,0 as Amount, '' as ShareBase,0 as FtyWK 
    from PackingList WITH (NOLOCK) 
    where  (Type = 'F' or Type = 'L')  and id='{0}' 
) ,
FTY AS
(
    select 0 as Selected,fe.ID as WKNo,fe.ShipModeID,WeightKg as GW, fe.Cbm, '' as ShippingAPID, Blno, '' as Bl2No,
    '' as InvNo,'' as Type,'' as CurrencyID,0 as Amount,'' as ShareBase,1 as FtyWK
    from FtyExport fe WITH (NOLOCK) 
    where fe.Type = 3  and fe.id='{0}'
)
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
                                   (accNo.Substring(0, 4).CompareTo("6105") == 0 || accNo.Substring(0, 4).CompareTo("5912") == 0) &&
                                    !dtExp.Rows[i]["FtyWK"].ToString().EqualString("1"))
                                {
                                    MyUtility.Msg.WarningBox(@"Please maintain [Shipping P01 Air Pre-Paid] first if [Shipping Mode] is A/P, E/P or E/P-C !!");
                                    return;
                                }

                                drGrid["Blno"] = dtExp.Rows[i]["Blno"];
                                drGrid["Bl2no"] = dtExp.Rows[i]["Bl2no"];
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
                                   (accNo.Substring(0, 4).CompareTo("6105") == 0 || accNo.Substring(0, 4).CompareTo("5912") == 0) &&
                                    !dtImp.Rows[i]["FtyWK"].ToString().EqualString("1"))
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
                .Text("BLNo", header: "BL/MAWB No.", width: Widths.AnsiChars(13), settings: bLNo)
                .Text("BL2No", header: "FCR/BL/HAWB", width: Widths.AnsiChars(13), settings: bL2No)
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
                        this.SAPP.DefaultView.RowFilter = filter;
                    }
                }
                else
                {
                    if (dr != null)
                    {
                        string filter = string.Format("BLNo = '{0}' and WKNo = '{1}' and InvNo = '{2}'", MyUtility.Convert.GetString(dr["BLNo"]), MyUtility.Convert.GetString(dr["WKNo"]), MyUtility.Convert.GetString(dr["InvNo"]));
                        this.SEData.DefaultView.RowFilter = filter;

                        string filter2 = $" InvNo = '{dr["InvNo"]}'";
                        this.SAPP.DefaultView.RowFilter = filter2;
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

            if (this.apData["SubType"].ToString().ToUpper() == "GARMENT"
                && this.apData["Type"].ToString().ToUpper() == "EXPORT"
                && !MyUtility.Check.Seek(strCheckSql))
            {
                if (ConfigurationManager.AppSettings["TaipeiServer"] == string.Empty
                    || (ConfigurationManager.AppSettings["TaipeiServer"] != string.Empty
                        && DBProxy.Current.DefaultModuleName.Contains("PMSDB") == false))
                {
                    this.AppendData();
                }
            }

            #region tab Shared Amt by App grid
            this.gridSAPP.IsEditingReadOnly = true;
            this.gridSAPP.DataSource = this.listControlBindingSource3;
            this.Helper.Controls.Grid.Generator(this.gridSAPP)
            .Text("PackingListID", header: "Packing#", width: Widths.AnsiChars(16))

            .Text("OrderID", header: "SP No.", width: Widths.AnsiChars(16))
            .Text("OrderShipmodeSeq", header: "Seq", width: Widths.AnsiChars(3))
            .Numeric("NW", header: "N.W.", decimal_places: 3)
            .Text("AirPPID", header: "APP#", width: Widths.AnsiChars(16))
            .Text("AccountID", header: "Account No", width: Widths.AnsiChars(10))
            .Text("Name", header: "Account Name", width: Widths.AnsiChars(20))
            .Numeric("RatioFty", header: "Factory Ratio", decimal_places: 2)
            .Numeric("AmtFty", header: "Share Amt - Fty", decimal_places: 2)
            .Numeric("RatioOther", header: "Other Ratio", decimal_places: 2)
            .Numeric("AmtOther", header: "Share Amt - Other", decimal_places: 2)
            .Numeric("APPExchageRate", header: "APP Exchage Rate", decimal_places: 6)
            .Numeric("APPAmt", header: "APP Amt (USD)", decimal_places: 2)
            ;
            #endregion
            this.QueryData();
        }

        private void AppendData()
        {
            string strSqlCmd = $@"
merge ShareExpense t
using (
	select distinct
[ShippingAPID] = '{this.apData["ID"]}'
,[BLNo] = iif(BLNo is null or BLNo='', BL2No,BLNo)
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
	and t.WKNO = s.WKNO	and t.InvNo = s.InvNo
when matched AND t.junk=1 then
	update set
	t.junk=0
when not matched by target then 
	insert (ShippingAPID, BLNo, WKNo, InvNo, Type, GW, CBM, CurrencyID, ShipModeID, FtyWK, AccountID, Junk)
	values (s.ShippingAPID, s.BLNo, s.WKNo, s.InvNo, s.Type, s.GW, s.CBM, s.CurrencyID, s.ShipModeID, s.FtyWK, s.AccountID, s.Junk);";

            DualResult result;

            if (!(result = DBProxy.Current.Execute(string.Empty, strSqlCmd)))
            {
                this.ShowErr(result);
                return;
            }

            // 重新計算
            result = DBProxy.Current.Execute(
                "Production",
                string.Format("exec CalculateShareExpense '{0}','{1}'", MyUtility.Convert.GetString(this.apData["ID"]), Sci.Env.User.UserID));
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Calcute share expense failed.\r\n" + result.ToString());
                return;
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
select [Amount] = Sum(sd.Amount)
from ShippingAP_Detail sd WITH (NOLOCK)
left join ShipExpense se WITH (NOLOCK) on se.ID = sd.ShipExpenseID
where sd.ID = '{0}'
and not (dbo.GetAccountNoExpressType(se.AccountID,'Vat') = 1 or dbo.GetAccountNoExpressType(se.AccountID,'SisFty') = 1)", MyUtility.Convert.GetString(this.apData["ID"]));
            MyUtility.Check.Seek(sqlCmd, out queryData);
            this.numTtlAmt.Value = MyUtility.Convert.GetDecimal(queryData["Amount"]);

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
left join SciFMS_AccountNo an on an.ID = sh.AccountID
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

            List<string> ilist = this.SEData.AsEnumerable().Select(s => MyUtility.Convert.GetString(s["InvNo"])).Distinct().ToList();
            string invNos = "'" + string.Join("','", ilist) + "'";
            #region Shared Amt by APP
            sqlCmd = $@"
select
    sa.ShippingAPID,
    sa.InvNo,
	sa.PackingListID,
	AirPP.OrderID,
	AirPP.OrderShipmodeSeq,
	sa.AirPPID,
	sa.NW,
	sa.AccountID,
	Name = (select Name from FinanceEN.dbo.AccountNo a with(Nolock) where a.ID = sa.AccountID),
	sa.RatioFty,
	sa.AmtFty,
	sa.RatioOther,
	sa.AmtOther,
    sap.APPExchageRate,
    APPAmt=iif(APPExchageRate=0,0, round((sa.AmtFty+sa.AmtOther)/sap.APPExchageRate,2))
from ShareExpense_APP sa with(nolock)
inner join AirPP with(nolock) on AirPP.id = sa.AirPPID
inner  join ShippingAP sap on sap.ID = sa.ShippingAPID
where sa.Junk = 0
and sa.ShippingAPID = '{this.apData["ID"]}' 
and sa.InvNo in ({invNos})
";
            result = DBProxy.Current.Select(null, sqlCmd, out this.SAPP);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.listControlBindingSource3.DataSource = this.SAPP;
            #endregion

            sqlCmd = string.Format(
                @"
select  ShippingAPID
        , se.Blno
        , [Bl2no] = (select BL2No from GMTBooking where id=se.InvNo)
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
        , [NoExportCharges] = (select NoExportCharges from GMTBooking where id=se.InvNo)
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
                bool haveSea = false, noExistNotSea = true;
                DualResult resultCheckShareExpense;
                bool bolNoImportCharges = true;
                string exportID = string.Empty;
                DataTable duplicData;
                DBProxy.Current.Select(null, "select BLNo,WKNo,InvNo from ShareExpense WITH (NOLOCK) where 1=0", out duplicData);

                // 取得AccountNo
                string accNo = MyUtility.GetValue.Lookup(string.Format(
                    @"select se.AccountID 
                      from ShippingAP_Detail sd WITH (NOLOCK) , ShipExpense se WITH (NOLOCK) 
                      where sd.ID = '{0}' 
                      and sd.ShipExpenseID = se.ID 
                      and se.AccountID != ''
                      and not (dbo.GetAccountNoExpressType(se.AccountID,'Vat') = 1 or dbo.GetAccountNoExpressType(se.AccountID,'SisFty') = 1)",
                    MyUtility.Convert.GetString(this.apData["ID"])));

                List<CheckResult> listCheckResult = new List<CheckResult>();
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

                        if (dr["BLNo"].ToString().IndexOf("'") > -1 || dr["WKNo"].ToString().IndexOf("'") > -1 || dr["InvNo"].ToString().IndexOf("'") > -1)
                        {
                            MyUtility.Msg.WarningBox("Can not save! Because value includes ' word in <BL/MAWB No.>, <FCR/BL/HAWB> or <WK#/Fty WK#>.");
                            return;
                        }

                        // 檢查重複值
                        DataRow[] findrow = null;

                        findrow = duplicData.Select(string.Format(
                            @"WKNo = '{0}' and InvNo = '{1}'",
                            MyUtility.Check.Empty(dr["WKNo"].ToString()) ? "Empty" : dr["WKNo"].ToString(),
                            MyUtility.Check.Empty(dr["InvNo"].ToString()) ? "Empty" : dr["InvNo"].ToString()));

                        if (findrow.Length == 0)
                        {
                            duplicData.ImportRow(dr);
                            for (int i = 0; i < duplicData.Rows.Count; i++)
                            {
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

                        exportID = MyUtility.Convert.GetString(this.apData["Type"]) == "IMPORT" ? MyUtility.Convert.GetString(dr["WKno"]) : MyUtility.Convert.GetString(dr["InvNo"]);
                        string sqlPrepaidFtyImportFee = $@"select PrepaidFtyImportFee from Export where id = '{exportID}'";
                        decimal prepaidFtyImportFee = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup(sqlPrepaidFtyImportFee));
                        bolNoImportCharges = MyUtility.Convert.GetBool(MyUtility.GetValue.Lookup(string.Format("select NoImportCharges from Export where id = '{0}'", exportID)));
                        if (bolNoImportCharges && prepaidFtyImportFee == 0)
                        {
                            MyUtility.Msg.WarningBox("P03. Import Schedule - [No Import Charge] has been checked, please reconfirm.");
                            return;
                        }
                        else if (bolNoImportCharges && prepaidFtyImportFee != 0)
                        {
                            MyUtility.Msg.WarningBox("Shipping-TW already paid the import charge, please check with forwarder if they bill us repeatedly and inform Shipping-TW at the same time.");
                            return;
                        }

                        // ISP20191331 檢查ShareExpense項目正確性
                        resultCheckShareExpense = this.CheckShareExpenseItem(dr, ref listCheckResult);
                        if (!resultCheckShareExpense)
                        {
                            this.ShowErr(resultCheckShareExpense);
                            return;
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

                if (listCheckResult.Count > 0)
                {
                    Func<List<CheckResult>, string, string, bool> showResultMsg = (inListCheckResult, msgType, mainMsg) =>
                        {
                            var isGMTBookingFail = listCheckResult.Where(s => s.resultType == msgType).Any();
                            if (isGMTBookingFail)
                            {
                                string gmtBookingIDs = listCheckResult.Where(s => s.resultType == msgType).Select(s => s.resultValue).Distinct().JoinToString(",");
                                MyUtility.Msg.WarningBox(mainMsg + gmtBookingIDs);
                                return true;
                            }

                            return false;
                        };

                    if (showResultMsg(listCheckResult, "GB", "Garment Booking must confirm first." + Environment.NewLine + "GB :"))
                    {
                        return;
                    }

                    if (showResultMsg(listCheckResult, "PL", "Packing List must import to Pullout Report and send to TPE." + Environment.NewLine + "PL :"))
                    {
                        return;
                    }

                    string shipModeAirPP = Prgs.GetNeedCreateAppShipMode();
                    string msgAirPP = $@"Please maintain Air Pre-Paid first (and GM Team must already locked APP), if shipping mode is {shipModeAirPP}
Orders (Seq) :";
                    if (showResultMsg(listCheckResult, "AirPP", msgAirPP))
                    {
                        return;
                    }
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
        and s.WKNo = '{1}' 
        and s.InvNo = '{2}';",
                            MyUtility.Convert.GetString(this.apData["ID"]),
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
using (select '{0}', '{2}', '{3}') as s (ShippingAPID, WKNO, InvNo)
on	t.ShippingAPID = s.ShippingAPID 	
	and t.WKNO = s.WKNO
	and t.InvNo = s.InvNo
when matched then
	update set t.Junk = 0
    , ShipModeID = '{8}'
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


                SqlConnection sqlConn = null;
                DBProxy.Current.OpenConnection(null, out sqlConn);
                using (TransactionScope transactionScope = new TransactionScope())
                using (sqlConn)
                {
                    try
                    {
                        DualResult result, result1;
                        bool lastResult = true;
                        string errmsg = string.Empty;

                        if (deleteCmds.Count != 0)
                        {
                            result = DBProxy.Current.ExecutesByConn(sqlConn, deleteCmds);
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
                            result1 = DBProxy.Current.ExecutesByConn(sqlConn, addCmds);
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

                        result = DBProxy.Current.ExecuteByConn(
                            sqlConn,
                            string.Format("exec CalculateShareExpense '{0}','{1}'", MyUtility.Convert.GetString(this.apData["ID"]), Sci.Env.User.UserID));
                        if (!result)
                        {
                            errmsg = errmsg + "Calcute share expense failed." + "\r\n" + result.ToString();
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
                #endregion

                if (this.Apflag)
                {
                    DataTable tmp = (DataTable)this.listControlBindingSource1.DataSource;
                    DataTable dt;
                    string sqlGBchk;
                    DualResult dualResult;
                    string msgs = string.Empty;

                    sqlGBchk = string.Format(
                        @"
select IsFactory = iif(IsFactory = 1, 'True', 'False')
from LocalSupp
where ID = '{0}'
and Junk = 0",
                       this.LocalSuppID);

                    // 當AP-Supplier 如果是SCI的工廠 跳過此判斷
                    bool chkIsFactory = MyUtility.Convert.GetBool(MyUtility.GetValue.Lookup(sqlGBchk));
                    if (!chkIsFactory)
                    {
                        sqlGBchk = $@"
select distinct g.Forwarder,g.id
from GMTBooking g
inner join #tmp t on g.id=t.InvNo
";
                        dualResult = MyUtility.Tool.ProcessWithDatatable(tmp, string.Empty, sqlGBchk, out dt);
                        if (!dualResult)
                        {
                            this.ShowErr(dualResult);
                        }

                        List<string> listID = dt.AsEnumerable().Where(w => MyUtility.Convert.GetString(w["Forwarder"]) != this.LocalSuppID).Select(s => MyUtility.Convert.GetString(s["id"])).Distinct().ToList();
                        if (listID.Count > 0)
                        {
                            string gid = string.Join(",", listID);
                            msgs += $@"Garment Booking : {gid}
";
                        }

                        sqlGBchk = $@"
select distinct AirPP.Forwarder,p.id
from GMTBooking g with(nolock)
inner join #tmp t on g.id=t.InvNo
inner join PackingList p with(nolock) on p.INVNo = g.id
inner join PackingList_Detail pd with(nolock) on pd.id = p.id
inner join AirPP with(nolock) on AirPP.OrderID = pd.OrderID and AirPP.OrderShipmodeSeq = pd.OrderShipmodeSeq
";
                        dualResult = MyUtility.Tool.ProcessWithDatatable(tmp, string.Empty, sqlGBchk, out dt);
                        if (!dualResult)
                        {
                            this.ShowErr(dualResult);
                        }

                        List<string> packingListID = dt.AsEnumerable().Where(w => MyUtility.Convert.GetString(w["Forwarder"]) != this.LocalSuppID).Select(s => MyUtility.Convert.GetString(s["id"])).Distinct().ToList();
                        if (packingListID.Count > 0)
                        {
                            string pid = string.Join(",", packingListID);
                            msgs += $@"Packing List : {pid}";
                        }

                        if (!MyUtility.Check.Empty(msgs))
                        {
                            msgs = @"Forwarder is different from APP request, please double check.
" + msgs;
                            MyUtility.Msg.WarningBox(msgs);
                        }
                    }
                }

                this.EditMode = false;
            }

            this.ControlButton();
            this.QueryData();
        }

        private class CheckResult
        {
            public string resultType { get; set; }

            public string resultValue { get; set; }
        }

        private DualResult CheckShareExpenseItem(DataRow shareExpenseItem, ref List<CheckResult> listCheckResult)
        {
            if (MyUtility.Check.Empty(shareExpenseItem["InvNo"]))
            {
                return new DualResult(true);
            }

            string sqlCheckGarmentBookingAndPackingList = $@"
declare @Status varchar(15) = ''
declare @invNo varchar(25) = @inputInvNo

--檢查GarmentBooking是否confirm
select @Status = Status
    from GMTBooking with (nolock) where ID = @invNo

if(@Status not in ('','Confirmed'))
begin

    select [resultType] = 'GB', [resultValue] = @invNo 
    return
end

--檢查PackingList 必須存在 Pullout Report 並且有 Send to TPE 的日期
Create Table #tmpPackingListCheck(
[resultType] varchar(2),
[resultValue] varchar(13)
)

insert into #tmpPackingListCheck
select [resultType] = 'PL',
       [resultValue] = p.ID
from PackingList p with (nolock) 
where   p.InvNo = @invNo and 
        not exists (select 1 from Pullout pul with (nolock)
                                  inner join Pullout_Detail pd with (nolock) on pul.ID = pd.ID
                                  where pd.PackingListID = p.ID and pul.SendToTPE is not null)

if exists (select 1 from #tmpPackingListCheck)
begin
    select * from #tmpPackingListCheck
    return
end

--檢查PL ShipMode 屬於必須建立 APP（請至資料表 : ShipMode 確認是否需要建立 APP : NeedCreateAPP）
Create Table #tmpAirPPCheck(
[resultType] varchar(5),
[resultValue] varchar(800)
)

insert into #tmpAirPPCheck
select [resultType] = 'AirPP',
       [resultValue] = pd.OrderID + '(' + pd.OrderShipmodeSeq + ')'
from PackingList p with (nolock) 
inner join ShipMode sm with (nolock) on p.ShipModeID = sm.ID and sm.NeedCreateAPP = 1
inner join PackingList_Detail pd with (nolock) on p.ID = pd.ID
where p.InvNo = @invNo and  not exists (select 1 from AirPP app with (nolock) where app.OrderID = pd.OrderID and app.OrderShipmodeSeq = pd.OrderShipmodeSeq and Status = 'Locked')


if exists (select 1 from #tmpAirPPCheck)
begin
    select * from #tmpAirPPCheck
    return
end

select [resultType] = 'OK',
       [resultValue] = ''
";

            DataTable dtResult;
            DualResult result = DBProxy.Current.Select(null, sqlCheckGarmentBookingAndPackingList, new List<SqlParameter>() { new SqlParameter("@inputInvNo", shareExpenseItem["InvNo"]) }, out dtResult);
            if (!result)
            {
                return result;
            }

            bool isCheckNotPass = dtResult.Rows[0]["resultType"].ToString() != "OK";
            if (isCheckNotPass)
            {
                foreach (DataRow item in dtResult.Rows)
                {
                    listCheckResult.Add(new CheckResult()
                    {
                        resultType = item["resultType"].ToString(),
                        resultValue = item["resultValue"].ToString()
                    });
                }
            }

            return new DualResult(true);
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
            DualResult result = DBProxy.Current.Execute(
                "Production",
                string.Format("exec CalculateShareExpense '{0}','{1}'", MyUtility.Convert.GetString(this.apData["ID"]), Sci.Env.User.UserID));
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Re-Calculate Delete faile\r\n" + result.ToString());
                return;
            }

            this.QueryData();
        }

        // Append
        private void BtnAppend_Click(object sender, EventArgs e)
        {
            DataRow newRow = ((DataTable)this.listControlBindingSource1.DataSource).NewRow();
            newRow["Blno"] = string.Empty;
            newRow["Bl2no"] = string.Empty;
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
