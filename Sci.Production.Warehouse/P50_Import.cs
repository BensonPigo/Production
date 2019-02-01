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
using System.Globalization;

namespace Sci.Production.Warehouse
{
    public partial class P50_Import : Sci.Win.Subs.Base
    {
        DataRow dr_master;
        DataTable dt_detail;
        int price1 = 0;
        int price2=0;
        private Dictionary<string, string> di_fabrictype = new Dictionary<string, string>();
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
      //  bool flag;
      //  string poType;
        protected DataTable dtFtyinventory;

        public P50_Import(DataRow master, DataTable detail)
        {
            InitializeComponent();
            di_fabrictype.Add("F", "Fabric");
            di_fabrictype.Add("A", "Accessory");
            dr_master = master;
            dt_detail = detail;
            MyUtility.Tool.SetupCombox(comboCategory, 2, 1, ",All,B,Bulk,S,Sample,M,Material");
            comboCategory.SelectedIndex = 0;
            MyUtility.Tool.SetupCombox(comboFabricType, 2, 1, ",All,F,Fabric,A,Accessory");
            comboFabricType.SelectedIndex = 0;
            MyUtility.Tool.SetupCombox(comboSortby, 1, 1, "Material Type,SP#");
            comboSortby.SelectedIndex = 0;
            this.numPrice1.Text = "";
            this.numPrice2.Text = "";
        }

        //Find Now Button
        private void btnFindNow_Click(object sender, EventArgs e)
        {
            StringBuilder strSQLCmd = new StringBuilder();

            String sp1 = this.txtSPNoStart.Text.TrimEnd();
            String sp2 = this.txtSPNoEnd.Text.TrimEnd();

            String category = this.comboCategory.SelectedValue.ToString();
            String fabrictype = this.comboFabricType.SelectedValue.ToString();

            String location = this.txtLocation.Text;
            //format 千分位符號
            if (!MyUtility.Check.Empty( this.numPrice1.Text))
            {
                 price1 = int.Parse(this.numPrice1.Text, NumberStyles.AllowThousands);    
            }
            if (!MyUtility.Check.Empty(this.numPrice2.Text))
            {
                 price2 = int.Parse(this.numPrice2.Text, NumberStyles.AllowThousands);    
            }
            
            

            String randomCount = this.numRandom.Value.ToString();

            if (MyUtility.Check.Empty(sp1) && MyUtility.Check.Empty(sp2) && MyUtility.Check.Empty(category) && MyUtility.Check.Empty(fabrictype))
            {
                MyUtility.Msg.WarningBox("< SP# > < Category > < Fabric Type > can't be empty!!");
                txtSPNoStart.Focus();
                return;
            }


            if (!(MyUtility.Check.Empty(this.numRandom.Value)))
            {
                strSQLCmd.Append(string.Format(@"select top {0} ", randomCount));
            }
            else
            {
                strSQLCmd.Append(string.Format(@"select "));
            }
            strSQLCmd.Append(string.Format(@"  
0 as selected ,'' id
,a.POID,a.seq1,a.seq2,concat(Ltrim(Rtrim(a.seq1)), ' ', a.seq2) seq
,a.Roll
,a.Dyelot
,a.StockType
,a.Ukey ftyinventoryukey
,b.Refno
,b.ColorID
,b.FabricType
,b.StockUnit
,dbo.getMtlDesc(a.poid,a.seq1,a.seq2,2,0) [description]
,dbo.Getlocation(a.ukey) [location]
,a.inqty-a.OutQty+a.AdjustQty qtybefore
,0.00 as QtyAfter
from dbo.FtyInventory a WITH (NOLOCK) 
inner join dbo.PO_Supp_Detail b WITH (NOLOCK) on b.ID = a.POID and b.SEQ1 = a.Seq1 and b.SEQ2 = a.Seq2
inner join dbo.Factory f on f.ID=b.factoryID"));

            if (!MyUtility.Check.Empty(category)) strSQLCmd.Append(string.Format(@" inner join dbo.orders c on c.id = a.poid"));
            if (!MyUtility.Check.Empty(location)) strSQLCmd.Append(string.Format(@" inner join dbo.FtyInventory_Detail d on d.Ukey = a.Ukey"));
            if (!MyUtility.Check.Empty(numPrice1.Value) || !MyUtility.Check.Empty(numPrice2.Value)) strSQLCmd.Append(string.Format(@" cross apply (select * from dbo.getusdprice(a.poid,a.seq1,a.seq2)) e"));
            strSQLCmd.Append(string.Format(@" where a.lock=0 and a.InQty - a.OutQty + a.AdjustQty > 0 and a.StockType ='{1}' 
and f.MDivisionID='{0}' ", Sci.Env.User.Keyword, dr_master["stocktype"])); // 

            if (!MyUtility.Check.Empty(category))
            {
                strSQLCmd.Append(string.Format(@" and c.category = '{0}' ", category));
            }
            if (!MyUtility.Check.Empty(fabrictype))
            {
                strSQLCmd.Append(string.Format(@" and b.fabrictype = '{0}' ", fabrictype));
            }
            if (!MyUtility.Check.Empty(sp1) || !MyUtility.Check.Empty(sp2))
            {
                strSQLCmd.Append(string.Format(@" and a.poid between '{0}' and '{1}' ", sp1, sp2));
            }
            if (!MyUtility.Check.Empty(location))
            {
                strSQLCmd.Append(string.Format(@" and d.mtllocationid = '{0}' ", location));
            }            
            if (!MyUtility.Check.Empty(numPrice1.Value) && !MyUtility.Check.Empty(numPrice2.Value))
            {
                strSQLCmd.Append(string.Format(@" and usd_price between {0} and {1} ", price1, price2));
            }      
            else
	        {
                if (!MyUtility.Check.Empty(numPrice1.Value))
                {
                    strSQLCmd.Append(string.Format(@" and usd_price > {0}  ", price1));
                }
                if (!MyUtility.Check.Empty(numPrice2.Value))
                {
                    strSQLCmd.Append(string.Format(@" and usd_price < {0}  ", price2));
                }
            }
            if (!MyUtility.Check.Empty(randomCount))
            {
                strSQLCmd.Append(string.Format(@" order by newid()"));
            }

            this.ShowWaitMessage("Data Loading....");

            Ict.DualResult result;
            if (result = DBProxy.Current.Select(null, strSQLCmd.ToString(), out dtFtyinventory))
            {
                if (dtFtyinventory.Rows.Count == 0) { MyUtility.Msg.WarningBox("Data not found!!"); }
                listControlBindingSource1.DataSource = dtFtyinventory;
                dtFtyinventory.DefaultView.Sort = "fabrictype,poid,seq1,seq2,location,dyelot,roll";
            }
            else { ShowErr(strSQLCmd.ToString(), result); }
            this.HideWaitMessage();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            Ict.Win.UI.DataGridViewComboBoxColumn cbb_fabrictype;
            this.gridImport.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.gridImport.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.gridImport)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)   //0
                .Text("poid", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(13)) //1
                .Text("seq", header: "Seq#", iseditingreadonly: true, width: Widths.AnsiChars(6)) //1
                .Text("roll", header: "Roll", iseditingreadonly: true, width: Widths.AnsiChars(6)) //4
                .Text("dyelot", header: "Dyelot", iseditingreadonly: true, width: Widths.AnsiChars(8)) //3
                .Text("refno", header: "Ref#", iseditingreadonly: true)      //5
                .Text("Colorid", header: "Color", iseditingreadonly: true)      //5
                .ComboBox("FabricType", header: "Material Type", iseditable: false).Get(out cbb_fabrictype)      //5
                .Text("StockUnit", header: "Unit", iseditingreadonly: true)      //5
                .Text("location", header: "Bulk Location", iseditingreadonly: true)      //2
               .EditText("Description", header: "Description", iseditingreadonly: true, width: Widths.AnsiChars(25))
               .Numeric("qtybefore", header: "Book Qty", decimal_places: 2, integer_places: 10, iseditingreadonly: true)  //7
               ; //8
            cbb_fabrictype.DataSource = new BindingSource(di_fabrictype, null);
            cbb_fabrictype.ValueMember = "Key";
            cbb_fabrictype.DisplayMember = "Value";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            //listControlBindingSource1.EndEdit();
            gridImport.ValidateControl();
            DataTable dtGridBS1 = (DataTable)listControlBindingSource1.DataSource;
            if (MyUtility.Check.Empty(dtGridBS1) || dtGridBS1.Rows.Count == 0) return;

            DataRow[] dr2 = dtGridBS1.Select("Selected = 1");
            if (dr2.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select rows first!", "Warnning");
                return;
            }


            foreach (DataRow tmp in dr2)
            {
                DataRow[] findrow = dt_detail.Select(string.Format("ftyinventoryukey = '{0}'", tmp["ftyinventoryukey"]));

                if (findrow.Length > 0)
                {
                    findrow[0]["qtyafter"] = tmp["qtyafter"];
                    findrow[0]["qtybefore"] = tmp["qtybefore"];
                }
                else
                {
                    tmp["id"] = dr_master["id"];
                    tmp.AcceptChanges();
                    tmp.SetAdded();
                    dt_detail.ImportRow(tmp);
                }
            }


            this.Close();
        }

        private void numRandom_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboSortby_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboSortby.SelectedIndex)
            {
                case 0:
                    if (!MyUtility.Check.Empty(dtFtyinventory))
                    dtFtyinventory.DefaultView.Sort = "fabrictype,poid,seq1,seq2,location,dyelot,roll";
                    break;
                case 1:
                    if (!MyUtility.Check.Empty(dtFtyinventory))
                    dtFtyinventory.DefaultView.Sort = "poid,seq1,seq2,location,dyelot,roll";
                    break;
               
            }
        }

        private void txtLocation_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Sci.Win.Forms.Base myForm = (Sci.Win.Forms.Base)this.FindForm();
            if (myForm.EditMode == false || txtLocation.ReadOnly == true) return;
            string sql = string.Format(@"
select  id
        , Description 
from    mtllocation WITH (NOLOCK) 
where   junk != '1'
        and stocktype='{0}'", dr_master["stocktype"]);
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sql, "10,20", this.txtLocation.Text);
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel) { return; }
            this.txtLocation.Text = item.GetSelectedString();
        }
    }
}
