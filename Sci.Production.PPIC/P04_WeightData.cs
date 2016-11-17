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
    public partial class P04_WeightData : Sci.Win.Subs.Input4
    {
        string uk;
        public P04_WeightData(bool canedit, string keyvalue1, string keyvalue2, string keyvalue3)
            : base(canedit, keyvalue1, keyvalue2, keyvalue3)
        {
            uk = keyvalue1;
            InitializeComponent();
        }
        protected override DualResult OnRequery(out DataTable datas)
        {
            datas = null;
            string sql = string.Format(@"select *
                            from Style_WeightData sw where StyleUkey = '{0}'
                            order by (select seq from Style_SizeCode ss where ss.StyleUkey = sw.StyleUkey and ss.SizeCode = sw.SizeCode)"
                            , uk);
            DualResult result;
            return result = DBProxy.Current.Select(null, sql, out datas);
        }

        protected override bool OnGridSetup()
        {
            Ict.Win.DataGridViewGeneratorTextColumnSettings size = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
            Ict.Win.DataGridViewGeneratorTextColumnSettings article = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
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
                            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(string.Format("select SizeCode from Style_SizeCode where StyleUkey = {0} order by Seq", KeyValue1), "8", dr["SizeCode"].ToString());
                            DialogResult returnResult = item.ShowDialog();
                            if (returnResult == DialogResult.Cancel) { return; }
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
                        //sql參數
                        System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter("@styleukey", KeyValue1);
                        System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter("@sizecode", e.FormattedValue.ToString());

                        IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                        cmds.Add(sp1);
                        cmds.Add(sp2);
                        DataTable StyleSizeCode;
                        string sqlCmd = "select SizeCode from Style_SizeCode where StyleUkey = @styleukey and SizeCode = @sizecode";
                        DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out StyleSizeCode);
                        if (!result || StyleSizeCode.Rows.Count <= 0)
                        {
                            if (!result)
                            {
                                MyUtility.Msg.WarningBox("Sql connection fail!!\r\n"+result.ToString());
                            }
                            else
                            {
                                MyUtility.Msg.WarningBox(string.Format("< Size: {0} > not found!!!", e.FormattedValue.ToString()));
                            }
                            dr["SizeCode"] = "";
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
                            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(string.Format("select Article from Style_Article where StyleUkey = {0} order by Seq", KeyValue1), "8",dr["Article"].ToString());
                            DialogResult returnResult = item.ShowDialog();
                            if (returnResult == DialogResult.Cancel) { return; }
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
                        //sql參數
                        System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter("@styleukey", KeyValue1);
                        System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter("@article", e.FormattedValue.ToString());

                        IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                        cmds.Add(sp1);
                        cmds.Add(sp2);
                        DataTable StyleArticle;
                        string sqlCmd = "select Article from Style_Article where StyleUkey = @styleukey and Article = @article";
                        DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out StyleArticle);
                        if (!result || StyleArticle.Rows.Count <= 0)
                        {
                            if (!result)
                            {
                                MyUtility.Msg.WarningBox("Sql connection fail!!\r\n" + result.ToString());
                            }
                            else
                            {
                                MyUtility.Msg.WarningBox(string.Format("< Article: {0} > not found!!!", e.FormattedValue.ToString()));
                            }
                            dr["Article"] = "";
                            e.Cancel = true;
                            dr.EndEdit();
                            return;
                        }
                    }
                }
            };
            #endregion 

            Helper.Controls.Grid.Generator(this.grid)
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(8), settings: size)
                .Text("Article", header: "Article", width: Widths.AnsiChars(8), settings: article)
                .Numeric("NW", header: "N.W.", width: Widths.AnsiChars(5), decimal_places: 3, integer_places: 2, maximum: 99.999m, minimum: 0m)
                .Numeric("NNW", header: "N.N.W.", width: Widths.AnsiChars(5), decimal_places: 3, integer_places: 2, maximum: 99.999m, minimum: 0m)
                .Text("CreateBy", header: "Create By", width: Widths.AnsiChars(30), iseditingreadonly: true)
                .Text("EditBy", header: "Edit By", width: Widths.AnsiChars(30), iseditingreadonly: true);
            
            return true;
        }

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

        protected override bool OnSaveBefore()
        {
            this.grid.ValidateControl();
            gridbs.EndEdit();
            //刪除Size或Article為空白的資料
            foreach (DataRow gridData in Datas)
            {
                if (MyUtility.Check.Empty(gridData["SizeCode"]) || MyUtility.Check.Empty(gridData["Article"]))
                {
                    gridData.Delete();
                }
            }
            return true;
        }

        //Copy Season
        private void button1_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P04_WeightData_CopySeason callNextForm = new Sci.Production.PPIC.P04_WeightData_CopySeason(KeyValue1);
            DialogResult result = callNextForm.ShowDialog(this);
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                DataRow styleData;

                if (MyUtility.Check.Seek(string.Format("select ID,SeasonID,BrandID from Style where UKey = {0}", KeyValue1), out styleData))
                {
                    if (!MyUtility.Check.Empty(callNextForm.PPICP04CopySeason) && callNextForm.PPICP04CopySeason != styleData["SeasonID"].ToString())
                    {
                        DataTable weightData;
                        string sqlCmd = string.Format(@"select sw.* from Style s
left join Style_WeightData sw on sw.StyleUkey = s.Ukey
where s.ID = '{0}' and s.BrandID = '{1}' and s.SeasonID = '{2}'", styleData["ID"].ToString(), styleData["BrandID"].ToString(), callNextForm.PPICP04CopySeason);
                        DualResult selectResult = DBProxy.Current.Select(null, sqlCmd, out weightData);
                        if (!selectResult)
                        {
                            MyUtility.Msg.ErrorBox("Query weight data fail!\r\n"+selectResult.ToString());
                            return;
                        }

                        StringBuilder lackMsg = new StringBuilder();
                        foreach (DataRow dr in ((DataTable)gridbs.DataSource).Rows)
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
                            MyUtility.Msg.WarningBox("Copied season lack below size(s):\r\n"+lackMsg.ToString());
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
