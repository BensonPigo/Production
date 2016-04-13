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
    public partial class P40 : Sci.Win.Tems.Input6
    {
        Ict.Win.DataGridViewGeneratorTextColumnSettings nlcode = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
        Ict.Win.DataGridViewGeneratorNumericColumnSettings qty = new DataGridViewGeneratorNumericColumnSettings();
        Ict.Win.UI.DataGridViewTextBoxColumn col_nlcode;
        Ict.Win.UI.DataGridViewNumericBoxColumn col_qty;
        bool localPurchase = false;
        DataTable NoNLCode, NotInPO, UnitNotFound;

        public P40(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            //建立NoNLCode, NotInPO, UnitNotFound的結構
            string sqlCmd = "select '' as SCIRefno,'' as Refno,'' as BrandID,'' as Type,'' as Description,'' as NLCode,'' as HSCode,'' as CustomsUnit,0.0 as PcsWidth,0.0 as PcsLength,0.0 as PcsKg,0 as NoDeclare from VNImportDeclaration where 1=0";
            DBProxy.Current.Select(null, sqlCmd, out NoNLCode);
            sqlCmd = "select '' as ID,'' as POID,'' as Seq1,'' as Seq2,'' as Seq,'' as Description,'' as Type,'' as OriUnit,0.0 as OriImportQty,0.0 as Width,'' as NLCode,'' as HSCode,'' as CustomsUnit,0.0 as PcsWidth,0.0 as PcsLength,0.0 as PcsKg,0 as NoDeclare from VNImportDeclaration where 1=0";
            DBProxy.Current.Select(null, sqlCmd, out NotInPO);
            sqlCmd = "select '' as OriUnit,'' as CustomsUnit,'' as RefNo from VNImportDeclaration where 1=0";
            DBProxy.Current.Select(null, sqlCmd, out UnitNotFound);
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            if (EditMode)
            {
                if (MyUtility.Convert.GetString(CurrentMaintain["Status"]).ToUpper() == "CONFIRMED")
                {
                    textBox3.ReadOnly = false;
                    dateBox1.ReadOnly = true;
                    textBox1.ReadOnly = true;
                    textBox2.ReadOnly = true;
                    textBox4.ReadOnly = true;
                    txtshipmode1.ReadOnly = true;
                    txtcountry1.TextBox1.ReadOnly = true;
                    gridicon.Append.Enabled = false;
                    gridicon.Insert.Enabled = false;
                    gridicon.Remove.Enabled = false;
                    detailgrid.IsEditingReadOnly = true;
                }
                else
                {
                    textBox3.ReadOnly = true;
                    dateBox1.ReadOnly = false;
                    textBox1.ReadOnly = false;
                    txtshipmode1.ReadOnly = false;
                    txtcountry1.TextBox1.ReadOnly = false;
                    if (MyUtility.Convert.GetString(CurrentMaintain["IsSystemCalculate"]).ToUpper() == "TRUE")
                    {
                        gridicon.Append.Enabled = false;
                        gridicon.Insert.Enabled = false;
                        gridicon.Remove.Enabled = false;
                        col_nlcode.IsEditingReadOnly = true;
                        col_qty.IsEditingReadOnly = true;
                    }
                    else
                    {
                        gridicon.Append.Enabled = true;
                        gridicon.Insert.Enabled = true;
                        gridicon.Remove.Enabled = true;
                        col_nlcode.IsEditingReadOnly = false;
                        col_qty.IsEditingReadOnly = false;
                    }

                    if (MyUtility.Check.Empty(CurrentMaintain["BLNo"]) && MyUtility.Check.Empty(CurrentMaintain["WKNo"]))
                    {
                        textBox2.ReadOnly = false;
                        textBox4.ReadOnly = false;
                    }
                    else if (MyUtility.Check.Empty(CurrentMaintain["BLNo"]))
                    {
                        textBox2.ReadOnly = true;
                        textBox4.ReadOnly = false;
                    }
                    else
                    {
                        textBox2.ReadOnly = false;
                        textBox4.ReadOnly = true;
                    }
                }
                detailgrid.EnsureStyle();
            }
        }

        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : MyUtility.Convert.GetString(e.Master["ID"]);
            this.DetailSelectCommand = string.Format(@"select * from VNImportDeclaration_Detail where ID = '{0}' order by CONVERT(int,SUBSTRING(NLCode,3,3))", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        protected override void OnDetailGridSetup()
        {
            #region NL Code的Validating
            nlcode.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    if (!MyUtility.Check.Empty(e.FormattedValue))
                    {
                        if (MyUtility.Convert.GetString(dr["nlcode"]) != MyUtility.Convert.GetString(e.FormattedValue))
                        {
                            DataRow seekData;
                            if (!MyUtility.Check.Seek(string.Format("select HSCode,UnitID from VNContract_Detail where ID = '{0}' and NLCode = '{1}'",
                                MyUtility.Convert.GetString(CurrentMaintain["VNContractID"]), MyUtility.Convert.GetString(e.FormattedValue)), out seekData))
                            {
                                MyUtility.Msg.WarningBox("NL Code not found!!");
                                dr["HSCode"] = "";
                                dr["NLCode"] = "";
                                dr["Qty"] = 0;
                                dr["UnitID"] = "";
                                e.Cancel = true;
                                return;
                            }
                            else
                            {
                                dr["HSCode"] = seekData["HSCode"];
                                dr["NLCode"] = e.FormattedValue;
                                dr["UnitID"] = seekData["UnitID"];
                            }
                        }
                    }
                    else
                    {
                        dr["HSCode"] = "";
                        dr["NLCode"] = "";
                        dr["Qty"] = 0;
                        dr["UnitID"] = "";
                    }
                }
            };
            #endregion

            qty.CellMouseDoubleClick += (s, e) =>
                {
                    if (!EditMode)
                    {
                        if (e.Button == System.Windows.Forms.MouseButtons.Left)
                        {
                            DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                            Sci.Production.Shipping.P40_Detail callNextForm = new Sci.Production.Shipping.P40_Detail(CurrentMaintain, MyUtility.Convert.GetString(dr["NLCode"]));
                            DialogResult result = callNextForm.ShowDialog(this);
                            callNextForm.Dispose();
                        }
                    }
                };
            base.OnDetailGridSetup();
            Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("HSCode", header: "HS Code", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("NLCode", header: "NL Code", width: Widths.AnsiChars(7), settings: nlcode).Get(out col_nlcode)
                .Numeric("Qty", header: "Stock Qty", decimal_places: 3, width: Widths.AnsiChars(15), settings: qty).Get(out col_qty)
                .Text("UnitID", header: "Unit", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("Remark", header: "Remark", width: Widths.AnsiChars(30));
        }

        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["Status"] = "New";
            CurrentMaintain["CDate"] = DateTime.Today;
            CurrentMaintain["VNContractID"] = MyUtility.GetValue.Lookup("select top 1 ID from VNContract where StartDate <= GETDATE() and EndDate >= GETDATE() and Status = 'Confirmed'");
        }

        protected override bool ClickDeleteBefore()
        {
            if (MyUtility.Convert.GetString(CurrentMaintain["Status"]).ToUpper() == "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("This record already confirmed, can't delete!!");
                return false;
            }
            return base.ClickDeleteBefore();
        }

        protected override bool ClickSaveBefore()
        {
            #region 檢查必輸欄位
            if (MyUtility.Check.Empty(CurrentMaintain["CDate"]))
            {
                MyUtility.Msg.WarningBox("Date can't empty!!");
                dateBox1.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["VNContractID"]))
            {
                MyUtility.Msg.WarningBox("Contract no. can't empty!!");
                textBox1.Focus();
                return false;
            }
            #endregion

            #region 檢查BL No.與WK No.不可重複
            if (!MyUtility.Check.Empty(CurrentMaintain["BLNo"]))
            {
                if (MyUtility.Check.Seek(string.Format("select ID from VNImportDeclaration where ID <> '{0}' and BLNo = '{1}'", MyUtility.Convert.GetString(CurrentMaintain["ID"]), MyUtility.Convert.GetString(CurrentMaintain["BLNo"]))))
                {
                    MyUtility.Msg.WarningBox("This B/L No. already exist!!");
                    return false;
                }
            }
            if (!MyUtility.Check.Empty(CurrentMaintain["WKNo"]))
            {
                if (MyUtility.Check.Seek(string.Format("select ID from VNImportDeclaration where ID <> '{0}' and WKNo = '{1}'", MyUtility.Convert.GetString(CurrentMaintain["ID"]), MyUtility.Convert.GetString(CurrentMaintain["WKNo"]))))
                {
                    MyUtility.Msg.WarningBox("This WK No. already exist!!");
                    return false;
                }
            }

            #endregion

            #region 刪除表身NL Code與Qty為0的資料
            int recCount = 0;
            foreach (DataRow dr in DetailDatas)
            {
                if (MyUtility.Check.Empty(dr["NLCode"]) || MyUtility.Check.Empty(dr["Qty"]))
                {
                    dr.Delete();
                    continue;
                }
                recCount++;
            }
            if (recCount == 0)
            {
                MyUtility.Msg.WarningBox("Detail can't empty!!");
                return false;
            }
            #endregion

            //Get ID
            if (IsDetailInserting)
            {
                string newID = MyUtility.GetValue.GetID(Sci.Env.User.Keyword + "ID", "VNImportDeclaration", Convert.ToDateTime(CurrentMaintain["CDate"]), 2, "ID", null);
                if (MyUtility.Check.Empty(newID))
                {
                    MyUtility.Msg.WarningBox("GetID fail, please try again!");
                    return false;
                }
                CurrentMaintain["ID"] = newID;
            }

            return base.ClickSaveBefore();
        }

        //Confirm
        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            string updateCmds = string.Format("update VNImportDeclaration set EditDate = GETDATE(), EditName = '{0}', Status = 'Confirmed' where ID = '{1}'",
                Sci.Env.User.UserID, MyUtility.Convert.GetString(CurrentMaintain["ID"]));

            DualResult result = DBProxy.Current.Execute(null, updateCmds);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Confirm fail!!\r\n" + result.ToString());
                return;
            }
            RenewData();
            OnDetailEntered();
            EnsureToolbarExt();
        }

        //Unconfirm
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();

            string updateCmds = string.Format("update VNImportDeclaration set EditDate = GETDATE(), EditName = '{0}', Status = 'New' where ID = '{1}'",
                            Sci.Env.User.UserID, MyUtility.Convert.GetString(CurrentMaintain["ID"]));

            DualResult result = DBProxy.Current.Execute(null, updateCmds);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Unconfirm fail!!\r\n" + result.ToString());
                return;
            }

            RenewData();
            OnDetailEntered();
            EnsureToolbarExt();
        }

        protected override bool ClickPrint()
        {
            Sci.Production.Shipping.P40_Print callPurchaseForm = new Sci.Production.Shipping.P40_Print(CurrentMaintain);
            callPurchaseForm.ShowDialog(this);
            return base.ClickPrint();
        }

        //Contract No.
        private void textBox1_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sqlCmd = string.Format("select ID from VNContract where StartDate <= {0} and EndDate >= {0} and Status = 'Confirmed'", MyUtility.Check.Empty(CurrentMaintain["CDate"]) ? "GETDATE()" : "'" + Convert.ToDateTime(CurrentMaintain["CDate"]).ToString("d") + "'");
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlCmd, "8", this.Text, false, ",");
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel) { return; }
            textBox1.Text = item.GetSelectedString();
        }

        //Contract No.
        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            if (EditMode && !MyUtility.Check.Empty(textBox1.Text) && textBox1.Text != textBox1.OldValue)
            {
                if (MyUtility.Check.Seek(string.Format("select ID from VNContract where ID = '{0}'", textBox1.Text)))
                {
                    if (!MyUtility.Check.Seek(string.Format("select ID from VNContract where  ID = '{0}' and StartDate <= {1} and EndDate >= {1} and Status = 'Confirmed'", textBox1.Text, MyUtility.Check.Empty(CurrentMaintain["CDate"]) ? "GETDATE()" : "'" + Convert.ToDateTime(CurrentMaintain["CDate"]).ToString("d") + "'")))
                    {
                        MyUtility.Msg.WarningBox("This Contract can't use.");
                        textBox1.Text = "";
                        e.Cancel = true;
                        return;
                    }
                }
                else
                {
                    MyUtility.Msg.WarningBox("Contract no. not found!!");
                    textBox1.Text = "";
                    e.Cancel = true;
                    return;
                }
            }
        }

        //B/L No.
        private void textBox2_Validating(object sender, CancelEventArgs e)
        {
            if (EditMode)
            {
                if (textBox2.Text != textBox2.OldValue)
                {
                    foreach (DataRow dr in DetailDatas)
                    {
                        dr.Delete();
                    }
                    int isFtyExport = 0;
                    if (MyUtility.Check.Empty(textBox2.Text))
                    {
                        textBox4.ReadOnly = false;
                        CurrentMaintain["IsSystemCalculate"] = 0;
                        CurrentMaintain["BLNo"] = "";
                        CurrentMaintain["IsFtyExport"] = 0;
                        CurrentMaintain["IsLocalPO"] = 0;
                        CurrentMaintain["ShipModeID"] = "";
                        CurrentMaintain["FromSite"] = "";
                    }
                    else
                    {
                        DataRow export;
                        if (MyUtility.Check.Seek(string.Format("select ShipModeID,ExportCountry from Export where BLNo = '{0}'", textBox2.Text), out export))
                        {
                            isFtyExport = 0;
                            localPurchase = false;
                        }
                        else if (MyUtility.Check.Seek(string.Format("select Type,ShipModeID,ExportCountry from FtyExport where BLNo = '{0}'", textBox2.Text), out export))
                        {
                            isFtyExport = 1;
                            localPurchase = MyUtility.Convert.GetString(export["Type"]) == "4" ? true : false;
                        }
                        else
                        {
                            MyUtility.Msg.WarningBox("BL No. not found!!");
                            CurrentMaintain["IsFtyExport"] = isFtyExport;
                            CurrentMaintain["IsLocalPO"] = localPurchase ? 1 : 0;
                            CurrentMaintain["BLNo"] = "";
                            CurrentMaintain["ShipModeID"] = "";
                            CurrentMaintain["FromSite"] = "";
                            CurrentMaintain["IsSystemCalculate"] = 0;
                            e.Cancel = true;
                            return;
                        }
                        CurrentMaintain["BLNo"] = textBox2.Text;
                        CurrentMaintain["ShipModeID"] = export["ShipModeID"];
                        CurrentMaintain["FromSite"] = export["ExportCountry"];
                        CurrentMaintain["IsFtyExport"] = isFtyExport;
                        CurrentMaintain["IsLocalPO"] = localPurchase ? 1 : 0;
                        CurrentMaintain["IsSystemCalculate"] = 1;
                        textBox4.ReadOnly = true;
                    }
                }
            }
        }

        //B/L No.
        private void textBox2_Validated(object sender, EventArgs e)
        {
            if (EditMode && !MyUtility.Check.Empty(CurrentMaintain["BLNo"]))
            {
                if (!QueryNonNLData(string.Format("e.BLNo = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["BLNo"]))))
                {
                    return;
                }
                 if (NoNLCode.Rows.Count > 0 || NotInPO.Rows.Count > 0)
                    {
                        Sci.Production.Shipping.P40_AssignNLCode callNextForm = new Sci.Production.Shipping.P40_AssignNLCode(NoNLCode, NotInPO, UnitNotFound, CurrentMaintain);
                        DialogResult result = callNextForm.ShowDialog(this);
                        if (result == System.Windows.Forms.DialogResult.OK)
                        {
                            NotInPO = callNextForm.notInPo;
                            callNextForm.Dispose();
                            CalaulateData(string.Format("e.BLNo = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["BLNo"])));
                        }
                        else
                        {
                            callNextForm.Dispose();
                        }
                    }
                    else
                    {
                        CalaulateData(string.Format("e.BLNo = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["BLNo"])));
                    }
            }
        }

        //WK No.
        private void textBox4_Validating(object sender, CancelEventArgs e)
        {
            if (EditMode)
            {
                if (textBox4.Text != textBox4.OldValue)
                {
                    foreach (DataRow dr in DetailDatas)
                    {
                        dr.Delete();
                    }

                    if (MyUtility.Check.Empty(textBox4.Text))
                    {
                        textBox2.ReadOnly = false;
                        CurrentMaintain["IsSystemCalculate"] = 0;
                        CurrentMaintain["WKNo"] = "";
                        CurrentMaintain["IsFtyExport"] = 0;
                        CurrentMaintain["IsLocalPO"] = 0;
                        CurrentMaintain["ShipModeID"] = "";
                        CurrentMaintain["FromSite"] = "";
                    }
                    else
                    {
                        DataRow export;
                        if (MyUtility.Check.Seek(string.Format("select BLNo,ShipModeID,ExportCountry from Export where ID = '{0}'", textBox4.Text), out export))
                        {
                            if (!MyUtility.Check.Empty(export["BLNo"]))
                            {
                                CurrentMaintain["BLNo"] = export["BLNo"];
                                CurrentMaintain["WKNo"] = "";
                                textBox2.ReadOnly = false;
                                textBox4.ReadOnly = true;
                            }
                            else
                            {
                                textBox2.ReadOnly = true;
                                CurrentMaintain["WKNo"] = textBox4.Text;
                            }
                            CurrentMaintain["IsFtyExport"] = 0;
                            CurrentMaintain["IsLocalPO"] = 0;
                            CurrentMaintain["ShipModeID"] = export["ShipModeID"];
                            CurrentMaintain["FromSite"] = export["ExportCountry"];
                            CurrentMaintain["IsSystemCalculate"] = 1;
                            localPurchase = false;
                        }
                        else if (MyUtility.Check.Seek(string.Format("select * from FtyExport where ID = '{0}'", textBox4.Text), out export))
                        {
                            if (MyUtility.Convert.GetString(export["Type"]) == "1")
                            {
                                MyUtility.Msg.WarningBox("The Fty WK No. is < 3rd Country>!!");
                                CurrentMaintain["WKNo"] = "";
                                CurrentMaintain["ShipModeID"] = "";
                                CurrentMaintain["FromSite"] = "";
                                CurrentMaintain["IsSystemCalculate"] = 0;
                                CurrentMaintain["IsLocalPO"] = 0;
                                e.Cancel = true;
                                return;
                            }
                            if (MyUtility.Convert.GetString(export["Type"]) == "3")
                            {
                                MyUtility.Msg.WarningBox("The Fty WK No. is < Transfer Out>!!");
                                CurrentMaintain["WKNo"] = "";
                                CurrentMaintain["ShipModeID"] = "";
                                CurrentMaintain["FromSite"] = "";
                                CurrentMaintain["IsSystemCalculate"] = 0;
                                CurrentMaintain["IsLocalPO"] = 0;
                                e.Cancel = true;
                                return;
                            }
                            localPurchase = MyUtility.Convert.GetString(export["Type"]) == "4" ? true : false;
                            if (!MyUtility.Check.Empty(export["BLNo"]))
                            {
                                CurrentMaintain["BLNo"] = export["BLNo"];
                                CurrentMaintain["WKNo"] = "";
                                textBox2.ReadOnly = false;
                                textBox4.ReadOnly = true;
                            }
                            else
                            {
                                textBox2.ReadOnly = true;
                                CurrentMaintain["WKNo"] = textBox4.Text;
                            }
                            CurrentMaintain["IsFtyExport"] = 1;
                            CurrentMaintain["IsLocalPO"] = localPurchase ? 1 : 0;
                            CurrentMaintain["ShipModeID"] = export["ShipModeID"];
                            CurrentMaintain["FromSite"] = export["ExportCountry"];
                            CurrentMaintain["IsSystemCalculate"] = 1;
                        }
                        else
                        {
                            MyUtility.Msg.WarningBox("WK No. not found!!");
                            CurrentMaintain["IsFtyExport"] = 0;
                            CurrentMaintain["IsLocalPO"] = 0;
                            CurrentMaintain["WKNo"] = "";
                            CurrentMaintain["ShipModeID"] = "";
                            CurrentMaintain["FromSite"] = "";
                            CurrentMaintain["IsSystemCalculate"] = 0;
                            e.Cancel = true;
                            return;
                        }
                    }
                }
            }
        }

        //WK No.
        private void textBox4_Validated(object sender, EventArgs e)
        {
            if (EditMode)
            {
                if (!MyUtility.Check.Empty(CurrentMaintain["BLNo"]))
                {
                    if (!QueryNonNLData(string.Format("e.BLNo = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["BLNo"]))))
                    {
                        return;
                    }
                    if (NoNLCode.Rows.Count > 0 || NotInPO.Rows.Count > 0)
                    {
                        Sci.Production.Shipping.P40_AssignNLCode callNextForm = new Sci.Production.Shipping.P40_AssignNLCode(NoNLCode, NotInPO, UnitNotFound, CurrentMaintain);
                        DialogResult result = callNextForm.ShowDialog(this);
                        if (result == System.Windows.Forms.DialogResult.OK)
                        {
                            NotInPO = callNextForm.notInPo;
                            callNextForm.Dispose();
                            CalaulateData(string.Format("e.BLNo = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["BLNo"])));
                        }
                        else
                        {
                            callNextForm.Dispose();
                        }
                    }
                    else
                    {
                        CalaulateData(string.Format("e.BLNo = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["BLNo"])));
                    }
                }
                else if (!MyUtility.Check.Empty(CurrentMaintain["WKNo"]))
                {
                    if(!QueryNonNLData(string.Format("e.ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["WKNo"]))))
                    {
                        return;
                    }
                    if (NoNLCode.Rows.Count > 0 || NotInPO.Rows.Count > 0)
                    {
                        Sci.Production.Shipping.P40_AssignNLCode callNextForm = new Sci.Production.Shipping.P40_AssignNLCode(NoNLCode, NotInPO, UnitNotFound, CurrentMaintain);
                        DialogResult result = callNextForm.ShowDialog(this);
                        if (result == System.Windows.Forms.DialogResult.OK)
                        {
                            NotInPO = callNextForm.notInPo;
                            callNextForm.Dispose();
                            CalaulateData(string.Format("e.ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["WKNo"])));
                        }
                        else
                        {
                            callNextForm.Dispose();
                        }
                    }
                    else
                    {
                        CalaulateData(string.Format("e.ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["WKNo"])));
                    }
                }
            }
        }

        private bool QueryNonNLData(string sqlWhere)
        {
            //先將tmpe table清空
            NoNLCode.Clear();
            NotInPO.Clear();
            UnitNotFound.Clear();
            string sqlCmd;
            #region 組撈資料Sql
            if (MyUtility.Convert.GetString(CurrentMaintain["IsFtyExport"]).ToUpper() == "TRUE")
            {
                if (localPurchase)
                {
                    sqlCmd = string.Format(@"select e.ID,ed.POID,'' as Seq1, '' as Seq2,IIF(ed.UnitID = 'CONE',li.MeterToCone,1)*ed.Qty as OriImportQty,IIF(ed.UnitID = 'CONE','M',ed.UnitID) as OriUnit,
ed.RefNo as SCIRefno,ed.RefNo,'' as BrandID,ed.MtlTypeID as Type,li.Description,'' as DescDetail,
isnull(li.NLCode,'') as NLCode,isnull(li.HSCode,'') as HSCode,isnull(li.CustomsUnit,'') as CustomsUnit,
isnull(li.PcsLength,0.0) as PcsLength,isnull(li.PcsWidth,0.0) as PcsWidth,isnull(li.PcsKg,0.0) as PcsKg,
isnull(li.NoDeclare,0) as NoDeclare,ed.Price,
isnull((select Rate from Unit_Rate where UnitFrom = IIF(ed.UnitID = 'CONE','M',ed.UnitID) and UnitTo = li.CustomsUnit),'') as UnitRate,
isnull((select Rate from Unit_Rate where UnitFrom = IIF(ed.UnitID = 'CONE','M',ed.UnitID) and UnitTo = 'M'),'') as M2UnitRate,
'01' as POSeq
from FtyExport e
inner join FtyExport_Detail ed on e.ID = ed.ID
left join LocalItem li on li.RefNo = ed.RefNo and li.LocalSuppid = ed.SuppID
where {0}", sqlWhere);
                }
                else
                {
                    sqlCmd = string.Format(@"select e.ID,ed.PoID,ed.Seq1,ed.Seq2,ed.qty as OriImportQty,ed.UnitId as OriUnit,
isnull(f.SCIRefno,'') as SCIRefno,isnull(f.Refno,'') as Refno,isnull(f.BrandID,'') as BrandID,
isnull(f.Type,'') as Type,isnull(f.Description,'') as Description,isnull(f.DescDetail,'') as DescDetail,
isnull(f.NLCode,'') as NLCode,isnull(f.HSCode,'') as HSCode,isnull(f.CustomsUnit,'') as CustomsUnit,
isnull(f.PcsLength,0.0) as PcsLength,isnull(f.PcsWidth,0.0) as PcsWidth,isnull(f.PcsKg,0.0) as PcsKg, 
isnull(f.NoDeclare,0) as NoDeclare,ed.Price,
isnull((select Rate from Unit_Rate where UnitFrom = ed.UnitId and UnitTo = f.CustomsUnit),'') as UnitRate,
isnull((select Rate from Unit_Rate where UnitFrom = ed.UnitId and UnitTo = 'M'),'') as M2UnitRate,
isnull(psd.Seq1,'') as POSeq
from FtyExport e
inner join FtyExport_Detail ed on e.ID = ed.ID
left join PO_Supp_Detail psd on psd.ID = ed.PoID and psd.SEQ1 = ed.Seq1 and psd.SEQ2 = ed.Seq2
left join Fabric f on f.SCIRefno = psd.SCIRefno
where {0}", sqlWhere);
                }
            }
            else
            {
                sqlCmd = string.Format(@"select e.ID,ed.PoID,ed.Seq1,ed.Seq2,ed.qty+ed.Foc as OriImportQty,ed.UnitId as OriUnit,
isnull(f.SCIRefno,'') as SCIRefno,isnull(f.Refno,'') as Refno,isnull(f.BrandID,'') as BrandID,
isnull(f.Type,'') as Type,isnull(f.Description,'') as Description,isnull(f.DescDetail,'') as DescDetail,
isnull(f.NLCode,'') as NLCode,isnull(f.HSCode,'') as HSCode,isnull(f.CustomsUnit,'') as CustomsUnit,
isnull(f.PcsLength,0.0) as PcsLength,isnull(f.PcsWidth,0.0) as PcsWidth,isnull(f.PcsKg,0.0) as PcsKg, 
isnull(f.NoDeclare,0) as NoDeclare,ed.Price,
isnull((select Rate from Unit_Rate where UnitFrom = ed.UnitId and UnitTo = f.CustomsUnit),'') as UnitRate,
isnull((select Rate from Unit_Rate where UnitFrom = ed.UnitId and UnitTo = 'M'),'') as M2UnitRate,
isnull(psd.Seq1,'') as POSeq
from Export e
inner join Export_Detail ed on e.ID = ed.ID
left join PO_Supp_Detail psd on psd.ID = ed.PoID and psd.SEQ1 = ed.Seq1 and psd.SEQ2 = ed.Seq2
left join Fabric f on f.SCIRefno = psd.SCIRefno
where {0}", sqlWhere);
            }
                #endregion
            DataTable tmpData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out tmpData);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Query data fail!!\r\n"+result.ToString());
                return false;
            }
            
            foreach (DataRow dr in tmpData.Rows)
            {
                if (MyUtility.Check.Empty(dr["POSeq"]))
                {
                    DataRow newRow = NotInPO.NewRow();
                    newRow["ID"] = dr["ID"];
                    newRow["POID"] = dr["POID"];
                    newRow["Seq1"] = dr["Seq1"];
                    newRow["Seq2"] = dr["Seq2"];
                    newRow["Seq"] = MyUtility.Convert.GetString(dr["Seq1"]) + "-" + MyUtility.Convert.GetString(dr["Seq2"]);
                    newRow["Type"] = dr["Type"];
                    newRow["OriUnit"] = dr["OriUnit"];
                    newRow["OriImportQty"] = dr["OriImportQty"];
                    newRow["NLCode"] = dr["NLCode"];
                    newRow["HSCode"] = dr["HSCode"];
                    newRow["CustomsUnit"] = dr["CustomsUnit"];
                    newRow["PcsWidth"] = dr["PcsWidth"];
                    newRow["PcsLength"] = dr["PcsLength"];
                    newRow["PcsKg"] = dr["PcsKg"];
                    newRow["NoDeclare"] = dr["NoDeclare"];
                    newRow["Price"] = dr["Price"];
                    NotInPO.Rows.Add(newRow);
                }
                else
                {
                    if (MyUtility.Check.Empty(dr["NLCode"]))
                    {
                        DataRow[] findrow = NoNLCode.Select(string.Format("SCIRefno = '{0}'", MyUtility.Convert.GetString(dr["SCIRefno"])));
                        if (findrow.Length == 0)
                        {
                            DataRow newRow = NoNLCode.NewRow();
                            newRow["SCIRefno"] = dr["SCIRefno"];
                            newRow["Refno"] = dr["Refno"];
                            newRow["BrandID"] = dr["BrandID"];
                            newRow["Type"] = dr["Type"];
                            newRow["Description"] = dr["Description"];
                            newRow["NLCode"] = dr["NLCode"];
                            newRow["HSCode"] = dr["HSCode"];
                            newRow["CustomsUnit"] = dr["CustomsUnit"];
                            newRow["PcsWidth"] = dr["PcsWidth"];
                            newRow["PcsLength"] = dr["PcsLength"];
                            newRow["PcsKg"] = dr["PcsKg"];
                            newRow["NoDeclare"] = dr["NoDeclare"];
                            NoNLCode.Rows.Add(newRow);
                        }
                    }
                }
                if ((MyUtility.Convert.GetString(dr["Type"]) == "F" && MyUtility.Convert.GetString(dr["OriUnit"]) != MyUtility.Convert.GetString(dr["CustomsUnit"]) && MyUtility.Convert.GetString(dr["CustomsUnit"]).ToUpper() == "M2" && MyUtility.Check.Empty(dr["M2UnitRate"])) ||
                    (MyUtility.Convert.GetString(dr["Type"]) == "F" && MyUtility.Convert.GetString(dr["OriUnit"]) != MyUtility.Convert.GetString(dr["CustomsUnit"]) && MyUtility.Convert.GetString(dr["CustomsUnit"]).ToUpper() != "M2" && MyUtility.Check.Empty(dr["UnitRate"])) ||
                    (MyUtility.Convert.GetString(dr["Type"]) != "F" && MyUtility.Convert.GetString(dr["OriUnit"]) != MyUtility.Convert.GetString(dr["CustomsUnit"]) && MyUtility.Convert.GetString(dr["CustomsUnit"]).ToUpper() == "M2" && MyUtility.Check.Empty(dr["M2UnitRate"])) ||
                    (MyUtility.Convert.GetString(dr["Type"]) != "F" && MyUtility.Convert.GetString(dr["OriUnit"]) != MyUtility.Convert.GetString(dr["CustomsUnit"]) && MyUtility.Convert.GetString(dr["CustomsUnit"]).ToUpper() == "M" && MyUtility.Check.Empty(dr["UnitRate"])) ||
                    (MyUtility.Convert.GetString(dr["Type"]) != "F" && MyUtility.Convert.GetString(dr["OriUnit"]) != MyUtility.Convert.GetString(dr["CustomsUnit"]) && MyUtility.Convert.GetString(dr["CustomsUnit"]).ToUpper() != "M2" && MyUtility.Convert.GetString(dr["CustomsUnit"]).ToUpper() != "M" && MyUtility.Check.Empty(dr["UnitRate"])))
                {
                    DataRow[] findrow = UnitNotFound.Select(string.Format("OriUnit = '{0}' and CustomsUnit = '{1}'", MyUtility.Convert.GetString(dr["OriUnit"]), MyUtility.Convert.GetString(dr["CustomsUnit"])));
                    if (findrow.Length == 0)
                    {
                        DataRow newRow = UnitNotFound.NewRow();
                        newRow["OriUnit"] = dr["OriUnit"];
                        newRow["CustomsUnit"] = dr["CustomsUnit"];
                        newRow["Refno"] = dr["Refno"];
                        UnitNotFound.Rows.Add(newRow);
                    }
                    else
                    {
                        string refno = MyUtility.Convert.GetString(findrow[0]["Refno"]);
                        if (refno.IndexOf(MyUtility.Convert.GetString(dr["Refno"])) == -1)
                        {
                            findrow[0]["Refno"] = refno + ',' + MyUtility.Convert.GetString(dr["Refno"]);
                        }
                    }
                }
            }
            return true;
        }

        private void CalaulateData(string sqlWhere)
        {
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(@"with ExportDetail
as (
");
            #region 組撈資料Sql
            if (MyUtility.Convert.GetString(CurrentMaintain["IsFtyExport"]).ToUpper() == "TRUE")
            {
                if (localPurchase)
                {
                    sqlCmd.Append(string.Format(@"select e.ID,ed.POID,'' as Seq1, '' as Seq2,IIF(ed.UnitID = 'CONE',li.MeterToCone,1)*ed.Qty as OriImportQty,IIF(ed.UnitID = 'CONE','M',ed.UnitID) as OriUnit,
ed.MtlTypeID as Type,IIF(ed.UnitID = 'CONE',ed.Price/li.MeterToCone,ed.Price) as Price,
isnull(li.NLCode,'') as NLCode,isnull(li.HSCode,'') as HSCode,isnull(li.CustomsUnit,'') as CustomsUnit,
isnull(li.PcsLength,0.0) as PcsLength,isnull(li.PcsWidth,0.0) as PcsWidth,isnull(li.PcsKg,0.0) as PcsKg,
isnull(li.NoDeclare,0) as NoDeclare,0.0 as Width,
isnull((select RateValue from dbo.View_Unitrate where FROM_U = IIF(ed.UnitID = 'CONE','M',ed.UnitID) and TO_U = li.CustomsUnit),1) as RateValue,
(select RateValue from dbo.View_Unitrate where FROM_U = IIF(ed.UnitID = 'CONE','M',ed.UnitID) and TO_U = 'M') as M2RateValue,
isnull((select Rate from Unit_Rate where UnitFrom = IIF(ed.UnitID = 'CONE','M',ed.UnitID) and UnitTo = li.CustomsUnit),'') as UnitRate,
isnull((select Rate from Unit_Rate where UnitFrom = IIF(ed.UnitID = 'CONE','M',ed.UnitID) and UnitTo = 'M'),'') as M2UnitRate
from FtyExport e
inner join FtyExport_Detail ed on e.ID = ed.ID
left join LocalItem li on li.RefNo = ed.RefNo and li.LocalSuppid = ed.SuppID
where {0}", sqlWhere));
                }
                else
                {
                    sqlCmd.Append(string.Format(@"select e.ID,ed.PoID,ed.Seq1,ed.Seq2,ed.qty as OriImportQty,ed.UnitId as OriUnit,
isnull(f.Type,'') as Type,ed.Price,
isnull(f.NLCode,'') as NLCode,isnull(f.HSCode,'') as HSCode,isnull(f.CustomsUnit,'') as CustomsUnit,
isnull(f.PcsLength,0.0) as PcsLength,isnull(f.PcsWidth,0.0) as PcsWidth,isnull(f.PcsKg,0.0) as PcsKg, 
isnull(f.NoDeclare,0) as NoDeclare,isnull(f.Width,0) as Width,
isnull((select RateValue from dbo.View_Unitrate where FROM_U = ed.UnitId and TO_U = f.CustomsUnit),1) as RateValue,
(select RateValue from dbo.View_Unitrate where FROM_U = ed.UnitId and TO_U = 'M') as M2RateValue,
isnull((select Rate from Unit_Rate where UnitFrom = ed.UnitId and UnitTo = f.CustomsUnit),'') as UnitRate,
isnull((select Rate from Unit_Rate where UnitFrom = ed.UnitId and UnitTo = 'M'),'') as M2UnitRate
from FtyExport e
inner join FtyExport_Detail ed on e.ID = ed.ID
left join PO_Supp_Detail psd on psd.ID = ed.PoID and psd.SEQ1 = ed.Seq1 and psd.SEQ2 = ed.Seq2
left join Fabric f on f.SCIRefno = psd.SCIRefno
where {0}", sqlWhere));
                }
            }
            else
            {
                sqlCmd.Append(string.Format(@"select e.ID,ed.PoID,ed.Seq1,ed.Seq2,ed.qty+ed.Foc as OriImportQty,ed.UnitId as OriUnit,
isnull(f.Type,'') as Type,ed.Price,
isnull(f.NLCode,'') as NLCode,isnull(f.HSCode,'') as HSCode,isnull(f.CustomsUnit,'') as CustomsUnit,
isnull(f.PcsLength,0.0) as PcsLength,isnull(f.PcsWidth,0.0) as PcsWidth,isnull(f.PcsKg,0.0) as PcsKg, 
isnull(f.NoDeclare,0) as NoDeclare,isnull(f.Width,0) as Width,
isnull((select RateValue from dbo.View_Unitrate where FROM_U = ed.UnitId and TO_U = f.CustomsUnit),1) as RateValue,
(select RateValue from dbo.View_Unitrate where FROM_U = ed.UnitId and TO_U = 'M') as M2RateValue,
isnull((select Rate from Unit_Rate where UnitFrom = ed.UnitId and UnitTo = f.CustomsUnit),'') as UnitRate,
isnull((select Rate from Unit_Rate where UnitFrom = ed.UnitId and UnitTo = 'M'),'') as M2UnitRate
from Export e
inner join Export_Detail ed on e.ID = ed.ID
left join PO_Supp_Detail psd on psd.ID = ed.PoID and psd.SEQ1 = ed.Seq1 and psd.SEQ2 = ed.Seq2
left join Fabric f on f.SCIRefno = psd.SCIRefno
where {0}", sqlWhere));
            }
            #endregion
            sqlCmd.Append(@"
)
select NLCode,HSCode,CustomsUnit,sum(NewQty) as NewQty, sum(NewQty*Price) as Price from (
select *,[dbo].getVNUnitTransfer(Type,OriUnit,CustomsUnit,OriImportQty,Width,PcsWidth,PcsLength,PcsKg,IIF(CustomsUnit = 'M2',M2RateValue,RateValue),IIF(CustomsUnit = 'M2',M2UnitRate,UnitRate)) as NewQty
from ExportDetail
where NoDeclare = 0) a
group by NLCode,HSCode,CustomsUnit");

            DataTable selectedData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out selectedData);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Calculate fail!!\r\n"+result.ToString());
                return;
            }

            // 將Not in po的資料加入
            DataTable GroupNoInPOData;
            if (NotInPO.Rows.Count > 0)
            {
                try
                {
                    MyUtility.Tool.ProcessWithDatatable(NotInPO, @"NLCode,HSCode,CustomsUnit,Type,OriUnit,OriImportQty,Width,PcsWidth,PcsLength,PcsKg,NoDeclare,Price",
                        @"select NLCode,HSCode,CustomsUnit,sum(NewQty) as NewQty, sum(Price*NewQty) as Price from (
select *,[dbo].getVNUnitTransfer(Type,OriUnit,CustomsUnit,OriImportQty,Width,PcsWidth,PcsLength,PcsKg,IIF(CustomsUnit = 'M2',M2RateValue,RateValue),IIF(CustomsUnit = 'M2',M2UnitRate,UnitRate)) as NewQty
from #tmp
where NoDeclare = 0) a
group by NLCode,HSCode,CustomsUnit", out GroupNoInPOData);
                }
                catch (Exception ex)
                {
                    MyUtility.Msg.ErrorBox("Calculate Not in PO Data fail!!\r\n" + ex.ToString());
                    return;
                }

                foreach (DataRow dr in GroupNoInPOData.Rows)
                {
                    DataRow[] findrow = selectedData.Select(string.Format("NLCode = '{0}'", MyUtility.Convert.GetString(dr["NLCode"])));
                    if (findrow.Length == 0)
                    {
                        DataRow newRow = selectedData.NewRow();
                        newRow["NLCode"] = dr["NLCode"];
                        newRow["HSCode"] = dr["HSCode"];
                        newRow["CustomsUnit"] = dr["CustomsUnit"];
                        newRow["NewQty"] = dr["NewQty"];
                        newRow["Price"] = dr["Price"];
                        selectedData.Rows.Add(newRow);
                    }
                    else
                    {
                        findrow[0]["NewQty"] = MyUtility.Convert.GetDecimal(findrow[0]["NewQty"]) + MyUtility.Convert.GetDecimal(dr["NewQty"]);
                        findrow[0]["Price"] = MyUtility.Convert.GetDecimal(findrow[0]["Price"]) + MyUtility.Convert.GetDecimal(dr["Price"]);
                    }
                }
            }

            //將資料做排序
            try
            {
                MyUtility.Tool.ProcessWithDatatable(selectedData, @"NLCode,HSCode,CustomsUnit,NewQty,Price",
                    @"select NLCode,HSCode,CustomsUnit,NewQty,Price/NewQty as Price
from #tmp
order by CONVERT(int,SUBSTRING(NLCode,3,3))", out GroupNoInPOData);
            }
            catch (Exception ex)
            {
                MyUtility.Msg.ErrorBox("Calculate Not in PO Data fail!!\r\n" + ex.ToString());
                return;
            }

            //刪除表身Grid資料
            foreach (DataRow dr in DetailDatas)
            {
                dr.Delete();
            }

            //將資料填入表身Grid中
            foreach (DataRow dr in GroupNoInPOData.Rows)
            {
                DataRow  newRow = ((DataTable)detailgridbs.DataSource).NewRow();
                newRow["ID"] = "";
                newRow["NLCode"] = dr["NLCode"];
                newRow["HSCode"] = dr["HSCode"];
                newRow["Qty"] = dr["NewQty"];
                newRow["UnitID"] = dr["CustomsUnit"];
                newRow["Price"] = dr["Price"];
                newRow["Remark"] = "";
                ((DataTable)detailgridbs.DataSource).Rows.Add(newRow);
            }

            if (QueryNonNLData(sqlWhere))
            {
                StringBuilder wrongData = new StringBuilder();
                if (NoNLCode.Rows.Count > 0)
                {
                    wrongData.Append("Below data is no NL Code in B40, B41:\r\n");
                    foreach (DataRow dr in NoNLCode.Rows)
                    {
                        wrongData.Append(string.Format("RefNo: {0}, Brand: {1}\r\n", MyUtility.Convert.GetString(dr["RefNo"]), MyUtility.Convert.GetString(dr["BrandID"])));
                    }
                }
                if (UnitNotFound.Rows.Count > 0)
                {
                    wrongData.Append("Below data is no transfer formula. Please contact with Taipei MIS.\r\n");
                    foreach (DataRow dr in UnitNotFound.Rows)
                    {
                        wrongData.Append(string.Format("Unit: {0} Transfer to Unit: {1} RefNo:{2}\r\n", MyUtility.Convert.GetString(dr["OriUnit"]), MyUtility.Convert.GetString(dr["CustomsUnit"]), MyUtility.Convert.GetString(dr["RefNo"])));
                    }
                }
                if (wrongData.Length > 0)
                {
                    MyUtility.Msg.WarningBox(wrongData.ToString());
                }
            }
        }
    }
}
