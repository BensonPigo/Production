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


namespace Sci.Production.Subcon
{
    public partial class P30_InComingList : Sci.Win.Subs.Base
    {
        protected string Detailukey;
        public P30_InComingList(string _detailukey)
        {
            InitializeComponent();
            Detailukey = _detailukey;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            string selectCommand1 = string.Format(@"
                    select LD.Id,L.IssueDate,LD.Qty,L.Status,Convert(date,L.AddDate) AS AddDate
                    from LocalReceiving_Detail LD
                    inner join LocalReceiving L on L.Id=LD.Id 
                    where LD.LocalPo_detailukey= '{0}'"
                                , Detailukey);
            DataTable selectDataTable1;

            DualResult selectResult1 = DBProxy.Current.Select(null, selectCommand1, out selectDataTable1);
            if (selectResult1 == false) ShowErr(selectCommand1,selectResult1);

            bindingSource1.DataSource = selectDataTable1;

            //設定Grid1的顯示欄位
            this.gridFarmInList.IsEditingReadOnly = true;
            this.gridFarmInList.DataSource = bindingSource1;
            Helper.Controls.Grid.Generator(this.gridFarmInList)
                 .Text("id", header: "In-coming No.", width: Widths.AnsiChars(15))
                 .Date("issuedate", header: "Date", width: Widths.AnsiChars(10))
                 .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(6), integer_places: 6, decimal_places: 0)
                 .Text("Status", header: "Status", width: Widths.AnsiChars(8))
                 .Date("adddate", header: "Create Date", width: Widths.AnsiChars(10));
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
