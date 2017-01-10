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

namespace Sci.Production.Shipping
{
    public partial class R11 : Sci.Win.Tems.PrintForm
    {
        DataTable printData;
        int reportType;
        DateTime? date1, date2;
        string brand, custCD, dest, shipMode, forwarder;
        public R11(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            radioButton1.Checked = true;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            txtshipmode1.SelectedIndex = -1;
        }

        //Forwarder
        private void textBox1_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string selectCommand;
            selectCommand = @"select ID,Abb from LocalSupp
union all
select ID,AbbEN from Supp
order by ID";

            DataTable tbSelect;
            DBProxy.Current.Select(null, selectCommand, out tbSelect);
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(tbSelect, "ID,Abb", "9,13", this.Text, false, ",", "ID,Abb");
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel) { return; }
            IList<DataRow> selected = item.GetSelecteds();
            this.textBox1.Text = item.GetSelectedString();
            displayBox1.Value = MyUtility.Convert.GetString(selected[0]["Abb"]);
        }

        //Forwarder
        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            if (textBox1.OldValue != textBox1.Text)
            {
                if (!MyUtility.Check.Empty(textBox1.Text))
                {
                    DataRow inputData;
                    string Sql = string.Format(@"select * from (
select ID,Abb from LocalSupp
union all
select ID,AbbEN from Supp) a
where a.ID = '{0}'", textBox1.Text);
                    if (!MyUtility.Check.Seek(Sql, out inputData))
                    {
                        MyUtility.Msg.WarningBox(string.Format("< Forwarder: {0} > not found!!!", textBox1.Text));
                        textBox1.Text = "";
                        displayBox1.Value = "";
                        e.Cancel = true;
                        return;
                    }
                    else
                    {
                        textBox1.Text = textBox1.Text;
                        displayBox1.Value = MyUtility.Convert.GetString(inputData["Abb"]);
                    }
                }
                else
                {
                    textBox1.Text = "";
                    displayBox1.Value = "";
                }
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                label2.Text = "Pullout Date";
                label2.Size = new System.Drawing.Size(101, 23);
                label2.Location = new System.Drawing.Point(13, 71);
                txtbrand1.Enabled = true;
                txtcustcd1.Enabled = true;
            }
            else
            {
                label2.Text = "Arrive Port Date \r\n (Ship Date)";
                label2.Size = new System.Drawing.Size(101, 36);
                label2.Location = new System.Drawing.Point(13, 64);
                txtbrand1.Enabled = false;
                txtcustcd1.Enabled = false;
            }
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            date1 = dateRange1.Value1;
            date2 = dateRange1.Value2;
            brand = txtbrand1.Text;
            custCD = txtcustcd1.Text;
            dest = txtcountry1.TextBox1.Text;
            shipMode = txtshipmode1.Text;
            forwarder = textBox1.Text;
            reportType = radioButton1.Checked ? 1 : 2;
            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            DualResult result;
            if (reportType == 1)
            {
                #region 組SQL Command
                sqlCmd.Append(@"with GBData
as (
select distinct 'Export' as IE, 'GARMENT' as Type,g.ID,g.Shipper,g.BrandID,
IIF(p.Type = 'B','Bulk',IIF(p.Type = 'S','Sample','')) as Category,
isnull((select sum(a.Qty) from (
select distinct oq.Id,oq.Seq,oq.Qty from PackingList_Detail pd, Order_QtyShip oq
where pd.ID = p.ID and pd.OrderID = oq.Id and pd.OrderShipmodeSeq = oq.Seq) a),0) as OrderQty,
g.CustCDID,g.Dest,g.ShipModeID,
(select MAX(PulloutDate) from PackingList where INVNo = g.ID) as PulloutDate,g.TotalShipQty,
g.TotalCTNQty,g.TotalGW,g.TotalCBM,g.Forwarder+'-'+isnull(l.Abb,'') as Forwarder,'' as BLNo
from GMTBooking g
inner join PackingList p on p.INVNo = g.ID
left join LocalSupp l on l.ID = g.Forwarder
where not exists (select 1 from ShareExpense where InvNo = g.ID)");
                if (!MyUtility.Check.Empty(date1))
                {
                    sqlCmd.Append(string.Format(" and p.PulloutDate >= '{0}'", Convert.ToDateTime(date1).ToString("d")));
                }
                if (!MyUtility.Check.Empty(date2))
                {
                    sqlCmd.Append(string.Format(" and p.PulloutDate <= '{0}'", Convert.ToDateTime(date2).ToString("d")));
                }
                if (!MyUtility.Check.Empty(brand))
                {
                    sqlCmd.Append(string.Format(" and g.BrandID = '{0}'", brand));
                }
                if (!MyUtility.Check.Empty(custCD))
                {
                    sqlCmd.Append(string.Format(" and g.CustCDID = '{0}'", custCD));
                }
                if (!MyUtility.Check.Empty(dest))
                {
                    sqlCmd.Append(string.Format(" and g.Dest = '{0}'", dest));
                }
                if (!MyUtility.Check.Empty(shipMode))
                {
                    sqlCmd.Append(string.Format(" and g.ShipModeID = '{0}'", shipMode));
                }
                if (!MyUtility.Check.Empty(forwarder))
                {
                    sqlCmd.Append(string.Format(" and g.Forwarder = '{0}'", forwarder));
                }
                sqlCmd.Append(@"),
PLData
as (
select  'Export' as IE, 'GARMENT' as Type,p.ID,p.MDivisionID,p.BrandID,
IIF((select top 1 o.Category from Orders o, PackingList_Detail pd 
where pd.ID = p.ID and o.ID = pd.OrderID)='B','Bulk','Sample') as Category,
isnull((select sum(a.Qty) from (
select distinct oq.Id,oq.Seq,oq.Qty from PackingList_Detail pd, Order_QtyShip oq
where pd.ID = p.ID and pd.OrderID = oq.Id and pd.OrderShipmodeSeq = oq.Seq) a),0) as OrderQty,
p.CustCDID,'' as Dest,p.ShipModeID,p.PulloutDate,p.ShipQty,p.CTNQty,p.GW,p.CBM,'' as Forwarder,
'' as BLNo
from PackingList p
where (p.Type = 'F' or p.Type = 'L')
and not exists (select 1 from ShareExpense where InvNo = p.ID)");
                if (!MyUtility.Check.Empty(date1))
                {
                    sqlCmd.Append(string.Format(" and p.PulloutDate >= '{0}'", Convert.ToDateTime(date1).ToString("d")));
                }
                if (!MyUtility.Check.Empty(date2))
                {
                    sqlCmd.Append(string.Format(" and p.PulloutDate <= '{0}'", Convert.ToDateTime(date2).ToString("d")));
                }
                if (!MyUtility.Check.Empty(brand))
                {
                    sqlCmd.Append(string.Format(" and p.BrandID = '{0}'", brand));
                }
                if (!MyUtility.Check.Empty(custCD))
                {
                    sqlCmd.Append(string.Format(" and p.CustCDID = '{0}'", custCD));
                }
                if (!MyUtility.Check.Empty(shipMode))
                {
                    sqlCmd.Append(string.Format(" and p.ShipModeID = '{0}'", shipMode));
                }
                sqlCmd.Append(@")

select * from GBData
union all
select * from PLData");
                #endregion
            }
            else
            {
                #region 組SQL Command
                sqlCmd.Append(@"with ExportData
as (
select 'Import' as IE,'MATERIAL' as Type,e.ID,e.ImportCountry,e.ShipModeID,e.PortArrival,e.WeightKg,
e.Cbm,e.Forwarder+'-'+isnull(s.AbbEN,'') as Forwarder,e.Blno,
(select top 1 ShippingAPID from ShareExpense where Blno = e.Blno) as APId1,
(select top 1 ShippingAPID from ShareExpense where WKNo = e.ID) as APId2
from Export e
left join Supp s on s.ID = e.Forwarder
where e.Junk = 0");
                if (!MyUtility.Check.Empty(date1))
                {
                    sqlCmd.Append(string.Format(" and e.PortArrival >= '{0}'", Convert.ToDateTime(date1).ToString("d")));
                }
                if (!MyUtility.Check.Empty(date2))
                {
                    sqlCmd.Append(string.Format(" and e.PortArrival <= '{0}'", Convert.ToDateTime(date2).ToString("d")));
                }
                if (!MyUtility.Check.Empty(dest))
                {
                    sqlCmd.Append(string.Format(" and e.ImportCountry = '{0}'", dest));
                }
                if (!MyUtility.Check.Empty(shipMode))
                {
                    sqlCmd.Append(string.Format(" and e.ShipModeID = '{0}'", shipMode));
                }
                if (!MyUtility.Check.Empty(forwarder))
                {
                    sqlCmd.Append(string.Format(" and e.Forwarder = '{0}'", forwarder));
                }
                sqlCmd.Append(@"),
FtyExportData
as (
select IIF(f.Type = 3,'Export','Import') as IE,
IIF(f.Type = 1,'3rd Country',IIF(f.Type = 2,'Transfer In',IIF(f.Type = 3,'Transfer Out','Local Purchase'))) as Type,
f.ID,f.ImportCountry,f.ShipModeID,f.PortArrival,f.WeightKg,f.Cbm,f.Forwarder+'-'+isnull(l.Abb,'') as Forwarder,
f.Blno
from FtyExport f
left join LocalSupp l on l.ID = f.Forwarder
where not exists (select 1 from ShareExpense where WKNo = f.ID)");
                if (!MyUtility.Check.Empty(date1))
                {
                    sqlCmd.Append(string.Format(" and f.PortArrival >= '{0}'", Convert.ToDateTime(date1).ToString("d")));
                }
                if (!MyUtility.Check.Empty(date2))
                {
                    sqlCmd.Append(string.Format(" and f.PortArrival <= '{0}'", Convert.ToDateTime(date2).ToString("d")));
                }
                if (!MyUtility.Check.Empty(dest))
                {
                    sqlCmd.Append(string.Format(" and f.ImportCountry = '{0}'", dest));
                }
                if (!MyUtility.Check.Empty(shipMode))
                {
                    sqlCmd.Append(string.Format(" and f.ShipModeID = '{0}'", shipMode));
                }
                if (!MyUtility.Check.Empty(forwarder))
                {
                    sqlCmd.Append(string.Format(" and f.Forwarder = '{0}'", forwarder));
                }
                sqlCmd.Append(@")

select IE,Type,ID,'' as Shipper,'' as BrandID,'' as Category,0 as OrderQty,'' as CustCDID,
ImportCountry,ShipModeID,PortArrival,0 as ShipQty,0 as CTNQty,WeightKg,Cbm,Forwarder,Blno
from ExportData
where (Blno <> '' and APId1 is null) or (Blno = '' and APId2 is null)
union all
select IE,Type,ID,'' as Shipper,'' as BrandID,'' as Category,0 as OrderQty,'' as CustCDID,
ImportCountry,ShipModeID,PortArrival,0 as ShipQty,0 as CTNQty,WeightKg,Cbm,Forwarder,Blno
from FtyExportData");
                #endregion
            }

            result = DBProxy.Current.Select(null, sqlCmd.ToString(), out printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Result.True;
        }

        // 產生Excel
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            SetCount(printData.Rows.Count);

            if (printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            this.ShowWaitMessage("Starting EXCEL...");
            string excelFile = reportType == 1 ? "Shipping_R11_NonSharedListGarment.xltx" : "Shipping_R11_NonSharedListMaterial.xltx";
            bool result = MyUtility.Excel.CopyToXls(printData, "", xltfile: excelFile, headerRow: 1);
            if (!result) { MyUtility.Msg.WarningBox(result.ToString(), "Warning"); }
            this.HideWaitMessage();
            return true;
        }
    }
}
