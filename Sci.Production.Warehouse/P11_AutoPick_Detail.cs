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
using System.Threading;

namespace Sci.Production.Warehouse
{
    public partial class P11_AutoPick_Detail : Sci.Win.Subs.Base
    {
        DataRow dr_master;
        DataTable dt_detail;
        int _isgridcurrentchanging;
        protected Sci.Win.UI.ListControlBindingSource gridbs = new Win.UI.ListControlBindingSource();
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        bool flag;
        string poType;
        protected DataTable dtArtwork;

        public P11_AutoPick_Detail()
        {
            InitializeComponent();
            //dr_master = master;
            //dt_detail = detail;
            //gridbs.DataSource = dt_detail;
        }

        public void SetGrid(DataTable datas)
        {
            if (null != datas) gridbs.DataSource = null;
            dt_detail = datas;
            gridbs.DataSource = datas;
            gridbs.MoveFirst();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            
            this.grid1.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.grid1.DataSource = gridbs;
            Helper.Controls.Grid.Generator(this.grid1)
                .Text("sizecode", header: "Size", iseditingreadonly: true, width: Widths.AnsiChars(6))
                .Numeric("qty", header: "Qty", iseditable: true, decimal_places: 2, integer_places: 10)
                ;

            this.grid1.Columns[1].DefaultCellStyle.BackColor = Color.Pink; 
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.grid1.ValidateControl()) { return; }
            foreach (DataRow dr in dt_detail.Rows)
            {
                dr["ori_qty"] = decimal.Parse(dr["qty"].ToString());
            }
            dt_detail.AcceptChanges();
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            foreach (DataRow dr in dt_detail.Rows)
            {
                dr["qty"] = decimal.Parse(dr["ori_qty"].ToString());
            }
            dt_detail.AcceptChanges();
            this.Close();
        }
    }
}
