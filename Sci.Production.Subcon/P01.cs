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
using System.Data.SqlClient;
using Sci.Win;
using System.Reflection;

namespace Sci.Production.Subcon
{
    public partial class P01 : Sci.Win.Tems.Input6
    {
        string artworkunit;
        int IrregularPriceReasonType = 0;
        public static DataTable _IrregularPriceReasonDT;

        public P01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.DefaultFilter = "MdivisionID = '" + Sci.Env.User.Keyword + "' and POTYPE='O'";
            gridicon.Append.Enabled = false;
            gridicon.Append.Visible = false;
            gridicon.Insert.Enabled = false;
            gridicon.Insert.Visible = false;
            dateApproveDate.ReadOnly = true;

            this.txtsubconSupplier.TextBox1.Validated += (s, e) =>
            {
                if (this.EditMode && this.txtsubconSupplier.TextBox1.Text != this.txtsubconSupplier.TextBox1.OldValue)
                {
                    CurrentMaintain["CurrencyID"] = MyUtility.GetValue.Lookup("CurrencyID", this.txtsubconSupplier.TextBox1.Text, "LocalSupp", "ID");
                    ((DataTable)detailgridbs.DataSource).Rows.Clear();

                }
            };
        }

        // 新增時預設資料
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["MdivisionID"] = Sci.Env.User.Keyword;
            CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
            CurrentMaintain["issuedate"] = System.DateTime.Today;
            CurrentMaintain["potype"] = "O";
            CurrentMaintain["handle"] = Sci.Env.User.UserID;
            CurrentMaintain["VatRate"] = 0;
            CurrentMaintain["Status"] = "New";
            ((DataTable)(detailgridbs.DataSource)).Rows[0].Delete();
        }

        // delete前檢查 CurrentMaintain["id"]的FarmOut_Detail/FarmIn_Detail有data則不能刪除
        protected override bool ClickDeleteBefore()
        {
            if (CurrentMaintain["Status"].ToString() != "New")
            {
                MyUtility.Msg.WarningBox("Data is approved or closed, can't delete.", "Warning");
                return false;
            }

            //sql參數準備
            System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
            sp1.ParameterName = "@id";
            sp1.Value = CurrentMaintain["id"].ToString();
            IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>();
            paras.Add(sp1);
            //FarmOut_Detail/FarmIn_Detail
            string sqlcmd;
            sqlcmd = @"select a.Farmin,a.Farmout from ArtworkPO_Detail a WITH (NOLOCK) where a.ID=@id";

            DataTable dt;
            DBProxy.Current.Select(null, sqlcmd, paras, out dt);
            //有則return
            if (dt.AsEnumerable().Any(r => MyUtility.Convert.GetInt(r["Farmin"]) > 0) ||
                dt.AsEnumerable().Any(r => MyUtility.Convert.GetInt(r["Farmout"]) > 0))
            {
                MyUtility.Msg.WarningBox(string.Format("Some SP# already have Farm In/Out data!!!"), "Warning");
                return false;
            }
            return base.ClickDeleteBefore();
        }

        // edit前檢查
        protected override bool ClickEditBefore()
        {
            //!EMPTY(APVName) OR !EMPTY(Closed)，只能編輯remark欄。
            if (CurrentMaintain["Status"].ToString() != "New")
            {
                var frm = new Sci.Production.PublicForm.EditRemark("artworkpo", "remark", CurrentMaintain);
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
                txtsubconSupplier.TextBox1.Focus();
                return false;
            }

            if (CurrentMaintain["issuedate"] == DBNull.Value || string.IsNullOrWhiteSpace(CurrentMaintain["issuedate"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Issue Date >  can't be empty!", "Warning");
                dateIssueDate.Focus();
                return false;
            }

            if (CurrentMaintain["Delivery"] == DBNull.Value || string.IsNullOrWhiteSpace(CurrentMaintain["Delivery"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Delivery Date >  can't be empty!", "Warning");
                dateDeliveryDate.Focus();
                return false;
            }

            if (CurrentMaintain["ArtworktypeId"] == DBNull.Value || string.IsNullOrWhiteSpace(CurrentMaintain["ArtworktypeId"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Artwork Type >  can't be empty!", "Warning");
                txtartworktype_ftyArtworkType.Focus();
                return false;
            }

            if (CurrentMaintain["CurrencyID"] == DBNull.Value || string.IsNullOrWhiteSpace(CurrentMaintain["CurrencyID"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Currency >  can't be empty!", "Warning");
                return false;
            }

            if (CurrentMaintain["Handle"] == DBNull.Value || string.IsNullOrWhiteSpace(CurrentMaintain["Handle"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Handle >  can't be empty!", "Warning");
                txtuserHandle.TextBox1.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["factoryid"]))
            {
                MyUtility.Msg.WarningBox("< Factory Id >  can't be empty!", "Warning");
                txtmfactory.Focus();
                return false;
            }
            #endregion

            foreach (DataRow row in ((DataTable)detailgridbs.DataSource).Select("poqty = 0"))
            {
                row.Delete();
            }

            if (DetailDatas.Count == 0)
            {
                MyUtility.Msg.WarningBox("Detail can't be empty", "Warning");
                return false;
            }

            //取單號： getID(MyApp.cKeyword+GetDocno('PMS', 'ARTWORKPO1'), 'ARTWORKPO', IssueDate, 2)
            if (this.IsDetailInserting)
            {
                string factorykeyword = Sci.MyUtility.GetValue.Lookup(string.Format("select keyword from dbo.factory WITH (NOLOCK) where ID ='{0}'", CurrentMaintain["factoryid"]));
                if (MyUtility.Check.Empty(factorykeyword))
                {
                    MyUtility.Msg.WarningBox("Factory Keyword is empty, Please contact to MIS!!");
                    return false;
                }
                CurrentMaintain["id"] = Sci.MyUtility.GetValue.GetID(Sci.Env.User.Keyword + "OS", "artworkpo", (DateTime)CurrentMaintain["issuedate"]);
            }

            #region 加總明細金額至表頭
            string str = MyUtility.GetValue.Lookup(string.Format("Select exact from Currency WITH (NOLOCK) where id = '{0}'", CurrentMaintain["currencyId"]), null);
            if (str == null || string.IsNullOrWhiteSpace(str))
            {
                MyUtility.Msg.WarningBox(string.Format("<{0}> is not found in Currency Basic Data , can't save!", CurrentMaintain["currencyID"]), "Warning");
                return false;
            }
            int exact = int.Parse(str);
            object detail_a = ((DataTable)detailgridbs.DataSource).Compute("sum(amount)", "");
            CurrentMaintain["amount"] = MyUtility.Math.Round((decimal)detail_a, exact);
            CurrentMaintain["vat"] = MyUtility.Math.Round((decimal)detail_a * (decimal)CurrentMaintain["vatrate"] / 100, exact);





            #endregion

            return base.ClickSaveBefore();
        }

        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["ID"].ToString();

            this.DetailSelectCommand = string.Format(@"
select  *
        , Price = unitprice * qtygarment
        , Style = o.styleid
        , sewinline = o.sewinline
        , scidelivery = o.scidelivery
from dbo.ArtworkPO_Detail a
left join dbo.Orders o on a.OrderID = o.id
where a.id = '{0}'  ORDER BY a.OrderID ", masterID);

            return base.OnDetailSelectCommandPrepare(e);
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            dateApproveDate.ReadOnly = true;
            ChangeDetailHeader();
            #region -- 加總明細金額，顯示於表頭 --
            if (!(CurrentMaintain == null))
            {
                decimal totalPoQty = 0;
                foreach (DataRow drr in ((DataTable)detailgridbs.DataSource).Rows)
                {
                    totalPoQty += (decimal)drr["PoQty"];
                }
                numTotalPOQty.Text = totalPoQty.ToString();
            }
            #endregion
            numTotal.Text = (Convert.ToDecimal(numVat.Text) + Convert.ToDecimal(numAmount.Text)).ToString();
            txtsubconSupplier.Enabled = !this.EditMode || IsDetailInserting;
            txtartworktype_ftyArtworkType.Enabled = !this.EditMode || IsDetailInserting;
            txtmfactory.Enabled = !this.EditMode || IsDetailInserting;
            btnIrrPriceReason.Enabled = !this.EditMode;
            //btnIrprice.Enabled = !this.EditMode;
            #region Status Label
            label25.Text = CurrentMaintain["Status"].ToString();
            #endregion
            #region exceed status
            label17.Visible = CurrentMaintain["Exceed"].ToString().ToUpper() == "TRUE";
            #endregion
            #region Batch Import, Special record button
            btnBatchImport.Enabled = this.EditMode;
            btnSpecialRecord.Enabled = this.EditMode;
            #endregion
            #region Batch create
            btnBatchCreate.Enabled = !this.EditMode;
            #endregion

            #region Irregular Price判斷
            this.btnIrrPriceReason.ForeColor = Color.Black;
            Check_Irregular_Price();
            #endregion
        }

        // Detail Grid 設定
        protected override void OnDetailGridSetup()
        {
            #region SP#右鍵開窗- modify poqty
            Ict.Win.DataGridViewGeneratorTextColumnSettings ts4 = new DataGridViewGeneratorTextColumnSettings();
            ts4.EditingMouseDown += (s, e) =>
            {
                if (!this.EditMode && (CurrentMaintain["Status"].ToString() != "Closed") && e.Button == MouseButtons.Right)
                {
                    var frm = new Sci.Production.Subcon.P01_ModifyPoQty(CurrentDetailData);
                    DialogResult result = frm.ShowDialog(this);

                    if (result == DialogResult.OK)
                    {
                        this.RenewData();
                        this.OnDetailEntered();

                        string sqlcmd2 = string.Format(@"
                                update artworkpo set Amount = {0}, Vat = {1}
                                where id = '{2}'", numAmount.Value, numVat.Value, this.CurrentMaintain["ID"].ToString());
                        DBProxy.Current.Execute(null, sqlcmd2);
                    }
                }
                this.RenewData();
                this.OnDetailEntered();
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
            #region Unit Price Valid
            Ict.Win.DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            ns.CellValidating += (s, e) =>
            {
                if (this.EditMode && e.FormattedValue != null)
                {
                    CurrentDetailData["amount"] = (decimal)CurrentDetailData["poqty"] * (decimal)e.FormattedValue * (decimal)CurrentDetailData["qtygarment"];
                    CurrentDetailData["unitprice"] = e.FormattedValue;
                    CurrentDetailData["Price"] = (decimal)e.FormattedValue * (decimal)CurrentDetailData["qtygarment"];
                }
            };
            #endregion

            #region qtygarment Valid
            Ict.Win.DataGridViewGeneratorNumericColumnSettings ns2 = new DataGridViewGeneratorNumericColumnSettings();
            ns2.CellValidating += (s, e) =>
            {
                if (this.EditMode && e.FormattedValue != null)
                {
                    CurrentDetailData["amount"] = (decimal)CurrentDetailData["poqty"] * (decimal)e.FormattedValue * (decimal)CurrentDetailData["unitprice"];
                    CurrentDetailData["qtygarment"] = e.FormattedValue;
                    CurrentDetailData["Price"] = (decimal)e.FormattedValue * (decimal)CurrentDetailData["unitprice"];
                }
            };
            #endregion
            #region 欄位設定
            Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("orderid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true, settings: ts4)  //0
            .Text("Style", header: "Style", width: Widths.AnsiChars(15), iseditingreadonly: true)   //1
            .Numeric("PoQty", header: "PO Qty", width: Widths.AnsiChars(6), iseditingreadonly: true)    //2
            .Date("sewinline", header: "SewInLine", width: Widths.AnsiChars(10), iseditingreadonly: true)   //3
            .Date("scidelivery", header: "SciDelivery", width: Widths.AnsiChars(10), iseditingreadonly: true)   //4
            .Text("ArtworkId", header: "Artwork", width: Widths.AnsiChars(8), iseditingreadonly: true)    //5
            .Numeric("coststitch", header: "Cost" + Environment.NewLine + "(PCS/Stitch)", width: Widths.AnsiChars(3), iseditingreadonly: true)//6
            .Numeric("stitch", header: "PCS/Stitch", width: Widths.AnsiChars(3))    //7
            .Text("patterncode", header: "Cutpart" + Environment.NewLine + "ID", width: Widths.AnsiChars(5), iseditingreadonly: true) //8
            .Text("PatternDesc", header: "Cutpart Name", width: Widths.AnsiChars(15), iseditingreadonly: true)   //9
            .Numeric("unitprice", header: "Unit Price", width: Widths.AnsiChars(5), settings: ns, decimal_places: 4, integer_places: 4)     //10
            .Numeric("cost", header: "Cost" + Environment.NewLine + "(USD)", width: Widths.AnsiChars(5), iseditingreadonly: true, decimal_places: 4, integer_places: 4)  //11
            .Numeric("qtygarment", header: "Qty/GMT", width: Widths.AnsiChars(5), settings: ns2, integer_places: 2)  //12
            .Numeric("Price", header: "Price/GMT", width: Widths.AnsiChars(5), iseditingreadonly: true, decimal_places: 4, integer_places: 5)   //13
            .Numeric("amount", header: "Amount", width: Widths.AnsiChars(8), iseditingreadonly: true, decimal_places: 2, integer_places: 14)   //14
            .Text("farmout", header: "Farm Out", width: Widths.AnsiChars(5), settings: ts, iseditingreadonly: true) //15
            .Text("farmin", header: "Farm In", width: Widths.AnsiChars(5), settings: ts2, iseditingreadonly: true)  //16
            .Text("apqty", header: "A/P Qty", width: Widths.AnsiChars(5), settings: ts3, iseditingreadonly: true)   //17
            .Text("exceedqty", header: "Exceed", width: Widths.AnsiChars(5), iseditingreadonly: true);     //18
            #endregion
            #region 可編輯欄位變色
            detailgrid.Columns["stitch"].DefaultCellStyle.BackColor = Color.Pink;  //PCS/Stitch
            //detailgrid.Columns[9].DefaultCellStyle.BackColor = Color.Pink;  //Cutpart Name
            detailgrid.Columns["unitprice"].DefaultCellStyle.BackColor = Color.Pink; //Unit Price
            detailgrid.Columns["qtygarment"].DefaultCellStyle.BackColor = Color.Pink; //Qty/GMT
            #endregion
        }

        protected override void ClickConfirm()
        {
            DualResult result;

            string sqlcmd;

            sqlcmd = string.Format("update artworkpo set status = 'Approved', apvname='{0}', apvdate = GETDATE() , editname = '{0}' , editdate = GETDATE() " +
                            "where id = '{1}'", Env.User.UserID, CurrentMaintain["id"]);


            if (!(result = DBProxy.Current.Execute(null, sqlcmd)))
            {
                ShowErr(sqlcmd, result);
                return;
            }

            base.ClickConfirm();
        }

        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            DualResult result;
            string checksql = string.Format("select ApQty from ArtworkPO_Detail WITH (NOLOCK) where id = '{0}'", CurrentMaintain["id"]);
            DataTable checkdt;
            String sqlcmd;
            if (!(result = DBProxy.Current.Select(null, checksql, out checkdt)))
            {
                ShowErr(checksql, result);
                return;
            }
            if (checkdt.Rows.Count > 0)
            {
                if (checkdt.AsEnumerable().Any(row => MyUtility.Convert.GetInt(row["ApQty"]) > 0))//subconP01需要檢查,P02不會有ApQty
                {
                    MessageBox.Show("Can not unconfirm");
                    return;
                }
            }

            DialogResult dResult = MyUtility.Msg.QuestionBox("Are you sure to unapprove it?", "Question", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);
            if (dResult.ToString().ToUpper() == "NO") return;
            sqlcmd = string.Format(@"update artworkpo set status = 'New', apvname='', apvdate = null , editname = '{0}' , editdate = GETDATE()  where id = '{1}'", Env.User.UserID, CurrentMaintain["id"]);
            if (!(result = DBProxy.Current.Execute(null, sqlcmd)))
            {
                ShowErr(sqlcmd, result);
                return;
            }

        }

        protected override void ClickClose()
        {
            base.ClickClose();

            if ((!(Prgs.GetAuthority(Env.User.UserID)) && CurrentMaintain["apvname"].ToString() != Env.User.UserID))
            {
                MyUtility.Msg.InfoBox("Only Apporver & leader can close!");
                return;
            }

            String sqlcmd;
            sqlcmd = string.Format("update artworkpo set status = 'Closed' , editname = '{0}' , editdate = GETDATE() " +
                            "where id = '{1}'", Env.User.UserID, CurrentMaintain["id"]);

            DualResult result;
            if (!(result = DBProxy.Current.Execute(null, sqlcmd)))
            {
                ShowErr(sqlcmd, result);
                return;
            }

        }

        protected override void ClickUnclose()
        {
            base.ClickUnclose();
            if ((!(Prgs.GetAuthority(Env.User.UserID)) && CurrentMaintain["apvname"].ToString() != Env.User.UserID))
            {
                MyUtility.Msg.InfoBox("Only Apporver & leader can unclose!");
                return;
            }
            String sqlcmd;
            DialogResult dResult = MyUtility.Msg.QuestionBox("Are you sure to unclose it?", "Question", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);
            if (dResult.ToString().ToUpper() == "NO") return;
            sqlcmd = string.Format("update artworkpo set Status = 'Approved'  , editname = '{0}' , editdate = GETDATE() " +
                            "where id = '{1}'", Env.User.UserID, CurrentMaintain["id"]);


            DualResult result;
            if (!(result = DBProxy.Current.Execute(null, sqlcmd)))
            {
                ShowErr(sqlcmd, result);
                return;
            }

        }

        //batch import
        private void btnBatchImport_Click(object sender, EventArgs e)
        {
            var dr = CurrentMaintain; if (null == dr) return;
            if (dr["localsuppid"] == DBNull.Value)
            {
                MyUtility.Msg.WarningBox("Please fill Supplier first!");
                txtsubconSupplier.TextBox1.Focus();
                return;
            }
            if (dr["artworktypeid"] == DBNull.Value)
            {
                MyUtility.Msg.WarningBox("Please fill Artworktype first!");
                txtartworktype_ftyArtworkType.Focus();
                return;
            }
            var frm = new Sci.Production.Subcon.P01_Import(dr, (DataTable)detailgridbs.DataSource, "P01");
            frm.ShowDialog(this);

            DataTable dg = (DataTable)detailgridbs.DataSource;
            if (dg.Columns["style"] == null) dg.Columns.Add("Style", typeof(String));
            if (dg.Columns["sewinline"] == null) dg.Columns.Add("sewinline", typeof(DateTime));
            if (dg.Columns["scidelivery"] == null) dg.Columns.Add("scidelivery", typeof(DateTime));
            foreach (DataRow drr in ((DataTable)detailgridbs.DataSource).Rows)
            {
                if (drr.RowState == DataRowState.Deleted) continue;
                drr["Price"] = (Decimal)drr["unitprice"] * (Decimal)drr["qtygarment"];
                DataTable order_dt;
                DBProxy.Current.Select(null, string.Format("select styleid, sewinline, scidelivery from orders WITH (NOLOCK) where id='{0}'", drr["orderid"].ToString()), out order_dt);
                if (order_dt.Rows.Count == 0)
                    break;
                drr["style"] = order_dt.Rows[0]["styleid"].ToString();
                drr["sewinline"] = order_dt.Rows[0]["sewinline"];
                drr["scidelivery"] = order_dt.Rows[0]["scidelivery"];
            }
            this.RenewData();
        }

        //Special Record
        private void btnSpecialRecord_Click(object sender, EventArgs e)
        {
            var dr = CurrentMaintain; if (null == dr) return;
            if (dr["artworktypeid"] == DBNull.Value)
            {
                MyUtility.Msg.WarningBox("Please fill Artworktype first!");
                txtartworktype_ftyArtworkType.Focus();
                return;
            }

            var frm = new Sci.Production.Subcon.P01_SpecialRecord(dr, (DataTable)detailgridbs.DataSource, "P01");
            frm.ShowDialog(this);
            this.RenewData();
            detailgridbs.EndEdit();
        }

        // batch create
        private void btnBatchCreate_Click(object sender, EventArgs e)
        {
            if (this.EditMode) return;
            var frm = new Sci.Production.Subcon.P01_BatchCreate("P01");
            frm.ShowDialog(this);
            ReloadDatas();
        }

        //print
        protected override bool ClickPrint()
        {
            //跳轉至PrintForm
            Sci.Production.Subcon.P01_Print callPrintForm = new Sci.Production.Subcon.P01_Print(this.CurrentMaintain, numTotal.Text, numTotalPOQty.Text);
            callPrintForm.ShowDialog(this);
            return true;
        }

        private void txtartworktype_ftyArtworkType_Validating(object sender, CancelEventArgs e)
        {
            Production.Class.txtartworktype_fty o;
            o = (Production.Class.txtartworktype_fty)sender;

            if ((o.Text != o.OldValue) && this.EditMode)
            {
                ((DataTable)detailgridbs.DataSource).Rows.Clear();
            }
            ChangeDetailHeader();
        }

        private void ChangeDetailHeader()
        {
            #region --動態unit header --
            artworkunit = MyUtility.GetValue.Lookup(string.Format("select artworkunit from artworktype WITH (NOLOCK) where id='{0}'", txtartworktype_ftyArtworkType.Text)).ToString().Trim();
            if (artworkunit == "") artworkunit = "PCS";
            this.detailgrid.Columns["coststitch"].HeaderText = "Cost" + Environment.NewLine + "(" + artworkunit + ")";
            this.detailgrid.Columns["stitch"].HeaderText = artworkunit;
            #endregion
        }

        protected override void OnDetailGridDelete()
        {
            if (((DataTable)this.detailgridbs.DataSource).Rows.Count == 0)
            {
                return;
            }

            string chkp10exists = string.Format(
                @"
select distinct aad.orderid,aad.id
from ArtworkPO_detail apd with(nolock)
inner join ArtworkAP_detail aad with(nolock) on apd.id = aad.artworkpoid and aad.artworkpo_detailukey = apd.ukey
where  apd.id = '{0}' and apd.ukey = '{1}'
",
                CurrentMaintain["id"], CurrentDetailData["Ukey"]);
            DualResult Result;
            DataTable dt;
            if (Result = DBProxy.Current.Select(null, chkp10exists, out dt))
            {
                if (dt.Rows.Count > 0)
                {
                    StringBuilder p10exists = new StringBuilder();
                    foreach (DataRow dr in dt.Rows)
                    {
                        p10exists.Append(string.Format("Please delete [Subcon][P10]:{0} {1} first !! \r\n", dr["id"], dr["orderid"]));
                    }
                    MyUtility.Msg.WarningBox(p10exists.ToString());
                    return;
                }
            }
            else
            {
                MyUtility.Msg.ErrorBox(Result.ToString());
                return;
            }

            base.OnDetailGridDelete();
        }

        private void btnIrrPriceReason_Click(object sender, EventArgs e)
        {
            var frm = new Sci.Production.Subcon.P01_IrregularPriceReason(_IrregularPriceReasonDT, this.CurrentMaintain["ID"].ToString(), IrregularPriceReasonType);
            frm.ShowDialog(this);
        }

        private void Check_Irregular_Price()
        {
            _IrregularPriceReasonDT=null;
            

            //採購價 > 標準價 =異常
            #region 變數宣告

            string poid = string.Empty;
            string artworkType = string.Empty;
            string BrandID = string.Empty;
            string StyleID = string.Empty;
            List<SqlParameter> parameters = new List<SqlParameter>();
            StringBuilder sql = new StringBuilder();
            DualResult result;

            DataTable IrregularPriceReason_InDB;//A DB現有的 "價格異常紀錄" IPR資料
            DataTable StdPrice_Dt;//B 只紀錄標準價用
            DataTable IrregularPriceReason_Newst = new DataTable();//C 最新的 " 價格異常紀錄" IPR資料
            DataTable Form_Except_IPR;//B not exists A 

            #endregion

            #region 欄位定義
            IrregularPriceReason_Newst.Columns.Add(new DataColumn("ArtworkTypeID", System.Type.GetType("System.String")));
            IrregularPriceReason_Newst.Columns.Add(new DataColumn("POID", System.Type.GetType("System.String")));
            IrregularPriceReason_Newst.Columns.Add(new DataColumn("StyleID", System.Type.GetType("System.String")));
            IrregularPriceReason_Newst.Columns.Add(new DataColumn("BrandID", System.Type.GetType("System.String")));
            IrregularPriceReason_Newst.Columns.Add(new DataColumn("PoPrice", System.Type.GetType("System.Decimal")));
            IrregularPriceReason_Newst.Columns.Add(new DataColumn("StdPrice", System.Type.GetType("System.Decimal")));
            IrregularPriceReason_Newst.Columns.Add(new DataColumn("SubconReasonID", System.Type.GetType("System.String")));
            IrregularPriceReason_Newst.Columns.Add(new DataColumn("AddDate", System.Type.GetType("System.DateTime")));
            IrregularPriceReason_Newst.Columns.Add(new DataColumn("AddName", System.Type.GetType("System.String")));
            IrregularPriceReason_Newst.Columns.Add(new DataColumn("EditDate", System.Type.GetType("System.DateTime")));
            IrregularPriceReason_Newst.Columns.Add(new DataColumn("EditName", System.Type.GetType("System.String")));
            #endregion

            string artWorkPO_ID = this.CurrentMaintain["ID"].ToString();
            parameters.Add(new SqlParameter("@artWorkPO_ID", artWorkPO_ID));
            //標準價
            decimal StdPrice = 0;

            //	1.標準價
            #region 取得POID以及對應的標準價、ArtworkType
            //先找出這個加工單底下，所有單號的POID(母單)，以及其加工類別
            sql.Append(" SELECT DISTINCT [ArtworkTypeID]=ad.ArtworkTypeID,  [POID]=o.POID  ,o.BrandID ,o.StyleID" + Environment.NewLine);
            sql.Append(" INTO #tmp_AllOrders" + Environment.NewLine);
            sql.Append("    FROM ArtworkPO a" + Environment.NewLine);
            sql.Append("    INNER JOIN ArtworkPO_Detail ad ON a.ID=ad.ID" + Environment.NewLine);
            sql.Append("    INNER JOIN Orders o ON o.ID=ad.OrderID" + Environment.NewLine);
            sql.Append("    WHERE a.ID=@artWorkPO_ID" + Environment.NewLine);
            sql.Append(" " + Environment.NewLine);

            //用上述資訊，與Orders、Order_TmsCost關聯，取得所有「有設定標準價」的子單數量總和、個別子單標準價* 個別子單數量
            //標準價 = SUM(子單數量 * 子單標準價) / SUM(子單數量)
            sql.Append(" SELECT " + Environment.NewLine + Environment.NewLine);
            sql.Append("         [Order_Qty]= IIF(odm.ArtworkTypeID IS NULL,NULL,ISNULL(SUM(od.qty),0)) " + Environment.NewLine);
            sql.Append("        ,[Order_Amt]= SUM(od.qty *Price) " + Environment.NewLine);
            sql.Append("        ,[POID]= t.POID" + Environment.NewLine);
            sql.Append("        ,[ArtworkTypeID]= odm.ArtworkTypeID" + Environment.NewLine);
            sql.Append("        ,[BrandID]= t.BrandID" + Environment.NewLine);
            sql.Append("        ,[StyleID]=t.StyleID" + Environment.NewLine);
            sql.Append(" INTO #tmp_summary" + Environment.NewLine);
            sql.Append(" FROM Orders od" + Environment.NewLine);
            sql.Append(" INNER JOIN Order_TmsCost odm ON od.id = odm.ID " + Environment.NewLine + Environment.NewLine);
            sql.Append(" INNER JOIN (SELECT DISTINCT POID,ArtworkTypeID ,BrandID ,StyleID FROM #tmp_AllOrders) t  " + Environment.NewLine + Environment.NewLine);
            sql.Append("            ON t.POID=od.POID AND t.ArtworkTypeID  = odm.ArtworkTypeID" + Environment.NewLine);
            sql.Append(" GROUP BY  odm.ArtworkTypeID,t.poid ,t.BrandID ,t.StyleID" + Environment.NewLine);
            sql.Append(" " + Environment.NewLine);
            sql.Append(" SELECT [StdPrice]=Order_Amt/ Order_Qty ,POID ,ArtworkTypeID ,BrandID ,StyleID FROM #tmp_summary WHERE Order_Qty!=0" + Environment.NewLine);
            sql.Append(" DROP TABLE #tmp_AllOrders,#tmp_summary" + Environment.NewLine);

            result = DBProxy.Current.Select(null, sql.ToString(), parameters, out StdPrice_Dt);
            sql.Clear();
            #endregion

            if (!result)
            {
                ShowErr(sql.ToString(), result);
                return;
            }
            if (StdPrice_Dt.Rows.Count > 0)
            {
                foreach (DataRow row in StdPrice_Dt.Rows)
                {
                    //用來準備填入 C 最新的 " 價格異常紀錄" IPR資料
                    StdPrice = Convert.ToDecimal(row["StdPrice"]);
                    poid = Convert.ToString(row["Poid"]);
                    artworkType = Convert.ToString(row["ArtworkTypeID"]);
                    BrandID = Convert.ToString(row["BrandID"]);
                    StyleID = Convert.ToString(row["StyleID"]);

                    //  2.採購價 =
                    // 如果ArtworkType = EMBROIDERY : （繡花物料成本 + 外發成本）/已建立採購的子單總訂單數量
                    // 不是EMBROIDERY :  外發成本/已建立採購的子單總訂單數量
                    decimal purchasePrice = 0;

                    #region 外發成本

                    //外發成本
                    //找出 所有狀態 = Approved 的 PO (扣除自己)
                    sql.Append(" SELECT DISTINCT [POID]= Others.POID, [ArtworkTypeID]= a.ArtworkTypeID" + Environment.NewLine);
                    sql.Append(" INTO #others" + Environment.NewLine);
                    sql.Append(" FROM ArtworkPO a" + Environment.NewLine);
                    sql.Append(" INNER JOIN ArtworkPO_Detail ad ON a.ID=ad.ID" + Environment.NewLine);
                    sql.Append(" OUTER APPLY(" + Environment.NewLine);
                    sql.Append(" 	SELECT [POID]=orders.POID " + Environment.NewLine);
                    sql.Append(" 	FROM orders WITH (NOLOCK) WHERE id=ad.OrderId" + Environment.NewLine);
                    sql.Append(" )Others" + Environment.NewLine);
                    sql.Append($" WHERE a.ID !=@artWorkPO_ID AND Status='Approved' AND Others.POID='{poid}'" + Environment.NewLine);
                    sql.Append(" " + Environment.NewLine);
                    
                    sql.Append(" SELECT [Amount]=ISNULL(SUM(summary.Amount),0.00) " + Environment.NewLine);
                    sql.Append("        --,[Qty]=ISNULL(SUM(summary.PoQty),0) " + Environment.NewLine);
                    sql.Append(" FROM( " + Environment.NewLine);
                    //與自己同POID、ArtworkType、Status='Approved'的「其他」採購單
                    sql.Append("       SELECT DISTINCT " + Environment.NewLine);
                    sql.Append("       a.CurrencyId, ad.Farmout, " + Environment.NewLine);
                    sql.Append("       ad.Price, " + Environment.NewLine);
                    sql.Append("       ad.PoQty, " + Environment.NewLine);
                    sql.Append("       ad.PoQty * ad.Price * dbo.getRate('FX', a.CurrencyID, 'USD', a.IssueDate) As Amount, " + Environment.NewLine);
                    sql.Append("       dbo.getRate('FX', a.CurrencyID, 'USD', a.IssueDate) As Rate " + Environment.NewLine);
                    sql.Append("       FROM ArtworkPO a " + Environment.NewLine);
                    sql.Append("       INNER JOIN ArtworkPO_Detail ad ON a.ID = ad.ID " + Environment.NewLine);
                    sql.Append("       INNER JOIN Orders o ON o.ID = ad.OrderID " + Environment.NewLine);
                    sql.Append("       INNER JOIN  #others t3 ON t3.POID=o.POID AND t3.ArtworkTypeID=ad.ArtworkTypeID " + Environment.NewLine);
                    sql.Append("       WHERE  a.ID != @artWorkPO_ID " + Environment.NewLine);
                    sql.Append("       UNION ALL " + Environment.NewLine);
                    //自己
                    sql.Append("       SELECT DISTINCT " + Environment.NewLine);
                    sql.Append("       a.CurrencyId, ad.Farmout, " + Environment.NewLine);
                    sql.Append("       ad.Price, " + Environment.NewLine);
                    sql.Append("       ad.PoQty, " + Environment.NewLine);
                    sql.Append("       ad.PoQty * ad.Price * dbo.getRate('FX', a.CurrencyID, 'USD', a.IssueDate) As Amount, " + Environment.NewLine);
                    sql.Append("       dbo.getRate('FX', a.CurrencyID, 'USD', a.IssueDate) As Rate " + Environment.NewLine);
                    sql.Append("       FROM ArtworkPO a " + Environment.NewLine);
                    sql.Append("       INNER JOIN ArtworkPO_Detail ad ON a.ID = ad.ID " + Environment.NewLine);
                    sql.Append("       INNER JOIN Orders o ON o.ID = ad.OrderID " + Environment.NewLine);
                    sql.Append("       WHERE  a.ID = @artWorkPO_ID " + Environment.NewLine);

                    sql.Append(" )summary " + Environment.NewLine);
                    sql.Append(" DROP TABLE #others " + Environment.NewLine);

                    decimal OutPrice = Convert.ToDecimal(MyUtility.GetValue.Lookup(sql.ToString(), parameters));

                    sql.Clear();
                    #endregion

                    #region 已建立採購的子單總訂單數量
                    // 已建立採購的子單總訂單數量
                    sql.Append(" SELECT DISTINCT[ArtworkTypeID] = ad.ArtworkTypeID,[POID] = o.POID" + Environment.NewLine);
                    sql.Append(" INTO #tmp_AllOrders FROM ArtworkPO a" + Environment.NewLine);
                    sql.Append(" INNER JOIN ArtworkPO_Detail ad ON a.ID = ad.ID INNER JOIN Orders o ON o.ID = ad.OrderID" + Environment.NewLine);
                    sql.Append($" WHERE a.ID = @artWorkPO_ID AND o.POID='{poid}'" + Environment.NewLine);
                    sql.Append(" " + Environment.NewLine);
                    sql.Append(" SELECT iif(odm.ArtworkTypeID is null,null,isnull(sum(od.qty),0)) Order_Qty" + Environment.NewLine);
                    sql.Append(" FROM Orders od INNER JOIN Order_TmsCost odm ON od.id = odm.ID " + Environment.NewLine);
                    sql.Append(" INNER JOIN (select distinct POID,ArtworkTypeID from #tmp_AllOrders) t ON t.POID=od.POID AND t.ArtworkTypeID  = odm.ArtworkTypeID " + Environment.NewLine);
                    sql.Append(" GROUP BY  odm.ArtworkTypeID,t.poid" + Environment.NewLine);
                    sql.Append(" DROP TABLE #tmp_AllOrders" + Environment.NewLine);

                    decimal Qty = Convert.ToDecimal(MyUtility.GetValue.Lookup(sql.ToString(), parameters));
                    sql.Clear();
                    #endregion

                    //如果ArtworkType是繡花（ArtworkTypeID = Embroidery ），要加上繡花物料成本
                    if (artworkType.ToUpper() == "EMBROIDERY")
                    {
                        #region 繡花物料成本
                        //搜尋所有PO
                        sql.Append(" SELECT DISTINCT [POID]=SamePoidOrders.POID, [ArtworkTypeID]=a.ArtworkTypeID" + Environment.NewLine);
                        sql.Append(" INTO #tmp_AllPo" + Environment.NewLine);
                        sql.Append(" FROM ArtworkPO a" + Environment.NewLine);
                        sql.Append(" INNER JOIN ArtworkPO_Detail ad ON a.ID = ad.ID" + Environment.NewLine);
                        sql.Append(" OUTER APPLY(" + Environment.NewLine);
                        sql.Append("             SELECT[POID] = orders.POID" + Environment.NewLine);
                        sql.Append("             FROM orders WITH(NOLOCK) WHERE id = ad.OrderId" + Environment.NewLine);
                        sql.Append(" )SamePoidOrders" + Environment.NewLine);
                        sql.Append($" WHERE  a.ArtworkTypeID = '{artworkType}' AND a.ID = @artWorkPO_ID AND SamePoidOrders.POID='{poid}'" + Environment.NewLine);
                        sql.Append(" " + Environment.NewLine);
                        sql.Append(" SELECT [LocalPo_Amt]=ISNULL(SUM(Embroidery.localap_amt),0.00) " + Environment.NewLine);
                        sql.Append("        --,[LocalPo_Qty]= ISNULL(SUM(Embroidery.localap_qty),0) " + Environment.NewLine);
                        sql.Append(" FROM (" + Environment.NewLine);
                        sql.Append("    SELECT" + Environment.NewLine);
                        sql.Append("            l.currencyid," + Environment.NewLine);
                        sql.Append("            ld.Price," + Environment.NewLine);
                        sql.Append("            ld.Qty localap_qty," + Environment.NewLine);
                        sql.Append("            ld.Qty * ld.Price * dbo.getRate('FX', l.CurrencyID, 'USD', l.issuedate) localap_amt," + Environment.NewLine);
                        sql.Append("            dbo.getRate('FX', l.CurrencyID, 'USD', l.issuedate) rate" + Environment.NewLine);

                        sql.Append("            FROM LocalAP l WITH(NOLOCK)" + Environment.NewLine);
                        sql.Append("            INNER JOIN Localap_Detail ld WITH(NOLOCK) on ld.id = l.Id" + Environment.NewLine);
                        sql.Append("            INNER JOIN Orders o WITH(NOLOCK) ON o.id = ld.orderid" + Environment.NewLine);
                        sql.Append("            INNER JOIN #tmp_AllPo allPo ON allPo.POID=o.POID" + Environment.NewLine);
                        sql.Append("            WHERE l.Category = 'EMB_THREAD'" + Environment.NewLine);
                        sql.Append(" ) Embroidery" + Environment.NewLine);
                        sql.Append(" DROP TABLE #tmp_AllPo" + Environment.NewLine);

                        decimal EmbroideryPrice = Convert.ToDecimal(MyUtility.GetValue.Lookup(sql.ToString(), parameters));

                        sql.Clear();
                        #endregion
                        //加上繡花物料成本
                        purchasePrice = (EmbroideryPrice + OutPrice) / Qty;
                    }
                    else
                    {
                        //不用加上繡花物料成本
                        purchasePrice = OutPrice / Qty;
                    }

                    //只要有異常就顯示紅色
                    if (purchasePrice > StdPrice)
                    {
                        this.btnIrrPriceReason.ForeColor = Color.Red;
                    }
                    else
                    {
                        this.btnIrrPriceReason.ForeColor = Color.Black;
                    }


                    #region 填入C 最新的 " 價格異常紀錄" IPR資料
                    if (purchasePrice > StdPrice)
                    {
                        //DataTable tmp;

                        //DBProxy.Current.Select(null, $"SELECT * FROM ArtworkPO_IrregularPrice WHERE POID='{poid}' AND ArtworkTypeID='{artworkType}'", out tmp);
                        DataRow ndr = IrregularPriceReason_Newst.NewRow();

                        ndr["poid"] = poid;
                        ndr["ArtworkTypeID"] = artworkType;
                        ndr["BrandID"] = BrandID;
                        ndr["StyleID"] = StyleID;
                        ndr["StdPrice"] = StdPrice;
                        ndr["PoPrice"] = purchasePrice;
                        ndr["SubconReasonID"] = "";
                        ndr["AddDate"] =  DBNull.Value;
                        ndr["AddName"] = "";
                        ndr["EditDate"] =  DBNull.Value;
                        ndr["EditName"] = "";

                        IrregularPriceReason_Newst.Rows.Add(ndr);
                    }
                    #endregion
                }

                #region 準備 A: DB現有的 "價格異常紀錄" IPR資料

                sql.Clear();
                sql.Append(" SELECT DISTINCT al.*" + Environment.NewLine);

                sql.Append(" FROM ArtWorkPO a" + Environment.NewLine);
                sql.Append(" INNER JOIN ArtworkPO_Detail ad ON a.ID = ad.ID" + Environment.NewLine);
                sql.Append(" INNER JOIN Orders o ON ad.OrderID = o.ID" + Environment.NewLine);
                sql.Append(" INNER JOIN ArtworkPO_IrregularPrice al ON al.POID = o.POID AND al.ArtworkTypeID = ad.ArtworkTypeID" + Environment.NewLine);
                sql.Append(" INNER JOIN SubconReason sr ON sr.Type = 'IP' AND sr.ID = al.SubconReasonID" + Environment.NewLine);
                sql.Append(" WHERE a.ID = @artWorkPO_ID AND sr.Junk=0" + Environment.NewLine);

                result = DBProxy.Current.Select(null, sql.ToString(), parameters, out IrregularPriceReason_InDB);
                #endregion
                
                #region 準備B not exists A ，兩者的差

                sql.Clear();

                //先找出這個加工單底下，所有單號的POID(母單)，以及其加工類別
                sql.Append(" SELECT DISTINCT [ArtworkTypeID]=ad.ArtworkTypeID,  [POID]=o.POID" + Environment.NewLine);
                sql.Append(" INTO #tmp_AllOrders" + Environment.NewLine);
                sql.Append("    FROM ArtworkPO a" + Environment.NewLine);
                sql.Append("    INNER JOIN ArtworkPO_Detail ad ON a.ID=ad.ID" + Environment.NewLine);
                sql.Append("    INNER JOIN Orders o ON o.ID=ad.OrderID" + Environment.NewLine);
                sql.Append("    WHERE a.ID=@artWorkPO_ID" + Environment.NewLine);
                sql.Append(" " + Environment.NewLine);

                //用上述資訊，與Orders、Order_TmsCost關聯，取得所有「有設定標準價」的子單數量總和、個別子單標準價* 個別子單數量
                //標準價 = SUM(子單數量 * 子單標準價) / SUM(子單數量)
                sql.Append(" SELECT " + Environment.NewLine + Environment.NewLine);
                sql.Append("         [Order_Qty]= IIF(odm.ArtworkTypeID IS NULL,NULL,ISNULL(SUM(od.qty),0)) " + Environment.NewLine);
                sql.Append("        ,[Order_Amt]= SUM(od.qty *Price) " + Environment.NewLine);
                sql.Append("        ,[POID]= t.POID" + Environment.NewLine);
                sql.Append("        ,[ArtworkTypeID]= odm.ArtworkTypeID" + Environment.NewLine);
                sql.Append(" INTO #tmp_summary" + Environment.NewLine);
                sql.Append(" FROM Orders od" + Environment.NewLine);
                sql.Append(" INNER JOIN Order_TmsCost odm ON od.id = odm.ID " + Environment.NewLine + Environment.NewLine);
                sql.Append(" INNER JOIN (SELECT DISTINCT POID,ArtworkTypeID FROM #tmp_AllOrders) t  " + Environment.NewLine + Environment.NewLine);
                sql.Append("            ON t.POID=od.POID AND t.ArtworkTypeID  = odm.ArtworkTypeID" + Environment.NewLine);
                sql.Append(" GROUP BY  odm.ArtworkTypeID,t.poid" + Environment.NewLine);
                sql.Append(" " + Environment.NewLine);
                sql.Append(" SELECT [StdPrice]=Order_Amt/ Order_Qty ,POID ,ArtworkTypeID FROM #tmp_summary WHERE Order_Qty!=0 AND not Exists(" + Environment.NewLine);

                sql.Append(" SELECT DISTINCT al.POID,al.ArtworkTypeID,al.POPrice,al.StandardPrice" + Environment.NewLine);
                sql.Append(" FROM ArtWorkPO a" + Environment.NewLine);
                sql.Append(" INNER JOIN ArtworkPO_Detail ad ON a.ID = ad.ID" + Environment.NewLine);
                sql.Append(" INNER JOIN Orders o ON ad.OrderID = o.ID" + Environment.NewLine);
                sql.Append(" INNER JOIN ArtworkPO_IrregularPrice al ON al.POID = o.POID AND al.ArtworkTypeID = ad.ArtworkTypeID" + Environment.NewLine);
                sql.Append(" INNER JOIN SubconReason sr ON sr.Type = 'IP' AND sr.ID = al.SubconReasonID" + Environment.NewLine);
                sql.Append(" WHERE a.ID = @artWorkPO_ID AND sr.Junk=0" + Environment.NewLine);

                sql.Append(" )" + Environment.NewLine);
                //最新的價格異常紀錄，DB還沒有的
                result = DBProxy.Current.Select(null, sql.ToString(), parameters, out Form_Except_IPR);
                if (!result)
                {
                    ShowErr(sql.ToString(), result);
                    return;
                }

                #endregion

                DataTable SubconReason;
                DBProxy.Current.Select(null, "SELECT * FROM SubconReason WHERE Type='IP' AND Junk=0", out SubconReason);

                #region 資料串接

                var summary = (from a in IrregularPriceReason_InDB.AsEnumerable()
                          join c in IrregularPriceReason_Newst.AsEnumerable() on new { POID = a.Field<string>("POID"), ArtWorkType = a.Field<string>("ArtworkTypeID") } equals new { POID = c.Field<string>("POID"), ArtWorkType = c.Field<string>("ArtworkTypeID") }
                          select new
                          {
                              POID = a.Field<string>("POID"),
                              Type = a.Field<string>("ArtworkTypeID"),
                              BrandID= c.Field<string>("BrandID"),
                              StyleID = c.Field<string>("StyleID"),
                              PoPrice = (string.IsNullOrEmpty(c.Field<string>("POID")) ? a.Field<decimal>("POPrice") : c.Field<decimal>("POPrice")),
                              StdPrice = (string.IsNullOrEmpty(c.Field<string>("POID")) ? a.Field<decimal>("StandardPrice") : c.Field<decimal>("StdPrice")),
                              SubconReasonID = c.Field<string>("SubconReasonID"),
                              AddDate=c.Field<DateTime?>("AddDate"),
                              AddName = c.Field<string>("AddName"),
                              EditDate = c.Field<DateTime?>("EditDate"),
                              EditName = c.Field<string>("EditName")

                          }).Union
                         (from b in Form_Except_IPR.AsEnumerable()
                          join c in IrregularPriceReason_Newst.AsEnumerable() on new { POID = b.Field<string>("POID"), ArtWorkType = b.Field<string>("ArtworkTypeID") } equals new { POID = c.Field<string>("POID"), ArtWorkType = c.Field<string>("ArtworkTypeID") }
                          select new
                          {
                              POID = c.Field<string>("POID"),
                              Type = c.Field<string>("ArtworkTypeID"),
                              BrandID = c.Field<string>("BrandID"),
                              StyleID = c.Field<string>("StyleID"),
                              PoPrice = c.Field<decimal>("PoPrice"),
                              StdPrice = c.Field<decimal>("StdPrice"),
                              SubconReasonID = c.Field<string>("SubconReasonID"),
                              AddDate = c.Field<DateTime?>("AddDate"),
                              AddName = c.Field<string>("AddName"),
                              EditDate = c.Field<DateTime?>("EditDate"),
                              EditName = c.Field<string>("EditName")
                          });


                var newest_IPR = from a in summary
                          join s in SubconReason.AsEnumerable() on a.SubconReasonID equals s.Field<string>("ID") into sr
                          from s in sr.DefaultIfEmpty()
                          select new
                          {
                                Factory = this.CurrentMaintain["FactoryID"].ToString(),
                                a.POID,
                                a.Type,
                                a.StyleID,
                                a.BrandID,
                                a.PoPrice,
                                a.StdPrice,
                                a.SubconReasonID,
                                Responsible = MyUtility.Check.Empty(s) ? "" : s.Field<string>("Responsible"),
                                Reason = MyUtility.Check.Empty(s) ? "" : s.Field<string>("Reason"),
                                a.AddDate,
                                a.AddName,
                                a.EditDate,
                                a.EditName
                          };

                #endregion

                #region 還原成 DataTable

                DataTable tbl = new DataTable();
                PropertyInfo[] props = null;

                foreach (var item in newest_IPR)
                {
                    if (props == null)
                    {
                        Type t = item.GetType();
                        props = t.GetProperties();
                        foreach (PropertyInfo pi in props)
                        {
                            Type colType = pi.PropertyType;
                            //針對Nullable<>特別處理
                            if (colType.IsGenericType && colType.GetGenericTypeDefinition() == typeof(Nullable<>))
                            {
                                colType = colType.GetGenericArguments()[0];
                            }
                            //建立欄位
                            tbl.Columns.Add(pi.Name, colType);
                        }
                    }
                    DataRow row = tbl.NewRow();
                    foreach (PropertyInfo pi in props)
                    {
                        row[pi.Name] = pi.GetValue(item, null) ?? DBNull.Value;
                    }
                    tbl.Rows.Add(row);
                }

                #endregion

                //「曾經」有過價格異常紀錄，現在價格正常
                if (IrregularPriceReason_InDB.Rows.Count>0 && tbl.Rows.Count == 0)
                {
                    IrregularPriceReasonType = 3;
                    //Irregular Price Reason放DB紀錄
                }
                //「曾經」有過價格異常紀錄，現在還是異常
                else if (IrregularPriceReason_InDB.Rows.Count > 0 && IrregularPriceReason_Newst.Rows.Count > 0)
                {
                    IrregularPriceReasonType = 2;
                    //_IrregularPriceReasonDT = tbl;
                    //Irregular Price Reason放DB紀錄
                }
                else if(IrregularPriceReason_InDB.Rows.Count==0 && tbl.Rows.Count>0)
                {
                    //「未曾有過」價格異常紀錄，現在價格異常 Type=1
                    IrregularPriceReasonType = 1;
                    _IrregularPriceReasonDT = tbl;
                }

            }

        }

    }

}
