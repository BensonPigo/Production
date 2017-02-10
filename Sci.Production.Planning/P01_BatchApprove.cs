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

namespace Sci.Production.Planning
{
    public partial class P01_BatchApprove : Sci.Win.Subs.Base
    {
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        public P01_BatchApprove()
        {
            InitializeComponent();

            //BUG FIX:0000388: PLANNING_P01_BatchApprove。2.sp#不需要預設帶值。
            //this.textBoxSp1.Text = orderid;
            //this.textBoxSp2.Text = orderid;

            dateRangeApvDate.Enabled = false;
            btnUnApprove.Enabled = false;
        }
        
        // Grid 設定
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.grid1.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.grid1.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.grid1)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)   //0
                .Text("factoryid", header: "Factory", iseditingreadonly: true) //1
                .Text("id", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(13))//2
                .Date("scidelivery", header: "SCI Delivery", iseditingreadonly: true) //1
                .Text("artworktypeid", header: "Artwork Type", iseditingreadonly: true)      //3
                 .Text("localsuppid", header: "Supplier", iseditingreadonly: true)      //3
                 .Text("SupplierName", header: "Supplier Name", iseditingreadonly: true)//4
                 .Date("ArtworkInLine", header: "Sub Process Inline", iseditingreadonly: true) //1
                 .DateTime("ApvDate", header: "Approve Date", iseditingreadonly: true, format:DataGridViewDateTimeFormat.yyyyMMdd) //1
                 ;//10

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

        //Query
        private void btnQuery_Click(object sender, EventArgs e)
        {
            String sp_b = this.textBoxSp1.Text;
            String sp_e = this.textBoxSp2.Text;
            String factory = this.txtfactory1.Text;
            String artworktype = this.txtartworktype_fty1.Text;
            bool chkApprove = checkBox3.Checked;

            string inline_b, inline_e, sewinline_b, sewinline_e, delivery_b, delivery_e,approve_b,approve_e;
            inline_b = null;
            inline_e = null;
            sewinline_b = null;
            sewinline_e = null;
            delivery_b = null;
            delivery_e = null;
            approve_b = null;
            approve_e = null;

            
            if (dateRangeInline.Value1 != null) {inline_b = this.dateRangeInline.Text1;}
            if (dateRangeInline.Value2 != null) { inline_e = this.dateRangeInline.Text2; }
            if (dateRangeSewInLine.Value1 != null) {sewinline_b = this.dateRangeSewInLine.Text1;}
            if (dateRangeSewInLine.Value2 != null) { sewinline_e = this.dateRangeSewInLine.Text2; }
            if (dateRangeSciDelivery.Value1 != null) {delivery_b = this.dateRangeSciDelivery.Text1;}
            if (dateRangeSciDelivery.Value2 != null) { delivery_e = this.dateRangeSciDelivery.Text2; }
            if (dateRangeApvDate.Value1 != null) {approve_b = this.dateRangeApvDate.Text1;}
            if (dateRangeApvDate.Value2 != null){ approve_e = this.dateRangeApvDate.Text2; }

            if (chkApprove && (MyUtility.Check.Empty(approve_b) || MyUtility.Check.Empty(approve_e)))
            {
                MyUtility.Msg.WarningBox("Approve date can't be empty", "Warning");
                return;
            }

            if ((MyUtility.Check.Empty(inline_b) && MyUtility.Check.Empty(inline_e)) &&
                (MyUtility.Check.Empty(sewinline_b) && MyUtility.Check.Empty(sewinline_e)) &&
                (MyUtility.Check.Empty(delivery_b) && MyUtility.Check.Empty(delivery_e)) &&
                MyUtility.Check.Empty(sp_b) && MyUtility.Check.Empty(sp_e) && 
                MyUtility.Check.Empty(artworktype) )
            {
                MyUtility.Msg.WarningBox("< Inline Date > or < SewInline Date > or < SCI Delivery > or < SP# > or < Artwork Type > can't be empty!!");
                textBoxSp1.Focus();
                return;
            }
            else
            {
                // 建立可以符合回傳的Cursor

                string strSQLCmd = string.Format(@"select 1 as Selected
,ods.FactoryID
,ot.ID
,ot.ArtworkTypeID
,ot.LocalSuppID
,ls.Abb SupplierName
,ot.ArtworkInLine
,ods.SciDelivery
,ot.ApvDate
from Order_TmsCost ot 
inner join orders ods on ot.ID = ods.ID
inner join LocalSupp ls on ls.id = ot.LocalSuppID
inner join dbo.factory on factory.id = ods.factoryid
where ods.finished=0 and ods.isforecast = 0 
    and (ods.category = 'B' or ods.category = 'S')
    and ods.qty > 0 and (ot.qty > 0 or ot.tms > 0)
    and factory.mdivisionid='{0}'", Sci.Env.User.Keyword);
                if (!chkApprove) { strSQLCmd += " and  ot.apvdate is null"; }
                if (!MyUtility.Check.Empty(sp_b)) { strSQLCmd += " and  ot.id  between @sp1 and  @sp2"; }
                if (!MyUtility.Check.Empty(inline_b)) { strSQLCmd += string.Format(" and not(ot.artworkinline > '{1}' or ot.artworkoffline < '{0}')", inline_b, inline_e); }
                if (!MyUtility.Check.Empty(sewinline_b)) { strSQLCmd += string.Format(" and not(ods.sewinline > '{1}' or ods.sewoffline < '{0}')", sewinline_b, sewinline_e); }
                if (!MyUtility.Check.Empty(artworktype)) { strSQLCmd += " and ot.artworktypeid = @artworktypeid"; }
                if (!MyUtility.Check.Empty(delivery_b)) { strSQLCmd += string.Format(" and ods.sciDelivery between '{0}' and '{1}'", delivery_b, delivery_e); }
                if (!MyUtility.Check.Empty(approve_b)) { strSQLCmd += string.Format(" and ot.apvdate between '{0}' and '{1}'", approve_b, approve_e); }
                if (!MyUtility.Check.Empty(factory)) { strSQLCmd += " and ods.factoryid = @factoryid"; }
                
                strSQLCmd += @" order by ods.FactoryID
,ot.ID
,ot.ArtworkTypeID
,ot.LocalSuppID,ot.ArtworkInLine,ods.SciDelivery";

                #region 準備sql參數資料
                System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
                sp1.ParameterName = "@sp1";
                sp1.Value = sp_b.TrimEnd();

                System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter();
                sp2.ParameterName = "@sp2";
                sp2.Value = sp_e.TrimEnd();

                System.Data.SqlClient.SqlParameter sp3 = new System.Data.SqlClient.SqlParameter();
                sp3.ParameterName = "@artworktypeid";
                sp3.Value = artworktype;

                System.Data.SqlClient.SqlParameter sp4 = new System.Data.SqlClient.SqlParameter();
                sp4.ParameterName = "@factoryid";
                sp4.Value = factory;

                IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                cmds.Add(sp1);
                cmds.Add(sp2);
                cmds.Add(sp3);
                cmds.Add(sp4);
                #endregion

                DataTable dtOT;
           
                Ict.DualResult result;
                if (result = DBProxy.Current.Select(null, strSQLCmd, cmds, out dtOT))
                {
                    if (dtOT.Rows.Count == 0)
                    { MyUtility.Msg.WarningBox("Data not found!!"); }
                    listControlBindingSource1.DataSource = dtOT;
                }
                else { ShowErr(strSQLCmd, result); }
            }
            this.grid1.AutoResizeColumns();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //check for approved data
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                dateRangeApvDate.Enabled = true;
                btnApprove.Enabled = false;
                btnUnApprove.Enabled = true;
                dateRangeApvDate.TextBox1.Value = DateTime.Now;
                dateRangeApvDate.TextBox2.Value = DateTime.Now;
            }
            else
            {
                dateRangeApvDate.Enabled = false;
                dateRangeApvDate.TextBox1.Value=null;
                dateRangeApvDate.TextBox2.Value = null;
                btnApprove.Enabled = true;
                btnUnApprove.Enabled = false;
            }
        }

        //Approve
        private void btnApprove_Click(object sender, EventArgs e)
        {
            listControlBindingSource1.EndEdit();
            grid1.ValidateControl();

            DataTable dtImport = (DataTable)listControlBindingSource1.DataSource;

            if (MyUtility.Check.Empty(dtImport) || dtImport.Rows.Count == 0) return;

            DataRow[] dr2 = dtImport.Select("Selected = 1 ");
            if (dr2.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select rows first!", "Warnning");
                return;
            }

            DialogResult dResult = MyUtility.Msg.QuestionBox("Do you want to Approve data?", "Question", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);
            if (dResult.ToString().ToUpper() == "NO") return;

            IList<SqlCommandText> updateCmds = new List<SqlCommandText>();

            for (int i = 0; i < dr2.Length; i++)
			{
                SqlCommandText tmp = new SqlCommandText(string.Format(@"update order_tmscost set apvdate = getdate() , apvname = '{0}'
                                            where id = '{1}' and artworktypeid = '{2}';", Env.User.UserID, dr2[i]["id"], dr2[i]["artworktypeid"]),null);
                updateCmds.Add(tmp);
			}

            DualResult result;
            if (!(result = DBProxy.Current.Executes(null, updateCmds)))
            {
                ShowErr(result);
                return;
            }

            MyUtility.Msg.InfoBox("Approve data successful.");


        }

        //To Excel
        private void btnToExcel_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)listControlBindingSource1.DataSource;
            MyUtility.Excel.CopyToXls(dt, "", "Planning_P01_BatchApprove.xltx", 1);
        }

        //unApprove
        private void btnUnApprove_Click(object sender, EventArgs e)
        {
            listControlBindingSource1.EndEdit();
            grid1.ValidateControl();

            DataTable dtImport = (DataTable)listControlBindingSource1.DataSource;

            if (MyUtility.Check.Empty(dtImport) || dtImport.Rows.Count == 0) return;

            DataRow[] dr2 = dtImport.Select("Selected = 1 ");
            if (dr2.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select rows first!", "Warnning");
                return;
            }

            DialogResult dResult = MyUtility.Msg.QuestionBox("Do you want to UnApprove data?", "Question", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);
            if (dResult.ToString().ToUpper() == "NO") return;

            IList<SqlCommandText> updateCmds = new List<SqlCommandText>();

            for (int i = 0; i < dr2.Length; i++)
            {
                SqlCommandText tmp = new SqlCommandText(string.Format(@"update order_tmscost set apvdate = null , apvname = ''
                                            where id = '{1}' and artworktypeid = '{2}';", Env.User.UserID, dr2[i]["id"], dr2[i]["artworktypeid"]), null);
                updateCmds.Add(tmp);
            }

            DualResult result;
            if (!(result = DBProxy.Current.Executes(null, updateCmds)))
            {
                ShowErr(result);
                return;
            }

            MyUtility.Msg.InfoBox("UnApprove data successful.");
        }
    }

   
}
