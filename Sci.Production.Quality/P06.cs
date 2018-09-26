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
using System.Data.SqlClient;

namespace Sci.Production.Quality
{
    public partial class P06 : Sci.Win.Tems.Input6
    {
        // 宣告Context Menu Item
        ToolStripMenuItem add, edit, delete;
        private new bool IsSupportEdit = true;

        public P06(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.detailgrid.ContextMenuStrip = detailgridmenus;            
        }

        //refresh
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
            spam.Add(new SqlParameter("@id", CurrentMaintain["ID"].ToString()));

            if (MyUtility.Check.Seek(sql_cmd, spam, out dr))
            {
                this.displaySP.Text = dr["id"].ToString();
                this.displayStyle.Text = dr["StyleID"].ToString();
                this.displaySeason.Text = dr["SeasonID"].ToString();
                this.displayBrand.Text = dr["BrandID"].ToString();
                this.editRemark.Text = dr["ColorFastnessLaboratoryRemark"].ToString();
            }

            if (MyUtility.Check.Seek(string.Format("select min(a.CutInLine) as CutInLine from Orders a WITH (NOLOCK) left join PO b WITH (NOLOCK) on a.POID=b.ID WHERE a.Poid='{0}'", CurrentMaintain["id"]), out drEarly))
            {
                if (drEarly["CutInLine"] == DBNull.Value) dateEarliestEstCuttingDate.Text = "";
                else dateEarliestEstCuttingDate.Value = Convert.ToDateTime(drEarly["CutInLine"]);
            }
            if (MyUtility.Check.Seek(string.Format("select * from dbo.GetSCI('{0}','')", CurrentMaintain["id"].ToString()), out drSci))
            {
                if (!MyUtility.Check.Empty(drSci["MinSciDelivery"]))
                {
                    if (drSci["MinSciDelivery"] == DBNull.Value) dateEarliestSCIDel.Text = "";
                    else dateEarliestSCIDel.Value = Convert.ToDateTime(drSci["MinSciDelivery"]);
                }
            }

            DateTime? targT = null;
            if (!MyUtility.Check.Empty(drEarly["CUTINLINE"]) && !MyUtility.Check.Empty(drSci["MinSciDelivery"]))
            {
                targT = Sci.Production.PublicPrg.Prgs.GetTargetLeadTime(drEarly["CUTINLINE"], drSci["MinSciDelivery"]);
            }
            if (targT != null)
            {
                dateTargetLeadtime.Value = targT;
            }
            else
            {
                dateTargetLeadtime.Text = "";
            }
            decimal dRowCount = DetailDatas.Count;
            string inspnum = "0";
            DataTable articleDT = (DataTable)detailgridbs.DataSource;

            if (articleDT.Rows.Count != 0)
            {
                DataRow[] articleAry = articleDT.Select("status='Confirmed'");
                if (articleAry.Length > 0)
                {
                    inspnum = Math.Round(((decimal)articleAry.Length / dRowCount) * 100, 2).ToString();
                    displayArticleofInspection.Text = inspnum + "%";
                }
                else
                {
                    displayArticleofInspection.Text = "";
                }
            }
            else
            {
                displayArticleofInspection.Text = "";
            }

            DateTime CompDate;
            if (inspnum == "100")
            {
                CompDate = ((DateTime)articleDT.Compute("Max(Inspdate)", ""));
                dateCompletionDate.Value = CompDate;
            }
            else
            {
                dateCompletionDate.Text = "";
            }

            //判斷Grid有無資料 , 沒資料就傳true並關閉 ContextMenu edit & delete
            contextMenuStrip();

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

            Helper.Controls.Grid.Generator(this.detailgrid)
                .Numeric("Testno", header: "No of Test", width: Widths.AnsiChars(5), iseditingreadonly: true, settings: testNoCell)
                .Date("Inspdate", header: "Test Date", width: Widths.AnsiChars(10), iseditingreadonly: true, settings: inspDate)
                .Text("Article", header: "Article", width: Widths.AnsiChars(10), iseditingreadonly: true, settings: articleCell)
                .Text("Result", header: "Result", width: Widths.AnsiChars(10), iseditingreadonly: true, settings: resultCell)
                .Text("Inspector", header: "Inspector", width: Widths.AnsiChars(10), iseditingreadonly: true, settings: inspectorCell)
                .Text("Remark", header: "Remark", width: Widths.AnsiChars(25), iseditingreadonly: true)
                .Text("LastUpdate", header: "Last Update", width: Widths.AnsiChars(30), iseditingreadonly: true);

        }

        public void grid_CellMouseClick(object sender,DataGridViewCellMouseEventArgs e)
        {
            this.grid_CellMouseClick(e.Button, e.RowIndex);
        }

        public void grid_CellMouseClick(System.Windows.Forms.MouseButtons eButton, int eRowIndex)
        {
            if (eButton== System.Windows.Forms.MouseButtons.Right)
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
            detailgridmenus.Items.Clear();//清空原有的Menu Item
            Helper.Controls.ContextMenu.Generator(this.detailgridmenus).Menu("Create New Test", onclick: (s, e) => CreateNewTest()).Get(out add);
            Helper.Controls.ContextMenu.Generator(this.detailgridmenus).Menu("Edit this Record's detail", onclick: (s, e) => EditThisDetail()).Get(out edit);
            Helper.Controls.ContextMenu.Generator(this.detailgridmenus).Menu("Delete this Record's detail", onclick: (s, e) => DeleteThisDetail()).Get(out delete);          
            
            base.OnFormLoaded();
        }     

        // Context Menu選擇Create New test
        private void CreateNewTest()
        {
            //string ID = MyUtility.GetValue.GetID("CF", "ColorFastness", DateTime.Today, 2, "ID", null);
            //string ID = MyUtility.GetValue.GetID(Sci.Env.User.Keyword + "CF", "ColorFastness", DateTime.Today, 2, "ID", null);

            Sci.Production.Quality.P06_Detail callNewDetailForm = new P06_Detail(IsSupportEdit, "New", null, null, null, this.displaySP.Text);
            callNewDetailForm.ShowDialog(this);
            callNewDetailForm.Dispose();
            this.RenewData();
            OnDetailEntered();
        }

        // Context Menu選擇Edit This Record's Detail
        private void EditThisDetail()
        {
            string currentID = this.CurrentDetailData["ID"].ToString();
            string spno = this.displaySP.Text;
            var dr = this.CurrentDetailData; if (null == dr) return;
            var frm = new Sci.Production.Quality.P06_Detail(IsSupportEdit, CurrentDetailData["ID"].ToString(), null, null, dr, displaySP.Text);
            frm.ShowDialog(this);
            frm.Dispose();
            //contextMenuStrip();
            this.RenewData();
            OnDetailEntered();
            // 固定滑鼠指向位置,避免被renew影響
            int rowindex = 0;
            for (int rIdx = 0; rIdx < detailgrid.Rows.Count; rIdx++)
            {
                DataGridViewRow dvr = detailgrid.Rows[rIdx];
                DataRow row = ((DataRowView)dvr.DataBoundItem).Row;

                if (row["ID"].ToString()== currentID )
                {
                    rowindex = rIdx;
                    break;
                }
            }
            detailgrid.SelectRowTo(rowindex);

        }
        
        // Context Menu選擇Delete This Record's Detail
        private void DeleteThisDetail()
        {
            DataTable dt = (DataTable)gridbs.DataSource;
            var dr = this.CurrentDetailData;

            if (dr["Status"].ToString() == "Confirmed")
            {
                return;
            }
            else
            {
                DualResult dResult;
                List<SqlParameter> spam = new List<SqlParameter>();
                spam.Add(new SqlParameter("@id", CurrentDetailData["ID"].ToString()));
                if (dResult = DBProxy.Current.Execute(null, @"delete from ColorFastness_Detail where id=@id  delete from ColorFastness where id=@id", spam))
                {
                    MyUtility.Msg.InfoBox("Data has been Delete! ");
                }
                else
                {
                    MyUtility.Msg.WarningBox("fail");
                }
            }
            contextMenuStrip();
            this.RenewData();
            OnDetailEntered();
        }

        private void contextMenuStrip()
        {
            var dr = this.CurrentDetailData;
            DataTable dtCheck;
            DataTable dtCheckDelete;
            add.Enabled = true;
            if (dr == null) // ColorFastness 空的
            {
                add.Enabled = true;
                edit.Enabled = false;
                delete.Enabled = false;
                return;
            }
            else // ColorFastness 有東西
            {
                DBProxy.Current.Select(null, string.Format("select * from ColorFastness WITH (NOLOCK) where id='{0}'", CurrentDetailData["ID"].ToString()), out dtCheck);
                DBProxy.Current.Select(null, string.Format("select * from ColorFastness WITH (NOLOCK) where POID='{0}'", displaySP.Text.ToString()), out dtCheckDelete);
                if (dtCheck.Rows.Count <= 0)
                {
                    edit.Enabled = false;
                    delete.Enabled = false;
                    return;
                }
                if (dtCheck.Rows.Count != 0)
                {
                    if (dtCheck.Rows[0]["Status"].ToString().Trim() == "New")
                    {
                        edit.Enabled = true;
                        delete.Enabled = true;
                    }
                    else
                    {
                        edit.Enabled = true;
                        delete.Enabled = false;
                    }
                }
            }

            DataTable dt = (DataTable)detailgridbs.DataSource;

            //判斷Grid有無資料 , 沒資料就傳true並關閉 ContextMenu edit & delete
            if (dtCheck.Rows.Count <= 0)
            {
                edit.Enabled = false;
                delete.Enabled = false;
            }
            if (EditMode)
            {
                add.Enabled = false;
                edit.Enabled = false;
                delete.Enabled = false;
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
            contextMenuStrip();
            base.OnDetailGridRowChanged();
        }
    }
}
