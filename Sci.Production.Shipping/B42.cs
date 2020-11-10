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
using Sci.Production.PublicPrg;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// B42
    /// </summary>
    public partial class B42 : Win.Tems.Input6
    {
        private DataTable tmpConsumptionArticle;
        private DataTable tmpConsumptionSizecode;

        /// <summary>
        /// B42
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public B42(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            MyUtility.Tool.SetupCombox(this.comboCategory, 2, 1, "B,Bulk,S,Sample");

            // 取VNConsumption_Article, VNConsumption_SizeCode結構，存檔時使用
            DBProxy.Current.Select(null, "select * from VNConsumption_Article WITH (NOLOCK) where 1 = 0", out this.tmpConsumptionArticle);
            DBProxy.Current.Select(null, "select * from VNConsumption_SizeCode WITH (NOLOCK) where 1 = 0", out this.tmpConsumptionSizecode);
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            // 新增Import From Barcode按鈕
            Win.UI.Button btn = new Win.UI.Button
            {
                Text = "Batch Create",
            };
            btn.Click += new EventHandler(this.Btn_Click);
            this.browsetop.Controls.Add(btn);

            // 預設是(80,30)
            btn.Size = new Size(120, 30);
            btn.Enabled = Prgs.GetAuthority(Env.User.UserID, "B42. Custom SP# and Consumption", "CanNew");
            this.grid.Columns[0].Visible = false;

            // 新增Import From Batch按鈕
            Win.UI.Button btn2 = new Win.UI.Button
            {
                Text = "Batch Import",
            };
            btn2.Click += this.Btn2_Click;
            this.browsetop.Controls.Add(btn2);

            // 預設是(80,30)
            btn2.Size = new Size(120, 30);
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : MyUtility.Convert.GetString(e.Master["ID"]);
            this.DetailSelectCommand = string.Format(
                @"
select
vdd.*,
vd.Waste
from VNConsumption_Detail_Detail vdd with (nolock)
inner join VNConsumption_Detail vd with (nolock) on vd.ID = vdd.ID and vd.NLCode = vdd.NLCode
where vdd.ID = '{0}'
                ", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        private void Btn2_Click(object sender, EventArgs e)
        {
            B42_BatchImport callNextForm = new B42_BatchImport();
            DialogResult result = callNextForm.ShowDialog(this);

            this.ReloadDatas();
        }

        // Batch Create按鈕的Click事件
        private void Btn_Click(object sender, EventArgs e)
        {
            B42_BatchCreate callNextForm = new B42_BatchCreate();
            DialogResult result = callNextForm.ShowDialog(this);

            this.ReloadDatas();
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            string colorWay = MyUtility.GetValue.Lookup(string.Format("select CONCAT(Article, ',') from VNConsumption_Article WITH (NOLOCK) where ID = '{0}' order by Article for xml path('')", MyUtility.Convert.GetString(this.CurrentMaintain["ID"])));
            this.editColorway.Text = MyUtility.Check.Empty(colorWay) ? string.Empty : colorWay.Substring(0, colorWay.Length - 1);
            string sizeGroup = MyUtility.GetValue.Lookup(string.Format("select CONCAT(SizeCode, ',') from VNConsumption_SizeCode WITH (NOLOCK) where ID = '{0}' order by SizeCode for xml path('')", MyUtility.Convert.GetString(this.CurrentMaintain["ID"])));
            this.editSizeGroup.Text = MyUtility.Check.Empty(sizeGroup) ? string.Empty : sizeGroup.Substring(0, sizeGroup.Length - 1);
        }

        private void ClearDetailDt(DataRow curDr)
        {
            curDr["NLCode"] = string.Empty;
            curDr["UsageUnit"] = string.Empty;
            curDr["Refno"] = string.Empty;
            curDr["StockUnit"] = string.Empty;
            curDr["SCIRefno"] = string.Empty;
            curDr["FabricBrandID"] = string.Empty;
            curDr["HSCode"] = string.Empty;
            curDr["UnitID"] = string.Empty;
            curDr["Qty"] = 0;
            curDr["FabricType"] = string.Empty;
            curDr["LocalItem"] = 0;
            curDr["Waste"] = 0;
            curDr.EndEdit();
        }

        private bool GetCustomRefnoInfo(DataRow drDetail, string refno, string nlCode, string usageUnit)
        {
            DataRow drNLCode;
            drNLCode = Prgs.GetNLCodeDataByRefno(refno, drDetail["UsageQty"].ToString(), this.CurrentMaintain["BrandID"].ToString(), drDetail["FabricType"].ToString(), nlCode: nlCode, usageUnit: usageUnit);

            if (drNLCode == null)
            {
                return false;
            }
            else
            {
                drDetail["NLCode"] = drNLCode["NLCode"];
                drDetail["StockUnit"] = drNLCode["StockUnit"];
                drDetail["UsageUnit"] = drNLCode["UsageUnit"];
                drDetail["StockQty"] = drNLCode["StockQty"];
                drDetail["SCIRefno"] = drNLCode["SCIRefno"];
                drDetail["FabricBrandID"] = drNLCode["FabricBrandID"];
                drDetail["HSCode"] = drNLCode["HSCode"];
                drDetail["UnitID"] = drNLCode["UnitID"];
                drDetail["Qty"] = drNLCode["Qty"];
                drDetail["UserCreate"] = 1;
                drDetail["FabricType"] = drNLCode["FabricType"];
                drDetail["LocalItem"] = drNLCode["LocalItem"];
                drDetail["Refno"] = refno;
                drDetail["Waste"] = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup(string.Format(
                    @"select [dbo].[getWaste]('{0}','{1}','{2}','{3}','{4}')",
                    MyUtility.Convert.GetString(this.CurrentMaintain["StyleID"]),
                    MyUtility.Convert.GetString(this.CurrentMaintain["BrandID"]),
                    MyUtility.Convert.GetString(this.CurrentMaintain["SeasonID"]),
                    MyUtility.Convert.GetString(this.CurrentMaintain["VNContractID"]),
                    MyUtility.Convert.GetString(drDetail["NLCode"]))));
            }

            return true;
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            #region Type column set
            DataGridViewGeneratorComboBoxColumnSettings typeCol = new DataGridViewGeneratorComboBoxColumnSettings();

            Dictionary<string, string> resultCombo = new Dictionary<string, string>();
            resultCombo.Add("A", "A");
            resultCombo.Add("F", "F");
            resultCombo.Add("L", "L");
            typeCol.DataSource = new BindingSource(resultCombo, null);
            typeCol.ValueMember = "Key";
            typeCol.DisplayMember = "Value";

            typeCol.CellEditable += (s, e) =>
            {
                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (!MyUtility.Convert.GetBool(dr["UserCreate"]))
                {
                    e.IsEditable = false;
                }
            };

            typeCol.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                string oldvalue = MyUtility.Convert.GetString(dr["FabricType"]);
                string newvalue = MyUtility.Convert.GetString(e.FormattedValue);

                if (MyUtility.Check.Empty(oldvalue))
                {
                    return;
                }

                if (newvalue == "L" || newvalue == "Misc")
                {
                    dr["usageUnit"] = string.Empty;
                }

                if (oldvalue != newvalue)
                {
                    this.ClearDetailDt(dr);
                    return;
                }
            };

            #endregion

            #region Customs Code
            DataGridViewGeneratorTextColumnSettings nlCodeCol = new DataGridViewGeneratorTextColumnSettings();
            nlCodeCol.CellEditable += (s, e) =>
            {
                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (!MyUtility.Convert.GetBool(dr["UserCreate"]))
                {
                    e.IsEditable = false;
                }
            };

            nlCodeCol.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                string oldvalue = MyUtility.Convert.GetString(dr["NLCode"]);
                string newvalue = MyUtility.Convert.GetString(e.FormattedValue);
                if (oldvalue == newvalue)
                {
                    return;
                }

                if (MyUtility.Check.Empty(newvalue))
                {
                    this.ClearDetailDt(dr);
                    return;
                }

                bool isExists = this.GetCustomRefnoInfo(dr, dr["Refno"].ToString(), newvalue, usageUnit: MyUtility.Convert.GetString(dr["usageUnit"]));
                if (!isExists)
                {
                    MyUtility.Msg.WarningBox("<Customs Code> not found!");
                    e.Cancel = true;
                }
            };

            #endregion

            #region usageUnit
            DataGridViewGeneratorTextColumnSettings usageUnit = new DataGridViewGeneratorTextColumnSettings();
            usageUnit.CellEditable += (s, e) =>
            {
                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (!MyUtility.Convert.GetBool(dr["UserCreate"])
                || !(MyUtility.Convert.GetString(dr["FabricType"]) == "F" || MyUtility.Convert.GetString(dr["FabricType"]) == "A"))
                {
                    e.IsEditable = false;
                }
            };

            usageUnit.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                string oldvalue = MyUtility.Convert.GetString(dr["usageUnit"]);
                string newvalue = MyUtility.Convert.GetString(e.FormattedValue);
                if (oldvalue == newvalue)
                {
                    return;
                }

                if (MyUtility.Check.Empty(newvalue))
                {
                    this.ClearDetailDt(dr);
                    return;
                }

                if ((MyUtility.Convert.GetString(dr["FabricType"]) == "F" || MyUtility.Convert.GetString(dr["FabricType"]) == "A") &&
                !MyUtility.Check.Empty(dr["NLCode"]) && !MyUtility.Check.Empty(dr["RefNo"]))
                {
                    bool isExists = this.GetCustomRefnoInfo(dr, dr["RefNo"].ToString(), dr["NLCode"].ToString(), usageUnit: newvalue);
                    if (!isExists)
                    {
                        MyUtility.Msg.WarningBox("<Usage Unit> not found!");
                        e.Cancel = true;
                    }
                }
            };
            #endregion

            #region Refno
            DataGridViewGeneratorTextColumnSettings refnoCol = new DataGridViewGeneratorTextColumnSettings();

            refnoCol.CellEditable += (s, e) =>
            {
                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (!MyUtility.Convert.GetBool(dr["UserCreate"]))
                {
                    e.IsEditable = false;
                }
            };

            refnoCol.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                string oldvalue = MyUtility.Convert.GetString(dr["Refno"]);
                string newvalue = MyUtility.Convert.GetString(e.FormattedValue);
                if (oldvalue == newvalue)
                {
                    return;
                }

                if (MyUtility.Check.Empty(newvalue))
                {
                    this.ClearDetailDt(dr);
                    return;
                }

                bool isExists = this.GetCustomRefnoInfo(dr, newvalue, string.Empty, usageUnit: MyUtility.Convert.GetString(dr["usageUnit"]));
                if (!isExists)
                {
                    MyUtility.Msg.WarningBox("<Ref No> not found!");
                    e.Cancel = true;
                }
            };
            #endregion
            #region Stock Qty
            DataGridViewGeneratorNumericColumnSettings usageQtyCol = new DataGridViewGeneratorNumericColumnSettings();
            usageQtyCol.CellEditable += (s, e) =>
            {
                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);

                bool isFixData = MyUtility.Check.Seek($"select 1 from VNFixedDeclareItem where Refno = '{dr["Refno"].ToString()}'");

                if (MyUtility.Check.Empty(dr["SCIRefno"]) && isFixData)
                {
                    e.IsEditable = false;
                }
            };

            usageQtyCol.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                string oldvalue = MyUtility.Convert.GetString(dr["UsageQty"]);
                string newvalue = MyUtility.Convert.GetString(e.FormattedValue);
                if (oldvalue == newvalue)
                {
                    return;
                }

                DataRow drNLCode;
                drNLCode = Prgs.GetNLCodeDataByRefno(dr["RefNo"].ToString(), newvalue, this.CurrentMaintain["BrandID"].ToString(), dr["FabricType"].ToString(), dr["SciRefNo"].ToString(), usageUnit: MyUtility.Convert.GetString(dr["usageUnit"]));
                if (drNLCode == null)
                {
                    MyUtility.Msg.WarningBox("<Customs Qty> Transfer error");
                }
                else
                {
                    dr["Qty"] = drNLCode["Qty"];
                    dr["StockUnit"] = drNLCode["StockUnit"];
                    dr["StockQty"] = drNLCode["StockQty"];
                    dr["UsageQty"] = newvalue;
                }
            };
            #endregion

            Ict.Win.UI.DataGridViewNumericBoxColumn qtyColumn;
            Ict.Win.UI.DataGridViewNumericBoxColumn systemQtyColumn;
            base.OnDetailGridSetup();
            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("NLCode", header: "Customs Code", width: Widths.AnsiChars(7), settings: nlCodeCol)
                .ComboBox("FabricType", header: "Type", width: Widths.AnsiChars(3), settings: typeCol)
                .Text("UsageUnit", header: "UsageUnit", width: Widths.AnsiChars(8), settings: usageUnit)
                .Text("RefNo", header: "Ref No", width: Widths.AnsiChars(20), settings: refnoCol)
                .Text("UnitID", header: "Customs Unit", width: Widths.AnsiChars(7), iseditingreadonly: true)
                .Numeric("SystemQty", header: "System Qty", decimal_places: 6, width: Widths.AnsiChars(15), iseditingreadonly: true).Get(out systemQtyColumn)
                .Numeric("UsageQty", header: " Act. Qty", decimal_places: 6, width: Widths.AnsiChars(15), settings: usageQtyCol).Get(out qtyColumn)
                .Numeric("Qty", header: "Customs Qty", decimal_places: 6, width: Widths.AnsiChars(15), iseditingreadonly: true).Get(out qtyColumn)
                .Numeric("Waste", header: "Waste", decimal_places: 6, iseditingreadonly: true)
                .CheckBox("UserCreate", header: "Create by user", width: Widths.AnsiChars(3), iseditable: false, trueValue: 1, falseValue: 0);

            qtyColumn.DecimalZeroize = Ict.Win.UI.NumericBoxDecimalZeroize.Default;
            systemQtyColumn.DecimalZeroize = Ict.Win.UI.NumericBoxDecimalZeroize.Default;
        }

        /// <inheritdoc/>
        protected override void EnsureToolbarExt()
        {
            base.EnsureToolbarExt();
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["Status"] = "New";
            this.CurrentMaintain["Category"] = "B";
            this.CurrentMaintain["VNContractID"] = MyUtility.GetValue.Lookup(@"
select  ID 
from VNContract WITH (NOLOCK) 
where StartDate = (select   MAX(StartDate) 
                   from VNContract WITH (NOLOCK) 
                   where    GETDATE() between StartDate and EndDate 
                            and Status = 'Confirmed')
and IsSubConIn = 0
");
            this.CurrentMaintain["CustomSP"] = "SP" + MyUtility.Convert.GetString(MyUtility.Convert.GetInt(MyUtility.GetValue.Lookup(string.Format(
                @"
select  CustomSP = isnull (MAX (CustomSP), 'SP000000') 
from VNConsumption WITH (NOLOCK) 
where VNContractID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["VNContractID"]))).Substring(2)) + 1).PadLeft(6, '0');

            this.CurrentMaintain["VNMultiple"] = MyUtility.GetValue.Lookup(@"
select  VNMultiple 
from System WITH (NOLOCK) ");
        }

        /// <inheritdoc/>
        protected override bool ClickEditBefore()
        {
            if (MyUtility.Convert.GetString(this.CurrentMaintain["Status"]).ToUpper() == "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("This record already confirmed, can't edit!!");
                return false;
            }

            // Contract No.  IsSubConIn = true,則不能編輯

            string sqlcmd = $@"select * from VNContract where IsSubconIn = 1 and ID='{this.CurrentMaintain["VNContractID"]}'";
            if (MyUtility.Check.Seek(sqlcmd))
            {
                MyUtility.Msg.WarningBox("This record is SubconIn, cannot edit!!");
                return false;
            }

            return base.ClickEditBefore();
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtCustomSPNo.ReadOnly = true;
            this.txtContractNo.ReadOnly = true;
            this.dateDate.ReadOnly = true;
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
        protected override DualResult ClickDeletePost()
        {
            IList<string> deleteCmds = new List<string>();
            deleteCmds.Add(string.Format("delete VNConsumption_Article where ID = '{0}';", MyUtility.Convert.GetString(this.CurrentMaintain["ID"])));
            deleteCmds.Add(string.Format("delete VNConsumption_SizeCode where ID = '{0}';", MyUtility.Convert.GetString(this.CurrentMaintain["ID"])));
            deleteCmds.Add(string.Format("delete VNConsumption_Detail where ID = '{0}';", MyUtility.Convert.GetString(this.CurrentMaintain["ID"])));
            return DBProxy.Current.Executes(null, deleteCmds);
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            #region 檢查必輸欄位
            if (MyUtility.Check.Empty(this.CurrentMaintain["CustomSP"]))
            {
                this.txtCustomSPNo.Focus();
                MyUtility.Msg.WarningBox("Custom SP# can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["VNContractID"]))
            {
                this.txtContractNo.Focus();
                MyUtility.Msg.WarningBox("Contract no. can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["CDate"]))
            {
                this.dateDate.Focus();
                MyUtility.Msg.WarningBox("Date can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["StyleID"]))
            {
                this.txtStyle.Focus();
                MyUtility.Msg.WarningBox("Style can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["Category"]))
            {
                this.comboCategory.Focus();
                MyUtility.Msg.WarningBox("Category can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["SizeCode"]))
            {
                this.txtSize.Focus();
                MyUtility.Msg.WarningBox("Size can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(this.editColorway.Text))
            {
                this.editColorway.Focus();
                MyUtility.Msg.WarningBox("Color way can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(this.editSizeGroup.Text))
            {
                this.editSizeGroup.Focus();
                MyUtility.Msg.WarningBox("Size Group can't empty!!");
                return false;
            }

            // 檢查Refno是否存在
            foreach (DataRow dr in this.DetailDatas)
            {
                bool isExists = false;
                if (dr["FabricType"].Equals("L"))
                {
                    isExists = MyUtility.Check.Seek($"select 1 from LocalItem with (nolock) where Refno = '{dr["Refno"]}' and NLCode = '{dr["NLCode"]}'");
                }
                else
                {
                    isExists = MyUtility.Check.Seek($"select 1 from Fabric with (nolock) where Refno = '{dr["Refno"]}' and NLCode = '{dr["NLCode"]}'");
                }

                if (!isExists)
                {
                    MyUtility.Msg.WarningBox($"<Refno>{dr["Refno"]}, <Customs Code>{dr["NLCode"]} not found");
                    return false;
                }
            }

            #endregion

            // 紀錄表身資料筆數
            int detailRecord = 0;

            #region 刪除表身Qty為0的資料
            foreach (DataRow dr in this.DetailDatas)
            {
                if (MyUtility.Check.Empty(dr["Qty"]))
                {
                    dr.Delete();
                    continue;
                }

                detailRecord++;
            }
            #endregion

            #region 表身資料不可為空
            if (detailRecord == 0)
            {
                MyUtility.Msg.WarningBox("Detail can't empty!!");
                return false;
            }
            #endregion

            #region 檢查ID,NLCode,HSCode,UnitID Group後是否有ID,NLCode重複的資料
            DataTable dtDetail = (DataTable)this.detailgridbs.DataSource;
            DataRow[] queryData = ((DataTable)this.detailgridbs.DataSource).Select("1=1");
            bool isVNConsumption_Detail_DetailHasDupData = !Prgs.CheckVNConsumption_Detail_Dup(queryData, false);
            if (isVNConsumption_Detail_DetailHasDupData)
            {
                return false;
            }
            #endregion

            // Get ID && Get Version
            if (this.IsDetailInserting)
            {
                string newID = MyUtility.GetValue.GetID(Env.User.Keyword + "SP", "VNConsumption", Convert.ToDateTime(this.CurrentMaintain["CDate"]), 2, "ID", null);
                if (MyUtility.Check.Empty(newID))
                {
                    MyUtility.Msg.WarningBox("GetID fail, please try again!");
                    return false;
                }

                this.CurrentMaintain["ID"] = newID;

                string maxVersion = MyUtility.GetValue.Lookup(string.Format("select isnull(MAX(Version),0) as MaxVersion from VNConsumption WITH (NOLOCK) where StyleUKey = {0}", MyUtility.Convert.GetString(this.CurrentMaintain["StyleUKey"])));
                this.CurrentMaintain["Version"] = MyUtility.Convert.GetString(MyUtility.Convert.GetInt(maxVersion) + 1).PadLeft(3, '0');
            }

            // 準備資料
            this.tmpConsumptionArticle.Clear();
            this.tmpConsumptionSizecode.Clear();
            string[] colorway = this.editColorway.Text.Split(',');
            string[] sizecode = this.editSizeGroup.Text.Split(',');
            foreach (string s in colorway)
            {
                DataRow dr = this.tmpConsumptionArticle.NewRow();
                dr["ID"] = this.CurrentMaintain["ID"];
                dr["Article"] = s;
                this.tmpConsumptionArticle.Rows.Add(dr);
            }

            foreach (string s in sizecode)
            {
                DataRow dr = this.tmpConsumptionSizecode.NewRow();
                dr["ID"] = this.CurrentMaintain["ID"];
                dr["SizeCode"] = s;
                this.tmpConsumptionSizecode.Rows.Add(dr);
            }

            #region 計算出 Qty(現有欄位)時, 將數值一併寫到 SystemQty 欄位
            foreach (DataRow dr in ((DataTable)((BindingSource)this.detailgrid.DataSource).DataSource).Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    if (Convert.ToBoolean(dr["UserCreate"]))
                    {
                        dr["SystemQty"] = dr["Qty"];
                        dr.EndEdit();
                    }
                }
            }
            #endregion

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override DualResult ClickSavePost()
        {
            // 存 VNConsumption_Article, VNConsumption_SizeCode資料
            if (!MyUtility.Tool.CursorUpdateTable(this.tmpConsumptionArticle, "VNConsumption_Article", "Production"))
            {
                DualResult failResult = new DualResult(false, "Save VNConsumption_Article fail!!");
                return failResult;
            }

            if (!MyUtility.Tool.CursorUpdateTable(this.tmpConsumptionSizecode, "VNConsumption_SizeCode", "Production"))
            {
                DualResult failResult = new DualResult(false, "Save VNConsumption_SizeCode fail!!");
                return failResult;
            }

            // 回寫VNConsumption_Detail
            string sqlCreateVNConsumption_Detail = $" exec CreateVNConsumption_Detail '{this.CurrentMaintain["ID"].ToString()}'";
            DualResult isCreateVNConsumption_DetailOK = DBProxy.Current.Execute(null, sqlCreateVNConsumption_Detail);
            if (!isCreateVNConsumption_DetailOK)
            {
                return isCreateVNConsumption_DetailOK;
            }

            return base.ClickSavePost();
        }

        /// <inheritdoc/>
        protected override DualResult ClickSave()
        {
            foreach (DataRow dr in ((DataTable)((BindingSource)this.detailgrid.DataSource).DataSource).Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    if (MyUtility.Check.Empty(dr["Waste"]))
                    {
                        DataRow drs;
                        if (MyUtility.Check.Seek(
                            $@"
select [dbo].[getWaste]( '{this.CurrentMaintain["StyleID"]}','{this.CurrentMaintain["BrandID"]}','{this.CurrentMaintain["SeasonID"]}','{this.CurrentMaintain["VNContractID"]}','{dr["NLCode"]}') as Waste", out drs))
                        {
                            dr["Waste"] = drs["Waste"];
                            dr.EndEdit();
                        }
                    }
                }
            }

            return base.ClickSave();
        }

        /// <inheritdoc/>
        protected override void OnDetailGridInsertClick()
        {
            base.OnDetailGridInsertClick();
            DataRow newrow = this.detailgrid.GetDataRow(this.detailgrid.GetSelectedRowIndex());
            newrow["UserCreate"] = 1;
        }

        /// <inheritdoc/>
        protected override void OnDetailGridAppendClick()
        {
            base.OnDetailGridAppendClick();
            DataRow newrow = this.detailgrid.GetDataRow(this.detailgrid.GetSelectedRowIndex());
            newrow["UserCreate"] = 1;
        }

        /// <inheritdoc/>
        protected override bool ClickPrint()
        {
            B42_Print callPurchaseForm = new B42_Print();
            callPurchaseForm.ShowDialog(this);
            return base.ClickPrint();
        }

        // Contract No.
        private void TxtContractNo_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            if (this.EditMode)
            {
                string sqlcmd = "select ID,StartDate,EndDate from VNContract WITH (NOLOCK) where GETDATE() between StartDate and EndDate and Status = 'Confirmed' and IsSubconIn = 0";

                Win.Tools.SelectItem item = new Win.Tools.SelectItem(sqlcmd, "15,10,10", this.txtContractNo.Text, headercaptions: "Contract No.,Start Date, End Date");
                DialogResult returnResult = item.ShowDialog();
                if (returnResult == DialogResult.Cancel)
                {
                    return;
                }

                this.txtContractNo.Text = item.GetSelectedString();
            }
        }

        // Contract No.
        private void TxtContractNo_Validating(object sender, CancelEventArgs e)
        {
            if (this.EditMode && this.txtContractNo.OldValue != this.txtContractNo.Text && !MyUtility.Check.Empty(this.txtContractNo.Text))
            {
                if (!MyUtility.Check.Seek(string.Format("select ID from VNContract WITH (NOLOCK) where ID = '{0}' and IsSubconIn = 0", this.txtContractNo.Text)))
                {
                    this.txtContractNo.Text = string.Empty;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox("Contract no. not found!!");
                    return;
                }
                else if (!MyUtility.Check.Seek(string.Format("select ID from VNContract WITH (NOLOCK) where ID = '{0}' and IsSubconIn = 0 and GETDATE() between StartDate and EndDate", this.txtContractNo.Text)))
                {
                    this.txtContractNo.Text = string.Empty;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox("This Contract can't use.");
                    return;
                }
            }
        }

        // Style
        private void TxtStyle_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            if (this.EditMode)
            {
                Win.Tools.SelectItem item = new Win.Tools.SelectItem("Select ID, SeasonID, BrandID,Ukey,CPU from Style WITH (NOLOCK) Order By BrandID, ID, SeasonID", "15,10,10,0", this.txtContractNo.Text, headercaptions: "Contract No.,Start Date, End Date,");
                DialogResult returnResult = item.ShowDialog();
                if (returnResult == DialogResult.Cancel)
                {
                    return;
                }

                IList<DataRow> selectedData = item.GetSelecteds();
                this.CurrentMaintain["StyleID"] = item.GetSelectedString();
                this.CurrentMaintain["SeasonID"] = selectedData[0]["SeasonID"];
                this.CurrentMaintain["BrandID"] = selectedData[0]["BrandID"];
                this.CurrentMaintain["StyleUKey"] = selectedData[0]["Ukey"];
                this.CurrentMaintain["CPU"] = selectedData[0]["CPU"];
            }
        }

        // Size
        private void TxtSize_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            if (this.EditMode)
            {
                Win.Tools.SelectItem item = new Win.Tools.SelectItem(string.Format("select SizeCode from Style_SizeCode WITH (NOLOCK) where {0} order by Seq", MyUtility.Check.Empty(this.CurrentMaintain["StyleUKey"]) ? "1=0" : "StyleUkey = " + MyUtility.Convert.GetString(this.CurrentMaintain["StyleUKey"])), "15,10,10,0", this.txtStyle.Text, headercaptions: "Size");
                DialogResult returnResult = item.ShowDialog();
                if (returnResult == DialogResult.Cancel)
                {
                    return;
                }

                this.CurrentMaintain["SizeCode"] = item.GetSelectedString();
            }
        }

        // Color Way
        private void EditColorway_PopUp(object sender, Win.UI.EditBoxPopUpEventArgs e)
        {
            if (this.EditMode)
            {
                Win.Tools.SelectItem2 item = new Win.Tools.SelectItem2(string.Format("select Article from Style_Article WITH (NOLOCK) where {0} order by Seq", MyUtility.Check.Empty(this.CurrentMaintain["StyleUKey"]) ? "1=0" : "StyleUkey = " + MyUtility.Convert.GetString(this.CurrentMaintain["StyleUKey"])), "Color Way", "8", this.editColorway.Text, null, null, null);
                DialogResult returnResult = item.ShowDialog();
                if (returnResult == DialogResult.Cancel)
                {
                    return;
                }

                this.editColorway.Text = item.GetSelectedString();
            }
        }

        // Size Group
        private void EditSizeGroup_PopUp(object sender, Win.UI.EditBoxPopUpEventArgs e)
        {
            if (this.EditMode)
            {
                Win.Tools.SelectItem2 item = new Win.Tools.SelectItem2(string.Format("select SizeCode from Style_SizeCode WITH (NOLOCK) where {0} order by Seq", MyUtility.Check.Empty(this.CurrentMaintain["StyleUKey"]) ? "1=0" : "StyleUkey = " + MyUtility.Convert.GetString(this.CurrentMaintain["StyleUKey"])), "Color Way", "8", this.editSizeGroup.Text, null, null, null);
                DialogResult returnResult = item.ShowDialog();
                if (returnResult == DialogResult.Cancel)
                {
                    return;
                }

                this.editSizeGroup.Text = item.GetSelectedString();
            }
        }

        // Sketch
        private void BtnSketch_Click(object sender, EventArgs e)
        {
            IE.P01_Sketch callNextForm = new IE.P01_Sketch(this.CurrentMaintain);
            DialogResult result = callNextForm.ShowDialog(this);
        }

        // Calculate
        private void BtnCalculate_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["StyleID"]) || MyUtility.Check.Empty(this.CurrentMaintain["Category"]) || MyUtility.Check.Empty(this.CurrentMaintain["SizeCode"]) || MyUtility.Check.Empty(this.editColorway.Text))
            {
                MyUtility.Msg.WarningBox("Style, Category, Size and Color way can't empty!!");
                return;
            }

            DataTable queryDetail2Data, necessaryItem, invalidData;
            StringBuilder sqlCmd = new StringBuilder();
            StringBuilder needItem = new StringBuilder();
            StringBuilder emptyNLCode = new StringBuilder();
            StringBuilder wrongUnit = new StringBuilder();
            StringBuilder allMessage = new StringBuilder();
            string[] colorway = this.editColorway.Text.Split(',');

            #region 取得VNConsumption_Detail_Detail資料
            Prgs.ParGetVNConsumption_Detail_Detail parData = new Prgs.ParGetVNConsumption_Detail_Detail
            {
                StyleUkey = MyUtility.Convert.GetString(this.CurrentMaintain["StyleUKey"]),
                SizeCode = MyUtility.Convert.GetString(this.CurrentMaintain["SizeCode"]),
                Article = colorway[0],
                Category = this.comboCategory.Text,
                ContractID = this.CurrentMaintain["VNContractID"].ToString(),
            };
            DualResult result = Prgs.GetVNConsumption_Detail_Detail(parData, out queryDetail2Data);
            if (!result)
            {
                MyUtility.Msg.WarningBox(string.Format("Query detail data fail.\r\n{0}", result.ToString()));
                return;
            }
            #endregion

            sqlCmd.Clear();
            #region 檢查是否有每次都該要出現的項目
            sqlCmd.Append(string.Format("select NLCode from VNContract_Detail WITH (NOLOCK) where ID = '{0}' and NecessaryItem = 1", MyUtility.Convert.GetString(this.CurrentMaintain["VNContractID"])));
            result = DBProxy.Current.Select(null, sqlCmd.ToString(), out necessaryItem);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Query necessary item data fail!!\r\n" + result.ToString());
                return;
            }

            foreach (DataRow dr in necessaryItem.Rows)
            {
                DataRow[] findrow = queryDetail2Data.Select(string.Format("NLCode = '{0}'", MyUtility.Convert.GetString(dr["NLCode"])));
                if (findrow.Length == 0)
                {
                    needItem.Append(string.Format("{0},", MyUtility.Convert.GetString(dr["NLCode"])));
                }
            }
            #endregion

            sqlCmd.Clear();
            #region 撈出NL Code為空或單位無法轉換的資料
            sqlCmd.Append(string.Format(
                @"Declare @styleukey bigint,
		@sizecode varchar(8),
		@article varchar(8),
		@category varchar(1),
		@vncontractid varchar(15)
set @styleukey = {0}
set @sizecode = '{1}'
set @article = '{2}'
set @category = '{3}'
set @vncontractid = '{4}';

with tmpMarkerData
as (
select sm.MarkerName,sm.StyleUkey,sm.FabricPanelCode,sma.Article,sms.SizeCode,dbo.MarkerLengthToYDS(sm.MarkerLength) as markerYDS,
sm.Width,sms.Qty,sc.FabricCode,sfqt.QTFabricCode
from Style_MarkerList sm WITH (NOLOCK) 
inner join Style_MarkerList_SizeQty sms WITH (NOLOCK) on sm.Ukey = sms.Style_MarkerListUkey and sms.SizeCode = @sizecode
inner join Style_ColorCombo sc WITH (NOLOCK) on sc.StyleUkey = sm.StyleUkey and sc.FabricPanelCode = sm.FabricPanelCode
left join Style_MarkerList_Article sma WITH (NOLOCK) on sm.Ukey = sma.Style_MarkerListUkey 
left join Style_FabricCode_QT sfqt WITH (NOLOCK)  on sm.FabricPanelCode = sfqt.FabricPanelCode and sm.StyleUkey = sfqt.StyleUkey
where sm.MixedSizeMarker = 1 and sm.StyleUkey = @styleukey and (sma.Article is null or sma.Article = @article)
and sc.Article = @article
),
tmpFabricCode
as (
select t.markerYDS,t.Width,t.Qty, IIF(t.QTFabricCode is null, sb.SCIRefno, sb1.SCIRefno) as SCIRefNo,
IIF(t.QTFabricCode is null, sb.SuppIDBulk, sb1.SuppIDBulk) as SuppIDBulk
from tmpMarkerData t
left join Style_BOF sb WITH (NOLOCK) on sb.StyleUkey = t.StyleUkey and sb.FabricCode = t.FabricCode
left join Style_BOF sb1 WITH (NOLOCK) on sb1.StyleUkey = t.StyleUkey and sb1.FabricCode = t.QTFabricCode
),
tmpBOFRateData
as (
select 'YDS' as UsageUnit,f.SCIRefno,f.Refno,f.BrandID,f.NLCode,f.CustomsUnit,f.Type,
isnull((select Rate from Unit_Rate WITH (NOLOCK) where UnitFrom = 'YDS' and UnitTo = f.CustomsUnit),'') as UnitRate,
isnull((select Rate from Unit_Rate WITH (NOLOCK) where UnitFrom = 'YDS' and UnitTo = 'M'),'') as M2UnitRate
from tmpFabricCode t
inner join Fabric f WITH (NOLOCK) on f.SCIRefno = t.SCIRefno
where (t.SuppIDBulk <> 'FTY' or t.SuppIDBulk <> 'FTY-C')
and f.NoDeclare = 0),
tmpBOA
as (
select sb.StyleUkey,sb.Ukey,sb.Refno,sb.SCIRefno,sb.SuppIDBulk,sb.SizeItem,sb.PatternPanel,sb.BomTypeArticle,sb.BomTypeColor,sb.ConsPC,
sc.ColorID,f.UsageUnit,f.HSCode,f.NLCode,f.CustomsUnit,f.PcsWidth,f.PcsLength,f.PcsKg,f.BomTypeCalculate,f.Type,f.BrandID
from Style_BOA sb WITH (NOLOCK) 
left join Style_ColorCombo sc WITH (NOLOCK) on sc.StyleUkey = sb.StyleUkey and sc.FabricPanelCode = sb.PatternPanel and sc.Article = @article
left join Fabric f WITH (NOLOCK) on sb.SCIRefno = f.SCIRefno
where sb.StyleUkey = @styleukey
and sb.IsCustCD <> 2
and (sb.SuppIDBulk <> 'FTY' and sb.SuppIDBulk <> 'FTY-C')
),
tmpBOAPrepareData
as (
select t.UsageUnit,t.SCIRefno,t.Refno,t.BrandID,t.NLCode,t.CustomsUnit,t.Type,
isnull((select Rate from Unit_Rate WITH (NOLOCK) where UnitFrom = t.UsageUnit and UnitTo = t.CustomsUnit),'') as UnitRate,
isnull((select Rate from Unit_Rate WITH (NOLOCK) where UnitFrom = t.UsageUnit and UnitTo = 'M'),'') as M2UnitRate
from tmpBOA t
where (t.BomTypeArticle = 0 and t.BomTypeColor = 0) or ((t.BomTypeArticle = 1 or t.BomTypeColor = 1) and t.ColorID is not null)
),
tmpLocalPO
as (
select ld.Refno,ld.Qty,ld.UnitId,li.MeterToCone,li.NLCode,li.HSCode,li.CustomsUnit,li.PcsWidth,li.PcsLength,li.PcsKg,o.Qty as OrderQty
,isnull(vd.Waste,0) as Waste
from LocalPO_Detail ld WITH (NOLOCK) 
left join LocalItem li WITH (NOLOCK) on li.RefNo = ld.Refno
left join Orders o WITH (NOLOCK) on ld.OrderId = o.ID
left join View_VNNLCodeWaste vd WITH (NOLOCK) on vd.NLCode = li.NLCode
where ld.OrderId = (select TOP 1 ID from Orders WITH (NOLOCK) where StyleUkey = @styleukey and Category = @category order by BuyerDelivery,ID)
and li.NoDeclare = 0
),
tmpConeToM
as (
select Refno,IIF(UnitId = 'CONE',Qty*MeterToCone,Qty) as Qty,OrderQty, IIF(UnitId = 'CONE','M',UnitId) as UnitId,Waste,
NLCode,HSCode,CustomsUnit,PcsWidth,PcsLength,PcsKg
from tmpLocalPO),
tmpPrepareRate
as (
select UnitId as UsageUnit,Refno as SCIRefno,Refno,'' as BrandID,NLCode,CustomsUnit,'' as Type,
isnull((select Rate from Unit_Rate WITH (NOLOCK) where UnitFrom = UnitId and UnitTo = CustomsUnit),'') as UnitRate,
isnull((select Rate from Unit_Rate WITH (NOLOCK) where UnitFrom = UnitId and UnitTo = 'M'),'') as M2UnitRate
from tmpConeToM),
tmpfinal
as (
select * from tmpBOFRateData
union
select * from tmpBOAPrepareData
union
select * from tmpPrepareRate),
tmpEmptyNLCode
as (
select '1' as DataType,* from tmpfinal where NLCode = ''
),
tmpUnitNotFound
as (
select distinct '2' as DataType,UsageUnit,SCIRefno,RefNo,'' as BrandID, '' as NLCode,'' as CustomsUnit,IIF(Type = 'F',Type,'') as Type,
UnitRate,M2UnitRate
from tmpfinal 
where UsageUnit <> CustomsUnit 
AND NOT (UsageUnit = 'PCS' and (CustomsUnit = 'KGS' OR CustomsUnit = 'M2' OR CustomsUnit = 'M'))
)
select * from tmpEmptyNLCode
union all
select * from tmpUnitNotFound
order by DataType,SCIRefno,UsageUnit",
                MyUtility.Convert.GetString(this.CurrentMaintain["StyleUKey"]),
                MyUtility.Convert.GetString(this.CurrentMaintain["SizeCode"]),
                colorway[0],
                MyUtility.Convert.GetString(this.CurrentMaintain["Category"]),
                MyUtility.Convert.GetString(this.CurrentMaintain["VNContractID"])));
            #endregion
            result = DBProxy.Current.Select(null, sqlCmd.ToString(), out invalidData);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Query invalid data fail!!\r\n" + result.ToString());
                return;
            }
            #region 組出NL Code為空或單位無法轉換的訊息
            foreach (DataRow dr in invalidData.Rows)
            {
                if (MyUtility.Convert.GetString(dr["DataType"]) == "1")
                {
                    emptyNLCode.Append(string.Format("RefNo: {0}, Brand: {1}\r\n", MyUtility.Convert.GetString(dr["RefNo"]), MyUtility.Convert.GetString(dr["BrandID"])));
                }
                else
                {
                    if ((MyUtility.Convert.GetString(dr["Type"]) == "F" && MyUtility.Convert.GetString(dr["CustomsUnit"]).ToUpper() == "M2" && MyUtility.Check.Empty(dr["M2UnitRate"])) ||
                        (MyUtility.Convert.GetString(dr["Type"]) == "F" && MyUtility.Convert.GetString(dr["CustomsUnit"]).ToUpper() != "M2" && MyUtility.Check.Empty(dr["UnitRate"])) ||
                        (MyUtility.Convert.GetString(dr["Type"]) != "F" && MyUtility.Convert.GetString(dr["CustomsUnit"]).ToUpper() == "M2" && MyUtility.Check.Empty(dr["M2UnitRate"])) ||
                        (MyUtility.Convert.GetString(dr["Type"]) != "F" && MyUtility.Convert.GetString(dr["CustomsUnit"]).ToUpper() == "M" && MyUtility.Check.Empty(dr["UnitRate"])) ||
                        (MyUtility.Convert.GetString(dr["Type"]) != "F" && MyUtility.Convert.GetString(dr["CustomsUnit"]).ToUpper() != "M2" && MyUtility.Convert.GetString(dr["CustomsUnit"]).ToUpper() != "M" && MyUtility.Check.Empty(dr["UnitRate"])))
                    {
                        wrongUnit.Append(string.Format("Customs Code:{0}  RefNo:{1}   Unit:{2} transfer to Unit:{3}\r\n", MyUtility.Convert.GetString(dr["NLCode"]), MyUtility.Convert.GetString(dr["RefNo"]), MyUtility.Convert.GetString(dr["UsageUnit"]), MyUtility.Convert.GetString(dr["CustomsUnit"])));
                    }
                }
            }
            #endregion

            #region 刪除VNConsumption_Detail與VNConsumption_Detail_Detail資料
            foreach (DataRow dr in this.DetailDatas)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    DataRow[] queryData = queryDetail2Data.Select(string.Format("NLCode = '{0}' and SCIRefNo = '{1}'", MyUtility.Convert.GetString(dr["NLCode"]), MyUtility.Convert.GetString(dr["SCIRefNo"])));
                    if (queryData.Length <= 0)
                    {
                        dr.Delete();
                    }
                }
            }

            #endregion

            #region 塞資料進VNConsumption_Detail_Detail
            foreach (DataRow dr in queryDetail2Data.Rows)
            {
                string strWhere = string.Format(
                    "SCIRefNo = '{0}' and Refno = '{1}' and NLCode = '{2}'",
                    MyUtility.Convert.GetString(dr["SCIRefNo"]),
                    MyUtility.Convert.GetString(dr["Refno"]),
                    MyUtility.Convert.GetString(dr["NLCode"]));
                DataRow[] queryData = ((DataTable)this.detailgridbs.DataSource).Select(strWhere);
                if (queryData.Length <= 0)
                {
                    DataRow newRow = ((DataTable)this.detailgridbs.DataSource).NewRow();
                    newRow["NLCode"] = dr["NLCode"];
                    newRow["SCIRefno"] = dr["SCIRefno"];
                    newRow["RefNo"] = dr["RefNo"];
                    newRow["LocalItem"] = dr["LocalItem"];
                    newRow["FabricBrandID"] = dr["BrandID"];
                    newRow["FabricType"] = dr["FabricType"];
                    newRow["UsageUnit"] = dr["UsageUnit"];
                    newRow["SystemQty"] = dr["Qty"];
                    newRow["UsageQty"] = dr["UsageQty"];
                    newRow["Qty"] = dr["Qty"];
                    newRow["StockQty"] = dr["StockQty"];
                    newRow["StockUnit"] = dr["StockUnit"];
                    newRow["HSCode"] = dr["HSCode"];
                    newRow["UnitID"] = dr["CustomsUnit"];
                    newRow["Waste"] = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup(string.Format(
                        @"select [dbo].[getWaste]('{0}','{1}','{2}','{3}','{4}')",
                        MyUtility.Convert.GetString(this.CurrentMaintain["StyleID"]),
                        MyUtility.Convert.GetString(this.CurrentMaintain["BrandID"]),
                        MyUtility.Convert.GetString(this.CurrentMaintain["SeasonID"]),
                        MyUtility.Convert.GetString(this.CurrentMaintain["VNContractID"]),
                        MyUtility.Convert.GetString(dr["NLCode"]))));
                    newRow["UserCreate"] = 0;
                    ((DataTable)this.detailgridbs.DataSource).Rows.Add(newRow);
                }
                else
                {
                    queryData[0]["SystemQty"] = dr["Qty"];
                    queryData[0]["Qty"] = dr["Qty"];
                    queryData[0]["StockQty"] = dr["StockQty"];
                    queryData[0]["UserCreate"] = 0;
                }
            }

            #endregion

            #region 組要顯示的訊息
            if (!MyUtility.Check.Empty(emptyNLCode.ToString()))
            {
                allMessage.Append(string.Format("Below data is no Customs Code in B40, B41:\r\n{0}\r\n", emptyNLCode.ToString()));
            }

            if (!MyUtility.Check.Empty(wrongUnit.ToString()))
            {
                allMessage.Append(string.Format("Below data is no transfer formula. Please contact with Taipei MIS.\r\n{0}\r\n", wrongUnit.ToString()));
            }

            if (!MyUtility.Check.Empty(needItem.ToString()))
            {
                allMessage.Append(string.Format("Below data is lacking item. Please pay attention!!\r\n{0}\r\n", needItem.ToString()));
            }
            #endregion

            if (MyUtility.Check.Empty(allMessage.ToString()))
            {
                MyUtility.Msg.InfoBox("Calculate complete!!");
            }
            else
            {
                MyUtility.Msg.WarningBox(allMessage.ToString());
            }
        }

        /// <inheritdoc/>
        protected override void ClickConfirm()
        {
            base.ClickConfirm();

            // 檢查表身Grid資料是否有缺一必輸的NLCode資料，若有，就出訊息告知使用者
            string sqlCmd = string.Format(
                @"
select NLCode from VNContract_Detail WITH (NOLOCK) where ID = '{0}' and NecessaryItem = 1
except
select NLCode from VNConsumption_Detail WITH (NOLOCK) where ID = '{1}'",
                MyUtility.Convert.GetString(this.CurrentMaintain["VNContractID"]),
                MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));

            DataTable lackData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out lackData);
            if (lackData.Rows.Count > 0)
            {
                StringBuilder lackNLCode = new StringBuilder();
                foreach (DataRow dr in lackData.Rows)
                {
                    lackNLCode.Append(MyUtility.Convert.GetString(dr["NLCode"]) + ",");
                }

                MyUtility.Msg.WarningBox(string.Format("Lacking regular Customs Code: {0}. Please double check.", lackNLCode.ToString(0, lackNLCode.ToString().Length - 1)));
            }

            string updateCmds = string.Format(
                "update VNConsumption set EditDate = GETDATE(), EditName = '{0}', Status = 'Confirmed' where ID = '{1}'",
                Env.User.UserID,
                MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));

            result = DBProxy.Current.Execute(null, updateCmds);
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
                "update VNConsumption set EditDate = GETDATE(), EditName = '{0}', Status = 'New' where ID = '{1}'",
                Env.User.UserID,
                MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));

            DualResult result = DBProxy.Current.Execute(null, updateCmds);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Unconfirm fail!!\r\n" + result.ToString());
                return;
            }
        }
    }
}
