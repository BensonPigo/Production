using Ict;
using Ict.Win;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Sci.Data;
using System.Transactions;
using System.Data.SqlClient;
using System.Linq;
using Sci.Production.PublicPrg;
using System.Threading.Tasks;
using Sci.Win.Tools;

namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public partial class P08_Import : Win.Subs.Base
    {
        private DataTable gridTable;

        /// <summary>
        /// Initializes a new instance of the <see cref="P08_Import"/> class.
        /// </summary>
        public P08_Import()
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            DataGridViewGeneratorTextColumnSettings dyelot = new DataGridViewGeneratorTextColumnSettings();
            dyelot.EditingMouseDown += (s, e) =>
            {
                DataRow dr = this.gridImport.GetDataRow(e.RowIndex);
                if (e.Button == MouseButtons.Right)
                {
                    string sql = $@"
select
    [Seq#] = concat(psd.SEQ1, ' ', psd.SEQ2),
    [Bulk Location] = fid.location,
    fi.Dyelot, fi.Roll, psd.StockUnit,
    [Balance Qty] = isnull(fi.InQty,0) - isnull(fi.OutQty,0) + isnull(fi.AdjustQty,0)
from PO_Supp_Detail psd with(nolock)
inner join FtyInventory fi with(nolock) on fi.POID = psd.ID and fi.Seq1 = psd.SEQ1 and fi.Seq2 = psd.SEQ2
outer apply(
	select location = Stuff ((
            select ',' + MtlLocationID 
            from dbo.FtyInventory_Detail WITH (NOLOCK) 
            where ukey = fi.Ukey
            for xml path('')
        ), 1, 1, '')
)fid
where psd.Refno = '{dr["Ref#"]}' and psd.ColorID = '{dr["ColorID"]}' and psd.id = '{dr["CuttingID"]}'
and isnull(fi.InQty,0) - isnull(fi.OutQty,0) + isnull(fi.AdjustQty,0) > 0
";
                    SelectItem selectItem = new SelectItem(sql, string.Empty, MyUtility.Convert.GetString(dr["Dyelot"]));
                    DialogResult result = selectItem.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    dr["Dyelot"] = selectItem.GetSelecteds()[0]["Dyelot"];
                    dr.EndEdit();
                }
            };

            dyelot.CellValidating += (s, e) =>
            {
                if (MyUtility.Check.Empty(e.FormattedValue))
                {
                    return;
                }

                DataRow dr = this.gridImport.GetDataRow(e.RowIndex);
                string sql = $@"
select  1
from PO_Supp_Detail psd with(nolock)
inner join FtyInventory fi with(nolock) on fi.POID = psd.ID and fi.Seq1 = psd.SEQ1 and fi.Seq2 = psd.SEQ2
where psd.Refno = '{dr["Ref#"]}' and psd.ColorID = '{dr["ColorID"]}' and psd.id = '{dr["CuttingID"]}' and fi.Dyelot = '{e.FormattedValue}'
and isnull(fi.InQty,0) - isnull(fi.OutQty,0) + isnull(fi.AdjustQty,0) > 0
";
                if (MyUtility.Check.Seek(sql))
                {
                    dr["Dyelot"] = e.FormattedValue;
                    dr.EndEdit();
                }
                else
                {
                    MyUtility.Msg.WarningBox("Dyelot not found!");
                    e.Cancel = true;
                }
            };

            this.gridImport.IsEditingReadOnly = false; // 必設定
            this.Helper.Controls.Grid.Generator(this.gridImport)
            .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
            .Text("FactoryID", header: "Factory", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("CuttingID", header: "CuttingID", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("Fab Combo", header: "Fabric Combo", width: Widths.AnsiChars(2), iseditingreadonly: true)
            .Text("MarkerName", header: "Mark Name", width: Widths.AnsiChars(2), iseditingreadonly: true)
            .Date("Each Cons", header: "Each Cons", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Date("Mtl ETA", header: "Mtl ETA", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Date("1stSewingDate", header: "1st Sewing Date", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("Type of Cutting", header: "Type of Cutting", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Numeric("Cut Width", header: "Cut Width", width: Widths.AnsiChars(2), integer_places: 8, decimal_places: 4, iseditingreadonly: true)
            .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(8), integer_places: 5, decimal_places: 2, iseditingreadonly: true)
            .Numeric("ReleaseQty", header: "Release Qty", width: Widths.AnsiChars(8), integer_places: 5, decimal_places: 2)
            .Text("Seq1", header: "Seq1", width: Widths.AnsiChars(3), iseditingreadonly: true)
            .Text("Seq2", header: "Seq2", width: Widths.AnsiChars(2), iseditingreadonly: true)
            .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(5), settings: dyelot)
            .Date("SCI Dlv", header: "SCI Dlv.", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Date("Buyer Dlv", header: "Buyer Dlv.", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("Ref#", header: "Ref#", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("Color Desc", header: "Color Desc", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("Remark", header: "Remark", width: Widths.AnsiChars(10))
            ;

            this.gridImport.Columns["Selected"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridImport.Columns["ReleaseQty"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridImport.Columns["Dyelot"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridImport.Columns["Remark"].DefaultCellStyle.BackColor = Color.Pink;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtCuttingID.Text))
            {
                return;
            }

            this.listControlBindingSource1.DataSource = null;
            this.gridTable = null;
            List<SqlParameter> sqlParameters = new List<SqlParameter>
            {
                new SqlParameter("@ID", this.txtCuttingID.Text),
            };

            string sqlcmd = $@"
declare @CuttingID varchar(13) = @ID
select
	Selected = CAST(0 as bit)
	, o.FactoryID
	, CuttingID = @CuttingID
	, [Fab Combo] = e.FabricCombo
	, MarkerName = e.MarkerName
	, [Each Cons] = format(o.EachConsApv,'yyyy/MM/dd')
	, [Mtl ETA] = format(mtl.ETA,'yyyy/MM/dd')
	, [1stSewingDate] = format(CutCombo.[1stSewingDate],'yyyy/MM/dd')
	, [Type of Cutting] = e.Direction
	, [Cut Width] = e.CuttingWidth
	, Qty = c.YDS
	, ReleaseQty = c.YDS
	, Seq1 = mtl.Seq1
	, Seq2 = mtl.Seq2
	, [Dyelot] = ''
	, [SCI Dlv] = format(CutCombo.SciDelivery,'yyyy/MM/dd')
	, [Buyer Dlv] = format(Cutcombo.BuyerDelivery,'yyyy/MM/dd')
	, [Ref#] = Fabric.Refno
	, [Color Desc] = col.ColorDesc
	, Remark=''
	, c.ColorID
	, Order_EachConsUkey = e.Ukey
	, POID = o.POID
    , MDivisionID = (select MDivisionID from Factory where Id = o.FactoryID)
	, e.ConsPC
	, oeca.Article
from dbo.Order_EachCons e WITH (NOLOCK)
left join dbo.Order_EachCons_Color c WITH (NOLOCK) on c.Order_EachConsUkey = e.Ukey
left join dbo.Order_EachCons_Color_Article oeca WITH (NOLOCK) on oeca.Order_EachCons_ColorUkey = c.Ukey
left join dbo.Order_BOF bof WITH (NOLOCK) on bof.Id = e.Id and bof.FabricCode = e.FabricCode
left join dbo.Fabric WITH (NOLOCK) on Fabric.SCIRefno = bof.SCIRefno
left join dbo.Orders o WITH (NOLOCK) on o.ID = e.Id
outer apply(
	select 
		BuyerDelivery= min(t.BuyerDelivery)
		, SciDelivery= min(t.SciDelivery)
		, [1stSewingDate] = min(t.SewInLine)
	from dbo.Orders t WITH (NOLOCK) 
	where t.CuttingSP = @CuttingID And t.IsForecast = 0 AND t.Junk = 0 AND t.LocalOrder = 0 AND t.category not in('M','T')
) CutCombo
outer apply(
	select ETA = max(FinalETA), Seq1 = min(po3.SEQ1), Seq2 = min(po3.SEQ2)
	from (
		select s.FinalETA, s.SEQ1, s.SEQ2
		from dbo.PO_Supp_Detail s WITH (NOLOCK) 
		where s.id = o.poid AND s.SCIRefno = bof.SCIRefno AND s.ColorID = c.ColorID 
			AND s.SEQ1 = 'A1' AND ((s.Special NOT LIKE ('%DIE CUT%')) and s.Special is not null)		
		union all
		select b1.FinalETA , s.SEQ1, s.SEQ2
		from PO_Supp_Detail s WITH (NOLOCK) , PO_Supp_Detail b1 WITH (NOLOCK) 
		where s.id = o.poid AND s.SCIRefno = bof.SCIRefno AND s.ColorID = c.ColorID 
			AND s.SEQ1 = 'A1' AND ((s.Special NOT LIKE ('%DIE CUT%')) and s.Special is not null) 
			AND b1.ID = s.StockPOID   and b1.SEQ1 = s.Stockseq1 and b1.SEQ2 = s.StockSeq2
	) po3
) mtl
outer apply(
	select ColorDesc = color.Name	
	from PO_Supp_Detail b  WITH (NOLOCK) ,color WITH (NOLOCK) 
	where b.ID = o.poid and b.SEQ1 = mtl.Seq1 and b.SEQ2 = mtl.Seq2
		and color.id = b.ColorID and color.BrandId = o.brandid
) col
where e.Id = @CuttingID and e.CuttingPiece = 1 
order by MarkerName, ColorID 
";
            DualResult result = DBProxy.Current.Select(null, sqlcmd, sqlParameters, out this.gridTable);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            if (this.gridTable.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return;
            }

            DataTable dt0 = Prgs.GetCuttingTapeData(this.txtCuttingID.Text);
            if (dt0.Rows.Count > 0)
            {
                foreach (DataRow row in this.gridTable.Rows)
                {
                    var x = dt0.AsEnumerable().Where(w =>
                    MyUtility.Convert.GetString(w["MarkerName"]) == MyUtility.Convert.GetString(row["MarkerName"]) &&
                    MyUtility.Convert.GetString(w["ColorID"]) == MyUtility.Convert.GetString(row["ColorID"]) &&
                    MyUtility.Convert.GetString(w["Article"]) == MyUtility.Convert.GetString(row["Article"]));
                    if (x.Any())
                    {
                        row["remark"] = x.Select(s => new
                        {
                            remark = MyUtility.Convert.GetString(s["SP"]) + "\\" +
                                 MyUtility.Convert.GetString(s["Article"]) + "\\" +
                                 MyUtility.Convert.GetString(s["SizeCode"]) + "\\" +
                                 MyUtility.Convert.GetString(s["CutQty"]),
                        }).OrderBy(o => o.remark).Select(s => s.remark).JoinToString(",");
                    }
                }
            }

            this.listControlBindingSource1.DataSource = this.gridTable;
            this.gridImport.AutoResizeColumns();
        }

        private void DateEstCutDate_Validating(object sender, CancelEventArgs e)
        {
            // 只能輸入今天和未來的日期
            if (!MyUtility.Check.Empty(this.dateEstCutDate.Value) && ((DateTime)this.dateEstCutDate.Value).Date < DateTime.Today)
            {
                MyUtility.Msg.WarningBox("Est. Cutting Date can't earlier than today!");
                e.Cancel = true;
                return;
            }
        }

        /// <inheritdoc/>
        public List<string> ImportedIDs { get; set; } = new List<string>();

        private void BtnImport_Click(object sender, EventArgs e)
        {
            this.gridImport.ValidateControl();
            this.ImportedIDs.Clear();
            if (this.listControlBindingSource1.DataSource == null || this.gridTable.Rows.Count == 0)
            {
                return;
            }

            if (this.gridTable.Select("selected = 1").Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select data first!");
                return;
            }

            if (MyUtility.Check.Empty(this.dateEstCutDate.Value))
            {
                MyUtility.Msg.WarningBox("<Est. Cut Date> can not be empty.");
                return;
            }

            // 只能輸入今天和未來的日期
            if (!MyUtility.Check.Empty(this.dateEstCutDate.Value) && ((DateTime)this.dateEstCutDate.Value).Date < DateTime.Today)
            {
                MyUtility.Msg.WarningBox("Est. Cutting Date can't earlier than today!");
                this.dateEstCutDate.Value = null;
                return;
            }

            DataTable seldt = this.gridTable.Select("selected = 1").CopyToDataTable();

            // Dyelot 必填 && Release Qty 要大於 0
            if (seldt.AsEnumerable().Where(w => MyUtility.Check.Empty(w["dyelot"]) || MyUtility.Convert.GetDecimal(w["ReleaseQty"]) <= 0).Any())
            {
                MyUtility.Msg.WarningBox("Dyelot or Release Qty can't empty!");
                return;
            }

            #region 相同的 CuttingID, FabCombo, MarkerName, ColorID, Dyelot 只能有出現一次
            if (seldt.AsEnumerable()
                .GroupBy(g => new
                {
                    CuttingID = g["CuttingID"].ToString(),
                    FabCombo = g["Fab Combo"].ToString(),
                    MarkerName = g["MarkerName"].ToString(),
                    ColorID = g["ColorID"].ToString(),
                    Dyelot = g["Dyelot"].ToString(),
                })
                .Select(s => new
                {
                    s.Key.CuttingID,
                    s.Key.FabCombo,
                    s.Key.MarkerName,
                    s.Key.ColorID,
                    s.Key.Dyelot,
                    ct = s.Count(),
                }).Any(r => r.ct > 1))
            {
                MyUtility.Msg.WarningBox("CuttingID, Fabric Combo, MarkerName, ColorID, Dyelot can not duplicate!!");
                return;
            }
            #endregion

            #region 檢查 Release Qty, 依據 FabCombo, MarkerName, ColorID 做群組加總, Sum of Release Qty 不能大於該群組的 Qty
            var x = seldt.AsEnumerable()
                .GroupBy(g => new
                {
                    CuttingID = g["CuttingID"].ToString(),
                    FabCombo = g["Fab Combo"].ToString(),
                    MarkerName = g["MarkerName"].ToString(),
                    ColorID = g["ColorID"].ToString(),
                    Qty = MyUtility.Convert.GetDecimal(g["Qty"]),
                })
                .Select(s => new
                {
                    s.Key.CuttingID,
                    s.Key.FabCombo,
                    s.Key.MarkerName,
                    s.Key.ColorID,
                    s.Key.Qty,
                    ReleaseQty = s.Sum(i => MyUtility.Convert.GetDecimal(i["ReleaseQty"])),
                });
            if (x.Where(w => w.ReleaseQty > w.Qty).Any())
            {
                MyUtility.Msg.WarningBox("According to <Fab Combo>, <Mark Name> and <Color> the total <Release Qty> cannot be greater than <Qty>");
                return;
            }
            #endregion

            string cutTapePlanID = MyUtility.GetValue.GetID(Env.User.Keyword + "CT", "CutTapePlan");
            this.ImportedIDs.Add(cutTapePlanID);
            string insertsql = $@"
INSERT INTO [dbo].[CutTapePlan]
           ([ID]
           ,[CuttingID]
           ,[MDivisionID]
           ,[FactoryID]
           ,[EstCutDate]
           ,[Status]
           ,[AddDate]
           ,[AddName])
     VALUES
           ('{cutTapePlanID}'
           ,'{seldt.Rows[0]["CuttingID"]}'
           ,'{seldt.Rows[0]["MDivisionID"]}'
           ,'{seldt.Rows[0]["FactoryID"]}'
           ,'{((DateTime)this.dateEstCutDate.Value).ToString("yyyy/MM/dd")}'
           ,'New'
           ,getdate()
           ,'{Env.User.UserID}')

INSERT INTO [dbo].[CutTapePlan_Detail]
           ([ID]
           ,[MarkerName]
           ,[ColorID]
           ,[Dyelot]
           ,[RefNo]
           ,[Qty]
           ,[ReleaseQty]
           ,[FabricCombo]
           ,[Direction]
           ,[CuttingWidth]
           ,[ConsPC]
           ,[Seq1]
           ,[Seq2]
           ,[Remark]
           ,[Order_EachConsUkey])
select
    [ID]='{cutTapePlanID}'
    ,[MarkerName]
    ,[ColorID]
    ,[Dyelot]
    ,[Ref#]
    ,[Qty]
    ,[ReleaseQty]
    ,[Fab Combo]
    ,[Type of Cutting]
    ,[Cut Width]
    ,[ConsPC]
    ,[Seq1]
    ,[Seq2]
    ,[Remark]
    ,[Order_EachConsUkey]
from #tmp
";
            #region transaction
            DualResult upResult;
            TransactionScope transactionscope = new TransactionScope();
            using (transactionscope)
            {
                if (!(upResult = MyUtility.Tool.ProcessWithDatatable(seldt, string.Empty, insertsql, out DataTable odt)))
                {
                    transactionscope.Dispose();
                }

                transactionscope.Complete();
            }

            if (!upResult)
            {
                this.ShowErr(upResult);
                return;
            }

            MyUtility.Msg.WarningBox("Successfully");
            #endregion

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnSplit_Click(object sender, EventArgs e)
        {
            this.gridImport.ValidateControl();
            if (this.listControlBindingSource1.DataSource == null || this.gridTable.Rows.Count == 0)
            {
                return;
            }

            if (this.gridTable.Select("selected = 1").Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select data first!");
                return;
            }

            DataTable seldt = this.gridTable.Select("selected = 1").CopyToDataTable();
            Parallel.ForEach(seldt.AsEnumerable(), row =>
            {
                row["ReleaseQty"] = 0;
                row["Selected"] = 0;
            });
            this.gridTable.Merge(seldt);
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            this.gridImport.ValidateControl();
            if (this.listControlBindingSource1.DataSource == null || this.gridTable.Rows.Count == 0)
            {
                return;
            }

            if (this.gridTable.Select("selected = 1").Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select data first!");
                return;
            }

            for (int i = this.gridTable.Select("selected = 1").Count() - 1; i >= 0; i--)
            {
                this.gridTable.Select("selected = 1")[i].Delete();
            }
        }
    }
}
