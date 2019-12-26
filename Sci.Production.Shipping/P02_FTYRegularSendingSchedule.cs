using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sci.Andy;
using Sci.Andy.ExtensionMethods;
using Ict.Win;
using Ict;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// P31_FTYRegularSendingSchedule
    /// </summary>
    internal partial class P02_FTYRegularSendingSchedule : Sci.Win.Subs.Base
    {
        private DataTable dtFactory;
        private DataTable dtCountry;
        private MyAdapter factoryExpressSendingScheduleAdapter = new MyAdapter("FactoryExpress_SendingSchedule");
        private MyAdapter factoryExpressSendingScheduleHistoryAdapter = new MyAdapter("FactoryExpress_SendingScheduleHistory") { autoDetectFeildInfo = false };
        private Ict.Win.UI.DataGridViewTextBoxColumn col_Factory = new Ict.Win.UI.DataGridViewTextBoxColumn();
        private Ict.Win.UI.DataGridViewTextBoxColumn col_ToAlias = new Ict.Win.UI.DataGridViewTextBoxColumn();
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_SUN = new Ict.Win.UI.DataGridViewCheckBoxColumn();
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_MON = new Ict.Win.UI.DataGridViewCheckBoxColumn();
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_TUE = new Ict.Win.UI.DataGridViewCheckBoxColumn();
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_WED = new Ict.Win.UI.DataGridViewCheckBoxColumn();
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_THU = new Ict.Win.UI.DataGridViewCheckBoxColumn();
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_FRI = new Ict.Win.UI.DataGridViewCheckBoxColumn();
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_SAT = new Ict.Win.UI.DataGridViewCheckBoxColumn();
        private Ict.Win.UI.DataGridViewDateBoxColumn col_BeginDate = new Ict.Win.UI.DataGridViewDateBoxColumn();
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_Junk = new Ict.Win.UI.DataGridViewCheckBoxColumn();

        /// <summary>
        /// P31_FTYRegularSendingSchedule
        /// </summary>
        public P02_FTYRegularSendingSchedule()
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.SetFactoryData();
            this.SetCountryData();
            this.SetupGrid();
            this.RefreshData();
        }

        /// <inheritdoc/>
        protected override void OnEditModeChanged()
        {
            base.OnEditModeChanged();
            this.btnUndoClose.Text = this.EditMode ? "Undo" : "Close";
            this.grid1.IsEditingReadOnly = !this.EditMode;
            this.SetGridColumn_EditableStyle(this.col_Factory, !this.EditMode);
            this.SetGridColumn_EditableStyle(this.col_ToAlias, !this.EditMode);
            this.SetGridColumn_EditableStyle(this.col_SUN, !this.EditMode);
            this.SetGridColumn_EditableStyle(this.col_MON, !this.EditMode);
            this.SetGridColumn_EditableStyle(this.col_TUE, !this.EditMode);
            this.SetGridColumn_EditableStyle(this.col_WED, !this.EditMode);
            this.SetGridColumn_EditableStyle(this.col_THU, !this.EditMode);
            this.SetGridColumn_EditableStyle(this.col_FRI, !this.EditMode);
            this.SetGridColumn_EditableStyle(this.col_SAT, !this.EditMode);
            this.SetGridColumn_EditableStyle(this.col_BeginDate, !this.EditMode);
            this.SetGridColumn_EditableStyle(this.col_Junk, !this.EditMode);
        }

        private void SetupGrid()
        {
            DataGridViewGeneratorTextColumnSettings regionCodeCell = new DataGridViewGeneratorTextColumnSettings();
            regionCodeCell.EditingMouseDown += (s, e) =>
            {
                if (e.RowIndex > -1)
                {
                    var dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                    if (this.EditMode && dr.RowState == DataRowState.Added && e.Button == MouseButtons.Right)
                    {
                        using (var factoryPopup = new Sci.Win.Tools.SelectItem(this.dtFactory, "CountryID,Factory", "3,5", string.Empty))
                        {
                            var result = factoryPopup.ShowDialog();
                            if (result == DialogResult.OK)
                            {
                                dr["Country"] = factoryPopup.GetSelecteds().FirstOrDefault().Field<string>("CountryID");
                                dr["RegionCode"] = factoryPopup.GetSelecteds().FirstOrDefault().Field<string>("Factory");
                                this.grid1.ValidateControl();
                            }
                        }
                    }
                }
            };
            regionCodeCell.CellValidating += (s, e) =>
            {
                var dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                var regionCode = e.FormattedValue.ToString();
                if (dr.RowState == DataRowState.Added && !dr["RegionCode"].EqualString(regionCode))
                {
                    if (regionCode.IsEmpty())
                    {
                        return;
                    }

                    var factoryItem = this.dtFactory.AsEnumerable().Where(r => r["Factory"].EqualString(regionCode));
                    if (!factoryItem.Any())
                    {
                        MyUtility.Msg.ErrorBox($"Factory: <{regionCode}> not found!!!");
                        e.Cancel = true;
                        return;
                    }

                    if (this.CheckDuplicate(e.FormattedValue.ToString(), dr["ToAlias"].ToString()))
                    {
                        MyUtility.Msg.ErrorBox($"Data is Duplicated!!!");
                        e.Cancel = true;
                        return;
                    }

                    dr["RegionCode"] = e.FormattedValue.ToString();
                    dr["Country"] = factoryItem.FirstOrDefault().Field<string>("Country");
                    this.lcbs.ResetBindings(false);
                }
            };

            DataGridViewGeneratorTextColumnSettings toAliasCell = new DataGridViewGeneratorTextColumnSettings();
            toAliasCell.EditingMouseDown += (s, e) =>
            {
                if (e.RowIndex > -1)
                {
                    var dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                    if (this.EditMode && dr.RowState == DataRowState.Added && e.Button == MouseButtons.Right)
                    {
                        using (var toAliasPopup = new Sci.Win.Tools.SelectItem(this.dtCountry, "ID,Alias", "5,10", string.Empty))
                        {
                            var result = toAliasPopup.ShowDialog();
                            if (result == DialogResult.OK)
                            {
                                dr["ToID"] = toAliasPopup.GetSelecteds().FirstOrDefault().Field<string>("ID");
                                dr["ToAlias"] = toAliasPopup.GetSelecteds().FirstOrDefault().Field<string>("Alias");
                                this.grid1.ValidateControl();
                            }
                        }
                    }
                }
            };
            toAliasCell.CellValidating += (s, e) =>
            {
                var dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                var country = e.FormattedValue.ToString();
                var id = string.Empty;
                if (dr.RowState == DataRowState.Added && !dr["ToAlias"].EqualString(country))
                {
                    if (country.IsEmpty())
                    {
                        return;
                    }

                    var countryItem = this.dtCountry.AsEnumerable().Where(r => r["ID"].EqualString(country));
                    if (countryItem.Any())
                    {
                        country = countryItem.FirstOrDefault().Field<string>("Alias");
                    }

                    countryItem = this.dtCountry.AsEnumerable().Where(r => r["Alias"].EqualString(country));
                    if (!countryItem.Any())
                    {
                        MyUtility.Msg.ErrorBox($"Factory: <{country}> not found!!!");
                        e.Cancel = true;
                        return;
                    }
                    else
                    {
                        id = countryItem.FirstOrDefault().Field<string>("ID");
                    }

                    if (this.CheckDuplicate(dr["RegionCode"].ToString(), country))
                    {
                        MyUtility.Msg.ErrorBox($"Data is Duplicated!!!");
                        e.Cancel = true;
                        return;
                    }

                    dr["ToID"] = id;
                    dr["ToAlias"] = country;
                    this.lcbs.ResetBindings(false);
                }
            };

            this.Helper.Controls.Grid.Generator(this.grid1)
                .Numeric("Seq", header: "Seq", width: Widths.AnsiChars(2), iseditingreadonly: true)
                .Text("Country", header: "Country", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Text("RegionCode", header: "Region Code", width: Widths.AnsiChars(5), iseditingreadonly: false, settings: regionCodeCell).Get(out this.col_Factory)
                .Text("ToAlias", header: "To", width: Widths.AnsiChars(10), iseditingreadonly: false, settings: toAliasCell).Get(out this.col_ToAlias)
                .CheckBox("SUN", header: "SUN", width: Widths.AnsiChars(3), trueValue: true, falseValue: false).Get(out this.col_SUN)
                .CheckBox("MON", header: "MON", width: Widths.AnsiChars(3), trueValue: true, falseValue: false).Get(out this.col_MON)
                .CheckBox("TUE", header: "TUE", width: Widths.AnsiChars(3), trueValue: true, falseValue: false).Get(out this.col_TUE)
                .CheckBox("WED", header: "WED", width: Widths.AnsiChars(3), trueValue: true, falseValue: false).Get(out this.col_WED)
                .CheckBox("THU", header: "THU", width: Widths.AnsiChars(3), trueValue: true, falseValue: false).Get(out this.col_THU)
                .CheckBox("FRI", header: "FRI", width: Widths.AnsiChars(3), trueValue: true, falseValue: false).Get(out this.col_FRI)
                .CheckBox("SAT", header: "SAT", width: Widths.AnsiChars(3), trueValue: true, falseValue: false).Get(out this.col_SAT)
                .Date("BeginDate", header: "Begin Date", width: Widths.AnsiChars(10), iseditingreadonly: false).Get(out this.col_BeginDate)
                .CheckBox("Junk", header: "Junk", width: Widths.AnsiChars(3), trueValue: true, falseValue: false).Get(out this.col_Junk)
                .Button("History", header: "H", width: Widths.AnsiChars(8), onclick: this.BtnHistory_Click)
                .Text("CreateBy", header: "Create By", width: Widths.AnsiChars(35), iseditingreadonly: true)
                .DateTime("AddDate", header: "Create Date", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("EditBy", header: "Edit By", width: Widths.AnsiChars(35), iseditingreadonly: true)
                .DateTime("EditDate", header: "Edit Date", width: Widths.AnsiChars(15), iseditingreadonly: true);

            this.grid1.EditingControlShowing += (s, e) =>
            {
                if (this.grid1.GetSelectedRowIndex() == -1)
                {
                    return;
                }

                DataGridViewColumn dgColumn = this.grid1.Columns[this.grid1.CurrentCellAddress.X];
                DataRow dr = this.grid1.GetDataRow(this.grid1.GetSelectedRowIndex());
                if ((dgColumn == this.col_Factory || dgColumn == this.col_ToAlias) && dr.RowState != DataRowState.Added)
                {
                    ((Ict.Win.UI.TextBox)e.Control).ReadOnly = true;
                }
            };
        }

        private void BtnUndoClose_Click(object sender, EventArgs e)
        {
            if (this.EditMode)
            {
                if (MyUtility.Msg.QuestionBox("Whether to cancel the editing?") == DialogResult.Yes)
                {
                    this.factoryExpressSendingScheduleAdapter.workTable.RejectChanges();
                    this.lcbs.ResetBindings(false);
                    this.EditMode = false;
                }
            }
            else
            {
                this.Close();
            }
        }

        private void RefreshData()
        {
            this.factoryExpressSendingScheduleAdapter = new MyAdapter("select * from FactoryExpress_SendingSchedule order by Seq");
            this.factoryExpressSendingScheduleAdapter.autoDetectFeildInfo = false;
            this.factoryExpressSendingScheduleAdapter.workTable.ColumnsStringAdd("CreateBy");
            this.factoryExpressSendingScheduleAdapter.workTable.ColumnsStringAdd("EditBy");
            this.factoryExpressSendingScheduleAdapter.workTable.AsEnumerable().ToList().ForEach(r =>
            {
                r["CreateBy"] = Sci.Production.Class.Commons.UserPrg.GetName(r["AddName"], Sci.Production.Class.Commons.UserPrg.NameType.idAndNameAndExt);
                r["EditBy"] = Sci.Production.Class.Commons.UserPrg.GetName(r["EditName"], Sci.Production.Class.Commons.UserPrg.NameType.idAndNameAndExt);
            });
            this.factoryExpressSendingScheduleAdapter.workTable.BeginLoadData();
            this.factoryExpressSendingScheduleAdapter.workTable.AcceptChanges();
            this.lcbs.DataSource = this.factoryExpressSendingScheduleAdapter.workTable;
        }

        private void SetFactoryData()
        {
            this.dtFactory = mySqlClient.Select(null, "select distinct CountryID,Factory = NegoRegion from Factory where NegoRegion <> ''");
        }

        private void SetCountryData()
        {
            this.dtCountry = mySqlClient.Select(null, "select ID,Alias from Country where Junk = 0");
        }

        private void AddAction()
        {
            var seqList = this.factoryExpressSendingScheduleAdapter.workTable.AsEnumerable().Select(r => r["Seq"].ToInt());
            var seq = seqList.Any() ? seqList.Max() : 0;
            var dr = this.factoryExpressSendingScheduleAdapter.Insert().AssignDefaultValue();
            dr["Seq"] = seq + 1;
            this.lcbs.ResetBindings(false);
            this.lcbs.Position = seq;
            this.grid1.BeginRowEdit(seq);
        }

        private bool IsSendingScheduleChange(DataRow r)
        {
            return r["SUN", DataRowVersion.Original].ToBool() != r["SUN"].ToBool() ||
                r["MON", DataRowVersion.Original].ToBool() != r["MON"].ToBool() ||
                r["TUE", DataRowVersion.Original].ToBool() != r["TUE"].ToBool() ||
                r["WED", DataRowVersion.Original].ToBool() != r["WED"].ToBool() ||
                r["THU", DataRowVersion.Original].ToBool() != r["THU"].ToBool() ||
                r["FRI", DataRowVersion.Original].ToBool() != r["FRI"].ToBool() ||
                r["SAT", DataRowVersion.Original].ToBool() != r["SAT"].ToBool() ||
                r["BeginDate", DataRowVersion.Original].ToDateTime() != r["BeginDate"].ToDateTime();
        }

        private void BtnHistory_Click(object sender, EventArgs e)
        {
            var dr = this.grid1.GetDataRow<DataRow>(this.lcbs.Position);
            using (var history = new P02_FTYRegularSendingScheduleHistory(dr["RegionCode"].ToString(), dr["ToAlias"].ToString()))
            {
                history.ShowDialog();
            }
        }

        private void Grid1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (!(this.grid1.Rows[0].DataBoundItem is DataRowView))
            {
                return;
            }

            for (int idx = e.RowIndex; idx < e.RowIndex + e.RowCount; idx++)
            {
                var item = new
                {
                    GridRow = ((DataGridView)sender).Rows[idx],
                    DataRow = ((DataRowView)((DataGridView)sender).Rows[idx].DataBoundItem).Row
                };
                DataGridViewRow gridRow = item.GridRow;
                DataRow currentRow = item.DataRow;
                Color def_ForeColor = gridRow.DefaultCellStyle.ForeColor;
                Color def_BackColor = gridRow.DefaultCellStyle.BackColor;

                var hasHistory = MyUtility.Check.Seek($"select 1 from FactoryExpress_SendingScheduleHistory where RegionCode = '{currentRow["RegionCode"].ToString()}' and ToAlias = '{currentRow["ToAlias"].ToString()}'");
                gridRow.Cells[13].Style.ForeColor = hasHistory ? Color.Blue : def_ForeColor;
                gridRow.DefaultCellStyle.BackColor = currentRow["Junk"].ToBool() ? Color.FromArgb(224, 224, 224) : def_BackColor;
            }
        }

        private void SetGridColumn_EditableStyle(DataGridViewColumn dataGridViewColumn, bool isDefault)
        {
            dataGridViewColumn.HeaderCell.Style.BackColor = isDefault ? Color.WhiteSmoke : Color.FromArgb(255, 255, 128);
            dataGridViewColumn.DefaultCellStyle.BackColor = isDefault ? Color.White : Color.FromArgb(183, 227, 255);
            dataGridViewColumn.DefaultCellStyle.ForeColor = isDefault ? Color.FromArgb(0, 0, 0) : Color.FromArgb(255, 0, 0);
            dataGridViewColumn.DefaultCellStyle.SelectionBackColor = Color.FromArgb(183, 227, 255);
            dataGridViewColumn.DefaultCellStyle.SelectionForeColor = isDefault ? Color.FromArgb(0, 0, 0) : Color.FromArgb(255, 0, 0);
        }

        private bool CheckDuplicate(string regionCode, string country)
        {
            return this.factoryExpressSendingScheduleAdapter.workTable.AsEnumerable().Where(r => r["RegionCode"].EqualString(regionCode) && r["ToAlias"].EqualString(country)).Any();
        }
    }
}
