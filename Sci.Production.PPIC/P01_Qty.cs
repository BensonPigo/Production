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
    public partial class P01_Qty : Sci.Win.Subs.Base
    {
        DataRow masterData;
        string poCombo;
        public P01_Qty(DataRow MasterData, string POCombo)
        {
            InitializeComponent();
            masterData = MasterData;
            poCombo = POCombo;
            Text = Text + " (" + MyUtility.Convert.GetString(masterData["ID"]) + ")";
            displayBox1.Value = poCombo;
            displayBox2.Value = poCombo;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            //撈出所有的Size
            string sqlCmd = string.Format("select * from Order_SizeCode where ID = '{0}' order by Seq", MyUtility.Convert.GetString(masterData["POID"]));
            DataTable headerData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out headerData);
            StringBuilder pivot = new StringBuilder();

            //設定Grid1的顯示欄位
            this.grid1.IsEditingReadOnly = true;
            this.grid1.DataSource = listControlBindingSource1;
            var gen = Helper.Controls.Grid.Generator(this.grid1);
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
            //設定Grid2的顯示欄位
            this.grid2.IsEditingReadOnly = true;
            this.grid2.DataSource = listControlBindingSource2;
            gen = Helper.Controls.Grid.Generator(this.grid2);
            CreateGrid(gen, "int", "TotalQty", "Total", Widths.AnsiChars(6));
            CreateGrid(gen, "string", "ID", "SP#", Widths.AnsiChars(15));
            CreateGrid(gen, "string", "Article", "Colorway", Widths.AnsiChars(8));
            if (headerData != null && headerData.Rows.Count > 0)
            {
                foreach (DataRow dr in headerData.Rows)
                {
                    CreateGrid(gen, "int", MyUtility.Convert.GetString(dr["SizeCode"]), MyUtility.Convert.GetString(dr["SizeCode"]), Widths.AnsiChars(8));
                }
            }
            //設定Grid3的顯示欄位
            this.grid3.IsEditingReadOnly = true;
            this.grid3.DataSource = listControlBindingSource3;
            gen = Helper.Controls.Grid.Generator(this.grid3);
            CreateGrid(gen, "int", "TotalQty", "Total", Widths.AnsiChars(6));
            CreateGrid(gen, "string", "Article", "Colorway", Widths.AnsiChars(8));
            if (headerData != null && headerData.Rows.Count > 0)
            {
                foreach (DataRow dr in headerData.Rows)
                {
                    CreateGrid(gen, "int", MyUtility.Convert.GetString(dr["SizeCode"]), MyUtility.Convert.GetString(dr["SizeCode"]), Widths.AnsiChars(8));
                }
            }

            //凍結欄位
            grid1.Columns[1].Frozen = true;
            grid2.Columns[2].Frozen = true;
            grid3.Columns[1].Frozen = true;

            #region 撈Grid1資料
            sqlCmd = string.Format(@"with tmpData
as (
select oq.Article,oq.SizeCode,oq.Qty,oa.Seq
from Order_Qty oq
left join Order_Article oa on oa.ID = oq.ID and oa.Article = oq.Article
where oq.ID = '{0}'
),
SubTotal
as (
select 'TTL' as Article,SizeCode,SUM(Qty) as Qty, '9999' as Seq
from tmpData
group by SizeCode
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
select *,(select sum(Qty) from UnionData where Article = p.Article) as TotalQty
from pivotData p
order by Seq", MyUtility.Convert.GetString(masterData["ID"]), MyUtility.Check.Empty(pivot.ToString()) ? "[ ]" : pivot.ToString().Substring(0, pivot.ToString().Length - 1));
            DataTable grid1Data;
            result = DBProxy.Current.Select(null, sqlCmd, out grid1Data);
            #endregion  

            #region 撈Grid2資料
            sqlCmd = string.Format(@"with tmpData
as (
select o.ID,oq.Article,oq.SizeCode,oq.Qty,oa.Seq,DENSE_RANK() OVER (ORDER BY o.ID) as rnk
from Orders o
inner join Order_Qty oq on o.ID = oq.ID
left join Order_Article oa on oa.ID = oq.ID and oa.Article = oq.Article
where o.POID = '{0}'
),
SubTotal
as (
select '' as ID,'TTL' as Article,SizeCode,SUM(Qty) as Qty, '9999' as Seq,99999 as rnk
from tmpData
group by SizeCode
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
select *,(select sum(Qty) from UnionData where ID = p.ID and Article = p.Article) as TotalQty
from pivotData p
order by rnk,Seq", MyUtility.Convert.GetString(masterData["POID"]), MyUtility.Check.Empty(pivot.ToString()) ? "[ ]" : pivot.ToString().Substring(0, pivot.ToString().Length - 1));
            DataTable grid2Data;
            result = DBProxy.Current.Select(null, sqlCmd, out grid2Data);
            #endregion

            #region 撈Grid3資料
            sqlCmd = string.Format(@"with tmpData
as (
select oq.Article,oq.SizeCode,oq.Qty,oa.Seq
from Orders o
inner join Order_Qty oq on o.ID = oq.ID
left join Order_Article oa on oa.ID = oq.ID and oa.Article = oq.Article
where o.POID = '{0}'
),
SubTotal
as (
select 'TTL' as Article,SizeCode,SUM(Qty) as Qty, '9999' as Seq
from tmpData
group by SizeCode
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
select *,(select sum(Qty) from UnionData where Article = p.Article) as TotalQty
from pivotData p
order by Seq", MyUtility.Convert.GetString(masterData["POID"]), MyUtility.Check.Empty(pivot.ToString()) ? "[ ]" : pivot.ToString().Substring(0, pivot.ToString().Length - 1));
            DataTable grid3Data;
            result = DBProxy.Current.Select(null, sqlCmd, out grid3Data);
            #endregion

            listControlBindingSource1.DataSource = grid1Data;
            listControlBindingSource2.DataSource = grid2Data;
            listControlBindingSource3.DataSource = grid3Data;
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
