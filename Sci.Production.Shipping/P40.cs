using System;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;
using System.Linq;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// P40
    /// </summary>
    public partial class P40 : Win.Tems.Input6
    {
        private DataGridViewGeneratorTextColumnSettings nlcode = new DataGridViewGeneratorTextColumnSettings();
        private DataGridViewGeneratorTextColumnSettings brand = new DataGridViewGeneratorTextColumnSettings();
        private DataGridViewGeneratorTextColumnSettings refno = new DataGridViewGeneratorTextColumnSettings();
        private DataGridViewGeneratorTextColumnSettings usageUnit = new DataGridViewGeneratorTextColumnSettings();
        private DataGridViewGeneratorTextColumnSettings fabricType = new DataGridViewGeneratorTextColumnSettings();
        private DataGridViewGeneratorNumericColumnSettings qty = new DataGridViewGeneratorNumericColumnSettings();
        private Ict.Win.UI.DataGridViewTextBoxColumn col_nlcode;
        private Ict.Win.UI.DataGridViewNumericBoxColumn col_qty;
        private bool localPurchase = false;
        private DataTable NoNLCode;
        private DataTable NotInPO;
        private DataTable UnitNotFound;

        /// <summary>
        /// P40
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P40(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();

            // 建立NoNLCode, NotInPO, UnitNotFound的結構
            string sqlCmd = "select '' as SCIRefno,'' as Refno,'' as BrandID,'' as Type,'' as Description,'' as NLCode,'' as HSCode,'' as CustomsUnit,0.0 as PcsWidth,0.0 as PcsLength,0.0 as PcsKg,0 as NoDeclare,UsageUnit='' from VNImportDeclaration WITH (NOLOCK) where 1=0";
            DBProxy.Current.Select(null, sqlCmd, out this.NoNLCode);
            sqlCmd = "select '' as ID,'' as POID,'' as Seq1,'' as Seq2,'' as Seq,'' as Description,'' as Type,'' as OriUnit,0.0 as OriImportQty,0.0 as Width,'' as NLCode,'' as HSCode,'' as CustomsUnit,0.0 as PcsWidth,0.0 as PcsLength,0.0 as PcsKg,0 as NoDeclare,0.0000 as Price,UsageUnit=''   from VNImportDeclaration WITH (NOLOCK) where 1=0";
            DBProxy.Current.Select(null, sqlCmd, out this.NotInPO);
            sqlCmd = "select '' as OriUnit,'' as CustomsUnit,'' as RefNo from VNImportDeclaration WITH (NOLOCK) where 1=0";
            DBProxy.Current.Select(null, sqlCmd, out this.UnitNotFound);
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            if (this.EditMode)
            {
                if (MyUtility.Convert.GetString(this.CurrentMaintain["Status"]).ToUpper() == "CONFIRMED")
                {
                    this.txtCustomdeclareno.ReadOnly = false;
                    this.dateDate.ReadOnly = true;
                    this.txtContractNo.ReadOnly = true;
                    this.txtBLNO.ReadOnly = true;
                    this.txtWKNo.ReadOnly = true;
                    this.txtshipmodeShipby.ReadOnly = true;
                    this.txtcountryCountryfrom.TextBox1.ReadOnly = true;
                    this.gridicon.Append.Enabled = false;
                    this.gridicon.Insert.Enabled = false;
                    this.gridicon.Remove.Enabled = false;
                    this.detailgrid.IsEditingReadOnly = true;
                }
                else
                {
                    this.txtCustomdeclareno.ReadOnly = true;
                    this.dateDate.ReadOnly = false;
                    this.txtContractNo.ReadOnly = false;
                    this.txtshipmodeShipby.ReadOnly = false;
                    this.txtcountryCountryfrom.TextBox1.ReadOnly = false;

                    if (MyUtility.Convert.GetString(this.CurrentMaintain["IsSystemCalculate"]).ToUpper() == "TRUE")
                    {
                        this.gridicon.Append.Enabled = false;
                        this.gridicon.Insert.Enabled = false;
                        this.gridicon.Remove.Enabled = false;
                        this.col_nlcode.IsEditingReadOnly = true;
                        this.col_qty.IsEditingReadOnly = true;
                    }
                    else
                    {
                        this.gridicon.Append.Enabled = true;
                        this.gridicon.Insert.Enabled = true;
                        this.gridicon.Remove.Enabled = true;
                        this.col_nlcode.IsEditingReadOnly = false;
                        this.col_qty.IsEditingReadOnly = false;
                    }

                    if (MyUtility.Check.Empty(this.CurrentMaintain["BLNo"]) && MyUtility.Check.Empty(this.CurrentMaintain["WKNo"]))
                    {
                        this.txtBLNO.ReadOnly = false;
                        this.txtWKNo.ReadOnly = false;
                    }
                    else if (MyUtility.Check.Empty(this.CurrentMaintain["BLNo"]))
                    {
                        this.txtBLNO.ReadOnly = true;
                        this.txtWKNo.ReadOnly = false;
                    }
                    else
                    {
                        this.txtBLNO.ReadOnly = false;
                        this.txtWKNo.ReadOnly = true;
                    }
                }

                this.detailgrid.EnsureStyle();
            }
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : MyUtility.Convert.GetString(e.Master["ID"]);
            this.DetailSelectCommand = $@"
select vdd.ID,vdd.Refno,vdd.FabricType,vdd.NLCode,Qty = Round(vdd.Qty,2),vdd.Remark,vd.HSCode,vd.UnitID,vd.Price,vdd.BrandID,vdd.UsageUnit
from VNImportDeclaration_Detail_Detail vdd with(nolock)
inner join VNImportDeclaration_Detail vd with(nolock) on vd.id = vdd.ID and vd.NLCode = vdd.NLCode
where vdd.id = '{masterID}'
order by TRY_CONVERT(int, SUBSTRING(vdd.NLCode, 3, LEN(vdd.NLCode))), vdd.NLCode
";
            return base.OnDetailSelectCommandPrepare(e);
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            #region NL Code的Validating
            this.nlcode.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    if (!MyUtility.Check.Empty(e.FormattedValue))
                    {
                        if (MyUtility.Convert.GetString(dr["nlcode"]) != MyUtility.Convert.GetString(e.FormattedValue))
                        {
                            DataRow seekData;
                            if (!MyUtility.Check.Seek(
                                string.Format(
                                "select HSCode,UnitID from VNContract_Detail WITH (NOLOCK) where ID = '{0}' and NLCode = '{1}'",
                                MyUtility.Convert.GetString(this.CurrentMaintain["VNContractID"]),
                                MyUtility.Convert.GetString(e.FormattedValue)),
                                out seekData))
                            {
                                dr["HSCode"] = string.Empty;
                                dr["NLCode"] = string.Empty;
                                dr["Qty"] = 0;
                                dr["UnitID"] = string.Empty;
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
                        dr["HSCode"] = string.Empty;
                        dr["NLCode"] = string.Empty;
                        dr["Qty"] = 0;
                        dr["UnitID"] = string.Empty;
                    }
                }
            };
            #endregion

            #region brand的Validating
            this.brand.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    if (!MyUtility.Check.Empty(e.FormattedValue))
                    {
                        dr["BrandID"] = e.FormattedValue;
                        dr.EndEdit();
                        this.BRT(dr);
                    }
                }
            };
            #endregion
            #region refno的Validating
            this.refno.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    if (!MyUtility.Check.Empty(e.FormattedValue))
                    {
                        dr["refno"] = e.FormattedValue;
                        dr.EndEdit();
                        this.BRT(dr);
                    }
                }
            };
            #endregion
            #region fabricType的Validating
            this.fabricType.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    if (!MyUtility.Check.Empty(e.FormattedValue))
                    {
                        dr["fabricType"] = e.FormattedValue;
                        dr.EndEdit();
                        if (!(MyUtility.Convert.GetString(dr["FabricType"]) == "F" || MyUtility.Convert.GetString(dr["FabricType"]) == "A"))
                        {
                            dr["usageUnit"] = string.Empty;
                            dr.EndEdit();
                        }

                        this.BRT(dr);
                    }
                }
            };
            #endregion
            #region usageUnit的Validating
            this.usageUnit.CellEditable += (s, e) =>
            {
                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (!(MyUtility.Convert.GetString(dr["FabricType"]) == "F" || MyUtility.Convert.GetString(dr["FabricType"]) == "A"))
                {
                    e.IsEditable = false;
                }
            };

            this.usageUnit.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    if (!MyUtility.Check.Empty(e.FormattedValue))
                    {
                        dr["usageUnit"] = e.FormattedValue;
                        dr.EndEdit();
                        this.BRT(dr);
                    }
                }
            };
            #endregion

            this.qty.CellMouseDoubleClick += (s, e) =>
                {
                    if (!this.EditMode)
                    {
                        if (e.Button == MouseButtons.Left)
                        {
                            DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                            P40_Detail callNextForm = new P40_Detail(this.CurrentMaintain, MyUtility.Convert.GetString(dr["NLCode"]));
                            DialogResult result = callNextForm.ShowDialog(this);
                            callNextForm.Dispose();
                        }
                    }
                };
            base.OnDetailGridSetup();
            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("BrandId", header: "Brand", width: Widths.AnsiChars(10), settings: this.brand)
                .Text("RefNo", header: "Ref No.", width: Widths.AnsiChars(10), settings: this.refno)
                .Text("FabricType", header: "Type", width: Widths.AnsiChars(10), settings: this.fabricType)
                .Text("UsageUnit", header: "UsageUnit", width: Widths.AnsiChars(8), settings: this.usageUnit)
                .Text("HSCode", header: "HS Code", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("NLCode", header: "Customs Code", width: Widths.AnsiChars(12), settings: this.nlcode).Get(out this.col_nlcode)
                .Numeric("Qty", header: "Customs Qty", decimal_places: 2, width: Widths.AnsiChars(15), settings: this.qty).Get(out this.col_qty)
                .Text("UnitID", header: "Customs Unit", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("Remark", header: "Remark", width: Widths.AnsiChars(30));
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["Status"] = "New";
            this.CurrentMaintain["CDate"] = DateTime.Today;
            this.CurrentMaintain["VNContractID"] = MyUtility.GetValue.Lookup("select top 1 ID from VNContract WITH (NOLOCK) where StartDate <= GETDATE() and EndDate >= GETDATE() and Status = 'Confirmed'");
        }

        /// <inheritdoc/>
        protected override bool ClickDeleteBefore()
        {
            if (MyUtility.Convert.GetString(this.CurrentMaintain["Status"]).ToUpper() == "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("This record already confirmed, can't delete!!");
                return false;
            }

            return base.ClickDeleteBefore();
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            #region 檢查必輸欄位
            if (MyUtility.Check.Empty(this.CurrentMaintain["CDate"]))
            {
                this.dateDate.Focus();
                MyUtility.Msg.WarningBox("Date can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["VNContractID"]))
            {
                this.txtContractNo.Focus();
                MyUtility.Msg.WarningBox("Contract no. can't empty!!");
                return false;
            }
            #endregion

            #region 檢查BL No.與WK No.不可重複
            if (!MyUtility.Check.Empty(this.CurrentMaintain["BLNo"]))
            {
                if (MyUtility.Check.Seek(string.Format("select ID from VNImportDeclaration WITH (NOLOCK) where ID <> '{0}' and BLNo = '{1}'", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]), MyUtility.Convert.GetString(this.CurrentMaintain["BLNo"]))))
                {
                    MyUtility.Msg.WarningBox("This B/L No. already exist!!");
                    return false;
                }
            }

            if (!MyUtility.Check.Empty(this.CurrentMaintain["WKNo"]))
            {
                if (MyUtility.Check.Seek(string.Format("select ID from VNImportDeclaration WITH (NOLOCK) where ID <> '{0}' and WKNo = '{1}'", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]), MyUtility.Convert.GetString(this.CurrentMaintain["WKNo"]))))
                {
                    MyUtility.Msg.WarningBox("This WK No. already exist!!");
                    return false;
                }
            }

            #endregion

            #region 刪除表身NL Code與Qty為0的資料
            int recCount = 0;
            foreach (DataRow dr in this.DetailDatas)
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

            #region 檢查畫面上的Refno在[Fabric], 是否能找到對應的[Fabric].NLCode
            foreach (DataRow dr in this.DetailDatas)
            {
                string fabricType = MyUtility.Convert.GetString(dr["FabricType"]);
                if (fabricType.EqualString("A") || fabricType.EqualString("F"))
                {
                    if (!MyUtility.Check.Seek($"select 1 from Fabric with(nolock) where Refno = '{dr["Refno"].ToString().Replace("'", "''")}' and NLCode2 = '{dr["NLcode"]}'"))
                    {
                        MyUtility.Msg.WarningBox($"Refno:{dr["Refno"]}, Customs Code:{dr["NLcode"]} not exists!");
                        return false;
                    }
                }
                else
                {
                    if (!MyUtility.Check.Seek($"select 1 from LocalItem with(nolock) where ltrim(Refno) = '{dr["Refno"].ToString().Replace("'", "''")}' and NLCode2 = '{dr["NLcode"]}'"))
                    {
                        MyUtility.Msg.WarningBox($"Refno:{dr["Refno"]}, Customs Code:{dr["NLcode"]} not exists!");
                        return false;
                    }
                }
            }
            #endregion

            #region
            var detailList = ((DataTable)this.detailgridbs.DataSource).AsEnumerable().Where(w => w.RowState != DataRowState.Deleted);
            if (detailList.Count() > 0)
            {
                var duplicateList = detailList
                    .GroupBy(g => new { ID = g["ID"].ToString(), NLCode = g["NLCode"].ToString(), HSCode = g["HSCode"].ToString() })
                    .GroupBy(g => new { g.Key.ID, g.Key.NLCode })
                    .Select(g => new { g.Key.ID, g.Key.NLCode, ct = g.Count() })
                    .Where(w => w.ct > 1);
                if (duplicateList.Count() > 0)
                {
                    string msg = @"
There have different HS Code in the same customs code.
Please check below material some HS Code need to update.
Brand, RefNo, HSCode, Customs Code
";
                    var dl = detailList.
                        Where(w => duplicateList.Select(s => s.NLCode).Contains(MyUtility.Convert.GetString(w["NLCode"]))).
                        OrderBy(o => MyUtility.Convert.GetString(o["NLCode"])).
                        Select(s => new
                        {
                            list = MyUtility.Convert.GetString(s["BrandID"]) + "," +
                                MyUtility.Convert.GetString(s["RefNo"]) + "," +
                                MyUtility.Convert.GetString(s["HSCode"]) + "," +
                                MyUtility.Convert.GetString(s["NLCode"]),
                        }).Select(s => s.list.ToString()).ToList();
                    MyUtility.Msg.WarningBox(msg + string.Join("\r\n", dl));
                    return false;
                }
            }
            #endregion

            // Get ID
            if (this.IsDetailInserting)
            {
                string newID = MyUtility.GetValue.GetID(Env.User.Keyword + "ID", "VNImportDeclaration", Convert.ToDateTime(this.CurrentMaintain["CDate"]), 2, "ID", null);
                if (MyUtility.Check.Empty(newID))
                {
                    MyUtility.Msg.WarningBox("GetID fail, please try again!");
                    return false;
                }

                this.CurrentMaintain["ID"] = newID;
            }

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override DualResult ClickSavePost()
        {
            string insertuUdataDetail = $@"
select ID,HSCode,NLCode,UnitID,a.Remark,Qty = sum(Qty),Price = Sum(Price)
into #tmps
from #tmp t
outer apply(select Remark=stuff((
		select concat(',',Remark)
		from #tmp t2
		where t.ID = t2.ID and t.NLCode = t2.NLCode and isnull(Remark,'') <> ''
		for xml path(''))
	,1,1,'')
)a
group by ID,HSCode,NLCode,UnitID,a.Remark

merge VNImportDeclaration_Detail t
using #tmps s
on t.ID = s.ID and t.NLCode = s.NLCode
when matched then update set
	t.HSCode = s.HSCode,
	t.UnitID = s.UnitID,
	t.Remark = s.Remark,
	t.Qty = s.Qty,
	t.Price = s.Price
when not matched by target then
	insert(ID,HSCode,NLCode,UnitID,Remark,Qty,Price)
	values(s.ID,s.HSCode,s.NLCode,s.UnitID,s.Remark,s.Qty,s.Price)
when not matched by source and t.id in(select id from #tmps) then
	delete
;
";
            DataTable dt;
            DualResult result = MyUtility.Tool.ProcessWithDatatable((DataTable)this.detailgridbs.DataSource, string.Empty, insertuUdataDetail, out dt);
            if (!result)
            {
                return Ict.Result.F(result.ToString());
            }

            return base.ClickSavePost();
        }

        /// <inheritdoc/>
        protected override DualResult ClickDelete()
        {
            string sqldelete = $@"Delete VNImportDeclaration_Detail where id = '{this.CurrentMaintain["ID"]}' ";
            DualResult reslut = DBProxy.Current.Execute(null, sqldelete);
            if (!reslut)
            {
                return Ict.Result.F(reslut.ToString());
            }

            return base.ClickDelete();
        }

        /// <inheritdoc/>
        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            string updateCmds = string.Format(
                "update VNImportDeclaration set EditDate = GETDATE(), EditName = '{0}', Status = 'Confirmed' where ID = '{1}'",
                Env.User.UserID,
                MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));

            DualResult result = DBProxy.Current.Execute(null, updateCmds);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Confirm fail!!\r\n" + result.ToString());
                return;
            }
        }

        /// <inheritdoc/>
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();

            string updateCmds = string.Format(
                "update VNImportDeclaration set EditDate = GETDATE(), EditName = '{0}', Status = 'New' where ID = '{1}'",
                Env.User.UserID,
                MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));

            DualResult result = DBProxy.Current.Execute(null, updateCmds);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Unconfirm fail!!\r\n" + result.ToString());
                return;
            }
        }

        /// <inheritdoc/>
        protected override bool ClickPrint()
        {
            if (MyUtility.Convert.GetString(this.CurrentMaintain["Status"]).ToUpper() == "NEW")
            {
                MyUtility.Msg.WarningBox("Can't Print it ,you should Confirmed first! ");
                return false;
            }

            P40_Print callPurchaseForm = new P40_Print(this.CurrentMaintain);
            callPurchaseForm.ShowDialog(this);
            return base.ClickPrint();
        }

        private void BRT(DataRow dr)
        {
            if ((!MyUtility.Check.Empty(dr["BrandID"]) && !MyUtility.Check.Empty(dr["Refno"]) && !MyUtility.Check.Empty(dr["FabricType"]) && !MyUtility.Check.Empty(dr["usageUnit"])) ||
                (MyUtility.Convert.GetString(dr["FabricType"]).EqualString("L") && !MyUtility.Check.Empty(dr["Refno"]) && !MyUtility.Check.Empty(dr["FabricType"])))
            {
                string type = MyUtility.Convert.GetString(dr["FabricType"]);
                if (type.EqualString("A") || type.EqualString("F"))
                {
                    string sql = $@"
select f.HSCode,f.CustomsUnit,[NLCode] = f.NLCode2
from brand b with(nolock)
outer apply(
	select top 1 f1.* 
	from Fabric f1 with(nolock) 
	inner join brand b2 with(nolock) on f1.BrandID = b2.id 
	where f1.Refno = '{dr["Refno"].ToString().Replace("'", "''")}' and b2.BrandGroup = b.BrandGroup and f1.Type = '{dr["FabricType"]}' and f1.Junk = 0 and f1.usageUnit = '{dr["usageUnit"]}'
	order by f1.NLCodeEditDate desc
)f
where b.id = '{dr["BrandID"]}'
";
                    DataRow row;
                    if (MyUtility.Check.Seek(sql, out row))
                    {
                        dr["NLCode"] = row["NLCode"];
                        dr["HSCode"] = row["HSCode"];

                        DataRow seekData;
                        string sqlunit = $@"
select HSCode,UnitID from VNContract_Detail WITH (NOLOCK) 
where ID = '{this.CurrentMaintain["VNContractID"]}' and NLCode = '{dr["NLCode"]}'";
                        if (!MyUtility.Check.Seek(sqlunit, out seekData))
                        {
                            dr["Qty"] = 0;
                            dr["UnitID"] = string.Empty;
                            MyUtility.Msg.WarningBox("Customs Code not found!!");
                            return;
                        }
                        else
                        {
                            dr["UnitID"] = seekData["UnitID"];
                        }
                    }
                    else
                    {
                        dr["HSCode"] = string.Empty;
                        dr["NLCode"] = string.Empty;
                        dr["Qty"] = 0;
                        dr["UnitID"] = string.Empty;
                    }
                }
                else if (type.EqualString("L"))
                {
                    string sql = $@"select li.HSCode,[NLCode] = li.NLCode2,li.CustomsUnit from LocalItem li where ltrim(li.RefNo) = '{dr["RefNo"].ToString().Replace("'", "''")}' ";
                    DataRow row;
                    if (MyUtility.Check.Seek(sql, out row))
                    {
                        dr["BrandID"] = string.Empty;
                        dr["NLCode"] = row["NLCode"];
                        dr["HSCode"] = row["HSCode"];
                        DataRow seekData;
                        string sqlunit = $@"
select HSCode,UnitID from VNContract_Detail WITH (NOLOCK) 
where ID = '{this.CurrentMaintain["VNContractID"]}' and NLCode = '{dr["NLCode"]}'";
                        if (!MyUtility.Check.Seek(sqlunit, out seekData))
                        {
                            dr["Qty"] = 0;
                            dr["UnitID"] = string.Empty;
                            MyUtility.Msg.WarningBox("Customs Code not found!!");
                            return;
                        }
                        else
                        {
                            dr["UnitID"] = seekData["UnitID"];
                        }
                    }
                    else
                    {
                        dr["HSCode"] = string.Empty;
                        dr["NLCode"] = string.Empty;
                        dr["Qty"] = 0;
                        dr["UnitID"] = string.Empty;
                    }
                }

                dr.EndEdit();
            }
        }

        // Contract No.
        private void TxtContractNo_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sqlCmd = string.Format("select ID from VNContract WITH (NOLOCK) where StartDate <= {0} and EndDate >= {0} and Status = 'Confirmed'", MyUtility.Check.Empty(this.CurrentMaintain["CDate"]) ? "GETDATE()" : "'" + Convert.ToDateTime(this.CurrentMaintain["CDate"]).ToString("d") + "'");
            Win.Tools.SelectItem item = new Win.Tools.SelectItem(sqlCmd, "8", this.Text, false, ",");
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.txtContractNo.Text = item.GetSelectedString();
        }

        // Contract No.
        private void TxtContractNo_Validating(object sender, CancelEventArgs e)
        {
            if (this.EditMode && !MyUtility.Check.Empty(this.txtContractNo.Text) && this.txtContractNo.Text != this.txtContractNo.OldValue)
            {
                if (MyUtility.Check.Seek(string.Format("select ID from VNContract WITH (NOLOCK) where ID = '{0}'", this.txtContractNo.Text)))
                {
                    if (!MyUtility.Check.Seek(string.Format("select ID from VNContract WITH (NOLOCK) where  ID = '{0}' and StartDate <= {1} and EndDate >= {1} and Status = 'Confirmed'", this.txtContractNo.Text, MyUtility.Check.Empty(this.CurrentMaintain["CDate"]) ? "GETDATE()" : "'" + Convert.ToDateTime(this.CurrentMaintain["CDate"]).ToString("d") + "'")))
                    {
                        this.txtContractNo.Text = string.Empty;
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox("This Contract can't use.");
                        return;
                    }
                }
                else
                {
                    this.txtContractNo.Text = string.Empty;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox("Contract no. not found!!");
                    return;
                }
            }
        }

        // B/L No.
        private void TxtBLNO_Validating(object sender, CancelEventArgs e)
        {
            if (this.EditMode)
            {
                if (this.txtBLNO.Text != this.txtBLNO.OldValue)
                {
                    foreach (DataRow dr in this.DetailDatas)
                    {
                        dr.Delete();
                    }

                    int isFtyExport = 0;
                    if (MyUtility.Check.Empty(this.txtBLNO.Text))
                    {
                        this.txtWKNo.ReadOnly = false;
                        this.CurrentMaintain["IsSystemCalculate"] = 0;
                        this.CurrentMaintain["BLNo"] = string.Empty;
                        this.CurrentMaintain["IsFtyExport"] = 0;
                        this.CurrentMaintain["IsLocalPO"] = 0;
                        this.CurrentMaintain["ShipModeID"] = string.Empty;
                        this.CurrentMaintain["FromSite"] = string.Empty;
                        this.CurrentMaintain["FromSite"] = string.Empty;
                    }
                    else
                    {
                        DataRow export;
                        if (MyUtility.Check.Seek(string.Format("select ShipModeID,ExportCountry from Export WITH (NOLOCK) where BLNo = '{0}'", this.txtBLNO.Text), out export))
                        {
                            isFtyExport = 0;
                            this.localPurchase = false;
                        }
                        else if (MyUtility.Check.Seek(string.Format("select Type,ShipModeID,ExportCountry from FtyExport WITH (NOLOCK) where BLNo = '{0}'", this.txtBLNO.Text), out export))
                        {
                            isFtyExport = 1;
                            this.localPurchase = MyUtility.Convert.GetString(export["Type"]) == "4" ? true : false;
                        }
                        else
                        {
                            this.CurrentMaintain["IsFtyExport"] = isFtyExport;
                            this.CurrentMaintain["IsLocalPO"] = this.localPurchase ? 1 : 0;
                            this.CurrentMaintain["BLNo"] = string.Empty;
                            this.CurrentMaintain["ShipModeID"] = string.Empty;
                            this.CurrentMaintain["FromSite"] = string.Empty;
                            this.CurrentMaintain["IsSystemCalculate"] = 0;
                            MyUtility.Msg.WarningBox("BL No. not found!!");
                            e.Cancel = true;
                            return;
                        }

                        this.CurrentMaintain["BLNo"] = this.txtBLNO.Text;
                        this.CurrentMaintain["ShipModeID"] = export["ShipModeID"];
                        this.CurrentMaintain["FromSite"] = export["ExportCountry"];
                        this.CurrentMaintain["IsFtyExport"] = isFtyExport;
                        this.CurrentMaintain["IsLocalPO"] = this.localPurchase ? 1 : 0;
                        this.CurrentMaintain["IsSystemCalculate"] = 1;
                        this.txtWKNo.ReadOnly = true;
                    }
                }

                this.OnDetailEntered();
            }
        }

        // B/L No.
        private void TxtBLNO_Validated(object sender, EventArgs e)
        {
            if (this.EditMode && !MyUtility.Check.Empty(this.CurrentMaintain["BLNo"]))
            {
                if (!this.QueryNonNLData(string.Format("e.BLNo = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["BLNo"]))))
                {
                    return;
                }

                if (this.NoNLCode.Rows.Count > 0 || this.NotInPO.Rows.Count > 0)
                {
                    P40_AssignNLCode callNextForm = new P40_AssignNLCode(this.NoNLCode, this.NotInPO, this.UnitNotFound, this.CurrentMaintain);
                    DialogResult result = callNextForm.ShowDialog(this);
                    if (result == DialogResult.OK)
                    {
                        this.NotInPO = callNextForm.NotInPo;
                        callNextForm.Dispose();
                        this.CalaulateData(string.Format("e.BLNo = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["BLNo"])));
                    }
                    else
                    {
                        callNextForm.Dispose();
                    }
                }
                else
                {
                    this.CalaulateData(string.Format("e.BLNo = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["BLNo"])));
                }
            }
        }

        // WK No.
        private void TxtWKNo_Validating(object sender, CancelEventArgs e)
        {
            if (this.EditMode)
            {
                if (this.txtWKNo.Text != this.txtWKNo.OldValue)
                {
                    foreach (DataRow dr in this.DetailDatas)
                    {
                        dr.Delete();
                    }

                    if (MyUtility.Check.Empty(this.txtWKNo.Text))
                    {
                        this.txtBLNO.ReadOnly = false;
                        this.CurrentMaintain["IsSystemCalculate"] = 0;
                        this.CurrentMaintain["WKNo"] = string.Empty;
                        this.CurrentMaintain["IsFtyExport"] = 0;
                        this.CurrentMaintain["IsLocalPO"] = 0;
                        this.CurrentMaintain["ShipModeID"] = string.Empty;
                        this.CurrentMaintain["FromSite"] = string.Empty;
                    }
                    else
                    {
                        DataRow export;
                        if (MyUtility.Check.Seek(string.Format("select BLNo,ShipModeID,ExportCountry from Export WITH (NOLOCK) where ID = '{0}'", this.txtWKNo.Text), out export))
                        {
                            if (!MyUtility.Check.Empty(export["BLNo"]))
                            {
                                this.CurrentMaintain["BLNo"] = export["BLNo"];
                                this.CurrentMaintain["WKNo"] = string.Empty;
                                this.txtBLNO.ReadOnly = false;
                                this.txtWKNo.ReadOnly = true;
                            }
                            else
                            {
                                this.txtBLNO.ReadOnly = true;
                                this.CurrentMaintain["WKNo"] = this.txtWKNo.Text;
                            }

                            this.CurrentMaintain["IsFtyExport"] = 0;
                            this.CurrentMaintain["IsLocalPO"] = 0;
                            this.CurrentMaintain["ShipModeID"] = export["ShipModeID"];
                            this.CurrentMaintain["FromSite"] = export["ExportCountry"];
                            this.CurrentMaintain["IsSystemCalculate"] = 1;
                            this.localPurchase = false;
                        }
                        else if (MyUtility.Check.Seek(string.Format("select * from FtyExport WITH (NOLOCK) where ID = '{0}'", this.txtWKNo.Text), out export))
                        {
                            if (MyUtility.Convert.GetString(export["Type"]) == "1")
                            {
                                this.CurrentMaintain["WKNo"] = this.txtWKNo.Text;
                                this.CurrentMaintain["IsLocalPO"] = this.localPurchase ? 1 : 0;
                                this.CurrentMaintain["ShipModeID"] = export["ShipModeID"];
                                this.CurrentMaintain["FromSite"] = export["ExportCountry"];
                                this.CurrentMaintain["IsSystemCalculate"] = 1;
                            }

                            if (MyUtility.Convert.GetString(export["Type"]) == "3")
                            {
                                this.CurrentMaintain["WKNo"] = string.Empty;
                                this.CurrentMaintain["ShipModeID"] = string.Empty;
                                this.CurrentMaintain["FromSite"] = string.Empty;
                                this.CurrentMaintain["IsSystemCalculate"] = 0;
                                this.CurrentMaintain["IsLocalPO"] = 0;
                                e.Cancel = true;
                                MyUtility.Msg.WarningBox("The Fty WK No. is < Transfer Out>!!");
                                return;
                            }

                            this.localPurchase = MyUtility.Convert.GetString(export["Type"]) == "4" ? true : false;
                            if (!MyUtility.Check.Empty(export["BLNo"]))
                            {
                                this.CurrentMaintain["BLNo"] = export["BLNo"];
                                this.CurrentMaintain["WKNo"] = string.Empty;
                                this.txtBLNO.ReadOnly = false;
                                this.txtWKNo.ReadOnly = true;
                            }
                            else
                            {
                                this.txtBLNO.ReadOnly = true;
                                this.CurrentMaintain["WKNo"] = this.txtWKNo.Text;
                            }

                            this.CurrentMaintain["IsFtyExport"] = 1;
                            this.CurrentMaintain["IsLocalPO"] = this.localPurchase ? 1 : 0;
                            this.CurrentMaintain["ShipModeID"] = export["ShipModeID"];
                            this.CurrentMaintain["FromSite"] = export["ExportCountry"];
                            this.CurrentMaintain["IsSystemCalculate"] = 1;
                        }
                        else
                        {
                            this.CurrentMaintain["IsFtyExport"] = 0;
                            this.CurrentMaintain["IsLocalPO"] = 0;
                            this.CurrentMaintain["WKNo"] = string.Empty;
                            this.CurrentMaintain["ShipModeID"] = string.Empty;
                            this.CurrentMaintain["FromSite"] = string.Empty;
                            this.CurrentMaintain["IsSystemCalculate"] = 0;
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox("WK No. not found!!");
                            return;
                        }
                    }
                }

                this.OnDetailEntered();
            }
        }

        // WK No.
        private void TxtWKNo_Validated(object sender, EventArgs e)
        {
            if (this.EditMode)
            {
                if (!MyUtility.Check.Empty(this.CurrentMaintain["BLNo"]))
                {
                    if (!this.QueryNonNLData(string.Format("e.BLNo = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["BLNo"]))))
                    {
                        return;
                    }

                    if (this.NoNLCode.Rows.Count > 0 || this.NotInPO.Rows.Count > 0)
                    {
                        P40_AssignNLCode callNextForm = new P40_AssignNLCode(this.NoNLCode, this.NotInPO, this.UnitNotFound, this.CurrentMaintain);
                        DialogResult result = callNextForm.ShowDialog(this);
                        if (result == DialogResult.OK)
                        {
                            this.NotInPO = callNextForm.NotInPo;
                            callNextForm.Dispose();
                            this.CalaulateData(string.Format("e.BLNo = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["BLNo"])));
                        }
                        else
                        {
                            callNextForm.Dispose();
                        }
                    }
                    else
                    {
                        this.CalaulateData(string.Format("e.BLNo = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["BLNo"])));
                    }
                }
                else if (!MyUtility.Check.Empty(this.CurrentMaintain["WKNo"]))
                {
                    if (!this.QueryNonNLData(string.Format("e.ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["WKNo"]))))
                    {
                        return;
                    }

                    if (this.NoNLCode.Rows.Count > 0 || this.NotInPO.Rows.Count > 0)
                    {
                        P40_AssignNLCode callNextForm = new P40_AssignNLCode(this.NoNLCode, this.NotInPO, this.UnitNotFound, this.CurrentMaintain);
                        DialogResult result = callNextForm.ShowDialog(this);
                        if (result == DialogResult.OK)
                        {
                            this.NotInPO = callNextForm.NotInPo;
                            callNextForm.Dispose();
                            this.CalaulateData(string.Format("e.ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["WKNo"])));
                        }
                        else
                        {
                            callNextForm.Dispose();
                        }
                    }
                    else
                    {
                        this.CalaulateData(string.Format("e.ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["WKNo"])));
                    }
                }
            }
        }

        private bool QueryNonNLData(string sqlWhere)
        {
            // 先將tmpe table清空
            this.NoNLCode.Clear();
            this.NotInPO.Clear();
            this.UnitNotFound.Clear();
            string sqlCmd;
            #region 組撈資料Sql
            if (MyUtility.Convert.GetString(this.CurrentMaintain["IsFtyExport"]).ToUpper() == "TRUE")
            {
                if (this.localPurchase)
                {
                    sqlCmd = string.Format(
                        @"
select e.ID
	   , ed.POID
	   , Seq1 = ''
	   , Seq2 = '' 
	   , OriImportQty = IIF(ed.UnitID = 'CONE', li.MeterToCone, 1) * ed.Qty 
	   , OriUnit = IIF(ed.UnitID = 'CONE', 'M', ed.UnitID)
	   , SCIRefno = ed.RefNo
	   , ed.RefNo
	   , BrandID = isnull(f.BrandID,'')
	   , Type = ed.MtlTypeID
	   , li.Description
	   , DescDetail = ''
	   , NLCode = isnull(li.NLCode2,'')
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
	   , f.UsageUnit
from FtyExport e WITH (NOLOCK) 
inner join FtyExport_Detail ed WITH (NOLOCK) on e.ID = ed.ID
left join LocalItem li WITH (NOLOCK) on li.RefNo = ed.RefNo and li.LocalSuppid = ed.SuppID
left join orders o with(nolock) on o.id = ed.POID
left join brand b with(nolock) on b.id = o.BrandID
outer apply(
    select top 1 f1.* 
    from Fabric f1 with(nolock) 
    inner join brand b2 with(nolock) on f1.BrandID = b2.id 
    where f1.Refno = ed.Refno and b2.BrandGroup = b.BrandGroup and f1.Type = ed.FabricType
    order by f1.NLCodeEditDate desc
)f
where {0}", sqlWhere);
                }
                else
                {
                    sqlCmd = string.Format(
                        @"
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
	   , NLCode = isnull(f.NLCode2, '') 
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
	   , f.UsageUnit
from FtyExport e WITH (NOLOCK) 
inner join FtyExport_Detail ed WITH (NOLOCK) on e.ID = ed.ID
left join PO_Supp_Detail psd WITH (NOLOCK) on psd.ID = ed.PoID and psd.SEQ1 = ed.Seq1 and psd.SEQ2 = ed.Seq2
left join orders o with(nolock) on o.id = ed.POID
left join brand b with(nolock) on b.id = o.BrandID
outer apply(
    select top 1 f1.* 
    from Fabric f1 with(nolock) 
    inner join brand b2 with(nolock) on f1.BrandID = b2.id 
    where f1.Refno = ed.Refno and b2.BrandGroup = b.BrandGroup and f1.Type = ed.FabricType
    order by f1.NLCodeEditDate desc
)f
where {0}", sqlWhere);
                }
            }
            else
            {
                sqlCmd = string.Format(
                    @"
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
	   , NLCode = isnull(f.NLCode2, '') 
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
	   , f.UsageUnit
from Export e WITH (NOLOCK) 
inner join Export_Detail ed WITH (NOLOCK) on e.ID = ed.ID
left join PO_Supp_Detail psd WITH (NOLOCK) on psd.ID = ed.PoID and psd.SEQ1 = ed.Seq1 and psd.SEQ2 = ed.Seq2
left join orders o with(nolock) on o.id = ed.POID
left join brand b with(nolock) on b.id = o.BrandID
outer apply(
    select top 1 f1.* 
    from Fabric f1 with(nolock) 
    inner join brand b2 with(nolock) on f1.BrandID = b2.id 
    where f1.Refno = ed.Refno and b2.BrandGroup = b.BrandGroup and f1.Type = ed.FabricType
    order by f1.NLCodeEditDate desc
)f
where {0}", sqlWhere);
            }
            #endregion
            DataTable tmpData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out tmpData);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Query data fail!!\r\n" + result.ToString());
                return false;
            }

            foreach (DataRow dr in tmpData.Rows)
            {
                if (MyUtility.Check.Empty(dr["POSeq"]))
                {
                    DataRow newRow = this.NotInPO.NewRow();
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
                    newRow["UsageUnit"] = dr["UsageUnit"];
                    this.NotInPO.Rows.Add(newRow);
                }
                else
                {
                    if (MyUtility.Check.Empty(dr["NLCode"]))
                    {
                        DataRow[] findrow = this.NoNLCode.Select(string.Format("SCIRefno = '{0}'", MyUtility.Convert.GetString(dr["SCIRefno"])));
                        if (findrow.Length == 0)
                        {
                            DataRow newRow = this.NoNLCode.NewRow();
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
                            newRow["UsageUnit"] = dr["UsageUnit"];
                            this.NoNLCode.Rows.Add(newRow);
                        }
                    }
                }

                if (MyUtility.Convert.GetString(dr["OriUnit"]) != MyUtility.Convert.GetString(dr["CustomsUnit"]) &&
                        (
                            (MyUtility.Convert.GetString(dr["Type"]) == "F"
                                && MyUtility.Convert.GetString(dr["CustomsUnit"]).ToUpper() == "M2"
                                && MyUtility.Check.Empty(dr["M2UnitRate"])) ||
                            (MyUtility.Convert.GetString(dr["Type"]) == "F"
                                && MyUtility.Convert.GetString(dr["CustomsUnit"]).ToUpper() != "M2"
                                && MyUtility.Check.Empty(dr["UnitRate"])) ||
                            (MyUtility.Convert.GetString(dr["Type"]) == "A"
                                && MyUtility.Convert.GetString(dr["CustomsUnit"]).ToUpper() == "M"
                                && dr["PcsLength"].ToString().EqualDecimal(0)
                                && MyUtility.Check.Empty(dr["UnitRate"])) ||
                            (MyUtility.Convert.GetString(dr["Type"]) == "A"
                                && MyUtility.Convert.GetString(dr["CustomsUnit"]).ToUpper() == "M2"
                                && MyUtility.Convert.GetString(dr["OriUnit"]).ToUpper() != "M"
                                && MyUtility.Check.Empty(dr["UnitRate"])
                                && dr["PcsLength"].ToString().EqualDecimal(0))))
                {
                    DataRow[] findrow = this.UnitNotFound.Select(string.Format("OriUnit = '{0}' and CustomsUnit = '{1}'", MyUtility.Convert.GetString(dr["OriUnit"]), MyUtility.Convert.GetString(dr["CustomsUnit"])));
                    if (findrow.Length == 0)
                    {
                        DataRow newRow = this.UnitNotFound.NewRow();
                        newRow["OriUnit"] = dr["OriUnit"];
                        newRow["CustomsUnit"] = dr["CustomsUnit"];
                        newRow["Refno"] = dr["Refno"];
                        this.UnitNotFound.Rows.Add(newRow);
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
            if (MyUtility.Convert.GetString(this.CurrentMaintain["IsFtyExport"]).ToUpper() == "TRUE")
            {
                if (this.localPurchase)
                {
                    sqlCmd.Append(string.Format(
                        @"
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
	       , NLCode = isnull(li.NLCode2, '') 
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
            , ed.RefNo 
            , f.Brandid
	        , f.UsageUnit
    from FtyExport e WITH (NOLOCK) 
    inner join FtyExport_Detail ed WITH (NOLOCK) on e.ID = ed.ID
    left join LocalItem li WITH (NOLOCK) on li.RefNo = ed.RefNo and li.LocalSuppid = ed.SuppID
    left join orders o with(nolock) on o.id = ed.POID
    left join brand b with(nolock) on b.id = o.BrandID
    outer apply(
        select top 1 f1.* 
        from Fabric f1 with(nolock) 
        inner join brand b2 with(nolock) on f1.BrandID = b2.id 
        where f1.Refno = ed.Refno and b2.BrandGroup = b.BrandGroup and f1.Type = ed.FabricType
        order by f1.NLCodeEditDate desc
    )f
    where {0}", sqlWhere));
                }
                else
                {
                    sqlCmd.Append(string.Format(
                        @"
    select e.ID
	       , ed.PoID
	       , ed.Seq1
	       , ed.Seq2
	       , OriImportQty = ed.qty 
	       , OriUnit = ed.UnitId
	       , Type = isnull(f.Type, '')
	       , Price = ed.Price * (select Rate 
	   						     from dbo.GetCurrencyRate('20', ed.CurrencyID, 'USD', e.AddDate))
	       , NLCode = isnull(f.NLCode2,'')
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
            , f.RefNo 
            , f.BrandID
	        , f.UsageUnit
    from FtyExport e WITH (NOLOCK) 
    inner join FtyExport_Detail ed WITH (NOLOCK) on e.ID = ed.ID
    left join PO_Supp_Detail psd WITH (NOLOCK) on psd.ID = ed.PoID and psd.SEQ1 = ed.Seq1 and psd.SEQ2 = ed.Seq2
    left join orders o with(nolock) on o.id = ed.POID
    left join brand b with(nolock) on b.id = o.BrandID
    outer apply(
        select top 1 f1.* 
        from Fabric f1 with(nolock) 
        inner join brand b2 with(nolock) on f1.BrandID = b2.id 
        where f1.Refno = ed.Refno and b2.BrandGroup = b.BrandGroup and f1.Type = ed.FabricType
        order by f1.NLCodeEditDate desc
    )f
    where {0}", sqlWhere));
                }
            }
            else
            {
                sqlCmd.Append(string.Format(
                    @"
    select e.ID
	       , ed.PoID
	       , ed.Seq1
	       , ed.Seq2
	       , OriImportQty = ed.qty + ed.Foc
	       , OriUnit = ed.UnitId
	       , Type = isnull(f.Type,'')
	       , Price = ed.Price * (select Rate 
	   						     from dbo.GetCurrencyRate('20', ed.CurrencyID, 'USD', e.CloseDate)) 
	       , NLCode = isnull(f.NLCode2, '') 
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
            , f.RefNo 
            , f.BrandID
	        , f.UsageUnit
    from Export e WITH (NOLOCK) 
    inner join Export_Detail ed WITH (NOLOCK) on e.ID = ed.ID
    left join PO_Supp_Detail psd WITH (NOLOCK) on psd.ID = ed.PoID and psd.SEQ1 = ed.Seq1 and psd.SEQ2 = ed.Seq2
    left join orders o with(nolock) on o.id = ed.POID
    left join brand b with(nolock) on b.id = o.BrandID
    outer apply(
        select top 1 f1.* 
        from Fabric f1 with(nolock) 
        inner join brand b2 with(nolock) on f1.BrandID = b2.id 
        where f1.Refno = ed.Refno and b2.BrandGroup = b.BrandGroup and f1.Type = ed.FabricType
        order by f1.NLCodeEditDate desc
    )f

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
           , f.RefNo
           , f.BrandID
	       , f.UsageUnit
	from #tmp
    left join PO_Supp_Detail psd WITH (NOLOCK) on psd.ID = #tmp.POID and psd.SEQ1 = #tmp.Seq1 and psd.SEQ2 = #tmp.Seq2
    left join orders o with(nolock) on o.id = psd.ID
    left join brand b with(nolock) on b.id = o.BrandID
    outer apply(
        select top 1 f1.* 
        from Fabric f1 with(nolock) 
        inner join brand b2 with(nolock) on f1.BrandID = b2.id 
        where f1.Refno = psd.Refno and b2.BrandGroup = b.BrandGroup and f1.Type = psd.FabricType
        order by f1.NLCodeEditDate desc
    )f
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
       , RefNo
       , Type
       , BrandID
       , UsageUnit
	   , sum(NewQty) as NewQty
	   , sum(NewQty * Price) as Price 
from (
	select NLCode
	       , HSCode
	       , CustomsUnit
           , Price
           , RefNo
           , Type
           , BrandID
           , UsageUnit
		   , NewQty = [dbo].getVNUnitTransfer(Type, OriUnit, CustomsUnit, OriImportQty, Width, PcsWidth, PcsLength, PcsKg, IIF(CustomsUnit = 'M2', M2RateValue, RateValue), IIF(CustomsUnit = 'M2', M2UnitRate, UnitRate),Refno)
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
           , RefNo
           , Type
           , BrandID
           , UsageUnit
		   , NewQty = [dbo].getVNUnitTransfer(Type, OriUnit, CustomsUnit, OriImportQty, Width, PcsWidth, PcsLength, PcsKg, IIF(CustomsUnit = 'M2', M2RateValue, RateValue), IIF(CustomsUnit = 'M2', M2UnitRate, UnitRate),Refno)
    from NotInPo WITH (NOLOCK) 
) a
group by NLCode, HSCode, CustomsUnit, RefNo,Type,BrandID,UsageUnit");

            DataTable selectedData;
            DualResult result = MyUtility.Tool.ProcessWithDatatable(this.NotInPO, null, sqlCmd.ToString(), out selectedData);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Calculate fail!!\r\n" + result.ToString());
                return;
            }

            // 將Not in po的資料加入
            DataTable groupNoInPOData;

            // 將資料做排序
            result = MyUtility.Tool.ProcessWithDatatable(
                    selectedData,
                    @"NLCode,HSCode,CustomsUnit,NewQty,Price,RefNo,Type,BrandID,UsageUnit",
                    string.Format(@"
select NLCode
	   , HSCode
	   , CustomsUnit
	   , NewQty
       , RefNo
       , Type
       , BrandID = isnull(BrandID,'')
       , UsageUnit
	   , Price = iif(NewQty=0,0,Price / NewQty)
from #tmp
order by TRY_CONVERT(int, SUBSTRING(NLCode, 3, LEN(NLCode))), NLCode"),
                    out groupNoInPOData);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Calculate Not in PO Data fail!!\r\n" + result.ToString());
                return;
            }

            // 刪除表身Grid資料
            foreach (DataRow dr in this.DetailDatas)
            {
                dr.Delete();
            }

            // 將資料填入表身Grid中
            foreach (DataRow dr in groupNoInPOData.Rows)
            {
                DataRow newRow = ((DataTable)this.detailgridbs.DataSource).NewRow();
                newRow["ID"] = string.Empty;
                newRow["Refno"] = dr["Refno"];
                newRow["BrandID"] = dr["BrandID"];
                newRow["FabricType"] = dr["Type"];
                newRow["NLCode"] = dr["NLCode"];
                newRow["HSCode"] = dr["HSCode"];
                newRow["Qty"] = dr["NewQty"];
                newRow["UnitID"] = dr["CustomsUnit"];
                newRow["Price"] = dr["Price"];
                newRow["UsageUnit"] = dr["UsageUnit"];
                newRow["Remark"] = string.Empty;
                ((DataTable)this.detailgridbs.DataSource).Rows.Add(newRow);
            }

            if (this.QueryNonNLData(sqlWhere))
            {
                StringBuilder wrongData = new StringBuilder();
                if (this.NoNLCode.Rows.Count > 0)
                {
                    wrongData.Append("Below data is no Customs Code in B40, B41:\r\n");
                    foreach (DataRow dr in this.NoNLCode.Rows)
                    {
                        wrongData.Append(string.Format("RefNo: {0}, Brand: {1}\r\n", MyUtility.Convert.GetString(dr["RefNo"]), MyUtility.Convert.GetString(dr["BrandID"])));
                    }
                }

                if (this.UnitNotFound.Rows.Count > 0)
                {
                    wrongData.Append("Below data is no transfer formula. Please contact with Taipei MIS.\r\n");
                    foreach (DataRow dr in this.UnitNotFound.Rows)
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
