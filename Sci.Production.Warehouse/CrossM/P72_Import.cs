using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci;
using Sci.Data;

namespace Sci.Production.Warehouse
{
    public partial class P72_Import : Sci.Win.Subs.Base
    {
        public Sci.Win.Tems.Base P72;
        DataRow dr_master;
        DataTable dt_detail;
        DataSet dsTmp;
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;

        protected DataTable dtArtwork;

        public P72_Import(DataRow master, DataTable detail)
        {
            InitializeComponent();
            dr_master = master;
            dt_detail = detail;

        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            StringBuilder strSQLCmd = new StringBuilder();
            #region -- gridMaster設定 --

            this.gridMaster.IsEditingReadOnly = true; //必設定, 否則CheckBox會顯示圖示
            this.gridMaster.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.gridMaster)
                .Text("frompoid", header: "Poid", iseditingreadonly: true, width: Widths.AnsiChars(13))
                .Text("fromseq1", header: "Seq1", iseditingreadonly: true, width: Widths.AnsiChars(3))
                .Text("fromseq2", header: "Seq2", iseditingreadonly: true, width: Widths.AnsiChars(2))
                .Numeric("qty", header: "Request Qty", decimal_places: 2, integer_places: 10, iseditingreadonly: true)
                .Numeric("total_qty", header: "Total Issue Qty", decimal_places: 2, integer_places: 10, iseditingreadonly: true)
               ;

            #endregion
            #region -- gridDetail設定 --
            Ict.Win.DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            ns.CellValidating += (s, e) =>
                {
                    if (this.EditMode && !MyUtility.Check.Empty(e.FormattedValue))
                    {
                        gridDetail.GetDataRow(gridDetail.GetSelectedRowIndex())["qty"] = e.FormattedValue;
                        gridDetail.GetDataRow(gridDetail.GetSelectedRowIndex())["selected"] = true;
                    }
                };

            this.gridDetail.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.gridDetail.DataSource = listControlBindingSource2;
            Helper.Controls.Grid.Generator(this.gridDetail)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)   //0
                .Text("poid", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(13)) //1
                .Text("seq1", header: "Seq1", iseditingreadonly: true, width: Widths.AnsiChars(3)) //2
                .Text("seq2", header: "Seq2", iseditingreadonly: true, width: Widths.AnsiChars(2)) //3
                .Text("location", header: "Inventory Location", iseditingreadonly: true)      //4
                .Text("dyelot", header: "Dyelot", iseditingreadonly: true, width: Widths.AnsiChars(6)) //5
                .Text("roll", header: "Roll", iseditingreadonly: true, width: Widths.AnsiChars(6)) //6
                .Numeric("balanceqty", header: "Stock Qty", iseditable: true, decimal_places: 2, integer_places: 10) //7
                .Numeric("qty", header: "Issue Qty", decimal_places: 2, integer_places: 10, settings: ns)  //8
                ;
            this.gridDetail.Columns[8].DefaultCellStyle.BackColor = Color.Pink;
            #endregion

            #region -- 撈資料 --
            string selectCommand1 = string.Format(@";with cte as
(
	select rd.*
	from dbo.RequestCrossM r inner join dbo.RequestCrossM_Detail rd on rd.id = r.Id
	where r.Status = 'Sent' and r.id='{0}' and r.MDivisionID = '{1}'
)
select rtrim(FromPOID) frompoid,rtrim(FromSeq1) FromSeq1,rtrim(FromSeq2) FromSeq2,Qty from cte;", dr_master["cutplanid"], Sci.Env.User.Keyword);
            
            string selectCommand2 = string.Format(@";with cte as
(
	select rd.*
	from dbo.RequestCrossM r inner join dbo.RequestCrossM_Detail rd on rd.id = r.Id
	where r.Status = 'Sent' and r.id='{0}' and r.MDivisionID = '{1}'
)
select 0 as selected,'' as id,fi.Ukey FtyInventoryUkey,0.00 as qty,fi.MDivisionID,fi.POID,rtrim(fi.seq1) seq1,fi.seq2
    ,dbo.getmtldesc(fi.poid,fi.seq1,fi.seq2,2,0) as [description]
    ,(select stockunit from Po_Supp_Detail p where p.id = fi.POID and p.seq1 = fi.seq1 and p.seq2 = fi.seq2) stockunit
    ,concat(rtrim(fi.seq1), ' ', fi.seq2) seq
	,fi.Roll,fi.Dyelot,fi.StockType,fi.InQty - fi.OutQty+fi.AdjustQty balanceqty 
    ,stuff((select ',' + mtllocationid from (select mtllocationid from dbo.ftyinventory_detail where ukey = fi.ukey) t for xml path('')), 1, 1, '') [location]
from cte inner join FtyInventory fi on fi.MDivisionID  =  cte.FromMDivisionID 
and fi.POID = cte.FromPOID and fi.Seq1 = cte.FromSeq1 and fi.Seq2 = cte.FromSeq2 
where fi.StockType = 'I' and lock = 0 and fi.InQty - fi.OutQty+fi.AdjustQty > 0;", dr_master["cutplanid"], Sci.Env.User.Keyword);

            P72.ShowWaitMessage("Data Loading....");

            DataSet data = new DataSet();
            DataTable dtReqest, dtFtyDetail;
            DualResult selectResult1 = DBProxy.Current.Select(null, selectCommand1, out dtReqest);
            if (selectResult1 == false) ShowErr(selectCommand1, selectResult1);
            dtReqest.TableName = "master";
            
            DualResult selectResult2 = DBProxy.Current.Select(null, selectCommand2, out dtFtyDetail);
            if (selectResult2 == false) ShowErr(selectCommand2, selectResult2);
            dtFtyDetail.TableName = "detail";
            data.Tables.Add(dtReqest);
            data.Tables.Add(dtFtyDetail);

            DataRelation relation = new DataRelation("rel1"
                , new DataColumn[] { dtReqest.Columns["FromPoid"], dtReqest.Columns["Fromseq1"], dtReqest.Columns["Fromseq2"] }
                , new DataColumn[] { dtFtyDetail.Columns["POID"], dtFtyDetail.Columns["seq1"], dtFtyDetail.Columns["seq2"] }
                , false);

            data.Relations.Add(relation);

            dtReqest.Columns.Add("total_qty", typeof(decimal), "sum(child.qty)");

            listControlBindingSource1.DataSource = data;
            listControlBindingSource1.DataMember = "master";
            listControlBindingSource2.DataSource = listControlBindingSource1;
            listControlBindingSource2.DataMember = "rel1";

            P72.HideWaitMessage();
            #endregion
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }



        private void button2_Click(object sender, EventArgs e)
        {
            gridDetail.ValidateControl();
            DataTable dtDetail = ((DataSet)listControlBindingSource1.DataSource).Tables["detail"];
            if (MyUtility.Check.Empty(dtDetail) || dtDetail.Rows.Count == 0) return;

            DataRow[] dr2 = dtDetail.Select("Selected = 1");
            if (dr2.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select rows first!", "Warnning");
                return;
            }

            dr2 = dtDetail.Select("qty = 0 and Selected = 1");
            if (dr2.Length > 0)
            {
                MyUtility.Msg.WarningBox("Qty of selected row can't be zero!", "Warning");
                return;
            }

            dr2 = dtDetail.Select("qty <> 0 and Selected = 1");
            foreach (DataRow tmp in dr2)
            {
                DataRow[] findrow = dt_detail.Select(string.Format("poid = '{0}' and seq1 = '{1}' and seq2 = '{2}' and roll ='{3}'and dyelot='{4}'"
                    , tmp["poid"].ToString(), tmp["seq1"].ToString(), tmp["seq2"].ToString(), tmp["roll"].ToString(), tmp["dyelot"].ToString()));

                if (findrow.Length > 0)
                {
                    findrow[0]["qty"] = tmp["qty"];
                }
                else
                {
                    tmp["id"] = dr_master["id"];
                    tmp.AcceptChanges();
                    tmp.SetAdded();
                    dt_detail.ImportRow(tmp);
                }
            }


            this.Close();
        }

        private void gridMaster_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            gridDetail.ValidateControl();
        }

        
    }
}
