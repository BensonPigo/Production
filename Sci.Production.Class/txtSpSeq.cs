using Ict;
using Sci.Data;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace Sci.Production.Class
{
    public partial class txtSpSeq : Sci.Win.UI._UserControl
    {

        private string oldSP = string.Empty;
        private string oldSeq = string.Empty;

        public txtSpSeq()
        {
            InitializeComponent();
        }


        [Bindable(true)]
        public string TextBoxSPBinding
        {
            set
            {
                this.txtSp.Text = value;
                if (!Env.DesignTime)
                {
                    //if (this.textBox1.Text == "" || MyUtility.Check.Empty(this.textBox1.Text))
                    //{
                    //    return;
                    //}

                    //Sci.Production.Class.Commons.UserPrg.GetName(this.TextBox1.Text, out myUsername, Sci.Production.Class.Commons.UserPrg.NameType.nameAndExt);
                    //this.DisplayBox1.Text = myUsername;

                }
            }
            get { return txtSp.Text; }
        }
        [Bindable(true)]
        public string TextBoxSeqBinding
        {
            set
            {
                this.txtSeq.Text = value;
            }
            get { return txtSeq.Text; }
        }

        public Sci.Win.UI.TextBox TextBoxSP
        {
            get { return this.txtSp; }
        }

        public Sci.Win.UI.TextBox TextBoxSeq
        {
            get { return this.txtSeq; }
        }

        private void txtSp_Validating(object sender, CancelEventArgs e)
        {
            string OrderID = this.txtSp.Text;
            string Seq = this.txtSeq.Text;
            if (MyUtility.Check.Empty(OrderID))
            {
                return;
            }
            List<SqlParameter> paras = new List<SqlParameter>();
            paras.Add(new SqlParameter("@ID", OrderID));

            bool exists = MyUtility.Check.Seek($@"
SELECT 1 
FROM Orders o
INNER JOIN OrderType ot ON o.OrderTypeID = ot.ID AND o.BrandID = ot.BrandID
WHERE o.ID=@ID 
AND o.Category IN('B', 'S', 'G')
AND ISNULL(ot.IsGMTMaster,0) = 0
",paras);
            if (!exists)
            {
                MyUtility.Msg.InfoBox("Data not found!!");
                e.Cancel = true;
                return;
            }

            bool IsSameM = MyUtility.Check.Seek($"SELECT 1 FROM Orders WHERE ID=@ID AND MDivisionID = '{Sci.Env.User.Keyword}'", paras);

            if (!IsSameM)
            {
                MyUtility.Msg.InfoBox("MDivisionID is different!!");
                e.Cancel = true;
                return;
            }

            DataTable dt;
            DualResult result;

            paras.Add(new SqlParameter("@Seq", Seq));

            string cmd = string.Empty;


            cmd = $@"
SELECT ID , Seq
FROM Order_QtyShip
WHERE ID = @ID
";

            result = DBProxy.Current.Select(null, cmd, paras, out dt);

            if (result)
            {
                // = 0
                if (dt.Rows.Count == 0)
                {
                    if (!MyUtility.Check.Empty(Seq))
                    {
                        MyUtility.Msg.InfoBox("SP# & Seq not found !!");
                        e.Cancel = true;
                    }
                }
                // > 1
                else if (dt.Rows.Count > 1)
                {

                    Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(dt, "ID,Seq", "15,10", OrderID, "ID,Seq");
                    item.Width = 600;
                    DialogResult Dresult = item.ShowDialog();
                    if (Dresult == DialogResult.OK)
                    {
                        IList<DataRow> selectedDatas = item.GetSelecteds();
                        Seq = selectedDatas[0]["Seq"].ToString();
                        this.txtSeq.Text = Seq;
                    }
                }
                else if (dt.Rows.Count == 1)
                {
                    Seq = dt.Rows[0]["Seq"].ToString();
                    this.txtSeq.Text = Seq;
                }
            }
            else
            {
                string msg = "";

                foreach (var Message in result.Messages)
                {
                    msg += Message + "\r\n";
                }

                MyUtility.Msg.WarningBox("DB Query Error : " + msg);
            }

            // 強制把binding的Text寫到DataRow
            //this.DataBindings.Cast<Binding>().ToList().ForEach(binding => binding.WriteValue());
            //this.TextBoxSPBinding = this.txtSp.Text;
            //foreach (var binding in this.DataBindings.Cast<Binding>().ToList())
            //{

            //}
        }

        private void txtSeq_Validating(object sender, CancelEventArgs e)
        {
            string OrderID = this.txtSp.Text;
            string Seq = this.txtSeq.Text;
            if (MyUtility.Check.Empty(Seq))
            {
                return;
            }

            DataTable dt;
            DualResult result;
            List<SqlParameter> paras = new List<SqlParameter>();
            paras.Add(new SqlParameter("@ID", OrderID));
            paras.Add(new SqlParameter("@Seq", Seq));

            string cmd = string.Empty;

            // OrderID 為空，不觸發自動帶入/跳出視窗
            if (MyUtility.Check.Empty(OrderID))
            {

            }
            // OrderID 為空，觸發自動帶入/跳出視窗
            else
            {
                cmd = $@"
SELECT ID , Seq
FROM Order_QtyShip
WHERE ID = @ID AND Seq = @Seq
";
                result = DBProxy.Current.Select(null, cmd, paras, out dt);
                if (dt.Rows.Count == 0)
                {
                    MyUtility.Msg.InfoBox("SP# & Seq not found !!");
                    e.Cancel = true;
                }
            }



            // 強制把binding的Text寫到DataRow
            //this.DataBindings.Cast<Binding>().ToList().ForEach(binding => binding.WriteValue());

            foreach (var binding in this.DataBindings.Cast<Binding>().ToList())
            {

            }
        }

        private void txtSeq_Leave(object sender, System.EventArgs e)
        {
            string newSeq = this.txtSeq.Text;

            if (newSeq != this.oldSeq)
            {

                string OrderID = this.txtSp.Text;

                // SP Seq都不為空才驗證
                if (!MyUtility.Check.Empty(OrderID) && !MyUtility.Check.Empty(newSeq))
                {
                    DataTable dt;
                    DualResult result;
                    List<SqlParameter> paras = new List<SqlParameter>();
                    paras.Add(new SqlParameter("@ID", OrderID));
                    paras.Add(new SqlParameter("@Seq", newSeq));

                    string cmd = $@"
SELECT ID , Seq
FROM Order_QtyShip
WHERE ID = @ID AND Seq = @Seq
";
                    result = DBProxy.Current.Select(null, cmd, paras, out dt);
                    if (dt.Rows.Count == 0)
                    {
                        // 驗證失敗清空
                        MyUtility.Msg.InfoBox("SP# & Seq not found !!");
                        this.txtSeq.Text = string.Empty;
                        return;
                    }

                    //驗證通過
                    this.oldSeq = this.txtSeq.Text;
                }
                else
                {
                    this.oldSeq = newSeq;
                }
            }
        }

        private void txtSp_Leave(object sender, System.EventArgs e)
        {
            string newSp = this.txtSp.Text;


            if (newSp != this.oldSP)
            {
                string Seq = this.txtSeq.Text;

                // SP Seq都不為空才驗證
                if (!MyUtility.Check.Empty(newSp) && !MyUtility.Check.Empty(Seq))
                {
                    DataTable dt;
                    DualResult result;
                    List<SqlParameter> paras = new List<SqlParameter>();
                    paras.Add(new SqlParameter("@ID", newSp));
                    paras.Add(new SqlParameter("@Seq", Seq));

                    string cmd = $@"
SELECT ID , Seq
FROM Order_QtyShip
WHERE ID = @ID AND Seq = @Seq
";
                    result = DBProxy.Current.Select(null, cmd, paras, out dt);
                    if (dt.Rows.Count == 0)
                    {
                        // 驗證失敗清空
                        MyUtility.Msg.InfoBox("SP# & Seq not found !!");
                        this.txtSp.Text = string.Empty;
                    }


                    //驗證通過
                    this.oldSP = this.txtSp.Text;
                }
                else
                {
                    this.oldSP = newSp;
                }

            }
        }
    }

}
