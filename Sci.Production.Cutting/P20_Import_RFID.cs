using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Sci.Production.Cutting
{
    public partial class P20_Import_RFID : Win.Forms.Base
    {
        private string loginID = Env.User.UserID;
        private string keyWord = Env.User.Keyword;
        DataTable gridTable;
        DataTable detailTable;
        DataTable currentdetailTable;
        DataRow drCurrentMaintain;

        public P20_Import_RFID(DataRow _drCurrentMaintain, DataTable dt)
        {
            this.InitializeComponent();
            this.drCurrentMaintain = _drCurrentMaintain;
            this.currentdetailTable = dt;
            this.txtfactory1.FilteMDivision = true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            this.gridImport.ValidateControl();
            DataRow[] seldr = this.gridTable.Select("Sel=1", string.Empty);
            if (seldr.Length > 0)
            {
                foreach (DataRow dr in seldr)
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
                        ndr["ConsPC"] = dr["ConsPC"];
                        ndr["SizeRatioQty"] = dr["SizeRatioQty"];
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
                        exist[0]["SizeRatioQty"] = dr["SizeRatioQty"];
                    }
                }

                this.gridTable.Clear();
                this.Close();
            }
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            DataGridViewGeneratorNumericColumnSettings Layer = new DataGridViewGeneratorNumericColumnSettings();
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
                //    dr["CuttingLayer"] = dr["CuttingLayer", DataRowVersion.Original];
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

        private void btnQuery_Click(object sender, EventArgs e)
        {
            if ((MyUtility.Check.Empty(this.dateRFID.Value1) && MyUtility.Check.Empty(this.dateRFID.Value2)) && MyUtility.Check.Empty(this.txtSP.Text))
            {
                MyUtility.Msg.WarningBox("<RFID Date> or <SP#> should fill one before you query data!");
                return;
            }

            this.gridImport.DataSource = null;
            StringBuilder sqlcmd = new StringBuilder();
            string rfidDate1 = this.dateRFID.Value1.ToString();
            string rfidDate2 = this.dateRFID.Value2.ToString();
            string spno = this.txtSP.Text;
            string factory = this.txtfactory1.Text;
            StringBuilder woUkeyCondition = new StringBuilder();
            string condition = string.Join(",", this.currentdetailTable.Rows.OfType<DataRow>().Select(r => "'" + (r.RowState != DataRowState.Deleted ? r["Workorderukey"].ToString() : string.Empty) + "'"));
            if (MyUtility.Check.Empty(condition))
            {
                condition = @"''";
            }

            if (!MyUtility.Check.Empty(rfidDate1))
            {
                woUkeyCondition.Append(string.Format(@" and BIO.AddDate >='{0}' ", Convert.ToDateTime(rfidDate1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(rfidDate2))
            {
                woUkeyCondition.Append(string.Format(@" and BIO.AddDate <='{0}'", Convert.ToDateTime(rfidDate2).ToString("d") + " 23:59:59"));
            }

            if (!MyUtility.Check.Empty(spno))
            {
                woUkeyCondition.Append(string.Format(@" and B.orderid='{0}'", spno));
            }

            if (!MyUtility.Check.Empty(factory))
            {
                woUkeyCondition.Append(string.Format(@" and WO.factoryid='{0}'", factory));
            }

            sqlcmd.Append(string.Format(
                @"

SELECT distinct WO.ukey
    into #QueryTarUkey
    FROM BundleInOut BIO WITH (NOLOCK) 
    inner join Bundle_Detail BD WITH (NOLOCK)  on BD.BundleNo = BIO.BundleNo
    inner join Bundle B WITH (NOLOCK)  on BD.Id = B.ID and B.cutref <> ''
    inner join workorder WO WITH (NOLOCK)  on WO.cutref=B.cutref and WO.ID=B.POID and WO.MDivisionID  = b.MDivisionID 
    where BIO.subprocessid='SORTING' 
    and isnull(bio.RFIDProcessLocationID,'') = '' {2}

select b.WorkOrderUkey,AccuCuttingLayer = sum(b.Layer) 
	into #AccuCuttingLayer
	from cuttingoutput_Detail b WITH (NOLOCK) 
	inner join  #QueryTarUkey a on a.ukey = b.WorkOrderUkey 
	group by b.WorkOrderUkey

select 0 as sel,
	WO.*,
	Cuttingid = WO.id,
	workorderukey = WO.ukey,
	OrderID = (
		Select orderid+'/' 
		From WorkOrder_Distribute c WITH (NOLOCK) 
		Where c.WorkOrderUkey =WO.Ukey and orderid!='EXCESS'
		For XML path('')
	),
	SizeRatio = (
		Select SizeCode+'/'+convert(varchar,Qty ) 
		From WorkOrder_SizeRatio c WITH (NOLOCK) 
		Where c.WorkOrderUkey =WO.Ukey 
		For XML path('')
	),
	WorkOderLayer = wo.Layer,
	AccuCuttingLayer = isnull(acc.AccuCuttingLayer,0),
	CuttingLayer = case when cl.CuttingLayer > WO.Layer - isnull(acc.AccuCuttingLayer,0) then WO.Layer - isnull(acc.AccuCuttingLayer,0)
                    when cl.CuttingLayer < 0 then 0
                    else cl.CuttingLayer
                    end,
	LackingLayers =  wo.Layer - isnull(acc.AccuCuttingLayer,0) - final.CuttingLayer,
    SRQ.SizeRatioQty
from WorkOrder WO WITH (NOLOCK) 
left join #AccuCuttingLayer acc on wo.Ukey = acc.WorkOrderUkey
outer apply(select SizeRatioQty = sum(b.Qty) from WorkOrder_SizeRatio b WITH (NOLOCK)  where b.WorkOrderUkey = wo.Ukey)SRQ
outer apply(
	select Qty = min(x2.Qty) -- 正常狀況,在同裁次內 每個size計算出來要一樣, 取min只是個保險
	from(
		select Qty = CEILING(iif( ws.Qty = 0, 0, cast(min(x.qty)as float) / ws.Qty)) 
		from(
			select Qty=SUM(bd.Qty),bd.SizeCode
			from Bundle b with(nolock)
			inner join Bundle_Detail bd with(nolock) on bd.Id = b.ID
			where b.CutRef = WO.CutRef
			group by bd.SizeCode, bd.Patterncode, bd.PatternDesc
		)x
		left join WorkOrder_SizeRatio ws with(nolock) on x.SizeCode = ws.SizeCode and ws.WorkOrderUkey = WO.Ukey
		group by x.SizeCode,ws.Qty
	)x2
)x3
outer apply(select CuttingLayer = isnull(x3.Qty,0)-isnull(acc.AccuCuttingLayer,0))cl
outer apply(select CuttingLayer = case when cl.CuttingLayer > WO.Layer - isnull(acc.AccuCuttingLayer,0) then WO.Layer - isnull(acc.AccuCuttingLayer,0)
                    when cl.CuttingLayer < 0 then 0
                    else cl.CuttingLayer
                    end) final
where mDivisionid = '{0}' 
and wo.Layer >  isnull(acc.AccuCuttingLayer,0)
and WO.CutRef != ''
and WO.ukey not in ( {1} )   
and exists (select 1 from #QueryTarUkey where Ukey = WO.Ukey)

Drop table #QueryTarUkey,#AccuCuttingLayer
 ", this.keyWord, condition, woUkeyCondition.ToString()));

            DualResult dResult = DBProxy.Current.Select(null, sqlcmd.ToString(), out this.detailTable);
            if (dResult)
            {
                this.gridTable = this.detailTable.Copy();
                this.gridImport.DataSource = this.gridTable;
            }
            else
            {
                this.ShowErr(sqlcmd.ToString(), dResult);
                return;
            }

            if (this.detailTable.Rows.Count <= 0)
            {
                MyUtility.Msg.InfoBox("Data not Found!");
                return;
            }
        }
    }
}
