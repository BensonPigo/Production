using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Transactions;
using Ict;
using Ict.Win;
using Sci.Data;
using System.IO;

namespace Sci.Production.Quality
{
    public partial class P27 : Win.Tems.QueryForm
    {
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_location;
        private DataTable gridData;
        private readonly IList<string> comboBox2_RowSource1 = new List<string>();
        private readonly IList<string> comboBox2_RowSource2 = new List<string>();
        private readonly IList<string> comboBox2_RowSource3 = new List<string>();
        private readonly IList<string> comboBox2_RowSource4 = new List<string>();
        private readonly IList<string> comboBox2_RowSource5 = new List<string>();
        private readonly IList<string> comboBox3_RowSource = new List<string>();
        private BindingSource comboxbs1;
        private BindingSource comboxbs2;
        private readonly string selectDataTable_DefaultView_Sort = string.Empty;

        public P27(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            this.EditMode = true;
            base.OnFormLoaded();
            this.txtCFALocation.M = Env.User.Keyword;

            Dictionary<string, string> comboBox1_RowSource = new Dictionary<string, string>();
            comboBox1_RowSource.Add("1", string.Empty);
            comboBox1_RowSource.Add("2", "Ctn#");
            comboBox1_RowSource.Add("3", "Colorway");
            comboBox1_RowSource.Add("4", "Color");
            comboBox1_RowSource.Add("5", "Size");
            this.comboxbs1 = new BindingSource(comboBox1_RowSource, null);
            this.comboFilter.DataSource = this.comboxbs1;
            this.comboFilter.ValueMember = "Key";
            this.comboFilter.DisplayMember = "Value";

            this.comboxbs2 = new BindingSource(this.comboBox2_RowSource1, null);
            this.comboFilter2.DataSource = this.comboxbs2;

            this.gridPackID.CellValueChanged += (s, e) =>
            {
                if (this.gridPackID.Columns[e.ColumnIndex].DataPropertyName == this.col_location.DataPropertyName)
                {
                    DataRow dr = this.gridPackID.GetDataRow<DataRow>(e.RowIndex);
                    dr["Selected"] = 1;
                }
            };

            this.gridPackID.CellValueChanged += (s, e) =>
            {
                if (this.gridPackID.Columns[e.ColumnIndex].Name == this.col_chk.Name)
                {
                    this.CalcCTNQty();
                }
            };

            this.gridPackID.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.gridPackID)
            .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
            .Text("ID", header: "Pack ID", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("OrderId", header: "SP#", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("CustPONo", header: "P.O.#", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("CTNStartNo", header: "CTN#", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Text("Article", header: "Color Way", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("Color", header: "Color", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Text("SizeCode", header: "Size", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("QtyPerCTN", header: "PC/Ctn", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Numeric("ShipQty", header: "Qty", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .CellCFALocation("CFALocationID", header: "Location No", width: Widths.AnsiChars(10), m: Env.User.Keyword).Get(out this.col_location)
            .Text("Remark", header: "Remarks", width: Widths.AnsiChars(10))
            ;
        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtSPNoStart.Text) && MyUtility.Check.Empty(this.txtSPNoEnd.Text)
                && MyUtility.Check.Empty(this.txtPackIDStart.Text) && MyUtility.Check.Empty(this.txtPackIDEnd.Text)
                && MyUtility.Check.Empty(this.txtPONoStart.Text) && MyUtility.Check.Empty(this.txtPONoEnd.Text))
            {
                this.txtSPNoStart.Focus();
                MyUtility.Msg.WarningBox("< SP# > or < Pack ID > or < PO# > can not empty!");
                return;
            }

            this.numSelectQty.Value = 0;
            this.numTTLCTNQty.Value = 0;
            this.gridData = null;
            this.listControlBindingSource1.DataSource = null;

            if (this.Query(false))
            {
                return;
            }
        }

        private void BtnUpdateAllLocation_Click(object sender, EventArgs e)
        {
            string location = this.txtCFALocation.Text.Trim();
            int pos = this.listControlBindingSource1.Position;
            DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
            if (MyUtility.Check.Empty(dt))
            {
                MyUtility.Msg.WarningBox("Please select data first!");
                return;
            }

            foreach (DataRow currentRecord in dt.Rows)
            {
                if (currentRecord["selected"].ToString() == "1")
                {
                    currentRecord["CFALocationID"] = location;
                }
            }

            this.listControlBindingSource1.Position = pos;
            this.gridPackID.SuspendLayout();
            this.gridPackID.DataSource = null;
            this.gridPackID.DataSource = this.listControlBindingSource1;
            this.listControlBindingSource1.Position = pos;
            this.gridPackID.ResumeLayout();
        }

        // Save
        private void BtnSave_Click(object sender, EventArgs e)
        {
            this.gridPackID.ValidateControl();
            this.gridPackID.EndEdit();
            this.listControlBindingSource1.EndEdit();
            DataTable detailData = (DataTable)this.listControlBindingSource1.DataSource;
            if (MyUtility.Check.Empty(detailData))
            {
                MyUtility.Msg.WarningBox("Please select data first!");
                return;
            }

            DataRow[] dr = detailData.Select("Selected = 1");
            if (dr.Length <= 0)
            {
                MyUtility.Msg.WarningBox("Please select at least one data!");
                return;
            }

            #region 更新資料到ClogReceive_Detail & PackingList_Detail
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    bool lastResult = true;

                    // 只存畫面上看到的那幾筆資料
                    foreach (DataRowView currentRowView in this.gridData.DefaultView)
                    {
                        DataRow currentRow = currentRowView.Row;
                        if (currentRow["Selected"].ToString() == "1")
                        {
                            DataTable tmpTable;
                            string chkCmd = string.Format(@"SELECT CFALocationID FROM PackingList_Detail PL WITH (NOLOCK) WHERE id ='{0}' AND CTNStartNo = '{1}' ", currentRow["ID"].ToString(), currentRow["CTNStartNo"].ToString());

                            DualResult result1CHK = DBProxy.Current.Select(null, chkCmd, out tmpTable);

                            string updateCmd = string.Empty;

                            if (tmpTable.Rows[0]["CFALocationID"].ToString() != currentRow["CFALocationID"].ToString())
                            {
                                updateCmd = @"update PackingList_Detail 
                                                set CFALocationID = @CFALocationID, Remark = @remark ,EditCFALocationDate=@EditCFALocationDate, EditCFALocationName =@EditCFALocationName 
                                                where id = @id and CTNStartNo = @ctnStartNo;";
                            }
                            else
                            {
                                updateCmd = @"update PackingList_Detail 
                                                set CFALocationID = @CFALocationID, Remark = @remark 
                                                where id = @id and CTNStartNo = @ctnStartNo;";
                            }

                            #region 準備sql參數資料
                            System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
                            sp1.ParameterName = "@CFALocationID";
                            sp1.Value = currentRow["CFALocationID"].ToString();

                            System.Data.SqlClient.SqlParameter sp3 = new System.Data.SqlClient.SqlParameter();
                            sp3.ParameterName = "@id";
                            sp3.Value = currentRow["ID"].ToString();

                            System.Data.SqlClient.SqlParameter sp5 = new System.Data.SqlClient.SqlParameter();
                            sp5.ParameterName = "@ctnStartNo";
                            sp5.Value = currentRow["CTNStartNo"].ToString();

                            System.Data.SqlClient.SqlParameter sp6 = new System.Data.SqlClient.SqlParameter();
                            sp6.ParameterName = "@remark";
                            sp6.Value = currentRow["Remark"].ToString();

                            System.Data.SqlClient.SqlParameter sp7 = new System.Data.SqlClient.SqlParameter();
                            System.Data.SqlClient.SqlParameter sp8 = new System.Data.SqlClient.SqlParameter();
                            if (tmpTable.Rows[0]["CFALocationID"].ToString() != currentRow["CFALocationID"].ToString())
                            {
                                sp7.ParameterName = "@EditCFALocationDate";
                                sp7.Value = DateTime.Now;
                                sp8.ParameterName = "@EditCFALocationName";
                                sp8.Value = Env.User.UserID;
                            }

                            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                            cmds.Add(sp1);
                            cmds.Add(sp3);
                            cmds.Add(sp5);
                            cmds.Add(sp6);

                            if (tmpTable.Rows[0]["CFALocationID"].ToString() != currentRow["CFALocationID"].ToString())
                            {
                                cmds.Add(sp7);
                                cmds.Add(sp8);
                            }
                            #endregion
                            DualResult result = DBProxy.Current.Execute(null, updateCmd, cmds);
                            if (!result)
                            {
                                lastResult = false;
                            }
                        }
                    }

                    if (lastResult)
                    {
                        transactionScope.Complete();
                    }
                    else
                    {
                        transactionScope.Dispose();
                        MyUtility.Msg.WarningBox("Save failed, Pleaes re-try");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    transactionScope.Dispose();
                    this.ShowErr("Commit transaction error.", ex);
                    return;
                }
            }
            #endregion

            // 更新成功後，將畫面上的資料都清空
            this.txtSPNoStart.Text = string.Empty;
            this.txtSPNoEnd.Text = string.Empty;
            this.txtPackIDStart.Text = string.Empty;
            this.txtPackIDEnd.Text = string.Empty;
            this.txtPONoStart.Text = string.Empty;
            this.txtPONoEnd.Text = string.Empty;
            this.numSelectQty.Value = 0;
            this.numTTLCTNQty.Value = 0;
            this.comboFilter.SelectedValue = "1";
            this.comboFilter2.SelectedIndex = -1;

            string sqlCmd = @"select 0 as selected,  b.ID, b.OrderID, c.CustPONo, b.CTNStartNo, b.Article, b.Color, b.SizeCode, b.QtyPerCTN, b.ShipQty, b.CFALocationID, b.Remark
                                           from PackingList a WITH (NOLOCK) , PackingList_Detail b WITH (NOLOCK) , Orders c WITH (NOLOCK) where 1=0";
            DualResult result1 = DBProxy.Current.Select(null, sqlCmd, out this.gridData);

            this.listControlBindingSource1.DataSource = this.gridData;
        }

        // Cancel
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Filter
        private void ComboFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboFilter.SelectedIndex != -1)
            {
                this.comboFilter2.Enabled = true;
                switch (this.comboFilter.SelectedValue.ToString())
                {
                    case "1":
                        this.comboFilter2.Enabled = false;
                        this.comboFilter2.DataSource = this.comboBox2_RowSource1;
                        if (this.gridData != null)
                        {
                            this.gridData.DefaultView.RowFilter = string.Empty;
                        }

                        break;
                    case "2":
                        this.comboFilter2.DataSource = this.comboBox2_RowSource2;
                        this.comboFilter2.SelectedIndex = -1;
                        if (this.gridData != null)
                        {
                            this.gridData.DefaultView.RowFilter = string.Empty;
                        }

                        break;
                    case "3":
                        this.comboFilter2.DataSource = this.comboBox2_RowSource3;
                        this.comboFilter2.SelectedIndex = -1;
                        if (this.gridData != null)
                        {
                            this.gridData.DefaultView.RowFilter = string.Empty;
                        }

                        break;
                    case "4":
                        this.comboFilter2.DataSource = this.comboBox2_RowSource4;
                        this.comboFilter2.SelectedIndex = -1;
                        if (this.gridData != null)
                        {
                            this.gridData.DefaultView.RowFilter = string.Empty;
                        }

                        break;
                    case "5":
                        this.comboFilter2.DataSource = this.comboBox2_RowSource5;
                        this.comboFilter2.SelectedIndex = -1;
                        if (this.gridData != null)
                        {
                            this.gridData.DefaultView.RowFilter = string.Empty;
                        }

                        break;
                    default:
                        this.comboFilter2.Enabled = false;
                        this.comboFilter2.DataSource = this.comboBox2_RowSource1;
                        break;
                }
            }
        }

        private void ComboFilter2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboFilter2.SelectedIndex != -1)
            {
                switch (this.comboFilter.SelectedValue.ToString())
                {
                    case "1":
                        break;
                    case "2":
                        this.gridData.DefaultView.RowFilter = "CTNStartNo = '" + this.comboFilter2.SelectedValue.ToString() + "'";
                        break;
                    case "3":
                        this.comboFilter2.DataSource = this.comboBox2_RowSource3;
                        this.gridData.DefaultView.RowFilter = "Article  = '" + this.comboFilter2.SelectedValue.ToString() + "'";
                        break;
                    case "4":
                        this.comboFilter2.DataSource = this.comboBox2_RowSource4;
                        this.gridData.DefaultView.RowFilter = "Color = '" + this.comboFilter2.SelectedValue.ToString() + "'";
                        break;
                    case "5":
                        this.comboFilter2.DataSource = this.comboBox2_RowSource5;
                        this.gridData.DefaultView.RowFilter = "SizeCode = '" + this.comboFilter2.SelectedValue.ToString() + "'";
                        break;
                    default:
                        this.comboFilter2.DataSource = this.comboBox2_RowSource1;
                        this.gridData.DefaultView.RowFilter = string.Empty;
                        break;
                }
            }
        }

        private string whereS;

        private void BtnImportFromBarcode_Click(object sender, EventArgs e)
        {
            // 設定只能選txt檔
            this.openFileDialog1.Filter = "txt files (*.txt)|*.txt";

            // 開窗且有選擇檔案
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.numSelectQty.Value = 0;
                this.numTTLCTNQty.Value = 0;

                // 讀檔案
                string wheresql = string.Empty;
                this.whereS = string.Empty;
                using (StreamReader reader = new StreamReader(this.openFileDialog1.FileName, Encoding.UTF8))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        System.Diagnostics.Debug.WriteLine(line);
                        IList<string> sl = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                        foreach (string item in sl)
                        {
                            if (item.Length >= 13)
                            {
                                wheresql += $" or (PL.ID = '{item.Substring(0, 13)}' and PLD.CTNStartNo = '{item.Substring(13, item.Length - 13)}') ";
                            }
                        }
                    }

                    if (MyUtility.Check.Empty(wheresql))
                    {
                        MyUtility.Msg.WarningBox("Connection faile.!");
                        return;
                    }

                    this.whereS = $" and(1=0 {wheresql} )";

                    this.gridData = null;
                    this.listControlBindingSource1.DataSource = null;
                    this.Query(true);
                }
            }
        }

        private bool Query(bool isimport)
        {
            StringBuilder sqlCmd = new StringBuilder();

            sqlCmd.Append(string.Format(
                @"
select *
       , rn = ROW_NUMBER() over(order by ID,OrderID,(RIGHT(REPLICATE('0', 6) + rtrim(ltrim(CTNStartNo)), 6)))
       , rn1 = ROW_NUMBER() over (order by TRY_CONVERT (int, CTNStartNo), (RIGHT (REPLICATE ('0', 6) + rtrim (ltrim (CTNStartNo)), 6)))
from (
    select 0 as selected
           , d.*
           , TransferPerCent = iif(d.TotalCTN > 0, ROUND((CONVERT(decimal, d.ClogCTN) / d.TotalCTN), 4), 0)
           , QtyPerCTN = (select sum(QtyPerCTN) 
                          from PackingList_Detail WITH (NOLOCK) 
                          where ID = d.ID 
                                and CTNStartNo = d.CTNStartNo)
           , ShipQty = (select sum(ShipQty) 
                        from PackingList_Detail WITH (NOLOCK) 
                        where ID = d.ID 
                              and CTNStartNo = d.CTNStartNo)
           , Article = stuff((select ',' + cast(Article as nvarchar) 
                                  from PackingList_Detail WITH (NOLOCK) 
                                  where ID = d.ID 
                                        and CTNStartNo = d.CTNStartNo for xml path(''))
                             , 1, 1, '')
           , Color = stuff((select ',' + cast(Color as nvarchar)
                            from PackingList_Detail WITH (NOLOCK) 
                            where ID = d.ID 
                                  and CTNStartNo = d.CTNStartNo for xml path(''))
                           , 1, 1, '')
           , SizeCode =  stuff((select ',' + cast(SizeCode as nvarchar) 
                                from PackingList_Detail WITH (NOLOCK) 
                                where ID = d.ID 
                                      and CTNStartNo = d.CTNStartNo for xml path(''))
                               , 1, 1, '')
    from (
          select distinct  PLD.ID
                 , PLD.OrderID
                 , orders.CustPONo
                 , PLD.CTNStartNo
                 , PLD.CFALocationID
                 , PLD.Remark
                 , orders.BrandID
                 , orders.BuyerDelivery
                 , orders.StyleID
                 , orders.FtyGroup
                 , Dest = '(' + orders.Dest + ')' + isnull((select Alias 
                                                            from Country 
                                                            where ID = orders.Dest),'')
                 , orders.TotalCTN
                 , orders.ClogCTN
          from PackingList PL WITH (NOLOCK)
          inner join PackingList_Detail PLD WITH (NOLOCK) on PL.ID = PLD.ID
          inner join Orders orders WITH (NOLOCK) on PLD.OrderID = orders.ID
          where (PL.Type = 'B' or PL.Type = 'L')
                and PLD.ReceiveDate is not null
		        and PLD.TransferCFADate is not null
		        and PLD.CFAReceiveDate is not null
                and PLD.CTNQty = 1
                and orders.MDivisionID =  '{0}'", Env.User.Keyword));
            #region 組條件
            if (!isimport)
            {
                if (!MyUtility.Check.Empty(this.txtSPNoStart.Text))
                {
                    sqlCmd.Append(string.Format(" and orders.ID >= '{0}'", this.txtSPNoStart.Text));
                }

                if (!MyUtility.Check.Empty(this.txtSPNoEnd.Text))
                {
                    sqlCmd.Append(string.Format(" and orders.ID <= '{0}'", this.txtSPNoEnd.Text));
                }

                if (!MyUtility.Check.Empty(this.txtPackIDStart.Text))
                {
                    sqlCmd.Append(string.Format(" and PL.ID >= '{0}'", this.txtPackIDStart.Text));
                }

                if (!MyUtility.Check.Empty(this.txtPackIDEnd.Text))
                {
                    sqlCmd.Append(string.Format(" and PL.ID <= '{0}'", this.txtPackIDEnd.Text));
                }

                if (!MyUtility.Check.Empty(this.txtPONoStart.Text))
                {
                    sqlCmd.Append(string.Format(" and orders.CustPONo >= '{0}'", this.txtPONoStart.Text));
                }

                if (!MyUtility.Check.Empty(this.txtPONoEnd.Text))
                {
                    sqlCmd.Append(string.Format(" and orders.CustPONo <= '{0}'", this.txtPONoEnd.Text));
                }
            }
            else
            {
                sqlCmd.Append(this.whereS);
            }
            #endregion
            sqlCmd.Append(@"
    ) d
)X 
order by rn");

            DualResult result;
            if (result = DBProxy.Current.Select(null, sqlCmd.ToString(), out this.gridData))
            {
                if (this.gridData.Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!");
                    return false;
                }
            }
            else
            {
                MyUtility.Msg.WarningBox("Sql connection fail!!\r\n" + result.ToString());
                return false;
            }

            this.listControlBindingSource1.DataSource = this.gridData;

            // 組Filter內容
            foreach (DataRow dr in this.gridData.Rows)
            {
                // Ctn#
                if (this.comboBox2_RowSource2.Contains(dr["CTNStartNo"].ToString()) == false)
                {
                    this.comboBox2_RowSource2.Add(dr["CTNStartNo"].ToString());
                }

                // Color Way
                if (this.comboBox2_RowSource3.Contains(dr["Article"].ToString()) == false)
                {
                    this.comboBox2_RowSource3.Add(dr["Article"].ToString());
                }

                // Color
                if (this.comboBox2_RowSource4.Contains(dr["Color"].ToString()) == false)
                {
                    this.comboBox2_RowSource4.Add(dr["Color"].ToString());
                }

                // Size
                if (this.comboBox2_RowSource5.Contains(dr["SizeCode"].ToString()) == false)
                {
                    this.comboBox2_RowSource5.Add(dr["SizeCode"].ToString());
                }
            }

            ((List<string>)this.comboBox2_RowSource2).Sort();
            ((List<string>)this.comboBox2_RowSource3).Sort();
            ((List<string>)this.comboBox2_RowSource4).Sort();
            ((List<string>)this.comboBox2_RowSource5).Sort();

            this.CalcCTNQty();
            return true;
        }

        private void CalcCTNQty()
        {
            if (this.listControlBindingSource1.DataSource != null)
            {
                this.gridPackID.ValidateControl();
                this.numTTLCTNQty.Value = ((DataTable)this.listControlBindingSource1.DataSource).Rows.Count;
                this.numSelectQty.Value = ((DataTable)this.listControlBindingSource1.DataSource).Select("selected = 1").Length;
            }
        }
    }
}
