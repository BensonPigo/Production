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

namespace Sci.Production.PPIC
{
    public partial class R01 : Sci.Win.Tems.PrintForm
    {
        DataTable printData;
        string mDivision, factory, line1, line2, brand;
        DateTime? inline, offline, buyerDelivery1, buyerDelivery2, sciDelivery1, sciDelivery2;

        public R01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            DataTable mDivision, factory;
            DBProxy.Current.Select(null, "select '' as ID union all select ID from MDivision", out mDivision);
            MyUtility.Tool.SetupCombox(comboBox1, 1, mDivision);
            DBProxy.Current.Select(null, "select '' as ID union all select distinct FTYGroup from Factory", out factory);
            MyUtility.Tool.SetupCombox(comboBox2, 1, factory);
            comboBox1.Text = Sci.Env.User.Keyword;
            comboBox2.SelectedIndex = 0;
        }

        //Sewing Line
        private void textBox1_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            textBox1.Text = SelectSewingLine(textBox1.Text);
        }

        //Sewing Line
        private void textBox2_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            textBox2.Text = SelectSewingLine(textBox2.Text);
        }

        private string SelectSewingLine(string line)
        {
            string sql = string.Format("Select Distinct ID From SewingLine{0}", MyUtility.Check.Empty(comboBox2.Text) ? "" : string.Format(" where FactoryID = '{0}'", comboBox2.Text));
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sql, "3", line, false, ",");
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return "";
            }
            else
            {
                return item.GetSelectedString();
            }
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            mDivision = comboBox1.Text;
            factory = comboBox2.Text;
            line1 = textBox1.Text;
            line2 = textBox2.Text;
            inline = dateBox1.Value;
            offline = dateBox2.Value;
            buyerDelivery1 = dateRange1.Value1;
            buyerDelivery2 = dateRange1.Value2;
            sciDelivery1 = dateRange2.Value1;
            sciDelivery2 = dateRange2.Value2;
            brand = txtbrand1.Text;

            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(@"with tmpAllArtwork
as (
select ot.ID,at.Abbreviation,ot.Qty,ot.TMS,at.Classify
from Order_TmsCost ot, ArtworkType at
where ot.ArtworkTypeID = at.ID
and (ot.Price > 0 or at.Classify = 'O')
and (at.Classify = 'S' or at.IsSubprocess = 1)
and (ot.TMS > 0 or ot.Qty > 0)
),
tmpArtWork
as (
select * from (
select ID,Abbreviation+':'+Convert(varchar,Qty) as Artwork from tmpAllArtwork where Qty > 0
union all
select ID,Abbreviation+':'+Convert(varchar,TMS) as Artwork from tmpAllArtwork where TMS > 0 and Classify = 'O') a
),
tmpOrderArtwork
as (
select ID,(select CONCAT(Artwork,', ') from tmpArtWork where ID = t.ID order by Artwork for xml path('')) as Artwork from tmpArtWork t
)
select SewingLineID,MDivisionID,FactoryID,OrderID,ComboType,IIF(Article = '','',SUBSTRING(Article,1,LEN(Article)-1)) as Article,
CdCodeID,StyleID,Qty,AlloQty,CutQty,SewingQty,ClogQty,InspDate,StandardOutput*WorkHour as TotalStandardOutput,WorkHour,
StandardOutput,MaxEff,KPILETA,PFRemark,MTLETA,MTLExport,Inline,
Offline,SciDelivery,BuyerDelivery,CPU,VasShas,ShipModeList,Alias,ArtWork,IIF(Remark = '','',SUBSTRING(Remark,1,LEN(Remark)-1)) as Remark
from (
select s.SewingLineID,s.MDivisionID,s.FactoryID,s.OrderID,s.ComboType,
(select CONCAT(Article,',') from (select distinct Article from SewingSchedule_Detail sd where sd.ID = s.ID) a for xml path('')) as Article,
o.CdCodeID,o.StyleID,o.Qty,s.AlloQty,
isnull((select sum(Qty) from CuttingOutput_WIP c where c.OrderID = s.OrderID and c.Article in (select Article from SewingSchedule_Detail sd where sd.ID = s.ID)),0) as CutQty,
isnull((select sum(sod.QAQty) from SewingOutput so, SewingOutput_Detail sod where so.ID = sod.ID and so.SewingLineID = s.SewingLineID and sod.OrderId = s.OrderID and sod.ComboType = s.ComboType),0) as SewingQty,
isnull((select sum(pd.ShipQty) from PackingList_Detail pd where pd.OrderID = s.OrderID and pd.ReceiveDate is not null),'') as ClogQty,
o.InspDate,s.StandardOutput,
(select IIF(ctn = 0,0,Hours/ctn) from (Select isnull(sum(w.Hours),0) as Hours, Count(w.Date) as ctn from WorkHour w where FactoryID = s.FactoryID and w.SewingLineID = s.SewingLineID and w.Date between s.Inline and s.Offline and w.Hours > 0) a) as WorkHour,
s.MaxEff,o.KPILETA,
isnull((Select op.Remark from Order_PFHis op where op.ID = s.OrderID and op.AddDate = (Select Max(AddDate) from Order_PFHis where ID = s.OrderID)),'') as PFRemark,
o.MTLETA,o.MTLExport,s.Inline,s.Offline,o.SciDelivery,o.BuyerDelivery,
o.CPU*o.CPUFactor*(isnull(sl.Rate,100)/100) as CPU,IIF(o.VasShas=1,'Y','') as VasShas,o.ShipModeList,isnull(c.Alias,'') as Alias,isnull(SUBSTRING(ta.Artwork,1,LEN(ta.Artwork)-1),'') as ArtWork,
isnull((select CONCAT(Remark,', ') from (select s1.SewingLineID+'('+s1.ComboType+'):'+CONVERT(varchar,s1.AlloQty) as Remark from SewingSchedule s1 where s1.OrderID = s.OrderID and s1.ID != s.ID) a for xml path('')),'') as Remark
from SewingSchedule s
inner join Orders o on o.ID = s.OrderID
left join Style_Location sl on sl.StyleUkey = o.StyleUkey
left join tmpOrderArtwork ta on ta.ID = s.OrderID
left join Country c on o.Dest = c.ID
where 1 = 1
");
            if (!MyUtility.Check.Empty(mDivision))
            {
                sqlCmd.Append(string.Format(" and s.MDivisionID = '{0}'", mDivision));
            }

            if (!MyUtility.Check.Empty(factory))
            {
                sqlCmd.Append(string.Format(" and s.FactoryID = '{0}'", factory));
            }

            if (!MyUtility.Check.Empty(line1))
            {
                sqlCmd.Append(string.Format(" and s.SewingLineID >= '{0}'", line1));
            }

            if (!MyUtility.Check.Empty(line2))
            {
                sqlCmd.Append(string.Format(" and s.SewingLineID <= '{0}'", line2));
            }

            if (!MyUtility.Check.Empty(inline))
            {
                sqlCmd.Append(string.Format(" and s.Inline >= '{0}'", Convert.ToDateTime(inline).ToString("d")));
            }

            if (!MyUtility.Check.Empty(offline))
            {
                sqlCmd.Append(string.Format(" and s.Offline < '{0}'", Convert.ToDateTime(offline).AddDays(1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(buyerDelivery1))
            {
                sqlCmd.Append(string.Format(" and o.BuyerDelivery >= '{0}'", Convert.ToDateTime(buyerDelivery1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(buyerDelivery2))
            {
                sqlCmd.Append(string.Format(" and o.BuyerDelivery <= '{0}'", Convert.ToDateTime(buyerDelivery2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(sciDelivery1))
            {
                sqlCmd.Append(string.Format(" and o.SciDelivery >= '{0}'", Convert.ToDateTime(sciDelivery1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(sciDelivery2))
            {
                sqlCmd.Append(string.Format(" and o.SciDelivery <= '{0}'", Convert.ToDateTime(sciDelivery2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(brand))
            {
                sqlCmd.Append(string.Format(" and o.BrandID = '{0}'", brand));
            }

            sqlCmd.Append(@" ) a
order by SewingLineID,MDivisionID,FactoryID,Inline,StyleID");

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out printData);
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

            bool result = MyUtility.Excel.CopyToXls(printData, "", xltfile: "PPIC_R01_SewingLineScheduleReport.xltx", headerRow: 1);
            if (!result) { MyUtility.Msg.WarningBox(result.ToString(), "Warning"); return false; }
            return true;

        }
    }
}
