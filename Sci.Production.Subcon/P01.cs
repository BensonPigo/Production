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
            if (dt.AsEnumerable().Any(r => MyUtility.Convert.GetInt(r["Farmin"]) > 0)||
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
            .Numeric("coststitch", header: "Cost"+ Environment.NewLine+"(PCS/Stitch)", width: Widths.AnsiChars(3), iseditingreadonly: true)//6
            .Numeric("stitch", header: "PCS/Stitch", width: Widths.AnsiChars(3))    //7
            .Text("patterncode", header: "Cutpart"+ Environment.NewLine+"ID", width: Widths.AnsiChars(5), iseditingreadonly: true) //8
            .Text("PatternDesc", header: "Cutpart Name", width: Widths.AnsiChars(15), iseditingreadonly: true)   //9
            .Numeric("unitprice", header: "Unit Price", width: Widths.AnsiChars(5), settings: ns, decimal_places: 4, integer_places: 4)     //10
            .Numeric("cost", header: "Cost"+ Environment.NewLine+"(USD)", width: Widths.AnsiChars(5), iseditingreadonly: true, decimal_places: 4, integer_places: 4)  //11
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
            if (((DataTable)this.detailgridbs.DataSource).Rows.Count==0)
            {
                return;
            }

            string chkp10exists = string.Format(
                @"
select distinct aad.orderid,aad.id
from ArtworkPO_detail apd with(nolock)
inner join ArtworkAP_detail aad with(nolock) on apd.id = aad.artworkpoid and aad.artworkpo_detailukey = apd.ukey
where  apd.id = '{0}' and aad.ukey = '{1}'
",
                CurrentMaintain["id"],CurrentDetailData["Ukey"]);
            DualResult Result;
            DataTable dt;
            if(Result = DBProxy.Current.Select(null, chkp10exists,out dt))
            {
                if (dt.Rows.Count > 1)
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
    }
}
