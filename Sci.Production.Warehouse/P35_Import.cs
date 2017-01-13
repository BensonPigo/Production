using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci;
using Sci.Data;
using Sci.Production.PublicPrg;

namespace Sci.Production.Warehouse
{
    public partial class P35_Import : Sci.Win.Subs.Base
    {
        DataRow dr_master;
        DataTable dt_detail;
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        protected DataTable dtInventory;

        public P35_Import(DataRow master, DataTable detail)
        {
            InitializeComponent();
            dr_master = master;
            dt_detail = detail;
        }

        //Find Now Button
        private void button1_Click(object sender, EventArgs e)
        {
            StringBuilder strSQLCmd = new StringBuilder();
            String sp = this.textBox1.Text.TrimEnd();
            String refno = this.textBox3.Text.TrimEnd();
            String location = this.textBox4.Text.TrimEnd();
            string fabrictype = txtdropdownlist1.SelectedValue.ToString();

            if (string.IsNullOrWhiteSpace(sp) && string.IsNullOrWhiteSpace(refno) && string.IsNullOrWhiteSpace(location))
            {
                MyUtility.Msg.WarningBox("< SP# > < Ref# > < Location > can't be empty!!");
                textBox1.Focus();
                return;
            }

            else
            {
                // 建立可以符合回傳的Cursor

                strSQLCmd.Append(string.Format(@"select 0 as selected 
,'' id, c.mdivisionid,c.PoId,a.Seq1,a.Seq2
,concat(Ltrim(Rtrim(a.seq1)), ' ', a.Seq2) as seq
,dbo.getmtldesc(a.id,a.seq1,a.seq2,2,0) as [Description]
,c.Roll
,c.Dyelot
,c.inqty-c.outqty + c.adjustqty as QtyBefore
,0.00 as QtyAfter
,isnull(stuff((select ',' + cast(mtllocationid as varchar) from (select mtllocationid from ftyinventory_detail where ukey = c.ukey)t for xml path('')), 1, 1, ''),'') as location
,'' reasonid
,'' reason_nm
,a.FabricType
,a.stockunit
,c.stockType
,c.ukey as ftyinventoryukey
from dbo.PO_Supp_Detail a 
inner join dbo.ftyinventory c on c.poid = a.id and c.seq1 = a.seq1 and c.seq2  = a.seq2 and c.stocktype = 'B'
Where c.lock = 0 and c.mdivisionid='{0}'", Sci.Env.User.Keyword));

                if (!MyUtility.Check.Empty(sp))
                {
                    strSQLCmd.Append(string.Format(@" and a.id = '{0}' ", sp));
                }

                if (!MyUtility.Check.Empty(refno))
                {
                    strSQLCmd.Append(string.Format(@" and a.refno = '{0}' ", refno));
                }

                if (!MyUtility.Check.Empty(location))
                {
                    strSQLCmd.Append(string.Format(@" and c.ukey in (select ukey from dbo.ftyinventory_detail where mtllocationid = '{0}') ", location));
                }

                if (!txtSeq1.checkEmpty(showErrMsg: false))
                {
                    strSQLCmd.Append(string.Format(@" and a.seq1 = '{0}' and a.seq2='{1}'", txtSeq1.seq1, txtSeq1.seq2));
                }

                switch (fabrictype)
                {
                    case "ALL":
                        break;
                    case "F":
                        strSQLCmd.Append(@" And a.fabrictype = 'F'");
                        break;
                    case "A":
                        strSQLCmd.Append(@" And a.fabrictype = 'A'");
                        break;
                }


                this.ShowWaitMessage("Data Loading....");
                Ict.DualResult result;
                if (result = DBProxy.Current.Select(null, strSQLCmd.ToString(), out dtInventory))
                {
                    if (dtInventory.Rows.Count == 0)
                    { MyUtility.Msg.WarningBox("Data not found!!"); }
                    else
                    {
                        dtInventory.Columns.Add("adjustqty", typeof(decimal));
                        dtInventory.Columns["adjustqty"].Expression = "qtyafter - qtybefore";
                        dtInventory.DefaultView.Sort = "seq1,seq2,location,dyelot";
                    }
                    listControlBindingSource1.DataSource = dtInventory;
                }
                else { ShowErr(strSQLCmd.ToString(), result); }
                this.HideWaitMessage();
            }
        }
        //Form Load
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            #region -- Reason Combox --
            string selectCommand = @"select Name idname,id from Reason where ReasonTypeID='Stock_Adjust' AND junk = 0";
            Ict.DualResult returnResult;
            DataTable dropDownListTable = new DataTable();
            if (returnResult = DBProxy.Current.Select(null, selectCommand, out dropDownListTable))
            {
                comboBox2.DataSource = dropDownListTable;
                comboBox2.DisplayMember = "IDName";
                comboBox2.ValueMember = "ID";
            }
            #endregion
            #region -- Current Qty Valid --
            Ict.Win.DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            ns.CellValidating += (s, e) =>
                {
                    if (this.EditMode && !MyUtility.Check.Empty(e.FormattedValue))
                    {
                        grid1.GetDataRow(grid1.GetSelectedRowIndex())["qtyafter"] = e.FormattedValue;
                        grid1.GetDataRow(grid1.GetSelectedRowIndex())["selected"] = true;
                    }
                };
            #endregion
            #region -- Reason ID 右鍵開窗 --
            Ict.Win.DataGridViewGeneratorTextColumnSettings ts = new DataGridViewGeneratorTextColumnSettings();
            ts.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    DataTable poitems;
                    string sqlcmd = "";
                    IList<DataRow> x;

                    sqlcmd = @"select id, Name from Reason where ReasonTypeID='Stock_Adjust' AND junk = 0";
                    DualResult result2 = DBProxy.Current.Select(null, sqlcmd, out poitems);
                    if (!result2)
                    {
                        ShowErr(sqlcmd, result2);
                        return;
                    }

                    Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(poitems
                        , "ID,Name"
                        , "5,150"
                        , grid1.GetDataRow(grid1.GetSelectedRowIndex())["reasonid"].ToString()
                        , "ID,Name");
                    item.Width = 600;
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    x = item.GetSelecteds();

                    grid1.GetDataRow(grid1.GetSelectedRowIndex())["reasonid"] = x[0]["id"];
                    grid1.GetDataRow(grid1.GetSelectedRowIndex())["reason_nm"] = x[0]["name"];
                }
            };
            ts.CellValidating += (s, e) =>
            {
                DataRow dr;
                if (!this.EditMode) return;
                if (String.Compare(e.FormattedValue.ToString(), grid1.GetDataRow(grid1.GetSelectedRowIndex())["reasonid"].ToString()) != 0)
                {
                    if (MyUtility.Check.Empty(e.FormattedValue))
                    {
                        grid1.GetDataRow(grid1.GetSelectedRowIndex())["reasonid"] = "";
                        grid1.GetDataRow(grid1.GetSelectedRowIndex())["reason_nm"] = "";
                    }
                    else
                    {
                        if (!MyUtility.Check.Seek(string.Format(@"select id, Name from Reason where id = '{0}' 
and ReasonTypeID='Stock_Adjust' AND junk = 0", e.FormattedValue), out dr, null))
                        {
                            MyUtility.Msg.WarningBox("Data not found!", "Reason ID");
                            e.Cancel = true;
                            return;
                        }
                        else
                        {
                            grid1.GetDataRow(grid1.GetSelectedRowIndex())["reasonid"] = e.FormattedValue;
                            grid1.GetDataRow(grid1.GetSelectedRowIndex())["reason_nm"] = dr["name"];
                        }
                    }
                }
            };
            #endregion

            this.grid1.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.grid1.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.grid1)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)   //0
                .Text("poid", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(14)) //1
                .Text("seq", header: "Seq#", iseditingreadonly: true, width: Widths.AnsiChars(6)) //2
                .EditText("Description", header: "Description", iseditingreadonly: true, width: Widths.AnsiChars(20)) //3
                .Text("roll", header: "Roll", iseditingreadonly: true, width: Widths.AnsiChars(6)) //4
                .Text("dyelot", header: "Dyelot", iseditingreadonly: true, width: Widths.AnsiChars(6)) //5
                .Numeric("QtyBefore", header: "Stock Qty", iseditable: true, decimal_places: 2, integer_places: 10, width: Widths.AnsiChars(6)) //6
                .Numeric("QtyAfter", header: "Current Qty", decimal_places: 2, integer_places: 10, settings: ns, width: Widths.AnsiChars(6))  //7
                .Numeric("adjustqty", header: "Adjust Qty", decimal_places: 2, integer_places: 10, width: Widths.AnsiChars(6))  //8
                .Text("location", header: "Bulk Location", iseditingreadonly: true, width: Widths.AnsiChars(6))      //9
                .Text("reasonid", header: "Reason ID", settings: ts, width: Widths.AnsiChars(6))    //10
                .Text("reason_nm", header: "Reason Name", iseditingreadonly: true, width: Widths.AnsiChars(20))      //11
               ;

            this.grid1.Columns[7].DefaultCellStyle.BackColor = Color.Pink;
            this.grid1.Columns[10].DefaultCellStyle.BackColor = Color.Pink;
        }
        //Close
        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //Import
        private void button2_Click(object sender, EventArgs e)
        {
            grid1.ValidateControl();
            DataTable dtGridBS1 = (DataTable)listControlBindingSource1.DataSource;
            if (MyUtility.Check.Empty(dtGridBS1) || dtGridBS1.Rows.Count == 0) return;

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
            if (dr2.Length > 0)
            {
                MyUtility.Msg.WarningBox("Reason ID of selected row can't be empty!", "Warning");
                return;
            }

            dr2 = dtGridBS1.Select("adjustqty <> 0 and Selected = 1");
            foreach (DataRow tmp in dr2)
            {
                DataRow[] findrow = dt_detail.Select(string.Format("ftyinventoryukey = {0} ", tmp["ftyinventoryukey"]));

                if (findrow.Length > 0)
                {
                    findrow[0]["qtybefore"] = tmp["qtybefore"];
                    findrow[0]["qtyafter"] = tmp["qtyafter"];
                    findrow[0]["adjustqty"] = tmp["adjustqty"];
                }
                else
                {
                    tmp["id"] = dr_master["id"];
                    tmp.AcceptChanges();
                    tmp.SetAdded();
                    dt_detail.ImportRow(tmp);
                }
            }


            this.Close();
        }

        //SP# Valid
        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            string sp = textBox1.Text.TrimEnd();

            if (MyUtility.Check.Empty(sp)) return;

            if (txtSeq1.checkEmpty(showErrMsg: false))
            {
                if (!MyUtility.Check.Seek(string.Format("select 1 where exists(select * from MdivisionPodetail where poid ='{0}' and mdivisionid='{1}')"
                    , sp, Sci.Env.User.Keyword), null))
                {
                    MyUtility.Msg.WarningBox("SP# is not found!!");
                    e.Cancel = true;
                    return;
                }
            }
            else
            {
                if (!MyUtility.Check.Seek(string.Format(@"select 1 where exists(select * from MdivisionPodetail where poid ='{0}' ' 
                        and seq1 = '{1}' and seq2 = '{2}' and mdivisionid='{3}')", sp, txtSeq1.seq1, txtSeq1.seq2, Sci.Env.User.Keyword), null))
                {
                    MyUtility.Msg.WarningBox("SP#-Seq is not found!!");
                    e.Cancel = true;
                    return;
                }
            }

        }

        //Seq Valid
        private void txtSeq1_Leave(object sender, EventArgs e)
        {
            string sp = textBox1.Text.TrimEnd();
            if (MyUtility.Check.Empty(sp) || txtSeq1.checkEmpty(showErrMsg: false)) return;

            if (!MyUtility.Check.Seek(string.Format(@"select 1 where exists(select * from MdivisionPodetail where poid ='{0}' 
                        and seq1 = '{1}' and seq2 = '{2}' and mdivisionid = '{3}')", sp, txtSeq1.seq1, txtSeq1.seq2, Sci.Env.User.Keyword), null))
            {
                MyUtility.Msg.WarningBox("SP#-Seq is not found!!");
                return;
            }
        }

        //Location  右鍵
        private void textBox4_MouseDown(object sender, MouseEventArgs e)
        {
            #region Location 右鍵開窗


            if (this.EditMode && e.Button == MouseButtons.Right)
            {

                Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(string.Format(@"select id,[Description] from dbo.MtlLocation where StockType='B' and mdivisionid='{0}'", Sci.Env.User.Keyword), "10,40", textBox4.Text, "ID,Desc");
                DialogResult result = item.ShowDialog();
                if (result == DialogResult.Cancel) { return; }
                textBox4.Text = item.GetSelectedString();
            }


            #endregion Location 右鍵開窗
        }

        //Update All
        private void button4_Click(object sender, EventArgs e)
        {
            string reasonid = comboBox2.SelectedValue.ToString();
            grid1.ValidateControl();

            if (dtInventory == null || dtInventory.Rows.Count == 0) return;
            DataRow[] drfound = dtInventory.Select("selected = 1");

            foreach (var item in drfound)
            {
                item["reasonid"] = reasonid;
                item["reason_nm"] = comboBox2.Text;
            }
        }

        private void textBox4_Validating(object sender, CancelEventArgs e)
        {
            if (textBox4.Text.ToString() == "") return;
            if (!MyUtility.Check.Seek(string.Format("select 1 where exists(select * from dbo.MtlLocation where StockType='B' and id = '{0}' and mdivisionid='{1}')", textBox4.Text, Sci.Env.User.Keyword), null))
            {
                MyUtility.Msg.WarningBox("Location is not exist!!", "Data not found");
                e.Cancel = true;
            }
        }
    }
}
