using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Class;
using Sci.Production.PublicPrg;
using Sci.Win.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Packing
{
    /// <summary>
    /// P03_RepackCartons
    /// </summary>
    public partial class P03_RepackCartons : Win.Tems.QueryForm
    {
        private DataTable dtMainDetail;
        private DataTable dtMiddleData;
        private DataTable dtNewPack;
        private DataRow drMaster;
        private DataRow drNewMaster;

        /// <summary>
        /// P03_RepackCartons
        /// </summary>
        /// <param name="master">master</param>
        /// <param name="mainDetail">mainDetail</param>
        public P03_RepackCartons(DataRow master, DataTable mainDetail)
        {
            this.InitializeComponent();
            this.dtMainDetail = mainDetail;
            this.drMaster = master;
            this.drNewMaster = master.Table.NewRow();
            this.dtMainDetail.ColumnsStringAdd("selected");

            this.EditMode = true;
            this.dtNewPack = this.dtMainDetail.Clone();
            this.dtMiddleData = this.dtMainDetail.Clone();
            this.displayBrand.Text = master["BrandID"].ToString();
            this.txtcustcd.Text = master["CustCDID"].ToString();
            this.txtDest.TextBox1.Text = master["Dest"].ToString();
            this.editRemark.Text = master["Remark"].ToString();
            this.txtshipmode.Text = master["ShipModeID"].ToString();
            this.DialogResult = DialogResult.No;
            this.gridNewPack.Sorted += this.Grid_Sorted;
            this.gridOriPack.Sorted += this.Grid_Sorted;
        }

        private void Grid_Sorted(object sender, EventArgs e)
        {
            this.GridContentChanged();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.GridSet();
            this.gridOriPack.DataSource = this.dtMainDetail;
            this.gridNewPack.DataSource = this.dtNewPack;
            this.txtshipmode.Text = this.drMaster["ShipModeID"].ToString();
            this.GridContentChanged();
        }

        private void GridSet()
        {
            this.Helper.Controls.Grid.Generator(this.gridOriPack)
                .CheckBox("selected", header: string.Empty, falseValue: string.Empty, trueValue: "Y", width: Widths.AnsiChars(2))
                .Text("OrderID", header: "SP No.", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("SeasonID", header: "Season", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("OrderShipmodeSeq", header: "Seq", width: Widths.AnsiChars(2), iseditingreadonly: true)
                .Text("StyleID", header: "Style No.", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("CustPONo", header: "P.O. No.", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("CTNStartNo", header: "CTN#", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Numeric("CTNQty", header: "# of CTN", iseditingreadonly: true)
                .Text("Article", header: "ColorWay", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("Color", header: "Color", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Numeric("QtyPerCTN", header: "PC/Ctn", iseditingreadonly: true)
                .Numeric("ShipQty", header: "Qty", iseditingreadonly: true);

            #region seq 右鍵開窗
            DataGridViewGeneratorTextColumnSettings seq = new DataGridViewGeneratorTextColumnSettings();
            seq.EditingMouseDown += (s, e) =>
            {
                if (e.Button != System.Windows.Forms.MouseButtons.Right ||
                    e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.gridNewPack.GetDataRow<DataRow>(e.RowIndex);

                DataTable dtOrderShipmodeSeq = this.GetOrderShipmodeSeq(dr["OrderID"].ToString());
                if (dtOrderShipmodeSeq == null)
                {
                    return;
                }

                Win.Tools.SelectItem item = new Win.Tools.SelectItem(dtOrderShipmodeSeq, "ID,Seq,BuyerDelivery,ShipmodeID,Qty", "15,4,20,20,10", string.Empty);
                DialogResult returnResult = item.ShowDialog();
                if (returnResult == DialogResult.Cancel)
                {
                    return;
                }

                e.EditingControl.Text = item.GetSelecteds()[0]["Seq"].ToString();
                dr["OrderShipmodeSeq"] = e.EditingControl.Text;
            };
            #endregion

            this.Helper.Controls.Grid.Generator(this.gridNewPack)
                .CheckBox("selected", header: string.Empty, falseValue: string.Empty, trueValue: "Y", width: Widths.AnsiChars(2))
                .Text("OrderID", header: "SP No.", width: Widths.AnsiChars(15))
                .Text("SeasonID", header: "Season", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("OrderShipmodeSeq", header: "Seq", width: Widths.AnsiChars(2), settings: seq)
                .Text("StyleID", header: "Style No.", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("CustPONo", header: "P.O. No.", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("CTNStartNo", header: "CTN#", width: Widths.AnsiChars(6))
                .Numeric("CTNQty", header: "# of CTN", iseditingreadonly: true)
                .Text("Article", header: "ColorWay", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("Color", header: "Color", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Numeric("QtyPerCTN", header: "PC/Ctn", iseditingreadonly: true)
                .Numeric("ShipQty", header: "Qty", iseditingreadonly: true);

            this.gridNewPack.Columns["OrderID"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridNewPack.Columns["OrderShipmodeSeq"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridNewPack.Columns["CTNStartNo"].DefaultCellStyle.BackColor = Color.Pink;
        }

        private void GridOriPack_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != 0)
            {
                return;
            }

            if (e.RowIndex > 1)
            {
                e.AdvancedBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
            }

            if (this.IsTheSameCellValue(e.RowIndex, (Grid)sender))
            {
                e.AdvancedBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.None;
            }
            else
            {
                e.AdvancedBorderStyle.Bottom = this.gridOriPack.AdvancedCellBorderStyle.Bottom;
            }
        }

        private bool IsTheSameCellValue(int row, Grid srcGrid)
        {
            if (row == srcGrid.Rows.Count - 1)
            {
                return false;
            }

            DataGridViewCell cell1 = srcGrid["CTNStartNo", row];
            DataGridViewCell cell2 = srcGrid["CTNStartNo", row + 1];
            if (cell1.Value == null || cell2.Value == null)
            {
                return false;
            }

            return cell1.Value.ToString() == cell2.Value.ToString();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnSelect_Click(object sender, EventArgs e)
        {
            bool isConditionEmpty = MyUtility.Check.Empty(this.numSelectCtnFrom.Text) && MyUtility.Check.Empty(this.numSelectCtnTo.Text);
            int cTNStartNo = 0;
            int selectCtnFrom = 0;
            int selectCtnTo = 0;
            bool isInt = false;
            if (isConditionEmpty)
            {
                MyUtility.Msg.WarningBox("Please enter at least one field <Select Original CTN#>");
                return;
            }

            if (!MyUtility.Check.Empty(this.numSelectCtnFrom.Text))
            {
                selectCtnFrom = (int)this.numSelectCtnFrom.Value;
            }
            else
            {
                selectCtnFrom = 0;
            }

            if (!MyUtility.Check.Empty(this.numSelectCtnTo.Text))
            {
                selectCtnTo = (int)this.numSelectCtnTo.Value;
            }
            else
            {
                selectCtnTo = 999999;
            }

            foreach (DataRow dr in this.dtMainDetail.Rows)
            {
                isInt = int.TryParse(dr["CTNStartNo"].ToString(), out cTNStartNo);
                if (!isInt || dr["CTNQty"].ToString() == "0")
                {
                    continue;
                }

                if (cTNStartNo >= selectCtnFrom && cTNStartNo <= selectCtnTo)
                {
                    dr["selected"] = "Y";
                }
            }
        }

        private void BtnDown_Click(object sender, EventArgs e)
        {
            this.gridOriPack.ValidateControl();
            var listDown = this.dtMainDetail.AsEnumerable().Where(s => s["selected"].ToString() == "Y").Select(s => s["CTNStartNo"]).ToList();

            if (!listDown.Any())
            {
                return;
            }

            this.gridNewPack.DataSource = null;

            foreach (var item in listDown)
            {
                DataRow[] listDownRow = this.dtMainDetail.Select($"CTNStartNo = '{item}'");
                foreach (DataRow drDown in listDownRow)
                {
                    drDown["OrigID"] = drDown["ID"];
                    drDown["OrigOrderID"] = drDown["OrderID"];
                    drDown["OrigCTNStartNo"] = drDown["CTNStartNo"];
                    this.dtNewPack.ImportRow(drDown);
                    this.dtMiddleData.ImportRow(drDown);
                    this.dtMainDetail.Rows.Remove(drDown);
                }
            }

            this.gridNewPack.DataSource = this.dtNewPack;
            this.GridContentChanged();
            this.SetTxtSeqFromShipmode();
        }

        private void BtnUp_Click(object sender, EventArgs e)
        {
            var listUp = this.dtNewPack.AsEnumerable().Where(s => s["selected"].ToString() == "Y")
                             .Select(s => new
                             {
                                 OriCTNStartNo = s["CTNStartNo", DataRowVersion.Original],
                                 NewCTNStartNo = s["CTNStartNo"],
                             }).ToList();

            if (!listUp.Any())
            {
                return;
            }

            foreach (var item in listUp)
            {
                DataRow[] listUpRow = this.dtMiddleData.Select($"CTNStartNo = '{item.OriCTNStartNo}'");
                foreach (DataRow drUp in listUpRow)
                {
                    drUp["selected"] = string.Empty;
                    this.dtMainDetail.ImportRow(drUp);
                    this.dtMiddleData.Rows.Remove(drUp);
                }

                DataRow[] listRemoveNew = this.dtNewPack.Select($"CTNStartNo = '{item.NewCTNStartNo}'");
                foreach (DataRow drRemoveNew in listRemoveNew)
                {
                    this.dtNewPack.Rows.Remove(drRemoveNew);
                }
            }

            this.dtMainDetail = this.dtMainDetail.AsEnumerable().OrderBy(s => s["Seq"]).CopyToDataTable();
            this.gridOriPack.DataSource = this.dtMainDetail;
            this.GridContentChanged();
        }

        private void SetGridProperty(ref MergeRowGrid grid)
        {
            foreach (DataGridViewColumn col in grid.Columns)
            {
                if (col.Name != "CTNStartNo")
                {
                    col.SortMode = DataGridViewColumnSortMode.NotSortable;
                }
            }

            foreach (DataGridViewRow dr in grid.Rows)
            {
                bool isNotMainCtn = dr.Cells["CTNQty"].Value.ToString() == "0";
                if (isNotMainCtn)
                {
                    dr.Cells["selected"] = new DataGridViewTextBoxCell();
                }
            }
        }

        private void GridContentChanged()
        {
            this.SetGridProperty(ref this.gridNewPack);
            this.SetGridProperty(ref this.gridOriPack);
        }

        private void TxtSeq_PopUp(object sender, TextBoxPopUpEventArgs e)
        {
            if (this.dtNewPack.Rows.Count == 0)
            {
                return;
            }

            string orderid = this.dtNewPack.Rows[0]["OrderID"].ToString();

            DataTable dtOrderShipmodeSeq = this.GetOrderShipmodeSeq(orderid);
            if (dtOrderShipmodeSeq == null)
            {
                return;
            }

            Win.Tools.SelectItem item = new Win.Tools.SelectItem(dtOrderShipmodeSeq, "ID,Seq,BuyerDelivery,ShipmodeID,Qty", "15,4,20,20,10", string.Empty);
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.txtSeq.Text = item.GetSelecteds()[0]["Seq"].ToString();
        }

        private void Txtshipmode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.dtNewPack.Rows.Count == 0)
            {
                return;
            }

            this.SetTxtSeqFromShipmode();
        }

        private void SetTxtSeqFromShipmode()
        {
            string orderid = this.dtNewPack.Rows[0]["OrderID"].ToString();
            DataTable dtOrderShipmodeSeq = this.GetOrderShipmodeSeq(orderid);
            if (dtOrderShipmodeSeq == null)
            {
                return;
            }

            if (dtOrderShipmodeSeq.Rows.Count == 1)
            {
                this.txtSeq.Text = dtOrderShipmodeSeq.Rows[0]["Seq"].ToString();
            }
        }

        private DataTable GetOrderShipmodeSeq(string orderID)
        {
            string sqlCmd = string.Format(
                              @"
select  oq.ID,oq.Seq
        , oq.BuyerDelivery
        , oq.ShipmodeID
        , oq.Qty 
from Order_QtyShip oq WITH (NOLOCK) 
inner join orders o WITH (NOLOCK) on oq.id = o.id  
where   oq.ID = '{0}' 
        and oq.ShipmodeID = '{1}' 
        and o.MDivisionID = '{2}'",
                              orderID,
                              this.txtshipmode.Text,
                              Sci.Env.User.Keyword);
            DataTable dtResult;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out dtResult);
            if (!result)
            {
                this.ShowErr(result);
            }

            return dtResult;
        }

        private void BtnUpdateNewCtn_Click(object sender, EventArgs e)
        {
            decimal? ctnNo = this.numStartFromCtn.Value - 1;
            foreach (DataRow dr in this.dtNewPack.Rows)
            {
                if (dr["CTNQty"].ToString() == "1")
                {
                    ctnNo++;
                }

                dr["OrderShipmodeSeq"] = this.txtSeq.Text;
                dr["CTNStartNo"] = ctnNo.ToString();
            }
        }

        private void BtnRepack_Click(object sender, EventArgs e)
        {
            var listSelectedNewPack = this.dtNewPack.AsEnumerable().Where(s => s["selected"].ToString() == "Y").Select(s => s["CTNStartNo"]).ToList();

            if (!listSelectedNewPack.Any())
            {
                MyUtility.Msg.WarningBox("Please select one Repack Carton");
                return;
            }

            DataTable dtSelectedNewPack = this.dtNewPack.Clone();
            foreach (var item in listSelectedNewPack)
            {
                DataRow[] listNewPack = this.dtNewPack.Select($"CTNStartNo = '{item}'");
                foreach (DataRow drUp in listNewPack)
                {
                    dtSelectedNewPack.ImportRow(drUp);
                }
            }

            string sqlUpdatePackingList = string.Empty;

            #region 產生New表頭資料 Sql
            List<SqlParameter> listSqlPar = new List<SqlParameter>();
            string newPackID = MyUtility.GetValue.GetID(Sci.Env.User.Keyword + "PL", "PackingList", DateTime.Today, 2, "Id", null);
            if (MyUtility.Check.Empty(newPackID))
            {
                MyUtility.Msg.WarningBox("GetID fail, please try again!");
                return;
            }

            // 存檔前檢查
            this.drNewMaster["ID"] = newPackID;
            this.drNewMaster["CustCDID"] = this.txtcustcd.Text;
            bool isNewSavecheckOK = Prgs.P03SaveCheck(this.drNewMaster, dtSelectedNewPack, null);
            if (!isNewSavecheckOK)
            {
                return;
            }

            string type = "B";
            string M = Sci.Env.User.Keyword;
            string factoryID = Sci.Env.User.Factory;
            string status = "New";
            listSqlPar.Add(new SqlParameter("@EstCTNBooking", this.drMaster["EstCTNBooking"]));
            listSqlPar.Add(new SqlParameter("@EstCTNArrive", this.drMaster["EstCTNArrive"]));

            bool isOriPackHasLocalPO = MyUtility.Check.Seek($"select 1 from LocalPo_Detail with (nolock) where RequestID = '{this.drMaster["ID"]}' ");
            string localPOID = isOriPackHasLocalPO ? "Y" : string.Empty;

            sqlUpdatePackingList = $@"
insert into PackingList(ID,Type,MDivisionID,FactoryID,ShipModeID,BrandID,Dest,CustCDID,CTNQty,ShipQty,NW,GW,NNW,CBM,Remark,EstCTNBooking,EstCTNArrive,LocalPOID,Status,AddName,AddDate,TransFerToClogID,ClogReceiveID,QueryDate)
        values('{newPackID}','{type}','{M}','{factoryID}','{this.txtshipmode.Text}','{this.displayBrand.Text}','{this.txtDest.TextBox1.Text}',
                '{this.txtcustcd.Text}',{this.drNewMaster["CTNQty"]},{this.drNewMaster["ShipQty"]},{this.drNewMaster["NW"]},{this.drNewMaster["GW"]},{this.drNewMaster["NNW"]},
                {this.drNewMaster["CBM"]},'{this.editRemark.Text}',@EstCTNBooking,@EstCTNArrive,'{localPOID}','{status}','{Env.User.UserID}',GETDATE(),'{this.drMaster["TransFerToClogID"]}','{this.drMaster["ClogReceiveID"]}',GETDATE())
";

            #endregion

            #region 產生更新原表頭資料sql並作資料檢查
            bool isOriSaveCheckOK = Prgs.P03SaveCheck(this.drMaster, this.dtMainDetail, null);
            if (!isOriSaveCheckOK)
            {
                return;
            }

            sqlUpdatePackingList += $@"
update PackingList set  CTNQty = {this.drMaster["CTNQty"]},
                        ShipQty = {this.drMaster["ShipQty"]},
                        NW = {this.drMaster["NW"]},
                        GW = {this.drMaster["GW"]},
                        NNW = {this.drMaster["NNW"]},
                        CBM = {this.drMaster["CBM"]},
                        EditName = '{Env.User.UserID}',
                        EditDate = GETDATE()
                where ID = '{this.drMaster["ID"]}'
";
            #endregion

            #region 產生更新detail sql

            foreach (DataRow dr in dtSelectedNewPack.Rows)
            {
                sqlUpdatePackingList += $@"
update PackingList_Detail set   ID = '{newPackID}',
                                OrderID = '{dr["OrderID"]}',
                                OrderShipmodeSeq = '{dr["OrderShipmodeSeq"]}',
                                CTNStartNo = '{dr["CTNStartNo"]}',
                                Seq = '{dr["Seq"]}',
                                OrigID = '{dr["OrigID"]}',
                                OrigOrderID = '{dr["OrigOrderID"]}',
                                OrigCTNStartNo = '{dr["OrigCTNStartNo"]}'
where SCICtnNo = '{dr["SCICtnNo"]}'
";
            }
            #endregion

            #region 更新回資料庫
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    DualResult result;
                    result = DBProxy.Current.Execute(null, sqlUpdatePackingList, listSqlPar);
                    if (!result)
                    {
                        transactionScope.Dispose();
                        this.ShowErr(result);
                        return;
                    }

                    // update GMTBooking
                    result = Prgs.P03_UpdateGMT(this.drMaster, this.dtMainDetail);
                    if (!result)
                    {
                        transactionScope.Dispose();
                        return;
                    }

                    // update others
                    result = Prgs.P03_UpdateOthers(this.drMaster);
                    if (!result)
                    {
                        transactionScope.Dispose();
                        return;
                    }

                    result = Prgs.P03_UpdateOthers(this.drNewMaster);
                    if (!result)
                    {
                        transactionScope.Dispose();
                        return;
                    }
                }
                catch (Exception ex)
                {
                    transactionScope.Dispose();
                    this.ShowErr(ex);
                    return;
                }

                transactionScope.Complete();
                MyUtility.Msg.InfoBox("Repack success.\r\n New PackID:" + newPackID);
                this.DialogResult = DialogResult.Yes;
                this.Close();
            }
            #endregion
        }
    }
}
