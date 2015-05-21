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

namespace Sci.Production.PublicForm
{
    /// <summary>
    /// EditRemark
    /// </summary>
    /// <param name="strTableNm"></param>
    /// <param name="strColumnNm"></param>
    /// <param name="drData"></param>
    public partial class EditRemark : Sci.Win.Subs.Base
    {
        string _tableNm, _columnNm ;
        DataRow dr;
        public EditRemark(string tableNm, string columnNm, DataRow Data)
        {
            InitializeComponent();

            _tableNm = tableNm;
            _columnNm = columnNm;
            dr = Data;

            editBox1.Text = dr[columnNm].ToString();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string sqlcmd = "";
            string sqlwhere = "";
            DualResult result;
            ITableSchema tbs;
            if (!(result = DBProxy.Current.GetTableSchema(null,_tableNm,out  tbs)))
            {
                ShowErr(_tableNm,result);
                return;
            }
            
            if (tbs.PKs.Count==0)
            {
                MessageBox.Show(string.Format("Table:{0} without PK, can't update!!",_tableNm),"Warrning",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                return;
            }

            for (int i=0; i < tbs.PKs.Count; i++)
            {
                if (i == 0)
                {
                    if (tbs.PKs[i].ColumnType.ToString().ToUpper()=="STRING")
                        sqlwhere = string.Format(" where {0} = '{1}'", tbs.PKs[i].ColumnName, dr[tbs.PKs[i].ColumnName]);
                    else
                        sqlwhere = string.Format(" where {0} = {1}", tbs.PKs[i].ColumnName, dr[tbs.PKs[i].ColumnName]);
                }
                else
                {
                    if (tbs.PKs[i].ColumnType.ToString().ToUpper() == "STRING")
                        sqlwhere = sqlwhere + string.Format(" and {0} = '{1}'", tbs.PKs[i].ColumnName, dr[tbs.PKs[i].ColumnName]);
                    else
                        sqlwhere = sqlwhere + string.Format(" and {0} = {1}", tbs.PKs[i].ColumnName, dr[tbs.PKs[i].ColumnName]);
                }
            }

            sqlcmd = string.Format("update {0} set {1} = '{2}'", _tableNm, _columnNm, editBox1.Text) + sqlwhere;
            if (!(result = DBProxy.Current.Execute(null, sqlcmd)))
                ShowErr(sqlcmd, result);
            else
                this.Close();
        }
    }
}
