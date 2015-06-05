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
using System.Transactions;
using Sci;

namespace Sci.Production.Logistic
{
    public partial class P02_BatchReceiving : Sci.Win.Subs.Base
    {
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        protected int allRecord = 0;
        DataTable receiveDetailData;
        public P02_BatchReceiving(DateTime receiveDate, DataTable detailData)
        {
            InitializeComponent();
            this.Text = "Carton Receiving - Batch Receive (Receive Date - " + receiveDate.ToString("d") + ")";
            receiveDetailData = detailData;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.grid1.CellValueChanged += (s, e) =>
                {
                    if (grid1.CurrentCellAddress.X == 0)
                    {
                        this.numericBox4.Value = grid1.GetCheckeds(col_chk).Count;
                    }
                };

            this.grid1.IsEditingReadOnly = false;
            this.grid1.DataSource = bindingSource1;

            Helper.Controls.Grid.Generator(this.grid1)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)
                .CellClogLocation("ClogLocationId", header: "Location No", width: Widths.AnsiChars(10))
                .Text("OrderId", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("CTNStartNo", header: "CTN#", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("StyleID", header: "Style#", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("SeasonID", header: "Season", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("BrandID", header: "Brand", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("Customize1", header: "Order#", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("CustPONo", header: "P.O.#", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Alias", header: "Destination#", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Date("BuyerDelivery", header: "Buyer Delivery", width: Widths.AnsiChars(10), iseditingreadonly: true);
        }

        //Find
        private void button1_Click(object sender, EventArgs e)
        {
            if (myUtility.Empty(this.textBox1) && myUtility.Empty(this.textBox2))
            {
                MessageBox.Show("< Transfer Clog No > can not be empty!");
                this.textBox1.Focus();
                return;
            }

            string selectCommand1, selectCommand2, selectCommand3;
            if (!myUtility.Empty(this.textBox1.Text) && !myUtility.Empty(this.textBox2.Text))
            {
                selectCommand1 = string.Format(@"Select '' as ID, 0 as selected,'' as ClogLocationId,a.*,b.StyleID,b.SeasonID,b.BrandID,b.Customize1,b.CustPONo,b.BuyerDelivery,c.Alias from 
                                                                            ((Select a.Id as TransferToClogId, PackingListId, OrderId, CTNStartNo 
                                                                                    from TransferToClog_Detail as a, TransferToClog as b 
                                                                                    where a.Id >= '{0}' and a.Id <= '{1}' and a.Id = b.ID  and b.FactoryID = '{2}') 
                                                                               except 
                                                                              (Select TransferToClogId, PackingListId, OrderId, CTNStartNo 
                                                                                    from ClogReceive_Detail 
                                                                                    where TransferToClogId >= '{0}' and TransferToClogId <= '{1}')) 
                                                                            as a ", this.textBox1.Text.Trim(), this.textBox2.Text.Trim(), Sci.Env.User.Factory);
                selectCommand1 = selectCommand1 + "left join Orders b On a.OrderID = b.ID left join Country c On b.Dest = c.ID";
                selectCommand2 = string.Format(@"Select count(TransferToClogId) as TTLCTN 
                                                                            from ((Select a.Id as TransferToClogId, PackingListId, OrderId, CTNStartNo 
                                                                                            from TransferToClog_Detail as a, TransferToClog as b 
                                                                                            where a.Id >= '{0}' and a.Id <= '{1}' and a.Id = b.ID and b.FactoryID = '{2}') 
                                                                                     except 
                                                                                      (Select a.TransferToClogId, a.PackingListId, a.OrderId, a.CTNStartNo 
                                                                                            from ClogReturn_Detail a, ClogReturn b 
                                                                                            where a.TransferToClogId >= '{0}' and a.TransferToClogId <= '{1}' and a.Id = b.Id and b.Encode = 1)) 
                                                                                    as result", this.textBox1.Text.Trim(), this.textBox2.Text.Trim(), Sci.Env.User.Factory);
                selectCommand3 = string.Format(@"Select count(TransferToClogId) as ReceivedCTN 
                                                                            from ((Select a.TransferToClogId, a.PackingListId, a.OrderId, a.CTNStartNo 
                                                                                            from ClogReceive_Detail a, ClogReceive b 
                                                                                            where a.TransferToClogId >= '{0}' and a.TransferToClogId <= '{1}' and a.Id = b.Id and b.Encode = 1 and b.FactoryID = '{2}') 
                                                                                    except 
                                                                                      (Select a.TransferToClogId, a.PackingListId, a.OrderId, a.CTNStartNo 
                                                                                            from ClogReturn_Detail a, ClogReturn b 
                                                                                            where a.TransferToClogId >= '{0}' and a.TransferToClogId <= '{1}' and a.Id = b.Id and b.Encode = 1)) 
                                                                                    as result", this.textBox1.Text.Trim(), this.textBox2.Text.Trim(), Sci.Env.User.Factory);
            }
            else
            {
                if (!myUtility.Empty(this.textBox1.Text))
                {
                    selectCommand1 = string.Format(@"Select '' as ID, 0 as selected,'' as ClogLocationId,a.*,b.StyleID,b.SeasonID,b.BrandID,b.Customize1,b.CustPONo,b.BuyerDelivery,c.Alias 
                                                                                from ((Select a.Id as TransferToClogId, PackingListId, OrderId, CTNStartNo 
                                                                                                from TransferToClog_Detail as a, TransferToClog as b 
                                                                                                where a.Id >= '{0}' and a.Id = b.ID and b.FactoryID = '{1}') 
                                                                                        except 
                                                                                          (Select TransferToClogId, PackingListId, OrderId, CTNStartNo 
                                                                                                from ClogReceive_Detail 
                                                                                                where TransferToClogId >= '{0}')) 
                                                                                        as a ", this.textBox1.Text.Trim(), Sci.Env.User.Factory);
                    selectCommand1 = selectCommand1 + "left join Orders b On a.OrderID = b.ID left join Country c On b.Dest = c.ID";
                    selectCommand2 = string.Format(@"Select count(TransferToClogId) as TTLCTN 
                                                                                from ((Select a.Id as TransferToClogId, PackingListId, OrderId, CTNStartNo 
                                                                                                from TransferToClog_Detail as a, TransferToClog as b 
                                                                                                where a.Id >= '{0}' and a.Id = b.ID and b.FactoryID = '{1}') 
                                                                                        except 
                                                                                          (Select a.TransferToClogId, a.PackingListId, a.OrderId, a.CTNStartNo 
                                                                                                from ClogReturn_Detail a, ClogReturn b 
                                                                                                where a.TransferToClogId >= '{0}' and a.Id = b.Id and b.Encode = 1)) 
                                                                                        as result", this.textBox1.Text.Trim(), Sci.Env.User.Factory);
                    selectCommand3 = string.Format(@"Select count(TransferToClogId) as ReceivedCTN 
                                                                                from ((Select a.TransferToClogId, a.PackingListId, a.OrderId, a.CTNStartNo 
                                                                                                from ClogReceive_Detail a, ClogReceive b 
                                                                                                where a.TransferToClogId >= '{0}' and a.Id = b.Id and b.Encode = 1 and b.FactoryID = '{1}') 
                                                                                        except 
                                                                                          (Select a.TransferToClogId, a.PackingListId, a.OrderId, a.CTNStartNo 
                                                                                                from ClogReturn_Detail a, ClogReturn b 
                                                                                                where a.TransferToClogId >= '{0}' and a.Id = b.Id and b.Encode = 1)) 
                                                                                        as result", this.textBox1.Text.Trim(), Sci.Env.User.Factory);
                }
                else
                {
                    selectCommand1 = string.Format(@"Select '' as ID, 0 as selected,'' as ClogLocationId,a.*,b.StyleID,b.SeasonID,b.BrandID,b.Customize1,b.CustPONo,b.BuyerDelivery,c.Alias 
                                                         from ((Select a.Id as TransferToClogId, PackingListId, OrderId, CTNStartNo 
                                                                        from TransferToClog_Detail as a, TransferToClog as b 
                                                                        where a.Id <= '{0}' and a.Id = b.ID and b.FactoryID = '{1}') 
                                                                  except 
                                                                   (Select TransferToClogId, PackingListId, OrderId, CTNStartNo 
                                                                        from ClogReceive_Detail 
                                                                        where TransferToClogId <= '{0}')) 
                                                                  as a ", this.textBox2.Text.Trim(), Sci.Env.User.Factory);
                    selectCommand1 = selectCommand1 + "left join Orders b On a.OrderID = b.ID left join Country c On b.Dest = c.ID";
                    selectCommand2 = string.Format(@"Select count(TransferToClogId) as TTLCTN 
                                                                                from ((Select a.Id as TransferToClogId, PackingListId, OrderId, CTNStartNo 
                                                                                                from TransferToClog_Detail as a, TransferToClog as b 
                                                                                                where a.Id <= '{0}' and a.Id = b.ID and b.FactoryID = '{1}') 
                                                                                         except 
                                                                                          (Select a.TransferToClogId, a.PackingListId, a.OrderId, a.CTNStartNo 
                                                                                                from ClogReturn_Detail a, ClogReturn b 
                                                                                                where a.TransferToClogId <= '{0}' and a.Id = b.Id and b.Encode = 1)) 
                                                                                         as result", this.textBox2.Text.Trim(), Sci.Env.User.Factory);
                    selectCommand3 = string.Format(@"Select count(TransferToClogId) as ReceivedCTN 
                                                                                from ((Select a.TransferToClogId, a.PackingListId, a.OrderId, a.CTNStartNo 
                                                                                                from ClogReceive_Detail a, ClogReceive b 
                                                                                                where a.TransferToClogId <= '{0}' and a.Id = b.Id and b.Encode = 1 and b.FactoryID = '{1}') 
                                                                                        except 
                                                                                          (Select a.TransferToClogId, a.PackingListId, a.OrderId, a.CTNStartNo 
                                                                                                from ClogReturn_Detail a, ClogReturn b 
                                                                                                where a.TransferToClogId <= '{0}' and a.Id = b.Id and b.Encode = 1)) 
                                                                                        as result", this.textBox2.Text.Trim(), Sci.Env.User.Factory);
                }
            }

            DataTable selectDataTable1, selectDataTable2, selectDataTable3;
            DualResult selectResult;
            if (selectResult = DBProxy.Current.Select(null, selectCommand1, out selectDataTable1))
            {
                if (selectDataTable1.Rows.Count == 0)
                {
                    MessageBox.Show("Data not found!");
                }
            }

            allRecord = 0;
            foreach (DataRow dr in selectDataTable1.Rows)
            {
                allRecord = allRecord + 1;
            }
            bindingSource1.DataSource = selectDataTable1;
            if (selectResult = DBProxy.Current.Select(null, selectCommand2, out selectDataTable2))
            {
                this.numericBox1.Text = selectDataTable2.Rows[0]["TTLCTN"].ToString();
            }
            if (selectResult = DBProxy.Current.Select(null, selectCommand3, out selectDataTable3))
            {
                this.numericBox2.Text = selectDataTable3.Rows[0]["ReceivedCTN"].ToString();
            }
            this.numericBox3.Value = this.numericBox1.Value - this.numericBox2.Value;
            this.numericBox4.Value = 0;
        }

        //Update All Location
        private void button2_Click(object sender, EventArgs e)
        {
            string location = this.txtcloglocation1.Text.Trim();
            //this.grid1.GetSelectedRowIndex();
            int pos = this.bindingSource1.Position;
            DataTable dt = (DataTable)bindingSource1.DataSource;
            foreach (DataRow currentRecord in dt.Rows)
            {
                currentRecord["ClogLocationId"] = location;
            }
            this.bindingSource1.Position = pos;
            //this.grid1.Invalidate();
            //this.bindingSource1.ResetBindings(false);
            grid1.SuspendLayout();
            this.grid1.DataSource = null;
            this.grid1.DataSource = bindingSource1;
            this.bindingSource1.Position = pos;
            grid1.ResumeLayout();
        }

        //全選的CheckBox
        private void checkBox1_Click(object sender, EventArgs e)
        {
            if (null != col_chk)
            {
                this.grid1.SetCheckeds(col_chk);
                if (col_chk.Index == this.grid1.CurrentCellAddress.X)
                {
                    if (this.grid1.IsCurrentCellInEditMode)
                    {
                        this.grid1.RefreshEdit();
                    }
                    this.numericBox4.Value = allRecord;
                }
            }
        }

        //全不選的CheckBox
        private void checkBox2_Click(object sender, EventArgs e)
        {
            if (null != col_chk)
            {
                this.grid1.SetUncheckeds(col_chk);
                if (col_chk.Index == this.grid1.CurrentCellAddress.X)
                {
                    if (this.grid1.IsCurrentCellInEditMode)
                    {
                        this.grid1.RefreshEdit();
                    }
                }
                this.numericBox4.Value = 0;
            }
        }

        //Save，將有勾選的資料回寫回上一層的Detail
        private void button3_Click(object sender, EventArgs e)
        {
            this.grid1.ValidateControl();
            DataTable gridData = (DataTable)bindingSource1.DataSource;
            if (gridData.Rows.Count == 0)
            {
                MessageBox.Show("No data!");
                return;
            }

            DataRow[] dr = gridData.Select("Selected = 1");
            if (dr.Length > 0)
            {
                foreach (DataRow currentRow in dr)
                {
                    DataRow[] findrow = receiveDetailData.Select(string.Format("TransferToClogId = '{0}' and PackingListId = '{1}' and OrderId = '{2}' and CTNStartNo = '{3}'", currentRow["TransferToClogId"].ToString(), currentRow["PackingListId"].ToString(), currentRow["OrderId"].ToString(), currentRow["CTNStartNo"].ToString()));
                    if (findrow.Length == 0)
                    {
                        currentRow.AcceptChanges();
                        currentRow.SetAdded();
                        receiveDetailData.ImportRow(currentRow);
                    }
                }
            }
            //系統會自動有回傳值
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
