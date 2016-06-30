using Ict.Win;
using Sci.Data;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using System.Data.SqlClient;


namespace Sci.Production.Quality
{
    public partial class P30 : Sci.Win.Tems.Input6
    {
        DataGridViewGeneratorTextColumnSettings colorSelect;
        DataGridViewGeneratorTextColumnSettings itemSelect;
        private Dictionary<string, string> MD_type = new Dictionary<string, string>();
        protected DataRow motherData;

        // (menuitem, args= 參數)
        public P30(ToolStripMenuItem menuitem,String history) : base(menuitem) 
        {
            InitializeComponent();
           
            MD_type.Add("Accessory", "Accessory");
            MD_type.Add("CutPartes", "CutPartes");
            MD_type.Add("Garment", "Garment");
            string mdClose=this.textBox8.Text;

            //設定init()
            string factoryId = Sci.Env.User.Factory;
            if (history == "1".ToString())
            {
                this.DefaultFilter = string.Format("FactoryId= '{0}' and MDClose is null", factoryId);
                this.Text = "P30 .MD Master List";
               
            }
            else if (history == "2".ToString())
            {
                this.DefaultFilter = string.Format("FactoryId= '{0}' and MDClose is not null", factoryId);
                this.Text = "P31 .MD Master List(History)";
                this.IsSupportEdit = false;

            }
 

          
           
        }
        // 設定Grid內容值
        protected override void OnDetailGridSetup()
        {
            #region OnClick Right Click Even
            colorSelect = new DataGridViewGeneratorTextColumnSettings();
            itemSelect = new DataGridViewGeneratorTextColumnSettings();
            colorSelect.CellMouseClick += (s, e) =>
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    if (null == dr) { return; }
                    string sqlcmd = " select colorid from po_supp_detail where id in (select poid from orders) and fabrictype='A' and colorid is not null group by colorid";
                    SelectItem item = new SelectItem(sqlcmd, "30", dr["ColorID"].ToString());
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    dr["Colorid"] = item.GetSelectedString();
                }


            };

            //右鍵帶出選擇視窗
            itemSelect.CellMouseClick += (s1, e1) =>
            {
                if (e1.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    DataRow dr1 = this.detailgrid.GetDataRow<DataRow>(e1.RowIndex);
                    if (null == dr1) { return; }
                    string sqlcmd1 = "    select scirefno from PO_Supp_Detail  where ID in (select id from orders)  and fabrictype='A'  and Scirefno is not null  group by scirefno";
                    SelectItem item1 = new SelectItem(sqlcmd1, "30", dr1["Item"].ToString());
                    DialogResult result1 = item1.ShowDialog();
                    if (result1 == DialogResult.Cancel) { return; }
                    dr1["Item"] = item1.GetSelectedString();
                }
            };

            #endregion

               
             

            // 定義一個 class member
            Ict.Win.UI.DataGridViewComboBoxColumn cbb_MD_type;

            //設定Grid屬性 text 對應欄位值 header :顯示欄位名稱

            Helper.Controls.Grid.Generator(this.detailgrid)
                .ComboBox("Type", header: "Main Item NO", width: Widths.AnsiChars(20), iseditable: true).Get(out cbb_MD_type)
                .Text("Item", header: "SEQ Ref", width: Widths.AnsiChars(20),iseditingreadonly : true,settings: itemSelect)
                .Text("Colorid", header: "Color", width: Widths.AnsiChars(20),iseditingreadonly:true, settings: colorSelect)
                 .Date("inspdate", header: "Inspdate", width: Widths.AnsiChars(18))
                 .Text("Result", header: "Result", width: Widths.AnsiChars(50));



            cbb_MD_type.DataSource = new BindingSource(MD_type, null);
            cbb_MD_type.ValueMember = "Key";
            cbb_MD_type.DisplayMember = "Value";
        }
        
        // When Edit and Grid is empty then 新增5筆資料 in GridView
        //choice ClickEditAfter becauser Transaction problemm
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            #region 設定表頭欄位只能Readonly        
                this.textBox1.ReadOnly = true;
                this.textBox2.ReadOnly = true;
                this.textBox3.ReadOnly = true;
                this.textBox4.ReadOnly = true;
                this.textBox5.ReadOnly = true;
                this.textBox6.ReadOnly = true;
                this.textBox7.ReadOnly = true;
                this.textBox8.ReadOnly = true;
                this.textBox9.ReadOnly = true;
                this.textBox10.ReadOnly = true;
                this.textBox11.ReadOnly = true;
                this.textBox12.ReadOnly = true;
                this.textBox13.ReadOnly = true;
                this.textBox14.ReadOnly = true;
                this.textBox15.ReadOnly = true;
                this.textBox16.ReadOnly = true;
                this.comboBox1.ReadOnly = true;
                this.checkBox1.ReadOnly = true;
                this.checkBox2.ReadOnly = true;
                this.checkBox3.ReadOnly = true;

            #endregion
            DataRow row = this.detailgrid.GetDataRow(this.detailgridbs.Position);
            if (MyUtility.Check.Empty(row))
            {
                string id = this.textBox1.Text;
                DataTable detailDt = (DataTable)this.detailgridbs.DataSource;

                //Row 1
                DataRow newRow1 = detailDt.NewRow();               
                newRow1["Type"] = "Accessory Items";
                newRow1["ID"] = id;
                detailDt.Rows.Add(newRow1);
                //Row 2
                DataRow newRow2 = detailDt.NewRow();
                newRow2["Type"] = "Accessory Items";
                newRow2["ID"] = id;
                detailDt.Rows.Add(newRow2);
                //Row 3
                DataRow newRow3 = detailDt.NewRow();
                newRow3["Type"] = "CutParts";
                newRow3["ID"] = id;
                detailDt.Rows.Add(newRow3);
                //Row 4
                DataRow newRow4 = detailDt.NewRow();
                newRow4["Type"] = "Garment";
                newRow4["Item"] = "First MD";
                newRow4["ID"] = id;
                detailDt.Rows.Add(newRow4);
                //Row 5
                DataRow newRow5 = detailDt.NewRow();
                newRow5["Type"] = "Garment";
                newRow5["Item"] = "If Open carton";
                newRow5["ID"] = id;
                detailDt.Rows.Add(newRow5);

        
            }           
        }

        // delete rows when the pk value is empty
        protected override DualResult ClickSavePre()
        {
            if (!MyUtility.Check.Empty(CurrentMaintain["ID"]))
            {
                DataTable detailDt = (DataTable)this.detailgridbs.DataSource;
                DataRow row = this.detailgrid.GetDataRow(this.detailgridbs.Position);
                
                int t =detailDt.Rows.Count;

                for (int i = t-1; i >= 0; i--)
                {
                    //MD.PK都要有值才能save
                    if (detailDt.Rows[i]["Result"].ToString()=="" ||detailDt.Rows[i]["Type"].ToString()==""||detailDt.Rows[i]["Item"].ToString()==""||detailDt.Rows[i]["Colorid"].ToString()=="")
                    {
                        //刪除
                        detailDt.Rows[i].Delete();
                    }
                    
         
                }
        
            }
            return Result.True;

        }
        //表頭combobox
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            /*  combox 固定值
            DataTable dtCategory= new DataTable();
            dtCategory.ColumnsStringAdd("Category");
            dtCategory.ColumnsStringAdd("Name");
            dtCategory.Rows.Add(new string[] { "B","Bulk" });
            dtCategory.Rows.Add(new string[] { "S", "Sample" });
            this.comboBox1.DataSource = dtCategory;
            this.comboBox1.DisplayMember = "Name";
            this.comboBox1.ValueMember = "Category";
            */

            DataTable dtCategory;
            Ict.DualResult cbResult;
            if (cbResult = DBProxy.Current.Select(null, " select ID,Name from DropDownList where type='category'", out dtCategory))
            {
                this.comboBox1.DataSource = dtCategory;
                this.comboBox1.DisplayMember = "Name";
                this.comboBox1.ValueMember = "ID";
            }
            else { ShowErr(cbResult); }

        }
                        


        // Button Finished
        private void button1_Click(object sender, EventArgs e)
        {
            
            DataRow row = this.detailgrid.GetDataRow(this.detailgridbs.Position);
            DialogResult buttonFinished = MyUtility.Msg.QuestionBox("Do you want to finished this order!", "Question", MessageBoxButtons.YesNo);
            //訊息方塊選擇"NO" MDClose清空
            if (buttonFinished == DialogResult.No)
            {
                
                DialogResult BackToMasterList = MyUtility.Msg.QuestionBox("Do you want to back to master list?", "Question", MessageBoxButtons.OKCancel);
                if (BackToMasterList == DialogResult.OK)
                {

                    string sqlCmdUpdate = "update orders  set MDClose= Null  where id=@MdID";
                    DataRow row1 = this.detailgrid.GetDataRow(this.detailgridbs.Position);
                    string sp1Value = row1["ID"].ToString();
                    List<SqlParameter> spam = new List<SqlParameter>();
                    spam.Add(new SqlParameter("@MdID", sp1Value));
                    DualResult result = DBProxy.Current.Execute("Production", sqlCmdUpdate,spam);
                    
                    if (!result) { return; }
                   // MyUtility.Msg.WarningBox("order.mdclose 清空");
                }
                else { return; }
            }
            //訊息方塊選擇YES, MDClose填入Date()
            else
            {
                string sqlCmdUpdate = "update orders  set MDClose= CONVERT(VARCHAR(20), GETDATE(), 120)  where id=@MdID";
                DataRow row1 = this.detailgrid.GetDataRow(this.detailgridbs.Position);
                string sp1Value = row1["ID"].ToString();
  
                List<SqlParameter> spam = new List<SqlParameter>();
                spam.Add(new SqlParameter("@MdID", sp1Value));
                DualResult result= DBProxy.Current.Execute("Production", sqlCmdUpdate,spam);       
                MyUtility.Msg.WarningBox("order.mdclose insert date");
            }
        }


       

    }
}
