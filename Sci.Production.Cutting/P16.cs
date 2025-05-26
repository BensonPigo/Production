using Ict;
using Ict.Win;
using Sci.Andy.ExtensionMethods;
using Sci.Data;
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

namespace Sci.Production.Cutting
{
    /// <summary>
    /// P16
    /// </summary>
    public partial class P16 : Win.Tems.QueryForm
    {
        /// <summary>
        /// P16
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P16(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.EditMode = true;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            DataGridViewGeneratorTextColumnSettings unfinishedCuttingReasonSetting = new DataGridViewGeneratorTextColumnSettings();

            unfinishedCuttingReasonSetting.EditingMouseDown += (s, e) =>
            {
                if (e.Button != MouseButtons.Right)
                {
                    return;
                }

                string sqlGetSelectItem = @"select ID, Name from DropDownList with (nolock) where type = 'PMS_UnFinCutReason' order by Seq";
                SelectItem selectItem = new SelectItem(sqlGetSelectItem, "0, 30", null);

                DialogResult dialogResult = selectItem.ShowDialog();

                if (dialogResult != DialogResult.OK)
                {
                    return;
                }

                DataRow curRow = this.gridCuttingReasonInput.GetDataRow(e.RowIndex);

                curRow["UnfinishedCuttingReasonDesc"] = selectItem.GetSelecteds()[0]["Name"];
                curRow["UnfinishedCuttingReason"] = selectItem.GetSelecteds()[0]["ID"];
            };

            unfinishedCuttingReasonSetting.CellValidating += (s, e) =>
            {
                if (MyUtility.Check.Empty(e.FormattedValue))
                {
                    return;
                }

                DataRow dr = this.gridCuttingReasonInput.GetDataRow<DataRow>(e.RowIndex);
                List<SqlParameter> listPar = new List<SqlParameter>() { new SqlParameter("@Name", e.FormattedValue) };
                string selcmd = @"select ID, Name from DropDownList with (nolock) where type = 'PMS_UnFinCutReason' and Name = @Name";
                DualResult result = DBProxy.Current.Select(null, selcmd, listPar, out DataTable dt);
                if (!result)
                {
                    this.ShowErr(result);
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox($"Reason {e.FormattedValue} not found!");
                    dr["UnfinishedCuttingReasonDesc"] = string.Empty;
                    dr["UnfinishedCuttingReason"] = string.Empty;
                }
                else
                {
                    dr["UnfinishedCuttingReasonDesc"] = dt.Rows[0]["Name"];
                    dr["UnfinishedCuttingReason"] = dt.Rows[0]["ID"];
                }

                dr.EndEdit();
            };

            this.Helper.Controls.Grid.Generator(this.gridCuttingReasonInput)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
                .Text("MDivisionID", header: "M", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("FtyGroup", header: "Factory", width: Widths.Auto(), iseditingreadonly: true)
                .Text("WeaveTypeID", header: "Fabrication", width: Widths.Auto(), iseditingreadonly: true)
                .Date("FinalETA", header: "ETA", width: Widths.Auto(), iseditingreadonly: true)
                .Date("EstCutDate", header: "Est." + Environment.NewLine + "Cutting Date", width: Widths.Auto(), iseditingreadonly: true)
                .Text("ID", header: "SP#", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("BrandID", header: "Brand", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("StyleID", header: "Style#", width: Widths.AnsiChars(12), iseditingreadonly: true)
                .Text("Refno", header: "FabRef#", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("CutRef", header: "Ref#", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Cutno", header: "Cut#", width: Widths.AnsiChars(2), iseditingreadonly: true)
                .Text("FabricCombo", header: "Combination", width: Widths.Auto(), iseditingreadonly: true)
                .Text("ColorWay", header: "Color Way", width: Widths.Auto(), iseditingreadonly: true)
                .Text("ColorID", header: "Color", width: Widths.Auto(), iseditingreadonly: true)
                .Text("Layer", header: "Layer", width: Widths.Auto(), iseditingreadonly: true)
                .Text("LackingLayers", header: "Lacking Layer", width: Widths.AnsiChars(12), iseditingreadonly: true)
                .Text("Size", header: "Size", width: Widths.AnsiChars(12), iseditingreadonly: true)
                .Numeric("BalanceCons", header: "Balance Cons.", width: Widths.AnsiChars(6), decimal_places: 4, integer_places: 5, iseditingreadonly: true)
                .Date("BuyerDelivery", header: "Buyer Delivery", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("UnfinishedCuttingReasonDesc", width: Widths.AnsiChars(30), header: "Reason", settings: unfinishedCuttingReasonSetting)
                .EditText("UnfinishedCuttingRemark", header: "Remark", width: Widths.AnsiChars(30))
                ;

            this.gridCuttingReasonInput.Columns["UnfinishedCuttingReasonDesc"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridCuttingReasonInput.ColumnFrozen(this.gridCuttingReasonInput.Columns["Selected"].Index);
            this.gridCuttingReasonInput.Columns["UnfinishedCuttingRemark"].DefaultCellStyle.BackColor = Color.Pink;
        }

        private void Query()
        {
            if (!this.dateEstCutDate.HasValue && !this.dateEstCutDate.HasValue1 &&
                this.txtSPNo.Text.IsEmpty() && this.dateETA.Value.IsEmpty())
            {
                MyUtility.Msg.WarningBox("Please input <Est. Cut Date> or <ETA> or <SP#> first!");
                return;
            }

            string sqlGetDate = string.Empty;
            string sqlWhere = string.Empty;
            List<SqlParameter> listPar = new List<SqlParameter>();

            if (this.dateEstCutDate.HasValue1)
            {
                sqlWhere += " and w.EstCutDate >= @EstCutDateFrom";
                listPar.Add(new SqlParameter("@EstCutDateFrom", this.dateEstCutDate.DateBox1.Value));
            }

            if (this.dateEstCutDate.HasValue2)
            {
                sqlWhere += " and w.EstCutDate <= @EstCutDateTo";
                listPar.Add(new SqlParameter("@EstCutDateTo", this.dateEstCutDate.DateBox2.Value));
            }

            if (!this.txtfactory.Text.Empty())
            {
                sqlWhere += " and o.FtyGroup = @FtyGroup";
                listPar.Add(new SqlParameter("@FtyGroup", this.txtfactory.Text));
            }

            if (!this.dateETA.Value.IsEmpty())
            {
                sqlWhere += " and psd.FinalETA = @ETA";
                listPar.Add(new SqlParameter("@ETA", this.dateETA.Value));
            }

            if (!this.txtSPNo.Text.IsEmpty())
            {
                sqlWhere += " and o.ID = @SPNo";
                listPar.Add(new SqlParameter("@SPNo", this.txtSPNo.Text));
            }

            sqlGetDate = $@"
select
    [Selected] = 0,
	w.MDivisionID,
	o.FtyGroup,
    f.WeaveTypeID,
    psd.FinalETA,
	w.EstCutDate,
	w.ID,
    o.BrandID,
	o.StyleID,
    w.Refno,
	w.CutRef,
    w.Cutno,
	w.FabricCombo,
	[ColorWay] = stuff(Article.Article,1,1,''),
	w.ColorID,   
    [Layer] = SUM(w.Layer),
    Artwork.Artwork,
    [Size] = Size.Size,
    [LackingLayers] = SUM(w.Layer - isnull(acc.AccuCuttingLayer,0)),  
    w.UnfinishedCuttingReason,
    [BalanceCons] = SUM(w.Cons - [ActConsOutput]),
    o.BuyerDelivery,
    [UnfinishedCuttingReasonDesc] = dw.Name,
    w.UnfinishedCuttingRemark
from WorkOrderForOutput w WITH (NOLOCK) 
inner join Orders o WITH (NOLOCK) on o.id = w.ID
inner join Cutting c WITH (NOLOCK) on c.ID = o.CuttingSP
left join DropDownList dw with (nolock) on dw.Type = 'PMS_UnFinCutReason' and dw.ID = w.UnfinishedCuttingReason
left join fabric f WITH (NOLOCK) on f.SCIRefno = w.SCIRefno
left join PO_Supp_Detail psd with(nolock) on psd.id = w.id and psd.seq1 = w.seq1 and psd.seq2 = w.seq2
outer apply(select AccuCuttingLayer = sum(aa.Layer) from cuttingoutput_Detail aa WITH (NOLOCK) where aa.WorkOrderForOutputUkey = w.Ukey)acc
outer apply(select YDSMarkerLength = dbo.MarkerLengthToYDS(w.MarkerLength)) ML
outer apply(select ActConsOutput = cast(isnull(iif(w.Layer - isnull(acc.AccuCuttingLayer,0) = 0, w.Cons, acc.AccuCuttingLayer * ML.YDSMarkerLength),0) as numeric(9,4)))ActConsOutput
outer apply(
select Size = stuff((
	Select concat(', ' , c.sizecode, '/ ', c.qty)
	From WorkOrderForOutput_SizeRatio c WITH (NOLOCK)
	Where c.WorkOrderForOutputUkey =w.Ukey
	For XML path('')
	),1,1,'')
)Size
outer apply(
	select Article = (
		select distinct concat('/',Article)
		from WorkOrderForOutput_Distribute WITH (NOLOCK) 
		where WorkOrderForOutputUKey = w.UKey
		and Article != ''
		for xml path('')
	)
)as Article
outer apply(
	SELECT TOP 1 SizeGroup=IIF(ISNULL(SizeGroup,'')='','N',SizeGroup)
	FROM Order_SizeCode WITH (NOLOCK)
	WHERE ID = o.POID and SizeCode IN 
	(
		select distinct wd.SizeCode
		from WorkOrderForOutput_Distribute wd WITH (NOLOCK)
		where wd.WorkOrderForOutputUkey = w.Ukey
	)
) as ss
outer apply(select p.PatternUkey from dbo.GetPatternUkey(o.POID,'',w.MarkerNo,o.StyleUkey,ss.SizeGroup) p ) p
outer apply(
	select Artwork=stuff((
	select distinct concat('+',s.data)
	from(
		select distinct pg.Annotation
		from Pattern_GL_LectraCode pgl WITH (NOLOCK)
		inner join Pattern_GL pg WITH (NOLOCK) on pgl.PatternUKEY = pg.PatternUKEY
									            and pgl.seq = pg.SEQ
									            and pg.Annotation is not null
									            and pg.Annotation!=''
		where pgl.PatternUKEY = p.patternUKey and pgl.FabricPanelCode = w.FabricPanelCode
	)a
	outer apply(select data=RTRIM(LTRIM(data)) from SplitString(dbo.[RemoveNumericCharacters](a.Annotation),'+'))s
	where exists(select 1 from SubProcess WITH (NOLOCK) where id = s.data)
	for xml path(''))
	,1,1,'')
)Artwork
where 1=1
{sqlWhere}
and (w.Layer - isnull(acc.AccuCuttingLayer,0)) > 0

group by w.MDivisionID,o.FtyGroup, f.WeaveTypeID,psd.FinalETA,w.EstCutDate,w.ID,o.BrandID,o.StyleID,
    w.Refno,w.CutRef,w.Cutno,w.FabricCombo,Article.Article,w.ColorID,Artwork.Artwork,Size.Size,
	w.UnfinishedCuttingReason,o.BuyerDelivery, dw.Name, w.UnfinishedCuttingRemark
order by w.EstCutDate asc,w.ID asc,w.FabricCombo asc";

            DataTable dtResult;
            DualResult result = DBProxy.Current.Select(null, sqlGetDate, listPar, out dtResult);

            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.gridCuttingReasonInput.DataSource = dtResult;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            this.Query();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (this.gridCuttingReasonInput.DataSource == null)
            {
                return;
            }

            var needSaveData = ((DataTable)this.gridCuttingReasonInput.DataSource).AsEnumerable().Where(s => s.RowState == DataRowState.Modified);

            if (!needSaveData.Any())
            {
                MyUtility.Msg.InfoBox("No data need save");
                return;
            }

            string sqlUpdate = string.Empty;

            foreach (var needSaveItem in needSaveData)
            {
                sqlUpdate += $@"
update WorkOrderForOutput set UnfinishedCuttingReason = '{needSaveItem["UnfinishedCuttingReason"]}', UnfinishedCuttingRemark = '{needSaveItem["UnfinishedCuttingRemark"]}' where CutRef = '{needSaveItem["CutRef"]}' and ID = '{needSaveItem["ID"]}'
";
            }

            if (!MyUtility.Check.Empty(sqlUpdate))
            {
                DualResult result = DBProxy.Current.Execute(null, sqlUpdate);
                if (!result)
                {
                    this.ShowErr(result);
                    return;
                }

                MyUtility.Msg.InfoBox("Save success.");
                this.Query();
            }
        }

        private void TxtReason_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sqlcmd = @"select Name from DropDownList with (nolock) where type = 'PMS_UnFinCutReason' order by Seq";
            SelectItem item = new SelectItem(sqlcmd, "35",null, headercaptions: "Name");
            item.Width = 400;
            DialogResult dialogResult = item.ShowDialog();
            if (dialogResult == DialogResult.Cancel)
            {
                return;
            }

            this.txtReason.Text = item.GetSelectedString();
        }

        private void btnBatchUpdate_Click(object sender, EventArgs e)
        {
            this.gridCuttingReasonInput.ValidateControl();
            string reasonName = this.txtReason.Text.Trim();
            string reasonID = MyUtility.GetValue.Lookup($@"select ID from DropDownList with (nolock) where type = 'PMS_UnFinCutReason' and Name='{reasonName}'");
            string remark = this.txtRemark.Text;
            DataTable dt = (DataTable)this.gridCuttingReasonInput.DataSource;
            if (dt == null || dt.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Please select data first!");
                return;
            }

            DataRow[] selectedData = dt.Select("Selected = 1");
            if (selectedData.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select data first!");
                return;
            }

            foreach (DataRow currentRecord in selectedData)
            {
                currentRecord["UnfinishedCuttingReason"] = reasonID;
                currentRecord["UnfinishedCuttingReasonDesc"] = reasonName;
                currentRecord["UnfinishedCuttingRemark"] = remark;
                currentRecord.EndEdit();
            }

            this.gridCuttingReasonInput.SuspendLayout();
        }

        private void txtReason_Validating(object sender, CancelEventArgs e)
        {
            if (this.txtReason.Text.IsEmpty())
            {
                return;
            }

            List<SqlParameter> listPar = new List<SqlParameter>() { new SqlParameter("@Name", this.txtReason.Text) };
            string selcmd = @"select Name from DropDownList with (nolock) where type = 'PMS_UnFinCutReason' and Name = @Name";
            DualResult result = DBProxy.Current.Select(null, selcmd, listPar, out DataTable dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            if (dt.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox($"Reason {this.txtReason.Text} not found!");
                this.txtReason.Text = string.Empty;
                this.txtReason.Focus();
            }
            else
            {
                this.txtReason.Text = dt.Rows[0]["Name"].ToString();
            }
        }
    }
}
