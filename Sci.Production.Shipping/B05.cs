using Sci.Data;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Ict;

namespace Sci.Production.Shipping
{
    public partial class B05 : Win.Tems.Input1
    {
      public B05(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

      protected override bool ClickSaveBefore()
        {
            return base.ClickSaveBefore();
        }

      protected override DualResult ClickSave()
        {
            // 修改表身資料,不寫入表頭EditName and EditDate
            ITableSchema pass1Schema;
            var ok = DBProxy.Current.GetTableSchema(null, "Brand", out pass1Schema);
            pass1Schema.IsSupportEditDate = false;
            pass1Schema.IsSupportEditName = false;

            // 因為不寫入表頭EditName and EditDate的設定，表頭資料不會存檔，所以這邊直接update
            string upd_brand_sql = @"update dbo.Brand set ShipLeaderEditDate = @ShipLeaderEditDate,ShipLeader = @ShipLeader where id = @ID;";
            List<System.Data.SqlClient.SqlParameter> upd_par = new List<System.Data.SqlClient.SqlParameter>()
            {
                new System.Data.SqlClient.SqlParameter("@ShipLeaderEditDate", DateTime.Now),
                new System.Data.SqlClient.SqlParameter("@ShipLeader", this.CurrentMaintain["ShipLeader"]),
                new System.Data.SqlClient.SqlParameter("@ID", this.CurrentMaintain["ID"]),
            };
            DBProxy.Current.Execute(string.Empty, upd_brand_sql, upd_par);

            return Ict.Result.True;
        }
    }
}
