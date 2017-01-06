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

namespace Sci.Production.Warehouse
{
    public partial class P16_Import : Sci.Win.Subs.Base
    {
        DataRow dr_master;
        DataTable dt_detail;
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        bool flag;
        string poType;
        protected DataTable dtlack, dtftyinventory;
        string Type;
        public P16_Import(DataRow master, DataTable detail,string type, string title)
        {
            this.Text = title.ToString();
            InitializeComponent();
            dr_master = master;
            dt_detail = detail;
            Type = type;
        }

        private void sum_checkedqty()
        {
            listControlBindingSource2.EndEdit();
            Object localPrice = dtftyinventory.Compute("Sum(qty)", "selected = 1");
            this.displayBox1.Value = localPrice.ToString();
        }
        private void grid1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            grid2.ValidateControl();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            StringBuilder strSQLCmd = new StringBuilder();

            #region -- 抓lack的資料 --
            //grid1
            strSQLCmd.Append(string.Format(@"select rtrim(a.POID) poid,b.seq1,b.seq2,left(b.seq1+' ',3)+b.Seq2 as seq
,dbo.getMtlDesc(a.poid,b.seq1,b.seq2,2,0) as [description]
,b.RequestQty
from dbo.lack a inner join dbo.Lack_Detail b on a.ID = b.ID
where a.id = '{0}';", dr_master["requestid"]));
            strSQLCmd.Append(Environment.NewLine); // 換行
            //grid2
            strSQLCmd.Append(string.Format(@"select 0 as selected ,'' id
,rtrim(c.PoId) poid,c.Seq1,c.Seq2,left(c.seq1+' ',3)+c.Seq2 as seq
,c.Roll
,c.Dyelot
,0.00 as Qty
,'B' StockType
,c.ukey as ftyinventoryukey
,isnull(stuff((select ',' + cast(MtlLocationid as varchar) from (select MtlLocationid from FtyInventory_Detail where ukey=c.Ukey) t for xml path('')), 1, 1, ''),'') as location
,c.inqty-c.outqty + c.adjustqty as balance
,(select stockunit from po_supp_detail where id = c.poid and seq1 =c.seq1 and seq2 = c.seq2 ) as stockunit
,dbo.getMtlDesc(c.poid,c.seq1,c.seq2,2,0) as [description]
,'{1}' as mdivisionid
from dbo.Lack_Detail a
inner join dbo.Lack b on b.ID= a.ID
inner join dbo.ftyinventory c on c.poid = b.POID and c.seq1 = a.seq1 and c.seq2  = a.seq2 and c.stocktype = 'B'
Where a.id = '{0}' and c.lock = 0  and c.mdivisionid='{1}' ", dr_master["requestid"],Sci.Env.User.Keyword)); // 
           //判斷LACKING
            //
            if (Type != "Lacking")
            { strSQLCmd.Append(" and (c.inqty-c.outqty + c.adjustqty) > 0"); }
           // string AA = strSQLCmd.ToString();
            #endregion

            MyUtility.Msg.WaitWindows("Data Loading....");

            DataSet data;
            DBProxy.Current.DefaultTimeout = 1200;
            try
            {
                MyUtility.Msg.WaitWindows("Data Loading....");
                if (!SQL.Selects("", strSQLCmd.ToString(), out data))
                {
                    ShowErr(strSQLCmd.ToString());
                    return;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                DBProxy.Current.DefaultTimeout = 0;
                MyUtility.Msg.WaitClear();
            }

            dtlack = data.Tables[0];
            dtftyinventory = data.Tables[1];

            if (dtlack.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!!");
            }

            dtlack.TableName = "dtlack";
            dtftyinventory.TableName = "dtftyinventory";

            DataRelation relation = new DataRelation("rel1"
                , new DataColumn[] { dtlack.Columns["poid"], dtlack.Columns["seq"] }
                , new DataColumn[] { dtftyinventory.Columns["poid"], dtftyinventory.Columns["seq"] }
                );
            data.Relations.Add(relation);

            dtlack.Columns.Add("issueqty", typeof(decimal), "sum(child.qty)");
            dtlack.Columns.Add("balance", typeof(decimal), "RequestQty - issueqty");

            listControlBindingSource1.DataSource = data;
            listControlBindingSource1.DataMember = "dtlack";
            listControlBindingSource2.DataSource = listControlBindingSource1;
            listControlBindingSource2.DataMember = "rel1";

            #region -- Grid1 Setting --
            this.grid1.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.grid1.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.grid1)
                .Text("Poid", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(13)) //0
                .Text("seq", header: "Seq#", iseditingreadonly: true, width: Widths.AnsiChars(6)) //1
                .EditText("Description", header: "Description", iseditingreadonly: true, width: Widths.AnsiChars(25)) //2
                .Text("StockUnit", header: "Unit", iseditingreadonly: true)      //3
                .Numeric("balance", header: "Request Qty", iseditable: true, decimal_places: 2, integer_places: 10) //4
                .Numeric("Issueqty", header: "Accu. Issue Qty", decimal_places: 2, integer_places: 10)  //5
                .Numeric("balance", header: "Balance Qty", iseditable: true, decimal_places: 2, integer_places: 10) //6
                ;
            #endregion
            #region --  Grid2 Setting --
            Ict.Win.DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            ns.CellValidating += (s, e) =>
                {
                    if (this.EditMode && !MyUtility.Check.Empty(e.FormattedValue))
                    {
                        grid2.GetDataRow(e.RowIndex)["qty"] = e.FormattedValue;
                        if (Type != "Lacking")
                        {
                            if ((decimal)e.FormattedValue > (decimal)grid2.GetDataRow(e.RowIndex)["balance"])
                            {
                                MyUtility.Msg.WarningBox("Issue qty can't be more than Stock qty!!");
                                e.Cancel = true;
                                return;
                            }
                        }
                        grid2.GetDataRow(e.RowIndex)["selected"] = true;
                        this.sum_checkedqty();
                    }
                };

            this.grid2.CellValueChanged += (s, e) =>
            {
                if (grid2.Columns[e.ColumnIndex].Name == col_chk.Name)
                {
                    this.sum_checkedqty();
                }
            };

            this.grid2.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.grid2.DataSource = listControlBindingSource2;
            Helper.Controls.Grid.Generator(this.grid2)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)   //0
                .Text("dyelot", header: "Dyelot", iseditingreadonly: true, width: Widths.AnsiChars(6)) //1
                .Text("roll", header: "Roll", iseditingreadonly: true, width: Widths.AnsiChars(6)) //2
                .Numeric("balance", header: "Stock Qty", iseditingreadonly: true, decimal_places: 2, integer_places: 10) //3
                .Numeric("qty", header: "Issue Qty", decimal_places: 2, integer_places: 10, settings: ns)  //4
                .Text("location", header: "Bulk Location", iseditingreadonly: true)      //5
                ;
            this.grid2.Columns[4].DefaultCellStyle.BackColor = Color.Pink;
            #endregion

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        // Import
        private void button2_Click(object sender, EventArgs e)
        {
            //listControlBindingSource1.EndEdit();
            grid2.ValidateControl();
            if (MyUtility.Check.Empty(dtftyinventory) || dtftyinventory.Rows.Count == 0) return;

            DataRow[] dr2 = dtftyinventory.Select("Selected = 1");
            if (dr2.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select rows first!", "Warnning");
                return;
            }

            dr2 = dtftyinventory.Select("qty = 0 and Selected = 1");
            if (dr2.Length > 0)
            {
                MyUtility.Msg.WarningBox("Qty of selected row can't be zero!", "Warning");
                return;
            }

            dr2 = dtftyinventory.Select("qty <> 0 and Selected = 1");
            foreach (DataRow tmp in dr2)
            {
                DataRow[] findrow = dt_detail.Select(string.Format("poid = '{0}' and seq1 = '{1}' and seq2 = '{2}' and roll ='{3}'and dyelot='{4}'"
                    , tmp["poid"].ToString(), tmp["seq1"].ToString(), tmp["seq2"].ToString(), tmp["roll"].ToString(), tmp["dyelot"].ToString()));

                if (findrow.Length > 0)
                {
                    findrow[0]["qty"] = tmp["qty"];
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
    }
}
