using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;
using System.Data.SqlClient;
using Sci.Win.Tools;
using System.Linq;
using System.Collections;
using System;
using Sci.Production.PublicPrg;
using static Sci.Production.PublicPrg.Prgs;

namespace Sci.Production.Packing
{
    /// <inheritdoc/>
    public partial class B03 : Sci.Win.Tems.Input6
    {
        private Hashtable ht = new Hashtable();
        private DataTable sizes;
        private DataTable sizesAll;
        private string oldStickerComb = string.Empty;
        private string oldBrandID = string.Empty;
        private List<ShippingMarkPicture_Detail> OriDetailDatas;
        private DataRow OriCurrentMaintain;

        /// <inheritdoc/>
        public B03(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();

            string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @".\Resources\");
            if (this.ht.Count == 0)
            {
                this.ht.Add("Picture1", path + "CTN.jpg");
                this.pictureBox1.ImageLocation = this.ht["Picture1"].ToString();
            }

            #region ComboBox
            DualResult result;
            string cmd = $@"
--SELECT [StickerSizeID]='' ,[SIze]='' 
--UNION
SELECT [StickerSizeID]=ID, SIze 
FROM StickerSize WITH (NOLOCK) 
where junk <> 1";
            result = DBProxy.Current.Select(null, cmd, out this.sizes);
            if (!result)
            {
                this.ShowErr(result);
            }

            cmd = $@"
SELECT [StickerSizeID]='' ,[SIze]='' 
UNION
SELECT [StickerSizeID]=ID, SIze 
FROM StickerSize WITH (NOLOCK) 
";
            result = DBProxy.Current.Select(null, cmd, out this.sizesAll);
            if (!result)
            {
                this.ShowErr(result);
            }

            // comboCategory
            cmd = $@"
SELECT [Text]=Name,[Value]=ID

FROM DropDownList
WHERE Type ='PMS_ShipMarkCategory' 
";
            DataTable categoryData;
            result = DBProxy.Current.Select(null, cmd, out categoryData);
            if (!result)
            {
                this.ShowErr(result);
            }

            this.comboCategory.DataSource = new BindingSource(categoryData, null);
            this.comboCategory.ValueMember = "Value";
            this.comboCategory.DisplayMember = "Text";
            #endregion
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.btnReloadSticker.Enabled = this.EditMode && !this.IsDetailInserting;
            this.txtStickerComb.Text = MyUtility.GetValue.Lookup($@"
SELECT ID
FROM ShippingMarkCombination WITH(NOLOCK)
WHERE Ukey = '{this.CurrentMaintain["ShippingMarkCombinationUkey"]}'
");

            this.oldStickerComb = this.txtStickerComb.Text;
            this.oldBrandID = MyUtility.Convert.GetString(this.CurrentMaintain["BrandID"]);

            this.chkIsMixPack.Checked = MyUtility.Convert.GetBool(MyUtility.GetValue.Lookup($@"
SELECT IsMixPack
FROM ShippingMarkCombination WITH(NOLOCK)
WHERE Ukey = '{this.CurrentMaintain["ShippingMarkCombinationUkey"]}'

"));

            this.disCtnHeight.Value = MyUtility.Convert.GetDecimal(this.CurrentMaintain["CTNHeight"]);

            if (MyUtility.Convert.GetString(this.CurrentMaintain["Category"]) == "HTML")
            {
                this.chkIsMixPack.Enabled = false;
            }
            else
            {
                this.chkIsMixPack.Enabled = true;
            }

            if (this.IsDetailInserting)
            {
                this.comboCategory.ReadOnly = false;
                this.txtbrand1.ReadOnly = false;
                this.txtStickerComb.ReadOnly = false;
                this.txtCTNRefno.ReadOnly = false;
            }
            else
            {
                this.comboCategory.ReadOnly = true;
                this.txtbrand1.ReadOnly = true;
                this.txtStickerComb.ReadOnly = true;
                this.txtCTNRefno.ReadOnly = true;
            }

            if (!MyUtility.Check.Empty(this.CurrentMaintain))
            {
                this.ChangeVisible(MyUtility.Convert.GetString(this.CurrentMaintain["Category"]));
            }
            else
            {
                this.ChangeVisible("PIC");
            }

            this.OriDetailDatas = new List<ShippingMarkPicture_Detail>();
            foreach (DataRow item in this.DetailDatas)
            {
                ShippingMarkPicture_Detail s = new ShippingMarkPicture_Detail()
                {
                    ShippingMarkPictureUkey = MyUtility.Convert.GetInt(item["ShippingMarkPictureUkey"]),
                    ShippingMarkTypeUkey = MyUtility.Convert.GetInt(item["ShippingMarkTypeUkey"]),
                    Seq = MyUtility.Convert.GetInt(item["Seq"]),
                    IsSSCC = MyUtility.Convert.GetBool(item["IsSSCC"]),
                    Side = MyUtility.Convert.GetString(item["Side"]),
                    FromRight = MyUtility.Convert.GetInt(item["FromRight"]),
                    FromBottom = MyUtility.Convert.GetInt(item["FromBottom"]),
                    StickerSizeID = MyUtility.Convert.GetInt(item["StickerSizeID"]),
                    Is2Side = MyUtility.Convert.GetBool(item["Is2Side"]),
                    IsHorizontal = MyUtility.Convert.GetBool(item["IsHorizontal"]),
                    IsOverCtnHt = MyUtility.Convert.GetBool(item["IsOverCtnHt"]),
                    NotAutomate = MyUtility.Convert.GetBool(item["NotAutomate"]),
                };
                this.OriDetailDatas.Add(s);
            }

            this.OriCurrentMaintain = this.CurrentMaintain;
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterUkey = (e.Master == null) ? string.Empty : e.Master["Ukey"].ToString();
            this.DetailSelectCommand = $@"
SELECT [ShippingMarkTypeID]=c.ID 
        , b.*
FROM ShippingMarkPicture a
INNER JOIN ShippingMarkPicture_Detail b ON a.Ukey = b.ShippingMarkPictureUkey
INNER JOIN ShippingMarkType c ON b.ShippingMarkTypeUkey = c.Ukey
WHERE a.Ukey = '{masterUkey}'
ORDER BY b.Seq
";
            return base.OnDetailSelectCommandPrepare(e);
        }

        /// <inheritdoc/>
        protected override bool OnGridSetup()
        {
            DataGridViewGeneratorComboBoxColumnSettings sideComboCell = new DataGridViewGeneratorComboBoxColumnSettings();
            DataGridViewGeneratorComboBoxColumnSettings stickerSizeCell = new DataGridViewGeneratorComboBoxColumnSettings();
            DataGridViewGeneratorNumericColumnSettings fromBottomCell = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorCheckBoxColumnSettings isHorizontalCell = new DataGridViewGeneratorCheckBoxColumnSettings();

            // sideComboCell
            Dictionary<string, string> side = new Dictionary<string, string>();
            side.Add("A", "A");
            side.Add("B", "B");
            side.Add("C", "C");
            side.Add("D", "D");
            sideComboCell.DataSource = new BindingSource(side, null);
            sideComboCell.ValueMember = "Key";
            sideComboCell.DisplayMember = "Value";

            // sideComboCell
            Dictionary<long, string> size = new Dictionary<long, string>();

            // 顯示不分有無Junk
            foreach (DataRow dr in this.sizesAll.Rows)
            {
                size.Add(MyUtility.Convert.GetLong(dr["StickerSizeID"]), MyUtility.Convert.GetString(dr["SIze"]));
            }

            stickerSizeCell.DataSource = new BindingSource(size, null);
            stickerSizeCell.ValueMember = "Key";
            stickerSizeCell.DisplayMember = "Value";

            fromBottomCell.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow currentRow = this.detailgrid.GetDataRow(e.RowIndex);
                    currentRow["FromBottom"] = e.FormattedValue;
                    this.CaculateCtnHeight(ref currentRow);

                    currentRow.EndEdit();
                }
            };

            isHorizontalCell.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow currentRow = this.detailgrid.GetDataRow(e.RowIndex);
                    currentRow["IsHorizontal"] = e.FormattedValue;
                    this.CaculateCtnHeight(ref currentRow);

                    currentRow.EndEdit();
                }
            };

            // 避免選了Junk
            stickerSizeCell.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow currentRow = this.detailgrid.GetDataRow(e.RowIndex);
                    int currentStickerSizeID = MyUtility.Convert.GetInt(e.FormattedValue);
                    DataRow dr;
                    string cmd = $@"
SELECT Size
FROM StickerSize WITH (NOLOCK) 
WHERE ID = '{currentStickerSizeID}'
AND Junk = 1
";
                    if (MyUtility.Check.Seek(cmd, out dr))
                    {
                        MyUtility.Msg.InfoBox($"Sticker Size {dr["Size"]} has junked !!");
                        currentRow["StickerSizeID"] = 0;
                        e.FormattedValue = 0;
                    }
                    else
                    {
                        currentRow["StickerSizeID"] = currentStickerSizeID;
                    }

                    this.CaculateCtnHeight(ref currentRow);

                    currentRow.EndEdit();
                }
            };

            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("ShippingMarkTypeID", header: "Mark Type", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Numeric("Seq", header: "Seq", width: Widths.AnsiChars(4), decimal_places: 0, iseditingreadonly: true)
                .CheckBox("IsSSCC", header: "SSCC", width: Widths.AnsiChars(3), iseditable: false, trueValue: 1, falseValue: 0)
                .ComboBox("Side", header: "Side", width: Widths.AnsiChars(10), settings: sideComboCell)
                .Numeric("FromRight", header: "From Right (mm)", width: Widths.AnsiChars(4), decimal_places: 0, iseditingreadonly: false)
                .Numeric("FromBottom", header: "From Bottom  (mm)", width: Widths.AnsiChars(4), decimal_places: 0, iseditingreadonly: false, settings: fromBottomCell)
                .ComboBox("StickerSizeID", header: "Mark Size", width: Widths.AnsiChars(20), settings: stickerSizeCell)
                .CheckBox("Is2Side", header: "Is 2 Side", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
                .CheckBox("IsHorizontal", header: "Is Horizontal", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0,settings: isHorizontalCell)
                .CheckBox("IsOverCtnHt", header: "Is Over Carton Height", width: Widths.AnsiChars(3), iseditable: false, trueValue: 1, falseValue: 0)
                .CheckBox("NotAutomate", header: "Not to Automate", width: Widths.AnsiChars(3), iseditable: false, trueValue: 1, falseValue: 0)
            ;
            return base.OnGridSetup();
        }

        /// <inheritdoc/>
        protected override bool ClickEditBefore()
        {
            if (MyUtility.Convert.GetBool(this.CurrentMaintain["Junk"]))
            {
                MyUtility.Msg.InfoBox("This record has junked, can't modify.");
                return false;
            }

            return base.ClickEditBefore();
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();

            this.CurrentMaintain["Category"] = "PIC";
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["BrandID"]))
            {
                MyUtility.Msg.WarningBox("Brand can not empty!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["CTNRefno"]))
            {
                MyUtility.Msg.WarningBox("CTNRefno can not empty!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["ShippingMarkCombinationUkey"]) || this.DetailDatas.Count() == 0 || this.DetailDatas.Where(o => MyUtility.Check.Empty(o["ShippingMarkTypeUkey"])).Any())
            {
                MyUtility.Msg.WarningBox("Sticker Conbiantion and Sticker Type cannot be empty.");
                return false;
            }

            string cmd = $@"
select 1
from ShippingMarkPicture WITH(NOLOCK)
where BrandID = '{this.CurrentMaintain["BrandID"]}'
AND Category='{this.CurrentMaintain["Category"]}'
AND ShippingMarkCombinationUkey='{this.CurrentMaintain["ShippingMarkCombinationUkey"]}'
AND CTNRefno='{this.CurrentMaintain["CTNRefno"]}'
AND Ukey <> {this.CurrentMaintain["Ukey"]}
";

            if (MyUtility.Convert.GetString(this.CurrentMaintain["Category"]) == "PIC")
            {
                if (MyUtility.Check.Seek(cmd))
                {
                    MyUtility.Msg.WarningBox($"Sticker : Brand {this.CurrentMaintain["BrandID"]}, Category {this.comboCategory.Text}, Combination {this.txtStickerComb.Text}, CTN Refon {this.CurrentMaintain["CTNRefno"]} already exists.");
                    return false;
                }
            }

            if (MyUtility.Convert.GetString(this.CurrentMaintain["Category"]) == "HTML")
            {
                if (MyUtility.Check.Seek(cmd))
                {
                    MyUtility.Msg.WarningBox($"Stamp : Brand {this.CurrentMaintain["BrandID"]}, Category {this.comboCategory.Text}, Combination {this.txtStickerComb.Text}, CTN Refon {this.CurrentMaintain["CTNRefno"]} already exists.");
                    return false;
                }
            }

            foreach (DataRow row in this.DetailDatas)
            {
                if (MyUtility.Check.Empty(row["StickerSizeID"]))
                {
                    MyUtility.Msg.WarningBox("Mark Size cannot be empty.");
                    return false;
                }

            }

            DataTable currentDt = (DataTable)this.detailgridbs.DataSource;

            foreach (DataRow current in currentDt.Rows)
            {
                bool isHorizontal = MyUtility.Convert.GetBool(current["IsHorizontal"]);
                string stickerSizeID = MyUtility.Convert.GetString(current["StickerSizeID"]);
                decimal ctnHeight = MyUtility.Convert.GetDecimal(this.disCtnHeight.Value);
                decimal fromBottom = MyUtility.Convert.GetDecimal(current["FromBottom"]);
                decimal width = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup($"SELECT Width FROM StickerSize WHERE ID = '{stickerSizeID}'"));
                decimal leghth = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup($"SELECT Length FROM StickerSize WHERE ID = '{stickerSizeID}'"));
                bool isOverCtnH = false;
                bool notAutomate = false;

                if (isHorizontal)
                {
                    isOverCtnH = (fromBottom + width) > ctnHeight;
                }
                else
                {
                    isOverCtnH = (fromBottom + leghth) > ctnHeight;
                }

                notAutomate = isOverCtnH;

                current["IsOverCtnHt"] = isOverCtnH;
                current["NotAutomate"] = notAutomate;
            }

            this.CurrentMaintain["CTNHeight"] = this.disCtnHeight.Value;

            bool isDetailChange = false;
            foreach (DataRow current in this.DetailDatas)
            {
                int shippingMarkPictureUkey = MyUtility.Convert.GetInt(current["ShippingMarkPictureUkey"]);
                int shippingMarkTypeUkey = MyUtility.Convert.GetInt(current["shippingMarkTypeUkey"]);
                var tmp = this.OriDetailDatas.Where(o => o.ShippingMarkPictureUkey == shippingMarkPictureUkey && o.ShippingMarkTypeUkey == shippingMarkTypeUkey);

                if (!tmp.Any())
                {
                    isDetailChange = true;
                    break;
                }

                DataRow ori = this.DetailDatas.FirstOrDefault();
                ShippingMarkPicture_Detail s = this.OriDetailDatas.FirstOrDefault();
                int oriShippingMarkTypeUkey = s.ShippingMarkTypeUkey;
                string oriSide = s.Side;
                int oriFromRight = s.FromRight;
                int oriFromBottom = s.FromBottom;
                int oriStickerSizeID = s.StickerSizeID;
                bool oriIs2Side = s.Is2Side;
                bool oriIsHorizontal = s.IsHorizontal;
                bool oriIsOverCtnHt = s.IsOverCtnHt;
                bool oriNotAutomate = s.NotAutomate;

                int newShippingMarkTypeUkey = MyUtility.Convert.GetInt(current["ShippingMarkTypeUkey"]);
                string newSide = MyUtility.Convert.GetString(current["Side"]);
                int newFromRight = MyUtility.Convert.GetInt(current["FromRight"]);
                int newFromBottom = MyUtility.Convert.GetInt(current["FromBottom"]);
                int newStickerSizeID = MyUtility.Convert.GetInt(current["StickerSizeID"]);
                bool newIs2Side = MyUtility.Convert.GetBool(current["Is2Side"]);
                bool newIsHorizontal = MyUtility.Convert.GetBool(current["IsHorizontal"]);
                bool newIsOverCtnHt = MyUtility.Convert.GetBool(current["IsOverCtnHt"]);
                bool newNotAutomate = MyUtility.Convert.GetBool(current["NotAutomate"]);

                if (oriShippingMarkTypeUkey != newShippingMarkTypeUkey ||
                    oriSide != newSide ||
                    oriFromRight != newFromRight ||
                    oriFromBottom != newFromBottom ||
                    oriStickerSizeID != newStickerSizeID ||
                    oriIs2Side != newIs2Side ||
                    oriIsHorizontal != newIsHorizontal ||
                    oriIsOverCtnHt != newIsOverCtnHt ||
                    oriNotAutomate != newNotAutomate)
                {
                    isDetailChange = true;
                }
            }

            foreach (ShippingMarkPicture_Detail ori in this.OriDetailDatas)
            {
                int shippingMarkPictureUkey = ori.ShippingMarkPictureUkey;
                int shippingMarkTypeUkey = ori.ShippingMarkTypeUkey;
                var tmp = this.DetailDatas.Where(o => MyUtility.Convert.GetInt(o["ShippingMarkPictureUkey"]) == shippingMarkPictureUkey && MyUtility.Convert.GetInt(o["shippingMarkTypeUkey"]) == shippingMarkTypeUkey);

                if (!tmp.Any())
                {
                    isDetailChange = true;
                    break;
                }
            }

            bool oriIsMix = MyUtility.Convert.GetBool(MyUtility.GetValue.Lookup($@"
SELECT IsMixPack
FROM ShippingMarkCombination WITH(NOLOCK)
WHERE Ukey = '{this.OriCurrentMaintain["ShippingMarkCombinationUkey"]}'

"));

            bool newIsMix = MyUtility.Convert.GetBool(MyUtility.GetValue.Lookup($@"
SELECT IsMixPack
FROM ShippingMarkCombination WITH(NOLOCK)
WHERE Ukey = '{this.CurrentMaintain["ShippingMarkCombinationUkey"]}'

"));

            bool isHeadChange = MyUtility.Convert.GetString(this.CurrentMaintain["BrandID"]) == MyUtility.Convert.GetString(this.OriCurrentMaintain["BrandID"]) &&
                MyUtility.Convert.GetString(this.CurrentMaintain["Category"]) == MyUtility.Convert.GetString(this.OriCurrentMaintain["Category"]) &&
                MyUtility.Convert.GetInt(this.CurrentMaintain["ShippingMarkCombinationUkey"]) == MyUtility.Convert.GetInt(this.OriCurrentMaintain["ShippingMarkCombinationUkey"]) &&
                MyUtility.Convert.GetString(this.CurrentMaintain["CTNRefno"]) == MyUtility.Convert.GetString(this.OriCurrentMaintain["CTNRefno"]) &&
                newIsMix == oriIsMix;

            if (isHeadChange && isDetailChange)
            {
                cmd = "select * from MailTo where ID='102' AND ToAddress != '' AND ToAddress IS NOT NULL";

                if (MyUtility.Check.Seek(cmd))
                {
                    Prgs.ShippingMarkPicture_RunningChange(this.CurrentMaintain, this.DetailDatas.ToList(), this.OriDetailDatas, MyUtility.Convert.GetString(this.CurrentMaintain["Category"]));
                }
            }

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override void EnsureToolbarExt()
        {
            base.EnsureToolbarExt();
            if (!this.EditMode && this.CurrentMaintain != null && this.tabs.SelectedIndex == 1)
            {
                bool junk = MyUtility.Convert.GetBool(this.CurrentMaintain["junk"]);
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
        protected override void ClickJunk()
        {
            base.ClickJunk();
            string sqlcmd = $@"update ShippingMarkPicture set junk = 1 ,EditDate=GETDATE(),EditName='{Sci.Env.User.UserID}'
where Ukey = '{this.CurrentMaintain["Ukey"]}'
";
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
            string sqlcmd = $@"update ShippingMarkPicture set junk = 0,EditDate=GETDATE(),EditName='{Sci.Env.User.UserID}'
where Ukey = '{this.CurrentMaintain["Ukey"]}'
";
            DualResult result = DBProxy.Current.Execute(null, sqlcmd);
            if (!result)
            {
                this.ShowErr(result);
            }
        }

        private void TxtStickerComb_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string cmd = $@"
SELECT  [Shipping Mark Combination ID]=ID
FROM ShippingMarkCombination
WHERE BrandID='{this.CurrentMaintain["BrandID"]}'
AND Category='{this.CurrentMaintain["Category"]}'
AND Junk=0";
            DataTable dt;
            DBProxy.Current.Select(null, cmd, out dt);
            SelectItem item = new SelectItem(cmd, "Shipping Mark Combination ID", "10", string.Empty, "Shipping Mark Combination ID");
            DialogResult returnResult = item.ShowDialog();

            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            cmd = $@"
SELECT  Ukey ,IsMixPack
FROM ShippingMarkCombination
WHERE BrandID='{this.CurrentMaintain["BrandID"]}'
AND Category='{this.CurrentMaintain["Category"]}'
AND Junk=0
AND ID = '{item.GetSelectedString()}'
";

            DataRow dr;
            MyUtility.Check.Seek(cmd, out dr);
            IList<DataRow> selectedData = item.GetSelecteds();
            this.txtStickerComb.Text = item.GetSelectedString();
            this.CurrentMaintain["ShippingMarkCombinationUkey"] = MyUtility.Convert.GetInt(dr["Ukey"]);
            this.chkIsMixPack.Checked = MyUtility.Convert.GetBool(dr["IsMixPack"]);
        }

        private void TxtStickerComb_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string newStickerComb = this.txtStickerComb.Text;

            if (this.txtStickerComb.OldValue != newStickerComb)
            {
                if (MyUtility.Check.Empty(newStickerComb))
                {
                    this.CurrentMaintain["ShippingMarkCombinationUkey"] = DBNull.Value;
                    this.txtStickerComb.Text = string.Empty;

                    // 刪除表身
                    foreach (DataRow del in this.DetailDatas)
                    {
                        del.Delete();
                    }

                    return;
                }

                string cmd = $@"
SELECT  [Shipping Mark Combination ID]=ID ,Ukey ,IsMixPack
FROM ShippingMarkCombination
WHERE BrandID='{this.CurrentMaintain["BrandID"]}'
AND ID = @ID
AND Category='{this.CurrentMaintain["Category"]}'
AND Junk=0";
                List<SqlParameter> paras = new List<SqlParameter>();
                paras.Add(new SqlParameter("@ID", newStickerComb));

                DataTable dt;
                DualResult r = DBProxy.Current.Select(null, cmd, paras, out dt);

                if (!r)
                {
                    this.ShowErr(r);
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    this.CurrentMaintain["ShippingMarkCombinationUkey"] = DBNull.Value;
                    this.txtStickerComb.Text = string.Empty;

                    // 刪除表身
                    foreach (DataRow del in this.DetailDatas)
                    {
                        del.Delete();
                    }

                    MyUtility.Msg.WarningBox("Data not found !!");
                }
                else
                {
                    this.CurrentMaintain["ShippingMarkCombinationUkey"] = MyUtility.Convert.GetInt(dt.Rows[0]["Ukey"]);
                    this.txtStickerComb.Text = newStickerComb;
                    this.chkIsMixPack.Checked = MyUtility.Convert.GetBool(dt.Rows[0]["IsMixPack"]);
                    this.AutoInsertDetail();
                }
            }
        }

        private void TxtCTNRefno_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string textValue = this.txtCTNRefno.Text;
            if (!string.IsNullOrWhiteSpace(textValue) && textValue != this.txtCTNRefno.OldValue)
            {
                if (!MyUtility.Check.Seek($"Select 1 from LocalItem WITH (NOLOCK) where Junk = 0 and Category='CARTON' and RefNo = '{textValue}'"))
                {
                    this.txtCTNRefno.Text = string.Empty;
                    this.CurrentMaintain["CtnHeight"] = 0;
                    MyUtility.Msg.WarningBox(string.Format("< RefNo : {0} > not found!!!", textValue));
                    return;
                }
                else
                {
                    string cmd = $@"
SELECT CASE WHEN CtnUnit = 'Inch' THEN CtnHeight * 25.4
            WHEN CtnUnit = 'MM' THEN CtnHeight * 1
            ELSE CtnHeight 
        END
FROM LocalItem
WHERE RefNo='{textValue}'
";
                    decimal height = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup(cmd));

                    this.disCtnHeight.Value = height;
                }
            }

            if (MyUtility.Check.Empty(textValue))
            {
                this.disCtnHeight.Value = 0;
            }

            DataTable currentDt = (DataTable)this.detailgridbs.DataSource;

            foreach (DataRow current in currentDt.Rows)
            {
                bool isHorizontal = MyUtility.Convert.GetBool(current["IsHorizontal"]);
                string stickerSizeID = MyUtility.Convert.GetString(current["StickerSizeID"]);
                decimal ctnHeight = MyUtility.Convert.GetDecimal(this.disCtnHeight.Value);
                decimal fromBottom = MyUtility.Convert.GetDecimal(current["FromBottom"]);
                decimal width = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup($"SELECT Width FROM StickerSize WHERE ID = '{stickerSizeID}'"));
                decimal leghth = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup($"SELECT Length FROM StickerSize WHERE ID = '{stickerSizeID}'"));
                bool isOverCtnH = false;
                bool notAutomate = false;

                if (isHorizontal)
                {
                    isOverCtnH = (fromBottom + width) > ctnHeight;
                }
                else
                {
                    isOverCtnH = (fromBottom + leghth) > ctnHeight;
                }

                notAutomate = isOverCtnH;

                current["IsOverCtnHt"] = isOverCtnH;
                current["NotAutomate"] = notAutomate;
            }
        }

        private void AutoInsertDetail()
        {
            // 刪除表身
            foreach (DataRow del in this.DetailDatas)
            {
                del.Delete();
            }

            string cmd = $@"
SELECT c.ID,b.Seq,c.IsSSCC ,a.IsMixPack ,c.Ukey
FROM ShippingMarkCombination a
INNER JOIN ShippingMarkCombination_Detail b ON a.Ukey = b.ShippingMarkCombinationUkey
INNER JOIN ShippingMarkType c ON b.ShippingMarkTypeUkey = c.Ukey
WHERE a.Ukey = '{this.CurrentMaintain["ShippingMarkCombinationUkey"]}'
";

            DataTable dt;
            DualResult r = DBProxy.Current.Select(null, cmd, out dt);

            if (!r)
            {
                this.ShowErr(r);
                return;
            }

            foreach (DataRow dr in dt.Rows)
            {
                DataTable detailDt = (DataTable)this.detailgridbs.DataSource;
                if (detailDt != null)
                {
                    DataRow ndr = detailDt.NewRow();

                    ndr["ShippingMarkTypeUkey"] = dr["Ukey"];
                    ndr["ShippingMarkTypeID"] = dr["ID"];
                    ndr["Seq"] = dr["Seq"];
                    ndr["IsSSCC"] = dr["IsSSCC"];
                    ndr["Side"] = "A";
                    detailDt.Rows.Add(ndr);
                }
            }

            // 表示B06表身是空的
            if (dt.Rows.Count == 0)
            {
                this.chkIsMixPack.Checked = false;
            }
            else
            {
                this.chkIsMixPack.Checked = MyUtility.Convert.GetBool(dt.Rows[0]["IsMixPack"]);
            }
        }

        private void ReloadtDetail()
        {
            string cmd = $@"
SELECT c.ID,b.Seq,c.IsSSCC ,a.IsMixPack ,c.Ukey
FROM ShippingMarkCombination a
INNER JOIN ShippingMarkCombination_Detail b ON a.Ukey = b.ShippingMarkCombinationUkey
INNER JOIN ShippingMarkType c ON b.ShippingMarkTypeUkey = c.Ukey
WHERE a.Ukey = '{this.CurrentMaintain["ShippingMarkCombinationUkey"]}'
";

            DataTable dt;

            // 取得最新資料
            DualResult r = DBProxy.Current.Select(null, cmd, out dt);

            if (!r)
            {
                this.ShowErr(r);
                return;
            }

            DataTable currentDt = (DataTable)this.detailgridbs.DataSource;

            // 新資料更新到現有資料
            foreach (DataRow lastDr in dt.Rows)
            {
                if (currentDt.AsEnumerable().Where(o => MyUtility.Convert.GetInt(o["ShippingMarkTypeUkey"]) == MyUtility.Convert.GetInt(lastDr["Ukey"])).Any())
                {
                    // 目前已經存在的：只需要更新 Seq, SSCC
                    foreach (DataRow nowRow in this.DetailDatas.Where(o => MyUtility.Convert.GetInt(o["ShippingMarkTypeUkey"]) == MyUtility.Convert.GetInt(lastDr["Ukey"])))
                    {
                        nowRow["Seq"] = lastDr["Seq"];
                        nowRow["IsSSCC"] = lastDr["IsSSCC"];
                    }
                }
                else
                {
                    // 目前不存在的：新增資料

                    DataRow ndr = currentDt.NewRow();

                    ndr["ShippingMarkTypeUkey"] = lastDr["Ukey"];
                    ndr["ShippingMarkTypeID"] = lastDr["ID"];
                    ndr["Seq"] = lastDr["Seq"];
                    ndr["IsSSCC"] = lastDr["IsSSCC"];
                    ndr["Side"] = "A";
                    currentDt.Rows.Add(ndr);
                }
            }

            // 現有資料一筆一筆回去檢查最新資料，如果最新資料沒有，則把該資料刪除
            foreach (DataRow current in this.DetailDatas)
            {
                if (!dt.AsEnumerable().Where(o => MyUtility.Convert.GetInt(o["Ukey"]) == MyUtility.Convert.GetInt(current["ShippingMarkTypeUkey"])).Any())
                {
                    current.Delete();
                }
            }

            // 表示B06表身是空的
            if (dt.Rows.Count == 0)
            {
                this.chkIsMixPack.Checked = false;
            }
            else
            {
                this.chkIsMixPack.Checked = MyUtility.Convert.GetBool(dt.Rows[0]["IsMixPack"]);
            }
        }

        private void TxtCTNRefno_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem("Select RefNo  from LocalItem WITH (NOLOCK) where Junk = 0 and Category='CARTON' ", null, this.txtCTNRefno.Text);

            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            string cmd = $@"
            SELECT CASE WHEN CtnUnit = 'Inch' THEN CtnHeight * 25.4
            			WHEN CtnUnit = 'MM' THEN CtnHeight * 1
            			ELSE CtnHeight 
            		END
            FROM LocalItem
            WHERE RefNo='{item.GetSelectedString()}'
            ";

            decimal height = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup(cmd));

            this.disCtnHeight.Value = height;

            DataTable currentDt = (DataTable)this.detailgridbs.DataSource;

            foreach (DataRow current in currentDt.Rows)
            {
                bool isHorizontal = MyUtility.Convert.GetBool(current["IsHorizontal"]);
                string stickerSizeID = MyUtility.Convert.GetString(current["StickerSizeID"]);
                decimal ctnHeight = MyUtility.Convert.GetDecimal(this.disCtnHeight.Value);
                decimal fromBottom = MyUtility.Convert.GetDecimal(current["FromBottom"]);
                decimal width = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup($"SELECT Width FROM StickerSize WHERE ID = '{stickerSizeID}'"));
                decimal leghth = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup($"SELECT Length FROM StickerSize WHERE ID = '{stickerSizeID}'"));
                bool isOverCtnH = false;
                bool notAutomate = false;

                if (isHorizontal)
                {
                    isOverCtnH = (fromBottom + width) > ctnHeight;
                }
                else
                {
                    isOverCtnH = (fromBottom + leghth) > ctnHeight;
                }

                notAutomate = isOverCtnH;

                current["IsOverCtnHt"] = isOverCtnH;
                current["NotAutomate"] = notAutomate;
            }

            this.txtCTNRefno.Text = item.GetSelectedString();
        }

        private void Txtbrand1_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string newBrandID = this.txtbrand1.Text;

            if (this.txtbrand1.OldValue != newBrandID)
            {
                this.CurrentMaintain["ShippingMarkCombinationUkey"] = DBNull.Value;
                this.txtStickerComb.Text = string.Empty;

                // 刪除表身
                foreach (DataRow del in this.DetailDatas)
                {
                    del.Delete();
                }

                this.CurrentMaintain["BrandID"] = newBrandID;
            }
        }

        private void ComboCategory_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this.CurrentMaintain["Category"] = this.comboCategory.SelectedValue;
            if (this.EditMode)
            {
                this.CurrentMaintain["ShippingMarkCombinationUkey"] = DBNull.Value;
                this.txtStickerComb.Text = string.Empty;

                // 刪除表身重新匯入
                foreach (DataRow del in this.DetailDatas)
                {
                    del.Delete();
                }
            }

            this.ChangeVisible(MyUtility.Convert.GetString(this.CurrentMaintain["Category"]));
        }

        private void ChangeVisible(string category)
        {
            if (category == "HTML")
            {
                this.detailgrid.Columns["IsSSCC"].Visible = false;
                this.detailgrid.Columns["Is2Side"].Visible = false;
                this.detailgrid.Columns["IsHorizontal"].Visible = false;
                this.detailgrid.Columns["IsOverCtnHt"].Visible = false;
                this.detailgrid.Columns["NotAutomate"].Visible = false;
            }

            if (category == "PIC")
            {
                this.detailgrid.Columns["IsSSCC"].Visible = true;
                this.detailgrid.Columns["Is2Side"].Visible = true;
                this.detailgrid.Columns["IsHorizontal"].Visible = true;
                this.detailgrid.Columns["IsOverCtnHt"].Visible = true;
                this.detailgrid.Columns["NotAutomate"].Visible = true;
            }
        }

        private void BtnReloadSticker_Click(object sender, EventArgs e)
        {
            try
            {
                this.ReloadtDetail();
                MyUtility.Msg.InfoBox("Finish!");
            }
            catch (Exception ex)
            {
                this.ShowErr(ex);
            }
        }

        private void CaculateCtnHeight(ref DataRow current)
        {
            bool isHorizontal = MyUtility.Convert.GetBool(current["IsHorizontal"]);
            string stickerSizeID = MyUtility.Convert.GetString(current["StickerSizeID"]);
            decimal ctnHeight = MyUtility.Convert.GetDecimal(this.disCtnHeight.Value);
            decimal fromBottom = MyUtility.Convert.GetDecimal(current["FromBottom"]);
            decimal width = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup($"SELECT Width FROM StickerSize WHERE ID = '{stickerSizeID}'"));
            decimal leghth = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup($"SELECT Length FROM StickerSize WHERE ID = '{stickerSizeID}'"));
            bool isOverCtnH = false;
            bool notAutomate = false;

            if (isHorizontal)
            {
                isOverCtnH = (fromBottom + width) > ctnHeight;
            }
            else
            {
                isOverCtnH = (fromBottom + leghth) > ctnHeight;
            }

            notAutomate = isOverCtnH;

            current["IsOverCtnHt"] = isOverCtnH;
            current["NotAutomate"] = notAutomate;
        }
    }
}
