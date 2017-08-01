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

namespace Sci.Production.PPIC
{
    public partial class P01_ProductionOutput_LoadingoutputDetail : Sci.Win.Subs.Base
    {
        string orderID, type, article, sizeCode;
        public P01_ProductionOutput_LoadingoutputDetail(string OrderID, string Type, string Article, string SizeCode)
        {
            InitializeComponent();
            orderID = OrderID;
            type = Type;
            article = Article;
            sizeCode = SizeCode;
            Text = "Loading Output Output - " + orderID;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            //設定Grid1的顯示欄位
            this.gridSewingDailyOutput.IsEditingReadOnly = true;
            this.gridSewingDailyOutput.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.gridSewingDailyOutput)
                 .DateTime("InComing", header: "InComing", width: Widths.AnsiChars(20))
                 .DateTime("OutGoing", header: "OutGoing", width: Widths.AnsiChars(20))
                 .Text("BundleNo", header: "BundleNo", width: Widths.AnsiChars(12))
                 .Text("FabricPanelCode", header: "FabricPanelCode", width: Widths.AnsiChars(2))
                 .Text("Patterncode", header: "PanelCode", width: Widths.AnsiChars(2))
                 .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(4));

            string sqlCmd;
            if (type == "A")
            {
                sqlCmd = string.Format(@"select BIO.InComing, BIO.OutGoing, BIO.BundleNo, B.FabricPanelCode, BD.Patterncode, BD.Qty
from Bundle B
left join Bundle_Detail BD on BD.Id=B.ID
left join BundleInOut BIO on BIO.BundleNo=BD.BundleNo 
where Orderid='{0}' and BIO.SubProcessId='LOADING'
order by BIO.InComing", orderID);
            }
            else
            {
                sqlCmd = string.Format(@"select BIO.InComing, BIO.OutGoing, BIO.BundleNo, B.FabricPanelCode, BD.Patterncode, BD.Qty
from Bundle B
left join Bundle_Detail BD on BD.Id=B.ID
left join BundleInOut BIO on BIO.BundleNo=BD.BundleNo 
where Orderid='{0}' and BIO.SubProcessId='LOADING' and B.Article='{1}' and BD.Sizecode='{2}'
order by BIO.InComing", orderID, article, sizeCode);
            }

            DataTable gridData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out gridData);
            listControlBindingSource1.DataSource = gridData;
        }
    }
}
