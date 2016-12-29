using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Sci;
using Sci.Data;
using Ict;
using Ict.Win;

namespace Sci.Production.Warehouse
{
    public partial class R17 : Sci.Win.Tems.PrintForm
    {
        DataTable dt;

        int selectindex = 0;
        public R17(ToolStripMenuItem menuitem)
            :base(menuitem)
        {
            InitializeComponent();
            this.EditMode = true;
            comboBox1.SelectedIndex = 0;
        }

        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(this.dateRange1.TextBox1.Value) && MyUtility.Check.Empty(textBox1.Text) && MyUtility.Check.Empty(textBox3.Text))
            {
                MyUtility.Msg.WarningBox("SP#, SCI Delivery, Location can't be empty!!");
                return false;
            }
            selectindex = comboBox1.SelectedIndex;
            return base.ValidateInput();
        }

        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            SetCount(dt.Rows.Count);
            DualResult result = Result.True;
            if (dt.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found!!");
                return result;
            }

            MyUtility.Excel.CopyToXls(dt,"","Warehouse_R17_Location_List.xltx");
            return false;
        }

        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            //return base.OnAsyncDataLoad(e);
            String spno = textBox1.Text.TrimEnd();
            String seq = textBox2.Text.PadRight(5);
            String location1 = textBox3.Text.TrimEnd();
            String location2 = textBox4.Text.TrimEnd();
            bool chkbalance = checkBox1.Checked;

            DualResult result = Result.True;
            StringBuilder sqlcmd = new StringBuilder();
            #region sql command
            if (MyUtility.Check.Empty(dateRange1.Value1))    // SCI Delivery empty
            {
                if (MyUtility.Check.Empty(location1)) // Location empty
                {
                    sqlcmd.Append(@"select distinct (select orders.Factoryid from orders where orders.id = a.poid) Factory
,a.Poid,a.seq1,a.seq2,a.Dyelot,a.Roll,p.Refno
,stuff((select ',' + cast(MtlLocationID as varchar) from (select MtlLocationID from FtyInventory_Detail where ukey = a.ukey) t for xml path('')), 1, 1, '') as location
,p.Width,p.ColorID Color,p.SizeSpec Size
,dbo.getMtlDesc(A.Poid,A.SEQ1,A.SEQ2,2,0) AS description 
,a.StockType
,(select max(Deadline) from dbo.Inventory i 
	where i.POID=a.Poid and i.seq1 =a.Seq1 and i.Seq2 =a.Seq2 and i.FactoryID = (select orders.Factoryid from orders where orders.id = a.poid)) deadline
,a.InQty,a.OutQty,a.AdjustQty
,a.inqty- a.outqty + a.adjustqty Balance
from dbo.FtyInventory a left join dbo.FtyInventory_Detail b on a.Ukey = b.Ukey
inner join dbo.PO_Supp_Detail p on p.id = a.Poid and p.seq1 = a.seq1 and p.seq2 = a.seq2
where 1=1");
                    if (!MyUtility.Check.Empty(spno)) sqlcmd.Append(string.Format(@"And a.Poid like '{0}%'", spno));
                    if (!MyUtility.Check.Empty(seq)) sqlcmd.Append(string.Format(@" And a.seq1 ='{0}' and a.seq2 = '{1}'", seq.Substring(0, 3), seq.Substring(3)));
                    if (chkbalance) sqlcmd.Append(@" And a.inqty- a.outqty + a.adjustqty > 0");
                    switch (selectindex)
                    {
                        case 0:
                            sqlcmd.Append(@" And (a.stocktype = 'B' or a.stocktype = 'I')");
                            break;
                        case 1:
                            sqlcmd.Append(@" And a.stocktype = 'B'");
                            break;
                        case 2:
                            sqlcmd.Append(@" And a.stocktype = 'I'");
                            break;
                    }
                }
                else
                {
                    sqlcmd.Append(string.Format(@"select distinct (select orders.Factoryid from orders where orders.id = a.poid) as factory,
a.Poid,a.seq1,a.seq2,a.Dyelot,a.Roll,p.Refno
,stuff((select ',' + cast(MtlLocationID as varchar) from (select MtlLocationID from FtyInventory_Detail where ukey = a.ukey) t for xml path('')), 1, 1, '') as location
,p.Width,p.ColorID,p.SizeSpec
,dbo.getMtlDesc(A.Poid,A.SEQ1,A.SEQ2,2,0) AS description 
,a.StockType
,(select max(Deadline) from dbo.Inventory i 
					where i.POID=a.Poid and i.seq1 =a.Seq1 and i.Seq2 =a.Seq2
						and i.FactoryID = (select FactoryID from orders where id = a.Poid)) deadline
,a.InQty,a.OutQty,a.AdjustQty,a.inqty- a.outqty + a.adjustqty Balance
from dbo.FtyInventory a left join dbo.FtyInventory_Detail b on a.Ukey = b.Ukey
inner join dbo.PO_Supp_Detail p on p.id = a.Poid and p.seq1 = a.seq1 and p.seq2 = a.seq2
where 1=1 And b.mtllocationid >= '{0}' and b.mtllocationid <= '{1}'", location1, location2));
                    if (!MyUtility.Check.Empty(spno)) sqlcmd.Append(string.Format(@" And a.Poid like '{0}%'", spno));
                    if (!MyUtility.Check.Empty(seq)) sqlcmd.Append(string.Format(@" And a.seq1 ='{0}' and a.seq2 = '{1}'", seq.Substring(0, 3), seq.Substring(3)));
                    if (chkbalance) sqlcmd.Append(@" And a.inqty- a.outqty + a.adjustqty > 0");
                    switch (selectindex)
                    {
                        case 0:
                            sqlcmd.Append(@" And (a.stocktype = 'B' or a.stocktype = 'I')");
                            break;
                        case 1:
                            sqlcmd.Append(@" And a.stocktype = 'B'");
                            break;
                        case 2:
                            sqlcmd.Append(@" And a.stocktype = 'I'");
                            break;
                    }
                }
            }
            else
            {// 有下sci delivery 條件
                if (MyUtility.Check.Empty())
                {
                    sqlcmd.Append(string.Format(@"select distinct orders.factoryid factory,a.Poid,a.seq1,a.seq2,a.Dyelot,a.Roll,p.Refno
,stuff((select ',' + cast(MtlLocationID as varchar) from (select MtlLocationID from FtyInventory_Detail where ukey = a.ukey) t for xml path('')), 1, 1, '') as location
,p.Width,p.ColorID,p.SizeSpec
,dbo.getMtlDesc(A.Poid,A.SEQ1,A.SEQ2,2,0) AS description 
,a.StockType
,(select max(Deadline) from dbo.Inventory i 
	where i.POID=a.Poid and i.seq1 =a.Seq1 and i.Seq2 =a.Seq2 and i.FactoryID = orders.Factoryid) deadline
,a.InQty,a.OutQty,a.AdjustQty,a.inqty- a.outqty + a.adjustqty Balance
from dbo.FtyInventory a left join dbo.FtyInventory_Detail b on a.Ukey = b.Ukey
inner join dbo.PO_Supp_Detail p on p.id = a.Poid and p.seq1 = a.seq1 and p.seq2 = a.seq2
inner join dbo.orders on orders.id = p.id
where 1=1
And orders.scidelivery between '{0}' and '{1}'", dateRange1.Text1, dateRange1.Text2));
                    if (!MyUtility.Check.Empty(spno)) sqlcmd.Append(string.Format(@" And a.Poid like '{0}%'", spno));
                    if (!MyUtility.Check.Empty(seq)) sqlcmd.Append(string.Format(@" And a.seq1 ='{0}' and a.seq2 = '{1}'", seq.Substring(0, 3), seq.Substring(3)));
                    if (chkbalance) sqlcmd.Append(@" And a.inqty- a.outqty + a.adjustqty > 0");
                    switch (selectindex)
                    {
                        case 0:
                            sqlcmd.Append(@" And (a.stocktype = 'B' or a.stocktype = 'I')");
                            break;
                        case 1:
                            sqlcmd.Append(@" And a.stocktype = 'B'");
                            break;
                        case 2:
                            sqlcmd.Append(@" And a.stocktype = 'I'");
                            break;
                    }
                }
                else
                {
                    sqlcmd.Append(string.Format(@"select distinct orders.factoryid as factory,a.Poid,a.seq1,a.seq2,a.Dyelot,a.Roll,p.Refno
,stuff((select ',' + cast(MtlLocationID as varchar) from (select MtlLocationID from FtyInventory_Detail where ukey = a.ukey) t for xml path('')), 1, 1, '') as location
,p.Width,p.ColorID,p.SizeSpec
,dbo.getMtlDesc(A.Poid,A.SEQ1,A.SEQ2,2,0) AS description 
,a.StockType
,(select max(Deadline) from dbo.Inventory i 
	where i.POID=a.Poid and i.seq1 =a.Seq1 and i.Seq2 =a.Seq2 and i.FactoryID = orders.FactoryID) deadline
,a.InQty,a.OutQty,a.AdjustQty,a.inqty- a.outqty + a.adjustqty Balance
from dbo.FtyInventory a 
left join dbo.FtyInventory_Detail b on a.Ukey = b.Ukey
inner join dbo.PO_Supp_Detail p on p.id = a.Poid and p.seq1 = a.seq1 and p.seq2 = a.seq2
inner join dbo.orders on orders.ID = p.ID
where 1=1
And b.mtllocationid >= '{0}' and b.mtllocationid <= '{1}'
And orders.scidelivery between '{2}' and '{3}'", location1, location2, dateRange1.Text1, dateRange1.Text2));
                    if (!MyUtility.Check.Empty(spno)) sqlcmd.Append(string.Format(@" And a.Poid like '{0}%'", spno));
                    if (!MyUtility.Check.Empty(seq)) sqlcmd.Append(string.Format(@" And a.seq1 ='{0}' and a.seq2 = '{1}'", seq.Substring(0, 3), seq.Substring(3)));
                    if (chkbalance) sqlcmd.Append(@" And a.inqty- a.outqty + a.adjustqty > 0");
                    switch (selectindex)
                    {
                        case 0:
                            sqlcmd.Append(@" And (a.stocktype = 'B' or a.stocktype = 'I')");
                            break;
                        case 1:
                            sqlcmd.Append(@" And a.stocktype = 'B'");
                            break;
                        case 2:
                            sqlcmd.Append(@" And a.stocktype = 'I'");
                            break;
                    }
                }
            }
            #endregion
            try
            {
                DBProxy.Current.DefaultTimeout = 600;
                result = DBProxy.Current.Select(null, sqlcmd.ToString(), out dt);
                DBProxy.Current.DefaultTimeout = 30;
                if (!result) return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        private void toexcel_Click(object sender, EventArgs e)
        {

        }
    }
}
