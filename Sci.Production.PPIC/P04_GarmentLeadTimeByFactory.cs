using System;
using System.Data;
using Ict.Win;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// P04_GarmentLeadTimeByFactory
    /// </summary>
    public partial class P04_GarmentLeadTimeByFactory : Win.Subs.Input4
    {
        /// <summary>
        /// P04_GarmentLeadTimeByFactory
        /// </summary>
        /// <param name="canedit">bool canedit</param>
        /// <param name="keyvalue1">string keyvalue1</param>
        /// <param name="keyvalue2">string keyvalue2</param>
        /// <param name="keyvalue3">string keyvalue3</param>
        public P04_GarmentLeadTimeByFactory(bool canedit, string keyvalue1, string keyvalue2, string keyvalue3)
            : base(canedit, keyvalue1, keyvalue2, keyvalue3)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override bool OnGridSetup()
        {
            this.Helper.Controls.Grid.Generator(this.grid)
                .Text("FactoryID", header: "Factory", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Numeric("GMTLT", header: "Garment L/T", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("CreateBy", header: "Create By", width: Widths.AnsiChars(30), iseditingreadonly: true)
                .Text("EditBy", header: "Edit By", width: Widths.AnsiChars(30), iseditingreadonly: true);
            return true;
        }

        /// <inheritdoc/>
        protected override void OnRequeryPost(DataTable datas)
        {
            base.OnRequeryPost(datas);
            datas.Columns.Add("CreateBy");
            datas.Columns.Add("EditBy");
            foreach (DataRow gridData in datas.Rows)
            {
                gridData["CreateBy"] = gridData["AddName"].ToString() + " - " + MyUtility.GetValue.Lookup("Name", gridData["AddName"].ToString(), "Pass1", "ID") + "   " + ((DateTime)gridData["AddDate"]).ToString(string.Format("{0}", Sci.Env.Cfg.DateTimeStringFormat));
                if (gridData["EditDate"] != System.DBNull.Value)
                {
                    gridData["EditBy"] = gridData["EditName"].ToString() + " - " + MyUtility.GetValue.Lookup("Name", gridData["EditName"].ToString(), "Pass1", "ID") + "   " + ((DateTime)gridData["EditDate"]).ToString(string.Format("{0}", Sci.Env.Cfg.DateTimeStringFormat));
                }

                gridData.AcceptChanges();
            }
        }
    }
}
