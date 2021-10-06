using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Sci;
using Sci.Win;
using Sci.Win.Tools;
using Ict;
using Ict.Data;
using System.Collections;
using System.Transactions;
using System.Data.SqlClient;
using Sci.Utility;
using System.Linq;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P33_QTThread_Detail : Sci.Win.Subs.Base
    {
        private DataRow style;
        private DataRow style_QTThreadColorCombo_History;
        private DataTable gridTable;
        private DataTable tbArticle;
        private string combdetail_id;

        /// <inheritdoc/>
        public P33_QTThread_Detail(DataRow dr_Style_QTThreadColorCombo_History)
        {
            this.InitializeComponent();

            this.style_QTThreadColorCombo_History = dr_Style_QTThreadColorCombo_History;
            this.combdetail_id = dr_Style_QTThreadColorCombo_History["Ukey"].ToString();

            DataTable dt;
            string cmd = $@"
SELECT *
FROM Style
WHERE Ukey='{MyUtility.Convert.GetString(dr_Style_QTThreadColorCombo_History["StyleUkey"])}'
";
            DBProxy.Current.Select(null, cmd, out dt);
            this.style = dt.Rows[0];

            // 上方資料
            this.displayStyleNo.Text = MyUtility.Convert.GetString(this.style["ID"]);
            this.displaySeason.Text = MyUtility.Convert.GetString(this.style["SeasonID"]);
            this.displayFabricPanelCode.Value = dr_Style_QTThreadColorCombo_History["FabricPanelCode"].ToString();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.GenerateGrid();
        }

        private void GenerateGrid()
        {
            #region 取得Article
            string articleSql = $"Select Article from Style_Article WITH (NOLOCK) where styleukey='{this.style["Ukey"]}'";
            StringBuilder art_col = new StringBuilder();
            if (!art_col.Empty())
            {
                art_col.Clear();
            }

            DualResult dResult = DBProxy.Current.Select(null, articleSql, out this.tbArticle);
            if (!dResult)
            {
                this.ShowErr(articleSql, dResult);
            }
            else
            {
                if (this.tbArticle.Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox("Article not found");
                    return;
                }

                foreach (DataRow dr in this.tbArticle.Rows)
                {
                    art_col.Append(string.Format(@",[{0}]", dr["article"].ToString().Trim()));
                    art_col.Append(string.Format(@",[{0}_Brand]", dr["article"].ToString().Trim()));
                }
            }
            #endregion

            #region gridDetail

            // 清空Column
            this.gridDetail.Columns.Clear();

            this.Helper.Controls.Grid.Generator(this.gridDetail)
                .Text("SEQ", header: "SEQ", width: Widths.Auto(true), iseditingreadonly: true)
                .Text("Location", header: "Thread Location", width: Widths.Auto(true), iseditingreadonly: true)
                .Text("Ratio", header: "Ratio", width: Widths.Auto(true), iseditingreadonly: true)
                .Text("SCIRefno", header: "Refno", width: Widths.AnsiChars(16))
                .Text("SuppId", header: "Supplier", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("SuppName", header: "Supp. Name", width: Widths.AnsiChars(8), iseditingreadonly: true);

            this.gridDetail.Columns["SCIRefno"].DefaultCellStyle.BackColor = Color.Pink;

            for (int i = 0; i < this.tbArticle.Rows.Count; i++)
            {
                this.Helper.Controls.Grid.Generator(this.gridDetail)
                    .Text(this.tbArticle.Rows[i]["article"].ToString().Trim(), header: this.tbArticle.Rows[i]["article"].ToString().Trim(), width: Widths.AnsiChars(10));

                this.gridDetail.Columns[this.tbArticle.Rows[i]["article"].ToString().Trim()].DefaultCellStyle.BackColor = Color.Pink;
            }
            #endregion

            #region gridDetail2

            // 清空Column
            this.gridDetail2.Columns.Clear();

            this.Helper.Controls.Grid.Generator(this.gridDetail2)
                .Text("SEQ", header: "SEQ", width: Widths.Auto(true), iseditingreadonly: true)
                .Text("Location", header: "Thread Location", width: Widths.Auto(true), iseditingreadonly: true)
                .Text("Ratio", header: "Ratio", width: Widths.Auto(true), iseditingreadonly: true)
                .Text("SCIRefno", header: "Refno", width: Widths.AnsiChars(16), iseditingreadonly: true)
                .Text("SuppId", header: "Supplier", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("SuppName", header: "Supp. Name", width: Widths.AnsiChars(8), iseditingreadonly: true);

            for (int i = 0; i < this.tbArticle.Rows.Count; i++)
            {
                this.Helper.Controls.Grid.Generator(this.gridDetail2)
                    .Text(this.tbArticle.Rows[i]["article"].ToString().Trim() + "_Brand", header: this.tbArticle.Rows[i]["article"].ToString().Trim(), width: Widths.AnsiChars(10));

                this.gridDetail2.Columns[this.tbArticle.Rows[i]["article"].ToString().Trim() + "_Brand"].DefaultCellStyle.BackColor = Color.Pink;
            }
            #endregion

            #region 資料

            StringBuilder op = new StringBuilder();
            for (int i = 0; i < this.tbArticle.Rows.Count; i++)
            {
                op.Append($@"
outer apply (
	select distinct t.SuppColor as '{this.tbArticle.Rows[i][0].ToString().Trim()}', t.ColorID as '{this.tbArticle.Rows[i][0].ToString().Trim()}_Brand'
	from Style_QTThreadColorCombo_History_Detail t WITH (NOLOCK) 
	where SEQ = TD.SEQ
	and Article='{this.tbArticle.Rows[i][0].ToString().Trim()}'
    and t.Style_QTThreadColorCombo_HistoryUkey ='{this.combdetail_id}'
	and t.SCIRefNo = TD.SCIRefNo
	and t.SuppId = TD.SuppId
)TC{i}

");
            }

            string sql = string.Empty;

            sql = $@"
select
    TD.SEQ,
    tql.Location,
    TD.Ratio,
    TD.Style_QTThreadColorCombo_HistoryUkey as Style_QTThreadColorComboUkey,
    TD.SCIRefNo,
	TD.SuppId,
    TD.SuppName
    {art_col}
from (
	select t.Seq, t.SCIRefNo, t.Style_QTThreadColorCombo_HistoryUkey, t.SuppId, iif(Supp.AbbEN = '', Supp.AbbCH, Supp.AbbEN) as SuppName, t.Ratio
	from Style_QTThreadColorCombo_History_Detail t WITH (NOLOCK) 
    left join Supp on t.SuppId = Supp.Id
	where t.Style_QTThreadColorCombo_HistoryUkey = '{this.combdetail_id}'
    and t.Article in (select Article from Style_Article where StyleUkey = {this.style["Ukey"]})
    group by t.Seq, t.SCIRefNo, t.Style_QTThreadColorCombo_HistoryUkey, t.SuppId, Supp.AbbCH, Supp.AbbEN, t.Ratio
)TD
Left Join Thread_Quilting_Size_Location tql WITH (NOLOCK) on tql.Thread_Quilting_SizeUkey = '{this.style_QTThreadColorCombo_History["Thread_Quilting_SizeUkey"]}' and tql.Seq = TD.Seq	
{op}
";
            dResult = DBProxy.Current.Select(null, sql, out this.gridTable);

            if (!dResult)
            {
                this.ShowErr(sql, dResult);
            }

            this.gridTable.DefaultView.Sort = "SEQ ASC";
            this.gridDetail.DataSource = this.gridDetail2.DataSource = this.gridTable;
            #endregion
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
