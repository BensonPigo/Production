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
    public partial class R42 : Sci.Win.Tems.PrintForm
    {
        DataTable printData;
        DateTime? date1, date2;
        string mDivision, factory;
        int category;
        public R42(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            MyUtility.Tool.SetupCombox(comboBox1, 1, 1, "Bulk,Sample,Bulk+Sample");
            DataTable mDivision, factory;
            DBProxy.Current.Select(null, "select '' as ID union all select ID from MDivision", out mDivision);
            MyUtility.Tool.SetupCombox(comboBox2, 1, mDivision);
            DBProxy.Current.Select(null, "select '' as ID union all select distinct FtyGroup from Factory", out factory);
            MyUtility.Tool.SetupCombox(comboBox3, 1, factory);
            comboBox1.SelectedIndex = 2;
            comboBox2.Text = Sci.Env.User.Keyword;
            comboBox3.SelectedIndex = -1;
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(dateRange1.Value1))
            {
                MyUtility.Msg.WarningBox("Date can't empty!!");
                dateRange1.TextBox1.Focus();
                return false;
            }

            if (comboBox1.SelectedIndex == -1)
            {
                MyUtility.Msg.WarningBox("Category can't empty!!");
                comboBox1.Focus();
                return false;
            }

            date1 = dateRange1.Value1;
            date2 = dateRange1.Value2;
            category = comboBox1.SelectedIndex;
            mDivision = comboBox2.Text;
            factory = comboBox3.Text;
            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(string.Format(@"select MDivisionID,FactoryID,ID,StyleID,SeasonID,BrandID,Category,SciDelivery,BuyerDelivery,OrderTypeID,Article,SizeCode,Qty,
IIF(CustomSP = '','',SUBSTRING(CustomSP,0,len(CustomSP)-1)) as CustomSP
from (
select o.MDivisionID,o.FactoryID,o.ID,o.StyleID,o.SeasonID,o.BrandID,IIF(o.Category  = 'B','Bulk','Sample') as Category,
o.SciDelivery,oq.BuyerDelivery,o.OrderTypeID,oqd.Article,oqd.SizeCode,oqd.Qty,oa.Seq as ASeq,os.Seq as SSeq,
isnull((select CONCAT(CustomSP, ',') from (
select distinct v.CustomSP+'('+v.VNContractID+')' as CustomSP from VNConsumption v,VNConsumption_Article va,VNConsumption_SizeCode vs
where v.ID = va.ID and v.ID = vs.ID and v.StyleID = o.StyleID and v.BrandID = o.BrandID and va.Article = oqd.Article and vs.SizeCode = oqd.SizeCode) a
ORDER BY CustomSP
FOR XML PATH('')),'') as CustomSP
from Order_QtyShip oq
inner join Order_QtyShip_Detail oqd on oq.Id = oqd.Id and oq.Seq = oqd.Seq
inner join Orders o on o.ID = oq.Id
left join Order_Article oa on o.ID = oa.ID and oa.Article = oqd.Article
left join Order_SizeCode os on o.ID = os.ID and os.SizeCode = oqd.SizeCode
where oq.BuyerDelivery between '{0}' and '{1}'
and {2}
and o.LocalOrder = 0
and o.Junk = 0
and oqd.Qty > 0", Convert.ToDateTime(date1).ToString("d"), Convert.ToDateTime(date2).ToString("d"), category == 0 ? "o.Category = 'B'" : category == 1 ? "o.Category = 'S'" : "(o.Category = 'B' or o.Category = 'S')"));
            if (!MyUtility.Check.Empty(mDivision))
            {
                sqlCmd.Append(string.Format(" and o.MDivisionID = '{0}'", mDivision));
            }
            if (!MyUtility.Check.Empty(factory))
            {
                sqlCmd.Append(string.Format(" and o.FactoryID = '{0}'", factory));
            }
            sqlCmd.Append(@") a
order by a.ID,a.BuyerDelivery,a.ASeq,a.SSeq");

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

            this.ShowWaitMessage("Starting EXCEL...");
            bool result = MyUtility.Excel.CopyToXls(printData, "", xltfile: "Shipping_R42_QtyBDownByColorwaySize.xltx", headerRow: 1);
            if (!result) { MyUtility.Msg.WarningBox(result.ToString(), "Warning"); }

            this.HideWaitMessage();
            return true;
        }
    }
}
