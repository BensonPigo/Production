using Ict;
using Sci.Data;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Sci.Production.Class
{
    /// <summary>
    /// TxtSpSeq
    /// </summary>
    public partial class TxtSpSeq : Sci.Win.UI._UserControl
    {
        private string oldSP = string.Empty;
        private string oldSeq = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="TxtSpSeq"/> class.
        /// </summary>
        public TxtSpSeq()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// SP Binding
        /// </summary>
        [Bindable(true)]
        public string TextBoxSPBinding
        {
            get
            {
                return this.TextBoxSP.Text;
            }

            set
            {
                this.TextBoxSP.Text = value;
            }
        }

        /// <summary>
        /// Seq Binding
        /// </summary>
        [Bindable(true)]
        public string TextBoxSeqBinding
        {
            get
            {
                return this.TextBoxSeq.Text;
            }

            set
            {
                this.TextBoxSeq.Text = value;
            }
        }

        /// <summary>
        /// SP
        /// </summary>
        public Win.UI.TextBox TextBoxSP { get; private set; }

        /// <summary>
        /// Seq
        /// </summary>
        public Win.UI.TextBox TextBoxSeq { get; private set; }

        private void TxtSp_Validating(object sender, CancelEventArgs e)
        {
            string orderID = this.TextBoxSP.Text;
            string seq = this.TextBoxSeq.Text;
            if (MyUtility.Check.Empty(orderID))
            {
                return;
            }

            List<SqlParameter> paras = new List<SqlParameter>
            {
                new SqlParameter("@ID", orderID),
            };

            bool exists = MyUtility.Check.Seek(
                $@"
SELECT 1 
FROM Orders o
WHERE o.ID=@ID 
AND o.Category IN('B', 'S', 'G')",
                paras);
            if (!exists)
            {
                MyUtility.Msg.InfoBox("Data not found!!");
                e.Cancel = true;
                return;
            }

            bool isSameM = MyUtility.Check.Seek($"SELECT 1 FROM Orders WHERE ID=@ID AND FtyGroup = '{Env.User.Factory}'", paras);

            if (!isSameM)
            {
                MyUtility.Msg.InfoBox("Factory is different!!");
                e.Cancel = true;
                return;
            }

            DualResult result;

            paras.Add(new SqlParameter("@Seq", seq));

            string cmd = string.Empty;

            cmd = $@"
SELECT ID , Seq
FROM Order_QtyShip
WHERE ID = @ID
";

            result = DBProxy.Current.Select(null, cmd, paras, out DataTable dt);

            if (result)
            {
                if (dt.Rows.Count == 0)
                {
                    // = 0
                    if (!MyUtility.Check.Empty(seq))
                    {
                        MyUtility.Msg.InfoBox("SP# & Seq not found !!");
                        e.Cancel = true;
                    }
                }
                else if (dt.Rows.Count > 1)
                {
                    // > 1
                    Win.Tools.SelectItem item = new Win.Tools.SelectItem(dt, "ID,Seq", "15,10", orderID, "ID,Seq")
                    {
                        Width = 600,
                    };
                    DialogResult dresult = item.ShowDialog();
                    if (dresult == DialogResult.OK)
                    {
                        IList<DataRow> selectedDatas = item.GetSelecteds();
                        seq = selectedDatas[0]["Seq"].ToString();
                        this.TextBoxSeq.Text = seq;
                    }
                }
                else if (dt.Rows.Count == 1)
                {
                    seq = dt.Rows[0]["Seq"].ToString();
                    this.TextBoxSeq.Text = seq;
                }
            }
            else
            {
                string msg = string.Empty;

                foreach (var message in result.Messages)
                {
                    msg += message + "\r\n";
                }

                MyUtility.Msg.WarningBox("DB Query Error : " + msg);
            }
        }

        private void TxtSeq_Validating(object sender, CancelEventArgs e)
        {
            string orderID = this.TextBoxSP.Text;
            string seq = this.TextBoxSeq.Text;
            if (MyUtility.Check.Empty(seq))
            {
                return;
            }

            DualResult result;
            List<SqlParameter> paras = new List<SqlParameter>
            {
                new SqlParameter("@ID", orderID),
                new SqlParameter("@Seq", seq),
            };

            string cmd = string.Empty;
            if (MyUtility.Check.Empty(orderID))
            {
                // OrderID 為空，不觸發自動帶入/跳出視窗
            }
            else
            {
                // OrderID 為空，觸發自動帶入/跳出視窗
                cmd = $@"
SELECT ID , Seq
FROM Order_QtyShip
WHERE ID = @ID AND Seq = @Seq
";
                result = DBProxy.Current.Select(null, cmd, paras, out DataTable dt);
                if (dt.Rows.Count == 0)
                {
                    MyUtility.Msg.InfoBox("SP# & Seq not found !!");
                    e.Cancel = true;
                }
            }
        }

        private void TxtSeq_Leave(object sender, System.EventArgs e)
        {
            string newSeq = this.TextBoxSeq.Text;

            if (newSeq != this.oldSeq)
            {
                string orderID = this.TextBoxSP.Text;

                // SP Seq都不為空才驗證
                if (!MyUtility.Check.Empty(orderID) && !MyUtility.Check.Empty(newSeq))
                {
                    DualResult result;
                    List<SqlParameter> paras = new List<SqlParameter>
                    {
                        new SqlParameter("@ID", orderID),
                        new SqlParameter("@Seq", newSeq),
                    };

                    string cmd = $@"
SELECT ID , Seq
FROM Order_QtyShip
WHERE ID = @ID AND Seq = @Seq
";
                    result = DBProxy.Current.Select(null, cmd, paras, out DataTable dt);
                    if (dt.Rows.Count == 0)
                    {
                        // 驗證失敗清空
                        MyUtility.Msg.InfoBox("SP# & Seq not found !!");
                        this.TextBoxSeq.Text = string.Empty;
                        return;
                    }

                    // 驗證通過
                    this.oldSeq = this.TextBoxSeq.Text;
                }
                else
                {
                    this.oldSeq = newSeq;
                }
            }
        }

        private void TxtSp_Leave(object sender, System.EventArgs e)
        {
            string newSp = this.TextBoxSP.Text;
            if (newSp != this.oldSP)
            {
                string seq = this.TextBoxSeq.Text;

                // SP Seq都不為空才驗證
                if (!MyUtility.Check.Empty(newSp) && !MyUtility.Check.Empty(seq))
                {
                    DualResult result;
                    List<SqlParameter> paras = new List<SqlParameter>
                    {
                        new SqlParameter("@ID", newSp),
                        new SqlParameter("@Seq", seq),
                    };

                    string cmd = $@"
SELECT ID , Seq
FROM Order_QtyShip
WHERE ID = @ID AND Seq = @Seq
";
                    result = DBProxy.Current.Select(null, cmd, paras, out DataTable dt);
                    if (dt.Rows.Count == 0)
                    {
                        // 驗證失敗清空
                        MyUtility.Msg.InfoBox("SP# & Seq not found !!");
                        this.TextBoxSP.Text = string.Empty;
                    }

                    // 驗證通過
                    this.oldSP = this.TextBoxSP.Text;
                }
                else
                {
                    this.oldSP = newSp;
                }
            }
        }
    }
}
