﻿using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Quality
{
    public partial class P26 : Sci.Win.Tems.QueryForm
    {
        private DataTable selectDataTable;
        private string selectDataTable_DefaultView_Sort = string.Empty;

        public P26(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.grid.IsEditingReadOnly = true;
            this.grid.DataSource = this.listControlBindingSource1;

            this.Helper.Controls.Grid.Generator(this.grid)
                 .Text("ReturnTo", header: "Return To", width: Widths.AnsiChars(7), iseditingreadonly: true)
                 .Date("ReturnDate", header: "Return Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Text("PackingListID", header: "Pack ID", width: Widths.AnsiChars(15), iseditingreadonly: true)
                 .Text("CTNStartNo", header: "CTN#", width: Widths.AnsiChars(6), iseditingreadonly: true)
                 .Text("OrderID", header: "SP#", width: Widths.AnsiChars(15), iseditingreadonly: true)                 
                 .Text("StyleID", header: "Style#", width: Widths.AnsiChars(13), iseditingreadonly: true)                 
                 .Text("BrandID", header: "Brand", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Text("Alias", header: "Destination", width: Widths.AnsiChars(12), iseditingreadonly: true)
                 .Date("BuyerDelivery", header: "Buyer Delivery", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Date("SciDelivery", header: "SCI Delivery", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Text("ReceivedBy", header: "Received By", width: Widths.AnsiChars(15), iseditingreadonly: true);

            // 增加CTNStartNo 有中文字的情況之下 按照我們希望的順序排
            int rowIndex = 0;
            int columIndex = 0;
            this.grid.CellClick += (s, e) =>
            {
                rowIndex = e.RowIndex;
                columIndex = e.ColumnIndex;
            };

            this.grid.Sorted += (s, e) =>
            {
                #region 如果準備排序的欄位 = "CTNStartNo" 則用以下方法排序
                if ((rowIndex == -1) && this.selectDataTable.Columns[columIndex].ColumnName.ToString().EqualString("CTNStartNo"))
                {
                    this.listControlBindingSource1.DataSource = null;

                    if (this.selectDataTable_DefaultView_Sort == "DESC")
                    {
                        this.selectDataTable.DefaultView.Sort = "rn DESC";
                        this.selectDataTable_DefaultView_Sort = string.Empty;
                    }
                    else
                    {
                        this.selectDataTable.DefaultView.Sort = "rn ASC";
                        this.selectDataTable_DefaultView_Sort = "DESC";
                    }

                    this.listControlBindingSource1.DataSource = this.selectDataTable;
                    return;
                }
                #endregion
            };
        }

        // Find
        private void Find()
        {
            string strReturnToStart = this.dateReturnDate.Value1.Empty() ? string.Empty : ((DateTime)this.dateReturnDate.Value1).ToString("yyyy/MM/dd");
            string strReturnToEnd = this.dateReturnDate.Value2.Empty() ? string.Empty : ((DateTime)this.dateReturnDate.Value2).ToString("yyyy/MM/dd");

            #region SqlParameter
            List<SqlParameter> listSQLParameter = new List<SqlParameter>();
            listSQLParameter.Add(new SqlParameter("@OrderID", this.txtSPNo.Text));
            listSQLParameter.Add(new SqlParameter("@ReturnTo", this.txtReturnTo.Text));
            listSQLParameter.Add(new SqlParameter("@PackID", this.txtPackID.Text));
            listSQLParameter.Add(new SqlParameter("@ReturnStart", strReturnToStart));
            listSQLParameter.Add(new SqlParameter("@ReturnEnd", strReturnToEnd));
            #endregion

            #region SQL Filter
            List<string> listSQLFilter = new List<string>();
            if (!MyUtility.Check.Empty(strReturnToStart)
                && !MyUtility.Check.Empty(strReturnToEnd))
            {
                listSQLFilter.Add("and c.ReturnDate between @ReturnStart and @ReturnEnd");
            }

            if (!MyUtility.Check.Empty(this.txtSPNo.Text))
            {
                listSQLFilter.Add("and c.orderid = @OrderID");
            }

            if (!MyUtility.Check.Empty(this.txtReturnTo.Text))
            {
                listSQLFilter.Add("and c.ReturnTo= @ReturnTo");
            }

            if (!MyUtility.Check.Empty(this.txtPackID.Text))
            {
                listSQLFilter.Add("and c.PackingListID= @PackID");
            }
            #endregion

            this.ShowWaitMessage("Data Loading....");

            #region Sql Command

            string strCmd = $@"
select c.ReturnTo
,c.ReturnDate
,c.PackingListID
,c.CTNStartNo
,c.OrderID
,o.CustPONo
,o.Styleid
,o.Brandid
,ct.Alias
,o.BuyerDelivery
,o.SciDelivery
,[ReceivedBy] = dbo.getPass1(c.AddName)
,c.ID
from cfareturn c
left join Orders o on o.id=c.orderid
left join Country ct WITH (NOLOCK) on ct.id=o.dest
where 1=1
{listSQLFilter.JoinToString($"{Environment.NewLine} ")}
";
            #endregion            
            DualResult result = DBProxy.Current.Select("", strCmd, listSQLParameter, out selectDataTable);

            if (!result)
            {
                MyUtility.Msg.WarningBox(result.ToString());
            }
            else if (selectDataTable.Rows.Count < 1)
            {
                this.listControlBindingSource1.DataSource = null;
                MyUtility.Msg.InfoBox("Data not found !");
            }
            else
            {
                this.listControlBindingSource1.DataSource = selectDataTable;
            }

            this.HideWaitMessage();
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            Find();
        }
    }
}
