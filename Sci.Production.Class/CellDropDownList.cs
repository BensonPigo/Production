using Ict.Win;
using Sci.Data;
using System.Collections.Generic;
using System.Data;
using Ict;

namespace Sci.Production.Class
{
    /// <summary>
    /// CellDropDownList
    /// </summary>
    public class CellDropDownList : DataGridViewGeneratorComboBoxColumnSettings
    {
        /// <summary>
        /// GetGridCell
        /// </summary>
        /// <param name="type"> Type </param>
        /// <returns>DataGridViewGeneratorComboBoxColumnSettings</returns>
        public static DataGridViewGeneratorComboBoxColumnSettings GetGridCell(string type)
        {
            CellDropDownList cellcb = new CellDropDownList();
            if (!Env.DesignTime)
            {
                string selectCommand = string.Format(
                    @"
select ID
       , Name = rtrim(Name)
from DropDownList WITH (NOLOCK) 
where Type = '{0}' 
order by Seq", type);
                DualResult returnResult;
                DataTable dropDownListTable = new DataTable();
                Dictionary<string, string> di_dropdown = new Dictionary<string, string>();
                if (returnResult = DBProxy.Current.Select(null, selectCommand, out dropDownListTable))
                {
                    cellcb.DataSource = dropDownListTable;
                    cellcb.DisplayMember = "Name";
                    cellcb.ValueMember = "ID";
                }
            }

            return cellcb;
        }
    }
}
