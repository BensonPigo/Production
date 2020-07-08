using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using Sci.Win.UI;
using System.Windows.Forms;
using Ict;
using Sci.Win.Tools;

namespace Sci.Production.Class
{
    public partial class comboDropDownList : Sci.Win.UI.ComboBox
    {
        private string type;

        [Category("Custom Properties")]
        public string Type
        {
            get { return this.type; }

            set
            {
                this.type = value;
                if (!Env.DesignTime)
                {
                    string selectCommand = string.Format(
                        @"
select ID
       , Name = rtrim(Name)
from DropDownList WITH (NOLOCK) 
where Type = '{0}' 
order by Seq", this.Type);
                    Ict.DualResult returnResult;
                    DataTable dropDownListTable = new DataTable();
                    if (returnResult = DBProxy.Current.Select(null, selectCommand, out dropDownListTable))
                    {
                        this.DataSource = dropDownListTable;
                        this.DisplayMember = "Name";
                        this.ValueMember = "ID";
                    }
                }
            }
        }

        public comboDropDownList()
        {
            this.InitializeComponent();
        }

        public comboDropDownList(System.ComponentModel.IContainer container)
        {
            container.Add(this);

            this.InitializeComponent();
        }
    }

    public class cellDropDownList : DataGridViewGeneratorComboBoxColumnSettings
    {
        public static DataGridViewGeneratorComboBoxColumnSettings GetGridCell(string Type)
        {
            cellDropDownList cellcb = new cellDropDownList();
            if (!Env.DesignTime)
            {
                string selectCommand = string.Format(
                    @"
select ID
       , Name = rtrim(Name)
from DropDownList WITH (NOLOCK) 
where Type = '{0}' 
order by Seq", Type);
                Ict.DualResult returnResult;
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

    public partial class txtDropDownList : Sci.Win.UI.TextBox
    {
        private string type;

        [Category("Custom Properties")]
        public string Type
        {
            get { return this.type; }

            set
            {
                this.type = value;
            }
        }

        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            base.OnPopUp(e);

            #region SQL CMD
            string sqlcmd = $@"
select ID
       , Name = rtrim(Name)
from DropDownList WITH (NOLOCK) 
where Type = '{this.type}' 
order by Seq";
            #endregion
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlcmd, "ID,Name", this.Text, false, null);
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.Text = item.GetSelectedString();
            this.ValidateText();
        }

        protected override void OnValidating(CancelEventArgs e)
        {
            base.OnValidating(e);
            string str = this.Text;
            #region SQL CMD
            string sqlcmd = $@"
select ID
       , Name = rtrim(Name)
from DropDownList WITH (NOLOCK) 
where Type = '{this.type}' 
and id ='{str}'
order by Seq";
            #endregion
            if (!string.IsNullOrWhiteSpace(str) && str != this.OldValue)
            {
                if (MyUtility.Check.Seek(sqlcmd) == false)
                {
                    this.Text = string.Empty;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("< Return TO : {0} > not found!!!", str));
                    return;
                }
            }
        }
    }

    public class cellTextDropDownList : DataGridViewGeneratorTextColumnSettings
    {
        public static DataGridViewGeneratorTextColumnSettings GetGridCell(string ctype)
        {
            cellTextDropDownList ts = new cellTextDropDownList();

            // Factory右鍵彈出功能
            ts.EditingMouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    System.Windows.Forms.DataGridView grid = ((DataGridViewColumn)s).DataGridView;

                    // Parent form 若是非編輯狀態就 return
                    if (!((Sci.Win.Forms.Base)grid.FindForm()).EditMode)
                    {
                        return;
                    }

                    DataRow row = grid.GetDataRow<DataRow>(e.RowIndex);

                    string Shiftcolname = grid.Columns.Cast<DataGridViewColumn>().Where(o => o.Name.Contains("Shift")).FirstOrDefault().Name;
                    string NewShiftcolname = grid.Columns.Cast<DataGridViewColumn>().Where(o => o.Name.Contains("NewShift")).FirstOrDefault() == null ?
                                            string.Empty : grid.Columns.Cast<DataGridViewColumn>().Where(o => o.Name.Contains("NewShift")).FirstOrDefault().Name;

                    string colName = string.IsNullOrEmpty(NewShiftcolname) ? Shiftcolname : NewShiftcolname;

                    #region SQL CMD
                    string sqlcmd = $@"
select ID
       , Name = rtrim(Name)
from DropDownList WITH (NOLOCK) 
where Type = '{ctype}' 
order by Seq";
                    #endregion

                    SelectItem sele = new SelectItem(sqlcmd, "10,40", row[colName].ToString(), false, ",");
                    DialogResult result = sele.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    e.EditingControl.Text = sele.GetSelectedString();
                }
            };

            // 正確性檢查
            ts.CellValidating += (s, e) =>
            {
                System.Windows.Forms.DataGridView grid = ((DataGridViewColumn)s).DataGridView;

                // Parent form 若是非編輯狀態就 return
                if (!((Sci.Win.Forms.Base)grid.FindForm()).EditMode)
                {
                    return;
                }

                DataRow row = grid.GetDataRow<DataRow>(e.RowIndex);

                // Cutting P03 使用時，預設值是NewShift，但其他地方沒有NewShift這個欄位名稱，因此動態抓不能寫死
                string Shiftcolname = grid.Columns.Cast<DataGridViewColumn>().Where(o => o.Name.Contains("Shift")).FirstOrDefault().Name;
                string NewShiftcolname = grid.Columns.Cast<DataGridViewColumn>().Where(o => o.Name.Contains("NewShift")).FirstOrDefault() == null ?
                                            string.Empty : grid.Columns.Cast<DataGridViewColumn>().Where(o => o.Name.Contains("NewShift")).FirstOrDefault().Name;

                string colName = string.IsNullOrEmpty(NewShiftcolname) ? Shiftcolname : NewShiftcolname;

                String oldValue = row[colName].ToString();
                String newValue = e.FormattedValue.ToString(); // user 編輯當下的value , 此值尚未存入DataRow

                if (!MyUtility.Check.Empty(newValue) && oldValue != newValue)
                {
                    #region SQL CMD
                    string sqlcmd = $@"
select ID
       , Name = rtrim(Name)
from DropDownList WITH (NOLOCK) 
where Type = '{ctype}' 
and id ='{newValue}'
order by Seq";
                    #endregion

                    if (!MyUtility.Check.Seek(sqlcmd))
                    {
                        row[colName] = string.Empty;
                        row.EndEdit();
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox(string.Format("< Return TO : {0} > not found!!!", newValue));
                        return;
                    }
                }
            };
            return ts;
        }
    }
}
