using System;
using System.Data;
using System.Windows.Forms;
using Ict;
using Sci.Data;

namespace Sci.Production.PublicForm
{
    /// <inheritdoc/>
    public partial class EditRemark : Win.Subs.Base
    {
        private string _tableNm;
        private string _columnNm;
        private DataRow dr;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditRemark"/> class.
        /// </summary>
        /// <param name="tableNm">tableNm</param>
        /// <param name="columnNm">columnNm</param>
        /// <param name="data">DataRow</param>
        public EditRemark(string tableNm, string columnNm, DataRow data)
        {
            this.InitializeComponent();

            this._tableNm = tableNm;
            this._columnNm = columnNm;
            this.dr = data;

            this.edit_Remark.Text = this.dr[columnNm].ToString();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            string sqlcmd = string.Empty;
            string sqlwhere = string.Empty;
            DualResult result;
            ITableSchema tbs;
            if (!(result = DBProxy.Current.GetTableSchema(null, this._tableNm, out tbs)))
            {
                this.ShowErr(this._tableNm, result);
                return;
            }

            if (tbs.PKs.Count == 0)
            {
                MessageBox.Show(string.Format("Table:{0} without PK, can't update!!", this._tableNm), "Warrning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            for (int i = 0; i < tbs.PKs.Count; i++)
            {
                if (i == 0)
                {
                    if (tbs.PKs[i].ColumnType.ToString().ToUpper() == "STRING")
                    {
                        sqlwhere = string.Format(" where {0} = '{1}'", tbs.PKs[i].ColumnName, this.dr[tbs.PKs[i].ColumnName]);
                    }
                    else
                    {
                        sqlwhere = string.Format(" where {0} = {1}", tbs.PKs[i].ColumnName, this.dr[tbs.PKs[i].ColumnName]);
                    }
                }
                else
                {
                    if (tbs.PKs[i].ColumnType.ToString().ToUpper() == "STRING")
                    {
                        sqlwhere = sqlwhere + string.Format(" and {0} = '{1}'", tbs.PKs[i].ColumnName, this.dr[tbs.PKs[i].ColumnName]);
                    }
                    else
                    {
                        sqlwhere = sqlwhere + string.Format(" and {0} = {1}", tbs.PKs[i].ColumnName, this.dr[tbs.PKs[i].ColumnName]);
                    }
                }
            }

            sqlcmd = string.Format("update {0} set {1} = '{2}'", this._tableNm, this._columnNm, this.edit_Remark.Text) + sqlwhere;
            if (!(result = DBProxy.Current.Execute(null, sqlcmd)))
            {
                this.ShowErr(sqlcmd, result);
            }
            else
            {
                this.Close();
            }
        }
    }
}
