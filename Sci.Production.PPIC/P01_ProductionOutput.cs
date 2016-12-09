using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.PPIC
{
    public partial class P01_ProductionOutput : Sci.Win.Subs.Base
    {
        DataRow masterData;
        string cuttingWorkType;
        Ict.Win.DataGridViewGeneratorNumericColumnSettings sewingqty = new Ict.Win.DataGridViewGeneratorNumericColumnSettings();
        Ict.Win.DataGridViewGeneratorNumericColumnSettings t = new Ict.Win.DataGridViewGeneratorNumericColumnSettings();
        Ict.Win.DataGridViewGeneratorNumericColumnSettings b = new Ict.Win.DataGridViewGeneratorNumericColumnSettings();
        Ict.Win.DataGridViewGeneratorNumericColumnSettings i = new Ict.Win.DataGridViewGeneratorNumericColumnSettings();
        Ict.Win.DataGridViewGeneratorNumericColumnSettings o = new Ict.Win.DataGridViewGeneratorNumericColumnSettings();
        Ict.Win.DataGridViewGeneratorNumericColumnSettings cuttingqty = new Ict.Win.DataGridViewGeneratorNumericColumnSettings();
        public P01_ProductionOutput(DataRow MasterData)
        {
            InitializeComponent();
            masterData = MasterData;
            Text = "Production output - " + MyUtility.Convert.GetString(masterData["ID"]);
            cuttingWorkType = MyUtility.GetValue.Lookup(string.Format("select WorkType from Cutting where ID = '{0}'", MyUtility.Convert.GetString(masterData["CuttingSP"])));
            //if (cuttingWorkType == "1")
            //{
            //    tabPage2.Text = "Cutting(Comb) - " + MyUtility.Convert.GetString(masterData["CuttingSP"]);
            //}
            //else
            //{
            //    tabPage2.Text = "Cutting - " + MyUtility.Convert.GetString(masterData["ID"]);
            //}
            tabPage2.Text = "Cutting output";
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            //撈Summary資料
            string sqlCmd = string.Format(@"select (select Max(s.OutputDate)
		from SewingOutput_Detail sd
		inner join SewingOutput s on sd.ID = s.ID
		where sd.OrderId = '{0}') as LastSewingDate,
isnull((dbo.getMinCompleteSewQty('{0}',null,null)),0) as SewingQty,
isnull((select SUM(c.Qty)
	   from Orders o
	   inner join CuttingOutput_WIP c on o.ID = c.OrderID
	   where {1}),0) as CutQty", MyUtility.Convert.GetString(masterData["ID"]),
                                        cuttingWorkType == "1" ? string.Format("o.CuttingSP = '{0}'", MyUtility.Convert.GetString(masterData["CuttingSP"])) : string.Format("o.ID = '{0}'", MyUtility.Convert.GetString(masterData["ID"])));
            DataTable summaryQty;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out summaryQty);
            dateBox1.Value = MyUtility.Convert.GetDate(summaryQty.Rows[0]["LastSewingDate"]);
            numericBox1.Value = MyUtility.Convert.GetInt(masterData["Qty"]);
            //numericBox2.Value = MyUtility.Convert.GetInt(summaryQty.Rows[0]["SewingQty"]);
            //numericBox3.Value = MyUtility.Convert.GetInt(summaryQty.Rows[0]["CutQty"]);
            numericBox4.Value = MyUtility.Convert.GetInt(masterData["Qty"]);

            sewingqty.CellMouseDoubleClick += (s, e) =>
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    DataRow dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                    Sci.Production.PPIC.P01_ProductionOutput_SewingDetail callNextForm = new Sci.Production.PPIC.P01_ProductionOutput_SewingDetail(MyUtility.Convert.GetString(masterData["ID"]), "S", MyUtility.Convert.GetString(dr["Article"]), MyUtility.Convert.GetString(dr["SizeCode"]));
                    callNextForm.ShowDialog(this);
                }
            };

            t.CellMouseDoubleClick += (s, e) =>
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    DataRow dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                    Sci.Production.PPIC.P01_ProductionOutput_SewingDetail callNextForm = new Sci.Production.PPIC.P01_ProductionOutput_SewingDetail(MyUtility.Convert.GetString(masterData["ID"]), "T", MyUtility.Convert.GetString(dr["Article"]), MyUtility.Convert.GetString(dr["SizeCode"]));
                    callNextForm.ShowDialog(this);
                }
            };

            b.CellMouseDoubleClick += (s, e) =>
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    DataRow dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                    Sci.Production.PPIC.P01_ProductionOutput_SewingDetail callNextForm = new Sci.Production.PPIC.P01_ProductionOutput_SewingDetail(MyUtility.Convert.GetString(masterData["ID"]), "B", MyUtility.Convert.GetString(dr["Article"]), MyUtility.Convert.GetString(dr["SizeCode"]));
                    callNextForm.ShowDialog(this);
                }
            };

            i.CellMouseDoubleClick += (s, e) =>
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    DataRow dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                    Sci.Production.PPIC.P01_ProductionOutput_SewingDetail callNextForm = new Sci.Production.PPIC.P01_ProductionOutput_SewingDetail(MyUtility.Convert.GetString(masterData["ID"]), "I", MyUtility.Convert.GetString(dr["Article"]), MyUtility.Convert.GetString(dr["SizeCode"]));
                    callNextForm.ShowDialog(this);
                }
            };

            o.CellMouseDoubleClick += (s, e) =>
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    DataRow dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                    Sci.Production.PPIC.P01_ProductionOutput_SewingDetail callNextForm = new Sci.Production.PPIC.P01_ProductionOutput_SewingDetail(MyUtility.Convert.GetString(masterData["ID"]), "O", MyUtility.Convert.GetString(dr["Article"]), MyUtility.Convert.GetString(dr["SizeCode"]));
                    callNextForm.ShowDialog(this);
                }
            };

            //設定Grid1的顯示欄位
            this.grid1.IsEditingReadOnly = true;
            this.grid1.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.grid1)
                 .Text("Article", header: "Colorway", width: Widths.AnsiChars(8))
                 .Text("SizeCode", header: "Size", width: Widths.AnsiChars(8))
                 .Numeric("Qty", header: "Order Q'ty", width: Widths.AnsiChars(6))
                 .Numeric("SewQty", header: "Sewing Q'ty", width: Widths.AnsiChars(6), settings: sewingqty)
                 .Numeric("T", header: "Top", width: Widths.AnsiChars(6), settings: t)
                 .Numeric("B", header: "Bottom", width: Widths.AnsiChars(6), settings: b)
                 .Numeric("I", header: "Inner", width: Widths.AnsiChars(6), settings: i)
                 .Numeric("O", header: "Outer", width: Widths.AnsiChars(6), settings: o);

            #region 控制Column是否可被看見
            grid1.Columns[4].Visible = false;
            grid1.Columns[5].Visible = false;
            grid1.Columns[6].Visible = false;
            grid1.Columns[7].Visible = false;
            if (MyUtility.Convert.GetString(masterData["StyleUnit"]) == "SETS")
            {
                sqlCmd = string.Format("select Location from Style_Location where StyleUkey = {0}", MyUtility.Convert.GetString(masterData["StyleUKey"]));
                DataTable styleLocation;
                result = DBProxy.Current.Select(null, sqlCmd, out styleLocation);
                if (styleLocation != null)
                {
                    foreach (DataRow dr in styleLocation.Rows)
                    {
                        if (MyUtility.Convert.GetString(dr["Location"]) == "T")
                        {
                            grid1.Columns[4].Visible = true;
                        }
                        if (MyUtility.Convert.GetString(dr["Location"]) == "B")
                        {
                            grid1.Columns[5].Visible = true;
                        }
                        if (MyUtility.Convert.GetString(dr["Location"]) == "I")
                        {
                            grid1.Columns[6].Visible = true;
                        }
                        if (MyUtility.Convert.GetString(dr["Location"]) == "O")
                        {
                            grid1.Columns[7].Visible = true;
                        }
                    }
                }
            }
            #endregion

            for (int j = 0; j < this.grid1.ColumnCount; j++)
            {
                this.grid1.Columns[j].SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            //當Article變動時，幣一筆不一樣的資料背景顏色要改變
            grid1.RowsAdded += (s, e) =>
            {
                DataTable dtData = (DataTable)listControlBindingSource1.DataSource;
                for (int j = 0; j < e.RowCount; j++)
                {
                    if (!MyUtility.Check.Empty(dtData.Rows[j]["LastArticle"]) && dtData.Rows[j]["LastArticle"].ToString() != dtData.Rows[j]["Article"].ToString())
                    {
                        grid1.Rows[j].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 128);
                    }
                    else
                    {
                        grid1.Rows[j].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255);
                    }
                }
            };


            cuttingqty.CellMouseDoubleClick += (s, e) =>
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    DataRow dr = this.grid2.GetDataRow<DataRow>(e.RowIndex);
                    Sci.Production.PPIC.P01_ProductionOutput_CuttingDetail callNextForm = new Sci.Production.PPIC.P01_ProductionOutput_CuttingDetail(cuttingWorkType, cuttingWorkType == "1" ? MyUtility.Convert.GetString(masterData["CuttingSP"]) : MyUtility.Convert.GetString(masterData["ID"]), "C", MyUtility.Convert.GetString(dr["Article"]), MyUtility.Convert.GetString(dr["SizeCode"]));
                    callNextForm.ShowDialog(this);
                }
            };

            //設定Grid2的顯示欄位
            this.grid2.IsEditingReadOnly = true;
            this.grid2.DataSource = listControlBindingSource2;
            Helper.Controls.Grid.Generator(this.grid2)
                 .Text("Article", header: "Colorway", width: Widths.AnsiChars(8))
                 .Text("SizeCode", header: "Size", width: Widths.AnsiChars(8))
                 .Numeric("Qty", header: "Order Q'ty", width: Widths.AnsiChars(6))
                 .Numeric("CutQty", header: "Cutting Q'ty", width: Widths.AnsiChars(6), settings: cuttingqty);

            for (int j = 0; j < this.grid2.ColumnCount; j++)
            {
                this.grid2.Columns[j].SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            //撈Sewing Data
            sqlCmd = string.Format(@"with SewQty
as (
select oq.Article,oq.SizeCode,oq.Qty,sdd.ComboType,isnull(sum(sdd.QAQty),0) as QAQty
from Orders o
inner join Order_Qty oq on oq.ID = o.ID
left join SewingOutput_Detail_Detail sdd on sdd.OrderId = o.ID and sdd.Article = oq.Article and sdd.SizeCode = oq.SizeCode
where o.ID = '{0}'
group by oq.Article,oq.SizeCode,oq.Qty,sdd.ComboType
),
minSewQty
as (
select Article,SizeCode,MIN(QAQty) as QAQty
from SewQty
group by Article,SizeCode
),
PivotData
as (
select *
from SewQty
PIVOT (SUM(QAQty)
FOR ComboType IN ([T],[B],[I],[O])) a
)
select p.*,m.QAQty as SewQty,LAG(p.Article,1,null) OVER (Order by oa.Seq,os.Seq) as LastArticle
from PivotData p
left join minSewQty m on m.Article = p.Article and m.SizeCode = p.SizeCode
left join Order_Article oa on oa.ID = '{1}' and oa.Article = p.Article
left join Order_SizeCode os on os.ID = '{1}' and os.SizeCode = p.SizeCode
order by oa.Seq,os.Seq", MyUtility.Convert.GetString(masterData["ID"]), MyUtility.Convert.GetString(masterData["POID"]));
            DataTable SewingData;
            result = DBProxy.Current.Select(null, sqlCmd, out SewingData);

            sqlCmd = string.Format(@"select oq.Article,oq.SizeCode,oq.Qty,sum(c.Qty) as CutQty
from Orders o
inner join Order_Qty oq on oq.id = o.ID
left join CuttingOutput_WIP c on c.OrderID = o.ID and c.Article = oq.Article and c.Size = oq.SizeCode
left join Order_Article oa on oa.ID = o.POID and oa.Article = oq.Article
left join Order_SizeCode os on os.ID = o.POID and os.SizeCode = oq.SizeCode
where {0}
group by oq.Article,oq.SizeCode,oq.Qty,oa.Seq,os.Seq
order by oa.Seq,os.Seq", string.Format("o.ID = '{0}'", MyUtility.Convert.GetString(masterData["ID"])));

            DataTable CuttingData;
            result = DBProxy.Current.Select(null, sqlCmd, out CuttingData);

            //bug fix:0000294: PPIC_P01_ProductionOutput
            numericBox2.Value = MyUtility.Convert.GetInt(SewingData.Compute("sum(SewQty)", ""));  //Sewing Q'ty
            numericBox3.Value = MyUtility.Convert.GetDecimal(CuttingData.Compute("sum(CutQty)", ""));  //Cutting Q'ty

            listControlBindingSource1.DataSource = SewingData;
            listControlBindingSource2.DataSource = CuttingData;
        }

        //Sewing Q'ty
        private void numericBox2_DoubleClick(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_ProductionOutput_SewingDetail callNextForm = new Sci.Production.PPIC.P01_ProductionOutput_SewingDetail(MyUtility.Convert.GetString(masterData["ID"]),"A","","");
            callNextForm.ShowDialog(this);
        }

        //Cutting Q'ty
        private void numericBox3_DoubleClick(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_ProductionOutput_CuttingDetail callNextForm = new Sci.Production.PPIC.P01_ProductionOutput_CuttingDetail(cuttingWorkType, cuttingWorkType == "1" ? MyUtility.Convert.GetString(masterData["CuttingSP"]) : MyUtility.Convert.GetString(masterData["ID"]), "A", "", "");
            callNextForm.ShowDialog(this);
        }
    }
}
