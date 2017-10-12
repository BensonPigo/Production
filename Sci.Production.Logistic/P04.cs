using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Transactions;
using Ict;
using Ict.Win;
using Sci.Data;
using Sci;
using Sci.Win;
using System.Reflection;

namespace Sci.Production.Logistic
{
    public partial class P04 : Sci.Win.Tems.QueryForm
    {
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        Ict.Win.UI.DataGridViewTextBoxColumn col_location;
        DataTable gridData;
        IList<string> comboBox2_RowSource1 = new List<string>();
        IList<string> comboBox2_RowSource2 = new List<string>();
        IList<string> comboBox2_RowSource3 = new List<string>();
        IList<string> comboBox2_RowSource4 = new List<string>();
        IList<string> comboBox2_RowSource5 = new List<string>();
        IList<string> comboBox3_RowSource = new List<string>();
        BindingSource comboxbs1, comboxbs2, comboxbs3;
        string selectDataTable_DefaultView_Sort = "";

        public P04(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            Dictionary<String, String> comboBox1_RowSource = new Dictionary<string, string>();
            comboBox1_RowSource.Add("1", "");
            comboBox1_RowSource.Add("2", "Ctn#");
            comboBox1_RowSource.Add("3", "Colorway");
            comboBox1_RowSource.Add("4", "Color");
            comboBox1_RowSource.Add("5", "Size");
            comboxbs1 = new BindingSource(comboBox1_RowSource, null);
            comboFilter.DataSource = comboxbs1;
            comboFilter.ValueMember = "Key";
            comboFilter.DisplayMember = "Value";

            comboxbs2 = new BindingSource(comboBox2_RowSource1, null);
            comboFilter2.DataSource = comboxbs2;

            comboBox3_RowSource.Add("CFA");
            comboBox3_RowSource.Add("INSPECTOR");
            comboxbs3 = new BindingSource(comboBox3_RowSource, null);
            comboRequestby.DataSource = comboxbs3;


            //Grid設定
            this.gridPackID.IsEditingReadOnly = false;
            this.gridPackID.DataSource = listControlBindingSource1;

            this.gridPackID.CellValueChanged += (s, e) =>
            {
                if (gridPackID.Columns[e.ColumnIndex].DataPropertyName == col_location.DataPropertyName)
                {
                    DataRow dr = this.gridPackID.GetDataRow<DataRow>(e.RowIndex);
                    dr["Selected"] = 1;
                }
            };

            Ict.Win.DataGridViewGeneratorTextColumnSettings ts = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
            ts.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.gridPackID.GetDataRow<DataRow>(e.RowIndex);
                    if (e.FormattedValue.ToString() != dr["Remark"].ToString())
                    {
                        dr["Remark"] = e.FormattedValue.ToString();
                        dr["Selected"] = 1;
                    }
                }
            };

            Helper.Controls.Grid.Generator(this.gridPackID)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)
                //.Text("TransferToClogID", header: "Trans. Slip#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("ID", header: "Pack ID", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("OrderId", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("CustPONo", header: "P.O.#", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("CTNStartNo", header: "CTN#", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("Article", header: "Color Way", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Color", header: "Color", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("QtyPerCTN", header: "PC/Ctn", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("ShipQty", header: "Qty", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .CellClogLocation("ClogLocationId", header: "Location No", width: Widths.AnsiChars(10)).Get(out col_location)
                .Text("Remark", header: "Reamrks", width: Widths.AnsiChars(10), settings: ts);

            int RowIndex = 0;
            int ColumIndex = 0;
            gridPackID.CellClick += (s, e) =>
            {
                RowIndex = e.RowIndex;
                ColumIndex = e.ColumnIndex;
            };

            gridPackID.Sorted += (s, e) =>
            {

                if ((RowIndex == -1) & (ColumIndex == 4))
                {

                    listControlBindingSource1.DataSource = null;

                    if (selectDataTable_DefaultView_Sort == "DESC")
                    {
                        gridData.DefaultView.Sort = "rn1 DESC";
                        selectDataTable_DefaultView_Sort = "";
                    }
                    else
                    {
                        gridData.DefaultView.Sort = "rn1 ASC";
                        selectDataTable_DefaultView_Sort = "DESC";
                    }
                    listControlBindingSource1.DataSource = gridData;
                    return;
                }
            };
        }

        //Query
        private void btnQuery_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtSPNoStart.Text) && MyUtility.Check.Empty(this.txtSPNoEnd.Text)
                && MyUtility.Check.Empty(this.txtPackIDStart.Text) && MyUtility.Check.Empty(this.txtPackIDEnd.Text)
                && MyUtility.Check.Empty(this.txtPONoStart.Text) && MyUtility.Check.Empty(this.txtPONoEnd.Text)
                && MyUtility.Check.Empty(this.textTransferSlipNo.Text))
            {
                this.txtSPNoStart.Focus();
                MyUtility.Msg.WarningBox("< SP# > or < Pack ID > or < Transfer Clog No. > or < PO# > can not empty!");
                return;
            }

            StringBuilder sqlCmd = new StringBuilder();

            sqlCmd.Append(string.Format(@"
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
                 , PLD.ClogLocationId
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
          from TransferToClog TtClog
          inner join PackingList PL WITH (NOLOCK) on TtClog.PackingListID = PL.ID
          inner join PackingList_Detail PLD WITH (NOLOCK) on PL.ID = PLD.ID
          inner join Orders orders WITH (NOLOCK) on PLD.OrderID = orders.ID
                                                    and TtClog.OrderID = orders.ID
          where (PL.Type = 'B' or PL.Type = 'L')
                and PLD.ReceiveDate is not null
                and PLD.CTNQty = 1
                and orders.MDivisionID =  '{0}'", Sci.Env.User.Keyword));
            #region 組條件
            if (!MyUtility.Check.Empty(this.txtSPNoStart.Text))
            {
                sqlCmd.Append(string.Format(" and TtClog.OrderID >= '{0}'", this.txtSPNoStart.Text));
            }

            if (!MyUtility.Check.Empty(this.txtSPNoEnd.Text))
            {
                sqlCmd.Append(string.Format(" and TtClog.OrderID <= '{0}'", this.txtSPNoEnd.Text));
            }

            if (!MyUtility.Check.Empty(this.txtPackIDStart.Text))
            {
                sqlCmd.Append(string.Format(" and TtClog.PackingListID >= '{0}'", this.txtPackIDStart.Text));
            }

            if (!MyUtility.Check.Empty(this.txtPackIDEnd.Text))
            {
                sqlCmd.Append(string.Format(" and TtClog.PackingListID <= '{0}'", this.txtPackIDEnd.Text));
            }

            if (!MyUtility.Check.Empty(this.txtPONoStart.Text))
            {
                sqlCmd.Append(string.Format(" and orders.CustPONo >= '{0}'", this.txtPONoStart.Text));
            }

            if (!MyUtility.Check.Empty(this.txtPONoEnd.Text))
            {
                sqlCmd.Append(string.Format(" and orders.CustPONo <= '{0}'", this.txtPONoEnd.Text));
            }

            if (!MyUtility.Check.Empty(this.textTransferSlipNo.Text))
            {
                sqlCmd.Append(string.Format(" and TtClog.TransferSlipNo = '{0}'", this.textTransferSlipNo.Text));
            }
            #endregion
            sqlCmd.Append(@"
    ) d
)X 
order by rn");
            DualResult result;
            if (result = DBProxy.Current.Select(null, sqlCmd.ToString(), out gridData))
            {
                if (gridData.Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!");
                }
            }
            else
            {
                MyUtility.Msg.WarningBox("Sql connection fail!!\r\n" + result.ToString());
                return;
            }
            listControlBindingSource1.DataSource = gridData;

            //組Filter內容
            foreach (DataRow dr in gridData.Rows)
            {
                //Ctn#
                if (comboBox2_RowSource2.Contains(dr["CTNStartNo"].ToString()) == false)
                {
                    comboBox2_RowSource2.Add(dr["CTNStartNo"].ToString());
                }

                //Color Way
                if (comboBox2_RowSource3.Contains(dr["Article"].ToString()) == false)
                {
                    comboBox2_RowSource3.Add(dr["Article"].ToString());
                }

                //Color
                if (comboBox2_RowSource4.Contains(dr["Color"].ToString()) == false)
                {
                    comboBox2_RowSource4.Add(dr["Color"].ToString());
                }

                //Size
                if (comboBox2_RowSource5.Contains(dr["SizeCode"].ToString()) == false)
                {
                    comboBox2_RowSource5.Add(dr["SizeCode"].ToString());
                }
            }
            ((List<string>)comboBox2_RowSource2).Sort();
            ((List<string>)comboBox2_RowSource3).Sort();
            ((List<string>)comboBox2_RowSource4).Sort();
            ((List<string>)comboBox2_RowSource5).Sort();
        }

        //Update All Location
        private void btnUpdateAllLocation_Click(object sender, EventArgs e)
        {
            string location = this.txtcloglocationLocationNo.Text.Trim();
            int pos = this.listControlBindingSource1.Position;
            DataTable dt = (DataTable)listControlBindingSource1.DataSource;
            if (MyUtility.Check.Empty(dt))
            {
                MyUtility.Msg.WarningBox("Please select data first!");
                return;
            }
            foreach (DataRow currentRecord in dt.Rows)
            {
                if (currentRecord["selected"].ToString() == "1")
                {
                    currentRecord["ClogLocationId"] = location;
                }
            }
            this.listControlBindingSource1.Position = pos;
            gridPackID.SuspendLayout();
            this.gridPackID.DataSource = null;
            this.gridPackID.DataSource = listControlBindingSource1;
            this.listControlBindingSource1.Position = pos;
            gridPackID.ResumeLayout();
        }

        //Save
        private void btnSave_Click(object sender, EventArgs e)
        {
            this.gridPackID.ValidateControl();
            this.gridPackID.EndEdit();
            listControlBindingSource1.EndEdit();
            DataTable detailData = (DataTable)listControlBindingSource1.DataSource;
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
                    //只存畫面上看到的那幾筆資料
                    foreach (DataRowView currentRowView in gridData.DefaultView)
                    {
                        DataRow currentRow = currentRowView.Row;
                        if (currentRow["Selected"].ToString() == "1")
                        {
                            //BUG FIX:512: LOGISTIC_P04_Update Location，存檔出現失敗訊息
                            //string updateCmd = @"update ClogReceive_Detail 
                            //                                    set ClogLocationId = @clogLocationId 
                            //                                    where ID = @clogReceiveID and PackingListId = @id and OrderId = @orderId and CTNStartNo = @ctnStartNo;
                            //                              update PackingList_Detail 
                            //                                    set ClogLocationId = @clogLocationId, Remark = @remark 
                            //                                    where id = @id and CTNStartNo = @ctnStartNo;";
                            string updateCmd = @"update PackingList_Detail 
                                                set ClogLocationId = @clogLocationId, Remark = @remark 
                                                where id = @id and CTNStartNo = @ctnStartNo;";

                            #region 準備sql參數資料
                            System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
                            sp1.ParameterName = "@clogLocationId";
                            sp1.Value = currentRow["ClogLocationId"].ToString();

                            //System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter();
                            //sp2.ParameterName = "@clogReceiveID";
                            //sp2.Value = currentRow["ClogReceiveID"].ToString();

                            System.Data.SqlClient.SqlParameter sp3 = new System.Data.SqlClient.SqlParameter();
                            sp3.ParameterName = "@id";
                            sp3.Value = currentRow["ID"].ToString();

                            System.Data.SqlClient.SqlParameter sp4 = new System.Data.SqlClient.SqlParameter();
                            sp4.ParameterName = "@orderId";
                            sp4.Value = currentRow["OrderId"].ToString();

                            System.Data.SqlClient.SqlParameter sp5 = new System.Data.SqlClient.SqlParameter();
                            sp5.ParameterName = "@ctnStartNo";
                            sp5.Value = currentRow["CTNStartNo"].ToString();

                            System.Data.SqlClient.SqlParameter sp6 = new System.Data.SqlClient.SqlParameter();
                            sp6.ParameterName = "@remark";
                            sp6.Value = currentRow["Remark"].ToString();

                            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                            cmds.Add(sp1);
                            //cmds.Add(sp2);
                            cmds.Add(sp3);
                            cmds.Add(sp4);
                            cmds.Add(sp5);
                            cmds.Add(sp6);
                            #endregion
                            DualResult result = Sci.Data.DBProxy.Current.Execute(null, updateCmd, cmds);
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
                    ShowErr("Commit transaction error.", ex);
                    return;
                }
            }
            #endregion

            //更新成功後，將畫面上的資料都清空
            this.txtSPNoStart.Text = "";
            this.txtSPNoEnd.Text = "";
            this.txtPackIDStart.Text = "";
            this.txtPackIDEnd.Text = "";
            this.txtPONoStart.Text = "";
            this.txtPONoEnd.Text = "";
            this.comboFilter.SelectedValue = "1";
            this.comboFilter2.SelectedIndex = -1;

            string sqlCmd = @"select 0 as selected,  b.ID, b.OrderID, c.CustPONo, b.CTNStartNo, b.Article, b.Color, b.SizeCode, b.QtyPerCTN, b.ShipQty, b.ClogLocationId, b.Remark
                                           from PackingList a WITH (NOLOCK) , PackingList_Detail b WITH (NOLOCK) , Orders c WITH (NOLOCK) where 1=0";
            DualResult result1 = DBProxy.Current.Select(null, sqlCmd, out gridData);

            listControlBindingSource1.DataSource = gridData;
        }

        //Cancel
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Filter
        private void comboFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboFilter.SelectedIndex != -1)
            {
                this.comboFilter2.Enabled = true;
                switch (comboFilter.SelectedValue.ToString())
                {
                    case "1":
                        this.comboFilter2.Enabled = false;
                        comboFilter2.DataSource = comboBox2_RowSource1;
                        if (gridData != null)
                        {
                            gridData.DefaultView.RowFilter = "";
                        }
                        break;
                    case "2":
                        comboFilter2.DataSource = comboBox2_RowSource2;
                        comboFilter2.SelectedIndex = -1;
                        if (gridData != null)
                        {
                            gridData.DefaultView.RowFilter = "";
                        }
                        break;
                    case "3":
                        comboFilter2.DataSource = comboBox2_RowSource3;
                        comboFilter2.SelectedIndex = -1;
                        {
                            gridData.DefaultView.RowFilter = "";
                        }
                        break;
                    case "4":
                        comboFilter2.DataSource = comboBox2_RowSource4;
                        comboFilter2.SelectedIndex = -1;
                        {
                            gridData.DefaultView.RowFilter = "";
                        }
                        break;
                    case "5":
                        comboFilter2.DataSource = comboBox2_RowSource5;
                        comboFilter2.SelectedIndex = -1;
                        {
                            gridData.DefaultView.RowFilter = "";
                        }
                        break;
                    default:
                        this.comboFilter2.Enabled = false;
                        comboFilter2.DataSource = comboBox2_RowSource1;
                        break;
                }
            }
        }

        //第二個Filter
        private void comboFilter2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboFilter2.SelectedIndex != -1)
            {
                switch (comboFilter.SelectedValue.ToString())
                {
                    case "1":
                        break;
                    case "2":
                        gridData.DefaultView.RowFilter = "CTNStartNo = '" + comboFilter2.SelectedValue.ToString() + "'";
                        break;
                    case "3":
                        comboFilter2.DataSource = comboBox2_RowSource3;
                        gridData.DefaultView.RowFilter = "Article  = '" + comboFilter2.SelectedValue.ToString() + "'";
                        break;
                    case "4":
                        comboFilter2.DataSource = comboBox2_RowSource4;
                        gridData.DefaultView.RowFilter = "Color = '" + comboFilter2.SelectedValue.ToString() + "'";
                        break;
                    case "5":
                        comboFilter2.DataSource = comboBox2_RowSource5;
                        gridData.DefaultView.RowFilter = "SizeCode = '" + comboFilter2.SelectedValue.ToString() + "'";
                        break;
                    default:
                        comboFilter2.DataSource = comboBox2_RowSource1;
                        gridData.DefaultView.RowFilter = "";
                        break;
                }
            }
        }

        //Print Move Ticket
        private void btnPrintMoveTicket_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(listControlBindingSource1.DataSource))
            {
                MyUtility.Msg.WarningBox("Please select data first!");
                return;
            }
            DualResult result;
            IReportResource reportresource;
            ReportDefinition rd = new ReportDefinition();
            if (!(result = ReportResources.ByEmbeddedResource(Assembly.GetAssembly(GetType()), GetType(), "P04_PrintMoveTicket.rdlc", out reportresource)))
            {
                MyUtility.Msg.ErrorBox(result.ToString());
            }
            else
            {
                DataTable Report_UpdateLocation;
                try
                {
                    MyUtility.Tool.ProcessWithDatatable((DataTable)listControlBindingSource1.DataSource, "Selected,OrderId,CustPONo,BrandID,StyleID,BuyerDelivery,Dest,FtyGroup,TotalCTN,ClogCTN,TransferPerCent,CTNStartNo,Article,Color,SizeCode,ShipQty,ClogLocationId,Remark", "select * from #tmp where Selected = 1 order by OrderID, CTNStartNo", out Report_UpdateLocation);
                }
                catch (Exception ex)
                {
                    MyUtility.Msg.ErrorBox("Prepare data error.\r\n" + ex.ToString());
                    return;
                }

                rd.ReportResource = reportresource;
                rd.ReportDataSources.Add(new System.Collections.Generic.KeyValuePair<string, object>("Report_UpdateLocation", Report_UpdateLocation));
                rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("title", MyUtility.GetValue.Lookup(string.Format("select NameEN from Factory WITH (NOLOCK) where ID = '{0}'", Sci.Env.User.Keyword))));
                rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("request", MyUtility.Convert.GetString(comboRequestby.SelectedValue)));

                using (var frm = new Sci.Win.Subs.ReportView(rd))
                {
                    frm.ShowDialog(this);
                }
            }
        }

    }
}
