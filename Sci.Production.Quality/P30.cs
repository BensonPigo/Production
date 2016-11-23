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
using Ict.Win.UI;


namespace Sci.Production.Quality
{
    public partial class P30 : Sci.Win.Tems.Input6
    {

        bool ISEE = false;
        protected DataRow motherData;

        // (menuitem, args= 參數)
        public P30(ToolStripMenuItem menuitem,String history) : base(menuitem) 
        {
            InitializeComponent();

            ISEE = true;
            string mdClose=this.textBox8.Text;

            //設定init()
            string factoryId = Sci.Env.User.Factory;
            if (history == "1".ToString())
            {
                this.DefaultFilter = string.Format("FactoryId= '{0}' and MDClose is null and orders.IsForecast<>1", factoryId);
                this.Text = "P30 .MD Master List";
               
            }
            else if (history == "2".ToString())
            {
                this.DefaultFilter = string.Format("FactoryId= '{0}' and MDClose is not null", factoryId);
                this.Text = "P31 .MD Master List(History)";
                this.IsSupportEdit = false;

            }         
           
        }
        
        protected override void OnEditModeChanged()
        {                      
            base.OnEditModeChanged();
            if (ISEE)
            {
                btnFinished.Enabled = !EditMode;
            }
        }
        //public void Grid_editingControl(object sender, DataGridViewTextBoxEditingControlEventArgs e)
        //{
        //    this.
        //}
        //public void Grid_editing(System.Windows.Forms.MouseButtons eButton, int eRowIndex)
        //{
        //    DataGridViewTextBoxEditingControl edd = (DataGridViewTextBoxEditingControl)eButton.Control;
        //}
        public void colorSelect_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            this.colorSelect_CellMouseClick(e.Button, e.RowIndex);
        }
        public void colorSelect_CellMouseClick(object sender, DataGridViewEditingControlMouseEventArgs e)
        {
            this.colorSelect_CellMouseClick(e.Button, e.RowIndex);
        }
        public void colorSelect_CellMouseClick(System.Windows.Forms.MouseButtons eButton ,int eRowIndex){
            if (eButton == System.Windows.Forms.MouseButtons.Right)
            {
                
                DataRow dr = this.detailgrid.GetDataRow<DataRow>(eRowIndex);
                if (null == dr) { return; }
                if ((dr.ItemArray[1].ToString()).ToUpper() == "CUTPARTS" || (dr.ItemArray[1].ToString()).ToUpper() == "GARMENT")
                {
                    return;
                }
                string sqlcmd = string.Format(@" select colorid from po_supp_detail a,Orders b  where a.id=b.POID and a.fabrictype='A' and colorid is not null and b.id='{0}' group by colorid"
                    , textBox1.Text.ToString());
                SelectItem item = new SelectItem(sqlcmd, "30", dr["ColorID"].ToString());
                DialogResult result = item.ShowDialog();
                if (result == DialogResult.Cancel) { return; }
                dr["Colorid"] = item.GetSelectedString(); 
            }
        }

        public void itemSelect_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            this.itemSelect_CellMouseClick(e.Button, e.RowIndex);
        }
        public void itemSelect_CellMouseClick(object sender, DataGridViewEditingControlMouseEventArgs e)
        {
            this.itemSelect_CellMouseClick(e.Button, e.RowIndex);
        }
        public void itemSelect_CellMouseClick(System.Windows.Forms.MouseButtons eButton, int eRowIndex)
        {
            DataRow dr1 = this.detailgrid.GetDataRow<DataRow>(eRowIndex);
            if (dr1["Type"].ToString().ToUpper() != "ACCESSORY ITEMS")
            {
                return;
            }
            
            if (eButton == System.Windows.Forms.MouseButtons.Right)
            {
                if (!this.EditMode) return;
               // DataRow dr1 = this.detailgrid.GetDataRow<DataRow>(eRowIndex);
                if (null == dr1) { return; }
                if ((dr1.ItemArray[1].ToString()).ToUpper() == "CUTPARTS" || (dr1.ItemArray[1].ToString()).ToUpper() == "GARMENT")
                {
                    return;
                }
                string sqlcmd1 = string.Format(@"select distinct refno from PO_Supp_Detail a,Orders b where a.id=b.POID and a.fabrictype='A'
                and a.Scirefno is not null 
                and b.id='{0}' group by a.refno"
                    ,textBox1.Text.ToString());
                SelectItem item1 = new SelectItem(sqlcmd1, "30", dr1["Item"].ToString());
                DialogResult result1 = item1.ShowDialog();
                if (result1 == DialogResult.Cancel) { return; }
                dr1["Item"] = item1.GetSelectedString();
            }
        }
        // 設定Grid內容值
        protected override void OnDetailGridSetup()
        {
            #region OnClick Right Click Even
            DataGridViewGeneratorTextColumnSettings colorSelect = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings itemSelect = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings typeSetting = new DataGridViewGeneratorTextColumnSettings();
            typeSetting.CharacterCasing = CharacterCasing.Normal;
            colorSelect.CellMouseClick += this.colorSelect_CellMouseClick;
            colorSelect.EditingMouseDown += this.colorSelect_CellMouseClick;
            itemSelect.CellEditable += (s, e) =>
            {
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                if (dr["Type"].ToString().ToUpper() == "ACCESSORY ITEMS")
                {
                    e.IsEditable = true;
                }
                else
                {
                    e.IsEditable = false;
                }
            
            };

            itemSelect.CellValidating += (s, e) =>
            {
                if (this.EditMode == false) return;
                if (MyUtility.Check.Empty(e.FormattedValue)) return;
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                DataRow dr1;
                string sqlcmd = string.Format(@"select  refno from PO_Supp_Detail a,Orders b where a.id=b.POID and a.fabrictype='A'
                and a.Scirefno is not null 
                and b.id='{0}' and a.refno='{1}'"
                   , textBox1.Text.ToString(),e.FormattedValue);

                if (MyUtility.Check.Seek(sqlcmd,out dr1))
                {
                    dr["Item"] = e.FormattedValue;
                }
                else
                {
                    MyUtility.Msg.InfoBox(e.FormattedValue + " does not exist!");
                    dr["Item"] = "";
                    dr.EndEdit();
                    e.Cancel = true; return;
                }

            };
            colorSelect.CellEditable += (s, e) =>
            {
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
               
                if (dr["Type"].ToString().ToUpper() == "ACCESSORY ITEMS")
                {
                    e.IsEditable = true;
                }
                else
                {
                    e.IsEditable = false;
                }
            };

            colorSelect.CellValidating += (s, e) =>
            {
                if (this.EditMode == false) return;
                if (MyUtility.Check.Empty(e.FormattedValue)) return;
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                DataRow dr1;

                string sqlcmd = string.Format(@" select colorid from po_supp_detail a,Orders b  
                where a.id=b.POID and a.fabrictype='A' 
                and colorid is not null  and b.id='{0}' and a.colorid='{1}'"
                  , textBox1.Text.ToString(), e.FormattedValue);
                if (MyUtility.Check.Seek(sqlcmd,out dr1))
                {
                    dr["Colorid"] = e.FormattedValue;
                }
                else
                {
                    MyUtility.Msg.InfoBox(e.FormattedValue + " does not exist!");
                    dr["Colorid"] = "";
                    dr.EndEdit();
                    e.Cancel = true; return;
                }
            };


            //colorSelect.EditingMouseDown

            itemSelect.CellMouseClick += this.itemSelect_CellMouseClick;
            itemSelect.EditingMouseDown += this.itemSelect_CellMouseClick;                     
      
            #endregion                         
             
            // 定義一個 class member
            Ict.Win.UI.DataGridViewComboBoxColumn cbb_MD_type;

            //設定Grid屬性 text 對應欄位值 header :顯示欄位名稱

            Helper.Controls.Grid.Generator(this.detailgrid)               
            .Text("Type", header: "Main Item NO", width: Ict.Win.Widths.AnsiChars(15), iseditingreadonly: true, iseditable: true, settings: typeSetting)
            .Text("Item", header: "SEQ Ref", width: Ict.Win.Widths.AnsiChars(15),settings: itemSelect)
            .Text("Colorid", header: "Color", width: Ict.Win.Widths.AnsiChars(10),settings: colorSelect)
            .Date("inspdate", header: "Inspdate", width: Ict.Win.Widths.AnsiChars(10))
            .Text("Result", header: "Result", width: Ict.Win.Widths.AnsiChars(20));
            detailgrid.ValidateControl();
      
        }

        
        // When click Edit button and Grid is empty then New 5 column in GridView
        //choice ClickEditAfter becauser Transaction problemm
        protected override void ClickEditAfter()
        {
            
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
                this.DetailDatas.Add(newRow1);
                //Row 2
                DataRow newRow2 = detailDt.NewRow();
                newRow2["Type"] = "Accessory Items";
                newRow2["ID"] = id;
                detailDt.Rows.Add(newRow2);
                this.DetailDatas.Add(newRow2);
                //Row 3
                DataRow newRow3 = detailDt.NewRow();
                newRow3["Type"] = "CutParts";
                newRow3["ID"] = id;
                detailDt.Rows.Add(newRow3);
                this.DetailDatas.Add(newRow3);
                //Row 4
                DataRow newRow4 = detailDt.NewRow();
                newRow4["Type"] = "Garment";
                newRow4["Item"] = "First MD";
                newRow4["ID"] = id;
                detailDt.Rows.Add(newRow4);
                this.DetailDatas.Add(newRow4);
                //Row 5
                DataRow newRow5 = detailDt.NewRow();
                newRow5["Type"] = "Garment";
                newRow5["Item"] = "If Open carton";
                newRow5["ID"] = id;
                detailDt.Rows.Add(newRow5);
                this.DetailDatas.Add(newRow5);

            }
            base.ClickEditAfter();        
        }

        protected override void OnDetailGridInsert(int index = -1)
        {
            base.OnDetailGridInsert(index);
            DataTable dt = (DataTable)detailgridbs.DataSource;

            CurrentDetailData["Type"] = "Accessory items";         

        }
        
        protected override void OnDetailGridDelete()
        {

            if (this.CurrentDetailData["Type"].StrStartsWith("Cutparts") || this.CurrentDetailData["Type"].StrStartsWith("Garment"))
            {
                MyUtility.Msg.WarningBox("If <Main item no> is Cutparts or Garment cannot delete");
                return;
            }
            base.OnDetailGridDelete();
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
                    if (                       
                        (detailDt.Rows[i]["Type"].ToString().ToUpper() == "ACCESSORY ITEMS" && detailDt.Rows[i]["Item"].ToString() == ""))
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

        protected override bool ClickSaveBefore()
        {

            int a = this.detailgrid.Rows.Count;
            var tab = (DataTable)this.detailgridbs.DataSource;
            int b = tab.Rows.Count;
            DualResult upResult = new DualResult(true);
                  
                DataTable detailDt = (DataTable)this.detailgridbs.DataSource;
                DataRow row = this.detailgrid.GetDataRow(this.detailgridbs.Position);

                int t = detailDt.Rows.Count;

                for (int i = t - 1; i >= 0; i--)
                {
                    if (MyUtility.Check.Empty(detailDt.Rows[i]["ID"].ToString()))
                    {
                        MyUtility.Msg.InfoBox("<ID> cannot be null !");
                        return false;
                    }                    
                    if (MyUtility.Check.Empty(detailDt.Rows[i]["Type"].ToString()))
                    {
                        MyUtility.Msg.InfoBox("<Main Item NO> cannot be null !");
                        return false;
                    }
                }
            return base.ClickSaveBefore();
        }
           
            
        protected override DualResult ClickSave()
        {
                     
            return base.ClickSave();
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
                string Today = DateTime.Now.ToShortDateString();
                string sqlCmdUpdate = "update orders  set MDClose= CONVERT(VARCHAR(20), GETDATE(), 120)  where id=@MdID";
                DataRow row1 = this.detailgrid.GetDataRow(this.detailgridbs.Position);
                string sp1Value;
                if (MyUtility.Check.Empty(row1))
                {
                     sp1Value = "";
                }
                else
                {
                     sp1Value = row1["ID"].ToString();
                }
                
  
                List<SqlParameter> spam = new List<SqlParameter>();
                spam.Add(new SqlParameter("@MdID", sp1Value));
                DualResult result= DBProxy.Current.Execute("Production", sqlCmdUpdate,spam);
                this.textBox8.Value = MyUtility.Convert.GetDate( Today);
                //MyUtility.Msg.WarningBox("order.mdclose insert date");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int a = this.detailgrid.Rows.Count;
            var tab = (DataTable)this.detailgridbs.DataSource;
            int b = tab.Rows.Count;
        }


       

    }
}
