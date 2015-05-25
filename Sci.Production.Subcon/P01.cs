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
            gridicon.Insert.Enabled = false;
            gridicon.Insert.Visible = false;

        }

        // 新增時預設資料
        protected override void OnNewAfter()
        {
            base.OnNewAfter();
            CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
            CurrentMaintain["ISSUEDATE"] = System.DateTime.Today;
            CurrentMaintain["POType"] = "O";
            CurrentMaintain["HANDLE"] = Sci.Env.User.UserID;
            ((DataTable)(detailgridbs.DataSource)).Rows[0].Delete();
        }

        // edit前檢查
        protected override bool OnEditBefore()
        {
            //!EMPTY(APVName) OR !EMPTY(Closed)，只能編輯remark欄。
            DataRow dr = grid.GetDataRow<DataRow>(grid.GetSelectedRowIndex());
            if (!string.IsNullOrWhiteSpace(dr["apvname"].ToString()) || dr["closed"].ToString().ToUpper() == "TRUE")
            {
                var frm = new Sci.Production.PublicForm.EditRemark("artworkpo","remark",dr);
                frm.ShowDialog(this);
                this.RenewData();
                return false;
            }

            return base.OnEditBefore();
        }

        // save前檢查 & 取id
        protected override bool OnSaveBefore()
        {
            //取單號： getID(MyApp.cKeyword+GetDocno('PMS', 'ARTWORKPO1'), 'ARTWORKPO', IssueDate, 2)
            if (string.IsNullOrWhiteSpace(CurrentMaintain["id"].ToString()))
            {
                //CurrentMaintain["id"] 
            }

            //nExact: CURRENCY.EXACT
            //REPLACE POAmount WITH ROUND(明細amount加總, nExact)
            //REPLACE Vat WITH ROUND(VatRate * POAmount / 100, nExact)
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
                    dr["Price"] = (Decimal)dr["unitprice"] * (Decimal)dr["qtygarment"];
                    DataTable order_dt;
                    DBProxy.Current.Select(null, string.Format("select styleid, sewinline, scidelivery from orders where id='{0}'", dr["orderid"].ToString()), out order_dt);
                    if (order_dt.Rows.Count == 0) 
                        break;
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
            if (!(CurrentMaintain==null))
            {
                if (!(CurrentMaintain["amount"] == DBNull.Value) && !(CurrentMaintain["vat"] == DBNull.Value))
                {
                    decimal amount = (decimal)CurrentMaintain["amount"] + (decimal)CurrentMaintain["vat"];
                    numericBox4.Text = amount.ToString();
                }
            }

            #region Status Label
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
            #endregion
            #region exceed status
            label17.Visible = CurrentMaintain["Exceed"].ToString().ToUpper() == "TRUE";
            #endregion
            #region Approve Button
            DataTable dt = (DataTable)detailgridbs.DataSource;
            DataRow[] dr = dt.Select("apqty > 0");

            
            button1.Enabled = !this.EditMode &&
                            !(CurrentMaintain["closed"].ToString().ToUpper() == "TRUE") &&
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
            #endregion
            #region Close button
            //ApvName = MyApp.cLogin OR getauthority(MyApp.cLogin)) AND !Thisform.LEditmode
            button2.Enabled = !this.EditMode && 
                                (Prgs.GetAuthority(Env.User.UserID) || 
                                CurrentMaintain["apvname"].ToString() == Env.User.UserID);
            if (CurrentMaintain["closed"].ToString().ToUpper()=="TRUE")
            {
                button2.Text = "Recall";
            }
            else
            {
                button2.Text = "Close";
            }
            #endregion
            #region Batch Import, Special record button
            button4.Enabled = this.EditMode;
            button5.Enabled = this.EditMode;
            #endregion
            #region
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
            .Text("orderid", header: "SP#", width: Widths.AnsiChars(13),iseditingreadonly:true, settings: ts4)  //0
            .Text("Style", header: "Style", width: Widths.AnsiChars(15), iseditingreadonly: true)   //1
            .Numeric("PoQty", header: "PO Qty", width: Widths.AnsiChars(6), iseditingreadonly: true)    //2
            .Date("sewinline", header: "SewInLine", width: Widths.AnsiChars(10), iseditingreadonly: true)   //3
            .Date("scidelivery", header: "SciDelivery", width: Widths.AnsiChars(10), iseditingreadonly: true)   //4
            .Text("ArtworkId", header: "Artwork", width: Widths.AnsiChars(8), iseditingreadonly: true)    //5
            .Numeric("coststitch", header: "Cost(PCS/Stitch)", width: Widths.AnsiChars(5), iseditingreadonly: true)//6
            .Numeric("stitch", header: "PCS/Stitch", width: Widths.AnsiChars(5))    //7
            .Text("patterncode", header: "CutpartID", width: Widths.AnsiChars(10), iseditingreadonly: true) //8
            .Text("PatternDesc", header: "Cutpart Name", width: Widths.AnsiChars(15))   //9
            .Numeric("unitprice", header: "Unit Price", width: Widths.AnsiChars(5),settings:ns)     //10
            .Numeric("cost", header: "Cost(USD)", width: Widths.AnsiChars(5), iseditingreadonly: true)  //11
            .Numeric("qtygarment", header: "Qty/GMT", width: Widths.AnsiChars(5),settings:ns2)  //12
            .Numeric("Price", header: "Price/GMT", width: Widths.AnsiChars(5), iseditingreadonly: true)   //13
            .Numeric("amount", header: "Amount", width: Widths.AnsiChars(5), iseditingreadonly: true)   //14
            .Text("farmout", header: "Farm Out", width: Widths.AnsiChars(5), settings: ts, iseditingreadonly: true) //15
            .Text("farmin", header: "Farm In", width: Widths.AnsiChars(5), settings: ts2, iseditingreadonly: true)  //16
            .Text("apqty", header: "A/P Qty", width: Widths.AnsiChars(5), settings: ts3, iseditingreadonly: true)   //17
            .Text("exceed", header: "Exceed", width: Widths.AnsiChars(5), iseditingreadonly: true);     //18
            #endregion
            #region 可編輯欄位變色
            detailgrid.Columns[7].DefaultCellStyle.BackColor = Color.Pink;  //PCS/Stitch
            detailgrid.Columns[9].DefaultCellStyle.BackColor = Color.Pink;  //Cutpart Name
            detailgrid.Columns[10].DefaultCellStyle.BackColor = Color.Pink; //Unit Price
            detailgrid.Columns[12].DefaultCellStyle.BackColor = Color.Pink; //Qty/GMT
            #endregion
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
                DialogResult dResult = MessageBox.Show("Do you want to unapprove it?", "Question", MessageBoxButtons.YesNo,MessageBoxIcon.Question,MessageBoxDefaultButton.Button2);
                if (dResult.ToString().ToUpper() == "NO") return;
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
            if (CurrentMaintain["closed"].ToString().ToUpper()=="FALSE")
            {
                sqlcmd = string.Format("update artworkpo set closed=1 , editname = '{0}' , editdate = GETDATE() " +
                                "where id = '{1}'", Env.User.UserID, CurrentMaintain["id"]);
            }
            else
            {
                sqlcmd = string.Format("update artworkpo set closed=0  , editname = '{0}' , editdate = GETDATE() " +
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

        //batch import
        private void button4_Click(object sender, EventArgs e)
        {
            var dr = CurrentMaintain; if (null == dr) return;
            if (dr["artworktypeid"] == DBNull.Value)
            {
                MessageBox.Show("Please fill Artworktype first!");
                txtartworktype_fty1.Focus();
                return;
            }
            var frm = new Sci.Production.Subcon.P01_Import(dr, (DataTable)detailgridbs.DataSource,"P01");
            frm.ShowDialog(this);
            this.RenewData();
        }

        //Special Record
        private void button5_Click(object sender, EventArgs e)
        {
            var dr = CurrentMaintain; if (null == dr) return;
            if (dr["artworktypeid"] == DBNull.Value)
            {
                MessageBox.Show("Please fill Artworktype first!");
                txtartworktype_fty1.Focus();
                return;
            }
           
            var frm = new Sci.Production.Subcon.P01_SpecialRecord(dr,(DataTable)detailgridbs.DataSource,"P01");
            frm.ShowDialog(this);
            this.RenewData();
            detailgridbs.EndEdit();
        }

    }
}
