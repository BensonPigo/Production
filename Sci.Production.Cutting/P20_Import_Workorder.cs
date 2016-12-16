using Ict;
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
                dateBox1.Value = DateTime.Today;
            }
            else
            {
                dateBox1.Value = Convert.ToDateTime(drCurrentMaintain["cDate"]);
            }

        }

        protected override void OnFormLoaded()
        {
            DBProxy.Current.Select(null,
            @"Select 0 as Sel, '' as cutref,'' as cuttingid,'' as orderid,'' as Fabriccombo,
            '' as LectraCode,'' as cutno, '' as MarkerName, '' as MarkerLength, '' as Colorid, 0 as Layer,
            0 as Cons, '' as Ratio from Workorder where 1=0", out gridTable);
            base.OnFormLoaded();
            this.grid1.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.grid1.DataSource = gridTable;
            Helper.Controls.Grid.Generator(this.grid1)
            .CheckBox("Sel", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
            .Text("Cutref", header: "Cut Ref#", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Text("Cuttingid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("OrderID", header: "Sub-SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("FabricCombo", header: "Fabric Combo", width: Widths.AnsiChars(2), iseditingreadonly: true)
            .Text("LectraCode", header: "Lectra Code", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("Cutno", header: "Cut#", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("MarkerName", header: "Marker Name", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("MarkerLength", header: "Marker Length", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Numeric("Layer", header: "Layers", width: Widths.AnsiChars(5), integer_places: 8, iseditingreadonly: true)
            .Text("Colorid", header: "Color", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Numeric("Cons", header: "Cons", width: Widths.AnsiChars(10), integer_places: 7, decimal_places: 2)
            .Text("sizeRatio", header: "Size Ratio", width: Widths.AnsiChars(15), iseditingreadonly: true);
            this.grid1.Columns[0].DefaultCellStyle.BackColor = Color.Pink;

        }

        private void Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Query_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(dateBox1.Value))
            {
                MyUtility.Msg.WarningBox("<Est. Cut Date> can not be empty.");
                return;
            }
            gridTable.Clear();
            string estcutdate = dateBox1.Text;
            string condition = string.Join(",", currentdetailTable.Rows.OfType<DataRow>().Select(r => "'" + r["CutRef"].ToString() + "'"));
            if (MyUtility.Check.Empty(condition)) condition = @"''";
            string sqlcmd = string.Format(
                @"Select 0 as sel,a.*, a.id as Cuttingid,a.ukey as workorderukey,    
                (
                Select orderid+'/' 
                From WorkOrder_Distribute c
                Where c.WorkOrderUkey =a.Ukey and orderid!='EXCESS'
                For XML path('')
                ) as OrderID ,
                (
                Select SizeCode+'/'+convert(varchar,Qty ) 
                From WorkOrder_SizeRatio c
                Where c.WorkOrderUkey =a.Ukey 
                For XML path('')
                ) as SizeRatio from WorkOrder a where mDivisionid = '{0}' and a.estcutdate = '{1}'
                and a.Ukey not in (Select WorkOrderUkey from CuttingOutput_Detail) 
                and CutRef != ''
                and CutRef not in ( {2} )
                order by cutref", keyWord, estcutdate, condition);
            DualResult dResult = DBProxy.Current.Select(null, sqlcmd, out detailTable);
            if (dResult)
            {
                gridTable = detailTable.Copy();
                grid1.DataSource = gridTable;
            }
            else
            {
                ShowErr(sqlcmd, dResult);
                return;
            }
        }

        private void Import_Click(object sender, EventArgs e)
        {
            grid1.ValidateControl();
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
                        ndr["LectraCode"] = dr["LectraCode"];
                        ndr["Cutno"] = dr["cutno"];
                        ndr["MarkerName"] = dr["MarkerName"];
                        ndr["MarkerLength"] = dr["MarkerLength"];
                        ndr["Layer"] = dr["Layer"];
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
                        exist[0]["LectraCode"] = dr["LectraCode"];
                        exist[0]["Cutno"] = dr["cutno"];
                        exist[0]["MarkerName"] = dr["MarkerName"];
                        exist[0]["MarkerLength"] = dr["MarkerLength"];
                        exist[0]["Layer"] = dr["Layer"];
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
