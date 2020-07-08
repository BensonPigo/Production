using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Class;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Sci.Production.Subcon
{
    public partial class P01_AccuPoQtyList : Sci.Win.Subs.Base
    {
        protected DataRow dr;

        public P01_AccuPoQtyList(DataRow data)
        {
            this.InitializeComponent();
            this.dr = data;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            string selectCommand1 = $@"
select a.Status
,a.ID
,ad.PoQty
,[HandleName] = (select name from Pass1 where id = a.Handle)
,a.Handle
from ArtworkPo_Detail AD, ArtworkPo A
where AD.ID = A.ID 
and ad.OrderID ='{this.dr["orderID"]}'
and ad.PatternCode = '{this.dr["PatternCode"]}'";

            DataTable selectDataTable1;

            DualResult selectResult1 = DBProxy.Current.Select(null, selectCommand1, out selectDataTable1);
            if (selectResult1 == false)
            {
                this.ShowErr(selectCommand1, selectResult1);
            }

            Ict.Win.DataGridViewGeneratorTextColumnSettings col_handle = new DataGridViewGeneratorTextColumnSettings();
            col_handle.CellMouseDoubleClick += (s, e) =>
            {
                DataRow dr = this.gridReqList.GetDataRow<DataRow>(e.RowIndex);
                if (dr == null)
                {
                    return;
                }

                Sci.Production.Class.Commons.UserPrg.GetName(dr["Handle"], Sci.Production.Class.Commons.UserPrg.NameType.nameAndExt);

                string sql;
                List<SqlParameter> sqlpar = new List<SqlParameter>();

                sql = @"select 	ID, 
		                    Name, 
		                    Ext= ExtNo, 
		                    Mail = email
                    from Pass1 WITH (NOLOCK) 
                    where id = @id";
                sqlpar.Add(new SqlParameter("@id", dr["Handle"]));

                userData ud = new userData(sql, sqlpar);

                if (ud.errMsg == null)
                {
                    ud.ShowDialog();
                }
                else
                {
                    MyUtility.Msg.ErrorBox(ud.errMsg);
                }
            };

            this.bindingSource1.DataSource = selectDataTable1;

            // 設定Grid1的顯示欄位
            this.gridReqList.IsEditingReadOnly = true;
            this.gridReqList.DataSource = this.bindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridReqList)
                 .Text("Status", header: "Status", width: Widths.AnsiChars(10))
                 .Text("ID", header: "PO #", width: Widths.AnsiChars(16))
                 .Numeric("PoQty", header: "PO Qty", width: Widths.AnsiChars(8), integer_places: 6, decimal_places: 0)
                 .Text("HandleName", header: "Handle", width: Widths.AnsiChars(15), settings: col_handle)
                 ;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
