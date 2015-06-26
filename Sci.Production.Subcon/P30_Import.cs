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

namespace Sci.Production.Subcon
{
    public partial class P30_Import : Sci.Win.Subs.Base
    {
        DataRow dr_localPO;
        DataTable dt_localPODetail;
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        bool flag;
        string poType;
        protected DataTable dtlocal;

        public P30_Import(DataRow master, DataTable detail)
        {
            InitializeComponent();
            dr_localPO = master;
            dt_localPODetail = detail;
            this.Text += string.Format(" ( Categgory:{0} - Supplier:{1} )", dr_localPO["category"].ToString(), dr_localPO["localsuppid"].ToString());
            this.dateRangeApvDate.Enabled = dr_localPO["category"].ToString().TrimEnd().ToUpper() == "CARTON";
        }

        //Find Now Button
        private void button1_Click(object sender, EventArgs e)
        {
            String sp_b = this.textBox1.Text;
            String sp_e = this.textBox2.Text;
            String brandid = this.txtbrand1.Text;

            string booking_b, booking_e, sewinline_b, sewinline_e, arrived_b, arrived_e, approved_b, approved_e, scidelivery_b, scidelivery_e;
            booking_b = null;
            booking_e = null;
            sewinline_b = null;
            sewinline_e = null;
            arrived_b= null;
            arrived_e = null;
            approved_b= null;
            approved_e= null;
            scidelivery_b= null;
            scidelivery_e = null;
            

            if (dateRangeBooking.Value1 != null) booking_b = this.dateRangeBooking.Text1;
            if (dateRangeBooking.Value2 != null) { booking_e = this.dateRangeBooking.Text2; }
            if (dateRangeSewInLine.Value1 != null) sewinline_b = this.dateRangeSewInLine.Text1;
            if (dateRangeSewInLine.Value2 != null) { sewinline_e = this.dateRangeSewInLine.Text2; }
            if (dateRangeArrived.Value1 != null) arrived_b = this.dateRangeArrived.Text1;
            if (dateRangeArrived.Value2 != null) { arrived_e = this.dateRangeArrived.Text2; }
            if (dateRangeApvDate.Value1 != null) approved_b = this.dateRangeApvDate.Text1;
            if (dateRangeApvDate.Value2 != null) { approved_e = this.dateRangeApvDate.Text2; }
            if (dateRangeSciDelivery.Value1 != null) scidelivery_b = this.dateRangeSciDelivery.Text1;
            if (dateRangeSciDelivery.Value2 != null) { scidelivery_e = this.dateRangeSciDelivery.Text2; }

            

            if ((myUtility.Empty(booking_b) && myUtility.Empty(booking_e)) &&
                (myUtility.Empty(sewinline_b) && myUtility.Empty(sewinline_e ))  &&
                (myUtility.Empty(arrived_b) && myUtility.Empty(arrived_e)) &&
                (myUtility.Empty(approved_b) && myUtility.Empty(approved_e)) &&
                (myUtility.Empty(scidelivery_b) && myUtility.Empty(scidelivery_e)) &&
                myUtility.Empty(sp_b) && myUtility.Empty(sp_e) && myUtility.Empty(brandid))
            {
                myUtility.WarningBox(@"< Booking Date > or < Sci Delivery > or < Arrived Date > or < Sewing Inline >  
                                                or < Approve Date > or  < SP# > or  < Brand > can't be empty!!");
                textBox1.Focus();
                return;
            }
            else
            {
                #region 組sql語法
                string strSQLCmd = null;
                IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                // 建立可以符合回傳的Cursor - Carton
                if (dr_localPO["category"].ToString().TrimEnd().ToUpper() == "CARTON")
                {
                    strSQLCmd = string.Format(@"select 1 as Selected,c.POID ,b.OrderID ,c.StyleID,c.SeasonID ,b.RefNo 
                                                                ,'' as description 
                                                                ,'' as threadcolorid
                                                                ,sum(b.CTNQty) qty, d.UnitID,d.Price, sum(b.CTNQty) * d.Price as amount 
                                                                ,'' as remark ,a.EstCTNArrive etd ,a.ID as requestid, '' as id
                                                                from dbo.PackingList a 
                                                                        , dbo.PackingList_Detail b 
                                                                        , dbo.Orders c 
                                                                        , LocalItem d
                                                                where a.ID = b.ID 
                                                                    and b.OrderID = c.ID
                                                                    and b.RefNo = d.RefNo
                                                                    and a.ApvToPurchase = 1 
                                                                    and a.LocalPOID =''
                                                                    and a.factoryid = '{0}'
                                                                "
                                                                ,Env.User.Factory);

                    if (!myUtility.Empty(sp_b)) { strSQLCmd += " and c.id between @sp1 and @sp2"; }
                    if (!myUtility.Empty(brandid)) { strSQLCmd += " and c.brandid = @brandid"; }
                    if (!myUtility.Empty(sewinline_b)) { strSQLCmd += string.Format(" and c.sewinline between '{0}' and '{1}'", sewinline_b, sewinline_e); }
                    if (!myUtility.Empty(scidelivery_b)) { strSQLCmd += string.Format(" and c.scidelivery between '{0}' and '{1}'", scidelivery_b, scidelivery_e); }
                    if (!myUtility.Empty(booking_b)) { strSQLCmd += string.Format(" and a.EstCTNBooking between '{0}' and '{1}'", booking_b, booking_e); }
                    if (!myUtility.Empty(arrived_b)) { strSQLCmd += string.Format(" and a.EstCTNArrive between '{0}' and '{1}'", arrived_b, arrived_e); }
                    if (!myUtility.Empty(approved_b)) { strSQLCmd += string.Format(" and a.ApvToPurchaseDate between '{0}' and '{1}'", approved_b, approved_e); }
                    strSQLCmd += " group by c.POID,b.OrderID,c.StyleID,c.SeasonID,b.RefNo,d.UnitID,d.Price,a.EstCTNArrive,a.ID ";

                    #region 準備sql參數資料
                    System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
                    sp1.ParameterName = "@sp1";
                    sp1.Value = sp_b;

                    System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter();
                    sp2.ParameterName = "@sp2";
                    sp2.Value = sp_e;

                    System.Data.SqlClient.SqlParameter sp3 = new System.Data.SqlClient.SqlParameter();
                    sp3.ParameterName = "@brandid";
                    sp3.Value = brandid;

                    
                    cmds.Add(sp1);
                    cmds.Add(sp2);
                    cmds.Add(sp3);
                    
                    #endregion
                }
                else
                {
                    strSQLCmd = string.Format(@"select 1 as Selected,c.POID ,a.OrderID ,a.StyleID,a.SeasonID ,b.RefNo 
                                                                ,'' as description 
                                                                ,b.threadcolorid
                                                                ,b.PurchaseQty as qty
                                                                ,d.UnitID,d.Price
                                                                ,b.PurchaseQty * d.Price as amount 
                                                                ,'' as remark ,a.EstArriveDate etd 
                                                                ,a.OrderID as requestid
                                                                , '' as id
                                                                from dbo.ThreadRequisition a 
                                                                        , dbo.ThreadRequisition_Detail b 
                                                                        , dbo.Orders c 
                                                                        , LocalItem d
                                                                where a.OrderID = b.OrderID 
                                                                    and b.OrderID = c.ID
                                                                    and b.RefNo = d.RefNo
                                                                    and a.status = 'Approved' 
                                                                    and a.factoryid = '{0}'
                                                                "
                        , Env.User.Factory);

                    if (!myUtility.Empty(sp_b)) { strSQLCmd += " and c.id between @sp1 and @sp2"; }
                    if (!myUtility.Empty(brandid)) { strSQLCmd += " and c.brandid = @brandid"; }
                    if (!myUtility.Empty(sewinline_b)) { strSQLCmd += string.Format(" and c.sewinline between '{0}' and '{1}'", sewinline_b, sewinline_e); }
                    if (!myUtility.Empty(scidelivery_b)) { strSQLCmd += string.Format(" and c.scidelivery between '{0}' and '{1}'", scidelivery_b, scidelivery_e); }
                    if (!myUtility.Empty(booking_b)) { strSQLCmd += string.Format(" and a.EstBookDate between '{0}' and '{1}'", booking_b, booking_e); }
                    if (!myUtility.Empty(arrived_b)) { strSQLCmd += string.Format(" and a.EstArriveDate between '{0}' and '{1}'", arrived_b, arrived_e); }
                    
                    //strSQLCmd += " group by c.POID,b.OrderID,c.StyleID,c.SeasonID,b.RefNo,d.UnitID,d.Price,a.EstArriveDate,a.ID ";

                    #region 準備sql參數資料
                    System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
                    sp1.ParameterName = "@sp1";
                    sp1.Value = sp_b;

                    System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter();
                    sp2.ParameterName = "@sp2";
                    sp2.Value = sp_e;

                    System.Data.SqlClient.SqlParameter sp3 = new System.Data.SqlClient.SqlParameter();
                    sp3.ParameterName = "@brandid";
                    sp3.Value = brandid;


                    cmds.Add(sp1);
                    cmds.Add(sp2);
                    cmds.Add(sp3);
                    #endregion  
                }
                #endregion

                Ict.DualResult result;
                if (result = DBProxy.Current.Select(null, strSQLCmd,cmds, out dtlocal))
                {
                    if (dtlocal.Rows.Count == 0)
                    { myUtility.WarningBox("Data not found!!"); }
                    listControlBindingSource1.DataSource = dtlocal;
                }
                else 
                    { ShowErr(strSQLCmd, result); }

                #region 加工remark欄位
                string category;
                decimal price;
                foreach (DataRow dr in ((DataTable)listControlBindingSource1.DataSource).Rows)
                {
                    category = myUtility.Lookup(string.Format(@"select category from orders where id = '{0}'", dr["orderid"]), null);
                    if (category == "B")
                    {
                        price = decimal.Parse(myUtility.Lookup(string.Format(@"select price from order_tmscost where id='{0}' and artworktypeid='{1}'"                            
                                                                                            , dr["orderid"], dr_localPO["category"].ToString().TrimEnd().ToUpper()), null));
                        if (myUtility.Empty(price) || price == 0)
                        {
                            dr["remark"] = "Price is 0, can not be transfered to local purchase!!";
                            dr["selected"] = 0;
                        }
                    }
                }
                #endregion  
            }
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            Ict.Win.DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            ns.CellValidating += (s, e) =>
            {
                if (null == e.FormattedValue) return;
                DataRow ddr = grid1.GetDataRow<DataRow>(e.RowIndex);
                if ((decimal)e.FormattedValue > (decimal)ddr["unpaid"])
                {
                    e.Cancel = true;
                    myUtility.WarningBox("Qty can't be more than unpaid");
                    return;
                }

                ddr["qty"] = e.FormattedValue;

            };


            this.grid1.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.grid1.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.grid1)
                 .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)   //0
                .Text("poid", header: "Master SP#", iseditingreadonly: true) //0
                .Text("orderid", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(13))//1
                .Text("styleid", header: "Style", iseditingreadonly: true, width: Widths.AnsiChars(13))//2
                .Text("Seasonid", header: "Season", iseditingreadonly: true, width: Widths.AnsiChars(13))//3
                 .Text("refno", header: "Refno", iseditingreadonly: true)      //4
                .Text("description", header: "Description", iseditingreadonly: true)//5
                .Text("threadcolorid", header: "Color Shade", iseditingreadonly: true)//6
                .Numeric("qty", header: "Qty", iseditingreadonly: true)//7
                .Text("Unitid", header: "Unit", iseditingreadonly: true)//8
                .Numeric("Price", header: "Price", iseditable: true, decimal_places: 4, integer_places: 4)  //9
                .Numeric("amount", header: "Amount", iseditable: true, decimal_places: 4, integer_places: 4)  //10
                .Text("remark", header: "Remark", iseditingreadonly: true)//11
                .Date("etd", header: "ETD", iseditingreadonly: true)//12
                 .Text("requestid", header: "Request ID", iseditingreadonly: true)//13
                ;
            //this.grid1.Columns[10].DefaultCellStyle.BackColor = Color.Pink;  //Qty   

            // 全選
            checkBox1.Click += (s, e) =>
            {
                if (null != col_chk)
                {
                    this.grid1.SetCheckeds(col_chk);
                    if (col_chk.Index == this.grid1.CurrentCellAddress.X)
                    {
                        if (this.grid1.IsCurrentCellInEditMode) this.grid1.RefreshEdit();
                    }
                }
            };

            // 全不選
            checkBox2.Click += (s, e) =>
            {
                if (null != col_chk)
                {
                    this.grid1.SetUncheckeds(col_chk);
                    if (col_chk.Index == this.grid1.CurrentCellAddress.X)
                    {
                        if (this.grid1.IsCurrentCellInEditMode) this.grid1.RefreshEdit();
                    }
                }
            };
        }

        // close
        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // import
        private void button2_Click(object sender, EventArgs e)
        {
            listControlBindingSource1.EndEdit();
            grid1.ValidateControl();
            
            DataTable dtImport = (DataTable)listControlBindingSource1.DataSource;
            
            if (myUtility.Empty(dtImport) || dtImport.Rows.Count == 0) return;
            
            DataRow[] dr2 = dtImport.Select("Selected = 1 and remark=''");
            if (dr2.Length > 0)
            {
                foreach (DataRow tmp in dr2)
                {
                    DataRow[] findrow = 
                        dt_localPODetail.Select(string.Format(@"orderid = '{0}' 
                                                                                    and refno = '{1}'
                                                                                    and threadcolorid = '{2}'"
                                                                                , tmp["orderid"].ToString()
                                                                                , tmp["refno"].ToString()
                                                                                , tmp["threadcolorid"].ToString())
                                                           );
                    if (findrow.Length > 0)
                    {
                        findrow[0]["Price"] = tmp["Price"];
                        findrow[0]["qty"] = tmp["qty"];
                    }
                    else
                    {
                        tmp["id"] = dr_localPO["id"];
                        tmp.AcceptChanges();
                        tmp.SetAdded();
                        dt_localPODetail.ImportRow(tmp);
                    }
                }
            }
            else
            {
                myUtility.WarningBox("Please select rows with empty remark first!", "Warnning");
                return;
            }
            this.Close();
        }

        // To Excel
        private void button4_Click(object sender, EventArgs e)
        {
            string MyDocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(Application.StartupPath);
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.RestoreDirectory = true;
            dlg.InitialDirectory = MyDocumentsPath;     //指定"我的文件"路徑
            dlg.Title = "Save as Excel File";
            dlg.FileName = "P30_Import_ToExcel_" + DateTime.Now.ToString("yyyyMMdd") + @".xls";

            dlg.Filter = "Excel Files (*.xls)|*.xls";            // Set filter for file extension and default file extension

            // Display OpenFileDialog by calling ShowDialog method ->ShowDialog()
            // Get the selected file name and CopyToXls
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK && dlg.FileName != null)
            {
                // Open document
                DataTable dt = (DataTable)listControlBindingSource1.DataSource;
                DualResult result = myUtility.CopyToXls(dt, dlg.FileName);
                if (result) { myUtility.XlsAutoFit(dlg.FileName); }   //XlsAutoFit(dlg.FileName, "MMDR030.xlt", 12);
                else { myUtility.WarningBox(result.ToMessages().ToString(), "Warning"); }
            }
            else
            {
                return;
            }
        }
    }
}
