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

namespace Sci.Production.Warehouse
{
    public partial class P50_Import : Sci.Win.Subs.Base
    {
        DataRow dr_master;
        DataTable dt_detail;
        private Dictionary<string, string> di_fabrictype = new Dictionary<string, string>();
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        bool flag;
        string poType;
        protected DataTable dtFtyinventory;

        public P50_Import(DataRow master, DataTable detail)
        {
            InitializeComponent();
            di_fabrictype.Add("F", "Fabric");
            di_fabrictype.Add("A", "Accessory");
            dr_master = master;
            dt_detail = detail;
            MyUtility.Tool.SetupCombox(cbbCategory, 2, 1, ",All,B,Bulk,S,Sample,M,Material");
            cbbCategory.SelectedIndex = 0;
            MyUtility.Tool.SetupCombox(cbbFabricType, 2, 1, ",All,F,Fabric,A,Accessory");
            cbbFabricType.SelectedIndex = 0;
            MyUtility.Tool.SetupCombox(cbbSort, 1, 1, "Material Type,SP#");
            cbbSort.SelectedIndex = 0;
        }

        //Find Now Button
        private void button1_Click(object sender, EventArgs e)
        {
            StringBuilder strSQLCmd = new StringBuilder();

            String sp1 = this.txtSP1.Text.TrimEnd();
            String sp2 = this.txtSP2.Text.TrimEnd();

            String category = this.cbbCategory.SelectedValue.ToString();
            String fabrictype = this.cbbFabricType.SelectedValue.ToString();

            String location = this.txtLocation.Text;
            String price1 = this.nbPrice1.Text;
            String price2 = this.nbPrice2.Text;

            String randomCount = this.nbRandom.Text;

            if (MyUtility.Check.Empty(sp1) && MyUtility.Check.Empty(sp2) && MyUtility.Check.Empty(category) && MyUtility.Check.Empty(fabrictype))
            {
                MyUtility.Msg.WarningBox("< SP# > < Category > < Fabric Type > can't be empty!!");
                txtSP1.Focus();
                return;
            }


            if (!(MyUtility.Check.Empty(this.nbRandom.Value)))
            {
                strSQLCmd.Append(string.Format(@"select top {0} ", randomCount));
            }
            else
            {
                strSQLCmd.Append(string.Format(@"select "));
            }
            strSQLCmd.Append(string.Format(@"  
0 as selected ,'' id
,a.POID,a.seq1,a.seq2,left(a.seq1+' ',3)+a.seq2 seq
,a.Roll
,a.Dyelot
,a.StockType
,a.MDivisionID
,a.Ukey ftyinventoryukey
,b.Refno
,b.ColorID
,b.FabricType
,b.StockUnit
,dbo.getMtlDesc(a.poid,a.seq1,a.seq2,2,0) [description]
,(select t.mtllocationid +',' from (select MtlLocationid from dbo.FtyInventory_Detail where Ukey = a.Ukey) t for xml path('')) [location]
,a.inqty-a.OutQty+a.AdjustQty qtybefore
,0.00 as QtyAfter
from dbo.FtyInventory a 
inner join dbo.PO_Supp_Detail b on b.ID = a.POID and b.SEQ1 = a.Seq1 and b.SEQ2 = a.Seq2"));

            if (!MyUtility.Check.Empty(category)) strSQLCmd.Append(string.Format(@" inner join dbo.orders c on c.id = a.poid"));
            if (!MyUtility.Check.Empty(location)) strSQLCmd.Append(string.Format(@" inner join dbo.FtyInventory_Detail d on d.Ukey = a.Ukey"));
            if (!MyUtility.Check.Empty(nbPrice1.Value) || !MyUtility.Check.Empty(nbPrice2.Value)) strSQLCmd.Append(string.Format(@" cross apply (select * from dbo.getusdprice(a.poid,a.seq1,a.seq2)) e"));
            strSQLCmd.Append(string.Format(@" where a.lock=0 and a.InQty - a.OutQty + a.AdjustQty > 0 and a.StockType ='{1}' 
and a.MDivisionID='{0}' ", Sci.Env.User.Keyword, dr_master["stocktype"])); // 

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
                strSQLCmd.Append(string.Format(@" and a.poid between '{0}' and '{1}' ", sp1, sp2.PadLeft(13,'Z')));
            }
            if (!MyUtility.Check.Empty(location))
            {
                strSQLCmd.Append(string.Format(@" and d.mtllocationid = '{0}' ", location));
            }
            if (!MyUtility.Check.Empty(nbPrice1.Value) || !MyUtility.Check.Empty(nbPrice2.Value))
            {
                strSQLCmd.Append(string.Format(@" and usd_price between {0} and {1} ", price1, price2));
            }

            if (!MyUtility.Check.Empty(randomCount))
            {
                strSQLCmd.Append(string.Format(@" order by newid()"));
            }

            MyUtility.Msg.WaitWindows("Data Loading....");

            Ict.DualResult result;
            if (result = DBProxy.Current.Select(null, strSQLCmd.ToString(), out dtFtyinventory))
            {
                if (dtFtyinventory.Rows.Count == 0) { MyUtility.Msg.WarningBox("Data not found!!"); }
                listControlBindingSource1.DataSource = dtFtyinventory;
                dtFtyinventory.DefaultView.Sort = "fabrictype,poid,seq1,seq2,location,dyelot,roll";
            }
            else { ShowErr(strSQLCmd.ToString(), result); }
            MyUtility.Msg.WaitClear();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            Ict.Win.UI.DataGridViewComboBoxColumn cbb_fabrictype;
            this.grid1.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.grid1.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.grid1)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)   //0
                .Text("poid", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(13)) //1
                .Text("seq", header: "Seq#", iseditingreadonly: true, width: Widths.AnsiChars(6)) //1
                .Text("roll", header: "Roll", iseditingreadonly: true, width: Widths.AnsiChars(6)) //4
                .Text("dyelot", header: "Dyelot", iseditingreadonly: true, width: Widths.AnsiChars(6)) //3
                .Text("refno", header: "Ref#", iseditingreadonly: true)      //5
                .Text("Colorid", header: "Color", iseditingreadonly: true)      //5
                .ComboBox("FabricType", header: "Fabric Type", iseditable: false).Get(out cbb_fabrictype)      //5
                .Text("StockUnit", header: "Unit", iseditingreadonly: true)      //5
                .Text("location", header: "Bulk Location", iseditingreadonly: true)      //2
               .EditText("Description", header: "Description", iseditingreadonly: true, width: Widths.AnsiChars(25))
               .Numeric("qtybefore", header: "Book Qty", decimal_places: 2, integer_places: 10, iseditingreadonly: true)  //7
               ; //8
            cbb_fabrictype.DataSource = new BindingSource(di_fabrictype, null);
            cbb_fabrictype.ValueMember = "Key";
            cbb_fabrictype.DisplayMember = "Value";

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

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }



        private void button2_Click(object sender, EventArgs e)
        {
            //listControlBindingSource1.EndEdit();
            grid1.ValidateControl();
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

        private void numericBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void cbbSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbbSort.SelectedIndex)
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
            string sql = string.Format("select id,Description from mtllocation where junk=0 or Junk is null and stocktype='{0}' and MDivisionID='{1}'", dr_master["stocktype"], dr_master["MDivisionID"]);
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sql, "10,20", this.txtLocation.Text);
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel) { return; }
            this.txtLocation.Text = item.GetSelectedString();
        }

        private void txtLocation_Validating(object sender, CancelEventArgs e)
        {
            base.OnValidating(e);
            string textValue = this.txtLocation.Text;
            if (!string.IsNullOrWhiteSpace(textValue) && textValue != this.txtLocation.OldValue)
            {
                string sql = string.Format("select id,Description from mtllocation where junk=0 or Junk is null and stocktype='{0}' and MDivisionID='{1}' and id='{2}'", dr_master["stocktype"], dr_master["MDivisionID"], textValue);
                if (!MyUtility.Check.Seek(sql))
                {
                    MyUtility.Msg.WarningBox(string.Format("< Location: {0} > not found!!!", textValue));
                    this.txtLocation.Text = "";
                    e.Cancel = true;
                    return;
                }
            }
        }


    }
}
