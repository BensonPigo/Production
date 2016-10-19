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
using Sci.Production;

using Sci.Production.PublicPrg;
using System.Linq;



namespace Sci.Production.Subcon
{
    public partial class P02 : Sci.Win.Tems.Input6
    {

        public P02(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.DefaultFilter = "MdivisionID = '" + Sci.Env.User.Keyword + "' and POTYPE='I'";
            gridicon.Append.Enabled = false;
            gridicon.Append.Visible = false;
            gridicon.Insert.Enabled = false;
            gridicon.Insert.Visible = false;

            this.txtsubcon1.TextBox1.Validated += (s, e) =>
            {
                if (this.EditMode && this.txtsubcon1.TextBox1.Text != this.txtsubcon1.TextBox1.OldValue)
                {
                    CurrentMaintain["CurrencyID"] = MyUtility.GetValue.Lookup("CurrencyID", this.txtsubcon1.TextBox1.Text, "LocalSupp", "ID");
                    ((DataTable)detailgridbs.DataSource).Rows.Clear();  //清空表身資料
                }
            };

        }

        // 新增時預設資料
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["Mdivisionid"] = Sci.Env.User.Keyword;
            CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
            CurrentMaintain["ISSUEDATE"] = System.DateTime.Today;
            CurrentMaintain["POType"] = "I";
            CurrentMaintain["HANDLE"] = Sci.Env.User.UserID;
            CurrentMaintain["VatRate"] = 0;
            CurrentMaintain["amount"] = 0;
            CurrentMaintain["vat"] = 0;
            CurrentMaintain["closed"] = 0;
            ((DataTable)(detailgridbs.DataSource)).Rows[0].Delete();
        }

        // delete前檢查

        protected override bool ClickDeleteBefore()
        {
            DataRow dr = grid.GetDataRow<DataRow>(grid.GetSelectedRowIndex());
            if (dr["Status"].ToString() != "New")
            {
                MyUtility.Msg.WarningBox("Data is approved or closed, can't delete.", "Warning");
                return false;
            }

            System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
            sp1.ParameterName = "@id";
            sp1.Value = dr["id"].ToString();

            IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>();
            paras.Add(sp1);

            string sqlcmd;
            sqlcmd = "select fd.ID from ArtworkPO_Detail ad, FarmOut_Detail fd where ad.Ukey = fd.ArtworkPo_DetailUkey and ad.id = @id" +
                     "   union all " +
                     "   select fo.ID from ArtworkPO_Detail ad, FarmOut_Detail fo where ad.Ukey = fo.ArtworkPo_DetailUkey and ad.id = @id " +
                     "      union all" +
                     "   select fi.ID from ArtworkPO_Detail ad, FarmIn_Detail fi where ad.Ukey = fi.ArtworkPo_DetailUkey and ad.id = @id";

            DataTable dt;
            DBProxy.Current.Select(null, sqlcmd, paras, out dt);
            if (dt.Rows.Count > 0)
            {
                string ids = "";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ids += dt.Rows[i][0].ToString() + ";";
                }
                MyUtility.Msg.WarningBox(string.Format("Below IDs {0} refer to details data, can't delete.", ids), "Warning");
                return false;
            }
            return base.ClickDeleteBefore();
        }

        // edit前檢查
        protected override bool ClickEditBefore()
        {
            //!EMPTY(APVName) OR !EMPTY(Closed)，只能編輯remark欄。
            DataRow dr = grid.GetDataRow<DataRow>(grid.GetSelectedRowIndex());
            if (dr["Status"].ToString() != "New")
            {
                var frm = new Sci.Production.PublicForm.EditRemark("artworkpo", "remark", dr);
                frm.ShowDialog(this);
                this.RenewData();
                return false;
            }

            return base.ClickEditBefore();
        }

        // save前檢查 & 取id
        protected override bool ClickSaveBefore()
        {

            #region 必輸檢查
            if (CurrentMaintain["LocalSuppID"] == DBNull.Value || string.IsNullOrWhiteSpace(CurrentMaintain["LocalSuppID"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Suppiler >  can't be empty!", "Warning");
                txtsubcon1.TextBox1.Focus();
                return false;
            }

            if (CurrentMaintain["issuedate"] == DBNull.Value || string.IsNullOrWhiteSpace(CurrentMaintain["issuedate"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Issue Date >  can't be empty!", "Warning");
                dateBox1.Focus();
                return false;
            }

            if (CurrentMaintain["Delivery"] == DBNull.Value || string.IsNullOrWhiteSpace(CurrentMaintain["Delivery"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Delivery Date >  can't be empty!", "Warning");
                dateBox2.Focus();
                return false;
            }

            if (CurrentMaintain["ArtworktypeId"] == DBNull.Value || string.IsNullOrWhiteSpace(CurrentMaintain["ArtworktypeId"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Artwork Type >  can't be empty!", "Warning");
                txtartworktype_fty1.Focus();
                return false;
            }

            if (CurrentMaintain["Handle"] == DBNull.Value || string.IsNullOrWhiteSpace(CurrentMaintain["Handle"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Handle >  can't be empty!", "Warning");
                txtuser1.TextBox1.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["factoryid"]))
            {
                MyUtility.Msg.WarningBox("< Factory Id >  can't be empty!", "Warning");
                txtmfactory1.Focus();
                return false;
            }

            #endregion

            if (DetailDatas.Count == 0)
            {
                MyUtility.Msg.WarningBox("Detail can't be empty", "Warning");
                return false;
            }

            //取單號： getID(MyApp.cKeyword+GetDocno('PMS', 'ARTWORKPO1'), 'ARTWORKPO', IssueDate, 2)
            if (this.IsDetailInserting)
            {
                string factorykeyword = Sci.MyUtility.GetValue.Lookup(string.Format("select keyword from dbo.factory where ID ='{0}'", CurrentMaintain["factoryid"]));
                if (MyUtility.Check.Empty(factorykeyword))
                {
                    MyUtility.Msg.WarningBox("Factory Keyword is empty, Please contact to MIS!!");
                    return false;
                }
                CurrentMaintain["id"] = Sci.MyUtility.GetValue.GetID(factorykeyword + "OS", "artworkpo", (DateTime)CurrentMaintain["issuedate"]);
            }

            return base.ClickSaveBefore();
        }

        // grid 加工填值
        protected override DualResult OnRenewDataDetailPost(RenewDataPostEventArgs e)
        {
            if (!tabs.TabPages[0].Equals(tabs.SelectedTab))
            {
                (e.Details).Columns.Add("Style", typeof(String));
                (e.Details).Columns.Add("sewinline", typeof(DateTime));
                (e.Details).Columns.Add("scidelivery", typeof(DateTime));

                foreach (DataRow dr in e.Details.Rows)
                {
                    DataTable order_dt;
                    DBProxy.Current.Select(null, string.Format("select styleid, sewinline, scidelivery from orders where id='{0}'", dr["orderid"].ToString()), out order_dt);
                    if (order_dt.Rows.Count == 0)
                        break;
                    dr["style"] = order_dt.Rows[0]["styleid"].ToString();
                    dr["sewinline"] = order_dt.Rows[0]["sewinline"];
                    dr["scidelivery"] = order_dt.Rows[0]["scidelivery"];

                }
            }
            return base.OnRenewDataDetailPost(e);
        }

        //refresh
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            #region --動態unit header --
            string artworkunit = MyUtility.GetValue.Lookup(string.Format("select artworkunit from artworktype where id='{0}'", CurrentMaintain["artworktypeid"])).ToString().Trim();
            if (artworkunit == "") artworkunit = "PCS";
            this.detailgrid.Columns[6].HeaderText = "Cost(" + artworkunit + ")";
            this.detailgrid.Columns[7].HeaderText = artworkunit;
            #endregion
            txtsubcon1.Enabled = !this.EditMode || IsDetailInserting;
            txtartworktype_fty1.Enabled = !this.EditMode || IsDetailInserting;
            txtmfactory1.Enabled = !this.EditMode || IsDetailInserting;
            #region Status Label
            label25.Text = CurrentMaintain["Status"].ToString();
            #endregion
            #region exceed status
            label17.Visible = CurrentMaintain["Exceed"].ToString().ToUpper() == "TRUE";
            #endregion
            #region Batch Import, Special record button
            button4.Enabled = this.EditMode;
            button5.Enabled = this.EditMode;
            #endregion
            #region Batch Create
            button3.Enabled = !this.EditMode;
            #endregion
        }

        // Detail Grid 設定
        protected override void OnDetailGridSetup()
        {
            #region SP#右鍵開窗
            Ict.Win.DataGridViewGeneratorTextColumnSettings ts4 = new DataGridViewGeneratorTextColumnSettings();
            ts4.EditingMouseDown += (s, e) =>
            {
                if (!this.EditMode && CurrentMaintain["closed"].ToString().ToUpper() == "FALSE" && e.Button == MouseButtons.Right)
                {
                    var frm = new Sci.Production.Subcon.P01_ModifyPoQty(CurrentDetailData);
                    DialogResult result = frm.ShowDialog(this);

                    if (result == DialogResult.OK)
                    {
                        this.RenewData();
                        this.OnDetailEntered();
                    }
                }

            };
            #endregion
            #region farm out qty 開窗
            Ict.Win.DataGridViewGeneratorTextColumnSettings ts = new DataGridViewGeneratorTextColumnSettings();
            ts.CellMouseDoubleClick += (s, e) =>
            {
                if (!this.EditMode)
                {
                    var dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    if (null == dr) return;
                    var frm = new Sci.Production.Subcon.P01_FarmOutList(dr);
                    frm.ShowDialog(this);
                    this.RenewData();
                }

            };
            #endregion
            #region Farm In qty 開窗
            Ict.Win.DataGridViewGeneratorTextColumnSettings ts2 = new DataGridViewGeneratorTextColumnSettings();
            ts2.CellMouseDoubleClick += (s, e) =>
            {
                if (!this.EditMode)
                {
                    var dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    if (null == dr) return;
                    var frm = new Sci.Production.Subcon.P01_FarmInList(dr);
                    frm.ShowDialog(this);
                    this.RenewData();
                }

            };
            #endregion
            #region AP qty 開窗
            Ict.Win.DataGridViewGeneratorTextColumnSettings ts3 = new DataGridViewGeneratorTextColumnSettings();
            ts3.CellMouseDoubleClick += (s, e) =>
            {
                if (!this.EditMode)
                {
                    var dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    if (null == dr) return;
                    var frm = new Sci.Production.Subcon.P01_Ap(dr);
                    frm.ShowDialog(this);
                    this.RenewData();
                }

            };
            #endregion
            MyUtility.Tool.SetGridFrozen(this.detailgrid);
            #region 欄位設定
            Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("orderid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true, settings: ts4)  //0
            .Text("Style", header: "Style", width: Widths.AnsiChars(15), iseditingreadonly: true)   //1
            .Numeric("PoQty", header: "PO Qty", width: Widths.AnsiChars(6), iseditingreadonly: true)    //2
            .Date("sewinline", header: "SewInLine", width: Widths.AnsiChars(10), iseditingreadonly: true)   //3
            .Date("scidelivery", header: "SciDelivery", width: Widths.AnsiChars(10), iseditingreadonly: true)   //4
            .Text("ArtworkId", header: "Artwork", width: Widths.AnsiChars(8), iseditingreadonly: true)    //5
            .Numeric("coststitch", header: "Cost(PCS/Stitch)", width: Widths.AnsiChars(5), iseditingreadonly: true)//6
            .Numeric("stitch", header: "PCS/Stitch", width: Widths.AnsiChars(5))    //7
            .Text("patterncode", header: "CutpartID", width: Widths.AnsiChars(10), iseditingreadonly: true) //8
            .Text("PatternDesc", header: "Cutpart Name", width: Widths.AnsiChars(15))   //9

            .Numeric("qtygarment", header: "Qty/GMT", width: Widths.AnsiChars(5), integer_places: 2)  //10

            .Text("farmout", header: "Farm Out", width: Widths.AnsiChars(5), settings: ts, iseditingreadonly: true) //11
            .Text("farmin", header: "Farm In", width: Widths.AnsiChars(5), settings: ts2, iseditingreadonly: true)  //12
            .Text("apqty", header: "A/P Qty", width: Widths.AnsiChars(5), settings: ts3, iseditingreadonly: true)   //13
            .Text("exceedqty", header: "Exceed", width: Widths.AnsiChars(5), iseditingreadonly: true);     //14
            #endregion
            #region 可編輯欄位變色
            detailgrid.Columns[7].DefaultCellStyle.BackColor = Color.Pink;  //PCS/Stitch
            detailgrid.Columns[9].DefaultCellStyle.BackColor = Color.Pink;  //Cutpart Name
            detailgrid.Columns[10].DefaultCellStyle.BackColor = Color.Pink; //Qty/GMT
            #endregion
        }

        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            String sqlcmd;

            sqlcmd = string.Format("update artworkpo set Status = 'Approved', apvname='{0}', apvdate = GETDATE() , editname = '{0}' , editdate = GETDATE() " +
                            "where id = '{1}'", Env.User.UserID, CurrentMaintain["id"]);

            DualResult result;
            if (!(result = DBProxy.Current.Execute(null, sqlcmd)))
            {
                ShowErr(sqlcmd, result);
                return;
            }
            RenewData();
            OnDetailEntered();
            this.EnsureToolbarExt();
        }

        //無權
        //protected override void ClickUnconfirm()
        //{
        //    base.ClickUnconfirm();
        //    String sqlcmd;

        //    DialogResult dResult = MyUtility.Msg.QuestionBox("Are you sure to unapprove it?", "Question", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);
        //    if (dResult.ToString().ToUpper() == "NO") return;
        //    sqlcmd = string.Format("update artworkpo set status = 'New', apvname='', apvdate = null , editname = '{0}' , editdate = GETDATE() " +
        //                    "where id = '{1}'", Env.User.UserID, CurrentMaintain["id"]);


        //    DualResult result;
        //    if (!(result = DBProxy.Current.Execute(null, sqlcmd)))
        //    {
        //        ShowErr(sqlcmd, result);
        //        return;
        //    }
        //    RenewData();
        //    OnDetailEntered();
        //    this.EnsureToolbarExt();
        //}

        protected override void ClickClose()
        {
            base.ClickClose();
            if (!Prgs.GetAuthority(Env.User.UserID) && CurrentMaintain["apvname"].ToString() != Env.User.UserID)
            {
                MyUtility.Msg.InfoBox("Only Apporver & leader can close!");
                return;
            }
            String sqlcmd;
            sqlcmd = string.Format("update artworkpo set status = 'Closed', closed=1 , editname = '{0}' , editdate = GETDATE() " +
                            "where id = '{1}'", Env.User.UserID, CurrentMaintain["id"]);

            DualResult result;
            if (!(result = DBProxy.Current.Execute(null, sqlcmd)))
            {
                ShowErr(sqlcmd, result);
                return;
            }
            RenewData();
            OnDetailEntered();
            this.EnsureToolbarExt();
        }

        protected override void ClickUnclose()
        {
            base.ClickUnclose();
            if (!(Prgs.GetAuthority(Env.User.UserID)))
            {
                MyUtility.Msg.InfoBox("Only Apporver & leader can unclose!");
                return;
            }

            String sqlcmd;
            DialogResult dResult = MyUtility.Msg.QuestionBox("Are you sure to unclose it?", "Question", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);
            if (dResult.ToString().ToUpper() == "NO") return;
            sqlcmd = string.Format("update artworkpo set status = 'Approved', closed=0  , editname = '{0}' , editdate = GETDATE() " +
                            "where id = '{1}'", Env.User.UserID, CurrentMaintain["id"]);

            DualResult result;
            if (!(result = DBProxy.Current.Execute(null, sqlcmd)))
            {
                ShowErr(sqlcmd, result);
                return;
            }
            RenewData();
            OnDetailEntered();
            this.EnsureToolbarExt();
        }

        //batch import
        private void button4_Click(object sender, EventArgs e)
        {
            var dr = CurrentMaintain; if (null == dr) return;
            if (dr["localsuppid"] == DBNull.Value)
            {
                MyUtility.Msg.WarningBox("Please fill Supplier first!");
                txtsubcon1.TextBox1.Focus();
                return;
            }

            if (dr["artworktypeid"] == DBNull.Value)
            {
                MyUtility.Msg.WarningBox("Please fill Artworktype first!");
                txtartworktype_fty1.Focus();
                return;
            }
            var frm = new Sci.Production.Subcon.P01_Import(dr, (DataTable)detailgridbs.DataSource, "P02");
            frm.ShowDialog(this);
            this.RenewData();
        }

        //Special Record
        private void button5_Click(object sender, EventArgs e)
        {
            var dr = CurrentMaintain; if (null == dr) return;
            if (dr["artworktypeid"] == DBNull.Value)
            {
                MyUtility.Msg.WarningBox("Please fill Artworktype first!");
                txtartworktype_fty1.Focus();
                return;
            }

            var frm = new Sci.Production.Subcon.P01_SpecialRecord(dr, (DataTable)detailgridbs.DataSource, "P02");
            frm.ShowDialog(this);
            this.RenewData();
            detailgridbs.EndEdit();
        }

        // batch create
        private void button3_Click(object sender, EventArgs e)
        {
            if (this.EditMode) return;
            var frm = new Sci.Production.Subcon.P01_BatchCreate("P02");
            frm.ShowDialog(this);
            ReloadDatas();
        }

        private void txtartworktype_fty1_Validating(object sender, CancelEventArgs e)
        {
            Production.Class.txtartworktype_fty o;
            o = (Production.Class.txtartworktype_fty)sender;

            if ((o.Text != o.OldValue) && this.EditMode)
            {
                ((DataTable)detailgridbs.DataSource).Rows.Clear();  //清空表身資料
            }
        }

    }
}
