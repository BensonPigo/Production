using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.PublicForm
{
    /// <inheritdoc/>
    public partial class MessageYESNO : Sci.Win.Tems.QueryForm
    {
        public bool isYN = false;
        private DataTable dt;

        /// <inheritdoc/>
        public MessageYESNO(string strMsg, DataTable dataTable, string titleName)
        {
            this.InitializeComponent();
            this.Text = titleName;
            this.displayBrand.Text = strMsg;
            this.dt = dataTable;
            this.dataGridView.AutoGenerateColumns = true;
            this.dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.DataSource = this.dt;
        }

        private void BtnYES_Click(object sender, EventArgs e)
        {
            this.isYN = true;
            this.Close();
        }

        private void BtnNO_Click(object sender, EventArgs e)
        {
            this.isYN = false;
            this.Close();
        }
    }
}
