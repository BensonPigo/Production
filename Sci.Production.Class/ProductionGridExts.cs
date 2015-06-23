using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sci.Win.UI;
using Sci.Data;
using Ict.Win;
using Ict;
using Sci;

namespace Sci
{
    public static class ProductionGridExts
    {
        //Clog Location
        public static IDataGridViewGenerator CellClogLocation(this IDataGridViewGenerator gen, string propertyname, string header, IWidth width = null, DataGridViewGeneratorTextColumnSettings settings = null, bool? iseditable = null, bool? iseditingreadonly = null, DataGridViewContentAlignment? alignment = null)
        {
            if (settings == null) settings = new DataGridViewGeneratorTextColumnSettings();
            settings.CharacterCasing = CharacterCasing.Upper;
            settings.EditingMouseDown += (s, e) =>
            {
                Sci.Win.UI.Grid g = (Sci.Win.UI.Grid) ((DataGridViewColumn) s).DataGridView;
                Sci.Win.Forms.Base frm = (Sci.Win.Forms.Base)g.FindForm();
                if (frm.EditMode)
                {
                    if (e.Button == System.Windows.Forms.MouseButtons.Right)
                    {
                        if (e.RowIndex != -1)
                        {
                            DataRow dr = g.GetDataRow<DataRow>(e.RowIndex);
                            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem("select ID,Description from ClogLocation order by ID", "10,40", dr["ClogLocationId"].ToString().Trim());
                            DialogResult returnResult = item.ShowDialog();
                            if (returnResult == DialogResult.Cancel) { return; }
                            dr["ClogLocationId"] = item.GetSelectedString();
                        }
                    }
                }
            };
            settings.CellValidating += (s,e) =>
            {
                Sci.Win.UI.Grid g = (Sci.Win.UI.Grid)((DataGridViewColumn)s).DataGridView;
                Sci.Win.Forms.Base frm = (Sci.Win.Forms.Base)g.FindForm();
                if (frm.EditMode)
                {
                    DataRow dr = g.GetDataRow<DataRow>(e.RowIndex);
                    if (!myUtility.Empty(e.FormattedValue.ToString()))
                    {
                        if (myUtility.Seek(e.FormattedValue.ToString(), "ClogLocation", "id") == false)
                        {
                            MessageBox.Show(string.Format("< ClogLocation : {0} > not found!!!", e.FormattedValue.ToString()));
                            dr["ClogLocationId"] = DBNull.Value;
                            e.Cancel = true;
                            return;
                        }
                    }
                }
            };
            return gen.Text(propertyname, header: header, width: width, settings: settings, iseditable: iseditable, iseditingreadonly: iseditingreadonly, alignment: alignment);
        }

        // order id
        public static IDataGridViewGenerator CellOrderId(this IDataGridViewGenerator gen, string propertyname
                                                                , string header, IWidth width = null
                                                                , DataGridViewGeneratorTextColumnSettings settings = null
                                                                , bool? iseditable = null, bool? iseditingreadonly = null
                                                                , DataGridViewContentAlignment? alignment = null)
        {
            if (settings == null) settings = new DataGridViewGeneratorTextColumnSettings();
            settings.CharacterCasing = CharacterCasing.Upper;
            
            settings.CellValidating += (s, e) =>
            {
                Sci.Win.UI.Grid g = (Sci.Win.UI.Grid)((DataGridViewColumn)s).DataGridView;
                Sci.Win.Forms.Base frm = (Sci.Win.Forms.Base)g.FindForm();
                if (frm.EditMode)
                {
                    DataRow dr = g.GetDataRow<DataRow>(e.RowIndex);
                    if (!myUtility.Empty(e.FormattedValue.ToString()))
                    {
                        if (myUtility.Seek(e.FormattedValue.ToString(), "Orders", "id") == false)
                        {
                            MessageBox.Show(string.Format("< Order Id : {0} > is not found!!!", e.FormattedValue.ToString()));
                            dr["orderid"] = DBNull.Value;
                            e.Cancel = true;
                            return;
                        }
                    }
                }
            };
            return gen.Text(propertyname, header: header, width: width, settings: settings, iseditable: iseditable, iseditingreadonly: iseditingreadonly, alignment: alignment);
        }

        //LocalItem: Carton
        public static IDataGridViewGenerator CellCartonItem(this IDataGridViewGenerator gen, string propertyname, string header, IWidth width = null, DataGridViewGeneratorTextColumnSettings settings = null, bool? iseditable = null, bool? iseditingreadonly = null, DataGridViewContentAlignment? alignment = null)
        {
            if (settings == null) settings = new DataGridViewGeneratorTextColumnSettings();
            settings.CharacterCasing = CharacterCasing.Upper;
            settings.EditingMouseDown += (s, e) =>
            {
                Sci.Win.UI.Grid g = (Sci.Win.UI.Grid)((DataGridViewColumn)s).DataGridView;
                Sci.Win.Forms.Base frm = (Sci.Win.Forms.Base)g.FindForm();
                if (frm.EditMode)
                {
                    if (e.Button == System.Windows.Forms.MouseButtons.Right)
                    {
                        if (e.RowIndex != -1)
                        {
                            DataRow dr = g.GetDataRow<DataRow>(e.RowIndex);
                            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem("select RefNo,Description,STR(CtnLength,8,4)+'*'+STR(CtnWidth,8,4)+'*'+STR(CtnHeight,8,4) as Dim from LocalItem where Category = 'CARTON' and Junk = 0 order by RefNo", "10,25,25", dr["RefNo"].ToString().Trim());
                            DialogResult returnResult = item.ShowDialog();
                            if (returnResult == DialogResult.Cancel) { return; }
                            dr["RefNo"] = item.GetSelectedString();
                        }
                    }
                }
            };
            settings.CellValidating += (s, e) =>
            {
                Sci.Win.UI.Grid g = (Sci.Win.UI.Grid)((DataGridViewColumn)s).DataGridView;
                Sci.Win.Forms.Base frm = (Sci.Win.Forms.Base)g.FindForm();
                if (frm.EditMode)
                {
                    DataRow dr = g.GetDataRow<DataRow>(e.RowIndex);
                    if (!myUtility.Empty(e.FormattedValue.ToString()))
                    {
                        string seekSql = string.Format("select RefNo from LocalItem where Category = 'CARTON' and Junk = 0 and RefNo = '{0}'", e.FormattedValue.ToString());
                        if (myUtility.Seek(seekSql) == false)
                        {
                            MessageBox.Show(string.Format("< Ref No. : {0} > not found!!!", e.FormattedValue.ToString()));
                            dr["RefNo"] = DBNull.Value;
                            e.Cancel = true;
                            return;
                        }
                    }
                }
            };
            return gen.Text(propertyname, header: header, width: width, settings: settings, iseditable: iseditable, iseditingreadonly: iseditingreadonly, alignment: alignment);
        }
    }
}
