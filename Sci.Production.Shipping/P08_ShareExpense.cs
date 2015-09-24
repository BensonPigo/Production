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
            displayBox1.Value = apData["CurrencyID"].ToString();
            numericBox1.DecimalPlaces = Convert.ToInt32(MyUtility.GetValue.Lookup("Exact", apData["CurrencyID"].ToString(), "Currency", "ID"));
            numericBox1.Value = Convert.ToDecimal(apData["Amount"]);
            ControlButton();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.grid1.IsEditingReadOnly = true;
            grid1.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.grid1)
                .Text("BLNo", header: "B/L No.", width: Widths.AnsiChars(13))
                .Text(apData["Type"].ToString() == "IMPORT" ? "WKNo" : "InvNo", header: apData["Type"].ToString() == "IMPORT" ? "WK#/Fty WK#" : "GB#/Fty WK#/Packing#", width: Widths.AnsiChars(18))
                .Text("ShipModeID", header: "Shipping Mode", width: Widths.AnsiChars(5))
                .Numeric("GW", header: "G.W.", decimal_places: 2)
                .Numeric("CBM", header: "CBM", decimal_places: 2)
                .Numeric("Amount", header: "Total Amount", decimal_places: 2);
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
                        string filter = string.Format("BLNo = '{0}' and WKNo = '{1}' and InvNo = '{2}'", dr["BLNo"].ToString(), dr["WKNo"].ToString(), dr["InvNo"].ToString());
                        SEData.DefaultView.RowFilter = filter;
                    }
                }
            };

            this.grid2.IsEditingReadOnly = true;
            grid2.DataSource = listControlBindingSource2;
            Helper.Controls.Grid.Generator(this.grid2)
                .Text("AccountNo", header: "Account No", width: Widths.AnsiChars(8))
                .Text("AccountName", header: "Account Name", width: Widths.AnsiChars(30))
                .Numeric("Amount", header: "Amount", decimal_places: 2)
                .Text("ShareRule", header: "Share by", width: Widths.AnsiChars(19));
            QueryData();
        }

        private void QueryData()
        {
            DataRow queryData;
            string sqlCmd = string.Format("select isnull(sum(GW),0) as GW,isnull(sum(CBM),0) as CBM,isnull(Count(BLNo),0) as RecCount from (select distinct BLNo,WKNo,InvNo,GW,CBM from ShareExpense where ShippingAPID = '{0}') a", apData["ID"].ToString());
            MyUtility.Check.Seek(sqlCmd, out queryData);
            numericBox2.Value = Convert.ToDecimal(queryData["GW"]);
            numericBox3.Value = Convert.ToDecimal(queryData["CBM"]);
            numericBox4.Value = Convert.ToInt32(queryData["RecCount"]);

            sqlCmd = string.Format("select *,case when ShareBase = 'G' then 'G.W.' when ShareBase = 'C' then 'CBM' else ' Number od Deliver Sheets' end as ShareRule from ShareExpense where ShippingAPID = '{0}' order by AccountNo", apData["ID"].ToString());
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out SEData);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query Detail fail.\r\n" + result.ToString());
                return;
            }
            listControlBindingSource2.DataSource = SEData;

            sqlCmd = string.Format("select ShippingAPID,BLNo,WKNo,InvNo,Type,ShipModeID,GW,CBM,CurrencyID,ShipModeID,FtyWK,isnull(sum(Amount),0) as Amount from ShareExpense where ShippingAPID = '{0}' group by ShippingAPID,BLNo,WKNo,InvNo,Type,ShipModeID,GW,CBM,CurrencyID,ShipModeID,FtyWK", apData["ID"].ToString());
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
            }
        }

        //Import
        private void button4_Click(object sender, EventArgs e)
        {
            if (apData["SubType"].ToString() == "OTHER")
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
                if (apData["SubType"].ToString() == "GARMENT")
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
                this.grid1.Rows.Remove(row);
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
                bool forwarderFee = MyUtility.Check.Seek(string.Format("select se.AccountNo from ShippingAP_Detail sd, ShipExpense se where sd.ID = '{0}' and sd.ShipExpenseID = se.ID and (se.AccountNo = '61022001' or se.AccountNo = '61012001')", apData["ID"].ToString()));
                bool haveSea = false, noExistNotSea = true;
                DataTable duplicData;
                DBProxy.Current.Select(null, "select BLNo,WKNo,InvNo from ShareExpense where 1=0", out duplicData);
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
                        DataRow[] findrow = duplicData.Select(string.Format("BLNo = '{0}' and WKNo = '{1}' and InvNo = '{2}'", dr["BLNo"].ToString(), dr["WKNo"].ToString(), dr["InvNo"].ToString()));
                        if (findrow.Length == 0)
                        {
                            duplicData.ImportRow(dr);
                        }
                        else
                        {
                            if (apData["Type"].ToString() == "IMPORT")
                            {
                                msg.Append(string.Format("{0}\r\n", dr["WKno"].ToString()));
                            }
                            else
                            {
                                msg.Append(string.Format("{0}\r\n", dr["InvNo"].ToString()));
                            }
                        }

                        //當有Forwarder費用且SubType不是Material時，要檢查如果有一筆Ship Mode為SEA時，全部的Ship Mode就都要為SEA
                        if (forwarderFee && (apData["Type"].ToString() == "EXPORT" || apData["SubType"].ToString() == "SISTER FACTORY TRANSFER"))
                        {
                            haveSea = haveSea || dr["ShipModeID"].ToString() == "SEA";
                            noExistNotSea = noExistNotSea && dr["ShipModeID"].ToString() == "SEA";
                        }
                    }
                }

                if (msg.ToString() != "")
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

                string accNo = MyUtility.GetValue.Lookup(string.Format("select se.AccountNo from ShippingAP_Detail sd, ShipExpense se where sd.ID = '{0}' and sd.ShipExpenseID = se.ID and se.AccountNo != ''", apData["ID"].ToString()));
                //刪除實體資料
                foreach (DataRow dr in ((DataTable)listControlBindingSource1.DataSource).Rows)
                {
                    if (dr.RowState == DataRowState.Deleted)
                    {
                        deleteCmds.Add(string.Format("delete ShareExpense where ShippingAPID = '{0}' and BLNo = '{1}' and WKNo = '{2}' and InvNo = '{3}';",apData["ID"].ToString(),dr["BLNo", DataRowVersion.Original].ToString().Trim(),dr["WKNo", DataRowVersion.Original].ToString().Trim(),dr["InvNo", DataRowVersion.Original].ToString().Trim()));
                        
                    }
                }
                //新增實體資料
                foreach (DataRow dr in SEGroupData.Rows)
                {
                    if (dr.RowState == DataRowState.Added)
                    {
                        addCmds.Add(string.Format(@"insert into ShareExpense (ShippingAPID,BLNo,WKNo,InvNo,Type,GW,CBM,CurrencyID,ShipModeID,FtyWK,AccountNo)
 values ('{0}','{1}','{2}','{3}','{4}',{5},{6},'{7}','{8}',{9},'{10}');", apData["ID"].ToString(), dr["BLNo"].ToString(), dr["WKNo"].ToString(), dr["InvNo"].ToString(),
                                                                 apData["SubType"].ToString(), dr["GW"].ToString(), dr["CBM"].ToString(), apData["CurrencyID"].ToString(), dr["ShipModeID"].ToString(),
                                                                 dr["FtyWK"].ToString() == "True"?"1":"0", accNo));
                    }
                    if (dr.RowState == DataRowState.Modified)
                    {
                        addCmds.Add(string.Format(@"update ShareExpense set ShipModeID = '{0}', GW = {1}, CBM = {2} where ShippingAPID = '{3}' and BLNo = '{4}' and WKNo = '{5}' and InvNo = '{6}';",
                                                                 dr["ShipModeID"].ToString(),dr["GW"].ToString(),dr["CBM"].ToString(),apData["ID"].ToString(), dr["BLNo"].ToString(), dr["WKNo"].ToString(), dr["InvNo"].ToString()));
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
                                    errmsg = errmsg + result1.ToString();
                                }
                            }

                            if (lastResult)
                            {
                                transactionScope.Complete();
                            }
                            else
                            {
                                MyUtility.Msg.WarningBox("Confirm failed, Pleaes re-try\r\n" + errmsg);
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            ShowErr("Commit transaction error.", ex);
                            return;
                        }
                    }
                }
                #endregion 

                bool returnValue = Prgs.CalculateShareExpense(apData["ID"].ToString());
                if (!returnValue)
                {
                    MyUtility.Msg.WarningBox("Calcute share expense failed, please retry 'Re-Calculate' later.");
                }
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
            if (apData["Type"].ToString() == "IMPORT")
            {
                string deleteCmd = string.Format("delete ShareExpense where ShippingAPID = '{0}' and WKNo != '' and WKNo not in (select ID from Export where ID = WKNo and ID is not null)",apData["ID"].ToString());
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
	from Export e, ShareExpense se, ShippingAP s
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
CLOSE cursor_allExport", apData["ID"].ToString());
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
                string deleteCmd = string.Format("delete ShareExpense where ShippingAPID = '{0}' and InvNo != '' and (InvNo not in (select ID from GMTBooking where ID = InvNo) and InvNo not in (select INVNo from PackingList where INVNo = ShareExpense.InvNo) and InvNo not in (select ID from FtyExport  where ID = InvNo))", apData["ID"].ToString());
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
	from GMTBooking g, ShippingAP s, ShareExpense se
	where g.ID = se.InvNo
	and s.id = se.ShippingAPID
	and se.FtyWK = 0
	and s.id = @apid

DECLARE cursor_FtyWK CURSOR FOR
	select f.ID,f.ShipModeID,f.WeightKg,f.Cbm,s.CurrencyID,s.SubType, f.Blno
	from FtyExport f, ShippingAP s, ShareExpense se
	where f.ID = se.InvNo
	and s.id = se.ShippingAPID
	and se.FtyWK = 1
	and s.id = @apid

DECLARE cursor_PackingList CURSOR FOR
	select p.ID,p.ShipModeID,p.GW,p.CBM,s.CurrencyID,s.SubType, '' as BLNo
	from PackingList p, ShippingAP s, ShareExpense se
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
CLOSE cursor_PackingList",apData["ID"].ToString());
                #endregion
                result = DBProxy.Current.Execute(null, updateCmd);
                if (!result)
                {
                    MyUtility.Msg.ErrorBox("Re-Calculate update faile\r\n" + result.ToString());
                    return;
                }

            }
            bool returnValue = Prgs.CalculateShareExpense(apData["ID"].ToString());
            if (!returnValue)
            {
                MyUtility.Msg.WarningBox("Re-calcute share expense failed, please retry later.");
            }

            QueryData();
        }
    }
}
