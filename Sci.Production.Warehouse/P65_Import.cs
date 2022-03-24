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

            DataGridViewGeneratorCheckBoxColumnSettings col_sel = new DataGridViewGeneratorCheckBoxColumnSettings();
            col_sel.CellValidating += (s, e) =>
            {
                this.gridImport.EndEdit();

                var upd_list = ((DataTable)this.gridImport.DataSource).AsEnumerable().Where(x => x["selected"].EqualDecimal(1)).ToList();
                if (upd_list.Count == 0)
                {
                    this.numTTLQty.Value = 0;
                    return;
                }

                decimal ttlQty = upd_list.Sum(r => Convert.ToInt32(r["Qty"]));
                this.numTTLQty.Value = ttlQty;
            };

            this.Helper.Controls.Grid.Generator(this.gridImport)
            .CheckBox("selected", trueValue: 1, falseValue: 0, iseditable: true, settings: col_sel)
            .Text("POID", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("Seq", header: "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .EditText("Desc", header: "Description", width: Widths.AnsiChars(25), iseditingreadonly: true)
            .Text("Color", header: "Color", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Text("Roll", header: "Roll", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("Tone", header: "Tone/Grp", width: Widths.AnsiChars(8), iseditingreadonly: true)
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

            string whereSeq = MyUtility.Check.Empty(this.txtSeq.Text) ? string.Empty : $" and sfi.Seq = '{this.txtSeq.Text}' ";
            string whereLocation = MyUtility.Check.Empty(this.txtLocation.Text) ? string.Empty : $" and Location.val like '%{this.txtLocation.Text}%' ";

            string sqlQuery = $@"
select  [selected] = 0,
        sfi.POID,
        sfi.Seq,
        sf.[Desc],
        sfi.Roll,
        sfi.Dyelot,
        sfi.Tone,
        sf.Color,
        sf.Unit,
        [Qty] = sfi.InQty - sfi.OutQty + sfi.AdjustQty,
        [Location] = Location.val
from    SemiFinishedInventory sfi with (nolock)
left join   SemiFinished sf with (nolock) on sf.Poid = sfi.Poid and sf.Seq = sfi.Seq
outer apply (SELECT val =  Stuff((select distinct concat( ',',MtlLocationID)   
                                from SemiFinishedInventory_Location sfl with (nolock)
                                where sfl.POID         = sfi.POID        and
                                      sfl.Seq          = sfi.Seq         and
                                      sfl.Roll         = sfi.Roll        and
                                      sfl.Dyelot       = sfi.Dyelot      and
                                      sfl.Tone         = sfi.Tone        and
                                      sfl.StockType    = sfi.StockType
                                FOR XML PATH('')),1,1,'')  ) Location
where   (sfi.InQty - sfi.OutQty + sfi.AdjustQty) > 0 and sfi.StockType = 'B' and sfi.POID = '{this.txtSP.Text}'
        {whereSeq}{whereLocation}
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
            this.numTTLQty.Value = 0;
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
                                             s["Seq"].ToString() == drImportSource["Seq"].ToString() &&
                                             s["Roll"].ToString() == drImportSource["Roll"].ToString() &&
                                             s["Dyelot"].ToString() == drImportSource["Dyelot"].ToString() &&
                                             s["Tone"].ToString() == drImportSource["Tone"].ToString()))
                {
                    continue;
                }

                DataRow drMainDetail = this.mainDetail.NewRow();
                drMainDetail["POID"] = drImportSource["POID"];
                drMainDetail["Seq"] = drImportSource["Seq"];
                drMainDetail["Roll"] = drImportSource["Roll"];
                drMainDetail["Dyelot"] = drImportSource["Dyelot"];
                drMainDetail["Color"] = drImportSource["Color"];
                drMainDetail["Tone"] = drImportSource["Tone"];
                drMainDetail["StockType"] = "B";
                drMainDetail["Qty"] = drImportSource["Qty"];
                drMainDetail["Desc"] = drImportSource["Desc"];
                drMainDetail["Unit"] = drImportSource["Unit"];
                drMainDetail["Location"] = drImportSource["Location"];
                this.mainDetail.Rows.Add(drMainDetail);
            }

            MyUtility.Msg.InfoBox("Import complete!!");
        }

        private void GridImport_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            CountSelectQty();
        }

        private void CountSelectQty()
        {
            this.gridImport.ValidateControl();
            DataGridViewColumn column = this.gridImport.Columns["selected"];
            if (!MyUtility.Check.Empty(column) && !MyUtility.Check.Empty(this.gridImport.DataSource))
            {
                var upd_list = ((DataTable)this.gridImport.DataSource).AsEnumerable().Where(x => x["selected"].EqualDecimal(1)).ToList();
                if (upd_list.Count == 0)
                {
                    this.numTTLQty.Value = 0;
                    return;
                }

                decimal ttlQty = upd_list.Sum(r => Convert.ToInt32(r["Qty"]));
                this.numTTLQty.Value = ttlQty;
            }
        }
    }
}
