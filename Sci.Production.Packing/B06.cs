using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;
using System.Data.SqlClient;
using Sci.Win.Tools;
using System.Linq;

namespace Sci.Production.Packing
{
    public partial class B06 : Sci.Win.Tems.Input6
    {
        private string oldBrand = string.Empty;

        public B06(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();

            string cmd = $@"
SELECT [Text]=Name,[Value]=ID

FROM DropDownList
WHERE Type ='PMS_ShipMarkCategory' 
";
            DataTable categoryData;
            DualResult result = DBProxy.Current.Select(null, cmd, out categoryData);
            if (!result)
            {
                this.ShowErr(result);
            }

            this.comboCategory.DataSource = new BindingSource(categoryData, null);
            this.comboCategory.ValueMember = "Value";
            this.comboCategory.DisplayMember = "Text";
        }

        /// <inheritdoc/>
        protected override void EnsureToolbarExt()
        {
            base.EnsureToolbarExt();
            if (!this.EditMode && this.CurrentMaintain != null && this.tabs.SelectedIndex == 1)
            {
                bool junk = MyUtility.Convert.GetBool(this.CurrentMaintain["Junk"]);
                this.toolbar.cmdJunk.Enabled = !junk && this.Perm.Junk;
                this.toolbar.cmdUnJunk.Enabled = junk && this.Perm.Junk;
            }
            else
            {
                this.toolbar.cmdJunk.Enabled = false;
                this.toolbar.cmdUnJunk.Enabled = false;
            }
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterUkey = (e.Master == null) ? string.Empty : e.Master["Ukey"].ToString();
            this.DetailSelectCommand = $@"
SELECT b.Seq ,b.ShippingMarkCombinationUkey ,b.ShippingMarkTypeUkey ,[ShippingMarkTypeID]=c.ID ,c.IsSSCC
FROM ShippingMarkCombination a
INNER JOIN ShippingMarkCombination_Detail b ON a.Ukey = b.ShippingMarkCombinationUkey
INNER JOIN ShippingMarkType c ON b.ShippingMarkTypeUkey = c.Ukey
WHERE b.ShippingMarkCombinationUkey = '{masterUkey}'
ORDER BY b.Seq
";
            return base.OnDetailSelectCommandPrepare(e);
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();

            this.chkIsMixPack.ReadOnly = true;
            this.txtbrand.ReadOnly = true;
            this.comboCategory.ReadOnly = true;
            this.txtID.ReadOnly = true;

            if (this.EditMode)
            {
                this.checkIsDefault.ReadOnly = false;
            }
            else
            {
                this.checkIsDefault.ReadOnly = true;
            }
        }

        /// <inheritdoc/>
        protected override bool ClickEditBefore()
        {
            if (MyUtility.Convert.GetBool(this.CurrentMaintain["Junk"]))
            {
                MyUtility.Msg.WarningBox("This record is < Junked >, can't be modified!");
                return false;
            }

            return base.ClickEditBefore();
        }

        /// <inheritdoc/>
        protected override bool OnGridSetup()
        {
            DataGridViewGeneratorTextColumnSettings shippingMarkTypeID = new DataGridViewGeneratorTextColumnSettings();

            shippingMarkTypeID.CellValidating += (s, e) =>
            {
                DataRow currentRow = this.detailgrid.GetDataRow(e.RowIndex);
                if (this.EditMode)
                {
                    if (MyUtility.Check.Empty(e.FormattedValue))
                    {
                        currentRow["ShippingMarkTypeID"] = e.FormattedValue;
                        currentRow["IsSSCC"] = false;
                        return;
                    }

                    string typeID = e.FormattedValue.ToString();

                    // 判斷 ShippingMarkType 基本檔中表頭的品牌 + Category是否存在這個 Shipping Mark 種類 （排除 Junk）
                    string cmd = $@"SELECT * FROM ShippingMarkType WITH(NOLOCK) WHERE BrandID = '{this.CurrentMaintain["BrandID"]}' AND Category = '{this.CurrentMaintain["Category"]}' AND ID=@ID AND Junk = 0";

                    List<SqlParameter> paras = new List<SqlParameter>();
                    paras.Add(new SqlParameter("@ID", typeID));

                    DataTable dt;
                    DualResult r = DBProxy.Current.Select(null, cmd, paras, out dt);
                    bool exists = MyUtility.Check.Seek(cmd, paras);

                    if (!r)
                    {
                        this.ShowErr(r);
                        e.FormattedValue = string.Empty;
                        currentRow.EndEdit();
                        return;
                    }

                    if (dt.Rows.Count == 0)
                    {
                        currentRow.EndEdit();
                        e.Cancel = true;
                        currentRow["IsSSCC"] = false;
                        MyUtility.Msg.WarningBox("Data not found !!");
                        return;
                    }
                    else
                    {
                        bool isSSCC = MyUtility.Convert.GetBool(dt.Rows[0]["IsSSCC"]);
                        currentRow["ShippingMarkTypeUkey"] = MyUtility.Convert.GetInt(dt.Rows[0]["Ukey"]);
                        currentRow["ShippingMarkTypeID"] = typeID;
                        currentRow["IsSSCC"] = isSSCC;

                        currentRow.EndEdit();
                    }
                }
            };

            shippingMarkTypeID.EditingMouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    if (MyUtility.Check.Empty(this.CurrentMaintain["BrandID"]))
                    {
                        MyUtility.Msg.InfoBox("Please set Brand first.");
                        return;
                    }

                    DataRow currentRow = this.detailgrid.GetDataRow(e.RowIndex);
                    string cmd = $@"SELECT [Shipping Mark Type ID]=ID,[IsSSCC] = IIF(IsSSCC=1,'Y',''),Ukey FROM ShippingMarkType WITH(NOLOCK) WHERE BrandID = '{this.CurrentMaintain["BrandID"]}' AND Category = '{this.CurrentMaintain["Category"]}' AND Junk = 0";
                    DataTable dt;
                    DBProxy.Current.Select(null, cmd, out dt);
                    SelectItem item = new SelectItem(dt, "Shipping Mark Type ID,IsSSCC", "10,10", currentRow["ShippingMarkTypeUkey"].ToString());
                    DialogResult returnResult = item.ShowDialog();
                    if (returnResult == DialogResult.Cancel)
                    {
                        return;
                    }

                    IList<DataRow> selectedData = item.GetSelecteds();
                    currentRow["ShippingMarkTypeID"] = item.GetSelectedString();
                    currentRow["IsSSCC"] = selectedData[0]["IsSSCC"].ToString() == "Y" ? true : false;
                    currentRow["ShippingMarkTypeUkey"] = MyUtility.Convert.GetInt(selectedData[0]["Ukey"]);

                    currentRow.EndEdit();
                }
            };

            this.Helper.Controls.Grid.Generator(this.detailgrid)
            .Numeric("Seq", header: "Seq", width: Widths.AnsiChars(4), decimal_places: 0, iseditingreadonly: false)
            .Text("ShippingMarkTypeID", header: "Sticker Type", width: Widths.AnsiChars(15), iseditingreadonly: false, settings: shippingMarkTypeID)
            .CheckBox("IsSSCC", header: "Is SSCC", width: Widths.AnsiChars(3), iseditable: false, trueValue: 1, falseValue: 0)
            ;
            return base.OnGridSetup();
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            this.CurrentMaintain["Category"] = "PIC";
            this.CurrentMaintain["IsMixPack"] = false;
            this.CurrentMaintain["IsDefault"] = false;

            this.chkIsMixPack.ReadOnly = false;
            this.txtbrand.ReadOnly = false;
            this.comboCategory.ReadOnly = false;
            this.txtID.ReadOnly = false;

            base.ClickNewAfter();
        }

        /// <inheritdoc/>
        protected override void OnDetailGridAppendClick()
        {
            base.OnDetailGridAppendClick();

            // DataRow newrow = this.detailgrid.GetDataRow(this.detailgrid.GetSelectedRowIndex());
            // newrow["Seq"] = 1;
        }

        private void Txtbrand_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string newBrand = this.txtbrand.Text;
            if (this.oldBrand != newBrand)
            {
                this.oldBrand = this.txtbrand.Text;

                // this.DetailDatas.Clear();

                // 刪除表身重新匯入
                foreach (DataRow del in this.DetailDatas)
                {
                    del.Delete();
                }
            }
        }

        /// <inheritdoc/>
        protected override void ClickJunk()
        {
            base.ClickJunk();

            if (MyUtility.Convert.GetBool(this.CurrentMaintain["IsDefault"]))
            {
                MyUtility.Msg.WarningBox("Default Shipping Mark Combination cannot be junk.");
                return;
            }

            string sqlcmd = $@"update ShippingMarkCombination set junk = 1,EditDate= GETDATE(), EditName='{Sci.Env.User.UserID}' where Ukey = {this.CurrentMaintain["Ukey"]}";
            DualResult result = DBProxy.Current.Execute(null, sqlcmd);
            if (!result)
            {
                this.ShowErr(result);
            }
        }

        /// <inheritdoc/>
        protected override void ClickUnJunk()
        {
            base.ClickUnJunk();
            string sqlcmd = $@"update ShippingMarkCombination set junk = 0,EditDate= GETDATE(), EditName='{Sci.Env.User.UserID}' where Ukey = {this.CurrentMaintain["Ukey"]}";
            DualResult result = DBProxy.Current.Execute(null, sqlcmd);
            if (!result)
            {
                this.ShowErr(result);
            }
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            // 1.ShippingMarkCombination & Brand 不可為空
            if (MyUtility.Check.Empty(this.CurrentMaintain["ID"]) || MyUtility.Check.Empty(this.CurrentMaintain["BrandID"]))
            {
                MyUtility.Msg.WarningBox("Shipping Mark Combination and Brand cannot be empty.");
                return false;
            }

            // 2.同一個品牌中ShippingMarkCombination 不可重複
            string cmd = $@"SELECT * FROM ShippingMarkCombination WHERE BrandID='{this.CurrentMaintain["BrandID"]}' AND ID ='{this.CurrentMaintain["ID"]}' AND Ukey <> {this.CurrentMaintain["UKey"]}";
            bool duplicate = MyUtility.Check.Seek(cmd);
            if (duplicate)
            {
                MyUtility.Msg.WarningBox($"Brand {this.CurrentMaintain["BrandID"]} Shiping Mark Combination {this.CurrentMaintain["ID"]} already exists.");
                return false;
            }

            // 3.表身的 Seq 不可重複
            // 先排除被刪掉的Row
            if (this.DetailDatas.Count > 0)
            {
                DataTable detail = this.DetailDatas.CopyToDataTable();

                if (detail.AsEnumerable().Select(o => MyUtility.Convert.GetInt(o["Seq"])).Distinct().Count() != detail.AsEnumerable().Select(o => MyUtility.Convert.GetInt(o["Seq"])).Count())
                {
                    MyUtility.Msg.WarningBox("Seq cannot be duplicated.");
                    return false;
                }

                // 4.表身的 Shipping Mark Type 不可重複
                if (detail.AsEnumerable().Select(o => o["ShippingMarkTypeID"].ToString()).Distinct().Count() != detail.AsEnumerable().Select(o => o["ShippingMarkTypeID"].ToString()).Count())
                {
                    MyUtility.Msg.WarningBox("Shipping Mark Type cannot be duplicated.");
                    return false;
                }

                // 表身ShippingMarkType不可為空
                if (detail.AsEnumerable().Where(o => MyUtility.Check.Empty(o["ShippingMarkTypeUkey"])).Any())
                {
                    MyUtility.Msg.WarningBox("Shipping Mark Type cannot be empty.");
                    return false;
                }
            }

            // 若Category = PIC，同品牌 + IsMixPack 只允許出現 1 個 Default
            cmd = $@"SELECT * FROM ShippingMarkCombination 
WHERE BrandID='{this.CurrentMaintain["BrandID"]}' 
AND IsMixPack='{(MyUtility.Convert.GetBool(this.CurrentMaintain["IsMixPack"]) ? "1" : "0")}' 
AND IsDefault=1 
AND Ukey <> {this.CurrentMaintain["UKey"]}
";
            DataTable dt;
            DualResult r = DBProxy.Current.Select(null, cmd, out dt);

            if (!r)
            {
                this.ShowErr(r);
                return false;
            }

            if (this.CurrentMaintain["Category"].ToString() == "PIC")
            {
                if (MyUtility.Convert.GetBool(this.CurrentMaintain["IsDefault"]))
                {
                    // 若有已勾選資料，詢問是否覆蓋
                    if (dt.Rows.Count > 0)
                    {
                        string otherBrand = dt.Rows[0]["BrandID"].ToString();
                        string otherIDd = dt.Rows[0]["ID"].ToString();

                        DialogResult diaR = MyUtility.Msg.QuestionBox($@"Brand {otherBrand} IsMixPack (True/False) Shipping Mark Combination {otherIDd}, already ticked ‘Is Default’, system will auto untick ‘Is Default’ for Shipping Mark Combination {this.CurrentMaintain["ID"]}.
Please check to continue process.");

                        if (diaR == DialogResult.Yes)
                        {
                            cmd = $@"UPDATE ShippingMarkCombination SET IsDefault = 0 WHERE BrandID='{otherBrand}' AND ID = '{otherIDd}'AND IsDefault = 1 ";
                            r = DBProxy.Current.Execute(null, cmd);
                            if (!r)
                            {
                                this.ShowErr(r);
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    // 若該品牌沒有 IsDefault=1的資料，則將該筆資料設定為 IsDefault
                    // 同一品牌中，『混尺碼 / 單尺碼』各至少要有一組貼標 Shipping Mark Combination 為預設
                    if (dt.Rows.Count == 0)
                    {
                        MyUtility.Msg.InfoBox($"Brand {this.CurrentMaintain["BrandID"]} IsMixPack (True/False) not yet set default, system will auto tick 'Is Default' for this Shipping Mark Combination.");
                        this.CurrentMaintain["IsDefault"] = true;
                    }
                }
            }

            return true;
        }

        /// <inheritdoc/>
        protected override void ClickSaveAfter()
        {
            base.ClickSaveAfter();
            this.Reload();
        }

        public void Reload()
        {
            if (this.CurrentDataRow != null)
            {
                string idIndex = string.Empty;
                if (!MyUtility.Check.Empty(this.CurrentMaintain))
                {
                    if (!MyUtility.Check.Empty(this.CurrentMaintain["Ukey"]))
                    {
                        idIndex = MyUtility.Convert.GetString(this.CurrentMaintain["Ukey"]);
                    }
                }

                this.ReloadDatas();
                this.RenewData();
                if (!MyUtility.Check.Empty(idIndex))
                {
                    this.gridbs.Position = this.gridbs.Find("Ukey", idIndex);
                }
            }
        }
    }
}
