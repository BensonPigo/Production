using Ict;
using Ict.Win;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Sci.Data;
using System.Data.SqlClient;

namespace Sci.Production.Quality
{
    public partial class P06 : Sci.Win.Tems.Input6
    {
        // 宣告Context Menu Item
        ToolStripMenuItem add;

        // 宣告Context Menu Item
        ToolStripMenuItem edit;

        // 宣告Context Menu Item
        ToolStripMenuItem delete;
        private new bool IsSupportEdit = true;

        public P06(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.detailgrid.ContextMenuStrip = this.detailgridmenus;
        }

        // refresh
        protected override void OnDetailEntered()
        {
            List<SqlParameter> spam = new List<SqlParameter>();
            DataRow dr, drEarly, drSci;
            this.detailgrid.AutoResizeColumns();
            string sql_cmd =
                @"select a.ID,b.StyleID,b.SeasonID,b.BrandID,b.CutInLine,c.Article,c.Result,a.ColorFastnessLaboratoryRemark,b.factoryid 
                from po a WITH (NOLOCK) 
                left join Orders b WITH (NOLOCK) on a.ID = b.POID
                left join ColorFastness c WITH (NOLOCK) on a.ID=c.POID
                where a.id=@id";
            spam.Add(new SqlParameter("@id", this.CurrentMaintain["ID"].ToString()));

            if (MyUtility.Check.Seek(sql_cmd, spam, out dr))
            {
                this.displaySP.Text = dr["id"].ToString();
                this.displayStyle.Text = dr["StyleID"].ToString();
                this.displaySeason.Text = dr["SeasonID"].ToString();
                this.displayBrand.Text = dr["BrandID"].ToString();
                this.editRemark.Text = dr["ColorFastnessLaboratoryRemark"].ToString();
            }

            if (MyUtility.Check.Seek(string.Format("select min(a.CutInLine) as CutInLine from Orders a WITH (NOLOCK) left join PO b WITH (NOLOCK) on a.POID=b.ID WHERE a.Poid='{0}'", this.CurrentMaintain["id"]), out drEarly))
            {
                if (drEarly["CutInLine"] == DBNull.Value)
                {
                    this.dateEarliestEstCuttingDate.Text = string.Empty;
                }
                else
                {
                    this.dateEarliestEstCuttingDate.Value = Convert.ToDateTime(drEarly["CutInLine"]);
                }
            }

            MyUtility.Check.Seek(string.Format("select * from dbo.GetSCI('{0}','')", this.CurrentMaintain["id"].ToString()), out drSci);

            DateTime? targT = null;
            if (!MyUtility.Check.Empty(drEarly["CUTINLINE"]) && !MyUtility.Check.Empty(drSci["MinSciDelivery"]))
            {
                targT = Sci.Production.PublicPrg.Prgs.GetTargetLeadTime(drEarly["CUTINLINE"], drSci["MinSciDelivery"]);
            }

            if (targT != null)
            {
                this.dateTargetLeadtime.Value = targT;
            }
            else
            {
                this.dateTargetLeadtime.Text = string.Empty;
            }

            decimal dRowCount = this.DetailDatas.Count;
            string inspnum = "0";
            DataTable articleDT = (DataTable)this.detailgridbs.DataSource;

            DateTime CompDate;
            if (inspnum == "100")
            {
                CompDate = (DateTime)articleDT.Compute("Max(Inspdate)", string.Empty);
                this.dateCompletionDate.Value = CompDate;
            }
            else
            {
                this.dateCompletionDate.Text = string.Empty;
            }

            // 判斷Grid有無資料 , 沒資料就傳true並關閉 ContextMenu edit & delete
            this.contextMenuStrip();

            base.OnDetailEntered();
        }

        protected override void OnDetailGridSetup()
        {
            DataGridViewGeneratorNumericColumnSettings testNoCell = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorDateColumnSettings inspDate = new DataGridViewGeneratorDateColumnSettings();
            DataGridViewGeneratorTextColumnSettings articleCell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings resultCell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings inspectorCell = new DataGridViewGeneratorTextColumnSettings();

            #region MouseClick
            testNoCell.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.CurrentDetailData;
                if (dr == null)
                {
                    return;
                }

                var frm = new Sci.Production.Quality.P06_Detail(false, this.CurrentDetailData["ID"].ToString(), null, null, dr, this.displaySP.Text);
                frm.ShowDialog(this);
                frm.Dispose();
                this.RenewData();
            };

            inspDate.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.CurrentDetailData;
                if (dr == null)
                {
                    return;
                }

                var frm = new Sci.Production.Quality.P06_Detail(false, this.CurrentDetailData["ID"].ToString(), null, null, dr, this.displaySP.Text);
                frm.ShowDialog(this);
                frm.Dispose();
                this.RenewData();
            };
            articleCell.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.CurrentDetailData;
                if (dr == null)
                {
                    return;
                }

                var frm = new Sci.Production.Quality.P06_Detail(false, this.CurrentDetailData["ID"].ToString(), null, null, dr, this.displaySP.Text);
                frm.ShowDialog(this);
                frm.Dispose();
                this.RenewData();
            };
            resultCell.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.CurrentDetailData;
                if (dr == null)
                {
                    return;
                }

                var frm = new Sci.Production.Quality.P06_Detail(false, this.CurrentDetailData["ID"].ToString(), null, null, dr, this.displaySP.Text);
                frm.ShowDialog(this);
                frm.Dispose();
                this.RenewData();
            };
            inspectorCell.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.CurrentDetailData;
                if (dr == null)
                {
                    return;
                }

                var frm = new Sci.Production.Quality.P06_Detail(false, this.CurrentDetailData["ID"].ToString(), null, null, dr, this.displaySP.Text);
                frm.ShowDialog(this);
                frm.Dispose();
                this.RenewData();
            };
            #endregion

            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .Numeric("Testno", header: "No of Test", width: Widths.AnsiChars(5), iseditingreadonly: true, settings: testNoCell)
                .Date("Inspdate", header: "Test Date", width: Widths.AnsiChars(10), iseditingreadonly: true, settings: inspDate)
                .Text("Article", header: "Article", width: Widths.AnsiChars(10), iseditingreadonly: true, settings: articleCell)
                .Text("Result", header: "Result", width: Widths.AnsiChars(10), iseditingreadonly: true, settings: resultCell)
                .Text("Inspector", header: "Inspector", width: Widths.AnsiChars(10), iseditingreadonly: true, settings: inspectorCell)
                .Text("Remark", header: "Remark", width: Widths.AnsiChars(25), iseditingreadonly: true)
                .Text("LastUpdate", header: "Last Update", width: Widths.AnsiChars(30), iseditingreadonly: true);
        }

        public void grid_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            this.grid_CellMouseClick(e.Button, e.RowIndex);
        }

        public void grid_CellMouseClick(System.Windows.Forms.MouseButtons eButton, int eRowIndex)
        {
            if (eButton == System.Windows.Forms.MouseButtons.Right)
            {
                MyUtility.Msg.InfoBox("Right Click Event!!");
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
        }

        protected override void OnFormLoaded()
        {
            this.detailgridmenus.Items.Clear(); // 清空原有的Menu Item
            this.Helper.Controls.ContextMenu.Generator(this.detailgridmenus).Menu("Create New Test", onclick: (s, e) => this.CreateNewTest()).Get(out this.add);
            this.Helper.Controls.ContextMenu.Generator(this.detailgridmenus).Menu("Edit this Record's detail", onclick: (s, e) => this.EditThisDetail()).Get(out this.edit);
            this.Helper.Controls.ContextMenu.Generator(this.detailgridmenus).Menu("Delete this Record's detail", onclick: (s, e) => this.DeleteThisDetail()).Get(out this.delete);

            base.OnFormLoaded();
        }

        protected override DualResult ClickSavePost()
        {
            string sqlcmd = string.Empty;
            foreach (DataRow dr in this.DetailDatas)
            {
                sqlcmd += $@"exec UpdateInspPercent 'LabColorFastness','{dr["POID"]}' ";
            }

            if (!MyUtility.Check.Empty(sqlcmd))
            {
                DualResult result = DBProxy.Current.Execute(null, sqlcmd);
                if (!result)
                {
                    return Result.F(result.ToString());
                }
            }

            return base.ClickSavePost();
        }

        // Context Menu選擇Create New test
        private void CreateNewTest()
        {
            // string ID = MyUtility.GetValue.GetID("CF", "ColorFastness", DateTime.Today, 2, "ID", null);
            // string ID = MyUtility.GetValue.GetID(Sci.Env.User.Keyword + "CF", "ColorFastness", DateTime.Today, 2, "ID", null);
            Sci.Production.Quality.P06_Detail callNewDetailForm = new P06_Detail(this.IsSupportEdit, "New", null, null, null, this.displaySP.Text);
            callNewDetailForm.ShowDialog(this);
            callNewDetailForm.Dispose();
            this.RenewData();
            this.OnDetailEntered();
        }

        // Context Menu選擇Edit This Record's Detail
        private void EditThisDetail()
        {
            string currentID = this.CurrentDetailData["ID"].ToString();
            string spno = this.displaySP.Text;
            var dr = this.CurrentDetailData;
            if (dr == null)
            {
                return;
            }

            var frm = new Sci.Production.Quality.P06_Detail(this.IsSupportEdit, this.CurrentDetailData["ID"].ToString(), null, null, dr, this.displaySP.Text);
            frm.ShowDialog(this);
            frm.Dispose();

            // contextMenuStrip();
            this.RenewData();
            this.OnDetailEntered();

            // 固定滑鼠指向位置,避免被renew影響
            int rowindex = 0;
            for (int rIdx = 0; rIdx < this.detailgrid.Rows.Count; rIdx++)
            {
                DataGridViewRow dvr = this.detailgrid.Rows[rIdx];
                DataRow row = ((DataRowView)dvr.DataBoundItem).Row;

                if (row["ID"].ToString() == currentID)
                {
                    rowindex = rIdx;
                    break;
                }
            }

            this.detailgrid.SelectRowTo(rowindex);
        }

        // Context Menu選擇Delete This Record's Detail
        private void DeleteThisDetail()
        {
            DataTable dt = (DataTable)this.gridbs.DataSource;
            var dr = this.CurrentDetailData;

            if (dr["Status"].ToString() == "Confirmed")
            {
                return;
            }
            else
            {
                DualResult dResult;
                List<SqlParameter> spam = new List<SqlParameter>();
                spam.Add(new SqlParameter("@id", this.CurrentDetailData["ID"].ToString()));
                if (dResult = DBProxy.Current.Execute(null, @"delete from ColorFastness_Detail where id=@id  delete from ColorFastness where id=@id", spam))
                {
                    MyUtility.Msg.InfoBox("Data has been Delete! ");
                }
                else
                {
                    MyUtility.Msg.WarningBox("fail");
                }
            }

            this.contextMenuStrip();
            this.RenewData();
            this.OnDetailEntered();
        }

        private void contextMenuStrip()
        {
            var dr = this.CurrentDetailData;
            DataTable dtCheck;
            DataTable dtCheckDelete;
            this.add.Enabled = true;
            if (dr == null) // ColorFastness 空的
            {
                this.add.Enabled = true;
                this.edit.Enabled = false;
                this.delete.Enabled = false;
                return;
            }
            else // ColorFastness 有東西
            {
                DBProxy.Current.Select(null, string.Format("select * from ColorFastness WITH (NOLOCK) where id='{0}'", this.CurrentDetailData["ID"].ToString()), out dtCheck);
                DBProxy.Current.Select(null, string.Format("select * from ColorFastness WITH (NOLOCK) where POID='{0}'", this.displaySP.Text.ToString()), out dtCheckDelete);
                if (dtCheck.Rows.Count <= 0)
                {
                    this.edit.Enabled = false;
                    this.delete.Enabled = false;
                    return;
                }

                if (dtCheck.Rows.Count != 0)
                {
                    if (dtCheck.Rows[0]["Status"].ToString().Trim() == "New")
                    {
                        this.edit.Enabled = true;
                        this.delete.Enabled = true;
                    }
                    else
                    {
                        this.edit.Enabled = true;
                        this.delete.Enabled = false;
                    }
                }
            }

            DataTable dt = (DataTable)this.detailgridbs.DataSource;

            // 判斷Grid有無資料 , 沒資料就傳true並關閉 ContextMenu edit & delete
            if (dtCheck.Rows.Count <= 0)
            {
                this.edit.Enabled = false;
                this.delete.Enabled = false;
            }

            if (this.EditMode)
            {
                this.add.Enabled = false;
                this.edit.Enabled = false;
                this.delete.Enabled = false;
            }
        }

        protected override DualResult OnRenewDataDetailPost(RenewDataPostEventArgs e)
        {
            DataTable dt = (DataTable)e.Details;
            dt.Columns.Add("LastUpdate", typeof(string));
            int i = 0;

            foreach (DataRow dr in dt.Rows)
            {
                dr["LastUpdate"] = Sci.Production.Class.Commons.UserPrg.GetName(
                    MyUtility.Check.Empty(dt.Rows[i]["EditName"].ToString()) ?
                    dt.Rows[i]["addName"].ToString() : dt.Rows[i]["EditName"].ToString(), Sci.Production.Class.Commons.UserPrg.NameType.nameOnly) + " - " + (
                    MyUtility.Check.Empty(dt.Rows[i]["EditDate"].ToString()) ?
                    ((DateTime)dt.Rows[i]["addDate"]).ToString("yyyy/MM/dd HH:mm:ss") :
                    ((DateTime)dt.Rows[i]["EditDate"]).ToString("yyyy/MM/dd HH:mm:ss"));
                i++;
            }

            return base.OnRenewDataDetailPost(e);
        }

        protected override void OnDetailGridRowChanged()
        {
            this.contextMenuStrip();
            base.OnDetailGridRowChanged();
        }
    }
}
