using Ict;
using Ict.Win;
using Sci.Production.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Sci.Win;
using Sci.Data;
using System.Transactions;
using Sci.Win.Tools;
using Sci.Production.Quality;
namespace Sci.Production.Quality
{
    public partial class P02_Detail : Sci.Win.Subs.Input6A
    {
        
        public P02_Detail(string airID)
        {
            InitializeComponent();
            string id = airID;

            string air_cmd = string.Format("select * from air where id='{0}'", id);
            DataRow dr;

            //combox
            DataTable dtCombo;
            Ict.DualResult cbResult;
            if(cbResult= DBProxy.Current.Select(null, string.Format("select ID,Result from air WHERE ID='{0}'", id),out dtCombo))
            {
                this.comboBox1.DataSource=dtCombo;
                this.comboBox1.DisplayMember="Result";
                this.comboBox1.ValueMember="ID";
            }

            // 串接table Po_Supp_Detail
            DataTable dtPoSuppDetail;
            Ict.DualResult pstResult;
            if (pstResult=DBProxy.Current.Select(null,string.Format("select * from PO_Supp_Detail a left join AIR b on a.ID=b.POID and a.SEQ1=b.SEQ1 and a.SEQ2=b.SEQ2 where b.ID='{0}'",id),out dtPoSuppDetail ))
            {
                ref_text.Text = dtPoSuppDetail.Rows[0]["SCIRefno"].ToString();
                brand_text.Text = dtPoSuppDetail.Rows[0]["BrandID"].ToString();
                lable_color.Text = dtPoSuppDetail.Rows[0]["colorid"].ToString();
                //沒有欄位
                //unit_text.Text = dtPoSuppDetail.Rows[0]["wearunit"].ToString();
                size_text.Text = dtPoSuppDetail.Rows[0]["sizespec"].ToString();
            }
          
            //串接table Receiving
            DataTable dtRec;
            Ict.DualResult wknoResult;
            if (wknoResult = DBProxy.Current.Select(null, string.Format("select * from Receiving a left join AIR b on a.Id=b.ReceivingID where b.ID='{0}' ", id), out dtRec)) 
            {
                wkno_text.Text = dtRec.Rows[0]["exportid"].ToString();
            }
            
            if (MyUtility.Check.Seek(air_cmd,out dr))
            {
                seq_text.Text = dr["SEQ1"].ToString()+" - "+dr["SEQ2"].ToString();
                inspQty_text.Text = dr["inspQty"].ToString();
                RejQty_text.Text=dr["REjectQty"].ToString();
                InsDate_text.Text = Convert.ToDateTime(dr["inspdate"]).ToShortDateString();
                Instor_text.Text = dr["inspector"].ToString();
                Remark_text.Text = dr["remark"].ToString();
                this.comboBox1.DisplayMember = dr["Result"].ToString();
               

            }
            //右鍵帶出選擇視窗
            //DefectSelect = new DataGridViewGeneratorTextColumnSettings();
            //DefectSelect = new 

            //DefectSelect.CellMouseClick += (s1, e1) =>
            //{
            //    if (e1.Button == System.Windows.Forms.MouseButtons.Right)
            //    {
            //        DataRow dr1 = this.detailgrid.GetDataRow<DataRow>(e1.RowIndex);
            //        if (null == dr1) { return; }
            //        string sqlcmd1 = "    select scirefno from PO_Supp_Detail  where ID in (select id from orders)  and fabrictype='A'  and Scirefno is not null  group by scirefno";
            //        SelectItem item1 = new SelectItem(sqlcmd1, "30", dr1["Item"].ToString());
            //        DialogResult result1 = item1.ShowDialog();
            //        if (result1 == DialogResult.Cancel) { return; }
            //        dr1["Item"] = item1.GetSelectedString();
            //    }
            //};


        }

        protected override void OnFormDispose()
        {

            base.OnFormDispose();

            
        }



       
    }
}
