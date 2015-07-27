using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;
using Sci;

namespace Sci.Production.Packing
{
    public partial class P13 : Sci.Win.Tems.QueryForm
    {
        private string sqlCmd;
        private DataTable gridData;
        private DualResult result;
        DataGridViewGeneratorNumericColumnSettings ctnqty = new DataGridViewGeneratorNumericColumnSettings();
        DataGridViewGeneratorNumericColumnSettings accuqty = new DataGridViewGeneratorNumericColumnSettings();
        DataGridViewGeneratorNumericColumnSettings poqty = new DataGridViewGeneratorNumericColumnSettings();
        public P13(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            //Grid設定
            this.grid1.IsEditingReadOnly = true;
            this.grid1.DataSource = listControlBindingSource1;

            //當欄位值為0時，顯示空白
            ctnqty.CellZeroStyle = Ict.Win.UI.DataGridViewNumericBoxZeroStyle.Empty;
            accuqty.CellZeroStyle = Ict.Win.UI.DataGridViewNumericBoxZeroStyle.Empty;
            poqty.CellZeroStyle = Ict.Win.UI.DataGridViewNumericBoxZeroStyle.Empty;

            Helper.Controls.Grid.Generator(this.grid1)
                .Text("BrandID", header: "Brand", width: Widths.AnsiChars(8))
                .Text("ID", header: "SP#", width: Widths.AnsiChars(13))
                .Text("Alias", header: "Destination", width: Widths.AnsiChars(13))
                .Date("SciDelivery", header: "SCI Delivery")
                .Date("SewInline", header: "Sewing Inline Date")
                .Text("RefNo", header: "Ref#", width: Widths.AnsiChars(13))
                .Text("Dimension", header: "L * W * H", width: Widths.AnsiChars(25))
                .Text("CTNUnit", header: "Carton Unit", width: Widths.AnsiChars(8))
                .Numeric("CTNQty", header: "Carton Qty", settings: ctnqty)
                .Numeric("AccuQty", header: "Accu. Qty", settings: accuqty)
                .Text("LocalPOID", header: "Local PO#", width: Widths.AnsiChars(13))
                .Date("Delivery", header: "Delivery")
                .Numeric("POQty", header: "PO Qty", settings: poqty);

            this.grid1.Columns[10].DefaultCellStyle.BackColor = Color.LightGreen;
            this.grid1.Columns[11].DefaultCellStyle.BackColor = Color.LightGreen;
            this.grid1.Columns[12].DefaultCellStyle.BackColor = Color.LightGreen;
        }

        //Query
        private void button1_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(dateRange1.Value1) && MyUtility.Check.Empty(dateRange1.Value2) && MyUtility.Check.Empty(dateRange2.Value1) && MyUtility.Check.Empty(dateRange2.Value2) && MyUtility.Check.Empty(dateRange3.Value1) && MyUtility.Check.Empty(dateRange3.Value2) && MyUtility.Check.Empty(dateRange4.Value1) && MyUtility.Check.Empty(dateRange4.Value2))
            {
                MyUtility.Msg.WarningBox("< SCI Delivery > or < Sewing Inline Date > or < Carton Est. Booking > or < Carton Est. Arrived > can not empty!");
                dateRange1.TextBox1.Focus();
                return;
            }

            if (MyUtility.Check.Empty(dateRange3.Value1) && MyUtility.Check.Empty(dateRange3.Value2) && MyUtility.Check.Empty(dateRange4.Value1) && MyUtility.Check.Empty(dateRange4.Value2))
            {
                sqlCmd = string.Format(@"with OrderData
as
(select o.BrandID,o.ID,o.SciDelivery,o.SewInLine,o.Dest,c.Alias,ocd.RefNo,STR(li.CtnLength,8,4)+'*'+STR(li.CtnWidth,8,4)+'*'+STR(li.CtnHeight,8,4) as Dimension,li.CtnUnit,ocd.CTNQty
 from Orders o
 left join Country c on c.id = o.Dest
 left join Order_CTNData ocd on ocd.ID = o.ID
 left join LocalItem li on li.RefNo = ocd.RefNo
 where o.FtyGroup = '{0}'",Sci.Env.User.Factory);
                if (!MyUtility.Check.Empty(dateRange1.Value1))
                {
                    sqlCmd = sqlCmd + string.Format(" and o.SciDelivery >= '{0}'",Convert.ToDateTime(dateRange1.Value1).ToString("d"));
                }
                if (!MyUtility.Check.Empty(dateRange1.Value2))
                {
                    sqlCmd = sqlCmd + string.Format(" and o.SciDelivery <= '{0}'",Convert.ToDateTime(dateRange1.Value2).ToString("d"));
                }
                if (!MyUtility.Check.Empty(dateRange2.Value1))
                {
                    sqlCmd = sqlCmd + string.Format(" and o.SewInLine >= '{0}'",Convert.ToDateTime(dateRange2.Value1).ToString("d"));
                }
                if (!MyUtility.Check.Empty(dateRange2.Value2))
                {
                    sqlCmd = sqlCmd + string.Format(" and o.SewInLine <= '{0}'",Convert.ToDateTime(dateRange2.Value2).ToString("d"));
                }

                sqlCmd = sqlCmd + @" ),
POData
as
(select od.BrandID,od.ID,od.SciDelivery,od.SewInLine,od.Alias,ld.Id as LocalPOID,ld.Refno,STR(li.CtnLength,8,4)+'*'+STR(li.CtnWidth,8,4)+'*'+STR(li.CtnHeight,8,4) as Dimension,li.CtnUnit,sum(ld.Qty) as POQty
 from OrderData od, LocalPO_Detail ld, LocalItem li
 where od.ID = ld.OrderId
 and li.RefNo = ld.Refno
 group by od.BrandID,od.ID,od.SciDelivery,od.SewInLine,od.Alias,ld.Id,ld.Refno,STR(li.CtnLength,8,4)+'*'+STR(li.CtnWidth,8,4)+'*'+STR(li.CtnHeight,8,4),li.CtnUnit
)
select isnull(od.ID,pd.ID) as ID,isnull(od.BrandID,pd.BrandID) as BrandID,isnull(od.SciDelivery,pd.SciDelivery) as SciDelivery,isnull(od.SewInLine,pd.SewInLine) as SewInLine,isnull(od.Alias,isnull(pd.Alias,'')) as Alias,isnull(od.Refno,pd.Refno) as Refno,
isnull(pd.LocalPOID,'') as LocalPOID,isnull(od.Dimension,isnull(pd.Dimension,'')) as Dimension,isnull(od.CtnUnit,isnull(pd.CtnUnit,'')) as CtnUnit,isnull(od.CTNQty,0) as CTNQty,isnull(pd.POQty,0) as POQty,isnull(od.CTNQty,0)-isnull(pd.POQty,0) as AccuQty
from OrderData od
full outer join POData pd on pd.ID = od.ID and pd.Refno = od.RefNo
order by SciDelivery,ID";
            }
            else
            {
                sqlCmd = @"with PackData
as
(select distinct pld.OrderID
 from PackingList pl, PackingList_Detail pld
 where pl.ID = pld.ID";
                if (!MyUtility.Check.Empty(dateRange3.Value1))
                {
                    sqlCmd = sqlCmd + string.Format(" and pl.EstCTNBooking >= '{0}'",Convert.ToDateTime(dateRange3.Value1).ToString("d"));
                }
                if (!MyUtility.Check.Empty(dateRange3.Value2))
                {
                    sqlCmd = sqlCmd + string.Format(" and pl.EstCTNBooking <= '{0}'",Convert.ToDateTime(dateRange3.Value2).ToString("d"));
                }
                if (!MyUtility.Check.Empty(dateRange4.Value1))
                {
                    sqlCmd = sqlCmd + string.Format(" and pl.EstCTNArrive >= '{0}'",Convert.ToDateTime(dateRange4.Value1).ToString("d"));
                }
                if (!MyUtility.Check.Empty(dateRange4.Value2))
                {
                    sqlCmd = sqlCmd + string.Format(" and pl.EstCTNArrive <= '{0}'",Convert.ToDateTime(dateRange4.Value2).ToString("d"));
                }
                sqlCmd = sqlCmd + string.Format(@"),
OrderData
as
(select o.BrandID,o.ID,o.SciDelivery,o.SewInLine,o.Dest,c.Alias,ocd.RefNo,STR(li.CtnLength,8,4)+'*'+STR(li.CtnWidth,8,4)+'*'+STR(li.CtnHeight,8,4) as Dimension,li.CtnUnit,ocd.CTNQty
 from PackData pd
 left join Orders o on o.ID = pd.OrderID
 left join Country c on c.id = o.Dest
 left join Order_CTNData ocd on ocd.ID = o.ID
 left join LocalItem li on li.RefNo = ocd.RefNo
 where o.FtyGroup = '{0}'",Sci.Env.User.Factory);
                if (!MyUtility.Check.Empty(dateRange1.Value1))
                {
                    sqlCmd = sqlCmd + string.Format(" and o.SciDelivery >= '{0}'",Convert.ToDateTime(dateRange1.Value1).ToString("d"));
                }
                if (!MyUtility.Check.Empty(dateRange1.Value2))
                {
                    sqlCmd = sqlCmd + string.Format(" and o.SciDelivery <= '{0}'",Convert.ToDateTime(dateRange1.Value2).ToString("d"));
                }
                if (!MyUtility.Check.Empty(dateRange2.Value1))
                {
                    sqlCmd = sqlCmd + string.Format(" and o.SewInLine >= '{0}'",Convert.ToDateTime(dateRange2.Value1).ToString("d"));
                }
                if (!MyUtility.Check.Empty(dateRange2.Value2))
                {
                    sqlCmd = sqlCmd + string.Format(" and o.SewInLine <= '{0}'",Convert.ToDateTime(dateRange2.Value2).ToString("d"));
                }
                sqlCmd = sqlCmd + @" ),
POData
as
(select od.BrandID,od.ID,od.SciDelivery,od.SewInLine,od.Alias,ld.Id as LocalPOID,ld.Refno,STR(li.CtnLength,8,4)+'*'+STR(li.CtnWidth,8,4)+'*'+STR(li.CtnHeight,8,4) as Dimension,li.CtnUnit,sum(ld.Qty) as POQty
 from OrderData od, LocalPO_Detail ld, LocalItem li
 where od.ID = ld.OrderId
 and li.RefNo = ld.Refno
 group by od.BrandID,od.ID,od.SciDelivery,od.SewInLine,od.Alias,ld.Id,ld.Refno,STR(li.CtnLength,8,4)+'*'+STR(li.CtnWidth,8,4)+'*'+STR(li.CtnHeight,8,4),li.CtnUnit
)
select isnull(od.ID,pd.ID) as ID,isnull(od.BrandID,pd.BrandID) as BrandID,isnull(od.SciDelivery,pd.SciDelivery) as SciDelivery,isnull(od.SewInLine,pd.SewInLine) as SewInLine,isnull(od.Alias,isnull(pd.Alias,'')) as Alias,isnull(od.Refno,pd.Refno) as Refno,
isnull(pd.LocalPOID,'') as LocalPOID,isnull(od.Dimension,isnull(pd.Dimension,'')) as Dimension,isnull(od.CtnUnit,isnull(pd.CtnUnit,'')) as CtnUnit,isnull(od.CTNQty,0) as CTNQty,isnull(pd.POQty,0) as POQty,isnull(od.CTNQty,0)-isnull(pd.POQty,0) as AccuQty
from OrderData od
full outer join POData pd on pd.ID = od.ID and pd.Refno = od.RefNo
order by SciDelivery,ID";
            }
            if (result = DBProxy.Current.Select(null, sqlCmd, out gridData))
            {
                if (gridData.Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!");
                }
            }
            listControlBindingSource1.DataSource = gridData;
        }

        //To Excel
        private void button2_Click(object sender, EventArgs e)
        {

        }

        //Close
        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
