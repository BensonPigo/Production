using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Microsoft.Office.Interop.Excel;
using Sci.Data;
using DataTable = System.Data.DataTable;

namespace Sci.Production.IE
{
    /// <summary>
    /// P01_SelectOperationCode
    /// </summary>
    public partial class P01_PartID : Win.Subs.Base
    {
        private DataTable gridData;
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;

        private DataRow _P01SelectPartID;
        private string _MoldID;
        private string _SewingMachineAttachmentID;

        /// <summary>
        /// p01SelectOperationCode
        /// </summary>
        public DataRow P01SelectPartID
        {
            get
            {
                return this._P01SelectPartID;
            }

            set
            {
                this._P01SelectPartID = value;
            }
        }

        /// <summary>
        /// P01_SelectOperationCode
        /// </summary>
        public P01_PartID(string moldID ,string sewingMachineAttachmentID)
        {
            this._MoldID = moldID;
            this._SewingMachineAttachmentID = sewingMachineAttachmentID;
            this.InitializeComponent();
        }

        /// <summary>
        /// OnFormLoaded
        /// </summary>
        protected override void OnFormLoaded()
        {
            DataGridViewGeneratorTextColumnSettings s1 = new DataGridViewGeneratorTextColumnSettings();
            base.OnFormLoaded();
            this.gridDetail.IsEditingReadOnly = false;
            this.gridDetail.DataSource = this.listControlBindingSource1;

            s1.CellMouseDoubleClick += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    DataGridViewSelectedRowCollection selectRows = this.gridDetail.SelectedRows;
                    foreach (DataGridViewRow datarow in selectRows)
                    {
                        this._P01SelectPartID = ((DataRowView)datarow.DataBoundItem).Row;
                        this.DialogResult = DialogResult.Cancel;
                    }
                }
            };

            this.Helper.Controls.Grid.Generator(this.gridDetail)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
                .Text("ID", header: "ID", width: Widths.AnsiChars(25), iseditingreadonly: true)
                 .Text("Description", header: "Description English", width: Widths.AnsiChars(35), iseditingreadonly: true)
                 .Text("MachineMasterGroupID", header: "Machine", width: Widths.AnsiChars(15), iseditingreadonly: true)
                 .Text("AttachmentTypeID", header: "Type", width: Widths.AnsiChars(15), iseditingreadonly: true)
                 .Text("MeasurementID", header: "Measurement", width: Widths.AnsiChars(15), iseditingreadonly: true)
                 .Text("FoldTypeID", header: "Direction/Fold Type", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 ;

            string selected = string.Empty;

            List<SqlParameter> paras = new List<SqlParameter>();
            if (MyUtility.Check.Empty(this._SewingMachineAttachmentID))
            {
                selected = $@"0";
            }
            else
            {
                List<string> paraList = new List<string>();
                int idx = 0;
                foreach (var item in this._SewingMachineAttachmentID.Split(','))
                {
                    paraList.Add($@"@item{idx}");
                    paras.Add(new SqlParameter($@"@item{idx}", item));
                    idx++;
                }

                selected = $@"
( IIF( a.ID IN ({string.Join(",", paraList)})  , 1 , 0) )
";
            }

            string sqlCmd = $@"
select Selected = CAST( {selected} as bit)
    ,a.ID
    ,a.Description
    ,a.MachineMasterGroupID
    ,AttachmentTypeID
    ,MeasurementID
    ,FoldTypeID
from SewingMachineAttachment a
left join AttachmentType b on a.AttachmentTypeID = b.Type 
left join AttachmentMeasurement c on a.MeasurementID = c.Measurement
left join AttachmentFoldType d on a.FoldTypeID = d.FoldType 
where 1 = 1
";
            if (this._MoldID.Split(',').Length > 1)
            {
                sqlCmd += $@"and MoldID IN ('{string.Join("','", this._MoldID.Split(','))}') ";
            }
            else
            {
                paras.Add(new SqlParameter($@"@MoldID", this._MoldID));
                sqlCmd += "and MoldID = @MoldID ";
            }

            DualResult result = DBProxy.Current.Select(null, sqlCmd, paras, out this.gridData);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query SewingMachineAttachment fail\r\n" + result.ToString());
            }

            this.listControlBindingSource1.DataSource = this.gridData;
        }

        // Find
        private void BtnFind_Click(object sender, EventArgs e)
        {
            StringBuilder filterCondition = new StringBuilder();
            List<SqlParameter> paras = new List<SqlParameter>();

            List<string> descList = new List<string>();
            if (!string.IsNullOrEmpty(this.txtDescription.Text))
            {
                string tmp = this.txtDescription.Text.Replace("'", "''");
                descList = tmp.Split(';').Where(o => !string.IsNullOrEmpty(o)).ToList();

                //for (int i = 0; i <= descList.Count - 1; i++)
                //{
                //    if (descList[i] == "'")
                //    {
                //        descList[i] = "''";
                //    }
                //}
            }

            if (!MyUtility.Check.Empty(this.txtType.Text))
            {
                filterCondition.Append($@" and ID like '%{this.txtType.Text.Trim()}%' ");
            }

            if (!MyUtility.Check.Empty(this.txtMeasurement.Text))
            {
                filterCondition.Append($@" and MeasurementID like '%{this.txtMeasurement.Text.Trim()}%' ");
            }

            if (!MyUtility.Check.Empty(this.txtMachine.Text))
            {
                filterCondition.Append($@" and MachineMasterGroupID like '%{this.txtMachine.Text.Trim()}%' ");
            }

            if (!MyUtility.Check.Empty(this.txtDirectionFoldType.Text))
            {
                filterCondition.Append($@" and FoldTypeID like '%{this.txtDirectionFoldType.Text.Trim()}%' ");
            }

            if (descList.Any())
            {
                filterCondition.Append($@" and (Description like '%{string.Join("%' and Description like '%", descList)}%' ) ");
            }

            if (filterCondition.Length > 0)
            {
                string filter = filterCondition.ToString();
                this.gridData.DefaultView.RowFilter = "1=1" + filter;
            }
            else
            {
                this.gridData.DefaultView.RowFilter = string.Empty;
            }

            this.gridDetail.AutoResizeColumns();
        }

        // Select
        private void BtnSelect_Click(object sender, EventArgs e)
        {
            if (this.gridDetail.SelectedRows.Count == 0)
            {
                MyUtility.Msg.WarningBox("message：Please select an Attachment Part!");
                return;
            }

            // 取出打勾的DataRow
            DataRow[] selectedDataRows = this.gridData.Select("Selected = 1");

            DataGridViewSelectedRowCollection selectRows = this.gridDetail.SelectedRows;
            foreach (DataGridViewRow datarow in selectRows)
            {
                this._P01SelectPartID = ((DataRowView)datarow.DataBoundItem).Row;

                // 把勾選的值整理過，塞進ID裡面讓外層可以抓到
                this._P01SelectPartID["ID"] = string.Join(",", selectedDataRows.Select(o => o["ID"]).ToList());
            }

            this.DialogResult = DialogResult.OK;
        }

        // Select
        private void gridDetail_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //if (MyUtility.Check.Empty(this.gridDetail.DataSource))
            //{
            //    return;
            //}

            //if (e.ColumnIndex == 0)
            //{

            //    //foreach (DataRow dr in ((DataTable)this.gridDetail.DataSource).Rows)
            //    //{
            //    //    if (dr.RowState != DataRowState.Deleted)
            //    //    {
            //    //        dr["Selected"] = 0;
            //    //    }
            //    //}

            //}

        }
    }
}
