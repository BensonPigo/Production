﻿using Ict;
using Ict.Win;
using Sci.Production.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Sci.Win;
using Sci.Data;
using System.Transactions;
using Sci.Win.Tools;
using System.Linq;


namespace Sci.Production.Cutting
{
    public partial class P20_Import_Workorder : Sci.Win.Subs.Base
    {
        private string loginID = Sci.Env.User.UserID;
        private string keyWord = Sci.Env.User.Keyword;
        DataTable gridTable, detailTable, currentdetailTable;
        DataRow drCurrentMaintain;


        public P20_Import_Workorder(DataRow _drCurrentMaintain, DataTable dt)
        {
            InitializeComponent();
            drCurrentMaintain = _drCurrentMaintain;
            currentdetailTable = dt;

            if (MyUtility.Check.Empty(drCurrentMaintain["cDate"]))
            {
                dateEstCutDate.Value = DateTime.Today;
            }
            else
            {
                dateEstCutDate.Value = Convert.ToDateTime(drCurrentMaintain["cDate"]);
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

                if (MyUtility.Convert.GetInt(e.FormattedValue) + MyUtility.Convert.GetInt(dr["AccuCuttingLayer"]) > MyUtility.Convert.GetInt(dr["WorkOderLayer"]))
                {
                    MyUtility.Msg.WarningBox("Cutting Layer can not more than LackingLayers");
                    dr["CuttingLayer"] = 0;
                    dr["LackingLayers"] = MyUtility.Convert.GetInt(dr["WorkOderLayer"]) - MyUtility.Convert.GetInt(dr["AccuCuttingLayer"]) - MyUtility.Convert.GetInt(dr["CuttingLayer"]);
                    dr.EndEdit();
                    return;
                }
                dr["CuttingLayer"] = e.FormattedValue;
                dr["LackingLayers"] = MyUtility.Convert.GetInt(dr["WorkOderLayer"])- MyUtility.Convert.GetInt(dr["AccuCuttingLayer"])- MyUtility.Convert.GetInt(dr["CuttingLayer"]);

                dr.EndEdit();
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
            .Numeric("LackingLayers", header: "Lacking\r\nLayer", width: Widths.AnsiChars(5), integer_places: 8, iseditingreadonly: true)
            .Text("Colorid", header: "Color", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Numeric("Cons", header: "Cons", width: Widths.AnsiChars(10), integer_places: 7, decimal_places: 2)
            .Text("sizeRatio", header: "Size Ratio", width: Widths.AnsiChars(15), iseditingreadonly: true);
            this.gridImport.Columns["Sel"].DefaultCellStyle.BackColor = Color.Pink;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(dateEstCutDate.Value))
            {
                MyUtility.Msg.WarningBox("<Est. Cut Date> can not be empty.");
                return;
            }
            gridImport.DataSource = null;
            StringBuilder strSQLCmd = new StringBuilder();
            string estcutdate = dateEstCutDate.Text;
            string cutRef = this.txtCutRef.Text;
            string SPNo = this.txtSP.Text;
            string FactoryID = this.txtfactory.Text;
            string condition = string.Join(",", currentdetailTable.Rows.OfType<DataRow>().Select(r => "'" + (r.RowState != DataRowState.Deleted ? r["CutRef"].ToString() : "") + "'"));
            if (MyUtility.Check.Empty(condition)) condition = @"''";            
            strSQLCmd.Append(string.Format(@"
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
	CuttingLayer = a.Layer-isnull(acc.AccuCuttingLayer,0),
	LackingLayers = 0
from WorkOrder a WITH (NOLOCK)
outer apply(select AccuCuttingLayer = sum(b.Layer) from cuttingoutput_Detail b where b.WorkOrderUkey = a.Ukey)acc
where mDivisionid = '{0}' and a.estcutdate = '{1}'
and a.Layer > isnull(acc.AccuCuttingLayer,0)
and CutRef != ''
and CutRef not in ( {2} ) ", keyWord, estcutdate, condition));
            if (!MyUtility.Check.Empty(cutRef))
            {
                strSQLCmd.Append(string.Format(@" 
                and a.CutRef = '{0}'", cutRef));
            }
            if (!MyUtility.Check.Empty(SPNo))
            {
                strSQLCmd.Append(string.Format(@"
                and a.orderID='{0}'",SPNo));
            }

            if (!MyUtility.Check.Empty(FactoryID))
            {
                strSQLCmd.Append(string.Format(@"
                and a.FactoryID='{0}'", FactoryID));
            }


            strSQLCmd.Append(@" order by cutref");
            DualResult dResult = DBProxy.Current.Select(null, strSQLCmd.ToString(), out detailTable);
            if (dResult)
            {
                gridTable = detailTable.Copy();
                gridImport.DataSource = gridTable;
            }
            else
            {
                ShowErr(strSQLCmd.ToString(), dResult);
                return;
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            gridImport.ValidateControl();
            DataRow[] selDr = gridTable.Select("Sel=1","");
            if(selDr.Length>0)
            {
                foreach (DataRow dr in selDr)
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
                        ndr["WorkOderLayer"] = dr["WorkOderLayer"];
                        ndr["AccuCuttingLayer"] = dr["AccuCuttingLayer"];
                        ndr["Layer"] = dr["CuttingLayer"];
                        ndr["LackingLayers"] = dr["LackingLayers"];
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
                        exist[0]["WorkOderLayer"] = dr["WorkOderLayer"];
                        exist[0]["AccuCuttingLayer"] = dr["AccuCuttingLayer"];
                        exist[0]["Layer"] = dr["CuttingLayer"];
                        exist[0]["LackingLayers"] = dr["LackingLayers"];
                        exist[0]["Colorid"] = dr["Colorid"];
                        exist[0]["cons"] = dr["cons"];
                        exist[0]["sizeRatio"] = dr["sizeRatio"];
                    }
                    
                }
                gridTable.Clear();
            }
        }

    }
}
