using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.PublicPrg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    /// <summary>
    /// P72_Import
    /// </summary>
    public partial class P72_Import : Win.Tems.QueryForm
    {
        private DataTable mainDetail;
        private DataTable dtInventory;

        /// <summary>
        /// P72_Import
        /// </summary>
        /// <param name="mainDetail">mainDetail</param>
        public P72_Import(DataTable mainDetail)
        {
            this.InitializeComponent();
            this.EditMode = true;
            this.mainDetail = mainDetail;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            #region -- Reason Combox --
            string selectCommand = @"select Name idname,id from Reason WITH (NOLOCK) where ReasonTypeID='Stock_Adjust' AND junk = 0";
            DualResult returnResult;
            DataTable dropDownListTable = new DataTable();
            if (returnResult = DBProxy.Current.Select(null, selectCommand, out dropDownListTable))
            {
                this.comboReason.DataSource = dropDownListTable;
                this.comboReason.DisplayMember = "IDName";
                this.comboReason.ValueMember = "ID";
            }
            #endregion

            DataGridViewGeneratorNumericColumnSettings colQtyAfter = new DataGridViewGeneratorNumericColumnSettings();
            colQtyAfter.CellValidating += (s, e) =>
            {
                if (this.EditMode && !MyUtility.Check.Empty(e.FormattedValue))
                {
                    this.gridImport.EndEdit();
                    decimal qtyAfter = (decimal)this.gridImport.Rows[e.RowIndex].Cells["OriginalQty"].Value;
                    decimal qtyBefore = (decimal)this.gridImport.Rows[e.RowIndex].Cells["CurrentQty"].Value;
                    this.gridImport.Rows[e.RowIndex].Cells["CurrentQty"].Value = e.FormattedValue;
                    this.gridImport.Rows[e.RowIndex].Cells["AdjustQty"].Value = qtyBefore - qtyAfter;
                    this.gridImport.Rows[e.RowIndex].Cells["selected"].Value = true;
                    this.gridImport.RefreshEdit();
                }
            };

            #region -- Reason ID 右鍵開窗 --
            DataGridViewGeneratorTextColumnSettings ts = new DataGridViewGeneratorTextColumnSettings();
            ts.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    DataTable poitems;
                    string sqlcmd = string.Empty;
                    IList<DataRow> x;

                    sqlcmd = @"select id, Name from Reason WITH (NOLOCK) where ReasonTypeID='Stock_Adjust' AND junk = 0";
                    DualResult result2 = DBProxy.Current.Select(null, sqlcmd, out poitems);
                    if (!result2)
                    {
                        this.ShowErr(sqlcmd, result2);
                        return;
                    }

                    Win.Tools.SelectItem item = new Win.Tools.SelectItem(
                        poitems,
                        "ID,Name",
                        "5,150",
                        this.gridImport.GetDataRow(this.gridImport.GetSelectedRowIndex())["ReasonID"].ToString(),
                        "ID,Name");
                    item.Width = 600;
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    x = item.GetSelecteds();

                    this.gridImport.GetDataRow(this.gridImport.GetSelectedRowIndex())["ReasonID"] = x[0]["id"];
                    this.gridImport.GetDataRow(this.gridImport.GetSelectedRowIndex())["ReasonName"] = x[0]["name"];
                }
            };
            ts.CellValidating += (s, e) =>
            {
                DataRow dr;
                if (!this.EditMode)
                {
                    return;
                }

                if (string.Compare(e.FormattedValue.ToString(), this.gridImport.GetDataRow(this.gridImport.GetSelectedRowIndex())["ReasonID"].ToString()) != 0)
                {
                    if (MyUtility.Check.Empty(e.FormattedValue))
                    {
                        this.gridImport.GetDataRow(this.gridImport.GetSelectedRowIndex())["ReasonID"] = string.Empty;
                        this.gridImport.GetDataRow(this.gridImport.GetSelectedRowIndex())["ReasonName"] = string.Empty;
                    }
                    else
                    {
                        if (!MyUtility.Check.Seek(
                            string.Format(
                            @"select id, Name from Reason WITH (NOLOCK) where id = '{0}' 
and ReasonTypeID='Stock_Adjust' AND junk = 0", e.FormattedValue), out dr, null))
                        {
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox($"<{e.FormattedValue}> @ReasonID not found!", "Reason ID");
                            return;
                        }
                        else
                        {
                            this.gridImport.GetDataRow(this.gridImport.GetSelectedRowIndex())["ReasonID"] = e.FormattedValue;
                            this.gridImport.GetDataRow(this.gridImport.GetSelectedRowIndex())["ReasonName"] = dr["name"];
                        }
                    }
                }
            };
            #endregion

            this.gridImport.IsEditingReadOnly = false; // 必設定, 否則CheckBox會顯示圖示
            this.gridImport.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridImport)
            .CheckBox("selected", trueValue: 1, falseValue: 0, iseditable: true)
            .Text("POID", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("Seq", header: "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Text("MaterialType", header: "MaterialType", width: Widths.AnsiChars(9), iseditingreadonly: true)
            .EditText("Desc", header: "Description", width: Widths.AnsiChars(25), iseditingreadonly: true)
            .Text("Color", header: "Color", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("Roll", header: "Roll", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("Tone", header: "Tone/Grp", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Numeric("OriginalQty", header: "Original Qty", decimal_places: 2, width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Numeric("CurrentQty", header: "Current Qty", decimal_places: 2, width: Widths.AnsiChars(8), settings: colQtyAfter)
            .Numeric("AdjustQty", header: "Adjust Qty", decimal_places: 2, width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("Unit", header: "Unit", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("Location", header: "Location", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("ReasonID", header: "Reason ID", width: Widths.AnsiChars(10), iseditingreadonly: false, settings: ts)
            .Text("ReasonName", header: "Reason Name", width: Widths.AnsiChars(15), iseditingreadonly: true)
            ;
        }

        private void Query()
        {
            if (MyUtility.Check.Empty(this.txtSP.Text))
            {
                MyUtility.Msg.WarningBox("SP# can not be empty");
                return;
            }

            string strwhere = string.Empty;
            if (!MyUtility.Check.Empty(this.txtSeq1.Text))
            {
                strwhere += $" and loi.Seq1 = '{this.txtSeq1.Text}'";
            }

            if (!MyUtility.Check.Empty(this.txtseq2.Text))
            {
                strwhere += $" and loi.Seq2 = '{this.txtseq2.Text}'";
            }

            if (!MyUtility.Check.Empty(this.txtLocation.Text))
            {
                strwhere += $" and Location.val like '%{this.txtLocation.Text}%' ";
            }

            string sqlQuery = $@"
select  [selected] = 0,
        loi.POID,
        [Seq] = CONCAT(loi.Seq1,' ',loi.Seq2),
		[MaterialType] = CASE lom.FabricType 
					WHEN 'F' then CONCAT('Fabric-',lom.MtlType)
					WHEN 'A' THEN CONCAT('Accessory-',lom.MtlType) ELSE '' END,
        lom.FabricType,
        loi.Seq1,loi.Seq2,
		lom.[Desc],
		lom.Color,        
        loi.Roll,
        loi.Dyelot,
        loi.Tone,        
        lom.Unit,
        [OriginalQty] = loi.InQty - loi.OutQty + loi.AdjustQty,
        [CurrentQty] = 0.00,
		[AdjustQty] = 0.00 - (loi.InQty - loi.OutQty + loi.AdjustQty),
        [Location] = Location.val,
        loi.StockType,
		[ReasonID] = '',
		[ReasonName] = '',
        [LocalOrderInventoryUkey] = loi.Ukey
from    LocalOrderInventory loi with (nolock)
left join  LocalOrderMaterial lom with (nolock) on loi.Poid = lom.Poid and loi.Seq1 = lom.Seq1 and loi.Seq2 = lom.Seq2
outer apply (
	SELECT val =  Stuff((select distinct concat( ',',MtlLocationID)   
	from LocalOrderInventory_Location loil with (nolock)
	WHERE loil.LocalOrderInventoryUkey	 = loi.Ukey
	FOR XML PATH('')),1,1,'')  
) Location
where   (loi.InQty - loi.OutQty + loi.AdjustQty) > 0 
and loi.StockType = 'B' 
and loi.POID = '{this.txtSP.Text}'
{strwhere}
";
            DualResult result = DBProxy.Current.Select(null, sqlQuery, out this.dtInventory);

            if (!result)
            {
                this.ShowErr(result);
                return;
            }
            else
            {
                if (this.dtInventory.Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!!");
                }

                this.listControlBindingSource1.DataSource = this.dtInventory;
            }
        }

        private void BtnFind_Click(object sender, EventArgs e)
        {
            this.Query();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            this.gridImport.ValidateControl();
            DataTable dtGridBS1 = (DataTable)this.listControlBindingSource1.DataSource;
            if (MyUtility.Check.Empty(dtGridBS1) || dtGridBS1.Rows.Count == 0)
            {
                return;
            }

            DataRow[] dr2 = dtGridBS1.Select("Selected = 1");
            if (dr2.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select rows first!", "Warnning");
                return;
            }

            dr2 = dtGridBS1.Select("adjustqty = 0 and Selected = 1");
            if (dr2.Length > 0)
            {
                MyUtility.Msg.WarningBox("Adjust Qty of selected row can't be zero!", "Warning");
                return;
            }

            dr2 = dtGridBS1.Select("adjustqty <> 0 and Selected = 1");
            var checkMainDetail = this.mainDetail.AsEnumerable().Where(s => s.RowState != DataRowState.Deleted);

            foreach (DataRow drImportSource in dr2)
            {
                if (drImportSource.RowState != DataRowState.Deleted)
                {
                    DataRow[] findrow = this.mainDetail.Select(string.Format("LocalOrderInventoryUkey = {0}", drImportSource["LocalOrderInventoryUkey"]));
                    if (findrow.Length > 0)
                    {
                        findrow[0]["QtyBefore"] = drImportSource["OriginalQty"];
                        findrow[0]["QtyAfter"] = drImportSource["CurrentQty"];
                        findrow[0]["AdjustQty"] = drImportSource["AdjustQty"];
                        findrow[0]["ReasonID"] = drImportSource["ReasonID"];
                        findrow[0]["ReasonName"] = drImportSource["ReasonName"];
                        continue;
                    }
                    else
                    {
                        DataRow drMainDetail = this.mainDetail.NewRow();
                        drMainDetail["POID"] = drImportSource["POID"];
                        drMainDetail["Seq"] = drImportSource["Seq"];
                        drMainDetail["Seq1"] = drImportSource["Seq1"];
                        drMainDetail["Seq2"] = drImportSource["Seq2"];
                        drMainDetail["Roll"] = drImportSource["Roll"];
                        drMainDetail["Dyelot"] = drImportSource["Dyelot"];
                        drMainDetail["Color"] = drImportSource["Color"];
                        drMainDetail["Tone"] = drImportSource["Tone"];
                        drMainDetail["FabricType"] = drImportSource["FabricType"];
                        drMainDetail["StockType"] = "B";
                        drMainDetail["QtyBefore"] = drImportSource["OriginalQty"];
                        drMainDetail["QtyAfter"] = drImportSource["CurrentQty"];
                        drMainDetail["AdjustQty"] = drImportSource["AdjustQty"];
                        drMainDetail["Desc"] = drImportSource["Desc"];
                        drMainDetail["Unit"] = drImportSource["Unit"];
                        drMainDetail["Location"] = drImportSource["Location"];
                        drMainDetail["ReasonID"] = drImportSource["ReasonID"];
                        drMainDetail["ReasonName"] = drImportSource["ReasonName"];
                        drMainDetail["LocalOrderInventoryUkey"] = drImportSource["LocalOrderInventoryUkey"];
                        this.mainDetail.Rows.Add(drMainDetail);
                    }
                }
            }

            MyUtility.Msg.InfoBox("Import complete!!");
        }

        private void btnUpdateAll_Click(object sender, EventArgs e)
        {
            string reasonid = this.comboReason.SelectedValue.ToString();
            this.gridImport.ValidateControl();

            if (this.dtInventory == null || this.dtInventory.Rows.Count == 0)
            {
                return;
            }

            DataRow[] drfound = this.dtInventory.Select("selected = 1");

            foreach (var item in drfound)
            {
                item["ReasonID"] = reasonid;
                item["ReasonName"] = this.comboReason.Text;
            }
        }
    }
}
