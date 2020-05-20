using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;

namespace Sci.Production.PPIC
{
    public partial class P21_ResponsibilityDept : Sci.Win.Subs.Input4
    {
        private string ID;
        private string FormType;
        private bool canConfirm;
        private bool canUnConfirm;
        private string checkTable;

        public P21_ResponsibilityDept(bool canedit, string id, string keyvalue2, string keyvalue3, string formType, bool canConfirm, bool canUnConfirm)
            : base(canedit, id, keyvalue2, keyvalue3)
        {
            this.InitializeComponent();
            this.ID = id;
            this.EditMode = canedit;
            this.FormType = formType;

            this.canConfirm = canConfirm;
            this.canUnConfirm = canUnConfirm;
            this.checkTable = formType == "Replacement" ? "ReplacementReport" : "ICR";
            this.ConfirmStatusCheck();
        }

        protected override bool OnGridSetup()
        {
            DataGridViewGeneratorTextColumnSettings col_Factory = new DataGridViewGeneratorTextColumnSettings();
            #region col_Factory
            col_Factory.CellMouseClick += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                if (this.EditMode == false) return;
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    DataRow dr = this.grid.GetDataRow(e.RowIndex);
                    string sqlcmd = $@"Select distinct FtyGroup from Factory where junk = 0 and Type in ('B','S')";
                    SelectItem item = new SelectItem(sqlcmd, "20", dr["FactoryID"].ToString());
                    DialogResult dresult = item.ShowDialog();
                    if (dresult == DialogResult.Cancel)
                    {
                        return;
                    }

                    dr["FactoryID"] = item.GetSelectedString();
                    dr.EndEdit();
                }
            };

            col_Factory.EditingMouseDown += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                if (this.EditMode == false) return;
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    DataRow dr = this.grid.GetDataRow(e.RowIndex);
                    string sqlcmd = $@"Select distinct FtyGroup from Factory where junk = 0 and Type in ('B','S')";
                    SelectItem item = new SelectItem(sqlcmd, "20", dr["FactoryID"].ToString());
                    DialogResult dresult = item.ShowDialog();
                    if (dresult == DialogResult.Cancel)
                    {
                        return;
                    }

                    dr["FactoryID"] = item.GetSelectedString();
                    dr.EndEdit();
                }
            };

            col_Factory.CellValidating += (s, e) =>
            {
                DataRow dr = this.grid.GetDataRow(e.RowIndex);
                string oldvalue = dr["FactoryID"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (!this.EditMode) return;//非編輯模式 
                if (e.RowIndex == -1) return; //沒東西 return
                if (oldvalue.Equals(newvalue)) return;
                if (MyUtility.Check.Empty(e.FormattedValue)) return;
                string sqlcmd = $@"Select distinct FtyGroup from Factory where junk = 0 and Type in ('B','S') and FtyGroup ='{newvalue}'";
                if (!MyUtility.Check.Seek(sqlcmd))
                {
                    dr["FactoryID"] = string.Empty;
                    dr.EndEdit();
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox("Data not found!!");
                    return;
                }

                dr.EndEdit();
            };
            #endregion

            DataGridViewGeneratorTextColumnSettings col_Dept = new DataGridViewGeneratorTextColumnSettings();
            #region col_Dept
            col_Dept.CellMouseClick += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                if (this.EditMode == false) return;
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    DataRow dr = this.grid.GetDataRow(e.RowIndex);
                    string sqlcmd = $@"select ID,Name from [FinanceEN].dbo.Department where Junk = 0";
                    SelectItem item = new SelectItem(sqlcmd, "10,30", dr["DepartmentID"].ToString());
                    DialogResult dresult = item.ShowDialog();
                    if (dresult == DialogResult.Cancel)
                    {
                        return;
                    }

                    dr["DepartmentID"] = item.GetSelectedString();
                    dr.EndEdit();
                }
            };

            col_Dept.EditingMouseDown += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                if (this.EditMode == false) return;
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    DataRow dr = this.grid.GetDataRow(e.RowIndex);
                    string sqlcmd = $@"select ID,Name from [FinanceEN].dbo.Department where Junk = 0";
                    SelectItem item = new SelectItem(sqlcmd, "10,30", dr["DepartmentID"].ToString());
                    DialogResult dresult = item.ShowDialog();
                    if (dresult == DialogResult.Cancel)
                    {
                        return;
                    }

                    dr["DepartmentID"] = item.GetSelectedString();
                    dr.EndEdit();
                }
            };

            col_Dept.CellValidating += (s, e) =>
            {
                DataRow dr = this.grid.GetDataRow(e.RowIndex);
                string oldvalue = dr["DepartmentID"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (!this.EditMode) return;//非編輯模式 
                if (e.RowIndex == -1) return; //沒東西 return
                if (oldvalue.Equals(newvalue)) return;
                if (MyUtility.Check.Empty(e.FormattedValue)) return;
                string sqlcmd = $@"select ID,Name from [FinanceEN].dbo.Department where Junk = 0 and id ='{newvalue}'";
                if (!MyUtility.Check.Seek(sqlcmd))
                {
                    dr["DepartmentID"] = string.Empty;
                    dr.EndEdit();
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox("Data not found!!");
                    return;
                }

                dr.EndEdit();
            };
            #endregion

            DataGridViewGeneratorNumericColumnSettings col_Percentage = new DataGridViewGeneratorNumericColumnSettings();
            col_Percentage.CellValidating += (s, e) => 
            {
                DataRow dr = this.grid.GetDataRow(e.RowIndex);
                decimal oldvalue = MyUtility.Convert.GetDecimal(dr["Percentage"]);
                decimal newvalue = MyUtility.Convert.GetDecimal(e.FormattedValue);
                if (!this.EditMode) return;//非編輯模式 
                if (e.RowIndex == -1) return; //沒東西 return
                if (oldvalue.Equals(newvalue)) return;
                //if (MyUtility.Check.Empty(e.FormattedValue)) return;
                decimal? amt = Math.Round(
                    (decimal)this.numTotalAmt.Value
                    * (newvalue
                    / 100), 2);
                dr["Amount"] = amt;
                dr["Percentage"] = newvalue;
                dr.EndEdit();

                // 加總表身Amount,Percentage資料
                decimal curAmount = 0;
                decimal curPercentage = 0;
                DataTable dt = (DataTable)this.gridbs.DataSource;
                foreach (DataRow drRow in dt.Rows)
                {
                    if (drRow.RowState != DataRowState.Deleted)
                    {
                        curAmount += MyUtility.Convert.GetDecimal(drRow["Amount"]);
                        curPercentage += MyUtility.Convert.GetDecimal(drRow["Percentage"]);
                    }
                }

                // 分配Amount 尾差
                if (curPercentage == 100 && curAmount != this.numTotalAmt.Value)
                {
                    dr["Amount"] = MyUtility.Convert.GetDecimal(dr["Amount"]) + (this.numTotalAmt.Value - curAmount);
                    curAmount = MyUtility.Convert.GetDecimal(this.numTotalAmt.Value);
                    dr.EndEdit();
                }

                this.numAmount.Value = curAmount;
                this.numPercentage.Value = curPercentage;
            };

            DataGridViewGeneratorNumericColumnSettings col_Amt = new DataGridViewGeneratorNumericColumnSettings();
            col_Amt.CellValidating += (s, e) =>
            {
                DataRow dr = this.grid.GetDataRow(e.RowIndex);
                double oldvalue = MyUtility.Convert.GetDouble(dr["Amount"]);
                double newvalue = MyUtility.Convert.GetDouble(e.FormattedValue);
                double useValue = 0;
                if (Math.Floor(oldvalue * 100) / 100 == newvalue)
                {
                    useValue = oldvalue;
                }
                else
                {
                    useValue = newvalue;
                }

                if (!this.EditMode) return;//非編輯模式 
                if (e.RowIndex == -1) return; //沒東西 return
                if (oldvalue.Equals(newvalue)) return;
                //if (MyUtility.Check.Empty(e.FormattedValue)) return;
                decimal? perct = 0;
                if (!MyUtility.Check.Empty(this.numTotalAmt.Value))
                {
                    perct = Math.Round(
                        (decimal)useValue / (decimal)this.numTotalAmt.Value * 100,
                        0);
                }

                dr["Percentage"] = perct;
                dr["Amount"] = useValue;
                dr.EndEdit();

                // 加總表身Amount,Percentage資料
                decimal curAmount = 0;
                decimal curPercentage = 0;
                DataTable dt = (DataTable)this.gridbs.DataSource;
                foreach (DataRow drRow in dt.Rows)
                {
                    if (drRow.RowState != DataRowState.Deleted)
                    {
                        curAmount += MyUtility.Convert.GetDecimal(drRow["Amount"]);
                        curPercentage += MyUtility.Convert.GetDecimal(drRow["Percentage"]);
                    }
                }

                // 分配%尾差
                if (curAmount == this.numTotalAmt.Value && curPercentage != 100)
                {
                    dr["Percentage"] = MyUtility.Convert.GetDecimal(dr["Percentage"]) + (100 - curPercentage);
                    curPercentage = 100;
                    dr.EndEdit();
                }

                this.numAmount.Value = curAmount;
                this.numPercentage.Value = curPercentage;
            };

            this.Helper.Controls.Grid.Generator(this.grid)
                .Text("FactoryID", "Factory", width: Widths.AnsiChars(10), iseditingreadonly: false, settings: col_Factory)
                .Text("DepartmentID", "Dept.", width: Widths.AnsiChars(21), iseditingreadonly: false, settings: col_Dept)
                .Numeric("Percentage", header: "%", width: Widths.AnsiChars(10), iseditingreadonly: false, decimal_places: 0, integer_places: 10, settings: col_Percentage)
                .Numeric("Amount", header: "Amt", width: Widths.AnsiChars(13), iseditingreadonly: false, decimal_places: 2, integer_places: 10, settings: col_Amt)
                ;
            return true;

        }

        /// <summary>
        /// OnEditModeChanged
        /// </summary>
        protected override void OnEditModeChanged()
        {
            base.OnEditModeChanged();
            this.ConfirmStatusCheck();
        }

        private void ConfirmStatusCheck()
        {
            if (this.btnConfirm == null)
            {
                return;
            }

            if (this.EditMode)
            {
                this.btnConfirm.Enabled = false;
                this.btnUnConfirm.Enabled = false;
                return;
            }

            if (this.IsRespDeptConfirmDateNull())
            {
                this.btnConfirm.Enabled = this.canConfirm;
                this.btnUnConfirm.Enabled = false;
            }
            else
            {
                this.btnConfirm.Enabled = false;
                this.btnUnConfirm.Enabled = this.canUnConfirm && this.IsVoucherNull();
            }
        }

        private bool IsRespDeptConfirmDateNull()
        {
            return MyUtility.Check.Seek($"select 1 from {this.checkTable} with (nolock) where ID = '{this.ID}' and RespDeptConfirmDate is null and RespDeptConfirmName = '' ");
        }

        private bool IsVoucherNull()
        {
            return MyUtility.Check.Seek($"select 1 from {this.checkTable} with (nolock) where ID = '{this.ID}' and VoucherDate is null and VoucherID = ''");
        }

        protected override void OnDelete()
        {
            base.OnDelete();
            // 加總表身Amount,Percentage資料
            decimal curAmount = 0;
            decimal curPercentage = 0;
            DataTable dt = (DataTable)this.gridbs.DataSource;
            foreach (DataRow drRow in dt.Rows)
            {
                if (drRow.RowState != DataRowState.Deleted)
                {
                    curAmount += MyUtility.Convert.GetDecimal(drRow["Amount"]);
                    curPercentage += MyUtility.Convert.GetDecimal(drRow["Percentage"]);
                }
            }

            this.numAmount.Value = curAmount;
            this.numPercentage.Value = curPercentage;
        }

        protected override DualResult OnRequery()
        {
            DataRow drMaster;
            DualResult result;
            string sqlcmd = string.Empty;
            if (FormType == "Replacement")
            {
                sqlcmd = $@"
select r.ID
, [TotalAmt] = ISNULL(RMtlAmt,0) + ISNULL(r.ActFreight,0) + ISNULL(r.EstFreight,0) + ISNULL(r.SurchargeAmt,0)
, [Percentage] = isnull(i3.Percentage,0)
, [Amt] = isnull(i3.Amt,0)
from ReplacementReport r
outer apply(
	select [Percentage] = SUM(ISNULL(Percentage,0))
	, [Amt] = sum(ISNULL(Amount,0))
	from ICR_ResponsibilityDept 
	where id=r.id
)i3
where r.id = '{this.ID}'
 ";
            }
            else
            {
                sqlcmd = $@"
select ICR.ID
, [TotalAmt] = (ISNULL(RMtlAmtUSD,0) + ISNULL(ActFreightUSD,0) + ISNULL(OtherAmtUSD,0))
, [Percentage] = isnull(i3.Percentage,0)
, [Amt] = isnull(i3.Amt,0)
from ICR
outer apply(
	select [Percentage] = SUM(ISNULL(Percentage,0))
	, [Amt] = sum(ISNULL(Amount,0))
	from ICR_ResponsibilityDept 
	where id=ICR.id
)i3
where ICR.id = '{this.ID}'
 ";
            }

            if (MyUtility.Check.Seek(sqlcmd, out drMaster))
            {
                this.numTotalAmt.Value = MyUtility.Convert.GetDecimal(drMaster["TotalAmt"]);
                this.numPercentage.Value = MyUtility.Convert.GetDecimal(drMaster["Percentage"]);
                this.numAmount.Value = MyUtility.Convert.GetDecimal(drMaster["Amt"]);
            }
            else
            {
                this.numTotalAmt.Value = 0;
                this.numPercentage.Value = 0;
                this.numAmount.Value = 0;
            }

            this.txtID.Text = this.ID;
            return base.OnRequery();
        }

        protected override bool OnSaveBefore()
        {
            DataTable dt = (DataTable)this.gridbs.DataSource;

            decimal curAmount = 0;
            decimal curPercentage = 0;
            foreach (DataRow dr in dt.Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    curAmount += MyUtility.Convert.GetDecimal(dr["Amount"]);
                    curPercentage += MyUtility.Convert.GetDecimal(dr["Percentage"]);
                }
            }

            if (curPercentage != 0 && !curPercentage.Equals(100))
            {
                MyUtility.Msg.WarningBox("Total % not equal to 100.");
                return false;
            }

            if (curAmount != 0 && !curAmount.Equals(this.numTotalAmt.Value))
            {
                MyUtility.Msg.WarningBox("Total Amt not equal to Total Amt (USD).");
                return false;
            }

            return base.OnSaveBefore();
        }

        protected override DualResult OnSavePre()
        {
            foreach (DataRow dr in this.Datas)
            {
                dr["EditName"] = Env.User.UserID;
                dr["EditDate"] = DateTime.Now;
            }

            return base.OnSavePre();
        }

        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            decimal sumAmount = MyUtility.Convert.GetDecimal(((DataTable)this.gridbs.DataSource).Compute("sum(Amount)", string.Empty));
            if (sumAmount != MyUtility.Convert.GetDecimal(this.numTotalAmt.Value))
            {
                MyUtility.Msg.WarningBox("Total department shared amount must equal to Total US$.");
                return;
            }

            bool isICR_ResponsibilityDeptNoData = !MyUtility.Check.Seek($"select 1 from ICR_ResponsibilityDept with (nolock) where ID = '{this.ID}'");

            if (isICR_ResponsibilityDeptNoData)
            {
                return;
            }

            string sqlConfirm = $@"
            update {this.checkTable} set RespDeptConfirmDate = getdate(), RespDeptConfirmName  = '{Env.User.UserID}' where ID = '{this.ID}'
";

            DualResult result = DBProxy.Current.Execute(null, sqlConfirm);

            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.ConfirmStatusCheck();
            MyUtility.Msg.InfoBox("Confirm success!");
            this.save.Visible = false;
        }

        private void BtnUnConfirm_Click(object sender, EventArgs e)
        {
            if (!this.IsVoucherNull())
            {
                this.ConfirmStatusCheck();
                return;
            }

            string sqlConfirm = $@"
            update {this.checkTable} set RespDeptConfirmDate = null, RespDeptConfirmName  = '' where ID = '{this.ID}'
";

            DualResult result = DBProxy.Current.Execute(null, sqlConfirm);

            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.ConfirmStatusCheck();
            MyUtility.Msg.InfoBox("UnConfirm success!");
            this.save.Visible = true;
        }
    }
}
