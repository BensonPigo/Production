using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    public partial class P48 : Win.Tems.QueryForm
    {
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        private DataTable dtInventory;

        public P48(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        // Query
        private void btnFindNow_Click(object sender, EventArgs e)
        {
            StringBuilder strSQLCmd = new StringBuilder();
            string sp1 = this.txtSPNo1.Text.TrimEnd();
            string sp2 = this.txtSPNo2.Text.TrimEnd();
            string factory = this.txtfactory.Text.TrimEnd();
            string refno = this.txtRef.Text.TrimEnd();
            string location = this.txtLocation.Text.TrimEnd();
            string fabrictype = this.txtdropdownlistFabricType.SelectedValue.ToString();
            string strCategory = this.comboCategory.SelectedValue.ToString();
            string brand = this.txtbrand.Text;
            string season = this.txtseason.Text;

            if (string.IsNullOrWhiteSpace(sp1)
                && string.IsNullOrWhiteSpace(sp2)
                && string.IsNullOrWhiteSpace(refno)
                && string.IsNullOrWhiteSpace(location)
                && string.IsNullOrWhiteSpace(season))
            {
                MyUtility.Msg.WarningBox("< Season > < SP# > < Ref# > < Location > can't be empty!!");
                this.txtSPNo1.Focus();
                return;
            }
            else
            {
                strSQLCmd.Append(string.Format(
                    @"
select  0 as selected 
        , '' id
        , c.PoId
        , a.Seq1
        , a.Seq2
        , concat(Ltrim(Rtrim(a.seq1)), ' ', a.Seq2) as seq
        , dbo.getmtldesc(a.id,a.seq1,a.seq2,2,0) as [Description]
        , c.Roll
        , c.Dyelot
        , c.inqty-c.outqty + c.adjustqty as QtyBefore
        , 0.00 as QtyAfter
        , dbo.Getlocation(c.ukey) as location
        , '' reasonid
        , '' reason_nm
        , a.FabricType
        , a.stockunit
        , c.stockType
        , c.ukey as ftyinventoryukey
        , [CreateStatus]='' 
        , [FabricTypeName] = (select name from DropDownList where Type='FabricType_Condition' and id=a.fabrictype)		
        , [Category] = case o.Category  when 'B' then 'Bulk'
										when 'M' then 'Material'
										when 'S' then 'Sample'
										when 'T' then 'SMTL' end
		,o.OrderTypeID
		,o.BrandID
        ,[MCHandle] = dbo.getPass1_ExtNo(o.MCHandle)
from dbo.PO_Supp_Detail a WITH (NOLOCK) 
inner join dbo.ftyinventory c WITH (NOLOCK) on c.poid = a.id and c.seq1 = a.seq1 and c.seq2  = a.seq2 and c.stocktype = 'O'
inner join dbo.factory f WITH (NOLOCK) on a.FactoryID=f.id
left join Orders o WITH (NOLOCK) on o.id=a.id
Where   c.lock = 0 
        and c.inqty-c.outqty + c.adjustqty > 0
        and f.mdivisionid = '{0}'        
        ", Sci.Env.User.Keyword));

                if (!MyUtility.Check.Empty(sp1))
                {
                    strSQLCmd.Append(string.Format(
                        @" 
        and a.id >= '{0}'  ", sp1));
                }

                if (!MyUtility.Check.Empty(sp2))
                {
                    strSQLCmd.Append(string.Format(
                        @" 
        and a.id <= '{0}'  ", sp2));
                }

                if (!MyUtility.Check.Empty(refno))
                {
                    strSQLCmd.Append(string.Format(
                        @" 
        and a.refno = '{0}' ", refno));
                }

                if (!MyUtility.Check.Empty(factory))
                {
                    strSQLCmd.Append($"AND o.FtyGroup='{factory}'");
                }

                if (!MyUtility.Check.Empty(brand))
                {
                    strSQLCmd.Append(string.Format(
                        @" 
        and o.BrandID = '{0}' ", brand));
                }

                if (!MyUtility.Check.Empty(season))
                {
                    strSQLCmd.Append(string.Format(
                        @" 
        and o.SeasonID = '{0}' ", season));
                }

                if (!MyUtility.Check.Empty(location))
                {
                    strSQLCmd.Append(string.Format(
                        @" 
        and c.ukey in ( select ukey 
                        from dbo.ftyinventory_detail WITH (NOLOCK) 
                        where mtllocationid = '{0}') ", location));
                }

                switch (fabrictype)
                {
                    case "ALL":
                        break;
                    case "F":
                        strSQLCmd.Append(@" 
        And a.fabrictype = 'F'");
                        break;
                    case "A":
                        strSQLCmd.Append(@" 
        And a.fabrictype = 'A'");
                        break;
                }

                strSQLCmd.Append($@" and o.Category in ({strCategory})");

                this.ShowWaitMessage("Data Loading....");
                DualResult result;
                if (result = DBProxy.Current.Select(null, strSQLCmd.ToString(), out this.dtInventory))
                {
                    if (this.dtInventory.Rows.Count == 0)
                    {
                        MyUtility.Msg.WarningBox("Data not found!!");
                    }
                    else
                    {
                        this.dtInventory.Columns.Add("adjustqty", typeof(decimal));
                        this.dtInventory.Columns["adjustqty"].Expression = "qtybefore-qtyafter";
                        this.dtInventory.DefaultView.Sort = "poid,seq1,seq2,roll,dyelot";
                    }

                    this.listControlBindingSource1.DataSource = this.dtInventory;
                }
                else
                {
                    this.ShowErr(strSQLCmd.ToString(), result);
                }

                this.HideWaitMessage();
            }

            this.btnImport.Enabled = true;
            this.HideWaitMessage();
        }

        // Form Load
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            #region -- Reason Combox --
            string selectCommand = @"select Name idname,id from Reason WITH (NOLOCK) where ReasonTypeID='Stock_Remove' AND junk = 0";
            DualResult returnResult;
            DataTable dropDownListTable = new DataTable();
            if (returnResult = DBProxy.Current.Select(null, selectCommand, out dropDownListTable))
            {
                this.comboReason.DataSource = dropDownListTable;
                this.comboReason.DisplayMember = "IDName";
                this.comboReason.ValueMember = "ID";
            }
            #endregion
            #region -- Current Qty Valid --
            DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            ns.CellValidating += (s, e) =>
            {
                if (this.EditMode && !MyUtility.Check.Empty(e.FormattedValue))
                {
                    DataRow dr = this.gridImport.GetDataRow(e.RowIndex);

                    if (MyUtility.Convert.GetDecimal(dr["QtyBefore"]) - MyUtility.Convert.GetDecimal(e.FormattedValue) <= 0)
                    {
                        dr["qtyafter"] = 0;
                        return;
                    }
                    else
                    {
                        dr["qtyafter"] = e.FormattedValue;
                        dr["selected"] = true;
                    }
                }
            };
            #endregion
            #region -- Reason ID 右鍵開窗 --
            DataGridViewGeneratorTextColumnSettings ts = new DataGridViewGeneratorTextColumnSettings();
            ts.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    DataTable poitems;
                    string sqlcmd = string.Empty;
                    IList<DataRow> x;

                    sqlcmd = @"select id, Name from Reason WITH (NOLOCK) where ReasonTypeID='Stock_Remove' AND junk = 0";
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
                        this.gridImport.GetDataRow(this.gridImport.GetSelectedRowIndex())["reasonid"].ToString(),
                        "ID,Name");
                    item.Width = 600;
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    x = item.GetSelecteds();

                    this.gridImport.GetDataRow(this.gridImport.GetSelectedRowIndex())["reasonid"] = x[0]["id"];
                    this.gridImport.GetDataRow(this.gridImport.GetSelectedRowIndex())["reason_nm"] = x[0]["name"];
                }
            };
            ts.CellValidating += (s, e) =>
            {
                DataRow dr;
                if (!this.EditMode)
                {
                    return;
                }

                if (string.Compare(e.FormattedValue.ToString(), this.gridImport.GetDataRow(this.gridImport.GetSelectedRowIndex())["reasonid"].ToString()) != 0)
                {
                    if (MyUtility.Check.Empty(e.FormattedValue))
                    {
                        this.gridImport.GetDataRow(this.gridImport.GetSelectedRowIndex())["reasonid"] = string.Empty;
                        this.gridImport.GetDataRow(this.gridImport.GetSelectedRowIndex())["reason_nm"] = string.Empty;
                    }
                    else
                    {
                        if (!MyUtility.Check.Seek(
                            string.Format(
                            @"select id, Name from Reason WITH (NOLOCK) where id = '{0}' 
and ReasonTypeID='Stock_Remove' AND junk = 0", e.FormattedValue), out dr, null))
                        {
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox("Data not found!", "Reason ID");
                            return;
                        }
                        else
                        {
                            this.gridImport.GetDataRow(this.gridImport.GetSelectedRowIndex())["reasonid"] = e.FormattedValue;
                            this.gridImport.GetDataRow(this.gridImport.GetSelectedRowIndex())["reason_nm"] = dr["name"];
                        }
                    }
                }
            };
            #endregion

            this.gridImport.IsEditingReadOnly = false; // 必設定, 否則CheckBox會顯示圖示
            this.gridImport.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridImport)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk) // 0
                .Text("poid", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(14))
                .Text("seq", header: "Seq#", iseditingreadonly: true, width: Widths.AnsiChars(6))
                .Text("OrderTypeID", header: "Order Type", iseditingreadonly: true, width: Widths.AnsiChars(13))
                .Text("BrandID", header: "Brand", iseditingreadonly: true, width: Widths.AnsiChars(7))
                .Text("roll", header: "Roll", iseditingreadonly: true, width: Widths.AnsiChars(6))
                .Text("dyelot", header: "Dyelot", iseditingreadonly: true, width: Widths.AnsiChars(8))
                .EditText("Description", header: "Description", iseditingreadonly: true, width: Widths.AnsiChars(20))
                .Text("FabricTypeName", header: "Fabric Type", iseditingreadonly: true, width: Widths.AnsiChars(8))
                .Text("Category", header: "Category", iseditingreadonly: true, width: Widths.AnsiChars(8))
                .Numeric("QtyBefore", header: "Original Qty", iseditable: true, decimal_places: 2, integer_places: 10, width: Widths.AnsiChars(6))
                .Numeric("QtyAfter", header: "Current Qty", decimal_places: 2, integer_places: 10, settings: ns, width: Widths.AnsiChars(6))
                .Numeric("adjustqty", header: "Remove Qty", decimal_places: 2, integer_places: 10, width: Widths.AnsiChars(6))
                .Text("location", header: "Location", iseditingreadonly: true, width: Widths.AnsiChars(6))
                .Text("reasonid", header: "Reason ID", settings: ts, width: Widths.AnsiChars(6))
                .Text("reason_nm", header: "Reason Name", iseditingreadonly: true, width: Widths.AnsiChars(20))
                .Text("MCHandle", header: "MC Handle", iseditingreadonly: true, width: Widths.AnsiChars(20))
                .Text("CreateStatus", header: "Create Status", iseditingreadonly: true, width: Widths.AnsiChars(50))
               ;

            this.gridImport.Columns["QtyAfter"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridImport.Columns["reasonid"].DefaultCellStyle.BackColor = Color.Pink;
            this.comboCategory.SelectedIndex = 4;
        }

        // Close
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Update All
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
                item["reasonid"] = reasonid;
                item["reason_nm"] = this.comboReason.Text;
            }
        }

        // Location Valid
        private void txtLocation_Validating(object sender, CancelEventArgs e)
        {
            if (this.txtLocation.Text.ToString() == string.Empty)
            {
                return;
            }

            if (!MyUtility.Check.Seek(
                string.Format(
                @"
select 1 
where exists(
    select * 
    from    dbo.MtlLocation WITH (NOLOCK) 
    where   StockType='O' 
            and id = '{0}'
            and junk != '1'
)", this.txtLocation.Text), null))
            {
                e.Cancel = true;
                MyUtility.Msg.WarningBox("Location is not exist!!", "Data not found");
            }
        }

        // Location 右鍵
        private void txtLocation_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            if (!this.EditMode)
            {
                return;
            }

            Win.Tools.SelectItem item = new Win.Tools.SelectItem(
                string.Format(@"
select  id
        , [Description] 
from    dbo.MtlLocation WITH (NOLOCK) 
where   StockType='O'
        and junk != '1'"), "10,40", this.txtLocation.Text, "ID,Desc");
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.txtLocation.Text = item.GetSelectedString();
        }

        // Create Batch
        private void btnImport_Click(object sender, EventArgs e)
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

            dr2 = dtGridBS1.Select("reasonid = '' and Selected = 1");
            DataTable warningTB = new DataTable();
            if (dr2.Length > 0)
            {
                warningTB.Columns.Add("SpNo");
                warningTB.Columns.Add("SEQ");
                warningTB.Columns.Add("Roll");
                foreach (DataRow drReason in dr2)
                {
                    DataRow drNoReason = warningTB.NewRow();
                    drNoReason["SpNo"] = drReason["POID"].ToString();
                    drNoReason["SEQ"] = drReason["seq"].ToString();
                    drNoReason["Roll"] = drReason["Roll"].ToString();
                    warningTB.Rows.Add(drNoReason);
                }

                if (warningTB.Rows.Count > 0)
                {
                    var m = MyUtility.Msg.ShowMsgGrid(warningTB, "These SP#'s Reason ID cannot be empty!", "Warning");
                    m.Width = 650;
                    m.grid1.Columns[0].Width = 200;
                    m.grid1.Columns[0].Width = 50;
                    m.grid1.Columns[0].Width = 100;
                    m.text_Find.Width = 150;
                    m.btn_Find.Location = new Point(170, 6);

                    m.btn_Find.Anchor = AnchorStyles.Left | AnchorStyles.Top;
                    return;
                }
            }

            #region Batch Create

            // *依照POID 批次建立P45 ID
            dr2 = dtGridBS1.Select("adjustqty <> 0 and Selected = 1");
            var listPoid = dr2.Select(row => row["Poid"]).Distinct().ToList();
            var tmpId = MyUtility.GetValue.GetBatchID(Sci.Env.User.Keyword + "AM", "Adjust", System.DateTime.Now, batchNumber: listPoid.Count);
            if (MyUtility.Check.Empty(tmpId))
            {
                MyUtility.Msg.WarningBox("Get document id fail!");
                return;
            }

            this.ShowWaitMessage("Data Creating....");
            #region insert Table

            string insertMaster = @"
insert into Adjust
        (id      , type   , issuedate, mdivisionid, FactoryID
         , status, addname, adddate  , remark)
select   id      , type   , issuedate, mdivisionid, FactoryID
         , status, addname, adddate   , remark
from #tmp";
            string insertDetail = @"
insert into Adjust_Detail
(ID, FtyInventoryUkey ,MDivisionID ,POID ,Seq1 ,Seq2 ,Roll    
,Dyelot ,StockType ,QtyBefore ,QtyAfter ,ReasonID )
select *
from #tmp";

            DataTable dtMaster = new DataTable();
            dtMaster.Columns.Add("ID");
            dtMaster.Columns.Add("MdivisionID");
            dtMaster.Columns.Add("FactoryID");
            dtMaster.Columns.Add("IssueDate");
            dtMaster.Columns.Add("Status");
            dtMaster.Columns.Add("AddName");
            dtMaster.Columns.Add("AddDate");
            dtMaster.Columns.Add("Type");
            dtMaster.Columns.Add("Remark");
            dtMaster.Columns.Add("Poid");

            DataTable dtDetail = new DataTable();
            dtDetail.Columns.Add("ID");
            dtDetail.Columns.Add("FtyInventoryUkey");
            dtDetail.Columns.Add("MDivisionID");
            dtDetail.Columns.Add("POID");
            dtDetail.Columns.Add("Seq1");
            dtDetail.Columns.Add("Seq2");
            dtDetail.Columns.Add("Roll");
            dtDetail.Columns.Add("Dyelot");
            dtDetail.Columns.Add("StockType");
            dtDetail.Columns.Add("QtyBefore");
            dtDetail.Columns.Add("QtyAfter");
            dtDetail.Columns.Add("ReasonID");

            for (int i = 0; i < listPoid.Count; i++)
            {
                DataRow drNewMaster = dtMaster.NewRow();
                drNewMaster["poid"] = listPoid[i].ToString();
                drNewMaster["id"] = tmpId[i].ToString();
                drNewMaster["type"] = "R";
                drNewMaster["issuedate"] = DateTime.Now.ToString("yyyy/MM/dd");
                drNewMaster["mdivisionid"] = Env.User.Keyword;
                drNewMaster["FactoryID"] = Sci.Env.User.Factory;
                drNewMaster["status"] = "New";
                drNewMaster["addname"] = Env.User.UserID;
                drNewMaster["adddate"] = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff");
                drNewMaster["remark"] = "Batch create by P48";
                dtMaster.Rows.Add(drNewMaster);
            }

            foreach (DataRow item in dr2)
            {
                DataRow[] drGetID = dtMaster.AsEnumerable().Where(row => row["poid"].EqualString(item["POID"])).ToArray();
                DataRow drNewDetail = dtDetail.NewRow();
                drNewDetail["ID"] = drGetID[0]["ID"];
                drNewDetail["FtyInventoryUkey"] = item["FtyInventoryUkey"];
                drNewDetail["MDivisionID"] = Env.User.Keyword;
                drNewDetail["POID"] = item["POID"];
                drNewDetail["Seq1"] = item["Seq1"];
                drNewDetail["Seq2"] = item["Seq2"];
                drNewDetail["Roll"] = item["Roll"];
                drNewDetail["Dyelot"] = item["Dyelot"];
                drNewDetail["StockType"] = item["StockType"];
                drNewDetail["QtyBefore"] = item["QtyBefore"];
                drNewDetail["QtyAfter"] = item["QtyAfter"];
                drNewDetail["ReasonID"] = item["ReasonID"];
                dtDetail.Rows.Add(drNewDetail);
            }

            #endregion

            #region TransactionScope
            TransactionScope _transactionscope = new TransactionScope();
            DualResult result;
            using (_transactionscope)
            {
                try
                {
                    DataTable dtResult;
                    if ((result = MyUtility.Tool.ProcessWithDatatable(dtMaster, null, insertMaster, out dtResult)) == false)
                    {
                        _transactionscope.Dispose();
                        MyUtility.Msg.WarningBox(result.ToString(), "Create failed");
                        return;
                    }

                    if ((result = MyUtility.Tool.ProcessWithDatatable(dtDetail, null, insertDetail, out dtResult)) == false)
                    {
                        _transactionscope.Dispose();
                        MyUtility.Msg.WarningBox(result.ToString(), "Create failed");
                        return;
                    }

                    _transactionscope.Complete();
                    _transactionscope.Dispose();
                }
                catch (Exception ex)
                {
                    _transactionscope.Dispose();
                    this.ShowErr("Commit transaction error.", ex);
                    return;
                }
            }

            _transactionscope = null;
            #endregion

            // Create後Btn失效，需重新Qurey才能再使用。
            this.btnImport.Enabled = false;

            #region Confirmed
            if (tmpId.Count < 1)
            {
                return;
            }

            for (int i = 0; i < listPoid.Count; i++)
            {
                DataTable[] dts;
                DualResult res = DBProxy.Current.SelectSP(string.Empty, "dbo.usp_RemoveScrapById", new List<SqlParameter> { new SqlParameter("@ID", tmpId[i].ToString()) }, out dts);
                if (!res)
                {
                    DataRow[] drfound = this.dtInventory.Select(string.Format("poid='{0}' and selected=1", listPoid[i].ToString()));
                    foreach (var item in drfound)
                    {
                        item["CreateStatus"] = string.Format("{0} Confirmed Fail! ", tmpId[i].ToString()) + res.ToString();
                    }
                }

                if (dts.Length > 0)
                {
                    foreach (DataRow drs in dts[0].Rows)
                    {
                        if (MyUtility.Convert.GetDecimal(drs["q"]) < 0)
                        {
                            DataRow[] drfail = this.dtInventory.Select(string.Format(@"poid='{0}' and seq1='{1}' and seq2='{2}'", drs["POID"].ToString(), drs["seq1"].ToString(), drs["seq2"].ToString()));
                            foreach (var item in drfail)
                            {
                                item["CreateStatus"] = string.Format(
                                    @"{2}'s balance: {0} is less than Adjust qty: {1}
                                    Balacne Qty is not enough!!", drs["balance"].ToString(), drs["Adjustqty"].ToString(), tmpId[i].ToString());
                            }
                        }
                    }
                }
                else
                {
                    DataRow[] drfound = this.dtInventory.Select(string.Format("poid='{0}' and selected=1", listPoid[i].ToString()));
                    foreach (var item in drfound)
                    {
                        item["CreateStatus"] = string.Format("{0} Create and Confirm Success ", tmpId[i].ToString());
                    }
                }
            }
            #endregion
            MyUtility.Msg.InfoBox("Create Successful.");

            // DataTable dtCreate = new DataTable();
            // dtCreate.Columns.Add("ID");
            // for (int i = 0; i < listPoid.Count; i++)
            // {
            //    DataRow drCreate = dtCreate.NewRow();
            //    drCreate["ID"] = tmpId[i].ToString();
            //    dtCreate.Rows.Add(drCreate);
            // }
            // if (dtCreate.Rows.Count > 0)
            // {
            //    var m = MyUtility.Msg.ShowMsgGrid(dtCreate, "These Adjust ID have been created.", "Create Successful.");
            //    m.Width = 400;
            //    m.grid1.Columns[0].Width = 150;
            //    m.text_Find.Width = 150;
            //    m.btn_Find.Location = new Point(170, 6);
            //    m.btn_Find.Anchor = (AnchorStyles.Left | AnchorStyles.Top);
            // }
            #endregion
            this.HideWaitMessage();
        }
    }
}
