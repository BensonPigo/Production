using System;
using System.Data;
using Ict.Win;
using Ict;
using Sci.Data;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// P02_CTNDimensionAndWeight
    /// </summary>
    public partial class P02_CTNDimensionAndWeight : Win.Subs.Input4
    {
        /// <summary>
        /// P02_CTNDimensionAndWeight
        /// </summary>
        /// <param name="canedit">canedit</param>
        /// <param name="keyvalue1">keyvalue1</param>
        /// <param name="keyvalue2">keyvalue2</param>
        /// <param name="keyvalue3">keyvalue3</param>
        public P02_CTNDimensionAndWeight(bool canedit, string keyvalue1, string keyvalue2, string keyvalue3)
            : base(canedit, keyvalue1, keyvalue2, keyvalue3)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override bool OnGridSetup()
        {
            this.Helper.Controls.Grid.Generator(this.grid)
                .Text("CTNNo", header: "C/No.", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Numeric("CtnLength", header: "Length (cm)", integer_places: 3, decimal_places: 2, maximum: 999.99m, minimum: 0m)
                .Numeric("CtnWidth", header: "Width (cm)", integer_places: 3, decimal_places: 2, maximum: 999.99m, minimum: 0m)
                .Numeric("CtnHeight", header: "Height (cm)", integer_places: 3, decimal_places: 2, maximum: 999.99m, minimum: 0m)
                .Numeric("CTNNW", header: "Carton Weight", integer_places: 6, decimal_places: 2, maximum: 999999.99m, minimum: 0m);
            return true;
        }

        /// <inheritdoc/>
        protected override DualResult OnSavePre()
        {
            string updatecmd = string.Empty;
            try
            {
                DataTable weightData;
                MyUtility.Tool.ProcessWithDatatable((DataTable)this.gridbs.DataSource, "CtnLength,CtnWidth,CtnHeight,CTNNW", "select isnull(sum(CTNNW),0) as CTNNW,isnull(sum((CtnLength*CtnWidth*CtnHeight)/6000),0) as VW from #tmp", out weightData);
                if (weightData.Rows.Count > 0)
                {
                    foreach (DataRow dr in weightData.Rows)
                    {
                        updatecmd = string.Format(@"update Express set CTNNW = {0}, VW = {1} where ID = '{2}'", MyUtility.Convert.GetString(dr["CTNNW"]), MyUtility.Convert.GetString(dr["VW"]), this.KeyValue1);
                    }
                }
            }
            catch (Exception ex)
            {
                DualResult failResult = new DualResult(false, "ProcessWithDatatable error.\r\n" + ex.ToString());
                return failResult;
            }

            if (updatecmd != string.Empty)
            {
                DualResult result = DBProxy.Current.Execute(null, updatecmd);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Update Express fail.\r\n" + result.ToString());
                    return failResult;
                }
            }

            return Ict.Result.True;
        }

        private void Save_Click(object sender, EventArgs e)
        {
            this.append.Visible = false;
            this.revise.Visible = false;
            this.delete.Visible = false;
        }
    }
}
