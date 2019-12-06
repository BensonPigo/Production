using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;
using Sci.Win.Tems;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// P11
    /// </summary>
    public partial class P11 : Sci.Win.Tems.Input6
    {
        /// <summary>
        /// P11
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P11(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();

            this.Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("ID", header: "Garment Booking", width: Widths.AnsiChars(30))
            ;
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();

            this.disExVoucherID.Text = this.CurrentMaintain["ExVoucherID"].ToString();
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (this.DetailDatas.Count == 0)
            {
                MyUtility.Msg.WarningBox("Garment Booking cannot be empty !");
                return false;
            }

            List<string> ls = new List<string>();
            foreach (DataRow dr in this.DetailDatas)
            {
                if (!MyUtility.Check.Seek($@"select 1 from GMTBooking where id = '{dr["id"]}'"))
                {
                    ls.Add(MyUtility.Convert.GetString(dr["id"]));
                }
            }

            if (ls.Count > 0)
            {
                MyUtility.Msg.WarningBox($@"Garment Booking does not exist, please check again!
{string.Join(", ", ls)}");
                return false;
            }

            return base.ClickSaveBefore();
        }

        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["Status"] = "New";
        }

        /// <inheritdoc/>
        protected override DualResult OnSaveDetail(IList<DataRow> details, ITableSchema detailtableschema)
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["ID"]))
            {
                return new DualResult(false, $"ID({this.CurrentMaintain["ID"].ToString()}) is empty,Please inform MIS to handle this issue");
            }

            string updatesql = $@"update GMTBooking set BIRID = null  where BIRID = '{this.CurrentMaintain["ID"]}'  ";
            DualResult result = DBProxy.Current.Execute(null, updatesql);
            if (!result)
            {
                return result;
            }

            IList<string> updateCmds = new List<string>();

            foreach (DataRow dr in details)
            {
                if (dr.RowState == DataRowState.Added || dr.RowState == DataRowState.Modified)
                {
                    updateCmds.Add($@"update GMTBooking set BIRID = '{this.CurrentMaintain["ID"]}' where ID = '{dr["id"]}';");
                }

                if (dr.RowState == DataRowState.Deleted)
                {
                    updateCmds.Add($@"update GMTBooking set BIRID = null where ID = '{dr["id",DataRowVersion.Original]}';");
                }
            }

            if (updateCmds.Count != 0)
            {
                result = DBProxy.Current.Executes(null, updateCmds);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, result.ToString());
                    return failResult;
                }
            }

            return Result.True;
        }

        /// <inheritdoc/>
        protected override DualResult ClickDelete()
        {
            string updatesql = $@"update GMTBooking set BIRID = null  where BIRID = '{this.CurrentMaintain["ID"]}'";
            DualResult result = DBProxy.Current.Execute(null, updatesql);
            if (!result)
            {
                return result;
            }

            return base.ClickDelete();
        }

        protected override void ClickConfirm()
        {
            string sqlupdate = $@"
update BIRInvoice set Status='Approved', Approve='{Sci.Env.User.UserID}', ApproveDate=getdate(), EditName='{Sci.Env.User.UserID}', EditDate=getdate()
where id = '{this.CurrentMaintain["ID"]}'
";
            DualResult result = DBProxy.Current.Execute(null, sqlupdate);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            base.ClickConfirm();
        }

        protected override void ClickUnconfirm()
        {
            string sqlchk = $@"select 1 from BIRInvoice  where ExVoucherID !='' and id = '{this.CurrentMaintain["ID"]}'";
            if (MyUtility.Check.Seek(sqlchk))
            {
                MyUtility.Msg.WarningBox("Cannot unconfirm because already created voucher no");
                return;
            }

            string sqlupdate = $@"
update BIRInvoice set Status='New', Approve='{Sci.Env.User.UserID}', ApproveDate=getdate(), EditName='{Sci.Env.User.UserID}', EditDate=getdate()
where id = '{this.CurrentMaintain["ID"]}'
";
            DualResult result = DBProxy.Current.Execute(null, sqlupdate);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }
            base.ClickUnconfirm();
        }

        protected override bool ClickDeleteBefore()
        {
            string sqlchk = $@"select 1 from BIRInvoice  where ExVoucherID is not null and id = '{this.CurrentMaintain["ID"]}' and status = 'Approved' ";
            if (MyUtility.Check.Seek(sqlchk))
            {
                MyUtility.Msg.WarningBox("Already approved, cannot delete!");
                return false;
            }

            return base.ClickDeleteBefore();
        }

        /// <inheritdoc/>
        private void Btnimport_Click(object sender, EventArgs e)
        {
            if (!this.EditMode)
            {
                return;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["InvSerial"]))
            {
                MyUtility.Msg.WarningBox("Invoice Serial cannot be empty!");
                return;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["BrandID"]))
            {
                MyUtility.Msg.WarningBox("Brand cannot be empty!");
                return;
            }

            string sqlchk = $@"select 1 from BIRInvoice b where b.InvSerial = '{this.CurrentMaintain["InvSerial"]}' and b.BrandID = '{this.CurrentMaintain["BrandID"]}'";
            if (MyUtility.Check.Seek(sqlchk))
            {
                MyUtility.Msg.WarningBox("Already has this reocrd!");
                return;
            }

            string sqlcmd = $@"
select *
from GMTBooking with(nolock)
where isnull(BIRID,0) = 0
and BrandID = '{this.CurrentMaintain["BrandID"]}'
and InvSerial like '{this.CurrentMaintain["InvSerial"]}%'
        ";
            DataTable dt;
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            if (dt.Rows.Count == 0)
            {
                this.ShowErr("Import error!");
                return;
            }

            foreach (DataRow dr in dt.Rows)
            {
                dr.AcceptChanges();
                dr.SetAdded();
                ((DataTable)this.detailgridbs.DataSource).ImportRow(dr);
            }
        }

        /// <inheritdoc/>
        protected override bool ClickPrint()
        {
            #region 
            List<string> ids = new List<string>();
            foreach (DataRow dr in this.DetailDatas)
            {
                ids.Add("'" + dr["id"] + "'");
            }

            DataTable dt;
            string sqlcmd = $@"
select 
	A=o.CustPONo,
	B=o.StyleID,
	C=s.Description,
	E=sum(pd.ShipQty),
	F='PCS',
	G=o.CurrencyID,
	H=ROUND(std.FtyCMP,2),
	J=sum(pd.ShipQty)*ROUND(isnull(std.FtyCMP,0),2)
from orders o with(nolock)
inner join PackingList_Detail pd with(nolock) on pd.OrderID = o.id
inner join PackingList p with(nolock) on p.id = pd.id
left join Style s with(nolock) on s.Ukey = o.StyleUkey
outer apply(select SubProcessCPU= sum(Isnull(Price,0)) from GetSubProcessDetailByOrderID(o.id,'CPU'))a
outer apply(select subProcessAMT= sum(Isnull(Price,0)) from GetSubProcessDetailByOrderID(o.id,'AMT'))b
outer apply(
	select fd.CpuCost
	from FtyShipper_Detail fsd WITH (NOLOCK) , FSRCpuCost_Detail fd WITH (NOLOCK) 
	where fsd.BrandID = o.BrandID
	and fsd.FactoryID = o.FactoryID
	and o.OrigBuyerDelivery between fsd.BeginDate and fsd.EndDate
	and fsd.ShipperID = fd.ShipperID
	and o.OrigBuyerDelivery between fd.BeginDate and fd.EndDate
	and o.OrigBuyerDelivery is not null
    and fsd.seasonID = o.seasonID
)f1
outer apply(
	select fd.CpuCost
	from FtyShipper_Detail fsd WITH (NOLOCK) , FSRCpuCost_Detail fd WITH (NOLOCK) 
	where fsd.BrandID = o.BrandID
	and fsd.FactoryID = o.FactoryID
	and o.OrigBuyerDelivery between fsd.BeginDate and fsd.EndDate
	and fsd.ShipperID = fd.ShipperID
	and o.OrigBuyerDelivery between fd.BeginDate and fd.EndDate
	and o.OrigBuyerDelivery is not null
    and fsd.seasonID = ''
)f
outer apply(
	select dbo.GetLocalPurchaseStdCost(o.id) price
)s3
outer apply(
	select FtyCMP = Round((isnull(round(o.CPU,3,1),0) + isnull(round(a.SubProcessCPU,3,1),0)) * 
	isnull(round(isnull(f1.CpuCost,f.CpuCost),3,1),0) + isnull(round(b.subProcessAMT,3,1),0) + isnull(round(s3.price,3,1),0), 3)
)std
where p.INVNo in({string.Join(",", ids)})
group by o.CustPONo,o.StyleID,s.Description,o.PoPrice,o.id,o.CPU,o.CurrencyID,std.FtyCMP
";
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out dt);
            if (!result)
            {
                this.ShowErr(result);
                return false;
            }
            #endregion

            string strXltName = Sci.Env.Cfg.XltPathDir + "\\Shipping_P11.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return false;
            }

            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            #region 產生頁首頁尾資料

            // 頁首
            string top1idsql = $@"
select top 1 id
from GMTBooking
where id in({string.Join(",", ids)})
order by FCRDate desc
";
            string top1id = MyUtility.GetValue.Lookup(top1idsql);

            string bIRShipToSql = $@"
select top 1 b.BIRShipTo
from GMTBooking a
inner join CustCD b on b.id = a.CustCDID and b.BrandID = a.BrandID
where a.id = '{top1id}' 
";
            string top1BIRShipTo = MyUtility.GetValue.Lookup(bIRShipToSql);
            if (!MyUtility.Check.Empty(top1BIRShipTo))
            {
                string[] BIRShipToarry = top1BIRShipTo.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0, j = 0; i < BIRShipToarry.Length; i++)
                {
                    if (!MyUtility.Check.Empty(BIRShipToarry[i]))
                    {
                        worksheet.Cells[12 + j, 2] = BIRShipToarry[i];
                        j++;
                    }
                }
            }

            DataRow drGMT;
            string top1GMT = $@"select * from GMTBooking where id = '{top1id}'";
            if (MyUtility.Check.Seek(top1GMT, out drGMT))
            {
                worksheet.Cells[9, 10] = drGMT["FCRDate"];
                worksheet.Cells[11, 10] = drGMT["Vessel"];
                worksheet.Cells[16, 10] = MyUtility.GetValue.Lookup($@"select NameEN from Country where id = '{drGMT["Dest"]}'");
            }

            // 頁尾
            decimal sumJ = MyUtility.Convert.GetDecimal(dt.Compute("sum(J)", null));
            decimal sumM = sumJ;
            decimal sumE = MyUtility.Convert.GetDecimal(dt.Compute("sum(E)", null));

            //worksheet.Cells[48, 3] = MyUtility.Convert.USDMoney(sumI).Replace("AND CENTS", Environment.NewLine + "AND CENTS");
            worksheet.Cells[57, 3] = MyUtility.Convert.USDMoney(sumJ);

            string sumGW = $@"
select sumGW = Sum (p.GW),sumNW=sum(p.NW),sumCBM=sum(p.CBM)
from PackingList p with(nolock)
where p.INVNo in ({string.Join(",", ids)})
";
            DataRow drsum;
            if (MyUtility.Check.Seek(sumGW, out drsum))
            {
                worksheet.Cells[66, 3] = drsum["sumGW"];
                worksheet.Cells[67, 3] = drsum["sumNW"];
                worksheet.Cells[68, 3] = drsum["sumCBM"];
            }

            worksheet.Cells[63, 10] = sumJ;
            worksheet.Cells[65, 10] = 0;
            worksheet.Cells[66, 10] = sumM;
            worksheet.Cells[60, 2] = sumE;
            worksheet.Cells[1, 10] = $@"InvSerial: {this.CurrentMaintain["InvSerial"]}";
            #endregion

            #region 內容

            // 如果內容超過41筆插入新的頁面
            int insertSheetCount = dt.Rows.Count / 41;
            for (int i = 1; i <= insertSheetCount; i++)
            {
                worksheet.Copy(Type.Missing, worksheet);
            }

            int contentCount = 41;
            int ttlSheetCount = excel.ActiveWorkbook.Worksheets.Count;
            for (int i = 1; i <= ttlSheetCount; i++)
            {
                var xlNewSheet = (Excel.Worksheet)excel.ActiveWorkbook.Worksheets[i];
                xlNewSheet.Name = "BIR Invoice-" + i.ToString();
                int intRowsStart = 25;
                int dataEnd = i * contentCount;
                int dataStart = dataEnd - contentCount;
                object[,] objArray = new object[1, 10];

                for (int j = dataStart; j < dataEnd; j++)
                {
                    if (j >= dt.Rows.Count)
                    {
                        break;
                    }

                    DataRow dr = dt.Rows[j];
                    int rownum = intRowsStart++;
                    objArray[0, 0] = dr["A"];
                    objArray[0, 1] = dr["B"];
                    objArray[0, 2] = dr["C"];
                    objArray[0, 3] = string.Empty;
                    objArray[0, 4] = dr["E"];
                    objArray[0, 5] = dr["F"];
                    objArray[0, 6] = dr["G"];
                    objArray[0, 7] = dr["H"];
                    objArray[0, 8] = string.Empty;
                    objArray[0, 9] = dr["J"];
                    xlNewSheet.Range[string.Format("A{0}:J{0}", rownum)].Value2 = objArray;
                }

                Marshal.ReleaseComObject(xlNewSheet);
            }
            #endregion

            #region Save & Show Excel
            worksheet = excel.ActiveWorkbook.Worksheets[1];
            worksheet.Activate();
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Shipping_P11");
            excel.ActiveWorkbook.SaveAs(strExcelName);
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(worksheet);

            strExcelName.OpenFile();
            #endregion
            return base.ClickPrint();
        }

        private void BtnBatchApprove(object sender, EventArgs e)
        {
            Sci.Production.Shipping.P11_BatchApprove callNextForm = new P11_BatchApprove(this.Reload);
            callNextForm.ShowDialog(this);
        }

        public void Reload()
        {
            this.ReloadDatas();
            this.RenewData();
        }
    }
}
