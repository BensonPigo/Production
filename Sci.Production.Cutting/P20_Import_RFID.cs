﻿using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Cutting
{
    public partial class P20_Import_RFID : Sci.Win.Forms.Base
    {
        private string loginID = Sci.Env.User.UserID;
        private string keyWord = Sci.Env.User.Keyword;
        DataTable gridTable, detailTable, currentdetailTable;
        DataRow drCurrentMaintain;
        public P20_Import_RFID(DataRow _drCurrentMaintain, DataTable dt)
        {
            InitializeComponent();
            drCurrentMaintain = _drCurrentMaintain;
            currentdetailTable = dt;
            txtfactory1.FilteMDivision = true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            gridImport.ValidateControl();
            DataRow[] seldr = gridTable.Select("Sel=1","");
            if (seldr.Length>0)
            {
                foreach (DataRow dr in seldr)
                {
                    DataRow[] exist = currentdetailTable.Select(string.Format("WorkorderUkey={0}", dr["WorkorderUkey"]));
                    if (exist.Length == 0)
                    {
                        DataRow ndr = currentdetailTable.NewRow();
                        ndr["cutref"] = dr["Cutref"];
                        ndr["Cuttingid"] = dr["Cuttingid"];
                        ndr["OrderID"] = dr["Orderid"];
                        ndr["FabricCombo"] = dr["FabricCombo"];
                        ndr["FabricPanelCode"] = dr["FabricPanelCode"];
                        ndr["Cutno"] = dr["cutno"];
                        ndr["MarkerName"] = dr["MarkerName"];
                        ndr["MarkerLength"] = dr["MarkerLength"];
                        ndr["Layer"] = dr["CuttingLayer"];
                        ndr["Colorid"] = dr["Colorid"];
                        ndr["cons"] = dr["cons"];
                        ndr["sizeRatio"] = dr["sizeRatio"];
                        ndr["WorkorderUkey"] = dr["WorkorderUkey"];
                        currentdetailTable.Rows.Add(ndr);
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
                        exist[0]["Layer"] = dr["CuttingLayer"];
                        exist[0]["Colorid"] = dr["Colorid"];
                        exist[0]["cons"] = dr["cons"];
                        exist[0]["sizeRatio"] = dr["sizeRatio"];
                    }
                }
                gridTable.Clear();
                this.Close();
            }
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            Ict.Win.DataGridViewGeneratorNumericColumnSettings Layer = new DataGridViewGeneratorNumericColumnSettings();
            Layer.CellValidating += (s, e) =>
            {
                if (!EditMode)
                {
                    return;
                }
                if (e.RowIndex == -1)
                {
                    return;
                }
                var dr = this.gridImport.GetDataRow<DataRow>(e.RowIndex);

                if (MyUtility.Convert.GetInt(dr["Layer"]) > MyUtility.Convert.GetInt(dr["LackingLayer"]))
                {
                    MyUtility.Msg.WarningBox("Cutting Layer can not more than LackingLayer");
                    return;
                }
            };
            this.gridImport.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            Helper.Controls.Grid.Generator(this.gridImport)
            .CheckBox("Sel", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
            .Text("Cutref", header: "Cut Ref#", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Text("FactoryID", header: "Factory", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Text("Cuttingid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("OrderID", header: "Sub-SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("FabricCombo", header: "Fabric Combo", width: Widths.AnsiChars(2), iseditingreadonly: true)
            .Text("FabricPanelCode", header: "Lectra Code", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("Cutno", header: "Cut#", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("MarkerName", header: "Marker Name", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("MarkerLength", header: "Marker Length", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Numeric("WorkOderLayer", header: "WorkOder\r\nLayer", width: Widths.AnsiChars(5), integer_places: 8, iseditingreadonly: true)
            .Numeric("AccuCuttingLayer", header: "Accu. Cutting\r\nLayer", width: Widths.AnsiChars(5), integer_places: 8, iseditingreadonly: true)
            .Numeric("CuttingLayer", header: "Cutting Layer", width: Widths.AnsiChars(5), integer_places: 8, settings: Layer)
            .Numeric("LackingLayer", header: "Lacking\r\nLayer", width: Widths.AnsiChars(5), integer_places: 8, iseditingreadonly: true)
            .Text("Colorid", header: "Color", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Numeric("Cons", header: "Cons", width: Widths.AnsiChars(10), integer_places: 7, decimal_places: 2)
            .Text("sizeRatio", header: "Size Ratio", width: Widths.AnsiChars(15), iseditingreadonly: true);
            this.gridImport.Columns["Sel"].DefaultCellStyle.BackColor = Color.Pink;
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            if ((MyUtility.Check.Empty(dateRFID.Value1) && MyUtility.Check.Empty(dateRFID.Value2)) && MyUtility.Check.Empty(txtSP.Text))
            {
                MyUtility.Msg.WarningBox("<RFID Date> or <SP#> should fill one before you query data!");
                return;
            }
            gridImport.DataSource = null;
            StringBuilder sqlcmd = new StringBuilder();
            string rfidDate1 = dateRFID.Value1.ToString();
            string rfidDate2 = dateRFID.Value2.ToString();
            string spno = this.txtSP.Text;
            string factory = txtfactory1.Text;
            string condition = string.Join(",", currentdetailTable.Rows.OfType<DataRow>().Select(r => "'" + (r.RowState != DataRowState.Deleted ? r["CutRef"].ToString() : "") + "'"));
            if (MyUtility.Check.Empty(condition)) condition = @"''";

            sqlcmd.Append(string.Format(@"
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
	CuttingLayer = wo.Layer-isnull(acc.AccuCuttingLayer,0),
	LackingLayer = wo.Layer-isnull(acc.AccuCuttingLayer,0)
from WorkOrder WO WITH (NOLOCK) 
outer apply(select AccuCuttingLayer = sum(b.Layer) from cuttingoutput_Detail b where b.WorkOrderUkey = wo.Ukey)acc
where mDivisionid = '{0}' 
and wo.Layer > acc.AccuCuttingLayer
and WO.CutRef != ''
and WO.CutRef not in ( {1} )   
and WO.Ukey in ( SELECT distinct WO.ukey
FROM BundleInOut BIO
left join Bundle_Detail BD on BD.BundleNo = BIO.BundleNo
left join Bundle B on BD.Id = B.ID
left join workorder WO on WO.cutref=B.cutref and WO.cutno=B.cutno and WO.fabricCombo=B.patternPanel
where BIO.subprocessid='SORTING' 
 ", keyWord, condition));

            if (!MyUtility.Check.Empty(rfidDate1))
            {
                sqlcmd.Append(string.Format(@" and BIO.AddDate >='{0}' ", Convert.ToDateTime(rfidDate1).ToString("d")));
            }
            if (!MyUtility.Check.Empty(rfidDate2))
            {
                sqlcmd.Append(string.Format(@" and BIO.AddDate <='{0}'", Convert.ToDateTime(rfidDate2).ToString("d") + " 23:59:59"));
            }
            if (!MyUtility.Check.Empty(spno))
            {
                sqlcmd.Append(string.Format(@" and B.orderid='{0}'", spno));
            }
            if (!MyUtility.Check.Empty(factory))
            {
                sqlcmd.Append(string.Format(@" and WO.factoryid='{0}'", factory));
            }
            sqlcmd.Append(" )");
            DualResult dResult = DBProxy.Current.Select(null, sqlcmd.ToString(), out detailTable);
            if (dResult)
            {
                gridTable = detailTable.Copy();
                gridImport.DataSource = gridTable;
            }
            else
            {
                ShowErr(sqlcmd.ToString(), dResult);
                return;
            }
            if (detailTable.Rows.Count<=0)
            {
                MyUtility.Msg.InfoBox("Data not Found!");
                return;
            }
        }
    }
}
