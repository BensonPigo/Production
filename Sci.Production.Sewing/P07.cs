﻿using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.PublicPrg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Sewing
{
    public partial class P07 : Sci.Win.Tems.QueryForm
    {
        public P07(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.EditMode = true;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            Ict.Win.UI.DataGridViewComboBoxColumn cbb_TransferTo;
            this.gridTransfer.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.gridTransfer)
                .CheckBox("selected", header: string.Empty, width: Widths.AnsiChars(15), iseditable: true, trueValue: 1, falseValue: 0)
                .Text("ID", header: "Pack ID", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("CTNStartNo", header: "CTN#", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("OrderIdlist", header: "SP#", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("CustPoNo", header: "PO#", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("StyleID", header: "Style", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("SeasonID", header: "Season", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("BrandID", header: "Brand", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("Alias", header: "Destination", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Date("BuyerDelivery", header: "Buyer Delivery", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .ComboBox("TransferTo", header: "Transfer to", width: Widths.AnsiChars(11)).Get(out cbb_TransferTo)
                .Text("Remark", header: "Remark", width: Widths.AnsiChars(25), iseditingreadonly: true);

            DataTable cbSrc;
            DBProxy.Current.Select(null, "select ID,Name from DropDownList where type = 'Pms_DRYTransferTo'", out cbSrc);
            cbb_TransferTo.DataSource = cbSrc;
            cbb_TransferTo.ValueMember = "ID";
            cbb_TransferTo.DisplayMember = "Name";

            this.gridTransfer.Columns["TransferTo"].DefaultCellStyle.BackColor = Color.Pink;

            foreach (DataGridViewColumn col in this.gridTransfer.Columns)
            {
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            string sqlCmd = $@"
select 
[selected] = 0,
pd.ID,
pd.CTNStartNo,
[OrderIdlist] = '',
pd.OrderID,
o.CustPONo,
o.StyleID,
o.BrandID,
o.SeasonID,
c.Alias,
o.BuyerDelivery,
[TransferTo] = '',
pd.Remark,
pd.TransferDate,
pd.DRYReceiveDate,
pu.Status,
pd.SCICtnNo
from  Orders o with (nolock)
left join PackingList_Detail pd with (nolock) on pd.OrderID = o.ID
left join PackingList p with (nolock) on p.id = pd.ID
left join Pullout pu with (nolock) on p.PulloutID = pu.ID
left join Country c with (nolock) on o.Dest = c.ID 
where	1 = 0";
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out this.dtTransfer);
            this.gridTransfer.DataSource = this.dtTransfer;
        }

        DataTable dtTransfer = new DataTable();

        private void BtnFind_Click(object sender, EventArgs e)
        {
            List<SqlParameter> sqlPar = new List<SqlParameter>();
            sqlPar.Add(new SqlParameter("@M", Env.User.Keyword));

            string addWhere = string.Empty;

            if (!MyUtility.Check.Empty(this.txtPackID.Text))
            {
                addWhere += " and pd.ID = @PackID ";
                sqlPar.Add(new SqlParameter("@PackID", this.txtPackID.Text));
            }

            if (!MyUtility.Check.Empty(this.txtSP.Text))
            {
                addWhere += " and pd.OrderID = @OrderID ";
                sqlPar.Add(new SqlParameter("@OrderID", this.txtSP.Text));
            }

            if (!MyUtility.Check.Empty(this.txtPO.Text))
            {
                addWhere += " and o.CustPONo = @CustPoNo ";
                sqlPar.Add(new SqlParameter("@CustPoNo", this.txtPO.Text));
            }

            if (this.dateRangeReceive.HasValue)
            {
                addWhere += " and pd.DRYReceiveDate  between @fromDt and @toDt ";
                sqlPar.Add(new SqlParameter("@fromDt", this.dateRangeReceive.DateBox1.Value));
                sqlPar.Add(new SqlParameter("@toDt", this.dateRangeReceive.DateBox2.Value));
            }

            string sqlCmd = $@"
select 
[selected] = 0,
pd.ID,
pd.CTNStartNo,
[OrderIdlist] = Stuff((select distinct concat( '/',OrderID)   from PackingList_Detail with (nolock) where ID = pd.ID and CTNStartNo = pd.CTNStartNo and DisposeFromClog= 0  FOR XML PATH('')),1,1,'') ,
pd.OrderID,
o.CustPONo,
o.StyleID,
o.BrandID,
o.SeasonID,
c.Alias,
o.BuyerDelivery,
[TransferTo] = '',
pd.Remark,
pd.TransferDate,
pd.DRYReceiveDate,
pu.Status,
pd.SCICtnNo
from  Orders o with (nolock)
left join PackingList_Detail pd with (nolock) on pd.OrderID = o.ID 
left join PackingList p with (nolock) on p.id = pd.ID
left join Pullout pu with (nolock) on p.PulloutID = pu.ID
left join Country c with (nolock) on o.Dest = c.ID 
where	pd.CTNStartNo != '' and 
		p.MDivisionID =@M and
        pd.CTNQty = 1 and
        pd.DisposeFromClog= 0 and
		p.Type in ('B','L') and
		pd.TransferDate  is null and
		pd.DRYReceiveDate  is not null and 
		(pu.Status = 'New' or pu.Status is null)
" + addWhere;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, sqlPar, out this.dtTransfer);

            if (result == false)
            {
                this.ShowErr(result);
                return;
            }

            if (this.dtTransfer.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("No data Found!");
                return;
            }

            this.gridTransfer.DataSource = this.dtTransfer;
        }

        private void TxtScanBarcode_Validating(object sender, CancelEventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtScanBarcode.Text))
            {
                return;
            }

            PackDataResult packDataResult = new PackDataResult();
            packDataResult = this.GetPackData(this.txtScanBarcode.Text);
            if (packDataResult.result == true)
            {
                if (this.dtTransfer.AsEnumerable().Any(s => s["ID"].Equals(packDataResult.Dr["ID"]) && s["CTNStartNo"].Equals(packDataResult.Dr["CTNStartNo"])))
                {
                    this.txtScanBarcode.Text = string.Empty;
                    e.Cancel = true;
                    return;
                }

                packDataResult.Dr.Table.Merge(this.dtTransfer);
                this.dtTransfer = packDataResult.Dr.Table;
                this.gridTransfer.DataSource = this.dtTransfer;

            }
            else
            {
                MyUtility.Msg.WarningBox(packDataResult.errMsg);
            }

            this.txtScanBarcode.Text = string.Empty;
            e.Cancel = true;
        }

        private class PackDataResult
        {
            public DataRow Dr;
            public bool result;
            public string errMsg;
        }

        private PackDataResult GetPackData(string PackNo, bool fromCustCTN = false)
        {
            string keyWhere = string.Empty;
            string PackingListID = string.Empty;
            string CTNStarNo = string.Empty;

            if (PackNo.Length > 13)
            {
                PackingListID = PackNo.Substring(0, 13);
                CTNStarNo = PackNo.Substring(13, PackNo.Length - 13);
            }

            if (fromCustCTN == true)
            {
                keyWhere = $"CustCTN = '{PackNo}'";
            }
            else
            {
                //keyWhere = $"(ID+CTNStartNo) = '{PackNo}'";

                if (PackNo.Length > 13)
                {
                    keyWhere = $" ID = '{PackingListID}' AND CTNStartNo ='{CTNStarNo}' ";
                }
                else
                {
                    keyWhere = $"(ID+CTNStartNo) = '{PackNo}'";
                }
            }

            PackDataResult packDataResult = new PackDataResult();
            string chkSql = $@"
select 
[selected] = 1,
pd.ID,
pd.CTNStartNo,
[OrderIdlist] = Stuff((select distinct concat( '/',OrderID)   from PackingList_Detail with (nolock) where {keyWhere} and DisposeFromClog= 0 FOR XML PATH('')),1,1,'') ,
pd.OrderID,
o.CustPONo,
o.StyleID,
o.BrandID,
o.SeasonID,
c.Alias,
o.BuyerDelivery,
[TransferTo] = '',
pd.Remark,
pd.TransferDate,
pd.DRYReceiveDate,
pu.Status,
pd.SCICtnNo
from  (select * from PackingList_Detail with (nolock) where {keyWhere} and CTNQty = 1 and DisposeFromClog= 0) pd
inner join Orders o with (nolock) on pd.OrderID = o.ID
left join PackingList p with (nolock) on p.id = pd.ID
left join Pullout pu with (nolock) on p.PulloutID = pu.ID
left join Country c with (nolock) on o.Dest = c.ID 
where	pd.CTNStartNo != '' and 
		p.MDivisionID = '{Env.User.Keyword}' and
		p.Type in ('B','L') ";

            bool checkBarcode = MyUtility.Check.Seek(chkSql, out packDataResult.Dr);
            packDataResult.result = false;
            if (checkBarcode == false)
            {
                packDataResult.errMsg = $"<CNT#:{PackNo}> does not exist!";
                return packDataResult;
            }

            if (MyUtility.Check.Empty(packDataResult.Dr["DRYReceiveDate"]))
            {
                packDataResult.errMsg = $"<CNT#:{PackNo}> This CTN# Dehumidifying Room not yet received.";
                this.txtScanBarcode.Focus();
                return packDataResult;
            }

            if (packDataResult.Dr["Status"].Equals("Confirmed") || packDataResult.Dr["Status"].Equals("Locked"))
            {
                packDataResult.errMsg = $"<CNT#:{PackNo}> Already pullout!";
                return packDataResult;
            }

            packDataResult.result = true;
            return packDataResult;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnImportFromBarcode_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "txt files (*.txt)|*.txt";

            // 開窗且有選擇檔案
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string file = openFileDialog1.FileName;
                using (StreamReader reader = new StreamReader(MyUtility.Convert.GetString(file), System.Text.Encoding.UTF8))
                {
                    this.ShowWaitMessage("Processing....");
                    string line;
                    try
                    {
                        string[] splitResult;

                        // 檢查第1碼是否都為2
                        while ((line = reader.ReadLine()) != null)
                        {
                            splitResult = line.Split('\t');
                            if (!splitResult[0].Trim().Equals("1"))
                            {
                                MyUtility.Msg.WarningBox("Format is not correct!");
                                return;
                            }
                        }

                        // 讀取資料
                        DataTable tmpDetail = this.dtTransfer.Clone();
                        PackDataResult packDataResult = new PackDataResult();
                        string packNo = string.Empty;
                        reader.DiscardBufferedData();
                        reader.BaseStream.Seek(0, System.IO.SeekOrigin.Begin);
                        while ((line = reader.ReadLine()) != null)
                        {
                            packNo = line.Split('\t')[1].Trim();

                            // 檢查PackingList_Detail.ID + PackingList_Detail.CustCTN
                            packDataResult = this.GetPackData(packNo, false);
                            if (packDataResult.result == true)
                            {
                                tmpDetail.Rows.Add(packDataResult.Dr.ItemArray);
                                continue;
                            }

                            // 檢查PackingList_Detail.CustCTN
                            packDataResult = this.GetPackData(packNo, true);
                            if (packDataResult.result == true)
                            {
                                tmpDetail.Rows.Add(packDataResult.Dr.ItemArray);
                                continue;
                            }
                        }

                        // 將tmpDetail 匯入 dtTransfer
                        tmpDetail.Merge(this.dtTransfer);
                        this.dtTransfer = tmpDetail;
                        this.gridTransfer.DataSource = this.dtTransfer;
                    }
                    catch (Exception err)
                    {
                        this.ShowErr(err);
                    }

                    this.HideWaitMessage();
                }
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!this.dtTransfer.AsEnumerable().Any(s => (int)s["selected"] == 1))
            {
                MyUtility.Msg.WarningBox("Please select detail data first");
                return;
            }

            this.ShowWaitMessage("Save Processing....");

            // 檢查資料
            PackDataResult packDataResult = new PackDataResult();
            string updSql = string.Empty;
            var checkData = this.dtTransfer.AsEnumerable().Where(s => (int)s["selected"] == 1).ToList();
            foreach (var item in checkData)
            {
                packDataResult = this.GetPackData(item["ID"].ToString() + item["CTNStartNo"].ToString());
                if (packDataResult.result == false)
                {
                    MyUtility.Msg.WarningBox(packDataResult.errMsg);
                    this.HideWaitMessage();
                    return;
                }

                if (MyUtility.Check.Empty(item["TransferTo"]))
                {
                    MyUtility.Msg.WarningBox($"Msg: <{item["ID"].ToString() + item["CTNStartNo"].ToString()}> Must Key-in Transfer to!!");
                    this.HideWaitMessage();
                    return;
                }
            }

            // save data
            DualResult result;
            foreach (var item in checkData)
            {
                updSql = $@"
update PackingList_Detail set DRYReceiveDate = null where ID = '{item["ID"]}' 
and CTNStartNo = '{item["CTNStartNo"]}' and DisposeFromClog= 0;
insert into DRYTransfer(TransferDate, MDivisionID, OrderID, PackingListID, CTNStartNo, TransferTo, AddName, AddDate, SCICtnNo)
            values(GETDATE(),'{Env.User.Keyword}','{item["OrderID"]}','{item["ID"]}','{item["CTNStartNo"]}', '{item["TransferTo"]}','{Env.User.UserID}',GETDATE(),'{item["SCICtnNo"]}');
";
                result = DBProxy.Current.Execute(null, updSql);
                if (result == false)
                {
                    this.ShowErr(result);
                    this.HideWaitMessage();
                    return;
                }

                Prgs.UpdateOrdersCTN(item["OrderID"].ToString());
            }

            foreach (DataRow item in checkData)
            {
                this.dtTransfer.Rows.Remove(item);
            }

            MyUtility.Msg.InfoBox("Save successfully");
            this.HideWaitMessage();
        }

        private void BtnUpdateAll_Click(object sender, EventArgs e)
        {
            var selectDr = this.dtTransfer.AsEnumerable().Where(s => (int)s["selected"] == 1).ToList();
            foreach (DataRow item in selectDr)
            {
                item["TransferTo"] = this.comboTransferTo.SelectedValue;
            }

        }
    }
}
