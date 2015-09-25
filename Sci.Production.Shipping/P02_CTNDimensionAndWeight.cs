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
    public partial class P02_CTNDimensionAndWeight : Sci.Win.Subs.Input4
    {
        public P02_CTNDimensionAndWeight(bool canedit, string keyvalue1, string keyvalue2, string keyvalue3)
            : base(canedit, keyvalue1, keyvalue2, keyvalue3)
        {
            InitializeComponent();
        }

        protected override bool OnGridSetup()
        {
            Helper.Controls.Grid.Generator(this.grid)
                .Text("CTNNo", header: "C/No.", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Numeric("CtnLength", header: "Length (cm)", integer_places: 3, decimal_places: 2, maximum: 999.99m, minimum: 0m)
                .Numeric("CtnWidth", header: "Width (cm)", integer_places: 3, decimal_places: 2, maximum: 999.99m, minimum: 0m)
                .Numeric("CtnHeight", header: "Height (cm)", integer_places: 3, decimal_places: 2, maximum: 999.99m, minimum: 0m)
                .Numeric("CTNNW", header: "Carton Weight", integer_places: 6, decimal_places: 2, maximum: 999999.99m, minimum: 0m);
            return true;
        }

        protected override bool OnSavePre()
        {
            string updatecmd = "";
            try
            {
                DataTable weightData;
                MyUtility.Tool.ProcessWithDatatable((DataTable)gridbs.DataSource, "CtnLength,CtnWidth,CtnHeight,CTNNW", "select isnull(sum(CTNNW),0) as CTNNW,isnull(sum((CtnLength*CtnWidth*CtnHeight)/6000),0) as VW from #tmp", out weightData);
                if (weightData.Rows.Count > 0)
                {
                    foreach (DataRow dr in weightData.Rows)
                    {
                        updatecmd = string.Format(@"update Express set CTNNW = {0}, VW = {1} where ID = '{2}'", dr["CTNNW"].ToString(), dr["VW"].ToString(), KeyValue1);
                    }
                }
            }
            catch (Exception ex)
            {
                MyUtility.Msg.ErrorBox("ProcessWithDatatable error.\r\n" + ex.ToString());
                return false;
            }

            if (updatecmd != "")
            {
                DualResult result = DBProxy.Current.Execute(null, updatecmd);
                if (!result)
                {
                    MyUtility.Msg.WarningBox("Update Express fail.\r\n" + result.ToString());
                    return false;
                }
            }

            return true;
        }


        private void save_Click(object sender, EventArgs e)
        {
            append.Visible = false;
            revise.Visible = false;
            delete.Visible = false;
        }
    }
}
