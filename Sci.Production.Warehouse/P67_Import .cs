using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.PublicPrg;
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
    /// P67_Import
    /// </summary>
    public partial class P67_Import : Win.Tems.QueryForm
    {
        private DataTable mainDetail;

        /// <summary>
        /// P67_Import
        /// </summary>
        /// <param name="mainDetail">mainDetail</param>
        public P67_Import(DataTable mainDetail)
        {
            this.InitializeComponent();
            this.EditMode = true;
            this.mainDetail = mainDetail;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            #region -- To Location 右鍵開窗 --
            DataGridViewGeneratorTextColumnSettings ts2 = new DataGridViewGeneratorTextColumnSettings();
            ts2.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    Win.Tools.SelectItem2 item = Prgs.SelectLocation(this.gridImport.CurrentDataRow["stocktype"].ToString(), this.gridImport.CurrentDataRow["tolocation"].ToString());
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    this.gridImport.CurrentDataRow["tolocation"] = item.GetSelectedString();
                }
            };

            ts2.CellValidating += (s, e) =>
            {
                if (this.EditMode && e.FormattedValue != null)
                {
                    this.gridImport.CurrentDataRow["tolocation"] = e.FormattedValue;
                    string sqlcmd = string.Format(
                        @"
SELECT  id
        , Description
        , StockType 
FROM    DBO.MtlLocation WITH (NOLOCK)
WHERE   StockType='{0}'
        and junk != '1'", this.gridImport.CurrentDataRow["stocktype"].ToString());
                    DBProxy.Current.Select(null, sqlcmd, out DataTable dt);
                    string[] getLocation = this.gridImport.CurrentDataRow["tolocation"].ToString().Split(',').Distinct().ToArray();
                    bool selectId = true;
                    List<string> errLocation = new List<string>();
                    List<string> trueLocation = new List<string>();
                    foreach (string location in getLocation)
                    {
                        if (!dt.AsEnumerable().Any(row => row["id"].EqualString(location)) && !location.EqualString(string.Empty))
                        {
                            selectId &= false;
                            errLocation.Add(location);
                        }
                        else if (!location.EqualString(string.Empty))
                        {
                            trueLocation.Add(location);
                        }
                    }

                    if (!selectId)
                    {
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox("Location : " + string.Join(",", errLocation.ToArray()) + "  Data not found !!", "Data not found");
                    }

                    trueLocation.Sort();
                    this.gridImport.CurrentDataRow["tolocation"] = string.Join(",", trueLocation.ToArray());

                    // 去除錯誤的Location將正確的Location填回
                }
            };
            #endregion

            this.Helper.Controls.Grid.Generator(this.gridImport)
            .CheckBox("selected", trueValue: 1, falseValue: 0, iseditable: true)
            .Text("POID", header: "SP#", width: Widths.AnsiChars(16), iseditingreadonly: true)
            .Text("Seq", header: "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .EditText("Desc", header: "Description", width: Widths.AnsiChars(25), iseditingreadonly: true)
            .Text("Color", header: "Color", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("Roll", header: "Roll", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("Tone", header: "Tone/Grp", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Numeric("Qty", header: "Qty", decimal_places: 2, width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("Unit", header: "Unit", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("FromLocation", header: "From Location", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("ToLocation", header: "To Location", width: Widths.AnsiChars(15), settings: ts2)
            ;

            this.gridImport.Columns["ToLocation"].DefaultCellStyle.BackColor = Color.Pink;
        }

        private void Query()
        {
            if (MyUtility.Check.Empty(this.txtSP.Text) && MyUtility.Check.Empty(this.txtLocation.Text))
            {
                MyUtility.Msg.WarningBox("SP#, Location can't all be empty.");
                return;
            }

            string where = string.Empty;
            if (!MyUtility.Check.Empty(this.txtSeq.Text))
            {
                where += $" and sfi.Seq = '{this.txtSeq.Text}' ";
            }

            if (!MyUtility.Check.Empty(this.txtLocation.Text))
            {
                where += $" and Location.val like '%{this.txtLocation.Text}%' ";
            }

            if (!MyUtility.Check.Empty(this.txtColor.Text))
            {
                where += $" and sf.Color = '{this.txtColor.Text}' ";
            }

            if (!MyUtility.Check.Empty(this.txtRoll.Text))
            {
                where += $" and sfi.Roll = '{this.txtRoll.Text}' ";
            }

            if (!MyUtility.Check.Empty(this.txtDyelot.Text))
            {
                where += $" and sfi.Dyelot = '{this.txtDyelot.Text}' ";
            }

            string sqlQuery = $@"
select  [selected] = 0,
        sfi.POID,
        sfi.Seq,
        sf.Color,
        sf.[Desc],
        sfi.Roll,
        sfi.Dyelot,
        sf.Unit,
        sfi.Tone,
        [Qty] = isnull(sfi.InQty - sfi.OutQty + sfi.AdjustQty, 0),
        [FromLocation] = Location.val,
        sfi.StockType,
        tolocation = ''
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
where sfi.StockType = 'B'
{where}
";
            DualResult result = DBProxy.Current.Select(null, sqlQuery, out DataTable dtResult);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.listControlBindingSource1.DataSource = dtResult;
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
            var selectedItems = ((DataTable)this.listControlBindingSource1.DataSource).AsEnumerable().Where(s => (int)s["selected"] == 1);

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
                drMainDetail["Desc"] = drImportSource["Desc"];
                drMainDetail["Qty"] = drImportSource["Qty"];
                drMainDetail["Unit"] = drImportSource["Unit"];
                drMainDetail["FromLocation"] = drImportSource["FromLocation"];
                drMainDetail["ToLocation"] = drImportSource["ToLocation"];
                this.mainDetail.Rows.Add(drMainDetail);
            }

            MyUtility.Msg.InfoBox("Import complete!!");
        }

        private void TxtUpadteLocation_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            this.SelectToLocation();
        }

        private void TxtUpadteLocation_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.SelectToLocation();
        }

        private void SelectToLocation()
        {
            Win.Tools.SelectItem2 item = Prgs.SelectLocation("B", this.txtUpadteLocation.Text);
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.txtUpadteLocation.Text = item.GetSelectedString();
        }

        private void BtnUpdateAllLocation_Click(object sender, EventArgs e)
        {
            var selectedItems = ((DataTable)this.listControlBindingSource1.DataSource).AsEnumerable().Where(s => (int)s["selected"] == 1);
            if (!selectedItems.Any())
            {
                return;
            }

            foreach (var item in selectedItems)
            {
                item["ToLocation"] = this.txtUpadteLocation.Text;
            }
        }

        private void ChkBalance_CheckedChanged(object sender, EventArgs e)
        {
            this.listControlBindingSource1.Filter = this.chkBalance.Checked ? "Qty > 0" : string.Empty;
        }
    }
}
