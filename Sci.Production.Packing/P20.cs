﻿using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Win.Tems;
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
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Packing
{
    /// <summary>
    /// P20
    /// </summary>
    public partial class P20 : QueryForm
    {
        private DataTable dtPackErrTransfer;
        private string mainPackQuerySql = $@"
set ARITHABORT on
select
[selected] = 0,
pd.ID,
pd.CTNStartNo,
[OrderID] = Stuff((select distinct concat( '/',OrderID)   from PackingList_Detail where ID = pd.ID and CTNStartNo = pd.CTNStartNo  FOR XML PATH('')),1,1,''),
o.CustPONo,
o.StyleID,
o.SeasonID,
o.BrandID,
c.Alias,
o.BuyerDelivery,
pd.Remark,
pd.TransferDate,
pd.DRYReceiveDate,
pd.PackErrTransferDate,
pu.Status,
[MainSP] = pd.OrderID
from PackingList_Detail pd with (nolock)
inner join PackingList p with (nolock) on pd.ID = p.ID
left join Orders o with (nolock) on o.ID = pd.OrderID
left join Country c with (nolock) on c.ID = o.Dest
left join Pullout pu with (nolock) on pu.ID = p.PulloutID
 ";

        /// <summary>
        /// P20
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P20(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.EditMode = true;
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.Helper.Controls.Grid.Generator(this.gridPackErrTransfer)
           .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(2), iseditable: true, trueValue: 1, falseValue: 0)
           .Text("ID", header: "Pack ID", width: Widths.AnsiChars(16), iseditingreadonly: true)
           .Text("CTNStartNo", header: "CTN#", width: Widths.AnsiChars(8), iseditingreadonly: true)
           .Text("OrderID", header: "SP#", width: Widths.AnsiChars(16), iseditingreadonly: true)
           .Text("CustPoNo", header: "PO#", width: Widths.AnsiChars(16), iseditingreadonly: true)
           .Text("StyleID", header: "Style", width: Widths.AnsiChars(16), iseditingreadonly: true)
           .Text("SeasonID", header: "Season", width: Widths.AnsiChars(12), iseditingreadonly: true)
           .Text("BrandID", header: "Brand", width: Widths.AnsiChars(8), iseditingreadonly: true)
           .Text("Alias", header: "Destination", width: Widths.AnsiChars(20), iseditingreadonly: true)
           .Date("BuyerDelivery", header: "Buyer Delivery")
           .Text("Remark", header: "Remark", width: Widths.AnsiChars(20), iseditingreadonly: true);

            string emptySql = this.mainPackQuerySql + " where 1 = 0";
            DualResult result = DBProxy.Current.Select(null, emptySql, out this.dtPackErrTransfer);
            this.gridPackErrTransfer.DataSource = this.dtPackErrTransfer;
        }

        private void BtnFind_Click(object sender, EventArgs e)
        {
            this.QueryData();

            if (this.dtPackErrTransfer.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("No Data Found!");
                return;
            }
        }

        private void TxtScanBarcode_Validating(object sender, CancelEventArgs e)
        {
            string scannedBarcode = this.txtScanBarcode.Text;
            this.txtScanBarcode.Text = string.Empty;

            if (MyUtility.Check.Empty(scannedBarcode))
            {
                return;
            }

            if (scannedBarcode.Length < 14)
            {
                MyUtility.Msg.WarningBox($"<Barcode>{scannedBarcode} format wrong");
                e.Cancel = true;
                return;
            }

            string packID = scannedBarcode.Substring(0, 13);
            string cartonStartNo = scannedBarcode.Substring(13);

            foreach (DataGridViewRow dr in this.gridPackErrTransfer.Rows)
            {
                if (dr.Cells["ID"].Value.Equals(packID) && dr.Cells["CTNStartNo"].Value.Equals(cartonStartNo))
                {
                    this.gridPackErrTransfer.SelectRowTo(dr.Index);
                    MyUtility.Msg.WarningBox($"<Barcode>{scannedBarcode} already scanned");
                    e.Cancel = true;
                    return;
                }
            }

            CheckPackResult checkPackResult = this.CheckPackID(packID, cartonStartNo);

            if (checkPackResult.IsOK)
            {
                this.dtPackErrTransfer.ImportRow(checkPackResult.DrResult);
            }

            e.Cancel = true;
        }

        private void BtnImportBarcode_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "txt files (*.txt)|*.txt";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string importFileName = openFileDialog.FileName;
                List<DataRow> listNewRow = new List<DataRow>();
                using (StreamReader reader = new StreamReader(importFileName, System.Text.Encoding.UTF8))
                {
                    this.ShowWaitMessage("Processing....");
                    string line;
                    try
                    {
                        string forsizesplit = string.Empty;
                        while ((line = reader.ReadLine()) != null)
                        {
                            string[] splitResult = line.Split('\t');
                            if (splitResult.Length != 5)
                            {
                                MyUtility.Msg.WarningBox("Format is not correct!");
                                this.HideWaitMessage();
                                return;
                            }

                            string checkCode = splitResult[0].TrimEnd();

                            if (checkCode != "1")
                            {
                                MyUtility.Msg.WarningBox("Format is not correct!");
                                this.HideWaitMessage();
                                return;
                            }

                            string packID = splitResult[2].Substring(0, 13).TrimEnd();
                            string cartonStartNo = splitResult[2].Substring(13).TrimEnd();

                            CheckPackResult checkPackResult = this.CheckPackID(packID, cartonStartNo);

                           if (checkPackResult.IsOK)
                            {
                                this.dtPackErrTransfer.ImportRow(checkPackResult.DrResult);
                            }
                            else
                            {
                                this.HideWaitMessage();
                                return;
                            }

                            checkPackResult.DrResult["selected"] = 1;
                            listNewRow.Add(checkPackResult.DrResult);
                        }

                        foreach (DataRow dr in listNewRow)
                        {
                            this.dtPackErrTransfer.Rows.Add(dr);
                        }
                    }
                    catch (Exception ex)
                    {
                        MyUtility.Msg.WarningBox("Error Import File:" + Environment.NewLine + ex.Message);
                    }

                    this.HideWaitMessage();
                }
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            this.ShowWaitMessage("Save Processing....");
            // 檢查資料
            var drSelected = this.dtPackErrTransfer.AsEnumerable().Where(s => (int)s["selected"] == 1).ToList();

            foreach (DataRow dr in drSelected)
            {
                bool isOkPackData = this.CheckPackID(dr["ID"].ToString(), dr["CTNStartNo"].ToString()).IsOK;
                if (!isOkPackData)
                {
                    return;
                }
            }

            string saveSql = string.Empty;
            DualResult updateResult;
            using (TransactionScope updateScope = new TransactionScope())
            {
                try
                {
                    foreach (DataRow dr in drSelected)
                    {
                        saveSql = $@"
update PackingList_Detail set PackErrTransferDate = null where ID = '{dr["ID"]}' and CTNStartNo = '{dr["CTNStartNo"]}'

insert into PackErrCFM(CFMDate,MDivisionID,OrderID,PackingListID,CTNStartNo,AddName,AddDate)
                    values(GETDATE(),'{Env.User.Keyword}','{dr["MainSP"]}','{dr["ID"]}','{dr["CTNStartNo"]}','{Env.User.UserID}',GETDATE())
";
                        updateResult = DBProxy.Current.Execute(null, saveSql);
                        if (!updateResult)
                        {
                            this.ShowErr(updateResult);
                            updateScope.Dispose();
                            this.HideWaitMessage();
                            return;
                        }

                        string[] listOrderID = dr["OrderID"].ToString().Split('/');
                        foreach (string orderID in listOrderID)
                        {
                            bool isOkUpdateOrdersCTN = PublicPrg.Prgs.UpdateOrdersCTN(orderID);
                            if (!isOkUpdateOrdersCTN)
                            {
                                MyUtility.Msg.WarningBox("Update OrdersCTN Fail..");
                                updateScope.Dispose();
                                this.HideWaitMessage();
                                return;
                            }
                        }
                    }

                    updateScope.Complete();

                    foreach (DataRow item in drSelected)
                    {
                        this.dtPackErrTransfer.Rows.Remove(item);
                    }

                    MyUtility.Msg.InfoBox("Save successfully");
                    this.HideWaitMessage();
                }
                catch (Exception ex)
                {
                    updateScope.Dispose();
                    this.ShowErr("Commit transaction error.", ex);
                    this.HideWaitMessage();
                    return;
                }
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void QueryData()
        {
            #region select 條件
            List<SqlParameter> listPar = new List<SqlParameter>();
            string sqlWhere = string.Empty;

            if (!MyUtility.Check.Empty(this.txtSP.Text))
            {
                sqlWhere += "and pd.OrderID = @orderID  " + Environment.NewLine;
                listPar.Add(new SqlParameter("@orderID", this.txtSP.Text));
            }

            if (!MyUtility.Check.Empty(this.txtPOID.Text))
            {
                sqlWhere += "and o.CustPONo = @CustPONo  " + Environment.NewLine;
                listPar.Add(new SqlParameter("@CustPONo", this.txtPOID.Text));
            }

            if (!MyUtility.Check.Empty(this.txtPackID.Text))
            {
                sqlWhere += "and pd.ID = @PackID  " + Environment.NewLine;
                listPar.Add(new SqlParameter("@PackID", this.txtPackID.Text));
            }

            if (this.dateRangeTransfer.HasValue)
            {
                sqlWhere += " and pd.PackErrTransferDate between @fromDt and @toDt ";
                listPar.Add(new SqlParameter("@fromDt", this.dateRangeTransfer.DateBox1.Value));
                listPar.Add(new SqlParameter("@toDt", this.dateRangeTransfer.DateBox2.Value));
            }
            #endregion

            string querySQL = $@"
{this.mainPackQuerySql}
where	pd.CTNStartNo <> '' 
		and p.MDivisionID = '{Env.User.Keyword}' 
		and p.Type in ('B','L') 
		and pd.PackErrTransferDate is not null 
		and (pu.Status = 'New' or pu.Status is null) 
        and pd.CTNQty = 1
        {sqlWhere}
order by pd.ID,pd.Seq
";

            DualResult result = DBProxy.Current.Select(null, querySQL, listPar, out this.dtPackErrTransfer);
            this.gridPackErrTransfer.DataSource = this.dtPackErrTransfer;
            if (!result)
            {
                this.ShowErr(result);
                return;
            }
        }

        private CheckPackResult CheckPackID(string packID, string cartonStartNo)
        {
            CheckPackResult checkPackResult = new CheckPackResult() { IsOK = true };
            DataRow drPackResult;
            List<SqlParameter> listPar = new List<SqlParameter>()
            {
                new SqlParameter("@ID", packID),
                new SqlParameter("@CTNStartNo", cartonStartNo)
            };
            string checkPackSql = $@"
{this.mainPackQuerySql}
where	pd.CTNStartNo <> '' 
		and p.MDivisionID = '{Env.User.Keyword}' 
		and p.Type in ('B','L') 
        and pd.CTNQty = 1
        and pd.ID = @ID
        and pd.CTNStartNo = @CTNStartNo
";
            bool result = MyUtility.Check.Seek(checkPackSql, listPar, out drPackResult);

            if (!result)
            {
                MyUtility.Msg.WarningBox($"<CTN#:{packID + cartonStartNo}> does not exist!");
                checkPackResult.IsOK = false;
                return checkPackResult;
            }

            if (drPackResult["PackErrTransferDate"] == DBNull.Value)
            {
                MyUtility.Msg.WarningBox($"<CTN#:{packID + cartonStartNo}> This CTN# Packing Error not yet transferred.");
                checkPackResult.IsOK = false;
                return checkPackResult;
            }

            if (drPackResult["Status"].Equals("Confirmed") || drPackResult["Status"].Equals("Locked"))
            {
                MyUtility.Msg.WarningBox($"<CTN#:{packID + cartonStartNo}> Already pullout!");
                checkPackResult.IsOK = false;
                return checkPackResult;
            }

            drPackResult["selected"] = 1;
            checkPackResult.DrResult = drPackResult;

            return checkPackResult;
        }

        private class CheckPackResult
        {
            private bool isOK;
            private DataRow drResult;

            public bool IsOK
            {
                get
                {
                    return this.isOK;
                }

                set
                {
                    this.isOK = value;
                }
            }

            public DataRow DrResult
            {
                get
                {
                    return this.drResult;
                }

                set
                {
                    this.drResult = value;
                }
            }
        }
    }
}
