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
{string.Join(", ",ls)}");
                return false;
            }

            return base.ClickSaveBefore(); 
        }

        /// <inheritdoc/>
        protected override DualResult OnSaveDetail(IList<DataRow> details, ITableSchema detailtableschema)
        {
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
	C=s.Description,
	D=null,
	E=sum(pd.ShipQty),
	F='PCS',
	G='US',
	H=round(o.PoPrice,2),
	I=null,
	J=sum(pd.ShipQty)*round(o.PoPrice,2),
	K=null,
	L=null,
	M=round((o.CPU*f.CpuCost)+(s1.Price+s2.Price)+s3.Price,2),
	N = sum(pd.ShipQty)*round((o.CPU*f.CpuCost)+(s1.Price+s2.Price)+s3.Price,2)
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
o.id,o.CPU,f.CpuCost,s1.Price+s2.Price,s3.Price
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
                        worksheet.Cells[11 + j, 2] = BIRShipToarry[i];
                        j++;
                    }
                }
            }

            DataRow drGMT;
            string top1GMT = $@"select * from GMTBooking where id = '{top1id}'";
            if (MyUtility.Check.Seek(top1GMT, out drGMT))
            {
                worksheet.Cells[8, 10] = drGMT["FCRDate"];
                worksheet.Cells[10, 10] = drGMT["Vessel"];
                worksheet.Cells[14, 10] = MyUtility.GetValue.Lookup($@"select NameEN from Country where id = '{drGMT["Dest"]}'");
            }

            int intRowsStart = 20;
            int dataRowCount = dt.Rows.Count;
            object[,] objArray = new object[1, 14];
            for (int i = 0; i < dataRowCount; i++)
            {
                DataRow dr = dt.Rows[i];
                int rownum = intRowsStart + i;
                objArray[0, 0] = dr["A"];
                objArray[0, 1] = dr["B"];
                objArray[0, 2] = dr["C"];
                objArray[0, 3] = dr["D"];
                objArray[0, 4] = dr["E"];
                objArray[0, 5] = dr["F"];
                objArray[0, 6] = dr["G"];
                objArray[0, 7] = dr["H"];
                objArray[0, 8] = dr["I"];
                objArray[0, 9] = dr["J"];
                objArray[0, 10] = dr["K"];
                objArray[0, 11] = dr["L"];
                objArray[0, 12] = dr["M"];
                objArray[0, 13] = dr["N"];
                worksheet.Range[string.Format("A{0}:N{0}", rownum)].Value2 = objArray;
            }

            decimal sumJ = MyUtility.Convert.GetDecimal(dt.Compute("sum(J)", null));
            worksheet.Cells[44, 3] = MyUtility.Convert.USDMoney(sumJ);

            string sumGW = $@"
select sumGW = Sum (pd.GW),sumNW=sum(pd.NW)
from PackingList_Detail pd with(nolock)
inner join PackingList p with(nolock) on p.ID = pd.ID
where p.INVNo in ({string.Join(",", ids)})
";
            DataRow drsum;
            if (MyUtility.Check.Seek(sumGW,out drsum))
            {
                worksheet.Cells[53, 3] = drsum["sumGW"];
                worksheet.Cells[54, 3] = drsum["sumNW"];
            }

            string sumcbm = $@"
select sumCBM=sum(p.CBM)
from PackingList p with(nolock) 
where p.INVNo in ({string.Join(",", ids)})
";
            worksheet.Cells[55, 3] = MyUtility.GetValue.Lookup(sumcbm);

            #region Save & Show Excel
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
