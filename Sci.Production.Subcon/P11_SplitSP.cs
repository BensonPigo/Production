using Ict;
using Ict.Win;
using Ict.Win.UI;
using Sci.Data;
using Sci.Production.Prg;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Subcon
{
    /// <inheritdoc/>
    public partial class P11_SplitSP : Win.Subs.Base
    {
        private DataTable dt_detail;
        private DataTable dtDetail;

        /// <inheritdoc/>
        public P11_SplitSP(DataTable detail)
        {
            this.InitializeComponent();
            this.dt_detail = detail;
            this.EditMode = true;
        }

        // Button Query
        private void BtnQuery_Click(object sender, EventArgs e) 
        {
            StringBuilder strSQLCmd = new StringBuilder();
            string sp = this.txtSPNo.Text.TrimEnd();
            string combotype = this.txtComboType.Text.TrimEnd();
            string article = this.txtArticle.Text.TrimEnd();

            // SP#不可為空
            if (MyUtility.Check.Empty(sp) &&
                MyUtility.Check.Empty(combotype) &&
                MyUtility.Check.Empty(article))
            {
                MyUtility.Msg.WarningBox("<SP#>, <Combo Type>, <Article> cannot all be empty.");
                return;
            }

            strSQLCmd.Append(
$@"
select [Selected] = 0
,oc.ToOrderID
,[ComboType] = '{this.txtComboType.Text}'
,oq.Article
,[OrderQty] = sum(oq.Qty)
,[OutputQty] = 0
from OrderChangeApplication oc
inner join Order_Qty oq on oq.ID = oc.ToOrderID
where oc.ToOrderID <> '' 
and oc.ToOrderID is not null
");
            if (!MyUtility.Check.Empty(sp))
            {
                strSQLCmd.Append($@" and oc.OrderID = '{sp}'");
            }

            if (!MyUtility.Check.Empty(article))
            {
                strSQLCmd.Append($@" and oq.Article = '{article}'");
            }

            strSQLCmd.Append(Environment.NewLine + @"group by oc.ToOrderID,oq.Article");

            this.ShowWaitMessage("Data Loading....");
            DualResult result;
            if (!(result = DBProxy.Current.Select(null, strSQLCmd.ToString(), out this.dtDetail)))
            {
                this.ShowErr(strSQLCmd.ToString(), result);
            }
            else
            {
                if (this.dtDetail.Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!!");
                }

                this.listControlBindingSource1.DataSource = this.dtDetail;
            }

            this.HideWaitMessage();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.txtSPNo.Focus();

            #region
            DataGridViewGeneratorNumericColumnSettings col_OutputQty = new DataGridViewGeneratorNumericColumnSettings();
            col_OutputQty.CellValidating += (s, e) =>
            {
                DataRow dr = this.gridImport.GetDataRow(e.RowIndex);
                if (!this.EditMode)
                {
                    return; // 非編輯模式
                }

                if (e.RowIndex == -1)
                {
                    return; // 沒東西 return
                }

                if (e.FormattedValue != null)
                {
                    dr["OutputQty"] = e.FormattedValue;
                    dr.EndEdit();
                    DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
                    if (dt.Rows.Count > 0)
                    {
                        this.numttlSubconOutQty.Value = dt.AsEnumerable().Sum(row => row.Field<int>("OutputQty"));
                    }
                }
            };
            #endregion

            this.gridImport.IsEditingReadOnly = false; // 必設定, 否則CheckBox會顯示圖示
            this.gridImport.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridImport)
                .CheckBox("Selected", header: string.Empty, width: Ict.Win.Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
                .Text("ToOrderID", header: "To SP#", width: Ict.Win.Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("ComboType", header: "Combo Type", width: Ict.Win.Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("Article", header: "Article", width: Ict.Win.Widths.AnsiChars(8), iseditingreadonly: true)
                .Numeric("OrderQty", header: "Order Qty", width: Ict.Win.Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("OutputQty", header: "Subcon Out Qty", width: Ict.Win.Widths.AnsiChars(10), iseditingreadonly: false, settings: col_OutputQty)
            ;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            this.listControlBindingSource1.EndEdit();
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

            foreach (DataRow tmp in dr2)
            {
                if (tmp.RowState != DataRowState.Deleted)
                {
                    DataRow[] findrow = this.dt_detail.AsEnumerable().Where(
                    row => row.RowState != DataRowState.Deleted &&
                    row["OrderID"].EqualString(tmp["ToOrderID"].ToString()) &&
                    row["ComboType"].EqualString(tmp["ComboType"].ToString()) &&
                    row["Article"].EqualString(tmp["Article"].ToString())
                    ).ToArray();

                    if (findrow.Length > 0)
                    {
                        findrow[0]["OrderQty"] = tmp["OrderQty"];
                        findrow[0]["OutputQty"] = tmp["OutputQty"];
                        findrow[0]["UnitPrice"] = tmp["UnitPrice"];
                        findrow[0]["LocalCurrencyID"] = tmp["LocalCurrencyID"];
                        findrow[0]["LocalUnitPrice"] = tmp["LocalUnitPrice"];
                        findrow[0]["Vat"] = tmp["Vat"];
                        findrow[0]["UPIncludeVAT"] = tmp["UPIncludeVAT"];
                        findrow[0]["KpiRate"] = tmp["KpiRate"];
                    }
                    else
                    {
                        tmp.AcceptChanges();
                        tmp.SetAdded();
                        this.dt_detail.ImportRow(tmp);
                    }
                }
            }

            this.Close();
        }

        private void SumTTLSubconOutQty()
        {
            DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
            if (dt.Rows.Count > 0)
            {
                this.numttlSubconOutQty.Value = dt.AsEnumerable().Sum(row => row.Field<int>("OutputQty"));
            }
        }

        private void FillSubConOutQty()
        {
            if (this.dt_detail.Rows.Count > 0)
            {
                DataRow[] spMactch = this.dt_detail.Select($" OrderId = '{this.txtSPNo.Text}' and ComboType = '{this.txtComboType.Text}' and Article = '{this.txtArticle.Text}'");
                if (spMactch.Length > 0)
                {
                    this.numSubconOutQty.Value = MyUtility.Convert.GetDecimal(spMactch[0]["OutputQty"]);
                }
                else
                {
                    this.numSubconOutQty.Value = 0;
                }
            }
        }

        private void txtSPNo_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string spno = this.txtSPNo.Text;
            if (this.dt_detail.Rows.Count > 0)
            {
                DataRow[] spMactch = this.dt_detail.Select($" OrderId = '{spno}'");
                if (spMactch.Length > 0)
                {
                    var distnct_List = spMactch.AsEnumerable().
                    Select(m => new
                    {
                        ComboType = m.Field<string>("ComboType"),
                    }).Distinct().ToList();
                    if (distnct_List.Count == 1)
                    {
                        this.txtComboType.Text = spMactch[0]["ComboType"].ToString();
                    }

                    var distnct_List2 = spMactch.AsEnumerable().
                    Select(m => new
                    {
                        Article = m.Field<string>("Article"),
                    }).Distinct().ToList();
                    if (distnct_List2.Count == 1)
                    {
                        this.txtArticle.Text = spMactch[0]["Article"].ToString();
                    }
                }
                else
                {
                    MyUtility.Msg.WarningBox("SP# not found");
                    return;
                }

                this.FillSubConOutQty();
            }
        }

        private void txtComboType_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            if (this.dt_detail.Rows.Count > 0)
            {
                DataTable distnct_List2 = this.dt_detail.AsEnumerable().
                  Select(m => new
                  {
                      ComboType = m.Field<string>("ComboType"),
                  }).Distinct().ToList().ToDataTable();
                SelectItem item = new SelectItem(distnct_List2, "ComboType", "15", this.txtComboType.Text);
                DialogResult returnResult = item.ShowDialog();
                if (returnResult == DialogResult.Cancel)
                {
                    return;
                }

                this.txtComboType.Text = item.GetSelectedString();
                this.FillSubConOutQty();
            }
        }

        private void txtComboType_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string cbtye = this.txtComboType.Text;
            if (this.dt_detail.Rows.Count > 0)
            {
                DataRow[] drChk = this.dt_detail.Select($" ComboType = '{this.txtComboType.Text}'");
                if (drChk.Length <= 0)
                {
                    MyUtility.Msg.WarningBox("ComboType not found");
                    e.Cancel = true;
                    return;
                }
            }
            else
            {
                MyUtility.Msg.WarningBox("Data not found");
                return;
            }

            this.FillSubConOutQty();
        }

        private void txtArticle_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string cbtye = this.txtComboType.Text;
            if (this.dt_detail.Rows.Count > 0)
            {
                DataRow[] drChk = this.dt_detail.Select($" Article = '{this.txtArticle.Text}'");
                if (drChk.Length <= 0)
                {
                    MyUtility.Msg.WarningBox("Article not found");
                    e.Cancel = true;
                    return;
                }
            }
            else
            {
                MyUtility.Msg.WarningBox("Data not found");
                return;
            }

            this.FillSubConOutQty();
        }

        private void txtArticle_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            if (this.dt_detail.Rows.Count > 0)
            {
                DataTable distnct_List2 = this.dt_detail.AsEnumerable().
                  Select(m => new
                  {
                      Article = m.Field<string>("Article"),
                  }).Distinct().ToList().ToDataTable();
                SelectItem item = new SelectItem(distnct_List2, "Article", "15", this.txtArticle.Text);
                DialogResult returnResult = item.ShowDialog();
                if (returnResult == DialogResult.Cancel)
                {
                    return;
                }

                this.txtArticle.Text = item.GetSelectedString();
                this.FillSubConOutQty();
            }
        }

        private void btnSplit_Click(object sender, EventArgs e)
        {
            this.listControlBindingSource1.EndEdit();
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

            if (this.numttlSubconOutQty.Value > this.numSubconOutQty.Value)
            {
                MyUtility.Msg.WarningBox("<Total Subcon Out Qty> cannot more than <Subcon Out Qty>");
                return;
            }

            foreach (DataRow tmp in dr2)
            {
                if (tmp.RowState != DataRowState.Deleted)
                {
                    DataRow[] findrow = this.dt_detail.AsEnumerable().Where(
                    row => row.RowState != DataRowState.Deleted &&
                    row["OrderID"].EqualString(tmp["ToOrderID"].ToString()) &&
                    row["ComboType"].EqualString(tmp["ComboType"].ToString()) &&
                    row["Article"].EqualString(tmp["Article"].ToString())
                    ).ToArray();

                    if (findrow.Length > 0)
                    {
                        findrow[0]["OrderQty"] = tmp["OrderQty"];
                        findrow[0]["OutputQty"] = tmp["OutputQty"];
                    }
                    else
                    {
                        tmp.AcceptChanges();
                        tmp.SetAdded();
                        this.dt_detail.ImportRow(tmp);
                    }
                }
            }

            this.Close();
        }
    }
}
