using Ict;
using Ict.Win;
using System;
using System.Data;
using System.Drawing;
using System.Text;
using Sci.Data;
using System.Linq;

namespace Sci.Production.Cutting
{
    public partial class P20_Import_Workorder : Sci.Win.Subs.Base
    {
        private string loginID = Sci.Env.User.UserID;
        private string keyWord = Sci.Env.User.Keyword;
        DataTable gridTable;
        DataTable detailTable;
        DataTable currentdetailTable;
        DataRow drCurrentMaintain;

        public P20_Import_Workorder(DataRow _drCurrentMaintain, DataTable dt)
        {
            this.InitializeComponent();
            this.drCurrentMaintain = _drCurrentMaintain;
            this.currentdetailTable = dt;

            if (MyUtility.Check.Empty(this.drCurrentMaintain["cDate"]))
            {
                this.dateEstCutDate.Value = DateTime.Today;
            }
            else
            {
                this.dateEstCutDate.Value = Convert.ToDateTime(this.drCurrentMaintain["cDate"]);
            }
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            Ict.Win.DataGridViewGeneratorNumericColumnSettings Layer = new DataGridViewGeneratorNumericColumnSettings();
            Layer.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                if (e.RowIndex == -1)
                {
                    return;
                }

                var dr = this.gridImport.GetDataRow<DataRow>(e.RowIndex);

                // if (MyUtility.Convert.GetInt(e.FormattedValue).EqualDecimal(0))
                // {
                //    MyUtility.Msg.WarningBox("Cutting layer can not be zero.");
                //    dr["CuttingLayer"] = dr["CuttingLayer", DataRowVersion.Original];
                //    e.Cancel = true;
                //    return;
                // }
                // if (MyUtility.Convert.GetInt(e.FormattedValue) + MyUtility.Convert.GetInt(dr["AccuCuttingLayer"]) > MyUtility.Convert.GetInt(dr["WorkOderLayer"]))
                // {
                //    MyUtility.Msg.WarningBox("Cutting Layer can not more than LackingLayers");
                //    dr["CuttingLayer"] = dr["CuttingLayer",DataRowVersion.Original];
                //    e.Cancel = true;
                //    return;
                // }
                dr["CuttingLayer"] = e.FormattedValue;
                dr["Cons"] = MyUtility.Convert.GetDecimal(e.FormattedValue) * MyUtility.Convert.GetDecimal(dr["ConsPC"]) * MyUtility.Convert.GetDecimal(dr["SizeRatioQty"]);
                dr["LackingLayers"] = MyUtility.Convert.GetInt(dr["WorkOderLayer"]) - MyUtility.Convert.GetInt(dr["AccuCuttingLayer"]) - MyUtility.Convert.GetInt(dr["CuttingLayer"]);

                dr.EndEdit();
            };
            this.gridImport.IsEditingReadOnly = false; // 必設定, 否則CheckBox會顯示圖示
            this.Helper.Controls.Grid.Generator(this.gridImport)
            .CheckBox("Sel", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
            .Text("Cutref", header: "Cut Ref#", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Text("FactoryID", header: "Factory", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Text("Cuttingid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("OrderID", header: "Sub-SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("FabricCombo", header: "Fabric Combo", width: Widths.AnsiChars(2), iseditingreadonly: true)
            .Text("FabricPanelCode", header: "Lectra Code", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("Cutno", header: "Cut#", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("MarkerName", header: "Marker Name", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("MarkerLength", header: "Marker Length", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Numeric("WorkOderLayer", header: "WorkOrder\r\nLayer", width: Widths.AnsiChars(5), integer_places: 8, iseditingreadonly: true)
            .Numeric("AccuCuttingLayer", header: "Accu. Cutting\r\nLayer", width: Widths.AnsiChars(5), integer_places: 8, iseditingreadonly: true)
            .Numeric("CuttingLayer", header: "Cutting Layer", width: Widths.AnsiChars(5), integer_places: 5, maximum: 99999, minimum: 0, settings: Layer)
            .Numeric("LackingLayers", header: "Lacking\r\nLayer", width: Widths.AnsiChars(5), integer_places: 8, iseditingreadonly: true)
            .Text("Colorid", header: "Color", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Numeric("Cons", header: "Cons", width: Widths.AnsiChars(10), integer_places: 7, decimal_places: 2)
            .Text("sizeRatio", header: "Size Ratio", width: Widths.AnsiChars(15), iseditingreadonly: true);
            this.gridImport.Columns["Sel"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridImport.Columns["CuttingLayer"].DefaultCellStyle.BackColor = Color.Pink;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.dateEstCutDate.Value) &&
                MyUtility.Check.Empty(this.txtSP.Text) &&
                MyUtility.Check.Empty(this.txtCutRef.Text))
            {
                MyUtility.Msg.WarningBox("<Est. Cut Date>, <SP#>, <CutRef#> can not be empty.");
                return;
            }

            this.gridImport.DataSource = null;
            StringBuilder strSQLCmd = new StringBuilder();
            DateTime? estcutdate = this.dateEstCutDate.Value;
            string cutRef = this.txtCutRef.Text;
            string SPNo = this.txtSP.Text;
            string FactoryID = this.txtfactory.Text;
            string condition = string.Join(",", this.currentdetailTable.Rows.OfType<DataRow>().Select(r => "'" + (r.RowState != DataRowState.Deleted ? r["Workorderukey"].ToString() : string.Empty) + "'"));
            if (MyUtility.Check.Empty(condition))
            {
                condition = @"''";
            }

            strSQLCmd.Append(string.Format(
                @"
Select sel = 0,
	a.*,
	Cuttingid = a.id,
	workorderukey = a.ukey,    
    OrderID = (
		Select orderid+'/' 
		From WorkOrder_Distribute c WITH (NOLOCK) 
		Where c.WorkOrderUkey =a.Ukey and orderid!='EXCESS'
		For XML path('')
    ),
    SizeRatio = (
		Select SizeCode+'/'+convert(varchar,Qty ) 
		From WorkOrder_SizeRatio c WITH (NOLOCK) 
		Where c.WorkOrderUkey =a.Ukey 
		For XML path('')
    ),
	WorkOderLayer = a.Layer,
	AccuCuttingLayer = isnull(acc.AccuCuttingLayer,0),
	CuttingLayer = final.CuttingLayer,
	LackingLayers =  a.Layer - isnull(acc.AccuCuttingLayer,0) - final.CuttingLayer,
    SRQ.SizeRatioQty
from WorkOrder a WITH (NOLOCK)
outer apply(select AccuCuttingLayer = sum(b.Layer) from cuttingoutput_Detail b where b.WorkOrderUkey = a.Ukey)acc
outer apply(select SizeRatioQty = sum(b.Qty) from WorkOrder_SizeRatio b where b.WorkOrderUkey = a.Ukey)SRQ
outer apply(
	select Qty = min(x2.Qty) -- 正常狀況,在同裁次內 每個size計算出來要一樣, 取min只是個保險
	from(
		select Qty = CEILING(iif( ws.Qty = 0, 0, cast(min(x.qty)as float) / ws.Qty)) 
		from(
			select Qty=SUM(bd.Qty),bd.SizeCode
			from Bundle b with(nolock)
			inner join Bundle_Detail bd with(nolock) on bd.Id = b.ID
			where b.CutRef = a.CutRef
			group by bd.SizeCode, bd.Patterncode, bd.PatternDesc
		)x
		left join WorkOrder_SizeRatio ws with(nolock) on x.SizeCode = ws.SizeCode and ws.WorkOrderUkey = a.Ukey
		group by x.SizeCode,ws.Qty
	)x2
)x3
outer apply(select CuttingLayer = isnull(x3.Qty,0)-isnull(acc.AccuCuttingLayer,0))cl
outer apply(select CuttingLayer = case when cl.CuttingLayer > a.Layer - isnull(acc.AccuCuttingLayer,0) then a.Layer - isnull(acc.AccuCuttingLayer,0)
                    when cl.CuttingLayer < 0 then 0
                    else cl.CuttingLayer
                    end) final
where mDivisionid = '{0}'
and a.Layer > isnull(acc.AccuCuttingLayer,0)
and CutRef != ''
and a.ukey not in ( {1} ) ", this.keyWord, condition));
            if (estcutdate.HasValue)
            {
                strSQLCmd.Append(string.Format(
                    @" 
                and a.estcutdate = '{0}'", estcutdate.Value.ToString("yyyy/MM/dd")));
            }

            if (!MyUtility.Check.Empty(cutRef))
            {
                strSQLCmd.Append(string.Format(
                    @" 
                and a.CutRef = '{0}'", cutRef));
            }

            if (!MyUtility.Check.Empty(SPNo))
            {
                strSQLCmd.Append(string.Format(
                    @"
                and a.orderID='{0}'", SPNo));
            }

            if (!MyUtility.Check.Empty(FactoryID))
            {
                strSQLCmd.Append(string.Format(
                    @"
                and a.FactoryID='{0}'", FactoryID));
            }

            strSQLCmd.Append(@" order by cutref");
            DualResult dResult = DBProxy.Current.Select(null, strSQLCmd.ToString(), out this.detailTable);
            if (dResult)
            {
                if (this.detailTable.Rows.Count == 0)
                {
                    MyUtility.Msg.InfoBox("Data not Found!");
                }

                this.gridTable = this.detailTable.Copy();
                this.gridImport.DataSource = this.gridTable;
            }
            else
            {
                this.ShowErr(strSQLCmd.ToString(), dResult);
                return;
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            this.gridImport.ValidateControl();
            DataRow[] selDr = this.gridTable.Select("Sel=1", string.Empty);
            if (selDr.Length > 0)
            {
                foreach (DataRow dr in selDr)
                {
                    DataRow[] exist = this.currentdetailTable.Select(string.Format("WorkorderUkey={0}", dr["WorkorderUkey"]));
                    if (exist.Length == 0)
                    {
                        DataRow ndr = this.currentdetailTable.NewRow();
                        ndr["cutref"] = dr["Cutref"];
                        ndr["Cuttingid"] = dr["Cuttingid"];
                        ndr["OrderID"] = dr["Orderid"];
                        ndr["FabricCombo"] = dr["FabricCombo"];
                        ndr["FabricPanelCode"] = dr["FabricPanelCode"];
                        ndr["Cutno"] = dr["cutno"];
                        ndr["MarkerName"] = dr["MarkerName"];
                        ndr["MarkerLength"] = dr["MarkerLength"];
                        ndr["WorkOderLayer"] = dr["WorkOderLayer"];
                        ndr["AccuCuttingLayer"] = dr["AccuCuttingLayer"];
                        ndr["Layer"] = dr["CuttingLayer"];
                        ndr["LackingLayers"] = dr["LackingLayers"];
                        ndr["Colorid"] = dr["Colorid"];
                        ndr["cons"] = dr["cons"];
                        ndr["sizeRatio"] = dr["sizeRatio"];
                        ndr["WorkorderUkey"] = dr["WorkorderUkey"];
                        ndr["consPC"] = dr["consPC"];
                        ndr["sizeRatioQty"] = dr["sizeRatioQty"];
                        this.currentdetailTable.Rows.Add(ndr);
                    }
                    else
                    {
                        exist[0]["cutref"] = dr["Cutref"];
                        exist[0]["Cuttingid"] = dr["Cuttingid"];
                        exist[0]["OrderID"] = dr["Orderid"];
                        exist[0]["FabricCombo"] = dr["FabricCombo"];
                        exist[0]["FabricPanelCode"] = dr["FabricPanelCode"];
                        exist[0]["Cutno"] = dr["cutno"];
                        exist[0]["MarkerName"] = dr["MarkerName"];
                        exist[0]["MarkerLength"] = dr["MarkerLength"];
                        exist[0]["WorkOderLayer"] = dr["WorkOderLayer"];
                        exist[0]["AccuCuttingLayer"] = dr["AccuCuttingLayer"];
                        exist[0]["Layer"] = dr["CuttingLayer"];
                        exist[0]["LackingLayers"] = dr["LackingLayers"];
                        exist[0]["Colorid"] = dr["Colorid"];
                        exist[0]["cons"] = dr["cons"];
                        exist[0]["sizeRatio"] = dr["sizeRatio"];
                        exist[0]["WorkorderUkey"] = dr["WorkorderUkey"];
                        exist[0]["ConsPC"] = dr["ConsPC"];
                        exist[0]["sizeRatioQty"] = dr["sizeRatioQty"];
                    }
                }

                this.gridTable.Clear();
            }
        }
    }
}
