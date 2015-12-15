using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Sci;
using Sci.Data;
using Ict;

namespace Sci.Production.Warehouse
{
    public partial class P03_TaipeiInventory : Sci.Win.Subs.Base
    {
        DataRow dr;
        DataTable selectDataTable1;
        public P03_TaipeiInventory(DataRow data)
        {
            InitializeComponent();
            dr = data;
            comboBox1.SelectedIndex = 0;
            this.Text += string.Format(" ({0}-{1}- {2})", dr["id"].ToString()
, dr["seq1"].ToString()
, dr["seq2"].ToString());
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            string selectCommand1 = string.Format(@"SELECT *, 
sum(TMP.inqty - TMP.outqty) over ( order by ID,SEQ,sum(TMP.inqty - TMP.outqty) desc ) as [balance]
FROM (
SELECT invtrans.ID,Type,case Type 
			when '1' then '1:Input'
			when '2' then '2:Output'
			when '3' then '3:Transfer In'
			when '4' then '4:Adjust'
			when '5' then '5:Obsolescene'
			when '6' then '6:Return'
			end as typename , ConfirmDate, Qty inqty,0 outqty, TPEPASS1.ID+'-'+TPEPASS1.NAME ConfirmHandle
                                                                        , Po3Seq70
                                                                        , case type when '3' then TransferFactory else FactoryID end as factoryid, invtransreason.ReasonEN
                                                                        ,case type when '3' then 2 else 1 end AS SEQ
                                                                        ,invtrans.remark
                                                                        FROM InvTrans left join invtransReason on invtrans.reasonid = invtransreason.id
																		INNER JOIN TPEPASS1 ON Invtrans.ConfirmHandle = TPEPASS1.ID
                                                                        WHERE Invtrans.InventoryPOID ='{0}'
                                                                        and InventorySeq1 = '{1}'
                                                                        and InventorySeq2 = '{2}' 
																		and Type in (1,3,4,6)
union
SELECT invtrans.ID, Type,case Type 
			when '1' then '1:Input'
			when '2' then '2:Output'
			when '3' then '3:Transfer Out'
			when '4' then '4:Adjust'
			when '5' then '5:Obsolescene'
			when '6' then '6:Return'
			end as typename, ConfirmDate, 0 inqty,Qty outqty, TPEPASS1.ID+'-'+TPEPASS1.NAME ConfirmHandle
                                                                        , Po3Seq70
                                                                        ,  case type when '3' then FactoryID else FactoryID end as FactoryID
                                                                        , invtransreason.ReasonEN
                                                                        ,case type when '3' then 1 else 2 end AS SEQ
                                                                        ,invtrans.remark
                                                                        FROM InvTrans left join invtransReason on invtrans.reasonid = invtransreason.id
																		INNER JOIN TPEPASS1 ON Invtrans.ConfirmHandle = TPEPASS1.ID
                                                                        WHERE Invtrans.InventoryPOID ='{0}'
                                                                        and InventorySeq1 = '{1}'
                                                                        and InventorySeq2 = '{2}'
																		and type in (2,3,5)
                                                                        ) TMP 
                                                                        GROUP BY TMP.ID,TMP.TYPE,TMP.typename,TMP.ConfirmDate,TMP.ConfirmHandle,TMP.factoryid,TMP.Po3Seq70,TMP.ReasonEN,TMP.SEQ,TMP.inqty,TMP.outqty,tmp.remark "
                                                , dr["id"].ToString()
                                                , dr["seq1"].ToString()
                                                , dr["seq2"].ToString());

            DualResult selectResult1 = DBProxy.Current.Select(null, selectCommand1, out selectDataTable1);
            if (selectResult1 == false) ShowErr(selectCommand1, selectResult1);
            else
            {
                string remark = "";
                foreach (DataRow dr2 in selectDataTable1.Rows)
                {
                    if (!MyUtility.Check.Empty(dr2["remark"].ToString()))
                    {
                        remark += dr2["remark"].ToString().TrimEnd() + Environment.NewLine;
                    }
                }
                this.editBox1.Text = remark;
            }
            bindingSource1.DataSource = selectDataTable1;
            MyUtility.Tool.SetGridFrozen(grid1);
            //設定Grid1的顯示欄位
            this.grid1.IsEditingReadOnly = true;
            this.grid1.DataSource = bindingSource1;
            Helper.Controls.Grid.Generator(this.grid1)
                 .Text("id", header: "Transaction ID", width: Widths.AnsiChars(13))
                 .Text("factoryid", header: "Factory", width: Widths.AnsiChars(8))
                 .Text("typeName", header: "Type", width: Widths.AnsiChars(13))
                 .Date("confirmdate", header: "Date", width: Widths.AnsiChars(10))
                 .Text("confirmhandle", header: "Handle", width: Widths.AnsiChars(20))
                 .Numeric("inqty", header: "Stock In Qty", width: Widths.AnsiChars(6), integer_places: 6, decimal_places: 0)
                 .Numeric("outqty", header: "Stock Allocated Qty", width: Widths.AnsiChars(6), integer_places: 6, decimal_places: 0)
                 .Numeric("balance", header: "Balance Qty", width: Widths.AnsiChars(6), integer_places: 6, decimal_places: 0)
                  .Text("Po3seq70", header: "Use for SP#", width: Widths.AnsiChars(20))
                  .Text("ReasonEN", header: "Reason", width: Widths.AnsiChars(60))
                 ;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void grid1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    if (MyUtility.Check.Empty(selectDataTable1)) break;
                    selectDataTable1.DefaultView.Sort = "confirmdate , id";
                    break;
                case 1:
                    if (MyUtility.Check.Empty(selectDataTable1)) break;
                    selectDataTable1.DefaultView.Sort = "type , id";
                    break;

                default:
                    break;
            }
        }
    }
}
