using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using System.Data.SqlClient;
using Sci.Data;


namespace Sci.Production.Quality
{
    public partial class P07_Oven : Sci.Win.Subs.Input2A
    {
        private DataRow maindr;
        private string ID, PoID, SEQ1, SEQ2;
        private bool EDIT;
        string sql;
        DualResult result;
        DataTable dtTemp;
        DataRow DR;


        public P07_Oven(bool canedit, string id, string Poid, string seq1, string seq2, DataRow mainDr)
        {
            InitializeComponent();
            maindr = mainDr;
            SetUpdate(maindr);
            ID = id.Trim();
            PoID = Poid.Trim();
            SEQ1 = seq1.Trim();
            SEQ2 = seq2.Trim();
            if (!canedit)
            {
                EDIT = false;
                this.save.Enabled = false;
            }
            else
            {
                EDIT = true;
            }

        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            #region [comboResult]
            Dictionary<String, String> Result_RowSource = new Dictionary<string, string>();
            Result_RowSource.Add("", "");
            Result_RowSource.Add("Pass", "Pass");
            Result_RowSource.Add("Fail", "Fail");
            comboResult.DataSource = new BindingSource(Result_RowSource, null);
            comboResult.ValueMember = "Key";
            comboResult.DisplayMember = "Value";
            #endregion

            OnRequery();
        }

        private void OnRequery()
        {
             sql = string.Format(@"select  C.ExportId , B.ArriveQty , E.StockUnit , E.SizeSpec , B.SCIRefno
	                                    , B.Refno , B.Suppid + '-' + D.AbbEN as supplier , E.ColorID
                                    from AIR_Laboratory A
                                    left join AIR B on A.id=B.id
                                    left join Receiving C on C.id=B.receivingID
                                    left join Supp D on D.ID=B.Suppid
                                    left join PO_Supp_Detail E on E.ID=A.POID and E.SEQ1=A.SEQ1 and E.SEQ2=A.SEQ2
                                    where A.id={0} and A.POID='{1}' and A.SEQ1='{2}' and A.SEQ2='{3}'", ID,PoID,SEQ1,SEQ2);
            if (MyUtility.Check.Seek(sql, out DR))
            {
                sp_text.Text = maindr["SEQ1"] + "-" + maindr["SEQ2"];
                WKNO_text.Text = DR["ExportId"].ToString().Trim();
                Qty_text.Value = Convert.ToDecimal(DR["ArriveQty"]);
                Unit_text.Text = DR["StockUnit"].ToString().Trim();
                Size_text.Text = DR["SizeSpec"].ToString().Trim();
                SciRefno.Text = DR["SCIRefno"].ToString().Trim();
                BrandRefno_text.Text = DR["Refno"].ToString().Trim();
                Supplier_text.Text = DR["supplier"].ToString().Trim();
                Color_text.Text = DR["ColorID"].ToString().Trim();
            }

            comboResult.SelectedValue2 = maindr["Oven"].ToString().Trim();
            //txtuser1.TextBox1Binding = "MIS";


        }











    }
}
