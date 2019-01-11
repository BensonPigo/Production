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

            string sqlcmd = $@"
select *
from GMTBooking with(nolock)
where BIRID is null
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
	C=null,
	D=s.Description,
	E=null,
	F=sum(pd.ShipQty),
	G='PCS',
	H=round(o.PoPrice,2),
	I=sum(pd.ShipQty)*round(o.PoPrice,2),
	L=round((o.CPU*isnull(f.CpuCost,0))+(isnull(s1.Price,0)+isnull(s2.Price,0))+isnull(s3.Price,0),2),
	M=sum(pd.ShipQty)*round((o.CPU*isnull(f.CpuCost,0))+(isnull(s1.Price,0)+isnull(s2.Price,0))+isnull(s3.Price,0),2)
from orders o with(nolock)
inner join PackingList_Detail pd with(nolock) on pd.OrderID = o.id
inner join PackingList p with(nolock) on p.id = pd.id
left join Style s with(nolock) on s.Ukey = o.StyleUkey
outer apply(
	select fd.CpuCost
	from FtyShipper_Detail fsd WITH (NOLOCK) , FSRCpuCost_Detail fd WITH (NOLOCK) 
	where fsd.BrandID = o.BrandID
	and fsd.FactoryID = o.FactoryID
	and o.OrigBuyerDelivery between fsd.BeginDate and fsd.EndDate
	and fsd.ShipperID = fd.ShipperID
	and o.OrigBuyerDelivery between fd.BeginDate and fd.EndDate
	and o.OrigBuyerDelivery is not null
)f
outer apply(
	select Price = sum(ot.Price)
	from Order_TmsCost ot WITH (NOLOCK) 
	left join ArtworkType a WITH (NOLOCK) on ot.ArtworkTypeID = a.ID
	where ot.ID = o.id and a.Classify = 'I'and a.IsTtlTMS <> 1
)s1
outer apply(
	select Price = sum(ot.Price)
	from Order_TmsCost ot WITH (NOLOCK) 
	left join ArtworkType a WITH (NOLOCK) on ot.ArtworkTypeID = a.ID
	where ot.ID = o.id and a.Classify = 'A'
)s2
outer apply(
	select Price = Round(sum(ot.Price),3,1)
	from Order_TmsCost ot WITH (NOLOCK) 
	left join ArtworkType a WITH (NOLOCK) on ot.ArtworkTypeID = a.ID
	where ot.ID = o.id and a.Classify = 'P'
)s3
where p.INVNo in({string.Join(",", ids)})
group by o.CustPONo,o.StyleID,s.Description,o.PoPrice,
o.id,o.CPU,isnull(f.CpuCost,0),isnull(s1.Price,0)+isnull(s2.Price,0),isnull(s3.Price,0)
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
                        worksheet.Cells[4 + j, 2] = BIRShipToarry[i];
                        j++;
                    }
                }
            }

            DataRow drGMT;
            string top1GMT = $@"select * from GMTBooking where id = '{top1id}'";
            if (MyUtility.Check.Seek(top1GMT, out drGMT))
            {
                worksheet.Cells[1, 9] = drGMT["FCRDate"];
                worksheet.Cells[3, 9] = drGMT["Vessel"];
                worksheet.Cells[8, 9] = MyUtility.GetValue.Lookup($@"select NameEN from Country where id = '{drGMT["Dest"]}'");
            }

            // 頁尾
            decimal sumI = MyUtility.Convert.GetDecimal(dt.Compute("sum(I)", null));
            decimal sumM = MyUtility.Convert.GetDecimal(dt.Compute("sum(M)", null));
            decimal sumF = MyUtility.Convert.GetDecimal(dt.Compute("sum(F)", null));

            worksheet.Cells[59, 3] = MyUtility.Convert.USDMoney(sumI).Replace("AND CENTS", Environment.NewLine + "AND CENTS");

            string sumGW = $@"
select sumGW = Sum (p.GW),sumNW=sum(p.NW),sumCBM=sum(p.CBM)
from PackingList p with(nolock)
where p.INVNo in ({string.Join(",", ids)})
";
            DataRow drsum;
            if (MyUtility.Check.Seek(sumGW, out drsum))
            {
                worksheet.Cells[69, 4] = drsum["sumGW"];
                worksheet.Cells[70, 4] = drsum["sumNW"];
                worksheet.Cells[71, 4] = drsum["sumCBM"];
            }

            worksheet.Cells[65, 9] = sumI;
            worksheet.Cells[67, 9] = sumM + sumI;
            worksheet.Cells[69, 9] = sumM;
            worksheet.Cells[62, 2] = sumF;
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
                int intRowsStart = 13;
                int dataEnd = i * contentCount;
                int dataStart = dataEnd - contentCount;
                object[,] objArray = new object[1, 9];

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
                    objArray[0, 3] = dr["D"];
                    objArray[0, 4] = dr["E"];
                    objArray[0, 5] = dr["F"];
                    objArray[0, 6] = dr["G"];
                    objArray[0, 7] = dr["H"];
                    objArray[0, 8] = dr["I"];
                    xlNewSheet.Range[string.Format("A{0}:I{0}", rownum)].Value2 = objArray;
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
    }
}
