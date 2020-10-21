using Ict.Win;
using Sci.Data;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Ict;
using System.Data.SqlClient;
using Ict.Win.UI;

namespace Sci.Production.Quality
{
    /// <inheritdoc/>
    public partial class P30 : Win.Tems.Input6
    {
        private Size thisSize;
        private bool FirstTime = true;

        /// <inheritdoc/>
        // (menuitem, args= 參數)
        public P30(ToolStripMenuItem menuitem, string history)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.thisSize = this.Size;
            this.FirstTime = false;

            // 設定init()
            string mDivisionID = Env.User.Keyword;
            if (history == "1".ToString())
            {
                this.DefaultFilter = string.Format("MDivisionID= '{0}' and MDClose is null and orders.IsForecast<>1", mDivisionID);
                this.Text = "P30 .MD Master List";
            }
            else if (history == "2".ToString())
            {
                this.DefaultFilter = string.Format("MDivisionID= '{0}' and MDClose is not null", mDivisionID);
                this.Text = "P31 .MD Master List(History)";
                this.IsSupportEdit = false;
            }
        }

        /// <inheritdoc/>
        protected override void OnEditModeChanged()
        {
            if (this.FirstTime)
            {
                return;
            }

            base.OnEditModeChanged();
            this.Button_enable();
        }

        /// <inheritdoc/>
        public void ColorSelect_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            this.ColorSelect_CellMouseClick(e.Button, e.RowIndex);
        }

        /// <inheritdoc/>
        public void ColorSelect_CellMouseClick(object sender, DataGridViewEditingControlMouseEventArgs e)
        {
            this.ColorSelect_CellMouseClick(e.Button, e.RowIndex);
        }

        /// <inheritdoc/>
        public void ColorSelect_CellMouseClick(MouseButtons eButton, int eRowIndex)
        {
            if (eButton == MouseButtons.Right)
            {
                DataRow dr = this.detailgrid.GetDataRow<DataRow>(eRowIndex);
                if (dr == null)
                {
                    return;
                }

                if (dr.ItemArray[1].ToString().ToUpper() == "CUTPARTS" || dr.ItemArray[1].ToString().ToUpper() == "GARMENT")
                {
                    return;
                }

                string sqlcmd = string.Format(
                    @" 
select ColorID
from (
	select distinct ColorID = dbo.GetColorMultipleID(a.BrandId, ColorID)
		   , RowNum = ROW_NUMBER() over (order by ColorID, a.BrandID)
	from po_supp_detail a WITH (NOLOCK) 
		 , Orders b WITH (NOLOCK)  
	where a.id = b.POID 
		  and a.fabrictype = 'A' 
		  and colorid is not null 
		  and b.id='{0}' 
		  and a.ColorID != ''
	Group by ColorID, a.BrandID
)x
order by RowNum", this.txtSP.Text.ToString());
                SelectItem item = new SelectItem(sqlcmd, "30", dr["ColorID"].ToString());
                DialogResult result = item.ShowDialog();
                if (result == DialogResult.Cancel)
                {
                    return;
                }

                dr["Colorid"] = item.GetSelectedString();
            }
        }

        /// <inheritdoc/>
        public void ItemSelect_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            this.ItemSelect_CellMouseClick(e.Button, e.RowIndex);
        }

        /// <inheritdoc/>
        public void ItemSelect_CellMouseClick(object sender, DataGridViewEditingControlMouseEventArgs e)
        {
            this.ItemSelect_CellMouseClick(e.Button, e.RowIndex);
        }

        /// <inheritdoc/>
        public void ItemSelect_CellMouseClick(MouseButtons eButton, int eRowIndex)
        {
            DataRow dr1 = this.detailgrid.GetDataRow<DataRow>(eRowIndex);
            if (dr1["Type"].ToString().ToUpper() != "ACCESSORY ITEMS")
            {
                return;
            }

            if (eButton == MouseButtons.Right)
            {
                if (!this.EditMode)
                {
                    return;
                }

                // DataRow dr1 = this.detailgrid.GetDataRow<DataRow>(eRowIndex);
                if (dr1 == null)
                {
                    return;
                }

                if (dr1.ItemArray[1].ToString().ToUpper() == "CUTPARTS" || dr1.ItemArray[1].ToString().ToUpper() == "GARMENT")
                {
                    return;
                }

                string sqlcmd1 = string.Format(
                    @"select distinct refno from PO_Supp_Detail a WITH (NOLOCK) ,Orders b WITH (NOLOCK)  where a.id=b.POID and a.fabrictype='A'
                and a.Scirefno is not null 
                and b.id='{0}' group by a.refno",
                    this.txtSP.Text.ToString());
                SelectItem item1 = new SelectItem(sqlcmd1, "30", dr1["Item"].ToString());
                DialogResult result1 = item1.ShowDialog();
                if (result1 == DialogResult.Cancel)
                {
                    return;
                }

                dr1["Item"] = item1.GetSelectedString();
            }
        }

        // 設定Grid內容值

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            #region OnClick Right Click Even
            DataGridViewGeneratorTextColumnSettings colorSelect = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings itemSelect = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings typeSetting = new DataGridViewGeneratorTextColumnSettings();
            typeSetting.CharacterCasing = CharacterCasing.Normal;
            colorSelect.CellMouseClick += this.ColorSelect_CellMouseClick;
            colorSelect.EditingMouseDown += this.ColorSelect_CellMouseClick;
            itemSelect.CellEditable += (s, e) =>
            {
                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (dr["Type"].ToString().ToUpper() == "ACCESSORY ITEMS")
                {
                    e.IsEditable = true;
                }
                else
                {
                    e.IsEditable = false;
                }
            };

            itemSelect.CellValidating += (s, e) =>
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

                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                DataRow dr1;
                string sqlcmd = string.Format(
                    @"select  refno from PO_Supp_Detail a WITH (NOLOCK) ,Orders b WITH (NOLOCK) where a.id=b.POID and a.fabrictype='A'
                and a.Scirefno is not null 
                and b.id='{0}' and a.refno='{1}'",
                    this.txtSP.Text.ToString(), e.FormattedValue);

                if (MyUtility.Check.Seek(sqlcmd, out dr1))
                {
                    dr["Item"] = e.FormattedValue;
                }
                else
                {
                    dr["Item"] = string.Empty;
                    dr.EndEdit();
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("<Item: {0}> does not exist!", e.FormattedValue));
                    return;
                }
            };
            colorSelect.CellEditable += (s, e) =>
            {
                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);

                if (dr["Type"].ToString().ToUpper() == "ACCESSORY ITEMS")
                {
                    e.IsEditable = true;
                }
                else
                {
                    e.IsEditable = false;
                }
            };

            colorSelect.CellValidating += (s, e) =>
            {
                if (this.EditMode == false)
                {
                    return;
                }

                if (MyUtility.Check.Empty(e.FormattedValue))
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                DataRow dr1;

                string sqlcmd = string.Format(
                    @" 
select ColorID
from (
	select distinct ColorID = dbo.GetColorMultipleID(a.BrandId, ColorID)
	from po_supp_detail a WITH (NOLOCK) 
		 , Orders b WITH (NOLOCK)  
	where a.id = b.POID 
		  and a.fabrictype = 'A' 
		  and colorid is not null 
		  and b.id = '{0}' 
		  and a.ColorID != ''
	Group by ColorID, a.BrandID
)x
where ColorID = '{1}'", this.txtSP.Text.ToString(), e.FormattedValue);
                if (MyUtility.Check.Seek(sqlcmd, out dr1))
                {
                    dr["Colorid"] = e.FormattedValue;
                }
                else
                {
                    dr["Colorid"] = string.Empty;
                    dr.EndEdit();
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("<Color: {0}> does not exist!", e.FormattedValue));
                    return;
                }
            };

            // colorSelect.EditingMouseDown
            itemSelect.CellMouseClick += this.ItemSelect_CellMouseClick;
            itemSelect.EditingMouseDown += this.ItemSelect_CellMouseClick;

            #endregion

            // 設定Grid屬性 text 對應欄位值 header :顯示欄位名稱
            this.Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("Type", header: "Main Item NO", width: Ict.Win.Widths.AnsiChars(15), iseditingreadonly: true, iseditable: true, settings: typeSetting)
            .Text("Item", header: "SEQ Ref", width: Ict.Win.Widths.AnsiChars(15), settings: itemSelect)
            .Text("Colorid", header: "Color", width: Ict.Win.Widths.AnsiChars(10), iseditingreadonly: true, settings: colorSelect)
            .Date("inspdate", header: "Inspdate", width: Ict.Win.Widths.AnsiChars(10))
            .Text("Result", header: "Result", width: Ict.Win.Widths.AnsiChars(20));
            this.detailgrid.ValidateControl();

            this.detailgrid.RowPostPaint += (s, e) =>
            {
                if (this.EditMode == true)
                {
                    this.detailgrid.Rows[e.RowIndex].Cells["Colorid"].Style.ForeColor = Color.Red;
                }
                else
                {
                    this.detailgrid.Rows[e.RowIndex].Cells["Colorid"].Style.ForeColor = Color.Black;
                }
            };
        }

        // When click Edit button and Grid is empty then New 5 column in GridView
        // choice ClickEditAfter becauser Transaction problemm

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            #region 設定表頭欄位只能Readonly
                this.txtSP.ReadOnly = true;
                this.txtBrand.ReadOnly = true;
                this.txtStyle.ReadOnly = true;
                this.txtSeason.ReadOnly = true;
                this.txtProject.ReadOnly = true;
                this.numOrderQty.ReadOnly = true;
                this.txtOrderQty.ReadOnly = true;
                this.dateMDFinished.ReadOnly = true;
                this.txtuserMCHandle.TextBox1.ReadOnly = true;
                this.dateSewingInline.ReadOnly = true;
                this.dateSewingOffline.ReadOnly = true;
                this.dateBuyerDelivery.ReadOnly = true;
                this.dateSDPDate.ReadOnly = true;
                this.dateRMTLETA.ReadOnly = true;
                this.comboCategory.ReadOnly = true;
                this.checkLocalOrder.ReadOnly = true;
                this.checkPullForwardOrder.ReadOnly = true;
                this.checkCancelledOrder.ReadOnly = true;

            #endregion
                DataRow row = this.detailgrid.GetDataRow(this.detailgridbs.Position);
                if (MyUtility.Check.Empty(row))
            {
                string id = this.txtSP.Text;

                DataTable detailDt = (DataTable)this.detailgridbs.DataSource;

                // Row 1
                DataRow newRow1 = detailDt.NewRow();
                newRow1["Type"] = "Accessory Items";
                newRow1["ID"] = id;
                detailDt.Rows.Add(newRow1);
                this.DetailDatas.Add(newRow1);

                // Row 2
                DataRow newRow2 = detailDt.NewRow();
                newRow2["Type"] = "Accessory Items";
                newRow2["ID"] = id;
                detailDt.Rows.Add(newRow2);
                this.DetailDatas.Add(newRow2);

                // Row 3
                DataRow newRow3 = detailDt.NewRow();
                newRow3["Type"] = "CutParts";
                newRow3["ID"] = id;
                detailDt.Rows.Add(newRow3);
                this.DetailDatas.Add(newRow3);

                // Row 4
                DataRow newRow4 = detailDt.NewRow();
                newRow4["Type"] = "Garment";
                newRow4["Item"] = "First MD";
                newRow4["ID"] = id;
                detailDt.Rows.Add(newRow4);
                this.DetailDatas.Add(newRow4);

                // Row 5
                DataRow newRow5 = detailDt.NewRow();
                newRow5["Type"] = "Garment";
                newRow5["Item"] = "If Open carton";
                newRow5["ID"] = id;
                detailDt.Rows.Add(newRow5);
                this.DetailDatas.Add(newRow5);
            }

                base.ClickEditAfter();
        }

        /// <inheritdoc/>
        protected override void OnDetailGridInsert(int index = -1)
        {
            base.OnDetailGridInsert(index);
            DataTable dt = (DataTable)this.detailgridbs.DataSource;

            this.CurrentDetailData["Type"] = "Accessory items";
        }

        /// <inheritdoc/>
        protected override void OnDetailGridDelete()
        {
            if (this.CurrentDetailData["Type"].StrStartsWith("Cutparts") || this.CurrentDetailData["Type"].StrStartsWith("Garment"))
            {
                MyUtility.Msg.WarningBox("If <Main item no> is Cutparts or Garment cannot delete");
                return;
            }

            base.OnDetailGridDelete();
        }

        // delete rows when the pk value is empty

        /// <inheritdoc/>
        protected override DualResult ClickSavePre()
        {
            if (!MyUtility.Check.Empty(this.CurrentMaintain["ID"]))
            {
                DataTable detailDt = (DataTable)this.detailgridbs.DataSource;
                DataRow row = this.detailgrid.GetDataRow(this.detailgridbs.Position);
                int t = detailDt.Rows.Count;
                for (int i = t - 1; i >= 0; i--)
                {
                    // MD.PK都要有值才能save
                    if (detailDt.Rows[i].RowState != DataRowState.Deleted && (detailDt.Rows[i]["Type"].ToString().ToUpper() == "ACCESSORY ITEMS" && detailDt.Rows[i]["Item"].ToString() == string.Empty))
                    {
                        // 刪除
                        detailDt.Rows[i].Delete();
                    }
                }
            }

            return Ict.Result.True;
        }

        // 表頭combobox

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.Size = this.thisSize;

            DataTable dtCategory;
            DualResult cbResult;
            if (cbResult = DBProxy.Current.Select(null, " select ID,Name from DropDownList WITH (NOLOCK) where type='category'", out dtCategory))
            {
                this.comboCategory.DataSource = dtCategory;
                this.comboCategory.DisplayMember = "Name";
                this.comboCategory.ValueMember = "ID";
            }
            else
            {
                this.ShowErr(cbResult);
            }
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.Button_enable();
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            int a = this.detailgrid.Rows.Count;
            var tab = (DataTable)this.detailgridbs.DataSource;
            int b = tab.Rows.Count;
            DualResult upResult = new DualResult(true);

            DataTable detailDt = (DataTable)this.detailgridbs.DataSource;
            DataRow row = this.detailgrid.GetDataRow(this.detailgridbs.Position);

            int t = detailDt.Rows.Count;

            for (int i = t - 1; i >= 0; i--)
                {
                    if (detailDt.Rows[i].RowState != DataRowState.Deleted && MyUtility.Check.Empty(detailDt.Rows[i]["ID"].ToString()))
                    {
                        MyUtility.Msg.WarningBox("<ID> cannot be null !");
                        return false;
                    }

                    if (detailDt.Rows[i].RowState != DataRowState.Deleted && MyUtility.Check.Empty(detailDt.Rows[i]["Type"].ToString()))
                    {
                        MyUtility.Msg.WarningBox("<Main Item NO> cannot be null !");
                        return false;
                    }
                }

            return base.ClickSaveBefore();
        }

        private void Button_enable()
        {
            DataTable dt;
            string cmd = string.Format("select MDClose from orders where id='{0}' ", this.txtSP.Text);
            DBProxy.Current.Select(null, cmd, out dt);
            if (dt.Rows.Count > 0)
            {
                this.btnFinished.Text = MyUtility.Check.Empty(dt.Rows[0]["MDClose"]) ? "Finished" : "Back to Master List";
                this.dateMDFinished.Value = MyUtility.Convert.GetDate(dt.Rows[0]["MDClose"]);
            }

            this.btnFinished.Enabled = !this.EditMode;
        }

        /// <inheritdoc/>
        protected override DualResult ClickSave()
        {
            return base.ClickSave();
        }

        // Button Finished
        private void BtnFinished_Click(object sender, EventArgs e)
        {
            if (this.btnFinished.Text == "Finished")
            {
                 DialogResult buttonFinished = MyUtility.Msg.QuestionBox("Do you want to finished this order!", "Question", MessageBoxButtons.YesNo);

            // 訊息方塊選擇"NO" MDClose清空
                 if (buttonFinished == DialogResult.Yes)
                 {
                     string sqlCmdUpdate = "update orders  set MDClose= CONVERT(VARCHAR(20), GETDATE(), 120)  where id=@MdID";
                     List<SqlParameter> spam = new List<SqlParameter>();
                     spam.Add(new SqlParameter("@MdID", this.txtSP.Text));
                     DualResult result = DBProxy.Current.Execute("Production", sqlCmdUpdate, spam);
                     if (!result)
                    {
                        return;
                    }
                 }
                 else
                 {
                     return;
                 }
            }
            else
            {
                DialogResult buttonFinished = MyUtility.Msg.QuestionBox("Do you want to back to master list?", "Question", MessageBoxButtons.YesNo);
                if (buttonFinished == DialogResult.Yes)
                {
                      string sqlCmdUpdate = "update orders  set MDClose= Null  where id=@MdID";

                      List<SqlParameter> spam = new List<SqlParameter>();
                      spam.Add(new SqlParameter("@MdID", this.txtSP.Text));
                      DualResult result = DBProxy.Current.Execute("Production", sqlCmdUpdate, spam);

                      if (!result)
                        {
                            return;
                        }
                }
                else
                {
                    return;
                }
            }

            this.OnDetailEntered();

            int rowindex = 0;
            for (int i = 0; i < this.CurrentDataRow.Table.Rows.Count; i++)
            {
                if (this.CurrentDataRow.Table.Rows[i]["id"].ToString() == this.txtSP.Text)
                {
                    rowindex = i;
                    break;
                }
            }

            this.CurrentDataRow["id"] = this.CurrentDataRow.Table.Rows[rowindex + 1]["id"].ToString();
            this.RenewData();
            this.ReloadDatas();
            this.gridbs.Position = rowindex;
        }

        private void BtnFabricInspectionList_Click(object sender, EventArgs e)
        {
            P01 callNextForm = new P01(MyUtility.Convert.GetString(this.txtSP.Text));
            callNextForm.ShowDialog(this);
        }

        private void BtnAccessoryInspectionList_Click(object sender, EventArgs e)
        {
            P02 callNextForm = new P02(MyUtility.Convert.GetString(this.txtSP.Text));
            callNextForm.ShowDialog(this);
        }
    }
}
