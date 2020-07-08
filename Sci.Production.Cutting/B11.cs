using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Sci.Production.Cutting
{
    /// <summary>
    /// B11
    /// </summary>
    public partial class B11 : Win.Tems.Input1
    {
        /// <summary>
        /// B11
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public B11(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.txtstyle1.tarBrand = this.txtbrand1;
            this.txtstyle1.tarSeason = this.txtseason1;
        }

        private DataTable Dt;

        protected override bool ClickSaveBefore()
        {
            // 檢查是否存在style
            string sqlcmd = $@"
select ukey
from style s with(nolock)
where s.ID = '{this.txtstyle1.Text}'
and s.seasonid = '{this.txtseason1.Text}'
and s.brandid = '{this.txtbrand1.Text}'
";
            string styleukey = string.Empty;
            try
            {
                styleukey = MyUtility.GetValue.Lookup(sqlcmd);
                if (MyUtility.Check.Empty(styleukey))
                {
                    MyUtility.Msg.WarningBox($"< Style#:{this.txtstyle1.Text}, Season:{this.txtseason1.Text}, Brand:{this.txtbrand1.Text}>  not found!");
                    return false;
                }
            }
            catch (Exception ex)
            {
                this.ShowErr(ex);
                return false;
            }

            // 檢查此style是否已存在SubProcessSeq
            if (this.IsDetailInserting)
            {
                sqlcmd = $@"select 1 from SubProcessSeq with(nolock) where styleukey = {styleukey}";
                try
                {
                    if (MyUtility.Check.Seek(sqlcmd))
                    {
                        MyUtility.Msg.WarningBox($"< Style#:{this.txtstyle1.Text}, Season:{this.txtseason1.Text}, Brand:{this.txtbrand1.Text}> already exists!!");
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    this.ShowErr(ex);
                    return false;
                }

                this.CurrentMaintain["styleukey"] = styleukey;
            }

            foreach (DataRow item in this.Dt.Rows)
            {
                item["styleukey"] = styleukey;
            }

            return base.ClickSaveBefore();
        }

        protected override DualResult ClickSave()
        {
            // ProcessWithDatatable無法解析tinyint, 先換成int
            DataTable dtCloned = this.Dt.Clone();
            dtCloned.Columns[0].DataType = typeof(int);
            foreach (DataRow row in this.Dt.Rows)
            {
                dtCloned.ImportRow(row);
            }

            string sqlcmd = $@"
update s set s.seq  = t.seq
from SubProcessSeq_Detail s
inner join #tmp t on t.StyleUkey = s.StyleUkey and t.SubProcessID = s.SubProcessID
where s.seq <> t.seq

INSERT INTO [dbo].[SubProcessSeq_Detail]([StyleUkey],[SubProcessID],[Seq])
select {this.CurrentMaintain["styleukey"]},t.SubProcessID,t.Seq
from #tmp t
left join SubProcessSeq_Detail s on t.StyleUkey = s.StyleUkey and t.SubProcessID = s.SubProcessID
where s.seq is null and t.seq > 0
";
            DataTable dt;
            DualResult result = MyUtility.Tool.ProcessWithDatatable(dtCloned, string.Empty, sqlcmd, out dt);
            if (!result)
            {
                return result;
            }

            return base.ClickSave();
        }

        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            string sqlcmd = DefaultQuerySql();
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out this.Dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.listControlBindingSource1.DataSource = this.Dt;
        }

        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtstyle1.ReadOnly = true;
            this.txtseason1.ReadOnly = true;
            this.txtbrand1.ReadOnly = true;
        }

        protected override bool ClickCopyBefore()
        {
            return base.ClickCopyBefore();
        }

        protected override void ClickCopyAfter()
        {
            base.ClickCopyAfter();
            this.CurrentMaintain["StyleID"] = string.Empty;
            this.CurrentMaintain["SeasonID"] = string.Empty;
            this.CurrentMaintain["BrandID"] = string.Empty;
        }

        protected override DualResult ClickDeletePost()
        {
            string sqlcmd = $@"delete SubProcessSeq_Detail where styleukey = {this.CurrentMaintain["styleukey"]}";
            DualResult result = DBProxy.Current.Execute(null, sqlcmd);
            if (!result)
            {
                return result;
            }

            return base.ClickDeletePost();
        }

        protected override void OnEditModeChanged()
        {
            base.OnEditModeChanged();
            if (this.grid1 != null)
            {
                if (this.EditMode)
                {
                    this.grid1.Columns["Seq"].DefaultCellStyle.BackColor = Color.Pink;
                    this.grid1.IsEditingReadOnly = false;
                }
                else
                {
                    this.grid1.Columns["Seq"].DefaultCellStyle.BackColor = Color.White;
                    this.grid1.IsEditingReadOnly = true;
                    this.Query();
                }
            }
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.Query();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.GridSetup();
            string sqlcmd = DefaultQuerySql();
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out this.Dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.listControlBindingSource1.DataSource = this.Dt;
        }

        private void GridSetup()
        {
            this.Helper.Controls.Grid.Generator(this.grid1)
            .Numeric("Seq", header: "Seq", width: Widths.AnsiChars(4), maximum: 255)
            .Text("SubProcessID", header: "SubProcess", width: Widths.AnsiChars(24), iseditingreadonly: true)
            .Text("ArtworkTypeId", header: "Artwork Type", width: Widths.AnsiChars(35), iseditingreadonly: true)
            ;

            for (int i = 0; i < this.grid1.ColumnCount; i++)
            {
                this.grid1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private void Query()
        {
            this.listControlBindingSource1.DataSource = null;
            if (this.CurrentMaintain == null)
            {
                return;
            }

            string sqlcmd = $@"
SELECT seq=isnull(ss.seq, 0),
       SubProcessID=s.id, 
       s.artworktypeid,
    ss.StyleUkey
FROM subprocess s WITH (nolock) 
left join SubProcessSeq_Detail ss WITH (nolock) on ss.subprocessid = s.id and ss.Styleukey = {this.CurrentMaintain["Styleukey"]}
WHERE  s.junk = 0 
AND s.isselection = 1
ORDER  BY ss.seq, s.seq
";
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out this.Dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.listControlBindingSource1.DataSource = this.Dt;
        }

        private void Tabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.tabs.SelectedIndex == 0)
            {
                this.btnBatchCreate.Enabled = true;
            }
            else
            {
                this.btnBatchCreate.Enabled = false;
            }
        }

        private void BtnCopy_Click(object sender, EventArgs e)
        {
            string sqlcmd = B10_QuerySql();
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out this.Dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.listControlBindingSource1.DataSource = this.Dt;
        }

        private void BtnBatchCreate_Click(object sender, EventArgs e)
        {
            var form = new B11_Batch_Create();
            form.ShowDialog();
            this.ReloadDatas();
        }

        private void PictureBoxup_Click(object sender, EventArgs e)
        {
            if (this.EditMode)
            {
                this.UpDataGridView(this.grid1);
            }
        }

        private void PictureBoxdown_Click(object sender, EventArgs e)
        {
            if (this.EditMode)
            {
                this.DownDataGridView(this.grid1);
            }
        }

        /// <summary>
        /// 上移
        /// </summary>
        /// <param name="dataGridView"></param>
        public void UpDataGridView(Win.UI.Grid dataGridView)
        {
            try
            {
                this.grid1.ValidateControl();
                DataGridViewSelectedRowCollection dgvsrc = dataGridView.SelectedRows;
                if (dgvsrc.Count > 0)
                {
                    int index = dataGridView.SelectedRows[0].Index;
                    if (index > 0)
                    {
                        // 交換Seq值
                        decimal seqOri = MyUtility.Convert.GetDecimal(this.Dt.Rows[index]["Seq"]);
                        decimal seqTo = MyUtility.Convert.GetDecimal(this.Dt.Rows[index - 1]["Seq"]);
                        this.Dt.Rows[index]["Seq"] = seqTo;
                        this.Dt.Rows[index - 1]["Seq"] = seqOri;
                        this.Dt.AcceptChanges();

                        // 交換整筆row
                        DataRow newdata = this.Dt.NewRow();
                        newdata.ItemArray = this.Dt.Rows[index].ItemArray;
                        this.Dt.Rows.RemoveAt(index);
                        this.Dt.Rows.InsertAt(newdata, index - 1);
                        this.Dt.AcceptChanges();
                        dataGridView.Rows[index - 1].Selected = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 下移
        /// </summary>
        /// <param name="dataGridView"></param>
        public void DownDataGridView(Win.UI.Grid dataGridView)
        {
            try
            {
                this.grid1.ValidateControl();
                DataGridViewSelectedRowCollection dgvsrc = dataGridView.SelectedRows;
                if (dgvsrc.Count > 0)
                {
                    int index = dataGridView.SelectedRows[0].Index;
                    if (index >= 0 & (dataGridView.RowCount - 1) != index)
                    {
                        // 交換Seq值
                        decimal seqOri = MyUtility.Convert.GetDecimal(this.Dt.Rows[index]["Seq"]);
                        decimal seqTo = MyUtility.Convert.GetDecimal(this.Dt.Rows[index + 1]["Seq"]);
                        this.Dt.Rows[index]["Seq"] = seqTo;
                        this.Dt.Rows[index + 1]["Seq"] = seqOri;
                        this.Dt.AcceptChanges();

                        // 交換整筆row
                        DataRow newdata = this.Dt.NewRow();
                        newdata.ItemArray = this.Dt.Rows[index].ItemArray;
                        this.Dt.Rows.RemoveAt(index);
                        this.Dt.Rows.InsertAt(newdata, index + 1);
                        this.Dt.AcceptChanges();
                        dataGridView.Rows[index + 1].Selected = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public static string DefaultQuerySql()
        {
            return $@"
SELECT seq = 0, 
       subprocessid = s.id, 
       s.artworktypeid,
    StyleUkey=0
FROM   subprocess s WITH (nolock) 
WHERE  s.junk = 0 
       AND s.isselection = 1
ORDER  BY s.seq ASC
";
        }

        public static string B10_QuerySql()
        {
            return $@"
SELECT s.seq,
       subprocessid = s.id, 
       s.artworktypeid,
    StyleUkey=0
FROM   subprocess s WITH (nolock) 
WHERE  s.junk = 0 
       AND s.isselection = 1
ORDER  BY s.seq ASC
";
        }
    }
}
