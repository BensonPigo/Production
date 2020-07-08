using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// P04_WeightData
    /// </summary>
    public partial class P04_WeightData : Win.Subs.Input4
    {
        private string uk;

        /// <summary>
        /// P04_WeightData
        /// </summary>
        /// <param name="canedit">bool canedit</param>
        /// <param name="keyvalue1">string keyvalue1</param>
        /// <param name="keyvalue2">string keyvalue2</param>
        /// <param name="keyvalue3">string keyvalue3</param>
        public P04_WeightData(bool canedit, string keyvalue1, string keyvalue2, string keyvalue3)
            : base(canedit, keyvalue1, keyvalue2, keyvalue3)
        {
            this.uk = keyvalue1;
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override DualResult OnRequery(out DataTable datas)
        {
            datas = null;
            string sql = string.Format(
                @"select *
                            from Style_WeightData sw WITH (NOLOCK) where StyleUkey = '{0}'
                            order by (select seq from Style_SizeCode ss WITH (NOLOCK) where ss.StyleUkey = sw.StyleUkey and ss.SizeCode = sw.SizeCode)",
                this.uk);

            DualResult result;
            return result = DBProxy.Current.Select(null, sql, out datas);
        }

        /// <inheritdoc/>
        protected override bool OnGridSetup()
        {
            DataGridViewGeneratorTextColumnSettings size = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings article = new DataGridViewGeneratorTextColumnSettings();
            #region Size的Right Click & Validating
            size.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode)
                {
                    if (e.Button == System.Windows.Forms.MouseButtons.Right)
                    {
                        if (e.RowIndex != -1)
                        {
                            DataRow dr = this.grid.GetDataRow<DataRow>(e.RowIndex);
                            Win.Tools.SelectItem item = new Win.Tools.SelectItem(string.Format("select SizeCode from Style_SizeCode WITH (NOLOCK) where StyleUkey = {0} order by Seq", this.KeyValue1), "8", dr["SizeCode"].ToString());
                            DialogResult returnResult = item.ShowDialog();
                            if (returnResult == DialogResult.Cancel)
                            {
                                return;
                            }

                            dr["SizeCode"] = item.GetSelectedString();
                        }
                    }
                }
            };

            size.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.grid.GetDataRow<DataRow>(e.RowIndex);
                    if (!MyUtility.Check.Empty(e.FormattedValue.ToString()))
                    {
                        // sql參數
                        System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter("@styleukey", this.KeyValue1);
                        System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter("@sizecode", e.FormattedValue.ToString());

                        IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                        cmds.Add(sp1);
                        cmds.Add(sp2);
                        DataTable styleSizeCode;
                        string sqlCmd = "select SizeCode from Style_SizeCode WITH (NOLOCK) where StyleUkey = @styleukey and SizeCode = @sizecode";
                        DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out styleSizeCode);
                        if (!result || styleSizeCode.Rows.Count <= 0)
                        {
                            if (!result)
                            {
                                MyUtility.Msg.WarningBox("Sql connection fail!!\r\n" + result.ToString());
                            }
                            else
                            {
                                MyUtility.Msg.WarningBox(string.Format("< Size: {0} > not found!!!", e.FormattedValue.ToString()));
                            }

                            dr["SizeCode"] = string.Empty;
                            e.Cancel = true;
                            dr.EndEdit();
                            return;
                        }
                    }
                }
            };
            #endregion

            #region Article的Right Click & Validating
            article.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode)
                {
                    if (e.Button == System.Windows.Forms.MouseButtons.Right)
                    {
                        if (e.RowIndex != -1)
                        {
                            DataRow dr = this.grid.GetDataRow<DataRow>(e.RowIndex);
                            Win.Tools.SelectItem item = new Win.Tools.SelectItem(string.Format("select Article from Style_Article WITH (NOLOCK) where StyleUkey = {0} order by Seq", this.KeyValue1), "8", dr["Article"].ToString());
                            DialogResult returnResult = item.ShowDialog();
                            if (returnResult == DialogResult.Cancel)
                            {
                                return;
                            }

                            dr["Article"] = item.GetSelectedString();
                        }
                    }
                }
            };

            article.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.grid.GetDataRow<DataRow>(e.RowIndex);
                    if (!string.IsNullOrWhiteSpace(e.FormattedValue.ToString()) && e.FormattedValue.ToString() != "----")
                    {
                        // sql參數
                        System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter("@styleukey", this.KeyValue1);
                        System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter("@article", e.FormattedValue.ToString());

                        IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                        cmds.Add(sp1);
                        cmds.Add(sp2);
                        DataTable styleArticle;
                        string sqlCmd = "select Article from Style_Article WITH (NOLOCK) where StyleUkey = @styleukey and Article = @article";
                        DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out styleArticle);
                        if (!result || styleArticle.Rows.Count <= 0)
                        {
                            if (!result)
                            {
                                MyUtility.Msg.WarningBox("Sql connection fail!!\r\n" + result.ToString());
                            }
                            else
                            {
                                MyUtility.Msg.WarningBox(string.Format("< Article: {0} > not found!!!", e.FormattedValue.ToString()));
                            }

                            dr["Article"] = string.Empty;
                            e.Cancel = true;
                            dr.EndEdit();
                            return;
                        }
                    }
                }
            };
            #endregion

            this.Helper.Controls.Grid.Generator(this.grid)
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(8), settings: size)
                .Text("Article", header: "Article", width: Widths.AnsiChars(8), settings: article)
                .Numeric("NW", header: "N.W.", width: Widths.AnsiChars(5), decimal_places: 3, integer_places: 2, maximum: 99.999m, minimum: 0m)
                .Numeric("NNW", header: "N.N.W.", width: Widths.AnsiChars(5), decimal_places: 3, integer_places: 2, maximum: 99.999m, minimum: 0m)
                .Text("CreateBy", header: "Create By", width: Widths.AnsiChars(35), iseditingreadonly: true)
                .Text("EditBy", header: "Edit By", width: Widths.AnsiChars(35), iseditingreadonly: true);

            return true;
        }

        /// <inheritdoc/>
        protected override void OnRequeryPost(DataTable datas)
        {
            base.OnRequeryPost(datas);
            datas.Columns.Add("CreateBy");
            datas.Columns.Add("EditBy");
            foreach (DataRow gridData in datas.Rows)
            {
                gridData["CreateBy"] = gridData["AddName"].ToString() + " - " + MyUtility.GetValue.Lookup("Name", gridData["AddName"].ToString(), "Pass1", "ID") + "   " + ((DateTime)gridData["AddDate"]).ToString(string.Format("{0}", Sci.Env.Cfg.DateTimeStringFormat));
                if (gridData["EditDate"] != System.DBNull.Value)
                {
                    gridData["EditBy"] = gridData["EditName"].ToString() + " - " + MyUtility.GetValue.Lookup("Name", gridData["EditName"].ToString(), "Pass1", "ID") + "   " + ((DateTime)gridData["EditDate"]).ToString(string.Format("{0}", Sci.Env.Cfg.DateTimeStringFormat));
                }

                gridData.AcceptChanges();
            }
        }

        /// <inheritdoc/>
        protected override bool OnSaveBefore()
        {
            this.grid.ValidateControl();
            this.gridbs.EndEdit();

            // 刪除Size或Article為空白的資料
            foreach (DataRow gridData in this.Datas)
            {
                if (MyUtility.Check.Empty(gridData["SizeCode"]) || MyUtility.Check.Empty(gridData["Article"]))
                {
                    gridData.Delete();
                }
            }

            return true;
        }

        // Copy Season
        private void BtnCopySeason_Click(object sender, EventArgs e)
        {
            P04_WeightData_CopySeason callNextForm = new P04_WeightData_CopySeason(this.KeyValue1);
            DialogResult result = callNextForm.ShowDialog(this);
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                DataRow styleData;

                if (MyUtility.Check.Seek(string.Format("select ID,SeasonID,BrandID from Style WITH (NOLOCK) where UKey = {0}", this.KeyValue1), out styleData))
                {
                    if (!MyUtility.Check.Empty(callNextForm.PPICP04CopySeason) && callNextForm.PPICP04CopySeason != styleData["SeasonID"].ToString())
                    {
                        DataTable weightData;
                        string sqlCmd = string.Format(
                            @"select sw.* from Style s WITH (NOLOCK) 
left join Style_WeightData sw WITH (NOLOCK) on sw.StyleUkey = s.Ukey
where s.ID = '{0}' and s.BrandID = '{1}' and s.SeasonID = '{2}'",
                            styleData["ID"].ToString(),
                            styleData["BrandID"].ToString(),
                            callNextForm.PPICP04CopySeason);

                        DualResult selectResult = DBProxy.Current.Select(null, sqlCmd, out weightData);
                        if (!selectResult)
                        {
                            MyUtility.Msg.ErrorBox("Query weight data fail!\r\n" + selectResult.ToString());
                            return;
                        }

                        StringBuilder lackMsg = new StringBuilder();
                        foreach (DataRow dr in ((DataTable)this.gridbs.DataSource).Rows)
                        {
                            DataRow[] findrow = weightData.Select(string.Format("SizeCode = '{0}'", dr["SizeCode"].ToString()));
                            if (findrow.Length == 0)
                            {
                                lackMsg.Append(string.Format("Size: {0}\r\n", dr["SizeCode"].ToString()));
                            }
                            else
                            {
                                dr["NW"] = MyUtility.Convert.GetDecimal(findrow[0]["NW"]);
                                dr["NNW"] = MyUtility.Convert.GetDecimal(findrow[0]["NNW"]);
                            }
                        }

                        if (lackMsg.Length > 0)
                        {
                            MyUtility.Msg.WarningBox("Copied season lack below size(s):\r\n" + lackMsg.ToString());
                        }
                    }
                }
                else
                {
                    MyUtility.Msg.WarningBox("Data not found!!");
                }
            }
        }
    }
}
