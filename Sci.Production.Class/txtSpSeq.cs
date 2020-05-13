using Ict;
using Sci.Data;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
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
                this.TextBoxSP.Text = value;
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
            get { return TextBoxSP.Text; }
        }
        [Bindable(true)]
        public string TextBoxSeqBinding
        {
            set
            {
                this.TextBoxSeq.Text = value;
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
            get { return TextBoxSeq.Text; }
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

            DataTable dt;
            DualResult result;
            List<SqlParameter> paras = new List<SqlParameter>();
            paras.Add(new SqlParameter("@ID", OrderID));
            paras.Add(new SqlParameter("@Seq", Seq));

            string cmd = string.Empty;

            // Seq 為空，觸發自動帶入/跳出視窗
            if (MyUtility.Check.Empty(Seq))
            {

                cmd = $@"
SELECT ID , Seq
FROM Order_QtyShip
WHERE ID = @ID
";

                result = DBProxy.Current.Select(null, cmd, paras, out dt);
                if (result)
                {
                    // = 1
                    if (dt.Rows.Count == 1 & dt.Rows.Count > 0)
                    {
                        Seq = dt.Rows[0]["Seq"].ToString();
                        this.txtSeq.Text = Seq;
                        return;
                    }
                    // > 1
                    else if (dt.Rows.Count > 1)
                    {

                        Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(dt, "ID,Seq", "15,10", OrderID, "ID,Seq");
                        item.Width = 600;
                        DialogResult Dresult = item.ShowDialog();
                        if (Dresult == DialogResult.Cancel) { return; }


                        IList<DataRow> selectedDatas = item.GetSelecteds();


                        Seq = selectedDatas[0]["Seq"].ToString();
                        this.txtSeq.Text = Seq;
                        return;
                    }
                    // 因為 Seq 為空，因此不驗證
                    else
                    {
                        //MyUtility.Msg.InfoBox("SP# not found !!");
                        //e.Cancel = true;
                        //return;
                    }
                }
            }
            // Seq 為空，觸發自動帶入/跳出視窗
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
                    return;
                }
            }
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
                /*
                cmd = $@"
SELECT ID , Seq
FROM Order_QtyShip
WHERE ID = @ID
";

                result = DBProxy.Current.Select(null, cmd, paras, out dt);
                if (result)
                {
                    // = 1
                    if (dt.Rows.Count == 1 & dt.Rows.Count > 0)
                    {
                        Seq = dt.Rows[0]["Seq"].ToString();
                        this.txtSeq.Text = Seq;
                        return;
                    }
                    // > 1
                    else if (dt.Rows.Count > 1)
                    {

                        Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(dt, "ID,Seq", "15,10", OrderID, "ID,Seq");
                        item.Width = 600;
                        DialogResult Dresult = item.ShowDialog();
                        if (Dresult == DialogResult.Cancel) { return; }


                        IList<DataRow> selectedDatas = item.GetSelecteds();


                        Seq = selectedDatas[0]["Seq"].ToString();
                        this.txtSeq.Text = Seq;
                        return;
                    }
                    // 因為 Seq 為空，因此不驗證
                    else
                    {
                        //MyUtility.Msg.InfoBox("SP# not found !!");
                        //e.Cancel = true;
                        //return;
                    }
                }
                */
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
                    return;
                }
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
