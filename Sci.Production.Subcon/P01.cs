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



namespace Sci.Production.Subcon
{
    public partial class P01 : Sci.Win.Tems.Input6
    {

        public P01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.DefaultFilter = "FactoryID = '" + Sci.Env.User.Factory + "' and POTYPE='O'";
            gridicon.Append.Enabled = false;
            gridicon.Append.Visible = false;

        }

        // 新增時預設資料
        protected override void OnNewAfter()
        {
            base.OnNewAfter();
            CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
            CurrentMaintain["ISSUEDATE"] = System.DateTime.Today;
            CurrentMaintain["TYPE"] = "O";
            CurrentMaintain["HANDLE"] = Sci.Env.User.UserID;
        }

        // edit前檢查
        protected override bool OnEditBefore()
        {
            //!EMPTY(APVName) OR !EMPTY(Closed)，只能編輯remark欄。
            //DO FORM PFPP024

            return base.OnEditBefore();
        }

        // save前檢查 & 取id
        protected override bool OnSaveBefore()
        {
            //取單號： getID(MyApp.cKeyword+GetDocno('PMS', 'ARTWORKPO1'), 'ARTWORKPO', IssueDate, 2)

            //nExact: CURRENCY.EXACT
            //REPLACE POAmount WITH ROUND(明細amount加總, nExact)
            //REPLACE Vat WITH ROUND(VatRate * POAmount / 100, nExact)

            //明細需判斷UKEY為空，則GETUKEY()。
            //明細Artworktype需填入表頭的artworktype

            return base.OnSaveBefore();
        }

        // grid 加工填值
        protected override DualResult OnRenewDataPost(RenewDataPostEventArgs e)
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
                    if (order_dt.Rows.Count == 0) break;
                    dr["style"] = order_dt.Rows[0]["styleid"].ToString();
                    dr["sewinline"] = order_dt.Rows[0]["sewinline"];
                    dr["scidelivery"] = order_dt.Rows[0]["scidelivery"];
                    
                }
            }
                return base.OnRenewDataPost(e);
        }

        //refresh
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            decimal amount = (decimal)CurrentMaintain["amount"] + (decimal)CurrentMaintain["vat"];
            numericBox4.Text = amount.ToString();
            
            if (CurrentMaintain["Closed"].ToString().ToUpper() == "TRUE")
            {
                label25.Text = "Closed";
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(CurrentMaintain["apvname"].ToString()))
                {
                    label25.Text = "Approved";
                }
                else
                {
                    label25.Text = "Not Approve";
                }
            }

            label17.Visible = CurrentMaintain["Exceed"].ToString().ToUpper() == "TRUE";

//            [Refresh]
//	Enabled: 編輯模式下不可使用。
//	Closed = "J"不可使用。
//	沒有Confirm權限不可使用。
//	當明細項已有存在apqty時，不可unapprove。

//	Caption: empty(ApvName) show "Approve"
//!empty(ApvName) show "Unapprove"
//	Fore color: 若!EMPTY(ApvName).則顯示RGB(0, 0, 255)
           
            DataTable dt = (DataTable)detailgridbs.DataSource;
            DataRow[] dr = dt.Select("apqty > 0");

            
            button1.Enabled = !this.EditMode &&
                            !(CurrentMaintain["closed"].ToString() == "1") &&
                            this.IsSupportConfirm &&
                            dr.Length == 0;

            if (string.IsNullOrWhiteSpace(CurrentMaintain["apvname"].ToString()))
            {
                button1.Text = "Approve";
                button1.ForeColor = Color.Black;
            }
            else
            {
                button1.Text = "Unapprove";
                button1.ForeColor = Color.Blue;
            }

            //ApvName = MyApp.cLogin OR getauthority(MyApp.cLogin)) AND !Thisform.LEditmode
            button2.Enabled = !this.EditMode && 
                                (Prgs.GetAuthority(Env.User.UserID) || 
                                CurrentMaintain["apvname"].ToString() == Env.User.UserID);
            if (CurrentMaintain["closed"].ToString()=="1")
            {
                button2.Text = "Recall";
            }
            else
            {
                button2.Text = "Close";
            }
            
        }

        // Detail Grid 設定
        protected override void OnDetailGridSetup()
        {
            #region SP#右鍵開窗
            Ict.Win.DataGridViewGeneratorTextColumnSettings ts4 = new DataGridViewGeneratorTextColumnSettings();
            ts4.EditingMouseDown += (s, e) =>
            {
                if (!this.EditMode && !string.IsNullOrWhiteSpace(CurrentMaintain["ApvName"].ToString()) && CurrentMaintain["closed"].ToString().ToUpper() == "FALSE" && e.Button == MouseButtons.Right)
                {
                    
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


            Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("orderid", header: "SP#", width: Widths.AnsiChars(13), settings: ts4)
            .Text("Style", header: "Style", width: Widths.AnsiChars(15))
            .Numeric("PoQty", header: "PO Qty", width: Widths.AnsiChars(6))
            .Date("sewinline", header: "SewInLine", width: Widths.AnsiChars(10))
            .Date("scidelivery", header: "SciDelivery", width: Widths.AnsiChars(10))
            .Text("artwork", header: "Artwork", width: Widths.AnsiChars(5))
            .Numeric("custstictch", header: "Cost(PCS/Stitch)", width: Widths.AnsiChars(5))
            .Numeric("stitch", header: "PCS/Stitch", width: Widths.AnsiChars(5))
            .Text("patterncode", header: "CutpartID", width: Widths.AnsiChars(5))
            .Text("PatternDesc", header: "Cutpart Name", width: Widths.AnsiChars(5))
            .Numeric("unitprice", header: "Unit Price", width: Widths.AnsiChars(5))
            .Numeric("cost", header: "Cost(USD)", width: Widths.AnsiChars(5))
            .Numeric("qtygarment", header: "Qty/GMT", width: Widths.AnsiChars(5))
            .Text("PatternDesc", header: "Price/GMT", width: Widths.AnsiChars(5))
            .Numeric("", header: "Price/GMT", width: Widths.AnsiChars(5))
            .Numeric("amount", header: "Amount", width: Widths.AnsiChars(5))
            .Text("farmout", header: "Farm Out", width: Widths.AnsiChars(5), settings: ts)
            .Text("farmin", header: "Farm In", width: Widths.AnsiChars(5), settings: ts2)
            .Text("apqty", header: "A/P Qty", width: Widths.AnsiChars(5), settings: ts3)
            .Text("exceed", header: "Exceed", width: Widths.AnsiChars(5));

            detailgrid.Columns[7].DefaultCellStyle.BackColor = Color.Pink;
            detailgrid.Columns[10].DefaultCellStyle.BackColor = Color.Pink;
            
        }

        //Approve
        private void button1_Click(object sender, EventArgs e)
        {
            String sqlcmd;
            if (string.IsNullOrWhiteSpace(CurrentMaintain["apvname"].ToString()))
            {
                sqlcmd = string.Format("update artworkpo set apvname='{0}', apvdate = GETDATE() , editname = '{0}' , editdate = GETDATE() " +
                                "where id = '{1}'", Env.User.UserID, CurrentMaintain["id"]);
            }
            else
            {
                sqlcmd = string.Format("update artworkpo set apvname='', apvdate = null , editname = '{0}' , editdate = GETDATE() " +
                                "where id = '{1}'", Env.User.UserID, CurrentMaintain["id"]);
            }
            DualResult result;
            if(!(result = DBProxy.Current.Execute(null,sqlcmd)))
            {
                ShowErr(sqlcmd,result);
                return;
            }
            RenewData();
            OnDetailEntered();
            
        }

        
        //Cloee
        private void button2_Click(object sender, EventArgs e)
        {
            String sqlcmd;
            if (string.IsNullOrWhiteSpace(CurrentMaintain["apvname"].ToString()))
            {
                sqlcmd = string.Format("update artworkpo set apvname='{0}', apvdate = GETDATE() , editname = '{0}' , editdate = GETDATE() " +
                                "where id = '{1}'", Env.User.UserID, CurrentMaintain["id"]);
            }
            else
            {
                sqlcmd = string.Format("update artworkpo set apvname='', apvdate = null , editname = '{0}' , editdate = GETDATE() " +
                                "where id = '{1}'", Env.User.UserID, CurrentMaintain["id"]);
            }
            DualResult result;
            if (!(result = DBProxy.Current.Execute(null, sqlcmd)))
            {
                ShowErr(sqlcmd, result);
                return;
            }
            RenewData();
            OnDetailEntered();
        }

    }
}
