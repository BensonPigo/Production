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
            string sqlCmd = "select '' as SCIRefno,'' as Refno,'' as BrandID,'' as Type,'' as Description,'' as NLCode,'' as HSCode,'' as CustomsUnit,0.0 as PcsWidth,0.0 as PcsLength,0.0 as PcsKg,0 as NoDeclare from VNImportDeclaration WITH (NOLOCK) where 1=0";
            DBProxy.Current.Select(null, sqlCmd, out NoNLCode);
            sqlCmd = "select '' as ID,'' as POID,'' as Seq1,'' as Seq2,'' as Seq,'' as Description,'' as Type,'' as OriUnit,0.0 as OriImportQty,0.0 as Width,'' as NLCode,'' as HSCode,'' as CustomsUnit,0.0 as PcsWidth,0.0 as PcsLength,0.0 as PcsKg,0 as NoDeclare,0.0000 as Price  from VNImportDeclaration WITH (NOLOCK) where 1=0";
            DBProxy.Current.Select(null, sqlCmd, out NotInPO);
            sqlCmd = "select '' as OriUnit,'' as CustomsUnit,'' as RefNo from VNImportDeclaration WITH (NOLOCK) where 1=0";
            DBProxy.Current.Select(null, sqlCmd, out UnitNotFound);
           
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();              
            if (EditMode)
            {
                if (MyUtility.Convert.GetString(CurrentMaintain["Status"]).ToUpper() == "CONFIRMED")
                {
                    txtCustomdeclareno.ReadOnly = false;
                    dateDate.ReadOnly = true;
                    txtContractNo.ReadOnly = true;
                    txtBLNO.ReadOnly = true;
                    txtWKNo.ReadOnly = true;
                    txtshipmodeShipby.ReadOnly = true;
                    txtcountryCountryfrom.TextBox1.ReadOnly = true;
                    gridicon.Append.Enabled = false;
                    gridicon.Insert.Enabled = false;
                    gridicon.Remove.Enabled = false;
                    detailgrid.IsEditingReadOnly = true;
                }
                else
                {
                    txtCustomdeclareno.ReadOnly = true;
                    dateDate.ReadOnly = false;
                    txtContractNo.ReadOnly = false;
                    txtshipmodeShipby.ReadOnly = false;
                    txtcountryCountryfrom.TextBox1.ReadOnly = false;
                    
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
                        txtBLNO.ReadOnly = false;
                        txtWKNo.ReadOnly = false;
                    }
                    else if (MyUtility.Check.Empty(CurrentMaintain["BLNo"]))
                    {
                        txtBLNO.ReadOnly = true;
                        txtWKNo.ReadOnly = false;
                    }
                    else
                    {
                        txtBLNO.ReadOnly = false;
                        txtWKNo.ReadOnly = true;
                    }
                }
                detailgrid.EnsureStyle();                
            }
            
        }

        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : MyUtility.Convert.GetString(e.Master["ID"]);
            this.DetailSelectCommand = string.Format(@"select v.id,v.HSCode,v.NLCode,Qty = Round(v.Qty,2),v.UnitID,v.Remark,v.Price from VNImportDeclaration_Detail v WITH (NOLOCK) where ID = '{0}' order by CONVERT(int,SUBSTRING(NLCode,3,3))", masterID);
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
                            if (!MyUtility.Check.Seek(string.Format("select HSCode,UnitID from VNContract_Detail WITH (NOLOCK) where ID = '{0}' and NLCode = '{1}'",
                                MyUtility.Convert.GetString(CurrentMaintain["VNContractID"]), MyUtility.Convert.GetString(e.FormattedValue)), out seekData))
                            {
                                dr["HSCode"] = "";
                                dr["NLCode"] = "";
                                dr["Qty"] = 0;
                                dr["UnitID"] = "";
                                e.Cancel = true;
                                MyUtility.Msg.WarningBox("Customs Code not found!!");
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
                    .Text("NLCode", header: "Customs Code", width: Widths.AnsiChars(7), settings: nlcode).Get(out col_nlcode)
                    .Numeric("Qty", header: "Stock Qty", decimal_places: 2, width: Widths.AnsiChars(15), settings: qty).Get(out col_qty)
                    .Text("UnitID", header: "Unit", width: Widths.AnsiChars(8), iseditingreadonly: true)
                    .Text("Remark", header: "Remark", width: Widths.AnsiChars(30));


        }

        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["Status"] = "New";
            CurrentMaintain["CDate"] = DateTime.Today;
            CurrentMaintain["VNContractID"] = MyUtility.GetValue.Lookup("select top 1 ID from VNContract WITH (NOLOCK) where StartDate <= GETDATE() and EndDate >= GETDATE() and Status = 'Confirmed'");
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
                dateDate.Focus();
                MyUtility.Msg.WarningBox("Date can't empty!!");
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["VNContractID"]))
            {
                txtContractNo.Focus();
                MyUtility.Msg.WarningBox("Contract no. can't empty!!");
                return false;
            }
            #endregion

            #region 檢查BL No.與WK No.不可重複
            if (!MyUtility.Check.Empty(CurrentMaintain["BLNo"]))
            {
                if (MyUtility.Check.Seek(string.Format("select ID from VNImportDeclaration WITH (NOLOCK) where ID <> '{0}' and BLNo = '{1}'", MyUtility.Convert.GetString(CurrentMaintain["ID"]), MyUtility.Convert.GetString(CurrentMaintain["BLNo"]))))
                {
                    MyUtility.Msg.WarningBox("This B/L No. already exist!!");
                    return false;
                }
            }
            if (!MyUtility.Check.Empty(CurrentMaintain["WKNo"]))
            {
                if (MyUtility.Check.Seek(string.Format("select ID from VNImportDeclaration WITH (NOLOCK) where ID <> '{0}' and WKNo = '{1}'", MyUtility.Convert.GetString(CurrentMaintain["ID"]), MyUtility.Convert.GetString(CurrentMaintain["WKNo"]))))
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

           
        }

        protected override bool ClickPrint()
        {
            if (MyUtility.Convert.GetString(CurrentMaintain["Status"]).ToUpper() == "NEW")
            {
                MyUtility.Msg.WarningBox("Can't Print it ,you should Confirmed first! ");
                return false;
            }
            Sci.Production.Shipping.P40_Print callPurchaseForm = new Sci.Production.Shipping.P40_Print(CurrentMaintain);
            callPurchaseForm.ShowDialog(this);
            return base.ClickPrint();
        }

        //Contract No.
        private void txtContractNo_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sqlCmd = string.Format("select ID from VNContract WITH (NOLOCK) where StartDate <= {0} and EndDate >= {0} and Status = 'Confirmed'", MyUtility.Check.Empty(CurrentMaintain["CDate"]) ? "GETDATE()" : "'" + Convert.ToDateTime(CurrentMaintain["CDate"]).ToString("d") + "'");
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlCmd, "8", this.Text, false, ",");
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel) { return; }
            txtContractNo.Text = item.GetSelectedString();
        }

        //Contract No.
        private void txtContractNo_Validating(object sender, CancelEventArgs e)
        {
            if (EditMode && !MyUtility.Check.Empty(txtContractNo.Text) && txtContractNo.Text != txtContractNo.OldValue)
            {
                if (MyUtility.Check.Seek(string.Format("select ID from VNContract WITH (NOLOCK) where ID = '{0}'", txtContractNo.Text)))
                {
                    if (!MyUtility.Check.Seek(string.Format("select ID from VNContract WITH (NOLOCK) where  ID = '{0}' and StartDate <= {1} and EndDate >= {1} and Status = 'Confirmed'", txtContractNo.Text, MyUtility.Check.Empty(CurrentMaintain["CDate"]) ? "GETDATE()" : "'" + Convert.ToDateTime(CurrentMaintain["CDate"]).ToString("d") + "'")))
                    {
                        txtContractNo.Text = "";
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox("This Contract can't use.");
                        return;
                    }
                }
                else
                {
                    txtContractNo.Text = "";
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox("Contract no. not found!!");
                    return;
                }
            }
        }

        //B/L No.
        private void txtBLNO_Validating(object sender, CancelEventArgs e)
        {
            if (EditMode)
            {
                if (txtBLNO.Text != txtBLNO.OldValue)
                {
                    foreach (DataRow dr in DetailDatas)
                    {
                        dr.Delete();
                    }
                    int isFtyExport = 0;
                    if (MyUtility.Check.Empty(txtBLNO.Text))
                    {
                        txtWKNo.ReadOnly = false;
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
                        if (MyUtility.Check.Seek(string.Format("select ShipModeID,ExportCountry from Export WITH (NOLOCK) where BLNo = '{0}'", txtBLNO.Text), out export))
                        {
                            isFtyExport = 0;
                            localPurchase = false;
                        }
                        else if (MyUtility.Check.Seek(string.Format("select Type,ShipModeID,ExportCountry from FtyExport WITH (NOLOCK) where BLNo = '{0}'", txtBLNO.Text), out export))
                        {
                            isFtyExport = 1;
                            localPurchase = MyUtility.Convert.GetString(export["Type"]) == "4" ? true : false;
                        }
                        else
                        {
                            CurrentMaintain["IsFtyExport"] = isFtyExport;
                            CurrentMaintain["IsLocalPO"] = localPurchase ? 1 : 0;
                            CurrentMaintain["BLNo"] = "";
                            CurrentMaintain["ShipModeID"] = "";
                            CurrentMaintain["FromSite"] = "";
                            CurrentMaintain["IsSystemCalculate"] = 0;
                            MyUtility.Msg.WarningBox("BL No. not found!!");
                            e.Cancel = true;
                            return;
                        }
                        CurrentMaintain["BLNo"] = txtBLNO.Text;
                        CurrentMaintain["ShipModeID"] = export["ShipModeID"];
                        CurrentMaintain["FromSite"] = export["ExportCountry"];
                        CurrentMaintain["IsFtyExport"] = isFtyExport;
                        CurrentMaintain["IsLocalPO"] = localPurchase ? 1 : 0;
                        CurrentMaintain["IsSystemCalculate"] = 1;
                        txtWKNo.ReadOnly = true;
                    }
                }
                OnDetailEntered();
            }
        }

        //B/L No.
        private void txtBLNO_Validated(object sender, EventArgs e)
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
        private void txtWKNo_Validating(object sender, CancelEventArgs e)
        {
            if (EditMode)
            {
                if (txtWKNo.Text != txtWKNo.OldValue)
                {
                    foreach (DataRow dr in DetailDatas)
                    {
                        dr.Delete();
                    }

                    if (MyUtility.Check.Empty(txtWKNo.Text))
                    {
                        txtBLNO.ReadOnly = false;
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
                        if (MyUtility.Check.Seek(string.Format("select BLNo,ShipModeID,ExportCountry from Export WITH (NOLOCK) where ID = '{0}'", txtWKNo.Text), out export))
                        {
                            if (!MyUtility.Check.Empty(export["BLNo"]))
                            {
                                CurrentMaintain["BLNo"] = export["BLNo"];
                                CurrentMaintain["WKNo"] = "";
                                txtBLNO.ReadOnly = false;
                                txtWKNo.ReadOnly = true;
                            }
                            else
                            {
                                txtBLNO.ReadOnly = true;
                                CurrentMaintain["WKNo"] = txtWKNo.Text;
                            }
                            CurrentMaintain["IsFtyExport"] = 0;
                            CurrentMaintain["IsLocalPO"] = 0;
                            CurrentMaintain["ShipModeID"] = export["ShipModeID"];
                            CurrentMaintain["FromSite"] = export["ExportCountry"];
                            CurrentMaintain["IsSystemCalculate"] = 1;
                            localPurchase = false;
                        }
                        else if (MyUtility.Check.Seek(string.Format("select * from FtyExport WITH (NOLOCK) where ID = '{0}'", txtWKNo.Text), out export))
                        {
                            if (MyUtility.Convert.GetString(export["Type"]) == "1")
                            {
                                CurrentMaintain["WKNo"] = "";
                                CurrentMaintain["ShipModeID"] = "";
                                CurrentMaintain["FromSite"] = "";
                                CurrentMaintain["IsSystemCalculate"] = 0;
                                CurrentMaintain["IsLocalPO"] = 0;
                                e.Cancel = true;
                                MyUtility.Msg.WarningBox("The Fty WK No. is < 3rd Country>!!");
                                return;
                            }
                            if (MyUtility.Convert.GetString(export["Type"]) == "3")
                            {
                                CurrentMaintain["WKNo"] = "";
                                CurrentMaintain["ShipModeID"] = "";
                                CurrentMaintain["FromSite"] = "";
                                CurrentMaintain["IsSystemCalculate"] = 0;
                                CurrentMaintain["IsLocalPO"] = 0;
                                e.Cancel = true;
                                MyUtility.Msg.WarningBox("The Fty WK No. is < Transfer Out>!!");
                                return;
                            }
                            localPurchase = MyUtility.Convert.GetString(export["Type"]) == "4" ? true : false;
                            if (!MyUtility.Check.Empty(export["BLNo"]))
                            {
                                CurrentMaintain["BLNo"] = export["BLNo"];
                                CurrentMaintain["WKNo"] = "";
                                txtBLNO.ReadOnly = false;
                                txtWKNo.ReadOnly = true;
                            }
                            else
                            {
                                txtBLNO.ReadOnly = true;
                                CurrentMaintain["WKNo"] = txtWKNo.Text;
                            }
                            CurrentMaintain["IsFtyExport"] = 1;
                            CurrentMaintain["IsLocalPO"] = localPurchase ? 1 : 0;
                            CurrentMaintain["ShipModeID"] = export["ShipModeID"];
                            CurrentMaintain["FromSite"] = export["ExportCountry"];
                            CurrentMaintain["IsSystemCalculate"] = 1;
                        }
                        else
                        {
                            CurrentMaintain["IsFtyExport"] = 0;
                            CurrentMaintain["IsLocalPO"] = 0;
                            CurrentMaintain["WKNo"] = "";
                            CurrentMaintain["ShipModeID"] = "";
                            CurrentMaintain["FromSite"] = "";
                            CurrentMaintain["IsSystemCalculate"] = 0;
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox("WK No. not found!!");
                            return;
                        }
                    }
                }
                OnDetailEntered();
            }
        }

        //WK No.
        private void txtWKNo_Validated(object sender, EventArgs e)
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
                    sqlCmd = string.Format(@"
select e.ID
	   , ed.POID
	   , Seq1 = ''
	   , Seq2 = '' 
	   , OriImportQty = IIF(ed.UnitID = 'CONE', li.MeterToCone, 1) * ed.Qty 
	   , OriUnit = IIF(ed.UnitID = 'CONE', 'M', ed.UnitID)
	   , SCIRefno = ed.RefNo
	   , ed.RefNo
	   , BrandID = ''
	   , Type = ed.MtlTypeID
	   , li.Description
	   , DescDetail = ''
	   , NLCode = isnull(li.NLCode,'')
	   , HSCode = isnull(li.HSCode,'')
	   , CustomsUnit = isnull(li.CustomsUnit,'')
	   , PcsLength = isnull(li.PcsLength,0.0) 
	   , PcsWidth = isnull(li.PcsWidth,0.0) 
	   , PcsKg = isnull(li.PcsKg,0.0) 
	   , NoDeclare = isnull(li.NoDeclare,0)
	   , ed.Price
	   , UnitRate = isnull((select Rate 
	   						from Unit_Rate WITH (NOLOCK) 
	   						where UnitFrom = IIF(ed.UnitID = 'CONE', 'M', ed.UnitID) 
								  and UnitTo = li.CustomsUnit),'') 
	   , M2UnitRate = isnull((select Rate 
	   						  from Unit_Rate WITH (NOLOCK) 
	   						  where UnitFrom = IIF(ed.UnitID = 'CONE', 'M', ed.UnitID) 
	   						  		and UnitTo = 'M'),'') 
	   , POSeq = '01'
from FtyExport e WITH (NOLOCK) 
inner join FtyExport_Detail ed WITH (NOLOCK) on e.ID = ed.ID
left join LocalItem li WITH (NOLOCK) on li.RefNo = ed.RefNo 
										and li.LocalSuppid = ed.SuppID
where {0}", sqlWhere);
                }
                else
                {
                    sqlCmd = string.Format(@"
select e.ID
	   , ed.PoID
	   , ed.Seq1
	   , ed.Seq2
	   , OriImportQty = ed.qty 
	   , OriUnit = ed.UnitId 
	   , SCIRefno = isnull(f.SCIRefno, '') 
	   , Refno = isnull(f.Refno, '') 
	   , BrandID = isnull(f.BrandID, '')
	   , Type = isnull(f.Type, '') 
	   , Description = isnull(f.Description, '') 
	   , DescDetail = isnull(f.DescDetail, '') 
	   , NLCode = isnull(f.NLCode, '') 
	   , HSCode = isnull(f.HSCode, '') 
	   , CustomsUnit = isnull(f.CustomsUnit, '') 
	   , PcsLength = isnull(f.PcsLength, 0.0) 
	   , PcsWidth = isnull(f.PcsWidth, 0.0) 
	   , PcsKg = isnull(f.PcsKg, 0.0) 
	   , NoDeclare = isnull(f.NoDeclare, 0) 
	   , ed.Price
	   , UnitRate = isnull((select Rate 
	   						from Unit_Rate WITH (NOLOCK) 
	   						where UnitFrom = ed.UnitId 
	   							  and UnitTo = f.CustomsUnit), '') 
	   , M2UnitRate = isnull((select Rate 
	   						  from Unit_Rate WITH (NOLOCK) 
	   						  where UnitFrom = ed.UnitId 
	   						  		and UnitTo = 'M'), '') 
	   , POSeq = isnull(psd.Seq1, '') 
from FtyExport e WITH (NOLOCK) 
inner join FtyExport_Detail ed WITH (NOLOCK) on e.ID = ed.ID
left join PO_Supp_Detail psd WITH (NOLOCK) on psd.ID = ed.PoID 
										  	  and psd.SEQ1 = ed.Seq1 
										  	  and psd.SEQ2 = ed.Seq2
left join Fabric f WITH (NOLOCK) on f.SCIRefno = psd.SCIRefno
where {0}", sqlWhere);
                }
            }
            else
            {
                sqlCmd = string.Format(@"
select e.ID
	   , ed.PoID
	   , ed.Seq1
	   , ed.Seq2
	   , OriImportQty = ed.qty + ed.Foc 
	   , OriUnit = ed.UnitId
	   , SCIRefno = isnull(f.SCIRefno, '')
	   , Refno = isnull(f.Refno, '') 
	   , BrandID = isnull(f.BrandID, '') 
	   , Type = isnull(f.Type, '') 
	   , Description = isnull(f.Description, '') 
	   , DescDetail = isnull(f.DescDetail, '') 
	   , NLCode = isnull(f.NLCode, '') 
	   , HSCode = isnull(f.HSCode, '') 
	   , CustomsUnit = isnull(f.CustomsUnit, '') 
	   , PcsLength = isnull(f.PcsLength, 0.0) 
	   , PcsWidth = isnull(f.PcsWidth, 0.0) 
	   , PcsKg = isnull(f.PcsKg, 0.0) 
	   , NoDeclare = isnull(f.NoDeclare, 0) 
	   , ed.Price
	   , UnitRate = isnull((select Rate 
	   						from Unit_Rate WITH (NOLOCK) 
	   						where UnitFrom = ed.UnitId 
	   							  and UnitTo = f.CustomsUnit), '') 
	   , M2UnitRate = isnull((select Rate 
	   						  from Unit_Rate WITH (NOLOCK) 
	   						  where UnitFrom = ed.UnitId 
	   						  		and UnitTo = 'M'), '') 
	   , POSeq = isnull(psd.Seq1, '') 
from Export e WITH (NOLOCK) 
inner join Export_Detail ed WITH (NOLOCK) on e.ID = ed.ID
left join PO_Supp_Detail psd WITH (NOLOCK) on psd.ID = ed.PoID and psd.SEQ1 = ed.Seq1 and psd.SEQ2 = ed.Seq2
left join Fabric f WITH (NOLOCK) on f.SCIRefno = psd.SCIRefno
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
                    (MyUtility.Convert.GetString(dr["Type"]) == "A" && MyUtility.Convert.GetString(dr["OriUnit"]) != MyUtility.Convert.GetString(dr["CustomsUnit"]) && MyUtility.Convert.GetString(dr["CustomsUnit"]).ToUpper() == "M" && dr["PcsLength"].ToString().EqualDecimal(0)) ||
                    (MyUtility.Convert.GetString(dr["Type"]) == "A" && MyUtility.Convert.GetString(dr["OriUnit"]) != MyUtility.Convert.GetString(dr["CustomsUnit"]) && MyUtility.Convert.GetString(dr["CustomsUnit"]).ToUpper() == "M2" && MyUtility.Convert.GetString(dr["OriUnit"]).ToUpper() != "M" && MyUtility.Check.Empty(dr["UnitRate"]) && dr["PcsLength"].ToString().EqualDecimal(0)))
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
            sqlCmd.Append(@"
with ExportDetail as (
");
            #region 組撈資料Sql
            if (MyUtility.Convert.GetString(CurrentMaintain["IsFtyExport"]).ToUpper() == "TRUE")
            {
                if (localPurchase)
                {
                    sqlCmd.Append(string.Format(@"
    select e.ID
	       , ed.POID
	       , Seq1 = ''
	       , Seq2 = '' 
	       , OriImportQty = IIF(ed.UnitID = 'CONE', li.MeterToCone, 1) * ed.Qty 
	       , OriUnit = IIF(ed.UnitID = 'CONE', 'M', ed.UnitID)
	       , Type = ed.MtlTypeID
	       , Price = IIF(ed.UnitID = 'CONE',(ed.Price * (select Rate 
	   												     from dbo.GetCurrencyRate('20', ed.CurrencyID, 'USD', e.AddDate)))
	   											      / li.MeterToCone
									       , ed.Price * (select Rate 
									   				     from dbo.GetCurrencyRate('20', ed.CurrencyID, 'USD', e.AddDate)))
	       , NLCode = isnull(li.NLCode, '') 
	       , HSCode = isnull(li.HSCode, '')
	       , CustomsUnit = isnull(li.CustomsUnit, '')
	       , PcsLength = isnull(li.PcsLength, 0.0) 
	       , PcsWidth = isnull(li.PcsWidth, 0.0) 
	       , PcsKg = isnull(li.PcsKg, 0.0) 
	       , NoDeclare = isnull(li.NoDeclare, 0)
	       , Width = 0.0 
	       , RateValue = isnull((select RateValue 
	   						     from dbo.View_Unitrate 
	   						     where FROM_U = IIF(ed.UnitID = 'CONE', 'M', ed.UnitID) 
					 			       and TO_U = li.CustomsUnit)
							    , 1) 
	       , M2RateValue = (select RateValue 
	   					    from dbo.View_Unitrate 
	   					    where FROM_U = IIF(ed.UnitID = 'CONE', 'M', ed.UnitID) 
   							      and TO_U = 'M') 
	       , UnitRate = isnull((select Rate 
	   						    from Unit_Rate WITH (NOLOCK) 
	   						    where UnitFrom = IIF(ed.UnitID = 'CONE', 'M', ed.UnitID) 
	   							      and UnitTo = li.CustomsUnit),'') 
	       , M2UnitRate = isnull((select Rate 
	   						      from Unit_Rate WITH (NOLOCK) 
	   						      where UnitFrom = IIF(ed.UnitID = 'CONE', 'M', ed.UnitID) 
	   						  		    and UnitTo = 'M'),'')
    from FtyExport e WITH (NOLOCK) 
    inner join FtyExport_Detail ed WITH (NOLOCK) on e.ID = ed.ID
    left join LocalItem li WITH (NOLOCK) on li.RefNo = ed.RefNo 
										    and li.LocalSuppid = ed.SuppID
    where {0}", sqlWhere));
                }
                else
                {
                    sqlCmd.Append(string.Format(@"
    select e.ID
	       , ed.PoID
	       , ed.Seq1
	       , ed.Seq2
	       , OriImportQty = ed.qty 
	       , OriUnit = ed.UnitId
	       , Type = isnull(f.Type, '')
	       , Price = ed.Price * (select Rate 
	   						     from dbo.GetCurrencyRate('20', ed.CurrencyID, 'USD', e.AddDate))
	       , NLCode = isnull(f.NLCode,'')
	       , HSCode = isnull(f.HSCode,'') 
	       , CustomsUnit = isnull(f.CustomsUnit,'') 
	       , PcsLength = isnull(f.PcsLength, 0.0)
	       , PcsWidth = isnull(f.PcsWidth, 0.0) 
	       , PcsKg = isnull(f.PcsKg, 0.0)
	       , NoDeclare = isnull(f.NoDeclare, 0) 
	       , Width = isnull(f.Width, 0) 
	       , RateValue = isnull((select RateValue 
	   						     from dbo.View_Unitrate 
	   						     where FROM_U = ed.UnitId 
	   						 	       and TO_U = f.CustomsUnit)
	   						    , 1) 
	       , M2RateValue = (select RateValue 
	   					    from dbo.View_Unitrate 
	   					    where FROM_U = ed.UnitId 
	   						      and TO_U = 'M') 
	       , UnitRate = isnull((select Rate 
	   						    from Unit_Rate WITH (NOLOCK) 
	   						    where UnitFrom = ed.UnitId 
	   							      and UnitTo = f.CustomsUnit), '')
	       , M2UnitRate = isnull((select Rate 
	   						      from Unit_Rate WITH (NOLOCK) 
	   						      where UnitFrom = ed.UnitId 
	   						  		    and UnitTo = 'M'),'') 
    from FtyExport e WITH (NOLOCK) 
    inner join FtyExport_Detail ed WITH (NOLOCK) on e.ID = ed.ID
    left join PO_Supp_Detail psd WITH (NOLOCK) on psd.ID = ed.PoID 
											      and psd.SEQ1 = ed.Seq1 
											      and psd.SEQ2 = ed.Seq2
    left join Fabric f WITH (NOLOCK) on f.SCIRefno = psd.SCIRefno
    where {0}", sqlWhere));
                }
            }
            else
            {
                sqlCmd.Append(string.Format(@"
    select e.ID
	       , ed.PoID
	       , ed.Seq1
	       , ed.Seq2
	       , OriImportQty = ed.qty + ed.Foc
	       , OriUnit = ed.UnitId
	       , Type = isnull(f.Type,'')
	       , Price = ed.Price * (select Rate 
	   						     from dbo.GetCurrencyRate('20', ed.CurrencyID, 'USD', e.CloseDate)) 
	       , NLCode = isnull(f.NLCode, '') 
	       , HSCode = isnull(f.HSCode, '') 
	       , CustomsUnit = isnull(f.CustomsUnit, '') 
	       , PcsLength = isnull(f.PcsLength, 0.0) 
	       , PcsWidth = isnull(f.PcsWidth, 0.0) 
	       , PcsKg = isnull(f.PcsKg, 0.0)
	       , NoDeclare = isnull(f.NoDeclare, 0)
	       , Width = isnull(f.Width, 0)
	       , RateValue = isnull((select RateValue 
	   						     from dbo.View_Unitrate 
	   						     where FROM_U = ed.UnitId 
	   						 	       and TO_U = f.CustomsUnit)
	   						    , 1) 
	       , M2RateValue = (select RateValue 
	   					    from dbo.View_Unitrate 
	   					    where FROM_U = ed.UnitId 
	   						      and TO_U = 'M') 
	       , UnitRate = isnull((select Rate 
	   						    from Unit_Rate WITH (NOLOCK) 
	   						    where UnitFrom = ed.UnitId 
   								      and UnitTo = f.CustomsUnit), '') 
	       , M2UnitRate = isnull((select Rate 
	   						      from Unit_Rate WITH (NOLOCK) 
	   						      where UnitFrom = ed.UnitId 
	   						  		    and UnitTo = 'M'), '') 
    from Export e WITH (NOLOCK) 
    inner join Export_Detail ed WITH (NOLOCK) on e.ID = ed.ID
    left join PO_Supp_Detail psd WITH (NOLOCK) on psd.ID = ed.PoID 
											      and psd.SEQ1 = ed.Seq1 
											      and psd.SEQ2 = ed.Seq2
    left join Fabric f WITH (NOLOCK) on f.SCIRefno = psd.SCIRefno
    where {0}", sqlWhere));
            }
            #endregion
            sqlCmd.Append(@"
),
NotInPo as (
	select #tmp.POID 
           , #tmp.Seq1 
           , #tmp.Seq2
           , #tmp.NLCode
           , #tmp.HSCode
	       , #tmp.CustomsUnit
           , #tmp.Price
		   , #tmp.Type
           , #tmp.OriUnit
           , #tmp.OriImportQty
           , #tmp.Width
           , #tmp.PcsWidth
           , #tmp.PcsLength
           , #tmp.PcsKg
           , M2RateValue = M2RateValue.value
           , RateValue = RateValue.value
           , M2UnitRate = M2UnitRate.value
           , UnitRate = UnitRate.value
	from #tmp
    left join PO_Supp_Detail psd WITH (NOLOCK) on psd.ID = #tmp.POID 
											    and psd.SEQ1 = #tmp.Seq1 
											    and psd.SEQ2 = #tmp.Seq2
    left join Fabric f WITH (NOLOCK) on f.SCIRefno = psd.SCIRefno
    outer apply (
        select value = (select RateValue 
	   					from dbo.View_Unitrate 
	   					where FROM_U = #tmp.OriUnit 
	   						and TO_U = 'M') 
    ) M2RateValue
    outer apply (
        select value = isnull((select RateValue 
	   						   from dbo.View_Unitrate 
	   						   where FROM_U = #tmp.OriUnit 
	   						 	     and TO_U = f.CustomsUnit)
	   						  , 1) 
    ) RateValue
    outer apply (
        select value = isnull((select Rate 
	   						   from Unit_Rate WITH (NOLOCK) 
	   						   where UnitFrom = #tmp.OriUnit 
	   						  	     and UnitTo = 'M'), '') 
    ) M2UnitRate
    outer apply (
        select value = isnull((select Rate 
	   						   from Unit_Rate WITH (NOLOCK) 
	   						   where UnitFrom = #tmp.OriUnit 
   								     and UnitTo = f.CustomsUnit), '') 
    ) UnitRate
	where #tmp.NoDeclare = 0
)
select NLCode
	   , HSCode
	   , CustomsUnit
	   , sum(NewQty) as NewQty
	   , sum(NewQty * Price) as Price 
from (
	select NLCode
	       , HSCode
	       , CustomsUnit
           , Price
		   , NewQty = [dbo].getVNUnitTransfer(Type, OriUnit, CustomsUnit, OriImportQty, Width, PcsWidth, PcsLength, PcsKg, IIF(CustomsUnit = 'M2', M2RateValue, RateValue), IIF(CustomsUnit = 'M2', M2UnitRate, UnitRate))
    from ExportDetail WITH (NOLOCK) 
    where NoDeclare = 0
          and not exists (select 1
                          from #tmp 
                          where #tmp.Poid = ExportDetail.Poid
                                and #tmp.seq1 = ExportDetail.seq1
                                and #tmp.seq2 = ExportDetail.seq2)
    
    union all
    select NLCode
	       , HSCode
	       , CustomsUnit
           , Price
		   , NewQty = [dbo].getVNUnitTransfer(Type, OriUnit, CustomsUnit, OriImportQty, Width, PcsWidth, PcsLength, PcsKg, IIF(CustomsUnit = 'M2', M2RateValue, RateValue), IIF(CustomsUnit = 'M2', M2UnitRate, UnitRate))
    from NotInPo WITH (NOLOCK) 
) a
group by NLCode, HSCode, CustomsUnit");

            DataTable selectedData;
            DualResult result = MyUtility.Tool.ProcessWithDatatable(NotInPO, null, sqlCmd.ToString(), out selectedData);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Calculate fail!!\r\n"+result.ToString());
                return;
            }

            // 將Not in po的資料加入
            DataTable GroupNoInPOData;

            //將資料做排序
            try
            {
                MyUtility.Tool.ProcessWithDatatable(selectedData, @"NLCode,HSCode,CustomsUnit,NewQty,Price",
                    @"
select NLCode
	   , HSCode
	   , CustomsUnit
	   , NewQty
	   , Price = Price / NewQty
from #tmp
order by CONVERT(int, SUBSTRING(NLCode, 3, 3))", out GroupNoInPOData);
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
                    wrongData.Append("Below data is no Customs Code in B40, B41:\r\n");
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
