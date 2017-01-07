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
using Sci.Production.Class;


namespace Sci.Production.Quality
{
    public partial class P05 : Sci.Win.Tems.Input6
    {
        // 宣告Context Menu Item
        ToolStripMenuItem add, edit, delete;

        private string loginID = Sci.Env.User.UserID;
        private string Factory = Sci.Env.User.Keyword;
        bool IsSupportEdit = true;

        public P05(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.detailgrid.ContextMenuStrip = detailgridmenus;
        }
       
      
        //refresh
        protected override void OnDetailEntered()
        {
            List<SqlParameter> spam = new List<SqlParameter>();
            DataRow dr,drEarly,drSci;

            string sql_cmd =
                @"select a.ID,b.StyleID,b.SeasonID,b.BrandID,b.CutInLine,c.Article,c.Result,a.OvenLaboratoryRemark,b.factoryid 
                from po a 
                left join Orders b on a.ID = b.POID
                left join Oven c on a.ID=c.POID
                where a.id=@id";
            spam.Add(new SqlParameter("@id", CurrentMaintain["ID"].ToString()));

            if (MyUtility.Check.Seek(sql_cmd, spam, out dr))
            {
                this.sp_text.Text = dr["id"].ToString();
                this.style_text.Text = dr["StyleID"].ToString();                
                this.season_text.Text = dr["SeasonID"].ToString();
                this.brand_text.Text = dr["BrandID"].ToString();
                this.remark_text.Text = dr["OvenLaboratoryRemark"].ToString();
            }

            if (MyUtility.Check.Seek(string.Format("select min(a.CutInLine) as CutInLine from Orders a where a.POID='{0}'", CurrentMaintain["id"].ToString()), out drEarly))
            {
                if (drEarly["CutInLine"] == DBNull.Value) Cutting_text.Text = "";
                else Cutting_text.Value = Convert.ToDateTime(drEarly["CutInLine"]);
            }
            if (MyUtility.Check.Seek(string.Format("select * from dbo.GetSCI('{0}','')",CurrentMaintain["id"].ToString()),out drSci))
            {
                if (!MyUtility.Check.Empty(drSci["MinSciDelivery"]))
                {
                    if (drSci["MinSciDelivery"] == DBNull.Value) Earliest_text.Text = "";
                    else Earliest_text.Value = Convert.ToDateTime(drSci["MinSciDelivery"]);
                }
            }

            DateTime? targT = null;
            if (!MyUtility.Check.Empty(drEarly["CUTINLINE"]) && !MyUtility.Check.Empty(drSci["MinSciDelivery"]))
            {
                targT = Sci.Production.PublicPrg.Prgs.GetTargetLeadTime(drEarly["CUTINLINE"], drSci["MinSciDelivery"]);
            }
            if (targT != null)
            {
                Target_text.Value = targT;
            }
            else
            {
                Target_text.Text = "";
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
                    Article_text.Text = inspnum + "%";
                }
                else
                {
                    Article_text.Text = "";
                }
            }
            else
            {
                Article_text.Text = "";
            }


            DateTime CompDate;
            if (inspnum == "100")
            {
                CompDate = ((DateTime)articleDT.Compute("Max(Inspdate)", ""));
                compl_text.Value = CompDate;
            }
            else
            {
                compl_text.Text = "";
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
                if (dr==null)
                {
                    return;
                }
                var frm = new Sci.Production.Quality.P05_Detail(false, this.CurrentDetailData["ID"].ToString(), null, null, dr,this.sp_text.Text);
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
                var frm = new Sci.Production.Quality.P05_Detail(false, this.CurrentDetailData["ID"].ToString(), null, null, dr, this.sp_text.Text);
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
                var frm = new Sci.Production.Quality.P05_Detail(false, this.CurrentDetailData["ID"].ToString(), null, null, dr, this.sp_text.Text);
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
                var frm = new Sci.Production.Quality.P05_Detail(false, this.CurrentDetailData["ID"].ToString(), null, null, dr, this.sp_text.Text);
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
                var frm = new Sci.Production.Quality.P05_Detail(false, this.CurrentDetailData["ID"].ToString(), null, null, dr, this.sp_text.Text);
                frm.ShowDialog(this);
                frm.Dispose();
                this.RenewData();
            };

            
            #endregion

            Helper.Controls.Grid.Generator(this.detailgrid)
                .Numeric("Testno", header: "No of Test", width: Widths.AnsiChars(5), iseditingreadonly: true,settings:testNoCell)
                .Date("Inspdate", header: "Test Date", width: Widths.AnsiChars(10), iseditingreadonly: true,settings:inspDate)
                .Text("Article", header: "Article", width: Widths.AnsiChars(10), iseditingreadonly: true,settings:articleCell)
                .Text("Result", header: "Result", width: Widths.AnsiChars(10),iseditingreadonly:true,settings:resultCell)
                .Text("Inspector", header: "Inspector", width: Widths.AnsiChars(10),iseditingreadonly:true,settings:inspectorCell)
                .Text("Remark", header: "Remark", width: Widths.AnsiChars(25),iseditingreadonly:true)
                .Text("LastUpdate", header: "Last Update", width: Widths.AnsiChars(30), iseditingreadonly: true);
            
        }
       
        protected override void OnFormLoaded()
        {         
            detailgridmenus.Items.Clear();//清空原有的Menu Item
            Helper.Controls.ContextMenu.Generator(this.detailgridmenus).Menu("Create New Test", onclick: (s, e) => CreateNewTest()).Get(out add);
            Helper.Controls.ContextMenu.Generator(this.detailgridmenus).Menu("Edit this Record's detail", onclick: (s, e) => EditThisDetail()).Get(out edit);
            Helper.Controls.ContextMenu.Generator(this.detailgridmenus).Menu("Delete this Record's detail", onclick: (s, e) => DeleteThisDetail()).Get(out delete);
            

            base.OnFormLoaded();
        }

        private void CreateNewTest()
        {
            DataTable dt;
            DBProxy.Current.Select(null, "select Max(id) as id from Oven", out dt);
        
               int ID = MyUtility.Convert.GetInt(dt.Rows[0]["id"]);
                ID = ID + 1;
                      
            Sci.Production.Quality.P05_Detail callNewDetailForm = new P05_Detail(IsSupportEdit,ID.ToString(), null, null, null, this.sp_text.Text);
            callNewDetailForm.ShowDialog(this);
            callNewDetailForm.Dispose();
            this.RenewData();
            OnDetailEntered();
        }
        // Context Menu選擇Edit This Record's Detail
        private void EditThisDetail()
        {
            string currentID = this.CurrentDetailData["ID"].ToString();
            string spno = this.sp_text.Text;
            var dr = this.CurrentDetailData; if (null == dr) return;
            string id = CurrentDetailData["ID"].ToString();
            var frm = new Sci.Production.Quality.P05_Detail(IsSupportEdit, CurrentDetailData["ID"].ToString(), null, null, dr,sp_text.Text);
            frm.ShowDialog(this);
            frm.Dispose();
            contextMenuStrip();
            this.RenewData();            
            OnDetailEntered();
            // 固定滑鼠指向位置,避免被renew影響
            int rowindex = 0;
            for (int rIdx = 0; rIdx < detailgrid.Rows.Count; rIdx++)
            {
                DataGridViewRow dvr = detailgrid.Rows[rIdx];
                DataRow row = ((DataRowView)dvr.DataBoundItem).Row;

                if (row["ID"].ToString() == currentID)
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
                if (dResult = DBProxy.Current.Execute(null, @"delete from Oven_Detail where id=@id  delete from Oven where id=@id", spam))
	            {
                    MyUtility.Msg.InfoBox("Data has been Delete! ");
	            }
                else
                {
                    MyUtility.Msg.InfoBox("fail");
                }
            }
            contextMenuStrip();          
            this.RenewData();
        }

        private void contextMenuStrip()
        {
            var dr = this.CurrentDetailData;
            DataTable dtCheck;
            DataTable dtCheckDelete;

            add.Enabled = true;

            if (dr == null) // oven 空的
            {
                edit.Enabled = false;
                delete.Enabled = false;
                return;
            }
            else // oven 有東西
            {
                DBProxy.Current.Select(null, string.Format("select * from oven where POID='{0}'", sp_text.Text.ToString()), out dtCheckDelete);
                DBProxy.Current.Select(null, string.Format("select * from oven where id='{0}'", CurrentDetailData["ID"].ToString()), out dtCheck);
                if (dtCheckDelete.Rows.Count <= 0)
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

                DataTable dt = (DataTable)detailgridbs.DataSource;

                //判斷Grid有無資料 , 沒資料就傳true並關閉 ContextMenu edit & delete
                if (dtCheckDelete.Rows.Count <= 0)
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
        }

        protected override DualResult OnRenewDataDetailPost(RenewDataPostEventArgs e)
        {

            DataTable dt = (DataTable)e.Details;
            dt.Columns.Add("LastUpdate", typeof(string));
            int i = 0;

            foreach (DataRow dr in dt.Rows)
            {
                dr["LastUpdate"] = Sci.Production.Class.Commons.UserPrg.GetName(dt.Rows[i]["EditName"].ToString(), Sci.Production.Class.Commons.UserPrg.NameType.nameOnly) + " - " + dt.Rows[i]["EditDate"].ToString();
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
