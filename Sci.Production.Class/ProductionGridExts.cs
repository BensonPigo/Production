﻿using System;
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
                            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem("select ID,Description from ClogLocation group by ID,Description order by ID", "10,40", dr["ClogLocationId"].ToString().Trim());
                            DialogResult returnResult = item.ShowDialog();
                            if (returnResult == DialogResult.Cancel) { return; }
                            e.EditingControl.Text = item.GetSelectedString();
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
                    if (!MyUtility.Check.Empty(e.FormattedValue.ToString()))
                    {
                        if (MyUtility.Check.Seek(e.FormattedValue.ToString(), "ClogLocation", "id") == false)
                        {
                            MyUtility.Msg.WarningBox(string.Format("< ClogLocation : {0} > not found!!!", e.FormattedValue.ToString()));
                            dr["ClogLocationId"] = "";
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
                    if (!MyUtility.Check.Empty(e.FormattedValue.ToString()))
                    {
                        if (MyUtility.Check.Seek(e.FormattedValue.ToString(), "Orders", "id") == false)
                        {
                            MyUtility.Msg.WarningBox(string.Format("< Order Id : {0} > is not found!!!", e.FormattedValue.ToString()));
                            dr["orderid"] = "";
                            e.Cancel = true;
                            return;
                        }
                    }
                }
            };
            return gen.Text(propertyname, header: header, width: width, settings: settings, iseditable: iseditable, iseditingreadonly: iseditingreadonly, alignment: alignment);
        }

        // POID with seq , roll ,dyelot
        public static IDataGridViewGenerator CellPOIDWithSeqRollDyelot(this IDataGridViewGenerator gen, string propertyname
                                                                , string header, IWidth width = null
                                                                , DataGridViewGeneratorTextColumnSettings settings = null
                                                                , bool? iseditable = null, bool? iseditingreadonly = null
                                                                , DataGridViewContentAlignment? alignment = null
                                                                ,bool CheckMDivisionID=false)
        {
            if (settings == null) settings = new DataGridViewGeneratorTextColumnSettings();
            settings.CharacterCasing = CharacterCasing.Upper;

            settings.CellValidating += (s, e) =>
            {
                Sci.Win.UI.Grid g = (Sci.Win.UI.Grid)((DataGridViewColumn)s).DataGridView;
                Sci.Win.Forms.Base frm = (Sci.Win.Forms.Base)g.FindForm();
                DataRow dr = g.GetDataRow<DataRow>(e.RowIndex);
                if (frm.EditMode && String.Compare(dr["poid"].ToString(),e.FormattedValue.ToString())!=0)
                {
                    if (!MyUtility.Check.Empty(e.FormattedValue.ToString()))
                    {
                        if (CheckMDivisionID)
                        {
                            if (!MyUtility.Check.Seek(string.Format("select * from dbo.orders inner join dbo.factory on orders.FtyGroup=factory.id where orders.ID='{0}' and factory.MDivisionID='{1}'", e.FormattedValue.ToString(),Sci.Env.User.Keyword), null))
                            {
                                MyUtility.Msg.WarningBox(string.Format("< Order Id : {0} > is not found!!!", e.FormattedValue.ToString()));
                                dr["poid"] = "";
                                e.Cancel = true;
                                return;
                            }
                            else
                            {
                                dr["poid"] = e.FormattedValue;
                                dr["seq"] = "";
                                dr["seq1"] = "";
                                dr["seq2"] = "";
                                dr["roll"] = "";
                                dr["dyelot"] = "";
                            }

                        }
                        else
                        {
                            if (MyUtility.Check.Seek(e.FormattedValue.ToString(), "Orders", "id") == false)
                            {
                                MyUtility.Msg.WarningBox(string.Format("< Order Id : {0} > is not found!!!", e.FormattedValue.ToString()));
                                dr["poid"] = "";
                                e.Cancel = true;
                                return;
                            }
                            else
                            {
                                dr["poid"] = e.FormattedValue;
                                dr["seq"] = "";
                                dr["seq1"] = "";
                                dr["seq2"] = "";
                                dr["roll"] = "";
                                dr["dyelot"] = "";
                            }
                        }
                    }
                    else
                    {
                        dr["poid"] = "";
                        dr["seq"] = "";
                        dr["seq1"] = "";
                        dr["seq2"] = "";
                        dr["roll"] = "";
                        dr["dyelot"] = "";
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
            settings.EditingMouseDown = new EventHandler<Ict.Win.UI.DataGridViewEditingControlMouseEventArgs>(CartonRefnoCommon.EditingMouseDown);
            
            settings.CellValidating += (s, e) =>
            {
                Sci.Win.UI.Grid g = (Sci.Win.UI.Grid)((DataGridViewColumn)s).DataGridView;
                Sci.Win.Forms.Base frm = (Sci.Win.Forms.Base)g.FindForm();
                if (frm.EditMode)
                {
                    DataRow dr = g.GetDataRow<DataRow>(e.RowIndex);
                    if (!MyUtility.Check.Empty(e.FormattedValue.ToString()))
                    {
                        string seekSql = string.Format("select RefNo from LocalItem where Category = 'CARTON' and Junk = 0 and RefNo = '{0}'", e.FormattedValue.ToString());
                        if (MyUtility.Check.Seek(seekSql) == false)
                        {
                            MyUtility.Msg.WarningBox(string.Format("< Ref No. : {0} > not found!!!", e.FormattedValue.ToString()));
                            dr["RefNo"] = "";
                            e.Cancel = true;
                            return;
                        }
                    }
                }
            };
            return gen.Text(propertyname, header: header, width: width, settings: settings, iseditable: iseditable, iseditingreadonly: iseditingreadonly, alignment: alignment);
        }
         //ThreadColor
        public static IDataGridViewGenerator CellThreadColor(this IDataGridViewGenerator gen, string propertyname, string header, IWidth width = null, DataGridViewGeneratorTextColumnSettings settings = null, bool? iseditable = null, bool? iseditingreadonly = null, DataGridViewContentAlignment? alignment = null)
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
                            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem("select ID,Description from ThreadColor where junk = 0 order by ID", "10,40", dr["ThreadColorid"].ToString().Trim());
                            DialogResult returnResult = item.ShowDialog();                           
                            if (returnResult == DialogResult.Cancel) { return; }
                            var sellist = item.GetSelecteds();
                            if (dr.Table.Columns.Contains("Colordesc")) dr["Colordesc"] = sellist[0][1];
                            e.EditingControl.Text = item.GetSelectedString();               
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
                    if (!MyUtility.Check.Empty(e.FormattedValue.ToString()))
                    {
                        string seekSql = string.Format("select id from ThreadColor where Junk = 0 and id = '{0}'", e.FormattedValue.ToString());
                        if (!MyUtility.Check.Seek(seekSql))
                        {
                            MyUtility.Msg.WarningBox(string.Format("< Thread Color : {0} > not found!!!", e.FormattedValue.ToString()));
                            dr["Threadcolorid"] = "";
                            e.Cancel = true;
                            return;
                        }
                    }
                }
            };
            return gen.Text(propertyname, header: header, width: width, settings: settings, iseditable: iseditable, iseditingreadonly: iseditingreadonly, alignment: alignment);
        }
        //ThreadLocation
        public static IDataGridViewGenerator CellThreadLocation(this IDataGridViewGenerator gen, string propertyname, string header, IWidth width = null, DataGridViewGeneratorTextColumnSettings settings = null, bool? iseditable = null, bool? iseditingreadonly = null, DataGridViewContentAlignment? alignment = null, bool? isChangeSelItem = null)
        {
            if (settings == null) settings = new DataGridViewGeneratorTextColumnSettings();
            string keyword = Sci.Env.User.Keyword;
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
                            string sql = "";
                            string strwidth = "10,40";
                            if (isChangeSelItem == true)
                            {
                                DataRow dd = g.GetDataRow<DataRow>(e.RowIndex);
                                string ddsql = string.Format("select 1 from ThreadStock where Refno='{0}' and ThreadColorID='{1}' and MDivisionid='{2}'", dd["Refno"].ToString(), dd["ThreadColorid"].ToString(), keyword);
                                if (MyUtility.Check.Seek(ddsql))
                                {
                                    sql = string.Format("select ID,Description,UsedCone,NewCone from ThreadLocation a left join ThreadStock b on a.id=b.ThreadLocationID  where  junk = 0 and Refno='{0}' and ThreadColorID='{1}' and a.MDivisionid='{2}' order by ID", dd["Refno"].ToString(), dd["ThreadColorid"].ToString(), keyword);
                                    strwidth = "8,18,10,10";
                                }
                                else {
                                    sql = string.Format("select ID,Description from ThreadLocation where mDivisionid = '{0}' and junk = 0 order by ID", keyword);
                                }
                            }
                            else {
                                sql = string.Format("select ID,Description from ThreadLocation where mDivisionid = '{0}' and junk = 0 order by ID", keyword);
                            }

                            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sql, strwidth, dr["ThreadLocationid"].ToString().Trim());
                            DialogResult returnResult = item.ShowDialog();
                            if (returnResult == DialogResult.Cancel) { return; }
                            e.EditingControl.Text = item.GetSelectedString();
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
                    if (!MyUtility.Check.Empty(e.FormattedValue.ToString()))
                    {
                        string seekSql = string.Format("select id from ThreadLocation where Junk = 0 and id = '{0}'", e.FormattedValue.ToString());
                        if (!MyUtility.Check.Seek(seekSql))
                        {
                            MyUtility.Msg.WarningBox(string.Format("< Thread Location : {0} > not found!!!", e.FormattedValue.ToString()));
                            dr["ThreadLocationid"] = "";
                            e.Cancel = true;
                            return;
                        }
                    }
                }
            };
            return gen.Text(propertyname, header: header, width: width, settings: settings, iseditable: iseditable, iseditingreadonly: iseditingreadonly, alignment: alignment);
        }
        //User LogID Name
        public static IDataGridViewGenerator CellUser(this IDataGridViewGenerator gen, string propertyname, string header, IWidth width = null, DataGridViewGeneratorTextColumnSettings settings = null, bool? iseditable = null, bool? iseditingreadonly = null, DataGridViewContentAlignment? alignment = null,string userNamePropertyName =null)
        {
            if (settings == null) settings = new DataGridViewGeneratorTextColumnSettings();
            string keyword = Sci.Env.User.Keyword;
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
                            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(string.Format("select ID,Name from Pass1 where id = '{0}' order by ID", keyword), "10,40", dr[propertyname].ToString().Trim());
                            DialogResult returnResult = item.ShowDialog();
                            if (returnResult == DialogResult.Cancel) { return; }
                            e.EditingControl.Text = item.GetSelectedString();
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
                    DataRow pass_dr;
                    if (!MyUtility.Check.Empty(e.FormattedValue.ToString()))
                    {
                        string seekSql = string.Format("select id,Name from Pass1 where id = '{0}'", e.FormattedValue.ToString());
                        if (!MyUtility.Check.Seek(seekSql, out pass_dr))
                        {
                            MyUtility.Msg.WarningBox(string.Format("< User ID : {0} > not found!!!", e.FormattedValue.ToString()));
                            dr[propertyname] = "";
                            e.Cancel = true;
                            return;
                        }
                        else
                        {
                            if (userNamePropertyName != null)
                            {
                                dr[userNamePropertyName] = pass_dr["Name"];
                                dr[propertyname] = e.FormattedValue.ToString();
                                dr.EndEdit();
                            }
                        }
                    }
                }
            };
            return gen.Text(propertyname, header: header, width: width, settings: settings, iseditable: iseditable, iseditingreadonly: iseditingreadonly, alignment: alignment);
        }
        //Scale
        public static IDataGridViewGenerator CellScale(this IDataGridViewGenerator gen, string propertyname, string header, IWidth width = null, DataGridViewGeneratorTextColumnSettings settings = null, bool? iseditable = null, bool? iseditingreadonly = null, DataGridViewContentAlignment? alignment = null)
        {
            if (settings == null) settings = new DataGridViewGeneratorTextColumnSettings();
            string keyword = Sci.Env.User.Keyword;
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
                            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem("select ID from Scale where junk = 0 order by ID", "10,40", dr["Scale"].ToString().Trim());
                            DialogResult returnResult = item.ShowDialog();
                            if (returnResult == DialogResult.Cancel) { return; }
                            e.EditingControl.Text = item.GetSelectedString();
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
                    if (!MyUtility.Check.Empty(e.FormattedValue.ToString()))
                    {
                        string seekSql = string.Format("select id from Scale where Junk = 0 and id = '{0}'", e.FormattedValue.ToString());
                        if (!MyUtility.Check.Seek(seekSql))
                        {
                            MyUtility.Msg.WarningBox(string.Format("< Scale : {0} > not found!!!", e.FormattedValue.ToString()));
                            dr["Scale"] = "";
                            e.Cancel = true;
                            return;
                        }
                    }
                }
            };
            return gen.Text(propertyname, header: header, width: width, settings: settings, iseditable: iseditable, iseditingreadonly: iseditingreadonly, alignment: alignment);
        }
    }

    public class CartonRefnoCommon
    {
        public static void EditingMouseDown(object sender, Ict.Win.UI.DataGridViewEditingControlMouseEventArgs e)
        {
            Sci.Win.UI.Grid g = (Sci.Win.UI.Grid)((DataGridViewColumn)sender).DataGridView;
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
                        e.EditingControl.Text = item.GetSelectedString();
                    }
                }
            }
        }
    }
}
