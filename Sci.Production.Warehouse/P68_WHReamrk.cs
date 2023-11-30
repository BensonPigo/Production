using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P68_WHReamrk : Win.Tems.QueryForm
    {
        private DataRow dr;

        /// <inheritdoc/>
        public P68_WHReamrk(DataRow dr)
        {
            this.InitializeComponent();
            this.dr = dr;
            this.Query();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void Query()
        {
            string sqlcmd = $@"
SELECT DISTINCT iss.Remark,RemarkEditDate,RemarkEditName
FROM issue i
INNER JOIN Issue_Summary iss ON iss.Id = i.ID
WHERE i.CutplanID = '{this.dr["ID"]}'
AND iss.POID = '{this.dr["POID"]}'
AND iss.SCIRefno = '{this.dr["SCIRefno"]}'
AND iss.Colorid = '{this.dr["Color"]}'
AND iss.Remark <> ''
";
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out DataTable dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            List<string> list = new List<string>();
            foreach (DataRow dr in dt.Rows)
            {
                string remarkEditName = MyUtility.Convert.GetString(dr["RemarkEditName"]);
                string remarkEditDate = MyUtility.Check.Empty(dr["RemarkEditDate"]) ? string.Empty : ((DateTime)dr["remarkEditDate"]).ToString("yyyy/MM/dd HH:mm:ss");
                string remark = MyUtility.Convert.GetString(dr["Remark"]);
                list.Add("[" + remarkEditName + " " + remarkEditDate + "]" + " " + remark);
            }

            this.displayBox1.Text = list.JoinToString(",\r\n");
        }
    }
}