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
    public partial class P10_Detail_Detail : Sci.Win.Subs.Base
    {
        public Sci.Win.Subs.Base P10_Detail;
        DataRow dr_master;
        DataTable dt_detail;
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        protected DataTable dtFtyinventory;
        int Type;
        public P10_Detail_Detail(DataRow master, DataTable detail, int type = 0)
        {
            InitializeComponent();
            dr_master = master;
            dt_detail = detail;
            this.Type = type;
        }

        private void sum_checkedqty()
        {
            listControlBindingSource1.EndEdit();
            DataTable dt = (DataTable)listControlBindingSource1.DataSource;
            Object localPrice = dt.Compute("Sum(qty)", "selected = 1");
            if (!MyUtility.Check.Empty(localPrice) && !MyUtility.Check.Empty(dr_master["requestqty"].ToString()))
            {
                numRequestVariance.Value = Convert.ToDecimal(dr_master["requestqty"].ToString()) - Convert.ToDecimal(localPrice.ToString());
            }
            this.displayTotalQty.Value = localPrice.ToString();              
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.displaySCIRefno.Text = dr_master["SCIRefno"].ToString();
            this.displayRefno.Text = dr_master["refno"].ToString();
            this.displaySPNo.Text = dr_master["poid"].ToString();
            this.displayColorID.Text = dr_master["colorid"].ToString();
            this.displaySizeSpec.Text = dr_master["sizespec"].ToString();
            this.displayDesc.Text = dr_master["description"].ToString();


            StringBuilder strSQLCmd = new StringBuilder();
            #region -- sqlcmd query -- 
            strSQLCmd.Append(string.Format(@"
with cte as 
(
      select Dyelot
             , sum(inqty-OutQty+AdjustQty) as GroupQty
      from dbo.FtyInventory a WITH (NOLOCK) 
      inner join dbo.PO_Supp_Detail p WITH (NOLOCK) on p.id = a.POID 
                                                       and p.seq1 = a.Seq1 
                                                       and p.seq2 = a.Seq2
      where poid = '{0}' 
            and Stocktype = 'B' 
            and inqty-OutQty+AdjustQty > 0
            and p.Refno = '{2}' 
            and p.ColorID = '{3}' 
            {4}--and a.Seq1 BETWEEN '00' AND '99'
      Group by Dyelot
) 
select 0 as selected 
       , id = '' 
       , PoId = a.id
       , a.Seq1
       , a.Seq2
       , seq = concat(Ltrim(Rtrim(a.seq1)), ' ', a.Seq2)
       , a.FabricType
       , a.stockunit
       , [Description] = dbo.getmtldesc(a.id,a.seq1,a.seq2,2,0)
       , Roll = Rtrim(Ltrim(c.Roll))
       , Dyelot = Rtrim(Ltrim(c.Dyelot))
       , Qty = 0.00
       , StockType = 'B'
       , ftyinventoryukey = c.ukey 
       , location = dbo.Getlocation(c.ukey) 
       , balanceqty = c.inqty-c.outqty + c.adjustqty
       , c.inqty
       , c.outqty
       , c.adjustqty 
from dbo.PO_Supp_Detail a WITH (NOLOCK) 
inner join dbo.ftyinventory c WITH (NOLOCK) on c.poid = a.id and c.seq1 = a.seq1 and c.seq2  = a.seq2 and c.stocktype = 'B'
inner join cte d on d.Dyelot=c.Dyelot
Where a.id = '{0}' and c.lock = 0 and c.inqty-c.outqty + c.adjustqty > 0 
and a.Refno='{2}' and a.colorid='{3}' {5}--and ltrim(a.seq1) between '01' and '99'
order by d.GroupQty DESC,c.Dyelot,balanceqty DESC", dr_master["poid"]
                                                  , Sci.Env.User.Keyword
                                                  , dr_master["Refno"]
                                                  , dr_master["colorid"]
                                                  , this.Type == 0 ? " and a.Seq1 BETWEEN '00' AND '99'" : string.Empty
                                                  , this.Type == 0 ? " and ltrim(a.seq1) between '01' and '99'" : string.Empty
                                                  ));
            #endregion

            P10_Detail.ShowWaitMessage("Data Loading....");
            Ict.DualResult result;
            if (result = DBProxy.Current.Select(null, strSQLCmd.ToString(), out dtFtyinventory))
            {
                if (dtFtyinventory.Rows.Count == 0)
                { MyUtility.Msg.WarningBox("Data not found!!"); }
                listControlBindingSource1.DataSource = dtFtyinventory;
            }
            else { ShowErr(strSQLCmd.ToString(), result); }
            P10_Detail.HideWaitMessage();

            Ict.Win.DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            ns.CellValidating += (s, e) =>
                {
                    if (this.EditMode && !MyUtility.Check.Empty(e.FormattedValue))
                    {
                        gridRollNo.GetDataRow(gridRollNo.GetSelectedRowIndex())["qty"] = e.FormattedValue;
                        gridRollNo.GetDataRow(gridRollNo.GetSelectedRowIndex())["selected"] = true;
                        this.sum_checkedqty();
                    }
                };
            
            this.gridRollNo.CellValueChanged += (s, e) =>
            {
                if (gridRollNo.Columns[e.ColumnIndex].Name == col_chk.Name)
                {
                    DataRow dr = gridRollNo.GetDataRow(e.RowIndex);
                    if (Convert.ToBoolean(dr["selected"]) == true && Convert.ToDecimal(dr["qty"].ToString()) == 0)
                    {
                        dr["qty"] = dr["balanceqty"];
                    }
                    else if (Convert.ToBoolean(dr["selected"]) == false)
                    {
                        dr["qty"] = 0;
                    }
                    dr.EndEdit();
                    this.sum_checkedqty();
                }
            };
            
            this.gridRollNo.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.gridRollNo.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.gridRollNo)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)   //0
                .Text("seq", header: "Seq#", iseditingreadonly: true, width: Widths.AnsiChars(6)) //1
                .Text("location", header: "Bulk Location", iseditingreadonly: true)      //2
                .Text("dyelot", header: "Dyelot", iseditingreadonly: true, width: Widths.AnsiChars(8)) //3
                .Text("roll", header: "Roll", iseditingreadonly: true, width: Widths.AnsiChars(6)) //4
                .Text("StockUnit", header: "Unit", iseditingreadonly: true)      //5
                .Numeric("balanceqty", header: "Balance Qty", iseditingreadonly: true, decimal_places: 2, integer_places: 10) //6
                .Numeric("qty", header: "Issue Qty", iseditable: true, decimal_places: 2, integer_places: 10, settings: ns)  //7
               .EditText("Description", header: "Description", iseditingreadonly: true, width: Widths.AnsiChars(25)); //8

            this.gridRollNo.Columns["qty"].DefaultCellStyle.BackColor = Color.Pink;

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            gridRollNo.ValidateControl();
            DataTable dtGridBS1 = (DataTable)listControlBindingSource1.DataSource;
            if (MyUtility.Check.Empty(dtGridBS1) || dtGridBS1.Rows.Count == 0) return;

            DataRow[] dr2 = dtGridBS1.Select("Selected = 1");
            if (dr2.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select rows first!", "Warnning");
                return;
            }

            dr2 = dtGridBS1.Select("qty = 0 and Selected = 1");
            if (dr2.Length > 0)
            {
                MyUtility.Msg.WarningBox("Issue Qty of selected row can't be zero!", "Warning");
                return;
            }

            dr2 = dtGridBS1.Select("qty > balanceqty and Selected = 1");
            if (dr2.Length > 0)
            {
                MyUtility.Msg.WarningBox("Issue Qty of selected row can't be more then balance qty!", "Warning");
                return;
            }

            dr2 = dtGridBS1.Select("qty <> 0 and Selected = 1");
            foreach (DataRow tmp in dr2)
            {
                DataRow[] findrow = dt_detail.Select(string.Format("poid = '{0}' and seq1 = '{1}' and seq2 = '{2}' and roll = '{3}' and dyelot = '{4}'",
                    tmp["poid"].ToString(), tmp["seq1"].ToString(), tmp["seq2"].ToString(), tmp["roll"].ToString(), tmp["dyelot"].ToString()));

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
