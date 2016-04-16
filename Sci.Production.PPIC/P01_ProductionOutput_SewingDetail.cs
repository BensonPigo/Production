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
    public partial class P01_ProductionOutput_SewingDetail : Sci.Win.Subs.Base
    {
        string orderID, type, article, sizeCode;
        public P01_ProductionOutput_SewingDetail(string OrderID, string Type, string Article, string SizeCode)
        {
            InitializeComponent();
            orderID = OrderID;
            type = Type;
            article = Article;
            sizeCode = SizeCode;
            if (type == "A")
            {
                Text = "Sewing Daily Output - " + orderID;
            }
            else
            {
                Text = "Sewing Daily Output - " + orderID + "(" + article + "-" + sizeCode + ")";
            }
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            //設定Grid1的顯示欄位
            this.grid1.IsEditingReadOnly = true;
            this.grid1.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.grid1)
                 .Date("OutputDate", header: "Date", width: Widths.AnsiChars(12))
                 .Text("SewingLineID", header: "Line#", width: Widths.AnsiChars(5))
                 .Text("ComboType", header: "*", width: Widths.AnsiChars(2))
                 .Numeric("QAQty", header: "Q'ty", width: Widths.AnsiChars(6));

            string sqlCmd;
            if (type == "A")
            {
                sqlCmd = string.Format(@"select s.OutputDate,s.SewingLineID,sd.ComboType,sum(sd.QAQty) as QAQty
from SewingOutput_Detail sd
inner join SewingOutput s on sd.ID = s.ID
where sd.OrderId = '{0}'
group by s.OutputDate,s.SewingLineID,sd.ComboType", orderID);
            }
            else if (type == "S")
            {
                sqlCmd = string.Format(@"select s.OutputDate,s.SewingLineID,sdd.ComboType,sum(sdd.QAQty) as QAQty
from SewingOutput_Detail_Detail sdd
inner join SewingOutput s on sdd.ID = s.ID
where sdd.OrderId = '{0}'
and sdd.Article = '{1}'
and sdd.SizeCode = '{2}'
group by s.OutputDate,s.SewingLineID,sdd.ComboType", orderID, article, sizeCode);
            }
            else
            {
                sqlCmd = string.Format(@"select s.OutputDate,s.SewingLineID,sdd.ComboType,sum(sdd.QAQty) as QAQty
from SewingOutput_Detail_Detail sdd
inner join SewingOutput s on sdd.ID = s.ID
where sdd.OrderId = '{0}'
and sdd.Article = '{1}'
and sdd.SizeCode = '{2}'
and sdd.ComboType = '{3}'
group by s.OutputDate,s.SewingLineID,sdd.ComboType", orderID, article, sizeCode, type);
            }

            DataTable gridData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out gridData);
            listControlBindingSource1.DataSource = gridData;
        }
    }
}
