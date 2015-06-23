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
            comboBox1.DataSource = comboxbs1;
            comboBox1.ValueMember = "Key";
            comboBox1.DisplayMember = "Value";

            //comboBox2_RowSource1.Add("");
            comboxbs2 = new BindingSource(comboBox2_RowSource1, null);
            comboBox2.DataSource = comboxbs2;

            comboBox3_RowSource.Add("CFA");
            comboBox3_RowSource.Add("INSPECTOR");
            comboxbs3 = new BindingSource(comboBox3_RowSource, null);
            comboBox3.DataSource = comboxbs3;


            //Grid設定
            this.grid1.IsEditingReadOnly = false;
            this.grid1.DataSource = listControlBindingSource1;

            this.grid1.CellValueChanged += (s, e) =>
            {
                if (grid1.Columns[e.ColumnIndex].Name == col_location.Name)
                {
                    DataRow dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                    dr["Selected"] = 1;
                }
            };

            Ict.Win.DataGridViewGeneratorTextColumnSettings ts = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
            ts.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                    if (e.FormattedValue.ToString() != dr["Remark"].ToString())
                    {
                        dr["Remark"] = e.FormattedValue.ToString();
                        dr["Selected"] = 1;
                    }
                }
            };

            Helper.Controls.Grid.Generator(this.grid1)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)
                .Text("TransferToClogID", header: "Trans. Slip#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("ID", header: "Pack ID", width: Widths.AnsiChars(13), iseditingreadonly: true)
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
        }

        //Query
        private void button1_Click(object sender, EventArgs e)
        {
            if (myUtility.Empty(this.textBox1.Text) && myUtility.Empty(this.textBox2.Text) && myUtility.Empty(this.textBox3.Text) && myUtility.Empty(this.textBox4.Text) && myUtility.Empty(this.textBox5.Text) && myUtility.Empty(this.textBox6.Text) && myUtility.Empty(this.textBox7.Text) && myUtility.Empty(this.textBox8.Text))
            {
                MessageBox.Show("< SP# > or < Pack ID > or < Transfer Clog No. > or < PO# > can not empty!");
                this.textBox1.Focus();
                return;
            }

            string sqlCmd = @"select 0 as selected, d.*,
	                                        (select sum(QtyPerCTN) from PackingList_Detail where ID = d.ID and CTNStartNo = d.CTNStartNo) as QtyPerCTN,
	                                        (select sum(ShipQty) from PackingList_Detail where ID = d.ID and CTNStartNo = d.CTNStartNo) as ShipQty,
	                                        substring((select cast(Article as nvarchar) + ',' from PackingList_Detail where ID = d.ID and CTNStartNo = d.CTNStartNo for xml path('')),1,len((select cast(Article as nvarchar) + ',' from PackingList_Detail where ID = d.ID and CTNStartNo = d.CTNStartNo for xml path('')))-1) as Article, 
	                                        substring((select cast(Color as nvarchar) + ',' from PackingList_Detail where ID = d.ID and CTNStartNo = d.CTNStartNo for xml path('')),1,len((select cast(Color as nvarchar) + ',' from PackingList_Detail where ID = d.ID and CTNStartNo = d.CTNStartNo for xml path('')))-1) as Color,
	                                        substring((select cast(SizeCode as nvarchar) + ',' from PackingList_Detail where ID = d.ID and CTNStartNo = d.CTNStartNo for xml path('')),1,len((select cast(SizeCode as nvarchar) + ',' from PackingList_Detail where ID = d.ID and CTNStartNo = d.CTNStartNo for xml path('')))-1) as SizeCode
                                           from (
                                                        select distinct b.TransferToClogID, b.ID, b.OrderID, c.CustPONo, b.CTNStartNo, b.ClogLocationId, b.Remark, b.ClogReceiveID
                                                        from PackingList a, PackingList_Detail b, Orders c
                                                        where (a.Type = 'B' or a.Type = 'L')
                                                        and a.ID = b.ID
                                                        and b.ReceiveDate is not null
                                                        and b.CTNQty = 1
                                                        and b.OrderID = c.ID";
            #region 組條件
            if (!myUtility.Empty(this.textBox1.Text))
            {
                sqlCmd = sqlCmd + string.Format(" and c.ID >= '{0}'", this.textBox1.Text);
            }
            if (!myUtility.Empty(this.textBox2.Text))
            {
                sqlCmd = sqlCmd + string.Format(" and c.ID <= '{0}'", this.textBox2.Text);
            }

            if (!myUtility.Empty(this.textBox3.Text))
            {
                sqlCmd = sqlCmd + string.Format(" and a.ID >= '{0}'", this.textBox3.Text);
            }
            if (!myUtility.Empty(this.textBox4.Text))
            {
                sqlCmd = sqlCmd + string.Format(" and a.ID <= '{0}'", this.textBox4.Text);
            }

            if (!myUtility.Empty(this.textBox5.Text))
            {
                sqlCmd = sqlCmd + string.Format(" and b.TransferToClogID >= '{0}'", this.textBox5.Text);
            }
            if (!myUtility.Empty(this.textBox6.Text))
            {
                sqlCmd = sqlCmd + string.Format(" and b.TransferToClogID <= '{0}'", this.textBox6.Text);
            }

            if (!myUtility.Empty(this.textBox7.Text))
            {
                sqlCmd = sqlCmd + string.Format(" and c.CustPONo >= '{0}'", this.textBox7.Text);
            }
            if (!myUtility.Empty(this.textBox8.Text))
            {
                sqlCmd = sqlCmd + string.Format(" and c.CustPONo <= '{0}'", this.textBox8.Text);
            }
            #endregion
            sqlCmd = sqlCmd + ") d";
            DualResult result;
            if (result = DBProxy.Current.Select(null, sqlCmd, out gridData))
            {
                if (gridData.Rows.Count == 0)
                {
                    MessageBox.Show("Data not found!");
                }
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
        private void button2_Click(object sender, EventArgs e)
        {
            string location = this.txtcloglocation1.Text.Trim();
            int pos = this.listControlBindingSource1.Position;
            DataTable dt = (DataTable)listControlBindingSource1.DataSource;
            foreach (DataRow currentRecord in dt.Rows)
            {
                if (currentRecord["selected"].ToString() == "1")
                {
                    currentRecord["ClogLocationId"] = location;
                }
            }
            this.listControlBindingSource1.Position = pos;
            grid1.SuspendLayout();
            this.grid1.DataSource = null;
            this.grid1.DataSource = listControlBindingSource1;
            this.listControlBindingSource1.Position = pos;
            grid1.ResumeLayout();
        }

        //Save
        private void button4_Click(object sender, EventArgs e)
        {
            this.grid1.ValidateControl();
            this.grid1.EndEdit();
            listControlBindingSource1.EndEdit();
            DataTable detailData = (DataTable)listControlBindingSource1.DataSource;
            DataRow[] dr = detailData.Select("Selected = 1");
            if (dr.Length <= 0)
            {
                MessageBox.Show("Please select at least one data!");
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
                            string updateCmd = @"update ClogReceive_Detail 
                                                                set ClogLocationId = @clogLocationId 
                                                                where ID = @clogReceiveID and PackingListId = @id and OrderId = @orderId and CTNStartNo = @ctnStartNo;
                                                          update PackingList_Detail 
                                                                set ClogLocationId = @clogLocationId, Remark = @remark 
                                                                where id = @id and CTNStartNo = @ctnStartNo;";
                            #region 準備sql參數資料
                            System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
                            sp1.ParameterName = "@clogLocationId";
                            sp1.Value = currentRow["ClogLocationId"].ToString();

                            System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter();
                            sp2.ParameterName = "@clogReceiveID";
                            sp2.Value = currentRow["ClogReceiveID"].ToString();

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
                            cmds.Add(sp2);
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
                        MessageBox.Show("Save failed, Pleaes re-try");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    ShowErr("Commit transaction error.", ex);
                    return;
                }
            }
            #endregion

            //更新成功後，將畫面上的資料都清空
            this.textBox1.Text = "";
            this.textBox2.Text = "";
            this.textBox3.Text = "";
            this.textBox4.Text = "";
            this.textBox5.Text = "";
            this.textBox6.Text = "";
            this.textBox7.Text = "";
            this.textBox8.Text = "";
            this.comboBox1.SelectedValue = "1";
            this.comboBox2.SelectedIndex = -1;

            string sqlCmd = @"select 0 as selected, b.TransferToClogID, b.ID, b.OrderID, c.CustPONo, b.CTNStartNo, b.Article, b.Color, b.SizeCode, b.QtyPerCTN, b.ShipQty, b.ClogLocationId, b.Remark, b.ClogReceiveID
                                           from PackingList a, PackingList_Detail b, Orders c where 1=0";
            DualResult result1 = DBProxy.Current.Select(null, sqlCmd, out gridData);
          
            listControlBindingSource1.DataSource = gridData;
        }

        //Cancel
        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Filter
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != -1)
            {
                this.comboBox2.Enabled = true;
                switch (comboBox1.SelectedValue.ToString())
                {
                    case "1":
                        this.comboBox2.Enabled = false;
                        comboBox2.DataSource = comboBox2_RowSource1;
                        if (gridData != null)
                        {
                            gridData.DefaultView.RowFilter = "";
                        }
                        break;
                    case "2":
                        comboBox2.DataSource = comboBox2_RowSource2;
                        comboBox2.SelectedIndex = -1;
                        gridData.DefaultView.RowFilter = "";
                        break;
                    case "3":
                        comboBox2.DataSource = comboBox2_RowSource3;
                        comboBox2.SelectedIndex = -1;
                        gridData.DefaultView.RowFilter = "";
                        break;
                    case "4":
                        comboBox2.DataSource = comboBox2_RowSource4;
                        comboBox2.SelectedIndex = -1;
                        gridData.DefaultView.RowFilter = "";
                        break;
                    case "5":
                        comboBox2.DataSource = comboBox2_RowSource5;
                        comboBox2.SelectedIndex = -1;
                        gridData.DefaultView.RowFilter = "";
                        break;
                    default:
                        this.comboBox2.Enabled = false;
                        comboBox2.DataSource = comboBox2_RowSource1;
                        break;
                }
            }
        }

        //第二個Filter
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex != -1)
            {
                switch (comboBox1.SelectedValue.ToString())
                {
                    case "1":
                        break;
                    case "2":
                        gridData.DefaultView.RowFilter = "CTNStartNo = '"+comboBox2.SelectedValue.ToString() +"'";
                        break;
                    case "3":
                        comboBox2.DataSource = comboBox2_RowSource3;
                        gridData.DefaultView.RowFilter = "Article  = '" + comboBox2.SelectedValue.ToString() + "'";
                        break;
                    case "4":
                        comboBox2.DataSource = comboBox2_RowSource4;
                        gridData.DefaultView.RowFilter = "Color = '" + comboBox2.SelectedValue.ToString() + "'";
                        break;
                    case "5":
                        comboBox2.DataSource = comboBox2_RowSource5;
                        gridData.DefaultView.RowFilter = "SizeCode = '" + comboBox2.SelectedValue.ToString() + "'";
                        break;
                    default:
                        comboBox2.DataSource = comboBox2_RowSource1;
                        gridData.DefaultView.RowFilter = "'";
                        break;
                }
            }
        }

    }
}
