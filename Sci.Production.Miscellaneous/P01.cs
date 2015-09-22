using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Data;
using Ict.Win;
using Sci;
using Sci.Data;
using Sci.Win;
using Sci.Win.Tools;
using Sci.Production.Class;
using System.IO;
using System.Transactions;
using System.Linq;

namespace Sci.Production.Miscellaneous
{
    public partial class P01 : Sci.Win.Tems.Input6
    {
        private string factory = Sci.Env.User.Factory;
        private string loginID = Sci.Env.User.UserID;
        private string keyWord = Sci.Env.User.Keyword;
        public P01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            string defaultfilter = string.Format("factoryid = '{0}'", factory);
            this.DefaultFilter = defaultfilter;

            Dictionary<String, String> comboBox1_RowSource1 = new Dictionary<string, string>();
            comboBox1_RowSource1.Add("T", "Taipei");
            comboBox1_RowSource1.Add("L", "Local");
            comboBox1.ValueMember = "Key";
            comboBox1.DisplayMember = "Value";
            comboBox1.DataSource = new BindingSource(comboBox1_RowSource1, null);

            Dictionary<String, String> comboBox2_RowSource1 = new Dictionary<string, string>();
            comboBox2_RowSource1.Add("Maintenance", "Maintenance");
            comboBox2_RowSource1.Add("General Affair", "General Affair");
            comboBox2.ValueMember = "Key";
            comboBox2.DisplayMember = "Value";
            comboBox2.DataSource = new BindingSource(comboBox2_RowSource1, null);
           

        }
        protected override DualResult OnDetailSelectCommandPrepare(Win.Tems.InputMasterDetail.PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["ID"].ToString();
            if (e.Master == null || e.Master["PurchaseFrom"].ToString() == "T")
            { this.DetailSelectCommand = string.Format(
            @"Select a.*,b.description,b.currencyid,b.MiscBrandid+'-'+c.Name as Miscbrandname,b.unitid,e.abben as abb,a.qty*a.price as amount,d.PurchaseFrom from MiscReq_Detail a left join Misc b on a.miscid = b.id left join MiscBrand c on c.id = b.Miscbrandid left join Supp e on e.id = a.suppid left join MiscReq d on d.id = a.id where a.id ='{0}'", masterID); }
            else { this.DetailSelectCommand = string.Format(
                @"Select a.*,b.description,b.currencyid,b.MiscBrandid+'-'+c.Name as Miscbrandname,b.unitid,e.abb as abb,a.qty*a.price as amount, d.PurchaseFrom from MiscReq_Detail a left join Misc b on a.miscid = b.id left join MiscBrand c on c.id = b.Miscbrandid left join LocalSupp e on e.id = a.suppid left join MiscReq d on d.id = a.id  where a.id ='{0}'", masterID);
            }
            return base.OnDetailSelectCommandPrepare(e);
        }

        protected override void OnDetailUIConvertToMaintain()
        {
            base.OnDetailUIConvertToMaintain();
            txtuser2.TextBox1.ReadOnly = true;
        }
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            #region grid1

            this.grid1.DataSource = listControlBindingSource1;

            Helper.Controls.Grid.Generator(this.grid1)
            .Text("Currencyid", header: "Currency", width: Widths.AnsiChars(3), iseditingreadonly: true)
            .Numeric("Amount", header: "Amount", width: Widths.AnsiChars(10), integer_places: 9, decimal_places: 4, iseditingreadonly: true);
            #endregion

        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            label9.Text = CurrentMaintain["Status"].ToString();
            string mastergridSql;
            if (CurrentMaintain["PurchaseFrom"].ToString() == "T")
            {
                mastergridSql = string.Format("Select CurrencyId, isnull(Sum(Price * Qty),0) as Amount from MiscReq_Detail a Left join Supp b on b.id = a.suppid where a.id = '{0}' group by currencyid", CurrentMaintain["ID"].ToString());
            }
            else
            {
                mastergridSql = string.Format("Select CurrencyId, isnull(Sum(Price * Qty),0) as Amount from MiscReq_Detail a Left join LocalSupp b on b.id = a.suppid where a.id = '{0}' group by currencyid", CurrentMaintain["ID"].ToString());
            }
            DataTable masterDt;
            DualResult dtResult = DBProxy.Current.Select(null, mastergridSql, out masterDt);
            listControlBindingSource1.DataSource = masterDt;

        }
        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();

            DataGridViewGeneratorTextColumnSettings miscCell = cellmisc.GetGridCell(true);
            DataGridViewGeneratorNumericColumnSettings amountCell = new DataGridViewGeneratorNumericColumnSettings();
            #region MiscCell
            miscCell.CellValidating += (s, e) =>
                {
                    if (!this.EditMode) return;
                    DataGridView grid = ((DataGridViewColumn)s).DataGridView;
                    DataRow row = grid.GetDataRow<DataRow>(e.RowIndex);
                    string oldValue = row["miscid"].ToString();
                    string newValue = e.FormattedValue.ToString();
                    string sqlPart = "";
                    if (oldValue == e.FormattedValue.ToString()) return;
                    DataTable dtMisc;
                    if (CurrentMaintain["PurchaseFrom"].ToString() == "T")
                    {
                        sqlPart = String.Format(
                        @"Select a.*,a.miscbrandid+'-'+b.name as Miscbrandname,c.abben as abb,c.currencyid from Misc a 
                    left join miscbrand b on b.id = a.miscbrandid 
                    left join Supp c on c.id = a.suppid where a.id='{0}' and junk = 0 and purchaseFrom='{1}'", newValue, CurrentMaintain["PurchaseFrom"].ToString());
                    }
                    else
                    {
                        sqlPart = String.Format(
                       @"Select a.*,a.miscbrandid+'-'+b.name as Miscbrandname,c.abb as abb,c.currencyid from Misc a 
                    left join miscbrand b on b.id = a.miscbrandid 
                    left join LocalSupp c on c.id = a.suppid where a.id='{0}' and a.junk = 0 and a.purchaseFrom = '{1}'", newValue,CurrentMaintain["PurchaseFrom"].ToString());
                    }
                    DualResult dResult = DBProxy.Current.Select(null, sqlPart, out dtMisc);

                    if (dResult && dtMisc.Rows.Count != 0)
                    {
                        DataTable dtgrid = ((DataTable)this.detailgridbs.DataSource);

                        CurrentDetailData["Miscid"] = dtMisc.Rows[0]["ID"];
                        CurrentDetailData["Description"] = dtMisc.Rows[0]["Description"];
                        CurrentDetailData["miscBrandname"] = dtMisc.Rows[0]["Miscbrandname"].ToString();
                        CurrentDetailData["Unitid"] = dtMisc.Rows[0]["unitid"];
                        CurrentDetailData["Qty"] = 0;
                        CurrentDetailData["miscBrandid"] = dtMisc.Rows[0]["miscbrandid"];
                        CurrentDetailData["Suppid"] = dtMisc.Rows[0]["Suppid"];
                        CurrentDetailData["abb"] = dtMisc.Rows[0]["abb"];
                        CurrentDetailData["Price"] = dtMisc.Rows[0]["price"];
                        CurrentDetailData["Currencyid"] = dtMisc.Rows[0]["Currencyid"];
                        CurrentDetailData["Amount"] = (decimal)CurrentDetailData["Qty"] * (decimal)dtMisc.Rows[0]["price"];
                    }
                    else
                    {
                        CurrentDetailData["Miscid"] = "";
                        CurrentDetailData["Description"] ="";
                        CurrentDetailData["miscBrandname"] = "";
                        CurrentDetailData["Unitid"] = "";
                        CurrentDetailData["Qty"] = 0;
                        CurrentDetailData["miscBrandid"] = "";
                        CurrentDetailData["Suppid"] = "";
                        CurrentDetailData["abb"] = "";
                        CurrentDetailData["Price"] = 0;
                        CurrentDetailData["Currencyid"] = "";
                        CurrentDetailData["Amount"] = 0;
                    }

                    CurrentDetailData.EndEdit();
                };
            #endregion 
            #region Qty Amount
            amountCell.CellValidating += (s, e) =>
            {
                if (e.FormattedValue == null) return;
                CurrentDetailData["Amount"] = (decimal)e.FormattedValue * (decimal)CurrentDetailData["Price"];
                CurrentDetailData["Qty"] = (decimal)e.FormattedValue;
                CurrentDetailData.EndEdit();
            };
            #endregion
            #region setup Grid
            Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("MiscID", header: "Miscellaneous", width: Widths.AnsiChars(23), settings: miscCell)
                .Text("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("Suppid", header: "Supplier", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("abb", header: "Supplier Abbr.", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("MiscBrandName", header: "Brand", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("ProjectId", header: "Project Name", width: Widths.AnsiChars(8))
                 .Numeric("Qty", header: "Request Qty", integer_places: 6, decimal_places: 2, settings: amountCell)
                .Text("Unitid", header: "Unit", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("Currencyid", header: "Currency", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Numeric("Price", header: "Unit Price", integer_places: 8, decimal_places: 4, iseditingreadonly: true)
                .Numeric("Amount", header: "Amount", integer_places: 8, decimal_places: 4, iseditingreadonly: true)
                .Text("MiscPOID", header: "PO#", width: Widths.AnsiChars(13), iseditingreadonly: true);

            #endregion

            this.detailgrid.Columns[0].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns[5].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns[6].DefaultCellStyle.BackColor = Color.Pink;
        }
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["cdate"] = DateTime.Now;
            CurrentMaintain["Factoryid"] = factory;
            CurrentMaintain["PurchaseFrom"] = "T";
            CurrentMaintain["Status"] = "New";
            comboBox2.ReadOnly = true;
            CurrentMaintain["PurchaseType"] = "";
            CurrentMaintain["Handle"] = loginID;
        }
        protected override void OnDetailGridDataInserted(DataRow data)
        {
            base.OnDetailGridDataInserted(data);
            data["PurchaseFrom"] = CurrentMaintain["PurchaseFrom"];
        }
        
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(CurrentMaintain["PurchaseFrom"]))
            {
                MyUtility.Msg.WarningBox("<Purchase From> can't be empty.");
                comboBox1.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["cDate"]))
            {
                MyUtility.Msg.WarningBox("<Create Date> can't be empty.");
                dateBox1.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["Handle"]))
            {
                MyUtility.Msg.WarningBox("<Handle> can't be empty.");
                txtuser1.Focus();
                return false;
            }
            foreach (DataRow dr in this.DetailDatas)
            {
                if ((decimal)dr["Qty"] == 0)
                {
                    dr.Delete();
                    continue;
                }
            }
            if (this.DetailDatas.Count == 0)
            {
                MyUtility.Msg.WarningBox("Detail can not be empty.");
                return false;
            }

            #region 填入ID
            if (this.IsDetailInserting)
            {
                string key = keyWord + "MR";
                string id = MyUtility.GetValue.GetID(key, "MiscReq", (DateTime)CurrentMaintain["cDate"]);

                if (string.IsNullOrWhiteSpace(id))
                {
                    return false;
                }
                CurrentMaintain["ID"] = id;
            }
            #endregion

            return base.ClickSaveBefore();
        }
        protected override bool ClickEditBefore()
        {
            if (CurrentMaintain["Status"].ToString() != "New")
            {
                MyUtility.Msg.WarningBox("The record status was confirmed, you can't modify.", "Warning");
                return false;
            }

            return base.ClickEditBefore();
        }
        protected override bool ClickDeleteBefore()
        {
            if (CurrentMaintain["Status"].ToString() != "New")
            {
                MyUtility.Msg.WarningBox("The record status was confirmed, you can't delete.", "Warning");
                return false;
            }
            return base.ClickDeleteBefore();
        }
        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (comboBox1.SelectedValue.ToString() == comboBox1.OldValue.ToString()) return;
            DialogResult diresult = MyUtility.Msg.QuestionBox("The detail grid will be cleared, are you sure change type?");
            if (diresult == DialogResult.No)
            {
                return;
            }
            foreach (DataRow dr in this.DetailDatas)
            {
                dr.Delete();
            }
            if (comboBox1.SelectedValue.ToString() == "T") 
            {
                comboBox2.ReadOnly = true;
                CurrentMaintain["PurchaseType"] = "";
                CurrentMaintain["PurchaseFrom"] = "T";
            }
            else
            {
                comboBox2.ReadOnly = false;
                CurrentMaintain["PurchaseType"] = "General Affair";
                CurrentMaintain["PurchaseFrom"] = "L";
            }
        }
        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            base.ClickCheck();

            string updSql = string.Format("update MiscReq set Status = 'Approved' ,editname='{0}', editdate = GETDATE() ,Approve = '{0}',ApproveDate = GetDate() where id='{1}'", loginID, CurrentMaintain["ID"].ToString());

            DualResult upResult;
            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    if (!(upResult = DBProxy.Current.Execute(null, updSql)))
                    {
                        ShowErr(updSql, upResult);
                        return;
                    }
                    _transactionscope.Complete();
                    MyUtility.Msg.WarningBox("Successfully");
                }
                catch (Exception ex)
                {
                    ShowErr("Commit transaction error.", ex);
                    return;
                }
            }
            _transactionscope.Dispose();
            _transactionscope = null;

            this.RenewData();
            this.OnDetailEntered();
            EnsureToolbarExt();
        }
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            string check = string.Format("Select id from Miscreq_detail where id='{0}' and miscpoid!=null ",CurrentMaintain["ID"].ToString());
            if (MyUtility.Check.Seek(check))
            {
                MyUtility.Msg.WarningBox("Already created PO can't Unapprove.");
            }

            string updSql = string.Format("update Miscreq set Status = 'New' ,editname='{0}', editdate = GETDATE() ,Approve = '',ApproveDate = null where id='{1}'", loginID, CurrentMaintain["ID"].ToString());

            DualResult upResult;
            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    if (!(upResult = DBProxy.Current.Execute(null, updSql)))
                    {
                        ShowErr(updSql, upResult);
                        return;
                    }
                    _transactionscope.Complete();
                    MyUtility.Msg.WarningBox("Successfully");
                }
                catch (Exception ex)
                {
                    ShowErr("Commit transaction error.", ex);
                    return;
                }
            }
            _transactionscope.Dispose();
            _transactionscope = null;

            this.RenewData();
            this.OnDetailEntered();
            EnsureToolbarExt();
        }
    }
}
