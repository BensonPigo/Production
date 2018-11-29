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
        bool useDBdata = false;
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
            var frm = new Sci.Production.Subcon.P01_IrregularPriceReason(_IrregularPriceReasonDT, this.CurrentMaintain["ID"].ToString(), this.CurrentMaintain["FactoryID"].ToString(), useDBdata);
            frm.ShowDialog(this);
            Check_Irregular_Price();
        }

        private void Check_Irregular_Price()
        {
            //重置
            _IrregularPriceReasonDT = null;

            #region 變數宣告

            bool Has_Irregular_Price = false;
            string poid = string.Empty;
            string artworkType = string.Empty;
            string BrandID = string.Empty;
            string StyleID = string.Empty;
            string artWorkPO_ID = string.Empty;
            List<SqlParameter> parameters = new List<SqlParameter>();
            StringBuilder sql = new StringBuilder();
            DualResult result;

            DataTable IrregularPriceReason_InDB;//A DB現有的 "價格異常紀錄" IPR資料
            DataTable Price_Dt;// 紀錄所有價格的Table
            DataTable IrregularPriceReason_Real = new DataTable();//C "當下實際存在的價格異常紀錄" IPR資料，無論DB有無
            DataTable IrregularPriceReason_New;//B 最新的價格異常紀錄，DB沒有
                                               //DataTable IrregularPriceReason_Repeat;//D C和A重複的資料
            #endregion

            #region 欄位定義
            IrregularPriceReason_Real.Columns.Add(new DataColumn("ArtworkTypeID", System.Type.GetType("System.String")));
            IrregularPriceReason_Real.Columns.Add(new DataColumn("POID", System.Type.GetType("System.String")));
            IrregularPriceReason_Real.Columns.Add(new DataColumn("StyleID", System.Type.GetType("System.String")));
            IrregularPriceReason_Real.Columns.Add(new DataColumn("BrandID", System.Type.GetType("System.String")));
            IrregularPriceReason_Real.Columns.Add(new DataColumn("PoPrice", System.Type.GetType("System.Decimal")));
            IrregularPriceReason_Real.Columns.Add(new DataColumn("StdPrice", System.Type.GetType("System.Decimal")));
            IrregularPriceReason_Real.Columns.Add(new DataColumn("SubconReasonID", System.Type.GetType("System.String")));
            IrregularPriceReason_Real.Columns.Add(new DataColumn("AddDate", System.Type.GetType("System.DateTime")));
            IrregularPriceReason_Real.Columns.Add(new DataColumn("AddName", System.Type.GetType("System.String")));
            IrregularPriceReason_Real.Columns.Add(new DataColumn("EditDate", System.Type.GetType("System.DateTime")));
            IrregularPriceReason_Real.Columns.Add(new DataColumn("EditName", System.Type.GetType("System.String")));
            #endregion

            try
            {
                //採購價 > 標準價 =異常
         
                artWorkPO_ID = this.CurrentMaintain["ID"].ToString();
                parameters.Add(new SqlParameter("@artWorkPO_ID", artWorkPO_ID));

                #region 查詢所有價格異常紀錄

                #region 何謂標準價&採購價

                /*
                標準價 = (訂單標準價 * 該母單中有設定標準價、且有被採購的訂單數量) / 該母單中有設定標準價、且有被採購的訂單數量

                --非繡花採購價--
                採購價 = 採購價格總和 / 該母單中有設定標準價的訂單數量

                => 採購價格總和 = 採購數量 * 採購單價 (* 匯率)
                    =>必須是：相同母單、加工的採購單
                    =>必須是：相同母單的採購單的訂單，去除沒被採購的訂單
    
                --繡花採購價--

                採購價 = ( 採購價格總和 + 繡花成本總和 ) / 該母單中有設定標準價的訂單數量

                => 採購價格總和 = Sum( 採購數量 * 採購單價 (* 匯率) )
                    =>必須是：「所有」相同母單、加工的採購單
                    =>必須是：上述的採購單的訂單，需去除沒被採購的

                => 繡花成本 = Sum( 採購數量 * 採購單價 (* 匯率) )
                    =>必須是：「所有」相同母單、加工=EMB_Thread（繡線）的採購單
                 */
                #endregion

                sql.Append(@"

--根據表頭LocalPO的ID，整理出ArtworkTypeID、POID、OrderID
SELECT DISTINCT[ArtworkTypeID] = ad.ArtworkTypeID ,[OrderId] = ad.OrderID  ,[POID]=o.POID
INTO #tmp_AllOrders 
FROM ArtworkPO a 
INNER JOIN ArtworkPO_Detail ad ON a.ID=ad.ID
INNER JOIn ORDERS o ON ad.OrderID=o.id
WHERE a.ID = @artWorkPO_ID

--從所有採購單中，找出同ArtworkTypeID、POID，有被採購的OrderID（不限採購單）
SELECT DISTINCT ad.OrderID 
INTO #BePurchased
FROM LocalPO a 
INNER JOIN LocalPO_Detail ad ON a.ID=ad.ID 
INNER JOIn Orders ods ON ad.OrderID=ods.id 
WHERE  ods.POID IN  (SELECT POID FROM  #tmp_AllOrders)

--列出採購價的清單（尚未總和）
SELECT  ap.ID
		,ap.ArtworkTypeID
		,Orders.POID
		,[OID]=apd.OrderId
		,ap.currencyid  --維護時檢查用，所以先註解留著
		,apd.Price
		,apd.PoQty
		,apd.PoQty * apd.Price * dbo.getRate('FX',ap.CurrencyID,'USD',ap.issuedate) PO_amt
		,dbo.getRate('FX',ap.CurrencyID,'USD',ap.issuedate) rate
INTO #total_PO
FROM ArtworkPO ap WITH (NOLOCK) 
INNER JOIN ArtworkPO_Detail apd WITH (NOLOCK) on apd.id = ap.Id 
INNER JOIN  Orders WITH (NOLOCK) on orders.id = apd.orderid
WHERE  EXiSTS  ( 
				SELECT ArtworkTypeID,POID 
				FROM #tmp_AllOrders 
				WHERE ArtworkTypeID= ap.ArtworkTypeID  AND POID=Orders.POID) --相同Category、POID
	   AND apd.OrderId  IN  ( SELECT OrderID FROM #BePurchased ) --且有被採購的OrderID
       --AND ap.Status = 'Approved' 現在不需要過濾狀態

--繡花成本處理：列出同POID、Category=EMB_Thread（繡線）的總額清單
SELECT   LPD.POID
		,LP.currencyid
		, [Price]=LPd.Price  * dbo.getRate('FX', LP.CurrencyID, 'USD', LP.issuedate) --採購單價
		, LPD.Qty localap_qty  --採購數量
		, [LocalPo_amt]=LPD.Price* LPD.Qty  * dbo.getRate('FX', LP.CurrencyID, 'USD', LP.issuedate) --採購總額
		, dbo.getRate('FX', LP.CurrencyID, 'USD', LP.issuedate) rate
INTO #Embroidery_List
FROM LocalPO LP
inner join LocalPO_Detail LPD on LP.Id = LPD.Id
INNER JOIN Orders ON Orders.ID=LPD.OrderId
WHERE LP.Category = 'EMB_Thread'  
	  AND Orders.POID IN  (SELECT POID FROM  #tmp_AllOrders)

--開始整合
SELECT o.BrandID ,o.StyleID  ,t.ArtworkTypeID  ,t.POID 
,[stdPrice]=round(Standard.order_amt/iif(Standard.order_qty=0,1,Standard.order_qty),3) 
,[PoPrice]=round(Po.PO_amt / iif(Standard.order_qty=0,1,Standard.order_qty),3) 
,[PoPriceWithEmbroidery] =IIF(Embroidery.LocalPo_amt IS NULL ,
							round(po.Po_Amt / iif(Standard.order_qty=0,1,Standard.order_qty),3) , 
							round((po.Po_Amt+Embroidery.LocalPo_amt) / iif(Standard.order_qty=0,1,Standard.order_qty)
							,3) )
FROM #tmp_AllOrders t
INNER JOIN Orders o WITH (NOLOCK) on o.id = t.OrderId
INNER JOIN Brand bra on bra.id=o.BrandID
OUTER APPLY(--標準價
	        select orders.POID
	        ,sum(orders.qty) order_qty        --實際外發數量
	        ,sum(orders.qty*Price) order_amt  --外發成本
	        from orders WITH (NOLOCK) 
	        inner join Order_TmsCost WITH (NOLOCK) on Order_TmsCost.id = orders.ID 
	        where POID= t.POID                   --相同母單
			AND ArtworkTypeID= t.ArtworkTypeID   --相同加工
			AND Order_TmsCost.ID  IN ( SELECT OrderID FROM #BePurchased ) --***限定 有被採購的訂單***
	        group by orders.poid,ArtworkTypeID
) Standard
OUTER APPLY (--採購價，根據ArtworkTypeID、POID，作分組加總
	SELECT isnull(sum(Q.PO_amt),0.00) PO_amt
	FROM (
		SELECT PO_amt FROM #total_PO
		WHERE ArtworkTypeID = t.artworktypeid 
		AND POID = t.POID 
	) Q
) Po	
outer apply(--繡花成本，根據POID，作分組加總
	SELECT  x.POID,[LocalPo_amt]=SUM(x.LocalPo_amt) 
	FROM (	
		SELECT * FROM #Embroidery_List
		) x   
	GROUP BY  x.POID
) Embroidery

GROUP BY  o.BrandID ,o.StyleID ,t.ArtworkTypeID ,t.POID ,Standard.order_amt ,Standard.order_qty ,po.Po_Amt ,Standard.order_qty ,Embroidery.LocalPo_amt

DROP TABLE #tmp_AllOrders ,#BePurchased ,#total_PO ,#Embroidery_List


" + Environment.NewLine);

                #endregion

                this.ShowWaitMessage("Data Loading...");
                result = DBProxy.Current.Select(null, sql.ToString(), parameters, out Price_Dt);
                sql.Clear();
                this.HideWaitMessage();

                if (!result)
                {
                    ShowErr(sql.ToString(), result);
                    return;
                }

                if (Price_Dt.Rows.Count > 0)
                {
                    #region 準備 C：當下實際存在的價格異常紀錄
                    foreach (DataRow row in Price_Dt.Rows)
                    {
                        decimal StdPrice = 0;
                        decimal purchasePrice = 0;
                        decimal PoPriceWithEmbroidery = 0;
                        //用來準備填入 C 最新的 " 價格異常紀錄" IPR資料
                        poid = Convert.ToString(MyUtility.Check.Empty(row["Poid"]) ? "" : row["Poid"]);
                        BrandID = Convert.ToString(MyUtility.Check.Empty(row["BrandID"]) ? "" : row["BrandID"]);
                        StyleID = Convert.ToString(MyUtility.Check.Empty(row["StyleID"]) ? "" : row["StyleID"]);
                        artworkType = Convert.ToString(MyUtility.Check.Empty(row["ArtworkTypeID"]) ? "" : row["ArtworkTypeID"]);
                        StdPrice = Convert.ToDecimal(MyUtility.Check.Empty(row["StdPrice"]) ? 0 : row["StdPrice"]);
                        purchasePrice = Convert.ToDecimal(MyUtility.Check.Empty(row["PoPrice"]) ? 0 : row["PoPrice"]);
                        PoPriceWithEmbroidery = Convert.ToDecimal(MyUtility.Check.Empty(row["PoPriceWithEmbroidery"]) ? 0 : row["PoPriceWithEmbroidery"]);


                        //如果ArtworkType是繡花（ArtworkTypeID = Embroidery ），要加上繡花物料成本
                        if (artworkType.ToUpper() == "EMBROIDERY")
                        {
                            if (PoPriceWithEmbroidery > StdPrice & StdPrice > 0)//未設定標準價不算
                            {                                
                                Has_Irregular_Price = true;
                                IrregularPriceReason_Real = CreateIrregularPriceReasonDataTabel(poid, artworkType, BrandID, StyleID, PoPriceWithEmbroidery, StdPrice, IrregularPriceReason_Real);
                            }
                        }
                        else
                        {
                            //不用加上繡花物料成本
                            if (purchasePrice > StdPrice & StdPrice > 0)//未設定標準價不算
                            {
                                Has_Irregular_Price = true;
                                IrregularPriceReason_Real = CreateIrregularPriceReasonDataTabel(poid, artworkType, BrandID, StyleID, purchasePrice, StdPrice, IrregularPriceReason_Real).AsEnumerable().Where(o => (decimal)o["StdPrice"] != 0).CopyToDataTable();
                            }
                        }
                    }

                    #endregion

                    //只要有異常就顯示紅色
                    if (Has_Irregular_Price)
                        this.btnIrrPriceReason.ForeColor = Color.Red;

                    #region 準備 A：DB現有的 "價格異常紀錄"

                    sql.Clear();
                    sql.Append(" SELECT DISTINCT al.POID ,al.ArtworkTypeID ,o.BrandID ,o.StyleID ,al.POPrice, al.StandardPrice ,al.SubconReasonID ,al.AddDate ,al.AddName ,al.EditDate ,al.EditName " + Environment.NewLine);

                    sql.Append(" FROM ArtWorkPO a" + Environment.NewLine);
                    sql.Append(" INNER JOIN ArtworkPO_Detail ad ON a.ID = ad.ID" + Environment.NewLine);
                    sql.Append(" INNER JOIN Orders o ON ad.OrderID = o.ID" + Environment.NewLine);
                    sql.Append(" INNER JOIN ArtworkPO_IrregularPrice al ON al.POID = o.POID AND al.ArtworkTypeID = ad.ArtworkTypeID" + Environment.NewLine);
                    sql.Append(" INNER JOIN SubconReason sr ON sr.Type = 'IP' AND sr.ID = al.SubconReasonID" + Environment.NewLine);
                    sql.Append(" WHERE a.ID = @artWorkPO_ID" + Environment.NewLine);

                    result = DBProxy.Current.Select(null, sql.ToString(), parameters, out IrregularPriceReason_InDB);
                    if (!result)
                    {
                        ShowErr(sql.ToString(), result);
                        return;
                    }
                    IrregularPriceReason_InDB = MyUtility.Check.Empty(IrregularPriceReason_InDB) ? new DataTable() : IrregularPriceReason_InDB;

                    #endregion

                    #region 準備 B：新增的紀錄 = 當下實際存在的價格異常紀錄 - DB現有紀錄

                    var Btable = from c in IrregularPriceReason_Real.AsEnumerable()
                                 where !IrregularPriceReason_InDB.AsEnumerable().Any(o => o["POID"].ToString() == c["POID"].ToString() && o["ArtworkTypeID"].ToString() == c["ArtworkTypeID"].ToString())
                                 select c;

                    IrregularPriceReason_New = Btable.AsEnumerable().Count() > 0 ? Btable.AsEnumerable().CopyToDataTable() : new DataTable();

                    #endregion

                    #region 準備 D：找出實際紀錄 與 DB紀錄 重複的部分

                    //var Rep = from c in IrregularPriceReason_InDB.AsEnumerable()
                    //          where !IrregularPriceReason_Real.AsEnumerable().Any(o => o["POID"].ToString() == c["POID"].ToString() && o["ArtworkTypeID"].ToString() == c["ArtworkTypeID"].ToString())
                    //          select c;

                    //IrregularPriceReason_Repeat = Rep.AsEnumerable().Count() > 0 ? Rep.AsEnumerable().CopyToDataTable() : new DataTable();
                    #endregion

                    DataTable SubconReason;
                    DBProxy.Current.Select(null, "SELECT ID,[ResponsibleID]=Responsible,(select Name from DropDownList d where d.type = 'Pms_PoIr_Responsible' and d.ID = SubconReason.Responsible) as ResponsibleName,Reason  FROM SubconReason WHERE Type='IP'", out SubconReason);

                    #region 資料串接

                    //同一組POID 、 ArtworkTypeID，若DB有資料就帶DB的所有資料；DB沒有就帶上面查詢的所有資料
                    var summary = (from a in IrregularPriceReason_InDB.AsEnumerable()
                                   select new
                                   {
                                       POID = a.Field<string>("POID"),
                                       Type = a.Field<string>("ArtworkTypeID"),
                                       BrandID = a.Field<string>("BrandID"),
                                       StyleID = a.Field<string>("StyleID"),
                                       PoPrice = a.Field<decimal>("POPrice"),// DB有紀錄一律帶DB，無論價格是否一樣
                                       StdPrice = a.Field<decimal>("StandardPrice"),// DB有紀錄一律帶DB，無論價格是否一樣  c.Field<decimal>("StdPrice") 
                                       SubconReasonID = a.Field<string>("SubconReasonID"),
                                       AddDate = a.Field<DateTime?>("AddDate"),
                                       AddName = a.Field<string>("AddName"),
                                       EditDate = a.Field<DateTime?>("EditDate"),
                                       EditName = a.Field<string>("EditName")

                                   }).Union
                             (from b in IrregularPriceReason_New.AsEnumerable()
                              join c in IrregularPriceReason_Real.AsEnumerable() on new { POID = b.Field<string>("POID"), ArtWorkType = b.Field<string>("ArtworkTypeID") } equals new { POID = c.Field<string>("POID"), ArtWorkType = c.Field<string>("ArtworkTypeID") }
                              select new
                              {
                                  POID = b.Field<string>("POID"),
                                  Type = b.Field<string>("ArtworkTypeID"),
                                  BrandID = b.Field<string>("BrandID"),
                                  StyleID = b.Field<string>("StyleID"),
                                  PoPrice = b.Field<decimal>("PoPrice"),
                                  StdPrice = b.Field<decimal>("StdPrice"),
                                  SubconReasonID = b.Field<string>("SubconReasonID"),
                                  AddDate = b.Field<DateTime?>("AddDate"),
                                  AddName = b.Field<string>("AddName"),
                                  EditDate = b.Field<DateTime?>("EditDate"),
                                  EditName = b.Field<string>("EditName")
                              });

                    //串SubconReason 資料表進來，組合成P01_IrregularPrice 裡面Grid的樣子
                    var total_IPR = from a in summary
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
                                        ResponsibleID = MyUtility.Check.Empty(s) ? "" : s.Field<string>("ResponsibleID"),
                                        ResponsibleName = MyUtility.Check.Empty(s) ? "" : s.Field<string>("ResponsibleName"),
                                        Reason = MyUtility.Check.Empty(s) ? "" : s.Field<string>("Reason"),
                                        a.AddDate,
                                        a.AddName,
                                        a.EditDate,
                                        a.EditName
                                    };

                    #endregion

                    #region 還原成 DataTable

                    DataTable IPR_Grid = new DataTable();
                    PropertyInfo[] props = null;

                    foreach (var item in total_IPR)
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
                                IPR_Grid.Columns.Add(pi.Name, colType);
                            }
                        }
                        DataRow row = IPR_Grid.NewRow();
                        foreach (PropertyInfo pi in props)
                        {
                            row[pi.Name] = pi.GetValue(item, null) ?? DBNull.Value;
                        }
                        IPR_Grid.Rows.Add(row);
                    }

                    #endregion
                    /*
                          | DB | New |
                    ----------------------------
                         1| 有 | 無  | 帶DB的 Datatable
                    ----------------------------
                         2| 無 | 有  | 帶DB + New 的 Datatable
                    ----------------------------
                         3| 有 | 有  | 帶DB + New 的 Datatable
                    ----------------------------
                         4| 有 | 無  | 帶DB + New 的 Datatable
                    ----------------------------
                         5| 無 | 無  | 沒事

                     */

                    //「曾經」有過價格異常紀錄，現在價格正常，沒有新的異常紀錄，只需要帶DB資料
                    if (IrregularPriceReason_InDB.AsEnumerable().Count() > 0 && IPR_Grid.Rows.Count == 0)
                    {
                        useDBdata = true;
                    }
                    else
                    {
                        useDBdata = false;
                        _IrregularPriceReasonDT = IPR_Grid;
                    }

                }
            }
            catch (Exception ex)
            {
                ShowErr(ex.Message, ex);
            }
        }

        private DataTable CreateIrregularPriceReasonDataTabel(string POID, string ArtWorkType, string BrandID, string StyleID, decimal purchasePrice, decimal StdPrice, DataTable IrregularPriceReason_New)
        {
            DataRow ndr = IrregularPriceReason_New.NewRow();

            ndr["poid"] = POID;
            ndr["ArtworkTypeID"] = ArtWorkType;
            ndr["BrandID"] = BrandID;
            ndr["StyleID"] = StyleID;
            ndr["StdPrice"] = StdPrice;
            ndr["PoPrice"] = purchasePrice;
            ndr["SubconReasonID"] = "";
            ndr["AddDate"] = DBNull.Value;
            ndr["AddName"] = "";
            ndr["EditDate"] = DBNull.Value;
            ndr["EditName"] = "";

            IrregularPriceReason_New.Rows.Add(ndr);

            return IrregularPriceReason_New;
        }

    }

}
