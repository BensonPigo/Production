using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    /// <summary>
    /// P65_Import
    /// </summary>
    public partial class P65_Import : Win.Tems.QueryForm
    {
        private DataTable mainDetail;

        /// <summary>
        /// P65_Import
        /// </summary>
        /// <param name="mainDetail">mainDetail</param>
        public P65_Import(DataTable mainDetail)
        {
            this.InitializeComponent();
            this.EditMode = true;
            this.mainDetail = mainDetail;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.Helper.Controls.Grid.Generator(this.gridImport)
            .CheckBox("selected", trueValue: 1, falseValue: 0, iseditable: true)
            .Text("POID", header: "SP#", width: Widths.AnsiChars(11), iseditingreadonly: true)
            .Text("Refno", header: "Refno", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .EditText("Description", header: "Description", width: Widths.AnsiChars(25), iseditingreadonly: true)
            .Text("Roll", header: "Roll", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Numeric("Qty", header: "Issue Qty", decimal_places: 2, width: Widths.AnsiChars(8))
            .Text("Unit", header: "Unit", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("Location", header: "Location", width: Widths.AnsiChars(15), iseditingreadonly: true)
            ;
        }

        private void Query()
        {
            if (MyUtility.Check.Empty(this.txtSP.Text))
            {
                MyUtility.Msg.WarningBox("SP# can not be empty");
                return;
            }

            string whereRefno = MyUtility.Check.Empty(this.txtRefno.Text) ? string.Empty : $" and sfi.Refno = '{this.txtRefno.Text}' ";
            string whereLocation = MyUtility.Check.Empty(this.txtLocation.Text) ? string.Empty : $" and Location.val like '%{this.txtLocation.Text}%' ";

            string sqlQuery = $@"
select  [selected] = 0,
        sfi.POID,
        sfi.Refno,
        sf.Description,
        sfi.Roll,
        sfi.Dyelot,
        sf.Unit,
        [Qty] = sfi.InQty - sfi.OutQty + sfi.AdjustQty,
        [Location] = Location.val
from    SemiFinishedInventory sfi with (nolock)
left join   SemiFinished sf with (nolock) on sf.Refno = sfi.Refno
outer apply (SELECT val =  Stuff((select distinct concat( ',',MtlLocationID)   
                                from SemiFinishedInventory_Location sfl with (nolock)
                                where sfl.POID         = sfi.POID        and
                                      sfl.Refno        = sfi.Refno       and
                                      sfl.Roll         = sfi.Roll        and
                                      sfl.Dyelot       = sfi.Dyelot      and
                                      sfl.StockType    = sfi.StockType
                                FOR XML PATH('')),1,1,'')  ) Location
where   (sfi.InQty - sfi.OutQty + sfi.AdjustQty) > 0 and sfi.StockType = 'B' and sfi.POID = '{this.txtSP.Text}'
        {whereRefno}{whereLocation}
";
            DataTable dtResult;
            DualResult result = DBProxy.Current.Select(null, sqlQuery, out dtResult);

            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.gridImport.DataSource = dtResult;
        }

        private void BtnFind_Click(object sender, EventArgs e)
        {
            this.Query();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            var selectedItems = ((DataTable)this.gridImport.DataSource).AsEnumerable().Where(s => (int)s["selected"] == 1);

            if (!selectedItems.Any())
            {
                MyUtility.Msg.WarningBox("Please select rows first!", "Warnning");
                return;
            }

            var checkMainDetail = this.mainDetail.AsEnumerable();

            foreach (DataRow drImportSource in selectedItems)
            {
                if (checkMainDetail.Any(s => s["POID"].ToString() == drImportSource["POID"].ToString() &&
                                             s["Refno"].ToString() == drImportSource["Refno"].ToString() &&
                                             s["Roll"].ToString() == drImportSource["Roll"].ToString() &&
                                             s["Dyelot"].ToString() == drImportSource["Dyelot"].ToString()))
                {
                    continue;
                }

                DataRow drMainDetail = this.mainDetail.NewRow();
                drMainDetail["POID"] = drImportSource["POID"];
                drMainDetail["Refno"] = drImportSource["Refno"];
                drMainDetail["Roll"] = drImportSource["Roll"];
                drMainDetail["Dyelot"] = drImportSource["Dyelot"];
                drMainDetail["StockType"] = "B";
                drMainDetail["Qty"] = drImportSource["Qty"];
                drMainDetail["Description"] = drImportSource["Description"];
                drMainDetail["Unit"] = drImportSource["Unit"];
                drMainDetail["Location"] = drImportSource["Location"];
                this.mainDetail.Rows.Add(drMainDetail);
            }

            MyUtility.Msg.InfoBox("Import complete!!");
        }
    }
}
