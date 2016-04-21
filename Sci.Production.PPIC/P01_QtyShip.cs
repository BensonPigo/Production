using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;

namespace Sci.Production.PPIC
{
    public partial class P01_QtyShip : Sci.Win.Subs.Base
    {
        DataRow masterData;
        DataTable grid1Data, grid2Data;

        public P01_QtyShip(DataRow MasterData)
        {
            InitializeComponent();
            masterData = MasterData;
            Text = Text + " (" + MyUtility.Convert.GetString(masterData["ID"]) + ")";
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            //設定Grid1的顯示欄位
            this.grid1.IsEditingReadOnly = true;
            this.grid1.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.grid1)
                .Text("Seq", header: "Seq", width: Widths.AnsiChars(2))
                .Text("ShipmodeID", header: "Ship Mode", width: Widths.AnsiChars(10))
                .Date("BuyerDelivery", header: "Delivery", width: Widths.AnsiChars(10))
                .Numeric("Qty", header: "Total Q'ty", width: Widths.AnsiChars(6))
                .Text("AddName", header: "Create by", width: Widths.AnsiChars(10))
                .DateTime("AddDate", header: "Create at", width: Widths.AnsiChars(18))
                .Text("EditName", header: "Edit by", width: Widths.AnsiChars(10))
                .DateTime("EditDate", header: "Edit at", width: Widths.AnsiChars(18));

            string sqlCmd = string.Format(@"select Seq,ShipmodeID,BuyerDelivery,Qty,AddName,AddDate,EditName,EditDate from Order_QtyShip 
where ID = '{0}'
order by Seq",MyUtility.Convert.GetString(masterData["ID"]));
            DualResult result = DBProxy.Current.Select(null,sqlCmd,out grid1Data);


            sqlCmd = string.Format("select * from Order_SizeCode where ID = '{0}' order by Seq", MyUtility.Convert.GetString(masterData["POID"]));
            DataTable headerData;
            result = DBProxy.Current.Select(null, sqlCmd, out headerData);
            StringBuilder pivot = new StringBuilder();

            //設定Grid2的顯示欄位
            this.grid2.IsEditingReadOnly = true;
            this.grid2.DataSource = listControlBindingSource2;
            var gen = Helper.Controls.Grid.Generator(this.grid2);
            CreateGrid(gen, "int", "TotalQty", "Total", Widths.AnsiChars(6));
            CreateGrid(gen, "string", "Article", "Colorway", Widths.AnsiChars(8));
            if (headerData != null && headerData.Rows.Count > 0)
            {
                foreach (DataRow dr in headerData.Rows)
                {
                    CreateGrid(gen, "int", MyUtility.Convert.GetString(dr["SizeCode"]), MyUtility.Convert.GetString(dr["SizeCode"]), Widths.AnsiChars(8));
                    pivot.Append(string.Format("[{0}],", MyUtility.Convert.GetString(dr["SizeCode"])));
                }
            }
            //凍結欄位
            grid2.Columns[1].Frozen = true;

            //撈Grid2資料
            sqlCmd = string.Format(@"with tmpData
as (
select oqd.Seq,oqd.Article,oqd.SizeCode,oqd.Qty,oa.Seq as ASeq
from Order_QtyShip_Detail oqd
left join Order_Article oa on oa.ID = '{0}' and oa.Article = oqd.Article
where oqd.ID = '{0}'
),
SubTotal
as (
select Seq,'TTL' as Article,SizeCode,SUM(Qty) as Qty, '9999' as ASeq
from tmpData
group by Seq,Article,SizeCode
),
UnionData
as (
select * from tmpData
union all
select * from SubTotal
),
pivotData
as (
select *
from UnionData
pivot( sum(Qty)
for SizeCode in ({1})
) a
)
select *,(select sum(Qty) from UnionData where Seq = p.Seq and Article = p.Article) as TotalQty
from pivotData p
order by ASeq", MyUtility.Convert.GetString(masterData["ID"]), MyUtility.Check.Empty(pivot.ToString()) ? "[ ]" : pivot.ToString().Substring(0, pivot.ToString().Length - 1));
            result = DBProxy.Current.Select(null, sqlCmd, out grid2Data);
            

            //設定兩個Grid的關聯
            if (grid2Data != null)
            {
                grid1.SelectionChanged += (s, e) =>
                {
                    grid1.ValidateControl();
                    DataRow dr = this.grid1.GetDataRow<DataRow>(grid1.GetSelectedRowIndex());
                    if (dr != null)
                    {
                        string filter = string.Format("Seq = '{0}'", MyUtility.Convert.GetString(dr["Seq"]));
                        grid2Data.DefaultView.RowFilter = filter;
                    }
                };
            }

            listControlBindingSource1.DataSource = grid1Data;
            listControlBindingSource2.DataSource = grid2Data;
        }

        public void CreateGrid(IDataGridViewGenerator gen, string datatype, string propname, string header, IWidth width)
        {
            CreateGridCol(gen, datatype
                , propname: propname
                , header: header
                , width: width
            );
        }

        private void CreateGridCol(IDataGridViewGenerator gen, string datatype
            , string propname = null, string header = null, IWidth width = null, bool? iseditingreadonly = null
            , int index = -1)
        {
            switch (datatype)
            {
                case "int":
                    gen.Numeric(propname, header: header, width: width, iseditingreadonly: iseditingreadonly);
                    break;
                case "string":
                default:
                    gen.Text(propname, header: header, width: width, iseditingreadonly: iseditingreadonly);
                    break;
            }
        }
    }
}
