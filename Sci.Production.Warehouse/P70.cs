using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.PublicPrg;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P70 : Win.Tems.Input6
    {
        /// <inheritdoc/>
        public P70(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = $" MDivisionID = '{Env.User.Keyword}'";
            this.detailgrid.RowsAdded += this.Detailgrid_RowsAdded;
        }

        private void Detailgrid_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            DataRow curDr = this.detailgrid.GetDataRow(e.RowIndex);
            curDr["StockType"] = "B";
        }

        /// <inheritdoc/>
        public P70(ToolStripMenuItem menuitem, string transID)
        {
            this.InitializeComponent();
            this.DefaultFilter = $"ID ='{transID}' AND MDivisionID = '{Env.User.Keyword}'";
            this.IsSupportNew = false;
            this.IsSupportEdit = false;
            this.IsSupportDelete = false;
            this.IsSupportConfirm = false;
            this.IsSupportUnconfirm = false;
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["Status"] = "New";
            this.CurrentMaintain["MDivisionID"] = Env.User.Keyword;
            this.CurrentMaintain["FactoryID"] = Env.User.Factory;
            this.detailgrid.Rows.RemoveAt(0);
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (this.DetailDatas.Count == 0)
            {
                MyUtility.Msg.WarningBox("No details");
                return false;
            }

            bool isDetailKeyColEmpty = this.DetailDatas
                                        .Where(s => MyUtility.Check.Empty(s["POID"]) || MyUtility.Check.Empty(s["Seq"]) || MyUtility.Check.Empty(s["Qty"]))
                                        .Any();

            if (isDetailKeyColEmpty)
            {
                MyUtility.Msg.WarningBox("<SP#>, <Seq>, <Qty> cannot be empty.");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["ID"]))
            {
                this.CurrentMaintain["ID"] = MyUtility.GetValue.GetID(Env.User.Keyword + "SR", "SemiFinishedReceiving", (DateTime)this.CurrentMaintain["AddDate"]);
            }

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override bool ClickPrint()
        {
            if (this.CurrentMaintain["Status"].ToString() != "Confirmed")
            {
                MyUtility.Msg.WarningBox("Data is not confirmed, cannot print.");
                return false;
            }
            else
            {
                // RDLC收料報表列印，
            }

            return base.ClickPrint();
        }

        /// <inheritdoc/>
        protected override bool ClickEditBefore()
        {
            if (this.CurrentMaintain["Status"].ToString() == "Confirmed")
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't modify.", "Warning");
                return false;
            }

            return base.ClickEditBefore();
        }

        /// <inheritdoc/>
        protected override bool ClickDeleteBefore()
        {
            if (this.CurrentMaintain["Status"].ToString() == "Confirmed")
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't delete.", "Warning");
                return false;
            }

            return base.ClickDeleteBefore();
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();

            this.DetailSelectCommand = $@"
            SELECT 
            [POID] = lord.POID,
            [Seq] = Concat (lord.Seq1, ' ', lord.Seq2),
            [Seq1] = lord.Seq1,
            [Seq2] = lord.Seq2,
            [MaterialType] = IIF(lom.FabricType = 'F' ,Concat ('Fabric-', lom.MtlType),Concat ('Accessory-', lom.MtlType)),
            [Roll] = lord.Roll,
            [Dyelt] = lord.Dyelot,
            [Weight] = lord.Weight,
            [ToneGrp] = lord.Tone,
            [Qty] = lord.Qty,
            [Unit] = lom.Unit,
            [Location] = lord.Location,
            [Refno] = lom.Refno,
            [Color] = lom.Color,
            [QrCode] = lord.Barcode,
            [ContainerCode] = lord.ContainerCode
            FROM LocalOrderReceiving_Detail lord
            LEFT JOIN LocalOrderMaterial lom WITH(NOLOCK) ON lom.POID = lord.POID AND lord.Seq1 = lom.Seq1 AND lord.Seq2 = lom.Seq2
            WHERE lord.ID = '{masterID}'
            ";
            return base.OnDetailSelectCommandPrepare(e);
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();

            #region SP event
            DataGridViewGeneratorTextColumnSettings colSP = new DataGridViewGeneratorTextColumnSettings();

            colSP.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                if (this.CurrentDetailData == null)
                {
                    return;
                }

                string oldvalue = MyUtility.Convert.GetString(this.CurrentDetailData["POID"]);
                string newvalue = MyUtility.Convert.GetString(e.FormattedValue);
                if (oldvalue == newvalue)
                {
                    return;
                }

                if (MyUtility.Check.Empty(newvalue))
                {
                    return;
                }

                List<SqlParameter> par = new List<SqlParameter>() { new SqlParameter("@poid", newvalue) };
                bool isPOIDnotExists = !MyUtility.Check.Seek("select 1 from orders with (nolock) where poid = @poid and localOrder = 1", par);

                if (isPOIDnotExists)
                {
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox($"Cannot found local order {newvalue}.");
                    return;
                }

                this.CurrentDetailData["POID"] = newvalue;
            };
            #endregion
            #region Seq event
            DataGridViewGeneratorTextColumnSettings colSeq = new DataGridViewGeneratorTextColumnSettings();

            colSeq.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                if (this.CurrentDetailData == null)
                {
                    return;
                }

                string oldvalue = MyUtility.Convert.GetString(this.CurrentDetailData["Seq"]);
                string newvalue = MyUtility.Convert.GetString(e.FormattedValue);
                if (oldvalue == newvalue)
                {
                    return;
                }

                if (MyUtility.Check.Empty(newvalue))
                {
                    return;
                }

                int spaceIndex = newvalue.IndexOf(' ');
                if (spaceIndex < 0)
                {
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox($"Cannot found Seq {newvalue}");
                    return;
                }

                string seq1 = newvalue.Substring(0, spaceIndex);
                string seq2 = newvalue.Substring(spaceIndex + 1);

                List<SqlParameter> par = new List<SqlParameter>()
                {
                    new SqlParameter("@POID", this.CurrentDetailData["POID"]),
                    new SqlParameter("@Seq1", seq1),
                    new SqlParameter("@Seq2", seq2),
                };
                DataRow drSeq;

                string sqlcmd = $@"
                select
                [Seq] = Concat (lom.Seq1, ' ', lom.Seq2),
                [Material Type] = IIF(lom.FabricType = 'F' ,Concat ('Fabric-', lom.MtlType),Concat ('Accessory-', lom.MtlType)),
                [Ref#] = lom.Refno,
                [Weave Type] = WeaveType,
                [Unit] = lom.Unit, 
                [Color] = lom.Color
                from LocalOrderMaterial lom WITH(NOLOCK)
                WHERE 
                lom.POID = @POID AND
                lom.Seq1 = @Seq1 AND
                lom.Seq2 = @Seq2                
                ";
                bool isPOIDnotExists = !MyUtility.Check.Seek(sqlcmd, par, out drSeq);

                if (isPOIDnotExists)
                {
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox("Seq is not exist.");
                    return;
                }

                this.CurrentDetailData["Seq"] = newvalue;
                this.CurrentDetailData["MaterialType"] = drSeq["Material Type"].ToString();
                this.CurrentDetailData["Unit"] = drSeq["Unit"].ToString();
                this.CurrentDetailData["Refno"] = drSeq["Ref#"].ToString();
                this.CurrentDetailData["Color"] = drSeq["Color"].ToString();
            };

            colSeq.EditingMouseDown += (s, e) =>
            {
                if (this.CurrentDetailData == null)
                {
                    return;
                }

                if (!this.EditMode)
                {
                    return;
                }

                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    string sqlcmd = $@"
                    select
                    [Seq] = Concat (lom.Seq1, ' ', lom.Seq2),
                    [Material Type] = IIF(lom.FabricType = 'F' ,Concat ('Fabric-', lom.MtlType),Concat ('Accessory-', lom.MtlType)),
                    [Ref#] = lom.Refno,
                    [Weave Type] = WeaveType,
                    [Unit] = lom.Unit, 
                    [Color] = lom.Color
                    from LocalOrderMaterial lom WITH(NOLOCK)
                    WHERE lom.POID = '{this.CurrentDetailData["POID"]}'                
                    ";

                    SelectItem item = new SelectItem(sqlcmd, null, null);
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    this.CurrentDetailData["Seq"] = item.GetSelecteds()[0]["Seq"];
                    this.CurrentDetailData["MaterialType"] = item.GetSelecteds()[0]["Material Type"];
                    this.CurrentDetailData["Unit"] = item.GetSelecteds()[0]["Unit"];
                    this.CurrentDetailData["Refno"] = item.GetSelecteds()[0]["Ref#"];
                    this.CurrentDetailData["Color"] = item.GetSelecteds()[0]["Color"];
                    this.CurrentDetailData.EndEdit();
                }
            };
            #endregion
            #region Location event

            DataGridViewGeneratorTextColumnSettings colLocation = new DataGridViewGeneratorTextColumnSettings();
            colLocation.EditingMouseDown += (s, e) =>
            {
                if (this.CurrentDetailData == null)
                {
                    return;
                }

                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    Win.Tools.SelectItem2 item = Prgs.SelectLocation("B", this.CurrentDetailData["Location"].ToString());
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    this.CurrentDetailData["Location"] = item.GetSelectedString();
                    this.CurrentDetailData.EndEdit();
                }
            };

            colLocation.CellValidating += (s, e) =>
            {
                if (this.CurrentDetailData == null)
                {
                    return;
                }

                string oldvalue = MyUtility.Convert.GetString(this.CurrentDetailData["Location"]);
                string newvalue = MyUtility.Convert.GetString(e.FormattedValue);
                if (oldvalue == newvalue)
                {
                    return;
                }

                if (this.EditMode && e.FormattedValue != null)
                {
                    this.CurrentDetailData["location"] = e.FormattedValue;
                    string sqlcmd = $@"
                    SELECT id 
                    FROM MtlLocation WITH (NOLOCK)
                    WHERE 
                    StockType='B' AND
                    junk != '1'";
                    DBProxy.Current.Select(null, sqlcmd, out DataTable dt);
                    string[] getLocation = this.CurrentDetailData["location"].ToString().Split(',').Distinct().ToArray();
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
                        MyUtility.Msg.WarningBox("Cannot found Location " + string.Join(",", errLocation.ToArray()) + ".", "Data not found");
                    }

                    trueLocation.Sort();
                    this.CurrentDetailData["Location"] = string.Join(",", trueLocation.ToArray());
                }
            };
            Ict.Win.UI.DataGridViewTextBoxColumn cbb_ContainerCode;
            #endregion Location 右鍵開窗
            this.Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("POID", header: "SP#", width: Widths.AnsiChars(13), settings: colSP)
            .Text("Seq", header: "Seq", width: Widths.AnsiChars(6), settings: colSeq)
            .Text("MaterialType", header: "Material Type", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("Roll", header: "Roll", width: Widths.AnsiChars(8))
            .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(8))
            .Numeric("Weight", header: "Weight", decimal_places: 2, width: Widths.AnsiChars(8))
            .Text("Tone", header: "Tone/Grp", width: Widths.AnsiChars(8))
            .Numeric("Qty", header: "Qty", decimal_places: 2, width: Widths.AnsiChars(8))
            .Text("Unit", header: "Unit", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("Location", header: "Location", width: Widths.AnsiChars(15), settings: colLocation)
            .Text("Refno", header: "Ref#", width: Widths.AnsiChars(25), settings: colLocation)
            .Text("Color", header: "Color", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("QrCode", header: "QR Code", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("ContainerCode", header: "ContainerCode", width: Widths.AnsiChars(20), iseditingreadonly: true).Get(out cbb_ContainerCode)
            ;
            cbb_ContainerCode.Visible = Automation.UtilityAutomation.IsAutomationEnable;
        }
    }
}
